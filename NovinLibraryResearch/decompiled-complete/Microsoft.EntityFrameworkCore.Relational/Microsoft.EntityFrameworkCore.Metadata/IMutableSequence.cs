using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IMutableSequence : ISequence
{
	new IMutableModel Model { get; }

	new long StartValue { get; set; }

	new int IncrementBy { get; set; }

	new long? MinValue { get; set; }

	new long? MaxValue { get; set; }

	new Type ClrType
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	new bool IsCyclic { get; set; }
}
