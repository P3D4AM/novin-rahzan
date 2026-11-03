using System;
using System.Threading;
using System.Threading.Tasks;

namespace EntityCache.Business.Accounting.ProductAttrebutes;

public class ProductAttributeValueReportBusiness
{
	public Guid ValueGuid { get; set; }

	public Guid MasterGuid { get; set; }

	public string Value { get; set; }

	public string AttributeName { get; set; }

	public bool IsChecked { get; set; }

	public string NameForDisplay => AttributeName + " - " + Value;

	public string ContextGuid { get; set; }

	public ProductAttributeValueReportBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public Task<ProductAttributeBusiness> GetMasterAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ProductAttributeBusiness.GetAsync(ContextGuid, MasterGuid, token, tranName);
	}
}
