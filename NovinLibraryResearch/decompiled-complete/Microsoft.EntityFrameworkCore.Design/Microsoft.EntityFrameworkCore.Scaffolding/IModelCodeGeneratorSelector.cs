using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Scaffolding;

public interface IModelCodeGeneratorSelector
{
	IModelCodeGenerator Select([JetBrains.Annotations.CanBeNull] string language);
}
