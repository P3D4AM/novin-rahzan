using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Diagnostics;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.SqlScriptPublish;

[TypeConverter(typeof(LocalizableTypeConverter))]
[LocalizedPropertyResources("Microsoft.SqlServer.Management.SqlScriptPublish.SqlScriptOptionsSR")]
public class SqlScriptOptions : IDynamicVisible, IDynamicReadOnly
{
	public enum ScriptCompatibilityOptions
	{
		[DisplayNameKey("OnlyScript90CompatibleFeatures")]
		Script90Compat,
		[DisplayNameKey("OnlyScript100CompatibleFeatures")]
		Script100Compat,
		[DisplayNameKey("OnlyScript105CompatibleFeatures")]
		Script105Compat,
		[DisplayNameKey("OnlyScript110CompatibleFeatures")]
		Script110Compat,
		[DisplayNameKey("OnlyScript120CompatibleFeatures")]
		Script120Compat,
		[DisplayNameKey("OnlyScript130CompatibleFeatures")]
		Script130Compat,
		[DisplayNameKey("OnlyScript140CompatibleFeatures")]
		Script140Compat,
		[DisplayNameKey("OnlyScript150CompatibleFeatures")]
		Script150Compat,
		[DisplayNameKey("OnlyScript160CompatibleFeatures")]
		Script160Compat
	}

	public enum ScriptDatabaseEngineType
	{
		[DisplayNameKey("SingleInstanceEngineType")]
		SingleInstance,
		[DisplayNameKey("SqlAzureEngineType")]
		SqlAzure
	}

	public enum ScriptDatabaseEngineEdition
	{
		[DisplayNameKey("PersonalEdition")]
		SqlServerPersonalEdition,
		[DisplayNameKey("StandardEdition")]
		SqlServerStandardEdition,
		[DisplayNameKey("EnterpriseEdition")]
		SqlServerEnterpriseEdition,
		[DisplayNameKey("ExpressEdition")]
		SqlServerExpressEdition,
		[DisplayNameKey("SqlAzureDatabaseEdition")]
		SqlAzureDatabaseEdition,
		[DisplayNameKey("SqlDataWarehouseEdition")]
		SqlDatawarehouseEdition,
		[DisplayNameKey("StretchEdition")]
		SqlServerStretchEdition,
		[DisplayNameKey("SqlServerManagedInstanceEdition")]
		SqlServerManagedInstanceEdition,
		[DisplayNameKey("SqlServerOnDemandEdition")]
		SqlServerOnDemandEdition,
		[DisplayNameKey("SqlDatabaseEdgeEdition")]
		SqlDatabaseEdgeEdition
	}

	public enum ScriptStatisticsOptions
	{
		[DisplayNameKey("ScriptStatisticsNone")]
		ScriptStatsNone,
		[DisplayNameKey("ScriptStatisticsDDL")]
		ScriptStatsDDL,
		[DisplayNameKey("ScriptStatisticsAll")]
		ScriptStatsAll
	}

	public enum BooleanTypeOptions
	{
		[DisplayNameKey("TrueText")]
		True,
		[DisplayNameKey("FalseText")]
		False
	}

	public enum ScriptCreateDropOptions
	{
		[DisplayNameKey("ScriptCreate")]
		ScriptCreate,
		[DisplayNameKey("ScriptDrop")]
		ScriptDrop,
		[DisplayNameKey("ScriptCreateDrop")]
		ScriptCreateDrop
	}

	public enum TypeOfDataToScriptOptions
	{
		[DisplayNameKey("SchemaAndData")]
		SchemaAndData,
		[DisplayNameKey("DataOnly")]
		DataOnly,
		[DisplayNameKey("SchemaOnly")]
		SchemaOnly
	}

	private BooleanTypeOptions generateScriptForDependentObjects = BooleanTypeOptions.False;

	private BooleanTypeOptions includeDescriptiveHeaders = BooleanTypeOptions.False;

	private BooleanTypeOptions includeScriptingParametersHeader = BooleanTypeOptions.False;

	private BooleanTypeOptions includeVarDecimal;

	private BooleanTypeOptions scriptExtendedProperties;

	private BooleanTypeOptions scriptUseDatabase = BooleanTypeOptions.False;

	private BooleanTypeOptions scriptSQLLogins = BooleanTypeOptions.False;

	private BooleanTypeOptions scriptObjectLevelPermissions = BooleanTypeOptions.False;

	private BooleanTypeOptions scriptOwner = BooleanTypeOptions.False;

	private ScriptCreateDropOptions scriptCreateDrop;

	private ScriptCompatibilityOptions compatMode;

	private ScriptDatabaseEngineType engineType;

	private ScriptDatabaseEngineEdition databaseEngineEdition = ScriptDatabaseEngineEdition.SqlServerStandardEdition;

	private BooleanTypeOptions scriptIndexes;

	private BooleanTypeOptions scriptFullTextIndexes = BooleanTypeOptions.False;

	private BooleanTypeOptions scriptTriggers;

	private BooleanTypeOptions scriptPrimaryKeys;

	private BooleanTypeOptions scriptForeignKeys;

	private BooleanTypeOptions uniqueKeys;

	private BooleanTypeOptions scriptCheckConstraints;

	private BooleanTypeOptions schemaQualify;

	private BooleanTypeOptions includeIfNotExists;

	private BooleanTypeOptions collation = BooleanTypeOptions.False;

	private BooleanTypeOptions continueScriptingOnError = BooleanTypeOptions.False;

	private BooleanTypeOptions appendToFile = BooleanTypeOptions.False;

	private BooleanTypeOptions scriptDefaults;

	private BooleanTypeOptions convertUDDTToBaseType = BooleanTypeOptions.False;

	private BooleanTypeOptions scriptDriIncludeSystemNames = BooleanTypeOptions.False;

	private BooleanTypeOptions scriptAnsiPadding = BooleanTypeOptions.False;

	private BooleanTypeOptions scriptChangeTracking = BooleanTypeOptions.False;

	private BooleanTypeOptions scriptDataCompressionOptions;

	private BooleanTypeOptions bindings;

	private TypeOfDataToScriptOptions typeOfDataToScript = TypeOfDataToScriptOptions.SchemaOnly;

	private ScriptStatisticsOptions scriptStatistics;

	private int sqlServerVersion;

	private BooleanTypeOptions scriptUnsupportedStatments = BooleanTypeOptions.False;

	private Dictionary<string, object> readonlyProperties = new Dictionary<string, object>(8);

	private Dictionary<string, object> originalValues = new Dictionary<string, object>(8);

	[DisplayNameKey("ScriptConvertUDDT")]
	[DisplayDescriptionKey("Generateconvertuddt")]
	[DisplayCategoryKey("General")]
	public virtual BooleanTypeOptions ConvertUDDTToBaseType
	{
		get
		{
			return convertUDDTToBaseType;
		}
		set
		{
			convertUDDTToBaseType = value;
		}
	}

	[DisplayNameKey("ScriptUseDatabase")]
	[DisplayDescriptionKey("Generateusedatabase")]
	[DisplayCategoryKey("General")]
	public virtual BooleanTypeOptions ScriptUseDatabase
	{
		get
		{
			return scriptUseDatabase;
		}
		set
		{
			scriptUseDatabase = value;
		}
	}

	[DisplayNameKey("ScriptLogins")]
	[DisplayDescriptionKey("Generatescriptfordatabaselogins")]
	[DisplayCategoryKey("General")]
	public virtual BooleanTypeOptions ScriptLogins
	{
		get
		{
			return scriptSQLLogins;
		}
		set
		{
			scriptSQLLogins = value;
		}
	}

	[DisplayNameKey("ScriptCreateDrop")]
	[DisplayDescriptionKey("ScriptCreateDropDescription")]
	[DisplayCategoryKey("General")]
	public virtual ScriptCreateDropOptions ScriptCreateDrop
	{
		get
		{
			return scriptCreateDrop;
		}
		set
		{
			scriptCreateDrop = value;
		}
	}

	[DisplayNameKey("TypesOfDataToScript")]
	[DisplayDescriptionKey("TypesOfDataToScriptDesc")]
	[DisplayCategoryKey("General")]
	public virtual TypeOfDataToScriptOptions TypeOfDataToScript
	{
		get
		{
			return typeOfDataToScript;
		}
		set
		{
			typeOfDataToScript = value;
		}
	}

	[DisplayNameKey("ScriptObjectLevelPermissions")]
	[DisplayDescriptionKey("Generateobjectlevelpermissions")]
	[DisplayCategoryKey("General")]
	public virtual BooleanTypeOptions ScriptObjectLevelPermissions
	{
		get
		{
			return scriptObjectLevelPermissions;
		}
		set
		{
			scriptObjectLevelPermissions = value;
		}
	}

	[DisplayNameKey("ScriptOwner")]
	[DisplayDescriptionKey("ScriptOwnerForTheObjects")]
	[DisplayCategoryKey("General")]
	public virtual BooleanTypeOptions ScriptOwner
	{
		get
		{
			return scriptOwner;
		}
		set
		{
			scriptOwner = value;
		}
	}

	[DisplayNameKey("GenerateScriptForDependentObjects")]
	[DisplayDescriptionKey("Generatescriptfordependentobjectsofeachtablescripted")]
	[DisplayCategoryKey("General")]
	public virtual BooleanTypeOptions GenerateScriptForDependentObjects
	{
		get
		{
			return generateScriptForDependentObjects;
		}
		set
		{
			generateScriptForDependentObjects = value;
		}
	}

	[DisplayNameKey("IncludeDescriptiveHeaders")]
	[DisplayDescriptionKey("Includedescriptiveheadersforeachtablescripted")]
	[DisplayCategoryKey("General")]
	public virtual BooleanTypeOptions IncludeDescriptiveHeaders
	{
		get
		{
			return includeDescriptiveHeaders;
		}
		set
		{
			includeDescriptiveHeaders = value;
		}
	}

	[DisplayNameKey("IncludeScriptingParametersHeader")]
	[DisplayDescriptionKey("IncludeScriptingParametersHeaderDescription")]
	[DisplayCategoryKey("General")]
	public virtual BooleanTypeOptions IncludeScriptingParametersHeader
	{
		get
		{
			return includeScriptingParametersHeader;
		}
		set
		{
			includeScriptingParametersHeader = value;
		}
	}

	[DisplayNameKey("IncludeVarDecimal")]
	[DisplayDescriptionKey("IncludeVarDecimalDescription")]
	[DisplayCategoryKey("General")]
	[Browsable(false)]
	public virtual BooleanTypeOptions IncludeVarDecimal
	{
		get
		{
			return includeVarDecimal;
		}
		set
		{
			includeVarDecimal = value;
		}
	}

	[DisplayNameKey("Bindings")]
	[DisplayDescriptionKey("BindingsDescription")]
	[DisplayCategoryKey("General")]
	public virtual BooleanTypeOptions Bindings
	{
		get
		{
			return bindings;
		}
		set
		{
			bindings = value;
		}
	}

	[DisplayNameKey("ContinueScriptingOnError")]
	[DisplayDescriptionKey("Continuescriptifanerroroccuredorotherwisestop")]
	[DisplayCategoryKey("General")]
	public virtual BooleanTypeOptions ContinueScriptingOnError
	{
		get
		{
			return continueScriptingOnError;
		}
		set
		{
			continueScriptingOnError = value;
		}
	}

	[DisplayNameKey("AppendToFile")]
	[DisplayDescriptionKey("Appendtofilethegeneratedscript")]
	[DisplayCategoryKey("General")]
	public virtual BooleanTypeOptions AppendToFile
	{
		get
		{
			return appendToFile;
		}
		set
		{
			appendToFile = value;
		}
	}

	[DisplayNameKey("ScriptExtendedProperties")]
	[DisplayDescriptionKey("Scriptextendedpropertiesforeachtablescripted")]
	[DisplayCategoryKey("General")]
	public virtual BooleanTypeOptions ScriptExtendedProperties
	{
		get
		{
			return scriptExtendedProperties;
		}
		set
		{
			scriptExtendedProperties = value;
		}
	}

	[DisplayNameKey("ScriptStatistics")]
	[DisplayDescriptionKey("generatescriptstatistics")]
	[DisplayCategoryKey("General")]
	public virtual ScriptStatisticsOptions ScriptStatistics
	{
		get
		{
			return scriptStatistics;
		}
		set
		{
			scriptStatistics = value;
		}
	}

	[DisplayNameKey("ScriptDriIncludeSystemNames")]
	[DisplayDescriptionKey("generateDriIncludeSystemNames")]
	[DisplayCategoryKey("General")]
	public virtual BooleanTypeOptions ScriptDriIncludeSystemNames
	{
		get
		{
			return scriptDriIncludeSystemNames;
		}
		set
		{
			scriptDriIncludeSystemNames = value;
		}
	}

	[DisplayNameKey("ScriptAnsiPadding")]
	[DisplayDescriptionKey("generateAnsiPadding")]
	[DisplayCategoryKey("General")]
	public virtual BooleanTypeOptions ScriptAnsiPadding
	{
		get
		{
			return scriptAnsiPadding;
		}
		set
		{
			scriptAnsiPadding = value;
		}
	}

	[DisplayNameKey("SchemaQualify")]
	[DisplayDescriptionKey("Prefixschemaforobjects")]
	[DisplayCategoryKey("General")]
	public virtual BooleanTypeOptions SchemaQualify
	{
		get
		{
			return schemaQualify;
		}
		set
		{
			schemaQualify = value;
		}
	}

	[DisplayNameKey("IncludeIfNotExists")]
	[DisplayDescriptionKey("Scriptobjectswithifnotexistoption")]
	[DisplayCategoryKey("General")]
	public virtual BooleanTypeOptions IncludeIfNotExists
	{
		get
		{
			return includeIfNotExists;
		}
		set
		{
			includeIfNotExists = value;
		}
	}

	[DisplayNameKey("GenerateScriptCollation")]
	[DisplayDescriptionKey("Scriptobjectswithcollation")]
	[DisplayCategoryKey("General")]
	public virtual BooleanTypeOptions Collation
	{
		get
		{
			return collation;
		}
		set
		{
			collation = value;
		}
	}

	[DisplayNameKey("Default")]
	[DisplayDescriptionKey("ScriptDefaults")]
	[DisplayCategoryKey("General")]
	public virtual BooleanTypeOptions Default
	{
		get
		{
			return scriptDefaults;
		}
		set
		{
			scriptDefaults = value;
		}
	}

	[DisplayNameKey("CompatibleFeatures")]
	[DisplayDescriptionKey("CompatibleFeaturesDescription")]
	[DisplayCategoryKey("General")]
	public virtual ScriptCompatibilityOptions ScriptCompatibilityOption
	{
		get
		{
			return compatMode;
		}
		set
		{
			compatMode = value;
		}
	}

	[DisplayNameKey("TargetEngineType")]
	[DisplayDescriptionKey("TargetEngineTypeDescription")]
	[DisplayCategoryKey("General")]
	public virtual ScriptDatabaseEngineType TargetDatabaseEngineType
	{
		get
		{
			return engineType;
		}
		set
		{
			if (engineType != value)
			{
				ScriptDatabaseEngineType scriptDatabaseEngineType = engineType;
				engineType = value;
				if (engineType == ScriptDatabaseEngineType.SingleInstance)
				{
					TargetDatabaseEngineEdition = ScriptDatabaseEngineEdition.SqlServerStandardEdition;
				}
				else
				{
					SqlScriptPublishModelTraceHelper.Assert(engineType == ScriptDatabaseEngineType.SqlAzure, "Unexpected database engine type detected, expecting SQL Azure.");
					TargetDatabaseEngineEdition = ScriptDatabaseEngineEdition.SqlAzureDatabaseEdition;
				}
				if (this.ReadOnlyPropertyChanged != null && (value == ScriptDatabaseEngineType.SqlAzure || scriptDatabaseEngineType == ScriptDatabaseEngineType.SqlAzure))
				{
					this.ReadOnlyPropertyChanged(this, new ReadOnlyPropertyChangedEventArgs("TargetDatabaseEngineType"));
				}
			}
		}
	}

	[DisplayNameKey("TargetEngineEdition")]
	[DisplayDescriptionKey("TargetEngineEditionDescription")]
	[DisplayCategoryKey("General")]
	public virtual ScriptDatabaseEngineEdition TargetDatabaseEngineEdition
	{
		get
		{
			return databaseEngineEdition;
		}
		set
		{
			if (databaseEngineEdition == value)
			{
				return;
			}
			_ = databaseEngineEdition;
			switch (value)
			{
			case ScriptDatabaseEngineEdition.SqlAzureDatabaseEdition:
			case ScriptDatabaseEngineEdition.SqlDatawarehouseEdition:
				if (TargetDatabaseEngineType != ScriptDatabaseEngineType.SqlAzure)
				{
					throw new ArgumentException(SR.ERROR_UnsupportedDatabaseEngineEditionAndDatabaseEngineTypeOptionCombination(value.ToString(), TargetDatabaseEngineType.ToString()));
				}
				break;
			case ScriptDatabaseEngineEdition.SqlServerPersonalEdition:
			case ScriptDatabaseEngineEdition.SqlServerStandardEdition:
			case ScriptDatabaseEngineEdition.SqlServerEnterpriseEdition:
			case ScriptDatabaseEngineEdition.SqlServerExpressEdition:
			case ScriptDatabaseEngineEdition.SqlServerStretchEdition:
			case ScriptDatabaseEngineEdition.SqlServerManagedInstanceEdition:
			case ScriptDatabaseEngineEdition.SqlServerOnDemandEdition:
				if (TargetDatabaseEngineType != 0)
				{
					throw new ArgumentException(SR.ERROR_UnsupportedDatabaseEngineEditionAndDatabaseEngineTypeOptionCombination(value.ToString(), TargetDatabaseEngineType.ToString()));
				}
				break;
			}
			databaseEngineEdition = value;
			if (this.ReadOnlyPropertyChanged != null)
			{
				this.ReadOnlyPropertyChanged(this, new ReadOnlyPropertyChangedEventArgs("TargetDatabaseEngineEdition"));
			}
		}
	}

	[DisplayNameKey("IncludeUnsupportedStatements")]
	[DisplayDescriptionKey("IncludeUnsupportedStatementsDescription")]
	[DisplayCategoryKey("General")]
	public virtual BooleanTypeOptions IncludeUnsupportedStatements
	{
		get
		{
			return scriptUnsupportedStatments;
		}
		set
		{
			scriptUnsupportedStatments = value;
		}
	}

	[DisplayNameKey("ScriptIndexes")]
	[DisplayDescriptionKey("Scriptindexesforeachtablescripted")]
	[DisplayCategoryKey("TableView")]
	public virtual BooleanTypeOptions ScriptIndexes
	{
		get
		{
			return scriptIndexes;
		}
		set
		{
			scriptIndexes = value;
		}
	}

	[DisplayNameKey("ScriptFullTextIndexes")]
	[DisplayDescriptionKey("Scriptfulltextindexesforeachtablescripted")]
	[DisplayCategoryKey("TableView")]
	public virtual BooleanTypeOptions ScriptFullTextIndexes
	{
		get
		{
			return scriptFullTextIndexes;
		}
		set
		{
			scriptFullTextIndexes = value;
		}
	}

	[DisplayNameKey("ScriptTriggers")]
	[DisplayDescriptionKey("Scripttriggersforeachtablescripted")]
	[DisplayCategoryKey("TableView")]
	public virtual BooleanTypeOptions ScriptTriggers
	{
		get
		{
			return scriptTriggers;
		}
		set
		{
			scriptTriggers = value;
		}
	}

	[DisplayNameKey("ScriptPrimaryKeys")]
	[DisplayDescriptionKey("Scriptprimarykeysforeachtablescripted")]
	[DisplayCategoryKey("TableView")]
	public virtual BooleanTypeOptions ScriptPrimaryKeys
	{
		get
		{
			return scriptPrimaryKeys;
		}
		set
		{
			scriptPrimaryKeys = value;
		}
	}

	[DisplayNameKey("ScriptUniqueKeys")]
	[DisplayDescriptionKey("Scriptuniqueskeysforeachtablescripted")]
	[DisplayCategoryKey("TableView")]
	public virtual BooleanTypeOptions UniqueKeys
	{
		get
		{
			return uniqueKeys;
		}
		set
		{
			uniqueKeys = value;
		}
	}

	[DisplayNameKey("ScriptForeignKeys")]
	[DisplayDescriptionKey("Scriptforeignkeysforeachtablescripted")]
	[DisplayCategoryKey("TableView")]
	public virtual BooleanTypeOptions ScriptForeignKeys
	{
		get
		{
			return scriptForeignKeys;
		}
		set
		{
			scriptForeignKeys = value;
		}
	}

	[DisplayNameKey("ScriptChangeTracking")]
	[DisplayDescriptionKey("ScriptChangeTrackingInformation")]
	[DisplayCategoryKey("TableView")]
	public BooleanTypeOptions ScriptChangeTracking
	{
		get
		{
			return scriptChangeTracking;
		}
		set
		{
			scriptChangeTracking = value;
		}
	}

	[DisplayNameKey("ScriptDataCompression")]
	[DisplayDescriptionKey("ScriptDataCompressionInformation")]
	[DisplayCategoryKey("TableView")]
	public BooleanTypeOptions ScriptDataCompressionOptions
	{
		get
		{
			return scriptDataCompressionOptions;
		}
		set
		{
			scriptDataCompressionOptions = value;
		}
	}

	[DisplayNameKey("ScriptCheckConstraints")]
	[DisplayDescriptionKey("Scriptcheckconstraintsforeachtablescripted")]
	[DisplayCategoryKey("TableView")]
	public virtual BooleanTypeOptions ScriptCheckConstraints
	{
		get
		{
			return scriptCheckConstraints;
		}
		set
		{
			scriptCheckConstraints = value;
		}
	}

	public event EventHandler<ReadOnlyPropertyChangedEventArgs> ReadOnlyPropertyChanged;

	public SqlScriptOptions(Version version)
	{
		sqlServerVersion = version.Major;
		if (sqlServerVersion < 8)
		{
			throw new ArgumentOutOfRangeException("version");
		}
		if (sqlServerVersion == 9)
		{
			compatMode = ScriptCompatibilityOptions.Script90Compat;
		}
		else if (sqlServerVersion == 10)
		{
			compatMode = ((version.Minor == 0) ? ScriptCompatibilityOptions.Script100Compat : ScriptCompatibilityOptions.Script105Compat);
		}
		else if (sqlServerVersion == 11)
		{
			compatMode = ScriptCompatibilityOptions.Script110Compat;
		}
		else if (sqlServerVersion == 12)
		{
			compatMode = ScriptCompatibilityOptions.Script120Compat;
		}
		else if (sqlServerVersion == 13)
		{
			compatMode = ScriptCompatibilityOptions.Script130Compat;
		}
		else if (sqlServerVersion == 14)
		{
			compatMode = ScriptCompatibilityOptions.Script140Compat;
		}
		else if (sqlServerVersion == 15)
		{
			compatMode = ScriptCompatibilityOptions.Script150Compat;
		}
		else
		{
			SqlScriptPublishModelTraceHelper.Assert(condition: false, "Unexpected server version. Setting Compatability Mode to 15.0!");
			compatMode = ScriptCompatibilityOptions.Script150Compat;
		}
		readonlyProperties.Add("ScriptUseDatabase", BooleanTypeOptions.False);
	}

	public void Copy(SqlScriptOptions source)
	{
		if (source != null)
		{
			appendToFile = source.appendToFile;
			bindings = source.bindings;
			collation = source.collation;
			compatMode = source.compatMode;
			engineType = source.engineType;
			databaseEngineEdition = source.databaseEngineEdition;
			continueScriptingOnError = source.continueScriptingOnError;
			convertUDDTToBaseType = source.convertUDDTToBaseType;
			generateScriptForDependentObjects = source.generateScriptForDependentObjects;
			includeDescriptiveHeaders = source.includeDescriptiveHeaders;
			includeScriptingParametersHeader = source.includeScriptingParametersHeader;
			includeIfNotExists = source.includeIfNotExists;
			includeVarDecimal = source.includeVarDecimal;
			schemaQualify = source.schemaQualify;
			scriptAnsiPadding = source.scriptAnsiPadding;
			scriptChangeTracking = source.scriptChangeTracking;
			scriptCheckConstraints = source.scriptCheckConstraints;
			scriptCreateDrop = source.scriptCreateDrop;
			typeOfDataToScript = source.typeOfDataToScript;
			scriptDataCompressionOptions = source.scriptDataCompressionOptions;
			scriptDefaults = source.scriptDefaults;
			scriptDriIncludeSystemNames = source.scriptDriIncludeSystemNames;
			scriptExtendedProperties = source.scriptExtendedProperties;
			scriptForeignKeys = source.scriptForeignKeys;
			scriptFullTextIndexes = source.scriptFullTextIndexes;
			scriptIndexes = source.scriptIndexes;
			scriptObjectLevelPermissions = source.scriptObjectLevelPermissions;
			scriptOwner = source.scriptOwner;
			scriptPrimaryKeys = source.scriptPrimaryKeys;
			scriptSQLLogins = source.scriptSQLLogins;
			scriptStatistics = source.scriptStatistics;
			scriptTriggers = source.scriptTriggers;
			scriptUseDatabase = source.scriptUseDatabase;
			sqlServerVersion = source.sqlServerVersion;
			uniqueKeys = source.uniqueKeys;
		}
	}

	public ICollection ConfigureVisibleEnumFields(ITypeDescriptorContext context, ArrayList values)
	{
		if (context.PropertyDescriptor.PropertyType == typeof(ScriptDatabaseEngineEdition))
		{
			if (TargetDatabaseEngineType == ScriptDatabaseEngineType.SingleInstance)
			{
				values.Remove(ScriptDatabaseEngineEdition.SqlAzureDatabaseEdition);
				values.Remove(ScriptDatabaseEngineEdition.SqlDatawarehouseEdition);
				values.Remove(ScriptDatabaseEngineEdition.SqlServerOnDemandEdition);
			}
			else
			{
				if (TargetDatabaseEngineType != ScriptDatabaseEngineType.SqlAzure)
				{
					throw new ArgumentException(SR.ERROR_UnexpectedDatabaseEngineTypeDetected(TargetDatabaseEngineType.ToString()));
				}
				values.Remove(ScriptDatabaseEngineEdition.SqlServerPersonalEdition);
				values.Remove(ScriptDatabaseEngineEdition.SqlServerStandardEdition);
				values.Remove(ScriptDatabaseEngineEdition.SqlServerEnterpriseEdition);
				values.Remove(ScriptDatabaseEngineEdition.SqlServerExpressEdition);
				values.Remove(ScriptDatabaseEngineEdition.SqlServerStretchEdition);
				values.Remove(ScriptDatabaseEngineEdition.SqlServerManagedInstanceEdition);
				values.Remove(ScriptDatabaseEngineEdition.SqlDatabaseEdgeEdition);
			}
		}
		return values;
	}

	public virtual void LoadShellScriptingOptions(IScriptPublishOptions scriptingOptions, SqlSmoObject smoObject)
	{
		generateScriptForDependentObjects = ConvertBooleanToBooleanTypeOption(scriptingOptions.ScriptDependentObjects);
		includeDescriptiveHeaders = ConvertBooleanToBooleanTypeOption(scriptingOptions.IncludeHeaders);
		includeScriptingParametersHeader = ConvertBooleanToBooleanTypeOption(scriptingOptions.IncludeScriptingParametersHeader);
		bindings = ConvertBooleanToBooleanTypeOption(scriptingOptions.ScriptBinding);
		includeVarDecimal = ConvertBooleanToBooleanTypeOption(scriptingOptions.IncludeVarDecimal);
		scriptExtendedProperties = ConvertBooleanToBooleanTypeOption(scriptingOptions.ScriptExtendedProperties);
		scriptUseDatabase = ConvertBooleanToBooleanTypeOption(scriptingOptions.ScriptUseDatabase);
		scriptObjectLevelPermissions = ConvertBooleanToBooleanTypeOption(scriptingOptions.ScriptPermissions);
		scriptOwner = ConvertBooleanToBooleanTypeOption(scriptingOptions.ScriptOwner);
		scriptIndexes = ConvertBooleanToBooleanTypeOption(scriptingOptions.ScriptIndexes);
		scriptFullTextIndexes = ConvertBooleanToBooleanTypeOption(scriptingOptions.ScriptFullTextIndexes);
		scriptTriggers = ConvertBooleanToBooleanTypeOption(scriptingOptions.ScriptTriggers);
		scriptPrimaryKeys = ConvertBooleanToBooleanTypeOption(scriptingOptions.ScriptPrimaryKeys);
		scriptForeignKeys = ConvertBooleanToBooleanTypeOption(scriptingOptions.ScriptForeignKeys);
		uniqueKeys = ConvertBooleanToBooleanTypeOption(scriptingOptions.ScriptUniqueKeys);
		scriptCheckConstraints = ConvertBooleanToBooleanTypeOption(scriptingOptions.ScriptCheckConstraints);
		schemaQualify = ConvertBooleanToBooleanTypeOption(scriptingOptions.SchemaQualify);
		includeIfNotExists = ConvertBooleanToBooleanTypeOption(scriptingOptions.IncludeIfNotExists);
		collation = ConvertBooleanToBooleanTypeOption(scriptingOptions.IncludeCollation);
		scriptDefaults = ConvertBooleanToBooleanTypeOption(scriptingOptions.ScriptDefaults);
		convertUDDTToBaseType = ConvertBooleanToBooleanTypeOption(scriptingOptions.ConvertUddtToBaseType);
		scriptDriIncludeSystemNames = ConvertBooleanToBooleanTypeOption(scriptingOptions.ScriptDriIncludeSystemNames);
		scriptAnsiPadding = ConvertBooleanToBooleanTypeOption(scriptingOptions.GenerateAnsiPadding);
		if (scriptingOptions.ScriptStatistics)
		{
			scriptStatistics = ScriptStatisticsOptions.ScriptStatsDDL;
		}
		scriptChangeTracking = ConvertBooleanToBooleanTypeOption(scriptingOptions.ScriptChangeTracking);
		scriptDataCompressionOptions = ConvertBooleanToBooleanTypeOption(scriptingOptions.ScriptDataCompressionOptions);
		LoadDestinationOptions(scriptingOptions, smoObject);
	}

	private void LoadDestinationOptions(IScriptPublishOptions scriptingOptions, SqlSmoObject smoObject)
	{
		ScriptingOptions scriptingOptions2 = ((smoObject == null) ? scriptingOptions.GetSmoScriptingOptions() : scriptingOptions.GetSmoScriptingOptions(smoObject));
		switch (scriptingOptions2.TargetServerVersion)
		{
		case SqlServerVersion.Version90:
			compatMode = ScriptCompatibilityOptions.Script90Compat;
			break;
		case SqlServerVersion.Version100:
		case SqlServerVersion.Version105:
			compatMode = ScriptCompatibilityOptions.Script100Compat;
			break;
		case SqlServerVersion.Version110:
			compatMode = ScriptCompatibilityOptions.Script110Compat;
			break;
		case SqlServerVersion.Version120:
			compatMode = ScriptCompatibilityOptions.Script120Compat;
			break;
		case SqlServerVersion.Version130:
			compatMode = ScriptCompatibilityOptions.Script130Compat;
			break;
		case SqlServerVersion.Version140:
			compatMode = ScriptCompatibilityOptions.Script140Compat;
			break;
		default:
			compatMode = ScriptCompatibilityOptions.Script150Compat;
			break;
		}
		TargetDatabaseEngineType = ScriptDatabaseEngineType.SingleInstance;
		switch (scriptingOptions2.TargetDatabaseEngineEdition)
		{
		case DatabaseEngineEdition.Enterprise:
			TargetDatabaseEngineEdition = ScriptDatabaseEngineEdition.SqlServerEnterpriseEdition;
			break;
		case DatabaseEngineEdition.Express:
			TargetDatabaseEngineEdition = ScriptDatabaseEngineEdition.SqlServerExpressEdition;
			break;
		case DatabaseEngineEdition.Personal:
			TargetDatabaseEngineEdition = ScriptDatabaseEngineEdition.SqlServerPersonalEdition;
			break;
		case DatabaseEngineEdition.SqlDatabase:
			TargetDatabaseEngineType = ScriptDatabaseEngineType.SqlAzure;
			TargetDatabaseEngineEdition = ScriptDatabaseEngineEdition.SqlAzureDatabaseEdition;
			break;
		case DatabaseEngineEdition.SqlDataWarehouse:
			TargetDatabaseEngineType = ScriptDatabaseEngineType.SqlAzure;
			TargetDatabaseEngineEdition = ScriptDatabaseEngineEdition.SqlDatawarehouseEdition;
			break;
		case DatabaseEngineEdition.SqlStretchDatabase:
			TargetDatabaseEngineType = ScriptDatabaseEngineType.SqlAzure;
			TargetDatabaseEngineEdition = ScriptDatabaseEngineEdition.SqlServerStretchEdition;
			break;
		case DatabaseEngineEdition.Standard:
			TargetDatabaseEngineEdition = ScriptDatabaseEngineEdition.SqlServerStandardEdition;
			break;
		case DatabaseEngineEdition.SqlManagedInstance:
			TargetDatabaseEngineEdition = ScriptDatabaseEngineEdition.SqlServerManagedInstanceEdition;
			break;
		case DatabaseEngineEdition.SqlOnDemand:
			TargetDatabaseEngineEdition = ScriptDatabaseEngineEdition.SqlServerOnDemandEdition;
			break;
		case DatabaseEngineEdition.SqlDatabaseEdge:
			TargetDatabaseEngineEdition = ScriptDatabaseEngineEdition.SqlDatabaseEdgeEdition;
			break;
		default:
			throw new ArgumentException(SR.ERROR_UnexpectedDatabaseEngineEditionDetected(scriptingOptions.TargetDatabaseEngineEdition));
		}
	}

	internal static bool ConvertBooleanTypeOptionToBoolean(BooleanTypeOptions option)
	{
		if (option == BooleanTypeOptions.True)
		{
			return true;
		}
		return false;
	}

	internal static BooleanTypeOptions ConvertBooleanToBooleanTypeOption(bool boolValue)
	{
		if (boolValue)
		{
			return BooleanTypeOptions.True;
		}
		return BooleanTypeOptions.False;
	}

	void IDynamicReadOnly.OverrideReadOnly(IList<LocalizablePropertyDescriptor> properties, ITypeDescriptorContext context, object value, Attribute[] attributes)
	{
		if (engineType == ScriptDatabaseEngineType.SqlAzure)
		{
			foreach (LocalizablePropertyDescriptor property in properties)
			{
				object value2 = null;
				if (readonlyProperties.TryGetValue(property.Name, out value2))
				{
					property.ForceReadOnly();
					object value3 = property.GetValue(this);
					if (value2 != value3 && !originalValues.ContainsKey(property.Name))
					{
						originalValues.Add(property.Name, value3);
					}
					property.SetValue(this, value2);
				}
			}
			return;
		}
		if (originalValues.Count <= 0)
		{
			return;
		}
		foreach (LocalizablePropertyDescriptor property2 in properties)
		{
			object value4 = null;
			if (originalValues.TryGetValue(property2.Name, out value4))
			{
				property2.SetValue(this, value4);
			}
		}
		originalValues.Clear();
	}
}
