using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace TimeAttendanceDAL.Dal.Taradod;

public class PayamakDatabase
{
	public static async Task<bool> ExistPayamakDataBaseAsync(string pConnString, CancellationToken token)
	{
		int recno = 0;
		try
		{
			SqlConnection cn = new SqlConnection(pConnString);
			try
			{
				SqlCommand val = new SqlCommand("SELECT count(1)  FROM sys.objects WHERE[type] in ('U') and([name] = 'SmsOutBox' or[name] =  'SMSNet_Account')", cn);
				((DbCommand)val).CommandType = CommandType.Text;
				SqlCommand cmd = val;
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
				}
				catch
				{
					return false;
				}
				SqlDataReader dr = cmd.ExecuteReader();
				if (await ((DbDataReader)(object)dr).ReadAsync(token) && ((DbDataReader)(object)dr)[0] != DBNull.Value)
				{
					recno = (int)((DbDataReader)(object)dr)[0];
				}
				((DbConnection)(object)cn).Close();
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
		}
		catch (ArgumentException ex)
		{
			Log.Error((Exception)ex, "ArgumentException");
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
		return recno >= 2;
	}
}
