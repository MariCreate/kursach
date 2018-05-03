using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using ApiAiSDK;
using ApiAiSDK.Model;



namespace KnitSencei_bot
{
    class Program
    {
        static TelegramBotClient Bot;
        static ApiAi apiAi;

        static void Main(string[] args)
        {
            //cee1d885245c490a85cf0ce0ef756cc4
            Bot = new TelegramBotClient("596483216:AAFe1hLBlTZDZWNLLIxpzX2LUq-NhqHR2mo");
            AIConfiguration config = new AIConfiguration("cee1d885245c490a85cf0ce0ef756cc4", SupportedLanguage.Russian);
            apiAi = new ApiAi(config);


            Bot.OnMessage += Bot_OnMessageReceived;
            Bot.OnCallbackQuery += Bot_OnCallbackQueryReceived;

            var me = Bot.GetMeAsync().Result;
            Console.WriteLine(me.FirstName);
            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }

        
        private static async void Bot_OnCallbackQueryReceived(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            string buttonText = e.CallbackQuery.Data;
            string name = $"{e.CallbackQuery.From.FirstName}{e.CallbackQuery.From.LastName}";
            Console.WriteLine($"{name} нажал(а) кнопку {buttonText}");

            if (buttonText == "Новичок")
            {
                await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Я с радостью научу тебя всему, что знаю сам!");

            }
            else if (buttonText == "Профессионал")
            {
                await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Мои поздравления, коллега! Чем могу помочь?");
            }


            else if (buttonText == "Указать данные")
            {

                await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Укажи вид изделия, размер и данные о пряже");
            }

            else if (buttonText == "Произвести расчет")

            {
                CalculationData cd = new CalculationData();
                cd.datas("майка", "43", "67", "0");

                await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Результат расчета - ");
            }



            await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, $"Ты нажал(а) кнопку {buttonText}");
        }



        private static async void Bot_OnMessageReceived(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message;
            if (message == null || message.Type != MessageType.TextMessage)
                return;
                
            string name = $"{message.From.FirstName}{message.From.LastName}";
            Console.WriteLine($"{name} отправил(a) сообщение '{message.Text}'  ");
           
            switch (message.Text)
            {
                case "/start":
                    string text =
 @"Список команд :
/start - запуск бота,
/inline - связь с разработчиком,
/keyboard - вывод клавиатуры,
/help - просить о помощи Сенсея,
/calculator - вызвать калькулятор количества пряжи
/ideas- показать идеи для вязания";
                    await Bot.SendTextMessageAsync(message.From.Id, text);
                    break;
                case "/inline":
                    var inlineKeyboard = new InlineKeyboardMarkup
                    (new[]
                    {new[]
                        {
                            InlineKeyboardButton.WithUrl("VK", "https://vk.com/id10618217"),
                            InlineKeyboardButton.WithUrl("Telegram","https://t.me/litvinesha")
                        },
                        new []
                        {
                            InlineKeyboardButton.WithUrl("Google+", "https://plus.google.com/109645919916102660305"),
                           
                        }
                    });
                    await Bot.SendTextMessageAsync(message.From.Id, "Выбери пункт меню", replyMarkup:inlineKeyboard);
                    break;
                case "/keyboard":
                    var replyKeyboard = new ReplyKeyboardMarkup(new[]
                    {
                        new []
                        {
                            new KeyboardButton("Ты кто?"),
                            new KeyboardButton("Привет!")
                        },
                        new []
                        {
                            new KeyboardButton("Помоги мне!"),
                            new KeyboardButton("Как дела?")
                        },
                        new [] 
                        {
                            new KeyboardButton("Контакт") {RequestContact = true},
                            new KeyboardButton("Геолокация") {RequestLocation = true}
                        }
                    });
                    await Bot.SendTextMessageAsync(message.Chat.Id, "А вот и Я!",
                        replyMarkup: replyKeyboard);
                    break;
                case "/help":
                    var inlineKeyboard2 = new InlineKeyboardMarkup
                  (new[] {
                            InlineKeyboardButton.WithCallbackData("Новичок"),
                            InlineKeyboardButton.WithCallbackData("Профессионал")
                         });
                    await Bot.SendTextMessageAsync(message.From.Id, "Выбери уровень своего мастерства", replyMarkup: inlineKeyboard2);
                    break;

                case "/ideas":
                    var inlineKeyboard3 = new InlineKeyboardMarkup
                  (new[] {
                            InlineKeyboardButton.WithCallbackData("Платья"),
                            InlineKeyboardButton.WithCallbackData("Свитера")
                         });
                    await Bot.SendTextMessageAsync(message.From.Id, "Выбери, какие идеи тебя интересуют", replyMarkup: inlineKeyboard3);
                    break;

                case "/calculator":
                    var inlineKeyboard4= new InlineKeyboardMarkup(new[]
                    {
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Указать данные")
                        },
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Произвести расчет")
                        }

                    });
                
                    await Bot.SendTextMessageAsync(message.From.Id, "Выбери, что ты хочешь сделать", replyMarkup: inlineKeyboard4);
                    break;


                default:
                    var response = apiAi.TextRequest(message.Text);
                    string answer = response.Result.Fulfillment.Speech;
                    if (answer == "")
                        answer = "Прости, я не понимаю тебя";
                    await Bot.SendTextMessageAsync(message.From.Id, answer);

                    break;
            }
        }
    }
}
