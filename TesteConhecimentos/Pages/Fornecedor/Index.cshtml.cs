using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using TesteConhecimentos.Data;

namespace TesteConhecimentos.Pages.Fornecedor;

public class Index : PageModel
{
    private readonly AppDbContext _context;

    public Index(AppDbContext context)
    {
        _context = context;
    }

    public List<Models.Fornecedor>? Fornecedores { get; set; }

    public async Task OnGet()
    {
        try
        {
            Fornecedores = await _context.Fornecedores.ToListAsync();

        }
        catch (MySqlException)
        {
            ModelState.AddModelError("DatabaseError", "Ocorreu um erro ao obter dados.");
        }
    }
}