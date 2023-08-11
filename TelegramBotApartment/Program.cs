using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotApartment.Services;
using TelegramBotApartment.Models;
using TelegramBotApartment.Extensions;

class main
{
    private const string Token = "5864781877:AAExo-hgBnPNIQrF2uNW1LXEYnFlNTy8ETY";

    private static string callBackQuery = "";
    private static string lastMessage = "";

    private static OwnerTokenService _tokenService;
    private static TokenGenerator _tokenGenerator;
    private static ReportService _reportService;

    static readonly string groupId = "-1001920722205";
    static async Task Main(string[] args)
    {
        _tokenService = new OwnerTokenService(new TelegramBotApartment.DbContext.BotDbContext());
        _tokenGenerator= new TokenGenerator();
        _reportService = new ReportService();
        var botClient = new TelegramBotClient(Token);


        using var cts = new CancellationTokenSource();
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };
        botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
        );

        var me = await botClient.GetMeAsync();

        Console.WriteLine($"Start listening for @{me.Username}");
        Console.ReadLine();
        cts.Cancel();        
    }

    static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var type = update.Message.Type;
        if (type == MessageType.Text)
        {
            var Updatehandler =

                update switch
                {

                    { Message: { } } => OnMessageText(),
                    { CallbackQuery: { } } => OnCallbackQueryReceived()
                };

            await Updatehandler;
        }

        async Task OnMessageText()
        {

            Message message = update.Message;
            var chatId = update.Message.Chat.Id;
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
            {
                new KeyboardButton[] { "Create group" },
                new KeyboardButton[] { "Join group" },
                new KeyboardButton[] { "Information" },
            })
            {
                ResizeKeyboard = true
            };

            Console.WriteLine($"Received a '{message.Text}' id = '{chatId}' message in chat {message.Chat.Username}.");

            if (message.Text == "/start"  && chatId > 0)
            {
                await botClient.SendTextMessageAsync(
                 chatId: chatId,
                 text: "Hi its bot",
                 replyToMessageId: message.MessageId,
                 replyMarkup: replyKeyboardMarkup,
                 cancellationToken: cancellationToken);
            }
            if (message.Chat.Username == "prosta_baha" && message.Text.StartsWith("group:",StringComparison.OrdinalIgnoreCase))
            {
                
                Console.WriteLine($"* Message sent to group:{message.Text} from {message.Chat.Username}");
                await botClient.SendTextMessageAsync(
                 chatId: groupId,
                 text: $"{message.Text[6..]}",
                 cancellationToken: cancellationToken);
            }
            
            if (message.Text.Length >= 3 && lastMessage == "Join group" && message.Text !="/start" && chatId >0)
            {
                string token = message.Text;
                if (_tokenService.CheckingToken(token).Result)
                {
                    ReplyKeyboardMarkup replyMarkup = new(new[]
                    {
                        new KeyboardButton[] { "Add sum" },
                        new KeyboardButton[] { "/report" },
                    })
                    {
                        ResizeKeyboard = true
                    };
                    await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "you joined to group",
                    replyMarkup:replyMarkup,
                    cancellationToken: cancellationToken);
                    var userToken = new UserToken
                    {
                        UserId = chatId,
                        Token = token
                    };
                    _tokenService.UserToken(userToken).Wait();
                    lastMessage = "";
                }
                else
                {
                    await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "token wrong please try again",
                    cancellationToken: cancellationToken);
                    
                }

            }
            if (lastMessage == "/addcard" && message.Text.Length == 16)
            {
                _tokenService.AddCard(chatId, message.Text).Wait();
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Karta nomeri qosildi",
                    replyToMessageId: message.MessageId,
                    cancellationToken: cancellationToken);
                lastMessage = String.Empty;
            }
            if(lastMessage == "/addcard" && message.Text.Length != 16)
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Karta nomer qate kiritildi:",
                    replyToMessageId: message.MessageId,
                    cancellationToken: cancellationToken);
            }
            if (message.Text == "/addcard" && chatId>0)
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Karta nomerin'izdi kiritin' (bos orin taslaman') :",
                    replyToMessageId:message.MessageId,
                    cancellationToken: cancellationToken);

                lastMessage = "/addcard";
            }
            
            if (lastMessage == "Create group" && message.Text.Length >= 3)
            {
                var token = _tokenGenerator.GenerateToken();
                var ownerToken = new OwnerToken()
                {
                    OwnerId = chatId,
                    GroupName = message.Text,
                    Token = token
                };
                _tokenService.CreateOwnerToken(ownerToken).Wait();
                _tokenService.UserToken(new UserToken
                {
                    Token = token,
                    UserId = chatId
                }).Wait();
                lastMessage = "";
                Console.WriteLine("group created");
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Created✅",
                    cancellationToken: cancellationToken);
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    parseMode: ParseMode.Markdown,
                    text: $"`{ownerToken.Token}`",
                    cancellationToken: cancellationToken);
            } 
            if(message.Text == "Create group")
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Enter group name :",
                    cancellationToken: cancellationToken);
                lastMessage = "Create group";
            }
            if (message.Text == "Join group")
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Enter token please :",
                    cancellationToken: cancellationToken);
                lastMessage = "Join group";
            }
            if (message.Text == "Information")
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Some Information",
                    replyToMessageId: message.MessageId,
                    cancellationToken: cancellationToken);
            }
            if (message.Text == "/addsum@apartmentDebt_bot" || 
                message.Text == "/addsum@apartmentDebt_bot" || 
                message.Text == "/create@apartmentDebt_bot" ||
                message.Text == "/join@apartmentDebt_bot")
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Bul buyriqti gruppada islete almaysan`!",
                    replyToMessageId: message.MessageId,
                    cancellationToken: cancellationToken);
            }
            if (chatId < 0 && message.Text == "/start" )
            {
                _tokenService.CreateOwnerGroup(new OwnerGroup
                {
                    OwnerId = message.From.Id,
                    GroupId = chatId
                }).Wait();
            }
            if(lastMessage == "Add sum" && message.Text.Count() > 1)
            {
                var sum = int.Parse(message.Text);

                var report = new Report
                {
                    UserId = chatId,
                    GroupId = _reportService.GetGroupIdByUserId(chatId).Result,
                    Sum = sum,
                    Username = message.Chat.FirstName
                };
                _reportService.CreateReport(report).Wait();
                await botClient.SendTextMessageAsync(
                   chatId: chatId,
                   text: "Added✅",
                   replyToMessageId: message.MessageId,
                   cancellationToken: cancellationToken);
                lastMessage = "";
            }
            if(message.Text == "Add sum")
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Enter sum:",
                    replyToMessageId: message.MessageId,
                    cancellationToken: cancellationToken);
                    lastMessage = "Add sum";    
            }
            if(message.Text == "/report" && message.Chat.Id > 0)
            {
                var groupId = _reportService.GetGroupIdByUserId(chatId).Result;
                var reports = _reportService.GetAllReport(groupId).Result;
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: ReportEditer.EditReport(reports: reports),
                    replyToMessageId: message.MessageId,
                    parseMode: ParseMode.Markdown,
                    cancellationToken: cancellationToken);
            }
            if (message.Text == "/report@apartmentDebt_bot" && message.Chat.Id < 0)
            {
                
                var reports = _reportService.GetAllReport(chatId).Result;
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Report:\n" + ReportEditer.EditReport(reports: reports),
                    replyToMessageId: message.MessageId,
                    parseMode: ParseMode.Markdown,
                    cancellationToken: cancellationToken);
            }
            if((message.Text == "/calculate" || message.Text == "/calculate@apartmentDebt_bot") && message.Chat.Id < 0)
            {
                var reports = _reportService.GetAllReport(chatId).Result;

                var sum = _reportService.Canculate(chatId).Result;
                var cards = _tokenService.GetAllCards(chatId).Result;
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: ReportEditer.OverAllReport(reports,sum,cards),
                    replyToMessageId: message.MessageId,
                    parseMode: ParseMode.Markdown,
                    cancellationToken: cancellationToken);
            }
            if ((message.Text == "/notification@apartmentDebt_bot" || message.Text == "/notification") && message.Chat.Id < 0)
            {
                var members = _reportService.GroupMemebers(chatId).Result;

                foreach (var memberId in members)
                {
                    await botClient.SendTextMessageAsync(
                    chatId: memberId,
                    text: "Aqsha shig`arg`an bolsan` botqa kirit!",
                    cancellationToken: cancellationToken);
                }
                
            }

        }

        async Task OnCallbackQueryReceived()
        {

            string callback = update.CallbackQuery.Data;
            var chatId = update.CallbackQuery.Message.Chat.Id;
            var message = update.CallbackQuery.Message;

            if (callback == "Create group")
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Enter group name :",
                    cancellationToken: cancellationToken);
            }
            if (callback == "Imformation")
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Some Information",
                    replyToMessageId: message.MessageId,
                    cancellationToken: cancellationToken);
            }
        }
       
    }
    #region Handle Error
    static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
    #endregion


}
