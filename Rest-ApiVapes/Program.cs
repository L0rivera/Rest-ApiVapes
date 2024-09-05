using Rest_ApiVapes.Data;
using Rest_ApiVapes.utils;

// Import the required packages
//==============================
using CloudinaryDotNet;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<UserData>();
builder.Services.AddScoped<VapeData>();
builder.Services.AddScoped<CloudinaryService>();

// Cloudinary
builder.Services.AddSingleton(new Cloudinary(new Account(
    "dyvpzazpi",
    "681921567519245",
    "foErK5jshYuaBs0YPuy0lHNb9I4")));

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirCualquierOrigen", builder =>
    {
        builder
            .WithOrigins("http://localhost:3000") // Agrega aquí los orígenes permitidos
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Esto permite enviar cookies/credenciales
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("PermitirCualquierOrigen");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.Run();


