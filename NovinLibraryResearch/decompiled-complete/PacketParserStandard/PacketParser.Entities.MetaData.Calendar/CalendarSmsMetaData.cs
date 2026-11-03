using System;
using System.Collections.Generic;
using System.Linq;

namespace PacketParser.Entities.MetaData.Calendar;

public class CalendarSmsMetaData
{
	public List<CalendarSms> CalendarSmsList { get; set; }

	public void AddCalendarSms(CalendarSms item)
	{
		try
		{
			if (item != null && !(item.Guid == Guid.Empty) && !string.IsNullOrEmpty(item.SmsPattern))
			{
				if (CalendarSmsList == null)
				{
					List<CalendarSms> list2 = (CalendarSmsList = new List<CalendarSms>());
				}
				CalendarSms calendarSms = CalendarSmsList.FirstOrDefault((CalendarSms q) => q.Guid == item.Guid || q.SmsPattern == item.SmsPattern);
				if (calendarSms != null)
				{
					CalendarSmsList.Remove(calendarSms);
				}
				CalendarSmsList.Add(item);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddCalendarSms", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\MetaData\\Calendar\\CalendarSmsMetaData.cs", 22);
		}
	}
}
