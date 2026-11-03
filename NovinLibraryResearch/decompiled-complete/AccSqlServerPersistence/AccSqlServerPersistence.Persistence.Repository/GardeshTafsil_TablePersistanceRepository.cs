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
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Persistence.Repository;

internal class GardeshTafsil_TablePersistanceRepository : GardeshTafsil_TablePersistanceRepositoryBase
{
	public GardeshTafsil_TablePersistanceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<ReturnedSaveFuncInfo> DeleteRangeAsync(IEnumerable<Guid> kolGuid, IEnumerable<Guid> moeinGuid, IEnumerable<Guid?> tafsilGuid, short tryCount, string tranName, CancellationToken token)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepository.cs", 25));
			try
			{
				if (tafsilGuid == null || !tafsilGuid.Any())
				{
					return ret;
				}
				foreach (Guid? item in tafsilGuid)
				{
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepository.cs", 32);
					}
					if (item.HasValue)
					{
						List<SqlParameter> parameters = new List<SqlParameter>
						{
							new SqlParameter("@TafsilGuid", (object)item)
						};
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await dbcontext.ExecuteNoneQueryAsync("sp_Acc_GardeshTafsil_DeleteByTafsilGuid", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepository.cs"), ReturnedState.None, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepository.cs", 35);
					}
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepository.cs", 38);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepository.cs", 41);
				ret.AddReturnedValue(ex, "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepository.cs", 42);
			}
			return ret;
		}
	}

	public override async Task<IEnumerable<GardeshTafsilBusiness>> GetAllFromVSanadDetAsync(List<Guid> listTafsilGuid, EnSystem system, int top, DateTime? date1, DateTime? date2, List<Guid> listMoeinGuid, Guid? kolGuid, bool? status, string tranName, CancellationToken token)
	{
		using (new JaegerMonitor("GetAllFromVSanadDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllFromVSanadDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepository.cs", 49));
			IEnumerable<GardeshTafsilBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 3;
				try
				{
					if (listTafsilGuid == null || listTafsilGuid.Count <= 0)
					{
						result = new List<GardeshTafsilBusiness>();
					}
					else
					{
						if (listMoeinGuid == null || listMoeinGuid.Count <= 0)
						{
							listMoeinGuid = new List<Guid>();
						}
						string text = string.Join(",", listTafsilGuid);
						string text2 = ((listMoeinGuid.Count > 0) ? string.Join(",", listMoeinGuid) : "");
						List<GardeshTafsilBusiness> list = new List<GardeshTafsilBusiness>();
						List<SqlParameter> list2 = new List<SqlParameter>
						{
							new SqlParameter("@ListTafsilGuid", (object)text),
							new SqlParameter("@ListMoeinGuid", (object)text2),
							new SqlParameter("@Top", (object)top)
						};
						if (date1.HasValue)
						{
							list2.Add(new SqlParameter("@Date1", (object)date1));
						}
						if (date2.HasValue)
						{
							list2.Add(new SqlParameter("@Date2", (object)date2));
						}
						using DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_GetGardeshTafsil", list2, tranName, token, CommandType.StoredProcedure, 30, "GetAllFromVSanadDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepository.cs");
						int rowIndex = 0;
						while (await dr.ReadAsync())
						{
							GardeshTafsilBusiness gardeshTafsilBusiness = await LoadFromDataReaderAsync(dr, system, setProduct: false);
							rowIndex = (gardeshTafsilBusiness.RowIndex = rowIndex + 1);
							list.Add(gardeshTafsilBusiness);
						}
						dr.Close();
						if (system == EnSystem.UserInterface)
						{
							await Task.WhenAll(list.Select((GardeshTafsilBusiness p) => p.SetPrdAsync(system)));
						}
						result = list;
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetAllFromVSanadDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepository.cs", 82);
					result = new List<GardeshTafsilBusiness>();
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllFromVSanadDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	private async Task<GardeshTafsilBusiness> LoadFromDataReaderAsync(DbDataReader dr, EnSystem system, bool setProduct)
	{
		GardeshTafsilBusiness obj = new GardeshTafsilBusiness(ContextGuid);
		using (new JaegerMonitor("LoadFromDataReaderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepository.cs"))
		{
			try
			{
				obj.Guid = Guid.NewGuid();
				if (dr["Date"] != DBNull.Value)
				{
					obj.Date = (DateTime)dr["Date"];
				}
				if (dr["ASGuid"] != DBNull.Value)
				{
					obj.AsGuid = (Guid)dr["ASGuid"];
				}
				if (dr["ASTYPE"] != DBNull.Value)
				{
					obj.AsType = dr["ASTYPE"].ToString().ParseToShort(0);
				}
				if (dr["DEBIT"] != DBNull.Value)
				{
					obj.Debit = (decimal)dr["DEBIT"];
				}
				if (dr["CREDIT"] != DBNull.Value)
				{
					obj.Credit = (decimal)dr["CREDIT"];
				}
				if (dr["UserGuid"] != DBNull.Value)
				{
					obj.UGuid = (Guid)dr["UserGuid"];
				}
				if (dr["SANADNO"] != DBNull.Value)
				{
					obj.SanadNO = (int)dr["SANADNO"];
				}
				if (dr["SANADST"] != DBNull.Value)
				{
					obj.SanadSt = (int)dr["SANADST"];
				}
				if (dr["MoeinGuid"] != DBNull.Value)
				{
					obj.MoeinGuid = (Guid)dr["MoeinGuid"];
				}
				if (dr["TafsilGuid"] != DBNull.Value)
				{
					obj.TafsilGuid = (Guid)dr["TafsilGuid"];
				}
				if (dr["ID2"] != DBNull.Value)
				{
					obj.Id2 = (int)dr["ID2"];
				}
				if (dr["QT"] != DBNull.Value)
				{
					obj.QT = (decimal)dr["QT"];
				}
				if (dr["Price"] != DBNull.Value)
				{
					obj.Price = (decimal)dr["Price"];
				}
				if (dr["QT2"] != DBNull.Value)
				{
					obj.QT2 = (decimal)dr["QT2"];
				}
				if (dr["RDiscount"] != DBNull.Value)
				{
					obj.Discount = (decimal)dr["RDiscount"];
				}
				if (dr["RBazaryab"] != DBNull.Value)
				{
					obj.Bazaryab = (decimal)dr["RBazaryab"];
				}
				if (dr["Serial"] != DBNull.Value)
				{
					obj.Serial = dr["Serial"].ToString();
				}
				if (dr["SanadDesc"] != DBNull.Value)
				{
					obj.SanadDesc = dr["SanadDesc"].ToString();
				}
				if (dr["RDesc"] != DBNull.Value)
				{
					obj.RowDesc = dr["RDesc"].ToString();
				}
				if (dr["PRDGuid"] != DBNull.Value)
				{
					await obj.SetPrdGuidAsync((Guid)dr["PRDGuid"], system, setProduct);
				}
				if (dr["DPTGuid"] != DBNull.Value)
				{
					obj.DepotGuid = (Guid)dr["DPTGuid"];
				}
				if (dr["VGuid"] != DBNull.Value)
				{
					obj.VGuid = (Guid)dr["VGuid"];
				}
				if (dr["DateMoaser"] != DBNull.Value)
				{
					obj.DateMoaser = (DateTime)dr["DateMoaser"];
				}
				if (dr["DecimalGuid"] != DBNull.Value)
				{
					obj.DecimalGuid = (Guid)dr["DecimalGuid"];
				}
				if (dr["DecimalPrice"] != DBNull.Value)
				{
					obj.DecimalPrice = (decimal)dr["DecimalPrice"];
				}
				if (dr["DecimalQT"] != DBNull.Value)
				{
					obj.DecimalQt = (decimal)dr["DecimalQT"];
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
				WebErrorLog.Instance.StartLog(ex2, "", "LoadFromDataReaderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\GardeshTafsil_TablePersistanceRepository.cs", 134);
				throw;
			}
			return obj;
		}
	}
}
