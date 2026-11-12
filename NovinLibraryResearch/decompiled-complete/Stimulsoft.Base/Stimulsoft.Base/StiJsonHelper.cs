using System.Globalization;
using Stimulsoft.Base.Json;

namespace Stimulsoft.Base;

public static class StiJsonHelper
{
	private static JsonSerializerSettings defaultSerializerSettings;

	public static JsonSerializerSettings DefaultSerializerSettings
	{
		get
		{
			object obj = defaultSerializerSettings;
			if (obj == null)
			{
				obj = new JsonSerializerSettings
				{
					Culture = new CultureInfo("en-US", useUserOverride: false),
					DefaultValueHandling = DefaultValueHandling.Populate,
					DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
					NullValueHandling = NullValueHandling.Ignore,
					ObjectCreationHandling = ObjectCreationHandling.Auto
				};
				defaultSerializerSettings = (JsonSerializerSettings)obj;
			}
			return (JsonSerializerSettings)obj;
		}
	}

	public static string SaveToJsonString(object value)
	{
		return JsonConvert.SerializeObject(value, Formatting.Indented, DefaultSerializerSettings);
	}

	public static void LoadFromJsonString(string str, object value)
	{
		JsonConvert.PopulateObject(str, value, DefaultSerializerSettings);
	}
}
