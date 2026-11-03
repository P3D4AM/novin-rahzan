using System;
using EntityCache.Utilities;
using PacketParser.Entities;

namespace EntityCache.Models.Accounting;

public class CustomerRoleModel
{
	public Guid Guid { get; set; }

	public string Name { get; set; }

	public string WordPressName { get; set; }

	public string PriceLevelName { get; set; }

	public string Description { get; set; }

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
}
