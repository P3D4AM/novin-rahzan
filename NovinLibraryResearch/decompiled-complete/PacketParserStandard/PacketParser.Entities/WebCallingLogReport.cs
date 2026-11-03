using System;
using System.Collections.Generic;

namespace PacketParser.Entities;

public class WebCallingLogReport
{
	public string Uuid { get; set; }

	public string? MasterUuid { get; set; }

	public Guid Guid { get; set; }

	public string CallerIdNumber { get; set; }

	public string NumberForDisplay { get; set; }

	public string DialedNumber { get; set; }

	public string Time { get; set; }

	public Guid? TicketGuid { get; set; }

	public double WaitingTime { get; set; }

	public EnumNotification Status { get; set; }

	public bool IsIncoming { get; set; }

	public bool IsCompleteCall { get; set; }

	public TimeSpan? PickUpTime { get; set; }

	public TimeSpan? HangUpTime { get; set; }

	public long CalcedNumber { get; set; }

	public string StatusName { get; set; }

	public EnConversation StatusConversation { get; set; }

	public byte[] SourceImage { get; set; }

	public List<Guid?> CashierList { get; set; }

	public DateTime DateM { get; set; }

	public string CallToName { get; set; }

	public short StatusId { get; set; }

	public string Message { get; set; }
}
