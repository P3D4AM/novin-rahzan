using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Design;

public interface IPluralizer
{
	string Pluralize([JetBrains.Annotations.CanBeNull] string identifier);

	string Singularize([JetBrains.Annotations.CanBeNull] string identifier);
}
