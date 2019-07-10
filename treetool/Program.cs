using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace treetool
{

    class TreeTool
    {
        int depth;

        public TreeTool (string path, int depth, bool showSize, bool readableSize)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            this.depth = depth;
                 
            OpenFolder(dir, 0, showSize, readableSize);
        }

        private void OpenFolder(DirectoryInfo curDir, int depthCurrent, bool showSize, bool readableSize)
        {
            if (depth != -1 && depthCurrent > depth) {
                return;
            } else {
                foreach (var file in curDir.EnumerateFiles()) {
                    try {
                        Console.WriteLine($"{Tabs(depthCurrent - 1)}|───{file.Name} {Size(file, showSize, readableSize)}");
                    } catch (UnauthorizedAccessException e) {
                        Console.WriteLine($"{Tabs(depthCurrent - 1)}|───Недоступный файл");
                    } catch (Exception e) {
                    }
                }

                foreach (var dir in curDir.EnumerateDirectories()) {
                    try {
                        Console.WriteLine($"{Tabs(depthCurrent - 1)}└───{dir.Name}");
                        OpenFolder(dir, depthCurrent + 1, showSize, readableSize);
                    } catch (UnauthorizedAccessException e) {
                        Console.WriteLine($"{Tabs(depthCurrent - 1)}|───Недоступная папка");
                    } catch (Exception e) {
                    }
                }
            }
        }

        private string Size(FileInfo file, bool showSize, bool readableSize)
        {
            if (!showSize) {
                return "";
            } else if (!readableSize || (file.Length < (1 << 10))) {
                return $"({file.Length} B)";
            } else {
                if (file.Length >= (1 << 30)) {
                    return $"({((file.Length >> 20) / (1.0 * (1 << 10))).ToString("0.00")} GB)";
                } else if (file.Length >= (1 << 20)) {
                    return $"({((file.Length >> 10) / (1.0 * (1 << 10))).ToString("0.00")} MB)";
                } else if (file.Length >= (1 << 10)) {
                    return $"({(file.Length / (1.0 * (1 << 10))).ToString("0.00")} KB)";
                } else {
                    if (file.Length < 1) return $"(empty)";
                    return $"({file.Length} B)";
                }
            }
        }

        static string Tabs(int n)
        {
            return new String(' ', n*4);
        }
    }

    class Program
    {
        static bool Contains(string str, string substr)
        {
            return (str.IndexOf(substr) != -1);
        }
        static bool Contains(string str, string substr1, string substr2)
        {
            return (str.IndexOf(substr1) != -1) || (str.IndexOf(substr2) != -1);
        }

        static void WriteInfo()
        {
            Console.Write(
                "Info:{0}Команды и параметры вводятся в одну строку через пробел!{0}exit - для выхода{0}Дополнительные аргументы при запуске приложения, которые могут быть использованы все вместе:{0}задают глубину вложенности(-d или--depth) далее через пробел глубина {0}(нумерация от 0, то есть 0 не раскрывает папки в текущем каталоге){0}показывают размер объектов(-s или--size),{0} в том числе удобном для восприятия виде(-h или--human - readable){0}справка по использованию(--help или -?)\n", "\n    ");
        }

        static int Depth(string str)
        {
            var numstr = str.Substring(str.IndexOf(' ', str.IndexOf("-d")) + 1);
            if (Contains(numstr, " ")) 
                numstr = numstr.Substring(0, numstr.IndexOf(' '));
            else
                numstr = numstr.Substring(0, numstr.Length);
            if (numstr.Length < 1)
                return 0;
            int num = 0;
            int.TryParse(numstr, out num);
            return num;
        }

        static void Main(string[] args)
        {
            string input = Console.ReadLine();
            while (!Contains(input, "exit")) {
                if (Contains(input, "-?", "--help")) {
                    WriteInfo();
                } else {
                    string path = @".";
                    int depth = Contains(input, "-d", "--depth") ? Depth(input): -1;
                    bool showSize = Contains(input, "-s", "--size");
                    bool readableSize = Contains(input, "-h", "--human - readable");
                    TreeTool tt = new TreeTool(path, depth, showSize, readableSize);
                }
                input = Console.ReadLine();
            }
        }
    }
}
