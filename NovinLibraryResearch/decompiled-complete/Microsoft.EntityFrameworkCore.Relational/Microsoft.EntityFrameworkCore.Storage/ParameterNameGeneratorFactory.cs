using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public class ParameterNameGeneratorFactory : IParameterNameGeneratorFactory
{
	public ParameterNameGeneratorFactory([JetBrains.Annotations.NotNull] ParameterNameGeneratorDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
	}

	public virtual ParameterNameGenerator Create()
	{
		return new ParameterNameGenerator();
	}
}
