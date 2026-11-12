using System;

namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsBiffFormulaCell : XlsBiffBlankCell
{
	[Flags]
	public enum FormulaFlags : ushort
	{
		AlwaysCalc = 1,
		CalcOnLoad = 2,
		SharedFormulaGroup = 8
	}

	public enum FormulaValueType
	{
		Unknown,
		String,
		EmptyString,
		Boolean,
		Error,
		Number
	}

	public FormulaFlags Flags { get; }

	public FormulaValueType FormulaType { get; }

	public bool BooleanValue { get; }

	public FORMULAERROR ErrorValue { get; }

	public double XNumValue { get; }

	internal XlsBiffFormulaCell(byte[] bytes, uint offset, int biffVersion)
		: base(bytes, offset, biffVersion)
	{
		if (biffVersion == 2)
		{
			Flags = (FormulaFlags)ReadUInt16(15);
			XNumValue = ReadDouble(7);
			FormulaType = FormulaValueType.Number;
			return;
		}
		Flags = (FormulaFlags)ReadUInt16(14);
		if (ReadUInt16(12) != ushort.MaxValue)
		{
			FormulaType = FormulaValueType.Number;
			XNumValue = ReadDouble(6);
			return;
		}
		byte b = ReadByte(6);
		byte b2 = ReadByte(8);
		ReadByte(15);
		switch (b)
		{
		case 0:
			FormulaType = FormulaValueType.String;
			break;
		case 1:
			FormulaType = FormulaValueType.Boolean;
			BooleanValue = b2 != 0;
			break;
		case 2:
			FormulaType = FormulaValueType.Error;
			ErrorValue = (FORMULAERROR)b2;
			break;
		case 3:
			FormulaType = FormulaValueType.EmptyString;
			break;
		default:
			FormulaType = FormulaValueType.Unknown;
			break;
		}
	}
}
