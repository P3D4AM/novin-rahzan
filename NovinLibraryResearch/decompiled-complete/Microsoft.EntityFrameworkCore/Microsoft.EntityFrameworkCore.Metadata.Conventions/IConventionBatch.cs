using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public interface IConventionBatch : IDisposable
{
	IConventionForeignKey Run([JetBrains.Annotations.NotNull] IConventionForeignKey foreignKey);

	IMetadataReference<IConventionForeignKey> Track([JetBrains.Annotations.NotNull] IConventionForeignKey foreignKey);
}
