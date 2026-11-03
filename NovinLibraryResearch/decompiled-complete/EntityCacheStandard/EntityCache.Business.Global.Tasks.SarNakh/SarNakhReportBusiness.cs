using System;
using EntityCache.Utilities;
using PacketParser;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global.Tasks;

namespace EntityCache.Business.Global.Tasks.SarNakh;

public class SarNakhReportBusiness : ISarNakh, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public DateTime CreationDate { get; set; }

	public string CreationDateSh => CreationDate.MiladiToCalture();

	public string Company { get; set; }

	public string FullName { get; set; }

	public Guid CashierGuid { get; set; }

	public string CashierName { get; set; }

	public string Description { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public short StatusId { get; set; }

	public string StatusName { get; set; }

	public Guid? IntroductionGuid { get; set; }

	public string IntroName { get; set; }

	public bool IsChangedToCustomer { get; set; }

	public byte[] LinkImage
	{
		get
		{
			if (IsChangedToCustomer)
			{
				return ImageResourceManager.Link;
			}
			return ImageResourceManager.UnLink;
		}
	}
}
