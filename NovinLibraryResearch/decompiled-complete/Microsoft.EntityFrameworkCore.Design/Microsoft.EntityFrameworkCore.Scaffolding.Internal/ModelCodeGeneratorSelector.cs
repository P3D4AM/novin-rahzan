using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design.Internal;

namespace Microsoft.EntityFrameworkCore.Scaffolding.Internal;

public class ModelCodeGeneratorSelector : LanguageBasedSelector<IModelCodeGenerator>, IModelCodeGeneratorSelector
{
	public ModelCodeGeneratorSelector([JetBrains.Annotations.NotNull] IEnumerable<IModelCodeGenerator> services)
		: base(services)
	{
	}
}
