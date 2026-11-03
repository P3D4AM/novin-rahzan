using System;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Placement;

public class PlacementKardexViewModel
{
	public Guid MasterGuid { get; set; }

	public DateTime StartDate { get; set; }

	public string StartDateSh => $"{StartDate.Hour}:{StartDate.Minute} * {StartDate.MiladiToCalture()}";

	public DateTime? EndDate { get; set; }

	public string EndDateSh
	{
		get
		{
			if (!EndDate.HasValue)
			{
				return "";
			}
			return $"{EndDate.Value.Hour}:{EndDate.Value.Minute} * {EndDate.MiladiToCalture()}";
		}
	}

	public Guid TafsilGuid { get; set; }

	public string TafsilName { get; set; }

	public Guid CashierGuid { get; set; }

	public string CashierName { get; set; }

	public string Description { get; set; }

	public decimal Sum { get; set; }

	public int PeopleCount { get; set; }

	public int Number { get; set; }

	public Guid? ReceptionGuid { get; set; }

	public decimal ReceptionSum { get; set; }

	public EnPlacementStatus State { get; set; }
}
