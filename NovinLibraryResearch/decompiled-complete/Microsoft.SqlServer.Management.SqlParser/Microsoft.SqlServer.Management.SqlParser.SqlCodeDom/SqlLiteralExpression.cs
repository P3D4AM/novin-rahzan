using System;
using System.Globalization;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlLiteralExpression : SqlScalarExpression
{
	private class StringLiteralExpression : SqlLiteralExpression
	{
		public StringLiteralExpression(string value)
			: base(value, LiteralValueType.String)
		{
		}
	}

	private class UnicodeStringLiteralExpression : SqlLiteralExpression
	{
		public UnicodeStringLiteralExpression(string value)
			: base(value, LiteralValueType.UnicodeString)
		{
		}
	}

	private class NullLiteralExpression : SqlLiteralExpression
	{
		public NullLiteralExpression(string value)
			: base(value, LiteralValueType.Null)
		{
		}
	}

	private class DefaultLiteralExpression : SqlLiteralExpression
	{
		public DefaultLiteralExpression(string value)
			: base(value, LiteralValueType.Default)
		{
		}
	}

	private class IdentifierLiteralExpression : SqlLiteralExpression
	{
		public IdentifierLiteralExpression(SqlIdentifier identifier)
			: this(identifier.ToString())
		{
		}

		public IdentifierLiteralExpression(string value)
			: base(value, LiteralValueType.Identifier)
		{
		}
	}

	private class NumericLiteralExpression : SqlLiteralExpression
	{
		private readonly bool hasDecimalPoint;

		private readonly bool isNegative;

		private readonly bool isInteger;

		private readonly long intValue;

		private readonly int effictiveDigitCount;

		public NumericLiteralExpression(string value)
			: base(value, LiteralValueType.Numeric)
		{
			hasDecimalPoint = value.Contains(".");
			isNegative = value.StartsWith("-", StringComparison.Ordinal);
			int num = 0;
			for (int i = (isNegative ? 1 : 0); i < value.Length && value[i] == '0'; i++)
			{
				num++;
			}
			effictiveDigitCount = value.Length - num;
			if (hasDecimalPoint)
			{
				effictiveDigitCount--;
			}
			if (isNegative)
			{
				effictiveDigitCount--;
			}
			if (effictiveDigitCount > 38)
			{
				ReportParseError(SR.MaximumPrecisionOutOfRange(value));
			}
			else
			{
				isInteger = long.TryParse(value, out intValue);
			}
		}

		public NumericLiteralExpression(long value)
			: base(value.ToString(), LiteralValueType.Numeric)
		{
			isInteger = true;
			intValue = value;
		}

		internal override int? AsInt32()
		{
			if (isInteger && intValue <= int.MaxValue && intValue >= int.MinValue)
			{
				return (int)intValue;
			}
			return null;
		}

		internal override long? AsInt64()
		{
			if (isInteger)
			{
				return intValue;
			}
			return null;
		}

		internal override float? AsFloat32()
		{
			if (isInteger)
			{
				return intValue;
			}
			return float.Parse(value, CultureInfo.InvariantCulture);
		}

		internal override double? AsFloat64()
		{
			if (isInteger)
			{
				return intValue;
			}
			return double.Parse(value, CultureInfo.InvariantCulture);
		}

		internal override bool HasDecimalPoint()
		{
			return hasDecimalPoint;
		}

		internal override int EffectiveNumberOfDigits()
		{
			return effictiveDigitCount;
		}
	}

	private class IntegerLiteralExpression : SqlLiteralExpression
	{
		private readonly int intValue;

		public IntegerLiteralExpression(string value)
			: base(value, LiteralValueType.Integer)
		{
			intValue = int.Parse(value);
		}

		public IntegerLiteralExpression(int value)
			: base(value.ToString(), LiteralValueType.Integer)
		{
			intValue = value;
		}

		internal override int? AsInt32()
		{
			return intValue;
		}

		internal override long? AsInt64()
		{
			return intValue;
		}

		internal override int EffectiveNumberOfDigits()
		{
			int num = intValue;
			int length = num.ToString(CultureInfo.InvariantCulture).Length;
			if (intValue < 0)
			{
				return length - 1;
			}
			return length;
		}
	}

	private class RealLiteralExpression : SqlLiteralExpression
	{
		private readonly bool hasValue;

		private readonly double floatValue;

		public RealLiteralExpression(string value)
			: base(value, LiteralValueType.Real)
		{
			hasValue = double.TryParse(value, out floatValue);
		}

		public RealLiteralExpression(float value)
			: base(value.ToString(), LiteralValueType.Real)
		{
			hasValue = true;
			floatValue = value;
		}

		public RealLiteralExpression(double value)
			: base(value.ToString(), LiteralValueType.Real)
		{
			hasValue = true;
			floatValue = value;
		}

		internal override float? AsFloat32()
		{
			if (hasValue && floatValue <= 3.4028234663852886E+38 && floatValue >= -3.4028234663852886E+38)
			{
				return (float)floatValue;
			}
			return null;
		}

		internal override double? AsFloat64()
		{
			if (hasValue)
			{
				return floatValue;
			}
			return null;
		}

		internal override bool HasDecimalPoint()
		{
			return true;
		}
	}

	private class BinaryLiteralExpression : SqlLiteralExpression
	{
		public readonly bool isInt32;

		private readonly int intValue;

		public BinaryLiteralExpression(string value)
			: base(value, LiteralValueType.Binary)
		{
			if (value.Length == 2)
			{
				isInt32 = true;
			}
			else
			{
				isInt32 = int.TryParse(value.Substring(2), NumberStyles.HexNumber, null, out intValue);
			}
		}

		internal override int? AsInt32()
		{
			if (isInt32)
			{
				return intValue;
			}
			return null;
		}

		internal override byte[] AsByteArray()
		{
			int num = value.Length - 2;
			byte[] array = new byte[num / 2 + (num & 1)];
			int i = 2;
			int num2 = 0;
			if (((uint)num & (true ? 1u : 0u)) != 0)
			{
				byte.TryParse(value.Substring(i, 1), NumberStyles.HexNumber, null, out array[num2++]);
				i++;
			}
			for (; i < value.Length; i += 2)
			{
				byte.TryParse(value.Substring(i, 2), NumberStyles.HexNumber, null, out array[num2++]);
			}
			return array;
		}
	}

	private class MoneyLiteralExpression : SqlLiteralExpression
	{
		public MoneyLiteralExpression(string value)
			: base(value, LiteralValueType.Money)
		{
		}
	}

	private readonly string value;

	private readonly LiteralValueType type;

	internal override bool IsVisitable => true;

	internal sealed override bool IsValidFilterExpressionConstant => true;

	public override IScalar BoundScalar
	{
		get
		{
			if (base.IsBound)
			{
				ILiteralScalarFactory literal = BindingContext.MetadataFactory.Scalar.Literal;
				return type switch
				{
					LiteralValueType.Binary => literal.Binary, 
					LiteralValueType.Default => literal.Default, 
					LiteralValueType.Identifier => literal.Identifier, 
					LiteralValueType.Image => literal.Image, 
					LiteralValueType.Integer => literal.Integer, 
					LiteralValueType.Money => literal.Money, 
					LiteralValueType.Null => literal.Null, 
					LiteralValueType.Numeric => literal.Numeric, 
					LiteralValueType.Real => literal.Real, 
					LiteralValueType.String => literal.String, 
					LiteralValueType.UnicodeString => literal.UnicodeString, 
					_ => null, 
				};
			}
			return null;
		}
	}

	internal override ScalarExpressionType ExpressionType => ScalarExpressionType.Literal;

	internal override bool IsConstantExpression => true;

	public string Value => value;

	public LiteralValueType Type => type;

	public override void Accept(ISqlCodeObjectVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override T Accept<T>(ISqlCodeObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	public override void Accept<C>(ISqlCodeObjectContextVisitor<C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this, context);
	}

	public override T Accept<T, C>(ISqlCodeObjectContextVisitor<T, C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this, context);
	}

	internal override void Bind(IBindingContext bindingContext)
	{
		base.BindingState |= BindingState.BindSucceeded;
	}

	private SqlLiteralExpression(string value, LiteralValueType type)
	{
		this.value = value;
		this.type = type;
	}

	public override string ToString()
	{
		return value;
	}

	internal static SqlLiteralExpression String(string value)
	{
		bool isUnicode;
		string text = EscapeSequence.UnescapeLiteral(value, out isUnicode);
		if (isUnicode)
		{
			return new UnicodeStringLiteralExpression(text);
		}
		return new StringLiteralExpression(text);
	}

	internal static SqlLiteralExpression Integer(int value)
	{
		return new IntegerLiteralExpression(value);
	}

	internal static SqlLiteralExpression Integer(long value)
	{
		return new NumericLiteralExpression(value);
	}

	internal static SqlLiteralExpression Integer(string value)
	{
		return Integer(value, isNegative: false);
	}

	internal static SqlLiteralExpression Integer(string value, bool isNegative)
	{
		if (isNegative)
		{
			value = "-" + value;
		}
		return new IntegerLiteralExpression(value);
	}

	internal static SqlLiteralExpression Real(float value)
	{
		return new RealLiteralExpression(value);
	}

	internal static SqlLiteralExpression Real(double value)
	{
		return new RealLiteralExpression(value);
	}

	internal static SqlLiteralExpression Real(string value)
	{
		return new RealLiteralExpression(value);
	}

	internal static SqlLiteralExpression Real(string value, bool isNegative)
	{
		if (isNegative)
		{
			value = "-" + value;
		}
		return new RealLiteralExpression(value);
	}

	internal static SqlLiteralExpression Numeric(string value)
	{
		return new NumericLiteralExpression(value);
	}

	internal static SqlLiteralExpression Numeric(string value, bool isNegative)
	{
		if (isNegative)
		{
			value = "-" + value;
		}
		return new NumericLiteralExpression(value);
	}

	internal static SqlLiteralExpression Binary(string value)
	{
		return new BinaryLiteralExpression(value);
	}

	internal static SqlLiteralExpression Money(string value)
	{
		return new MoneyLiteralExpression(value);
	}

	internal static SqlLiteralExpression Money(string value, bool isNegative)
	{
		if (isNegative)
		{
			value = "-" + value;
		}
		return new MoneyLiteralExpression(value);
	}

	internal static SqlLiteralExpression Null(string value)
	{
		return new NullLiteralExpression(value);
	}

	internal static SqlLiteralExpression Default(string value)
	{
		return new DefaultLiteralExpression(value);
	}

	internal static SqlLiteralExpression Identifier(SqlIdentifier identifier)
	{
		return new IdentifierLiteralExpression(identifier);
	}

	internal virtual int? AsInt32()
	{
		return null;
	}

	internal virtual long? AsInt64()
	{
		return null;
	}

	internal virtual float? AsFloat32()
	{
		return null;
	}

	internal virtual double? AsFloat64()
	{
		return null;
	}

	internal virtual byte[] AsByteArray()
	{
		return null;
	}

	internal virtual bool HasDecimalPoint()
	{
		return false;
	}

	internal virtual int EffectiveNumberOfDigits()
	{
		return 0;
	}
}
