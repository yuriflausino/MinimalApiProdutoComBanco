using Microsoft.EntityFrameworkCore;

public class ProdutoContext : DbContext
{
    public DbSet<Produto> Produtos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=produtos.db");
    }
}
