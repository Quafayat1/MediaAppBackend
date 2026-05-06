using ASPBackendApp.Contracts;
using ASPBackendApp.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Initialize Cosmos DB service
try
{
    var dbService = builder.Configuration.GetSection("CosmosDb")
                            .InitializeCosmosClientInstanceAsync()
                            .GetAwaiter()
                            .GetResult();

    builder.Services.AddSingleton<IDBService>(dbService);
}
catch (Exception ex)
{
    Console.WriteLine($"Cosmos DB initialization failed: {ex.Message}");
}

var app = builder.Build();

// Enable serving static files from wwwroot
app.UseStaticFiles(); // <-- Added this here, before UseRouting()

// Enable Swagger in dev only
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS and authorization
app.UseHttpsRedirection();
app.UseAuthorization();

app.UseRouting();

app.MapControllers();

app.Run();
