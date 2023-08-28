using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
using TesteConhecimentos.Data;
using TesteConhecimentos.Models;
using TesteConhecimentos.Services;

namespace TesteConhecimentos.Pages.Fornecedor;

public class Edit : PageModel
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpClientFactory _clientFactory;

    public Edit(AppDbContext dbContext, IHttpClientFactory clientFactory)
    {
        _dbContext = dbContext;
        _clientFactory = clientFactory;
    }

    [BindProperty] public Models.Fornecedor? Fornecedor { get; set; }


    public async Task OnGet(int id)
    {
        try
        {
            Fornecedor = await _dbContext.Fornecedores.FindAsync(id);
        }
        catch (MySqlException)
        {
            ModelState.AddModelError("DatabaseError", "Ocorreu um erro ao obter dados.");
        }
    }

    public async Task<IActionResult> OnPostSave()
    {
        if (!CnpjValidator.IsValid(Fornecedor.Cnpj))
        {
            ModelState.AddModelError("ValidationError", "Cnpj não é válido");
        }


        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var fornecedor = await _dbContext.Fornecedores.FindAsync(Fornecedor.Id);


            if (fornecedor is not null)
            {
                fornecedor.Especialidade = Fornecedor.Especialidade;
                fornecedor.Cnpj = Fornecedor.Cnpj;
                fornecedor.Nome = Fornecedor.Nome;


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

                fornecedor.Cep = Fornecedor.Cep;
                fornecedor.Endereco = Fornecedor.Endereco;


                await _dbContext.SaveChangesAsync();


                return RedirectToPage("/Fornecedor/Index");
            }
        }
        catch (MySqlException)
        {
            ModelState.AddModelError("DatabaseError", "Ocorreu um erro ao salvar os dados no banco de dados.");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostDelete()
    {
        try
        {
            var fornecedor = await _dbContext.Fornecedores.FindAsync(Fornecedor.Id);
            if (fornecedor is null)
            {
                return Page();
            }

            _dbContext.Fornecedores.Remove(fornecedor);
            await _dbContext.SaveChangesAsync();
            return RedirectToPage("/Fornecedor/Index");
        }
        catch (MySqlException)
        {
            ModelState.AddModelError("DatabaseError", "Ocorreu um erro ao salvar os dados no banco de dados.");
        }

        return Page();
    }
}