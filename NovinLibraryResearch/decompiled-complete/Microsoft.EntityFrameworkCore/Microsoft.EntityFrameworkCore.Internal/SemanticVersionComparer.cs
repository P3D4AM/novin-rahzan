using System;
using System.Collections.Generic;

namespace Microsoft.EntityFrameworkCore.Internal;

public class SemanticVersionComparer : IComparer<string>
{
	public virtual int Compare(string x, string y)
	{
		return CreateVersion(x).CompareTo(CreateVersion(y));
	}

	private static Version CreateVersion(string semanticVersion)
	{
		int num = semanticVersion.IndexOf("-", StringComparison.Ordinal);
		if (num != -1)
		{
			semanticVersion = semanticVersion.Substring(0, num);
		}
		return new Version(semanticVersion);
	}
}
