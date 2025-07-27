using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePlayer
{
   public class Cls
    {
        public void StartMenu()
        {
            Console.WriteLine(@"  ___                  _       ___ _            V.1.0.0");
            Console.WriteLine(@" / __|___ _ _  ___ ___| |___  | _ \ |__ _ _  _ ___ _ _ ");
            Console.WriteLine(@"| (__/ _ \ ' \(_-</ _ \ / -_) |  _/ / _` | || / -_) '_|");
            Console.WriteLine(@" \___\___/_||_/__/\___/_\___| |_| |_\__,_|\_, \___|_|  ");
            Console.WriteLine(@"                                         |__/           ");           
            Console.WriteLine("_____________________________________________________");
            Console.WriteLine("\ta - Start Track lista");
            Console.WriteLine("\tb - Összes zene");
            Console.WriteLine("\tc - Zene kezelő");
            Console.WriteLine("\td - Stop");
            Console.WriteLine("\te - Opciok");
            Console.WriteLine("\ti - Info");
            Console.WriteLine("\tx - Kilépés");
            Console.WriteLine("Válasz Opciót /? ");
            Console.WriteLine("_____________________________________________________");
        }
        public void Start()
        {
            Console.Clear();
            StartMenu();
        }
    }
}
