using System.Configuration;

namespace ConfigSection
{
    public class BCLConfigurationSection:ConfigurationSection
    {
        [ConfigurationProperty("rules")]
        public RuleElementCollection Rules => (RuleElementCollection)base["rules"];

        [ConfigurationProperty("culture")]
        public CultureElement Culture => (CultureElement)this["culture"];
        [ConfigurationProperty("defaultDirectory")]
        public DefaultDirectoryElement DefaultDirectory => (DefaultDirectoryElement) this["defaultDirectory"];

        [ConfigurationProperty("listenDirectories")]
        public ListenDirectoryElementCollection ListenDirectories => (ListenDirectoryElementCollection)base["listenDirectories"];
    }
}
