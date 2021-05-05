using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Talk_to_me
{
    class Program
    {
        static void Main(string[] args)
        {
           // Console.WriteLine("Hello World!");

            Speech.ExecuteSpeak("Olá! Tudo bem? How are you? Ça-vá?");
        }
    }

    public static class Speech
    {
        public static void ExecuteSpeak(string text, bool wait = false)
        {
            Execute(
                $@"Add-Type -AssemblyName System.speech; 
                $speak = New-Object System.Speech.Synthesis.SpeechSynthesizer; 
                $speak.Speak(""{text}"");");

            void Execute(string command)
            {
                string path = Path.GetTempPath() + Guid.NewGuid() + ".ps1";

                using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
                {
                    sw.Write(command);

                    ProcessStartInfo start = new ProcessStartInfo()
                    {
                        FileName = @"C:\Windows\System32\windowspowershell\v1.0\powershell.exe",
                        LoadUserProfile = false,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        Arguments = $"-executionpolicy bypass -File {path}",
                        WindowStyle = ProcessWindowStyle.Hidden
                    };

                    Process process = Process.Start(start);

                    if (wait)
                        process.WaitForExit();
                }
            }
        }
    }
}
