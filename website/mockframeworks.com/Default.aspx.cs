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
		const string AsirraServiceURL = "http://challenge.asirra.com/cgi/Asirra";
		string newId;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Page.IsPostBack && String.IsNullOrEmpty(DeterminePostBackMode()["__EVENTARGUMENT"]))
			{
				if (Page.IsValid)
				{
					if (ValidatesAsirraChallenge())
					{
						AddNew();
					}
					else
					{
						validationFailed.Visible = true;
						validationFailed.Text = "Could not validate Asirra ticket. Please try again (unless you're a bot ;)).";
					}
				}
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

		bool ValidatesAsirraChallenge()
		{
			// We get a quoted string and keep it quoted in order to construct a query url
			string ticket = Request.QueryString.GetValues("Asirra_Ticket")[0];
			string validationURL = AsirraServiceURL + "?action=ValidateTicket&ticket=" + ticket;

			System.Xml.XmlDocument validationDocument = new System.Xml.XmlDocument();

			using (XmlReader reader = XmlReader.Create(validationURL))
			{
				validationDocument.Load(reader);
			}

			string validationValue = validationDocument.GetElementsByTagName("Result")[0].ChildNodes[0].Value;

			// If Asirra tells us the challenge was passed, return true
			return validationValue == "Pass";
		}
	}
}
