using System;
using Microsoft.SqlServer.Management.Assessment.Checks;
using Microsoft.SqlServer.Management.Assessment.Expressions;
using Microsoft.SqlServer.Management.Assessment.Properties;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Customization;

internal sealed class CheckParameterAccessor : IChangeAccessor<Check, Expression>, IEquatable<CheckParameterAccessor>
{
	public string ParameterName { get; }

	public CheckParameterAccessor(string parameterName)
	{
		if (parameterName == null)
		{
			throw new ArgumentNullException("parameterName");
		}
		if (parameterName.Length == 0)
		{
			throw new ArgumentException(Resources.ErrorEmtyString, "parameterName");
		}
		ParameterName = parameterName;
	}

	public void SetValue(Check target, Expression newValue)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		target.Parameters[ParameterName] = newValue;
	}

	public Expression GetValue(Check target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		if (target.Parameters.TryGetValue(ParameterName, out var value))
		{
			return value;
		}
		return NullLiteralExpression.Instance;
	}

	public bool Equals(CheckParameterAccessor other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		return string.Equals(ParameterName, other.ParameterName, StringComparison.Ordinal);
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
		return Equals((CheckParameterAccessor)obj);
	}

	public override int GetHashCode()
	{
		return ParameterName.GetHashCode();
	}
}
