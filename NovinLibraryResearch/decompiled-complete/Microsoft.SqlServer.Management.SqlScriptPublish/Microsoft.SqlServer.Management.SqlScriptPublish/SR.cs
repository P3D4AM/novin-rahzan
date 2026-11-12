using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.SqlServer.Management.SqlScriptPublish;

[CompilerGenerated]
internal class SR
{
	[CompilerGenerated]
	public class Keys
	{
		private static ResourceManager resourceManager = new ResourceManager(typeof(SR));

		private static CultureInfo _culture = null;

		public const string Table = "Table";

		public const string View = "View";

		public const string StoredProcedure = "StoredProcedure";

		public const string UserDefinedFunction = "UserDefinedFunction";

		public const string UserDefinedDataType = "UserDefinedDataType";

		public const string UserDefinedTableType = "UserDefinedTableType";

		public const string SecurityPolicy = "SecurityPolicy";

		public const string Default = "Default";

		public const string Rule = "Rule";

		public const string ApplicationRole = "ApplicationRole";

		public const string SqlAssembly = "SqlAssembly";

		public const string DdlTrigger = "DdlTrigger";

		public const string DatabaseRole = "DatabaseRole";

		public const string Synonym = "Synonym";

		public const string User = "User";

		public const string XmlSchemaCollection = "XmlSchemaCollection";

		public const string Schema = "Schema";

		public const string PlanGuide = "PlanGuide";

		public const string UserDefinedType = "UserDefinedType";

		public const string UserDefinedAggregate = "UserDefinedAggregate";

		public const string FullTextCatalog = "FullTextCatalog";

		public const string None = "None";

		public const string InProgress = "InProgress";

		public const string Success = "Success";

		public const string Warning = "Warning";

		public const string Error = "Error";

		public const string UnableToConnect = "UnableToConnect";

		public const string UnableToFind = "UnableToFind";

		public const string ValueIsNull = "ValueIsNull";

		public const string ValueIsNullOrEmpty = "ValueIsNullOrEmpty";

		public const string InvalidObjectType = "InvalidObjectType";

		public const string InvalidObjectTypeForVersion = "InvalidObjectTypeForVersion";

		public const string ERROR_ScriptingFailed = "ERROR_ScriptingFailed";

		public const string InvalidScriptPreText = "InvalidScriptPreText";

		public const string InvalidScriptPostText = "InvalidScriptPostText";

		public const string ERROR_PublishDataInternalError = "ERROR_PublishDataInternalError";

		public const string ERROR_PublishDataUdtError = "ERROR_PublishDataUdtError";

		public const string ERROR_RowExceedsMaxRequestSize = "ERROR_RowExceedsMaxRequestSize";

		public const string ERROR_PublishScriptFailed = "ERROR_PublishScriptFailed";

		public const string ERROR_UnsupportedDatabaseEngineEditionAndDatabaseEngineTypeOptionCombination = "ERROR_UnsupportedDatabaseEngineEditionAndDatabaseEngineTypeOptionCombination";

		public const string ERROR_UnexpectedDatabaseEngineTypeDetected = "ERROR_UnexpectedDatabaseEngineTypeDetected";

		public const string ERROR_UnexpectedDatabaseEngineEditionDetected = "ERROR_UnexpectedDatabaseEngineEditionDetected";

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

		public static string GetString(string key, object arg0, object arg1)
		{
			return string.Format(CultureInfo.CurrentCulture, resourceManager.GetString(key, _culture), arg0, arg1);
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

	public static string Table => Keys.GetString("Table");

	public static string View => Keys.GetString("View");

	public static string StoredProcedure => Keys.GetString("StoredProcedure");

	public static string UserDefinedFunction => Keys.GetString("UserDefinedFunction");

	public static string UserDefinedDataType => Keys.GetString("UserDefinedDataType");

	public static string UserDefinedTableType => Keys.GetString("UserDefinedTableType");

	public static string SecurityPolicy => Keys.GetString("SecurityPolicy");

	public static string Default => Keys.GetString("Default");

	public static string Rule => Keys.GetString("Rule");

	public static string ApplicationRole => Keys.GetString("ApplicationRole");

	public static string SqlAssembly => Keys.GetString("SqlAssembly");

	public static string DdlTrigger => Keys.GetString("DdlTrigger");

	public static string DatabaseRole => Keys.GetString("DatabaseRole");

	public static string Synonym => Keys.GetString("Synonym");

	public static string User => Keys.GetString("User");

	public static string XmlSchemaCollection => Keys.GetString("XmlSchemaCollection");

	public static string Schema => Keys.GetString("Schema");

	public static string PlanGuide => Keys.GetString("PlanGuide");

	public static string UserDefinedType => Keys.GetString("UserDefinedType");

	public static string UserDefinedAggregate => Keys.GetString("UserDefinedAggregate");

	public static string FullTextCatalog => Keys.GetString("FullTextCatalog");

	public static string None => Keys.GetString("None");

	public static string InProgress => Keys.GetString("InProgress");

	public static string Success => Keys.GetString("Success");

	public static string Warning => Keys.GetString("Warning");

	public static string Error => Keys.GetString("Error");

	public static string ERROR_ScriptingFailed => Keys.GetString("ERROR_ScriptingFailed");

	public static string InvalidScriptPreText => Keys.GetString("InvalidScriptPreText");

	public static string InvalidScriptPostText => Keys.GetString("InvalidScriptPostText");

	public static string ERROR_PublishDataInternalError => Keys.GetString("ERROR_PublishDataInternalError");

	public static string ERROR_PublishDataUdtError => Keys.GetString("ERROR_PublishDataUdtError");

	public static string ERROR_RowExceedsMaxRequestSize => Keys.GetString("ERROR_RowExceedsMaxRequestSize");

	public static string ERROR_PublishScriptFailed => Keys.GetString("ERROR_PublishScriptFailed");

	protected SR()
	{
	}

	public static string UnableToConnect(string server)
	{
		return Keys.GetString("UnableToConnect", server);
	}

	public static string UnableToFind(string @object)
	{
		return Keys.GetString("UnableToFind", @object);
	}

	public static string ValueIsNull(string str)
	{
		return Keys.GetString("ValueIsNull", str);
	}

	public static string ValueIsNullOrEmpty(string str)
	{
		return Keys.GetString("ValueIsNullOrEmpty", str);
	}

	public static string InvalidObjectType(string typeName)
	{
		return Keys.GetString("InvalidObjectType", typeName);
	}

	public static string InvalidObjectTypeForVersion(string name, string type)
	{
		return Keys.GetString("InvalidObjectTypeForVersion", name, type);
	}

	public static string ERROR_UnsupportedDatabaseEngineEditionAndDatabaseEngineTypeOptionCombination(string databaseEngineEdition, string databaseEngineType)
	{
		return Keys.GetString("ERROR_UnsupportedDatabaseEngineEditionAndDatabaseEngineTypeOptionCombination", databaseEngineEdition, databaseEngineType);
	}

	public static string ERROR_UnexpectedDatabaseEngineTypeDetected(string databaseEngineType)
	{
		return Keys.GetString("ERROR_UnexpectedDatabaseEngineTypeDetected", databaseEngineType);
	}

	public static string ERROR_UnexpectedDatabaseEngineEditionDetected(string databaseEngineEdition)
	{
		return Keys.GetString("ERROR_UnexpectedDatabaseEngineEditionDetected", databaseEngineEdition);
	}
}
