using System;
using System.Collections.Generic;
using System.Data.Common;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage.Internal;

public class CompositeRelationalParameter : RelationalParameterBase
{
	public override string InvariantName { get; }

	public virtual IReadOnlyList<IRelationalParameter> RelationalParameters { get; }

	public CompositeRelationalParameter([JetBrains.Annotations.NotNull] string invariantName, [JetBrains.Annotations.NotNull] IReadOnlyList<IRelationalParameter> relationalParameters)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(invariantName, "invariantName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(relationalParameters, "relationalParameters");
		InvariantName = invariantName;
		RelationalParameters = relationalParameters;
	}

	public override void AddDbParameter(DbCommand command, object value)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(command, "command");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(value, "value");
		if (value is object[] array)
		{
			if (array.Length < RelationalParameters.Count)
			{
				throw new InvalidOperationException(RelationalStrings.MissingParameterValue(RelationalParameters[array.Length].InvariantName));
			}
			for (int i = 0; i < RelationalParameters.Count; i++)
			{
				RelationalParameters[i].AddDbParameter(command, array[i]);
			}
			return;
		}
		throw new InvalidOperationException(RelationalStrings.ParameterNotObjectArray(InvariantName));
	}
}
