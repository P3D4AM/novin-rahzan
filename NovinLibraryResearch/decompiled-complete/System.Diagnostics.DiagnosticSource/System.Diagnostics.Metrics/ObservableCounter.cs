using System.Collections.Generic;
using System.Security;

namespace System.Diagnostics.Metrics;

[SecuritySafeCritical]
public sealed class ObservableCounter<T> : ObservableInstrument<T> where T : struct
{
	private object _callback;

	internal ObservableCounter(Meter meter, string name, Func<T> observeValue, string unit, string description)
		: base(meter, name, unit, description)
	{
		_callback = observeValue ?? throw new ArgumentNullException("observeValue");
		Publish();
	}

	internal ObservableCounter(Meter meter, string name, Func<Measurement<T>> observeValue, string unit, string description)
		: base(meter, name, unit, description)
	{
		_callback = observeValue ?? throw new ArgumentNullException("observeValue");
		Publish();
	}

	internal ObservableCounter(Meter meter, string name, Func<IEnumerable<Measurement<T>>> observeValues, string unit, string description)
		: base(meter, name, unit, description)
	{
		_callback = observeValues ?? throw new ArgumentNullException("observeValues");
		Publish();
	}

	protected override IEnumerable<Measurement<T>> Observe()
	{
		return ObservableInstrument<T>.Observe(_callback);
	}
}
