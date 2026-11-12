using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.SqlServer.Management.Dmf;

[CompilerGenerated]
internal class FacetPropertiesSR
{
	[CompilerGenerated]
	public class Keys
	{
		private static ResourceManager resourceManager = new ResourceManager(typeof(FacetPropertiesSR));

		private static CultureInfo _culture = null;

		public const string OffByDefault_AdHocDistributedQueriesEnabled = "OffByDefault_AdHocDistributedQueriesEnabled";

		public const string OffByDefault_DatabaseMailEnabled = "OffByDefault_DatabaseMailEnabled";

		public const string ConditionExpressionOnObject = "ConditionExpressionOnObject";

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

	public static string OffByDefault_AdHocDistributedQueriesEnabled => Keys.GetString("OffByDefault_AdHocDistributedQueriesEnabled");

	public static string OffByDefault_DatabaseMailEnabled => Keys.GetString("OffByDefault_DatabaseMailEnabled");

	protected FacetPropertiesSR()
	{
	}

	public static string ConditionExpressionOnObject(string policy)
	{
		return Keys.GetString("ConditionExpressionOnObject", policy);
	}
}
