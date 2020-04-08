using System.Diagnostics;
using System.IO;
using System.Windows;

namespace KcptunManager
{
    public class Kcptun
    {
        private static readonly string _executablePath = "client_windows_amd64.exe";

        public static Process GetInstance(Config config)
        {
            if (!File.Exists(_executablePath))
            {
                MessageBox.Show("请确保kcptun客户端文件位于当前程序同一目录下", string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            var process = new Process
            {
                EnableRaisingEvents = true,
                StartInfo =
                {
                    FileName = _executablePath,
                    Arguments = $@"-l ""{config.LocalAddress}"" "+
                                $@"-r ""{config.RemoteAddress}"" "+
                                $@"--key ""{config.Key}"" "+
                                $@"--crypt ""{config.Crypt}"" "+
                                $@"--mode ""{config.Mode}"" "+
                                $@"--conn ""{config.Connections}"" "+
                                $@"--autoexpire ""{config.AutoExpire}"" "+
                                $@"--scavengettl ""{config.Scavengettl}"" "+
                                $@"--mtu ""{config.Mtu}"" "+
                                $@"--sndwnd ""{config.Send}"" "+
                                $@"--rcvwnd ""{config.Recv}"" "+
                                $@"--datashard ""{config.DataShard}"" "+
                                $@"--parityshard ""{config.ParityShard}"" "+
                                $@"--dscp ""{config.Dscp}"" "+
                                $@"--sockbuf ""{config.SockBuffer}"" "+
                                $@"--smuxver ""{config.SmuxVersion}"" "+
                                $@"--smuxbuf ""{config.SmuxBuffer}"" "+
                                $@"--streambuf ""{config.StreamBuffer}"" "+
                                $@"--keepalive ""{config.KeepAlive}"" "+
                                $@"--nocomp "+
                                $@"--quiet",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };

            return process;
        }
    }
}