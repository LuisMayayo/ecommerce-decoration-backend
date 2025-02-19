var builder = WebApplication.CreateBuilder(args);

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:5173")  // Frontend URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Obtener la cadena de conexión desde appsettings.json
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registrar la cadena de conexión en el contenedor de dependencias
builder.Services.AddSingleton(connectionString);

// Servicios a la API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();  // Habilita Swagger

// Registro de dependencias de otros repositorios y servicios
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IReseñaRepository, ReseñaRepository>();
builder.Services.AddScoped<IDetallePedidoRepository, DetallePedidoRepository>();

builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IReseñaService, ReseñaService>();
builder.Services.AddScoped<IDetallePedidoService, DetallePedidoService>();

// Agregar las dependencias de las categorías
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();

var app = builder.Build();

// Swagger para desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();  // Habilita Swagger UI en el entorno de desarrollo
}

// Usar CORS antes de usar la autorización
app.UseCors("AllowLocalhost");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
