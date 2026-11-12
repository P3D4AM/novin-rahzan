using System.Globalization;

namespace Microsoft.EntityFrameworkCore.Storage;

public class ParameterNameGenerator
{
	private int _count;

	public virtual string GenerateNext()
	{
		return string.Format(CultureInfo.InvariantCulture, "p{0}", _count++);
	}

	public virtual void Reset()
	{
		_count = 0;
	}
}
