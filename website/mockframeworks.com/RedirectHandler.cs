using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;

namespace MockFrameworks
{
	public class RedirectHandler : IHttpHandler
	{
		public bool IsReusable
		{
			get { return true; }
		}

		public void ProcessRequest(HttpContext context)
		{
			var feed = (SyndicationFeed)context.Cache["feed"];

			string id = context.Request.Path.Replace(".ashx", "").ToLower();
			if (id.StartsWith("/"))
				id = id.Substring(1);

			var item = feed.Items.Where(i => i.Id.ToLower().EndsWith(id)).FirstOrDefault();

			if (item != null)
				context.Response.Redirect(item.Links[0].Uri.ToString());
			else
				context.Response.Redirect("~/");
		}
	}
}
