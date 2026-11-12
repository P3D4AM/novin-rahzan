using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

[DebuggerDisplay("{DebuggerDisplay(),nq}")]
public readonly struct TypeIdentity
{
	public string Name
	{
		[DebuggerStepThrough]
		get;
	}

	public Type Type
	{
		[DebuggerStepThrough]
		get;
	}

	[DebuggerStepThrough]
	public TypeIdentity([JetBrains.Annotations.NotNull] string name)
	{
		Name = name;
		Type = null;
	}

	[DebuggerStepThrough]
	public TypeIdentity([JetBrains.Annotations.NotNull] Type type, [JetBrains.Annotations.NotNull] Model model)
	{
		Name = model.GetDisplayName(type);
		Type = type;
	}

	private string DebuggerDisplay()
	{
		return Name;
	}
}
