using System;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace mCore;

public class WapPush : IWapPush
{
	[Obsolete("mCore.WapPush.EnumAction enumerator was obsoleted in version 1.1. Use mCore.WapPushAction instead", true)]
	public enum EnumAction
	{
		SignalLow = 1,
		SignalMedium,
		SignalHigh
	}

	private class a
	{
		internal bool a;

		internal string b;

		internal string c;

		internal string d;

		internal string e;

		internal string f;

		internal int g;

		internal DateTime h;

		internal bool i;

		internal int j;

		internal string k;

		internal string l;

		internal bool m;

		public a(string A_0, string A_1, string A_2, string A_3, string A_4, DateTime A_5, int A_6, bool A_7, int A_8 = 2)
		{
			a = true;
			b = "";
			c = "";
			d = "";
			e = "0";
			f = "";
			h = default(DateTime);
			i = false;
			k = "";
			l = "";
			m = false;
			d = A_0;
			b = A_1;
			c = A_2;
			e = A_3;
			f = A_4;
			h = A_5;
			g = A_6;
			m = A_7;
			j = A_8;
		}
	}

	private const string m_a = "05";

	private const string m_b = "06";

	private const string c = "07";

	private const string d = "08";

	private const string e = "09";

	private string f;

	private string g;

	private string h;

	private string i;

	private string j;

	private DateTime k;

	private f l;

	private SMS m;

	private const int n = 1;

	public string Destination
	{
		get
		{
			l.aw();
			return h;
		}
		set
		{
			l.aw();
			if (value == null)
			{
				value = "";
			}
			h = value.Trim();
			if (h.Length == 0 || (!l.j(h) && !l.f(h)))
			{
				l.a(7001L, "Invalid destination mobile number", "WAPPUSH");
				h = "";
			}
			a();
		}
	}

	public string Text
	{
		get
		{
			l.aw();
			return f;
		}
		set
		{
			l.aw();
			if (value == null)
			{
				value = "";
			}
			f = value;
			if (value.Length <= 0)
			{
				l.a(7002L, "Text message cannot be zero length", "WAPPUSH");
				f = "";
			}
			a();
		}
	}

	public string URL
	{
		get
		{
			l.aw();
			return g;
		}
		set
		{
			l.aw();
			if (value == null)
			{
				value = "";
			}
			g = value.Trim();
			if (value.Length <= 0)
			{
				l.a(7003L, "Push URL cannot be zero length", "WAPPUSH");
				g = "";
			}
			a();
		}
	}

	public WapPushAction Action
	{
		get
		{
			l.aw();
			WapPushAction wapPushAction = default(WapPushAction);
			return j switch
			{
				"06" => WapPushAction.SignalLow, 
				"07" => WapPushAction.SignalMedium, 
				"08" => WapPushAction.SignalHigh, 
				_ => wapPushAction, 
			};
		}
		set
		{
			l.aw();
			if (value >= WapPushAction.SignalLow && value <= WapPushAction.SignalHigh)
			{
				switch (value)
				{
				case WapPushAction.SignalLow:
					j = "06";
					break;
				case WapPushAction.SignalMedium:
					j = "07";
					break;
				case WapPushAction.SignalHigh:
					j = "08";
					break;
				}
			}
			else
			{
				l.a(7004L, "Invalid action attribute", "WAPPUSH");
				j = "07";
			}
			a();
		}
	}

	public DateTime Created
	{
		get
		{
			l.aw();
			DateTime result = k;
			a();
			return result;
		}
		set
		{
			l.aw();
			if (DateTime.Compare(value, DateTime.MinValue) == 0)
			{
				k = value;
			}
			else if (Information.IsDate((object)value))
			{
				k = value;
			}
			else
			{
				l.a(7005L, "Invalid create date value", "WAPPUSH");
				k = default(DateTime);
			}
			a();
		}
	}

	public string Expiry
	{
		get
		{
			l.aw();
			return i;
		}
		set
		{
			string text = "";
			long num = 0L;
			bool flag = true;
			checked
			{
				try
				{
					l.aw();
					if (value == null)
					{
						value = "0";
					}
					if (Operators.CompareString(value.Trim(), "0", false) != 0)
					{
						if (value.Length >= 2)
						{
							text = value.Substring(value.Length - 1, 1).ToUpper();
							if (Versioned.IsNumeric((object)value.Substring(0, value.Length - 1)) && !value.Contains(",") && !value.Contains("."))
							{
								num = Conversions.ToLong(value.Substring(0, value.Length - 1));
							}
							else
							{
								i = "0";
								flag = false;
							}
						}
						else
						{
							i = "0";
							flag = false;
						}
						if (num != 0)
						{
							goto IL_0109;
						}
						switch (text)
						{
						case "H":
						case "D":
						case "M":
							break;
						default:
							goto IL_0109;
						}
						i = "0";
						goto IL_01b5;
					}
					i = "0";
					goto end_IL_000b;
					IL_0109:
					switch (text)
					{
					case "H":
					case "D":
					case "M":
						if (num > 0)
						{
							if ((Operators.CompareString(text, "H", false) == 0 && num <= 24 && num >= 1) || (Operators.CompareString(text, "D", false) == 0 && num <= 31 && num >= 1) || (Operators.CompareString(text, "M", false) == 0 && num <= 12 && num >= 1))
							{
								i = value.ToUpper();
								break;
							}
							i = "0";
							flag = false;
							break;
						}
						goto default;
					default:
						i = "0";
						flag = false;
						break;
					}
					goto IL_01b5;
					IL_01b5:
					if (!flag)
					{
						i = "0";
						l.a(7006L, "Invalid expiry time", "WAPPUSH");
					}
					end_IL_000b:;
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					m.r.b("[WapPush:Expiry:1] " + ex2.Message);
					ProjectData.ClearProjectError();
				}
				a();
			}
		}
	}

	public bool IsLengthOK
	{
		get
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			long num = 0L;
			bool result = default(bool);
			try
			{
				result = false;
				text = l.a(f, g, k, i, j);
				text2 = l.an();
				text3 = l.ax();
				if (checked((int)Math.Round((double)(text + text2 + text3).Length / 2.0)) <= 140)
				{
					result = true;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				m.r.b("[WapPush:IsLengthOK:1] " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			return result;
		}
	}

	internal WapPush(ref SMS A_0)
	{
		f = "";
		g = "";
		h = "";
		i = "0";
		j = "07";
		k = default(DateTime);
		m = A_0;
		l = m.gm;
	}

	public string Send()
	{
		a a = new a(h, f, g, i, j, k, l.bt, l.bu);
		ThreadPool.RegisterWaitForSingleObject(l.dz, b, a, -1, executeOnlyOnce: true);
		while (a.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		string text = a.k;
		if (a != null)
		{
			a = null;
		}
		string result = text;
		this.a();
		return result;
	}

	string IWapPush.Send()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Send
		return this.Send();
	}

	public string SendToQueue()
	{
		a a = new a(h, f, g, i, j, k, l.bt, l.bu);
		ThreadPool.RegisterWaitForSingleObject(l.dz, this.a, a, -1, executeOnlyOnce: true);
		while (a.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		string text = a.l;
		if (a != null)
		{
			a = null;
		}
		string result = text;
		this.a();
		return result;
	}

	string IWapPush.SendToQueue()
	{
		//ILSpy generated this explicit interface implementation from .override directive in SendToQueue
		return this.SendToQueue();
	}

	public string SendToQueue(QueuePriority Priority)
	{
		a a = new a(h, f, g, i, j, k, l.bt, l.bu, (int)Priority);
		ThreadPool.RegisterWaitForSingleObject(l.dz, this.a, a, -1, executeOnlyOnce: true);
		while (a.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		string text = a.l;
		if (a != null)
		{
			a = null;
		}
		string result = text;
		this.a();
		return result;
	}

	string IWapPush.SendToQueue(QueuePriority Priority)
	{
		//ILSpy generated this explicit interface implementation from .override directive in SendToQueue
		return this.SendToQueue(Priority);
	}

	private void b(object A_0, bool A_1)
	{
		a a = (a)A_0;
		string text = "";
		bool flag = false;
		try
		{
			l.aw();
			if (!l.af)
			{
				m.q.a();
			}
			if (Operators.CompareString(l.ah, l.cl, false) == 0)
			{
				l.a(7007L, l.l("odumu5VOg_:5-26cCP/GtqPa6g$"), "WAPPUSH");
				a.i = false;
				return;
			}
			if (a.b.Length != 0 && a.c.Length != 0 && a.d.Length != 0)
			{
				text = l.a(a.d, a.b, a.c, a.h, a.e, a.f, a.g, a.m);
				if (text.Trim().Length <= 0)
				{
					if (l.cc == 0 && l.z == 0)
					{
						l.cc = 7008L;
						l.cb = "WAP push send error";
					}
					if (l.cc > 0)
					{
						l.a(l.cc, l.cb, "WAPPUSH");
					}
				}
				else
				{
					flag = true;
				}
			}
			else if (a.b.Length <= 0)
			{
				l.a(7009L, "Text message not specified", "WAPPUSH");
			}
			else if (a.c.Length <= 0)
			{
				l.a(7010L, "Push URL not specified", "WAPPUSH");
			}
			else if (a.d.Length <= 0)
			{
				l.a(7011L, "Destination mobile not specified", "WAPPUSH");
			}
			a.i = flag;
			a.k = text;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			m.r.b("[WapPush:Send_Method:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			a.a = false;
			l.dz.Set();
		}
	}

	private void a(object A_0, bool A_1)
	{
		a a = (a)A_0;
		try
		{
			l.aw();
			if (!l.af)
			{
				m.q.a();
			}
			if (Operators.CompareString(l.ah, l.cl, false) == 0)
			{
				l.a(7007L, l.l("odumu5VOg_:5-26cCP/GtqPa6g$"), "WAPPUSH");
			}
			else if (a.b.Length != 0 && a.c.Length != 0 && a.d.Length != 0)
			{
				a.l = m.objQueue.a(a.d, a.b, a.c, a.h, a.e, a.f, a.j, a.g, a.m);
			}
			else if (a.b.Length <= 0)
			{
				l.a(7009L, "Text message not specified", "WAPPUSH");
			}
			else if (a.c.Length <= 0)
			{
				l.a(7010L, "Push URL not specified", "WAPPUSH");
			}
			else if (a.d.Length <= 0)
			{
				l.a(7011L, "Destination mobile not specified", "WAPPUSH");
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			m.r.b("[WapPush:SendToQueue_Method:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			a.a = false;
			l.dz.Set();
		}
	}

	private void a()
	{
		if (l.ac.Length > 0)
		{
			switch (l.ac.ToUpper())
			{
			case "GENERAL":
				throw new GeneralException("ERROR " + l.z + ": " + l.y);
			case "SMSSEND":
				throw new SMSSendException("ERROR " + l.z + ": " + l.y);
			case "SMSREAD":
				throw new SMSReadException("ERROR " + l.z + ": " + l.y);
			case "SMSDELETE":
				throw new SMSDeleteException("ERROR " + l.z + ": " + l.y);
			case "WAPPUSH":
				throw new WapPushException("ERROR " + l.z + ": " + l.y);
			case "VCARD":
				throw new vCardException("ERROR " + l.z + ": " + l.y);
			case "VCALENDAR":
				throw new vCalendarException("ERROR " + l.z + ": " + l.y);
			}
		}
	}
}
