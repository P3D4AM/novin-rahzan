using System;
using Ninject.Activation;
using Ninject.Infrastructure;
using Ninject.Planning.Targets;

namespace Ninject.Parameters;

public class Parameter : IParameter, IEquatable<IParameter>
{
	public string Name { get; private set; }

	public bool ShouldInherit { get; private set; }

	public Func<IContext, ITarget, object> ValueCallback { get; internal set; }

	public Parameter(string name, object value, bool shouldInherit)
		: this(name, (IContext ctx, ITarget target) => value, shouldInherit)
	{
	}

	public Parameter(string name, Func<IContext, object> valueCallback, bool shouldInherit)
	{
		Ensure.ArgumentNotNullOrEmpty(name, "name");
		Ensure.ArgumentNotNull(valueCallback, "valueCallback");
		Name = name;
		ValueCallback = (IContext ctx, ITarget target) => valueCallback(ctx);
		ShouldInherit = shouldInherit;
	}

	public Parameter(string name, Func<IContext, ITarget, object> valueCallback, bool shouldInherit)
	{
		Ensure.ArgumentNotNullOrEmpty(name, "name");
		Ensure.ArgumentNotNull(valueCallback, "valueCallback");
		Name = name;
		ValueCallback = valueCallback;
		ShouldInherit = shouldInherit;
	}

	public object GetValue(IContext context, ITarget target)
	{
		Ensure.ArgumentNotNull(context, "context");
		return ValueCallback(context, target);
	}

	public override bool Equals(object obj)
	{
		if (!(obj is IParameter other))
		{
			return base.Equals(obj);
		}
		return Equals(other);
	}

	public override int GetHashCode()
	{
		return GetType().GetHashCode() ^ Name.GetHashCode();
	}

	public bool Equals(IParameter other)
	{
		if (other.GetType() == GetType())
		{
			return other.Name.Equals(Name);
		}
		return false;
	}
}
