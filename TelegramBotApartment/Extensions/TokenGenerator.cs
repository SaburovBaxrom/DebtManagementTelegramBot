using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotApartment.Extensions
{
    public class TokenGenerator : ITokenGenerator
    {
        public string GenerateToken()
        {
            string token = Guid.NewGuid().ToString() + Guid.NewGuid().ToString()[0..1];
            return token;
        }
    }
}
