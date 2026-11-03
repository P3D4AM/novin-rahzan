using System;
using System.Linq;
using EntityCache.Utilities;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Settings.Accounting;

namespace EntityCache.Business.Products.ProductUnit;

public class ProductVahedModel : IPrdVahed, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public string Description { get; set; }

	public string Name { get; set; }

	public string TaxName { get; private set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public bool DecimalSales { get; set; }

	public Guid? TaxUnitGuid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

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

	public ProductVahedModel(TaxUnits setting, Guid? guid)
	{
		LoadTaxName(setting, guid);
	}

	private void LoadTaxName(TaxUnits setting, Guid? guid)
	{
		try
		{
			if (!((guid ?? Guid.Empty) == Guid.Empty) && (setting?.UnitList?.Any()).GetValueOrDefault())
			{
				WebTaxUnit webTaxUnit = setting.UnitList.SingleOrDefault((WebTaxUnit q) => q.Guid == guid.Value);
				if (webTaxUnit != null)
				{
					TaxName = webTaxUnit.Name;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadTaxName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\ProductVahedModel.cs", 49);
		}
	}
}
