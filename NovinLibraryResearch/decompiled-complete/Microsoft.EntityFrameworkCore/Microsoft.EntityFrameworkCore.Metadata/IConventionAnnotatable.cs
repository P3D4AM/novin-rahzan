using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IConventionAnnotatable : IAnnotatable
{
	new IEnumerable<IConventionAnnotation> GetAnnotations();

	IConventionAnnotation AddAnnotation([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] object value, bool fromDataAnnotation = false);

	void SetAnnotation([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] object value, bool fromDataAnnotation = false);

	new IConventionAnnotation FindAnnotation([JetBrains.Annotations.NotNull] string name);

	IConventionAnnotation RemoveAnnotation([JetBrains.Annotations.NotNull] string name);
}
