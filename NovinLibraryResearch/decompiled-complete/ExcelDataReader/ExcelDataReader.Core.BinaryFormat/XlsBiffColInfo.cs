using System;

namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsBiffColInfo : XlsBiffRecord
{
	[Flags]
	private enum ColInfoSettings
	{
		Hidden = 1,
		UserSet = 0x10
	}

	public Col Value { get; }

	public XlsBiffColInfo(byte[] bytes, uint offset)
		: base(bytes, offset)
	{
		ushort min = ReadUInt16(0);
		ushort max = ReadUInt16(2);
		ushort num = ReadUInt16(4);
		ushort num2 = ReadUInt16(8);
		bool customWidth = (num2 & 0x10) != 0;
		bool hidden = (num2 & 1) != 0;
		Value = new Col
		{
			CustomWidth = customWidth,
			Max = max,
			Min = min,
			Width = (double)(int)num / 256.0,
			Hidden = hidden
		};
	}
}
