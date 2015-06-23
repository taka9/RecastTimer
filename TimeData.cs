using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace RecastTimer
{
    public class TimeData : INotifyPropertyChanged
    {
        private DispatcherTimer Timer;
        private int StartTime;
        private int _time;
        private int Time
        {
            get
            {
                return this._time;
            }
            set
            {
                this._time = value;
                OnPropertyChanged("TimeStr");
                OnPropertyChanged("FontColor");
            }
        }

        public string TimeStr
        {
            get { return this.GetTimeStr();}
        }

        public SolidColorBrush FontColor
        {
            get
            {
                if (this.Time > 10)
                {
                    return new SolidColorBrush(Colors.Gray);
                }
                else
                {
                    return new SolidColorBrush(Colors.Red);
                }
            
            }
        }

        public TimeData(int time)
        {
            this.Time = time;
            this.StartTime = time;
            this.Timer = new DispatcherTimer();
            this.Timer.Interval = TimeSpan.FromSeconds(1);
            this.Timer.Tick += new EventHandler(DispatcherTimer_Tick);
        }

        public void TimerStart()
        {
            this.Time = this.StartTime;
            this.Timer.Start();
        }

        public void SetEvent(EventHandler handler)
        {
            this.Timer.Tick += new EventHandler(handler);
        }

        public string GetTimeStr()
        {
            if (this.Time < 100)
            {
                return this.Time.ToString("D2");
            }
            else
            {
                return this.Time.ToString();
            }
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            this.Time--;
            if (this.Time <= 0)
            {
                this.Timer.Stop();
            }
        }

        public void Reset()
        {
            this.Timer.Stop();
            this.Time = this.StartTime;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
