using System.Configuration;

namespace ConfigSection
{
    public class DefaultDirectoryElement:ConfigurationElement
    {
        [ConfigurationProperty("path")]
        public string Path => (string)base["path"];
    }
}