using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Settings;

namespace TimeAttendanceDAL.Dal.Device;

[Serializable]
[DoNotPruneType]
public class EventArgsNotify : EventArgs, IHasContext
{
	public string ContextGuid { get; set; }

	public int Id { get; private set; }

	public string Message { get; set; }

	public string Title { get; set; }

	public long ObjectId { get; set; }

	public NotificationEnum NotificationEnum { get; set; }

	public string NotificationEnumDesc => Utilities.GetDisplay<NotificationEnum>(NotificationEnum);

	public short Secounts { get; set; }

	public bool St { get; set; } = true;


	public EventNotifyEnumType StatusNotify { get; set; } = (EventNotifyEnumType)1;


	public DateTime DateM { get; set; } = DateTime.Now;


	public string DateSh => Calendar.MiladiToCalture(DateM);

	public string Time => DateM.ToString("HH:mm");

	public bool Check { get; set; }

	public EventArgsNotify(string contextGuid)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		ContextGuid = contextGuid;
	}

	public static async Task<EventArgsNotify> GetAsync(string contextGuid, long objId, string message, string title, NotificationEnum notificationEnum, short secounts = 3, CancellationToken token = default(CancellationToken))
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			EventArgsNotify ret = new EventArgsNotify
			{
				ContextGuid = contextGuid,
				Message = message,
				Title = title,
				NotificationEnum = notificationEnum,
				Secounts = secounts,
				ObjectId = objId
			};
			await ret.SaveAsync(token);
			return ret;
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

	public static async Task<EventArgsNotify> GetAsync(string contextGuid, int id, CancellationToken token)
	{
		EventArgsNotify ret = new EventArgsNotify
		{
			ContextGuid = contextGuid
		};
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			SqlCommand cmd = new SqlCommand("sp_EventArgsNotify_SelectRow", cn);
			cmd.Parameters.AddWithValue("Id", (object)id);
			((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
			await ((DbConnection)(object)cn).OpenAsync(token);
			SqlDataReader dr = cmd.ExecuteReader();
			if (await ((DbDataReader)(object)dr).ReadAsync(token))
			{
				ret.LoadData(dr);
			}
			((DbConnection)(object)cn).Close();
			return ret;
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

	public EventArgsNotify(string contextGuid, SqlDataReader dr)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		ContextGuid = contextGuid;
		LoadData(dr);
	}

	public EventArgsNotify()
	{
	}//IL_0009: Unknown result type (might be due to invalid IL or missing references)


	private void LoadData(SqlDataReader dr)
	{
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (((DbDataReader)(object)dr)["Id"].ToString() != DBNull.Value.ToString())
			{
				Id = (int)((DbDataReader)(object)dr)["Id"];
			}
			if (((DbDataReader)(object)dr)["Message"].ToString() != DBNull.Value.ToString())
			{
				Message = ((DbDataReader)(object)dr)["Message"].ToString();
			}
			if (((DbDataReader)(object)dr)["Title"].ToString() != DBNull.Value.ToString())
			{
				Title = ((DbDataReader)(object)dr)["Title"].ToString();
			}
			if (((DbDataReader)(object)dr)["ObjectId"].ToString() != DBNull.Value.ToString())
			{
				ObjectId = (int)((DbDataReader)(object)dr)["ObjectId"];
			}
			if (((DbDataReader)(object)dr)["NotificationEnum"].ToString() != DBNull.Value.ToString())
			{
				NotificationEnum = (NotificationEnum)((DbDataReader)(object)dr)["NotificationEnum"];
			}
			if (((DbDataReader)(object)dr)["Secounts"].ToString() != DBNull.Value.ToString())
			{
				Secounts = (short)((DbDataReader)(object)dr)["Secounts"];
			}
			if (((DbDataReader)(object)dr)["St"].ToString() != DBNull.Value.ToString())
			{
				St = (bool)((DbDataReader)(object)dr)["St"];
			}
			if (((DbDataReader)(object)dr)["DateM"].ToString() != DBNull.Value.ToString())
			{
				DateM = (DateTime)((DbDataReader)(object)dr)["DateM"];
			}
			if (((DbDataReader)(object)dr)["StatusNotify"].ToString() != DBNull.Value.ToString())
			{
				StatusNotify = (EventNotifyEnumType)((DbDataReader)(object)dr)["StatusNotify"];
			}
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

	public async Task<bool> SaveAsync(CancellationToken token)
	{
		_ = 2;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("Message", (object)Text.FixString(Message));
					cmd.Parameters.AddWithValue("Title", (object)Text.FixString(Title));
					cmd.Parameters.AddWithValue("ObjectId", (object)ObjectId);
					cmd.Parameters.AddWithValue("NotificationEnum", (object)NotificationEnum);
					cmd.Parameters.AddWithValue("Secounts", (object)Secounts);
					cmd.Parameters.AddWithValue("St", (object)St.ToString());
					cmd.Parameters.AddWithValue("DateM", (object)DateM);
					cmd.Parameters.AddWithValue("StatusNotify", (object)StatusNotify);
					await ((DbConnection)(object)cn).OpenAsync(token);
					if (Id <= 0)
					{
						((DbCommand)(object)cmd).CommandText = "dbo.sp_EventArgsNotify_Insert";
						object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
						if (obj != null)
						{
							Id = Utilities.ParseToInt(obj.ToString(), 0);
						}
					}
					else
					{
						((DbCommand)(object)cmd).CommandText = "dbo.sp_EventArgsNotify_Update";
						cmd.Parameters.AddWithValue("Id", (object)Id);
						await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					}
					((DbConnection)(object)cn).Close();
					return true;
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
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

	public static async Task<bool> DeleteAsync(string contextGuid, EventArgsNotify notify, CancellationToken token)
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("[dbo].[sp_EventArgsNotify_DeleteRow]", cn);
				try
				{
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("Id", (object)notify.Id);
					await ((DbConnection)(object)cn).OpenAsync(token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					((DbConnection)(object)cn).Close();
					return true;
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
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

	public static async Task<List<EventArgsNotify>> SelectAllAsync(string contextGuid, DateTime date1, DateTime date2, EventNotifyEnumType statusNotify, CancellationToken token)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		List<EventArgsNotify> ret = new List<EventArgsNotify>();
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_EventArgsNotify_SelectAll]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					if (date1 > ConstantValues.MinDateM)
					{
						cmd.Parameters.AddWithValue("date1", (object)date1);
						cmd.Parameters.AddWithValue("date2", (object)date2);
						cmd.Parameters.AddWithValue("statusNotify", (object)(EventNotifyEnumType)0);
					}
					else
					{
						cmd.Parameters.AddWithValue("statusNotify", (object)statusNotify);
					}
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret.Add(new EventArgsNotify(contextGuid, dr));
						}
						((DbConnection)(object)cn).Close();
					}
					finally
					{
						((IDisposable)dr)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
			return ret;
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

	public static async Task<int> CountAllAsync(string contextGuid, EventNotifyEnumType statusNotify, CancellationToken token)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_EventArgsNotify_CountAll]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("statusNotify", (object)statusNotify);
					await ((DbConnection)(object)cn).OpenAsync(token);
					object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
					((DbConnection)(object)cn).Close();
					if (obj != null)
					{
						return Utilities.ParseToInt(obj.ToString(), 0);
					}
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
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
		return 0;
	}
}
