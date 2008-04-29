using System;
using System.Web.UI.WebControls;

/// <summary>
/// Provides an ASP.NET validator that wraps the 
/// CAPTCHA-style Asirra service from MSR.
/// </summary>
public class AsirraValidator : BaseValidator
{
	const string AsirraServiceURL = "http://challenge.asirra.com/cgi/Asirra";

	protected override bool EvaluateIsValid()
	{
		return ValidatesAsirraChallenge();
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
