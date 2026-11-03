using System;
using System.Collections.Generic;
using System.Linq;

namespace PacketParser.Connection;

public class PrefixRecognizer
{
	public static Tuple<short, short, short, byte[]> Analyze(byte[] data)
	{
		checked
		{
			short item = (short)((data[0] << 8) + data[1]);
			short item2 = (short)((data[2] << 8) + data[3]);
			short item3 = (short)((data[4] << 8) + data[5]);
			data = data.ToList().Skip(20).ToArray();
			return new Tuple<short, short, short, byte[]>(item, item2, item3, data);
		}
	}

	public static List<byte> Generate(short part, int totalParts, short refrencedId)
	{
		return checked(new List<byte>
		{
			(byte)(part >> 8),
			(byte)(part & 0xFF),
			(byte)(totalParts >> 8),
			(byte)(totalParts & 0xFF),
			(byte)(refrencedId >> 8),
			(byte)(refrencedId & 0xFF),
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0
		});
	}
}
