using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage.Internal;

public abstract class RelationalParameterBase : IRelationalParameter
{
	public virtual string InvariantName { get; }

	public abstract void AddDbParameter(DbCommand command, object value);

	public virtual void AddDbParameter(DbCommand command, IReadOnlyDictionary<string, object> parameterValues)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(command, "command");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(parameterValues, "parameterValues");
		if (parameterValues.TryGetValue(InvariantName, out var value))
		{
			AddDbParameter(command, value);
			return;
		}
		throw new InvalidOperationException(RelationalStrings.MissingParameterValue(InvariantName));
	}
}
