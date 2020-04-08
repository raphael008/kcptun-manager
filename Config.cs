using KcptunManager.Annotations;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KcptunManager
{
    public class Config : INotifyPropertyChanged, ICloneable
    {
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public string LocalAddress { get; set; }

        public string RemoteAddress { get; set; }

        public string Key { get; set; } = "it's a secret";

        public string Crypt { get; set; } = "aes";

        public string Mode { get; set; } = "fast";

        public int Connections { get; set; } = 1;

        public int AutoExpire { get; set; } = 0;

        public int Scavengettl { get; set; } = 600;

        public int Mtu { get; set; } = 1350;

        public int Send { get; set; } = 128;

        public int Recv { get; set; } = 512;

        public int DataShard { get; set; } = 10;

        public int ParityShard { get; set; } = 3;

        public int Dscp { get; set; } = 0;

        public int SockBuffer { get; set; } = 4194304;

        public int SmuxVersion { get; set; } = 1;

        public int SmuxBuffer { get; set; } = 4194304;

        public int StreamBuffer { get; set; } = 2097152;

        public int KeepAlive { get; set; } = 10;

        public bool NoCompression { get; set; } = true;

        public bool Quiet { get; set; } = true;

        public bool AutoStart { get; set; } = false;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
