using System;
using Ninject.Activation;
using Ninject.Planning.Targets;

namespace Ninject.Parameters;

public class PropertyValue : Parameter, IPropertyValue, IParameter, IEquatable<IParameter>
{
	public PropertyValue(string name, object value)
		: base(name, value, shouldInherit: false)
	{
	}

	public PropertyValue(string name, Func<IContext, object> valueCallback)
		: base(name, valueCallback, shouldInherit: false)
	{
	}

	public PropertyValue(string name, Func<IContext, ITarget, object> valueCallback)
		: base(name, valueCallback, shouldInherit: false)
	{
	}
}
