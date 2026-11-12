using System;
using System.Reflection;
using Ninject.Activation;
using Ninject.Planning.Bindings;

namespace Ninject.Planning.Targets;

public interface ITarget : ICustomAttributeProvider
{
	Type Type { get; }

	string Name { get; }

	MemberInfo Member { get; }

	Func<IBindingMetadata, bool> Constraint { get; }

	bool IsOptional { get; }

	bool HasDefaultValue { get; }

	object DefaultValue { get; }

	object ResolveWithin(IContext parent);
}
