using System;
using System.ComponentModel;

namespace PacketParser.Settings.Accounting;

public class SanadSettings
{
	[DefaultValue(2)]
	public short GroupLen { get; set; } = 2;


	[DefaultValue(4)]
	public short KolLen { get; set; } = 4;


	[DefaultValue(7)]
	public short MoeinLen { get; set; } = 7;


	[DefaultValue(6)]
	public short TafsilLen { get; set; } = 6;


	[DefaultValue(true)]
	public bool MoeinNotGroupTafsil { get; set; } = true;


	[Obsolete("removed, but needed for json data for old clients", false)]
	public DateTime StartMaliYearDate { get; set; } = (DateTime.Now.MiladiToCalture().Substring(0, 4) + "/01/01").ShamsiToMiladi();


	[Obsolete("removed, but needed for json data for old clients", false)]
	public DateTime EndMaliYearDate { get; set; } = (DateTime.Now.MiladiToCalture().Substring(0, 4) + "/12/" + Calendar.EndDayOfMonth(DateTime.Now.MiladiToCalture().Substring(0, 4).ParseToShort(0), 12)).ShamsiToMiladi();

}
