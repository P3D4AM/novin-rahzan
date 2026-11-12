using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

public class StringGuidConverter<TModel, TProvider> : ValueConverter<TModel, TProvider>
{
	protected static readonly ConverterMappingHints _defaultHints = new ConverterMappingHints(36, null, null, null, (IProperty p, IEntityType t) => new SequentialGuidValueGenerator());

	public StringGuidConverter([JetBrains.Annotations.NotNull] Expression<Func<TModel, TProvider>> convertToProviderExpression, [JetBrains.Annotations.NotNull] Expression<Func<TProvider, TModel>> convertFromProviderExpression, [JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(convertToProviderExpression, convertFromProviderExpression, mappingHints)
	{
	}

	protected new static Expression<Func<Guid, string>> ToString()
	{
		return (Guid v) => v.ToString("D");
	}

	protected static Expression<Func<string, Guid>> ToGuid()
	{
		return (string v) => (v == null) ? default(Guid) : new Guid(v);
	}
}
