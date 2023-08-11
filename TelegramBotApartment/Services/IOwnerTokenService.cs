
using TelegramBotApartment.Models;

namespace TelegramBotApartment.Services
{
    public interface IOwnerTokenService
    {
        Task CreateOwnerToken(OwnerToken ownerToken);
        Task<OwnerToken> GetByToken(string token);
        Task<bool> CheckingToken(string token);
        Task UserToken(UserToken userToken);
        Task CreateOwnerGroup(OwnerGroup ownerGroup);
        Task<OwnerGroup> GetOwnerGroupByOwnerId(long id);
        Task AddCard(long id,string cardNumber);
        Task<string> GetCardNumber(long id);
        Task<Dictionary<long, string>> GetAllCards(long groupId);
    }
}
