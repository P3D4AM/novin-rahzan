using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ninject.Activation;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Introspection;
using Ninject.Infrastructure.Language;
using Ninject.Planning.Bindings;

namespace Ninject.Planning.Targets;

public abstract class Target<T> : ITarget, ICustomAttributeProvider where T : ICustomAttributeProvider
{
	private readonly Lazy<Func<IBindingMetadata, bool>> constraint;

	private readonly Lazy<bool> isOptional;

	public MemberInfo Member { get; private set; }

	public T Site { get; private set; }

	public abstract string Name { get; }

	public abstract Type Type { get; }

	public Func<IBindingMetadata, bool> Constraint => constraint.Value;

	public bool IsOptional => isOptional.Value;

	public virtual bool HasDefaultValue => false;

	public virtual object DefaultValue
	{
		get
		{
			throw new InvalidOperationException(ExceptionFormatter.TargetDoesNotHaveADefaultValue(this));
		}
	}

	protected Target(MemberInfo member, T site)
	{
		Ensure.ArgumentNotNull(member, "member");
		Ensure.ArgumentNotNull(site, "site");
		Member = member;
		Site = site;
		constraint = new Lazy<Func<IBindingMetadata, bool>>(ReadConstraintFromTarget);
		isOptional = new Lazy<bool>(ReadOptionalFromTarget);
	}

	public object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		Ensure.ArgumentNotNull(attributeType, "attributeType");
		return Site.GetCustomAttributesExtended(attributeType, inherit);
	}

	public object[] GetCustomAttributes(bool inherit)
	{
		return Site.GetCustomAttributes(inherit);
	}

	public bool IsDefined(Type attributeType, bool inherit)
	{
		Ensure.ArgumentNotNull(attributeType, "attributeType");
		return Site.IsDefined(attributeType, inherit);
	}

	public object ResolveWithin(IContext parent)
	{
		Ensure.ArgumentNotNull(parent, "parent");
		IRequest request = parent.Request.CreateChild(Type, parent, this);
		request.IsUnique = true;
		return parent.Kernel.Resolve(request).SingleOrDefault();
	}

	[Obsolete]
	protected virtual IEnumerable<object> GetValues(Type service, IContext parent)
	{
		Ensure.ArgumentNotNull(service, "service");
		Ensure.ArgumentNotNull(parent, "parent");
		IRequest request = parent.Request.CreateChild(service, parent, this);
		request.IsOptional = true;
		return parent.Kernel.Resolve(request);
	}

	[Obsolete]
	protected virtual object GetValue(Type service, IContext parent)
	{
		Ensure.ArgumentNotNull(service, "service");
		Ensure.ArgumentNotNull(parent, "parent");
		IRequest request = parent.Request.CreateChild(service, parent, this);
		request.IsUnique = true;
		return parent.Kernel.Resolve(request).SingleOrDefault();
	}

	protected virtual bool ReadOptionalFromTarget()
	{
		return Site.HasAttribute(typeof(OptionalAttribute));
	}

	protected virtual Func<IBindingMetadata, bool> ReadConstraintFromTarget()
	{
		ConstraintAttribute[] attributes = GetCustomAttributes(typeof(ConstraintAttribute), inherit: true) as ConstraintAttribute[];
		if (attributes == null || attributes.Length == 0)
		{
			return null;
		}
		if (attributes.Length == 1)
		{
			return attributes[0].Matches;
		}
		return (IBindingMetadata metadata) => attributes.All((ConstraintAttribute attribute) => attribute.Matches(metadata));
	}
}
