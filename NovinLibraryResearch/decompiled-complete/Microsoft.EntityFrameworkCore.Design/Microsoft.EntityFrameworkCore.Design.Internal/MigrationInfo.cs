using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Design.Internal;

public class MigrationInfo
{
	public virtual string Id
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	public virtual string Name
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}
}
