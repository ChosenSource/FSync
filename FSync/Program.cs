using Spectre.Console;
using FSync.DataManagment;
using System.Xml.Linq;

namespace FSync
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //progres boiler plate
            if (args.Length == 0)
            {

                args = new string[] { "sync" };
            }
            switch (args.Length)
            {
                case 0:
                    {

                        ShowHelp();
                        break;

                    }
                case 1:
                    {
                        switch (args[0].ToLower())
                        {

                            case "--help":
                            case "-h":
                                ShowHelp();
                                break;

                            case "sync":
                                {

                                    int i = 1;
                                    //operation sync blah
                                    var syncs = DataRepository.ReadFromJson();
                                    Console.WriteLine($"Syncing {i} of {syncs.Count} sync's");
                                    await AnsiConsole.Progress()
                                    .StartAsync(async ctx =>
                                    {
                                        // Define tasks
                                        var task1 = ctx.AddTask("[green]Syncing Files[/]");
                                        while (!ctx.IsFinished)
                                        {
                                            foreach (var item in syncs)
                                            {

                                    
                                                await Task.Run(() => { CopyDirectory(item.Path, item.Destination); });
                                                i++;                                  
                                                Console.WriteLine($" Source : {item.Path} Destenation : {item.Destination}");
                                            }
                                            int perc = (100) / syncs.Count;

                                            // Increment
                                            task1.Increment(perc);
                                        }
                                    });




                                    break;
                                }
                            case "list":
                                {
                                    Console.WriteLine("list");
                                    foreach (var item in DataRepository.ReadFromJson())
                                    {
                                        Console.WriteLine($"Syncname : {item.Name} Source : {item.Path} Destenation : {item.Destination}");
                                    }

                                    break;
                                }
                            default:
                                Console.WriteLine($"Unknown option: {args[0]}  Try --help for a list of options.");
                                break;

                        }
                        break;
                    }
                case 2:
                    {
                        switch (args[0].ToLower())
                        {

                            case "delete":
                                {
                                    Console.WriteLine($"delete {args[1]}");
                                    DataRepository.Delete(args[1]);
                                    break;
                                }
                            default:
                                Console.WriteLine($"Unknown option: {args[0]}  Try --help for a list of options.");
                                break;

                        }
                        break;
                    }
                case 4:
                    {
                        switch (args[0].ToLower())
                        {

                            case "add":
                                {
                                    Console.WriteLine($"add {args[1]} {args[2]} {args[3]}");
                                    SyncData syncData = new SyncData() { Name = args[1], Path = args[2], Destination = args[3] };
                                    DataRepository.Add(syncData);
                                    break;
                                }
                            default:
                                Console.WriteLine($"Unknown option: {args[0]}  Try --help for a list of options.");
                                break;

                        }
                        break;
                    }
                default:
                    Console.WriteLine($"Unknown option: {args[0]}  Try --help for a list of options.");
                    break;
            }
        }


        private static void ShowHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  -h, --help                                     ||Show help information.");
            Console.WriteLine("  sync, sync                                     ||sync files");
            Console.WriteLine("  add, add name \"source\" \"destenation\"           ||add a new sync");
            Console.WriteLine("  delete, delete name                            ||delete a sync");
            Console.WriteLine("  list, list                                     ||list sync's");

        }
        static void CopyDirectory(string sourceDir, string destinationDir)
        {
            // Create the destination directory  
            Directory.CreateDirectory(destinationDir);

            // Copy all files in the directory  
            foreach (string filePath in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(filePath);
                string destFilePath = Path.Combine(destinationDir, fileName);
                File.Copy(filePath, destFilePath, true); // Overwrite existing files  
            }

            // Recursively copy subdirectories  
            foreach (string subdirectory in Directory.GetDirectories(sourceDir))
            {
                string subDirName = Path.GetFileName(subdirectory);
                string destSubDirectory = Path.Combine(destinationDir, subDirName);
                CopyDirectory(subdirectory, destSubDirectory); // Recursive call  
            }
        }


    }
}
