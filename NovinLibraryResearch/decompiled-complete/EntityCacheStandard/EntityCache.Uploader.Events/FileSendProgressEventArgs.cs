using System;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;

namespace EntityCache.Uploader.Events;

public class FileSendProgressEventArgs
{
	public Guid FileId { get; set; }

	public int TotalPacket { get; set; }

	public int SentCount { get; set; }

	public bool SendSuccesss { get; set; }

	public int Percentage { get; set; }

	internal static async Task<FileSendProgressEventArgs> GetFileSendProgressEventArgsAsync(string contextGuid, Guid file, bool success)
	{
		using (new JaegerMonitor("GetFileSendProgressEventArgsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Events\\FileSendProgressEventArgs.cs"))
		{
			FileSendProgressEventArgs ret = new FileSendProgressEventArgs();
			try
			{
				if (file == Guid.Empty)
				{
					return ret;
				}
				FileSendProgressEventArgs fileSendProgressEventArgs = ret;
				fileSendProgressEventArgs.TotalPacket = await UploadFilePartBusiness.GetTotalPartCountAsync(contextGuid, file);
				fileSendProgressEventArgs = ret;
				fileSendProgressEventArgs.SentCount = await UploadFilePartBusiness.GetPartSentCountAsync(contextGuid, file);
				ret.FileId = file;
				ret.SendSuccesss = success;
				if (ret.TotalPacket > 0)
				{
					ret.Percentage = Convert.ToInt32(Math.Ceiling((float)ret.SentCount / (float)ret.TotalPacket * 100f));
				}
				else
				{
					ret.Percentage = 0;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetFileSendProgressEventArgsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Events\\FileSendProgressEventArgs.cs", 37);
			}
			return ret;
		}
	}
}
