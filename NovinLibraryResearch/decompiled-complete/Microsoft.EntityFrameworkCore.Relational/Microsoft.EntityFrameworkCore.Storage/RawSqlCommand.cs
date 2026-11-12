using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public class RawSqlCommand
{
	public virtual IRelationalCommand RelationalCommand { get; }

	public virtual IReadOnlyDictionary<string, object> ParameterValues { get; }

	public RawSqlCommand([JetBrains.Annotations.NotNull] IRelationalCommand relationalCommand, [JetBrains.Annotations.NotNull] IReadOnlyDictionary<string, object> parameterValues)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(relationalCommand, "relationalCommand");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(parameterValues, "parameterValues");
		RelationalCommand = relationalCommand;
		ParameterValues = parameterValues;
	}
}
