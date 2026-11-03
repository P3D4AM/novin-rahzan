using System;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Tolid;

namespace EntityCache.Business.Accounting.Tolid;

public class TolidReportBusiness : ITolid, IHasJustGuid, IHasModified, IHasSanadNo
{
	public int Id2 { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public int SanadNo { get; set; }

	public SanadSt SanadSt { get; set; }

	public string SanadStName => SanadSt.GetDisplay().GetTranslate();

	public Guid CashierGuid { get; set; }

	public string Description { get; set; }

	public decimal SumMavad { get; set; }

	public decimal SumMahsol { get; set; }

	public decimal SumHazine { get; set; }

	public Guid MavadMoeinGuid { get; set; }

	public Guid? MavadTafsilGuid { get; set; }

	public Guid MahsolMoeinGuid { get; set; }

	public Guid? MahsolTafsilGuid { get; set; }

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool SanadAffective { get; set; }

	public bool KardexAffective { get; set; }
}
