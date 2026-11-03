using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.AssistanceClasses;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.DalInterfaces;
using PacketParser.Entities;
using SharpCompress.Archives;
using SharpCompress.Archives.GZip;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Common;
using SharpCompress.Common.GZip;
using SharpCompress.Common.SevenZip;
using SharpCompress.Readers;
using SharpCompress.Writers;

namespace BackUpDLL.Zip;

public class GZip
{
	private class NestedCompressFile
	{
		internal static readonly GZip instance;

		static NestedCompressFile()
		{
			instance = new GZip();
		}
	}

	public static GZip CompressFileInstance => NestedCompressFile.instance;

	public event EventHandler<CompressedBytesReadEventArgs> OnCompressedBytesRead;

	public async Task<ReturnedSaveFuncInfoWithValue<string>> CompressFileAsync(ISettingRepo repo, string pathForZipDirectory, string zipFilePath, BackUpMethod backupMode, CancellationToken token, string refrefrenceGuid, string connectionString)
	{
		ReturnedSaveFuncInfoWithValue<string> ret = new ReturnedSaveFuncInfoWithValue<string>();
		int line = 0;
		try
		{
			line = 1;
			zipFilePath = zipFilePath.ToLower();
			line = 2;
			FileInfo fileInfo = new FileInfo(zipFilePath);
			line = 3;
			zipFilePath = fileInfo.FullName.Replace(fileInfo.Extension, ".npz2");
			EventLogger.NewInstance(EventLoggerType.Backup, "شروع فرایند فشرده سازی با متد:GZip", ReturnedState.Success, "", refrefrenceGuid?.ToString());
			IWritableArchive val = ArchiveFactory.Create((ArchiveType)4);
			try
			{
				line = 48;
				((IArchive)val).CompressedBytesRead -= Archive_CompressedBytesRead;
				line = 49;
				((IArchive)val).CompressedBytesRead += Archive_CompressedBytesRead;
				line = 50;
				IWritableArchiveExtensions.AddAllFromDirectory(val, pathForZipDirectory, "*.*", SearchOption.AllDirectories);
				line = 510;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "CompressFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\Zip\\GZip.cs", 63);
				}
				line = 511;
				IWritableArchiveExtensions.SaveTo(val, zipFilePath, WriterOptions.op_Implicit((CompressionType)1));
				line = 512;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			line = 513;
			EventLogger.NewInstance(EventLoggerType.Backup, "پایان فرایند فشرده سازی با متد:GZip", ReturnedState.Success, "", refrefrenceGuid?.ToString());
			line = 514;
			await SqlBackUpLogMaster.SaveAsync(repo, EnBackUpStatus.Success, "فشرده سازی GZip", "نسخه پشتیبان با استفاده از متد GZip فشرده شد", zipFilePath, connectionString);
			line = 515;
			line = 52;
			token.ThrowIfCancellationRequested();
			line = 53;
			ret.value = zipFilePath;
			Directory.Delete(pathForZipDirectory, recursive: true);
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "CompressFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\Zip\\GZip.cs", 82);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "CompressFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\Zip\\GZip.cs", 85);
			ret.AddReturnedValue(ex, "", "CompressFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\Zip\\GZip.cs", 86);
			EventLogger.NewInstance(EventLoggerType.Backup, "خطا در فشرده سازی", ReturnedState.Success, "", description: ex.Message, referencedId: refrefrenceGuid?.ToString());
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<string>> ExtractTempDIR(string archiveName, string startup)
	{
		int num = 1;
		ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<string>();
		string text = "";
		try
		{
			num = 2;
			text = MasterZip.TempDirName(startup);
			num = 3;
			FileInfo fileInfo = new FileInfo(archiveName);
			num = 4;
			text = Path.Combine(text, fileInfo.Name);
			num = 5;
			text = Path.ChangeExtension(text, ".bak");
			num = 6;
			try
			{
				GZipArchive val = GZipArchive.Open(archiveName, (ReaderOptions)null);
				try
				{
					num = 7;
					foreach (GZipArchiveEntry item in ((AbstractArchive<GZipArchiveEntry, GZipVolume>)(object)val).Entries.Where((GZipArchiveEntry entry) => !((Entry)entry).IsDirectory))
					{
						num = 8;
						IArchiveEntryExtensions.WriteToFile((IArchiveEntry)(object)item, text, new ExtractionOptions
						{
							ExtractFullPath = true,
							Overwrite = true
						});
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			catch (Exception ex2)
			{
				try
				{
					SevenZipArchive val2 = SevenZipArchive.Open(archiveName, new ReaderOptions
					{
						Password = "@NoViN 09158980915_+"
					});
					try
					{
						num = 7;
						foreach (SevenZipArchiveEntry item2 in ((AbstractArchive<SevenZipArchiveEntry, SevenZipVolume>)(object)val2).Entries.Where((SevenZipArchiveEntry entry) => !((Entry)entry).IsDirectory))
						{
							num = 8;
							IArchiveEntryExtensions.WriteToFile((IArchiveEntry)(object)item2, text, new ExtractionOptions
							{
								ExtractFullPath = true,
								Overwrite = true
							});
						}
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "ExtractTempDIR", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\Zip\\GZip.cs", 145);
					WebErrorLog.Instance.StartLog(ex2, "", "ExtractTempDIR", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\Zip\\GZip.cs", 146);
					returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "ExtractTempDIR", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\Zip\\GZip.cs", 147);
					returnedSaveFuncInfoWithValue.AddReturnedValue(ex2, "", "ExtractTempDIR", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\Zip\\GZip.cs", 148);
				}
			}
			num = 9;
			returnedSaveFuncInfoWithValue.value = text;
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, $"PathTemp:{text} \r\n ArchiveName:{archiveName} \r\n Line:{num}", "ExtractTempDIR", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\Zip\\GZip.cs", 156);
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex3, "", "ExtractTempDIR", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\Zip\\GZip.cs", 158);
		}
		return returnedSaveFuncInfoWithValue;
	}

	private static void Archive_CompressedBytesRead(object sender, CompressedBytesReadEventArgs e)
	{
		try
		{
			CompressFileInstance.OnCompressedBytesRead?.Invoke(null, e);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Archive_CompressedBytesRead", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\Zip\\GZip.cs", 171);
		}
	}
}
