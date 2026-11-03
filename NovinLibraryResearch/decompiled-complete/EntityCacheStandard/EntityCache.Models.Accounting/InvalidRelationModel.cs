using System.Collections.Generic;
using EntityCache.Business.Accounting.Sanad;
using PacketParser.Entities;

namespace EntityCache.Models.Accounting;

public class InvalidRelationModel
{
	public DocumentDet AsType { get; set; }

	public int Count { get; set; }

	public List<ViewSanadDetailCoverBusiness> SanadList { get; set; }
}
