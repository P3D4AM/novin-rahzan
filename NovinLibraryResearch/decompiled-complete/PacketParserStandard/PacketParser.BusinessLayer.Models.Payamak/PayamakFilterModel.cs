using System;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Payamak;

public class PayamakFilterModel
{
	public DateTime DateM1 { get; set; }

	public DateTime DateM2 { get; set; }

	public DeliveryStatus Status { get; set; }

	public string Text { get; set; }

	public string MobF { get; set; }

	public string MobG { get; set; }

	public PayamakFilterModel(DateTime DateM1, DateTime DateM2, DeliveryStatus Status, string Text, string MobF, string MobG)
	{
		this.DateM1 = DateM1;
		this.DateM2 = DateM2;
		this.Status = Status;
		this.Text = Text;
		this.MobF = MobF;
		this.MobG = MobG;
	}
}
