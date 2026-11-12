using System;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Management.Assessment.Properties;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Json;

internal class RangeConverter<T> : JsonConverter<Range<T>> where T : IComparable<T>
{
	public delegate bool ParserDelegate(string input, out T result);

	public const char OpeningInclusiveParenthesis = '[';

	public const char ClosingInclusiveParenthesis = ']';

	public const string LeftBracketGroupName = "leftBracket";

	public const string RightBracketGroupName = "rightBracket";

	public const string LeftGroupName = "left";

	public const string RightGroupName = "right";

	public const string CommaGroupName = "comma";

	private static readonly Regex ParsingPattern = new Regex("^\\s*(?<leftBracket>[([])?\\s*(?<left>[^,)\\] ]+)?\\s*(?<comma>,)?\\s*(?<right>[^,)\\] ]+)?\\s*(?<rightBracket>[)\\]])?\\s*$");

	private ParserDelegate Parser { get; }

	public RangeConverter(ParserDelegate parser)
	{
		Parser = parser;
	}

	public override void WriteJson(JsonWriter writer, Range<T> value, JsonSerializer serializer)
	{
		writer.WriteValue(value.ToString());
	}

	public override Range<T> ReadJson(JsonReader reader, Type objectType, Range<T> existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		switch (reader.TokenType)
		{
		case JsonToken.Null:
			return new Range<T>();
		case JsonToken.String:
		{
			string text = (string)reader.Value;
			Match match = ParsingPattern.Match(text);
			if (!match.Success)
			{
				throw SqlAssessmentJsonException.Create(reader, Resources.ErrorJsonWrongValue, text, "Range");
			}
			bool flag = !match.Groups["leftBracket"].Success || match.Groups["leftBracket"].Value[0] == '[';
			bool flag2 = !match.Groups["rightBracket"].Success || match.Groups["rightBracket"].Value[0] == ']';
			T val = ParseBoundary(match.Groups["left"], reader);
			T right;
			if (match.Groups["comma"].Success)
			{
				right = ParseBoundary(match.Groups["right"], reader);
			}
			else
			{
				if (!(flag && flag2))
				{
					throw SqlAssessmentJsonException.Create(reader, Resources.ErrorJsonSingularRangeInclusive, val, text);
				}
				right = val;
			}
			return new Range<T>(val, right, flag, flag2);
		}
		default:
			throw SqlAssessmentJsonException.Create(reader, Resources.ErrorJsonUnexpectedToken, reader.TokenType, "Range");
		}
	}

	private T ParseBoundary(Group representation, JsonReader reader)
	{
		T result = default(T);
		if (representation.Success)
		{
			Exception innerException = null;
			bool flag;
			try
			{
				flag = Parser(representation.Value, out result);
			}
			catch (Exception ex)
			{
				if (ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				innerException = ex;
				flag = false;
			}
			if (!flag)
			{
				throw SqlAssessmentJsonException.Create(reader, Resources.ErrorJsonWrongValue, representation.Value, "Range", innerException);
			}
		}
		return result;
	}
}
