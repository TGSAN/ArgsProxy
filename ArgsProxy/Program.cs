using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArgsProxy
{
    class Program
    {
        //static Thread threadInput;
        //static Thread threadOutput;
        //static bool canceled = false;
        static void Main(string[] args)
        {

            ////Console.WriteLine("Hello World!");
            ////Console.WriteLine(GetUnixTimeStamp());
            StringBuilder args_str = new StringBuilder();
            if (args.Length > 0)
            {
                foreach (string x in args)
                {
                    args_str.Append($"{x} ");
                }
                args_str = new StringBuilder().Append(args_str.ToString().Trim());
            }
            //Console.WriteLine("Hello World!");
            string self_path = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            string self_name = Path.GetFileNameWithoutExtension(self_path);
            string self_ext = Path.GetExtension(self_path);
            //Console.WriteLine($"{self_name}args:{args_str}");
            string log_path = @"ArgsProxyLogs\" + self_name + self_ext + @"\";
            //Console.WriteLine(log_path);
            if (!Directory.Exists(log_path))
                Directory.CreateDirectory(log_path);
            File.WriteAllText(log_path + GetUnixTimeStampMilliseconds() + ".txt", $"{self_name + self_ext} args:{args_str}");
            //Proc("ping", "www.baidu.com");
            //Proc("cmd");
            Proc(self_name + "Base" + self_ext, args_str.ToString());
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        static long GetUnixTimeStampMilliseconds()
        {
            DateTime startTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Utc, TimeZoneInfo.Local); // 当前时区
            long timeStamp = (long)(DateTime.Now - startTime).TotalMilliseconds;
            return timeStamp;
        }

        static void Proc(string path, string args = "")
        {
            //canceled = false;
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = path;
            proc.StartInfo.Arguments = args;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            //proc.OutputDataReceived += (o, e) => { Console.WriteLine(e.Data); };
            proc.ErrorDataReceived += (o, e) => { Console.Error.WriteLine(e.Data); };

            //proc.StandardInput.AutoFlush = true;
            //new Thread(() =>
            //{
            //    Console.OpenStandardInput().CopyTo(proc.StandardInput.BaseStream);
            //}).Start();

            //new Thread(() => {
            //    while (true)
            //    {
            //        proc.StandardInput.WriteLine(Console.ReadLine());
            //    }
            //}).Start();
            //threadInput = new Thread(() =>
            //{
            //    //StreamReader streamReader = new StreamReader(Console.OpenStandardInput());
            //    while (!canceled)
            //    {
            //        try
            //        {
            //            //if (proc.HasExited)
            //            //    threadInput.Abort();
            //            //proc.StandardInput.BaseStream.WriteByte((byte)Console.OpenStandardInput().ReadByte());
            //            proc.StandardInput.Write((char)Console.OpenStandardInput().ReadByte());
            //        }
            //        catch { }
            //    }
            //});

            Task taskInput = new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        //if (proc.HasExited)
                        //    threadInput.Abort();
                        //proc.StandardInput.BaseStream.WriteByte((byte)Console.OpenStandardInput().ReadByte());
                        proc.StandardInput.Write((char)Console.OpenStandardInput().ReadByte());
                    }
                    catch { }
                }
            });
            taskInput.Start();
            //threadOutput = new Thread(() =>
            //{
            //    while (!canceled)
            //    {
            //        try
            //        {
            //            //if (proc.HasExited)
            //            //{
            //            //    Console.WriteLine("END");
            //            //    threadOutput.Abort();
            //            //}
            //            Console.OpenStandardOutput().WriteByte((byte)proc.StandardOutput.BaseStream.ReadByte());
            //            //Console.Out.Write((char)proc.StandardOutput.BaseStream.ReadByte());
            //        }
            //        catch { }
            //    }
            //    Console.WriteLine();
            //});

            //threadInput.Start();
            //threadOutput.Start();

            //new Thread(() =>
            //{
            //    while (true)
            //    {
            //        try
            //        {
            //            Console.OpenStandardError().WriteByte((byte)proc.StandardError.BaseStream.ReadByte());
            //            //Console.Error.Write((char)proc.StandardError.BaseStream.ReadByte());
            //        }
            //        catch { }
            //    }
            //}).Start();
            proc.Start();
            proc.StandardInput.AutoFlush = true;
            //proc.StandardOutput.BaseStream.CopyTo(Console.OpenStandardOutput());
            //proc.StandardError.BaseStream.CopyTo(Console.OpenStandardError());
            //proc.OutputDataReceived += Proc_OutputDataReceived;
            //proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();
            //new Thread(() => {
            //    //Console.OpenStandardInput().CopyTo(proc.StandardInput.BaseStream);
            //    while (true)
            //    {
            //        Console.Write(proc.StandardOutput.Read());
            //    }
            //}).Start();
            while (true)
            {
                try
                {
                    int cbyte = proc.StandardOutput.BaseStream.ReadByte();
                    if (cbyte != -1)
                        Console.OpenStandardOutput().WriteByte((byte)cbyte);
                    else
                        break;
                }
                catch { }
            }
            //proc.WaitForExit();
            //canceled = true;
            //threadInput.Abort();
            proc.Kill();
            //while (!proc.HasExited)
            //{
            //    //proc.StandardInput.Write(Console.Read());
            //}
            //if (proc.HasExited)
            //    proc.Kill();
        }
    }
}
