using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.GettingUpdates;
using Telegram.BotAPI.AvailableMethods;

namespace ReplyKeyboardMarkup_01
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Start!");
            var bot = new BotClient("BotToken");  //Токен телеграм Бота
            var bot1 = new TelegramBotClient("BotToken");
            var updates = bot.GetUpdates();
            List<string> jokesStorage = new List<string>();     //Список вызванных шуток
            List<string> supportStorage = new List<string>();   //Список вызванных фраз подддержки
            List<string> pictureStorage = new List<string>();   //Список вызванных картинок
            List<string> videoStorage = new List<string>();     //Список вызванных видео
            string jokeBot;     //Выдаваемый анекдот
            string supportBot;  //Выдаваемая фраза поддержки
            string pictureBot;  //Выдаваемое название картинки
            string videoBot;    //Выдаваемое название видео
            int jokeBotSize;    //Выдаваемый размер массива анекдотов
            int supportBotSize; //Выдаваемый размер массива фраз поддержки
            int pictureBotSize; //Выдаваемый размер массива картинок
            int videoBotSize;   //Выдаваемый размер массива видео


            bot.SetMyCommands(new BotCommand("start", "Open"), new BotCommand("close", "Close"));   //Изначальные комманды Бота
            while (true)
            {

                if (updates.Length > 0)
                {
                    foreach (var update in updates)
                    {
                        switch (update.Message.Text)
                        {
                            case "/start":  //Запуск клавиатуры
                                bot.SendMessageAsync(update.Message.Chat.Id, "С чего начнём?", replyMarkup: (ReplyMarkup)GetButtons());
                                break;
                            case "Скинь видео с животными":                                 //Вызов видео
                                #region Видео
                                Program.GetVideo(out videoBot, out videoBotSize);           //Получение названия видео и размера массива видео из метода GetVideo

                                if (videoStorage.Count == videoBotSize)                     //Проверка заполненности списка вызванных видео
                                {
                                    videoStorage.Clear();                                   //Очистка списка, если он заполнен
                                }

                                while (videoStorage.Contains(videoBot))                     //Проверка, есть ли полученное название видео в списке вызванных видео (с целью избежания отправки одинаковых видео)
                                {
                                    Program.GetVideo(out videoBot, out videoBotSize);       //Если подобное название видео есть, повторное получение названия видео и размера массива видео из метода GetVideo
                                }

                                FileStream fsVideo = System.IO.File.OpenRead(@$"C:\Users\s-dam\Desktop\CuteVideos\{videoBot}.mp4");     //Получение файла видео с определенным названием
                                InputOnlineFile myVideo = new InputOnlineFile(fsVideo, "video");
                                bot1.SendVideoAsync(update.Message.Chat.Id, myVideo);       //Вывод видео через Бота
                                videoStorage.Add(videoBot);                                 //Добавление полученного названия видео в список вызванных видео
                                break;
                            #endregion
                            case "Скинь фотку с животными":                                 //Вызов картинок
                                #region Картинка
                                Program.GetPicture(out pictureBot, out pictureBotSize);     //Получение названия картинки и размера массива картинок из метода GetPicture

                                if (pictureStorage.Count == pictureBotSize)                 //Проверка заполненности списка вызванных картинок
                                {
                                    pictureStorage.Clear();                                 //Очистка списка, если он заполнен
                                }

                                while (pictureStorage.Contains(pictureBot))                 //Проверка, есть ли полученное название картинки в списке вызванных картинок (с целью избежания отправки одинаковых картинок)
                                {
                                    Program.GetPicture(out pictureBot, out pictureBotSize); //Если подобное название видео есть, повторное получение названия картинки и размера массива картинок из метода GetPicture
                                }

                                FileStream fsPhoto = System.IO.File.OpenRead(@$"C:\Users\s-dam\Desktop\CutePhotos\{pictureBot}.jpg");   //Получение файла картинки с определенным названием
                                InputOnlineFile myPicture = new InputOnlineFile(fsPhoto, "picture");
                                bot1.SendPhotoAsync(update.Message.Chat.Id, myPicture);     //Вывод картинки через Бота
                                pictureStorage.Add(pictureBot);                             //Добавление полученного названия картинки в список вызванных картинок
                                break;
                            #endregion
                            case "Расскажи анекдот":                                        //Вызов анекдота
                                #region Анекдот
                                Program.GetJoke(out jokeBot, out jokeBotSize);              //Получение анекдота и размера массива анекдотов из метода GetJoke

                                if (jokesStorage.Count == jokeBotSize)                      //Проверка заполненности списка вызванных анекдотов
                                {
                                    bot.SendMessageAsync(update.Message.Chat.Id, "Извини, свежих анекдотов пока что больше нет.");  //Вывод фразы о том, что все свежие анекдоты закончились
                                    jokesStorage.Clear();                                   //Очистка списка, если он заполнен
                                }

                                while (jokesStorage.Contains(jokeBot))                      //Проверка, есть ли полученный анекдот в списке вызванных анекдотов (с целью избежания отправки одинаковых анекдотов)
                                {
                                    Program.GetJoke(out jokeBot, out jokeBotSize);          //Если подобный анекдот есть, повторное получение анекдота и размера массива анекдотов из метода GetJoke
                                }
                                bot.SendMessageAsync(update.Message.Chat.Id, jokeBot);      //Вывод анекдота через Бота
                                jokesStorage.Add(jokeBot);                                  //Добавление полученного анекдота в список вызванных анекдотов
                                break;
                            #endregion
                            case "Поддержи меня":                                           //Вызов фразы поддержки
                                #region Фраза поддержки
                                Program.GetSupport(out supportBot, out supportBotSize);     //Получение фразы поддержки и размера массива фраз поддержки из метода GetSupport

                                if (supportStorage.Count == supportBotSize)                 //Проверка заполненности списка вызванных фраз поддержки
                                {
                                    supportStorage.Clear();                                 //Очистка списка, если он заполнен
                                }

                                while (supportStorage.Contains(supportBot))                 //Проверка, есть ли полученная фраза поддержки в списке вызванных фраз поддержки (с целью избежания отправки одинаковых фраз поддержки)
                                {
                                    Program.GetSupport(out supportBot, out supportBotSize); //Если подобная фраза поддержки есть, повторное получение фразы поддержки и размера массива фраз поддержки из метода GetSupport
                                }
                                bot.SendMessageAsync(update.Message.Chat.Id, supportBot);   //Вывод фразы поддержки через Бота
                                supportStorage.Add(supportBot);                             //Добавление полученной фразы поддержки в список вызванных фраз поддержки
                                break;
                            #endregion
                            case "Закрой клавиатуру":
                                bot.SendMessageAsync(update.Message.Chat.Id, "Уже уходишь?😕", replyMarkup: (ReplyMarkup)GetQuitButtons());                              //Попытка оставить пользователя
                                break;
                            case "Извини, мне пора. Я вернусь чуть позже!":
                                bot.SendMessageAsync(update.Message.Chat.Id, "До встречи, я всегда рад тебе!", replyMarkup: new ReplyKeyboardRemove());                  //Закрытие клавиатуры, если пользователь решил уйти
                                break;
                            case "Что ты, я просто изучаю твой функционал!😅":
                                bot.SendMessageAsync(update.Message.Chat.Id, "Хе-хе-хе. Мне нравится твоя любознательность!", replyMarkup: (ReplyMarkup)GetButtons());   //Ответная реакция, если пользователь решил остаться
                                break;
                            default:
                                bot.SendMessageAsync(update.Message.Chat.Id, "Извини, я тебя не понял. Если ты хочешь открыть клавиатуру, напиши /start");               //Ответ на команду не из списка
                                break;
                        }
                    }
                    updates = bot.GetUpdates(offset: updates.Max(u => u.UpdateId) + 1);
                }
                else
                {
                    updates = bot.GetUpdates();
                }
            }
        }
        private static object GetButtons()  //Метод вызова клавиатуры 
        {
            var keyboard = new ReplyKeyboardMarkup
            {
                Keyboard = new KeyboardButton[][]{
                             new KeyboardButton[]{
                             new KeyboardButton("Скинь видео с животными"),     //1 строка 1 столбец, кнопка вызова видео
                             new KeyboardButton("Скинь фотку с животными")      //1 строка 2 столбец, кнопка вызова картинки
                             }, //1 строка
                             new KeyboardButton[]{
                             new KeyboardButton("Расскажи анекдот"),            //2 строка 1 столбец, кнопка вызова анекдота
                             new KeyboardButton("Поддержи меня")                //2 строка 2 столбец, кнопка вызова фразы поддержки
                             }, //2 строка
                             new KeyboardButton[]{
                             new KeyboardButton("Закрой клавиатуру")            //3 строка 1 столбец, кнопка закрытия клавиатуры
                             }  //3 строка 
                   },
                ResizeKeyboard = true
            };
            return keyboard;                                                    //Возвращение клавиатуры
        }
        private static object GetQuitButtons()  //Метод вызова клавиатуры перед её закрытием
        {
            var keyboard = new ReplyKeyboardMarkup
            {
                Keyboard = new KeyboardButton[][]{
                             new KeyboardButton[]{
                             new KeyboardButton("Извини, мне пора. Я вернусь чуть позже!"),       //1 строка 1 столбец, кнопка закрытия клавиатуры
                             }, //1 строка
                             new KeyboardButton[]{
                             new KeyboardButton("Что ты, я просто изучаю твой функционал!😅"),   //2 строка 1 столбец, кнопка возврата к основной клавиатуре
                             } //2 строка
                   },
                ResizeKeyboard = true
            };
            return keyboard;                                                                     //Возвращение клавиатуры
        }
        private static void GetVideo(out string video, out int videoArraySize)  //Метод вызова видео с животными
        {
            string[] VideoArray = {
                    "1","2","3","4","5","6","7","8","9","10","11","12","13","14","15","16","17","18","19","20","21","22","23","24","25"};   //Массив названий видео

            var randVideo = new Random().Next(VideoArray.Length);       //Получение псевдослучайного индекса в массиве видео
            video = VideoArray[randVideo];                              //Получение названия видео с псевдослучайным индексом из массива видео
            videoArraySize = VideoArray.Length;                         //Получение размера массива видео
        }
        private static void GetPicture(out string picture, out int pictureArraySize)    //Метод вызова картинок с животными
        {
            string[] PictureArray = {
                    "1","2","3","4","5","6","7","8","9","10","11","12","13","14","15","16","17","18","19","20","21","22","23","24","25","26","27","28","29","30","31","32","33","34","35","36","37","38","39","40","41","42","43","44","45","46","47","48","49","50"};   //Массив названий фото

            var randPicture = new Random().Next(PictureArray.Length);   //Получение псевдослучайного индекса в массиве картинок
            picture = PictureArray[randPicture];                        //Получение названия картинки с псевдослучайным индексом из массива картинок
            pictureArraySize = PictureArray.Length;                     //Получение размера массива картинок
        }
        private static void GetJoke(out string joke, out int jokesArraySize) //Метод вызова анекдота
        {
            string[] JokesArray = {
                    "За что судят охранника цеха по сборке печей после ночного ограбления? \n... \n... \n... \nЗа беспечность.",
                    "Сапёр, сделавший лимонную бомбу, появился на публике с очень кислой миной.",
                    "В семье пуговиц трагедия. Отца пришили.",
                    "Учительница — ученику: — Вот если я тебе дам белку, потом ещё двух белок, а потом ещё трёх белок! Сколько будет? \nУченик: — Семь! \nУчительница:— Слушай внимательно!Сначала одну белку, потом ещё двух и потом ещё трёх. Сколько ? \n— Семь! \n— Таак! Давай по-другому! Одно яблоко плюс два яблока, плюс ещё три яблока!Сколько ? \n— Шесть! \n— Ну наконец-то! А белка плюс две белки плюс три белки!Сколько ? \n— Семь! \nУчительница: — Ну почему? \n— Да у меня уже есть дома белка",
                    "Врач прослушивает сталкера и бормочет: \n- Хорошо, хорошо... \n- Доктор, а что хорошо-то? \n- Хорошо, что не у меня...",
                    "Заходит как-то раз черепаха в бар и подходит к бармену, заказывает стакан воды и уходит. \nПриходит на следующий день, снова заказывает стакан воды и так ещё несколько дней подряд. \nВ какой-то из дней, бармен спрашивает у черепахи: \n— Черепаха, почему ты заказываешь воду, ведь у меня столько алкоголя? \nЧерепаха ему отвечает: \n— Не сейчас, у меня дом горит.",
                    "Заходят как-то аморал, нигилист и циник в бар. \nА бармен им: — У нас спиртное только с 18.",
                    "— Какой самый сложный трюк в скейтбординге? \n— Жить отдельно от родителей.",
                    "— Стой! Что ты пьешь! Это же метанол, ты что, не видишь?! \n— Нет.",
                    "Если ты считаешь свой труд бессмысленным, просто вспомни, что на футболках с логотипами групп «КиШ» и «Сектор газа» есть бирки с рекомендациями по стирке.",
                    "Случай при съёме жилья: \n— Тараканы в доме есть? \n— Нет, их пауки съели. \n— У Вас пауки есть? \n— Нет, их крысы съели. \n— У Вас крысы есть? \n— Нет, их Иван Андреевич съел. \n— А кто это? \n— Забыл представиться.",
                    "—Сколько я буду получать на должности учителя математики? \n— 10000 рублей. \n— Как-то маловато. \n— Тогда 100000. \n— На порядок лучше.",
                    "Экскаваторщик Иванов обижается на родителей за такое глупое имя - «Экскаваторщик»",
                    "Заходит нейтрон в бар и спрашивает: \n— А почём у вас выпивка? \n— А бармен отвечает: \n— Тебе хватит, ты уже и так заряжен.",
                    "Два дровосека стучат в лезную избушку. Оттуда выходит третий дровосек: \n— В чём дело? \n— Да вот, нашли в лесу тело убитого и подумали, может быть, это ты. \n— В красной рубашке или коричневой? \n— В красной. \n— Ну, тогда не я."};   //Массив анекдотов

            var randJoke = new Random().Next(JokesArray.Length);        //Получение псевдослучайного индекса в массиве анекдотов
            joke = JokesArray[randJoke];                                //Получение анекдота с псевдослучайным индексом из массива анекдотов
            jokesArraySize = JokesArray.Length;                         //Получение размера массива анекдотов
        }
        private static void GetSupport(out string support, out int supportArraySize)  //Метод вызова фразы поддержки
        {
            string[] SupportArray = {
                    "Не переживай, всё обязательно наладится. Побольше отдыхай, ты молодец и заслуживаешь этого. 🖤",
                    "У тебя всё обязательно получится, я в тебя верю. Ты много трудишься и ты добьёшься своего. 🖤",
                    "Каким бы темным и мрачным местом не казался тебе мир вокруг, помни, что трудности - лишь временны. Не переживай, следующая остановка - «Счастье».🖤",
                    "Через беспокойства проходят почти все люди - это нормальное состояние человека. Беспокойство - это защитный механизм человека, позволяющий создавать мир таким, каким он нам нравится. Давай не будем зацикливаться на проблеме, а лучше подумаем над её решением и тем, как не повторить прежние ошибки? 🖤",
                    "Даже если тебе кажется, что от тебя мало пользы, помни, что всегда есть люди, для которых ты - источник мотивации и центр Вселенной. 🖤",
                    "Золотко, люби себя. Поверь, ты заслуживаешь всего, о чём ты мечтаешь. 🖤",
                    "Испытывать эмоции - это совершенно нормально. Ты живой человек, и у тебя есть чувства. Не стоит всё держать в себе и замыкаться. 🖤",
                    "Я уверен, что всё обязательно наладится. Но не забывай об отдыхе. Помни, трудности лишь временны, главное - здоровье. 🖤",
                    "Пожалуйста, держись. Не позволяй себе погрузиться в уныние. Самовнушение имеет важное значение для состояния организма! Если будешь искренне верить в хорошее, то и самочувствие станет лучше! 🖤",
                    "Знаешь, я всегда восхищался твоим талантом преодолевать все невзгоды с изяществом и юмором. 🖤",
                    "Я понимаю, как тебе сейчас нелегко. Я хорошо тебя понимаю: порой я тоже испытывал трудности. Но знаешь, за черной полосой нашей жизни всегда следует белая. 🖤",
                    "Запомни: самый темный час - всегда перед рассветом. В любом случае, если у тебя уже стемнело, ложись спать. 🖤",
                    "Есть пирожок с капустой, есть пирожок с повидлом, а есть ты - самый классный пирожочек. Люблю тебя. 🖤",
                    "Вот настооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооолько сильно я верю в тебя и твой успех. 🖤",
                    "Кто прочитал, того ждёт успех, удача и крепкое здоровье. 🖤"};   //Массив фраз поддержки

            var randSupport = new Random().Next(SupportArray.Length);   //Получение псевдослучайного индекса в массиве фраз поддержки
            support = SupportArray[randSupport];                        //Получение фразы поддержки с псевдослучайным индексом из массива фраз поддержки
            supportArraySize = SupportArray.Length;                     //Получение размера массива фраз поддержки
        }
    }
}