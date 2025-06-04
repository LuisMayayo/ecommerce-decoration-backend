using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using EcommerceBackend.Data;
using EcommerceBackend.Repositories;
using EcommerceBackend.Services;
using EcommerceBackend.Models;
using EcommerceBackend.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// 1) Configurar logging mejorado
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Configurar nivel de logging específico para EmailService
builder.Logging.AddFilter("EcommerceBackend.Services.EmailService", LogLevel.Debug);

// 2) Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins(
            "http://localhost:5173",
            "https://lm-decoraciones.retocsv.es"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

// 3) Configurar cadena de conexión y DbContext
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new ArgumentNullException("DefaultConnection", "Connection string is missing");

builder.Services.AddDbContext<EcommerceDbContext>(options =>
{
    options.UseSqlServer(connectionString);
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

// 4) Configurar autenticación JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new ArgumentNullException("Jwt:Key is missing"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            RoleClaimType = ClaimTypes.Role
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogWarning("Authentication failed: {Message}", context.Exception.Message);
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var payload = new
                {
                    error = "No estás autorizado. Asegúrate de enviar un token válido en el header Authorization."
                };

                return context.Response.WriteAsync(JsonSerializer.Serialize(payload));
            },
            OnForbidden = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";

                var payload = new
                {
                    error = "Acceso denegado. No tienes los permisos necesarios (rol)."
                };

                return context.Response.WriteAsync(JsonSerializer.Serialize(payload));
            }
        };
    });

builder.Services.AddAuthorization();

// 5) Registrar controladores y configurar Swagger
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Ecommerce API",
        Version = "v1",
        Description = "API de Ecommerce con autenticación JWT"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Introduce el token en el siguiente formato: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

// 6) Registrar repositorios y servicios
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IDetallePedidoRepository, DetallePedidoRepository>();
builder.Services.AddScoped<IReseñaRepository, ReseñaRepository>();
builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IDetallePedidoService, DetallePedidoService>();
builder.Services.AddScoped<IReseñaService, ReseñaService>();
builder.Services.AddScoped<IProveedorService, ProveedorService>();

builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

// 7) Registrar EmailSettings y EmailService
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();

// 8) Registrar JwtService
builder.Services.AddSingleton<JwtService>();

// 9) Configurar opciones HTTP
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

// 10) Construir la aplicación
var app = builder.Build();

// Configurar middleware de logging para debugging
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecommerce API v1");
        c.RoutePrefix = string.Empty;
    });
}
else
{
    // Para producción, mantener Swagger disponible pero con configuración segura
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecommerce API v1");
        c.RoutePrefix = "api-docs"; // Cambiar la ruta en producción
    });
}

// Middleware para logging de requests (solo en desarrollo)
if (app.Environment.IsDevelopment())
{
    app.Use(async (context, next) =>
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Request: {Method} {Path}", context.Request.Method, context.Request.Path);
        await next();
        logger.LogInformation("Response: {StatusCode}", context.Response.StatusCode);
    });
}

app.UseCors("AllowLocalhost");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Middleware global de manejo de errores
app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        var contextFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        
        if (contextFeature != null)
        {
            logger.LogError(contextFeature.Error, "Error no manejado: {Message}", contextFeature.Error.Message);
            
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            
            var response = new
            {
                error = app.Environment.IsDevelopment() 
                    ? contextFeature.Error.Message 
                    : "Error interno del servidor"
            };
            
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    });
});

app.MapControllers();

// Log de inicio
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Aplicación iniciada en entorno: {Environment}", app.Environment.EnvironmentName);

app.Run();