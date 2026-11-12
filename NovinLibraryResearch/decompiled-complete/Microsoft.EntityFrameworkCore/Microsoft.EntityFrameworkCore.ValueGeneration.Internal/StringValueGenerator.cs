using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

public class StringValueGenerator : ValueGenerator<string>
{
	public override bool GeneratesTemporaryValues { get; }

	public StringValueGenerator(bool generateTemporaryValues)
	{
		GeneratesTemporaryValues = generateTemporaryValues;
	}

	public override string Next(EntityEntry entry)
	{
		return Guid.NewGuid().ToString();
	}
}
