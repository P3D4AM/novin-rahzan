using System;

namespace PacketParser.Entities.Attendance;

public class AttHelpURL : Attribute
{
	public string Text;

	public AttHelpURL(string text)
	{
		Text = text;
	}
}
