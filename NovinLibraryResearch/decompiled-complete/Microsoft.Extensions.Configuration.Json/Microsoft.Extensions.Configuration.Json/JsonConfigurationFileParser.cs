using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Microsoft.Extensions.Configuration.Json;

internal sealed class JsonConfigurationFileParser
{
	private readonly Dictionary<string, string> _data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

	private readonly Stack<string> _paths = new Stack<string>();

	private JsonConfigurationFileParser()
	{
	}

	public static IDictionary<string, string> Parse(Stream input)
	{
		return new JsonConfigurationFileParser().ParseStream(input);
	}

	private IDictionary<string, string> ParseStream(Stream input)
	{
		JsonDocumentOptions jsonDocumentOptions = default(JsonDocumentOptions);
		jsonDocumentOptions.CommentHandling = JsonCommentHandling.Skip;
		jsonDocumentOptions.AllowTrailingCommas = true;
		JsonDocumentOptions options = jsonDocumentOptions;
		using (StreamReader streamReader = new StreamReader(input))
		{
			using JsonDocument jsonDocument = JsonDocument.Parse(streamReader.ReadToEnd(), options);
			if (jsonDocument.RootElement.ValueKind != JsonValueKind.Object)
			{
				throw new FormatException(System.SR.Format(System.SR.Error_InvalidTopLevelJSONElement, jsonDocument.RootElement.ValueKind));
			}
			VisitElement(jsonDocument.RootElement);
		}
		return _data;
	}

	private void VisitElement(JsonElement element)
	{
		bool flag = true;
		foreach (JsonProperty item in element.EnumerateObject())
		{
			flag = false;
			EnterContext(item.Name);
			VisitValue(item.Value);
			ExitContext();
		}
		if (flag && _paths.Count > 0)
		{
			_data[_paths.Peek()] = null;
		}
	}

	private void VisitValue(JsonElement value)
	{
		switch (value.ValueKind)
		{
		case JsonValueKind.Object:
			VisitElement(value);
			break;
		case JsonValueKind.Array:
		{
			int num = 0;
			{
				foreach (JsonElement item in value.EnumerateArray())
				{
					EnterContext(num.ToString());
					VisitValue(item);
					ExitContext();
					num++;
				}
				break;
			}
		}
		case JsonValueKind.String:
		case JsonValueKind.Number:
		case JsonValueKind.True:
		case JsonValueKind.False:
		case JsonValueKind.Null:
		{
			string text = _paths.Peek();
			if (_data.ContainsKey(text))
			{
				throw new FormatException(System.SR.Format(System.SR.Error_KeyIsDuplicated, text));
			}
			_data[text] = value.ToString();
			break;
		}
		default:
			throw new FormatException(System.SR.Format(System.SR.Error_UnsupportedJSONToken, value.ValueKind));
		}
	}

	private void EnterContext(string context)
	{
		_paths.Push((_paths.Count > 0) ? (_paths.Peek() + ConfigurationPath.KeyDelimiter + context) : context);
	}

	private void ExitContext()
	{
		_paths.Pop();
	}
}
