using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

[AttributeUsage(AttributeTargets.Class)]
public sealed class DbContextAttribute : Attribute
{
	public Type ContextType { get; }

	public DbContextAttribute([JetBrains.Annotations.NotNull] Type contextType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(contextType, "contextType");
		ContextType = contextType;
	}
}
