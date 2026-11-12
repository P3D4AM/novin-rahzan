using System;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.Caching.Memory;

public class MemoryCacheOptions : IOptions<MemoryCacheOptions>
{
	private long? _sizeLimit;

	private double _compactionPercentage = 0.05;

	public ISystemClock Clock { get; set; }

	public TimeSpan ExpirationScanFrequency { get; set; } = TimeSpan.FromMinutes(1.0);


	public long? SizeLimit
	{
		get
		{
			return _sizeLimit;
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value", value, "value must be non-negative.");
			}
			_sizeLimit = value;
		}
	}

	public double CompactionPercentage
	{
		get
		{
			return _compactionPercentage;
		}
		set
		{
			if (value < 0.0 || value > 1.0)
			{
				throw new ArgumentOutOfRangeException("value", value, "value must be between 0 and 1 inclusive.");
			}
			_compactionPercentage = value;
		}
	}

	MemoryCacheOptions IOptions<MemoryCacheOptions>.Value => this;
}
