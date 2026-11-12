using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.SqlServer.Management.Assessment.Checks;
using Microsoft.SqlServer.Management.Assessment.Expressions;
using Microsoft.SqlServer.Management.Assessment.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Json;

internal class ProbeReferenceConverter : JsonConverter<ProbeReference>
{
	public const string IdPropertyName = "id";

	public const string ParametersPropertyName = "params";

	public const string AliasPropertyName = "alias";

	public const string TransformPropertyName = "transform";

	public override bool CanWrite => false;

	public override void WriteJson(JsonWriter writer, ProbeReference value, JsonSerializer serializer)
	{
		throw new NotImplementedException(string.Format(CultureInfo.CurrentCulture, Resources.ErrorJsonSerializationNotSupported, "ProbeReference"));
	}

	public override ProbeReference ReadJson(JsonReader reader, Type objectType, ProbeReference existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		switch (reader.TokenType)
		{
		case JsonToken.StartObject:
		{
			JObject parent = JObject.Load(reader);
			string propertyValue = SerializationUtil.GetPropertyValue<string>(parent, "id", reader, allowNull: false, new JTokenType[1] { JTokenType.String });
			string propertyValue2 = SerializationUtil.GetPropertyValue<string>(parent, "alias", reader, allowNull: true, new JTokenType[1] { JTokenType.String });
			Dictionary<string, Expression> propertyValue3 = SerializationUtil.GetPropertyValue<Dictionary<string, Expression>>(parent, "params", reader, serializer, allowNull: true, new JTokenType[1] { JTokenType.Object });
			IDataMorph propertyValue4 = SerializationUtil.GetPropertyValue<IDataMorph>(parent, "transform", reader, serializer, allowNull: true, new JTokenType[2]
			{
				JTokenType.Object,
				JTokenType.Array
			});
			return new ProbeReference(propertyValue, propertyValue3, propertyValue2, propertyValue4);
		}
		case JsonToken.String:
		{
			string probeName = reader.Value as string;
			return new ProbeReference(probeName);
		}
		default:
			throw new InvalidDataException();
		}
	}
}
