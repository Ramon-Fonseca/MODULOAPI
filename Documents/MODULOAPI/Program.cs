using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Adiciona serviços de controllers
builder.Services.AddControllers();

// 🔹 Configura HTTPS
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 7199; // porta HTTPS definida no launchSettings.json
});

// 🔹 Adiciona Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🔹 Configura o pipeline HTTP
app.UseRouting();

// 🔹 Configura CORS
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// 🔹 Configura Swagger e SwaggerUI apenas no ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MODULOAPI v1");
        c.RoutePrefix = "swagger"; // você acessa em http://localhost:5103/swagger
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

// 🔹 Mapeia os controllers
app.MapControllers();

app.Run();
