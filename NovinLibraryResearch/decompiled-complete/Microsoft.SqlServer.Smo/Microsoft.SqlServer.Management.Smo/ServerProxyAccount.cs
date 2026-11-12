using System;
using System.Collections.Specialized;
using System.Security;
using System.Text;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;
using Microsoft.SqlServer.Management.Smo.Internal;

namespace Microsoft.SqlServer.Management.Smo;

[SfcElement(SfcElementFlags.Standalone)]
public sealed class ServerProxyAccount : SqlSmoObject, IAlterable
{
	internal class PropertyMetadataProvider : SqlPropertyMetadataProvider
	{
		private static int[] versionCount = new int[10] { 0, 2, 2, 2, 2, 2, 2, 2, 2, 2 };

		private static int[] cloudVersionCount = new int[3];

		private static int sqlDwPropertyCount = 0;

		internal static StaticMetadata[] sqlDwStaticMetadata = new StaticMetadata[0];

		internal static StaticMetadata[] cloudStaticMetadata = new StaticMetadata[0];

		internal static StaticMetadata[] staticMetadata = new StaticMetadata[2]
		{
			new StaticMetadata("IsEnabled", expensive: false, readOnly: false, typeof(bool)),
			new StaticMetadata("WindowsAccount", expensive: false, readOnly: false, typeof(string))
		};

		public override int Count
		{
			get
			{
				if (base.DatabaseEngineType == DatabaseEngineType.SqlAzureDatabase)
				{
					if (base.DatabaseEngineEdition == DatabaseEngineEdition.SqlDataWarehouse)
					{
						return sqlDwPropertyCount;
					}
					int num = ((currentVersionIndex < cloudVersionCount.Length) ? currentVersionIndex : (cloudVersionCount.Length - 1));
					return cloudVersionCount[num];
				}
				int num2 = ((currentVersionIndex < versionCount.Length) ? currentVersionIndex : (versionCount.Length - 1));
				return versionCount[num2];
			}
		}

		protected override int[] VersionCount
		{
			get
			{
				if (base.DatabaseEngineType == DatabaseEngineType.SqlAzureDatabase)
				{
					if (base.DatabaseEngineEdition == DatabaseEngineEdition.SqlDataWarehouse)
					{
						return new int[1] { sqlDwPropertyCount };
					}
					return cloudVersionCount;
				}
				return versionCount;
			}
		}

		internal PropertyMetadataProvider(ServerVersion version, DatabaseEngineType databaseEngineType, DatabaseEngineEdition databaseEngineEdition)
			: base(version, databaseEngineType, databaseEngineEdition)
		{
		}

		public override int PropertyNameToIDLookup(string propertyName)
		{
			if (base.DatabaseEngineType == DatabaseEngineType.SqlAzureDatabase)
			{
				_ = base.DatabaseEngineEdition;
				_ = 6;
				return -1;
			}
			if (!(propertyName == "IsEnabled"))
			{
				if (propertyName == "WindowsAccount")
				{
					return 1;
				}
				return -1;
			}
			return 0;
		}

		internal new static int[] GetVersionArray(DatabaseEngineType databaseEngineType, DatabaseEngineEdition databaseEngineEdition)
		{
			if (databaseEngineType == DatabaseEngineType.SqlAzureDatabase)
			{
				if (databaseEngineEdition == DatabaseEngineEdition.SqlDataWarehouse)
				{
					return new int[1] { sqlDwPropertyCount };
				}
				return cloudVersionCount;
			}
			return versionCount;
		}

		public override StaticMetadata GetStaticMetadata(int id)
		{
			if (base.DatabaseEngineType == DatabaseEngineType.SqlAzureDatabase)
			{
				if (base.DatabaseEngineEdition == DatabaseEngineEdition.SqlDataWarehouse)
				{
					return sqlDwStaticMetadata[id];
				}
				return cloudStaticMetadata[id];
			}
			return staticMetadata[id];
		}

		internal new static StaticMetadata[] GetStaticMetadataArray(DatabaseEngineType databaseEngineType, DatabaseEngineEdition databaseEngineEdition)
		{
			if (databaseEngineType == DatabaseEngineType.SqlAzureDatabase)
			{
				if (databaseEngineEdition == DatabaseEngineEdition.SqlDataWarehouse)
				{
					return sqlDwStaticMetadata;
				}
				return cloudStaticMetadata;
			}
			return staticMetadata;
		}
	}

	private SqlSecureString m_password;

	[SfcObject(SfcObjectRelationship.ParentObject)]
	public Server Parent
	{
		get
		{
			CheckObjectState();
			return singletonParent as Server;
		}
	}

	public static string UrnSuffix => "ServerProxyAccount";

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public bool IsEnabled
	{
		get
		{
			return (bool)base.Properties.GetValueWithNullReplacement("IsEnabled");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("IsEnabled", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public string WindowsAccount
	{
		get
		{
			return (string)base.Properties.GetValueWithNullReplacement("WindowsAccount");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("WindowsAccount", value);
		}
	}

	internal ServerProxyAccount()
	{
	}

	internal ServerProxyAccount(Server parent, ObjectKeyBase key, SqlSmoState state)
		: base(key, state)
	{
		singletonParent = parent;
		SetServerObject(parent.GetServerObject());
	}

	public void SetPassword(string password)
	{
		if (password == null)
		{
			throw new FailedOperationException(ExceptionTemplatesImpl.SetPassword, this, new ArgumentNullException("password"));
		}
		SetPassword(new SqlSecureString(password));
	}

	public void SetPassword(SecureString password)
	{
		if (password == null)
		{
			throw new FailedOperationException(ExceptionTemplatesImpl.SetPassword, this, new ArgumentNullException("password"));
		}
		m_password = password;
	}

	public void SetAccount(string windowsAccount, string password)
	{
		if (windowsAccount == null)
		{
			throw new FailedOperationException(ExceptionTemplatesImpl.SetHostLoginAccount, this, new ArgumentNullException("windowsAccount"));
		}
		if (password == null)
		{
			throw new FailedOperationException(ExceptionTemplatesImpl.SetHostLoginAccount, this, new ArgumentNullException("password"));
		}
		SetAccount(windowsAccount, new SqlSecureString(password));
	}

	public void SetAccount(string windowsAccount, SecureString password)
	{
		if (windowsAccount == null)
		{
			throw new FailedOperationException(ExceptionTemplatesImpl.SetHostLoginAccount, this, new ArgumentNullException("windowsAccount"));
		}
		if (password == null)
		{
			throw new FailedOperationException(ExceptionTemplatesImpl.SetHostLoginAccount, this, new ArgumentNullException("password"));
		}
		base.Properties.Get("WindowsAccount").Value = windowsAccount;
		SetPassword(password);
	}

	internal static void ParseAccountName(string accountName, StringBuilder domainName, StringBuilder userName)
	{
		string[] array = accountName.Split(new char[1] { '\\' });
		if (array.Length == 2)
		{
			domainName.Append(array[0]);
			userName.Append(array[1]);
			return;
		}
		if (array.Length == 1)
		{
			userName.Append(array[0]);
			return;
		}
		throw new SmoException(ExceptionTemplatesImpl.InvalidAcctName);
	}

	internal override void ScriptAlter(StringCollection query, ScriptingPreferences sp)
	{
		ThrowIfAboveVersion80();
		StringBuilder stringBuilder = new StringBuilder(Globals.INIT_BUFFER_SIZE);
		StringBuilder stringBuilder2 = new StringBuilder();
		StringBuilder stringBuilder3 = new StringBuilder();
		if ((bool)GetPropValue("IsEnabled"))
		{
			ParseAccountName(base.Properties["WindowsAccount"].Value as string, stringBuilder2, stringBuilder3);
			stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "EXEC master.dbo.xp_sqlagent_proxy_account N'SET', N'{0}', N'{1}', N'{2}'", SqlSmoObject.SqlString(stringBuilder2.ToString()), SqlSmoObject.SqlString(stringBuilder3.ToString()), SqlSmoObject.SqlString((string)m_password));
			stringBuilder.Append(Globals.newline);
			stringBuilder.Append("EXEC msdb.dbo.sp_set_sqlagent_properties @sysadmin_only=0");
		}
		else
		{
			stringBuilder.Append("EXEC msdb.dbo.sp_set_sqlagent_properties @sysadmin_only=1");
		}
		query.Add(stringBuilder.ToString());
	}

	public void Alter()
	{
		if (base.ServerVersion.Major <= 8)
		{
			AlterImpl();
			return;
		}
		Credential credential = Parent.Credentials["##xp_cmdshell_proxy_account##"];
		if ((bool)GetPropValue("IsEnabled"))
		{
			string identity = (string)base.Properties["WindowsAccount"].Value;
			if (credential != null)
			{
				credential.Alter(identity, m_password);
			}
			else
			{
				new Credential(Parent, "##xp_cmdshell_proxy_account##").Create(identity, m_password);
			}
		}
		else
		{
			credential?.Drop();
		}
	}

	protected sealed override void GetUrnRecursive(StringBuilder urnbuilder, UrnIdOption idOption)
	{
		Parent.GetUrnRecImpl(urnbuilder, idOption);
		urnbuilder.AppendFormat(SmoApplication.DefaultCulture, "/{0}", UrnSuffix);
	}

	internal override SqlPropertyMetadataProvider GetPropertyMetadataProvider()
	{
		return new PropertyMetadataProvider(base.ServerVersion, DatabaseEngineType, DatabaseEngineEdition);
	}
}
