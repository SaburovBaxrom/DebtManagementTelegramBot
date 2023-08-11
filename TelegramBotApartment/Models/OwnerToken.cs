
namespace TelegramBotApartment.Models;

public class OwnerToken
{
    public Guid Id { get; set; }
    public long OwnerId { get; set; }
    public string Token { get; set; }
    public string GroupName { get; set; }
}
