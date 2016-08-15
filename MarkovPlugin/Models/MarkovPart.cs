using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LimeBeanEnhancements.Attributes;
using LimeBeanEnhancements;

namespace MarkovPlugin.Models
{
    [BeanTable("markovpart")]
    public class MarkovPart : EnhancedBean<MarkovPart>
    {
        [BeanProperty("word_before")]
        [BeanRelation(typeof(Word))]
        public Word WordBefore { get; set; }

        [BeanProperty("word")]
        [BeanRelation(typeof(Word))]
        public Word Word { get; set; }

        [BeanProperty("word_after")]
        [BeanRelation(typeof(Word))]
        public Word WordAfter { get; set; }

        [BeanProperty("count")]
        public long Count { get; set; }

        [BeanProperty("start")]
        public long IsStartWord { get; set; }
    }
}
