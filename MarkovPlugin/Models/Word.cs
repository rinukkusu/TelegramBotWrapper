using LimeBeanEnhancements;
using LimeBeanEnhancements.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkovPlugin.Models
{
    [BeanTable("word")]
    public class Word : EnhancedBean<Word>
    {
        [BeanProperty("value")]
        public string Value { get; set; }
    }
}
