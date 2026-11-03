using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FormCustom.Cache;
using PacketParser.Entities;

namespace PayamakUi.Forms.FRMOthers;

public class FRMProgress : CacheBaseForm
{
	private int _Counts;

	private int _Counter = 1;

	private IContainer components;

	internal ProgressBar ProgressBar1;

	internal Label Label1;

	public string PortNumber
	{
		set
		{
			try
			{
				_Counter++;
				((Control)Label1).Text = "در حال بررسی ارتباط با " + value;
				short num = (short)Math.Round(Math.Round((double)_Counter / (double)_Counts * 100.0));
				if (num >= 100)
				{
					ProgressBar1.Value = 100;
					((Form)this).DialogResult = (DialogResult)1;
					((Form)this).Close();
				}
				else
				{
					ProgressBar1.Value = num;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "PortNumber", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOthers\\FRMProgress.cs", 42);
			}
		}
	}

	public FRMProgress(string contextGuid, int pcount)
		: base(contextGuid)
	{
		InitializeComponent();
		_Counter = pcount;
	}

	private void FRMProgress_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 27)
			{
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMProgress_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOthers\\FRMProgress.cs", 53);
		}
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Expected O, but got Unknown
		ProgressBar1 = new ProgressBar();
		Label1 = new Label();
		((Control)this).SuspendLayout();
		((Control)ProgressBar1).Anchor = (AnchorStyles)13;
		((Control)ProgressBar1).Location = new Point(12, 12);
		((Control)ProgressBar1).Name = "ProgressBar1";
		((Control)ProgressBar1).Size = new Size(536, 43);
		((Control)ProgressBar1).TabIndex = 0;
		((Control)Label1).Anchor = (AnchorStyles)13;
		((Control)Label1).ForeColor = Color.Blue;
		((Control)Label1).Location = new Point(12, 67);
		((Control)Label1).Name = "Label1";
		((Control)Label1).Size = new Size(536, 36);
		((Control)Label1).TabIndex = 1;
		((Control)Label1).Text = "Label1";
		Label1.TextAlign = (ContentAlignment)32;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(560, 112);
		((Control)this).Controls.Add((Control)(object)Label1);
		((Control)this).Controls.Add((Control)(object)ProgressBar1);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FRMProgress";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "در حال جستجوی پورت کام";
		((Control)this).KeyDown += new KeyEventHandler(FRMProgress_KeyDown);
		((Control)this).ResumeLayout(false);
	}
}
