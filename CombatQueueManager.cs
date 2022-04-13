using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BattleSimulator
{
    public class CombatQueueManager
    {
        private Thread cqthread;
        private SemaphoreSlim semaphore;
        private CombatQueue cq;
        private volatile TextBlock battleComments;
        private volatile TextBlock cqDisplay;

        private bool running = false;

        public CombatQueue CombatQueue
        {
            get { return cq; }
        }

        public CombatQueueManager(TextBlock batCom, TextBlock cqDisp)
        {
            cq = new CombatQueue();
            semaphore = new SemaphoreSlim(10);
            cqthread = new Thread(ProcessQueue);
            battleComments = batCom;
            cqDisplay = cqDisp;
        }
        public void StartQueueWatch()
        {
            running = true;
            cqthread.Start();
        }

        public void StopQueueWatch()
        {
            running = false;
        }

        public async Task UpdateCombatQueueList()
        {
            await Task.Delay(new TimeSpan(0, 0, 0, 0, 50));
            if (cq.Count > 0)
            {
                cqDisplay.Dispatcher.Invoke(() =>
                {
                    cqDisplay.Text = string.Empty;
                    foreach (IParticipant f in cq)
                    {
                        cqDisplay.Text += $"{f.Name} ";
                    }
                });
            }
            else
            {
                cqDisplay.Dispatcher.Invoke(() =>
                {
                    cqDisplay.Text = string.Empty;
                });
            }


        }

        public async void ProcessQueue()
        {
            while (running)
            {
                Thread.Sleep(2000);
                if (cq.Count > 0)
                {
                    var character = SafeDequeue();
                    // Need to set this up so it uses the correct choice.
                    battleComments.Dispatcher.Invoke(() =>
                    {
                        battleComments.Text = character.Result.Attack();
                    });
                    await UpdateCombatQueueList();
                }
            }
        }

        public async Task SafeEnqueue(IParticipant part)
        {
            await semaphore.WaitAsync();
            lock (cq)
            {
                cq.Enqueue(part);
            }

            semaphore.Release();
        }

        public async Task<IParticipant> SafeDequeue()
        {
            await semaphore.WaitAsync();
            lock (cq)
            {
                semaphore.Release();
                return cq.Dequeue();
            }

            

        }
    }
}
