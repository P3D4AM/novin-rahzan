using System;
using EntityCache.Utilities;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Entities;

namespace EntityCache.Business.Accounting;

public class PrdAnbarGardaniReportBusiness
{
	public Guid Guid { get; set; }

	public int Number { get; set; }

	public int SanadNo { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public string Time => DateM.ToShortTimeString();

	public string Description { get; set; }

	public decimal KasrSum { get; set; }

	public decimal EzafeSum { get; set; }

	public decimal KasrQt { get; set; }

	public decimal EzafeQt { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public byte[] ServerStatusImage
	{
		get
		{
			ServerStatusForClient serverStatus = ServerStatus;
			if ((serverStatus == ServerStatusForClient.Delivered || serverStatus == ServerStatusForClient.DirectDelivery) ? true : false)
			{
				return ImageResourceManager.ServerStatusDelivered;
			}
			if (ServerStatus == ServerStatusForClient.DeliveryError)
			{
				return ImageResourceManager.ServerStatusDeliveryFailed;
			}
			if (ServerStatus == ServerStatusForClient.Sent)
			{
				return ImageResourceManager.ServerStatusSent;
			}
			if (ServerStatus == ServerStatusForClient.SendError)
			{
				return ImageResourceManager.ServerStatusSentError;
			}
			return ImageResourceManager.ServerStatusNone;
		}
	}

	public string UserName { get; set; }

	public short StatusId { get; set; }

	public bool IsTemp { get; set; }

	public string DeliveryShortMessageForAndroid { get; set; } = "جای خالی را با بیل پر کنید";


	[JsonIgnore]
	public int StatusForOrdering
	{
		get
		{
			switch (ServerStatus)
			{
			case ServerStatusForClient.Sent:
				return 5;
			case ServerStatusForClient.Delivered:
			case ServerStatusForClient.DirectDelivery:
				return 7;
			case ServerStatusForClient.SendError:
				return 1;
			case ServerStatusForClient.DeliveryError:
				return 2;
			case ServerStatusForClient.LinkToOrder:
				return 10;
			default:
				if (IsTemp)
				{
					return 3;
				}
				return 4;
			}
		}
	}

	public SanadType Type { get; set; }

	public string TypeString => Type.GetDisplay().GetTranslate();
}
