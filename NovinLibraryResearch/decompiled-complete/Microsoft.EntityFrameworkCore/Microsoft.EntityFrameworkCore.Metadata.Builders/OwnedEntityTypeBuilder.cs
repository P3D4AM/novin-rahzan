using System.ComponentModel;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OwnedEntityTypeBuilder
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override string ToString()
	{
		return base.ToString();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
public class OwnedEntityTypeBuilder<T> : OwnedEntityTypeBuilder
{
}
