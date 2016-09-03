using System;
using System.Timers;
using System.Linq;
using System.IO;

namespace Discord
{
    class Announcer
    {
        private Timer msgTimer;
        private Random rand = new Random();
        int timerInterval = int.Parse(Settings.AnnouncerInterval);

        public Announcer()
        {
            timerInterval = rand.Next(timerInterval - 300000, timerInterval + 300000);
            msgTimer = new Timer(timerInterval);
            msgTimer.Elapsed += new ElapsedEventHandler(msgAnnouncer);
            msgTimer.Enabled = true;
            Console.WriteLine($"Next announcement to {Settings.DefaultChan} in {((timerInterval / 1000) / 60)}minutes");
        }

        private async void msgAnnouncer(object s, ElapsedEventArgs e)
        {
            timerInterval = rand.Next(timerInterval - 300000, timerInterval + 300000);
            msgTimer.Interval = timerInterval;

            string[] _lines = File.ReadAllLines(Environment.CurrentDirectory + @"\Announcements.txt");
            int _randLineNum = rand.Next(_lines.Length);
            string _randLine = _lines[rand.Next(_randLineNum)];

            if (_randLine != "")
                await Connect._client.Servers.FirstOrDefault(x => x.Id == Settings.ServerID)?.TextChannels.FirstOrDefault(x => x.Name == Settings.DefaultChan).SendMessage("`Random Poké Fact #" + _randLineNum + "`\n" + _randLine);
            else return;
        }
    }
}
