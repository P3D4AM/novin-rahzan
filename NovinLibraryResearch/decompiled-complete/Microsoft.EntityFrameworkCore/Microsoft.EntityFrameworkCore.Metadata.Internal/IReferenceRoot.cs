using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public interface IReferenceRoot<T>
{
	Reference<T> Track([JetBrains.Annotations.NotNull] T @object);

	void Release([JetBrains.Annotations.NotNull] Reference<T> reference);
}
