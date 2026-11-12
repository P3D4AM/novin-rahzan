using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo.Internal;

namespace Microsoft.SqlServer.Management.Smo;

public abstract class TransferBase
{
	private Database database;

	private ArrayList objectList;

	private readonly List<Urn> incompatibleObjects = new List<Urn>();

	private string destinationLogin = string.Empty;

	private SqlSecureString destinationPassword = string.Empty;

	private string destinationServer = string.Empty;

	private DatabaseFileMappingsDictionary databaseFileMappings = new DatabaseFileMappingsDictionary();

	private Scripter scripter;

	public Database Database
	{
		get
		{
			return database;
		}
		set
		{
			SetDatabase(value);
		}
	}

	public ArrayList ObjectList
	{
		get
		{
			if (objectList == null)
			{
				objectList = new ArrayList();
			}
			return objectList;
		}
		set
		{
			if (value == null)
			{
				throw new SmoException(ExceptionTemplatesImpl.InnerException, new ArgumentNullException("ObjectList"));
			}
			objectList = value;
		}
	}

	public IList<Urn> IncompatibleObjects => incompatibleObjects;

	public bool CopyAllObjects { get; set; }

	public bool CopyAllFullTextCatalogs { get; set; }

	public bool CopyAllFullTextStopLists { get; set; }

	public bool CopyAllSearchPropertyLists { get; set; }

	public bool CopyAllTables { get; set; }

	public bool CopyAllViews { get; set; }

	public bool CopyAllStoredProcedures { get; set; }

	public bool CopyAllUserDefinedFunctions { get; set; }

	public bool CopyAllUserDefinedDataTypes { get; set; }

	public bool CopyAllUserDefinedTableTypes { get; set; }

	public bool CopyAllSecurityPolicies { get; set; }

	public bool CopyAllPlanGuides { get; set; }

	public bool CopyAllRules { get; set; }

	public bool CopyAllDefaults { get; set; }

	public bool CopyAllUsers { get; set; }

	public bool CopyAllRoles { get; set; }

	public bool CopyAllPartitionSchemes { get; set; }

	public bool CopyAllPartitionFunctions { get; set; }

	public bool CopyAllXmlSchemaCollections { get; set; }

	public bool CopyAllSqlAssemblies { get; set; }

	public bool CopyAllUserDefinedAggregates { get; set; }

	public bool CopyAllUserDefinedTypes { get; set; }

	public bool CopyAllSchemas { get; set; }

	public bool CopyAllSynonyms { get; set; }

	public bool CopyAllSequences { get; set; }

	public bool CopyAllDatabaseTriggers { get; set; }

	public bool CopyAllDatabaseScopedCredentials { get; set; }

	public bool CopyAllExternalFileFormats { get; set; }

	public bool CopyAllExternalDataSources { get; set; }

	public bool CopyAllLogins { get; set; }

	public bool CopyAllExternalLanguages { get; set; }

	public bool CopyAllExternalLibraries { get; set; }

	public bool CopySchema { get; set; }

	public bool CopyData { get; set; }

	public bool DropDestinationObjectsFirst { get; set; }

	public bool CreateTargetDatabase { get; set; }

	public bool DestinationTranslateChar { get; set; }

	public bool SourceTranslateChar { get; set; }

	public bool UseDestinationTransaction { get; set; }

	public bool PreserveLogins { get; set; }

	public bool PrefetchObjects { get; set; }

	public bool CopyAllColumnMasterkey { get; set; }

	public bool CopyAllColumnEncryptionKey { get; set; }

	public bool CopyExternalTables { get; set; }

	public bool PreserveDbo { get; set; }

	public bool DestinationLoginSecure { get; set; }

	public string DestinationDatabase { get; set; }

	public string DestinationLogin
	{
		get
		{
			return destinationLogin;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("DestinationLogin");
			}
			destinationLogin = value;
		}
	}

	public string DestinationPassword
	{
		get
		{
			return (string)destinationPassword;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("DestinationPassword");
			}
			destinationPassword = new SqlSecureString(value);
		}
	}

	public string DestinationServer
	{
		get
		{
			return destinationServer;
		}
		set
		{
			destinationServer = value ?? throw new ArgumentNullException("DestinationServer");
		}
	}

	public ServerConnection DestinationServerConnection { get; set; }

	public string TargetDatabaseFilePath { get; set; }

	public string TargetLogFilePath { get; set; }

	public DatabaseFileMappingsDictionary DatabaseFileMappings
	{
		get
		{
			return databaseFileMappings;
		}
		set
		{
			if (value == null)
			{
				throw new WrongPropertyValueException(ExceptionTemplatesImpl.WrongPropertyValueException("DatabaseFileMappings", "NULL"));
			}
			databaseFileMappings = value;
		}
	}

	protected Scripter Scripter
	{
		get
		{
			if (scripter == null)
			{
				if (Database == null)
				{
					throw new PropertyNotSetException("Database");
				}
				scripter = new Scripter(Database.Parent);
				scripter.PrefetchObjects = false;
				scripter.Options.WithDependencies = true;
			}
			return scripter;
		}
	}

	public ScriptingOptions Options
	{
		get
		{
			return Scripter.GetOptions();
		}
		set
		{
			Scripter.Options = value;
		}
	}

	public event ProgressReportEventHandler DiscoveryProgress
	{
		add
		{
			Scripter.DiscoveryProgress += value;
		}
		remove
		{
			Scripter.DiscoveryProgress -= value;
		}
	}

	public event ProgressReportEventHandler ScriptingProgress
	{
		add
		{
			Scripter.ScriptingProgress += value;
		}
		remove
		{
			Scripter.ScriptingProgress -= value;
		}
	}

	public event ScriptingErrorEventHandler ScriptingError
	{
		add
		{
			Scripter.ScriptingError += value;
		}
		remove
		{
			Scripter.ScriptingError -= value;
		}
	}

	public TransferBase()
	{
		Init();
	}

	public TransferBase(Database database)
		: this()
	{
		SetDatabase(database);
	}

	private void SetDatabase(Database database)
	{
		if (database == null)
		{
			throw new FailedOperationException(ExceptionTemplatesImpl.SetDatabase, this, new ArgumentNullException("database"));
		}
		if (database.State == SqlSmoState.Pending || database.State == SqlSmoState.Creating)
		{
			throw new InvalidSmoOperationException(ExceptionTemplatesImpl.TransferCtorErr, new ArgumentException("database"));
		}
		this.database = database;
	}

	private void Init()
	{
		CopyAllObjects = true;
		CopySchema = true;
		CopyData = true;
		DestinationDatabase = string.Empty;
		DestinationLoginSecure = true;
		DestinationTranslateChar = true;
		SourceTranslateChar = true;
		TargetDatabaseFilePath = string.Empty;
		TargetLogFilePath = string.Empty;
		PrefetchObjects = true;
	}

	protected void SetTargetServerInfo()
	{
		ServerConnection destinationServerConnection = GetDestinationServerConnection();
		try
		{
			destinationServerConnection.Connect();
			Scripter.Options.SetTargetServerVersion(destinationServerConnection.ServerVersion);
		}
		finally
		{
			destinationServerConnection.Disconnect();
		}
	}

	protected ServerConnection GetDestinationServerConnection()
	{
		if (DestinationServerConnection != null)
		{
			return DestinationServerConnection;
		}
		ServerConnection serverConnection = new ServerConnection(DestinationServer)
		{
			LoginSecure = DestinationLoginSecure,
			NonPooledConnection = true
		};
		if (!serverConnection.LoginSecure)
		{
			serverConnection.Login = DestinationLogin;
			serverConnection.Password = DestinationPassword;
		}
		return serverConnection;
	}

	public StringCollection ScriptTransfer()
	{
		if (Database == null)
		{
			throw new PropertyNotSetException("Database");
		}
		if (Options.ScriptData)
		{
			throw new FailedOperationException(ExceptionTemplatesImpl.ScriptDataNotSupportedByThisMethod);
		}
		try
		{
			return EnumerableContainer.IEnumerableToStringCollection(EnumScriptTransfer());
		}
		catch (Exception ex)
		{
			SqlSmoObject.FilterException(ex);
			throw new FailedOperationException(ExceptionTemplatesImpl.ScriptTransfer, this, ex);
		}
	}

	public IEnumerable<string> EnumScriptTransfer()
	{
		if (Database == null)
		{
			throw new PropertyNotSetException("Database");
		}
		bool withDependencies = Scripter.Options.WithDependencies;
		bool includeDatabaseContext = Options.IncludeDatabaseContext;
		try
		{
			EnumerableContainer enumerableContainer = new EnumerableContainer();
			Scripter.PrefetchObjects = false;
			Options.IncludeDatabaseContext = false;
			if (!string.IsNullOrEmpty(DestinationServer))
			{
				SetTargetServerInfo();
			}
			if (includeDatabaseContext)
			{
				enumerableContainer.Add(new List<string> { string.Format(SmoApplication.DefaultCulture, Scripts.USEDB, SqlSmoObject.SqlBraket(string.IsNullOrEmpty(DestinationDatabase) ? Database.Name : DestinationDatabase)) });
			}
			Scripter.ScriptingError += ScripterOnScriptingError;
			enumerableContainer.Add(Scripter.EnumScriptWithList(GetObjectList(ordered: false)));
			Scripter.ScriptingError -= ScripterOnScriptingError;
			if (Options.ContinueScriptingOnError)
			{
				enumerableContainer.Add(IncompatibleObjects.Select((Urn n) => $"-- {DatabaseRestorePlannerSR.IncompatibleObject(n.Type, n.GetNameForType(n.Type))}"));
			}
			return enumerableContainer;
		}
		catch (PropertyCannotBeRetrievedException innerException)
		{
			throw new FailedOperationException(ExceptionTemplatesImpl.FailedOperationExceptionTextScript(SqlSmoObject.GetTypeName(Database.GetType().Name), Database.ToString()), innerException)
			{
				Operation = ExceptionTemplatesImpl.ScriptTransfer,
				FailedObject = this
			};
		}
		catch (Exception ex)
		{
			SqlSmoObject.FilterException(ex);
			throw new FailedOperationException(ExceptionTemplatesImpl.ScriptTransfer, this, ex);
		}
		finally
		{
			Options.IncludeDatabaseContext = includeDatabaseContext;
			Scripter.Options.WithDependencies = withDependencies;
		}
	}

	private void ScripterOnScriptingError(object sender, ScriptingErrorEventArgs e)
	{
		incompatibleObjects.Add(e.Current);
	}

	public UrnCollection EnumObjects()
	{
		return EnumObjects(ordered: true);
	}

	public UrnCollection EnumObjects(bool ordered)
	{
		DependencyCollection dependencyCollection = GetObjectList(ordered);
		UrnCollection urnCollection = new UrnCollection();
		foreach (DependencyCollectionNode item in dependencyCollection)
		{
			urnCollection.Add(item.Urn);
		}
		return urnCollection;
	}

	internal DependencyCollection GetObjectList(bool ordered)
	{
		DependencyCollection dependencyCollection = null;
		HashSet<Urn> hashSet = new HashSet<Urn>();
		HashSet<Urn> nonDepDiscList = new HashSet<Urn>();
		Dictionary<string, HashSet<Urn>> dictionary = new Dictionary<string, HashSet<Urn>>();
		dictionary.Add(User.UrnSuffix, new HashSet<Urn>());
		dictionary.Add(DatabaseRole.UrnSuffix, new HashSet<Urn>());
		dictionary.Add(ApplicationRole.UrnSuffix, new HashSet<Urn>());
		dictionary.Add(Login.UrnSuffix, new HashSet<Urn>());
		dictionary.Add(Endpoint.UrnSuffix, new HashSet<Urn>());
		HashSet<Urn> hashSet2 = new HashSet<Urn>();
		ScriptingPreferences scriptingPreferences = Options.GetScriptingPreferences();
		SeparateDiscoverySupportedObjects(hashSet, nonDepDiscList, dictionary, hashSet2);
		Dictionary<Type, StringCollection> originalDefaultFields = new Dictionary<Type, StringCollection>();
		try
		{
			if (scriptingPreferences.IncludeScripts.ExtendedProperties && IsSupportedObject<ExtendedProperty>(scriptingPreferences) && CopyAllObjects)
			{
				AddAllObjects<ExtendedProperty>(hashSet2, Database.ExtendedProperties);
			}
			AddDiscoverableObjects(hashSet, originalDefaultFields);
			dependencyCollection = new DependencyCollection();
			bool flag = Options.WithDependencies && !CopyAllObjects;
			if (0 < hashSet.Count && flag)
			{
				hashSet = DiscoverDependencies(hashSet);
			}
			PrefetchSecurityObjects(originalDefaultFields);
			AddDiscoveryUnsupportedObjects(hashSet, nonDepDiscList);
			if (ordered)
			{
				dependencyCollection = GetDependencyOrderedCollection(hashSet);
				AddSecurityObjectsInOrder(dependencyCollection, dictionary);
			}
			else
			{
				foreach (Urn item in hashSet)
				{
					dependencyCollection.Add(new DependencyCollectionNode(item, isSchemaBound: true, fRoot: true));
				}
				AddSecurityObjectsWithoutOrder(dependencyCollection, dictionary);
			}
			CheckDownLevelScripting(dependencyCollection, scriptingPreferences);
			foreach (Urn item2 in hashSet2)
			{
				dependencyCollection.Insert(0, new DependencyCollectionNode(item2, isSchemaBound: true, fRoot: true));
			}
			return dependencyCollection;
		}
		finally
		{
			RestoreDefaultInitFields(originalDefaultFields);
		}
	}

	private HashSet<Urn> DiscoverDependencies(HashSet<Urn> depDiscInputList)
	{
		HashSet<Urn> hashSet = new HashSet<Urn>();
		TraceHelper.Assert(Database != null, "null == this.Database");
		bool flag = Options.ScriptSchema && Options.ScriptDrops;
		Urn[] array = new Urn[depDiscInputList.Count];
		depDiscInputList.CopyTo(array);
		DependencyTree dependencyTree = Scripter.DiscoverDependencies(array, !flag);
		DependencyChainCollection dependencies = dependencyTree.Dependencies;
		TraceHelper.Assert(dependencies != null, "GetDependencies() returned null");
		HashSet<Urn> hashSet2 = new HashSet<Urn>();
		if (dependencyTree.FirstChild != null)
		{
			TreeTraversal(dependencyTree.FirstChild, hashSet2);
		}
		Urn urn = "Server/Database[@Name='" + Urn.EscapeString(Database.Name) + "']";
		foreach (Urn item in hashSet2)
		{
			if (!item.Type.Equals("Default", StringComparison.OrdinalIgnoreCase) || !item.Parent.Type.Equals("Column", StringComparison.OrdinalIgnoreCase))
			{
				Urn parent = item.Parent;
				while (parent.XPathExpression.Length > 2)
				{
					parent = parent.Parent;
				}
				if (Database.Parent.CompareUrn(urn, parent) == 0 && item.Type != "UnresolvedEntity")
				{
					hashSet.Add(item);
				}
			}
		}
		return hashSet;
	}

	private void AddWithoutDependencyDiscovery(DependencyCollection depList, DependencyCollectionNode node)
	{
		depList.Insert(0, node);
	}

	private void AddWithoutDependencyDiscoveryCollection(DependencyCollection depList, HashSet<Urn> nonDepHashSet)
	{
		foreach (Urn item in nonDepHashSet)
		{
			AddWithoutDependencyDiscovery(depList, new DependencyCollectionNode(item, isSchemaBound: true, fRoot: true));
		}
	}

	private void AddDiscoveryUnsupportedObjects(HashSet<Urn> list, HashSet<Urn> nonDepDiscList)
	{
		ScriptingPreferences scriptingPreferences = Options.GetScriptingPreferences();
		if (IsSupportedObject<FullTextCatalog>(scriptingPreferences) && (CopyAllObjects || CopyAllFullTextCatalogs || Options.FullTextCatalogs))
		{
			foreach (FullTextCatalog fullTextCatalog in Database.FullTextCatalogs)
			{
				list.Add(fullTextCatalog.Urn);
			}
		}
		if (IsSupportedObject<FullTextStopList>(scriptingPreferences) && (CopyAllObjects || CopyAllFullTextStopLists || Options.FullTextStopLists))
		{
			foreach (FullTextStopList fullTextStopList in Database.FullTextStopLists)
			{
				list.Add(fullTextStopList.Urn);
			}
		}
		if (IsSupportedObject<SearchPropertyList>(scriptingPreferences) && (CopyAllObjects || CopyAllSearchPropertyLists))
		{
			foreach (SearchPropertyList searchPropertyList in Database.SearchPropertyLists)
			{
				list.Add(searchPropertyList.Urn);
			}
		}
		if (IsSupportedObject<Schema>(scriptingPreferences) && (CopyAllObjects || CopyAllSchemas))
		{
			foreach (Schema schema in Database.Schemas)
			{
				if (schema.ID > 4 && (schema.ID < 16384 || schema.ID >= 16400))
				{
					list.Add(schema.Urn);
				}
			}
		}
		list.UnionWith(nonDepDiscList);
	}

	private void PrefetchSecurityObjects(Dictionary<Type, StringCollection> originalDefaultFields)
	{
		ScriptingPreferences scriptingPreferences = Options.GetScriptingPreferences();
		if (PrefetchObjects && CopySchema)
		{
			if (CopyAllObjects || CopyAllUsers)
			{
				Database.PrefetchUsers(scriptingPreferences);
			}
			if (CopyAllObjects || CopyAllRoles)
			{
				Database.PrefetchDatabaseRoles(scriptingPreferences);
			}
			if (CopyAllObjects || CopyAllSchemas)
			{
				Database.PrefetchSchemas(scriptingPreferences);
			}
		}
		else
		{
			SetDefaultInitFields(originalDefaultFields, typeof(User), "IsSystemObject");
			SetDefaultInitFields(originalDefaultFields, typeof(Schema), "ID");
			SetDefaultInitFields(originalDefaultFields, typeof(DatabaseRole), "Owner", "IsFixedRole");
		}
	}

	private void AddSecurityObjectsInOrder(DependencyCollection depList, Dictionary<string, HashSet<Urn>> nonDepListDictionary)
	{
		if (CopyAllObjects || CopyAllRoles)
		{
			DependencyObjects dependencyObjects = new DependencyObjects();
			foreach (DatabaseRole role in Database.Roles)
			{
				dependencyObjects.Add(role);
				string owner = role.Owner;
				if (Database.Roles.Contains(owner))
				{
					if (!Database.Roles[owner].IsFixedRole)
					{
						dependencyObjects.Add(Database.Roles[owner], role);
					}
				}
				else if (Database.Users.Contains(owner) && !Database.Users[owner].IsSystemObject)
				{
					dependencyObjects.Add(Database.Users[owner], role);
				}
				StringEnumerator enumerator2 = role.EnumMembers().GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						string current = enumerator2.Current;
						if (Database.Roles.Contains(current))
						{
							dependencyObjects.Add(role, Database.Roles[current]);
						}
						else if (Database.Users.Contains(current))
						{
							dependencyObjects.Add(role, Database.Users[current]);
						}
					}
				}
				finally
				{
					if (enumerator2 is IDisposable disposable)
					{
						disposable.Dispose();
					}
				}
			}
			foreach (SqlSmoObject dependency in dependencyObjects.GetDependencies())
			{
				if (!(dependency is DatabaseRole))
				{
					continue;
				}
				DatabaseRole databaseRole2 = (DatabaseRole)dependency;
				if (!databaseRole2.IsFixedRole && databaseRole2.Name != "public")
				{
					AddWithoutDependencyDiscovery(depList, new DependencyCollectionNode(databaseRole2.Urn, isSchemaBound: true, fRoot: true));
					if (nonDepListDictionary[DatabaseRole.UrnSuffix].Contains(databaseRole2.Urn))
					{
						nonDepListDictionary[DatabaseRole.UrnSuffix].Remove(databaseRole2.Urn);
					}
				}
			}
		}
		AddWithoutDependencyDiscoveryCollection(depList, nonDepListDictionary[DatabaseRole.UrnSuffix]);
		AddWithoutDependencyDiscoveryCollection(depList, nonDepListDictionary[ApplicationRole.UrnSuffix]);
		ScriptingPreferences scriptingPreferences = Options.GetScriptingPreferences();
		if ((CopyAllObjects || CopyAllRoles) && IsSupportedObject<ApplicationRole>(scriptingPreferences))
		{
			foreach (ApplicationRole applicationRole in Database.ApplicationRoles)
			{
				if (!nonDepListDictionary[ApplicationRole.UrnSuffix].Contains(applicationRole.Urn))
				{
					AddWithoutDependencyDiscovery(depList, new DependencyCollectionNode(applicationRole.Urn, isSchemaBound: true, fRoot: true));
				}
			}
		}
		AddWithoutDependencyDiscoveryCollection(depList, nonDepListDictionary[User.UrnSuffix]);
		if (CopyAllObjects || CopyAllUsers)
		{
			foreach (User user in Database.Users)
			{
				if (!user.IsSystemObject && string.Compare(user.Name, "guest", StringComparison.OrdinalIgnoreCase) != 0 && !nonDepListDictionary[User.UrnSuffix].Contains(user.Urn))
				{
					AddWithoutDependencyDiscovery(depList, new DependencyCollectionNode(user.Urn, isSchemaBound: true, fRoot: true));
				}
			}
		}
		AddWithoutDependencyDiscoveryCollection(depList, nonDepListDictionary[Endpoint.UrnSuffix]);
		AddWithoutDependencyDiscoveryCollection(depList, nonDepListDictionary[Login.UrnSuffix]);
		if (!CopyAllLogins)
		{
			return;
		}
		foreach (Login login in Database.Parent.Logins)
		{
			if (!login.IsSystemObject && !nonDepListDictionary[Login.UrnSuffix].Contains(login.Urn))
			{
				AddWithoutDependencyDiscovery(depList, new DependencyCollectionNode(login.Urn, isSchemaBound: true, fRoot: true));
			}
		}
	}

	private void AddSecurityObjectsWithoutOrder(DependencyCollection depList, Dictionary<string, HashSet<Urn>> nonDepListDictionary)
	{
		if (CopyAllObjects || CopyAllRoles)
		{
			foreach (DatabaseRole role in Database.Roles)
			{
				if (!role.IsFixedRole && role.Name != "public")
				{
					nonDepListDictionary[DatabaseRole.UrnSuffix].Add(role.Urn);
				}
			}
		}
		ScriptingPreferences scriptingPreferences = Options.GetScriptingPreferences();
		if ((CopyAllObjects || CopyAllRoles) && IsSupportedObject<ApplicationRole>(scriptingPreferences))
		{
			foreach (ApplicationRole applicationRole in Database.ApplicationRoles)
			{
				nonDepListDictionary[ApplicationRole.UrnSuffix].Add(applicationRole.Urn);
			}
		}
		if (CopyAllObjects || CopyAllUsers)
		{
			foreach (User user in Database.Users)
			{
				if (!user.IsSystemObject && string.Compare(user.Name, "guest", StringComparison.OrdinalIgnoreCase) != 0)
				{
					nonDepListDictionary[User.UrnSuffix].Add(user.Urn);
				}
			}
		}
		if (CopyAllLogins)
		{
			foreach (Login login in Database.Parent.Logins)
			{
				if (!login.IsSystemObject)
				{
					nonDepListDictionary[Login.UrnSuffix].Add(login.Urn);
				}
			}
		}
		AddWithoutDependencyDiscoveryCollection(depList, nonDepListDictionary[User.UrnSuffix]);
		AddWithoutDependencyDiscoveryCollection(depList, nonDepListDictionary[DatabaseRole.UrnSuffix]);
		AddWithoutDependencyDiscoveryCollection(depList, nonDepListDictionary[ApplicationRole.UrnSuffix]);
		AddWithoutDependencyDiscoveryCollection(depList, nonDepListDictionary[Endpoint.UrnSuffix]);
		AddWithoutDependencyDiscoveryCollection(depList, nonDepListDictionary[Login.UrnSuffix]);
	}

	private void AddDiscoverableObjects(HashSet<Urn> depDiscInputList, Dictionary<Type, StringCollection> originalDefaultFields)
	{
		ScriptingPreferences scriptingPreferences = Options.GetScriptingPreferences();
		if (IsSupportedObject<PartitionFunction>(scriptingPreferences))
		{
			AddAllObjects<PartitionFunction>(depDiscInputList, Database.PartitionFunctions, CopyAllPartitionFunctions, Database.PrefetchPartitionFunctions, originalDefaultFields, Array.Empty<string>());
		}
		if (IsSupportedObject<PartitionScheme>(scriptingPreferences))
		{
			AddAllObjects<PartitionScheme>(depDiscInputList, Database.PartitionSchemes, CopyAllPartitionSchemes, Database.PrefetchPartitionSchemes, originalDefaultFields, Array.Empty<string>());
		}
		if (IsSupportedObject<DatabaseScopedCredential>(scriptingPreferences))
		{
			AddAllObjects<DatabaseScopedCredential>(depDiscInputList, Database.DatabaseScopedCredentials, CopyAllDatabaseScopedCredentials, Database.PrefetchDatabaseScopedCredentials, null, Array.Empty<string>());
		}
		if (IsSupportedObject<ExternalFileFormat>(scriptingPreferences))
		{
			AddAllObjects<ExternalFileFormat>(depDiscInputList, Database.ExternalFileFormats, CopyAllExternalFileFormats, Database.PrefetchExternalFileFormats, null, Array.Empty<string>());
		}
		if (IsSupportedObject<ExternalDataSource>(scriptingPreferences))
		{
			AddAllObjects<ExternalDataSource>(depDiscInputList, Database.ExternalDataSources, CopyAllExternalDataSources, Database.PrefetchExternalDataSources, null, Array.Empty<string>());
		}
		if (CopyAllObjects || CopyAllTables)
		{
			if (PrefetchObjects)
			{
				Database.PrefetchTables(scriptingPreferences);
			}
			else if (IsSupportedObject<ExternalDataSource>(scriptingPreferences))
			{
				SetDefaultInitFields(originalDefaultFields, typeof(Table), "ID", "IsSystemObject", "IsExternal");
			}
			else
			{
				SetDefaultInitFields(originalDefaultFields, typeof(Table), "ID", "IsSystemObject");
			}
			AddAllNonSystemObjects<Table>(depDiscInputList, Database.Tables);
		}
		if (IsSupportedObject<View>(scriptingPreferences))
		{
			AddAllNonSystemObjects<View>(depDiscInputList, Database.Views, CopyAllViews, Database.PrefetchViews, originalDefaultFields, new string[2] { "ID", "IsSystemObject" });
		}
		if (IsSupportedObject<StoredProcedure>(scriptingPreferences))
		{
			AddAllNonSystemObjects<StoredProcedure>(depDiscInputList, Database.StoredProcedures, CopyAllStoredProcedures, Database.PrefetchStoredProcedures, originalDefaultFields, new string[1] { "IsSystemObject" });
		}
		if (IsSupportedObject<UserDefinedFunction>(scriptingPreferences))
		{
			AddAllNonSystemObjects<UserDefinedFunction>(depDiscInputList, Database.UserDefinedFunctions, CopyAllUserDefinedFunctions, Database.PrefetchUserDefinedFunctions, originalDefaultFields, new string[2] { "IsSystemObject", "FunctionType" });
		}
		if (IsSupportedObject<SecurityPolicy>(scriptingPreferences))
		{
			AddAllObjects<SecurityPolicy>(depDiscInputList, Database.SecurityPolicies, CopyAllSecurityPolicies, Database.PrefetchSecurityPolicy, null, Array.Empty<string>());
		}
		if (IsSupportedObject<XmlSchemaCollection>(scriptingPreferences))
		{
			AddAllObjects<XmlSchemaCollection>(depDiscInputList, Database.XmlSchemaCollections, CopyAllXmlSchemaCollections, Database.PrefetchXmlSchemaCollections, null, Array.Empty<string>());
		}
		if (IsSupportedObject<SqlAssembly>(scriptingPreferences))
		{
			AddAllNonSystemObjects<SqlAssembly>(depDiscInputList, Database.Assemblies, CopyAllSqlAssemblies, Database.PrefetchSqlAssemblies, originalDefaultFields, new string[1] { "IsSystemObject" });
		}
		if (IsSupportedObject<UserDefinedAggregate>(scriptingPreferences))
		{
			AddAllObjects<UserDefinedAggregate>(depDiscInputList, Database.UserDefinedAggregates, CopyAllUserDefinedAggregates, Database.PrefetchUserDefinedAggregates, null, Array.Empty<string>());
		}
		if (IsSupportedObject<ColumnMasterKey>(scriptingPreferences))
		{
			AddAllObjects<ColumnMasterKey>(depDiscInputList, Database.ColumnMasterKeys, CopyAllColumnMasterkey, null, null, Array.Empty<string>());
		}
		if (IsSupportedObject<ColumnEncryptionKey>(scriptingPreferences))
		{
			AddAllObjects<ColumnEncryptionKey>(depDiscInputList, Database.ColumnEncryptionKeys, CopyAllColumnEncryptionKey, Database.PrefetchColumnEncryptionKey, null, Array.Empty<string>());
		}
		if (IsSupportedObject<UserDefinedType>(scriptingPreferences))
		{
			AddAllObjects<UserDefinedType>(depDiscInputList, Database.UserDefinedTypes, CopyAllUserDefinedTypes, Database.PrefetchUserDefinedTypes, null, Array.Empty<string>());
		}
		if (IsSupportedObject<PlanGuide>(scriptingPreferences))
		{
			AddAllObjects<PlanGuide>(depDiscInputList, Database.PlanGuides, CopyAllPlanGuides, null, null, Array.Empty<string>());
		}
		if (IsSupportedObject<UserDefinedTableType>(scriptingPreferences))
		{
			AddAllObjects<UserDefinedTableType>(depDiscInputList, Database.UserDefinedTableTypes, CopyAllUserDefinedTableTypes, Database.PrefetchUserDefinedTableTypes, null, Array.Empty<string>());
		}
		if (IsSupportedObject<Synonym>(scriptingPreferences))
		{
			AddAllObjects<Synonym>(depDiscInputList, Database.Synonyms, CopyAllSynonyms, null, null, Array.Empty<string>());
		}
		if (IsSupportedObject<Sequence>(scriptingPreferences))
		{
			AddAllObjects<Sequence>(depDiscInputList, Database.Sequences, CopyAllSequences, null, null, Array.Empty<string>());
		}
		if (IsSupportedObject<DatabaseDdlTrigger>(scriptingPreferences))
		{
			AddAllObjects<DatabaseDdlTrigger>(depDiscInputList, Database.Triggers, CopyAllDatabaseTriggers, null, null, Array.Empty<string>());
		}
		if (IsSupportedObject<ExternalLanguage>(scriptingPreferences))
		{
			AddAllNonSystemObjects<ExternalLanguage>(depDiscInputList, Database.ExternalLanguages, CopyAllExternalLanguages, Database.PrefetchExternalLanguages, originalDefaultFields, new string[1] { "IsSystemObject" });
		}
		if (IsSupportedObject<ExternalLibrary>(scriptingPreferences))
		{
			AddAllObjects<ExternalLibrary>(depDiscInputList, Database.ExternalLibraries, CopyAllExternalLibraries, Database.PrefetchExternalLibraries, null, Array.Empty<string>());
		}
		if (IsSupportedObject<UserDefinedDataType>(scriptingPreferences))
		{
			AddAllObjects<UserDefinedDataType>(depDiscInputList, Database.UserDefinedDataTypes, CopyAllUserDefinedDataTypes, null, null, Array.Empty<string>());
		}
		if (IsSupportedObject<Rule>(scriptingPreferences))
		{
			AddAllObjects<Rule>(depDiscInputList, Database.Rules, CopyAllRules, Database.PrefetchRules, null, Array.Empty<string>());
		}
		if (IsSupportedObject<Default>(scriptingPreferences))
		{
			AddAllObjects<Default>(depDiscInputList, Database.Defaults, CopyAllDefaults, Database.PrefetchDefaults, null, Array.Empty<string>());
		}
	}

	private void AddAllObjects<T>(ICollection<Urn> List, SmoCollectionBase collection) where T : SqlSmoObject
	{
		foreach (T item in collection)
		{
			List.Add(item.Urn);
		}
	}

	private void AddAllObjects<T>(ICollection<Urn> List, SmoCollectionBase collection, bool copyAll, Action<ScriptingPreferences> prefetch, Dictionary<Type, StringCollection> originalDefaultFields, params string[] fields) where T : SqlSmoObject
	{
		if (CopyAllObjects || copyAll)
		{
			if (PrefetchObjects && CopySchema)
			{
				prefetch?.Invoke(Options.GetScriptingPreferences());
			}
			else if (originalDefaultFields != null)
			{
				SetDefaultInitFields(originalDefaultFields, typeof(T), fields);
			}
			AddAllObjects<T>(List, collection);
		}
	}

	private void AddAllNonSystemObjects<T>(ICollection<Urn> List, SmoCollectionBase collection) where T : SqlSmoObject
	{
		foreach (T item in collection)
		{
			if (!item.GetPropValueOptional("IsSystemObject", defaultValue: false))
			{
				List.Add(item.Urn);
			}
		}
	}

	private void AddAllNonSystemObjects<T>(ICollection<Urn> List, SmoCollectionBase collection, bool copyAll, Action<ScriptingPreferences> prefetch, Dictionary<Type, StringCollection> originalDefaultFields, params string[] fields) where T : SqlSmoObject
	{
		if (CopyAllObjects || copyAll)
		{
			if (PrefetchObjects && CopySchema)
			{
				prefetch?.Invoke(Options.GetScriptingPreferences());
			}
			else if (originalDefaultFields != null)
			{
				SetDefaultInitFields(originalDefaultFields, typeof(T), fields);
			}
			AddAllNonSystemObjects<T>(List, collection);
		}
	}

	private bool IsSupportedObject<T>(ScriptingPreferences sp) where T : SqlSmoObject
	{
		if (SmoUtility.IsSupportedObject(typeof(T), Database.ServerVersion, Database.DatabaseEngineType, Database.DatabaseEngineEdition))
		{
			return SmoUtility.IsSupportedObject(typeof(T), ScriptingOptions.ConvertToServerVersion(sp.TargetServerVersion), sp.TargetDatabaseEngineType, sp.TargetDatabaseEngineEdition);
		}
		return false;
	}

	private void SeparateDiscoverySupportedObjects(HashSet<Urn> depDiscInputList, HashSet<Urn> nonDepDiscList, Dictionary<string, HashSet<Urn>> nonDepListDictionary, HashSet<Urn> nonDepList)
	{
		foreach (object @object in ObjectList)
		{
			if (!(@object is SqlSmoObject))
			{
				if (!Scripter.Options.ContinueScriptingOnError)
				{
					if (@object == null)
					{
						throw new ArgumentNullException();
					}
					throw new SmoException(ExceptionTemplatesImpl.NeedToPassObject(@object.GetType().ToString()));
				}
				continue;
			}
			switch (@object.GetType().Name)
			{
			case "View":
			case "StoredProcedure":
			case "UserDefinedFunction":
			case "PartitionScheme":
			case "PartitionFunction":
			case "XmlSchemaCollection":
			case "UserDefinedAggregate":
			case "UserDefinedType":
			case "SqlAssembly":
			case "Synonym":
			case "Sequence":
			case "PlanGuide":
			case "UserDefinedTableType":
			case "Rule":
			case "Default":
			case "DdlTrigger":
			case "Trigger":
			case "UserDefinedDataType":
			case "Table":
				depDiscInputList.Add(((SqlSmoObject)@object).Urn);
				break;
			case "FullTextCatalog":
			case "FullTextStopList":
			case "SearchPropertyList":
			case "Schema":
				nonDepDiscList.Add(((SqlSmoObject)@object).Urn);
				break;
			case "User":
			case "ApplicationRole":
			case "Role":
			case "Login":
			case "Endpoint":
				nonDepListDictionary[((SqlSmoObject)@object).Urn.Type].Add(((SqlSmoObject)@object).Urn);
				break;
			default:
				nonDepList.Add(((SqlSmoObject)@object).Urn);
				break;
			}
		}
	}

	private void CheckDownLevelScripting(DependencyCollection depList, ScriptingPreferences preferences)
	{
		List<DependencyCollectionNode> list = new List<DependencyCollectionNode>();
		foreach (DependencyCollectionNode dep in depList)
		{
			if ((Options.TargetServerVersionInternal < SqlServerVersionInternal.Version100 && Database.CompatibilityLevel >= CompatibilityLevel.Version90 && !CanScriptDownlevel(dep.Urn, Options.TargetServerVersionInternal)) || !CanScriptCrossPlatform(dep.Urn, preferences))
			{
				list.Add(dep);
			}
		}
		foreach (DependencyCollectionNode item in list)
		{
			depList.Remove(item);
			incompatibleObjects.Add(item.Urn);
		}
		if (list.Any() && !Options.ContinueScriptingOnError)
		{
			throw new SmoException(ExceptionTemplatesImpl.UnsupportedVersionException);
		}
	}

	private bool CanScriptCrossPlatform(Urn urn, ScriptingPreferences preferences)
	{
		SqlSmoObject sqlSmoObject = null;
		if (preferences.TargetDatabaseEngineEdition == DatabaseEngineEdition.SqlDataWarehouse && Database.Parent.DatabaseEngineEdition != DatabaseEngineEdition.SqlDataWarehouse && urn.Type == UserDefinedFunction.UrnSuffix)
		{
			sqlSmoObject = Database.Parent.GetSmoObject(urn);
			if (sqlSmoObject is UserDefinedFunction { FunctionType: not UserDefinedFunctionType.Scalar })
			{
				return false;
			}
		}
		if (!CopyExternalTables && (Database.Parent.DatabaseEngineType != DatabaseEngineType.Standalone || Database.Parent.VersionMajor >= 13) && urn.Type == Table.UrnSuffix)
		{
			sqlSmoObject = sqlSmoObject ?? Database.Parent.GetSmoObject(urn);
			if (sqlSmoObject is Table table && table.IsSupportedProperty("IsExternal") && table.IsExternal)
			{
				return false;
			}
		}
		return true;
	}

	private void TreeTraversal(DependencyTreeNode node, HashSet<Urn> visitedUrns)
	{
		if (visitedUrns.Add(node.Urn))
		{
			if (node.NextSibling != null)
			{
				TreeTraversal(node.NextSibling, visitedUrns);
			}
			if (node.HasChildNodes)
			{
				TreeTraversal(node.FirstChild, visitedUrns);
			}
		}
	}

	private void SetDefaultInitFields(Dictionary<Type, StringCollection> originalDefaultFields, Type type, params string[] fields)
	{
		originalDefaultFields[type] = Database.GetServerObject().GetDefaultInitFields(type);
		Database.GetServerObject().SetDefaultInitFields(type, fields);
	}

	private void RestoreDefaultInitFields(Dictionary<Type, StringCollection> originalDefaultFields)
	{
		if (originalDefaultFields == null)
		{
			return;
		}
		foreach (KeyValuePair<Type, StringCollection> originalDefaultField in originalDefaultFields)
		{
			Database.GetServerObject().SetDefaultInitFields(originalDefaultField.Key, originalDefaultField.Value);
		}
	}

	private bool CanScriptDownlevel(Urn urn, SqlServerVersionInternal targetVersion)
	{
		TraceHelper.Assert(null != urn, "null == urn");
		SqlSmoObject smoObject = Database.Parent.GetSmoObject(urn);
		if (smoObject is Table && ContainsUnsupportedType(((Table)smoObject).Columns, targetVersion))
		{
			return false;
		}
		if (smoObject is View && ContainsUnsupportedType(((View)smoObject).Columns, targetVersion))
		{
			return false;
		}
		if (smoObject is StoredProcedure)
		{
			StoredProcedure storedProcedure = (StoredProcedure)smoObject;
			if (ContainsUnsupportedType(storedProcedure.Parameters, targetVersion) || (storedProcedure.ImplementationType == ImplementationType.SqlClr && targetVersion < SqlServerVersionInternal.Version90))
			{
				return false;
			}
		}
		if (smoObject is UserDefinedFunction)
		{
			UserDefinedFunction userDefinedFunction = (UserDefinedFunction)smoObject;
			if (ContainsUnsupportedType(userDefinedFunction.Parameters, targetVersion) || (userDefinedFunction.DataType != null && userDefinedFunction.FunctionType == UserDefinedFunctionType.Scalar && IsUnsupportedType(userDefinedFunction.DataType.SqlDataType, targetVersion)) || (userDefinedFunction.ImplementationType == ImplementationType.SqlClr && targetVersion < SqlServerVersionInternal.Version90))
			{
				return false;
			}
		}
		if (smoObject is UserDefinedDataType && ((UserDefinedDataType)smoObject).SystemType == "xml" && targetVersion < SqlServerVersionInternal.Version90)
		{
			return false;
		}
		return true;
	}

	private bool IsUnsupportedType(SqlDataType type, SqlServerVersionInternal targetVersion)
	{
		if (targetVersion == SqlServerVersionInternal.Version80)
		{
			switch (type)
			{
			case SqlDataType.NVarCharMax:
			case SqlDataType.VarBinaryMax:
			case SqlDataType.VarCharMax:
			case SqlDataType.Xml:
			case SqlDataType.UserDefinedTableType:
			case SqlDataType.HierarchyId:
			case SqlDataType.Geometry:
			case SqlDataType.Geography:
				return true;
			default:
				return false;
			}
		}
		if ((uint)(type - 40) <= 3u)
		{
			return true;
		}
		return false;
	}

	private bool ContainsUnsupportedType(ParameterCollectionBase parms, SqlServerVersionInternal targetVersion)
	{
		foreach (Parameter parm in parms)
		{
			if (IsUnsupportedType(parm.DataType.SqlDataType, targetVersion))
			{
				return true;
			}
		}
		return false;
	}

	private bool ContainsUnsupportedType(ColumnCollection cols, SqlServerVersionInternal targetVersion)
	{
		foreach (Column col in cols)
		{
			if (IsUnsupportedType(col.DataType.SqlDataType, targetVersion))
			{
				return true;
			}
		}
		return false;
	}

	private DependencyCollection GetDependencyOrderedCollection(HashSet<Urn> transferSet)
	{
		SmoDependencyOrderer smoDependencyOrderer = new SmoDependencyOrderer(Database.GetServerObject());
		smoDependencyOrderer.ScriptingPreferences = (ScriptingPreferences)Options.GetScriptingPreferences().Clone();
		smoDependencyOrderer.ScriptingPreferences.IncludeScripts.Data = CopyData;
		smoDependencyOrderer.ScriptingPreferences.IncludeScripts.Ddl = CopySchema;
		smoDependencyOrderer.ScriptingPreferences.Behavior = ScriptBehavior.Create;
		smoDependencyOrderer.ScriptingPreferences.IncludeScripts.Associations = false;
		smoDependencyOrderer.ScriptingPreferences.IncludeScripts.Permissions = false;
		smoDependencyOrderer.ScriptingPreferences.IncludeScripts.Owner = false;
		smoDependencyOrderer.creatingDictionary = new CreatingObjectDictionary(Database.GetServerObject());
		List<Urn> list = new List<Urn>();
		list.AddRange(transferSet);
		List<Urn> list2 = smoDependencyOrderer.Order(list);
		DependencyCollection dependencyCollection = new DependencyCollection();
		if (CopySchema)
		{
			foreach (Urn item in list2)
			{
				if (!item.Type.Equals("Special"))
				{
					dependencyCollection.Add(new DependencyCollectionNode(item, isSchemaBound: true, fRoot: true));
				}
				else if (item.Parent.Type == "Object")
				{
					dependencyCollection.Add(new DependencyCollectionNode(item.Parent.Parent, isSchemaBound: true, fRoot: true));
				}
			}
		}
		else
		{
			foreach (Urn item2 in list2)
			{
				TraceHelper.Assert(item2.Type.Equals("Special") && item2.Parent.Type == "Data", "only data entries expected");
				dependencyCollection.Add(new DependencyCollectionNode(item2.Parent.Parent, isSchemaBound: true, fRoot: true));
			}
		}
		return dependencyCollection;
	}
}
