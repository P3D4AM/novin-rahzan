using System;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class OccasionCalendarModel
{
	public string Title { get; set; }

	public string Value { get; set; }

	public string SmsPattern { get; set; }

	public bool Status { get; set; }

	public bool Required { get; set; }

	public short Ordinal { get; set; }

	public DateTime? StartSendTime { get; set; }

	public DateTime? EndSendTime { get; set; }

	public Guid? BaseCouponDiscountGuid { get; set; }

	public static string FillSmsText(string smsPatern, string name, string family, string fullName)
	{
		try
		{
			if (string.IsNullOrEmpty(smsPatern))
			{
				return "";
			}
			if (smsPatern.Contains("[نام]"))
			{
				smsPatern = smsPatern.Replace("[نام]", name);
			}
			if (smsPatern.Contains("[نام خانوادگی]"))
			{
				smsPatern = smsPatern.Replace("[نام خانوادگی]", family);
			}
			if (smsPatern.Contains("[نام کامل]"))
			{
				smsPatern = smsPatern.Replace("[نام کامل]", fullName);
			}
			return smsPatern;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FillSmsText", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\OccasionCalendarModel.cs", 31);
			throw;
		}
	}
}
