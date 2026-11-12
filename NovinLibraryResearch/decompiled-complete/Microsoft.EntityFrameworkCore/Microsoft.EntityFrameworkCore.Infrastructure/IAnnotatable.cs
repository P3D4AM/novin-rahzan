using System.Collections.Generic;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public interface IAnnotatable
{
	object this[[JetBrains.Annotations.NotNull] string name] { get; }

	IAnnotation FindAnnotation([JetBrains.Annotations.NotNull] string name);

	IEnumerable<IAnnotation> GetAnnotations();
}
