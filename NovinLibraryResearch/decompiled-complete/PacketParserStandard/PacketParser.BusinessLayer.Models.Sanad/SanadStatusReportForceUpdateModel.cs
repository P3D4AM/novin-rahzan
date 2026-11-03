using System;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Sanad;

public class SanadStatusReportForceUpdateModel
{
	public Guid SanadGuid { get; set; }

	public Document SanadType { get; set; }

	public int CurrentStatusId { get; set; }

	public int TargetStatusId { get; set; }

	public DateTime SanadDate { get; set; }

	public DateTime ActivityDate { get; set; }

	public int InactivityDays { get; set; }
}
