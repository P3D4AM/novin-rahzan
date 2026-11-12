using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class RelationalConverterMappingHints : ConverterMappingHints
{
	public virtual bool? IsFixedLength { get; }

	public RelationalConverterMappingHints(int? size = null, int? precision = null, int? scale = null, bool? unicode = null, bool? fixedLength = null, [JetBrains.Annotations.CanBeNull] Func<IProperty, IEntityType, ValueGenerator> valueGeneratorFactory = null)
		: base(size, precision, scale, unicode, valueGeneratorFactory)
	{
		IsFixedLength = fixedLength;
	}

	public override ConverterMappingHints With(ConverterMappingHints hints)
	{
		if (hints != null)
		{
			return new RelationalConverterMappingHints(hints.Size ?? Size, hints.Precision ?? Precision, hints.Scale ?? Scale, hints.IsUnicode ?? IsUnicode, (hints as RelationalConverterMappingHints)?.IsFixedLength ?? IsFixedLength, hints.ValueGeneratorFactory ?? ValueGeneratorFactory);
		}
		return this;
	}
}
