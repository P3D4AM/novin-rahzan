using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Design.Controls;

[ToolboxItem(false)]
public class StiTextBoxButton : TextBox
{
	private static bool IsWindowsVistaOrGreater = Environment.OSVersion.Version.Major >= 6 && Environment.OSVersion.Version.Minor >= 1;

	private StiOperationButton Button { get; }

	public override Size MinimumSize
	{
		get
		{
			return new Size(0, 24);
		}
		set
		{
			((Control)this).MinimumSize = value;
		}
	}

	public event EventHandler ButtonClick
	{
		add
		{
			((Control)Button).Click += value;
		}
		remove
		{
			((Control)Button).Click -= value;
		}
	}

	protected override void OnResize(EventArgs e)
	{
		((Control)this).OnResize(e);
		int height = ((Control)this).ClientSize.Height;
		Rectangle rectangle = new Rectangle(((Control)this).ClientSize.Width - height, 0, height, height);
		if (IsWindowsVistaOrGreater)
		{
			rectangle.Inflate(1, 1);
		}
		((Control)Button).Location = rectangle.Location;
		((Control)Button).Size = rectangle.Size;
		SendMessage(((Control)this).Handle, 211, (IntPtr)2, (IntPtr)(((Control)Button).Width << 16));
	}

	[DllImport("user32.dll")]
	private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

	public StiTextBoxButton()
	{
		StiOperationButton stiOperationButton = new StiOperationButton();
		((Control)stiOperationButton).Cursor = Cursors.Default;
		((ButtonBase)stiOperationButton).FlatStyle = (FlatStyle)2;
		((Control)stiOperationButton).BackColor = Color.Transparent;
		((Control)stiOperationButton).TabStop = false;
		Button = stiOperationButton;
		((Control)Button).SizeChanged += delegate(object o, EventArgs e)
		{
			((Control)this).OnResize(e);
		};
		((Control)this).Controls.Add((Control)(object)Button);
	}
}
