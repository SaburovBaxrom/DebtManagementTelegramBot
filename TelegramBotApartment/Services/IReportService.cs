
using TelegramBotApartment.Models;

namespace TelegramBotApartment.Services
{
    public interface IReportService
    {
        Task CreateReport(Report report);
        Task<List<Report>> GetAllReport(long groupId);
        Task<long> GetGroupIdByUserId(long userId);
        Task<decimal> Canculate(long groupId);
        Task<List<long>> GroupMemebers(long groupId);
    }
}
