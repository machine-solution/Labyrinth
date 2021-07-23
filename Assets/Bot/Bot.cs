using System.Collections.Generic;
// TODO: closure-checking in ConflictWall function
/// <summary> Корень иерархии классов ботов </summary>
public class Bot {
    //constants
    protected static readonly List<string> types = new List<string> { "step", "strike", "fire", "throw" };
    protected static readonly List<string> sides = new List<string> { "left", "down", "right", "up" };
    protected const int STEP = 0, STRIKE = 1, FIRE = 2, THROW = 3;
    protected const int LEFT = 0, DOWN = 1, RIGHT = 2, UP = 3, NUMOFSIDES = 4;

    //variables
    public string ansType, ansSide;
    public bool broken;
    public static string error = "";
    protected static System.Random rand = new System.Random();

    //functions
    public virtual void Join(int players, int treasures, int size, int id) { }
    public virtual void Update(string type, string side, string result, int id) { }
}
/// <summary> Бот: version 1.5 </summary>
public class Bot_Bob: Bot {
    /// <summary> Бот: version 1.5 </summary>
    public Bot_Bob() { }
    //constants
    protected const int EXIT = -2, WALL = -1, UNKNOWN = 0, FREE = 1;
    protected const int GO = 1; //step
    protected const int MISS = 0, HIT = 1; //attack
    protected const int NO_CHOICE = 0, EXPLORE = 1, TAKE_AFTER_STRIKE = 2, TAKE_AFTER_FIRE = 3,
                        CRAZY = 4, GO_TO_KILL = 5; //choice
    protected const int MERGE_PRECISION = 2;

    //variables
    // NOT RELOAD //
    protected int Players, Treasures, Size, my_id;
    protected int treasures, treasuresOut, knifes, bullets, armors, crackers;
    // RELOAD  //
    protected int k, choice;
    protected bool aftHosp;
    public Map hosp_map, my_map;
    public Player[] players;
    //    BFS    //
    protected int[] v = new int[2];
    protected int[][] notExpl = new int[11][], notWas = new int[11][];
    protected int[,][] p;
    protected int[][] path;
    protected bool[,] used;
    protected int path_size, notExpl_size, notWas_size;
    //    Jam    //
    protected int dspy_x, dspy_y, is_spy;

    //structures and classes
    public struct Player {
        public Map A, B;
        public int treasures, treasuresOut, knifes, bullets, armors, crackers, id;
        public bool spy;
        public int dspy_x, dspy_y;
        public int choice;
        public bool aftHosp;
        public void UpdateStats() {
            treasures = Check.treasures(id);
            treasuresOut = Check.treasureOut(id);
            armors = Check.armors(id);
            knifes = Check.knifes(id);
            bullets = Check.bullets(id);
            crackers = Check.crackers(id);
        }
        public int X() => B.x + dspy_x;
        public int Y() => B.y + dspy_y;
    }
    public class Map {
        public int size;
        public int rank;
        public int x = 0, y = 0;
        public int minx, miny, maxx, maxy;
        public int[,,] can_to_move;
        public int[] exit;
        public bool[,] was;
        public void NewLife() {
            rank = 0;
            x = y = minx = maxx = miny = maxy = size / 2 - 1;
            can_to_move = new int[size, size, 2];
            exit = new int[3] { -1, -1, -1 };
            was = new bool[size, size];
        }
        void Init(int Size) {
            size = Size;
            NewLife();
        }
        public Map(int Size) => Init(Size);
        public Map copy() => new Map(this);
        Map(Map A) {
            Init(A.size);
            A.UpdateBorders();
            rank = A.rank;
            minx = A.minx; miny = A.miny;
            maxx = A.maxx; maxy = A.maxy;
            x = A.x; y = A.y;
            exit = new int[3] { A.exit[0], A.exit[1], A.exit[2] };
            for (int i = 0; i < size; ++i)
                for (int j = 0; j < size; ++j) {
                    can_to_move[i, j, 0] = A.can_to_move[i, j, 0];
                    can_to_move[i, j, 1] = A.can_to_move[i, j, 1];
                }
        }
        public void Move(int side) {
            if (side == LEFT) { if (x == minx) --minx; --x; }
            if (side == DOWN) { if (y == miny) --miny; --y; }
            if (side == RIGHT) { if (x == maxx) ++maxx; ++x; }
            if (side == UP) { if (y == maxy) ++maxy; ++y; }
            was[x, y] = true;
        }
        public int Can(int a, int b, int side) {
            int dx = (side == RIGHT ? 1 : 0), dy = (side == UP ? 1 : 0);
            return can_to_move[a + dx, b + dy, side % 2];
        }
        public void UpdateCoord(int a, int b) {
            x = a; y = b;
        }
        public void UpdateCan(int res, int side) {
            int dx = (side == RIGHT ? 1 : 0), dy = (side == UP ? 1 : 0);
            can_to_move[x + dx, y + dy, side % 2] = res;
            rank += res > 0 ? res : -res;
        }
        public void UpdateCan(int a, int b, int res, int side) {
            int dx = (side == RIGHT ? 1 : 0), dy = (side == UP ? 1 : 0);
            can_to_move[a + dx, b + dy, side % 2] = res;
            rank += res > 0 ? res : -res;
        }
        public void UpdateExit(int side) {
            exit = new int[3] { x, y, side }; UpdateBorders();
        }
        public void UpdateExit(int a, int b, int side) {
            exit = new int[3] { a, b, side }; UpdateBorders();
        }
        public void AddWall(int a, int b, int side) {
            if (can_to_move[a, b, side] == UNKNOWN) UpdateCan(a, b, WALL, side);
        }
        public void AddFree(int a, int b, int side) {
            if (can_to_move[a, b, side] == UNKNOWN) UpdateCan(a, b, FREE, side);
        }
        public void UpdateBorders() {
            if (exit[0] > -1) {
                int a = exit[0], b = exit[1], side = exit[2], Size = size / 2;
                bool A = (side == LEFT && (a < 0 || a >= 2 * Size || a + Size < 0 || a + Size >= 2 * Size));
                bool B = (side == DOWN && (b < 0 || b >= 2 * Size || b + Size < 0 || b + Size >= 2 * Size));
                bool C = (side == RIGHT && (a + 1 < 0 || a + 1 >= 2 * Size || a + 1 - Size < 0 || a + 1 - Size >= 2 * Size));
                bool D = (side == UP && (b + 1 < 0 || b + 1 >= 2 * Size || b + 1 - Size < 0 || b + 1 - Size >= 2 * Size));
                if (A || B || C || D)
                    exit[0] = -2;
                else {
                    if (side == LEFT && maxy - miny == Size - 1) {
                        minx = a; maxx = a + Size - 1;
                        for (int i = a; i < a + Size; ++i) { AddWall(i, miny, 1); AddWall(i, miny + Size, 1); }
                        for (int j = miny; j <= maxy; ++j) { AddWall(a, j, 0); AddWall(a + Size, j, 0); }
                    }
                    else if (side == DOWN && maxx - minx == Size - 1) {
                        miny = b; maxy = b + Size - 1;
                        for (int i = minx; i <= maxx; ++i) { AddWall(i, b, 1); AddWall(i, b + Size, 1); }
                        for (int j = b; j < b + Size; ++j) { AddWall(minx, j, 0); AddWall(minx + Size, j, 0); }
                    }
                    else if (side == RIGHT && maxy - miny == Size - 1) {
                        minx = a + 1 - Size; maxx = a;
                        for (int i = a + 1 - Size; i < a + 1; ++i) { AddWall(i, miny, 1); AddWall(i, miny + Size, 1); }
                        for (int j = miny; j <= maxy; ++j) { AddWall(a + 1 - Size, j, 0); AddWall(a + 1, j, 0); }
                    }
                    else if (side == UP && maxx - minx == Size - 1) {
                        miny = b + 1 - Size; maxy = b;
                        for (int i = minx; i <= maxx; ++i) { AddWall(i, b + 1 - Size, 1); AddWall(i, b + 1, 1); }
                        for (int j = b + 1 - Size; j < b + 1; ++j) { AddWall(minx, j, 0); AddWall(minx + Size, j, 0); }
                    }
                }
            }
        }
    }

    //functions
    protected void Reload() {
        k = choice = 0;
        aftHosp = false;
        hosp_map = my_map = null;
        players = null;
        v = new int[2];
        InitBFS();
        dspy_x = dspy_y = is_spy = 0;
        Join(Players, Treasures, Size, my_id);
        RandomAns();
    }
    public override void Join(int the_players, int the_treasures, int the_size, int the_id) {
        ansType = types[STEP];
        ansSide = sides[rand.Next(NUMOFSIDES)];
        Players = the_players;
        Treasures = the_treasures;
        Size = the_size;
        my_id = the_id;
        players = new Player[Players];
        for (int i = 0; i < Players; ++i) {
            players[i].A = new Map(2 * Size); players[i].B = new Map(2 * Size);
            players[i].id = i;
        }
        my_map = new Map(2 * Size);
        hosp_map = new Map(2 * Size);
        UpdateStats();
    }
    protected int SideToNum(string m) {
        for (int i = 0; i < NUMOFSIDES; ++i)
            if (m == sides[i])
                return i;
        return -1;
    }
    protected virtual int Can(int a, int b, int k) => my_map.Can(a, b, k);
    protected int Can(Coord coord, int k) => my_map.Can(coord.x, coord.y, k);
    protected bool Have(int form, int a, int b) {
        for (int i = 0; i < NUMOFSIDES; ++i)
            if (Can(a, b, i) == form)
                return true;
        return false;
    }
    protected bool Have(int form) => Have(form, my_map.x, my_map.y);
    protected void BFS() {
        InitBFS();

        Queue<int[]> q = new Queue<int[]>();
        int[] s = { my_map.x, my_map.y };
        q.Enqueue(s);
        used[s[0], s[1]] = true;
        int[] null_ = { -1, -1 };
        p[s[0], s[1]] = null_;
        while (q.Count > 0) {
            int[] v = q.Dequeue();
            int z = v[0], t = v[1];
            for (int i = 0; i < 4; ++i) {
                int dx = 0, dy = 0;

                if (i == LEFT) dx = -1;
                else if (i == DOWN) dy = -1;
                else if (i == RIGHT) dx = 1;
                else if (i == UP) dy = 1;

                bool Can = this.Can(z, t, i) == FREE;
                int[] to = { z + dx, t + dy };
                if (Can && !used[to[0], to[1]]) {
                    used[to[0], to[1]] = true;
                    q.Enqueue(to);
                    p[to[0], to[1]] = v;
                    if (Have(UNKNOWN, to[0], to[1]) && notExpl_size < 10)
                        notExpl[++notExpl_size] = to;
                    if (!my_map.was[to[0], to[1]] && notWas_size < 10)
                        notWas[++notWas_size] = to;
                }
            }
        }
    }
    private void InitBFS() {
        used = new bool[2 * Size, 2 * Size];
        p = new int[2 * Size, 2 * Size][];
        notExpl = new int[11][];
        notWas = new int[11][];
        notExpl_size = notWas_size = path_size = 0;
    }
    protected void Path(int a, int b) {
        path = new int[Size * Size + 1][];
        path_size = 0;
        int[] to = { a, b };
        for (int[] v = to; v[0] != -1; v = p[v[0], v[1]]) {
            path[++path_size] = v;
        }
    }
    protected void GoToV() {
        int x = my_map.x, y = my_map.y;
        if (knifes > 0 && rand.Next(5) == 0 && Have(FREE)) { ansType = "strike"; ansSide = Random(FREE); choice = NO_CHOICE; }
        else if (bullets > 0 && rand.Next(15) == 0 && Have(FREE)) { ansType = "fire"; ansSide = Random(FREE); choice = NO_CHOICE; }
        else {
            --path_size;
            int t = -1;
            if (path[path_size] != null) {
                int a = path[path_size][0], b = path[path_size][1];
                if (a == x - 1) t = 0; if (b == y - 1) t = 1; if (a == x + 1) t = 2; if (b == y + 1) t = 3;
                ansType = "step"; ansSide = sides[t];
                if (v[0] == a && v[1] == b) choice = NO_CHOICE;
            }
            else {
                ansType = "step"; ansSide = sides[rand.Next(NUMOFSIDES)];
                choice = NO_CHOICE;
            }
        }
    }
    protected void UpdateAns() {
        UpdateStats();
        if (IsJam()) bullets = 0;
        if (choice == NO_CHOICE) UpdateChoice();
        if (choice == EXPLORE || choice == GO_TO_KILL) GoToV();
        if (choice == TAKE_AFTER_STRIKE || choice == TAKE_AFTER_FIRE) { ansType = "step"; ansSide = sides[k]; }
        if (choice == CRAZY) {
            if (knifes > 0) { ansType = "strike"; ansSide = (Have(FREE, my_map.x, my_map.y)) ? Random(FREE) : Random(UNKNOWN); }
            else {
                ansType = "step";
                ansSide =
                    Have(WALL) ? Random(WALL)
                    : (Have(EXIT) ? Random(EXIT)
                    : sides[rand.Next(NUMOFSIDES)]);
            }
        }
        if (IsJam()) bullets = Check.bullets(my_id);
    }
    protected void UpdateChoice() {
        if (knifes > 0 && rand.Next(5) == 0 && Have(FREE)) { ansType = "strike"; ansSide = Random(FREE); }
        else if (bullets > 0 && rand.Next(15) == 0 && Have(FREE)) { ansType = "fire"; ansSide = Random(FREE); }
        else {
            BFS(); ansType = "step"; int t = Max((Treasures - SumOut()) / 2 - treasures, 1);
            if (my_map.x == my_map.exit[0] && my_map.y == my_map.exit[1] && treasures > 0) { ansSide = sides[my_map.exit[2]]; }
            else if (treasures > 0 && rand.Next(t) == 0 && (my_map.exit[0] > -1 && p[my_map.exit[0], my_map.exit[1]] != null)) {
                v[0] = my_map.exit[0]; v[1] = my_map.exit[1];
                Path(v[0], v[1]);
                choice = EXPLORE;
            }
            else if (Have(UNKNOWN)) ansSide = Random(UNKNOWN);
            else {
                int a = notExpl_size > 0 ? 1 : 0,
                    b = notWas_size > 0 ? 2 : 0,
                    c = a + b == 0 ? 1 : 0;
                switch (massRand(new int[] { a, b, c })) {
                    case 0:
                        t = rand.Next(rand.Next(notExpl_size)) + 1;
                        v[0] = notExpl[t][0]; v[1] = notExpl[t][1];
                        Path(v[0], v[1]);
                        choice = EXPLORE;
                        break;
                    case 1:
                        t = rand.Next(rand.Next(notWas_size)) + 1;
                        v[0] = notWas[t][0]; v[1] = notWas[t][1];
                        Path(v[0], v[1]);
                        choice = EXPLORE;
                        break;
                    default: ansSide = Random(FREE); break;
                }
            }
        }
    }
    protected int Max(int a, int b) => (a > b) ? a : b;
    protected int Min(int a, int b) => (a < b) ? a : b;
    protected int SumOut() {
        int sum = 0;
        for (int i = 0; i < Players; ++i) sum += Check.treasureOut(i);
        return sum;
    }
    protected string Random(int form, int i, int j) {
        int[] val = new int[NUMOFSIDES];
        int k = 0;
        for (int d = 0; d < NUMOFSIDES; ++d)
            if (Can(i, j, d) == form)
                val[k++] = d;
        if (k > 0) {
            int t = rand.Next(k);
            return sides[val[t]];
        }
        return "";
    }
    protected string Random(int form) => Random(form, my_map.x, my_map.y);
    protected int GameAns(string s) {
        switch (s) {
            case "wall\n": return WALL;
            case "exit\n": return EXIT;
            case "hit\n": return HIT;
            case "miss\n": return MISS;
            default: return GO;
        }
    }
    protected Map High(Map A, Map B) {
        return A.rank >= B.rank ? A.copy() : B.copy();
    }
    protected void Add(Map A, Map B, int a, int b) {
        A.UpdateBorders(); B.UpdateBorders();
        for (int i = B.minx - B.x + a; i <= B.maxx + 1 - B.x + a; ++i)
            for (int j = B.miny - B.y + b; j <= B.maxy + 1 - B.y + b; ++j) {
                if (A.x + i < 0 || A.x + i >= 2 * Size || A.y + j < 0 || A.y + j >= 2 * Size) { GetInfoB(); return; }
                if (B.was[B.x - a + i, B.y - b + j])
                    A.was[A.x + i, A.y + j] = B.was[B.x - a + i, B.y - b + j];
                if (A.can_to_move[A.x + i, A.y + j, 0] == UNKNOWN)
                    A.UpdateCan(A.x + i, A.y + j, B.can_to_move[B.x - a + i, B.y - b + j, 0], 0);
                if (A.can_to_move[A.x + i, A.y + j, 1] == UNKNOWN)
                    A.UpdateCan(A.x + i, A.y + j, B.can_to_move[B.x - a + i, B.y - b + j, 1], 1);
            }
        if (B.exit[0] > -1) {
            A.exit[0] = A.x + B.exit[0] - (B.x - a);
            A.exit[1] = A.y + B.exit[1] - (B.y - b);
            A.exit[2] = B.exit[2];
        }
        used = new bool[2 * Size, 2 * Size]; //init 
        void DFS(int m, int n) {
            const int l = LEFT, d = DOWN, r = RIGHT, u = UP;
            used[m, n] = true;
            if (m < A.minx) A.minx = m; if (m > A.maxx) A.maxx = m;
            if (n < A.miny) A.miny = n; if (n > A.maxy) A.maxy = n;
            if (A.Can(m, n, l) == FREE) if (!used[m - 1, n]) DFS(m - 1, n);
            if (A.Can(m, n, d) == FREE) if (!used[m, n - 1]) DFS(m, n - 1);
            if (A.Can(m, n, r) == FREE) if (!used[m + 1, n]) DFS(m + 1, n);
            if (A.Can(m, n, u) == FREE) if (!used[m, n + 1]) DFS(m, n + 1);
        }
        DFS(A.x, A.y);
        A.UpdateBorders();
    }
    /// <summary> Попытка совместить две карты с вероятностью хотя бы 1/k, обновляет первый аргумент </summary>
    protected void Merge(ref Map A, Map B, int k = 1) {
        A.UpdateBorders(); B.UpdateBorders();
        Map C = new Map(Size + 1);
        int var = 0;
        List<int> gxa = new List<int>(), gya = new List<int>(), gxb = new List<int>(), gyb = new List<int>();
        int dxa = (A.maxx - A.minx), dya = (A.maxy - A.miny), dxb = (B.maxx - B.minx), dyb = (B.maxy - B.miny);
        for (int ia = 0; ia < Size - dxa; ++ia)
            for (int ja = 0; ja < Size - dya; ++ja)
                for (int ib = 0; ib < Size - dxb; ++ib)
                    for (int jb = 0; jb < Size - dyb; ++jb) {
                        int err = 0;
                        C = new Map(Size + 1);
                        for (int i = ia; i <= ia + dxa + 1; ++i)
                            for (int j = ja; j <= ja + dya + 1; ++j) {
                                C.can_to_move[i, j, 0] = A.can_to_move[i - ia + A.minx, j - ja + A.miny, 0];
                                C.can_to_move[i, j, 1] = A.can_to_move[i - ia + A.minx, j - ja + A.miny, 1];
                            }
                        for (int i = ib; i <= ib + dxb + 1 && err == 0; ++i)
                            for (int j = jb; j <= jb + dyb + 1 && err == 0; ++j) {
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
                                else if (i == 0) {
                                    if (C.can_to_move[i, j, 1] == 0) C.can_to_move[i, j, 1] = 1;
                                    if (C.can_to_move[i, j, 0] == 1) err = 1;
                                }
                                else if (j == 0) {
                                    if (C.can_to_move[i, j, 0] == 0) C.can_to_move[i, j, 0] = 1;
                                    if (C.can_to_move[i, j, 1] == 1) err = 1;
                                }
                                else {
                                    if (C.can_to_move[i, j, 0] == 0) C.can_to_move[i, j, 0] = 1;
                                    if (C.can_to_move[i, j, 1] == 0) C.can_to_move[i, j, 1] = 1;
                                }
                        if (err != 0) continue;

                        used = new bool[2 * Size, 2 * Size]; //init
                        int DFS(int a, int b, Map Map) {
                            int s = 1;
                            used[a, b] = true;
                            if (Map.Can(a, b, 0) == 1) if (!used[a - 1, b]) s += DFS(a - 1, b, Map);
                            if (Map.Can(a, b, 1) == 1) if (!used[a, b - 1]) s += DFS(a, b - 1, Map);
                            if (Map.Can(a, b, 2) == 1) if (!used[a + 1, b]) s += DFS(a + 1, b, Map);
                            if (Map.Can(a, b, 3) == 1) if (!used[a, b + 1]) s += DFS(a, b + 1, Map);
                            return s;
                        }

                        int sum = DFS(0, 0, C);
                        if (sum == Size * Size) {
                            ++var; gxa.Add(ia); gya.Add(ja); gxb.Add(ib); gyb.Add(jb);
                        }
                        if (var > k) return;
                    }
        if (var > 0) {
            int r = rand.Next(var);
            int x = A.minx - B.minx + gxb[r] - gxa[r], y = A.miny - B.miny + gyb[r] - gya[r];
            for (int i = B.minx; i <= B.maxx + 1; ++i)
                for (int j = B.miny; j <= B.maxy + 1; ++j) {
                    if (B.was[i, j])
                        A.was[x + i, y + j] = B.was[i, j];
                    if (A.can_to_move[x + i, y + j, 0] == 0)
                        A.UpdateCan(x + i, y + j, B.can_to_move[i, j, 0], 0);
                    if (A.can_to_move[x + i, y + j, 1] == 0)
                        A.UpdateCan(x + i, y + j, B.can_to_move[i, j, 1], 1);
                }
            if (B.exit[0] > -1) {
                A.exit[0] = B.exit[0] + x;
                A.exit[1] = B.exit[1] + y;
                A.exit[2] = B.exit[2];
            }
            used = new bool[2 * Size, 2 * Size]; //init
            void DFS(ref Map map, int a, int b) {
                used[a, b] = true;
                if (a < map.minx) map.minx = a; if (a > map.maxx) map.maxx = a;
                if (b < map.miny) map.miny = b; if (b > map.maxy) map.maxy = b;
                if (map.Can(a, b, 0) == 1) if (!used[a - 1, b]) DFS(ref map, a - 1, b);
                if (map.Can(a, b, 1) == 1) if (!used[a, b - 1]) DFS(ref map, a, b - 1);
                if (map.Can(a, b, 2) == 1) if (!used[a + 1, b]) DFS(ref map, a + 1, b);
                if (map.Can(a, b, 3) == 1) if (!used[a, b + 1]) DFS(ref map, a, b + 1);
            }
            DFS(ref A, A.x, A.y);
            A.UpdateBorders();
            if (A.exit[0] == -2) { GetInfoB(); return; }
            Spy_detect(x, y);
        }
        else GetInfoB();
    }
    protected bool ConflictRes(int res, int side) =>
        Can(my_map.x, my_map.y, side) != res &&
        Can(my_map.x, my_map.y, side) != UNKNOWN;
    protected bool ConflictMove() => my_map.maxx - my_map.minx >= Size || my_map.maxy - my_map.miny >= Size;
    // TODO: closure-checking
    protected bool ConflictWall() {
        bool[,] used = new bool[2 * Size, 2 * Size];
        int[,,] wall = new int[2 * Size, 2 * Size, 2];
        int Can(Coord cor, int k) {
            bool A = cor.x < 0, B = cor.x >= 2 * Size - 1, C = cor.y < 0, D = cor.y >= 2 * Size - 1;
            if ((A || B) && (C || D)) return UNKNOWN;
            if (A) if (k != RIGHT) return UNKNOWN;
            if (B) if (k != LEFT) return UNKNOWN;
            if (C) if (k != UP) return UNKNOWN;
            if (D) if (k != DOWN) return UNKNOWN;
            return this.Can(cor, k);
        }
        void Move(ref Coord coord, int k) {
            switch (k) {
                case LEFT: --coord.x; break;
                case DOWN: --coord.y; break;
                case RIGHT: ++coord.x; break;
                case UP: ++coord.y; break;
            }
        }
        void UpdateWall(Coord cor, int k) {
            bool A = cor.x < 0, B = cor.x >= 2 * Size - 1, C = cor.y < 0, D = cor.y >= 2 * Size - 1;
            if ((A || B) && (C || D)) return;
            if (A) if (k != RIGHT) return;
            if (B) if (k != LEFT) return;
            if (C) if (k != UP) return;
            if (D) if (k != DOWN) return;
            ++wall[cor.x + (k == RIGHT ? 1 : 0), cor.y + (k == UP ? 1 : 0), k % 2];
        }
        void Walking(Coord start, int k) {
            if (wall[start.x + (k == RIGHT ? 1 : 0), start.y + (k == UP ? 1 : 0), k % 2] == 0) {
                Coord coord = start; int dir = k;
                do {
                    if (Can(coord, dir) < 0 && Can(coord, (dir + 1) % 4) >= 0) {
                        UpdateWall(coord, dir);
                        Move(ref coord, (dir + 1) % 4);
                    }
                    else if (Can(coord, dir) >= 0) {
                        Move(ref coord, dir);
                        dir = (4 + dir - 1) % 4;
                    }
                    else {
                        UpdateWall(coord, dir);
                        dir = (dir + 1) % 4;
                        UpdateWall(coord, dir);
                    }
                }
                while (coord != start || dir != k);
            }
        }

        void DFS(int a, int b) {
            used[a, b] = true;
            const int l = LEFT, d = DOWN, r = RIGHT, u = UP;

            if (this.Can(a, b, l) == FREE) if (!used[a - 1, b]) DFS(a - 1, b);
            if (this.Can(a, b, d) == FREE) if (!used[a, b - 1]) DFS(a, b - 1);
            if (this.Can(a, b, r) == FREE) if (!used[a + 1, b]) DFS(a + 1, b);
            if (this.Can(a, b, u) == FREE) if (!used[a, b + 1]) DFS(a, b + 1);

            for (int i = 0; i < NUMOFSIDES; ++i)
                if (this.Can(a, b, i) == WALL)
                    Walking(new Coord(a, b), i);

        }

        bool HaveClose() {
            for (int i = 0; i < 2 * Size; ++i)
                for (int j = 0; j < 2 * Size; ++j)
                    if (wall[i, j, 0] == 1 || wall[i, j, 1] == 1)
                        if (my_map.exit[0] < 0 ||
                            i != my_map.minx &&
                            j != my_map.miny &&
                            i != my_map.maxx + 1 &&
                            j != my_map.maxy + 1) {

                            //First.lab.Show();
                            //First.bot_show(my_map, my_id);
                            return true;

                        }
            return false;
        }

        try {
            DFS(my_map.x, my_map.y);
            return HaveClose();
        }
        catch {
            //First.lab.Show();
            //First.bot_show(my_map, my_id);
            return true;
        }
    }
    protected void UpdateStats() {
        treasures = Check.treasures(my_id);
        treasuresOut = Check.treasureOut(my_id);
        knifes = Check.knifes(my_id);
        bullets = Check.bullets(my_id);
        armors = Check.armors(my_id);
        crackers = Check.crackers(my_id);
    }
    protected void GetInfoB() {
        if (IsJam())
            for (int i = 0; i < Players; ++i) players[i].spy = false;
        aftHosp = false;
        my_map = players[my_id].B.copy();
        choice = NO_CHOICE;
        if (ConflictMove() || ConflictWall())
            Reload();
    }
    protected bool SmbLosed() {
        for (int i = 0; i < Players; ++i) if (players[i].treasures > Check.treasures(i)) return true;
        return false;
    }
    protected void UpdateCan_and_xy(int gameAns, int k) //only for "step"
    {
        if (choice != CRAZY) {
            int res;
            if (gameAns == WALL) {
                res = -1;
                if (ConflictRes(res, k)) GetInfoB();
                my_map.UpdateCan(res, k);
                players[my_id].B.UpdateCan(res, k);
            }
            if (gameAns == GO) {
                res = 1;
                if (ConflictRes(res, k)) GetInfoB();
                my_map.UpdateCan(res, k);
                my_map.Move(k);
                players[my_id].B.UpdateCan(res, k);
                players[my_id].B.Move(k);
            }
            if (gameAns == EXIT) {
                res = EXIT;
                if (ConflictRes(res, k)) {
                    GetInfoB();
                    if (ConflictRes(res, k))
                        Reload();
                }
                my_map.UpdateCan(res, k);
                my_map.UpdateExit(k);
                if (my_map.exit[0] == -2) GetInfoB();
                players[my_id].B.UpdateCan(res, k);
                players[my_id].B.UpdateExit(k);
            }
            if (ConflictMove()) GetInfoB();
            if (ConflictWall()) GetInfoB();
            if (choice == TAKE_AFTER_STRIKE) choice = NO_CHOICE;
            if (choice == TAKE_AFTER_FIRE && (Check.treasures(my_id) > treasures || gameAns != GO)) choice = NO_CHOICE;
            if (aftHosp) Add(hosp_map, players[my_id].B, players[my_id].B.x - hosp_map.x, players[my_id].B.y - hosp_map.y);
        }
        else {
            choice = NO_CHOICE;
            if (gameAns == GO) {
                players[my_id].A = High(players[my_id].A, players[my_id].B);
                players[my_id].B.NewLife();
                GetInfoB();
            }
        }
    }
    public override void Update(string ansType_id, string ansSide_id, string gameAns_id, int id) {
        try {
            int gameAns = GameAns(gameAns_id); k = SideToNum(ansSide_id);
            if (gameAns_id == "hit\n") {
                if (id != my_id) {
                    bool A = Check.treasures(my_id) == 0 && (players[my_id].B.Can(players[my_id].B.x, players[my_id].B.y, (k + 2) % 4) >= 0 || players[id].choice == CRAZY);
                    if (ansType_id != "throw") {
                        if (armors == 0 && A) {
                            if (treasures > 0) {
                                players[my_id].A = High(players[my_id].A, players[my_id].B);
                                used = new bool[2 * Size, 2 * Size]; //init
                                void DFS(int a, int b) {
                                    used[a, b] = true;
                                    if (a < hosp_map.minx) hosp_map.minx = a; if (a > hosp_map.maxx) hosp_map.maxx = a;
                                    if (b < hosp_map.miny) hosp_map.miny = b; if (b > hosp_map.maxy) hosp_map.maxy = b;
                                    if (hosp_map.Can(a, b, 0) == 1) if (!used[a - 1, b]) DFS(a - 1, b);
                                    if (hosp_map.Can(a, b, 1) == 1) if (!used[a, b - 1]) DFS(a, b - 1);
                                    if (hosp_map.Can(a, b, 2) == 1) if (!used[a + 1, b]) DFS(a + 1, b);
                                    if (hosp_map.Can(a, b, 3) == 1) if (!used[a, b + 1]) DFS(a, b + 1);
                                }
                                DFS(Size - 1, Size - 1);
                                Merge(ref hosp_map, players[my_id].B);
                                hosp_map.UpdateCoord(Size - 1, Size - 1);
                                players[my_id].B = hosp_map.copy();
                                GetInfoB();
                                UpdateAns();
                                aftHosp = true;
                            }
                            else {
                                aftHosp = false;
                                players[my_id].A = High(players[my_id].A, players[my_id].B);
                                players[my_id].B.NewLife();
                            }
                        }
                        if (armors != Check.armors(my_id) && ansType_id == "strike" && players[id].choice != CRAZY) {
                            int dx = 0, dy = 0;
                            if (k == 0) dx = 1; if (k == 2) dx = -1;
                            if (k == 1) dy = 1; if (k == 3) dy = -1;
                            Add(players[my_id].B, players[id].B, dx, dy);
                            if (players[my_id].B.Can(players[my_id].B.x, players[my_id].B.y, (k + 2) % 4) == 0) {
                                players[my_id].B.UpdateCan(1, (k + 2) % 4);
                            }
                            Add(my_map, players[my_id].B, 0, 0);
                            if (ConflictMove()) GetInfoB();
                            if (ConflictWall()) GetInfoB();
                        }
                    }
                    else {
                        if (A) {
                            choice = CRAZY;
                            UpdateAns();
                        }
                    }
                }
                for (int i = 0; i < Players; ++i)
                    if (i != id && i != my_id) {
                        bool A = players[i].B.Can(players[i].B.x, players[i].B.y, (k + 2) % 4) > -1 || players[id].choice == CRAZY;
                        if (ansType_id != "throw") {
                            if (Check.treasures(i) == 0 && players[i].armors == 0 && A) {
                                bool B = players[id].choice != CRAZY && (id != my_id || choice != CRAZY);

                                //strike and hit mean that dist(i,id)==1 
                                if (ansType_id == "strike" && players[i].treasures > 0 && B) {
                                    int dx = 0, dy = 0;
                                    if (k == 0) dx = -1; if (k == 2) dx = 1;
                                    if (k == 1) dy = -1; if (k == 3) dy = 1;
                                    Add(players[id].B, players[i].B, dx, dy);
                                    Add(players[i].B, players[id].B, -dx, -dy);
                                    if (id == my_id) {
                                        Add(my_map, players[my_id].B, 0, 0);
                                        if (ConflictMove()) GetInfoB();
                                        if (ConflictWall()) GetInfoB();
                                    }
                                }

                                players[i].A = High(players[i].A, players[i].B);
                                players[i].B.NewLife();
                                players[i].aftHosp = players[i].treasures > 0;

                                Spy_off(i); //Jam
                            }
                        }
                        else if (A) players[i].choice = CRAZY;
                    }
            }
            if (id == my_id) {
                if (ansType_id != "step") {
                    if (choice != CRAZY) {
                        if (ansType_id != "throw") {
                            if (gameAns == WALL) { players[my_id].B.UpdateCan(WALL, k); GetInfoB(); } //lose knife
                            else if (gameAns == HIT && SmbLosed()) {
                                if (ansType_id == "strike") choice = TAKE_AFTER_STRIKE;
                                if (ansType_id == "fire") choice = TAKE_AFTER_FIRE;
                            }
                        }
                        else if (gameAns == HIT) choice = CRAZY;
                    }
                    else if (ansType_id != "throw") {
                        if (gameAns == HIT && SmbLosed()) {
                            if (ansType_id == "strike") choice = TAKE_AFTER_STRIKE;
                            if (ansType_id == "fire") choice = TAKE_AFTER_FIRE;
                        }
                        else choice = NO_CHOICE;
                    }
                    else choice = gameAns != EXIT ? 0 : 4;
                }
                else UpdateCan_and_xy(gameAns, k);
                Merge(ref my_map, players[my_id].A, MERGE_PRECISION);
                Merge(ref players[my_id].A, players[my_id].B, MERGE_PRECISION);
                UpdateAns();
            }
            else {
                if (gameAns_id == "hit\n") {
                    int x = players[id].B.x, y = players[id].B.y;
                    if (ansType_id != "throw" && players[id].choice == CRAZY) players[id].choice = NO_CHOICE;
                    if (ansType_id == "throw" && (players[id].B.Can(x, y, k) != 1 || players[id].choice == CRAZY))
                        players[id].choice = CRAZY;
                }
                else {
                    if (ansType_id == "step") {
                        if (players[id].choice != CRAZY) {
                            int res;
                            if (gameAns == WALL) {
                                res = -1;
                                players[id].B.UpdateCan(res, k);
                            }
                            if (gameAns == GO) {
                                res = 1;
                                players[id].B.UpdateCan(res, k);
                                players[id].B.Move(k);
                            }
                            if (gameAns == EXIT) {
                                res = -2;
                                players[id].B.UpdateCan(res, k);
                                players[id].B.UpdateExit(k);
                            }
                            if (players[id].aftHosp) {
                                Add(hosp_map, players[id].B, players[id].B.x - Size + 1, players[id].B.y - Size + 1);
                                if (IsJam()) {
                                    Add(players[id].B, hosp_map, Size - 1 - players[id].B.x, Size - 1 - players[id].B.y);
                                    if (!players[id].spy) Spy_on(id);
                                }
                            }
                        }
                        else {
                            players[id].choice = NO_CHOICE;
                            if (gameAns == GO) {
                                players[id].A = High(players[id].A, players[id].B);
                                players[id].B.NewLife();
                                players[id].choice = NO_CHOICE;
                                players[id].aftHosp = false;
                                Spy_off(id);
                            }
                        }
                    }
                    else { if (players[id].choice == CRAZY) players[id].choice = NO_CHOICE; }
                    Merge(ref players[my_id].B, players[id].B);
                    Merge(ref my_map, players[id].A, MERGE_PRECISION);
                    Merge(ref my_map, players[id].B, MERGE_PRECISION);
                    Spy_on(id);
                }
            }
            players[id].UpdateStats();
            UpdateStats();
            TryKill();
        }
        catch (System.Exception e) {
            broken = true;
            error = e.Message + "\n" + e.StackTrace + "\n";
            Reload();
        }
        finally {
            checkAns();
        }
    }

    protected void checkAns() {
        if (!types.Contains(ansType) || !sides.Contains(ansSide)) RandomAns();
    }

    //Jam
    protected virtual bool IsJam() => false;
    protected virtual void Spy_on(int id) { }
    protected virtual void Spy_off(int id) { }
    protected virtual void Spy_detect(int a, int b) { }
    protected virtual void TryKill() { }
    //~Jam
    protected int massRand(int[] mr) {
        int[] sum = new int[mr.Length + 1];
        sum[0] = 0;
        for (int i = 0; i < mr.Length; ++i) sum[i + 1] = sum[i] + mr[i];
        int x = rand.Next(sum[mr.Length]);
        for (int i = 0; i < mr.Length; ++i) if (sum[i] <= x && x < sum[i + 1]) return i;
        return -1;
    }
    protected void RandomAns() {
        int A = Check.knifes(my_id), B = Check.bullets(my_id), C = Check.crackers(my_id);
        int type = massRand(new int[] { A + B + C + 1, A, B, C }), side = rand.Next(4);
        ansType = types[type];
        ansSide = sides[side];
    }
}
/// <summary> Бот: version 1.0 </summary>
public class Bot_Alice: Bot_Bob {
    /// <summary> Бот: version 1.0 </summary>
    public Bot_Alice() { }
    //variables
    new Player[] players;
    int x, y;
    int[,,] can_to_move = new int[20, 20, 2];
    public int[] exit = { -1, -1, -1 };
    bool doubt;
    bool[,] was;


    //structures
    new struct Player {
        public int treasures;
    }

    //functions
    protected new void Reload() {
        k = choice = 0;
        aftHosp = false;
        hosp_map = my_map = null;
        players = null;
        v = new int[2];
        InitBFS();
        exit = new int[] { -1, -1, -1 };
        x = y = 0;
        can_to_move = new int[20, 20, 2];
        doubt = false;
        Join(Players, Treasures, Size, my_id);
        RandomAns();
    }
    public override void Join(int the_players, int the_treasures, int the_size, int the_id) {
        ansType = types[STEP];
        ansSide = sides[rand.Next(NUMOFSIDES)];
        Players = the_players;
        Treasures = the_treasures;
        Size = the_size;
        my_id = the_id;
        x = y = 9;
        players = new Player[the_players];
        was = new bool[20, 20];
        UpdateStats();
    }
    protected override int Can(int a, int b, int side) {
        int dx = (side == 2 ? 1 : 0), dy = (side == 3 ? 1 : 0); side = (side > 1) ? dy : side;
        return can_to_move[a + dx, b + dy, side];
    }
    void move(int side) {
        if (side == 0) --x;
        if (side == 1) --y;
        if (side == 2) ++x;
        if (side == 3) ++y;
        was[x, y] = true;
    }
    protected new void BFS() {
        InitBFS();

        Queue<int[]> q = new Queue<int[]>();
        int[] s = { x, y };
        q.Enqueue(s);
        used[s[0], s[1]] = true;
        int[] null_ = { -1, -1 };
        p[s[0], s[1]] = null_;
        while (q.Count > 0) {
            int[] v = q.Dequeue(); int z = v[0], t = v[1];
            for (int i = 0; i < 4; ++i) {
                int dx = 0, dy = 0; if (i == 0) dx = -1; if (i == 1) dy = -1; if (i == 2) dx = 1; if (i == 3) dy = 1;
                bool Can = this.Can(z, t, i) == 1;
                int[] to = { z + dx, t + dy };
                if (Can && !used[to[0], to[1]]) {
                    used[to[0], to[1]] = true;
                    q.Enqueue(to);
                    p[to[0], to[1]] = v;
                    if (Have(UNKNOWN, to[0], to[1]) && notExpl_size < 10)
                        notExpl[++notExpl_size] = to;
                    if (!was[to[0], to[1]] && notWas_size < 10)
                        notWas[++notWas_size] = to;
                }
            }
        }
    }
    private void InitBFS() {
        used = new bool[20, 20];
        p = new int[20, 20][];
        path = new int[101][];
        notExpl = new int[11][];
        notWas = new int[11][];
        notExpl_size = notWas_size = path_size = 0;
    }
    protected new void Path(int x, int y) {
        path_size = 0;
        int[] to = { x, y };
        for (int[] v = to; v[0] != -1; v = p[v[0], v[1]])
            path[++path_size] = v;
    }
    protected new void GoToV() {
        if (knifes > 0 && rand.Next(5) == 0 && Have(FREE, x, y)) { ansType = types[STRIKE]; ansSide = Random(FREE); }
        else if (bullets > 0 && rand.Next(5) == 0 && Have(FREE, x, y)) { ansType = types[FIRE]; ansSide = Random(FREE); }
        else {
            --path_size;
            int t = -1;
            int a = path[path_size][0], b = path[path_size][1];
            if (a == x - 1) t = 0; if (b == y - 1) t = 1; if (a == x + 1) t = 2; if (b == y + 1) t = 3;
            ansType = types[STEP]; ansSide = sides[t];
            if (v[0] == a && v[1] == b) choice = NO_CHOICE;
        }
    }
    protected new void UpdateAns() {
        UpdateStats();
        UpdateChoice();
        if (choice == EXPLORE) GoToV();
        else if (choice == TAKE_AFTER_STRIKE) {
            ansType = types[STEP]; ansSide = sides[k]; choice = NO_CHOICE;
            if (doubt) {
                path[path_size] = new int[2];
                path[path_size][0] = x; path[path_size][1] = y;
                ++path_size;
                choice = EXPLORE;
            }
        }
        else if (choice > 2) Reload();
    }
    protected new string Random(int form) => Random(form, x, y);
    protected new void UpdateChoice() {
        if (choice == NO_CHOICE) {
            bool A = (x == exit[0]) && (y == exit[1]) && doubt;
            if (knifes > 0 && rand.Next(5) == 0 && Have(FREE, x, y) && !A) { ansType = "strike"; ansSide = Random(FREE); }
            else if (bullets > 0 && rand.Next(5) == 0 && Have(FREE, x, y) && !A) { ansType = "fire"; ansSide = Random(FREE); }
            else {
                ansType = "step"; int t = Max((Treasures - SumOut()) / 2 - treasures, 1);
                if ((x == exit[0] && y == exit[1] && treasures > 0) || (x == exit[0] && y == exit[1] && doubt)) { ansSide = sides[exit[2]]; }
                else if (treasures > 0 && exit[0] > -1 && rand.Next(t) == 0) {
                    v[0] = exit[0]; v[1] = exit[1];
                    BFS();
                    Path(v[0], v[1]);
                    choice = EXPLORE;
                }
                else if (Have(UNKNOWN, x, y)) ansSide = Random(UNKNOWN);
                else {
                    BFS();
                    int a = notExpl_size > 0 ? 1 : 0,
                        b = notWas_size > 0 ? 2 : 0,
                        c = a + b == 0 ? 1 : 0;
                    switch (massRand(new int[] { a, b, c })) {
                        case 0:
                            t = rand.Next(rand.Next(notExpl_size)) + 1;
                            v[0] = notExpl[t][0]; v[1] = notExpl[t][1];
                            Path(v[0], v[1]);
                            choice = EXPLORE;
                            break;
                        case 1:
                            t = rand.Next(rand.Next(notWas_size)) + 1;
                            v[0] = notWas[t][0]; v[1] = notWas[t][1];
                            Path(v[0], v[1]);
                            choice = EXPLORE;
                            break;
                        default: ansSide = Random(FREE); break;
                    }
                }
            }
        }
    }
    protected new int GameAns(string s) {
        switch (s) {
            case "wall\n": return WALL;
            case "exit\n": return EXIT;
            case "hit\n": return HIT;
            case "miss\n": return MISS;
            default: return GO;
        }
    }
    protected void updateCan(int res, int k) {
        int dx = (k == 2 ? 1 : 0), dy = (k == 3 ? 1 : 0); k = (k > 1) ? dy : k;
        can_to_move[x + dx, y + dy, k] = res;
    }
    protected new void UpdateCan_and_xy(int gameAns, int k) {
        if (choice == CRAZY) {
            if (gameAns != GO) choice = NO_CHOICE;
            else Reload();
        }
        else {
            if (doubt) {
                if (x == exit[0] && y == exit[1] && gameAns != EXIT) Reload();
                else if (gameAns == WALL) Reload();
                else if (gameAns == GO) move(k);
                else if (gameAns == EXIT) { doubt = false; x = exit[0]; y = exit[1]; choice = NO_CHOICE; }
                if (x == -1 || y == -1 || x == 19 || y == 19) Reload();
            }
            else {
                if (gameAns == WALL) { updateCan(-1, k); }
                if (gameAns == GO) { updateCan(1, k); move(k); }
                if (gameAns == EXIT) { updateCan(-1, k); exit[0] = x; exit[1] = y; exit[2] = k; }
            }
        }
    }

    public override void Update(string ansType_id, string ansSide_id, string gameAns_id, int id) {
        try {
            int gameAns = GameAns(gameAns_id); k = SideToNum(ansSide_id);
            if (id == my_id) {
                if (ansType_id != "step") {
                    if (gameAns == HIT && SmbLosed()) choice = TAKE_AFTER_STRIKE;
                    if (gameAns == WALL && choice != CRAZY) Reload();
                    if (choice == CRAZY) choice = NO_CHOICE;
                }
                else UpdateCan_and_xy(gameAns, k);
                UpdateAns();
            }
            else {
                if (gameAns_id == "hit\n") {
                    if (ansType_id == "throw") {
                        if (knifes > 0) { ansType = "strike"; ansSide = (Have(FREE, x, y)) ? Random(FREE) : Random(UNKNOWN); } else if (bullets > 0) { ansType = "fire"; ansSide = (Have(FREE, x, y)) ? Random(FREE) : Random(UNKNOWN); } else { ansType = "step"; ansSide = (Have(WALL, x, y)) ? Random(WALL) : Random(UNKNOWN); }
                        choice = CRAZY;
                    }
                    else if (Check.treasures(my_id) == 0 && armors == 0) {
                        if (treasures > 0 || exit[0] == -1) { Reload(); UpdateAns(); }
                        else {
                            doubt = true;
                            v[0] = exit[0]; v[1] = exit[1];
                            if (x == v[0] && y == v[1]) choice = NO_CHOICE;
                            else {
                                BFS();
                                Path(v[0], v[1]);
                                choice = EXPLORE;
                            }
                            UpdateAns();
                        }
                    }
                }
            }
            players[id].treasures = Check.treasures(id);
            UpdateStats();
        }
        catch (System.Exception e) {
            broken = true;
            error = e.Message + "\n" + e.StackTrace + "\n";
            Reload();
        }
        finally {
            checkAns();
        }
    }
    protected new bool SmbLosed() {
        for (int i = 0; i < Players; ++i)
            if (players[i].treasures > Check.treasures(i))
                return true;
        return false;
    }
}
/// <summary> Бот: version 1.7 </summary>
public class Bot_Jam: Bot_Bob {
    /// <summary> Бот: version 1.7 </summary>
    public Bot_Jam() { }
    protected void BFS(int[,] indspy) {
        //init
        used = new bool[2 * Size, 2 * Size];
        p = new int[2 * Size, 2 * Size][];
        notExpl = new int[11][];
        notExpl_size = 0;

        Queue<int[]> q = new Queue<int[]>();
        int[] s = { my_map.x, my_map.y };
        q.Enqueue(s);
        used[s[0], s[1]] = true;
        int[] null_ = { -1, -1 };
        p[s[0], s[1]] = null_;
        while (q.Count > 0 && notExpl_size < 1) {
            int[] v = q.Dequeue(); int z = v[0], t = v[1];
            for (int i = 0; i < 4 && notExpl_size < 1; ++i) {
                int dx = 0, dy = 0; if (i == 0) dx = -1; if (i == 1) dy = -1; if (i == 2) dx = 1; if (i == 3) dy = 1;
                bool Can = this.Can(z, t, i) == 1;
                int[] to = { z + dx, t + dy };
                if (Can && !used[to[0], to[1]]) {
                    used[to[0], to[1]] = true;
                    q.Enqueue(to);
                    p[to[0], to[1]] = v;
                    if (indspy[to[0], to[1]] > 0 && notExpl_size < 1)
                        if (Check.treasures(indspy[to[0], to[1]] - 1) > 0)
                            notExpl[++notExpl_size] = to;
                }
            }
        }
    }
    protected override bool IsJam() => true;
    protected override void Spy_on(int id) {
        is_spy = 0;
        Merge(ref my_map, players[id].B, MERGE_PRECISION);
        if (is_spy == 1) {
            players[id].spy = true; players[id].dspy_x = dspy_x; players[id].dspy_y = dspy_y;
        }
    }
    protected override void Spy_off(int id) => players[id].spy = false;
    protected override void Spy_detect(int a, int b) {
        is_spy = 1; dspy_x = a; dspy_y = b;
    }
    protected override void TryKill() {
        players[my_id].spy = false;
        if (choice != CRAZY && (knifes + bullets > 0)) {
            bool spy = false;
            bool hit = false; int profit = 0;
            for (int i = 0; i < Players; ++i)
                if (players[i].spy) {
                    if (players[i].treasures > 0) spy = true;
                    if (players[i].treasures > profit) {
                        int x = my_map.x, y = my_map.y;
                        int xp = players[i].X(), yp = players[i].Y();
                        int dx = xp - x, dy = yp - y;
                        if (knifes > 0) {
                            void Win(int xx) { ansSide = sides[xx]; ansType = "strike"; profit = players[i].treasures; choice = NO_CHOICE; hit = true; }
                            if (dx == -1 && dy == 0 && Can(x, y, 0) == 1) { Win(0); continue; }
                            if (dx == 0 && dy == -1 && Can(x, y, 1) == 1) { Win(1); continue; }
                            if (dx == 1 && dy == 0 && Can(x, y, 2) == 1) { Win(2); continue; }
                            if (dx == 0 && dy == 1 && Can(x, y, 3) == 1) { Win(3); continue; }
                        }
                        if (bullets > 0) {
                            void Win(int xx) { ansSide = sides[xx]; ansType = "fire"; profit = players[i].treasures; choice = NO_CHOICE; hit = true; }
                            if (dx == 0) {
                                int pos = y;
                                if (dy < 0) {
                                    while (pos != yp)
                                        if (pos > -1 && pos < 2 * Size - 1)
                                            if (Can(x, pos, 1) == 1) --pos;
                                            else break;
                                        else break;
                                    if (pos == yp) { Win(1); continue; }
                                }
                                if (dy > 0) {
                                    while (pos != yp)
                                        if (pos > -1 && pos < 2 * Size - 1)
                                            if (Can(x, pos, 3) == 1) ++pos;
                                            else break;
                                        else break;
                                    if (pos == yp) { Win(3); continue; }
                                }
                            }
                            if (dy == 0) {
                                int pos = x;
                                if (dx < 0) {
                                    while (pos != xp)
                                        if (pos > -1 && pos < 2 * Size - 1)
                                            if (Can(x, pos, 0) == 1) --pos;
                                            else break;
                                        else break;
                                    if (pos == xp) { Win(0); continue; }
                                }
                                if (dx > 0) {
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
            if (spy && !hit && choice != TAKE_AFTER_STRIKE && choice != TAKE_AFTER_FIRE
                && (3 * treasures <= Treasures - SumOut() || my_map.exit[0] < 0))// && treasures==0)
            {
                int[,] indspy = new int[2 * Size - 1, 2 * Size - 1];
                for (int i = 0; i < Players; ++i) {
                    if (players[i].spy && players[i].treasures > 0) {
                        int x = players[i].X(), y = players[i].Y();
                        if (x != my_map.x || y != my_map.y) indspy[x, y] = i + 1;
                    }
                }
                BFS(indspy);
                if (notExpl_size != 0) {
                    notExpl[1].CopyTo(v, 0);
                    Path(v[0], v[1]);
                    choice = GO_TO_KILL;
                    UpdateAns();
                }
                else if (choice == GO_TO_KILL) { choice = NO_CHOICE; UpdateAns(); }
            }
            if (!hit && ansType != "step" && choice == NO_CHOICE) UpdateAns();
        }
    }
}
/// <summary> Бот: version 0.1 </summary>
public class Bot_Rand: Bot_Bob {
    /// <summary> Бот: version 0.1 </summary>
    public Bot_Rand() { }
    public override void Update(string ansType_id, string ansSide_id, string gameAns_id, int id)
        => RandomAns();
}
/// <summary> Бот: version 2.0 (in developing) </summary>
public class Bot_v2: Bot {
    //protected void BFS(int[,] indspy) {
    //    //init
    //    used = new bool[2 * Size, 2 * Size];
    //    p = new int[2 * Size, 2 * Size][];
    //    notExpl = new int[11][];
    //    notExpl_size = 0;

    //    Queue<int[]> q = new Queue<int[]>();
    //    int[] s = { my_map.x, my_map.y };
    //    q.Enqueue(s);
    //    used[s[0], s[1]] = true;
    //    int[] null_ = { -1, -1 };
    //    p[s[0], s[1]] = null_;
    //    while (q.Count > 0 && notExpl_size < 1) {
    //        int[] v = q.Dequeue(); int z = v[0], t = v[1];
    //        for (int i = 0; i < 4 && notExpl_size < 1; ++i) {
    //            int dx = 0, dy = 0; if (i == 0) dx = -1; if (i == 1) dy = -1; if (i == 2) dx = 1; if (i == 3) dy = 1;
    //            bool Can = this.Can(z, t, i) == 1;
    //            int[] to = { z + dx, t + dy };
    //            if (Can && !used[to[0], to[1]]) {
    //                used[to[0], to[1]] = true;
    //                q.Enqueue(to);
    //                p[to[0], to[1]] = v;
    //                if (indspy[to[0], to[1]] > 0 && notExpl_size < 1)
    //                    if (Check.treasures(indspy[to[0], to[1]] - 1) > 0)
    //                        notExpl[++notExpl_size] = to;
    //            }
    //        }
    //    }
    //}
    public struct point {
        public int x, y;
        public point(int X, int Y) { x = X; y = Y; }
        public static bool operator ==(point p1, point p2) => p1.x == p2.x && p1.y == p2.y;
        public static bool operator !=(point p1, point p2) => p1.x != p2.x || p1.y != p2.y;
        public static implicit operator point(Coord cor) => new point(cor.x, cor.y);
    }
    protected void InitBFS() {

    }
    protected static class Statement {
        public static bool BFS(int stat) {
            return true;
        }
    }
    protected static class Change {
        public static void BFS(int stat) {

        }
    }
    protected void BFS(point a, int stat = 0) {
        InitBFS();
        Queue<point> q = new Queue<point>();
        q.Enqueue(a);
        while (q.Count > 0 && Statement.BFS(stat)) {

        }
    }
}
