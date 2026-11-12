using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IClrPropertyGetter
{
	object GetClrValue([JetBrains.Annotations.NotNull] object entity);

	bool HasDefaultValue([JetBrains.Annotations.NotNull] object entity);
}
