using System;
using System.Globalization;
using Microsoft.SqlServer.Management.Assessment.Checks;
using Microsoft.SqlServer.Management.Assessment.Configuration.Customization;
using Microsoft.SqlServer.Management.Assessment.Properties;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Json;

internal class CheckModificationConverter : JsonConverter<CheckRule>
{
	public override bool CanWrite => false;

	public override void WriteJson(JsonWriter writer, CheckRule value, JsonSerializer serializer)
	{
		throw new NotImplementedException(string.Format(CultureInfo.CurrentCulture, Resources.ErrorJsonSerializationNotSupported, "CheckRule"));
	}

	public override CheckRule ReadJson(JsonReader reader, Type objectType, CheckRule existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		SerializationUtil.CheckTokenType(reader, JsonToken.StartObject);
		CheckModificationBuilder checkModificationBuilder = new CheckModificationBuilder
		{
			Reader = reader
		};
		while (reader.Read() && reader.TokenType != JsonToken.EndObject)
		{
			SerializationUtil.CheckTokenType(reader, JsonToken.PropertyName);
			switch ((string)reader.Value)
			{
			case "id":
				checkModificationBuilder.Selector = ReadSelector(reader, serializer);
				break;
			case "itemType":
				checkModificationBuilder.ItemType = reader.ReadAsString();
				break;
			case "targetFilter":
				reader.Read();
				checkModificationBuilder.TargetFilter = serializer.Deserialize<SqlObjectLocatorPattern>(reader);
				break;
			case "target":
				reader.Read();
				checkModificationBuilder.Target = serializer.Deserialize<SqlObjectLocatorPattern>(reader);
				break;
			default:
				checkModificationBuilder.Children.Add(ReadChange(reader, serializer));
				break;
			}
		}
		return checkModificationBuilder.Render();
	}

	private static ItemChange<Check, TValue> ReadReplaceItemChange<TValue>(JsonReader reader, JsonSerializer serializer, IChangeAccessor<Check, TValue> accessor)
	{
		OverwriteTransform<TValue> transform = new OverwriteTransform<TValue>(serializer.Deserialize<TValue>(reader));
		return new ItemChange<Check, TValue>(accessor, transform);
	}

	private static ItemChange<Check, IPattern<ISqlObjectLocator>> ReadTargetChange(JsonReader reader, JsonSerializer serializer, IChangeAccessor<Check, IPattern<ISqlObjectLocator>> accessor)
	{
		SqlObjectLocatorPattern value = serializer.Deserialize<SqlObjectLocatorPattern>(reader);
		OverwriteTransform<IPattern<ISqlObjectLocator>> transform = new OverwriteTransform<IPattern<ISqlObjectLocator>>(value);
		return new ItemChange<Check, IPattern<ISqlObjectLocator>>(accessor, transform);
	}

	private static IItemChange<Check> ReadChange(JsonReader reader, JsonSerializer serializer)
	{
		string text = (string)reader.Value;
		reader.Read();
		return text switch
		{
			"target" => ReadTargetChange(reader, serializer, CheckTargetAccessor.Instance), 
			"displayName" => ReadReplaceItemChange(reader, serializer, CheckDisplayNameAccessor.Instance), 
			"description" => ReadReplaceItemChange(reader, serializer, CheckDescriptionAccessor.Instance), 
			"message" => ReadReplaceItemChange(reader, serializer, CheckMessageAccessor.Instance), 
			"helpLink" => ReadReplaceItemChange(reader, serializer, CheckHelpLinkAccessor.Instance), 
			"level" => ReadReplaceItemChange(reader, serializer, CheckSeverityAccessor.Instance), 
			"probes" => ReadReplaceItemChange(reader, serializer, CheckProbesAccessor.Instance), 
			"condition" => ReadReplaceItemChange(reader, serializer, CheckConditionAccessor.Instance), 
			"enabled" => ReadReplaceItemChange(reader, serializer, CheckEnabledAccessor.Instance), 
			"tags" => ReadReplaceItemChange(reader, serializer, CheckTagsAccessor.Instance), 
			"locals" => ReadReplaceItemChange(reader, serializer, CheckLocalsAccessor.Instance), 
			_ => ReadReplaceItemChange(reader, serializer, new CheckParameterAccessor(text)), 
		};
	}

	private static CheckSelector ReadSelector(JsonReader reader, JsonSerializer serializer)
	{
		reader.Read();
		SerializationUtil.CheckTokenType(reader, JsonToken.String, JsonToken.StartArray);
		switch (reader.TokenType)
		{
		case JsonToken.String:
		{
			string checkId = (string)reader.Value;
			return new CheckByIdSelector(checkId, reader);
		}
		case JsonToken.StartArray:
		{
			string[] tags = serializer.Deserialize<string[]>(reader);
			return new CheckByTagSelector(tags);
		}
		default:
			throw SqlAssessmentJsonException.Create(reader, Resources.ErrorJsonWrongToken, reader.TokenType, "string, string[]");
		}
	}
}
