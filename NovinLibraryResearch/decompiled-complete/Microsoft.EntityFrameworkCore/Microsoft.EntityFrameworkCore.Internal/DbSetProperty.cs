using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Internal;

public readonly struct DbSetProperty
{
	public string Name { get; }

	public Type ClrType { get; }

	public IClrPropertySetter Setter { get; }

	public bool IsKeyless { get; }

	public DbSetProperty([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] Type clrType, [JetBrains.Annotations.CanBeNull] IClrPropertySetter setter, bool keyless = false)
	{
		Name = name;
		ClrType = clrType;
		Setter = setter;
		IsKeyless = keyless;
	}
}
