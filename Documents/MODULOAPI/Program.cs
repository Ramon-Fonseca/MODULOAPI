using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Adiciona serviços de controllers
builder.Services.AddControllers();

// 🔹 Configura CORS (política mais permissiva para desenvolvimento)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.SetIsOriginAllowed(origin => true) // Permite qualquer origem
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

// 🔹 Configura HTTPS
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 7199; // porta HTTPS definida no launchSettings.json
});

// 🔹 Adiciona Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🔹 Habilita CORS - deve ser um dos primeiros middlewares
app.UseCors();

// 🔹 Configura o pipeline HTTP
app.UseRouting();

// 🔹 Configura Swagger e SwaggerUI apenas no ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MODULOAPI v1");
        c.RoutePrefix = string.Empty; // Define a raiz como página inicial
        c.ConfigObject.AdditionalItems["persistAuthorization"] = true;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

// 🔹 Mapeia os controllers
app.MapControllers();

app.Run();
