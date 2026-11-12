using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.SqlServer.Management.SqlScriptPublish;

[CompilerGenerated]
internal class SqlScriptOptionsSR
{
	[CompilerGenerated]
	public class Keys
	{
		private static ResourceManager resourceManager = new ResourceManager(typeof(SqlScriptOptionsSR));

		private static CultureInfo _culture = null;

		public const string General = "General";

		public const string TableView = "TableView";

		public const string OptionsLabel = "OptionsLabel";

		public const string PublishingOptions = "PublishingOptions";

		public const string TrueText = "TrueText";

		public const string FalseText = "FalseText";

		public const string ScriptUseDatabase = "ScriptUseDatabase";

		public const string Generateusedatabase = "Generateusedatabase";

		public const string ScriptLogins = "ScriptLogins";

		public const string Generatescriptfordatabaselogins = "Generatescriptfordatabaselogins";

		public const string ScriptObjectLevelPermissions = "ScriptObjectLevelPermissions";

		public const string Generateobjectlevelpermissions = "Generateobjectlevelpermissions";

		public const string PublishObjectLevelPermissions = "PublishObjectLevelPermissions";

		public const string PublishDObjectLevelPermissions = "PublishDObjectLevelPermissions";

		public const string ScriptCreate = "ScriptCreate";

		public const string ScriptDrop = "ScriptDrop";

		public const string ScriptCreateDrop = "ScriptCreateDrop";

		public const string ScriptCreateDropDescription = "ScriptCreateDropDescription";

		public const string TypesOfDataToScriptDesc = "TypesOfDataToScriptDesc";

		public const string TypesOfDataToScript = "TypesOfDataToScript";

		public const string DataOnly = "DataOnly";

		public const string SchemaOnly = "SchemaOnly";

		public const string SchemaAndData = "SchemaAndData";

		public const string CreateMode = "CreateMode";

		public const string DropMode = "DropMode";

		public const string GenerateScriptForDependentObjects = "GenerateScriptForDependentObjects";

		public const string Generatescriptfordependentobjectsofeachtablescripted = "Generatescriptfordependentobjectsofeachtablescripted";

		public const string PublishForDependentObjects = "PublishForDependentObjects";

		public const string Publishfordependentobjectsofeachtablescripted = "Publishfordependentobjectsofeachtablescripted";

		public const string IncludeDescriptiveHeaders = "IncludeDescriptiveHeaders";

		public const string Includedescriptiveheadersforeachtablescripted = "Includedescriptiveheadersforeachtablescripted";

		public const string IncludeScriptingParametersHeader = "IncludeScriptingParametersHeader";

		public const string IncludeScriptingParametersHeaderDescription = "IncludeScriptingParametersHeaderDescription";

		public const string ContinueScriptingOnError = "ContinueScriptingOnError";

		public const string Continuescriptifanerroroccuredorotherwisestop = "Continuescriptifanerroroccuredorotherwisestop";

		public const string AppendToFile = "AppendToFile";

		public const string Appendtofilethegeneratedscript = "Appendtofilethegeneratedscript";

		public const string ScriptExtendedProperties = "ScriptExtendedProperties";

		public const string Scriptextendedpropertiesforeachtablescripted = "Scriptextendedpropertiesforeachtablescripted";

		public const string PublishExtendedProperties = "PublishExtendedProperties";

		public const string Publishextendedpropertiesforeachtablescripted = "Publishextendedpropertiesforeachtablescripted";

		public const string ScriptOwnerForTheObjects = "ScriptOwnerForTheObjects";

		public const string ScriptOwner = "ScriptOwner";

		public const string Prefixschemaforobjects = "Prefixschemaforobjects";

		public const string SchemaQualify = "SchemaQualify";

		public const string IncludeIfNotExists = "IncludeIfNotExists";

		public const string Scriptobjectswithifnotexistoption = "Scriptobjectswithifnotexistoption";

		public const string GenerateScriptCollation = "GenerateScriptCollation";

		public const string Scriptobjectswithcollation = "Scriptobjectswithcollation";

		public const string PublishCollation = "PublishCollation";

		public const string Publishobjectswithcollation = "Publishobjectswithcollation";

		public const string Default = "Default";

		public const string ScriptChangeTracking = "ScriptChangeTracking";

		public const string ScriptChangeTrackingInformation = "ScriptChangeTrackingInformation";

		public const string ScriptDefaults = "ScriptDefaults";

		public const string ScriptDataCompression = "ScriptDataCompression";

		public const string ScriptDataCompressionInformation = "ScriptDataCompressionInformation";

		public const string PublishDefault = "PublishDefault";

		public const string PublishDefaults = "PublishDefaults";

		public const string Onlyscript70compatiblefeaturesforeachtablescripted = "Onlyscript70compatiblefeaturesforeachtablescripted";

		public const string ScriptIndexes = "ScriptIndexes";

		public const string Scriptindexesforeachtablescripted = "Scriptindexesforeachtablescripted";

		public const string PublishIndexes = "PublishIndexes";

		public const string Publishindexesforeachtablescripted = "Publishindexesforeachtablescripted";

		public const string ScriptFullTextIndexes = "ScriptFullTextIndexes";

		public const string Scriptfulltextindexesforeachtablescripted = "Scriptfulltextindexesforeachtablescripted";

		public const string PublishFullTextIndexes = "PublishFullTextIndexes";

		public const string Publishfulltextindexesforeachtablescripted = "Publishfulltextindexesforeachtablescripted";

		public const string ScriptTriggers = "ScriptTriggers";

		public const string Scripttriggersforeachtablescripted = "Scripttriggersforeachtablescripted";

		public const string PublishTriggers = "PublishTriggers";

		public const string Publishtriggersforeachtablescripted = "Publishtriggersforeachtablescripted";

		public const string ScriptPrimaryKeys = "ScriptPrimaryKeys";

		public const string Scriptprimarykeysforeachtablescripted = "Scriptprimarykeysforeachtablescripted";

		public const string PublishPrimaryKeys = "PublishPrimaryKeys";

		public const string Publishprimarykeysforeachtablescripted = "Publishprimarykeysforeachtablescripted";

		public const string ScriptUniqueKeys = "ScriptUniqueKeys";

		public const string Scriptuniqueskeysforeachtablescripted = "Scriptuniqueskeysforeachtablescripted";

		public const string PublishUniqueKeys = "PublishUniqueKeys";

		public const string Publishuniqueskeysforeachtablescripted = "Publishuniqueskeysforeachtablescripted";

		public const string ScriptForeignKeys = "ScriptForeignKeys";

		public const string Scriptforeignkeysforeachtablescripted = "Scriptforeignkeysforeachtablescripted";

		public const string PublishForeignKeys = "PublishForeignKeys";

		public const string Publishforeignkeysforeachtablescripted = "Publishforeignkeysforeachtablescripted";

		public const string ScriptCheckConstraints = "ScriptCheckConstraints";

		public const string Scriptcheckconstraintsforeachtablescripted = "Scriptcheckconstraintsforeachtablescripted";

		public const string PublishCheckConstraints = "PublishCheckConstraints";

		public const string Publishcheckconstraintsforeachtablescripted = "Publishcheckconstraintsforeachtablescripted";

		public const string ScriptConvertUDDT = "ScriptConvertUDDT";

		public const string Generateconvertuddt = "Generateconvertuddt";

		public const string ScriptStatistics = "ScriptStatistics";

		public const string generatescriptstatistics = "generatescriptstatistics";

		public const string ScriptStatisticsNone = "ScriptStatisticsNone";

		public const string ScriptStatisticsDDL = "ScriptStatisticsDDL";

		public const string ScriptStatisticsAll = "ScriptStatisticsAll";

		public const string PublishStatistics = "PublishStatistics";

		public const string Publishgeneratescriptstatistics = "Publishgeneratescriptstatistics";

		public const string PublishStatisticsNone = "PublishStatisticsNone";

		public const string PublishStatisticsDDL = "PublishStatisticsDDL";

		public const string PublishStatisticsAll = "PublishStatisticsAll";

		public const string IncludeVarDecimal = "IncludeVarDecimal";

		public const string IncludeVarDecimalDescription = "IncludeVarDecimalDescription";

		public const string Bindings = "Bindings";

		public const string BindingsDescription = "BindingsDescription";

		public const string PublishIncludeVarDecimal = "PublishIncludeVarDecimal";

		public const string PublishIncludeVarDecimalDescription = "PublishIncludeVarDecimalDescription";

		public const string ScriptDriIncludeSystemNames = "ScriptDriIncludeSystemNames";

		public const string generateDriIncludeSystemNames = "generateDriIncludeSystemNames";

		public const string ScriptAnsiPadding = "ScriptAnsiPadding";

		public const string generateAnsiPadding = "generateAnsiPadding";

		public const string ScriptDatabaseCreate = "ScriptDatabaseCreate";

		public const string genreatescriptdatabasecreate = "genreatescriptdatabasecreate";

		public const string UseTransactionForPublish = "UseTransactionForPublish";

		public const string UseTransactionForPublishDesc = "UseTransactionForPublishDesc";

		public const string GenerateCreateCommand = "GenerateCreateCommand";

		public const string GenerateDropCommand = "GenerateDropCommand";

		public const string CompatibleFeatures = "CompatibleFeatures";

		public const string CompatibleFeaturesDescription = "CompatibleFeaturesDescription";

		public const string PublishCompatibleFeatures = "PublishCompatibleFeatures";

		public const string PublishCompatibleFeaturesDescription = "PublishCompatibleFeaturesDescription";

		public const string OnlyScript90CompatibleFeatures = "OnlyScript90CompatibleFeatures";

		public const string OnlyScript100CompatibleFeatures = "OnlyScript100CompatibleFeatures";

		public const string OnlyScript105CompatibleFeatures = "OnlyScript105CompatibleFeatures";

		public const string OnlyScript110CompatibleFeatures = "OnlyScript110CompatibleFeatures";

		public const string OnlyScript120CompatibleFeatures = "OnlyScript120CompatibleFeatures";

		public const string OnlyScript130CompatibleFeatures = "OnlyScript130CompatibleFeatures";

		public const string OnlyScript140CompatibleFeatures = "OnlyScript140CompatibleFeatures";

		public const string OnlyScript150CompatibleFeatures = "OnlyScript150CompatibleFeatures";

		public const string OnlyScript160CompatibleFeatures = "OnlyScript160CompatibleFeatures";

		public const string TargetEngineType = "TargetEngineType";

		public const string TargetEngineTypeDescription = "TargetEngineTypeDescription";

		public const string TargetEngineEdition = "TargetEngineEdition";

		public const string TargetEngineEditionDescription = "TargetEngineEditionDescription";

		public const string SingleInstanceEngineType = "SingleInstanceEngineType";

		public const string SqlAzureEngineType = "SqlAzureEngineType";

		public const string PersonalEdition = "PersonalEdition";

		public const string StandardEdition = "StandardEdition";

		public const string EnterpriseEdition = "EnterpriseEdition";

		public const string ExpressEdition = "ExpressEdition";

		public const string SqlAzureDatabaseEdition = "SqlAzureDatabaseEdition";

		public const string SqlDataWarehouseEdition = "SqlDataWarehouseEdition";

		public const string StretchEdition = "StretchEdition";

		public const string SqlServerManagedInstanceEdition = "SqlServerManagedInstanceEdition";

		public const string SqlServerOnDemandEdition = "SqlServerOnDemandEdition";

		public const string SqlDatabaseEdgeEdition = "SqlDatabaseEdgeEdition";

		public const string IncludeUnsupportedStatements = "IncludeUnsupportedStatements";

		public const string IncludeUnsupportedStatementsDescription = "IncludeUnsupportedStatementsDescription";

		public const string GenerateScriptProgressTitle = "GenerateScriptProgressTitle";

		public const string PublishProgressTitle = "PublishProgressTitle";

		public const string GenerateScriptTable = "GenerateScriptTable";

		public const string GenerateScriptView = "GenerateScriptView";

		public const string GenerateScriptSP = "GenerateScriptSP";

		public const string GenerateScriptUdf = "GenerateScriptUdf";

		public const string GenerateScriptUddt = "GenerateScriptUddt";

		public const string GenerateScriptUdtt = "GenerateScriptUdtt";

		public const string GenerateScriptDefault = "GenerateScriptDefault";

		public const string GenerateScriptRule = "GenerateScriptRule";

		public const string GenerateScriptAppRole = "GenerateScriptAppRole";

		public const string GenerateScriptAssembly = "GenerateScriptAssembly";

		public const string GenerateScriptDatabaseTrigger = "GenerateScriptDatabaseTrigger";

		public const string GenerateScriptDatabaseRole = "GenerateScriptDatabaseRole";

		public const string GenerateScriptSynonym = "GenerateScriptSynonym";

		public const string GenerateScriptUser = "GenerateScriptUser";

		public const string GenerateScriptXMLSchema = "GenerateScriptXMLSchema";

		public const string GenerateScriptSchema = "GenerateScriptSchema";

		public const string GenerateScriptDatabase = "GenerateScriptDatabase";

		public const string GenerateScriptPlanGuide = "GenerateScriptPlanGuide";

		public const string GenerateScriptProgressDiscovery = "GenerateScriptProgressDiscovery";

		public const string GenerateScriptUserDefinedType = "GenerateScriptUserDefinedType";

		public const string GenerateScriptFullTextCatalog = "GenerateScriptFullTextCatalog";

		public const string GenerateScriptUserDefinedAggregate = "GenerateScriptUserDefinedAggregate";

		public const string GenerateScriptDBNotFound = "GenerateScriptDBNotFound";

		public const string GenerateScriptNoObjects = "GenerateScriptNoObjects";

		public const string GenerateScriptFileExists = "GenerateScriptFileExists";

		public const string GenerateScriptInvalidDirectory = "GenerateScriptInvalidDirectory";

		public const string GenarateScriptInvalidFile = "GenarateScriptInvalidFile";

		public const string GenerateScriptStatsWarning = "GenerateScriptStatsWarning";

		public const string ProviderListTitle = "ProviderListTitle";

		public const string ConfiguredProviders = "ConfiguredProviders";

		public const string MSAAConfiguredProviders = "MSAAConfiguredProviders";

		public const string ProviderName = "ProviderName";

		public const string WebServiceAddress = "WebServiceAddress";

		public const string NewText = "NewText";

		public const string EditText = "EditText";

		public const string TestText = "TestText";

		public const string DeleteText = "DeleteText";

		public const string MSAANewText = "MSAANewText";

		public const string MSAAEditText = "MSAAEditText";

		public const string MSAATestText = "MSAATestText";

		public const string MSAADeleteText = "MSAADeleteText";

		public const string ProviderDeleteMessage = "ProviderDeleteMessage";

		public const string ProviderDeleteTitle = "ProviderDeleteTitle";

		public const string TestTitleSuccess = "TestTitleSuccess";

		public const string TestSuccessMessage = "TestSuccessMessage";

		public const string TestTitleFailure = "TestTitleFailure";

		public const string TestFailureMessage = "TestFailureMessage";

		public const string TestFailureUsernamePasswordMessage = "TestFailureUsernamePasswordMessage";

		public const string ProviderListSaveChangeMessage = "ProviderListSaveChangeMessage";

		public const string ProviderListSaveChangeTitle = "ProviderListSaveChangeTitle";

		public const string ERROR_SaveFailedTitle = "ERROR_SaveFailedTitle";

		public const string ERROR_DeleteFailedTitle = "ERROR_DeleteFailedTitle";

		public const string MSAAButtonPanel = "MSAAButtonPanel";

		public const string ProviderDetailTitle = "ProviderDetailTitle";

		public const string ProviderDetailProviderName = "ProviderDetailProviderName";

		public const string MSAAProviderDetailProviderName = "MSAAProviderDetailProviderName";

		public const string ProviderDetailWebAdd = "ProviderDetailWebAdd";

		public const string MSAAProviderDetailWebAdd = "MSAAProviderDetailWebAdd";

		public const string ProviderDetailWebSerAuth = "ProviderDetailWebSerAuth";

		public const string ProviderDetailWebSerUser = "ProviderDetailWebSerUser";

		public const string ProviderDetailWebSerPassword = "ProviderDetailWebSerPassword";

		public const string ProviderDetailWebSerSavePassword = "ProviderDetailWebSerSavePassword";

		public const string ProviderDetailDatabaseGroup = "ProviderDetailDatabaseGroup";

		public const string SetAsDefaultText = "SetAsDefaultText";

		public const string MSAAProviderDetailWebSerUser = "MSAAProviderDetailWebSerUser";

		public const string MSAAProviderDetailWebSerPassword = "MSAAProviderDetailWebSerPassword";

		public const string MSAAProviderDetailWebSerSavePassword = "MSAAProviderDetailWebSerSavePassword";

		public const string MSAAProviderDetailDatabaseGroup = "MSAAProviderDetailDatabaseGroup";

		public const string MSAASetAsDefaultText = "MSAASetAsDefaultText";

		public const string DownloadConfigurationsText = "DownloadConfigurationsText";

		public const string ProviderDetailDeleteMessage = "ProviderDetailDeleteMessage";

		public const string ProviderDetailDeleteTitle = "ProviderDetailDeleteTitle";

		public const string ProviderDetailConfirmDownloadTitle = "ProviderDetailConfirmDownloadTitle";

		public const string ProviderDetailConfirmDownloadText = "ProviderDetailConfirmDownloadText";

		public const string ProviderDetailDownloadSucceedTitle = "ProviderDetailDownloadSucceedTitle";

		public const string ProviderDetailDownloadSucceedText = "ProviderDetailDownloadSucceedText";

		public const string ProviderDetailDownloadFailedTitle = "ProviderDetailDownloadFailedTitle";

		public const string ProviderDetailDownloadFailedNotConfigured = "ProviderDetailDownloadFailedNotConfigured";

		public const string ProviderDetailDownloadFailedCannotConnect = "ProviderDetailDownloadFailedCannotConnect";

		public const string ProviderDetailDownloadFailedUserPassword = "ProviderDetailDownloadFailedUserPassword";

		public const string ProviderDetailSaveChangeMessage = "ProviderDetailSaveChangeMessage";

		public const string ProviderDetailSaveChangeTitle = "ProviderDetailSaveChangeTitle";

		public const string ProviderDetailEmptyProviderNameMessage = "ProviderDetailEmptyProviderNameMessage";

		public const string ProviderDetailEmptyProviderNameMessageTitle = "ProviderDetailEmptyProviderNameMessageTitle";

		public const string ProviderDetailNewProvider = "ProviderDetailNewProvider";

		public const string DatabaseDetailTitle = "DatabaseDetailTitle";

		public const string DatabaseDetailDatabaseName = "DatabaseDetailDatabaseName";

		public const string DatabaseDetailServerName = "DatabaseDetailServerName";

		public const string MSAADatabaseDetailDatabaseName = "MSAADatabaseDetailDatabaseName";

		public const string MSAADatabaseDetailServerName = "MSAADatabaseDetailServerName";

		public const string DatabaseDetailAuthentication = "DatabaseDetailAuthentication";

		public const string DatabaseDetailUserName = "DatabaseDetailUserName";

		public const string DatabaseDetailPassword = "DatabaseDetailPassword";

		public const string DatabaseDetailSavePassword = "DatabaseDetailSavePassword";

		public const string MSAADatabaseDetailUserName = "MSAADatabaseDetailUserName";

		public const string MSAADatabaseDetailPassword = "MSAADatabaseDetailPassword";

		public const string MSAADatabaseDetailSavePassword = "MSAADatabaseDetailSavePassword";

		public const string DatabaseDetailSaveChangeMessage = "DatabaseDetailSaveChangeMessage";

		public const string DatabaseDetailSaveChangeTitle = "DatabaseDetailSaveChangeTitle";

		public const string DatabaseDetailEmptyDatabaseNameMessage = "DatabaseDetailEmptyDatabaseNameMessage";

		public const string DatabaseDetailEmptyDatabaseNameMessageTitle = "DatabaseDetailEmptyDatabaseNameMessageTitle";

		public const string DatabaseDetailNewDatabase = "DatabaseDetailNewDatabase";

		public const string RemoteCreditDialogTitle = "RemoteCreditDialogTitle";

		public const string RemoteCreditSubTitle = "RemoteCreditSubTitle";

		public const string RemoteCreditProviderGroupText = "RemoteCreditProviderGroupText";

		public const string RemoteCreditDatabaseGroupText = "RemoteCreditDatabaseGroupText";

		public const string RemoteCreditProviderName = "RemoteCreditProviderName";

		public const string RemoteCreditWebAdd = "RemoteCreditWebAdd";

		public const string RemoteCreditWebSerUser = "RemoteCreditWebSerUser";

		public const string RemoteCreditWebSerPassword = "RemoteCreditWebSerPassword";

		public const string MSAARemoteCreditWebSerUser = "MSAARemoteCreditWebSerUser";

		public const string MSAARemoteCreditWebSerPassword = "MSAARemoteCreditWebSerPassword";

		public const string RemoteCreditDatabaseName = "RemoteCreditDatabaseName";

		public const string RemoteCreditServerName = "RemoteCreditServerName";

		public const string RemoteCreditUserName = "RemoteCreditUserName";

		public const string RemoteCreditPassword = "RemoteCreditPassword";

		public const string MSAARemoteCreditUserName = "MSAARemoteCreditUserName";

		public const string MSAARemoteCreditPassword = "MSAARemoteCreditPassword";

		public const string PublishOutputOptionTitle = "PublishOutputOptionTitle";

		public const string PublishOutputOptionSubTitle = "PublishOutputOptionSubTitle";

		public static CultureInfo Culture
		{
			get
			{
				return _culture;
			}
			set
			{
				_culture = value;
			}
		}

		private Keys()
		{
		}

		public static string GetString(string key)
		{
			return resourceManager.GetString(key, _culture);
		}

		public static string GetString(string key, object arg0)
		{
			return string.Format(CultureInfo.CurrentCulture, resourceManager.GetString(key, _culture), arg0);
		}
	}

	public static CultureInfo Culture
	{
		get
		{
			return Keys.Culture;
		}
		set
		{
			Keys.Culture = value;
		}
	}

	public static string General => Keys.GetString("General");

	public static string TableView => Keys.GetString("TableView");

	public static string OptionsLabel => Keys.GetString("OptionsLabel");

	public static string PublishingOptions => Keys.GetString("PublishingOptions");

	public static string TrueText => Keys.GetString("TrueText");

	public static string FalseText => Keys.GetString("FalseText");

	public static string ScriptUseDatabase => Keys.GetString("ScriptUseDatabase");

	public static string Generateusedatabase => Keys.GetString("Generateusedatabase");

	public static string ScriptLogins => Keys.GetString("ScriptLogins");

	public static string Generatescriptfordatabaselogins => Keys.GetString("Generatescriptfordatabaselogins");

	public static string ScriptObjectLevelPermissions => Keys.GetString("ScriptObjectLevelPermissions");

	public static string Generateobjectlevelpermissions => Keys.GetString("Generateobjectlevelpermissions");

	public static string PublishObjectLevelPermissions => Keys.GetString("PublishObjectLevelPermissions");

	public static string PublishDObjectLevelPermissions => Keys.GetString("PublishDObjectLevelPermissions");

	public static string ScriptCreate => Keys.GetString("ScriptCreate");

	public static string ScriptDrop => Keys.GetString("ScriptDrop");

	public static string ScriptCreateDrop => Keys.GetString("ScriptCreateDrop");

	public static string ScriptCreateDropDescription => Keys.GetString("ScriptCreateDropDescription");

	public static string TypesOfDataToScriptDesc => Keys.GetString("TypesOfDataToScriptDesc");

	public static string TypesOfDataToScript => Keys.GetString("TypesOfDataToScript");

	public static string DataOnly => Keys.GetString("DataOnly");

	public static string SchemaOnly => Keys.GetString("SchemaOnly");

	public static string SchemaAndData => Keys.GetString("SchemaAndData");

	public static string CreateMode => Keys.GetString("CreateMode");

	public static string DropMode => Keys.GetString("DropMode");

	public static string GenerateScriptForDependentObjects => Keys.GetString("GenerateScriptForDependentObjects");

	public static string Generatescriptfordependentobjectsofeachtablescripted => Keys.GetString("Generatescriptfordependentobjectsofeachtablescripted");

	public static string PublishForDependentObjects => Keys.GetString("PublishForDependentObjects");

	public static string Publishfordependentobjectsofeachtablescripted => Keys.GetString("Publishfordependentobjectsofeachtablescripted");

	public static string IncludeDescriptiveHeaders => Keys.GetString("IncludeDescriptiveHeaders");

	public static string Includedescriptiveheadersforeachtablescripted => Keys.GetString("Includedescriptiveheadersforeachtablescripted");

	public static string IncludeScriptingParametersHeader => Keys.GetString("IncludeScriptingParametersHeader");

	public static string IncludeScriptingParametersHeaderDescription => Keys.GetString("IncludeScriptingParametersHeaderDescription");

	public static string ContinueScriptingOnError => Keys.GetString("ContinueScriptingOnError");

	public static string Continuescriptifanerroroccuredorotherwisestop => Keys.GetString("Continuescriptifanerroroccuredorotherwisestop");

	public static string AppendToFile => Keys.GetString("AppendToFile");

	public static string Appendtofilethegeneratedscript => Keys.GetString("Appendtofilethegeneratedscript");

	public static string ScriptExtendedProperties => Keys.GetString("ScriptExtendedProperties");

	public static string Scriptextendedpropertiesforeachtablescripted => Keys.GetString("Scriptextendedpropertiesforeachtablescripted");

	public static string PublishExtendedProperties => Keys.GetString("PublishExtendedProperties");

	public static string Publishextendedpropertiesforeachtablescripted => Keys.GetString("Publishextendedpropertiesforeachtablescripted");

	public static string ScriptOwnerForTheObjects => Keys.GetString("ScriptOwnerForTheObjects");

	public static string ScriptOwner => Keys.GetString("ScriptOwner");

	public static string Prefixschemaforobjects => Keys.GetString("Prefixschemaforobjects");

	public static string SchemaQualify => Keys.GetString("SchemaQualify");

	public static string IncludeIfNotExists => Keys.GetString("IncludeIfNotExists");

	public static string Scriptobjectswithifnotexistoption => Keys.GetString("Scriptobjectswithifnotexistoption");

	public static string GenerateScriptCollation => Keys.GetString("GenerateScriptCollation");

	public static string Scriptobjectswithcollation => Keys.GetString("Scriptobjectswithcollation");

	public static string PublishCollation => Keys.GetString("PublishCollation");

	public static string Publishobjectswithcollation => Keys.GetString("Publishobjectswithcollation");

	public static string Default => Keys.GetString("Default");

	public static string ScriptChangeTracking => Keys.GetString("ScriptChangeTracking");

	public static string ScriptChangeTrackingInformation => Keys.GetString("ScriptChangeTrackingInformation");

	public static string ScriptDefaults => Keys.GetString("ScriptDefaults");

	public static string ScriptDataCompression => Keys.GetString("ScriptDataCompression");

	public static string ScriptDataCompressionInformation => Keys.GetString("ScriptDataCompressionInformation");

	public static string PublishDefault => Keys.GetString("PublishDefault");

	public static string PublishDefaults => Keys.GetString("PublishDefaults");

	public static string Onlyscript70compatiblefeaturesforeachtablescripted => Keys.GetString("Onlyscript70compatiblefeaturesforeachtablescripted");

	public static string ScriptIndexes => Keys.GetString("ScriptIndexes");

	public static string Scriptindexesforeachtablescripted => Keys.GetString("Scriptindexesforeachtablescripted");

	public static string PublishIndexes => Keys.GetString("PublishIndexes");

	public static string Publishindexesforeachtablescripted => Keys.GetString("Publishindexesforeachtablescripted");

	public static string ScriptFullTextIndexes => Keys.GetString("ScriptFullTextIndexes");

	public static string Scriptfulltextindexesforeachtablescripted => Keys.GetString("Scriptfulltextindexesforeachtablescripted");

	public static string PublishFullTextIndexes => Keys.GetString("PublishFullTextIndexes");

	public static string Publishfulltextindexesforeachtablescripted => Keys.GetString("Publishfulltextindexesforeachtablescripted");

	public static string ScriptTriggers => Keys.GetString("ScriptTriggers");

	public static string Scripttriggersforeachtablescripted => Keys.GetString("Scripttriggersforeachtablescripted");

	public static string PublishTriggers => Keys.GetString("PublishTriggers");

	public static string Publishtriggersforeachtablescripted => Keys.GetString("Publishtriggersforeachtablescripted");

	public static string ScriptPrimaryKeys => Keys.GetString("ScriptPrimaryKeys");

	public static string Scriptprimarykeysforeachtablescripted => Keys.GetString("Scriptprimarykeysforeachtablescripted");

	public static string PublishPrimaryKeys => Keys.GetString("PublishPrimaryKeys");

	public static string Publishprimarykeysforeachtablescripted => Keys.GetString("Publishprimarykeysforeachtablescripted");

	public static string ScriptUniqueKeys => Keys.GetString("ScriptUniqueKeys");

	public static string Scriptuniqueskeysforeachtablescripted => Keys.GetString("Scriptuniqueskeysforeachtablescripted");

	public static string PublishUniqueKeys => Keys.GetString("PublishUniqueKeys");

	public static string Publishuniqueskeysforeachtablescripted => Keys.GetString("Publishuniqueskeysforeachtablescripted");

	public static string ScriptForeignKeys => Keys.GetString("ScriptForeignKeys");

	public static string Scriptforeignkeysforeachtablescripted => Keys.GetString("Scriptforeignkeysforeachtablescripted");

	public static string PublishForeignKeys => Keys.GetString("PublishForeignKeys");

	public static string Publishforeignkeysforeachtablescripted => Keys.GetString("Publishforeignkeysforeachtablescripted");

	public static string ScriptCheckConstraints => Keys.GetString("ScriptCheckConstraints");

	public static string Scriptcheckconstraintsforeachtablescripted => Keys.GetString("Scriptcheckconstraintsforeachtablescripted");

	public static string PublishCheckConstraints => Keys.GetString("PublishCheckConstraints");

	public static string Publishcheckconstraintsforeachtablescripted => Keys.GetString("Publishcheckconstraintsforeachtablescripted");

	public static string ScriptConvertUDDT => Keys.GetString("ScriptConvertUDDT");

	public static string Generateconvertuddt => Keys.GetString("Generateconvertuddt");

	public static string ScriptStatistics => Keys.GetString("ScriptStatistics");

	public static string generatescriptstatistics => Keys.GetString("generatescriptstatistics");

	public static string ScriptStatisticsNone => Keys.GetString("ScriptStatisticsNone");

	public static string ScriptStatisticsDDL => Keys.GetString("ScriptStatisticsDDL");

	public static string ScriptStatisticsAll => Keys.GetString("ScriptStatisticsAll");

	public static string PublishStatistics => Keys.GetString("PublishStatistics");

	public static string Publishgeneratescriptstatistics => Keys.GetString("Publishgeneratescriptstatistics");

	public static string PublishStatisticsNone => Keys.GetString("PublishStatisticsNone");

	public static string PublishStatisticsDDL => Keys.GetString("PublishStatisticsDDL");

	public static string PublishStatisticsAll => Keys.GetString("PublishStatisticsAll");

	public static string IncludeVarDecimal => Keys.GetString("IncludeVarDecimal");

	public static string IncludeVarDecimalDescription => Keys.GetString("IncludeVarDecimalDescription");

	public static string Bindings => Keys.GetString("Bindings");

	public static string BindingsDescription => Keys.GetString("BindingsDescription");

	public static string PublishIncludeVarDecimal => Keys.GetString("PublishIncludeVarDecimal");

	public static string PublishIncludeVarDecimalDescription => Keys.GetString("PublishIncludeVarDecimalDescription");

	public static string ScriptDriIncludeSystemNames => Keys.GetString("ScriptDriIncludeSystemNames");

	public static string generateDriIncludeSystemNames => Keys.GetString("generateDriIncludeSystemNames");

	public static string ScriptAnsiPadding => Keys.GetString("ScriptAnsiPadding");

	public static string generateAnsiPadding => Keys.GetString("generateAnsiPadding");

	public static string ScriptDatabaseCreate => Keys.GetString("ScriptDatabaseCreate");

	public static string genreatescriptdatabasecreate => Keys.GetString("genreatescriptdatabasecreate");

	public static string UseTransactionForPublish => Keys.GetString("UseTransactionForPublish");

	public static string UseTransactionForPublishDesc => Keys.GetString("UseTransactionForPublishDesc");

	public static string GenerateCreateCommand => Keys.GetString("GenerateCreateCommand");

	public static string GenerateDropCommand => Keys.GetString("GenerateDropCommand");

	public static string CompatibleFeatures => Keys.GetString("CompatibleFeatures");

	public static string CompatibleFeaturesDescription => Keys.GetString("CompatibleFeaturesDescription");

	public static string PublishCompatibleFeatures => Keys.GetString("PublishCompatibleFeatures");

	public static string PublishCompatibleFeaturesDescription => Keys.GetString("PublishCompatibleFeaturesDescription");

	public static string OnlyScript90CompatibleFeatures => Keys.GetString("OnlyScript90CompatibleFeatures");

	public static string OnlyScript100CompatibleFeatures => Keys.GetString("OnlyScript100CompatibleFeatures");

	public static string OnlyScript105CompatibleFeatures => Keys.GetString("OnlyScript105CompatibleFeatures");

	public static string OnlyScript110CompatibleFeatures => Keys.GetString("OnlyScript110CompatibleFeatures");

	public static string OnlyScript120CompatibleFeatures => Keys.GetString("OnlyScript120CompatibleFeatures");

	public static string OnlyScript130CompatibleFeatures => Keys.GetString("OnlyScript130CompatibleFeatures");

	public static string OnlyScript140CompatibleFeatures => Keys.GetString("OnlyScript140CompatibleFeatures");

	public static string OnlyScript150CompatibleFeatures => Keys.GetString("OnlyScript150CompatibleFeatures");

	public static string OnlyScript160CompatibleFeatures => Keys.GetString("OnlyScript160CompatibleFeatures");

	public static string TargetEngineType => Keys.GetString("TargetEngineType");

	public static string TargetEngineTypeDescription => Keys.GetString("TargetEngineTypeDescription");

	public static string TargetEngineEdition => Keys.GetString("TargetEngineEdition");

	public static string TargetEngineEditionDescription => Keys.GetString("TargetEngineEditionDescription");

	public static string SingleInstanceEngineType => Keys.GetString("SingleInstanceEngineType");

	public static string SqlAzureEngineType => Keys.GetString("SqlAzureEngineType");

	public static string PersonalEdition => Keys.GetString("PersonalEdition");

	public static string StandardEdition => Keys.GetString("StandardEdition");

	public static string EnterpriseEdition => Keys.GetString("EnterpriseEdition");

	public static string ExpressEdition => Keys.GetString("ExpressEdition");

	public static string SqlAzureDatabaseEdition => Keys.GetString("SqlAzureDatabaseEdition");

	public static string SqlDataWarehouseEdition => Keys.GetString("SqlDataWarehouseEdition");

	public static string StretchEdition => Keys.GetString("StretchEdition");

	public static string SqlServerManagedInstanceEdition => Keys.GetString("SqlServerManagedInstanceEdition");

	public static string SqlServerOnDemandEdition => Keys.GetString("SqlServerOnDemandEdition");

	public static string SqlDatabaseEdgeEdition => Keys.GetString("SqlDatabaseEdgeEdition");

	public static string IncludeUnsupportedStatements => Keys.GetString("IncludeUnsupportedStatements");

	public static string IncludeUnsupportedStatementsDescription => Keys.GetString("IncludeUnsupportedStatementsDescription");

	public static string GenerateScriptProgressTitle => Keys.GetString("GenerateScriptProgressTitle");

	public static string PublishProgressTitle => Keys.GetString("PublishProgressTitle");

	public static string GenerateScriptTable => Keys.GetString("GenerateScriptTable");

	public static string GenerateScriptView => Keys.GetString("GenerateScriptView");

	public static string GenerateScriptSP => Keys.GetString("GenerateScriptSP");

	public static string GenerateScriptUdf => Keys.GetString("GenerateScriptUdf");

	public static string GenerateScriptUddt => Keys.GetString("GenerateScriptUddt");

	public static string GenerateScriptUdtt => Keys.GetString("GenerateScriptUdtt");

	public static string GenerateScriptDefault => Keys.GetString("GenerateScriptDefault");

	public static string GenerateScriptRule => Keys.GetString("GenerateScriptRule");

	public static string GenerateScriptAppRole => Keys.GetString("GenerateScriptAppRole");

	public static string GenerateScriptAssembly => Keys.GetString("GenerateScriptAssembly");

	public static string GenerateScriptDatabaseTrigger => Keys.GetString("GenerateScriptDatabaseTrigger");

	public static string GenerateScriptDatabaseRole => Keys.GetString("GenerateScriptDatabaseRole");

	public static string GenerateScriptSynonym => Keys.GetString("GenerateScriptSynonym");

	public static string GenerateScriptUser => Keys.GetString("GenerateScriptUser");

	public static string GenerateScriptXMLSchema => Keys.GetString("GenerateScriptXMLSchema");

	public static string GenerateScriptSchema => Keys.GetString("GenerateScriptSchema");

	public static string GenerateScriptDatabase => Keys.GetString("GenerateScriptDatabase");

	public static string GenerateScriptPlanGuide => Keys.GetString("GenerateScriptPlanGuide");

	public static string GenerateScriptUserDefinedType => Keys.GetString("GenerateScriptUserDefinedType");

	public static string GenerateScriptFullTextCatalog => Keys.GetString("GenerateScriptFullTextCatalog");

	public static string GenerateScriptUserDefinedAggregate => Keys.GetString("GenerateScriptUserDefinedAggregate");

	public static string GenerateScriptStatsWarning => Keys.GetString("GenerateScriptStatsWarning");

	public static string ProviderListTitle => Keys.GetString("ProviderListTitle");

	public static string ConfiguredProviders => Keys.GetString("ConfiguredProviders");

	public static string MSAAConfiguredProviders => Keys.GetString("MSAAConfiguredProviders");

	public static string ProviderName => Keys.GetString("ProviderName");

	public static string WebServiceAddress => Keys.GetString("WebServiceAddress");

	public static string NewText => Keys.GetString("NewText");

	public static string EditText => Keys.GetString("EditText");

	public static string TestText => Keys.GetString("TestText");

	public static string DeleteText => Keys.GetString("DeleteText");

	public static string MSAANewText => Keys.GetString("MSAANewText");

	public static string MSAAEditText => Keys.GetString("MSAAEditText");

	public static string MSAATestText => Keys.GetString("MSAATestText");

	public static string MSAADeleteText => Keys.GetString("MSAADeleteText");

	public static string ProviderDeleteMessage => Keys.GetString("ProviderDeleteMessage");

	public static string ProviderDeleteTitle => Keys.GetString("ProviderDeleteTitle");

	public static string TestTitleSuccess => Keys.GetString("TestTitleSuccess");

	public static string TestSuccessMessage => Keys.GetString("TestSuccessMessage");

	public static string TestTitleFailure => Keys.GetString("TestTitleFailure");

	public static string TestFailureMessage => Keys.GetString("TestFailureMessage");

	public static string TestFailureUsernamePasswordMessage => Keys.GetString("TestFailureUsernamePasswordMessage");

	public static string ProviderListSaveChangeMessage => Keys.GetString("ProviderListSaveChangeMessage");

	public static string ProviderListSaveChangeTitle => Keys.GetString("ProviderListSaveChangeTitle");

	public static string ERROR_SaveFailedTitle => Keys.GetString("ERROR_SaveFailedTitle");

	public static string ERROR_DeleteFailedTitle => Keys.GetString("ERROR_DeleteFailedTitle");

	public static string MSAAButtonPanel => Keys.GetString("MSAAButtonPanel");

	public static string ProviderDetailTitle => Keys.GetString("ProviderDetailTitle");

	public static string ProviderDetailProviderName => Keys.GetString("ProviderDetailProviderName");

	public static string MSAAProviderDetailProviderName => Keys.GetString("MSAAProviderDetailProviderName");

	public static string ProviderDetailWebAdd => Keys.GetString("ProviderDetailWebAdd");

	public static string MSAAProviderDetailWebAdd => Keys.GetString("MSAAProviderDetailWebAdd");

	public static string ProviderDetailWebSerAuth => Keys.GetString("ProviderDetailWebSerAuth");

	public static string ProviderDetailWebSerUser => Keys.GetString("ProviderDetailWebSerUser");

	public static string ProviderDetailWebSerPassword => Keys.GetString("ProviderDetailWebSerPassword");

	public static string ProviderDetailWebSerSavePassword => Keys.GetString("ProviderDetailWebSerSavePassword");

	public static string ProviderDetailDatabaseGroup => Keys.GetString("ProviderDetailDatabaseGroup");

	public static string SetAsDefaultText => Keys.GetString("SetAsDefaultText");

	public static string MSAAProviderDetailWebSerUser => Keys.GetString("MSAAProviderDetailWebSerUser");

	public static string MSAAProviderDetailWebSerPassword => Keys.GetString("MSAAProviderDetailWebSerPassword");

	public static string MSAAProviderDetailWebSerSavePassword => Keys.GetString("MSAAProviderDetailWebSerSavePassword");

	public static string MSAAProviderDetailDatabaseGroup => Keys.GetString("MSAAProviderDetailDatabaseGroup");

	public static string MSAASetAsDefaultText => Keys.GetString("MSAASetAsDefaultText");

	public static string DownloadConfigurationsText => Keys.GetString("DownloadConfigurationsText");

	public static string ProviderDetailDeleteMessage => Keys.GetString("ProviderDetailDeleteMessage");

	public static string ProviderDetailDeleteTitle => Keys.GetString("ProviderDetailDeleteTitle");

	public static string ProviderDetailConfirmDownloadTitle => Keys.GetString("ProviderDetailConfirmDownloadTitle");

	public static string ProviderDetailConfirmDownloadText => Keys.GetString("ProviderDetailConfirmDownloadText");

	public static string ProviderDetailDownloadSucceedTitle => Keys.GetString("ProviderDetailDownloadSucceedTitle");

	public static string ProviderDetailDownloadSucceedText => Keys.GetString("ProviderDetailDownloadSucceedText");

	public static string ProviderDetailDownloadFailedTitle => Keys.GetString("ProviderDetailDownloadFailedTitle");

	public static string ProviderDetailDownloadFailedNotConfigured => Keys.GetString("ProviderDetailDownloadFailedNotConfigured");

	public static string ProviderDetailDownloadFailedCannotConnect => Keys.GetString("ProviderDetailDownloadFailedCannotConnect");

	public static string ProviderDetailDownloadFailedUserPassword => Keys.GetString("ProviderDetailDownloadFailedUserPassword");

	public static string ProviderDetailSaveChangeMessage => Keys.GetString("ProviderDetailSaveChangeMessage");

	public static string ProviderDetailSaveChangeTitle => Keys.GetString("ProviderDetailSaveChangeTitle");

	public static string ProviderDetailEmptyProviderNameMessage => Keys.GetString("ProviderDetailEmptyProviderNameMessage");

	public static string ProviderDetailEmptyProviderNameMessageTitle => Keys.GetString("ProviderDetailEmptyProviderNameMessageTitle");

	public static string ProviderDetailNewProvider => Keys.GetString("ProviderDetailNewProvider");

	public static string DatabaseDetailTitle => Keys.GetString("DatabaseDetailTitle");

	public static string DatabaseDetailDatabaseName => Keys.GetString("DatabaseDetailDatabaseName");

	public static string DatabaseDetailServerName => Keys.GetString("DatabaseDetailServerName");

	public static string MSAADatabaseDetailDatabaseName => Keys.GetString("MSAADatabaseDetailDatabaseName");

	public static string MSAADatabaseDetailServerName => Keys.GetString("MSAADatabaseDetailServerName");

	public static string DatabaseDetailAuthentication => Keys.GetString("DatabaseDetailAuthentication");

	public static string DatabaseDetailUserName => Keys.GetString("DatabaseDetailUserName");

	public static string DatabaseDetailPassword => Keys.GetString("DatabaseDetailPassword");

	public static string DatabaseDetailSavePassword => Keys.GetString("DatabaseDetailSavePassword");

	public static string MSAADatabaseDetailUserName => Keys.GetString("MSAADatabaseDetailUserName");

	public static string MSAADatabaseDetailPassword => Keys.GetString("MSAADatabaseDetailPassword");

	public static string MSAADatabaseDetailSavePassword => Keys.GetString("MSAADatabaseDetailSavePassword");

	public static string DatabaseDetailSaveChangeMessage => Keys.GetString("DatabaseDetailSaveChangeMessage");

	public static string DatabaseDetailSaveChangeTitle => Keys.GetString("DatabaseDetailSaveChangeTitle");

	public static string DatabaseDetailEmptyDatabaseNameMessage => Keys.GetString("DatabaseDetailEmptyDatabaseNameMessage");

	public static string DatabaseDetailEmptyDatabaseNameMessageTitle => Keys.GetString("DatabaseDetailEmptyDatabaseNameMessageTitle");

	public static string DatabaseDetailNewDatabase => Keys.GetString("DatabaseDetailNewDatabase");

	public static string RemoteCreditDialogTitle => Keys.GetString("RemoteCreditDialogTitle");

	public static string RemoteCreditSubTitle => Keys.GetString("RemoteCreditSubTitle");

	public static string RemoteCreditProviderGroupText => Keys.GetString("RemoteCreditProviderGroupText");

	public static string RemoteCreditDatabaseGroupText => Keys.GetString("RemoteCreditDatabaseGroupText");

	public static string RemoteCreditProviderName => Keys.GetString("RemoteCreditProviderName");

	public static string RemoteCreditWebAdd => Keys.GetString("RemoteCreditWebAdd");

	public static string RemoteCreditWebSerUser => Keys.GetString("RemoteCreditWebSerUser");

	public static string RemoteCreditWebSerPassword => Keys.GetString("RemoteCreditWebSerPassword");

	public static string MSAARemoteCreditWebSerUser => Keys.GetString("MSAARemoteCreditWebSerUser");

	public static string MSAARemoteCreditWebSerPassword => Keys.GetString("MSAARemoteCreditWebSerPassword");

	public static string RemoteCreditDatabaseName => Keys.GetString("RemoteCreditDatabaseName");

	public static string RemoteCreditServerName => Keys.GetString("RemoteCreditServerName");

	public static string RemoteCreditUserName => Keys.GetString("RemoteCreditUserName");

	public static string RemoteCreditPassword => Keys.GetString("RemoteCreditPassword");

	public static string MSAARemoteCreditUserName => Keys.GetString("MSAARemoteCreditUserName");

	public static string MSAARemoteCreditPassword => Keys.GetString("MSAARemoteCreditPassword");

	public static string PublishOutputOptionTitle => Keys.GetString("PublishOutputOptionTitle");

	public static string PublishOutputOptionSubTitle => Keys.GetString("PublishOutputOptionSubTitle");

	protected SqlScriptOptionsSR()
	{
	}

	public static string GenerateScriptProgressDiscovery(string databaseName)
	{
		return Keys.GetString("GenerateScriptProgressDiscovery", databaseName);
	}

	public static string GenerateScriptDBNotFound(string databaseName)
	{
		return Keys.GetString("GenerateScriptDBNotFound", databaseName);
	}

	public static string GenerateScriptNoObjects(string databaseName)
	{
		return Keys.GetString("GenerateScriptNoObjects", databaseName);
	}

	public static string GenerateScriptFileExists(string fileName)
	{
		return Keys.GetString("GenerateScriptFileExists", fileName);
	}

	public static string GenerateScriptInvalidDirectory(string directoryName)
	{
		return Keys.GetString("GenerateScriptInvalidDirectory", directoryName);
	}

	public static string GenarateScriptInvalidFile(string filename)
	{
		return Keys.GetString("GenarateScriptInvalidFile", filename);
	}
}
