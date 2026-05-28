using Microsoft.OpenApi;
using Scalar.AspNetCore;
using System.Reflection;
using DotnetApi.Middleware;
using DotnetApi.Middlewares;
using DotnetApi.Data;
using Microsoft.EntityFrameworkCore;
using DotnetApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddLogging();
builder.Services.AddControllers();

// Database
//builder.Services.AddDbContext<AppDbContext>(options =>
//{
//    options.UseInMemoryDatabase("DotnetApi");
//});
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionnString = builder.Configuration.GetConnectionString("SqlServerConnection") ?? string.Empty;
    options.UseSqlServer(connectionnString);
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    options.AddSecurityDefinition("BasicAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        Description = "Autenticación básica con nombre de usuario y contraseña. Usa 'platzi' como usuario y '12345' como contraseña."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement() {
        [new OpenApiSecuritySchemeReference("BasicAuth",document)] = []
    });
});

var MyAllowOrigins = "MyAllowOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: MyAllowOrigins,
            policy =>
            {
                policy.AllowAnyHeader();
                policy.WithOrigins(builder.Configuration["AllowedHosts"] ?? String.Empty);
            }
            );
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskService, TaskService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowOrigins);

app.UseHttpsRedirection();

app.UseBasicAuth();

app.UseAuthorization();

// custom middleware

app.UseRequestLogging();

app.MapControllers();

app.Run();
