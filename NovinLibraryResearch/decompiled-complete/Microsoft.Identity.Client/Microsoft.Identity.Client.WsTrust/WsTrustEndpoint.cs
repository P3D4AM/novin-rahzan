using System;
using System.Globalization;
using System.Text;
using System.Xml;
using Microsoft.Identity.Client.Utils;

namespace Microsoft.Identity.Client.WsTrust;

internal class WsTrustEndpoint
{
	private const string EnvelopeNamespaceValue = "http://www.w3.org/2003/05/soap-envelope";

	private const string WsuNamespaceValue = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";

	private readonly ITimeService _timeService;

	private readonly IGuidFactory _guidFactory;

	public Uri Uri { get; }

	public WsTrustVersion Version { get; }

	public WsTrustEndpoint(Uri uri, WsTrustVersion version, ITimeService timeService = null, IGuidFactory guidFactory = null)
	{
		Uri = uri;
		Version = version;
		_timeService = timeService ?? new TimeService();
		_guidFactory = guidFactory ?? new GuidFactory();
	}

	public string BuildTokenRequestMessageWindowsIntegratedAuth(string cloudAudienceUri)
	{
		return BuildTokenRequestMessage(UserAuthType.IntegratedAuth, cloudAudienceUri, string.Empty, string.Empty);
	}

	public string BuildTokenRequestMessageUsernamePassword(string cloudAudienceUri, string username, string password)
	{
		return BuildTokenRequestMessage(UserAuthType.UsernamePassword, cloudAudienceUri, username, password);
	}

	private string BuildTokenRequestMessage(UserAuthType authType, string cloudAudienceUri, string username, string password)
	{
		string text;
		string ns;
		string text2;
		string text3;
		if (Version == WsTrustVersion.WsTrust2005)
		{
			text = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue";
			ns = "http://schemas.xmlsoap.org/ws/2005/02/trust";
			text2 = "http://schemas.xmlsoap.org/ws/2005/05/identity/NoProofKey";
			text3 = "http://schemas.xmlsoap.org/ws/2005/02/trust/Issue";
		}
		else
		{
			text = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Issue";
			ns = "http://docs.oasis-open.org/ws-sx/ws-trust/200512";
			text2 = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/Bearer";
			text3 = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/Issue";
		}
		using StringWriterWithEncoding stringWriterWithEncoding = new StringWriterWithEncoding(Encoding.UTF8);
		using (XmlWriter xmlWriter = XmlWriter.Create(stringWriterWithEncoding, new XmlWriterSettings
		{
			Async = false,
			Encoding = Encoding.UTF8,
			CloseOutput = false
		}))
		{
			xmlWriter.WriteStartElement("s", "Envelope", "http://www.w3.org/2003/05/soap-envelope");
			xmlWriter.WriteAttributeString("wsa", "http://www.w3.org/2000/xmlns/", "http://www.w3.org/2005/08/addressing");
			xmlWriter.WriteAttributeString("wsu", "http://www.w3.org/2000/xmlns/", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
			xmlWriter.WriteStartElement("Header", "http://www.w3.org/2003/05/soap-envelope");
			xmlWriter.WriteStartElement("Action", "http://www.w3.org/2005/08/addressing");
			xmlWriter.WriteAttributeString("mustUnderstand", "http://www.w3.org/2003/05/soap-envelope", "1");
			xmlWriter.WriteString(text);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("messageID", "http://www.w3.org/2005/08/addressing");
			xmlWriter.WriteString("urn:uuid:" + _guidFactory.NewGuid().ToString("D"));
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("ReplyTo", "http://www.w3.org/2005/08/addressing");
			xmlWriter.WriteStartElement("Address", "http://www.w3.org/2005/08/addressing");
			xmlWriter.WriteString("http://www.w3.org/2005/08/addressing/anonymous");
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("To", "http://www.w3.org/2005/08/addressing");
			xmlWriter.WriteAttributeString("mustUnderstand", "http://www.w3.org/2003/05/soap-envelope", "1");
			xmlWriter.WriteString(Uri.ToString());
			xmlWriter.WriteEndElement();
			if (authType == UserAuthType.UsernamePassword)
			{
				AppendSecurityHeader(xmlWriter, username, password);
			}
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("Body", "http://www.w3.org/2003/05/soap-envelope");
			xmlWriter.WriteStartElement("wst", "RequestSecurityToken", ns);
			xmlWriter.WriteStartElement("wsp", "AppliesTo", "http://schemas.xmlsoap.org/ws/2004/09/policy");
			xmlWriter.WriteStartElement("EndpointReference", "http://www.w3.org/2005/08/addressing");
			xmlWriter.WriteStartElement("Address", "http://www.w3.org/2005/08/addressing");
			xmlWriter.WriteString(cloudAudienceUri);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("KeyType", ns);
			xmlWriter.WriteString(text2);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("RequestType", ns);
			xmlWriter.WriteString(text3);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
		}
		return stringWriterWithEncoding.ToString();
	}

	private void AppendSecurityHeader(XmlWriter writer, string username, string password)
	{
		DateTime utcNow = _timeService.GetUtcNow();
		string value = BuildTimeString(utcNow);
		string value2 = BuildTimeString(utcNow.AddMinutes(10.0));
		string value3 = ((Version == WsTrustVersion.WsTrust2005) ? "UnPwSecTok2005-" : "UnPwSecTok13-") + _guidFactory.NewGuid().ToString("D");
		writer.WriteStartElement("wsse", "Security", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
		writer.WriteAttributeString("mustUnderstand", "http://www.w3.org/2003/05/soap-envelope", "1");
		writer.WriteStartElement("Timestamp", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
		writer.WriteAttributeString("Id", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd", "MSATimeStamp");
		writer.WriteElementString("Created", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd", value);
		writer.WriteElementString("Expires", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd", value2);
		writer.WriteEndElement();
		writer.WriteStartElement("UsernameToken", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
		writer.WriteAttributeString("Id", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd", value3);
		writer.WriteElementString("Username", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd", username);
		writer.WriteElementString("Password", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd", password);
		writer.WriteEndElement();
		writer.WriteEndElement();
	}

	private static string BuildTimeString(DateTime utcTime)
	{
		return utcTime.ToString("yyyy-MM-ddTHH:mm:ss.068Z", CultureInfo.InvariantCulture);
	}
}
