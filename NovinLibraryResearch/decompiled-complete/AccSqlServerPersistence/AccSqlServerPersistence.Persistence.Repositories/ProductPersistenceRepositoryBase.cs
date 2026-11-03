using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Entities.Accounting.Selection;
using AccSqlServerPersistence.Entities.Accounting.Tolid;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence.Repositories.Accounting;
using EntityCache.Business;
using EntityCache.Business.Accounting.Selection;
using EntityCache.Business.Global;
using EntityCache.Business.Products;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Models.Accounting.Prd;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.BusinessLayer.Models.Product;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData;
using PacketParser.EventArgs;
using PacketParser.Filters;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Products;
using SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Products;

namespace AccSqlServerPersistence.Persistence.Repositories;

public abstract class ProductPersistenceRepositoryBase : PersistenceDetailRepository<ProductBusiness, Product>, IProductRepository, IDetailRepository<ProductBusiness>, IEditableDelivery, IDisposable
{
	private class mapper : IItemAsyncMapper<ProductBusiness, Product>, IItemAsyncMapper<Product, ProductBusiness>, IItemMapper<Product, ProductBusiness>, IItemAsyncMapper<Product, ProductForIndexingModel>
	{
		public Task<Product> MapAsync(string contextGuid, ProductBusiness source, Product destination, string tranName, CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs"))
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
						destination = new Product();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.AutoSellSt = source.AutoSellSt;
					destination.BPercent1 = source.BPercent1;
					destination.BPercent2 = source.BPercent2;
					destination.BPercent3 = source.BPercent3;
					destination.BPercent4 = source.BPercent4;
					destination.Code = source.Code;
					destination.CreateDate = source.CreateDate;
					destination.Description = source.Description;
					destination.DiscountPercent = source.DiscountPercent;
					destination.FullContent = source.FullContent;
					destination.Height = source.Height;
					destination.IdProduct = source.IdProduct;
					destination.IsTax = source.IsTax;
					List<ProductTagsRecord> tagList = source.TagList;
					if (tagList != null && tagList.Any())
					{
						destination.Tags = string.Join(",", source.TagList?.Select((ProductTagsRecord q) => q.Tag)?.Where((string q) => !string.IsNullOrEmpty(q))?.ToList());
					}
					else
					{
						destination.Tags = null;
					}
					destination.IsSerial = source.IsSerial;
					destination.KardexPrice = source.KardexPrice;
					destination.Keywords = source.Keywords;
					destination.MetaDesc = source.MetaDesc;
					destination.Mojodi = source.Mojodi;
					destination.Name = source.Name;
					destination.OtherPics = source.OtherPics;
					destination.Pic = "";
					destination.QuickText = source.QuickText;
					destination.ScaleSt = source.ScaleSt;
					destination.Score = source.Score;
					destination.Special = source.Special;
					destination.Status = source.Status;
					destination.V2Qt = source.V2Qt;
					destination.V3Qt = source.V3Qt;
					destination.V4Qt = source.V4Qt;
					destination.Width = source.Width;
					destination.Weight = source.Weight;
					destination.GuidVahed = source.GuidVahed;
					destination.V2Guid = source.V2Guid;
					destination.V3Guid = source.V3Guid;
					destination.V4Guid = source.V4Guid;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryDate = source.DeliveryDate;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.OrderPoint = source.OrderPoint;
					destination.MasterGuid = source.MasterGuid;
					destination.ImageList = ((source.ImageListData != null) ? string.Join(",", source.ImageListData) : "");
					destination.Type = source.Type;
					destination.Sku = source.Sku;
					destination.Slug = source.Slug;
					destination.TolidFormulaGuid = source.TolidFormulaGuid;
					destination.Version = source.Version;
					destination.SendToServerDate = source.SendToServerDate;
					destination.Length = source.Length;
					destination.PayementType = source.PayementType;
					destination.IndexingType = (EnProductIndex)0;
					destination.Kind = source.Kind;
					destination.TaxGuid = source.TaxGuid;
					destination.DiscountStartDate = source.DiscountStartDate;
					destination.DiscountEndDate = source.DiscountEndDate;
					destination.DiscountPrice = source.DiscountPrice;
					destination.IsDependentOnCurrency = source.IsDependentOnCurrency;
					destination.MappId = source.MappId;
					destination.NameForDisplay = source.NameForDisplay ?? "";
					destination.DefaultVariable = source.DefaultVariable;
					if (source.DecimalMetaData != null)
					{
						destination.DecimalMetaData = Json.ToStringJson(source.DecimalMetaData);
					}
					return Task.FromResult(destination);
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 1149);
					throw;
				}
			}
		}

		public Task<ProductBusiness> MapAsync(string contextGuid, Product source, ProductBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return Task.FromResult(Map(contextGuid, source, destination, tranName, token));
		}

		public ProductBusiness Map(string contextGuid, Product source, ProductBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs"))
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
						destination = new ProductBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.MasterGuid = source.MasterGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.AutoSellSt = source.AutoSellSt;
					destination.BPercent1 = source.BPercent1;
					destination.BPercent2 = source.BPercent2;
					destination.BPercent3 = source.BPercent3;
					destination.BPercent4 = source.BPercent4;
					destination.Code = source.Code;
					destination.CreateDate = source.CreateDate;
					destination.Description = source.Description;
					destination.DiscountPercent = source.DiscountPercent;
					destination.FullContent = source.FullContent;
					destination.Height = source.Height;
					destination.IdProduct = source.IdProduct;
					destination.IsTax = source.IsTax;
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
					destination.IsSerial = source.IsSerial;
					destination.KardexPrice = source.KardexPrice;
					destination.Keywords = source.Keywords;
					destination.MetaDesc = source.MetaDesc;
					destination.Mojodi = source.Mojodi;
					destination.Name = source.Name;
					destination.OtherPics = source.OtherPics;
					destination.QuickText = source.QuickText;
					destination.ScaleSt = source.ScaleSt;
					destination.Score = source.Score;
					destination.Special = source.Special;
					destination.Status = source.Status;
					destination.V2Qt = source.V2Qt;
					destination.V3Qt = source.V3Qt;
					destination.V4Qt = source.V4Qt;
					destination.Width = source.Width;
					destination.Weight = source.Weight;
					destination.GuidVahed = source.GuidVahed;
					destination.V2Guid = source.V2Guid;
					destination.V3Guid = source.V3Guid;
					destination.V4Guid = source.V4Guid;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryDate = source.DeliveryDate;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.OrderPoint = source.OrderPoint;
					destination.Type = source.Type;
					destination.ImageListData = source?.ImageList?.SplitString(",");
					destination.Sku = source.Sku;
					destination.Slug = source.Slug;
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
					destination.MappId = source.MappId;
					destination.NameForDisplay = source.NameForDisplay ?? "";
					destination.DefaultVariable = source.DefaultVariable;
					destination.ChangeStatus = EntityChangeSt.Edited;
					if (!string.IsNullOrEmpty(source.DecimalMetaData))
					{
						destination.DecimalMetaData = source.DecimalMetaData.FromJson<ProductDecimalMetaData>("Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 1243);
					}
					return destination;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 1250);
					throw;
				}
			}
		}

		public Task<ProductForIndexingModel> MapAsync(string contextGuid, Product source, ProductForIndexingModel destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs"))
			{
				short num = 0;
				try
				{
					if (source == null)
					{
						destination = null;
						return Task.FromResult(destination);
					}
					num = 1;
					if (destination == null)
					{
						destination = new ProductForIndexingModel();
					}
					num = 2;
					destination.Guid = source.Guid;
					num = 3;
					destination.Description = source.Description;
					num = 4;
					destination.IdProduct = source.IdProduct;
					num = 5;
					destination.Name = source.Name;
					destination.NameForDisplay = source.NameForDisplay ?? "";
					num = 6;
					destination.T1 = source.Technicals?.SingleOrDefault((PrdTechnical q) => q.TechnicalType == 1)?.Name ?? "";
					num = 7;
					destination.T2 = source.Technicals?.SingleOrDefault((PrdTechnical q) => q.TechnicalType == 2)?.Name ?? "";
					num = 8;
					destination.T3 = source.Technicals?.SingleOrDefault((PrdTechnical q) => q.TechnicalType == 3)?.Name ?? "";
					num = 9;
					destination.T4 = source.Technicals?.SingleOrDefault((PrdTechnical q) => q.TechnicalType == 4)?.Name ?? "";
					num = 10;
					destination.T5 = source.Technicals?.SingleOrDefault((PrdTechnical q) => q.TechnicalType == 5)?.Name ?? "";
					num = 11;
					destination.T6 = source.Technicals?.SingleOrDefault((PrdTechnical q) => q.TechnicalType == 6)?.Name ?? "";
					num = 11;
					destination.T7 = source.Technicals?.SingleOrDefault((PrdTechnical q) => q.TechnicalType == 7)?.Name ?? "";
					num = 12;
					destination.T8 = source.Technicals?.SingleOrDefault((PrdTechnical q) => q.TechnicalType == 8)?.Name ?? "";
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, $"#Product : \r\n{Json.ToStringJson(source)}\r\n \ud83d\udd11#Guid : {source.Guid} \r\n#Line_{num}", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 1297);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<Product>> MapListAsync(string contextGuid, IEnumerable<ProductBusiness> source, List<Product> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ProductBusiness>> MapListAsync(string contextGuid, IEnumerable<Product> source, List<ProductBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ProductForIndexingModel>> MapListAsync(string contextGuid, IEnumerable<Product> source, List<ProductForIndexingModel> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	internal class ProductBusinessWithEmptyConstructor : ProductBusiness
	{
		public ProductBusinessWithEmptyConstructor()
			: base(Guid.Empty.ToString())
		{
		}
	}

	protected ProductAttributeRelationPersistenceRepository _relationRepo;

	private ProductPriceRelationPersistenceRepository _priceRepo;

	private ProductGroupRelationPersistenceRepository _groupRepo;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<Product, ProductBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ProductBusiness, Product> MapToDatabase => Mapper;

	public override IItemMapper<Product, ProductBusiness> MapFromDatabase_noneAsync => Mapper;

	public bool IsDisposed { get; private set; }

	public ProductPersistenceRepositoryBase(ContextHolder dbContect)
		: base(dbContect)
	{
		try
		{
			_relationRepo = new ProductAttributeRelationPersistenceRepository(dbContect);
			_priceRepo = new ProductPriceRelationPersistenceRepository(dbContect);
			_groupRepo = new ProductGroupRelationPersistenceRepository(dbContect);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 54);
		}
	}

	public Task<ProductBusiness> GetFromDatabaseAsync(Guid guid, CancellationToken token, string tranName)
	{
		return GetAsync(guid, token, tranName);
	}

	public Task<List<ProductBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ProductBusiness> GetByIdProductAsync(string idProduct, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetByIdProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 61));
		using (new JaegerMonitor("GetByIdProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs"))
		{
			ProductBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					Product source = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Product>((IQueryable<Product>)c.Products, (Expression<Func<Product, bool>>)((Product x) => x.IdProduct == idProduct), token);
					result = await MapFromDatabase.MapAsync(base.ContextGuid, source, null, tranName, token);
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "GetByIdProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 72);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetByIdProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<long> GetNextCodeAsync(Guid defGroupGuid, EnSuggestionType suggestionMode, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetNextCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 83));
		using (new JaegerMonitor("GetNextCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs"))
		{
			long result;
			try
			{
				_ = 1;
				try
				{
					if (suggestionMode == EnSuggestionType.NotSuggestion)
					{
						result = 0L;
					}
					else
					{
						IQueryable<Product> queryable = ((IQueryable<Product>)c.Products).Where((Product p) => p.Code > 0);
						if (defGroupGuid != Guid.Empty)
						{
							List<Guid> validProductGuidList = ((IQueryable<ProductGroupRelation>)c.PrdGroupRelation).Where((ProductGroupRelation q) => q.GroupGuid == defGroupGuid)?.Select((ProductGroupRelation q) => q.ProductGuid)?.ToList();
							if (validProductGuidList?.Any() ?? false)
							{
								queryable = queryable?.Where((Product q) => validProductGuidList.Contains(q.Guid));
							}
						}
						long? num = queryable?.Select((Product q) => q.Code)?.MinOrDefault(100L);
						HashSet<long> hashSet = ((IQueryable<Product>)c.Products).Select((Product p) => p.Code).ToHashSet();
						long num2 = num.Value;
						switch (suggestionMode)
						{
						case EnSuggestionType.FillGaps:
							for (; hashSet.Contains(num2); num2++)
							{
							}
							result = num2;
							break;
						case EnSuggestionType.NextBiggest:
						{
							HashSet<long> hashSet2 = hashSet;
							if (defGroupGuid != Guid.Empty)
							{
								hashSet2 = queryable?.Select((Product p) => p.Code)?.ToHashSet();
							}
							num2 = ((hashSet2 == null || !hashSet2.Any()) ? 1 : (hashSet2.Max() + 1));
							result = ((!hashSet.Contains(num2)) ? num2 : (await GetNextCodeAsync(defGroupGuid, EnSuggestionType.FillGaps, token, tranName)));
							break;
						}
						default:
							throw new NotImplementedException();
						}
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetNextCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 120);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetNextCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<ProductBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				foreach (ProductBusiness item in items)
				{
					if (item != null)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 140);
						if (ret.HasError)
						{
							return ret;
						}
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 146);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 147);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(ProductBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 157));
				if (item.ProductAttributes?.Any() ?? false)
				{
					ProductPriceLevel priceType = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<ProductPriceLevel>((IQueryable<ProductPriceLevel>)dbcontext.ProductPriceLevel, (Expression<Func<ProductPriceLevel, bool>>)((ProductPriceLevel q) => q.Type == EnPriceLevelType.Sell && q.Ordinal == 1), default(CancellationToken));
					if (priceType != null)
					{
						List<ProductPriceRelationBusiness> list = item.ProductAttributes?.SelectMany((ProductBusiness q) => q.PriceList)?.Where((ProductPriceRelationBusiness q) => q.PriceLevelGuid == priceType.Guid)?.ToList();
						if (list != null && list.Any())
						{
							await item.UpdateFirstSellAsync(EnSystem.Mapper, list.Min((ProductPriceRelationBusiness q) => q.Price), token, tranName);
						}
					}
					ProductPriceLevel priceType2 = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<ProductPriceLevel>((IQueryable<ProductPriceLevel>)dbcontext.ProductPriceLevel, (Expression<Func<ProductPriceLevel, bool>>)((ProductPriceLevel q) => q.Type == EnPriceLevelType.Sell && q.Ordinal == 2), default(CancellationToken));
					if (priceType2 != null)
					{
						List<ProductPriceRelationBusiness> list2 = item.ProductAttributes?.SelectMany((ProductBusiness q) => q.PriceList)?.Where((ProductPriceRelationBusiness q) => q.PriceLevelGuid == priceType2.Guid)?.ToList();
						if (list2 != null && list2.Any())
						{
							item.UpdatePrice(priceType2.Guid, list2.Min((ProductPriceRelationBusiness q) => q.Price), EnPriceLevelType.Sell, token, tranName);
						}
					}
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				Acc_ModelContext acc_ModelContext = dbcontext;
				returnedSaveFuncInfo.AddReturnedValue(await acc_ModelContext.AddOrUpdateAsync(await Mapper.MapAsync(base.ContextGuid, item, new Product(), tranName, token), item.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 178);
				if (ret.HasError)
				{
					return ret;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await _priceRepo.SaveAsync(item.PriceList?.ToList(), item.Guid, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 181);
				if (ret.HasError)
				{
					return ret;
				}
				if (item.GroupList?.Any() ?? false)
				{
					foreach (ProductGroupRelationBusiness group in item.GroupList)
					{
						if (group == null)
						{
							throw new ArgumentException();
						}
						if (group.ProductGuid == Guid.Empty)
						{
							group.ProductGuid = item.Guid;
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await _groupRepo.SaveAsync(item.GroupList?.ToList(), item.Guid, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 191);
				if (ret.HasError)
				{
					return ret;
				}
				if ((item.AttributeRelations?.Any() ?? false) || (item.ProductAttributes?.Any() ?? false))
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await _relationRepo.RemoveRangeAsync(item.Guid, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 196);
					if (ret.HasError)
					{
						return ret;
					}
					foreach (ProductBusiness attribute in item.ProductAttributes)
					{
						if (ret.HasError)
						{
							return ret;
						}
						attribute.GuidVahed = item.GuidVahed;
						attribute.MasterGuid = item.Guid;
						attribute.ServerStatus = ServerStatusForClient.None;
						attribute.Special = item.Special;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await _priceRepo.SaveAsync(attribute.PriceList?.ToList(), attribute.Guid, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 207);
						if (ret.HasError)
						{
							return ret;
						}
						if (attribute.GroupList?.Any() ?? false)
						{
							foreach (ProductGroupRelationBusiness group2 in attribute.GroupList)
							{
								if (group2 == null)
								{
									throw new ArgumentException();
								}
								if (group2.ProductGuid == Guid.Empty)
								{
									group2.ProductGuid = attribute.Guid;
								}
							}
						}
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await _groupRepo.SaveAsync(attribute.GroupList?.ToList(), attribute.Guid, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 217);
						if (ret.HasError)
						{
							return ret;
						}
						Product product = await MapToDatabase.MapAsync(base.ContextGuid, attribute, null, tranName, token);
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(product, product.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 221);
					}
					if (ret.HasError)
					{
						return ret;
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await _relationRepo.SaveRangeAsync(item.AttributeRelations, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 224);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 229);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 230);
			}
			return ret;
		}
	}

	public override Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse res, CancellationToken token, short tryCount, string tranName)
	{
		return base.SetSaveResultAsync(res, token, tryCount, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SetDelivery(WebDelivery del, CancellationToken token)
	{
		return SetDelivery(del, token);
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 245);
		}
	}

	public void ResetCache(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<List<ProductBusiness>> GetVariationProductAsync(Guid prdGuid, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetVariationProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext context = null;
			List<ProductBusiness> result;
			try
			{
				_ = 1;
				try
				{
					context = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetVariationProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 258));
					List<Product> source = ((IQueryable<Product>)context.Products).Where((Product w) => w.MasterGuid == prdGuid)?.ToList();
					result = await MapFromDatabase.MapListAsync(base.ContextGuid, source, new List<ProductBusiness>(), tranName, token);
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "GetVariationProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 267);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await context.DisposeAsync("GetVariationProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<ProductBusiness> GetByMappedIdAsync(string mappedId, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetByMappedIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 278));
		using (new JaegerMonitor("GetByMappedIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs"))
		{
			ProductBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 14;
				try
				{
					if (string.IsNullOrEmpty(mappedId))
					{
						result = null;
					}
					else
					{
						Product temp = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Product>((IQueryable<Product>)c.Products, (Expression<Func<Product, bool>>)((Product x) => x.MappId.ToLower() == mappedId.ToLower()), token);
						if (temp == null)
						{
							result = null;
						}
						else
						{
							ProductBusiness mappedData = await MapFromDatabase.MapAsync(base.ContextGuid, temp, null, tranName, token);
							IEnumerable<ProductGroupRelationBusiness> enumerable = await _groupRepo.GetAllAsync(temp.Guid, null, token, tranName);
							if (enumerable != null && enumerable.Any())
							{
								mappedData.AddRangeToGroupRelationList(enumerable, emptyList: true);
							}
							IEnumerable<ProductPriceRelationBusiness> enumerable2 = await _priceRepo.GetAllAsync(new ProductPriceRelationFilter
							{
								ProductGuid = temp.Guid
							}, token, tranName);
							if (enumerable2 != null && enumerable2.Any())
							{
								await mappedData.AddRangeToPriceListAsync(enumerable2?.ToList(), token);
							}
							if (mappedData.Kind == EnProductKind.Variation)
							{
								ProductPriceLevel priceType = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<ProductPriceLevel>((IQueryable<ProductPriceLevel>)c.ProductPriceLevel, (Expression<Func<ProductPriceLevel, bool>>)((ProductPriceLevel q) => q.Type == EnPriceLevelType.Sell && q.Ordinal == 1), default(CancellationToken));
								ProductPriceLevel priceType2 = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<ProductPriceLevel>((IQueryable<ProductPriceLevel>)c.ProductPriceLevel, (Expression<Func<ProductPriceLevel, bool>>)((ProductPriceLevel q) => q.Type == EnPriceLevelType.Sell && q.Ordinal == 2), default(CancellationToken));
								IEnumerable<ProductAttributeRelationBusiness> enumerable3 = await _relationRepo.GetAllWithMasterProductGuidAsync(mappedData.Guid, token, tranName);
								if (enumerable3 != null && enumerable3.Any())
								{
									await mappedData.AddAttributeRelationListAsync(enumerable3, token);
								}
								List<Product> list = await EntityFrameworkQueryableExtensions.ToListAsync<Product>(((IQueryable<Product>)EntityFrameworkQueryableExtensions.Include<Product, ICollection<ProductPriceRelation>>((IQueryable<Product>)EntityFrameworkQueryableExtensions.Include<Product, ICollection<PrdAnbarRelation>>((IQueryable<Product>)c.Products, (Expression<Func<Product, ICollection<PrdAnbarRelation>>>)((Product q) => q.PrdAnbarRelations)), (Expression<Func<Product, ICollection<ProductPriceRelation>>>)((Product q) => q.ProductPriceRelation))).Where((Product q) => q.MasterGuid == temp.Guid), token);
								if (list != null && list.Any())
								{
									foreach (Product item in list)
									{
										ProductBusiness mappedVariation = await MapFromDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
										mappedVariation.Amount = item.PrdAnbarRelations.Sum((PrdAnbarRelation x) => x.Amount);
										if (priceType != null)
										{
											mappedVariation.VariationPrice = item.ProductPriceRelation.SingleOrDefault((ProductPriceRelation q) => q.PriceLevelGuid == priceType.Guid)?.Price ?? 0m;
										}
										if (priceType2 != null)
										{
											mappedVariation.VariationPrice2 = item.ProductPriceRelation.SingleOrDefault((ProductPriceRelation q) => q.PriceLevelGuid == priceType2.Guid)?.Price ?? 0m;
										}
										IEnumerable<ProductGroupRelationBusiness> enumerable4 = await _groupRepo.GetAllAsync(item.Guid, null, token, tranName);
										if (enumerable4 != null && enumerable4.Any())
										{
											mappedVariation.AddRangeToGroupRelationList(enumerable4, emptyList: true);
										}
										IEnumerable<ProductPriceRelationBusiness> enumerable5 = await _priceRepo.GetAllAsync(new ProductPriceRelationFilter
										{
											ProductGuid = item.Guid
										}, token, tranName);
										if (enumerable5 != null && enumerable5.Any())
										{
											await mappedVariation.AddRangeToPriceListAsync(enumerable5?.ToList(), token);
										}
										await mappedData.AddAttributeAsync(mappedVariation, token);
									}
								}
							}
							result = mappedData;
						}
					}
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "GetByMappedIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 330);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetByMappedIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<ProductBusiness> GetBySkuAsync(string sku, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetBySkuAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 341));
		using (new JaegerMonitor("GetBySkuAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs"))
		{
			ProductBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					if (string.IsNullOrEmpty(sku))
					{
						sku = "";
					}
					Product source = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Product>((IQueryable<Product>)c.Products, (Expression<Func<Product, bool>>)((Product x) => x.Sku.ToLower() == sku.ToLower()), token);
					result = await MapFromDatabase.MapAsync(base.ContextGuid, source, null, tranName, token);
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "GetBySkuAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 353);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetBySkuAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<ProductBusiness> GetBySlugAsync(string slug, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetBySlugAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 364));
		using (new JaegerMonitor("GetBySlugAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs"))
		{
			ProductBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					if (string.IsNullOrEmpty(slug))
					{
						slug = "";
					}
					Product source = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Product>((IQueryable<Product>)c.Products, (Expression<Func<Product, bool>>)((Product x) => x.Slug.ToLower() == slug.ToLower()), token);
					result = await MapFromDatabase.MapAsync(base.ContextGuid, source, null, tranName, token);
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "GetBySlugAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 376);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetBySlugAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(List<Guid> productGuids, ServerStatusForClient status, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (!(productGuids?.Any() ?? false))
				{
					return ret;
				}
				ReturnedSaveFuncInfoWithValue<int> refIf = await StimulQueryBusiness.SaveAsync(base.ContextGuid, productGuids, token);
				ret.AddReturnedValue(refIf, ReturnedState.None, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 394);
				BaseModelContext obj = await dbContext.GetContextAsync(tranName, 60000, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 396);
				string text = "Update Acc_Product Set ServerStatus = {0} , modified=getdate() from Acc_Product inner join StimulQuery on StimulQuery.Guid=Acc_Product.Guid Where ServerStatus<>{0} And StimulQuery.RefrencedId={1}";
				await RelationalDatabaseFacadeExtensions.ExecuteSqlRawAsync(((DbContext)obj).Database, text, (IEnumerable<object>)new List<object>
				{
					(short)status,
					refIf.value
				}, token);
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 400);
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 404);
				ret.AddReturnedValue(ex2, "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 405);
			}
			return ret;
		}
	}

	public abstract Task<ReturnedSaveFuncInfo> ChangeTaxPercentAsync(IEnumerable<Guid> productGuidList, Guid? taxGuid, CancellationToken token, string tranName);

	public async Task<ReturnedSaveFuncInfo> UpdateSendToServerDateAsync(Guid guid, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("UpdateSendToServerDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateSendToServerDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 417));
				Product product = (await dbcontext.Products.FindAsync(new object[1] { guid })) ?? throw new NullReferenceException($"product with guid({guid}) not founded!");
				product.SendToServerDate = DateTime.Now;
				((DbContext)dbcontext).Update<Product>(product);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateSendToServerDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 424);
				ret.AddReturnedValue(ex, "", "UpdateSendToServerDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 425);
			}
			return ret;
		}
	}

	public virtual Task<ReturnedSaveFuncInfo> ReNewAllCodesAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public virtual Task<ReturnedSaveFuncInfo> ReNewEmptyCodesAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public virtual Task<ReturnedSaveFuncInfo> UpdateRegularPriceAsync(Guid priceLevelGuid, Guid regularPriceGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	private List<Product> MapWebPrdToDatabase(List<WebProduct> lst, CancellationToken token)
	{
		return lst.Select((WebProduct source) => new Product
		{
			Guid = source.Guid,
			Modified = source.Modified,
			AutoSellSt = source.AutoSellSt,
			BPercent1 = source.BPercent1,
			BPercent2 = source.BPercent2,
			BPercent3 = source.BPercent3,
			BPercent4 = source.BPercent4,
			Code = source.Code,
			CreateDate = source.CreateDate,
			Description = source.Description,
			DiscountPercent = source.DiscountPercent,
			FullContent = source.FullContent,
			Height = source.Height,
			IdProduct = source.IdProduct,
			IsTax = source.IsTax,
			Tags = source.Tags,
			IsSerial = source.IsSerial,
			KardexPrice = source.KardexPrice,
			Keywords = source.Keywords,
			MetaDesc = source.MetaDesc,
			Mojodi = source.Mojodi,
			Name = source.Name,
			OtherPics = source.OtherPics,
			Pic = "",
			QuickText = source.QuickText,
			ScaleSt = source.ScaleSt,
			Score = source.Score,
			Special = source.Special,
			Status = source.Status,
			V2Qt = source.V2Qt,
			V3Qt = source.V3Qt,
			V4Qt = source.V4Qt,
			Width = source.Width,
			Weight = source.Weight,
			GuidVahed = source.GuidVahed,
			V2Guid = source.V2Guid,
			V3Guid = source.V3Guid,
			V4Guid = source.V4Guid,
			ServerStatus = source.ServerStatus,
			DeliveryDate = source.DeliveryDate,
			DeliveryId = source.DeliveryId,
			DeliveryMessage = source.DeliveryMessage,
			DeliveryShortMessage = source.DeliveryShortMessage,
			HostRefrenced = source.HostRefrenced,
			OrderPoint = source.OrderPoint,
			MasterGuid = source.MasterGuid,
			ImageList = ((source.ImageListData != null) ? string.Join(",", source.ImageListData) : ""),
			Type = source.Type,
			Sku = source.Sku,
			Slug = source.Slug,
			TolidFormulaGuid = source.TolidFormulaGuid,
			Version = source.Version,
			SendToServerDate = source.SendToServerDate,
			Length = source.Length,
			PayementType = source.PayementType,
			Kind = source.Kind,
			DiscountEndDate = source.DiscountEndDate,
			IsDependentOnCurrency = source.IsDependentOnCurrency,
			NameForDisplay = (source.NameForDisplay ?? ""),
			MappId = source.MappId,
			TaxGuid = source.TaxGuid,
			DiscountStartDate = source.DiscountStartDate
		}).ToList();
	}

	private List<PrdAnbarRelation> MapWebPrdToAnbarRelation(List<WebProduct> lst, CancellationToken token)
	{
		return lst.Select((WebProduct prd) => prd.PrdAnbarRelation.Select((WebPrdAnbarRelation anbar) => new PrdAnbarRelation
		{
			PrdGuid = prd.Guid,
			AnbarGuid = anbar.AnbarGuid,
			Amount = anbar.Amount,
			Min = anbar.Min,
			Modified = DateTime.Now
		})).SelectMany((IEnumerable<PrdAnbarRelation> p) => p).ToList();
	}

	private List<ProductPriceRelation>? MapWebPrdToPriceRelation(List<WebProduct> lst, CancellationToken token)
	{
		return (lst?.Where((WebProduct s) => s.Prices != null)?.Select((WebProduct prd) => prd.Prices?.Select((WebProductPriceRelation anbar) => new ProductPriceRelation
		{
			Guid = anbar.Guid,
			DeliveryId = anbar.DeliveryId,
			KasrRial = anbar.KasrRial,
			DeliveryDate = anbar.DeliveryDate,
			DeliveryMessage = anbar.DeliveryMessage,
			DeliveryShortMessage = anbar.DeliveryShortMessage,
			HostRefrenced = anbar.HostRefrenced,
			Price = anbar.Price,
			PriceLevelGuid = anbar.PriceLevelGuid,
			ProductGuid = anbar.ProductGuid,
			Profit = anbar.Profit,
			Quantity = anbar.Quantity,
			ServerStatus = anbar.ServerStatus,
			Status = anbar.Status,
			Type = anbar.Type,
			UnitGuid = anbar.UnitGuid,
			Modified = DateTime.Now
		})).ToList()).SelectMany((IEnumerable<ProductPriceRelation> p) => p)?.ToList();
	}

	private List<Barcode> MapWebPrdToBarcode(List<WebProduct> lst, CancellationToken token)
	{
		return lst.Select((WebProduct prd) => prd.PrdBarcode.Select((WebBarcode q) => new Barcode
		{
			PrdGuid = prd.Guid,
			BarCode = q.BarCode,
			CreateDate = q.CreateDate,
			DepotGuid = q.DepotGuid,
			Description = q.Description,
			Name = q.Name,
			Price = q.Price,
			PriceTypeGuid = q.PriceTypeGuid,
			Quantity = q.Quantity,
			Type = q.Type
		})).SelectMany((IEnumerable<Barcode> p) => p).ToList();
	}

	private List<PrdTechnical> MapWebPrdToTechnicals(List<WebProduct> lst, CancellationToken token)
	{
		return lst.Select((WebProduct prd) => prd.PrdTechnicalList.Select((WebPrdTechnical q) => new PrdTechnical
		{
			Name = q.Name,
			TechnicalType = q.TechnicalType,
			Code = q.Code,
			Guid = q.Guid,
			Modified = q.Modified,
			ProductGuid = q.ProductGuid,
			Status = q.Status
		})).SelectMany((IEnumerable<PrdTechnical> p) => p).ToList();
	}

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<WebProduct> items, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 569));
			try
			{
				List<WebProduct> list = items.ToList();
				List<Product> dbItems = MapWebPrdToDatabase(list, token);
				List<PrdAnbarRelation> relationDbItems = MapWebPrdToAnbarRelation(list, token);
				List<Barcode> barcodeDbItems = MapWebPrdToBarcode(list, token);
				barcodeDbItems?.Select((Barcode q) => q.BarCode)?.ToList();
				new List<Barcode>();
				List<Guid> lst = dbItems?.Select((Product q) => q.Guid)?.ToList();
				List<ProductPriceRelation> prices = MapWebPrdToPriceRelation(list, token);
				List<ProductGroupRelation> grupRelation = MapWebPrdToGroupRelation(list, token);
				if (prices == null)
				{
					prices = new List<ProductPriceRelation>();
				}
				List<PrdTechnical> techList = new List<PrdTechnical>();
				if (list == null || !list.Any())
				{
					techList = MapWebPrdToTechnicals(list, token);
				}
				token.ThrowIfCancellationRequested();
				ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = await StimulQueryBusiness.SaveAsync(base.ContextGuid, lst);
				returnedSaveFuncInfoWithValue.throwExceptionIfError();
				string spName = $"Delete from Acc_Barcode Where PrdGuid in (Select Guid From StimulQuery where RefrencedId={returnedSaveFuncInfoWithValue.value})";
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync(spName, null, tranName, token, "", CommandType.StoredProcedure, 30, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs"), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 593);
				token.ThrowIfCancellationRequested();
				foreach (Product item in dbItems)
				{
					token.ThrowIfCancellationRequested();
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(item, item.Guid), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 599);
				}
				foreach (PrdAnbarRelation item2 in relationDbItems)
				{
					if (item2 != null)
					{
						token.ThrowIfCancellationRequested();
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(item2, item2.PrdGuid, item2.AnbarGuid), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 606);
					}
				}
				foreach (Barcode item3 in barcodeDbItems)
				{
					token.ThrowIfCancellationRequested();
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(item3, item3.BarCode), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 612);
				}
				foreach (PrdTechnical item4 in techList)
				{
					token.ThrowIfCancellationRequested();
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(item4, item4.ProductGuid, item4.TechnicalType), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 618);
				}
				if (prices != null && prices.Any())
				{
					List<string> values = prices.Select((ProductPriceRelation a) => "'" + a.ProductGuid.ToString().ToUpper() + "'").ToList();
					spName = "Delete from Acc_ProductPriceRelation WHERE Guid IN (" + string.Join(",", values) + ")";
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync(spName, null, tranName, token, "", CommandType.StoredProcedure, 30, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs"), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 624);
					foreach (ProductPriceRelation item5 in prices)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(item5, token, item5.Guid), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 627);
					}
				}
				if (grupRelation != null && grupRelation.Any())
				{
					List<string> values2 = grupRelation.Select((ProductGroupRelation a) => "'" + a.ProductGuid.ToString().ToUpper() + "'").ToList();
					spName = "Delete from Acc_ProductGroupRelation WHERE ProductGuid IN (" + string.Join(",", values2) + ")";
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync(spName, null, tranName, token, "", CommandType.StoredProcedure, 30, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs"), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 634);
					await ((DbContext)c).AddRangeAsync((IEnumerable<object>)grupRelation, default(CancellationToken));
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 641);
				ret.AddReturnedValue(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 642);
			}
			return ret;
		}
	}

	private List<ProductGroupRelation>? MapWebPrdToGroupRelation(List<WebProduct> temp, CancellationToken token)
	{
		return ((from s in temp?.Where((WebProduct s) => s.GroupList != null)
			select from a in s.GroupList
				select new ProductGroupRelation
				{
					GroupGuid = a.GroupGuid,
					Ordinal = a.Ordinal,
					ProductGuid = s.Guid
				})?.ToList())?.SelectMany((IEnumerable<ProductGroupRelation> s) => s)?.ToList();
	}

	public virtual async Task<HashSet<Guid>> GetAllForIndexingAsync(EnProductIndex mode, int count, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllForIndexingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 658));
		HashSet<Guid> result;
		try
		{
			_ = 1;
			try
			{
				List<Guid> list = await EntityFrameworkQueryableExtensions.ToListAsync<Guid>((from q in (IQueryable<Product>)c.Products
					where q.IndexingType != mode
					orderby Guid.NewGuid()
					select q into p
					select p.Guid).Take(count), token);
				if (list == null || !list.Any())
				{
					result = null;
				}
				else
				{
					HashSet<Guid> hashSet = new HashSet<Guid>();
					foreach (Guid item in list)
					{
						hashSet.Add(item);
					}
					result = hashSet;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllForIndexingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 672);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllForIndexingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}

	public async Task<IEnumerable<ProductForIndexingModel>> GetAllForIndexingAsync(HashSet<Guid> guids, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllForIndexingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 683));
		IEnumerable<ProductForIndexingModel> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<Product> list = await EntityFrameworkQueryableExtensions.ToListAsync<Product>(((IQueryable<Product>)EntityFrameworkQueryableExtensions.Include<Product, ICollection<PrdTechnical>>((IQueryable<Product>)c.Products, (Expression<Func<Product, ICollection<PrdTechnical>>>)((Product q) => q.Technicals))).Where((Product q) => guids.Contains(q.Guid)), token);
				result = ((list != null && list.Any()) ? (await Mapper.MapListAsync(base.ContextGuid, list, new List<ProductForIndexingModel>(), tranName, token)) : null);
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllForIndexingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 694);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllForIndexingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}

	public async Task<ReturnedSaveFuncInfo> UpdateIndexTypeAsync(HashSet<Guid> guids, EnProductIndex mode, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateIndexTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 708));
			ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = await StimulQueryBusiness.SaveAsync(base.ContextGuid, guids);
			res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "UpdateIndexTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 710);
			res.throwExceptionIfError();
			string spName = $"Update Acc_Product set IndexingType={(short)mode} Where Guid in (Select Guid From StimulQuery where RefrencedId={returnedSaveFuncInfoWithValue.value})";
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync(spName, null, tranName, token, "", CommandType.Text, 30, "UpdateIndexTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs"), ReturnedState.None, "UpdateIndexTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 713);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateIndexTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 717);
			res.AddReturnedValue(ex, "", "UpdateIndexTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 718);
		}
		return res;
	}

	public virtual Task<IEnumerable<ProductDiscountModel>> GetAllDiscountModelAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> UpdateDiscountPercentAsync(List<Tuple<Guid, decimal>> changes, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("UpdateDiscountPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			if (!(changes?.Any() ?? false))
			{
				return res;
			}
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateDiscountPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 730));
				foreach (Tuple<Guid, decimal> item in changes)
				{
					if (IsDisposed || token.IsCancellationRequested)
					{
						return res.AddErrorTokenIsCancelled("", "", "UpdateDiscountPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 733);
					}
					if (!((item?.Item1 ?? Guid.Empty) == Guid.Empty))
					{
						Product product = await c.Products.FindAsync(new object[1] { item.Item1 });
						if (!((product?.DiscountPercent ?? 0m) == item.Item2))
						{
							product.DiscountPercent = item.Item2;
							product.ServerStatus = ServerStatusForClient.None;
							product.Modified = DateTime.Now;
							c.Products.Update(product);
						}
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateDiscountPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 746);
				res.AddReturnedValue(ex, "", "UpdateDiscountPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 747);
			}
			return res;
		}
	}

	public override async Task<ProductBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext c = null;
			ProductBusiness result;
			try
			{
				_ = 15;
				try
				{
					c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 757));
					Product product = await c.Products.FindAsync(new object[1] { guid }, token);
					if (product == null)
					{
						result = null;
					}
					else
					{
						ProductBusiness mappedData = await MapFromDatabase.MapAsync(base.ContextGuid, product, null, tranName, token);
						IEnumerable<ProductGroupRelationBusiness> enumerable = await _groupRepo.GetAllAsync(guid, null, token, tranName);
						if (enumerable != null && enumerable.Any())
						{
							mappedData.AddRangeToGroupRelationList(enumerable, emptyList: true);
						}
						IEnumerable<ProductPriceRelationBusiness> enumerable2 = await _priceRepo.GetAllAsync(new ProductPriceRelationFilter
						{
							ProductGuid = guid
						}, token, tranName);
						if (enumerable2 != null && enumerable2.Any())
						{
							await mappedData.AddRangeToPriceListAsync(enumerable2?.ToList(), token);
						}
						if (mappedData.Kind == EnProductKind.Variation)
						{
							ProductPriceLevel priceType = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<ProductPriceLevel>((IQueryable<ProductPriceLevel>)c.ProductPriceLevel, (Expression<Func<ProductPriceLevel, bool>>)((ProductPriceLevel q) => q.Type == EnPriceLevelType.Sell && q.Ordinal == 1), default(CancellationToken));
							ProductPriceLevel priceType2 = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<ProductPriceLevel>((IQueryable<ProductPriceLevel>)c.ProductPriceLevel, (Expression<Func<ProductPriceLevel, bool>>)((ProductPriceLevel q) => q.Type == EnPriceLevelType.Sell && q.Ordinal == 2), default(CancellationToken));
							IEnumerable<ProductAttributeRelationBusiness> enumerable3 = await _relationRepo.GetAllWithMasterProductGuidAsync(mappedData.Guid, token, tranName);
							if (enumerable3 != null && enumerable3.Any())
							{
								await mappedData.AddAttributeRelationListAsync(enumerable3, token);
							}
							List<Product> list = await EntityFrameworkQueryableExtensions.ToListAsync<Product>(((IQueryable<Product>)EntityFrameworkQueryableExtensions.Include<Product, ICollection<ProductPriceRelation>>((IQueryable<Product>)EntityFrameworkQueryableExtensions.Include<Product, ICollection<PrdAnbarRelation>>((IQueryable<Product>)c.Products, (Expression<Func<Product, ICollection<PrdAnbarRelation>>>)((Product q) => q.PrdAnbarRelations)), (Expression<Func<Product, ICollection<ProductPriceRelation>>>)((Product q) => q.ProductPriceRelation))).Where((Product q) => q.MasterGuid == guid), token);
							if (list != null && list.Any())
							{
								foreach (Product item in list)
								{
									ProductBusiness mappedVariation = await MapFromDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
									mappedVariation.Amount = item.PrdAnbarRelations.Sum((PrdAnbarRelation x) => x.Amount);
									if (priceType != null)
									{
										mappedVariation.VariationPrice = item.ProductPriceRelation.SingleOrDefault((ProductPriceRelation q) => q.PriceLevelGuid == priceType.Guid)?.Price ?? 0m;
									}
									if (priceType2 != null)
									{
										mappedVariation.VariationPrice2 = item.ProductPriceRelation.SingleOrDefault((ProductPriceRelation q) => q.PriceLevelGuid == priceType2.Guid)?.Price ?? 0m;
									}
									IEnumerable<ProductGroupRelationBusiness> enumerable4 = await _groupRepo.GetAllAsync(item.Guid, null, token, tranName);
									if (enumerable4 != null && enumerable4.Any())
									{
										mappedVariation.AddRangeToGroupRelationList(enumerable4, emptyList: true);
									}
									IEnumerable<ProductPriceRelationBusiness> enumerable5 = await _priceRepo.GetAllAsync(new ProductPriceRelationFilter
									{
										ProductGuid = item.Guid
									}, token, tranName);
									if (enumerable5 != null && enumerable5.Any())
									{
										await mappedVariation.AddRangeToPriceListAsync(enumerable5?.ToList(), token);
									}
									await mappedData.AddAttributeAsync(mappedVariation, token);
								}
							}
						}
						result = mappedData;
					}
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					if (ex3.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 805);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> UpdateStatusAsync(Guid guid, bool newStatus, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("UpdateStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 820));
				Product product = await c.Products.FindAsync(new object[1] { guid });
				if (product == null)
				{
					return res;
				}
				product.Status = newStatus;
				product.ServerStatus = ServerStatusForClient.None;
				product.Modified = DateTime.Now;
				c.Products.Update(product);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 830);
				res.AddReturnedValue(ex, "", "UpdateStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 831);
			}
			return res;
		}
	}

	public virtual Task<ReturnedSaveFuncInfo> UpdateMasterProductAmountAsync(Guid? guid, CancellationToken token, string tranName)
	{
		return Task.FromResult(new ReturnedSaveFuncInfo());
	}

	public async Task<List<ProductTolidReportModel>> GetAllTolidReportAsync(Guid productGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllTolidReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 838));
		List<ProductTolidReportModel> result2;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<ProductTolidReportModel> result = new List<ProductTolidReportModel>();
				List<TolidFormulaExport> list = await EntityFrameworkQueryableExtensions.ToListAsync<TolidFormulaExport>(((IQueryable<TolidFormulaExport>)EntityFrameworkQueryableExtensions.Include<TolidFormulaExport, TolidFormula>((IQueryable<TolidFormulaExport>)c.TolidFormulaExports, (Expression<Func<TolidFormulaExport, TolidFormula>>)((TolidFormulaExport q) => q.Master))).Where((TolidFormulaExport q) => q.ProductGuid == productGuid), token);
				if (list != null && list.Any())
				{
					result.AddRange(list.Select((TolidFormulaExport q) => new ProductTolidReportModel
					{
						DateM = q.Master.DateM,
						ProductGuid = productGuid,
						Quantity = q.Quantity,
						Title = q.Master.Title,
						Type = EnProductTolidType.Export,
						TolidFormulaGuid = q.MasterGuid
					}).ToList());
				}
				List<TolidFormulaMavad> list2 = await EntityFrameworkQueryableExtensions.ToListAsync<TolidFormulaMavad>(((IQueryable<TolidFormulaMavad>)EntityFrameworkQueryableExtensions.Include<TolidFormulaMavad, TolidFormula>((IQueryable<TolidFormulaMavad>)c.TolidFormulaMavads, (Expression<Func<TolidFormulaMavad, TolidFormula>>)((TolidFormulaMavad q) => q.Master))).Where((TolidFormulaMavad q) => q.ProductGuid == productGuid), token);
				if (list2 != null && list2.Any())
				{
					result.AddRange(list2.Select((TolidFormulaMavad q) => new ProductTolidReportModel
					{
						DateM = q.Master.DateM,
						ProductGuid = productGuid,
						Quantity = q.Quantity,
						Title = q.Master.Title,
						Type = EnProductTolidType.Mavad,
						TolidFormulaGuid = q.MasterGuid
					}).ToList());
				}
				result2 = result;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllTolidReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 872);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllTolidReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs");
			}
		}
		return result2;
	}

	public async Task<ProductBusiness> GetAsync(Guid guid, Guid? depotGuid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext c = null;
			ProductBusiness result;
			try
			{
				_ = 3;
				try
				{
					c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 887));
					ProductBusiness product = await GetAsync(guid, token, tranName);
					if (product == null)
					{
						result = null;
					}
					else
					{
						if ((depotGuid ?? Guid.Empty) != Guid.Empty)
						{
							PrdAnbarRelation prdAnbarRelation = await c.PrdAnbarRelation.FindAsync(new object[2] { guid, depotGuid.Value });
							if (prdAnbarRelation != null)
							{
								product.Amount = prdAnbarRelation.Amount;
							}
							if ((product?.ProductAttributes?.Any()).GetValueOrDefault())
							{
								foreach (ProductBusiness item in product.ProductAttributes)
								{
									PrdAnbarRelation prdAnbarRelation2 = await c.PrdAnbarRelation.FindAsync(new object[2] { item.Guid, depotGuid.Value });
									if (prdAnbarRelation2 != null)
									{
										item.Amount = prdAnbarRelation2.Amount;
									}
								}
							}
						}
						else
						{
							product.Amount = ((IQueryable<PrdAnbarRelation>)c.PrdAnbarRelation).Where((PrdAnbarRelation q) => q.PrdGuid == guid)?.Sum((PrdAnbarRelation q) => q.Amount) ?? 0m;
						}
						result = product;
					}
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					if (ex3.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 920);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> UpdateProductsAsync(List<ProductUpdateBaseModel> originalList, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("UpdateProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				if (!(originalList?.Any() ?? false))
				{
					return res;
				}
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 936));
				ProductUpdateBaseModel item;
				foreach (ProductUpdateBaseModel original in originalList)
				{
					item = original;
					if (!(item?.DetailList?.Any()).GetValueOrDefault())
					{
						continue;
					}
					Product prd = await c.Products.FindAsync(new object[1] { item.ProductGuid });
					if (prd == null)
					{
						continue;
					}
					prd.ServerStatus = ServerStatusForClient.None;
					prd.Modified = DateTime.Now;
					foreach (ProductUpdateDetailModel detail in item.DetailList)
					{
						if (detail == null)
						{
							continue;
						}
						switch (detail.Key)
						{
						case EnProductUpdateKeys.Visitor1Percent:
							prd.BPercent1 = (decimal)detail.Value;
							break;
						case EnProductUpdateKeys.Visitor2Percent:
							prd.BPercent2 = (decimal)detail.Value;
							break;
						case EnProductUpdateKeys.Visitor3Percent:
							prd.BPercent3 = (decimal)detail.Value;
							break;
						case EnProductUpdateKeys.Visitor4Percent:
							prd.BPercent4 = (decimal)detail.Value;
							break;
						case EnProductUpdateKeys.DiscountPercent:
							prd.DiscountPercent = (decimal)detail.Value;
							break;
						case EnProductUpdateKeys.DiscountStartDate:
							prd.DiscountStartDate = (DateTime?)detail.Value;
							break;
						case EnProductUpdateKeys.DiscountEndDate:
							prd.DiscountEndDate = (DateTime?)detail.Value;
							break;
						case EnProductUpdateKeys.Special:
							prd.Special = (bool)detail.Value;
							break;
						case EnProductUpdateKeys.TaxClassGuid:
							prd.TaxGuid = ((detail.Value == null) ? null : ((Guid?)detail.Value));
							prd.IsTax = detail.Value != null;
							break;
						case EnProductUpdateKeys.GroupGuid:
						{
							List<ProductGroupRelationBusiness> lst = (List<ProductGroupRelationBusiness>)detail.Value;
							if (!(lst?.Any() ?? false))
							{
								break;
							}
							List<ProductGroupRelation> list = await EntityFrameworkQueryableExtensions.ToListAsync<ProductGroupRelation>(((IQueryable<ProductGroupRelation>)c.PrdGroupRelation).Where((ProductGroupRelation q) => q.ProductGuid == item.ProductGuid), token);
							if (list != null && list.Any())
							{
								((DbContext)c).RemoveRange((IEnumerable<object>)list);
							}
							foreach (ProductGroupRelationBusiness item2 in lst)
							{
								item2.ProductGuid = item.ProductGuid;
							}
							await ((DbContext)c).AddRangeAsync((IEnumerable<object>)(await _groupRepo.MapToDatabase.MapListAsync(base.ContextGuid, lst, null, tranName, token)), token);
							break;
						}
						case EnProductUpdateKeys.TechnicalInfo:
							if (detail.Value != null && !string.IsNullOrEmpty(detail.Value.ToString()))
							{
								ProductTechnicalInfoUpdateModel value = (ProductTechnicalInfoUpdateModel)detail.Value;
								PrdTechnical prdTechnical = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<PrdTechnical>(((IQueryable<PrdTechnical>)c.PrdTechnicals).Where((PrdTechnical q) => q.ProductGuid == prd.Guid && q.TechnicalType == value.Ordinal), token);
								if (prdTechnical == null)
								{
									prdTechnical = new PrdTechnical
									{
										TechnicalType = value.Ordinal,
										Code = "",
										Guid = Guid.NewGuid(),
										Modified = DateTime.Now,
										ProductGuid = prd.Guid,
										Status = true
									};
								}
								prdTechnical.Name = value.TechnicalInfoTitle;
								ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(prdTechnical, prdTechnical.ProductGuid, prdTechnical.TechnicalType), ReturnedState.None, "UpdateProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 987);
							}
							break;
						default:
							throw new NotImplementedException();
						}
					}
					c.Products.Update(prd);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 998);
				res.AddReturnedValue(ex, "", "UpdateProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 999);
			}
			return res;
		}
	}

	public async Task<ReturnedSaveFuncInfo> UpdateSpecialAsync(Guid groupGuid, bool status, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("UpdateSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				if (groupGuid == Guid.Empty)
				{
					return res;
				}
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 1010));
				List<Guid> groupGuidList = await GetGroupGuidListAsync(groupGuid, c, token);
				List<ProductGroupRelation> list = await EntityFrameworkQueryableExtensions.ToListAsync<ProductGroupRelation>(((IQueryable<ProductGroupRelation>)c.PrdGroupRelation).Where((ProductGroupRelation q) => groupGuidList.Contains(q.GroupGuid) && q.Ordinal == 1), token);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return res.AddErrorTokenIsCancelled("", "", "UpdateSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 1014);
				}
				foreach (ProductGroupRelation item in list)
				{
					if (IsDisposed || token.IsCancellationRequested)
					{
						return res.AddErrorTokenIsCancelled("", "", "UpdateSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 1017);
					}
					Product product = await c.Products.FindAsync(new object[1] { item.ProductGuid });
					if (product == null)
					{
						return res;
					}
					product.ServerStatus = ServerStatusForClient.None;
					product.Modified = DateTime.Now;
					product.Special = status;
					c.Products.Update(product);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 1028);
				res.AddReturnedValue(ex, "", "UpdateSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 1029);
			}
			return res;
		}
	}

	private async Task<List<Guid>> GetGroupGuidListAsync(Guid groupGuid, Acc_ModelContext context, CancellationToken token)
	{
		try
		{
			List<Guid> groupGuidList = new List<Guid> { groupGuid };
			List<ProductGroup> list = await EntityFrameworkQueryableExtensions.ToListAsync<ProductGroup>(((IQueryable<ProductGroup>)context.Products_group).Where((ProductGroup q) => q.ParentGuid == groupGuid), token);
			if (list != null && list.Any())
			{
				groupGuidList.AddRange(list.Select((ProductGroup q) => q.Guid));
				foreach (ProductGroup item in list)
				{
					if (item != null)
					{
						List<Guid> list2 = groupGuidList;
						list2.AddRange(await GetGroupGuidListAsync(item.Guid, context, token));
					}
				}
			}
			return groupGuidList.Distinct().ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetGroupGuidListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductPersistenceRepositoryBase.cs", 1053);
			throw;
		}
	}

	public virtual Task<List<ProductAttributeAmountModel>> GetAllAttributeAmountAsync(Guid productGuid, Guid rowAttributeGuid, Guid columnAttributeGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
