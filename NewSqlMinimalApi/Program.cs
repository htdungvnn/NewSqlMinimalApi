using Microsoft.EntityFrameworkCore;
using NewSqlMinimalApi;

var builder = WebApplication.CreateBuilder(args);

// Add connection string here (update with your CockroachDB info)
var connectionString = builder.Configuration.GetConnectionString("CockroachDb");

// Register DbContext with Npgsql provider (PostgreSQL compatible)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CRUD endpoints:

// Get all products
app.MapGet("/products", async (AppDbContext db) =>
    await db.Products.ToListAsync());

// Get product by id
app.MapGet("/products/{id:int}", async (int id, AppDbContext db) =>
    await db.Products.FindAsync(id) is Product product ? Results.Ok(product) : Results.NotFound());

// Create new product
app.MapPost("/products", async (Product product, AppDbContext db) =>
{
    db.Products.Add(product);
    await db.SaveChangesAsync();
    return Results.Created($"/products/{product.Id}", product);
});

// Update product
app.MapPut("/products/{id:int}", async (int id, Product inputProduct, AppDbContext db) =>
{
    var product = await db.Products.FindAsync(id);
    if (product == null) return Results.NotFound();

    product.Name = inputProduct.Name;
    product.Price = inputProduct.Price;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Delete product
app.MapDelete("/products/{id:int}", async (int id, AppDbContext db) =>
{
    var product = await db.Products.FindAsync(id);
    if (product == null) return Results.NotFound();

    db.Products.Remove(product);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();