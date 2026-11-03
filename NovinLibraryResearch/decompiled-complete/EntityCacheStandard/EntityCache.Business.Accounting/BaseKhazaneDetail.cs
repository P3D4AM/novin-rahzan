using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Core;
using EntityCache.Interfaces;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs.Sanad;

namespace EntityCache.Business.Accounting;

public abstract class BaseKhazaneDetail : Serializable<BaseKhazaneDetail>, IHasContext, INotifyPropertyChanged, IFileAttachment, IHasJustGuid
{
	private bool _isDisposed;

	private List<FileInfoBusiness> _addedFileList = new List<FileInfoBusiness>();

	private List<FileInfoBusiness> _removedFileList = new List<FileInfoBusiness>();

	public Guid Guid { get; set; } = Guid.NewGuid();


	public string ContextGuid { get; set; }

	public new bool IsDisposed
	{
		get
		{
			if (!base.IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	public decimal Price { get; protected set; }

	public abstract string DescriptionForUi { get; }

	public abstract KhazaneType Type { get; }

	public string TypeStr => Type.GetDisplay().GetTranslate();

	public virtual int Ordinal { get; private set; }

	public virtual DateTime DateM { get; set; } = DateManagment.DateM;


	public string Time => $"{DateM.Hour}:{DateM.Minute}";

	public Guid? MasterGuid { get; set; }

	public abstract FileManagement_Parts Part { get; }

	public IReadOnlyCollection<FileInfoBusiness> AddedFiles => _addedFileList;

	public IReadOnlyCollection<FileInfoBusiness> RemovedFiles => _removedFileList;

	public DateTime? DateUtc { get; set; }

	public event Func<object, EventArgs, Task> OnPriceChange;

	public event Func<object, EventArgs, Task> OnDisposed;

	public event PropertyChangedEventHandler PropertyChanged;

	public event Func<object, OrdinalChangedEventArgs, Task> OnOrdinalChanged;

	public override void Dispose()
	{
		Dispose(disposing: true);
	}

	public override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				if (this.OnDisposed != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnDisposed, this, new EventArgs(), default(CancellationToken), 60, "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BaseKhazaneDetail.cs");
				}
				this.OnPriceChange = null;
				this.OnDisposed = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BaseKhazaneDetail.cs", 54);
		}
	}

	public void SetOrdinal(EnSystem system, int value, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (!IsDisposed)
			{
				if (value < 0)
				{
					throw new ArgumentException($"value({value}) is not valid for BaseKhazaneDetail.Ordinal");
				}
				int ordinal = Ordinal;
				Ordinal = value;
				if (system == EnSystem.UserInterface && Ordinal >= 0 && this.OnOrdinalChanged != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnOrdinalChanged, this, new OrdinalChangedEventArgs(ordinal, value, Guid, system), token, 60, "SetOrdinal", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BaseKhazaneDetail.cs");
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetOrdinal", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BaseKhazaneDetail.cs", 68);
		}
	}

	public Task AddFileAsync(FileInfoBusiness file)
	{
		return BaseFileAttachmentBusiness.AddFileAsync(file, _addedFileList);
	}

	public Task RemoveFileAsync(FileInfoBusiness file)
	{
		return BaseFileAttachmentBusiness.RemoveFileAsync(file, _removedFileList);
	}

	public void SetPrice(EnSystem system, decimal value, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (!(Price == value))
			{
				Price = value;
				if (system != EnSystem.Mapper && this.OnPriceChange != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnPriceChange, this, new EventArgs(), token, 60, "SetPrice", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BaseKhazaneDetail.cs");
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetPrice", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\BaseKhazaneDetail.cs", 84);
			throw;
		}
	}

	public abstract Task<string> GetShortDescriptionAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "");
}
