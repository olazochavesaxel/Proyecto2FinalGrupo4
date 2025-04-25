using _00_DTO;
using CoreApp;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Configuration;
using WebAPI.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Configuración de settings desde appsettings.json
builder.Services.Configure<PayPalSettings>(builder.Configuration.GetSection("PayPal"));
builder.Services.Configure<AlpacaSettings>(builder.Configuration.GetSection("Alpaca"));

// Servicios de negocio
builder.Services.AddScoped<PagoManager>();
builder.Services.AddSingleton<AlpacaManager>();

// Servicios de MVC y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// Middleware
app.UseCors("AllowAllOrigins");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.WebRootPath, "uploads")),
    RequestPath = "/uploads"
});

