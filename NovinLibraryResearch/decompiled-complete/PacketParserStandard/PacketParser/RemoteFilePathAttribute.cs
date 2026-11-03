using System;

namespace PacketParser;

public static class RemoteFilePathAttribute
{
	public class RemoteFilePath : Attribute
	{
		public readonly string Text;

		public RemoteFilePath(string text)
		{
			Text = text;
		}
	}
}
