using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using FormCustom.Cache;
using PacketParser.Entities;

namespace FormCustom.Froms.LkManage;

public class FRMAlarm : CacheBaseForm
{
	private bool _isDisposed;

	private Container components;

	internal Label Label1;

	public new bool IsDisposed
	{
		get
		{
			if (!base.IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	public FRMAlarm(string contextGuid, string Erro)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
			((Control)Label1).Text = Erro;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMAlarm.cs", 18);
		}
	}

	public void FRMAlarm_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMAlarm_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMAlarm.cs", 29);
		}
	}

	public void FRMAlarm_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMAlarm_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMAlarm.cs", 41);
		}
	}

	public void Label1_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Label1_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMAlarm.cs", 53);
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMAlarm.cs", 70);
		}
	}

	private void FRMAlarm_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	[DebuggerStepThrough]
	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Expected O, but got Unknown
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Expected O, but got Unknown
		Label1 = new Label();
		((Control)this).SuspendLayout();
		((Control)Label1).Dock = (DockStyle)5;
		((Control)Label1).Font = new Font("B Titr", 15.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label1).ForeColor = Color.FromArgb(0, 0, 192);
		((Control)Label1).Location = new Point(0, 0);
		((Control)Label1).Name = "Label1";
		((Control)Label1).Size = new Size(292, 273);
		((Control)Label1).TabIndex = 0;
		((Control)Label1).Text = "Label1";
		Label1.TextAlign = (ContentAlignment)32;
		((Control)Label1).Click += Label1_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Red;
		((Form)this).ClientSize = new Size(292, 273);
		((Control)this).Controls.Add((Control)(object)Label1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FRMAlarm";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "FRMAlarm";
		((Form)this).WindowState = (FormWindowState)2;
		((Form)this).FormClosed += new FormClosedEventHandler(FRMAlarm_FormClosed);
		((Control)this).Click += FRMAlarm_Click;
		((Control)this).KeyDown += new KeyEventHandler(FRMAlarm_KeyDown);
		((Control)this).ResumeLayout(false);
	}
}
