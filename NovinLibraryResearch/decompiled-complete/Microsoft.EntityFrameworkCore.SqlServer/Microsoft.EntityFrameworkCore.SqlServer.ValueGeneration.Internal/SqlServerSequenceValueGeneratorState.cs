using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore.SqlServer.ValueGeneration.Internal;

public class SqlServerSequenceValueGeneratorState : HiLoValueGeneratorState
{
	public virtual ISequence Sequence { get; }

	public SqlServerSequenceValueGeneratorState([JetBrains.Annotations.NotNull] ISequence sequence)
		: base(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sequence, "sequence").IncrementBy)
	{
		Sequence = sequence;
	}
}
