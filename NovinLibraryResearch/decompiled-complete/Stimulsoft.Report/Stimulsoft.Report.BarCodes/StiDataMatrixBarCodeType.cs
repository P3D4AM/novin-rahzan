using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.BarCodes.Design;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.BarCodes;

[TypeConverter(typeof(StiDataMatrixBarCodeTypeConverter))]
public class StiDataMatrixBarCodeType : StiBarCodeTypeService
{
	protected class StiDataMatrixException : Exception
	{
		public StiDataMatrixException()
		{
		}

		public StiDataMatrixException(string message)
			: base(message)
		{
		}

		public StiDataMatrixException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}

	protected class StiDataMatrix
	{
		public enum CommandCode
		{
			Padding = 129,
			ModeC40 = 230,
			ModeBinary = 231,
			FNC1 = 232,
			ModeX12 = 238,
			ModeText = 239,
			ModeEdifact = 240,
			EscapeToAscii = 254
		}

		public class ReedSolomon
		{
			private int logmod;

			private int rlen;

			private int[] log;

			private int[] alog;

			private int[] rspoly;

			public ReedSolomon(int nsym)
			{
				logmod = 255;
				log = new int[logmod + 1];
				alog = new int[logmod];
				int num = 1;
				for (int i = 0; i < logmod; i++)
				{
					alog[i] = num;
					log[num] = i;
					num <<= 1;
					if (num > 255)
					{
						num ^= 0x12D;
					}
				}
				rlen = nsym;
				rspoly = new int[nsym + 1];
				int num2 = 1;
				rspoly[0] = 1;
				for (int j = 1; j <= nsym; j++)
				{
					rspoly[j] = 1;
					for (int num3 = j - 1; num3 > 0; num3--)
					{
						if (rspoly[num3] != 0)
						{
							rspoly[num3] = alog[(log[rspoly[num3]] + num2) % logmod];
						}
						rspoly[num3] ^= rspoly[num3 - 1];
					}
					rspoly[0] = alog[(log[rspoly[0]] + num2) % logmod];
					num2++;
				}
			}

			public byte[] Encode(int len, byte[] data)
			{
				byte[] array = new byte[rlen];
				for (int i = 0; i < len; i++)
				{
					int num = array[rlen - 1] ^ data[i];
					for (int num2 = rlen - 1; num2 > 0; num2--)
					{
						if (num != 0 && rspoly[num2] != 0)
						{
							array[num2] = (byte)(array[num2 - 1] ^ alog[(log[num] + log[rspoly[num2]]) % logmod]);
						}
						else
						{
							array[num2] = array[num2 - 1];
						}
					}
					if (num != 0 && rspoly[0] != 0)
					{
						array[0] = (byte)alog[(log[num] + log[rspoly[0]]) % logmod];
					}
					else
					{
						array[0] = 0;
					}
				}
				return array;
			}
		}

		private struct ecc200ListItem
		{
			public int Height;

			public int Width;

			public int FH;

			public int FW;

			public int Bytes;

			public int Datablock;

			public int RSblock;

			public ecc200ListItem(int h, int w, int fh, int fw, int bytes, int datablock, int rsblock)
			{
				Height = h;
				Width = w;
				FH = fh;
				FW = fw;
				Bytes = bytes;
				Datablock = datablock;
				RSblock = rsblock;
			}
		}

		private int gridWidth;

		private int gridHeight;

		private byte[] grid;

		private string errorMessage;

		private ecc200ListItem[] ecc200List;

		private bool processTilde;

		public byte[] Matrix => grid;

		public int Width => gridWidth;

		public int Height => gridHeight;

		public string ErrorMessage => errorMessage;

		private void DataMatrixPlacementbit(int[] array, int numRows, int numColumns, int row, int column, int pos, byte b)
		{
			if (row < 0)
			{
				row += numRows;
				column += 4 - (numRows + 4) % 8;
			}
			if (column < 0)
			{
				column += numColumns;
				row += 4 - (numColumns + 4) % 8;
			}
			array[row * numColumns + column] = (pos << 3) + b;
		}

		private void DataMatrixPlacementBlock(int[] array, int numRows, int numColumns, int row, int column, int pos)
		{
			DataMatrixPlacementbit(array, numRows, numColumns, row - 2, column - 2, pos, 7);
			DataMatrixPlacementbit(array, numRows, numColumns, row - 2, column - 1, pos, 6);
			DataMatrixPlacementbit(array, numRows, numColumns, row - 1, column - 2, pos, 5);
			DataMatrixPlacementbit(array, numRows, numColumns, row - 1, column - 1, pos, 4);
			DataMatrixPlacementbit(array, numRows, numColumns, row - 1, column, pos, 3);
			DataMatrixPlacementbit(array, numRows, numColumns, row, column - 2, pos, 2);
			DataMatrixPlacementbit(array, numRows, numColumns, row, column - 1, pos, 1);
			DataMatrixPlacementbit(array, numRows, numColumns, row, column, pos, 0);
		}

		private void DataMatrixPlacementCornerA(int[] array, int numRows, int numColumns, int pos)
		{
			DataMatrixPlacementbit(array, numRows, numColumns, numRows - 1, 0, pos, 7);
			DataMatrixPlacementbit(array, numRows, numColumns, numRows - 1, 1, pos, 6);
			DataMatrixPlacementbit(array, numRows, numColumns, numRows - 1, 2, pos, 5);
			DataMatrixPlacementbit(array, numRows, numColumns, 0, numColumns - 2, pos, 4);
			DataMatrixPlacementbit(array, numRows, numColumns, 0, numColumns - 1, pos, 3);
			DataMatrixPlacementbit(array, numRows, numColumns, 1, numColumns - 1, pos, 2);
			DataMatrixPlacementbit(array, numRows, numColumns, 2, numColumns - 1, pos, 1);
			DataMatrixPlacementbit(array, numRows, numColumns, 3, numColumns - 1, pos, 0);
		}

		private void DataMatrixPlacementCornerB(int[] array, int numRows, int numColumns, int pos)
		{
			DataMatrixPlacementbit(array, numRows, numColumns, numRows - 3, 0, pos, 7);
			DataMatrixPlacementbit(array, numRows, numColumns, numRows - 2, 0, pos, 6);
			DataMatrixPlacementbit(array, numRows, numColumns, numRows - 1, 0, pos, 5);
			DataMatrixPlacementbit(array, numRows, numColumns, 0, numColumns - 4, pos, 4);
			DataMatrixPlacementbit(array, numRows, numColumns, 0, numColumns - 3, pos, 3);
			DataMatrixPlacementbit(array, numRows, numColumns, 0, numColumns - 2, pos, 2);
			DataMatrixPlacementbit(array, numRows, numColumns, 0, numColumns - 1, pos, 1);
			DataMatrixPlacementbit(array, numRows, numColumns, 1, numColumns - 1, pos, 0);
		}

		private void DataMatrixPlacementCornerC(int[] array, int numRows, int numColumns, int pos)
		{
			DataMatrixPlacementbit(array, numRows, numColumns, numRows - 3, 0, pos, 7);
			DataMatrixPlacementbit(array, numRows, numColumns, numRows - 2, 0, pos, 6);
			DataMatrixPlacementbit(array, numRows, numColumns, numRows - 1, 0, pos, 5);
			DataMatrixPlacementbit(array, numRows, numColumns, 0, numColumns - 2, pos, 4);
			DataMatrixPlacementbit(array, numRows, numColumns, 0, numColumns - 1, pos, 3);
			DataMatrixPlacementbit(array, numRows, numColumns, 1, numColumns - 1, pos, 2);
			DataMatrixPlacementbit(array, numRows, numColumns, 2, numColumns - 1, pos, 1);
			DataMatrixPlacementbit(array, numRows, numColumns, 3, numColumns - 1, pos, 0);
		}

		private void DataMatrixPlacementCornerD(int[] array, int numRows, int numColumns, int pos)
		{
			DataMatrixPlacementbit(array, numRows, numColumns, numRows - 1, 0, pos, 7);
			DataMatrixPlacementbit(array, numRows, numColumns, numRows - 1, numColumns - 1, pos, 6);
			DataMatrixPlacementbit(array, numRows, numColumns, 0, numColumns - 3, pos, 5);
			DataMatrixPlacementbit(array, numRows, numColumns, 0, numColumns - 2, pos, 4);
			DataMatrixPlacementbit(array, numRows, numColumns, 0, numColumns - 1, pos, 3);
			DataMatrixPlacementbit(array, numRows, numColumns, 1, numColumns - 3, pos, 2);
			DataMatrixPlacementbit(array, numRows, numColumns, 1, numColumns - 2, pos, 1);
			DataMatrixPlacementbit(array, numRows, numColumns, 1, numColumns - 1, pos, 0);
		}

		private int[] DataMatrixPlacement(int numRows, int numColumns)
		{
			int[] array = new int[numColumns * numRows];
			int num = 4;
			int num2 = 0;
			int num3 = 1;
			do
			{
				if (num == numRows && num2 == 0)
				{
					DataMatrixPlacementCornerA(array, numRows, numColumns, num3++);
				}
				if (num == numRows - 2 && num2 == 0 && numColumns % 4 != 0)
				{
					DataMatrixPlacementCornerB(array, numRows, numColumns, num3++);
				}
				if (num == numRows - 2 && num2 == 0 && numColumns % 8 == 4)
				{
					DataMatrixPlacementCornerC(array, numRows, numColumns, num3++);
				}
				if (num == numRows + 4 && num2 == 2 && numColumns % 8 == 0)
				{
					DataMatrixPlacementCornerD(array, numRows, numColumns, num3++);
				}
				do
				{
					if (num < numRows && num2 >= 0 && array[num * numColumns + num2] == 0)
					{
						DataMatrixPlacementBlock(array, numRows, numColumns, num, num2, num3++);
					}
					num -= 2;
					num2 += 2;
				}
				while (num >= 0 && num2 < numColumns);
				num++;
				num2 += 3;
				do
				{
					if (num >= 0 && num2 < numColumns && array[num * numColumns + num2] == 0)
					{
						DataMatrixPlacementBlock(array, numRows, numColumns, num, num2, num3++);
					}
					num += 2;
					num2 -= 2;
				}
				while (num < numRows && num2 >= 0);
				num += 3;
				num2++;
			}
			while (num < numRows || num2 < numColumns);
			if (array[numRows * numColumns - 1] == 0)
			{
				array[numRows * numColumns - 1] = (array[numRows * numColumns - numColumns - 2] = 1);
			}
			return array;
		}

		private void MakeEcc200Blocks(byte[] binary, int bytes, int datablock, int rsblock)
		{
			ReedSolomon reedSolomon = new ReedSolomon(rsblock);
			int num = (bytes + 2) / datablock;
			for (int i = 0; i < num; i++)
			{
				byte[] array = new byte[256];
				int len = 0;
				for (int j = i; j < bytes; j += num)
				{
					array[len++] = binary[j];
				}
				byte[] array2 = reedSolomon.Encode(len, array);
				len = rsblock - 1;
				for (int k = i; k < rsblock * num; k += num)
				{
					binary[bytes + k] = array2[len--];
				}
			}
		}

		private int DataMatrixEncode(byte[] output, int matrixLength, byte[] barcode, StiDataMatrixEncodingType encoding)
		{
			int outPos = 0;
			int barPos = 0;
			int[] array = StiBarCodeTypeService.UnpackTilde(barcode, processTilde);
			if (barPos < array.Length && array[barPos] == 256)
			{
				output[outPos++] = 232;
				barPos++;
			}
			if (barcode.Length != 0)
			{
				switch (encoding)
				{
				case StiDataMatrixEncodingType.C40:
				case StiDataMatrixEncodingType.Text:
				case StiDataMatrixEncodingType.X12:
					EncodeCTX(output, matrixLength, array, encoding, ref barPos, ref outPos);
					break;
				case StiDataMatrixEncodingType.Edifact:
					EncodeE(output, matrixLength, array, ref barPos, ref outPos);
					break;
				case StiDataMatrixEncodingType.Ascii:
					EncodeA(output, matrixLength, array, ref barPos, ref outPos);
					break;
				case StiDataMatrixEncodingType.Binary:
					EncodeB(output, matrixLength, array, ref barPos, ref outPos);
					break;
				}
			}
			int result = outPos;
			if (outPos < matrixLength && encoding != 0)
			{
				output[outPos++] = 254;
			}
			if (outPos < matrixLength)
			{
				output[outPos++] = 129;
			}
			while (outPos < matrixLength)
			{
				int num = 129 + (outPos + 1) * 149 % 253 + 1;
				if (num > 254)
				{
					num -= 254;
				}
				output[outPos++] = (byte)num;
			}
			if (outPos > matrixLength || barPos < array.Length)
			{
				return 0;
			}
			return result;
		}

		private void EncodeB(byte[] output, int matrixLength, int[] barcode, ref int barPos, ref int outPos)
		{
			output[outPos++] = 231;
			if (barcode.Length < 250)
			{
				output[outPos++] = (byte)barcode.Length;
			}
			else
			{
				output[outPos++] = (byte)(249 + barcode.Length / 250);
				output[outPos++] = (byte)(barcode.Length % 250);
			}
			do
			{
				output[outPos] = (byte)((barcode[barPos++] & 0xFF) + (outPos + 1) * 149 % 255 + 1);
				outPos++;
			}
			while (barPos < barcode.Length && outPos < matrixLength);
		}

		private void EncodeA(byte[] output, int matrixLength, int[] barcode, ref int barPos, ref int outPos)
		{
			while (barPos < barcode.Length && outPos < matrixLength)
			{
				if (barcode[barPos] > 255)
				{
					if (barcode[barPos] == 256)
					{
						output[outPos++] = 232;
					}
					barPos++;
				}
				else if (barcode.Length - barPos >= 2 && IsDigit(barcode[barPos]) && IsDigit(barcode[barPos + 1]))
				{
					output[outPos++] = (byte)((barcode[barPos] - 48) * 10 + barcode[barPos + 1] - 48 + 130);
					barPos += 2;
				}
				else if (barcode[barPos] > 127)
				{
					output[outPos++] = 235;
					output[outPos++] = (byte)(barcode[barPos++] - 127);
				}
				else
				{
					output[outPos++] = (byte)(barcode[barPos++] + 1);
				}
			}
		}

		private void EncodeE(byte[] output, int matrixLength, int[] barcode, ref int barPos, ref int outPos)
		{
			byte[] array = new byte[4];
			output[outPos++] = 240;
			do
			{
				array[0] = 0;
				array[1] = 0;
				array[2] = 0;
				array[3] = 0;
				int num = 0;
				while (barPos < barcode.Length && num < 4)
				{
					array[num++] = (byte)((uint)barcode[barPos++] & 0xFFu);
				}
				if (num < 4)
				{
					array[num++] = 31;
				}
				output[outPos] = (byte)((array[0] & 0x3F) << 2);
				output[outPos++] |= (byte)((array[1] & 0x30) >> 4);
				output[outPos] = (byte)((array[1] & 0xF) << 4);
				if (num == 2)
				{
					outPos++;
					continue;
				}
				output[outPos++] |= (byte)((array[2] & 0x3C) >> 2);
				output[outPos] = (byte)((array[2] & 3) << 6);
				output[outPos++] |= (byte)(array[3] & 0x3F);
			}
			while (barPos < barcode.Length && outPos < matrixLength);
		}

		private void EncodeCTX(byte[] output, int matrixLength, int[] barcode, StiDataMatrixEncodingType encoding, ref int barPos, ref int outPos)
		{
			int[] array = new int[6];
			int num = 0;
			string text = "!\"#$%&'()*+,-./:;<=>?@[\\]_Ā";
			string text2 = "";
			string text3 = "";
			if (encoding == StiDataMatrixEncodingType.C40)
			{
				output[outPos++] = 230;
				text3 = " 0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
				text2 = "`abcdefghijklmnopqrstuvwxyz{|}~\u007f";
			}
			if (encoding == StiDataMatrixEncodingType.Text)
			{
				output[outPos++] = 239;
				text3 = " 0123456789abcdefghijklmnopqrstuvwxyz";
				text2 = "`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~\u007f";
			}
			if (encoding == StiDataMatrixEncodingType.X12)
			{
				output[outPos++] = 238;
				text3 = " 0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ\r*>";
			}
			do
			{
				if (barPos + 1 == barcode.Length && num == 0)
				{
					output[outPos++] = 254;
					EncodeA(output, matrixLength, barcode, ref barPos, ref outPos);
					encoding = StiDataMatrixEncodingType.Ascii;
					break;
				}
				int num2 = barcode[barPos++];
				if (((uint)num2 & 0x80u) != 0)
				{
					if (encoding == StiDataMatrixEncodingType.X12)
					{
						throw new StiDataMatrixException($"Cannot encode char 0x{num2:X2} in X12");
					}
					num2 &= 0x7F;
					array[num++] = 1;
					array[num++] = 30;
				}
				int num3 = text3.IndexOf((char)num2);
				if (num3 >= 0)
				{
					array[num++] = (num3 + 3) % 40;
				}
				else
				{
					if (encoding == StiDataMatrixEncodingType.X12)
					{
						throw new StiDataMatrixException($"Cannot encode char 0x{num2:X2} in X12");
					}
					if (num2 < 32)
					{
						array[num++] = 0;
						array[num++] = num2;
					}
					else
					{
						num3 = text.IndexOf((char)num2);
						if (num3 >= 0)
						{
							array[num++] = 1;
							array[num++] = num3;
						}
						else
						{
							num3 = text2.IndexOf((char)num2);
							if (num3 < 0)
							{
								throw new StiDataMatrixException($"Cannot encode char 0x{num2:X2}");
							}
							array[num++] = 2;
							array[num++] = num3;
						}
					}
				}
				if (barPos == barcode.Length && num % 3 == 2)
				{
					array[num++] = 0;
				}
				while (num >= 3)
				{
					int num4 = array[0] * 1600 + array[1] * 40 + array[2] + 1;
					output[outPos++] = (byte)(num4 >> 8);
					output[outPos++] = (byte)((uint)num4 & 0xFFu);
					num -= 3;
					array[0] = array[3];
					array[1] = array[4];
					array[2] = array[5];
					array[3] = 0;
					array[4] = 0;
					array[5] = 0;
				}
			}
			while (barPos < barcode.Length && outPos < matrixLength);
		}

		private static bool IsDigit(int b)
		{
			if (b >= 48)
			{
				return b <= 57;
			}
			return false;
		}

		private static byte[] ConvertStringToBytes(string st)
		{
			char[] array = st.ToCharArray();
			byte[] array2 = new byte[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = (byte)array[i];
			}
			return array2;
		}

		private byte[] MakeGrid(byte[] barcode, ref int widthOriginal, ref int heightOriginal, StiDataMatrixEncodingType globalEncoding)
		{
			int num = widthOriginal;
			int num2 = heightOriginal;
			byte[] array = new byte[3200];
			byte[] array2 = null;
			int num3 = 0;
			if (num != 0)
			{
				for (num3 = 0; num3 < ecc200List.Length && (ecc200List[num3].Width != num || ecc200List[num3].Height != num2); num3++)
				{
				}
				if (num3 == ecc200List.Length)
				{
					throw new StiDataMatrixException($"Invalid size {num}x{num2}");
				}
			}
			else
			{
				int num4 = DataMatrixEncode(array, 1558, barcode, globalEncoding);
				for (num3 = 0; num3 < ecc200List.Length && num4 > ecc200List[num3].Bytes; num3++)
				{
				}
				if (num3 == ecc200List.Length || (num4 == 0 && barcode.Length != 0))
				{
					throw new StiDataMatrixException("Cannot find suitable size, barcode too long");
				}
				num = ecc200List[num3].Width;
				num2 = ecc200List[num3].Height;
			}
			if (DataMatrixEncode(array, ecc200List[num3].Bytes, barcode, globalEncoding) == 0 && barcode.Length != 0)
			{
				throw new StiDataMatrixException($"Barcode too long for {num}x{num2}");
			}
			MakeEcc200Blocks(array, ecc200List[num3].Bytes, ecc200List[num3].Datablock, ecc200List[num3].RSblock);
			int num5 = num - 2 * (num / ecc200List[num3].FW);
			int num6 = num2 - 2 * (num2 / ecc200List[num3].FH);
			int[] array3 = DataMatrixPlacement(num6, num5);
			array2 = new byte[num * num2];
			int num7 = 0;
			int num8 = 0;
			for (num8 = 0; num8 < num2; num8 += ecc200List[num3].FH)
			{
				for (num7 = 0; num7 < num; num7++)
				{
					array2[num8 * num + num7] = 1;
				}
				for (num7 = 0; num7 < num; num7 += 2)
				{
					array2[(num8 + ecc200List[num3].FH - 1) * num + num7] = 1;
				}
			}
			for (num7 = 0; num7 < num; num7 += ecc200List[num3].FW)
			{
				for (num8 = 0; num8 < num2; num8++)
				{
					array2[num8 * num + num7] = 1;
				}
				for (num8 = 0; num8 < num2; num8 += 2)
				{
					array2[num8 * num + num7 + ecc200List[num3].FW - 1] = 1;
				}
			}
			for (num8 = 0; num8 < num6; num8++)
			{
				for (num7 = 0; num7 < num5; num7++)
				{
					int num9 = array3[(num6 - num8 - 1) * num5 + num7];
					if (num9 == 1 || (num9 > 7 && (array[(num9 >> 3) - 1] & (1 << (num9 & 7))) != 0))
					{
						array2[(1 + num8 + 2 * (num8 / (ecc200List[num3].FH - 2))) * num + 1 + num7 + 2 * (num7 / (ecc200List[num3].FW - 2))] = 1;
					}
				}
			}
			widthOriginal = num;
			heightOriginal = num2;
			return array2;
		}

		public StiDataMatrix(string message, StiDataMatrixEncodingType globalEncoding, bool useRectangularSymbols, StiDataMatrixSize matrixSize, bool processTilde)
			: this(ConvertStringToBytes(message), globalEncoding, useRectangularSymbols, matrixSize, processTilde)
		{
		}

		public StiDataMatrix(byte[] data, StiDataMatrixEncodingType globalEncoding, bool useRectangularSymbols, StiDataMatrixSize matrixSize, bool processTilde)
		{
			this.processTilde = processTilde;
			gridWidth = 0;
			gridHeight = 0;
			byte[] array = null;
			if (matrixSize != StiDataMatrixSize.Automatic)
			{
				useRectangularSymbols = true;
			}
			if (useRectangularSymbols)
			{
				ecc200List = new ecc200ListItem[30]
				{
					new ecc200ListItem(10, 10, 10, 10, 3, 3, 5),
					new ecc200ListItem(12, 12, 12, 12, 5, 5, 7),
					new ecc200ListItem(8, 18, 8, 18, 5, 5, 7),
					new ecc200ListItem(14, 14, 14, 14, 8, 8, 10),
					new ecc200ListItem(8, 32, 8, 16, 10, 10, 11),
					new ecc200ListItem(16, 16, 16, 16, 12, 12, 12),
					new ecc200ListItem(12, 26, 12, 26, 16, 16, 14),
					new ecc200ListItem(18, 18, 18, 18, 18, 18, 14),
					new ecc200ListItem(20, 20, 20, 20, 22, 22, 18),
					new ecc200ListItem(12, 36, 12, 18, 22, 22, 18),
					new ecc200ListItem(22, 22, 22, 22, 30, 30, 20),
					new ecc200ListItem(16, 36, 16, 18, 32, 32, 24),
					new ecc200ListItem(24, 24, 24, 24, 36, 36, 24),
					new ecc200ListItem(26, 26, 26, 26, 44, 44, 28),
					new ecc200ListItem(16, 48, 16, 24, 49, 49, 28),
					new ecc200ListItem(32, 32, 16, 16, 62, 62, 36),
					new ecc200ListItem(36, 36, 18, 18, 86, 86, 42),
					new ecc200ListItem(40, 40, 20, 20, 114, 114, 48),
					new ecc200ListItem(44, 44, 22, 22, 144, 144, 56),
					new ecc200ListItem(48, 48, 24, 24, 174, 174, 68),
					new ecc200ListItem(52, 52, 26, 26, 204, 102, 42),
					new ecc200ListItem(64, 64, 16, 16, 280, 140, 56),
					new ecc200ListItem(72, 72, 18, 18, 368, 92, 36),
					new ecc200ListItem(80, 80, 20, 20, 456, 114, 48),
					new ecc200ListItem(88, 88, 22, 22, 576, 144, 56),
					new ecc200ListItem(96, 96, 24, 24, 696, 174, 68),
					new ecc200ListItem(104, 104, 26, 26, 816, 136, 56),
					new ecc200ListItem(120, 120, 20, 20, 1050, 175, 68),
					new ecc200ListItem(132, 132, 22, 22, 1304, 163, 62),
					new ecc200ListItem(144, 144, 24, 24, 1558, 156, 62)
				};
			}
			else
			{
				ecc200List = new ecc200ListItem[24]
				{
					new ecc200ListItem(10, 10, 10, 10, 3, 3, 5),
					new ecc200ListItem(12, 12, 12, 12, 5, 5, 7),
					new ecc200ListItem(14, 14, 14, 14, 8, 8, 10),
					new ecc200ListItem(16, 16, 16, 16, 12, 12, 12),
					new ecc200ListItem(18, 18, 18, 18, 18, 18, 14),
					new ecc200ListItem(20, 20, 20, 20, 22, 22, 18),
					new ecc200ListItem(22, 22, 22, 22, 30, 30, 20),
					new ecc200ListItem(24, 24, 24, 24, 36, 36, 24),
					new ecc200ListItem(26, 26, 26, 26, 44, 44, 28),
					new ecc200ListItem(32, 32, 16, 16, 62, 62, 36),
					new ecc200ListItem(36, 36, 18, 18, 86, 86, 42),
					new ecc200ListItem(40, 40, 20, 20, 114, 114, 48),
					new ecc200ListItem(44, 44, 22, 22, 144, 144, 56),
					new ecc200ListItem(48, 48, 24, 24, 174, 174, 68),
					new ecc200ListItem(52, 52, 26, 26, 204, 102, 42),
					new ecc200ListItem(64, 64, 16, 16, 280, 140, 56),
					new ecc200ListItem(72, 72, 18, 18, 368, 92, 36),
					new ecc200ListItem(80, 80, 20, 20, 456, 114, 48),
					new ecc200ListItem(88, 88, 22, 22, 576, 144, 56),
					new ecc200ListItem(96, 96, 24, 24, 696, 174, 68),
					new ecc200ListItem(104, 104, 26, 26, 816, 136, 56),
					new ecc200ListItem(120, 120, 20, 20, 1050, 175, 68),
					new ecc200ListItem(132, 132, 22, 22, 1304, 163, 62),
					new ecc200ListItem(144, 144, 24, 24, 1558, 156, 62)
				};
			}
			if (matrixSize != StiDataMatrixSize.Automatic)
			{
				ecc200ListItem ecc200ListItem = ecc200List[(int)matrixSize];
				gridWidth = ecc200ListItem.Width;
				gridHeight = ecc200ListItem.Height;
			}
			try
			{
				array = MakeGrid(data, ref gridWidth, ref gridHeight, globalEncoding);
			}
			catch (StiDataMatrixException ex)
			{
				errorMessage = ex.Message;
			}
			if (array == null)
			{
				return;
			}
			grid = new byte[gridWidth * gridHeight];
			for (int i = 0; i < gridHeight; i++)
			{
				int num = i * gridWidth;
				int num2 = (gridHeight - i - 1) * gridWidth;
				for (int j = 0; j < gridWidth; j++)
				{
					grid[num2 + j] = array[num + j];
				}
			}
		}
	}

	private float module = 40f;

	private float height = 1f;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiDataMatrixBarCodeType;

	public override string ServiceName => "DataMatrix";

	public override string DefaultCodeValue => "12345678901";

	[Description("Gets or sets width of the most fine element of the bar code.")]
	[DefaultValue(40f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public override float Module
	{
		get
		{
			return module;
		}
		set
		{
			module = value;
			if (value < 2f)
			{
				module = 2f;
			}
			if (value > 400f)
			{
				module = 400f;
			}
		}
	}

	[Description("Gets os sets height factor of the bar code.")]
	[Browsable(false)]
	[DefaultValue(1f)]
	public override float Height
	{
		get
		{
			return height;
		}
		set
		{
			height = value;
		}
	}

	[Description("Gets or sets type of encoding type.")]
	[DefaultValue(StiDataMatrixEncodingType.Ascii)]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public StiDataMatrixEncodingType EncodingType { get; set; }

	[Description("Gets or sets the flag that indicates whether the data message supports character '~' as the escape.")]
	[DefaultValue(false)]
	[StiOrder(110)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public bool ProcessTilde { get; set; }

	[DefaultValue(StiDataMatrixSize.Automatic)]
	[StiSerializable]
	[Description("Gets or sets matrix size.")]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public StiDataMatrixSize MatrixSize { get; set; } = StiDataMatrixSize.Automatic;


	[DefaultValue(false)]
	[StiSerializable]
	[Description("Gets or sets value which indicates will RectangularSymbols be used or not.")]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public bool UseRectangularSymbols { get; set; }

	internal override float LabelFontHeight => 8f;

	public override bool[] VisibleProperties => new bool[27]
	{
		false, false, false, false, false, false, false, false, false, true,
		false, false, true, true, false, false, false, false, false, false,
		false, true, false, false, false, false, false
	};

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyFloat(val, "Height", Height, 1f);
		JsonExtensions.AddPropertyFloat(val, "Module", Module, 40f);
		JsonExtensions.AddPropertyEnum(val, "EncodingType", (Enum)EncodingType, (Enum)StiDataMatrixEncodingType.Ascii);
		JsonExtensions.AddPropertyEnum(val, "MatrixSize", (Enum)MatrixSize, (Enum)StiDataMatrixSize.Automatic);
		JsonExtensions.AddPropertyBool(val, "UseRectangularSymbols", UseRectangularSymbols, false, false);
		JsonExtensions.AddPropertyBool(val, "ProcessTilde", ProcessTilde, false, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Module":
				module = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "Height":
				height = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "EncodingType":
				EncodingType = StiJsonReportObjectHelper.DeserializeEnum<StiDataMatrixEncodingType>(item);
				break;
			case "MatrixSize":
				MatrixSize = StiJsonReportObjectHelper.DeserializeEnum<StiDataMatrixSize>(item);
				break;
			case "UseRectangularSymbols":
				UseRectangularSymbols = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ProcessTilde":
				ProcessTilde = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[4]
		{
			propertiesHelper.EncodingType(),
			propertiesHelper.MatrixSize(),
			propertiesHelper.Module(),
			propertiesHelper.UseRectangularSymbols()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public override void Draw(object context, StiBarCode barCode, RectangleF rect, float zoom)
	{
		string code = GetCode(barCode);
		base.BarCodeData.Code = code;
		StiDataMatrix stiDataMatrix = new StiDataMatrix(code, EncodingType, UseRectangularSymbols, MatrixSize, ProcessTilde);
		base.BarCodeData.MatrixGrid = stiDataMatrix.Matrix;
		base.BarCodeData.MatrixWidth = stiDataMatrix.Width;
		base.BarCodeData.MatrixHeight = stiDataMatrix.Height;
		base.BarCodeData.MatrixRatioY = 1;
		if (stiDataMatrix.ErrorMessage == null)
		{
			Draw2DBarCode(context, rect, barCode, zoom);
		}
		else
		{
			DrawBarCodeError(context, rect, barCode, stiDataMatrix.ErrorMessage);
		}
	}

	public override StiBarCodeTypeService CreateNew()
	{
		return new StiDataMatrixBarCodeType();
	}

	public StiDataMatrixBarCodeType()
		: this(40f, StiDataMatrixEncodingType.Ascii, useRectangularSymbols: false, StiDataMatrixSize.Automatic, processTilde: false)
	{
	}

	public StiDataMatrixBarCodeType(float module, StiDataMatrixEncodingType encodingType, bool useRectangularSymbols, StiDataMatrixSize matrixSize)
		: this(module, encodingType, useRectangularSymbols, matrixSize, processTilde: false)
	{
	}

	public StiDataMatrixBarCodeType(float module, StiDataMatrixEncodingType encodingType, bool useRectangularSymbols, StiDataMatrixSize matrixSize, bool processTilde)
	{
		this.module = module;
		EncodingType = encodingType;
		UseRectangularSymbols = useRectangularSymbols;
		MatrixSize = matrixSize;
		ProcessTilde = processTilde;
	}
}
