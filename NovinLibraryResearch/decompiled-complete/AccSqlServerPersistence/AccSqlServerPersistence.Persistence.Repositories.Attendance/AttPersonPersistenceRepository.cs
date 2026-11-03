using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Attendance;
using PacketParser.Entities;
using Serilog;
using SharedAccEntityFramworkPersistence.Persistence.Repositories.Attendance;

namespace AccSqlServerPersistence.Persistence.Repositories.Attendance;

internal class AttPersonPersistenceRepository : AttPersonPersistenceRepositoryBase
{
	public AttPersonPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<int> CountInDepartmentAsync(Guid departmentGuid, CancellationToken token, string tranName)
	{
		BaseModelContext c = await base.dbContext.GetContextAsync(tranName, 60000, "CountInDepartmentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Attendance\\AttPersonPersistenceRepository.cs", 21);
		if (departmentGuid == Guid.Empty)
		{
			throw new ArgumentException();
		}
		int result;
		try
		{
			_ = 1;
			try
			{
				string spName = "Select Count(*) from Person Where DepartmentGuid=@DepGuid and St=@Status";
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@depGuid", (object)departmentGuid),
					new SqlParameter("@Status", (object)true)
				};
				result = await c.ExecuteScalarAsync<int>(spName, parameters, tranName, token, 30, "CountInDepartmentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Attendance\\AttPersonPersistenceRepository.cs", CommandType.Text);
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
				WebErrorLog.Instance.StartLog(ex2, "", "CountInDepartmentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Attendance\\AttPersonPersistenceRepository.cs", 38);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("CountInDepartmentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Attendance\\AttPersonPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<AttPersonBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		BaseModelContext c = await base.dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Attendance\\AttPersonPersistenceRepository.cs", 49);
		if (guid == Guid.Empty)
		{
			throw new ArgumentException();
		}
		AttPersonBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				AttPersonBusiness person = null;
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@Guid", (object)guid)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("Select * from Person Where Guid=@Guid", parameters, tranName, token, CommandType.Text, 30, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Attendance\\AttPersonPersistenceRepository.cs");
				if (await dr.ReadAsync(token))
				{
					person = LoadData(dr);
				}
				result = person;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Attendance\\AttPersonPersistenceRepository.cs", 63);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Attendance\\AttPersonPersistenceRepository.cs");
			}
		}
		return result;
	}

	private AttPersonBusiness LoadData(DbDataReader dr)
	{
		try
		{
			AttPersonBusiness attPersonBusiness = new AttPersonBusiness(base.ContextGuid);
			if (dr["Id"] != DBNull.Value)
			{
				attPersonBusiness.Id = (int)dr["Id"];
			}
			if (dr["CodeTafsil"] != DBNull.Value)
			{
				attPersonBusiness.CodeTafsil = long.Parse(dr["CodeTafsil"].ToString());
			}
			if (dr["CodeMoein"] != DBNull.Value)
			{
				attPersonBusiness.CodeMoein = long.Parse(dr["CodeMoein"].ToString());
			}
			if (dr["GroupID"] != DBNull.Value)
			{
				attPersonBusiness.GroupId = (short)dr["GroupID"];
			}
			if (dr["TitleID"] != DBNull.Value)
			{
				attPersonBusiness.TitleId = (short)dr["TitleID"];
			}
			if (dr["Name"] != DBNull.Value)
			{
				attPersonBusiness.Name = dr["Name"].ToString();
			}
			if (dr["Family"] != DBNull.Value)
			{
				attPersonBusiness.Family = dr["Family"].ToString();
			}
			if (dr["Mob1"] != DBNull.Value)
			{
				attPersonBusiness.Mob1 = dr["Mob1"].ToString().Trim();
			}
			if (dr["Mob2"] != DBNull.Value)
			{
				attPersonBusiness.Mob2 = dr["Mob2"].ToString().Trim();
			}
			if (dr["Tel1"] != DBNull.Value)
			{
				attPersonBusiness.Tel1 = dr["Tel1"].ToString().Trim();
			}
			if (dr["Tel2"] != DBNull.Value)
			{
				attPersonBusiness.Tel2 = dr["Tel2"].ToString().Trim();
			}
			if (dr["Adress"] != DBNull.Value)
			{
				attPersonBusiness.Address = dr["Adress"].ToString();
			}
			if (dr["Description"] != DBNull.Value)
			{
				attPersonBusiness.Description = dr["Description"].ToString();
			}
			if (dr["CodeMeli"] != DBNull.Value)
			{
				attPersonBusiness.CodeMeli = dr["CodeMeli"].ToString();
			}
			if (dr["ZipCode"] != DBNull.Value)
			{
				attPersonBusiness.ZipCode = dr["ZipCode"].ToString();
			}
			if (dr["SHSH"] != DBNull.Value)
			{
				attPersonBusiness.Shsh = dr["SHSH"].ToString();
			}
			if (dr["SodorSH"] != DBNull.Value)
			{
				attPersonBusiness.SodorSh = dr["SodorSH"].ToString();
			}
			if (dr["FatherName"] != DBNull.Value)
			{
				attPersonBusiness.FatherName = dr["FatherName"].ToString();
			}
			if (dr["SerialSH"] != DBNull.Value)
			{
				attPersonBusiness.SerialSh = dr["SerialSH"].ToString();
			}
			if (dr["Jensiat"] != DBNull.Value)
			{
				attPersonBusiness.Jensiat = (short)dr["Jensiat"];
			}
			if (dr["DateMBirthday"] != DBNull.Value)
			{
				attPersonBusiness.DateMBirthday = (DateTime)dr["DateMBirthday"];
			}
			if (dr["NOBimeh"] != DBNull.Value)
			{
				attPersonBusiness.NoBimeh = dr["NOBimeh"].ToString();
			}
			if (dr["MadrakTahsili"] != DBNull.Value)
			{
				attPersonBusiness.MadrakTahsili = dr["MadrakTahsili"].ToString();
			}
			if (dr["DateMStartWork"] != DBNull.Value)
			{
				attPersonBusiness.DateMStartWork = (DateTime)dr["DateMStartWork"];
			}
			if (dr["DateMFinishW"] != DBNull.Value)
			{
				attPersonBusiness.DateMFinishW = (DateTime)dr["DateMFinishW"];
			}
			if (dr["EisarID"] != DBNull.Value)
			{
				attPersonBusiness.EisarId = (short)dr["EisarID"];
			}
			if (dr["Hid"] != DBNull.Value)
			{
				attPersonBusiness.Hid = (int)dr["Hid"];
			}
			if (dr["HType"] != DBNull.Value)
			{
				attPersonBusiness.HType = (short)dr["HType"];
			}
			if (dr["Type"] != DBNull.Value)
			{
				attPersonBusiness.Type = (short)dr["Type"];
			}
			if (dr["FullCode"] != DBNull.Value)
			{
				attPersonBusiness.FullCode = dr["FullCode"].ToString();
			}
			if (dr["St"] != DBNull.Value)
			{
				attPersonBusiness.St = (bool)dr["St"];
			}
			if (dr["Guid"] != DBNull.Value)
			{
				attPersonBusiness.Guid = (Guid)dr["Guid"];
			}
			if (dr["PicPath"] != DBNull.Value)
			{
				attPersonBusiness.PicPath = dr["PicPath"].ToString();
			}
			if (dr["MTavalod"] != DBNull.Value)
			{
				attPersonBusiness.MTavalod = dr["MTavalod"].ToString();
			}
			if (dr["UserName"] != DBNull.Value)
			{
				attPersonBusiness.UserName = dr["UserName"].ToString();
			}
			if (dr["HashPassword"] != DBNull.Value)
			{
				attPersonBusiness.HashPassword = (byte[])dr["HashPassword"];
			}
			if (dr["LatinFullName"] != DBNull.Value)
			{
				attPersonBusiness.LatinFullName = dr["LatinFullName"].ToString();
			}
			return attPersonBusiness;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}
}
