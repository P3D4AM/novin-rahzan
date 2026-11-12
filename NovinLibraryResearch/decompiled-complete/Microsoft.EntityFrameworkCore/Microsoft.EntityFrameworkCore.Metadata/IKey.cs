using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IKey : IAnnotatable
{
	IReadOnlyList<IProperty> Properties { get; }

	IEntityType DeclaringEntityType { get; }
}
