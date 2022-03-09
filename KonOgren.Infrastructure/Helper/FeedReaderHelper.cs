using KonOgren.Infrastructure.Result;
using KonOgren.Infrastructure.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;

namespace KonOgren.Infrastructure.Helper
{
    public class FeedReaderHelper
    {
        public static Result<List<FeedViewModel>> FetchFeed(string url) {
            Result<List<FeedViewModel>> result = new Result<List<FeedViewModel>>();
            try
            {
                XmlReader reader = XmlReader.Create(url);
                SyndicationFeed feed = SyndicationFeed.Load(reader);
                reader.Close();
                var topFiveFeed = feed.Items.Take(5)
                    .Select(a => new FeedViewModel { Title = a.Title.Text, Link = a.Links.FirstOrDefault().Uri.AbsoluteUri, PublishDate = a.LastUpdatedTime }).ToList();
                result.Data = topFiveFeed;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;

            }
            return result;
        }
    }
}
