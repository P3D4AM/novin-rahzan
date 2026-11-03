using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Selection;
using EntityCache.Business.Customers;
using EntityCache.Business.Products;
using EntityCache.Mapping.BusinessMappers.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting.Product;
using EntityCache.Mapping.BusinessMappers.Accounting.ProductAttrebutes;
using EntityCache.Models.Accounting.Prd;
using PacketParser;
using PacketParser.BusinessLayer.Models;
using PacketParser.BusinessLayer.Models.Product;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Product;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class ProductMapper : IItemAsyncMapper<ProductBusiness, WebProduct>, IItemAsyncMapper<ProductBusiness, ProductBusiness>, IItemAsyncMapper<WebProduct, ProductBusiness>, IItemAsyncMapper<ProductBusiness, ProductCompactBusiness>, IItemAsyncMapper<ProductCompactBusiness, ProductCompactBusiness>, IItemAsyncMapper<ProductCompactBusiness, WebProductCompact>, IItemAsyncMapper<WebProductCompact, ProductCompactBusiness>
{
	public static ProductMapper Instance { get; private set; } = new ProductMapper();


	public async Task<WebProduct> MapAsync(string contextGuid, ProductBusiness source, WebProduct destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		Stopwatch sw = Stopwatch.StartNew();
		using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\ProductMapper.cs"))
		{
			_ = 23;
			try
			{
				if (source == null)
				{
					destination = null;
					return destination;
				}
				if (destination == null)
				{
					destination = new WebProduct();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Special = source.Special;
				destination.Status = source.Status;
				destination.Code = source.Code;
				destination.Description = source.Description;
				destination.DiscountPercent = source.DiscountPercent;
				destination.IdProduct = source.IdProduct;
				destination.Name = source.Name;
				destination.V2Guid = source.V2Guid;
				destination.V3Guid = source.V3Guid;
				destination.V4Guid = source.V4Guid;
				destination.V2Qt = source.V2Qt;
				destination.Weight = source.Weight;
				destination.GuidVahed = source.GuidVahed;
				destination.Amount = source.Amount;
				destination.AutoSellSt = source.AutoSellSt;
				destination.BPercent1 = source.BPercent1;
				destination.BPercent2 = source.BPercent2;
				destination.BPercent3 = source.BPercent3;
				destination.BPercent4 = source.BPercent4;
				destination.CreateDate = source.CreateDate;
				destination.FullContent = source.FullContent;
				destination.Height = source.Height;
				destination.IsTax = source.IsTax;
				destination.IsSerial = source.IsSerial;
				destination.KardexPrice = source.KardexPrice;
				destination.Keywords = source.Keywords;
				destination.MetaDesc = source.MetaDesc;
				destination.Mojodi = source.Mojodi;
				destination.OtherPics = source.OtherPics;
				destination.QuickText = source.QuickText;
				destination.ScaleSt = source.ScaleSt;
				destination.Score = source.Score;
				IReadOnlyList<PrdTechnicalBusiness> readOnlyList = await source.GetPrdTechnicalListAsync(EnSystem.Mapper, token, tranName);
				destination.T1 = readOnlyList?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 1)?.Name ?? "";
				destination.T2 = readOnlyList?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 2)?.Name ?? "";
				destination.T3 = readOnlyList?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 3)?.Name ?? "";
				destination.T4 = readOnlyList?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 4)?.Name ?? "";
				destination.T5 = readOnlyList?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 5)?.Name ?? "";
				destination.T6 = readOnlyList?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 6)?.Name ?? "";
				destination.T7 = readOnlyList?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 7)?.Name ?? "";
				destination.T8 = readOnlyList?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 8)?.Name ?? "";
				destination.V3Qt = source.V3Qt;
				destination.V4Qt = source.V4Qt;
				destination.Width = source.Width;
				destination.OrderPoint = source.OrderPoint;
				if (source.TagList?.Any() ?? false)
				{
					destination.Tags = string.Join(",", source.TagList?.Select((ProductTagsRecord q) => q.Tag)?.Where((string q) => !string.IsNullOrEmpty(q))?.ToList());
				}
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.ImageListData = source.ImageListData;
				destination.Type = source.Type;
				destination.Sku = source.Sku ?? "";
				destination.Slug = source.Slug ?? "";
				destination.MasterGuid = source.MasterGuid;
				WebProduct webProduct = destination;
				webProduct.VahedName = (await source.GetVahedAsync(token, tranName))?.Name ?? "";
				List<BarcodeBusiness> source2 = await BarcodeBusiness.GetByPrdGuidAsync(contextGuid, new List<Guid> { source.Guid }, default(CancellationToken), tranName);
				webProduct = destination;
				webProduct.PrdBarcode = await BarcodeMapper.Instance.MapListAsync(contextGuid, source2, new List<WebBarcode>());
				List<PrdAnbarRelationBusiness> source3 = await source.GetAnbarRelationsAsync(EnSystem.Mapper, token, tranName);
				webProduct = destination;
				webProduct.PrdAnbarRelation = await PrdAnbarRelationMapper.Instance.MapListAsync(contextGuid, source3, new List<WebPrdAnbarRelation>());
				destination.TolidFormulaGuid = source.TolidFormulaGuid;
				destination.Version = source.Version;
				destination.SendToServerDate = source.SendToServerDate;
				destination.Length = source.Length;
				destination.ClientVersion = 7;
				webProduct = destination;
				webProduct.TaxCode = await source.GetTaxCodeAsync(token, tranName);
				webProduct = destination;
				webProduct.TaxName = await source.GetTaxNameAsync(token, tranName);
				destination.PayementType = source.PayementType;
				destination.IndexingType = source.IndexingType;
				destination.Kind = source.Kind;
				destination.TaxGuid = source.TaxGuid;
				destination.DiscountStartDate = source.DiscountStartDate;
				destination.DiscountEndDate = source.DiscountEndDate;
				destination.DiscountPrice = source.DiscountPrice;
				destination.IsDependentOnCurrency = source.IsDependentOnCurrency;
				if (source.AttributeRelations?.Any() ?? false)
				{
					destination.AttributeRelations = await ProductAttributeRelationMapper.Instance.MapListAsync(contextGuid, source.AttributeRelations, new List<WebProductAttributeRelation>(), tranName, token);
				}
				List<Guid> list = await source.GetAttributeGuidListAsync(token, tranName);
				if (list != null && list.Any())
				{
					if (list.Count != list.Distinct().Count())
					{
						throw new InvalidOperationException("ویژگی تکراری در لیست وجود دارد".GetTranslate() + " \r\n " + Json.ToStringJson(list));
					}
					destination.RelatedAttributes = new List<WebProductAttribute>();
					foreach (Guid item3 in list)
					{
						ProductAttributeBusiness source4 = await ProductAttributeBusiness.GetAsync(contextGuid, item3, token, tranName);
						WebProductAttribute item2 = await ProductAttributeMapper.Instance.MapAsync(contextGuid, source4, new WebProductAttribute(), tranName, token);
						destination.RelatedAttributes.Add(item2);
					}
				}
				if ((source.MasterGuid ?? Guid.Empty) != Guid.Empty)
				{
					IEnumerable<ProductAttributeRelationBusiness> enumerable = await ClsCache.GetCache(contextGuid).CacheRepository.ProductAttributeRelationRepository.GetAllAsync(source.Guid, token, tranName);
					if (enumerable != null && enumerable.Any())
					{
						destination.AttributeRelations = await ProductAttributeRelationMapper.Instance.MapListAsync(contextGuid, enumerable, new List<WebProductAttributeRelation>(), tranName, token);
					}
				}
				IReadOnlyList<PrdTechnicalBusiness> readOnlyList2 = await source.GetPrdTechnicalListAsync(EnSystem.Mapper, token, tranName);
				if (readOnlyList2 != null && readOnlyList2.Any())
				{
					webProduct = destination;
					webProduct.PrdTechnicalList = await PrdTechnicalMapper.Instance.MapListAsync(contextGuid, readOnlyList2, new List<WebPrdTechnical>(), tranName, token);
				}
				if (source.ProductAttributes?.Any() ?? false)
				{
					destination.ProductAttributeGuids = source.ProductAttributes.Select((ProductBusiness q) => q.Guid)?.ToList();
				}
				IEnumerable<CustomerRoleBusiness> enumerable2 = await CustomerRoleBusiness.GetAllAsync(contextGuid, true, token, tranName);
				if (enumerable2 != null && enumerable2.Any())
				{
					List<IGrouping<string, CustomerRoleBusiness>> list2 = (from q in enumerable2
						group q by q.WordPressName).ToList();
					if (list2 != null && list2.Any())
					{
						WebProduct webProduct2 = destination;
						if (webProduct2.PriceRoleList == null)
						{
							webProduct2.PriceRoleList = new List<ProductPrice>();
						}
						foreach (IGrouping<string, CustomerRoleBusiness> item in list2)
						{
							if (item != null)
							{
								Guid? guid = item.Select((CustomerRoleBusiness q) => q).FirstOrDefault((CustomerRoleBusiness q) => (q.PriceTypeGuid ?? Guid.Empty) != Guid.Empty)?.PriceTypeGuid;
								if ((guid ?? Guid.Empty) != Guid.Empty)
								{
									decimal price = await source.GetSellAsync(EnSystem.Mapper, guid.Value, isLoadOriginalPrice: true, token, tranName);
									destination.PriceRoleList.Add(new ProductPrice
									{
										WordPressRoleName = item.Key,
										Price = price
									});
								}
							}
						}
					}
				}
				if ((ClsCache.GetCache(contextGuid)?.Source ?? ENSource.Accounting) != ENSource.Accounting)
				{
					PrdVahedBusiness prdVahedBusiness = await source.GetVahedAsync(token, tranName);
					if (prdVahedBusiness != null)
					{
						webProduct = destination;
						webProduct.Unit = await PrdVahedMapper.Instance.MapAsync(contextGuid, prdVahedBusiness, new WebPrdVahed(), tranName, token);
					}
				}
				IEnumerable<ProductPriceRelationModel> priceList = await source.GetAllPricesAsync(EnSystem.Mapper, null, token, tranName);
				if (priceList?.Any() ?? false)
				{
					webProduct = destination;
					webProduct.Prices = await ProductPriceRelationMapper.Instance.MapListAsync(contextGuid, source.PriceList, new List<WebProductPriceRelation>(), tranName, token);
					ProductPriceRelationModel productPriceRelationModel = priceList.SingleOrDefault((ProductPriceRelationModel q) => q.Ordinal == 1 && q.Type == EnPriceLevelType.Sell);
					ProductPriceRelationModel productPriceRelationModel2 = priceList.SingleOrDefault((ProductPriceRelationModel q) => q.Ordinal == 2 && q.Type == EnPriceLevelType.Sell);
					ProductPriceRelationModel productPriceRelationModel3 = priceList.SingleOrDefault((ProductPriceRelationModel q) => q.Ordinal == 3 && q.Type == EnPriceLevelType.Sell);
					ProductPriceRelationModel productPriceRelationModel4 = priceList.SingleOrDefault((ProductPriceRelationModel q) => q.Ordinal == 4 && q.Type == EnPriceLevelType.Sell);
					ProductPriceRelationModel productPriceRelationModel5 = priceList.SingleOrDefault((ProductPriceRelationModel q) => q.Ordinal == 5 && q.Type == EnPriceLevelType.Sell);
					ProductPriceRelationModel productPriceRelationModel6 = priceList.SingleOrDefault((ProductPriceRelationModel q) => q.Ordinal == 6 && q.Type == EnPriceLevelType.Sell);
					ProductPriceRelationModel productPriceRelationModel7 = priceList.SingleOrDefault((ProductPriceRelationModel q) => q.Ordinal == 7 && q.Type == EnPriceLevelType.Sell);
					ProductPriceRelationModel productPriceRelationModel8 = priceList.SingleOrDefault((ProductPriceRelationModel q) => q.Ordinal == 8 && q.Type == EnPriceLevelType.Sell);
					priceList.SingleOrDefault((ProductPriceRelationModel q) => q.Ordinal == 9 && q.Type == EnPriceLevelType.Sell);
					ProductPriceRelationModel productPriceRelationModel9 = priceList.SingleOrDefault((ProductPriceRelationModel q) => q.Ordinal == 10 && q.Type == EnPriceLevelType.Buy);
					ProductPriceRelationModel productPriceRelationModel10 = priceList.SingleOrDefault((ProductPriceRelationModel q) => q.Ordinal == 11 && q.Type == EnPriceLevelType.Buy);
					ProductPriceRelationModel productPriceRelationModel11 = priceList.SingleOrDefault((ProductPriceRelationModel q) => q.Ordinal == 12 && q.Type == EnPriceLevelType.Buy);
					ProductPriceRelationModel productPriceRelationModel12 = priceList.SingleOrDefault((ProductPriceRelationModel q) => q.Ordinal == 13 && q.Type == EnPriceLevelType.Buy);
					destination.Sell1 = productPriceRelationModel?.Price ?? 0m;
					destination.Sell2 = productPriceRelationModel2?.Price ?? 0m;
					destination.Sell3 = productPriceRelationModel3?.Price ?? 0m;
					destination.Sell4 = productPriceRelationModel4?.Price ?? 0m;
					destination.Sell5 = productPriceRelationModel5?.Price ?? 0m;
					destination.Sell6 = productPriceRelationModel6?.Price ?? 0m;
					destination.Sell7 = productPriceRelationModel7?.Price ?? 0m;
					destination.Sell8 = productPriceRelationModel8?.Price ?? 0m;
					destination.Sell1Qt = productPriceRelationModel?.Quantity ?? 0m;
					destination.Sell2Qt = productPriceRelationModel2?.Quantity ?? 0m;
					destination.Sell3Qt = productPriceRelationModel3?.Quantity ?? 0m;
					destination.Sell4Qt = productPriceRelationModel4?.Quantity ?? 0m;
					destination.Sell5Qt = productPriceRelationModel5?.Quantity ?? 0m;
					destination.Sell6Qt = productPriceRelationModel6?.Quantity ?? 0m;
					destination.Sell7Qt = productPriceRelationModel7?.Quantity ?? 0m;
					destination.Sell8Qt = productPriceRelationModel8?.Quantity ?? 0m;
					destination.Buy1 = productPriceRelationModel9?.Price ?? 0m;
					destination.Buy2 = productPriceRelationModel10?.Price ?? 0m;
					destination.Buy3 = productPriceRelationModel11?.Price ?? 0m;
					destination.BuyLast = productPriceRelationModel12?.Price ?? 0m;
					destination.SellKasr1 = productPriceRelationModel?.KasrRial ?? 0m;
					destination.SellKasr2 = productPriceRelationModel2?.KasrRial ?? 0m;
					destination.SellKasr3 = productPriceRelationModel3?.KasrRial ?? 0m;
					destination.SellKasr4 = productPriceRelationModel4?.KasrRial ?? 0m;
					destination.SellKasr5 = productPriceRelationModel5?.KasrRial ?? 0m;
					destination.SellKasr6 = productPriceRelationModel6?.KasrRial ?? 0m;
					destination.SellKasr7 = productPriceRelationModel7?.KasrRial ?? 0m;
					destination.SellKasr8 = productPriceRelationModel8?.KasrRial ?? 0m;
					destination.SellSood1 = productPriceRelationModel?.Profit ?? 0m;
					destination.SellSood2 = productPriceRelationModel2?.Profit ?? 0m;
					destination.SellSood3 = productPriceRelationModel3?.Profit ?? 0m;
					destination.SellSood4 = productPriceRelationModel4?.Profit ?? 0m;
					destination.SellSood5 = productPriceRelationModel5?.Profit ?? 0m;
					destination.SellSood6 = productPriceRelationModel6?.Profit ?? 0m;
					destination.SellSood7 = productPriceRelationModel7?.Profit ?? 0m;
					destination.SellSood8 = productPriceRelationModel8?.Profit ?? 0m;
				}
				if (source.GroupList?.Any() ?? false)
				{
					webProduct = destination;
					webProduct.GroupList = await ProductGroupRelationMapper.Instance.MapListAsync(contextGuid, source.GroupList, new List<WebProductGroupRelation>(), tranName, token);
					destination.GuidGroup = source.GroupList.OrderBy((ProductGroupRelationBusiness q) => q.Ordinal)?.FirstOrDefault()?.GroupGuid ?? Guid.Empty;
				}
				destination.MappId = source.MappId;
				destination.NameForDisplay = source.NameForDisplay;
				destination.DefaultVariable = source.DefaultVariable;
				ProductSeoModel productSeoModel = await source.GetSeoModelAsync(token, tranName);
				destination.SeoTitle = productSeoModel?.SeoTitle ?? "";
				destination.SeoDescription = productSeoModel?.SeoDescription ?? "";
				destination.SeoFocusKeyword = productSeoModel?.SeoMainKeyWord ?? "";
				return destination;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\ProductMapper.cs", 282);
				throw;
			}
			finally
			{
				Console.WriteLine($"Mapping time: {sw.ElapsedMilliseconds}(ms)");
			}
		}
	}

	public Task<List<WebProduct>> MapListAsync(string contextGuid, IEnumerable<ProductBusiness> source, List<WebProduct> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20) ?? Task.FromResult<List<WebProduct>>(null);
	}

	public async Task<ProductBusiness> MapAsync(string contextGuid, ProductBusiness source, ProductBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 6;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new ProductBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Type = source.Type;
			destination.Sku = source.Sku;
			destination.Slug = source.Slug;
			destination.Modified = source.Modified;
			destination.Special = source.Special;
			destination.Status = source.Status;
			destination.Code = source.Code;
			destination.Description = source.Description;
			destination.DiscountPercent = source.DiscountPercent;
			destination.IdProduct = source.IdProduct;
			destination.Name = source.Name;
			destination.V2Guid = source.V2Guid;
			destination.V3Guid = source.V3Guid;
			destination.V4Guid = source.V4Guid;
			destination.V2Qt = source.V2Qt;
			destination.Weight = source.Weight;
			destination.GuidVahed = source.GuidVahed;
			destination.Amount = source.Amount;
			destination.AutoSellSt = source.AutoSellSt;
			destination.BPercent1 = source.BPercent1;
			destination.BPercent2 = source.BPercent2;
			destination.BPercent3 = source.BPercent3;
			destination.BPercent4 = source.BPercent4;
			destination.CreateDate = source.CreateDate;
			destination.FullContent = source.FullContent;
			destination.Height = source.Height;
			destination.IsTax = source.IsTax;
			destination.IsSerial = source.IsSerial;
			destination.KardexPrice = source.KardexPrice;
			destination.Keywords = source.Keywords;
			destination.MetaDesc = source.MetaDesc;
			destination.Mojodi = source.Mojodi;
			destination.OtherPics = source.OtherPics;
			destination.QuickText = source.QuickText;
			destination.ScaleSt = source.ScaleSt;
			destination.Score = source.Score;
			destination.V3Qt = source.V3Qt;
			destination.V4Qt = source.V4Qt;
			destination.Width = source.Width;
			destination.OrderPoint = source.OrderPoint;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.ImageListData = source.ImageListData;
			destination.MasterGuid = source.MasterGuid;
			destination.Version = source.Version;
			destination.SendToServerDate = source.SendToServerDate;
			destination.TolidFormulaGuid = source.TolidFormulaGuid;
			destination.Length = source.Length;
			destination.PayementType = source.PayementType;
			destination.IndexingType = source.IndexingType;
			destination.Kind = source.Kind;
			destination.TaxGuid = source.TaxGuid;
			destination.DiscountStartDate = source.DiscountStartDate;
			destination.DiscountEndDate = source.DiscountEndDate;
			destination.DiscountPrice = source.DiscountPrice;
			destination.IsDependentOnCurrency = source.IsDependentOnCurrency;
			string taxCode = await source.GetTaxCodeAsync(token, tranName);
			string taxName = await source.GetTaxNameAsync(token, tranName);
			ProductSeoModel seoModel = await source.GetSeoModelAsync(token, tranName);
			List<Guid> list = await source.GetAttributeGuidListAsync(token, tranName);
			destination.AddMetaData(ProductMetaKey.TaxCode, taxCode);
			destination.AddMetaData(ProductMetaKey.TaxName, taxName);
			if (list != null && list.Any())
			{
				destination.AddMetaData(ProductMetaKey.AttributeGuidList, Json.ToStringJson(list.Distinct().ToList()));
			}
			if (seoModel != null)
			{
				destination.AddMetaData(ProductMetaKey.SeoModel, Json.ToStringJson(seoModel));
			}
			if (source.TagList?.Any() ?? false)
			{
				destination.TagList = new List<ProductTagsRecord>();
				foreach (ProductTagsRecord tag in source.TagList)
				{
					destination.TagList.Add(new ProductTagsRecord
					{
						Guid = tag.Guid,
						Tag = tag.Tag
					});
				}
			}
			if (source.PriceList?.Any() ?? false)
			{
				await destination.AddRangeToPriceListAsync(await ProductPriceRelationMapper.Instance.MapListAsync(contextGuid, source.PriceList, new List<ProductPriceRelationBusiness>(), tranName, token), token);
			}
			if (source.GroupList?.Any() ?? false)
			{
				destination.AddRangeToGroupRelationList(await ProductGroupRelationMapper.Instance.MapListAsync(contextGuid, source.GroupList, new List<ProductGroupRelationBusiness>(), tranName, token), emptyList: true);
			}
			destination.MappId = source.MappId;
			destination.NameForDisplay = source.NameForDisplay;
			destination.DefaultVariable = source.DefaultVariable;
			return destination;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\ProductMapper.cs", 404);
			throw;
		}
	}

	public async Task<ProductBusiness> MapAsync(string contextGuid, WebProduct source, ProductBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 4;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new ProductBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Type = source.Type;
			destination.Sku = source.Sku;
			destination.Slug = source.Slug;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Special = source.Special;
			destination.Status = source.Status;
			destination.Code = source.Code;
			destination.Description = source.Description ?? "";
			destination.DiscountPercent = source.DiscountPercent;
			destination.IdProduct = source.IdProduct ?? "";
			destination.Name = source.Name ?? "";
			destination.V2Guid = source.V2Guid;
			destination.V3Guid = source.V3Guid;
			destination.V4Guid = source.V4Guid;
			destination.V2Qt = source.V2Qt;
			destination.Weight = source.Weight;
			destination.GuidVahed = source.GuidVahed;
			destination.Amount = source.Amount;
			destination.AutoSellSt = source.AutoSellSt;
			destination.BPercent1 = source.BPercent1;
			destination.BPercent2 = source.BPercent2;
			destination.BPercent3 = source.BPercent3;
			destination.BPercent4 = source.BPercent4;
			destination.CreateDate = source.CreateDate;
			destination.FullContent = source.FullContent ?? "";
			destination.Height = source.Height;
			destination.IsTax = source.IsTax;
			destination.IsSerial = source.IsSerial;
			destination.KardexPrice = source.KardexPrice;
			destination.Keywords = source.Keywords;
			destination.MetaDesc = source.MetaDesc;
			destination.Mojodi = source.Mojodi;
			destination.OtherPics = source.OtherPics;
			destination.QuickText = source.QuickText;
			destination.ScaleSt = source.ScaleSt;
			destination.Score = source.Score;
			destination.V3Qt = source.V3Qt;
			destination.V4Qt = source.V4Qt;
			destination.Width = source.Width;
			destination.OrderPoint = source.OrderPoint;
			if (!string.IsNullOrEmpty(source.Tags))
			{
				List<string> list = source.Tags.Split(new char[1] { ',' })?.ToList();
				if (list == null || !list.Any())
				{
					return null;
				}
				destination.TagList = list.Select((string q) => new ProductTagsRecord
				{
					Guid = Guid.NewGuid(),
					Tag = q.ToString()
				})?.ToList();
			}
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.ImageListData = source.ImageListData;
			destination.MasterGuid = source.MasterGuid;
			destination.TolidFormulaGuid = source.TolidFormulaGuid;
			destination.Version = source.Version;
			destination.SendToServerDate = source.SendToServerDate;
			destination.Length = source.Length;
			destination.PayementType = source.PayementType;
			destination.IndexingType = source.IndexingType;
			destination.Kind = source.Kind;
			destination.TaxGuid = source.TaxGuid;
			destination.DiscountStartDate = source.DiscountStartDate;
			destination.DiscountEndDate = source.DiscountEndDate;
			destination.DiscountPrice = source.DiscountPrice;
			destination.IsDependentOnCurrency = source.IsDependentOnCurrency;
			if (source.AttributeRelations?.Any() ?? false)
			{
				await destination.AddAttributeRelationListAsync(await ProductAttributeRelationMapper.Instance.MapListAsync(contextGuid, source.AttributeRelations, new List<ProductAttributeRelationBusiness>(), tranName, token), token);
			}
			if (source.Prices?.Any() ?? false)
			{
				await destination.AddRangeToPriceListAsync(await ProductPriceRelationMapper.Instance.MapListAsync(contextGuid, source.Prices, new List<ProductPriceRelationBusiness>(), tranName, token), token);
			}
			else
			{
				destination.AddToPriceList(new ProductPriceRelationBusiness(contextGuid)
				{
					Guid = Guid.NewGuid(),
					KasrRial = source.SellKasr1,
					Modified = DateTime.Now,
					Price = source.Sell1,
					PriceLevelGuid = ParentDefaults.PriceLevel.Sell1,
					ProductGuid = source.Guid,
					Profit = source.SellSood1,
					Quantity = source.Sell1Qt,
					ServerStatus = ServerStatusForClient.None,
					Type = EnPriceLevelType.Sell
				});
				destination.AddToPriceList(new ProductPriceRelationBusiness(contextGuid)
				{
					Guid = Guid.NewGuid(),
					KasrRial = source.SellKasr2,
					Modified = DateTime.Now,
					Price = source.Sell2,
					PriceLevelGuid = ParentDefaults.PriceLevel.Sell2,
					ProductGuid = source.Guid,
					Profit = source.SellSood2,
					Quantity = source.Sell2Qt,
					ServerStatus = ServerStatusForClient.None,
					Type = EnPriceLevelType.Sell
				});
				destination.AddToPriceList(new ProductPriceRelationBusiness(contextGuid)
				{
					Guid = Guid.NewGuid(),
					KasrRial = source.SellKasr3,
					Modified = DateTime.Now,
					Price = source.Sell3,
					PriceLevelGuid = ParentDefaults.PriceLevel.Sell3,
					ProductGuid = source.Guid,
					Profit = source.SellSood3,
					Quantity = source.Sell3Qt,
					ServerStatus = ServerStatusForClient.None,
					Type = EnPriceLevelType.Sell
				});
				destination.AddToPriceList(new ProductPriceRelationBusiness(contextGuid)
				{
					Guid = Guid.NewGuid(),
					KasrRial = source.SellKasr4,
					Modified = DateTime.Now,
					Price = source.Sell4,
					PriceLevelGuid = ParentDefaults.PriceLevel.Sell4,
					ProductGuid = source.Guid,
					Profit = source.SellSood4,
					Quantity = source.Sell4Qt,
					ServerStatus = ServerStatusForClient.None,
					Type = EnPriceLevelType.Sell
				});
				destination.AddToPriceList(new ProductPriceRelationBusiness(contextGuid)
				{
					Guid = Guid.NewGuid(),
					KasrRial = source.SellKasr5,
					Modified = DateTime.Now,
					Price = source.Sell5,
					PriceLevelGuid = ParentDefaults.PriceLevel.Sell5,
					ProductGuid = source.Guid,
					Profit = source.SellSood5,
					Quantity = source.Sell5Qt,
					ServerStatus = ServerStatusForClient.None,
					Type = EnPriceLevelType.Sell
				});
				destination.AddToPriceList(new ProductPriceRelationBusiness(contextGuid)
				{
					Guid = Guid.NewGuid(),
					KasrRial = source.SellKasr6,
					Modified = DateTime.Now,
					Price = source.Sell6,
					PriceLevelGuid = ParentDefaults.PriceLevel.Sell6,
					ProductGuid = source.Guid,
					Profit = source.SellSood6,
					Quantity = source.Sell6Qt,
					ServerStatus = ServerStatusForClient.None,
					Type = EnPriceLevelType.Sell
				});
				destination.AddToPriceList(new ProductPriceRelationBusiness(contextGuid)
				{
					Guid = Guid.NewGuid(),
					KasrRial = source.SellKasr7,
					Modified = DateTime.Now,
					Price = source.Sell7,
					PriceLevelGuid = ParentDefaults.PriceLevel.Sell7,
					ProductGuid = source.Guid,
					Profit = source.SellSood7,
					Quantity = source.Sell7Qt,
					ServerStatus = ServerStatusForClient.None,
					Type = EnPriceLevelType.Sell
				});
				destination.AddToPriceList(new ProductPriceRelationBusiness(contextGuid)
				{
					Guid = Guid.NewGuid(),
					KasrRial = source.SellKasr8,
					Modified = DateTime.Now,
					Price = source.Sell8,
					PriceLevelGuid = ParentDefaults.PriceLevel.Sell8,
					ProductGuid = source.Guid,
					Profit = source.SellSood8,
					Quantity = source.Sell8Qt,
					ServerStatus = ServerStatusForClient.None,
					Type = EnPriceLevelType.Sell
				});
				destination.AddToPriceList(new ProductPriceRelationBusiness(contextGuid)
				{
					Guid = Guid.NewGuid(),
					Modified = DateTime.Now,
					Price = source.RegularPrice,
					PriceLevelGuid = ParentDefaults.PriceLevel.RegularPrice,
					ProductGuid = source.Guid,
					ServerStatus = ServerStatusForClient.None,
					Type = EnPriceLevelType.Sell
				});
				destination.AddToPriceList(new ProductPriceRelationBusiness(contextGuid)
				{
					Guid = Guid.NewGuid(),
					Modified = DateTime.Now,
					Price = source.Buy1,
					PriceLevelGuid = ParentDefaults.PriceLevel.Buy1,
					ProductGuid = source.Guid,
					ServerStatus = ServerStatusForClient.None,
					Type = EnPriceLevelType.Buy
				});
				destination.AddToPriceList(new ProductPriceRelationBusiness(contextGuid)
				{
					Guid = Guid.NewGuid(),
					Modified = DateTime.Now,
					Price = source.Buy2,
					PriceLevelGuid = ParentDefaults.PriceLevel.Buy2,
					ProductGuid = source.Guid,
					ServerStatus = ServerStatusForClient.None,
					Type = EnPriceLevelType.Buy
				});
				destination.AddToPriceList(new ProductPriceRelationBusiness(contextGuid)
				{
					Guid = Guid.NewGuid(),
					Modified = DateTime.Now,
					Price = source.Buy3,
					PriceLevelGuid = ParentDefaults.PriceLevel.Buy3,
					ProductGuid = source.Guid,
					ServerStatus = ServerStatusForClient.None,
					Type = EnPriceLevelType.Buy
				});
				destination.AddToPriceList(new ProductPriceRelationBusiness(contextGuid)
				{
					Guid = Guid.NewGuid(),
					Modified = DateTime.Now,
					Price = source.BuyLast,
					PriceLevelGuid = ParentDefaults.PriceLevel.LastBuy,
					ProductGuid = source.Guid,
					ServerStatus = ServerStatusForClient.None,
					Type = EnPriceLevelType.Buy
				});
			}
			if (source.GroupList?.Any() ?? false)
			{
				destination.AddRangeToGroupRelationList(await ProductGroupRelationMapper.Instance.MapListAsync(contextGuid, source.GroupList, new List<ProductGroupRelationBusiness>(), tranName, token), emptyList: true);
			}
			else
			{
				destination.AddToGroupRelationList(source.GuidGroup);
			}
			destination.MappId = source.MappId;
			destination.NameForDisplay = source.NameForDisplay;
			destination.DefaultVariable = source.DefaultVariable;
			destination.AddMetaData(ProductMetaKey.TaxCode, source.TaxCode);
			destination.AddMetaData(ProductMetaKey.TaxName, source.TaxName);
			ProductSeoModel item = new ProductSeoModel
			{
				SeoDescription = source.SeoDescription,
				SeoMainKeyWord = source.SeoFocusKeyword,
				SeoTitle = source.SeoTitle
			};
			destination.AddMetaData(ProductMetaKey.SeoModel, Json.ToStringJson(item));
			return destination;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\ProductMapper.cs", 692);
			throw;
		}
	}

	public async Task<ProductCompactBusiness> MapAsync(string contextGuid, ProductBusiness source, ProductCompactBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new ProductCompactBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Type = source.Type;
			destination.Modified = source.Modified;
			destination.Special = source.Special;
			destination.Status = source.Status;
			destination.Code = source.Code;
			destination.Description = source.Description;
			destination.DiscountPercent = source.DiscountPercent;
			destination.IdProduct = source.IdProduct;
			destination.Name = source.Name;
			destination.V2Guid = source.V2Guid;
			destination.V2Qt = source.V2Qt;
			destination.GuidVahed = source.GuidVahed;
			destination.Amount = source.Amount;
			destination.IsTax = source.IsTax;
			destination.OrderPoint = source.OrderPoint;
			destination.ImageListData = source.ImageListData;
			destination.ServerStatus = source.ServerStatus;
			destination.TaxGuid = source.TaxGuid;
			destination.MasterGuid = source.MasterGuid;
			destination.VahedName = (await source.GetVahedAsync(token, tranName))?.Name ?? "";
			return destination;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\ProductMapper.cs", 736);
			throw;
		}
	}

	public Task<ProductCompactBusiness> MapAsync(string contextGuid, ProductCompactBusiness source, ProductCompactBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new ProductCompactBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Type = source.Type;
			destination.Modified = source.Modified;
			destination.Special = source.Special;
			destination.Status = source.Status;
			destination.Code = source.Code;
			destination.Description = source.Description;
			destination.DiscountPercent = source.DiscountPercent;
			destination.IdProduct = source.IdProduct;
			destination.Name = source.Name;
			destination.Pic = source.Pic;
			destination.V2Guid = source.V2Guid;
			destination.V2Qt = source.V2Qt;
			destination.GuidVahed = source.GuidVahed;
			destination.Amount = source.Amount;
			destination.IsTax = source.IsTax;
			destination.OrderPoint = source.OrderPoint;
			destination.ImageListData = source.ImageListData;
			destination.ServerStatus = source.ServerStatus;
			destination.TaxGuid = source.TaxGuid;
			return Task.FromResult(destination);
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\ProductMapper.cs", 776);
			throw;
		}
	}

	public Task<List<ProductCompactBusiness>> MapListAsync(string contextGuid, IEnumerable<ProductBusiness> source, List<ProductCompactBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20) ?? Task.FromResult<List<ProductCompactBusiness>>(null);
	}

	public Task<List<ProductBusiness>> MapListAsync(string contextGuid, IEnumerable<WebProduct> source, List<ProductBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20) ?? Task.FromResult<List<ProductBusiness>>(null);
	}

	public Task<List<ProductCompactBusiness>> MapListAsync(string contextGuid, IEnumerable<ProductCompactBusiness> source, List<ProductCompactBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20) ?? Task.FromResult<List<ProductCompactBusiness>>(null);
	}

	public Task<List<ProductBusiness>> MapListAsync(string contextGuid, IEnumerable<ProductBusiness> source, List<ProductBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20) ?? Task.FromResult<List<ProductBusiness>>(null);
	}

	public Task<WebProductCompact> MapAsync(string contextGuid, ProductCompactBusiness source, WebProductCompact destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		int num = 0;
		try
		{
			if (source == null)
			{
				destination = null;
				num = 1;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new WebProductCompact();
			}
			num = 2;
			destination.Guid = source.Guid;
			num = 4;
			destination.Type = source.Type;
			num = 5;
			destination.Modified = source.Modified;
			num = 6;
			destination.Special = source.Special;
			num = 7;
			destination.Status = source.Status;
			destination.Code = source.Code;
			num = 8;
			destination.Description = source.Description;
			num = 9;
			destination.DiscountPercent = source.DiscountPercent;
			num = 10;
			destination.IdProduct = source.IdProduct;
			num = 11;
			destination.Name = source.Name;
			num = 29;
			destination.V2Guid = source.V2Guid;
			num = 30;
			destination.V2Qt = source.V2Qt;
			num = 31;
			num = 32;
			destination.GuidVahed = source.GuidVahed;
			num = 33;
			destination.Amount = source.Amount;
			num = 34;
			destination.IsTax = source.IsTax;
			num = 36;
			destination.OrderPoint = source.OrderPoint;
			num = 38;
			destination.ServerStatus = source.ServerStatus;
			destination.Sell1 = source.Sell1;
			destination.Sell2 = source.Sell2;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, string.Format("The Error Line Is :{0} And ContextGuid Is {1}", num, contextGuid ?? ""), "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\ProductMapper.cs", 840);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebProductCompact>> MapListAsync(string contextGuid, IEnumerable<ProductCompactBusiness> source, List<WebProductCompact> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20) ?? Task.FromResult<List<WebProductCompact>>(null);
	}

	public Task<ProductCompactBusiness> MapAsync(string contextGuid, WebProductCompact source, ProductCompactBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		int num = 0;
		try
		{
			if (source == null)
			{
				destination = null;
				num = 1;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new ProductCompactBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			num = 2;
			destination.Guid = source.Guid;
			num = 4;
			destination.Type = source.Type;
			num = 5;
			destination.Modified = source.Modified;
			num = 6;
			destination.Special = source.Special;
			num = 7;
			destination.Status = source.Status;
			destination.Code = source.Code;
			num = 8;
			destination.Description = source.Description;
			num = 9;
			destination.DiscountPercent = source.DiscountPercent;
			num = 10;
			destination.IdProduct = source.IdProduct;
			num = 11;
			destination.Name = source.Name;
			num = 12;
			destination.V2Guid = source.V2Guid;
			num = 30;
			destination.V2Qt = source.V2Qt;
			num = 31;
			destination.GuidVahed = source.GuidVahed;
			num = 33;
			destination.Amount = source.Amount;
			num = 34;
			destination.IsTax = source.IsTax;
			num = 36;
			destination.OrderPoint = source.OrderPoint;
			num = 38;
			destination.ServerStatus = source.ServerStatus;
			destination.Sell1 = source.Sell1;
			destination.Sell2 = source.Sell2;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, string.Format("The Error Line Is :{0} And ContextGuid Is {1}", num, contextGuid ?? ""), "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\ProductMapper.cs", 899);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<ProductCompactBusiness>> MapListAsync(string contextGuid, IEnumerable<WebProductCompact> source, List<ProductCompactBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20) ?? Task.FromResult<List<ProductCompactBusiness>>(null);
	}
}
