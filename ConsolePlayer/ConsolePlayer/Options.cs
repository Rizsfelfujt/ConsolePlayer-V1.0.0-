using INIFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePlayer
{
    class Options
    {
        public void TexColors(int index, bool message = false)
        {
            string[] szinek = { "Black", "Blue", "Cyan", "DarkBlue", "DarkCyan", "DarkGray","DarkGreen", "DarkMagenta", "DarkRed", "DarkYellow", "Gray", "Green", "Magenta", "Red", "White", "Yellow" };
        
            if (index < 1 || index >= szinek.Length)
            {
                Console.WriteLine("Érvénytelen index!");
                return;
            }
            ConsoleColor szin;
            if (Enum.TryParse(szinek[index], out szin))
            {
                Console.ForegroundColor = szin;        
                if (message == true)
                {
                    Console.WriteLine("Beálítások érvényesítéséhez lépj ki a menüből! [2]");
                }
            }
        }


        public void Start()
        {
            string inifile = Environment.ExpandEnvironmentVariables(@"C:\Users\%USERNAME%\AppData\Roaming\ConsolePlayer\user.ini");
            Console.WriteLine("\t1 - Batű színe");
            Console.WriteLine("\t2 - Exit");
            int Exit = 0;
            do
            {
                try
                {
                    switch (Console.ReadLine())
                    {
                        case "1":
                            Console.WriteLine("Színkód 1-15  közt adható meg.");
                            Console.Write("set: ");
                            string index0 = Console.ReadLine();
                            bool message = true;
                            INIFile inif = new INIFile(inifile);
                            inif.Write("Settings", "Color", index0);  //irás
                            int index = int.Parse(index0);
                            TexColors(index, message);
                            break;
                        case "2":
                            Cls STmenu = new Cls();
                            STmenu.Start();
                            Exit++;
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Hibás bemeneti paraméter!");
                }
            } while (Exit < 1);
        }
    }
}
