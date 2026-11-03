using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using EntityCache;
using EntityCache.Business.Global;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.BusinessLayer.Models.Product;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class ProductPersistenceRepository : ProductPersistenceRepositoryBase
{
	public ProductPersistenceRepository(ContextHolder dbContect)
		: base(dbContect)
	{
	}

	public override async Task<ReturnedSaveFuncInfo> ChangeTaxPercentAsync(IEnumerable<Guid> productGuidList, Guid? taxGuid, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "ChangeTaxPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs", 25);
		try
		{
			if ((taxGuid ?? Guid.Empty) == Guid.Empty)
			{
				taxGuid = null;
			}
			ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = await StimulQueryBusiness.SaveAsync(base.ContextGuid, productGuidList);
			res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "ChangeTaxPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs", 30);
			if (res.HasError)
			{
				return res;
			}
			List<SqlParameter> list = new List<SqlParameter>
			{
				new SqlParameter("@RefrenceId", (object)returnedSaveFuncInfoWithValue.value)
			};
			if ((taxGuid ?? Guid.Empty) != Guid.Empty)
			{
				list.Add(new SqlParameter("@TaxGuid", (object)taxGuid));
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_Acc_Product_UpdateTaxPercent", list, tranName, token, "", CommandType.StoredProcedure, 30, "ChangeTaxPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs"), ReturnedState.None, "ChangeTaxPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs", 34);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChangeTaxPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs", 38);
			res.AddReturnedValue(ex, "", "ChangeTaxPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs", 39);
		}
		return res;
	}

	public override async Task<ReturnedSaveFuncInfo> ReNewAllCodesAsync(CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext baseModelContext = await dbContext.GetContextAsync(tranName, 60000, "ReNewAllCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs", 46);
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await baseModelContext.ExecuteNoneQueryAsync("sp_Acc_Product_ReNewAllCodes", null, tranName, token, "", CommandType.StoredProcedure, 30, "ReNewAllCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs"), ReturnedState.None, "ReNewAllCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs", 49);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ReNewAllCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs", 53);
			res.AddReturnedValue(ex, "", "ReNewAllCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs", 54);
		}
		return res;
	}

	public override async Task<ReturnedSaveFuncInfo> ReNewEmptyCodesAsync(CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext baseModelContext = await dbContext.GetContextAsync(tranName, 60000, "ReNewEmptyCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs", 61);
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await baseModelContext.ExecuteNoneQueryAsync("sp_Acc_Product_ReNewEmptyCodes", null, tranName, token, "", CommandType.StoredProcedure, 30, "ReNewEmptyCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs"), ReturnedState.None, "ReNewEmptyCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs", 64);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ReNewEmptyCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs", 68);
			res.AddReturnedValue(ex, "", "ReNewEmptyCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs", 69);
		}
		return res;
	}

	public override async Task<ReturnedSaveFuncInfo> UpdateRegularPriceAsync(Guid priceLevelGuid, Guid regularPriceGuid, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext baseModelContext = await dbContext.GetContextAsync(tranName, 60000, "UpdateRegularPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs", 76);
		try
		{
			if (priceLevelGuid == Guid.Empty || regularPriceGuid == Guid.Empty)
			{
				return res;
			}
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@PriceLevelGuid", (object)priceLevelGuid),
				new SqlParameter("@RegularPriceGuid", (object)regularPriceGuid)
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await baseModelContext.ExecuteNoneQueryAsync("sp_Acc_ProductPriceRelation_UpdateRegularPrice", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "UpdateRegularPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs"), ReturnedState.None, "UpdateRegularPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs", 85);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateRegularPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs", 89);
			res.AddReturnedValue(ex, "", "UpdateRegularPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs", 90);
		}
		return res;
	}

	public override async Task<IEnumerable<ProductDiscountModel>> GetAllDiscountModelAsync(CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllDiscountModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs", 96);
		IEnumerable<ProductDiscountModel> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<ProductDiscountModel> list = new List<ProductDiscountModel>();
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_Product_GetDiscountModel", null, tranName, token, CommandType.StoredProcedure, 30, "GetAllDiscountModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs");
				while (true)
				{
					if (await dr.ReadAsync(token))
					{
						list.Add(LoadData(dr));
						if (base.IsDisposed || token.IsCancellationRequested)
						{
							result = null;
							break;
						}
						continue;
					}
					dr.Close();
					result = list;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllDiscountModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs", 113);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllDiscountModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<ReturnedSaveFuncInfo> UpdateMasterProductAmountAsync(Guid? guid, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext baseModelContext = await dbContext.GetContextAsync(tranName, 60000, "UpdateMasterProductAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs", 125);
		try
		{
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@DigitForRound", (object)ClsCache.GetCache(base.ContextGuid).MultiLanguageConfig.DigitForRound)
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await baseModelContext.ExecuteNoneQueryAsync("sp_Acc_Product_FixMasterAmount", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "UpdateMasterProductAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs"), ReturnedState.None, "UpdateMasterProductAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs", 129);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateMasterProductAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs", 133);
			res.AddReturnedValue(ex, "", "UpdateMasterProductAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs", 134);
		}
		return res;
	}

	public override async Task<List<ProductAttributeAmountModel>> GetAllAttributeAmountAsync(Guid productGuid, Guid rowAttributeGuid, Guid columnAttributeGuid, CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllAttributeAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs", 140);
		List<ProductAttributeAmountModel> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				if (productGuid == Guid.Empty || rowAttributeGuid == Guid.Empty || columnAttributeGuid == Guid.Empty)
				{
					result = null;
				}
				else
				{
					List<ProductAttributeAmountModel> list = new List<ProductAttributeAmountModel>();
					List<SqlParameter> parameters = new List<SqlParameter>
					{
						new SqlParameter("@MasterGuid", (object)productGuid),
						new SqlParameter("@RowAttr", (object)rowAttributeGuid),
						new SqlParameter("@ColAttr", (object)columnAttributeGuid)
					};
					using DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_Product_SelectAttributeAmounts", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllAttributeAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs");
					while (true)
					{
						if (await dr.ReadAsync(token))
						{
							list.Add(LoadDataProductAttributeAmountModel(dr));
							if (base.IsDisposed || token.IsCancellationRequested)
							{
								result = null;
								break;
							}
							continue;
						}
						result = list?.OrderBy((ProductAttributeAmountModel q) => q.RowAttributeValue)?.ToList();
						break;
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAttributeAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs", 161);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAttributeAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs");
			}
		}
		return result;
	}

	private ProductDiscountModel LoadData(DbDataReader dr)
	{
		try
		{
			ProductDiscountModel productDiscountModel = new ProductDiscountModel();
			if (dr["Guid"] != DBNull.Value)
			{
				productDiscountModel.Guid = (Guid)dr["Guid"];
			}
			if (dr["IdProduct"] != DBNull.Value)
			{
				productDiscountModel.IdProduct = dr["IdProduct"].ToString();
			}
			if (dr["name"] != DBNull.Value)
			{
				productDiscountModel.Name = dr["name"].ToString();
			}
			if (dr["KardexPrice"] != DBNull.Value)
			{
				productDiscountModel.KardexPrice = (decimal)dr["KardexPrice"];
			}
			if (dr["Price"] != DBNull.Value)
			{
				productDiscountModel.Sell1 = (decimal)dr["Price"];
			}
			if (dr["DiscountPercent"] != DBNull.Value)
			{
				productDiscountModel.DiscountPercent = dr["DiscountPercent"].ToString().ParseToDecimal(0m, 3);
			}
			if (dr["Profit"] != DBNull.Value)
			{
				productDiscountModel.Prefit = (decimal)dr["Profit"];
			}
			if (dr["UnitName"] != DBNull.Value)
			{
				productDiscountModel.VahedName = dr["UnitName"].ToString();
			}
			return productDiscountModel;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs", 190);
			throw;
		}
	}

	private ProductAttributeAmountModel LoadDataProductAttributeAmountModel(DbDataReader dr)
	{
		try
		{
			List<string> list = new List<string>();
			for (int i = 1; i < dr.FieldCount; i++)
			{
				list.Add(dr.GetName(i));
			}
			ProductAttributeAmountModel productAttributeAmountModel = new ProductAttributeAmountModel
			{
				ColumnValues = new List<ProductAttributeAmountDetailModel>()
			};
			if (dr[0] != DBNull.Value)
			{
				productAttributeAmountModel.RowAttributeValue = dr.GetString(0);
			}
			foreach (string item2 in list)
			{
				int ordinal = dr.GetOrdinal(item2);
				decimal amount = default(decimal);
				if (!dr.IsDBNull(ordinal))
				{
					amount = dr.GetDecimal(ordinal);
				}
				ProductAttributeAmountDetailModel item = new ProductAttributeAmountDetailModel
				{
					ColumnAttributeValue = item2,
					Amount = amount
				};
				productAttributeAmountModel.ColumnValues.Add(item);
			}
			return productAttributeAmountModel;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadDataProductAttributeAmountModel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\ProductPersistenceRepository.cs", 226);
			throw;
		}
	}
}
