using System.Reflection;
using System.Text.Json.Serialization;
using AssecoPraksa.Database;
using AssecoPraksa.Database.Repositories;
using AssecoPraksa.Services;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);


var myLocalHostPolicy = "MyCORSPolicy";

// postavljanje cors. Primaju se headeri samo sa localhost:4200 - angular server
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myLocalHostPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyHeader();
    });
});

// Add services to the container.
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

// get the current implementation of AutoMapper and add it to the application scope
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// sta ce nam ovo
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(
        new JsonStringEnumConverter(System.Text.Json.JsonNamingPolicy.CamelCase)
        );
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DBContext registration
builder.Services.AddDbContext<TransactionDbContext>(opt =>
{
    opt.UseNpgsql(CreateConnectionString(builder.Configuration));
});


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.GetService<IServiceScopeFactory>().CreateScope();
    scope.ServiceProvider.GetRequiredService<TransactionDbContext>().Database.Migrate();
}

app.UseAuthorization();

app.MapControllers();

app.UseCors(myLocalHostPolicy);

app.Run();

// kreiranje database connection stringa
// dohvataju se lokalni podaci iz properties/launchSettings.json
string CreateConnectionString(IConfiguration configuration)
{
    var username = Environment.GetEnvironmentVariable("DATABASE_USERNAME");
    var pass = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");
    var databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME") ?? "products";
    var host = Environment.GetEnvironmentVariable("DATABASE_HOST") ?? "localhost";
    var port = Environment.GetEnvironmentVariable("DATABASE_PORT") ?? "5432";

    var connBuilder = new NpgsqlConnectionStringBuilder
    {
        Host = host,
        Port = int.Parse(port),
        Username = username,
        Database = databaseName,
        Password = pass,
        Pooling = true
    };

    return connBuilder.ConnectionString;
}
