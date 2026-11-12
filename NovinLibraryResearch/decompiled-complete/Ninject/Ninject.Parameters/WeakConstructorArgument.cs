using System;
using Ninject.Activation;
using Ninject.Planning.Targets;

namespace Ninject.Parameters;

public class WeakConstructorArgument : Parameter, IConstructorArgument, IParameter, IEquatable<IParameter>
{
	private readonly WeakReference weakReference;

	public WeakConstructorArgument(string name, object value)
		: this(name, value, shouldInherit: false)
	{
	}

	public WeakConstructorArgument(string name, object value, bool shouldInherit)
		: base(name, value, shouldInherit)
	{
		weakReference = new WeakReference(value);
		base.ValueCallback = (IContext ctx, ITarget target) => weakReference.Target;
	}

	public bool AppliesToTarget(IContext context, ITarget target)
	{
		return string.Equals(base.Name, target.Name);
	}
}
