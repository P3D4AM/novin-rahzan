using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Business.Products;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Filters;

namespace EntityCache.DescriptionParser;

public static class ProductDescParser
{
	private static string Code => "<" + "کد کالا".GetTranslate() + ">";

	private static string Description => "<" + "شرح".GetTranslate() + ">";

	private static string Name => "<" + "نام کالا".GetTranslate() + ">";

	private static string Technical1 => "<" + "مشخصه فنی 1".GetTranslate() + ">";

	private static string Technical2 => "<" + "مشخصه فنی 2".GetTranslate() + ">";

	private static string Technical3 => "<" + "مشخصه فنی 3".GetTranslate() + ">";

	private static string Technical4 => "<" + "مشخصه فنی 4".GetTranslate() + ">";

	private static string Technical5 => "<" + "مشخصه فنی 5".GetTranslate() + ">";

	private static string Technical6 => "<" + "مشخصه فنی 6".GetTranslate() + ">";

	private static string Technical7 => "<" + "مشخصه فنی 7".GetTranslate() + ">";

	private static string Technical8 => "<" + "مشخصه فنی 8".GetTranslate() + ">";

	private static string V2Qt => "<" + "تعداد در واحد دوم".GetTranslate() + ">";

	private static string V3Qt => "<" + "تعداد در واحد سوم".GetTranslate() + ">";

	private static string V4Qt => "<" + "تعداد در واحد چهارم".GetTranslate() + ">";

	private static string GroupName => "<" + "نام گروه".GetTranslate() + ">";

	private static string VahedName => "<" + "نام واحد اصلی".GetTranslate() + ">";

	private static string Sku => "<" + "Sku".GetTranslate() + ">";

	private static string Slug => "<" + "Slug".GetTranslate() + ">";

	public static async Task<List<string>> GetProductDescListAsync(string contextGuid, EnSystem system, CancellationToken token)
	{
		List<string> list = new List<string>();
		try
		{
			foreach (string item in (from x in (await ProductPriceLevelBusiness.GetAllAsync(contextGuid, system, new ProductPriceLevelFilter(), token)).ToList()
				select x.Name).ToList())
			{
				list.Add(item.Replace(item, "<" + item + ">"));
			}
			list.AddRange(new string[18]
			{
				Code, Description, Name, Technical1, Technical2, Technical3, Technical4, Technical5, Technical6, Technical7,
				Technical8, V2Qt, V3Qt, V4Qt, GroupName, VahedName, Sku, Slug
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetProductDescListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\DescriptionParser\\ProductDescParser.cs", 50);
		}
		return list;
	}

	public static async Task<string> GetStringAsync(this ProductBusiness p, string template, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		string desc = template;
		try
		{
			desc = desc.Replace(Code, p.IdProduct);
			desc = desc.Replace(Description, p.Description);
			desc = desc.Replace(Name, p.Name);
			ProductPriceLevelFilter filter = new ProductPriceLevelFilter
			{
				Status = true
			};
			IEnumerable<ProductPriceLevelBusiness> enumerable = await ProductPriceLevelBusiness.GetAllAsync(p.ContextGuid, system, filter, token, tranName);
			if (enumerable != null && enumerable.Any())
			{
				foreach (ProductPriceLevelBusiness priceLevel in enumerable)
				{
					if (priceLevel != null && desc.Contains("<" + priceLevel.Name + ">"))
					{
						desc = desc.Replace(newValue: ((p.PriceList?.SingleOrDefault((ProductPriceRelationBusiness q) => q.PriceLevelGuid == priceLevel.Guid))?.Price ?? 0m).ToString().ParseToDecimal(0m, 3).NpToString("N0", 0), oldValue: "<" + priceLevel.Name + ">");
					}
				}
			}
			IReadOnlyList<PrdTechnicalBusiness> obj = await p.GetPrdTechnicalListAsync(system, token, tranName);
			string newValue2 = obj?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 1)?.Name ?? "";
			string newValue3 = obj?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 2)?.Name ?? "";
			string newValue4 = obj?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 3)?.Name ?? "";
			string newValue5 = obj?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 4)?.Name ?? "";
			string newValue6 = obj?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 5)?.Name ?? "";
			string newValue7 = obj?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 6)?.Name ?? "";
			string newValue8 = obj?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 7)?.Name ?? "";
			string newValue9 = obj?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 8)?.Name ?? "";
			desc = desc.Replace(Technical1, newValue2);
			desc = desc.Replace(Technical2, newValue3);
			desc = desc.Replace(Technical3, newValue4);
			desc = desc.Replace(Technical4, newValue5);
			desc = desc.Replace(Technical5, newValue6);
			desc = desc.Replace(Technical6, newValue7);
			desc = desc.Replace(Technical7, newValue8);
			desc = desc.Replace(Technical8, newValue9);
			desc = desc.Replace(V2Qt, p.V2Qt.ToString().ParseToDecimal(0m, 3).ToString());
			desc = desc.Replace(V3Qt, p.V3Qt.ToString().ParseToDecimal(0m, 3).ToString());
			desc = desc.Replace(V4Qt, p.V4Qt.ToString().ParseToDecimal(0m, 3).ToString());
			if (!desc.Contains(GroupName))
			{
				string text = desc;
				string groupName = GroupName;
				desc = text.Replace(groupName, await p.GetGroupNameAsync(token));
			}
			if (!desc.Contains(VahedName))
			{
				string groupName = desc;
				string text = VahedName;
				desc = groupName.Replace(text, (await p.GetVahedAsync(token)).Name);
			}
			desc = desc.Replace(Sku, p.Sku);
			desc = desc.Replace(Slug, p.Slug);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetStringAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\DescriptionParser\\ProductDescParser.cs", 122);
		}
		return desc;
	}
}
