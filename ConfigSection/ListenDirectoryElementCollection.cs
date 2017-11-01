using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigSection
{
    [ConfigurationCollection(typeof(ListenDirectoryElement), AddItemName = "listenDirectory")]
    public class ListenDirectoryElementCollection:ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ListenDirectoryElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ListenDirectoryElement)element).Path;
        }
    }
}
