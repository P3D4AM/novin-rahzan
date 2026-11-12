using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Scaffolding.Internal;

public interface IScaffoldingTypeMapper
{
	TypeScaffoldingInfo FindMapping([JetBrains.Annotations.NotNull] string storeType, bool keyOrIndex, bool rowVersion);
}
