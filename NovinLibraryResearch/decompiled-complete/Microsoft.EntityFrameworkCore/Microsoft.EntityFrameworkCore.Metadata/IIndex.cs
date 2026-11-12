using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IIndex : IAnnotatable
{
	IReadOnlyList<IProperty> Properties { get; }

	bool IsUnique { get; }

	IEntityType DeclaringEntityType { get; }
}
