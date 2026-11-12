using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

public class DiscriminatorValueGenerator : ValueGenerator
{
	public static readonly Func<IProperty, IEntityType, ValueGenerator> Factory = (IProperty _, IEntityType e) => new DiscriminatorValueGenerator(e.GetDiscriminatorValue());

	private readonly object _discriminator;

	public override bool GeneratesTemporaryValues => false;

	public DiscriminatorValueGenerator([JetBrains.Annotations.NotNull] object discriminator)
	{
		_discriminator = discriminator;
	}

	protected override object NextValue(EntityEntry entry)
	{
		return _discriminator;
	}
}
