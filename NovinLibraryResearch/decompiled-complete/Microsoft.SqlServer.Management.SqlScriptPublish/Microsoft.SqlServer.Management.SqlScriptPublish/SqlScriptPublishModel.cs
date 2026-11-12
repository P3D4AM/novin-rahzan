using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Diagnostics;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.SqlScriptPublish;

public class SqlScriptPublishModel
{
	internal const string ComponentName = "DPW";

	private readonly SqlQueryHandler sqlQueryHandler;

	private readonly Microsoft.SqlServer.Management.Smo.Server smoServer;

	private readonly string databaseName;

	private UrnCollection selectedObjects;

	private bool scriptAllObjects;

	private bool skipCreateDatabase;

	private string rawScript;

	private OutputType outputType;

	private IScriptPublishOptions shellScriptingOptions;

	private SqlScriptOptions scriptAdvancedOptions;

	private Dictionary<Urn, int> urnItemCount;

	private readonly HashSet<Urn> erroredItems = new HashSet<Urn>();

	private int step;

	public string ServerName => smoServer.Name;

	public string DatabaseName => databaseName;

	public UrnCollection SelectedObjects
	{
		get
		{
			if (selectedObjects == null)
			{
				selectedObjects = new UrnCollection();
			}
			return selectedObjects;
		}
	}

	public bool ScriptAllObjects
	{
		get
		{
			return scriptAllObjects;
		}
		set
		{
			scriptAllObjects = value;
		}
	}

	public bool SkipCreateDatabase
	{
		get
		{
			return skipCreateDatabase;
		}
		set
		{
			skipCreateDatabase = value;
		}
	}

	public OutputType OutputType
	{
		get
		{
			return outputType;
		}
		set
		{
			outputType = value;
		}
	}

	public SqlScriptOptions AdvancedOptions => GetAdvancedScriptingOptions();

	public string RawScript
	{
		get
		{
			return rawScript;
		}
		set
		{
			rawScript = value;
		}
	}

	public bool AllowSystemObjects { get; set; }

	internal Microsoft.SqlServer.Management.Smo.Server Server
	{
		get
		{
			SqlScriptPublishModelTraceHelper.Assert(smoServer != null);
			return smoServer;
		}
	}

	internal IScriptPublishOptions ShellScriptingOptions => shellScriptingOptions;

	public event EventHandler<ScriptEventArgs> ScriptProgress;

	public event EventHandler<ScriptEventArgs> ScriptError;

	public event EventHandler<ScriptItemsArgs> ScriptItemsCollected;

	private SqlScriptPublishModel()
	{
		SqlScriptPublishModelTraceHelper.SetDefaultLevel("DPW", 1u);
	}

	public SqlScriptPublishModel(SqlConnectionInfo sqlConnectionInfo, string databaseName, IScriptPublishOptions shellScriptingOptions)
		: this()
	{
		if (sqlConnectionInfo == null)
		{
			throw new ArgumentNullException("sqlConnectionInfo");
		}
		if (shellScriptingOptions == null)
		{
			throw new ArgumentNullException("shellScriptingOptions");
		}
		this.shellScriptingOptions = shellScriptingOptions;
		this.databaseName = (string.IsNullOrEmpty(databaseName) ? "master" : databaseName);
		try
		{
			ServerConnection serverConnection = ((!(sqlConnectionInfo is SqlConnectionInfoWithConnection sqlConnectionInfoWithConnection)) ? new ServerConnection(sqlConnectionInfo).GetDatabaseConnection(databaseName) : sqlConnectionInfoWithConnection.ServerConnection.Copy().GetDatabaseConnection(databaseName));
			smoServer = new Microsoft.SqlServer.Management.Smo.Server(serverConnection);
		}
		catch (Exception innerException)
		{
			throw new SqlScriptPublishException(SR.UnableToConnect(sqlConnectionInfo.ServerName), innerException);
		}
		InitServer();
		sqlQueryHandler = new SqlQueryHandler(this);
	}

	public SqlScriptPublishModel(string connectionString)
		: this()
	{
		SqlScriptPublishModelTraceHelper.Assert(!string.IsNullOrEmpty(connectionString), "connectionString is empty");
		if (string.IsNullOrEmpty(connectionString))
		{
			throw new ArgumentNullException("connectionString");
		}
		SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
		string dataSource = sqlConnectionStringBuilder.DataSource;
		try
		{
			SqlConnectionInfo sci = ((!sqlConnectionStringBuilder.IntegratedSecurity) ? new SqlConnectionInfo(sqlConnectionStringBuilder.DataSource, sqlConnectionStringBuilder.UserID, sqlConnectionStringBuilder.Password) : new SqlConnectionInfo(sqlConnectionStringBuilder.DataSource));
			smoServer = new Microsoft.SqlServer.Management.Smo.Server(new ServerConnection(sci));
			InitServer();
			databaseName = sqlConnectionStringBuilder.InitialCatalog;
			if (string.IsNullOrEmpty(databaseName))
			{
				databaseName = "master";
			}
		}
		catch (Exception innerException)
		{
			throw new SqlScriptPublishException(SR.UnableToConnect(dataSource), innerException);
		}
		sqlQueryHandler = new SqlQueryHandler(this);
	}

	private void InitServer()
	{
		StringCollection defaultInitFields = Server.GetDefaultInitFields(typeof(Database), smoServer.DatabaseEngineEdition);
		defaultInitFields.AddRange(new string[3] { "CompatibilityLevel", "IsMirroringEnabled", "Collation" });
		smoServer.SetDefaultInitFields(typeof(Database), defaultInitFields);
	}

	public void GenerateScript(ScriptOutputOptions outputOptions)
	{
		if (string.IsNullOrEmpty(outputOptions.SaveFileName) && (outputOptions.ScriptDestination == ScriptDestination.ToSingleFile || outputOptions.ScriptDestination == ScriptDestination.ToFilePerObject || outputOptions.ScriptDestination == ScriptDestination.ToNotebook))
		{
			throw new SqlScriptPublishException(SR.ValueIsNullOrEmpty("SaveFileName"));
		}
		SqlScriptGenerator sqlScriptGenerator = new SqlScriptGenerator(this);
		sqlScriptGenerator.GetUrnList();
		sqlScriptGenerator.DoScript(outputOptions);
	}

	public IEnumerable<DatabaseObjectType> GetDatabaseObjectTypes()
	{
		return sqlQueryHandler.GetDatabaseObjectTypes();
	}

	public IEnumerable<KeyValuePair<string, string>> EnumChildrenForDatabaseObjectType(DatabaseObjectType objectType)
	{
		return sqlQueryHandler.EnumChildrenForDatabaseObjectType(objectType);
	}

	public void RefreshDatabaseCache()
	{
		Database database = smoServer.Databases[databaseName];
		foreach (SfcMetadataRelation relation in new SfcMetadataDiscovery(database.GetType()).Relations)
		{
			if (relation.Relationship != SfcRelationship.ObjectContainer)
			{
				continue;
			}
			object obj = null;
			try
			{
				obj = database.GetType().GetProperty(relation.PropertyName).GetValue(database, null);
			}
			catch (TargetInvocationException)
			{
			}
			if (obj != null)
			{
				try
				{
					(obj as SmoCollectionBase).Refresh();
				}
				catch (EnumeratorException)
				{
				}
			}
		}
	}

	internal void OnScriptError(object sender, ScriptingErrorEventArgs e)
	{
		if (this.ScriptError != null)
		{
			Urn selectedItem = GetSelectedItem(e.Current);
			erroredItems.Add(selectedItem);
			ScriptEventArgs e2 = new ScriptEventArgs(selectedItem, e.InnerException);
			this.ScriptError(sender, e2);
		}
	}

	internal void OnScriptingProgress(object sender, ScriptingProgressEventArgs e)
	{
		if (e.ProgressStage == ScriptingProgressStages.FilteringDone)
		{
			ValidateUrnList(e.Urns);
			return;
		}
		step = ((AdvancedOptions.ScriptCreateDrop != SqlScriptOptions.ScriptCreateDropOptions.ScriptCreateDrop) ? 1 : 2);
		if (this.ScriptItemsCollected == null || e.ProgressStage != ScriptingProgressStages.OrderingDone)
		{
			return;
		}
		urnItemCount = new Dictionary<Urn, int>();
		List<Urn> list = new List<Urn>();
		foreach (Urn urn in e.Urns)
		{
			Urn selectedItem = GetSelectedItem(urn);
			if (urnItemCount.ContainsKey(selectedItem))
			{
				urnItemCount[selectedItem] += step;
				continue;
			}
			int value = step;
			urnItemCount.Add(selectedItem, value);
			list.Add(selectedItem);
		}
		this.ScriptItemsCollected(this, new ScriptItemsArgs(list));
	}

	internal void OnObjectScriptingProgress(object sender, ObjectScriptingEventArgs e)
	{
		if (this.ScriptProgress != null)
		{
			Urn selectedItem = GetSelectedItem(e.Original);
			urnItemCount[selectedItem]--;
			if (!erroredItems.Contains(selectedItem))
			{
				ScriptEventArgs e2 = new ScriptEventArgs(selectedItem, null, urnItemCount[selectedItem] == 0);
				this.ScriptProgress(sender, e2);
			}
		}
	}

	private SqlScriptOptions GetAdvancedScriptingOptions()
	{
		if (scriptAdvancedOptions == null)
		{
			scriptAdvancedOptions = new SqlScriptOptions(Server.Version);
			IScriptPublishOptions scriptPublishOptions = ShellScriptingOptions;
			if (scriptPublishOptions != null)
			{
				scriptAdvancedOptions.LoadShellScriptingOptions(scriptPublishOptions, smoServer);
			}
			if (ScriptAllObjects)
			{
				SqlTransferOptions sqlTransferOptions = new SqlTransferOptions(Server.Version);
				sqlTransferOptions.Copy(scriptAdvancedOptions);
				sqlTransferOptions.GenerateScriptForDependentObjects = SqlScriptOptions.BooleanTypeOptions.True;
				sqlTransferOptions.ScriptIndexes = SqlScriptOptions.BooleanTypeOptions.True;
				scriptAdvancedOptions = sqlTransferOptions;
			}
		}
		else if ((ScriptAllObjects && !(scriptAdvancedOptions is SqlTransferOptions)) || (!ScriptAllObjects && scriptAdvancedOptions is SqlTransferOptions))
		{
			SqlScriptPublishModelTraceHelper.Assert(condition: false, "WARNING: all changes to AdvancedOptions have been lost due to changes in ScriptAllObjects.");
			scriptAdvancedOptions = null;
			return GetAdvancedScriptingOptions();
		}
		return scriptAdvancedOptions;
	}

	private void ValidateUrnList(IEnumerable<Urn> urnList)
	{
		if (scriptAdvancedOptions.TargetDatabaseEngineType != SqlScriptOptions.ScriptDatabaseEngineType.SqlAzure || smoServer.DatabaseEngineType != DatabaseEngineType.Standalone)
		{
			return;
		}
		HashSet<string> hashSet = new HashSet<string>();
		DatabaseEngineEdition edition = DatabaseEngineEdition.SqlDatabase;
		switch (scriptAdvancedOptions.TargetDatabaseEngineEdition)
		{
		case SqlScriptOptions.ScriptDatabaseEngineEdition.SqlDatawarehouseEdition:
			edition = DatabaseEngineEdition.SqlDataWarehouse;
			break;
		case SqlScriptOptions.ScriptDatabaseEngineEdition.SqlServerOnDemandEdition:
			edition = DatabaseEngineEdition.SqlDatabase;
			break;
		}
		foreach (DatabaseObjectType item in sqlQueryHandler.InvalidObjectTypesForAzure(edition))
		{
			hashSet.Add(item.ToString());
		}
		foreach (Urn urn in urnList)
		{
			if (hashSet.Contains(urn.Type) && this.ScriptError != null)
			{
				SqlScriptPublishException ex = new SqlScriptPublishException(SR.InvalidObjectTypeForVersion(urn.GetAttribute("Name"), urn.Type));
				ScriptEventArgs e = new ScriptEventArgs(null, ex);
				this.ScriptError(this, e);
				throw ex;
			}
		}
	}

	private Urn GetSelectedItem(Urn urn)
	{
		if (urn.Type.Equals("Special"))
		{
			return GetSelectedItem(urn.Parent.Parent);
		}
		if (urn.Parent == null || urn.Parent.Type.Equals("Server") || urn.Parent.Type.Equals("Database"))
		{
			return urn;
		}
		return GetSelectedItem(urn.Parent);
	}
}
