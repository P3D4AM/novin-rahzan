using System;
using Ninject.Activation;
using Ninject.Infrastructure;
using Ninject.Planning.Targets;

namespace Ninject.Parameters;

public class TypeMatchingConstructorArgument : IConstructorArgument, IParameter, IEquatable<IParameter>
{
	private readonly Type type;

	public string Name
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public bool ShouldInherit { get; private set; }

	private Func<IContext, ITarget, object> ValueCallback { get; set; }

	public TypeMatchingConstructorArgument(Type type, Func<IContext, ITarget, object> valueCallback)
		: this(type, valueCallback, shouldInherit: false)
	{
	}

	public TypeMatchingConstructorArgument(Type type, Func<IContext, ITarget, object> valueCallback, bool shouldInherit)
	{
		Ensure.ArgumentNotNull(type, "type");
		Ensure.ArgumentNotNull(valueCallback, "valueCallback");
		ValueCallback = valueCallback;
		ShouldInherit = shouldInherit;
		this.type = type;
	}

	public bool AppliesToTarget(IContext context, ITarget target)
	{
		return target.Type == type;
	}

	public object GetValue(IContext context, ITarget target)
	{
		Ensure.ArgumentNotNull(context, "context");
		return ValueCallback(context, target);
	}

	public bool Equals(IParameter other)
	{
		if (other is TypeMatchingConstructorArgument typeMatchingConstructorArgument)
		{
			return typeMatchingConstructorArgument.type == type;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is IParameter other))
		{
			return this == obj;
		}
		return Equals(other);
	}

	public override int GetHashCode()
	{
		return GetType().GetHashCode() ^ type.GetHashCode();
	}
}
