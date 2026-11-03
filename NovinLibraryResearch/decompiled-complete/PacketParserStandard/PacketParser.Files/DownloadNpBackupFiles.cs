using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Interfaces;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Files;

public class DownloadNpBackupFiles : INpDisposable, IDisposable
{
	private bool _isDisposed;

	private IReturnedSaveFuncInfo _iReturnedSaveFuncInfo;

	private string UserName { get; set; }

	private string Url { get; set; }

	private string Password { get; set; }

	public FileTransfer FileTransferObject { get; set; }

	public CancellationToken Token { get; set; }

	private short StartHour { get; set; }

	private short EndHour { get; set; } = 24;


	private long MaxCapcityFileNeeds { get; set; } = 1073741824L;


	public string DownLoadAddress { get; private set; } = "D:\\DownLoadBackup";


	public bool IsDisposed => _isDisposed;

	public DownloadNpBackupFiles(string userName, string passord, string url, IReturnedSaveFuncInfo iReturnedSaveFuncInfo)
	{
		_iReturnedSaveFuncInfo = iReturnedSaveFuncInfo;
		UserName = userName;
		Password = passord;
		Url = url;
	}

	public DownloadNpBackupFiles(string userName, string passord, string url, string path, long Capcity, CancellationToken token, short startTime, short endtime, IReturnedSaveFuncInfo iReturnedSaveFuncInfo)
	{
		try
		{
			_iReturnedSaveFuncInfo = iReturnedSaveFuncInfo;
			UserName = userName;
			Password = passord;
			Url = url;
			DownLoadAddress = path;
			StartHour = startTime;
			EndHour = endtime;
			MaxCapcityFileNeeds = Capcity;
			Token = token;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Files\\DownloadNpBackupFiles.cs", 48);
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	public void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				FileTransferObject?.Dispose();
				FileTransferObject = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Files\\DownloadNpBackupFiles.cs", 66);
		}
	}

	public void Start()
	{
		Task.Run(() => Start_());
	}

	private async Task<ReturnedSaveFuncInfo> Start_()
	{
		DateTime runAtStart = DateTime.Today.AddHours(StartHour);
		DateTime runAtEnd = DateTime.Today.AddHours(EndHour);
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo(_iReturnedSaveFuncInfo);
		try
		{
			while (!IsDisposed && !Token.IsCancellationRequested)
			{
				if (runAtStart <= DateTime.Now && runAtEnd > DateTime.Now)
				{
					FileTransferObject = new FileTransfer(UserName, Password, Url, _iReturnedSaveFuncInfo, DownLoadAddress);
					ret = await FileTransferObject.BackUpDeleteOnServer(MaxCapcityFileNeeds, Token, 5);
				}
				await Task.Delay(300000, Token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Start_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Files\\DownloadNpBackupFiles.cs", 91);
			ret.AddReturnedValue(ex, "", "Start_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Files\\DownloadNpBackupFiles.cs", 92);
		}
		return ret;
	}
}
