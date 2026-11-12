using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiDotSeparator : Control
{
	private IContainer components;

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		if (((Control)this).Width <= 6)
		{
			return;
		}
		Graphics graphics = e.Graphics;
		Pen val = new Pen(StiUX.Separator);
		try
		{
			if (!StiUX.IsDark)
			{
				val.DashPattern = new float[2] { 1f, 2f };
			}
			graphics.DrawLine(val, 0, ((Control)this).Height / 2, ((Control)this).Width, ((Control)this).Height / 2);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public StiDotSeparator()
	{
		((Control)this).MinimumSize = new Size(0, 5);
		((Control)this).MaximumSize = new Size(0, 5);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Control)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
	}
}
