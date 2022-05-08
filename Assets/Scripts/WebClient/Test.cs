using UnityEngine;
class Test: MonoBehaviour
{

    private void Start()
    {
        test_message();
    }

    static void wait()
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        while (Web.res == "")
            if (watch.ElapsedMilliseconds >= Web.tick + 1000)
            {
                Debug.Log("so long ... ");
                break;
            }
    }
    public static void test_message()
    {
        try
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            Web.IsConnect();
            wait();
            if (Web.res != "1")
                throw new System.ArgumentException("no connection");

            Web.Create(239,1,0);
            wait();
            Debug.Log($"{Web.res} (creating the room {Web.room})");

            Web.Join(1, Web.room, "Andrey&Sergey");
            wait();
            Debug.Log($"{Web.res} (==index, if joined to the room {Web.room})");

            Web.RoomWait();
            wait();
            Debug.Log($"{Web.res} (==1, if waiting other players in the current room)");

            string str = "Привет! Мой ход: fire&down";
            Web.Set(str);
            wait();
            Debug.Log($"{Web.res} (==0, if successfully sent string to the current room)");

            Web.Get();
            wait();
            Debug.Log($"{Web.res} (string from the current room)");

            Web.Exists(Web.room);
            wait();
            Debug.Log($"{Web.res} (==1, if the room {Web.room} exists)");

            Web.Names();
            wait();
            Debug.Log($"names: {Web.res}");

            Web.GenKey();
            wait();
            Debug.Log($"generation key: {Web.res}");

            Web.Delete();
            wait();
            Debug.Log($"{Web.res} (==0, if successfully delete the current room)");

            watch.Stop();
            Debug.Log($"time = {(int)watch.ElapsedMilliseconds} ms");
        }
        catch (System.Exception exc) { Debug.Log(exc.Message); }
    }
    public static void delete_all(int count = 100)
    {
        try
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < count; ++i)
            {
                Web.Delete(i);
                wait();
            }
            Debug.Log($"time = {(int)watch.ElapsedMilliseconds} ms");
        }
        catch { Debug.Log("ops"); }
    }
}
