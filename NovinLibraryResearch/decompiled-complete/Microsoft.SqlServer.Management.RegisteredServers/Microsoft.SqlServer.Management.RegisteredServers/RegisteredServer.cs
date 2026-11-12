#define TRACE
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.RegisteredServers;

public sealed class RegisteredServer : SfcInstance, ISfcValidate, ISfcCreatable, ICreatable, ISfcAlterable, IAlterable, ISfcDroppable, IDroppable, ISfcRenamable, IRenamable, ISfcMovable
{
	public sealed class Key : SfcKey
	{
		private string keyName;

		public string Name => keyName;

		public Key()
		{
		}

		public Key(Key other)
		{
			keyName = other.Name;
		}

		public Key(string name)
		{
			keyName = name;
		}

		internal Key(Dictionary<string, object> filedDict)
		{
			keyName = (string)filedDict["Name"];
		}

		public override bool Equals(object obj)
		{
			return this == obj;
		}

		public new static bool Equals(object obj1, object obj2)
		{
			return obj1 as Key == obj2 as Key;
		}

		public override bool Equals(SfcKey key)
		{
			return this == key;
		}

		public static bool operator ==(object obj, Key rightOperand)
		{
			if (obj == null || obj is Key)
			{
				return (Key)obj == rightOperand;
			}
			return false;
		}

		public static bool operator ==(Key leftOperand, object obj)
		{
			if (obj == null || obj is Key)
			{
				return leftOperand == (Key)obj;
			}
			return false;
		}

		public static bool operator ==(Key leftOperand, Key rightOperand)
		{
			if ((object)leftOperand == (object)rightOperand)
			{
				return true;
			}
			if ((object)leftOperand == null || (object)rightOperand == null)
			{
				return false;
			}
			return leftOperand.IsEqual(rightOperand);
		}

		public static bool operator !=(object obj, Key rightOperand)
		{
			return !(obj == rightOperand);
		}

		public static bool operator !=(Key leftOperand, object obj)
		{
			return !(leftOperand == obj);
		}

		public static bool operator !=(Key leftOperand, Key rightOperand)
		{
			return !(leftOperand == rightOperand);
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}

		private bool IsEqual(Key key)
		{
			return string.CompareOrdinal(Name, key.Name) == 0;
		}

		public override string GetUrnFragment()
		{
			return string.Format("{0}[@Name='{1}']", "RegisteredServer", SfcSecureString.EscapeSquote(Name));
		}
	}

	private sealed class ObjectFactory : SfcObjectFactory
	{
		private static readonly ObjectFactory instance;

		public static ObjectFactory Instance => instance;

		static ObjectFactory()
		{
			instance = new ObjectFactory();
		}

		private ObjectFactory()
		{
		}

		protected override SfcInstance CreateImpl()
		{
			return new RegisteredServer();
		}
	}

	private static readonly SfcTsqlProcFormatter scriptCreateAction;

	private static readonly SfcTsqlProcFormatter scriptAlterAction;

	private static readonly SfcTsqlProcFormatter scriptDropAction;

	private static readonly SfcTsqlProcFormatter scriptRenameAction;

	private static readonly SfcTsqlProcFormatter scriptMoveAction;

	private SecureString secureConnectionString;

	internal const string typeName = "RegisteredServer";

	[SfcIgnore]
	public SecureString SecureConnectionString
	{
		get
		{
			if (secureConnectionString == null)
			{
				DbConnectionStringBuilder dbConnectionStringBuilder = new DbConnectionStringBuilder();
				if (!string.IsNullOrEmpty(ConnectionStringWithEncryptedPassword))
				{
					dbConnectionStringBuilder.ConnectionString = ConnectionStringWithEncryptedPassword;
					if (dbConnectionStringBuilder.ContainsKey("password") && dbConnectionStringBuilder["password"] is string input)
					{
						dbConnectionStringBuilder["password"] = ProtectData(input, encrypt: false);
					}
					secureConnectionString = EncryptionUtility.EncryptString(dbConnectionStringBuilder.ConnectionString);
				}
				else if (Parent != null && !Parent.IsLocal)
				{
					dbConnectionStringBuilder["server"] = base.Properties["ServerName"].Value as string;
					dbConnectionStringBuilder["integrated security"] = "true";
					secureConnectionString = EncryptionUtility.EncryptString(dbConnectionStringBuilder.ConnectionString);
				}
			}
			return secureConnectionString;
		}
		set
		{
			secureConnectionString = value;
			UpdateConnectionStringWithEncryptedPassword();
		}
	}

	[SfcIgnore]
	public string ConnectionString
	{
		get
		{
			if (SecureConnectionString != null)
			{
				return EncryptionUtility.DecryptSecureString(SecureConnectionString);
			}
			return string.Empty;
		}
		set
		{
			if (!string.IsNullOrEmpty(value))
			{
				string serverNameFromConnectionString = GetServerNameFromConnectionString(value);
				if (string.IsNullOrEmpty(serverNameFromConnectionString))
				{
					if (!string.IsNullOrEmpty(ServerName))
					{
						new DbConnectionStringBuilder().ConnectionString = value;
						SetServerNameInConnectionString(ServerName);
					}
				}
				else
				{
					if (!serverNameFromConnectionString.Equals((string)base.Properties["ServerName"].Value, StringComparison.OrdinalIgnoreCase))
					{
						base.Properties["ServerName"].Value = serverNameFromConnectionString;
					}
					SecureConnectionString = EncryptionUtility.EncryptString(value);
				}
			}
			else
			{
				SecureConnectionString = null;
			}
		}
	}

	[SfcObject(SfcObjectRelationship.ParentObject, SfcObjectCardinality.One)]
	public new ServerGroup Parent
	{
		get
		{
			return (ServerGroup)base.Parent;
		}
		set
		{
			base.Parent = value;
		}
	}

	[SfcIgnore]
	public Key IdentityKey => (Key)base.AbstractIdentityKey;

	[SfcProperty(SfcPropertyFlags.Required | SfcPropertyFlags.ReadOnlyAfterCreation)]
	[SfcKey(0)]
	public string Name => (string)base.Properties["Name"].Value;

	[SfcProperty(SfcPropertyFlags.Data)]
	public int ID => (int)base.Properties["ID"].Value;

	[SfcProperty(SfcPropertyFlags.None)]
	public string Description
	{
		get
		{
			return (string)base.Properties["Description"].Value;
		}
		set
		{
			base.Properties["Description"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.Required)]
	public string ServerName
	{
		get
		{
			string serverNameFromConnectionString = GetServerNameFromConnectionString(ConnectionString);
			if (!string.IsNullOrEmpty(serverNameFromConnectionString))
			{
				return serverNameFromConnectionString;
			}
			return (string)base.Properties["ServerName"].Value;
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentException(RegSvrStrings.ArgumentNullOrEmpty("ServerName"));
			}
			if (!GetServerNameFromConnectionString(ConnectionString).Equals(value, StringComparison.OrdinalIgnoreCase))
			{
				SetServerNameInConnectionString(value);
			}
			base.Properties["ServerName"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.None)]
	public bool UseCustomConnectionColor
	{
		get
		{
			SfcProperty sfcProperty = base.Properties["UseCustomConnectionColor"];
			if (sfcProperty.Value != null)
			{
				return (bool)sfcProperty.Value;
			}
			return false;
		}
		set
		{
			base.Properties["UseCustomConnectionColor"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.None)]
	public int CustomConnectionColorArgb
	{
		get
		{
			SfcProperty sfcProperty = base.Properties["CustomConnectionColorArgb"];
			if (sfcProperty.Value != null)
			{
				return (int)sfcProperty.Value;
			}
			return 0;
		}
		set
		{
			base.Properties["CustomConnectionColorArgb"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.None)]
	public ServerType ServerType
	{
		get
		{
			SfcProperty sfcProperty = base.Properties["ServerType"];
			if (sfcProperty.Value == null)
			{
				sfcProperty.Value = Parent.ServerType;
			}
			return (ServerType)sfcProperty.Value;
		}
	}

	[SfcProperty(SfcPropertyFlags.None)]
	public string ConnectionStringWithEncryptedPassword
	{
		get
		{
			return (string)base.Properties["ConnectionStringWithEncryptedPassword"].Value;
		}
		internal set
		{
			base.Properties["ConnectionStringWithEncryptedPassword"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.Required)]
	public CredentialPersistenceType CredentialPersistenceType
	{
		get
		{
			SfcProperty sfcProperty = base.Properties["CredentialPersistenceType"];
			Trace.Assert(null != sfcProperty);
			if (sfcProperty.Value == null && base.State == SfcObjectState.Pending)
			{
				sfcProperty.Value = CredentialPersistenceType.None;
			}
			return (CredentialPersistenceType)sfcProperty.Value;
		}
		set
		{
			base.Properties["CredentialPersistenceType"].Value = value;
			UpdateConnectionStringWithEncryptedPassword();
		}
	}

	[SfcProperty(SfcPropertyFlags.None)]
	public string OtherParams
	{
		get
		{
			SfcProperty sfcProperty = base.Properties["OtherParams"];
			if (sfcProperty.Value != null)
			{
				return sfcProperty.Value.ToString();
			}
			return string.Empty;
		}
		set
		{
			base.Properties["OtherParams"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.None)]
	public int AuthenticationType
	{
		get
		{
			return Convert.ToInt32(base.Properties["AuthenticationType"].Value ?? ((object)int.MinValue));
		}
		set
		{
			base.Properties["AuthenticationType"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.None)]
	public string ActiveDirectoryUserId
	{
		get
		{
			return Convert.ToString(base.Properties["ActiveDirectoryUserId"].Value ?? string.Empty);
		}
		set
		{
			base.Properties["ActiveDirectoryUserId"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.None)]
	public string ActiveDirectoryTenant
	{
		get
		{
			return Convert.ToString(base.Properties["ActiveDirectoryTenant"].Value ?? string.Empty);
		}
		set
		{
			base.Properties["ActiveDirectoryTenant"].Value = value;
		}
	}

	[SfcIgnore]
	public bool IsLocal
	{
		get
		{
			ISfcDomain domain = base.KeyChain.RootKey.Domain;
			Trace.Assert(domain is RegisteredServersStore);
			return ((RegisteredServersStore)domain).IsLocal;
		}
	}

	[SfcIgnore]
	public bool IsDropped => base.State == SfcObjectState.Dropped;

	static RegisteredServer()
	{
		scriptCreateAction = new SfcTsqlProcFormatter();
		scriptAlterAction = new SfcTsqlProcFormatter();
		scriptDropAction = new SfcTsqlProcFormatter();
		scriptRenameAction = new SfcTsqlProcFormatter();
		scriptMoveAction = new SfcTsqlProcFormatter();
		scriptCreateAction.Procedure = "msdb.dbo.sp_sysmanagement_add_shared_registered_server";
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("server_group_id", required: true));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("name", "Name", required: true, output: false));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("server_name", "ServerName", required: true, output: false));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("description", "Description", required: false, output: false));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("server_type", required: true));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("server_id", "ID", required: false, output: true));
		scriptAlterAction.Procedure = "msdb.dbo.sp_sysmanagement_update_shared_registered_server";
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("server_id", "ID", required: true, output: false));
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("server_name", "ServerName", required: false, output: false));
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("description", "Description", required: false, output: false));
		scriptDropAction.Procedure = "msdb.dbo.sp_sysmanagement_delete_shared_registered_server";
		scriptDropAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("server_id", "ID", required: true, output: false));
		scriptRenameAction.Procedure = "msdb.dbo.sp_sysmanagement_rename_shared_registered_server";
		scriptRenameAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("server_id", "ID", required: true, output: false));
		scriptRenameAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("new_name", required: true));
		scriptMoveAction.Procedure = "msdb.dbo.sp_sysmanagement_move_shared_registered_server";
		scriptMoveAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("server_id", "ID", required: true, output: false));
		scriptMoveAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("new_parent_id", required: true));
	}

	public RegisteredServer()
	{
		SetDefaultCredentialPersistenceType();
	}

	public RegisteredServer(string name)
	{
		SetName(name);
		SetDefaultCredentialPersistenceType();
	}

	public RegisteredServer(ServerGroup parent, string name)
	{
		SetName(name);
		SetDefaultCredentialPersistenceType();
		Parent = parent;
	}

	private void UpdateConnectionStringWithEncryptedPassword()
	{
		if (base.Properties["CredentialPersistenceType"].Value == null)
		{
			throw new PropertyNotSetException("CredentialPersistenceType");
		}
		ConnectionStringWithEncryptedPassword = GetConnectionStringWithEncryptedPassword(CredentialPersistenceType);
	}

	internal string GetConnectionStringWithEncryptedPassword(CredentialPersistenceType cpt)
	{
		if (SecureConnectionString == null)
		{
			return null;
		}
		DbConnectionStringBuilder dbConnectionStringBuilder = new DbConnectionStringBuilder
		{
			ConnectionString = EncryptionUtility.DecryptSecureString(secureConnectionString)
		};
		switch (cpt)
		{
		case CredentialPersistenceType.None:
			dbConnectionStringBuilder["password"] = null;
			dbConnectionStringBuilder["user id"] = null;
			dbConnectionStringBuilder["uid"] = null;
			break;
		case CredentialPersistenceType.PersistLoginName:
			dbConnectionStringBuilder["password"] = null;
			break;
		case CredentialPersistenceType.PersistLoginNameAndPassword:
			if (dbConnectionStringBuilder.ContainsKey("password") && dbConnectionStringBuilder["password"] is string input)
			{
				dbConnectionStringBuilder["password"] = ProtectData(input, encrypt: true);
			}
			break;
		default:
			throw new RegisteredServerException(RegSvrStrings.UnknownEnumeration("CredentialPersistenceType"));
		}
		return dbConnectionStringBuilder.ConnectionString;
	}

	internal string ProtectData(string input, bool encrypt)
	{
		byte[] array = null;
		array = ((!encrypt) ? Convert.FromBase64String(input) : Encoding.Unicode.GetBytes(input));
		Trace.Assert(array != null);
		int i = 0;
		bool flag = false;
		byte[] array2 = null;
		for (; i < 10; i++)
		{
			if (flag)
			{
				break;
			}
			try
			{
				array2 = ((!encrypt) ? ProtectedData.Unprotect(array, (byte[])null, (DataProtectionScope)1) : ProtectedData.Protect(array, (byte[])null, (DataProtectionScope)1));
				flag = true;
			}
			catch (CryptographicException)
			{
				if (i == 9)
				{
					throw;
				}
			}
			catch (OutOfMemoryException)
			{
				if (i == 9)
				{
					throw;
				}
			}
		}
		if (array2 == null)
		{
			return string.Empty;
		}
		if (encrypt)
		{
			return Convert.ToBase64String(array2);
		}
		return Encoding.Unicode.GetString(array2);
	}

	private void SetDefaultCredentialPersistenceType()
	{
		CredentialPersistenceType = CredentialPersistenceType.None;
	}

	public ISfcConnection GetConnectionObject()
	{
		if (ServerType == ServerType.DatabaseEngine)
		{
			if (string.IsNullOrEmpty(ConnectionString))
			{
				return null;
			}
			return new ServerConnection(new SqlConnection(ConnectionString));
		}
		return null;
	}

	public static SfcObjectFactory GetObjectFactory()
	{
		return ObjectFactory.Instance;
	}

	protected internal override SfcKey CreateIdentityKey()
	{
		Key result = null;
		if (Name != null)
		{
			result = new Key(Name);
		}
		return result;
	}

	private void SetName(string name)
	{
		base.Properties["Name"].Value = name;
	}

	private string GetServerNameFromConnectionString(string connString)
	{
		DbConnectionStringBuilder dbConnectionStringBuilder = new DbConnectionStringBuilder();
		dbConnectionStringBuilder.ConnectionString = connString;
		string serverNameKeyInConnectionString = GetServerNameKeyInConnectionString(connString);
		if (!string.IsNullOrEmpty(serverNameKeyInConnectionString))
		{
			return (string)dbConnectionStringBuilder[serverNameKeyInConnectionString];
		}
		return string.Empty;
	}

	private void SetServerNameInConnectionString(string svrName)
	{
		if (string.IsNullOrEmpty(svrName))
		{
			return;
		}
		DbConnectionStringBuilder dbConnectionStringBuilder = new DbConnectionStringBuilder();
		dbConnectionStringBuilder.ConnectionString = ConnectionString;
		string serverNameKeyInConnectionString = GetServerNameKeyInConnectionString(ConnectionString);
		if (!string.IsNullOrEmpty(serverNameKeyInConnectionString))
		{
			dbConnectionStringBuilder[serverNameKeyInConnectionString] = svrName;
		}
		else
		{
			switch (ServerType)
			{
			case ServerType.IntegrationServices:
				dbConnectionStringBuilder.Add("server", svrName);
				break;
			case ServerType.ReportingServices:
				dbConnectionStringBuilder.Add("address", svrName);
				break;
			default:
				dbConnectionStringBuilder.Add("data source", svrName);
				break;
			}
		}
		SecureConnectionString = EncryptionUtility.EncryptString(dbConnectionStringBuilder.ConnectionString);
	}

	private string GetServerNameKeyInConnectionString(string connString)
	{
		if (!string.IsNullOrEmpty(connString))
		{
			foreach (string key in new DbConnectionStringBuilder
			{
				ConnectionString = connString
			}.Keys)
			{
				switch (key.ToLower())
				{
				case "server":
				case "data source":
				case "address":
					return key;
				}
			}
		}
		return string.Empty;
	}

	protected internal override ISfcCollection GetChildCollection(string elementType)
	{
		throw new RegisteredServerException(RegSvrStrings.NoSuchCollection(elementType));
	}

	public ValidationState Validate(string methodName, params object[] arguments)
	{
		ValidationState validationState = new ValidationState();
		Validate(methodName, throwOnFirst: false, validationState);
		return validationState;
	}

	internal void Validate(string validationMode, bool throwOnFirst, ValidationState validationState)
	{
		ValidateStringProperty(validationMode, throwOnFirst, validationState, "Name", Name);
		ValidateStringProperty(validationMode, throwOnFirst, validationState, "ServerName", ServerName);
	}

	private void ValidateStringProperty(string validationMode, bool throwOnFirst, ValidationState validationState, string propName, string propVal)
	{
		if (string.IsNullOrEmpty(propVal))
		{
			Exception ex = new SfcPropertyNotSetException(propName);
			if (throwOnFirst)
			{
				throw ex;
			}
			validationState.AddError(ex, propName);
		}
	}

	ISfcScript ISfcCreatable.ScriptCreate()
	{
		if (IsLocal)
		{
			return new SfcTSqlScript();
		}
		List<SfcTsqlProcFormatter.RuntimeArg> list = new List<SfcTsqlProcFormatter.RuntimeArg>();
		list.Add(new SfcTsqlProcFormatter.RuntimeArg(Parent.ID.GetType(), Parent.ID));
		list.Add(new SfcTsqlProcFormatter.RuntimeArg(Parent.ServerType.GetType(), Parent.ServerType));
		return new SfcTSqlScript(scriptCreateAction.GenerateScript(this, list));
	}

	public void Create()
	{
		Validate(ValidationMethod.Create);
		CreateImpl();
		if (IsLocal && GetStore().IsSerializeOnCreation)
		{
			GetStore().Serialize();
		}
	}

	protected override void PostCreate(object executionResult)
	{
		if (!IsLocal)
		{
			base.Properties["ID"].Value = executionResult;
		}
		base.Properties["ServerType"].Value = Parent.ServerType;
	}

	public void Alter()
	{
		Validate(ValidationMethod.Alter);
		AlterImpl();
		if (IsLocal)
		{
			GetStore().Serialize();
		}
	}

	ISfcScript ISfcAlterable.ScriptAlter()
	{
		if (IsLocal)
		{
			return new SfcTSqlScript();
		}
		return new SfcTSqlScript(scriptAlterAction.GenerateScript(this));
	}

	ISfcScript ISfcDroppable.ScriptDrop()
	{
		if (IsLocal)
		{
			return new SfcTSqlScript();
		}
		return new SfcTSqlScript(scriptDropAction.GenerateScript(this));
	}

	public void Drop()
	{
		DropImpl();
		if (IsLocal)
		{
			GetStore().Serialize();
		}
	}

	public void Rename(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentException(RegSvrStrings.ArgumentNullOrEmpty("Name"));
		}
		Rename(new Key(name));
	}

	public void Rename(SfcKey key)
	{
		RenameImpl(key);
		if (IsLocal)
		{
			GetStore().Serialize();
		}
	}

	ISfcScript ISfcRenamable.ScriptRename(SfcKey key)
	{
		if (IsLocal)
		{
			return new SfcTSqlScript();
		}
		Key key2 = (Key)key;
		List<SfcTsqlProcFormatter.RuntimeArg> list = new List<SfcTsqlProcFormatter.RuntimeArg>();
		list.Add(new SfcTsqlProcFormatter.RuntimeArg(key2.Name.GetType(), key2.Name));
		return new SfcTSqlScript(scriptRenameAction.GenerateScript(this, list));
	}

	public void Move(ServerGroup newParent)
	{
		((ISfcMovable)this).Move((SfcInstance)newParent);
	}

	void ISfcMovable.Move(SfcInstance newParent)
	{
		if (newParent == null)
		{
			throw new ArgumentNullException("newParent");
		}
		if (!(newParent is ServerGroup))
		{
			throw new InvalidArgumentException("newParent");
		}
		MoveImpl(newParent);
		Trace.Assert(Parent == newParent);
		Trace.Assert(Parent.RegisteredServers.Contains(this));
		if (IsLocal)
		{
			GetStore().Serialize();
		}
	}

	ISfcScript ISfcMovable.ScriptMove(SfcInstance newParent)
	{
		if (IsLocal)
		{
			return new SfcTSqlScript();
		}
		Trace.Assert(newParent is ServerGroup);
		ServerGroup serverGroup = (ServerGroup)newParent;
		List<SfcTsqlProcFormatter.RuntimeArg> list = new List<SfcTsqlProcFormatter.RuntimeArg>();
		list.Add(new SfcTsqlProcFormatter.RuntimeArg(serverGroup.ID.GetType(), serverGroup.ID));
		return new SfcTSqlScript(scriptMoveAction.GenerateScript(this, list));
	}

	internal RegisteredServersStore GetStore()
	{
		RegisteredServersStore obj = base.KeyChain.RootKey.Domain as RegisteredServersStore;
		Trace.Assert(obj != null);
		return obj;
	}

	public void Export(string file, CredentialPersistenceType cpt)
	{
		RegisteredServersStore.Export(this, file, cpt);
	}
}
