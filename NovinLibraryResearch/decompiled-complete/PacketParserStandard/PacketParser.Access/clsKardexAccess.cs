using System;
using System.ComponentModel;
using PacketParser.Entities;

namespace PacketParser.Access;

[Serializable]
public class clsKardexAccess
{
	public Guid Guid { get; set; }

	public string Name { get; set; }

	public EnKardexAccessType Type { get; set; } = EnKardexAccessType.PrdGroup;


	[DefaultValue(true)]
	public bool Rialy_Buy_Vorod { get; set; } = true;


	[DefaultValue(true)]
	public bool Rialy_BackSell_Vorod { get; set; } = true;


	[DefaultValue(true)]
	public bool Rialy_Anbargardani_Ezafat_Vorod { get; set; } = true;


	[DefaultValue(true)]
	public bool Rialy_Tolid_Mahsol_Vorod { get; set; } = true;


	[DefaultValue(true)]
	public bool Rialy_Enteghal_Vorod { get; set; } = true;


	[DefaultValue(true)]
	public bool Rialy_Anbargardani_Kasr_khoroj { get; set; } = true;


	[DefaultValue(true)]
	public bool Rialy_Sell_Khoroj { get; set; } = true;


	[DefaultValue(true)]
	public bool Rialy_BackBuy_Khoroj { get; set; } = true;


	[DefaultValue(true)]
	public bool Rialy_Zayeat_Khoroj { get; set; } = true;


	[DefaultValue(true)]
	public bool Rialy_Tolid_Mavad_Khoroj { get; set; } = true;


	[DefaultValue(true)]
	public bool Rialy_Enteghal_Khoroj { get; set; } = true;


	[DefaultValue(true)]
	public bool Rialy_Sood { get; set; } = true;


	[DefaultValue(true)]
	public bool Rialy_TotalSood { get; set; } = true;


	[DefaultValue(true)]
	public bool Rialy_baha_Vahed { get; set; } = true;


	[DefaultValue(true)]
	public bool Rialy_Baha_Finished { get; set; } = true;


	[DefaultValue(true)]
	public bool Rialy_Fi_Kardex { get; set; } = true;


	[DefaultValue(true)]
	public bool Rialy_Bazaryab { get; set; } = true;


	[DefaultValue(true)]
	public bool Rialy_Takhfif { get; set; } = true;


	[DefaultValue(true)]
	public bool Qt_Buy_Vorod { get; set; } = true;


	[DefaultValue(true)]
	public bool Qt_BackSell_Vorod { get; set; } = true;


	[DefaultValue(true)]
	public bool Qt_Anbargardani_Ezafat_Vorod { get; set; } = true;


	[DefaultValue(true)]
	public bool Qt_Tolid_Mahsol_Vorod { get; set; } = true;


	[DefaultValue(true)]
	public bool Qt_Enteghal_Vorod { get; set; } = true;


	[DefaultValue(true)]
	public bool Qt_Anbargardani_Kasr_Khoroj { get; set; } = true;


	[DefaultValue(true)]
	public bool Qt_Sell_Khoroj { get; set; } = true;


	[DefaultValue(true)]
	public bool Qt_BackBuy_Khoroj { get; set; } = true;


	[DefaultValue(true)]
	public bool Qt_Zayeat_Khoroj { get; set; } = true;


	[DefaultValue(true)]
	public bool Qt_Tolid_Mavad_Khoroj { get; set; } = true;


	[DefaultValue(true)]
	public bool Qt_Enteghal_Khoroj { get; set; } = true;


	[DefaultValue(true)]
	public bool Qt_Mande { get; set; } = true;


	public clsKardexAccess()
	{
	}

	public clsKardexAccess(Guid guid, string name, EnKardexAccessType type)
	{
		Guid = guid;
		Name = name;
		Type = type;
	}
}
