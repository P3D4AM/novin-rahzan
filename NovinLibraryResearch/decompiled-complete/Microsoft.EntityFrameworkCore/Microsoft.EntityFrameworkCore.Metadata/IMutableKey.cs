using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IMutableKey : IMutableAnnotatable, IAnnotatable, IKey
{
	new IReadOnlyList<IMutableProperty> Properties { get; }

	new IMutableEntityType DeclaringEntityType { get; }
}
