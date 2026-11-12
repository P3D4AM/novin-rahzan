using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IMutableAnnotatable : IAnnotatable
{
	new object this[[JetBrains.Annotations.NotNull] string name]
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	IAnnotation AddAnnotation([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] object value);

	void SetAnnotation([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] object value);

	IAnnotation RemoveAnnotation([JetBrains.Annotations.NotNull] string name);
}
