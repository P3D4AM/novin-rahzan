using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Scaffolding;

public class ScaffoldedFile
{
	public virtual string Path
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	public virtual string Code
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}
}
