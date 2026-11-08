using BBoxSoft.Application.Services.Universe;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Servicios
builder.Services.AddControllers();
builder.Services.AddOpenApi();  // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddScoped<UnvTenantService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
