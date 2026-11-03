using System.Collections.Generic;
using System.Text;
using PacketParser.Entities;

namespace PacketParser.PosToPc.Sadad.Classes;

internal abstract class PcPosBaseMessage
{
	private List<MessageBuilder> items = new List<MessageBuilder>();

	public abstract byte[] Body { get; }

	public EnSadadPacketType PacketType { get; private set; }

	public string SerialNo { get; private set; }

	public EnSadadFrameVersion Version { get; private set; }

	public PcPosBaseMessage()
	{
		items.Add(new ByteArrayMessageBuilder("STX", 0, new byte[1] { 2 }, FramePduLen.Header));
		items.Add(new ByteArrayMessageBuilder("ETX", 5, new byte[1] { 3 }, FramePduLen.Footer));
	}

	public void AddToFrame(MessageBuilder msg)
	{
		if (!items.Contains(msg))
		{
			items.Add(msg);
		}
	}

	internal abstract byte[] Build();

	public byte[] GetFrame()
	{
		Sort();
		List<byte> list = new List<byte>();
		foreach (MessageBuilder item in items)
		{
			if (!(item is StringMessageBuilder))
			{
				list.AddRange((item as ByteArrayMessageBuilder).Data);
				continue;
			}
			StringMessageBuilder stringMessageBuilder = item as StringMessageBuilder;
			if (stringMessageBuilder.Data != null)
			{
				list.AddRange(stringMessageBuilder.IsPadRight ? Encoding.ASCII.GetBytes(stringMessageBuilder.Data.PadRight(item.MaxLength, stringMessageBuilder.PadChar)) : Encoding.ASCII.GetBytes(stringMessageBuilder.Data.PadLeft(item.MaxLength, stringMessageBuilder.PadChar)));
			}
		}
		return list.ToArray();
	}

	public int GetFrameLength()
	{
		Sort();
		int num = 0;
		checked
		{
			foreach (MessageBuilder item in items)
			{
				if (!(item is StringMessageBuilder))
				{
					num += (item as ByteArrayMessageBuilder).Data.Length;
					continue;
				}
				StringMessageBuilder stringMessageBuilder = item as StringMessageBuilder;
				num += (stringMessageBuilder.IsPadRight ? Encoding.ASCII.GetBytes(stringMessageBuilder.Data.PadRight(item.MaxLength, stringMessageBuilder.PadChar)) : Encoding.ASCII.GetBytes(stringMessageBuilder.Data.PadLeft(item.MaxLength, stringMessageBuilder.PadChar))).Length;
			}
			return num;
		}
	}

	public byte[] GetFrameRange(int index, int count)
	{
		Sort();
		List<byte> list = new List<byte>();
		foreach (MessageBuilder item in items)
		{
			if (!(item is StringMessageBuilder))
			{
				list.AddRange((item as ByteArrayMessageBuilder).Data);
				continue;
			}
			StringMessageBuilder stringMessageBuilder = item as StringMessageBuilder;
			list.AddRange(stringMessageBuilder.IsPadRight ? Encoding.ASCII.GetBytes(stringMessageBuilder.Data.PadRight(item.MaxLength, stringMessageBuilder.PadChar)) : Encoding.ASCII.GetBytes(stringMessageBuilder.Data.PadLeft(item.MaxLength, stringMessageBuilder.PadChar)));
		}
		return list.GetRange(index, count).ToArray();
	}

	internal PcPosBaseMessage SetPacketType(EnSadadPacketType type)
	{
		PacketType = type;
		items.Add(new ByteArrayMessageBuilder("Message Type", 3, new byte[1] { checked((byte)PacketType) }, FramePduLen.Header));
		return this;
	}

	internal PcPosBaseMessage SetSerialNo(string serialNo)
	{
		SerialNo = serialNo;
		if (!string.IsNullOrEmpty(serialNo))
		{
			SetVersion(EnSadadFrameVersion.SadadEncryption);
		}
		return this;
	}

	internal PcPosBaseMessage SetVersion(EnSadadFrameVersion version)
	{
		Version = version;
		items.Add(new StringMessageBuilder("Message Version", 2, Version.ToString("d"), FramePduLen.Version, Encoding.ASCII, isPadRight: false, '0'));
		return this;
	}

	public void Sort()
	{
		items.Sort(new StringMessageBuilder());
	}
}
