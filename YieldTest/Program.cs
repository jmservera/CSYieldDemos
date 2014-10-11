using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YieldTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int x = 40, y = 10;
            Task.Run(() =>
            {
                YieldDemo demo = new YieldDemo();
                var strings = demo.CycleStrings(new string[] { "|", "/", "-", "\\" });

                foreach (var str in strings)
                {
                    Console.Clear();
                    Console.SetCursorPosition(x, y);
                    Console.Write(str);
                    System.Threading.Thread.Sleep(90);
                }
            });

            while (true)
            {
                var key = Console.ReadKey();
                bool exit = false;
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if(y>0) y--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (y < 24) y++;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (x >0) x--;
                        break;
                    case ConsoleKey.RightArrow:
                        if (x < Console.BufferWidth-1) x++;
                        break;
                    case ConsoleKey.Enter:
                        exit=true;
                        break;
                }
                if (exit) break;
            }
        }
    }
}
