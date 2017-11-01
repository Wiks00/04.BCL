using System;

namespace FileSystemSorter
{
    public class RuleFoundEventArgs : EventArgs
    {
        public string Rule { get; set; }
        public string PathToMove { get; set; }
        public string FileName { get; set; }
    }
}