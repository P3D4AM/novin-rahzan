using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Attendance.LaborLaw;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceDAL.Logger;
using TimeAttendanceDAL.Settings;

namespace TimeAttendanceDAL.Dal;

public class Initialize
{
	public static async Task<ReturnedSaveFuncInfo> UpdateSomeDataAsync(string contextGuid, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			AttLogger.LoggerUi.Information("WebTaxTable.GetFromApi is working");
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebTaxTable>>> response = await WebTaxTable.GetFromApi(default(CancellationToken));
			if (((ReturnedSaveFuncInfo)response).HasError || response.value == null)
			{
				ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)response, (ReturnedState)0, "UpdateSomeDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Initialize.cs", 26);
				return ret;
			}
			if (response.value.HttpClietnStatusCode != HttpStatusCode.OK)
			{
				ret.AddReturnedHttpStatusCode(response.value.HttpClietnStatusCode, (Exception)null, "UpdateSomeDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Initialize.cs", 31);
				if (!string.IsNullOrEmpty(response.value.Description))
				{
					ret.AddError((EnSystem)0, response.value.Description, "", "UpdateSomeDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Initialize.cs", 33, (StringBuilder)null);
				}
				return ret;
			}
			AttLogger.LoggerUi.Information("WebTaxTable.GetFromApi is ok");
			List<WebTaxTable> data = response.value.Data;
			if (data != null && data.Count > 0)
			{
				AttLogger.LoggerUi.Information("save TaxTable is working");
				SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					foreach (TaxTable item in response.value.Data.Select((WebTaxTable taxTable) => new TaxTable(contextGuid)
					{
						StartDateM = taxTable.StartDateM,
						EndDateM = taxTable.EndDateM,
						Year = taxTable.Year,
						FromPrice = taxTable.FromPrice,
						ToPrice = taxTable.ToPrice,
						TaxPercentage = taxTable.TaxPercentage,
						InitialValue = taxTable.InitialValue,
						Description = taxTable.Description
					}))
					{
						await item.SaveAsync(cn, token);
					}
					((DbConnection)(object)cn).Close();
					AttLogger.LoggerUi.Information("save TaxTable is ok");
					AttLogger.LoggerUi.Information("TimeAttendanceSettings save field IsTaxTableUpdated is working");
					TimeAttendanceSettings obj = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(contextGuid, token);
					obj.UpdateDetails.IsTaxTableUpdated = true;
					await obj.SaveAsync(contextGuid, token);
					AttLogger.LoggerUi.Information("TimeAttendanceSettings save field IsTaxTableUpdated is ok");
				}
				finally
				{
					((IDisposable)cn)?.Dispose();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "UpdateSomeDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Initialize.cs", 70);
			throw;
		}
		return ret;
	}
}
