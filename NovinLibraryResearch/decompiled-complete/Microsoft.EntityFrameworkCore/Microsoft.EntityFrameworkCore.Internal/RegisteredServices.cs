using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Internal;

public class RegisteredServices : IRegisteredServices
{
	public virtual ISet<Type> Services { get; }

	public RegisteredServices([JetBrains.Annotations.NotNull] IEnumerable<Type> services)
	{
		Services = new HashSet<Type>(services);
	}
}
