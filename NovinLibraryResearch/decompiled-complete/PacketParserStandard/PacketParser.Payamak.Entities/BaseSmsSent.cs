using System;
using PacketParser.Entities;
using PacketParser.Payamak.Interfaces.Entities;

namespace PacketParser.Payamak.Entities;

public class BaseSmsSent : BaseSms, ISmsSent, IBaseSms
{
	private string _senderNumber = "";

	public DateTime SendDateM { get; set; } = DateTime.Now;


	public string SendDateSh => SendDateM.MiladiToCalture();

	public string SendTime => SendDateM.ToShortTimeString();

	public DeliveryStatus DeliverySt { get; set; } = DeliveryStatus.Unknow;


	public string DeliveryStatusForDisplay => DeliverySt.GetDisplay().GetTranslate();

	public short DeliverySTShort => (short)DeliverySt;

	public bool DeliveryRequested { get; set; }

	public string RefrencedIds { get; set; } = "";


	public string SenderNumber
	{
		get
		{
			return _senderNumber ?? "";
		}
		set
		{
			_senderNumber = value.FixString();
		}
	}

	public string RefrenseAccountId { get; set; } = "";


	public Guid? RefrenceGuid { get; set; }

	public string RefrencedID
	{
		get
		{
			if (!RefrencedIds.Contains(","))
			{
				return RefrencedIds;
			}
			return RefrencedIds.Split(new char[1] { ',' })[0];
		}
	}
}
