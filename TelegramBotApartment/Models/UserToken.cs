using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotApartment.Models;

public class UserToken
{
    public Guid Id { get; set; }
    public long UserId { get; set; }
    public string Token { get; set; }
    public string CardNumber { get; set; }
}
