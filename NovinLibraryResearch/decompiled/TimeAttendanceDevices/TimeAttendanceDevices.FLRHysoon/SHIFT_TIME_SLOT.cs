namespace TimeAttendanceDevices.FLRHysoon;

internal struct SHIFT_TIME_SLOT
{
	public byte AMStartH;

	public byte AMStartM;

	public byte AMEndH;

	public byte AMEndM;

	public byte PMStartH;

	public byte PMStartM;

	public byte PMEndH;

	public byte PMEndM;

	public byte OVStartH;

	public byte OVStartM;

	public byte OVEndH;

	public byte OVEndM;

	public void Init()
	{
		AMStartH = 0;
		AMStartM = 0;
		AMEndH = 0;
		AMEndM = 0;
		PMStartH = 0;
		PMStartM = 0;
		PMEndH = 0;
		PMEndM = 0;
		OVStartH = 0;
		OVStartM = 0;
		OVEndH = 0;
		OVEndM = 0;
	}
}
