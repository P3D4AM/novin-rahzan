using System;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.Smo.Agent;

public sealed class TargetServer : AgentObjectBase
{
	internal class PropertyMetadataProvider : SqlPropertyMetadataProvider
	{
		private static int[] versionCount = new int[10] { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 };

		private static int[] cloudVersionCount = new int[3];

		private static int sqlDwPropertyCount = 0;

		internal static StaticMetadata[] sqlDwStaticMetadata = new StaticMetadata[0];

		internal static StaticMetadata[] cloudStaticMetadata = new StaticMetadata[0];

		internal static StaticMetadata[] staticMetadata = new StaticMetadata[9]
		{
			new StaticMetadata("EnlistDate", expensive: false, readOnly: true, typeof(DateTime)),
			new StaticMetadata("ID", expensive: false, readOnly: true, typeof(int)),
			new StaticMetadata("LastPollDate", expensive: false, readOnly: true, typeof(DateTime)),
			new StaticMetadata("LocalTime", expensive: false, readOnly: true, typeof(DateTime)),
			new StaticMetadata("Location", expensive: false, readOnly: true, typeof(string)),
			new StaticMetadata("PendingInstructions", expensive: false, readOnly: true, typeof(int)),
			new StaticMetadata("PollingInterval", expensive: false, readOnly: true, typeof(int)),
			new StaticMetadata("Status", expensive: false, readOnly: true, typeof(TargetServerStatus)),
			new StaticMetadata("TimeZoneAdjustment", expensive: false, readOnly: true, typeof(int))
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
				"EnlistDate" => 0, 
				"ID" => 1, 
				"LastPollDate" => 2, 
				"LocalTime" => 3, 
				"Location" => 4, 
				"PendingInstructions" => 5, 
				"PollingInterval" => 6, 
				"Status" => 7, 
				"TimeZoneAdjustment" => 8, 
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

	public static string UrnSuffix => "TargetServer";

	[SfcObject(SfcObjectRelationship.ParentObject)]
	public JobServer Parent
	{
		get
		{
			CheckObjectState();
			return base.ParentColl.ParentInstance as JobServer;
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public DateTime EnlistDate => (DateTime)base.Properties.GetValueWithNullReplacement("EnlistDate");

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public int ID => (int)base.Properties.GetValueWithNullReplacement("ID");

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public DateTime LastPollDate => (DateTime)base.Properties.GetValueWithNullReplacement("LastPollDate");

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public DateTime LocalTime => (DateTime)base.Properties.GetValueWithNullReplacement("LocalTime");

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public string Location => (string)base.Properties.GetValueWithNullReplacement("Location");

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public int PendingInstructions => (int)base.Properties.GetValueWithNullReplacement("PendingInstructions");

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public int PollingInterval => (int)base.Properties.GetValueWithNullReplacement("PollingInterval");

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public TargetServerStatus Status => (TargetServerStatus)base.Properties.GetValueWithNullReplacement("Status");

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public int TimeZoneAdjustment => (int)base.Properties.GetValueWithNullReplacement("TimeZoneAdjustment");

	internal TargetServer(AbstractCollectionBase parentColl, ObjectKeyBase key, SqlSmoState state)
		: base(parentColl, key, state)
	{
	}

	internal override SqlPropertyMetadataProvider GetPropertyMetadataProvider()
	{
		return new PropertyMetadataProvider(base.ServerVersion, DatabaseEngineType, DatabaseEngineEdition);
	}
}
