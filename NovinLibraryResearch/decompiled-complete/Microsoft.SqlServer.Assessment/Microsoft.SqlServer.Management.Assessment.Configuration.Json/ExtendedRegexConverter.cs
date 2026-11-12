using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Management.Assessment.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Json;

internal class ExtendedRegexConverter : JsonConverter<IPattern<string>>
{
	private const char RegexSign = '/';

	private const string ComplementarySign = "not";

	public override bool CanWrite => false;

	public override void WriteJson(JsonWriter writer, IPattern<string> value, JsonSerializer serializer)
	{
		throw new NotImplementedException(string.Format(CultureInfo.CurrentCulture, Resources.ErrorJsonSerializationNotSupported, "IPattern"));
	}

	public override IPattern<string> ReadJson(JsonReader reader, Type objectType, IPattern<string> existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		if (reader.TokenType == JsonToken.StartArray)
		{
			IPattern<string>[] items = serializer.Deserialize<IPattern<string>[]>(reader);
			return new PatternList<string>(items);
		}
		if (reader.TokenType != JsonToken.StartObject)
		{
			return ReadPattern(reader, serializer);
		}
		JObject jObject = JObject.Load(reader);
		if (jObject.TryGetValue("not", out JToken value))
		{
			JsonReader jsonReader = value.CreateReader();
			jsonReader.Read();
			IPattern<string> original = ReadJson(jsonReader, objectType, null, hasExistingValue: false, serializer);
			return new ComplementaryPattern<string>(original);
		}
		return ReadPattern(jObject.CreateReader(), serializer);
	}

	private static IPattern<string> ReadPattern(JsonReader reader, JsonSerializer serializer)
	{
		bool flag = false;
		if (reader.TokenType == JsonToken.String)
		{
			string text = (string)reader.Value;
			int num = text.LastIndexOf('/');
			if (text[0] != '/' || num < 0)
			{
				return new ExactStringPattern(text);
			}
			flag = num > 0 && text.IndexOf('c', num) > 0;
		}
		Regex regex = serializer.Deserialize<Regex>(reader);
		if (!flag && !regex.Options.HasFlag(RegexOptions.IgnoreCase))
		{
			regex = new Regex(regex.ToString(), regex.Options | RegexOptions.IgnoreCase);
		}
		return new RegexPattern(regex);
	}
}
