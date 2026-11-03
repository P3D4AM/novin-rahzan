using System;
using System.Collections.Generic;
using System.Text;
using PacketParser.TransferLayer.Interfaces;

namespace PacketParser.Connection;

public static class ChunkingOperations
{
	private static short _RefrencedId;

	private static short NextRefrencedId()
	{
		if (_RefrencedId == short.MaxValue)
		{
			_RefrencedId = 0;
		}
		return checked(++_RefrencedId);
	}

	public static byte[] ProcessReceivedData(byte[] data, string connectionId, IConnectionData connection, StringBuilder sb)
	{
		sb.AppendLine("ProcessReceivedData...");
		Tuple<short, short, short, byte[]> tuple = PrefixRecognizer.Analyze(data);
		return connection.ProgressReceived(tuple.Item4, tuple.Item1, tuple.Item2, tuple.Item3.ToString(), sb);
	}

	public static List<byte[]> Chunk(byte[] temp)
	{
		List<List<byte>> list = temp.ChunkList(65m);
		List<byte[]> list2 = new List<byte[]>();
		short num = 0;
		short refrencedId = NextRefrencedId();
		foreach (List<byte> item in list)
		{
			List<byte> list3 = PrefixRecognizer.Generate(num, list.Count, refrencedId);
			list3.AddRange(item);
			list2.Add(list3.ToArray());
			num = checked((short)(num + 1));
		}
		return list2;
	}
}
