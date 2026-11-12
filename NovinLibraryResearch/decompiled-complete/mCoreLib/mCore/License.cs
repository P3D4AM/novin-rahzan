using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualBasic.CompilerServices;

namespace mCore;

public class License : ILicense, IDisposable
{
	private string m_a;

	private string b;

	private string c;

	private f d;

	private bool e;

	public string Company
	{
		get
		{
			d.aw();
			return this.m_a;
		}
		set
		{
			d.aw();
			d.ag = true;
			d.af = false;
			if (value == null)
			{
				value = "";
			}
			this.m_a = value.Trim().ToUpper();
			a();
		}
	}

	public string Key
	{
		get
		{
			d.aw();
			return b;
		}
		set
		{
			d.aw();
			d.ag = true;
			d.af = false;
			if (value == null)
			{
				value = "";
			}
			b = value.Trim().ToUpper();
			a();
		}
	}

	public string LicenseType
	{
		get
		{
			d.aw();
			return c;
		}
		set
		{
			d.aw();
			d.ag = true;
			d.af = false;
			if (value == null)
			{
				value = "";
			}
			c = value.Trim().ToUpper();
			a();
		}
	}

	public bool IsLicensed => true;

	internal License(ref SMS A_0)
	{
		this.m_a = "NoneForce/UnREal-RCE\0\0\0\0\0\0\0\0\0\0\0\0";
		b = "";
		c = "";
		e = false;
		d = A_0.gm;
	}

	internal bool a(bool A_0 = true)
	{
		//Discarded unreachable code: IL_0052, IL_008c, IL_008e, IL_3a28
		return true;
	}

	private string a(string A_0, string A_1, string A_2, double A_3, int A_4, string A_5)
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		string text4 = "";
		try
		{
			text3 = "";
			text2 = "";
			text4 = "43210ABCDEFGHJKLMNPQRTUVWXY98765";
			if (A_0.Length != 0 && A_1.Length != 0 && A_2.Length != 0 && A_3 >= 1.0 && A_4 >= 0)
			{
				text = a(A_0 + A_1.ToUpper() + A_2.ToUpper() + A_3 + A_4 + A_5);
				if (text.Length >= 32)
				{
					int num = 1;
					do
					{
						long num2 = Conversions.ToLong("&H" + text.Substring(checked(num - 1), 2)) % 32;
						checked
						{
							text2 = text4.Substring((int)num2, 1) + text2;
							num += 2;
						}
					}
					while (num <= 32);
					text3 = text2.Substring(4, 4) + "-" + text2.Substring(12, 4) + "-" + text2.Substring(0, 4) + "-" + text2.Substring(8, 4);
				}
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			d.a.b("[License:1] " + ex2.Message);
			text3 = "";
			ProjectData.ClearProjectError();
		}
		return text3;
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
			d.a.b("[License:2] " + ex2.Message);
			text = "";
			ProjectData.ClearProjectError();
		}
		return text;
	}

	private void a()
	{
		if (d.ac.Length > 0)
		{
			switch (d.ac.ToUpper())
			{
			case "GENERAL":
				throw new GeneralException("ERROR " + d.z + ": " + d.y);
			case "SMSSEND":
				throw new SMSSendException("ERROR " + d.z + ": " + d.y);
			case "SMSREAD":
				throw new SMSReadException("ERROR " + d.z + ": " + d.y);
			case "SMSDELETE":
				throw new SMSDeleteException("ERROR " + d.z + ": " + d.y);
			case "WAPPUSH":
				throw new WapPushException("ERROR " + d.z + ": " + d.y);
			case "VCARD":
				throw new vCardException("ERROR " + d.z + ": " + d.y);
			case "VCALENDAR":
				throw new vCalendarException("ERROR " + d.z + ": " + d.y);
			}
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!e && disposing)
		{
			try
			{
				if (f.f != null)
				{
					f.f.h();
					f.f = null;
					f.e = false;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				ProjectData.ClearProjectError();
			}
		}
		e = true;
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
