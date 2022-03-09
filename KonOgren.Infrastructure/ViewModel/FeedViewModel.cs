using System;
using System.Collections.Generic;
using System.Text;

namespace KonOgren.Infrastructure.ViewModel
{
    public class FeedViewModel
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public DateTimeOffset PublishDate { get; set; }
    }
}
