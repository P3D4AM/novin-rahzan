using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualBasic.CompilerServices;

internal class h
{
	internal string a;

	internal string b;

	internal int c;

	internal string d;

	internal string e;

	internal bool f;

	internal int g;

	internal int h;

	internal bool i;

	internal int j;

	internal string k;

	internal string l;

	internal DateTime m;

	internal string n;

	internal string o;

	internal string p;

	internal string q;

	internal int r;

	internal int s;

	private Random t;

	internal h(string A_0, int A_1, string A_2, string A_3, bool A_4, int A_5, int A_6, bool A_7, int A_8, string A_9, string A_10, DateTime A_11, string A_12, string A_13, string A_14, string A_15, string A_16, int A_17, int A_18)
	{
		this.a = "";
		b = "";
		c = 2;
		d = "";
		e = "";
		f = false;
		g = 7;
		h = 3;
		i = false;
		j = 167;
		k = "";
		l = "";
		m = default(DateTime);
		n = "";
		o = "";
		p = "";
		q = "";
		r = 0;
		s = 0;
		this.a = A_0;
		c = A_1;
		d = A_2;
		e = A_3;
		f = A_4;
		g = A_5;
		h = A_6;
		i = A_7;
		j = A_8;
		k = A_9;
		l = A_10;
		m = A_11;
		n = A_12;
		o = A_13;
		p = A_14;
		q = A_15;
		r = A_17;
		s = A_18;
		checked
		{
			t = new Random((int)unchecked(DateTime.Now.Ticks % int.MaxValue));
			b = a(GetHashCode() + this.a + c + d + e + f + g + h + k + l + n + o + p + q + DateTime.UtcNow.ToString() + t.Next(0, 9999) + A_16);
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
			text = "";
			ProjectData.ClearProjectError();
		}
		return text;
	}
}
