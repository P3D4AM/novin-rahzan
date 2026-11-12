using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Internal;

public interface IDbSetSource
{
	object Create([JetBrains.Annotations.NotNull] DbContext context, [JetBrains.Annotations.NotNull] Type type);
}
