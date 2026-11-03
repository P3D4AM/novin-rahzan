using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting;
using EntityCache.Business.Global;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Filters;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class ResidPersistenceRepository : BaseResidPersistenceRepository
{
	public ResidPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<IEnumerable<ResidCompactBusiness>> GetAllCompactAsync(ResidFilter filter, CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllCompactAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\ResidPersistenceRepository.cs", 23);
		IEnumerable<ResidCompactBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				List<ResidCompactBusiness> list = new List<ResidCompactBusiness>();
				List<SqlParameter> param = new List<SqlParameter>();
				if (filter?.DateFrom.HasValue ?? false)
				{
					param.Add(new SqlParameter("@DateFrom", (object)filter.DateFrom.Value));
				}
				if (filter?.DateTo.HasValue ?? false)
				{
					param.Add(new SqlParameter("@DateTo", (object)filter.DateTo.Value));
				}
				if ((filter?.SelectedStatus?.Any()).GetValueOrDefault())
				{
					ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = await StimulQueryBusiness.SaveAsync(ContextGuid, filter.SelectedStatus, token);
					returnedSaveFuncInfoWithValue.throwExceptionIfError();
					param.Add(new SqlParameter("@RefId", (object)returnedSaveFuncInfoWithValue.value));
				}
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_Resid_GetAllCompact", param, tranName, token, CommandType.StoredProcedure, 30, "GetAllCompactAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\ResidPersistenceRepository.cs");
				while (true)
				{
					if (await dr.ReadAsync())
					{
						if (base.IsDisposed || token.IsCancellationRequested)
						{
							result = null;
							break;
						}
						list.Add(LoadData(dr));
						continue;
					}
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllCompactAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\ResidPersistenceRepository.cs", 48);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllCompactAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\ResidPersistenceRepository.cs");
			}
		}
		return result;
	}

	private ResidCompactBusiness LoadData(DbDataReader dr)
	{
		try
		{
			ResidCompactBusiness residCompactBusiness = new ResidCompactBusiness(ContextGuid);
			if (dr["Guid"] != DBNull.Value)
			{
				residCompactBusiness.Guid = (Guid)dr["Guid"];
			}
			if (dr["Modified"] != DBNull.Value)
			{
				residCompactBusiness.Modified = (DateTime)dr["Modified"];
			}
			if (dr["ID2"] != DBNull.Value)
			{
				residCompactBusiness.ID2 = (long)dr["ID2"];
			}
			if (dr["DateM"] != DBNull.Value)
			{
				residCompactBusiness.DateM = (DateTime)dr["DateM"];
			}
			if (dr["Description"] != DBNull.Value)
			{
				residCompactBusiness.Description = dr["Description"].ToString();
			}
			if (dr["Date1"] != DBNull.Value)
			{
				residCompactBusiness.Date1 = (DateTime)dr["Date1"];
			}
			if (dr["Date2"] != DBNull.Value)
			{
				residCompactBusiness.Date2 = (DateTime)dr["Date2"];
			}
			if (dr["GuidVahed"] != DBNull.Value)
			{
				residCompactBusiness.GuidVahed = (Guid)dr["GuidVahed"];
			}
			if (dr["VahedName"] != DBNull.Value)
			{
				residCompactBusiness.VahedName = dr["VahedName"].ToString();
			}
			if (dr["Price"] != DBNull.Value)
			{
				residCompactBusiness.Price = (decimal)dr["Price"];
			}
			if (dr["Qt"] != DBNull.Value)
			{
				residCompactBusiness.Qt = (decimal)dr["Qt"];
			}
			if (dr["Beyane"] != DBNull.Value)
			{
				residCompactBusiness.Beyane = (decimal)dr["Beyane"];
			}
			if (dr["Mandeh"] != DBNull.Value)
			{
				residCompactBusiness.Mandeh = (decimal)dr["Mandeh"];
			}
			if (dr["PrdModel"] != DBNull.Value)
			{
				residCompactBusiness.PrdModel = dr["PrdModel"].ToString();
			}
			if (dr["PrdColor"] != DBNull.Value)
			{
				residCompactBusiness.PrdColor = dr["PrdColor"].ToString();
			}
			if (dr["PrdSerial"] != DBNull.Value)
			{
				residCompactBusiness.PrdSerial = dr["PrdSerial"].ToString();
			}
			if (dr["TafsilGuid"] != DBNull.Value)
			{
				residCompactBusiness.TafsilGuid = (Guid)dr["TafsilGuid"];
			}
			if (dr["TafsilName"] != DBNull.Value)
			{
				residCompactBusiness.TafsilName = dr["TafsilName"].ToString();
			}
			if (dr["Problems"] != DBNull.Value)
			{
				residCompactBusiness.Problems = dr["Problems"].ToString();
			}
			if (dr["Darkhast"] != DBNull.Value)
			{
				residCompactBusiness.Darkhast = dr["Darkhast"].ToString();
			}
			if (dr["PrdName"] != DBNull.Value)
			{
				residCompactBusiness.PrdName = dr["PrdName"].ToString();
			}
			if (dr["StateDisplay"] != DBNull.Value)
			{
				residCompactBusiness.StateDisplay = dr["StateDisplay"].ToString();
			}
			if (dr["StatusId"] != DBNull.Value)
			{
				residCompactBusiness.StatusId = (short)dr["StatusId"];
			}
			if (dr["CashierGuid"] != DBNull.Value)
			{
				residCompactBusiness.CashierGuid = (Guid)dr["CashierGuid"];
			}
			if (dr["ReceptionGuid"] != DBNull.Value)
			{
				residCompactBusiness.ReceptionGuid = (Guid)dr["ReceptionGuid"];
			}
			if (dr["OrderGuid"] != DBNull.Value)
			{
				residCompactBusiness.OrderGuid = (Guid)dr["OrderGuid"];
			}
			if (dr["StatusGuid"] != DBNull.Value)
			{
				residCompactBusiness.SanadStatusReportGuid = (Guid)dr["StatusGuid"];
			}
			if (dr["Reserve"] != DBNull.Value)
			{
				residCompactBusiness.Reserve = dr["Reserve"].ToString();
			}
			if (dr["PrdGuid"] != DBNull.Value)
			{
				residCompactBusiness.ProductGuid = (Guid)dr["PrdGuid"];
			}
			return residCompactBusiness;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\ResidPersistenceRepository.cs", 97);
			throw;
		}
	}
}
