using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IMutableIndex : IIndex, IAnnotatable, IMutableAnnotatable
{
	new bool IsUnique { get; set; }

	new IReadOnlyList<IMutableProperty> Properties { get; }

	new IMutableEntityType DeclaringEntityType { get; }
}
