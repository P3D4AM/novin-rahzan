using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Json;

internal class FlatMeasurementJsonConverter : JsonConverter<IDataRow>
{
	public override bool CanWrite => false;

	public override void WriteJson(JsonWriter writer, IDataRow value, JsonSerializer serializer)
	{
		throw new InvalidOperationException();
	}

	public override IDataRow ReadJson(JsonReader reader, Type objectType, IDataRow existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}

	public static IList<JsonConverter> GetConverters()
	{
		return new List<JsonConverter>
		{
			new FlatMeasurementJsonConverter(),
			new SingleItemOrArrayConverter<IDataView, MeasurementCollection, IDataRow>()
		};
	}
}
