using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations;

[AttributeUsage(AttributeTargets.Class)]
public sealed class MigrationAttribute : Attribute
{
	public string Id { get; }

	public MigrationAttribute([JetBrains.Annotations.NotNull] string id)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(id, "id");
		Id = id;
	}
}
