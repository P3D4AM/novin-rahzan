using System;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.Smo;

[SfcElement(SfcElementFlags.Standalone | SfcElementFlags.SqlAzureDatabase)]
public sealed class IndexedColumn : NamedSmoObject, ISfcSupportsDesignMode, IPropertyDataDispatch
{
	internal class PropertyMetadataProvider : SqlPropertyMetadataProvider
	{
		private static int[] versionCount = new int[10] { 2, 3, 4, 4, 4, 4, 4, 4, 4, 5 };

		private static int[] cloudVersionCount = new int[3] { 4, 4, 5 };

		private static int sqlDwPropertyCount = 4;

		internal static StaticMetadata[] sqlDwStaticMetadata = new StaticMetadata[4]
		{
			new StaticMetadata("ColumnStoreOrderOrdinal", expensive: false, readOnly: true, typeof(int)),
			new StaticMetadata("Descending", expensive: false, readOnly: false, typeof(bool)),
			new StaticMetadata("ID", expensive: false, readOnly: true, typeof(int)),
			new StaticMetadata("IsIncluded", expensive: false, readOnly: false, typeof(bool))
		};

		internal static StaticMetadata[] cloudStaticMetadata = new StaticMetadata[5]
		{
			new StaticMetadata("Descending", expensive: false, readOnly: false, typeof(bool)),
			new StaticMetadata("ID", expensive: false, readOnly: true, typeof(int)),
			new StaticMetadata("IsComputed", expensive: false, readOnly: true, typeof(bool)),
			new StaticMetadata("IsIncluded", expensive: false, readOnly: false, typeof(bool)),
			new StaticMetadata("ColumnStoreOrderOrdinal", expensive: false, readOnly: true, typeof(int))
		};

		internal static StaticMetadata[] staticMetadata = new StaticMetadata[5]
		{
			new StaticMetadata("ID", expensive: false, readOnly: true, typeof(int)),
			new StaticMetadata("IsComputed", expensive: false, readOnly: true, typeof(bool)),
			new StaticMetadata("Descending", expensive: false, readOnly: false, typeof(bool)),
			new StaticMetadata("IsIncluded", expensive: false, readOnly: false, typeof(bool)),
			new StaticMetadata("ColumnStoreOrderOrdinal", expensive: false, readOnly: true, typeof(int))
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
					return propertyName switch
					{
						"ColumnStoreOrderOrdinal" => 0, 
						"Descending" => 1, 
						"ID" => 2, 
						"IsIncluded" => 3, 
						_ => -1, 
					};
				}
				return propertyName switch
				{
					"Descending" => 0, 
					"ID" => 1, 
					"IsComputed" => 2, 
					"IsIncluded" => 3, 
					"ColumnStoreOrderOrdinal" => 4, 
					_ => -1, 
				};
			}
			return propertyName switch
			{
				"ID" => 0, 
				"IsComputed" => 1, 
				"Descending" => 2, 
				"IsIncluded" => 3, 
				"ColumnStoreOrderOrdinal" => 4, 
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

	private sealed class XSchemaProps
	{
		private int _ColumnStoreOrderOrdinal;

		private bool _Descending;

		private bool _IsComputed;

		private bool _IsIncluded;

		internal int ColumnStoreOrderOrdinal
		{
			get
			{
				return _ColumnStoreOrderOrdinal;
			}
			set
			{
				_ColumnStoreOrderOrdinal = value;
			}
		}

		internal bool Descending
		{
			get
			{
				return _Descending;
			}
			set
			{
				_Descending = value;
			}
		}

		internal bool IsComputed
		{
			get
			{
				return _IsComputed;
			}
			set
			{
				_IsComputed = value;
			}
		}

		internal bool IsIncluded
		{
			get
			{
				return _IsIncluded;
			}
			set
			{
				_IsIncluded = value;
			}
		}
	}

	private sealed class XRuntimeProps
	{
		private int _ID;

		internal int ID
		{
			get
			{
				return _ID;
			}
			set
			{
				_ID = value;
			}
		}
	}

	private XSchemaProps _XSchema;

	private XRuntimeProps _XRuntime;

	public static string UrnSuffix => "IndexedColumn";

	[SfcKey(0)]
	[SfcProperty(SfcPropertyFlags.ReadOnlyAfterCreation | SfcPropertyFlags.Standalone | SfcPropertyFlags.SqlAzureDatabase | SfcPropertyFlags.Design | SfcPropertyFlags.Deploy)]
	public override string Name
	{
		get
		{
			return base.Name;
		}
		set
		{
			base.Name = value;
		}
	}

	[SfcObject(SfcObjectRelationship.ParentObject, SfcObjectFlags.Design | SfcObjectFlags.Deploy)]
	public Index Parent
	{
		get
		{
			CheckObjectState();
			return base.ParentColl.ParentInstance as Index;
		}
		set
		{
			SetParentImpl(value);
		}
	}

	bool ISfcSupportsDesignMode.IsDesignMode => base.IsDesignMode;

	private XSchemaProps XSchema
	{
		get
		{
			if (_XSchema == null)
			{
				_XSchema = new XSchemaProps();
			}
			return _XSchema;
		}
	}

	private XRuntimeProps XRuntime
	{
		get
		{
			if (_XRuntime == null)
			{
				_XRuntime = new XRuntimeProps();
			}
			return _XRuntime;
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone | SfcPropertyFlags.SqlAzureDatabase)]
	public int ColumnStoreOrderOrdinal => (int)base.Properties.GetValueWithNullReplacement("ColumnStoreOrderOrdinal");

	[SfcProperty(SfcPropertyFlags.Standalone | SfcPropertyFlags.SqlAzureDatabase | SfcPropertyFlags.Design | SfcPropertyFlags.Deploy)]
	public bool Descending
	{
		get
		{
			return (bool)base.Properties.GetValueWithNullReplacement("Descending");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("Descending", value);
		}
	}

	[SfcProperty(SfcPropertyFlags.Standalone | SfcPropertyFlags.SqlAzureDatabase)]
	public int ID => (int)base.Properties.GetValueWithNullReplacement("ID");

	[SfcProperty(SfcPropertyFlags.Standalone | SfcPropertyFlags.SqlAzureDatabase)]
	public bool IsComputed => (bool)base.Properties.GetValueWithNullReplacement("IsComputed");

	[SfcProperty(SfcPropertyFlags.Standalone | SfcPropertyFlags.SqlAzureDatabase | SfcPropertyFlags.Design | SfcPropertyFlags.Deploy)]
	public bool IsIncluded
	{
		get
		{
			return (bool)base.Properties.GetValueWithNullReplacement("IsIncluded");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("IsIncluded", value);
		}
	}

	internal IndexedColumn(AbstractCollectionBase parentColl, ObjectKeyBase key, SqlSmoState state)
		: base(parentColl, key, state)
	{
	}

	public IndexedColumn(Index index, string name, bool descending)
	{
		ValidateName(name);
		key = new SimpleObjectKey(name);
		Parent = index;
		Descending = descending;
	}

	internal static string[] GetScriptFields(Type parentType, ServerVersion version, DatabaseEngineType databaseEngineType, DatabaseEngineEdition databaseEngineEdition, bool defaultTextMode)
	{
		string[] fields = new string[4] { "IsIncluded", "Descending", "IsComputed", "ColumnStoreOrderOrdinal" };
		return SqlSmoObject.GetSupportedScriptFields(typeof(PropertyMetadataProvider), fields, version, databaseEngineType, databaseEngineEdition).ToArray();
	}

	public IndexedColumn()
	{
	}

	public IndexedColumn(Index index, string name)
	{
		ValidateName(name);
		key = new SimpleObjectKey(name);
		Parent = index;
	}

	internal override SqlPropertyMetadataProvider GetPropertyMetadataProvider()
	{
		return new PropertyMetadataProvider(base.ServerVersion, DatabaseEngineType, DatabaseEngineEdition);
	}

	object IPropertyDataDispatch.GetPropertyValue(int index)
	{
		if (DatabaseEngineType == DatabaseEngineType.SqlAzureDatabase)
		{
			if (DatabaseEngineEdition == DatabaseEngineEdition.SqlDataWarehouse)
			{
				return index switch
				{
					0 => XSchema.ColumnStoreOrderOrdinal, 
					1 => XSchema.Descending, 
					2 => XRuntime.ID, 
					3 => XSchema.IsIncluded, 
					_ => throw new IndexOutOfRangeException(), 
				};
			}
			return index switch
			{
				4 => XSchema.ColumnStoreOrderOrdinal, 
				0 => XSchema.Descending, 
				1 => XRuntime.ID, 
				2 => XSchema.IsComputed, 
				3 => XSchema.IsIncluded, 
				_ => throw new IndexOutOfRangeException(), 
			};
		}
		return index switch
		{
			4 => XSchema.ColumnStoreOrderOrdinal, 
			2 => XSchema.Descending, 
			0 => XRuntime.ID, 
			1 => XSchema.IsComputed, 
			3 => XSchema.IsIncluded, 
			_ => throw new IndexOutOfRangeException(), 
		};
	}

	void IPropertyDataDispatch.SetPropertyValue(int index, object value)
	{
		if (DatabaseEngineType == DatabaseEngineType.SqlAzureDatabase)
		{
			if (DatabaseEngineEdition == DatabaseEngineEdition.SqlDataWarehouse)
			{
				switch (index)
				{
				case 0:
					XSchema.ColumnStoreOrderOrdinal = (int)value;
					break;
				case 1:
					XSchema.Descending = (bool)value;
					break;
				case 2:
					XRuntime.ID = (int)value;
					break;
				case 3:
					XSchema.IsIncluded = (bool)value;
					break;
				default:
					throw new IndexOutOfRangeException();
				}
				return;
			}
			switch (index)
			{
			case 4:
				XSchema.ColumnStoreOrderOrdinal = (int)value;
				break;
			case 0:
				XSchema.Descending = (bool)value;
				break;
			case 1:
				XRuntime.ID = (int)value;
				break;
			case 2:
				XSchema.IsComputed = (bool)value;
				break;
			case 3:
				XSchema.IsIncluded = (bool)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
		else
		{
			switch (index)
			{
			case 4:
				XSchema.ColumnStoreOrderOrdinal = (int)value;
				break;
			case 2:
				XSchema.Descending = (bool)value;
				break;
			case 0:
				XRuntime.ID = (int)value;
				break;
			case 1:
				XSchema.IsComputed = (bool)value;
				break;
			case 3:
				XSchema.IsIncluded = (bool)value;
				break;
			default:
				throw new IndexOutOfRangeException();
			}
		}
	}
}
