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
            if (watch.ElapsedMilliseconds >= 6000)
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
            
            Web.isConnect();
            wait();
            if (Web.res != "1")
                throw new System.ArgumentException("no connection");

            Web.create(239, 1, 0);
            wait();
            Debug.Log(Web.res);

            Web.join(1, Web.room, "names");
            wait();
            Debug.Log(Web.res);

            Web.rwait();
            wait();
            Debug.Log(Web.res);

            string str = "hi!";
            Web.set(str);
            wait();
            Debug.Log(Web.res);

            Web.get();
            wait();
            Debug.Log(Web.res);

            Web.exists(Web.room);
            wait();
            Debug.Log($"room {Web.room} exists, but this free: {Web.res}");

            Web.names();
            wait();
            Debug.Log($"Web.names: {Web.res}");

            Web.parms();
            wait();
            Debug.Log($"Web.parms: {Web.res}");

            Web.delete();
            wait();
            Debug.Log(Web.res);

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
                Web.delete(i);
                wait();
            }
            Debug.Log($"time = {(int)watch.ElapsedMilliseconds} ms");
        }
        catch { Debug.Log("ops"); }
    }
}
