using System;
using System.Buffers;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.AspNetCore.Internal;

internal static class JsonUtils
{
	private class JsonArrayPool<T> : IArrayPool<T>
	{
		private readonly ArrayPool<T> _inner;

		internal static readonly JsonArrayPool<T> Shared = new JsonArrayPool<T>(ArrayPool<T>.Shared);

		public JsonArrayPool(ArrayPool<T> inner)
		{
			_inner = inner;
		}

		public T[] Rent(int minimumLength)
		{
			return _inner.Rent(minimumLength);
		}

		public void Return(T[] array)
		{
			if (array != null)
			{
				_inner.Return(array);
			}
		}
	}

	internal static JsonTextReader CreateJsonTextReader(TextReader textReader)
	{
		return new JsonTextReader(textReader)
		{
			ArrayPool = JsonArrayPool<char>.Shared,
			CloseInput = false
		};
	}

	internal static JsonTextWriter CreateJsonTextWriter(TextWriter textWriter)
	{
		return new JsonTextWriter(textWriter)
		{
			ArrayPool = JsonArrayPool<char>.Shared,
			CloseOutput = false,
			AutoCompleteOnClose = false
		};
	}

	public static JObject GetObject(JToken token)
	{
		if (token == null || token.Type != JTokenType.Object)
		{
			throw new InvalidDataException($"Unexpected JSON Token Type '{token?.Type}'. Expected a JSON Object.");
		}
		return (JObject)token;
	}

	public static T GetRequiredProperty<T>(JObject json, string property, JTokenType expectedType = JTokenType.None)
	{
		JToken jToken = json[property];
		if (jToken == null)
		{
			throw new InvalidDataException("Missing required property '" + property + "'.");
		}
		return GetValue<T>(property, expectedType, jToken);
	}

	public static T GetValue<T>(string property, JTokenType expectedType, JToken prop)
	{
		if (expectedType != 0 && prop.Type != expectedType)
		{
			throw new InvalidDataException($"Expected '{property}' to be of type {expectedType}.");
		}
		return prop.Value<T>();
	}

	public static string GetTokenString(JsonToken tokenType)
	{
		return tokenType switch
		{
			JsonToken.StartObject => JTokenType.Object.ToString(), 
			JsonToken.StartArray => JTokenType.Array.ToString(), 
			JsonToken.PropertyName => JTokenType.Property.ToString(), 
			_ => tokenType.ToString(), 
		};
	}

	public static void EnsureObjectStart(JsonTextReader reader)
	{
		if (reader.TokenType != JsonToken.StartObject)
		{
			throw new InvalidDataException("Unexpected JSON Token Type '" + GetTokenString(reader.TokenType) + "'. Expected a JSON Object.");
		}
	}

	public static void EnsureArrayStart(JsonTextReader reader)
	{
		if (reader.TokenType != JsonToken.StartArray)
		{
			throw new InvalidDataException("Unexpected JSON Token Type '" + GetTokenString(reader.TokenType) + "'. Expected a JSON Array.");
		}
	}

	public static bool ReadAsBoolean(JsonTextReader reader, string propertyName)
	{
		reader.Read();
		if (reader.TokenType != JsonToken.Boolean || reader.Value == null)
		{
			throw new InvalidDataException($"Expected '{propertyName}' to be of type {JTokenType.Boolean}.");
		}
		return Convert.ToBoolean(reader.Value, CultureInfo.InvariantCulture);
	}

	public static int? ReadAsInt32(JsonTextReader reader, string propertyName)
	{
		reader.Read();
		if (reader.TokenType != JsonToken.Integer)
		{
			throw new InvalidDataException($"Expected '{propertyName}' to be of type {JTokenType.Integer}.");
		}
		if (reader.Value == null)
		{
			return null;
		}
		return Convert.ToInt32(reader.Value, CultureInfo.InvariantCulture);
	}

	public static string ReadAsString(JsonTextReader reader, string propertyName)
	{
		reader.Read();
		if (reader.TokenType != JsonToken.String)
		{
			throw new InvalidDataException($"Expected '{propertyName}' to be of type {JTokenType.String}.");
		}
		return reader.Value?.ToString();
	}

	public static bool CheckRead(JsonTextReader reader)
	{
		if (!reader.Read())
		{
			throw new InvalidDataException("Unexpected end when reading JSON.");
		}
		return true;
	}

	public static bool ReadForType(JsonTextReader reader, Type type)
	{
		if (type == typeof(DateTime) || type == typeof(DateTime?))
		{
			reader.ReadAsDateTime();
		}
		else if (type == typeof(DateTimeOffset) || type == typeof(DateTimeOffset?))
		{
			reader.ReadAsDateTimeOffset();
		}
		else
		{
			reader.Read();
		}
		return reader.TokenType != JsonToken.None;
	}
}
