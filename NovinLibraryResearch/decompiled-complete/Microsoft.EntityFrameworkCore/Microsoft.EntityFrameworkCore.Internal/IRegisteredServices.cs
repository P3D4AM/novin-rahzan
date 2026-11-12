using System;
using System.Collections.Generic;

namespace Microsoft.EntityFrameworkCore.Internal;

public interface IRegisteredServices
{
	ISet<Type> Services { get; }
}
