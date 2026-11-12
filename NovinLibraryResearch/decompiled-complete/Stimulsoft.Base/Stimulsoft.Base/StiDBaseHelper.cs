using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;

namespace Stimulsoft.Base;

public class StiDBaseHelper
{
	private class DBaseHeader
	{
		public byte TableType;

		public int Year;

		public int Month;

		public int Day;

		public uint RecordsCount;

		public uint HeaderSize;

		public uint RecordSize;

		public byte CodePage;
	}

	private enum DBaseFieldType
	{
		Unknown,
		Char,
		Date,
		Float,
		Numeric,
		Logical,
		Memo,
		Variable,
		Picture,
		Binary,
		General,
		ShortInt,
		LongInt,
		Double,
		Blob
	}

	[Flags]
	private enum DBaseFieldFlags
	{
		System = 1,
		MayBeNull = 2,
		Binary = 4,
		AutoIncrement = 8
	}

	private class DBaseFieldInfo
	{
		public string Name;

		public DBaseFieldType Type;

		public int Offset;

		public int Size;

		public int FractionSize;

		public DBaseFieldFlags Flags;
	}

	public static int[,] CodePageCodes = new int[27, 2]
	{
		{ 0, 0 },
		{ 1, 437 },
		{ 105, 620 },
		{ 106, 737 },
		{ 2, 850 },
		{ 100, 852 },
		{ 107, 857 },
		{ 103, 861 },
		{ 102, 865 },
		{ 101, 866 },
		{ 124, 874 },
		{ 104, 895 },
		{ 123, 932 },
		{ 122, 936 },
		{ 121, 949 },
		{ 120, 950 },
		{ 200, 1250 },
		{ 201, 1251 },
		{ 3, 1252 },
		{ 203, 1253 },
		{ 202, 1254 },
		{ 125, 1255 },
		{ 126, 1256 },
		{ 4, 10000 },
		{ 152, 10006 },
		{ 150, 10007 },
		{ 151, 10029 }
	};

	public static string[] CodePageNames = new string[27]
	{
		"Default", "437  U.S. MS-DOS", "620  Mazovia (Polish) MS-DOS", "737  Greek MS-DOS (437G)", "850  International MS-DOS", "852  Eastern European MS-DOS", "857  Turkish MS-DOS", "861  Icelandic MS-DOS", "865  Nordic MS-DOS", "866  Russian MS-DOS",
		"874  Thai Windows", "895  Kamenicky (Czech) MS-DOS", "932  Japanese Windows", "936  Chinese Simplified (PRC, Singapore) Windows", "949  Korean Windows", "950  Traditional Chinese (Hong Kong SAR, Taiwan) Windows", "1250  EasternEuropean MS-DOS", "1251  Russian Windows", "1252  Windows ANSI", "1253  Greek Windows",
		"1254  Turkish Windows", "1255  Hebrew Windows", "1256  Arabic Windows", "10000  Standard Macintosh", "10006  Greek Macintosh", "10007  Russian Macintosh", "10029  Eastern European Macintosh"
	};

	private const int DBaseHeaderLength = 32;

	private const int DBaseFieldInfoLength = 32;

	public static DataTable GetTable(string path, int maxDataRows = 0)
	{
		return GetTable(path, 0, maxDataRows);
	}

	public static DataTable GetTable(string path, int codePage, int maxDataRows = 0)
	{
		return GetTable(path, codePage != 0, codePage, maxDataRows);
	}

	private static DataTable GetTable(string path, bool forceCodePage, int forcedCodePage, int maxDataRows = 0)
	{
		DataTable result = null;
		using (Stream memoryStream = new FileStream(path, FileMode.Open))
		{
			string path2 = Path.ChangeExtension(path, "fpt");
			if (File.Exists(path2))
			{
				using Stream memo = new FileStream(path2, FileMode.Open);
				result = GetTable(memoryStream, memo, forceCodePage, forcedCodePage, maxDataRows);
			}
			else
			{
				result = GetTable(memoryStream, null, forceCodePage, forcedCodePage, maxDataRows);
			}
		}
		return result;
	}

	public static DataSet GetDataSet(byte[] data, string tableName, bool forceCodePage, int forcedCodePage, int maxDataRows = 0)
	{
		DataTable dataTable = null;
		using (MemoryStream memoryStream = new MemoryStream(data))
		{
			dataTable = GetTable(memoryStream, null, forceCodePage, forcedCodePage, maxDataRows);
		}
		if (dataTable == null)
		{
			return null;
		}
		dataTable.TableName = (string.IsNullOrWhiteSpace(tableName) ? "DBase-Table" : tableName);
		DataSet dataSet = new DataSet
		{
			EnforceConstraints = false
		};
		dataSet.Tables.Add(dataTable);
		return dataSet;
	}

	public static DataTable GetTable(byte[] data, int codePage, int maxDataRows = 0)
	{
		using MemoryStream memoryStream = new MemoryStream(data);
		return GetTable(memoryStream, null, codePage != 0, codePage, maxDataRows);
	}

	public static DataTable GetTable(Stream memoryStream, Stream memo, bool forceCodePage, int forcedCodePage, int maxDataRows = 0)
	{
		memoryStream.Seek(0L, SeekOrigin.Begin);
		byte[] array = new byte[32];
		memoryStream.Read(array, 0, array.Length);
		DBaseHeader dBaseHeader = new DBaseHeader
		{
			TableType = array[0],
			Year = 2000 + array[1],
			Month = array[2],
			Day = array[3],
			RecordsCount = BitConverter.ToUInt32(array, 4),
			HeaderSize = BitConverter.ToUInt16(array, 8),
			RecordSize = BitConverter.ToUInt16(array, 10),
			CodePage = array[29]
		};
		int num = 0;
		for (int i = 0; i < CodePageCodes.GetLength(0); i++)
		{
			if (CodePageCodes[i, 0] == dBaseHeader.CodePage)
			{
				num = CodePageCodes[i, 1];
				break;
			}
		}
		if (forceCodePage)
		{
			num = forcedCodePage;
		}
		Decoder decoder = ((num == 0) ? Encoding.Default.GetDecoder() : Encoding.GetEncoding(num).GetDecoder());
		uint num2 = (dBaseHeader.HeaderSize - 32 - 1) / 32;
		byte[] array2 = new byte[dBaseHeader.HeaderSize - 32];
		memoryStream.Read(array2, 0, array2.Length);
		List<DBaseFieldInfo> list = new List<DBaseFieldInfo>();
		for (int j = 0; j < num2; j++)
		{
			int num3 = j * 32;
			if (array2[num3] == 13)
			{
				num2 = (uint)j;
				break;
			}
			list.Add(new DBaseFieldInfo());
			int k;
			for (k = 0; k < 10 && array2[num3 + k] != 0; k++)
			{
			}
			char[] array3 = new char[decoder.GetCharCount(array2, num3, k)];
			decoder.GetChars(array2, num3, k, array3, 0);
			list[j].Name = new string(array3);
			DBaseFieldType type = DBaseFieldType.Unknown;
			switch ((char)array2[num3 + 11])
			{
			case 'C':
				type = DBaseFieldType.Char;
				break;
			case 'D':
				type = DBaseFieldType.Date;
				break;
			case 'F':
				type = DBaseFieldType.Float;
				break;
			case 'N':
				type = DBaseFieldType.Numeric;
				break;
			case 'L':
				type = DBaseFieldType.Logical;
				break;
			case 'M':
				type = DBaseFieldType.Memo;
				break;
			case 'V':
				type = DBaseFieldType.Variable;
				break;
			case 'P':
				type = DBaseFieldType.Picture;
				break;
			case 'B':
				type = DBaseFieldType.Binary;
				break;
			case 'W':
				type = DBaseFieldType.Blob;
				break;
			case 'G':
				type = DBaseFieldType.General;
				break;
			case '2':
				type = DBaseFieldType.ShortInt;
				break;
			case '4':
			case 'I':
				type = DBaseFieldType.LongInt;
				break;
			case '8':
				type = DBaseFieldType.Double;
				break;
			}
			list[j].Type = type;
			list[j].Offset = BitConverter.ToInt32(array2, num3 + 12);
			list[j].Size = array2[num3 + 16];
			list[j].FractionSize = array2[num3 + 17];
			list[j].Flags = (DBaseFieldFlags)array2[num3 + 18];
		}
		DataTable dataTable = new DataTable("DBase-Table");
		foreach (DBaseFieldInfo item in list)
		{
			Type dataType;
			switch (item.Type)
			{
			case DBaseFieldType.Char:
				dataType = typeof(string);
				break;
			case DBaseFieldType.Date:
				dataType = typeof(DateTime);
				break;
			case DBaseFieldType.Float:
				dataType = typeof(decimal);
				break;
			case DBaseFieldType.Numeric:
				dataType = ((item.FractionSize != 0 || item.Size >= 10) ? typeof(decimal) : typeof(int));
				break;
			case DBaseFieldType.ShortInt:
			case DBaseFieldType.LongInt:
				dataType = typeof(int);
				break;
			case DBaseFieldType.Logical:
				dataType = typeof(bool);
				break;
			case DBaseFieldType.Memo:
				dataType = (((item.Flags & DBaseFieldFlags.Binary) <= (DBaseFieldFlags)0) ? typeof(string) : typeof(object));
				break;
			case DBaseFieldType.Picture:
			case DBaseFieldType.Binary:
			case DBaseFieldType.General:
			case DBaseFieldType.Blob:
				dataType = typeof(object);
				break;
			default:
				dataType = typeof(object);
				break;
			}
			DataColumn column = new DataColumn(item.Name, dataType);
			dataTable.Columns.Add(column);
		}
		dataTable.Locale = CultureInfo.InvariantCulture;
		int num4 = 0;
		if (memo != null)
		{
			memo.Seek(6L, SeekOrigin.Begin);
			num4 = memo.ReadByte() * 256 + memo.ReadByte();
		}
		byte[] array4 = new byte[8];
		byte[] array5 = new byte[dBaseHeader.RecordSize];
		string numberDecimalSeparator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
		for (int l = 0; l < dBaseHeader.RecordsCount; l++)
		{
			memoryStream.Read(array5, 0, array5.Length);
			if (array5[0] == 42)
			{
				continue;
			}
			DataRow dataRow = dataTable.NewRow();
			int num5 = 1;
			for (int m = 0; m < list.Count; m++)
			{
				string text = string.Empty;
				DBaseFieldType type2 = list[m].Type;
				if ((uint)(type2 - 1) <= 3u)
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int n = 0; n < list[m].Size; n++)
					{
						stringBuilder.Append((char)array5[num5 + n]);
					}
					text = stringBuilder.ToString().TrimEnd(new char[0]);
				}
				switch (list[m].Type)
				{
				case DBaseFieldType.Char:
				{
					if ((list[m].Flags & DBaseFieldFlags.Binary) > (DBaseFieldFlags)0)
					{
						dataRow[m] = text;
						break;
					}
					char[] array6 = new char[list[m].Size];
					decoder.GetChars(array5, num5, list[m].Size, array6, 0);
					dataRow[m] = new string(array6);
					break;
				}
				case DBaseFieldType.Date:
					if (!string.IsNullOrWhiteSpace(text))
					{
						try
						{
							dataRow[m] = new DateTime(int.Parse(text.Substring(0, 4)), int.Parse(text.Substring(4, 2)), int.Parse(text.Substring(6, 2)));
						}
						catch
						{
						}
					}
					break;
				case DBaseFieldType.Float:
				case DBaseFieldType.Numeric:
					if (text.Length <= 0)
					{
						break;
					}
					if (list[m].FractionSize == 0 && list[m].Size < 10)
					{
						try
						{
							dataRow[m] = int.Parse(text);
						}
						catch
						{
						}
					}
					else
					{
						try
						{
							dataRow[m] = decimal.Parse(text.Replace(".", numberDecimalSeparator));
						}
						catch
						{
						}
					}
					break;
				case DBaseFieldType.LongInt:
					dataRow[m] = BitConverter.ToInt32(array5, num5);
					break;
				case DBaseFieldType.Memo:
				case DBaseFieldType.Picture:
				case DBaseFieldType.Binary:
				case DBaseFieldType.General:
				case DBaseFieldType.Blob:
				{
					int num6 = BitConverter.ToInt32(array5, num5);
					if (num6 <= 0 || memo == null)
					{
						break;
					}
					try
					{
						memo.Seek(num6 * num4, SeekOrigin.Begin);
						memo.Read(array4, 0, 8);
						int num7 = array4[3];
						int num8 = array4[4] * 256 * 256 * 256 + array4[5] * 256 * 256 + array4[6] * 256 + array4[7];
						byte[] array7 = new byte[num8];
						memo.Read(array7, 0, num8);
						if (list[m].Type == DBaseFieldType.Memo && (list[m].Flags & DBaseFieldFlags.Binary) == 0)
						{
							char[] array8 = new char[num8];
							decoder.GetChars(array7, 0, num8, array8, 0);
							dataRow[m] = new string(array8);
						}
						else
						{
							dataRow[m] = array7;
						}
					}
					catch
					{
					}
					break;
				}
				case DBaseFieldType.Logical:
				{
					char c = (char)array5[num5];
					if (c == 'T' || c == 't' || c == 'Y' || c == 'y')
					{
						dataRow[m] = true;
					}
					if (c == 'F' || c == 'f' || c == 'N' || c == 'n')
					{
						dataRow[m] = true;
					}
					break;
				}
				default:
					dataRow[m] = DBNull.Value;
					break;
				}
				num5 += list[m].Size;
			}
			dataTable.Rows.Add(dataRow);
			if (maxDataRows != 0 && maxDataRows < dataTable.Rows.Count)
			{
				break;
			}
		}
		return dataTable;
	}
}
