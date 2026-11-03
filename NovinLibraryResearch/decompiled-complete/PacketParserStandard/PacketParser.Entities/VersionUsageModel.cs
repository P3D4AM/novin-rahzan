using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PacketParser.Entities;

public class VersionUsageModel
{
	public string Version { get; set; }

	public long VersionLong => Version.VersionStringToLong();

	public ENSource Source { get; set; }

	public string SourceForDisplay => Source.GetDisplay();

	public int Count { get; set; }

	public int DayOfUsage { get; set; }

	public int AverageDays { get; set; }

	public DateTime PublishDate { get; set; }

	public string PublishDateSh => PublishDate.MiladiToShamsi();

	public DateTime LastDate { get; set; }

	public string LastDateSh => LastDate.MiladiToShamsi();

	public int CriticalBugCount { get; set; }

	public int HighBugCount { get; set; }

	public int MediumBugCount { get; set; }

	public int LowBugCount { get; set; }

	public static async Task<List<VersionUsageModel>> GetAllReportAsync(string url = "http://novinrank.ir/versionusage/getallreport", int timeout = 15000, CancellationToken token = default(CancellationToken))
	{
		try
		{
			return (await url.GetFromApi<List<VersionUsageModel>>(token, 3, null, timeout)).value.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\VersionUsageModel.cs", 34);
		}
		return null;
	}
}
