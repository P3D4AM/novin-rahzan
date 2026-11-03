using System.Runtime.InteropServices;

internal struct POST_NAME
{
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
	public byte[] PostName;

	public void Init()
	{
		PostName = new byte[128];
	}
}
