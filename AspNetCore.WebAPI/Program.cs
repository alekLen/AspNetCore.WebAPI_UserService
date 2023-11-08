using AspNetCore.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(); // ��������� ������� CORS


// �������� ������ ����������� �� ����� ������������
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// ��������� �������� ApplicationContext � �������� ������� � ����������
builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(connection));
builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

var app = builder.Build();
// ����������� CORS
//app.UseCors(builder => builder.AllowAnyOrigin());

app.UseCors(builder => builder.WithOrigins("http://localhost:4200")//��� angular ��������� Galery
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
