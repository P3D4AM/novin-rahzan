using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Model;
using EntityCache;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Business.Global;
using EntityCache.Core.Repository.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Filters;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class OrderPersistenceRepository : OrderPersistenceRepositoryBase
{
	public OrderPersistenceRepository(ContextHolder context, ITableRelationShipRepository attachmentRepo)
		: base(context, attachmentRepo)
	{
	}

	public override async Task<IEnumerable<OrderReportBusiness>> GetAllCustomerLastTransactionAsync(Document? factorType, CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllCustomerLastTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 29);
		IEnumerable<OrderReportBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				List<OrderReportBusiness> list = new List<OrderReportBusiness>();
				List<SqlParameter> parameters = null;
				if (factorType.HasValue)
				{
					parameters = new List<SqlParameter>
					{
						new SqlParameter("@factorType", (object)(short)factorType.Value)
					};
				}
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Customer_LastTransaction", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllCustomerLastTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					List<OrderReportBusiness> list2 = list;
					list2.Add(await LoadDataAsync(dr, token, tranName));
				}
				dr.Close();
				result = list;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllCustomerLastTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 48);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllCustomerLastTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<IEnumerable<OrderNotPayedViewModel>> GetAllOrderNotPayedAsync(DateTime orderDate, Guid tafsilGuid, Guid orderGuid, CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllOrderNotPayedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 59);
		List<OrderNotPayedViewModel> list = new List<OrderNotPayedViewModel>();
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@OrderDate", (object)orderDate),
					new SqlParameter("@TafsilGuid", (object)tafsilGuid),
					new SqlParameter("@OrderGuid", (object)orderGuid)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_Order_GetAllNotPayed", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllOrderNotPayedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					list.Add(LoadDataNotPayed(dr));
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllOrderNotPayedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 79);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllOrderNotPayedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs");
			}
		}
		return list;
	}

	public override async Task<ReturnedSaveFuncInfo> UpdateSanadAffectiveAsync(bool value, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			BaseModelContext obj = await dbContext.GetContextAsync(tranName, 60000, "UpdateSanadAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 94);
			string spName = $"update Acc_Order set SanadAffective='{value}' , KardexAffective='{value}'";
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await obj.ExecuteNoneQueryAsync(spName, null, tranName, token, "", CommandType.Text, 30, "UpdateSanadAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs"), ReturnedState.None, "UpdateSanadAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 96);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateSanadAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 100);
			res.AddReturnedValue(ex, "", "UpdateSanadAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 101);
		}
		return res;
	}

	public override async Task<ReturnedSaveFuncInfo> UpdateCustomerFullNameAsync(Guid tafsilGuid, string name, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			BaseModelContext obj = await dbContext.GetContextAsync(tranName, 60000, "UpdateCustomerFullNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 111);
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@TafsilGuid", (object)tafsilGuid),
				new SqlParameter("@Name", (object)name)
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await obj.ExecuteNoneQueryAsync("sp_Acc_Order_UpdateCustomerFullName", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "UpdateCustomerFullNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs"), ReturnedState.None, "UpdateCustomerFullNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 117);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateCustomerFullNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 121);
			res.AddReturnedValue(ex, "", "UpdateCustomerFullNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 122);
		}
		return res;
	}

	public override async Task<IEnumerable<OrderDetailSerialModel>> GetAllDetailSerialAsync(string serial, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(serial))
		{
			throw new ArgumentNullException("serial");
		}
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllDetailSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 129);
		IEnumerable<OrderDetailSerialModel> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<OrderDetailSerialModel> list = new List<OrderDetailSerialModel>();
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@Serial", (object)serial)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_OrderDetail_GetBySerial", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllDetailSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					list.Add(LoadOrderDetailSerial(dr));
				}
				dr.Close();
				result = list;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllDetailSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 145);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllDetailSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<ReturnedSaveFuncInfo> UpdateDateMoaserAsync(Guid? guid, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			BaseModelContext obj = await dbContext.GetContextAsync(tranName, 60000, "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 160);
			List<SqlParameter> parameters = null;
			if ((guid ?? Guid.Empty) != Guid.Empty)
			{
				parameters = new List<SqlParameter>
				{
					new SqlParameter("@Guid", (object)guid)
				};
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await obj.ExecuteNoneQueryAsync("Acc_Order_UpdateDateMoaser", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs"), ReturnedState.None, "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 163);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 167);
			res.AddReturnedValue(ex, "", "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 168);
		}
		return res;
	}

	public override async Task<IEnumerable<OrderReportBusiness>> GetAllAsync(OrderFilter filter, CancellationToken token, string tranName)
	{
		_ = 2;
		try
		{
			IEnumerable<OrderReportBusiness> list = await base.GetAllAsync(filter, token, tranName);
			if (!(list?.Any() ?? false))
			{
				return null;
			}
			ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = await StimulQueryBusiness.SaveAsync(base.ContextGuid, list.Select((OrderReportBusiness q) => q.Guid), token);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				return list;
			}
			List<Tuple<Guid, decimal>> list2 = await GetAttachmentSumAsync(returnedSaveFuncInfoWithValue.value, token, tranName);
			if (list2 == null || !list2.Any())
			{
				return list;
			}
			foreach (Tuple<Guid, decimal> item in list2)
			{
				if (item != null)
				{
					OrderReportBusiness orderReportBusiness = list.SingleOrDefault((OrderReportBusiness q) => q.Guid == item.Item1);
					if (orderReportBusiness != null)
					{
						orderReportBusiness.AttachmentSum = item.Item2;
					}
				}
			}
			return list;
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 195);
			throw;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> UpdateKardexAffectiveAsync(Document factorType, bool kardexAffective, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			BaseModelContext obj = await dbContext.GetContextAsync(tranName, 60000, "UpdateKardexAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 205);
			string spName = $"update Acc_Order set KardexAffective='{kardexAffective}' Where FactorType='{(short)factorType}'";
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await obj.ExecuteNoneQueryAsync(spName, null, tranName, token, "", CommandType.Text, 30, "UpdateKardexAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs"), ReturnedState.None, "UpdateKardexAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 207);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateKardexAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 211);
			res.AddReturnedValue(ex, "", "UpdateKardexAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 212);
		}
		return res;
	}

	public override async Task<OrderReportBusiness> GetReportAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 218));
		using (new JaegerMonitor("GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs"))
		{
			OrderReportBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 3;
				try
				{
					Order order = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Order>((IQueryable<Order>)c.Order, (Expression<Func<Order, bool>>)((Order x) => x.Guid == guid), token);
					if (order == null)
					{
						result = null;
					}
					else
					{
						OrderReportBusiness ret = await base.MapFromDatabaseToReport.MapAsync(base.ContextGuid, order, null, tranName, token);
						ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = await StimulQueryBusiness.SaveAsync(base.ContextGuid, new List<Guid> { guid }, token);
						if (returnedSaveFuncInfoWithValue.HasError)
						{
							result = ret;
						}
						else
						{
							List<Tuple<Guid, decimal>> list = await GetAttachmentSumAsync(returnedSaveFuncInfoWithValue.value, token, tranName);
							if (list == null || !list.Any())
							{
								result = ret;
							}
							else
							{
								foreach (Tuple<Guid, decimal> item in list)
								{
									if (item != null)
									{
										ret.AttachmentSum = item.Item2;
									}
								}
								result = ret;
							}
						}
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 243);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<decimal> GetRelatedBackSellSumAsync(Guid orderGuid, CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetRelatedBackSellSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 254);
		decimal result;
		try
		{
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@Guid", (object)orderGuid)
				};
				decimal res = default(decimal);
				object obj = await c.ExecuteScalarAsync<object>("sp_Acc_Order_GetRelatedBackSellSum", parameters, tranName, token, 30, "GetRelatedBackSellSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs");
				if (obj != null)
				{
					res = obj.ToString().ParseToDecimal(0m, ClsCache.GetCache(base.ContextGuid).MultiLanguageConfig.DigitAfterPoint);
				}
				result = res;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetRelatedBackSellSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 268);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetRelatedBackSellSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs");
			}
		}
		return result;
	}

	private async Task<List<Tuple<Guid, decimal>>> GetAttachmentSumAsync(int refId, CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAttachmentSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 280);
		List<Tuple<Guid, decimal>> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<Tuple<Guid, decimal>> list = new List<Tuple<Guid, decimal>>();
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@refId", (object)refId)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_acc_order_GetAttachmentSum", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAttachmentSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs");
				while (true)
				{
					if (await dr.ReadAsync(token))
					{
						if (token.IsCancellationRequested)
						{
							result = null;
							break;
						}
						Guid item = (Guid)dr["Guid"];
						decimal item2 = (decimal)dr["AttachmentSum"];
						list.Add(new Tuple<Guid, decimal>(item, item2));
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAttachmentSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 299);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAttachmentSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs");
			}
		}
		return result;
	}

	private async Task<OrderReportBusiness> LoadDataAsync(DbDataReader dr, CancellationToken token, string tranName)
	{
		OrderReportBusiness item = new OrderReportBusiness(base.ContextGuid);
		try
		{
			if (dr["ReceptionGuid"] != DBNull.Value)
			{
				item.ReceptionGuid = (Guid)dr["ReceptionGuid"];
			}
			if (dr["PardakhtGuid"] != DBNull.Value)
			{
				item.PardakhtGuid = (Guid)dr["PardakhtGuid"];
			}
			if (dr["AfterAccount"] != DBNull.Value)
			{
				item.AfterAccount = (decimal)dr["AfterAccount"];
			}
			if (dr["AghsatGuid"] != DBNull.Value)
			{
				item.AghsatGuid = (Guid)dr["AghsatGuid"];
			}
			if (dr["AndroidIme"] != DBNull.Value)
			{
				item.AndroidIme = dr["AndroidIme"].ToString();
			}
			if (dr["AndroidImeLong"] != DBNull.Value)
			{
				item.AndroidImeLong = (long)dr["AndroidImeLong"];
			}
			if (dr["Avarez"] != DBNull.Value)
			{
				item.Avarez = (decimal)dr["Avarez"];
			}
			if (dr["B1TafsilGuid"] != DBNull.Value)
			{
				item.B1TafsilGuid = (Guid)dr["B1TafsilGuid"];
			}
			if (dr["B2TafsilGuid"] != DBNull.Value)
			{
				item.B2TafsilGuid = (Guid)dr["B2TafsilGuid"];
			}
			if (dr["B3TafsilGuid"] != DBNull.Value)
			{
				item.B3TafsilGuid = (Guid)dr["B3TafsilGuid"];
			}
			if (dr["B4TafsilGuid"] != DBNull.Value)
			{
				item.B4TafsilGuid = (Guid)dr["B4TafsilGuid"];
			}
			if (dr["BarnameNo"] != DBNull.Value)
			{
				item.BarnameNo = dr["BarnameNo"].ToString();
			}
			if (dr["TransferDate"] != DBNull.Value)
			{
				item.TransferDate = (DateTime)dr["TransferDate"];
			}
			if (dr["BeforeAccount"] != DBNull.Value)
			{
				item.BeforeAccount = (decimal)dr["BeforeAccount"];
			}
			if (dr["CpuSerial"] != DBNull.Value)
			{
				item.CpuSerial = dr["CpuSerial"].ToString();
			}
			if (dr["Date"] != DBNull.Value)
			{
				item.Date = (DateTime)dr["Date"];
			}
			if (dr["DateSabt"] != DBNull.Value)
			{
				item.DateSabt = (DateTime)dr["DateSabt"];
			}
			if (dr["Description"] != DBNull.Value)
			{
				item.Description = dr["Description"].ToString();
			}
			if (dr["Discount"] != DBNull.Value)
			{
				item.Discount = (decimal)dr["Discount"];
			}
			if (dr["DiscountRows"] != DBNull.Value)
			{
				item.DiscountRows = (decimal)dr["DiscountRows"];
			}
			if (dr["FactorType"] != DBNull.Value)
			{
				item.FactorType = (Document)dr["FactorType"];
			}
			if (dr["DailyFactorNo"] != DBNull.Value)
			{
				item.DailyFactorNo = (short)dr["DailyFactorNo"];
			}
			if (dr["HazineTafsilGuid1"] != DBNull.Value)
			{
				item.HazineTafsilGuid1 = (Guid)dr["HazineTafsilGuid1"];
			}
			if (dr["HazineTafsilGuid2"] != DBNull.Value)
			{
				item.HazineTafsilGuid2 = (Guid)dr["HazineTafsilGuid2"];
			}
			if (dr["HazineTafsilGuid3"] != DBNull.Value)
			{
				item.HazineTafsilGuid3 = (Guid)dr["HazineTafsilGuid3"];
			}
			if (dr["HazineTafsilGuid4"] != DBNull.Value)
			{
				item.HazineTafsilGuid4 = (Guid)dr["HazineTafsilGuid4"];
			}
			if (dr["HazineTafsilGuid5"] != DBNull.Value)
			{
				item.HazineTafsilGuid5 = (Guid)dr["HazineTafsilGuid5"];
			}
			if (dr["HazineMoeinGuid1"] != DBNull.Value)
			{
				item.HazineMoeinGuid1 = (Guid)dr["HazineMoeinGuid1"];
			}
			if (dr["HazineMoeinGuid2"] != DBNull.Value)
			{
				item.HazineMoeinGuid2 = (Guid)dr["HazineMoeinGuid2"];
			}
			if (dr["HazineMoeinGuid3"] != DBNull.Value)
			{
				item.HazineMoeinGuid3 = (Guid)dr["HazineMoeinGuid3"];
			}
			if (dr["HazineMoeinGuid4"] != DBNull.Value)
			{
				item.HazineMoeinGuid4 = (Guid)dr["HazineMoeinGuid4"];
			}
			if (dr["HazineMoeinGuid5"] != DBNull.Value)
			{
				item.HazineMoeinGuid5 = (Guid)dr["HazineMoeinGuid5"];
			}
			if (dr["HddSerial"] != DBNull.Value)
			{
				item.HddSerial = dr["HddSerial"].ToString();
			}
			if (dr["TafsilGuid"] != DBNull.Value)
			{
				item.TafsilGuid = (Guid)dr["TafsilGuid"];
			}
			if (dr["MoeinGuid"] != DBNull.Value)
			{
				item.MoeinGuid = (Guid)dr["MoeinGuid"];
			}
			if (dr["Id2"] != DBNull.Value)
			{
				item.Id2 = (int)dr["Id2"];
			}
			if (dr["Ip"] != DBNull.Value)
			{
				item.Ip = dr["Ip"].ToString();
			}
			if (dr["Keraye"] != DBNull.Value)
			{
				item.Keraye = (decimal)dr["Keraye"];
			}
			if (dr["FareMoeinGuid"] != DBNull.Value)
			{
				item.FareMoeinGuid = (Guid)dr["FareMoeinGuid"];
			}
			if (dr["FareTafsilGuid"] != DBNull.Value)
			{
				item.FareTafsilGuid = (Guid)dr["FareTafsilGuid"];
			}
			if (dr["LkSerial"] != DBNull.Value)
			{
				item.LkSerial = dr["LkSerial"].ToString();
			}
			if (dr["PrintCount"] != DBNull.Value)
			{
				item.PrintCount = (short)dr["PrintCount"];
			}
			if (dr["PrintDate"] != DBNull.Value)
			{
				item.PrintDate = (DateTime)dr["PrintDate"];
			}
			if (dr["SanadNo"] != DBNull.Value)
			{
				item.SanadNo = (int)dr["SanadNo"];
			}
			if (dr["SanadSt"] != DBNull.Value)
			{
				item.SanadSt = (SanadSt)dr["SanadSt"];
			}
			if (dr["UserAccGuid"] != DBNull.Value)
			{
				item.UserAccGuid = (Guid)dr["UserAccGuid"];
			}
			if (dr["DeliveryId"] != DBNull.Value)
			{
				item.DeliveryId = (int)dr["DeliveryId"];
			}
			if (dr["DeliveryMessage"] != DBNull.Value)
			{
				item.DeliveryMessage = dr["DeliveryMessage"].ToString();
			}
			if (dr["DeliveryShortMessage"] != DBNull.Value)
			{
				item.DeliveryShortMessage = dr["DeliveryShortMessage"].ToString();
			}
			if (dr["DeliveryDate"] != DBNull.Value)
			{
				item.DeliveryDate = (DateTime)dr["DeliveryDate"];
			}
			if (dr["Guid"] != DBNull.Value)
			{
				item.Guid = (Guid)dr["Guid"];
			}
			if (dr["Modified"] != DBNull.Value)
			{
				item.Modified = (DateTime)dr["Modified"];
			}
			if (dr["PriceTypeGuid"] != DBNull.Value)
			{
				item.PriceTypeGuid = (Guid)dr["PriceTypeGuid"];
			}
			if (dr["IsTemp"] != DBNull.Value)
			{
				item.IsTemp = (bool)dr["IsTemp"];
			}
			if (dr["HostRefrenced"] != DBNull.Value)
			{
				item.HostRefrenced = dr["HostRefrenced"].ToString();
			}
			if (dr["B1MoeinGuid"] != DBNull.Value)
			{
				item.B1MoeinGuid = (Guid)dr["B1MoeinGuid"];
			}
			if (dr["B2MoeinGuid"] != DBNull.Value)
			{
				item.B2MoeinGuid = (Guid)dr["B2MoeinGuid"];
			}
			if (dr["B3MoeinGuid"] != DBNull.Value)
			{
				item.B3MoeinGuid = (Guid)dr["B3MoeinGuid"];
			}
			if (dr["B4MoeinGuid"] != DBNull.Value)
			{
				item.B4MoeinGuid = (Guid)dr["B4MoeinGuid"];
			}
			if (dr["AvarezTafsilGuid"] != DBNull.Value)
			{
				item.AvarezTafsilGuid = (Guid)dr["AvarezTafsilGuid"];
			}
			if (dr["AvarezMoeinGuid"] != DBNull.Value)
			{
				item.AvarezMoeinGuid = (Guid)dr["AvarezMoeinGuid"];
			}
			if (dr["TaxTafsilGuid"] != DBNull.Value)
			{
				item.TaxTafsilGuid = (Guid)dr["TaxTafsilGuid"];
			}
			if (dr["TaxMoeinGuid"] != DBNull.Value)
			{
				item.TaxMoeinGuid = (Guid)dr["TaxMoeinGuid"];
			}
			if (dr["CustName"] != DBNull.Value)
			{
				item.CustName = dr["CustName"].ToString();
			}
			if (dr["CustFamily"] != DBNull.Value)
			{
				item.CustFamily = dr["CustFamily"].ToString();
			}
			if (dr["CustMobile"] != DBNull.Value)
			{
				item.CustMobile = dr["CustMobile"].ToString();
			}
			if (dr["CustAddress"] != DBNull.Value)
			{
				item.CustAddress = dr["CustAddress"].ToString();
			}
			if (dr["CustZipCode"] != DBNull.Value)
			{
				item.CustZipCode = dr["CustZipCode"].ToString();
			}
			if (dr["CustTell"] != DBNull.Value)
			{
				item.CustTell = dr["CustTell"].ToString();
			}
			if (dr["PaymentMethod"] != DBNull.Value)
			{
				item.PaymentMethod = (PaymentMethod)dr["PaymentMethod"];
			}
			if (dr["ServerStatus"] != DBNull.Value)
			{
				item.ServerStatus = (ServerStatusForClient)dr["ServerStatus"];
			}
			if (dr["DeliveryStatus"] != DBNull.Value)
			{
				item.DeliveryStatus = (bool)dr["DeliveryStatus"];
			}
			if (dr["Source"] != DBNull.Value)
			{
				item.Source = (ENSource)dr["Source"];
			}
			if (dr["B1"] != DBNull.Value)
			{
				item.B1 = (decimal)dr["B1"];
			}
			if (dr["B2"] != DBNull.Value)
			{
				item.B2 = (decimal)dr["B2"];
			}
			if (dr["B3"] != DBNull.Value)
			{
				item.B3 = (decimal)dr["B3"];
			}
			if (dr["B4"] != DBNull.Value)
			{
				item.B4 = (decimal)dr["B4"];
			}
			if (dr["CustomerFullName"] != DBNull.Value)
			{
				item.CustomerFullName = dr["CustomerFullName"].ToString();
			}
			item.DateSh = item.Date.MiladiToCalture();
			if (dr["Status"] != DBNull.Value)
			{
				item.Status = (bool)dr["Status"];
			}
			if (dr["Sum"] != DBNull.Value)
			{
				item.Sum = (decimal)dr["Sum"];
			}
			if (dr["SumKala"] != DBNull.Value)
			{
				item.SumKala = (decimal)dr["SumKala"];
			}
			if (dr["TasvieDay"] != DBNull.Value)
			{
				item.TasvieDay = (int)dr["TasvieDay"];
			}
			if (dr["Tax"] != DBNull.Value)
			{
				item.Tax = (decimal)dr["Tax"];
			}
			if (dr["TotalHazine"] != DBNull.Value)
			{
				item.TotalHazine = (decimal)dr["TotalHazine"];
			}
			if (dr["TotalHazine1"] != DBNull.Value)
			{
				item.TotalHazine1 = (decimal)dr["TotalHazine1"];
			}
			if (dr["TotalHazine2"] != DBNull.Value)
			{
				item.TotalHazine2 = (decimal)dr["TotalHazine2"];
			}
			if (dr["TotalHazine3"] != DBNull.Value)
			{
				item.TotalHazine3 = (decimal)dr["TotalHazine3"];
			}
			if (dr["TotalHazine4"] != DBNull.Value)
			{
				item.TotalHazine4 = (decimal)dr["TotalHazine4"];
			}
			if (dr["TotalHazine5"] != DBNull.Value)
			{
				item.TotalHazine5 = (decimal)dr["TotalHazine5"];
			}
			if (dr["TotalSum"] != DBNull.Value)
			{
				item.TotalSum = (decimal)dr["TotalSum"];
			}
			if (dr["SellerFactorNo"] != DBNull.Value)
			{
				item.SellerFactorNo = dr["SellerFactorNo"].ToString();
			}
			if (dr["ProductMoeinGuid"] != DBNull.Value)
			{
				item.ProductMoeinGuid = (Guid)dr["ProductMoeinGuid"];
			}
			if (dr["DiscountTafsilGuid"] != DBNull.Value)
			{
				item.DiscountTafsilGuid = (Guid)dr["DiscountTafsilGuid"];
			}
			if (dr["DiscountMoeinGuid"] != DBNull.Value)
			{
				item.DiscountMoeinGuid = (Guid)dr["DiscountMoeinGuid"];
			}
			if (dr["TafsilGroupGuid"] != DBNull.Value)
			{
				item.TafsilGroupGuid = (Guid)dr["TafsilGroupGuid"];
			}
			CusCompactBusiness cusCompactBusiness = await CusCompactBusiness.GetAsync(base.ContextGuid, EnSystem.None, item.TafsilGuid, null, tranName, token);
			item.TafsilAccount = cusCompactBusiness?.Account ?? 0m;
			item.CustomerGroupGuid = cusCompactBusiness?.GroupGuid ?? Guid.Empty;
			string text = "";
			if (item.B1TafsilGuid.HasValue && item.B1TafsilGuid != Guid.Empty)
			{
				string text2 = text;
				text = text2 + (await TafsilBusiness.GetAsync(base.ContextGuid, EnSystem.None, item.B1TafsilGuid.Value, tranName, token, "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs"))?.Name + " - ";
			}
			if (item.B2TafsilGuid.HasValue && item.B2TafsilGuid != Guid.Empty)
			{
				string text2 = text;
				text = text2 + (await TafsilBusiness.GetAsync(base.ContextGuid, EnSystem.None, item.B2TafsilGuid.Value, tranName, token, "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs"))?.Name + " - ";
			}
			if (item.B3TafsilGuid.HasValue && item.B3TafsilGuid != Guid.Empty)
			{
				string text2 = text;
				text = text2 + (await TafsilBusiness.GetAsync(base.ContextGuid, EnSystem.None, item.B3TafsilGuid.Value, tranName, token, "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs"))?.Name + " - ";
			}
			if (item.B4TafsilGuid.HasValue && item.B4TafsilGuid != Guid.Empty)
			{
				string text2 = text;
				text = text2 + (await TafsilBusiness.GetAsync(base.ContextGuid, EnSystem.None, item.B4TafsilGuid.Value, tranName, token, "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs"))?.Name;
			}
			item.BazaryabFullName = text;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 430);
			throw;
		}
	}

	private OrderNotPayedViewModel LoadDataNotPayed(DbDataReader dr)
	{
		try
		{
			OrderNotPayedViewModel orderNotPayedViewModel = new OrderNotPayedViewModel();
			if (dr["Guid"] != DBNull.Value)
			{
				orderNotPayedViewModel.Guid = (Guid)dr["Guid"];
			}
			if (dr["Id2"] != DBNull.Value)
			{
				orderNotPayedViewModel.Id2 = (int)dr["Id2"];
			}
			if (dr["Date"] != DBNull.Value)
			{
				orderNotPayedViewModel.DateSabt = (DateTime)dr["Date"];
			}
			if (dr["TotalSum"] != DBNull.Value)
			{
				orderNotPayedViewModel.TotalSum = (decimal)dr["TotalSum"];
			}
			return orderNotPayedViewModel;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadDataNotPayed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 450);
			throw;
		}
	}

	private OrderDetailSerialModel LoadOrderDetailSerial(DbDataReader dr)
	{
		try
		{
			OrderDetailSerialModel orderDetailSerialModel = new OrderDetailSerialModel();
			if (dr["Guid"] != DBNull.Value)
			{
				orderDetailSerialModel.OrderDetailGuid = (Guid)dr["Guid"];
			}
			if (dr["FactorType"] != DBNull.Value)
			{
				orderDetailSerialModel.OrderType = (Document)(short)dr["FactorType"];
			}
			if (dr["Date"] != DBNull.Value)
			{
				orderDetailSerialModel.OrderDate = (DateTime)dr["Date"];
			}
			return orderDetailSerialModel;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadOrderDetailSerial", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\OrderPersistenceRepository.cs", 468);
			throw;
		}
	}
}
