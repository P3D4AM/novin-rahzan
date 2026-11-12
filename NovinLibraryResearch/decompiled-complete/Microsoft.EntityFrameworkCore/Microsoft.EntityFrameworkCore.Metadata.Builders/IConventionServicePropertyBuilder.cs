using System.Reflection;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

public interface IConventionServicePropertyBuilder : IConventionAnnotatableBuilder
{
	new IConventionServiceProperty Metadata { get; }

	IConventionServicePropertyBuilder HasField([JetBrains.Annotations.CanBeNull] string fieldName, bool fromDataAnnotation = false);

	IConventionServicePropertyBuilder HasField([JetBrains.Annotations.CanBeNull] FieldInfo fieldInfo, bool fromDataAnnotation = false);

	bool CanSetField([JetBrains.Annotations.CanBeNull] string fieldName, bool fromDataAnnotation = false);

	bool CanSetField([JetBrains.Annotations.CanBeNull] FieldInfo fieldInfo, bool fromDataAnnotation = false);

	IConventionServicePropertyBuilder HasParameterBinding([JetBrains.Annotations.CanBeNull] ServiceParameterBinding parameterBinding, bool fromDataAnnotation = false);

	bool CanSetParameterBinding([JetBrains.Annotations.CanBeNull] ServiceParameterBinding parameterBinding, bool fromDataAnnotation = false);
}
