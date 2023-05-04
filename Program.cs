using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.IO;
using NPOI.XWPF.UserModel;



namespace SylviaBot
{
    internal class Program
    {
        public static List<Info> reportInfo = new List<Info>();
        static string? situation;
        static string? thought;
        static string? emotion;
        static string? reaction;
        private static string GenerateReport()
        {
            string report = "Дата и время\tСитуация\tМысли\tЭмоции\tРеакция\n";
            foreach (var inf in reportInfo)
            {
                string time = inf.Date.ToString("MM-dd HH:mm");
                report += $"{time}\t{inf.Situation}\t{inf.Thought}\t{inf.Emotion}\t{inf.Reaction}\n";
            }

            return report;
        }
        static void Main(string[] args)
        {
            var client = new TelegramBotClient("6111728145:AAEHa18U_3OJ3W2y9I__OLF2skY5kWRT6ck");
            var person = client.GetMeAsync().Result;
            client.StartReceiving(HandleUpdate, HandleError);
            Console.ReadLine();
        }

        private static Task HandleError(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }

        async static Task HandleUpdate(ITelegramBotClient client, Update update, CancellationToken token)
        {

            var message = update.Message;
            if (message == null)
            {
                return;
            }
            if (message.Text != null)
            {
                if (message.Text.Contains("/start"))
                {
                    var keyboard = new ReplyKeyboardMarkup(new[]
                    {
                new[]
                {
                    new KeyboardButton("Ситуация"),
                    new KeyboardButton("Мысли"),
                },
                new[]
                {
                    new KeyboardButton("Эмоции"),
                    new KeyboardButton("Реакция")
                },
                new[]
                {
                    new KeyboardButton("Отчет")
                },
                new[]
                {
                    new KeyboardButton("Помощь")
                }
            });
                    keyboard.ResizeKeyboard = true;
                    await client.SendTextMessageAsync(message.Chat.Id, "Меня зовут Сильвия! Я бот, основанный на протоколе СМЭР. Я буду помогать Вам отслеживать информацию о Ваших эмоциях и автоматических мыслях", replyMarkup: keyboard);
                }
                {
                    var chatId = message.Chat.Id;
                    if (message.Type == MessageType.Text)
                    {

                        switch (message.Text)
                        {

                            case "Ситуация":
                                {
                                   
                                    await client.SendTextMessageAsync(chatId, "Расскажите о том, что произошло:");
                                    situation = null;
                                    while (true)
                                    {
                                        var updates = await client.GetUpdatesAsync();
                                        var newMessage = updates.LastOrDefault()?.Message;
                                        if (newMessage == null)
                                        {
                                            continue;
                                        }
                                        if (newMessage.Text != null)
                                        {
                                            situation = newMessage.Text;
                                            break;
                                        }
                                        if (newMessage.Photo != null)
                                        {
                                            await client.SendTextMessageAsync(chatId, "Жаль, что я не умеею работать с фотографиями");
                                            return;
                                        }
                                    }
                                    break;
                                }
                            case "Мысли":
                                {
                                    await client.SendTextMessageAsync(chatId, "Расскажите, о чем Вы думаете");
                                    thought = null;
                                    while (true)
                                    {
                                        var updates = await client.GetUpdatesAsync();
                                        var newMessage = updates.LastOrDefault()?.Message;
                                        if (newMessage == null)
                                        {
                                            continue;
                                        }
                                        if (newMessage.Text != null)
                                        {
                                            thought = newMessage.Text;
                                            break;
                                        }
                                        if (newMessage.Photo != null)
                                        {
                                            await client.SendTextMessageAsync(chatId, "Жаль, что я не умеею работать с фотографиями");
                                            return;
                                        }
                                    }
                                    break;
                                }
                            case "Эмоции":
                                {
                                    await client.SendTextMessageAsync(chatId, "Расскажите, что вы чувствуете:");
                                    emotion = null;
                                    while (true)
                                    {
                                        var updates = await client.GetUpdatesAsync();
                                        var newMessage = updates.LastOrDefault()?.Message;
                                        if (newMessage == null)
                                        {
                                            continue;
                                        }
                                        if (newMessage.Text != null)
                                        {
                                            emotion = newMessage.Text;
                                            break;
                                        }
                                        if (newMessage.Photo != null)
                                        {
                                            await client.SendTextMessageAsync(chatId, "Жаль, что я не умеею работать с фотографиями");
                                            return;
                                        }
                                    }
                                    break;
                                }
                            case "Реакция":
                                {
                                    await client.SendTextMessageAsync(chatId, "Расскажите, как Вы отреагировали на произошедшее:");
                                    reaction = null;
                                    while (true)
                                    {
                                        var updates = await client.GetUpdatesAsync();
                                        var newMessage = updates.LastOrDefault()?.Message;
                                        if (newMessage == null)
                                        {
                                            continue;
                                        }
                                        if (newMessage.Text != null)
                                        {
                                            reaction = newMessage.Text;
                                            break;
                                        }
                                        if (newMessage.Photo != null)
                                        {
                                            await client.SendTextMessageAsync(chatId, "Жаль, что я не умеею работать с фотографиями");
                                            return;
                                        }
                                    }
                                    break;
                                }
                            case "Отчет":
                                {
                                    if (reportInfo.Count > 0)
                                    {
                                        string report = GenerateReport();
                                        await client.SendTextMessageAsync(chatId, report);
                                    }
                                    else
                                    {
                                        await client.SendTextMessageAsync(chatId, "Вы не сделали ни одной записи. Я не могу составить пустой отчет");
                                    }
                                }
                                break;
                        }
                        reportInfo.Clear();
                    }
                    var inf = new Info()
                    {
                        Date = DateTime.Now,
                        Situation = situation,
                        Thought = thought,
                        Emotion = emotion,
                        Reaction = reaction
                    };
                    reportInfo.Add(inf);
                }
                if (message.Photo != null)
                {
                    await client.SendTextMessageAsync(message.Chat.Id, "Жаль, что я не умеею работать с фотографиями");
                    return;
                }
            }

            static Task HandleError(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
            {
                Console.WriteLine(exception);
                return Task.CompletedTask;
            }
        }
    }
}
    

