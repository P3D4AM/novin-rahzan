using System;
using System.IO;
using System.Security;
using System.Xml;
using Microsoft.SqlServer.Management.Common;

namespace Microsoft.SqlServer.Management.Smo.RegSvrEnum;

public abstract class RegistrationInfo : IComparable
{
	private string fName;

	private string desc;

	private Guid stype;

	private ParentRegistrationInfo parent;

	public string FriendlyName
	{
		get
		{
			return fName;
		}
		set
		{
			if (!(value == fName))
			{
				if (Parent != null && Parent.HasChildWithName(value))
				{
					throw new RegisteredServerException(SRError.ErrCannotHaveSiblingsWithSameName);
				}
				fName = value;
			}
		}
	}

	public string Description
	{
		get
		{
			return desc;
		}
		set
		{
			desc = value;
		}
	}

	public Guid ServerType
	{
		get
		{
			if (stype != Guid.Empty)
			{
				return stype;
			}
			foreach (RegistrationInfo ancestor in Ancestors)
			{
				if (ancestor.stype != Guid.Empty)
				{
					return ancestor.stype;
				}
			}
			return Guid.Empty;
		}
		set
		{
			stype = value;
		}
	}

	public ParentRegistrationInfo Parent
	{
		get
		{
			return parent;
		}
		set
		{
			parent = value;
		}
	}

	internal RegistrationInfoCollection Ancestors
	{
		get
		{
			RegistrationInfoCollection registrationInfoCollection = new RegistrationInfoCollection();
			RegistrationInfo registrationInfo = this;
			while (registrationInfo.Parent != null)
			{
				registrationInfoCollection.Add(registrationInfo.Parent);
				registrationInfo = registrationInfo.Parent;
			}
			return registrationInfoCollection;
		}
	}

	internal RegistrationInfo()
		: this(string.Empty, string.Empty, Guid.Empty)
	{
	}

	internal RegistrationInfo(string name, string desc, Guid st)
	{
		FriendlyName = name;
		Description = desc;
		ServerType = st;
		parent = null;
	}

	internal static RegistrationInfo LoadNodeFromXml(XmlReader reader)
	{
		while (reader.NodeType != XmlNodeType.Element && reader.Read())
		{
		}
		string text = reader.ReadOuterXml();
		if (text == string.Empty)
		{
			return null;
		}
		XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(text))
		{
			DtdProcessing = DtdProcessing.Prohibit,
			WhitespaceHandling = WhitespaceHandling.None
		};
		while (xmlTextReader.Read())
		{
			if (xmlTextReader.NodeType == XmlNodeType.Element)
			{
				RegistrationInfo result = null;
				switch (xmlTextReader.LocalName)
				{
				case "ServerType":
					result = LoadServerTypeFromXml(xmlTextReader);
					break;
				case "Server":
					result = LoadServerFromXml(xmlTextReader);
					break;
				case "Group":
					result = LoadGroupFromXml(xmlTextReader);
					break;
				}
				return result;
			}
		}
		return null;
	}

	internal abstract void SaveNodeToXml(XmlWriter writer, bool saveName);

	private static ServerTypeRegistrationInfo LoadServerTypeFromXml(XmlReader reader)
	{
		XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(reader.ReadOuterXml()))
		{
			DtdProcessing = DtdProcessing.Prohibit
		};
		xmlTextReader.Read();
		ServerTypeRegistrationInfo serverTypeRegistrationInfo = new ServerTypeRegistrationInfo();
		bool isEmptyElement = xmlTextReader.IsEmptyElement;
		xmlTextReader.MoveToFirstAttribute();
		serverTypeRegistrationInfo.ServerType = new Guid(xmlTextReader.Value);
		xmlTextReader.MoveToNextAttribute();
		serverTypeRegistrationInfo.FriendlyName = xmlTextReader.Value;
		if (!isEmptyElement)
		{
			RegistrationInfo registrationInfo = null;
			while ((registrationInfo = LoadNodeFromXml(xmlTextReader)) != null)
			{
				serverTypeRegistrationInfo.AddChild(registrationInfo);
			}
		}
		while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
		{
		}
		return serverTypeRegistrationInfo;
	}

	private static GroupRegistrationInfo LoadGroupFromXml(XmlReader reader)
	{
		XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(reader.ReadOuterXml()))
		{
			DtdProcessing = DtdProcessing.Prohibit,
			WhitespaceHandling = WhitespaceHandling.None
		};
		xmlTextReader.Read();
		GroupRegistrationInfo groupRegistrationInfo = new GroupRegistrationInfo();
		bool isEmptyElement = xmlTextReader.IsEmptyElement;
		xmlTextReader.MoveToFirstAttribute();
		groupRegistrationInfo.FriendlyName = xmlTextReader.Value;
		xmlTextReader.MoveToNextAttribute();
		groupRegistrationInfo.Description = xmlTextReader.Value;
		if (!isEmptyElement)
		{
			RegistrationInfo registrationInfo = null;
			while ((registrationInfo = LoadNodeFromXml(xmlTextReader)) != null)
			{
				if (groupRegistrationInfo.Children[registrationInfo.FriendlyName] != null)
				{
					registrationInfo.FriendlyName = groupRegistrationInfo.GenerateUniqueName(registrationInfo.FriendlyName);
				}
				groupRegistrationInfo.AddChild(registrationInfo);
			}
		}
		return groupRegistrationInfo;
	}

	private static ServerInstanceRegistrationInfo LoadServerFromXml(XmlReader reader)
	{
		XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(reader.ReadOuterXml()))
		{
			DtdProcessing = DtdProcessing.Prohibit,
			WhitespaceHandling = WhitespaceHandling.None
		};
		xmlTextReader.Read();
		ServerInstanceRegistrationInfo serverInstanceRegistrationInfo = new ServerInstanceRegistrationInfo();
		xmlTextReader.MoveToFirstAttribute();
		serverInstanceRegistrationInfo.FriendlyName = xmlTextReader.Value;
		xmlTextReader.MoveToNextAttribute();
		serverInstanceRegistrationInfo.Description = xmlTextReader.Value;
		while (xmlTextReader.LocalName != "ConnectionInformation" && xmlTextReader.Read())
		{
		}
		serverInstanceRegistrationInfo.ConnectionInfo = UIConnectionInfo.LoadFromStream(xmlTextReader);
		serverInstanceRegistrationInfo.ServerType = serverInstanceRegistrationInfo.ConnectionInfo.ServerType;
		SecureString sqlRegSvrCredential = WindowsCredential.GetSqlRegSvrCredential(serverInstanceRegistrationInfo.ConnectionInfo.ServerName, serverInstanceRegistrationInfo.ConnectionInfo.AuthenticationType, serverInstanceRegistrationInfo.ConnectionInfo.UserName, serverInstanceRegistrationInfo.ServerType, RegistrationProvider.ProviderStoreCredentialVersion);
		if (sqlRegSvrCredential != null)
		{
			serverInstanceRegistrationInfo.ConnectionInfo.InMemoryPassword = sqlRegSvrCredential;
		}
		return serverInstanceRegistrationInfo;
	}

	public int CompareTo(object obj)
	{
		if (obj is RegistrationInfo)
		{
			string friendlyName = (obj as RegistrationInfo).FriendlyName;
			return string.Compare(fName, friendlyName, StringComparison.CurrentCultureIgnoreCase);
		}
		throw new ArgumentException("Object is not of type RegistrationInfo");
	}
}
