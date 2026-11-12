using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

internal class BatchBindingContext : IBindingContext
{
	private delegate T FindObjectDelegate<T>(BatchBindingContext bindingContext, string databaseName, string schemaName, string name, LexicalScope lexicalScope, bool allowSynonyms) where T : ISchemaOwnedObject;

	private sealed class SwitchDatabaseBindingContext : BatchBindingContext
	{
		public SwitchDatabaseBindingContext(BatchBindingContext baseBindingContext, DatabaseEx database)
			: base(baseBindingContext, database)
		{
		}
	}

	private sealed class ModuleBindingContext : BatchBindingContext
	{
		private readonly BatchBindingContext m_baseBindingContext;

		public override BatchBindingContext Root => m_baseBindingContext;

		public ModuleBindingContext(BatchBindingContext baseBindingContext, SchemaEx schema, IMetadataCollection<ITabular> dmlTables)
			: base(baseBindingContext, schema, dmlTables)
		{
			m_baseBindingContext = baseBindingContext.Root;
		}
	}

	private sealed class ResolvedSynonymConstructor : ISchemaOwnedObjectVisitor<IResolvedSynonym>
	{
		private readonly IMetadataFactory metadataFactory;

		private readonly ISynonym synonym;

		private ResolvedSynonymConstructor(IMetadataFactory metadataFactory, ISynonym synonym)
		{
			this.metadataFactory = metadataFactory;
			this.synonym = synonym;
		}

		public static T CreateResolvedSynonym<T>(IMetadataFactory metadataFactory, ISynonym synonym, ISchemaOwnedObject targetObject) where T : class, ISchemaOwnedObject
		{
			ResolvedSynonymConstructor visitor = new ResolvedSynonymConstructor(metadataFactory, synonym);
			return (T)targetObject.Accept(visitor);
		}

		public IResolvedSynonym Visit(IExtendedStoredProcedure extendedStoredProcedure)
		{
			return metadataFactory.ResolvedSynonym.CreateResolvedExtendedStoredProcedureSynonym(synonym, extendedStoredProcedure);
		}

		public IResolvedSynonym Visit(IScalarValuedFunction scalarValuedFunction)
		{
			return metadataFactory.ResolvedSynonym.CreateResolvedScalarValuedFunctionSynonym(synonym, scalarValuedFunction);
		}

		public IResolvedSynonym Visit(IStoredProcedure storedProcedure)
		{
			return metadataFactory.ResolvedSynonym.CreateResolvedStoredProcedureSynonym(synonym, storedProcedure);
		}

		public IResolvedSynonym Visit(ISynonym synonym)
		{
			return null;
		}

		public IResolvedSynonym Visit(ITable table)
		{
			return metadataFactory.ResolvedSynonym.CreateResolvedTableSynonym(synonym, table);
		}

		public IResolvedSynonym Visit(ITableValuedFunction tableValuedFunction)
		{
			return metadataFactory.ResolvedSynonym.CreateResolvedTableValuedFunctionSynonym(synonym, tableValuedFunction);
		}

		public IResolvedSynonym Visit(IUserDefinedAggregate userDefinedAggregate)
		{
			return metadataFactory.ResolvedSynonym.CreateResolvedUserDefinedAggregateSynonym(synonym, userDefinedAggregate);
		}

		public IResolvedSynonym Visit(IUserDefinedClrType userDefinedClrType)
		{
			return null;
		}

		public IResolvedSynonym Visit(IUserDefinedDataType userDefinedDataType)
		{
			return null;
		}

		public IResolvedSynonym Visit(IUserDefinedTableType userDefinedTableType)
		{
			return null;
		}

		public IResolvedSynonym Visit(IView view)
		{
			return metadataFactory.ResolvedSynonym.CreateResolvedViewSynonym(synonym, view);
		}
	}

	private const string IdExpr = "((?<id>\\[[^\\]]*(\\]\\][^\\]]*)*\\])|(?<id>\"[^\"]*(\"\"[^\"]*)*\")|(?<id>[^\\[\\.\"]*))";

	private static readonly Regex ObjectNameRegex = new Regex("^((?<id>\\[[^\\]]*(\\]\\][^\\]]*)*\\])|(?<id>\"[^\"]*(\"\"[^\"]*)*\")|(?<id>[^\\[\\.\"]*))(\\.((?<id>\\[[^\\]]*(\\]\\][^\\]]*)*\\])|(?<id>\"[^\"]*(\"\"[^\"]*)*\")|(?<id>[^\\[\\.\"]*)))*$");

	private static readonly FindObjectDelegate<ITableViewBase> findTableOrViewDelegate = FindTableOrView;

	private static readonly FindObjectDelegate<ITableValuedFunction> findTableValuedFunctionDelegate = FindTableValuedFunction;

	private static readonly FindObjectDelegate<IScalarValuedFunction> findScalarValuedFunctionDelegate = FindScalarValuedFunction;

	private static readonly FindObjectDelegate<IUserDefinedAggregate> findUserDefinedAggregateDelegate = FindUserDefinedAggregate;

	private static readonly FindObjectDelegate<ICallableModule> findModuleDelegate = FindModule;

	private static readonly FindObjectDelegate<IStoredProcedure> findStoredProcedureDelegate = FindStoredProcedure;

	private static readonly FindObjectDelegate<ISchemaOwnedObject> findSchemaOwnedObjectDelegate = FindSchemaOwnedObject;

	private readonly ServerEx m_server;

	private readonly DatabaseEx m_connectedDatabase;

	private readonly SchemaEx m_unnamedSchema;

	private readonly LocalSymbolTable m_localSymbolTable;

	private readonly IMetadataCollection<ITabular> m_dmlTables;

	private readonly IMetadataCollection<ITableValuedFunction> m_builtInTableValuedFunctions;

	private readonly Dictionary<object, IMetadataObject> m_boundObjectMap;

	public virtual BatchBindingContext Root => this;

	public bool IsRoot => this == Root;

	public DatabaseEx ConnectedDatabase => m_connectedDatabase;

	public string ConnectedDatabaseName => m_connectedDatabase.Name;

	public BatchBindingContext Batch => this;

	public BatchBindingContext AsBatchBindingContext => this;

	public LocalBindingContext AsLocalBindingContext => null;

	public CollationInfo DatabaseCollation => m_connectedDatabase.CollationInfo;

	public CollationInfo ServerCollation => m_server.CollationInfo;

	public IMetadataFactory MetadataFactory => m_server.MetadataFactory;

	public IMetadataCollection<ITabular> LocalTableExpressions => Collection<ITabular>.Empty;

	public IMetadataOrderedCollection<IColumn> LocalColumnExpressions => Collection<IColumn>.EmptyOrdered;

	public BatchBindingContext(DatabaseEx database)
	{
		m_server = database.Server;
		m_connectedDatabase = database;
		m_unnamedSchema = m_connectedDatabase.UnnamedSchema;
		m_builtInTableValuedFunctions = TableValuedFunctionCollection.CreateBuiltInCollection(database.EmptySchema);
		m_localSymbolTable = new LocalSymbolTable(m_server.CollationInfo);
		m_dmlTables = Collection<ITabular>.Empty;
		m_boundObjectMap = new Dictionary<object, IMetadataObject>(500);
	}

	private BatchBindingContext(BatchBindingContext baseBindingContext, SchemaEx unnamedSchema, IMetadataCollection<ITabular> dmlTables)
		: this(baseBindingContext)
	{
		m_unnamedSchema = unnamedSchema;
		m_dmlTables = dmlTables;
		m_localSymbolTable = new LocalSymbolTable(m_server.CollationInfo);
	}

	private BatchBindingContext(BatchBindingContext baseBindingContext, SchemaEx unnamedSchema)
		: this(baseBindingContext, unnamedSchema, Collection<ITabular>.Empty)
	{
	}

	private BatchBindingContext(BatchBindingContext baseBindingContext, DatabaseEx connectedDatabase)
		: this(baseBindingContext)
	{
		m_connectedDatabase = connectedDatabase;
		m_unnamedSchema = m_connectedDatabase.UnnamedSchema;
	}

	protected BatchBindingContext(BatchBindingContext baseBindingContext)
	{
		m_server = baseBindingContext.m_server;
		m_connectedDatabase = baseBindingContext.m_connectedDatabase;
		m_unnamedSchema = baseBindingContext.m_unnamedSchema;
		m_builtInTableValuedFunctions = baseBindingContext.m_builtInTableValuedFunctions;
		m_localSymbolTable = baseBindingContext.m_localSymbolTable;
		m_dmlTables = baseBindingContext.m_dmlTables;
		m_boundObjectMap = baseBindingContext.m_boundObjectMap;
	}

	public BatchBindingContext GetOrCreateModuleBindingContext(SchemaEx schema, IMetadataCollection<ITabular> dmlTables)
	{
		SchemaEx schemaEx = m_connectedDatabase.MakeUnnamedSchema(schema);
		if (schemaEx == m_unnamedSchema && m_localSymbolTable.IsEmpty && dmlTables.Count == 0)
		{
			return this;
		}
		return new ModuleBindingContext(this, schemaEx, dmlTables);
	}

	public BatchBindingContext GetOrCreateModuleBindingContext(SchemaEx schema)
	{
		return GetOrCreateModuleBindingContext(schema, Collection<ITabular>.Empty);
	}

	public virtual CteBindingContext CreateCteBindingContext()
	{
		return new CteBindingContext(this);
	}

	public BatchBindingContext GetSiblingBatchBindingContext(DatabaseEx database)
	{
		if (database == null)
		{
			database = m_connectedDatabase;
		}
		if (!m_localSymbolTable.IsEmpty || m_connectedDatabase != database)
		{
			return new BatchBindingContext(database);
		}
		return this;
	}

	public BatchBindingContext GetBindingContextForDatabase(DatabaseEx database)
	{
		if (database == null || database == m_connectedDatabase)
		{
			return this;
		}
		return new SwitchDatabaseBindingContext(this, database);
	}

	public bool AddLocalTableVariable(ITableVariable variable, LexicalScope lexicalScope)
	{
		return m_localSymbolTable.Add(variable, lexicalScope);
	}

	public bool AddLocalScalarVariable(IScalarVariable variable, LexicalScope lexicalScope)
	{
		return m_localSymbolTable.Add(variable, lexicalScope);
	}

	public bool AddLocalCursorVariable(ICursorVariable variable, LexicalScope lexicalScope)
	{
		return m_localSymbolTable.Add(variable, lexicalScope);
	}

	public virtual CteScopeSnapshot CaptureCurrentCteSnapshot()
	{
		return null;
	}

	public ServerEx GetServer()
	{
		return m_server;
	}

	public ITableViewBase FindTableOrView(string databaseName, string schemaName, string name, LexicalScope lexicalScope)
	{
		return FindTableOrView(this, databaseName, schemaName, name, lexicalScope, allowSynonyms: true);
	}

	private static ITableViewBase FindTableOrView(BatchBindingContext bindingContext, string databaseName, string schemaName, string name, LexicalScope lexicalScope, bool allowSynonyms)
	{
		SchemaEx schema = bindingContext.GetSchema(databaseName, schemaName, lexicalScope);
		ITableViewBase tableViewBase = null;
		if (schema != null)
		{
			tableViewBase = FindTableOrViewInSchema(schema, name, lexicalScope);
		}
		if (tableViewBase == null && schemaName == null && databaseName == null)
		{
			tableViewBase = bindingContext.FindTempTable(name, lexicalScope);
		}
		if (tableViewBase == null && allowSynonyms)
		{
			tableViewBase = bindingContext.FindAndResolveSynonym(databaseName, schemaName, name, lexicalScope, findTableOrViewDelegate);
		}
		return tableViewBase;
	}

	public static ITableViewBase FindTableOrViewInSchema(SchemaEx schema, string name, LexicalScope lexicalScope)
	{
		if ((schema.IsDboSchema || schema.IsUnnamedSchema) && schema.Database.IsY2KCatalogViewName(name))
		{
			schema = schema.Database.SysSchema;
		}
		return schema.FindTableOrView(name, lexicalScope);
	}

	public ITable FindTempTable(string name, LexicalScope lexicalScope)
	{
		return m_server.TempTableSchema?.Tables[name, lexicalScope];
	}

	public ITableValuedFunction FindTableValuedFunction(string databaseName, string schemaName, string name, LexicalScope lexicalScope)
	{
		if (string.IsNullOrEmpty(databaseName) && string.IsNullOrEmpty(schemaName))
		{
			ITableValuedFunction tableValuedFunction = m_builtInTableValuedFunctions[name];
			if (tableValuedFunction != null)
			{
				return tableValuedFunction;
			}
		}
		return FindTableValuedFunction(this, databaseName, schemaName, name, lexicalScope, allowSynonyms: true);
	}

	private static ITableValuedFunction FindTableValuedFunction(BatchBindingContext bindingContext, string databaseName, string schemaName, string name, LexicalScope lexicalScope, bool allowSynonyms)
	{
		SchemaEx schema = bindingContext.GetSchema(databaseName, schemaName, lexicalScope);
		ITableValuedFunction tableValuedFunction = null;
		if (schema != null)
		{
			tableValuedFunction = FindTableValuedFunctionInSchema(schema, name, lexicalScope);
		}
		if (tableValuedFunction == null && allowSynonyms)
		{
			tableValuedFunction = bindingContext.FindAndResolveSynonym(databaseName, schemaName, name, lexicalScope, findTableValuedFunctionDelegate);
		}
		return tableValuedFunction;
	}

	public static ITableValuedFunction FindTableValuedFunctionInSchema(SchemaEx schema, string name, LexicalScope lexicalScope)
	{
		ITableValuedFunction tableValuedFunction = null;
		if ((schema.IsDboSchema || schema.IsUnnamedSchema) && schema.Database.HasMSReservedNamePrefix(name))
		{
			tableValuedFunction = ((ISchema)schema.Database.SysSchema).TableValuedFunctions[name];
		}
		if (tableValuedFunction == null)
		{
			tableValuedFunction = schema.TableValuedFunctions[name, lexicalScope];
		}
		return tableValuedFunction;
	}

	public ITabular FindBatchTableExpression(string databaseName, string schemaName, string name, LexicalScope lexicalScope)
	{
		ITabular tabular = FindTableOrView(databaseName, schemaName, name, lexicalScope);
		if (tabular == null)
		{
			tabular = FindTableValuedFunction(databaseName, schemaName, name, lexicalScope);
		}
		if (tabular == null && databaseName == null && schemaName == null)
		{
			tabular = FindCommonTableExpression(name);
		}
		return tabular;
	}

	public IScalarValuedFunction FindScalarValuedFunction(string databaseName, string schemaName, string name, LexicalScope lexicalScope)
	{
		return FindScalarValuedFunction(this, databaseName, schemaName, name, lexicalScope, allowSynonyms: true);
	}

	private static IScalarValuedFunction FindScalarValuedFunction(BatchBindingContext bindingContext, string databaseName, string schemaName, string name, LexicalScope lexicalScope, bool allowSynonyms)
	{
		SchemaEx schema = bindingContext.GetSchema(databaseName, schemaName, lexicalScope);
		IScalarValuedFunction scalarValuedFunction = null;
		if (schema != null)
		{
			scalarValuedFunction = FindScalarValuedFunctionInSchema(schema, name, lexicalScope);
		}
		if (scalarValuedFunction == null && allowSynonyms)
		{
			scalarValuedFunction = bindingContext.FindAndResolveSynonym(databaseName, schemaName, name, lexicalScope, findScalarValuedFunctionDelegate);
		}
		return scalarValuedFunction;
	}

	public static IScalarValuedFunction FindScalarValuedFunctionInSchema(SchemaEx schema, string name, LexicalScope lexicalScope)
	{
		IScalarValuedFunction scalarValuedFunction = null;
		if ((schema.IsDboSchema || schema.IsUnnamedSchema) && schema.Database.HasMSReservedNamePrefix(name))
		{
			scalarValuedFunction = ((ISchema)schema.Database.SysSchema).ScalarValuedFunctions[name];
		}
		if (scalarValuedFunction == null)
		{
			scalarValuedFunction = schema.ScalarValuedFunctions[name, lexicalScope];
		}
		return scalarValuedFunction;
	}

	public IUserDefinedAggregate FindUserDefinedAggregate(string databaseName, string schemaName, string name, LexicalScope lexicalScope)
	{
		return FindUserDefinedAggregate(this, databaseName, schemaName, name, lexicalScope, allowSynonyms: true);
	}

	private static IUserDefinedAggregate FindUserDefinedAggregate(BatchBindingContext bindingContext, string databaseName, string schemaName, string name, LexicalScope lexicalScope, bool allowSynonyms)
	{
		SchemaEx schema = bindingContext.GetSchema(databaseName, schemaName, lexicalScope);
		IUserDefinedAggregate userDefinedAggregate = null;
		if (schema != null && !schema.IsUnnamedSchema)
		{
			userDefinedAggregate = schema.UserDefinedAggregates[name, lexicalScope];
		}
		if (userDefinedAggregate == null && allowSynonyms)
		{
			userDefinedAggregate = bindingContext.FindAndResolveSynonym(databaseName, schemaName, name, lexicalScope, findUserDefinedAggregateDelegate);
		}
		return userDefinedAggregate;
	}

	public ICallableModule FindModule(string databaseName, string schemaName, string name, LexicalScope lexicalScope)
	{
		return FindModule(this, databaseName, schemaName, name, lexicalScope, allowSynonyms: true);
	}

	private static ICallableModule FindModule(BatchBindingContext bindingContext, string databaseName, string schemaName, string name, LexicalScope lexicalScope, bool allowSynonyms)
	{
		SchemaEx schema = bindingContext.GetSchema(databaseName, schemaName, lexicalScope);
		ICallableModule callableModule = null;
		if (schema != null)
		{
			callableModule = FindModuleInSchema(schema, name, lexicalScope);
		}
		if (callableModule == null && allowSynonyms)
		{
			callableModule = bindingContext.FindAndResolveSynonym(databaseName, schemaName, name, lexicalScope, findModuleDelegate);
		}
		return callableModule;
	}

	public static ICallableModule FindModuleInSchema(SchemaEx schema, string name, LexicalScope lexicalScope)
	{
		ICallableModule callableModule = null;
		if ((schema.IsDboSchema || schema.IsUnnamedSchema) && schema.Database.HasMSReservedNamePrefix(name))
		{
			callableModule = schema.Database.SysSchema.FindModule(name, null);
		}
		if (callableModule == null)
		{
			callableModule = schema.FindModule(name, lexicalScope);
		}
		return callableModule;
	}

	public IStoredProcedure FindStoredProcedure(string databaseName, string schemaName, string name, LexicalScope lexicalScope)
	{
		return FindStoredProcedure(this, databaseName, schemaName, name, lexicalScope, allowSynonyms: true);
	}

	private static IStoredProcedure FindStoredProcedure(BatchBindingContext bindingContext, string databaseName, string schemaName, string name, LexicalScope lexicalScope, bool allowSynonyms)
	{
		SchemaEx schema = bindingContext.GetSchema(databaseName, schemaName, lexicalScope);
		IStoredProcedure storedProcedure = null;
		if (schema != null)
		{
			storedProcedure = FindStoredProcedureInSchema(schema, name, lexicalScope);
		}
		if (storedProcedure == null && allowSynonyms)
		{
			storedProcedure = bindingContext.FindAndResolveSynonym(databaseName, schemaName, name, lexicalScope, findStoredProcedureDelegate);
		}
		return storedProcedure;
	}

	public static IStoredProcedure FindStoredProcedureInSchema(SchemaEx schema, string name, LexicalScope lexicalScope)
	{
		return schema.StoredProcedures[name, lexicalScope];
	}

	public ISynonym FindSynonym(string databaseName, string schemaName, string name, LexicalScope lexicalScope)
	{
		SchemaEx schema = GetSchema(databaseName, schemaName, lexicalScope);
		if (schema == null)
		{
			return null;
		}
		return FindSynonymInSchema(schema, name, lexicalScope);
	}

	public static ISynonym FindSynonymInSchema(SchemaEx schema, string name, LexicalScope lexicalScope)
	{
		return schema.Synonyms[name, lexicalScope];
	}

	public IUserDefinedType FindUserDefinedType(string schemaName, string typeName, LexicalScope lexicalScope)
	{
		SchemaEx schema = GetSchema(null, schemaName, lexicalScope);
		if (schema == null)
		{
			return null;
		}
		return FindUserDefinedTypeInSchema(schema, typeName, lexicalScope);
	}

	public static IUserDefinedType FindUserDefinedTypeInSchema(SchemaEx schema, string typeName, LexicalScope lexicalScope)
	{
		return schema.FindUserDefinedTypes(typeName, lexicalScope);
	}

	public IDmlTrigger FindDmlTrigger(string schemaName, string tableName, string triggerName, LexicalScope lexicalScope, out ITableViewBase targetTable)
	{
		SchemaEx schema = GetSchema(null, schemaName, lexicalScope);
		if (schema != null)
		{
			targetTable = FindTableOrViewInSchema(schema, tableName, lexicalScope);
			return (targetTable != null) ? targetTable.Triggers[triggerName] : null;
		}
		targetTable = null;
		return null;
	}

	public IDatabaseDdlTrigger FindDatabaseTrigger(string triggerName, LexicalScope lexicalScope)
	{
		return m_connectedDatabase.Triggers[triggerName, lexicalScope];
	}

	public IServerDdlTrigger FindServerTrigger(string triggerName, LexicalScope lexicalScope)
	{
		return m_server.Triggers[triggerName, lexicalScope];
	}

	public ISchemaOwnedObject FindSchemaOwnedObject(string databaseName, string schemaName, string name, LexicalScope lexicalScope)
	{
		return FindSchemaOwnedObject(this, databaseName, schemaName, name, lexicalScope, allowSynonyms: true);
	}

	private static ISchemaOwnedObject FindSchemaOwnedObject(BatchBindingContext bindingContext, string databaseName, string schemaName, string name, LexicalScope lexicalScope, bool allowSynonyms)
	{
		SchemaEx schema = bindingContext.GetSchema(databaseName, schemaName, lexicalScope);
		ISchemaOwnedObject schemaOwnedObject = null;
		if (schema != null)
		{
			schemaOwnedObject = FindSchemaOwnedObjectInSchema(schema, name, lexicalScope, allowSynonyms);
		}
		if (schemaOwnedObject == null && allowSynonyms)
		{
			schemaOwnedObject = bindingContext.FindAndResolveSynonym(databaseName, schemaName, name, lexicalScope, findSchemaOwnedObjectDelegate);
		}
		return schemaOwnedObject;
	}

	public static ISchemaOwnedObject FindSchemaOwnedObjectInSchema(SchemaEx schema, string objectName, LexicalScope lexicalScope)
	{
		return FindSchemaOwnedObjectInSchema(schema, objectName, lexicalScope, allowSynonyms: true);
	}

	private static ISchemaOwnedObject FindSchemaOwnedObjectInSchema(SchemaEx schema, string objectName, LexicalScope lexicalScope, bool allowSynonyms)
	{
		ISchemaOwnedObject schemaOwnedObject = FindTableOrViewInSchema(schema, objectName, lexicalScope);
		if (schemaOwnedObject != null)
		{
			return schemaOwnedObject;
		}
		schemaOwnedObject = FindTableValuedFunctionInSchema(schema, objectName, lexicalScope);
		if (schemaOwnedObject != null)
		{
			return schemaOwnedObject;
		}
		schemaOwnedObject = FindModuleInSchema(schema, objectName, lexicalScope);
		if (schemaOwnedObject != null)
		{
			return schemaOwnedObject;
		}
		if (allowSynonyms)
		{
			schemaOwnedObject = FindSynonymInSchema(schema, objectName, lexicalScope);
			if (schemaOwnedObject != null)
			{
				return schemaOwnedObject;
			}
		}
		return null;
	}

	public bool ObjectExists(string databaseName, string schemaName, string name, LexicalScope lexicalScope)
	{
		return FindSchemaOwnedObject(databaseName, schemaName, name, lexicalScope) != null;
	}

	public static bool ObjectExistsInSchema(SchemaEx schema, string objectName, LexicalScope lexicalScope)
	{
		return FindSchemaOwnedObjectInSchema(schema, objectName, lexicalScope) != null;
	}

	private T FindAndResolveSynonym<T>(string databaseName, string schemaName, string name, LexicalScope lexicalScope, FindObjectDelegate<T> findObjectDelegate) where T : class, ISchemaOwnedObject
	{
		ISynonym synonym = FindSynonym(databaseName, schemaName, name, lexicalScope);
		if (synonym == null)
		{
			return null;
		}
		return ResolveSynonym(synonym, lexicalScope, findObjectDelegate);
	}

	private T ResolveSynonym<T>(ISynonym synonym, LexicalScope lexicalScope, FindObjectDelegate<T> findObjectDelegate) where T : class, ISchemaOwnedObject
	{
		T val = FindSynonymTarget(synonym.BaseObjectName, lexicalScope, findObjectDelegate);
		if (val != null)
		{
			return ResolvedSynonymConstructor.CreateResolvedSynonym<T>(MetadataFactory, synonym, val);
		}
		return null;
	}

	private T FindSynonymTarget<T>(string baseObjectName, LexicalScope lexicalScope, FindObjectDelegate<T> findObjectDelegate) where T : class, ISchemaOwnedObject
	{
		IList<string> list = ParseMultipartSqlIdentifier(baseObjectName);
		T result = null;
		if (list != null && list.Count > 0 && list.Count <= 3)
		{
			string text = list[list.Count - 1];
			string schemaName = ((list.Count >= 2) ? list[list.Count - 2] : null);
			string databaseName = ((list.Count >= 3) ? list[list.Count - 3] : null);
			if (text != null)
			{
				return findObjectDelegate(this, databaseName, schemaName, text, lexicalScope, allowSynonyms: false);
			}
		}
		return result;
	}

	private static IList<string> ParseMultipartSqlIdentifier(string sqlIdentifier)
	{
		Match match = ObjectNameRegex.Match(sqlIdentifier);
		if (!match.Success)
		{
			return null;
		}
		return (from Capture c in match.Groups["id"].Captures
			select ParseSqlIdentifier(c.Value)).ToArray();
	}

	private static string ParseSqlIdentifier(string id)
	{
		if (id == null || id.Length <= 0)
		{
			return null;
		}
		return EscapeSequence.UnescapeIdentifier(id);
	}

	public IAsymmetricKey FindServerAsymmetricKey(string asymmetricKeyName, LexicalScope lexicalScope)
	{
		return GetDatabase("master").AsymmetricKeys[asymmetricKeyName];
	}

	public IAsymmetricKey FindConnectedDatabaseAsymmetricKey(string asymmetricKeyName, LexicalScope lexicalScope)
	{
		return m_connectedDatabase.AsymmetricKeys[asymmetricKeyName];
	}

	public ICertificate FindServerCertificate(string certificateName, LexicalScope lexicalScope)
	{
		return GetDatabase("master").Certificates[certificateName];
	}

	public ICertificate FindConnectedDatabaseCertificate(string certificateName, LexicalScope lexicalScope)
	{
		return m_connectedDatabase.Certificates[certificateName];
	}

	public ICredential FindCredential(string credentialName, LexicalScope lexicalScope)
	{
		return m_server.Credentials[credentialName];
	}

	public ILogin FindLogin(string loginName, LexicalScope lexicalScope)
	{
		return m_server.Logins[loginName, lexicalScope];
	}

	public IUser FindUser(string userName, LexicalScope lexicalScope)
	{
		return m_connectedDatabase.Users[userName, lexicalScope];
	}

	public IApplicationRole FindApplicationRole(string roleName, LexicalScope lexicalScope)
	{
		return m_connectedDatabase.ApplicationRoles[roleName, lexicalScope];
	}

	public IDatabaseRole FindDatabaseRole(string roleName, LexicalScope lexicalScope)
	{
		return m_connectedDatabase.Roles[roleName, lexicalScope];
	}

	public IDatabasePrincipal FindDatabasePrincipal(string principalName, LexicalScope lexicalScope)
	{
		return m_connectedDatabase.FindDatabasePrincipal(principalName, lexicalScope);
	}

	public SchemaEx FindSchema(string databaseName, string schemaName, LexicalScope lexicalScope)
	{
		return GetSchema(databaseName, schemaName, lexicalScope);
	}

	public DatabaseEx FindDatabase(string databaseName)
	{
		return GetDatabase(databaseName);
	}

	public ISystemDataType FindSystemDataType(DataTypeSpec typeSpec)
	{
		return FindSystemDataType(null, typeSpec, isMaximum: false);
	}

	public ISystemDataType FindSystemDataType(DataTypeSpec typeSpec, bool isMaximum)
	{
		return FindSystemDataType(null, typeSpec, isMaximum);
	}

	public ISystemDataType FindSystemDataType(string schemaName, DataTypeSpec typeSpec, bool isMaximum)
	{
		if (!IsNullOrSysSchema(schemaName))
		{
			return null;
		}
		return m_server.SystemDataTypeLookup.Find(typeSpec, isMaximum);
	}

	public ISystemDataType FindSystemDataType(DataTypeSpec typeSpec, int precisionOrMaxLength)
	{
		return FindSystemDataType(null, typeSpec, precisionOrMaxLength);
	}

	public ISystemDataType FindSystemDataType(string schemaName, DataTypeSpec typeSpec, int precisionOrMaxLength)
	{
		if (!IsNullOrSysSchema(schemaName))
		{
			return null;
		}
		return m_server.SystemDataTypeLookup.Find(typeSpec, precisionOrMaxLength);
	}

	public ISystemDataType FindSystemDataType(DataTypeSpec typeSpec, int precision, int scale)
	{
		return FindSystemDataType(null, typeSpec, precision, scale);
	}

	public ISystemDataType FindSystemDataType(string schemaName, DataTypeSpec typeSpec, int precision, int scale)
	{
		if (!IsNullOrSysSchema(schemaName))
		{
			return null;
		}
		return m_server.SystemDataTypeLookup.Find(typeSpec, precision, scale);
	}

	public IBuiltInFunction FindBuiltIn(string builtInName)
	{
		return m_server.BuiltInFunctionLookup.AllBuiltIns[builtInName];
	}

	public IBuiltInFunction FindBuiltInFunction(string functionName)
	{
		return m_server.BuiltInFunctionLookup.BuiltInFunctions[functionName];
	}

	public IBuiltInFunction FindGlobalVariable(string variableName)
	{
		return m_server.BuiltInFunctionLookup.GlobalVariables[variableName];
	}

	public IDatePart FindDatePart(string datePartName)
	{
		return m_server.BuiltInFunctionLookup.DateParts[datePartName];
	}

	public IBuiltInFunction FindMultiTypeBuiltInFunction(string name, ISystemDataType dataType)
	{
		return m_server.BuiltInFunctionLookup.GetMultiTypeBuiltInFunction(name, dataType);
	}

	public ICollation FindCollation(string collationName)
	{
		return CollationInfo.GetCollationInfo(collationName);
	}

	public SchemaEx GetTempTableSchema()
	{
		return m_server.GetOrCreateTempTableSchema();
	}

	public SchemaEx GetCDASchema(string schemaName, LexicalScope lexicalScope)
	{
		return GetCDASchema(null, schemaName, lexicalScope);
	}

	public SchemaEx GetCDASchema(string databaseName, string schemaName, LexicalScope lexicalScope)
	{
		SchemaEx schema = GetSchema(databaseName, schemaName, lexicalScope);
		if (schema == null)
		{
			return GetDatabase(databaseName)?.EmptySchema;
		}
		return schema.AsSingleSchema;
	}

	public SchemaEx SchemaToSchemaEx(ISchema schema)
	{
		SchemaEx schemaEx = schema as SchemaEx;
		if (schemaEx == null)
		{
			schemaEx = DatabaseToDatabaseEx(schema.Database).GetOrCreateSchemaEx(schema);
		}
		return schemaEx;
	}

	public DatabaseEx DatabaseToDatabaseEx(IDatabase database)
	{
		return (database as DatabaseEx) ?? m_server.GetOrCreateDatabaseEx(database);
	}

	public IScalarDataType EvalScalarExpressionDataType(IScalarDataType type1, IScalarDataType type2)
	{
		ISystemDataType baseSystemDataType = type1.BaseSystemDataType;
		ISystemDataType baseSystemDataType2 = type2.BaseSystemDataType;
		IScalarDataType scalarDataType = ((baseSystemDataType != null && baseSystemDataType2 != null) ? EvalExpressionDataType(baseSystemDataType, baseSystemDataType2) : (type1.IsVoid ? type2 : ((!type2.IsVoid) ? null : type1)));
		return scalarDataType ?? MetadataFactory.DataType.UnknownScalar;
	}

	private ISystemDataType EvalExpressionDataType(ISystemDataType sysType1, ISystemDataType sysType2)
	{
		if (sysType1 == sysType2)
		{
			return sysType1;
		}
		DataTypeSpec typeSpec = sysType1.TypeSpec;
		DataTypeSpec typeSpec2 = sysType2.TypeSpec;
		DataTypeSpec dataTypeSpec = null;
		switch (typeSpec.SqlDataType)
		{
		case SqlDataType.Bit:
			switch (typeSpec2.SqlDataType)
			{
			case SqlDataType.BigInt:
			case SqlDataType.Int:
			case SqlDataType.SmallInt:
			case SqlDataType.TinyInt:
				dataTypeSpec = typeSpec2;
				break;
			}
			break;
		case SqlDataType.TinyInt:
			switch (typeSpec2.SqlDataType)
			{
			case SqlDataType.Bit:
				dataTypeSpec = typeSpec;
				break;
			case SqlDataType.BigInt:
			case SqlDataType.Int:
			case SqlDataType.SmallInt:
				dataTypeSpec = typeSpec2;
				break;
			}
			break;
		case SqlDataType.SmallInt:
			switch (typeSpec2.SqlDataType)
			{
			case SqlDataType.Bit:
			case SqlDataType.TinyInt:
				dataTypeSpec = typeSpec;
				break;
			case SqlDataType.BigInt:
			case SqlDataType.Int:
				dataTypeSpec = typeSpec2;
				break;
			}
			break;
		case SqlDataType.Int:
			switch (typeSpec2.SqlDataType)
			{
			case SqlDataType.Bit:
			case SqlDataType.SmallInt:
			case SqlDataType.TinyInt:
				dataTypeSpec = typeSpec;
				break;
			case SqlDataType.BigInt:
				dataTypeSpec = typeSpec2;
				break;
			}
			break;
		case SqlDataType.BigInt:
			switch (typeSpec2.SqlDataType)
			{
			case SqlDataType.Bit:
			case SqlDataType.Int:
			case SqlDataType.SmallInt:
			case SqlDataType.TinyInt:
				dataTypeSpec = typeSpec;
				break;
			}
			break;
		}
		if (dataTypeSpec != null)
		{
			return FindSystemDataType(dataTypeSpec);
		}
		return null;
	}

	public void SetBoundObject(object key, IMetadataObject boundObject)
	{
		m_boundObjectMap.Add(key, boundObject);
	}

	public IMetadataObject GetBoundObject(object key)
	{
		m_boundObjectMap.TryGetValue(key, out var value);
		return value;
	}

	public T GetBoundObject<T>(object key) where T : class, IMetadataObject
	{
		return GetBoundObject(key) as T;
	}

	public LocalBindingContext CreateLocalBindingContext()
	{
		return new LocalBindingContext(this);
	}

	public ITableVariable FindLocalTableVariable(string name, LexicalScope lexicalScope)
	{
		return m_localSymbolTable.FindTableVariable(name, lexicalScope);
	}

	public IScalarVariable FindLocalScalarVariable(string name, LexicalScope lexicalScope)
	{
		return m_localSymbolTable.FindScalarVariable(name, lexicalScope);
	}

	public ICursorVariable FindLocalCursorVariable(string name, LexicalScope lexicalScope)
	{
		return m_localSymbolTable.FindCursorVariable(name, lexicalScope);
	}

	public ILocalVariable FindLocalVariable(string name, LexicalScope lexicalScope)
	{
		return m_localSymbolTable.FindVariable(name, lexicalScope);
	}

	public virtual void OnBindingToSelfCte()
	{
	}

	public virtual ITabular FindCommonTableExpression(string name, out CteBindMode bindMode, out CteQueryFlags queryFlags)
	{
		bindMode = CteBindMode.None;
		queryFlags = CteQueryFlags.None;
		return null;
	}

	public virtual ITabular FindDmlTable(string name)
	{
		return m_dmlTables[name];
	}

	public IColumn AddLocalColumnExpression(IColumn expression)
	{
		return expression;
	}

	public ITabular AddLocalTableExpression(ITabular expression)
	{
		return expression;
	}

	public IScalar FindLocalScalarExpression(string name)
	{
		return null;
	}

	public ITabular FindLocalTableExpression(string databaseName, string schemaName, string name)
	{
		return null;
	}

	public IColumn FindColumn(string name, out bool isAmbiguous)
	{
		isAmbiguous = false;
		return null;
	}

	public IEnumerable<IColumn> FindAllColumns(string name)
	{
		yield break;
	}

	public virtual IMetadataCollection<IMetadataObject> FindAll(CompletionContext context, LexicalScope lexicalScope)
	{
		return FindAll(context, lexicalScope, null);
	}

	public virtual IMetadataCollection<IMetadataObject> FindAll(CompletionContext context, LexicalScope lexicalScope, IMetadataCollection<IMetadataObject> commonTableExpressions)
	{
		List<IMetadataCollection<IMetadataObject>> list = new List<IMetadataCollection<IMetadataObject>>(8);
		if ((context & CompletionContext.Databases) == CompletionContext.Databases)
		{
			list.Add(m_server.Databases.GetMetadataCollection(lexicalScope));
		}
		if ((context & CompletionContext.Schemas) == CompletionContext.Schemas)
		{
			list.Add(m_connectedDatabase.Schemas.GetMetadataCollection(lexicalScope));
		}
		if ((context & CompletionContext.Tables) == CompletionContext.Tables)
		{
			list.Add(m_unnamedSchema.Tables.GetMetadataCollection(lexicalScope));
			SchemaEx tempTableSchema = m_server.TempTableSchema;
			if (tempTableSchema != null)
			{
				list.Add(tempTableSchema.Tables.GetMetadataCollection(lexicalScope));
			}
		}
		if ((context & CompletionContext.Views) == CompletionContext.Views)
		{
			list.Add(m_unnamedSchema.Views.GetMetadataCollection(lexicalScope));
		}
		if ((context & CompletionContext.AllBuiltins) != 0)
		{
			if ((context & CompletionContext.AllBuiltins) == CompletionContext.AllBuiltins)
			{
				list.Add(m_server.BuiltInFunctionLookup.AllBuiltIns.AsMetadataObjectCollection);
			}
			else if ((context & CompletionContext.BuiltinFunctions) == CompletionContext.BuiltinFunctions)
			{
				list.Add(m_server.BuiltInFunctionLookup.BuiltInFunctions.AsMetadataObjectCollection);
			}
			else if ((context & CompletionContext.GlobalVariables) == CompletionContext.GlobalVariables)
			{
				list.Add(m_server.BuiltInFunctionLookup.GlobalVariables.AsMetadataObjectCollection);
			}
		}
		if ((context & CompletionContext.TableValuedFunctions) == CompletionContext.TableValuedFunctions)
		{
			list.Add(m_unnamedSchema.TableValuedFunctions.GetMetadataCollection(lexicalScope));
			list.Add(m_builtInTableValuedFunctions.AsMetadataObjectCollection);
		}
		if ((context & CompletionContext.StoredProcedures) == CompletionContext.StoredProcedures)
		{
			list.Add(m_unnamedSchema.StoredProcedures.GetMetadataCollection(lexicalScope));
		}
		if ((context & CompletionContext.SynyonymBaseObjects) != 0)
		{
			list.Add(FindResolvedSynonyms(m_unnamedSchema, context, lexicalScope));
		}
		if ((context & CompletionContext.ScalarVariables) == CompletionContext.ScalarVariables)
		{
			IMetadataCollection<IScalarVariable> scalarVariablesAt = m_localSymbolTable.GetScalarVariablesAt(lexicalScope);
			list.Add(scalarVariablesAt.AsMetadataObjectCollection);
		}
		if ((context & CompletionContext.TableVariables) == CompletionContext.TableVariables)
		{
			IMetadataCollection<ITableVariable> tableVariablesAt = m_localSymbolTable.GetTableVariablesAt(lexicalScope);
			list.Add(tableVariablesAt.AsMetadataObjectCollection);
		}
		if ((context & CompletionContext.TablesOrViews) != 0)
		{
			list.Add(commonTableExpressions);
		}
		if ((context & CompletionContext.TablesOrViews) != 0)
		{
			list.Add(m_dmlTables.AsMetadataObjectCollection);
		}
		if ((context & CompletionContext.Datepart) == CompletionContext.Datepart)
		{
			list.Add(m_server.BuiltInFunctionLookup.DateParts.AsMetadataObjectCollection);
		}
		if ((context & CompletionContext.Credentials) == CompletionContext.Credentials)
		{
			list.Add(m_server.Credentials.AsMetadataObjectCollection);
		}
		if ((context & CompletionContext.ServerAsymmetricKeys) == CompletionContext.ServerAsymmetricKeys)
		{
			DatabaseEx database = GetDatabase("master");
			list.Add(database.AsymmetricKeys.AsMetadataObjectCollection);
		}
		if ((context & CompletionContext.ConnectedDatabaseAsymmetricKeys) == CompletionContext.ConnectedDatabaseAsymmetricKeys)
		{
			DatabaseEx connectedDatabase = m_connectedDatabase;
			list.Add(connectedDatabase.AsymmetricKeys.AsMetadataObjectCollection);
		}
		if ((context & CompletionContext.ServerCertificates) == CompletionContext.ServerCertificates)
		{
			DatabaseEx database2 = GetDatabase("master");
			list.Add(database2.Certificates.AsMetadataObjectCollection);
		}
		if ((context & CompletionContext.ConnectedDatabaseCertificates) == CompletionContext.ConnectedDatabaseCertificates)
		{
			DatabaseEx connectedDatabase2 = m_connectedDatabase;
			list.Add(connectedDatabase2.Certificates.AsMetadataObjectCollection);
		}
		if ((context & CompletionContext.Logins) == CompletionContext.Logins)
		{
			list.Add(m_server.Logins.GetMetadataCollection(lexicalScope));
		}
		if ((context & CompletionContext.Users) == CompletionContext.Users)
		{
			DatabaseEx connectedDatabase3 = m_connectedDatabase;
			list.Add(connectedDatabase3.Users.GetMetadataCollection(lexicalScope));
		}
		if ((context & CompletionContext.SystemDataTypes) == CompletionContext.SystemDataTypes)
		{
			list.Add(m_server.SystemDataTypes);
		}
		IMetadataCollection<IMetadataObject>[] array = new IMetadataCollection<IMetadataObject>[list.Count];
		list.CopyTo(array);
		return Collection<IMetadataObject>.Merge(array);
	}

	public IMetadataCollection<IMetadataObject> FindAll(List<string> ids, CompletionContext context, LexicalScope lexicalScope)
	{
		return FindAll(ids, context, lexicalScope, null);
	}

	public virtual IMetadataCollection<IMetadataObject> FindAll(List<string> ids, CompletionContext context, LexicalScope lexicalScope, IMetadataCollection<IMetadataObject> commonTableExpressions)
	{
		if (ids == null || ids.Count == 0)
		{
			return FindAll(context, lexicalScope, commonTableExpressions);
		}
		IMetadataCollection<IMetadataObject> metadataCollection = null;
		IMetadataCollection<IMetadataObject> metadataCollection2 = null;
		IMetadataCollection<IMetadataObject> metadataCollection3 = null;
		IMetadataCollection<IMetadataObject> metadataCollection4 = null;
		IMetadataCollection<IMetadataObject> metadataCollection5 = null;
		IMetadataCollection<IMetadataObject> metadataCollection6 = null;
		IMetadataCollection<IMetadataObject> metadataCollection7 = null;
		IMetadataCollection<IMetadataObject> metadataCollection8 = null;
		IMetadataCollection<IMetadataObject> metadataCollection9 = null;
		DatabaseEx databaseEx = null;
		SchemaEx schemaEx = null;
		ITabular tabular = null;
		switch (ids.Count)
		{
		case 1:
			if ((context & CompletionContext.Databases) != 0)
			{
				databaseEx = GetDatabase(ids[0]);
			}
			if ((context & CompletionContext.Schemas) != 0)
			{
				schemaEx = GetSchema(null, ids[0], lexicalScope);
			}
			tabular = FindTableOrView(null, null, ids[0], lexicalScope);
			break;
		case 2:
			databaseEx = null;
			if ((context & CompletionContext.Schemas) != 0)
			{
				schemaEx = GetSchema(ids[0], ids[1], lexicalScope);
			}
			tabular = FindTableOrView(null, ids[0], ids[1], lexicalScope);
			break;
		case 3:
			databaseEx = null;
			schemaEx = null;
			tabular = FindTableOrView(ids[0], ids[1], ids[2], lexicalScope);
			break;
		}
		if (databaseEx != null)
		{
			metadataCollection = databaseEx.Schemas.GetMetadataCollection(lexicalScope);
		}
		if (schemaEx != null)
		{
			if ((context & CompletionContext.Tables) == CompletionContext.Tables)
			{
				metadataCollection2 = schemaEx.Tables.GetMetadataCollection(lexicalScope);
			}
			if ((context & CompletionContext.Views) == CompletionContext.Views)
			{
				metadataCollection3 = schemaEx.Views.GetMetadataCollection(lexicalScope);
			}
			if ((context & CompletionContext.TableValuedFunctions) == CompletionContext.TableValuedFunctions)
			{
				metadataCollection4 = schemaEx.TableValuedFunctions.GetMetadataCollection(lexicalScope);
			}
			if ((context & CompletionContext.ScalarValuedFunctions) == CompletionContext.ScalarValuedFunctions)
			{
				metadataCollection5 = schemaEx.ScalarValuedFunctions.GetMetadataCollection(lexicalScope);
				metadataCollection6 = schemaEx.UserDefinedAggregates.GetMetadataCollection(lexicalScope);
			}
			if ((context & CompletionContext.StoredProcedures) == CompletionContext.StoredProcedures)
			{
				metadataCollection8 = schemaEx.StoredProcedures.GetMetadataCollection(lexicalScope);
			}
			if ((context & CompletionContext.SynyonymBaseObjects) != 0)
			{
				metadataCollection7 = FindResolvedSynonyms(schemaEx, context, lexicalScope);
			}
		}
		if (tabular != null && (context & CompletionContext.Columns) == CompletionContext.Columns)
		{
			metadataCollection9 = tabular.Columns.AsMetadataObjectCollection;
		}
		return Collection<IMetadataObject>.Merge(metadataCollection, metadataCollection2, metadataCollection3, metadataCollection4, metadataCollection8, metadataCollection5, metadataCollection6, metadataCollection7, metadataCollection9);
	}

	private IMetadataCollection<IMetadataObject> FindResolvedSynonyms(SchemaEx matchedSchema, CompletionContext context, LexicalScope lexicalScope)
	{
		return Collection<IMetadataObject>.Filter(matchedSchema.Synonyms.GetMetadataCollection(lexicalScope), delegate(IMetadataObject synonymObj)
		{
			ISynonym synonym = (ISynonym)synonymObj;
			return IsMatchingSynonym(matchedSchema, synonym, context, lexicalScope);
		});
	}

	private bool IsMatchingSynonym(SchemaEx matchedSchema, ISynonym synonym, CompletionContext context, LexicalScope lexicalScope)
	{
		ISchemaOwnedObject schemaOwnedObject = FindSynonymTarget(synonym.BaseObjectName, lexicalScope, findSchemaOwnedObjectDelegate);
		bool flag = false;
		if ((context & CompletionContext.Tables) == CompletionContext.Tables)
		{
			flag = flag || schemaOwnedObject is ITable;
		}
		if ((context & CompletionContext.Views) == CompletionContext.Views)
		{
			flag = flag || schemaOwnedObject is IView;
		}
		if ((context & CompletionContext.TableValuedFunctions) == CompletionContext.TableValuedFunctions)
		{
			flag = flag || schemaOwnedObject is ITableValuedFunction;
		}
		if ((context & CompletionContext.ScalarValuedFunctions) == CompletionContext.ScalarValuedFunctions && !matchedSchema.IsUnnamedSchema)
		{
			flag = flag || schemaOwnedObject is IScalarValuedFunction || schemaOwnedObject is IUserDefinedAggregate;
		}
		if ((context & CompletionContext.StoredProcedures) == CompletionContext.StoredProcedures)
		{
			flag = flag || schemaOwnedObject is IStoredProcedure || schemaOwnedObject is IExtendedStoredProcedure;
		}
		return flag;
	}

	public void EnterTempBindMode(TemporaryBindMode mode)
	{
	}

	public void LeaveTempBindMode(TemporaryBindMode mode)
	{
	}

	private SchemaEx GetSchema(string databaseName, string schemaName, LexicalScope lexicalScope)
	{
		if (string.IsNullOrEmpty(databaseName))
		{
			if (string.IsNullOrEmpty(schemaName))
			{
				return m_unnamedSchema;
			}
			return m_connectedDatabase.FindSchema(schemaName, lexicalScope);
		}
		DatabaseEx databaseEx = m_server[databaseName];
		if (databaseEx != null)
		{
			if (string.IsNullOrEmpty(schemaName))
			{
				return databaseEx.UnnamedSchema;
			}
			return databaseEx.FindSchema(schemaName, lexicalScope);
		}
		return null;
	}

	private DatabaseEx GetDatabase(string databaseName)
	{
		if (!string.IsNullOrEmpty(databaseName))
		{
			return m_server[databaseName];
		}
		return m_connectedDatabase;
	}

	private ITabular FindCommonTableExpression(string name)
	{
		CteBindMode bindMode;
		CteQueryFlags queryFlags;
		return FindCommonTableExpression(name, out bindMode, out queryFlags);
	}

	private bool IsNullOrSysSchema(string schemaName)
	{
		if (schemaName != null)
		{
			return DatabaseCollation.EqualityComparer.Equals(schemaName, "sys");
		}
		return true;
	}
}
