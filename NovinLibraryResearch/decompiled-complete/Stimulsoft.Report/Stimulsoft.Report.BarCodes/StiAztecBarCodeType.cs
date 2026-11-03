using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.BarCodes.Design;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.BarCodes;

[TypeConverter(typeof(StiAztecBarCodeTypeConverter))]
public class StiAztecBarCodeType : StiBarCodeTypeService
{
	protected class StiAztecException : Exception
	{
		public StiAztecException()
		{
		}

		public StiAztecException(string message)
			: base(message)
		{
		}

		public StiAztecException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}

	private sealed class StiAztec
	{
		internal sealed class AztecCode
		{
			public bool IsCompact { get; set; }

			public int Size { get; set; }

			public int Layers { get; set; }

			public int CodeWords { get; set; }

			public StiBarcodeUtils.BitMatrix Matrix { get; set; }
		}

		private static class Encoder
		{
			private const int MAX_NB_BITS = 32;

			private const int MAX_NB_BITS_COMPACT = 4;

			private static readonly int[] WORD_SIZE = new int[33]
			{
				4, 6, 6, 8, 8, 8, 8, 8, 8, 10,
				10, 10, 10, 10, 10, 10, 10, 10, 10, 10,
				10, 10, 10, 12, 12, 12, 12, 12, 12, 12,
				12, 12, 12
			};

			public static AztecCode Encode(byte[] data, int eci, int minECCPercent, StiAztecSize userSpecifiedLayers)
			{
				BitArray bitArray = new HighLevelEncoder(data, eci).Encode();
				int num = bitArray.Size * minECCPercent / 100 + 11;
				int num2 = bitArray.Size + num;
				bool flag;
				int num3;
				int num4;
				int num5;
				BitArray bitArray2;
				if (userSpecifiedLayers != 0)
				{
					flag = userSpecifiedLayers < StiAztecSize.Automatic;
					num3 = Math.Abs((int)userSpecifiedLayers);
					if (num3 > (flag ? 4 : 32))
					{
						num3 = (flag ? 4 : 32);
					}
					num4 = TotalBitsInLayer(num3, flag);
					num5 = WORD_SIZE[num3];
					int num6 = num4 - num4 % num5;
					bitArray2 = StuffBits(bitArray, num5);
					if (bitArray2.Size + num > num6 || (flag && bitArray2.Size > num5 * 64))
					{
						throw new StiAztecException("Too many data for specified layer");
					}
				}
				else
				{
					num5 = 0;
					bitArray2 = null;
					int num7 = 0;
					while (true)
					{
						if (num7 > 32)
						{
							throw new StiAztecException("Too many data for an Aztec code");
						}
						flag = num7 <= 3;
						num3 = (flag ? (num7 + 1) : num7);
						num4 = TotalBitsInLayer(num3, flag);
						if (num2 <= num4)
						{
							if (bitArray2 == null || num5 != WORD_SIZE[num3])
							{
								num5 = WORD_SIZE[num3];
								bitArray2 = StuffBits(bitArray, num5);
							}
							int num8 = num4 - num4 % num5;
							if ((!flag || bitArray2.Size <= num5 * 64) && bitArray2.Size + num <= num8)
							{
								break;
							}
						}
						num7++;
					}
				}
				BitArray bitArray3 = GenerateCheckWords(bitArray2, num4, num5);
				int num9 = bitArray2.Size / num5;
				BitArray modeMessage = GenerateModeMessage(flag, num3, num9);
				int num10 = (flag ? 11 : 14) + num3 * 4;
				int[] array = new int[num10];
				int num11;
				if (flag)
				{
					num11 = num10;
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = i;
					}
				}
				else
				{
					num11 = num10 + 1 + 2 * ((num10 / 2 - 1) / 15);
					int num12 = num10 / 2;
					int num13 = num11 / 2;
					for (int j = 0; j < num12; j++)
					{
						int num14 = j + j / 15;
						array[num12 - j - 1] = num13 - num14 - 1;
						array[num12 + j] = num13 + num14 + 1;
					}
				}
				StiBarcodeUtils.BitMatrix bitMatrix = new StiBarcodeUtils.BitMatrix(num11);
				int k = 0;
				int num15 = 0;
				for (; k < num3; k++)
				{
					int num16 = (num3 - k) * 4 + (flag ? 9 : 12);
					for (int l = 0; l < num16; l++)
					{
						int num17 = l * 2;
						for (int m = 0; m < 2; m++)
						{
							if (bitArray3[num15 + num17 + m])
							{
								bitMatrix[array[k * 2 + m], array[k * 2 + l]] = true;
							}
							if (bitArray3[num15 + num16 * 2 + num17 + m])
							{
								bitMatrix[array[k * 2 + l], array[num10 - 1 - k * 2 - m]] = true;
							}
							if (bitArray3[num15 + num16 * 4 + num17 + m])
							{
								bitMatrix[array[num10 - 1 - k * 2 - m], array[num10 - 1 - k * 2 - l]] = true;
							}
							if (bitArray3[num15 + num16 * 6 + num17 + m])
							{
								bitMatrix[array[num10 - 1 - k * 2 - l], array[k * 2 + m]] = true;
							}
						}
					}
					num15 += num16 * 8;
				}
				DrawModeMessage(bitMatrix, flag, num11, modeMessage);
				if (flag)
				{
					DrawBullsEye(bitMatrix, num11 / 2, 5);
				}
				else
				{
					DrawBullsEye(bitMatrix, num11 / 2, 7);
					int num18 = 0;
					int num19 = 0;
					while (num18 < num10 / 2 - 1)
					{
						for (int n = (num11 / 2) & 1; n < num11; n += 2)
						{
							bitMatrix[num11 / 2 - num19, n] = true;
							bitMatrix[num11 / 2 + num19, n] = true;
							bitMatrix[n, num11 / 2 - num19] = true;
							bitMatrix[n, num11 / 2 + num19] = true;
						}
						num18 += 15;
						num19 += 16;
					}
				}
				return new AztecCode
				{
					IsCompact = flag,
					Size = num11,
					Layers = num3,
					CodeWords = num9,
					Matrix = bitMatrix
				};
			}

			private static void DrawBullsEye(StiBarcodeUtils.BitMatrix matrix, int center, int size)
			{
				for (int i = 0; i < size; i += 2)
				{
					for (int j = center - i; j <= center + i; j++)
					{
						matrix[j, center - i] = true;
						matrix[j, center + i] = true;
						matrix[center - i, j] = true;
						matrix[center + i, j] = true;
					}
				}
				matrix[center - size, center - size] = true;
				matrix[center - size + 1, center - size] = true;
				matrix[center - size, center - size + 1] = true;
				matrix[center + size, center - size] = true;
				matrix[center + size, center - size + 1] = true;
				matrix[center + size, center + size - 1] = true;
			}

			internal static BitArray GenerateModeMessage(bool compact, int layers, int messageSizeInWords)
			{
				BitArray bitArray = new BitArray();
				if (compact)
				{
					bitArray.AppendBits(layers - 1, 2);
					bitArray.AppendBits(messageSizeInWords - 1, 6);
					return GenerateCheckWords(bitArray, 28, 4);
				}
				bitArray.AppendBits(layers - 1, 5);
				bitArray.AppendBits(messageSizeInWords - 1, 11);
				return GenerateCheckWords(bitArray, 40, 4);
			}

			private static void DrawModeMessage(StiBarcodeUtils.BitMatrix matrix, bool compact, int matrixSize, BitArray modeMessage)
			{
				int num = matrixSize / 2;
				if (compact)
				{
					for (int i = 0; i < 7; i++)
					{
						int num2 = num - 3 + i;
						if (modeMessage[i])
						{
							matrix[num2, num - 5] = true;
						}
						if (modeMessage[i + 7])
						{
							matrix[num + 5, num2] = true;
						}
						if (modeMessage[20 - i])
						{
							matrix[num2, num + 5] = true;
						}
						if (modeMessage[27 - i])
						{
							matrix[num - 5, num2] = true;
						}
					}
					return;
				}
				for (int j = 0; j < 10; j++)
				{
					int num3 = num - 5 + j + j / 5;
					if (modeMessage[j])
					{
						matrix[num3, num - 7] = true;
					}
					if (modeMessage[j + 10])
					{
						matrix[num + 7, num3] = true;
					}
					if (modeMessage[29 - j])
					{
						matrix[num3, num + 7] = true;
					}
					if (modeMessage[39 - j])
					{
						matrix[num - 7, num3] = true;
					}
				}
			}

			private static BitArray GenerateCheckWords(BitArray bitArray, int totalBits, int wordSize)
			{
				if (bitArray.Size % wordSize != 0)
				{
					throw new StiAztecException("size of bit array is not a multiple of the word size");
				}
				int num = bitArray.Size / wordSize;
				StiBarcodeUtils.ReedSolomonEncoder reedSolomonEncoder = new StiBarcodeUtils.ReedSolomonEncoder(GetGF(wordSize));
				int num2 = totalBits / wordSize;
				int[] array = BitsToWords(bitArray, wordSize, num2);
				reedSolomonEncoder.Encode(array, num2 - num);
				int numBits = totalBits % wordSize;
				BitArray bitArray2 = new BitArray();
				bitArray2.AppendBits(0, numBits);
				int[] array2 = array;
				foreach (int value in array2)
				{
					bitArray2.AppendBits(value, wordSize);
				}
				return bitArray2;
			}

			private static int[] BitsToWords(BitArray stuffedBits, int wordSize, int totalWords)
			{
				int[] array = new int[totalWords];
				int i = 0;
				for (int num = stuffedBits.Size / wordSize; i < num; i++)
				{
					int num2 = 0;
					for (int j = 0; j < wordSize; j++)
					{
						num2 |= (stuffedBits[i * wordSize + j] ? (1 << wordSize - j - 1) : 0);
					}
					array[i] = num2;
				}
				return array;
			}

			private static StiBarcodeUtils.GaloisField GetGF(int wordSize)
			{
				return wordSize switch
				{
					4 => StiBarcodeUtils.GaloisField.Aztec_Param, 
					6 => StiBarcodeUtils.GaloisField.Aztec_Data_6, 
					8 => StiBarcodeUtils.GaloisField.Aztec_Data_8, 
					10 => StiBarcodeUtils.GaloisField.Aztec_Data_10, 
					12 => StiBarcodeUtils.GaloisField.Aztec_Data_12, 
					_ => throw new StiAztecException("Unsupported word size " + wordSize), 
				};
			}

			internal static BitArray StuffBits(BitArray bits, int wordSize)
			{
				BitArray bitArray = new BitArray();
				int size = bits.Size;
				int num = (1 << wordSize) - 2;
				for (int i = 0; i < size; i += wordSize)
				{
					int num2 = 0;
					for (int j = 0; j < wordSize; j++)
					{
						if (i + j >= size || bits[i + j])
						{
							num2 |= 1 << wordSize - 1 - j;
						}
					}
					if ((num2 & num) == num)
					{
						bitArray.AppendBits(num2 & num, wordSize);
						i--;
					}
					else if ((num2 & num) == 0)
					{
						bitArray.AppendBits(num2 | 1, wordSize);
						i--;
					}
					else
					{
						bitArray.AppendBits(num2, wordSize);
					}
				}
				return bitArray;
			}

			private static int TotalBitsInLayer(int layers, bool compact)
			{
				return ((compact ? 88 : 112) + 16 * layers) * layers;
			}
		}

		private sealed class BitArray
		{
			private int[] bits;

			private int size;

			public int Size => size;

			public int SizeInBytes => size + 7 >> 3;

			public bool this[int i]
			{
				get
				{
					return (bits[i >> 5] & (1 << i)) != 0;
				}
				set
				{
					if (value)
					{
						bits[i >> 5] |= 1 << i;
					}
				}
			}

			public BitArray()
			{
				size = 0;
				bits = new int[1];
			}

			private void EnsureCapacity(int size)
			{
				if (size > bits.Length << 5)
				{
					int[] destinationArray = MakeArray(size);
					Array.Copy(bits, 0, destinationArray, 0, bits.Length);
					bits = destinationArray;
				}
			}

			public void AppendBit(bool bit)
			{
				EnsureCapacity(size + 1);
				if (bit)
				{
					bits[size >> 5] |= 1 << size;
				}
				size++;
			}

			public void AppendBits(int value, int numBits)
			{
				if (numBits < 0 || numBits > 32)
				{
					throw new StiAztecException("Num bits must be between 0 and 32");
				}
				EnsureCapacity(size + numBits);
				for (int num = numBits; num > 0; num--)
				{
					AppendBit(((value >> num - 1) & 1) == 1);
				}
			}

			public void AppendBitArray(BitArray other)
			{
				int num = other.size;
				EnsureCapacity(size + num);
				for (int i = 0; i < num; i++)
				{
					AppendBit(other[i]);
				}
			}

			private static int[] MakeArray(int size)
			{
				return new int[size + 31 >> 5];
			}
		}

		private abstract class Token
		{
			public static Token EMPTY = new SimpleToken(null, 0, 0);

			private readonly Token previous;

			public Token Previous => previous;

			protected Token(Token previous)
			{
				this.previous = previous;
			}

			public Token Add(int value, int bitCount)
			{
				return new SimpleToken(this, value, bitCount);
			}

			public Token AddBinaryShift(int start, int byteCount)
			{
				return new BinaryShiftToken(this, start, byteCount);
			}

			public abstract void AppendTo(BitArray bitArray, byte[] text);
		}

		private sealed class SimpleToken : Token
		{
			private readonly short value;

			private readonly short bitCount;

			public SimpleToken(Token previous, int value, int bitCount)
				: base(previous)
			{
				this.value = (short)value;
				this.bitCount = (short)bitCount;
			}

			public override void AppendTo(BitArray bitArray, byte[] text)
			{
				bitArray.AppendBits(value, bitCount);
			}
		}

		private sealed class BinaryShiftToken : Token
		{
			private readonly short binaryShiftStart;

			private readonly short binaryShiftByteCount;

			public BinaryShiftToken(Token previous, int binaryShiftStart, int binaryShiftByteCount)
				: base(previous)
			{
				this.binaryShiftStart = (short)binaryShiftStart;
				this.binaryShiftByteCount = (short)binaryShiftByteCount;
			}

			public override void AppendTo(BitArray bitArray, byte[] text)
			{
				for (int i = 0; i < binaryShiftByteCount; i++)
				{
					if (i == 0 || (i == 31 && binaryShiftByteCount <= 62))
					{
						bitArray.AppendBits(31, 5);
						if (binaryShiftByteCount > 62)
						{
							bitArray.AppendBits(binaryShiftByteCount - 31, 16);
						}
						else if (i == 0)
						{
							bitArray.AppendBits(Math.Min(binaryShiftByteCount, (short)31), 5);
						}
						else
						{
							bitArray.AppendBits(binaryShiftByteCount - 31, 5);
						}
					}
					bitArray.AppendBits(text[binaryShiftStart + i], 8);
				}
			}
		}

		private sealed class State
		{
			public static readonly State INITIAL_STATE = new State(Token.EMPTY, 0, 0, 0);

			private readonly int mode;

			private readonly Token token;

			private readonly int binaryShiftByteCount;

			private readonly int bitCount;

			public int Mode => mode;

			public Token Token => token;

			public int BinaryShiftByteCount => binaryShiftByteCount;

			public int BitCount => bitCount;

			public State(Token token, int mode, int binaryBytes, int bitCount)
			{
				this.token = token;
				this.mode = mode;
				binaryShiftByteCount = binaryBytes;
				this.bitCount = bitCount;
			}

			public State AppendFLGn(int eci)
			{
				State state = ShiftAndAppend(4, 0);
				Token token = state.token;
				int num = 3;
				if (eci < 0)
				{
					token = token.Add(0, 3);
				}
				else
				{
					int num2 = eci % 10;
					int num3 = eci / 10;
					if (num3 > 0)
					{
						token = token.Add(2, 3);
						token = token.Add(num3 + 2, 4);
						token = token.Add(num2 + 2, 4);
						num += 8;
					}
					else
					{
						token = token.Add(1, 3);
						token = token.Add(num2 + 2, 4);
						num += 4;
					}
				}
				return new State(token, mode, 0, bitCount + num);
			}

			public State LatchAndAppend(int mode, int value)
			{
				int num = bitCount;
				Token token = this.token;
				if (mode != this.mode)
				{
					int num2 = HighLevelEncoder.LATCH_TABLE[this.mode][mode];
					token = token.Add(num2 & 0xFFFF, num2 >> 16);
					num += num2 >> 16;
				}
				int num3 = ((mode == 2) ? 4 : 5);
				token = token.Add(value, num3);
				return new State(token, mode, 0, num + num3);
			}

			public State ShiftAndAppend(int mode, int value)
			{
				Token token = this.token;
				int num = ((this.mode == 2) ? 4 : 5);
				token = token.Add(HighLevelEncoder.SHIFT_TABLE[this.mode][mode], num);
				token = token.Add(value, 5);
				return new State(token, this.mode, 0, bitCount + num + 5);
			}

			public State AddBinaryShiftChar(int index)
			{
				Token token = this.token;
				int num = mode;
				int num2 = bitCount;
				if (mode == 4 || mode == 2)
				{
					int num3 = HighLevelEncoder.LATCH_TABLE[num][0];
					token = token.Add(num3 & 0xFFFF, num3 >> 16);
					num2 += num3 >> 16;
					num = 0;
				}
				int num4 = ((binaryShiftByteCount == 0 || binaryShiftByteCount == 31) ? 18 : ((binaryShiftByteCount == 62) ? 9 : 8));
				State state = new State(token, num, binaryShiftByteCount + 1, num2 + num4);
				if (state.binaryShiftByteCount == 2078)
				{
					state = state.EndBinaryShift(index + 1);
				}
				return state;
			}

			public State EndBinaryShift(int index)
			{
				if (binaryShiftByteCount == 0)
				{
					return this;
				}
				Token token = this.token;
				token = token.AddBinaryShift(index - binaryShiftByteCount, binaryShiftByteCount);
				return new State(token, mode, 0, bitCount);
			}

			public bool IsBetterThanOrEqualTo(State other)
			{
				int num = bitCount + (HighLevelEncoder.LATCH_TABLE[mode][other.mode] >> 16);
				if (binaryShiftByteCount < other.binaryShiftByteCount)
				{
					num += CalculateBinaryShiftCost(other) - CalculateBinaryShiftCost(this);
				}
				else if (binaryShiftByteCount > other.binaryShiftByteCount && other.binaryShiftByteCount > 0)
				{
					num += 10;
				}
				return num <= other.bitCount;
			}

			public BitArray ToBitArray(byte[] text)
			{
				LinkedList<Token> linkedList = new LinkedList<Token>();
				for (Token previous = EndBinaryShift(text.Length).token; previous != null; previous = previous.Previous)
				{
					linkedList.AddFirst(previous);
				}
				BitArray bitArray = new BitArray();
				foreach (Token item in linkedList)
				{
					item.AppendTo(bitArray, text);
				}
				return bitArray;
			}

			private static int CalculateBinaryShiftCost(State state)
			{
				if (state.binaryShiftByteCount > 62)
				{
					return 21;
				}
				if (state.binaryShiftByteCount > 31)
				{
					return 20;
				}
				if (state.binaryShiftByteCount > 0)
				{
					return 10;
				}
				return 0;
			}
		}

		private sealed class HighLevelEncoder
		{
			internal const int MODE_UPPER = 0;

			internal const int MODE_LOWER = 1;

			internal const int MODE_DIGIT = 2;

			internal const int MODE_MIXED = 3;

			internal const int MODE_PUNCT = 4;

			internal static readonly int[][] LATCH_TABLE;

			internal static readonly int[][] CHAR_MAP;

			internal static readonly int[][] SHIFT_TABLE;

			private readonly byte[] text;

			private readonly int eci;

			static HighLevelEncoder()
			{
				LATCH_TABLE = new int[5][]
				{
					new int[5] { 0, 327708, 327710, 327709, 656318 },
					new int[5] { 590318, 0, 327710, 327709, 656318 },
					new int[5] { 262158, 590300, 0, 590301, 932798 },
					new int[5] { 327709, 327708, 656318, 0, 327710 },
					new int[5] { 327711, 656380, 656382, 656381, 0 }
				};
				CHAR_MAP = new int[5][];
				SHIFT_TABLE = new int[6][];
				CHAR_MAP[0] = new int[256];
				CHAR_MAP[1] = new int[256];
				CHAR_MAP[2] = new int[256];
				CHAR_MAP[3] = new int[256];
				CHAR_MAP[4] = new int[256];
				SHIFT_TABLE[0] = new int[6];
				SHIFT_TABLE[1] = new int[6];
				SHIFT_TABLE[2] = new int[6];
				SHIFT_TABLE[3] = new int[6];
				SHIFT_TABLE[4] = new int[6];
				SHIFT_TABLE[5] = new int[6];
				CHAR_MAP[0][32] = 1;
				for (int i = 65; i <= 90; i++)
				{
					CHAR_MAP[0][i] = i - 65 + 2;
				}
				CHAR_MAP[1][32] = 1;
				for (int j = 97; j <= 122; j++)
				{
					CHAR_MAP[1][j] = j - 97 + 2;
				}
				CHAR_MAP[2][32] = 1;
				for (int k = 48; k <= 57; k++)
				{
					CHAR_MAP[2][k] = k - 48 + 2;
				}
				CHAR_MAP[2][44] = 12;
				CHAR_MAP[2][46] = 13;
				int[] array = new int[28]
				{
					0, 32, 1, 2, 3, 4, 5, 6, 7, 8,
					9, 10, 11, 12, 13, 27, 28, 29, 30, 31,
					64, 92, 94, 95, 96, 124, 126, 127
				};
				for (int l = 0; l < array.Length; l++)
				{
					CHAR_MAP[3][array[l]] = l;
				}
				int[] array2 = new int[31]
				{
					0, 13, 0, 0, 0, 0, 33, 39, 35, 36,
					37, 38, 39, 40, 41, 42, 43, 44, 45, 46,
					47, 58, 59, 60, 61, 62, 63, 91, 93, 123,
					125
				};
				for (int m = 0; m < array2.Length; m++)
				{
					if (array2[m] > 0)
					{
						CHAR_MAP[4][array2[m]] = m;
					}
				}
				int[][] sHIFT_TABLE = SHIFT_TABLE;
				foreach (int[] array3 in sHIFT_TABLE)
				{
					for (int num = 0; num < array3.Length; num++)
					{
						array3[num] = -1;
					}
				}
				SHIFT_TABLE[0][4] = 0;
				SHIFT_TABLE[1][4] = 0;
				SHIFT_TABLE[1][0] = 28;
				SHIFT_TABLE[3][4] = 0;
				SHIFT_TABLE[2][4] = 0;
				SHIFT_TABLE[2][0] = 15;
			}

			public HighLevelEncoder(byte[] text, int eci)
			{
				this.text = text;
				this.eci = eci;
			}

			public BitArray Encode()
			{
				State state = State.INITIAL_STATE;
				if (eci != 0)
				{
					state = state.AppendFLGn(eci);
				}
				ICollection<State> collection = new Collection<State>();
				collection.Add(state);
				for (int i = 0; i < text.Length; i++)
				{
					int num = ((i + 1 < text.Length) ? text[i + 1] : 0);
					int num2 = text[i] switch
					{
						13 => (num == 10) ? 2 : 0, 
						46 => (num == 32) ? 3 : 0, 
						44 => (num == 32) ? 4 : 0, 
						58 => (num == 32) ? 5 : 0, 
						_ => 0, 
					};
					if (num2 > 0)
					{
						collection = UpdateStateListForPair(collection, i, num2);
						i++;
					}
					else
					{
						collection = UpdateStateListForChar(collection, i);
					}
				}
				State state2 = null;
				foreach (State item in collection)
				{
					if (state2 == null)
					{
						state2 = item;
					}
					else if (item.BitCount < state2.BitCount)
					{
						state2 = item;
					}
				}
				return state2.ToBitArray(text);
			}

			private ICollection<State> UpdateStateListForChar(IEnumerable<State> states, int index)
			{
				LinkedList<State> linkedList = new LinkedList<State>();
				foreach (State state in states)
				{
					UpdateStateForChar(state, index, linkedList);
				}
				return SimplifyStates(linkedList);
			}

			private void UpdateStateForChar(State state, int index, ICollection<State> result)
			{
				char c = (char)(text[index] & 0xFFu);
				bool flag = CHAR_MAP[state.Mode][(uint)c] > 0;
				State state2 = null;
				for (int i = 0; i <= 4; i++)
				{
					int num = CHAR_MAP[i][(uint)c];
					if (num > 0)
					{
						if (state2 == null)
						{
							state2 = state.EndBinaryShift(index);
						}
						if (!flag || i == state.Mode || i == 2)
						{
							State item = state2.LatchAndAppend(i, num);
							result.Add(item);
						}
						if (!flag && SHIFT_TABLE[state.Mode][i] >= 0)
						{
							State item2 = state2.ShiftAndAppend(i, num);
							result.Add(item2);
						}
					}
				}
				if (state.BinaryShiftByteCount > 0 || CHAR_MAP[state.Mode][(uint)c] == 0)
				{
					State item3 = state.AddBinaryShiftChar(index);
					result.Add(item3);
				}
			}

			private static ICollection<State> UpdateStateListForPair(IEnumerable<State> states, int index, int pairCode)
			{
				LinkedList<State> linkedList = new LinkedList<State>();
				foreach (State state in states)
				{
					UpdateStateForPair(state, index, pairCode, linkedList);
				}
				return SimplifyStates(linkedList);
			}

			private static void UpdateStateForPair(State state, int index, int pairCode, ICollection<State> result)
			{
				State state2 = state.EndBinaryShift(index);
				result.Add(state2.LatchAndAppend(4, pairCode));
				if (state.Mode != 4)
				{
					result.Add(state2.ShiftAndAppend(4, pairCode));
				}
				if (pairCode == 3 || pairCode == 4)
				{
					State item = state2.LatchAndAppend(2, 16 - pairCode).LatchAndAppend(2, 1);
					result.Add(item);
				}
				if (state.BinaryShiftByteCount > 0)
				{
					State item2 = state.AddBinaryShiftChar(index).AddBinaryShiftChar(index + 1);
					result.Add(item2);
				}
			}

			private static ICollection<State> SimplifyStates(IEnumerable<State> states)
			{
				LinkedList<State> linkedList = new LinkedList<State>();
				List<State> list = new List<State>();
				foreach (State state in states)
				{
					bool flag = true;
					list.Clear();
					foreach (State item in linkedList)
					{
						if (item.IsBetterThanOrEqualTo(state))
						{
							flag = false;
							break;
						}
						if (state.IsBetterThanOrEqualTo(item))
						{
							list.Add(item);
						}
					}
					if (flag)
					{
						linkedList.AddLast(state);
					}
					foreach (State item2 in list)
					{
						linkedList.Remove(item2);
					}
				}
				return linkedList;
			}
		}

		internal const int Default_EC_Percent = 33;

		internal const StiAztecSize Default_Aztec_Layers = StiAztecSize.Automatic;

		internal const int Default_Aztec_CodePage = 28591;

		public static AztecCode Encode(string contents, int eccPercent = 33, StiAztecSize layers = StiAztecSize.Automatic, int codePage = 28591, bool useEci = false)
		{
			eccPercent = Math.Max(25, Math.Min(eccPercent, 95));
			Encoding encoding = Encoding.GetEncoding(28591);
			int eci = 0;
			byte[] bytes = encoding.GetBytes(contents);
			return Encoder.Encode(bytes, eci, eccPercent, layers);
		}
	}

	private float module = 40f;

	private float height = 1f;

	private int errorCorrectionLevel = 33;

	private int codePage = 28591;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiAztecBarCodeType;

	public override string ServiceName => "Aztec";

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
			if (value < 5f)
			{
				module = 5f;
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
	[DefaultValue(33)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public int ErrorCorrectionLevel
	{
		get
		{
			return errorCorrectionLevel;
		}
		set
		{
			errorCorrectionLevel = value;
			if (value < 10)
			{
				errorCorrectionLevel = 10;
			}
			if (value > 95)
			{
				errorCorrectionLevel = 95;
			}
		}
	}

	[DefaultValue(StiAztecSize.Automatic)]
	[StiSerializable]
	[Description("Gets or sets matrix size.")]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public StiAztecSize MatrixSize { get; set; }

	[Description("Gets or sets the default codepage.")]
	[DefaultValue(28591)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public int CodePage
	{
		get
		{
			return codePage;
		}
		set
		{
			try
			{
				Encoding encoding = Encoding.GetEncoding(value);
				if (encoding != null)
				{
					codePage = value;
				}
			}
			catch
			{
			}
		}
	}

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
		JsonExtensions.AddPropertyInt(val, "ErrorCorrectionLevel", ErrorCorrectionLevel, 33);
		JsonExtensions.AddPropertyEnum(val, "MatrixSize", (Enum)MatrixSize, (Enum)StiAztecSize.Automatic);
		JsonExtensions.AddPropertyInt(val, "CodePage", CodePage, 33);
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
				ErrorCorrectionLevel = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "MatrixSize":
				MatrixSize = StiJsonReportObjectHelper.DeserializeEnum<StiAztecSize>(item);
				break;
			case "CodePage":
				CodePage = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[3]
		{
			propertiesHelper.ErrorCorrectionLevel(),
			propertiesHelper.MatrixSize(),
			propertiesHelper.Module()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public override void Draw(object context, StiBarCode barCode, RectangleF rect, float zoom)
	{
		string code = GetCode(barCode);
		base.BarCodeData.Code = code;
		try
		{
			StiBarcodeUtils.BitMatrix bitMatrix = new StiBarcodeUtils.BitMatrix(1, 1);
			if (!(context is StiBarCodeExportPainter) || !(context as StiBarCodeExportPainter).OnlyAssembleData)
			{
				bitMatrix = StiAztec.Encode(code, ErrorCorrectionLevel, MatrixSize, CodePage).Matrix;
			}
			byte[] array = new byte[bitMatrix.Width * bitMatrix.Height];
			for (int i = 0; i < bitMatrix.Height; i++)
			{
				int num = i * bitMatrix.Width;
				for (int j = 0; j < bitMatrix.Width; j++)
				{
					array[num + j] = (byte)(bitMatrix[j, i] ? 1 : 0);
				}
			}
			base.BarCodeData.MatrixGrid = array;
			base.BarCodeData.MatrixWidth = bitMatrix.Width;
			base.BarCodeData.MatrixHeight = bitMatrix.Height;
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
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Expected O, but got Unknown
		StiBarCode stiBarCode = new StiBarCode();
		stiBarCode.BarCodeType = new StiAztecBarCodeType(10f, 33, StiAztecSize.Automatic, 28591);
		StiBarCodeData stiBarCodeData = (stiBarCode.BarCodeType as StiAztecBarCodeType).BarCodeData;
		stiBarCodeData.Code = code;
		Image val = null;
		try
		{
			StiBarcodeUtils.BitMatrix matrix = StiAztec.Encode(code).Matrix;
			byte[] array = new byte[matrix.Width * matrix.Height];
			for (int i = 0; i < matrix.Height; i++)
			{
				int num = i * matrix.Width;
				for (int j = 0; j < matrix.Width; j++)
				{
					array[num + j] = (byte)(matrix[j, i] ? 1 : 0);
				}
			}
			stiBarCodeData.MatrixGrid = array;
			stiBarCodeData.MatrixWidth = matrix.Width;
			stiBarCodeData.MatrixHeight = matrix.Height;
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
				(stiBarCode.BarCodeType as StiAztecBarCodeType).Draw2DBarCode(val2, new Rectangle(0, 0, num3, num4), stiBarCode, zoom);
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
		return new StiAztecBarCodeType();
	}

	public StiAztecBarCodeType()
		: this(40f, 33, StiAztecSize.Automatic, 28591)
	{
	}

	public StiAztecBarCodeType(float module, int errorCorrectionLevel, StiAztecSize matrixSize, int codePage)
	{
		this.module = module;
		ErrorCorrectionLevel = errorCorrectionLevel;
		MatrixSize = matrixSize;
		CodePage = codePage;
	}
}
