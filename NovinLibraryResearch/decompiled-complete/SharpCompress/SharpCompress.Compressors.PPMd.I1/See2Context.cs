namespace SharpCompress.Compressors.PPMd.I1;

internal class See2Context
{
	private const byte PERIOD_BIT_COUNT = 7;

	public ushort _summary;

	public byte _shift;

	public byte _count;

	public void Initialize(uint initialValue)
	{
		_shift = 3;
		_summary = (ushort)(initialValue << (int)_shift);
		_count = 7;
	}

	public uint Mean()
	{
		uint num = (uint)(_summary >> (int)_shift);
		_summary = (ushort)(_summary - num);
		return (uint)(num + ((num == 0) ? 1 : 0));
	}

	public void Update()
	{
		if (_shift < 7 && --_count == 0)
		{
			_summary += _summary;
			_count = (byte)(3 << (int)_shift++);
		}
	}
}
