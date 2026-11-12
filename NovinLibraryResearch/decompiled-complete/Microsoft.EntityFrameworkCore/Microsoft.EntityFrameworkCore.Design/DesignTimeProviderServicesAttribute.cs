using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Design;

[AttributeUsage(AttributeTargets.Assembly)]
public sealed class DesignTimeProviderServicesAttribute : Attribute
{
	public string TypeName { get; }

	public DesignTimeProviderServicesAttribute([JetBrains.Annotations.NotNull] string typeName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(typeName, "typeName");
		TypeName = typeName;
	}
}
