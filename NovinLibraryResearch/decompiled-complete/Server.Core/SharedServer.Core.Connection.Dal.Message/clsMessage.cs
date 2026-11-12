using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Message;
using Server.Core.Assistance;

namespace SharedServer.Core.Connection.Dal.Message;

public class clsMessage
{
	public static clsMessage Instance => new clsMessage();

	public async Task<ReturnedSaveFuncInfo> SaveAsync(short depId, WebMessage message)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Message\\clsMessage.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				if ((message?.FilesForUpload?.Any()).GetValueOrDefault())
				{
					List<string> list = await UploadFilesAsync(depId, message?.FilesForUpload);
					if (list != null && list.Any())
					{
						message.MessageDetails = new List<WebMessageDetail>();
						list.ForEach(delegate(string url)
						{
							message.MessageDetails.Add(WebMessageDetail.Factory(url, enWebMesssageDetailType.Url));
						});
					}
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				await returnedSaveFuncInfo.AddReturnedValueAsync(await Initializer.Dal.Message.InsertAsync(message, depId), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Message\\clsMessage.cs", 38);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Message\\clsMessage.cs", 42);
				res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Message\\clsMessage.cs", 43);
			}
			return res;
		}
	}

	private async Task<List<string>> UploadFilesAsync(short depId, IEnumerable<WebFileInfo> files)
	{
		try
		{
			List<string> urlList = new List<string>();
			foreach (WebFileInfo file in files)
			{
				List<string> list = urlList;
				list.Add(await Initializer.Dal.FileInfo.SavePhysicalFileForNewClientsAsync(depId, file));
			}
			return urlList;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UploadFilesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Message\\clsMessage.cs", 58);
			throw;
		}
	}
}
