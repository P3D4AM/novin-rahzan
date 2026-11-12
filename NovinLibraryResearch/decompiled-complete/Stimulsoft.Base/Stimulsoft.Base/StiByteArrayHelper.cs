using System.Linq;

namespace Stimulsoft.Base;

public static class StiByteArrayHelper
{
	public static byte[] Split(byte[] a1, byte[] a2)
	{
		if (a1 == null)
		{
			return a2;
		}
		if (a2 == null)
		{
			return a1;
		}
		return a1.Concat(a2).ToArray();
	}
}
