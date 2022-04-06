using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void VoidFunc();
public delegate void VoidFunc_Int(int n);
public delegate void VoidFunc_String(string s);
public delegate void VoidFunc_Int_String(int n, string s);

public class Pair : System.IComparable
{
    public int val1;
    public int val2;

    public Pair(int Val1, int Val2)
    {
        val1 = Val1;
        val2 = Val2;
    }

    public static bool operator<(Pair p1, Pair p2)
    {
        if (p1.val1 == p2.val1)
            return p1.val2 < p2.val2;
        else
            return p1.val1 < p2.val2;
    }

    public static bool operator>(Pair p1, Pair p2)
    {
        if (p1.val1 == p2.val1)
            return p1.val2 > p2.val2;
        else
            return p1.val1 > p2.val2;
    }

    public static bool operator==(Pair p1, Pair p2)
    {
        if (p1.val1 == p2.val1)
            return p1.val2 == p2.val2;
        else
            return false;
    }

    public static bool operator!=(Pair p1, Pair p2)
    {
        if (p1.val1 == p2.val1)
            return p1.val2 != p2.val2;
        else
            return true;
    }

    public static bool operator <=(Pair p1, Pair p2)
    {
        if (p1.val1 == p2.val1)
            return p1.val2 <= p2.val2;
        else
            return p1.val1 <= p2.val2;
    }

    public static bool operator >=(Pair p1, Pair p2)
    {
        if (p1.val1 == p2.val1)
            return p1.val2 >= p2.val2;
        else
            return p1.val1 >= p2.val2;
    }

    public int CompareTo(object o)
    {
        Pair p = o as Pair;
        if (this < p)
            return -1;
        if (this == p)
            return 0;
        if (this > p)
            return 1;
        return 0;
    }
}

public class Array<T>
{
    private T[] massive;
    private int capacity;
    private int size;
    public Array(int Size = 0)
    {
        size = Size;
        massive = new T[size];
        capacity = size;
    }

    void Resize(int new_size)
    {
        if (new_size < 0)
            return;
        while (new_size > capacity)
            IncreaseCapacity();
        size = new_size;
    }

    void IncreaseCapacity()
    {
        if (capacity == 0)
        {
            massive = new T[1];
            capacity = 1;
            return;
        }
        T[] mass = massive;
        massive = new T[massive.Length * 2];
        capacity *= 2;
        for (int i = 0; i < mass.Length; ++i)
            massive[i] = mass[i];
    }

    public void Set(int i, T s)
    {
        if (i >= size || i < 0)
            return;
        massive[i] = s;
    }

    public T Get(int i)
    {
        return massive[i];
    }

    public T this[int i]
    {
        get
        {
            return massive[i];
        }
        set
        {
            if (i >= size || i < 0)
                return;
            massive[i] = value;
        }
    }

    public void Push_back(T s)
    {
        Resize(size + 1);
        massive[size - 1] = s;
    }

    public T Back()
    {
        return massive[size-1];
    }

    public void Erase(int i) // O(size)
    {
        if (i < 0 || i >= size)
            return;
        for (int j = i + 1; j < size; ++j)
        {
            massive[j - 1] = massive[j];
        }
        Resize(size - 1);
    }

    public int Size()
    {
        return size;
    }
}

public class Node_dll<T>
{
    public Node_dll<T> left;
    public Node_dll<T> right;
    public T value;

    public Node_dll(Node_dll<T> Left, Node_dll<T> Right, T Value)
    {
        left = Left;
        right = Right;
        value = Value;
    }

    public Node_dll(Node_dll<T> Left, Node_dll<T> Right)
    {
        left = Left;
        right = Right;
    }

    public Node_dll()
    {
    }
}

public class DoublyLinkedList<T>
{
    int size = 0;
    Node_dll<T> begin = new Node_dll<T>();
    Node_dll<T> end = new Node_dll<T>();

    public DoublyLinkedList()
    {
        begin.left = null;
        begin.right = end;

        end.left = begin;
        end.right = null;
    }

    public int Size()
    {
        return size;
    }

    public void Push_back(T val)
    {
        Node_dll<T> node = new Node_dll<T>(end.left, end, val);
        end.left.right = node;
        end.left = node;
        ++size;
    }

    public T Back()
    {
        return end.left.value;
    }

    public void Pop_back()
    {
        if (size > 0)
        {
            end.left.left.right = end;
            end.left = end.left.left;
            size--;
        }
    }

    public void Push_front(T val)
    {
        Node_dll<T> node = new Node_dll<T>(begin, begin.right, val);
        begin.right.left = node;
        begin.right = node;
        ++size;
    }

    public T Front()
    {
        return begin.right.value;
    }

    public void Pop_front()
    {
        if (size > 0)
        {
            begin.right.right.left = begin;
            begin.right = begin.right.right;
            size--;
        }
    }
}

public struct Coord : System.ICloneable
{
    public int x;
    public int y;
    public Coord(int X, int Y)
    {
        x = X;
        y = Y;
    }

    public static bool operator ==(Coord cor1, Coord cor2)
    {
        return (cor1.x == cor2.x && cor1.y == cor2.y);
    }
    public static bool operator !=(Coord cor1, Coord cor2)
    {
        return (cor1.x != cor2.x || cor1.y != cor2.y);
    }


    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

public struct Wall : System.ICloneable
{
    public Coord place;
    public int type; // 0-horizontal, 1-vertical
    public bool have;
    // wall stay in down and left sides in square 

    public Wall(Coord Place, int Type, bool Have)
    {
        place = Place;
        type = Type;
        have = Have;
    }

    public bool getHave()
    {
        return have;
    }
    public void add() => have = true;
    public void destroy() => have = false;


    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

public struct LabMap : System.ICloneable
{
    public int size;
    public Wall[,,] walls; 
    public Coord hospital;
    public Coord arsenal; 
    public Coord[] teleport;


    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

public struct GamePosition : System.ICloneable
{
    public GamePosition(GamePosition gp)
    {
        index = gp.index;
        numberOfTreasures = gp.numberOfTreasures;
        numberOfPlayers = gp.numberOfPlayers;
        players = new Player[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; ++i)
            players[i] = (Player)gp.players[i].Clone();
        arsRecharge = gp.arsRecharge;
        arsNum = gp.arsNum;
        arsSupply = gp.arsSupply;
        treasure = new Coord[numberOfTreasures];
        for (int i = 0; i < numberOfTreasures; ++i)
            treasure[i] = (Coord)gp.treasure[i].Clone();
        trNum = new int[numberOfTreasures];
        for (int i = 0; i < numberOfTreasures; ++i)
            trNum[i] = gp.trNum[i];
    }
    public int index;
    public int numberOfTreasures;
    public int numberOfPlayers;
    public Player[] players;
    public int arsRecharge;
    public int arsNum;
    public int arsSupply;
    public Coord[] treasure;
    public int[] trNum;


    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

public struct GameTransition : System.ICloneable
{
    public string type;
    public string side;
    public string answer;

    public bool arsenal;


    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

public static class Check
{
    public static int bullets(int id)
    {
        return Base.currentPosition.players[id].bullets;
    }

    public static int knifes(int id)
    {
        return Base.currentPosition.players[id].knifes;
    }

    public static int armors(int id)
    {
        return Base.currentPosition.players[id].armors;
    }

    public static int treasureOut(int id)
    {
        return Base.currentPosition.players[id].treasureOut;
    }

    public static int treasures(int id)
    {
        return Base.currentPosition.players[id].treasures;
    }

    public static int crackers(int id)
    {
        return Base.currentPosition.players[id].crackers;
    }

    //__________________________________ читы
    public static Coord position(int id)
    {
        return Base.currentPosition.players[id].location;
    }

    public static bool wall(int side, int x, int y) //side: 0 == down wall, 1 == left wall of (x,y)
    {
        return Base.map.walls[side, x, y].getHave();
    }

    public static Coord positionOfTeleport(int num) // number of teleport in pair
    {
        return Base.map.teleport[num];
    }

}

public struct Player : System.ICloneable
{
    public string name;
    public Coord location;
    public int bullets;
    public int knifes;
    public int crackers;
    public int armors;
    public int treasureOut;
    public int treasures;

    public bool stunned;

    public Bot bot;

    public string actionType;
    public string actionSide;
    public string actionAnswer;

    public Player(string Name, Coord Location, int type = 0, int Bullets = 0, int Knifes = 1,
        int Crackers = 0, int Armors = 0, int TreasureOut = 0)
    {
        name = Name;
        location = Location;
        bot = null;
        switch (type)
        {
            case 0: bot = null; break;
            case 1: bot = new Bot_Alice(); break;
            case 2: bot = new Bot_Bob(); break;
            case 3: bot = new Bot_Jam(); break;
        }
        stunned = false;
        bullets = Bullets;
        knifes = Knifes;
        crackers = Crackers;
        armors = Armors;
        treasureOut = TreasureOut;
        actionType = "";
        actionSide = "";
        actionAnswer = "";
        treasures = 0;
    }
    

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

public struct PlayerText
{
    public GameObject statsList;
    public TMPro.TextMeshPro nameText;
    public TMPro.TextMeshPro knifeText;
    public TMPro.TextMeshPro bulletText;
    public TMPro.TextMeshPro crackerText;
    public TMPro.TextMeshPro armorText;
    public TMPro.TextMeshPro treasureText;
}

public struct InitData
{
    public int size;
    public int numberOfPlayers;
    public int numberOfTreasures;
    public bool teleportFlag;
    public int key;

    public InitData(int Size, int NumberOfPlayers, int NumberOfTreasures, bool TeleportFlag, int Key)
    {
        size = Size;
        numberOfPlayers = NumberOfPlayers;
        numberOfTreasures = NumberOfTreasures;
        teleportFlag = TeleportFlag;
        key = Key;
    }
}


public static class Base
{
    public static int size;
    public static int numberOfPlayers;
    public static int numberOfTreasures;
    public static bool teleportFlag;
    public static PlayerText[] playersText;
    public static LabMap map;
    public static GamePosition currentPosition;
    public static GameTransition currentTransition;
    public static Array<GamePosition> positions;
    public static Array<GameTransition> transitions;
    public static System.Random gameRandom;


    public static GameObject strikeBut, fireBut, throwBut, actType, actSide;
    public static bool endOfGame = false;

    public static GameObject[] actList = new GameObject[5];
    public static int[] results;
    public static bool is_tournament;
    public static bool is_fresh_res;
    public static int[] tourTreasures;
    public static int[] tourPoints;
    public static string[] tourNames;
    public static int[] tourTypes;
    public static int tourNum;

    public static Array<string> savedTournaments = new Array<string>(0);


    public static Main main = GameObject.Find("Main").gameObject.GetComponent<Main>();
    //  int Size, int NumberOfPlayers, int NumberOfTreasures, bool TeleportFlag,
    //  string[] Name, int[] PlayerType, int key

    public static string ConvertDataToString(int Size, int NumberOfPlayers, int NumberOfTreasures, bool TeleportFlag, int key, string delimiter = "\n")
    {
        string answer;
        answer = Size.ToString() + delimiter +
            NumberOfPlayers.ToString() + delimiter +
            NumberOfTreasures.ToString() + delimiter +
            TeleportFlag.ToString() + delimiter +
            key.ToString() + delimiter;

        return answer;
    }

    public static InitData ConvertStringToData(string code, string delimiter = "\n")
    {
        string s;

        int Size;
        s = code.Substring(0, code.IndexOf(delimiter) - 1);
        code = code.Substring(code.IndexOf(delimiter) + 1);
        Size = int.Parse(s);

        int NumberOfPlayers;
        s = code.Substring(0, code.IndexOf(delimiter) - 1);
        code = code.Substring(code.IndexOf(delimiter) + 1);
        NumberOfPlayers = int.Parse(s);

        int NumberOfTreasures;
        s = code.Substring(0, code.IndexOf(delimiter) - 1);
        code = code.Substring(code.IndexOf(delimiter) + 1);
        NumberOfTreasures = int.Parse(s);

        bool TeleportFlag;
        s = code.Substring(0, code.IndexOf(delimiter) - 1);
        code = code.Substring(code.IndexOf(delimiter) + 1);
        TeleportFlag = bool.Parse(s);

        int Key;
        s = code.Substring(0, code.IndexOf(delimiter) - 1);
        code = code.Substring(code.IndexOf(delimiter) + 1);
        Key = int.Parse(s);


        return new InitData(Size, NumberOfPlayers, NumberOfTreasures, TeleportFlag, Key);
    }


    public static void RecountTreasures()
    {
        for (int i = 0; i < numberOfPlayers; ++i)
        {
            currentPosition.players[i].treasures = 0;
        }
        for (int i = 0; i < numberOfTreasures; ++i)
        {
            if (currentPosition.trNum[i] != -1 && currentPosition.trNum[i] != numberOfPlayers)
                currentPosition.players[currentPosition.trNum[i]].treasures++;
        }
    }

    public static void UpdateStats()
    {
        for (int i = 0; i < numberOfPlayers; ++i)
        {
            playersText[i].nameText.text =
                currentPosition.players[i].name + " [" + currentPosition.players[i].treasureOut + "]";
            playersText[i].knifeText.text =
                "knifes: " + currentPosition.players[i].knifes.ToString();
            playersText[i].bulletText.text =
                "bullets: " + currentPosition.players[i].bullets.ToString();
            playersText[i].crackerText.text =
                "crackers: " + currentPosition.players[i].crackers.ToString();
            playersText[i].armorText.text =
                "armors: " + currentPosition.players[i].armors.ToString();
            playersText[i].treasureText.text =
                "treasures: " + currentPosition.players[i].treasures.ToString();
        }
    }


    public static Coord Up(Coord cor)
    {
        if (!map.walls[0, cor.x, cor.y + 1].getHave())
            return new Coord(cor.x, cor.y + 1);
        return cor;
    }
    public static Coord Right(Coord cor)
    {
        if (!map.walls[1, cor.x + 1, cor.y].getHave())
            return new Coord(cor.x + 1, cor.y);
        return cor;
    }
    public static Coord Down(Coord cor)
    {
        if (!map.walls[0, cor.x, cor.y].getHave())
            return new Coord(cor.x, cor.y - 1);
        return cor;
    }
    public static Coord Left(Coord cor)
    {
        if (!map.walls[1, cor.x, cor.y].getHave())
            return new Coord(cor.x - 1, cor.y);
        return cor;
    }

    public static void Dfs(Coord cor, ref bool[,] used)
    {
        used[cor.x, cor.y] = true;
        Coord cor1;
        cor1 = Up(cor);
        if (cor1.y < size && !used[cor1.x, cor1.y])
            Dfs(cor1, ref used);
        cor1 = Right(cor);
        if (cor1.x < size && !used[cor1.x, cor1.y])
            Dfs(cor1, ref used);
        cor1 = Down(cor);
        if (cor1.y > 0 && !used[cor1.x, cor1.y])
            Dfs(cor1, ref used);
        cor1 = Left(cor);
        if (cor1.x > 0 && !used[cor1.x, cor1.y])
            Dfs(cor1, ref used);
    }

    public static bool Connected()
    {
        bool[,] used = new bool[size, size];
        for (int i = 0; i < size; ++i)
        {
            for (int j = 0; j < size; ++j)
            {
                used[i, j] = false;
            }
        }

        Dfs(new Coord(0, 0), ref used);
        bool answer = true;
        for (int i = 0; i < size; ++i)
        {
            for (int j = 0; j < size; ++j)
            {
                answer = answer && used[i, j];
            }
        }

        return answer;
    }

    public static void NormalizeButtons()
    {
        if (currentPosition.players[currentPosition.index].knifes == 0)
            strikeBut.GetComponent<Button>().Block();
        else
            strikeBut.GetComponent<Button>().Unlock();
        if (currentPosition.players[currentPosition.index].bullets == 0)
            fireBut.GetComponent<Button>().Block();
        else
            fireBut.GetComponent<Button>().Unlock();
        if (currentPosition.players[currentPosition.index].crackers == 0)
            throwBut.GetComponent<Button>().Block();
        else
            throwBut.GetComponent<Button>().Unlock();
    }

    public static void InitMap(int Size, int key_map)
    {
        System.Random random = new System.Random(key_map);

        currentPosition.index = 0;
        map = new LabMap();
        size = Size;
        map.size = Size;
        map.walls = new Wall[2, size + 1, size + 1];
        { // walls initialization 0 == horizontal, 1 == vertical
            for (int i = 0; i <= size; ++i)
            {
                for (int j = 0; j <= size; ++j)
                {
                    map.walls[0, i, j] = new Wall(new Coord(i, j), 0, false);
                    map.walls[1, i, j] = new Wall(new Coord(i, j), 1, false);
                }
            }
            for (int i = 0; i < size; ++i)
            {
                map.walls[0, i, 0].add();
                map.walls[0, i, size].add();
                map.walls[1, 0, i].add();
                map.walls[1, size, i].add();
            }
            if (random.Next(0, 2) == 0)
            {
                int rx = random.Next(0, size);
                int ry = random.Next(0, 2) * size;
                map.walls[0, rx, ry].destroy();
            }
            else
            {
                int rx = random.Next(0, 2) * size;
                int ry = random.Next(0, size);
                map.walls[1, rx, ry].destroy();
            }

            int w = 0; // счётчик постройки стен

            while (w < size * (size - 1))
            {
                if (random.Next(0, 2) == 0)
                {
                    int rx = random.Next(0, size);
                    int ry = random.Next(1, size);
                    map.walls[0, rx, ry].add();
                    if (!Connected())
                        map.walls[0, rx, ry].destroy();
                    w++;
                }
                else
                {
                    int rx = random.Next(1, size);
                    int ry = random.Next(0, size);
                    map.walls[1, rx, ry].add();
                    if (!Connected())
                        map.walls[1, rx, ry].destroy();
                    w++;
                }
            }

        }
    }

    public static void InitTeleports(bool TeleportFlag, int key_teleport)
    {
        System.Random random = new System.Random(key_teleport);
        teleportFlag = TeleportFlag;
        if (!teleportFlag)
            return;
        map.teleport = new Coord[2];
        map.teleport[0] = new Coord(random.Next(0, size), random.Next(0, size));
        int[] mass = new int[size * size];
        for (int i = 0; i < size; ++i)
            for (int j = 0; j < size; ++j)
            {
                mass[i * size + j] = 1;
                if (new Coord(i - 1, j) == map.teleport[0])
                    mass[i * size + j] = 0;
                if (new Coord(i + 1, j) == map.teleport[0])
                    mass[i * size + j] = 0;
                if (new Coord(i, j - 1) == map.teleport[0])
                    mass[i * size + j] = 0;
                if (new Coord(i, j + 1) == map.teleport[0])
                    mass[i * size + j] = 0;
                if (new Coord(i, j) == map.teleport[0])
                    mass[i * size + j] = 0;
            }
        int t = MassRand(mass, random.Next());
        map.teleport[1] = new Coord(t / size, t % size);
    }

    public static void InitTreasures(int NumberOfTreasures, int key_treasures)
    {
        System.Random random = new System.Random(key_treasures);
        
        numberOfTreasures = NumberOfTreasures;

        currentPosition.treasure = new Coord[numberOfTreasures];
        currentPosition.trNum = new int[numberOfTreasures];
        for (int i = 0; i < numberOfTreasures; ++i)
        {
            currentPosition.treasure[i] = new Coord(random.Next(0, size), random.Next(0, size));
            currentPosition.trNum[i] = numberOfPlayers;
        }
    }

    public static void Initialisation(int Size, int NumberOfPlayers, int NumberOfTreasures, bool TeleportFlag,
        string[] Name, int[] PlayerType, int key)
    {
        gameRandom = new System.Random(key);
        if (NumberOfTreasures == 0)
            NumberOfTreasures = gameRandom.Next(1, 100 + 1);
        if (Size < 2)
            Size = gameRandom.Next(2, 11);

        positions = new Array<GamePosition>(0);
        transitions = new Array<GameTransition>(0);
        currentPosition = new GamePosition();
        currentTransition = new GameTransition();
        currentTransition.arsenal = false;
        currentPosition.numberOfPlayers = NumberOfPlayers;
        currentPosition.numberOfTreasures = NumberOfTreasures;

        InitMap(Size, gameRandom.Next());

        map.hospital = new Coord(gameRandom.Next(0, size), gameRandom.Next(0, size));

        map.arsenal = new Coord(gameRandom.Next(0, size), gameRandom.Next(0, size));
        currentPosition.arsRecharge = 5;
        currentPosition.arsNum = 0;
        currentPosition.arsSupply = MassRand(new int[] { 20, 10, 6, 5 }, gameRandom.Next());

        InitTeleports(TeleportFlag, gameRandom.Next());

        numberOfPlayers = NumberOfPlayers;
        currentPosition.players = new Player[numberOfPlayers];
        playersText = new PlayerText[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; ++i)
        {
            currentPosition.players[i] =
                new Player(Name[i], new Coord(gameRandom.Next(0, size), gameRandom.Next(0, size)), PlayerType[i]);
            if (currentPosition.players[i].bot != null)
                currentPosition.players[i].bot.Join(numberOfPlayers, numberOfTreasures, size, i);
        }

        numberOfTreasures = NumberOfTreasures;
        InitTreasures(NumberOfTreasures, gameRandom.Next());

        Coord[] player_pos = new Coord[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; ++i)
        {
            player_pos[i] = currentPosition.players[i].location;
        }
        Coord[] treasure_pos = new Coord[numberOfTreasures];
        for (int i = 0; i < numberOfTreasures; ++i)
        {
            treasure_pos[i] = currentPosition.treasure[i];
        }

        playersText = new PlayerText[numberOfPlayers];
        positions.Push_back(new GamePosition(currentPosition));
    }

    public static void SaveResults()
    {
        results = new int[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; ++i)
        {
            results[i] = currentPosition.players[i].treasureOut;
        }
    }

    public static void NewTournament(int k)
    {
        is_tournament = true;
        is_fresh_res = false;
        tourTreasures = new int[k];
        tourPoints = new int[k];
        tourNames = new string[k];
        tourTypes = new int[k];
        for (int i = 0; i < k; ++i)
        {
            tourTreasures[i] = 0;
            tourPoints[i] = 0;
        }
        tourNum = 0;
    }

    public static int MassRand(int[] mass)
    {
        int s = 0;
        for (int i = 0; i < mass.Length; ++i)
            s += mass[i];
        int r = Random.Range(0, s);
        int a = 0;
        while (r >= mass[a])
        {
            r -= mass[a];
            ++a;
        }
        return a;
    }

    public static int MassRand(int[] mass, int key)
    {
        System.Random random = new System.Random(key);

        int s = 0;
        for (int i = 0; i < mass.Length; ++i)
            s += mass[i];
        int r = random.Next(0, s);
        int a = 0;
        while (r >= mass[a])
        {
            r -= mass[a];
            ++a;
        }
        return a;
    }

    public static Coord Move(Coord cor, string side)
    {
        Coord ans = new Coord(0, 0);
        if (side == "up")
            ans = Up(cor);
        if (side == "down")
            ans = Down(cor);
        if (side == "right")
            ans = Right(cor);
        if (side == "left")
            ans = Left(cor);
        return ans;
    }

    public static void OutTreasures()
    {
        for (int i = 0; i < numberOfTreasures; ++i)
        {
            if (currentPosition.trNum[i] == currentPosition.index)
            {
                currentPosition.trNum[i] = -1;
                currentPosition.players[currentPosition.index].treasureOut++;
            }
        }
    }

    public static Coord Teleporting(Coord cor)
    {
        if (teleportFlag)
        {
            if (cor == map.teleport[0])
            {
                cor = map.teleport[1];
            }
            else if (cor == map.teleport[1])
            {
                cor = map.teleport[0];
            }
            return cor;
        }
        return cor;
    }

    public static bool GetArsenalSupply()
    {
        if (currentPosition.arsRecharge >= 5)
        {
            currentPosition.arsRecharge = 0;
            currentPosition.arsNum = (currentPosition.index + 1) % numberOfPlayers;
            switch (currentPosition.arsSupply)
            {
                case 0:
                    currentPosition.players[currentPosition.index].actionAnswer += "knife\n";
                    currentPosition.players[currentPosition.index].knifes++;
                    break;
                case 1:
                    currentPosition.players[currentPosition.index].actionAnswer += "bullet\n";
                    currentPosition.players[currentPosition.index].bullets++;
                    break;
                case 2:
                    currentPosition.players[currentPosition.index].actionAnswer += "armor\n";
                    currentPosition.players[currentPosition.index].armors++;
                    break;
                case 3:
                    currentPosition.players[currentPosition.index].actionAnswer += "cracker\n";
                    currentPosition.players[currentPosition.index].crackers++;
                    break;
            }
            currentPosition.arsSupply = MassRand(new int[] { 20, 10, 6, 5 }, gameRandom.Next());
            return true;
        }
        return false;
    }

    public static int GetTreasures(Coord cor)
    {
        int trAdd = 0;
        for (int i = 0; i < numberOfTreasures; ++i)
        {
            if (currentPosition.treasure[i] == cor && currentPosition.trNum[i] == numberOfPlayers)
            {
                currentPosition.trNum[i] = currentPosition.index;
                trAdd++;
            }
        }
        return trAdd;
    }

    public static int CountOfTreasures(Coord cor)
    {
        int trCnt = 0;
        for (int i = 0; i < numberOfTreasures; ++i)
        {
            if (currentPosition.treasure[i] == cor && currentPosition.trNum[i] == numberOfPlayers)
            {
                trCnt++;
            }
        }
        return trCnt;
    }

    public static void MoveOwnTreasures(Coord cor)
    {
        for (int i = 0; i < numberOfTreasures; ++i)
        {
            if (currentPosition.trNum[i] == currentPosition.index)
                currentPosition.treasure[i] = cor;
        }
    }

    public static bool HavePlayers(Coord cor)
    {
        for (int i = 0; i < numberOfPlayers; ++i)
            if (cor == currentPosition.players[i].location)
                return true;
        return false;
    }

    public static void LoseTreasures(int id)
    {
        for (int j = 0; j < numberOfTreasures; ++j)
        {
            if (currentPosition.trNum[j] == id)
                currentPosition.trNum[j] = numberOfPlayers;
        }
        currentPosition.players[id].treasures = 0;
    }

    public static void Kill(int id)
    {
        if (currentPosition.players[id].armors > 0)
        {
            currentPosition.players[id].armors--;
        }
        else
        {
            currentPosition.players[id].location = map.hospital;
            LoseTreasures(id);
        }
    }

    public static bool Attack(Coord cor)
    {
        bool ans = false;
        for (int i = 0; i < numberOfPlayers; ++i)
        {
            if (cor == currentPosition.players[i].location)
            {
                ans = true;
                Kill(i);
            }
        }
        return ans;
    }

    public static bool Stune(Coord cor)
    {
        bool ans = false;
        for (int i = 0; i < numberOfPlayers; ++i)
        {
            if (cor == currentPosition.players[i].location)
            {
                ans = true;
                currentPosition.players[currentPosition.index].stunned = true;
                LoseTreasures(i);
            }
        }
        return ans;
    }

    public static string RandSide()
    {
        switch (gameRandom.Next(0, 4))
        {
            case 0: return "up";
            case 1: return "right";
            case 2: return "down";
            case 3: return "left";
        }
        return "";
    }

    public static void Step(string side)
    {
        Coord cor = Move(currentPosition.players[currentPosition.index].location, side);

        if (currentPosition.players[currentPosition.index].location == cor)
        {
            currentPosition.players[currentPosition.index].actionAnswer += "wall\n";
        }
        else if (cor.x == -1 || cor.x == size ||
            cor.y == -1 || cor.y == size)
        {
            OutTreasures();
            currentPosition.players[currentPosition.index].actionAnswer += "exit\n";
        }
        else
        {
            bool actGo = true;
            cor = Teleporting(cor);
            if (cor == map.arsenal)
            {
                if (GetArsenalSupply())
                {
                    actGo = false;
                    currentTransition.arsenal = true;
                }
            }
            {
                int trAdd = GetTreasures(cor);

                if (trAdd > 0)
                {
                    currentPosition.players[currentPosition.index].actionAnswer += "treasure " + trAdd.ToString() + "\n";
                    actGo = false;
                }

            }
            if (actGo)
                currentPosition.players[currentPosition.index].actionAnswer += "go\n";
            MoveOwnTreasures(cor);
            currentPosition.players[currentPosition.index].location = cor;
        }

    }

    public static void Fire(string side)
    {
        currentPosition.players[currentPosition.index].bullets--;
        Coord bullet = currentPosition.players[currentPosition.index].location;
        Coord cor = bullet;
        while (true)
        {
            if (bullet == cor)
                cor = Move(cor, side);

            if (cor == bullet || cor.x == -1 || cor.x == size ||
                cor.y == -1 || cor.y == size)
            {
                currentPosition.players[currentPosition.index].actionAnswer += "miss\n";
                break;
            }
            else
            {
                bool kill = Attack(cor);

                if (kill)
                {
                    currentPosition.players[currentPosition.index].actionAnswer += "hit\n";
                    break;
                }

                bullet = cor;
                cor = Teleporting(cor);
            }
        }
    }

    public static void Throw(string side)
    {
        currentPosition.players[currentPosition.index].crackers--;
        Coord cracker = currentPosition.players[currentPosition.index].location;
        Coord cor = cracker;
        int dist = 2;
        while (dist-- > 0)
        {
            cor = Move(cor, side);

            if (cor == cracker || cor.x == -1 || cor.x == size ||
                cor.y == -1 || cor.y == size)
            {
                if (Stune(cor))
                    currentPosition.players[currentPosition.index].actionAnswer += "hit\n";
                else
                    currentPosition.players[currentPosition.index].actionAnswer += "miss\n";
                break;
            }
            else
            {
                bool kill = Stune(cor);

                if (kill)
                {
                    currentPosition.players[currentPosition.index].actionAnswer += "hit\n";
                    break;
                }

                cracker = cor;
                cor = Teleporting(cor);
                {
                    if (Stune(cor))
                        currentPosition.players[currentPosition.index].actionAnswer += "hit\n";
                    else
                        currentPosition.players[currentPosition.index].actionAnswer += "miss\n";
                    break;
                }
            }
        }
        if (dist < 0)
            currentPosition.players[currentPosition.index].actionAnswer += "miss\n";
    }

    public static void Strike(string side)
    {
        Coord cor = Move(currentPosition.players[currentPosition.index].location, side);
        if (cor == currentPosition.players[currentPosition.index].location)
        {
            currentPosition.players[currentPosition.index].actionAnswer += "wall\n";
            currentPosition.players[currentPosition.index].knifes--;
        }
        else
        {
            bool kill = Attack(cor);
            if (kill)
            {
                currentPosition.players[currentPosition.index].actionAnswer += "hit\n";
                currentPosition.players[currentPosition.index].knifes--;
            }
            else
            {
                currentPosition.players[currentPosition.index].actionAnswer += "miss\n";
            }
        }
    }

    public static void Next()
    {
        string fictionSide = currentPosition.players[currentPosition.index].actionSide;
        if (currentPosition.players[currentPosition.index].stunned)
        {
            currentPosition.players[currentPosition.index].stunned = false;
            currentPosition.players[currentPosition.index].actionSide = RandSide();
        }
        if (currentPosition.players[currentPosition.index].actionType == "step")
            Step(currentPosition.players[currentPosition.index].actionSide);
        if (currentPosition.players[currentPosition.index].actionType == "strike")
            Strike(currentPosition.players[currentPosition.index].actionSide);
        if (currentPosition.players[currentPosition.index].actionType == "fire")
            Fire(currentPosition.players[currentPosition.index].actionSide);
        if (currentPosition.players[currentPosition.index].actionType == "throw")
            Throw(currentPosition.players[currentPosition.index].actionSide);

        currentTransition.type = currentPosition.players[currentPosition.index].actionType;
        currentTransition.side = currentPosition.players[currentPosition.index].actionSide;
        currentTransition.answer = currentPosition.players[currentPosition.index].actionAnswer;



        RecountTreasures();

        actList[0].GetComponent<TMPro.TextMeshPro>().text =
            currentPosition.players[currentPosition.index].name + ": " + currentPosition.players[currentPosition.index].actionType + " " + fictionSide   + "\n" +
            currentPosition.players[currentPosition.index].actionAnswer;

        UpdateStats();

        for (int i = 4; i > 0; --i)
        {
            actList[i].GetComponent<TMPro.TextMeshPro>().text =
                actList[i - 1].GetComponent<TMPro.TextMeshPro>().text;
        }


        int s = 0;
        for (int i = 0; i < numberOfPlayers; ++i)
            s += currentPosition.players[i].treasureOut;
        if (s == numberOfTreasures)
        {
            endOfGame = true;
        }

        for (int i = 0; i < numberOfPlayers; ++i)
        {
            if (currentPosition.players[i].bot != null)
                currentPosition.players[i].bot.Update(currentPosition.players[currentPosition.index].actionType, currentPosition.players[currentPosition.index].actionSide,
            currentPosition.players[currentPosition.index].actionAnswer, currentPosition.index);
        }

        transitions.Push_back((GameTransition)currentTransition.Clone());
        currentTransition.arsenal = false;

        currentPosition.index = (currentPosition.index + 1) % numberOfPlayers;
        GameObject.Find("Play").GetComponent<Play>().SweepPlayerStats(currentPosition.index - GameObject.Find("Play").GetComponent<Play>().menuLeftPl - 1);
        actList[0].GetComponent<TMPro.TextMeshPro>().text = currentPosition.players[currentPosition.index].name + ": ";
        currentPosition.players[currentPosition.index].actionAnswer = "";
        currentPosition.players[currentPosition.index].actionType = "";
        currentPosition.players[currentPosition.index].actionSide = "";
        if (currentPosition.index == currentPosition.arsNum)
            ++currentPosition.arsRecharge;
        positions.Push_back(new GamePosition(currentPosition));

        NormalizeButtons();
        if (currentPosition.players[currentPosition.index].bot != null)
        {
            actType.SetActive(false);
            actSide.SetActive(false);
        }
        else
        {
            actType.SetActive(true);
            actSide.SetActive(false);
        }
    }
}

public static class MyFunctions
{
    /// <summary> returns val % mod in range [0, mod) </summary>
    public static int TrueMod(int val, int mod)
    {
        return (val % mod + mod) % mod;
    }

}

public enum Scene
{
    MENU = 0,
    SETTINGS,
    PLAY,
    TOUR_PLAY,
    END,
    TOUR_END,
    INITIALIZATION,
    TOUR_INITIALIZATION,
    MAP,
    RULES,
    MP_MENU,
    MP_JOIN,
    MP_PLAY,
    MAX_SCENE
}

public class Main : MonoBehaviour
{
    public Color[] std_color;

    public GameObject music;
    public GameObject background;
    //__________________________________
    [System.Obsolete]
    IEnumerator Transition()
    {
        /*        for (int i = 0; i < 96; ++i)
                {
                    Company.color -= new Color(0, 0, 0, 1f / 192);
                    yield return new WaitForSeconds(1f / 96);
                }
        */
        yield return new WaitForSeconds(1.5f);
        background.SetActive(true);
        Application.LoadLevel("Menu");
    }

    //__________________________________
    public string GetChar()
    {
        bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        if (Input.GetKeyDown(KeyCode.A))
        { if (shift) return "A"; return "a"; }
        if (Input.GetKeyDown(KeyCode.B))
        { if (shift) return "B"; return "b"; }
        if (Input.GetKeyDown(KeyCode.C))
        { if (shift) return "C"; return "c"; }
        if (Input.GetKeyDown(KeyCode.D))
        { if (shift) return "D"; return "d"; }
        if (Input.GetKeyDown(KeyCode.E))
        { if (shift) return "E"; return "e"; }
        if (Input.GetKeyDown(KeyCode.F))
        { if (shift) return "F"; return "f"; }
        if (Input.GetKeyDown(KeyCode.G))
        { if (shift) return "G"; return "g"; }
        if (Input.GetKeyDown(KeyCode.H))
        { if (shift) return "H"; return "h"; }
        if (Input.GetKeyDown(KeyCode.I))
        { if (shift) return "I"; return "i"; }
        if (Input.GetKeyDown(KeyCode.J))
        { if (shift) return "J"; return "j"; }
        if (Input.GetKeyDown(KeyCode.K))
        { if (shift) return "K"; return "k"; }
        if (Input.GetKeyDown(KeyCode.L))
        { if (shift) return "L"; return "l"; }
        if (Input.GetKeyDown(KeyCode.M))
        { if (shift) return "M"; return "m"; }
        if (Input.GetKeyDown(KeyCode.N))
        { if (shift) return "N"; return "n"; }
        if (Input.GetKeyDown(KeyCode.O))
        { if (shift) return "O"; return "o"; }
        if (Input.GetKeyDown(KeyCode.P))
        { if (shift) return "P"; return "p"; }
        if (Input.GetKeyDown(KeyCode.Q))
        { if (shift) return "Q"; return "q"; }
        if (Input.GetKeyDown(KeyCode.R))
        { if (shift) return "R"; return "r"; }
        if (Input.GetKeyDown(KeyCode.S))
        { if (shift) return "S"; return "s"; }
        if (Input.GetKeyDown(KeyCode.T))
        { if (shift) return "T"; return "t"; }
        if (Input.GetKeyDown(KeyCode.U))
        { if (shift) return "U"; return "u"; }
        if (Input.GetKeyDown(KeyCode.V))
        { if (shift) return "V"; return "v"; }
        if (Input.GetKeyDown(KeyCode.W))
        { if (shift) return "W"; return "w"; }
        if (Input.GetKeyDown(KeyCode.X))
        { if (shift) return "X"; return "x"; }
        if (Input.GetKeyDown(KeyCode.Y))
        { if (shift) return "Y"; return "y"; }
        if (Input.GetKeyDown(KeyCode.Z))
        { if (shift) return "Z"; return "z"; }
        if (Input.GetKeyDown(KeyCode.Return))
            return "return";
        if (Input.GetKeyDown(KeyCode.Backspace))
            return "delete";
        return "";
    }

    public int GetInt()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            return 1;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            return 2;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            return 3;
        if (Input.GetKeyDown(KeyCode.Alpha4))
            return 4;
        if (Input.GetKeyDown(KeyCode.Alpha5))
            return 5;
        if (Input.GetKeyDown(KeyCode.Alpha6))
            return 6;
        if (Input.GetKeyDown(KeyCode.Alpha7))
            return 7;
        if (Input.GetKeyDown(KeyCode.Alpha8))
            return 8;
        if (Input.GetKeyDown(KeyCode.Alpha9))
            return 9;
        if (Input.GetKeyDown(KeyCode.Alpha0))
            return 0;
        if (Input.GetKeyDown(KeyCode.Return))
            return 10;
        if (Input.GetKeyDown(KeyCode.Backspace))
            return 11;
        return 100; // null code
    }

    //__________________________________
    public void SaveMassiveOfInt(string Name, int[] Massive)
    {
        PlayerPrefs.SetInt(Name + "_size", Massive.Length);
        for (int i = 0; i < Massive.Length; ++i)
        {
            PlayerPrefs.SetInt(Name + "_int_" + i.ToString(), Massive[i]);
        }
    }
    public void SaveMassiveOfString(string Name, string[] Massive)
    {
        PlayerPrefs.SetInt(Name + "_size", Massive.Length);
        for (int i = 0; i < Massive.Length; ++i)
        {
            PlayerPrefs.SetString(Name + "_string_" + i.ToString(), Massive[i]);
        }
    }
    public void SaveMassiveOfFloat(string Name, float[] Massive)
    {
        PlayerPrefs.SetInt(Name + "_size", Massive.Length);
        for (int i = 0; i < Massive.Length; ++i)
        {
            PlayerPrefs.SetFloat(Name + "_float_" + i.ToString(), Massive[i]);
        }
    }

    public int[] LoadMassiveOfInt(string Name)
    {
        int size = PlayerPrefs.GetInt(Name + "_size");
        int[] Massive = new int[size];
        for (int i = 0; i < size; ++i)
        {
            Massive[i] = PlayerPrefs.GetInt(Name + "_int_" + i.ToString());
        }
        return Massive;
    }
    public string[] LoadMassiveOfString(string Name)
    {
        int size = PlayerPrefs.GetInt(Name + "_size");
        string[] Massive = new string[size];
        for (int i = 0; i < size; ++i)
        {
            Massive[i] = PlayerPrefs.GetString(Name + "_string_" + i.ToString());
        }
        return Massive;
    }
    public float[] LoadMassiveOfFloat(string Name)
    {
        int size = PlayerPrefs.GetInt(Name + "_size");
        float[] Massive = new float[size];
        for (int i = 0; i < size; ++i)
        {
            Massive[i] = PlayerPrefs.GetFloat(Name + "_float_" + i.ToString());
        }
        return Massive;
    }

    public void EraseMassiveOfInt(string Name)
    {
        int size = PlayerPrefs.GetInt(Name + "_size");
        PlayerPrefs.DeleteKey(Name + "_size");
        for (int i = 0; i < size; ++i)
        {
            PlayerPrefs.DeleteKey(Name + "_int_" + i.ToString());
        }
    }
    public void EraseMassiveOfString(string Name)
    {
        int size = PlayerPrefs.GetInt(Name + "_size");
        PlayerPrefs.DeleteKey(Name + "_size");
        for (int i = 0; i < size; ++i)
        {
            PlayerPrefs.DeleteKey(Name + "_string_" + i.ToString());
        }
    }
    public void EraseMassiveOfFloat(string Name)
    {
        int size = PlayerPrefs.GetInt(Name + "_size");
        PlayerPrefs.DeleteKey(Name + "_size");
        for (int i = 0; i < size; ++i)
        {
            PlayerPrefs.DeleteKey(Name + "_float_" + i.ToString());
        }
    }
    //__________________________________
    public int n = 2, k = 2, t = 1; // size, players, treasures
    public bool f_tp = true;
    public int volume = 100;

    public void LoadGameSettings()
    {
        if (PlayerPrefs.HasKey("size_of_labyrinth"))
            n = PlayerPrefs.GetInt("size_of_labyrinth");
        if (PlayerPrefs.HasKey("number_of_players"))
            k = PlayerPrefs.GetInt("number_of_players");
        if (PlayerPrefs.HasKey("number_of_treasures"))
            t = PlayerPrefs.GetInt("number_of_treasures");
        if (PlayerPrefs.HasKey("have_teleports"))
            f_tp = (PlayerPrefs.GetInt("have_teleports") == 1);
    }

    public void SaveGameSettings(int N, int K, int T, bool F_tp)
    {
        PlayerPrefs.SetInt("size_of_labyrinth", N);
        PlayerPrefs.SetInt("number_of_players", K);
        PlayerPrefs.SetInt("number_of_treasures", T);
        if (F_tp)
            PlayerPrefs.SetInt("have_teleports", 1);
        else
            PlayerPrefs.SetInt("have_teleports", 0);
    }

    public void SetGameSettings(int N, int K, int T, bool F_tp)
    {
        n = N;
        k = K;
        t = T;
        f_tp = F_tp;
    }

    public void LoadSoundSettings()
    {
        if (PlayerPrefs.HasKey("volumeOfMusic"))
            volume = PlayerPrefs.GetInt("volumeOfMusic");
        music.GetComponent<AudioSource>().volume = volume * 0.01f;
    }

    public void SaveSoundSettings(int Volume)
    {
        volume = Volume;
        music.GetComponent<AudioSource>().volume = volume * 0.01f;
        PlayerPrefs.SetInt("volumeOfMusic", Volume);
    }

    public void LoadTournamentList()
    {
        string[] tourList = LoadMassiveOfString("TourList");

        Base.savedTournaments = new Array<string>(tourList.Length);
        for (int i = 0; i < Base.savedTournaments.Size(); ++i)
        {
            Base.savedTournaments[i] = tourList[i];
        }
    }

    public void SaveTournamentList()
    {
        string[] tourList = new string[Base.savedTournaments.Size()];
        for (int i = 0; i < Base.savedTournaments.Size(); ++i)
        {
            tourList[i] = Base.savedTournaments[i];
        }
        SaveMassiveOfString("TourList", tourList);
    }

    public void SaveTournament(string Name)
    {
        SaveMassiveOfInt(Name + "_tourTreasures", Base.tourTreasures);
        SaveMassiveOfInt(Name + "_tourPoints", Base.tourPoints);
        SaveMassiveOfInt(Name + "_tourTypes", Base.tourTypes);
        SaveMassiveOfString(Name + "_tourNames", Base.tourNames);

        bool yes = false;
        for (int i = 0; i < Base.savedTournaments.Size(); ++i)
        {
            if (Base.savedTournaments[i] == Name)
                yes = true;
        }
        if (!yes)
            Base.savedTournaments.Push_back(Name);
        SaveTournamentList();
    }
    public void LoadTournament(string Name)
    {
        Base.tourTreasures = LoadMassiveOfInt(Name + "_tourTreasures");
        Base.tourPoints = LoadMassiveOfInt(Name + "_tourPoints");
        Base.tourTypes = LoadMassiveOfInt(Name + "_tourTypes");
        Base.tourNames = LoadMassiveOfString(Name + "_tourNames");
    }
    public void EraseTournament(string Name)
    {
        EraseMassiveOfInt(Name + "_tourTreasures");
        EraseMassiveOfInt(Name + "_tourPoints");
        EraseMassiveOfInt(Name + "_tourTypes");
        EraseMassiveOfString(Name + "_tourNames");

        for (int i = 0; i < Base.savedTournaments.Size(); ++i)
        {
            if (Base.savedTournaments[i] == Name)
                Base.savedTournaments.Erase(i);
        }
        SaveTournamentList();
    }
    //__________________________________
    public Scene currentScene = Scene.MAX_SCENE;
    public Scene lastScene = Scene.MAX_SCENE;
    private string[] sceneName = { "Menu", "Settings", "Play", "TourPlay", "End",
        "TourEnd", "Initialization", "TourInitialization", "Map", "Rules", "MP_Menu",
        "MP_Join", "MP_Play"};

    [System.Obsolete]
    public void OnScene(Scene scene)
    {
        lastScene = currentScene;
        currentScene = scene;
        InputText.act_id = -1;
        Application.LoadLevel(sceneName[(int)scene]);
    }

    void Start()
    {
        currentScene = Scene.MENU;
        StartCoroutine("Transition");
        LoadGameSettings();
        LoadSoundSettings();
        LoadTournamentList();
//        Resolution[] resolutions = Screen.resolutions;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
