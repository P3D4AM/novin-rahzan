using System;

namespace PacketParser;

public static class TelegramNameAttribute
{
	public class TelegramName : Attribute
	{
		public readonly string Text;

		public TelegramName(string text)
		{
			Text = text;
		}
	}
}
