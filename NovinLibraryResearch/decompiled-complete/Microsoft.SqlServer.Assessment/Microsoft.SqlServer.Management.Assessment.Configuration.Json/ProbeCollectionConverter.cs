using System;
using System.Globalization;
using Microsoft.SqlServer.Management.Assessment.Probes;
using Microsoft.SqlServer.Management.Assessment.Properties;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Json;

internal class ProbeCollectionConverter : JsonConverter<ProbeCollection>
{
	public override bool CanWrite => false;

	public override ProbeCollection ReadJson(JsonReader reader, Type objectType, ProbeCollection existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		ProbeCollection probeCollection = (hasExistingValue ? existingValue : new ProbeCollection());
		reader.Read();
		while (reader.TokenType != JsonToken.EndObject)
		{
			string text = reader.Value as string;
			reader.Read();
			ProbeFamily probeFamily = new ProbeFamily(text);
			serializer.Populate(reader, probeFamily);
			reader.Read();
			probeCollection.Add(text, probeFamily);
		}
		return probeCollection;
	}

	public override void WriteJson(JsonWriter writer, ProbeCollection value, JsonSerializer serializer)
	{
		throw new NotImplementedException(string.Format(CultureInfo.CurrentCulture, Resources.ErrorJsonSerializationNotSupported, "ProbeCollection"));
	}
}
