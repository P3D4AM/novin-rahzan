namespace Microsoft.EntityFrameworkCore.Design.Internal;

public class NullPluralizer : IPluralizer
{
	public virtual string Pluralize(string identifier)
	{
		return identifier;
	}

	public virtual string Singularize(string identifier)
	{
		return identifier;
	}
}
