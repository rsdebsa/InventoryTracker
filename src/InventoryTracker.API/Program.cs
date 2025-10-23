using InventoryTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1) À»  Controller Â« (Å«?Åù·«?‰ MVC)
builder.Services.AddControllers();

/// 2) « ’«· EF Core »Â SQL Server »« ConnectionString «“ appsettings.json
///    - AppDbContext œ— “„«‰ «Ã—« «“ DI œ—?«›  „?ù‘Êœ.
///    - UseSqlServer: Provider „‰«”» SQL Server.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

/// 3) Swagger »—«? „” ‰œ”«“? Ê  ”  API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

/// 4) ›⁄«·ù”«“? Swagger ›ﬁÿ œ— „Õ?ÿ Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/// 5) „?«‰ù«›“«—Â« (Pipeline)
app.UseHttpsRedirection(); // Âœ«?  »Â HTTPS («ê— êÊ«Â? dev œ«—?œ)
app.UseAuthorization();     // ›⁄·« «Õ—«“ ÂÊ?  ‰œ«—?„° «„« ¬?‰œÂù‰ê—«‰Â ‰êÂ „?ùœ«—?„.

/// 6) À»  Route Â«? Controller Â«
app.MapControllers();

/// 7) «Ã—«? »—‰«„Â
app.Run();
