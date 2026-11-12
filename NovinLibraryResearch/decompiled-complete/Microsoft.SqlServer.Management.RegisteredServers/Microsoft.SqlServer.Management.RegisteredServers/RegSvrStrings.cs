using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.SqlServer.Management.RegisteredServers;

[CompilerGenerated]
internal class RegSvrStrings
{
	[CompilerGenerated]
	public class Keys
	{
		private static ResourceManager resourceManager = new ResourceManager(typeof(RegSvrStrings));

		private static CultureInfo _culture = null;

		public const string NoSuchCollection = "NoSuchCollection";

		public const string ArgumentNullOrEmpty = "ArgumentNullOrEmpty";

		public const string InvalidServerGroup = "InvalidServerGroup";

		public const string CannotRenameSystemServerGroup = "CannotRenameSystemServerGroup";

		public const string CannotDropSystemServerGroup = "CannotDropSystemServerGroup";

		public const string ServerGroupAlreadyExists = "ServerGroupAlreadyExists";

		public const string UnknownEnumeration = "UnknownEnumeration";

		public const string FailedOperation = "FailedOperation";

		public const string InvalidSqlServer2005FileFormat = "InvalidSqlServer2005FileFormat";

		public const string LocalStoreOnly = "LocalStoreOnly";

		public const string FailedToDeserialize = "FailedToDeserialize";

		public const string CannotCreateAServerGroupUnderneathCentralManagementServerGroup = "CannotCreateAServerGroupUnderneathCentralManagementServerGroup";

		public const string Export = "Export";

		public const string Import = "Import";

		public const string Upgrade = "Upgrade";

		public const string LocalServerStoreDisplayName = "LocalServerStoreDisplayName";

		public const string CentralManagementServersDisplayName = "CentralManagementServersDisplayName";

		public const string DatabaseEngineServerGroupDisplayName = "DatabaseEngineServerGroupDisplayName";

		public const string AnalysisServicesServerGroupDisplayName = "AnalysisServicesServerGroupDisplayName";

		public const string ReportingServicesServerGroupDisplayName = "ReportingServicesServerGroupDisplayName";

		public const string IntegrationServicesServerGroupDisplayName = "IntegrationServicesServerGroupDisplayName";

		public const string SqlServerCompactServerGroupDisplayName = "SqlServerCompactServerGroupDisplayName";

		public const string CentralManagementServerGroupDisplayName = "CentralManagementServerGroupDisplayName";

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

	public static string LocalStoreOnly => Keys.GetString("LocalStoreOnly");

	public static string FailedToDeserialize => Keys.GetString("FailedToDeserialize");

	public static string CannotCreateAServerGroupUnderneathCentralManagementServerGroup => Keys.GetString("CannotCreateAServerGroupUnderneathCentralManagementServerGroup");

	public static string Export => Keys.GetString("Export");

	public static string Import => Keys.GetString("Import");

	public static string Upgrade => Keys.GetString("Upgrade");

	public static string LocalServerStoreDisplayName => Keys.GetString("LocalServerStoreDisplayName");

	public static string CentralManagementServersDisplayName => Keys.GetString("CentralManagementServersDisplayName");

	public static string DatabaseEngineServerGroupDisplayName => Keys.GetString("DatabaseEngineServerGroupDisplayName");

	public static string AnalysisServicesServerGroupDisplayName => Keys.GetString("AnalysisServicesServerGroupDisplayName");

	public static string ReportingServicesServerGroupDisplayName => Keys.GetString("ReportingServicesServerGroupDisplayName");

	public static string IntegrationServicesServerGroupDisplayName => Keys.GetString("IntegrationServicesServerGroupDisplayName");

	public static string SqlServerCompactServerGroupDisplayName => Keys.GetString("SqlServerCompactServerGroupDisplayName");

	public static string CentralManagementServerGroupDisplayName => Keys.GetString("CentralManagementServerGroupDisplayName");

	protected RegSvrStrings()
	{
	}

	public static string NoSuchCollection(string name)
	{
		return Keys.GetString("NoSuchCollection", name);
	}

	public static string ArgumentNullOrEmpty(string argument)
	{
		return Keys.GetString("ArgumentNullOrEmpty", argument);
	}

	public static string InvalidServerGroup(string name)
	{
		return Keys.GetString("InvalidServerGroup", name);
	}

	public static string CannotRenameSystemServerGroup(string name)
	{
		return Keys.GetString("CannotRenameSystemServerGroup", name);
	}

	public static string CannotDropSystemServerGroup(string name)
	{
		return Keys.GetString("CannotDropSystemServerGroup", name);
	}

	public static string ServerGroupAlreadyExists(string name)
	{
		return Keys.GetString("ServerGroupAlreadyExists", name);
	}

	public static string UnknownEnumeration(string type)
	{
		return Keys.GetString("UnknownEnumeration", type);
	}

	public static string FailedOperation(string op)
	{
		return Keys.GetString("FailedOperation", op);
	}

	public static string InvalidSqlServer2005FileFormat(string element)
	{
		return Keys.GetString("InvalidSqlServer2005FileFormat", element);
	}
}
