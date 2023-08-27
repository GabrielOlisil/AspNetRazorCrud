using Microsoft.EntityFrameworkCore;
using TesteConhecimentos.Models;

namespace TesteConhecimentos.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {
        
    }

    public DbSet<Fornecedor> Fornecedores { get; set; }
}