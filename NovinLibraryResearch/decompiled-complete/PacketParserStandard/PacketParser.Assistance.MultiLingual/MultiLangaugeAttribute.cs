using System;
using PacketParser.Entities;

namespace PacketParser.Assistance.MultiLingual;

public static class MultiLangaugeAttribute
{
	public class KeyName : Attribute
	{
		public readonly string Text;

		public KeyName(string text)
		{
			try
			{
				if (LanguageAndCalendar.Instance != null)
				{
					Text = text;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\MultiLangugeAttribute.cs", 23);
			}
		}
	}
}
