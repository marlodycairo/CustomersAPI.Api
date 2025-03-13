using CustomersAPI.Api.Data;
using CustomersAPI.Api.Data.IRepository;
using CustomersAPI.Api.Middleware;
using CustomersAPI.Api.Services;
using CustomersAPI.Api.Services.IServices;
using CustomersAPI.Api.Utils;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddDbContextPool<AppDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionDB"));
//});

//// Opción 1: Usar DbContextFactory (NO POOLING)
//builder.Services.AddDbContextFactory<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionDB"))
//);

//// Opción 2: Usar PooledDbContextFactory (CON POOLING)
//builder.Services.AddPooledDbContextFactory<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionDB")), poolSize: 5
//);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionDB"));
});

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductExternalApiService, ProductExternalApiService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IBasicOperationsService, BasicOperationsService>();
builder.Services.AddScoped<OperationsService>();
builder.Services.AddScoped(typeof(IUtilities<>), typeof(Utilities<>));

builder.Services.AddHttpClient();
builder.Services.AddControllers()
    .AddNewtonsoftJson();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

// Agregar servicios de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
