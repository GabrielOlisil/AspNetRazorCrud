using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR.Protocol;
using TesteConhecimentos.Data;
using TesteConhecimentos.Models;
using System.Net.Http.Json;
using System.Runtime.InteropServices.ComTypes;
using MySqlConnector;
using TesteConhecimentos.Services;

namespace TesteConhecimentos.Pages.Fornecedor;

public class Create : PageModel
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpClientFactory _clientFactory;

    public Create(AppDbContext dbContext, IHttpClientFactory clientFactory)
    {
        _dbContext = dbContext;
        _clientFactory = clientFactory;
    }

    [BindProperty] public Models.Fornecedor Fornecedor { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {

        
        
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        if (!CnpjValidator.IsValid(Fornecedor.Cnpj))
        {
            ModelState.AddModelError("ValidationError", "Cnpj não é válido");
            return Page();
        }
        
        var cep = Fornecedor.Cep;

        var client = _clientFactory.CreateClient("ViaCep");
        var response = await client.GetAsync($"{cep}/json");
        
        
        if (response.IsSuccessStatusCode)
        {
            var enderecoResponse = await response.Content.ReadFromJsonAsync<EnderecoApiModel>();

            if (enderecoResponse.Erro is not null)
            {
                ModelState.AddModelError("ValidationError", "Cep não existe");

                return Page();
            }
        }
        

        try
        {
        
            var fornecedor = new Models.Fornecedor()
            {
                Especialidade = Fornecedor.Especialidade,
                Cep = Fornecedor.Cep,
                Nome = Fornecedor.Nome,
                Cnpj = Fornecedor.Cnpj,
                Endereco = Fornecedor.Endereco
            };
        
            await _dbContext.Fornecedores.AddAsync(fornecedor);
            await _dbContext.SaveChangesAsync();
        
            return RedirectToPage("/Fornecedor/Index"); // Redirecionar para a lista após a inserção
        }
        catch (MySqlException)
        {
            ModelState.AddModelError("DatabaseError", "Ocorreu um erro ao salvar os dados no banco de dados.");
        }


        return Page();
    }
}