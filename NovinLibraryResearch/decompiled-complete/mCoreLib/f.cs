using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using mCore;

internal class f : IDisposable
{
	internal delegate void f(NewMessageReceivedEventArgs A_0);

	internal delegate void e(NewDeliveryReportEventArgs A_0);

	internal delegate void b(NewIncomingCallEventArgs A_0);

	internal delegate void a(NewUSSDReceivedEventArgs A_0);

	private class g
	{
		internal AutoResetEvent a;

		public g()
		{
			a = new AutoResetEvent(initialState: false);
		}
	}

	private class c
	{
		internal string a;

		internal string b;

		internal string c;

		internal string d;

		internal AutoResetEvent e;

		public c(string A_0, string A_1, string A_2)
		{
			a = "";
			b = "";
			c = "";
			d = "";
			e = new AutoResetEvent(initialState: false);
			a = A_0;
			c = A_1;
			d = A_2;
		}
	}

	private class d
	{
		internal AutoResetEvent a;

		public d()
		{
			a = new AutoResetEvent(initialState: false);
		}
	}

	internal o a;

	internal mCore.License b;

	internal global::c c;

	private bool m_d;

	internal static bool e = false;

	internal static global::a f = null;

	private f m_g;

	private e m_h;

	private b m_i;

	private a m_j;

	private global::b m_k;

	private Collection m_l;

	internal j m;

	internal string n;

	internal string o;

	internal FileVersionInfo p;

	internal string q;

	internal bool r;

	internal bool s;

	internal bool t;

	internal bool u;

	internal bool v;

	internal bool w;

	internal bool x;

	internal string y;

	internal long z;

	internal string aa;

	internal long ab;

	internal string ac;

	internal string ad;

	internal int ae;

	internal bool af;

	internal bool ag;

	internal string ah;

	internal string ai;

	internal string aj;

	internal int ak;

	internal Parity al;

	internal int am;

	internal StopBits an;

	internal Handshake ao;

	private const int m_ap = 4096;

	private const int m_aq = 1024;

	private const int m_ar = 750;

	private const int m_as = 750;

	private const int m_at = 1;

	private const string m_au = "\n";

	internal long av;

	internal string aw;

	private SerialPort m_ax;

	private SerialPort ay;

	internal int az;

	private const int a0 = 25;

	private const int a1 = 125;

	private const int a2 = 25;

	internal int a3;

	internal int a4;

	internal string a5;

	internal string a6;

	internal bool a7;

	internal string a8;

	internal string a9;

	internal string ba;

	internal string bb;

	internal string bc;

	internal string bd;

	internal int be;

	internal string bf;

	internal string bg;

	internal string bh;

	internal string bi;

	internal long bj;

	internal bool bk;

	internal bool bl;

	internal string bm;

	internal long bn;

	internal int bo;

	internal int bp;

	internal string bq;

	internal string br;

	internal string bs;

	internal int bt;

	internal bool bu;

	internal bool bv;

	internal bool bw;

	internal int bx;

	internal int by;

	internal int bz;

	internal int b0;

	internal bool b1;

	internal long b2;

	internal string b3;

	internal long b4;

	internal string b5;

	internal long b6;

	internal string b7;

	internal long b8;

	internal string b9;

	internal long ca;

	internal string cb;

	internal long cc;

	internal string cd;

	internal long ce;

	internal string cf;

	internal long cg;

	internal string ch;

	internal long ci;

	internal string cj;

	internal string ck;

	internal string cl;

	internal string cm;

	internal string cn;

	private string co;

	private string cp;

	private string cq;

	private string cr;

	private string cs;

	private string ct;

	private bool cu;

	internal int cv;

	internal double cw;

	internal DateTime cx;

	internal bool cy;

	internal Collection cz;

	internal Collection c0;

	internal Collection c1;

	internal Collection c2;

	private string c3;

	internal bool c4;

	internal bool c5;

	internal bool c6;

	internal bool c7;

	internal bool c8;

	internal bool c9;

	private const int da = 30;

	private const int db = 1000;

	private const string dc = "05";

	private const string dd = "0B84";

	private const string de = "23F0";

	private const string df = "25";

	private const string dg = "06";

	private const string dh = "03AE81EA";

	private const string di = "AF";

	private const string dj = "82";

	private const string dk = "B484";

	internal const string dl = "GENERAL";

	internal const string dm = "SMSSEND";

	internal const string dn = "SMSREAD";

	internal const string @do = "SMSDELETE";

	internal const string dp = "WAPPUSH";

	internal const string dq = "VCARD";

	internal const string dr = "VCALENDAR";

	private Hashtable ds;

	private Hashtable dt;

	private Hashtable du;

	private Hashtable dv;

	private Hashtable dw;

	private Hashtable dx;

	private Hashtable dy;

	internal AutoResetEvent dz;

	private AutoResetEvent d0;

	private AutoResetEvent d1;

	private AutoResetEvent d2;

	private AutoResetEvent d3;

	private AutoResetEvent d4;

	private AutoResetEvent d5;

	private AutoResetEvent d6;

	private const int d7 = 1;

	private bool d8;

	[MethodImpl(MethodImplOptions.Synchronized)]
	[SpecialName]
	[DebuggerNonUserCode]
	internal void a(f A_0)
	{
		this.m_g = (f)Delegate.Combine(this.m_g, A_0);
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	[SpecialName]
	[DebuggerNonUserCode]
	internal void b(f A_0)
	{
		this.m_g = (f)Delegate.Remove(this.m_g, A_0);
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	[SpecialName]
	[DebuggerNonUserCode]
	internal void a(e A_0)
	{
		this.m_h = (e)Delegate.Combine(this.m_h, A_0);
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	[SpecialName]
	[DebuggerNonUserCode]
	internal void b(e A_0)
	{
		this.m_h = (e)Delegate.Remove(this.m_h, A_0);
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	[SpecialName]
	[DebuggerNonUserCode]
	internal void a(b A_0)
	{
		this.m_i = (b)Delegate.Combine(this.m_i, A_0);
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	[SpecialName]
	[DebuggerNonUserCode]
	internal void b(b A_0)
	{
		this.m_i = (b)Delegate.Remove(this.m_i, A_0);
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	[SpecialName]
	[DebuggerNonUserCode]
	internal void a(a A_0)
	{
		this.m_j = (a)Delegate.Combine(this.m_j, A_0);
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	[SpecialName]
	[DebuggerNonUserCode]
	internal void b(a A_0)
	{
		this.m_j = (a)Delegate.Remove(this.m_j, A_0);
	}

	[SpecialName]
	[DebuggerNonUserCode]
	internal virtual SerialPort s()
	{
		return this.m_ax;
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	[SpecialName]
	[DebuggerNonUserCode]
	internal virtual void a(SerialPort A_0)
	{
		this.m_ax = A_0;
	}

	internal f()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Expected O, but got Unknown
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Expected O, but got Unknown
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Expected O, but got Unknown
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Expected O, but got Unknown
		//IL_055d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0567: Expected O, but got Unknown
		this.m_d = false;
		this.m_l = new Collection();
		this.n = "mCoreLib.dll";
		this.o = "";
		this.p = null;
		this.q = "";
		this.r = false;
		this.s = false;
		this.t = false;
		this.u = false;
		this.v = false;
		this.w = false;
		this.x = false;
		this.y = "";
		this.aa = "";
		this.ac = "";
		this.ad = l("(S8\"Wz\u0001!y2+w@\"\u001a]77 5~");
		this.af = false;
		this.ag = true;
		this.ah = "";
		this.ai = "";
		this.aj = "COM1";
		this.ak = 19200;
		this.al = (Parity)0;
		this.am = 8;
		this.an = (StopBits)1;
		this.ao = (Handshake)0;
		this.av = 30000L;
		this.aw = "";
		a(new SerialPort());
		az = 15000;
		a3 = 150;
		a4 = 300000;
		a5 = "AT E0 V1 +CMEE=1";
		a6 = "";
		a7 = false;
		a8 = "SM";
		a9 = "";
		ba = "";
		bb = "";
		bc = "";
		bd = "";
		be = -1;
		bf = "";
		bg = "";
		bh = "";
		bi = "";
		bj = 0L;
		bk = false;
		bl = false;
		bm = "";
		bq = "";
		br = "";
		bs = "";
		bt = 167;
		bu = false;
		bv = false;
		bw = false;
		bx = 7;
		by = 3;
		bz = 1000;
		b0 = 1;
		b1 = true;
		b2 = 5L;
		b3 = "";
		b4 = 0L;
		b5 = "";
		b6 = 0L;
		b7 = "";
		b8 = 0L;
		b9 = "";
		ca = 0L;
		cb = "";
		cc = 0L;
		cd = "";
		ce = 0L;
		cf = "";
		cg = 0L;
		ch = "";
		ci = 0L;
		cj = DateTime.Now.ToString();
		ck = "";
		cl = "";
		cm = "";
		cn = "";
		co = "0";
		cp = "0";
		cq = "0";
		cr = "0";
		cs = "2";
		ct = "ATH";
		cu = false;
		cv = DateTime.Now.Second;
		cx = DateAndTime.Now;
		cy = true;
		cz = new Collection();
		c0 = new Collection();
		c1 = new Collection();
		c2 = new Collection();
		c3 = "";
		c4 = false;
		c5 = false;
		c6 = false;
		c7 = false;
		c8 = false;
		c9 = false;
		ds = new Hashtable();
		dt = new Hashtable();
		du = new Hashtable();
		dv = new Hashtable();
		dw = new Hashtable();
		dx = new Hashtable();
		dy = new Hashtable();
		dz = new AutoResetEvent(initialState: false);
		d0 = new AutoResetEvent(initialState: false);
		d1 = new AutoResetEvent(initialState: false);
		d2 = new AutoResetEvent(initialState: false);
		d3 = new AutoResetEvent(initialState: false);
		d4 = new AutoResetEvent(initialState: false);
		d5 = new AutoResetEvent(initialState: false);
		d6 = new AutoResetEvent(initialState: false);
		d8 = false;
		try
		{
			this.o = Assembly.GetExecutingAssembly().Location.ToString();
			this.p = FileVersionInfo.GetVersionInfo(this.o);
			this.q = this.o.Substring(0, checked(this.o.Length - this.n.Length - 1));
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.q = "";
			ProjectData.ClearProjectError();
		}
		e();
		f();
		d();
		c();
		b();
		a();
		l();
		s().DataReceived += new SerialDataReceivedEventHandler(a);
		q();
		this.m_k = new global::b();
		this.m = new j();
		d0.Set();
		d1.Set();
		d2.Set();
		d3.Set();
		d4.Set();
		d6.Set();
	}

	private void q()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Invalid comparison between Unknown and I4
		s().PortName = this.aj;
		s().BaudRate = this.ak;
		s().Parity = this.al;
		s().DataBits = this.am;
		s().StopBits = this.an;
		s().Handshake = this.ao;
		if ((int)s().Handshake != 2)
		{
			s().RtsEnable = false;
		}
		s().DtrEnable = false;
		s().ReadBufferSize = 4096;
		s().WriteBufferSize = 1024;
		s().ReadTimeout = 750;
		s().WriteTimeout = 750;
		s().ReceivedBytesThreshold = 1;
		s().NewLine = "\n";
	}

	internal void a(long A_0, string A_1, string A_2)
	{
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		string text = "";
		if (A_0 != 0 && (A_0 == 0 || this.z != 0))
		{
			return;
		}
		if (this.z > 0)
		{
			this.ab = this.z;
			this.aa = this.y;
		}
		this.z = A_0;
		this.y = A_1.Replace('\r'.ToString(), "<cr>").Replace('\n'.ToString(), "<lf>");
		this.ac = A_2;
		if (A_0 > 0 && this.r)
		{
			text = "ERROR " + this.z + ": " + this.y + "\r\n\r\nPrevious Error: ";
			text = ((this.ab <= 0) ? (text + "NONE") : (text + "\r\nERROR " + this.ab + ": " + this.aa));
			try
			{
				Interaction.MsgBox((object)text, (MsgBoxStyle)16, (object)"mCore .NET SMS Library Debug");
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				ProjectData.ClearProjectError();
			}
		}
		if (A_0 > 0)
		{
			this.a.a(A_0, A_1);
		}
	}

	internal void aw()
	{
		if (this.z > 0)
		{
			a(0L, "", "");
		}
	}

	internal string a(string A_0, bool A_1, bool A_2, bool A_3)
	{
		if (A_1)
		{
			if (Strings.InStr(A_0.ToUpper(), "NOKIA", (CompareMethod)0) >= 1)
			{
				A_0.ToUpper().Replace("NOKIA", "NOXKIA").Replace("OK", "")
					.Replace("NOXKIA", "NOKIA");
			}
			else
			{
				A_0 = A_0.Replace("OK", "");
			}
		}
		if (A_2)
		{
			A_0 = A_0.Replace("\r", "");
		}
		if (A_3)
		{
			A_0 = A_0.Replace("\n", "");
		}
		return A_0.Trim();
	}

	internal bool r(string A_0)
	{
		int num = 0;
		string text = "";
		try
		{
			string[] portNames = SerialPort.GetPortNames();
			string[] array = portNames;
			for (int num2 = 0; num2 < array.Length; num2 = checked(num2 + 1))
			{
				text = array[num2];
				if (Operators.CompareString(A_0.Trim().ToUpper(), text.Trim().ToUpper(), false) == 0)
				{
					return true;
				}
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			a(1051L, "Error enumerating available serial ports", "GENERAL");
			ProjectData.ClearProjectError();
		}
		return false;
	}

	internal bool v()
	{
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Invalid comparison between Unknown and I4
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Invalid comparison between Unknown and I4
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Invalid comparison between Unknown and I4
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		string text = "";
		bool result;
		try
		{
			if (s().IsOpen)
			{
				result = true;
			}
			else if (this.aj.Length < 4)
			{
				a(1010L, "COM port not defined for modem", "GENERAL");
				result = false;
			}
			else if (!this.aj.StartsWith("COM") || !Versioned.IsNumeric((object)this.aj.Substring(3, checked(this.aj.Length - 3))) || this.aj.Contains(",") || this.aj.Contains("."))
			{
				a(1002L, "Cannot find COM port", "GENERAL");
				result = false;
			}
			else if (!r(this.aj))
			{
				a(1050L, "Specified COM port does not exist", "GENERAL");
				result = false;
			}
			else
			{
				ac();
				SerialPort val = s();
				val.PortName = this.aj;
				text = "baud=" + val.BaudRate + " parity=" + ((Enum)val.Parity).ToString() + " data=" + val.DataBits + " stop=" + ((Enum)val.StopBits).ToString() + " handshake=" + ((Enum)val.Handshake).ToString();
				val = null;
				this.a.b("Opening port " + s().PortName + " [" + text + "]");
				try
				{
					s().Open();
				}
				catch (IOException ex)
				{
					ProjectData.SetProjectError((Exception)ex);
					IOException ex2 = ex;
					if ((int)s().Handshake != 2)
					{
						s().RtsEnable = false;
					}
					s().DtrEnable = false;
					s().Close();
					a(1008L, "Cannot open COM port " + s().PortName + " [" + text + "]. " + ex2.Message, "GENERAL");
					result = false;
					result = false;
					ProjectData.ClearProjectError();
					goto end_IL_0006;
				}
				catch (UnauthorizedAccessException ex3)
				{
					ProjectData.SetProjectError((Exception)ex3);
					UnauthorizedAccessException ex4 = ex3;
					if ((int)s().Handshake != 2)
					{
						s().RtsEnable = false;
					}
					s().DtrEnable = false;
					s().Close();
					result = false;
					a(1009L, "Access denied to COM port " + s().PortName + ". " + ex4.Message, "GENERAL");
					result = false;
					ProjectData.ClearProjectError();
					goto end_IL_0006;
				}
				catch (Exception ex5)
				{
					ProjectData.SetProjectError(ex5);
					Exception ex6 = ex5;
					ProjectData.ClearProjectError();
				}
				try
				{
					s().DiscardOutBuffer();
					s().DiscardInBuffer();
					if (Monitor.TryEnter(this.m_l, a4))
					{
						try
						{
							this.m_k.a = "";
						}
						catch (Exception ex7)
						{
							ProjectData.SetProjectError(ex7);
							Exception ex8 = ex7;
							ProjectData.ClearProjectError();
						}
						finally
						{
							Monitor.Exit(this.m_l);
						}
						if ((int)s().Handshake != 2)
						{
							s().RtsEnable = true;
						}
						s().DtrEnable = true;
						goto IL_0408;
					}
					aw();
					x();
					s().Close();
					a(9011L, "Thread monitor enter attempt timeout [GSM:ComConnect:A]", "GENERAL");
					result = false;
				}
				catch (Exception ex9)
				{
					ProjectData.SetProjectError(ex9);
					Exception ex10 = ex9;
					this.a.b("[GSM:ComConnect:1] " + ex10.Message);
					ProjectData.ClearProjectError();
					goto IL_0408;
				}
			}
			goto end_IL_0006;
			IL_0408:
			if (aa())
			{
				if (Operators.CompareString(m(), "5", false) == 0)
				{
					j();
				}
				if (ah())
				{
					if (n())
					{
						if (bl && Operators.CompareString(bh, "READY", false) == 0)
						{
							goto IL_04d9;
						}
						this.a.b("Verifying network registration status");
						if (aj())
						{
							goto IL_04d9;
						}
						try
						{
							aw();
							x();
							s().Close();
							a(bn, bm, "GENERAL");
						}
						catch (Exception ex11)
						{
							ProjectData.SetProjectError(ex11);
							Exception ex12 = ex11;
							this.a.b("[GSM:ComConnect:5] " + ex12.Message);
							ProjectData.ClearProjectError();
						}
						result = false;
					}
					else
					{
						try
						{
							x();
							s().Close();
						}
						catch (Exception ex13)
						{
							ProjectData.SetProjectError(ex13);
							Exception ex14 = ex13;
							this.a.b("[GSM:ComConnect:6] " + ex14.Message);
							ProjectData.ClearProjectError();
						}
						result = false;
					}
				}
				else
				{
					try
					{
						aw();
						x();
						s().Close();
						a(bj, bi, "GENERAL");
					}
					catch (Exception ex15)
					{
						ProjectData.SetProjectError(ex15);
						Exception ex16 = ex15;
						this.a.b("[GSM:ComConnect:9] " + ex16.Message);
						ProjectData.ClearProjectError();
					}
					result = false;
				}
			}
			else
			{
				try
				{
					aw();
					x();
					s().Close();
					a(1046L, "Modem not found on the specified serial port", "GENERAL");
				}
				catch (Exception ex17)
				{
					ProjectData.SetProjectError(ex17);
					Exception ex18 = ex17;
					this.a.b("[GSM:ComConnect:10] " + ex18.Message);
					ProjectData.ClearProjectError();
				}
				result = false;
			}
			goto end_IL_0006;
			IL_04d9:
			if (y())
			{
				if (av())
				{
					if (bw)
					{
						this.a.b("Port " + s().PortName + " opened [" + text + "]");
						b(0);
						r();
						h();
						result = true;
					}
					else
					{
						try
						{
							aw();
							x();
							s().Close();
							a(1044L, "Modem does not support PDU mode", "GENERAL");
						}
						catch (Exception ex19)
						{
							ProjectData.SetProjectError(ex19);
							Exception ex20 = ex19;
							this.a.b("[GSM:ComConnect:2] " + ex20.Message);
							ProjectData.ClearProjectError();
						}
						result = false;
					}
				}
				else
				{
					try
					{
						aw();
						x();
						s().Close();
						a(1043L, "Cannot check PDU mode support", "GENERAL");
					}
					catch (Exception ex21)
					{
						ProjectData.SetProjectError(ex21);
						Exception ex22 = ex21;
						this.a.b("[GSM:ComConnect:3] " + ex22.Message);
						ProjectData.ClearProjectError();
					}
					result = false;
				}
			}
			else
			{
				try
				{
					x();
					s().Close();
				}
				catch (Exception ex23)
				{
					ProjectData.SetProjectError(ex23);
					Exception ex24 = ex23;
					this.a.b("[GSM:ComConnect:4] " + ex24.Message);
					ProjectData.ClearProjectError();
				}
				result = false;
			}
			end_IL_0006:;
		}
		catch (Exception ex25)
		{
			ProjectData.SetProjectError(ex25);
			Exception ex26 = ex25;
			this.a.b("[GSM:ComConnect:8] " + ex26.Message);
			try
			{
				x();
				s().Close();
			}
			catch (Exception ex27)
			{
				ProjectData.SetProjectError(ex27);
				Exception ex28 = ex27;
				ProjectData.ClearProjectError();
			}
			result = false;
			ProjectData.ClearProjectError();
		}
		return result;
	}

	internal bool ak()
	{
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Invalid comparison between Unknown and I4
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Invalid comparison between Unknown and I4
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Expected O, but got Unknown
		try
		{
			if (!s().IsOpen)
			{
				return true;
			}
			this.a.b("Closing port " + s().PortName);
			if ((int)s().Handshake != 2)
			{
				s().RtsEnable = false;
			}
			s().DtrEnable = false;
			if (Monitor.TryEnter(this.m_l, a4))
			{
				try
				{
					this.m_k.a = "";
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					ProjectData.ClearProjectError();
				}
				finally
				{
					Monitor.Exit(this.m_l);
				}
			}
			else
			{
				a(9011L, "Thread monitor enter attempt timeout [GSM:ComDisconnect:A]", "GENERAL");
				aw();
			}
			try
			{
				s().Close();
			}
			catch (InvalidOperationException ex3)
			{
				ProjectData.SetProjectError((Exception)ex3);
				InvalidOperationException ex4 = ex3;
				bool flag = true;
				a(1011L, "COM port " + s().PortName + " is already closed. " + ex4.Message, "GENERAL");
				ProjectData.ClearProjectError();
			}
			this.a.b("Port " + s().PortName + " closed");
		}
		catch (Exception ex5)
		{
			ProjectData.SetProjectError(ex5);
			Exception ex6 = ex5;
			this.a.b("[GSM:ComDisconnect:1] " + ex6.Message);
			if ((int)s().Handshake != 2)
			{
				s().RtsEnable = false;
			}
			s().DtrEnable = false;
			if (Monitor.TryEnter(this.m_l, a4))
			{
				try
				{
					this.m_k.a = "";
				}
				catch (Exception ex7)
				{
					ProjectData.SetProjectError(ex7);
					Exception ex8 = ex7;
					ProjectData.ClearProjectError();
				}
				finally
				{
					Monitor.Exit(this.m_l);
				}
			}
			else
			{
				a(9011L, "Thread monitor enter attempt timeout [GSM:ComDisconnect:B]", "GENERAL");
				aw();
			}
			ProjectData.ClearProjectError();
		}
		finally
		{
			s().DataReceived -= new SerialDataReceivedEventHandler(a);
			if (s() != null)
			{
				((Component)(object)s()).Dispose();
			}
			if (s() != null)
			{
				a((SerialPort)null);
			}
			a(new SerialPort());
			q();
			s().DataReceived += new SerialDataReceivedEventHandler(a);
		}
		ac();
		return true;
	}

	internal void ac()
	{
		bv = false;
		bw = false;
		bh = "";
		bl = false;
		a7 = false;
	}

	internal void x()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		if ((int)s().Handshake != 2)
		{
			s().RtsEnable = false;
		}
		s().DtrEnable = false;
		s().DiscardOutBuffer();
		s().DiscardInBuffer();
		if (Monitor.TryEnter(this.m_l, a4))
		{
			try
			{
				this.m_k.a = "";
				return;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				ProjectData.ClearProjectError();
				return;
			}
			finally
			{
				Monitor.Exit(this.m_l);
			}
		}
		a(9011L, "Thread monitor enter attempt timeout [GSM:ResetPortForClose:A]", "GENERAL");
		aw();
	}

	internal string n(string A_0)
	{
		string text = "";
		bool flag = false;
		char[] trimChars = "\r\n".ToCharArray();
		try
		{
			this.a.b("Send AT command [" + A_0 + "]");
			aw();
			if (!s().IsOpen)
			{
				this.a.b("Serial port is not open");
				v();
			}
			if (!s().IsOpen || this.z != 0)
			{
				this.a.b("Modem connection and/or initialization failed");
			}
			else
			{
				try
				{
					c4 = true;
					if (A_0.ToUpper().StartsWith("AT+CMGL=") || A_0.ToUpper().StartsWith("AT+CMGR"))
					{
						c7 = true;
					}
					if (A_0.ToUpper().StartsWith("AT+CMGS="))
					{
						c5 = true;
					}
					if (A_0.ToUpper().StartsWith("AT+CMGD="))
					{
						c8 = true;
					}
					if (Monitor.TryEnter(this.m_l, a4))
					{
						try
						{
							this.m_k.a = "";
						}
						catch (Exception ex)
						{
							ProjectData.SetProjectError(ex);
							Exception ex2 = ex;
							ProjectData.ClearProjectError();
						}
						finally
						{
							Monitor.Exit(this.m_l);
						}
					}
					else
					{
						string text2 = text;
						c4 = false;
						a(9011L, "Thread monitor enter attempt timeout [GSM:SendATCommand:A]", "GENERAL");
					}
					if (c4)
					{
						this.a.b("Send: " + A_0);
						s().Write(A_0);
					}
				}
				catch (InvalidOperationException ex3)
				{
					ProjectData.SetProjectError((Exception)ex3);
					InvalidOperationException ex4 = ex3;
					string text2 = text;
					c4 = false;
					a(1012L, "Cannot send command to modem (Port not open). " + ex4.Message, "GENERAL");
					ProjectData.ClearProjectError();
				}
				catch (TimeoutException ex5)
				{
					ProjectData.SetProjectError((Exception)ex5);
					TimeoutException ex6 = ex5;
					string text2 = text;
					c4 = false;
					a(1013L, "Cannot send command to modem (Timeout). " + ex6.Message, "GENERAL");
					ProjectData.ClearProjectError();
				}
				catch (Exception ex7)
				{
					ProjectData.SetProjectError(ex7);
					Exception ex8 = ex7;
					ProjectData.ClearProjectError();
				}
				if (c4)
				{
					DateTime now = DateTime.Now;
					while (!flag && (double)DateAndTime.DateDiff((DateInterval)9, now, DateTime.Now, (FirstDayOfWeek)1, (FirstWeekOfYear)1) < (double)this.av / 1000.0)
					{
						Thread.Sleep(checked((int)Math.Round((double)a3 / 2.0)));
						text += p();
						if (text.Trim().Length > 0 && string.Compare(text, A_0, ignoreCase: true) != 0)
						{
							flag = true;
							break;
						}
					}
					c4 = false;
					if (!flag)
					{
						string text2 = text;
						a(1014L, "Cannot read modem response", "GENERAL");
					}
					if (flag && text.Length > 0)
					{
						text = text.Replace(A_0, "");
						text = text.Trim(trimChars);
					}
					this.a.b("Recv: " + text);
				}
			}
		}
		catch (Exception ex9)
		{
			ProjectData.SetProjectError(ex9);
			Exception ex10 = ex9;
			this.a.b("[GSM:SendATCommand:1] " + ex10.Message);
			c4 = false;
			ProjectData.ClearProjectError();
		}
		c4 = false;
		c5 = false;
		c7 = false;
		c8 = false;
		return text;
	}

	private string p()
	{
		string result = "";
		try
		{
			if (Monitor.TryEnter(this.m_l, a4))
			{
				try
				{
					result = this.m_k.a;
					this.m_k.a = "";
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					ProjectData.ClearProjectError();
				}
				finally
				{
					Monitor.Exit(this.m_l);
				}
			}
			else
			{
				a(9011L, "Thread monitor enter attempt timeout [GSM:GetModemResponse:A]", "GENERAL");
			}
		}
		catch (Exception ex3)
		{
			ProjectData.SetProjectError(ex3);
			Exception ex4 = ex3;
			this.a.b("[GSM:GetModemResponse:1] " + ex4.Message);
			ProjectData.ClearProjectError();
		}
		return result;
	}

	private void a(object A_0, SerialDataReceivedEventArgs A_1)
	{
		checked
		{
			if (Monitor.TryEnter(this.m_l, a4))
			{
				try
				{
					StringBuilder stringBuilder = new StringBuilder("");
					string text = "";
					int num = 0;
					string text2 = "";
					char[] array = "\r\n".ToCharArray();
					bool flag = false;
					int num2 = 2;
					if (a3 > 25)
					{
						num2 = (int)Math.Round((double)a3 / 25.0);
					}
					if (num2 <= 1)
					{
						num2 = 2;
					}
					text = "";
					text2 = "";
					num = 0;
					try
					{
						if (s().BytesToRead <= 0)
						{
							return;
						}
						while (s().BytesToRead > 0)
						{
							flag = false;
							text2 = "";
							try
							{
								text2 = s().ReadLine();
								stringBuilder.Append(text2);
								stringBuilder.Append('\n');
								flag = true;
							}
							catch (TimeoutException ex)
							{
								ProjectData.SetProjectError((Exception)ex);
								TimeoutException ex2 = ex;
								stringBuilder.Append(text2);
								ProjectData.ClearProjectError();
							}
							if (!flag)
							{
								stringBuilder.Append(s().ReadExisting());
							}
							if (s().BytesToRead <= 0)
							{
								int num3 = num2;
								for (int num4 = 1; num4 <= num3; num4++)
								{
									Thread.Sleep(25);
									if (s().BytesToRead > 0)
									{
										break;
									}
								}
							}
							Application.DoEvents();
						}
						text = stringBuilder.ToString();
						if (text.Trim().Length > 0)
						{
							if (text.ToUpper().Trim().Contains("+CMT:") || text.ToUpper().Trim().Contains("+CMTI:") || text.ToUpper().Trim().Contains("+CDS:") || text.ToUpper().Trim().Contains("+CDSI:") || text.ToUpper().Trim().Contains("RING") || text.ToUpper().Trim().Contains("+CLIP:") || text.ToUpper().Trim().Contains("+CUSD:"))
							{
								string[] array2 = Strings.Split(text, "\r\n", -1, (CompareMethod)0);
								text = "";
								for (; num <= array2.GetUpperBound(0); num++)
								{
									if (array2[num].ToUpper().Trim().StartsWith("+CMT:") || array2[num].ToUpper().Trim().StartsWith("+CMTI:") || array2[num].ToUpper().Trim().StartsWith("+CDS:") || array2[num].ToUpper().Trim().StartsWith("+CDSI:") || array2[num].ToUpper().Trim().StartsWith("RING") || array2[num].ToUpper().Trim().StartsWith("+CLIP:") || array2[num].ToUpper().Trim().StartsWith("+CUSD:"))
									{
										if (array2[num].ToUpper().Trim().StartsWith("+CMT"))
										{
											if (array2[num].ToUpper().Trim().StartsWith("+CMT:"))
											{
												try
												{
													bool flag2 = false;
													string text3 = array2[num];
													while (!flag2)
													{
														if (array2[num + 1].Trim().Length > 0)
														{
															text3 = text3 + "\r\n" + array2[num + 1];
															flag2 = true;
														}
														num++;
													}
													if (this.s)
													{
														this.m_k.c.Add((object)text3, (string)null, (object)null, (object)null);
													}
												}
												catch (Exception ex3)
												{
													ProjectData.SetProjectError(ex3);
													Exception ex4 = ex3;
													ProjectData.ClearProjectError();
												}
											}
											else if (array2[num].ToUpper().Trim().StartsWith("+CMTI:") && this.s)
											{
												this.m_k.b.Add((object)array2[num], (string)null, (object)null, (object)null);
											}
										}
										else if (array2[num].ToUpper().Trim().StartsWith("+CDS"))
										{
											if (array2[num].ToUpper().Trim().StartsWith("+CDS:"))
											{
												try
												{
													bool flag3 = false;
													string text4 = array2[num];
													while (!flag3)
													{
														if (array2[num + 1].Trim().Length > 0)
														{
															text4 = text4 + "\r\n" + array2[num + 1];
															flag3 = true;
														}
														num++;
													}
													if (this.u)
													{
														this.m_k.d.Add((object)text4, (string)null, (object)null, (object)null);
													}
												}
												catch (Exception ex5)
												{
													ProjectData.SetProjectError(ex5);
													Exception ex6 = ex5;
													ProjectData.ClearProjectError();
												}
											}
											else if (array2[num].ToUpper().Trim().StartsWith("+CDSI:") && this.u)
											{
												this.m_k.e.Add((object)array2[num], (string)null, (object)null, (object)null);
											}
										}
										else if (array2[num].ToUpper().Trim().StartsWith("RING"))
										{
											num++;
										}
										else if (array2[num].ToUpper().Trim().StartsWith("+CLIP:"))
										{
											if (this.v && !this.m_k.f.Contains(array2[num].ToUpper()))
											{
												this.m_k.f.Add((object)array2[num], array2[num].ToUpper(), (object)null, (object)null);
											}
											num++;
										}
										else if (array2[num].ToUpper().Trim().StartsWith("+CUSD:") && this.w)
										{
											this.m_k.h.Add((object)array2[num], (string)null, (object)null, (object)null);
										}
									}
									else
									{
										text = ((text.Length > 0) ? (text + "\r\n" + array2[num]) : array2[num]);
									}
								}
								if (this.m_k.f.Count > 0)
								{
									ThreadPool.RegisterWaitForSingleObject(d0, g, null, -1, executeOnlyOnce: true);
								}
								if (this.m_k.c.Count > 0)
								{
									ThreadPool.RegisterWaitForSingleObject(d1, e, null, -1, executeOnlyOnce: true);
								}
								if (this.m_k.b.Count > 0)
								{
									ThreadPool.RegisterWaitForSingleObject(d2, f, null, -1, executeOnlyOnce: true);
								}
								if (this.m_k.d.Count > 0)
								{
									ThreadPool.RegisterWaitForSingleObject(d3, c, null, -1, executeOnlyOnce: true);
								}
								if (this.m_k.e.Count > 0)
								{
									ThreadPool.RegisterWaitForSingleObject(d4, b, null, -1, executeOnlyOnce: true);
								}
								if (this.m_k.h.Count > 0)
								{
									ThreadPool.RegisterWaitForSingleObject(d5, a, null, -1, executeOnlyOnce: true);
								}
							}
						}
						else
						{
							this.a.b("SerialDataReceived event returned zero characters");
						}
					}
					catch (Exception ex7)
					{
						ProjectData.SetProjectError(ex7);
						Exception ex8 = ex7;
						this.a.b("[GSM:SerialDataReceived:1] " + ex8.Message);
						ProjectData.ClearProjectError();
					}
					if (text.Length > 0)
					{
						if (c4)
						{
							global::b b2 = this.m_k;
							b2.a += text;
						}
						else
						{
							this.m_k.a = "";
						}
					}
					return;
				}
				catch (Exception ex9)
				{
					ProjectData.SetProjectError(ex9);
					Exception ex10 = ex9;
					ProjectData.ClearProjectError();
					return;
				}
				finally
				{
					Monitor.Exit(this.m_l);
				}
			}
			a(9011L, "Thread monitor enter attempt timeout [GSM:SerialDataReceived:A]", "GENERAL");
		}
	}

	internal void r()
	{
		string text = "";
		if (this.v)
		{
			text = a("AT+CLIP=1\r").ToUpper();
			if (text.Contains("ERROR") || !text.Contains("OK"))
			{
				this.v = false;
				a(1036L, "Modem does not support new call indication with caller ID", "GENERAL");
			}
		}
		else
		{
			text = a("AT+CLIP=0\r");
		}
	}

	internal void b(int A_0)
	{
		string a_ = "";
		if (!s().IsOpen)
		{
			v();
		}
		if (A_0 == 0)
		{
			cs = "2";
			cr = "0";
		}
		if (A_0 == 1)
		{
			if (this.s)
			{
				switch (this.x)
				{
				case true:
					co = "2";
					break;
				case false:
					co = "1";
					break;
				}
			}
			else
			{
				co = "0";
			}
		}
		if (A_0 == 2)
		{
			if (this.u)
			{
				cp = "1";
			}
			else
			{
				cp = "0";
			}
		}
		if (A_0 == 3)
		{
			if (!this.s)
			{
				return;
			}
			switch (this.x)
			{
			case true:
				co = "2";
				break;
			case false:
				co = "1";
				break;
			}
		}
		long num = default(long);
		if (!a(cs, co, cq, cp, cr) && s().IsOpen)
		{
			if (A_0 == 0)
			{
				this.a.b("Modem does not support <mode>=" + cs + " and <bfr>=" + cr);
				cr = "1";
				if (!a(cs, co, cq, cp, cr))
				{
					this.a.b("Modem does not support <mode>=" + cs + " and <bfr>=" + cr);
					cr = "0";
					cs = "3";
					if (!a(cs, co, cq, cp, cr))
					{
						this.a.b("Modem does not support <mode>=" + cs + " and <bfr>=" + cr);
						cr = "1";
						if (!a(cs, co, cq, cp, cr))
						{
							this.a.b("Modem does not support <mode>=" + cs + " and <bfr>=" + cr);
							this.a.b("Modem does not support AT+CNMI command with <mode>=2 or 3 and <bfr>=0 or 1");
							cs = "2";
							cr = "0";
						}
					}
				}
			}
			if (A_0 == 1)
			{
				this.a.b("Modem does not support <mt>=" + co);
				if (this.s)
				{
					switch (this.x)
					{
					case true:
						co = "1";
						break;
					case false:
						co = "2";
						break;
					}
					if (!a(cs, co, cq, cp, cr))
					{
						this.a.b("Modem does not support <mt>=" + co);
						co = "3";
						if (!a(cs, co, cq, cp, cr))
						{
							this.a.b("Modem does not support <mt>=" + co);
							co = "0";
							a_ = "Modem does not support new message indication";
							num = 1038L;
							this.s = false;
							this.x = false;
						}
						else if (!this.x)
						{
							a_ = "Modem does not support non auto delete type new message indication";
							num = 1038L;
							this.x = true;
						}
					}
				}
				else
				{
					a_ = "Error disabling new message indication";
					num = 1039L;
					this.s = false;
				}
			}
			if (A_0 == 2)
			{
				this.a.b("Modem does not support <ds>=" + cp);
				if (this.u)
				{
					cp = "2";
					if (!a(cs, co, cq, cp, cr))
					{
						this.a.b("Modem does not support <ds>=" + cp);
						a_ = "Modem does not support delivery report indication command";
						num = 1040L;
						this.u = false;
						bu = false;
					}
				}
				else
				{
					a_ = "Error disabling delivery report indication";
					num = 1041L;
					this.u = false;
					bu = false;
				}
			}
			if (A_0 == 3)
			{
				this.a.b("Modem does not support <mt>=" + co);
				if (this.x)
				{
					co = "3";
					if (!a(cs, co, cq, cp, cr))
					{
						this.a.b("Modem does not support <mt>=" + co);
						co = "1";
						if (!a(cs, co, cq, cp, cr))
						{
							this.a.b("Modem does not support <mt>=" + co);
							co = "0";
							a_ = "Modem does not support new message indication";
							num = 1038L;
							this.s = false;
							this.x = false;
						}
					}
				}
				else
				{
					co = "2";
					if (!a(cs, co, cq, cp, cr))
					{
						this.a.b("Modem does not support <mt>=" + co);
						co = "3";
						if (!a(cs, co, cq, cp, cr))
						{
							this.a.b("Modem does not support <mt>=" + co);
							co = "0";
							a_ = "Modem does not support new message indication";
							num = 1038L;
							this.s = false;
							this.x = false;
						}
						else
						{
							a_ = "Modem does not support non auto delete type new message indication";
							num = 1038L;
							this.x = true;
						}
					}
					else
					{
						a_ = "Modem does not support non auto delete type new message indication";
						num = 1038L;
						this.x = true;
					}
				}
			}
		}
		if (num > 0)
		{
			a(num, a_, "GENERAL");
		}
	}

	private bool a(string A_0, string A_1, string A_2, string A_3, string A_4)
	{
		string text = "";
		char[] trimChars = "\r\n".ToCharArray();
		text = a("AT+CNMI=" + cs + "," + co + "," + cq + "," + cp + "," + cr + "\r");
		text = text.Trim().Trim(trimChars).Trim()
			.ToUpper();
		if (text.Trim().Length <= 0 || text.Contains("ERROR") || !text.Contains("OK"))
		{
			return false;
		}
		return true;
	}

	private bool o()
	{
		string text = "";
		string text2 = "";
		bool result;
		try
		{
			this.a.b("Verifying connection");
			text = "AT\r";
			text2 = a(text);
			if (text2.ToUpper().Contains("OK"))
			{
				this.a.b("Connection verified OK");
				result = true;
			}
			else
			{
				a(1015L, "Connection verification failed", "GENERAL");
				ak();
				result = false;
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:VerifyConnection:1] " + ex2.Message);
			result = false;
			ProjectData.ClearProjectError();
		}
		return result;
	}

	private bool n()
	{
		string text = "";
		string text2 = "";
		bool flag = false;
		string text3 = m();
		long num = 60000L;
		long num2 = this.av;
		if (Operators.CompareString(text3, "5", false) == 0)
		{
			j();
			try
			{
				this.a.b("Verify PIN status after full functionality mode");
				if (!ah())
				{
					if (bj == 0)
					{
						bj = 2030L;
						bi = "Error verifying PIN status after full functionality mode";
					}
					a(bj, bi, "GENERAL");
					return false;
				}
				if (bj != 0)
				{
					a(bj, bi, "GENERAL");
					return false;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				this.a.b("[GSM:InitializeModem:8] " + ex2.Message);
				aw();
				ProjectData.ClearProjectError();
			}
		}
		else if (Operators.CompareString(text3, "1", false) == 0)
		{
			a(2031L, "The modem is not in a state to accept AT Commands", "GENERAL");
			return false;
		}
		try
		{
			if (a6.Trim().Length > 0)
			{
				text = a6.Trim() + "\r";
				if (!text.ToUpper().StartsWith("AT"))
				{
					text = "AT" + text;
				}
				this.a.b("Initializing modem (user defined)");
				text2 = a(text);
				if (text2.ToUpper().Contains("ERROR"))
				{
					a(1021L, "Error response to user defined modem initialization string", "");
				}
			}
			if (this.z != 0)
			{
				a(1022L, "User defined modem initialization failed", "");
				aw();
			}
		}
		catch (Exception ex3)
		{
			ProjectData.SetProjectError(ex3);
			Exception ex4 = ex3;
			this.a.b("[GSM:InitializeModem:2] " + ex4.Message);
			aw();
			ProjectData.ClearProjectError();
		}
		try
		{
			this.a.b("Disable modem echo");
			text = "ATE0\r";
			text2 = a(text);
			if (text2.Trim().Length <= 0)
			{
				a(1023L, "No response to disable modem echo command", "");
				aw();
				flag = true;
			}
			else if (this.z != 0)
			{
				a(1024L, "Error disabling modem echo", "");
				aw();
				flag = true;
			}
			else if (text2.ToUpper().Contains("ERROR"))
			{
				a(1025L, "Error response to disable modem echo", "");
				aw();
				flag = true;
			}
		}
		catch (Exception ex5)
		{
			ProjectData.SetProjectError(ex5);
			Exception ex6 = ex5;
			this.a.b("[GSM:InitializeModem:3] " + ex6.Message);
			aw();
			ProjectData.ClearProjectError();
		}
		try
		{
			this.a.b("Ignore DTR toggle setting");
			text = "AT&D0\r";
			text2 = a(text);
			if (text2.Trim().Length <= 0)
			{
				a(1033L, "No response to ignore DTR toggle setting", "");
				aw();
				flag = true;
			}
			else if (this.z != 0)
			{
				a(1034L, "Error ignoring DTR toggle setting", "");
				aw();
				flag = true;
			}
			else if (text2.ToUpper().Contains("ERROR"))
			{
				a(1035L, "Error response to ignore DTR toggle setting command", "");
				aw();
				flag = true;
			}
		}
		catch (Exception ex7)
		{
			ProjectData.SetProjectError(ex7);
			Exception ex8 = ex7;
			this.a.b("[GSM:InitializeModem:4] " + ex8.Message);
			aw();
			ProjectData.ClearProjectError();
		}
		try
		{
			this.a.b("Enable long result code format");
			text = "ATV1\r";
			text2 = a(text);
			if (text2.Trim().Length <= 0)
			{
				a(1026L, "No response to enable long result code format command", "");
				aw();
				flag = true;
			}
			else if (this.z != 0)
			{
				a(1027L, "Error enabling long result code format", "");
				aw();
				flag = true;
			}
			else if (text2.ToUpper().Contains("ERROR"))
			{
				a(1028L, "Error response to enable long result code format", "");
				aw();
				flag = true;
			}
		}
		catch (Exception ex9)
		{
			ProjectData.SetProjectError(ex9);
			Exception ex10 = ex9;
			this.a.b("[GSM:InitializeModem:5] " + ex10.Message);
			aw();
			ProjectData.ClearProjectError();
		}
		flag = !i();
		try
		{
			this.a.b("Disable network registration unsolicited result code");
			text = "AT+CREG=0\r";
			text2 = a(text);
			if (text2.Trim().Length <= 0)
			{
				a(1019L, "No response to disable network registration unsolicited result code", "");
				aw();
				flag = true;
			}
			else if (this.z != 0)
			{
				a(1020L, "Disabling network registration unsolicited result code failed", "");
				aw();
				flag = true;
			}
		}
		catch (Exception ex11)
		{
			ProjectData.SetProjectError(ex11);
			Exception ex12 = ex11;
			this.a.b("[GSM:InitializeModem:9] " + ex12.Message);
			aw();
			ProjectData.ClearProjectError();
		}
		if (!aj())
		{
			this.a.b(bm);
			aw();
			try
			{
				k();
			}
			catch (Exception ex13)
			{
				ProjectData.SetProjectError(ex13);
				Exception ex14 = ex13;
				this.a.b("[GSM:InitializeModem:10] " + ex14.Message);
				aw();
				ProjectData.ClearProjectError();
			}
			Thread.Sleep(2500);
			try
			{
				j();
			}
			catch (Exception ex15)
			{
				ProjectData.SetProjectError(ex15);
				Exception ex16 = ex15;
				this.a.b("[GSM:InitializeModem:11] " + ex16.Message);
				aw();
				ProjectData.ClearProjectError();
			}
			Thread.Sleep(2500);
			try
			{
				this.a.b("Enable automatic network selection");
				text = "AT+COPS=0\r";
				this.av = num;
				text2 = a(text);
				this.av = num2;
				if (text2.Trim().Length <= 0)
				{
					a(1047L, "No response to enable automatic network selection command", "");
					aw();
					flag = true;
				}
				else if (this.z != 0)
				{
					a(1048L, "Error enabling automatic network selection", "");
					aw();
					flag = true;
				}
				else if (text2.ToUpper().Contains("ERROR"))
				{
					a(1049L, "Error response to automatic network selection command", "");
					aw();
					flag = true;
				}
			}
			catch (Exception ex17)
			{
				ProjectData.SetProjectError(ex17);
				Exception ex18 = ex17;
				this.a.b("[GSM:InitializeModem:6] " + ex18.Message);
				aw();
				ProjectData.ClearProjectError();
			}
		}
		if (flag)
		{
			a(1016L, "At least one default modem initialization command failed", "");
			aw();
		}
		try
		{
			this.a.b("Initializing call hang-up command string");
			text = "AT+CHUP=?\r";
			text2 = a(text);
			if (text2.Trim().Length > 0 && !text2.ToUpper().Contains("ERROR") && text2.ToUpper().Contains("OK"))
			{
				ct = "AT+CHUP";
				this.a.b("Call hang-up command set to AT+CHUP");
			}
			else
			{
				this.a.b("Call hang-up command set to ATH");
			}
		}
		catch (Exception ex19)
		{
			ProjectData.SetProjectError(ex19);
			Exception ex20 = ex19;
			this.a.b("[GSM:InitializeModem:7] " + ex20.Message);
			aw();
			ProjectData.ClearProjectError();
		}
		return true;
	}

	private string m()
	{
		string text = "";
		string text2 = "-1";
		try
		{
			this.a.b("Checking ME activity status");
			text = a("AT+CPAS\r").ToUpper();
			if (text.Length > 0 && !text.ToUpper().Contains("ERROR"))
			{
				text2 = a(text, A_1: true, A_2: true, A_3: true).Replace("+CPAS:", "").Trim();
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:GetModemActivityStatus:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		if (dx.Contains(text2))
		{
			this.a.b(dx[text2].ToString());
		}
		return text2;
	}

	private void l()
	{
		dx.Add("-1", "Error checking ME state");
		dx.Add("0", "ME is in ready state");
		dx.Add("1", "ME is not available");
		dx.Add("2", "ME is in a unknown state");
		dx.Add("3", "ME is ready but ringer is active");
		dx.Add("4", "ME is ready but call is in progress");
		dx.Add("5", "ME is asleep (low functionality state)");
	}

	private void k()
	{
		string text = "";
		try
		{
			this.a.b("Set ME to minimum functionality mode");
			text = a("AT+CFUN=0\r");
			if (text.Trim().Length <= 0)
			{
				a(1052L, "No response to minimum functionality mode command", "");
				aw();
			}
			else if (this.z != 0)
			{
				a(1053L, "Setting ME to minimum functionality mode failed", "");
				aw();
			}
			Thread.Sleep(2000);
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:PutModemToSleep:1] " + ex2.Message);
			aw();
			ProjectData.ClearProjectError();
		}
	}

	private void j()
	{
		string text = "";
		try
		{
			this.a.b("Set ME to full functionality mode");
			text = a("AT+CFUN=1\r");
			if (text.Trim().Length <= 0)
			{
				a(1017L, "No response to full functionality mode command", "");
				aw();
			}
			else if (this.z != 0)
			{
				a(1018L, "Setting of ME to full functionality mode failed", "");
				aw();
			}
			Thread.Sleep(2000);
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:WakeUpModem:1] " + ex2.Message);
			aw();
			ProjectData.ClearProjectError();
		}
	}

	private bool i()
	{
		string text = "";
		string text2 = "";
		bool result = true;
		try
		{
			this.a.b("Enable result code with numeric values");
			text = "AT+CMEE=1\r";
			text2 = a(text);
			if (text2.Trim().Length <= 0)
			{
				a(1029L, "No response to enable numeric result code format", "");
				aw();
				result = false;
			}
			else if (this.z != 0)
			{
				a(1030L, "Error enabling numeric result code format", "");
				aw();
				result = false;
			}
			else if (text2.ToUpper().Contains("ERROR"))
			{
				a(1031L, "Error response to enable numeric result code format", "");
				aw();
				result = false;
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:EnableResultCodeWithNumericValues:1] " + ex2.Message);
			aw();
			ProjectData.ClearProjectError();
		}
		return result;
	}

	internal bool y()
	{
		string text = "";
		bool flag = false;
		try
		{
			cu = false;
			this.a.b("Checking for SMS support and new message acknowledgement requirement");
			text = a("AT+CSMS?\r").ToUpper();
			if (text.Length > 0 && !text.ToUpper().Contains("ERROR") && text.Contains("OK"))
			{
				text = a(text, A_1: true, A_2: true, A_3: true).Replace("+CSMS:", "").Trim();
				if (text.Contains(","))
				{
					string[] array = text.Split(",".ToCharArray());
					if (Operators.CompareString(array[1].Trim(), "1", false) == 0 && Operators.CompareString(array[2].Trim(), "1", false) == 0)
					{
						flag = true;
						this.a.b("Modem supports MT & MO type SMS");
					}
					if (flag)
					{
						if (Operators.CompareString(array[0].Trim(), "1", false) == 0)
						{
							this.a.b("New message acknowledgement required");
							cu = true;
						}
						else
						{
							this.a.b("New message acknowledgement not required");
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:CheckSMSSupport:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		if (!flag)
		{
			a(1042L, "Modem does not support SMS service", "GENERAL");
		}
		return flag;
	}

	private string a(string A_0)
	{
		string text = "";
		int num = 1;
		try
		{
			num = 1;
			do
			{
				text = n(A_0).ToUpper();
				if (text.Length <= 0 || (!text.Contains("CMS ERROR") && !text.Contains("CME ERROR")) || !text.Contains("515") || num >= 30)
				{
					break;
				}
				this.a.b("Modem is busy initializing or previous command processing in progress");
				Thread.Sleep(1000);
				this.a.b("Retrying command after " + 1000 + "ms delay");
				num = checked(num + 1);
			}
			while (num <= 30);
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:SendATCommandWithBusyCheck:1] " + ex2.Message);
			text = "";
			ProjectData.ClearProjectError();
		}
		return text;
	}

	internal bool aa()
	{
		string text = "";
		string text2 = "";
		bool result = false;
		try
		{
			this.a.b("Verifying if port connected");
			result = false;
			if (!s().IsOpen)
			{
				this.a.b("Port is not connected");
			}
			else
			{
				this.a.b("Port is connected");
				this.a.b("Verifying if modem is connected");
				text = "AT\r";
				text2 = a(text);
				if (!text2.ToUpper().Contains("OK"))
				{
					this.a.b("Modem is not connected");
				}
				else
				{
					result = true;
					this.a.b("Modem is connected");
				}
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:IsModemConnected:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		return result;
	}

	private void h()
	{
		bool flag = false;
		if (a7)
		{
			this.a.b("Restore message memory setting");
			flag = i(a8);
		}
	}

	internal bool i(string A_0)
	{
		string text = "";
		string text2 = "";
		if (string.Compare(A_0, "ME", ignoreCase: true) != 0 && string.Compare(A_0, "SM", ignoreCase: true) != 0 && string.Compare(A_0, "MT", ignoreCase: true) != 0 && string.Compare(A_0, "BM", ignoreCase: true) != 0 && string.Compare(A_0, "TA", ignoreCase: true) != 0 && string.Compare(A_0, "SR", ignoreCase: true) != 0)
		{
			a(2004L, "Invalid message memory type", "GENERAL");
			a7 = false;
			return false;
		}
		if (!bl || Operators.CompareString(bh, "READY", false) != 0)
		{
			this.a.b("Verifying network registration status");
			if (!aj())
			{
				a(bn, bm, "GENERAL");
				return false;
			}
		}
		bool result;
		try
		{
			this.a.b("Setting message memory as '" + A_0.ToUpper() + "'");
			text = "AT+CPMS=\"" + A_0 + "\",,\"" + A_0 + "\"\r";
			text2 = a(text).ToUpper();
			if (text2.Length > 0 && text2.Contains("OK") && !text2.Contains("ERROR"))
			{
				a8 = A_0.ToUpper();
				a7 = true;
				this.a.b("Message memory set as '" + A_0.ToUpper() + "'");
				result = true;
			}
			else
			{
				text = "AT+CPMS=\"" + A_0 + "\"\r";
				text2 = a(text).ToUpper();
				if (text2.Length > 0 && text2.Contains("OK") && !text2.Contains("ERROR"))
				{
					a8 = A_0.ToUpper();
					a7 = true;
					this.a.b("Message memory set as '" + A_0.ToUpper() + "'");
					result = true;
				}
				else
				{
					result = false;
				}
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:SetMessageMemory:1] " + ex2.Message);
			a7 = false;
			result = false;
			ProjectData.ClearProjectError();
		}
		return result;
	}

	internal string u()
	{
		string text = "";
		string text2 = "";
		if (!bl || Operators.CompareString(bh, "READY", false) != 0)
		{
			this.a.b("Verifying network registration status");
			if (!aj())
			{
				a(bn, bm, "GENERAL");
				return "";
			}
		}
		string result;
		try
		{
			this.a.b("Reading message memory setting");
			text = "AT+CPMS?\r";
			text2 = a(text).ToUpper();
			if (this.z == 0 && text2.Length > 0 && text2.ToUpper().Contains("OK") && !text2.ToUpper().Contains("ERROR"))
			{
				text2 = text2.ToUpper().Replace("+CPMS:", "").Trim();
				text2 = a(text2, A_1: true, A_2: true, A_3: true).Replace(" ", "").Replace("\"", "");
				string[] array = text2.Split(",".ToCharArray());
				a8 = array[0].ToUpper();
				this.a.b("Message memory type: '" + a8 + "'");
			}
			result = a8;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:GetMessageMemory:1] " + ex2.Message);
			result = a8;
			ProjectData.ClearProjectError();
		}
		return result;
	}

	internal string t()
	{
		string text = "";
		string text2 = "";
		bool flag = true;
		a9 = "";
		string result;
		try
		{
			this.a.b("Reading modem manufacturer information");
			text = "AT+GMI\r";
			text2 = a(text);
			if (this.z != 0)
			{
				goto IL_0132;
			}
			if (text2.Length <= 0 || text2.ToUpper().Contains("ERROR"))
			{
				flag = false;
				goto IL_00b1;
			}
			text2 = a(text2, A_1: true, A_2: true, A_3: true).Trim();
			if (text2.Length <= 0)
			{
				flag = false;
				goto IL_00b1;
			}
			a9 = text2;
			this.a.b("Modem manufacturer: '" + a9 + "'");
			result = a9;
			goto end_IL_0019;
			IL_00b1:
			if (flag)
			{
				goto IL_0132;
			}
			this.a.b("AT+GMI not supported. Trying AT+CGMI");
			text = "AT+CGMI\r";
			text2 = a(text);
			if (this.z != 0 || text2.Length <= 0 || text2.ToUpper().Contains("ERROR"))
			{
				goto IL_0132;
			}
			text2 = a(text2, A_1: true, A_2: true, A_3: true);
			a9 = text2;
			this.a.b("Modem manufacturer: '" + a9 + "'");
			result = a9;
			goto end_IL_0019;
			IL_0132:
			result = a9;
			end_IL_0019:;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:GetManufacturer:1] " + ex2.Message);
			a9 = "";
			result = a9;
			ProjectData.ClearProjectError();
		}
		return result;
	}

	internal string ag()
	{
		string text = "";
		string text2 = "";
		bool flag = true;
		ba = "";
		string result;
		try
		{
			this.a.b("Reading modem model information");
			text = "AT+GMM\r";
			text2 = a(text);
			if (this.z != 0)
			{
				goto IL_012d;
			}
			if (text2.Length <= 0 || text2.ToUpper().Contains("ERROR"))
			{
				flag = false;
				goto IL_00ac;
			}
			text2 = a(text2, A_1: true, A_2: true, A_3: true);
			if (text2.Length <= 0)
			{
				flag = false;
				goto IL_00ac;
			}
			ba = text2;
			this.a.b("Modem model: '" + ba + "'");
			result = ba;
			goto end_IL_0019;
			IL_00ac:
			if (flag)
			{
				goto IL_012d;
			}
			this.a.b("AT+GMM not supported. Trying AT+CGMM");
			text = "AT+CGMM\r";
			text2 = a(text);
			if (this.z != 0 || text2.Length <= 0 || text2.ToUpper().Contains("ERROR"))
			{
				goto IL_012d;
			}
			text2 = a(text2, A_1: true, A_2: true, A_3: true);
			ba = text2;
			this.a.b("Modem model: '" + ba + "'");
			result = ba;
			goto end_IL_0019;
			IL_012d:
			result = ba;
			end_IL_0019:;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:GetModel:1] " + ex2.Message);
			ba = "";
			result = ba;
			ProjectData.ClearProjectError();
		}
		return result;
	}

	internal string au()
	{
		string text = "";
		string text2 = "";
		bool flag = true;
		bc = "";
		string result;
		try
		{
			this.a.b("Reading modem IMEI");
			text = "AT+CGSN\r";
			text2 = a(text);
			if (this.z != 0)
			{
				goto IL_0172;
			}
			if (text2.Length <= 0 || !text2.ToUpper().Contains("OK") || text2.ToUpper().Contains("ERROR"))
			{
				flag = false;
				goto IL_00cd;
			}
			text2 = a(text2.Replace("+CGSN:", ""), A_1: true, A_2: true, A_3: true);
			if (text2.Length <= 0)
			{
				flag = false;
				goto IL_00cd;
			}
			bc = text2;
			this.a.b("Modem IMEI: '" + bc + "'");
			result = bc;
			goto end_IL_0019;
			IL_0172:
			result = bc;
			goto end_IL_0019;
			IL_00cd:
			if (flag)
			{
				goto IL_0172;
			}
			this.a.b("AT+CGSN not supported. Trying AT+GSN");
			text = "AT+GSN\r";
			text2 = a(text);
			if (this.z != 0 || text2.Length <= 0 || !text2.ToUpper().Contains("OK") || text2.ToUpper().Contains("ERROR"))
			{
				goto IL_0172;
			}
			text2 = a(text2.Replace("+GSN:", ""), A_1: true, A_2: true, A_3: true);
			bc = text2;
			this.a.b("Modem IMEI: '" + bc + "'");
			result = bc;
			end_IL_0019:;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:GetIMEI:1] " + ex2.Message);
			bc = "";
			result = bc;
			ProjectData.ClearProjectError();
		}
		return result;
	}

	internal string w()
	{
		string text = "";
		string text2 = "";
		bd = "";
		string result;
		try
		{
			this.a.b("Reading IMSI");
			text = "AT+CIMI\r";
			text2 = a(text);
			if (this.z != 0 || text2.Length <= 0 || !text2.ToUpper().Contains("OK") || text2.ToUpper().Contains("ERROR"))
			{
				goto IL_00c2;
			}
			text2 = a(text2.Replace("+CIMI:", ""), A_1: true, A_2: true, A_3: true);
			if (text2.Length <= 0)
			{
				goto IL_00c2;
			}
			bd = text2;
			this.a.b("IMSI: '" + bd + "'");
			result = bd;
			goto end_IL_0017;
			IL_00c2:
			result = bd;
			end_IL_0017:;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:GetIMSI:1] " + ex2.Message);
			bd = "";
			result = bd;
			ProjectData.ClearProjectError();
		}
		return result;
	}

	internal string ai()
	{
		string text = "";
		string text2 = "";
		bool flag = true;
		bb = "";
		string result;
		try
		{
			this.a.b("Reading modem firmware revision");
			text = "AT+GMR\r";
			text2 = a(text);
			if (this.z != 0)
			{
				goto IL_0144;
			}
			if (text2.Length <= 0 || !text2.ToUpper().Contains("OK") || text2.ToUpper().Contains("ERROR"))
			{
				flag = false;
				goto IL_00be;
			}
			text2 = a(text2, A_1: true, A_2: true, A_3: true);
			if (text2.Length <= 0)
			{
				flag = false;
				goto IL_00be;
			}
			bb = text2;
			this.a.b("Modem Firmware Revision: '" + bb + "'");
			result = bb;
			goto end_IL_0019;
			IL_0144:
			result = bb;
			goto end_IL_0019;
			IL_00be:
			if (flag)
			{
				goto IL_0144;
			}
			text = "AT+CGMR\r";
			text2 = a(text);
			if (this.z != 0 || text2.Length <= 0 || !text2.ToUpper().Contains("OK") || text2.ToUpper().Contains("ERROR"))
			{
				goto IL_0144;
			}
			text2 = a(text2, A_1: true, A_2: true, A_3: true);
			bb = text2;
			this.a.b("Modem Firmware Revision: '" + bb + "'");
			result = bb;
			end_IL_0019:;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:GetRevision:1] " + ex2.Message);
			bb = "";
			result = bb;
			ProjectData.ClearProjectError();
		}
		return result;
	}

	internal int af()
	{
		string text = "";
		string text2 = "";
		be = -2;
		int result;
		try
		{
			this.a.b("Reading battery charge level");
			text = "AT+CBC\r";
			text2 = a(text).ToUpper();
			if (this.z == 0 && text2.Length > 0 && text2.ToUpper().Contains("OK") && !text2.ToUpper().Contains("ERROR"))
			{
				text2 = text2.ToUpper().Replace("+CBC:", "").Trim();
				text2 = a(text2, A_1: true, A_2: true, A_3: true).Replace(" ", "");
				string[] array = text2.Split(",".ToCharArray());
				if (array.GetUpperBound(0) >= 1)
				{
					if ((Operators.CompareString(array[0].Trim(), "0", false) == 0 || Operators.CompareString(array[0].Trim(), "1", false) == 0) && Versioned.IsNumeric((object)array[1]) && !array[1].Contains(",") && !array[1].Contains("."))
					{
						be = Conversions.ToInteger(array[1]);
						this.a.b("Battery level: '" + be + "%'");
					}
					else
					{
						string text3 = array[0].Trim();
						if (Operators.CompareString(text3, "2", false) == 0)
						{
							this.a.b("No battery present in GSM modem");
						}
						else if (Operators.CompareString(text3, "3", false) == 0)
						{
							this.a.b("Modem reported a power fault condition");
						}
						be = -1;
					}
				}
			}
			if (be == -2)
			{
				be = -1;
				this.a.b("Modem does not support reading battery charge level");
			}
			result = be;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:GetBatteryLevel:1] " + ex2.Message);
			be = -1;
			result = be;
			ProjectData.ClearProjectError();
		}
		return result;
	}

	internal string ab()
	{
		string text = "";
		string text2 = "";
		bf = "";
		if (!bl || Operators.CompareString(bh, "READY", false) != 0)
		{
			this.a.b("Verifying network registration status");
			if (!aj())
			{
				a(bn, bm, "GENERAL");
				return bf;
			}
		}
		string result;
		try
		{
			this.a.b("Reading own number");
			text = "AT+CNUM\r";
			text2 = a(text);
			if (this.z != 0 || text2.Length <= 0 || !text2.ToUpper().Contains("OK") || text2.ToUpper().Contains("ERROR"))
			{
				goto IL_020b;
			}
			text2 = text2.ToUpper().Replace("+CNUM:", "");
			text2 = a(text2, A_1: true, A_2: true, A_3: true).Replace("\"", "");
			if (!text2.Contains(","))
			{
				goto IL_020b;
			}
			string[] array = text2.Split(",".ToCharArray());
			if (array.GetUpperBound(0) < 1 || !Versioned.IsNumeric((object)array[1].Trim()) || array[1].Contains(",") || array[1].Contains("."))
			{
				goto IL_020b;
			}
			bf = array[1].Trim();
			if (array[2].Length >= 1)
			{
				if ((Operators.CompareString(array[2].Trim(), "145", false) == 0 || Operators.CompareString(array[2].Trim(), "129", false) != 0) && Operators.CompareString(text2.Substring(0, 1), "+", false) != 0)
				{
					bf = "+" + bf;
				}
			}
			else
			{
				bf = "+" + bf;
			}
			this.a.b("Own Number: " + bf);
			result = bf;
			goto end_IL_0069;
			IL_020b:
			result = bf;
			end_IL_0069:;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:GetOwnNumber:1] " + ex2.Message);
			bf = "";
			result = bf;
			ProjectData.ClearProjectError();
		}
		return result;
	}

	internal bool e(string A_0)
	{
		string text = "";
		string text2 = "";
		bool result = default(bool);
		try
		{
			if (!this.af)
			{
				this.b.a();
			}
			if (Operators.CompareString(this.ah, cl, false) == 0)
			{
				a(4022L, l("odumu5VOg_:5-26cCP/GtqPa6g$"), "VCALENDAR");
				result = false;
			}
			else
			{
				if (bl && Operators.CompareString(bh, "READY", false) == 0)
				{
					goto IL_00ab;
				}
				this.a.b("Verifying network registration status");
				if (aj())
				{
					goto IL_00ab;
				}
				a(bn, bm, "GENERAL");
				result = false;
			}
			goto end_IL_000c;
			IL_00ab:
			this.a.b("Sending USSD command to network");
			text = "AT+CUSD=" + A_0 + "\r";
			text2 = a(text).ToUpper();
			if (this.z == 0)
			{
				result = ((text2.Length > 0 && text2.ToUpper().Contains("OK") && !text2.ToUpper().Contains("ERROR")) ? true : false);
			}
			end_IL_000c:;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:SendUSSDMessage:1] " + ex2.Message);
			result = false;
			ProjectData.ClearProjectError();
		}
		return result;
	}

	internal int ar()
	{
		string text = "";
		string text2 = "";
		bo = 99;
		if (!bl || Operators.CompareString(bh, "READY", false) != 0)
		{
			this.a.b("Verifying network registration status");
			if (!aj())
			{
				a(bn, bm, "GENERAL");
				return bo;
			}
		}
		int result;
		try
		{
			this.a.b("Reading GSM signal strength in DB");
			text = "AT+CSQ\r";
			text2 = a(text);
			if (this.z != 0 || text2.Length <= 0 || !text2.ToUpper().Contains("OK") || text2.ToUpper().Contains("ERROR"))
			{
				goto IL_01b1;
			}
			text2 = text2.ToUpper().Replace("+CSQ:", "").Trim();
			text2 = a(text2, A_1: true, A_2: true, A_3: true);
			if (!text2.Contains(","))
			{
				goto IL_01b1;
			}
			string[] array = Strings.Split(text2, ",", -1, (CompareMethod)0);
			if (array.GetUpperBound(0) < 0 || !Versioned.IsNumeric((object)array[0].Trim()) || Operators.CompareString(array[0], "99", false) == 0 || array[0].Contains(",") || array[0].Contains("."))
			{
				goto IL_01b1;
			}
			bo = checked(Conversions.ToInteger(array[0].Trim()) * 2 - 113);
			bp = a(bo);
			this.a.b("Signal Strength: '" + Conversions.ToString(bo) + "dB'");
			result = bo;
			goto end_IL_0066;
			IL_01b1:
			result = bo;
			end_IL_0066:;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:GetSignalStrengthDB:1] " + ex2.Message);
			bo = 99;
			bp = -1;
			result = bo;
			ProjectData.ClearProjectError();
		}
		return result;
	}

	private int a(int A_0)
	{
		int result;
		try
		{
			result = ((A_0 == 99) ? (-1) : ((A_0 > -113) ? ((A_0 < -51) ? checked((int)Math.Round(Math.Round((double)(A_0 + 113) / 62.0 * 100.0, 1))) : 100) : 0));
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:dB2Percent:1] " + ex2.Message);
			result = -1;
			ProjectData.ClearProjectError();
		}
		return result;
	}

	internal string aq()
	{
		string text = "";
		string text2 = "";
		bq = "";
		bs = "00";
		br = "";
		if (!bl || Operators.CompareString(bh, "READY", false) != 0)
		{
			this.a.b("Verifying network registration status");
			if (!aj())
			{
				a(bn, bm, "GENERAL");
				return bq;
			}
		}
		string result;
		try
		{
			this.a.b("Reading SMSC number");
			text = "AT+CSCA?\r";
			text2 = a(text);
			if (this.z != 0 || text2.Length <= 0 || !text2.ToUpper().Contains("OK") || text2.ToUpper().Contains("ERROR"))
			{
				goto IL_0271;
			}
			text2 = text2.ToUpper().Replace("+CSCA:", "").Trim();
			text2 = a(text2, A_1: true, A_2: true, A_3: true).Replace("\"", "");
			if (text2.Length <= 0)
			{
				goto IL_0271;
			}
			string[] array = text2.Split(",".ToCharArray());
			if (array.GetUpperBound(0) < 0 || (!j(array[0].Trim()) && !f(array[0].Trim())))
			{
				goto IL_0271;
			}
			bq = array[0].Trim();
			if (array.GetUpperBound(0) >= 1)
			{
				if (array[1].Length >= 1)
				{
					br = "81";
					if (Operators.CompareString(array[1].Trim(), "145", false) == 0 || Operators.CompareString(array[1].Trim(), "129", false) != 0)
					{
						if (Operators.CompareString(bq.Substring(0, 1), "+", false) != 0)
						{
							bq = "+" + bq;
						}
						br = "91";
					}
				}
				else
				{
					bq = "+" + bq;
					br = "91";
				}
			}
			else
			{
				br = "81";
				if (j(bq))
				{
					br = "91";
				}
			}
			g();
			this.a.b("SMSC: " + bq);
			result = bq;
			goto end_IL_007f;
			IL_0271:
			result = bq;
			end_IL_007f:;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:GetSMSC:1] " + ex2.Message);
			bq = "";
			br = "";
			bs = "00";
			result = bq;
			ProjectData.ClearProjectError();
		}
		return result;
	}

	internal bool o(string A_0)
	{
		string text = "";
		string text2 = "";
		bs = "00";
		if (!bl || Operators.CompareString(bh, "READY", false) != 0)
		{
			this.a.b("Verifying network registration status");
			if (!aj())
			{
				a(bn, bm, "GENERAL");
				return false;
			}
		}
		bool result;
		try
		{
			this.a.b("Setting SMSC as '" + A_0 + "'");
			if (!j(A_0) && !f(A_0))
			{
				goto IL_016c;
			}
			text = "AT+CSCA=\"" + A_0 + "\"\r";
			text2 = a(text);
			if (this.z != 0 || text2.Length <= 0 || !text2.ToUpper().Contains("OK") || text2.ToUpper().Contains("ERROR"))
			{
				goto IL_016c;
			}
			text2 = a(text2.ToUpper(), A_1: false, A_2: true, A_3: true);
			if (string.Compare(text2.Trim(), "OK", ignoreCase: true) != 0)
			{
				goto IL_016c;
			}
			bq = A_0;
			if (j(A_0))
			{
				br = "91";
			}
			else if (f(A_0))
			{
				br = "81";
			}
			g();
			this.a.b("SMSC number set as '" + bq + "'");
			result = true;
			goto end_IL_0064;
			IL_016c:
			result = false;
			end_IL_0064:;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:SetSMSC:1] " + ex2.Message);
			bq = "";
			br = "";
			bs = "00";
			result = false;
			ProjectData.ClearProjectError();
		}
		return result;
	}

	private void g()
	{
		string text = "";
		bs = "00";
		checked
		{
			try
			{
				text = (j(bq) ? bq.Substring(1, bq.Length - 1) : ((!f(bq)) ? "" : bq));
				if (text.Length != 0 && br.Length != 0)
				{
					bs = br + b(text);
					bs = b((int)Math.Round((double)bs.Length / 2.0), 2) + bs;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				this.a.b("[GSM:CreateSMSCPDU:1] " + ex2.Message);
				bs = "00";
				ProjectData.ClearProjectError();
			}
		}
	}

	internal string at()
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		string text4 = "";
		long num = this.av;
		text3 = "";
		if (!bl || Operators.CompareString(bh, "READY", false) != 0)
		{
			this.a.b("Verifying network registration status");
			if (!aj())
			{
				a(bn, bm, "GENERAL");
				return text3;
			}
		}
		this.av = 60000L;
		checked
		{
			try
			{
				this.a.b("Reading network information");
				text = "AT+COPS?\r";
				text2 = a(text);
				if (this.z == 0 && text2.Length > 0 && text2.ToUpper().Contains("OK") && !text2.ToUpper().Contains("ERROR"))
				{
					text2 = text2.ToUpper().Replace("+COPS:", "").Replace("\"", "");
					text2 = a(text2, A_1: true, A_2: true, A_3: true).Trim().ToUpper();
					if (text2.Contains(","))
					{
						string[] array = text2.Split(",".ToCharArray());
						if (array.GetUpperBound(0) >= 2 && array[2].Trim().Length > 0)
						{
							text3 = array[2];
						}
					}
				}
				if (Operators.CompareString(text3.Trim(), "", false) == 0)
				{
					this.a.b("AT+COPS? command did not return network information");
					text = "AT+COPS=?\r";
					text2 = a(text);
					if (this.z == 0 && text2.Length > 0 && text2.ToUpper().Contains("OK") && !text2.ToUpper().Contains("ERROR"))
					{
						text2 = text2.ToUpper().Replace("+COPS:", "").Replace("\"", "");
						text2 = a(text2, A_1: true, A_2: true, A_3: true).Trim().ToUpper();
						if (text2.Contains("("))
						{
							string[] array2 = text2.Split("(".ToCharArray());
							int upperBound = array2.GetUpperBound(0);
							for (int num2 = 0; num2 <= upperBound; num2++)
							{
								if (array2[num2].Trim().Length <= 0)
								{
									continue;
								}
								array2[num2] = array2[num2].Replace("),,", "").Replace("),", "").Replace(")", "")
									.Trim();
								int num3 = 0;
								int length = array2[num2].Length;
								for (int num4 = 1; num4 <= length; num4++)
								{
									if (Operators.CompareString(Strings.Mid(array2[num2], num4, 1), ",", false) == 0)
									{
										num3++;
									}
								}
								if (num3 < 3)
								{
									continue;
								}
								if (Operators.CompareString(Strings.Mid(array2[num2], 1, 1), "2", false) == 0)
								{
									string[] array3 = array2[num2].Split(",".ToCharArray());
									if (array3[1].Length >= 1)
									{
										text3 = array3[1];
									}
									else if (array3[2].Length >= 1)
									{
										text3 = array3[2];
									}
									else if (array3[3].Length >= 1)
									{
										text3 = Strings.Mid(array3[3], 1, 3) + " " + Strings.Mid(array3[3], 4, 2);
									}
								}
								else if (Operators.CompareString(Strings.Mid(array2[num2], 1, 1), "1", false) == 0)
								{
									string[] array3 = array2[num2].Split(",".ToCharArray());
									if (array3[1].Length >= 1)
									{
										text4 = array3[1];
									}
									else if (array3[2].Length >= 1)
									{
										text4 = array3[2];
									}
									else if (array3[3].Length >= 1)
									{
										text4 = Strings.Mid(array3[3], 1, 3) + " " + Strings.Mid(array3[3], 4, 2);
									}
								}
							}
						}
					}
					if (text3.Trim().Length <= 0 && text4.Trim().Length > 0)
					{
						text3 = text4 + " [Available Network]";
					}
				}
				this.a.b("Network: " + text3);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				this.a.b("Error reading network information");
				text3 = "";
				ProjectData.ClearProjectError();
			}
			this.av = num;
			return text3;
		}
	}

	internal bool g(string A_0)
	{
		string text = "";
		long num = -1L;
		if (!bl || Operators.CompareString(bh, "READY", false) != 0)
		{
			this.a.b("Verifying network registration status");
			if (!aj())
			{
				a(bn, bm, "GENERAL");
				return false;
			}
		}
		checked
		{
			bool result;
			try
			{
				if (A_0.Length < 2)
				{
					goto IL_02b5;
				}
				text = A_0.Substring(A_0.Length - 1, 1);
				long num2 = 0L;
				if (Versioned.IsNumeric((object)A_0.Substring(0, A_0.Length - 1)) && !A_0.Contains(",") && !A_0.Contains("."))
				{
					num2 = Conversions.ToLong(A_0.Substring(0, A_0.Length - 1));
				}
				if (num2 <= 0 || (string.Compare(text, "M", ignoreCase: true) != 0 && string.Compare(text, "H", ignoreCase: true) != 0 && string.Compare(text, "D", ignoreCase: true) != 0 && string.Compare(text, "W", ignoreCase: true) != 0))
				{
					goto IL_02b5;
				}
				this.a.b("Setting validity period as " + num2 + text);
				switch (text.ToUpper())
				{
				case "M":
					if (num2 >= 5 && num2 <= 720)
					{
						if (unchecked(num2 % 5) != 0)
						{
							num2 = (long)Math.Round(Math.Round((double)num2 / 5.0, 0) * 5.0);
						}
						num = (long)Math.Round((double)num2 / 5.0 - 1.0);
					}
					break;
				case "H":
					if (num2 >= 1 && num2 <= 24)
					{
						num2 = Convert.ToInt64(Math.Round(new decimal(num2), 0));
						if (num2 < 12)
						{
							num2 *= 60;
							num = (long)Math.Round((double)num2 / 5.0 - 1.0);
						}
						else
						{
							num = (num2 - 12) * 2 + 143;
						}
					}
					break;
				case "D":
					if (num2 >= 1 && num2 <= 30)
					{
						num2 = Convert.ToInt64(Math.Round(new decimal(num2), 0));
						num = ((num2 != 1) ? (num2 + 166) : 167);
					}
					break;
				case "W":
					if (num2 >= 1 && num2 <= 63)
					{
						num2 = Convert.ToInt64(Math.Round(new decimal(num2), 0));
						if (num2 < 5)
						{
							num2 *= 7;
							num = num2 + 166;
						}
						else
						{
							num = num2 + 192;
						}
					}
					break;
				}
				if (num < 0 || num > 255)
				{
					goto IL_02b5;
				}
				bt = (int)num;
				result = true;
				goto end_IL_0056;
				IL_02b5:
				result = false;
				end_IL_0056:;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				this.a.b("[GSM:SetValidityPeriod:1] " + ex2.Message);
				bt = 167;
				result = false;
				ProjectData.ClearProjectError();
			}
			return result;
		}
	}

	internal string al()
	{
		string text = "";
		if (!bl || Operators.CompareString(bh, "READY", false) != 0)
		{
			this.a.b("Verifying network registration status");
			if (!aj())
			{
				a(bn, bm, "GENERAL");
				return "";
			}
		}
		string result;
		try
		{
			text = ((bt < 0 || bt > 255) ? "1D" : checked((bt >= 0 && bt <= 143) ? ((bt + 1) * 5 + "M") : ((bt >= 144 && bt <= 167) ? (12.0 + (double)(bt - 143) * 0.5 + "H") : ((bt < 168 || bt > 196) ? (bt - 192 + "W") : (bt - 166 + "D")))));
			result = text;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:GetValidityPeriod:1] " + ex2.Message);
			bt = 167;
			result = "1D";
			ProjectData.ClearProjectError();
		}
		return result;
	}

	internal bool av()
	{
		string text = "";
		string text2 = "";
		bool result;
		try
		{
			this.a.b("Setting PDU mode in GSM modem");
			text = "AT+CMGF=0\r";
			text2 = a(text);
			if (this.z != 0 || text2.Length <= 0 || !text2.ToUpper().Contains("OK") || text2.ToUpper().Contains("ERROR"))
			{
				goto IL_00e6;
			}
			if (text2.ToUpper().Contains("+CMGF:"))
			{
				text2 = text2.ToUpper().Replace("+CMGF:", "").Replace("0", "")
					.Replace("1", "")
					.Trim();
			}
			text2 = a(text2, A_1: false, A_2: true, A_3: true).Trim();
			if (string.Compare(text2, "OK", ignoreCase: true) != 0)
			{
				goto IL_00e6;
			}
			bw = true;
			bv = true;
			result = true;
			goto end_IL_000c;
			IL_00e6:
			result = false;
			end_IL_000c:;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:CheckPDUSupport:1] " + ex2.Message);
			bw = false;
			bv = false;
			result = false;
			ProjectData.ClearProjectError();
		}
		return result;
	}

	internal bool ad()
	{
		string text = "";
		string text2 = "";
		bh = "";
		bi = "";
		bj = 0L;
		bool result;
		try
		{
			this.a.b("Checking PIN status");
			i();
			text = "AT+CPIN?\r";
			text2 = a(text);
			if (this.z != 0)
			{
				bj = 2022L;
				bi = "Communication error while checking PIN status";
				goto IL_023e;
			}
			if (text2.ToUpper().Contains("ERROR"))
			{
				text2 = a(text2, A_1: true, A_2: true, A_3: true).Trim().ToUpper().Replace("+CME ERROR:", "");
				string text3 = text2.ToUpper();
				if (Operators.CompareString(text3, "13", false) == 0)
				{
					bj = 2017L;
					bi = "SIM card is damaged";
				}
				else if (Operators.CompareString(text3, "10", false) == 0)
				{
					bj = 2018L;
					bi = "No SIM card is inserted";
				}
				else if (Operators.CompareString(text3, "16", false) == 0)
				{
					bj = 2019L;
					bi = "Invalid or wrong PIN";
				}
				else if (Operators.CompareString(text3, "10", false) == 0)
				{
					bj = 2020L;
					bi = "Invalid or wrong PUK";
				}
				if (bj == 0)
				{
					bj = 2021L;
					bi = "Invalid PIN status response";
				}
				bh = "ERROR";
				goto IL_023e;
			}
			text2 = text2.ToUpper().Replace("+CPIN:", "");
			text2 = a(text2, A_1: true, A_2: true, A_3: true).Trim();
			if (text2.ToUpper().Contains("READY"))
			{
				bh = "READY";
				result = true;
			}
			else
			{
				bh = text2.ToUpper();
				if (string.Compare(bh, "SIM PIN", ignoreCase: true) != 0)
				{
					if (string.Compare(bh, "SIM PUK", ignoreCase: true) == 0)
					{
						bj = 2015L;
						bi = "Modem is waiting SIM PUK to be given";
					}
					else
					{
						bj = 2016L;
						bi = "Unknown error while checking PIN status";
					}
					goto IL_023e;
				}
				result = true;
			}
			goto end_IL_002a;
			IL_023e:
			result = false;
			end_IL_002a:;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			bj = 2023L;
			bi = "Error checking PIN Status. " + ex2.Message;
			bh = "";
			result = false;
			ProjectData.ClearProjectError();
		}
		return result;
	}

	internal bool ah()
	{
		if (bk)
		{
			this.a.b("Bypassing PIN status check because disabled");
			bh = "READY";
			return true;
		}
		string text = "";
		string text2 = "";
		bi = "";
		bj = 0L;
		if (!ad())
		{
			return false;
		}
		if (bj != 0)
		{
			return false;
		}
		if (string.Compare(bh, "READY", ignoreCase: true) == 0)
		{
			return true;
		}
		bool result;
		try
		{
			if (string.Compare(bh, "SIM PIN", ignoreCase: true) != 0)
			{
				goto IL_0218;
			}
			if (bg.Length <= 0)
			{
				bj = 2013L;
				bi = "Blank PIN is not allowed (Modem is waiting for PIN)";
				goto IL_0218;
			}
			this.a.b("Sending PIN [" + bg + "]");
			text = "AT+CPIN=\"" + bg + "\"\r";
			text2 = a(text);
			DateTime now = DateAndTime.Now;
			if (this.z != 0)
			{
				bj = 2012L;
				bi = "Communication error while sending PIN";
				goto IL_0218;
			}
			text2 = a(text2, A_1: false, A_2: true, A_3: true).Trim().ToUpper();
			if (text2.Contains("ERROR"))
			{
				bj = 2009L;
				bi = "Invalid PIN";
				result = false;
			}
			else
			{
				if (!text2.Contains("OK"))
				{
					bj = 2011L;
					bi = "Unknown error while sending PIN";
					goto IL_0218;
				}
				while (Operators.CompareString(bh, "READY", false) != 0 && (double)DateAndTime.DateDiff((DateInterval)9, now, DateTime.Now, (FirstDayOfWeek)1, (FirstWeekOfYear)1) < (double)this.av / 1000.0)
				{
					Thread.Sleep(100);
					ad();
				}
				if (Operators.CompareString(bh, "READY", false) != 0)
				{
					bj = 2010L;
					bi = "Unknown error after sending PIN";
					goto IL_0218;
				}
				this.a.b("Modem unlocked with PIN");
				Thread.Sleep(az);
				result = true;
			}
			goto end_IL_0070;
			IL_0218:
			result = false;
			end_IL_0070:;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			bj = 2014L;
			bi = "Error sending PIN. " + ex2.Message;
			bh = "";
			result = false;
			ProjectData.ClearProjectError();
		}
		return result;
	}

	internal bool z()
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		bl = false;
		bn = 0L;
		bm = "";
		if (Operators.CompareString(bh, "READY", false) != 0 && !bk && !ah())
		{
			return false;
		}
		DateTime now = DateAndTime.Now;
		if (bj != 0)
		{
			return false;
		}
		if (Operators.CompareString(bh, "READY", false) != 0)
		{
			return false;
		}
		bool result;
		try
		{
			bool flag = false;
			this.a.b("Checking network registration status");
			text = "AT+CREG?\r";
			string[] array = default(string[]);
			while (!flag && DateAndTime.DateDiff((DateInterval)9, now, DateTime.Now, (FirstDayOfWeek)1, (FirstWeekOfYear)1) < 60)
			{
				text2 = a(text);
				text3 = text2;
				if (this.z > 0)
				{
					break;
				}
				if (text2.Length > 0 && !text2.ToUpper().Contains("ERROR"))
				{
					text2 = a(text2, A_1: true, A_2: true, A_3: true).Trim().ToUpper().Replace("+CREG:", "")
						.Trim();
					if (text2.Contains(","))
					{
						array = text2.Split(",".ToCharArray());
						if (array.GetUpperBound(0) >= 1 && array[1].Trim().Length >= 1)
						{
							switch (Strings.Mid(array[1].Trim(), array[1].Trim().Length, 1).Trim())
							{
							case "1":
							case "5":
								flag = true;
								goto end_IL_0282;
							case "2":
								this.a.b("Modem is searching for a new operator to register to");
								Thread.Sleep(1000);
								this.a.b("Checking network registration status again after 1000ms delay");
								break;
							case "3":
								this.a.b("Network registration denied to the modem");
								Thread.Sleep(1000);
								this.a.b("Checking network registration status again after 1000ms delay");
								break;
							case "0":
								goto end_IL_0282;
							}
						}
					}
					else
					{
						text2 = " ,-1";
						array = text2.Split(",".ToCharArray());
					}
				}
				else
				{
					text2 = " ,-1";
					array = text2.Split(",".ToCharArray());
				}
				if (Operators.CompareString(text2, " ,-1", false) == 0)
				{
					this.a.b("Disconnecting modem as unknown network registration status detected.");
					ak();
					Thread.Sleep(1000);
				}
				continue;
				end_IL_0282:
				break;
			}
			if (!flag)
			{
				if (this.z == 0)
				{
					if (array.GetUpperBound(0) >= 1)
					{
						if (array[1].Trim().Length >= 1)
						{
							switch (Strings.Mid(array[1].Trim(), array[1].Trim().Length, 1).Trim())
							{
							case "0":
							case "2":
							case "4":
								bn = 3004L;
								bm = "Network registration failed";
								break;
							case "3":
								bn = 3005L;
								bm = "Network registration denied";
								break;
							default:
								bn = 3006L;
								bm = "Unknown network registration status";
								break;
							}
							if (text3.Trim().Length > 0)
							{
								bm = bm + " [Modem Response: " + text3.Trim().Replace("\r", "<cr>").Replace("\n", "<lf>") + "]";
							}
						}
					}
					else
					{
						bn = 3007L;
						bm = "Unknown network registration status";
						if (text3.Trim().Length > 0)
						{
							bm = bm + " [Modem Response: " + text3.Trim().Replace("\r", "<cr>").Replace("\n", "<lf>") + "]";
						}
					}
				}
				else
				{
					bn = this.z;
					bm = this.y;
				}
				result = false;
			}
			else
			{
				bl = true;
				result = true;
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			bn = 3008L;
			bm = "Error checking network registration status. [" + ex2.Message + "]";
			bl = false;
			result = false;
			ProjectData.ClearProjectError();
		}
		return result;
	}

	internal bool aj()
	{
		bn = 0L;
		bm = "";
		bool result;
		try
		{
			if (z())
			{
				result = true;
			}
			else
			{
				if (bj != 0)
				{
					bm = bi;
					bn = bj;
				}
				else if (bn == 0)
				{
					bm = "Unknown network registration status";
					bn = 3002L;
				}
				result = false;
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			bm = "Error checking network registration status. " + ex2.Message;
			bn = 3003L;
			result = false;
			ProjectData.ClearProjectError();
		}
		return result;
	}

	internal bool a(int A_0, string A_1)
	{
		string text = "";
		string text2 = "";
		bool flag = false;
		b7 = "";
		b8 = 0L;
		bool result = false;
		this.a.b("Deleting message " + A_0 + " [Memory Loc: " + A_1 + "]");
		if (!a7)
		{
			i(a8);
		}
		if (!bl || Operators.CompareString(bh, "READY", false) != 0)
		{
			this.a.b("Verifying network registration status");
			if (!aj())
			{
				if (this.z == 0)
				{
					b7 = bm;
					b8 = bn;
				}
				return false;
			}
		}
		if (!bv)
		{
			this.a.b("Verifying PDU support in modem");
			if (!av())
			{
				if (this.z == 0)
				{
					b7 = "Cannot check PDU mode support";
					b8 = 6006L;
				}
				return false;
			}
		}
		if (!bw)
		{
			if (this.z == 0)
			{
				b7 = "Modem does not support PDU mode";
				b8 = 6007L;
			}
			return false;
		}
		string[] array = Strings.Split(A_1, ",", -1, (CompareMethod)0);
		int num = Information.UBound((Array)array, 1);
		int num2 = 0;
		checked
		{
			bool flag3 = default(bool);
			while (true)
			{
				bool flag2;
				DateTime now;
				if (num2 <= num)
				{
					if (!s().IsOpen)
					{
						v();
					}
					if (!s().IsOpen || this.z != 0)
					{
						b7 = "Cannot connect to modem for deleting message";
						b8 = 6008L;
						return false;
					}
					text2 = "";
					text = "AT+CMGD=" + array[num2].ToString() + "\r";
					flag2 = false;
					try
					{
						c4 = true;
						if (Monitor.TryEnter(this.m_l, a4))
						{
							try
							{
								this.m_k.a = "";
							}
							catch (Exception ex)
							{
								ProjectData.SetProjectError(ex);
								Exception ex2 = ex;
								ProjectData.ClearProjectError();
							}
							finally
							{
								Monitor.Exit(this.m_l);
							}
							s().Write(text);
							now = DateTime.Now;
							this.a.b("Modem Command: " + text);
							goto IL_0372;
						}
						c4 = false;
						b7 = "Thread monitor enter attempt timeout [GSM:DeleteMessage:A]";
						b8 = 9011L;
						ak();
						result = false;
					}
					catch (InvalidOperationException ex3)
					{
						ProjectData.SetProjectError((Exception)ex3);
						InvalidOperationException ex4 = ex3;
						c4 = false;
						b7 = "Delete message command error (Port not open). " + ex4.Message;
						b8 = 6009L;
						ak();
						result = false;
						ProjectData.ClearProjectError();
					}
					catch (TimeoutException ex5)
					{
						ProjectData.SetProjectError((Exception)ex5);
						TimeoutException ex6 = ex5;
						c4 = false;
						b7 = "Delete message command error (Timeout). " + ex6.Message;
						b8 = 6010L;
						ak();
						result = false;
						ProjectData.ClearProjectError();
					}
					break;
				}
				if (flag)
				{
					c0.Remove(A_0.ToString());
				}
				if (flag3)
				{
					result = true;
				}
				c4 = false;
				break;
				IL_0372:
				while (!flag2 && (double)DateAndTime.DateDiff((DateInterval)9, now, DateTime.Now, (FirstDayOfWeek)1, (FirstWeekOfYear)1) < (double)this.av / 1000.0)
				{
					Thread.Sleep((int)Math.Round((double)a3 / 2.0));
					text2 = p();
					if (text2.Length > 0 && text2.ToUpper().Contains("OK") && !text2.ToUpper().Contains("ERROR"))
					{
						flag2 = true;
						break;
					}
					if (text2.ToUpper().Contains("ERROR"))
					{
						break;
					}
					Application.DoEvents();
				}
				c4 = false;
				if (!flag2)
				{
					this.a.b("Modem Response: " + text2);
					b7 = "Invalid response to delete message command";
					b8 = 6011L;
					if (text2.Trim().Length <= 0)
					{
						ak();
					}
					return false;
				}
				flag = true;
				if (num2 == array.GetUpperBound(0))
				{
					flag3 = true;
				}
				cz.Remove(array[num2]);
				num2++;
			}
			return result;
		}
	}

	internal bool a(int A_0, bool A_1)
	{
		string text = "";
		string text2 = "";
		long num = 0L;
		char[] trimChars = "\r\n".ToCharArray();
		b5 = "";
		b6 = 0L;
		bool flag = false;
		this.a.b("Reading incoming messages [Option-" + A_0 + "]");
		this.c.a();
		if (!s().IsOpen)
		{
			if (!v())
			{
				return false;
			}
			if (this.z != 0)
			{
				return false;
			}
		}
		if (!a7)
		{
			if (!i(a8))
			{
				return false;
			}
			if (this.z != 0)
			{
				return false;
			}
		}
		if (!bl || Operators.CompareString(bh, "READY", false) != 0)
		{
			this.a.b("Verifying network registration status");
			if (!aj())
			{
				b5 = bm;
				b6 = bn;
				return false;
			}
		}
		if (!bv && !av())
		{
			b5 = "Cannot check PDU mode support";
			b6 = 5004L;
			return false;
		}
		if (!bw)
		{
			b5 = "Modem does not support PDU mode";
			b6 = 5005L;
			return false;
		}
		checked
		{
			try
			{
				int num2 = default(int);
				int num3 = default(int);
				switch (A_0)
				{
				case 0:
					num2 = 0;
					num3 = 0;
					break;
				case 1:
					num2 = 1;
					num3 = 1;
					break;
				case 2:
					num2 = 0;
					num3 = 1;
					break;
				}
				int num4 = num3;
				int num5 = num2;
				int num6 = num4;
				while (true)
				{
					if (num6 < num5)
					{
						this.a.b("Processing incoming messages [" + cz.Count + " Msgs]");
						ap();
						flag = true;
						break;
					}
					if (!s().IsOpen)
					{
						v();
					}
					bool flag2;
					DateTime now;
					if (!s().IsOpen || this.z != 0)
					{
						b5 = "Cannot connect to modem for reading message";
						b6 = 5006L;
						flag = false;
					}
					else
					{
						flag2 = false;
						text = "AT+CMGL=" + num6 + "\r";
						text2 = "";
						try
						{
							c4 = true;
							c7 = true;
							if (Monitor.TryEnter(this.m_l, a4))
							{
								try
								{
									this.m_k.a = "";
								}
								catch (Exception ex)
								{
									ProjectData.SetProjectError(ex);
									Exception ex2 = ex;
									ProjectData.ClearProjectError();
								}
								finally
								{
									Monitor.Exit(this.m_l);
								}
								s().Write(text);
								now = DateTime.Now;
								this.a.b("Modem Command: " + text);
								goto IL_03da;
							}
							c4 = false;
							c7 = false;
							b3 = "Thread monitor enter attempt timeout [GSM:ReadInbox:A]";
							b4 = 9011L;
							ak();
							flag = false;
						}
						catch (InvalidOperationException ex3)
						{
							ProjectData.SetProjectError((Exception)ex3);
							InvalidOperationException ex4 = ex3;
							c4 = false;
							c7 = false;
							b3 = "Read message command error (Port not open). " + ex4.Message;
							b4 = 5007L;
							ak();
							flag = false;
							ProjectData.ClearProjectError();
						}
						catch (TimeoutException ex5)
						{
							ProjectData.SetProjectError((Exception)ex5);
							TimeoutException ex6 = ex5;
							c4 = false;
							c7 = false;
							b3 = "Read message command error (Timeout). " + ex6.Message;
							b4 = 5008L;
							ak();
							flag = false;
							ProjectData.ClearProjectError();
						}
					}
					goto end_IL_013e;
					IL_03da:
					while (!flag2 && (double)DateAndTime.DateDiff((DateInterval)9, now, DateAndTime.Now, (FirstDayOfWeek)1, (FirstWeekOfYear)1) < (double)this.av / 1000.0)
					{
						Thread.Sleep((int)Math.Round((double)a3 / 2.0));
						text2 += p();
						if (text2.Length > 0 && string.Compare(text2, text, ignoreCase: true) != 0 && ((text2.ToUpper().Contains("+CMS ERROR") && text2.ToUpper().Contains("321")) || (text2.ToUpper().Contains("OK") && !text2.ToUpper().Contains("ERROR"))))
						{
							flag2 = true;
							break;
						}
						if (text2.ToUpper().Contains("ERROR"))
						{
							break;
						}
						Application.DoEvents();
					}
					c4 = false;
					c7 = false;
					this.a.b("Recv: " + text2);
					if (flag2)
					{
						text2 = Strings.Replace(text2, text, "", 1, -1, (CompareMethod)0);
						text2 = a(text2, A_1: true, A_2: false, A_3: false);
						if (text2.Length > 0)
						{
							text2 = text2.Trim(trimChars);
						}
						if (text2.Length > 0 && text2.ToUpper().Contains("+CMGL:"))
						{
							string[] array = Strings.Split(text2.ToUpper(), "+CMGL:", -1, (CompareMethod)0);
							int upperBound = array.GetUpperBound(0);
							for (int num7 = 0; num7 <= upperBound; num7++)
							{
								if (array[num7].Trim().Length > 0)
								{
									a(array[num7].Trim(), A_1);
								}
								Application.DoEvents();
							}
						}
						num6 += -1;
						continue;
					}
					b5 = "Invalid or error response to read message command";
					b6 = 5009L;
					if (text2.Trim().Length <= 0)
					{
						ak();
					}
					flag = false;
					goto end_IL_013e;
				}
				goto IL_057d;
				end_IL_013e:;
			}
			catch (Exception ex7)
			{
				ProjectData.SetProjectError(ex7);
				Exception ex8 = ex7;
				this.a.b("[GSM:ReadInbox:1] " + ex8.Message);
				flag = false;
				ProjectData.ClearProjectError();
				goto IL_057d;
			}
			goto IL_058b;
		}
		IL_057d:
		c4 = false;
		c7 = false;
		goto IL_058b;
		IL_058b:
		return flag;
	}

	internal string a(string A_0, string A_1, bool A_2, int A_3, int A_4, bool A_5, int A_6)
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		string text = "";
		string text2 = "";
		string text3 = "0";
		string text4 = "";
		string text5 = "";
		string text6 = "";
		string text7 = "\u001b";
		string text8 = "";
		string result = "";
		text6 = "";
		b3 = "";
		b4 = 0L;
		int num = 160;
		int num2 = A_3;
		int num3 = A_4;
		int a_ = cv;
		Collection val = new Collection();
		text8 = l("O\u0017k.\u0017T0FNjD~{p\r=+6`%\\-P\u00059@GC");
		int length = text8.Length;
		checked
		{
			try
			{
				this.a.b("Sending message to " + A_0 + " [Option: " + num3 + "; Encoding: " + num2 + " bits]");
				switch (num2)
				{
				case 7:
					num = 160;
					break;
				case 8:
					num = 140;
					break;
				case 16:
					num = 70;
					break;
				}
				int num4 = num;
				if (num2 == 7)
				{
					A_1 = c(A_1);
				}
				if (!this.af)
				{
					this.b.a();
				}
				if (num3 == 0)
				{
					if (Operators.CompareString(this.ah, ck, false) == 0)
					{
						num -= length;
					}
					else
					{
						text8 = "";
					}
					if (A_1.Length > num)
					{
						A_1 = A_1.Substring(0, num);
					}
					if (num2 == 7 && A_1.Length > 0)
					{
						A_1 = A_1.TrimEnd(text7.ToCharArray());
					}
				}
				if (num3 == 1)
				{
					if (Operators.CompareString(this.ah, ck, false) == 0)
					{
						num -= length;
					}
					else
					{
						text8 = "";
					}
					if (A_1.Length > unchecked((long)num) * 255L)
					{
						A_1 = A_1.Substring(0, num * 255);
					}
					if (num2 == 7 && A_1.Length > 0)
					{
						A_1 = A_1.TrimEnd(text7.ToCharArray());
					}
				}
				if (num3 == 2)
				{
					num -= 12;
					if (Operators.CompareString(this.ah, ck, false) == 0)
					{
						num -= length;
					}
					else
					{
						text8 = "";
					}
					if (A_1.Length > num * 9)
					{
						A_1 = A_1.Substring(0, num * 9);
					}
					if (num2 == 7 && A_1.Length > 0)
					{
						A_1 = A_1.TrimEnd(text7.ToCharArray());
					}
				}
				if (num3 == 3)
				{
					switch (num2)
					{
					case 7:
						num = 152;
						break;
					case 8:
						num = 134;
						break;
					case 16:
						num = 67;
						break;
					}
					if (Operators.CompareString(this.ah, ck, false) == 0)
					{
						if (A_1.Length > num * 255 - length)
						{
							A_1 = A_1.Substring(0, num * 255 - length);
						}
					}
					else
					{
						if (A_1.Length > num * 255)
						{
							A_1 = A_1.Substring(0, num * 255);
						}
						text8 = "";
					}
					A_1 += text8;
					if (num2 == 7 && A_1.Length > 0)
					{
						A_1 = A_1.TrimEnd(text7.ToCharArray());
					}
				}
				if (bl && Operators.CompareString(bh, "READY", false) == 0)
				{
					goto IL_03b4;
				}
				this.a.b("Verifying network registration status");
				if (aj())
				{
					goto IL_03b4;
				}
				b3 = bm;
				b4 = bn;
				goto end_IL_008c;
				IL_03b4:
				if (!bv && !av())
				{
					b3 = "Cannot check PDU mode support";
					b4 = 4008L;
				}
				else if (!bw)
				{
					b3 = "Modem does not support PDU mode";
					b4 = 4009L;
				}
				else
				{
					if (A_1.Length <= num4)
					{
						num = num4;
					}
					bool flag = false;
					int num5 = 0;
					int num6 = 1;
					while (!flag)
					{
						text5 = Strings.Mid(A_1, num5 + 1, num);
						if (num2 == 7 && text5.Length > 0)
						{
							text5 = text5.TrimEnd(text7.ToCharArray());
						}
						val.Add((object)text5, num6.ToString(), (object)null, (object)null);
						num5 += text5.Length;
						num6++;
						if (num5 >= A_1.Length)
						{
							flag = true;
						}
					}
					int count = val.Count;
					if (count > 0)
					{
						int a_2 = ((num3 != 3) ? 1 : count);
						num5 = 1;
						int num7 = count;
						num5 = 1;
						while (true)
						{
							if (num5 > num7)
							{
								val = null;
								if (b4 == 0)
								{
									this.a.b("Message sent [Msg ID: " + text6 + "]");
									result = text6;
								}
								else
								{
									this.a.b("Message sending failed");
									result = "";
								}
								break;
							}
							aw();
							int num8 = num5;
							int num9 = 0;
							text5 = val[num5.ToString()].ToString();
							switch (num3)
							{
							case 0:
							case 1:
								text5 += text8;
								break;
							case 2:
								text5 = ((count <= 1) ? (text5 + text8) : ((num8 >= count) ? (text5 + "\r-Msg " + num8 + "/" + Conversions.ToString(count) + "-" + text8) : (text5 + "..\r-Msg " + num8 + "/" + Conversions.ToString(count) + "-" + text8)));
								break;
							}
							if ((num8 > 0 && num3 < 3) || (num8 == 1 && num3 == 3))
							{
								cv++;
								if (cv > 255)
								{
									cv = 1;
								}
							}
							text4 = a(A_0, text5, num2, 0, a_, num8, a_2, A_2, A_5, A_6);
							if (this.z <= 0)
							{
								long num10 = text4.Length;
								if (num10 == 0)
								{
									b3 = "Error generating message PDU";
									b4 = 4010L;
								}
								else
								{
									while (num9 <= b0)
									{
										aw();
										if (!s().IsOpen)
										{
											v();
										}
										bool flag2;
										DateTime now;
										if (!s().IsOpen || this.z != 0)
										{
											b3 = "Cannot connect to modem for sending message";
											b4 = 4012L;
										}
										else
										{
											if (num8 == 1 && num9 == 0)
											{
												while ((double)DateAndTime.DateDiff((DateInterval)9, cx, DateAndTime.Now, (FirstDayOfWeek)1, (FirstWeekOfYear)1) < (double)bz / 1000.0)
												{
													Thread.Sleep(10);
												}
											}
											else
											{
												Thread.Sleep(bz);
											}
											flag2 = false;
											text = "AT+CMGS=" + (double)(num10 - bs.Length) / 2.0 + "\r";
											try
											{
												c4 = true;
												c5 = true;
												if (Monitor.TryEnter(this.m_l, a4))
												{
													try
													{
														this.m_k.a = "";
													}
													catch (Exception ex)
													{
														ProjectData.SetProjectError(ex);
														Exception ex2 = ex;
														ProjectData.ClearProjectError();
													}
													finally
													{
														Monitor.Exit(this.m_l);
													}
													s().Write(text);
													now = DateTime.Now;
													this.a.b("Modem Command: " + text);
													goto IL_0820;
												}
												b3 = "Thread monitor enter attempt timeout [GSM:SubmitMessage:A]";
												b4 = 9011L;
												ak();
											}
											catch (InvalidOperationException ex3)
											{
												ProjectData.SetProjectError((Exception)ex3);
												InvalidOperationException ex4 = ex3;
												b3 = "Send message command error (Port not open). " + ex4.Message;
												b4 = 4013L;
												ak();
												ProjectData.ClearProjectError();
											}
											catch (TimeoutException ex5)
											{
												ProjectData.SetProjectError((Exception)ex5);
												TimeoutException ex6 = ex5;
												b3 = "Send message command error (Timeout). " + ex6.Message;
												b4 = 4014L;
												ak();
												ProjectData.ClearProjectError();
											}
										}
										goto IL_0c92;
										IL_0c92:
										c4 = false;
										c5 = false;
										c6 = false;
										num9++;
										if (b4 == 0)
										{
											b4 = 4019L;
											b3 = "Unknown error while sending message";
										}
										if (num9 <= b0)
										{
											a(b4, b3 + " [Send Attempt: " + num9 + "]", "SMSSEND");
											b4 = 0L;
											b3 = "";
										}
										if (c9 && s().IsOpen)
										{
											ak();
										}
										cx = DateAndTime.Now;
										continue;
										IL_0a39:
										bool flag3 = false;
										Thread.Sleep((int)Math.Round((double)a3 / 2.0));
										text2 = "";
										while (!flag3 && (double)DateAndTime.DateDiff((DateInterval)9, now, DateAndTime.Now, (FirstDayOfWeek)1, (FirstWeekOfYear)1) < (double)this.av / 1000.0)
										{
											text2 += p().ToUpper();
											if (text2.Length > 0 && string.Compare(text2, text4 + "\u001a", ignoreCase: true) != 0 && !text2.Contains("ERROR") && text2.Contains("+CMGS:"))
											{
												try
												{
													text3 = text2.Replace(text4 + "\u001a", "");
													if (text3.Contains("\r\nOK"))
													{
														text3 = Strings.Mid(text3, 1, Strings.InStr(text3, "\r\nOK", (CompareMethod)1) + 3);
													}
													text3 = a(text3.Replace("+CMGS:", ""), A_1: true, A_2: true, A_3: true).Trim();
													if (text3.Contains(","))
													{
														string[] array = text3.Split(",".ToCharArray());
														if (array.GetUpperBound(0) >= 1)
														{
															text3 = array[0];
														}
													}
													if (Versioned.IsNumeric((object)text3))
													{
														flag3 = true;
														break;
													}
												}
												catch (Exception ex7)
												{
													ProjectData.SetProjectError(ex7);
													Exception ex8 = ex7;
													ProjectData.ClearProjectError();
												}
											}
											if (text2.ToUpper().Contains("ERROR"))
											{
												break;
											}
											Application.DoEvents();
											Thread.Sleep((int)Math.Round((double)a3 / 2.0));
										}
										c6 = false;
										c4 = false;
										text2 = text2.Replace(text4 + "\u001a", "");
										this.a.b("Modem Response: " + text2);
										if (!flag3)
										{
											b3 = "Invalid response to submit PDU [Response: " + text2 + "]";
											b4 = 4018L;
											if (text2.Trim().Length <= 0 || text2.Trim().Contains("ERROR"))
											{
												ak();
											}
											goto IL_0c92;
										}
										text6 = ((text6.Length > 0) ? (text6 + "," + text3) : text3);
										cx = DateAndTime.Now;
										break;
										IL_0820:
										text2 = "";
										while (!flag2 && (double)DateAndTime.DateDiff((DateInterval)9, now, DateTime.Now, (FirstDayOfWeek)1, (FirstWeekOfYear)1) < (double)this.av / 1000.0)
										{
											Thread.Sleep((int)Math.Round((double)a3 / 2.0));
											text2 += p();
											if (text2.Length > 0 && text2.ToUpper().Contains(">") && !text2.ToUpper().Contains("ERROR"))
											{
												flag2 = true;
												break;
											}
											if (text2.ToUpper().Contains("ERROR"))
											{
												break;
											}
											Application.DoEvents();
										}
										c5 = false;
										c4 = false;
										if (!flag2)
										{
											this.a.b("Modem Response: " + text2);
											b3 = "Invalid or error response to send message command";
											b4 = 4015L;
											ak();
										}
										else
										{
											try
											{
												c4 = true;
												c6 = true;
												if (Monitor.TryEnter(this.m_l, a4))
												{
													try
													{
														this.m_k.a = "";
													}
													catch (Exception ex9)
													{
														ProjectData.SetProjectError(ex9);
														Exception ex10 = ex9;
														ProjectData.ClearProjectError();
													}
													finally
													{
														Monitor.Exit(this.m_l);
													}
													s().Write(text4 + "\u001a");
													now = DateAndTime.Now;
													this.a.b("PDU: " + text4);
													goto IL_0a39;
												}
												b3 = "Thread monitor enter attempt timeout [GSM:SubmitMessage:B]";
												b4 = 9011L;
												ak();
											}
											catch (InvalidOperationException ex11)
											{
												ProjectData.SetProjectError((Exception)ex11);
												InvalidOperationException ex12 = ex11;
												b3 = "Submit PDU error (Port not open). " + ex12.Message;
												b4 = 4016L;
												ak();
												ProjectData.ClearProjectError();
											}
											catch (TimeoutException ex13)
											{
												ProjectData.SetProjectError((Exception)ex13);
												TimeoutException ex14 = ex13;
												b3 = "Submit PDU error (Timeout). " + ex14.Message;
												b4 = 4017L;
												ak();
												ProjectData.ClearProjectError();
											}
										}
										goto IL_0c92;
									}
									if (num9 <= b0)
									{
										num5++;
										continue;
									}
								}
							}
							goto end_IL_008c;
						}
						goto IL_0e15;
					}
				}
				end_IL_008c:;
			}
			catch (Exception ex15)
			{
				ProjectData.SetProjectError(ex15);
				Exception ex16 = ex15;
				c4 = false;
				c5 = false;
				c6 = false;
				b4 = 4021L;
				b3 = "System error (" + ex16.Message + ")";
				this.a.b("Message sending failed");
				result = "";
				ProjectData.ClearProjectError();
				goto IL_0e15;
			}
			goto IL_0e2a;
		}
		IL_0e15:
		c4 = false;
		c5 = false;
		c6 = false;
		goto IL_0e2a;
		IL_0e2a:
		return result;
	}

	internal string a(string A_0, string A_1, int A_2, int A_3, int A_4, int A_5, int A_6, bool A_7, bool A_8, int A_9)
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		string text4 = "";
		string text5 = "";
		string text6 = "";
		string text7 = "";
		string text8 = "";
		string text9 = "";
		string text10 = "";
		string a_ = "";
		bool flag = A_8;
		string result = "";
		checked
		{
			try
			{
				if (bs.Length != 0)
				{
					goto IL_00b2;
				}
				aq();
				if (bq.Length > 0)
				{
					if (bs.Length == 0)
					{
						bs = "00";
					}
					goto IL_00b2;
				}
				a(4011L, "SMSC not specified", "SMSSEND");
				result = "";
				goto end_IL_0054;
				IL_00b2:
				text = bs;
				if (flag && A_6 > 1 && A_5 < A_6)
				{
					flag = false;
				}
				if (A_6 > 1)
				{
					switch (flag)
					{
					case true:
						text2 = "71";
						break;
					case false:
						text2 = "51";
						break;
					}
				}
				else
				{
					switch (flag)
					{
					case true:
						text2 = "31";
						break;
					case false:
						text2 = "11";
						break;
					}
				}
				text3 = "00";
				if (j(A_0))
				{
					A_0 = A_0.Substring(1, A_0.Length - 1);
					text5 = "91";
					goto IL_0157;
				}
				if (f(A_0))
				{
					text5 = "81";
					goto IL_0157;
				}
				result = "";
				goto end_IL_0054;
				IL_0157:
				text4 = b(A_0.Length, 2) + text5 + b(A_0);
				text6 = "00";
				if (A_7)
				{
					switch (A_2)
					{
					case 7:
						text10 = "10";
						break;
					case 8:
						text10 = "14";
						break;
					case 16:
						text10 = "18";
						break;
					}
				}
				else
				{
					switch (A_2)
					{
					case 7:
						text10 = "00";
						break;
					case 8:
						text10 = "04";
						break;
					case 16:
						text10 = "08";
						break;
					}
				}
				text9 = b(A_9, 2);
				if (A_6 > 1)
				{
					if (A_2 == 7)
					{
						a_ = "0";
					}
					switch (A_2)
					{
					case 7:
						text7 = b(A_1.Length + 7, 2);
						break;
					case 8:
						text7 = b(A_1.Length + 6, 2);
						break;
					case 16:
						text7 = b(A_1.Length * 2 + 6, 2);
						break;
					}
					text8 = "050003" + b(A_4, 2) + b(A_6, 2) + b(A_5, 2) + a(A_1, A_2, a_);
				}
				else
				{
					switch (A_2)
					{
					case 7:
					case 8:
						text7 = b(A_1.Length, 2);
						break;
					case 16:
						text7 = b(A_1.Length * 2, 2);
						break;
					}
					text8 = a(A_1, A_2, a_);
				}
				result = text + text2 + text3 + text4 + text6 + text10 + text9 + text7 + text8;
				end_IL_0054:;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				this.a.b("[GSM:SubmitPDU:1] " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			return result;
		}
	}

	internal string a(string A_0, int A_1, string A_2)
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		string result = "";
		checked
		{
			try
			{
				if (A_1 != 7 && A_1 != 8 && A_1 != 16)
				{
					result = "";
				}
				else
				{
					text = A_2;
					text2 = "";
					int length = A_0.Length;
					for (int num = 1; num <= length; num++)
					{
						text3 = A_0.Substring(num - 1, 1);
						text = a(Strings.AscW(text3), A_1) + text;
					}
					int num2 = default(int);
					switch (A_1)
					{
					case 7:
					case 8:
						num2 = 8;
						break;
					case 16:
						num2 = 16;
						break;
					}
					int num3 = unchecked(text.Length % num2);
					if (num3 > 0)
					{
						int num4 = num2 - num3;
						for (int num = 1; num <= num4; num++)
						{
							text = "0" + text;
						}
					}
					int num5 = unchecked(text.Length / num2);
					int num6 = num5;
					for (int num = 1; num <= num6; num++)
					{
						text2 += a(Strings.Mid(text, text.Length + 1 - num2 * num, num2), (int)Math.Round((double)num2 / 4.0));
					}
					result = text2;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				this.a.b("[GSM:TxtToPDU:1] " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			return result;
		}
	}

	internal bool a(string A_0, bool A_1)
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		string text4 = "";
		string text5 = "";
		string text6 = "";
		string text7 = "";
		string text8 = "";
		string text9 = "";
		string text10 = "";
		string text11 = "";
		string text12 = "";
		string text13 = "";
		string text14 = "";
		string text15 = "";
		string text16 = "";
		string text17 = "";
		string text18 = "";
		string text19 = "";
		string text20 = "";
		string text21 = "";
		string text22 = "";
		string text23 = "";
		string text24 = "";
		string text25 = "";
		string text26 = "";
		string text27 = "";
		string text28 = "";
		string text29 = "";
		string text30 = "";
		int num = 0;
		string text31 = "";
		int num2 = 0;
		string text32 = "";
		DateTime dateTime = default(DateTime);
		string text33 = "";
		string text34 = "";
		string text35 = "";
		string text36 = "";
		string text37 = "";
		string text38 = "";
		string text39 = "";
		string text40 = "";
		bool flag = false;
		long num3 = 0L;
		int num4 = 0;
		long num5 = 0L;
		int num6 = 0;
		bool result = false;
		checked
		{
			try
			{
				string[] array = Strings.Split(A_0, "\r\n", -1, (CompareMethod)0);
				string[] array2 = Strings.Split(array[0], ",", -1, (CompareMethod)0);
				int a_;
				int num9 = default(int);
				DateTime dateTime2;
				if (!Versioned.IsNumeric((object)array2[0]) || array2[0].Contains(".") || array2[0].Contains(","))
				{
					result = false;
				}
				else if (array[1].Length < 2)
				{
					result = false;
				}
				else
				{
					a_ = Conversions.ToInteger(array2[0]);
					text = array[1];
					int num7 = (int)(m(Strings.Left(text, 2)) * 2);
					text = Strings.Right(text, text.Length - 2);
					if (num7 > 0)
					{
						text2 = Strings.Left(text, 2);
						text = Strings.Right(text, text.Length - 2);
						text3 = b(Strings.Left(text, num7 - 2));
						if (string.Compare(Strings.Right(text3, 1), "F", ignoreCase: true) == 0)
						{
							text3 = Strings.Left(text3, text3.Length - 1);
						}
						if (string.Compare(text2, "91", ignoreCase: true) == 0)
						{
							text3 = "+" + text3;
						}
						text = Strings.Right(text, text.Length - num7 + 2);
					}
					text4 = Strings.Left(text, 2);
					text = Strings.Right(text, text.Length - 2);
					text5 = p(text4);
					text6 = Strings.Mid(text5, 2, 1);
					text7 = Strings.Mid(text5, 7, 2);
					if (Operators.CompareString(text7, "00", false) == 0)
					{
						int num8 = (int)m(Strings.Left(text, 2));
						if (unchecked(num8 % 2) != 0)
						{
							num8++;
						}
						int A_2 = num8;
						text = Strings.Right(text, text.Length - 2);
						text8 = Strings.Left(text, 2);
						text = Strings.Right(text, text.Length - 2);
						text9 = Strings.Left(text, num8);
						if (Operators.CompareString(Strings.Mid(text8, 1, 1), "D", false) == 0)
						{
							text9 = a(text9, 7, "0", ref A_2);
						}
						else
						{
							text9 = b(text9);
							if (string.Compare(Strings.Right(text9, 1), "F", ignoreCase: true) == 0)
							{
								text9 = Strings.Left(text9, text9.Length - 1);
							}
						}
						if (Operators.CompareString(text8, "91", false) == 0)
						{
							text9 = "+" + text9;
						}
						text = Strings.Right(text, text.Length - num8);
						text22 = Strings.Left(text, 2);
						text = Strings.Right(text, text.Length - 2);
						text23 = Strings.Left(text, 2);
						text = Strings.Right(text, text.Length - 2);
						text25 = p(text23);
						text26 = Strings.Mid(text25, 1, 2);
						text27 = Strings.Mid(text25, 3, 2);
						text28 = Strings.Mid(text25, 5, 2);
						if (Operators.CompareString(text26, "00", false) == 0 || Operators.CompareString(text26 + text27, "1111", false) == 0)
						{
							if (Operators.CompareString(text26, "00", false) != 0)
							{
								goto IL_04b3;
							}
							if (Operators.CompareString(Strings.Mid(text27, 1, 1), "1", false) != 0)
							{
								switch (text28)
								{
								case "00":
									num9 = 7;
									break;
								case "01":
									num9 = 8;
									break;
								case "10":
									num9 = 16;
									break;
								case "11":
									result = false;
									goto end_IL_0137;
								}
								goto IL_04b3;
							}
							result = false;
						}
					}
					else if (Operators.CompareString(text7, "10", false) == 0)
					{
						text31 = Strings.Mid(text5, 3, 1);
						if (Operators.CompareString(text31, "1", false) == 0)
						{
							throw new Exception();
						}
						num2 = (int)m(Strings.Left(text, 2));
						text = Strings.Right(text, text.Length - 2);
						int num8 = (int)m(Strings.Left(text, 2));
						if (unchecked(num8 % 2) != 0)
						{
							num8++;
						}
						text = Strings.Right(text, text.Length - 2);
						text8 = Strings.Left(text, 2);
						text = Strings.Right(text, text.Length - 2);
						text9 = Strings.Left(text, num8);
						if (Operators.CompareString(text8, "D0", false) == 0)
						{
							text9 = a(text9, 7, "0", ref num8);
						}
						else
						{
							text9 = b(text9);
							if (Operators.CompareString(Strings.UCase(Strings.Right(text9, 1)), "F", false) == 0)
							{
								text9 = Strings.Left(text9, text9.Length - 1);
							}
						}
						if (Operators.CompareString(text8, "91", false) == 0)
						{
							text9 = "+" + text9;
						}
						text = Strings.Right(text, text.Length - num8);
						text10 = b(Strings.Left(text, 14));
						text13 = Strings.Mid(text10, 1, 2);
						text12 = Strings.Mid(text10, 3, 2);
						text11 = Strings.Mid(text10, 5, 2);
						text14 = Strings.Mid(text10, 7, 2);
						text15 = Strings.Mid(text10, 9, 2);
						text16 = Strings.Mid(text10, 11, 2);
						text18 = Strings.Mid(text10, 13, 2);
						dateTime2 = DateAndTime.DateSerial(Conversions.ToInteger("20" + text13), Conversions.ToInteger(text12), Conversions.ToInteger(text11));
						dateTime2 = DateAndTime.DateAdd("s", (double)(Conversions.ToLong(text14) * 3600 + Conversions.ToLong(text15) * 60 + Conversions.ToLong(text16)), (object)dateTime2);
						if (Operators.CompareString(Strings.Mid(p(text18), 1, 1), "0", false) == 0)
						{
							try
							{
								num3 = Conversions.ToLong(text18) * 15;
							}
							catch (Exception ex)
							{
								ProjectData.SetProjectError(ex);
								Exception ex2 = ex;
								num3 = 0L;
								ProjectData.ClearProjectError();
							}
							num4 = (int)num3;
						}
						else
						{
							try
							{
								num3 = Conversions.ToLong(a("0" + Strings.Mid(p(text18), 2, 7), 2)) * 15;
							}
							catch (Exception ex3)
							{
								ProjectData.SetProjectError(ex3);
								Exception ex4 = ex3;
								num3 = 0L;
								ProjectData.ClearProjectError();
							}
							num4 = -(int)num3;
						}
						text = Strings.Right(text, text.Length - 14);
						text32 = b(Strings.Left(text, 14));
						text35 = Strings.Mid(text32, 1, 2);
						text34 = Strings.Mid(text32, 3, 2);
						text33 = Strings.Mid(text32, 5, 2);
						text36 = Strings.Mid(text32, 7, 2);
						text37 = Strings.Mid(text32, 9, 2);
						text38 = Strings.Mid(text32, 11, 2);
						text39 = Strings.Mid(text32, 13, 2);
						dateTime = DateAndTime.DateSerial(Conversions.ToInteger("20" + text35), Conversions.ToInteger(text34), Conversions.ToInteger(text33));
						dateTime = DateAndTime.DateAdd("s", (double)(Conversions.ToLong(text36) * 3600 + Conversions.ToLong(text37) * 60 + Conversions.ToLong(text38)), (object)dateTime);
						if (Operators.CompareString(Strings.Mid(p(text39), 1, 1), "0", false) == 0)
						{
							try
							{
								num5 = Conversions.ToLong(text39) * 15;
							}
							catch (Exception ex5)
							{
								ProjectData.SetProjectError(ex5);
								Exception ex6 = ex5;
								num5 = 0L;
								ProjectData.ClearProjectError();
							}
							num6 = (int)num5;
						}
						else
						{
							try
							{
								num5 = Conversions.ToLong(a("0" + Strings.Mid(p(text39), 2, 7), 2)) * 15;
							}
							catch (Exception ex7)
							{
								ProjectData.SetProjectError(ex7);
								Exception ex8 = ex7;
								num5 = 0L;
								ProjectData.ClearProjectError();
							}
							num6 = -(int)num5;
						}
						text = Strings.Right(text, text.Length - 14);
						text40 = Strings.Left(text, 2);
						long num10 = m(text40);
						if ((num10 >= 3 && num10 <= 63) || (num10 >= 96 && num10 <= 255))
						{
							result = false;
						}
						else
						{
							flag = ((num10 >= 0 && num10 <= 2) ? true : false);
							NewDeliveryReportEventArgs newDeliveryReportEventArgs = new NewDeliveryReportEventArgs(num2, text9, flag, dateTime, num6, dateTime2, num4);
							this.m_k.g.Add((object)newDeliveryReportEventArgs, a_.ToString(), (object)null, (object)null);
							text30 = a("AT+CMGD=" + a_ + "\r");
							ThreadPool.RegisterWaitForSingleObject(d6, d, null, -1, executeOnlyOnce: true);
							result = true;
						}
					}
					else
					{
						if (A_1)
						{
							text30 = a("AT+CMGD=" + a_ + "\r");
						}
						result = true;
					}
				}
				goto end_IL_0137;
				IL_051c:
				int a_2;
				int a_3;
				int a_4;
				int A_3 = default(int);
				if (num9 != 7 && num9 != 8 && num9 != 16)
				{
					result = false;
				}
				else
				{
					text10 = b(Strings.Left(text, 14));
					text13 = Strings.Mid(text10, 1, 2);
					text12 = Strings.Mid(text10, 3, 2);
					text11 = Strings.Mid(text10, 5, 2);
					text14 = Strings.Mid(text10, 7, 2);
					text15 = Strings.Mid(text10, 9, 2);
					text16 = Strings.Mid(text10, 11, 2);
					text18 = Strings.Mid(text10, 13, 2);
					dateTime2 = DateAndTime.DateSerial(Conversions.ToInteger("20" + text13), Conversions.ToInteger(text12), Conversions.ToInteger(text11));
					dateTime2 = DateAndTime.DateAdd((DateInterval)9, (double)(Conversions.ToLong(text14) * 3600 + Conversions.ToLong(text15) * 60 + Conversions.ToLong(text16)), dateTime2);
					long num11;
					if (Operators.CompareString(Strings.Mid(p(Strings.Mid(text10, 13, 2)), 1, 1), "0", false) == 0)
					{
						try
						{
							num11 = Conversions.ToLong(text18) * 15;
						}
						catch (Exception ex9)
						{
							ProjectData.SetProjectError(ex9);
							Exception ex10 = ex9;
							num11 = 0L;
							ProjectData.ClearProjectError();
						}
						text21 = "+";
						num = (int)num11;
					}
					else
					{
						try
						{
							num11 = Conversions.ToLong(a("0" + Strings.Mid(p(text18), 2, 7), 2)) * 15;
						}
						catch (Exception ex11)
						{
							ProjectData.SetProjectError(ex11);
							Exception ex12 = ex11;
							num11 = 0L;
							ProjectData.ClearProjectError();
						}
						text21 = "-";
						num = -(int)num11;
					}
					text19 = Conversion.Int((double)num11 / 60.0).ToString();
					text20 = unchecked(num11 % 60).ToString();
					if (text19.Length < 2)
					{
						text19 = "0" + text19;
					}
					if (text20.Length < 2)
					{
						text20 = "0" + text20;
					}
					text21 = ((Operators.CompareString(text19, "00", false) != 0 || Operators.CompareString(text20, "00", false) != 0) ? (text21 + text19 + ":" + text20) : "GMT");
					text17 = DateAndTime.WeekdayName(DateAndTime.Weekday(dateTime2, (FirstDayOfWeek)1), true, (FirstDayOfWeek)0) + ", " + text11 + " " + DateAndTime.MonthName(Conversions.ToInteger(text12), true) + " 20" + text13 + " " + text14 + ":" + text15 + ":" + text16 + " " + text21;
					text = Strings.Right(text, text.Length - 14);
					int num12 = (int)m(Strings.Left(text, 2));
					text = Strings.Right(text, text.Length - 2);
					a_2 = 1;
					a_3 = 1;
					a_4 = 0;
					if (Operators.CompareString(text6, "1", false) != 0)
					{
						A_3 = num12;
						goto IL_08f3;
					}
					text29 = Strings.Mid(text, 1, 12);
					if (Operators.CompareString(Strings.Mid(text29, 1, 6), "050003", false) == 0)
					{
						text = Strings.Mid(text, 13, text.Length - 12);
						a_4 = (int)m(Strings.Mid(text29, 7, 2));
						a_2 = (int)m(Strings.Mid(text29, 9, 2));
						a_3 = (int)m(Strings.Mid(text29, 11, 2));
						switch (num9)
						{
						case 7:
							A_3 = num12 - 7;
							break;
						case 8:
						case 16:
							A_3 = num12 - 6;
							break;
						}
						goto IL_08f3;
					}
					if (A_1)
					{
						text30 = a("AT+CMGD=" + a_ + "\r");
					}
				}
				goto end_IL_0137;
				IL_08f3:
				if (num9 == 16)
				{
					A_3 = (int)Math.Round((double)A_3 / 2.0);
				}
				text24 = a(text, num9, text6, ref A_3);
				if (text24.Length > A_3)
				{
					text24 = Strings.Left(text24, A_3);
				}
				this.c.a(a_, 0, text9, text24, dateTime2, text17, num, text3, a_4, a_2, a_3);
				result = true;
				goto end_IL_0137;
				IL_04b3:
				if (Operators.CompareString(text26, "11", false) != 0)
				{
					goto IL_051c;
				}
				if (Operators.CompareString(Strings.Mid(text28, 1, 1), "1", false) != 0)
				{
					string text41 = Strings.Mid(text28, 2, 1);
					if (Operators.CompareString(text41, "0", false) == 0)
					{
						num9 = 7;
					}
					else if (Operators.CompareString(text41, "1", false) == 0)
					{
						num9 = 8;
					}
					goto IL_051c;
				}
				result = false;
				end_IL_0137:;
			}
			catch (Exception ex13)
			{
				ProjectData.SetProjectError(ex13);
				Exception ex14 = ex13;
				this.a.b("[GSM:ReadPDU:1] " + ex14.Message);
				result = false;
				ProjectData.ClearProjectError();
			}
			return result;
		}
	}

	internal string a(string A_0, int A_1, string A_2, ref int A_3)
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		string text4 = "";
		string result = "";
		checked
		{
			try
			{
				int num = default(int);
				switch (A_1)
				{
				case 7:
				case 8:
					num = 2;
					break;
				case 16:
					num = 4;
					break;
				}
				int num2 = num * 4;
				int length = A_0.Length;
				int num3 = num;
				int num4 = length;
				for (int num5 = 1; ((num3 >> 31) ^ num5) <= ((num3 >> 31) ^ num4); num5 += num3)
				{
					text3 = Strings.Mid(A_0, num5, num);
					text = p(text3) + text;
				}
				if (A_1 == 7 && Operators.CompareString(A_2, "1", false) == 0)
				{
					text = Strings.Mid(text, 1, text.Length - 1);
				}
				int num7;
				unchecked
				{
					int num6 = text.Length / A_1;
					num7 = text.Length % A_1;
				}
				text = Strings.Mid(text, num7 + 1, text.Length - num7);
				int length2 = text.Length;
				for (int num5 = 1; ((A_1 >> 31) ^ num5) <= ((A_1 >> 31) ^ length2); num5 += A_1)
				{
					text2 = Conversions.ToString(Strings.ChrW((int)m(a(Strings.Mid(text, num5, A_1), num)))) + text2;
				}
				if (A_1 == 7)
				{
					text2 = a(text2, ref A_3);
				}
				result = text2;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				this.a.b("[GSM:PDUToTxt:1] " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			return result;
		}
	}

	private string a(int A_0, int A_1)
	{
		int num = 2;
		string text = "";
		string text2 = "";
		string text3 = "1";
		bool flag = false;
		string text4 = "";
		string text5;
		try
		{
			if (A_1 != 7 && A_1 != 8 && A_1 != 16)
			{
				text5 = "";
			}
			else
			{
				if (A_0 < 0)
				{
					flag = true;
					A_0 = checked(-A_0);
				}
				int num2;
				do
				{
					text = text2 + text;
					num2 = Conversion.Int(A_0 / num);
					text2 = (A_0 % num).ToString();
					A_0 = num2;
				}
				while (num2 > 1);
				text5 = num2 + text2 + text;
				if (flag)
				{
					while (text5.Length % 4 != 0)
					{
						text5 = "0" + text5;
					}
					text4 = text5;
					int length = text5.Length;
					checked
					{
						for (int num3 = 1; num3 <= length; num3++)
						{
							text4 = ((Operators.CompareString(Strings.Mid(text5, num3, 1), "0", false) != 0) ? (text4 + "0") : (text4 + "1"));
						}
						text5 = "";
						for (int num4 = text4.Length; num4 >= 1; num4 += -1)
						{
							if (Operators.CompareString(text3, "0", false) == 0)
							{
								text5 = Strings.Mid(text4, num4, 1) + text5;
							}
							else if (Operators.CompareString(Strings.Mid(text4, num4, 1), "0", false) == 0)
							{
								text5 = "1" + text5;
								text3 = "0";
							}
							else
							{
								text5 = "0" + text5;
							}
						}
					}
				}
				switch (A_1)
				{
				case 7:
					text5 = Strings.Right("0000000" + text5, 7);
					break;
				case 8:
					text5 = Strings.Right("00000000" + text5, 8);
					break;
				case 16:
					text5 = Strings.Right("0000000000000000" + text5, 16);
					break;
				case 9:
				case 10:
				case 11:
				case 12:
				case 13:
				case 14:
				case 15:
					break;
				}
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:Biner:1] " + ex2.Message);
			text5 = "";
			ProjectData.ClearProjectError();
		}
		return text5;
	}

	internal string a(string A_0, int A_1)
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		checked
		{
			string result;
			try
			{
				for (int num = 1; num <= A_1; num++)
				{
					text3 += "0000";
				}
				A_0 = Strings.Right(text3 + A_0, A_1 * 4);
				for (int num2 = 1; num2 <= A_1; num2++)
				{
					text2 = dy[Strings.Mid(A_0, A_0.Length + 1 - 4 * num2, 4)].ToString() + text2;
				}
				result = text2;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				this.a.b("[GSM:BinToHex:1] " + ex2.Message);
				result = "";
				ProjectData.ClearProjectError();
			}
			return result;
		}
	}

	private void f()
	{
		IDictionaryEnumerator enumerator = ds.GetEnumerator();
		DictionaryEntry dictionaryEntry = default(DictionaryEntry);
		while (enumerator.MoveNext())
		{
			object current = enumerator.Current;
			DictionaryEntry dictionaryEntry2 = ((current != null) ? ((DictionaryEntry)current) : dictionaryEntry);
			dy.Add(RuntimeHelpers.GetObjectValue(dictionaryEntry2.Value), RuntimeHelpers.GetObjectValue(dictionaryEntry2.Key));
		}
	}

	private void e()
	{
		ds.Add("0", "0000");
		ds.Add("1", "0001");
		ds.Add("2", "0010");
		ds.Add("3", "0011");
		ds.Add("4", "0100");
		ds.Add("5", "0101");
		ds.Add("6", "0110");
		ds.Add("7", "0111");
		ds.Add("8", "1000");
		ds.Add("9", "1001");
		ds.Add("A", "1010");
		ds.Add("B", "1011");
		ds.Add("C", "1100");
		ds.Add("D", "1101");
		ds.Add("E", "1110");
		ds.Add("F", "1111");
	}

	internal string p(string A_0)
	{
		string text = "";
		string result;
		try
		{
			int length = A_0.Length;
			for (int num = 1; num <= length; num = checked(num + 1))
			{
				text += ds[Strings.Mid(A_0, num, 1)].ToString();
			}
			result = text;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:HexToBin:1] " + ex2.Message);
			result = "";
			ProjectData.ClearProjectError();
		}
		return result;
	}

	internal bool j(string A_0)
	{
		bool result;
		try
		{
			result = (checked(A_0.Length >= 2 && Operators.CompareString(A_0.Substring(0, 1), "+", false) == 0 && Operators.CompareString(A_0.Substring(1, 1), "+", false) != 0 && Operators.CompareString(A_0.Substring(1, 1), "-", false) != 0 && Versioned.IsNumeric((object)A_0.Substring(1, A_0.Length - 1)) && !A_0.Contains(",") && !A_0.Contains(".") && Conversions.ToDouble(A_0.Substring(1, A_0.Length - 1)) != 0.0) ? true : false);
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:IsValidIntPhone:1] " + ex2.Message);
			result = false;
			ProjectData.ClearProjectError();
		}
		return result;
	}

	internal bool f(string A_0)
	{
		bool result;
		try
		{
			result = ((A_0.Length >= 1 && Operators.CompareString(A_0.Substring(0, 1), "+", false) != 0 && Operators.CompareString(A_0.Substring(0, 1), "-", false) != 0 && Versioned.IsNumeric((object)A_0) && !A_0.Contains(",") && !A_0.Contains(".") && Conversions.ToDouble(A_0) != 0.0) ? true : false);
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:IsValidNatPhone:1] " + ex2.Message);
			result = false;
			ProjectData.ClearProjectError();
		}
		return result;
	}

	private void d()
	{
		dt.Add(64, 0);
		dt.Add(163, 1);
		dt.Add(36, 2);
		dt.Add(165, 3);
		dt.Add(232, 4);
		dt.Add(233, 5);
		dt.Add(249, 6);
		dt.Add(236, 7);
		dt.Add(242, 8);
		dt.Add(199, 9);
		dt.Add(216, 11);
		dt.Add(248, 12);
		dt.Add(197, 14);
		dt.Add(229, 15);
		dt.Add(916, 16);
		dt.Add(95, 17);
		dt.Add(934, 18);
		dt.Add(915, 19);
		dt.Add(923, 20);
		dt.Add(937, 21);
		dt.Add(928, 22);
		dt.Add(936, 23);
		dt.Add(931, 24);
		dt.Add(920, 25);
		dt.Add(926, 26);
		dt.Add(198, 28);
		dt.Add(230, 29);
		dt.Add(223, 30);
		dt.Add(201, 31);
		dt.Add(164, 36);
		dt.Add(196, 91);
		dt.Add(214, 92);
		dt.Add(209, 93);
		dt.Add(220, 94);
		dt.Add(167, 95);
		dt.Add(191, 96);
		dt.Add(228, 123);
		dt.Add(246, 124);
		dt.Add(241, 125);
		dt.Add(252, 126);
		dt.Add(224, 127);
	}

	private void c()
	{
		du.Add(12, 10);
		du.Add(94, 20);
		du.Add(123, 40);
		du.Add(125, 41);
		du.Add(92, 47);
		du.Add(91, 60);
		du.Add(126, 61);
		du.Add(93, 62);
		du.Add(124, 64);
		du.Add(8364, 101);
	}

	private void b()
	{
		dv.Add(0, 64);
		dv.Add(1, 163);
		dv.Add(2, 36);
		dv.Add(3, 165);
		dv.Add(4, 232);
		dv.Add(5, 233);
		dv.Add(6, 249);
		dv.Add(7, 236);
		dv.Add(8, 242);
		dv.Add(9, 199);
		dv.Add(11, 216);
		dv.Add(12, 248);
		dv.Add(14, 197);
		dv.Add(15, 229);
		dv.Add(16, 916);
		dv.Add(17, 95);
		dv.Add(18, 934);
		dv.Add(19, 915);
		dv.Add(20, 923);
		dv.Add(21, 937);
		dv.Add(22, 928);
		dv.Add(23, 936);
		dv.Add(24, 931);
		dv.Add(25, 920);
		dv.Add(26, 926);
		dv.Add(28, 198);
		dv.Add(29, 230);
		dv.Add(30, 223);
		dv.Add(31, 201);
		dv.Add(36, 164);
		dv.Add(91, 196);
		dv.Add(92, 214);
		dv.Add(93, 209);
		dv.Add(94, 220);
		dv.Add(95, 167);
		dv.Add(96, 191);
		dv.Add(123, 228);
		dv.Add(124, 246);
		dv.Add(125, 241);
		dv.Add(126, 252);
		dv.Add(127, 224);
	}

	private void a()
	{
		dw.Add(10, 12);
		dw.Add(20, 94);
		dw.Add(40, 123);
		dw.Add(41, 125);
		dw.Add(47, 92);
		dw.Add(60, 91);
		dw.Add(61, 126);
		dw.Add(62, 93);
		dw.Add(64, 124);
		dw.Add(101, 8364);
		dw.Add(27, 32);
	}

	internal string c(string A_0)
	{
		string text = "";
		string text2 = "\u001b";
		string result = A_0;
		checked
		{
			try
			{
				if (A_0.Length > 0)
				{
					int length = A_0.Length;
					int num3;
					for (int num = 1; num <= length; text += Conversions.ToString(Strings.ChrW(num3)), num++)
					{
						int num2 = Strings.AscW(A_0.Substring(num - 1, 1));
						num3 = 999;
						if (dt.Contains(num2))
						{
							num3 = Conversions.ToInteger(dt[num2]);
							continue;
						}
						if (du.Contains(num2))
						{
							num3 = Conversions.ToInteger(du[num2]);
							text += text2;
							continue;
						}
						switch (num2)
						{
						default:
							if (num2 < 97 || num2 > 122)
							{
								break;
							}
							goto case 10;
						case 10:
						case 13:
						case 32:
						case 33:
						case 34:
						case 35:
						case 37:
						case 38:
						case 39:
						case 40:
						case 41:
						case 42:
						case 43:
						case 44:
						case 45:
						case 46:
						case 47:
						case 48:
						case 49:
						case 50:
						case 51:
						case 52:
						case 53:
						case 54:
						case 55:
						case 56:
						case 57:
						case 58:
						case 59:
						case 60:
						case 61:
						case 62:
						case 63:
						case 65:
						case 66:
						case 67:
						case 68:
						case 69:
						case 70:
						case 71:
						case 72:
						case 73:
						case 74:
						case 75:
						case 76:
						case 77:
						case 78:
						case 79:
						case 80:
						case 81:
						case 82:
						case 83:
						case 84:
						case 85:
						case 86:
						case 87:
						case 88:
						case 89:
						case 90:
							num3 = num2;
							continue;
						}
						num3 = 63;
					}
					result = text;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				this.a.b("Error converting message to 7-bit default GSM string");
				result = "";
				ProjectData.ClearProjectError();
			}
			return result;
		}
	}

	internal string a(string A_0, ref int A_1)
	{
		bool flag = false;
		string text = "\u001b";
		string text2 = "";
		string result = A_0;
		checked
		{
			try
			{
				if (A_0.Length > 0)
				{
					int length = A_0.Length;
					for (int num = 1; num <= length; num++)
					{
						int num2 = Strings.AscW(Strings.Mid(A_0, num, 1));
						int num3 = 63;
						if (!flag)
						{
							if (dv.Contains(num2))
							{
								num3 = Conversions.ToInteger(dv[num2]);
							}
							else if (num2 == 27)
							{
								flag = true;
							}
							else
							{
								num3 = num2;
							}
						}
						else
						{
							if (dw.Contains(num2))
							{
								num3 = Conversions.ToInteger(dw[num2]);
							}
							A_1--;
							flag = false;
						}
						if (!flag)
						{
							text2 += Conversions.ToString(Strings.ChrW(num3));
						}
					}
					result = text2;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				this.a.b("[GSM:GSM2ISO:1] " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			return result;
		}
	}

	internal string b(string A_0)
	{
		int num = 1;
		string text = "";
		string text2 = "";
		string text3 = "";
		string text4 = "";
		string result;
		try
		{
			text = "";
			for (; num <= A_0.Length; num = checked(num + 2))
			{
				text4 = Strings.Mid(A_0, num, 2);
				text3 = Strings.Mid(text4, 1, 1);
				text2 = Strings.Mid(text4, 2, 1);
				if (text2.Length == 0)
				{
					text2 = "F";
				}
				text = text + text2 + text3;
			}
			result = text;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:RevNum:1] " + ex2.Message);
			result = "";
			ProjectData.ClearProjectError();
		}
		return result;
	}

	internal string b(int A_0, int A_1 = 2)
	{
		string text = "";
		string result;
		try
		{
			text = Conversion.Hex(A_0);
			while (text.Length < A_1)
			{
				text = "0" + text;
			}
			result = text;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:DecToHex:1] " + ex2.Message);
			result = "";
			ProjectData.ClearProjectError();
		}
		return result;
	}

	internal long m(string A_0)
	{
		long result;
		try
		{
			result = Conversions.ToLong("&H" + A_0);
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:HexToDec:1] " + ex2.Message);
			result = 0L;
			ProjectData.ClearProjectError();
		}
		return result;
	}

	internal string a(string A_0, string A_1, string A_2, DateTime A_3, string A_4, string A_5, int A_6, bool A_7)
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		int num = 0;
		string text4 = "";
		this.a.b("Sending WAP push to " + A_0 + " [Action: " + A_5 + "; Create: " + Conversions.ToString(A_3) + "; Expiry: " + A_4 + "]");
		string result = "";
		cb = "";
		cc = 0L;
		if (!bl || Operators.CompareString(bh, "READY", false) != 0)
		{
			this.a.b("Verifying network registration status");
			if (!aj())
			{
				cb = bm;
				cc = bn;
				goto IL_08a1;
			}
		}
		checked
		{
			if (!bv && !av())
			{
				cb = "Cannot check PDU mode support";
				cc = 7012L;
			}
			else if (!bw)
			{
				cb = "Modem does not support PDU mode";
				cc = 7013L;
			}
			else
			{
				try
				{
					cv++;
					if (cv > 255)
					{
						cv = 1;
					}
					text = a(A_0, A_1, A_2, A_3, A_4, A_5, cv, A_6, A_7);
					long num2 = text.Length;
					if (num2 <= 0)
					{
						cb = "Error generating PDU for WAP push";
						cc = 7014L;
						goto IL_08a1;
					}
					while (num <= b0)
					{
						aw();
						if (!s().IsOpen)
						{
							v();
						}
						bool flag;
						DateTime now;
						if (!s().IsOpen || this.z != 0)
						{
							cb = "Cannot connect to modem for sending WAP push";
							cc = 7015L;
						}
						else
						{
							if (num == 0)
							{
								while ((double)DateAndTime.DateDiff((DateInterval)9, cx, DateAndTime.Now, (FirstDayOfWeek)1, (FirstWeekOfYear)1) < (double)bz / 1000.0)
								{
									Thread.Sleep(10);
								}
							}
							else
							{
								Thread.Sleep(bz);
							}
							flag = false;
							text2 = "AT+CMGS=" + (double)(num2 - bs.Length) / 2.0 + "\r";
							try
							{
								c4 = true;
								c5 = true;
								if (Monitor.TryEnter(this.m_l, a4))
								{
									try
									{
										this.m_k.a = "";
									}
									catch (Exception ex)
									{
										ProjectData.SetProjectError(ex);
										Exception ex2 = ex;
										ProjectData.ClearProjectError();
									}
									finally
									{
										Monitor.Exit(this.m_l);
									}
									s().Write(text2);
									now = DateTime.Now;
									this.a.b("Modem Command: " + text2);
									goto IL_0385;
								}
								cb = "Thread monitor enter attempt timeout [GSM:SendWapPush:A]";
								cc = 9011L;
								ak();
							}
							catch (InvalidOperationException ex3)
							{
								ProjectData.SetProjectError((Exception)ex3);
								InvalidOperationException ex4 = ex3;
								cb = "Send WAP push command error (Port not open). " + ex4.Message;
								cc = 7016L;
								ak();
								ProjectData.ClearProjectError();
							}
							catch (TimeoutException ex5)
							{
								ProjectData.SetProjectError((Exception)ex5);
								TimeoutException ex6 = ex5;
								cb = "Send WAP push command error (Timeout). " + ex6.Message;
								cc = 7017L;
								ak();
								ProjectData.ClearProjectError();
							}
						}
						goto IL_071c;
						IL_071c:
						c4 = false;
						c5 = false;
						c6 = false;
						num++;
						if (cc == 0)
						{
							cc = 7022L;
							cb = "Unknown error while sending WAP push";
						}
						if (num <= b0)
						{
							a(cc, cb + " [Send Attempt: " + num + "]", "WAPPUSH");
							cc = 0L;
							cb = "";
						}
						if (c9 && s().IsOpen)
						{
							ak();
						}
						cx = DateAndTime.Now;
						continue;
						IL_0385:
						text3 = "";
						while (!flag && (double)DateAndTime.DateDiff((DateInterval)9, now, DateAndTime.Now, (FirstDayOfWeek)1, (FirstWeekOfYear)1) < (double)this.av / 1000.0)
						{
							Thread.Sleep(5);
							text3 += p();
							if (text3.Length > 0 && text3.ToUpper().Contains(">") && !text3.ToUpper().Contains("ERROR"))
							{
								flag = true;
								break;
							}
							if (text3.ToUpper().Contains("ERROR"))
							{
								break;
							}
							Application.DoEvents();
						}
						c5 = false;
						c4 = false;
						if (!flag)
						{
							this.a.b("Modem Response: " + text3);
							cb = "Invalid response to WAP push command";
							cc = 7018L;
							ak();
						}
						else
						{
							try
							{
								c4 = true;
								c6 = true;
								s().Write(text + "\u001a");
								now = DateAndTime.Now;
								this.a.b("PDU: " + text);
							}
							catch (InvalidOperationException ex7)
							{
								ProjectData.SetProjectError((Exception)ex7);
								InvalidOperationException ex8 = ex7;
								cb = "Submit WAP push PDU error (Port not open). " + ex8.Message;
								cc = 7019L;
								ak();
								ProjectData.ClearProjectError();
								goto IL_071c;
							}
							catch (TimeoutException ex9)
							{
								ProjectData.SetProjectError((Exception)ex9);
								TimeoutException ex10 = ex9;
								cb = "Submit WAP push PDU error (Timeout). " + ex10.Message;
								cc = 7020L;
								ak();
								ProjectData.ClearProjectError();
								goto IL_071c;
							}
							bool flag2 = false;
							Thread.Sleep(1000);
							text3 = "";
							while (!flag2 && (double)DateAndTime.DateDiff((DateInterval)9, now, DateAndTime.Now, (FirstDayOfWeek)1, (FirstWeekOfYear)1) < (double)this.av / 1000.0)
							{
								text3 += p();
								if (text3.Length > 0 && string.Compare(text3, text + "\u001a", ignoreCase: true) != 0 && !text3.ToUpper().Contains("ERROR"))
								{
									try
									{
										text4 = text3.Replace(text + "\u001a", "");
										if (text4.Contains("\r\nOK"))
										{
											text4 = Strings.Mid(text4, 1, Strings.InStr(text4, "\r\nOK", (CompareMethod)1) + 3);
										}
										text4 = a(text4.Replace("+CMGS:", ""), A_1: true, A_2: true, A_3: true).Trim();
										if (text4.Contains(","))
										{
											string[] array = text4.Split(",".ToCharArray());
											if (array.GetUpperBound(0) >= 1)
											{
												text4 = array[0];
											}
										}
										if (Versioned.IsNumeric((object)text4))
										{
											flag2 = true;
											break;
										}
									}
									catch (Exception ex11)
									{
										ProjectData.SetProjectError(ex11);
										Exception ex12 = ex11;
										ProjectData.ClearProjectError();
									}
								}
								if (text3.ToUpper().Contains("ERROR"))
								{
									break;
								}
								Application.DoEvents();
								Thread.Sleep(25);
							}
							c6 = false;
							c4 = false;
							text3 = text3.Replace(text + "\u001a", "");
							this.a.b("Modem Response: " + text3);
							if (flag2)
							{
								cx = DateAndTime.Now;
								break;
							}
							cb = "Invalid response to submit WAP push PDU [Response: " + text3 + "]";
							cc = 7021L;
							if (text3.Trim().Length <= 0 || text3.Contains("ERROR"))
							{
								ak();
							}
						}
						goto IL_071c;
					}
					if (num > b0 || cc > 0)
					{
						this.a.b("Wap push sending failed");
						result = "";
					}
					if (cc == 0)
					{
						result = text4;
						this.a.b("Wap push sent [Msg ID: " + text4 + "]");
					}
				}
				catch (Exception ex13)
				{
					ProjectData.SetProjectError(ex13);
					Exception ex14 = ex13;
					c4 = false;
					c5 = false;
					c6 = false;
					cb = "System error (" + ex14.Message + ")";
					cc = 7026L;
					this.a.b("Wap push sending failed");
					result = "";
					ProjectData.ClearProjectError();
				}
				c4 = false;
				c5 = false;
				c6 = false;
			}
			goto IL_08a1;
		}
		IL_08a1:
		return result;
	}

	internal string a(string A_0, string A_1, string A_2, DateTime A_3, string A_4, string A_5, int A_6, int A_7, bool A_8)
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		string text4 = "";
		string text5 = "";
		string text6 = "";
		string text7 = "";
		string text8 = "";
		string text9 = "";
		string text10 = "";
		string text11 = "";
		string text12 = "";
		string text13 = "";
		bool flag = A_8;
		string result = "";
		text3 = a(A_1, A_2, A_3, A_4, A_5);
		text = an();
		text2 = ax();
		checked
		{
			try
			{
				if (bs.Length != 0)
				{
					goto IL_00e0;
				}
				aq();
				if (bq.Length > 0)
				{
					if (bs.Length == 0)
					{
						bs = "00";
					}
					goto IL_00e0;
				}
				a(7023L, "SMSC not specified", "WAPPUSH");
				result = "";
				goto end_IL_0082;
				IL_0141:
				text10 = b(A_0.Length, 2) + text9 + b(A_0);
				text7 = "00";
				text8 = "F5";
				text11 = b(A_7, 2);
				text12 = text + text2 + text3;
				text13 = b((int)Math.Round((double)text12.Length / 2.0), 2);
				if (m(text13) > 140)
				{
					a(7024L, "WAP push data length overflow", "WAPPUSH");
					result = "";
				}
				else
				{
					result = text4 + text5 + text6 + text10 + text7 + text8 + text11 + text13 + text12;
				}
				goto end_IL_0082;
				IL_00e0:
				text4 = bs;
				text5 = ((!flag) ? "51" : "71");
				text6 = "00";
				if (j(A_0))
				{
					A_0 = A_0.Substring(1, A_0.Length - 1);
					text9 = "91";
					goto IL_0141;
				}
				if (f(A_0))
				{
					text9 = "81";
					goto IL_0141;
				}
				result = "";
				end_IL_0082:;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				this.a.b("[GSM:GetWapPDU:1] " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			return result;
		}
	}

	internal string an()
	{
		string text = "";
		string text2 = "";
		string result = "";
		checked
		{
			try
			{
				text2 = b((int)Math.Round((double)("0B84".Length + "23F0".Length) / 2.0), 2);
				text = b((int)Math.Round((double)("05".Length + text2.Length) / 2.0 + (double)m(text2)), 2);
				result = text + "05" + text2 + "0B8423F0";
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				this.a.b("[GSM:WDPHeader:1] " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			return result;
		}
	}

	internal string ax()
	{
		string text = "";
		string text2 = "";
		string result = "";
		try
		{
			text = "03AE81EAAF82B484";
			text2 = b(checked((int)Math.Round((double)text.Length / 2.0)), 2);
			result = "2506" + text2 + text;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:WSPHeader:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		return result;
	}

	internal string a(string A_0, string A_1, DateTime A_2, string A_3, string A_4)
	{
		//IL_0572: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		string text = "";
		string text2 = "";
		string text3 = "";
		string text4 = "";
		string text5 = "";
		string text6 = "";
		string text7 = "";
		string text8 = "";
		string text9 = "";
		string text10 = "";
		string text11 = "";
		string text12 = "";
		string text13 = "";
		string text14 = "";
		string text15 = "";
		string text16 = "";
		string text17 = "";
		string text18 = "";
		string text19 = "";
		string text20 = "";
		string result = "";
		checked
		{
			try
			{
				text20 = l("O\u0017k.\u0017T0FNjD~{p\r=+6`%\\-P\u00059@GC");
				text2 = A_1;
				text3 = "";
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				int num = 0;
				DateTime now = DateAndTime.Now;
				if (!this.af)
				{
					this.b.a();
				}
				if (Operators.CompareString(this.ah, cl, false) == 0)
				{
					a(7025L, l("odumu5VOg_:5-26cCP/GtqPa6g$"), "WAPPUSH");
					result = "";
				}
				else
				{
					if (Operators.CompareString(this.ah, ck, false) == 0)
					{
						A_0 += text20;
					}
					text = "01056A0045C6";
					text += A_4;
					if (A_1.ToLower().StartsWith("http://"))
					{
						if (A_1.ToLower().StartsWith("http://www."))
						{
							text += "0D";
							text3 = "http://www.";
						}
						else
						{
							text += "0C";
							text3 = "http://";
						}
					}
					else if (A_1.ToLower().StartsWith("https://"))
					{
						if (A_1.ToLower().StartsWith("https://www."))
						{
							text += "0F";
							text3 = "https://www.";
						}
						else
						{
							text += "0E";
							text3 = "https://";
						}
					}
					if (text3.Length == 0)
					{
						text += "0B";
						text3 = "";
					}
					else
					{
						text2 = Strings.Right(text2, text2.Length - text3.Length);
					}
					switch (Strings.Right(text2, 4).ToLower())
					{
					case ".com":
					case ".edu":
					case ".net":
					case ".org":
						text2 += "/";
						break;
					}
					num = Strings.InStr(text2.ToLower(), ".com/", (CompareMethod)0);
					if (num != 0)
					{
						flag = true;
					}
					else
					{
						num = Strings.InStr(text2.ToLower(), ".edu/", (CompareMethod)0);
						if (num != 0)
						{
							flag2 = true;
						}
						else
						{
							num = Strings.InStr(text2.ToLower(), ".net/", (CompareMethod)0);
							if (num != 0)
							{
								flag3 = true;
							}
							else
							{
								num = Strings.InStr(text2.ToLower(), ".org/", (CompareMethod)0);
								if (num != 0)
								{
									flag4 = true;
								}
							}
						}
					}
					if (num > 0)
					{
						text = text + "03" + q(text2.Substring(0, num - 1)) + "00";
						if (flag)
						{
							text += "85";
						}
						else if (flag2)
						{
							text += "86";
						}
						else if (flag3)
						{
							text += "87";
						}
						else if (flag4)
						{
							text += "88";
						}
						if (text2.Length > num + 4)
						{
							text = text + "03" + q(text2.Substring(num + 4, text2.Length - num - 4)) + "00";
						}
					}
					else
					{
						text = text + "03" + q(text2) + "00";
					}
					if (DateTime.Compare(A_2, DateTime.MinValue) != 0)
					{
						DateTime dateTime = default(DateTime);
						if (Information.IsDate((object)A_2))
						{
							dateTime = A_2;
						}
						text6 = DateAndTime.Year(dateTime).ToString();
						text7 = DateAndTime.Month(dateTime).ToString();
						text8 = DateAndTime.Day(dateTime).ToString();
						text9 = DateAndTime.Hour(dateTime).ToString();
						text10 = DateAndTime.Minute(dateTime).ToString();
						while (text7.Length < 2)
						{
							text7 = "0" + text7;
						}
						while (text8.Length < 2)
						{
							text8 = "0" + text8;
						}
						while (text9.Length < 2)
						{
							text9 = "0" + text9;
						}
						while (text10.Length < 2)
						{
							text10 = "0" + text10;
						}
						text5 = text6 + text7 + text8 + text9 + text10;
						while (Operators.CompareString(Strings.Right(text5, 2), "00", false) == 0)
						{
							text5 = text5.Substring(0, text5.Length - 2);
						}
						text18 = b((int)Math.Round((double)text5.Length / 2.0), 2);
						text = text + "0AC3" + text18 + text5;
					}
					if (Operators.CompareString(A_3, "0", false) != 0)
					{
						text11 = A_3.Substring(A_3.Length - 1, 1).ToUpper();
						long num2 = Conversions.ToLong(A_3.Substring(0, A_3.Length - 1));
						DateInterval val = (DateInterval)4;
						switch (text11)
						{
						case "H":
							val = (DateInterval)7;
							break;
						case "D":
							val = (DateInterval)4;
							break;
						case "M":
							val = (DateInterval)2;
							break;
						}
						DateTime dateTime2 = DateAndTime.DateAdd(val, (double)num2, now);
						text12 = DateAndTime.Year(dateTime2).ToString();
						text13 = DateAndTime.Month(dateTime2).ToString();
						text14 = DateAndTime.Day(dateTime2).ToString();
						text15 = DateAndTime.Hour(dateTime2).ToString();
						text16 = DateAndTime.Minute(dateTime2).ToString();
						while (text13.Length < 2)
						{
							text13 = "0" + text13;
						}
						while (text14.Length < 2)
						{
							text14 = "0" + text14;
						}
						while (text15.Length < 2)
						{
							text15 = "0" + text15;
						}
						while (text16.Length < 2)
						{
							text16 = "0" + text16;
						}
						text17 = text12 + text13 + text14 + text15 + text16;
						while (Operators.CompareString(Strings.Right(text17, 2), "00", false) == 0)
						{
							text17 = text17.Substring(0, text17.Length - 2);
						}
						text19 = b((int)Math.Round((double)text17.Length / 2.0), 2);
						text = text + "10C3" + text19 + text17;
					}
					text = text + "0103" + q(A_0) + "000101";
					result = text;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				this.a.b("[GSM:WapPushWBXML:1] " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			return result;
		}
	}

	internal string a(string A_0, string A_1, int A_2, bool A_3)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		string text = "";
		string text2 = "";
		string text3 = "";
		bool flag = false;
		string text4 = "";
		string text5 = "\u001b";
		string text6 = "";
		string text7 = "";
		this.a.b("Sending vCard to " + A_1);
		string result = "";
		cd = "";
		ce = 0L;
		int a_ = cv;
		Collection val = new Collection();
		if (!bl || Operators.CompareString(bh, "READY", false) != 0)
		{
			this.a.b("Verifying network registration status");
			if (!aj())
			{
				cd = bm;
				ce = bn;
				goto IL_096c;
			}
		}
		checked
		{
			if (!bv && !av())
			{
				cd = "Cannot check PDU mode support";
				ce = 8044L;
			}
			else if (!bw)
			{
				cd = "Modem does not support PDU mode";
				ce = 8045L;
			}
			else
			{
				try
				{
					cv++;
					if (cv > 255)
					{
						cv = 1;
					}
					if (A_0.Length <= 133)
					{
						val.Add((object)A_0, "1", (object)null, (object)null);
						flag = true;
					}
					else
					{
						int num = 0;
						int num2 = 1;
						while (!flag)
						{
							text4 = Strings.Mid(A_0, num + 1, 128);
							val.Add((object)text4, num2.ToString(), (object)null, (object)null);
							num += text4.Length;
							num2++;
							if (num >= A_0.Length)
							{
								flag = true;
							}
						}
					}
					int count = val.Count;
					if (count > 0 && count <= 255)
					{
						int num = 1;
						int num3 = count;
						num = 1;
						while (true)
						{
							if (num <= num3)
							{
								aw();
								int num4 = 0;
								text4 = val[num.ToString()].ToString();
								text3 = b(text4, A_1, cv, a_, num, count, A_2, A_3);
								long num5 = text3.Length;
								if (num5 <= 0)
								{
									cd = "Error generating PDU for vCard";
									ce = 8046L;
									break;
								}
								while (num4 <= b0)
								{
									aw();
									if (!s().IsOpen)
									{
										v();
									}
									bool flag2;
									DateTime now;
									if (!s().IsOpen || this.z != 0)
									{
										cd = "Cannot connect to modem for sending vCard";
										ce = 8047L;
									}
									else
									{
										if (num == 1 && num4 == 0)
										{
											while ((double)DateAndTime.DateDiff((DateInterval)9, cx, DateAndTime.Now, (FirstDayOfWeek)1, (FirstWeekOfYear)1) < (double)bz / 1000.0)
											{
												Thread.Sleep(10);
											}
										}
										else
										{
											Thread.Sleep(bz);
										}
										flag2 = false;
										text = "AT+CMGS=" + (double)(num5 - bs.Length) / 2.0 + "\r";
										try
										{
											c4 = true;
											c5 = true;
											if (Monitor.TryEnter(this.m_l, a4))
											{
												try
												{
													this.m_k.a = "";
												}
												catch (Exception ex)
												{
													ProjectData.SetProjectError(ex);
													Exception ex2 = ex;
													ProjectData.ClearProjectError();
												}
												finally
												{
													Monitor.Exit(this.m_l);
												}
												s().Write(text);
												now = DateTime.Now;
												this.a.b("Modem Command: " + text);
												goto IL_0411;
											}
											cd = "Thread monitor enter attempt timeout [GSM:SendVCard:A]";
											ce = 9011L;
											ak();
										}
										catch (InvalidOperationException ex3)
										{
											ProjectData.SetProjectError((Exception)ex3);
											InvalidOperationException ex4 = ex3;
											cd = "Send vCard command error (Port not open). " + ex4.Message;
											ce = 8048L;
											ak();
											ProjectData.ClearProjectError();
										}
										catch (TimeoutException ex5)
										{
											ProjectData.SetProjectError((Exception)ex5);
											TimeoutException ex6 = ex5;
											cd = "Send vCard command error (Timeout). " + ex6.Message;
											ce = 8049L;
											ak();
											ProjectData.ClearProjectError();
										}
									}
									goto IL_07ce;
									IL_07ce:
									c4 = false;
									c5 = false;
									c6 = false;
									num4++;
									if (ce == 0)
									{
										ce = 8054L;
										cd = "Unknown error while sending vCard";
									}
									if (num4 <= b0)
									{
										a(ce, cd + " [Send Attempt: " + num4 + "]", "VCARD");
										ce = 0L;
										cd = "";
									}
									if (c9 && s().IsOpen)
									{
										ak();
									}
									cx = DateAndTime.Now;
									continue;
									IL_0411:
									text2 = "";
									while (!flag2 && (double)DateAndTime.DateDiff((DateInterval)9, now, DateAndTime.Now, (FirstDayOfWeek)1, (FirstWeekOfYear)1) < (double)this.av / 1000.0)
									{
										Thread.Sleep(5);
										text2 += p();
										if (text2.Length > 0 && text2.ToUpper().Contains(">") && !text2.ToUpper().Contains("ERROR"))
										{
											flag2 = true;
											break;
										}
										if (text2.ToUpper().Contains("ERROR"))
										{
											break;
										}
										Application.DoEvents();
									}
									c5 = false;
									c4 = false;
									if (!flag2)
									{
										this.a.b("Modem Response: " + text2);
										cd = "Invalid response to send vCard command";
										ce = 8050L;
										ak();
									}
									else
									{
										try
										{
											c4 = true;
											c6 = true;
											s().Write(text3 + "\u001a");
											now = DateAndTime.Now;
											this.a.b("PDU: " + text3);
										}
										catch (InvalidOperationException ex7)
										{
											ProjectData.SetProjectError((Exception)ex7);
											InvalidOperationException ex8 = ex7;
											cd = "Submit vCard PDU error (Port not open). " + ex8.Message;
											ce = 8051L;
											ak();
											ProjectData.ClearProjectError();
											goto IL_07ce;
										}
										catch (TimeoutException ex9)
										{
											ProjectData.SetProjectError((Exception)ex9);
											TimeoutException ex10 = ex9;
											cd = "Submit vCard PDU error (Timeout). " + ex10.Message;
											ce = 8052L;
											ak();
											ProjectData.ClearProjectError();
											goto IL_07ce;
										}
										bool flag3 = false;
										Thread.Sleep(1000);
										text2 = "";
										while (!flag3 && (double)DateAndTime.DateDiff((DateInterval)9, now, DateAndTime.Now, (FirstDayOfWeek)1, (FirstWeekOfYear)1) < (double)this.av / 1000.0)
										{
											text2 += p();
											if (text2.Length > 0 && string.Compare(text2, text3 + "\u001a", ignoreCase: true) != 0 && !text2.ToUpper().Contains("ERROR"))
											{
												try
												{
													text6 = text2.Replace(text3 + "\u001a", "");
													if (text6.Contains("\r\nOK"))
													{
														text6 = Strings.Mid(text6, 1, Strings.InStr(text6, "\r\nOK", (CompareMethod)1) + 3);
													}
													text6 = a(text6.Replace("+CMGS:", ""), A_1: true, A_2: true, A_3: true).Trim();
													if (text6.Contains(","))
													{
														string[] array = text6.Split(",".ToCharArray());
														if (array.GetUpperBound(0) >= 1)
														{
															text6 = array[0];
														}
													}
													if (Versioned.IsNumeric((object)text6))
													{
														flag3 = true;
														break;
													}
												}
												catch (Exception ex11)
												{
													ProjectData.SetProjectError(ex11);
													Exception ex12 = ex11;
													ProjectData.ClearProjectError();
												}
											}
											if (text2.ToUpper().Contains("ERROR"))
											{
												break;
											}
											Application.DoEvents();
											Thread.Sleep(25);
										}
										c6 = false;
										c4 = false;
										text2 = text2.Replace(text3 + "\u001a", "");
										this.a.b("Modem Response: " + text2);
										if (flag3)
										{
											cx = DateAndTime.Now;
											text7 = ((text7.Length > 0) ? (text7 + "," + text6) : text6);
											break;
										}
										cd = "Invalid response to submit vCard PDU [Response: " + text2 + "]";
										ce = 8053L;
										if (text2.Trim().Length <= 0 || text2.Contains("ERROR"))
										{
											ak();
										}
									}
									goto IL_07ce;
								}
								if (num4 <= b0 && ce <= 0)
								{
									num++;
									continue;
								}
							}
							if (ce > 0)
							{
								this.a.b("vCard sending failed");
								result = "";
							}
							else
							{
								this.a.b("vCard sent [Msg ID: " + text7 + "]");
								result = text7;
							}
							goto IL_0957;
						}
					}
				}
				catch (Exception ex13)
				{
					ProjectData.SetProjectError(ex13);
					Exception ex14 = ex13;
					c4 = false;
					c5 = false;
					c6 = false;
					ce = 8055L;
					cd = "System error (" + ex14.Message + ")";
					this.a.b("vCard sending failed");
					result = "";
					ProjectData.ClearProjectError();
					goto IL_0957;
				}
			}
			goto IL_096c;
		}
		IL_0957:
		c4 = false;
		c5 = false;
		c6 = false;
		goto IL_096c;
		IL_096c:
		return result;
	}

	private string b(string A_0, string A_1, int A_2, int A_3, int A_4, int A_5, int A_6, bool A_7)
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		string text4 = "";
		string text5 = "";
		string text6 = "";
		string text7 = "";
		string text8 = "";
		string text9 = "";
		string text10 = "";
		bool flag = A_7;
		string result = "";
		checked
		{
			try
			{
				if (bs.Length != 0)
				{
					goto IL_00ab;
				}
				aq();
				if (bq.Length > 0)
				{
					if (bs.Length == 0)
					{
						bs = "00";
					}
					goto IL_00ab;
				}
				a(8056L, "SMSC not specified", "VCARD");
				result = "";
				goto end_IL_004d;
				IL_00ab:
				text = bs;
				if (flag && A_5 > 1 && A_4 < A_5)
				{
					flag = false;
				}
				text2 = ((!flag) ? "51" : "71");
				text3 = "00";
				if (j(A_1))
				{
					A_1 = A_1.Substring(1, A_1.Length - 1);
					text6 = "91";
					goto IL_011c;
				}
				if (f(A_1))
				{
					text6 = "81";
					goto IL_011c;
				}
				result = "";
				goto end_IL_004d;
				IL_011c:
				text7 = b(A_1.Length, 2) + text6 + b(A_1);
				text4 = "00";
				text5 = "F5";
				text8 = b(A_6, 2);
				text9 = ((A_5 <= 1) ? ("06050423F40000" + q(A_0)) : ("0B050423F400000003" + b(A_3, 2) + b(A_5, 2) + b(A_4, 2) + q(A_0)));
				text10 = b((int)Math.Round((double)text9.Length / 2.0), 2);
				if (m(text10) > 140)
				{
					a(8057L, "vCard data length overflow", "VCARD");
					result = "";
				}
				else
				{
					result = text + text2 + text3 + text7 + text4 + text5 + text8 + text10 + text9;
				}
				end_IL_004d:;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				this.a.b("[GSM:GetVCardPDU:1] " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			return result;
		}
	}

	internal string b(string A_0, string A_1, int A_2, bool A_3)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		string text = "";
		string text2 = "";
		string text3 = "";
		bool flag = false;
		string text4 = "";
		string text5 = "\u001b";
		string text6 = "";
		string text7 = "";
		this.a.b("Sending vCalendar to " + A_1);
		string result = "";
		cf = "";
		cg = 0L;
		int a_ = cv;
		Collection val = new Collection();
		if (!bl || Operators.CompareString(bh, "READY", false) != 0)
		{
			this.a.b("Verifying network registration status");
			if (!aj())
			{
				cf = bm;
				cg = bn;
				goto IL_096c;
			}
		}
		checked
		{
			if (!bv && !av())
			{
				cf = "Cannot check PDU mode support";
				cg = 8058L;
			}
			else if (!bw)
			{
				cf = "Modem does not support PDU mode";
				cg = 8059L;
			}
			else
			{
				try
				{
					cv++;
					if (cv > 255)
					{
						cv = 1;
					}
					if (A_0.Length <= 133)
					{
						val.Add((object)A_0, "1", (object)null, (object)null);
						flag = true;
					}
					else
					{
						int num = 0;
						int num2 = 1;
						while (!flag)
						{
							text4 = Strings.Mid(A_0, num + 1, 128);
							val.Add((object)text4, num2.ToString(), (object)null, (object)null);
							num += text4.Length;
							num2++;
							if (num >= A_0.Length)
							{
								flag = true;
							}
						}
					}
					int count = val.Count;
					if (count > 0 && count <= 255)
					{
						int num = 1;
						int num3 = count;
						num = 1;
						while (true)
						{
							if (num <= num3)
							{
								aw();
								int num4 = 0;
								text4 = val[num.ToString()].ToString();
								text3 = a(text4, A_1, cv, a_, num, count, A_2, A_3);
								long num5 = text3.Length;
								if (num5 <= 0)
								{
									cf = "Error generating PDU for vCalendar";
									cg = 8060L;
									break;
								}
								while (num4 <= b0)
								{
									aw();
									if (!s().IsOpen)
									{
										v();
									}
									bool flag2;
									DateTime now;
									if (!s().IsOpen || this.z != 0)
									{
										cf = "Cannot connect to modem for sending vCalendar";
										cg = 8061L;
									}
									else
									{
										if (num == 1 && num4 == 0)
										{
											while ((double)DateAndTime.DateDiff((DateInterval)9, cx, DateAndTime.Now, (FirstDayOfWeek)1, (FirstWeekOfYear)1) < (double)bz / 1000.0)
											{
												Thread.Sleep(10);
											}
										}
										else
										{
											Thread.Sleep(bz);
										}
										flag2 = false;
										text = "AT+CMGS=" + (double)(num5 - bs.Length) / 2.0 + "\r";
										try
										{
											c4 = true;
											c5 = true;
											if (Monitor.TryEnter(this.m_l, a4))
											{
												try
												{
													this.m_k.a = "";
												}
												catch (Exception ex)
												{
													ProjectData.SetProjectError(ex);
													Exception ex2 = ex;
													ProjectData.ClearProjectError();
												}
												finally
												{
													Monitor.Exit(this.m_l);
												}
												s().Write(text);
												now = DateTime.Now;
												this.a.b("Modem Command: " + text);
												goto IL_0411;
											}
											cf = "Thread monitor enter attempt timeout [GSM:SendVCalendar:A]";
											cg = 9011L;
											ak();
										}
										catch (InvalidOperationException ex3)
										{
											ProjectData.SetProjectError((Exception)ex3);
											InvalidOperationException ex4 = ex3;
											cf = "Send vCalendar command error (Port not open). " + ex4.Message;
											cg = 8062L;
											ak();
											ProjectData.ClearProjectError();
										}
										catch (TimeoutException ex5)
										{
											ProjectData.SetProjectError((Exception)ex5);
											TimeoutException ex6 = ex5;
											cf = "Send vCalendar command error (Timeout). " + ex6.Message;
											cg = 8063L;
											ak();
											ProjectData.ClearProjectError();
										}
									}
									goto IL_07ce;
									IL_07ce:
									c4 = false;
									c5 = false;
									c6 = false;
									num4++;
									if (cg == 0)
									{
										cg = 8068L;
										cf = "Unknown error while sending vCalendar";
									}
									if (num4 <= b0)
									{
										a(cg, cf + " [Send Attempt: " + num4 + "]", "VCALENDAR");
										cg = 0L;
										cf = "";
									}
									if (c9 && s().IsOpen)
									{
										ak();
									}
									cx = DateAndTime.Now;
									continue;
									IL_0411:
									text2 = "";
									while (!flag2 && (double)DateAndTime.DateDiff((DateInterval)9, now, DateAndTime.Now, (FirstDayOfWeek)1, (FirstWeekOfYear)1) < (double)this.av / 1000.0)
									{
										Thread.Sleep(5);
										text2 += p();
										if (text2.Length > 0 && text2.ToUpper().Contains(">") && !text2.ToUpper().Contains("ERROR"))
										{
											flag2 = true;
											break;
										}
										if (text2.ToUpper().Contains("ERROR"))
										{
											break;
										}
										Application.DoEvents();
									}
									c5 = false;
									c4 = false;
									if (!flag2)
									{
										this.a.b("Modem Response: " + text2);
										cf = "Invalid response to vCalendar command";
										cg = 8064L;
										ak();
									}
									else
									{
										try
										{
											c4 = true;
											c6 = true;
											s().Write(text3 + "\u001a");
											now = DateAndTime.Now;
											this.a.b("PDU: " + text3);
										}
										catch (InvalidOperationException ex7)
										{
											ProjectData.SetProjectError((Exception)ex7);
											InvalidOperationException ex8 = ex7;
											cf = "Submit vCalendar PDU error (Port not open). " + ex8.Message;
											cg = 8065L;
											ak();
											ProjectData.ClearProjectError();
											goto IL_07ce;
										}
										catch (TimeoutException ex9)
										{
											ProjectData.SetProjectError((Exception)ex9);
											TimeoutException ex10 = ex9;
											cf = "Submit vCalendar PDU error (Timeout). " + ex10.Message;
											cg = 8066L;
											ak();
											ProjectData.ClearProjectError();
											goto IL_07ce;
										}
										bool flag3 = false;
										Thread.Sleep(1000);
										text2 = "";
										while (!flag3 && (double)DateAndTime.DateDiff((DateInterval)9, now, DateAndTime.Now, (FirstDayOfWeek)1, (FirstWeekOfYear)1) < (double)this.av / 1000.0)
										{
											text2 += p();
											if (text2.Length > 0 && string.Compare(text2, text3 + "\u001a", ignoreCase: true) != 0 && !text2.ToUpper().Contains("ERROR"))
											{
												try
												{
													text6 = text2.Replace(text3 + "\u001a", "");
													if (text6.Contains("\r\nOK"))
													{
														text6 = Strings.Mid(text6, 1, Strings.InStr(text6, "\r\nOK", (CompareMethod)1) + 3);
													}
													text6 = a(text6.Replace("+CMGS:", ""), A_1: true, A_2: true, A_3: true).Trim();
													if (text6.Contains(","))
													{
														string[] array = text6.Split(",".ToCharArray());
														if (array.GetUpperBound(0) >= 1)
														{
															text6 = array[0];
														}
													}
													if (Versioned.IsNumeric((object)text6))
													{
														flag3 = true;
														break;
													}
												}
												catch (Exception ex11)
												{
													ProjectData.SetProjectError(ex11);
													Exception ex12 = ex11;
													ProjectData.ClearProjectError();
												}
											}
											if (text2.ToUpper().Contains("ERROR"))
											{
												break;
											}
											Application.DoEvents();
											Thread.Sleep(25);
										}
										c6 = false;
										c4 = false;
										text2 = text2.Replace(text3 + "\u001a", "");
										this.a.b("Modem Response: " + text2);
										if (flag3)
										{
											cx = DateAndTime.Now;
											text7 = ((text7.Length > 0) ? (text7 + "," + text6) : text6);
											break;
										}
										cf = "Invalid response to submit vCalendar PDU [Response: " + text2 + "]";
										cg = 8067L;
										if (text2.Trim().Length <= 0 || text2.Contains("ERROR"))
										{
											ak();
										}
									}
									goto IL_07ce;
								}
								if (num4 <= b0 && cg <= 0)
								{
									num++;
									continue;
								}
							}
							if (cg > 0)
							{
								this.a.b("vCalendar sending failed");
								result = "";
							}
							else
							{
								this.a.b("vCalendar sent [Msg ID: " + text7 + "]");
								result = text7;
							}
							goto IL_0957;
						}
					}
				}
				catch (Exception ex13)
				{
					ProjectData.SetProjectError(ex13);
					Exception ex14 = ex13;
					c4 = false;
					c5 = false;
					c6 = false;
					cg = 8069L;
					cf = "System error (" + ex14.Message + ")";
					this.a.b("vCalendar sending failed");
					result = "";
					ProjectData.ClearProjectError();
					goto IL_0957;
				}
			}
			goto IL_096c;
		}
		IL_0957:
		c4 = false;
		c5 = false;
		c6 = false;
		goto IL_096c;
		IL_096c:
		return result;
	}

	private string a(string A_0, string A_1, int A_2, int A_3, int A_4, int A_5, int A_6, bool A_7)
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		string text4 = "";
		string text5 = "";
		string text6 = "";
		string text7 = "";
		string text8 = "";
		string text9 = "";
		bool flag = A_7;
		string result = "";
		checked
		{
			try
			{
				if (bs.Length != 0)
				{
					goto IL_00a4;
				}
				aq();
				if (bq.Length > 0)
				{
					if (bs.Length == 0)
					{
						bs = "00";
					}
					goto IL_00a4;
				}
				a(8070L, "SMSC not specified", "VCALENDAR");
				result = "";
				goto end_IL_0046;
				IL_00a4:
				text = bs;
				if (flag && A_5 > 1 && A_4 < A_5)
				{
					flag = false;
				}
				text2 = ((!flag) ? "51" : "71");
				text3 = "00";
				if (j(A_1))
				{
					A_1 = A_1.Substring(1, A_1.Length - 1);
					text5 = "91";
					goto IL_0115;
				}
				if (f(A_1))
				{
					text5 = "81";
					goto IL_0115;
				}
				result = "";
				goto end_IL_0046;
				IL_0115:
				text6 = b(A_1.Length, 2) + text5 + b(A_1);
				string text10 = "00";
				text4 = "F5";
				text7 = b(A_6, 2);
				text8 = ((A_5 <= 1) ? ("06050423F50000" + q(A_0)) : ("0B050423F500000003" + b(A_3, 2) + b(A_5, 2) + b(A_4, 2) + q(A_0)));
				text9 = b((int)Math.Round((double)text8.Length / 2.0), 2);
				if (m(text9) > 140)
				{
					a(8071L, "vCalendar data length overflow", "VCALENDAR");
					result = "";
				}
				else
				{
					result = text + text2 + text3 + text6 + text10 + text4 + text7 + text9 + text8;
				}
				end_IL_0046:;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				this.a.b("[GSM:GetVCalendarPDU:1] " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			return result;
		}
	}

	internal string a(string A_0, string A_1, int A_2, bool A_3, int A_4, int A_5)
	{
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		string text = "";
		string text2 = "";
		string text3 = "";
		bool flag = false;
		string text4 = "";
		string text5 = "\u001b";
		string text6 = "";
		string text7 = "";
		string a_ = "0000";
		string a_2 = "0000";
		if (A_4 > 0)
		{
			a_ = b(A_4, 4);
		}
		if (A_5 > 0)
		{
			a_2 = b(A_5, 4);
		}
		this.a.b("Sending smart message to " + A_1);
		string result = "";
		ch = "";
		ci = 0L;
		int a_3 = cv;
		Collection val = new Collection();
		if (!bl || Operators.CompareString(bh, "READY", false) != 0)
		{
			this.a.b("Verifying network registration status");
			if (!aj())
			{
				ch = bm;
				ci = bn;
				goto IL_0982;
			}
		}
		checked
		{
			if (!bv && !av())
			{
				ch = "Cannot check PDU mode support";
				ci = 4023L;
			}
			else if (!bw)
			{
				ch = "Modem does not support PDU mode";
				ci = 4024L;
			}
			else
			{
				try
				{
					cv++;
					if (cv > 255)
					{
						cv = 1;
					}
					if (A_0.Length <= 133)
					{
						val.Add((object)A_0, "1", (object)null, (object)null);
						flag = true;
					}
					else
					{
						int num = 0;
						int num2 = 1;
						while (!flag)
						{
							text4 = Strings.Mid(A_0, num + 1, 128);
							val.Add((object)text4, num2.ToString(), (object)null, (object)null);
							num += text4.Length;
							num2++;
							if (num >= A_0.Length)
							{
								flag = true;
							}
						}
					}
					int count = val.Count;
					if (count > 0 && count <= 255)
					{
						int num = 1;
						int num3 = count;
						num = 1;
						while (true)
						{
							if (num <= num3)
							{
								aw();
								int num4 = 0;
								text4 = val[num.ToString()].ToString();
								text3 = a(text4, A_1, cv, a_3, num, count, A_2, A_3, a_, a_2);
								long num5 = text3.Length;
								if (num5 <= 0)
								{
									ch = "Error generating smart message PDU";
									ci = 4025L;
									break;
								}
								while (num4 <= b0)
								{
									aw();
									if (!s().IsOpen)
									{
										v();
									}
									bool flag2;
									DateTime now;
									if (!s().IsOpen || this.z != 0)
									{
										ch = "Cannot connect to modem for sending smart message";
										ci = 4026L;
									}
									else
									{
										if (num == 1 && num4 == 0)
										{
											while ((double)DateAndTime.DateDiff((DateInterval)9, cx, DateAndTime.Now, (FirstDayOfWeek)1, (FirstWeekOfYear)1) < (double)bz / 1000.0)
											{
												Thread.Sleep(10);
											}
										}
										else
										{
											Thread.Sleep(bz);
										}
										flag2 = false;
										text = "AT+CMGS=" + (double)(num5 - bs.Length) / 2.0 + "\r";
										try
										{
											c4 = true;
											c5 = true;
											if (Monitor.TryEnter(this.m_l, a4))
											{
												try
												{
													this.m_k.a = "";
												}
												catch (Exception ex)
												{
													ProjectData.SetProjectError(ex);
													Exception ex2 = ex;
													ProjectData.ClearProjectError();
												}
												finally
												{
													Monitor.Exit(this.m_l);
												}
												s().Write(text);
												now = DateTime.Now;
												this.a.b("Modem Command: " + text);
												goto IL_0443;
											}
											ch = "Thread monitor enter attempt timeout [GSM:SendSmartMessage:A]";
											ci = 9011L;
											ak();
										}
										catch (InvalidOperationException ex3)
										{
											ProjectData.SetProjectError((Exception)ex3);
											InvalidOperationException ex4 = ex3;
											ch = "Send smart message command error (Port not open). " + ex4.Message;
											ci = 4027L;
											ak();
											ProjectData.ClearProjectError();
										}
										catch (TimeoutException ex5)
										{
											ProjectData.SetProjectError((Exception)ex5);
											TimeoutException ex6 = ex5;
											ch = "Send smart message command error (Timeout). " + ex6.Message;
											ci = 4028L;
											ak();
											ProjectData.ClearProjectError();
										}
									}
									goto IL_0800;
									IL_0800:
									c4 = false;
									c5 = false;
									c6 = false;
									num4++;
									if (ci == 0)
									{
										ci = 4033L;
										ch = "Unknown error while sending smart message";
									}
									if (num4 <= b0)
									{
										a(ci, ch + " [Send Attempt: " + num4 + "]", "VCALENDAR");
										ci = 0L;
										ch = "";
									}
									cx = DateAndTime.Now;
									continue;
									IL_0443:
									text2 = "";
									while (!flag2 && (double)DateAndTime.DateDiff((DateInterval)9, now, DateAndTime.Now, (FirstDayOfWeek)1, (FirstWeekOfYear)1) < (double)this.av / 1000.0)
									{
										Thread.Sleep(5);
										text2 += p();
										if (text2.Length > 0 && text2.ToUpper().Contains(">") && !text2.ToUpper().Contains("ERROR"))
										{
											flag2 = true;
											break;
										}
										if (text2.ToUpper().Contains("ERROR"))
										{
											break;
										}
										Application.DoEvents();
									}
									c5 = false;
									c4 = false;
									if (!flag2)
									{
										this.a.b("Modem Response: " + text2);
										ch = "Invalid response to smart message command";
										ci = 4029L;
										ak();
									}
									else
									{
										try
										{
											c4 = true;
											c6 = true;
											s().Write(text3 + "\u001a");
											now = DateAndTime.Now;
											this.a.b("PDU: " + text3);
										}
										catch (InvalidOperationException ex7)
										{
											ProjectData.SetProjectError((Exception)ex7);
											InvalidOperationException ex8 = ex7;
											ch = "Submit smart message PDU error (Port not open). " + ex8.Message;
											ci = 4030L;
											ak();
											ProjectData.ClearProjectError();
											goto IL_0800;
										}
										catch (TimeoutException ex9)
										{
											ProjectData.SetProjectError((Exception)ex9);
											TimeoutException ex10 = ex9;
											ch = "Submit smart message PDU error (Timeout). " + ex10.Message;
											ci = 4031L;
											ak();
											ProjectData.ClearProjectError();
											goto IL_0800;
										}
										bool flag3 = false;
										Thread.Sleep(1000);
										text2 = "";
										while (!flag3 && (double)DateAndTime.DateDiff((DateInterval)9, now, DateAndTime.Now, (FirstDayOfWeek)1, (FirstWeekOfYear)1) < (double)this.av / 1000.0)
										{
											text2 += p();
											if (text2.Length > 0 && string.Compare(text2, text3 + "\u001a", ignoreCase: true) != 0 && !text2.ToUpper().Contains("ERROR"))
											{
												try
												{
													text6 = text2.Replace(text3 + "\u001a", "");
													if (text6.Contains("\r\nOK"))
													{
														text6 = Strings.Mid(text6, 1, Strings.InStr(text6, "\r\nOK", (CompareMethod)1) + 3);
													}
													text6 = a(text6.Replace("+CMGS:", ""), A_1: true, A_2: true, A_3: true).Trim();
													if (text6.Contains(","))
													{
														string[] array = text6.Split(",".ToCharArray());
														if (array.GetUpperBound(0) >= 1)
														{
															text6 = array[0];
														}
													}
													if (Versioned.IsNumeric((object)text6))
													{
														flag3 = true;
														break;
													}
												}
												catch (Exception ex11)
												{
													ProjectData.SetProjectError(ex11);
													Exception ex12 = ex11;
													ProjectData.ClearProjectError();
												}
											}
											if (text2.ToUpper().Contains("ERROR"))
											{
												break;
											}
											Application.DoEvents();
											Thread.Sleep(25);
										}
										c6 = false;
										c4 = false;
										text2 = text2.Replace(text3 + "\u001a", "");
										this.a.b("Modem Response: " + text2);
										if (flag3)
										{
											cx = DateAndTime.Now;
											text7 = ((text7.Length > 0) ? (text7 + "," + text6) : text6);
											break;
										}
										ch = "Invalid response to submit smart message PDU [Response: " + text2 + "]";
										ci = 4032L;
										if (text2.Trim().Length <= 0 || text2.Contains("ERROR"))
										{
											ak();
										}
									}
									goto IL_0800;
								}
								if (num4 <= b0 && ci <= 0)
								{
									num++;
									continue;
								}
							}
							if (ci > 0)
							{
								this.a.b("Smart message sending failed");
								result = "";
							}
							else
							{
								this.a.b("Smart message sent [Msg ID: " + text7 + "]");
								result = text7;
							}
							goto IL_096d;
						}
					}
				}
				catch (Exception ex13)
				{
					ProjectData.SetProjectError(ex13);
					Exception ex14 = ex13;
					c4 = false;
					c5 = false;
					c6 = false;
					ci = 4034L;
					ch = "System error (" + ex14.Message + ")";
					this.a.b("Smart message sending failed");
					result = "";
					ProjectData.ClearProjectError();
					goto IL_096d;
				}
			}
			goto IL_0982;
		}
		IL_096d:
		c4 = false;
		c5 = false;
		c6 = false;
		goto IL_0982;
		IL_0982:
		return result;
	}

	private string a(string A_0, string A_1, int A_2, int A_3, int A_4, int A_5, int A_6, bool A_7, string A_8, string A_9)
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		string text4 = "";
		string text5 = "";
		string text6 = "";
		string text7 = "";
		string text8 = "";
		string text9 = "";
		bool flag = A_7;
		string result = "";
		checked
		{
			try
			{
				if (bs.Length != 0)
				{
					goto IL_00a4;
				}
				aq();
				if (bq.Length > 0)
				{
					if (bs.Length == 0)
					{
						bs = "00";
					}
					goto IL_00a4;
				}
				a(4035L, "SMSC not specified", "VCALENDAR");
				result = "";
				goto end_IL_0046;
				IL_00a4:
				text = bs;
				if (flag && A_5 > 1 && A_4 < A_5)
				{
					flag = false;
				}
				text2 = ((!flag) ? "51" : "71");
				text3 = "00";
				if (j(A_1))
				{
					A_1 = A_1.Substring(1, A_1.Length - 1);
					text5 = "91";
					goto IL_0115;
				}
				if (f(A_1))
				{
					text5 = "81";
					goto IL_0115;
				}
				result = "";
				goto end_IL_0046;
				IL_0115:
				text6 = b(A_1.Length, 2) + text5 + b(A_1);
				string text10 = "00";
				text4 = "F5";
				text7 = b(A_6, 2);
				text8 = ((A_5 <= 1) ? ("060504" + A_9 + A_8 + q(A_0)) : ("0B0504" + A_9 + A_8 + "0003" + b(A_3, 2) + b(A_5, 2) + b(A_4, 2) + q(A_0)));
				text9 = b((int)Math.Round((double)text8.Length / 2.0), 2);
				if (m(text9) > 140)
				{
					a(4036L, "Smart message data length overflow", "VCALENDAR");
					result = "";
				}
				else
				{
					result = text + text2 + text3 + text6 + text10 + text4 + text7 + text9 + text8;
				}
				end_IL_0046:;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				this.a.b("[GSM:GetVCalendarPDU:1] " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			return result;
		}
	}

	internal string q(string A_0)
	{
		string text = "";
		string text2 = "";
		byte[] bytes = System.Text.Encoding.UTF8.GetBytes(A_0);
		checked
		{
			int num = bytes.Length - 1;
			for (int num2 = 0; num2 <= num; num2++)
			{
				text += b(bytes[num2], 2);
			}
			return text;
		}
	}

	internal void ap()
	{
		c0.Clear();
		if (cz.Count <= 0)
		{
			return;
		}
		string text = "";
		string text2 = "";
		string a_ = "";
		string text3 = "";
		string text4 = "";
		string text5 = "";
		int a_2 = 0;
		checked
		{
			try
			{
				if (!this.af)
				{
					this.b.a();
				}
				if (!b1)
				{
					int count = cz.Count;
					for (int num = 1; num <= count; num++)
					{
						i i2 = (i)cz[num];
						text4 = i2.a.ToString();
						if (Operators.CompareString(this.ah, ck, false) == 0 && num > b2)
						{
							this.c.a(num, i2.c, i2.h, this.ad, i2.e, i2.f, i2.g);
						}
						else
						{
							this.c.a(num, i2.c, i2.h, i2.d, i2.e, i2.f, i2.g);
						}
						((i)cz[text4]).b = num;
					}
					return;
				}
				int num2 = 1;
				int count2 = cz.Count;
				DateTime a_3 = default(DateTime);
				for (int num = 1; num <= count2; num++)
				{
					i i2 = (i)cz[num];
					text4 = i2.a.ToString();
					int num3 = i2.j;
					if (num3 == 1)
					{
						if (Operators.CompareString(this.ah, ck, false) == 0 && num2 > b2)
						{
							this.c.a(num2, i2.c, i2.h, this.ad, i2.e, i2.f, i2.g);
						}
						else
						{
							this.c.a(num2, i2.c, i2.h, i2.d, i2.e, i2.f, i2.g);
						}
						((i)cz[text4]).b = num2;
						num2++;
					}
					else
					{
						if (i2.k != 1)
						{
							continue;
						}
						bool flag = false;
						text3 = i2.d;
						text = i2.c;
						text2 = i2.h;
						int num4 = i2.i;
						for (int num5 = 2; num5 <= num3; num5++)
						{
							if (flag)
							{
								break;
							}
							bool flag2 = false;
							for (int num6 = 1; num6 <= cz.Count; num6++)
							{
								if (flag2)
								{
									break;
								}
								i2 = (i)cz[num6];
								text4 = i2.a.ToString();
								if (Operators.CompareString(i2.c, text, false) == 0 && Operators.CompareString(i2.h, text2, false) == 0 && i2.i == num4 && i2.k == num5 && i2.j == num3)
								{
									text3 += i2.d;
									if (num5 == num3)
									{
										a_3 = i2.e;
										a_ = i2.f;
										a_2 = i2.g;
									}
									flag2 = true;
								}
							}
							if (!flag2)
							{
								flag = true;
							}
						}
						if (flag)
						{
							continue;
						}
						if (Operators.CompareString(this.ah, ck, false) == 0 && num2 > b2)
						{
							this.c.a(num2, text, text2, this.ad, a_3, a_, a_2);
						}
						else
						{
							this.c.a(num2, text, text2, text3, a_3, a_, a_2);
						}
						bool flag3 = false;
						int num7 = 1;
						int num8 = 0;
						for (; num7 <= cz.Count; num7++)
						{
							if (flag3)
							{
								break;
							}
							i2 = (i)cz[num7];
							text5 = i2.a.ToString();
							if (Operators.CompareString(i2.c, text, false) == 0 && Operators.CompareString(i2.h, text2, false) == 0 && i2.i == num4 && i2.j == num3)
							{
								((i)cz[text5]).b = num2;
								num8++;
								if (num8 >= num3)
								{
									flag3 = true;
								}
							}
						}
						num2++;
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				this.a.b("[GSM:ProcessIncomingMessages:1] " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}
	}

	internal bool h(string A_0)
	{
		string text = "";
		bool result;
		try
		{
			result = false;
			int length = A_0.Length;
			if (length == 0)
			{
				result = true;
			}
			else if (length > 0)
			{
				int num = length;
				int num2 = 1;
				while (true)
				{
					if (num2 <= num)
					{
						text = Strings.Mid(A_0, num2, 1);
						switch (text)
						{
						default:
							if (LikeOperator.LikeString(text, "[0-9A-Za-z.:/()#&'+, ]", (CompareMethod)0))
							{
								break;
							}
							goto end_IL_007e;
						case " ":
						case "-":
						case "\r":
						case "\n":
							break;
						}
						num2 = checked(num2 + 1);
						continue;
					}
					result = true;
					break;
					continue;
					end_IL_007e:
					break;
				}
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:IsAlphaNumeric:1] " + ex2.Message);
			result = false;
			ProjectData.ClearProjectError();
		}
		return result;
	}

	internal bool d(string A_0)
	{
		bool result;
		try
		{
			result = checked(A_0.Length > 5 && Strings.InStr(A_0, "@", (CompareMethod)0) > 1 && Strings.InStr(A_0, "@", (CompareMethod)0) <= A_0.Length - 4 && Strings.InStr(A_0, "@", (CompareMethod)0) == Strings.InStrRev(A_0, "@", -1, (CompareMethod)0) && Strings.InStrRev(A_0, ".", -1, (CompareMethod)0) >= 4 && Strings.InStrRev(A_0, ".", -1, (CompareMethod)0) >= Strings.InStr(A_0, "@", (CompareMethod)0) && Strings.InStrRev(A_0, ".", -1, (CompareMethod)0) <= A_0.Length - 2) && ((Strings.InStr(A_0, " ", (CompareMethod)0) < 1 && Strings.InStr(A_0, ";", (CompareMethod)0) < 1 && Strings.InStr(A_0, ",", (CompareMethod)0) < 1 && Strings.InStr(A_0, "&", (CompareMethod)0) < 1 && Strings.InStr(A_0, ":", (CompareMethod)0) < 1) ? true : false);
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.a.b("[GSM:IsEmail:1] " + ex2.Message);
			result = false;
			ProjectData.ClearProjectError();
		}
		return result;
	}

	internal string k(string A_0)
	{
		int num = 76;
		string text = "";
		int num2 = 1;
		int num3 = 1;
		string text2 = "";
		string text3 = "";
		checked
		{
			string result;
			try
			{
				A_0 = A_0.Replace("=", "=3D");
				A_0 = A_0.Replace("\r", "=0D");
				A_0 = A_0.Replace("\n", "=0A");
				int length = A_0.Length;
				while (num2 <= length)
				{
					text2 = Strings.Mid(A_0, num2, 1);
					if (Operators.CompareString(text2, "=", false) == 0 || Operators.CompareString(text2, " ", false) == 0)
					{
						int num4;
						int num5;
						bool flag;
						if (Operators.CompareString(text2, "=", false) == 0)
						{
							num4 = num3 + 3;
							num5 = num2 + 3;
							flag = false;
							while (num4 <= num && num5 <= length && !flag)
							{
								text3 = Strings.Mid(A_0, num5, 1);
								if (Operators.CompareString(text3, "=", false) != 0 && Operators.CompareString(text3, " ", false) != 0)
								{
									flag = true;
								}
								if (!flag)
								{
									if (Operators.CompareString(text3, "=", false) == 0)
									{
										num5 += 3;
										num4 += 3;
									}
									else
									{
										num5++;
										num4++;
									}
								}
							}
							if (flag)
							{
								int num6 = num5 - num2 + 1;
								text += Strings.Mid(A_0, num2, num6);
								num2 = num5 + 1;
								num3 = num4 + 1;
							}
							else if (num5 <= length)
							{
								text += "=\r\n";
								num3 = 1;
							}
							else
							{
								int num6 = num5 - num2 + 1;
								text += Strings.Mid(A_0, num2, num6);
								num2 = num5 + 1;
								num3 = num4 + 1;
							}
							continue;
						}
						num4 = num3 + 1;
						num5 = num2 + 1;
						flag = false;
						while (num4 <= num && num5 <= length && !flag)
						{
							text3 = Strings.Mid(A_0, num5, 1);
							if (Operators.CompareString(text3, "=", false) != 0 && Operators.CompareString(text3, " ", false) != 0)
							{
								flag = true;
							}
							if (!flag)
							{
								if (Operators.CompareString(text3, "=", false) == 0)
								{
									num5 += 3;
									num4 += 3;
								}
								else
								{
									num5++;
									num4++;
								}
							}
						}
						if (flag)
						{
							int num6 = num5 - num2 + 1;
							text += Strings.Mid(A_0, num2, num6);
							num2 = num5 + 1;
							num3 = num4 + 1;
						}
						else
						{
							text += "=\r\n";
							num3 = 1;
						}
					}
					else if (num3 < num)
					{
						text += text2;
						num3++;
						num2++;
					}
					else
					{
						text = text + text2 + "=\r\n";
						num3 = 1;
						num2++;
					}
				}
				result = text;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				this.a.b("[GSM:QuotedPrintable:1] " + ex2.Message);
				result = "";
				ProjectData.ClearProjectError();
			}
			return result;
		}
	}

	internal string a(DateTime A_0)
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		string text4 = "";
		string text5 = "";
		string text6 = "";
		long num = 0L;
		long num2 = 0L;
		long num3 = 0L;
		string text7 = "+";
		string text8 = "";
		string text9 = "";
		checked
		{
			string result;
			try
			{
				num = (long)Math.Round(TimeZone.CurrentTimeZone.GetUtcOffset(DateAndTime.Now).TotalMinutes);
				num3 = unchecked(num % 60);
				num2 = (long)Math.Round((double)(num - num3) / 60.0);
				if (num < 0)
				{
					text7 = "-";
					num3 = -1 * num3;
					num2 = -1 * num2;
				}
				text8 = num2.ToString();
				text9 = num3.ToString();
				if (text8.Length < 2)
				{
					text8 = "0" + text8;
				}
				if (text9.Length < 2)
				{
					text9 = "0" + text9;
				}
				text = DateAndTime.Year(A_0).ToString();
				text2 = DateAndTime.Month(A_0).ToString();
				if (text2.Length < 2)
				{
					text2 = "0" + text2;
				}
				text3 = DateAndTime.Day(A_0).ToString();
				if (text3.Length < 2)
				{
					text3 = "0" + text3;
				}
				text4 = DateAndTime.Hour(A_0).ToString();
				if (text4.Length < 2)
				{
					text4 = "0" + text4;
				}
				text5 = DateAndTime.Minute(A_0).ToString();
				if (text5.Length < 2)
				{
					text5 = "0" + text5;
				}
				text6 = DateAndTime.Second(A_0).ToString();
				if (text6.Length < 2)
				{
					text6 = "0" + text6;
				}
				result = text + text2 + text3 + "T" + text4 + text5 + text6 + text7 + text8 + text9;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				this.a.b("[GSM:ToISO8601:1] " + ex2.Message);
				result = "";
				ProjectData.ClearProjectError();
			}
			return result;
		}
	}

	private void j(object A_0, bool A_1)
	{
		try
		{
			if (this.m_k.f.Count > 0)
			{
				n(ct + "\r");
				if (string.Compare(ct, "ATH", ignoreCase: true) != 0)
				{
					a("ATH\r");
				}
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
			((g)A_0).a.Set();
			dz.Set();
		}
	}

	private void i(object A_0, bool A_1)
	{
		c c2 = (c)A_0;
		try
		{
			bool flag = true;
			if (string.Compare(c2.c, c2.d, ignoreCase: true) != 0)
			{
				flag = false;
				i(c2.c);
				Thread.Sleep(50);
			}
			else if (!a7)
			{
				i(c2.c);
				Thread.Sleep(50);
			}
			c2.b = a("AT+CMGR=" + c2.a + "\r").ToUpper();
			if (this.x)
			{
				string text = "";
				int num = 1;
				do
				{
					Thread.Sleep(100);
					text = a("AT+CMGD=" + c2.a + "\r");
					if (text.Length > 0 && text.ToUpper().Contains("OK") && !text.ToUpper().Contains("ERROR"))
					{
						break;
					}
					Thread.Sleep(900);
					num = checked(num + 1);
				}
				while (num <= 5);
			}
			if (!flag)
			{
				Thread.Sleep(50);
				i(c2.d);
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
			c2.e.Set();
			dz.Set();
		}
	}

	private void h(object A_0, bool A_1)
	{
		d d2 = (d)A_0;
		try
		{
			a("AT+CNMA=0\r");
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			ProjectData.ClearProjectError();
		}
		finally
		{
			d2.a.Set();
			dz.Set();
		}
	}

	private void d(object A_0)
	{
		try
		{
			this.m_i?.Invoke((NewIncomingCallEventArgs)A_0);
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			ProjectData.ClearProjectError();
		}
	}

	private void c(object A_0)
	{
		try
		{
			this.m_g?.Invoke((NewMessageReceivedEventArgs)A_0);
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			ProjectData.ClearProjectError();
		}
	}

	private void b(object A_0)
	{
		try
		{
			this.m_h?.Invoke((NewDeliveryReportEventArgs)A_0);
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			ProjectData.ClearProjectError();
		}
	}

	private void a(object A_0)
	{
		try
		{
			this.m_j?.Invoke((NewUSSDReceivedEventArgs)A_0);
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			ProjectData.ClearProjectError();
		}
	}

	private void g(object A_0, bool A_1)
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		try
		{
			if (this.m_k.f.Count <= 0)
			{
				return;
			}
			g g2 = new g();
			ThreadPool.RegisterWaitForSingleObject(dz, j, g2, -1, executeOnlyOnce: true);
			g2.a.WaitOne();
			if (g2 != null)
			{
				g2 = null;
			}
			Thread.Sleep(1000);
			while (this.m_k.f.Count > 0)
			{
				if (Monitor.TryEnter(this.m_l, a4))
				{
					try
					{
						if (this.m_k.f.Count <= 0)
						{
							break;
						}
						text2 = this.m_k.f[1].ToString();
						this.m_k.f.Remove(1);
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						ProjectData.ClearProjectError();
					}
					finally
					{
						Monitor.Exit(this.m_l);
					}
					this.a.b("Recv: " + text2);
					try
					{
						if (!text2.Contains(","))
						{
							throw new Exception();
						}
						string[] array = Strings.Split(text2, ",", -1, (CompareMethod)0);
						text3 = array[0].Replace("+CLIP:", "").Trim();
						if (this.m_k.f.Count >= 1)
						{
							if (!Monitor.TryEnter(this.m_l, a4))
							{
								a(9011L, "Thread monitor enter attempt timeout [GSM:NewIncomingCallCLIP:D]", "GENERAL");
								aw();
								break;
							}
							try
							{
								for (int num = this.m_k.f.Count; num >= 1; num = checked(num + -1))
								{
									if (this.m_k.f[num].ToString().Contains(text3))
									{
										this.m_k.f.Remove(num);
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
								Monitor.Exit(this.m_l);
							}
						}
						NewIncomingCallEventArgs state = new NewIncomingCallEventArgs(text3.Replace("\"", ""));
						ThreadPool.QueueUserWorkItem(d, state);
					}
					catch (Exception ex5)
					{
						ProjectData.SetProjectError(ex5);
						Exception ex6 = ex5;
						this.a.b("[GSM:NewIncomingCallCLIP:1] " + ex6.Message);
						ProjectData.ClearProjectError();
					}
					continue;
				}
				a(9011L, "Thread monitor enter attempt timeout [GSM:NewIncomingCallCLIP:C]", "GENERAL");
				aw();
				break;
			}
		}
		catch (Exception ex7)
		{
			ProjectData.SetProjectError(ex7);
			Exception ex8 = ex7;
			ProjectData.ClearProjectError();
		}
		finally
		{
			d0.Set();
		}
	}

	private void f(object A_0, bool A_1)
	{
		char[] trimChars = "\r\n".ToCharArray();
		string text = "";
		string text2 = "";
		string text3 = "";
		bool flag = true;
		string text4 = "";
		string text5 = "";
		string text6 = "";
		string a_ = a8;
		int num = 0;
		string text7 = "";
		string text8 = "";
		DateTime dateTime = default(DateTime);
		string text9 = "";
		int num2 = 0;
		string text10 = "";
		string text11 = "";
		string text12 = "";
		string text13 = "";
		string text14 = "";
		string text15 = "";
		int num3 = 0;
		int num4 = 0;
		string text16 = "";
		string text17 = "";
		string text18 = "";
		string text19 = "";
		string text20 = "";
		string text21 = "";
		string text22 = "";
		string text23 = "";
		int num5 = 7;
		string text24 = "";
		string text25 = "";
		string text26 = "";
		string text27 = "";
		string text28 = "";
		string text29 = "";
		string text30 = "";
		string text31 = "";
		DateTime dateTime2 = default(DateTime);
		int num6 = 0;
		int num7 = 1;
		int num8 = 1;
		int num9 = 0;
		string text32 = "";
		int A_2 = 0;
		long num10 = 0L;
		string text33 = "";
		string text34 = "";
		string text35 = "";
		string text36 = "";
		int num11 = 0;
		int num12 = 0;
		bool flag2 = false;
		string text37 = "";
		string text38 = "";
		int num13 = 0;
		checked
		{
			try
			{
				NewMessageReceivedEventArgs newMessageReceivedEventArgs = default(NewMessageReceivedEventArgs);
				while (this.m_k.b.Count > 0)
				{
					if (Monitor.TryEnter(this.m_l, a4))
					{
						try
						{
							if (this.m_k.b.Count <= 0)
							{
								break;
							}
							text38 = this.m_k.b[1].ToString();
							this.m_k.b.Remove(1);
						}
						catch (Exception ex)
						{
							ProjectData.SetProjectError(ex);
							Exception ex2 = ex;
							ProjectData.ClearProjectError();
						}
						finally
						{
							Monitor.Exit(this.m_l);
						}
						this.a.b("Recv: " + text38);
						try
						{
							text38 = text38.Trim(trimChars).ToUpper().Trim()
								.Replace("+CMTI:", "")
								.Trim();
							if (text38.Length <= 0 || !text38.Contains(","))
							{
								throw new Exception();
							}
							string[] array = text38.Split(",".ToCharArray());
							if (array.GetUpperBound(0) < 1)
							{
								throw new Exception();
							}
							text2 = array[0].Replace("\"", "").Trim();
							text3 = array[1].Trim();
							c c2 = new c(text3, text2, a_);
							ThreadPool.RegisterWaitForSingleObject(dz, i, c2, -1, executeOnlyOnce: true);
							c2.e.WaitOne();
							text5 = c2.b;
							if (c2 != null)
							{
								c2 = null;
							}
							if (text5.Trim(trimChars).Trim().Length <= 0 || text5.Contains("ERROR"))
							{
								throw new Exception();
							}
							text5 = a(text5, A_1: true, A_2: false, A_3: false).Trim(trimChars).Trim();
							if (!text5.Contains("\r\n"))
							{
								throw new Exception();
							}
							string[] array2 = Strings.Split(text5, "\r\n", -1, (CompareMethod)0);
							if (array2.GetUpperBound(0) < 1)
							{
								throw new Exception();
							}
							text = array2[1].ToUpper();
							num2 = (int)(m(Strings.Left(text, 2)) * 2);
							text = Strings.Right(text, text.Length - 2);
							if (num2 > 0)
							{
								text10 = Strings.Left(text, 2);
								text = Strings.Right(text, text.Length - 2);
								text11 = b(Strings.Left(text, num2 - 2));
								if (string.Compare(Strings.Right(text11, 1), "F", ignoreCase: true) == 0)
								{
									text11 = Strings.Left(text11, text11.Length - 1);
								}
								if (string.Compare(text10, "91", ignoreCase: true) == 0)
								{
									text11 = "+" + text11;
								}
								text = Strings.Right(text, text.Length - num2 + 2);
							}
							text12 = Strings.Left(text, 2);
							text = Strings.Right(text, text.Length - 2);
							text13 = p(text12);
							text14 = Strings.Mid(text13, 2, 1);
							text15 = Strings.Mid(text13, 7, 2);
							if (Operators.CompareString(text15, "00", false) != 0)
							{
								text = array2[1].ToUpper();
								text12 = Strings.Left(text, 2);
								text = Strings.Right(text, text.Length - 2);
								text13 = p(text12);
								text14 = Strings.Mid(text13, 2, 1);
								text15 = Strings.Mid(text13, 7, 2);
								if (Operators.CompareString(text15, "00", false) != 0)
								{
									throw new Exception("Invalid SMS Deliver PDU [MTI <> '00']");
								}
							}
							num3 = (int)m(Strings.Left(text, 2));
							if (unchecked(num3 % 2) != 0)
							{
								num3++;
							}
							num4 = num3;
							text = Strings.Right(text, text.Length - 2);
							text16 = Strings.Left(text, 2);
							text = Strings.Right(text, text.Length - 2);
							text17 = Strings.Left(text, num3);
							if (Operators.CompareString(Strings.Mid(text16, 1, 1), "D", false) == 0)
							{
								text17 = a(text17, 7, "0", ref num4);
							}
							else
							{
								text17 = b(text17);
								if (string.Compare(Strings.Right(text17, 1), "F", ignoreCase: true) == 0)
								{
									text17 = Strings.Left(text17, text17.Length - 1);
								}
							}
							if (string.Compare(text16, "91", ignoreCase: true) == 0)
							{
								text17 = "+" + text17;
							}
							text = Strings.Right(text, text.Length - num3);
							text18 = Strings.Left(text, 2);
							text = Strings.Right(text, text.Length - 2);
							text19 = Strings.Left(text, 2);
							text = Strings.Right(text, text.Length - 2);
							text20 = p(text19);
							text21 = Strings.Mid(text20, 1, 2);
							text22 = Strings.Mid(text20, 3, 2);
							text23 = Strings.Mid(text20, 5, 2);
							if (Operators.CompareString(text21, "00", false) == 0 || Operators.CompareString(text21 + text22, "1111", false) == 0)
							{
								if (Operators.CompareString(text21, "00", false) == 0)
								{
									if (Operators.CompareString(Strings.Mid(text22, 1, 1), "1", false) == 0)
									{
										throw new Exception("Invalid SMS Deliver PDU [DCS76 = '00' and DCS5 = '1']");
									}
									switch (text23)
									{
									case "00":
										num5 = 7;
										break;
									case "01":
										num5 = 8;
										break;
									case "10":
										num5 = 16;
										break;
									case "11":
										throw new Exception();
									}
								}
								if (Operators.CompareString(text21, "11", false) == 0)
								{
									if (Operators.CompareString(Strings.Mid(text23, 1, 1), "1", false) == 0)
									{
										throw new Exception("Invalid SMS Deliver PDU [DCS76 = '11' and DCS3 = '1']");
									}
									string text39 = Strings.Mid(text23, 2, 1);
									if (Operators.CompareString(text39, "0", false) == 0)
									{
										num5 = 7;
									}
									else if (Operators.CompareString(text39, "1", false) == 0)
									{
										num5 = 8;
									}
								}
								if (num5 != 7 && num5 != 8 && num5 != 16)
								{
									throw new Exception();
								}
								text24 = b(Strings.Left(text, 14));
								text25 = Strings.Mid(text24, 1, 2);
								text26 = Strings.Mid(text24, 3, 2);
								text27 = Strings.Mid(text24, 5, 2);
								text28 = Strings.Mid(text24, 7, 2);
								text29 = Strings.Mid(text24, 9, 2);
								text30 = Strings.Mid(text24, 11, 2);
								text31 = Strings.Mid(text24, 13, 2);
								dateTime2 = DateAndTime.DateSerial(Conversions.ToInteger("20" + text25), Conversions.ToInteger(text26), Conversions.ToInteger(text27));
								dateTime2 = DateAndTime.DateAdd((DateInterval)9, (double)(Conversions.ToLong(text28) * 3600 + Conversions.ToLong(text29) * 60 + Conversions.ToLong(text30)), dateTime2);
								if (Operators.CompareString(Strings.Mid(p(text31), 1, 1), "0", false) == 0)
								{
									try
									{
										num10 = Conversions.ToLong(text31) * 15;
									}
									catch (Exception ex3)
									{
										ProjectData.SetProjectError(ex3);
										Exception ex4 = ex3;
										num10 = 0L;
										ProjectData.ClearProjectError();
									}
									text33 = "+";
									num13 = (int)num10;
								}
								else
								{
									try
									{
										num10 = Conversions.ToLong(a("0" + Strings.Mid(p(text31), 2, 7), 2)) * 15;
									}
									catch (Exception ex5)
									{
										ProjectData.SetProjectError(ex5);
										Exception ex6 = ex5;
										num10 = 0L;
										ProjectData.ClearProjectError();
									}
									text33 = "-";
									num13 = -(int)num10;
								}
								text34 = Conversion.Int((double)num10 / 60.0).ToString();
								text35 = unchecked(num10 % 60).ToString();
								if (text34.Length < 2)
								{
									text34 = "0" + text34;
								}
								if (text35.Length < 2)
								{
									text35 = "0" + text35;
								}
								text33 = ((Operators.CompareString(text34, "00", false) != 0 || Operators.CompareString(text35, "00", false) != 0) ? (text33 + text34 + ":" + text35) : "GMT");
								text36 = DateAndTime.WeekdayName(DateAndTime.Weekday(dateTime2, (FirstDayOfWeek)1), true, (FirstDayOfWeek)0) + ", " + text27 + " " + DateAndTime.MonthName(Conversions.ToInteger(text26), true) + " 20" + text25 + " " + text28 + ":" + text29 + ":" + text30 + " " + text33;
								text = Strings.Right(text, text.Length - 14);
								num6 = (int)m(Strings.Left(text, 2));
								text = Strings.Right(text, text.Length - 2);
								if (Operators.CompareString(text14, "1", false) == 0)
								{
									text32 = Strings.Mid(text, 1, 12);
									if (Operators.CompareString(Strings.Mid(text32, 1, 6), "050003", false) != 0)
									{
										throw new Exception();
									}
									text = Strings.Mid(text, 13, text.Length - 12);
									num9 = (int)m(Strings.Mid(text32, 7, 2));
									num7 = (int)m(Strings.Mid(text32, 9, 2));
									num8 = (int)m(Strings.Mid(text32, 11, 2));
									switch (num5)
									{
									case 7:
										A_2 = num6 - 7;
										break;
									case 8:
									case 16:
										A_2 = num6 - 6;
										break;
									}
								}
								else
								{
									A_2 = num6;
								}
								if (num5 == 16)
								{
									A_2 = (int)Math.Round((double)A_2 / 2.0);
								}
								text4 = a(text, num5, text14, ref A_2);
								if (text4.Length > A_2)
								{
									text4 = Strings.Left(text4, A_2);
								}
								if (num7 > 1 && this.t)
								{
									if (!Monitor.TryEnter(this.c2, a4))
									{
										a(9011L, "Thread monitor enter attempt timeout [GSM:NewMsgCMTI:E]", "GENERAL");
										aw();
										break;
									}
									try
									{
										k k2 = new k(Conversions.ToInteger(text3), 0, text17, text4, dateTime2, text36, num13, text11, num9, num7, num8);
										c1.Add((object)k2, (string)null, (object)null, (object)null);
										text37 = "";
										int num14 = num7;
										for (num11 = 1; num11 <= num14; num11++)
										{
											flag2 = false;
											int count = c1.Count;
											for (num12 = 1; num12 <= count; num12++)
											{
												k k3 = (k)c1[num12];
												if (k3.i == num9 && k3.k == num11 && k3.j == num7 && Operators.CompareString(k3.c, text17, false) == 0 && Operators.CompareString(k3.h, text11, false) == 0)
												{
													text37 += k3.d;
													flag2 = true;
													break;
												}
											}
											if (!flag2)
											{
												break;
											}
										}
									}
									catch (Exception ex7)
									{
										ProjectData.SetProjectError(ex7);
										Exception ex8 = ex7;
										ProjectData.ClearProjectError();
									}
									finally
									{
										Monitor.Exit(this.c2);
									}
									if (num11 == num7 + 1 && flag2)
									{
										if (!Monitor.TryEnter(this.c2, a4))
										{
											a(9011L, "Thread monitor enter attempt timeout [GSM:NewMsgCMTI:F]", "GENERAL");
											aw();
											break;
										}
										try
										{
											int num15 = num7;
											for (num11 = 1; num11 <= num15; num11++)
											{
												for (num12 = c1.Count; num12 >= 1; num12 += -1)
												{
													k k3 = (k)c1[num12];
													if (k3.i == num9 && k3.k == num11 && k3.j == num7 && Operators.CompareString(k3.c, text17, false) == 0 && Operators.CompareString(k3.h, text11, false) == 0)
													{
														c1.Remove(num12);
													}
												}
											}
										}
										catch (Exception ex9)
										{
											ProjectData.SetProjectError(ex9);
											Exception ex10 = ex9;
											ProjectData.ClearProjectError();
										}
										finally
										{
											Monitor.Exit(this.c2);
										}
										newMessageReceivedEventArgs = new NewMessageReceivedEventArgs(text17, text37, text11, dateTime2, text36, num13, num7, num9, 0);
									}
								}
								else
								{
									newMessageReceivedEventArgs = new NewMessageReceivedEventArgs(text17, text4, text11, dateTime2, text36, num13, num7, num9, num8);
								}
								if (newMessageReceivedEventArgs != null)
								{
									ThreadPool.QueueUserWorkItem(c, newMessageReceivedEventArgs);
								}
								continue;
							}
							throw new Exception("Invalid SMS Deliver PDU [Incorrect DCS)");
						}
						catch (Exception ex11)
						{
							ProjectData.SetProjectError(ex11);
							Exception ex12 = ex11;
							this.a.b("[GSM:NewMsgCMTI:1] " + ex12.Message);
							ProjectData.ClearProjectError();
						}
						continue;
					}
					a(9011L, "Thread monitor enter attempt timeout [GSM:NewMsgCMTI:B]", "GENERAL");
					aw();
					break;
				}
			}
			catch (Exception ex13)
			{
				ProjectData.SetProjectError(ex13);
				Exception ex14 = ex13;
				ProjectData.ClearProjectError();
			}
			finally
			{
				if (Monitor.TryEnter(this.c2, a4))
				{
					try
					{
						if (c1.Count > 0)
						{
							for (num12 = c1.Count; num12 >= 1; num12 += -1)
							{
								k k3 = (k)c1[num12];
								if (DateAndTime.DateDiff((DateInterval)9, k3.l, DateTime.Now, (FirstDayOfWeek)1, (FirstWeekOfYear)1) > 900)
								{
									c1.Remove(num12);
								}
							}
						}
					}
					catch (Exception ex15)
					{
						ProjectData.SetProjectError(ex15);
						Exception ex16 = ex15;
						ProjectData.ClearProjectError();
					}
					finally
					{
						Monitor.Exit(this.c2);
					}
				}
				else
				{
					a(9011L, "Thread monitor enter attempt timeout [GSM:NewMsgCMTI:G]", "GENERAL");
					aw();
				}
				d2.Set();
			}
		}
	}

	private void e(object A_0, bool A_1)
	{
		char[] trimChars = "\r\n".ToCharArray();
		string text = "";
		string text2 = "";
		int num = 0;
		string text3 = "";
		string text4 = "";
		string text5 = "";
		string text6 = "";
		string text7 = "";
		string text8 = "";
		int num2 = 0;
		int num3 = 0;
		string text9 = "";
		string text10 = "";
		string text11 = "";
		string text12 = "";
		string text13 = "";
		string text14 = "";
		string text15 = "";
		string text16 = "";
		int num4 = 7;
		string text17 = "";
		string text18 = "";
		string text19 = "";
		string text20 = "";
		string text21 = "";
		string text22 = "";
		string text23 = "";
		string text24 = "";
		DateTime dateTime = default(DateTime);
		int num5 = 0;
		int num6 = 1;
		int num7 = 1;
		int num8 = 0;
		string text25 = "";
		int A_2 = 0;
		long num9 = 0L;
		string text26 = "";
		string text27 = "";
		string text28 = "";
		string text29 = "";
		int num10 = 0;
		int num11 = 0;
		bool flag = false;
		string text30 = "";
		string text31 = "";
		int num12 = 0;
		checked
		{
			try
			{
				NewMessageReceivedEventArgs newMessageReceivedEventArgs = default(NewMessageReceivedEventArgs);
				while (this.m_k.c.Count > 0)
				{
					if (cu)
					{
						d d2 = new d();
						ThreadPool.RegisterWaitForSingleObject(dz, h, d2, -1, executeOnlyOnce: true);
						d2.a.WaitOne();
						if (d2 != null)
						{
							d2 = null;
						}
					}
					if (Monitor.TryEnter(this.m_l, a4))
					{
						try
						{
							if (this.m_k.c.Count <= 0)
							{
								break;
							}
							text31 = this.m_k.c[1].ToString();
							this.m_k.c.Remove(1);
						}
						catch (Exception ex)
						{
							ProjectData.SetProjectError(ex);
							Exception ex2 = ex;
							ProjectData.ClearProjectError();
						}
						finally
						{
							Monitor.Exit(this.m_l);
						}
						this.a.b("Recv: " + text31);
						try
						{
							text31 = text31.Trim(trimChars).ToUpper().Trim()
								.Replace("+CMT:", "")
								.Trim();
							if (!text31.Contains("\r\n"))
							{
								throw new Exception();
							}
							string[] array = Strings.Split(text31, "\r\n", -1, (CompareMethod)0);
							if (array.GetUpperBound(0) < 1)
							{
								throw new Exception();
							}
							text = array[1].ToUpper();
							num = (int)(m(Strings.Left(text, 2)) * 2);
							text = Strings.Right(text, text.Length - 2);
							if (num > 0)
							{
								text3 = Strings.Left(text, 2);
								text = Strings.Right(text, text.Length - 2);
								text4 = b(Strings.Left(text, num - 2));
								if (string.Compare(Strings.Right(text4, 1), "F", ignoreCase: true) == 0)
								{
									text4 = Strings.Left(text4, text4.Length - 1);
								}
								if (string.Compare(text3, "91", ignoreCase: true) == 0)
								{
									text4 = "+" + text4;
								}
								text = Strings.Right(text, text.Length - num + 2);
							}
							text5 = Strings.Left(text, 2);
							text = Strings.Right(text, text.Length - 2);
							text6 = p(text5);
							text7 = Strings.Mid(text6, 2, 1);
							text8 = Strings.Mid(text6, 7, 2);
							if (Operators.CompareString(text8, "00", false) != 0)
							{
								text = array[1].ToUpper();
								text5 = Strings.Left(text, 2);
								text = Strings.Right(text, text.Length - 2);
								text6 = p(text5);
								text7 = Strings.Mid(text6, 2, 1);
								text8 = Strings.Mid(text6, 7, 2);
								if (Operators.CompareString(text8, "00", false) != 0)
								{
									throw new Exception("Invalid SMS Deliver PDU [MTI <> '00']");
								}
							}
							num2 = (int)m(Strings.Left(text, 2));
							if (unchecked(num2 % 2) != 0)
							{
								num2++;
							}
							num3 = num2;
							text = Strings.Right(text, text.Length - 2);
							text9 = Strings.Left(text, 2);
							text = Strings.Right(text, text.Length - 2);
							text10 = Strings.Left(text, num2);
							if (Operators.CompareString(Strings.Mid(text9, 1, 1), "D", false) == 0)
							{
								text10 = a(text10, 7, "0", ref num3);
							}
							else
							{
								text10 = b(text10);
								if (string.Compare(Strings.Right(text10, 1), "F", ignoreCase: true) == 0)
								{
									text10 = Strings.Left(text10, text10.Length - 1);
								}
							}
							if (string.Compare(text9, "91", ignoreCase: true) == 0)
							{
								text10 = "+" + text10;
							}
							text = Strings.Right(text, text.Length - num2);
							text11 = Strings.Left(text, 2);
							text = Strings.Right(text, text.Length - 2);
							text12 = Strings.Left(text, 2);
							text = Strings.Right(text, text.Length - 2);
							text13 = p(text12);
							text14 = Strings.Mid(text13, 1, 2);
							text15 = Strings.Mid(text13, 3, 2);
							text16 = Strings.Mid(text13, 5, 2);
							if (Operators.CompareString(text14, "00", false) == 0 || Operators.CompareString(text14 + text15, "1111", false) == 0)
							{
								if (Operators.CompareString(text14, "00", false) == 0)
								{
									if (Operators.CompareString(Strings.Mid(text15, 1, 1), "1", false) == 0)
									{
										throw new Exception("Invalid SMS Deliver PDU [DCS76 = '00' and DCS5 = '1']");
									}
									switch (text16)
									{
									case "00":
										num4 = 7;
										break;
									case "01":
										num4 = 8;
										break;
									case "10":
										num4 = 16;
										break;
									case "11":
										throw new Exception("Invalid SMS Deliver PDU [DCS32 = '11']");
									}
								}
								if (Operators.CompareString(text14, "11", false) == 0)
								{
									if (Operators.CompareString(Strings.Mid(text16, 1, 1), "1", false) == 0)
									{
										throw new Exception("Invalid SMS Deliver PDU [DCS76 = '11' and DCS3 = '1']");
									}
									string text32 = Strings.Mid(text16, 2, 1);
									if (Operators.CompareString(text32, "0", false) == 0)
									{
										num4 = 7;
									}
									else if (Operators.CompareString(text32, "1", false) == 0)
									{
										num4 = 8;
									}
								}
								if (num4 != 7 && num4 != 8 && num4 != 16)
								{
									throw new Exception();
								}
								text17 = b(Strings.Left(text, 14));
								text18 = Strings.Mid(text17, 1, 2);
								text19 = Strings.Mid(text17, 3, 2);
								text20 = Strings.Mid(text17, 5, 2);
								text21 = Strings.Mid(text17, 7, 2);
								text22 = Strings.Mid(text17, 9, 2);
								text23 = Strings.Mid(text17, 11, 2);
								text24 = Strings.Mid(text17, 13, 2);
								dateTime = DateAndTime.DateSerial(Conversions.ToInteger("20" + text18), Conversions.ToInteger(text19), Conversions.ToInteger(text20));
								dateTime = DateAndTime.DateAdd((DateInterval)9, (double)(Conversions.ToLong(text21) * 3600 + Conversions.ToLong(text22) * 60 + Conversions.ToLong(text23)), dateTime);
								if (Operators.CompareString(Strings.Mid(p(text24), 1, 1), "0", false) == 0)
								{
									try
									{
										num9 = Conversions.ToLong(text24) * 15;
									}
									catch (Exception ex3)
									{
										ProjectData.SetProjectError(ex3);
										Exception ex4 = ex3;
										num9 = 0L;
										ProjectData.ClearProjectError();
									}
									text26 = "+";
									num12 = (int)num9;
								}
								else
								{
									try
									{
										num9 = Conversions.ToLong(a("0" + Strings.Mid(p(text24), 2, 7), 2)) * 15;
									}
									catch (Exception ex5)
									{
										ProjectData.SetProjectError(ex5);
										Exception ex6 = ex5;
										num9 = 0L;
										ProjectData.ClearProjectError();
									}
									text26 = "-";
									num12 = -(int)num9;
								}
								text27 = Conversion.Int((double)num9 / 60.0).ToString();
								text28 = unchecked(num9 % 60).ToString();
								if (text27.Length < 2)
								{
									text27 = "0" + text27;
								}
								if (text28.Length < 2)
								{
									text28 = "0" + text28;
								}
								text26 = ((Operators.CompareString(text27, "00", false) != 0 || Operators.CompareString(text28, "00", false) != 0) ? (text26 + text27 + ":" + text28) : "GMT");
								text29 = DateAndTime.WeekdayName(DateAndTime.Weekday(dateTime, (FirstDayOfWeek)1), true, (FirstDayOfWeek)0) + ", " + text20 + " " + DateAndTime.MonthName(Conversions.ToInteger(text19), true) + " 20" + text18 + " " + text21 + ":" + text22 + ":" + text23 + " " + text26;
								text = Strings.Right(text, text.Length - 14);
								num5 = (int)m(Strings.Left(text, 2));
								text = Strings.Right(text, text.Length - 2);
								if (Operators.CompareString(text7, "1", false) == 0)
								{
									text25 = Strings.Mid(text, 1, 12);
									if (Operators.CompareString(Strings.Mid(text25, 1, 6), "050003", false) != 0)
									{
										throw new Exception();
									}
									text = Strings.Mid(text, 13, text.Length - 12);
									num8 = (int)m(Strings.Mid(text25, 7, 2));
									num6 = (int)m(Strings.Mid(text25, 9, 2));
									num7 = (int)m(Strings.Mid(text25, 11, 2));
									switch (num4)
									{
									case 7:
										A_2 = num5 - 7;
										break;
									case 8:
									case 16:
										A_2 = num5 - 6;
										break;
									}
								}
								else
								{
									A_2 = num5;
								}
								if (num4 == 16)
								{
									A_2 = (int)Math.Round((double)A_2 / 2.0);
								}
								text2 = a(text, num4, text7, ref A_2);
								if (text2.Length > A_2)
								{
									text2 = Strings.Left(text2, A_2);
								}
								if (num6 > 1 && this.t)
								{
									if (!Monitor.TryEnter(c2, a4))
									{
										a(9011L, "Thread monitor enter attempt timeout [GSM:NewMsgCMT:E]", "GENERAL");
										aw();
										break;
									}
									try
									{
										k k2 = new k(0, 0, text10, text2, dateTime, text29, num12, text4, num8, num6, num7);
										c1.Add((object)k2, (string)null, (object)null, (object)null);
										text30 = "";
										int num13 = num6;
										for (num10 = 1; num10 <= num13; num10++)
										{
											flag = false;
											int count = c1.Count;
											for (num11 = 1; num11 <= count; num11++)
											{
												k k3 = (k)c1[num11];
												if (k3.i == num8 && k3.k == num10 && k3.j == num6 && Operators.CompareString(k3.c, text10, false) == 0 && Operators.CompareString(k3.h, text4, false) == 0)
												{
													text30 += k3.d;
													flag = true;
													break;
												}
											}
											if (!flag)
											{
												break;
											}
										}
									}
									catch (Exception ex7)
									{
										ProjectData.SetProjectError(ex7);
										Exception ex8 = ex7;
										ProjectData.ClearProjectError();
									}
									finally
									{
										Monitor.Exit(c2);
									}
									if (num10 == num6 + 1 && flag)
									{
										if (!Monitor.TryEnter(c2, a4))
										{
											a(9011L, "Thread monitor enter attempt timeout [GSM:NewMsgCMT:F]", "GENERAL");
											aw();
											break;
										}
										try
										{
											int num14 = num6;
											for (num10 = 1; num10 <= num14; num10++)
											{
												for (num11 = c1.Count; num11 >= 1; num11 += -1)
												{
													k k3 = (k)c1[num11];
													if (k3.i == num8 && k3.k == num10 && k3.j == num6 && Operators.CompareString(k3.c, text10, false) == 0 && Operators.CompareString(k3.h, text4, false) == 0)
													{
														c1.Remove(num11);
													}
												}
											}
										}
										catch (Exception ex9)
										{
											ProjectData.SetProjectError(ex9);
											Exception ex10 = ex9;
											ProjectData.ClearProjectError();
										}
										finally
										{
											Monitor.Exit(c2);
										}
										newMessageReceivedEventArgs = new NewMessageReceivedEventArgs(text10, text30, text4, dateTime, text29, num12, num6, num8, 0);
									}
								}
								else
								{
									newMessageReceivedEventArgs = new NewMessageReceivedEventArgs(text10, text2, text4, dateTime, text29, num12, num6, num8, num7);
								}
								if (newMessageReceivedEventArgs != null)
								{
									ThreadPool.QueueUserWorkItem(c, newMessageReceivedEventArgs);
								}
								continue;
							}
							throw new Exception("Invalid SMS Deliver PDU [Incorrect DCS]");
						}
						catch (Exception ex11)
						{
							ProjectData.SetProjectError(ex11);
							Exception ex12 = ex11;
							this.a.b("[GSM:NewMsgCMT:1] " + ex12.Message);
							ProjectData.ClearProjectError();
						}
						continue;
					}
					a(9011L, "Thread monitor enter attempt timeout [GSM:NewMsgCMT:C]", "GENERAL");
					aw();
					break;
				}
			}
			catch (Exception ex13)
			{
				ProjectData.SetProjectError(ex13);
				Exception ex14 = ex13;
				ProjectData.ClearProjectError();
			}
			finally
			{
				if (Monitor.TryEnter(c2, a4))
				{
					try
					{
						if (c1.Count > 0)
						{
							for (num11 = c1.Count; num11 >= 1; num11 += -1)
							{
								k k3 = (k)c1[num11];
								if (DateAndTime.DateDiff((DateInterval)9, k3.l, DateTime.Now, (FirstDayOfWeek)1, (FirstWeekOfYear)1) > 900)
								{
									c1.Remove(num11);
								}
							}
						}
					}
					catch (Exception ex15)
					{
						ProjectData.SetProjectError(ex15);
						Exception ex16 = ex15;
						ProjectData.ClearProjectError();
					}
					finally
					{
						Monitor.Exit(c2);
					}
				}
				else
				{
					a(9011L, "Thread monitor enter attempt timeout [GSM:NewMsgCMT:G]", "GENERAL");
					aw();
				}
				d1.Set();
			}
		}
	}

	private void d(object A_0, bool A_1)
	{
		NewDeliveryReportEventArgs state = null;
		try
		{
			while (this.m_k.g.Count > 0)
			{
				if (Monitor.TryEnter(this.m_l, a4))
				{
					try
					{
						if (this.m_k.g.Count <= 0)
						{
							break;
						}
						state = (NewDeliveryReportEventArgs)this.m_k.g[1];
						this.m_k.g.Remove(1);
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						this.a.b("[GSM:NewReportInbox:1] " + ex2.Message);
						ProjectData.ClearProjectError();
					}
					finally
					{
						Monitor.Exit(this.m_l);
					}
					ThreadPool.QueueUserWorkItem(b, state);
					continue;
				}
				a(9011L, "Thread monitor enter attempt timeout [GSM:NewReportInbox:B]", "GENERAL");
				aw();
				break;
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
			d6.Set();
		}
	}

	private void c(object A_0, bool A_1)
	{
		char[] trimChars = "\r\n".ToCharArray();
		string text = "";
		int num = 0;
		string text2 = "";
		string text3 = "";
		string text4 = "";
		string text5 = "";
		string text6 = "";
		string text7 = "";
		string text8 = "";
		int num2 = 0;
		int num3 = 0;
		string text9 = "";
		string text10 = "";
		string text11 = "";
		string text12 = "";
		string text13 = "";
		string text14 = "";
		string text15 = "";
		string text16 = "";
		string text17 = "";
		string text18 = "";
		DateTime dateTime = default(DateTime);
		string text19 = "";
		DateTime dateTime2 = default(DateTime);
		string text20 = "";
		string text21 = "";
		string text22 = "";
		string text23 = "";
		string text24 = "";
		string text25 = "";
		string text26 = "";
		string text27 = "";
		bool flag = false;
		string text28 = "";
		string text29 = "";
		long num4 = 0L;
		int num5 = 0;
		long num6 = 0L;
		int num7 = 0;
		checked
		{
			try
			{
				while (this.m_k.d.Count > 0)
				{
					if (cu)
					{
						d d2 = new d();
						ThreadPool.RegisterWaitForSingleObject(dz, h, d2, -1, executeOnlyOnce: true);
						d2.a.WaitOne();
						if (d2 != null)
						{
							d2 = null;
						}
					}
					if (Monitor.TryEnter(this.m_l, a4))
					{
						try
						{
							if (this.m_k.d.Count <= 0)
							{
								break;
							}
							text28 = this.m_k.d[1].ToString();
							this.m_k.d.Remove(1);
						}
						catch (Exception ex)
						{
							ProjectData.SetProjectError(ex);
							Exception ex2 = ex;
							ProjectData.ClearProjectError();
						}
						finally
						{
							Monitor.Exit(this.m_l);
						}
						this.a.b("Recv: " + text28);
						try
						{
							text28 = text28.Trim(trimChars).ToUpper().Trim()
								.Replace("+CDS:", "")
								.Trim();
							if (!text28.Contains("\r\n"))
							{
								throw new Exception("Invalid Status Report PDU [does not contain CrLf]");
							}
							string[] array = Strings.Split(text28, "\r\n", -1, (CompareMethod)0);
							if (array.GetUpperBound(0) < 1)
							{
								throw new Exception("Invalid Status Report PDU [Only one part after splitting on CrLf]");
							}
							text = array[1].ToUpper();
							num = (int)(m(Strings.Left(text, 2)) * 2);
							text = Strings.Right(text, text.Length - 2);
							if (num > 0)
							{
								text2 = Strings.Left(text, 2);
								text = Strings.Right(text, text.Length - 2);
								text3 = b(Strings.Left(text, num - 2));
								if (string.Compare(Strings.Right(text3, 1), "F", ignoreCase: true) == 0)
								{
									text3 = Strings.Left(text3, text3.Length - 1);
								}
								if (string.Compare(text2, "91", ignoreCase: true) == 0)
								{
									text3 = "+" + text3;
								}
								text = Strings.Right(text, text.Length - num + 2);
							}
							text4 = Strings.Left(text, 2);
							text = Strings.Right(text, text.Length - 2);
							text5 = p(text4);
							text6 = Strings.Mid(text5, 2, 1);
							text7 = Strings.Mid(text5, 7, 2);
							if (Operators.CompareString(text7, "10", false) != 0)
							{
								text = array[1].ToUpper();
								text4 = Strings.Left(text, 2);
								text = Strings.Right(text, text.Length - 2);
								text5 = p(text4);
								text6 = Strings.Mid(text5, 2, 1);
								text7 = Strings.Mid(text5, 7, 2);
								if (Operators.CompareString(text7, "10", false) != 0)
								{
									throw new Exception("Invalid Status Report PDU [MTI <> '10']");
								}
							}
							text8 = Strings.Mid(text5, 3, 1);
							if (Operators.CompareString(text8, "1", false) == 0)
							{
								text = array[1].ToUpper();
								text4 = Strings.Left(text, 2);
								text = Strings.Right(text, text.Length - 2);
								text5 = p(text4);
								text6 = Strings.Mid(text5, 2, 1);
								text7 = Strings.Mid(text5, 7, 2);
								if (Operators.CompareString(text7, "10", false) != 0)
								{
									throw new Exception("Invalid Status Report PDU [MTI <> '10']");
								}
								text8 = Strings.Mid(text5, 3, 1);
								if (Operators.CompareString(text8, "1", false) == 0)
								{
									throw new Exception("Invalid Status Report PDU [SRQ = '1']");
								}
							}
							if (Operators.CompareString(text8, "1", false) == 0)
							{
								throw new Exception("Invalid Status Report PDU [SRQ = '1']");
							}
							num2 = (int)m(Strings.Left(text, 2));
							text = Strings.Right(text, text.Length - 2);
							num3 = (int)m(Strings.Left(text, 2));
							if (unchecked(num3 % 2) != 0)
							{
								num3++;
							}
							text = Strings.Right(text, text.Length - 2);
							text9 = Strings.Left(text, 2);
							text = Strings.Right(text, text.Length - 2);
							text10 = Strings.Left(text, num3);
							if (Operators.CompareString(text9, "D0", false) == 0)
							{
								text10 = a(text10, 7, "0", ref num3);
							}
							else
							{
								text10 = b(text10);
								if (Operators.CompareString(Strings.UCase(Strings.Right(text10, 1)), "F", false) == 0)
								{
									text10 = Strings.Left(text10, text10.Length - 1);
								}
							}
							if (Operators.CompareString(text9, "91", false) == 0)
							{
								text10 = "+" + text10;
							}
							text = Strings.Right(text, text.Length - num3);
							text11 = b(Strings.Left(text, 14));
							text12 = Strings.Mid(text11, 1, 2);
							text13 = Strings.Mid(text11, 3, 2);
							text14 = Strings.Mid(text11, 5, 2);
							text15 = Strings.Mid(text11, 7, 2);
							text16 = Strings.Mid(text11, 9, 2);
							text17 = Strings.Mid(text11, 11, 2);
							text18 = Strings.Mid(text11, 13, 2);
							dateTime = DateAndTime.DateSerial(Conversions.ToInteger("20" + text12), Conversions.ToInteger(text13), Conversions.ToInteger(text14));
							dateTime = DateAndTime.DateAdd("s", (double)(Conversions.ToLong(text15) * 3600 + Conversions.ToLong(text16) * 60 + Conversions.ToLong(text17)), (object)dateTime);
							if (Operators.CompareString(Strings.Mid(p(text18), 1, 1), "0", false) == 0)
							{
								try
								{
									num4 = Conversions.ToLong(text18) * 15;
								}
								catch (Exception ex3)
								{
									ProjectData.SetProjectError(ex3);
									Exception ex4 = ex3;
									num4 = 0L;
									ProjectData.ClearProjectError();
								}
								num5 = (int)num4;
							}
							else
							{
								try
								{
									num4 = Conversions.ToLong(a("0" + Strings.Mid(p(text18), 2, 7), 2)) * 15;
								}
								catch (Exception ex5)
								{
									ProjectData.SetProjectError(ex5);
									Exception ex6 = ex5;
									num4 = 0L;
									ProjectData.ClearProjectError();
								}
								num5 = -(int)num4;
							}
							text = Strings.Right(text, text.Length - 14);
							text19 = b(Strings.Left(text, 14));
							text22 = Strings.Mid(text19, 1, 2);
							text21 = Strings.Mid(text19, 3, 2);
							text20 = Strings.Mid(text19, 5, 2);
							text23 = Strings.Mid(text19, 7, 2);
							text24 = Strings.Mid(text19, 9, 2);
							text25 = Strings.Mid(text19, 11, 2);
							text26 = Strings.Mid(text19, 13, 2);
							dateTime2 = DateAndTime.DateSerial(Conversions.ToInteger("20" + text22), Conversions.ToInteger(text21), Conversions.ToInteger(text20));
							dateTime2 = DateAndTime.DateAdd("s", (double)(Conversions.ToLong(text23) * 3600 + Conversions.ToLong(text24) * 60 + Conversions.ToLong(text25)), (object)dateTime2);
							if (Operators.CompareString(Strings.Mid(p(text26), 1, 1), "0", false) == 0)
							{
								try
								{
									num6 = Conversions.ToLong(text26) * 15;
								}
								catch (Exception ex7)
								{
									ProjectData.SetProjectError(ex7);
									Exception ex8 = ex7;
									num6 = 0L;
									ProjectData.ClearProjectError();
								}
								num7 = (int)num6;
							}
							else
							{
								try
								{
									num6 = Conversions.ToLong(a("0" + Strings.Mid(p(text26), 2, 7), 2)) * 15;
								}
								catch (Exception ex9)
								{
									ProjectData.SetProjectError(ex9);
									Exception ex10 = ex9;
									num6 = 0L;
									ProjectData.ClearProjectError();
								}
								num7 = -(int)num6;
							}
							text = Strings.Right(text, text.Length - 14);
							text27 = Strings.Left(text, 2);
							long num8 = m(text27);
							if ((num8 >= 3 && num8 <= 63) || (num8 >= 96 && num8 <= 255))
							{
								break;
							}
							flag = ((num8 >= 0 && num8 <= 2) ? true : false);
							NewDeliveryReportEventArgs state = new NewDeliveryReportEventArgs(num2, text10, flag, dateTime2, num7, dateTime, num5);
							ThreadPool.QueueUserWorkItem(b, state);
						}
						catch (Exception ex11)
						{
							ProjectData.SetProjectError(ex11);
							Exception ex12 = ex11;
							this.a.b("[GSM:NewReportCDS:1] " + ex12.Message);
							ProjectData.ClearProjectError();
						}
						continue;
					}
					a(9011L, "Thread monitor enter attempt timeout [GSM:NewReportCDS:C]", "GENERAL");
					aw();
					break;
				}
			}
			catch (Exception ex13)
			{
				ProjectData.SetProjectError(ex13);
				Exception ex14 = ex13;
				ProjectData.ClearProjectError();
			}
			finally
			{
				d3.Set();
			}
		}
	}

	private void b(object A_0, bool A_1)
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		string a_ = a8;
		char[] trimChars = "\r\n".ToCharArray();
		bool flag = true;
		string text4 = "";
		string text5 = "";
		int num = 0;
		string text6 = "";
		string text7 = "";
		string text8 = "";
		string text9 = "";
		string text10 = "";
		string text11 = "";
		string text12 = "";
		int num2 = 0;
		int num3 = 0;
		string text13 = "";
		string text14 = "";
		string text15 = "";
		string text16 = "";
		string text17 = "";
		string text18 = "";
		string text19 = "";
		string text20 = "";
		string text21 = "";
		string text22 = "";
		DateTime dateTime = default(DateTime);
		string text23 = "";
		DateTime dateTime2 = default(DateTime);
		string text24 = "";
		string text25 = "";
		string text26 = "";
		string text27 = "";
		string text28 = "";
		string text29 = "";
		string text30 = "";
		string text31 = "";
		bool flag2 = false;
		string text32 = "";
		int num4 = 0;
		long num5 = 0L;
		int num6 = 0;
		long num7 = 0L;
		int num8 = 0;
		checked
		{
			try
			{
				while (this.m_k.e.Count > 0)
				{
					if (Monitor.TryEnter(this.m_l, a4))
					{
						try
						{
							if (this.m_k.e.Count <= 0)
							{
								break;
							}
							text = this.m_k.e[1].ToString();
							this.m_k.e.Remove(1);
						}
						catch (Exception ex)
						{
							ProjectData.SetProjectError(ex);
							Exception ex2 = ex;
							ProjectData.ClearProjectError();
						}
						finally
						{
							Monitor.Exit(this.m_l);
						}
						this.a.b("Recv: " + text);
						try
						{
							text = text.Trim(trimChars).ToUpper().Trim()
								.Replace("+CDSI:", "")
								.Trim();
							if (text.Length <= 0 || !text.Contains(","))
							{
								throw new Exception("Invalid Status Report PDU [does not contain ',']");
							}
							string[] array = text.Split(",".ToCharArray());
							if (array.GetUpperBound(0) < 1)
							{
								throw new Exception("Invalid Status Report PDU [only one part after splitting on ',']");
							}
							text2 = array[0].Replace("\"", "").Trim();
							text3 = array[1].Trim();
							c c2 = new c(text3, text2, a_);
							ThreadPool.RegisterWaitForSingleObject(dz, i, c2, -1, executeOnlyOnce: true);
							c2.e.WaitOne();
							text4 = c2.b;
							if (c2 != null)
							{
								c2 = null;
							}
							if (text4.Trim(trimChars).Trim().Length <= 0 || text4.Contains("ERROR"))
							{
								throw new Exception("Invalid Status Report PDU");
							}
							text4 = a(text4, A_1: true, A_2: false, A_3: false).Trim(trimChars).Trim();
							if (!text4.Contains("\r\n"))
							{
								throw new Exception();
							}
							string[] array2 = Strings.Split(text4, "\r\n", -1, (CompareMethod)0);
							if (array2.GetUpperBound(0) < 1)
							{
								throw new Exception();
							}
							text5 = array2[1].ToUpper();
							num = (int)(m(Strings.Left(text5, 2)) * 2);
							text5 = Strings.Right(text5, text5.Length - 2);
							if (num > 0)
							{
								text6 = Strings.Left(text5, 2);
								text5 = Strings.Right(text5, text5.Length - 2);
								text7 = b(Strings.Left(text5, num - 2));
								if (string.Compare(Strings.Right(text7, 1), "F", ignoreCase: true) == 0)
								{
									text7 = Strings.Left(text7, text7.Length - 1);
								}
								if (string.Compare(text6, "91", ignoreCase: true) == 0)
								{
									text7 = "+" + text7;
								}
								text5 = Strings.Right(text5, text5.Length - num + 2);
							}
							text8 = Strings.Left(text5, 2);
							text5 = Strings.Right(text5, text5.Length - 2);
							text9 = p(text8);
							text10 = Strings.Mid(text9, 2, 1);
							text11 = Strings.Mid(text9, 7, 2);
							if (Operators.CompareString(text11, "10", false) != 0)
							{
								text5 = array2[1].ToUpper();
								text8 = Strings.Left(text5, 2);
								text5 = Strings.Right(text5, text5.Length - 2);
								text9 = p(text8);
								text10 = Strings.Mid(text9, 2, 1);
								text11 = Strings.Mid(text9, 7, 2);
								if (Operators.CompareString(text11, "10", false) != 0)
								{
									throw new Exception("Invalid Status Report PDU [MTI <> '10']");
								}
							}
							text12 = Strings.Mid(text9, 3, 1);
							if (Operators.CompareString(text12, "1", false) == 0)
							{
								text5 = array2[1].ToUpper();
								text8 = Strings.Left(text5, 2);
								text5 = Strings.Right(text5, text5.Length - 2);
								text9 = p(text8);
								text10 = Strings.Mid(text9, 2, 1);
								text11 = Strings.Mid(text9, 7, 2);
								if (Operators.CompareString(text11, "10", false) != 0)
								{
									throw new Exception("Invalid Status Report PDU [MTI <> '10']");
								}
								text12 = Strings.Mid(text9, 3, 1);
								if (Operators.CompareString(text12, "1", false) == 0)
								{
									throw new Exception("Invalid Status Report PDU [SRQ = '1']");
								}
							}
							num2 = (int)m(Strings.Left(text5, 2));
							text5 = Strings.Right(text5, text5.Length - 2);
							num3 = (int)m(Strings.Left(text5, 2));
							if (unchecked(num3 % 2) != 0)
							{
								num3++;
							}
							text5 = Strings.Right(text5, text5.Length - 2);
							text13 = Strings.Left(text5, 2);
							text5 = Strings.Right(text5, text5.Length - 2);
							text14 = Strings.Left(text5, num3);
							if (Operators.CompareString(text13, "D0", false) == 0)
							{
								text14 = a(text14, 7, "0", ref num3);
							}
							else
							{
								text14 = b(text14);
								if (Operators.CompareString(Strings.UCase(Strings.Right(text14, 1)), "F", false) == 0)
								{
									text14 = Strings.Left(text14, text14.Length - 1);
								}
							}
							if (Operators.CompareString(text13, "91", false) == 0)
							{
								text14 = "+" + text14;
							}
							text5 = Strings.Right(text5, text5.Length - num3);
							text15 = b(Strings.Left(text5, 14));
							text16 = Strings.Mid(text15, 1, 2);
							text17 = Strings.Mid(text15, 3, 2);
							text18 = Strings.Mid(text15, 5, 2);
							text19 = Strings.Mid(text15, 7, 2);
							text20 = Strings.Mid(text15, 9, 2);
							text21 = Strings.Mid(text15, 11, 2);
							text22 = Strings.Mid(text15, 13, 2);
							dateTime = DateAndTime.DateSerial(Conversions.ToInteger("20" + text16), Conversions.ToInteger(text17), Conversions.ToInteger(text18));
							dateTime = DateAndTime.DateAdd("s", (double)(Conversions.ToLong(text19) * 3600 + Conversions.ToLong(text20) * 60 + Conversions.ToLong(text21)), (object)dateTime);
							if (Operators.CompareString(Strings.Mid(p(text22), 1, 1), "0", false) == 0)
							{
								try
								{
									num5 = Conversions.ToLong(text22) * 15;
								}
								catch (Exception ex3)
								{
									ProjectData.SetProjectError(ex3);
									Exception ex4 = ex3;
									num5 = 0L;
									ProjectData.ClearProjectError();
								}
								num6 = (int)num5;
							}
							else
							{
								try
								{
									num5 = Conversions.ToLong(a("0" + Strings.Mid(p(text22), 2, 7), 2)) * 15;
								}
								catch (Exception ex5)
								{
									ProjectData.SetProjectError(ex5);
									Exception ex6 = ex5;
									num5 = 0L;
									ProjectData.ClearProjectError();
								}
								num6 = -(int)num5;
							}
							text5 = Strings.Right(text5, text5.Length - 14);
							text23 = b(Strings.Left(text5, 14));
							text26 = Strings.Mid(text23, 1, 2);
							text25 = Strings.Mid(text23, 3, 2);
							text24 = Strings.Mid(text23, 5, 2);
							text27 = Strings.Mid(text23, 7, 2);
							text28 = Strings.Mid(text23, 9, 2);
							text29 = Strings.Mid(text23, 11, 2);
							text30 = Strings.Mid(text23, 13, 2);
							dateTime2 = DateAndTime.DateSerial(Conversions.ToInteger("20" + text26), Conversions.ToInteger(text25), Conversions.ToInteger(text24));
							dateTime2 = DateAndTime.DateAdd("s", (double)(Conversions.ToLong(text27) * 3600 + Conversions.ToLong(text28) * 60 + Conversions.ToLong(text29)), (object)dateTime2);
							if (Operators.CompareString(Strings.Mid(p(text30), 1, 1), "0", false) == 0)
							{
								try
								{
									num7 = Conversions.ToLong(text30) * 15;
								}
								catch (Exception ex7)
								{
									ProjectData.SetProjectError(ex7);
									Exception ex8 = ex7;
									num7 = 0L;
									ProjectData.ClearProjectError();
								}
								num8 = (int)num7;
							}
							else
							{
								try
								{
									num7 = Conversions.ToLong(a("0" + Strings.Mid(p(text30), 2, 7), 2)) * 15;
								}
								catch (Exception ex9)
								{
									ProjectData.SetProjectError(ex9);
									Exception ex10 = ex9;
									num7 = 0L;
									ProjectData.ClearProjectError();
								}
								num8 = -(int)num7;
							}
							text5 = Strings.Right(text5, text5.Length - 14);
							text31 = Strings.Left(text5, 2);
							long num9 = m(text31);
							if ((num9 >= 3 && num9 <= 63) || (num9 >= 96 && num9 <= 255))
							{
								break;
							}
							flag2 = ((num9 >= 0 && num9 <= 2) ? true : false);
							NewDeliveryReportEventArgs state = new NewDeliveryReportEventArgs(num2, text14, flag2, dateTime2, num8, dateTime, num6);
							ThreadPool.QueueUserWorkItem(b, state);
						}
						catch (Exception ex11)
						{
							ProjectData.SetProjectError(ex11);
							Exception ex12 = ex11;
							this.a.b("[GSM:NewReportCDSI:1] " + ex12.Message);
							ProjectData.ClearProjectError();
						}
						continue;
					}
					a(9011L, "Thread monitor enter attempt timeout [GSM:NewReportCDSI:B]", "GENERAL");
					aw();
					break;
				}
			}
			catch (Exception ex13)
			{
				ProjectData.SetProjectError(ex13);
				Exception ex14 = ex13;
				ProjectData.ClearProjectError();
			}
			finally
			{
				d4.Set();
			}
		}
	}

	private void a(object A_0, bool A_1)
	{
		string text = "";
		int num = -1;
		int num2 = -1;
		string text2 = "";
		string text3 = "";
		NewUSSDReceivedEventArgs newUSSDReceivedEventArgs = null;
		try
		{
			while (this.m_k.h.Count > 0)
			{
				text = "";
				num = -1;
				num2 = -1;
				text2 = "";
				text3 = "";
				if (newUSSDReceivedEventArgs != null)
				{
					newUSSDReceivedEventArgs = null;
				}
				if (Monitor.TryEnter(this.m_l, a4))
				{
					try
					{
						if (this.m_k.h.Count <= 0)
						{
							break;
						}
						text = this.m_k.h[1].ToString();
						this.m_k.h.Remove(1);
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						ProjectData.ClearProjectError();
					}
					finally
					{
						Monitor.Exit(this.m_l);
					}
					this.a.b("Recv: " + text);
					try
					{
						string[] array = Strings.Split(text, "+CUSD:", -1, (CompareMethod)0);
						if (array.GetUpperBound(0) > 0)
						{
							array = Strings.Split(array[1], ",", -1, (CompareMethod)0);
							num = Conversions.ToInteger(array[0]);
							checked
							{
								int num3 = array.GetUpperBound(0) - 1;
								for (int num4 = 1; num4 <= num3; num4++)
								{
									text2 += array[num4];
								}
								text2 = Strings.Mid(text2, 2, text2.Length - 2);
								num2 = Conversions.ToInteger(array[array.GetUpperBound(0)]);
							}
							if (num2 <= 15 && num2 >= 0)
							{
								text3 = text2;
								newUSSDReceivedEventArgs = new NewUSSDReceivedEventArgs(text2, (USSDStatus)num);
								ThreadPool.QueueUserWorkItem(a, newUSSDReceivedEventArgs);
							}
						}
					}
					catch (Exception ex3)
					{
						ProjectData.SetProjectError(ex3);
						Exception ex4 = ex3;
						this.a.b("[GSM:NewUSSD:C] " + ex4.Message);
						ProjectData.ClearProjectError();
					}
					continue;
				}
				a(9011L, "Thread monitor enter attempt timeout [GSM:NewUSSD:B]", "GENERAL");
				aw();
				break;
			}
		}
		catch (Exception ex5)
		{
			ProjectData.SetProjectError(ex5);
			Exception ex6 = ex5;
			ProjectData.ClearProjectError();
		}
		finally
		{
			d5.Set();
		}
	}

	internal string l(string A_0)
	{
		checked
		{
			int num = -A_0.Length;
			string text = "";
			VBMath.Rnd((float)num);
			int length = A_0.Length;
			for (int num2 = 1; num2 <= length; num2++)
			{
				text += Strings.ChrW((int)(Strings.AscW(Strings.Mid(A_0, num2)) ^ (long)Math.Round(VBMath.Rnd() * 99f)));
			}
			return text;
		}
	}

	[SpecialName]
	internal string @as()
	{
		string text = this.p.FileMajorPart.ToString();
		string text2 = this.p.FileMinorPart.ToString();
		string text3 = this.p.FileBuildPart.ToString();
		string text4 = this.p.FilePrivatePart.ToString();
		string comments = this.p.Comments;
		string legalCopyright = this.p.LegalCopyright;
		aw();
		this.b.a();
		if (this.ag)
		{
			return comments + " (Version " + text + "." + text2 + "." + text3 + "." + text4 + ")\r\n" + legalCopyright + "\r\n\r\nRegistered To: " + this.b.Company + "\r\nLicense Type: " + this.b.LicenseType;
		}
		return comments + " (Version " + text + "." + text2 + "." + text3 + "." + text4 + ")\r\n" + legalCopyright + "\r\n\r\n" + l("\u0013SB)BS6\u0014ue\rl.dk");
	}

	[SpecialName]
	internal string am()
	{
		string text = this.p.FileMajorPart.ToString();
		string text2 = this.p.FileMinorPart.ToString();
		string text3 = this.p.FileBuildPart.ToString();
		string comments = this.p.Comments;
		string legalCopyright = this.p.LegalCopyright;
		aw();
		if (!this.af)
		{
			this.b.a(A_0: false);
		}
		if (this.ag)
		{
			return comments + " (Ver." + text + "." + text2 + "." + text3 + ") " + this.b.Company + " [" + this.b.LicenseType + "]";
		}
		return comments + " (Ver." + text + "." + text2 + "." + text3 + ") " + l("\u0013SB)BS6\u0014ue\rl.dk");
	}

	protected virtual void a(bool A_0)
	{
		if (!d8 && A_0)
		{
			try
			{
				((Component)(object)s()).Dispose();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				ProjectData.ClearProjectError();
			}
			if (this.b != null)
			{
				this.b.Dispose();
				this.b = null;
			}
		}
		d8 = true;
	}

	public void ae()
	{
		a(A_0: true);
		GC.SuppressFinalize(this);
	}

	void IDisposable.Dispose()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ae
		this.ae();
	}

	protected virtual void ao()
	{
		a(A_0: false);
		base.Finalize();
	}
}
