using System.Collections.Generic;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Scaffolding;

public class ScaffoldedModel
{
	public virtual ScaffoldedFile ContextFile
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	public virtual IList<ScaffoldedFile> AdditionalFiles { get; } = new List<ScaffoldedFile>();

}
