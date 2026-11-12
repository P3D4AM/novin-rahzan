using System.Xml.Linq;

namespace Microsoft.Identity.Client.Internal;

internal static class XmlNamespace
{
	public static readonly XNamespace Wsdl = XNamespace.op_Implicit("http://schemas.xmlsoap.org/wsdl/");

	public static readonly XNamespace Wsp = XNamespace.op_Implicit("http://schemas.xmlsoap.org/ws/2004/09/policy");

	public static readonly XNamespace Http = XNamespace.op_Implicit("http://schemas.microsoft.com/ws/06/2004/policy/http");

	public static readonly XNamespace Sp = XNamespace.op_Implicit("http://docs.oasis-open.org/ws-sx/ws-securitypolicy/200702");

	public static readonly XNamespace Sp2005 = XNamespace.op_Implicit("http://schemas.xmlsoap.org/ws/2005/07/securitypolicy");

	public static readonly XNamespace Wsu = XNamespace.op_Implicit("http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");

	public static readonly XNamespace Soap12 = XNamespace.op_Implicit("http://schemas.xmlsoap.org/wsdl/soap12/");

	public static readonly XNamespace Wsa10 = XNamespace.op_Implicit("http://www.w3.org/2005/08/addressing");

	public static readonly XNamespace Trust = XNamespace.op_Implicit("http://docs.oasis-open.org/ws-sx/ws-trust/200512");

	public static readonly XNamespace Trust2005 = XNamespace.op_Implicit("http://schemas.xmlsoap.org/ws/2005/02/trust");

	public static readonly XNamespace Issue = XNamespace.op_Implicit("http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Issue");

	public static readonly XNamespace Issue2005 = XNamespace.op_Implicit("http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue");

	public static readonly XNamespace SoapEnvelope = XNamespace.op_Implicit("http://www.w3.org/2003/05/soap-envelope");
}
