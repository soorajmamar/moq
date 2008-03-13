using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace MockFrameworks
{
	public partial class Default : System.Web.UI.Page, IPostBackEventHandler
	{
		string newId;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Page.IsPostBack && String.IsNullOrEmpty(DeterminePostBackMode()["__EVENTARGUMENT"]))
			{
				AddNew();
			}
		}

		protected bool IsNew(string itemId)
		{
			return itemId == newId;
		}

		protected IEnumerable<KeyValuePair<string, IEnumerable<SyndicationItem>>> GetItems()
		{
			return (from item in ReadFeed().Items
					orderby item.Title.Text
					group item by item.Categories.First().Name into g
					select new KeyValuePair<string, IEnumerable<SyndicationItem>>(g.Key, g.AsEnumerable())
				   ).ToList();
		}

		public void RaisePostBackEvent(string eventArgument)
		{
			DeleteExisting(eventArgument);
		}

		private void AddNew()
		{
			var item = new SyndicationItem(
				txtName.Text,
				txtName.Text,
				new Uri(txtRedirectUrl.Text),
				"http://www.mockframeworks.com/" + txtShortUrl.Text,
				DateTimeOffset.Now);
			item.Categories.Add(new SyndicationCategory(txtPlatform.Text));

			var feed = ReadFeed();
			var modified = new SyndicationFeed(feed.Title.Text, feed.Description.Text,
					new Uri("http://www.mockframeworks.com/frameworks.xml"),
					feed.Items.Concat(new[] { item }));

			SaveFeed(modified);

			newId = item.Id;
		}

		private void DeleteExisting(string frameworkId)
		{
			var feed = ReadFeed();
			var modified = new SyndicationFeed(feed.Title.Text, feed.Description.Text,
					new Uri("http://www.mockframeworks.com/frameworks.xml"),
					feed.Items.Where(item => item.Id != frameworkId));

			SaveFeed(modified);
		}

		private SyndicationFeed ReadFeed()
		{
			using (XmlReader xr = XmlReader.Create(Server.MapPath("frameworks.xml")))
			{
				return SyndicationFeed.Load(xr);
			}
		}

		private void SaveFeed(SyndicationFeed modified)
		{
			using (XmlWriter xw = XmlWriter.Create(Server.MapPath("frameworks.xml"), new XmlWriterSettings { Indent = true }))
			{
				modified.SaveAsRss20(xw);
			}

			Cache["feed"] = modified;
		}
	}
}
