using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata;

public class SimpleModelFactory
{
	public virtual IMutableModel Create()
	{
		return new Model(new ConventionSet());
	}
}
