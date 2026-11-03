using System;

namespace PacketParser.Entities.Attendance;

public class AttReportFileName : Attribute
{
	public string Text;

	public AttReportFileName(string text)
	{
		Text = text;
	}
}
