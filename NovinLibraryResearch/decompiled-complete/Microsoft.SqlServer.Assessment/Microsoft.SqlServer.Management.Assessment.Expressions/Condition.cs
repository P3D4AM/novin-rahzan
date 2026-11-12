using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment.Expressions;

public class Condition : Expression, IEquatable<Condition>
{
	public Expression Implementation { get; }

	public Condition(Expression implementation)
	{
		Implementation = implementation ?? throw new ArgumentNullException("implementation");
	}

	public bool Equals(Condition other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		return object.Equals(Implementation, other.Implementation);
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (obj.GetType() != GetType())
		{
			return false;
		}
		return Equals((Condition)obj);
	}

	public override int GetHashCode()
	{
		return Implementation.GetHashCode();
	}

	internal override object EvaluateCore(IEvaluationContext data)
	{
		object value = Implementation.Evaluate(data);
		return value.Normalize<bool>();
	}

	internal override IEnumerable<string> GetNames()
	{
		return Implementation.GetNames();
	}
}
