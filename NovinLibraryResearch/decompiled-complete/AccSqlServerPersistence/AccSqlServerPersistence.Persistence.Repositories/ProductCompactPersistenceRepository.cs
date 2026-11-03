using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Selection;
using EntityCache.Business.Global;
using EntityCache.Business.Products;
using EntityCache.Filters;
using EntityCache.Models.Accounting.Prd;
using Microsoft.Data.SqlClient;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Product;
using PacketParser.Entities;
using PacketParser.Filters.Configs;
using PacketParser.Records;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class ProductCompactPersistenceRepository : ProductCompactPersistenceRepositoryBase
{
	public ProductCompactPersistenceRepository(ContextHolder dbContext)
		: base(dbContext)
	{
	}

	public override async Task<List<ProductCompactBusiness>> GetAllAsync(Guid? depotGuid, Guid? guid, Document Type, CancellationToken token, ProductCompactFilter filter, string tranName)
	{
		string startWord = "";
		string endWord = "";
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs", 33));
			List<ProductCompactBusiness> list = new List<ProductCompactBusiness>();
			try
			{
				int num;
				_ = num - 1;
				_ = 5;
				List<ProductCompactBusiness> result;
				try
				{
					if (filter.GroupGuids != null && filter.GroupGuids.Contains(Guid.Empty))
					{
						throw new Exception("Group guids contais Empty value \r\n" + string.Join(",", filter.GroupGuids));
					}
					string searchText = "";
					if (!string.IsNullOrEmpty(filter.SearchText))
					{
						List<string> list2 = filter.SearchText.SplitString(" ");
						int num2 = 0;
						int count = list2.Count;
						foreach (string item in filter.SearchText.SplitString(" \\\r\n\t)(-_=+$#@*|><,`~'\":.?؟،;").Distinct())
						{
							if (!string.IsNullOrEmpty(item))
							{
								num2++;
								searchText = ((!string.IsNullOrEmpty(searchText)) ? (searchText + " " + GetWord(item, filter.SearchLocation)) : (GetWord(item, filter.SearchLocation) ?? ""));
								if (num2 == 1 && filter.SearchLocation.GetValueOrDefault() == SearchLocation.Start)
								{
									startWord = item + "%";
								}
								else if (num2 == count && filter.SearchLocation.GetValueOrDefault() == SearchLocation.End)
								{
									endWord = "%" + item;
								}
							}
						}
					}
					int groupRefId = 0;
					int cashierAccessGroupRefId = 0;
					if (filter.GroupGuids?.Any() ?? false)
					{
						ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = await StimulQueryBusiness.SaveAsync(base.ContextGuid, filter.GroupGuids, token);
						if (!returnedSaveFuncInfoWithValue.HasError)
						{
							groupRefId = returnedSaveFuncInfoWithValue.value;
						}
					}
					if (filter.CashierAccessGroupGuids?.Any() ?? false)
					{
						ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue2 = await StimulQueryBusiness.SaveAsync(base.ContextGuid, filter.CashierAccessGroupGuids, token);
						if (!returnedSaveFuncInfoWithValue2.HasError)
						{
							cashierAccessGroupRefId = returnedSaveFuncInfoWithValue2.value;
						}
					}
					else
					{
						cashierAccessGroupRefId = -1;
					}
					List<SqlParameter> param = new List<SqlParameter>
					{
						new SqlParameter("@SearchString", (object)searchText),
						new SqlParameter("@SearchOption", (object)(short)filter.ProductSearchOption.GetValueOrDefault(ProductSearchOption.None)),
						new SqlParameter("@Groups", (object)groupRefId),
						new SqlParameter("@StartWord", (object)startWord),
						new SqlParameter("@EndWord", (object)endWord),
						new SqlParameter("@isLoadJustParent", (object)(filter?.LoadJustParents ?? true)),
						new SqlParameter("@CashierAccessGroups", (object)cashierAccessGroupRefId)
					};
					if (depotGuid.HasValue)
					{
						param.Add(new SqlParameter("@depotGuid", (object)depotGuid));
					}
					if (guid.HasValue)
					{
						param.Add(new SqlParameter("@guid", (object)guid));
					}
					if (filter.Status.HasValue)
					{
						param.Add(new SqlParameter("@status", (object)filter.Status.Value));
					}
					if (filter.IsTax.HasValue)
					{
						param.Add(new SqlParameter("@isTax", (object)filter.IsTax.Value));
					}
					if ((filter?.MasterGuid ?? Guid.Empty) != Guid.Empty)
					{
						param.Add(new SqlParameter("@masterGuid", (object)filter.MasterGuid.Value));
					}
					if ((filter?.MaxCount).GetValueOrDefault() > 0)
					{
						param.Add(new SqlParameter("@maxCount", (object)filter.MaxCount.Value));
					}
					if (filter?.CreationDate.HasValue ?? false)
					{
						param.Add(new SqlParameter("@CreationDate", (object)filter.CreationDate.Value));
					}
					if (filter?.HasDiscount.HasValue ?? false)
					{
						param.Add(new SqlParameter("@HasDiscount", (object)filter.HasDiscount.Value));
					}
					if ((filter?.TaxClassGuid ?? Guid.Empty) != Guid.Empty)
					{
						param.Add(new SqlParameter("@TaxClassGuid", (object)filter.TaxClassGuid.Value));
					}
					if (filter.IsSpecial.HasValue)
					{
						param.Add(new SqlParameter("@IsSpecial", (object)filter.IsSpecial.Value));
					}
					CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(base.ContextGuid, token, tranName);
					if (token.IsCancellationRequested)
					{
						result = list;
					}
					else
					{
						using DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_Product_SelectAll", param, tranName, token, CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs");
						if (token.IsCancellationRequested)
						{
							result = list;
						}
						else
						{
							while (true)
							{
								if (await dr.ReadAsync(token))
								{
									if (token.IsCancellationRequested)
									{
										result = null;
										break;
									}
									ProductCompactBusiness productCompactBusiness = LoadDataFromReader(dr, base.ContextGuid, isLoadDets: true, (cashier?.Settings?.Products?.ShowNameForDisplayAgainstName).GetValueOrDefault());
									if (filter?.Order != null)
									{
										productCompactBusiness.Order = filter.Order;
									}
									list.Add(productCompactBusiness);
									continue;
								}
								if (filter.LoadBarcodeInsteadOfCode)
								{
									List<BarcodeBusiness> list3 = (from p in await BarcodeBusiness.GetAllAsync(base.ContextGuid, token, tranName)
										group p by p.PrdGuid into p
										select p.OrderBy((BarcodeBusiness x) => x.Ordinal).FirstOrDefault()).ToList();
									if (list3 != null && list3.Any() && (list?.Any() ?? false))
									{
										List<Guid> prdGuids = list.Select((ProductCompactBusiness q) => q.Guid).ToList();
										list3 = list3.Where((BarcodeBusiness q) => prdGuids.Contains(q.PrdGuid))?.ToList();
										if (list3 != null && list3.Any())
										{
											foreach (BarcodeBusiness barcode in list3)
											{
												ProductCompactBusiness productCompactBusiness2 = list.SingleOrDefault((ProductCompactBusiness p) => p.Guid == barcode.PrdGuid);
												if (productCompactBusiness2 != null && !string.IsNullOrEmpty(barcode?.BarCode))
												{
													productCompactBusiness2.IdProduct = barcode.BarCode;
													productCompactBusiness2.CodeForDisplay = productCompactBusiness2.IdProduct;
												}
											}
										}
									}
								}
								if (token.IsCancellationRequested)
								{
									result = null;
									break;
								}
								if (filter?.HasAmount.HasValue ?? false)
								{
									list = ((!filter.HasAmount.Value) ? list?.Where((ProductCompactBusiness q) => q.Amount <= 0m)?.ToList() : list?.Where((ProductCompactBusiness q) => q.Amount > 0m)?.ToList());
								}
								if (token.IsCancellationRequested)
								{
									result = null;
									break;
								}
								if ((filter?.UnderOrderPoint).GetValueOrDefault())
								{
									list = list?.Where((ProductCompactBusiness q) => q.Amount < q.OrderPoint)?.ToList();
								}
								if (token.IsCancellationRequested)
								{
									result = null;
									break;
								}
								if ((filter?.KindList?.Any()).GetValueOrDefault())
								{
									list = list?.Where((ProductCompactBusiness q) => filter.KindList.Contains(q.Kind))?.ToList();
								}
								if (token.IsCancellationRequested)
								{
									result = null;
									break;
								}
								if (filter.HasOrderQuantity.GetValueOrDefault())
								{
									List<Guid> productGuids = filter?.Order?.ListKala?.Where((OrderDetailBusiness q) => q.Quantity > 0m)?.Select((OrderDetailBusiness q) => q.ProductGuid)?.ToList();
									if (productGuids?.Any() ?? false)
									{
										list = list?.Where((ProductCompactBusiness q) => productGuids.Contains(q.Guid))?.ToList();
									}
								}
								if (token.IsCancellationRequested)
								{
									result = null;
									break;
								}
								if (filter.Sort.HasValue)
								{
									ProductSort? sort = filter.Sort;
									if (sort.HasValue)
									{
										switch (sort.GetValueOrDefault())
										{
										case ProductSort.AmontAsc:
											result = list?.OrderBy((ProductCompactBusiness q) => q.Amount)?.ToList();
											goto end_IL_0b39;
										case ProductSort.AmontDesc:
											result = list?.OrderByDescending((ProductCompactBusiness q) => q.Amount)?.ToList();
											goto end_IL_0b39;
										case ProductSort.CodeAsc:
											result = list?.OrderBy((ProductCompactBusiness q) => q.Code)?.ToList();
											goto end_IL_0b39;
										case ProductSort.CodeDesc:
											result = list?.OrderByDescending((ProductCompactBusiness q) => q.Code)?.ToList();
											goto end_IL_0b39;
										case ProductSort.NameAsc:
											result = list?.OrderBy((ProductCompactBusiness q) => q.Name)?.ToList();
											goto end_IL_0b39;
										case ProductSort.NameDesc:
											result = list?.OrderByDescending((ProductCompactBusiness q) => q.Name)?.ToList();
											goto end_IL_0b39;
										case ProductSort.PriceAsc:
											result = list?.OrderBy((ProductCompactBusiness q) => q.Sell1)?.ToList();
											goto end_IL_0b39;
										case ProductSort.PriceDesc:
											result = list?.OrderByDescending((ProductCompactBusiness q) => q.Sell1)?.ToList();
											goto end_IL_0b39;
										}
									}
								}
								goto end_IL_0126;
								continue;
								end_IL_0b39:
								break;
							}
						}
					}
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (SqlException val)
				{
					SqlException val2 = val;
					if (((Exception)(object)val2).Message.Contains("Operation cancelled by user"))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog((Exception)(object)val2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs", 168);
					throw;
				}
				catch (SqlException val3)
				{
					SqlException val4 = val3;
					if (((Exception)(object)val4).Message.Contains("Operation cancelled by user"))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog((Exception)(object)val4, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs", 177);
					throw;
				}
				catch (Exception ex2)
				{
					if (!token.IsCancellationRequested)
					{
						WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs", 185);
						throw;
					}
					goto end_IL_0126;
				}
				return result;
				end_IL_0126:;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs");
				}
			}
			return list;
		}
	}

	private string GetWord(string word, SearchLocation? searchLocation)
	{
		try
		{
			if (searchLocation.GetValueOrDefault() == SearchLocation.Start)
			{
				return word + "%";
			}
			if (searchLocation.GetValueOrDefault() == SearchLocation.Equal)
			{
				return word;
			}
			if (searchLocation.GetValueOrDefault() == SearchLocation.End)
			{
				return "%" + word;
			}
			return "%" + word + "%";
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetWord", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs", 209);
			throw;
		}
	}

	private ProductCompactBusiness LoadDataFromReader(DbDataReader dr, string contextGuid, bool isLoadDets = true, bool isLoadNameForDisplay = false)
	{
		ProductCompactBusiness productCompactBusiness = new ProductCompactBusiness(contextGuid);
		try
		{
			if (dr["Guid"] != DBNull.Value)
			{
				productCompactBusiness.Guid = (Guid)dr["Guid"];
			}
			if (dr["Modified"] != DBNull.Value)
			{
				productCompactBusiness.Modified = (DateTime)dr["Modified"];
			}
			if (dr["IdProduct"] != DBNull.Value)
			{
				productCompactBusiness.IdProduct = dr["IdProduct"].ToString();
			}
			if (dr["Code"] != DBNull.Value)
			{
				productCompactBusiness.Code = (long)dr["Code"];
				productCompactBusiness.CodeForDisplay = productCompactBusiness.Code;
			}
			if (dr["Description"] != DBNull.Value)
			{
				productCompactBusiness.Description = dr["Description"].ToString();
			}
			if (dr["DiscountPercent"] != DBNull.Value)
			{
				productCompactBusiness.DiscountPercent = (decimal)dr["DiscountPercent"];
			}
			if (dr["IsTax"] != DBNull.Value)
			{
				productCompactBusiness.IsTax = (bool)dr["IsTax"];
			}
			if (dr["Type"] != DBNull.Value)
			{
				productCompactBusiness.Type = (EnProductType)dr["Type"];
			}
			if (dr["Name"] != DBNull.Value)
			{
				productCompactBusiness.Name = dr["Name"].ToString();
			}
			if (dr["Pic"] != DBNull.Value)
			{
				productCompactBusiness.Pic = dr["Pic"].ToString();
			}
			if (dr["Special"] != DBNull.Value)
			{
				productCompactBusiness.Special = (bool)dr["Special"];
			}
			if (dr["Status"] != DBNull.Value)
			{
				productCompactBusiness.Status = (bool)dr["Status"];
			}
			if (dr["V2Qt"] != DBNull.Value)
			{
				productCompactBusiness.V2Qt = dr["V2Qt"].ToString().ParseToDecimal(0m, 3);
			}
			if (dr["GuidVahed"] != DBNull.Value)
			{
				productCompactBusiness.GuidVahed = (Guid)dr["GuidVahed"];
			}
			if (dr["V2Guid"] != DBNull.Value)
			{
				productCompactBusiness.V2Guid = (Guid)dr["V2Guid"];
			}
			if (dr["ImageList"] != DBNull.Value)
			{
				productCompactBusiness.ImageListData = dr["ImageList"].ToString().SplitString(",");
			}
			if (dr["OrderPoint"] != DBNull.Value)
			{
				productCompactBusiness.OrderPoint = dr["OrderPoint"].ToString().ParseToDecimal(0m, 3);
			}
			if (dr["ServerStatus"] != DBNull.Value)
			{
				productCompactBusiness.ServerStatus = (ServerStatusForClient)(short)dr["ServerStatus"];
			}
			if (dr["Kind"] != DBNull.Value)
			{
				productCompactBusiness.Kind = (EnProductKind)(short)dr["Kind"];
			}
			if (dr["MasterGuid"] != DBNull.Value)
			{
				productCompactBusiness.MasterGuid = (Guid)dr["MasterGuid"];
			}
			if (dr["TaxGuid"] != DBNull.Value)
			{
				productCompactBusiness.TaxGuid = (Guid)dr["TaxGuid"];
			}
			if (dr["DiscountStartDate"] != DBNull.Value)
			{
				productCompactBusiness.DiscountStartDate = (DateTime?)dr["DiscountStartDate"];
			}
			if (dr["DiscountEndDate"] != DBNull.Value)
			{
				productCompactBusiness.DiscountEndDate = (DateTime?)dr["DiscountEndDate"];
			}
			if (dr["DiscountPrice"] != DBNull.Value)
			{
				productCompactBusiness.DiscountPrice = (decimal)dr["DiscountPrice"];
			}
			if (isLoadDets)
			{
				if (dr["Amount"] != DBNull.Value)
				{
					productCompactBusiness.Amount = dr["Amount"].ToString().ParseToDecimal(0m, 3);
				}
				if (dr["VahedName"] != DBNull.Value)
				{
					productCompactBusiness.VahedName = dr["VahedName"].ToString();
				}
				if (dr["Weight"] != DBNull.Value)
				{
					productCompactBusiness.Weight = dr["Weight"].ToString().ParseToDecimal(0m, 3);
				}
				if (dr["GroupName"] != DBNull.Value)
				{
					productCompactBusiness.GroupName = dr["GroupName"].ToString();
				}
				if (dr["GroupShortName"] != DBNull.Value)
				{
					productCompactBusiness.GroupShortName = dr["GroupShortName"].ToString();
				}
				if (dr["Sell1"] != DBNull.Value)
				{
					productCompactBusiness.Sell1 = (decimal)dr["Sell1"];
				}
				if (dr["Sell2"] != DBNull.Value)
				{
					productCompactBusiness.Sell2 = (decimal)dr["Sell2"];
				}
				if (dr["IsDependentOnCurrency"] != DBNull.Value)
				{
					productCompactBusiness.IsDependentOnCurrency = (bool)dr["IsDependentOnCurrency"];
				}
			}
			if (isLoadNameForDisplay && dr["NameForDisplay"] != DBNull.Value)
			{
				string text = dr["NameForDisplay"].ToString();
				if (!string.IsNullOrEmpty(text))
				{
					productCompactBusiness.Name = text;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadDataFromReader", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs", 267);
			throw;
		}
		return productCompactBusiness;
	}

	public override async Task<IEnumerable<ProductCompactBusiness>> GetAllEmptySkuAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllEmptySkuAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs", 274));
		List<ProductCompactBusiness> list = new List<ProductCompactBusiness>();
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				using DbDataReader dr = await c.ExecuteReaderAsync("Select * from Acc_Product where Sku=CAST(Guid As nvarchar(100))", null, tranName, token, CommandType.Text, 30, "GetAllEmptySkuAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					list.Add(LoadDataFromReader(dr, base.ContextGuid, isLoadDets: false));
				}
				dr.Close();
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllEmptySkuAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs", 288);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllEmptySkuAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs");
			}
		}
		return list;
	}

	public override async Task<IEnumerable<ProductCompactBusiness>> GetAllEmptySlugAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllEmptySlugAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs", 300));
		List<ProductCompactBusiness> list = new List<ProductCompactBusiness>();
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				using DbDataReader dr = await c.ExecuteReaderAsync("Select * from Acc_Product where Slug=CAST(Guid As nvarchar(100))", null, tranName, token, CommandType.Text, 30, "GetAllEmptySlugAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					list.Add(LoadDataFromReader(dr, base.ContextGuid, isLoadDets: false));
				}
				dr.Close();
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllEmptySlugAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs", 314);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllEmptySlugAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs");
			}
		}
		return list;
	}

	public override async Task<IEnumerable<ProductPriceReportBusiness>> GetAllProductPriceModelAsync(ProductPriceReportConfig config, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllProductPriceModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs", 326));
		IEnumerable<ProductPriceReportBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				if (config == null)
				{
					throw new ArgumentException("Config Is Null !!");
				}
				List<PriceRelationModel> list = new List<PriceRelationModel>();
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@RefrenceId", (object)config.RefrencedId),
					new SqlParameter("@Meyar_Kardex", (object)config.IsKardexMeyar)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Products_Discount", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllProductPriceModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs");
				while (true)
				{
					if (await dr.ReadAsync(token))
					{
						if (base.IsDisposed || token.IsCancellationRequested)
						{
							result = null;
							break;
						}
						list.Add(LoadDataFromReader(dr));
						continue;
					}
					list = list?.Where((PriceRelationModel q) => q.ProductGuid != Guid.Empty)?.ToList();
					if (!(list?.Any() ?? false) || base.IsDisposed || token.IsCancellationRequested)
					{
						result = null;
						break;
					}
					IEnumerable<ProductCompactBusiness> enumerable = await ProductCompactBusiness.GetAllAsync(base.ContextGuid, EnSystem.UserInterface, new ProductCompactFilter
					{
						LoadJustParents = false,
						Status = true
					});
					if (enumerable == null || !enumerable.Any() || base.IsDisposed || token.IsCancellationRequested)
					{
						result = null;
						break;
					}
					var inner = from q in list
						group q by q.ProductGuid into q
						select new
						{
							ProductGuid = q.Key,
							Prices = q.ToList()
						};
					IEnumerable<ProductPriceReportContainer> enumerable2 = from prd in enumerable
						join price in inner on prd.Guid equals price.ProductGuid
						select new ProductPriceReportContainer
						{
							Product = prd,
							PriceRelationList = price?.Prices
						};
					result = ((enumerable2 != null && enumerable2.Any() && !base.IsDisposed && !token.IsCancellationRequested) ? enumerable2?.Select((ProductPriceReportContainer q) => new ProductPriceReportBusiness(base.ContextGuid, q, config))?.ToList() : null);
					break;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllProductPriceModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs", 364);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllProductPriceModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs");
			}
		}
		return result;
	}

	private PriceRelationModel LoadDataFromReader(DbDataReader dr)
	{
		PriceRelationModel priceRelationModel = new PriceRelationModel();
		try
		{
			if (dr["Guid"] != DBNull.Value)
			{
				priceRelationModel.Guid = (Guid)dr["Guid"];
			}
			if (dr["ProductGuid"] != DBNull.Value)
			{
				priceRelationModel.ProductGuid = (Guid)dr["ProductGuid"];
			}
			if (dr["LevelGuid"] != DBNull.Value)
			{
				priceRelationModel.PriceLevelGuid = (Guid)dr["LevelGuid"];
			}
			if (dr["Price"] != DBNull.Value)
			{
				priceRelationModel.Price = (decimal)dr["Price"];
			}
			if (dr["Quantity"] != DBNull.Value)
			{
				priceRelationModel.Quantity = (decimal)dr["Quantity"];
			}
			if (dr["Profit"] != DBNull.Value)
			{
				priceRelationModel.Profit = (decimal)dr["Profit"];
			}
			if (dr["KasrRial"] != DBNull.Value)
			{
				priceRelationModel.KasrRial = (decimal)dr["KasrRial"];
			}
			if (dr["KardexPrice"] != DBNull.Value)
			{
				priceRelationModel.KardexPrice = (decimal)dr["KardexPrice"];
			}
			if (dr["Ordinal"] != DBNull.Value)
			{
				priceRelationModel.Ordinal = (short)dr["Ordinal"];
			}
			return priceRelationModel;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadDataFromReader", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs", 393);
			throw;
		}
	}

	public override async Task<ProductCompactBusiness> GetAsync(Guid? depotGuid, string code, bool? status, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs", 400));
			ProductCompactBusiness res = null;
			try
			{
				int num;
				_ = num - 1;
				_ = 3;
				ProductCompactBusiness result;
				try
				{
					List<SqlParameter> list = new List<SqlParameter>
					{
						new SqlParameter("@code", (object)(code?.FixString()?.TrimStart(Array.Empty<char>())?.TrimEnd(Array.Empty<char>()) ?? ""))
					};
					if ((depotGuid ?? Guid.Empty) != Guid.Empty)
					{
						list.Add(new SqlParameter("@depotGuid", (object)depotGuid.Value));
					}
					if (status.HasValue)
					{
						list.Add(new SqlParameter("@status", (object)status.Value));
					}
					using (DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_Product_GetByCode", list, tranName, token, CommandType.StoredProcedure, 30, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs"))
					{
						if (await dr.ReadAsync())
						{
							res = LoadDataFromReader(dr, base.ContextGuid);
						}
						dr.Close();
						if (!token.IsCancellationRequested && !base.IsDisposed && res != null)
						{
							if (((await LocalSettingBusiness.GetSettingAsync(base.ContextGuid, token))?.Products?.LoadBarcodeInsteadOfCode).GetValueOrDefault())
							{
								List<BarcodeBusiness> list2 = await BarcodeBusiness.GetByPrdGuidAsync(base.ContextGuid, new List<Guid> { res.Guid }, token, tranName);
								if (list2 != null && list2.Any())
								{
									BarcodeBusiness barcodeBusiness = list2.OrderBy((BarcodeBusiness q) => q.Ordinal)?.FirstOrDefault();
									if (barcodeBusiness != null)
									{
										res.IdProduct = barcodeBusiness.BarCode;
										res.CodeForDisplay = barcodeBusiness.BarCode;
									}
								}
							}
							goto end_IL_02ce;
						}
						result = res;
						goto end_IL_0102;
						end_IL_02ce:;
					}
					goto end_IL_00eb;
					end_IL_0102:;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs", 434);
					throw;
				}
				return result;
				end_IL_00eb:;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs");
				}
			}
			return res;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> UpdateGroupOfProductsAsync(List<ProductGroupRelationBusiness> originalList, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("UpdateGroupOfProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateGroupOfProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs", 448));
			try
			{
				if (!(originalList?.Any() ?? false))
				{
					return ret;
				}
				IEnumerable<StimulQueryComlpexRecord> lst = originalList.Select((ProductGroupRelationBusiness q) => new StimulQueryComlpexRecord
				{
					Guid = q.ProductGuid,
					Id = q.Ordinal,
					SecondKey = q.GroupGuid.ToString()
				});
				ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = await StimulQueryBusiness.SaveAsync(base.ContextGuid, lst, token);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "UpdateGroupOfProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs", 460);
				if (ret.HasError)
				{
					return ret;
				}
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@RefId", (object)returnedSaveFuncInfoWithValue.value)
				};
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_acc_Product_UpdateGroups", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "UpdateGroupOfProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs"), ReturnedState.None, "UpdateGroupOfProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs", 464);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateGroupOfProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs", 468);
				ret.AddReturnedValue(ex, "", "UpdateGroupOfProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs", 469);
			}
			return ret;
		}
	}

	public override async Task<IEnumerable<ProductCompactBusiness>> GetAllEmptySkuOrSlugAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllEmptySkuOrSlugAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs", 475));
		List<ProductCompactBusiness> list = new List<ProductCompactBusiness>();
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				using DbDataReader dr = await c.ExecuteReaderAsync("Select * from Acc_Product where Slug=CAST(Guid As nvarchar(100)) Or Sku=CAST(Guid As nvarchar(100))", null, tranName, token, CommandType.Text, 30, "GetAllEmptySkuOrSlugAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					list.Add(LoadDataFromReader(dr, base.ContextGuid, isLoadDets: false));
				}
				dr.Close();
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllEmptySkuOrSlugAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs", 489);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllEmptySkuOrSlugAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductCompactPersistenceRepository.cs");
			}
		}
		return list;
	}

	public override Task<List<ProductMicroBusiness>> GetAllAsync(ProductCompactFilter filter, Guid? guid, Guid? depotGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
