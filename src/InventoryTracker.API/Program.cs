using InventoryTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1) ��� Controller �� (��?����?� MVC)
builder.Services.AddControllers();

/// 2) ����� EF Core �� SQL Server �� ConnectionString �� appsettings.json
///    - AppDbContext �� ���� ���� �� DI ��?��� �?����.
///    - UseSqlServer: Provider ����� SQL Server.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

/// 3) Swagger ���? ��������? � ��� API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

/// 4) �������? Swagger ��� �� ��?� Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/// 5) �?��������� (Pipeline)
app.UseHttpsRedirection(); // ���?� �� HTTPS (ǐ� ����? dev ���?�)
app.UseAuthorization();     // ����� ����� ��?� ����?� ��� �?�������� �� �?����?�.

/// 6) ��� Route ��? Controller ��
app.MapControllers();

/// 7) ����? ������
app.Run();
