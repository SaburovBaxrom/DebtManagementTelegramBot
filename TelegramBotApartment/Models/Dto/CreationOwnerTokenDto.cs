using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotApartment.Models.Dto
{
    public class CreationOwnerTokenDto
    {
        public long OwnerId { get; set; }
        public string Token { get; set; }
        public string GroupName { get; set; }
    }
}
