using System;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace mCore;

public class Message : IMessage
{
	private class a
	{
		internal bool a;

		internal bool b;

		public a()
		{
			a = true;
			b = false;
		}
	}

	private string m_a;

	private string b;

	private DateTime c;

	private string d;

	private string e;

	private int f;

	private int g;

	private SMS h;

	private f i;

	private const int j = 1;

	public int Index => f;

	public string Phone => this.m_a;

	public string Text => b;

	public DateTime TimeStamp => c;

	public string TimeStampRFC => d;

	public int TimeZone => g;

	public string SMSC => e;

	internal Message(ref SMS A_0, int A_1, string A_2, string A_3, DateTime A_4, string A_5, int A_6, string A_7)
	{
		this.m_a = "";
		b = "";
		d = "";
		e = "";
		g = 0;
		h = A_0;
		i = h.gm;
		f = A_1;
		this.m_a = A_2;
		b = A_3;
		c = A_4;
		d = A_5;
		g = A_6;
		e = A_7;
	}

	public bool Delete()
	{
		a a = new a();
		ThreadPool.RegisterWaitForSingleObject(i.dz, this.a, a, -1, executeOnlyOnce: true);
		while (a.a)
		{
			Thread.Sleep(1);
			Application.DoEvents();
		}
		return a.b;
	}

	bool IMessage.Delete()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Delete
		return this.Delete();
	}

	internal bool a()
	{
		string text = "";
		bool result = false;
		try
		{
			this.i.aw();
			Collection cz = this.i.cz;
			Collection c = this.i.c0;
			text = "";
			if (this.i.ae <= 0 || this.i.ae > this.i.c0.Count)
			{
				this.i.a(6003L, "Invalid message item index", "SMSDELETE");
				bool result2 = default(bool);
				return result2;
			}
			Message message = (Message)c[this.i.ae.ToString()];
			int index = message.Index;
			int count = cz.Count;
			for (int i = 1; i <= count; i = checked(i + 1))
			{
				i i2 = (i)cz[i];
				if (i2.b == index)
				{
					text = ((Operators.CompareString(text, "", false) != 0) ? (text + "," + i2.a) : i2.a.ToString());
				}
			}
			bool flag;
			if (text.Length > 0)
			{
				flag = this.i.a(index, text);
			}
			else
			{
				this.i.b8 = 6004L;
				this.i.b7 = "Cannot find the requested message for deletion";
				flag = false;
			}
			if (flag)
			{
				result = true;
			}
			else
			{
				result = false;
				if (this.i.b8 == 0)
				{
					this.i.b8 = 6005L;
					this.i.b7 = "Delete message error";
				}
				if (this.i.b8 != 0)
				{
					this.i.a(this.i.b8, this.i.b7, "SMSDELETE");
				}
			}
			if (this.i.cy)
			{
				this.i.ap();
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.i.a.b("[Message:DeleteEx:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		return result;
	}

	private void a(object A_0, bool A_1)
	{
		a a = (a)A_0;
		string text = "";
		try
		{
			this.i.aw();
			Collection cz = this.i.cz;
			Collection c = this.i.c0;
			a.b = false;
			text = "";
			if (this.i.ae <= 0 || this.i.ae > this.i.c0.Count)
			{
				this.i.a(6003L, "Invalid message item index", "SMSDELETE");
				return;
			}
			Message message = (Message)c[this.i.ae.ToString()];
			int index = message.Index;
			int count = cz.Count;
			for (int i = 1; i <= count; i = checked(i + 1))
			{
				i i2 = (i)cz[i];
				if (i2.b == index)
				{
					text = ((Operators.CompareString(text, "", false) != 0) ? (text + "," + i2.a) : i2.a.ToString());
				}
			}
			bool flag;
			if (text.Length > 0)
			{
				flag = this.i.a(index, text);
			}
			else
			{
				this.i.b8 = 6004L;
				this.i.b7 = "Cannot find the requested message for deletion";
				flag = false;
			}
			if (flag)
			{
				a.b = true;
			}
			else
			{
				a.b = false;
				if (this.i.b8 == 0)
				{
					this.i.b8 = 6005L;
					this.i.b7 = "Delete message error";
				}
				if (this.i.b8 != 0)
				{
					this.i.a(this.i.b8, this.i.b7, "SMSDELETE");
				}
			}
			if (this.i.cy)
			{
				this.i.ap();
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.i.a.b("[Message:Delete_Method:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		finally
		{
			a.a = false;
			this.i.dz.Set();
		}
	}
}
