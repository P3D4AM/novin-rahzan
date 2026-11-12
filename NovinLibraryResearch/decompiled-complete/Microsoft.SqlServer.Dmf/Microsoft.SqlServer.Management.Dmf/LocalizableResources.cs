using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.SqlServer.Management.Dmf;

[CompilerGenerated]
internal class LocalizableResources
{
	[CompilerGenerated]
	public class Keys
	{
		private static ResourceManager resourceManager = new ResourceManager(typeof(LocalizableResources));

		private static CultureInfo _culture = null;

		public const string PolicyStore_Name = "PolicyStore_Name";

		public const string PolicyStore_Desc = "PolicyStore_Desc";

		public const string PolicyStore_EnabledName = "PolicyStore_EnabledName";

		public const string PolicyStore_EnabledDesc = "PolicyStore_EnabledDesc";

		public const string PolicyStore_HistoryRetentionInDaysName = "PolicyStore_HistoryRetentionInDaysName";

		public const string PolicyStore_HistoryRetentionInDaysDesc = "PolicyStore_HistoryRetentionInDaysDesc";

		public const string PolicyStore_LogOnSuccessName = "PolicyStore_LogOnSuccessName";

		public const string PolicyStore_LogOnSuccessDesc = "PolicyStore_LogOnSuccessDesc";

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

	public static string PolicyStore_Name => Keys.GetString("PolicyStore_Name");

	public static string PolicyStore_Desc => Keys.GetString("PolicyStore_Desc");

	public static string PolicyStore_EnabledName => Keys.GetString("PolicyStore_EnabledName");

	public static string PolicyStore_EnabledDesc => Keys.GetString("PolicyStore_EnabledDesc");

	public static string PolicyStore_HistoryRetentionInDaysName => Keys.GetString("PolicyStore_HistoryRetentionInDaysName");

	public static string PolicyStore_HistoryRetentionInDaysDesc => Keys.GetString("PolicyStore_HistoryRetentionInDaysDesc");

	public static string PolicyStore_LogOnSuccessName => Keys.GetString("PolicyStore_LogOnSuccessName");

	public static string PolicyStore_LogOnSuccessDesc => Keys.GetString("PolicyStore_LogOnSuccessDesc");

	protected LocalizableResources()
	{
	}
}
