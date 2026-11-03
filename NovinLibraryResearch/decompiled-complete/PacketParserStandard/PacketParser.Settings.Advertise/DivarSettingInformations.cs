using PacketParser.Entities;

namespace PacketParser.Settings.Advertise;

public class DivarSettingInformations : Serializable<DivarSettingInformations>
{
	public int AdvCountInDay { get; set; }

	public int AdvCountInMonth { get; set; }

	public int DayCountForUpdateView { get; set; }

	public int DayCountForUpdateState { get; set; }

	public int DayCountForDelete { get; set; }

	public string Category1 { get; set; } = "";


	public string Category2 { get; set; } = "";


	public string Category3 { get; set; } = "";


	public int PicCountInPerAdv { get; set; }

	public int CountAdvInIp { get; set; }
}
