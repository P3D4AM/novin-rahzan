using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

internal sealed class SchemaEx : DatabaseObjectEx<ISchema>, ISchema, IDatabaseOwnedObject, IDatabaseObject, IMetadataObject
{
	private enum SpecialType
	{
		None,
		DboSchema,
		EmptySchema,
		SysSchema,
		TempTableSchema,
		UnnamedSchema
	}

	private readonly DatabaseEx m_parent;

	private readonly SpecialType m_specialType;

	private readonly SchemaEx m_singleSchema;

	private readonly IAlterableScopedMetadataCollection<ITable, IMutableTable> m_tables;

	private readonly IAlterableScopedMetadataCollection<IView, IMutableView> m_views;

	private readonly IScopedMetadataCollection<IUserDefinedAggregate, IUserDefinedAggregate> m_userDefinedAggregates;

	private readonly IScopedMetadataCollection<ITableValuedFunction, IMutableTableValuedFunction> m_tableValuedFunctions;

	private readonly IScopedMetadataCollection<IScalarValuedFunction, IMutableScalarValuedFunction> m_scalarValuedFunctions;

	private readonly IScopedMetadataCollection<IStoredProcedure, IMutableStoredProcedure> m_storedProcedures;

	private readonly IScopedMetadataCollection<ISynonym, IMutableSynonym> m_synonyms;

	private readonly IScopedMetadataCollection<IExtendedStoredProcedure, IExtendedStoredProcedure> m_extendedStoredProcedures;

	private readonly IScopedMetadataCollection<IUserDefinedDataType, IMutableUserDefinedDataType> m_userDefinedDataTypes;

	private readonly IScopedMetadataCollection<IUserDefinedTableType, IMutableUserDefinedTableType> m_userDefinedTableTypes;

	private readonly IScopedMetadataCollection<IUserDefinedClrType, IMutableUserDefinedClrType> m_userDefinedClrTypes;

	private ITable m_emptyTable;

	public bool IsDboSchema => m_specialType == SpecialType.DboSchema;

	public bool IsSysSchema => m_specialType == SpecialType.SysSchema;

	public bool IsUnnamedSchema => m_specialType == SpecialType.UnnamedSchema;

	public bool IsEmptySchema => m_specialType == SpecialType.EmptySchema;

	public bool IsSingleSchema => this == m_singleSchema;

	public SchemaEx AsSingleSchema => m_singleSchema;

	public ITable EmptyTable
	{
		get
		{
			if (m_emptyTable == null)
			{
				m_emptyTable = Database.Server.MetadataFactory.Tabular.CreateTable(base.ProxyObject, string.Empty);
			}
			return m_emptyTable;
		}
	}

	public DatabaseEx Database => m_parent;

	public IAlterableScopedMetadataCollection<ITable, IMutableTable> Tables => m_tables;

	public IAlterableScopedMetadataCollection<IView, IMutableView> Views => m_views;

	public IScopedMetadataCollection<IUserDefinedAggregate, IUserDefinedAggregate> UserDefinedAggregates => m_userDefinedAggregates;

	public IScopedMetadataCollection<ITableValuedFunction, IMutableTableValuedFunction> TableValuedFunctions => m_tableValuedFunctions;

	public IScopedMetadataCollection<IScalarValuedFunction, IMutableScalarValuedFunction> ScalarValuedFunctions => m_scalarValuedFunctions;

	public IScopedMetadataCollection<IStoredProcedure, IMutableStoredProcedure> StoredProcedures => m_storedProcedures;

	public IScopedMetadataCollection<ISynonym, IMutableSynonym> Synonyms => m_synonyms;

	public IScopedMetadataCollection<IExtendedStoredProcedure, IExtendedStoredProcedure> ExtendedStoredProcedures => m_extendedStoredProcedures;

	public IScopedMetadataCollection<IUserDefinedDataType, IMutableUserDefinedDataType> UserDefinedDataTypes => m_userDefinedDataTypes;

	public IScopedMetadataCollection<IUserDefinedTableType, IMutableUserDefinedTableType> UserDefinedTableTypes => m_userDefinedTableTypes;

	public IScopedMetadataCollection<IUserDefinedClrType, IMutableUserDefinedClrType> UserDefinedClrTypes => m_userDefinedClrTypes;

	IDatabase IDatabaseOwnedObject.Database => Database;

	IDatabasePrincipal ISchema.Owner => m_proxyObject.Owner;

	IMetadataCollection<ITable> ISchema.Tables => m_tables.GlobalMetadata;

	IMetadataCollection<IView> ISchema.Views => m_views.GlobalMetadata;

	IMetadataCollection<IUserDefinedAggregate> ISchema.UserDefinedAggregates => m_userDefinedAggregates.GlobalMetadata;

	IMetadataCollection<ITableValuedFunction> ISchema.TableValuedFunctions => m_tableValuedFunctions.GlobalMetadata;

	IMetadataCollection<IScalarValuedFunction> ISchema.ScalarValuedFunctions => m_scalarValuedFunctions.GlobalMetadata;

	IMetadataCollection<IStoredProcedure> ISchema.StoredProcedures => m_storedProcedures.GlobalMetadata;

	IMetadataCollection<ISynonym> ISchema.Synonyms => m_synonyms.GlobalMetadata;

	IMetadataCollection<IExtendedStoredProcedure> ISchema.ExtendedStoredProcedures => m_extendedStoredProcedures.GlobalMetadata;

	IMetadataCollection<IUserDefinedDataType> ISchema.UserDefinedDataTypes => m_userDefinedDataTypes.GlobalMetadata;

	IMetadataCollection<IUserDefinedTableType> ISchema.UserDefinedTableTypes => m_userDefinedTableTypes.GlobalMetadata;

	IMetadataCollection<IUserDefinedClrType> ISchema.UserDefinedClrTypes => m_userDefinedClrTypes.GlobalMetadata;

	public static SchemaEx CreateEmptySchema(DatabaseEx database)
	{
		return new SchemaEx(database, SpecialType.EmptySchema);
	}

	public static SchemaEx CreateTempTableSchema(DatabaseEx database)
	{
		return new SchemaEx(database, SpecialType.TempTableSchema);
	}

	public static SchemaEx CreateUnnamedSchema(DatabaseEx database)
	{
		return CreateUnnamedSchema(database, null);
	}

	public static SchemaEx CreateUnnamedSchema(DatabaseEx database, SchemaEx defaultSchema)
	{
		if (defaultSchema == null)
		{
			defaultSchema = database.DefaultSchema;
		}
		return new SchemaEx(database, defaultSchema);
	}

	public SchemaEx(DatabaseEx parent, ISchema proxySchema)
		: this(parent, proxySchema, GetSchemaSpecialType(proxySchema))
	{
		CollationInfo collationInfo = parent.CollationInfo;
		Duplicator metadataDuplicator = Database.Server.MetadataDuplicator;
		m_tables = ScopedMetadataCollection<ITable, IMutableTable>.CreateImmutableCollection(proxySchema.Tables, collationInfo, metadataDuplicator);
		m_views = ScopedMetadataCollection<IView, IMutableView>.CreateImmutableCollection(proxySchema.Views, collationInfo, metadataDuplicator);
		m_userDefinedAggregates = ScopedMetadataCollection<IUserDefinedAggregate, IUserDefinedAggregate>.CreateImmutableCollection(proxySchema.UserDefinedAggregates, collationInfo);
		m_tableValuedFunctions = ScopedMetadataCollection<ITableValuedFunction, IMutableTableValuedFunction>.CreateImmutableCollection(proxySchema.TableValuedFunctions, collationInfo);
		m_scalarValuedFunctions = ScopedMetadataCollection<IScalarValuedFunction, IMutableScalarValuedFunction>.CreateImmutableCollection(proxySchema.ScalarValuedFunctions, collationInfo);
		m_storedProcedures = ScopedMetadataCollection<IStoredProcedure, IMutableStoredProcedure>.CreateImmutableCollection(proxySchema.StoredProcedures, collationInfo);
		m_synonyms = ScopedMetadataCollection<ISynonym, IMutableSynonym>.CreateImmutableCollection(proxySchema.Synonyms, collationInfo);
		m_extendedStoredProcedures = ScopedMetadataCollection<IExtendedStoredProcedure, IExtendedStoredProcedure>.CreateImmutableCollection(proxySchema.ExtendedStoredProcedures, collationInfo);
		m_userDefinedDataTypes = ScopedMetadataCollection<IUserDefinedDataType, IMutableUserDefinedDataType>.CreateImmutableCollection(proxySchema.UserDefinedDataTypes, collationInfo);
		m_userDefinedTableTypes = ScopedMetadataCollection<IUserDefinedTableType, IMutableUserDefinedTableType>.CreateImmutableCollection(proxySchema.UserDefinedTableTypes, collationInfo);
		m_userDefinedClrTypes = ScopedMetadataCollection<IUserDefinedClrType, IMutableUserDefinedClrType>.CreateImmutableCollection(proxySchema.UserDefinedClrTypes, collationInfo);
	}

	public SchemaEx(DatabaseEx parent, IMutableSchema proxySchema)
		: this(parent, proxySchema, GetSchemaSpecialType(proxySchema))
	{
		CollationInfo collationInfo = parent.CollationInfo;
		Duplicator metadataDuplicator = Database.Server.MetadataDuplicator;
		m_tables = ScopedMetadataCollection<ITable, IMutableTable>.CreateMutableCollection(proxySchema.Tables, collationInfo, metadataDuplicator);
		m_views = ScopedMetadataCollection<IView, IMutableView>.CreateMutableCollection(proxySchema.Views, collationInfo, metadataDuplicator);
		m_userDefinedAggregates = ScopedMetadataCollection<IUserDefinedAggregate, IUserDefinedAggregate>.CreateMutableCollection(proxySchema.UserDefinedAggregates, collationInfo);
		m_tableValuedFunctions = ScopedMetadataCollection<ITableValuedFunction, IMutableTableValuedFunction>.CreateMutableCollection(proxySchema.TableValuedFunctions, collationInfo);
		m_scalarValuedFunctions = ScopedMetadataCollection<IScalarValuedFunction, IMutableScalarValuedFunction>.CreateMutableCollection(proxySchema.ScalarValuedFunctions, collationInfo);
		m_storedProcedures = ScopedMetadataCollection<IStoredProcedure, IMutableStoredProcedure>.CreateMutableCollection(proxySchema.StoredProcedures, collationInfo);
		m_synonyms = ScopedMetadataCollection<ISynonym, IMutableSynonym>.CreateMutableCollection(proxySchema.Synonyms, collationInfo);
		m_extendedStoredProcedures = ScopedMetadataCollection<IExtendedStoredProcedure, IExtendedStoredProcedure>.CreateMutableCollection(proxySchema.ExtendedStoredProcedures, collationInfo);
		m_userDefinedDataTypes = ScopedMetadataCollection<IUserDefinedDataType, IMutableUserDefinedDataType>.CreateMutableCollection(proxySchema.UserDefinedDataTypes, collationInfo);
		m_userDefinedTableTypes = ScopedMetadataCollection<IUserDefinedTableType, IMutableUserDefinedTableType>.CreateMutableCollection(proxySchema.UserDefinedTableTypes, collationInfo);
		m_userDefinedClrTypes = ScopedMetadataCollection<IUserDefinedClrType, IMutableUserDefinedClrType>.CreateMutableCollection(proxySchema.UserDefinedClrTypes, collationInfo);
	}

	private SchemaEx(DatabaseEx parent, SchemaEx defaultSchema)
		: this(parent, defaultSchema.m_proxyObject, SpecialType.UnnamedSchema)
	{
		SchemaEx dboSchema = parent.DboSchema;
		SchemaEx sysSchema = parent.SysSchema;
		m_tables = ScopedMetadataCollection<ITable, IMutableTable>.Merge(defaultSchema.Tables, dboSchema.Tables);
		m_views = ScopedMetadataCollection<IView, IMutableView>.Merge(defaultSchema.Views, dboSchema.Views);
		m_userDefinedAggregates = ScopedMetadataCollection<IUserDefinedAggregate, IUserDefinedAggregate>.Merge(defaultSchema.UserDefinedAggregates, dboSchema.UserDefinedAggregates);
		m_tableValuedFunctions = ScopedMetadataCollection<ITableValuedFunction, IMutableTableValuedFunction>.Merge(defaultSchema.TableValuedFunctions, dboSchema.TableValuedFunctions, sysSchema.TableValuedFunctions);
		m_scalarValuedFunctions = ScopedMetadataCollection<IScalarValuedFunction, IMutableScalarValuedFunction>.Merge(defaultSchema.ScalarValuedFunctions, dboSchema.ScalarValuedFunctions, sysSchema.ScalarValuedFunctions);
		m_storedProcedures = ScopedMetadataCollection<IStoredProcedure, IMutableStoredProcedure>.Merge(defaultSchema.StoredProcedures, dboSchema.StoredProcedures, sysSchema.StoredProcedures);
		m_synonyms = ScopedMetadataCollection<ISynonym, IMutableSynonym>.Merge(defaultSchema.Synonyms, dboSchema.Synonyms, sysSchema.Synonyms);
		m_extendedStoredProcedures = ScopedMetadataCollection<IExtendedStoredProcedure, IExtendedStoredProcedure>.Merge(defaultSchema.ExtendedStoredProcedures, dboSchema.ExtendedStoredProcedures, sysSchema.ExtendedStoredProcedures);
		m_userDefinedDataTypes = ScopedMetadataCollection<IUserDefinedDataType, IMutableUserDefinedDataType>.Merge(defaultSchema.UserDefinedDataTypes, dboSchema.UserDefinedDataTypes);
		m_userDefinedTableTypes = ScopedMetadataCollection<IUserDefinedTableType, IMutableUserDefinedTableType>.Merge(defaultSchema.UserDefinedTableTypes, dboSchema.UserDefinedTableTypes);
		m_userDefinedClrTypes = ScopedMetadataCollection<IUserDefinedClrType, IMutableUserDefinedClrType>.Merge(defaultSchema.UserDefinedClrTypes, dboSchema.UserDefinedClrTypes);
		m_singleSchema = (defaultSchema.IsEmptySchema ? dboSchema : defaultSchema);
	}

	private SchemaEx(DatabaseEx parent, SpecialType specialType)
		: this(parent, new EmptySchema(parent, string.Empty, isSystemObject: false), specialType)
	{
		if (specialType == SpecialType.TempTableSchema)
		{
			Duplicator metadataDuplicator = Database.Server.MetadataDuplicator;
			m_tables = ScopedMetadataCollection<ITable, IMutableTable>.CreateImmutableCollection(m_proxyObject.Tables, parent.CollationInfo, metadataDuplicator);
		}
		else
		{
			m_tables = ScopedMetadataCollection<ITable, IMutableTable>.Empty;
		}
		m_views = ScopedMetadataCollection<IView, IMutableView>.Empty;
		m_userDefinedAggregates = ScopedMetadataCollection<IUserDefinedAggregate, IUserDefinedAggregate>.Empty;
		m_tableValuedFunctions = ScopedMetadataCollection<ITableValuedFunction, IMutableTableValuedFunction>.Empty;
		m_scalarValuedFunctions = ScopedMetadataCollection<IScalarValuedFunction, IMutableScalarValuedFunction>.Empty;
		m_storedProcedures = ScopedMetadataCollection<IStoredProcedure, IMutableStoredProcedure>.Empty;
		m_synonyms = ScopedMetadataCollection<ISynonym, IMutableSynonym>.Empty;
		m_extendedStoredProcedures = ScopedMetadataCollection<IExtendedStoredProcedure, IExtendedStoredProcedure>.Empty;
		m_userDefinedDataTypes = ScopedMetadataCollection<IUserDefinedDataType, IMutableUserDefinedDataType>.Empty;
		m_userDefinedTableTypes = ScopedMetadataCollection<IUserDefinedTableType, IMutableUserDefinedTableType>.Empty;
		m_userDefinedClrTypes = ScopedMetadataCollection<IUserDefinedClrType, IMutableUserDefinedClrType>.Empty;
	}

	private SchemaEx(DatabaseEx parent, ISchema proxySchema, SpecialType specialType)
		: base(proxySchema)
	{
		m_parent = parent;
		m_specialType = specialType;
		m_singleSchema = this;
	}

	public static bool IsNullOrEmpty(SchemaEx schema)
	{
		return schema?.IsEmptySchema ?? true;
	}

	public ITableViewBase FindTableOrView(string name, LexicalScope lexicalScope)
	{
		ITableViewBase tableViewBase = m_tables[name, lexicalScope];
		if (tableViewBase != null)
		{
			return tableViewBase;
		}
		return m_views[name, lexicalScope];
	}

	public ICallableModule FindModule(string name, LexicalScope lexicalScope)
	{
		ICallableModule callableModule = m_storedProcedures[name, lexicalScope];
		if (callableModule != null)
		{
			return callableModule;
		}
		callableModule = m_scalarValuedFunctions[name, lexicalScope];
		if (callableModule != null)
		{
			return callableModule;
		}
		return m_extendedStoredProcedures[name, lexicalScope];
	}

	public IUserDefinedType FindUserDefinedTypes(string name, LexicalScope lexicalScope)
	{
		IUserDefinedType userDefinedType = m_userDefinedDataTypes[name, lexicalScope];
		if (userDefinedType != null)
		{
			return userDefinedType;
		}
		userDefinedType = m_userDefinedTableTypes[name, lexicalScope];
		if (userDefinedType != null)
		{
			return userDefinedType;
		}
		return m_userDefinedClrTypes[name, lexicalScope];
	}

	public T Accept<T>(IDatabaseOwnedObjectVisitor<T> visitor)
	{
		return m_proxyObject.Accept(visitor);
	}

	private static SpecialType GetSchemaSpecialType(ISchema schema)
	{
		string name = schema.Name;
		IEqualityComparer<string> equalityComparer = schema.Database.CollationInfo.EqualityComparer;
		if (equalityComparer.Equals(name, "dbo"))
		{
			return SpecialType.DboSchema;
		}
		if (equalityComparer.Equals(name, "sys"))
		{
			return SpecialType.SysSchema;
		}
		return SpecialType.None;
	}
}
