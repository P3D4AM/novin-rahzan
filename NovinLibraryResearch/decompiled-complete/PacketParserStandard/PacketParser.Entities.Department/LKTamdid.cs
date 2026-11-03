using System;

namespace PacketParser.Entities.Department;

public class LKTamdid
{
	private long _LKSerial;

	public int ID { get; set; }

	public DateTime DateM { get; set; } = DateTime.Now;


	public long LKSerial
	{
		get
		{
			if (_LKSerial.ToString().Length > 12)
			{
				_LKSerial = _LKSerial.ToString().Substring(0, 12).ParseToLong(0L);
			}
			return _LKSerial;
		}
		set
		{
			_LKSerial = value;
		}
	}

	public decimal Price { get; set; }

	public DateTime TODate { get; set; } = DateTime.Now;


	public string TranInfo { get; set; } = "";


	public bool SaveOnServer { get; set; }

	public Guid GUID { get; set; } = Guid.Empty;


	public DateTime LastTransfer { get; set; } = DateTime.MinValue;

}
