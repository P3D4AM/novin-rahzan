using System;

namespace PacketParser;

public static class PersianNameAttribute
{
	public class PersianName : Attribute
	{
		public readonly string Text;

		public PersianName(string text)
		{
			Text = text;
		}
	}
}
