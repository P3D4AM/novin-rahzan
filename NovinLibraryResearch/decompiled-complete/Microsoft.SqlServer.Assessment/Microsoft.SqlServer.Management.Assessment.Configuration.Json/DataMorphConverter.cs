using System;
using System.Globalization;
using Microsoft.SqlServer.Management.Assessment.Data.Morphs;
using Microsoft.SqlServer.Management.Assessment.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Json;

internal class DataMorphConverter : JsonConverter<IDataMorph>
{
	private const string TypePropertyName = "type";

	public override bool CanWrite => false;

	public override void WriteJson(JsonWriter writer, IDataMorph value, JsonSerializer serializer)
	{
		throw new NotImplementedException(string.Format(CultureInfo.CurrentCulture, Resources.ErrorJsonSerializationNotSupported, "IDataMorph"));
	}

	public override IDataMorph ReadJson(JsonReader reader, Type objectType, IDataMorph existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		return reader.TokenType switch
		{
			JsonToken.String => ReadMorphByName(reader, serializer), 
			JsonToken.StartArray => ReadCompositeMorph(reader, serializer), 
			JsonToken.StartObject => ReadMorphObject(reader, serializer), 
			_ => throw SqlAssessmentJsonException.Create(reader, Resources.ErrorStringArrayOrObjectExpected, reader.TokenType), 
		};
	}

	private static IDataMorph ReadMorphByName(JsonReader reader, JsonSerializer _1)
	{
		string morphCode = (string)reader.Value;
		return CreateMorph(morphCode, reader);
	}

	private static IDataMorph CreateMorph(string morphCode, JsonReader reader)
	{
		if (!SerializationUtil.DataMorphCodes.TryGetValue(morphCode, out var value) || value == null)
		{
			throw SqlAssessmentJsonException.Create(reader, Resources.ErrorJsonUnsupportedOperation, morphCode);
		}
		return value();
	}

	private static IDataMorph ReadMorphObject(JsonReader reader, JsonSerializer serializer)
	{
		JObject jObject = JObject.Load(reader);
		string propertyValue = SerializationUtil.GetPropertyValue<string>(jObject, "type", reader, serializer, allowNull: false, new JTokenType[1] { JTokenType.String });
		IDataMorph dataMorph = CreateMorph(propertyValue, reader);
		serializer.Populate(jObject.CreateReader(), dataMorph);
		return dataMorph;
	}

	private static IDataMorph ReadCompositeMorph(JsonReader reader, JsonSerializer serializer)
	{
		IDataMorph[] children = serializer.Deserialize<IDataMorph[]>(reader);
		return new CompositeDataMorph(children);
	}
}
