using System.Runtime.InteropServices;

internal struct TIME_ZONE
{
	public int Size;

	public int TimeZoneId;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
	public TIME_SLOT[] TimeSlots;

	public void Init()
	{
		Size = 32;
		TimeZoneId = 0;
		TimeSlots = new TIME_SLOT[6];
	}
}
