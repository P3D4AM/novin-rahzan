using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IPropertyBase : IAnnotatable
{
	string Name { get; }

	ITypeBase DeclaringType { get; }

	Type ClrType { get; }

	PropertyInfo PropertyInfo { get; }

	FieldInfo FieldInfo { get; }
}
