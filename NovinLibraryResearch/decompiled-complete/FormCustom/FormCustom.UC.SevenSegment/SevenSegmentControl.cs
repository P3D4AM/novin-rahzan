using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using PacketParser.Entities;

namespace FormCustom.UC.SevenSegment;

public class SevenSegmentControl : UserControl
{
	private CancellationTokenSource _resizing;

	private bool _isDisposed;

	private decimal weight;

	private IContainer components;

	private UcSevenSegment ucSevenSegment1;

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((Control)this).IsDisposed;
			}
			return true;
		}
	}

	public decimal Weight
	{
		get
		{
			return weight;
		}
		set
		{
			if (!IsDisposed)
			{
				ucSevenSegment1.Weight = value;
			}
		}
	}

	public SevenSegmentControl()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_resizing?.Cancel();
				_resizing?.Dispose();
				_resizing = null;
				components?.Dispose();
				((ContainerControl)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\SevenSegment\\SevenSegmentControl.cs", 112);
		}
	}

	private void SevenSegmentControl_SizeChanged(object sender, EventArgs e)
	{
		try
		{
			_resizing?.Cancel();
			_resizing = new CancellationTokenSource();
			_ = _resizing.Token;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SevenSegmentControl_SizeChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\SevenSegment\\SevenSegmentControl.cs", 124);
		}
	}

	private void InitializeComponent()
	{
		ucSevenSegment1 = new UcSevenSegment();
		((Control)this).SuspendLayout();
		((Control)ucSevenSegment1).Dock = (DockStyle)5;
		((Control)ucSevenSegment1).Location = new Point(0, 0);
		((Control)ucSevenSegment1).Name = "ucSevenSegment1";
		((Control)ucSevenSegment1).Size = new Size(721, 428);
		((Control)ucSevenSegment1).TabIndex = 0;
		ucSevenSegment1.Weight = new decimal(new int[4] { 9256, 0, 0, -2147287040 });
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)ucSevenSegment1);
		((Control)this).Name = "SevenSegmentControl";
		((Control)this).Size = new Size(721, 428);
		((Control)this).SizeChanged += SevenSegmentControl_SizeChanged;
		((Control)this).ResumeLayout(false);
	}
}
