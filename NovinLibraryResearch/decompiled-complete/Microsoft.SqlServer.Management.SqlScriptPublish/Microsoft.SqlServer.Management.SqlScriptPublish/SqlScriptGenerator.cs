using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Diagnostics;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Notebook;

namespace Microsoft.SqlServer.Management.SqlScriptPublish;

public class SqlScriptGenerator
{
	private SqlScriptPublishModel model;

	private SqlScriptOptions scriptOptions;

	private ScriptMaker scriptMaker;

	private List<Urn> urnList;

	private string BatchTerminator
	{
		get
		{
			if (model.ShellScriptingOptions == null)
			{
				return "GO";
			}
			return model.ShellScriptingOptions.BatchSeparator;
		}
	}

	private bool ScriptBatchTerminator
	{
		get
		{
			if (model.ShellScriptingOptions == null)
			{
				return true;
			}
			return model.ShellScriptingOptions.DelimitStatements;
		}
	}

	public SqlScriptGenerator(SqlScriptPublishModel model)
	{
		if (model == null)
		{
			throw new ArgumentNullException("model");
		}
		this.model = model;
		scriptOptions = this.model.AdvancedOptions;
		SetupScriptMaker();
	}

	private void SetupScriptMaker()
	{
		scriptMaker = new ScriptMaker(model.Server);
		ScriptingOptions scriptingOptions = new ScriptingOptions(model.Server);
		SetScriptingOptions(scriptingOptions);
		scriptMaker.Preferences = scriptingOptions.GetScriptingPreferences();
		switch (scriptOptions.TypeOfDataToScript)
		{
		case SqlScriptOptions.TypeOfDataToScriptOptions.SchemaAndData:
			scriptMaker.Preferences.IncludeScripts.Ddl = true;
			scriptMaker.Preferences.IncludeScripts.Data = true;
			break;
		case SqlScriptOptions.TypeOfDataToScriptOptions.DataOnly:
			scriptMaker.Preferences.IncludeScripts.Ddl = false;
			scriptMaker.Preferences.IncludeScripts.Data = true;
			break;
		case SqlScriptOptions.TypeOfDataToScriptOptions.SchemaOnly:
			scriptMaker.Preferences.IncludeScripts.Ddl = true;
			scriptMaker.Preferences.IncludeScripts.Data = false;
			break;
		}
		switch (scriptOptions.ScriptCreateDrop)
		{
		case SqlScriptOptions.ScriptCreateDropOptions.ScriptCreate:
			scriptMaker.Preferences.Behavior = ScriptBehavior.Create;
			break;
		case SqlScriptOptions.ScriptCreateDropOptions.ScriptDrop:
			scriptMaker.Preferences.Behavior = ScriptBehavior.Drop;
			break;
		case SqlScriptOptions.ScriptCreateDropOptions.ScriptCreateDrop:
			scriptMaker.Preferences.Behavior = ScriptBehavior.DropAndCreate;
			break;
		}
		SmoDependencyDiscoverer smoDependencyDiscoverer = new SmoDependencyDiscoverer(model.Server);
		smoDependencyDiscoverer.Preferences = scriptMaker.Preferences;
		smoDependencyDiscoverer.Preferences.DependentObjects = scriptingOptions.WithDependencies && !model.ScriptAllObjects;
		smoDependencyDiscoverer.Preferences.IgnoreDependencyError = true;
		smoDependencyDiscoverer.filteredUrnTypes = scriptingOptions.GetSmoUrnFilterForDiscovery(model.Server).filteredTypes;
		scriptMaker.discoverer = smoDependencyDiscoverer;
		scriptMaker.ObjectScripting += OnObjectScriptingProgress;
		scriptMaker.ScriptingProgress += OnScriptingProgress;
		scriptMaker.ScriptingError += OnScriptingError;
		if (scriptOptions.IncludeUnsupportedStatements == SqlScriptOptions.BooleanTypeOptions.True && (scriptMaker.Preferences.TargetServerVersion != ScriptingOptions.ConvertVersion(model.Server.Version) || scriptMaker.Preferences.TargetDatabaseEngineType != model.Server.DatabaseEngineType))
		{
			scriptMaker.Retry += OnRetryRequested;
		}
	}

	internal void GetUrnList()
	{
		Transfer transfer = GetTransfer();
		urnList = new List<Urn>();
		scriptMaker.DatabasePrefetch = GetDatabasePrefetch();
		urnList.AddRange(model.SelectedObjects);
		if (model.ScriptAllObjects)
		{
			if (!model.SkipCreateDatabase)
			{
				((SqlTransferOptions)scriptOptions).ScriptDatabaseCreate = SqlScriptOptions.BooleanTypeOptions.True;
				urnList.Add(model.Server.Databases[model.DatabaseName].Urn);
			}
			urnList.AddRange(transfer.EnumObjects(ordered: false));
		}
		else
		{
			AddScriptItems();
		}
	}

	internal void DoScript(ScriptOutputOptions outputOptions)
	{
		SqlScriptPublishModelTraceHelper.Assert(outputOptions != null, "outputOptions is null");
		if (outputOptions == null)
		{
			throw new ArgumentNullException("outputOptions");
		}
		ISmoScriptWriter scriptWriter = GetScriptWriter(outputOptions);
		try
		{
			scriptMaker.Script(urnList.ToArray(), scriptWriter);
		}
		catch (Exception innerException)
		{
			throw new SqlScriptPublishException(SR.ERROR_ScriptingFailed, innerException);
		}
		finally
		{
			CloseWriter(outputOptions, scriptWriter);
		}
	}

	private void CloseWriter(ScriptOutputOptions outputOptions, ISmoScriptWriter writer)
	{
		switch (outputOptions.ScriptDestination)
		{
		case ScriptDestination.ToClipboard:
		case ScriptDestination.ToEditor:
		{
			SmoStringWriter smoStringWriter = writer as SmoStringWriter;
			model.RawScript = GetScript(smoStringWriter.FinalStringCollection);
			break;
		}
		case ScriptDestination.ToSingleFile:
			(writer as SingleFileWriter).Close();
			break;
		case ScriptDestination.ToFilePerObject:
			(writer as FilePerObjectWriter).Close();
			break;
		case ScriptDestination.ToNotebook:
			(writer as NotebookFileWriter).Close();
			break;
		}
	}

	private ISmoScriptWriter GetScriptWriter(ScriptOutputOptions outputOptions)
	{
		ISmoScriptWriter result = null;
		Encoding encoding = ((outputOptions.SaveFileType == ScriptFileType.Unicode) ? Encoding.Unicode : Encoding.Default);
		switch (outputOptions.ScriptDestination)
		{
		case ScriptDestination.ToClipboard:
		case ScriptDestination.ToEditor:
			result = new SmoStringWriter();
			break;
		case ScriptDestination.ToSingleFile:
			result = new SingleFileWriter(outputOptions.SaveFileName, outputOptions.SaveFileMode == ScriptFileMode.Append, encoding)
			{
				BatchTerminator = BatchTerminator,
				ScriptBatchTerminator = ScriptBatchTerminator
			};
			break;
		case ScriptDestination.ToFilePerObject:
			result = new FilePerObjectWriter(outputOptions.SaveFileName)
			{
				AppendToFile = (outputOptions.SaveFileMode == ScriptFileMode.Append),
				Encoding = encoding,
				BatchTerminator = BatchTerminator,
				ScriptBatchTerminator = ScriptBatchTerminator
			};
			break;
		case ScriptDestination.ToNotebook:
			result = new NotebookFileWriter(outputOptions.SaveFileName)
			{
				BatchTerminator = BatchTerminator,
				ScriptBatchTerminator = ScriptBatchTerminator,
				Indented = outputOptions.Indented
			};
			break;
		case ScriptDestination.ToCustomWriter:
			result = outputOptions.CustomSmoScriptWriter;
			break;
		}
		return result;
	}

	private string GetScript(StringCollection stringCollection)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringEnumerator enumerator = stringCollection.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				stringBuilder.Append(current);
				if (ScriptBatchTerminator)
				{
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}{1}{2}", current.EndsWith(Environment.NewLine) ? string.Empty : Environment.NewLine, BatchTerminator, Environment.NewLine);
				}
				else
				{
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, Environment.NewLine);
				}
			}
		}
		finally
		{
			if (enumerator is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}
		return stringBuilder.ToString();
	}

	private void OnScriptingError(object sender, ScriptingErrorEventArgs e)
	{
		model.OnScriptError(sender, e);
	}

	private void OnScriptingProgress(object sender, ScriptingProgressEventArgs e)
	{
		model.OnScriptingProgress(sender, e);
	}

	private void OnObjectScriptingProgress(object sender, ObjectScriptingEventArgs e)
	{
		model.OnObjectScriptingProgress(sender, e);
	}

	private void OnRetryRequested(object sender, RetryRequestedEventArgs e)
	{
		e.PreText = SR.InvalidScriptPreText;
		e.PostText = SR.InvalidScriptPostText;
		e.ScriptingPreferences.TargetDatabaseEngineType = model.Server.ServerType;
		switch (model.Server.VersionMajor)
		{
		case 10:
			if (model.Server.VersionMinor == 50)
			{
				e.ScriptingPreferences.TargetServerVersion = SqlServerVersion.Version105;
			}
			else
			{
				e.ScriptingPreferences.TargetServerVersion = SqlServerVersion.Version100;
			}
			break;
		case 9:
			e.ScriptingPreferences.TargetServerVersion = SqlServerVersion.Version90;
			break;
		case 8:
			e.ScriptingPreferences.TargetServerVersion = SqlServerVersion.Version80;
			break;
		default:
			e.ScriptingPreferences.TargetServerVersion = SqlServerVersion.Version105;
			break;
		}
		e.ShouldRetry = true;
	}

	private Transfer GetTransfer()
	{
		Transfer transfer = new Transfer(model.Server.Databases[model.DatabaseName])
		{
			CopyExternalTables = true
		};
		if (scriptOptions.ScriptLogins == SqlScriptOptions.BooleanTypeOptions.True)
		{
			transfer.CopyAllLogins = true;
		}
		SetScriptingOptions(transfer.Options);
		transfer.PrefetchObjects = false;
		return transfer;
	}

	private IDatabasePrefetch GetDatabasePrefetch()
	{
		HashSet<UrnTypeKey> filteredTypes = ((scriptMaker.discoverer is SmoDependencyDiscoverer smoDependencyDiscoverer) ? smoDependencyDiscoverer.filteredUrnTypes : new HashSet<UrnTypeKey>());
		return new GswDatabasePrefetch(model.Server.Databases[model.DatabaseName], scriptMaker.Preferences, filteredTypes);
	}

	private void AddScriptItems()
	{
		urnList.AddRange(model.SelectedObjects);
		if (scriptOptions.ScriptLogins != 0)
		{
			return;
		}
		foreach (Login login in model.Server.Logins)
		{
			if (!login.IsSystemObject)
			{
				urnList.Add(login.Urn);
			}
		}
	}

	private void SetScriptingOptions(ScriptingOptions scriptingOptions)
	{
		scriptingOptions.AllowSystemObjects = model.AllowSystemObjects;
		scriptingOptions.EnforceScriptingOptions = true;
		scriptingOptions.IncludeDatabaseRoleMemberships = true;
		scriptingOptions.Bindings = SqlScriptOptions.ConvertBooleanTypeOptionToBoolean(scriptOptions.Bindings);
		scriptingOptions.ContinueScriptingOnError = SqlScriptOptions.ConvertBooleanTypeOptionToBoolean(scriptOptions.ContinueScriptingOnError);
		scriptingOptions.ChangeTracking = SqlScriptOptions.ConvertBooleanTypeOptionToBoolean(scriptOptions.ScriptChangeTracking);
		scriptingOptions.IncludeHeaders = SqlScriptOptions.ConvertBooleanTypeOptionToBoolean(scriptOptions.IncludeDescriptiveHeaders);
		scriptingOptions.IncludeScriptingParametersHeader = SqlScriptOptions.ConvertBooleanTypeOptionToBoolean(scriptOptions.IncludeScriptingParametersHeader);
		scriptingOptions.WithDependencies = SqlScriptOptions.ConvertBooleanTypeOptionToBoolean(scriptOptions.GenerateScriptForDependentObjects);
		scriptingOptions.ScriptDataCompression = SqlScriptOptions.ConvertBooleanTypeOptionToBoolean(scriptOptions.ScriptDataCompressionOptions);
		if (scriptOptions.ScriptCompatibilityOption == SqlScriptOptions.ScriptCompatibilityOptions.Script150Compat)
		{
			scriptingOptions.TargetServerVersion = SqlServerVersion.Version150;
		}
		else if (scriptOptions.ScriptCompatibilityOption == SqlScriptOptions.ScriptCompatibilityOptions.Script140Compat)
		{
			scriptingOptions.TargetServerVersion = SqlServerVersion.Version140;
		}
		else if (scriptOptions.ScriptCompatibilityOption == SqlScriptOptions.ScriptCompatibilityOptions.Script130Compat)
		{
			scriptingOptions.TargetServerVersion = SqlServerVersion.Version130;
		}
		else if (scriptOptions.ScriptCompatibilityOption == SqlScriptOptions.ScriptCompatibilityOptions.Script120Compat)
		{
			scriptingOptions.TargetServerVersion = SqlServerVersion.Version120;
		}
		else if (scriptOptions.ScriptCompatibilityOption == SqlScriptOptions.ScriptCompatibilityOptions.Script110Compat)
		{
			scriptingOptions.TargetServerVersion = SqlServerVersion.Version110;
		}
		else if (scriptOptions.ScriptCompatibilityOption == SqlScriptOptions.ScriptCompatibilityOptions.Script105Compat)
		{
			scriptingOptions.TargetServerVersion = SqlServerVersion.Version105;
		}
		else if (scriptOptions.ScriptCompatibilityOption == SqlScriptOptions.ScriptCompatibilityOptions.Script100Compat)
		{
			scriptingOptions.TargetServerVersion = SqlServerVersion.Version100;
		}
		else if (scriptOptions.ScriptCompatibilityOption == SqlScriptOptions.ScriptCompatibilityOptions.Script90Compat)
		{
			scriptingOptions.TargetServerVersion = SqlServerVersion.Version90;
		}
		else
		{
			SqlScriptPublishModelTraceHelper.Assert(condition: false, "This part of the code is not updated corresponding to latest version change");
		}
		switch (scriptOptions.TargetDatabaseEngineType)
		{
		case SqlScriptOptions.ScriptDatabaseEngineType.SingleInstance:
			scriptingOptions.TargetDatabaseEngineType = DatabaseEngineType.Standalone;
			break;
		case SqlScriptOptions.ScriptDatabaseEngineType.SqlAzure:
			scriptingOptions.TargetDatabaseEngineType = DatabaseEngineType.SqlAzureDatabase;
			break;
		}
		switch (scriptOptions.TargetDatabaseEngineEdition)
		{
		case SqlScriptOptions.ScriptDatabaseEngineEdition.SqlServerPersonalEdition:
			scriptingOptions.TargetDatabaseEngineEdition = DatabaseEngineEdition.Personal;
			break;
		case SqlScriptOptions.ScriptDatabaseEngineEdition.SqlServerStandardEdition:
			scriptingOptions.TargetDatabaseEngineEdition = DatabaseEngineEdition.Standard;
			break;
		case SqlScriptOptions.ScriptDatabaseEngineEdition.SqlServerEnterpriseEdition:
			scriptingOptions.TargetDatabaseEngineEdition = DatabaseEngineEdition.Enterprise;
			break;
		case SqlScriptOptions.ScriptDatabaseEngineEdition.SqlServerExpressEdition:
			scriptingOptions.TargetDatabaseEngineEdition = DatabaseEngineEdition.Express;
			break;
		case SqlScriptOptions.ScriptDatabaseEngineEdition.SqlAzureDatabaseEdition:
			scriptingOptions.TargetDatabaseEngineEdition = DatabaseEngineEdition.SqlDatabase;
			break;
		case SqlScriptOptions.ScriptDatabaseEngineEdition.SqlDatawarehouseEdition:
			scriptingOptions.TargetDatabaseEngineEdition = DatabaseEngineEdition.SqlDataWarehouse;
			break;
		case SqlScriptOptions.ScriptDatabaseEngineEdition.SqlServerStretchEdition:
			scriptingOptions.TargetDatabaseEngineEdition = DatabaseEngineEdition.SqlStretchDatabase;
			break;
		case SqlScriptOptions.ScriptDatabaseEngineEdition.SqlServerManagedInstanceEdition:
			scriptingOptions.TargetDatabaseEngineEdition = DatabaseEngineEdition.SqlManagedInstance;
			break;
		case SqlScriptOptions.ScriptDatabaseEngineEdition.SqlServerOnDemandEdition:
			scriptingOptions.TargetDatabaseEngineEdition = DatabaseEngineEdition.SqlOnDemand;
			break;
		default:
			SqlScriptPublishModelTraceHelper.Assert(scriptingOptions.TargetDatabaseEngineEdition == DatabaseEngineEdition.Standard, "The default database engine edition is Standard.");
			scriptingOptions.TargetDatabaseEngineEdition = DatabaseEngineEdition.Standard;
			break;
		}
		scriptingOptions.SchemaQualify = SqlScriptOptions.ConvertBooleanTypeOptionToBoolean(scriptOptions.SchemaQualify);
		scriptingOptions.SchemaQualifyForeignKeysReferences = SqlScriptOptions.ConvertBooleanTypeOptionToBoolean(scriptOptions.SchemaQualify);
		scriptingOptions.IncludeIfNotExists = SqlScriptOptions.ConvertBooleanTypeOptionToBoolean(scriptOptions.IncludeIfNotExists);
		if (scriptingOptions.Indexes != SqlScriptOptions.ConvertBooleanTypeOptionToBoolean(scriptOptions.ScriptIndexes))
		{
			scriptingOptions.Indexes = SqlScriptOptions.ConvertBooleanTypeOptionToBoolean(scriptOptions.ScriptIndexes);
		}
		scriptingOptions.DriChecks = SqlScriptOptions.ConvertBooleanTypeOptionToBoolean(scriptOptions.ScriptCheckConstraints);
		scriptingOptions.DriForeignKeys = SqlScriptOptions.ConvertBooleanTypeOptionToBoolean(scriptOptions.ScriptForeignKeys);
		scriptingOptions.DriPrimaryKey = SqlScriptOptions.ConvertBooleanTypeOptionToBoolean(scriptOptions.ScriptPrimaryKeys);
		scriptingOptions.DriUniqueKeys = SqlScriptOptions.ConvertBooleanTypeOptionToBoolean(scriptOptions.UniqueKeys);
		scriptingOptions.Triggers = SqlScriptOptions.ConvertBooleanTypeOptionToBoolean(scriptOptions.ScriptTriggers);
		scriptingOptions.NoCollation = !SqlScriptOptions.ConvertBooleanTypeOptionToBoolean(scriptOptions.Collation);
		scriptingOptions.DriDefaults = SqlScriptOptions.ConvertBooleanTypeOptionToBoolean(scriptOptions.Default);
		scriptingOptions.IncludeDatabaseContext = SqlScriptOptions.ConvertBooleanTypeOptionToBoolean(scriptOptions.ScriptUseDatabase);
		scriptingOptions.ExtendedProperties = SqlScriptOptions.ConvertBooleanTypeOptionToBoolean(scriptOptions.ScriptExtendedProperties);
		scriptingOptions.FullTextIndexes = SqlScriptOptions.ConvertBooleanTypeOptionToBoolean(scriptOptions.ScriptFullTextIndexes);
		scriptingOptions.ConvertUserDefinedDataTypesToBaseType = SqlScriptOptions.ConvertBooleanTypeOptionToBoolean(scriptOptions.ConvertUDDTToBaseType);
		scriptingOptions.DriIncludeSystemNames = SqlScriptOptions.ConvertBooleanTypeOptionToBoolean(scriptOptions.ScriptDriIncludeSystemNames);
		scriptingOptions.AnsiPadding = SqlScriptOptions.ConvertBooleanTypeOptionToBoolean(scriptOptions.ScriptAnsiPadding);
		scriptingOptions.NoVardecimal = false;
		switch (scriptOptions.ScriptStatistics)
		{
		case SqlScriptOptions.ScriptStatisticsOptions.ScriptStatsAll:
			scriptingOptions.Statistics = true;
			scriptingOptions.OptimizerData = true;
			break;
		case SqlScriptOptions.ScriptStatisticsOptions.ScriptStatsDDL:
			scriptingOptions.Statistics = true;
			scriptingOptions.OptimizerData = false;
			break;
		case SqlScriptOptions.ScriptStatisticsOptions.ScriptStatsNone:
			scriptingOptions.Statistics = false;
			scriptingOptions.OptimizerData = false;
			break;
		}
		if (scriptingOptions.Statistics && scriptingOptions.OptimizerData)
		{
			scriptingOptions.DriIncludeSystemNames = true;
			scriptingOptions.AnsiPadding = true;
		}
		scriptingOptions.Permissions = SqlScriptOptions.ConvertBooleanTypeOptionToBoolean(scriptOptions.ScriptObjectLevelPermissions);
		scriptingOptions.ScriptOwner = SqlScriptOptions.ConvertBooleanTypeOptionToBoolean(scriptOptions.ScriptOwner);
	}
}
