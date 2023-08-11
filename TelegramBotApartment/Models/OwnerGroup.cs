
namespace TelegramBotApartment.Models;

public class OwnerGroup
{
    public Guid Id { get; set; }    
    public long OwnerId { get; set; }
    public long GroupId { get; set; } 
}
