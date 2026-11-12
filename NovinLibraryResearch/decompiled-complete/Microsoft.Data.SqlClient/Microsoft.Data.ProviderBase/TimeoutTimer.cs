using Microsoft.Data.Common;

namespace Microsoft.Data.ProviderBase;

internal class TimeoutTimer
{
	private long _timerExpire;

	private bool _isInfiniteTimeout;

	private long _originalTimerTicks;

	internal static readonly long InfiniteTimeout;

	internal bool IsExpired
	{
		get
		{
			if (!IsInfinite)
			{
				return ADP.TimerHasExpired(_timerExpire);
			}
			return false;
		}
	}

	internal bool IsInfinite => _isInfiniteTimeout;

	internal long LegacyTimerExpire
	{
		get
		{
			if (!_isInfiniteTimeout)
			{
				return _timerExpire;
			}
			return long.MaxValue;
		}
	}

	internal long MillisecondsRemaining
	{
		get
		{
			long num;
			if (_isInfiniteTimeout)
			{
				num = long.MaxValue;
			}
			else
			{
				num = ADP.TimerRemainingMilliseconds(_timerExpire);
				if (0 > num)
				{
					num = 0L;
				}
			}
			return num;
		}
	}

	internal static TimeoutTimer StartSecondsTimeout(int seconds)
	{
		TimeoutTimer timeoutTimer = new TimeoutTimer();
		timeoutTimer.SetTimeoutSeconds(seconds);
		return timeoutTimer;
	}

	internal static TimeoutTimer StartMillisecondsTimeout(long milliseconds)
	{
		TimeoutTimer timeoutTimer = new TimeoutTimer();
		timeoutTimer._originalTimerTicks = milliseconds * 10000;
		timeoutTimer._timerExpire = checked(ADP.TimerCurrent() + timeoutTimer._originalTimerTicks);
		timeoutTimer._isInfiniteTimeout = false;
		return timeoutTimer;
	}

	internal void SetTimeoutSeconds(int seconds)
	{
		if (InfiniteTimeout == seconds)
		{
			_isInfiniteTimeout = true;
			return;
		}
		_originalTimerTicks = ADP.TimerFromSeconds(seconds);
		_timerExpire = checked(ADP.TimerCurrent() + _originalTimerTicks);
		_isInfiniteTimeout = false;
	}

	internal void Reset()
	{
		if (InfiniteTimeout == _originalTimerTicks)
		{
			_isInfiniteTimeout = true;
			return;
		}
		_timerExpire = checked(ADP.TimerCurrent() + _originalTimerTicks);
		_isInfiniteTimeout = false;
	}
}
