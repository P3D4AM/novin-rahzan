using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace mCore;

public class Queue : IQueue
{
	internal delegate void d(QueueSMSSentEventArgs A_0);

	internal delegate void c(QueueWapPushSentEventArgs A_0);

	internal delegate void i(QueueVCardSentEventArgs A_0);

	internal delegate void j(QueueVCalendarSentEventArgs A_0);

	internal delegate void g(QueueSmartMessageSentEventArgs A_0);

	internal delegate void f(QueueSMSSendingEventArgs A_0);

	internal delegate void e(QueueWapPushSendingEventArgs A_0);

	internal delegate void h(QueueVCardSendingEventArgs A_0);

	internal delegate void a(QueueVCalendarSendingEventArgs A_0);

	internal delegate void b(QueueSmartMessageSendingEventArgs A_0);

	private class k
	{
		internal bool a;

		internal global::h b;

		public k(global::h A_0)
		{
			a = true;
			b = A_0;
		}
	}

	private global::f m_a;

	private SMS m_b;

	private Collection m_c;

	private Collection m_d;

	private Collection m_e;

	private Collection m_f;

	private Collection m_g;

	private Collection m_h;

	private int m_i;

	private bool m_j;

	private bool m_k;

	private d l;

	private c m;

	private i n;

	private j o;

	private g p;

	private f q;

	private e r;

	private h s;

	private a t;

	private b u;

	private const int v = 1;

	public bool Enabled
	{
		get
		{
			return this.m_k;
		}
		set
		{
			this.m_k = value;
			if (this.m_k)
			{
				this.m_b.r.b("Message queue enabled");
			}
			else
			{
				this.m_b.r.b("Message queue disabled");
			}
			if ((this.m_c.Count > 0 || this.m_d.Count > 0 || this.m_e.Count > 0) && this.m_k && !this.m_j)
			{
				Thread thread = new Thread(b);
				thread.Name = "SendQueueThread";
				thread.Start();
			}
		}
	}

	public int Count
	{
		get
		{
			int num = 0;
			num = checked(this.m_c.Count + this.m_d.Count + this.m_e.Count);
			if (num > 0)
			{
				this.m_b.r.b(num + " unsent messages in queue");
			}
			else
			{
				this.m_b.r.b("Message queue is empty");
			}
			return num;
		}
	}

	internal event d QSMSSent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		add
		{
			l = (d)Delegate.Combine(l, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		remove
		{
			l = (d)Delegate.Remove(l, value);
		}
	}

	internal event c QWapPushSent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		add
		{
			m = (c)Delegate.Combine(m, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		remove
		{
			m = (c)Delegate.Remove(m, value);
		}
	}

	internal event i QVCardSent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		add
		{
			n = (i)Delegate.Combine(n, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		remove
		{
			n = (i)Delegate.Remove(n, value);
		}
	}

	internal event j QVCalendarSent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		add
		{
			o = (j)Delegate.Combine(o, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		remove
		{
			o = (j)Delegate.Remove(o, value);
		}
	}

	internal event g QSmartMessageSent
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		add
		{
			p = (g)Delegate.Combine(p, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		remove
		{
			p = (g)Delegate.Remove(p, value);
		}
	}

	internal event f QSMSSending
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		add
		{
			q = (f)Delegate.Combine(q, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		remove
		{
			q = (f)Delegate.Remove(q, value);
		}
	}

	internal event e QWapPushSending
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		add
		{
			r = (e)Delegate.Combine(r, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		remove
		{
			r = (e)Delegate.Remove(r, value);
		}
	}

	internal event h QVCardSending
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		add
		{
			s = (h)Delegate.Combine(s, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		remove
		{
			s = (h)Delegate.Remove(s, value);
		}
	}

	internal event a QVCalendarSending
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		add
		{
			t = (a)Delegate.Combine(t, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		remove
		{
			t = (a)Delegate.Remove(t, value);
		}
	}

	internal event b QSmartMessageSending
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		add
		{
			u = (b)Delegate.Combine(u, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		remove
		{
			u = (b)Delegate.Remove(u, value);
		}
	}

	internal Queue(ref SMS A_0)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		this.m_c = new Collection();
		this.m_d = new Collection();
		this.m_e = new Collection();
		this.m_f = new Collection();
		this.m_g = new Collection();
		this.m_h = new Collection();
		this.m_i = 0;
		this.m_j = false;
		this.m_k = false;
		this.m_b = A_0;
		this.m_a = this.m_b.gm;
	}

	public bool Clear()
	{
		bool result;
		try
		{
			if (Monitor.TryEnter(this.m_f, this.m_a.a4))
			{
				try
				{
					this.m_c.Clear();
				}
				finally
				{
					Monitor.Exit(this.m_f);
				}
			}
			else
			{
				this.m_a.a(9011L, "Thread monitor enter attempt timeout [Queue:Clear:A]", "GENERAL");
			}
			if (Monitor.TryEnter(this.m_g, this.m_a.a4))
			{
				try
				{
					this.m_d.Clear();
				}
				finally
				{
					Monitor.Exit(this.m_g);
				}
			}
			else
			{
				this.m_a.a(9011L, "Thread monitor enter attempt timeout [Queue:Clear:B]", "GENERAL");
			}
			if (Monitor.TryEnter(this.m_h, this.m_a.a4))
			{
				try
				{
					this.m_e.Clear();
				}
				finally
				{
					Monitor.Exit(this.m_h);
				}
			}
			else
			{
				this.m_a.a(9011L, "Thread monitor enter attempt timeout [Queue:Clear:C]", "GENERAL");
			}
			this.m_b.r.b("Unsent messages cleared from message queue");
			result = true;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.m_b.r.b("Error encountered clearing message queue");
			result = false;
			ProjectData.ClearProjectError();
		}
		return result;
	}

	bool IQueue.Clear()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Clear
		return this.Clear();
	}

	internal string a(string A_0, string A_1, int A_2, bool A_3, int A_4, int A_5, bool A_6, int A_7)
	{
		if (Monitor.TryEnter(this.m_f, this.m_a.a4))
		{
			global::h h = new global::h("SMS", A_2, A_0, A_1, A_3, A_4, A_5, A_6, A_7, "", "", DateTime.MinValue, "", "", "", "", a(), 0, 0);
			string text = h.b;
			try
			{
				switch (A_2)
				{
				case 1:
					this.m_c.Add((object)h, text, (object)null, (object)null);
					break;
				case 2:
					if (Monitor.TryEnter(this.m_g, this.m_a.a4))
					{
						try
						{
							this.m_d.Add((object)h, text, (object)null, (object)null);
						}
						finally
						{
							Monitor.Exit(this.m_g);
						}
					}
					else
					{
						this.m_a.a(9011L, "Thread monitor enter attempt timeout [Queue:AddSMS:B]", "GENERAL");
					}
					break;
				case 3:
					if (Monitor.TryEnter(this.m_h, this.m_a.a4))
					{
						try
						{
							this.m_e.Add((object)h, text, (object)null, (object)null);
						}
						finally
						{
							Monitor.Exit(this.m_h);
						}
					}
					else
					{
						this.m_a.a(9011L, "Thread monitor enter attempt timeout [Queue:AddSMS:C]", "GENERAL");
					}
					break;
				}
				h = null;
				this.m_b.r.b("SMS message added to queue [Dest: " + A_0 + "; Priority: " + ((QueuePriority)A_2).ToString() + "; QueueID: " + text + "]");
				if ((this.m_c.Count > 0 || this.m_d.Count > 0 || this.m_c.Count > 0) && this.m_k && !this.m_j)
				{
					Thread thread = new Thread(b);
					thread.Name = "SendQueueThread";
					thread.Start();
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				this.m_b.r.b("[Queue:AddSMS:1] " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			finally
			{
				Monitor.Exit(this.m_f);
			}
			return text;
		}
		this.m_a.a(9011L, "Thread monitor enter attempt timeout [Queue:AddSMS:A]", "GENERAL");
		return "";
	}

	internal string a(string A_0, string A_1, string A_2, DateTime A_3, string A_4, string A_5, int A_6, int A_7, bool A_8)
	{
		if (Monitor.TryEnter(this.m_f, this.m_a.a4))
		{
			global::h h = new global::h("WAPPUSH", A_6, A_0, "", A_4: false, this.m_a.bx, this.m_a.by, A_8, A_7, A_1, A_2, A_3, A_4, A_5, "", "", a(), 0, 0);
			string text = h.b;
			try
			{
				switch (A_6)
				{
				case 1:
					this.m_c.Add((object)h, text, (object)null, (object)null);
					break;
				case 2:
					if (Monitor.TryEnter(this.m_g, this.m_a.a4))
					{
						try
						{
							this.m_d.Add((object)h, text, (object)null, (object)null);
						}
						finally
						{
							Monitor.Exit(this.m_g);
						}
					}
					else
					{
						this.m_a.a(9011L, "Thread monitor enter attempt timeout [Queue:AddWapPush:B]", "GENERAL");
					}
					break;
				case 3:
					if (Monitor.TryEnter(this.m_h, this.m_a.a4))
					{
						try
						{
							this.m_e.Add((object)h, text, (object)null, (object)null);
						}
						finally
						{
							Monitor.Exit(this.m_h);
						}
					}
					else
					{
						this.m_a.a(9011L, "Thread monitor enter attempt timeout [Queue:AddWapPush:C]", "GENERAL");
					}
					break;
				}
				h = null;
				this.m_b.r.b("WAP Push message added to queue [Dest: " + A_0 + "; Priority: " + ((QueuePriority)A_6).ToString() + "; QueueID: " + text + "]");
				if ((this.m_c.Count > 0 || this.m_d.Count > 0 || this.m_c.Count > 0) && this.m_k && !this.m_j)
				{
					Thread thread = new Thread(b);
					thread.Name = "SendQueueThread";
					thread.Start();
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				this.m_b.r.b("[Queue:AddWapPush:1] " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			finally
			{
				Monitor.Exit(this.m_f);
			}
			return text;
		}
		this.m_a.a(9011L, "Thread monitor enter attempt timeout [Queue:AddWapPush:A]", "GENERAL");
		return "";
	}

	internal string a(string A_0, string A_1, int A_2, int A_3, bool A_4)
	{
		if (Monitor.TryEnter(this.m_f, this.m_a.a4))
		{
			global::h h = new global::h("VCARD", A_2, A_0, "", A_4: false, this.m_a.bx, this.m_a.by, A_4, A_3, "", "", DateTime.MinValue, "", "", A_1, "", a(), 0, 0);
			string text = h.b;
			try
			{
				switch (A_2)
				{
				case 1:
					this.m_c.Add((object)h, text, (object)null, (object)null);
					break;
				case 2:
					if (Monitor.TryEnter(this.m_g, this.m_a.a4))
					{
						try
						{
							this.m_d.Add((object)h, text, (object)null, (object)null);
						}
						finally
						{
							Monitor.Exit(this.m_g);
						}
					}
					else
					{
						this.m_a.a(9011L, "Thread monitor enter attempt timeout [Queue:AddVCard:B]", "GENERAL");
					}
					break;
				case 3:
					if (Monitor.TryEnter(this.m_h, this.m_a.a4))
					{
						try
						{
							this.m_e.Add((object)h, text, (object)null, (object)null);
						}
						finally
						{
							Monitor.Exit(this.m_h);
						}
					}
					else
					{
						this.m_a.a(9011L, "Thread monitor enter attempt timeout [Queue:AddVCard:C]", "GENERAL");
					}
					break;
				}
				h = null;
				this.m_b.r.b("vCard message added to queue [Dest: " + A_0 + "; Priority: " + ((QueuePriority)A_2).ToString() + "; QueueID: " + text + "]");
				if ((this.m_c.Count > 0 || this.m_d.Count > 0 || this.m_c.Count > 0) && this.m_k && !this.m_j)
				{
					Thread thread = new Thread(b);
					thread.Name = "SendQueueThread";
					thread.Start();
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				this.m_b.r.b("[Queue:AddVCard:1] " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			finally
			{
				Monitor.Exit(this.m_f);
			}
			return text;
		}
		this.m_a.a(9011L, "Thread monitor enter attempt timeout [Queue:AddVCard:A]", "GENERAL");
		return "";
	}

	internal string b(string A_0, string A_1, int A_2, int A_3, bool A_4)
	{
		if (Monitor.TryEnter(this.m_f, this.m_a.a4))
		{
			global::h h = new global::h("VCALENDAR", A_2, A_0, "", A_4: false, this.m_a.bx, this.m_a.by, A_4, A_3, "", "", DateTime.MinValue, "", "", "", A_1, a(), 0, 0);
			string text = h.b;
			try
			{
				switch (A_2)
				{
				case 1:
					this.m_c.Add((object)h, text, (object)null, (object)null);
					break;
				case 2:
					if (Monitor.TryEnter(this.m_g, this.m_a.a4))
					{
						try
						{
							this.m_d.Add((object)h, text, (object)null, (object)null);
						}
						finally
						{
							Monitor.Exit(this.m_g);
						}
					}
					else
					{
						this.m_a.a(9011L, "Thread monitor enter attempt timeout [Queue:AddVCalendar:B]", "GENERAL");
					}
					break;
				case 3:
					if (Monitor.TryEnter(this.m_h, this.m_a.a4))
					{
						try
						{
							this.m_e.Add((object)h, text, (object)null, (object)null);
						}
						finally
						{
							Monitor.Exit(this.m_h);
						}
					}
					else
					{
						this.m_a.a(9011L, "Thread monitor enter attempt timeout [Queue:AddVCalendar:C]", "GENERAL");
					}
					break;
				}
				h = null;
				this.m_b.r.b("vCalendar message added to queue [Dest: " + A_0 + "; Priority: " + ((QueuePriority)A_2).ToString() + "; QueueID: " + text + "]");
				if ((this.m_c.Count > 0 || this.m_d.Count > 0 || this.m_c.Count > 0) && this.m_k && !this.m_j)
				{
					Thread thread = new Thread(b);
					thread.Name = "SendQueueThread";
					thread.Start();
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				this.m_b.r.b("[Queue:AddVCalendar:1] " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			finally
			{
				Monitor.Exit(this.m_f);
			}
			return text;
		}
		this.m_a.a(9011L, "Thread monitor enter attempt timeout [Queue:AddVCalendar:A]", "GENERAL");
		return "";
	}

	internal string a(string A_0, string A_1, int A_2, bool A_3, int A_4, int A_5, int A_6)
	{
		if (Monitor.TryEnter(this.m_f, this.m_a.a4))
		{
			global::h h = new global::h("SMART", A_2, A_0, A_1, A_4: false, 7, 3, A_3, A_4, "", "", DateTime.MinValue, "", "", "", "", a(), A_6, A_5);
			string text = h.b;
			try
			{
				switch (A_2)
				{
				case 1:
					this.m_c.Add((object)h, text, (object)null, (object)null);
					break;
				case 2:
					if (Monitor.TryEnter(this.m_g, this.m_a.a4))
					{
						try
						{
							this.m_d.Add((object)h, text, (object)null, (object)null);
						}
						finally
						{
							Monitor.Exit(this.m_g);
						}
					}
					else
					{
						this.m_a.a(9011L, "Thread monitor enter attempt timeout [Queue:AddSMS:B]", "GENERAL");
					}
					break;
				case 3:
					if (Monitor.TryEnter(this.m_h, this.m_a.a4))
					{
						try
						{
							this.m_e.Add((object)h, text, (object)null, (object)null);
						}
						finally
						{
							Monitor.Exit(this.m_h);
						}
					}
					else
					{
						this.m_a.a(9011L, "Thread monitor enter attempt timeout [Queue:AddSMS:C]", "GENERAL");
					}
					break;
				}
				h = null;
				this.m_b.r.b("Smart message added to queue [Dest: " + A_0 + "; Priority: " + ((QueuePriority)A_2).ToString() + "; QueueID: " + text + "]");
				if ((this.m_c.Count > 0 || this.m_d.Count > 0 || this.m_c.Count > 0) && this.m_k && !this.m_j)
				{
					Thread thread = new Thread(b);
					thread.Name = "SendQueueThread";
					thread.Start();
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				this.m_b.r.b("[Queue:AddSmartMessage:1] " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			finally
			{
				Monitor.Exit(this.m_f);
			}
			return text;
		}
		this.m_a.a(9011L, "Thread monitor enter attempt timeout [Queue:AddSmartMessage:A]", "GENERAL");
		return "";
	}

	private void b()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		if (this.m_j)
		{
			return;
		}
		this.m_j = true;
		this.m_b.r.b("Message queue sending started");
		try
		{
			while ((this.m_c.Count > 0 || this.m_d.Count > 0 || this.m_e.Count > 0) && this.m_k)
			{
				bool flag = false;
				global::h h = null;
				try
				{
					if (this.m_c.Count > 0)
					{
						if (Monitor.TryEnter(this.m_f, this.m_a.a4))
						{
							try
							{
								h = (global::h)this.m_c[1];
								this.m_c.Remove(1);
								flag = true;
							}
							finally
							{
								Monitor.Exit(this.m_f);
							}
						}
						else
						{
							this.m_a.a(9011L, "Thread monitor enter attempt timeout [Queue:SendQueue:A]", "GENERAL");
						}
					}
					else if (!flag && this.m_d.Count > 0)
					{
						if (Monitor.TryEnter(this.m_g, this.m_a.a4))
						{
							try
							{
								h = (global::h)this.m_d[1];
								this.m_d.Remove(1);
								flag = true;
							}
							finally
							{
								Monitor.Exit(this.m_g);
							}
						}
						else
						{
							this.m_a.a(9011L, "Thread monitor enter attempt timeout [Queue:SendQueue:B]", "GENERAL");
						}
					}
					else if (!flag && this.m_e.Count > 0)
					{
						if (Monitor.TryEnter(this.m_h, this.m_a.a4))
						{
							try
							{
								h = (global::h)this.m_e[1];
								this.m_e.Remove(1);
								flag = true;
							}
							finally
							{
								Monitor.Exit(this.m_h);
							}
						}
						else
						{
							this.m_a.a(9011L, "Thread monitor enter attempt timeout [Queue:SendQueue:C]", "GENERAL");
						}
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					this.m_b.r.b("[Queue:SendQueue:1] " + ex2.Message);
					ProjectData.ClearProjectError();
				}
				k k = new k(h);
				switch (h.a.ToUpper())
				{
				case "SMS":
					ThreadPool.RegisterWaitForSingleObject(this.m_a.dz, e, k, -1, executeOnlyOnce: true);
					while (k.a)
					{
						Thread.Sleep(1);
						Application.DoEvents();
					}
					break;
				case "SMART":
					ThreadPool.RegisterWaitForSingleObject(this.m_a.dz, d, k, -1, executeOnlyOnce: true);
					while (k.a)
					{
						Thread.Sleep(1);
						Application.DoEvents();
					}
					break;
				case "WAPPUSH":
					ThreadPool.RegisterWaitForSingleObject(this.m_a.dz, c, k, -1, executeOnlyOnce: true);
					while (k.a)
					{
						Thread.Sleep(1);
						Application.DoEvents();
					}
					break;
				case "VCARD":
					ThreadPool.RegisterWaitForSingleObject(this.m_a.dz, b, k, -1, executeOnlyOnce: true);
					while (k.a)
					{
						Thread.Sleep(1);
						Application.DoEvents();
					}
					break;
				case "VCALENDAR":
					ThreadPool.RegisterWaitForSingleObject(this.m_a.dz, a, k, -1, executeOnlyOnce: true);
					while (k.a)
					{
						Thread.Sleep(1);
						Application.DoEvents();
					}
					break;
				}
			}
		}
		catch (Exception ex3)
		{
			ProjectData.SetProjectError(ex3);
			Exception ex4 = ex3;
			ProjectData.ClearProjectError();
		}
		finally
		{
			this.m_j = false;
		}
	}

	private void e(object A_0, bool A_1)
	{
		k k = (k)A_0;
		string text = "";
		bool a_ = false;
		string text2 = "";
		long b = default(long);
		try
		{
			text = "";
			a_ = false;
			this.m_b.r.b("Sending queued SMS to " + k.b.d + " [Priority: " + ((QueuePriority)k.b.c).ToString() + "; QueueID: " + k.b.b + "]");
			QueueSMSSendingEventArgs a_2 = new QueueSMSSendingEventArgs(k.b.d, (QueuePriority)k.b.c, k.b.b);
			q?.Invoke(a_2);
			text = this.m_a.a(k.b.d, k.b.e, k.b.f, k.b.g, k.b.h, k.b.i, k.b.j);
			if (text.Length == 0)
			{
				text = "-1";
				if (this.m_a.b4 == 0)
				{
					if (this.m_a.z == 0)
					{
						this.m_a.b4 = 4006L;
						this.m_a.b3 = "Send message error";
					}
					else
					{
						this.m_a.b4 = this.m_a.z;
						this.m_a.b3 = this.m_a.y;
					}
				}
				b = this.m_a.b4;
				text2 = this.m_a.b3;
				if (b > 0)
				{
					this.m_b.r.a(b, text2);
				}
			}
			else
			{
				a_ = true;
				this.m_b.r.b("Queued SMS sent to " + k.b.d + " [QueueID: " + k.b.b + "]");
			}
		}
		finally
		{
			k.a = false;
			this.m_a.dz.Set();
		}
		QueueSMSSentEventArgs a_3 = new QueueSMSSentEventArgs(k.b.b, k.b.d, a_, text, b, text2);
		l?.Invoke(a_3);
	}

	private void d(object A_0, bool A_1)
	{
		k k = (k)A_0;
		string text = "";
		bool a_ = false;
		string text2 = "";
		long ci = default(long);
		try
		{
			text = "";
			a_ = false;
			this.m_b.r.b("Sending queued smart message to " + k.b.d + " [Priority: " + ((QueuePriority)k.b.c).ToString() + "; QueueID: " + k.b.b + "]");
			QueueSmartMessageSendingEventArgs a_2 = new QueueSmartMessageSendingEventArgs(k.b.d, (QueuePriority)k.b.c, k.b.b);
			u?.Invoke(a_2);
			text = this.m_a.a(k.b.e, k.b.d, k.b.j, k.b.i, k.b.r, k.b.s);
			if (text.Length == 0)
			{
				text = (-1).ToString();
				if (this.m_a.ci == 0)
				{
					if (this.m_a.z == 0)
					{
						this.m_a.ci = 4046L;
						this.m_a.ch = "Smart message send error";
					}
					else
					{
						this.m_a.ci = this.m_a.z;
						this.m_a.ch = this.m_a.y;
					}
				}
				ci = this.m_a.ci;
				text2 = this.m_a.ch;
				if (ci > 0)
				{
					this.m_b.r.a(ci, text2);
				}
			}
			else
			{
				a_ = true;
				this.m_b.r.b("Queued smart message sent to " + k.b.d + " [QueueID: " + k.b.b + "]");
			}
		}
		finally
		{
			k.a = false;
			this.m_a.dz.Set();
		}
		QueueSmartMessageSentEventArgs a_3 = new QueueSmartMessageSentEventArgs(k.b.b, k.b.d, a_, text, ci, text2);
		p?.Invoke(a_3);
	}

	private void c(object A_0, bool A_1)
	{
		k k = (k)A_0;
		string text = "";
		bool a_ = false;
		string text2 = "";
		long cc = default(long);
		try
		{
			text = "";
			a_ = false;
			this.m_b.r.b("Sending queued wap push to " + k.b.d + " [Priority: " + ((QueuePriority)k.b.c).ToString() + "; QueueID: " + k.b.b + "]");
			QueueWapPushSendingEventArgs a_2 = new QueueWapPushSendingEventArgs(k.b.d, (QueuePriority)k.b.c, k.b.b);
			r?.Invoke(a_2);
			text = this.m_a.a(k.b.d, k.b.k, k.b.l, k.b.m, k.b.n, k.b.o, k.b.j, k.b.i);
			if (text.Length == 0)
			{
				text = (-1).ToString();
				if (this.m_a.cc == 0)
				{
					if (this.m_a.z == 0)
					{
						this.m_a.cc = 7008L;
						this.m_a.cb = "WAP push send error";
					}
					else
					{
						this.m_a.cc = this.m_a.z;
						this.m_a.cb = this.m_a.y;
					}
				}
				cc = this.m_a.cc;
				text2 = this.m_a.cb;
				if (cc > 0)
				{
					this.m_b.r.a(cc, text2);
				}
			}
			else
			{
				a_ = true;
				this.m_b.r.b("Queued wap push sent to " + k.b.d + " [QueueID: " + k.b.b + "]");
			}
		}
		finally
		{
			k.a = false;
			this.m_a.dz.Set();
		}
		QueueWapPushSentEventArgs a_3 = new QueueWapPushSentEventArgs(k.b.b, k.b.d, a_, text, cc, text2);
		m?.Invoke(a_3);
	}

	private void b(object A_0, bool A_1)
	{
		k k = (k)A_0;
		string text = "";
		bool a_ = false;
		string text2 = "";
		long ce = default(long);
		try
		{
			a_ = false;
			text = "";
			this.m_b.r.b("Sending queued vCard to " + k.b.d + " [Priority: " + ((QueuePriority)k.b.c).ToString() + "; QueueID: " + k.b.b + "]");
			QueueVCardSendingEventArgs a_2 = new QueueVCardSendingEventArgs(k.b.d, (QueuePriority)k.b.c, k.b.b);
			s?.Invoke(a_2);
			text = this.m_a.a(k.b.p, k.b.d, k.b.j, k.b.i);
			if (text.Trim().Length <= 0)
			{
				text = (-1).ToString();
				if (this.m_a.ce == 0)
				{
					if (this.m_a.z == 0)
					{
						this.m_a.ce = 8026L;
						this.m_a.cd = "vCard send error";
					}
					else
					{
						this.m_a.ce = this.m_a.z;
						this.m_a.cd = this.m_a.y;
					}
				}
				ce = this.m_a.ce;
				text2 = this.m_a.cd;
				if (ce > 0)
				{
					this.m_b.r.a(ce, text2);
				}
			}
			else
			{
				a_ = true;
				this.m_b.r.b("Queued vCard sent to " + k.b.d + " [QueueID: " + k.b.b + "]");
			}
		}
		finally
		{
			k.a = false;
			this.m_a.dz.Set();
		}
		QueueVCardSentEventArgs a_3 = new QueueVCardSentEventArgs(k.b.b, k.b.d, a_, text, ce, text2);
		n?.Invoke(a_3);
	}

	private void a(object A_0, bool A_1)
	{
		k k = (k)A_0;
		string text = "";
		bool a_ = false;
		string text2 = "";
		long cg = default(long);
		try
		{
			text = "";
			a_ = false;
			this.m_b.r.b("Sending queued vCalendar to " + k.b.d + " [Priority: " + ((QueuePriority)k.b.c).ToString() + "; QueueID: " + k.b.b + "]");
			QueueVCalendarSendingEventArgs a_2 = new QueueVCalendarSendingEventArgs(k.b.d, (QueuePriority)k.b.c, k.b.b);
			t?.Invoke(a_2);
			text = this.m_a.b(k.b.q, k.b.d, k.b.j, k.b.i);
			if (text.Trim().Length <= 0)
			{
				text = (-1).ToString();
				if (this.m_a.cg == 0)
				{
					if (this.m_a.z == 0)
					{
						this.m_a.cg = 8037L;
						this.m_a.cf = "vCalendar send error";
					}
					else
					{
						this.m_a.cg = this.m_a.z;
						this.m_a.cf = this.m_a.y;
					}
				}
				cg = this.m_a.cg;
				text2 = this.m_a.cf;
				if (cg > 0)
				{
					this.m_b.r.a(cg, text2);
				}
			}
			else
			{
				a_ = true;
				this.m_b.r.b("Queued vCalendar sent to " + k.b.d + " [QueueID: " + k.b.b + "]");
			}
		}
		finally
		{
			k.a = false;
			this.m_a.dz.Set();
		}
		QueueVCalendarSentEventArgs a_3 = new QueueVCalendarSentEventArgs(k.b.b, k.b.d, a_, text, cg, text2);
		o?.Invoke(a_3);
	}

	private string a()
	{
		if (this.m_i >= int.MaxValue)
		{
			this.m_i = 0;
		}
		checked
		{
			this.m_i++;
			return this.m_i.ToString();
		}
	}
}
