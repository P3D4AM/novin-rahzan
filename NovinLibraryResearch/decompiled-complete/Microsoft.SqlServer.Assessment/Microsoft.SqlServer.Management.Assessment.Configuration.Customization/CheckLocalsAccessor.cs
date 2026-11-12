using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.Assessment.Checks;
using Microsoft.SqlServer.Management.Assessment.Expressions;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Customization;

internal sealed class CheckLocalsAccessor : IChangeAccessor<Check, IDictionary<string, Expression>>
{
	public static CheckLocalsAccessor Instance { get; } = new CheckLocalsAccessor();


	private CheckLocalsAccessor()
	{
	}

	public void SetValue(Check target, IDictionary<string, Expression> newValue)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		target.Locals?.Clear();
		if (newValue == null || newValue.Count <= 0)
		{
			return;
		}
		if (target.Locals == null)
		{
			target.Locals = new Dictionary<string, Expression>();
		}
		foreach (KeyValuePair<string, Expression> item in newValue)
		{
			target.Locals.Add(item);
		}
	}

	public IDictionary<string, Expression> GetValue(Check target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.Locals;
	}
}
