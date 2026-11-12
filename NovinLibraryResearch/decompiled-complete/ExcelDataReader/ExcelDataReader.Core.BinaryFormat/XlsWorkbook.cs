using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ExcelDataReader.Core.OfficeCrypto;
using ExcelDataReader.Exceptions;

namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsWorkbook : CommonWorkbook, IWorkbook<XlsWorksheet>
{
	public Stream Stream { get; }

	public int BiffVersion { get; }

	public byte[] SecretKey { get; }

	public EncryptionInfo Encryption { get; }

	public Encoding Encoding { get; private set; }

	public XlsBiffInterfaceHdr InterfaceHdr { get; set; }

	public XlsBiffRecord Mms { get; set; }

	public XlsBiffRecord WriteAccess { get; set; }

	public XlsBiffSimpleValueRecord CodePage { get; set; }

	public XlsBiffRecord Dsf { get; set; }

	public XlsBiffRecord Country { get; set; }

	public XlsBiffSimpleValueRecord Backup { get; set; }

	public List<XlsBiffRecord> Fonts { get; } = new List<XlsBiffRecord>();


	public List<XlsBiffBoundSheet> Sheets { get; } = new List<XlsBiffBoundSheet>();


	public XlsBiffSST SST { get; set; }

	public XlsBiffRecord ExtSST { get; set; }

	public bool IsDate1904 { get; private set; }

	public int ResultsCount => Sheets?.Count ?? (-1);

	internal XlsWorkbook(Stream stream, string password, Encoding fallbackEncoding)
	{
		Stream = stream;
		using XlsBiffStream xlsBiffStream = new XlsBiffStream(stream, 0, 0, password);
		if (xlsBiffStream.BiffVersion == 0)
		{
			throw new ExcelReaderException("Error reading Workbook Globals - Stream has invalid data.");
		}
		BiffVersion = xlsBiffStream.BiffVersion;
		SecretKey = xlsBiffStream.SecretKey;
		Encryption = xlsBiffStream.Encryption;
		Encoding = ((xlsBiffStream.BiffVersion == 8) ? Encoding.Unicode : fallbackEncoding);
		if (xlsBiffStream.BiffType == BIFFTYPE.WorkbookGlobals)
		{
			ReadWorkbookGlobals(xlsBiffStream);
			return;
		}
		if (xlsBiffStream.BiffType == BIFFTYPE.Worksheet)
		{
			Sheets.Add(new XlsBiffBoundSheet(0u, XlsBiffBoundSheet.SheetType.Worksheet, XlsBiffBoundSheet.SheetVisibility.Visible, "Sheet"));
			return;
		}
		throw new ExcelReaderException("Error reading Workbook Globals - Stream has invalid data.");
	}

	public static bool IsRawBiffStream(byte[] bytes)
	{
		if (bytes.Length < 8)
		{
			throw new ArgumentException("Needs at least 8 bytes to probe", "bytes");
		}
		ushort num = BitConverter.ToUInt16(bytes, 0);
		ushort num2 = BitConverter.ToUInt16(bytes, 2);
		ushort num3 = BitConverter.ToUInt16(bytes, 4);
		ushort num4 = BitConverter.ToUInt16(bytes, 6);
		switch (num)
		{
		case 9:
			if (num2 != 4)
			{
				return false;
			}
			if (num4 != 16 && num4 != 32 && num4 != 64)
			{
				return false;
			}
			return true;
		case 521:
		case 1033:
			if (num2 != 6)
			{
				return false;
			}
			if (num4 != 16 && num4 != 32 && num4 != 64 && num4 != 256)
			{
				return false;
			}
			return true;
		case 2057:
			if (num2 < 4)
			{
				return false;
			}
			if (num3 != 0 && num3 != 512 && num3 != 768 && num3 != 1024 && num3 != 1280 && num3 != 1536)
			{
				return false;
			}
			if (num4 != 5 && num4 != 6 && num4 != 16 && num4 != 32 && num4 != 64 && num4 != 256)
			{
				return false;
			}
			return true;
		default:
			return false;
		}
	}

	public IEnumerable<XlsWorksheet> ReadWorksheets()
	{
		int i = 0;
		while (i < Sheets.Count)
		{
			yield return new XlsWorksheet(this, Sheets[i], Stream);
			int num = i + 1;
			i = num;
		}
	}

	private void ReadWorkbookGlobals(XlsBiffStream biffStream)
	{
		Dictionary<ushort, XlsBiffFormatString> dictionary = new Dictionary<ushort, XlsBiffFormatString>();
		XlsBiffRecord xlsBiffRecord;
		while ((xlsBiffRecord = biffStream.Read()) != null && xlsBiffRecord.Id != BIFFRECORDTYPE.EOF)
		{
			switch (xlsBiffRecord.Id)
			{
			case BIFFRECORDTYPE.INTERFACEHDR:
				InterfaceHdr = (XlsBiffInterfaceHdr)xlsBiffRecord;
				break;
			case BIFFRECORDTYPE.BOUNDSHEET:
			{
				XlsBiffBoundSheet xlsBiffBoundSheet = (XlsBiffBoundSheet)xlsBiffRecord;
				if (xlsBiffBoundSheet.Type == XlsBiffBoundSheet.SheetType.Worksheet)
				{
					Sheets.Add(xlsBiffBoundSheet);
				}
				break;
			}
			case BIFFRECORDTYPE.MMS:
				Mms = xlsBiffRecord;
				break;
			case BIFFRECORDTYPE.COUNTRY:
				Country = xlsBiffRecord;
				break;
			case BIFFRECORDTYPE.CODEPAGE:
				CodePage = (XlsBiffSimpleValueRecord)xlsBiffRecord;
				Encoding = EncodingHelper.GetEncoding(CodePage.Value);
				break;
			case BIFFRECORDTYPE.FONT:
			case BIFFRECORDTYPE.FONT_V34:
				Fonts.Add(xlsBiffRecord);
				break;
			case BIFFRECORDTYPE.FORMAT_V23:
			{
				XlsBiffFormatString value = (XlsBiffFormatString)xlsBiffRecord;
				dictionary.Add((ushort)dictionary.Count, value);
				break;
			}
			case BIFFRECORDTYPE.FORMAT:
			{
				XlsBiffFormatString xlsBiffFormatString = (XlsBiffFormatString)xlsBiffRecord;
				dictionary.Add(xlsBiffFormatString.Index, xlsBiffFormatString);
				break;
			}
			case BIFFRECORDTYPE.XF_V2:
			case BIFFRECORDTYPE.XF:
			case BIFFRECORDTYPE.XF_V3:
			case BIFFRECORDTYPE.XF_V4:
				AddExtendedFormat(GetExtendedFormatCount(), ((XlsBiffXF)xlsBiffRecord).Format, applyNumberFormat: true);
				break;
			case BIFFRECORDTYPE.SST:
				SST = (XlsBiffSST)xlsBiffRecord;
				SST.ReadStrings(biffStream);
				break;
			case BIFFRECORDTYPE.EXTSST:
				ExtSST = xlsBiffRecord;
				break;
			case BIFFRECORDTYPE.RECORD1904:
				IsDate1904 = ((XlsBiffSimpleValueRecord)xlsBiffRecord).Value == 1;
				break;
			}
		}
		foreach (KeyValuePair<ushort, XlsBiffFormatString> item in dictionary)
		{
			AddNumberFormat(item.Key, item.Value.GetValue(Encoding));
		}
	}
}
