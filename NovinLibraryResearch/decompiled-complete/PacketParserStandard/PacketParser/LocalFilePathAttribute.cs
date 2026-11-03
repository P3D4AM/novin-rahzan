using System;

namespace PacketParser;

public static class LocalFilePathAttribute
{
	public class LocalFilePath : Attribute
	{
		public readonly string Text;

		public LocalFilePath(string text)
		{
			Text = text;
		}
	}
}
