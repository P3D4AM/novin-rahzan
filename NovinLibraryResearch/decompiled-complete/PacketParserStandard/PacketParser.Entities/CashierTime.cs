using System;

namespace PacketParser.Entities;

public class CashierTime
{
	public DateTime DTime1 { get; set; }

	public string Time1 => $"{DTime1.Hour}:{DTime1.Minute}";

	public DateTime DateTime1 => new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DTime1.Hour, DTime1.Minute, 0);

	public DateTime DTime2 { get; set; }

	public string Time2 => $"{DTime2.Hour}:{DTime2.Minute}";

	public DateTime DateTime2 => new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DTime2.Hour, DTime2.Minute, 0);
}
