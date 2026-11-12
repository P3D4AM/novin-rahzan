using System;
using System.IO;
using System.Security.Cryptography;
using ExcelDataReader.Core.OfficeCrypto;
using ExcelDataReader.Exceptions;

namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsBiffStream : IDisposable
{
	public int BiffVersion { get; }

	public BIFFTYPE BiffType { get; }

	public int Size => (int)BaseStream.Length;

	public int Position
	{
		get
		{
			return (int)BaseStream.Position;
		}
		set
		{
			Seek(value, SeekOrigin.Begin);
		}
	}

	public Stream BaseStream { get; }

	public byte[] SecretKey { get; }

	public EncryptionInfo Encryption { get; }

	public SymmetricAlgorithm Cipher { get; }

	public ICryptoTransform CipherTransform { get; set; }

	public int CipherBlock { get; set; }

	public XlsBiffStream(Stream baseStream, int offset = 0, int explicitVersion = 0, string password = null, byte[] secretKey = null, EncryptionInfo encryption = null)
	{
		BaseStream = baseStream;
		Position = offset;
		if (Read() is XlsBiffBOF xlsBiffBOF)
		{
			BiffVersion = ((explicitVersion == 0) ? GetBiffVersion(xlsBiffBOF) : explicitVersion);
			BiffType = xlsBiffBOF.Type;
		}
		CipherBlock = -1;
		if (secretKey != null)
		{
			SecretKey = secretKey;
			Encryption = encryption;
			Cipher = Encryption.CreateCipher();
		}
		else
		{
			XlsBiffFilePass xlsBiffFilePass = Read() as XlsBiffFilePass;
			if (xlsBiffFilePass == null)
			{
				xlsBiffFilePass = Read() as XlsBiffFilePass;
			}
			if (xlsBiffFilePass != null)
			{
				Encryption = xlsBiffFilePass.EncryptionInfo;
				if (Encryption.VerifyPassword("VelvetSweatshop"))
				{
					password = "VelvetSweatshop";
				}
				else if (password == null || !Encryption.VerifyPassword(password))
				{
					throw new InvalidPasswordException("Invalid password.");
				}
				SecretKey = Encryption.GenerateSecretKey(password);
				Cipher = Encryption.CreateCipher();
			}
		}
		Position = offset;
	}

	public void Seek(int offset, SeekOrigin origin)
	{
		BaseStream.Seek(offset, origin);
		if (Position < 0)
		{
			throw new ArgumentOutOfRangeException(string.Format("{0} On offset={1}", "BIFF Stream error: Moving before stream start.", offset));
		}
		if (Position > Size)
		{
			throw new ArgumentOutOfRangeException(string.Format("{0} On offset={1}", "BIFF Stream error: Moving after stream end.", offset));
		}
		if (SecretKey != null)
		{
			CreateBlockDecryptor(offset / 1024);
			AlignBlockDecryptor(offset % 1024);
		}
	}

	public XlsBiffRecord Read()
	{
		if ((uint)(Position + 4) >= Size)
		{
			return null;
		}
		XlsBiffRecord result = GetRecord(BaseStream);
		if (Position > Size)
		{
			result = null;
		}
		return result;
	}

	public XlsBiffRecord GetRecord(Stream stream)
	{
		int startPosition = (int)stream.Position;
		byte[] array = new byte[4];
		stream.Read(array, 0, 4);
		BIFFRECORDTYPE bIFFRECORDTYPE = (BIFFRECORDTYPE)BitConverter.ToUInt16(array, 0);
		int num = BitConverter.ToUInt16(array, 2);
		byte[] array2 = new byte[4 + num];
		Array.Copy(array, array2, 4);
		stream.Read(array2, 4, num);
		if (SecretKey != null)
		{
			DecryptRecord(startPosition, bIFFRECORDTYPE, array2);
		}
		uint offset = 0u;
		int biffVersion = BiffVersion;
		switch (bIFFRECORDTYPE)
		{
		case BIFFRECORDTYPE.BOF_V2:
		case BIFFRECORDTYPE.BOF_V3:
		case BIFFRECORDTYPE.BOF_V4:
		case BIFFRECORDTYPE.BOF:
			return new XlsBiffBOF(array2, offset);
		case BIFFRECORDTYPE.EOF:
			return new XlsBiffEof(array2, offset);
		case BIFFRECORDTYPE.INTERFACEHDR:
			return new XlsBiffInterfaceHdr(array2, offset);
		case BIFFRECORDTYPE.SST:
			return new XlsBiffSST(array2, offset);
		case BIFFRECORDTYPE.INDEX:
			return new XlsBiffIndex(array2, offset, biffVersion == 8);
		case BIFFRECORDTYPE.DEFAULTROWHEIGHT_V2:
		case BIFFRECORDTYPE.DEFAULTROWHEIGHT:
			return new XlsBiffDefaultRowHeight(array2, offset, biffVersion);
		case BIFFRECORDTYPE.ROW_V2:
		case BIFFRECORDTYPE.ROW:
			return new XlsBiffRow(array2, offset);
		case BIFFRECORDTYPE.DBCELL:
			return new XlsBiffDbCell(array2, offset);
		case BIFFRECORDTYPE.BLANK_OLD:
		case BIFFRECORDTYPE.BOOLERR_OLD:
		case BIFFRECORDTYPE.BLANK:
		case BIFFRECORDTYPE.BOOLERR:
			return new XlsBiffBlankCell(array2, offset, biffVersion);
		case BIFFRECORDTYPE.MULBLANK:
			return new XlsBiffMulBlankCell(array2, offset, biffVersion);
		case BIFFRECORDTYPE.LABEL_OLD:
		case BIFFRECORDTYPE.RSTRING:
		case BIFFRECORDTYPE.LABEL:
			return new XlsBiffLabelCell(array2, offset, biffVersion);
		case BIFFRECORDTYPE.LABELSST:
			return new XlsBiffLabelSSTCell(array2, offset, biffVersion);
		case BIFFRECORDTYPE.INTEGER_OLD:
		case BIFFRECORDTYPE.INTEGER:
			return new XlsBiffIntegerCell(array2, offset, biffVersion);
		case BIFFRECORDTYPE.NUMBER_OLD:
		case BIFFRECORDTYPE.NUMBER:
			return new XlsBiffNumberCell(array2, offset, biffVersion);
		case BIFFRECORDTYPE.RK:
			return new XlsBiffRKCell(array2, offset, biffVersion);
		case BIFFRECORDTYPE.MULRK:
			return new XlsBiffMulRKCell(array2, offset, biffVersion);
		case BIFFRECORDTYPE.FORMULA:
		case BIFFRECORDTYPE.FORMULA_V3:
		case BIFFRECORDTYPE.FORMULA_V4:
			return new XlsBiffFormulaCell(array2, offset, biffVersion);
		case BIFFRECORDTYPE.FORMAT_V23:
		case BIFFRECORDTYPE.FORMAT:
			return new XlsBiffFormatString(array2, offset, biffVersion);
		case BIFFRECORDTYPE.STRING_OLD:
		case BIFFRECORDTYPE.STRING:
			return new XlsBiffFormulaString(array2, offset, biffVersion);
		case BIFFRECORDTYPE.CONTINUE:
			return new XlsBiffContinue(array2, offset);
		case BIFFRECORDTYPE.DIMENSIONS_V2:
		case BIFFRECORDTYPE.DIMENSIONS:
			return new XlsBiffDimensions(array2, offset, biffVersion);
		case BIFFRECORDTYPE.BOUNDSHEET:
			return new XlsBiffBoundSheet(array2, offset, biffVersion);
		case BIFFRECORDTYPE.WINDOW1:
			return new XlsBiffWindow1(array2, offset);
		case BIFFRECORDTYPE.CODEPAGE:
			return new XlsBiffSimpleValueRecord(array2, offset);
		case BIFFRECORDTYPE.FNGROUPCOUNT:
			return new XlsBiffSimpleValueRecord(array2, offset);
		case BIFFRECORDTYPE.RECORD1904:
			return new XlsBiffSimpleValueRecord(array2, offset);
		case BIFFRECORDTYPE.BOOKBOOL:
			return new XlsBiffSimpleValueRecord(array2, offset);
		case BIFFRECORDTYPE.BACKUP:
			return new XlsBiffSimpleValueRecord(array2, offset);
		case BIFFRECORDTYPE.HIDEOBJ:
			return new XlsBiffSimpleValueRecord(array2, offset);
		case BIFFRECORDTYPE.USESELFS:
			return new XlsBiffSimpleValueRecord(array2, offset);
		case BIFFRECORDTYPE.UNCALCED:
			return new XlsBiffUncalced(array2, offset);
		case BIFFRECORDTYPE.QUICKTIP:
			return new XlsBiffQuickTip(array2, offset);
		case BIFFRECORDTYPE.MSODRAWING:
			return new XlsBiffMSODrawing(array2, offset);
		case BIFFRECORDTYPE.FILEPASS:
			return new XlsBiffFilePass(array2, offset, biffVersion);
		case BIFFRECORDTYPE.HEADER:
		case BIFFRECORDTYPE.FOOTER:
			return new XlsBiffHeaderFooterString(array2, offset, biffVersion);
		case BIFFRECORDTYPE.CODENAME:
			return new XlsBiffCodeName(array2, offset);
		case BIFFRECORDTYPE.XF_V2:
		case BIFFRECORDTYPE.XF:
		case BIFFRECORDTYPE.XF_V3:
		case BIFFRECORDTYPE.XF_V4:
			return new XlsBiffXF(array2, offset);
		case BIFFRECORDTYPE.MERGECELLS:
			return new XlsBiffMergeCells(array2, offset);
		case BIFFRECORDTYPE.COLINFO:
			return new XlsBiffColInfo(array2, offset);
		default:
			return new XlsBiffRecord(array2, offset);
		}
	}

	public void Dispose()
	{
		CipherTransform?.Dispose();
		((IDisposable)Cipher)?.Dispose();
	}

	private int GetBiffVersion(XlsBiffBOF bof)
	{
		switch (bof.Id)
		{
		case BIFFRECORDTYPE.BOF_V2:
			return 2;
		case BIFFRECORDTYPE.BOF_V3:
			return 3;
		case BIFFRECORDTYPE.BOF_V4:
			return 4;
		case BIFFRECORDTYPE.BOF:
			if (bof.Version == 512)
			{
				return 2;
			}
			if (bof.Version == 768)
			{
				return 3;
			}
			if (bof.Version == 1024)
			{
				return 4;
			}
			if (bof.Version == 1280 || bof.Version == 0)
			{
				return 5;
			}
			if (bof.Version == 1536)
			{
				return 8;
			}
			break;
		}
		return 0;
	}

	private void CreateBlockDecryptor(int blockNumber)
	{
		CipherTransform?.Dispose();
		byte[] rgbKey = Encryption.GenerateBlockKey(blockNumber, SecretKey);
		CipherTransform = Cipher.CreateDecryptor(rgbKey, null);
		CipherBlock = blockNumber;
	}

	private void AlignBlockDecryptor(int blockOffset)
	{
		byte[] bytes = new byte[blockOffset];
		CryptoHelpers.DecryptBytes(CipherTransform, bytes);
	}

	private void DecryptRecord(int startPosition, BIFFRECORDTYPE id, byte[] bytes)
	{
		int num = 4;
		int num2 = bytes.Length;
		switch (id)
		{
		case BIFFRECORDTYPE.FILEPASS:
		case BIFFRECORDTYPE.INTERFACEHDR:
		case BIFFRECORDTYPE.BOF:
			num = num2;
			break;
		case BIFFRECORDTYPE.BOUNDSHEET:
			num += 4;
			break;
		}
		int num3 = 0;
		while (num3 < num2)
		{
			int num4 = startPosition + num3;
			int num5 = num4 / 1024;
			int num6 = num4 % 1024;
			if (num5 != CipherBlock)
			{
				CreateBlockDecryptor(num5);
			}
			if (Encryption.IsXor)
			{
				((XorManaged.XorTransform)CipherTransform).XorArrayIndex = num4 + num2 - 4;
			}
			int num7 = Math.Min(num2 - num3, 1024 - num6);
			byte[] array = new byte[num7];
			Array.Copy(bytes, num3, array, 0, num7);
			byte[] array2 = CryptoHelpers.DecryptBytes(CipherTransform, array);
			for (int i = 0; i < array2.Length; i++)
			{
				if (num3 >= num)
				{
					bytes[num3] = array2[i];
				}
				num3++;
			}
		}
	}
}
