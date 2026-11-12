using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IConstructorBindingFactory
{
	[JetBrains.Annotations.ContractAnnotation("=>true, binding:notnull, failedBindings:null; =>false, binding:null, failedBindings:notnull")]
	bool TryBindConstructor([JetBrains.Annotations.NotNull] IConventionEntityType entityType, [JetBrains.Annotations.NotNull] ConstructorInfo constructor, [JetBrains.Annotations.CanBeNull] out InstantiationBinding binding, [JetBrains.Annotations.CanBeNull] out IEnumerable<ParameterInfo> unboundParameters);

	[JetBrains.Annotations.ContractAnnotation("=>true, binding:notnull, failedBindings:null; =>false, binding:null, failedBindings:notnull")]
	bool TryBindConstructor([JetBrains.Annotations.NotNull] IMutableEntityType entityType, [JetBrains.Annotations.NotNull] ConstructorInfo constructor, [JetBrains.Annotations.CanBeNull] out InstantiationBinding binding, [JetBrains.Annotations.CanBeNull] out IEnumerable<ParameterInfo> unboundParameters);
}
