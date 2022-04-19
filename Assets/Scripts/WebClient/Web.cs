class Web
{
    // The ip and port number for the remote device. 
    private const string ip = "http://radiant-taiga-93891.herokuapp.com/";
    public static bool online = true;

    // Tick for thread timers
    private const int tick = 5000;

    // Separate requests
    private const string sep = "#";
    public static string post(string str)
    {
        try
        {
            // https://stackoverflow.com/questions/4015324/how-to-make-an-http-post-web-request // it's hard
            // https://stackoverflow.com/questions/9145667/how-to-post-json-to-a-server-using-c
            System.Net.HttpWebRequest httpWebRequest;
            if (online) httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(ip);
            else httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create("http://127.0.0.1:80");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new System.IO.StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = $"{{\"request\":\"{str}\"}}";
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

    // The response from the remote device.  
    private static string response = string.Empty;

    // Sync threads
    private static object locker = new object();

    private static System.Threading.Thread thread =
        new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(get));

    private static System.Threading.Thread thread_timer =
        new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(timer));


    public static string res = string.Empty;
    public static int room = 0;
    public static int index = 0;
    public static void create(int par, int n) => _getResponse($"CREATE{sep}{par}{sep}{n}");
    public static void create(int par, int n, int x)
    {
        _getResponse($"CREATEX{sep}{par}{sep}{n}{sep}{x}");
        room = x;
    }
    public static void join(int k, int x, string nam)
    {
        _getResponse($"JOIN{sep}{k}{sep}{x}{sep}{nam}");
        room = x;
    }
    public static void rwait() => _getResponse($"RWAIT{sep}{room}");
    public static void names() => _getResponse($"NAMES{sep}{room}");
    public static void parms() => _getResponse($"PARMS{sep}{room}");
    public static void delete() => _getResponse($"DELETE{sep}{room}");
    public static void delete(int x) => _getResponse($"DELETE{sep}{x}");
    public static void clear() => _getResponse($"CLEAR{sep}{room}");
    public static void clear(int x) => _getResponse($"CLEAR{sep}{x}");
    public static void set(string str) => _getResponse($"SET{sep}{room}{sep}{str}");
    public static void getResponse() => _getResponse($"GET{sep}{room}");
    public static void wait() => _getResponse($"WAIT{sep}{room}{sep}{index}");
    public static void exists(int x) => _getResponse($"EXISTS{sep}{x}");
    private static void _getResponse(string str)
    {
        try
        {
            res = "";
            if (thread_timer.IsAlive) thread_timer.Abort();
            thread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(get));
            thread_timer = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(timer));
            thread.Start(str);
            thread_timer.Start(thread);
        }
        catch
        {
            res = "-1";
        }
    }
    private static void timer(object th)
    {
        System.Threading.Thread.Sleep(tick);
        if (thread.IsAlive && thread.Equals(th))
        {
            thread.Abort();
            res = "-1";
        }
    }
    private static void get(object str)
    {
        lock (locker)
        {
            try
            {
                string s = (string)str;
                response = post(s).Trim('\"');
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
            catch { res = "-1"; }
        }
    }
}
