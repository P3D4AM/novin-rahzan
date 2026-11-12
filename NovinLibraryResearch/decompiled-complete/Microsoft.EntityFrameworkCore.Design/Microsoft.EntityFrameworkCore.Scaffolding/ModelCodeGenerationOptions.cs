using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Scaffolding;

public class ModelCodeGenerationOptions
{
	public virtual bool UseDataAnnotations { get; set; }

	public virtual bool SuppressConnectionStringWarning { get; set; }

	public virtual string RootNamespace
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual string ModelNamespace
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual string ContextNamespace
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual string Language
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual string ContextDir
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual string ContextName
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual string ConnectionString
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}
}
