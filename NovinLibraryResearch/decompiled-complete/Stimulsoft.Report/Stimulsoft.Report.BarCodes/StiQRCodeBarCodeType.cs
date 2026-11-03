using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.BarCodes.Design;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.BarCodes;

[TypeConverter(typeof(StiQRCodeBarCodeTypeConverter))]
public class StiQRCodeBarCodeType : StiBarCodeTypeService
{
	protected class StiQRCodeException : Exception
	{
		public StiQRCodeException()
		{
		}

		public StiQRCodeException(string message)
			: base(message)
		{
		}

		public StiQRCodeException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}

	private sealed class StiQRCode
	{
		public sealed class QREncoder
		{
			private static readonly int[] ALPHANUMERIC_TABLE = new int[96]
			{
				-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
				-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
				-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
				-1, -1, 36, -1, -1, -1, 37, 38, -1, -1,
				-1, -1, 39, 40, -1, 41, 42, 43, 0, 1,
				2, 3, 4, 5, 6, 7, 8, 9, 44, -1,
				-1, -1, -1, -1, -1, 10, 11, 12, 13, 14,
				15, 16, 17, 18, 19, 20, 21, 22, 23, 24,
				25, 26, 27, 28, 29, 30, 31, 32, 33, 34,
				35, -1, -1, -1, -1, -1
			};

			private const string defaultECIByteModeEncoding = "ISO-8859-1";

			private const string BYTE_MODE_UTF8 = "UTF-8";

			private const int QUESTION_MARK_CHAR = 63;

			private static int CalculateMaskPenalty(ByteMatrix matrix)
			{
				int num = 0;
				num += MaskUtil.ApplyMaskPenaltyRule1(matrix);
				num += MaskUtil.ApplyMaskPenaltyRule2(matrix);
				num += MaskUtil.ApplyMaskPenaltyRule3(matrix);
				return num + MaskUtil.ApplyMaskPenaltyRule4(matrix);
			}

			public static void Encode(string content, ErrorCorrectionLevel ecLevel, StiQRCode qrCode, StiQRCodeSize startVersion)
			{
				if (string.IsNullOrEmpty(content))
				{
					throw new StiQRCodeException("Found empty contents");
				}
				string encodingByNumber = CharacterSetECI.GetEncodingByNumber((int)StiOptions.Engine.BarcodeQRCodeDefaultByteModeEncoding, "ISO-8859-1");
				string text = encodingByNumber;
				Mode mode = ChooseMode(content, text);
				if (mode == Mode.BYTE && encodingByNumber != "UTF-8")
				{
					text = GetEncodingName(content, encodingByNumber);
				}
				bool flag = mode == Mode.BYTE && !"ISO-8859-1".Equals(text) && text != "UTF-8";
				BitVector bitVector = new BitVector();
				AppendBytes(content, mode, bitVector, text);
				int numInputBytes = bitVector.SizeInBytes();
				InitQRCode(numInputBytes, ecLevel, mode, qrCode, startVersion, flag);
				BitVector bitVector2 = new BitVector();
				if (flag)
				{
					CharacterSetECI characterSetECIByName = CharacterSetECI.GetCharacterSetECIByName(text);
					if (characterSetECIByName != null)
					{
						AppendECI(characterSetECIByName, bitVector2);
					}
				}
				AppendModeInfo(mode, bitVector2);
				int numLetters = (mode.Equals(Mode.BYTE) ? bitVector.SizeInBytes() : content.Length);
				AppendLengthInfo(numLetters, qrCode.GetVersion(), mode, bitVector2);
				bitVector2.AppendBitVector(bitVector);
				TerminateBits(qrCode.GetNumDataBytes(), bitVector2);
				BitVector bitVector3 = new BitVector();
				InterleaveWithECBytes(bitVector2, qrCode.GetNumTotalBytes(), qrCode.GetNumDataBytes(), qrCode.GetNumRSBlocks(), bitVector3);
				ByteMatrix matrix = new ByteMatrix(qrCode.GetMatrixWidth(), qrCode.GetMatrixWidth());
				if (true)
				{
					int maskPattern = ChooseMaskPattern2(bitVector3, qrCode.GetECLevel(), qrCode.GetVersion(), ref matrix);
					qrCode.SetMaskPattern(maskPattern);
				}
				else
				{
					int maskPattern2 = ChooseMaskPattern(bitVector3, qrCode.GetECLevel(), qrCode.GetVersion(), matrix);
					qrCode.SetMaskPattern(maskPattern2);
					MatrixUtil.BuildMatrix(bitVector3, qrCode.GetECLevel(), qrCode.GetVersion(), qrCode.GetMaskPattern(), matrix);
				}
				qrCode.SetMatrix(matrix);
				if (!qrCode.IsValid())
				{
					throw new StiQRCodeException("Invalid QR code: " + qrCode.ToString());
				}
			}

			private static string GetEncodingName(string content, string defaultByteModeEncoding)
			{
				if (TryEncodeByte(content, defaultByteModeEncoding))
				{
					return defaultByteModeEncoding;
				}
				foreach (KeyValuePair<string, CharacterSetECI> item in CharacterSetECI.NAME_TO_ECI)
				{
					if (item.Key != "UTF-8" && TryEncodeByte(content, item.Key))
					{
						return item.Key;
					}
				}
				return "UTF-8";
			}

			private static bool TryEncodeByte(string content, string encodingName)
			{
				Encoding encoding;
				try
				{
					encoding = Encoding.GetEncoding(encodingName);
				}
				catch
				{
					return false;
				}
				byte[] bytes = encoding.GetBytes(content);
				string @string = encoding.GetString(bytes);
				if (content != @string)
				{
					return false;
				}
				char[] array = new char[1];
				for (int i = 0; i < content.Length; i++)
				{
					array[0] = content[i];
					bytes = encoding.GetBytes(array);
					int num = bytes.Length;
					if (array[0] != '?' && num == 1 && bytes[0] == 63)
					{
						return false;
					}
					if (num > 1)
					{
						return false;
					}
				}
				return true;
			}

			private static int GetAlphanumericCode(int code)
			{
				if (code < ALPHANUMERIC_TABLE.Length)
				{
					return ALPHANUMERIC_TABLE[code];
				}
				return -1;
			}

			public static Mode ChooseMode(string content)
			{
				return ChooseMode(content, null);
			}

			public static Mode ChooseMode(string content, string encoding)
			{
				if ("Shift_JIS".Equals(encoding))
				{
					if (!IsOnlyDoubleByteKanji(content))
					{
						return Mode.BYTE;
					}
					return Mode.KANJI;
				}
				bool flag = false;
				bool flag2 = false;
				foreach (char c in content)
				{
					if (c >= '0' && c <= '9')
					{
						flag = true;
						continue;
					}
					if (GetAlphanumericCode(c) != -1)
					{
						flag2 = true;
						continue;
					}
					return Mode.BYTE;
				}
				if (flag2)
				{
					return Mode.ALPHANUMERIC;
				}
				if (flag)
				{
					return Mode.NUMERIC;
				}
				return Mode.BYTE;
			}

			private static bool IsOnlyDoubleByteKanji(string content)
			{
				byte[] bytes;
				try
				{
					bytes = Encoding.GetEncoding("Shift_JIS").GetBytes(content);
				}
				catch
				{
					return false;
				}
				int num = bytes.Length;
				if (num % 2 != 0)
				{
					return false;
				}
				for (int i = 0; i < num; i += 2)
				{
					int num2 = bytes[i] & 0xFF;
					if ((num2 < 129 || num2 > 159) && (num2 < 224 || num2 > 235))
					{
						return false;
					}
				}
				return true;
			}

			private static int ChooseMaskPattern(BitVector bits, ErrorCorrectionLevel ecLevel, int version, ByteMatrix matrix)
			{
				int num = int.MaxValue;
				int result = -1;
				for (int i = 0; i < 8; i++)
				{
					MatrixUtil.BuildMatrix(bits, ecLevel, version, i, matrix);
					int num2 = CalculateMaskPenalty(matrix);
					if (num2 < num)
					{
						num = num2;
						result = i;
					}
				}
				return result;
			}

			private static int ChooseMaskPattern2(BitVector bits, ErrorCorrectionLevel ecLevel, int version, ref ByteMatrix matrix)
			{
				List<ByteMatrix> list = new List<ByteMatrix>();
				list.Add(matrix);
				for (int i = 1; i < 8; i++)
				{
					list.Add(new ByteMatrix(matrix.GetWidth(), matrix.GetHeight()));
				}
				for (int j = 0; j < 8; j++)
				{
					matrix = list[j];
					MatrixUtil.ClearMatrix(matrix);
					MatrixUtil.EmbedBasicPatterns(version, matrix);
					MatrixUtil.EmbedTypeInfo(ecLevel, j, matrix);
					MatrixUtil.MaybeEmbedVersionInfo(version, matrix);
				}
				MatrixUtil.EmbedDataBits2(bits, list);
				int num = int.MaxValue;
				int num2 = -1;
				for (int k = 0; k < 8; k++)
				{
					int num3 = CalculateMaskPenalty(list[k]);
					if (num3 < num)
					{
						num = num3;
						num2 = k;
					}
				}
				matrix = list[num2];
				return num2;
			}

			private static void InitQRCode(int numInputBytes, ErrorCorrectionLevel ecLevel, Mode mode, StiQRCode qrCode, StiQRCodeSize startVersion, bool needAppendECI)
			{
				qrCode.SetECLevel(ecLevel);
				qrCode.SetMode(mode);
				int i = 1;
				if (startVersion != 0)
				{
					i = (int)startVersion;
				}
				for (; i <= 40; i++)
				{
					Version versionForNumber = Version.GetVersionForNumber(i);
					int totalCodewords = versionForNumber.GetTotalCodewords();
					Version.ECBlocks eCBlocksForLevel = versionForNumber.GetECBlocksForLevel(ecLevel);
					int totalECCodewords = eCBlocksForLevel.GetTotalECCodewords();
					int numBlocks = eCBlocksForLevel.GetNumBlocks();
					int num = totalCodewords - totalECCodewords;
					int num2 = 4 + (needAppendECI ? 12 : 0);
					try
					{
						num2 += mode.GetCharacterCountBits(versionForNumber);
					}
					catch
					{
						num2 = 32;
					}
					if (num * 8 >= numInputBytes * 8 + num2)
					{
						qrCode.SetVersion(i);
						qrCode.SetNumTotalBytes(totalCodewords);
						qrCode.SetNumDataBytes(num);
						qrCode.SetNumRSBlocks(numBlocks);
						qrCode.SetNumECBytes(totalECCodewords);
						qrCode.SetMatrixWidth(versionForNumber.GetDimensionForVersion());
						return;
					}
				}
				throw new StiQRCodeException("Cannot find proper rs block info (input data too big?)");
			}

			private static void TerminateBits(int numDataBytes, BitVector bits)
			{
				int num = numDataBytes << 3;
				if (bits.Size() > num)
				{
					throw new StiQRCodeException("data bits cannot fit in the QR Code " + bits.Size() + " > " + num);
				}
				for (int i = 0; i < 4; i++)
				{
					if (bits.Size() >= num)
					{
						break;
					}
					bits.AppendBit(0);
				}
				int num2 = bits.Size() % 8;
				if (num2 > 0)
				{
					int num3 = 8 - num2;
					for (int j = 0; j < num3; j++)
					{
						bits.AppendBit(0);
					}
				}
				if (bits.Size() % 8 != 0)
				{
					throw new StiQRCodeException("Number of bits is not a multiple of 8");
				}
				int num4 = numDataBytes - bits.SizeInBytes();
				for (int k = 0; k < num4; k++)
				{
					if (k % 2 == 0)
					{
						bits.AppendBits(236, 8);
					}
					else
					{
						bits.AppendBits(17, 8);
					}
				}
				if (bits.Size() != num)
				{
					throw new StiQRCodeException("Bits size does not equal capacity");
				}
			}

			private static void GetNumDataBytesAndNumECBytesForBlockID(int numTotalBytes, int numDataBytes, int numRSBlocks, int blockID, int[] numDataBytesInBlock, int[] numECBytesInBlock)
			{
				if (blockID >= numRSBlocks)
				{
					throw new StiQRCodeException("Block ID too large");
				}
				int num = numTotalBytes % numRSBlocks;
				int num2 = numRSBlocks - num;
				int num3 = numTotalBytes / numRSBlocks;
				int num4 = num3 + 1;
				int num5 = numDataBytes / numRSBlocks;
				int num6 = num5 + 1;
				int num7 = num3 - num5;
				int num8 = num4 - num6;
				if (num7 != num8)
				{
					throw new StiQRCodeException("EC bytes mismatch");
				}
				if (numRSBlocks != num2 + num)
				{
					throw new StiQRCodeException("RS blocks mismatch");
				}
				if (numTotalBytes != (num5 + num7) * num2 + (num6 + num8) * num)
				{
					throw new StiQRCodeException("Total bytes mismatch");
				}
				if (blockID < num2)
				{
					numDataBytesInBlock[0] = num5;
					numECBytesInBlock[0] = num7;
				}
				else
				{
					numDataBytesInBlock[0] = num6;
					numECBytesInBlock[0] = num8;
				}
			}

			private static void InterleaveWithECBytes(BitVector bits, int numTotalBytes, int numDataBytes, int numRSBlocks, BitVector result)
			{
				if (bits.SizeInBytes() != numDataBytes)
				{
					throw new StiQRCodeException("Number of bits and data bytes does not match");
				}
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				List<BlockPair> list = new List<BlockPair>(numRSBlocks);
				for (int i = 0; i < numRSBlocks; i++)
				{
					int[] array = new int[1];
					int[] array2 = new int[1];
					GetNumDataBytesAndNumECBytesForBlockID(numTotalBytes, numDataBytes, numRSBlocks, i, array, array2);
					ByteArray byteArray = new ByteArray();
					byteArray.Set(bits.GetArray(), num, array[0]);
					ByteArray byteArray2 = GenerateECBytes(byteArray, array2[0]);
					list.Add(new BlockPair(byteArray, byteArray2));
					num2 = Math.Max(num2, byteArray.Size());
					num3 = Math.Max(num3, byteArray2.Size());
					num += array[0];
				}
				if (numDataBytes != num)
				{
					throw new StiQRCodeException("Data bytes does not match offset");
				}
				for (int j = 0; j < num2; j++)
				{
					for (int k = 0; k < list.Count; k++)
					{
						ByteArray dataBytes = list[k].GetDataBytes();
						if (j < dataBytes.Size())
						{
							result.AppendBits(dataBytes.At(j), 8);
						}
					}
				}
				for (int l = 0; l < num3; l++)
				{
					for (int m = 0; m < list.Count; m++)
					{
						ByteArray errorCorrectionBytes = list[m].GetErrorCorrectionBytes();
						if (l < errorCorrectionBytes.Size())
						{
							result.AppendBits(errorCorrectionBytes.At(l), 8);
						}
					}
				}
				if (numTotalBytes != result.SizeInBytes())
				{
					throw new StiQRCodeException("Interleaving error: " + numTotalBytes + " and " + result.SizeInBytes() + " differ.");
				}
			}

			private static ByteArray GenerateECBytes(ByteArray dataBytes, int numEcBytesInBlock)
			{
				int num = dataBytes.Size();
				int[] array = new int[num + numEcBytesInBlock];
				for (int i = 0; i < num; i++)
				{
					array[i] = dataBytes.At(i);
				}
				new StiBarcodeUtils.ReedSolomonEncoder(StiBarcodeUtils.GaloisField.QRCode_256).Encode(array, numEcBytesInBlock);
				ByteArray byteArray = new ByteArray(numEcBytesInBlock);
				for (int j = 0; j < numEcBytesInBlock; j++)
				{
					byteArray.Set(j, array[num + j]);
				}
				return byteArray;
			}

			private static void AppendModeInfo(Mode mode, BitVector bits)
			{
				bits.AppendBits(mode.GetBits(), 4);
			}

			private static void AppendLengthInfo(int numLetters, int version, Mode mode, BitVector bits)
			{
				int characterCountBits = mode.GetCharacterCountBits(Version.GetVersionForNumber(version));
				if (numLetters > (1 << characterCountBits) - 1)
				{
					throw new StiQRCodeException(numLetters + "is bigger than" + ((1 << characterCountBits) - 1));
				}
				bits.AppendBits(numLetters, characterCountBits);
			}

			private static void AppendBytes(string content, Mode mode, BitVector bits, string encoding)
			{
				if (mode.Equals(Mode.NUMERIC))
				{
					AppendNumericBytes(content, bits);
					return;
				}
				if (mode.Equals(Mode.ALPHANUMERIC))
				{
					AppendAlphanumericBytes(content, bits);
					return;
				}
				if (mode.Equals(Mode.BYTE))
				{
					Append8BitBytes(content, bits, encoding);
					return;
				}
				if (mode.Equals(Mode.KANJI))
				{
					AppendKanjiBytes(content, bits);
					return;
				}
				throw new StiQRCodeException("Invalid mode: " + mode);
			}

			private static void AppendNumericBytes(string content, BitVector bits)
			{
				int length = content.Length;
				int num = 0;
				while (num < length)
				{
					int num2 = content[num] - 48;
					if (num + 2 < length)
					{
						int num3 = content[num + 1] - 48;
						int num4 = content[num + 2] - 48;
						bits.AppendBits(num2 * 100 + num3 * 10 + num4, 10);
						num += 3;
					}
					else if (num + 1 < length)
					{
						int num5 = content[num + 1] - 48;
						bits.AppendBits(num2 * 10 + num5, 7);
						num += 2;
					}
					else
					{
						bits.AppendBits(num2, 4);
						num++;
					}
				}
			}

			private static void AppendAlphanumericBytes(string content, BitVector bits)
			{
				int length = content.Length;
				int num = 0;
				while (num < length)
				{
					int alphanumericCode = GetAlphanumericCode(content[num]);
					if (alphanumericCode == -1)
					{
						throw new StiQRCodeException();
					}
					if (num + 1 < length)
					{
						int alphanumericCode2 = GetAlphanumericCode(content[num + 1]);
						if (alphanumericCode2 == -1)
						{
							throw new StiQRCodeException();
						}
						bits.AppendBits(alphanumericCode * 45 + alphanumericCode2, 11);
						num += 2;
					}
					else
					{
						bits.AppendBits(alphanumericCode, 6);
						num++;
					}
				}
			}

			private static void Append8BitBytes(string content, BitVector bits, string encoding)
			{
				byte[] bytes;
				try
				{
					bytes = Encoding.GetEncoding(encoding).GetBytes(content);
				}
				catch (Exception ex)
				{
					throw new StiQRCodeException(ex.Message);
				}
				if (encoding == "UTF-8" && StiOptions.Engine.BarcodeQRCodeAllowUnicodeBOM)
				{
					bits.AppendBits(239, 8);
					bits.AppendBits(187, 8);
					bits.AppendBits(191, 8);
				}
				for (int i = 0; i < bytes.Length; i++)
				{
					bits.AppendBits(bytes[i], 8);
				}
			}

			private static void AppendKanjiBytes(string content, BitVector bits)
			{
				byte[] bytes;
				try
				{
					bytes = Encoding.GetEncoding("Shift_JIS").GetBytes(content);
				}
				catch (Exception ex)
				{
					throw new StiQRCodeException(ex.Message);
				}
				int num = bytes.Length;
				for (int i = 0; i < num; i += 2)
				{
					int num2 = bytes[i] & 0xFF;
					int num3 = bytes[i + 1] & 0xFF;
					int num4 = (num2 << 8) | num3;
					int num5 = -1;
					if (num4 >= 33088 && num4 <= 40956)
					{
						num5 = num4 - 33088;
					}
					else if (num4 >= 57408 && num4 <= 60351)
					{
						num5 = num4 - 49472;
					}
					if (num5 == -1)
					{
						throw new StiQRCodeException("Invalid byte sequence");
					}
					int value = (num5 >> 8) * 192 + (num5 & 0xFF);
					bits.AppendBits(value, 13);
				}
			}

			private static void AppendECI(CharacterSetECI eci, BitVector bits)
			{
				bits.AppendBits(Mode.ECI.GetBits(), 4);
				bits.AppendBits(eci.GetValue(), 8);
			}
		}

		public sealed class ErrorCorrectionLevel
		{
			public static readonly ErrorCorrectionLevel L = new ErrorCorrectionLevel(0, 1, "L");

			public static readonly ErrorCorrectionLevel M = new ErrorCorrectionLevel(1, 0, "M");

			public static readonly ErrorCorrectionLevel Q = new ErrorCorrectionLevel(2, 3, "Q");

			public static readonly ErrorCorrectionLevel H = new ErrorCorrectionLevel(3, 2, "H");

			private static readonly ErrorCorrectionLevel[] FOR_BITS = new ErrorCorrectionLevel[4] { M, L, H, Q };

			private int ordinal;

			private int bits;

			private string name;

			private ErrorCorrectionLevel(int ordinal, int bits, string name)
			{
				this.ordinal = ordinal;
				this.bits = bits;
				this.name = name;
			}

			public int Ordinal()
			{
				return ordinal;
			}

			public int GetBits()
			{
				return bits;
			}

			public string GetName()
			{
				return name;
			}

			public override string ToString()
			{
				return name;
			}

			public static ErrorCorrectionLevel ForBits(int bits)
			{
				if (bits < 0 || bits >= FOR_BITS.Length)
				{
					throw new IndexOutOfRangeException();
				}
				return FOR_BITS[bits];
			}
		}

		public sealed class ByteMatrix
		{
			private sbyte[][] bytes;

			private int width;

			private int height;

			public ByteMatrix(int width, int height)
			{
				bytes = new sbyte[height][];
				for (int i = 0; i < height; i++)
				{
					bytes[i] = new sbyte[width];
				}
				this.width = width;
				this.height = height;
			}

			public int GetHeight()
			{
				return height;
			}

			public int GetWidth()
			{
				return width;
			}

			public sbyte Get(int x, int y)
			{
				return bytes[y][x];
			}

			public sbyte[][] GetArray()
			{
				return bytes;
			}

			public void Set(int x, int y, sbyte value)
			{
				bytes[y][x] = value;
			}

			public void Set(int x, int y, int value)
			{
				bytes[y][x] = (sbyte)value;
			}

			public void Clear(sbyte value)
			{
				for (int i = 0; i < height; i++)
				{
					for (int j = 0; j < width; j++)
					{
						bytes[i][j] = value;
					}
				}
			}
		}

		public sealed class ByteArray
		{
			private const int INITIAL_SIZE = 32;

			private byte[] bytes;

			private int size;

			public ByteArray()
			{
				bytes = null;
				size = 0;
			}

			public ByteArray(int size)
			{
				bytes = new byte[size];
				this.size = size;
			}

			public ByteArray(byte[] byteArray)
			{
				bytes = byteArray;
				size = bytes.Length;
			}

			public int At(int index)
			{
				return bytes[index] & 0xFF;
			}

			public void Set(int index, int value)
			{
				bytes[index] = (byte)value;
			}

			public int Size()
			{
				return size;
			}

			public bool IsEmpty()
			{
				return size == 0;
			}

			public void AppendByte(int value)
			{
				if (size == 0 || size >= bytes.Length)
				{
					int capacity = Math.Max(32, size << 1);
					Reserve(capacity);
				}
				bytes[size] = (byte)value;
				size++;
			}

			public void Reserve(int capacity)
			{
				if (bytes == null || bytes.Length < capacity)
				{
					byte[] destinationArray = new byte[capacity];
					if (bytes != null)
					{
						Array.Copy(bytes, 0, destinationArray, 0, bytes.Length);
					}
					bytes = destinationArray;
				}
			}

			public void Set(byte[] source, int offset, int count)
			{
				bytes = new byte[count];
				size = count;
				for (int i = 0; i < count; i++)
				{
					bytes[i] = source[offset + i];
				}
			}
		}

		public sealed class BlockPair
		{
			private ByteArray dataBytes;

			private ByteArray errorCorrectionBytes;

			internal BlockPair(ByteArray data, ByteArray errorCorrection)
			{
				dataBytes = data;
				errorCorrectionBytes = errorCorrection;
			}

			public ByteArray GetDataBytes()
			{
				return dataBytes;
			}

			public ByteArray GetErrorCorrectionBytes()
			{
				return errorCorrectionBytes;
			}
		}

		public sealed class BitVector
		{
			private int sizeInBits;

			private byte[] array;

			private const int DEFAULT_SIZE_IN_BYTES = 32;

			public BitVector()
			{
				sizeInBits = 0;
				array = new byte[32];
			}

			public int At(int index)
			{
				if (index < 0 || index >= sizeInBits)
				{
					throw new IndexOutOfRangeException("Bad index: " + index);
				}
				int num = array[index >> 3] & 0xFF;
				return (num >> 7 - (index & 7)) & 1;
			}

			public int Size()
			{
				return sizeInBits;
			}

			public int SizeInBytes()
			{
				return sizeInBits + 7 >> 3;
			}

			public void AppendBit(int bit)
			{
				if (bit != 0 && bit != 1)
				{
					throw new StiQRCodeException("Bad bit");
				}
				int num = sizeInBits & 7;
				if (num == 0)
				{
					AppendByte(0);
					sizeInBits -= 8;
				}
				array[sizeInBits >> 3] |= (byte)(bit << 7 - num);
				sizeInBits++;
			}

			public void AppendBits(int value, int numBits)
			{
				if (numBits < 0 || numBits > 32)
				{
					throw new StiQRCodeException("Num bits must be between 0 and 32");
				}
				int num = numBits;
				while (num > 0)
				{
					if ((sizeInBits & 7) == 0 && num >= 8)
					{
						int value2 = (value >> num - 8) & 0xFF;
						AppendByte(value2);
						num -= 8;
					}
					else
					{
						int bit = (value >> num - 1) & 1;
						AppendBit(bit);
						num--;
					}
				}
			}

			public void AppendBitVector(BitVector bits)
			{
				int num = bits.Size();
				for (int i = 0; i < num; i++)
				{
					AppendBit(bits.At(i));
				}
			}

			public void Xor(BitVector other)
			{
				if (sizeInBits != other.Size())
				{
					throw new StiQRCodeException("BitVector sizes don't match");
				}
				int num = sizeInBits + 7 >> 3;
				for (int i = 0; i < num; i++)
				{
					array[i] ^= other.array[i];
				}
			}

			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder(sizeInBits);
				for (int i = 0; i < sizeInBits; i++)
				{
					if (At(i) == 0)
					{
						stringBuilder.Append('0');
						continue;
					}
					if (At(i) == 1)
					{
						stringBuilder.Append('1');
						continue;
					}
					throw new StiQRCodeException("Byte isn't 0 or 1");
				}
				return stringBuilder.ToString();
			}

			public byte[] GetArray()
			{
				return array;
			}

			private void AppendByte(int value)
			{
				if (sizeInBits >> 3 == array.Length)
				{
					byte[] destinationArray = new byte[array.Length << 1];
					Array.Copy(array, 0, destinationArray, 0, array.Length);
					array = destinationArray;
				}
				array[sizeInBits >> 3] = (byte)value;
				sizeInBits += 8;
			}
		}

		public sealed class Version
		{
			public sealed class ECBlocks
			{
				private int ecCodewordsPerBlock;

				private ECB[] ecBlocks;

				public ECBlocks(int ecCodewordsPerBlock, ECB ecBlocks)
				{
					this.ecCodewordsPerBlock = ecCodewordsPerBlock;
					this.ecBlocks = new ECB[1] { ecBlocks };
				}

				public ECBlocks(int ecCodewordsPerBlock, ECB ecBlocks1, ECB ecBlocks2)
				{
					this.ecCodewordsPerBlock = ecCodewordsPerBlock;
					ecBlocks = new ECB[2] { ecBlocks1, ecBlocks2 };
				}

				public int GetECCodewordsPerBlock()
				{
					return ecCodewordsPerBlock;
				}

				public int GetNumBlocks()
				{
					int num = 0;
					for (int i = 0; i < ecBlocks.Length; i++)
					{
						num += ecBlocks[i].GetCount();
					}
					return num;
				}

				public int GetTotalECCodewords()
				{
					return ecCodewordsPerBlock * GetNumBlocks();
				}

				public ECB[] GetECBlocks()
				{
					return ecBlocks;
				}
			}

			public sealed class ECB
			{
				private int count;

				private int dataCodewords;

				public ECB(int count, int dataCodewords)
				{
					this.count = count;
					this.dataCodewords = dataCodewords;
				}

				public int GetCount()
				{
					return count;
				}

				public int GetDataCodewords()
				{
					return dataCodewords;
				}
			}

			private static readonly int[] VERSION_DECODE_INFO = new int[34]
			{
				31892, 34236, 39577, 42195, 48118, 51042, 55367, 58893, 63784, 68472,
				70749, 76311, 79154, 84390, 87683, 92361, 96236, 102084, 102881, 110507,
				110734, 117786, 119615, 126325, 127568, 133589, 136944, 141498, 145311, 150283,
				152622, 158308, 161089, 167017
			};

			private static readonly Version[] VERSIONS = BuildVersions();

			private int versionNumber;

			private int[] alignmentPatternCenters;

			private ECBlocks[] ecBlocks;

			private int totalCodewords;

			private Version(int versionNumber, int[] alignmentPatternCenters, ECBlocks ecBlocks1, ECBlocks ecBlocks2, ECBlocks ecBlocks3, ECBlocks ecBlocks4)
			{
				this.versionNumber = versionNumber;
				this.alignmentPatternCenters = alignmentPatternCenters;
				ecBlocks = new ECBlocks[4] { ecBlocks1, ecBlocks2, ecBlocks3, ecBlocks4 };
				int num = 0;
				int eCCodewordsPerBlock = ecBlocks1.GetECCodewordsPerBlock();
				ECB[] eCBlocks = ecBlocks1.GetECBlocks();
				foreach (ECB eCB in eCBlocks)
				{
					num += eCB.GetCount() * (eCB.GetDataCodewords() + eCCodewordsPerBlock);
				}
				totalCodewords = num;
			}

			public int GetVersionNumber()
			{
				return versionNumber;
			}

			public int[] GetAlignmentPatternCenters()
			{
				return alignmentPatternCenters;
			}

			public int GetTotalCodewords()
			{
				return totalCodewords;
			}

			public int GetDimensionForVersion()
			{
				return 17 + 4 * versionNumber;
			}

			public ECBlocks GetECBlocksForLevel(ErrorCorrectionLevel ecLevel)
			{
				return ecBlocks[ecLevel.Ordinal()];
			}

			public static Version GetProvisionalVersionForDimension(int dimension)
			{
				if (dimension % 4 != 1)
				{
					throw new StiQRCodeException();
				}
				try
				{
					return GetVersionForNumber(dimension - 17 >> 2);
				}
				catch (ArgumentException ex)
				{
					throw ex;
				}
			}

			public static Version GetVersionForNumber(int versionNumber)
			{
				if (versionNumber < 1 || versionNumber > 40)
				{
					throw new StiQRCodeException();
				}
				return VERSIONS[versionNumber - 1];
			}

			private static Version DecodeVersionInformation(int versionBits)
			{
				int num = int.MaxValue;
				int num2 = 0;
				for (int i = 0; i < VERSION_DECODE_INFO.Length; i++)
				{
					int num3 = VERSION_DECODE_INFO[i];
					if (num3 == versionBits)
					{
						return GetVersionForNumber(i + 7);
					}
					int num4 = FormatInformation.NumBitsDiffering(versionBits, num3);
					if (num4 < num)
					{
						num2 = i + 7;
						num = num4;
					}
				}
				if (num <= 3)
				{
					return GetVersionForNumber(num2);
				}
				return null;
			}

			public override string ToString()
			{
				return versionNumber.ToString();
			}

			private static Version[] BuildVersions()
			{
				return new Version[40]
				{
					new Version(1, new int[0], new ECBlocks(7, new ECB(1, 19)), new ECBlocks(10, new ECB(1, 16)), new ECBlocks(13, new ECB(1, 13)), new ECBlocks(17, new ECB(1, 9))),
					new Version(2, new int[2] { 6, 18 }, new ECBlocks(10, new ECB(1, 34)), new ECBlocks(16, new ECB(1, 28)), new ECBlocks(22, new ECB(1, 22)), new ECBlocks(28, new ECB(1, 16))),
					new Version(3, new int[2] { 6, 22 }, new ECBlocks(15, new ECB(1, 55)), new ECBlocks(26, new ECB(1, 44)), new ECBlocks(18, new ECB(2, 17)), new ECBlocks(22, new ECB(2, 13))),
					new Version(4, new int[2] { 6, 26 }, new ECBlocks(20, new ECB(1, 80)), new ECBlocks(18, new ECB(2, 32)), new ECBlocks(26, new ECB(2, 24)), new ECBlocks(16, new ECB(4, 9))),
					new Version(5, new int[2] { 6, 30 }, new ECBlocks(26, new ECB(1, 108)), new ECBlocks(24, new ECB(2, 43)), new ECBlocks(18, new ECB(2, 15), new ECB(2, 16)), new ECBlocks(22, new ECB(2, 11), new ECB(2, 12))),
					new Version(6, new int[2] { 6, 34 }, new ECBlocks(18, new ECB(2, 68)), new ECBlocks(16, new ECB(4, 27)), new ECBlocks(24, new ECB(4, 19)), new ECBlocks(28, new ECB(4, 15))),
					new Version(7, new int[3] { 6, 22, 38 }, new ECBlocks(20, new ECB(2, 78)), new ECBlocks(18, new ECB(4, 31)), new ECBlocks(18, new ECB(2, 14), new ECB(4, 15)), new ECBlocks(26, new ECB(4, 13), new ECB(1, 14))),
					new Version(8, new int[3] { 6, 24, 42 }, new ECBlocks(24, new ECB(2, 97)), new ECBlocks(22, new ECB(2, 38), new ECB(2, 39)), new ECBlocks(22, new ECB(4, 18), new ECB(2, 19)), new ECBlocks(26, new ECB(4, 14), new ECB(2, 15))),
					new Version(9, new int[3] { 6, 26, 46 }, new ECBlocks(30, new ECB(2, 116)), new ECBlocks(22, new ECB(3, 36), new ECB(2, 37)), new ECBlocks(20, new ECB(4, 16), new ECB(4, 17)), new ECBlocks(24, new ECB(4, 12), new ECB(4, 13))),
					new Version(10, new int[3] { 6, 28, 50 }, new ECBlocks(18, new ECB(2, 68), new ECB(2, 69)), new ECBlocks(26, new ECB(4, 43), new ECB(1, 44)), new ECBlocks(24, new ECB(6, 19), new ECB(2, 20)), new ECBlocks(28, new ECB(6, 15), new ECB(2, 16))),
					new Version(11, new int[3] { 6, 30, 54 }, new ECBlocks(20, new ECB(4, 81)), new ECBlocks(30, new ECB(1, 50), new ECB(4, 51)), new ECBlocks(28, new ECB(4, 22), new ECB(4, 23)), new ECBlocks(24, new ECB(3, 12), new ECB(8, 13))),
					new Version(12, new int[3] { 6, 32, 58 }, new ECBlocks(24, new ECB(2, 92), new ECB(2, 93)), new ECBlocks(22, new ECB(6, 36), new ECB(2, 37)), new ECBlocks(26, new ECB(4, 20), new ECB(6, 21)), new ECBlocks(28, new ECB(7, 14), new ECB(4, 15))),
					new Version(13, new int[3] { 6, 34, 62 }, new ECBlocks(26, new ECB(4, 107)), new ECBlocks(22, new ECB(8, 37), new ECB(1, 38)), new ECBlocks(24, new ECB(8, 20), new ECB(4, 21)), new ECBlocks(22, new ECB(12, 11), new ECB(4, 12))),
					new Version(14, new int[4] { 6, 26, 46, 66 }, new ECBlocks(30, new ECB(3, 115), new ECB(1, 116)), new ECBlocks(24, new ECB(4, 40), new ECB(5, 41)), new ECBlocks(20, new ECB(11, 16), new ECB(5, 17)), new ECBlocks(24, new ECB(11, 12), new ECB(5, 13))),
					new Version(15, new int[4] { 6, 26, 48, 70 }, new ECBlocks(22, new ECB(5, 87), new ECB(1, 88)), new ECBlocks(24, new ECB(5, 41), new ECB(5, 42)), new ECBlocks(30, new ECB(5, 24), new ECB(7, 25)), new ECBlocks(24, new ECB(11, 12), new ECB(7, 13))),
					new Version(16, new int[4] { 6, 26, 50, 74 }, new ECBlocks(24, new ECB(5, 98), new ECB(1, 99)), new ECBlocks(28, new ECB(7, 45), new ECB(3, 46)), new ECBlocks(24, new ECB(15, 19), new ECB(2, 20)), new ECBlocks(30, new ECB(3, 15), new ECB(13, 16))),
					new Version(17, new int[4] { 6, 30, 54, 78 }, new ECBlocks(28, new ECB(1, 107), new ECB(5, 108)), new ECBlocks(28, new ECB(10, 46), new ECB(1, 47)), new ECBlocks(28, new ECB(1, 22), new ECB(15, 23)), new ECBlocks(28, new ECB(2, 14), new ECB(17, 15))),
					new Version(18, new int[4] { 6, 30, 56, 82 }, new ECBlocks(30, new ECB(5, 120), new ECB(1, 121)), new ECBlocks(26, new ECB(9, 43), new ECB(4, 44)), new ECBlocks(28, new ECB(17, 22), new ECB(1, 23)), new ECBlocks(28, new ECB(2, 14), new ECB(19, 15))),
					new Version(19, new int[4] { 6, 30, 58, 86 }, new ECBlocks(28, new ECB(3, 113), new ECB(4, 114)), new ECBlocks(26, new ECB(3, 44), new ECB(11, 45)), new ECBlocks(26, new ECB(17, 21), new ECB(4, 22)), new ECBlocks(26, new ECB(9, 13), new ECB(16, 14))),
					new Version(20, new int[4] { 6, 34, 62, 90 }, new ECBlocks(28, new ECB(3, 107), new ECB(5, 108)), new ECBlocks(26, new ECB(3, 41), new ECB(13, 42)), new ECBlocks(30, new ECB(15, 24), new ECB(5, 25)), new ECBlocks(28, new ECB(15, 15), new ECB(10, 16))),
					new Version(21, new int[5] { 6, 28, 50, 72, 94 }, new ECBlocks(28, new ECB(4, 116), new ECB(4, 117)), new ECBlocks(26, new ECB(17, 42)), new ECBlocks(28, new ECB(17, 22), new ECB(6, 23)), new ECBlocks(30, new ECB(19, 16), new ECB(6, 17))),
					new Version(22, new int[5] { 6, 26, 50, 74, 98 }, new ECBlocks(28, new ECB(2, 111), new ECB(7, 112)), new ECBlocks(28, new ECB(17, 46)), new ECBlocks(30, new ECB(7, 24), new ECB(16, 25)), new ECBlocks(24, new ECB(34, 13))),
					new Version(23, new int[5] { 6, 30, 54, 74, 102 }, new ECBlocks(30, new ECB(4, 121), new ECB(5, 122)), new ECBlocks(28, new ECB(4, 47), new ECB(14, 48)), new ECBlocks(30, new ECB(11, 24), new ECB(14, 25)), new ECBlocks(30, new ECB(16, 15), new ECB(14, 16))),
					new Version(24, new int[5] { 6, 28, 54, 80, 106 }, new ECBlocks(30, new ECB(6, 117), new ECB(4, 118)), new ECBlocks(28, new ECB(6, 45), new ECB(14, 46)), new ECBlocks(30, new ECB(11, 24), new ECB(16, 25)), new ECBlocks(30, new ECB(30, 16), new ECB(2, 17))),
					new Version(25, new int[5] { 6, 32, 58, 84, 110 }, new ECBlocks(26, new ECB(8, 106), new ECB(4, 107)), new ECBlocks(28, new ECB(8, 47), new ECB(13, 48)), new ECBlocks(30, new ECB(7, 24), new ECB(22, 25)), new ECBlocks(30, new ECB(22, 15), new ECB(13, 16))),
					new Version(26, new int[5] { 6, 30, 58, 86, 114 }, new ECBlocks(28, new ECB(10, 114), new ECB(2, 115)), new ECBlocks(28, new ECB(19, 46), new ECB(4, 47)), new ECBlocks(28, new ECB(28, 22), new ECB(6, 23)), new ECBlocks(30, new ECB(33, 16), new ECB(4, 17))),
					new Version(27, new int[5] { 6, 34, 62, 90, 118 }, new ECBlocks(30, new ECB(8, 122), new ECB(4, 123)), new ECBlocks(28, new ECB(22, 45), new ECB(3, 46)), new ECBlocks(30, new ECB(8, 23), new ECB(26, 24)), new ECBlocks(30, new ECB(12, 15), new ECB(28, 16))),
					new Version(28, new int[6] { 6, 26, 50, 74, 98, 122 }, new ECBlocks(30, new ECB(3, 117), new ECB(10, 118)), new ECBlocks(28, new ECB(3, 45), new ECB(23, 46)), new ECBlocks(30, new ECB(4, 24), new ECB(31, 25)), new ECBlocks(30, new ECB(11, 15), new ECB(31, 16))),
					new Version(29, new int[6] { 6, 30, 54, 78, 102, 126 }, new ECBlocks(30, new ECB(7, 116), new ECB(7, 117)), new ECBlocks(28, new ECB(21, 45), new ECB(7, 46)), new ECBlocks(30, new ECB(1, 23), new ECB(37, 24)), new ECBlocks(30, new ECB(19, 15), new ECB(26, 16))),
					new Version(30, new int[6] { 6, 26, 52, 78, 104, 130 }, new ECBlocks(30, new ECB(5, 115), new ECB(10, 116)), new ECBlocks(28, new ECB(19, 47), new ECB(10, 48)), new ECBlocks(30, new ECB(15, 24), new ECB(25, 25)), new ECBlocks(30, new ECB(23, 15), new ECB(25, 16))),
					new Version(31, new int[6] { 6, 30, 56, 82, 108, 134 }, new ECBlocks(30, new ECB(13, 115), new ECB(3, 116)), new ECBlocks(28, new ECB(2, 46), new ECB(29, 47)), new ECBlocks(30, new ECB(42, 24), new ECB(1, 25)), new ECBlocks(30, new ECB(23, 15), new ECB(28, 16))),
					new Version(32, new int[6] { 6, 34, 60, 86, 112, 138 }, new ECBlocks(30, new ECB(17, 115)), new ECBlocks(28, new ECB(10, 46), new ECB(23, 47)), new ECBlocks(30, new ECB(10, 24), new ECB(35, 25)), new ECBlocks(30, new ECB(19, 15), new ECB(35, 16))),
					new Version(33, new int[6] { 6, 30, 58, 86, 114, 142 }, new ECBlocks(30, new ECB(17, 115), new ECB(1, 116)), new ECBlocks(28, new ECB(14, 46), new ECB(21, 47)), new ECBlocks(30, new ECB(29, 24), new ECB(19, 25)), new ECBlocks(30, new ECB(11, 15), new ECB(46, 16))),
					new Version(34, new int[6] { 6, 34, 62, 90, 118, 146 }, new ECBlocks(30, new ECB(13, 115), new ECB(6, 116)), new ECBlocks(28, new ECB(14, 46), new ECB(23, 47)), new ECBlocks(30, new ECB(44, 24), new ECB(7, 25)), new ECBlocks(30, new ECB(59, 16), new ECB(1, 17))),
					new Version(35, new int[7] { 6, 30, 54, 78, 102, 126, 150 }, new ECBlocks(30, new ECB(12, 121), new ECB(7, 122)), new ECBlocks(28, new ECB(12, 47), new ECB(26, 48)), new ECBlocks(30, new ECB(39, 24), new ECB(14, 25)), new ECBlocks(30, new ECB(22, 15), new ECB(41, 16))),
					new Version(36, new int[7] { 6, 24, 50, 76, 102, 128, 154 }, new ECBlocks(30, new ECB(6, 121), new ECB(14, 122)), new ECBlocks(28, new ECB(6, 47), new ECB(34, 48)), new ECBlocks(30, new ECB(46, 24), new ECB(10, 25)), new ECBlocks(30, new ECB(2, 15), new ECB(64, 16))),
					new Version(37, new int[7] { 6, 28, 54, 80, 106, 132, 158 }, new ECBlocks(30, new ECB(17, 122), new ECB(4, 123)), new ECBlocks(28, new ECB(29, 46), new ECB(14, 47)), new ECBlocks(30, new ECB(49, 24), new ECB(10, 25)), new ECBlocks(30, new ECB(24, 15), new ECB(46, 16))),
					new Version(38, new int[7] { 6, 32, 58, 84, 110, 136, 162 }, new ECBlocks(30, new ECB(4, 122), new ECB(18, 123)), new ECBlocks(28, new ECB(13, 46), new ECB(32, 47)), new ECBlocks(30, new ECB(48, 24), new ECB(14, 25)), new ECBlocks(30, new ECB(42, 15), new ECB(32, 16))),
					new Version(39, new int[7] { 6, 26, 54, 82, 110, 138, 166 }, new ECBlocks(30, new ECB(20, 117), new ECB(4, 118)), new ECBlocks(28, new ECB(40, 47), new ECB(7, 48)), new ECBlocks(30, new ECB(43, 24), new ECB(22, 25)), new ECBlocks(30, new ECB(10, 15), new ECB(67, 16))),
					new Version(40, new int[7] { 6, 30, 58, 86, 114, 142, 170 }, new ECBlocks(30, new ECB(19, 118), new ECB(6, 119)), new ECBlocks(28, new ECB(18, 47), new ECB(31, 48)), new ECBlocks(30, new ECB(34, 24), new ECB(34, 25)), new ECBlocks(30, new ECB(20, 15), new ECB(61, 16)))
				};
			}
		}

		public sealed class StiQRCodeException : Exception
		{
			public StiQRCodeException()
			{
			}

			public StiQRCodeException(string message)
				: base(message)
			{
			}
		}

		public class CharacterSetECI
		{
			private static object lockNAME_TO_ECI = new object();

			private static Dictionary<string, CharacterSetECI> name_to_eci = null;

			private string encodingName;

			private int value;

			public static Dictionary<string, CharacterSetECI> NAME_TO_ECI
			{
				get
				{
					if (name_to_eci == null)
					{
						lock (lockNAME_TO_ECI)
						{
							Initialize();
						}
					}
					return name_to_eci;
				}
				set
				{
					name_to_eci = value;
				}
			}

			private static void Initialize()
			{
				Dictionary<string, CharacterSetECI> n = new Dictionary<string, CharacterSetECI>();
				AddCharacterSet(2, "Cp437", n);
				AddCharacterSet(3, "ISO-8859-1", n);
				AddCharacterSet(4, "ISO-8859-2", n);
				AddCharacterSet(5, "ISO-8859-3", n);
				AddCharacterSet(6, "ISO-8859-4", n);
				AddCharacterSet(7, "ISO-8859-5", n);
				AddCharacterSet(8, "ISO-8859-6", n);
				AddCharacterSet(9, "ISO-8859-7", n);
				AddCharacterSet(10, "ISO-8859-8", n);
				AddCharacterSet(11, "ISO-8859-9", n);
				AddCharacterSet(13, "ISO-8859-11", n);
				AddCharacterSet(15, "ISO-8859-13", n);
				AddCharacterSet(17, "ISO-8859-15", n);
				AddCharacterSet(20, "Shift_JIS", n);
				AddCharacterSet(21, "Windows-1250", n);
				AddCharacterSet(22, "Windows-1251", n);
				AddCharacterSet(23, "Windows-1252", n);
				AddCharacterSet(24, "Windows-1256", n);
				AddCharacterSet(26, "UTF-8", n);
				name_to_eci = n;
			}

			private CharacterSetECI(int value, string encodingName)
			{
				this.value = value;
				this.encodingName = encodingName;
			}

			public string GetEncodingName()
			{
				return encodingName;
			}

			public int GetValue()
			{
				return value;
			}

			private static void AddCharacterSet(int value, string encodingName, Dictionary<string, CharacterSetECI> n)
			{
				CharacterSetECI characterSetECI = new CharacterSetECI(value, encodingName);
				n[encodingName] = characterSetECI;
			}

			public static CharacterSetECI GetCharacterSetECIByName(string name)
			{
				NAME_TO_ECI.TryGetValue(name, out var result);
				return result;
			}

			public static string GetEncodingByNumber(int number, string defaultEncoding)
			{
				foreach (KeyValuePair<string, CharacterSetECI> item in NAME_TO_ECI)
				{
					if (item.Value.value == number)
					{
						return item.Value.encodingName;
					}
				}
				return defaultEncoding;
			}
		}

		public sealed class FormatInformation
		{
			private const int FORMAT_INFO_MASK_QR = 21522;

			private static readonly int[][] FORMAT_INFO_DECODE_LOOKUP = new int[32][]
			{
				new int[2] { 21522, 0 },
				new int[2] { 20773, 1 },
				new int[2] { 24188, 2 },
				new int[2] { 23371, 3 },
				new int[2] { 17913, 4 },
				new int[2] { 16590, 5 },
				new int[2] { 20375, 6 },
				new int[2] { 19104, 7 },
				new int[2] { 30660, 8 },
				new int[2] { 29427, 9 },
				new int[2] { 32170, 10 },
				new int[2] { 30877, 11 },
				new int[2] { 26159, 12 },
				new int[2] { 25368, 13 },
				new int[2] { 27713, 14 },
				new int[2] { 26998, 15 },
				new int[2] { 5769, 16 },
				new int[2] { 5054, 17 },
				new int[2] { 7399, 18 },
				new int[2] { 6608, 19 },
				new int[2] { 1890, 20 },
				new int[2] { 597, 21 },
				new int[2] { 3340, 22 },
				new int[2] { 2107, 23 },
				new int[2] { 13663, 24 },
				new int[2] { 12392, 25 },
				new int[2] { 16177, 26 },
				new int[2] { 14854, 27 },
				new int[2] { 9396, 28 },
				new int[2] { 8579, 29 },
				new int[2] { 11994, 30 },
				new int[2] { 11245, 31 }
			};

			private static readonly int[] BITS_SET_IN_HALF_BYTE = new int[16]
			{
				0, 1, 1, 2, 1, 2, 2, 3, 1, 2,
				2, 3, 2, 3, 3, 4
			};

			private ErrorCorrectionLevel errorCorrectionLevel;

			private byte dataMask;

			private FormatInformation(int formatInfo)
			{
				errorCorrectionLevel = ErrorCorrectionLevel.ForBits((formatInfo >> 3) & 3);
				dataMask = (byte)((uint)formatInfo & 7u);
			}

			public static int NumBitsDiffering(int a, int b)
			{
				a ^= b;
				return BITS_SET_IN_HALF_BYTE[a & 0xF] + BITS_SET_IN_HALF_BYTE[(a >> 4) & 0xF] + BITS_SET_IN_HALF_BYTE[(a >> 8) & 0xF] + BITS_SET_IN_HALF_BYTE[(a >> 12) & 0xF] + BITS_SET_IN_HALF_BYTE[(a >> 16) & 0xF] + BITS_SET_IN_HALF_BYTE[(a >> 20) & 0xF] + BITS_SET_IN_HALF_BYTE[(a >> 24) & 0xF] + BITS_SET_IN_HALF_BYTE[(a >> 28) & 0xF];
			}

			public static FormatInformation DecodeFormatInformation(int maskedFormatInfo1, int maskedFormatInfo2)
			{
				FormatInformation formatInformation = DoDecodeFormatInformation(maskedFormatInfo1, maskedFormatInfo2);
				if (formatInformation != null)
				{
					return formatInformation;
				}
				return DoDecodeFormatInformation(maskedFormatInfo1 ^ 0x5412, maskedFormatInfo2 ^ 0x5412);
			}

			private static FormatInformation DoDecodeFormatInformation(int maskedFormatInfo1, int maskedFormatInfo2)
			{
				int num = int.MaxValue;
				int formatInfo = 0;
				for (int i = 0; i < FORMAT_INFO_DECODE_LOOKUP.GetLength(0); i++)
				{
					int[] array = FORMAT_INFO_DECODE_LOOKUP[i];
					int num2 = array[0];
					if (num2 == maskedFormatInfo1 || num2 == maskedFormatInfo2)
					{
						return new FormatInformation(array[1]);
					}
					int num3 = NumBitsDiffering(maskedFormatInfo1, num2);
					if (num3 < num)
					{
						formatInfo = array[1];
						num = num3;
					}
					if (maskedFormatInfo1 != maskedFormatInfo2)
					{
						num3 = NumBitsDiffering(maskedFormatInfo2, num2);
						if (num3 < num)
						{
							formatInfo = array[1];
							num = num3;
						}
					}
				}
				if (num <= 3)
				{
					return new FormatInformation(formatInfo);
				}
				return null;
			}

			public ErrorCorrectionLevel GetErrorCorrectionLevel()
			{
				return errorCorrectionLevel;
			}

			public byte GetDataMask()
			{
				return dataMask;
			}

			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			public override bool Equals(object o)
			{
				if (!(o is FormatInformation))
				{
					return false;
				}
				FormatInformation formatInformation = (FormatInformation)o;
				if (errorCorrectionLevel == formatInformation.errorCorrectionLevel)
				{
					return dataMask == formatInformation.dataMask;
				}
				return false;
			}
		}

		public sealed class Mode
		{
			public static readonly Mode TERMINATOR = new Mode(new int[3], 0, "TERMINATOR");

			public static readonly Mode NUMERIC = new Mode(new int[3] { 10, 12, 14 }, 1, "NUMERIC");

			public static readonly Mode ALPHANUMERIC = new Mode(new int[3] { 9, 11, 13 }, 2, "ALPHANUMERIC");

			public static readonly Mode STRUCTURED_APPEND = new Mode(new int[3], 3, "STRUCTURED_APPEND");

			public static readonly Mode BYTE = new Mode(new int[3] { 8, 16, 16 }, 4, "BYTE");

			public static readonly Mode ECI = new Mode(null, 7, "ECI");

			public static readonly Mode KANJI = new Mode(new int[3] { 8, 10, 12 }, 8, "KANJI");

			public static readonly Mode FNC1_FIRST_POSITION = new Mode(null, 5, "FNC1_FIRST_POSITION");

			public static readonly Mode FNC1_SECOND_POSITION = new Mode(null, 9, "FNC1_SECOND_POSITION");

			private int[] characterCountBitsForVersions;

			private int bits;

			private string name;

			private Mode(int[] characterCountBitsForVersions, int bits, string name)
			{
				this.characterCountBitsForVersions = characterCountBitsForVersions;
				this.bits = bits;
				this.name = name;
			}

			public static Mode ForBits(int bits)
			{
				return bits switch
				{
					0 => TERMINATOR, 
					1 => NUMERIC, 
					2 => ALPHANUMERIC, 
					3 => STRUCTURED_APPEND, 
					4 => BYTE, 
					5 => FNC1_FIRST_POSITION, 
					7 => ECI, 
					8 => KANJI, 
					9 => FNC1_SECOND_POSITION, 
					_ => throw new StiQRCodeException(), 
				};
			}

			public int GetCharacterCountBits(Version version)
			{
				if (characterCountBitsForVersions == null)
				{
					throw new StiQRCodeException("Character count doesn't apply to this mode");
				}
				int versionNumber = version.GetVersionNumber();
				int num = ((versionNumber > 9) ? ((versionNumber <= 26) ? 1 : 2) : 0);
				return characterCountBitsForVersions[num];
			}

			public int GetBits()
			{
				return bits;
			}

			public string GetName()
			{
				return name;
			}

			public override string ToString()
			{
				return name;
			}
		}

		public sealed class MaskUtil
		{
			private MaskUtil()
			{
			}

			public static int ApplyMaskPenaltyRule1(ByteMatrix matrix)
			{
				return ApplyMaskPenaltyRule1Internal(matrix, isHorizontal: true) + ApplyMaskPenaltyRule1Internal(matrix, isHorizontal: false);
			}

			public static int ApplyMaskPenaltyRule2(ByteMatrix matrix)
			{
				int num = 0;
				sbyte[][] array = matrix.GetArray();
				int width = matrix.GetWidth();
				int height = matrix.GetHeight();
				for (int i = 0; i < height - 1; i++)
				{
					for (int j = 0; j < width - 1; j++)
					{
						int num2 = array[i][j];
						if (num2 == array[i][j + 1] && num2 == array[i + 1][j] && num2 == array[i + 1][j + 1])
						{
							num += 3;
						}
					}
				}
				return num;
			}

			public static int ApplyMaskPenaltyRule3(ByteMatrix matrix)
			{
				int num = 0;
				sbyte[][] array = matrix.GetArray();
				int width = matrix.GetWidth();
				int height = matrix.GetHeight();
				for (int i = 0; i < height; i++)
				{
					for (int j = 0; j < width; j++)
					{
						if (j + 6 < width && array[i][j] == 1 && array[i][j + 1] == 0 && array[i][j + 2] == 1 && array[i][j + 3] == 1 && array[i][j + 4] == 1 && array[i][j + 5] == 0 && array[i][j + 6] == 1 && ((j + 10 < width && array[i][j + 7] == 0 && array[i][j + 8] == 0 && array[i][j + 9] == 0 && array[i][j + 10] == 0) || (j - 4 >= 0 && array[i][j - 1] == 0 && array[i][j - 2] == 0 && array[i][j - 3] == 0 && array[i][j - 4] == 0)))
						{
							num += 40;
						}
						if (i + 6 < height && array[i][j] == 1 && array[i + 1][j] == 0 && array[i + 2][j] == 1 && array[i + 3][j] == 1 && array[i + 4][j] == 1 && array[i + 5][j] == 0 && array[i + 6][j] == 1 && ((i + 10 < height && array[i + 7][j] == 0 && array[i + 8][j] == 0 && array[i + 9][j] == 0 && array[i + 10][j] == 0) || (i - 4 >= 0 && array[i - 1][j] == 0 && array[i - 2][j] == 0 && array[i - 3][j] == 0 && array[i - 4][j] == 0)))
						{
							num += 40;
						}
					}
				}
				return num;
			}

			public static int ApplyMaskPenaltyRule4(ByteMatrix matrix)
			{
				int num = 0;
				sbyte[][] array = matrix.GetArray();
				int width = matrix.GetWidth();
				int height = matrix.GetHeight();
				for (int i = 0; i < height; i++)
				{
					for (int j = 0; j < width; j++)
					{
						if (array[i][j] == 1)
						{
							num++;
						}
					}
				}
				int num2 = matrix.GetHeight() * matrix.GetWidth();
				double num3 = (double)num / (double)num2;
				return Math.Abs((int)(num3 * 100.0 - 50.0)) / 5 * 10;
			}

			public static bool GetDataMaskBit(int maskPattern, int x, int y)
			{
				int num2;
				switch (maskPattern)
				{
				case 0:
					num2 = (y + x) & 1;
					break;
				case 1:
					num2 = y & 1;
					break;
				case 2:
					num2 = x % 3;
					break;
				case 3:
					num2 = (y + x) % 3;
					break;
				case 4:
					num2 = ((y >> 1) + x / 3) & 1;
					break;
				case 5:
				{
					int num = y * x;
					num2 = (num & 1) + num % 3;
					break;
				}
				case 6:
				{
					int num = y * x;
					num2 = ((num & 1) + num % 3) & 1;
					break;
				}
				case 7:
				{
					int num = y * x;
					num2 = (num % 3 + ((y + x) & 1)) & 1;
					break;
				}
				default:
					throw new StiQRCodeException("Invalid mask pattern: " + maskPattern);
				}
				return num2 == 0;
			}

			private static int ApplyMaskPenaltyRule1Internal(ByteMatrix matrix, bool isHorizontal)
			{
				int num = 0;
				int num2 = 0;
				int num3 = -1;
				int num4 = (isHorizontal ? matrix.GetHeight() : matrix.GetWidth());
				int num5 = (isHorizontal ? matrix.GetWidth() : matrix.GetHeight());
				sbyte[][] array = matrix.GetArray();
				for (int i = 0; i < num4; i++)
				{
					for (int j = 0; j < num5; j++)
					{
						int num6 = (isHorizontal ? array[i][j] : array[j][i]);
						if (num6 == num3)
						{
							num2++;
							if (num2 == 5)
							{
								num += 3;
							}
							else if (num2 > 5)
							{
								num++;
							}
						}
						else
						{
							num2 = 1;
							num3 = num6;
						}
					}
					num2 = 0;
				}
				return num;
			}
		}

		public sealed class MatrixUtil
		{
			private static readonly int[][] POSITION_DETECTION_PATTERN = new int[7][]
			{
				new int[7] { 1, 1, 1, 1, 1, 1, 1 },
				new int[7] { 1, 0, 0, 0, 0, 0, 1 },
				new int[7] { 1, 0, 1, 1, 1, 0, 1 },
				new int[7] { 1, 0, 1, 1, 1, 0, 1 },
				new int[7] { 1, 0, 1, 1, 1, 0, 1 },
				new int[7] { 1, 0, 0, 0, 0, 0, 1 },
				new int[7] { 1, 1, 1, 1, 1, 1, 1 }
			};

			private static readonly int[][] HORIZONTAL_SEPARATION_PATTERN = new int[1][] { new int[8] };

			private static readonly int[][] VERTICAL_SEPARATION_PATTERN = new int[7][]
			{
				new int[1],
				new int[1],
				new int[1],
				new int[1],
				new int[1],
				new int[1],
				new int[1]
			};

			private static readonly int[][] POSITION_ADJUSTMENT_PATTERN = new int[5][]
			{
				new int[5] { 1, 1, 1, 1, 1 },
				new int[5] { 1, 0, 0, 0, 1 },
				new int[5] { 1, 0, 1, 0, 1 },
				new int[5] { 1, 0, 0, 0, 1 },
				new int[5] { 1, 1, 1, 1, 1 }
			};

			private static readonly int[][] POSITION_ADJUSTMENT_PATTERN_COORDINATE_TABLE = new int[40][]
			{
				new int[7] { -1, -1, -1, -1, -1, -1, -1 },
				new int[7] { 6, 18, -1, -1, -1, -1, -1 },
				new int[7] { 6, 22, -1, -1, -1, -1, -1 },
				new int[7] { 6, 26, -1, -1, -1, -1, -1 },
				new int[7] { 6, 30, -1, -1, -1, -1, -1 },
				new int[7] { 6, 34, -1, -1, -1, -1, -1 },
				new int[7] { 6, 22, 38, -1, -1, -1, -1 },
				new int[7] { 6, 24, 42, -1, -1, -1, -1 },
				new int[7] { 6, 26, 46, -1, -1, -1, -1 },
				new int[7] { 6, 28, 50, -1, -1, -1, -1 },
				new int[7] { 6, 30, 54, -1, -1, -1, -1 },
				new int[7] { 6, 32, 58, -1, -1, -1, -1 },
				new int[7] { 6, 34, 62, -1, -1, -1, -1 },
				new int[7] { 6, 26, 46, 66, -1, -1, -1 },
				new int[7] { 6, 26, 48, 70, -1, -1, -1 },
				new int[7] { 6, 26, 50, 74, -1, -1, -1 },
				new int[7] { 6, 30, 54, 78, -1, -1, -1 },
				new int[7] { 6, 30, 56, 82, -1, -1, -1 },
				new int[7] { 6, 30, 58, 86, -1, -1, -1 },
				new int[7] { 6, 34, 62, 90, -1, -1, -1 },
				new int[7] { 6, 28, 50, 72, 94, -1, -1 },
				new int[7] { 6, 26, 50, 74, 98, -1, -1 },
				new int[7] { 6, 30, 54, 78, 102, -1, -1 },
				new int[7] { 6, 28, 54, 80, 106, -1, -1 },
				new int[7] { 6, 32, 58, 84, 110, -1, -1 },
				new int[7] { 6, 30, 58, 86, 114, -1, -1 },
				new int[7] { 6, 34, 62, 90, 118, -1, -1 },
				new int[7] { 6, 26, 50, 74, 98, 122, -1 },
				new int[7] { 6, 30, 54, 78, 102, 126, -1 },
				new int[7] { 6, 26, 52, 78, 104, 130, -1 },
				new int[7] { 6, 30, 56, 82, 108, 134, -1 },
				new int[7] { 6, 34, 60, 86, 112, 138, -1 },
				new int[7] { 6, 30, 58, 86, 114, 142, -1 },
				new int[7] { 6, 34, 62, 90, 118, 146, -1 },
				new int[7] { 6, 30, 54, 78, 102, 126, 150 },
				new int[7] { 6, 24, 50, 76, 102, 128, 154 },
				new int[7] { 6, 28, 54, 80, 106, 132, 158 },
				new int[7] { 6, 32, 58, 84, 110, 136, 162 },
				new int[7] { 6, 26, 54, 82, 110, 138, 166 },
				new int[7] { 6, 30, 58, 86, 114, 142, 170 }
			};

			private static readonly int[][] TYPE_INFO_COORDINATES = new int[15][]
			{
				new int[2] { 8, 0 },
				new int[2] { 8, 1 },
				new int[2] { 8, 2 },
				new int[2] { 8, 3 },
				new int[2] { 8, 4 },
				new int[2] { 8, 5 },
				new int[2] { 8, 7 },
				new int[2] { 8, 8 },
				new int[2] { 7, 8 },
				new int[2] { 5, 8 },
				new int[2] { 4, 8 },
				new int[2] { 3, 8 },
				new int[2] { 2, 8 },
				new int[2] { 1, 8 },
				new int[2] { 0, 8 }
			};

			private const int VERSION_INFO_POLY = 7973;

			private const int TYPE_INFO_POLY = 1335;

			private const int TYPE_INFO_MASK_PATTERN = 21522;

			private MatrixUtil()
			{
			}

			public static void ClearMatrix(ByteMatrix matrix)
			{
				matrix.Clear(-1);
			}

			public static void BuildMatrix(BitVector dataBits, ErrorCorrectionLevel ecLevel, int version, int maskPattern, ByteMatrix matrix)
			{
				ClearMatrix(matrix);
				EmbedBasicPatterns(version, matrix);
				EmbedTypeInfo(ecLevel, maskPattern, matrix);
				MaybeEmbedVersionInfo(version, matrix);
				EmbedDataBits(dataBits, maskPattern, matrix);
			}

			public static void EmbedBasicPatterns(int version, ByteMatrix matrix)
			{
				EmbedPositionDetectionPatternsAndSeparators(matrix);
				EmbedDarkDotAtLeftBottomCorner(matrix);
				MaybeEmbedPositionAdjustmentPatterns(version, matrix);
				EmbedTimingPatterns(matrix);
			}

			public static void EmbedTypeInfo(ErrorCorrectionLevel ecLevel, int maskPattern, ByteMatrix matrix)
			{
				BitVector bitVector = new BitVector();
				MakeTypeInfoBits(ecLevel, maskPattern, bitVector);
				for (int i = 0; i < bitVector.Size(); i++)
				{
					int value = bitVector.At(bitVector.Size() - 1 - i);
					int x = TYPE_INFO_COORDINATES[i][0];
					int y = TYPE_INFO_COORDINATES[i][1];
					matrix.Set(x, y, value);
					if (i < 8)
					{
						int x2 = matrix.GetWidth() - i - 1;
						int y2 = 8;
						matrix.Set(x2, y2, value);
					}
					else
					{
						int x3 = 8;
						int y3 = matrix.GetHeight() - 7 + (i - 8);
						matrix.Set(x3, y3, value);
					}
				}
			}

			public static void MaybeEmbedVersionInfo(int version, ByteMatrix matrix)
			{
				if (version < 7)
				{
					return;
				}
				BitVector bitVector = new BitVector();
				MakeVersionInfoBits(version, bitVector);
				int num = 17;
				for (int i = 0; i < 6; i++)
				{
					for (int j = 0; j < 3; j++)
					{
						int value = bitVector.At(num);
						num--;
						matrix.Set(i, matrix.GetHeight() - 11 + j, value);
						matrix.Set(matrix.GetHeight() - 11 + j, i, value);
					}
				}
			}

			public static void EmbedDataBits(BitVector dataBits, int maskPattern, ByteMatrix matrix)
			{
				int num = 0;
				int num2 = -1;
				int num3 = matrix.GetWidth() - 1;
				int i = matrix.GetHeight() - 1;
				while (num3 > 0)
				{
					if (num3 == 6)
					{
						num3--;
					}
					for (; i >= 0 && i < matrix.GetHeight(); i += num2)
					{
						for (int j = 0; j < 2; j++)
						{
							int x = num3 - j;
							if (IsEmpty(matrix.Get(x, i)))
							{
								int num4;
								if (num < dataBits.Size())
								{
									num4 = dataBits.At(num);
									num++;
								}
								else
								{
									num4 = 0;
								}
								if (maskPattern != -1 && MaskUtil.GetDataMaskBit(maskPattern, x, i))
								{
									num4 ^= 1;
								}
								matrix.Set(x, i, num4);
							}
						}
					}
					num2 = -num2;
					i += num2;
					num3 -= 2;
				}
				if (num != dataBits.Size())
				{
					throw new StiQRCodeException("Not all bits consumed: " + num + "/" + dataBits.Size());
				}
			}

			public static void EmbedDataBits2(BitVector dataBits, List<ByteMatrix> matrices)
			{
				ByteMatrix byteMatrix = matrices[0];
				int width = byteMatrix.GetWidth();
				int height = byteMatrix.GetHeight();
				int num = 0;
				int num2 = -1;
				int num3 = width - 1;
				int i = height - 1;
				while (num3 > 0)
				{
					if (num3 == 6)
					{
						num3--;
					}
					for (; i >= 0 && i < height; i += num2)
					{
						for (int j = 0; j < 2; j++)
						{
							int x = num3 - j;
							if (byteMatrix.Get(x, i) != -1)
							{
								continue;
							}
							int num4;
							if (num < dataBits.Size())
							{
								num4 = dataBits.At(num);
								num++;
							}
							else
							{
								num4 = 0;
							}
							for (int k = 0; k < 8; k++)
							{
								int num5 = num4;
								if (MaskUtil.GetDataMaskBit(k, x, i))
								{
									num5 ^= 1;
								}
								matrices[k].Set(x, i, num5);
							}
						}
					}
					num2 = -num2;
					i += num2;
					num3 -= 2;
				}
				if (num != dataBits.Size())
				{
					throw new StiQRCodeException("Not all bits consumed: " + num + "/" + dataBits.Size());
				}
			}

			public static int FindMSBSet(int value)
			{
				uint num = (uint)value;
				int num2 = 0;
				while (num != 0)
				{
					num >>= 1;
					num2++;
				}
				return num2;
			}

			public static int CalculateBCHCode(int value, int poly)
			{
				int num = FindMSBSet(poly);
				value <<= num - 1;
				while (FindMSBSet(value) >= num)
				{
					value ^= poly << FindMSBSet(value) - num;
				}
				return value;
			}

			public static void MakeTypeInfoBits(ErrorCorrectionLevel ecLevel, int maskPattern, BitVector bits)
			{
				if (!IsValidMaskPattern(maskPattern))
				{
					throw new StiQRCodeException("Invalid mask pattern");
				}
				int value = (ecLevel.GetBits() << 3) | maskPattern;
				bits.AppendBits(value, 5);
				int value2 = CalculateBCHCode(value, 1335);
				bits.AppendBits(value2, 10);
				BitVector bitVector = new BitVector();
				bitVector.AppendBits(21522, 15);
				bits.Xor(bitVector);
				if (bits.Size() != 15)
				{
					throw new StiQRCodeException("should not happen but we got: " + bits.Size());
				}
			}

			public static void MakeVersionInfoBits(int version, BitVector bits)
			{
				bits.AppendBits(version, 6);
				int value = CalculateBCHCode(version, 7973);
				bits.AppendBits(value, 12);
				if (bits.Size() != 18)
				{
					throw new StiQRCodeException("should not happen but we got: " + bits.Size());
				}
			}

			private static bool IsEmpty(int value)
			{
				return value == -1;
			}

			private static bool IsValidValue(int value)
			{
				if (value != -1 && value != 0)
				{
					return value == 1;
				}
				return true;
			}

			private static void EmbedTimingPatterns(ByteMatrix matrix)
			{
				for (int i = 8; i < matrix.GetWidth() - 8; i++)
				{
					int value = (i + 1) % 2;
					if (!IsValidValue(matrix.Get(i, 6)))
					{
						throw new StiQRCodeException();
					}
					if (IsEmpty(matrix.Get(i, 6)))
					{
						matrix.Set(i, 6, value);
					}
					if (!IsValidValue(matrix.Get(6, i)))
					{
						throw new StiQRCodeException();
					}
					if (IsEmpty(matrix.Get(6, i)))
					{
						matrix.Set(6, i, value);
					}
				}
			}

			private static void EmbedDarkDotAtLeftBottomCorner(ByteMatrix matrix)
			{
				if (matrix.Get(8, matrix.GetHeight() - 8) == 0)
				{
					throw new StiQRCodeException();
				}
				matrix.Set(8, matrix.GetHeight() - 8, 1);
			}

			private static void EmbedHorizontalSeparationPattern(int xStart, int yStart, ByteMatrix matrix)
			{
				if (HORIZONTAL_SEPARATION_PATTERN[0].Length != 8 || HORIZONTAL_SEPARATION_PATTERN.GetLength(0) != 1)
				{
					throw new StiQRCodeException("Bad horizontal separation pattern");
				}
				for (int i = 0; i < 8; i++)
				{
					if (!IsEmpty(matrix.Get(xStart + i, yStart)))
					{
						throw new StiQRCodeException();
					}
					matrix.Set(xStart + i, yStart, HORIZONTAL_SEPARATION_PATTERN[0][i]);
				}
			}

			private static void EmbedVerticalSeparationPattern(int xStart, int yStart, ByteMatrix matrix)
			{
				if (VERTICAL_SEPARATION_PATTERN[0].Length != 1 || VERTICAL_SEPARATION_PATTERN.GetLength(0) != 7)
				{
					throw new StiQRCodeException("Bad vertical separation pattern");
				}
				for (int i = 0; i < 7; i++)
				{
					if (!IsEmpty(matrix.Get(xStart, yStart + i)))
					{
						throw new StiQRCodeException();
					}
					matrix.Set(xStart, yStart + i, VERTICAL_SEPARATION_PATTERN[i][0]);
				}
			}

			private static void EmbedPositionAdjustmentPattern(int xStart, int yStart, ByteMatrix matrix)
			{
				if (POSITION_ADJUSTMENT_PATTERN[0].Length != 5 || POSITION_ADJUSTMENT_PATTERN.GetLength(0) != 5)
				{
					throw new StiQRCodeException("Bad position adjustment");
				}
				for (int i = 0; i < 5; i++)
				{
					for (int j = 0; j < 5; j++)
					{
						if (!IsEmpty(matrix.Get(xStart + j, yStart + i)))
						{
							throw new StiQRCodeException();
						}
						matrix.Set(xStart + j, yStart + i, POSITION_ADJUSTMENT_PATTERN[i][j]);
					}
				}
			}

			private static void EmbedPositionDetectionPattern(int xStart, int yStart, ByteMatrix matrix)
			{
				if (POSITION_DETECTION_PATTERN[0].Length != 7 || POSITION_DETECTION_PATTERN.GetLength(0) != 7)
				{
					throw new StiQRCodeException("Bad position detection pattern");
				}
				for (int i = 0; i < 7; i++)
				{
					for (int j = 0; j < 7; j++)
					{
						if (!IsEmpty(matrix.Get(xStart + j, yStart + i)))
						{
							throw new StiQRCodeException();
						}
						matrix.Set(xStart + j, yStart + i, POSITION_DETECTION_PATTERN[i][j]);
					}
				}
			}

			private static void EmbedPositionDetectionPatternsAndSeparators(ByteMatrix matrix)
			{
				int num = POSITION_DETECTION_PATTERN[0].Length;
				EmbedPositionDetectionPattern(0, 0, matrix);
				EmbedPositionDetectionPattern(matrix.GetWidth() - num, 0, matrix);
				EmbedPositionDetectionPattern(0, matrix.GetWidth() - num, matrix);
				int num2 = HORIZONTAL_SEPARATION_PATTERN[0].Length;
				EmbedHorizontalSeparationPattern(0, num2 - 1, matrix);
				EmbedHorizontalSeparationPattern(matrix.GetWidth() - num2, num2 - 1, matrix);
				EmbedHorizontalSeparationPattern(0, matrix.GetWidth() - num2, matrix);
				int num3 = VERTICAL_SEPARATION_PATTERN.Length;
				EmbedVerticalSeparationPattern(num3, 0, matrix);
				EmbedVerticalSeparationPattern(matrix.GetHeight() - num3 - 1, 0, matrix);
				EmbedVerticalSeparationPattern(num3, matrix.GetHeight() - num3, matrix);
			}

			private static void MaybeEmbedPositionAdjustmentPatterns(int version, ByteMatrix matrix)
			{
				if (version < 2)
				{
					return;
				}
				int num = version - 1;
				int[] array = POSITION_ADJUSTMENT_PATTERN_COORDINATE_TABLE[num];
				int num2 = POSITION_ADJUSTMENT_PATTERN_COORDINATE_TABLE[num].Length;
				for (int i = 0; i < num2; i++)
				{
					for (int j = 0; j < num2; j++)
					{
						int num3 = array[i];
						int num4 = array[j];
						if (num4 != -1 && num3 != -1 && IsEmpty(matrix.Get(num4, num3)))
						{
							EmbedPositionAdjustmentPattern(num4 - 2, num3 - 2, matrix);
						}
					}
				}
			}
		}

		public const int NUM_MASK_PATTERNS = 8;

		private Mode mode;

		private ErrorCorrectionLevel ecLevel;

		private int version;

		private int matrixWidth;

		private int maskPattern;

		private int numTotalBytes;

		private int numDataBytes;

		private int numECBytes;

		private int numRSBlocks;

		private ByteMatrix matrix;

		public Mode GetMode()
		{
			return mode;
		}

		public ErrorCorrectionLevel GetECLevel()
		{
			return ecLevel;
		}

		public int GetVersion()
		{
			return version;
		}

		public int GetMatrixWidth()
		{
			return matrixWidth;
		}

		public int GetMaskPattern()
		{
			return maskPattern;
		}

		public int GetNumTotalBytes()
		{
			return numTotalBytes;
		}

		public int GetNumDataBytes()
		{
			return numDataBytes;
		}

		public int GetNumECBytes()
		{
			return numECBytes;
		}

		public int GetNumRSBlocks()
		{
			return numRSBlocks;
		}

		public ByteMatrix GetMatrix()
		{
			return matrix;
		}

		public int At(int x, int y)
		{
			int num = matrix.Get(x, y);
			if (num != 0 && num != 1)
			{
				throw new StiQRCodeException("Bad value");
			}
			return num;
		}

		public bool IsValid()
		{
			if (mode != null && ecLevel != null && version != -1 && matrixWidth != -1 && maskPattern != -1 && numTotalBytes != -1 && numDataBytes != -1 && numECBytes != -1 && numRSBlocks != -1 && IsValidMaskPattern(maskPattern) && numTotalBytes == numDataBytes + numECBytes && matrix != null && matrixWidth == matrix.GetWidth())
			{
				return matrix.GetWidth() == matrix.GetHeight();
			}
			return false;
		}

		public void SetMode(Mode value)
		{
			mode = value;
		}

		public void SetECLevel(ErrorCorrectionLevel value)
		{
			ecLevel = value;
		}

		public void SetVersion(int value)
		{
			version = value;
		}

		public void SetMatrixWidth(int value)
		{
			matrixWidth = value;
		}

		public void SetMaskPattern(int value)
		{
			maskPattern = value;
		}

		public void SetNumTotalBytes(int value)
		{
			numTotalBytes = value;
		}

		public void SetNumDataBytes(int value)
		{
			numDataBytes = value;
		}

		public void SetNumECBytes(int value)
		{
			numECBytes = value;
		}

		public void SetNumRSBlocks(int value)
		{
			numRSBlocks = value;
		}

		public void SetMatrix(ByteMatrix value)
		{
			matrix = value;
		}

		public static bool IsValidMaskPattern(int maskPattern)
		{
			if (maskPattern >= 0)
			{
				return maskPattern < 8;
			}
			return false;
		}

		public StiQRCode()
		{
			mode = null;
			ecLevel = null;
			version = -1;
			matrixWidth = -1;
			maskPattern = -1;
			numTotalBytes = -1;
			numDataBytes = -1;
			numECBytes = -1;
			numRSBlocks = -1;
			matrix = null;
		}
	}

	private float module = 40f;

	private float height = 1f;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiQRCodeBarCodeType;

	public override string ServiceName => "QR Code";

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

	[Description("Gets or sets the error correction level.")]
	[DefaultValue(StiQRCodeErrorCorrectionLevel.Level1)]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public StiQRCodeErrorCorrectionLevel ErrorCorrectionLevel { get; set; }

	[DefaultValue(StiQRCodeSize.Automatic)]
	[StiSerializable]
	[Description("Gets or sets matrix size.")]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public StiQRCodeSize MatrixSize { get; set; }

	[StiSerializable]
	[DefaultValue(null)]
	[Description("Gets or sets image value.")]
	[Editor("Stimulsoft.Report.Components.Design.StiSimpleImageEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiSimpeImageConverter))]
	[StiOrder(120)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public virtual Image Image { get; set; }

	[StiSerializable]
	[DefaultValue(1.0)]
	[Description("Gets or sets value to multiply by it an image size.")]
	[StiOrder(130)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public virtual double ImageMultipleFactor { get; set; } = 1.0;


	internal override float LabelFontHeight => 8f;

	public override bool[] VisibleProperties
	{
		get
		{
			bool[] array = new bool[27];
			array[10] = true;
			array[12] = true;
			array[13] = true;
			return array;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyFloat(val, "Height", Height, 1f);
		JsonExtensions.AddPropertyFloat(val, "Module", Module, 40f);
		JsonExtensions.AddPropertyEnum(val, "ErrorCorrectionLevel", (Enum)ErrorCorrectionLevel, (Enum)StiQRCodeErrorCorrectionLevel.Level1);
		JsonExtensions.AddPropertyEnum(val, "MatrixSize", (Enum)MatrixSize, (Enum)StiQRCodeSize.Automatic);
		JsonExtensions.AddPropertyDouble(val, "ImageMultipleFactor", ImageMultipleFactor, 1.0);
		JsonExtensions.AddPropertyImage(val, "Image", Image);
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
			case "ErrorCorrectionLevel":
				ErrorCorrectionLevel = StiJsonReportObjectHelper.DeserializeEnum<StiQRCodeErrorCorrectionLevel>(item);
				break;
			case "MatrixSize":
				MatrixSize = StiJsonReportObjectHelper.DeserializeEnum<StiQRCodeSize>(item);
				break;
			case "ImageMultipleFactor":
				ImageMultipleFactor = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "Image":
				Image = StiJsonReportObjectHelper.DeserializeImage(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[5]
		{
			propertiesHelper.ErrorCorrectionLevel(),
			propertiesHelper.MatrixSize(),
			propertiesHelper.Image(),
			propertiesHelper.ImageMultipleFactor(),
			propertiesHelper.Module()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public override void Draw(object context, StiBarCode barCode, RectangleF rect, float zoom)
	{
		string code = GetCode(barCode);
		base.BarCodeData.Code = code;
		StiQRCode.ErrorCorrectionLevel ecLevel = StiQRCode.ErrorCorrectionLevel.L;
		if (ErrorCorrectionLevel == StiQRCodeErrorCorrectionLevel.Level2)
		{
			ecLevel = StiQRCode.ErrorCorrectionLevel.M;
		}
		if (ErrorCorrectionLevel == StiQRCodeErrorCorrectionLevel.Level3)
		{
			ecLevel = StiQRCode.ErrorCorrectionLevel.Q;
		}
		if (ErrorCorrectionLevel == StiQRCodeErrorCorrectionLevel.Level4)
		{
			ecLevel = StiQRCode.ErrorCorrectionLevel.H;
		}
		try
		{
			StiQRCode stiQRCode = new StiQRCode();
			if (!(context is StiBarCodeExportPainter) || !(context as StiBarCodeExportPainter).OnlyAssembleData)
			{
				StiQRCode.QREncoder.Encode(code, ecLevel, stiQRCode, MatrixSize);
			}
			else
			{
				stiQRCode.SetMatrix(new StiQRCode.ByteMatrix(1, 1));
			}
			StiQRCode.ByteMatrix matrix = stiQRCode.GetMatrix();
			byte[] array = new byte[matrix.GetWidth() * matrix.GetHeight()];
			for (int i = 0; i < matrix.GetHeight(); i++)
			{
				int num = i * matrix.GetWidth();
				for (int j = 0; j < matrix.GetWidth(); j++)
				{
					array[num + j] = (byte)matrix.Get(j, i);
				}
			}
			base.BarCodeData.MatrixGrid = array;
			base.BarCodeData.MatrixWidth = matrix.GetWidth();
			base.BarCodeData.MatrixHeight = matrix.GetHeight();
			base.BarCodeData.MatrixRatioY = 1;
			Draw2DBarCode(context, rect, barCode, zoom);
		}
		catch
		{
			DrawBarCodeError(context, rect, barCode);
		}
	}

	public static Image GetBarcodeImage(string code, int zoom)
	{
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		StiBarCode stiBarCode = new StiBarCode();
		stiBarCode.BarCodeType = new StiQRCodeBarCodeType(10f, StiQRCodeErrorCorrectionLevel.Level1, StiQRCodeSize.Automatic);
		StiBarCodeData stiBarCodeData = (stiBarCode.BarCodeType as StiQRCodeBarCodeType).BarCodeData;
		stiBarCodeData.Code = code;
		Image val = null;
		StiQRCode.ErrorCorrectionLevel l = StiQRCode.ErrorCorrectionLevel.L;
		try
		{
			StiQRCode stiQRCode = new StiQRCode();
			StiQRCode.QREncoder.Encode(code, l, stiQRCode, StiQRCodeSize.Automatic);
			StiQRCode.ByteMatrix matrix = stiQRCode.GetMatrix();
			byte[] array = new byte[matrix.GetWidth() * matrix.GetHeight()];
			for (int i = 0; i < matrix.GetHeight(); i++)
			{
				int num = i * matrix.GetWidth();
				for (int j = 0; j < matrix.GetWidth(); j++)
				{
					array[num + j] = (byte)matrix.Get(j, i);
				}
			}
			stiBarCodeData.MatrixGrid = array;
			stiBarCodeData.MatrixWidth = matrix.GetWidth();
			stiBarCodeData.MatrixHeight = matrix.GetHeight();
			stiBarCodeData.MatrixRatioY = 1;
			int num2 = 2;
			if (!stiBarCode.ShowQuietZones)
			{
				num2 = 0;
			}
			int num3 = (stiBarCodeData.MatrixWidth + num2 * 2) * zoom;
			int num4 = (stiBarCodeData.MatrixHeight + num2 * 2) * zoom;
			val = (Image)new Bitmap(num3, num4);
			Graphics val2 = Graphics.FromImage(val);
			try
			{
				(stiBarCode.BarCodeType as StiQRCodeBarCodeType).Draw2DBarCode(val2, new Rectangle(0, 0, num3, num4), stiBarCode, zoom);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		catch
		{
		}
		return val;
	}

	public override StiBarCodeTypeService CreateNew()
	{
		return new StiQRCodeBarCodeType();
	}

	public StiQRCodeBarCodeType()
		: this(40f, StiQRCodeErrorCorrectionLevel.Level1, StiQRCodeSize.Automatic)
	{
	}

	public StiQRCodeBarCodeType(float module, StiQRCodeErrorCorrectionLevel errorCorrectionLevel, StiQRCodeSize matrixSize)
	{
		this.module = module;
		ErrorCorrectionLevel = errorCorrectionLevel;
		MatrixSize = matrixSize;
	}

	public StiQRCodeBarCodeType(float module, StiQRCodeErrorCorrectionLevel errorCorrectionLevel, StiQRCodeSize matrixSize, Image image)
	{
		this.module = module;
		ErrorCorrectionLevel = errorCorrectionLevel;
		MatrixSize = matrixSize;
		Image = image;
	}

	public StiQRCodeBarCodeType(float module, StiQRCodeErrorCorrectionLevel errorCorrectionLevel, StiQRCodeSize matrixSize, Image image, double imageMultipleFactor)
	{
		this.module = module;
		ErrorCorrectionLevel = errorCorrectionLevel;
		MatrixSize = matrixSize;
		Image = image;
		ImageMultipleFactor = imageMultipleFactor;
	}
}
