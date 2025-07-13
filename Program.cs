using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManagementApiProject.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.AddSecurityDefinition("custom-auth", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Name = "X-User",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Cabe�alho X-User e X-Role para simular autentica��o",
        });

        options.OperationFilter<AddRequiredHeadersFilter>();
    });

// Autoriza��o com base em roles
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("GerenteOnly", policy =>
        policy.RequireRole("gerente"));
});

var app = builder.Build();

// -------------------------
// Middleware de autentica��o fake (para testes locais)
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
// Migrations autom�ticas (DEV)
// -------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS
app.UseHttpsRedirection();

// Autoriza��o
app.UseAuthorization();

app.MapControllers();

app.Run();
