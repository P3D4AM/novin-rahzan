using System.Collections.Generic;

namespace PacketParser.Entities.Department;

public class LkInfoMeta
{
	public List<PspModel> PspList { get; set; }

	public bool Tax { get; set; }

	public bool Aghsat { get; set; }

	public bool Cheque_Khazane { get; set; }

	public bool CustomerClub { get; set; }

	public bool ProductDepot_Pishrafteh { get; set; }

	public bool Service { get; set; }

	public bool VoiceCall { get; set; }

	public bool ProductSerial { get; set; }

	public bool MultiCurrency { get; set; }

	public bool AdvancedFinance { get; set; }
}
