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
        static FillIProduct fill_product = new FillIProduct();
        static List<DataOfProduct> product = new List<DataOfProduct>();
        static Keyboard keyboard = new Keyboard();

        static void Main(string[] args)
        {
            //cee1d885245c490a85cf0ce0ef756cc4
            Bot = new TelegramBotClient("596483216:AAFe1hLBlTZDZWNLLIxpzX2LUq-NhqHR2mo");
            AIConfiguration config = new AIConfiguration("cee1d885245c490a85cf0ce0ef756cc4", SupportedLanguage.Russian);
            apiAi = new ApiAi(config);

            Bot.OnMessage += Bot_OnMessageReceived;
            Bot.OnCallbackQuery += Bot_OnCallbackQueryReceived;

            Bot.StartReceiving();
            fill_product.Fill_product(product); //заполнение листа информацией из тхт
            Console.ReadLine();
            Bot.StopReceiving();
        }

        
        private static async void Bot_OnCallbackQueryReceived(object sender, CallbackQueryEventArgs e)
        {
            string buttonText = e.CallbackQuery.Data;
            string name = $"{e.CallbackQuery.From.FirstName}{e.CallbackQuery.From.LastName}";
            Console.WriteLine($"{name} нажал(а) кнопку {buttonText}");

            if (buttonText == "Новичок")
                await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Я с радостью научу тебя всему, что знаю сам!");
            else if (buttonText == "Профессионал")
                await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Мои поздравления, коллега! Чем могу помочь?");
            else if (buttonText == "Указать данные")
                await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Укажи вид изделия, размер и данные о пряже");
            else if (buttonText == "Произвести расчет")
            { 
                CalculationData cd = new CalculationData();
                cd.datas("майка", "43", "67", "0");
                await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Результат расчета - ");
            }
            await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, $"Ты нажал(а) кнопку {buttonText}");
        }

        private static void Bot_OnMessageReceived(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message;
            if (message == null || message.Type != MessageType.TextMessage)
                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Извини, но я могу воспринимать только текст");
            string name = $"{message.From.FirstName}{message.From.LastName}";
            Console.WriteLine($"{name} отправил(a) сообщение '{message.Text}'  ");
            switch (message.Text)
            {
                case "/start": keyboard.Start(Bot, e); break;
                case "/inline": keyboard.Inline(Bot, e); break;
                case "/keyboard": keyboard.Keyboard_k(Bot, e); break;
                case "/help": keyboard.Help(Bot, e); break;
                case "/ideas": keyboard.Idea(Bot, e); break;
                case "/calculator": keyboard.Calculator(Bot, e); break;
                default:
                    /*var response = apiAi.TextRequest(message.Text);
                    string answer = response.Result.Fulfillment.Speech;
                    if (answer == "")
                        answer = "Прости, я не понимаю тебя";
                    await Bot.SendTextMessageAsync(message.From.Id, answer);*/
                    break;
            }
        }
    }
}
