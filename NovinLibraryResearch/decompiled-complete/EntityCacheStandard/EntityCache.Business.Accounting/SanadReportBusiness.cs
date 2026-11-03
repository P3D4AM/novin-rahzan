using System;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.Accounting;

public class SanadReportBusiness : IHasContext
{
	public DateTime DateM { get; set; }

	public string DateSh { get; set; }

	public string Description { get; set; }

	public int SanadNo { get; set; }

	public int SanadId { get; set; }

	public SanadSt SanadSt { get; set; }

	public SanadType SanadType { get; set; }

	public SanadMode Mode { get; set; }

	public Guid UserGuid { get; set; }

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public decimal Sum { get; set; }

	public bool IsChecked { get; set; }

	public string SanadStName => SanadSt.GetDisplay();

	public string SanadTypeName => SanadType.GetDisplay();

	public string UserName { get; set; }

	public string ContextGuid { get; set; }

	public short StatusId { get; set; }

	public SanadReportBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
