using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IParameterBindingFactories
{
	IParameterBindingFactory FindFactory([JetBrains.Annotations.NotNull] Type parameterType, [JetBrains.Annotations.NotNull] string parameterName);
}
