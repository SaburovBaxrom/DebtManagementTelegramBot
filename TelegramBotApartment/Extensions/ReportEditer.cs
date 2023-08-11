using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using TelegramBotApartment.Models;
using TelegramBotApartment.Services;

namespace TelegramBotApartment.Extensions
{
    public static class ReportEditer
    {
        private static IReportService reportService;
      
        public static string EditReport(List<Report> reports)
        {
            string result = "Shig'arilg'an aqshalar :\n";
            foreach (Report report in reports) 
            {
                result += $"[{report.Username}](tg://user?id={report.UserId})  -  {Math.Round(report.Sum/1000)}k\n" ;
            }

            return result;
        }

        public static string OverAllReport(List<Report> reports,decimal sum, Dictionary<long, string> cards)
        {
            string result = $"Ortaliq {sum} sum boldi\n\n";
            decimal averageSum = sum / reports.Count();
            result += "Aqsha aliwi kerek:\n";
            foreach (Report report in reports)
            {
                if ((averageSum - report.Sum) < 0) 
                {
                    Console.WriteLine(cards[report.UserId]);
                    result += $"[{report.Username}](tg://user?id={report.UserId})  (`{cards[report.UserId]}`)- " +
                        ((averageSum - report.Sum) < 0 ? Math.Round((averageSum - report.Sum) * (-1) / 1000) : Math.Round((averageSum - report.Sum) / 1000)) +
                        ((averageSum - report.Sum) > 0 ? "k  beriwi kerek\n" : "k  aliwi kerek\n");
                }
            }
            result += "\nAqsha beriwi kerek:\n";
            foreach (Report report in reports)
            {
                if ((averageSum - report.Sum) > 0)
                {
                    result += $"[{report.Username}](tg://user?id={report.UserId}) - " +
                        ((averageSum - report.Sum) < 0 ? Math.Round((averageSum - report.Sum) * (-1) / 1000) : Math.Round((averageSum - report.Sum) / 1000)) +
                        ((averageSum - report.Sum) > 0 ? "k  beriwi kerek\n" : "k  aliwi kerek\n");
                }
            }
            result += "\n";
            
            result += EditReport(reports);
            
            return result;
        }
    }
}
