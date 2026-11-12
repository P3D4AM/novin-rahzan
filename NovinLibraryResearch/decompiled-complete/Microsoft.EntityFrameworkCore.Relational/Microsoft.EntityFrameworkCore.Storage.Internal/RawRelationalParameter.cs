using System.Collections.Generic;
using System.Data.Common;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage.Internal;

public class RawRelationalParameter : RelationalParameterBase
{
	private readonly DbParameter _parameter;

	public override string InvariantName { get; }

	public RawRelationalParameter([JetBrains.Annotations.NotNull] string invariantName, [JetBrains.Annotations.NotNull] DbParameter parameter)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(invariantName, "invariantName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(parameter, "parameter");
		InvariantName = invariantName;
		_parameter = parameter;
	}

	public override void AddDbParameter(DbCommand command, IReadOnlyDictionary<string, object> parameterValues)
	{
		AddDbParameter(command, _parameter);
	}

	public override void AddDbParameter(DbCommand command, object value)
	{
		command.Parameters.Add(value);
	}
}
