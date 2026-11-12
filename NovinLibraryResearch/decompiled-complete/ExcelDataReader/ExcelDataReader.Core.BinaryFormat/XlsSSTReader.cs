using System;

namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsSSTReader
{
	private XlsBiffSST Sst { get; }

	private XlsBiffStream BiffStream { get; }

	private XlsBiffRecord CurrentRecord { get; set; }

	private int CurrentRecordOffset { get; set; }

	public XlsSSTReader(XlsBiffSST sst, XlsBiffStream biffStream)
	{
		Sst = sst;
		BiffStream = biffStream;
		CurrentRecord = Sst;
		CurrentRecordOffset = 12;
	}

	public IXlsString ReadString()
	{
		EnsureRecord();
		XlsSSTStringHeader xlsSSTStringHeader = new XlsSSTStringHeader(CurrentRecord.Bytes, (uint)(CurrentRecord.Offset + CurrentRecordOffset));
		Advance((int)xlsSSTStringHeader.HeadSize);
		int num = xlsSSTStringHeader.CharacterCount;
		byte[] array = new byte[3 + num * 2];
		array[0] = (byte)((uint)num & 0xFFu);
		array[1] = (byte)((num & 0xFF00) >> 8);
		array[2] = 1;
		int num2 = 3;
		bool flag = xlsSSTStringHeader.IsMultiByte;
		while (num > 0)
		{
			if (EnsureRecord())
			{
				flag = ReadByte() != 0;
			}
			int num3 = ((!flag) ? 1 : 2);
			int num4 = Math.Min((CurrentRecord.Size - CurrentRecordOffset) / num3, num);
			ReadUnicodeBytes(array, num2, num4, flag);
			num2 += num4 * 2;
			num -= num4;
		}
		Advance((int)xlsSSTStringHeader.TailSize);
		return new XlsUnicodeString(array, 0u);
	}

	private void ReadUnicodeBytes(byte[] dest, int offset, int characterCount, bool isMultiByte)
	{
		if (CurrentRecordOffset >= CurrentRecord.Size)
		{
			throw new InvalidOperationException("SST read position out of range");
		}
		if (characterCount == 0)
		{
			throw new InvalidOperationException("Bad SST format");
		}
		if (isMultiByte)
		{
			Array.Copy(CurrentRecord.Bytes, CurrentRecord.Offset + CurrentRecordOffset, dest, offset, characterCount * 2);
			CurrentRecordOffset += characterCount * 2;
			return;
		}
		for (int i = 0; i < characterCount; i++)
		{
			dest[offset + i * 2] = CurrentRecord.Bytes[CurrentRecord.Offset + CurrentRecordOffset + i];
			dest[offset + i * 2 + 1] = 0;
		}
		CurrentRecordOffset += characterCount;
	}

	private byte ReadByte()
	{
		if (CurrentRecordOffset >= CurrentRecord.Size)
		{
			throw new InvalidOperationException("SST read position out of range");
		}
		byte result = CurrentRecord.Bytes[CurrentRecord.Offset + CurrentRecordOffset];
		CurrentRecordOffset++;
		return result;
	}

	private bool EnsureRecord()
	{
		if (CurrentRecordOffset == CurrentRecord.Size)
		{
			CurrentRecord = BiffStream.Read();
			if (CurrentRecord == null || CurrentRecord.Id != BIFFRECORDTYPE.CONTINUE)
			{
				throw new InvalidOperationException("Bad SST format");
			}
			CurrentRecordOffset = 4;
			return true;
		}
		return false;
	}

	private void Advance(int bytes)
	{
		int size = CurrentRecord.Size;
		while (CurrentRecordOffset + bytes > size)
		{
			bytes = Math.Min(CurrentRecordOffset + bytes - size, bytes);
			CurrentRecordOffset = CurrentRecord.Size;
			EnsureRecord();
			size = CurrentRecord.Size;
		}
		CurrentRecordOffset += bytes;
	}
}
