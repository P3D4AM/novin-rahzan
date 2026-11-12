using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microsoft.EntityFrameworkCore.ValueGeneration;

public class GuidValueGenerator : ValueGenerator<Guid>
{
	public override bool GeneratesTemporaryValues => false;

	public override Guid Next(EntityEntry entry)
	{
		return Guid.NewGuid();
	}
}
