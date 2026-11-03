using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PacketParser.Entities;

namespace FormCustom.SignalRConnection.Uc;

public class UcSyncDoneResponseDetail : UserControl
{
	private decimal _percent;

	private string _title;

	private IContainer components;

	private Label lblDate;

	private Label lblTitle;

	private ProgressBar progressBar1;

	public string Title
	{
		get
		{
			return _title;
		}
		set
		{
			_title = value;
			((Control)lblTitle).Text = _title;
		}
	}

	public string Date
	{
		get
		{
			return ((Control)lblDate).Text;
		}
		set
		{
			((Control)lblDate).Text = value;
		}
	}

	public decimal Percent
	{
		get
		{
			return _percent;
		}
		set
		{
			_percent = value;
			progressBar1.Value = (int)value;
			if (_percent > 0m)
			{
				((Control)lblTitle).Text = $"{_title} ({_percent}%)";
			}
		}
	}

	public UcSyncDoneResponseDetail()
	{
		try
		{
			InitializeComponent();
			((Control)(object)this).Transelate();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcSyncDoneResponseDetail.cs", 41);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Expected O, but got Unknown
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		lblDate = new Label();
		lblTitle = new Label();
		progressBar1 = new ProgressBar();
		((Control)this).SuspendLayout();
		((Control)lblDate).Cursor = Cursors.Default;
		((Control)lblDate).Font = new Font("B Yekan", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblDate).ForeColor = Color.FromArgb(66, 82, 219);
		((Control)lblDate).Location = new Point(0, 2);
		((Control)lblDate).Margin = new Padding(4, 0, 4, 0);
		((Control)lblDate).Name = "lblDate";
		((Control)lblDate).Size = new Size(126, 23);
		((Control)lblDate).TabIndex = 7;
		((Control)lblDate).Text = "---";
		lblDate.TextAlign = (ContentAlignment)64;
		((Control)lblTitle).Anchor = (AnchorStyles)13;
		((Control)lblTitle).Cursor = Cursors.Default;
		((Control)lblTitle).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblTitle).ForeColor = Color.FromArgb(66, 82, 219);
		((Control)lblTitle).Location = new Point(128, 0);
		((Control)lblTitle).Margin = new Padding(4, 0, 4, 0);
		((Control)lblTitle).Name = "lblTitle";
		((Control)lblTitle).Size = new Size(145, 25);
		((Control)lblTitle).TabIndex = 7;
		((Control)lblTitle).Text = "---";
		lblTitle.TextAlign = (ContentAlignment)16;
		((Control)progressBar1).Anchor = (AnchorStyles)13;
		((Control)progressBar1).Location = new Point(0, 28);
		((Control)progressBar1).Name = "progressBar1";
		progressBar1.RightToLeftLayout = true;
		((Control)progressBar1).Size = new Size(273, 14);
		progressBar1.Step = 1;
		((Control)progressBar1).TabIndex = 8;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)progressBar1);
		((Control)this).Controls.Add((Control)(object)lblTitle);
		((Control)this).Controls.Add((Control)(object)lblDate);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcSyncDoneResponseDetail";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(273, 43);
		((Control)this).ResumeLayout(false);
	}
}
