using System;
using System.Windows.Forms;

namespace ZDC2911.Business;

public class InitData
{
	public static readonly DateTime MinDateTime = new DateTime(2010, 1, 1, 0, 0, 0);

	public static readonly DateTime MaxDateTime = new DateTime(2099, 12, 31, 23, 59, 59);

	private InitData()
	{
	}

	public static void InitAlarmDN(ComboBox cbo)
	{
		for (int i = 1; i <= 24; i++)
		{
			cbo.Items.Add((object)i);
		}
	}

	public static void InitValidAttendTimeDN(ComboBox cbo)
	{
		for (int i = 1; i <= 24; i++)
		{
			cbo.Items.Add((object)i);
		}
	}

	public static void InitTimerOnOffDN(ComboBox cbo)
	{
		for (int i = 1; i <= 12; i++)
		{
			cbo.Items.Add((object)i);
		}
	}

	public static void InitHour(ComboBox cbo)
	{
		for (int i = 0; i <= 23; i++)
		{
			cbo.Items.Add((object)i);
		}
	}

	public static void InitMinuteOrSecond(ComboBox cbo)
	{
		for (int i = 0; i <= 59; i++)
		{
			cbo.Items.Add((object)i);
		}
	}

	public static void InitZoneNo(ComboBox cbo)
	{
		for (int i = 1; i <= 30; i++)
		{
			cbo.Items.Add((object)i);
		}
	}

	public static void InitGroupNo(ComboBox cbo)
	{
		for (int i = 1; i <= 30; i++)
		{
			cbo.Items.Add((object)i);
		}
	}

	public static void InitHolidayNo(ComboBox cbo)
	{
		for (int i = 1; i <= 30; i++)
		{
			cbo.Items.Add((object)i);
		}
	}

	public static void InitMessageSN(ComboBox cbo)
	{
		for (int i = 1; i <= 10; i++)
		{
			cbo.Items.Add((object)i);
		}
	}
}
