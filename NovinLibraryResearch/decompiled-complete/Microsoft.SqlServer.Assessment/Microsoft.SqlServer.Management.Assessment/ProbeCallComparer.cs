using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment;

internal class ProbeCallComparer : IEqualityComparer<ProbeCall>
{
	public static ProbeCallComparer Instance { get; } = new ProbeCallComparer();


	public bool Equals(ProbeCall x, ProbeCall y)
	{
		if (x == null)
		{
			return y == null;
		}
		if (y == null)
		{
			return false;
		}
		if (string.CompareOrdinal(x.Name, y.Name) != 0)
		{
			return false;
		}
		if (x.Parameters.Count != y.Parameters.Count)
		{
			return false;
		}
		return x.Parameters.All((KeyValuePair<string, object> p) => HasSameParameter(y, p));
	}

	public int GetHashCode(ProbeCall obj)
	{
		int num = obj.Name?.GetHashCode() ?? 0;
		foreach (string item in obj.Parameters.Keys.OrderBy((string s) => s))
		{
			num = (num * 397) ^ item.GetHashCode();
			num = ((num * 397) ^ obj.Parameters[item]?.GetHashCode()).GetValueOrDefault();
		}
		return num;
	}

	private static bool HasSameParameter(ProbeCall call, KeyValuePair<string, object> parameter)
	{
		if (call.Parameters.TryGetValue(parameter.Key, out var value))
		{
			return parameter.Value?.Equals(value) ?? (value == null);
		}
		return false;
	}
}
