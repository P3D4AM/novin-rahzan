using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IPropertyParameterBindingFactory
{
	ParameterBinding FindParameter([JetBrains.Annotations.NotNull] IEntityType entityType, [JetBrains.Annotations.NotNull] Type parameterType, [JetBrains.Annotations.NotNull] string parameterName);
}
