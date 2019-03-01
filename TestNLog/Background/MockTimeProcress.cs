using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace TestNLog.Background
{
     
    public class MockTimeProcress
    {

        public static int TimeConsumingOperation(BackgroundWorker bw, int sleepPeriod)
        {
            int yourResult = 0;
            Random rand = new Random();

            while (!bw.CancellationPending)
            {
                bool completeTask = false;

                switch (rand.Next(3))
                {
                    // Raise an exception.
                    case 0:
                        {
                            throw new Exception("random = 0 case exception found.");
                        }

                    case 1:
                        {
                            Console.WriteLine("you has a bad luck waiting background {0} ms.", sleepPeriod);
                            //bw.ReportProgress(55, new MyUserState()
                            //{
                            //    Value = 55,
                            //    Content = string.Format("you has a bad luck waiting background {0} ms.", sleepPeriod)
                            //});
                            Thread.Sleep(sleepPeriod);
                            break;
                        }

                    case 2:
                        {
                            Console.WriteLine("Lucky.");
                            yourResult = rand.Next(15) + 1;
                            completeTask = true;
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
                if (completeTask)
                {
                    break;
                }
            }
            return yourResult;
        }
    }
}
