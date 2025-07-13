using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManagementApiProject.Data;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// -------------------------
// Servi�os
// -------------------------

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("custom-auth", new OpenApiSecurityScheme
    {
        Name = "X-User",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "Cabe�alho X-User e X-Role para simular autentica��o",
    });

    options.OperationFilter<AddRequiredHeadersFilter>();
});

// Autoriza��o baseada em Roles
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("GerenteOnly", policy =>
        policy.RequireRole("gerente"));
});

var app = builder.Build();

// -------------------------
// Middleware de autentica��o fake
// -------------------------

app.Use(async (context, next) =>
{
    var username = context.Request.Headers["X-User"].ToString();
    var role = context.Request.Headers["X-Role"].ToString();

    if (!string.IsNullOrEmpty(username))
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, string.IsNullOrEmpty(role) ? "colaborador" : role)
        };
        var identity = new ClaimsIdentity(claims, "FakeAuth");
        context.User = new ClaimsPrincipal(identity);
    }

    await next();
});

// -------------------------
// Migrations autom�ticas
// -------------------------

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// -------------------------
// Swagger habilitado sempre
// -------------------------

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManager API v1");
    // c.RoutePrefix = string.Empty; // Ative isso se quiser acessar Swagger em "/"
});

// -------------------------
// HTTPS e rotas
// -------------------------

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// -------------------------
// For�a o app a escutar na porta 8080 (necess�rio no Docker)
// -------------------------

app.Urls.Add("http://+:8080");

app.Run();
