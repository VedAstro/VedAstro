using System.Diagnostics;
using System.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using CliWrap;
using CliWrap.Buffered;
using System.IO.Compression;
using System.Reflection;
using System.IO;

namespace APILauncher
{
    internal class Program
    {

        static async Task Main()
        {
            PrintGreetingMessage();

            Console.WriteLine("⏱ T-minus countdown");
            Console.WriteLine("✅ Go/No-Go Poll");
            Console.WriteLine(".NET 8 Runtime go!");
            Console.WriteLine("Function Core Tools go!");
            Console.WriteLine("🚦 All systems are go");
            Console.WriteLine("🔑 Astronauts, start your engines");
            Console.WriteLine("💨 Main engine start ");
            Console.WriteLine("💥 Ignition sequence start");
            Console.WriteLine("🔥 All engines running");
            Console.WriteLine("🚀 We have lift off!");

            Console.WriteLine("🧐 Checking Requirements...");

            //check if .NET is installed, else install it first
            if (!IsDotNetInstalled()) { InstallDotNet(); }

            // set file permissions
            //Console.WriteLine("🔐 Setting file permissions...");
            //string[] commands = new string[]
            //{
            //    "chmod +x ./Azure.Functions.Cli/func",
            //    "chmod +x ./api-build/API",
            //};

            //foreach (var command in commands)
            //{
            //    ExecuteCommand(command);
            //}


            await StartAPIServer();


            Console.ReadLine();
        }

        private static void PrintGreetingMessage()
        {
            Console.WriteLine("\t✨ VedAstro ✨");
            Console.WriteLine("--- Open Source. Powerful. Free. ---");
            Console.WriteLine("💫 Astronomy - 🔮 Astrology - 💻 API Server");
            Console.WriteLine("-------------------------------------------------");
        }

        static bool IsDotNetInstalled()
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = "--list-runtimes",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                process.WaitForExit();
                var isDotNetInstalled = process.ExitCode == 0;

                //print out for easy debug
                Console.WriteLine(!isDotNetInstalled ? ".NET not found ❌" : ".NET found ✅");

                return isDotNetInstalled;
            }
            catch (Exception)
            {
                Console.WriteLine(".NET not found ❌");
                return false;
            }
        }

        static void ExecuteCommand(string command)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo("bash", $"-c \"{command}\"")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            using (Process process = new Process())
            {
                process.StartInfo = processInfo;
                process.Start();

                // Read the output (if needed)
                string output = process.StandardOutput.ReadToEnd();

                process.WaitForExit();
                int exitCode = process.ExitCode;

                //Console.WriteLine($"Command: {command}");
                //Console.WriteLine($"Exit Code: {exitCode}");
                //Console.WriteLine($"Output:\n{output}\n");
            }
        }

        static void InstallDotNet()
        {
            Console.WriteLine("Staring .NET install...⌛");

            //install command for mac osx
            ExecuteCommand("sudo installer -pkg dotnet-runtime-8.0.6-osx-x64.pkg -target /");
        }

        private static async Task StartAPIServer()
        {
            Console.WriteLine("Starting API Server 🚀");

            //NOTE : important to get exact location where executable is dynamically located in Mac, Linux and Windows
            var pathToApiLauncherExec = Process.GetCurrentProcess().MainModule?.FileName; //full file path
            //extract out the location of parent folder (directory)
            string apiLauncherDirectoryPath = Path.GetDirectoryName(pathToApiLauncherExec);
            //print out for east Debug
            Console.WriteLine("CurrentDirectory" + apiLauncherDirectoryPath);


            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    // Register any additional services here if needed.
                })
                .ConfigureLogging((context, configLogging) =>
                {
                    configLogging.AddConsole();
                })
                .Build();

            await host.StartAsync();

            try
            {
                //starts func located inside "api-build/Azure.Functions.Cli/func"
                ProcessStartInfo processInfo = new ProcessStartInfo("Azure.Functions.Cli/func", @$"start --no-build --verbose")
                {
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    WorkingDirectory = apiLauncherDirectoryPath,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processInfo))
                {
                    string line;
                    while ((line = await process.StandardOutput.ReadLineAsync()) != null)
                    {
                        Console.WriteLine($"[stdout]: {line}");
                    }

                    foreach (var errorLine in process.StandardError.ReadToEnd().Split('\n'))
                    {
                        Console.WriteLine($"[stderr]: {errorLine}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            //needed to Kill `Func` and `API` process after close
            await host.StopAsync();

        }
    }

}
