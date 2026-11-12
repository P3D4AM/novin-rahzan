using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.Smo;

[SfcElementType("QueryStoreOptions")]
[SfcElement(SfcElementFlags.Standalone | SfcElementFlags.SqlAzureDatabase)]
public sealed class QueryStoreOptions : SqlSmoObject, IAlterable, IScriptable, ISfcSupportsDesignMode
{
	internal class PropertyMetadataProvider : SqlPropertyMetadataProvider
	{
		private static int[] versionCount = new int[10] { 0, 0, 0, 0, 0, 0, 0, 11, 12, 16 };

		private static int[] cloudVersionCount = new int[3] { 0, 0, 12 };

		private static int sqlDwPropertyCount = 0;

		internal static StaticMetadata[] sqlDwStaticMetadata = new StaticMetadata[0];

		internal static StaticMetadata[] cloudStaticMetadata = new StaticMetadata[12]
		{
			new StaticMetadata("ActualState", expensive: false, readOnly: true, typeof(QueryStoreOperationMode)),
			new StaticMetadata("CurrentStorageSizeInMB", expensive: false, readOnly: false, typeof(long)),
			new StaticMetadata("DataFlushIntervalInSeconds", expensive: false, readOnly: false, typeof(long)),
			new StaticMetadata("DesiredState", expensive: false, readOnly: false, typeof(QueryStoreOperationMode)),
			new StaticMetadata("MaxPlansPerQuery", expensive: false, readOnly: false, typeof(long)),
			new StaticMetadata("MaxStorageSizeInMB", expensive: false, readOnly: false, typeof(long)),
			new StaticMetadata("QueryCaptureMode", expensive: false, readOnly: false, typeof(QueryStoreCaptureMode)),
			new StaticMetadata("ReadOnlyReason", expensive: false, readOnly: false, typeof(int)),
			new StaticMetadata("SizeBasedCleanupMode", expensive: false, readOnly: false, typeof(QueryStoreSizeBasedCleanupMode)),
			new StaticMetadata("StaleQueryThresholdInDays", expensive: false, readOnly: false, typeof(long)),
			new StaticMetadata("StatisticsCollectionIntervalInMinutes", expensive: false, readOnly: false, typeof(long)),
			new StaticMetadata("WaitStatsCaptureMode", expensive: false, readOnly: false, typeof(QueryStoreWaitStatsCaptureMode))
		};

		internal static StaticMetadata[] staticMetadata = new StaticMetadata[16]
		{
			new StaticMetadata("ActualState", expensive: false, readOnly: true, typeof(QueryStoreOperationMode)),
			new StaticMetadata("CurrentStorageSizeInMB", expensive: false, readOnly: false, typeof(long)),
			new StaticMetadata("DataFlushIntervalInSeconds", expensive: false, readOnly: false, typeof(long)),
			new StaticMetadata("DesiredState", expensive: false, readOnly: false, typeof(QueryStoreOperationMode)),
			new StaticMetadata("MaxPlansPerQuery", expensive: false, readOnly: false, typeof(long)),
			new StaticMetadata("MaxStorageSizeInMB", expensive: false, readOnly: false, typeof(long)),
			new StaticMetadata("QueryCaptureMode", expensive: false, readOnly: false, typeof(QueryStoreCaptureMode)),
			new StaticMetadata("ReadOnlyReason", expensive: false, readOnly: false, typeof(int)),
			new StaticMetadata("SizeBasedCleanupMode", expensive: false, readOnly: false, typeof(QueryStoreSizeBasedCleanupMode)),
			new StaticMetadata("StaleQueryThresholdInDays", expensive: false, readOnly: false, typeof(long)),
			new StaticMetadata("StatisticsCollectionIntervalInMinutes", expensive: false, readOnly: false, typeof(long)),
			new StaticMetadata("WaitStatsCaptureMode", expensive: false, readOnly: false, typeof(QueryStoreWaitStatsCaptureMode)),
			new StaticMetadata("CapturePolicyExecutionCount", expensive: false, readOnly: false, typeof(int)),
			new StaticMetadata("CapturePolicyStaleThresholdInHrs", expensive: false, readOnly: false, typeof(int)),
			new StaticMetadata("CapturePolicyTotalCompileCpuTimeInMS", expensive: false, readOnly: false, typeof(long)),
			new StaticMetadata("CapturePolicyTotalExecutionCpuTimeInMS", expensive: false, readOnly: false, typeof(long))
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
				if (base.DatabaseEngineEdition == DatabaseEngineEdition.SqlDataWarehouse)
				{
					return -1;
				}
				return propertyName switch
				{
					"ActualState" => 0, 
					"CurrentStorageSizeInMB" => 1, 
					"DataFlushIntervalInSeconds" => 2, 
					"DesiredState" => 3, 
					"MaxPlansPerQuery" => 4, 
					"MaxStorageSizeInMB" => 5, 
					"QueryCaptureMode" => 6, 
					"ReadOnlyReason" => 7, 
					"SizeBasedCleanupMode" => 8, 
					"StaleQueryThresholdInDays" => 9, 
					"StatisticsCollectionIntervalInMinutes" => 10, 
					"WaitStatsCaptureMode" => 11, 
					_ => -1, 
				};
			}
			return propertyName switch
			{
				"ActualState" => 0, 
				"CurrentStorageSizeInMB" => 1, 
				"DataFlushIntervalInSeconds" => 2, 
				"DesiredState" => 3, 
				"MaxPlansPerQuery" => 4, 
				"MaxStorageSizeInMB" => 5, 
				"QueryCaptureMode" => 6, 
				"ReadOnlyReason" => 7, 
				"SizeBasedCleanupMode" => 8, 
				"StaleQueryThresholdInDays" => 9, 
				"StatisticsCollectionIntervalInMinutes" => 10, 
				"WaitStatsCaptureMode" => 11, 
				"CapturePolicyExecutionCount" => 12, 
				"CapturePolicyStaleThresholdInHrs" => 13, 
				"CapturePolicyTotalCompileCpuTimeInMS" => 14, 
				"CapturePolicyTotalExecutionCpuTimeInMS" => 15, 
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

	internal static string UrnSuffix => "QueryStoreOptions";

	[SfcObject(SfcObjectRelationship.ParentObject)]
	public Database Parent => singletonParent as Database;

	bool ISfcSupportsDesignMode.IsDesignMode => base.IsDesignMode;

	[SfcProperty(SfcPropertyFlags.Standalone | SfcPropertyFlags.SqlAzureDatabase)]
	public QueryStoreOperationMode ActualState => (QueryStoreOperationMode)base.Properties.GetValueWithNullReplacement("ActualState");

	[SfcProperty(SfcPropertyFlags.Standalone | SfcPropertyFlags.Deploy)]
	public int CapturePolicyExecutionCount
	{
		get
		{
			return (int)base.Properties.GetValueWithNullReplacement("CapturePolicyExecutionCount");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("CapturePolicyExecutionCount", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone | SfcPropertyFlags.Deploy)]
	public int CapturePolicyStaleThresholdInHrs
	{
		get
		{
			return (int)base.Properties.GetValueWithNullReplacement("CapturePolicyStaleThresholdInHrs");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("CapturePolicyStaleThresholdInHrs", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone | SfcPropertyFlags.Deploy)]
	public long CapturePolicyTotalCompileCpuTimeInMS
	{
		get
		{
			return (long)base.Properties.GetValueWithNullReplacement("CapturePolicyTotalCompileCpuTimeInMS");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("CapturePolicyTotalCompileCpuTimeInMS", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone | SfcPropertyFlags.Deploy)]
	public long CapturePolicyTotalExecutionCpuTimeInMS
	{
		get
		{
			return (long)base.Properties.GetValueWithNullReplacement("CapturePolicyTotalExecutionCpuTimeInMS");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("CapturePolicyTotalExecutionCpuTimeInMS", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone | SfcPropertyFlags.SqlAzureDatabase | SfcPropertyFlags.Deploy)]
	public long CurrentStorageSizeInMB
	{
		get
		{
			return (long)base.Properties.GetValueWithNullReplacement("CurrentStorageSizeInMB");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("CurrentStorageSizeInMB", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone | SfcPropertyFlags.SqlAzureDatabase | SfcPropertyFlags.Deploy)]
	public long DataFlushIntervalInSeconds
	{
		get
		{
			return (long)base.Properties.GetValueWithNullReplacement("DataFlushIntervalInSeconds");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("DataFlushIntervalInSeconds", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone | SfcPropertyFlags.SqlAzureDatabase | SfcPropertyFlags.Deploy)]
	public QueryStoreOperationMode DesiredState
	{
		get
		{
			return (QueryStoreOperationMode)base.Properties.GetValueWithNullReplacement("DesiredState");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("DesiredState", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone | SfcPropertyFlags.SqlAzureDatabase | SfcPropertyFlags.Deploy)]
	public long MaxPlansPerQuery
	{
		get
		{
			return (long)base.Properties.GetValueWithNullReplacement("MaxPlansPerQuery");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("MaxPlansPerQuery", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone | SfcPropertyFlags.SqlAzureDatabase | SfcPropertyFlags.Deploy)]
	public long MaxStorageSizeInMB
	{
		get
		{
			return (long)base.Properties.GetValueWithNullReplacement("MaxStorageSizeInMB");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("MaxStorageSizeInMB", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone | SfcPropertyFlags.SqlAzureDatabase | SfcPropertyFlags.Deploy)]
	public QueryStoreCaptureMode QueryCaptureMode
	{
		get
		{
			return (QueryStoreCaptureMode)base.Properties.GetValueWithNullReplacement("QueryCaptureMode");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("QueryCaptureMode", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone | SfcPropertyFlags.SqlAzureDatabase)]
	public int ReadOnlyReason
	{
		get
		{
			return (int)base.Properties.GetValueWithNullReplacement("ReadOnlyReason");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("ReadOnlyReason", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone | SfcPropertyFlags.SqlAzureDatabase | SfcPropertyFlags.Deploy)]
	public QueryStoreSizeBasedCleanupMode SizeBasedCleanupMode
	{
		get
		{
			return (QueryStoreSizeBasedCleanupMode)base.Properties.GetValueWithNullReplacement("SizeBasedCleanupMode");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("SizeBasedCleanupMode", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone | SfcPropertyFlags.SqlAzureDatabase | SfcPropertyFlags.Deploy)]
	public long StaleQueryThresholdInDays
	{
		get
		{
			return (long)base.Properties.GetValueWithNullReplacement("StaleQueryThresholdInDays");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("StaleQueryThresholdInDays", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone | SfcPropertyFlags.SqlAzureDatabase | SfcPropertyFlags.Deploy)]
	public long StatisticsCollectionIntervalInMinutes
	{
		get
		{
			return (long)base.Properties.GetValueWithNullReplacement("StatisticsCollectionIntervalInMinutes");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("StatisticsCollectionIntervalInMinutes", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone | SfcPropertyFlags.SqlAzureDatabase | SfcPropertyFlags.Deploy)]
	public QueryStoreWaitStatsCaptureMode WaitStatsCaptureMode
	{
		get
		{
			return (QueryStoreWaitStatsCaptureMode)base.Properties.GetValueWithNullReplacement("WaitStatsCaptureMode");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("WaitStatsCaptureMode", value);
		}
	}

	internal QueryStoreOptions(Database parentdb, ObjectKeyBase key, SqlSmoState state)
		: base(key, state)
	{
		singletonParent = parentdb;
	}

	protected sealed override void GetUrnRecursive(StringBuilder urnbuilder, UrnIdOption idOption)
	{
		Parent.GetUrnRecImpl(urnbuilder, idOption);
		urnbuilder.AppendFormat(SmoApplication.DefaultCulture, "/{0}", UrnSuffix);
	}

	internal override void ScriptAlter(StringCollection query, ScriptingPreferences sp)
	{
		if (Parent.Status == DatabaseStatus.Restoring)
		{
			Property property = base.Properties.Get("DesiredState");
			Property property2 = base.Properties.Get("ActualState");
			Property property3 = base.Properties.Get("StaleQueryThresholdInDays");
			Property property4 = base.Properties.Get("DataFlushIntervalInSeconds");
			Property property5 = base.Properties.Get("StatisticsCollectionIntervalInMinutes");
			Property property6 = base.Properties.Get("MaxStorageSizeInMB");
			Property property7 = base.Properties.Get("QueryCaptureMode");
			Property property8 = base.Properties.Get("SizeBasedCleanupMode");
			Property property9 = (IsSupportedProperty("MaxPlansPerQuery") ? base.Properties.Get("MaxPlansPerQuery") : null);
			Property property10 = (IsSupportedProperty("WaitStatsCaptureMode") ? base.Properties.Get("WaitStatsCaptureMode") : null);
			Property property11 = (IsSupportedProperty("CapturePolicyExecutionCount") ? base.Properties.Get("CapturePolicyExecutionCount") : null);
			Property property12 = (IsSupportedProperty("CapturePolicyTotalCompileCpuTimeInMS") ? base.Properties.Get("CapturePolicyTotalCompileCpuTimeInMS") : null);
			Property property13 = (IsSupportedProperty("CapturePolicyTotalExecutionCpuTimeInMS") ? base.Properties.Get("CapturePolicyTotalExecutionCpuTimeInMS") : null);
			Property property14 = (IsSupportedProperty("CapturePolicyStaleThresholdInHrs") ? base.Properties.Get("CapturePolicyStaleThresholdInHrs") : null);
			if ((property != null && property.Dirty) || (property2 != null && property2.Dirty) || (property3 != null && property3.Dirty) || (property4 != null && property4.Dirty) || (property5 != null && property5.Dirty) || (property6 != null && property6.Dirty) || (property7 != null && property7.Dirty) || (property8 != null && property8.Dirty) || (property9 != null && property9.Dirty) || (property10 != null && property10.Dirty) || (property11 != null && property11.Dirty) || (property12 != null && property12.Dirty) || (property13 != null && property13.Dirty) || (property14 != null && property14.Dirty))
			{
				throw new InvalidSmoOperationException(ExceptionTemplatesImpl.AlterQueryStorePropertyForRestoringDatabase);
			}
		}
		else
		{
			ScriptQueryStoreOptions(query, sp, scriptAll: false);
		}
	}

	internal override void ScriptCreate(StringCollection query, ScriptingPreferences sp)
	{
		ScriptQueryStoreOptions(query, sp, scriptAll: true);
	}

	private void ScriptQueryStoreOptions(StringCollection query, ScriptingPreferences sp, bool scriptAll)
	{
		CheckObjectState();
		this.ThrowIfNotSupported(GetType(), sp);
		StringBuilder stringBuilder = new StringBuilder();
		Property propertyOptional = GetPropertyOptional("DesiredState");
		if (propertyOptional.Value == null)
		{
			return;
		}
		Property propertyOptional2 = GetPropertyOptional("ActualState");
		Property propertyOptional3 = GetPropertyOptional("StaleQueryThresholdInDays");
		Property propertyOptional4 = GetPropertyOptional("DataFlushIntervalInSeconds");
		Property propertyOptional5 = GetPropertyOptional("StatisticsCollectionIntervalInMinutes");
		Property propertyOptional6 = GetPropertyOptional("MaxStorageSizeInMB");
		Property propertyOptional7 = GetPropertyOptional("QueryCaptureMode");
		Property propertyOptional8 = GetPropertyOptional("SizeBasedCleanupMode");
		Property property = (IsSupportedProperty("MaxPlansPerQuery") ? GetPropertyOptional("MaxPlansPerQuery") : null);
		Property property2 = (IsSupportedProperty("WaitStatsCaptureMode") ? GetPropertyOptional("WaitStatsCaptureMode") : null);
		Property property3 = (IsSupportedProperty("CapturePolicyExecutionCount") ? GetPropertyOptional("CapturePolicyExecutionCount") : null);
		Property property4 = (IsSupportedProperty("CapturePolicyTotalCompileCpuTimeInMS") ? GetPropertyOptional("CapturePolicyTotalCompileCpuTimeInMS") : null);
		Property property5 = (IsSupportedProperty("CapturePolicyTotalExecutionCpuTimeInMS") ? GetPropertyOptional("CapturePolicyTotalExecutionCpuTimeInMS") : null);
		Property property6 = (IsSupportedProperty("CapturePolicyStaleThresholdInHrs") ? GetPropertyOptional("CapturePolicyStaleThresholdInHrs") : null);
		TypeConverter typeConverter = SmoManagementUtil.GetTypeConverter(typeof(QueryStoreOperationMode));
		TypeConverter typeConverter2 = SmoManagementUtil.GetTypeConverter(typeof(QueryStoreCaptureMode));
		TypeConverter typeConverter3 = SmoManagementUtil.GetTypeConverter(typeof(QueryStoreSizeBasedCleanupMode));
		TypeConverter typeConverter4 = SmoManagementUtil.GetTypeConverter(typeof(QueryStoreWaitStatsCaptureMode));
		QueryStoreOperationMode queryStoreOperationMode = (QueryStoreOperationMode)propertyOptional.Value;
		QueryStoreOperationMode queryStoreOperationMode2 = ((!propertyOptional2.IsNull) ? ((QueryStoreOperationMode)propertyOptional2.Value) : QueryStoreOperationMode.Off);
		if (scriptAll || (propertyOptional.Dirty && queryStoreOperationMode == QueryStoreOperationMode.Off != (queryStoreOperationMode2 == QueryStoreOperationMode.Off)))
		{
			stringBuilder.Append(string.Format(SmoApplication.DefaultCulture, "ALTER DATABASE [{0}] SET QUERY_STORE = {1}", SqlSmoObject.SqlBraket(Parent.Name), (queryStoreOperationMode != 0) ? "ON" : "OFF"));
			query.Add(stringBuilder.ToString());
		}
		if (queryStoreOperationMode == QueryStoreOperationMode.Off)
		{
			return;
		}
		stringBuilder = new StringBuilder();
		if (scriptAll || propertyOptional.Dirty || propertyOptional3.Dirty || propertyOptional4.Dirty || propertyOptional5.Dirty || propertyOptional6.Dirty || propertyOptional7.Dirty || propertyOptional8.Dirty || (property != null && property.Dirty) || (property2 != null && property2.Dirty) || (property3 != null && property3.Dirty) || (property4 != null && property4.Dirty) || (property5 != null && property5.Dirty) || (property6 != null && property6.Dirty))
		{
			stringBuilder.Append(string.Format(SmoApplication.DefaultCulture, "ALTER DATABASE [{0}] SET QUERY_STORE (", SqlSmoObject.SqlBraket(Parent.Name)));
			stringBuilder.Append(string.Format(SmoApplication.DefaultCulture, "OPERATION_MODE = {0}, ", typeConverter.ConvertToInvariantString(queryStoreOperationMode)));
			if (scriptAll || propertyOptional3.Dirty)
			{
				stringBuilder.Append(string.Format(SmoApplication.DefaultCulture, "CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = {0}), ", (long)propertyOptional3.Value));
			}
			if (scriptAll || propertyOptional4.Dirty)
			{
				stringBuilder.Append(string.Format(SmoApplication.DefaultCulture, "DATA_FLUSH_INTERVAL_SECONDS = {0}, ", (long)propertyOptional4.Value));
			}
			if (scriptAll || propertyOptional5.Dirty)
			{
				stringBuilder.Append(string.Format(SmoApplication.DefaultCulture, "INTERVAL_LENGTH_MINUTES = {0}, ", (long)propertyOptional5.Value));
			}
			if (scriptAll || propertyOptional6.Dirty)
			{
				stringBuilder.Append(string.Format(SmoApplication.DefaultCulture, "MAX_STORAGE_SIZE_MB = {0}, ", (long)propertyOptional6.Value));
			}
			QueryStoreCaptureMode queryStoreCaptureMode = (QueryStoreCaptureMode)propertyOptional7.Value;
			if (queryStoreCaptureMode == QueryStoreCaptureMode.Custom && !IsSupportedProperty("CapturePolicyExecutionCount", sp))
			{
				queryStoreCaptureMode = QueryStoreCaptureMode.Auto;
			}
			if (scriptAll || propertyOptional7.Dirty)
			{
				stringBuilder.Append(string.Format(SmoApplication.DefaultCulture, "QUERY_CAPTURE_MODE = {0}, ", typeConverter2.ConvertToInvariantString(queryStoreCaptureMode)));
			}
			if (queryStoreCaptureMode == QueryStoreCaptureMode.Custom)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				if (scriptAll || property6.Dirty)
				{
					stringBuilder2.Append(string.Format(SmoApplication.DefaultCulture, "STALE_CAPTURE_POLICY_THRESHOLD = {0} HOURS, ", (int)property6.Value));
				}
				if (scriptAll || property3.Dirty)
				{
					stringBuilder2.Append(string.Format(SmoApplication.DefaultCulture, "EXECUTION_COUNT = {0}, ", (int)property3.Value));
				}
				if (scriptAll || property4.Dirty)
				{
					stringBuilder2.Append(string.Format(SmoApplication.DefaultCulture, "TOTAL_COMPILE_CPU_TIME_MS = {0}, ", (long)property4.Value));
				}
				if (scriptAll || property5.Dirty)
				{
					stringBuilder2.Append(string.Format(SmoApplication.DefaultCulture, "TOTAL_EXECUTION_CPU_TIME_MS = {0}, ", (long)property5.Value));
				}
				if (stringBuilder2.Length > 0)
				{
					stringBuilder2.Length -= 2;
					stringBuilder.Append(string.Format(SmoApplication.DefaultCulture, "QUERY_CAPTURE_POLICY = ("));
					stringBuilder.Append(stringBuilder2.ToString());
					stringBuilder.Append("), ");
				}
			}
			if (scriptAll || propertyOptional8.Dirty)
			{
				stringBuilder.Append(string.Format(SmoApplication.DefaultCulture, "SIZE_BASED_CLEANUP_MODE = {0}, ", typeConverter3.ConvertToInvariantString(propertyOptional8.Value)));
			}
			if (IsSupportedProperty("MaxPlansPerQuery", sp) && (scriptAll || property.Dirty))
			{
				stringBuilder.Append(string.Format(SmoApplication.DefaultCulture, "MAX_PLANS_PER_QUERY = {0}, ", (long)property.Value));
			}
			if (IsSupportedProperty("WaitStatsCaptureMode", sp) && (scriptAll || property2.Dirty))
			{
				stringBuilder.Append(string.Format(SmoApplication.DefaultCulture, "WAIT_STATS_CAPTURE_MODE = {0}, ", typeConverter4.ConvertToInvariantString(property2.Value)));
			}
			stringBuilder.Length -= 2;
			stringBuilder.Append(")");
		}
		query.Add(stringBuilder.ToString());
	}

	public void Alter()
	{
		AlterImpl();
	}

	public StringCollection Script()
	{
		return ScriptImpl();
	}

	public StringCollection Script(ScriptingOptions scriptingOptions)
	{
		return ScriptImpl(scriptingOptions);
	}

	public void PurgeQueryStoreData()
	{
		try
		{
			CheckObjectState();
			this.ThrowIfNotSupported(typeof(QueryStoreOptions));
			Parent.ExecuteNonQuery(string.Format(SmoApplication.DefaultCulture, "ALTER DATABASE [{0}] SET QUERY_STORE CLEAR ALL", SqlSmoObject.SqlBraket(Parent.Name)));
		}
		catch (Exception ex)
		{
			SqlSmoObject.FilterException(ex);
			throw new FailedOperationException(ExceptionTemplatesImpl.SetQueryStoreOptions, Parent, ex);
		}
	}

	internal static string[] GetScriptFields(Type parentType, ServerVersion version, DatabaseEngineType databaseEngineType, DatabaseEngineEdition databaseEngineEdition, bool defaultTextMode)
	{
		string[] fields = new string[14]
		{
			"ActualState", "DataFlushIntervalInSeconds", "DesiredState", "MaxStorageSizeInMB", "QueryCaptureMode", "SizeBasedCleanupMode", "StaleQueryThresholdInDays", "StatisticsCollectionIntervalInMinutes", "MaxPlansPerQuery", "WaitStatsCaptureMode",
			"CapturePolicyExecutionCount", "CapturePolicyTotalCompileCpuTimeInMS", "CapturePolicyTotalExecutionCpuTimeInMS", "CapturePolicyStaleThresholdInHrs"
		};
		return SqlSmoObject.GetSupportedScriptFields(typeof(PropertyMetadataProvider), fields, version, databaseEngineType, databaseEngineEdition).ToArray();
	}

	internal override SqlPropertyMetadataProvider GetPropertyMetadataProvider()
	{
		return new PropertyMetadataProvider(base.ServerVersion, DatabaseEngineType, DatabaseEngineEdition);
	}
}
