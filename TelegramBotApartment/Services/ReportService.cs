
using Microsoft.EntityFrameworkCore;
using TelegramBotApartment.DbContext;
using TelegramBotApartment.Models;

namespace TelegramBotApartment.Services
{
    public class ReportService : IReportService
    {
        private BotDbContext _dbContext;
        public ReportService()
        {
            _dbContext = new BotDbContext();
        }
        public async Task CreateReport(Report report)
        {
            var exsitsReport = await _dbContext.Reports
                .Where(x => x.UserId == report.UserId && x.GroupId == report.GroupId)
                .FirstOrDefaultAsync();
            if(exsitsReport != null) 
            {
                exsitsReport.Sum += report.Sum;
                await _dbContext.SaveChangesAsync();
                Console.WriteLine("report added");
            }
            else
            {
                await _dbContext.Reports.AddAsync(report);
                await _dbContext.SaveChangesAsync();
                Console.WriteLine("report created");
            }
            
        }

        public async Task<List<Report>> GetAllReport(long groupId)
        {
            return await _dbContext.Reports
                .Where(x => x.GroupId == groupId)
                .ToListAsync();
        }

        public async Task<long> GetGroupIdByUserId(long userId)
        {
            var userToken = await _dbContext.UserToken
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            var token = userToken.Token;

            var ownerId = (await _dbContext.OwnerToken
                .Where(x => x.Token == token)
                .FirstOrDefaultAsync()).OwnerId;

            var groupId = (await _dbContext.OwnerGroup
                .Where(x => x.OwnerId == ownerId)
                .FirstOrDefaultAsync())
                .GroupId;
            
            return groupId;
        }
        public async Task<List<long>> GroupMemebers(long groupId)
        {
            var ownerId = (await _dbContext.OwnerGroup.Where(x => x.GroupId == groupId).FirstOrDefaultAsync()).OwnerId;

            var token = (await _dbContext.OwnerToken.Where(x => x.OwnerId == ownerId).FirstOrDefaultAsync()).Token;

            var users = await _dbContext.UserToken.Where(x => x.Token == token).Select(x1 => x1.UserId).ToListAsync();

            return users;
        }
        public async Task<decimal> Canculate(long groupId)
        {
            var usersId = await GroupMemebers(groupId);

            decimal sum = 0;

            foreach(var id in usersId) 
            {
                var report = await _dbContext.Reports.Where(x => x.UserId == id).FirstOrDefaultAsync();
                if(report != null)
                    sum += report.Sum;
            }

            return sum;
        }
        public async Task<int> CountMembers(long groupId)
        {
            var count = await GroupMemebers(groupId);
            return count.Count;
        }

    }
}
