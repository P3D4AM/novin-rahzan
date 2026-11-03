namespace PacketParser.ErroHandler;

internal class SystemInfo
{
	private static long _LKSerial;

	public static long LKSerial
	{
		get
		{
			if (_LKSerial <= 0)
			{
				long num = 0L;
				if (num > 0)
				{
					_LKSerial = num;
				}
			}
			return _LKSerial;
		}
		set
		{
			_LKSerial = value;
		}
	}
}
