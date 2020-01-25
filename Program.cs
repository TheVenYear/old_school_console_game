using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Menu
{
    class Program
    {
        static int BackWidth = 120, BackHeight = 29, current = 0, money = 0, n, m, posy, posx, plus1 = 4, plus2 = 8, level = 1, level_count = 0, coin_count, current_coins = 0;
        static bool[] enabled = new bool[5];
        static bool game = true, pause = false, status = false, win = false, loaded = false, lose = false;
        static char[,] frame;
        static int[,] frame2;
        static char player = '&';
        static string CurrentMap, CurrentSwitch;

        static void Background()
        {
            for (int i = 0; i <= BackHeight; i++)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write(new String(' ', BackWidth));
            }

        }

        static void ShowBlocks()
        {
            string first = "Новая игра";
            if (pause || loaded) first = "Продолжить";
            int pos = (120 - 30) / 2;
            Block(pos, 2, first, enabled[0]);
            Block(pos, 9, "Загрузить игру", enabled[1]);
            if (pause)
            {
                Block(pos, 16, "Сохранить игру", enabled[2]);
                Block(pos, 23, " Выйти из игры", enabled[3]);
            }
            else Block(pos, 16, " Выйти из игры", enabled[3]);

        }

        static void Block(int a, int b, string text, bool enabled)
        {
            ConsoleColor firstcolor, secondcolor, foreground;
            if (!enabled)
            {
                firstcolor = ConsoleColor.Black;
                secondcolor = ConsoleColor.White;
                foreground = ConsoleColor.White;
            }
            else
            {
                firstcolor = ConsoleColor.White;
                secondcolor = ConsoleColor.Black;
                foreground = ConsoleColor.Black;
            }
            Console.SetCursorPosition(a, b);
            Console.ForegroundColor = foreground;
            var textheader = (30 - text.Length) / 2;
            for (int i = 0; i < 2; i++)
            {
                Console.BackgroundColor = firstcolor;
                Console.WriteLine(new String(' ', 30));
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write(new String(' ', a));
            }
            Console.BackgroundColor = firstcolor;
            Console.Write(new String(' ', textheader));
            Console.Write(text);
            Console.WriteLine(new String(' ', textheader));
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(new String(' ', a));
            for (int i = 0; i < 1; i++)
            {
                Console.BackgroundColor = firstcolor;
                Console.WriteLine(new String(' ', 30));
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write(new String(' ', a));
            }
            Console.BackgroundColor = secondcolor;
            Console.Write(new String(' ', 30));
            Console.BackgroundColor = ConsoleColor.Red;

        }

        static void Switch()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            ConsoleKeyInfo sender = Console.ReadKey();
            switch (sender.Key)
            {
                case ConsoleKey.UpArrow:
                    enabled[current] = false;
                    if (current - 1 < 0)
                        current = 3;
                    else if (current - 1 == 2 && !pause) current -= 2;
                    else current -= 1;
                    enabled[current] = true;
                    ShowBlocks();
                    break;
                case ConsoleKey.DownArrow:
                    enabled[current] = false;
                    if (current + 1 > 3)
                        current = 0;
                    else if (current + 1 == 2 && !pause) current += 2;
                    else current += 1;
                    enabled[current] = true;
                    ShowBlocks();
                    break;
                case ConsoleKey.Enter:
                    if (enabled[3] == true)
                        game = false;
                    else if (enabled[0] == true)
                        status = true;
                    else if (enabled[1] == true)
                        Load();
                    else if (enabled[2] == true)
                        Save();
                    break;
                case ConsoleKey.Escape:
                    if (pause)
                        status = true;
                    break;
            }
        }

        static void Load()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Сохранения: ");
            DirectoryInfo dir = new DirectoryInfo("saves/");
            int i = 1;
            foreach (DirectoryInfo item in dir.GetDirectories())
            {
                Console.WriteLine($"{i}: {item.Name}");
                i++;
            }
            var error = false;
            int chosen = 1;
            if (i == 1)
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Тут пусто :(, нажми любую клавишу");
                Console.ReadKey();
                MainMenu();
            }
            if (i != 1)
            {
                Console.WriteLine("Введите нужное сохранение: ");
                while (!error || chosen >= i || chosen < 1)
                {
                    error = int.TryParse(Console.ReadLine(), out chosen);
                    if (!error || chosen >= i || chosen < 1) Console.WriteLine("Неверное сохранение, введите номер повторно");
                }
                i = 1;
                foreach (DirectoryInfo item in dir.GetDirectories())
                {
                    if (chosen == i)
                    {
                        CurrentMap = dir + "\\" + item + "\\MapData.txt";
                        CurrentSwitch = dir + "\\" + item + "\\SwitchData.txt";
                        money = Convert.ToInt32(File.ReadAllLines(dir + "/" + item + "/OtherData.txt")[0]);
                        level = Convert.ToInt32(File.ReadAllLines(dir + "/" + item + "/OtherData.txt")[1]);
                        current_coins = Convert.ToInt32(File.ReadAllLines(dir + "/" + item + "/OtherData.txt")[2]);
                        coin_count = Convert.ToInt32(File.ReadAllLines(dir + "/" + item + "/OtherData.txt")[3]);
                        loaded = true;
                        pause = false;
                    }
                    i++;
                    status = true;
                }
            }
        }

        static void Save()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Выберете название для сейва: ");
            var SaveName = Console.ReadLine();
            string[] Lines = new string[n];
            for (int i = plus1; i < n + plus1; i++)
                for (int j = plus2; j < m + plus2; j++)
                    Lines[i - plus1] = Lines[i - plus1] + frame[i, j];
            Directory.CreateDirectory("saves/" + SaveName + "/");
            File.WriteAllLines("saves/" + SaveName + "/" + "MapData.txt", Lines);
            Lines = new string[n];
            for (int i = plus1; i < n + plus1; i++)
                for (int j = plus2; j < m + plus2; j++)
                    Lines[i - plus1] = Lines[i - plus1] + frame2[i, j];
            Directory.CreateDirectory("saves/" + SaveName + "/");
            File.WriteAllLines("saves/" + SaveName + "/" + "SwitchData.txt", Lines);

            File.WriteAllText("saves/" + SaveName + "/" + "OtherData.txt", Convert.ToString($"{money}\n{level}\n{current_coins}\n{coin_count}"));
            MainMenu();
        }

        static void CheckPos()
        {
            for (int i = plus1; i < n + plus1; i++)
                for (int j = plus2; j < m + plus2; j++)
                    if (frame[i, j] == player)
                    {
                        posy = i;
                        posx = j;
                    }
        }

        static void fix()
        {
            for (int i = plus1; i < n + plus1; i++)
                for (int j = plus2; j < n + plus2; j++)
                    if (frame[i, j] == '0' && frame[i + 1, j] == '0' && frame[i - 1, j] == '0'
                        && frame[i, j - 1] == '0' && frame[i, j + 1] == '0') frame[i, j] = '╬';

            for (int i = 1; i < n + plus1; i++)
                for (int j = 1; j < m + plus2; j++)
                    if (frame[i, j] == '0' && (frame[i - 1, j] == '0' || frame[i - 1, j] != ' ') && (frame[i + 1, j] == '0'
                        || frame[i + 1, j] != ' ') && (frame[i, j + 1] == '0' || frame[i, j + 1] != ' ')) frame[i, j] = '╠';

            for (int i = 1; i < n + plus1; i++)
                for (int j = 1; j < m + plus2; j++)
                    if (frame[i, j] == '0' && (frame[i - 1, j] == '0' || frame[i - 1, j] != ' ') && (frame[i + 1, j] == '0'
                        || frame[i + 1, j] != ' ') && (frame[i, j - 1] == '0' || frame[i, j - 1] != ' ')) frame[i, j] = '╣';

            for (int i = 1; i < n + plus1; i++)
                for (int j = 1; j < m + plus2; j++)
                    if (frame[i, j] == '0' && (frame[i + 1, j] == '0' || frame[i + 1, j] != ' ') && (frame[i, j - 1] == '0'
                        || frame[i, j - 1] != ' ') && (frame[i, j + 1] == '0' || frame[i, j + 1] != ' ')) frame[i, j] = '╦';

            for (int i = 1; i < n + plus1; i++)
                for (int j = 1; j < m + plus2; j++)
                    if (frame[i, j] == '0' && (frame[i - 1, j] == '0' || frame[i - 1, j] != ' ') && (frame[i, j - 1] == '0'
                        || frame[i, j - 1] != ' ') && (frame[i, j + 1] == '0' || frame[i, j + 1] != ' ')) frame[i, j] = '╩';

            for (int i = 1; i < n + plus1; i++)
                for (int j = 1; j < m + plus2; j++)
                    if (frame[i, j] == '0' && (frame[i - 1, j] == '0' || frame[i - 1, j] != ' ')
                        && (frame[i, j - 1] == '0' || frame[i, j - 1] != ' ')) frame[i, j] = '╝';

            for (int i = 1; i < n + plus1; i++)
                for (int j = 1; j < m + plus2; j++)
                    if (frame[i, j] == '0' && (frame[i - 1, j] == '0' || frame[i - 1, j] != ' ')
                        && (frame[i, j + 1] == '0' || frame[i, j + 1] != ' ')) frame[i, j] = '╚';

            for (int i = 1; i < n + plus1; i++)
                for (int j = 1; j < m + plus2; j++)
                    if (frame[i, j] == '0' && (frame[i + 1, j] == '0' || frame[i + 1, j] != ' ')
                        && (frame[i, j + 1] == '0' || frame[i, j + 1] != ' ')) frame[i, j] = '╔';

            for (int i = 1; i < n + plus1; i++)
                for (int j = 1; j < m + plus2; j++)
                    if (frame[i, j] == '0' && (frame[i + 1, j] == '0' || frame[i + 1, j] != ' ')
                        && (frame[i, j - 1] == '0' || frame[i, j - 1] != ' ')) frame[i, j] = '╗';

            for (int i = 1; i < n + plus1; i++)
                for (int j = 1; j < m + plus2; j++)
                    if (frame[i, j] == '0' && (frame[i, j + 1] == '0' || frame[i, j - 1] == '0'
                        || frame[i, j + 1] != ' ' || frame[i, j - 1] != ' ')) frame[i, j] = '═';

            for (int i = 1; i < n + plus1; i++)
                for (int j = 1; j < m + plus2; j++)
                    if (frame[i, j] == '0' && (frame[i + 1, j] == '0' || frame[i - 1, j] == '0'
                        || frame[i + 1, j] != ' ' || frame[i - 1, j] != ' ')) frame[i, j] = '║';

            for (int i = 1; i < n + plus1; i++)
                for (int j = 1; j < m + plus2; j++)
                    if (frame[i, j] == '*') frame[i, j] = '▒';
        }

        static void Generate()
        {
            string[] file = File.ReadAllLines(CurrentMap);
            n = file.Length;
            m = 0;
            foreach (string line in file)
                if (line.Length >= m) m = line.Length;
            frame = new char[n + plus1 * 2, m + plus2 * 2];
            frame2 = new int[n + plus1 * 2, m + plus2 * 2];
            int i = plus1, j = plus2;
            foreach (string line in file)
            {
                foreach (char element in line)
                {
                    frame[i, j] = element;
                    j++;
                }
                i++;
                j = plus2;
            }
            i = plus1;
            j = plus2;
            if (loaded)
            {
                string[] file2 = File.ReadAllLines(CurrentSwitch);
                foreach (string line in file2)
                {
                    foreach (char element in line)
                    {
                        if (element == '1')
                            frame2[i, j] = 1;
                        j++;
                    }
                    i++;
                    j = plus2;
                }
            }
            else if (!loaded)
            {
                coin_count = 0;
                for (i = plus1; i < n + plus1; i++)
                    for (j = plus2; j < m + plus2; j++)
                        if (frame[i, j] == '$') coin_count++;
            }

        }

        static void Show()
        {
            int y = (29 - n) / 2, x = (120 - m) / 2, count = 0;
            Console.SetCursorPosition(x, y);
            for (int i = plus1; i < n + plus1; i++)
            {
                Console.SetCursorPosition(x, y + count);
                for (int j = plus2; j < m + plus2; j++)
                {
                    if (frame2[i, j] == 1)
                    {
                        if (frame[i, j] == '&') Console.ForegroundColor = ConsoleColor.Green;
                        else if (frame[i, j] == '$') Console.ForegroundColor = ConsoleColor.Yellow;
                        else if (frame[i, j] == '▒') Console.ForegroundColor = ConsoleColor.Red;
                        else if (frame[i, j] == '?') Console.ForegroundColor = ConsoleColor.Gray;
                        else if (frame[i, j] == '>') Console.ForegroundColor = ConsoleColor.Magenta;
                        else Console.ForegroundColor = ConsoleColor.Cyan;
                    }
                    else Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(frame[i, j]);
                }
                count++;
            }
            Console.SetCursorPosition(0, 27);
            Console.BackgroundColor = ConsoleColor.White;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 120; j++)
                {
                    Console.Write(" ");
                }
            }


            Console.SetCursorPosition(1, 28);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(" Ваши очки: ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(money + " ");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(" Текущий уровень: ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(level + " ");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(" Количество собранных монет: ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(current_coins + " из " + coin_count);
            Console.SetCursorPosition(1, 28);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, 0);
        }

        static void play(int a, int b)
        {
            if (frame[a, b] == ' ')
            {
                frame[a, b] = player;
                frame[posy, posx] = ' ';
                Show();
            }
            else if (frame[a, b] == '▒')
            {
                status = false;
                lose = true;
                money = 0;
                Show();
            }
            else if (frame[a, b] == '$')
            {
                Console.Beep();
                frame[a, b] = player;
                frame[posy, posx] = ' ';
                money += 25;
                current_coins++;
                checkescape();
                Show();
            }
            else if (frame[a, b] == '>' && level == level_count)
            {
                status = false;
                win = true;
            }
            else if (frame[a, b] == '>') status = false;
        }

        static void Move()
        {
            CheckPos();
            ConsoleKeyInfo sender = Console.ReadKey();
            switch (sender.Key)
            {
                case ConsoleKey.UpArrow:
                    play(posy - 1, posx);
                    break;
                case ConsoleKey.DownArrow:
                    play(posy + 1, posx);
                    break;
                case ConsoleKey.LeftArrow:
                    play(posy, posx - 1);
                    break;
                case ConsoleKey.RightArrow:
                    play(posy, posx + 1);
                    break;
                case ConsoleKey.Escape:
                    pause = true;
                    status = false;
                    break;
                case ConsoleKey.Spacebar:
                    if (level == level_count) 
                    { 
                        win = true;
                        status = false;
                    }

                    else status = false;
                    break;
            }
            if (!pause)
            {
                Check();
            }


        }

        static void checkescape()
        {
            if (current_coins >= coin_count)
                for (int i = plus1; i < n + plus1; i++)
                    for (int j = plus2; j < m + plus2; j++)
                        if (frame[i, j] == '?') frame[i, j] = '>';

        }


        static void Check()
        {
            CheckPos();
            for (int i = posy - plus1; i < posy + plus1; i++)
                for (int j = posx - plus2; j < posx + plus2; j++)
                    frame2[i, j] = 1;
        }

        static void Goplay()
        {
            status = true;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            if (loaded && !pause)
            {
                Generate();
                win = false;
                lose = false;
            }
            else if (!loaded && !pause)
            {
                CurrentMap = "levels/" + level + ".txt";
                Generate();
                current_coins = 0;
                posy = 0;
                posx = 0;
                win = false;
                lose = false;
            }
            else
            {
                pause = false;
                loaded = false;
            }
            fix();
            Check();
            Show();
            while (status)
                Move();
            if (pause) Console.Clear();
            loaded = false;
            if (win)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Clear();
                Console.WriteLine("Ты прошёл мою игру, ПОЗДРАВЛЯЮ, правда, я не знаю чего тебе это стоило и чем ты так разочаровался в своей жизни, что начал играть в низкоуровневые проекты, сделанные всякими дэбилами, но, всё-же, поблагодарю тебя за трату своего времени)");
                Console.ReadKey();
                level = 1;
                MainMenu();
            }
            else if (lose)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Clear();
                Console.WriteLine("Не расстраивайся, в следующий раз повезёт, наверно...");
                Console.ReadKey();
                level = 1;
                MainMenu();
            }
            else if (!lose && !pause)
            {
                level++;
                status = true;
            }
            else MainMenu();
        }
        static void MainMenu()
        {
            Console.Clear();
            for (int i = 0; i < 5; i++)
                enabled[i] = false;
            Console.CursorVisible = false;
            Background();
            enabled[current] = true;
            ShowBlocks();
            Console.SetCursorPosition(0, 0);
        }

        static void calc_levels()
        {
            DirectoryInfo dir = new DirectoryInfo("levels/");
            foreach (FileInfo item in dir.GetFiles())
            {
                level_count += 1;
            }

        }
        static void Main(string[] args)
        {
            MainMenu();
            calc_levels();
            while (game)
            {
                Switch();
                while (status) Goplay();
            }

        }
    }
}
