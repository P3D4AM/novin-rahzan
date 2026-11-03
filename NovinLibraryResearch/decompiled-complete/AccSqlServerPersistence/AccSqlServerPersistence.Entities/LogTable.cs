using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_Log")]
public class LogTable : Serializable<ILog>, ILog, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	private bool _isDisposed;

	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public DateTime DateM { get; set; }

	[Required]
	public string Message { get; set; }

	[Required]
	public string UserDescription { get; set; }

	public LogPartEnum LogPart { get; set; }

	public string? SerializedData { get; set; }

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

	public override void Dispose()
	{
		Dispose(disposing: true);
	}

	public override void Dispose(bool disposing)
	{
		if (!_isDisposed)
		{
			_isDisposed = true;
			base.Dispose();
		}
	}

	public T FromString<T>(string data)
	{
		using (new JaegerMonitor("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Entities\\LogTable.cs"))
		{
			try
			{
				if (string.IsNullOrEmpty(data))
				{
					return default(T);
				}
				return data.FromJson<T>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Entities\\LogTable.cs", 36);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Entities\\LogTable.cs", 40);
				return default(T);
			}
		}
	}
}
