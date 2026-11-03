using System;
using EntityCache.Utilities;
using PacketParser;
using PacketParser.Entities;

namespace EntityCache.Business.Products.ProductSerial;

public class PrdSerialReportBusiness
{
	public Guid Guid { get; set; }

	public bool Status { get; set; }

	public Guid ProductGuid { get; set; }

	public string Serial { get; set; }

	public string GarrantyCode { get; set; }

	public DateTime? StartDate { get; set; }

	public DateTime? EndDate { get; set; }

	public string ProductName { get; set; }

	public long ProductCode { get; set; }

	public string StartDateSh => StartDate.MiladiToCalture();

	public string EndDateSh => EndDate.MiladiToCalture();

	public bool IsChecked { get; set; }

	public bool IsMojod { get; set; }

	public string StatusForDisplay
	{
		get
		{
			if (!IsMojod)
			{
				return "ناموجود";
			}
			return "موجود";
		}
	}

	public ServerStatusForClient ServerStatus { get; set; }

	public string TechnicalInfo1 { get; set; }

	public string TechnicalInfo2 { get; set; }

	public string TechnicalInfo3 { get; set; }

	public byte[] TechnicalInfoImage => ImageResourceManager.Information;
}
