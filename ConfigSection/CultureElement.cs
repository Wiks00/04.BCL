using System.Configuration;

namespace ConfigSection
{
    public class CultureElement:ConfigurationElement
    {
        [ConfigurationProperty("name")]
        public string Name => (string)base["name"];
    }
}
