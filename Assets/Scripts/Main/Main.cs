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

public class Bot
{
    protected bool broken;
    public bool bot_show;
    protected System.Random rand = new System.Random();
    protected int Players, Treasures, Size, my_id;
    public string ansType, ansSide;
    public virtual void Join(int the_players, int the_treasures, int the_size, int the_id)
    {
        ansType = "step";
        ansSide = Int_to_ansSide(rand.Next(4));
        try
        {
            Players = the_players;
            Treasures = the_treasures;
            Size = the_size;
            my_id = the_id;
            players = new player[Players];
            for (int i = 0; i < Players; ++i)
            {
                players[i].A = new Map(2 * Size); players[i].B = new Map(2 * Size);
                players[i].id = i;
            }
            my_map = new Map(2 * Size);
            hosp_map = new Map(2 * Size);
            UpdateStats();
        }
        catch
        {
            broken = true;
        }
    }
    public int k, choice;
    protected string Int_to_ansSide(int q)
    {
        string[] s = { "left", "down", "right", "up" };
        return s[q];
    }
    protected int AnsSide_to_int(string m)
    {
        string[] s = { "left", "down", "right", "up" };
        for (int i = 0; i < s.Length; ++i) if (m == s[i]) return i;
        return -1;
    }
    protected virtual int Can(int a, int b, int k)
    {
        return my_map.Can(a, b, k);
    }
    protected virtual bool HaveWall_or_Exit(int a, int b)
    {
        return (Can(a, b, 0) < 0 || Can(a, b, 1) < 0 || Can(a, b, 2) < 0 || Can(a, b, 3) < 0);
    }
    protected virtual bool Have0(int a, int b)
    {
        return (Can(a, b, 0) == 0 || Can(a, b, 1) == 0 || Can(a, b, 2) == 0 || Can(a, b, 3) == 0);
    }
    protected virtual bool Have1(int a, int b)
    {
        return (Can(a, b, 0) == 1 || Can(a, b, 1) == 1 || Can(a, b, 2) == 1 || Can(a, b, 3) == 1);
    }
    protected virtual bool HaveArs()
    {
        return (knifes != Check.knifes(my_id) || bullets != Check.bullets(my_id) || armors != Check.armors(my_id) || crackers != Check.crackers(my_id));
    }
    protected int[] v = new int[2];
    protected int[][] notExpl = new int[11][];
    protected int[,][] p;
    protected int[][] path;
    protected bool[,] used;
    protected int path_size, notExpl_size;
    protected virtual void BFS()
    {
        NewBFS();
        Queue<int[]> q = new Queue<int[]>();
        int[] s = { my_map.x, my_map.y };
        q.Enqueue(s);
        used[s[0], s[1]] = true;
        int[] null_ = { -1, -1 };
        p[s[0], s[1]] = null_;
        while (q.Count > 0)
        {
            int[] v = q.Dequeue(); int z = v[0], t = v[1];
            for (int i = 0; i < 4; ++i)
            {
                int dx = 0, dy = 0; if (i == 0) dx = -1; if (i == 1) dy = -1; if (i == 2) dx = 1; if (i == 3) dy = 1;
                bool Can = this.Can(z, t, i) == 1;
                int[] to = { z + dx, t + dy };
                if (Can && !used[to[0], to[1]])
                {
                    used[to[0], to[1]] = true;
                    q.Enqueue(to);
                    p[to[0], to[1]] = v;
                    if (Have0(to[0], to[1]) && notExpl_size < 10)
                    { ++notExpl_size; notExpl[notExpl_size] = to; }
                }
            }
        }
    }
    protected virtual void BFS(int[,] a)
    {
        NewBFS();
        Queue<int[]> q = new Queue<int[]>();
        int[] s = { my_map.x, my_map.y };
        q.Enqueue(s);
        used[s[0], s[1]] = true;
        int[] null_ = { -1, -1 };
        p[s[0], s[1]] = null_;
        while (q.Count > 0 && notExpl_size < 1)
        {
            int[] v = q.Dequeue(); int z = v[0], t = v[1];
            for (int i = 0; i < 4 && notExpl_size < 1; ++i)
            {
                int dx = 0, dy = 0; if (i == 0) dx = -1; if (i == 1) dy = -1; if (i == 2) dx = 1; if (i == 3) dy = 1;
                bool Can = this.Can(z, t, i) == 1;
                int[] to = { z + dx, t + dy };
                if (Can && !used[to[0], to[1]])
                {
                    used[to[0], to[1]] = true;
                    q.Enqueue(to);
                    p[to[0], to[1]] = v;
                    if (a[to[0], to[1]] > 0 && notExpl_size < 1)
                        if (Check.treasures(a[to[0], to[1]] - 1) > 0)
                        { ++notExpl_size; notExpl[notExpl_size] = to; }
                }
            }
        }
    }
    protected virtual void NewBFS()
    {
        used = new bool[2 * Size, 2 * Size];
        p = new int[2 * Size, 2 * Size][];
        notExpl = new int[11][];
        notExpl_size = 0;
    }
    protected virtual void Path(int a, int b)
    {
        path = new int[Size * Size + 1][];
        path_size = 0;
        int[] to = { a, b };
        for (int[] v = to; v[0] != -1; v = p[v[0], v[1]])
        {
            ++path_size;
            path[path_size] = v;
        }
    }
    protected virtual void NewDFS()
    {
        used = new bool[2 * Size, 2 * Size];
    }
    protected virtual void GoToV()
    {
        int x = my_map.x, y = my_map.y;
        if (knifes > 0 && rand.Next(5) == 0 && Have1(x, y)) { ansType = "strike"; ansSide = F(1); choice = 0; }
        else if (bullets > 0 && rand.Next(15) == 0 && Have1(x, y)) { ansType = "fire"; ansSide = F(1); choice = 0; }
        else
        {
            --path_size;
            int t = -1;
            if (path[path_size] != null)
            {
                int a = path[path_size][0], b = path[path_size][1];
                if (a == x - 1) t = 0; if (b == y - 1) t = 1; if (a == x + 1) t = 2; if (b == y + 1) t = 3;
                ansType = "step"; ansSide = Int_to_ansSide(t);
                if (v[0] == a && v[1] == b) choice = 0;
            }
            else
            {
                ansType = "step"; ansSide = Int_to_ansSide(rand.Next(4));
                choice = 0;
            }
        }
    }
    protected virtual void UpdateAns()
    {
        UpdateStats();
        if (IsJam()) bullets = 0;
        if (choice == 0) UpdateChoice();
        if (choice == 1 || choice == 5) GoToV();
        if (choice == 2 || choice == 3) { ansType = "step"; ansSide = Int_to_ansSide(k); }
        if (choice == 4)
        {
            if (knifes > 0) { ansType = "strike"; ansSide = (Have1(my_map.x, my_map.y)) ? F(1) : F(0); }
            else { ansType = "step"; ansSide = F(-1) != "" ? F(-1) : (F(-2) != "" ? F(-2) : Int_to_ansSide(rand.Next(4))); }
        }
        if (IsJam()) bullets = Check.bullets(my_id);
    }
    protected virtual void UpdateChoice()
    {
        int x = my_map.x, y = my_map.y;
        if (knifes > 0 && rand.Next(5) == 0 && Have1(x, y)) { ansType = "strike"; ansSide = F(1); }
        else if (bullets > 0 && rand.Next(15) == 0 && Have1(x, y)) { ansType = "fire"; ansSide = F(1); }
        else
        {
            BFS(); ansType = "step"; int t = Max((Treasures - SumOut()) / 2 - treasures, 1);
            if (x == my_map.exit[0] && y == my_map.exit[1] && treasures > 0) { ansSide = Int_to_ansSide(my_map.exit[2]); }
            else if (treasures > 0 && rand.Next(t) == 0 && (my_map.exit[0] > -1 ? p[my_map.exit[0], my_map.exit[1]] != null : false))
            {
                v[0] = my_map.exit[0]; v[1] = my_map.exit[1];
                Path(v[0], v[1]);
                choice = 1;
            }
            else if (Have0(x, y)) ansSide = F(0);
            else if (notExpl_size > 0)
            {
                t = rand.Next(rand.Next(notExpl_size)) + 1;
                v[0] = notExpl[t][0]; v[1] = notExpl[t][1];
                Path(v[0], v[1]);
                choice = 1;
            }
            else ansSide = F(1);
        }
    }
    protected virtual int Max(int a, int b)
    {
        return (a > b) ? a : b;
    }
    protected virtual int Min(int a, int b)
    {
        return (a < b) ? a : b;
    }
    protected virtual int SumOut()
    {
        int sum = 0;
        for (int i = 0; i < Players; ++i) sum += Check.treasureOut(i);
        return sum;
    }
    protected virtual string F(int w)
    {
        int isW(int q) { if (q == w) return 1; else return 0; }
        int i = my_map.x, j = my_map.y;
        int sum = isW(Can(i, j, 0)) + isW(Can(i, j, 1)) + isW(Can(i, j, 2)) + isW(Can(i, j, 3)), t;
        if (sum != 0)
        {
            t = rand.Next(sum) + 1;
            if (t == 1)
            {
                if (Can(i, j, 0) == w) return "left";
                if (Can(i, j, 1) == w) return "down";
                if (Can(i, j, 2) == w) return "right";
                if (Can(i, j, 3) == w) return "up";
            }
            if (t == 2)
            {
                if (Can(i, j, 0) == w && Can(i, j, 1) == w) return "down";
                if ((Can(i, j, 0) == w || Can(i, j, 1) == w) && Can(i, j, 2) == w) return "right";
                if (Can(i, j, 3) == w) return "up";
            }
            if (t == 3)
            {
                if (Can(i, j, 0) == w && Can(i, j, 1) >= w && Can(i, j, 2) == w) return "right";
                if (Can(i, j, 3) == w) return "up";
            }
            if (t == 4)
                if (Can(i, j, 3) == w) return "up";
        }
        return "";
    }
    protected virtual int GameAns(string s)
    {
        if (s == "wall\n") return 0;
        else if (s == "exit\n" || s == "hit\n") return 2;
        else return 1;
    }
    public struct Map
    {
        public int size;
        public int rank;
        public int x, y;
        public int minx, miny, maxx, maxy;
        public int[,,] can_to_move;
        public int[] exit;
        public Map(int t)
        {
            size = t;
            minx = maxx = miny = maxy = x = y = size / 2 - 1;
            rank = 0;
            can_to_move = new int[size, size, 2];
            exit = new int[3];
            exit[0] = -1;
        }
        public void Move(int k)
        {
            if (k == 0) { if (x == minx) --minx; --x; }
            if (k == 1) { if (y == miny) --miny; --y; }
            if (k == 2) { if (x == maxx) ++maxx; ++x; }
            if (k == 3) { if (y == maxy) ++maxy; ++y; }
        }
        public int Can(int a, int b, int k)
        {
            int dx = (k == 2 ? 1 : 0), dy = (k == 3 ? 1 : 0), t = (k > 1) ? dy : k;
            return can_to_move[a + dx, b + dy, t];
        }
        public void UpdateCoord(int a, int b)
        {
            x = a; y = b;
        }
        public void UpdateCan(int res, int k)
        {
            int dx = (k == 2 ? 1 : 0), dy = (k == 3 ? 1 : 0), t = (k > 1) ? dy : k;
            can_to_move[x + dx, y + dy, t] = res;
            rank += res > 0 ? res : -res;
        }
        public void UpdateCan(int a, int b, int res, int k)
        {
            int dx = (k == 2 ? 1 : 0), dy = (k == 3 ? 1 : 0), t = (k > 1) ? dy : k;
            can_to_move[a + dx, b + dy, t] = res;
            rank += res > 0 ? res : -res;
        }
        public void UpdateExit(int k)
        {
            exit[0] = x; exit[1] = y; exit[2] = k; UpdateBorders();
        }
        public void UpdateExit(int a, int b, int k)
        {
            exit[0] = a; exit[1] = b; exit[2] = k; UpdateBorders();
        }
        void AddWall(int a, int b, int k)
        {
            if (can_to_move[a, b, k] == 0) UpdateCan(a, b, -1, k);
        }
        public void UpdateBorders()
        {
            if (exit[0] > -1)
            {
                int a = exit[0], b = exit[1], k = exit[2], Size = size / 2;
                bool A = (k == 0 && (a < 0 || a >= 2 * Size || a + Size < 0 || a + Size >= 2 * Size));
                bool B = (k == 1 && (b < 0 || b >= 2 * Size || b + Size < 0 || b + Size >= 2 * Size));
                bool C = (k == 2 && (a + 1 < 0 || a + 1 >= 2 * Size || a + 1 - Size < 0 || a + 1 - Size >= 2 * Size));
                bool D = (k == 3 && (b + 1 < 0 || b + 1 >= 2 * Size || b + 1 - Size < 0 || b + 1 - Size >= 2 * Size));
                if (A || B || C || D) exit[0] = -2;
                else
                {
                    if (k == 0 && maxy - miny == Size - 1)
                    {
                        for (int i = a; i < a + Size; ++i) { AddWall(i, miny, 1); AddWall(i, miny + Size, 1); }
                        for (int j = miny; j <= maxy; ++j) { AddWall(a, j, 0); AddWall(a + Size, j, 0); }
                    }
                    if (k == 1 && maxx - minx == Size - 1)
                    {
                        for (int i = minx; i <= maxx; ++i) { AddWall(i, b, 1); AddWall(i, b + Size, 1); }
                        for (int j = b; j < b + Size; ++j) { AddWall(minx, j, 0); AddWall(minx + Size, j, 0); }
                    }
                    if (k == 2 && maxy - miny == Size - 1)
                    {
                        for (int i = a + 1 - Size; i < a + 1; ++i) { AddWall(i, miny, 1); AddWall(i, miny + Size, 1); }
                        for (int j = miny; j <= maxy; ++j) { AddWall(a + 1 - Size, j, 0); AddWall(a + 1, j, 0); }
                    }
                    if (k == 3 && maxx - minx == Size - 1)
                    {
                        for (int i = minx; i <= maxx; ++i) { AddWall(i, b + 1 - Size, 1); AddWall(i, b + 1, 1); }
                        for (int j = b + 1 - Size; j < b + 1; ++j) { AddWall(minx, j, 0); AddWall(minx + Size, j, 0); }
                    }
                }
            }
        }
        public void NewLife()
        {
            minx = maxx = miny = maxy = x = y = size / 2 - 1;
            can_to_move = new int[size, size, 2];
            exit[0] = -1;
            rank = 0;
        }
    }
    public Map hosp_map, my_map;
    protected bool aftHosp;
    protected virtual Map High(Map A, Map B)
    {
        if (A.rank < B.rank) return Equating(A, B); else return A;
    }
    protected virtual Map Add(Map A, Map B, int a, int b)
    {
        A.UpdateBorders(); B.UpdateBorders();
        for (int i = B.minx - B.x + a; i <= B.maxx + 1 - B.x + a; ++i)
            for (int j = B.miny - B.y + b; j <= B.maxy + 1 - B.y + b; ++j)
            {
                if (A.x + i < 0 || A.x + i >= 2 * Size || A.y + j < 0 || A.y + j >= 2 * Size)
                { GetInfoB(); return my_map; }
                if (A.can_to_move[A.x + i, A.y + j, 0] == 0)
                    A.UpdateCan(A.x + i, A.y + j, B.can_to_move[B.x - a + i, B.y - b + j, 0], 0);
                if (A.can_to_move[A.x + i, A.y + j, 1] == 0)
                    A.UpdateCan(A.x + i, A.y + j, B.can_to_move[B.x - a + i, B.y - b + j, 1], 1);
            }
        if (B.exit[0] > -1)
        {
            A.exit[0] = A.x + B.exit[0] - (B.x - a);
            A.exit[1] = A.y + B.exit[1] - (B.y - b);
            A.exit[2] = B.exit[2];
        }
        NewDFS();
        void DFS(int m, int n)
        {
            used[m, n] = true;
            if (m < A.minx) A.minx = m; if (m > A.maxx) A.maxx = m;
            if (n < A.miny) A.miny = n; if (n > A.maxy) A.maxy = n;
            if (A.Can(m, n, 0) == 1) if (!used[m - 1, n]) DFS(m - 1, n);
            if (A.Can(m, n, 1) == 1) if (!used[m, n - 1]) DFS(m, n - 1);
            if (A.Can(m, n, 2) == 1) if (!used[m + 1, n]) DFS(m + 1, n);
            if (A.Can(m, n, 3) == 1) if (!used[m, n + 1]) DFS(m, n + 1);
        }
        DFS(A.minx, A.miny);
        A.UpdateBorders();
        return A;
    }
    protected virtual Map Equating(Map A, Map B)
    {
        B.UpdateBorders();
        A.minx = B.minx; A.miny = B.miny;
        A.maxx = B.maxx; A.maxy = B.maxy;
        A.x = B.x; A.y = B.y; A.rank = B.rank;
        A.exit[0] = B.exit[0]; A.exit[1] = B.exit[1]; A.exit[2] = B.exit[2];
        for (int i = 0; i < A.size; ++i)
            for (int j = 0; j < A.size; ++j)
            {
                A.can_to_move[i, j, 0] = B.can_to_move[i, j, 0];
                A.can_to_move[i, j, 1] = B.can_to_move[i, j, 1];
            }
        return A;
    }
    protected virtual Map Merge(Map A, Map B)
    {
        A.UpdateBorders(); B.UpdateBorders();
        Map C = new Map(Size + 1);
        int var = 0;
        int gxa = 0, gya = 0, gxb = 0, gyb = 0;
        int dxa = (A.maxx - A.minx), dya = (A.maxy - A.miny), dxb = (B.maxx - B.minx), dyb = (B.maxy - B.miny);
        for (int ia = 0; ia < Size - dxa; ++ia)
            for (int ja = 0; ja < Size - dya; ++ja)
                for (int ib = 0; ib < Size - dxb; ++ib)
                    for (int jb = 0; jb < Size - dyb; ++jb)
                    {
                        int err = 0;
                        C = new Map(Size + 1);
                        for (int i = ia; i <= ia + dxa + 1; ++i)
                            for (int j = ja; j <= ja + dya + 1; ++j)
                            {
                                C.can_to_move[i, j, 0] = A.can_to_move[i - ia + A.minx, j - ja + A.miny, 0];
                                C.can_to_move[i, j, 1] = A.can_to_move[i - ia + A.minx, j - ja + A.miny, 1];
                            }
                        for (int i = ib; i <= ib + dxb + 1 && err == 0; ++i)
                            for (int j = jb; j <= jb + dyb + 1 && err == 0; ++j)
                            {
                                int x = B.can_to_move[i - ib + B.minx, j - jb + B.miny, 0], y = B.can_to_move[i - ib + B.minx, j - jb + B.miny, 1];
                                int z = C.can_to_move[i, j, 0], t = C.can_to_move[i, j, 1];
                                if ((x != z && x * z != 0) || (y != t && y * t != 0)) err = 1;
                                C.can_to_move[i, j, 0] = x;
                                C.can_to_move[i, j, 1] = y;
                            }
                        for (int i = 0; i < Size + 1 && err == 0; ++i)
                            for (int j = 0; j < Size + 1 && err == 0; ++j)
                                if (i == 0 && j == 0) { if (C.can_to_move[i, j, 1] == 1 || C.can_to_move[i, j, 0] == 1) err = 1; }
                                else if (i == Size) { if (C.can_to_move[i, j, 0] == 1) err = 1; }
                                else if (j == Size) { if (C.can_to_move[i, j, 1] == 1) err = 1; }
                                else if (i == 0)
                                {
                                    if (C.can_to_move[i, j, 0] == 1) err = 1;
                                    if (C.can_to_move[i, j, 1] == 0) C.can_to_move[i, j, 1] = 1;
                                }
                                else if (j == 0)
                                {
                                    if (C.can_to_move[i, j, 1] == 1) err = 1;
                                    if (C.can_to_move[i, j, 0] == 0) C.can_to_move[i, j, 0] = 1;
                                }
                                else
                                {
                                    if (C.can_to_move[i, j, 0] == 0) C.can_to_move[i, j, 0] = 1;
                                    if (C.can_to_move[i, j, 1] == 0) C.can_to_move[i, j, 1] = 1;
                                }
                        if (err != 0) continue;
                        NewDFS();
                        int DFS(int a, int b, Map Map)
                        {
                            int s = 1;
                            used[a, b] = true;
                            if (Map.Can(a, b, 0) == 1) if (!used[a - 1, b]) s += DFS(a - 1, b, Map);
                            if (Map.Can(a, b, 1) == 1) if (!used[a, b - 1]) s += DFS(a, b - 1, Map);
                            if (Map.Can(a, b, 2) == 1) if (!used[a + 1, b]) s += DFS(a + 1, b, Map);
                            if (Map.Can(a, b, 3) == 1) if (!used[a, b + 1]) s += DFS(a, b + 1, Map);
                            return s;
                        }
                        int sum = DFS(0, 0, C);
                        if (sum == Size * Size)
                        {
                            ++var; gxa = ia; gya = ja; gxb = ib; gyb = jb;
                        }
                        if (var > 1) return A;
                    }
        if (var == 1)
        {
            int x = A.minx - B.minx + gxb - gxa, y = A.miny - B.miny + gyb - gya;
            for (int i = B.minx; i <= B.maxx + 1; ++i)
                for (int j = B.miny; j <= B.maxy + 1; ++j)
                {
                    if (A.can_to_move[x + i, y + j, 0] == 0)
                        A.UpdateCan(x + i, y + j, B.can_to_move[i, j, 0], 0);
                    if (A.can_to_move[x + i, y + j, 1] == 0)
                        A.UpdateCan(x + i, y + j, B.can_to_move[i, j, 1], 1);
                }
            if (B.exit[0] > -1)
            {
                A.exit[0] = B.exit[0] + x;
                A.exit[1] = B.exit[1] + y;
                A.exit[2] = B.exit[2];
            }
            NewDFS();
            void DFS(int a, int b)
            {
                used[a, b] = true;
                if (a < A.minx) A.minx = a; if (a > A.maxx) A.maxx = a;
                if (b < A.miny) A.miny = b; if (b > A.maxy) A.maxy = b;
                if (A.Can(a, b, 0) == 1) if (!used[a - 1, b]) DFS(a - 1, b);
                if (A.Can(a, b, 1) == 1) if (!used[a, b - 1]) DFS(a, b - 1);
                if (A.Can(a, b, 2) == 1) if (!used[a + 1, b]) DFS(a + 1, b);
                if (A.Can(a, b, 3) == 1) if (!used[a, b + 1]) DFS(a, b + 1);
            }
            DFS(A.minx, A.miny);
            Spy_d(x, y);
        }
        A.UpdateBorders();
        if (var == 0 || A.exit[0] == -2) { GetInfoB(); return my_map; }
        else return A;
    }
    protected virtual bool ConflictRes(int res, int k)
    {
        return Can(my_map.x, my_map.y, k) != res && Can(my_map.x, my_map.y, k) != 0;
    }
    protected virtual bool ConflictWall()
    {
        NewDFS();
        int DFS(int a, int b)
        {
            int sum = 1;
            used[a, b] = true;
            if (Have0(a, b)) return 0;
            if (Can(a, b, 0) == 1) if (!used[a - 1, b]) { int t = DFS(a - 1, b); if (t == 0) return 0; else sum += t; }
            if (Can(a, b, 1) == 1) if (!used[a, b - 1]) { int t = DFS(a, b - 1); if (t == 0) return 0; else sum += t; }
            if (Can(a, b, 2) == 1) if (!used[a + 1, b]) { int t = DFS(a + 1, b); if (t == 0) return 0; else sum += t; }
            if (Can(a, b, 3) == 1) if (!used[a, b + 1]) { int t = DFS(a, b + 1); if (t == 0) return 0; else sum += t; }
            return sum;
        }
        int res = DFS(my_map.x, my_map.y);
        return (res > 0 && res != Size * Size);
    }
    protected virtual bool ConflictMove()
    {
        return my_map.maxx - my_map.minx >= Size || my_map.maxy - my_map.miny >= Size;
    }
    protected virtual void UpdateStats()
    {
        treasures = Check.treasures(my_id);
        treasuresOut = Check.treasureOut(my_id);
        knifes = Check.knifes(my_id);
        bullets = Check.bullets(my_id);
        armors = Check.armors(my_id);
        crackers = Check.crackers(my_id);
    }
    protected virtual void GetInfoB()
    {
        for (int i = 0; i < Players; ++i) players[i].spy = false;
        aftHosp = false;
        my_map = Equating(my_map, players[my_id].B);
        bot_show = true;
        choice = 0;
    }
    public struct player
    {
        public Map A, B;
        public int treasures, treasuresOut, knifes, bullets, armors, crackers, id;
        public bool spy;
        public int dspy_x, dspy_y;
        public int choice;
        public bool aftHosp;
        public void UpdateStats()
        {
            treasures = Check.treasures(id);
            treasuresOut = Check.treasureOut(id);
            armors = Check.armors(id);
            knifes = Check.knifes(id);
            bullets = Check.bullets(id);
            crackers = Check.crackers(id);
        }
        public int X()
        {
            return (B.x + dspy_x);
        }
        public int Y()
        {
            return (B.y + dspy_y);
        }
    }
    public player[] players;
    public virtual bool SmbLosed()
    {
        for (int i = 0; i < Players; ++i) if (players[i].treasures > Check.treasures(i)) return true;
        return false;
    }
    protected virtual void UpdateCan_and_xy(int game, int k) //only for "step"
    {
        if (choice != 4)
        {
            int res;
            if (game == 0)
            {
                res = -1;
                if (ConflictRes(res, k)) GetInfoB();
                my_map.UpdateCan(res, k);
                players[my_id].B.UpdateCan(res, k);
            }
            if (game == 1)
            {
                res = 1;
                if (ConflictRes(res, k)) GetInfoB();
                my_map.UpdateCan(res, k);
                my_map.Move(k);
                players[my_id].B.UpdateCan(res, k);
                players[my_id].B.Move(k);
            }
            if (game == 2)
            {
                res = -2;
                if (ConflictRes(res, k)) GetInfoB();
                my_map.UpdateCan(res, k);
                my_map.UpdateExit(k);
                if (my_map.exit[0] == -2) GetInfoB();
                players[my_id].B.UpdateCan(res, k);
                players[my_id].B.UpdateExit(k);
            }
            if (ConflictMove()) GetInfoB();
            if (ConflictWall()) GetInfoB();
            if (choice == 2) choice = 0;
            if (choice == 3 && (Check.treasures(my_id) > treasures || game == 0)) choice = 0;
            if (aftHosp) hosp_map = Add(hosp_map, players[my_id].B, players[my_id].B.x - hosp_map.x, players[my_id].B.y - hosp_map.y);
        }
        else
        {
            choice = 0;
            if (game == 1)
            {
                players[my_id].A = High(players[my_id].A, players[my_id].B);
                players[my_id].B.NewLife();
                GetInfoB();
            }
        }
    }
    public virtual void Update(string ansType_id, string ansSide_id, string gameAns_id, int id)
    {
        if (!broken)
        {
            try
            {
                int game = GameAns(gameAns_id); k = AnsSide_to_int(ansSide_id); bot_show = false;
                if (gameAns_id == "hit\n")
                {
                    if (id != my_id)
                    {
                        bool A = Check.treasures(my_id) == 0 && (players[my_id].B.Can(players[my_id].B.x, players[my_id].B.y, (k + 2) % 4) > -1 || players[id].choice == 4);
                        if (ansType_id != "throw")
                        {
                            if (armors == 0 && A)
                            {
                                if (treasures > 0)
                                {
                                    players[my_id].A = High(players[my_id].A, players[my_id].B);
                                    NewDFS();
                                    void DFS(int a, int b)
                                    {
                                        used[a, b] = true;
                                        if (a < hosp_map.minx) hosp_map.minx = a; if (a > hosp_map.maxx) hosp_map.maxx = a;
                                        if (b < hosp_map.miny) hosp_map.miny = b; if (b > hosp_map.maxy) hosp_map.maxy = b;
                                        if (hosp_map.Can(a, b, 0) == 1) if (!used[a - 1, b]) DFS(a - 1, b);
                                        if (hosp_map.Can(a, b, 1) == 1) if (!used[a, b - 1]) DFS(a, b - 1);
                                        if (hosp_map.Can(a, b, 2) == 1) if (!used[a + 1, b]) DFS(a + 1, b);
                                        if (hosp_map.Can(a, b, 3) == 1) if (!used[a, b + 1]) DFS(a, b + 1);
                                    }
                                    DFS(hosp_map.minx, hosp_map.miny);
                                    hosp_map = Merge(hosp_map, players[my_id].A);
                                    hosp_map.UpdateCoord(Size - 1, Size - 1);
                                    players[my_id].B = Equating(players[my_id].B, hosp_map);
                                    GetInfoB();
                                    UpdateAns();
                                    aftHosp = true;
                                }
                                else
                                {
                                    aftHosp = false;
                                    players[my_id].A = High(players[my_id].A, players[my_id].B);
                                    players[my_id].B.NewLife();
                                }
                            }
                            if (armors != Check.armors(my_id) && ansType_id == "strike" && players[id].choice != 4)
                            {
                                int dx = 0, dy = 0;
                                if (k == 0) dx = 1; if (k == 2) dx = -1;
                                if (k == 1) dy = 1; if (k == 3) dy = -1;
                                players[my_id].B = Add(players[my_id].B, players[id].B, dx, dy);
                                if (players[my_id].B.Can(players[my_id].B.x, players[my_id].B.y, (k + 2) % 4) == 0)
                                {
                                    players[my_id].B.UpdateCan(1, (k + 2) % 4);
                                }
                                my_map = Add(my_map, players[my_id].B, 0, 0);
                                if (ConflictMove()) GetInfoB();
                                if (ConflictWall()) GetInfoB();
                            }
                        }
                        else
                        {
                            if (A)
                            {
                                choice = 4;
                                UpdateAns();
                            }
                        }
                    }
                    for (int i = 0; i < Players; ++i)
                        if (i != id && i != my_id)
                        {
                            bool A = players[i].B.Can(players[i].B.x, players[i].B.y, (k + 2) % 4) > -1 || players[id].choice == 4;
                            if (ansType_id != "throw")
                            {
                                if (Check.treasures(i) == 0 && players[i].armors == 0 && A)
                                {
                                    bool B = (players[id].choice != 4 && (id != my_id || choice != 4));
                                    if (ansType_id == "strike" && players[i].treasures > 0 && B)
                                    {
                                        int dx = 0, dy = 0;
                                        if (k == 0) dx = -1; if (k == 2) dx = 1;
                                        if (k == 1) dy = -1; if (k == 3) dy = 1;
                                        players[id].B = Add(players[id].B, players[i].B, dx, dy);
                                        if (id == my_id)
                                        {
                                            my_map = Add(my_map, players[my_id].B, 0, 0);
                                            if (ConflictMove()) GetInfoB();
                                            if (ConflictWall()) GetInfoB();
                                        }
                                    }
                                    players[i].A = High(players[i].A, players[i].B);
                                    players[i].B.NewLife();
                                    if (players[i].treasures > 0) players[i].aftHosp = true;
                                    else players[i].aftHosp = false;
                                    Spy_off(i);
                                }
                            }
                            else if (A) players[i].choice = 4;
                        }
                }
                if (id == my_id)
                {
                    if (ansType_id != "step")
                    {
                        if (choice != 4)
                        {
                            if (ansType_id != "throw")
                            {
                                if (game == 0) { players[my_id].B.UpdateCan(-1, k); GetInfoB(); } //knife
                                if (game == 2 && SmbLosed())
                                {
                                    if (ansType_id == "strike") choice = 2;
                                    if (ansType_id == "fire") choice = 3;
                                }
                            }
                            else if (game == 2) choice = 4;
                        }
                        else if (ansType_id != "throw")
                        {
                            if (game == 2 && SmbLosed())
                            {
                                if (ansType_id == "strike") choice = 2;
                                if (ansType_id == "fire") choice = 3;
                            }
                            else choice = 0;
                        }
                        else if (game != 2) choice = 0;
                        else choice = 4;
                    }
                    else UpdateCan_and_xy(game, k);
                    my_map = Merge(my_map, players[my_id].A);
                    players[my_id].B = Merge(players[my_id].B, players[my_id].A);
                    players[my_id].A = Merge(players[my_id].A, players[my_id].B);
                    UpdateAns();
                }
                else
                {
                    if (gameAns_id == "hit\n")
                    {
                        int x = players[id].B.x, y = players[id].B.y;
                        if (ansType_id != "throw" && players[id].choice == 4) players[id].choice = 0;
                        if (ansType_id == "throw" && (players[id].B.Can(x, y, k) != 1 || players[id].choice == 4))
                            players[id].choice = 4;
                    }
                    else
                    {
                        if (ansType_id == "step")
                        {
                            if (players[id].choice != 4)
                            {
                                int res;
                                if (game == 0)
                                {
                                    res = -1;
                                    players[id].B.UpdateCan(res, k);
                                }
                                if (game == 1)
                                {
                                    res = 1;
                                    players[id].B.UpdateCan(res, k);
                                    players[id].B.Move(k);
                                }
                                if (game == 2)
                                {
                                    res = -2;
                                    players[id].B.UpdateCan(res, k);
                                    players[id].B.UpdateExit(k);
                                }
                                if (players[id].aftHosp)
                                {
                                    hosp_map = Add(hosp_map, players[id].B, players[id].B.x - Size + 1, players[id].B.y - Size + 1);
                                    //players[id].B = Add(players[id].B, hosp_map, Size - 1 - players[id].B.x, Size - 1 - players[id].B.y);
                                    //if (!players[id].spy) Spy_on(id);
                                }
                            }
                            else
                            {
                                players[id].choice = 0;
                                if (game == 1)
                                {
                                    players[id].A = High(players[id].A, players[id].B);
                                    players[id].B.NewLife();
                                    players[id].choice = 0;
                                    players[id].aftHosp = false;
                                    Spy_off(id);
                                }
                            }
                        }
                        else { if (players[id].choice == 4) players[id].choice = 0; }
                        players[my_id].B = Merge(players[my_id].B, players[id].A);
                        players[my_id].B = Merge(players[my_id].B, players[id].B);
                        my_map = Merge(my_map, players[id].A);
                        my_map = Merge(my_map, players[id].B);
                        Spy_on(id);
                    }
                }
                players[id].UpdateStats();
                UpdateStats();
                TryKill();
            }
            catch
            {
                broken = true;
                randomAns();
            }
        }
        else randomAns();
    }
    protected int treasures, treasuresOut, knifes, bullets, armors, crackers;
    //Jam.begin();
    protected virtual bool IsJam() { return false; }
    protected virtual void Spy_on(int id) { }
    protected virtual void Spy_off(int id) { }
    protected virtual void Spy_d(int a, int b) { }
    protected virtual void TryKill() { }
    //Jam.end();
    protected void randomAns()
    {
        int massRand(int[] mr)
        {
            int[] sum = new int[mr.Length + 1];
            sum[0] = 0;
            for (int i = 0; i < mr.Length; ++i) sum[i + 1] = sum[i] + mr[i];
            int x = rand.Next(sum[mr.Length]);
            for (int i = 0; i < mr.Length; ++i) if (sum[i] <= x && x < sum[i + 1]) return i;
            return -1;
        }
        int A = Check.knifes(my_id), B = Check.bullets(my_id), C = Check.crackers(my_id);
        int type = massRand(new int[] { A + B + C + 1, A, B, C }), side = rand.Next(4);
        if (type == 0) ansType = "step";
        else if (type == 1) ansType = "strike";
        else if (type == 2) ansType = "fire";
        else if (type == 3) ansType = "throw";
        ansSide = Int_to_ansSide(side);
    }
}
public class Bot_Alice : Bot
{
    int x, y;
    int[,,] can_to_move = new int[20, 20, 2];
    public override void Join(int the_players, int the_treasures, int the_size, int the_id)
    {
        ansType = "step";
        ansSide = Int_to_ansSide(rand.Next(4));
        try
        {
            Players = the_players;
            Treasures = the_treasures;
            Size = the_size;
            my_id = the_id;
            x = y = 9;
            players = new player[the_players];
            knifes = 1;
        }
        catch
        {
            broken = true;
        }
    }
    protected override int Can(int a, int b, int side)
    {
        int dx = (side == 2 ? 1 : 0), dy = (side == 3 ? 1 : 0); side = (side > 1) ? dy : side;
        return can_to_move[a + dx, b + dy, side];
    }
    void move(int side)
    {
        if (side == 0) --x;
        if (side == 1) --y;
        if (side == 2) ++x;
        if (side == 3) ++y;
    }
    new int[] v = new int[2];
    new int[][] notExpl = new int[11][];
    new int[,][] p = new int[20, 20][];
    new int[][] path = new int[101][];
    new bool[,] used = new bool[20, 20];
    new int path_size, notExpl_size;
    protected override void BFS()
    {
        NewBFS();
        Queue<int[]> q = new Queue<int[]>();
        int[] s = { x, y };
        q.Enqueue(s);
        used[s[0], s[1]] = true;
        int[] null_ = { -1, -1 };
        p[s[0], s[1]] = null_;
        while (q.Count > 0)
        {
            int[] v = q.Dequeue(); int z = v[0], t = v[1];
            for (int i = 0; i < 4; ++i)
            {
                int dx = 0, dy = 0; if (i == 0) dx = -1; if (i == 1) dy = -1; if (i == 2) dx = 1; if (i == 3) dy = 1;
                bool Can = this.Can(z, t, i) == 1;
                int[] to = { z + dx, t + dy };
                if (Can && !used[to[0], to[1]])
                {
                    used[to[0], to[1]] = true;
                    q.Enqueue(to);
                    p[to[0], to[1]] = v;
                    if (Have0(to[0], to[1]) && notExpl_size < 10)
                    { ++notExpl_size; notExpl[notExpl_size] = to; }
                }
            }
        }
    }
    protected override void Path(int x, int y)
    {
        path_size = 0;
        int[] to = { x, y };
        for (int[] v = to; v[0] != -1; v = p[v[0], v[1]])
        {
            ++path_size;
            path[path_size] = v;
        }
    }
    protected override void GoToV()
    {
        if (knifes > 0 && rand.Next(5) == 0 && Have1(x, y)) { ansType = "strike"; ansSide = F(1); }
        else if (bullets > 0 && rand.Next(5) == 0 && Have1(x, y)) { ansType = "fire"; ansSide = F(1); }
        else
        {
            --path_size;
            int t = -1;
            int a = path[path_size][0], b = path[path_size][1];
            if (a == x - 1) t = 0; if (b == y - 1) t = 1; if (a == x + 1) t = 2; if (b == y + 1) t = 3;
            ansType = "step"; ansSide = Int_to_ansSide(t);
            if (v[0] == a && v[1] == b) choice = 0;
        }
    }
    protected override void UpdateAns()
    {
        UpdateStats();
        if (choice == 0) UpdateChoice();
        if (choice == 1) GoToV();
        if (choice == 2)
        {
            ansType = "step"; ansSide = Int_to_ansSide(k); choice = 0;
            if (doubt)
            {
                path[path_size] = new int[2];
                path[path_size][0] = x; path[path_size][1] = y;
                ++path_size;
                choice = 1;
            }
        }
    }
    int[] exit = { -1, -1, -1 };
    protected override string F(int w)
    {
        int isW(int q) { if (q == w) return 1; else return 0; }
        int i = x, j = y;
        int sum = isW(Can(i, j, 0)) + isW(Can(i, j, 1)) + isW(Can(i, j, 2)) + isW(Can(i, j, 3)), t = rand.Next(sum) + 1;
        if (t == 1)
        {
            if (Can(i, j, 0) == w) return "left";
            if (Can(i, j, 1) == w) return "down";
            if (Can(i, j, 2) == w) return "right";
            if (Can(i, j, 3) == w) return "up";
        }
        if (t == 2)
        {
            if (Can(i, j, 0) == w && Can(i, j, 1) == w) return "down";
            if ((Can(i, j, 0) == w || Can(i, j, 1) == w) && Can(i, j, 2) == w) return "right";
            if (Can(i, j, 3) == w) return "up";
        }
        if (t == 3)
        {
            if (Can(i, j, 0) == w && Can(i, j, 1) >= w && Can(i, j, 2) == w) return "right";
            if (Can(i, j, 3) == w) return "up";
        }
        if (t == 4)
            if (Can(i, j, 3) == w) return "up";
        return "";
    }
    bool doubt;
    protected override void UpdateChoice()
    {
        bool A = (x == exit[0]) && (y == exit[1]) && doubt;
        if (knifes > 0 && rand.Next(5) == 0 && Have1(x, y) && !A) { ansType = "strike"; ansSide = F(1); }
        else if (bullets > 0 && rand.Next(5) == 0 && Have1(x, y) && !A) { ansType = "fire"; ansSide = F(1); }
        else
        {
            ansType = "step"; int t = Max((Treasures - SumOut()) / 2 - treasures, 1);
            if ((x == exit[0] && y == exit[1] && treasures > 0) || (x == exit[0] && y == exit[1] && doubt)) { ansSide = Int_to_ansSide(exit[2]); }
            else if (treasures > 0 && exit[0] > -1 && rand.Next(t) == 0)
            {
                v[0] = exit[0]; v[1] = exit[1];
                BFS();
                Path(v[0], v[1]);
                choice = 1;
            }
            else if (Have0(x, y)) ansSide = F(0);
            else
            {
                BFS();
                if (notExpl_size > 0)
                {
                    t = rand.Next(rand.Next(notExpl_size)) + 1;
                    v[0] = notExpl[t][0]; v[1] = notExpl[t][1];
                    Path(v[0], v[1]);
                    choice = 1;
                }
                else ansSide = F(1);
            }
        }
    }
    protected override int GameAns(string s)
    {
        if (s == "wall\n") return 0;
        else if (s == "exit\n" || s == "hit\n") return 2;
        else return 1;
    }
    void updateCan(int res, int k)
    {
        int dx = (k == 2 ? 1 : 0), dy = (k == 3 ? 1 : 0); k = (k > 1) ? dy : k;
        can_to_move[x + dx, y + dy, k] = res;
    }
    new struct Map
    {
        public int a, b;
        public int[,,] can;
        public int[] exit;
        public Map(int x)
        {
            a = b = -1;
            can = new int[x + 1, x + 1, 2];
            exit = new int[3];
            exit[0] = -1;
        }
        public void Coord(int x, int y)
        {
            a = x; b = y;
        }
    }
    protected override void UpdateCan_and_xy(int game, int k)
    {
        if (choice == 4)
        {
            if (game != 1) choice = 0;
            else newLife();
        }
        else
        {
            if (doubt)
            {
                if (x == exit[0] && y == exit[1] && game != 2) newLife();
                else if (game == 0) newLife();
                else if (game == 1) move(k);
                else if (game == 2) { doubt = false; x = exit[0]; y = exit[1]; choice = 0; }
                if (x == -1 || y == -1 || x == 19 || y == 19) newLife();
            }
            else
            {
                if (game == 0) { updateCan(-1, k); }
                if (game == 1) { updateCan(1, k); move(k); }
                if (game == 2) { updateCan(-1, k); exit[0] = x; exit[1] = y; exit[2] = k; }
            }
        }
    }
    void newLife()
    {
        x = y = 9;
        aftHosp = false;
        can_to_move = new int[20, 20, 2];
        choice = 0;
        exit[0] = exit[1] = exit[2] = -1;
        doubt = false;
    }
    protected override void NewBFS()
    {
        used = new bool[20, 20];
        p = new int[20, 20][];
        path = new int[101][];
        notExpl_size = path_size = 0;
    }
    new struct player
    {
        public int treasures;
    }
    new player[] players;
    public override void Update(string ansType_id, string ansSide_id, string gameAns_id, int id)
    {
        if (!broken)
        {
            try
            {
                int game = GameAns(gameAns_id); k = AnsSide_to_int(ansSide_id);
                if (id == my_id)
                {
                    if (ansType_id != "step") { if (game == 2 && SmbLosed()) choice = 2; if (game == 0 && choice != 4) newLife(); if (choice == 4) choice = 0; }
                    else UpdateCan_and_xy(game, k);
                    UpdateAns();
                }
                else
                {
                    if (gameAns_id == "hit\n")
                    {
                        if (ansType_id == "throw")
                        {
                            if (knifes > 0) { ansType = "strike"; ansSide = (Have1(x, y)) ? F(1) : F(0); }
                            else if (bullets > 0) { ansType = "fire"; ansSide = (Have1(x, y)) ? F(1) : F(0); }
                            else { ansType = "step"; ansSide = (HaveWall_or_Exit(x, y)) ? F(-1) : F(0); }
                            choice = 4;
                        }
                        else if (Check.treasures(my_id) == 0 && armors == 0)
                        {
                            if (treasures > 0 || exit[0] == -1) { newLife(); UpdateAns(); }
                            else
                            {
                                doubt = true;
                                v[0] = exit[0]; v[1] = exit[1];
                                if (x == v[0] && y == v[1]) choice = 0;
                                else
                                {
                                    BFS();
                                    Path(v[0], v[1]);
                                    choice = 1;
                                }
                                UpdateAns();
                            }
                        }
                    }
                }
                players[id].treasures = Check.treasures(id);
                UpdateStats();
            }
            catch { broken = true; randomAns(); }
        }
        else randomAns();
    }
    public override bool SmbLosed()
    {
        for (int i = 0; i < Players; ++i) if (players[i].treasures > Check.treasures(i)) return true;
        return false;
    }
}
public class Bot_Bob : Bot
{

}
public class Bot_Jam : Bot
{
    protected override bool IsJam() { return true; }
    protected int dspy_x, dspy_y, is_spy;
    protected override void Spy_on(int id)
    {
        is_spy = 0;
        my_map = Merge(my_map, players[id].B);
        if (is_spy == 1)
        {
            players[id].spy = true; players[id].dspy_x = dspy_x; players[id].dspy_y = dspy_y;
        }
    }
    protected override void Spy_off(int id)
    {
        players[id].spy = false;
    }
    protected override void Spy_d(int a, int b)
    {
        is_spy = 1; dspy_x = a; dspy_y = b;
    }
    protected override void TryKill()
    {
        players[my_id].spy = false;
        if (choice != 4 && (knifes + bullets > 0))
        {
            bool spy = false;
            bool hit = false; int profit = 0;
            for (int i = 0; i < Players; ++i)
                if (players[i].spy)
                {
                    if (players[i].treasures > 0) spy = true;
                    if (players[i].treasures > profit)
                    {
                        int x = my_map.x, y = my_map.y;
                        int xp = players[i].X(), yp = players[i].Y();
                        int dx = xp - x, dy = yp - y;
                        if (knifes > 0)
                        {
                            void Win(int xx) { ansSide = Int_to_ansSide(xx); ansType = "strike"; profit = players[i].treasures; choice = 0; hit = true; }
                            if (dx == -1 && dy == 0 && Can(x, y, 0) == 1) { Win(0); continue; }
                            if (dx == 0 && dy == -1 && Can(x, y, 1) == 1) { Win(1); continue; }
                            if (dx == 1 && dy == 0 && Can(x, y, 2) == 1) { Win(2); continue; }
                            if (dx == 0 && dy == 1 && Can(x, y, 3) == 1) { Win(3); continue; }
                        }
                        if (bullets > 0)
                        {
                            void Win(int xx) { ansSide = Int_to_ansSide(xx); ansType = "fire"; profit = players[i].treasures; choice = 0; hit = true; }
                            if (dx == 0)
                            {
                                int pos = y;
                                if (dy < 0)
                                {
                                    while (pos != yp)
                                        if (pos > -1 && pos < 2 * Size - 1)
                                            if (Can(x, pos, 1) == 1) --pos;
                                            else break;
                                        else break;
                                    if (pos == yp) { Win(1); continue; }
                                }
                                if (dy > 0)
                                {
                                    while (pos != yp)
                                        if (pos > -1 && pos < 2 * Size - 1)
                                            if (Can(x, pos, 3) == 1) ++pos;
                                            else break;
                                        else break;
                                    if (pos == yp) { Win(3); continue; }
                                }
                            }
                            if (dy == 0)
                            {
                                int pos = x;
                                if (dx < 0)
                                {
                                    while (pos != xp)
                                        if (pos > -1 && pos < 2 * Size - 1)
                                            if (Can(x, pos, 0) == 1) --pos;
                                            else break;
                                        else break;
                                    if (pos == xp) { Win(0); continue; }
                                }
                                if (dx > 0)
                                {
                                    while (pos != xp)
                                        if (pos > -1 && pos < 2 * Size - 1)
                                            if (Can(x, pos, 2) == 1) ++pos;
                                            else break;
                                        else break;
                                    if (pos == xp) { Win(2); continue; }
                                }
                            }
                        }
                    }
                }
            if (spy && !hit && choice != 2 && choice != 3 && (3 * treasures <= Treasures - SumOut() || my_map.exit[0] < 0))// && treasures==0)
            {
                int[,] posPlayer = new int[2 * Size - 1, 2 * Size - 1];
                for (int i = 0; i < Players; ++i)
                {
                    if (players[i].spy && players[i].treasures > 0)
                    {
                        int x = players[i].X(), y = players[i].Y();
                        if (x != my_map.x || y != my_map.y) posPlayer[x, y] = i + 1;
                    }
                }
                BFS(posPlayer);
                if (notExpl_size != 0)
                {
                    notExpl[1].CopyTo(v, 0);
                    Path(v[0], v[1]);
                    choice = 5;
                    UpdateAns();
                }
                else if (choice == 5) { choice = 0; UpdateAns(); }
            }
            if (!hit && ansType != "step" && choice == 0) UpdateAns();
        }
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
    public void SaveMassiveOfInt(string Name, int[]Massive)
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
    public int n = 2, k = 2, t = 1; //221
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
    public string currentScene;
    public string lastScene;
    [System.Obsolete]
    public void OnScene_Menu()
    {
        lastScene = currentScene;
        currentScene = "Menu";
        InputText.act_id = -1;
        Base.is_tournament = false;
        Application.LoadLevel("Menu");
    }

    [System.Obsolete]
    public void OnScene_Settings()
    {
        lastScene = currentScene;
        currentScene = "Settings";
        InputText.act_id = -1;
        Application.LoadLevel("Settings");
    }

    [System.Obsolete]
    public void OnScene_Play()
    {
        lastScene = currentScene;
        currentScene = "Play";
        InputText.act_id = -1;
        Application.LoadLevel("Play");
    }

    [System.Obsolete]
    public void OnScene_TourPlay()
    {
        lastScene = currentScene;
        currentScene = "TourPlay";
        InputText.act_id = -1;
        Application.LoadLevel("TourPlay");
    }

    [System.Obsolete]
    public void OnScene_End()
    {
        lastScene = currentScene;
        currentScene = "End";
        InputText.act_id = -1;
        Application.LoadLevel("End");
    }

    [System.Obsolete]
    public void OnScene_TourEnd()
    {
        lastScene = currentScene;
        currentScene = "End";
        InputText.act_id = -1;
        Application.LoadLevel("TourEnd");
    }

    [System.Obsolete]
    public void OnScene_Initialization()
    {
        lastScene = currentScene;
        currentScene = "Initialization";
        InputText.act_id = -1;
        Application.LoadLevel("Initialization");
    }

    [System.Obsolete]
    public void OnScene_TourInitialization()
    {
        lastScene = currentScene;
        currentScene = "TourInitialization";
        InputText.act_id = -1;
        Application.LoadLevel("TourInitialization");
    }

    [System.Obsolete]
    public void OnScene_Map()
    {
        lastScene = currentScene;
        currentScene = "Map";
        InputText.act_id = -1;
        Application.LoadLevel("Map");
    }

    [System.Obsolete]
    public void OnScene_Rules()
    {
        lastScene = currentScene;
        currentScene = "Rules";
        InputText.act_id = -1;
        Application.LoadLevel("Rules");
    }

    [System.Obsolete]
    public void OnScene_MP_Menu()
    {
        lastScene = currentScene;
        currentScene = "MP_Menu";
        InputText.act_id = -1;
        Application.LoadLevel("MP_Menu");
    }

    [System.Obsolete]
    public void OnScene_MP_Join()
    {
        lastScene = currentScene;
        currentScene = "MP_Join";
        InputText.act_id = -1;
        Application.LoadLevel("MP_Join");
    }

    [System.Obsolete]
    public void OnScene_MP_Play()
    {
        lastScene = currentScene;
        currentScene = "MP_Play";
        InputText.act_id = -1;
        Application.LoadLevel("MP_Play");
    }

    [System.Obsolete]
    public void OnScene(string scene)
    {
        lastScene = currentScene;
        currentScene = scene;
        InputText.act_id = -1;
        Application.LoadLevel(scene);
    }

    void Start()
    {
        currentScene = "Menu";
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
