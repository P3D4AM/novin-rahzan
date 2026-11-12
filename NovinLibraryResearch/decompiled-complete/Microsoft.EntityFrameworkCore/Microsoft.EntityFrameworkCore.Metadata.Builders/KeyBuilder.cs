using System.ComponentModel;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

public class KeyBuilder : IInfrastructure<InternalKeyBuilder>
{
	private readonly InternalKeyBuilder _builder;

	InternalKeyBuilder IInfrastructure<InternalKeyBuilder>.Instance => _builder;

	public virtual IMutableKey Metadata => Builder.Metadata;

	private InternalKeyBuilder Builder => this.GetInfrastructure();

	[EntityFrameworkInternal]
	public KeyBuilder([JetBrains.Annotations.NotNull] IMutableKey key)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(key, "key");
		_builder = ((Key)key).Builder;
	}

	public virtual KeyBuilder HasAnnotation([JetBrains.Annotations.NotNull] string annotation, [JetBrains.Annotations.NotNull] object value)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(annotation, "annotation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(value, "value");
		Builder.HasAnnotation(annotation, value, ConfigurationSource.Explicit);
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
