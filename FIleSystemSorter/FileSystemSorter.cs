using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace FileSystemSorter
{
    public class FileSystemSorter
    {
        private readonly FileSystemWatcher fsWatcher;
        private readonly IDictionary<string, string> filesPatternsDictionary;
        private readonly string defaultFolderPath;
        private readonly Func<string, string, string> fileNameGenerator;

        public event EventHandler<RuleFoundEventArgs> RuleFound;
        public event EventHandler<RuleNotFoundEventArgs> RuleNotFound;

        protected FileSystemSorter()
        {
        }

        public FileSystemSorter(string watchingDirectory, IDictionary<string, string> filesPatternsDictionary,
            Func<string, string, string> fileNameGenerator,
            string defaultFolderPath)
        {
            if (watchingDirectory == null || filesPatternsDictionary == null || defaultFolderPath == null ||
                fileNameGenerator == null)
            {
                throw new ArgumentNullException();
            }

            if (!Directory.Exists(watchingDirectory))
            {
                throw new ArgumentOutOfRangeException();
            }

            fsWatcher = new FileSystemWatcher(watchingDirectory);
            this.filesPatternsDictionary = filesPatternsDictionary;
            this.defaultFolderPath = defaultFolderPath;
            this.fileNameGenerator = fileNameGenerator;

            fsWatcher.IncludeSubdirectories = true;
            fsWatcher.Created += OnCreated;
        }

        protected virtual void OnRuleFound(RuleFoundEventArgs e)
            => RuleFound?.Invoke(this, e);

        protected virtual void OnRuleNotFound(RuleNotFoundEventArgs e)
            => RuleNotFound?.Invoke(this, e);
        

        private string GetPathToMove(string fileName)
        {
            foreach (var keyValuePair in filesPatternsDictionary)
            {
                var filePattern = keyValuePair.Key;
                var regex = new Regex(filePattern);

                if (regex.IsMatch(fileName))
                {
                    OnRuleFound(new RuleFoundEventArgs
                    {
                        PathToMove = keyValuePair.Value,
                        Rule = filePattern,
                        FileName = fileName
                    });
                    return keyValuePair.Value;
                }
            }
            OnRuleNotFound(new RuleNotFoundEventArgs
            {
                FileName = fileName,
                DefaultPath = defaultFolderPath
            });

            return null;
        }


        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            if (File.Exists(Path.Combine(fsWatcher.Path, e.Name)))
            {
                var destinationPath = GetPathToMove(e.Name) ?? defaultFolderPath;
                MoveFile(e.Name, destinationPath);
            }
        }

        public void AddOnChangedHandler(FileSystemEventHandler handler)
            => fsWatcher.Changed += handler;

        public void AddOnCreatedHandler(FileSystemEventHandler handler)
            => fsWatcher.Created += handler;

        public void AddOnDeletedHandler(FileSystemEventHandler handler)
            => fsWatcher.Deleted += handler;

        public void StartTrackingFolder() 
            => fsWatcher.EnableRaisingEvents = true;

        public void StopTrackingFolder() 
            => fsWatcher.EnableRaisingEvents = false;

        private void MoveFile(string fileName, string destinationPath)
        {
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            var sourcePath = Path.Combine(fsWatcher.Path, fileName);

            var newFileName = fileNameGenerator(Path.GetFileName(fileName), destinationPath);
            var targetPath = Path.Combine(destinationPath, newFileName);

            if (File.Exists(targetPath))
            {
                targetPath = Path.Combine(destinationPath, $"{Guid.NewGuid()}_{newFileName}");
            }

            do
            {
                Thread.Sleep(100);
            } while (File.Exists(sourcePath) && IsFileLocked(new FileInfo(sourcePath)));

            if (File.Exists(sourcePath))
            {
                Directory.Move(sourcePath, targetPath);
            }
        }

        private static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open
                (
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.None
                );
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                stream?.Close();
            }

            return false;
        }
    }
}