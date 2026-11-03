using System;
using PacketParser;
using PacketParser.Entities;

namespace EntityCache.Business.Crm.Report;

public class CallingLogDetailReportBusiness
{
	public Guid MasterGuid { get; set; }

	public Guid? CashierGuid { get; set; }

	public DateTime? PickUpDateM { get; set; }

	public string PickUpDateSh => PickUpDateM.MiladiToCalture();

	public string PickUpTime => PickUpDateM?.ToLongTimeString();

	public DateTime? HangUpDateM { get; set; }

	public string HangUpDateSh => HangUpDateM.MiladiToCalture();

	public string HangUpDateTime => HangUpDateM?.ToLongTimeString();

	public DateTime DateInsert { get; set; }

	public DateTime Modified { get; set; }

	public string ModifiedSh => Modified.ToLongTimeString();

	public EnumNotification Status { get; set; }

	public string Uuid { get; set; }

	public string LinkedId { get; set; }

	public string CallFrom_Number { get; set; }

	public string CallFrom_Name { get; set; }

	public string CallTo_Number { get; set; }

	public string CallTo_Name { get; set; }

	public double? Duration { get; set; }

	public string Description { get; set; }

	public short Rings { get; set; }

	public short StatusId { get; set; }

	public string Message { get; set; }
}
