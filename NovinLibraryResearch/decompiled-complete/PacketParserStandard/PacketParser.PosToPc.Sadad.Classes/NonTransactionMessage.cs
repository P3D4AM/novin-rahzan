using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using PacketParser.Entities;

namespace PacketParser.PosToPc.Sadad.Classes;

internal class NonTransactionMessage : PcPosBaseMessage
{
	private List<MessageBuilder> items = new List<MessageBuilder>();

	public decimal Amount { get; private set; }

	public override byte[] Body
	{
		get
		{
			checked
			{
				try
				{
					List<byte> list = new List<byte>();
					if (base.PacketType == EnSadadPacketType.Request)
					{
						list.AddRange(GetFrame());
						if (!base.Version.Equals(EnSadadFrameVersion.SadadEncryption))
						{
							return list.ToArray();
						}
						byte[] array = new byte[16];
						List<byte> list2 = new List<byte>();
						byte[] array2 = new byte[16]
						{
							48, 243, 167, 104, 58, 42, 129, 144, 203, 89,
							104, 231, 47, 86, 122, 196
						};
						byte[] bytes = Encoding.ASCII.GetBytes(base.SerialNo);
						Encoding aSCII = Encoding.ASCII;
						list2.AddRange(aSCII.GetBytes(list.ToArray().Length.ToString("000")));
						list2.AddRange(list);
						list2.Add(Utility.GetLRC(list.ToArray()));
						for (int i = 0; i < 16; i++)
						{
							array[i] = (byte)(array2[i] * bytes[i]);
						}
						return new TripleDESCryptoServiceProvider
						{
							Key = array,
							Mode = CipherMode.ECB,
							Padding = PaddingMode.Zeros
						}.CreateEncryptor().TransformFinalBlock(list2.ToArray(), 0, list2.ToArray().Length);
					}
					return list.ToArray();
				}
				catch (Exception)
				{
					throw;
				}
			}
		}
	}

	public string DataLength
	{
		get
		{
			int length = 0;
			items.ForEach(checked(delegate(MessageBuilder mb)
			{
				length += mb.Length;
			}));
			return length.ToString().PadLeft(3, '0');
		}
	}

	public string DllVersion { get; private set; }

	public string FunctionCode { get; private set; }

	public string OrderId { get; private set; }

	internal override byte[] Build()
	{
		byte[] body = Body;
		checked
		{
			string data = (FramePduLen.Version + FramePduLen.Lrc + FramePduLen.Footer + FramePduLen.Footer + body.Length).ToString();
			AddToFrame(new StringMessageBuilder("Message Len", 1, data, FramePduLen.Length, Encoding.ASCII, isPadRight: false, '0'));
			AddToFrame(new ByteArrayMessageBuilder("Message Body", 4, body, FramePduLen.Length));
			byte lRC = Utility.GetLRC(GetFrameRange(FramePduLen.Header, FramePduLen.Length + FramePduLen.Version + FramePduLen.PacketType + body.Length));
			AddToFrame(new ByteArrayMessageBuilder("Message Lrc", 6, new byte[1] { lRC }, FramePduLen.Lrc));
			return base.GetFrame();
		}
	}

	public new byte[] GetFrame()
	{
		items.RemoveAll((MessageBuilder mb) => mb.Order == 1);
		items.Add(new StringMessageBuilder("Data Length", 1, DataLength, 3, Encoding.ASCII, isPadRight: false, '0'));
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
			if (!string.IsNullOrEmpty(stringMessageBuilder.Data))
			{
				list.AddRange(stringMessageBuilder.IsPadRight ? Encoding.ASCII.GetBytes(stringMessageBuilder.Data.PadRight(item.MaxLength, stringMessageBuilder.PadChar)) : Encoding.ASCII.GetBytes(stringMessageBuilder.Data.PadLeft(item.MaxLength, stringMessageBuilder.PadChar)));
			}
		}
		return list.ToArray();
	}

	internal NonTransactionMessage SetAmount(decimal amount)
	{
		Amount = amount;
		items.Add(new StringMessageBuilder("Amount", 4, amount.ToString(), 12, Encoding.ASCII, isPadRight: false, '0'));
		return this;
	}

	internal NonTransactionMessage SetDllVersion(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return this;
		}
		string data = value.Length.ToString("D2") + value;
		items.Add(new StringMessageBuilder("DLL Version", 6, data, 3, Encoding.ASCII, isPadRight: false, '0'));
		return this;
	}

	internal NonTransactionMessage SetFunctionCode(string functionCode)
	{
		FunctionCode = functionCode;
		items.Add(new StringMessageBuilder("Function Code", 0, functionCode, 4, Encoding.ASCII, isPadRight: false, '0'));
		return this;
	}

	internal NonTransactionMessage SetOrderId(string orderId)
	{
		OrderId = orderId;
		StringBuilder stringBuilder = new StringBuilder(100000);
		stringBuilder.Append(orderId.Length.ToString("D2"));
		stringBuilder.Append(orderId);
		items.Add(new StringMessageBuilder("OrderId", 5, stringBuilder.ToString(), stringBuilder.Length, Encoding.ASCII, isPadRight: false, '0'));
		return this;
	}

	public new void Sort()
	{
		items.Sort(new StringMessageBuilder());
	}
}
