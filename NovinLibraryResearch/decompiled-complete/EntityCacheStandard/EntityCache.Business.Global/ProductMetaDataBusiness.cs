using System;
using EntityCache.Business.Accounting;
using PacketParser.Entities;

namespace EntityCache.Business.Global;

public class ProductMetaDataBusiness : MetaDataBusiness
{
	public ProductMetaKey MetaKey
	{
		get
		{
			try
			{
				if (short.TryParse(base.Key, out var result))
				{
					return (ProductMetaKey)result;
				}
				string key = base.Key;
				if (key == "شناسه مالیاتی کالا")
				{
					return ProductMetaKey.TaxCode;
				}
				if (!(key == ((short)1).ToString()))
				{
					if (!(key == "AttributeGuidList"))
					{
						if (key == "SeoModel")
						{
							return ProductMetaKey.SeoModel;
						}
						if (key == ((short)4).ToString())
						{
							return ProductMetaKey.AvailableForSell;
						}
						if (key == ((short)5).ToString())
						{
							return ProductMetaKey.AvailableForBuy;
						}
						if (key == ((short)7).ToString())
						{
							return ProductMetaKey.AvailableForReturn;
						}
						if (key == ((short)6).ToString())
						{
							return ProductMetaKey.AvailableForSellToOnlineShop;
						}
						return ProductMetaKey.None;
					}
					return ProductMetaKey.AttributeGuidList;
				}
				return ProductMetaKey.TaxName;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MetaKey", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\ProductMetaDataBusiness.cs", 31);
				throw;
			}
		}
	}

	public ProductMetaDataBusiness(string contextGuid, MetaDataBusiness master)
		: base(contextGuid)
	{
		try
		{
			base.EntityGuid = master.EntityGuid;
			base.EntityType = master.EntityType;
			base.Key = master.Key;
			base.Value = master.Value;
			base.DataType = master.DataType;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\ProductMetaDataBusiness.cs", 47);
		}
	}

	public ProductMetaDataBusiness(string contextGuid)
		: base(contextGuid)
	{
	}
}
