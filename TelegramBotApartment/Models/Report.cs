using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotApartment.Models
{
    public class Report
    {
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public decimal Sum { get; set; }
        public string Username { get; set; }
        public long GroupId { get; set; }
    }
}
