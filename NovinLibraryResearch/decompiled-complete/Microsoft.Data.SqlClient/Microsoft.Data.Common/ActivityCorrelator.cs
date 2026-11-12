using System;
using System.Globalization;

namespace Microsoft.Data.Common;

internal static class ActivityCorrelator
{
	internal sealed class ActivityId
	{
		internal readonly Guid Id;

		internal readonly uint Sequence;

		internal ActivityId(uint sequence)
		{
			Id = Guid.NewGuid();
			Sequence = sequence;
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}:{1}", Id, Sequence);
		}
	}

	[ThreadStatic]
	private static ActivityId t_tlsActivity;

	internal static ActivityId Current
	{
		get
		{
			if (t_tlsActivity == null)
			{
				t_tlsActivity = new ActivityId(1u);
			}
			return t_tlsActivity;
		}
	}

	internal static ActivityId Next()
	{
		t_tlsActivity = new ActivityId((t_tlsActivity?.Sequence ?? 0) + 1);
		return t_tlsActivity;
	}
}
