using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment.Configuration;

internal abstract class CompositePattern<T> : IPattern<T>
{
	public List<IPattern<T>> Arguments { get; } = new List<IPattern<T>>();


	protected CompositePattern()
	{
	}

	protected CompositePattern(IEnumerable<IPattern<T>> arguments)
	{
		if (arguments == null)
		{
			throw new ArgumentNullException("arguments");
		}
		Arguments.AddRange(arguments);
	}

	public abstract bool IsMatch(T value);

	protected abstract CompositePattern<T> Copy();
}
