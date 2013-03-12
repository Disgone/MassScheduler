using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using System.Xml;
using MassScheduler.Models;

namespace MassScheduler.Helpers
{
    public class RssResult : FileResult
    {
        public List<Meeting> Meetings { get; set; }
        public string Title { get; set; }

        private Uri _currentUrl;

        public RssResult() : base("application/rss+xml") { }

        public RssResult(List<Meeting> meetings, string title)
            : this()
        {
            Meetings = meetings;
            Title = title;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            _currentUrl = context.RequestContext.HttpContext.Request.Url;
            base.ExecuteResult(context);
        }

        protected override void WriteFile(System.Web.HttpResponseBase response)
        {
            var items = new List<SyndicationItem>();

            foreach (Meeting d in Meetings)
            {
                var contentString = String.Format("{0} by {1} on {2:MMM dd, yyyy} at {3}. Where: {4}",
                            d.Title, d.Sponsor, d.StartDate, d.StartDate.ToLocalTime().ToShortTimeString(), d.Location);

                var item = new SyndicationItem(
                    d.Title, 
                    contentString, 
                    new Uri("http://temp/meetings/details/" + d.Id),
                    d.Id.ToString(), 
                    d.StartDate.ToUniversalTime()
                    )
                {
                    PublishDate = d.StartDate.ToUniversalTime(),
                    Summary = new TextSyndicationContent(contentString, TextSyndicationContentKind.Plaintext)
                };
                items.Add(item);
            }

            var feed = new SyndicationFeed(
                Title,
                Title,
                _currentUrl,
                items);

            var formatter = new Rss20FeedFormatter(feed);

            using (var writer = XmlWriter.Create(response.Output))
            {
                formatter.WriteTo(writer);
            }
        }
    }
}