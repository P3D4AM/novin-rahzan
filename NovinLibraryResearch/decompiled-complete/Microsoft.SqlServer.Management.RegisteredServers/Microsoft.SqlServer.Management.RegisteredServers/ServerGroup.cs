#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Xml;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.RegisteredServers;

public sealed class ServerGroup : ServerGroupParent, ISfcValidate, ISfcCreatable, ICreatable, ISfcAlterable, IAlterable, ISfcDroppable, IDroppable, ISfcRenamable, IRenamable, ISfcMovable
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
			return string.Format("{0}[@Name='{1}']", "ServerGroup", SfcSecureString.EscapeSquote(Name));
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
			return new ServerGroup();
		}
	}

	private static readonly SfcTsqlProcFormatter scriptCreateAction;

	private static readonly SfcTsqlProcFormatter scriptAlterAction;

	private static readonly SfcTsqlProcFormatter scriptDropAction;

	private static readonly SfcTsqlProcFormatter scriptRenameAction;

	private static readonly SfcTsqlProcFormatter scriptMoveAction;

	private RegisteredServerCollection registeredServers;

	private ServerGroupCollection serverGroups;

	internal const string typeName = "ServerGroup";

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(RegisteredServer))]
	public RegisteredServerCollection RegisteredServers
	{
		get
		{
			if (registeredServers == null)
			{
				IComparer<string> customComparer = null;
				if (base.KeyChain != null && !IsLocal)
				{
					customComparer = new ServerComparer(GetStore().ServerConnection, "msdb");
				}
				registeredServers = new RegisteredServerCollection(this, customComparer);
				if (base.KeyChain != null && !IsLocal && RegisteredServerChildCount == 0)
				{
					registeredServers.SkipInitialSqlLoad = true;
				}
			}
			return registeredServers;
		}
	}

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(ServerGroup))]
	public ServerGroupCollection ServerGroups
	{
		get
		{
			if (serverGroups == null)
			{
				IComparer<string> customComparer = null;
				if (base.KeyChain != null && !IsLocal)
				{
					customComparer = new ServerComparer(GetStore().ServerConnection, "msdb");
				}
				serverGroups = new ServerGroupCollection(this, customComparer);
				if (base.KeyChain != null && !IsLocal && ServerGroupChildCount == 0)
				{
					serverGroups.SkipInitialSqlLoad = true;
				}
			}
			return serverGroups;
		}
	}

	[SfcObject(SfcObjectRelationship.ParentObject, SfcObjectCardinality.One)]
	public new ServerGroupParent Parent
	{
		get
		{
			return (ServerGroupParent)base.Parent;
		}
		set
		{
			base.Parent = value;
			if (value is ServerGroup)
			{
				ServerType = ((ServerGroup)Parent).ServerType;
			}
		}
	}

	[SfcIgnore]
	public Key IdentityKey => (Key)base.AbstractIdentityKey;

	[SfcProperty(SfcPropertyFlags.Required | SfcPropertyFlags.ReadOnlyAfterCreation)]
	[SfcKey(0)]
	public string Name => (string)base.Properties["Name"].Value;

	[SfcProperty(SfcPropertyFlags.Data)]
	public int ID => (int)base.Properties["ID"].Value;

	[SfcProperty(SfcPropertyFlags.Data)]
	private int ServerGroupChildCount
	{
		get
		{
			object value = base.Properties["ServerGroupChildCount"].Value;
			if (value == null)
			{
				return 0;
			}
			return (int)value;
		}
	}

	[SfcProperty(SfcPropertyFlags.Data)]
	private int RegisteredServerChildCount
	{
		get
		{
			object value = base.Properties["RegisteredServerChildCount"].Value;
			if (value == null)
			{
				return 0;
			}
			return (int)value;
		}
	}

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

	[SfcProperty(SfcPropertyFlags.None)]
	public ServerType ServerType
	{
		get
		{
			return (ServerType)base.Properties["ServerType"].Value;
		}
		internal set
		{
			base.Properties["ServerType"].Value = value;
		}
	}

	[SfcIgnore]
	public string DisplayName
	{
		get
		{
			string result = Name;
			if (Parent is RegisteredServersStore registeredServersStore)
			{
				if (this == registeredServersStore.AnalysisServicesServerGroup)
				{
					result = RegSvrStrings.AnalysisServicesServerGroupDisplayName;
				}
				else if (this == registeredServersStore.CentralManagementServerGroup)
				{
					result = RegSvrStrings.CentralManagementServerGroupDisplayName;
				}
				else if (this == registeredServersStore.DatabaseEngineServerGroup)
				{
					result = RegSvrStrings.DatabaseEngineServerGroupDisplayName;
				}
				else if (this == registeredServersStore.IntegrationServicesServerGroup)
				{
					result = RegSvrStrings.IntegrationServicesServerGroupDisplayName;
				}
				else if (this == registeredServersStore.ReportingServicesServerGroup)
				{
					result = RegSvrStrings.ReportingServicesServerGroupDisplayName;
				}
				else if (this == registeredServersStore.SqlServerCompactEditionServerGroup)
				{
					result = RegSvrStrings.SqlServerCompactServerGroupDisplayName;
				}
				else
				{
					Trace.Assert(condition: false, "unexpected group");
				}
			}
			return result;
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
	public bool IsSystemServerGroup
	{
		get
		{
			if (Parent != null && Parent.GetType().Equals(typeof(RegisteredServersStore)) && IsStandardServerGroup)
			{
				return true;
			}
			return false;
		}
	}

	[SfcIgnore]
	internal bool IsStandardServerGroup
	{
		get
		{
			if (Name.Equals(RegisteredServersStore.databaseEngineServerGroupName) || Name.Equals(RegisteredServersStore.analysisServicesServerGroupName) || Name.Equals(RegisteredServersStore.reportingServicesServerGroupName) || Name.Equals(RegisteredServersStore.sqlServerCompactEditionServerGroupName) || Name.Equals(RegisteredServersStore.integrationServicesServerGroupName))
			{
				return true;
			}
			return false;
		}
	}

	[SfcIgnore]
	public bool IsDropped => base.State == SfcObjectState.Dropped;

	public event EventHandler<DuplicateFoundEventArgs> DuplicateFound;

	static ServerGroup()
	{
		scriptCreateAction = new SfcTsqlProcFormatter();
		scriptAlterAction = new SfcTsqlProcFormatter();
		scriptDropAction = new SfcTsqlProcFormatter();
		scriptRenameAction = new SfcTsqlProcFormatter();
		scriptMoveAction = new SfcTsqlProcFormatter();
		scriptCreateAction.Procedure = "msdb.dbo.sp_sysmanagement_add_shared_server_group";
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("parent_id", required: true));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("name", "Name", required: true, output: false));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("description", "Description", required: false, output: false));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("server_type", "ServerType", required: true, output: false));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("server_group_id", "ID", required: false, output: true));
		scriptAlterAction.Procedure = "msdb.dbo.sp_sysmanagement_update_shared_server_group";
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("server_group_id", "ID", required: true, output: false));
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("description", "Description", required: false, output: false));
		scriptDropAction.Procedure = "msdb.dbo.sp_sysmanagement_delete_shared_server_group";
		scriptDropAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("server_group_id", "ID", required: true, output: false));
		scriptRenameAction.Procedure = "msdb.dbo.sp_sysmanagement_rename_shared_server_group";
		scriptRenameAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("server_group_id", "ID", required: true, output: false));
		scriptRenameAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("new_name", required: true));
		scriptMoveAction.Procedure = "msdb.dbo.sp_sysmanagement_move_shared_server_group";
		scriptMoveAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("server_group_id", "ID", required: true, output: false));
		scriptMoveAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("new_parent_id", required: true));
	}

	public ServerGroup()
	{
	}

	public ServerGroup(string name)
	{
		SetName(name);
	}

	public ServerGroup(ServerGroup parent, string name)
		: this(name)
	{
		Parent = parent;
		ServerType = ((ServerGroup)Parent).ServerType;
	}

	internal ServerGroup(RegisteredServersStore parent, string name)
	{
		SetName(name);
		Parent = parent;
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

	protected internal override ISfcCollection GetChildCollection(string elementType)
	{
		if (!(elementType == "ServerGroup"))
		{
			if (elementType == "RegisteredServer")
			{
				return RegisteredServers;
			}
			throw new RegisteredServerException(RegSvrStrings.NoSuchCollection(elementType));
		}
		return ServerGroups;
	}

	public ValidationState Validate(string methodName, params object[] arguments)
	{
		ValidationState validationState = new ValidationState();
		Validate(methodName, throwOnFirst: false, validationState);
		return validationState;
	}

	internal void Validate(string validationMode, bool throwOnFirst, ValidationState validationState)
	{
		if (Parent != null && Parent is ServerGroup serverGroup && serverGroup.Name.Equals(RegisteredServersStore.centralManagementServerGroupName))
		{
			throw new RegisteredServerException(RegSvrStrings.CannotCreateAServerGroupUnderneathCentralManagementServerGroup);
		}
		if (string.IsNullOrEmpty(Name))
		{
			Exception ex = new SfcPropertyNotSetException("Name");
			if (throwOnFirst)
			{
				throw ex;
			}
			validationState.AddError(ex, "Name");
		}
	}

	ISfcScript ISfcCreatable.ScriptCreate()
	{
		if (IsLocal)
		{
			return new SfcTSqlScript();
		}
		List<SfcTsqlProcFormatter.RuntimeArg> list = new List<SfcTsqlProcFormatter.RuntimeArg>();
		Trace.Assert(Parent is ServerGroup);
		list.Add(new SfcTsqlProcFormatter.RuntimeArg(((ServerGroup)Parent).ID.GetType(), ((ServerGroup)Parent).ID));
		return new SfcTSqlScript(scriptCreateAction.GenerateScript(this, list));
	}

	public void Create()
	{
		ServerGroupCollection serverGroupCollection = null;
		if (Parent is ServerGroup)
		{
			serverGroupCollection = ((ServerGroup)Parent).ServerGroups;
		}
		else if (Parent is RegisteredServersStore)
		{
			serverGroupCollection = ((RegisteredServersStore)Parent).ServerGroups;
		}
		if (serverGroupCollection.Contains(Name))
		{
			throw new RegisteredServerException(RegSvrStrings.ServerGroupAlreadyExists(Name));
		}
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
		if (IsSystemServerGroup)
		{
			throw new RegisteredServerException(RegSvrStrings.CannotDropSystemServerGroup(Name));
		}
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
		if (IsSystemServerGroup)
		{
			throw new RegisteredServerException(RegSvrStrings.CannotRenameSystemServerGroup(Name));
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
		Trace.Assert(((ServerGroup)newParent).ServerGroups.Contains(this));
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

	private RegisteredServersStore GetStore()
	{
		RegisteredServersStore obj = base.KeyChain.RootKey.Domain as RegisteredServersStore;
		Trace.Assert(obj != null);
		return obj;
	}

	public List<RegisteredServer> GetDescendantRegisteredServers()
	{
		List<RegisteredServer> list = new List<RegisteredServer>();
		GetDescendantRegisteredServersRec(list);
		return list;
	}

	private void GetDescendantRegisteredServersRec(List<RegisteredServer> regSrvList)
	{
		regSrvList.AddRange(RegisteredServers);
		foreach (ServerGroup serverGroup in ServerGroups)
		{
			serverGroup.GetDescendantRegisteredServersRec(regSrvList);
		}
	}

	public override void Discover(ISfcDependencyDiscoveryObjectSink sink)
	{
		if (sink.Action != SfcDependencyAction.Serialize)
		{
			return;
		}
		foreach (RegisteredServer registeredServer in RegisteredServers)
		{
			sink.Add(SfcDependencyDirection.Inbound, registeredServer, SfcTypeRelation.ContainedChild, discovered: false);
		}
		foreach (ServerGroup serverGroup in ServerGroups)
		{
			sink.Add(SfcDependencyDirection.Inbound, serverGroup, SfcTypeRelation.ContainedChild, discovered: false);
		}
	}

	public void Export(string file, CredentialPersistenceType cpt)
	{
		RegisteredServersStore.Export(this, file, cpt);
	}

	private DialogResult DeleteObject(object obj, object[] param, bool isLocal, ref bool applyToAll, ref bool confirm)
	{
		if (this.DuplicateFound != null)
		{
			DuplicateFoundEventArgs duplicateFoundEventArgs = null;
			MethodInfo method = obj.GetType().GetMethod("Drop");
			if (applyToAll)
			{
				if (!confirm)
				{
					return DialogResult.No;
				}
				if (method != null)
				{
					method.Invoke(obj, param);
					if (isLocal)
					{
						RaiseSfcAppObjectDroppedEvent((SfcInstance)obj);
					}
				}
			}
			else
			{
				duplicateFoundEventArgs = new DuplicateFoundEventArgs();
				this.DuplicateFound(obj, duplicateFoundEventArgs);
				if (duplicateFoundEventArgs.Cancel)
				{
					return DialogResult.Cancel;
				}
				if (duplicateFoundEventArgs.ApplyToAll)
				{
					applyToAll = true;
					confirm = duplicateFoundEventArgs.Confirm;
				}
				if (!duplicateFoundEventArgs.Confirm)
				{
					return DialogResult.No;
				}
				if (method != null)
				{
					method.Invoke(obj, param);
					if (isLocal)
					{
						RaiseSfcAppObjectDroppedEvent((SfcInstance)obj);
					}
				}
			}
		}
		return DialogResult.Yes;
	}

	public void Import(string file)
	{
		try
		{
			if (file == null)
			{
				throw new ArgumentNullException("file");
			}
			bool applyToAll = false;
			bool confirm = false;
			using XmlTextReader xmlReader = new XmlTextReader(file)
			{
				DtdProcessing = DtdProcessing.Prohibit
			};
			SfcSerializer sfcSerializer = new SfcSerializer();
			SfcObjectState state = ((!IsLocal) ? SfcObjectState.Pending : SfcObjectState.Existing);
			base.State = SfcObjectState.Existing;
			object obj = sfcSerializer.Deserialize(xmlReader, state);
			if (obj is RegisteredServer)
			{
				RegisteredServer registeredServer = obj as RegisteredServer;
				registeredServer.Parent = this;
				if (!IsLocal)
				{
					DialogResult dialogResult = DialogResult.Yes;
					if (registeredServer.Parent.RegisteredServers.Contains(registeredServer.Name))
					{
						dialogResult = DeleteObject(registeredServer.Parent.RegisteredServers[registeredServer.Name], null, IsLocal, ref applyToAll, ref confirm);
					}
					if (dialogResult != 0 && DialogResult.Yes == dialogResult)
					{
						registeredServer.Create();
					}
					return;
				}
				DialogResult dialogResult2 = DialogResult.Yes;
				if (RegisteredServers.Contains(registeredServer.Name))
				{
					dialogResult2 = DeleteObject(RegisteredServers[registeredServer.Name], null, IsLocal, ref applyToAll, ref confirm);
				}
				if (dialogResult2 != 0 && DialogResult.Yes == dialogResult2)
				{
					RegisteredServers.Add(registeredServer);
					RaiseSfcAppObjectCreatedEvent(registeredServer);
				}
			}
			else
			{
				if (!(obj is ServerGroup))
				{
					return;
				}
				ServerGroup serverGroup = obj as ServerGroup;
				serverGroup.Parent = this;
				if (!IsLocal)
				{
					if (serverGroup.IsStandardServerGroup)
					{
						foreach (RegisteredServer item in new List<RegisteredServer>(serverGroup.RegisteredServers))
						{
							item.Parent = this;
							DialogResult dialogResult3 = DialogResult.Yes;
							if (item.Parent.RegisteredServers.Contains(item.Name))
							{
								dialogResult3 = DeleteObject(item.Parent.RegisteredServers[item.Name], null, IsLocal, ref applyToAll, ref confirm);
							}
							if (dialogResult3 == DialogResult.Cancel)
							{
								return;
							}
							if (DialogResult.Yes == dialogResult3)
							{
								item.Create();
							}
						}
						{
							foreach (ServerGroup item2 in new List<ServerGroup>(serverGroup.ServerGroups))
							{
								item2.Parent = this;
								DialogResult dialogResult4 = DialogResult.Yes;
								if (ServerGroups.Contains(item2.Name))
								{
									dialogResult4 = DeleteObject(ServerGroups[item2.Name], null, IsLocal, ref applyToAll, ref confirm);
								}
								if (dialogResult4 == DialogResult.Cancel)
								{
									break;
								}
								if (DialogResult.Yes == dialogResult4)
								{
									item2.DeepCreate();
								}
							}
							return;
						}
					}
					DialogResult dialogResult5 = DialogResult.Yes;
					if (ServerGroups.Contains(serverGroup.Name))
					{
						dialogResult5 = DeleteObject(ServerGroups[serverGroup.Name], null, IsLocal, ref applyToAll, ref confirm);
					}
					if (dialogResult5 != 0 && DialogResult.Yes == dialogResult5)
					{
						serverGroup.DeepCreate();
					}
					return;
				}
				if (serverGroup.IsStandardServerGroup)
				{
					foreach (ServerGroup item3 in new List<ServerGroup>(serverGroup.ServerGroups))
					{
						DialogResult dialogResult6 = DialogResult.Yes;
						if (ServerGroups.Contains(item3.Name))
						{
							dialogResult6 = DeleteObject(ServerGroups[item3.Name], null, IsLocal, ref applyToAll, ref confirm);
						}
						if (dialogResult6 == DialogResult.Cancel)
						{
							return;
						}
						if (DialogResult.Yes == dialogResult6)
						{
							item3.Move(this);
							RaiseSfcAppObjectCreatedEvent(item3);
						}
					}
					{
						foreach (RegisteredServer item4 in new List<RegisteredServer>(serverGroup.RegisteredServers))
						{
							DialogResult dialogResult7 = DialogResult.Yes;
							if (RegisteredServers.Contains(item4.Name))
							{
								dialogResult7 = DeleteObject(RegisteredServers[item4.Name], null, IsLocal, ref applyToAll, ref confirm);
							}
							if (dialogResult7 == DialogResult.Cancel)
							{
								break;
							}
							if (DialogResult.Yes == dialogResult7)
							{
								item4.Move(this);
								RaiseSfcAppObjectCreatedEvent(item4);
							}
						}
						return;
					}
				}
				DialogResult dialogResult8 = DialogResult.Yes;
				if (ServerGroups.Contains(serverGroup.Name))
				{
					dialogResult8 = DeleteObject(ServerGroups[serverGroup.Name], null, IsLocal, ref applyToAll, ref confirm);
				}
				if (dialogResult8 != 0 && DialogResult.Yes == dialogResult8)
				{
					ServerGroups.Add(serverGroup);
					GetStore().Serialize();
					RaiseSfcAppObjectCreatedEvent(serverGroup);
				}
			}
		}
		catch (Exception ex)
		{
			RegisteredServersStore.FilterException(ex);
			throw new RegisteredServerException(RegSvrStrings.FailedOperation(RegSvrStrings.Import), ex);
		}
	}

	internal void DeepCreate()
	{
		List<RegisteredServer> list = new List<RegisteredServer>(RegisteredServers.Count);
		foreach (RegisteredServer registeredServer in RegisteredServers)
		{
			list.Add(registeredServer);
		}
		List<ServerGroup> list2 = new List<ServerGroup>(ServerGroups.Count);
		foreach (ServerGroup serverGroup in ServerGroups)
		{
			list2.Add(serverGroup);
		}
		Create();
		foreach (RegisteredServer item in list)
		{
			item.Create();
		}
		foreach (ServerGroup item2 in list2)
		{
			item2.DeepCreate();
		}
	}

	internal void RaiseSfcAppObjectCreatedEvent(SfcInstance obj)
	{
		SfcApplication.Events.OnObjectCreated(obj, new SfcObjectCreatedEventArgs(obj.Urn, obj));
	}

	internal void RaiseSfcAppObjectDroppedEvent(SfcInstance obj)
	{
		SfcApplication.Events.OnObjectDropped(obj, new SfcObjectDroppedEventArgs(obj.Urn, obj));
	}
}
