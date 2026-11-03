using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Details;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Order;

public class OrderDetailVersion5PersistenceRepository : IOrderDetailRepository, IPersistenceMasterRepository<OrderDetailBusiness>, IDisposable
{
	private string ContextGuid;

	private bool _isDisposed;

	private static ConcurrentDictionary<string, OrderDetailVersion5PersistenceRepository> _instance = new ConcurrentDictionary<string, OrderDetailVersion5PersistenceRepository>();

	public ContextHolder DatabaseContext { get; private set; }

	public IItemAsyncMapper<OrderDetail, OrderDetailBusiness> MapFromDatabase { get; }

	public IItemAsyncMapper<OrderDetailBusiness, OrderDetail> MapToDatabase { get; }

	public bool IsDisposed => _isDisposed;

	public OrderDetailVersion5PersistenceRepository(ContextHolder dbContext)
	{
		_instance.TryAdd(dbContext.Context.Guid, this);
		DatabaseContext = dbContext;
		ContextGuid = dbContext.Context.Guid;
	}

	public static OrderDetailVersion5PersistenceRepository GetInstance(string contextGuid)
	{
		OrderDetailVersion5PersistenceRepository orderDetailVersion5PersistenceRepository = null;
		try
		{
			_instance.TryGetValue(contextGuid, out var value);
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderDetailVersion5PersistenceRepository.cs", 45);
			throw;
		}
	}

	public Task GetAllAsync(OrderBusiness order, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync(Guid masterGuid, int masterId, IReadOnlyList<OrderDetailBusiness> lstItems, Document factorType, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> RemoveRangeDetailsAsync(int orderId, Document type, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext baseModelContext = await DatabaseContext.GetContextAsync(tranName, 60000, "RemoveRangeDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderDetailVersion5PersistenceRepository.cs", 109);
		try
		{
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@OrderId", (object)orderId),
				new SqlParameter("@FactorType", (object)(short)type)
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await baseModelContext.ExecuteNoneQueryAsync("sp_OrderDetails_DeleteRows", parameters, tranName, default(CancellationToken), "", CommandType.StoredProcedure, 30, "RemoveRangeDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderDetailVersion5PersistenceRepository.cs"), ReturnedState.None, "RemoveRangeDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderDetailVersion5PersistenceRepository.cs", 117);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveRangeDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderDetailVersion5PersistenceRepository.cs", 121);
			res.AddReturnedValue(ex, "", "RemoveRangeDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderDetailVersion5PersistenceRepository.cs", 122);
		}
		return res;
	}

	public async Task<List<WebOrderDetail>> GetAllDetailsAsync(Guid orderGuid, Document type, string tranName = "")
	{
		List<WebOrderDetail> list = new List<WebOrderDetail>();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAllDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderDetailVersion5PersistenceRepository.cs", 130);
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@Guid", (object)orderGuid),
					new SqlParameter("@Type", (object)(short)type)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("SP_Factor_GetDetail", parameters, tranName, default(CancellationToken), CommandType.StoredProcedure, 30, "GetAllDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderDetailVersion5PersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					List<WebOrderDetail> list2 = list;
					list2.Add(await LoadDetailsAsync(dr, type, tranName));
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderDetailVersion5PersistenceRepository.cs", 145);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderDetailVersion5PersistenceRepository.cs");
			}
		}
		return list?.OrderBy((WebOrderDetail q) => q.ID)?.ToList();
	}

	private async Task<WebOrderDetail> LoadDetailsAsync(DbDataReader dr, Document orderType, string tranName = "")
	{
		WebOrderDetail webOrderDetail = new WebOrderDetail();
		try
		{
			webOrderDetail.IsEdit = true;
			if (dr["DepotGuid"] != DBNull.Value)
			{
				webOrderDetail.DepotGuid = (Guid)dr["DepotGuid"];
			}
			if (dr["Price"] != DBNull.Value)
			{
				webOrderDetail.Price = (decimal)dr["Price"];
			}
			if (dr["ProductGuid"] != DBNull.Value)
			{
				webOrderDetail.ProductGuid = (Guid)dr["ProductGuid"];
			}
			if (dr["VGuid"] != DBNull.Value)
			{
				webOrderDetail.VGuid = (Guid)dr["VGuid"];
			}
			if (dr["Quantity"] != DBNull.Value)
			{
				webOrderDetail.Quantity = (decimal)(double)dr["Quantity"];
			}
			if (dr["V2QT"] != DBNull.Value)
			{
				webOrderDetail.V2Qt = (decimal)(double)dr["V2QT"];
			}
			if (dr["Serial"] != DBNull.Value)
			{
				webOrderDetail.Serial = dr["Serial"].ToString();
			}
			if (dr["Description"] != DBNull.Value)
			{
				webOrderDetail.Description = dr["Description"].ToString();
			}
			if (dr["Discount"] != DBNull.Value)
			{
				webOrderDetail.Discount = dr["Discount"].ToString().ParseToDecimal();
			}
			if (dr["B1"] != DBNull.Value)
			{
				webOrderDetail.VisitorPrice1 = (decimal)dr["B1"];
			}
			if (dr["B2"] != DBNull.Value)
			{
				webOrderDetail.VisitorPrice2 = (decimal)dr["B2"];
			}
			if (dr["B3"] != DBNull.Value)
			{
				webOrderDetail.VisitorPrice3 = (decimal)dr["B3"];
			}
			if (dr["B4"] != DBNull.Value)
			{
				webOrderDetail.VisitorPrice4 = (decimal)dr["B4"];
			}
			if (dr["Avarez"] != DBNull.Value)
			{
				webOrderDetail.Avarez = (decimal)dr["Avarez"];
			}
			if (dr["Maliat"] != DBNull.Value)
			{
				webOrderDetail.Tax = (decimal)dr["Maliat"];
			}
			webOrderDetail.Guid = Guid.NewGuid();
			if (webOrderDetail.Guid == Guid.Empty)
			{
				webOrderDetail.Guid = Guid.NewGuid();
			}
			if (dr["DecimalGuid"] != DBNull.Value)
			{
				webOrderDetail.DecimalGuid = (Guid)dr["DecimalGuid"];
			}
			if (dr["DecimalPrice"] != DBNull.Value)
			{
				webOrderDetail.DecimalPrice = (decimal)dr["DecimalPrice"];
			}
			if (dr["DecimalQt"] != DBNull.Value)
			{
				webOrderDetail.DecimalQt = (decimal)(double)dr["DecimalQt"];
			}
			if (dr["Hazine1"] != DBNull.Value)
			{
				webOrderDetail.Hazine1Price = dr["Hazine1"].ToString().ParseToDecimal();
			}
			if (dr["Hazine2"] != DBNull.Value)
			{
				webOrderDetail.Hazine2Price = dr["Hazine2"].ToString().ParseToDecimal();
			}
			if (dr["Hazine3"] != DBNull.Value)
			{
				webOrderDetail.Hazine3Price = dr["Hazine3"].ToString().ParseToDecimal();
			}
			if (dr["Hazine4"] != DBNull.Value)
			{
				webOrderDetail.Hazine4Price = dr["Hazine4"].ToString().ParseToDecimal();
			}
			if (dr["Hazine5"] != DBNull.Value)
			{
				webOrderDetail.Hazine5Price = dr["Hazine5"].ToString().ParseToDecimal();
			}
			if (dr["V2Price"] != DBNull.Value)
			{
				webOrderDetail.V2Price = dr["V2Price"].ToString().ParseToDecimal();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderDetailVersion5PersistenceRepository.cs", 202);
			throw;
		}
		return webOrderDetail;
	}

	public void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderDetailVersion5PersistenceRepository.cs", 216);
		}
	}

	public Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<OrderDetailBusiness> items, Guid MasterGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> RemoveAsync(Guid MasterGuid, Document factorType, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<List<OrderDetailBusiness>> GetAllAsync(Guid MasterGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> RemoveAsync(Guid MasterGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<Guid> GetDepotGuidAsync(Guid guid, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<Guid> GetMasterGuidAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
