using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Design;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class DesignTimeServicesReferenceAttribute : Attribute
{
	public string TypeName { get; }

	public string ForProvider { get; }

	public DesignTimeServicesReferenceAttribute([JetBrains.Annotations.NotNull] string typeName)
		: this(typeName, null)
	{
	}

	public DesignTimeServicesReferenceAttribute([JetBrains.Annotations.NotNull] string typeName, [JetBrains.Annotations.CanBeNull] string forProvider)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(typeName, "typeName");
		TypeName = typeName;
		ForProvider = forProvider;
	}
}
