using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.ValueGeneration;

public class HiLoValueGeneratorState
{
	private class HiLoValue
	{
		public long Low { get; }

		public long High { get; }

		public HiLoValue(long low, long high)
		{
			Low = low;
			High = high;
		}

		public HiLoValue NextValue()
		{
			return new HiLoValue(Low + 1, High);
		}
	}

	private readonly AsyncLock _asyncLock = new AsyncLock();

	private HiLoValue _currentValue;

	private readonly int _blockSize;

	public HiLoValueGeneratorState(int blockSize)
	{
		if (blockSize <= 0)
		{
			throw new ArgumentOutOfRangeException("blockSize", CoreStrings.HiLoBadBlockSize);
		}
		_blockSize = blockSize;
		_currentValue = new HiLoValue(-1L, 0L);
	}

	public virtual TValue Next<TValue>([JetBrains.Annotations.NotNull] Func<long> getNewLowValue)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(getNewLowValue, "getNewLowValue");
		HiLoValue hiLoValue = GetNextValue();
		while (hiLoValue.Low >= hiLoValue.High)
		{
			using (_asyncLock.Lock())
			{
				if (hiLoValue.High == _currentValue.High)
				{
					long num = getNewLowValue();
					hiLoValue = (_currentValue = new HiLoValue(num, num + _blockSize));
				}
				else
				{
					hiLoValue = GetNextValue();
				}
			}
		}
		return ConvertResult<TValue>(hiLoValue);
	}

	public virtual async ValueTask<TValue> NextAsync<TValue>([JetBrains.Annotations.NotNull] Func<CancellationToken, Task<long>> getNewLowValue, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(getNewLowValue, "getNewLowValue");
		HiLoValue newValue = GetNextValue();
		while (newValue.Low >= newValue.High)
		{
			using (await _asyncLock.LockAsync(cancellationToken))
			{
				if (newValue.High == _currentValue.High)
				{
					long num = await getNewLowValue(cancellationToken);
					newValue = (_currentValue = new HiLoValue(num, num + _blockSize));
				}
				else
				{
					newValue = GetNextValue();
				}
			}
		}
		return ConvertResult<TValue>(newValue);
	}

	private static TValue ConvertResult<TValue>(HiLoValue newValue)
	{
		return (TValue)Convert.ChangeType(newValue.Low, typeof(TValue), CultureInfo.InvariantCulture);
	}

	private HiLoValue GetNextValue()
	{
		HiLoValue currentValue;
		HiLoValue hiLoValue;
		do
		{
			currentValue = _currentValue;
			hiLoValue = currentValue.NextValue();
		}
		while (Interlocked.CompareExchange(ref _currentValue, hiLoValue, currentValue) != currentValue);
		return hiLoValue;
	}
}
