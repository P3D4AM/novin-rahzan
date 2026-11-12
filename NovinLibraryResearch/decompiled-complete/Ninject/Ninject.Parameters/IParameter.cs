using System;
using Ninject.Activation;
using Ninject.Planning.Targets;

namespace Ninject.Parameters;

public interface IParameter : IEquatable<IParameter>
{
	string Name { get; }

	bool ShouldInherit { get; }

	object GetValue(IContext context, ITarget target);
}
