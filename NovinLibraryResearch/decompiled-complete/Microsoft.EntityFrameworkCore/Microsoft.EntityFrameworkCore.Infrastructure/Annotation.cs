using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public class Annotation : IAnnotation
{
	public virtual string Name { get; }

	public virtual object Value { get; }

	public Annotation([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] object value)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Name = name;
		Value = value;
	}
}
