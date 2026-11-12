using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class ConverterMappingHints
{
	public virtual int? Size { get; }

	public virtual int? Precision { get; }

	public virtual int? Scale { get; }

	public virtual bool? IsUnicode { get; }

	public virtual Func<IProperty, IEntityType, ValueGenerator> ValueGeneratorFactory { get; }

	public ConverterMappingHints(int? size = null, int? precision = null, int? scale = null, bool? unicode = null, [JetBrains.Annotations.CanBeNull] Func<IProperty, IEntityType, ValueGenerator> valueGeneratorFactory = null)
	{
		Size = size;
		Precision = precision;
		Scale = scale;
		IsUnicode = unicode;
		ValueGeneratorFactory = valueGeneratorFactory;
	}

	public virtual ConverterMappingHints With([JetBrains.Annotations.CanBeNull] ConverterMappingHints hints)
	{
		if (hints != null)
		{
			return new ConverterMappingHints(hints.Size ?? Size, hints.Precision ?? Precision, hints.Scale ?? Scale, hints.IsUnicode ?? IsUnicode, hints.ValueGeneratorFactory ?? ValueGeneratorFactory);
		}
		return this;
	}
}
