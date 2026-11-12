using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.SqlServer.Management.Collector;

[CompilerGenerated]
internal class ExceptionTemplatesSR
{
	[CompilerGenerated]
	public class Keys
	{
		private static ResourceManager resourceManager = new ResourceManager(typeof(ExceptionTemplatesSR));

		private static CultureInfo _culture = null;

		public const string ConfigStoreCannotBeNull = "ConfigStoreCannotBeNull";

		public const string UnableToAlterConfigStore = "UnableToAlterConfigStore";

		public const string UnableToEnableCollector = "UnableToEnableCollector";

		public const string UnableToDisableCollector = "UnableToDisableCollector";

		public const string UnableToEnumerateCollectorTypes = "UnableToEnumerateCollectorTypes";

		public const string UnableToReadExecutionLog = "UnableToReadExecutionLog";

		public const string CollectionItemAlreadyCreatedBefore = "CollectionItemAlreadyCreatedBefore";

		public const string CollectionItemAlreadyInCollection = "CollectionItemAlreadyInCollection";

		public const string CollectionItemNoType = "CollectionItemNoType";

		public const string CollectionItemHasNoName = "CollectionItemHasNoName";

		public const string CollectionItemNotInCollection = "CollectionItemNotInCollection";

		public const string CollectionItemWasNotCreated = "CollectionItemWasNotCreated";

		public const string CollectionItemWasRemoved = "CollectionItemWasRemoved";

		public const string CollectionFrequencyIsNegative = "CollectionFrequencyIsNegative";

		public const string CollectionFrequencyTooLarge = "CollectionFrequencyTooLarge";

		public const string CollectionItemCannotBeNull = "CollectionItemCannotBeNull";

		public const string ParentCollectionSetCannotBeNull = "ParentCollectionSetCannotBeNull";

		public const string UnableToAlterCollectionItem = "UnableToAlterCollectionItem";

		public const string CollectionSetAlreadyExists = "CollectionSetAlreadyExists";

		public const string CollectionSetWithoutSchedule = "CollectionSetWithoutSchedule";

		public const string CollectionSetProxyNotExist = "CollectionSetProxyNotExist";

		public const string CollectionSetScheduleNotExist = "CollectionSetScheduleNotExist";

		public const string CollectionSetHasNoName = "CollectionSetHasNoName";

		public const string CollectionSetDoesNotExist = "CollectionSetDoesNotExist";

		public const string CollectionSetWasRemoved = "CollectionSetWasRemoved";

		public const string InvalidCollectionMode = "InvalidCollectionMode";

		public const string InvalidLoggingLevel = "InvalidLoggingLevel";

		public const string InvalidDaysUntilExpiration = "InvalidDaysUntilExpiration";

		public const string UnableToCreateCollectionSet = "UnableToCreateCollectionSet";

		public const string UnableToAlterCollectionSet = "UnableToAlterCollectionSet";

		public const string UnableToDropCollectionSet = "UnableToDropCollectionSet";

		public const string UnableToStartCollectionSet = "UnableToStartCollectionSet";

		public const string UnableToStopCollectionSet = "UnableToStopCollectionSet";

		public const string UnableToUploadCollectionSet = "UnableToUploadCollectionSet";

		public const string UnableToRunCollectionSet = "UnableToRunCollectionSet";

		public const string UnableToCleanupCollectionSet = "UnableToCleanupCollectionSet";

		public const string CollectionSetCannotBeNull = "CollectionSetCannotBeNull";

		public const string CannotSetObjectNameExistingObject = "CannotSetObjectNameExistingObject";

		public const string NameNotNull = "NameNotNull";

		public const string ValueTooLong = "ValueTooLong";

		public const string NoSuchCollection = "NoSuchCollection";

		public const string SqlConnectionCannotBeNull = "SqlConnectionCannotBeNull";

		public const string UnknownEnumeration = "UnknownEnumeration";

		public const string UnsupportedAction = "UnsupportedAction";

		public const string UnsupportedObjectTypeByFactory = "UnsupportedObjectTypeByFactory";

		public const string UnsupportedObjectType = "UnsupportedObjectType";

		public const string UnsupportedConnectionType = "UnsupportedConnectionType";

		public const string UnableToCreateSPParserFn = "UnableToCreateSPParserFn";

		public const string UnableToCreateTblParserFn = "UnableToCreateTblParserFn";

		public const string UnableToCreateMDWObjects = "UnableToCreateMDWObjects";

		public const string StoredProcAnalysisCollectorName = "StoredProcAnalysisCollectorName";

		public const string StoredProcAnalysisCollectorDesc = "StoredProcAnalysisCollectorDesc";

		public const string TableAnalysisCollectorName = "TableAnalysisCollectorName";

		public const string TableAnalysisCollectorDesc = "TableAnalysisCollectorDesc";

		public const string TableStatsName = "TableStatsName";

		public const string MigrationBlockers = "MigrationBlockers";

		public const string TransactionCount = "TransactionCount";

		public const string SPStatsName = "SPStatsName";

		public const string SPTableRefs = "SPTableRefs";

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

	public static string ConfigStoreCannotBeNull => Keys.GetString("ConfigStoreCannotBeNull");

	public static string UnableToAlterConfigStore => Keys.GetString("UnableToAlterConfigStore");

	public static string UnableToEnableCollector => Keys.GetString("UnableToEnableCollector");

	public static string UnableToDisableCollector => Keys.GetString("UnableToDisableCollector");

	public static string UnableToEnumerateCollectorTypes => Keys.GetString("UnableToEnumerateCollectorTypes");

	public static string UnableToReadExecutionLog => Keys.GetString("UnableToReadExecutionLog");

	public static string CollectionItemHasNoName => Keys.GetString("CollectionItemHasNoName");

	public static string CollectionItemCannotBeNull => Keys.GetString("CollectionItemCannotBeNull");

	public static string ParentCollectionSetCannotBeNull => Keys.GetString("ParentCollectionSetCannotBeNull");

	public static string CollectionSetHasNoName => Keys.GetString("CollectionSetHasNoName");

	public static string CollectionSetCannotBeNull => Keys.GetString("CollectionSetCannotBeNull");

	public static string CannotSetObjectNameExistingObject => Keys.GetString("CannotSetObjectNameExistingObject");

	public static string NameNotNull => Keys.GetString("NameNotNull");

	public static string SqlConnectionCannotBeNull => Keys.GetString("SqlConnectionCannotBeNull");

	public static string UnsupportedAction => Keys.GetString("UnsupportedAction");

	public static string UnableToCreateMDWObjects => Keys.GetString("UnableToCreateMDWObjects");

	public static string StoredProcAnalysisCollectorName => Keys.GetString("StoredProcAnalysisCollectorName");

	public static string StoredProcAnalysisCollectorDesc => Keys.GetString("StoredProcAnalysisCollectorDesc");

	public static string TableAnalysisCollectorName => Keys.GetString("TableAnalysisCollectorName");

	public static string TableAnalysisCollectorDesc => Keys.GetString("TableAnalysisCollectorDesc");

	public static string TableStatsName => Keys.GetString("TableStatsName");

	public static string MigrationBlockers => Keys.GetString("MigrationBlockers");

	public static string TransactionCount => Keys.GetString("TransactionCount");

	public static string SPStatsName => Keys.GetString("SPStatsName");

	public static string SPTableRefs => Keys.GetString("SPTableRefs");

	protected ExceptionTemplatesSR()
	{
	}

	public static string CollectionItemAlreadyCreatedBefore(string itemName)
	{
		return Keys.GetString("CollectionItemAlreadyCreatedBefore", itemName);
	}

	public static string CollectionItemAlreadyInCollection(string itemName)
	{
		return Keys.GetString("CollectionItemAlreadyInCollection", itemName);
	}

	public static string CollectionItemNoType(string itemName)
	{
		return Keys.GetString("CollectionItemNoType", itemName);
	}

	public static string CollectionItemNotInCollection(string itemName)
	{
		return Keys.GetString("CollectionItemNotInCollection", itemName);
	}

	public static string CollectionItemWasNotCreated(string itemName)
	{
		return Keys.GetString("CollectionItemWasNotCreated", itemName);
	}

	public static string CollectionItemWasRemoved(string itemName)
	{
		return Keys.GetString("CollectionItemWasRemoved", itemName);
	}

	public static string CollectionFrequencyIsNegative(string itemName)
	{
		return Keys.GetString("CollectionFrequencyIsNegative", itemName);
	}

	public static string CollectionFrequencyTooLarge(int maxVal)
	{
		return Keys.GetString("CollectionFrequencyTooLarge", maxVal);
	}

	public static string UnableToAlterCollectionItem(string itemName)
	{
		return Keys.GetString("UnableToAlterCollectionItem", itemName);
	}

	public static string CollectionSetAlreadyExists(string setName)
	{
		return Keys.GetString("CollectionSetAlreadyExists", setName);
	}

	public static string CollectionSetWithoutSchedule(string setName)
	{
		return Keys.GetString("CollectionSetWithoutSchedule", setName);
	}

	public static string CollectionSetProxyNotExist(string proxyName)
	{
		return Keys.GetString("CollectionSetProxyNotExist", proxyName);
	}

	public static string CollectionSetScheduleNotExist(string schedName, string setName)
	{
		return Keys.GetString("CollectionSetScheduleNotExist", schedName, setName);
	}

	public static string CollectionSetDoesNotExist(string setName)
	{
		return Keys.GetString("CollectionSetDoesNotExist", setName);
	}

	public static string CollectionSetWasRemoved(string setName)
	{
		return Keys.GetString("CollectionSetWasRemoved", setName);
	}

	public static string InvalidCollectionMode(string setName)
	{
		return Keys.GetString("InvalidCollectionMode", setName);
	}

	public static string InvalidLoggingLevel(string setName)
	{
		return Keys.GetString("InvalidLoggingLevel", setName);
	}

	public static string InvalidDaysUntilExpiration(string setName)
	{
		return Keys.GetString("InvalidDaysUntilExpiration", setName);
	}

	public static string UnableToCreateCollectionSet(string setName)
	{
		return Keys.GetString("UnableToCreateCollectionSet", setName);
	}

	public static string UnableToAlterCollectionSet(string setName)
	{
		return Keys.GetString("UnableToAlterCollectionSet", setName);
	}

	public static string UnableToDropCollectionSet(string setName)
	{
		return Keys.GetString("UnableToDropCollectionSet", setName);
	}

	public static string UnableToStartCollectionSet(string setName)
	{
		return Keys.GetString("UnableToStartCollectionSet", setName);
	}

	public static string UnableToStopCollectionSet(string setName)
	{
		return Keys.GetString("UnableToStopCollectionSet", setName);
	}

	public static string UnableToUploadCollectionSet(string setName)
	{
		return Keys.GetString("UnableToUploadCollectionSet", setName);
	}

	public static string UnableToRunCollectionSet(string setName)
	{
		return Keys.GetString("UnableToRunCollectionSet", setName);
	}

	public static string UnableToCleanupCollectionSet(string setName)
	{
		return Keys.GetString("UnableToCleanupCollectionSet", setName);
	}

	public static string ValueTooLong(string valueName, int maxLength)
	{
		return Keys.GetString("ValueTooLong", valueName, maxLength);
	}

	public static string NoSuchCollection(string name)
	{
		return Keys.GetString("NoSuchCollection", name);
	}

	public static string UnknownEnumeration(string type)
	{
		return Keys.GetString("UnknownEnumeration", type);
	}

	public static string UnsupportedObjectTypeByFactory(string host, string typeName)
	{
		return Keys.GetString("UnsupportedObjectTypeByFactory", host, typeName);
	}

	public static string UnsupportedObjectType(string type, string host)
	{
		return Keys.GetString("UnsupportedObjectType", type, host);
	}

	public static string UnsupportedConnectionType(string type)
	{
		return Keys.GetString("UnsupportedConnectionType", type);
	}

	public static string UnableToCreateSPParserFn(string name)
	{
		return Keys.GetString("UnableToCreateSPParserFn", name);
	}

	public static string UnableToCreateTblParserFn(string name)
	{
		return Keys.GetString("UnableToCreateTblParserFn", name);
	}
}
