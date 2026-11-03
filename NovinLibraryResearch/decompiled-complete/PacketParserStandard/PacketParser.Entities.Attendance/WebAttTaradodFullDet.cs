using System;
using System.Collections.Generic;
using System.Drawing;
using PacketParser.Attendance;
using PacketParser.Attendance.InterFaces;

namespace PacketParser.Entities.Attendance;

public class WebAttTaradodFullDet : ITaradodFullDet
{
	private string _ekTime = "";

	private string _kkTime = "";

	private string _hozorTime = "";

	public List<WebAttCoupleTaradod> CoupleTaradods { get; set; }

	public List<WebAttPersonLeave> QuiddityList { get; set; }

	public bool ReloadNextRow { get; set; }

	public string ManulaEzafeKarTime { get; set; }

	public string ManulaKasreKarTime { get; set; }

	public string ManulaMissionTime { get; set; }

	public string ManulaVacationTime { get; set; }

	public string ManulaFridayTime { get; set; }

	public string ManulaShiftTime1 { get; set; }

	public string ManulaShiftTime2 { get; set; }

	public string ManulaShiftTime3 { get; set; }

	public string ManulaAbsentTime { get; set; }

	public string ManulaNightTime { get; set; }

	public Color ColorManulaEzafeKar { get; set; }

	public Color ColorManulaKasreKarTime { get; set; }

	public Color ColorManulaMissionTime { get; set; }

	public Color ColorManulaVacationTime { get; set; }

	public Color ColorManulaFridayTime { get; set; }

	public Color ColorManulaShiftTime1 { get; set; }

	public Color ColorManulaShiftTime2 { get; set; }

	public Color ColorManulaShiftTime3 { get; set; }

	public Color ColorManulaAbsentTime { get; set; }

	public Color ColorManulaNightTime { get; set; }

	public string KkTime
	{
		get
		{
			if (!string.IsNullOrEmpty(_kkTime))
			{
				return _kkTime;
			}
			return "00:00";
		}
		set
		{
			_kkTime = value;
		}
	}

	public string EkTime
	{
		get
		{
			if (!string.IsNullOrEmpty(_ekTime))
			{
				return _ekTime;
			}
			return "00:00";
		}
		set
		{
			_ekTime = value;
		}
	}

	public string HozorTime
	{
		get
		{
			if (!string.IsNullOrEmpty(_hozorTime))
			{
				return _hozorTime;
			}
			return "00:00";
		}
		set
		{
			_hozorTime = value;
		}
	}

	public string MorSaatiTime { get; set; }

	public string MissionTime { get; set; }

	public long CodeTafsil { get; set; }

	public string FullName { get; set; }

	public string Family { get; set; }

	public string FirstName { get; set; }

	public DateTime DateM { get; set; }

	public string ShiftTime { get; set; }

	public string GheybatTime { get; set; }

	public string Dayofweek { get; set; }

	public bool CanEditV { get; set; }

	public bool CanEditK { get; set; }

	public Color ColorEk { get; set; }

	public string DateSh { get; set; }

	public int DetailStatusTypeTaradod { get; set; }

	public string TatilRasmiToolTip { get; set; }

	public string MorakhasiToolTip { get; set; }

	public int GheybatMin { get; set; }

	public short DayofShMonth { get; set; }

	public int MonthOfDateSh => Calendar.GetMonthOfDate(DateSh);

	public int YearOfDateSh => Calendar.GetYearOfDateSh(DateSh);

	public Color ColorKk { get; set; }

	public int KasrKar { get; set; }

	public int KasreKarBeginShift { get; set; }

	public int JarimeRoz { get; set; }

	public int KasreKarEndShift { get; set; }

	public int KasreKarMidShift { get; set; }

	public int EzafeKar { get; set; }

	public int EzafeKarBeginShift { get; set; }

	public int EzafeKarEndShift { get; set; }

	public int EzafeKarMidShift { get; set; }

	public int Karkard { get; set; }

	public string KarkardForDisplay
	{
		get
		{
			if (Karkard <= 0)
			{
				return "00:00:00";
			}
			return Time.IntToTime(Karkard) + ":00";
		}
	}

	public int OnFriDayWorkKarkard { get; set; }

	public int OnTatilWorkKarkard { get; set; }

	public int ShiftWorkKarkard1 { get; set; }

	public int ShiftWorkKarkard2 { get; set; }

	public int ShiftWorkKarkard3 { get; set; }

	public int NightWorkKarkard { get; set; }

	public int LunchTime { get; set; }

	public int SumShiftMinuts { get; set; }

	public int SumMoazafiMinutes { get; set; }

	public StatuseTaradodEnumType StatuseTaradod { get; set; }

	public string StatuseTaradodForDisplay => StatuseTaradod.GetDisplay();

	public Color StatuseTaradodColor { get; set; }

	public Color StatuseTaradodForeColor { get; set; }

	public Color StatuseTaradodForeColorHeader { get; set; }

	public WebAttTaradodDet TaradodDet { get; set; }
}
