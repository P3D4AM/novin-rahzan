using System;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class NumberToBytesConverter<TNumber> : ValueConverter<TNumber, byte[]>
{
	private static readonly ConverterMappingHints _defaultHints = new ConverterMappingHints(GetByteCount());

	private static readonly MethodInfo _reverseLongMethod = typeof(NumberToBytesConverter<TNumber>).GetMethod("ReverseLong", BindingFlags.Static | BindingFlags.NonPublic);

	private static readonly MethodInfo _reverseIntMethod = typeof(NumberToBytesConverter<TNumber>).GetMethod("ReverseInt", BindingFlags.Static | BindingFlags.NonPublic);

	private static readonly MethodInfo _reverseShortMethod = typeof(NumberToBytesConverter<TNumber>).GetMethod("ReverseShort", BindingFlags.Static | BindingFlags.NonPublic);

	private static readonly MethodInfo _toBytesMethod = typeof(NumberToBytesConverter<TNumber>).GetMethod("DecimalToBytes", BindingFlags.Static | BindingFlags.NonPublic);

	private static readonly MethodInfo _toDecimalMethod = typeof(NumberToBytesConverter<TNumber>).GetMethod("BytesToDecimal", BindingFlags.Static | BindingFlags.NonPublic);

	public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(TNumber), typeof(byte[]), (ValueConverterInfo i) => new NumberToBytesConverter<TNumber>(i.MappingHints), _defaultHints);


	public NumberToBytesConverter([JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(ToBytes(), ToNumber(), _defaultHints.With(mappingHints))
	{
	}

	private static Expression<Func<TNumber, byte[]>> ToBytes()
	{
		Type type = typeof(TNumber).UnwrapNullableType();
		ValueConverter.CheckTypeSupported(type, typeof(NumberToBytesConverter<TNumber>), typeof(double), typeof(float), typeof(decimal), typeof(char), typeof(int), typeof(long), typeof(short), typeof(byte), typeof(uint), typeof(ulong), typeof(ushort), typeof(sbyte));
		ParameterExpression parameterExpression = Expression.Parameter(typeof(TNumber), "v");
		Expression expression = (typeof(TNumber).IsNullableType() ? ((Expression)Expression.Convert(parameterExpression, type)) : ((Expression)parameterExpression));
		Expression expression2 = ((type == typeof(byte)) ? Expression.NewArrayInit(typeof(byte), expression) : ((type == typeof(sbyte)) ? Expression.NewArrayInit(typeof(byte), Expression.Convert(expression, typeof(byte))) : ((type == typeof(decimal)) ? Expression.Call(_toBytesMethod, expression) : EnsureEndian(Expression.Call(typeof(BitConverter).GetMethod("GetBytes", new Type[1] { type }), expression)))));
		if (typeof(TNumber).IsNullableType())
		{
			expression2 = Expression.Condition(Expression.Property(parameterExpression, typeof(TNumber).GetProperty("HasValue")), expression2, Expression.Constant(null, typeof(byte[])));
		}
		return Expression.Lambda<Func<TNumber, byte[]>>(expression2, new ParameterExpression[1] { parameterExpression });
	}

	private static Expression<Func<byte[], TNumber>> ToNumber()
	{
		Type type = typeof(TNumber).UnwrapNullableType();
		ParameterExpression parameterExpression = Expression.Parameter(typeof(byte[]), "v");
		Expression expression = ((type == typeof(byte)) ? Expression.ArrayAccess(parameterExpression, Expression.Constant(0)) : ((type == typeof(sbyte)) ? ((Expression)Expression.Convert(Expression.ArrayAccess(parameterExpression, Expression.Constant(0)), typeof(sbyte))) : ((Expression)((type == typeof(decimal)) ? Expression.Call(_toDecimalMethod, parameterExpression) : Expression.Call(typeof(BitConverter).GetMethod("To" + type.Name, new Type[2]
		{
			typeof(byte[]),
			typeof(int)
		}), EnsureEndian(parameterExpression), Expression.Constant(0))))));
		if (typeof(TNumber).IsNullableType())
		{
			expression = Expression.Convert(expression, typeof(TNumber));
		}
		return Expression.Lambda<Func<byte[], TNumber>>(Expression.Condition(Expression.ReferenceEqual(parameterExpression, Expression.Constant(null)), Expression.Constant(default(TNumber), typeof(TNumber)), expression), new ParameterExpression[1] { parameterExpression });
	}

	private static Expression EnsureEndian(Expression expression)
	{
		if (!BitConverter.IsLittleEndian)
		{
			return expression;
		}
		return GetByteCount() switch
		{
			8 => Expression.Call(_reverseLongMethod, expression), 
			4 => Expression.Call(_reverseIntMethod, expression), 
			2 => Expression.Call(_reverseShortMethod, expression), 
			_ => expression, 
		};
	}

	private static byte[] ReverseLong(byte[] bytes)
	{
		return new byte[8]
		{
			bytes[7],
			bytes[6],
			bytes[5],
			bytes[4],
			bytes[3],
			bytes[2],
			bytes[1],
			bytes[0]
		};
	}

	private static byte[] ReverseInt(byte[] bytes)
	{
		return new byte[4]
		{
			bytes[3],
			bytes[2],
			bytes[1],
			bytes[0]
		};
	}

	private static byte[] ReverseShort(byte[] bytes)
	{
		return new byte[2]
		{
			bytes[1],
			bytes[0]
		};
	}

	private static int GetByteCount()
	{
		Type type = typeof(TNumber).UnwrapNullableType();
		if (!(type == typeof(decimal)))
		{
			if (!(type == typeof(long)) && !(type == typeof(ulong)) && !(type == typeof(double)))
			{
				if (!(type == typeof(int)) && !(type == typeof(uint)) && !(type == typeof(float)))
				{
					if (!(type == typeof(short)) && !(type == typeof(ushort)) && !(type == typeof(char)))
					{
						return 1;
					}
					return 2;
				}
				return 4;
			}
			return 8;
		}
		return 16;
	}

	private static byte[] EnsureEndianInt(byte[] bytes)
	{
		if (!BitConverter.IsLittleEndian)
		{
			return bytes;
		}
		return ReverseInt(bytes);
	}

	private static byte[] DecimalToBytes(decimal value)
	{
		int[] bits = decimal.GetBits(value);
		byte[] array = new byte[16];
		Array.Copy(EnsureEndianInt(BitConverter.GetBytes(bits[0])), 0, array, 12, 4);
		Array.Copy(EnsureEndianInt(BitConverter.GetBytes(bits[1])), 0, array, 8, 4);
		Array.Copy(EnsureEndianInt(BitConverter.GetBytes(bits[2])), 0, array, 4, 4);
		Array.Copy(EnsureEndianInt(BitConverter.GetBytes(bits[3])), 0, array, 0, 4);
		return array;
	}

	private static decimal BytesToDecimal(byte[] bytes)
	{
		if (BitConverter.IsLittleEndian)
		{
			Array.Reverse((Array)bytes, 0, 4);
			Array.Reverse((Array)bytes, 4, 4);
			Array.Reverse((Array)bytes, 8, 4);
			Array.Reverse((Array)bytes, 12, 4);
		}
		uint num = BitConverter.ToUInt32(bytes, 0);
		return new decimal(BitConverter.ToInt32(bytes, 12), BitConverter.ToInt32(bytes, 8), BitConverter.ToInt32(bytes, 4), (num & 0x80000000u) != 0, (byte)((num & 0xFF0000) >> 16));
	}
}
