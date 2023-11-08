using AspNetCore.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(); // добавл€ем сервисы CORS


// ѕолучаем строку подключени€ из файла конфигурации
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// добавл€ем контекст ApplicationContext в качестве сервиса в приложение
builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(connection));
builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

var app = builder.Build();
// настраиваем CORS
//app.UseCors(builder => builder.AllowAnyOrigin());

app.UseCors(builder => builder.WithOrigins("http://localhost:4200")//дл€ angular прилжени€ Galery
                            .AllowAnyHeader()
                            .AllowAnyMethod());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
