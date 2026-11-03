using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PacketParser.Entities;

namespace FormCustom;

public class frmProgress : AccountingBaseForm
{
	private int _total;

	private bool _isDisposed;

	private IContainer components;

	internal Label lblText;

	internal Label Label1;

	internal ProgressBar ProgressBar1;

	public int Level
	{
		set
		{
			try
			{
				ProgressBar1.Value = 100 * value / _total;
				Application.DoEvents();
				if (value - 1 >= _total)
				{
					((Form)this).Close();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Level", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmProgress.cs", 58);
			}
		}
	}

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

	public frmProgress(string contextGuid, int total)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
			_total = total;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmProgress.cs", 19);
		}
	}

	public frmProgress(int total, string message, string context)
		: base(context)
	{
		try
		{
			InitializeComponent();
			_total = total;
			((Control)lblText).Text = message;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmProgress.cs", 32);
		}
	}

	private void frmProgress_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "frmProgress_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmProgress.cs", 44);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmProgress.cs", 78);
		}
	}

	private void frmProgress_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Expected O, but got Unknown
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Expected O, but got Unknown
		lblText = new Label();
		Label1 = new Label();
		ProgressBar1 = new ProgressBar();
		((Control)this).SuspendLayout();
		((Control)lblText).Font = new Font("B Titr", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)lblText).ForeColor = Color.Blue;
		((Control)lblText).Location = new Point(11, 43);
		((Control)lblText).Name = "lblText";
		((Control)lblText).Size = new Size(479, 31);
		((Control)lblText).TabIndex = 5;
		((Control)lblText).Text = "[سیستم در حال ثبت اطلاعات میباشد ....]";
		((Control)Label1).Font = new Font("B Titr", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label1).ForeColor = Color.Blue;
		((Control)Label1).Location = new Point(11, 10);
		((Control)Label1).Name = "Label1";
		((Control)Label1).Size = new Size(479, 31);
		((Control)Label1).TabIndex = 3;
		((Control)Label1).Text = "[لطفا کمی صبر کنید ....]";
		((Control)ProgressBar1).Location = new Point(11, 77);
		((Control)ProgressBar1).Name = "ProgressBar1";
		((Control)ProgressBar1).Size = new Size(479, 50);
		((Control)ProgressBar1).TabIndex = 4;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(501, 136);
		((Form)this).ControlBox = false;
		((Control)this).Controls.Add((Control)(object)lblText);
		((Control)this).Controls.Add((Control)(object)Label1);
		((Control)this).Controls.Add((Control)(object)ProgressBar1);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmProgress";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Form)this).FormClosed += new FormClosedEventHandler(frmProgress_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(frmProgress_KeyDown);
		((Control)this).ResumeLayout(false);
	}
}
