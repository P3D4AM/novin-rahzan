using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Management.Assessment.Data;
using Microsoft.SqlServer.Management.Assessment.Data.AggregateFuncs;
using Microsoft.SqlServer.Management.Assessment.Data.Morphs;
using Microsoft.SqlServer.Management.Assessment.Data.Morphs.Indexes;
using Microsoft.SqlServer.Management.Assessment.Expressions;
using Microsoft.SqlServer.Management.Assessment.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Json;

internal static class SerializationUtil
{
	public static readonly JsonConverter[] StandardConverters = new JsonConverter[14]
	{
		new ExtendedRegexConverter(),
		new ProbeConverter(),
		new ConditionConverter(),
		new RangeConverter<Version>(Version.TryParse),
		new RangeListConverter<Version>(),
		new ExpressionConverter(),
		new ProbeCollectionConverter(),
		new ProbeReferenceConverter(),
		new AssessmentMessageConverter(),
		new DataMorphConverter(),
		new AggregateFuncConverter(),
		new GrouperConverter(),
		new CheckModificationConverter(),
		new SingleItemOrArrayConverter<List<Regex>, List<Regex>, Regex>()
	};

	public static readonly Dictionary<string, Func<IEnumerable<Expression>, Expression>> ExpressionTypeCodes = new Dictionary<string, Func<IEnumerable<Expression>, Expression>>
	{
		{
			"less",
			(IEnumerable<Expression> args) => new LessExpression(args)
		},
		{
			"greater",
			(IEnumerable<Expression> args) => new GreaterExpression(args)
		},
		{
			"equal",
			(IEnumerable<Expression> args) => new EqualsExpression(args)
		},
		{
			"lt",
			(IEnumerable<Expression> args) => new LessExpression(args)
		},
		{
			"gt",
			(IEnumerable<Expression> args) => new GreaterExpression(args)
		},
		{
			"eq",
			(IEnumerable<Expression> args) => new EqualsExpression(args)
		},
		{
			"ieq",
			(IEnumerable<Expression> args) => new EqualsExpression(StringComparison.OrdinalIgnoreCase, args)
		},
		{
			"in",
			(IEnumerable<Expression> args) => new InExpression(args)
		},
		{
			"iin",
			(IEnumerable<Expression> args) => new InExpression(StringComparison.OrdinalIgnoreCase, args)
		},
		{
			"not",
			(IEnumerable<Expression> args) => new NotExpression(args)
		},
		{
			"or",
			(IEnumerable<Expression> args) => new OrExpression(args)
		},
		{
			"and",
			(IEnumerable<Expression> args) => new AndExpression(args)
		},
		{
			"add",
			(IEnumerable<Expression> args) => new DecimalAssociativeExpression(decimal.Add, args)
		},
		{
			"sub",
			(IEnumerable<Expression> args) => new DecimalBinaryExpression(decimal.Subtract, args)
		},
		{
			"mul",
			(IEnumerable<Expression> args) => new DecimalAssociativeExpression(decimal.Multiply, args)
		},
		{
			"div",
			(IEnumerable<Expression> args) => new DecimalBinaryExpression(decimal.Divide, args)
		},
		{
			"mod",
			(IEnumerable<Expression> args) => new DecimalBinaryExpression(decimal.Remainder, args)
		},
		{
			"bitand",
			(IEnumerable<Expression> args) => new DecimalAssociativeExpression(Numeric.BitAnd, args)
		},
		{
			"bitor",
			(IEnumerable<Expression> args) => new DecimalAssociativeExpression(Numeric.BitOr, args)
		},
		{
			"bitxor",
			(IEnumerable<Expression> args) => new DecimalAssociativeExpression(Numeric.BitXor, args)
		},
		{
			"greaterequal",
			(IEnumerable<Expression> args) => new NonstrictComparisonExpression<decimal>(new GreaterExpression(args))
		},
		{
			"lessequal",
			(IEnumerable<Expression> args) => new NonstrictComparisonExpression<decimal>(new LessExpression(args))
		},
		{
			"notequal",
			(IEnumerable<Expression> args) => new NotExpression(new EqualsExpression[1]
			{
				new EqualsExpression(args)
			})
		},
		{
			"ge",
			(IEnumerable<Expression> args) => new NonstrictComparisonExpression<decimal>(new GreaterExpression(args))
		},
		{
			"le",
			(IEnumerable<Expression> args) => new NonstrictComparisonExpression<decimal>(new LessExpression(args))
		},
		{
			"ne",
			(IEnumerable<Expression> args) => new NotExpression(new EqualsExpression[1]
			{
				new EqualsExpression(args)
			})
		},
		{
			"ine",
			(IEnumerable<Expression> args) => new NotExpression(new EqualsExpression[1]
			{
				new EqualsExpression(StringComparison.OrdinalIgnoreCase, args)
			})
		},
		{
			"intersect",
			(IEnumerable<Expression> args) => new IntersectExpression(args)
		},
		{
			"ceiling",
			(IEnumerable<Expression> args) => new UnaryFuncExpression(decimal.Ceiling, args)
		},
		{
			"floor",
			(IEnumerable<Expression> args) => new UnaryFuncExpression(decimal.Floor, args)
		},
		{
			"match",
			(IEnumerable<Expression> args) => new MatchExpression(args)
		},
		{
			"imatch",
			(IEnumerable<Expression> args) => new MatchExpression(RegexOptions.IgnoreCase, args)
		},
		{
			"indexof",
			(IEnumerable<Expression> args) => new StringBinaryExpression<int>((string l, string r, StringComparison c) => l.IndexOf(r, c), args, -1)
		},
		{
			"iindexof",
			(IEnumerable<Expression> args) => new StringBinaryExpression<int>((string l, string r, StringComparison c) => l.IndexOf(r, c), StringComparison.OrdinalIgnoreCase, args, -1)
		},
		{
			"startswith",
			(IEnumerable<Expression> args) => new StringBinaryExpression<bool>((string l, string r, StringComparison c) => l.StartsWith(r, c), args, nullParamResult: false)
		},
		{
			"istartswith",
			(IEnumerable<Expression> args) => new StringBinaryExpression<bool>((string l, string r, StringComparison c) => l.StartsWith(r, c), StringComparison.OrdinalIgnoreCase, args, nullParamResult: false)
		},
		{
			"endswith",
			(IEnumerable<Expression> args) => new StringBinaryExpression<bool>((string l, string r, StringComparison c) => l.EndsWith(r, c), args, nullParamResult: false)
		},
		{
			"iendswith",
			(IEnumerable<Expression> args) => new StringBinaryExpression<bool>((string l, string r, StringComparison c) => l.EndsWith(r, c), StringComparison.OrdinalIgnoreCase, args, nullParamResult: false)
		},
		{
			"interval",
			(IEnumerable<Expression> args) => new IntervalExpression(args)
		},
		{
			"min",
			(IEnumerable<Expression> args) => new DecimalAssociativeExpression(Math.Min, args)
		},
		{
			"max",
			(IEnumerable<Expression> args) => new DecimalAssociativeExpression(Math.Max, args)
		}
	};

	public static readonly Dictionary<string, Func<IDataMorph>> DataMorphCodes = new Dictionary<string, Func<IDataMorph>>
	{
		{
			"aggregate",
			() => new AggregateProbeMorph()
		},
		{
			"rename",
			() => new RenameDataMorph()
		},
		{
			"indexDefinition",
			() => new IndexDefinitionMorph()
		},
		{
			"redundantIndexes",
			() => new RedundantIndexesMorph()
		},
		{
			"duplicateIndexes",
			() => new DuplicateIndexesMorph()
		},
		{
			"dbInfo",
			() => new DbInfoMorph()
		},
		{
			"noData",
			() => new NoDataMorph()
		},
		{
			"nameValuePairs",
			() => new NameValuePairsMorph()
		},
		{
			"parse",
			() => new StringFieldParseMorph()
		},
		{
			"toString",
			() => new ToStringMorph()
		},
		{
			"defaultValue",
			() => new DefaultValueMorph()
		}
	};

	public static readonly Dictionary<string, Func<IAggregateFunc>> AggregateFuncCodes = new Dictionary<string, Func<IAggregateFunc>>
	{
		{
			"join",
			() => new StringJoinFunc()
		},
		{
			"array",
			() => default(ArrayAggregateFunc)
		},
		{
			"count",
			() => new CountFunc()
		},
		{
			"min",
			() => new MinimumFunc()
		},
		{
			"max",
			() => new MaximumFunc()
		}
	};

	public static JsonSerializer CreateJsonSerializer()
	{
		JsonSerializerSettings settings = new JsonSerializerSettings
		{
			PreserveReferencesHandling = PreserveReferencesHandling.All,
			DefaultValueHandling = DefaultValueHandling.Ignore,
			NullValueHandling = NullValueHandling.Ignore,
			Converters = StandardConverters
		};
		return JsonSerializer.Create(settings);
	}

	internal static JToken GetPropertyObject(JObject rep, string propertyName, JTokenType tokenType, JsonReader reader)
	{
		JToken jToken = rep[propertyName];
		if (jToken == null)
		{
			throw SqlAssessmentJsonException.Create(reader, Resources.ErrorJsonMissingProperty, propertyName);
		}
		if (jToken.Type != tokenType)
		{
			throw SqlAssessmentJsonException.Create(reader, Resources.ErrorJsonWrongToken, jToken.Type, tokenType);
		}
		return jToken;
	}

	internal static TResult GetPropertyValue<TResult>(JToken parent, string propertyName, JsonReader reader, bool allowNull, params JTokenType[] tokenTypes)
	{
		JToken jToken = parent[propertyName];
		if (jToken == null)
		{
			if (!allowNull)
			{
				throw SqlAssessmentJsonException.Create(reader, Resources.ErrorJsonMissingProperty, propertyName);
			}
			return default(TResult);
		}
		if (Array.IndexOf(tokenTypes, jToken.Type) < 0)
		{
			throw SqlAssessmentJsonException.Create(reader, Resources.ErrorJsonMissingProperty, propertyName);
		}
		return jToken.Value<TResult>();
	}

	internal static TResult GetPropertyValue<TResult>(JToken parent, string propertyName, JsonReader reader, JsonSerializer serializer, bool allowNull, params JTokenType[] tokenTypes)
	{
		JToken jToken = parent[propertyName];
		if (jToken == null)
		{
			if (!allowNull)
			{
				throw SqlAssessmentJsonException.Create(reader, Resources.ErrorJsonMissingProperty, propertyName);
			}
			return default(TResult);
		}
		if (Array.IndexOf(tokenTypes, jToken.Type) < 0)
		{
			throw SqlAssessmentJsonException.Create(reader, Resources.ErrorJsonMissingProperty, propertyName);
		}
		return serializer.Deserialize<TResult>(jToken.CreateReader());
	}

	internal static void CheckTokenType(JsonReader reader, params JsonToken[] expected)
	{
		if (!((IList)expected).Contains((object?)reader.TokenType))
		{
			throw CreateWrongTokenException(reader, expected);
		}
	}

	internal static SqlAssessmentJsonException CreateWrongTokenException(JsonReader reader, params JsonToken[] expected)
	{
		return SqlAssessmentJsonException.Create(reader, Resources.ErrorJsonWrongToken, reader.TokenType, string.Join(" or ", expected));
	}

	internal static void CheckNull(object value, JsonReader reader, string message)
	{
		if (value == null)
		{
			throw SqlAssessmentJsonException.Create(reader, message);
		}
	}

	internal static void CheckNull(object value, JsonReader reader, string message, object arg0)
	{
		if (value == null)
		{
			throw SqlAssessmentJsonException.Create(reader, message, arg0);
		}
	}

	internal static void CheckNull(object value, JsonReader reader, string message, object arg0, object arg1)
	{
		if (value == null)
		{
			throw SqlAssessmentJsonException.Create(reader, message, arg0, arg1);
		}
	}
}
