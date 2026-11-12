using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.Controls;

[ToolboxItem(false)]
public class StiZoneControl : UserControl
{
	private Container components;

	[DefaultValue(false)]
	private bool isDragOver;

	private StiSimpleButton closeButton;

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

	[DefaultValue(true)]
	public bool DrawBorder { get; set; } = true;


	[DefaultValue(false)]
	public bool ShowClose { get; set; }

	internal bool IsDragOver
	{
		get
		{
			return isDragOver;
		}
		set
		{
			if (isDragOver != value)
			{
				isDragOver = value;
				((Control)this).Invalidate();
			}
		}
	}

	public event EventHandler Closed;

	public StiZoneControl()
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
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).DoubleBuffered = true;
		((Control)this).Margin = new Padding(2, 2, 2, 2);
		((Control)this).Name = "StiZoneControl";
		((Control)this).Size = new Size(368, 58);
		((Control)this).ResumeLayout(false);
	}

	private void OnUpdateLayout(object sender, EventArgs e)
	{
		SetBackColor();
	}

	public virtual void SetBackColor()
	{
		((Control)this).BackColor = Color.White;
	}

	private void AddCloseButton()
	{
		if (ShowClose)
		{
			if (closeButton == null)
			{
				closeButton = new StiSimpleButton();
				closeButton.BackColor = StiUX.ButtonBackground;
				int num = StiScale.XXI(16.0);
				((Control)closeButton).Anchor = (AnchorStyles)9;
				((ButtonBase)closeButton).Image = StiReportDesignImages.SystemParts.Remove(StiImageSize.Normal, allowCache: false);
				StiSimpleButton stiSimpleButton = closeButton;
				int width = (((Control)closeButton).Height = num);
				((Control)stiSimpleButton).Width = width;
				((Control)closeButton).Location = new Point(((Control)this).ClientRectangle.Right - num - StiScale.I3, StiScale.I3);
				((Control)closeButton).Click += CloseButton_Click;
				((Control)this).Controls.Add((Control)(object)closeButton);
				((Control)this).Controls.SetChildIndex((Control)(object)closeButton, 0);
			}
			else
			{
				((Control)closeButton).Visible = true;
			}
		}
	}

	private void CloseButton_Click(object sender, EventArgs e)
	{
		InvokeClosed();
	}

	private void HideCloseButton()
	{
		if (closeButton != null)
		{
			((Control)closeButton).Visible = false;
		}
	}

	internal void InvokeClosed()
	{
		this.Closed?.Invoke(this, null);
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
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		clientRectangle.X++;
		clientRectangle.Y++;
		clientRectangle.Width -= 3;
		clientRectangle.Height -= 3;
		SolidBrush val = new SolidBrush(GetLightBaseColor());
		try
		{
			Pen val2 = new Pen(StiUX.IsDark ? StiUX.ButtonBorder : GetBaseColor());
			try
			{
				graphics.FillRectangle((Brush)(object)val, clientRectangle.X + 1, clientRectangle.Y + 1, clientRectangle.Right - 1, clientRectangle.Bottom - 1);
				if (DrawBorder)
				{
					graphics.DrawRectangle(val2, clientRectangle.X, clientRectangle.Y, clientRectangle.Right - 1, clientRectangle.Bottom - 1);
				}
				if (isDragOver)
				{
					graphics.DrawRectangle(Pens.DarkGray, clientRectangle.X, clientRectangle.Y, clientRectangle.Right - 1, clientRectangle.Bottom - 1);
				}
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
		if (StiUX.IsDark)
		{
			if (!IsFocused)
			{
				return StiUX.BackgroundDark;
			}
			return StiUX.ItemBackgroundHover;
		}
		return StiColorUtils.Dark(Color.White, (byte)(IsFocused ? 30u : 20u));
	}

	protected Color GetLightBaseColor()
	{
		if (StiUX.IsDark)
		{
			return GetBaseColor();
		}
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
		HideCloseButton();
		((Control)this).Invalidate();
	}

	protected override void OnEnter(EventArgs e)
	{
		((Control)this).OnEnter(e);
		SetBackColor();
		AddCloseButton();
		((Control)this).Invalidate();
	}
}
