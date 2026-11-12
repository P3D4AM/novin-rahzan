using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class GuidToBytesConverter : ValueConverter<Guid, byte[]>
{
	private static readonly ConverterMappingHints _defaultHints = new ConverterMappingHints(16, null, null, null, (IProperty p, IEntityType t) => new SequentialGuidValueGenerator());

	public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(Guid), typeof(byte[]), (ValueConverterInfo i) => new GuidToBytesConverter(i.MappingHints), _defaultHints);


	public GuidToBytesConverter([JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base((Expression<Func<Guid, byte[]>>)((Guid v) => v.ToByteArray()), (Expression<Func<byte[], Guid>>)((byte[] v) => (v == null) ? Guid.Empty : new Guid(v)), _defaultHints.With(mappingHints))
	{
	}
}
