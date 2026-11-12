using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Security;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Xml;
using Microsoft.SqlServer.Management.Common;

namespace Microsoft.SqlServer.Management.Smo.RegSvrEnum;

public class UIConnectionInfo : IComparable<UIConnectionInfo>
{
	private const string XmlStart = "ConnectionInformation";

	private const string XmlServerType = "ServerType";

	private const string XmlServerName = "ServerName";

	private const string XmlDisplayName = "DisplayName";

	private const string XmlUserName = "UserName";

	private const string XmlPassword = "Password";

	private const string XmlAuthenticationType = "AuthenticationType";

	private const string XmlAdvancedOptions = "AdvancedOptions";

	private const string XmlItemTypeAttribute = "type";

	private NameValueCollection advancedOptions;

	private Guid serverType = Guid.Empty;

	private int authType = int.MinValue;

	private string serverName;

	private string displayName;

	private string userName;

	private string password;

	[NonSerialized]
	private SecureString inMemoryPassword;

	private string appName;

	private bool savepwd;

	private ServerVersion serverVer;

	private string otherParams;

	private long id;

	private static long nextId;

	public IRenewableToken RenewableToken { get; set; }

	public bool PersistPassword
	{
		get
		{
			return savepwd;
		}
		set
		{
			savepwd = value;
		}
	}

	public Guid ServerType
	{
		get
		{
			return serverType;
		}
		set
		{
			serverType = value;
		}
	}

	public string ApplicationName
	{
		get
		{
			if (appName != null)
			{
				return appName;
			}
			return string.Empty;
		}
		set
		{
			appName = value;
		}
	}

	public string ServerName
	{
		get
		{
			return serverName;
		}
		set
		{
			serverName = value;
		}
	}

	public string DisplayName
	{
		get
		{
			if (displayName == null)
			{
				return ServerNameNoDot;
			}
			return displayName;
		}
		set
		{
			displayName = value;
		}
	}

	public string ServerNameNoDot
	{
		get
		{
			string text = ServerName;
			if (text != null)
			{
				text = text.Trim();
			}
			if (text.Length == 0 || text == ".")
			{
				return "(local)";
			}
			if (text.StartsWith(".\\", StringComparison.Ordinal))
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}{1}", "(local)", text.Substring(1));
			}
			return text;
		}
	}

	public virtual string UserName
	{
		get
		{
			return userName ?? string.Empty;
		}
		set
		{
			userName = value;
		}
	}

	public string Password
	{
		get
		{
			if (inMemoryPassword != null)
			{
				return inMemoryPassword.SecureStringToString();
			}
			return password;
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				password = string.Empty;
				inMemoryPassword = string.Empty.StringToSecureString();
			}
			else
			{
				inMemoryPassword = value.StringToSecureString();
				password = null;
			}
		}
	}

	public SecureString InMemoryPassword
	{
		get
		{
			return inMemoryPassword;
		}
		set
		{
			inMemoryPassword = value;
			password = null;
		}
	}

	[Obsolete]
	public string EncryptedPassword
	{
		get
		{
			return password;
		}
		set
		{
			password = value;
		}
	}

	public int AuthenticationType
	{
		get
		{
			return authType;
		}
		set
		{
			authType = value;
		}
	}

	public NameValueCollection AdvancedOptions => advancedOptions;

	public ServerVersion ServerVersion
	{
		get
		{
			return serverVer;
		}
		set
		{
			serverVer = value;
		}
	}

	public long Id => id;

	public string OtherParams
	{
		get
		{
			return otherParams;
		}
		set
		{
			otherParams = value;
		}
	}

	public UIConnectionInfo()
	{
		advancedOptions = new NameValueCollection();
		id = NewId();
	}

	public UIConnectionInfo(UIConnectionInfo lhs, bool generateNewId)
		: this(lhs)
	{
		if (generateNewId)
		{
			id = NewId();
		}
	}

	public UIConnectionInfo(UIConnectionInfo lhs)
	{
		serverType = lhs.serverType;
		AuthenticationType = lhs.AuthenticationType;
		appName = lhs.appName;
		id = lhs.id;
		serverName = lhs.serverName;
		displayName = lhs.displayName;
		userName = lhs.userName;
		inMemoryPassword = lhs.inMemoryPassword;
		password = lhs.password;
		savepwd = lhs.savepwd;
		otherParams = lhs.otherParams;
		RenewableToken = lhs.RenewableToken;
		if (lhs.advancedOptions != null)
		{
			advancedOptions = new NameValueCollection(lhs.advancedOptions.Count);
			{
				foreach (string key in lhs.advancedOptions.Keys)
				{
					advancedOptions[key] = lhs.AdvancedOptions[key];
				}
				return;
			}
		}
		advancedOptions = null;
	}

	static UIConnectionInfo()
	{
		nextId = 0L;
	}

	public void SaveToStream(XmlWriter writer, bool saveName)
	{
		writer.WriteStartElement("ConnectionInformation");
		writer.WriteElementString("ServerType", null, ServerType.ToString());
		writer.WriteElementString("ServerName", null, ServerName.ToString());
		writer.WriteElementString("DisplayName", null, DisplayName);
		writer.WriteElementString("AuthenticationType", null, AuthenticationType.ToString(CultureInfo.InvariantCulture));
		string empty = string.Empty;
		if (saveName && UserName.Length > 0 && UserName != ((ClaimsIdentity)(object)WindowsIdentity.GetCurrent()).Name)
		{
			empty = UserName;
		}
		writer.WriteElementString("UserName", null, empty);
		writer.WriteStartElement("AdvancedOptions", null);
		foreach (string key in AdvancedOptions.Keys)
		{
			writer.WriteElementString(key, null, AdvancedOptions[key]);
		}
		writer.WriteEndElement();
		writer.WriteEndElement();
	}

	public static UIConnectionInfo LoadFromStream(XmlReader reader)
	{
		UIConnectionInfo uIConnectionInfo = new UIConnectionInfo();
		XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(reader.ReadOuterXml()))
		{
			DtdProcessing = DtdProcessing.Prohibit
		};
		while (xmlTextReader.Read())
		{
			if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.LocalName == "ServerType")
			{
				uIConnectionInfo.ServerType = new Guid(xmlTextReader.ReadString());
				break;
			}
		}
		while (xmlTextReader.Read())
		{
			if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.LocalName == "ServerName")
			{
				uIConnectionInfo.ServerName = xmlTextReader.ReadString();
				break;
			}
		}
		while (xmlTextReader.Read())
		{
			if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.LocalName == "DisplayName")
			{
				uIConnectionInfo.DisplayName = xmlTextReader.ReadString();
				break;
			}
		}
		while (xmlTextReader.Read())
		{
			if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.LocalName == "AuthenticationType")
			{
				uIConnectionInfo.AuthenticationType = Convert.ToInt32(xmlTextReader.ReadString(), CultureInfo.InvariantCulture);
				break;
			}
		}
		while (xmlTextReader.Read())
		{
			if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.LocalName == "UserName")
			{
				uIConnectionInfo.UserName = xmlTextReader.ReadString();
				break;
			}
		}
		bool flag = false;
		while (xmlTextReader.Read())
		{
			if (xmlTextReader.NodeType == XmlNodeType.Element && (xmlTextReader.LocalName == "Password" || xmlTextReader.LocalName == "AdvancedOptions"))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			if (xmlTextReader.LocalName == "Password")
			{
				string data = xmlTextReader.ReadString();
				uIConnectionInfo.Password = DataProtection.UnprotectData(data);
				while (xmlTextReader.Read() && (xmlTextReader.NodeType != XmlNodeType.Element || !(xmlTextReader.LocalName == "AdvancedOptions")))
				{
				}
			}
			if (!xmlTextReader.IsEmptyElement)
			{
				while (xmlTextReader.Read())
				{
					if (xmlTextReader.NodeType == XmlNodeType.Element)
					{
						uIConnectionInfo.AdvancedOptions.Set(xmlTextReader.LocalName, xmlTextReader.ReadString());
					}
				}
			}
		}
		return uIConnectionInfo;
	}

	public UIConnectionInfo Copy()
	{
		return new UIConnectionInfo(this, generateNewId: true);
	}

	private static long NewId()
	{
		return Interlocked.Add(ref nextId, 1L);
	}

	public int CompareTo(UIConnectionInfo other)
	{
		int num = string.CompareOrdinal(ServerType.ToString(), other.ServerType.ToString());
		if (num == 0)
		{
			num = string.CompareOrdinal(DisplayName, other.DisplayName);
		}
		if (num == 0)
		{
			num = string.CompareOrdinal(ServerName, other.ServerName);
		}
		if (num == 0)
		{
			num = string.CompareOrdinal(UserName, other.UserName);
		}
		if (num == 0 && RenewableToken != null && other.RenewableToken != null)
		{
			num = string.CompareOrdinal(RenewableToken.Resource, other.RenewableToken.Resource);
			if (num == 0)
			{
				num = string.CompareOrdinal(RenewableToken.UserId, other.RenewableToken.UserId);
			}
			if (num == 0)
			{
				num = string.CompareOrdinal(RenewableToken.Tenant, other.RenewableToken.Tenant);
			}
		}
		if (num == 0)
		{
			if (Id < other.Id)
			{
				num = -1;
			}
			else if (other.Id < Id)
			{
				num = 1;
			}
		}
		return num;
	}

	public override bool Equals(object obj)
	{
		bool result = false;
		UIConnectionInfo uIConnectionInfo = obj as UIConnectionInfo;
		if (uIConnectionInfo != null)
		{
			result = Id == uIConnectionInfo.Id;
		}
		return result;
	}

	public override int GetHashCode()
	{
		return (int)(Id % int.MaxValue);
	}

	public static bool operator ==(UIConnectionInfo infoA, UIConnectionInfo infoB)
	{
		bool result = false;
		bool flag = (object)infoA == null;
		bool flag2 = (object)infoB == null;
		if (flag && flag2)
		{
			result = true;
		}
		else if (!flag && !flag2)
		{
			result = infoA.Equals(infoB);
		}
		return result;
	}

	public static bool operator ==(UIConnectionInfo infoA, object infoB)
	{
		bool result = false;
		bool flag = (object)infoA == null;
		bool flag2 = infoB == null;
		if (flag && flag2)
		{
			result = true;
		}
		else if (!flag && !flag2)
		{
			result = infoA.Equals(infoB);
		}
		return result;
	}

	public static bool operator ==(object infoA, UIConnectionInfo infoB)
	{
		bool result = false;
		bool flag = infoA == null;
		bool flag2 = (object)infoB == null;
		if (flag && flag2)
		{
			result = true;
		}
		else if (!flag && !flag2)
		{
			result = infoB.Equals(infoA);
		}
		return result;
	}

	public static bool operator !=(UIConnectionInfo infoA, UIConnectionInfo infoB)
	{
		return !(infoA == infoB);
	}

	public static bool operator !=(UIConnectionInfo infoA, object infoB)
	{
		return !(infoA == infoB);
	}

	public static bool operator !=(object infoA, UIConnectionInfo infoB)
	{
		return !(infoA == infoB);
	}
}
