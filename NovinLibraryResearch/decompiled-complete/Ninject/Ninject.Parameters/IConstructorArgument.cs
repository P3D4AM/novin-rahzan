using System;
using Ninject.Activation;
using Ninject.Planning.Targets;

namespace Ninject.Parameters;

public interface IConstructorArgument : IParameter, IEquatable<IParameter>
{
	bool AppliesToTarget(IContext context, ITarget target);
}
