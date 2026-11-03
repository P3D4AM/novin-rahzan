using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class WebPerfMonitor : System.EventArgs, IPerfMonitor, IDisposable
{
	private string className = "";

	private string funcName = "";

	public string ClassName
	{
		get
		{
			if ((className?.Length ?? 0) <= 200)
			{
				return className;
			}
			return className.Substring(0, 200);
		}
		set
		{
			className = value;
		}
	}

	public string FullName => ClassName + "." + FuncName;

	public string FuncName
	{
		get
		{
			if ((funcName?.Length ?? 0) <= 200)
			{
				return funcName;
			}
			return funcName.Substring(0, 200);
		}
		set
		{
			funcName = value;
		}
	}

	public DateTime Start { get; set; } = DateTime.Now;


	public int Duration { get; set; }

	public int Min { get; set; }

	public int Max { get; set; } = int.MaxValue;


	public long MinMemory { get; set; }

	public long MaxMemory { get; set; }

	public long MemoryUsage { get; set; }

	public string UserDescription { get; set; } = "";


	public int ExcuteCount { get; set; } = 1;


	protected virtual void Dispose(bool disposing)
	{
	}

	public virtual void Dispose()
	{
		Dispose(disposing: true);
	}
}
