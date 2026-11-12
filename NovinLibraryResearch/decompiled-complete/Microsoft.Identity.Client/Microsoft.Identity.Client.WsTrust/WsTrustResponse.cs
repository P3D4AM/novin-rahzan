using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Identity.Client.Internal;

namespace Microsoft.Identity.Client.WsTrust;

internal class WsTrustResponse
{
	public const string Saml1Assertion = "urn:oasis:names:tc:SAML:1.0:assertion";

	public string Token { get; private set; }

	public string TokenType { get; private set; }

	public static WsTrustResponse CreateFromResponse(string response, WsTrustVersion version)
	{
		return CreateFromResponseDocument(XDocument.Parse(response, (LoadOptions)1), version);
	}

	public static string ReadErrorResponse(XDocument responseDocument, RequestContext requestContext)
	{
		string text = null;
		XElement val = ((XContainer)responseDocument).Descendants(XmlNamespace.SoapEnvelope + "Body").FirstOrDefault();
		if (val != null)
		{
			XElement val2 = ((XContainer)val).Elements(XmlNamespace.SoapEnvelope + "Fault").FirstOrDefault();
			if (val2 != null)
			{
				text = GetFaultMessage(val2);
			}
		}
		if (string.IsNullOrWhiteSpace(text))
		{
			text = ((object)responseDocument).ToString();
		}
		return text;
	}

	private static string GetFaultMessage(XElement fault)
	{
		XElement val = ((XContainer)fault).Elements(XmlNamespace.SoapEnvelope + "Reason").FirstOrDefault();
		if (val != null)
		{
			XElement val2 = ((XContainer)val).Elements(XmlNamespace.SoapEnvelope + "Text").FirstOrDefault();
			if (val2 != null)
			{
				using (XmlReader xmlReader = ((XNode)val2).CreateReader())
				{
					xmlReader.MoveToContent();
					return xmlReader.ReadInnerXml();
				}
			}
		}
		return null;
	}

	internal static WsTrustResponse CreateFromResponseDocument(XDocument responseDocument, WsTrustVersion version)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		XNamespace val = XmlNamespace.Trust;
		if (version == WsTrustVersion.WsTrust2005)
		{
			val = XmlNamespace.Trust2005;
		}
		bool flag = true;
		if (version == WsTrustVersion.WsTrust13 && ((XContainer)responseDocument).Descendants(val + "RequestSecurityTokenResponseCollection").FirstOrDefault() == null)
		{
			flag = false;
		}
		if (!flag)
		{
			return null;
		}
		foreach (XElement item in ((XContainer)responseDocument).Descendants(val + "RequestSecurityTokenResponse"))
		{
			XElement val2 = ((XContainer)item).Elements(val + "TokenType").FirstOrDefault();
			if (val2 == null)
			{
				continue;
			}
			XElement val3 = ((XContainer)item).Elements(val + "RequestedSecurityToken").FirstOrDefault();
			if (val3 == null)
			{
				continue;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (XNode item2 in ((XContainer)val3).Nodes())
			{
				stringBuilder.Append(item2.ToString((SaveOptions)1));
			}
			dictionary.Add(val2.Value, stringBuilder.ToString());
		}
		if (dictionary.Count == 0)
		{
			return null;
		}
		string text = (dictionary.ContainsKey("urn:oasis:names:tc:SAML:1.0:assertion") ? "urn:oasis:names:tc:SAML:1.0:assertion" : dictionary.Keys.First());
		return new WsTrustResponse
		{
			TokenType = text,
			Token = dictionary[text]
		};
	}
}
