using System;
using System.Diagnostics;
using System.Linq;

class ItMain {
    static int numberOfGames = 500;
    static int steps = 0;
    public static Random rand = new Random();
    static int Main() {
        int errors = 0;
        Console.Write($" Хотите ввести число игр? (y/n) ");
        if (Console.ReadKey().Key == ConsoleKey.Y) {
            Console.WriteLine(" ");
            numberOfGames = int.Parse(Console.ReadLine());
        } else Console.WriteLine(" ");

        First[] games = new First[numberOfGames];
        for (int i = 0; i < numberOfGames; ++i) {
            try {
                games[i] = new First();
                games[i].show();
                if (Bot.error != "") {
                    Console.Write(Bot.error);
                    ++errors;
                    Bot.error = "";
                }
                steps += First.steps / First.players;
            }
            catch (Exception e) {
                Console.WriteLine($" Game error: {e.Message+"\n"+e.StackTrace}");
            }
        }
        Console.WriteLine($" Среднее число ходов: {steps / numberOfGames}");
        Console.WriteLine($" Число ошибок: {errors} из {numberOfGames}");
        Console.ReadLine();
        return 0;
    }
}
class First {
    public int show() => first();
    public static int id = -1, players = 2, human = 0, treasures = 100, size = 5, steps = 0;
    public static string[] name;
    public static Bot[] Alice;
    public static Map lab;
    static Random rand = ItMain.rand;
    public static int first() {
        bool withWrLn = true, withPauses = true, Bot_show = false;
        human = 0;
        players = 4; treasures = 100; size = 5;
        id = -1; steps = 0;
        was = new bool[2 * size - 1, 2 * size - 1, players];
        name = new string[players];
        Alice = new Bot[players];
        lab = new Map();
        for (int i = 0; i < players; ++i) {
            lab.player[i].knifes = 1;
            lab.player[i].bullets = i == 0 ? 100 : 0;
            lab.player[i].armors = 0;
            Alice[i] = new Bot_Jam();
            Alice[i].Join(players, treasures, size, i);
            name[i] = getName();
            // string[] s = new string[] { "Освальд", "Эдвард", "Барбара", "Рейчел", "Эшли" };
            //if (players == s.Length) takeName(s);
        }
        //lab.Show();
        //Console.ReadKey(true);
        while (sum() != treasures) {
            //if (Console.ReadKey(false).Key == ConsoleKey.Escape) break;
            id = (id + 1) % players;
            int kn = Check.knifes(id), bl = Check.bullets(id),
                ar = Check.armors(id), cr = Check.crackers(id);
            int trs = lab.player[id].treasures;
            if (id < human) {
                string s, type = "", side = "";
                string[] types = { "step", "strike", "fire", "throw" };
                string[] sides = { "left", "down", "right", "up" };
                while (true) {
                    s = Console.ReadLine(); bool end = false;
                    try {
                        int pos = s.IndexOf(' ');
                        type = Alice[id].ansType = s.Substring(0, pos);
                        side = Alice[id].ansSide = s.Substring(pos + 1, s.Length - pos - 1);
                    }
                    catch { }
                    if (types.Contains(type) && sides.Contains(side)) end = true;
                    else Console.WriteLine("Incorrect! Try again!");
                    if (end) break;
                }
            }
            string gameAns;
            gameAns = lab.GameAns(Alice[id].ansType, Alice[id].ansSide, id);
            string ansType = Alice[id].ansType, ansSide = Alice[id].ansSide;
            //was[Alice[id].my_map.x, Alice[id].my_map.y, id] = true;
            if (withWrLn) {
                Console.WriteLine($" {name[id]}: {ansType} {ansSide}");
                Console.WriteLine($" {gameAns}");
                lab.Show();
            }
            for (int i = human; i < players; ++i) {
                Alice[i].Update(ansType, ansSide, gameAns, id);
                if (Bot_show) updateBotShow(((Bot_Bob)Alice[i]).my_map, i);
            }
            if (withWrLn) {
                bot_show(((Bot_Bob)Alice[id]).my_map, id);
                bool[] isEvent = {
                    gameAns == "exit\n" && trs != lab.player[id].treasures,
                    ansType == "strike" && gameAns == "hit\n",
                    ansType == "fire" && gameAns == "hit\n",
                    ansType == "step" && (kn != Check.knifes(id) || bl != Check.bullets(id) ||
                    ar!=Check.armors(id) || cr!=Check.crackers(id))
                };
                string[] Event = {
                    $" {trs} treasures thrown out by {name[id]}!",
                    $" {name[id]} takes murder with knife!",
                    $" {name[id]} takes murder with fire!",
                    $" {name[id]} gets weapon from arsenal!"
                };
                void printInfo(bool A, string s) {
                    if (A) {
                        Console.WriteLine(s);
                        if (s == Event[1] || s == Event[2]) {
                            s = " ";
                            int deathes = 0;
                            for (int i = 0; i < players; ++i)
                                if (lab.killed[i]) { s += $"{name[i]}, "; ++deathes; }
                            if (deathes == 0) s += "No one";
                            else s = s.Substring(0, s.Length - 2);
                            if (deathes <= 1) s += " is killed";
                            else s += " are killed";
                            Console.WriteLine(s);
                        }
                        if (withPauses) {
                            Console.WriteLine(" Press any key to continue... \n");
                            Console.ReadKey();
                        }
                        //else
                        //Thread.Sleep(3200);
                    }
                }
                for (int i = 0; i < Event.Length; ++i) printInfo(isEvent[i], Event[i]);

            }
            //withWrLn = true;
            //if (gameAns == "hit\n" && Alice[id].smbLosed()) Thread.Sleep(SleepTimeAfterGame);
            ++steps;
            //Thread.Sleep(1000);

            //Console.MoveBufferArea(0, 0, 100, 1, 0, 40);
            //Console.Clear();
        }
        if (withWrLn) Console.WriteLine();
        Console.WriteLine(" Game over with {0} steps. players = {1}, treasures = {2}, size = {3}", steps / players, players, treasures, size);
        if (withWrLn) for (int i = 0; i < players; ++i) { Console.WriteLine(" {0} treasures: {1}", name[i], lab.player[i].treasuresOut); }
        if (withWrLn) Console.WriteLine();
        if (withWrLn) Console.WriteLine(" If you want to end this process press \"Escape\",\n If you want to continue press any other key");
        if (withWrLn) if (Console.ReadKey().Key == ConsoleKey.Escape) Process.GetCurrentProcess().Kill();
        return 0;
    }
    public static void updateBotShow(Bot_Bob.Map map, int id) {
        for (int i = 0; i < 2 * size - 1; ++i)
            for (int j = 0; j < 2 * size - 1; ++j)
                was[i, j, id] = false;
        was[map.x, map.y, id] = true;
    }
    static void takeName(string[] s) {
        bool[] a = new bool[s.Length];
        for (int i = 0; i < s.Length; ++i) {
            int x = rand.Next(s.Length);
            while (a[x]) x = rand.Next(s.Length);
            name[i] = s[x];
            a[x] = true;
        }
    }
    static int sum() {
        int x = 0;
        for (int i = 0; i < players; ++i) x += lab.player[i].treasuresOut;
        return x;
    }
    static string getName() {
        Random rand = ItMain.rand;
        if (rand.Next(2) == 0) return Name.man[rand.Next(Name.man.Length)];
        else return Name.woman[rand.Next(Name.woman.Length)];
    }
    static string getName(int i) => (i + 1).ToString();
    public static void bot_show(Bot_Bob.Map map, int id) {
        void print(int a) {
            try {
                Console.WriteLine(
                    $"   treasures {name[players - a]}: {lab.player[players - a].treasures}," +
                    $" out: {lab.player[players - a].treasuresOut}," +
                    $" my_map.exit: {((Bot_Bob)Alice[players - a]).my_map.exit[0] > -1}," +
                    $" steps: {steps / players}"
                    );
            }
            catch { Console.WriteLine(" Sorry, there was an error "); }
        }
        Bot_Bob.Map this_map = map;
        int Size = 2 * size - 1;
        for (int j = 2 * (Size + 1) - 1; j > 0; --j)
            for (int i = 1; i < 2 * (Size + 1); ++i) {
                if (i % 2 == 1 && j % 2 == 1) Console.Write("═╬═");
                if (i % 2 == 0 && j % 2 == 1) {
                    if (this_map.can_to_move[(i - 1) / 2, (j - 1) / 2, 1] == 1) Console.Write("   ");
                    if (this_map.can_to_move[(i - 1) / 2, (j - 1) / 2, 1] == 0) Console.Write(" ? ");
                    if (this_map.can_to_move[(i - 1) / 2, (j - 1) / 2, 1] < 0) {
                        Console.ForegroundColor = ConsoleColor.Red;
                        if ((i - 1) / 2 == this_map.exit[0] && (j - 1) / 2 == this_map.exit[1] && 1 == this_map.exit[2]) Console.Write(" E ");
                        else if ((i - 1) / 2 == this_map.exit[0] && (j - 3) / 2 == this_map.exit[1] && 3 == this_map.exit[2]) Console.Write(" E ");
                        else Console.Write("═══");
                        Console.ResetColor();
                    }
                }
                if (i % 2 == 1 && j % 2 == 0) {
                    if (this_map.can_to_move[(i - 1) / 2, (j - 1) / 2, 0] == 1) Console.Write("   ");
                    if (this_map.can_to_move[(i - 1) / 2, (j - 1) / 2, 0] == 0) Console.Write(" ? ");
                    if (this_map.can_to_move[(i - 1) / 2, (j - 1) / 2, 0] < 0) {
                        Console.ForegroundColor = ConsoleColor.Red;
                        if ((i - 1) / 2 == this_map.exit[0] && (j - 1) / 2 == this_map.exit[1] && 0 == this_map.exit[2]) Console.Write(" E ");
                        else if ((i - 3) / 2 == this_map.exit[0] && (j - 1) / 2 == this_map.exit[1] && 2 == this_map.exit[2]) Console.Write(" E ");
                        else Console.Write(" ║ ");
                        Console.ResetColor();
                    }
                }
                if (i % 2 == 0 && j % 2 == 0)
                    if (this_map.x == (i - 1) / 2 && this_map.y == (j - 1) / 2) {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(" I ");
                        Console.ResetColor();
                    } else {
                        if (was[(i - 1) / 2, (j - 1) / 2, id]) {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(" o ");
                            Console.ResetColor();
                        } else Console.Write(" o ");
                    }
                if (i == 2 * Size + 1)
                    if (j <= players)
                        print(j);
                    else
                        Console.WriteLine();
            }
        Console.WriteLine();
    }
    public static bool[,,] was = new bool[2 * size - 1, 2 * size - 1, players];
}
class Map {
    int[] arsSettings = new int[] { 20, 10, 6, 0 };
    Random rand = ItMain.rand;
    int massRand(int[] mass) {
        int[] sum = new int[mass.Length + 1];
        sum[0] = 0;
        for (int i = 0; i < mass.Length; ++i) sum[i + 1] = sum[i] + mass[i];
        int x = rand.Next(sum[mass.Length]);
        for (int i = 0; i < mass.Length; ++i) if (sum[i] <= x && x < sum[i + 1]) return i;
        return -1;
    }
    int players = First.players, treasures = First.treasures, size = First.size;
    int hos0, hos1, ars0, ars1;
    int arsRecharge = 0, arsNum = -1;
    int[,,] can = new int[First.size + 1, First.size + 1, 5];
    int[,] treasure = new int[First.size, First.size];
    public bool[] killed = new bool[First.players];
    public struct game_player {
        public int x, y, treasures, treasuresOut, knifes, bullets, armors, crackers;
    }
    public game_player[] player = new game_player[First.players];
    int Can(int x, int y, int k) {
        int dx = (k == 2 ? 1 : 0), dy = (k == 3 ? 1 : 0), t = (k > 1) ? dy : k;
        return can[x + dx, y + dy, t];
    }
    void Move(int k, int id) {
        if (k == 0) player[id].x--;
        if (k == 1) player[id].y--;
        if (k == 2) player[id].x++;
        if (k == 3) player[id].y++;
    }
    void Transport(int x, int y, int id) {
        player[id].x = x;
        player[id].y = y;
    }
    public Map() {
        void ChangeCan(int x, int y, int k, int res) {
            int dx = (k == 2 ? 1 : 0), dy = (k == 3 ? 1 : 0), t = (k > 1) ? dy : k;
            can[x + dx, y + dy, t] = res;
        }
        for (int i = 0; i < players; ++i) {
            player[i] = new game_player {
                x = rand.Next(size),
                y = rand.Next(size),
                knifes = 1
            };
        }
        for (int i = 0; i < treasures; ++i) {
            ++treasure[rand.Next(size), rand.Next(size)];
        }
        while (!false) {
            for (int y = 0; y < size; ++y) { ChangeCan(0, y, 0, -1); ChangeCan(size - 1, y, 2, -1); }
            for (int x = 0; x < size; ++x) { ChangeCan(x, 0, 1, -1); ChangeCan(x, size - 1, 3, -1); }
            for (int x = 0; x < size; ++x)
                for (int y = 0; y < size; ++y)
                    for (int k = 0; k < 4; ++k)
                        if (rand.Next(7) == 0) ChangeCan(x, y, k, -1);
            bool[,] used = new bool[size, size];
            int DFS(int x, int y) {
                used[x, y] = true;
                int sum = 1;
                if (Can(x, y, 0) == 0) if (!used[x - 1, y]) sum += DFS(x - 1, y);
                if (Can(x, y, 1) == 0) if (!used[x, y - 1]) sum += DFS(x, y - 1);
                if (Can(x, y, 2) == 0) if (!used[x + 1, y]) sum += DFS(x + 1, y);
                if (Can(x, y, 3) == 0) if (!used[x, y + 1]) sum += DFS(x, y + 1);
                return sum;
            }
            if (DFS(0, 0) == size * size) break;
            else
                for (int x = 0; x < size; ++x)
                    for (int y = 0; y < size; ++y)
                        for (int k = 0; k < 4; ++k)
                            ChangeCan(x, y, k, 0);
        }
        hos0 = rand.Next(size); hos1 = rand.Next(size);
        ars0 = rand.Next(size); ars1 = rand.Next(size);
        int xe, ye, ke = rand.Next(4);
        switch (ke) {
            case 0:
                xe = 0;
                ye = rand.Next(size);
                ChangeCan(xe, ye, ke, -2);
                break;
            case 1:
                xe = rand.Next(size);
                ye = 0;
                ChangeCan(xe, ye, ke, -2);
                break;
            case 2:
                xe = size - 1;
                ye = rand.Next(size);
                ChangeCan(xe, ye, ke, -2);
                break;
            case 3:
                xe = rand.Next(size);
                ye = size - 1;
                ChangeCan(xe, ye, ke, -2);
                break;
            default:
                break;
        }
    }
    public string GameAns(string AnsType, string AnsSide, int id) {
        string game = "";
        for (int i = 0; i < players; ++i) killed[i] = false;
        int AnsSide_to_int(string m) {
            string[] s = { "left", "down", "right", "up" };
            for (int i = 0; i < s.Length; ++i) if (m == s[i]) return i;
            return -1;
        }
        int x = player[id].x, y = player[id].y, k = AnsSide_to_int(AnsSide);
        if (arsNum == id) {
            if (arsRecharge > 0) ++arsRecharge;
            if (arsRecharge == 6) arsRecharge = 0;
        }
        if (AnsType == "step") {
            if (Can(x, y, k) == 0) {
                Move(k, id);
                x = player[id].x; y = player[id].y;
                bool trs = treasure[x, y] > 0, ars = x == ars0 && y == ars1 && arsRecharge == 0;
                if (!trs && !ars) game += "go\n";
                if (trs) {
                    game += "treasure " + treasure[x, y].ToString() + "\n";
                    player[id].treasures += treasure[x, y];
                    treasure[x, y] = 0;
                }
                if (ars) {
                    switch (massRand(arsSettings)) {
                        case 0:
                            game += "knife\n";
                            ++player[id].knifes;
                            break;
                        case 1:
                            game += "bullet\n";
                            ++player[id].bullets;
                            break;
                        case 2:
                            game += "armor\n";
                            ++player[id].armors;
                            break;
                        case 3:
                            game += "cracker\n";
                            ++player[id].crackers;
                            break;
                        default:
                            break;
                    }
                    arsNum = (id + 1) % players;
                    ++arsRecharge;
                }
            } else if (Can(x, y, k) == -1) game += "wall\n";
            else {
                game += "exit\n";
                player[id].treasuresOut += player[id].treasures;
                player[id].treasures = 0;
            }
        } else if (AnsType == "strike") {
            if (Can(x, y, k) == 0) {
                Move(k, id);
                bool hit = false;
                int xp = player[id].x, yp = player[id].y;
                for (int i = 0; i < players; ++i) {
                    if (i != id && player[i].x == xp && player[i].y == yp) {
                        hit = true;
                        if (player[i].armors > 0) --player[i].armors;
                        else {
                            treasure[xp, yp] += player[i].treasures;
                            player[i].treasures = 0;
                            killed[i] = true;
                            Transport(hos0, hos1, i);
                        }
                    }
                }
                Transport(x, y, id);
                if (hit) { game += "hit\n"; --player[id].knifes; } else game += "miss\n";
            } else if (Can(x, y, k) == -1) { game += "wall\n"; --player[id].knifes; } else game += "miss\n";
        } else if (AnsType == "fire") {
            --player[id].bullets;
            bool hit = false;
            int xp = player[id].x, yp = player[id].y;
            while (Can(xp, yp, k) == 0 && !hit) {
                Move(k, id);
                xp = player[id].x; yp = player[id].y;
                for (int i = 0; i < players; ++i) {
                    if (i != id && player[i].x == xp && player[i].y == yp) {
                        hit = true;
                        if (player[i].armors > 0) --player[i].armors;
                        else {
                            treasure[xp, yp] += player[i].treasures;
                            player[i].treasures = 0;
                            killed[i] = true;
                            Transport(hos0, hos1, i);
                        }
                    }
                }
            }
            Transport(x, y, id);
            if (hit) game += "hit\n";
            else game += "miss\n";
        }
        return game;
    }
    public void Show() {
        void printInfo(int id) {
            string name = First.name[id];
            int trs = player[id].treasures, tout = player[id].treasuresOut;
            int kn = player[id].knifes, bl = player[id].bullets, ar = player[id].armors;
            int cr = player[id].crackers;
            Console.WriteLine($"{name} trs: {trs}, out: {tout}, k,b,a,c: {{ {kn}, {bl}, {ar}, {cr} }}");
        }
        void print(string s, ConsoleColor color) {
            Console.ForegroundColor = color;
            Console.Write(s);
            Console.ResetColor();
        }
        for (int j = 2 * size - 1; j >= -1; --j)
            for (int i = -1; i <= 2 * size - 1; ++i) {
                if (i % 2 != 0 && j % 2 != 0) {
                    if (i == -1 && j == 2 * size - 1) Console.Write(" ╔═");
                    else if (i == 2 * size - 1 && j == 2 * size - 1) Console.Write("═╗ ");
                    else if (i == -1 && j == -1) Console.Write(" ╚═");
                    else if (i == 2 * size - 1 && j == -1) Console.Write("═╝ ");
                    else if (i == -1) Console.Write(" ╠═");
                    else if (i == 2 * size - 1) Console.Write("═╣ ");
                    else if (j == 2 * size - 1) Console.Write("═╦═");
                    else if (j == -1) Console.Write("═╩═");
                    else {
                        int x = 0;
                        if (Can((i + 1) / 2, (j - 1) / 2, 0) < 0) x += 1;
                        if (Can((i - 1) / 2, (j - 1) / 2, 3) < 0) x += 2;
                        if (Can((i - 1) / 2, (j + 1) / 2, 2) < 0) x += 4;
                        if (Can((i + 1) / 2, (j + 1) / 2, 1) < 0) x += 8;
                        if (x == 0) Console.Write("   ");
                        if (x == 3) Console.Write("═╗ ");
                        if (x == 6) Console.Write("═╝ ");
                        if (x == 7) Console.Write("═╣ ");
                        if (x == 9) Console.Write(" ╔═");
                        if (x == 11) Console.Write("═╦═");
                        if (x == 12) Console.Write(" ╚═");
                        if (x == 13) Console.Write(" ╠═");
                        if (x == 14) Console.Write("═╩═");
                        if (x == 1 || x == 4 || x == 5 || x == 2 || x == 8 || x == 10 || x == 15) Console.Write("═╬═");
                    }
                }
                if (i % 2 != 0 && j % 2 == 0) {
                    if (Can((i + 1) / 2, j / 2, 0) == -1) Console.Write(" ║ ");
                    else Console.Write("   ");
                }
                if (i % 2 == 0 && j % 2 != 0) {
                    if (Can(i / 2, (j + 1) / 2, 1) == -1) Console.Write("═══");
                    else Console.Write("   ");
                }
                if (i % 2 == 0 && j % 2 == 0) {
                    bool x = false;
                    for (int id = 0; id < players && !x; ++id) {
                        if (player[id].x == i / 2 && player[id].y == j / 2) {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(" {0} ", id + 1); x = true;
                            Console.ResetColor();
                        }
                    }
                    if (!x) {
                        if (treasure[i / 2, j / 2] > 0) print(" t ", ConsoleColor.DarkBlue);
                        else if (ars0 == i / 2 && ars1 == j / 2) print(" a ", ConsoleColor.Green);
                        else if (hos0 == i / 2 && hos1 == j / 2) print(" h ", ConsoleColor.Green);
                        else Console.Write(" o ");
                    }
                }
                if (i == 2 * size - 1)
                    if (2 * size - 1 - j < players)
                        printInfo(2 * size - 1 - j);
                    else
                        Console.WriteLine();
            }
    }
}
class Check {
    public static int treasures(int id) => First.lab.player[id].treasures;
    public static int treasureOut(int id) => First.lab.player[id].treasuresOut;
    public static int knifes(int id) => First.lab.player[id].knifes;
    public static int armors(int id) => First.lab.player[id].armors;

    public static int bullets(int id) => First.lab.player[id].bullets;
    public static int crackers(int id) => First.lab.player[id].crackers;
}
class Name {
    public static string[] man = { "Аарон", "Абрам", "Аваз", "Августин", "Авраам", "Агап", "Агапит", "Агат", "Агафон", "Адам", "Адриан", "Азамат", "Азат", "Азиз", "Аид", "Айдар", "Айрат", "Акакий", "Аким", "Алан", "Александр", "Алексей", "Али", "Алик", "Алим", "Алихан", "Алишер", "Алмаз", "Альберт", "Амир", "Амирам", "Амиран", "Анар", "Анастасий", "Анатолий", "Анвар", "Ангел", "Андрей", "Анзор", "Антон", "Анфим", "Арам", "Аристарх", "Аркадий", "Арман", "Армен", "Арсен", "Арсений", "Арслан", "Артём", "Артемий", "Артур", "Архип", "Аскар", "Аслан", "Асхан", "Асхат", "Ахмет", "Ашот", "Бахрам", "Бенджамин", "Блез", "Богдан", "Борис", "Борислав", "Бронислав", "Булат", "Вадим", "Валентин", "Валерий", "Вальдемар", "Вардан", "Василий", "Вениамин", "Виктор", "Вильгельм", "Вит", "Виталий", "Влад", "Владимир", "Владислав", "Владлен", "Влас", "Всеволод", "Вячеслав", "Гавриил", "Гамлет", "Гарри", "Геннадий", "Генри", "Генрих", "Георгий", "Герасим", "Герман", "Германн", "Глеб", "Гордей", "Григорий", "Густав", "Давид", "Давлат", "Дамир", "Дана", "Даниил", "Данил", "Данис", "Данислав", "Даниэль", "Данияр", "Дарий", "Даурен", "Демид", "Демьян", "Денис", "Джамал", "Джан", "Джеймс", "Джереми", "Иеремия", "Джозеф", "Джонатан", "Дик", "Дин", "Динар", "Дино", "Дмитрий", "Добрыня", "Доминик", "Евгений", "Евдоким", "Евсей", "Евстахий", "Егор", "Елисей", "Емельян", "Еремей", "Ефим", "Ефрем", "Ждан", "Жерар", "Жигер", "Закир", "Заур", "Захар", "Зенон", "Зигмунд", "Зиновий", "Зураб", "Зуфар", "Ибрагим", "Иван", "Игнат", "Игнатий", "Игорь", "Иероним", "Джером", "Иисус", "Ильгиз", "Ильнур", "Ильшат", "Илья", "Ильяс", "Имран", "Иннокентий", "Ираклий", "Исаак", "Исаакий", "Исидор", "Искандер", "Ислам", "Исмаил", "Итан", "Казбек", "Камиль", "Карен", "Карим", "Карл", "Ким", "Кир", "Кирилл", "Клаус", "Клим", "Конрад", "Константин", "Коре", "Корнелий", "Кристиан", "Кузьма", "Лаврентий", "Ладо", "Лев", "Ленар", "Леон", "Леонард", "Леонид", "Леопольд", "Лоренс", "Лука", "Лукиллиан", "Лукьян", "Любомир", "Людвиг", "Людовик", "Люций", "Маджид", "Майкл", "Макар", "Макарий", "Максим", "Максимилиан", "Максуд", "Мансур", "Мар", "Марат", "Марк", "Марсель", "Мартин", "Мартын", "Матвей", "Махмуд", "Мика", "Микула", "Милослав", "Мирон", "Мирослав", "Михаил", "Моисей", "Мстислав", "Мурат", "Муслим", "Мухаммед", "Мэтью", "Назар", "Наиль", "Нариман", "Нестор", "Ник", "Никита", "Никодим", "Никола", "Николай", "Нильс", "Огюст", "Олег", "Оливер", "Орест", "Орландо", "Осип", "Иосиф", "Оскар", "Осман", "Остап", "Остин", "Павел", "Панкрат", "Патрик", "Педро", "Перри", "Пётр", "Платон", "Потап", "Прохор", "Равиль", "Радий", "Радик", "Радомир", "Радослав", "Разиль", "Раиль", "Райан", "Раймонд", "Рамазан", "Рамзес", "Рамиз", "Рамиль", "Рамон", "Ранель", "Расим", "Расул", "Ратибор", "Ратмир", "Раушан", "Рафаэль", "Рафик", "Рашид", "Ринат", "Ренат", "Ричард", "Роберт", "Родим", "Родион", "Рожден", "Ролан", "Роман", "Ростислав", "Рубен", "Рудольф", "Руслан", "Рустам", "Рэй", "Савва", "Савелий", "Саид", "Салават", "Самат", "Самвел", "Самир", "Самуил", "Санжар", "Сани", "Святослав", "Севастьян", "Семён", "Серафим", "Сергей", "Сидор", "Симба", "Соломон", "Спартак", "Станислав", "Степан", "Сулейман", "Султан", "Сурен", "Тагир", "Таир", "Тайлер", "Талгат", "Тамаз", "Тамерлан", "Тарас", "Тахир", "Тигран", "Тимофей", "Тимур", "Тихон", "Томас", "Трофим", "Уинслоу", "Умар", "Устин", "Фазиль", "Фарид", "Фархад", "Фёдор", "Федот", "Феликс", "Филипп", "Флор", "Фома", "Фред", "Фридрих", "Хабиб", "Хаким", "Харитон", "Хасан", "Цезарь", "Цефас", "Цецилий", "Сесил", "Цицерон", "Чарльз", "Чеслав", "Чингиз", "Шамиль", "Шарль", "Шерлок", "Эдгар", "Эдуард", "Эльдар", "Эмиль", "Эмин", "Эрик", "Эркюль", "Эрмин", "Эрнест", "Эузебио", "Юлиан", "Юлий", "Юнус", "Юрий", "Юстиниан", "Юстус", "Яков", "Ян", "Яромир", "Ярослав" };
    public static string[] woman = { "Ава", "Августа", "Августина", "Авдотья", "Аврора", "Агапия", "Агата", "Агафья", "Аглая", "Агния", "Агунда", "Ада", "Аделаида", "Аделина", "Адель", "Адиля", "Адриана", "Аза", "Азалия", "Азиза", "Аида", "Аиша", "Ай", "Айару", "Айгерим", "Айгуль", "Айлин", "Айнагуль", "Айнур", "Айсель", "Айсун", "Айсылу", "Аксинья", "Алана", "Алевтина", "Александра", "Алёна", "Алеста", "Алина", "Алиса", "Алия", "Алла", "Алсу", "Алтын", "Альба", "Альбина", "Альфия", "Аля", "Амалия", "Амаль", "Амина", "Амира", "Анаит", "Анастасия", "Ангелина", "Анжела", "Анжелика", "Анисья", "Анита", "Анна", "Антонина", "Анфиса", "Аполлинария", "Арабелла", "Ариадна", "Ариана", "Арианда", "Арина", "Ария", "Асель", "Асия", "Астрид", "Ася", "Афина", "Аэлита", "Аяна", "Бажена", "Беатрис", "Бела", "Белинда", "Белла", "Бэлла", "Берта", "Богдана", "Божена", "Бьянка", "Валентина", "Валерия", "Ванда", "Ванесса", "Варвара", "Василина", "Василиса", "Венера", "Вера", "Вероника", "Веста", "Вета", "Викторина", "Виктория", "Вилена", "Виола", "Виолетта", "Вита", "Виталина", "Виталия", "Влада", "Владана", "Владислава", "Габриэлла", "Галина", "Галия", "Гаяна", "Гаянэ", "Генриетта", "Глафира", "Гоар", "Грета", "Гульзира", "Гульмира", "Гульназ", "Гульнара", "Гульшат", "Гюзель", "Далида", "Дамира", "Дана", "Даниэла", "Дания", "Дара", "Дарина", "Дарья", "Даяна", "Джамиля", "Дженна", "Дженнифер", "Джессика", "Джиневра", "Диана", "Дильназ", "Дильнара", "Диля", "Дилярам", "Дина", "Динара", "Долорес", "Доминика", "Домна", "Домника", "Ева", "Евангелина", "Евгения", "Евдокия", "Екатерина", "Елена", "Елизавета", "Есения", "Жаклин", "Жанна", "Жансая", "Жасмин", "Жозефина", "Жоржина", "Забава", "Заира", "Залина", "Замира", "Зара", "Зарема", "Зарина", "Земфира", "Зинаида", "Зита", "Злата", "Златослава", "Зоряна", "Зоя", "Зульфия", "Зухра", "Иветта", "Ивета", "Изабелла", "Илина", "Иллирика", "Илона", "Ильзира", "Илюза", "Инга", "Индира", "Инесса", "Инна", "Иоанна", "Ира", "Ирада", "Ираида", "Ирина", "Ирма", "Искра", "Камила", "Камилла", "Кара", "Каре", "Карима", "Карина", "Каролина", "Кира", "Клавдия", "Клара", "Кора", "Корнелия", "Кристина", "Ксения", "Лада", "Лана", "Лара", "Лариса", "Лаура", "Лейла", "Леона", "Лера", "Леся", "Лета", "Лиана", "Лидия", "Лиза", "Лика", "Лили", "Лилиана", "Лилит", "Лилия", "Лина", "Линда", "Лиора", "Лира", "Лия", "Лола", "Лолита", "Лора", "Луиза", "Лукерья", "Лукия", "Луна", "Любава", "Любовь", "Людмила", "Люсиль", "Люсьена", "Люция", "Люче", "Ляйсан", "Ляля", "Мавиле", "Мавлюда", "Магда", "Магдалeна", "Мадина", "Мадлен", "Майя", "Макария", "Малика", "Мара", "Маргарита", "Марианна", "Марика", "Марина", "Мария", "Мариям", "Марта", "Марфа", "Мелания", "Мелисса", "Мика", "Мила", "Милада", "Милана", "Милен", "Милена", "Милица", "Милослава", "Мира", "Мирослава", "Мирра", "Мишель", "Мия", "Моника", "Муза", "Надежда", "Наиля", "Наима", "Нана", "Наоми", "Наргиза", "Наталья", "Нелли", "Нея", "Ника", "Николь", "Нина", "Нинель", "Номина", "Нора", "Нурия", "Одетта", "Оксана", "Октябрина", "Олеся", "Оливия", "Ольга", "Офелия", "Павлина", "Памела", "Патриция", "Паула", "Пейтон", "Пелагея", "Перизат", "Платонида", "Полина", "Прасковья", "Равшана", "Рада", "Разина", "Раиля", "Раиса", "Ралина", "Рамина", "Раяна", "Ребекка", "Регина", "Резеда", "Рена", "Рената", "Риана", "Рианна", "Рикарда", "Римма", "Рина", "Рита", "Рогнеда", "Роза", "Роксана", "Роксолана", "Рузалия", "Рузанна", "Русалина", "Руслана", "Руфина", "Руфь", "Сабина", "Сабрина", "Сажида", "Саида", "Салима", "Саломея", "Сальма", "Самира", "Сандра", "Сания", "Сара", "Сати", "Сауле", "Сафия", "Сафура", "Светлана", "Севара", "Селена", "Сельма", "Серафима", "Сильвия", "Симона", "Снежана", "Соня", "Софья", "Стелла", "Стефания", "Сусанна", "Таисия", "Тамара", "Тамила", "Тара", "Татьяна", "Тая", "Таяна", "Теона", "Тереза", "Тея", "Тина", "Тиффани", "Томирис", "Тора", "Тэмми", "Ульяна", "Урсула", "Устинья", "Фазиля", "Фаина", "Фарида", "Фариза", "Фатима", "Федора", "Фёкла", "Фелисити", "Фелиция", "Феруза", "Физалия", "Фируза", "Флора", "Флорентина", "Флоренция", "Флоренс", "Флориана", "Фредерика", "Фрида", "Хадия", "Хилари", "Хлоя", "Хюррем", "Цагана", "Цветана", "Цецилия", "Сесилия", "Циара", "Сиара", "Челси", "Чеслава", "Чулпан", "Шакира", "Шарлотта", "Шахина", "Шейла", "Шелли", "Шерил", "Эвелина", "Эвита", "Элеонора", "Элиана", "Элиза", "Элина", "Элла", "Эльвина", "Эльвира", "Эльмира", "Эльнара", "Эля", "Эмили", "Эмилия", "Эмма", "Энже", "Эрика", "Эрмина", "Эсмеральда", "Эсмира", "Эстер", "Этель", "Этери", "Юлианна", "Юлия", "Юна", "Юния", "Юнона", "Ядвига", "Яна", "Янина", "Ярина", "Ярослава", "Ясмина" };
}

public struct Coord: ICloneable {
    public int x;
    public int y;
    public Coord(int X, int Y) {
        x = X;
        y = Y;
    }

    public static bool operator ==(Coord cor1, Coord cor2) => (cor1.x == cor2.x && cor1.y == cor2.y);
    public static bool operator !=(Coord cor1, Coord cor2) => (cor1.x != cor2.x || cor1.y != cor2.y);

    public object Clone() => MemberwiseClone();
}