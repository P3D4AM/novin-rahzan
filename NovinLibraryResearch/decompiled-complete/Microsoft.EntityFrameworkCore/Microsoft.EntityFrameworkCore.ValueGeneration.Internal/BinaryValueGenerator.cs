using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

public class BinaryValueGenerator : ValueGenerator<byte[]>
{
	public override bool GeneratesTemporaryValues { get; }

	public BinaryValueGenerator(bool generateTemporaryValues)
	{
		GeneratesTemporaryValues = generateTemporaryValues;
	}

	public override byte[] Next(EntityEntry entry)
	{
		return Guid.NewGuid().ToByteArray();
	}
}
