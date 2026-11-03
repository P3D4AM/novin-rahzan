using System;

namespace PacketParser;

public static class MultiLangaugeNameAttribute
{
	public class KeyName : Attribute
	{
		public readonly string Text;

		public KeyName(string text)
		{
			Text = text;
		}
	}
}
