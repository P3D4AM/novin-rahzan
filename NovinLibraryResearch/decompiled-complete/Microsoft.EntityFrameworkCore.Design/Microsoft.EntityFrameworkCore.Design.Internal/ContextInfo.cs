using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Design.Internal;

public class ContextInfo
{
	public virtual string ProviderName
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	public virtual string DatabaseName
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	public virtual string DataSource
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	public virtual string Options
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}
}
