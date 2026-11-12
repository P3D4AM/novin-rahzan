using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.SqlServer.Management.Assessment.Expressions;
using Microsoft.SqlServer.Management.Assessment.Properties;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Json;

internal class ExpressionConverter : JsonConverter<Expression>
{
	public override bool CanWrite => false;

	public override void WriteJson(JsonWriter writer, Expression value, JsonSerializer serializer)
	{
		throw new NotImplementedException(string.Format(CultureInfo.CurrentCulture, Resources.ErrorJsonSerializationNotSupported, "Expression"));
	}

	public override Expression ReadJson(JsonReader reader, Type objectType, Expression existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		return reader.TokenType switch
		{
			JsonToken.StartObject => ReadObject(reader, serializer), 
			JsonToken.Integer => ReadIntegerLiteral(reader, serializer), 
			JsonToken.Float => ReadFloatLiteral(reader, serializer), 
			JsonToken.String => ReadString(reader, serializer), 
			JsonToken.Boolean => ReadBooleanLiteral(reader, serializer), 
			JsonToken.Null => ReadNullLiteral(reader, serializer), 
			JsonToken.Date => ReadDateLiteral(reader, serializer), 
			JsonToken.PropertyName => ReadNamedExpression(reader, serializer), 
			JsonToken.StartArray => ReadArray(reader, serializer), 
			_ => throw SqlAssessmentJsonException.Create(reader, Resources.ErrorJsonUnexpectedToken, reader.TokenType, "Expression"), 
		};
	}

	private static IEnumerable<Expression> ReadSequence(Expression firstItem, JsonReader reader, JsonSerializer serializer, JsonToken endToken)
	{
		yield return firstItem;
		foreach (Expression item in ReadSequence(reader, serializer, endToken))
		{
			yield return item;
		}
	}

	private static IEnumerable<Expression> ReadSequence(JsonReader reader, JsonSerializer serializer, JsonToken endToken)
	{
		while (reader.TokenType != endToken)
		{
			yield return serializer.Deserialize<Expression>(reader);
			if (!reader.Read())
			{
				throw SqlAssessmentJsonException.Create(reader, Resources.ErrorJsonUnexpectedEndOfDocument);
			}
		}
	}

	private static Expression ReadArray(JsonReader reader, JsonSerializer serializer)
	{
		Expression[] source = serializer.Deserialize<Expression[]>(reader);
		return new ArrayExpression(source);
	}

	private static Expression ReadDateLiteral(JsonReader reader, JsonSerializer serializer)
	{
		SerializationUtil.CheckTokenType(reader, JsonToken.Date);
		return new Literal<DateTimeOffset>(serializer.Deserialize<DateTimeOffset>(reader));
	}

	private static Expression ReadNullLiteral(JsonReader _1, JsonSerializer _2)
	{
		return NullLiteralExpression.Instance;
	}

	private static Expression ReadBooleanLiteral(JsonReader reader, JsonSerializer serializer)
	{
		SerializationUtil.CheckTokenType(reader, JsonToken.Boolean);
		return new Literal<bool>(serializer.Deserialize<bool>(reader));
	}

	private static Expression ReadFloatLiteral(JsonReader reader, JsonSerializer serializer)
	{
		SerializationUtil.CheckTokenType(reader, JsonToken.Float);
		return new Literal<decimal>(serializer.Deserialize<decimal>(reader));
	}

	private static Expression ReadString(JsonReader reader, JsonSerializer _1)
	{
		SerializationUtil.CheckTokenType(reader, JsonToken.String);
		string text = (string)reader.Value;
		if (!string.IsNullOrEmpty(text) && text[0] == '@')
		{
			return new ColumnExpression(text.Substring(1));
		}
		return new Literal<string>(text);
	}

	private static IEnumerable<Expression> ReadSequence(JsonReader reader, JsonSerializer serializer)
	{
		if (reader.TokenType != JsonToken.StartArray)
		{
			yield return serializer.Deserialize<Expression>(reader);
			yield break;
		}
		Expression[] array = serializer.Deserialize<Expression[]>(reader);
		for (int i = 0; i < array.Length; i++)
		{
			yield return array[i];
		}
	}

	private static Expression ReadIntegerLiteral(JsonReader reader, JsonSerializer serializer)
	{
		SerializationUtil.CheckTokenType(reader, JsonToken.Integer);
		return new Literal<decimal>(serializer.Deserialize<decimal>(reader));
	}

	private static Expression ReadObject(JsonReader reader, JsonSerializer serializer)
	{
		reader.Read();
		if (reader.TokenType == JsonToken.EndObject)
		{
			return NullLiteralExpression.Instance;
		}
		Expression expression = serializer.Deserialize<Expression>(reader);
		reader.Read();
		if (reader.TokenType == JsonToken.EndObject)
		{
			return expression;
		}
		return new AndExpression(ReadSequence(expression, reader, serializer, JsonToken.EndObject));
	}

	private static Expression ReadNamedExpression(JsonReader reader, JsonSerializer serializer)
	{
		string operationCode = (string)reader.Value;
		if (operationCode[0] == '@')
		{
			return new EqualsExpression(ReadOperands());
		}
		if (!SerializationUtil.ExpressionTypeCodes.TryGetValue(operationCode, out var value))
		{
			throw SqlAssessmentJsonException.Create(reader, Resources.ErrorJsonUnsupportedOperation, operationCode);
		}
		reader.Read();
		List<Expression> arg = ReadSequence(reader, serializer).ToList();
		return value(arg);
		IEnumerable<Expression> ReadOperands()
		{
			yield return new ColumnExpression(operationCode.Substring(1));
			reader.Read();
			yield return serializer.Deserialize<Expression>(reader);
		}
	}
}
