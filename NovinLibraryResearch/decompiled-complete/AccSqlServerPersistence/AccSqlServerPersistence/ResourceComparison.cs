using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Force.Crc32;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace AccSqlServerPersistence;

public class ResourceComparison
{
	public static ReturnedSaveFuncInfoWithValue<long> CheckCrc(IEnumerable<EnCacheConfig> configs, ENSource source)
	{
		ReturnedSaveFuncInfoWithValue<long> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<long>();
		try
		{
			Stopwatch.StartNew();
			long num = 0L;
			List<string> list = Assembly.GetExecutingAssembly().GetManifestResourceNames()?.ToList();
			if (list.Count <= 0)
			{
				return returnedSaveFuncInfoWithValue;
			}
			if (configs.Any((EnCacheConfig x) => x == EnCacheConfig.Accounting))
			{
				num += CheckCrc(list.Single((string q) => q.Contains("Acc_sp")));
				num += CheckCrc(list.Single((string q) => q.Contains("FixAccountingData")));
				if (source == ENSource.Accounting)
				{
					num += CheckCrc(list.Single((string q) => q.Contains("DropConstraintForClientsOfAccounting")));
				}
				else
				{
					num += CheckCrc(list.Single((string q) => q.Contains("ErteghaFuctions_Baje")));
					num += CheckCrc(list.Single((string q) => q.Contains("DropConstraintForClientsOfSignalR")));
				}
			}
			if (configs.Any((EnCacheConfig x) => x == EnCacheConfig.Advertise))
			{
				num += CheckCrc(list.Single((string q) => q.Contains("Advertise_sp")));
			}
			if (configs.Any((EnCacheConfig x) => x == EnCacheConfig.Global))
			{
				num += CheckCrc(list.Single((string q) => q.Contains("Global_sp")));
			}
			if (configs.Any((EnCacheConfig x) => x == EnCacheConfig.Attendance))
			{
				num += CheckCrc(list.Single((string q) => q.Contains("Attendance_sp")));
			}
			returnedSaveFuncInfoWithValue.value = num;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckCrc", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ResourceComparison.cs", 50);
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "CheckCrc", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ResourceComparison.cs", 51);
		}
		return returnedSaveFuncInfoWithValue;
	}

	private static long CheckCrc(string sourceName)
	{
		long result = 0L;
		try
		{
			using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(sourceName);
			if (stream == null)
			{
				return 0L;
			}
			result = Crc32Algorithm.Compute(new byte[stream.Length]);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckCrc", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\ResourceComparison.cs", 64);
		}
		return result;
	}
}
