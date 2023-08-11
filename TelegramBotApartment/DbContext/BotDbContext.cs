
using Microsoft.EntityFrameworkCore;
using TelegramBotApartment.Models;

namespace TelegramBotApartment.DbContext;

public class BotDbContext : Microsoft.EntityFrameworkCore.DbContext
{

	
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string path = "server=(localdb)\\mssqllocaldb; database=TelegramBotDb; Trusted_connection = true; TrustServerCertificate = true";
        optionsBuilder.UseSqlServer(path);
    }
    public DbSet<OwnerGroup> OwnerGroup { get; set; }
	public DbSet<OwnerToken> OwnerToken { get; set; }
	public DbSet<UserToken> UserToken { get; set; }
    public DbSet<Report> Reports { get; set; }
}
