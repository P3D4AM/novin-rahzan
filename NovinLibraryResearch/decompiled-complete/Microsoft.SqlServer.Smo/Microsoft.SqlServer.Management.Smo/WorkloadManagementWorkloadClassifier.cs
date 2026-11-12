using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.Smo;

[SfcElement(SfcElementFlags.Standalone | SfcElementFlags.SqlAzureDatabase)]
public sealed class WorkloadManagementWorkloadClassifier : ScriptNameObjectBase, ICreatable, IDroppable, IScriptable, ISfcSupportsDesignMode
{
	internal class PropertyMetadataProvider : SqlPropertyMetadataProvider
	{
		private static int[] versionCount = new int[10];

		private static int[] cloudVersionCount = new int[3];

		private static int sqlDwPropertyCount = 8;

		internal static StaticMetadata[] sqlDwStaticMetadata = new StaticMetadata[8]
		{
			new StaticMetadata("EndTime", expensive: false, readOnly: false, typeof(string)),
			new StaticMetadata("GroupName", expensive: false, readOnly: false, typeof(string)),
			new StaticMetadata("Id", expensive: false, readOnly: false, typeof(int)),
			new StaticMetadata("Importance", expensive: false, readOnly: false, typeof(WorkloadManagementImportance)),
			new StaticMetadata("MemberName", expensive: false, readOnly: false, typeof(string)),
			new StaticMetadata("StartTime", expensive: false, readOnly: false, typeof(string)),
			new StaticMetadata("WlmContext", expensive: false, readOnly: false, typeof(string)),
			new StaticMetadata("WlmLabel", expensive: false, readOnly: false, typeof(string))
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
						"EndTime" => 0, 
						"GroupName" => 1, 
						"Id" => 2, 
						"Importance" => 3, 
						"MemberName" => 4, 
						"StartTime" => 5, 
						"WlmContext" => 6, 
						"WlmLabel" => 7, 
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

	public static string UrnSuffix => "WorkloadManagementWorkloadClassifier";

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
	public string EndTime
	{
		get
		{
			return (string)base.Properties.GetValueWithNullReplacement("EndTime");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("EndTime", value);
		}
	}

	[SfcProperty]
	public string GroupName
	{
		get
		{
			return (string)base.Properties.GetValueWithNullReplacement("GroupName");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("GroupName", value);
		}
	}

	[SfcProperty]
	public int Id
	{
		get
		{
			return (int)base.Properties.GetValueWithNullReplacement("Id");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("Id", value);
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
	public string MemberName
	{
		get
		{
			return (string)base.Properties.GetValueWithNullReplacement("MemberName");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("MemberName", value);
		}
	}

	[SfcProperty]
	public string StartTime
	{
		get
		{
			return (string)base.Properties.GetValueWithNullReplacement("StartTime");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("StartTime", value);
		}
	}

	[SfcProperty]
	public string WlmContext
	{
		get
		{
			return (string)base.Properties.GetValueWithNullReplacement("WlmContext");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("WlmContext", value);
		}
	}

	[SfcProperty]
	public string WlmLabel
	{
		get
		{
			return (string)base.Properties.GetValueWithNullReplacement("WlmLabel");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("WlmLabel", value);
		}
	}

	internal WorkloadManagementWorkloadClassifier(AbstractCollectionBase parentColl, ObjectKeyBase key, SqlSmoState state)
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

	internal override void ScriptCreate(StringCollection queries, ScriptingPreferences sp)
	{
		ThrowIfTargetNotDw(sp.TargetEngineIsAzureSqlDw());
		StringBuilder stringBuilder = new StringBuilder(Globals.INIT_BUFFER_SIZE);
		if (sp.IncludeScripts.ExistenceCheck)
		{
			stringBuilder.Append(IncludeExistsWorkloadClassifier(FormatFullNameForScripting(sp, nameIsIndentifier: false)));
			stringBuilder.Append(sp.NewLine);
			stringBuilder.Append(Scripts.BEGIN);
			stringBuilder.Append(sp.NewLine);
		}
		ScriptIncludeHeaders(stringBuilder, sp, UrnSuffix);
		stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "CREATE WORKLOAD CLASSIFIER {0}", FormatFullNameForScripting(sp));
		if (sp.IncludeScripts.ExistenceCheck)
		{
			stringBuilder.Append(sp.NewLine);
			stringBuilder.Append(Scripts.END);
			stringBuilder.Append(sp.NewLine);
		}
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
			stringBuilder.Append(IncludeExistsWorkloadClassifier(FormatFullNameForScripting(sp, nameIsIndentifier: false)));
			stringBuilder.Append(sp.NewLine);
			stringBuilder.Append(Scripts.BEGIN);
			stringBuilder.Append(sp.NewLine);
		}
		stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "DROP WORKLOAD CLASSIFIER {0}", FormatFullNameForScripting(sp));
		if (sp.IncludeScripts.ExistenceCheck)
		{
			stringBuilder.Append(sp.NewLine);
			stringBuilder.Append(Scripts.END);
			stringBuilder.Append(sp.NewLine);
		}
		queries.Add(stringBuilder.ToString());
	}

	public static string IncludeExistsWorkloadClassifier(string name)
	{
		return "IF EXISTS ( SELECT name FROM sys.workload_management_workload_classifiers WHERE name = " + name + ")";
	}

	public void SetActivityWindow(int hours, int minutes, TimeSpan duration)
	{
		string text = hours.ToString().PadLeft(2, '0');
		string text2 = minutes.ToString().PadLeft(2, '0');
		StartTime = text + ":" + text2;
		duration += TimeSpan.FromHours(hours);
		duration += TimeSpan.FromMinutes(minutes);
		string endTime = duration.ToString("hh\\:mm");
		EndTime = endTime;
	}

	internal void ThrowIfTargetNotDw(bool targetEngineIsAzureSqlDw)
	{
		if (!targetEngineIsAzureSqlDw)
		{
			throw new FailedOperationException(ExceptionTemplatesImpl.UnsupportedVersionException, this, new UnsupportedEngineEditionException());
		}
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
			buffer.AppendFormat(SmoApplication.DefaultCulture, "IMPORTANCE={0}", typeConverter.ConvertToInvariantString(propertyOptional.Value));
		}
	}

	private void GetAllParams(StringBuilder sb, ScriptingPreferences so, ref int count)
	{
		StringBuilder stringBuilder = new StringBuilder(Globals.INIT_BUFFER_SIZE);
		GetParameter(stringBuilder, so, "GroupName", "WORKLOAD_GROUP='{0}'", ref count);
		GetParameter(stringBuilder, so, "MemberName", "MEMBERNAME='{0}'", ref count);
		GetParameter(stringBuilder, so, "WlmLabel", "WLM_LABEL='{0}'", ref count);
		GetParameter(stringBuilder, so, "WlmContext", "WLM_CONTEXT='{0}'", ref count);
		GetParameter(stringBuilder, so, "StartTime", "START_TIME='{0}'", ref count);
		GetParameter(stringBuilder, so, "EndTime", "END_TIME='{0}'", ref count);
		GetImportanceTSql(stringBuilder, ref count);
		if (0 < count)
		{
			sb.AppendFormat(SmoApplication.DefaultCulture, " WITH({0})", stringBuilder.ToString());
		}
	}

	public WorkloadManagementWorkloadClassifier()
	{
	}

	public WorkloadManagementWorkloadClassifier(Database database, string name)
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
