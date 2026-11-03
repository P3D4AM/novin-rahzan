using System;
using PacketParser.Entities;

namespace AccSqlServerPersistence.ModelClass;

public class SanadReportModel
{
	public DateTime DateM { get; set; }

	public string DateSh { get; set; }

	public string Description { get; set; }

	public int SanadNo { get; set; }

	public SanadSt SanadSt { get; set; }

	public SanadType SanadType { get; set; }

	public Guid UserGuid { get; set; }

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public decimal Sum { get; set; }
}
