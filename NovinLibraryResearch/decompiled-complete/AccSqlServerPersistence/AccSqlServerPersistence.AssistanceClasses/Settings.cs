using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;

namespace AccSqlServerPersistence.AssistanceClasses;

public class Settings
{
	public static string ConnectionString = "";

	private static string _ServerConnectionsString;

	public static string _destinationFileFolder = "";

	private static string FileName => Path.Combine(".", "cn_Server.txt");

	public static string ServerConnectionsString
	{
		get
		{
			try
			{
				if (_ServerConnectionsString == null)
				{
					if (File.Exists(FileName))
					{
						_ServerConnectionsString = File.ReadAllText(FileName);
					}
					else
					{
						_ServerConnectionsString = "Data Source=.;Integrated Security=True;MultipleActiveResultSets=True";
						File.Create(FileName).Close();
						File.WriteAllText(FileName, _ServerConnectionsString);
					}
				}
				return _ServerConnectionsString;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ServerConnectionsString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\Settings.cs", 37);
				return "";
			}
		}
		set
		{
			try
			{
				_ServerConnectionsString = value;
				if (!File.Exists(FileName))
				{
					File.Create(FileName).Close();
				}
				File.WriteAllText(FileName, _ServerConnectionsString);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ServerConnectionsString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\Settings.cs", 50);
			}
		}
	}

	public static async Task<bool> SetDestinationFolderAsync(string serverConnection)
	{
		int line = 0;
		string msg = "";
		try
		{
			string text = Assembly.GetEntryAssembly()?.Location;
			msg = msg + text + "\r\n";
			if (text == null)
			{
				return true;
			}
			FileInfo fileInfo = new FileInfo(text);
			line = 1;
			if (fileInfo.DirectoryName == null)
			{
				return true;
			}
			string path2 = Path.Combine(fileInfo.DirectoryName, "DataBase");
			msg = msg + path2 + "\r\n";
			line = 2;
			if (!Directory.Exists(path2))
			{
				Directory.CreateDirectory(path2);
			}
			line = 3;
			if (path2.ToLower().StartsWith("c:\\") || path2.ToLower().StartsWith("d:\\projects"))
			{
				return false;
			}
			line = 4;
			SqlConnection cn = new SqlConnection(serverConnection);
			try
			{
				line = 5;
				SqlCommand cmd = new SqlCommand("", cn);
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
					line = 6;
					path2 = "N'" + path2 + "'";
					((DbCommand)(object)cmd).CommandText = "EXEC xp_instance_regwrite N'HKEY_LOCAL_MACHINE', N'Software\\Microsoft\\MSSQLServer\\MSSQLServer', N'DefaultLog', REG_SZ, " + path2;
					line = 7;
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync();
					line = 8;
					((DbCommand)(object)cmd).CommandText = "EXEC xp_instance_regwrite N'HKEY_LOCAL_MACHINE', N'Software\\Microsoft\\MSSQLServer\\MSSQLServer', N'DefaultData', REG_SZ," + path2;
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync();
					line = 9;
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
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog($"line={line},serverConnection={serverConnection}, msg={msg}\r\n ex={ex}", "", "SetDestinationFolderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\Settings.cs", 98);
			return false;
		}
	}
}
