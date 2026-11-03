using System;
using System.ComponentModel;

namespace PacketParser.Settings.Accounting;

public class DateAndTimePriodSettings
{
	private DateTime? _startDate;

	private DateTime? _endDate;

	public DateTime ChequeAndInstallmentStartDate
	{
		get
		{
			if (!_startDate.HasValue)
			{
				_startDate = DateTime.Now.AddYears(-1);
			}
			return _startDate.Value;
		}
		set
		{
			_startDate = value;
		}
	}

	public DateTime ChequeAndInstallmentEndDate
	{
		get
		{
			if (!_endDate.HasValue)
			{
				_endDate = DateTime.Now.AddYears(10);
			}
			return _endDate.Value;
		}
		set
		{
			_endDate = value;
		}
	}

	[DefaultValue(90)]
	public int ResidMaxDays { get; set; } = 90;

}
