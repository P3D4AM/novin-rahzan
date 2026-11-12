using System.Diagnostics;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public abstract class InternalModelItemBuilder<TMetadata> : InternalAnnotatableBuilder<TMetadata> where TMetadata : ConventionAnnotatable
{
	public override InternalModelBuilder ModelBuilder
	{
		[DebuggerStepThrough]
		get;
	}

	protected InternalModelItemBuilder([JetBrains.Annotations.NotNull] TMetadata metadata, [JetBrains.Annotations.NotNull] InternalModelBuilder modelBuilder)
		: base(metadata)
	{
		ModelBuilder = modelBuilder;
	}
}
