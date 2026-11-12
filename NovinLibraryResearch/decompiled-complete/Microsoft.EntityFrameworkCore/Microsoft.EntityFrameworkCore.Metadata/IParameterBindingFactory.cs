using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IParameterBindingFactory
{
	bool CanBind([JetBrains.Annotations.NotNull] Type parameterType, [JetBrains.Annotations.NotNull] string parameterName);

	ParameterBinding Bind([JetBrains.Annotations.NotNull] IMutableEntityType entityType, [JetBrains.Annotations.NotNull] Type parameterType, [JetBrains.Annotations.NotNull] string parameterName);

	ParameterBinding Bind([JetBrains.Annotations.NotNull] IConventionEntityType entityType, [JetBrains.Annotations.NotNull] Type parameterType, [JetBrains.Annotations.NotNull] string parameterName);
}
