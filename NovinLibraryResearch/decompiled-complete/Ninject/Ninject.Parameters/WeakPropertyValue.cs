using System;
using Ninject.Activation;
using Ninject.Planning.Targets;

namespace Ninject.Parameters;

public class WeakPropertyValue : Parameter, IPropertyValue, IParameter, IEquatable<IParameter>
{
	private readonly WeakReference weakReference;

	public WeakPropertyValue(string name, object value)
		: base(name, (object)null, shouldInherit: false)
	{
		weakReference = new WeakReference(value);
		base.ValueCallback = (IContext ctx, ITarget target) => weakReference.Target;
	}
}
