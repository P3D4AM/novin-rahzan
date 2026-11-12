using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Json;

internal class SingleItemOrArrayConverter<TCollectionInterface, TCollection, TItem> : JsonConverter<TCollectionInterface> where TCollection : ICollection<TItem>, TCollectionInterface, new()
{
	public override bool CanWrite => false;

	public override void WriteJson(JsonWriter writer, TCollectionInterface value, JsonSerializer serializer)
	{
		throw new InvalidOperationException();
	}

	public override TCollectionInterface ReadJson(JsonReader reader, Type objectType, TCollectionInterface existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		TCollection val = new TCollection();
		if (reader.TokenType != JsonToken.StartArray)
		{
			TItem item = serializer.Deserialize<TItem>(reader);
			val.Add(item);
		}
		else
		{
			TItem[] array = serializer.Deserialize<TItem[]>(reader);
			foreach (TItem item2 in array)
			{
				val.Add(item2);
			}
		}
		return (TCollectionInterface)(object)val;
	}
}
