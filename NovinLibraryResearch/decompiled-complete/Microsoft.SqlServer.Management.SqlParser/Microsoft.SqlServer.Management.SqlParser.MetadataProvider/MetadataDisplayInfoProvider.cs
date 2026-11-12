using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Microsoft.SqlServer.Management.SqlParser.Common;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class MetadataDisplayInfoProvider : IMetadataDisplayInfoProvider
{
	private class MetadataDisplayInfoVisitor : IMetadataObjectVisitor<string>, IDatabaseObjectVisitor<string>, IServerOwnedObjectVisitor<string>, IDatabaseOwnedObjectVisitor<string>, ISchemaOwnedObjectVisitor<string>
	{
		private readonly MetadataDisplayInfoProvider m_provider;

		private readonly PropertySelector m_propertySelector;

		public MetadataDisplayInfoVisitor(MetadataDisplayInfoProvider provider, PropertySelector propertySelector)
		{
			m_provider = provider;
			m_propertySelector = propertySelector;
		}

		public string Visit(IApplicationRole applicationRole)
		{
			return m_provider.m_applicationRole[applicationRole, m_propertySelector];
		}

		public string Visit(IAsymmetricKey asymmetricKey)
		{
			return m_provider.m_asymmetricKey[asymmetricKey, m_propertySelector];
		}

		public string Visit(IBuiltInFunction builtInFunction)
		{
			if (builtInFunction.IsGlobalVariable)
			{
				return m_provider.m_globalVariable[builtInFunction, m_propertySelector];
			}
			return m_provider.m_builtInFunction[builtInFunction, m_propertySelector];
		}

		public string Visit(ICertificate certificate)
		{
			return m_provider.m_certificate[certificate, m_propertySelector];
		}

		public string Visit(ICheckConstraint checkConstraint)
		{
			return m_provider.m_checkConstraint[checkConstraint, m_propertySelector];
		}

		public string Visit(ICollation collation)
		{
			return m_provider.m_collation[collation, m_propertySelector];
		}

		public string Visit(IColumn column)
		{
			return m_provider.m_column[column, m_propertySelector];
		}

		public string Visit(ICredential credential)
		{
			return m_provider.m_credential[credential, m_propertySelector];
		}

		public string Visit(ICursorDataType cursorDataType)
		{
			return m_provider.m_cursorDataType[cursorDataType, m_propertySelector];
		}

		public string Visit(ICursorParameter cursorParameter)
		{
			return m_provider.m_cursorParameter[cursorParameter, m_propertySelector];
		}

		public string Visit(ICursorVariable cursorVariable)
		{
			return m_provider.m_cursorVariable[cursorVariable, m_propertySelector];
		}

		public string Visit(IDatabase database)
		{
			return m_provider.m_database[database, m_propertySelector];
		}

		public string Visit(IDatabaseDdlTrigger databaseDdlTrigger)
		{
			return m_provider.m_databaseDdlTrigger[databaseDdlTrigger, m_propertySelector];
		}

		public string Visit(IDatabasePermission databasePermission)
		{
			return m_provider.m_databasePermission[databasePermission, m_propertySelector];
		}

		public string Visit(IDatabaseRole databaseRole)
		{
			return m_provider.m_databaseRole[databaseRole, m_propertySelector];
		}

		public string Visit(IDatePart datePart)
		{
			return m_provider.m_datePart[datePart, m_propertySelector];
		}

		public string Visit(IDefaultConstraint defaultConstraint)
		{
			return m_provider.m_defaultConstraint[defaultConstraint, m_propertySelector];
		}

		public string Visit(IDmlTrigger dmlTrigger)
		{
			return m_provider.m_dmlTrigger[dmlTrigger, m_propertySelector];
		}

		public string Visit(IExecutionContext executionContext)
		{
			return m_provider.m_executionContext[executionContext, m_propertySelector];
		}

		public string Visit(IExtendedStoredProcedure extendedStoredProcedure)
		{
			return m_provider.m_extendedStoredProcedure[extendedStoredProcedure, m_propertySelector];
		}

		public string Visit(IFileGroup fileGroup)
		{
			return m_provider.m_fileGroup[fileGroup, m_propertySelector];
		}

		public string Visit(IForeignKeyColumn foreignKeyColumn)
		{
			return m_provider.m_foreignKeyColumn[foreignKeyColumn, m_propertySelector];
		}

		public string Visit(IForeignKeyConstraint foreignKeyConstraint)
		{
			return m_provider.m_foreignKeyConstraint[foreignKeyConstraint, m_propertySelector];
		}

		public string Visit(IIndexedColumn indexedColumn)
		{
			return m_provider.m_indexedColumn[indexedColumn, m_propertySelector];
		}

		public string Visit(ILogin login)
		{
			return m_provider.m_login[login, m_propertySelector];
		}

		public string Visit(IPartitionScheme partitionScheme)
		{
			return m_provider.m_partitionScheme[partitionScheme, m_propertySelector];
		}

		public string Visit(IPrimaryKeyConstraint primaryKeyConstraint)
		{
			return m_provider.m_primaryKeyConstraint[primaryKeyConstraint, m_propertySelector];
		}

		public string Visit(IRelationalIndex relationalIndex)
		{
			return m_provider.m_relationalIndex[relationalIndex, m_propertySelector];
		}

		public string Visit(IScalarExpression scalarExpression)
		{
			return m_provider.m_scalarExpression[scalarExpression, m_propertySelector];
		}

		public string Visit(IScalarDataType scalarDataType)
		{
			return m_provider.m_scalarDataType[scalarDataType, m_propertySelector];
		}

		public string Visit(IClrDataType scalarDataType)
		{
			return m_provider.m_scalarDataType[scalarDataType, m_propertySelector];
		}

		public string Visit(IXmlDataType scalarDataType)
		{
			return m_provider.m_scalarDataType[scalarDataType, m_propertySelector];
		}

		public string Visit(IVoidDataType scalarDataType)
		{
			return m_provider.m_scalarDataType[scalarDataType, m_propertySelector];
		}

		public string Visit(ISystemClrDataType scalarDataType)
		{
			return m_provider.m_scalarDataType[scalarDataType, m_propertySelector];
		}

		public string Visit(IScalarParameter scalarParameter)
		{
			return m_provider.m_scalarParameter[scalarParameter, m_propertySelector];
		}

		public string Visit(IScalarValuedFunction scalarValuedFunction)
		{
			return m_provider.m_scalarValuedFunction[scalarValuedFunction, m_propertySelector];
		}

		public string Visit(IScalarVariable scalarVariable)
		{
			return m_provider.m_scalarVariable[scalarVariable, m_propertySelector];
		}

		public string Visit(ISchema schema)
		{
			return m_provider.m_schema[schema, m_propertySelector];
		}

		public string Visit(IServer server)
		{
			return m_provider.m_server[server, m_propertySelector];
		}

		public string Visit(IServerDdlTrigger serverDdlTrigger)
		{
			return m_provider.m_serverDdlTrigger[serverDdlTrigger, m_propertySelector];
		}

		public string Visit(ISpatialIndex spatialIndex)
		{
			return m_provider.m_spatialIndex[spatialIndex, m_propertySelector];
		}

		public string Visit(IStatistics statistics)
		{
			return m_provider.m_statistics[statistics, m_propertySelector];
		}

		public string Visit(IStoredProcedure storedProcedure)
		{
			return m_provider.m_storedProcedure[storedProcedure, m_propertySelector];
		}

		public string Visit(ISynonym synonym)
		{
			return m_provider.m_synonym[synonym, m_propertySelector];
		}

		public string Visit(ISystemDataType systemDataType)
		{
			return m_provider.m_systemDataType[systemDataType, m_propertySelector];
		}

		public string Visit(IUser user)
		{
			return m_provider.m_user[user, m_propertySelector];
		}

		public string Visit(ITable table)
		{
			if (table.TabularType == TabularType.TemporaryTable)
			{
				return m_provider.m_tempTable[table, m_propertySelector];
			}
			return m_provider.m_table[table, m_propertySelector];
		}

		public string Visit(ITableDataType tableDataType)
		{
			return m_provider.m_tableDataType[tableDataType, m_propertySelector];
		}

		public string Visit(ITableParameter tableParameter)
		{
			return m_provider.m_tableParameter[tableParameter, m_propertySelector];
		}

		public string Visit(ITableValuedFunction tableValuedFunction)
		{
			return m_provider.m_tableValuedFunction[tableValuedFunction, m_propertySelector];
		}

		public string Visit(ITableVariable tableVariable)
		{
			return m_provider.m_tableVariable[tableVariable, m_propertySelector];
		}

		public string Visit(IUdtDataMember udtDataMember)
		{
			return m_provider.m_udtDataMember[udtDataMember, m_propertySelector];
		}

		public string Visit(IUdtMethod udtMethod)
		{
			return m_provider.m_udtMethod[udtMethod, m_propertySelector];
		}

		public string Visit(IUniqueConstraint uniqueConstraint)
		{
			return m_provider.m_uniqueConstraint[uniqueConstraint, m_propertySelector];
		}

		public string Visit(IUserDefinedAggregate userDefinedAggregate)
		{
			return m_provider.m_userDefinedAggregate[userDefinedAggregate, m_propertySelector];
		}

		public string Visit(IUserDefinedClrType userDefinedClrType)
		{
			return m_provider.m_userDefinedClrType[userDefinedClrType, m_propertySelector];
		}

		public string Visit(IUserDefinedDataType userDefinedDataType)
		{
			return m_provider.m_userDefinedDataType[userDefinedDataType, m_propertySelector];
		}

		public string Visit(IUserDefinedTableType userDefinedTableType)
		{
			return m_provider.m_userDefinedTableType[userDefinedTableType, m_propertySelector];
		}

		public string Visit(IView view)
		{
			return m_provider.m_view[view, m_propertySelector];
		}

		public string Visit(IVirtualTable virtualTable)
		{
			return virtualTable.TabularType switch
			{
				TabularType.CommonTableExpression => m_provider.m_commonTableExpression[virtualTable, m_propertySelector], 
				TabularType.DerivedTable => m_provider.m_derivedTable[virtualTable, m_propertySelector], 
				TabularType.DmlInsertedDeleted => m_provider.m_dmlInsertedDeleted[virtualTable, m_propertySelector], 
				TabularType.DmlTableSource => m_provider.m_dmlTableSource[virtualTable, m_propertySelector], 
				TabularType.PivotTable => m_provider.m_pivotTable[virtualTable, m_propertySelector], 
				TabularType.TableAlias => m_provider.m_tableAlias[virtualTable, m_propertySelector], 
				TabularType.UnpivotTable => m_provider.m_unpivotTable[virtualTable, m_propertySelector], 
				_ => string.Empty, 
			};
		}

		public string Visit(IXmlDataTypeMethod xmlDataTypeMethod)
		{
			return m_provider.m_xmlDataTypeMethod[xmlDataTypeMethod, m_propertySelector];
		}

		public string Visit(IXmlDataTypeTableMethod xmlDataTypeTableMethod)
		{
			return m_provider.m_xmlDataTypeTableMethod[xmlDataTypeTableMethod, m_propertySelector];
		}

		public string Visit(IXmlIndex xmlIndex)
		{
			return m_provider.m_xmlIndex[xmlIndex, m_propertySelector];
		}
	}

	private enum PropertySelector
	{
		Name,
		Description,
		ObjectString,
		DbQualifiedName
	}

	[Flags]
	private enum CacheFlags
	{
		None = 0,
		DisplayName = 1,
		Description = 2,
		ObjectString = 4,
		DbQualifiedName = 8,
		DescrpAndObjStr = 6,
		NameDescrpAndObjStr = 7,
		All = 0xF
	}

	private class CacheSpec
	{
		private readonly int capacity;

		private readonly CacheFlags flags;

		public int Capacity => capacity;

		public bool CacheNameValues => (flags & CacheFlags.DisplayName) != 0;

		public bool CacheDescriptionValues => (flags & CacheFlags.Description) != 0;

		public bool CacheObjectStringValues => (flags & CacheFlags.ObjectString) != 0;

		public bool CacheDbQualifiedNameValues => (flags & CacheFlags.DbQualifiedName) != 0;

		public CacheSpec(int capacity, CacheFlags flags)
		{
			this.capacity = capacity;
			this.flags = flags;
		}
	}

	private abstract class MetadataObjectDisplayInfo<T> where T : class, IMetadataObject
	{
		private delegate string CaseConvertor(string value);

		private delegate string PropertyRetriever(T metadataObject);

		private class PropertyCache
		{
			private readonly PropertyRetriever m_retriever;

			private readonly Dictionary<T, string> m_cache;

			public string this[T metadataObject]
			{
				get
				{
					if (!m_cache.TryGetValue(metadataObject, out var value))
					{
						lock (m_cache)
						{
							if (!m_cache.TryGetValue(metadataObject, out value))
							{
								value = m_retriever(metadataObject);
								m_cache[metadataObject] = value;
							}
						}
					}
					return value;
				}
			}

			public PropertyCache(PropertyRetriever retriever, int initialCapacity)
			{
				m_retriever = retriever;
				m_cache = new Dictionary<T, string>(initialCapacity);
			}
		}

		protected readonly MetadataDisplayInfoProvider m_provider;

		protected readonly string m_metadataTypeName;

		private readonly CaseConvertor m_caseConvertor;

		private readonly PropertyCache m_nameCache;

		private readonly PropertyCache m_descriptionCache;

		private readonly PropertyCache m_objectStringCache;

		private readonly PropertyCache m_dbQualifiedNameCache;

		public string this[T metadataObject, PropertySelector propertySelector] => propertySelector switch
		{
			PropertySelector.Name => (m_nameCache != null) ? m_nameCache[metadataObject] : GetDisplayName(metadataObject), 
			PropertySelector.Description => (m_descriptionCache != null) ? m_descriptionCache[metadataObject] : GetDescription(metadataObject), 
			PropertySelector.ObjectString => (m_objectStringCache != null) ? m_objectStringCache[metadataObject] : GetObjectString(metadataObject), 
			PropertySelector.DbQualifiedName => (m_dbQualifiedNameCache != null) ? m_dbQualifiedNameCache[metadataObject] : GetDatabaseQualifiedName(metadataObject), 
			_ => null, 
		};

		protected MetadataObjectDisplayInfo(MetadataDisplayInfoProvider provider)
			: this(provider, (string)null, CasingStyle.None, (CacheSpec)null)
		{
		}

		protected MetadataObjectDisplayInfo(MetadataDisplayInfoProvider provider, string metadataTypeName)
			: this(provider, metadataTypeName, CasingStyle.None, (CacheSpec)null)
		{
		}

		protected MetadataObjectDisplayInfo(MetadataDisplayInfoProvider provider, string metadataTypeName, CasingStyle nameCasing)
			: this(provider, metadataTypeName, nameCasing, (CacheSpec)null)
		{
		}

		protected MetadataObjectDisplayInfo(MetadataDisplayInfoProvider provider, string metadataTypeName, CacheSpec cacheSpec)
			: this(provider, metadataTypeName, CasingStyle.None, cacheSpec)
		{
		}

		protected MetadataObjectDisplayInfo(MetadataDisplayInfoProvider provider, string metadataTypeName, CasingStyle nameCasing, CacheSpec cacheSpec)
		{
			m_metadataTypeName = metadataTypeName;
			m_provider = provider;
			switch (nameCasing)
			{
			case CasingStyle.Lowercase:
				m_caseConvertor = CultureInfo.InvariantCulture.TextInfo.ToLower;
				break;
			case CasingStyle.Uppercase:
				m_caseConvertor = CultureInfo.InvariantCulture.TextInfo.ToUpper;
				break;
			default:
				m_caseConvertor = null;
				break;
			}
			if (cacheSpec != null)
			{
				if (cacheSpec.CacheNameValues)
				{
					m_nameCache = new PropertyCache(GetDisplayName, cacheSpec.Capacity);
				}
				if (cacheSpec.CacheDescriptionValues)
				{
					m_descriptionCache = new PropertyCache(GetDescription, cacheSpec.Capacity);
				}
				if (cacheSpec.CacheObjectStringValues)
				{
					m_objectStringCache = new PropertyCache(GetObjectString, cacheSpec.Capacity);
				}
				if (cacheSpec.CacheDbQualifiedNameValues)
				{
					m_dbQualifiedNameCache = new PropertyCache(GetDatabaseQualifiedName, cacheSpec.Capacity);
				}
			}
		}

		protected virtual string GetDisplayName(T metadataObject)
		{
			string text = metadataObject.Name;
			if (m_caseConvertor != null && text != null)
			{
				text = m_caseConvertor(text);
			}
			return text;
		}

		protected virtual string GetDescription(T metadataObject)
		{
			return m_provider.MakeDescription(m_metadataTypeName, this[metadataObject, PropertySelector.ObjectString]);
		}

		protected virtual string GetObjectString(T metadataObject)
		{
			return this[metadataObject, PropertySelector.Name];
		}

		protected virtual string GetDatabaseQualifiedName(T metadataObject)
		{
			return null;
		}

		[Conditional("DEBUG")]
		protected virtual void RetrievePropertyPreChecks(T metadataObject)
		{
		}
	}

	private sealed class EmptyDisplayInfo<T> : MetadataObjectDisplayInfo<T> where T : class, IMetadataObject
	{
		public EmptyDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider)
		{
		}

		protected override string GetDisplayName(T metadataObject)
		{
			return string.Empty;
		}

		protected override string GetDescription(T metadataObject)
		{
			return string.Empty;
		}

		protected override string GetObjectString(T metadataObject)
		{
			return metadataObject.GetType().ToString();
		}
	}

	private abstract class DatabaseOwnedObjectDisplayInfo<T> : MetadataObjectDisplayInfo<T> where T : class, IDatabaseOwnedObject
	{
		protected DatabaseOwnedObjectDisplayInfo(MetadataDisplayInfoProvider provider, string metadataTypeName)
			: this(provider, metadataTypeName, (CacheSpec)null)
		{
		}

		protected DatabaseOwnedObjectDisplayInfo(MetadataDisplayInfoProvider provider, string metadataTypeName, CacheSpec cacheSpec)
			: base(provider, metadataTypeName, cacheSpec)
		{
		}

		protected override string GetObjectString(T metadataObject)
		{
			return GetDatabaseQualifiedName(metadataObject);
		}

		protected override string GetDatabaseQualifiedName(T metadataObject)
		{
			return m_provider.MakeDatabaseQualifiedName(metadataObject);
		}
	}

	private abstract class SchemaObjectDisplayInfo<T> : MetadataObjectDisplayInfo<T> where T : class, ISchemaOwnedObject
	{
		protected SchemaObjectDisplayInfo(MetadataDisplayInfoProvider provider, string metadataTypeName)
			: this(provider, metadataTypeName, (CacheSpec)null)
		{
		}

		protected SchemaObjectDisplayInfo(MetadataDisplayInfoProvider provider, string metadataTypeName, CacheSpec cacheSpec)
			: base(provider, metadataTypeName, cacheSpec)
		{
		}

		protected override string GetObjectString(T metadataObject)
		{
			return GetDatabaseQualifiedName(metadataObject);
		}

		protected override string GetDatabaseQualifiedName(T metadataObject)
		{
			return m_provider.MakeDatabaseQualifiedName(metadataObject);
		}
	}

	private sealed class ServerDisplayInfo : MetadataObjectDisplayInfo<IServer>
	{
		public ServerDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.Server)
		{
		}
	}

	private sealed class DatabaseDisplayInfo : MetadataObjectDisplayInfo<IDatabase>
	{
		public DatabaseDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.Database)
		{
		}

		protected sealed override string GetDatabaseQualifiedName(IDatabase metadataObject)
		{
			return m_provider.MakeDatabaseQualifiedName(metadataObject);
		}
	}

	private sealed class SchemaDisplayInfo : MetadataObjectDisplayInfo<ISchema>
	{
		public SchemaDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.Schema)
		{
		}

		protected override string GetObjectString(ISchema metadataObject)
		{
			return m_provider.MakeDatabaseQualifiedName(metadataObject);
		}

		protected override string GetDatabaseQualifiedName(ISchema metadataObject)
		{
			return m_provider.MakeDatabaseQualifiedName(metadataObject);
		}
	}

	private sealed class TableDisplayInfo : SchemaObjectDisplayInfo<ITable>
	{
		public TableDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.Table)
		{
		}

		protected override void RetrievePropertyPreChecks(ITable metadataObject)
		{
		}
	}

	private sealed class TempTableDisplayInfo : SchemaObjectDisplayInfo<ITable>
	{
		public TempTableDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.TemporaryTable)
		{
		}

		protected override void RetrievePropertyPreChecks(ITable metadataObject)
		{
		}

		protected override string GetDatabaseQualifiedName(ITable metadataObject)
		{
			return GetDisplayName(metadataObject);
		}
	}

	private sealed class ViewDisplayInfo : SchemaObjectDisplayInfo<IView>
	{
		public ViewDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.View)
		{
		}
	}

	private sealed class CteDisplayInfo : MetadataObjectDisplayInfo<IVirtualTable>
	{
		public CteDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.CommonTableExpression)
		{
		}

		protected override void RetrievePropertyPreChecks(IVirtualTable metadataObject)
		{
		}
	}

	private sealed class DerivedTableDisplayInfo : MetadataObjectDisplayInfo<IVirtualTable>
	{
		public DerivedTableDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.DerivedTable)
		{
		}

		protected override void RetrievePropertyPreChecks(IVirtualTable metadataObject)
		{
		}
	}

	private sealed class DmlInsertedDeletedDisplayInfo : MetadataObjectDisplayInfo<IVirtualTable>
	{
		public DmlInsertedDeletedDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, (string)null)
		{
		}

		protected override string GetDescription(IVirtualTable metadataObject)
		{
			return m_provider.GetDescription(metadataObject.TargetTable);
		}

		protected override void RetrievePropertyPreChecks(IVirtualTable metadataObject)
		{
		}
	}

	private sealed class DmlTableSourceDisplayInfo : MetadataObjectDisplayInfo<IVirtualTable>
	{
		public DmlTableSourceDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.DmlTableSource)
		{
		}

		protected override void RetrievePropertyPreChecks(IVirtualTable metadataObject)
		{
		}
	}

	private sealed class PivotTableDisplayInfo : MetadataObjectDisplayInfo<IVirtualTable>
	{
		public PivotTableDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.PivotTable)
		{
		}

		protected override void RetrievePropertyPreChecks(IVirtualTable metadataObject)
		{
		}
	}

	private sealed class TableAliasDisplayInfo : MetadataObjectDisplayInfo<IVirtualTable>
	{
		public TableAliasDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, (string)null)
		{
		}

		protected override string GetDescription(IVirtualTable metadataObject)
		{
			return m_provider.MakeAliasedObjectDescription(metadataObject.Unaliased, GetDisplayName(metadataObject));
		}

		protected override void RetrievePropertyPreChecks(IVirtualTable metadataObject)
		{
		}
	}

	private sealed class UnpivotTableDisplayInfo : MetadataObjectDisplayInfo<IVirtualTable>
	{
		public UnpivotTableDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.UnpivotTable)
		{
		}

		protected override void RetrievePropertyPreChecks(IVirtualTable metadataObject)
		{
		}
	}

	private sealed class CollationDisplayInfo : MetadataObjectDisplayInfo<ICollation>
	{
		public CollationDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.Collation)
		{
		}
	}

	private sealed class ColumnDisplayInfo : MetadataObjectDisplayInfo<IColumn>
	{
		public ColumnDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.Column)
		{
		}

		protected override string GetObjectString(IColumn metadataObject)
		{
			string displayName = GetDisplayName(metadataObject);
			string text = (metadataObject.InPrimaryKey ? "PK, " : string.Empty);
			string displayName2 = m_provider.GetDisplayName(metadataObject.DataType);
			string text2 = (metadataObject.Nullable ? ", null" : ", not null");
			return displayName + "(" + text + displayName2 + text2 + ")";
		}
	}

	private sealed class ScalarExpressionDisplayInfo : MetadataObjectDisplayInfo<IScalarExpression>
	{
		public ScalarExpressionDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, (string)null)
		{
		}

		protected override string GetObjectString(IScalarExpression metadataObject)
		{
			string text = m_provider.ObjectToString(metadataObject.DataType);
			if (!string.IsNullOrEmpty(text))
			{
				return "<expression> " + text;
			}
			return "<expression>";
		}
	}

	private sealed class ScalarDataTypeDisplayInfo : MetadataObjectDisplayInfo<IScalarDataType>
	{
		public ScalarDataTypeDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, (string)null)
		{
		}

		protected override string GetDescription(IScalarDataType metadataObject)
		{
			string text = metadataObject.Name;
			if (metadataObject.IsUnknown && string.IsNullOrEmpty(text))
			{
				text = null;
			}
			return text;
		}
	}

	private sealed class TableDataTypeDisplayInfo : MetadataObjectDisplayInfo<ITableDataType>
	{
		public TableDataTypeDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.DataType)
		{
		}

		protected override string GetDescription(ITableDataType metadataObject)
		{
			return null;
		}
	}

	private sealed class CursorDataTypeDisplayInfo : MetadataObjectDisplayInfo<ICursorDataType>
	{
		public CursorDataTypeDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.DataType)
		{
		}

		protected override string GetObjectString(ICursorDataType metadataObject)
		{
			return "CURSOR";
		}
	}

	private sealed class SystemDataTypeDisplayInfo : MetadataObjectDisplayInfo<ISystemDataType>
	{
		public SystemDataTypeDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.SystemDataType, new CacheSpec(64, CacheFlags.DescrpAndObjStr))
		{
		}

		protected override string GetObjectString(ISystemDataType metadataObject)
		{
			if (metadataObject.TypeSpec.ArgSpec2 != null)
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}({1}, {2})", metadataObject.Name, metadataObject.NumericPrecision, metadataObject.NumericScale);
			}
			if (metadataObject.TypeSpec.ArgSpec1 != null)
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}({1})", metadataObject.Name, metadataObject.TypeSpec.ArgIsScale ? metadataObject.NumericScale : metadataObject.Length);
			}
			return metadataObject.TypeSpec.SqlDataType switch
			{
				SqlDataType.NVarCharMax => "nvarchar(max)", 
				SqlDataType.VarBinaryMax => "varbinary(max)", 
				SqlDataType.VarCharMax => "varchar(max)", 
				_ => metadataObject.Name, 
			};
		}
	}

	private sealed class UserDefinedDataTypeDisplayInfo : SchemaObjectDisplayInfo<IUserDefinedDataType>
	{
		public UserDefinedDataTypeDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.UserDefinedDataType, new CacheSpec(64, CacheFlags.DescrpAndObjStr))
		{
		}

		protected override string GetObjectString(IUserDefinedDataType metadataObject)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}({1}, {2})", GetDatabaseQualifiedName(metadataObject), m_provider.ObjectToString(metadataObject.BaseSystemDataType), metadataObject.Nullable ? "null" : "not null");
		}
	}

	private sealed class UserDefinedTableTypeDisplayInfo : SchemaObjectDisplayInfo<IUserDefinedTableType>
	{
		public UserDefinedTableTypeDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.UserDefinedTableType, new CacheSpec(64, CacheFlags.DescrpAndObjStr))
		{
		}

		protected override string GetObjectString(IUserDefinedTableType metadataObject)
		{
			return GetDatabaseQualifiedName(metadataObject) + " TABLE";
		}
	}

	private sealed class UserDefinedClrTypeDisplayInfo : SchemaObjectDisplayInfo<IUserDefinedClrType>
	{
		public UserDefinedClrTypeDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.UserDefinedClrType, new CacheSpec(64, CacheFlags.DescrpAndObjStr))
		{
		}
	}

	private abstract class VariableDisplayInfo<T> : MetadataObjectDisplayInfo<T> where T : class, ILocalVariable
	{
		protected VariableDisplayInfo(MetadataDisplayInfoProvider provider)
			: this(provider, SR.LocalVariable)
		{
		}

		protected VariableDisplayInfo(MetadataDisplayInfoProvider provider, string metadataTypeName)
			: base(provider, metadataTypeName)
		{
		}

		protected override string GetObjectString(T metadataObject)
		{
			string text = m_provider.ObjectToString(metadataObject.DataType);
			if (!string.IsNullOrEmpty(text))
			{
				return GetDisplayName(metadataObject) + " " + text;
			}
			return GetDisplayName(metadataObject);
		}
	}

	private sealed class CursorVariableDisplayInfo : VariableDisplayInfo<ICursorVariable>
	{
		public CursorVariableDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider)
		{
		}
	}

	private sealed class ScalarVariableDisplayInfo : VariableDisplayInfo<IScalarVariable>
	{
		public ScalarVariableDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider)
		{
		}
	}

	private sealed class TableVariableDisplayInfo : VariableDisplayInfo<ITableVariable>
	{
		public TableVariableDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider)
		{
		}
	}

	private abstract class ParameterDisplayInfo<T> : VariableDisplayInfo<T> where T : class, IParameter
	{
		protected ParameterDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.Parameter)
		{
		}

		protected override string GetObjectString(T metadataObject)
		{
			string text = base.GetObjectString(metadataObject);
			if (!string.IsNullOrEmpty(metadataObject.DefaultValue))
			{
				text = text + " = " + metadataObject.DefaultValue;
			}
			if (metadataObject.IsOutput)
			{
				text += " OUTPUT";
			}
			else if (metadataObject.IsReadOnly)
			{
				text += " READONLY";
			}
			return text;
		}
	}

	private sealed class CursorParameterDisplayInfo : ParameterDisplayInfo<ICursorParameter>
	{
		public CursorParameterDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider)
		{
		}
	}

	private sealed class ScalarParameterDisplayInfo : ParameterDisplayInfo<IScalarParameter>
	{
		public ScalarParameterDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider)
		{
		}
	}

	private sealed class TableParameterDisplayInfo : ParameterDisplayInfo<ITableParameter>
	{
		public TableParameterDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider)
		{
		}
	}

	private sealed class UdtDataMemberDisplayInfo : MetadataObjectDisplayInfo<IUdtDataMember>
	{
		public UdtDataMemberDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, (string)null)
		{
		}

		protected override string GetObjectString(IUdtDataMember metadataObject)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}.{1} {2}", m_provider.GetDisplayName(metadataObject.UserDefinedType), GetDisplayName(metadataObject), m_provider.ObjectToString(metadataObject.DataType));
		}
	}

	private sealed class UdtMethodDisplayInfo : MetadataObjectDisplayInfo<IUdtMethod>
	{
		public UdtMethodDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, (string)null)
		{
		}

		protected override string GetObjectString(IUdtMethod metadataObject)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}.{1}({2}) {3}", m_provider.GetDisplayName(metadataObject.UserDefinedType), GetDisplayName(metadataObject), m_provider.CollectionToString(metadataObject.Parameters, singleLine: true), m_provider.ObjectToString(metadataObject.DataType));
		}
	}

	private sealed class BuiltInFunctionDisplayInfo : MetadataObjectDisplayInfo<IBuiltInFunction>
	{
		public BuiltInFunctionDisplayInfo(MetadataDisplayInfoProvider provider, CasingStyle casingStyle)
			: base(provider, SR.BuiltinFunction, casingStyle, new CacheSpec(256, CacheFlags.NameDescrpAndObjStr))
		{
		}

		protected override string GetObjectString(IBuiltInFunction metadataObject)
		{
			string displayName = GetDisplayName(metadataObject);
			string text = m_provider.CollectionToString(metadataObject.Parameters, singleLine: true);
			if (metadataObject.IsVarArg)
			{
				text += ((text.Length == 0) ? "..." : ", ...");
			}
			string returnType = m_provider.ObjectToString(metadataObject.DataType);
			return m_provider.FunctionToString(displayName, text, returnType);
		}

		protected override void RetrievePropertyPreChecks(IBuiltInFunction metadataObject)
		{
		}
	}

	private sealed class GlobalVariableDisplayInfo : MetadataObjectDisplayInfo<IBuiltInFunction>
	{
		public GlobalVariableDisplayInfo(MetadataDisplayInfoProvider provider, CasingStyle casingStyle)
			: base(provider, SR.GlobalVariable, casingStyle, new CacheSpec(256, CacheFlags.NameDescrpAndObjStr))
		{
		}

		protected override string GetObjectString(IBuiltInFunction metadataObject)
		{
			string displayName = GetDisplayName(metadataObject);
			string text = m_provider.ObjectToString(metadataObject.DataType);
			if (!string.IsNullOrEmpty(text))
			{
				return displayName + " " + text;
			}
			return displayName;
		}

		protected override void RetrievePropertyPreChecks(IBuiltInFunction metadataObject)
		{
		}
	}

	private sealed class DatePartDisplayInfo : MetadataObjectDisplayInfo<IDatePart>
	{
		public DatePartDisplayInfo(MetadataDisplayInfoProvider provider, CasingStyle casingStyle)
			: base(provider, SR.Datepart, casingStyle, new CacheSpec(64, CacheFlags.NameDescrpAndObjStr))
		{
		}

		protected override string GetObjectString(IDatePart metadataObject)
		{
			switch (StringTokenLookup.Find(metadataObject.Name))
			{
			case StringToken.YY:
			case StringToken.Year:
			case StringToken.YYYY:
				return "Year";
			case StringToken.Q:
			case StringToken.QQ:
			case StringToken.Quarter:
				return "Quarter";
			case StringToken.M:
			case StringToken.MM:
			case StringToken.Month:
				return "Month";
			case StringToken.Y:
			case StringToken.DY:
			case StringToken.DayOfYear:
				return "Day of year";
			case StringToken.D:
			case StringToken.DD:
			case StringToken.Day:
				return "Day";
			case StringToken.WK:
			case StringToken.WW:
			case StringToken.Week:
				return "Week";
			case StringToken.W:
			case StringToken.DW:
			case StringToken.Weekday:
				return "Weekday";
			case StringToken.HH:
			case StringToken.Hour:
				return "Hour";
			case StringToken.N:
			case StringToken.MI:
			case StringToken.Minute:
				return "Minute";
			case StringToken.S:
			case StringToken.SS:
			case StringToken.Second:
				return "Second";
			case StringToken.MS:
			case StringToken.Millisecond:
				return "Millisecond";
			case StringToken.MCS:
			case StringToken.Microsecond:
				return "Microsecond";
			case StringToken.NS:
			case StringToken.Nanosecond:
				return "Nanosecond";
			case StringToken.TZ:
			case StringToken.TzOffset:
				return "Time zone offset";
			case StringToken.IsoWk:
			case StringToken.IsoWw:
			case StringToken.IsoWeek:
				return "ISO week";
			default:
				return string.Empty;
			}
		}
	}

	private sealed class StoredProcedureDisplayInfo : SchemaObjectDisplayInfo<IStoredProcedure>
	{
		public StoredProcedureDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.StoredProcedure)
		{
		}

		protected override string GetObjectString(IStoredProcedure metadataObject)
		{
			string databaseQualifiedName = GetDatabaseQualifiedName(metadataObject);
			string text = m_provider.CollectionToString(metadataObject.Parameters, singleLine: true);
			if (text.Length != 0)
			{
				return databaseQualifiedName + " " + text;
			}
			return databaseQualifiedName;
		}
	}

	private sealed class ExtendedStoredProcedureDisplayInfo : SchemaObjectDisplayInfo<IExtendedStoredProcedure>
	{
		public ExtendedStoredProcedureDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.ExtendedStoredProcedure)
		{
		}
	}

	private sealed class SynonymDisplayInfo : SchemaObjectDisplayInfo<ISynonym>
	{
		public SynonymDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.Synonym)
		{
		}
	}

	private sealed class DatabaseDdlTriggerDisplayInfo : MetadataObjectDisplayInfo<IDatabaseDdlTrigger>
	{
		public DatabaseDdlTriggerDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.DatabaseDdlTrigger)
		{
		}
	}

	private sealed class DmlTriggerDisplayInfo : MetadataObjectDisplayInfo<IDmlTrigger>
	{
		public DmlTriggerDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.DmlTrigger)
		{
		}
	}

	private sealed class ServerDdlTriggerDisplayInfo : MetadataObjectDisplayInfo<IServerDdlTrigger>
	{
		public ServerDdlTriggerDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.ServerDdlTrigger)
		{
		}
	}

	private sealed class ScalarValuedFunctionDisplayInfo : SchemaObjectDisplayInfo<IScalarValuedFunction>
	{
		public ScalarValuedFunctionDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.ScalarValuedFunction, new CacheSpec(512, CacheFlags.ObjectString))
		{
		}

		protected override string GetObjectString(IScalarValuedFunction metadataObject)
		{
			string databaseQualifiedName = GetDatabaseQualifiedName(metadataObject);
			string paramList = m_provider.CollectionToString(metadataObject.Parameters, singleLine: true);
			string returnType = m_provider.ObjectToString(metadataObject.ReturnType);
			return m_provider.FunctionToString(databaseQualifiedName, paramList, returnType);
		}
	}

	private sealed class TableValuedFunctionDisplayInfo : SchemaObjectDisplayInfo<ITableValuedFunction>
	{
		public TableValuedFunctionDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.TableValuedFunction, new CacheSpec(64, CacheFlags.ObjectString))
		{
		}

		protected override string GetObjectString(ITableValuedFunction metadataObject)
		{
			string databaseQualifiedName = GetDatabaseQualifiedName(metadataObject);
			string paramList = m_provider.CollectionToString(metadataObject.Parameters, singleLine: true);
			string returnType = "TABLE";
			return m_provider.FunctionToString(databaseQualifiedName, paramList, returnType);
		}
	}

	private sealed class UserDefinedAggregateDisplayInfo : SchemaObjectDisplayInfo<IUserDefinedAggregate>
	{
		public UserDefinedAggregateDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.UserDefinedAggregate, new CacheSpec(64, CacheFlags.ObjectString))
		{
		}

		protected override string GetObjectString(IUserDefinedAggregate metadataObject)
		{
			string databaseQualifiedName = GetDatabaseQualifiedName(metadataObject);
			string paramList = m_provider.CollectionToString(metadataObject.Parameters, singleLine: true);
			string returnType = m_provider.ObjectToString(metadataObject.DataType);
			return m_provider.FunctionToString(databaseQualifiedName, paramList, returnType);
		}
	}

	private sealed class ApplicationRoleDisplayInfo : DatabaseOwnedObjectDisplayInfo<IApplicationRole>
	{
		public ApplicationRoleDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.ApplicationRole)
		{
		}
	}

	private sealed class AsymmetricKeyDisplayInfo : DatabaseOwnedObjectDisplayInfo<IAsymmetricKey>
	{
		public AsymmetricKeyDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.AsymmetricKey)
		{
		}
	}

	private sealed class CertificateDisplayInfo : DatabaseOwnedObjectDisplayInfo<ICertificate>
	{
		public CertificateDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.Certificate)
		{
		}
	}

	private sealed class CredentialDisplayInfo : MetadataObjectDisplayInfo<ICredential>
	{
		public CredentialDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.Credential)
		{
		}
	}

	private sealed class DatabasePermissionDisplayInfo : MetadataObjectDisplayInfo<IDatabasePermission>
	{
		public DatabasePermissionDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.DatabasePermission)
		{
		}
	}

	private sealed class DatabaseRoleDisplayInfo : DatabaseOwnedObjectDisplayInfo<IDatabaseRole>
	{
		public DatabaseRoleDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.DatabaseRole)
		{
		}
	}

	private sealed class LoginDisplayInfo : MetadataObjectDisplayInfo<ILogin>
	{
		public LoginDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.Login)
		{
		}
	}

	private sealed class UserDisplayInfo : DatabaseOwnedObjectDisplayInfo<IUser>
	{
		public UserDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.User)
		{
		}
	}

	private sealed class XmlDataTypeMethodDisplayInfo : MetadataObjectDisplayInfo<IXmlDataTypeMethod>
	{
		public XmlDataTypeMethodDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.XmlDataTypeMethod, new CacheSpec(8, CacheFlags.NameDescrpAndObjStr))
		{
		}

		protected override string GetObjectString(IXmlDataTypeMethod metadataObject)
		{
			string displayName = GetDisplayName(metadataObject);
			string paramList = m_provider.CollectionToString(metadataObject.Parameters, singleLine: true);
			string returnType = (metadataObject.DataType.IsUnknown ? "scalar" : m_provider.ObjectToString(metadataObject.DataType));
			return m_provider.FunctionToString(displayName, paramList, returnType);
		}
	}

	private sealed class XmlDataTypeTableMethodDisplayInfo : MetadataObjectDisplayInfo<IXmlDataTypeTableMethod>
	{
		public XmlDataTypeTableMethodDisplayInfo(MetadataDisplayInfoProvider provider)
			: base(provider, SR.XmlDataTypeMethod, new CacheSpec(8, CacheFlags.NameDescrpAndObjStr))
		{
		}

		protected override string GetObjectString(IXmlDataTypeTableMethod metadataObject)
		{
			string displayName = GetDisplayName(metadataObject);
			string paramList = m_provider.CollectionToString(metadataObject.Parameters, singleLine: true);
			string returnType = "table";
			return m_provider.FunctionToString(displayName, paramList, returnType);
		}
	}

	private CasingStyle m_builtInCasing;

	private readonly ServerDisplayInfo m_server;

	private readonly DatabaseDisplayInfo m_database;

	private readonly SchemaDisplayInfo m_schema;

	private readonly CteDisplayInfo m_commonTableExpression;

	private readonly DerivedTableDisplayInfo m_derivedTable;

	private readonly DmlInsertedDeletedDisplayInfo m_dmlInsertedDeleted;

	private readonly DmlTableSourceDisplayInfo m_dmlTableSource;

	private readonly PivotTableDisplayInfo m_pivotTable;

	private readonly TableDisplayInfo m_table;

	private readonly TableAliasDisplayInfo m_tableAlias;

	private readonly TempTableDisplayInfo m_tempTable;

	private readonly UnpivotTableDisplayInfo m_unpivotTable;

	private readonly ViewDisplayInfo m_view;

	private readonly CollationDisplayInfo m_collation;

	private readonly ColumnDisplayInfo m_column;

	private readonly ScalarExpressionDisplayInfo m_scalarExpression;

	private readonly CursorDataTypeDisplayInfo m_cursorDataType;

	private readonly ScalarDataTypeDisplayInfo m_scalarDataType;

	private readonly SystemDataTypeDisplayInfo m_systemDataType;

	private readonly TableDataTypeDisplayInfo m_tableDataType;

	private readonly UserDefinedClrTypeDisplayInfo m_userDefinedClrType;

	private readonly UserDefinedDataTypeDisplayInfo m_userDefinedDataType;

	private readonly UserDefinedTableTypeDisplayInfo m_userDefinedTableType;

	private readonly CursorVariableDisplayInfo m_cursorVariable;

	private readonly ScalarVariableDisplayInfo m_scalarVariable;

	private readonly TableVariableDisplayInfo m_tableVariable;

	private readonly CursorParameterDisplayInfo m_cursorParameter;

	private readonly ScalarParameterDisplayInfo m_scalarParameter;

	private readonly TableParameterDisplayInfo m_tableParameter;

	private readonly UdtDataMemberDisplayInfo m_udtDataMember;

	private readonly UdtMethodDisplayInfo m_udtMethod;

	private readonly BuiltInFunctionDisplayInfo m_builtInFunctionUpper;

	private readonly GlobalVariableDisplayInfo m_globalVariableUpper;

	private readonly DatePartDisplayInfo m_datePartUpper;

	private readonly BuiltInFunctionDisplayInfo m_builtInFunctionLower;

	private readonly GlobalVariableDisplayInfo m_globalVariableLower;

	private readonly DatePartDisplayInfo m_datePartLower;

	private BuiltInFunctionDisplayInfo m_builtInFunction;

	private GlobalVariableDisplayInfo m_globalVariable;

	private DatePartDisplayInfo m_datePart;

	private readonly StoredProcedureDisplayInfo m_storedProcedure;

	private readonly ExtendedStoredProcedureDisplayInfo m_extendedStoredProcedure;

	private readonly ScalarValuedFunctionDisplayInfo m_scalarValuedFunction;

	private readonly TableValuedFunctionDisplayInfo m_tableValuedFunction;

	private readonly UserDefinedAggregateDisplayInfo m_userDefinedAggregate;

	private readonly DatabaseDdlTriggerDisplayInfo m_databaseDdlTrigger;

	private readonly DmlTriggerDisplayInfo m_dmlTrigger;

	private readonly ServerDdlTriggerDisplayInfo m_serverDdlTrigger;

	private readonly SynonymDisplayInfo m_synonym;

	private readonly ApplicationRoleDisplayInfo m_applicationRole;

	private readonly AsymmetricKeyDisplayInfo m_asymmetricKey;

	private readonly CertificateDisplayInfo m_certificate;

	private readonly CredentialDisplayInfo m_credential;

	private readonly DatabasePermissionDisplayInfo m_databasePermission;

	private readonly DatabaseRoleDisplayInfo m_databaseRole;

	private readonly LoginDisplayInfo m_login;

	private readonly UserDisplayInfo m_user;

	private readonly XmlDataTypeMethodDisplayInfo m_xmlDataTypeMethod;

	private readonly XmlDataTypeTableMethodDisplayInfo m_xmlDataTypeTableMethod;

	private readonly EmptyDisplayInfo<ICheckConstraint> m_checkConstraint;

	private readonly EmptyDisplayInfo<IDefaultConstraint> m_defaultConstraint;

	private readonly EmptyDisplayInfo<IExecutionContext> m_executionContext;

	private readonly EmptyDisplayInfo<IFileGroup> m_fileGroup;

	private readonly EmptyDisplayInfo<IForeignKeyColumn> m_foreignKeyColumn;

	private readonly EmptyDisplayInfo<IForeignKeyConstraint> m_foreignKeyConstraint;

	private readonly EmptyDisplayInfo<IIndexedColumn> m_indexedColumn;

	private readonly EmptyDisplayInfo<IPartitionScheme> m_partitionScheme;

	private readonly EmptyDisplayInfo<IPrimaryKeyConstraint> m_primaryKeyConstraint;

	private readonly EmptyDisplayInfo<IRelationalIndex> m_relationalIndex;

	private readonly EmptyDisplayInfo<ISpatialIndex> m_spatialIndex;

	private readonly EmptyDisplayInfo<IStatistics> m_statistics;

	private readonly EmptyDisplayInfo<IUniqueConstraint> m_uniqueConstraint;

	private readonly EmptyDisplayInfo<IXmlIndex> m_xmlIndex;

	private readonly MetadataDisplayInfoVisitor m_displayName;

	private readonly MetadataDisplayInfoVisitor m_description;

	private readonly MetadataDisplayInfoVisitor m_objectString;

	private readonly MetadataDisplayInfoVisitor m_dbQualifiedName;

	public CasingStyle BuiltInCasing
	{
		get
		{
			return m_builtInCasing;
		}
		set
		{
			switch (value)
			{
			case CasingStyle.Uppercase:
				m_builtInFunction = m_builtInFunctionUpper;
				m_globalVariable = m_globalVariableUpper;
				m_datePart = m_datePartUpper;
				break;
			case CasingStyle.Lowercase:
				m_builtInFunction = m_builtInFunctionLower;
				m_globalVariable = m_globalVariableLower;
				m_datePart = m_datePartLower;
				break;
			}
			m_builtInCasing = value;
		}
	}

	public MetadataDisplayInfoProvider()
	{
		m_server = new ServerDisplayInfo(this);
		m_database = new DatabaseDisplayInfo(this);
		m_schema = new SchemaDisplayInfo(this);
		m_commonTableExpression = new CteDisplayInfo(this);
		m_derivedTable = new DerivedTableDisplayInfo(this);
		m_dmlInsertedDeleted = new DmlInsertedDeletedDisplayInfo(this);
		m_dmlTableSource = new DmlTableSourceDisplayInfo(this);
		m_pivotTable = new PivotTableDisplayInfo(this);
		m_table = new TableDisplayInfo(this);
		m_tableAlias = new TableAliasDisplayInfo(this);
		m_tempTable = new TempTableDisplayInfo(this);
		m_unpivotTable = new UnpivotTableDisplayInfo(this);
		m_view = new ViewDisplayInfo(this);
		m_collation = new CollationDisplayInfo(this);
		m_column = new ColumnDisplayInfo(this);
		m_scalarExpression = new ScalarExpressionDisplayInfo(this);
		m_cursorDataType = new CursorDataTypeDisplayInfo(this);
		m_scalarDataType = new ScalarDataTypeDisplayInfo(this);
		m_systemDataType = new SystemDataTypeDisplayInfo(this);
		m_tableDataType = new TableDataTypeDisplayInfo(this);
		m_userDefinedClrType = new UserDefinedClrTypeDisplayInfo(this);
		m_userDefinedDataType = new UserDefinedDataTypeDisplayInfo(this);
		m_userDefinedTableType = new UserDefinedTableTypeDisplayInfo(this);
		m_cursorVariable = new CursorVariableDisplayInfo(this);
		m_scalarVariable = new ScalarVariableDisplayInfo(this);
		m_tableVariable = new TableVariableDisplayInfo(this);
		m_cursorParameter = new CursorParameterDisplayInfo(this);
		m_scalarParameter = new ScalarParameterDisplayInfo(this);
		m_tableParameter = new TableParameterDisplayInfo(this);
		m_udtDataMember = new UdtDataMemberDisplayInfo(this);
		m_udtMethod = new UdtMethodDisplayInfo(this);
		m_builtInFunctionUpper = new BuiltInFunctionDisplayInfo(this, CasingStyle.Uppercase);
		m_globalVariableUpper = new GlobalVariableDisplayInfo(this, CasingStyle.Uppercase);
		m_datePartUpper = new DatePartDisplayInfo(this, CasingStyle.Uppercase);
		m_builtInFunctionLower = new BuiltInFunctionDisplayInfo(this, CasingStyle.Lowercase);
		m_globalVariableLower = new GlobalVariableDisplayInfo(this, CasingStyle.Lowercase);
		m_datePartLower = new DatePartDisplayInfo(this, CasingStyle.Lowercase);
		m_storedProcedure = new StoredProcedureDisplayInfo(this);
		m_extendedStoredProcedure = new ExtendedStoredProcedureDisplayInfo(this);
		m_scalarValuedFunction = new ScalarValuedFunctionDisplayInfo(this);
		m_tableValuedFunction = new TableValuedFunctionDisplayInfo(this);
		m_userDefinedAggregate = new UserDefinedAggregateDisplayInfo(this);
		m_databaseDdlTrigger = new DatabaseDdlTriggerDisplayInfo(this);
		m_dmlTrigger = new DmlTriggerDisplayInfo(this);
		m_serverDdlTrigger = new ServerDdlTriggerDisplayInfo(this);
		m_synonym = new SynonymDisplayInfo(this);
		m_applicationRole = new ApplicationRoleDisplayInfo(this);
		m_asymmetricKey = new AsymmetricKeyDisplayInfo(this);
		m_certificate = new CertificateDisplayInfo(this);
		m_credential = new CredentialDisplayInfo(this);
		m_databasePermission = new DatabasePermissionDisplayInfo(this);
		m_databaseRole = new DatabaseRoleDisplayInfo(this);
		m_login = new LoginDisplayInfo(this);
		m_user = new UserDisplayInfo(this);
		m_xmlDataTypeMethod = new XmlDataTypeMethodDisplayInfo(this);
		m_xmlDataTypeTableMethod = new XmlDataTypeTableMethodDisplayInfo(this);
		m_checkConstraint = new EmptyDisplayInfo<ICheckConstraint>(this);
		m_defaultConstraint = new EmptyDisplayInfo<IDefaultConstraint>(this);
		m_executionContext = new EmptyDisplayInfo<IExecutionContext>(this);
		m_fileGroup = new EmptyDisplayInfo<IFileGroup>(this);
		m_foreignKeyColumn = new EmptyDisplayInfo<IForeignKeyColumn>(this);
		m_foreignKeyConstraint = new EmptyDisplayInfo<IForeignKeyConstraint>(this);
		m_indexedColumn = new EmptyDisplayInfo<IIndexedColumn>(this);
		m_partitionScheme = new EmptyDisplayInfo<IPartitionScheme>(this);
		m_primaryKeyConstraint = new EmptyDisplayInfo<IPrimaryKeyConstraint>(this);
		m_relationalIndex = new EmptyDisplayInfo<IRelationalIndex>(this);
		m_spatialIndex = new EmptyDisplayInfo<ISpatialIndex>(this);
		m_statistics = new EmptyDisplayInfo<IStatistics>(this);
		m_uniqueConstraint = new EmptyDisplayInfo<IUniqueConstraint>(this);
		m_xmlIndex = new EmptyDisplayInfo<IXmlIndex>(this);
		m_displayName = new MetadataDisplayInfoVisitor(this, PropertySelector.Name);
		m_description = new MetadataDisplayInfoVisitor(this, PropertySelector.Description);
		m_objectString = new MetadataDisplayInfoVisitor(this, PropertySelector.ObjectString);
		m_dbQualifiedName = new MetadataDisplayInfoVisitor(this, PropertySelector.DbQualifiedName);
		BuiltInCasing = CasingStyle.Uppercase;
	}

	public string GetDisplayName(IMetadataObject metadataObject)
	{
		return metadataObject?.Accept(m_displayName);
	}

	public string GetDescription(IMetadataObject metadataObject)
	{
		return metadataObject?.Accept(m_description);
	}

	public string GetDatabaseQualifiedName(IMetadataObject metadataObject)
	{
		return metadataObject?.Accept(m_dbQualifiedName);
	}

	public string ObjectToString(IMetadataObject metadataObject)
	{
		return metadataObject?.Accept(m_objectString);
	}

	public string CollectionToString<T>(IMetadataCollection<T> metadataCollection, bool singleLine) where T : class, IMetadataObject
	{
		if (metadataCollection == null)
		{
			return null;
		}
		string result = null;
		switch (metadataCollection.Count)
		{
		case 0:
			result = string.Empty;
			break;
		case 1:
			foreach (T item in metadataCollection)
			{
				result = ObjectToString(item);
			}
			break;
		default:
		{
			string paramListSeparator = GetParamListSeparator(singleLine);
			StringBuilder stringBuilder = new StringBuilder();
			foreach (T item2 in metadataCollection)
			{
				stringBuilder.Append(ObjectToString(item2));
				stringBuilder.Append(paramListSeparator);
			}
			result = stringBuilder.ToString(0, stringBuilder.Length - paramListSeparator.Length);
			break;
		}
		}
		return result;
	}

	public string CollectionToString<T>(IMetadataOrderedCollection<T> metadataCollection, bool singleLine) where T : class, IMetadataObject
	{
		if (metadataCollection == null)
		{
			return null;
		}
		int count = metadataCollection.Count;
		switch (count)
		{
		case 0:
			return string.Empty;
		case 1:
			return ObjectToString(metadataCollection[0]);
		case 2:
			return ObjectToString(metadataCollection[0]) + GetParamListSeparator(singleLine) + ObjectToString(metadataCollection[1]);
		default:
		{
			string paramListSeparator = GetParamListSeparator(singleLine);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < count - 1; i++)
			{
				stringBuilder.Append(ObjectToString(metadataCollection[i]));
				stringBuilder.Append(paramListSeparator);
			}
			stringBuilder.Append(ObjectToString(metadataCollection[count - 1]));
			return stringBuilder.ToString();
		}
		}
	}

	private static string GetParamListSeparator(bool singleLine)
	{
		if (!singleLine)
		{
			return "," + Environment.NewLine;
		}
		return ", ";
	}

	private string MakeDatabaseQualifiedName(ISchemaOwnedObject schemaObject)
	{
		ISchema schema = schemaObject.Schema;
		IDatabase database = schema.Database;
		return database.Name + "." + schema.Name + "." + schemaObject.Name;
	}

	private string MakeDatabaseQualifiedName(IDatabaseOwnedObject dbOwnedObject)
	{
		return dbOwnedObject.Database.Name + "." + dbOwnedObject.Name;
	}

	private string MakeDatabaseQualifiedName(IDatabase database)
	{
		return database.Name;
	}

	private string MakeDescription(string metadataTypeName, string metadataObjectString)
	{
		if (metadataTypeName == null)
		{
			return metadataObjectString;
		}
		return metadataTypeName + " " + metadataObjectString;
	}

	private string MakeAliasedObjectDescription(IMetadataObject aliasedObject, string alias)
	{
		return GetDescription(aliasedObject) + " AS " + alias;
	}

	private string FunctionToString(string name, string paramList, string returnType)
	{
		return string.Format(CultureInfo.InvariantCulture, "{0}({1}) RETURNS {2}", name, paramList, returnType);
	}
}
