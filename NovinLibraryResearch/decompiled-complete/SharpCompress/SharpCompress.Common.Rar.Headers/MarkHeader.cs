using System;
using System.IO;

namespace SharpCompress.Common.Rar.Headers;

internal class MarkHeader : IRarHeader
{
	private const int MAX_SFX_SIZE = 524272;

	internal bool OldNumberingFormat { get; private set; }

	public bool IsRar5 { get; }

	public HeaderType HeaderType => HeaderType.Mark;

	private MarkHeader(bool isRar5)
	{
		IsRar5 = isRar5;
	}

	private static byte GetByte(Stream stream)
	{
		int num = stream.ReadByte();
		if (num != -1)
		{
			return (byte)num;
		}
		throw new EndOfStreamException();
	}

	public static MarkHeader Read(Stream stream, bool leaveStreamOpen, bool lookForHeader)
	{
		int num = (lookForHeader ? 524272 : 0);
		try
		{
			int num2 = -1;
			byte @byte = GetByte(stream);
			num2++;
			while (num2 <= num)
			{
				if (@byte == 82)
				{
					@byte = GetByte(stream);
					num2++;
					switch (@byte)
					{
					case 97:
						@byte = GetByte(stream);
						num2++;
						if (@byte != 114)
						{
							break;
						}
						@byte = GetByte(stream);
						num2++;
						if (@byte != 33)
						{
							break;
						}
						@byte = GetByte(stream);
						num2++;
						if (@byte != 26)
						{
							break;
						}
						@byte = GetByte(stream);
						num2++;
						if (@byte != 7)
						{
							break;
						}
						@byte = GetByte(stream);
						num2++;
						switch (@byte)
						{
						case 1:
							@byte = GetByte(stream);
							num2++;
							if (@byte == 0)
							{
								return new MarkHeader(isRar5: true);
							}
							break;
						case 0:
							return new MarkHeader(isRar5: false);
						}
						break;
					case 69:
						@byte = GetByte(stream);
						num2++;
						if (@byte == 126)
						{
							@byte = GetByte(stream);
							num2++;
							if (@byte == 94)
							{
								throw new InvalidFormatException("Rar format version pre-4 is unsupported.");
							}
						}
						break;
					}
				}
				else
				{
					@byte = GetByte(stream);
					num2++;
				}
			}
		}
		catch (Exception inner)
		{
			if (!leaveStreamOpen)
			{
				stream.Dispose();
			}
			throw new InvalidFormatException("Error trying to read rar signature.", inner);
		}
		throw new InvalidFormatException("Rar signature not found");
	}
}
