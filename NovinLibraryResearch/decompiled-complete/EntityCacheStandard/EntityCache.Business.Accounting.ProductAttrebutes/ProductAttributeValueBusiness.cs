using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.ProductAttrebutes;

namespace EntityCache.Business.Accounting.ProductAttrebutes;

public class ProductAttributeValueBusiness : Serializable<ProductAttributeValueBusiness>, IProductAttributeValue, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, IHasContext
{
	private bool _isDisposed;

	public Guid Guid { get; set; }

	public string Name { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Guid MasterGuid { get; set; }

	public string Slug { get; set; }

	public string ContextGuid { get; set; }

	public event Func<object, EventArgs, Task> OnDisposed;

	public ProductAttributeValueBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<IEnumerable<ProductAttributeValueReportBusiness>> GetAllAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ProductAttributeRepository.GetAllValueAsync(token, tranName);
	}

	public static ProductAttributeValueBusiness GenerateNew(string contextGuid, string name = "", Guid? masterGuid = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			ProductAttributeValueBusiness productAttributeValueBusiness = new ProductAttributeValueBusiness(contextGuid)
			{
				ContextGuid = contextGuid,
				Name = name,
				Guid = Guid.NewGuid(),
				Modified = DateTime.Now,
				Slug = name.WordPressFixSlugOrSku(),
				Status = true
			};
			if ((masterGuid ?? Guid.Empty) != Guid.Empty)
			{
				productAttributeValueBusiness.MasterGuid = masterGuid.Value;
			}
			return productAttributeValueBusiness;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNew", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeValueBusiness.cs", 51);
			throw;
		}
	}

	public override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				if (this.OnDisposed != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnDisposed, this, new EventArgs(), default(CancellationToken), 60, "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeValueBusiness.cs");
				}
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ProductAttrebutes\\ProductAttributeValueBusiness.cs", 67);
		}
	}
}
