using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public sealed class ModelCustomizerDependencies
{
	public IDbSetFinder SetFinder { get; }

	[EntityFrameworkInternal]
	public ModelCustomizerDependencies([JetBrains.Annotations.NotNull] IDbSetFinder setFinder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(setFinder, "setFinder");
		SetFinder = setFinder;
	}

	public ModelCustomizerDependencies With([JetBrains.Annotations.NotNull] IDbSetFinder setFinder)
	{
		return new ModelCustomizerDependencies(setFinder);
	}
}
