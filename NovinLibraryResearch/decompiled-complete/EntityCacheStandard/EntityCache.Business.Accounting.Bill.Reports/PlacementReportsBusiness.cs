using System;
using PacketParser;
using PacketParser.Entities;

namespace EntityCache.Business.Accounting.Bill.Reports;

public class PlacementReportsBusiness
{
	public Guid Guid { get; set; }

	public string PlacementGroupName { get; set; }

	public int Number { get; set; }

	public int ItemCount { get; set; }

	public bool IsChecked { get; set; }

	public bool Status { get; set; }

	public Guid? MasterGuid { get; set; }

	public decimal SumPrice { get; set; }

	public decimal ExtraPrice { get; set; }

	public string Title { get; set; }

	public string TitleForDisplay
	{
		get
		{
			string result = Title;
			if (LastReserveStartDate.HasValue && LastReserveEndDate.HasValue)
			{
				string text = "";
				string text2 = "";
				if (!string.IsNullOrEmpty(LastReserveDescription))
				{
					text = "** " + LastReserveDescription;
				}
				if (LastReservePeopleCount != 0)
				{
					text2 = "*" + "تعداد".GetTranslate() + ":" + $" {LastReservePeopleCount}" + "نفر".GetTranslate();
				}
				result = (Title ?? "").GetTranslate() + "(" + "رزرو شماره".GetTranslate() + " " + $" {LastReserveNumber}" + " " + " توسط".GetTranslate() + "  " + LastReserveTafsilName + " " + "در تاریخ".GetTranslate() + "  " + LastReserveStartDate.Value.MiladiToCalture() + " " + " از ساعت".GetTranslate() + $" {LastReserveStartDate.Value.Hour}:{LastReserveStartDate.Value.Minute}" + " " + " الی".GetTranslate() + " " + $" {LastReserveEndDate.Value.Hour}:{LastReserveEndDate.Value.Minute} {text2} {text})";
			}
			return result;
		}
	}

	public string Description { get; set; }

	public EnPlacementStatus State { get; set; }

	public string StateForDisplay => State.GetDisplay().GetTranslate();

	public string HallName { get; set; }

	public DateTime? LastReserveStartDate { get; set; }

	public DateTime? LastReserveEndDate { get; set; }

	public string LastReserveTafsilName { get; set; }

	public string LastReserveDescription { get; set; }

	public int LastReservePeopleCount { get; set; }

	public int LastReserveNumber { get; set; }
}
