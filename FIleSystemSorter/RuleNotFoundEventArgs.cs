using System;

namespace FileSystemSorter
{
    public class RuleNotFoundEventArgs:EventArgs
    {
        public string FileName { get; set; }
        public string DefaultPath { get; set; }
    }
}