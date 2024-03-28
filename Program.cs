using DishesAPI.DbContexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//register the DbContext on the container, getting the
// connection string from appSettings
builder.Services.AddDbContext<DishesDbContext>(o => o.UseSqlite(
    builder.Configuration["ConnectionStrings:DishesDBConnectionString"]));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();



app.MapGet("/dishes", async (DishesDbContext dishesDbContext) =>
{
    return await dishesDbContext.Dishes.ToListAsync();
});

app.MapGet("/dishes/{dishId:guid}", async (DishesDbContext dishesDbContext,
    Guid dishId) =>
{
    return await dishesDbContext.Dishes
        .FirstOrDefaultAsync(d => d.Id ==dishId);
});

app.MapGet("/dishes/{dishName}", async (DishesDbContext dishesDbContext,
    string dishName) =>
{
    return await dishesDbContext.Dishes
        .FirstOrDefaultAsync(d => d.Name == dishName);
});

app.MapGet("/dishes/{dishId}/ingredients", async (DishesDbContext dishesDbContext,
    Guid dishId) =>
{
    return (await dishesDbContext.Dishes
    .Include(d => d.Ingredients)
        .FirstOrDefaultAsync(d => d.Id == dishId))?.Ingredients;
});

//recreate $ migrate the database on each run, for demo purposes
using (var serviceScope = app.Services.GetService<IServiceScopeFactory>
    ().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<DishesDbContext>();
    context.Database.EnsureDeleted();
    context.Database.Migrate();
}

app.Run();


