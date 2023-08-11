using Microsoft.EntityFrameworkCore;
using TelegramBotApartment.DbContext;
using TelegramBotApartment.Models;
using TelegramBotApartment.Models.Dto;

namespace TelegramBotApartment.Services
{
    public class OwnerTokenService : IOwnerTokenService
    {
        private BotDbContext _dbContext;
        private ReportService _report;

        public OwnerTokenService(BotDbContext botDbContext)
        {
            _dbContext = botDbContext;
            _report= new ReportService();
        }

        public async Task AddCard(long id, string cardNumber)
        {
            var userToken = await _dbContext.UserToken
                .Where(x => x.UserId == id)
                .FirstOrDefaultAsync();

            if (userToken != null) 
            { 
                userToken.CardNumber = cardNumber;
                Console.WriteLine("Card number added");
                _dbContext.SaveChanges();
            }
        }

        public async Task<bool> CheckingToken(string token)
        {
            var list = _dbContext.OwnerToken.Where(x => x.Token == token).ToList();
            if(list.Count > 0)
            {
                return true;
            }
            return false;
        }

        public async Task CreateOwnerGroup(OwnerGroup ownerGroup)
        {
            var owner = await _dbContext.OwnerGroup
                        .Where(x => x.OwnerId == ownerGroup.OwnerId)
                        .FirstOrDefaultAsync();
            if (owner != null)
            {
                Console.WriteLine("already exsits");
            }
            else
            {
                await _dbContext.OwnerGroup.AddAsync(ownerGroup);
                await _dbContext.SaveChangesAsync();
                Console.WriteLine("group and user connected");
            }
            
        }

        public async Task CreateOwnerToken(OwnerToken ownerToken)
        {

            await _dbContext.OwnerToken.AddAsync(ownerToken);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Dictionary<long,string>> GetAllCards(long groupId)
        {
            var groupMembers = _report.GroupMemebers(groupId).Result;

            var cards = new Dictionary<long, string>();
            foreach (var member in groupMembers)
            {
                var userToken = await _dbContext.UserToken
                    .Where(x => x.UserId == member)
                    .FirstOrDefaultAsync();

                if(userToken != null)
                {
                    cards[member] = (userToken).CardNumber;
                }
                else
                {
                    cards[member] = String.Empty;
                }
            }

            return cards;

            
        }

        public Task<OwnerToken> GetByToken(string token)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetCardNumber(long id)
        {
            var userToken = await _dbContext.UserToken
                .Where(x => x.UserId == id)
                .FirstOrDefaultAsync();
            if(userToken != null)
                return userToken.CardNumber;

            return String.Empty;
        }

        public async Task<OwnerGroup> GetOwnerGroupByOwnerId(long id)
        {
            return await _dbContext.OwnerGroup.Where(owner => owner.OwnerId == id).FirstOrDefaultAsync();
        }

        public async Task UserToken(UserToken userToken)
        {
            var userTokendb = await _dbContext.UserToken
                .Where(x => x.Token == userToken.Token && x.UserId == userToken.UserId)
                .FirstOrDefaultAsync();
            if (userTokendb == null)
            {
                await _dbContext.UserToken.AddAsync(userToken);
                await _dbContext.SaveChangesAsync();
            }
            
        }
    }
}
