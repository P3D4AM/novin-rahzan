using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;

public static class ConventionBatchExtensions
{
	[DebuggerStepThrough]
	public static InternalRelationshipBuilder Run([JetBrains.Annotations.NotNull] this IConventionBatch batch, [JetBrains.Annotations.NotNull] InternalRelationshipBuilder relationshipBuilder)
	{
		return (InternalRelationshipBuilder)(batch.Run(relationshipBuilder.Metadata)?.Builder);
	}
}
