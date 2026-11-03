using System;
using System.Threading;

namespace Riss.Devices;

public abstract class ZdCommBase
{
	private Device a;

	private int b;

	private double c;

	private static EventHandler d;

	public Device Device
	{
		get
		{
			return a;
		}
		set
		{
			a = value;
		}
	}

	public int CmdLen
	{
		get
		{
			return b;
		}
		set
		{
			b = value;
		}
	}

	public double CommunicationPercent
	{
		get
		{
			return c;
		}
		set
		{
			c = value;
			OnPercentChanged(this);
		}
	}

	public static event EventHandler PercentChanged
	{
		add
		{
			EventHandler eventHandler = d;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref d, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = d;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref d, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public ZdCommBase()
	{
	}

	public abstract void Close();

	public abstract bool Open();

	public abstract int SendData(ref byte[] buffer, int size, int timeout);

	public virtual int SendData(ref byte[] buffer, int size)
	{
		return SendData(ref buffer, size, 30000);
	}

	public abstract int RecvData(ref byte[] buffer, int size, int timeout);

	public virtual int RecvData(ref byte[] buffer, int size)
	{
		return RecvData(ref buffer, size, 30000);
	}

	public virtual bool WakeupMachine()
	{
		return false;
	}

	protected static void OnPercentChanged(ZdCommBase zdBase)
	{
		if (d != null)
		{
			d(zdBase, null);
		}
	}
}
