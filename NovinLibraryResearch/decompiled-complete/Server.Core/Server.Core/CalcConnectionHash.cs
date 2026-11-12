using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;

namespace Server.Core;

public static class CalcConnectionHash
{
	public static async Task<byte[]> CalcHashDataAsync(string caller, int UserId, Guid DbGuid, int CustomerTafsilId, long AndroidIme, long LkSerial, string CpuSerial, Guid cashierGuid, string ClientContextGuid, CancellationToken token)
	{
		using (new JaegerMonitor("CalcHashDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\CalcConnectionHash.cs"))
		{
			int num = 0;
			try
			{
				num = 1;
				token.ThrowIfCancellationRequested();
				num = 2;
				if (UserId <= 0)
				{
					throw new ArgumentException("UserId", "is less or equel zero");
				}
				num = 3;
				string s = UserId + DbGuid.ToString() + CustomerTafsilId + "/" + AndroidIme + "/" + LkSerial + "/" + CpuSerial + "/" + cashierGuid;
				num = 4;
				token.ThrowIfCancellationRequested();
				num = 5;
				SHA256CryptoServiceProvider sHA256CryptoServiceProvider = new SHA256CryptoServiceProvider();
				num = 6;
				token.ThrowIfCancellationRequested();
				num = 7;
				return sHA256CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(s));
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (ArgumentException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, "#caller_" + caller + "\r\n" + $"#Line_{num}", "CalcHashDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\CalcConnectionHash.cs", 37);
				return null;
			}
		}
	}

	public static Task<byte[]> CalcHashDataAsync(string v, int userId, Guid dbGuid, int hesabTreeId, long androidIme, long lkSerial, string cpuSerial, Guid cashierGuid, object clientContextGuid, CancellationToken token)
	{
		throw new NotImplementedException();
	}
}
