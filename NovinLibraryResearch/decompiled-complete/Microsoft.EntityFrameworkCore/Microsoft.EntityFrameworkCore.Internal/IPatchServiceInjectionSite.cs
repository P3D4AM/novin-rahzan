using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Internal;

public interface IPatchServiceInjectionSite
{
	void InjectServices([JetBrains.Annotations.NotNull] IServiceProvider serviceProvider);
}
