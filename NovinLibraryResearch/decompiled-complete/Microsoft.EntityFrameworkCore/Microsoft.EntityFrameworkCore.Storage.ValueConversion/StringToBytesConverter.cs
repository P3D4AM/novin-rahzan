using System;
using System.Linq.Expressions;
using System.Text;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class StringToBytesConverter : ValueConverter<string, byte[]>
{
	public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(string), typeof(byte[]), (ValueConverterInfo i) => new StringToBytesConverter(Encoding.UTF8, i.MappingHints));


	public StringToBytesConverter([JetBrains.Annotations.NotNull] Encoding encoding, [JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base((Expression<Func<string, byte[]>>)((string v) => (v == null) ? null : encoding.GetBytes(v)), (Expression<Func<byte[], string>>)((byte[] v) => (v == null) ? null : encoding.GetString(v)), mappingHints)
	{
	}
}
