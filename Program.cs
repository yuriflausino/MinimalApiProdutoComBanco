using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adicionar o contexto do EF Core
builder.Services.AddDbContext<ProdutoContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Inicializar o banco de dados (apenas para garantir que o banco exista)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProdutoContext>();
    dbContext.Database.EnsureCreated();
}

// Rotas da API
app.MapGet("/produtos", async (ProdutoContext db) =>
{
    return await db.Produtos.ToListAsync();
});

app.MapGet("/produtos/{id}", async (int id, ProdutoContext db) =>
{
    return await db.Produtos.FindAsync(id) is Produto produto ? Results.Ok(produto) : Results.NotFound();
});

app.MapPost("/produtos", async (Produto produto, ProdutoContext db) =>
{
    db.Produtos.Add(produto);
    await db.SaveChangesAsync();
    return Results.Created($"/produtos/{produto.Id}", produto);
});

app.MapPut("/produtos/{id}", async (int id, Produto updatedProduto, ProdutoContext db) =>
{
    var produto = await db.Produtos.FindAsync(id);
    if (produto is null) return Results.NotFound();

    produto.Nome = updatedProduto.Nome;
    produto.Preco = updatedProduto.Preco;
    produto.Quantidade = updatedProduto.Quantidade;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/produtos/{id}", async (int id, ProdutoContext db) =>
{
    var produto = await db.Produtos.FindAsync(id);
    if (produto is null) return Results.NotFound();

    db.Produtos.Remove(produto);
    await db.SaveChangesAsync();
    return Results.Ok(produto);
});

app.Run();
