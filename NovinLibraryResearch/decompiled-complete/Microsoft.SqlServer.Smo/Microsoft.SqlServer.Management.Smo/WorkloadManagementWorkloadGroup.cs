using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.Smo;

[SfcElement(SfcElementFlags.Standalone | SfcElementFlags.SqlAzureDatabase)]
public sealed class WorkloadManagementWorkloadGroup : ScriptNameObjectBase, ICreatable, IDroppable, IDropIfExists, IAlterable, IScriptable, ISfcSupportsDesignMode
{
	internal class PropertyMetadataProvider : SqlPropertyMetadataProvider
	{
		private static int[] versionCount = new int[10];

		private static int[] cloudVersionCount = new int[3];

		private static int sqlDwPropertyCount = 9;

		internal static StaticMetadata[] sqlDwStaticMetadata = new StaticMetadata[9]
		{
			new StaticMetadata("CapPercentageResource", expensive: false, readOnly: false, typeof(int)),
			new StaticMetadata("GroupId", expensive: false, readOnly: false, typeof(int)),
			new StaticMetadata("HasClassifier", expensive: true, readOnly: false, typeof(bool)),
			new StaticMetadata("Importance", expensive: false, readOnly: false, typeof(WorkloadManagementImportance)),
			new StaticMetadata("IsSystemObject", expensive: false, readOnly: true, typeof(bool)),
			new StaticMetadata("MinPercentageResource", expensive: false, readOnly: false, typeof(int)),
			new StaticMetadata("QueryExecutionTimeoutSec", expensive: false, readOnly: false, typeof(int)),
			new StaticMetadata("RequestMaxResourceGrantPercent", expensive: false, readOnly: false, typeof(double)),
			new StaticMetadata("RequestMinResourceGrantPercent", expensive: false, readOnly: false, typeof(double))
		};

		internal static StaticMetadata[] cloudStaticMetadata = new StaticMetadata[0];

		internal static StaticMetadata[] staticMetadata = new StaticMetadata[0];

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
				if (base.DatabaseEngineEdition == DatabaseEngineEdition.SqlDataWarehouse)
				{
					return propertyName switch
					{
						"CapPercentageResource" => 0, 
						"GroupId" => 1, 
						"HasClassifier" => 2, 
						"Importance" => 3, 
						"IsSystemObject" => 4, 
						"MinPercentageResource" => 5, 
						"QueryExecutionTimeoutSec" => 6, 
						"RequestMaxResourceGrantPercent" => 7, 
						"RequestMinResourceGrantPercent" => 8, 
						_ => -1, 
					};
				}
				return -1;
			}
			return -1;
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

	public static string UrnSuffix => "WorkloadManagementWorkloadGroup";

	[SfcObject(SfcObjectRelationship.ParentObject)]
	public Database Parent
	{
		get
		{
			CheckObjectState();
			return base.ParentColl.ParentInstance as Database;
		}
		set
		{
			SetParentImpl(value);
		}
	}

	bool ISfcSupportsDesignMode.IsDesignMode => base.IsDesignMode;

	[SfcProperty]
	public int CapPercentageResource
	{
		get
		{
			return (int)base.Properties.GetValueWithNullReplacement("CapPercentageResource");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("CapPercentageResource", value);
		}
	}

	[SfcProperty]
	public int GroupId
	{
		get
		{
			return (int)base.Properties.GetValueWithNullReplacement("GroupId");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("GroupId", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Expensive)]
	public bool HasClassifier
	{
		get
		{
			return (bool)base.Properties.GetValueWithNullReplacement("HasClassifier");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("HasClassifier", value);
		}
	}

	[SfcProperty]
	public WorkloadManagementImportance Importance
	{
		get
		{
			return (WorkloadManagementImportance)base.Properties.GetValueWithNullReplacement("Importance");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("Importance", value);
		}
	}

	[SfcProperty]
	public bool IsSystemObject => (bool)base.Properties.GetValueWithNullReplacement("IsSystemObject");

	[SfcProperty]
	public int MinPercentageResource
	{
		get
		{
			return (int)base.Properties.GetValueWithNullReplacement("MinPercentageResource");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("MinPercentageResource", value);
		}
	}

	[SfcProperty]
	public int QueryExecutionTimeoutSec
	{
		get
		{
			return (int)base.Properties.GetValueWithNullReplacement("QueryExecutionTimeoutSec");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("QueryExecutionTimeoutSec", value);
		}
	}

	[SfcProperty]
	public double RequestMaxResourceGrantPercent
	{
		get
		{
			return (double)base.Properties.GetValueWithNullReplacement("RequestMaxResourceGrantPercent");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("RequestMaxResourceGrantPercent", value);
		}
	}

	[SfcProperty]
	public double RequestMinResourceGrantPercent
	{
		get
		{
			return (double)base.Properties.GetValueWithNullReplacement("RequestMinResourceGrantPercent");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("RequestMinResourceGrantPercent", value);
		}
	}

	internal WorkloadManagementWorkloadGroup(AbstractCollectionBase parentColl, ObjectKeyBase key, SqlSmoState state)
		: base(parentColl, key, state)
	{
	}

	public void Create()
	{
		CreateImpl();
	}

	public void Alter()
	{
		AlterImpl();
	}

	public void Drop()
	{
		DropImpl();
	}

	public void DropIfExists()
	{
		DropImpl(isDropIfExists: true);
	}

	public StringCollection Script()
	{
		return ScriptImpl();
	}

	public StringCollection Script(ScriptingOptions scriptingOptions)
	{
		return ScriptImpl(scriptingOptions);
	}

	internal void ThrowIfTargetNotDw(bool targetEngineIsAzureSqlDw)
	{
		if (!targetEngineIsAzureSqlDw)
		{
			throw new FailedOperationException(ExceptionTemplatesImpl.UnsupportedVersionException, this, new UnsupportedEngineEditionException());
		}
	}

	internal override void ScriptCreate(StringCollection queries, ScriptingPreferences sp)
	{
		ThrowIfTargetNotDw(sp.TargetEngineIsAzureSqlDw());
		StringBuilder stringBuilder = new StringBuilder(Globals.INIT_BUFFER_SIZE);
		if (sp.IncludeScripts.Header)
		{
			ScriptIncludeHeaders(stringBuilder, sp, UrnSuffix);
		}
		if (sp.IncludeScripts.ExistenceCheck)
		{
			stringBuilder.Append(IncludeExistsWorkloadManagementWorkloadGroup(fExists: false, FormatFullNameForScripting(sp, nameIsIndentifier: false)));
			stringBuilder.Append(sp.NewLine);
			stringBuilder.Append(Scripts.BEGIN);
			stringBuilder.Append(sp.NewLine);
		}
		stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "CREATE WORKLOAD GROUP {0}", FormatFullNameForScripting(sp));
		stringBuilder.Append(sp.NewLine);
		int count = 0;
		GetAllParams(stringBuilder, sp, ref count);
		if (sp.IncludeScripts.ExistenceCheck)
		{
			stringBuilder.Append(sp.NewLine);
			stringBuilder.Append(Scripts.END);
			stringBuilder.Append(sp.NewLine);
		}
		queries.Add(stringBuilder.ToString());
	}

	internal override void ScriptAlter(StringCollection queries, ScriptingPreferences sp)
	{
		ThrowIfTargetNotDw(sp.TargetEngineIsAzureSqlDw());
		StringBuilder stringBuilder = new StringBuilder(Globals.INIT_BUFFER_SIZE);
		ScriptIncludeHeaders(stringBuilder, sp, UrnSuffix);
		stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "ALTER WORKLOAD GROUP {0}", FormatFullNameForScripting(sp));
		int count = 0;
		GetAllParams(stringBuilder, sp, ref count);
		queries.Add(stringBuilder.ToString());
	}

	internal override void ScriptDrop(StringCollection queries, ScriptingPreferences sp)
	{
		ThrowIfTargetNotDw(sp.TargetEngineIsAzureSqlDw());
		StringBuilder stringBuilder = new StringBuilder(Globals.INIT_BUFFER_SIZE);
		if (sp.IncludeScripts.ExistenceCheck)
		{
			stringBuilder.Append(IncludeExistsWorkloadManagementWorkloadGroup(fExists: true, FormatFullNameForScripting(sp, nameIsIndentifier: false)));
			stringBuilder.Append(sp.NewLine);
			stringBuilder.Append(Scripts.BEGIN);
			stringBuilder.Append(sp.NewLine);
		}
		stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "DROP WORKLOAD GROUP {0}", FormatFullNameForScripting(sp));
		if (sp.IncludeScripts.ExistenceCheck)
		{
			stringBuilder.Append(sp.NewLine);
			stringBuilder.Append(Scripts.END);
			stringBuilder.Append(sp.NewLine);
		}
		queries.Add(stringBuilder.ToString());
	}

	private void GetAllParams(StringBuilder sb, ScriptingPreferences sp, ref int count)
	{
		StringBuilder stringBuilder = new StringBuilder(Globals.INIT_BUFFER_SIZE);
		GetParameter(stringBuilder, sp, "MinPercentageResource", "min_percentage_resource={0}", ref count);
		GetParameter(stringBuilder, sp, "CapPercentageResource", "cap_percentage_resource={0}", ref count);
		GetParameter(stringBuilder, sp, "RequestMinResourceGrantPercent", "request_min_resource_grant_percent={0}", ref count);
		GetParameter(stringBuilder, sp, "RequestMaxResourceGrantPercent", "request_max_resource_grant_percent={0}", ref count);
		GetImportanceTSql(stringBuilder, ref count);
		GetParameter(stringBuilder, sp, "QueryExecutionTimeoutSec", "query_execution_timeout_sec={0}", ref count);
		sb.AppendFormat(SmoApplication.DefaultCulture, "WITH({0})", stringBuilder.ToString());
	}

	private void GetImportanceTSql(StringBuilder buffer, ref int count)
	{
		Property propertyOptional = GetPropertyOptional("Importance");
		if (propertyOptional.Value != null)
		{
			if (count++ > 0)
			{
				buffer.Append(Globals.commaspace);
				buffer.Append(Globals.newline);
				buffer.Append(Globals.tab);
				buffer.Append(Globals.tab);
			}
			TypeConverter typeConverter = SmoManagementUtil.GetTypeConverter(typeof(WorkloadManagementImportance));
			buffer.AppendFormat(SmoApplication.DefaultCulture, "importance={0}", typeConverter.ConvertToInvariantString(propertyOptional.Value));
		}
	}

	private string IncludeExistsWorkloadManagementWorkloadGroup(bool fExists, string workloadGroupName)
	{
		return "IF" + (fExists ? string.Empty : " NOT") + " EXISTS (SELECT name FROM sys.workload_management_workload_groups WHERE name = " + workloadGroupName + ")";
	}

	internal static string[] GetScriptFields(Type parentType, ServerVersion version, DatabaseEngineType databaseEngineType, DatabaseEngineEdition databaseEngineEdition, bool defaultTextMode)
	{
		string[] fields = new string[10] { "CapPercentageResource", "GroupId", "HasClassifier", "Importance", "IsSystemObject", "MinPercentageResource", "Name", "QueryExecutionTimeoutSec", "RequestMaxResourceGrantPercent", "RequestMinResourceGrantPercent" };
		return SqlSmoObject.GetSupportedScriptFields(typeof(View.PropertyMetadataProvider), fields, version, databaseEngineType, databaseEngineEdition).ToArray();
	}

	public WorkloadManagementWorkloadGroup()
	{
	}

	public WorkloadManagementWorkloadGroup(Database database, string name)
	{
		ValidateName(name);
		key = new SimpleObjectKey(name);
		Parent = database;
	}

	internal override SqlPropertyMetadataProvider GetPropertyMetadataProvider()
	{
		return new PropertyMetadataProvider(base.ServerVersion, DatabaseEngineType, DatabaseEngineEdition);
	}
}
