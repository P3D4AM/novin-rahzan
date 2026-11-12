using System;

namespace Microsoft.SqlServer.Management.Assessment.Configuration;

internal class ComplementaryPattern<T> : IPattern<T>
{
	public IPattern<T> Original { get; }

	public ComplementaryPattern(IPattern<T> original)
	{
		Original = original ?? throw new ArgumentNullException("original");
	}

	public bool IsMatch(T value)
	{
		return !Original.IsMatch(value);
	}
}
