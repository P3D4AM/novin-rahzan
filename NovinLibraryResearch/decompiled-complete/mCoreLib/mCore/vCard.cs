using System;
using System.IO;
using System.Security;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace mCore;

public class vCard : IvCard
{
	private class a
	{
		internal bool a;

		internal string b;

		internal string c;

		internal string d;

		internal string e;

		internal string f;

		internal string g;

		internal string h;

		internal string i;

		internal string j;

		internal string k;

		internal string l;

		internal string m;

		internal string n;

		internal string o;

		internal string p;

		internal string q;

		internal string r;

		internal string s;

		internal string t;

		internal string u;

		internal string v;

		internal string w;

		internal int x;

		internal bool y;

		internal bool z;

		internal int aa;

		internal string ab;

		internal string ac;

		public a(string A_0, string A_1, string A_2, string A_3, string A_4, string A_5, string A_6, string A_7, string A_8, string A_9, string A_10, string A_11, string A_12, string A_13, string A_14, string A_15, string A_16, string A_17, string A_18, string A_19, string A_20, string A_21, string A_22, int A_23, bool A_24, int A_25 = 2)
		{
			a = true;
			b = "";
			c = "";
			d = "";
			e = "";
			f = "";
			g = "";
			h = "";
			i = "";
			j = "";
			k = "";
			l = "";
			m = "";
			n = "";
			o = "";
			p = "";
			q = "";
			r = "";
			s = "";
			t = "";
			u = "";
			v = "";
			w = "";
			y = false;
			z = false;
			ab = "";
			b = A_0;
			g = A_1;
			h = A_2;
			i = A_3;
			c = A_4 + A_22;
			d = A_5 + A_22;
			e = A_6 + A_22;
			f = A_7;
			j = A_8;
			k = A_9;
			l = A_10;
			m = A_11;
			n = A_12;
			o = A_13;
			p = A_14;
			q = A_15;
			r = A_16;
			s = A_17;
			t = A_18;
			u = A_19;
			v = A_20;
			w = A_21;
			x = A_23;
			y = A_24;
			aa = A_25;
		}
	}

	private class b
	{
		internal bool a;

		internal string b;

		internal string c;

		internal string d;

		internal bool e;

		internal string f;

		internal int g;

		internal string h;

		internal int i;

		internal bool j;

		public b(string A_0, string A_1, string A_2, int A_3, bool A_4, int A_5 = 2)
		{
			a = true;
			b = "";
			c = "";
			e = false;
			f = "";
			j = false;
			c = A_0;
			b = A_1;
			d = A_2;
			i = A_3;
			j = A_4;
			g = A_5;
		}
	}

	private string m_a;

	private string m_b;

	private string m_c;

	private string m_d;

	private string e;

	private string f;

	private string g;

	private string h;

	private string i;

	private string j;

	private string k;

	private string l;

	private string m;

	private string n;

	private string o;

	private string p;

	private string q;

	private string r;

	private string s;

	private string t;

	private string u;

	private string v;

	private Random w;

	private f x;

	private SMS y;

	private const int z = 1;

	public string Destination
	{
		get
		{
			x.aw();
			return this.m_a;
		}
		set
		{
			x.aw();
			if (value == null)
			{
				value = "";
			}
			this.m_a = value.Trim();
			if (this.m_a.Length == 0 || (!x.j(this.m_a) && !x.f(this.m_a)))
			{
				this.m_a = "";
				x.a(8001L, "Invalid destination mobile number", "VCARD");
			}
			a();
		}
	}

	public string FirstName
	{
		get
		{
			x.aw();
			return f;
		}
		set
		{
			x.aw();
			if (value == null)
			{
				value = "";
			}
			f = value.Trim();
			if (value.Trim().Length > 0 && !x.h(value))
			{
				f = "";
				x.a(8002L, "Invalid characters in first name", "VCARD");
			}
			a();
		}
	}

	public string MiddleName
	{
		get
		{
			x.aw();
			return g;
		}
		set
		{
			x.aw();
			if (value == null)
			{
				value = "";
			}
			g = value.Trim();
			if (value.Trim().Length > 0 && !x.h(value))
			{
				g = "";
				x.a(8003L, "Invalid characters in middle name", "VCARD");
			}
			a();
		}
	}

	public string LastName
	{
		get
		{
			x.aw();
			return h;
		}
		set
		{
			x.aw();
			if (value == null)
			{
				value = "";
			}
			h = value.Trim();
			if (value.Trim().Length > 0 && !x.h(value))
			{
				h = "";
				x.a(8004L, "Invalid characters in last name", "VCARD");
			}
			a();
		}
	}

	public string HomePhone
	{
		get
		{
			x.aw();
			return this.m_b;
		}
		set
		{
			x.aw();
			if (value == null)
			{
				value = "";
			}
			this.m_b = value.Trim();
			if (this.m_b.Trim().Length > 0 && !x.j(this.m_b) && !x.f(this.m_b))
			{
				this.m_b = "";
				x.a(8005L, "Invalid characters in home phone number", "VCARD");
			}
			a();
		}
	}

	public string BusinessPhone
	{
		get
		{
			x.aw();
			return this.m_c;
		}
		set
		{
			x.aw();
			if (value == null)
			{
				value = "";
			}
			this.m_c = value.Trim();
			if (this.m_c.Trim().Length > 0 && !x.j(this.m_c) && !x.f(this.m_c))
			{
				this.m_c = "";
				x.a(8006L, "Invalid characters in business phone number", "VCARD");
			}
		}
	}

	public string MobilePhone
	{
		get
		{
			x.aw();
			return this.m_d;
		}
		set
		{
			x.aw();
			if (value == null)
			{
				value = "";
			}
			this.m_d = value.Trim();
			if (this.m_d.Trim().Length > 0 && !x.j(this.m_d) && !x.f(this.m_d))
			{
				this.m_d = "";
				x.a(8007L, "Invalid characters in mobile phone number", "VCARD");
			}
			a();
		}
	}

	public string Email
	{
		get
		{
			x.aw();
			return e;
		}
		set
		{
			x.aw();
			if (value == null)
			{
				value = "";
			}
			e = value.Trim();
			if (value.Trim().Length > 0 && !x.d(value.Trim()))
			{
				e = "";
				x.a(8008L, "Invalid email address", "VCARD");
			}
			a();
		}
	}

	public string URL
	{
		get
		{
			x.aw();
			return i;
		}
		set
		{
			x.aw();
			if (value == null)
			{
				value = "";
			}
			i = value.Trim();
			a();
		}
	}

	public string JobTitle
	{
		get
		{
			x.aw();
			return j;
		}
		set
		{
			x.aw();
			if (value == null)
			{
				value = "";
			}
			j = value.Trim();
			if (value.Trim().Length > 0 && !x.h(value))
			{
				j = "";
				x.a(8009L, "Invalid characters in title", "VCARD");
			}
			a();
		}
	}

	public string Company
	{
		get
		{
			x.aw();
			return k;
		}
		set
		{
			x.aw();
			if (value == null)
			{
				value = "";
			}
			k = value.Trim();
			if (value.Trim().Length > 0 && !x.h(value))
			{
				k = "";
				x.a(8010L, "Invalid characters in company name", "VCARD");
			}
			a();
		}
	}

	public string HomeAddressStreet
	{
		get
		{
			x.aw();
			return l;
		}
		set
		{
			x.aw();
			if (value == null)
			{
				value = "";
			}
			l = value.Trim();
			if (value.Trim().Length > 0 && !x.h(value))
			{
				l = "";
				x.a(8011L, "Invalid characters in home street address", "VCARD");
			}
			a();
		}
	}

	public string HomeAddressCity
	{
		get
		{
			x.aw();
			return m;
		}
		set
		{
			x.aw();
			if (value == null)
			{
				value = "";
			}
			m = value.Trim();
			if (value.Trim().Length > 0 && !x.h(value))
			{
				m = "";
				x.a(8012L, "Invalid characters in home address city name", "VCARD");
			}
			a();
		}
	}

	public string HomeAddressState
	{
		get
		{
			x.aw();
			return n;
		}
		set
		{
			x.aw();
			if (value == null)
			{
				value = "";
			}
			n = value.Trim();
			if (value.Trim().Length > 0 && !x.h(value))
			{
				n = "";
				x.a(8013L, "Invalid characters in home address state name", "VCARD");
			}
			a();
		}
	}

	public string HomeAddressZIP
	{
		get
		{
			x.aw();
			return o;
		}
		set
		{
			x.aw();
			if (value == null)
			{
				value = "";
			}
			o = value.Trim();
			if (value.Trim().Length > 0 && !x.h(value))
			{
				o = "";
				x.a(8014L, "Invalid characters in home address ZIP code", "VCARD");
			}
			a();
		}
	}

	public string HomeAddressCountry
	{
		get
		{
			x.aw();
			return p;
		}
		set
		{
			x.aw();
			if (value == null)
			{
				value = "";
			}
			p = value.Trim();
			if (value.Trim().Length > 0 && !x.h(value))
			{
				p = "";
				x.a(8015L, "Invalid characters in home address country name", "VCARD");
			}
			a();
		}
	}

	public string BusinessAddressStreet
	{
		get
		{
			x.aw();
			return q;
		}
		set
		{
			x.aw();
			if (value == null)
			{
				value = "";
			}
			q = value.Trim();
			if (value.Trim().Length > 0 && !x.h(value))
			{
				q = "";
				x.a(8016L, "Invalid characters in business street address", "VCARD");
			}
			a();
		}
	}

	public string BusinessAddressCity
	{
		get
		{
			x.aw();
			return r;
		}
		set
		{
			x.aw();
			if (value == null)
			{
				value = "";
			}
			r = value.Trim();
			if (value.Trim().Length > 0 && !x.h(value))
			{
				r = "";
				x.a(8017L, "Invalid characters in business address city name", "VCARD");
			}
			a();
		}
	}

	public string BusinessAddressState
	{
		get
		{
			x.aw();
			return s;
		}
		set
		{
			x.aw();
			if (value == null)
			{
				value = "";
			}
			s = value.Trim();
			if (value.Trim().Length > 0 && !x.h(value))
			{
				s = "";
				x.a(8018L, "Invalid characters in business address state name", "VCARD");
			}
		}
	}

	public string BusinessAddressZIP
	{
		get
		{
			x.aw();
			return t;
		}
		set
		{
			x.aw();
			if (value == null)
			{
				value = "";
			}
			t = value.Trim();
			if (value.Trim().Length > 0 && !x.h(value))
			{
				t = "";
				x.a(8019L, "Invalid characters in business address ZIP code", "VCARD");
			}
			a();
		}
	}

	public string BusinessAddressCountry
	{
		get
		{
			x.aw();
			return u;
		}
		set
		{
			x.aw();
			if (value == null)
			{
				value = "";
			}
			u = value.Trim();
			if (value.Trim().Length > 0 && !x.h(value))
			{
				u = "";
				x.a(8020L, "Invalid characters in business address country name", "VCARD");
			}
			a();
		}
	}

	public string Comment
	{
		get
		{
			x.aw();
			return v;
		}
		set
		{
			x.aw();
			if (value == null)
			{
				value = "";
			}
			v = value.Trim();
			if (value.Trim().Length > 0 && !x.h(value))
			{
				v = "";
				x.a(8021L, "Invalid characters in comment", "VCARD");
			}
			a();
		}
	}

	internal vCard(ref SMS A_0)
	{
		this.m_a = "";
		this.m_b = "";
		this.m_c = "";
		this.m_d = "";
		e = "";
		f = "";
		g = "";
		h = "";
		i = "";
		j = "";
		k = "";
		l = "";
		m = "";
		n = "";
		o = "";
		p = "";
		q = "";
		r = "";
		s = "";
		t = "";
		u = "";
		v = "";
		y = A_0;
		x = y.gm;
		checked
		{
			w = new Random((int)unchecked(DateTime.Now.Ticks % int.MaxValue));
		}
	}

	public string Send()
	{
		string a_ = "";
		if (string.Compare(x.ah, x.ck, ignoreCase: true) == 0)
		{
			a_ = w.Next(0, 10).ToString();
		}
		a a = new a(this.m_a, f, g, h, this.m_b, this.m_c, this.m_d, e, i, j, k, l, m, n, o, p, q, r, s, t, u, v, a_, x.bt, x.bu);
		ThreadPool.RegisterWaitForSingleObject(x.dz, d, a, -1, executeOnlyOnce: true);
		while (a.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		string ac = a.ac;
		if (a != null)
		{
			a = null;
		}
		string result = ac;
		this.a();
		return result;
	}

	string IvCard.Send()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Send
		return this.Send();
	}

	public string SendToQueue()
	{
		string a_ = "";
		if (string.Compare(x.ah, x.ck, ignoreCase: true) == 0)
		{
			a_ = w.Next(0, 10).ToString();
		}
		a a = new a(this.m_a, f, g, h, this.m_b, this.m_c, this.m_d, e, i, j, k, l, m, n, o, p, q, r, s, t, u, v, a_, x.bt, x.bu);
		ThreadPool.RegisterWaitForSingleObject(x.dz, c, a, -1, executeOnlyOnce: true);
		while (a.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		string ab = a.ab;
		if (a != null)
		{
			a = null;
		}
		string result = ab;
		this.a();
		return result;
	}

	string IvCard.SendToQueue()
	{
		//ILSpy generated this explicit interface implementation from .override directive in SendToQueue
		return this.SendToQueue();
	}

	public string SendToQueue(QueuePriority Priority)
	{
		string a_ = "";
		if (string.Compare(x.ah, x.ck, ignoreCase: true) == 0)
		{
			a_ = w.Next(0, 10).ToString();
		}
		a a = new a(this.m_a, f, g, h, this.m_b, this.m_c, this.m_d, e, i, j, k, l, m, n, o, p, q, r, s, t, u, v, a_, x.bt, x.bu, (int)Priority);
		ThreadPool.RegisterWaitForSingleObject(x.dz, c, a, -1, executeOnlyOnce: true);
		while (a.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		string ab = a.ab;
		if (a != null)
		{
			a = null;
		}
		string result = ab;
		this.a();
		return result;
	}

	string IvCard.SendToQueue(QueuePriority Priority)
	{
		//ILSpy generated this explicit interface implementation from .override directive in SendToQueue
		return this.SendToQueue(Priority);
	}

	public string SendFile(string vCardFile)
	{
		string a_ = "";
		if (string.Compare(x.ah, x.ck, ignoreCase: true) == 0)
		{
			a_ = w.Next(0, 10).ToString();
		}
		b b = new b(this.m_a, vCardFile, a_, x.bt, x.bu);
		ThreadPool.RegisterWaitForSingleObject(x.dz, this.b, b, -1, executeOnlyOnce: true);
		while (b.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		string text = b.f;
		if (b != null)
		{
			b = null;
		}
		string result = text;
		a();
		return result;
	}

	string IvCard.SendFile(string vCardFile)
	{
		//ILSpy generated this explicit interface implementation from .override directive in SendFile
		return this.SendFile(vCardFile);
	}

	public string SendFileToQueue(string vCardFile)
	{
		string a_ = "";
		if (string.Compare(x.ah, x.ck, ignoreCase: true) == 0)
		{
			a_ = w.Next(0, 10).ToString();
		}
		b b = new b(this.m_a, vCardFile, a_, x.bt, x.bu);
		ThreadPool.RegisterWaitForSingleObject(x.dz, a, b, -1, executeOnlyOnce: true);
		while (b.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		string text = b.h;
		if (b != null)
		{
			b = null;
		}
		string result = text;
		a();
		return result;
	}

	string IvCard.SendFileToQueue(string vCardFile)
	{
		//ILSpy generated this explicit interface implementation from .override directive in SendFileToQueue
		return this.SendFileToQueue(vCardFile);
	}

	public string SendFileToQueue(string vCardFile, QueuePriority Priority)
	{
		string a_ = "";
		if (string.Compare(x.ah, x.ck, ignoreCase: true) == 0)
		{
			a_ = w.Next(0, 10).ToString();
		}
		b b = new b(this.m_a, vCardFile, a_, x.bt, x.bu, (int)Priority);
		ThreadPool.RegisterWaitForSingleObject(x.dz, a, b, -1, executeOnlyOnce: true);
		while (b.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		string text = b.h;
		if (b != null)
		{
			b = null;
		}
		string result = text;
		a();
		return result;
	}

	string IvCard.SendFileToQueue(string vCardFile, QueuePriority Priority)
	{
		//ILSpy generated this explicit interface implementation from .override directive in SendFileToQueue
		return this.SendFileToQueue(vCardFile, Priority);
	}

	private void d(object A_0, bool A_1)
	{
		a a = (a)A_0;
		string text = "";
		bool flag = false;
		string text2 = "";
		try
		{
			x.aw();
			if (!x.af)
			{
				y.q.a();
			}
			if (Operators.CompareString(x.ah, x.cl, false) == 0)
			{
				x.a(8022L, x.l("odumu5VOg_:5-26cCP/GtqPa6g$"), "VCARD");
				a.z = false;
				return;
			}
			if (a.b.Length <= 0)
			{
				x.a(8023L, "Destination mobile number not defined", "VCARD");
				a.z = false;
				return;
			}
			if (a.g.Length <= 0 && a.i.Length <= 0 && a.h.Length <= 0)
			{
				x.a(8024L, "Contact name is not defined", "VCARD");
				a.z = false;
				return;
			}
			if (a.c.Length <= 0 && a.d.Length <= 0 && a.e.Length <= 0 && a.f.Length <= 0)
			{
				x.a(8025L, "Contact phone not defined", "VCARD");
				a.z = false;
				return;
			}
			text = this.a(a);
			if (text.Length <= 0)
			{
				a.z = false;
				return;
			}
			text2 = x.a(text, a.b, a.x, a.y);
			if (text2.Trim().Length <= 0)
			{
				if (x.ce == 0 && x.z == 0)
				{
					x.ce = 8026L;
					x.cd = "vCard send error";
				}
				if (x.ce > 0)
				{
					x.a(x.ce, x.cd, "VCARD");
				}
			}
			else
			{
				flag = true;
			}
			a.z = flag;
			a.ac = text2;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			y.r.b("[vCard:Send_Method:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			a.a = false;
			x.dz.Set();
		}
	}

	private void c(object A_0, bool A_1)
	{
		a a = (a)A_0;
		string text = "";
		try
		{
			x.aw();
			if (!x.af)
			{
				y.q.a();
			}
			if (Operators.CompareString(x.ah, x.cl, false) == 0)
			{
				x.a(8022L, x.l("odumu5VOg_:5-26cCP/GtqPa6g$"), "VCARD");
				return;
			}
			if (a.b.Length <= 0)
			{
				x.a(8023L, "Destination mobile number not defined", "VCARD");
				return;
			}
			if (a.g.Length <= 0 && a.i.Length <= 0 && a.h.Length <= 0)
			{
				x.a(8024L, "Contact name is not defined", "VCARD");
				return;
			}
			if (a.c.Length <= 0 && a.d.Length <= 0 && a.e.Length <= 0 && a.f.Length <= 0)
			{
				x.a(8025L, "Contact phone not defined", "VCARD");
				return;
			}
			text = this.a(a);
			if (text.Length > 0)
			{
				a.ab = y.objQueue.a(a.b, text, a.aa, a.x, a.y);
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			y.r.b("[vCard:SendToQueue_Method:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			a.a = false;
			x.dz.Set();
		}
	}

	private string a(a A_0)
	{
		string text = "";
		string text2 = "";
		text = "N:";
		text = ((A_0.h.Length <= 0) ? (text + A_0.i + ";") : ((A_0.i.Length <= 0) ? (text + A_0.h + ";") : (text + A_0.h + " " + A_0.i + ";")));
		text += A_0.g;
		if (A_0.c.Length > 0)
		{
			text = text + "\r\nTEL;HOME:" + A_0.c;
		}
		if (A_0.d.Length > 0)
		{
			text = text + "\r\nTEL;WORK:" + A_0.d;
		}
		if (A_0.e.Length > 0)
		{
			text = text + "\r\nTEL;CELL:" + A_0.e;
		}
		if (A_0.f.Length > 0)
		{
			text = text + "\r\nEMAIL;INTERNET:" + A_0.f;
		}
		if (A_0.j.Length > 0)
		{
			text = text + "\r\nURL:" + A_0.j;
		}
		if (A_0.l.Trim().Length > 0)
		{
			text = text + "\r\nORG:" + A_0.l.Trim();
		}
		if (A_0.k.Trim().Length > 0)
		{
			text = text + "\r\nTITLE:" + A_0.k.Trim();
		}
		if (A_0.m.Trim().Length > 0 || A_0.n.Trim().Length > 0 || A_0.o.Trim().Length > 0 || A_0.p.Trim().Length > 0 || A_0.q.Trim().Length > 0)
		{
			text2 = ";;" + A_0.m.Trim() + ";" + A_0.n.Trim() + ";" + A_0.o.Trim() + ";" + A_0.p.Trim() + ";" + A_0.q.Trim();
			text = text + "\r\nADR;HOME;ENCODING=QUOTED-PRINTABLE:" + x.k(text2);
		}
		if (A_0.r.Trim().Length > 0 || A_0.s.Trim().Length > 0 || A_0.t.Trim().Length > 0 || A_0.u.Trim().Length > 0 || A_0.v.Trim().Length > 0)
		{
			text2 = ";;" + A_0.r.Trim() + ";" + A_0.s.Trim() + ";" + A_0.t.Trim() + ";" + A_0.u.Trim() + ";" + A_0.v.Trim();
			text = text + "\r\nADR;WORK;ENCODING=QUOTED-PRINTABLE:" + x.k(text2);
		}
		if (A_0.w.Trim().Length > 0)
		{
			text = text + "\r\nNOTE;ENCODING=QUOTED-PRINTABLE:" + x.k(A_0.w.Trim());
		}
		if (text.Length <= 0)
		{
			return text;
		}
		return "BEGIN:VCARD\r\nVERSION:2.1\r\n" + text + "\r\nEND:VCARD\r\n";
	}

	private void b(object A_0, bool A_1)
	{
		b b = (b)A_0;
		string text = "";
		try
		{
			x.aw();
			if (!x.af)
			{
				y.q.a();
			}
			if (Operators.CompareString(x.ah, x.cl, false) == 0)
			{
				x.a(8022L, x.l("odumu5VOg_:5-26cCP/GtqPa6g$"), "VCARD");
			}
			else if (b.c.Length <= 0)
			{
				x.a(8023L, "Destination mobile number not defined", "VCARD");
			}
			else if (b.b.Length > 0 && b.b.ToUpper().EndsWith(".VCF"))
			{
				if (File.Exists(b.b))
				{
					if (string.Compare(x.ah, x.ck, ignoreCase: true) == 0)
					{
						string[] array = default(string[]);
						try
						{
							array = File.ReadAllLines(b.b);
						}
						catch (ArgumentException ex)
						{
							ProjectData.SetProjectError((Exception)ex);
							ArgumentException ex2 = ex;
							x.a(8072L, "Invalid vCard file name", "VCARD");
							ProjectData.ClearProjectError();
						}
						catch (PathTooLongException ex3)
						{
							ProjectData.SetProjectError((Exception)ex3);
							PathTooLongException ex4 = ex3;
							x.a(8075L, "The specified path, file name, or both exceed the system defined maximum length", "VCARD");
							ProjectData.ClearProjectError();
						}
						catch (DirectoryNotFoundException ex5)
						{
							ProjectData.SetProjectError((Exception)ex5);
							DirectoryNotFoundException ex6 = ex5;
							x.a(8076L, "The specified path for vCard file is invalid", "VCARD");
							ProjectData.ClearProjectError();
						}
						catch (FileNotFoundException ex7)
						{
							ProjectData.SetProjectError((Exception)ex7);
							FileNotFoundException ex8 = ex7;
							x.a(8077L, "The specified vCard file [" + b.b + "] not found", "VCARD");
							ProjectData.ClearProjectError();
						}
						catch (IOException ex9)
						{
							ProjectData.SetProjectError((Exception)ex9);
							IOException ex10 = ex9;
							x.a(8078L, "An I/O error occured while opening the vCard file", "VCARD");
							ProjectData.ClearProjectError();
						}
						catch (UnauthorizedAccessException ex11)
						{
							ProjectData.SetProjectError((Exception)ex11);
							UnauthorizedAccessException ex12 = ex11;
							x.a(8079L, "Permission denied to access the vCard file", "VCARD");
							ProjectData.ClearProjectError();
						}
						catch (NotSupportedException ex13)
						{
							ProjectData.SetProjectError((Exception)ex13);
							NotSupportedException ex14 = ex13;
							x.a(8080L, "The specified vCard file path is in an invalid format", "VCARD");
							ProjectData.ClearProjectError();
						}
						catch (SecurityException ex15)
						{
							ProjectData.SetProjectError((Exception)ex15);
							SecurityException ex16 = ex15;
							x.a(8081L, "Insufficient permission to access the vCard file", "VCARD");
							ProjectData.ClearProjectError();
						}
						if (x.z > 0)
						{
							return;
						}
						if (array.GetUpperBound(0) > 0)
						{
							int upperBound = array.GetUpperBound(0);
							for (int i = 0; i <= upperBound; i = checked(i + 1))
							{
								if (array[i].ToUpper().StartsWith("TEL;") || array[i].ToUpper().StartsWith("TEL:"))
								{
									string[] array2 = array;
									int num = i;
									array2[num] = array[num] + b.d;
								}
								text = text + array[i] + "\r\n";
							}
						}
					}
					else
					{
						try
						{
							text = File.ReadAllText(b.b);
						}
						catch (ArgumentException ex17)
						{
							ProjectData.SetProjectError((Exception)ex17);
							ArgumentException ex18 = ex17;
							x.a(8072L, "Invalid vCard file name", "VCARD");
							ProjectData.ClearProjectError();
						}
						catch (PathTooLongException ex19)
						{
							ProjectData.SetProjectError((Exception)ex19);
							PathTooLongException ex20 = ex19;
							x.a(8075L, "The specified path, file name, or both exceed the system defined maximum length", "VCARD");
							ProjectData.ClearProjectError();
						}
						catch (DirectoryNotFoundException ex21)
						{
							ProjectData.SetProjectError((Exception)ex21);
							DirectoryNotFoundException ex22 = ex21;
							x.a(8076L, "The specified path for vCard file is invalid", "VCARD");
							ProjectData.ClearProjectError();
						}
						catch (FileNotFoundException ex23)
						{
							ProjectData.SetProjectError((Exception)ex23);
							FileNotFoundException ex24 = ex23;
							x.a(8077L, "The specified vCard file [" + b.b + "] not found", "VCARD");
							ProjectData.ClearProjectError();
						}
						catch (IOException ex25)
						{
							ProjectData.SetProjectError((Exception)ex25);
							IOException ex26 = ex25;
							x.a(8078L, "An I/O error occured while opening the vCard file", "VCARD");
							ProjectData.ClearProjectError();
						}
						catch (UnauthorizedAccessException ex27)
						{
							ProjectData.SetProjectError((Exception)ex27);
							UnauthorizedAccessException ex28 = ex27;
							x.a(8079L, "Permission denied to access the vCard file", "VCARD");
							ProjectData.ClearProjectError();
						}
						catch (NotSupportedException ex29)
						{
							ProjectData.SetProjectError((Exception)ex29);
							NotSupportedException ex30 = ex29;
							x.a(8080L, "The specified vCard file path is in an invalid format", "VCARD");
							ProjectData.ClearProjectError();
						}
						catch (SecurityException ex31)
						{
							ProjectData.SetProjectError((Exception)ex31);
							SecurityException ex32 = ex31;
							x.a(8081L, "Insufficient permission to access the vCard file", "VCARD");
							ProjectData.ClearProjectError();
						}
						if (x.z > 0)
						{
							return;
						}
					}
					if (text.Replace("\r", "").Replace("\n", "").Trim()
						.Length > 0)
					{
						b.f = x.a(text, b.c, b.i, b.j);
						if (b.f.Trim().Length <= 0)
						{
							if (x.ce == 0 && x.z == 0)
							{
								x.ce = 8093L;
								x.cd = "vCard file send error";
							}
							if (x.ce > 0)
							{
								x.a(x.ce, x.cd, "VCARD");
							}
						}
						else
						{
							b.e = true;
						}
					}
					else
					{
						x.a(8074L, "vCard file is empty", "VCARD");
					}
				}
				else
				{
					x.a(8073L, "vCard file [" + b.b + "] not found", "VCARD");
				}
			}
			else
			{
				x.a(8072L, "Invalid vCard file name", "VCARD");
			}
		}
		catch (Exception ex33)
		{
			ProjectData.SetProjectError(ex33);
			Exception ex34 = ex33;
			y.r.b("[vCard:SendFile_Method:1] " + ex34.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			b.a = false;
			x.dz.Set();
		}
	}

	private void a(object A_0, bool A_1)
	{
		b b = (b)A_0;
		string text = "";
		try
		{
			x.aw();
			if (!x.af)
			{
				y.q.a();
			}
			if (Operators.CompareString(x.ah, x.cl, false) == 0)
			{
				x.a(8022L, x.l("odumu5VOg_:5-26cCP/GtqPa6g$"), "VCARD");
			}
			else if (b.c.Length <= 0)
			{
				x.a(8023L, "Destination mobile number not defined", "VCARD");
			}
			else if (b.b.Length > 0 && b.b.ToUpper().EndsWith(".VCF"))
			{
				if (File.Exists(b.b))
				{
					if (string.Compare(x.ah, x.ck, ignoreCase: true) == 0)
					{
						string[] array = default(string[]);
						try
						{
							array = File.ReadAllLines(b.b);
						}
						catch (ArgumentException ex)
						{
							ProjectData.SetProjectError((Exception)ex);
							ArgumentException ex2 = ex;
							x.a(8072L, "Invalid vCard file name", "VCARD");
							ProjectData.ClearProjectError();
						}
						catch (PathTooLongException ex3)
						{
							ProjectData.SetProjectError((Exception)ex3);
							PathTooLongException ex4 = ex3;
							x.a(8075L, "The specified path, file name, or both exceed the system defined maximum length", "VCARD");
							ProjectData.ClearProjectError();
						}
						catch (DirectoryNotFoundException ex5)
						{
							ProjectData.SetProjectError((Exception)ex5);
							DirectoryNotFoundException ex6 = ex5;
							x.a(8076L, "The specified path for vCard file is invalid", "VCARD");
							ProjectData.ClearProjectError();
						}
						catch (FileNotFoundException ex7)
						{
							ProjectData.SetProjectError((Exception)ex7);
							FileNotFoundException ex8 = ex7;
							x.a(8077L, "The specified vCard file [" + b.b + "] not found", "VCARD");
							ProjectData.ClearProjectError();
						}
						catch (IOException ex9)
						{
							ProjectData.SetProjectError((Exception)ex9);
							IOException ex10 = ex9;
							x.a(8078L, "An I/O error occured while opening the vCard file", "VCARD");
							ProjectData.ClearProjectError();
						}
						catch (UnauthorizedAccessException ex11)
						{
							ProjectData.SetProjectError((Exception)ex11);
							UnauthorizedAccessException ex12 = ex11;
							x.a(8079L, "Permission denied to access the vCard file", "VCARD");
							ProjectData.ClearProjectError();
						}
						catch (NotSupportedException ex13)
						{
							ProjectData.SetProjectError((Exception)ex13);
							NotSupportedException ex14 = ex13;
							x.a(8080L, "The specified vCard file path is in an invalid format", "VCARD");
							ProjectData.ClearProjectError();
						}
						catch (SecurityException ex15)
						{
							ProjectData.SetProjectError((Exception)ex15);
							SecurityException ex16 = ex15;
							x.a(8081L, "Insufficient permission to access the vCard file", "VCARD");
							ProjectData.ClearProjectError();
						}
						if (x.z > 0)
						{
							return;
						}
						if (array.GetUpperBound(0) > 0)
						{
							int upperBound = array.GetUpperBound(0);
							for (int i = 0; i <= upperBound; i = checked(i + 1))
							{
								if (array[i].ToUpper().StartsWith("TEL;") || array[i].ToUpper().StartsWith("TEL:"))
								{
									array[i] += b.d;
								}
								text = text + array[i] + "\r\n";
							}
						}
					}
					else
					{
						try
						{
							text = File.ReadAllText(b.b);
						}
						catch (ArgumentException ex17)
						{
							ProjectData.SetProjectError((Exception)ex17);
							ArgumentException ex18 = ex17;
							x.a(8072L, "Invalid vCard file name", "VCARD");
							ProjectData.ClearProjectError();
						}
						catch (PathTooLongException ex19)
						{
							ProjectData.SetProjectError((Exception)ex19);
							PathTooLongException ex20 = ex19;
							x.a(8075L, "The specified path, file name, or both exceed the system defined maximum length", "VCARD");
							ProjectData.ClearProjectError();
						}
						catch (DirectoryNotFoundException ex21)
						{
							ProjectData.SetProjectError((Exception)ex21);
							DirectoryNotFoundException ex22 = ex21;
							x.a(8076L, "The specified path for vCard file is invalid", "VCARD");
							ProjectData.ClearProjectError();
						}
						catch (FileNotFoundException ex23)
						{
							ProjectData.SetProjectError((Exception)ex23);
							FileNotFoundException ex24 = ex23;
							x.a(8077L, "The specified vCard file [" + b.b + "] not found", "VCARD");
							ProjectData.ClearProjectError();
						}
						catch (IOException ex25)
						{
							ProjectData.SetProjectError((Exception)ex25);
							IOException ex26 = ex25;
							x.a(8078L, "An I/O error occured while opening the vCard file", "VCARD");
							ProjectData.ClearProjectError();
						}
						catch (UnauthorizedAccessException ex27)
						{
							ProjectData.SetProjectError((Exception)ex27);
							UnauthorizedAccessException ex28 = ex27;
							x.a(8079L, "Permission denied to access the vCard file", "VCARD");
							ProjectData.ClearProjectError();
						}
						catch (NotSupportedException ex29)
						{
							ProjectData.SetProjectError((Exception)ex29);
							NotSupportedException ex30 = ex29;
							x.a(8080L, "The specified vCard file path is in an invalid format", "VCARD");
							ProjectData.ClearProjectError();
						}
						catch (SecurityException ex31)
						{
							ProjectData.SetProjectError((Exception)ex31);
							SecurityException ex32 = ex31;
							x.a(8081L, "Insufficient permission to access the vCard file", "VCARD");
							ProjectData.ClearProjectError();
						}
					}
					if (x.z <= 0)
					{
						if (text.Replace("\r", "").Replace("\n", "").Trim()
							.Length > 0)
						{
							b.h = y.objQueue.a(b.c, text, b.g, b.i, b.j);
						}
						else
						{
							x.a(8074L, "vCard file is empty", "VCARD");
						}
					}
				}
				else
				{
					x.a(8073L, "vCard file [" + b.b + "] not found", "VCARD");
				}
			}
			else
			{
				x.a(8072L, "Invalid vCard file name", "VCARD");
			}
		}
		catch (Exception ex33)
		{
			ProjectData.SetProjectError(ex33);
			Exception ex34 = ex33;
			y.r.b("[vCard:SendFile_Class:SendQ:1] " + ex34.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			b.a = false;
			x.dz.Set();
		}
	}

	private void a()
	{
		if (x.ac.Length > 0)
		{
			switch (x.ac.ToUpper())
			{
			case "GENERAL":
				throw new GeneralException("ERROR " + x.z + ": " + x.y);
			case "SMSSEND":
				throw new SMSSendException("ERROR " + x.z + ": " + x.y);
			case "SMSREAD":
				throw new SMSReadException("ERROR " + x.z + ": " + x.y);
			case "SMSDELETE":
				throw new SMSDeleteException("ERROR " + x.z + ": " + x.y);
			case "WAPPUSH":
				throw new WapPushException("ERROR " + x.z + ": " + x.y);
			case "VCARD":
				throw new vCardException("ERROR " + x.z + ": " + x.y);
			case "VCALENDAR":
				throw new vCalendarException("ERROR " + x.z + ": " + x.y);
			}
		}
	}
}
