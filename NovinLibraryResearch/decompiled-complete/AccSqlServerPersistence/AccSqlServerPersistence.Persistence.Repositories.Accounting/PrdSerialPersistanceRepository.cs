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
using EntityCache.Business.Products.ProductSerial;
using PacketParser.Entities;
using PacketParser.Filters;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class PrdSerialPersistanceRepository : PrdSerialPersistanceRepositoryBase
{
	private PrdSerialDetailPersistenceRepository _detRepo;

	public PrdSerialPersistanceRepository(ContextHolder db)
		: base(db)
	{
		_detRepo = new PrdSerialDetailPersistenceRepository(db);
	}

	public override async Task<IEnumerable<PrdSerialBusiness>> GetAllAsync(ProductSerialFilter filter, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepository.cs", 27));
			IEnumerable<PrdSerialBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 4;
				try
				{
					List<PrdSerialBusiness> list = new List<PrdSerialBusiness>();
					List<SqlParameter> list2 = new List<SqlParameter>();
					if (filter != null)
					{
						_ = filter.ProductGuid;
						if (filter.ProductGuid != Guid.Empty)
						{
							list2.Add(new SqlParameter("@productGuid", (object)filter.ProductGuid));
						}
						if (!string.IsNullOrEmpty(filter.Serial))
						{
							list2.Add(new SqlParameter("@serial", (object)filter.Serial));
						}
						if (filter.AsType != 0)
						{
							list2.Add(new SqlParameter("@asType", (object)(short)filter.AsType));
						}
						if (filter.DateM.HasValue)
						{
							list2.Add(new SqlParameter("@DateM", (object)filter.DateM));
						}
						_ = filter.DetailGuid;
						if (filter.DetailGuid != Guid.Empty)
						{
							list2.Add(new SqlParameter("@detailGuid", (object)filter.DetailGuid));
						}
					}
					DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_PrdSerial_GetAll", list2, tranName, token, CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepository.cs");
					while (true)
					{
						if (await dr.ReadAsync(token))
						{
							if (base.IsDisposed || token.IsCancellationRequested)
							{
								result = null;
								break;
							}
							PrdSerialBusiness item2 = LoadData(dr, checkIsMojod: true);
							if (item2 != null)
							{
								List<PrdSerialDetailBusiness> list3 = await _detRepo.GetAllAsync(item2.Guid, token, tranName);
								if (list3 != null && list3.Any())
								{
									item2.AddDetList(list3, clearDets: true);
								}
								list.Add(item2);
							}
							continue;
						}
						dr.Close();
						if (filter?.IsMojod.HasValue ?? false)
						{
							list = list?.Where((PrdSerialBusiness x) => x.IsMojod == filter.IsMojod).ToList();
						}
						if (!filter.DepotGuid.HasValue || filter.DepotGuid == Guid.Empty)
						{
							result = list;
							break;
						}
						List<PrdSerialBusiness> serialExists = new List<PrdSerialBusiness>();
						foreach (PrdSerialBusiness item2 in list)
						{
							if (item2 == null)
							{
								continue;
							}
							foreach (PrdSerialDetailBusiness det in item2.DetList)
							{
								Guid guid = Guid.Empty;
								if (det.AsType == Document.AnbarGardani)
								{
									guid = await PrdAnbarGardaniDetBusiness.GetDepotGuidAsync(ContextGuid, det.DetailGuid, token, tranName);
								}
								else if (det.AsType == Document.OrderSell || det.AsType == Document.OrderBuy || det.AsType == Document.OrderBackBuy || det.AsType == Document.OrderBackSell)
								{
									guid = await OrderDetailBusiness.GetDepotGuidAsync(ContextGuid, det.DetailGuid, token, tranName);
								}
								Guid value = guid;
								Guid? depotGuid = filter.DepotGuid;
								if (value == depotGuid && !serialExists.Select((PrdSerialBusiness q) => q.Serial).Contains(item2.Serial))
								{
									serialExists.Add(item2);
								}
							}
						}
						result = ((serialExists?.Any() ?? false) ? serialExists : list);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepository.cs", 80);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<IEnumerable<PrdSerialDetailReportBusiness>> GetAllAsync(string serial, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepository.cs", 92));
			IEnumerable<PrdSerialDetailReportBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					List<PrdSerialDetailReportBusiness> list = new List<PrdSerialDetailReportBusiness>();
					List<SqlParameter> parameters = new List<SqlParameter>
					{
						new SqlParameter("@serial", (object)serial)
					};
					DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_PrdSerial_GetAllGardesh", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepository.cs");
					while (await dr.ReadAsync())
					{
						list.Add(LoadDataViewMode(dr));
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepository.cs", 109);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<IEnumerable<PrdSerialBusiness>> GetAllWithoutDetailAsync(CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllWithoutDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllWithoutDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepository.cs", 121));
			IEnumerable<PrdSerialBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					List<PrdSerialBusiness> list = new List<PrdSerialBusiness>();
					string commandText = "select Acc_PrdSerial.* from Acc_PrdSerial left outer join Acc_PrdSerialDetail on Acc_PrdSerialDetail.MasterGuid=Acc_PrdSerial.Guid where Acc_PrdSerialDetail.Guid is null";
					DbDataReader dr = await c.ExecuteReaderAsync(commandText, null, tranName, token, CommandType.Text, 300, "GetAllWithoutDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepository.cs");
					while (await dr.ReadAsync())
					{
						list.Add(LoadData(dr, checkIsMojod: false));
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllWithoutDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepository.cs", 137);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllWithoutDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<IEnumerable<PrdSerialReportBusiness>> GetAllModelAsync(ProductSerialFilter filter, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepository.cs", 149));
			IEnumerable<PrdSerialReportBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					List<PrdSerialReportBusiness> list = new List<PrdSerialReportBusiness>();
					List<SqlParameter> list2 = new List<SqlParameter>();
					if (filter != null)
					{
						if ((filter.ProductGroupGuid ?? Guid.Empty) != Guid.Empty)
						{
							list2.Add(new SqlParameter("@GroupGuid", (object)filter.ProductGroupGuid.Value));
						}
						if (filter.IsMojod.HasValue)
						{
							list2.Add(new SqlParameter("@IsMojod", (object)filter.IsMojod.Value));
						}
						if (filter.GarrantyStartDate.HasValue)
						{
							list2.Add(new SqlParameter("@GarrantyStartDate", (object)filter.GarrantyStartDate.Value));
						}
						if (filter.GarrantyEndDate.HasValue)
						{
							list2.Add(new SqlParameter("@GarrantyEndDate", (object)filter.GarrantyEndDate.Value));
						}
					}
					DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_PrdSerial_GetAllModel", list2, tranName, token, CommandType.StoredProcedure, 30, "GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepository.cs");
					while (true)
					{
						if (await dr.ReadAsync(token))
						{
							if (base.IsDisposed || token.IsCancellationRequested)
							{
								result = null;
								break;
							}
							list.Add(LoadDataModel(dr));
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepository.cs", 177);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	private PrdSerialBusiness LoadData(DbDataReader dr, bool checkIsMojod)
	{
		try
		{
			PrdSerialBusiness prdSerialBusiness = new PrdSerialBusiness(ContextGuid);
			if (dr["Guid"] != DBNull.Value)
			{
				prdSerialBusiness.Guid = (Guid)dr["Guid"];
			}
			if (dr["Serial"] != DBNull.Value)
			{
				prdSerialBusiness.Serial = dr["Serial"].ToString();
			}
			if (dr["GarrantyCode"] != DBNull.Value)
			{
				prdSerialBusiness.GarrantyCode = dr["GarrantyCode"].ToString();
			}
			if (dr["StartDate"] != DBNull.Value)
			{
				prdSerialBusiness.StartDate = (DateTime)dr["StartDate"];
			}
			if (dr["EndDate"] != DBNull.Value)
			{
				prdSerialBusiness.EndDate = (DateTime)dr["EndDate"];
			}
			if (dr["ProductGuid"] != DBNull.Value)
			{
				prdSerialBusiness.ProductGuid = (Guid)dr["ProductGuid"];
			}
			if (dr["TechnicalInfo1"] != DBNull.Value)
			{
				prdSerialBusiness.TechnicalInfo1 = dr["TechnicalInfo1"].ToString();
			}
			if (dr["TechnicalInfo2"] != DBNull.Value)
			{
				prdSerialBusiness.TechnicalInfo2 = dr["TechnicalInfo2"].ToString();
			}
			if (dr["TechnicalInfo3"] != DBNull.Value)
			{
				prdSerialBusiness.TechnicalInfo3 = dr["TechnicalInfo3"].ToString();
			}
			if (!checkIsMojod)
			{
				return prdSerialBusiness;
			}
			if (dr["IsMojod"] != DBNull.Value)
			{
				string text = dr["IsMojod"].ToString();
				if (text == "1" || text.ToLower() == "true")
				{
					prdSerialBusiness.IsMojod = true;
				}
				else
				{
					prdSerialBusiness.IsMojod = false;
				}
			}
			return prdSerialBusiness;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepository.cs", 214);
			throw;
		}
	}

	private PrdSerialDetailReportBusiness LoadDataViewMode(DbDataReader dr)
	{
		try
		{
			PrdSerialDetailReportBusiness prdSerialDetailReportBusiness = new PrdSerialDetailReportBusiness(ContextGuid);
			if (dr["Guid"] != DBNull.Value)
			{
				prdSerialDetailReportBusiness.Guid = (Guid)dr["Guid"];
			}
			if (dr["Serial"] != DBNull.Value)
			{
				prdSerialDetailReportBusiness.Serial = dr["Serial"].ToString();
			}
			if (dr["GarrantyCode"] != DBNull.Value)
			{
				prdSerialDetailReportBusiness.GuarrantyCode = dr["GarrantyCode"].ToString();
			}
			if (dr["StartDate"] != DBNull.Value)
			{
				prdSerialDetailReportBusiness.StartDate = (DateTime)dr["StartDate"];
			}
			if (dr["EndDate"] != DBNull.Value)
			{
				prdSerialDetailReportBusiness.EndDate = (DateTime)dr["EndDate"];
			}
			if (dr["ProductGuid"] != DBNull.Value)
			{
				prdSerialDetailReportBusiness.ProductGuid = (Guid)dr["ProductGuid"];
			}
			if (dr["DetailGuid"] != DBNull.Value)
			{
				prdSerialDetailReportBusiness.DetailGuid = (Guid)dr["DetailGuid"];
			}
			if (dr["AsType"] != DBNull.Value)
			{
				prdSerialDetailReportBusiness.AsType = (Document)dr["AsType"];
			}
			if (dr["IsVorod"] != DBNull.Value)
			{
				prdSerialDetailReportBusiness.IsVorod = (bool)dr["IsVorod"];
			}
			if (dr["OrderDate"] != DBNull.Value)
			{
				prdSerialDetailReportBusiness.OrderDate = (DateTime)dr["OrderDate"];
			}
			if (dr["CashierGuid"] != DBNull.Value)
			{
				prdSerialDetailReportBusiness.CashierGuid = (Guid)dr["CashierGuid"];
			}
			if (dr["OrderDescription"] != DBNull.Value)
			{
				prdSerialDetailReportBusiness.OrderDescription = dr["OrderDescription"].ToString();
			}
			if (dr["OrderGuid"] != DBNull.Value)
			{
				prdSerialDetailReportBusiness.OrderGuid = (Guid)dr["OrderGuid"];
			}
			return prdSerialDetailReportBusiness;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadDataViewMode", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepository.cs", 242);
			throw;
		}
	}

	private PrdSerialReportBusiness LoadDataModel(DbDataReader dr)
	{
		try
		{
			PrdSerialReportBusiness prdSerialReportBusiness = new PrdSerialReportBusiness();
			if (dr["Guid"] != DBNull.Value)
			{
				prdSerialReportBusiness.Guid = (Guid)dr["Guid"];
			}
			if (dr["Status"] != DBNull.Value)
			{
				prdSerialReportBusiness.Status = (bool)dr["Status"];
			}
			if (dr["ProductGuid"] != DBNull.Value)
			{
				prdSerialReportBusiness.ProductGuid = (Guid)dr["ProductGuid"];
			}
			if (dr["Serial"] != DBNull.Value)
			{
				prdSerialReportBusiness.Serial = dr["Serial"].ToString();
			}
			if (dr["GarrantyCode"] != DBNull.Value)
			{
				prdSerialReportBusiness.GarrantyCode = dr["GarrantyCode"].ToString();
			}
			if (dr["StartDate"] != DBNull.Value)
			{
				prdSerialReportBusiness.StartDate = (DateTime)dr["StartDate"];
			}
			if (dr["EndDate"] != DBNull.Value)
			{
				prdSerialReportBusiness.EndDate = (DateTime)dr["EndDate"];
			}
			if (dr["ProductName"] != DBNull.Value)
			{
				prdSerialReportBusiness.ProductName = dr["ProductName"].ToString();
			}
			if (dr["ProductCode"] != DBNull.Value)
			{
				prdSerialReportBusiness.ProductCode = (long)dr["ProductCode"];
			}
			if (dr["IsMojod"] != DBNull.Value)
			{
				string text = dr["IsMojod"].ToString();
				if (text == "1" || text.ToLower() == "true")
				{
					prdSerialReportBusiness.IsMojod = true;
				}
				else
				{
					prdSerialReportBusiness.IsMojod = false;
				}
			}
			if (dr["ServerStatus"] != DBNull.Value)
			{
				prdSerialReportBusiness.ServerStatus = (ServerStatusForClient)dr["ServerStatus"];
			}
			if (dr["TechnicalInfo1"] != DBNull.Value)
			{
				prdSerialReportBusiness.TechnicalInfo1 = dr["TechnicalInfo1"].ToString();
			}
			if (dr["TechnicalInfo2"] != DBNull.Value)
			{
				prdSerialReportBusiness.TechnicalInfo2 = dr["TechnicalInfo2"].ToString();
			}
			if (dr["TechnicalInfo3"] != DBNull.Value)
			{
				prdSerialReportBusiness.TechnicalInfo3 = dr["TechnicalInfo3"].ToString();
			}
			return prdSerialReportBusiness;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadDataModel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdSerialPersistanceRepository.cs", 276);
			throw;
		}
	}
}
