using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SectorBalanceClient.Entities
{
    public class ContentItem
    {
        public ContentItem()
        {

        }

        public string Id { get; set; }

        public string Comment { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }
    }
}
