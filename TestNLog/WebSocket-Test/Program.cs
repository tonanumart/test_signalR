using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace WebSocket_Test
{
    class Program
    {
        private static string ws = "wss://echo.websocket.org";
        static void Main(string[] args)
        {
            using (var ws = new WebSocket(Program.ws))
            {
                ws.OnMessage += (sender, e) =>
                    Console.WriteLine("Laputa says: " + e.Data);

                ws.Connect();
                ws.Send("BALUS");
                Console.ReadKey(true);
            }
        }
    }
}
