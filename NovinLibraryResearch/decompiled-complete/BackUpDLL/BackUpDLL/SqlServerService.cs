using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Smo.Wmi;
using PacketParser.Entities;

namespace BackUpDLL;

public class SqlServerService
{
	public static async Task<(bool, List<string>)> IsSqlServiceOkAsync(string connectionstring)
	{
		if (string.IsNullOrEmpty(connectionstring))
		{
			throw new ArgumentNullException("connectionstring");
		}
		try
		{
			(bool, List<string>) result = await CheckConnectionAsync(connectionstring);
			if (result.Item1)
			{
				return result;
			}
			SqlConnectionStringBuilder val = new SqlConnectionStringBuilder(connectionstring);
			if (string.IsNullOrEmpty(val.DataSource))
			{
				return (false, new List<string> { "رشته اتصال نامعتبر است" });
			}
			if (IPAddress.TryParse(val.DataSource, out IPAddress address) && val.DataSource != "127.0.0.1" && !RemoteServerIsAlive(address))
			{
				return (false, new List<string> { "سیستم میزبان sql server در دسترس نیست" });
			}
			ServiceCollection services = new ManagedComputer().Services;
			List<string> list = new List<string>();
			bool item = false;
			bool flag = false;
			foreach (Service item2 in services)
			{
				Service val2 = item2;
				ManagedServiceType type = val2.Type;
				if (!((object)(ManagedServiceType)(ref type)).Equals((object)(ManagedServiceType)1))
				{
					continue;
				}
				flag = true;
				switch (WindowsServiceHelper.GetServiceStatus(((WmiSmoObject)val2).Name))
				{
				case ServiceState.NotFound:
					list.Add("سرویس " + val2.DisplayName + " پیدا نشد.");
					break;
				case ServiceState.Stopped:
					try
					{
						WindowsServiceHelper.StartService(((WmiSmoObject)val2).Name);
						list.Add("سرویس " + val2.DisplayName + " اجرا شد.");
						item = true;
					}
					catch (Exception ex)
					{
						list.Add(" خطا در اجرای سرویس " + val2.DisplayName + ".");
						list.Add(ex.Message);
						item = true;
					}
					break;
				default:
					item = true;
					break;
				}
				Console.WriteLine(val2.DisplayName);
			}
			if (!flag)
			{
				list.Add("سرویس sql پیدا نشد");
			}
			return (item, list);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "IsSqlServiceOkAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\SqlServerService.cs", 72);
			return (false, new List<string> { ex2.Message });
		}
	}

	private static async Task<(bool, List<string>)> CheckConnectionAsync(string connectionString)
	{
		try
		{
			SqlConnection connection = new SqlConnection(connectionString);
			try
			{
				await ((DbConnection)(object)connection).OpenAsync();
				((DbConnection)(object)connection).Close();
				return (true, new List<string>());
			}
			finally
			{
				((IDisposable)connection)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			if (ex.Message.ToLower().Contains("A network-related or instance-specific"))
			{
				return (false, new List<string> { "سرور در دسترس نیست" });
			}
			if (ex.Message.Contains("but then an error occurred during the login process"))
			{
				return (false, new List<string> { "نام کاربری یا رمز عبور اشتباه است" });
			}
			return (false, new List<string> { ex.Message });
		}
	}

	private static bool RemoteServerIsAlive(IPAddress remoteIp)
	{
		try
		{
			bool result = false;
			using (Ping ping = new Ping())
			{
				result = ping.Send(remoteIp).Status == IPStatus.Success;
			}
			return result;
		}
		catch
		{
			return false;
		}
	}
}
