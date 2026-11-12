namespace Force.Crc32;

internal class SafeProxyC : SafeProxy
{
	private const uint Poly = 2197175160u;

	internal SafeProxyC()
	{
		Init(2197175160u);
	}
}
