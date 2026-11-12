using System;
using Ninject.Activation;
using Ninject.Planning.Targets;

namespace Ninject.Parameters;

public class ConstructorArgument : Parameter, IConstructorArgument, IParameter, IEquatable<IParameter>
{
	public ConstructorArgument(string name, object value)
		: base(name, value, shouldInherit: false)
	{
	}

	public ConstructorArgument(string name, Func<IContext, object> valueCallback)
		: base(name, valueCallback, shouldInherit: false)
	{
	}

	public ConstructorArgument(string name, Func<IContext, ITarget, object> valueCallback)
		: base(name, valueCallback, shouldInherit: false)
	{
	}

	public ConstructorArgument(string name, object value, bool shouldInherit)
		: base(name, value, shouldInherit)
	{
	}

	public ConstructorArgument(string name, Func<IContext, object> valueCallback, bool shouldInherit)
		: base(name, valueCallback, shouldInherit)
	{
	}

	public ConstructorArgument(string name, Func<IContext, ITarget, object> valueCallback, bool shouldInherit)
		: base(name, valueCallback, shouldInherit)
	{
	}

	public bool AppliesToTarget(IContext context, ITarget target)
	{
		return string.Equals(base.Name, target.Name);
	}
}
