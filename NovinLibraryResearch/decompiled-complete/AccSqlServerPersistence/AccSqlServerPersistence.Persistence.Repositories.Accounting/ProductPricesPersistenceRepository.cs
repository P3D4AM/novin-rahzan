using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class ProductPricesPersistenceRepository : ProductDecimalRelationPersistenceBaseRepository
{
	public ProductPricesPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<IEnumerable<ProductDecimalRelationBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\ProductPricesPersistenceRepository.cs", 22));
		IEnumerable<ProductDecimalRelationBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				IEnumerable<DecimalBusiness> decimalList = await DecimalBusiness.GetAllAsync(base.ContextGuid, true, token, tranName);
				List<ProductDecimalRelationBusiness> list = new List<ProductDecimalRelationBusiness>();
				DbDataReader dr = await c.ExecuteReaderAsync("sp_ProductPrices_GetAll", null, tranName, token, CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\ProductPricesPersistenceRepository.cs");
				while (true)
				{
					if (await dr.ReadAsync())
					{
						if (base.IsDisposed || token.IsCancellationRequested)
						{
							result = null;
							break;
						}
						list.Add(LoadData(dr, decimalList));
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\ProductPricesPersistenceRepository.cs", 41);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\ProductPricesPersistenceRepository.cs");
			}
		}
		return result;
	}

	private ProductDecimalRelationBusiness LoadData(DbDataReader dr, IEnumerable<DecimalBusiness> decimalList)
	{
		try
		{
			ProductDecimalRelationBusiness item = new ProductDecimalRelationBusiness(base.ContextGuid);
			if (dr["ProductGuid"] != DBNull.Value)
			{
				item.ProductGuid = (Guid)dr["ProductGuid"];
			}
			if (dr["ProductCode"] != DBNull.Value)
			{
				item.ProductCode = dr["ProductCode"].ToString();
			}
			if (dr["ProductName"] != DBNull.Value)
			{
				item.ProductName = dr["ProductName"].ToString();
			}
			if (dr["VahedName"] != DBNull.Value)
			{
				item.ProductVahed = dr["VahedName"].ToString();
			}
			if (dr["DecimalGuid"] != DBNull.Value)
			{
				item.DecimalGuid = (Guid)dr["DecimalGuid"];
			}
			if (dr["Amount"] != DBNull.Value)
			{
				item.Amount = (decimal)dr["Amount"];
			}
			if (dr["BuyLast"] != DBNull.Value)
			{
				item.LastBuy = (decimal)dr["BuyLast"];
			}
			if (dr["GroupGuid"] != DBNull.Value)
			{
				item.ProductGroupGuid = (Guid)dr["GroupGuid"];
			}
			if (decimalList != null && decimalList.Any())
			{
				DecimalBusiness decimalBusiness = decimalList?.SingleOrDefault((DecimalBusiness q) => q.Guid == item.DecimalGuid);
				item.DecimalName = decimalBusiness.Name + " (" + decimalBusiness.Country + ")";
				item.DecimalPrice = decimalBusiness.LastPrice;
			}
			return item;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\ProductPricesPersistenceRepository.cs", 76);
			throw;
		}
	}
}
