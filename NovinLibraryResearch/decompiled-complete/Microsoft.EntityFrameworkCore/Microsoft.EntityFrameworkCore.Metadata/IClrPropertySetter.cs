using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IClrPropertySetter
{
	void SetClrValue([JetBrains.Annotations.NotNull] object instance, [JetBrains.Annotations.CanBeNull] object value);
}
