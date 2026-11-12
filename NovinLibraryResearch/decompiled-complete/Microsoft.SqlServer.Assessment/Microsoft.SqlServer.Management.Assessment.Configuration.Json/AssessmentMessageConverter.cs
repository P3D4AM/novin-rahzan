using System;
using System.Globalization;
using Microsoft.SqlServer.Management.Assessment.Properties;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Json;

internal class AssessmentMessageConverter : JsonConverter<ParameterizedText>
{
	public override bool CanWrite => false;

	public override ParameterizedText ReadJson(JsonReader reader, Type objectType, ParameterizedText existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		SerializationUtil.CheckTokenType(reader, JsonToken.String);
		ParameterizedText parameterizedText = (hasExistingValue ? existingValue : new ParameterizedText());
		parameterizedText.Template = serializer.Deserialize<string>(reader);
		return parameterizedText;
	}

	public override void WriteJson(JsonWriter writer, ParameterizedText value, JsonSerializer serializer)
	{
		throw new NotImplementedException(string.Format(CultureInfo.CurrentCulture, Resources.ErrorJsonSerializationNotSupported, "ParameterizedText"));
	}
}
