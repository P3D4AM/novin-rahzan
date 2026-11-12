using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Internal;

public interface IDbSetFinder
{
	IReadOnlyList<DbSetProperty> FindSets([JetBrains.Annotations.NotNull] Type contextType);
}
