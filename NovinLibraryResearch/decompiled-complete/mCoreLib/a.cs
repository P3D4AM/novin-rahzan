using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

internal class a : IDisposable
{
	private IContainer m_a;

	private NotifyIcon m_b;

	private ContextMenuStrip m_c;

	private ToolStripMenuItem m_d;

	private ToolStripSeparator m_e;

	private ToolStripMenuItem m_f;

	private Icon m_g;

	private bool m_h;

	private bool i;

	private string j;

	private string k;

	private FileVersionInfo l;

	private string m;

	private string n;

	private string o;

	private string p;

	private bool q;

	[SpecialName]
	[DebuggerNonUserCode]
	private virtual NotifyIcon g()
	{
		return this.m_b;
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	[SpecialName]
	[DebuggerNonUserCode]
	private virtual void a(NotifyIcon A_0)
	{
		EventHandler eventHandler = a;
		if (this.m_b != null)
		{
			this.m_b.DoubleClick -= eventHandler;
		}
		this.m_b = A_0;
		if (this.m_b != null)
		{
			this.m_b.DoubleClick += eventHandler;
		}
	}

	[SpecialName]
	[DebuggerNonUserCode]
	private virtual ToolStripMenuItem d()
	{
		return this.m_d;
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	[SpecialName]
	[DebuggerNonUserCode]
	private virtual void a(ToolStripMenuItem A_0)
	{
		EventHandler eventHandler = b;
		if (this.m_d != null)
		{
			((ToolStripItem)this.m_d).Click -= eventHandler;
		}
		this.m_d = A_0;
		if (this.m_d != null)
		{
			((ToolStripItem)this.m_d).Click += eventHandler;
		}
	}

	[SpecialName]
	[DebuggerNonUserCode]
	private virtual ToolStripSeparator b()
	{
		return this.m_e;
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	[SpecialName]
	[DebuggerNonUserCode]
	private virtual void a(ToolStripSeparator A_0)
	{
		this.m_e = A_0;
	}

	[SpecialName]
	[DebuggerNonUserCode]
	private virtual ToolStripMenuItem f()
	{
		return this.m_f;
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	[SpecialName]
	[DebuggerNonUserCode]
	private virtual void b(ToolStripMenuItem A_0)
	{
		EventHandler eventHandler = c;
		if (this.m_f != null)
		{
			((ToolStripItem)this.m_f).Click -= eventHandler;
		}
		this.m_f = A_0;
		if (this.m_f != null)
		{
			((ToolStripItem)this.m_f).Click += eventHandler;
		}
	}

	internal a(bool A_0, string A_1, string A_2, FileVersionInfo A_3, string A_4, string A_5, string A_6, string A_7)
	{
		a((NotifyIcon)null);
		this.m_c = null;
		a((ToolStripMenuItem)null);
		a((ToolStripSeparator)null);
		b(null);
		this.m_g = null;
		this.m_h = false;
		i = false;
		j = "";
		k = "";
		l = null;
		m = "";
		q = false;
		i = A_0;
		j = A_1;
		k = A_2;
		l = A_3;
		m = A_4;
		n = A_5;
		o = A_6;
		p = A_7;
	}

	internal bool c()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		this.m_a = new Container();
		a(new NotifyIcon(this.m_a));
		this.m_c = new ContextMenuStrip(this.m_a);
		this.m_g = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("mCore.mCoreIcon.ico"));
		a(new ToolStripMenuItem("About"));
		a(new ToolStripSeparator());
		b(new ToolStripMenuItem("What's this?"));
		((Control)this.m_c).SuspendLayout();
		((Control)this.m_c).Name = "cmsSysTray";
		ContextMenuStrip obj = this.m_c;
		Size size = new Size(147, 98);
		((Control)obj).Size = size;
		((ToolStripItem)d()).Name = "csiAbout";
		ToolStripMenuItem obj2 = d();
		size = new Size(146, 22);
		((ToolStripItem)obj2).Size = size;
		((ToolStripItem)b()).Name = "csiSeparator";
		ToolStripSeparator obj3 = b();
		size = new Size(143, 6);
		((ToolStripItem)obj3).Size = size;
		((ToolStripItem)f()).Name = "csiWhatsThis";
		ToolStripMenuItem obj4 = f();
		size = new Size(146, 22);
		((ToolStripItem)obj4).Size = size;
		((ToolStrip)this.m_c).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[3]
		{
			(ToolStripItem)d(),
			(ToolStripItem)b(),
			(ToolStripItem)f()
		});
		g().Icon = this.m_g;
		if (i)
		{
			g().Text = l.ProductName + "\r\nSTANDARD LICENSE";
		}
		else
		{
			g().Text = l.ProductName + "\r\nTRIAL VERSION";
		}
		g().ContextMenuStrip = this.m_c;
		((Control)this.m_c).ResumeLayout(false);
		g().Visible = true;
		if (i)
		{
			g().ShowBalloonTip(2000, l.ProductName, "Registered To: " + j + "\r\nLicense Type: " + k + "\r\nRight-click here for more information", (ToolTipIcon)1);
		}
		else
		{
			g().ShowBalloonTip(5000, l.ProductName, "TRIAL VERSION\r\nRight-click here for more information", (ToolTipIcon)1);
		}
		return true;
	}

	internal void a(bool A_0, string A_1, string A_2, FileVersionInfo A_3, string A_4, string A_5, string A_6, string A_7)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		i = A_0;
		j = A_1;
		k = A_2;
		l = A_3;
		m = A_4;
		n = A_5;
		o = A_6;
		p = A_7;
		if (g() != null)
		{
			this.m_c = new ContextMenuStrip(this.m_a);
			this.m_g = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("mCore.mCoreIcon.ico"));
			a(new ToolStripMenuItem("About"));
			a(new ToolStripSeparator());
			b(new ToolStripMenuItem("What's this?"));
			((Control)this.m_c).SuspendLayout();
			((Control)this.m_c).Name = "cmsSysTray";
			ContextMenuStrip obj = this.m_c;
			Size size = new Size(147, 98);
			((Control)obj).Size = size;
			((ToolStripItem)d()).Name = "csiAbout";
			ToolStripMenuItem obj2 = d();
			size = new Size(146, 22);
			((ToolStripItem)obj2).Size = size;
			((ToolStripItem)b()).Name = "csiSeparator";
			ToolStripSeparator obj3 = b();
			size = new Size(143, 6);
			((ToolStripItem)obj3).Size = size;
			((ToolStripItem)f()).Name = "csiWhatsThis";
			ToolStripMenuItem obj4 = f();
			size = new Size(146, 22);
			((ToolStripItem)obj4).Size = size;
			((ToolStrip)this.m_c).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[3]
			{
				(ToolStripItem)d(),
				(ToolStripItem)b(),
				(ToolStripItem)f()
			});
			g().Icon = this.m_g;
			if (i)
			{
				g().Text = l.ProductName + "\r\nSTANDARD LICENSE";
			}
			else
			{
				g().Text = l.ProductName + "\r\nTRIAL VERSION";
			}
			g().ContextMenuStrip = this.m_c;
			((Control)this.m_c).ResumeLayout(false);
			g().Visible = true;
			if (i)
			{
				g().ShowBalloonTip(2000, l.ProductName, "Registered To: " + j + "\r\nLicense Type: " + k + "\r\nRight-click here for more information", (ToolTipIcon)1);
			}
			else
			{
				g().ShowBalloonTip(5000, l.ProductName, "TRIAL VERSION\r\nRight-click here for more information", (ToolTipIcon)1);
			}
		}
	}

	private void a(object A_0, EventArgs A_1)
	{
		a();
	}

	internal void b(object A_0, EventArgs A_1)
	{
		a();
	}

	internal void c(object A_0, EventArgs A_1)
	{
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		string text = "";
		if (Operators.CompareString(m, n, false) == 0)
		{
			text = "This notification icon is displayed in the SysTray because one of the application running presently on this computer is using TRIAL VERSION of " + l.ProductName + ".\r\n\r\nPurchasing DISTRIBUTION License (LITE or PRO) will remove the icon from the systray.\r\n\r\nPurchasing STANDARD License (LITE or PRO) will still display this icon, but the trial version message will be replaced with license information.\r\n\r\nFor more information please visit http://www.logixmobile.com";
		}
		else if (Operators.CompareString(m, o, false) == 0)
		{
			text = "This notification icon is displayed in the SysTray because one of the application presently running on this computer is using '" + l.ProductName + "' (LITE-STANDARD License).\r\n\r\nDISTRIBUTION License (LITE or PRO) removes the icon from the SysTray.\r\n\r\nFor more information please visit http://www.logixmobile.com";
		}
		else if (Operators.CompareString(m, p, false) == 0)
		{
			text = "This notification icon is displayed in the SysTray because one of the application presently running on this computer is using '" + l.ProductName + "' (PRO-STANDARD License).\r\n\r\nDISTRIBUTION License (LITE or PRO) removes the icon from the SysTray.\r\n\r\nFor more information please visit http://www.logixmobile.com";
		}
		try
		{
			Interaction.MsgBox((object)text, (MsgBoxStyle)64, (object)"What's this?");
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			ProjectData.ClearProjectError();
		}
	}

	private void a()
	{
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		string text = "";
		string text2 = l.FileMajorPart.ToString();
		string text3 = l.FileMinorPart.ToString();
		string text4 = l.FileBuildPart.ToString();
		string comments = l.Comments;
		string legalCopyright = l.LegalCopyright;
		text = ((!i) ? (comments + " (Version " + text2 + "." + text3 + "." + text4 + ")\r\n" + legalCopyright + "\r\n\r\n" + a("\u0013SB)BS6\u0014ue\rl.dk")) : (comments + " (Version " + text2 + "." + text3 + "." + text4 + ")\r\n" + legalCopyright + "\r\nhttp://www.logixmobile.com\r\n\r\nRegistered To: " + j + "\r\nLicense Type: " + k));
		try
		{
			Interaction.MsgBox((object)text, (MsgBoxStyle)64, (object)"About...");
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			ProjectData.ClearProjectError();
		}
	}

	private string a(string A_0)
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

	protected virtual void a(bool A_0)
	{
		if (!q && A_0)
		{
			if (this.m_a != null)
			{
				this.m_a.Dispose();
			}
			if (l != null)
			{
				l = null;
			}
			if (this.m_g != null)
			{
				this.m_g = null;
			}
			if (d() != null)
			{
				a((ToolStripMenuItem)null);
			}
			if (b() != null)
			{
				a((ToolStripSeparator)null);
			}
			if (f() != null)
			{
				b(null);
			}
		}
		q = true;
	}

	public void h()
	{
		a(A_0: true);
		GC.SuppressFinalize(this);
	}

	void IDisposable.Dispose()
	{
		//ILSpy generated this explicit interface implementation from .override directive in h
		this.h();
	}

	protected virtual void e()
	{
		a(A_0: false);
		base.Finalize();
	}
}
