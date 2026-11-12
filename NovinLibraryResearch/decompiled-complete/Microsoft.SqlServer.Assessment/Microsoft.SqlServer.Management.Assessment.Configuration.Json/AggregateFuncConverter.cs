using System;
using System.Globalization;
using Microsoft.SqlServer.Management.Assessment.Data;
using Microsoft.SqlServer.Management.Assessment.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Json;

internal class AggregateFuncConverter : JsonConverter<IAggregateFunc>
{
	private const string TypePropertyName = "type";

	public override bool CanWrite => false;

	public override void WriteJson(JsonWriter writer, IAggregateFunc value, JsonSerializer serializer)
	{
		throw new NotImplementedException(string.Format(CultureInfo.CurrentCulture, Resources.ErrorJsonSerializationNotSupported, "IAggregateFunc"));
	}

	public override IAggregateFunc ReadJson(JsonReader reader, Type objectType, IAggregateFunc existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		return reader.TokenType switch
		{
			JsonToken.String => ReadFuncByName(reader, serializer), 
			JsonToken.StartObject => ReadAggregateFuncObject(reader, serializer), 
			_ => throw SqlAssessmentJsonException.Create(reader, Resources.ErrorStringOrObjectExpected, reader.TokenType), 
		};
	}

	private static IAggregateFunc CreateMorph(string funcCode, JsonReader reader)
	{
		if (!SerializationUtil.AggregateFuncCodes.TryGetValue(funcCode, out var value) || value == null)
		{
			throw SqlAssessmentJsonException.Create(reader, Resources.ErrorJsonUnsupportedOperation, funcCode);
		}
		return value();
	}

	private static IAggregateFunc ReadAggregateFuncObject(JsonReader reader, JsonSerializer serializer)
	{
		JObject jObject = JObject.Load(reader);
		string propertyValue = SerializationUtil.GetPropertyValue<string>(jObject, "type", reader, serializer, allowNull: false, new JTokenType[1] { JTokenType.String });
		IAggregateFunc aggregateFunc = CreateMorph(propertyValue, reader);
		serializer.Populate(jObject.CreateReader(), aggregateFunc);
		return aggregateFunc;
	}

	private static IAggregateFunc ReadFuncByName(JsonReader reader, JsonSerializer _1)
	{
		string funcCode = (string)reader.Value;
		return CreateMorph(funcCode, reader);
	}
}
