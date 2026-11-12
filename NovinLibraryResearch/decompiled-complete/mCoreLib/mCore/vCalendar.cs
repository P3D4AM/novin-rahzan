using System;
using System.IO;
using System.Security;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace mCore;

public class vCalendar : IvCalendar
{
	private class b
	{
		internal bool a;

		internal DateTime b;

		internal DateTime c;

		internal string d;

		internal string e;

		internal string f;

		internal string g;

		internal int h;

		internal string i;

		internal string j;

		internal bool k;

		internal int l;

		internal string m;

		internal bool n;

		internal string o;

		public b(string A_0, DateTime A_1, DateTime A_2, string A_3, string A_4, string A_5, string A_6, int A_7, bool A_8, int A_9 = 2)
		{
			a = true;
			b = default(DateTime);
			c = default(DateTime);
			d = "0";
			e = "";
			f = "";
			g = "";
			i = "";
			j = "";
			k = false;
			m = "";
			n = false;
			o = "";
			g = A_0;
			b = A_1;
			c = A_2;
			d = A_3;
			e = A_4;
			f = A_5;
			i = A_6;
			h = A_7;
			n = A_8;
			l = A_9;
		}
	}

	private class a
	{
		internal bool a;

		internal string b;

		internal string c;

		internal bool d;

		internal string e;

		internal int f;

		internal string g;

		internal int h;

		internal bool i;

		internal string j;

		public a(string A_0, string A_1, int A_2, bool A_3, int A_4 = 2)
		{
			a = true;
			b = "";
			c = "";
			d = false;
			e = "";
			i = false;
			j = "";
			c = A_0;
			b = A_1;
			h = A_2;
			i = A_3;
			f = A_4;
		}
	}

	private DateTime m_a;

	private DateTime m_b;

	private string m_c;

	private string m_d;

	private string e;

	private string f;

	private string g;

	private bool h;

	private f i;

	private SMS j;

	private const int k = 1;

	public string Destination
	{
		get
		{
			i.aw();
			return f;
		}
		set
		{
			i.aw();
			if (value == null)
			{
				value = "";
			}
			f = value.Trim();
			if (f.Length == 0 || (!i.j(f) && !i.f(f)))
			{
				f = "";
				i.a(8027L, "Invalid destination mobile number", "VCALENDAR");
			}
			a();
		}
	}

	public DateTime StartDateTime
	{
		get
		{
			i.aw();
			return this.m_a;
		}
		set
		{
			i.aw();
			if (DateTime.Compare(value, DateTime.MinValue) != 0 && Information.IsDate((object)value))
			{
				if (DateTime.Compare(this.m_b, DateTime.MinValue) != 0)
				{
					if (DateAndTime.DateDiff((DateInterval)8, this.m_a, this.m_b, (FirstDayOfWeek)1, (FirstWeekOfYear)1) > 0)
					{
						this.m_a = value;
					}
					else
					{
						this.m_a = default(DateTime);
						i.a(8028L, "Event start date time cannot be greater than end date time", "VCALENDAR");
					}
				}
				else
				{
					this.m_a = value;
				}
			}
			else
			{
				this.m_a = default(DateTime);
				i.a(8029L, "Invalid event start date time", "VCALENDAR");
			}
			b();
			a();
		}
	}

	public DateTime EndDateTime
	{
		get
		{
			i.aw();
			return this.m_b;
		}
		set
		{
			int num = 0;
			i.aw();
			if (DateTime.Compare(value, DateTime.MinValue) != 0 && Information.IsDate((object)value))
			{
				if (DateTime.Compare(this.m_a, DateTime.MinValue) != 0)
				{
					if (DateAndTime.DateDiff((DateInterval)8, this.m_a, value, (FirstDayOfWeek)1, (FirstWeekOfYear)1) > 0)
					{
						this.m_b = value;
						h = false;
					}
					else
					{
						this.m_b = default(DateTime);
						h = true;
						i.a(8030L, "Event end date time cannot be less than start date time", "VCALENDAR");
					}
				}
				else
				{
					this.m_b = value;
					h = false;
				}
			}
			else
			{
				this.m_b = default(DateTime);
				h = true;
				i.a(8031L, "Invalid event end date time", "VCALENDAR");
			}
			b();
			a();
		}
	}

	public string EventDuration
	{
		get
		{
			i.aw();
			return this.m_c;
		}
		set
		{
			string text = "";
			long num = 0L;
			bool flag = false;
			i.aw();
			if (value == null)
			{
				value = "0";
			}
			checked
			{
				if (value.Length >= 2)
				{
					text = value.Substring(value.Length - 1, 1).ToUpper();
					if (Versioned.IsNumeric((object)value.Substring(0, value.Length - 1)) && !value.Contains(",") && !value.Contains("."))
					{
						num = Conversions.ToLong(value.Substring(0, value.Length - 1));
						flag = true;
					}
					else
					{
						this.m_c = "0";
					}
				}
				else
				{
					this.m_c = "0";
				}
				if (num != 0)
				{
					goto IL_00e0;
				}
				switch (text)
				{
				case "H":
				case "D":
				case "M":
					break;
				default:
					goto IL_00e0;
				}
				this.m_c = "0";
				goto IL_012d;
			}
			IL_00e0:
			switch (text)
			{
			case "H":
			case "D":
			case "M":
				if (num > 0)
				{
					this.m_c = value.ToUpper();
					break;
				}
				goto default;
			default:
				this.m_c = "0";
				break;
			}
			goto IL_012d;
			IL_012d:
			if (!flag)
			{
				this.m_c = "0";
				h = false;
				b();
				i.a(8032L, "Invalid event duration", "VCALENDAR");
			}
			else
			{
				h = true;
				b();
			}
			a();
		}
	}

	public string Description
	{
		get
		{
			i.aw();
			return this.m_d;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			this.m_d = value.Trim();
			if (value.Trim().Length > 0 && !i.h(value))
			{
				this.m_d = "";
				i.a(8033L, "Invalid characters in event description", "VCALENDAR");
			}
			a();
		}
	}

	public string Summary
	{
		get
		{
			i.aw();
			return e;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			e = value.Trim();
			if (value.Trim().Length > 0 && !i.h(value))
			{
				e = "";
				i.a(8034L, "Invalid characters in event summary", "VCALENDAR");
			}
			a();
		}
	}

	public string Location
	{
		get
		{
			i.aw();
			return g;
		}
		set
		{
			i.aw();
			if (value == null)
			{
				value = "";
			}
			g = value.Trim();
			if (value.Trim().Length > 0 && !i.h(value))
			{
				g = "";
				i.a(8035L, "Invalid characters in event location name", "VCALENDAR");
			}
			a();
		}
	}

	internal vCalendar(ref SMS A_0)
	{
		this.m_a = default(DateTime);
		this.m_b = default(DateTime);
		this.m_c = "0";
		this.m_d = "";
		e = "";
		f = "";
		g = "";
		h = true;
		j = A_0;
		i = j.gm;
	}

	public string Send()
	{
		b b = new b(f, this.m_a, this.m_b, this.m_c, this.m_d, e, g, i.bt, i.bu);
		ThreadPool.RegisterWaitForSingleObject(i.dz, d, b, -1, executeOnlyOnce: true);
		while (b.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		string o = b.o;
		if (b != null)
		{
			b = null;
		}
		string result = o;
		a();
		return result;
	}

	string IvCalendar.Send()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Send
		return this.Send();
	}

	public string SendToQueue()
	{
		b b = new b(f, this.m_a, this.m_b, this.m_c, this.m_d, e, g, i.bt, i.bu);
		ThreadPool.RegisterWaitForSingleObject(i.dz, c, b, -1, executeOnlyOnce: true);
		while (b.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		string m = b.m;
		if (b != null)
		{
			b = null;
		}
		string result = m;
		a();
		return result;
	}

	string IvCalendar.SendToQueue()
	{
		//ILSpy generated this explicit interface implementation from .override directive in SendToQueue
		return this.SendToQueue();
	}

	public string SendToQueue(QueuePriority Priority)
	{
		b b = new b(f, this.m_a, this.m_b, this.m_c, this.m_d, e, g, i.bt, i.bu, (int)Priority);
		ThreadPool.RegisterWaitForSingleObject(i.dz, c, b, -1, executeOnlyOnce: true);
		while (b.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		string m = b.m;
		if (b != null)
		{
			b = null;
		}
		string result = m;
		a();
		return result;
	}

	string IvCalendar.SendToQueue(QueuePriority Priority)
	{
		//ILSpy generated this explicit interface implementation from .override directive in SendToQueue
		return this.SendToQueue(Priority);
	}

	public string SendFile(string vCalendarFile)
	{
		a a = new a(f, vCalendarFile, i.bt, i.bu);
		ThreadPool.RegisterWaitForSingleObject(i.dz, b, a, -1, executeOnlyOnce: true);
		while (a.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		string text = a.e;
		if (a != null)
		{
			a = null;
		}
		string result = text;
		this.a();
		return result;
	}

	string IvCalendar.SendFile(string vCalendarFile)
	{
		//ILSpy generated this explicit interface implementation from .override directive in SendFile
		return this.SendFile(vCalendarFile);
	}

	public string SendFileToQueue(string vCalendarFile)
	{
		a a = new a(f, vCalendarFile, i.bt, i.bu);
		ThreadPool.RegisterWaitForSingleObject(i.dz, this.a, a, -1, executeOnlyOnce: true);
		while (a.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		string text = a.g;
		if (a != null)
		{
			a = null;
		}
		string result = text;
		this.a();
		return result;
	}

	string IvCalendar.SendFileToQueue(string vCalendarFile)
	{
		//ILSpy generated this explicit interface implementation from .override directive in SendFileToQueue
		return this.SendFileToQueue(vCalendarFile);
	}

	public string SendFileToQueue(string vCalendarFile, QueuePriority Priority)
	{
		a a = new a(f, vCalendarFile, i.bt, i.bu, (int)Priority);
		ThreadPool.RegisterWaitForSingleObject(i.dz, this.a, a, -1, executeOnlyOnce: true);
		while (a.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		string text = a.g;
		if (a != null)
		{
			a = null;
		}
		string result = text;
		this.a();
		return result;
	}

	string IvCalendar.SendFileToQueue(string vCalendarFile, QueuePriority Priority)
	{
		//ILSpy generated this explicit interface implementation from .override directive in SendFileToQueue
		return this.SendFileToQueue(vCalendarFile, Priority);
	}

	private void b()
	{
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		string text = "";
		long num = 0L;
		string text2 = "";
		long num2 = 0L;
		checked
		{
			DateInterval val = default(DateInterval);
			if (this.m_c.Trim().Length >= 2)
			{
				text = this.m_c.Trim().Substring(this.m_c.Trim().Length - 1, 1);
				if (Versioned.IsNumeric((object)this.m_c.Trim().Substring(0, this.m_c.Trim().Length - 1)) && !this.m_c.Contains(",") && !this.m_c.Contains("."))
				{
					num = Conversions.ToLong(this.m_c.Trim().Substring(0, this.m_c.Trim().Length - 1));
					switch (text.ToUpper())
					{
					case "M":
						val = (DateInterval)8;
						break;
					case "H":
						val = (DateInterval)7;
						break;
					case "D":
						val = (DateInterval)4;
						break;
					}
				}
				else
				{
					this.m_c = "0";
				}
			}
			if (DateTime.Compare(this.m_a, DateTime.MinValue) != 0)
			{
				if (h)
				{
					if (Operators.CompareString(this.m_c, "0", false) == 0)
					{
						this.m_b = default(DateTime);
					}
					else
					{
						this.m_b = DateAndTime.DateAdd(val, (double)num, this.m_a);
					}
					return;
				}
				if (DateTime.Compare(this.m_b, DateTime.MinValue) == 0)
				{
					this.m_c = "0";
					return;
				}
				num2 = DateAndTime.DateDiff((DateInterval)8, this.m_a, this.m_b, (FirstDayOfWeek)1, (FirstWeekOfYear)1);
				text2 = "M";
				if (unchecked(num2 % 60) == 0)
				{
					num2 = (long)Math.Round((double)num2 / 60.0);
					text2 = "H";
					if (unchecked(num2 % 24) == 0)
					{
						num2 = (long)Math.Round((double)num2 / 24.0);
						text2 = "D";
					}
				}
				this.m_c = num2 + text2;
			}
			else if (h)
			{
				this.m_b = default(DateTime);
			}
			else
			{
				this.m_c = "0";
			}
		}
	}

	private void d(object A_0, bool A_1)
	{
		b b = (b)A_0;
		string text = "";
		bool flag = false;
		string text2 = "";
		try
		{
			b.k = false;
			i.aw();
			if (!i.af)
			{
				i.b.a();
			}
			if (Operators.CompareString(i.ah, i.cl, false) == 0)
			{
				i.a(8036L, i.l("odumu5VOg_:5-26cCP/GtqPa6g$"), "VCALENDAR");
				b.k = false;
				return;
			}
			if (b.g.Length <= 0)
			{
				i.a(8023L, "Destination mobile number not defined", "VCALENDAR");
				b.k = false;
				return;
			}
			if (Operators.CompareString(i.ah, i.ck, false) == 0)
			{
				b.e = b.j + b.e;
				b.f = b.j + b.f;
			}
			text = a(b);
			if (text.Length <= 0)
			{
				b.k = false;
				return;
			}
			text2 = i.b(text, b.g, b.h, b.n);
			if (text2.Trim().Length <= 0)
			{
				if (i.cg == 0 && i.z == 0)
				{
					i.cg = 8037L;
					i.cd = "vCalendar send error";
				}
				if (i.cg > 0)
				{
					i.a(i.cg, i.cf, "VCALENDAR");
				}
			}
			else
			{
				flag = true;
			}
			b.k = flag;
			b.o = text2;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			j.r.b("[vCalendar:Send_Method:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			b.a = false;
			i.dz.Set();
		}
	}

	private void c(object A_0, bool A_1)
	{
		b b = (b)A_0;
		string text = "";
		try
		{
			i.aw();
			if (!i.af)
			{
				j.q.a();
			}
			if (Operators.CompareString(i.ah, i.cl, false) == 0)
			{
				i.a(8036L, i.l("odumu5VOg_:5-26cCP/GtqPa6g$"), "VCALENDAR");
				return;
			}
			if (b.g.Length <= 0)
			{
				i.a(8023L, "Destination mobile number not defined", "VCALENDAR");
				return;
			}
			if (Operators.CompareString(i.ah, i.ck, false) == 0)
			{
				b.e = b.j + b.e;
				b.f = b.j + b.f;
			}
			text = a(b);
			if (text.Length > 0)
			{
				b.m = j.objQueue.b(b.g, text, b.l, b.h, b.n);
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			j.r.b("[vCalendar:SendToQueue_Method:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			b.a = false;
			i.dz.Set();
		}
	}

	private string a(b A_0)
	{
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		string text = "";
		string text2 = "";
		long num = 0L;
		string text3 = "";
		if (DateTime.Compare(A_0.b, DateTime.MinValue) == 0)
		{
			i.a(8038L, "vCalendar event start date not defined", "VCALENDAR");
			return "";
		}
		checked
		{
			if (DateTime.Compare(A_0.c, DateTime.MinValue) == 0)
			{
				if (Operators.CompareString(A_0.d.Trim(), "0", false) == 0)
				{
					i.a(8039L, "Both vCalendar event end date and duration not defined", "VCALENDAR");
					return "";
				}
				if (A_0.d.Trim().Length < 2)
				{
					i.a(8042L, "Invalid vCalendar even duration", "VCALENDAR");
					return "";
				}
				text2 = A_0.d.Trim().Substring(A_0.d.Trim().Length - 1, 1);
				if (!Versioned.IsNumeric((object)A_0.d.Trim().Substring(0, A_0.d.Trim().Length - 1)) || A_0.d.Contains(",") || A_0.d.Contains(".") || (Operators.CompareString(text2.ToUpper(), "M", false) != 0 && Operators.CompareString(text2.ToUpper(), "H", false) != 0 && Operators.CompareString(text2.ToUpper(), "D", false) != 0))
				{
					i.a(8041L, "Invalid vCalendar event duration", "VCALENDAR");
					return "";
				}
				num = Conversions.ToLong(A_0.d.Trim().Substring(0, A_0.d.Trim().Length - 1));
				if (num <= 0)
				{
					i.a(8040L, "vCalendar event start and end date cannot be same", "VCALENDAR");
					return "";
				}
				DateInterval val = default(DateInterval);
				switch (text2.ToUpper())
				{
				case "M":
					val = (DateInterval)8;
					break;
				case "H":
					val = (DateInterval)7;
					break;
				case "D":
					val = (DateInterval)4;
					break;
				}
				A_0.c = DateAndTime.DateAdd(val, (double)num, A_0.b);
			}
			if (Operators.CompareString(A_0.d.Trim(), "0", false) == 0)
			{
				long num2 = DateAndTime.DateDiff((DateInterval)8, A_0.b, A_0.c, (FirstDayOfWeek)1, (FirstWeekOfYear)1);
				text3 = "M";
				if (unchecked(num2 % 60) == 0)
				{
					num2 = (long)Math.Round((double)num2 / 60.0);
					text3 = "H";
					if (unchecked(num2 % 24) == 0)
					{
						num2 = (long)Math.Round((double)num2 / 24.0);
						text3 = "D";
					}
				}
				A_0.d = num2 + text3;
			}
			if (A_0.e.Trim().Length == 0 && A_0.f.Trim().Length == 0)
			{
				i.a(8043L, "Both vCalendar event description and summary are not defined", "VCALENDAR");
				return "";
			}
			text = "BEGIN:VCALENDAR\r\nVERSION:1.0\r\nBEGIN:VEVENT";
			if (A_0.e.Trim().Length > 0)
			{
				text = text + "\r\nDESCRIPTION;ENCODING=QUOTED-PRINTABLE:" + i.k(A_0.e);
			}
			if (A_0.f.Trim().Length > 0)
			{
				text = text + "\r\nSUMMARY;ENCODING=QUOTED-PRINTABLE:" + i.k(A_0.f);
			}
			if (A_0.i.Trim().Length > 0)
			{
				text = text + "\r\nLOCATION;ENCODING=QUOTED-PRINTABLE:" + i.k(A_0.i);
			}
			text = text + "\r\nDTSTART:" + i.a(A_0.b);
			text = text + "\r\nDTEND:" + i.a(A_0.c);
			return text + "\r\nEND:VEVENT\r\nEND:VCALENDAR";
		}
	}

	private void b(object A_0, bool A_1)
	{
		a a = (a)A_0;
		bool flag = false;
		string text = "";
		string text2 = "";
		string text3 = "";
		string text4 = "";
		checked
		{
			try
			{
				a.d = false;
				a.e = "";
				this.i.aw();
				if (f.Length <= 0)
				{
					this.i.a(8023L, "Destination mobile number not defined", "VCALENDAR");
				}
				else if (a.b.Length > 0 && a.b.ToUpper().EndsWith(".VCS"))
				{
					if (File.Exists(a.b))
					{
						if (string.Compare(this.i.ah, this.i.ck, ignoreCase: true) == 0)
						{
							string[] array = default(string[]);
							try
							{
								array = File.ReadAllLines(a.b);
							}
							catch (ArgumentException ex)
							{
								ProjectData.SetProjectError((Exception)ex);
								ArgumentException ex2 = ex;
								this.i.a(8082L, "Invalid vCalendar file name", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							catch (PathTooLongException ex3)
							{
								ProjectData.SetProjectError((Exception)ex3);
								PathTooLongException ex4 = ex3;
								this.i.a(8083L, "The specified path, file name, or both exceed the system defined maximum length", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							catch (DirectoryNotFoundException ex5)
							{
								ProjectData.SetProjectError((Exception)ex5);
								DirectoryNotFoundException ex6 = ex5;
								this.i.a(8084L, "The specified path for vCalendar file is invalid", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							catch (FileNotFoundException ex7)
							{
								ProjectData.SetProjectError((Exception)ex7);
								FileNotFoundException ex8 = ex7;
								this.i.a(8085L, "The specified vCalendar file [" + a.b + "] not found", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							catch (IOException ex9)
							{
								ProjectData.SetProjectError((Exception)ex9);
								IOException ex10 = ex9;
								this.i.a(8086L, "An I/O error occured while opening the vCalendar file", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							catch (UnauthorizedAccessException ex11)
							{
								ProjectData.SetProjectError((Exception)ex11);
								UnauthorizedAccessException ex12 = ex11;
								this.i.a(8087L, "Permission denied to access the vCalendar file", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							catch (NotSupportedException ex13)
							{
								ProjectData.SetProjectError((Exception)ex13);
								NotSupportedException ex14 = ex13;
								this.i.a(8088L, "The specified vCalendar file path is in an invalid format", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							catch (SecurityException ex15)
							{
								ProjectData.SetProjectError((Exception)ex15);
								SecurityException ex16 = ex15;
								this.i.a(8089L, "Insufficient permission to access the vCalendar file", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							if (this.i.z > 0)
							{
								return;
							}
							if (array.GetUpperBound(0) <= 0)
							{
								this.i.a(8090L, "vCalendar file is empty", "VCALENDAR");
								return;
							}
							int upperBound = array.GetUpperBound(0);
							for (int i = 0; i <= upperBound; i++)
							{
								text = "";
								text2 = "";
								text3 = "";
								if (array[i].Length > 0)
								{
									if (array[i].Contains(":"))
									{
										string[] array2 = array[i].Split(":".ToCharArray());
										text2 = array2[0];
										int upperBound2 = array2.GetUpperBound(0);
										for (int j = 1; j <= upperBound2; j++)
										{
											if (j > 1)
											{
												text3 += ":";
											}
											text3 += array2[j];
										}
									}
									if (array[i].ToUpper().Contains("ENCODING=QUOTED-PRINTABLE"))
									{
										text3 = text3.Replace("=0D", "\r").Replace("=0A", "\n");
										flag = true;
										if (text3.EndsWith("="))
										{
											text3 = Strings.Left(text3, text3.Length - 1);
											flag = false;
										}
										while (!flag)
										{
											i++;
											text3 += array[i].Replace("=0D", "\r").Replace("=0A", "\n");
											if (text3.EndsWith("="))
											{
												text3 = Strings.Left(text3, text3.Length - 1);
												flag = false;
											}
											else
											{
												flag = true;
											}
										}
										text = ((!text2.ToUpper().StartsWith("DESCRIPTION;") && !text2.ToUpper().StartsWith("DESCRIPTION:") && !text2.ToUpper().StartsWith("SUMMARY;") && !text2.ToUpper().StartsWith("SUMMARY:")) ? (text2 + ":" + this.i.k(text3)) : (text2 + ":" + this.i.k(a.j + text3)));
									}
									else
									{
										text = ((!text2.ToUpper().StartsWith("DESCRIPTION;") && !text2.ToUpper().StartsWith("DESCRIPTION:") && !text2.ToUpper().StartsWith("SUMMARY;") && !text2.ToUpper().StartsWith("SUMMARY:")) ? array[i] : (text2 + ";ENCODING=QUOTED-PRINTABLE:" + this.i.k(a.j + text3)));
									}
								}
								else
								{
									text = array[i];
								}
								text4 = text4 + text + "\r\n";
							}
						}
						else
						{
							try
							{
								text4 = File.ReadAllText(a.b);
							}
							catch (ArgumentException ex17)
							{
								ProjectData.SetProjectError((Exception)ex17);
								ArgumentException ex18 = ex17;
								this.i.a(8082L, "Invalid vCalendar file name", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							catch (PathTooLongException ex19)
							{
								ProjectData.SetProjectError((Exception)ex19);
								PathTooLongException ex20 = ex19;
								this.i.a(8083L, "The specified path, file name, or both exceed the system defined maximum length", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							catch (DirectoryNotFoundException ex21)
							{
								ProjectData.SetProjectError((Exception)ex21);
								DirectoryNotFoundException ex22 = ex21;
								this.i.a(8084L, "The specified path for vCalendar file is invalid", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							catch (FileNotFoundException ex23)
							{
								ProjectData.SetProjectError((Exception)ex23);
								FileNotFoundException ex24 = ex23;
								this.i.a(8085L, "The specified vCalendar file [" + a.b + "] not found", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							catch (IOException ex25)
							{
								ProjectData.SetProjectError((Exception)ex25);
								IOException ex26 = ex25;
								this.i.a(8086L, "An I/O error occured while opening the vCalendar file", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							catch (UnauthorizedAccessException ex27)
							{
								ProjectData.SetProjectError((Exception)ex27);
								UnauthorizedAccessException ex28 = ex27;
								this.i.a(8087L, "Permission denied to access the vCalendar file", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							catch (NotSupportedException ex29)
							{
								ProjectData.SetProjectError((Exception)ex29);
								NotSupportedException ex30 = ex29;
								this.i.a(8088L, "The specified vCalendar file path is in an invalid format", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							catch (SecurityException ex31)
							{
								ProjectData.SetProjectError((Exception)ex31);
								SecurityException ex32 = ex31;
								this.i.a(8089L, "Insufficient permission to access the vCalendar file", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							if (this.i.z > 0)
							{
								return;
							}
						}
						if (text4.Replace("\r\n", "").Trim().Length > 0)
						{
							a.e = this.i.b(text4, a.c, a.h, a.i);
							if (a.e.Trim().Length <= 0)
							{
								if (this.i.cg == 0 && this.i.z == 0)
								{
									this.i.cg = 8094L;
									this.i.cd = "vCalendar file send error";
								}
								if (this.i.cg > 0)
								{
									this.i.a(this.i.cg, this.i.cf, "VCALENDAR");
								}
							}
							else
							{
								a.d = true;
							}
						}
						else
						{
							this.i.a(8090L, "vCalendar file is empty", "VCALENDAR");
						}
					}
					else
					{
						this.i.a(8091L, "vCalendar file [" + a.b + "] not found", "VCALENDAR");
					}
				}
				else
				{
					this.i.a(8092L, "Invalid vCalendar file name", "VCALENDAR");
				}
			}
			catch (Exception ex33)
			{
				ProjectData.SetProjectError(ex33);
				Exception ex34 = ex33;
				this.j.r.b("[vCalendar:Send_Class:Send:1] " + ex34.Message);
				ProjectData.ClearProjectError();
			}
			finally
			{
				a.a = false;
				this.i.dz.Set();
			}
		}
	}

	private void a(object A_0, bool A_1)
	{
		a a = (a)A_0;
		bool flag = false;
		string text = "";
		string text2 = "";
		string text3 = "";
		string text4 = "";
		checked
		{
			try
			{
				if (a.c.Length <= 0)
				{
					this.i.a(8023L, "Destination mobile number not defined", "VCALENDAR");
				}
				else if (a.b.Length > 0 && a.b.ToUpper().EndsWith(".VCS"))
				{
					if (File.Exists(a.b))
					{
						if (string.Compare(this.i.ah, this.i.ck, ignoreCase: true) == 0)
						{
							string[] array = default(string[]);
							try
							{
								array = File.ReadAllLines(a.b);
							}
							catch (ArgumentException ex)
							{
								ProjectData.SetProjectError((Exception)ex);
								ArgumentException ex2 = ex;
								this.i.a(8082L, "Invalid vCalendar file name", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							catch (PathTooLongException ex3)
							{
								ProjectData.SetProjectError((Exception)ex3);
								PathTooLongException ex4 = ex3;
								this.i.a(8083L, "The specified path, file name, or both exceed the system defined maximum length", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							catch (DirectoryNotFoundException ex5)
							{
								ProjectData.SetProjectError((Exception)ex5);
								DirectoryNotFoundException ex6 = ex5;
								this.i.a(8084L, "The specified path for vCalendar file is invalid", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							catch (FileNotFoundException ex7)
							{
								ProjectData.SetProjectError((Exception)ex7);
								FileNotFoundException ex8 = ex7;
								this.i.a(8085L, "The specified vCalendar file [" + a.b + "] not found", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							catch (IOException ex9)
							{
								ProjectData.SetProjectError((Exception)ex9);
								IOException ex10 = ex9;
								this.i.a(8086L, "An I/O error occured while opening the vCalendar file", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							catch (UnauthorizedAccessException ex11)
							{
								ProjectData.SetProjectError((Exception)ex11);
								UnauthorizedAccessException ex12 = ex11;
								this.i.a(8087L, "Permission denied to access the vCalendar file", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							catch (NotSupportedException ex13)
							{
								ProjectData.SetProjectError((Exception)ex13);
								NotSupportedException ex14 = ex13;
								this.i.a(8088L, "The specified vCalendar file path is in an invalid format", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							catch (SecurityException ex15)
							{
								ProjectData.SetProjectError((Exception)ex15);
								SecurityException ex16 = ex15;
								this.i.a(8089L, "Insufficient permission to access the vCalendar file", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							if (this.i.z > 0)
							{
								return;
							}
							if (array.GetUpperBound(0) <= 0)
							{
								this.i.a(8090L, "vCalendar file is empty", "VCALENDAR");
								return;
							}
							int upperBound = array.GetUpperBound(0);
							for (int i = 0; i <= upperBound; i++)
							{
								text = "";
								text2 = "";
								text3 = "";
								if (array[i].Length > 0)
								{
									if (array[i].Contains(":"))
									{
										string[] array2 = array[i].Split(":".ToCharArray());
										text2 = array2[0];
										int upperBound2 = array2.GetUpperBound(0);
										for (int j = 1; j <= upperBound2; j++)
										{
											if (j > 1)
											{
												text3 += ":";
											}
											text3 += array2[j];
										}
									}
									if (array[i].ToUpper().Contains("ENCODING=QUOTED-PRINTABLE"))
									{
										text3 = text3.Replace("=0D", "\r").Replace("=0A", "\n");
										flag = true;
										if (text3.EndsWith("="))
										{
											text3 = Strings.Left(text3, text3.Length - 1);
											flag = false;
										}
										while (!flag)
										{
											i++;
											text3 += array[i].Replace("=0D", "\r").Replace("=0A", "\n");
											if (text3.EndsWith("="))
											{
												text3 = Strings.Left(text3, text3.Length - 1);
												flag = false;
											}
											else
											{
												flag = true;
											}
										}
										text = ((!text2.ToUpper().StartsWith("DESCRIPTION;") && !text2.ToUpper().StartsWith("DESCRIPTION:") && !text2.ToUpper().StartsWith("SUMMARY;") && !text2.ToUpper().StartsWith("SUMMARY:")) ? (text2 + ":" + this.i.k(text3)) : (text2 + ":" + this.i.k(a.j + text3)));
									}
									else
									{
										text = ((!text2.ToUpper().StartsWith("DESCRIPTION;") && !text2.ToUpper().StartsWith("DESCRIPTION:") && !text2.ToUpper().StartsWith("SUMMARY;") && !text2.ToUpper().StartsWith("SUMMARY:")) ? array[i] : (text2 + ";ENCODING=QUOTED-PRINTABLE:" + this.i.k(a.j + text3)));
									}
								}
								else
								{
									text = array[i];
								}
								text4 = text4 + text + "\r\n";
							}
						}
						else
						{
							try
							{
								text4 = File.ReadAllText(a.b);
							}
							catch (ArgumentException ex17)
							{
								ProjectData.SetProjectError((Exception)ex17);
								ArgumentException ex18 = ex17;
								this.i.a(8082L, "Invalid vCalendar file name", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							catch (PathTooLongException ex19)
							{
								ProjectData.SetProjectError((Exception)ex19);
								PathTooLongException ex20 = ex19;
								this.i.a(8083L, "The specified path, file name, or both exceed the system defined maximum length", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							catch (DirectoryNotFoundException ex21)
							{
								ProjectData.SetProjectError((Exception)ex21);
								DirectoryNotFoundException ex22 = ex21;
								this.i.a(8084L, "The specified path for vCalendar file is invalid", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							catch (FileNotFoundException ex23)
							{
								ProjectData.SetProjectError((Exception)ex23);
								FileNotFoundException ex24 = ex23;
								this.i.a(8085L, "The specified vCalendar file [" + a.b + "] not found", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							catch (IOException ex25)
							{
								ProjectData.SetProjectError((Exception)ex25);
								IOException ex26 = ex25;
								this.i.a(8086L, "An I/O error occured while opening the vCalendar file", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							catch (UnauthorizedAccessException ex27)
							{
								ProjectData.SetProjectError((Exception)ex27);
								UnauthorizedAccessException ex28 = ex27;
								this.i.a(8087L, "Permission denied to access the vCalendar file", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							catch (NotSupportedException ex29)
							{
								ProjectData.SetProjectError((Exception)ex29);
								NotSupportedException ex30 = ex29;
								this.i.a(8088L, "The specified vCalendar file path is in an invalid format", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							catch (SecurityException ex31)
							{
								ProjectData.SetProjectError((Exception)ex31);
								SecurityException ex32 = ex31;
								this.i.a(8089L, "Insufficient permission to access the vCalendar file", "VCALENDAR");
								ProjectData.ClearProjectError();
							}
							if (this.i.z > 0)
							{
								return;
							}
						}
						if (text4.Replace("\r\n", "").Trim().Length > 0)
						{
							a.g = this.j.objQueue.b(a.c, text4, a.f, a.h, a.i);
						}
						else
						{
							this.i.a(8090L, "vCalendar file is empty", "VCALENDAR");
						}
					}
					else
					{
						this.i.a(8091L, "vCalendar file [" + a.b + "] not found", "VCALENDAR");
					}
				}
				else
				{
					this.i.a(8092L, "Invalid vCalendar file name", "VCALENDAR");
				}
			}
			catch (Exception ex33)
			{
				ProjectData.SetProjectError(ex33);
				Exception ex34 = ex33;
				this.j.r.b("[vCalendar:SendFileToQueue_Method:1] " + ex34.Message);
				ProjectData.ClearProjectError();
			}
			finally
			{
				a.a = false;
				this.i.dz.Set();
			}
		}
	}

	private void a()
	{
		if (i.ac.Length > 0)
		{
			switch (i.ac.ToUpper())
			{
			case "GENERAL":
				throw new GeneralException("ERROR " + i.z + ": " + i.y);
			case "SMSSEND":
				throw new SMSSendException("ERROR " + i.z + ": " + i.y);
			case "SMSREAD":
				throw new SMSReadException("ERROR " + i.z + ": " + i.y);
			case "SMSDELETE":
				throw new SMSDeleteException("ERROR " + i.z + ": " + i.y);
			case "WAPPUSH":
				throw new WapPushException("ERROR " + i.z + ": " + i.y);
			case "VCARD":
				throw new vCardException("ERROR " + i.z + ": " + i.y);
			case "VCALENDAR":
				throw new vCalendarException("ERROR " + i.z + ": " + i.y);
			}
		}
	}
}
