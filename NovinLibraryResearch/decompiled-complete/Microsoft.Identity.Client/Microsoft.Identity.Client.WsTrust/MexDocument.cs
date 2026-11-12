using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Identity.Client.Internal;

namespace Microsoft.Identity.Client.WsTrust;

internal class MexDocument
{
	private class MexPolicy
	{
		public WsTrustVersion Version { get; set; }

		public string Id { get; set; }

		public UserAuthType AuthType { get; set; }

		public Uri Url { get; set; }
	}

	private const string WsTrustSoapTransport = "http://schemas.xmlsoap.org/soap/http";

	private readonly Dictionary<string, MexPolicy> _policies = new Dictionary<string, MexPolicy>();

	private readonly Dictionary<string, MexPolicy> _bindings = new Dictionary<string, MexPolicy>();

	public MexDocument(string responseBody)
	{
		XDocument val = XDocument.Parse(responseBody, (LoadOptions)0);
		ReadPolicies((XContainer)(object)val);
		ReadPolicyBindings((XContainer)(object)val);
		SetPolicyEndpointAddresses((XContainer)(object)val);
	}

	public WsTrustEndpoint GetWsTrustUsernamePasswordEndpoint()
	{
		return GetWsTrustEndpoint(UserAuthType.UsernamePassword);
	}

	public WsTrustEndpoint GetWsTrustWindowsTransportEndpoint()
	{
		return GetWsTrustEndpoint(UserAuthType.IntegratedAuth);
	}

	private WsTrustEndpoint GetWsTrustEndpoint(UserAuthType userAuthType)
	{
		MexPolicy mexPolicy = SelectPolicy(userAuthType);
		if (mexPolicy == null)
		{
			return null;
		}
		return new WsTrustEndpoint(mexPolicy.Url, mexPolicy.Version);
	}

	private MexPolicy SelectPolicy(UserAuthType userAuthType)
	{
		return _policies.Values.Where((MexPolicy p) => p.Url != null && p.AuthType == userAuthType && p.Version == WsTrustVersion.WsTrust13).FirstOrDefault() ?? _policies.Values.Where((MexPolicy p) => p.Url != null && p.AuthType == userAuthType).FirstOrDefault();
	}

	private void ReadPolicies(XContainer mexDocument)
	{
		foreach (XElement item in ((XContainer)mexDocument.Elements().First()).Elements(XmlNamespace.Wsp + "Policy"))
		{
			XElement val = ((XContainer)item).Elements(XmlNamespace.Wsp + "ExactlyOne").FirstOrDefault();
			if (val == null)
			{
				continue;
			}
			foreach (XElement item2 in ((XContainer)val).Descendants(XmlNamespace.Wsp + "All"))
			{
				XNamespace sp = XmlNamespace.Sp;
				XElement val2 = ((XContainer)item2).Elements(XmlNamespace.Http + "NegotiateAuthentication").FirstOrDefault();
				if (val2 != null)
				{
					AddPolicy(item, UserAuthType.IntegratedAuth);
				}
				val2 = ((XContainer)item2).Elements(sp + "SignedEncryptedSupportingTokens").FirstOrDefault();
				if (val2 == null && (val2 = ((XContainer)item2).Elements(XmlNamespace.Sp2005 + "SignedSupportingTokens").FirstOrDefault()) == null)
				{
					continue;
				}
				sp = XmlNamespace.Sp2005;
				XElement val3 = ((XContainer)val2).Elements(XmlNamespace.Wsp + "Policy").FirstOrDefault();
				if (val3 == null)
				{
					continue;
				}
				XElement val4 = ((XContainer)val3).Elements(sp + "UsernameToken").FirstOrDefault();
				if (val4 != null)
				{
					XElement val5 = ((XContainer)val4).Elements(XmlNamespace.Wsp + "Policy").FirstOrDefault();
					if (val5 != null && ((XContainer)val5).Elements(sp + "WssUsernameToken10").FirstOrDefault() != null)
					{
						AddPolicy(item, UserAuthType.UsernamePassword);
					}
				}
			}
		}
	}

	private void ReadPolicyBindings(XContainer mexDocument)
	{
		foreach (XElement item in ((XContainer)mexDocument.Elements().First()).Elements(XmlNamespace.Wsdl + "binding"))
		{
			foreach (XElement item2 in ((XContainer)item).Elements(XmlNamespace.Wsp + "PolicyReference"))
			{
				XAttribute val = item2.Attribute(XName.op_Implicit("URI"));
				if (val == null || !_policies.ContainsKey(val.Value))
				{
					continue;
				}
				XAttribute val2 = item.Attribute(XName.op_Implicit("name"));
				if (val2 == null)
				{
					continue;
				}
				XElement val3 = ((XContainer)item).Elements(XmlNamespace.Wsdl + "operation").FirstOrDefault();
				if (val3 == null)
				{
					continue;
				}
				XElement val4 = ((XContainer)val3).Elements(XmlNamespace.Soap12 + "operation").FirstOrDefault();
				if (val4 == null)
				{
					continue;
				}
				XAttribute val5 = val4.Attribute(XName.op_Implicit("soapAction"));
				if (val5 == null || (string.Compare(((object)XmlNamespace.Issue).ToString(), val5.Value, StringComparison.OrdinalIgnoreCase) != 0 && string.Compare(((object)XmlNamespace.Issue2005).ToString(), val5.Value, StringComparison.OrdinalIgnoreCase) != 0))
				{
					continue;
				}
				bool flag = string.Compare(((object)XmlNamespace.Issue2005).ToString(), val5.Value, StringComparison.OrdinalIgnoreCase) == 0;
				_policies[val.Value].Version = (flag ? WsTrustVersion.WsTrust2005 : WsTrustVersion.WsTrust13);
				XElement val6 = ((XContainer)item).Elements(XmlNamespace.Soap12 + "binding").FirstOrDefault();
				if (val6 != null)
				{
					XAttribute val7 = val6.Attribute(XName.op_Implicit("transport"));
					if (val7 != null && string.Compare("http://schemas.xmlsoap.org/soap/http", val7.Value, StringComparison.OrdinalIgnoreCase) == 0)
					{
						_bindings.Add(val2.Value, _policies[val.Value]);
					}
				}
			}
		}
	}

	private void SetPolicyEndpointAddresses(XContainer mexDocument)
	{
		foreach (XElement item in ((XContainer)((XContainer)mexDocument.Elements().First()).Elements(XmlNamespace.Wsdl + "service").First()).Elements(XmlNamespace.Wsdl + "port"))
		{
			XAttribute val = item.Attribute(XName.op_Implicit("binding"));
			if (val == null)
			{
				continue;
			}
			string[] array = val.Value.Split(new char[1] { ':' }, 2);
			if (array.Length < 2 || !_bindings.ContainsKey(array[1]))
			{
				continue;
			}
			XElement val2 = ((XContainer)item).Elements(XmlNamespace.Wsa10 + "EndpointReference").FirstOrDefault();
			if (val2 != null)
			{
				XElement val3 = ((XContainer)val2).Elements(XmlNamespace.Wsa10 + "Address").FirstOrDefault();
				if (val3 != null && Uri.IsWellFormedUriString(val3.Value, UriKind.Absolute))
				{
					_bindings[array[1]].Url = new Uri(val3.Value);
				}
			}
		}
	}

	private void AddPolicy(XElement policy, UserAuthType policyAuthType)
	{
		if ((((XContainer)policy).Descendants(XmlNamespace.Sp + "TransportBinding").FirstOrDefault() ?? ((XContainer)policy).Descendants(XmlNamespace.Sp2005 + "TransportBinding").FirstOrDefault()) != null)
		{
			XAttribute val = policy.Attribute(XmlNamespace.Wsu + "Id");
			if (val != null)
			{
				_policies.Add("#" + val.Value, new MexPolicy
				{
					Id = val.Value,
					AuthType = policyAuthType
				});
			}
		}
	}
}
