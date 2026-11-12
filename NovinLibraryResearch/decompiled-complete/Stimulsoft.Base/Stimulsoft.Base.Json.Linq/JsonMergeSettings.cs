using System;

namespace Stimulsoft.Base.Json.Linq;

public class JsonMergeSettings
{
	private MergeArrayHandling _mergeArrayHandling;

	public MergeArrayHandling MergeArrayHandling
	{
		get
		{
			return _mergeArrayHandling;
		}
		set
		{
			if (value < MergeArrayHandling.Concat || value > MergeArrayHandling.Merge)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			_mergeArrayHandling = value;
		}
	}
}
