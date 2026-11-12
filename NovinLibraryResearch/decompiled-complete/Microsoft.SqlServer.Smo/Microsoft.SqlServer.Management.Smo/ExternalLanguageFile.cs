using System;
using System.Data;
using System.Globalization;
using System.Text;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.Smo;

[SfcElement(SfcElementFlags.Standalone)]
public sealed class ExternalLanguageFile : ScriptNameObjectBase, ISfcSupportsDesignMode
{
	internal class PropertyMetadataProvider : SqlPropertyMetadataProvider
	{
		private static int[] versionCount = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 7 };

		private static int[] cloudVersionCount = new int[3];

		private static int sqlDwPropertyCount = 0;

		internal static StaticMetadata[] sqlDwStaticMetadata = new StaticMetadata[0];

		internal static StaticMetadata[] cloudStaticMetadata = new StaticMetadata[0];

		internal static StaticMetadata[] staticMetadata = new StaticMetadata[7]
		{
			new StaticMetadata("ContentFromBinary", expensive: false, readOnly: false, typeof(byte[])),
			new StaticMetadata("ContentFromFile", expensive: false, readOnly: false, typeof(string)),
			new StaticMetadata("EnvironmentVariables", expensive: false, readOnly: true, typeof(string)),
			new StaticMetadata("FileName", expensive: false, readOnly: false, typeof(string)),
			new StaticMetadata("IsSystemNamed", expensive: false, readOnly: true, typeof(bool)),
			new StaticMetadata("Parameters", expensive: false, readOnly: true, typeof(string)),
			new StaticMetadata("Platform", expensive: false, readOnly: false, typeof(ExternalLanguageFilePlatform))
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
				"ContentFromBinary" => 0, 
				"ContentFromFile" => 1, 
				"EnvironmentVariables" => 2, 
				"FileName" => 3, 
				"IsSystemNamed" => 4, 
				"Parameters" => 5, 
				"Platform" => 6, 
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

	public static string UrnSuffix => "ExternalLanguageFile";

	[SfcObject(SfcObjectRelationship.ParentObject)]
	public ExternalLanguage Parent
	{
		get
		{
			CheckObjectState();
			return base.ParentColl.ParentInstance as ExternalLanguage;
		}
		set
		{
			SetParentImpl(value);
		}
	}

	bool ISfcSupportsDesignMode.IsDesignMode => base.IsDesignMode;

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public byte[] ContentFromBinary
	{
		get
		{
			return (byte[])base.Properties.GetValueWithNullReplacement("ContentFromBinary");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("ContentFromBinary", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public string ContentFromFile
	{
		get
		{
			return (string)base.Properties.GetValueWithNullReplacement("ContentFromFile");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("ContentFromFile", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public string EnvironmentVariables => (string)base.Properties.GetValueWithNullReplacement("EnvironmentVariables");

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public string FileName
	{
		get
		{
			return (string)base.Properties.GetValueWithNullReplacement("FileName");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("FileName", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public bool IsSystemNamed => (bool)base.Properties.GetValueWithNullReplacement("IsSystemNamed");

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public string Parameters => (string)base.Properties.GetValueWithNullReplacement("Parameters");

	[SfcProperty(SfcPropertyFlags.Standalone)]
	public ExternalLanguageFilePlatform Platform
	{
		get
		{
			return (ExternalLanguageFilePlatform)base.Properties.GetValueWithNullReplacement("Platform");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("Platform", value);
		}
	}

	internal ExternalLanguageFile(AbstractCollectionBase parentColl, ObjectKeyBase key, SqlSmoState state)
		: base(parentColl, key, state)
	{
	}

	public byte[] GetFileBytes()
	{
		Request req = new Request(base.Urn, new string[1] { "ContentFromBinary" });
		DataTable enumeratorData = ExecutionManager.GetEnumeratorData(req);
		if (enumeratorData.Rows.Count > 0)
		{
			return (byte[])enumeratorData.Rows[0][0];
		}
		return new byte[0];
	}

	public string GetFileName()
	{
		Request req = new Request(base.Urn, new string[1] { "FileName" });
		DataTable enumeratorData = ExecutionManager.GetEnumeratorData(req);
		if (enumeratorData.Rows.Count > 0)
		{
			return enumeratorData.Rows[0][0].ToString();
		}
		return string.Empty;
	}

	public string GetFileText()
	{
		byte[] fileBytes = GetFileBytes();
		StringBuilder stringBuilder = new StringBuilder(fileBytes.Length + 2);
		stringBuilder.Append("0x");
		byte[] array = fileBytes;
		foreach (byte b in array)
		{
			stringBuilder.Append(b.ToString("X2", CultureInfo.InvariantCulture));
		}
		return stringBuilder.ToString();
	}

	internal static string[] GetScriptFields(Type parentType, ServerVersion version, DatabaseEngineType databaseEngineType, DatabaseEngineEdition databaseEngineEdition, bool defaultTextMode)
	{
		return new string[6] { "ContentFromFile", "ContentFromBinary", "FileName", "Parameters", "Platform", "EnvironmentVariables" };
	}

	public ExternalLanguageFile()
	{
	}

	public ExternalLanguageFile(ExternalLanguage externalLanguage, string name)
	{
		ValidateName(name);
		key = new SimpleObjectKey(name);
		Parent = externalLanguage;
	}

	internal override SqlPropertyMetadataProvider GetPropertyMetadataProvider()
	{
		return new PropertyMetadataProvider(base.ServerVersion, DatabaseEngineType, DatabaseEngineEdition);
	}
}
