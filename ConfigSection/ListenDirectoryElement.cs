using System.Configuration;

namespace ConfigSection
{
    public class ListenDirectoryElement : ConfigurationElement
    {
        [ConfigurationProperty("path", IsKey = true, IsRequired = true)]
        public string Path => (string) base["path"];
    }
}