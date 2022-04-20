/// <summary>
/// Класс для взаимодействия с комнатами на сервере.
/// </summary>
class Web
{
    /// <summary> Ответ сервера. res = "-1", если связи нет; ""(пустая), если запрос ещё обрабатывается. </summary>
    public static string res = string.Empty;
    /// <summary> Номер текущей комнаты (от 0 до 99), по умолчанию 0. Обновляется автоматически после метода Create/Join. </summary>
    public static int room = 0;
    /// <summary> Номер в комнате у первого игрока устройства, по умолчанию 0. Обновляется автоматически после метода Join. </summary>
    public static int index = 0;
    /// <summary> Ограничение времени работы одного потока (в миллисекундах). </summary>
    public static int tick = 5000;

    /// <summary> Адрес удалённого сервера. </summary>
    private const string ip = "http://radiant-taiga-93891.herokuapp.com/";
    /// <summary> Режим работы: удалённо или локально. </summary>
    private static readonly bool online = true;
    /// <summary> Разделитель для описания команд. </summary>
    private const string sep = "#";

    /// <summary> post-запрос на удалённый сервер. </summary>
    private static string Post(string mes)
    {
        try
        {
            // https://stackoverflow.com/questions/4015324/how-to-make-an-http-post-web-request // сложно
            // https://stackoverflow.com/questions/9145667/how-to-post-json-to-a-server-using-c
            System.Net.HttpWebRequest httpWebRequest;
            if (online) httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(ip);
            else httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create("http://127.0.0.1:80");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new System.IO.StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = $"{{\"request\":\"{mes}\"}}";
                streamWriter.Write(json);
            }

            var httpResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                return result;
            }
        }
        catch
        {
            res = "-1";
            return string.Empty;
        }
    }

    /// <summary>
    /// Объект, не дающий двум потокам одновременно использовать одну и ту же часть кода.
    /// </summary>
    private static readonly object locker = new object();
    /// <summary>
    /// Текущий поток с запросом на сервер.
    /// </summary>
    private static System.Threading.Thread thread =
        new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(Get));
    /// <summary>
    /// Текущий таймер, ограничивающий время работы thread.
    /// </summary>
    private static System.Threading.Thread thread_timer =
        new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(Timer));
    /// <summary>
    /// Проверка соединения, res = "1".
    /// </summary>
    public static void IsConnect() => GetResponse("TESTCON");
    /// <summary>
    /// Создать комнату с данным ключом генерации, вместимостью.
    /// res = "Успешно! Ваш номер комнаты x" или "Нет места".
    /// </summary>
    /// <param name="genKey">Ключ генерации</param>
    /// <param name="capacityRoom">Вместимость комнаты</param>
    public static void Create(int genKey, int capacityRoom) => GetResponse($"CREATE{sep}{genKey}{sep}{capacityRoom}");
    /// <summary>
    /// Создать комнату с данным ключом генерации, вместимостью, номером (от 0 до 99).
    /// res = "Успешно!" или "Такая комната уже существует".
    /// </summary>
    /// <param name="genKey">Ключ генерации</param>
    /// <param name="capacityRoom">Вместимость комнаты</param>
    /// <param name="indexRoom">Номер комнаты (от 0 до 99)</param>
    public static void Create(int genKey, int capacityRoom, int indexRoom)
    {
        GetResponse($"CREATEX{sep}{genKey}{sep}{capacityRoom}{sep}{indexRoom}");
        room = indexRoom;
    }
    /// <summary>
    /// Подключить игроков на данном устройстве к комнате, вместе с их именами (без спецсимволов, кавычек и '#').
    /// </summary>
    /// <param name="countPlayersOnDevice">Количество игроков на данном устройстве.</param>
    /// <param name="indexRoom">Номер комнаты (от 0 до 99).</param>
    /// <param name="names">Имена игроков (без спецсимволов, кавычек и '#').</param>
    public static void Join(int countPlayersOnDevice, int indexRoom, string names)
    {
        GetResponse($"JOIN{sep}{countPlayersOnDevice}{sep}{indexRoom}{sep}{names}");
        room = indexRoom;
    }
    /// <summary>
    /// Ожидание заполненности комнаты.
    /// res = "0" или "1".
    /// </summary>
    public static void RoomWait() => GetResponse($"RWAIT{sep}{room}");
    /// <summary>
    /// Получить имена (без спецсимволов, кавычек и '#') с сервера. res = имена.
    /// </summary>
    public static void Names() => GetResponse($"NAMES{sep}{room}");
    /// <summary>
    /// Получить ключ генерации с сервера. res = ключ генерации.
    /// </summary>
    public static void GenKey() => GetResponse($"PARMS{sep}{room}");
    /// <summary>
    /// Удалить текущую комнату. res = "0".
    /// </summary>
    public static void Delete() => GetResponse($"DELETE{sep}{room}");
    /// <summary>
    /// Удалить комнату с данным номером (от 0 до 99). res = "0".
    /// </summary>
    /// <param name="indexRoom">Номер комнаты (от 0 до 99).</param>
    public static void Delete(int indexRoom) => GetResponse($"DELETE{sep}{indexRoom}");
    /// <summary>
    /// Очистить комнату. res = "0".
    /// </summary>
    public static void Clear() => GetResponse($"CLEAR{sep}{room}");
    /// <summary>
    /// Очистить комнату с данным номером (от 0 до 99). res = "0".
    /// </summary>
    /// <param name="indexRoom">Номер комнаты (от 0 до 99).</param>
    public static void Clear(int indexRoom) => GetResponse($"CLEAR{sep}{indexRoom}");
    /// <summary>
    /// Удалённо сохранить строку (без спецсимволов, кавычек и '#') в текущей комнате. res = "0" или "1".
    /// </summary>
    /// <param name="str">Строка (без спецсимволов, кавычек и '#').</param>
    public static void Set(string str) => GetResponse($"SET{sep}{room}{sep}{str}");
    /// <summary>
    /// Получить сохранённую строку (без спецсимволов, кавычек и '#') из текущей комнаты. res = сохр.строка или "1".
    /// </summary>
    public static void Get() => GetResponse($"GET{sep}{room}");
    /// <summary>
    /// Ожидание, пока все игроки получат результат последнего хода. res = "0" или "1".
    /// </summary>
    public static void Wait() => GetResponse($"WAIT{sep}{room}{sep}{index}");
    /// <summary>
    /// Проверка существования комнаты с данным номером (от 0 до 99). (булев ответ) res = "0" или "1".
    /// </summary>
    /// <param name="indexRoom">Номер комнаты (от 0 до 99).</param>
    public static void Exists(int indexRoom) => GetResponse($"EXISTS{sep}{indexRoom}");
    /// <summary>
    /// Запуск запроса на сервер в потоке, время работы которого ограниченно таймером.
    /// </summary>
    /// <param name="mes">Сообщение на сервер.</param>
    private static void GetResponse(string mes)
    {
        try
        {
            res = "";
            if (thread_timer.IsAlive) thread_timer.Abort();
            thread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(Get));
            thread_timer = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(Timer));
            thread.Start(mes);
            thread_timer.Start(thread);
        }
        catch
        {
            res = "-1";
        }
    }
    /// <summary>
    /// Таймер, ограничивающий работу потока.
    /// </summary>
    /// <param name="th">Поток, который должен отключиться по таймеру.</param>
    private static void Timer(object th)
    {
        System.Threading.Thread.Sleep(tick);
        if (thread.IsAlive && thread.Equals(th))
        {
            thread.Abort();
            res = "-1";
        }
    }
    /// <summary>
    /// Обработка ответа сервера на запрос.
    /// </summary>
    /// <param name="mes">Сообщение на сервер.</param>
    private static void Get(object mes)
    {
        lock (locker)
        {
            try
            {
                string s = (string)mes;
                string response = Post(s).Trim('\"');
                if (response == string.Empty) res = "-1";
                else
                {
                    if (s.StartsWith($"CREATE{sep}"))
                    {
                        string[] split = response.Split();
                        int len = split.Length;
                        int.TryParse(split[len - 1], out room);
                    }
                    else if (s.StartsWith($"JOIN{sep}"))
                        int.TryParse(response, out index);
                    res = response;
                }
                
            }
            catch { res = "-1"; }
        }
    }
}
