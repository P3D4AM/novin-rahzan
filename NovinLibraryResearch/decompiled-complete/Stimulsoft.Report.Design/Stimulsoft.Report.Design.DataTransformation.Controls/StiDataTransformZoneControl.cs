using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Design.DataTransformation.Controls;

[ToolboxItem(false)]
public class StiDataTransformZoneControl : UserControl
{
	private IContainer components;

	public bool IsFocused
	{
		get
		{
			if (!((Control)this).Focused)
			{
				return ((Control)this).ContainsFocus;
			}
			return true;
		}
	}

	public StiDataTransformZoneControl()
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			((Control)this).SetStyle((ControlStyles)512, true);
			((Control)this).SetStyle((ControlStyles)16, true);
			((Control)this).SetStyle((ControlStyles)8192, true);
			((Control)this).SetStyle((ControlStyles)2, true);
			((Control)this).SetStyle((ControlStyles)1, true);
		}
	}

	private void OnUpdateLayout(object sender, EventArgs e)
	{
		SetBackColor();
	}

	public virtual void SetBackColor()
	{
		((Control)this).BackColor = Color.White;
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		((Control)this).OnControlAdded(e);
		e.Control.Leave += OnUpdateLayout;
		e.Control.Enter += OnUpdateLayout;
	}

	protected override void OnControlRemoved(ControlEventArgs e)
	{
		((Control)this).OnControlRemoved(e);
		e.Control.Leave -= OnUpdateLayout;
		e.Control.Enter -= OnUpdateLayout;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		clientRectangle.X++;
		clientRectangle.Y++;
		clientRectangle.Width -= 3;
		clientRectangle.Height -= 3;
		SolidBrush val = new SolidBrush(GetLightBaseColor());
		try
		{
			Pen val2 = new Pen(GetBaseColor());
			try
			{
				graphics.FillRectangle((Brush)(object)val, clientRectangle.X + 1, clientRectangle.Y + 1, clientRectangle.Right - 1, clientRectangle.Bottom - 1);
				graphics.DrawRectangle(val2, clientRectangle.X, clientRectangle.Y, clientRectangle.Right, clientRectangle.Bottom);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected Color GetBaseColor()
	{
		return StiColorUtils.Dark(Color.White, (byte)(IsFocused ? 30u : 20u));
	}

	protected Color GetLightBaseColor()
	{
		return StiColorUtils.Light(GetBaseColor(), 20);
	}

	protected override void OnClick(EventArgs e)
	{
		((Control)this).OnClick(e);
		((Control)this).Focus();
		SetBackColor();
		((Control)this).Invalidate();
	}

	protected override void OnLeave(EventArgs e)
	{
		((Control)this).OnLeave(e);
		SetBackColor();
		((Control)this).Invalidate();
	}

	protected override void OnEnter(EventArgs e)
	{
		((Control)this).OnEnter(e);
		SetBackColor();
		((Control)this).Invalidate();
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
		components = new Container();
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
	}
}
