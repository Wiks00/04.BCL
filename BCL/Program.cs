using ConfigSection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;


namespace BCL
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CreateSpecificCulture(Configuration.Culture.Name);
            
            var patternPathDictionary = Configuration.Rules.Cast<RuleElement>().ToDictionary(rule => rule.FileNameRegexPattern, rule => rule.DestinationPath);
            var listenDirectories = Configuration.ListenDirectories;

            var fsSorters = new List<FileSystemSorter.FileSystemSorter>();

            foreach (ListenDirectoryElement directory in listenDirectories)
            {
                var sorter =
                    new FileSystemSorter.FileSystemSorter(directory.Path, patternPathDictionary, GenerateNewFileName, Configuration.DefaultDirectory.Path);

                sorter.AddOnCreatedHandler(OnCreated);
                sorter.AddOnChangedHandler(OnChanged);
                sorter.AddOnDeletedHandler(OnDeleted);

                sorter.RuleFound += (s, e) => Console.WriteLine(Resource.FileFoundMessage, e.Rule, e.FileName,
                    e.PathToMove);
                sorter.RuleNotFound += (s, e) => Console.WriteLine(Resource.FileNotFoundMessage, e.FileName, e.DefaultPath);

                fsSorters.Add(sorter);
            }
            Console.WriteLine(Resource.EnableDisableTrackingTip);

            while (true)
            {
                var key = Console.ReadKey().Key;

                if (key == ConsoleKey.D)
                {
                    foreach (var sorter in fsSorters)
                    {
                        sorter.StopTrackingFolder();
                    }

                    Console.WriteLine();
                    Console.WriteLine(Resource.TrackingStopped);
                }

                if (key == ConsoleKey.E)
                {
                    foreach (var sorter in fsSorters)
                    {
                        sorter.StartTrackingFolder();
                    }

                    Console.WriteLine();
                    Console.WriteLine(Resource.TrackingStarted);
                }
            }
        }
        private static BCLConfigurationSection Configuration 
            => (BCLConfigurationSection)ConfigurationManager.GetSection("SorterServiceConfiguration");

        private static string GetFileNameWithoutFolders(string path) 
            => path.Split('\\').Last();
        private static string GetPathWithoutFile(string path) 
            => path.Substring(0, path.LastIndexOf('\\'));

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(Resource.EntityChangedMsg, GetFileNameWithoutFolders(e.Name),
                GetPathWithoutFile(e.FullPath));
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(Resource.EntityCreatedMsg, GetFileNameWithoutFolders(e.Name),
                GetPathWithoutFile(e.FullPath));
        }

        private static void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(Resource.EntityDeletedMsg, GetFileNameWithoutFolders(e.Name),
                GetPathWithoutFile(e.FullPath));
        }

        private static string GenerateNewFileName(string fileName, string destinationPath)
        {
            string newFileName = fileName;

            if (Configuration.Rules.EnableCreateDateAddition)
            {
                var dateTimeFormat = CultureInfo.CurrentUICulture.DateTimeFormat;
                var date = DateTime.Now.ToString("G", dateTimeFormat);
                var separatorList = date.Where(x => !char.IsNumber(x)).ToList();

                foreach (var separator in separatorList)
                {
                    date = date.Replace(separator, '_');
                }

                newFileName = $"{date}_{newFileName}";
            }

            if (Configuration.Rules.EnableAddFileIndex)
            {
                var fileCount = Directory.GetFiles(destinationPath).Length;
                newFileName = $"{++fileCount}_{newFileName}";
            }

            return newFileName;
        }
    }
}