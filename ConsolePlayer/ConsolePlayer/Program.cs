using INIFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsolePlayer
{
    class Program
    {
        public static Cls STmenu = new Cls();
        public static ProcessKill ProcKiller = new ProcessKill();
        public static Options Opt = new Options();

        public static void TextColor()
        {
            try
            {             
                string inifile = Environment.ExpandEnvironmentVariables(@"C:\Users\%USERNAME%\AppData\Roaming\ConsolePlayer\user.ini");
                INIFile inif = new INIFile(inifile);
                int colorNumber = int.Parse(inif.Read("Settings", "Color"));
                Opt.TexColors(colorNumber);
            }
            catch (Exception ex)
            {
                ErrorLogs Log = new ErrorLogs();
                string exMess = ex.Message + " " + "[rutin:  TextColor()]";
                Log.Errors(exMess);
            }
        }

        static void Main(string[] args)
        {
            TextColor();
            Console.Title = "ConsolePlayer";
            STmenu.StartMenu();

            ProcKiller.PidStart();
            
            int Exit = 0;
            do
            {
                switch (Console.ReadLine())
                {
                    case "a":
                        PlayAlbum St = new PlayAlbum();
                        Console.WriteLine("Start Track lista");
                        St.Start();
                        break;
                    case "b":
                        PlayMusic ST = new PlayMusic();
                        Console.WriteLine("Album megnyitásához add meg a nevét (pl zenék)");
                        ST.Start();
                        break;
                    case "c":
                        Console.WriteLine("Zenék hozzáadása ");
                        ZeneKezelo zene = new ZeneKezelo();
                        zene.Start();
                        break;
                    case "d":
                        Console.WriteLine("Stop! ");
                        int pid = ProcKiller.PIdChack();
                        ProcKiller.Kill(pid);
                        break;
                    case "e":
                        Console.WriteLine("Lejátszó beállítások. ");
                        Opt.Start();
                        break;
                    case "i":
                        Info inf = new Info();
                        inf.information();
                        break;
                    case "cls":
                        STmenu.Start();
                        break;
                    case "x":
                        Console.WriteLine("kilépés");
                        int pid0 = ProcKiller.PIdChack();
                        ProcKiller.Kill(pid0);
                        Exit++;
                        break;
                }
            } while (Exit < 1);
        }
    }
}
