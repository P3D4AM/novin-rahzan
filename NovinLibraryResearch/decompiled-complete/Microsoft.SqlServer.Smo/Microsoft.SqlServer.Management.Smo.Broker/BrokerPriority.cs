using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Dmf;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.Smo.Broker;

[EvaluationMode(AutomatedPolicyEvaluationMode.CheckOnSchedule)]
[PhysicalFacet]
[LocalizedPropertyResources("Microsoft.SqlServer.Management.Smo.Broker.BrokerLocalizableResources", true)]
[TypeConverter(typeof(LocalizableTypeConverter))]
public sealed class BrokerPriority : BrokerObjectBase, ICreatable, IAlterable, IDroppable, IDropIfExists, IScriptable
{
	internal class PropertyMetadataProvider : SqlPropertyMetadataProvider
	{
		private static int[] versionCount = new int[10] { 0, 0, 0, 6, 6, 6, 6, 6, 6, 6 };

		private static int[] cloudVersionCount = new int[3];

		private static int sqlDwPropertyCount = 0;

		internal static StaticMetadata[] sqlDwStaticMetadata = new StaticMetadata[0];

		internal static StaticMetadata[] cloudStaticMetadata = new StaticMetadata[0];

		internal static StaticMetadata[] staticMetadata = new StaticMetadata[6]
		{
			new StaticMetadata("ContractName", expensive: false, readOnly: false, typeof(string)),
			new StaticMetadata("ID", expensive: false, readOnly: true, typeof(int)),
			new StaticMetadata("LocalServiceName", expensive: false, readOnly: false, typeof(string)),
			new StaticMetadata("PolicyHealthState", expensive: true, readOnly: false, typeof(PolicyHealthState)),
			new StaticMetadata("PriorityLevel", expensive: false, readOnly: false, typeof(byte)),
			new StaticMetadata("RemoteServiceName", expensive: false, readOnly: false, typeof(string))
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
			return propertyName switch
			{
				"ContractName" => 0, 
				"ID" => 1, 
				"LocalServiceName" => 2, 
				"PolicyHealthState" => 3, 
				"PriorityLevel" => 4, 
				"RemoteServiceName" => 5, 
				_ => -1, 
			};
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

	public static string UrnSuffix => "BrokerPriority";

	[SfcObject(SfcObjectRelationship.ParentObject)]
	public ServiceBroker Parent
	{
		get
		{
			CheckObjectState();
			return base.ParentColl.ParentInstance as ServiceBroker;
		}
		set
		{
			SetParentImpl(value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	[SfcReference(typeof(ServiceContract), "Server[@Name = '{0}']/Database[@Name = '{1}']/ServiceBroker/ServiceContract[@Name='{2}']", new string[] { "Parent.Parent.Parent.ConnectionContext.TrueName", "Parent.Parent.Name", "ContractName" })]
	[CLSCompliant(false)]
	public string ContractName
	{
		get
		{
			return (string)base.Properties.GetValueWithNullReplacement("ContractName");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("ContractName", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public int ID => (int)base.Properties.GetValueWithNullReplacement("ID");

	[SfcProperty(SfcPropertyFlags.Standalone)]
	[SfcReference(typeof(BrokerService), "Server[@Name = '{0}']/Database[@Name = '{1}']/ServiceBroker/BrokerService[@Name='{2}']", new string[] { "Parent.Parent.Parent.ConnectionContext.TrueName", "Parent.Parent.Name", "LocalServiceName" })]
	[CLSCompliant(false)]
	public string LocalServiceName
	{
		get
		{
			return (string)base.Properties.GetValueWithNullReplacement("LocalServiceName");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("LocalServiceName", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public byte PriorityLevel
	{
		get
		{
			return (byte)base.Properties.GetValueWithNullReplacement("PriorityLevel");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("PriorityLevel", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public string RemoteServiceName
	{
		get
		{
			return (string)base.Properties.GetValueWithNullReplacement("RemoteServiceName");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("RemoteServiceName", value);
		}
	}

	internal BrokerPriority(AbstractCollectionBase parentColl, ObjectKeyBase key, SqlSmoState state)
		: base(parentColl, key, state)
	{
	}

	public void Create()
	{
		CreateImpl();
	}

	internal override void ScriptCreate(StringCollection queries, ScriptingPreferences sp)
	{
		SqlSmoObject.ThrowIfBelowVersion100(sp.TargetServerVersionInternal);
		GetDDL(queries, sp, bCreate: true);
	}

	public void Drop()
	{
		DropImpl();
	}

	public void DropIfExists()
	{
		DropImpl(isDropIfExists: true);
	}

	private void GetDDL(StringCollection queries, ScriptingPreferences sp, bool bCreate)
	{
		StringBuilder stringBuilder = new StringBuilder(Globals.INIT_BUFFER_SIZE);
		ScriptIncludeHeaders(stringBuilder, sp, UrnSuffix);
		string arg = FormatFullNameForScripting(sp);
		if (bCreate && sp.IncludeScripts.ExistenceCheck)
		{
			stringBuilder.AppendFormat(SmoApplication.DefaultCulture, Scripts.INCLUDE_EXISTS_BROKER_PRIORITY, "NOT", FormatFullNameForScripting(sp, nameIsIndentifier: false));
			stringBuilder.Append(sp.NewLine);
		}
		stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "{0} BROKER PRIORITY {1} {2}", bCreate ? "CREATE" : "ALTER", arg, Globals.newline);
		stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "FOR CONVERSATION");
		stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "{0} SET {1}", Globals.space, Globals.LParen);
		string empty = string.Empty;
		empty = (string)GetPropValueOptional("ContractName");
		stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "{0} CONTRACT_NAME = {1} {2}", Globals.newline, string.IsNullOrEmpty(empty) ? Scripts.ANY : SqlSmoObject.MakeSqlBraket(empty), Globals.comma);
		empty = (string)GetPropValueOptional("LocalServiceName");
		stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "{0}  LOCAL_SERVICE_NAME = {1} {2}", Globals.newline, string.IsNullOrEmpty(empty) ? Scripts.ANY : SqlSmoObject.MakeSqlBraket(empty), Globals.comma);
		empty = (string)GetPropValueOptional("RemoteServiceName");
		stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "{0}  REMOTE_SERVICE_NAME = {1} {2}", Globals.newline, string.IsNullOrEmpty(empty) ? Scripts.ANY : SqlSmoObject.MakeSqlString(empty), Globals.comma);
		object propValueOptional = GetPropValueOptional("PriorityLevel");
		stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "{0}  PRIORITY_LEVEL = {1} {2} ", Globals.newline, (propValueOptional == null) ? Scripts.DEFAULT : ((byte)propValueOptional).ToString(SmoApplication.DefaultCulture), Globals.RParen);
		queries.Add(stringBuilder.ToString());
	}

	internal override void ScriptDrop(StringCollection queries, ScriptingPreferences sp)
	{
		SqlSmoObject.ThrowIfBelowVersion100(sp.TargetServerVersionInternal);
		StringBuilder stringBuilder = new StringBuilder(Globals.INIT_BUFFER_SIZE);
		ScriptIncludeHeaders(stringBuilder, sp, UrnSuffix);
		if (sp.IncludeScripts.ExistenceCheck)
		{
			stringBuilder.AppendFormat(SmoApplication.DefaultCulture, Scripts.INCLUDE_EXISTS_BROKER_PRIORITY, "", FormatFullNameForScripting(sp, nameIsIndentifier: false));
			stringBuilder.Append(sp.NewLine);
		}
		stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "DROP BROKER PRIORITY {0}", FormatFullNameForScripting(sp));
		queries.Add(stringBuilder.ToString());
	}

	public void Alter()
	{
		AlterImpl();
	}

	internal override void ScriptAlter(StringCollection queries, ScriptingPreferences sp)
	{
		SqlSmoObject.ThrowIfBelowVersion100(sp.TargetServerVersionInternal);
		if (IsObjectDirty())
		{
			GetDDL(queries, sp, bCreate: false);
		}
	}

	public BrokerPriority()
	{
	}

	public BrokerPriority(ServiceBroker serviceBroker, string name)
	{
		ValidateName(name);
		key = new SimpleObjectKey(name);
		Parent = serviceBroker;
	}

	internal override SqlPropertyMetadataProvider GetPropertyMetadataProvider()
	{
		return new PropertyMetadataProvider(base.ServerVersion, DatabaseEngineType, DatabaseEngineEdition);
	}
}
