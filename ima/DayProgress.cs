/**
 * Copyright (C) 2019  Dwscdv3 <dwscdv3@hotmail.com>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.ComponentModel;
using System.Timers;
using Dwscdv3.ima.Helpers;
using Dwscdv3.ima.Properties;

namespace Dwscdv3.ima
{
    internal class DayProgress : INotifyPropertyChanged
    {
        private static DayProgress _instance = null;
        public static DayProgress Instance => _instance is null ? _instance = new DayProgress() : _instance;

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action DayStarted;
        public event Action DayEnded;

        public DateTime StartTime =>
            DateTime.Now.TimeOfDay >= Settings.Default.DayStart
            ? DateTime.Today + Settings.Default.DayStart
            : DateTime.Today.AddDays(-1) + Settings.Default.DayStart;

        public DateTime EndTime =>
            Settings.Default.DayEnd > Settings.Default.DayStart
            ? StartTime.Date + Settings.Default.DayEnd
            : StartTime.Date.AddDays(1) + Settings.Default.DayEnd;

        public TimeSpan Remaining =>
            (EndTime - DateTime.Now) >= TimeSpan.Zero ? EndTime - DateTime.Now : TimeSpan.Zero;

        public double ElapsedNormalized =>
            ((double)(DateTime.Now - StartTime).Ticks / (EndTime - StartTime).Ticks).Clamp();
        public double RemainingNormalized => 1 - ElapsedNormalized;

        private Timer timer = new Timer(1000);
        private DateTime nextStartTime;
        private DateTime nextEndTime;

        private DayProgress()
        {
            Settings.Default.PropertyChanged += OnSettingsChanged;

            SetNextEventTime();

            timer.Elapsed += (sender, e) =>
            {
                if (DateTime.Now >= nextStartTime)
                {
                    nextStartTime = nextStartTime.AddDays(1);
                    DayStarted?.Invoke();
                }
                if (DateTime.Now >= nextEndTime)
                {
                    nextEndTime = nextEndTime.AddDays(1);
                    DayEnded?.Invoke();
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Remaining"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ElapsedNormalized"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RemainingNormalized"));
            };
            timer.Start();
        }

        private void OnSettingsChanged(object sender, PropertyChangedEventArgs e) => SetNextEventTime();

        private void SetNextEventTime()
        {
            nextStartTime = StartTime.AddDays(1);
            nextEndTime = DateTime.Now >= EndTime ? EndTime.AddDays(1) : EndTime;
        }
    }
}
