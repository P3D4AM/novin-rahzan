using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using Server.Core;

namespace PacketParser.Assistance;

public class LoginMonitor
{
	private static HashSet<string> WhiteListIp = new HashSet<string> { "94.183.134.226", "5.202.53.227", "185.231.113.133", "5.160.152.164" };

	public static List<LoginMonitor> LstLoginMonitor = new List<LoginMonitor>();

	public string Ip { get; set; }

	private DateTime DateEvent { get; set; }

	public EnLoginResult LoginStatus { get; private set; }

	public static Task<ReturnedSaveFuncInfo> SaveAsync(string ip, string userName, string password, EnLoginResult FailedType, IDalClientLoginHistory dal, string desc = "")
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			LoginMonitor item = new LoginMonitor
			{
				Ip = ip,
				DateEvent = DateTime.Now,
				LoginStatus = FailedType
			};
			if (!WhiteListIp.Contains(ip) && FailedType != EnLoginResult.ServerError)
			{
				LstLoginMonitor.Add(item);
				if (LstLoginMonitor.Count > 5000)
				{
					WebErrorLog.Instance.StartLog($"count of LogMonit for ip is exceeded! LstLoginMonitor.Count:{LstLoginMonitor.Count}", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\LoginMonitor.cs", 37);
					LstLoginMonitor.Clear();
				}
			}
			WebErrorLog.Instance.StartLog($"New ip blocked! Id:{ip}/userName{userName}/password:{password}/failType:{FailedType}/Desc:{desc}", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\LoginMonitor.cs", 42);
			if (dal != null)
			{
				ClientLoginHistory logObj = new ClientLoginHistory
				{
					Ip = ip,
					Pass = password,
					DisconnectDescription = "rest login resul: " + FailedType.GetDisplay() + "  desc:" + desc,
					Source = ENSource.RestApi,
					Username = userName,
					AndroidIme = 0L,
					ClientVersion = "",
					ConnectionUniqueId = "",
					CpuSerial = "",
					CusGuid = Guid.Empty,
					CusPass = "",
					CusUserName = "",
					DbGuid = Guid.Empty,
					DepId = 0,
					DisconnectDateM = null,
					HashId = 0L,
					LoginDateM = DateTime.Now
				};
				dal.InsertLogAsync(logObj, 10);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\LoginMonitor.cs", 71);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\LoginMonitor.cs", 72);
		}
		return Task.FromResult(returnedSaveFuncInfo);
	}

	private static ReturnedSaveFuncInfo CheckLostExpireTime()
	{
		ReturnedSaveFuncInfoWithValue<LoginMonitor> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<LoginMonitor>();
		try
		{
			if (LstLoginMonitor == null || LstLoginMonitor.Count <= 0)
			{
				return returnedSaveFuncInfoWithValue;
			}
			DateTime dateEvent = LstLoginMonitor[0].DateEvent;
			if ((DateTime.Now - dateEvent).TotalMinutes > 15.0)
			{
				LstLoginMonitor = new List<LoginMonitor>();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckLostExpireTime", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\LoginMonitor.cs", 88);
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "CheckLostExpireTime", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\LoginMonitor.cs", 89);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static int FailedLoginCount(string ip)
	{
		try
		{
			if (LstLoginMonitor != null && LstLoginMonitor.Count > 0)
			{
				CheckLostExpireTime();
				return LstLoginMonitor.Count((LoginMonitor p) => p.Ip == ip);
			}
			return 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FailedLoginCount", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\LoginMonitor.cs", 106);
			return 0;
		}
	}
}
