using System;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class DateTimeOffsetToBytesConverter : ValueConverter<DateTimeOffset, byte[]>
{
	private static readonly ConverterMappingHints _defaultHints = new ConverterMappingHints(12);

	private static readonly NumberToBytesConverter<long> _longToBytes = new NumberToBytesConverter<long>();

	private static readonly NumberToBytesConverter<short> _shortToBytes = new NumberToBytesConverter<short>();

	public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(DateTimeOffset), typeof(byte[]), (ValueConverterInfo i) => new DateTimeOffsetToBytesConverter(i.MappingHints), _defaultHints);


	public DateTimeOffsetToBytesConverter([JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base((Expression<Func<DateTimeOffset, byte[]>>)((DateTimeOffset v) => ToBytes(v)), (Expression<Func<byte[], DateTimeOffset>>)((byte[] v) => (v == null) ? default(DateTimeOffset) : FromBytes(v)), _defaultHints.With(mappingHints))
	{
	}

	private static byte[] ToBytes(DateTimeOffset value)
	{
		byte[] first = (byte[])_longToBytes.ConvertToProvider(value.DateTime.ToBinary());
		byte[] second = (byte[])_shortToBytes.ConvertToProvider(value.Offset.TotalMinutes);
		return first.Concat(second).ToArray();
	}

	private static DateTimeOffset FromBytes(byte[] bytes)
	{
		long dateData = (long)_longToBytes.ConvertFromProvider(bytes);
		return new DateTimeOffset(offset: new TimeSpan(0, (short)_shortToBytes.ConvertFromProvider(bytes.Skip(8).ToArray()), 0), dateTime: DateTime.FromBinary(dateData));
	}
}
