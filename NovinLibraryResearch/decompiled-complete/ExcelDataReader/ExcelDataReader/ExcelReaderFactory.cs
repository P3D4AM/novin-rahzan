using System.IO;
using ExcelDataReader.Core.BinaryFormat;
using ExcelDataReader.Core.CompoundFormat;
using ExcelDataReader.Core.OfficeCrypto;
using ExcelDataReader.Exceptions;
using ExcelDataReader.Misc;

namespace ExcelDataReader;

public static class ExcelReaderFactory
{
	private const string DirectoryEntryWorkbook = "Workbook";

	private const string DirectoryEntryBook = "Book";

	private const string DirectoryEntryEncryptedPackage = "EncryptedPackage";

	private const string DirectoryEntryEncryptionInfo = "EncryptionInfo";

	public static IExcelDataReader CreateReader(Stream fileStream, ExcelReaderConfiguration configuration = null)
	{
		if (configuration == null)
		{
			configuration = new ExcelReaderConfiguration();
		}
		if (configuration.LeaveOpen)
		{
			fileStream = new LeaveOpenStream(fileStream);
		}
		byte[] array = new byte[8];
		fileStream.Seek(0L, SeekOrigin.Begin);
		fileStream.Read(array, 0, array.Length);
		fileStream.Seek(0L, SeekOrigin.Begin);
		if (CompoundDocument.IsCompoundDocument(array))
		{
			CompoundDocument document = new CompoundDocument(fileStream);
			if (TryGetWorkbook(fileStream, document, out var stream))
			{
				return new ExcelBinaryReader(stream, configuration.Password, configuration.FallbackEncoding);
			}
			if (TryGetEncryptedPackage(fileStream, document, configuration.Password, out stream))
			{
				return new ExcelOpenXmlReader(stream);
			}
			throw new ExcelReaderException("Neither stream 'Workbook' nor 'Book' was found in file.");
		}
		if (XlsWorkbook.IsRawBiffStream(array))
		{
			return new ExcelBinaryReader(fileStream, configuration.Password, configuration.FallbackEncoding);
		}
		if (array[0] == 80 && array[1] == 75)
		{
			return new ExcelOpenXmlReader(fileStream);
		}
		throw new HeaderException("Invalid file signature.");
	}

	public static IExcelDataReader CreateBinaryReader(Stream fileStream, ExcelReaderConfiguration configuration = null)
	{
		if (configuration == null)
		{
			configuration = new ExcelReaderConfiguration();
		}
		if (configuration.LeaveOpen)
		{
			fileStream = new LeaveOpenStream(fileStream);
		}
		byte[] array = new byte[8];
		fileStream.Seek(0L, SeekOrigin.Begin);
		fileStream.Read(array, 0, array.Length);
		fileStream.Seek(0L, SeekOrigin.Begin);
		if (CompoundDocument.IsCompoundDocument(array))
		{
			CompoundDocument document = new CompoundDocument(fileStream);
			if (TryGetWorkbook(fileStream, document, out var stream))
			{
				return new ExcelBinaryReader(stream, configuration.Password, configuration.FallbackEncoding);
			}
			throw new ExcelReaderException("Neither stream 'Workbook' nor 'Book' was found in file.");
		}
		if (XlsWorkbook.IsRawBiffStream(array))
		{
			return new ExcelBinaryReader(fileStream, configuration.Password, configuration.FallbackEncoding);
		}
		throw new HeaderException("Invalid file signature.");
	}

	public static IExcelDataReader CreateOpenXmlReader(Stream fileStream, ExcelReaderConfiguration configuration = null)
	{
		if (configuration == null)
		{
			configuration = new ExcelReaderConfiguration();
		}
		if (configuration.LeaveOpen)
		{
			fileStream = new LeaveOpenStream(fileStream);
		}
		byte[] array = new byte[8];
		fileStream.Seek(0L, SeekOrigin.Begin);
		fileStream.Read(array, 0, array.Length);
		fileStream.Seek(0L, SeekOrigin.Begin);
		if (CompoundDocument.IsCompoundDocument(array))
		{
			CompoundDocument document = new CompoundDocument(fileStream);
			if (TryGetEncryptedPackage(fileStream, document, configuration.Password, out var stream))
			{
				return new ExcelOpenXmlReader(stream);
			}
			throw new ExcelReaderException("Detected compound document, but not a valid OpenXml file.");
		}
		if (array[0] == 80 && array[1] == 75)
		{
			return new ExcelOpenXmlReader(fileStream);
		}
		throw new HeaderException("Invalid file signature.");
	}

	public static IExcelDataReader CreateCsvReader(Stream fileStream, ExcelReaderConfiguration configuration = null)
	{
		if (configuration == null)
		{
			configuration = new ExcelReaderConfiguration();
		}
		if (configuration.LeaveOpen)
		{
			fileStream = new LeaveOpenStream(fileStream);
		}
		return new ExcelCsvReader(fileStream, configuration.FallbackEncoding, configuration.AutodetectSeparators, configuration.AnalyzeInitialCsvRows);
	}

	private static bool TryGetWorkbook(Stream fileStream, CompoundDocument document, out Stream stream)
	{
		CompoundDirectoryEntry compoundDirectoryEntry = document.FindEntry("Workbook") ?? document.FindEntry("Book");
		if (compoundDirectoryEntry != null)
		{
			if (compoundDirectoryEntry.EntryType != STGTY.STGTY_STREAM)
			{
				throw new ExcelReaderException("Workbook directory entry is not a Stream.");
			}
			stream = new CompoundStream(document, fileStream, compoundDirectoryEntry.StreamFirstSector, (int)compoundDirectoryEntry.StreamSize, compoundDirectoryEntry.IsEntryMiniStream, leaveOpen: false);
			return true;
		}
		stream = null;
		return false;
	}

	private static bool TryGetEncryptedPackage(Stream fileStream, CompoundDocument document, string password, out Stream stream)
	{
		CompoundDirectoryEntry compoundDirectoryEntry = document.FindEntry("EncryptedPackage");
		CompoundDirectoryEntry compoundDirectoryEntry2 = document.FindEntry("EncryptionInfo");
		if (compoundDirectoryEntry == null || compoundDirectoryEntry2 == null)
		{
			stream = null;
			return false;
		}
		EncryptionInfo encryptionInfo = EncryptionInfo.Create(document.ReadStream(fileStream, compoundDirectoryEntry2.StreamFirstSector, (int)compoundDirectoryEntry2.StreamSize, compoundDirectoryEntry2.IsEntryMiniStream));
		if (encryptionInfo.VerifyPassword("VelvetSweatshop"))
		{
			password = "VelvetSweatshop";
		}
		else if (password == null || !encryptionInfo.VerifyPassword(password))
		{
			throw new InvalidPasswordException("Invalid password.");
		}
		byte[] secretKey = encryptionInfo.GenerateSecretKey(password);
		CompoundStream stream2 = new CompoundStream(document, fileStream, compoundDirectoryEntry.StreamFirstSector, (int)compoundDirectoryEntry.StreamSize, compoundDirectoryEntry.IsEntryMiniStream, leaveOpen: false);
		stream = encryptionInfo.CreateEncryptedPackageStream(stream2, secretKey);
		return true;
	}
}
