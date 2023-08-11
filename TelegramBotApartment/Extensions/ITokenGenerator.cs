using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotApartment.Extensions
{
    public interface ITokenGenerator
    {
        string GenerateToken();
    }
}
