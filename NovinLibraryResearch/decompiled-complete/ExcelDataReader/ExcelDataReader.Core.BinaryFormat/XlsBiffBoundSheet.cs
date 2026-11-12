using System;
using System.Text;

namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsBiffBoundSheet : XlsBiffRecord
{
	public enum SheetType : byte
	{
		Worksheet = 0,
		MacroSheet = 1,
		Chart = 2,
		VBModule = 6
	}

	public enum SheetVisibility : byte
	{
		Visible,
		Hidden,
		VeryHidden
	}

	private readonly IXlsString _sheetName;

	public uint StartOffset { get; }

	public SheetType Type { get; }

	public SheetVisibility VisibleState { get; }

	internal XlsBiffBoundSheet(byte[] bytes, uint offset, int biffVersion)
		: base(bytes, offset)
	{
		StartOffset = ReadUInt32(0);
		Type = (SheetType)ReadByte(5);
		VisibleState = (SheetVisibility)ReadByte(4);
		switch (biffVersion)
		{
		case 8:
			_sheetName = new XlsShortUnicodeString(bytes, offset + 4 + 6);
			break;
		case 5:
			_sheetName = new XlsShortByteString(bytes, offset + 4 + 6);
			break;
		default:
			throw new ArgumentException("Unexpected BIFF version " + biffVersion, "biffVersion");
		}
	}

	internal XlsBiffBoundSheet(uint startOffset, SheetType type, SheetVisibility visibleState, string name)
		: base(new byte[32], 0u)
	{
		StartOffset = startOffset;
		Type = type;
		VisibleState = visibleState;
		_sheetName = new XlsInternalString(name);
	}

	public string GetSheetName(Encoding encoding)
	{
		return _sheetName.GetValue(encoding);
	}
}
