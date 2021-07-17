using System.Collections.Generic;

public class Bot {
    //constants
    protected static readonly string[] types = { "step", "strike", "fire", "throw" };
    protected static readonly string[] sides = { "left", "down", "right", "up" };
    protected const int STEP = 0, STRIKE = 1, FIRE = 2, THROW = 3;
    protected const int LEFT = 0, DOWN = 1, RIGHT = 2, UP = 3, ARITY = 4;

    //variables
    public bool broken;
    public static string error = "";
    protected static System.Random rand = new System.Random();
    public string ansType, ansSide;

    //functions
    public virtual void Join(int players, int treasures, int size, int id) { }
    public virtual void Update(string type, string side, string result, int id) { }
}
public class Bot_v1: Bot {
    protected const int EXIT = -2, WALL = -1, UNKNOWN = 0, FREE = 1;
    protected int Players, Treasures, Size, my_id;
    public override void Join(int the_players, int the_treasures, int the_size, int the_id) {
        ansType = types[STEP];
        ansSide = sides[rand.Next(ARITY)];
        try {
            Players = the_players;
            Treasures = the_treasures;
            Size = the_size;
            my_id = the_id;
            players = new player[Players];
            for (int i = 0; i < Players; ++i) {
                players[i].A = new Map(2 * Size); players[i].B = new Map(2 * Size);
                players[i].id = i;
            }
            my_map = new Map(2 * Size);
            hosp_map = new Map(2 * Size);
            UpdateStats();
        }
        catch (System.Exception e) {
            broken = true;
            error += e.Message + "\n" + e.StackTrace + "\n";
        }
    }
    protected int k, choice;
    protected int side_to_int(string m) {
        for (int i = 0; i < ARITY; ++i)
            if (m == sides[i])
                return i;
        return -1;
    }
    protected virtual int Can(int a, int b, int k) => my_map.Can(a, b, k);
    protected virtual bool Have(int form, int a, int b) {
        for (int i = 0; i < ARITY; ++i)
            if (Can(a, b, i) == form)
                return true;
        return false;
    }
    protected virtual bool Have(int form) => Have(form, my_map.x, my_map.y);
    protected virtual bool HaveArs() {
        return knifes != Check.knifes(my_id) || bullets != Check.bullets(my_id) || armors != Check.armors(my_id) || crackers != Check.crackers(my_id);
    }
    protected int[] v = new int[2];
    protected int[][] notExpl = new int[11][];
    protected int[,][] p;
    protected int[][] path;
    protected bool[,] used;
    protected int path_size, notExpl_size;
    protected virtual void NewBFS() {
        used = new bool[2 * Size, 2 * Size];
        p = new int[2 * Size, 2 * Size][];
        notExpl = new int[11][];
        notExpl_size = 0;
    }
    protected virtual void BFS() {
        NewBFS();
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
                }
            }
        }
    }
    protected virtual void Path(int a, int b) {
        path = new int[Size * Size + 1][];
        path_size = 0;
        int[] to = { a, b };
        for (int[] v = to; v[0] != -1; v = p[v[0], v[1]]) {
            path[++path_size] = v;
        }
    }
    protected virtual void NewDFS() => used = new bool[2 * Size, 2 * Size];
    protected virtual void GoToV() {
        int x = my_map.x, y = my_map.y;
        if (knifes > 0 && rand.Next(5) == 0 && Have(FREE)) { ansType = "strike"; ansSide = Random(FREE); choice = 0; }
        else if (bullets > 0 && rand.Next(15) == 0 && Have(FREE)) { ansType = "fire"; ansSide = Random(FREE); choice = 0; }
        else {
            --path_size;
            int t = -1;
            if (path[path_size] != null) {
                int a = path[path_size][0], b = path[path_size][1];
                if (a == x - 1) t = 0; if (b == y - 1) t = 1; if (a == x + 1) t = 2; if (b == y + 1) t = 3;
                ansType = "step"; ansSide = sides[t];
                if (v[0] == a && v[1] == b) choice = 0;
            }
            else {
                ansType = "step"; ansSide = sides[rand.Next(ARITY)];
                choice = 0;
            }
        }
    }
    protected virtual void UpdateAns() {
        UpdateStats();
        if (IsJam()) bullets = 0;
        if (choice == 0) UpdateChoice();
        if (choice == 1 || choice == 5) GoToV();
        if (choice == 2 || choice == 3) { ansType = "step"; ansSide = sides[k]; }
        if (choice == 4) {
            if (knifes > 0) { ansType = "strike"; ansSide = (Have(FREE, my_map.x, my_map.y)) ? Random(FREE) : Random(UNKNOWN); }
            else {
                ansType = "step";
                ansSide =
                    Have(WALL) ? Random(WALL)
                    : (Have(EXIT) ? Random(EXIT)
                    : sides[rand.Next(ARITY)]);
            }
        }
        if (IsJam()) bullets = Check.bullets(my_id);
    }
    protected virtual void UpdateChoice() {
        if (knifes > 0 && rand.Next(5) == 0 && Have(FREE)) { ansType = "strike"; ansSide = Random(FREE); }
        else if (bullets > 0 && rand.Next(15) == 0 && Have(FREE)) { ansType = "fire"; ansSide = Random(FREE); }
        else {
            BFS(); ansType = "step"; int t = Max((Treasures - SumOut()) / 2 - treasures, 1);
            if (my_map.x == my_map.exit[0] && my_map.y == my_map.exit[1] && treasures > 0) { ansSide = sides[my_map.exit[2]]; }
            else if (treasures > 0 && rand.Next(t) == 0 && (my_map.exit[0] > -1 ? p[my_map.exit[0], my_map.exit[1]] != null : false)) {
                v[0] = my_map.exit[0]; v[1] = my_map.exit[1];
                Path(v[0], v[1]);
                choice = 1;
            }
            else if (Have(UNKNOWN)) ansSide = Random(UNKNOWN);
            else if (notExpl_size > 0) {
                t = rand.Next(rand.Next(notExpl_size)) + 1;
                v[0] = notExpl[t][0]; v[1] = notExpl[t][1];
                Path(v[0], v[1]);
                choice = 1;
            }
            else ansSide = Random(FREE);
        }
    }
    protected virtual int Max(int a, int b) => (a > b) ? a : b;
    protected virtual int Min(int a, int b) => (a < b) ? a : b;
    protected virtual int SumOut() {
        int sum = 0;
        for (int i = 0; i < Players; ++i) sum += Check.treasureOut(i);
        return sum;
    }
    protected string random(int form, int i, int j) {
        int[] val = new int[ARITY];
        int k = 0;
        for (int d = 0; d < ARITY; ++d)
            if (Can(i, j, d) == form)
                val[k++] = d;
        if (k > 0) {
            int t = rand.Next(k);
            return sides[val[t]];
        }
        return "";
    }
    protected virtual string Random(int form) {
        return random(form, my_map.x, my_map.y);
    }
    protected virtual int GameAns(string s) {
        if (s == "wall\n") return 0;
        else if (s == "exit\n" || s == "hit\n") return 2;
        else return 1;
    }
    public class Map {
        public int size;
        public int rank;
        public int x, y;
        public int minx, miny, maxx, maxy;
        public int[] exit;
        public int[,,] can_to_move;
        void Init(int Size) {
            size = Size;
            rank = 0;
            minx = maxx = miny = maxy = x = y = size / 2 - 1;
            can_to_move = new int[size, size, 2];
            exit = new int[3] { -1, -1, -1 };
        }
        public Map(int Size) {
            Init(Size);
        }
        public Map copy() {
            return new Map(this);
        }
        Map(Map B) {
            Init(B.size);
            B.UpdateBorders();
            rank = B.rank;
            minx = B.minx; miny = B.miny;
            maxx = B.maxx; maxy = B.maxy;
            x = B.x; y = B.y;
            exit = new int[3] { B.exit[0], B.exit[1], B.exit[2] };
            for (int i = 0; i < size; ++i)
                for (int j = 0; j < size; ++j) {
                    can_to_move[i, j, 0] = B.can_to_move[i, j, 0];
                    can_to_move[i, j, 1] = B.can_to_move[i, j, 1];
                }
        }
        public void Move(int side) {
            if (side == LEFT) { if (x == minx) --minx; --x; }
            if (side == DOWN) { if (y == miny) --miny; --y; }
            if (side == RIGHT) { if (x == maxx) ++maxx; ++x; }
            if (side == UP) { if (y == maxy) ++maxy; ++y; }
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
                if (A || B || C || D) exit[0] = -2;
                else {
                    if (side == LEFT && maxy - miny == Size - 1) {
                        for (int i = a; i < a + Size; ++i) { AddWall(i, miny, 1); AddWall(i, miny + Size, 1); }
                        for (int j = miny; j <= maxy; ++j) { AddWall(a, j, 0); AddWall(a + Size, j, 0); }
                    }
                    else if (side == DOWN && maxx - minx == Size - 1) {
                        for (int i = minx; i <= maxx; ++i) { AddWall(i, b, 1); AddWall(i, b + Size, 1); }
                        for (int j = b; j < b + Size; ++j) { AddWall(minx, j, 0); AddWall(minx + Size, j, 0); }
                    }
                    else if (side == RIGHT && maxy - miny == Size - 1) {
                        for (int i = a + 1 - Size; i < a + 1; ++i) { AddWall(i, miny, 1); AddWall(i, miny + Size, 1); }
                        for (int j = miny; j <= maxy; ++j) { AddWall(a + 1 - Size, j, 0); AddWall(a + 1, j, 0); }
                    }
                    else if (side == UP && maxx - minx == Size - 1) {
                        for (int i = minx; i <= maxx; ++i) { AddWall(i, b + 1 - Size, 1); AddWall(i, b + 1, 1); }
                        for (int j = b + 1 - Size; j < b + 1; ++j) { AddWall(minx, j, 0); AddWall(minx + Size, j, 0); }
                    }
                }
            }
        }
        public void NewLife() {
            minx = maxx = miny = maxy = x = y = size / 2 - 1;
            can_to_move = new int[size, size, 2];
            exit = new int[3] { -1, -1, -1 };
            rank = 0;
        }
    }
    public Map hosp_map, my_map;
    protected bool aftHosp;
    protected virtual Map High(Map A, Map B) {
        if (A.rank >= B.rank) return A.copy();
        else return B.copy();
    }
    protected virtual void Add(Map A, Map B, int a, int b) {
        A.UpdateBorders(); B.UpdateBorders();
        for (int i = B.minx - B.x + a; i <= B.maxx + 1 - B.x + a; ++i)
            for (int j = B.miny - B.y + b; j <= B.maxy + 1 - B.y + b; ++j) {
                if (A.x + i < 0 || A.x + i >= 2 * Size || A.y + j < 0 || A.y + j >= 2 * Size) { GetInfoB(); return; }
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
        NewDFS();
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
        DFS(A.minx, A.miny);
        A.UpdateBorders();
    }
    protected virtual Map Merge(Map A, Map B) {
        A.UpdateBorders(); B.UpdateBorders();
        Map C = new Map(Size + 1);
        int var = 0;
        int gxa = 0, gya = 0, gxb = 0, gyb = 0;
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
                        NewDFS();
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
                            ++var; gxa = ia; gya = ja; gxb = ib; gyb = jb;
                        }
                        if (var > 1) return A;
                    }
        if (var == 1) {
            int x = A.minx - B.minx + gxb - gxa, y = A.miny - B.miny + gyb - gya;
            for (int i = B.minx; i <= B.maxx + 1; ++i)
                for (int j = B.miny; j <= B.maxy + 1; ++j) {
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
            NewDFS();
            void DFS(int a, int b) {
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
    protected virtual bool ConflictRes(int res, int side) {
        return Can(my_map.x, my_map.y, side) != res && Can(my_map.x, my_map.y, side) != UNKNOWN;
    }
    protected virtual bool ConflictMove() => my_map.maxx - my_map.minx >= Size || my_map.maxy - my_map.miny >= Size;
    protected virtual bool ConflictWall() {
        NewDFS();
        int DFS(int a, int b) {
            used[a, b] = true;
            if (Have(UNKNOWN, a, b)) return 0;
            int sum = 1, L = LEFT, D = DOWN, R = RIGHT, U = UP;
            if (Can(a, b, L) == FREE) if (!used[a - 1, b]) { int t = DFS(a - 1, b); if (t == 0) return 0; else sum += t; }
            if (Can(a, b, D) == FREE) if (!used[a, b - 1]) { int t = DFS(a, b - 1); if (t == 0) return 0; else sum += t; }
            if (Can(a, b, R) == FREE) if (!used[a + 1, b]) { int t = DFS(a + 1, b); if (t == 0) return 0; else sum += t; }
            if (Can(a, b, U) == FREE) if (!used[a, b + 1]) { int t = DFS(a, b + 1); if (t == 0) return 0; else sum += t; }
            return sum;
        }
        int res = DFS(my_map.x, my_map.y);
        return (res > 0 && res != Size * Size);
    }
    protected virtual void UpdateStats() {
        treasures = Check.treasures(my_id);
        treasuresOut = Check.treasureOut(my_id);
        knifes = Check.knifes(my_id);
        bullets = Check.bullets(my_id);
        armors = Check.armors(my_id);
        crackers = Check.crackers(my_id);
    }
    protected virtual void GetInfoB() {
        if (IsJam())
            for (int i = 0; i < Players; ++i) players[i].spy = false;
        aftHosp = false;
        my_map = players[my_id].B.copy();
        choice = 0;
    }
    public struct player {
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
        public int X() {
            return B.x + dspy_x;
        }
        public int Y() {
            return B.y + dspy_y;
        }
    }
    public player[] players;
    protected virtual bool SmbLosed() {
        for (int i = 0; i < Players; ++i) if (players[i].treasures > Check.treasures(i)) return true;
        return false;
    }
    protected virtual void UpdateCan_and_xy(int game, int k) //only for "step"
    {
        if (choice != 4) {
            int res;
            if (game == 0) {
                res = -1;
                if (ConflictRes(res, k)) GetInfoB();
                my_map.UpdateCan(res, k);
                players[my_id].B.UpdateCan(res, k);
            }
            if (game == 1) {
                res = 1;
                if (ConflictRes(res, k)) GetInfoB();
                my_map.UpdateCan(res, k);
                my_map.Move(k);
                players[my_id].B.UpdateCan(res, k);
                players[my_id].B.Move(k);
            }
            if (game == 2) {
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
            if (aftHosp) Add(hosp_map, players[my_id].B, players[my_id].B.x - hosp_map.x, players[my_id].B.y - hosp_map.y);
        }
        else {
            choice = 0;
            if (game == 1) {
                players[my_id].A = High(players[my_id].A, players[my_id].B);
                players[my_id].B.NewLife();
                GetInfoB();
            }
        }
    }
    public override void Update(string ansType_id, string ansSide_id, string gameAns_id, int id) {
        if (!broken) {
            try {
                int game = GameAns(gameAns_id); k = side_to_int(ansSide_id);
                if (gameAns_id == "hit\n") {
                    if (id != my_id) {
                        bool A = Check.treasures(my_id) == 0 && (players[my_id].B.Can(players[my_id].B.x, players[my_id].B.y, (k + 2) % 4) > -1 || players[id].choice == 4);
                        if (ansType_id != "throw") {
                            if (armors == 0 && A) {
                                if (treasures > 0) {
                                    players[my_id].A = High(players[my_id].A, players[my_id].B);
                                    NewDFS();
                                    void DFS(int a, int b) {
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
                            if (armors != Check.armors(my_id) && ansType_id == "strike" && players[id].choice != 4) {
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
                                choice = 4;
                                UpdateAns();
                            }
                        }
                    }
                    for (int i = 0; i < Players; ++i)
                        if (i != id && i != my_id) {
                            bool A = players[i].B.Can(players[i].B.x, players[i].B.y, (k + 2) % 4) > -1 || players[id].choice == 4;
                            if (ansType_id != "throw") {
                                if (Check.treasures(i) == 0 && players[i].armors == 0 && A) {
                                    bool B = players[id].choice != 4 && (id != my_id || choice != 4);

                                    //strike and hit mean that dist(i,id)==1 
                                    if (ansType_id == "strike" && players[i].treasures > 0 && B) {
                                        int dx = 0, dy = 0;
                                        if (k == 0) dx = -1; if (k == 2) dx = 1;
                                        if (k == 1) dy = -1; if (k == 3) dy = 1;
                                        Add(players[id].B, players[i].B, dx, dy);
                                        if (id == my_id) {
                                            Add(my_map, players[my_id].B, 0, 0);
                                            if (ConflictMove()) GetInfoB();
                                            if (ConflictWall()) GetInfoB();
                                        }
                                    }

                                    players[i].A = High(players[i].A, players[i].B);
                                    players[i].B.NewLife();
                                    if (players[i].treasures > 0) players[i].aftHosp = true;
                                    else players[i].aftHosp = false;

                                    Spy_off(i); //Jam
                                }
                            }
                            else if (A) players[i].choice = 4;
                        }
                }
                if (id == my_id) {
                    if (ansType_id != "step") {
                        if (choice != 4) {
                            if (ansType_id != "throw") {
                                if (game == 0) { players[my_id].B.UpdateCan(WALL, k); GetInfoB(); } //lose knife
                                else if (game == 2 && SmbLosed()) {
                                    if (ansType_id == "strike") choice = 2;
                                    if (ansType_id == "fire") choice = 3;
                                }
                            }
                            else if (game == 2) choice = 4;
                        }
                        else if (ansType_id != "throw") {
                            if (game == 2 && SmbLosed()) {
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
                else {
                    if (gameAns_id == "hit\n") {
                        int x = players[id].B.x, y = players[id].B.y;
                        if (ansType_id != "throw" && players[id].choice == 4) players[id].choice = 0;
                        if (ansType_id == "throw" && (players[id].B.Can(x, y, k) != 1 || players[id].choice == 4))
                            players[id].choice = 4;
                    }
                    else {
                        if (ansType_id == "step") {
                            if (players[id].choice != 4) {
                                int res;
                                if (game == 0) {
                                    res = -1;
                                    players[id].B.UpdateCan(res, k);
                                }
                                if (game == 1) {
                                    res = 1;
                                    players[id].B.UpdateCan(res, k);
                                    players[id].B.Move(k);
                                }
                                if (game == 2) {
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
                                players[id].choice = 0;
                                if (game == 1) {
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
            catch (System.Exception e) {
                broken = true;
                error += e.Message + "\n" + e.StackTrace + "\n";
                randomAns();
            }
        }
        else randomAns();
    }
    protected int treasures, treasuresOut, knifes, bullets, armors, crackers;
    //Jam.begin();
    protected virtual bool IsJam() => false;
    protected virtual void Spy_on(int id) { }
    protected virtual void Spy_off(int id) { }
    protected virtual void Spy_d(int a, int b) { }
    protected virtual void TryKill() { }
    //Jam.end();
    protected void randomAns() {
        int massRand(int[] mr) {
            int[] sum = new int[mr.Length + 1];
            sum[0] = 0;
            for (int i = 0; i < mr.Length; ++i) sum[i + 1] = sum[i] + mr[i];
            int x = rand.Next(sum[mr.Length]);
            for (int i = 0; i < mr.Length; ++i) if (sum[i] <= x && x < sum[i + 1]) return i;
            return -1;
        }
        int A = Check.knifes(my_id), B = Check.bullets(my_id), C = Check.crackers(my_id);
        int type = massRand(new int[] { A + B + C + 1, A, B, C }), side = rand.Next(4);
        ansType = types[type];
        ansSide = sides[side];
    }
}
public class Bot_Alice: Bot_v1 {
    int x, y;
    int[,,] can_to_move = new int[20, 20, 2];
    public override void Join(int the_players, int the_treasures, int the_size, int the_id) {
        ansType = types[STEP];
        ansSide = sides[rand.Next(ARITY)];
        try {
            Players = the_players;
            Treasures = the_treasures;
            Size = the_size;
            my_id = the_id;
            x = y = 9;
            players = new player[the_players];
            knifes = 1;
        }
        catch (System.Exception e) {
            broken = true;
            error += e.Message + "\n" + e.StackTrace + "\n";
        }
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
    }
    new int[] v = new int[2];
    new int[][] notExpl = new int[11][];
    new int[,][] p = new int[20, 20][];
    new int[][] path = new int[101][];
    new bool[,] used = new bool[20, 20];
    new int path_size, notExpl_size;
    protected override void BFS() {
        NewBFS();
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
                }
            }
        }
    }
    protected override void Path(int x, int y) {
        path_size = 0;
        int[] to = { x, y };
        for (int[] v = to; v[0] != -1; v = p[v[0], v[1]])
            path[++path_size] = v;
    }
    protected override void GoToV() {
        if (knifes > 0 && rand.Next(5) == 0 && Have(FREE, x, y)) { ansType = types[STRIKE]; ansSide = Random(FREE); }
        else if (bullets > 0 && rand.Next(5) == 0 && Have(FREE, x, y)) { ansType = types[FIRE]; ansSide = Random(FREE); }
        else {
            --path_size;
            int t = -1;
            int a = path[path_size][0], b = path[path_size][1];
            if (a == x - 1) t = 0; if (b == y - 1) t = 1; if (a == x + 1) t = 2; if (b == y + 1) t = 3;
            ansType = types[STEP]; ansSide = sides[t];
            if (v[0] == a && v[1] == b) choice = 0;
        }
    }
    protected override void UpdateAns() {
        UpdateStats();
        if (choice == 0) UpdateChoice();
        if (choice == 1) GoToV();
        if (choice == 2) {
            ansType = types[STEP]; ansSide = sides[k]; choice = 0;
            if (doubt) {
                path[path_size] = new int[2];
                path[path_size][0] = x; path[path_size][1] = y;
                ++path_size;
                choice = 1;
            }
        }
    }
    int[] exit = { -1, -1, -1 };
    protected override string Random(int form) => random(form, x, y);
    bool doubt;
    protected override void UpdateChoice() {
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
                choice = 1;
            }
            else if (Have(UNKNOWN, x, y)) ansSide = Random(UNKNOWN);
            else {
                BFS();
                if (notExpl_size > 0) {
                    t = rand.Next(rand.Next(notExpl_size)) + 1;
                    v[0] = notExpl[t][0]; v[1] = notExpl[t][1];
                    Path(v[0], v[1]);
                    choice = 1;
                }
                else ansSide = Random(FREE);
            }
        }
    }
    protected override int GameAns(string s) {
        if (s == "wall\n") return 0;
        else if (s == "exit\n" || s == "hit\n") return 2;
        else return 1;
    }
    void updateCan(int res, int k) {
        int dx = (k == 2 ? 1 : 0), dy = (k == 3 ? 1 : 0); k = (k > 1) ? dy : k;
        can_to_move[x + dx, y + dy, k] = res;
    }
    new struct Map {
        public int a, b;
        public int[,,] can;
        public int[] exit;
        public Map(int x) {
            a = b = -1;
            can = new int[x + 1, x + 1, 2];
            exit = new int[3];
            exit[0] = -1;
        }
        public void Coord(int x, int y) {
            a = x; b = y;
        }
    }
    protected override void UpdateCan_and_xy(int game, int k) {
        if (choice == 4) {
            if (game != 1) choice = 0;
            else newLife();
        }
        else {
            if (doubt) {
                if (x == exit[0] && y == exit[1] && game != 2) newLife();
                else if (game == 0) newLife();
                else if (game == 1) move(k);
                else if (game == 2) { doubt = false; x = exit[0]; y = exit[1]; choice = 0; }
                if (x == -1 || y == -1 || x == 19 || y == 19) newLife();
            }
            else {
                if (game == 0) { updateCan(-1, k); }
                if (game == 1) { updateCan(1, k); move(k); }
                if (game == 2) { updateCan(-1, k); exit[0] = x; exit[1] = y; exit[2] = k; }
            }
        }
    }
    void newLife() {
        x = y = 9;
        aftHosp = false;
        can_to_move = new int[20, 20, 2];
        choice = 0;
        exit[0] = exit[1] = exit[2] = -1;
        doubt = false;
    }
    protected override void NewBFS() {
        used = new bool[20, 20];
        p = new int[20, 20][];
        path = new int[101][];
        notExpl_size = path_size = 0;
    }
    new struct player {
        public int treasures;
    }
    new player[] players;
    public override void Update(string ansType_id, string ansSide_id, string gameAns_id, int id) {
        if (!broken) {
            try {
                int game = GameAns(gameAns_id); k = side_to_int(ansSide_id);
                if (id == my_id) {
                    if (ansType_id != "step") { if (game == 2 && SmbLosed()) choice = 2; if (game == 0 && choice != 4) newLife(); if (choice == 4) choice = 0; }
                    else UpdateCan_and_xy(game, k);
                    UpdateAns();
                }
                else {
                    if (gameAns_id == "hit\n") {
                        if (ansType_id == "throw") {
                            if (knifes > 0) { ansType = "strike"; ansSide = (Have(FREE, x, y)) ? Random(FREE) : Random(UNKNOWN); }
                            else if (bullets > 0) { ansType = "fire"; ansSide = (Have(FREE, x, y)) ? Random(FREE) : Random(UNKNOWN); }
                            else { ansType = "step"; ansSide = (Have(WALL, x, y)) ? Random(WALL) : Random(UNKNOWN); }
                            choice = 4;
                        }
                        else if (Check.treasures(my_id) == 0 && armors == 0) {
                            if (treasures > 0 || exit[0] == -1) { newLife(); UpdateAns(); }
                            else {
                                doubt = true;
                                v[0] = exit[0]; v[1] = exit[1];
                                if (x == v[0] && y == v[1]) choice = 0;
                                else {
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
            catch (System.Exception e) {
                broken = true;
                randomAns();
                error += e.Message + "\n" + e.StackTrace + "\n";
            }
        }
        else randomAns();
    }
    protected override bool SmbLosed() {
        for (int i = 0; i < Players; ++i)
            if (players[i].treasures > Check.treasures(i))
                return true;
        return false;
    }
}
public class Bot_Bob: Bot_v1 {

}
public class Bot_Jam: Bot_v1 {
    protected void BFS(int[,] ind) {
        NewBFS();
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
                    if (ind[to[0], to[1]] > 0 && notExpl_size < 1)
                        if (Check.treasures(ind[to[0], to[1]] - 1) > 0)
                            notExpl[++notExpl_size] = to;
                }
            }
        }
    }
    protected override bool IsJam() => true;
    protected int dspy_x, dspy_y, is_spy;
    protected override void Spy_on(int id) {
        is_spy = 0;
        my_map = Merge(my_map, players[id].B);
        if (is_spy == 1) {
            players[id].spy = true; players[id].dspy_x = dspy_x; players[id].dspy_y = dspy_y;
        }
    }
    protected override void Spy_off(int id) => players[id].spy = false;
    protected override void Spy_d(int a, int b) {
        is_spy = 1; dspy_x = a; dspy_y = b;
    }
    protected override void TryKill() {
        players[my_id].spy = false;
        if (choice != 4 && (knifes + bullets > 0)) {
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
                            void Win(int xx) { ansSide = sides[xx]; ansType = "strike"; profit = players[i].treasures; choice = 0; hit = true; }
                            if (dx == -1 && dy == 0 && Can(x, y, 0) == 1) { Win(0); continue; }
                            if (dx == 0 && dy == -1 && Can(x, y, 1) == 1) { Win(1); continue; }
                            if (dx == 1 && dy == 0 && Can(x, y, 2) == 1) { Win(2); continue; }
                            if (dx == 0 && dy == 1 && Can(x, y, 3) == 1) { Win(3); continue; }
                        }
                        if (bullets > 0) {
                            void Win(int xx) { ansSide = sides[xx]; ansType = "fire"; profit = players[i].treasures; choice = 0; hit = true; }
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
            if (spy && !hit && choice != 2 && choice != 3 && (3 * treasures <= Treasures - SumOut() || my_map.exit[0] < 0))// && treasures==0)
            {
                int[,] posPlayer = new int[2 * Size - 1, 2 * Size - 1];
                for (int i = 0; i < Players; ++i) {
                    if (players[i].spy && players[i].treasures > 0) {
                        int x = players[i].X(), y = players[i].Y();
                        if (x != my_map.x || y != my_map.y) posPlayer[x, y] = i + 1;
                    }
                }
                BFS(posPlayer);
                if (notExpl_size != 0) {
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