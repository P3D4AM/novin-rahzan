using System.Data.Common;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage.Internal;

public class TypeMappedRelationalParameter : RelationalParameterBase
{
	public override string InvariantName { get; }

	public virtual string Name { get; }

	internal RelationalTypeMapping RelationalTypeMapping { get; }

	internal bool? IsNullable { get; }

	public TypeMappedRelationalParameter([JetBrains.Annotations.NotNull] string invariantName, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] RelationalTypeMapping relationalTypeMapping, bool? nullable)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(invariantName, "invariantName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(relationalTypeMapping, "relationalTypeMapping");
		InvariantName = invariantName;
		Name = name;
		RelationalTypeMapping = relationalTypeMapping;
		IsNullable = nullable;
	}

	public override void AddDbParameter(DbCommand command, object value)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(command, "command");
		command.Parameters.Add(RelationalTypeMapping.CreateParameter(command, Name, value, IsNullable));
	}
}
