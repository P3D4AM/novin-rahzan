using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting.ProductAttrebutes;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class ProductAttributePersistenceRepository : ProductAttributeBasePersistenceRepository
{
	public ProductAttributePersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<IEnumerable<ProductAttributeValueReportBusiness>> GetAllValueAsync(CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributePersistenceRepository.cs", 16);
		IEnumerable<ProductAttributeValueReportBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<ProductAttributeValueReportBusiness> list = new List<ProductAttributeValueReportBusiness>();
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_ProductAttributeValue_GetAll", null, tranName, token, CommandType.StoredProcedure, 30, "GetAllValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributePersistenceRepository.cs");
				while (true)
				{
					if (await dr.ReadAsync())
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributePersistenceRepository.cs", 33);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributePersistenceRepository.cs");
			}
		}
		return result;
	}

	private ProductAttributeValueReportBusiness LoadData(DbDataReader dr)
	{
		try
		{
			ProductAttributeValueReportBusiness productAttributeValueReportBusiness = new ProductAttributeValueReportBusiness(ContextGuid);
			if (dr["Guid"] != DBNull.Value)
			{
				productAttributeValueReportBusiness.ValueGuid = (Guid)dr["Guid"];
			}
			if (dr["AttributeGuid"] != DBNull.Value)
			{
				productAttributeValueReportBusiness.MasterGuid = (Guid)dr["AttributeGuid"];
			}
			if (dr["AttributeName"] != DBNull.Value)
			{
				productAttributeValueReportBusiness.AttributeName = dr["AttributeName"].ToString();
			}
			if (dr["Value"] != DBNull.Value)
			{
				productAttributeValueReportBusiness.Value = dr["Value"].ToString();
			}
			return productAttributeValueReportBusiness;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributePersistenceRepository.cs", 57);
			throw;
		}
	}
}
