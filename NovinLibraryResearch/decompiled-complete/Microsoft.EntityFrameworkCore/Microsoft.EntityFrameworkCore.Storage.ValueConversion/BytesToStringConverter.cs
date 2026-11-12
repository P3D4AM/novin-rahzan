using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class BytesToStringConverter : ValueConverter<byte[], string>
{
	public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(byte[]), typeof(string), (ValueConverterInfo i) => new BytesToStringConverter(i.MappingHints));


	public BytesToStringConverter([JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base((Expression<Func<byte[], string>>)((byte[] v) => (v == null) ? null : Convert.ToBase64String(v)), (Expression<Func<string, byte[]>>)((string v) => (v == null) ? null : Convert.FromBase64String(v)), mappingHints)
	{
	}
}
