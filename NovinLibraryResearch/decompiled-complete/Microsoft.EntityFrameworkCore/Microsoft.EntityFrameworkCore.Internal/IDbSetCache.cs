using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Internal;

public interface IDbSetCache
{
	object GetOrAddSet([JetBrains.Annotations.NotNull] IDbSetSource source, [JetBrains.Annotations.NotNull] Type type);
}
