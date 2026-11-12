using System;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;

namespace Microsoft.Extensions.FileSystemGlobbing.Internal.PatternContexts;

public class PatternContextLinearExclude : PatternContextLinear
{
	public PatternContextLinearExclude(ILinearPattern pattern)
		: base(pattern)
	{
	}

	public override bool Test(DirectoryInfoBase directory)
	{
		if (IsStackEmpty())
		{
			throw new InvalidOperationException(System.SR.CannotTestDirectory);
		}
		if (Frame.IsNotApplicable)
		{
			return false;
		}
		if (IsLastSegment())
		{
			return TestMatchingSegment(directory.Name);
		}
		return false;
	}
}
