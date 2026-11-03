using System;
using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.Filters.Crm;

public class CallingLogFilter
{
	public List<EnumNotification>? Status { get; set; }

	public bool? IsIncoming { get; set; }

	public bool? IsCompleteCall { get; set; }

	public Guid? TicketGuid { get; set; }

	public bool? IsNullMasterUuid { get; set; }

	public string MasterUuid { get; set; }

	public TimeSpan? WaitingTime { get; set; }

	public TimeSpan? DurationFrom { get; set; }

	public TimeSpan? DurationTo { get; set; }

	public double? DurationFromTime => DurationFrom?.TotalMinutes;

	public double? DurationToTime => DurationTo?.TotalMinutes;

	public TimeSpan? PickUpTime { get; set; }

	public TimeSpan? HangUpTime { get; set; }

	public DateTime? DateFrom { get; set; }

	public string CallFromName { get; set; }

	public GardeshHesab SelectionType { get; set; }

	public DateTime? DateTo { get; set; }

	public bool? HasTicket { get; set; }

	public short StatusId { get; set; }

	public bool? IsNullMessage { get; set; }

	public string Message { get; set; }

	public List<string> NumberString { get; set; }

	public List<Guid> CashierList { get; set; }

	public int? TakeCount { get; set; }

	public List<int> ExtentionNumberList { get; set; }
}
