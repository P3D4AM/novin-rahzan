using System;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace mCore;

public class SMS : ISMS, IDisposable
{
	public delegate void NewMessageReceivedEventHandler(object sender, NewMessageReceivedEventArgs e);

	public delegate void NewDeliveryReportEventHandler(object sender, NewDeliveryReportEventArgs e);

	public delegate void NewIncomingCallEventHandler(object sender, NewIncomingCallEventArgs e);

	public delegate void NewUSSDReceivedEventhandler(object sender, NewUSSDReceivedEventArgs e);

	public delegate void QueueSMSSentEventHandler(object sender, QueueSMSSentEventArgs e);

	public delegate void QueueWapPushSentEventHandler(object sender, QueueWapPushSentEventArgs e);

	public delegate void QueueVCardSentEventHandler(object sender, QueueVCardSentEventArgs e);

	public delegate void QueueVCalendarSentEventHandler(object sender, QueueVCalendarSentEventArgs e);

	public delegate void QueueSmartMessageSentEventHandler(object sender, QueueSmartMessageSentEventArgs e);

	public delegate void QueueSMSSendingEventHandler(object sender, QueueSMSSendingEventArgs e);

	public delegate void QueueWapPushSendingEventHandler(object sender, QueueWapPushSendingEventArgs e);

	public delegate void QueueVCardSendingEventHandler(object sender, QueueVCardSendingEventArgs e);

	public delegate void QueueVCalendarSendingEventHandler(object sender, QueueVCalendarSendingEventArgs e);

	public delegate void QueueSmartMessageSendingEventHandler(object sender, QueueSmartMessageSendingEventArgs e);

	[Obsolete("mCore.SMS.EnumBaudRate enumerator was obsoleted in version 1.1. Use mCore.BaudRate instead", true)]
	public enum EnumBaudRate
	{
		BaudRate_110 = 110,
		BaudRate_300 = 300,
		BaudRate_1200 = 1200,
		BaudRate_2400 = 2400,
		BaudRate_4800 = 4800,
		BaudRate_9600 = 9600,
		BaudRate_14400 = 14400,
		BaudRate_19200 = 19200,
		BaudRate_38400 = 38400,
		BaudRate_57600 = 57600,
		BaudRate_115200 = 115200,
		BaudRate_230400 = 230400,
		BaudRate_460800 = 460800,
		BaudRate_921600 = 921600
	}

	[Obsolete("mCore.SMS.EnumParity enumerator was obsoleted in version 1.1. Use mCore.Parity instead", true)]
	public enum EnumParity
	{
		None,
		Odd,
		Even,
		Mark,
		Space
	}

	[Obsolete("mCore.SMS.EnumDataBits enumerator was obsoleted in version 1.1. Use mCore.DataBits instead", true)]
	public enum EnumDataBits
	{
		Four = 4,
		Five,
		Six,
		Seven,
		Eight
	}

	[Obsolete("mCore.SMS.EnumStopBits enumerator was obsoleted in version 1.1. Use mCore.StopBits instead", true)]
	public enum EnumStopBits
	{
		One = 1,
		Two,
		OnePointFive
	}

	[Obsolete("mCore.SMS.EnumFlowControl enumerator was obsoleted in version 1.1. Use mCore.FlowControl instead", true)]
	public enum EnumFlowControl
	{
		None,
		RTS_CTS,
		Xon_Xoff
	}

	[Obsolete("mCore.SMS.EnumLongMessage enumerator was obsoleted in version 1.1. Use mCore.LongMessage instead", true)]
	public enum EnumLongMessage
	{
		Truncate,
		SimpleSplit,
		FormattedSplit,
		Concatenate
	}

	[Obsolete("mCore.SMS.EnumEncoding enumerator was obsoleted in version 1.1. Use mCore.Encoding instead", true)]
	public enum EnumEncoding
	{
		GSM_Default_7Bit,
		ANSI_8Bit,
		Unicode_16Bit
	}

	[Obsolete("mCore.SMS.EnumLogType enumerator was obsoleted in version 1.1. Use mCore.LogType instead", true)]
	public enum EnumLogType
	{
		NoLog,
		ErrorLog,
		ErrorEventLog
	}

	[Obsolete("mCore.SMS.EnumMessageMemory enumerator was obsoleted in version 1.1. Use mCore.MessageMemory instead", true)]
	public enum EnumMessageMemory
	{
		SM,
		ME,
		MT
	}

	[Obsolete("mCore.SMS.EnumQueuePriority enumerator was obsoleted in version 1.1. Use mCore.QueuePriority instead", true)]
	public enum EnumQueuePriority
	{
		High = 1,
		Normal,
		Low
	}

	[Obsolete("mCore.SMS.EnumUSSDStatus enumerator was obsoleted in version 1.1. Use mCore.USSDStatus instead", true)]
	public enum EnumUSSDStatus
	{
		NoFurtherUserActionRequired,
		FurtherUserActionRequired,
		USSDTerminatedByNetwork,
		OtherLocalClientHasResponded,
		OperationNotSupported,
		NetworkTimeout
	}

	private class c
	{
		internal bool a;

		internal bool b;

		internal MessageMemory c;

		internal string d;

		internal int e;

		internal bool f;

		public c()
		{
			a = true;
			b = false;
			d = "";
			f = false;
		}
	}

	private class b
	{
		internal bool a;

		internal string b;

		internal string c;

		internal bool d;

		internal int e;

		internal int f;

		internal bool g;

		internal int h;

		internal string i;

		internal int j;

		public b(string A_0, string A_1, bool A_2, int A_3, int A_4, bool A_5, int A_6, int A_7 = 2)
		{
			a = true;
			b = "";
			c = "";
			d = false;
			i = "";
			b = A_0;
			c = A_1;
			d = A_2;
			e = A_3;
			f = A_4;
			g = A_5;
			h = A_6;
			j = A_7;
		}
	}

	private class a
	{
		internal bool a;

		internal string b;

		internal string c;

		internal bool d;

		internal int e;

		internal string f;

		internal int g;

		internal int h;

		internal int i;

		public a(string A_0, string A_1, bool A_2, int A_3, int A_4, int A_5, int A_6 = 2)
		{
			a = true;
			b = "";
			c = "";
			f = "";
			g = 0;
			h = 0;
			b = A_0;
			c = A_1;
			d = A_2;
			e = A_3;
			g = A_4;
			h = A_5;
			i = A_6;
		}
	}

	private class d
	{
		internal bool a;

		internal string b;

		internal bool c;

		public d(string A_0)
		{
			a = true;
			b = "";
			c = false;
			b = A_0;
		}
	}

	private NewMessageReceivedEventHandler m_a;

	private NewDeliveryReportEventHandler m_b;

	private NewIncomingCallEventHandler m_c;

	private NewUSSDReceivedEventhandler m_d;

	private QueueSMSSentEventHandler m_e;

	private QueueWapPushSentEventHandler m_f;

	private QueueVCardSentEventHandler m_g;

	private QueueVCalendarSentEventHandler m_h;

	private QueueSmartMessageSentEventHandler m_i;

	private QueueSMSSendingEventHandler m_j;

	private QueueWapPushSendingEventHandler m_k;

	private QueueVCardSendingEventHandler m_l;

	private QueueVCalendarSendingEventHandler m_m;

	private QueueSmartMessageSendingEventHandler m_n;

	[AccessedThroughProperty("gm")]
	private f m_o;

	[AccessedThroughProperty("objQueue")]
	private Queue m_p;

	internal License q;

	internal o r;

	internal WapPush s;

	internal vCard t;

	internal vCalendar u;

	internal global::c v;

	internal Inbox w;

	private const int m_x = 1;

	private bool m_y;

	internal virtual f gm
	{
		[DebuggerNonUserCode]
		get
		{
			return this.m_o;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			this.m_o = value;
		}
	}

	internal virtual Queue objQueue
	{
		[DebuggerNonUserCode]
		get
		{
			return this.m_p;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			this.m_p = value;
		}
	}

	public string About => gm.@as();

	public string Port
	{
		get
		{
			gm.aw();
			return gm.aj;
		}
		set
		{
			string text = "";
			gm.aw();
			if (value == null)
			{
				value = "";
			}
			text = value.Trim().ToUpper();
			checked
			{
				if (text.Length >= 4)
				{
					if (text.StartsWith("COM") && Versioned.IsNumeric((object)text.Substring(3, text.Length - 3)) && !text.Contains(",") && !text.Contains("."))
					{
						if (Conversions.ToInteger(text.Substring(3, text.Length - 3)) >= 1 && Conversions.ToInteger(text.Substring(3, text.Length - 3)) <= 256)
						{
							if (gm.r(text))
							{
								if (string.Compare(gm.aj, text, ignoreCase: true) != 0)
								{
									if (gm.s().IsOpen)
									{
										gm.ak();
									}
									gm.be = -1;
									gm.bh = "";
									gm.bi = "";
									gm.bj = 0L;
									gm.bl = false;
									gm.a7 = false;
									gm.bv = false;
									gm.bw = false;
									if (this.v != null)
									{
										this.v = null;
									}
									if (this.w != null)
									{
										this.w = null;
									}
									SMS A_ = this;
									this.v = new global::c(ref A_);
									A_ = this;
									this.w = new Inbox(ref A_);
								}
								gm.aj = text;
								this.r.b("Port: " + text);
							}
							else
							{
								gm.a(1050L, "Specified COM port does not exist", "GENERAL");
							}
						}
						else
						{
							gm.a(1001L, "Invalid COM port setting", "GENERAL");
						}
					}
					else
					{
						gm.a(1001L, "Invalid COM port setting", "GENERAL");
					}
				}
				else
				{
					gm.a(1001L, "Invalid COM port setting", "GENERAL");
				}
				a();
			}
		}
	}

	public BaudRate BaudRate
	{
		get
		{
			gm.aw();
			BaudRate baudRate;
			try
			{
				baudRate = (BaudRate)gm.s().BaudRate;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				gm.a.b("[SMS:BaudRate:1] " + ex2.Message);
				gm.s().BaudRate = 9600;
				baudRate = (BaudRate)gm.s().BaudRate;
				ProjectData.ClearProjectError();
			}
			return baudRate;
		}
		set
		{
			gm.aw();
			if (value == BaudRate.BaudRate_110 || value == BaudRate.BaudRate_300 || value == BaudRate.BaudRate_1200 || value == BaudRate.BaudRate_2400 || value == BaudRate.BaudRate_4800 || value == BaudRate.BaudRate_9600 || value == BaudRate.BaudRate_14400 || value == BaudRate.BaudRate_19200 || value == BaudRate.BaudRate_38400 || value == BaudRate.BaudRate_57600 || value == BaudRate.BaudRate_115200 || value == BaudRate.BaudRate_230400 || value == BaudRate.BaudRate_460800 || value == BaudRate.BaudRate_921600)
			{
				gm.s().BaudRate = (int)value;
				gm.ak = (int)value;
				o obj = this.r;
				int num = (int)value;
				obj.b("BaudRate: " + num);
			}
			else
			{
				gm.a(1003L, "Invalid baudrate setting", "GENERAL");
			}
			a();
		}
	}

	public Parity Parity
	{
		get
		{
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Expected I4, but got Unknown
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected I4, but got Unknown
			gm.aw();
			Parity result;
			try
			{
				result = (Parity)gm.s().Parity;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				gm.a.b("[SMS:Parity:1] " + ex2.Message);
				gm.s().Parity = (Parity)0;
				result = (Parity)gm.s().Parity;
				ProjectData.ClearProjectError();
			}
			return result;
		}
		set
		{
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			gm.aw();
			if (value >= Parity.None && value <= Parity.Space)
			{
				switch (value)
				{
				case Parity.None:
					gm.s().Parity = (Parity)0;
					break;
				case Parity.Odd:
					gm.s().Parity = (Parity)1;
					break;
				case Parity.Even:
					gm.s().Parity = (Parity)2;
					break;
				case Parity.Mark:
					gm.s().Parity = (Parity)3;
					break;
				case Parity.Space:
					gm.s().Parity = (Parity)4;
					break;
				}
				gm.al = gm.s().Parity;
				this.r.b("Parity: " + value);
			}
			else
			{
				gm.a(1004L, "Invalid parity setting", "GENERAL");
			}
			a();
		}
	}

	public DataBits DataBits
	{
		get
		{
			gm.aw();
			DataBits dataBits;
			try
			{
				dataBits = (DataBits)gm.s().DataBits;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				gm.a.b("[SMS:DataBits:1] " + ex2.Message);
				gm.s().DataBits = 8;
				dataBits = (DataBits)gm.s().DataBits;
				ProjectData.ClearProjectError();
			}
			return dataBits;
		}
		set
		{
			gm.aw();
			if (value >= DataBits.Four && value <= DataBits.Eight)
			{
				gm.s().DataBits = (int)value;
				gm.am = (int)value;
				o obj = this.r;
				int num = (int)value;
				obj.b("DataBits: " + num);
			}
			else
			{
				gm.a(1005L, "Invalid data bits setting", "GENERAL");
			}
			a();
		}
	}

	public StopBits StopBits
	{
		get
		{
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Expected I4, but got Unknown
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected I4, but got Unknown
			gm.aw();
			StopBits result;
			try
			{
				result = (StopBits)gm.s().StopBits;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				gm.a.b("[SMS:StopBits:1] " + ex2.Message);
				gm.s().StopBits = (StopBits)1;
				result = (StopBits)gm.s().StopBits;
				ProjectData.ClearProjectError();
			}
			a();
			return result;
		}
		set
		{
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			gm.aw();
			if (value >= StopBits.One && value <= StopBits.OnePointFive)
			{
				switch (value)
				{
				case StopBits.One:
					gm.s().StopBits = (StopBits)1;
					break;
				case StopBits.OnePointFive:
					gm.s().StopBits = (StopBits)3;
					break;
				case StopBits.Two:
					gm.s().StopBits = (StopBits)2;
					break;
				}
				gm.an = gm.s().StopBits;
				this.r.b("StopBits: " + value);
			}
			else
			{
				gm.a(1006L, "Invalid stop bits setting", "GENERAL");
			}
			a();
		}
	}

	public FlowControl FlowControl
	{
		get
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected I4, but got Unknown
			gm.aw();
			FlowControl result = default(FlowControl);
			switch ((int)gm.s().Handshake)
			{
			case 0:
				result = FlowControl.None;
				break;
			case 2:
				result = FlowControl.RTS_CTS;
				break;
			case 1:
				result = FlowControl.Xon_Xoff;
				break;
			case 3:
				gm.s().Handshake = (Handshake)0;
				result = FlowControl.None;
				break;
			}
			a();
			return result;
		}
		set
		{
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Invalid comparison between Unknown and I4
			if (!gm.s().IsOpen)
			{
				switch (value)
				{
				case FlowControl.None:
					gm.s().Handshake = (Handshake)0;
					break;
				case FlowControl.RTS_CTS:
					gm.s().Handshake = (Handshake)2;
					break;
				case FlowControl.Xon_Xoff:
					gm.s().Handshake = (Handshake)1;
					break;
				}
				gm.ao = gm.s().Handshake;
				if ((int)gm.s().Handshake != 2)
				{
					gm.s().RtsEnable = false;
				}
				gm.s().DtrEnable = false;
				this.r.b("Handshake: " + value);
			}
			else
			{
				gm.a(1032L, "Cannot change flow control when port is open", "GENERAL");
			}
			a();
		}
	}

	public long Timeout
	{
		get
		{
			gm.aw();
			long av = gm.av;
			a();
			return av;
		}
		set
		{
			gm.aw();
			if (value >= 500 && value <= 60000)
			{
				gm.av = value;
				if (gm.s().IsOpen)
				{
					gm.ak();
				}
				this.r.b("Timeout: " + value + "ms");
			}
			else
			{
				gm.a(1007L, "Invalid timeout setting", "GENERAL");
			}
			a();
		}
	}

	public int ReadIntervalTimeout
	{
		get
		{
			gm.aw();
			return gm.a3;
		}
		set
		{
			gm.aw();
			if (value > 0 && value <= 5000)
			{
				gm.a3 = value;
				this.r.b("ReadIntervalTimeout: " + value + "ms");
			}
			else
			{
				gm.a(1045L, "Invalid value for read delay timeout", "GENERAL");
			}
			a();
		}
	}

	public int ThreadLockTimeout
	{
		get
		{
			gm.aw();
			return gm.a4;
		}
		set
		{
			gm.aw();
			if (value >= 60000 && value <= int.MaxValue)
			{
				gm.a4 = value;
				this.r.b("ThreadLockTimeout: " + value + "ms");
			}
			else
			{
				gm.a(9010L, "Invalid value for thread lock timeout", "GENERAL");
			}
			a();
		}
	}

	public bool IsConnected
	{
		get
		{
			c c = new c();
			ThreadPool.RegisterWaitForSingleObject(gm.dz, r, c, -1, executeOnlyOnce: true);
			while (c.a)
			{
				Thread.Sleep(1);
				Application.DoEvents();
			}
			bool result = c.b;
			a();
			return result;
		}
	}

	public MessageMemory MessageMemory
	{
		get
		{
			c c = new c();
			ThreadPool.RegisterWaitForSingleObject(gm.dz, v, c, -1, executeOnlyOnce: true);
			while (c.a)
			{
				Thread.Sleep(1);
				Application.DoEvents();
			}
			MessageMemory result = c.c;
			a();
			return result;
		}
		set
		{
			c c = new c();
			c.c = value;
			ThreadPool.RegisterWaitForSingleObject(gm.dz, u, c, -1, executeOnlyOnce: true);
			while (c.a)
			{
				Thread.Sleep(1);
				Application.DoEvents();
			}
			a();
		}
	}

	public string Manufacturer
	{
		get
		{
			c c = new c();
			ThreadPool.RegisterWaitForSingleObject(gm.dz, q, c, -1, executeOnlyOnce: true);
			while (c.a)
			{
				Thread.Sleep(1);
				Application.DoEvents();
			}
			string result = c.d;
			a();
			return result;
		}
	}

	public string Model
	{
		get
		{
			c c = new c();
			ThreadPool.RegisterWaitForSingleObject(gm.dz, p, c, -1, executeOnlyOnce: true);
			while (c.a)
			{
				Thread.Sleep(1);
				Application.DoEvents();
			}
			string result = c.d;
			a();
			return result;
		}
	}

	public string IMEI
	{
		get
		{
			c c = new c();
			ThreadPool.RegisterWaitForSingleObject(gm.dz, o, c, -1, executeOnlyOnce: true);
			while (c.a)
			{
				Thread.Sleep(1);
				Application.DoEvents();
			}
			string result = c.d;
			a();
			return result;
		}
	}

	public string IMSI
	{
		get
		{
			c c = new c();
			ThreadPool.RegisterWaitForSingleObject(gm.dz, n, c, -1, executeOnlyOnce: true);
			while (c.a)
			{
				Thread.Sleep(1);
				Application.DoEvents();
			}
			string result = c.d;
			a();
			return result;
		}
	}

	public string Revision
	{
		get
		{
			c c = new c();
			ThreadPool.RegisterWaitForSingleObject(gm.dz, m, c, -1, executeOnlyOnce: true);
			while (c.a)
			{
				Thread.Sleep(1);
				Application.DoEvents();
			}
			string result = c.d;
			a();
			return result;
		}
	}

	public int SignalStrength
	{
		get
		{
			c c = new c();
			ThreadPool.RegisterWaitForSingleObject(gm.dz, l, c, -1, executeOnlyOnce: true);
			while (c.a)
			{
				Thread.Sleep(1);
				Application.DoEvents();
			}
			this.r.b("Signal strength: " + c.e + "%");
			int result = c.e;
			a();
			return result;
		}
	}

	public int SignalStrengthDB
	{
		get
		{
			c c = new c();
			ThreadPool.RegisterWaitForSingleObject(gm.dz, k, c, -1, executeOnlyOnce: true);
			while (c.a)
			{
				Thread.Sleep(1);
				Application.DoEvents();
			}
			this.r.b("Signal strength (DB): " + c.e + "dB");
			int result = c.e;
			a();
			return result;
		}
	}

	public string SMSC
	{
		get
		{
			c c = new c();
			ThreadPool.RegisterWaitForSingleObject(gm.dz, j, c, -1, executeOnlyOnce: true);
			while (c.a)
			{
				Thread.Sleep(1);
				Application.DoEvents();
			}
			string result = c.d;
			a();
			return result;
		}
		set
		{
			c c = new c();
			c.d = value;
			ThreadPool.RegisterWaitForSingleObject(gm.dz, i, c, -1, executeOnlyOnce: true);
			while (c.a)
			{
				Thread.Sleep(1);
				Application.DoEvents();
			}
			a();
		}
	}

	public string Network
	{
		get
		{
			c c = new c();
			ThreadPool.RegisterWaitForSingleObject(gm.dz, h, c, -1, executeOnlyOnce: true);
			while (c.a)
			{
				Thread.Sleep(1);
				Application.DoEvents();
			}
			string result = c.d;
			a();
			return result;
		}
	}

	public string Validity
	{
		get
		{
			gm.aw();
			string result = gm.al();
			a();
			return result;
		}
		set
		{
			gm.aw();
			if (value == null)
			{
				value = "";
			}
			if (!gm.g(value.Trim()) && gm.z == 0)
			{
				gm.a(4001L, "Unable to set validity period", "GENERAL");
			}
			a();
		}
	}

	public LongMessage LongMessage
	{
		get
		{
			gm.aw();
			return (LongMessage)gm.by;
		}
		set
		{
			gm.aw();
			if (value >= LongMessage.Truncate && value <= LongMessage.Concatenate)
			{
				gm.by = (int)value;
				this.r.b("Long Message: " + value);
			}
			else
			{
				gm.a(4002L, "Invalid long message action", "GENERAL");
			}
			a();
		}
	}

	public string PIN
	{
		set
		{
			gm.aw();
			if (value == null)
			{
				value = "";
			}
			gm.bg = value.Trim();
			this.r.b("PIN: " + value.Trim());
		}
	}

	public bool DisableCheckPIN
	{
		get
		{
			gm.aw();
			return gm.bk;
		}
		set
		{
			gm.aw();
			gm.bk = value;
			if (value)
			{
				this.r.b("Disable PIN status check");
			}
			else
			{
				this.r.b("Enable PIN status check");
			}
		}
	}

	public int DelayAfterPIN
	{
		get
		{
			gm.aw();
			return gm.az;
		}
		set
		{
			gm.aw();
			if (value >= 1 && value <= 60000)
			{
				gm.az = value;
				this.r.b("Delay after PIN: " + value + "ms");
			}
			else
			{
				gm.a(2028L, "Invalid value for delay after PIN", "GENERAL");
			}
			a();
		}
	}

	public int SendDelay
	{
		get
		{
			gm.aw();
			return gm.bz;
		}
		set
		{
			gm.aw();
			if (value >= 1 && value <= 30000)
			{
				gm.bz = value;
				this.r.b("Send Delay: " + value + "ms");
			}
			else
			{
				gm.a(4003L, "Invalid send delay value", "GENERAL");
			}
			a();
		}
	}

	public int SendRetry
	{
		get
		{
			gm.aw();
			return gm.b0;
		}
		set
		{
			gm.aw();
			if (value >= 0 && value <= 10)
			{
				gm.b0 = value;
				this.r.b("Send Retry: " + value);
			}
			else
			{
				gm.a(4004L, "Invalid send retry value", "GENERAL");
			}
			a();
		}
	}

	public Encoding Encoding
	{
		get
		{
			gm.aw();
			Encoding encoding = default(Encoding);
			return gm.bx switch
			{
				7 => Encoding.GSM_Default_7Bit, 
				8 => Encoding.ANSI_8Bit, 
				16 => Encoding.Unicode_16Bit, 
				_ => encoding, 
			};
		}
		set
		{
			gm.aw();
			if (value == Encoding.GSM_Default_7Bit || value == Encoding.ANSI_8Bit || value == Encoding.Unicode_16Bit)
			{
				switch (value)
				{
				case Encoding.GSM_Default_7Bit:
					gm.bx = 7;
					break;
				case Encoding.ANSI_8Bit:
					gm.bx = 8;
					break;
				case Encoding.Unicode_16Bit:
					gm.bx = 16;
					break;
				}
				this.r.b("Encoding: " + value);
			}
			else
			{
				gm.a(4005L, "Invalid character encoding", "GENERAL");
			}
			a();
		}
	}

	public bool ReconnectOnSendError
	{
		get
		{
			return gm.c9;
		}
		set
		{
			gm.c9 = value;
		}
	}

	public int BatteryLevel
	{
		get
		{
			c c = new c();
			ThreadPool.RegisterWaitForSingleObject(gm.dz, g, c, -1, executeOnlyOnce: true);
			while (c.a)
			{
				Thread.Sleep(1);
				Application.DoEvents();
			}
			int result = c.e;
			a();
			return result;
		}
	}

	public string OwnNumber
	{
		get
		{
			c c = new c();
			ThreadPool.RegisterWaitForSingleObject(gm.dz, f, c, -1, executeOnlyOnce: true);
			while (c.a)
			{
				Thread.Sleep(1);
				Application.DoEvents();
			}
			string result = c.d;
			a();
			return result;
		}
	}

	public bool DebugMode
	{
		get
		{
			gm.aw();
			return gm.r;
		}
		set
		{
			gm.aw();
			gm.r = value;
			this.r.b("Debug mode: " + value);
		}
	}

	public LogType LogType
	{
		get
		{
			gm.aw();
			return (LogType)this.r.b();
		}
		set
		{
			gm.aw();
			if (this.r.a().Trim().Length > 0)
			{
				if (value >= LogType.NoLog && value <= LogType.ErrorEventLog)
				{
					this.r.b((int)value);
					this.r.b(gm.am());
					this.r.b("Log type: " + value);
				}
				else
				{
					gm.a(9001L, "Invalid log type option", "GENERAL");
				}
			}
			else
			{
				gm.a(9003L, "Invalid log file path", "GENERAL");
			}
			a();
		}
	}

	public string LogFolderPath
	{
		get
		{
			gm.aw();
			return this.r.a();
		}
		set
		{
			gm.aw();
			if (value == null)
			{
				value = "";
			}
			value = value.Trim();
			if (value.Length > 0)
			{
				if (Directory.Exists(value))
				{
					this.r.a(value);
					this.r.b("Log folder: " + value);
				}
				else
				{
					try
					{
						Directory.CreateDirectory(value);
						this.r.a(value);
						this.r.b("Log folder: " + value);
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						gm.a(9002L, "Error creating log file path", "GENERAL");
						ProjectData.ClearProjectError();
					}
				}
			}
			else
			{
				gm.a(9003L, "Invalid log file path", "GENERAL");
			}
			a();
		}
	}

	public long LogSize
	{
		get
		{
			gm.aw();
			long result = this.r.c();
			a();
			return result;
		}
	}

	public long ErrorCode => gm.z;

	public string ErrorDescription => gm.y;

	public string ModemInitString
	{
		get
		{
			gm.aw();
			return gm.a6;
		}
		set
		{
			gm.aw();
			if (value == null)
			{
				value = "";
			}
			gm.a6 = value.Trim().ToUpper();
		}
	}

	public bool DeliveryReport
	{
		get
		{
			gm.aw();
			return gm.bu;
		}
		set
		{
			c c = new c();
			c.f = value;
			ThreadPool.RegisterWaitForSingleObject(gm.dz, d, c, -1, executeOnlyOnce: true);
			while (c.a)
			{
				Thread.Sleep(1);
				Application.DoEvents();
			}
			a();
		}
	}

	public bool NewMessageIndication
	{
		get
		{
			gm.aw();
			return gm.s;
		}
		set
		{
			c c = new c();
			c.f = value;
			ThreadPool.RegisterWaitForSingleObject(gm.dz, this.c, c, -1, executeOnlyOnce: true);
			while (c.a)
			{
				Thread.Sleep(1);
				Application.DoEvents();
			}
			a();
		}
	}

	public bool NewUSSDIndication
	{
		get
		{
			gm.aw();
			return gm.w;
		}
		set
		{
			gm.aw();
			gm.w = value;
		}
	}

	public bool NewMessageConcatenate
	{
		get
		{
			gm.aw();
			return gm.t;
		}
		set
		{
			gm.aw();
			gm.t = value;
		}
	}

	public bool AutoDeleteNewMessage
	{
		get
		{
			gm.aw();
			return gm.x;
		}
		set
		{
			c c = new c();
			c.f = value;
			ThreadPool.RegisterWaitForSingleObject(gm.dz, b, c, -1, executeOnlyOnce: true);
			while (c.a)
			{
				Thread.Sleep(1);
				Application.DoEvents();
			}
			a();
		}
	}

	public bool IncomingCallIndication
	{
		get
		{
			gm.aw();
			return gm.v;
		}
		set
		{
			c c = new c();
			c.f = value;
			ThreadPool.RegisterWaitForSingleObject(gm.dz, a, c, -1, executeOnlyOnce: true);
			while (c.a)
			{
				Thread.Sleep(1);
				Application.DoEvents();
			}
			a();
		}
	}

	public event NewMessageReceivedEventHandler NewMessageReceived
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		add
		{
			this.m_a = (NewMessageReceivedEventHandler)Delegate.Combine(this.m_a, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		remove
		{
			this.m_a = (NewMessageReceivedEventHandler)Delegate.Remove(this.m_a, value);
		}
	}

	public event NewDeliveryReportEventHandler NewDeliveryReport
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		add
		{
			this.m_b = (NewDeliveryReportEventHandler)Delegate.Combine(this.m_b, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		remove
		{
			this.m_b = (NewDeliveryReportEventHandler)Delegate.Remove(this.m_b, value);
		}
	}

	public event NewIncomingCallEventHandler NewIncomingCall
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		add
		{
			this.m_c = (NewIncomingCallEventHandler)Delegate.Combine(this.m_c, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		remove
		{
			this.m_c = (NewIncomingCallEventHandler)Delegate.Remove(this.m_c, value);
		}
	}

	public event NewUSSDReceivedEventhandler NewUSSDReceived
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		add
		{
			this.m_d = (NewUSSDReceivedEventhandler)Delegate.Combine(this.m_d, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		remove
		{
			this.m_d = (NewUSSDReceivedEventhandler)Delegate.Remove(this.m_d, value);
		}
	}

	public event QueueSMSSentEventHandler QueueSMSSent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		add
		{
			this.m_e = (QueueSMSSentEventHandler)Delegate.Combine(this.m_e, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		remove
		{
			this.m_e = (QueueSMSSentEventHandler)Delegate.Remove(this.m_e, value);
		}
	}

	public event QueueWapPushSentEventHandler QueueWapPushSent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		add
		{
			this.m_f = (QueueWapPushSentEventHandler)Delegate.Combine(this.m_f, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		remove
		{
			this.m_f = (QueueWapPushSentEventHandler)Delegate.Remove(this.m_f, value);
		}
	}

	public event QueueVCardSentEventHandler QueueVCardSent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		add
		{
			this.m_g = (QueueVCardSentEventHandler)Delegate.Combine(this.m_g, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		remove
		{
			this.m_g = (QueueVCardSentEventHandler)Delegate.Remove(this.m_g, value);
		}
	}

	public event QueueVCalendarSentEventHandler QueueVCalendarSent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		add
		{
			this.m_h = (QueueVCalendarSentEventHandler)Delegate.Combine(this.m_h, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		remove
		{
			this.m_h = (QueueVCalendarSentEventHandler)Delegate.Remove(this.m_h, value);
		}
	}

	public event QueueSmartMessageSentEventHandler QueueSmartMessageSent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		add
		{
			this.m_i = (QueueSmartMessageSentEventHandler)Delegate.Combine(this.m_i, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		remove
		{
			this.m_i = (QueueSmartMessageSentEventHandler)Delegate.Remove(this.m_i, value);
		}
	}

	public event QueueSMSSendingEventHandler QueueSMSSending
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		add
		{
			this.m_j = (QueueSMSSendingEventHandler)Delegate.Combine(this.m_j, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		remove
		{
			this.m_j = (QueueSMSSendingEventHandler)Delegate.Remove(this.m_j, value);
		}
	}

	public event QueueWapPushSendingEventHandler QueueWapPushSending
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		add
		{
			this.m_k = (QueueWapPushSendingEventHandler)Delegate.Combine(this.m_k, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		remove
		{
			this.m_k = (QueueWapPushSendingEventHandler)Delegate.Remove(this.m_k, value);
		}
	}

	public event QueueVCardSendingEventHandler QueueVCardSending
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		add
		{
			this.m_l = (QueueVCardSendingEventHandler)Delegate.Combine(this.m_l, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		remove
		{
			this.m_l = (QueueVCardSendingEventHandler)Delegate.Remove(this.m_l, value);
		}
	}

	public event QueueVCalendarSendingEventHandler QueueVCalendarSending
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		add
		{
			this.m_m = (QueueVCalendarSendingEventHandler)Delegate.Combine(this.m_m, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		remove
		{
			this.m_m = (QueueVCalendarSendingEventHandler)Delegate.Remove(this.m_m, value);
		}
	}

	public event QueueSmartMessageSendingEventHandler QueueSmartMessageSending
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		add
		{
			this.m_n = (QueueSmartMessageSendingEventHandler)Delegate.Combine(this.m_n, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		remove
		{
			this.m_n = (QueueSmartMessageSendingEventHandler)Delegate.Remove(this.m_n, value);
		}
	}

	public SMS()
	{
		this.m_y = false;
		b();
	}

	public SMS(string License_Company, string License_Type, string License_Key)
	{
		this.m_y = false;
		b();
		if (this.q == null)
		{
			SMS A_ = this;
			this.q = new License(ref A_);
		}
		this.q.Company = License_Company;
		this.q.LicenseType = License_Type;
		this.q.Key = License_Key;
	}

	private void b()
	{
		gm = new f();
		SMS A_ = this;
		objQueue = new Queue(ref A_);
		A_ = this;
		this.r = new o(ref A_);
		A_ = this;
		this.q = new License(ref A_);
		A_ = this;
		this.s = new WapPush(ref A_);
		A_ = this;
		this.t = new vCard(ref A_);
		A_ = this;
		this.u = new vCalendar(ref A_);
		A_ = this;
		this.v = new global::c(ref A_);
		A_ = this;
		this.w = new Inbox(ref A_);
		gm.a = this.r;
		gm.b = this.q;
		gm.c = this.v;
		gm.ck = a(gm.l("P") + gm.cj + gm.l("V"));
		gm.cl = a(gm.l("H") + gm.cj + gm.l("M"));
		gm.cm = a(gm.l("T") + gm.cj + gm.l("V"));
		gm.cn = a(gm.l("@") + gm.cj + gm.l("M"));
		gm.ah = gm.cn;
		this.r.b(0);
		this.r.a(gm.q);
		gm.a((f.f)a);
		gm.a((f.e)a);
		gm.a((f.b)a);
		gm.a((f.a)a);
		objQueue.QSMSSent += a;
		objQueue.QWapPushSent += a;
		objQueue.QVCardSent += a;
		objQueue.QVCalendarSent += a;
		objQueue.QSmartMessageSent += a;
		objQueue.QSMSSending += a;
		objQueue.QWapPushSending += a;
		objQueue.QVCardSending += a;
		objQueue.QVCalendarSending += a;
		objQueue.QSmartMessageSending += a;
		gm.dz.Set();
	}

	public WapPush WapPush()
	{
		if (this.s == null)
		{
			SMS A_ = this;
			this.s = new WapPush(ref A_);
		}
		return this.s;
	}

	WapPush ISMS.WapPush()
	{
		//ILSpy generated this explicit interface implementation from .override directive in WapPush
		return this.WapPush();
	}

	public vCard vCard()
	{
		if (this.t == null)
		{
			SMS A_ = this;
			this.t = new vCard(ref A_);
		}
		return this.t;
	}

	vCard ISMS.vCard()
	{
		//ILSpy generated this explicit interface implementation from .override directive in vCard
		return this.vCard();
	}

	public vCalendar vCalendar()
	{
		if (this.u == null)
		{
			SMS A_ = this;
			this.u = new vCalendar(ref A_);
		}
		return this.u;
	}

	vCalendar ISMS.vCalendar()
	{
		//ILSpy generated this explicit interface implementation from .override directive in vCalendar
		return this.vCalendar();
	}

	public License License()
	{
		if (this.q == null)
		{
			SMS A_ = this;
			this.q = new License(ref A_);
		}
		return this.q;
	}

	License ISMS.License()
	{
		//ILSpy generated this explicit interface implementation from .override directive in License
		return this.License();
	}

	public Queue Queue()
	{
		if (objQueue == null)
		{
			SMS A_ = this;
			objQueue = new Queue(ref A_);
		}
		return objQueue;
	}

	Queue ISMS.Queue()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Queue
		return this.Queue();
	}

	public Inbox Inbox()
	{
		if (this.v == null)
		{
			SMS A_ = this;
			this.v = new global::c(ref A_);
		}
		if (this.w == null)
		{
			SMS A_ = this;
			this.w = new Inbox(ref A_);
		}
		return this.w;
	}

	Inbox ISMS.inbox()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Inbox
		return this.Inbox();
	}

	public Message Inbox(int Index)
	{
		Message result = null;
		try
		{
			gm.aw();
			if (Index <= 0 || Index > gm.c0.Count)
			{
				gm.ae = 0;
				gm.a(5001L, "Invalid inbox message index", "SMSREAD");
				result = null;
			}
			else
			{
				gm.ae = Index;
				result = (Message)gm.c0[Index.ToString()];
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			gm.a.b("[SMS:Inbox:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		return result;
	}

	Message ISMS.inbox(int Index)
	{
		//ILSpy generated this explicit interface implementation from .override directive in Inbox
		return this.Inbox(Index);
	}

	public bool Connect()
	{
		c c = new c();
		ThreadPool.RegisterWaitForSingleObject(gm.dz, t, c, -1, executeOnlyOnce: true);
		while (c.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		bool result = c.b;
		if (c != null)
		{
			c = null;
		}
		a();
		return result;
	}

	bool ISMS.Connect()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Connect
		return this.Connect();
	}

	public bool Disconnect()
	{
		c c = new c();
		ThreadPool.RegisterWaitForSingleObject(gm.dz, s, c, -1, executeOnlyOnce: true);
		while (c.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		bool result = c.b;
		if (c != null)
		{
			c = null;
		}
		a();
		return result;
	}

	bool ISMS.Disconnect()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Disconnect
		return this.Disconnect();
	}

	public bool SendUSSD(string USSD_Command)
	{
		d d = new d("1,\"" + USSD_Command + "\",15");
		ThreadPool.RegisterWaitForSingleObject(gm.dz, w, d, -1, executeOnlyOnce: true);
		while (d.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		bool flag = d.c;
		if (d != null)
		{
			d = null;
		}
		bool result = flag;
		a();
		return result;
	}

	bool ISMS.SendUSSD(string USSD_Command)
	{
		//ILSpy generated this explicit interface implementation from .override directive in SendUSSD
		return this.SendUSSD(USSD_Command);
	}

	public bool CancelUSSDSession()
	{
		d d = new d("2");
		ThreadPool.RegisterWaitForSingleObject(gm.dz, w, d, -1, executeOnlyOnce: true);
		while (d.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		bool flag = d.c;
		if (d != null)
		{
			d = null;
		}
		bool result = true;
		a();
		return result;
	}

	bool ISMS.CancelUSSDSession()
	{
		//ILSpy generated this explicit interface implementation from .override directive in CancelUSSDSession
		return this.CancelUSSDSession();
	}

	public string SendSMS(string DestinationNumber, string TextMessage)
	{
		b b = new b(DestinationNumber, TextMessage, A_2: false, gm.bx, gm.by, gm.bu, gm.bt);
		ThreadPool.RegisterWaitForSingleObject(gm.dz, aa, b, -1, executeOnlyOnce: true);
		while (b.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		string text = b.i;
		if (b != null)
		{
			b = null;
		}
		string result = text;
		a();
		return result;
	}

	string ISMS.SendSMS(string DestinationNumber, string TextMessage)
	{
		//ILSpy generated this explicit interface implementation from .override directive in SendSMS
		return this.SendSMS(DestinationNumber, TextMessage);
	}

	public string SendSMS(string DestinationNumber, string TextMessage, bool AlertMessage)
	{
		b b = new b(DestinationNumber, TextMessage, AlertMessage, gm.bx, gm.by, gm.bu, gm.bt);
		ThreadPool.RegisterWaitForSingleObject(gm.dz, aa, b, -1, executeOnlyOnce: true);
		while (b.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		string text = b.i;
		if (b != null)
		{
			b = null;
		}
		string result = text;
		a();
		return result;
	}

	string ISMS.SendSMS(string DestinationNumber, string TextMessage, bool AlertMessage)
	{
		//ILSpy generated this explicit interface implementation from .override directive in SendSMS
		return this.SendSMS(DestinationNumber, TextMessage, AlertMessage);
	}

	public string SendSmartMessage(string DestinationNumber, string TextMessage, int DestinationPort)
	{
		a a = new a(DestinationNumber, TextMessage, gm.bu, gm.bt, 0, DestinationPort);
		ThreadPool.RegisterWaitForSingleObject(gm.dz, y, a, -1, executeOnlyOnce: true);
		while (a.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		string text = a.f;
		if (a != null)
		{
			a = null;
		}
		string result = text;
		this.a();
		return result;
	}

	string ISMS.SendSmartMessage(string DestinationNumber, string TextMessage, int DestinationPort)
	{
		//ILSpy generated this explicit interface implementation from .override directive in SendSmartMessage
		return this.SendSmartMessage(DestinationNumber, TextMessage, DestinationPort);
	}

	public string SendSmartMessage(string DestinationNumber, string TextMessage, int DestinationPort, int SourcePort)
	{
		a a = new a(DestinationNumber, TextMessage, gm.bu, gm.bt, SourcePort, DestinationPort);
		ThreadPool.RegisterWaitForSingleObject(gm.dz, y, a, -1, executeOnlyOnce: true);
		while (a.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		string text = a.f;
		if (a != null)
		{
			a = null;
		}
		string result = text;
		this.a();
		return result;
	}

	string ISMS.SendSmartMessage(string DestinationNumber, string TextMessage, int DestinationPort, int SourcePort)
	{
		//ILSpy generated this explicit interface implementation from .override directive in SendSmartMessage
		return this.SendSmartMessage(DestinationNumber, TextMessage, DestinationPort, SourcePort);
	}

	public string SendSmartMessageToQueue(string DestinationNumber, string TextMessage, int DestinationPort)
	{
		a a = new a(DestinationNumber, TextMessage, gm.bu, gm.bt, 0, DestinationPort);
		ThreadPool.RegisterWaitForSingleObject(gm.dz, x, a, -1, executeOnlyOnce: true);
		while (a.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		string text = a.f;
		if (a != null)
		{
			a = null;
		}
		string result = text;
		this.a();
		return result;
	}

	string ISMS.SendSmartMessageToQueue(string DestinationNumber, string TextMessage, int DestinationPort)
	{
		//ILSpy generated this explicit interface implementation from .override directive in SendSmartMessageToQueue
		return this.SendSmartMessageToQueue(DestinationNumber, TextMessage, DestinationPort);
	}

	public string SendSmartMessageToQueue(string DestinationNumber, string TextMessage, int DestinationPort, int SourcePort)
	{
		a a = new a(DestinationNumber, TextMessage, gm.bu, gm.bt, SourcePort, DestinationPort);
		ThreadPool.RegisterWaitForSingleObject(gm.dz, x, a, -1, executeOnlyOnce: true);
		while (a.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		string text = a.f;
		if (a != null)
		{
			a = null;
		}
		string result = text;
		this.a();
		return result;
	}

	string ISMS.SendSmartMessageToQueue(string DestinationNumber, string TextMessage, int DestinationPort, int SourcePort)
	{
		//ILSpy generated this explicit interface implementation from .override directive in SendSmartMessageToQueue
		return this.SendSmartMessageToQueue(DestinationNumber, TextMessage, DestinationPort, SourcePort);
	}

	public string SendSmartMessageToQueue(string DestinationNumber, string TextMessage, int DestinationPort, QueuePriority Priority)
	{
		a a = new a(DestinationNumber, TextMessage, gm.bu, gm.bt, 0, DestinationPort, (int)Priority);
		ThreadPool.RegisterWaitForSingleObject(gm.dz, x, a, -1, executeOnlyOnce: true);
		while (a.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		string text = a.f;
		if (a != null)
		{
			a = null;
		}
		string result = text;
		this.a();
		return result;
	}

	string ISMS.SendSmartMessageToQueue(string DestinationNumber, string TextMessage, int DestinationPort, QueuePriority Priority)
	{
		//ILSpy generated this explicit interface implementation from .override directive in SendSmartMessageToQueue
		return this.SendSmartMessageToQueue(DestinationNumber, TextMessage, DestinationPort, Priority);
	}

	public string SendSmartMessageToQueue(string DestinationNumber, string TextMessage, int DestinationPort, int SourcePort, QueuePriority Priority)
	{
		a a = new a(DestinationNumber, TextMessage, gm.bu, gm.bt, SourcePort, DestinationPort, (int)Priority);
		ThreadPool.RegisterWaitForSingleObject(gm.dz, x, a, -1, executeOnlyOnce: true);
		while (a.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		string text = a.f;
		if (a != null)
		{
			a = null;
		}
		string result = text;
		this.a();
		return result;
	}

	string ISMS.SendSmartMessageToQueue(string DestinationNumber, string TextMessage, int DestinationPort, int SourcePort, QueuePriority Priority)
	{
		//ILSpy generated this explicit interface implementation from .override directive in SendSmartMessageToQueue
		return this.SendSmartMessageToQueue(DestinationNumber, TextMessage, DestinationPort, SourcePort, Priority);
	}

	public string SendSMSToQueue(string DestinationNumber, string TextMessage)
	{
		b b = new b(DestinationNumber, TextMessage, A_2: false, gm.bx, gm.by, gm.bu, gm.bt);
		ThreadPool.RegisterWaitForSingleObject(gm.dz, z, b, -1, executeOnlyOnce: true);
		while (b.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		string text = b.i;
		if (b != null)
		{
			b = null;
		}
		string result = text;
		a();
		return result;
	}

	string ISMS.SendSMSToQueue(string DestinationNumber, string TextMessage)
	{
		//ILSpy generated this explicit interface implementation from .override directive in SendSMSToQueue
		return this.SendSMSToQueue(DestinationNumber, TextMessage);
	}

	public string SendSMSToQueue(string DestinationNumber, string TextMessage, QueuePriority Priority)
	{
		b b = new b(DestinationNumber, TextMessage, A_2: false, gm.bx, gm.by, gm.bu, gm.bt, (int)Priority);
		ThreadPool.RegisterWaitForSingleObject(gm.dz, z, b, -1, executeOnlyOnce: true);
		while (b.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		string text = b.i;
		if (b != null)
		{
			b = null;
		}
		string result = text;
		a();
		return result;
	}

	string ISMS.SendSMSToQueue(string DestinationNumber, string TextMessage, QueuePriority Priority)
	{
		//ILSpy generated this explicit interface implementation from .override directive in SendSMSToQueue
		return this.SendSMSToQueue(DestinationNumber, TextMessage, Priority);
	}

	public string SendSMSToQueue(string DestinationNumber, string TextMessage, QueuePriority Priority, bool AlertMessage)
	{
		b b = new b(DestinationNumber, TextMessage, AlertMessage, gm.bx, gm.by, gm.bu, gm.bt, (int)Priority);
		ThreadPool.RegisterWaitForSingleObject(gm.dz, z, b, -1, executeOnlyOnce: true);
		while (b.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		string text = b.i;
		if (b != null)
		{
			b = null;
		}
		string result = text;
		a();
		return result;
	}

	string ISMS.SendSMSToQueue(string DestinationNumber, string TextMessage, QueuePriority Priority, bool AlertMessage)
	{
		//ILSpy generated this explicit interface implementation from .override directive in SendSMSToQueue
		return this.SendSMSToQueue(DestinationNumber, TextMessage, Priority, AlertMessage);
	}

	private void a(QueueSMSSentEventArgs A_0)
	{
		this.m_e?.Invoke(this, A_0);
	}

	private void a(QueueWapPushSentEventArgs A_0)
	{
		this.m_f?.Invoke(this, A_0);
	}

	private void a(QueueVCardSentEventArgs A_0)
	{
		this.m_g?.Invoke(this, A_0);
	}

	private void a(QueueVCalendarSentEventArgs A_0)
	{
		this.m_h?.Invoke(this, A_0);
	}

	private void a(QueueSMSSendingEventArgs A_0)
	{
		this.m_j?.Invoke(this, A_0);
	}

	private void a(QueueWapPushSendingEventArgs A_0)
	{
		this.m_k?.Invoke(this, A_0);
	}

	private void a(QueueVCardSendingEventArgs A_0)
	{
		this.m_l?.Invoke(this, A_0);
	}

	private void a(QueueVCalendarSendingEventArgs A_0)
	{
		this.m_m?.Invoke(this, A_0);
	}

	private void a(QueueSmartMessageSendingEventArgs A_0)
	{
		this.m_n?.Invoke(this, A_0);
	}

	private void a(QueueSmartMessageSentEventArgs A_0)
	{
		this.m_i?.Invoke(this, A_0);
	}

	public bool ClearLog()
	{
		gm.aw();
		bool result = this.r.a(0);
		a();
		return result;
	}

	bool ISMS.ClearLog()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ClearLog
		return this.ClearLog();
	}

	public bool ClearLog(int KeepLinesInLog)
	{
		gm.aw();
		bool result = this.r.a(KeepLinesInLog);
		a();
		return result;
	}

	bool ISMS.ClearLog(int KeepLinesInLog)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ClearLog
		return this.ClearLog(KeepLinesInLog);
	}

	public bool IsError()
	{
		string text = "";
		bool result = false;
		if (gm.z != 0)
		{
			result = true;
		}
		return result;
	}

	bool ISMS.IsError()
	{
		//ILSpy generated this explicit interface implementation from .override directive in IsError
		return this.IsError();
	}

	public bool IsError(bool ShowMsgBox)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		string text = "";
		bool result = false;
		if (gm.z != 0)
		{
			if (ShowMsgBox)
			{
				text = "ERROR " + gm.z + ": " + gm.y;
				try
				{
					Interaction.MsgBox((object)text, (MsgBoxStyle)16, (object)"mCore Library Error");
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					ProjectData.ClearProjectError();
				}
			}
			result = true;
		}
		return result;
	}

	bool ISMS.IsError(bool ShowMsgBox)
	{
		//ILSpy generated this explicit interface implementation from .override directive in IsError
		return this.IsError(ShowMsgBox);
	}

	public bool IsError(bool ShowMsgBox, string MsgBoxTitle)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		string text = "";
		bool result = false;
		if (gm.z != 0)
		{
			if (ShowMsgBox)
			{
				text = "ERROR " + gm.z + ": " + gm.y;
				try
				{
					Interaction.MsgBox((object)text, (MsgBoxStyle)16, (object)MsgBoxTitle);
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					ProjectData.ClearProjectError();
				}
			}
			result = true;
		}
		return result;
	}

	bool ISMS.IsError(bool ShowMsgBox, string MsgBoxTitle)
	{
		//ILSpy generated this explicit interface implementation from .override directive in IsError
		return this.IsError(ShowMsgBox, MsgBoxTitle);
	}

	public string Command(string ATCommand)
	{
		c c = new c();
		c.d = ATCommand;
		ThreadPool.RegisterWaitForSingleObject(gm.dz, e, c, -1, executeOnlyOnce: true);
		while (c.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		string result = c.d;
		a();
		return result;
	}

	string ISMS.Command(string ATCommand)
	{
		//ILSpy generated this explicit interface implementation from .override directive in Command
		return this.Command(ATCommand);
	}

	private void a(NewMessageReceivedEventArgs A_0)
	{
		this.m_a?.Invoke(this, A_0);
	}

	private void a(NewDeliveryReportEventArgs A_0)
	{
		this.m_b?.Invoke(this, A_0);
	}

	private void a(NewIncomingCallEventArgs A_0)
	{
		this.m_c?.Invoke(this, A_0);
	}

	private void a(NewUSSDReceivedEventArgs A_0)
	{
		this.m_d?.Invoke(this, A_0);
	}

	private void aa(object A_0, bool A_1)
	{
		b b = (b)A_0;
		try
		{
			gm.aw();
			string text = "";
			b.i = "-1";
			if (gm.j(b.b) || gm.f(b.b))
			{
				text = gm.a(b.b, b.c, b.d, b.e, b.f, b.g, b.h);
				if (text.Length != 0)
				{
					b.i = text;
					return;
				}
				if (gm.b4 == 0 && gm.z == 0)
				{
					gm.b4 = 4006L;
					gm.b3 = "Send message error";
				}
				if (gm.b4 > 0)
				{
					gm.a(gm.b4, gm.b3, "SMSSEND");
				}
			}
			else
			{
				gm.a(4007L, "Invalid destination number", "SMSSEND");
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			gm.a.b("[SMS:SendSMS_Method:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			b.a = false;
			gm.dz.Set();
		}
	}

	private void z(object A_0, bool A_1)
	{
		b b = (b)A_0;
		try
		{
			gm.aw();
			if (!gm.af)
			{
				this.q.a();
			}
			if (Operators.CompareString(gm.ah, gm.cl, false) == 0)
			{
				gm.a(4020L, gm.l("odumu5VOg_:5-26cCP/GtqPa6g$"), "SMSSEND");
			}
			else if (gm.j(b.b) || gm.f(b.b))
			{
				b.i = objQueue.a(b.b, b.c, b.j, b.d, b.e, b.f, b.g, b.h);
			}
			else
			{
				gm.a(4007L, "Invalid destination number", "SMSSEND");
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			gm.a.b("[SMS:SendSMSToQueue_Method:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			b.a = false;
			gm.dz.Set();
		}
	}

	private void y(object A_0, bool A_1)
	{
		a a = (a)A_0;
		try
		{
			string text = "";
			string text2 = gm.l("O\u0017k.\u0017T0FNjD~{p\r=+6`%\\-P\u00059@GC");
			a.f = "-1";
			gm.aw();
			if (!gm.af)
			{
				this.q.a();
			}
			if (Operators.CompareString(gm.ah, gm.cl, false) == 0)
			{
				gm.a(4041L, gm.l("odumu5VOg_:5-26cCP/GtqPa6g$"), "VCARD");
				return;
			}
			if (a.g < 0 || a.g > 65535)
			{
				gm.a(4039L, "Invalid smart message source port", "SMSSEND");
				return;
			}
			if (a.h < 0 || a.h > 65535)
			{
				gm.a(4040L, "Invalid smart message destination port", "SMSSEND");
				return;
			}
			if (!gm.j(a.b) && !gm.f(a.b))
			{
				gm.a(4038L, "Invalid destination number", "SMSSEND");
				return;
			}
			if (Operators.CompareString(gm.ah, gm.ck, false) == 0)
			{
				a a2 = a;
				a2.c += text2;
			}
			text = "";
			text = gm.a(a.c, a.b, a.e, a.d, a.g, a.h);
			if (text.Length != 0)
			{
				a.f = text;
				return;
			}
			if (gm.ci == 0 && gm.z == 0)
			{
				gm.ci = 4037L;
				gm.ch = "Send smart message error";
			}
			if (gm.ci > 0)
			{
				gm.a(gm.ci, gm.ch, "SMSSEND");
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			gm.a.b("[SMS:SendSmartMessage_Method:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			a.a = false;
			gm.dz.Set();
		}
	}

	private void x(object A_0, bool A_1)
	{
		a a = (a)A_0;
		try
		{
			gm.aw();
			if (!gm.af)
			{
				this.q.a();
			}
			if (Operators.CompareString(gm.ah, gm.cl, false) == 0)
			{
				gm.a(4042L, gm.l("odumu5VOg_:5-26cCP/GtqPa6g$"), "SMSSEND");
			}
			else if (a.g < 0 || a.g > 65535)
			{
				gm.a(4043L, "Invalid smart message source port", "SMSSEND");
			}
			else if (a.h < 0 || a.h > 65535)
			{
				gm.a(4044L, "Invalid smart message destination port", "SMSSEND");
			}
			else if (!gm.j(a.b) && !gm.f(a.b))
			{
				gm.a(4045L, "Invalid destination number", "SMSSEND");
			}
			else
			{
				a.f = objQueue.a(a.b, a.c, a.i, a.d, a.e, a.h, a.g);
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			gm.a.b("[SMS:SendSmartMessageToQueue_Method:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			a.a = false;
			gm.dz.Set();
		}
	}

	private void w(object A_0, bool A_1)
	{
		d d = (d)A_0;
		try
		{
			gm.aw();
			d.c = gm.e(d.b);
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			gm.a.b("[SMS:SendUSSD_Method:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			d.a = false;
			gm.dz.Set();
		}
	}

	private void v(object A_0, bool A_1)
	{
		c c = (c)A_0;
		try
		{
			gm.aw();
			string text = gm.u();
			if (text.Length > 0)
			{
				switch (text.ToUpper())
				{
				case "SM":
					c.c = MessageMemory.SM;
					break;
				case "ME":
					c.c = MessageMemory.ME;
					break;
				case "MT":
					c.c = MessageMemory.MT;
					break;
				}
			}
			else if (gm.z == 0)
			{
				gm.a(2001L, "Cannot read message memory setting", "GENERAL");
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			ProjectData.ClearProjectError();
		}
		finally
		{
			c.a = false;
			gm.dz.Set();
		}
	}

	private void u(object A_0, bool A_1)
	{
		c c = (c)A_0;
		try
		{
			gm.aw();
			string text = "SM";
			switch (c.c)
			{
			case MessageMemory.SM:
				text = "SM";
				break;
			case MessageMemory.ME:
				text = "ME";
				break;
			case MessageMemory.MT:
				text = "MT";
				break;
			}
			switch (text)
			{
			case "SM":
			case "ME":
			case "MT":
				if (!gm.i(text))
				{
					gm.a(2002L, "Error setting selected message memory", "GENERAL");
				}
				break;
			default:
				if (gm.z == 0)
				{
					gm.a(2003L, "Invalid message memory setting", "GENERAL");
				}
				break;
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			ProjectData.ClearProjectError();
		}
		finally
		{
			c.a = false;
			gm.dz.Set();
		}
	}

	private void t(object A_0, bool A_1)
	{
		c c = (c)A_0;
		try
		{
			gm.aw();
			c.b = gm.v();
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			ProjectData.ClearProjectError();
		}
		finally
		{
			c.a = false;
			gm.dz.Set();
		}
	}

	private void s(object A_0, bool A_1)
	{
		c c = (c)A_0;
		try
		{
			gm.aw();
			c.b = gm.ak();
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			ProjectData.ClearProjectError();
		}
		finally
		{
			c.a = false;
			gm.dz.Set();
		}
	}

	private void r(object A_0, bool A_1)
	{
		c c = (c)A_0;
		try
		{
			gm.aw();
			c.b = gm.aa();
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			ProjectData.ClearProjectError();
		}
		finally
		{
			c.a = false;
			gm.dz.Set();
		}
	}

	private void q(object A_0, bool A_1)
	{
		c c = (c)A_0;
		try
		{
			gm.aw();
			string text = gm.t();
			if (text.Length > 0)
			{
				c.d = text;
			}
			else if (gm.z == 0)
			{
				gm.a(2005L, "Cannot read modem manufacturer", "GENERAL");
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			gm.a.b("[SMS:Manufacturer_Get:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			c.a = false;
			gm.dz.Set();
		}
	}

	private void p(object A_0, bool A_1)
	{
		c c = (c)A_0;
		try
		{
			gm.aw();
			string text = gm.ag();
			if (text.Length > 0)
			{
				c.d = text;
			}
			else if (gm.z == 0)
			{
				gm.a(2006L, "Cannot read modem model", "GENERAL");
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			gm.a.b("[SMS:Model_Get:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			c.a = false;
			gm.dz.Set();
		}
	}

	private void o(object A_0, bool A_1)
	{
		c c = (c)A_0;
		try
		{
			gm.aw();
			string text = gm.au();
			if (text.Length > 0)
			{
				c.d = text;
			}
			else if (gm.z == 0)
			{
				gm.a(2007L, "Cannot read modem IMEI", "GENERAL");
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			gm.a.b("[SMS:IMEI_Get:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			c.a = false;
			gm.dz.Set();
		}
	}

	private void n(object A_0, bool A_1)
	{
		c c = (c)A_0;
		try
		{
			gm.aw();
			string text = gm.w();
			if (text.Length > 0)
			{
				c.d = text;
			}
			else if (gm.z == 0)
			{
				gm.a(2029L, "Cannot read IMSI information", "GENERAL");
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			gm.a.b("[SMS:IMSI_Get:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			c.a = false;
			gm.dz.Set();
		}
	}

	private void m(object A_0, bool A_1)
	{
		c c = (c)A_0;
		try
		{
			gm.aw();
			string text = gm.ai();
			if (text.Length > 0)
			{
				c.d = text;
			}
			else if (gm.z == 0)
			{
				gm.a(2008L, "Cannot read modem firmware revision", "GENERAL");
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			gm.a.b("[SMS:Revision_Get:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			c.a = false;
			gm.dz.Set();
		}
	}

	private void l(object A_0, bool A_1)
	{
		c c = (c)A_0;
		try
		{
			c.e = -1;
			gm.aw();
			int num = gm.ar();
			if (num != 99)
			{
				c.e = gm.bp;
			}
			else if (gm.z == 0)
			{
				gm.a(3001L, "Signal strength not known or not detectable", "GENERAL");
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			gm.a.b("[SMS:SignalStrength_Get:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			c.a = false;
			gm.dz.Set();
		}
	}

	private void k(object A_0, bool A_1)
	{
		c c = (c)A_0;
		try
		{
			c.e = 99;
			gm.aw();
			if ((c.e = gm.ar()) == 99 && gm.z == 0)
			{
				gm.a(3001L, "Signal strength not known or not detectable", "GENERAL");
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			gm.a.b("[SMS:SignalStrengthDB_Get:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			c.a = false;
			gm.dz.Set();
		}
	}

	private void j(object A_0, bool A_1)
	{
		c c = (c)A_0;
		try
		{
			gm.aw();
			if ((c.d = gm.aq()).Length == 0 && gm.z == 0)
			{
				gm.a(2024L, "Unable to read SMSC from SIM", "GENERAL");
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			gm.a.b("[SMS:SMSC_Get:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			c.a = false;
			gm.dz.Set();
		}
	}

	private void i(object A_0, bool A_1)
	{
		c c = (c)A_0;
		try
		{
			gm.aw();
			if (c.d == null)
			{
				c.d = "";
			}
			if (!gm.o(c.d) && gm.z == 0)
			{
				gm.a(2025L, "Unable to save SMSC", "GENERAL");
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			gm.a.b("[SMS:SMSC_Set:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			c.a = false;
			gm.dz.Set();
		}
	}

	private void h(object A_0, bool A_1)
	{
		c c = (c)A_0;
		try
		{
			gm.aw();
			if ((c.d = gm.at()).Length == 0 && gm.z == 0)
			{
				gm.a(3009L, "Unable to read network information", "GENERAL");
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			gm.a.b("[SMS:Network_Get:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			c.a = false;
			gm.dz.Set();
		}
	}

	private void g(object A_0, bool A_1)
	{
		c c = (c)A_0;
		try
		{
			gm.aw();
			gm.be = -1;
			gm.af();
			if (gm.be < 0 && gm.z == 0)
			{
				gm.a(2026L, "Cannot read battery charge level", "GENERAL");
			}
			c.e = gm.be;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			gm.a.b("[SMS:BatteryLevel_Get:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			c.a = false;
			gm.dz.Set();
		}
	}

	private void f(object A_0, bool A_1)
	{
		c c = (c)A_0;
		try
		{
			gm.aw();
			gm.bf = "";
			gm.ab();
			if (gm.bf.Length <= 0 && gm.z == 0)
			{
				gm.a(2027L, "Cannot read own number", "GENERAL");
			}
			c.d = gm.bf;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			gm.a.b("[SMS:OwnNumber_Get:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			c.a = false;
			gm.dz.Set();
		}
	}

	private void e(object A_0, bool A_1)
	{
		c c = (c)A_0;
		try
		{
			string text = c.d.Trim();
			c.d = "";
			if (text.Length > 0 && !text.EndsWith("\u001a") && !text.EndsWith("\r"))
			{
				text += "\r";
			}
			gm.aw();
			if (text.Length > 0)
			{
				c.d = gm.n(text);
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			ProjectData.ClearProjectError();
		}
		finally
		{
			c.a = false;
			gm.dz.Set();
		}
	}

	private void d(object A_0, bool A_1)
	{
		c c = (c)A_0;
		try
		{
			gm.aw();
			gm.bu = c.f;
			gm.u = c.f;
			gm.b(2);
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			ProjectData.ClearProjectError();
		}
		finally
		{
			c.a = false;
			gm.dz.Set();
		}
	}

	private void c(object A_0, bool A_1)
	{
		c c = (c)A_0;
		try
		{
			gm.aw();
			gm.s = c.f;
			gm.b(1);
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			ProjectData.ClearProjectError();
		}
		finally
		{
			c.a = false;
			gm.dz.Set();
		}
	}

	private void b(object A_0, bool A_1)
	{
		c c = (c)A_0;
		try
		{
			gm.aw();
			gm.x = c.f;
			gm.b(3);
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			ProjectData.ClearProjectError();
		}
		finally
		{
			c.a = false;
			gm.dz.Set();
		}
	}

	private void a(object A_0, bool A_1)
	{
		c c = (c)A_0;
		try
		{
			gm.aw();
			gm.v = c.f;
			gm.r();
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			ProjectData.ClearProjectError();
		}
		finally
		{
			c.a = false;
			gm.dz.Set();
		}
	}

	private void a()
	{
		if (gm.ac.Length > 0)
		{
			switch (gm.ac.ToUpper())
			{
			case "GENERAL":
				throw new GeneralException("ERROR " + gm.z + ": " + gm.y);
			case "SMSSEND":
				throw new SMSSendException("ERROR " + gm.z + ": " + gm.y);
			case "SMSREAD":
				throw new SMSReadException("ERROR " + gm.z + ": " + gm.y);
			case "SMSDELETE":
				throw new SMSDeleteException("ERROR " + gm.z + ": " + gm.y);
			case "WAPPUSH":
				throw new WapPushException("ERROR " + gm.z + ": " + gm.y);
			case "VCARD":
				throw new vCardException("ERROR " + gm.z + ": " + gm.y);
			case "VCALENDAR":
				throw new vCalendarException("ERROR " + gm.z + ": " + gm.y);
			}
		}
	}

	private string a(string A_0)
	{
		string text = "";
		try
		{
			UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			byte[] array = mD5CryptoServiceProvider.ComputeHash(unicodeEncoding.GetBytes(A_0));
			text = BitConverter.ToString(array).Replace("-", "");
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			gm.a.b("[SMS:MD5Encrypt:1] " + ex2.Message);
			text = "";
			ProjectData.ClearProjectError();
		}
		return text;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!this.m_y && disposing && gm != null)
		{
			gm.ae();
			gm = null;
		}
		this.m_y = true;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	void IDisposable.Dispose()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Dispose
		this.Dispose();
	}

	protected virtual void Finalize()
	{
		Dispose(disposing: false);
		base.Finalize();
	}
}
