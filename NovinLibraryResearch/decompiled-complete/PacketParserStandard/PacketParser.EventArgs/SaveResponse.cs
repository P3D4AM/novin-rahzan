using System;
using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class SaveResponse : BaseSaveResponse
{
	public long ReferencedId { get; set; }

	public Guid Guid { get; set; } = Guid.Empty;


	public EntityPart EntityPart { get; set; }

	public SaveResponse()
	{
	}

	public SaveResponse(int referencedId, Guid guid, DateTime date, bool saveResultStatus, string shortMessage = "", string message = "")
	{
		try
		{
			ReferencedId = referencedId;
			Guid = guid;
			base.Date = date;
			base.SaveResultStatus = saveResultStatus;
			base.Shortmessage = shortMessage;
			base.Message = message;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\SaveResponseEventArgs.cs", 113);
		}
	}
}
public class SaveResponse<TKey> : BaseSaveResponse
{
	public TKey Key { get; set; }

	public SaveResponse()
	{
	}

	public SaveResponse(int referencedId, TKey key, DateTime date, bool saveResultStatus, string shortMessage = "", string message = "")
	{
		try
		{
			Key = key;
			base.Date = date;
			base.SaveResultStatus = saveResultStatus;
			base.Shortmessage = shortMessage;
			base.Message = message;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\SaveResponseEventArgs.cs", 131);
		}
	}
}
