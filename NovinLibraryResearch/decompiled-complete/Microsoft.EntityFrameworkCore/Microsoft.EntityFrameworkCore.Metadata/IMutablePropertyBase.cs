using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IMutablePropertyBase : IPropertyBase, IAnnotatable, IMutableAnnotatable
{
	new IMutableTypeBase DeclaringType { get; }

	new FieldInfo FieldInfo
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}
}
