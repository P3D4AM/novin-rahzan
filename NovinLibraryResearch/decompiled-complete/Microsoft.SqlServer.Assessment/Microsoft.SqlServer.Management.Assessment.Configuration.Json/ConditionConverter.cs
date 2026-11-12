using System;
using System.Globalization;
using Microsoft.SqlServer.Management.Assessment.Expressions;
using Microsoft.SqlServer.Management.Assessment.Properties;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Json;

internal class ConditionConverter : JsonConverter<Condition>
{
	public override bool CanWrite => false;

	public override void WriteJson(JsonWriter writer, Condition value, JsonSerializer serializer)
	{
		throw new NotImplementedException(string.Format(CultureInfo.CurrentCulture, Resources.ErrorJsonSerializationNotSupported, "Condition"));
	}

	public override Condition ReadJson(JsonReader reader, Type objectType, Condition existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		Expression implementation;
		switch (reader.TokenType)
		{
		case JsonToken.StartObject:
			implementation = serializer.Deserialize<Expression>(reader);
			break;
		case JsonToken.StartArray:
		{
			Expression[] array = serializer.Deserialize<Expression[]>(reader);
			implementation = ((array.LongLength == 0L) ? ((Expression)NullLiteralExpression.Instance) : ((Expression)new OrExpression(array)));
			break;
		}
		default:
			implementation = serializer.Deserialize<Expression>(reader);
			break;
		}
		return new Condition(implementation);
	}
}
