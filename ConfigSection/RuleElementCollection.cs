using System.Configuration;

namespace ConfigSection
{
    [ConfigurationCollection(typeof(RuleElement),AddItemName = "rule")]
    public class RuleElementCollection : ConfigurationElementCollection
    {
        [ConfigurationProperty("addIndexNumber")]
        public bool EnableAddFileIndex => (bool)base["addIndexNumber"];

        [ConfigurationProperty("addCreationDate")]
        public bool EnableCreateDateAddition => (bool)base["addCreationDate"];
        protected override ConfigurationElement CreateNewElement()
        {
            return new RuleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RuleElement) element).FileNameRegexPattern;
        }
    }
}
