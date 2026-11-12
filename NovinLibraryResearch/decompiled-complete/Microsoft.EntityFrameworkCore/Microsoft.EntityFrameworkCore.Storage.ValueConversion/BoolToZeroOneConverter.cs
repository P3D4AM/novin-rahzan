using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class BoolToZeroOneConverter<TProvider> : BoolToTwoValuesConverter<TProvider>
{
	public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(bool), typeof(TProvider), (ValueConverterInfo i) => new BoolToZeroOneConverter<TProvider>(i.MappingHints));


	public BoolToZeroOneConverter([JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(Zero(), One(), (Expression<Func<TProvider, bool>>)null, mappingHints)
	{
	}

	private static TProvider Zero()
	{
		ValueConverter.CheckTypeSupported(typeof(TProvider).UnwrapNullableType(), typeof(BoolToZeroOneConverter<TProvider>), typeof(int), typeof(short), typeof(long), typeof(sbyte), typeof(uint), typeof(ushort), typeof(ulong), typeof(byte), typeof(decimal), typeof(double), typeof(float));
		return Activator.CreateInstance<TProvider>();
	}

	private static TProvider One()
	{
		Type type = typeof(TProvider).UnwrapNullableType();
		return (TProvider)((type == typeof(int)) ? ((object)1) : ((type == typeof(short)) ? ((object)(short)1) : ((type == typeof(long)) ? ((object)1L) : ((type == typeof(sbyte)) ? ((object)(sbyte)1) : ((type == typeof(uint)) ? ((object)1u) : ((type == typeof(ushort)) ? ((object)(ushort)1) : ((type == typeof(ulong)) ? ((object)1uL) : ((type == typeof(byte)) ? ((object)(byte)1) : ((type == typeof(decimal)) ? ((object)1m) : ((type == typeof(double)) ? ((object)1.0) : ((type == typeof(float)) ? ((object)1f) : ((object)1))))))))))));
	}
}
