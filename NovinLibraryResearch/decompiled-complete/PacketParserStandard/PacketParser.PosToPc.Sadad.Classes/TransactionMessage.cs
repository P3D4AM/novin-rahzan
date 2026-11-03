using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using PacketParser.Entities;

namespace PacketParser.PosToPc.Sadad.Classes;

internal class TransactionMessage : PcPosBaseMessage
{
	private const int OptionalFieldLength = 999;

	private byte BitMap;

	private List<MessageBuilder> transactionItems = new List<MessageBuilder>();

	private string JustDecompileGenerated_Field1_k__BackingField = string.Empty;

	private string JustDecompileGenerated_Field2_k__BackingField = string.Empty;

	private string JustDecompileGenerated_Field3_k__BackingField = string.Empty;

	private string JustDecompileGenerated_Field4_k__BackingField = string.Empty;

	private string JustDecompileGenerated_Field5_k__BackingField = string.Empty;

	private string JustDecompileGenerated_Field6_k__BackingField = string.Empty;

	private string JustDecompileGenerated_Field7_k__BackingField = string.Empty;

	private string JustDecompileGenerated_Field8_k__BackingField = string.Empty;

	private string JustDecompileGenerated_FunctionCode_k__BackingField = string.Empty;

	private string JustDecompileGenerated_InvoiceNumber_k__BackingField = string.Empty;

	private string JustDecompileGenerated_MessageType_k__BackingField = string.Empty;

	private string JustDecompileGenerated_ProcessingCode_k__BackingField = string.Empty;

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

	public string Field1
	{
		get
		{
			return JustDecompileGenerated_get_Field1();
		}
		set
		{
			JustDecompileGenerated_set_Field1(value);
		}
	}

	public string Field2
	{
		get
		{
			return JustDecompileGenerated_get_Field2();
		}
		set
		{
			JustDecompileGenerated_set_Field2(value);
		}
	}

	public string Field3
	{
		get
		{
			return JustDecompileGenerated_get_Field3();
		}
		set
		{
			JustDecompileGenerated_set_Field3(value);
		}
	}

	public string Field4
	{
		get
		{
			return JustDecompileGenerated_get_Field4();
		}
		set
		{
			JustDecompileGenerated_set_Field4(value);
		}
	}

	public string Field5
	{
		get
		{
			return JustDecompileGenerated_get_Field5();
		}
		set
		{
			JustDecompileGenerated_set_Field5(value);
		}
	}

	public string Field6
	{
		get
		{
			return JustDecompileGenerated_get_Field6();
		}
		set
		{
			JustDecompileGenerated_set_Field6(value);
		}
	}

	public string Field7
	{
		get
		{
			return JustDecompileGenerated_get_Field7();
		}
		set
		{
			JustDecompileGenerated_set_Field7(value);
		}
	}

	public string Field8
	{
		get
		{
			return JustDecompileGenerated_get_Field8();
		}
		set
		{
			JustDecompileGenerated_set_Field8(value);
		}
	}

	public string FunctionCode
	{
		get
		{
			return JustDecompileGenerated_get_FunctionCode();
		}
		set
		{
			JustDecompileGenerated_set_FunctionCode(value);
		}
	}

	public string InvoiceNumber
	{
		get
		{
			return JustDecompileGenerated_get_InvoiceNumber();
		}
		set
		{
			JustDecompileGenerated_set_InvoiceNumber(value);
		}
	}

	public string MessageType
	{
		get
		{
			return JustDecompileGenerated_get_MessageType();
		}
		set
		{
			JustDecompileGenerated_set_MessageType(value);
		}
	}

	public string ProcessingCode
	{
		get
		{
			return JustDecompileGenerated_get_ProcessingCode();
		}
		set
		{
			JustDecompileGenerated_set_ProcessingCode(value);
		}
	}

	public virtual string JustDecompileGenerated_get_Field1()
	{
		return JustDecompileGenerated_Field1_k__BackingField;
	}

	private void JustDecompileGenerated_set_Field1(string value)
	{
		JustDecompileGenerated_Field1_k__BackingField = value;
	}

	public virtual string JustDecompileGenerated_get_Field2()
	{
		return JustDecompileGenerated_Field2_k__BackingField;
	}

	private void JustDecompileGenerated_set_Field2(string value)
	{
		JustDecompileGenerated_Field2_k__BackingField = value;
	}

	public virtual string JustDecompileGenerated_get_Field3()
	{
		return JustDecompileGenerated_Field3_k__BackingField;
	}

	private void JustDecompileGenerated_set_Field3(string value)
	{
		JustDecompileGenerated_Field3_k__BackingField = value;
	}

	public virtual string JustDecompileGenerated_get_Field4()
	{
		return JustDecompileGenerated_Field4_k__BackingField;
	}

	private void JustDecompileGenerated_set_Field4(string value)
	{
		JustDecompileGenerated_Field4_k__BackingField = value;
	}

	public virtual string JustDecompileGenerated_get_Field5()
	{
		return JustDecompileGenerated_Field5_k__BackingField;
	}

	private void JustDecompileGenerated_set_Field5(string value)
	{
		JustDecompileGenerated_Field5_k__BackingField = value;
	}

	public virtual string JustDecompileGenerated_get_Field6()
	{
		return JustDecompileGenerated_Field6_k__BackingField;
	}

	private void JustDecompileGenerated_set_Field6(string value)
	{
		JustDecompileGenerated_Field6_k__BackingField = value;
	}

	public virtual string JustDecompileGenerated_get_Field7()
	{
		return JustDecompileGenerated_Field7_k__BackingField;
	}

	private void JustDecompileGenerated_set_Field7(string value)
	{
		JustDecompileGenerated_Field7_k__BackingField = value;
	}

	public virtual string JustDecompileGenerated_get_Field8()
	{
		return JustDecompileGenerated_Field8_k__BackingField;
	}

	private void JustDecompileGenerated_set_Field8(string value)
	{
		JustDecompileGenerated_Field8_k__BackingField = value;
	}

	public virtual string JustDecompileGenerated_get_FunctionCode()
	{
		return JustDecompileGenerated_FunctionCode_k__BackingField;
	}

	private void JustDecompileGenerated_set_FunctionCode(string value)
	{
		JustDecompileGenerated_FunctionCode_k__BackingField = value;
	}

	public virtual string JustDecompileGenerated_get_InvoiceNumber()
	{
		return JustDecompileGenerated_InvoiceNumber_k__BackingField;
	}

	private void JustDecompileGenerated_set_InvoiceNumber(string value)
	{
		JustDecompileGenerated_InvoiceNumber_k__BackingField = value;
	}

	public virtual string JustDecompileGenerated_get_MessageType()
	{
		return JustDecompileGenerated_MessageType_k__BackingField;
	}

	private void JustDecompileGenerated_set_MessageType(string value)
	{
		JustDecompileGenerated_MessageType_k__BackingField = value;
	}

	public virtual string JustDecompileGenerated_get_ProcessingCode()
	{
		return JustDecompileGenerated_ProcessingCode_k__BackingField;
	}

	private void JustDecompileGenerated_set_ProcessingCode(string value)
	{
		JustDecompileGenerated_ProcessingCode_k__BackingField = value;
	}

	public TransactionMessage()
	{
		SetInvoiceNumber("");
		SetFunctionCode("");
	}

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
		Sort();
		List<byte> list = new List<byte>();
		foreach (MessageBuilder transactionItem in transactionItems)
		{
			if (!(transactionItem is StringMessageBuilder))
			{
				list.AddRange((transactionItem as ByteArrayMessageBuilder).Data);
				continue;
			}
			StringMessageBuilder stringMessageBuilder = transactionItem as StringMessageBuilder;
			byte[] array = new byte[0];
			if (stringMessageBuilder.Data != null)
			{
				array = ((stringMessageBuilder.Encoding == Encoding.UTF8) ? stringMessageBuilder.Encoding.GetBytes(stringMessageBuilder.Data) : ((stringMessageBuilder.Encoding == Encoding.Unicode) ? stringMessageBuilder.Encoding.GetBytes(stringMessageBuilder.Data) : ((stringMessageBuilder.Encoding == Encoding.ASCII) ? (stringMessageBuilder.IsPadRight ? Encoding.ASCII.GetBytes(stringMessageBuilder.Data.PadRight(transactionItem.MaxLength, stringMessageBuilder.PadChar)) : Encoding.ASCII.GetBytes(stringMessageBuilder.Data.PadLeft(transactionItem.MaxLength, stringMessageBuilder.PadChar))) : stringMessageBuilder.Encoding.GetBytes(stringMessageBuilder.Data))));
				list.AddRange(array);
			}
		}
		return list.ToArray();
	}

	internal TransactionMessage SetAdvertisement(string value)
	{
		checked
		{
			if (!string.IsNullOrEmpty(value))
			{
				Field6 = ((value.Length <= 999) ? value.Length : 999).ToString().PadLeft(3, '0') + value;
				BitMap |= 0x20;
				transactionItems.Add(new StringMessageBuilder("Advertisement", 11, Field6, 3, Encoding.GetEncoding(1256), isPadRight: false, '0'));
			}
			else
			{
				Field6 = string.Empty;
				BitMap &= 0xDF;
				transactionItems.RemoveAll((MessageBuilder mb) => mb.Order == 11);
			}
			transactionItems.RemoveAll((MessageBuilder mb) => mb.Order == 5);
			transactionItems.Add(new ByteArrayMessageBuilder("BitMap", 5, new byte[1] { BitMap }, 1));
			return this;
		}
	}

	internal TransactionMessage SetAmount(decimal amount)
	{
		Amount = amount;
		transactionItems.Add(new StringMessageBuilder("Amount", 0, amount.ToString(), 13, Encoding.ASCII, isPadRight: true, '\0'));
		return this;
	}

	internal TransactionMessage SetBillInfo(string billInfo)
	{
		if (ProcessingCode != 170000.ToString())
		{
			return this;
		}
		checked
		{
			if (!string.IsNullOrEmpty(billInfo))
			{
				Field3 = ((billInfo.Length <= 999) ? billInfo.Length : 999).ToString().PadLeft(3, '0') + billInfo;
				BitMap |= 4;
				transactionItems.Add(new StringMessageBuilder("Bill Info", 8, Field3, 3, Encoding.ASCII, isPadRight: false, '0'));
			}
			else
			{
				Field3 = string.Empty;
				BitMap &= 0xFB;
				transactionItems.RemoveAll((MessageBuilder mb) => mb.Order == 8);
			}
			transactionItems.RemoveAll((MessageBuilder mb) => mb.Order == 5);
			transactionItems.Add(new ByteArrayMessageBuilder("BitMap", 5, new byte[1] { BitMap }, 1));
			return this;
		}
	}

	internal TransactionMessage SetCardInfo(string cardInfo)
	{
		checked
		{
			if (!string.IsNullOrEmpty(cardInfo))
			{
				Field4 = ((cardInfo.Length <= 999) ? cardInfo.Length : 999).ToString().PadLeft(3, '0') + cardInfo;
				BitMap |= 8;
				transactionItems.Add(new StringMessageBuilder("Card Info", 9, Field4, 3, Encoding.ASCII, isPadRight: false, '0'));
			}
			else
			{
				Field4 = string.Empty;
				BitMap &= 0xF7;
				transactionItems.RemoveAll((MessageBuilder mb) => mb.Order == 9);
			}
			transactionItems.RemoveAll((MessageBuilder mb) => mb.Order == 5);
			transactionItems.Add(new ByteArrayMessageBuilder("BitMap", 5, new byte[1] { BitMap }, 1));
			return this;
		}
	}

	internal TransactionMessage SetDllVersion(string value)
	{
		checked
		{
			if (!string.IsNullOrEmpty(value))
			{
				Field8 = ((value.Length <= 999) ? value.Length : 999).ToString().PadLeft(3, '0') + value;
				BitMap |= 0x80;
				transactionItems.Add(new StringMessageBuilder("Field 8", 13, Field8, 3, Encoding.ASCII, isPadRight: false, '0'));
			}
			else
			{
				Field8 = string.Empty;
				BitMap &= 0x7F;
				transactionItems.RemoveAll((MessageBuilder mb) => mb.Order == 13);
			}
			transactionItems.RemoveAll((MessageBuilder mb) => mb.Order == 5);
			transactionItems.Add(new ByteArrayMessageBuilder("BitMap", 5, new byte[1] { BitMap }, 1));
			return this;
		}
	}

	internal TransactionMessage SetField7(string value)
	{
		checked
		{
			if (!string.IsNullOrEmpty(value))
			{
				Field7 = ((value.Length <= 999) ? value.Length : 999).ToString().PadLeft(3, '0') + value;
				BitMap |= 0x40;
				transactionItems.Add(new StringMessageBuilder("Field 7", 12, Field7, 3, Encoding.ASCII, isPadRight: false, '0'));
			}
			else
			{
				Field7 = string.Empty;
				BitMap &= 0xBF;
				transactionItems.RemoveAll((MessageBuilder mb) => mb.Order == 12);
			}
			transactionItems.RemoveAll((MessageBuilder mb) => mb.Order == 5);
			transactionItems.Add(new ByteArrayMessageBuilder("BitMap", 5, new byte[1] { BitMap }, 1));
			return this;
		}
	}

	internal TransactionMessage SetFixedDuty(int providerId, int serviceCode, int count)
	{
		checked
		{
			if (providerId <= 0 || serviceCode <= 0 || count <= 0 || providerId > 999 || serviceCode > 9999 || count > 999)
			{
				Field2 = string.Empty;
				BitMap &= 0xFD;
				transactionItems.RemoveAll((MessageBuilder mb) => mb.Order == 7);
			}
			else
			{
				string text = providerId.ToString("D3") + serviceCode.ToString("D4") + count.ToString("D3");
				Field2 = ((text.Length <= 999) ? text.Length : 999).ToString().PadLeft(3, '0') + text;
				BitMap |= 2;
				transactionItems.Add(new StringMessageBuilder("Fixed Duty", 7, Field2, 3, Encoding.ASCII, isPadRight: false, '0'));
			}
			transactionItems.RemoveAll((MessageBuilder mb) => mb.Order == 5);
			transactionItems.Add(new ByteArrayMessageBuilder("BitMap", 5, new byte[1] { BitMap }, 1));
			return this;
		}
	}

	internal TransactionMessage SetFunctionCode(string functionCode)
	{
		FunctionCode = functionCode;
		transactionItems.Add(new StringMessageBuilder("Function Code", 2, functionCode, 4, Encoding.ASCII));
		return this;
	}

	internal TransactionMessage SetInvoiceNumber(string invoiceNumber)
	{
		InvoiceNumber = invoiceNumber;
		transactionItems.Add(new StringMessageBuilder("InvoiceNumber", 1, invoiceNumber, 10, Encoding.ASCII));
		return this;
	}

	internal TransactionMessage SetMessageType(string messageType)
	{
		MessageType = messageType;
		transactionItems.Add(new StringMessageBuilder("Message Type", 3, messageType, 4, Encoding.ASCII));
		return this;
	}

	internal TransactionMessage SetMultiAccount(string multiAccount)
	{
		checked
		{
			if (!string.IsNullOrEmpty(multiAccount))
			{
				Field1 = ((multiAccount.Length <= 999) ? multiAccount.Length : 999).ToString().PadLeft(3, '0') + multiAccount;
				BitMap |= 1;
				transactionItems.Add(new StringMessageBuilder("Multi-Iban", 6, Field1, 3, Encoding.ASCII, isPadRight: false, '0'));
			}
			else
			{
				Field1 = string.Empty;
				BitMap &= 0xFE;
				transactionItems.RemoveAll((MessageBuilder mb) => mb.Order == 6);
			}
			transactionItems.RemoveAll((MessageBuilder mb) => mb.Order == 5);
			transactionItems.Add(new ByteArrayMessageBuilder("BitMap", 5, new byte[1] { BitMap }, 1));
			return this;
		}
	}

	internal TransactionMessage SetOrderId(string orderId)
	{
		checked
		{
			if (!string.IsNullOrEmpty(orderId))
			{
				Field5 = ((orderId.Length <= 999) ? orderId.Length : 999).ToString().PadLeft(3, '0') + orderId;
				BitMap |= 0x10;
				transactionItems.Add(new StringMessageBuilder("Order Id", 10, Field5, 3, Encoding.ASCII, isPadRight: false, '0'));
			}
			else
			{
				Field5 = string.Empty;
				BitMap &= 0xEF;
				transactionItems.RemoveAll((MessageBuilder mb) => mb.Order == 10);
			}
			transactionItems.RemoveAll((MessageBuilder mb) => mb.Order == 5);
			transactionItems.Add(new ByteArrayMessageBuilder("BitMap", 5, new byte[1] { BitMap }, 1));
			return this;
		}
	}

	internal TransactionMessage SetProcessingCode(string processingCode)
	{
		ProcessingCode = processingCode;
		transactionItems.Add(new StringMessageBuilder("Processing Code", 4, processingCode, 6, Encoding.ASCII));
		return this;
	}

	internal TransactionMessage SetSaleId(string saleId)
	{
		checked
		{
			if (!string.IsNullOrEmpty(saleId))
			{
				Field2 = ((saleId.Length <= 999) ? saleId.Length : 999).ToString().PadLeft(3, '0') + saleId;
				BitMap |= 2;
				transactionItems.Add(new StringMessageBuilder("Sale Id", 7, Field2, 3, Encoding.ASCII, isPadRight: false, '0'));
			}
			else
			{
				Field2 = string.Empty;
				BitMap &= 0xFD;
				transactionItems.RemoveAll((MessageBuilder mb) => mb.Order == 7);
			}
			transactionItems.RemoveAll((MessageBuilder mb) => mb.Order == 5);
			transactionItems.Add(new ByteArrayMessageBuilder("BitMap", 5, new byte[1] { BitMap }, 1));
			return this;
		}
	}

	internal TransactionMessage SetSaleId(SaleIdProvider bcdSaleId)
	{
		checked
		{
			if (bcdSaleId == null || bcdSaleId.Count <= 0)
			{
				BitMap &= 0xFD;
				transactionItems.RemoveAll((MessageBuilder mb) => mb.Order == 7);
			}
			else
			{
				BitMap |= 2;
				byte[] array = bcdSaleId.ToBcd();
				List<byte> list = new List<byte>(Utility.ConvertToBinaryCodedDecimal(isLittleEndian: false, ((array.Length <= 999) ? array.Length : 999).ToString().PadLeft(3, '0')));
				list.AddRange(array);
				transactionItems.Add(new ByteArrayMessageBuilder("Multi Sale Id", 7, list.ToArray(), 3));
			}
			transactionItems.RemoveAll((MessageBuilder mb) => mb.Order == 5);
			transactionItems.Add(new ByteArrayMessageBuilder("BitMap", 5, new byte[1] { BitMap }, 1));
			return this;
		}
	}

	public new void Sort()
	{
		transactionItems.Sort(new StringMessageBuilder());
	}
}
