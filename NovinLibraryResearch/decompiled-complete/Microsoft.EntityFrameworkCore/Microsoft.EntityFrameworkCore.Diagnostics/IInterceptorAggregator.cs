using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public interface IInterceptorAggregator
{
	Type InterceptorType { get; }

	IInterceptor AggregateInterceptors([JetBrains.Annotations.NotNull] IReadOnlyList<IInterceptor> interceptors);
}
