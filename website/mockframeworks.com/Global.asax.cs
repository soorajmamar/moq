using System;
using System.ServiceModel.Syndication;
using System.Xml;

namespace MockFrameworks
{
	public class Global : System.Web.HttpApplication
	{
		protected void Application_Start(object sender, EventArgs e)
		{
			using (XmlReader xr = XmlReader.Create(Server.MapPath("frameworks.xml")))
			{
				Context.Cache["feed"] = SyndicationFeed.Load(xr);
			}
		}
	}
}