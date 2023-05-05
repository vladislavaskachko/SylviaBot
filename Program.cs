using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;



namespace SylviaBot
{
    internal class Program
    {
        public static List<string> situations = new List<string>();
        public static List<string> thoughts = new List<string>();
        public static List<string> emotions = new List<string>();
        public static List<string> reactions = new List<string>();
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
        private static InlineKeyboardMarkup Menu()
        {
            var keyboard = new[]
            {
        new []
        {
            InlineKeyboardButton.WithCallbackData("Ситуация"),
            InlineKeyboardButton.WithCallbackData("Мысли")
        },
        new []
        {
            InlineKeyboardButton.WithCallbackData("Эмоции"),
            InlineKeyboardButton.WithCallbackData("Реакция")
        },
        new []
        {
            InlineKeyboardButton.WithCallbackData("Отчет")
        },
        new []
        {
            InlineKeyboardButton.WithCallbackData("Помощь")
        },
            };

            return new InlineKeyboardMarkup(keyboard);
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
                    new KeyboardButton("Заполнить дневник"),
                },
            });
                    keyboard.ResizeKeyboard = true;
                    await client.SendTextMessageAsync(message.Chat.Id, "Меня зовут Сильвия! Я бот, основанный на протоколе СМЭР. Я буду помогать Вам отслеживать информацию о Ваших эмоциях и автоматических мыслях. Каждый раз, когда Вы хотите рассказать мне о том, как Ваши дела, нажимайте кнопку 'Заполнить дневник'", replyMarkup: keyboard);
                }
                else if (message.Text == "Заполнить дневник")
                {
                    var menu = Menu();
                    await client.SendTextMessageAsync(message.Chat.Id, "Здесь Вы можете записать всю информацию, а я сформирую из нее отчет", replyMarkup: menu);
                }
            }
            else if (message.Type == MessageType.Photo)
            {
                await client.SendTextMessageAsync(message.Chat.Id, "Жаль, что я не умеею работать с фотографиями");
                return;
            }
            else if (update.CallbackQuery != null)
            {
                await HandleCallbackQuery(update.CallbackQuery, client, update);
            }
        }

        async static Task HandleCallbackQuery(CallbackQuery callbackQuery, ITelegramBotClient client, Update update)
        {
            if (callbackQuery == null)
            {
                return;
            }
            var chatId = callbackQuery.Message.Chat.Id;
            var messageId = callbackQuery.Message.MessageId;
            var button = callbackQuery.Data;
            if (button == "Ситуация")
            {
                await client.SendTextMessageAsync(chatId, "Расскажите, что случилось");
                var message = update.Message;
                if (message != null && message.Text != null)
                {
                    situations.Add(message.Text);
                }
                else if (message != null && message.Photo != null)
                {
                    await client.SendTextMessageAsync(message.Chat.Id, "Жаль, что я не умеею работать с фотографиями");
                }
            }
            else if (button == "Мысли")
            {
                await client.SendTextMessageAsync(chatId, "Какие мысли у Вас возникли?");
                var message = update.Message;
                if (message != null && message.Text != null)
                {
                    thoughts.Add(message.Text);
                }
                else if (message != null && message.Photo != null)
                {
                    await client.SendTextMessageAsync(message.Chat.Id, "Жаль, что я не умеею работать с фотографиями");
                }
            }
            else if (button == "Эмоции")
            {
                await client.SendTextMessageAsync(chatId, "Что Вы чувствуете?");
                var message = update.Message;
                if (message != null && message.Text != null)
                {
                    emotions.Add(message.Text);
                }
                else if (message != null && message.Photo != null)
                {
                    await client.SendTextMessageAsync(message.Chat.Id, "Жаль, что я не умеею работать с фотографиями");
                }
            }
            else if (button == "Реакция")
            {
                await client.SendTextMessageAsync(chatId, "Как Вы отреагировали?");
                var message = update.Message;
                if (message != null && message.Text != null)
                {
                    reactions.Add(message.Text);
                }
                else if (message != null && message.Photo != null)
                {
                    await client.SendTextMessageAsync(message.Chat.Id, "Жаль, что я не умеею работать с фотографиями");
                }
            }
        }




}
    }









