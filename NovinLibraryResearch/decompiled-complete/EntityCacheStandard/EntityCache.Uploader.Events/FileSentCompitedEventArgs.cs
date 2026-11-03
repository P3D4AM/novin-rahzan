using System;
using EntityCache.Business;
using PacketParser.Entities;

namespace EntityCache.Uploader.Events;

public class FileSentCompitedEventArgs
{
	public string FileName { get; set; }

	public Guid FileId { get; set; }

	public string Url { get; set; }

	public long Crc { get; set; }

	public bool Success { get; set; }

	public string Message { get; set; }

	internal FileSentCompitedEventArgs(UploadFileBusiness file, string url, string msg)
	{
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Events\\FileSentCompitedEventArgs.cs"))
		{
			try
			{
				FileName = file.Name;
				FileId = file.Id;
				Url = url;
				Crc = file.Crc;
				Success = url != null && url.Length > 0;
				Message = msg;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Events\\FileSentCompitedEventArgs.cs", 30);
			}
		}
	}

	public FileSentCompitedEventArgs()
	{
	}
}
