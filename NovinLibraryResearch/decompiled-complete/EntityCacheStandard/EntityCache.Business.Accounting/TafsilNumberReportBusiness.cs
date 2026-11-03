using EntityCache.Utilities;

namespace EntityCache.Business.Accounting;

public class TafsilNumberReportBusiness : TafsilNumberBusiness
{
	public string Name { get; set; }

	public byte[] CallIcon => ImageResourceManager.Telephone;

	public TafsilNumberReportBusiness(string contextGuid)
		: base(contextGuid)
	{
	}
}
