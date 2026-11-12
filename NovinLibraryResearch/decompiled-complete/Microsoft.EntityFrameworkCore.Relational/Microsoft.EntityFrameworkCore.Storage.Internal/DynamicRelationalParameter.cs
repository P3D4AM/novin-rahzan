using System;
using System.Data.Common;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage.Internal;

public class DynamicRelationalParameter : RelationalParameterBase
{
	private readonly IRelationalTypeMappingSource _typeMappingSource;

	public override string InvariantName { get; }

	public virtual string Name { get; }

	public DynamicRelationalParameter([JetBrains.Annotations.NotNull] string invariantName, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] IRelationalTypeMappingSource typeMappingSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(invariantName, "invariantName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(typeMappingSource, "typeMappingSource");
		InvariantName = invariantName;
		Name = name;
		_typeMappingSource = typeMappingSource;
	}

	public override void AddDbParameter(DbCommand command, object value)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(command, "command");
		if (value == null)
		{
			command.Parameters.Add(_typeMappingSource.GetMappingForValue(null).CreateParameter(command, Name, null));
			return;
		}
		if (value is DbParameter value2)
		{
			command.Parameters.Add(value2);
			return;
		}
		Type type = value.GetType();
		command.Parameters.Add(_typeMappingSource.GetMapping(type).CreateParameter(command, Name, value, type.IsNullableType()));
	}
}
