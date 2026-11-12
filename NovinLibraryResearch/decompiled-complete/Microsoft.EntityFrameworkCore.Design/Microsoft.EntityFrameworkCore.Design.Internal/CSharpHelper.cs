using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Design.Internal;

public class CSharpHelper : ICSharpHelper
{
	private readonly IRelationalTypeMappingSource _relationalTypeMappingSource;

	private static readonly IReadOnlyDictionary<Type, string> _builtInTypes = new Dictionary<Type, string>
	{
		{
			typeof(bool),
			"bool"
		},
		{
			typeof(byte),
			"byte"
		},
		{
			typeof(sbyte),
			"sbyte"
		},
		{
			typeof(char),
			"char"
		},
		{
			typeof(short),
			"short"
		},
		{
			typeof(int),
			"int"
		},
		{
			typeof(long),
			"long"
		},
		{
			typeof(ushort),
			"ushort"
		},
		{
			typeof(uint),
			"uint"
		},
		{
			typeof(ulong),
			"ulong"
		},
		{
			typeof(decimal),
			"decimal"
		},
		{
			typeof(float),
			"float"
		},
		{
			typeof(double),
			"double"
		},
		{
			typeof(string),
			"string"
		},
		{
			typeof(object),
			"object"
		}
	};

	private static readonly IReadOnlyCollection<string> _keywords = (IReadOnlyCollection<string>)(object)new string[81]
	{
		"__arglist", "__makeref", "__reftype", "__refvalue", "abstract", "as", "base", "bool", "break", "byte",
		"case", "catch", "char", "checked", "class", "const", "continue", "decimal", "default", "delegate",
		"do", "double", "else", "enum", "event", "explicit", "extern", "false", "finally", "fixed",
		"float", "for", "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal",
		"is", "lock", "long", "namespace", "new", "null", "object", "operator", "out", "override",
		"params", "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed", "short",
		"sizeof", "stackalloc", "static", "string", "struct", "switch", "this", "throw", "true", "try",
		"typeof", "uint", "ulong", "unchecked", "unsafe", "ushort", "using", "virtual", "void", "volatile",
		"while"
	};

	private static readonly IReadOnlyDictionary<Type, Func<CSharpHelper, object, string>> _literalFuncs = new Dictionary<Type, Func<CSharpHelper, object, string>>
	{
		{
			typeof(bool),
			(CSharpHelper c, object v) => c.Literal((bool)v)
		},
		{
			typeof(byte),
			(CSharpHelper c, object v) => c.Literal((byte)v)
		},
		{
			typeof(byte[]),
			(CSharpHelper c, object v) => c.Literal((byte[])v)
		},
		{
			typeof(char),
			(CSharpHelper c, object v) => c.Literal((char)v)
		},
		{
			typeof(DateTime),
			(CSharpHelper c, object v) => c.Literal((DateTime)v)
		},
		{
			typeof(DateTimeOffset),
			(CSharpHelper c, object v) => c.Literal((DateTimeOffset)v)
		},
		{
			typeof(decimal),
			(CSharpHelper c, object v) => c.Literal((decimal)v)
		},
		{
			typeof(double),
			(CSharpHelper c, object v) => c.Literal((double)v)
		},
		{
			typeof(float),
			(CSharpHelper c, object v) => c.Literal((float)v)
		},
		{
			typeof(Guid),
			(CSharpHelper c, object v) => c.Literal((Guid)v)
		},
		{
			typeof(int),
			(CSharpHelper c, object v) => c.Literal((int)v)
		},
		{
			typeof(long),
			(CSharpHelper c, object v) => c.Literal((long)v)
		},
		{
			typeof(NestedClosureCodeFragment),
			(CSharpHelper c, object v) => c.Fragment((NestedClosureCodeFragment)v)
		},
		{
			typeof(object[]),
			(CSharpHelper c, object v) => c.Literal((object[])v)
		},
		{
			typeof(object[,]),
			(CSharpHelper c, object v) => c.Literal((object[,])v)
		},
		{
			typeof(sbyte),
			(CSharpHelper c, object v) => c.Literal((sbyte)v)
		},
		{
			typeof(short),
			(CSharpHelper c, object v) => c.Literal((short)v)
		},
		{
			typeof(string),
			(CSharpHelper c, object v) => c.Literal((string)v)
		},
		{
			typeof(TimeSpan),
			(CSharpHelper c, object v) => c.Literal((TimeSpan)v)
		},
		{
			typeof(uint),
			(CSharpHelper c, object v) => c.Literal((uint)v)
		},
		{
			typeof(ulong),
			(CSharpHelper c, object v) => c.Literal((ulong)v)
		},
		{
			typeof(ushort),
			(CSharpHelper c, object v) => c.Literal((ushort)v)
		},
		{
			typeof(BigInteger),
			(CSharpHelper c, object v) => c.Literal((BigInteger)v)
		}
	};

	public CSharpHelper([JetBrains.Annotations.NotNull] IRelationalTypeMappingSource relationalTypeMappingSource)
	{
		_relationalTypeMappingSource = relationalTypeMappingSource;
	}

	public virtual string Lambda(IReadOnlyList<string> properties)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(properties, "properties");
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("x => ");
		if (properties.Count == 1)
		{
			stringBuilder.Append("x.").Append(properties[0]);
		}
		else
		{
			stringBuilder.Append("new { ");
			stringBuilder.Append(string.Join(", ", properties.Select((string p) => "x." + p)));
			stringBuilder.Append(" }");
		}
		return stringBuilder.ToString();
	}

	public virtual string Reference(Type type)
	{
		return Reference(type, useFullName: false);
	}

	private string Reference(Type type, bool useFullName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type");
		if (_builtInTypes.TryGetValue(type, out var value))
		{
			return value;
		}
		if (type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
		{
			return Reference(type.UnwrapNullableType()) + "?";
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (type.IsArray)
		{
			stringBuilder.Append(Reference(type.GetElementType())).Append("[");
			int arrayRank = type.GetArrayRank();
			for (int i = 1; i < arrayRank; i++)
			{
				stringBuilder.Append(",");
			}
			stringBuilder.Append("]");
			return stringBuilder.ToString();
		}
		if (type.IsNested)
		{
			stringBuilder.Append(Reference(type.DeclaringType)).Append(".");
		}
		stringBuilder.Append(useFullName ? type.DisplayName() : type.ShortDisplayName());
		return stringBuilder.ToString();
	}

	public virtual string Identifier(string name, ICollection<string> scope = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		for (int i = 0; i < name.Length; i++)
		{
			if (!IsIdentifierPartCharacter(name[i]))
			{
				if (num != i)
				{
					stringBuilder.Append(name, num, i - num);
				}
				num = i + 1;
			}
		}
		if (num != name.Length)
		{
			stringBuilder.Append(name.Substring(num));
		}
		if (stringBuilder.Length == 0 || !IsIdentifierStartCharacter(stringBuilder[0]))
		{
			stringBuilder.Insert(0, "_");
		}
		string text = stringBuilder.ToString();
		if (scope != null)
		{
			string text2 = text;
			int num2 = 0;
			while (scope.Contains(text2))
			{
				text2 = text + num2++;
			}
			scope.Add(text2);
			text = text2;
		}
		if (!_keywords.Contains(text))
		{
			return text;
		}
		return "@" + text;
	}

	public virtual string Namespace(params string[] name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(name, "name");
		StringBuilder stringBuilder = new StringBuilder();
		foreach (string item in name.Where((string p) => !string.IsNullOrEmpty(p)).SelectMany((string p) => p.Split(new char[1] { '.' }, StringSplitOptions.RemoveEmptyEntries)))
		{
			string value = Identifier(item);
			if (!string.IsNullOrEmpty(value))
			{
				stringBuilder.Append(value).Append('.');
			}
		}
		if (stringBuilder.Length <= 0)
		{
			return "_";
		}
		return stringBuilder.Remove(stringBuilder.Length - 1, 1).ToString();
	}

	public virtual string Literal(string value)
	{
		if (!Enumerable.Contains(value, '\n') && !Enumerable.Contains(value, '\r'))
		{
			return "\"" + value.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
		}
		return "@\"" + value.Replace("\"", "\"\"") + "\"";
	}

	public virtual string Literal(bool value)
	{
		if (!value)
		{
			return "false";
		}
		return "true";
	}

	public virtual string Literal(byte value)
	{
		return "(byte)" + value;
	}

	public virtual string Literal(char value)
	{
		return "'" + ((value == '\'') ? "\\'" : value.ToString()) + "'";
	}

	public virtual string Literal(DateTime value)
	{
		return string.Format(CultureInfo.InvariantCulture, "new DateTime({0}, {1}, {2}, {3}, {4}, {5}, {6}, DateTimeKind.{7})", value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond, value.Kind) + ((value.Ticks % 10000 == 0L) ? "" : string.Format(CultureInfo.InvariantCulture, ".AddTicks({0})", value.Ticks % 10000));
	}

	public virtual string Literal(DateTimeOffset value)
	{
		return "new DateTimeOffset(" + Literal(value.DateTime) + ", " + Literal(value.Offset) + ")";
	}

	public virtual string Literal(decimal value)
	{
		return value.ToString(CultureInfo.InvariantCulture) + "m";
	}

	public virtual string Literal(double value)
	{
		return EnsureDecimalPlaces(value);
	}

	private static string EnsureDecimalPlaces(double number)
	{
		string text = number.ToString("G17", CultureInfo.InvariantCulture);
		if (double.IsNaN(number))
		{
			return "double.NaN";
		}
		if (double.IsNegativeInfinity(number))
		{
			return "double.NegativeInfinity";
		}
		if (double.IsPositiveInfinity(number))
		{
			return "double.PositiveInfinity";
		}
		if (text.Contains("E") || text.Contains("e") || text.Contains("."))
		{
			return text;
		}
		return text + ".0";
	}

	public virtual string Literal(float value)
	{
		return value.ToString(CultureInfo.InvariantCulture) + "f";
	}

	public virtual string Literal(Guid value)
	{
		Guid guid = value;
		return "new Guid(\"" + guid.ToString() + "\")";
	}

	public virtual string Literal(int value)
	{
		return value.ToString(CultureInfo.InvariantCulture);
	}

	public virtual string Literal(long value)
	{
		return value.ToString(CultureInfo.InvariantCulture) + "L";
	}

	public virtual string Literal(sbyte value)
	{
		return "(sbyte)" + value;
	}

	public virtual string Literal(short value)
	{
		return "(short)" + value;
	}

	public virtual string Literal(TimeSpan value)
	{
		if (value.Ticks % 10000 != 0L)
		{
			return string.Format(CultureInfo.InvariantCulture, "new TimeSpan({0})", value.Ticks);
		}
		return string.Format(CultureInfo.InvariantCulture, "new TimeSpan({0}, {1}, {2}, {3}, {4})", value.Days, value.Hours, value.Minutes, value.Seconds, value.Milliseconds);
	}

	public virtual string Literal(uint value)
	{
		return value + "u";
	}

	public virtual string Literal(ulong value)
	{
		return value + "ul";
	}

	public virtual string Literal(ushort value)
	{
		return "(ushort)" + value;
	}

	public virtual string Literal(BigInteger value)
	{
		return "BigInteger.Parse(\"" + value.ToString(NumberFormatInfo.InvariantInfo) + "\", NumberFormatInfo.InvariantInfo)";
	}

	public virtual string Literal<T>(T? value) where T : struct
	{
		return UnknownLiteral(value);
	}

	public virtual string Literal<T>(T[] values, bool vertical = false)
	{
		return Array(typeof(T), values, vertical);
	}

	private string Array(Type type, IEnumerable values, bool vertical = false)
	{
		IndentedStringBuilder indentedStringBuilder = new IndentedStringBuilder();
		indentedStringBuilder.Append("new");
		bool flag = type == typeof(byte);
		if (flag)
		{
			indentedStringBuilder.Append(" byte");
		}
		else if (type == typeof(object))
		{
			indentedStringBuilder.Append(" object");
		}
		indentedStringBuilder.Append("[]");
		if (vertical)
		{
			indentedStringBuilder.AppendLine();
		}
		else
		{
			indentedStringBuilder.Append(" ");
		}
		indentedStringBuilder.Append("{");
		if (vertical)
		{
			indentedStringBuilder.AppendLine();
			indentedStringBuilder.IncrementIndent();
		}
		else
		{
			indentedStringBuilder.Append(" ");
		}
		bool flag2 = true;
		foreach (object value in values)
		{
			if (flag2)
			{
				flag2 = false;
			}
			else
			{
				indentedStringBuilder.Append(",");
				if (vertical)
				{
					indentedStringBuilder.AppendLine();
				}
				else
				{
					indentedStringBuilder.Append(" ");
				}
			}
			indentedStringBuilder.Append(flag ? Literal((int)(byte)value) : UnknownLiteral(value));
		}
		if (vertical)
		{
			indentedStringBuilder.AppendLine();
			indentedStringBuilder.DecrementIndent();
		}
		else
		{
			indentedStringBuilder.Append(" ");
		}
		indentedStringBuilder.Append("}");
		return indentedStringBuilder.ToString();
	}

	public virtual string Literal(object[,] values)
	{
		IndentedStringBuilder indentedStringBuilder = new IndentedStringBuilder();
		indentedStringBuilder.AppendLine("new object[,]").AppendLine("{");
		using (indentedStringBuilder.Indent())
		{
			int length = values.GetLength(0);
			int length2 = values.GetLength(1);
			for (int i = 0; i < length; i++)
			{
				if (i != 0)
				{
					indentedStringBuilder.AppendLine(",");
				}
				indentedStringBuilder.Append("{ ");
				for (int j = 0; j < length2; j++)
				{
					if (j != 0)
					{
						indentedStringBuilder.Append(", ");
					}
					indentedStringBuilder.Append(UnknownLiteral(values[i, j]));
				}
				indentedStringBuilder.Append(" }");
			}
		}
		indentedStringBuilder.AppendLine().Append("}");
		return indentedStringBuilder.ToString();
	}

	public virtual string Literal(Enum value)
	{
		Type type = value.GetType();
		string name = Enum.GetName(type, value);
		if (name != null)
		{
			return GetSimpleEnumValue(type, name);
		}
		return GetCompositeEnumValue(type, value);
	}

	protected virtual string GetSimpleEnumValue(Type type, string name)
	{
		return Reference(type) + "." + name;
	}

	protected virtual string GetCompositeEnumValue(Type type, Enum flags)
	{
		HashSet<Enum> hashSet = new HashSet<Enum>(GetFlags(flags));
		foreach (Enum currentValue in hashSet.ToList())
		{
			IReadOnlyCollection<Enum> flags2 = GetFlags(currentValue);
			if (flags2.Count > 1)
			{
				hashSet.ExceptWith(flags2.Where((Enum v) => !object.Equals(v, currentValue)));
			}
		}
		return hashSet.Aggregate(null, (string previous, Enum current) => (previous != null) ? (previous + " | " + GetSimpleEnumValue(type, Enum.GetName(type, current))) : GetSimpleEnumValue(type, Enum.GetName(type, current)));
	}

	internal static IReadOnlyCollection<Enum> GetFlags(Enum flags)
	{
		List<Enum> list = new List<Enum>();
		Type type = flags.GetType();
		object obj = Enum.ToObject(type, 0);
		foreach (Enum value in Enum.GetValues(type))
		{
			if (!value.Equals(obj) && flags.HasFlag(value))
			{
				list.Add(value);
			}
		}
		return list;
	}

	public virtual string UnknownLiteral(object value)
	{
		if (value == null || value == DBNull.Value)
		{
			return "null";
		}
		Type type = value.GetType();
		if (_literalFuncs.TryGetValue(type.UnwrapNullableType(), out var value2))
		{
			return value2(this, value);
		}
		if (value is Enum value3)
		{
			return Literal(value3);
		}
		if (value is Array values)
		{
			return Array(type.GetElementType(), values);
		}
		RelationalTypeMapping relationalTypeMapping = _relationalTypeMappingSource.FindMapping(type);
		if (relationalTypeMapping != null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Expression expression = relationalTypeMapping.GenerateCodeLiteral(value);
			if (!HandleExpression(expression, stringBuilder))
			{
				throw new NotSupportedException(DesignStrings.LiteralExpressionNotSupported(expression.ToString(), type.ShortDisplayName()));
			}
			return stringBuilder.ToString();
		}
		throw new InvalidOperationException(DesignStrings.UnknownLiteral(type));
	}

	private bool HandleExpression(Expression expression, StringBuilder builder, bool simple = false)
	{
		switch (expression.NodeType)
		{
		case ExpressionType.NewArrayInit:
			builder.Append("new ").Append(Reference(expression.Type.GetElementType())).Append("[] { ");
			HandleList(((NewArrayExpression)expression).Expressions, builder, simple: true);
			builder.Append(" }");
			return true;
		case ExpressionType.Convert:
			builder.Append('(').Append(Reference(expression.Type, useFullName: true)).Append(')');
			return HandleExpression(((UnaryExpression)expression).Operand, builder);
		case ExpressionType.New:
			builder.Append("new ").Append(Reference(expression.Type, useFullName: true));
			return HandleArguments(((NewExpression)expression).Arguments, builder);
		case ExpressionType.Call:
		{
			MethodCallExpression methodCallExpression = (MethodCallExpression)expression;
			if (methodCallExpression.Method.IsStatic)
			{
				builder.Append(Reference(methodCallExpression.Method.DeclaringType, useFullName: true));
			}
			else if (!HandleExpression(methodCallExpression.Object, builder))
			{
				return false;
			}
			builder.Append('.').Append(methodCallExpression.Method.Name);
			return HandleArguments(methodCallExpression.Arguments, builder);
		}
		case ExpressionType.Constant:
		{
			object value = ((ConstantExpression)expression).Value;
			builder.Append((simple && value != null && value.GetType()?.IsNumeric() == true) ? value : UnknownLiteral(value));
			return true;
		}
		case ExpressionType.MemberAccess:
		{
			MemberExpression memberExpression = (MemberExpression)expression;
			if (memberExpression.Expression == null)
			{
				builder.Append(Reference(memberExpression.Member.DeclaringType, useFullName: true));
			}
			else if (!HandleExpression(memberExpression.Expression, builder))
			{
				return false;
			}
			builder.Append('.').Append(memberExpression.Member.Name);
			return true;
		}
		default:
			return false;
		}
	}

	private bool HandleArguments(IEnumerable<Expression> argumentExpressions, StringBuilder builder)
	{
		builder.Append('(');
		if (!HandleList(argumentExpressions, builder))
		{
			return false;
		}
		builder.Append(')');
		return true;
	}

	private bool HandleList(IEnumerable<Expression> argumentExpressions, StringBuilder builder, bool simple = false)
	{
		string value = string.Empty;
		foreach (Expression argumentExpression in argumentExpressions)
		{
			builder.Append(value);
			if (!HandleExpression(argumentExpression, builder, simple))
			{
				return false;
			}
			value = ", ";
		}
		return true;
	}

	public virtual string Fragment(MethodCallCodeFragment fragment)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (MethodCallCodeFragment methodCallCodeFragment = fragment; methodCallCodeFragment != null; methodCallCodeFragment = methodCallCodeFragment.ChainedCall)
		{
			stringBuilder.Append(".").Append(methodCallCodeFragment.Method).Append("(");
			for (int i = 0; i < methodCallCodeFragment.Arguments.Count; i++)
			{
				if (i != 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(UnknownLiteral(methodCallCodeFragment.Arguments[i]));
			}
			stringBuilder.Append(")");
		}
		return stringBuilder.ToString();
	}

	private string Fragment(NestedClosureCodeFragment fragment)
	{
		return fragment.Parameter + " => " + fragment.Parameter + Fragment(fragment.MethodCall);
	}

	private static bool IsIdentifierStartCharacter(char ch)
	{
		if (ch < 'a')
		{
			if (ch >= 'A')
			{
				if (ch > 'Z')
				{
					return ch == '_';
				}
				return true;
			}
			return false;
		}
		if (ch <= 'z')
		{
			return true;
		}
		if (ch > '\u007f')
		{
			return IsLetterChar(CharUnicodeInfo.GetUnicodeCategory(ch));
		}
		return false;
	}

	private static bool IsIdentifierPartCharacter(char ch)
	{
		if (ch < 'a')
		{
			if (ch >= 'A')
			{
				if (ch > 'Z')
				{
					return ch == '_';
				}
				return true;
			}
			if (ch >= '0')
			{
				return ch <= '9';
			}
			return false;
		}
		if (ch <= 'z')
		{
			return true;
		}
		if (ch <= '\u007f')
		{
			return false;
		}
		UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(ch);
		if (IsLetterChar(unicodeCategory))
		{
			return true;
		}
		switch (unicodeCategory)
		{
		case UnicodeCategory.NonSpacingMark:
		case UnicodeCategory.SpacingCombiningMark:
		case UnicodeCategory.DecimalDigitNumber:
		case UnicodeCategory.Format:
		case UnicodeCategory.ConnectorPunctuation:
			return true;
		default:
			return false;
		}
	}

	private static bool IsLetterChar(UnicodeCategory cat)
	{
		if ((uint)cat <= 4u || cat == UnicodeCategory.LetterNumber)
		{
			return true;
		}
		return false;
	}
}
