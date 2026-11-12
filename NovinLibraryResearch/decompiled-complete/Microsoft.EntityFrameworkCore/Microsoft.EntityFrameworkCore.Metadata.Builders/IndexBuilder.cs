using System.ComponentModel;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

public class IndexBuilder : IInfrastructure<InternalIndexBuilder>
{
	private readonly InternalIndexBuilder _builder;

	InternalIndexBuilder IInfrastructure<InternalIndexBuilder>.Instance => _builder;

	public virtual IMutableIndex Metadata => Builder.Metadata;

	private InternalIndexBuilder Builder => this.GetInfrastructure();

	[EntityFrameworkInternal]
	public IndexBuilder([JetBrains.Annotations.NotNull] IMutableIndex index)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(index, "index");
		_builder = ((Index)index).Builder;
	}

	public virtual IndexBuilder HasAnnotation([JetBrains.Annotations.NotNull] string annotation, [JetBrains.Annotations.NotNull] object value)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(annotation, "annotation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(value, "value");
		Builder.HasAnnotation(annotation, value, ConfigurationSource.Explicit);
		return this;
	}

	public virtual IndexBuilder IsUnique(bool unique = true)
	{
		Builder.IsUnique(unique, ConfigurationSource.Explicit);
		return this;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override string ToString()
	{
		return base.ToString();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
public class IndexBuilder<T> : IndexBuilder
{
	[EntityFrameworkInternal]
	public IndexBuilder([JetBrains.Annotations.NotNull] IMutableIndex index)
		: base(index)
	{
	}

	public new virtual IndexBuilder<T> HasAnnotation([JetBrains.Annotations.NotNull] string annotation, [JetBrains.Annotations.NotNull] object value)
	{
		return (IndexBuilder<T>)base.HasAnnotation(annotation, value);
	}

	public new virtual IndexBuilder<T> IsUnique(bool unique = true)
	{
		return (IndexBuilder<T>)base.IsUnique(unique);
	}
}
