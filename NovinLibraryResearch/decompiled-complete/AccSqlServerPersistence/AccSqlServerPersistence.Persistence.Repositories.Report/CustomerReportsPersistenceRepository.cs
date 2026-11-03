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
using EntityCache.Business.Customers;
using EntityCache.Core.Repository.Accounting;
using PacketParser;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Persistence.Repositories.Report;

internal class CustomerReportsPersistenceRepository : CustomerReportsPersistenceRepositoryBase
{
	public CustomerReportsPersistenceRepository(ContextHolder context, ITableRelationShipRepository attachmentRepo)
		: base(context, attachmentRepo)
	{
	}

	public override async Task<IEnumerable<RasBedahiBusiness>> GetAllRasBedahi(string contextGuid, DateTime Date1, DateTime Date2, short darsad, CancellationToken token, string tranName = "")
	{
		using (new JaegerMonitor("GetAllRasBedahi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\CustomerReportsPersistenceRepository.cs"))
		{
			BaseModelContext c = await base.DatabaseContext.GetContextAsync(tranName, 60000, "GetAllRasBedahi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\CustomerReportsPersistenceRepository.cs", 24);
			IEnumerable<RasBedahiBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					List<RasBedahiBusiness> list = new List<RasBedahiBusiness>();
					List<SqlParameter> parameters = new List<SqlParameter>
					{
						new SqlParameter("@Date1", (object)Date1),
						new SqlParameter("@Date2", (object)Date2),
						new SqlParameter("@Darsad", (object)darsad),
						new SqlParameter("@DigitAfterPoint", (object)ClsCache.GetCache(contextGuid).MultiLanguageConfig.DigitAfterPoint)
					};
					using DbDataReader dr = await c.ExecuteReaderAsync("sp_Customer_BedehiGeneration", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllRasBedahi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\CustomerReportsPersistenceRepository.cs");
					while (true)
					{
						if (await dr.ReadAsync(token))
						{
							if (token.IsCancellationRequested)
							{
								result = null;
								break;
							}
							RasBedahiBusiness rasBedahiBusiness = LoadDataRasBedahi(dr);
							if (rasBedahiBusiness != null)
							{
								list.Add(rasBedahiBusiness);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllRasBedahi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\CustomerReportsPersistenceRepository.cs", 49);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllRasBedahi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\CustomerReportsPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<List<TafsilCurrencyGardeshModel>> GetAllTafsilCurrencyAsync(Guid tafsilGuid, DateTime? date1, DateTime? date2, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllTafsilCurrencyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\CustomerReportsPersistenceRepository.cs"))
		{
			BaseModelContext c = await base.DatabaseContext.GetContextAsync(tranName, 60000, "GetAllTafsilCurrencyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\CustomerReportsPersistenceRepository.cs", 61);
			List<TafsilCurrencyGardeshModel> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					List<TafsilCurrencyGardeshModel> list = new List<TafsilCurrencyGardeshModel>();
					List<SqlParameter> parameters = new List<SqlParameter>
					{
						new SqlParameter("@TafsilGuid", (object)tafsilGuid),
						new SqlParameter("@Date1", (object)date1),
						new SqlParameter("@Date2", (object)date2)
					};
					using DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_Customer_GardeshArzi", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllTafsilCurrencyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\CustomerReportsPersistenceRepository.cs");
					while (true)
					{
						if (await dr.ReadAsync(token))
						{
							if (token.IsCancellationRequested)
							{
								result = null;
								break;
							}
							TafsilCurrencyGardeshModel tafsilCurrencyGardeshModel = LoadDataTafsilCurrency(dr);
							if (tafsilCurrencyGardeshModel != null)
							{
								list.Add(tafsilCurrencyGardeshModel);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllTafsilCurrencyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\CustomerReportsPersistenceRepository.cs", 85);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllTafsilCurrencyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\CustomerReportsPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<List<CustomerVisitPriodModel>> GetAllCustomerVisitPriodAsync(CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllCustomerVisitPriodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\CustomerReportsPersistenceRepository.cs"))
		{
			BaseModelContext c = await base.DatabaseContext.GetContextAsync(tranName, 60000, "GetAllCustomerVisitPriodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\CustomerReportsPersistenceRepository.cs", 97);
			List<CustomerVisitPriodModel> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					List<CustomerVisitPriodModel> list2 = new List<CustomerVisitPriodModel>();
					using DbDataReader dr = await c.ExecuteReaderAsync("sp_GetCustomerVisitPriod", null, tranName, token, CommandType.StoredProcedure, 30, "GetAllCustomerVisitPriodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\CustomerReportsPersistenceRepository.cs");
					while (true)
					{
						if (await dr.ReadAsync(token))
						{
							if (token.IsCancellationRequested)
							{
								result = null;
								break;
							}
							CustomerVisitPriodModel customerVisitPriodModel = LoadDataVisitPriod(dr);
							if (customerVisitPriodModel != null)
							{
								list2.Add(customerVisitPriodModel);
							}
							continue;
						}
						dr.Close();
						list2 = list2?.Where((CustomerVisitPriodModel q) => q.Term > 0)?.ToList();
						result = list2;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllCustomerVisitPriodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\CustomerReportsPersistenceRepository.cs", 116);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllCustomerVisitPriodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\CustomerReportsPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	private TafsilCurrencyGardeshModel LoadDataTafsilCurrency(DbDataReader dr)
	{
		try
		{
			TafsilCurrencyGardeshModel tafsilCurrencyGardeshModel = new TafsilCurrencyGardeshModel();
			if (dr["SanadNo"] != DBNull.Value)
			{
				tafsilCurrencyGardeshModel.SanadNo = (int)dr["SanadNo"];
			}
			if (dr["Date"] != DBNull.Value)
			{
				tafsilCurrencyGardeshModel.DateM = (DateTime)dr["Date"];
			}
			if (dr["DecimalName"] != DBNull.Value)
			{
				tafsilCurrencyGardeshModel.DecimalName = dr["DecimalName"].ToString();
			}
			if (dr["DecimalGuid"] != DBNull.Value)
			{
				tafsilCurrencyGardeshModel.DecimalGuid = (Guid)dr["DecimalGuid"];
			}
			if (dr["TafsilGuid"] != DBNull.Value)
			{
				tafsilCurrencyGardeshModel.TafsilGuid = (Guid)dr["TafsilGuid"];
			}
			if (dr["AsGuid"] != DBNull.Value)
			{
				tafsilCurrencyGardeshModel.AsGuid = (Guid)dr["AsGuid"];
			}
			if (dr["AsType"] != DBNull.Value)
			{
				tafsilCurrencyGardeshModel.AsType = (Document)dr["AsType"];
			}
			if (dr["Description"] != DBNull.Value)
			{
				tafsilCurrencyGardeshModel.Description = dr["Description"].ToString();
			}
			if (dr["Debit"] != DBNull.Value)
			{
				tafsilCurrencyGardeshModel.Debit = (decimal)(double)dr["Debit"];
			}
			if (dr["Credit"] != DBNull.Value)
			{
				tafsilCurrencyGardeshModel.Credit = (decimal)(double)dr["Credit"];
			}
			if (dr["Mandeh"] != DBNull.Value)
			{
				tafsilCurrencyGardeshModel.Rem = (decimal)(double)dr["Mandeh"];
			}
			return tafsilCurrencyGardeshModel;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadDataTafsilCurrency", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\CustomerReportsPersistenceRepository.cs", 147);
			throw;
		}
	}

	private RasBedahiBusiness LoadDataRasBedahi(DbDataReader dr)
	{
		try
		{
			RasBedahiBusiness rasBedahiBusiness = new RasBedahiBusiness();
			if (dr["Guid"] != DBNull.Value)
			{
				rasBedahiBusiness.CustomerGuid = (Guid)dr["Guid"];
			}
			if (dr["code"] != DBNull.Value)
			{
				rasBedahiBusiness.TafsilAccount = (decimal)dr["Account"];
			}
			if (dr["Fullname"] != DBNull.Value)
			{
				rasBedahiBusiness.Fullname = dr["Fullname"].ToString();
			}
			if (dr["GroupGuid"] != DBNull.Value)
			{
				rasBedahiBusiness.GroupGuid = (Guid)dr["GroupGuid"];
			}
			if (dr["RasDays"] != DBNull.Value)
			{
				rasBedahiBusiness.RasDays = (decimal)dr["RasDays"];
			}
			if (dr["TotalSood"] != DBNull.Value)
			{
				rasBedahiBusiness.TotalSood = (double)dr["TotalSood"];
			}
			if (dr["DateRas"] != DBNull.Value)
			{
				rasBedahiBusiness.DateRas = dr["DateRas"].ToString();
			}
			if (dr["code"] != DBNull.Value)
			{
				rasBedahiBusiness.TafsilCode = (long)dr["code"];
			}
			return rasBedahiBusiness;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadDataRasBedahi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\CustomerReportsPersistenceRepository.cs", 170);
			throw;
		}
	}

	private CustomerVisitPriodModel LoadDataVisitPriod(DbDataReader dr)
	{
		try
		{
			CustomerVisitPriodModel customerVisitPriodModel = new CustomerVisitPriodModel();
			if (dr["TafsilGuid"] != DBNull.Value)
			{
				customerVisitPriodModel.TafsilGuid = (Guid)dr["TafsilGuid"];
			}
			if (dr["LastVisitTime"] != DBNull.Value)
			{
				customerVisitPriodModel.LastVisitDateM = (DateTime)dr["LastVisitTime"];
			}
			if (dr["NextVisitTime"] != DBNull.Value)
			{
				customerVisitPriodModel.NextVisitDateM = (DateTime)dr["NextVisitTime"];
			}
			if (dr["PriodType"] != DBNull.Value)
			{
				customerVisitPriodModel.PriodType = (EnVisitPeriod)dr["PriodType"].ToString().ParseToShort(0);
			}
			if (dr["TafsilName"] != DBNull.Value)
			{
				customerVisitPriodModel.TafsilName = dr["TafsilName"].ToString();
			}
			if (dr["Term"] != DBNull.Value)
			{
				customerVisitPriodModel.Term = dr["Term"].ToString().ParseToInt();
			}
			if (dr["Account"] != DBNull.Value)
			{
				customerVisitPriodModel.Account = (decimal)dr["Account"];
			}
			if (dr["TafsilCode"] != DBNull.Value)
			{
				customerVisitPriodModel.TafsilCode = (long)dr["TafsilCode"];
			}
			if (dr["TafsilNumber"] != DBNull.Value)
			{
				long num = (long)dr["TafsilNumber"];
				if (num > 0)
				{
					customerVisitPriodModel.TafsilNumber = num.ToString();
				}
			}
			return customerVisitPriodModel;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadDataVisitPriod", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Report\\CustomerReportsPersistenceRepository.cs", 198);
			throw;
		}
	}
}
