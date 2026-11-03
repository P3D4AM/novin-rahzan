using System;
using System.Text;
using PacketParser.Entities;

namespace PacketParser.EventArgs.TaxService;

public class InvoiceTrackingEventArgs
{
	public Guid OrderGuid { get; set; }

	public string RefrenceId { get; set; }

	public string Uid { get; set; }

	public static InvoiceTrackingEventArgs FactoryMethod(string orderguid, string refID, string uID)
	{
		StringBuilder stringBuilder = new StringBuilder(100000);
		try
		{
			bool flag = true;
			Guid result = Guid.Empty;
			if (!Guid.TryParse(orderguid, out result))
			{
				stringBuilder.AppendLine("شناسه فاکتور وارد نشده است");
				flag = false;
			}
			if (string.IsNullOrEmpty(refID))
			{
				stringBuilder.AppendLine("کد پیگیری وارد نشده است");
				flag = false;
			}
			if (string.IsNullOrEmpty(uID))
			{
				stringBuilder.AppendLine("uid وارد نشده است");
				flag = false;
			}
			if (!flag)
			{
				throw new Exception(stringBuilder.ToString());
			}
			return new InvoiceTrackingEventArgs
			{
				OrderGuid = result,
				RefrenceId = refID,
				Uid = uID
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, stringBuilder.ToString(), "FactoryMethod", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\TaxService\\LoginEventArgs.cs", 128);
		}
		return null;
	}
}
