using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsolePlayer
{
    class Player   //régi Windows Media Player COM vezérlőt (WMPLib.WindowsMediaPlayer) használom
    {
        public void Play(string path, int id, string title, string length)
        {
            ErrorLogs Log = new ErrorLogs();
            try
            {           
                WMPLib.WindowsMediaPlayer player = new WMPLib.WindowsMediaPlayer();
                int Sec1 = 0;
                int Min1 = 0;
                double Counter = 0;
                double Sec;
                ManualResetEvent playbackStarted = new ManualResetEvent(false);  //  esemény akkor állítódik be ha elindult a lejátszás

                player.PlayStateChange += (int NewState) =>          // Eseménykezelő beállítása: ha valóban elindult a lejátszás, beállítjuk az eseményt
                {
                    if (NewState == (int)WMPLib.WMPPlayState.wmppsPlaying)
                    {
                        playbackStarted.Set();   // Jelzi, hogy a zene elindult
                    }
                };

                player.URL = path;
                player.controls.play();

                if (!playbackStarted.WaitOne(5000))
                {
                    throw new Exception("A lejátszás nem indult el időben.");
                }

                double duration = player.currentMedia.duration;  //duration alapján számolt idő nem vágódik le!
                Stopwatch sw = new Stopwatch();
                sw.Start();
                Sec = player.currentMedia.duration;
                Console.Clear();

                do
                {
                    string status = id + ": " + title + " " + length + "  [" + Min1 + ":" + Sec1.ToString("D2") + "]";
                    Console.SetCursorPosition(0, 0);                       // Sor kiírása ugyanarra a helyre, régi tartalom felülírásával
                    Console.Write(new string(' ', Console.WindowWidth));   // villogásmentes kiírás

                    Console.SetCursorPosition(0, 0);
                    Console.Write(status);

                    Thread.Sleep(1000);
                    if (Sec1 < 59)
                    {
                        Sec1++;
                    }
                    else
                    {
                        Min1++;
                        Sec1 = 0;
                    }
                    Counter++;
                } while (Counter < Sec);
                Counter = 0;
                Sec1 = 0;
                Min1 = 0;
            }
            catch (Exception ex)
            {
                string exMess = ex.Message + " " + "[rutin:  Play()]";
                Console.WriteLine("Ismeretlen hiba");
                Log.Errors(exMess);
            }
        }

        private string TimerMod(string hossz)
        {
            string[] reszek = hossz.Split(':');       //szétbontom : mentén
            int sec = int.Parse(reszek[1]);         // első szám tag
            int min = int.Parse(reszek[2]);   // második szám tag
            string time = string.Format("{0},{1:00}", sec, min);
            return time;
        }
    }
}
