using System.Security;

namespace System.Diagnostics;

[SecuritySafeCritical]
public readonly struct ActivityChangedEventArgs
{
	public Activity? Previous { get; init; }

	public Activity? Current { get; init; }

	internal ActivityChangedEventArgs(Activity previous, Activity current)
	{
		Previous = previous;
		Current = current;
	}
}
