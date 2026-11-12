using System;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface ISequence
{
	string Name { get; }

	string Schema { get; }

	IModel Model { get; }

	long StartValue { get; }

	int IncrementBy { get; }

	long? MinValue { get; }

	long? MaxValue { get; }

	Type ClrType { get; }

	bool IsCyclic { get; }
}
