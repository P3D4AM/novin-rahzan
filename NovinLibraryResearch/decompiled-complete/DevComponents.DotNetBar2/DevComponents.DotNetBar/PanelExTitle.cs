using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

[EditorBrowsable(EditorBrowsableState.Never)]
[ToolboxItem(false)]
public class PanelExTitle : PanelEx
{
	private ItemContainer ଡ଼;

	private ButtonItem _212E;

	private int ℯ = 3;

	private eTitleButtonAlignment Ḁ = eTitleButtonAlignment.Right;

	private bool ℰ = true;

	private EventHandler ℱ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public ButtonItem ExpandChangeButton => _212E;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool DrawText
	{
		get
		{
			return ℰ;
		}
		set
		{
			ℰ = value;
		}
	}

	[Description("Indicates whether expand button is visible or not.")]
	[Category("Expand Button")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ExpandButtonVisible
	{
		get
		{
			return _212E.Visible;
		}
		set
		{
			_212E.Visible = value;
			UpdateButtonPosition();
			((Control)this).Invalidate();
		}
	}

	[Browsable(false)]
	[Category("Expand Button")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue(eTitleButtonAlignment.Right)]
	[Description("Indicates the alignment of button.")]
	public eTitleButtonAlignment ButtonAlignment
	{
		get
		{
			return Ḁ;
		}
		set
		{
			Ḁ = value;
			UpdateButtonPosition();
			((Control)this).Invalidate();
		}
	}

	[Description("Indicates whether text markup if it occupies less space than control provides uses the Style Alignment and LineAlignment properties to align the markup inside of the control.")]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Browsable(true)]
	public override bool MarkupUsesStyleAlignment
	{
		get
		{
			return base.MarkupUsesStyleAlignment;
		}
		set
		{
			base.MarkupUsesStyleAlignment = value;
		}
	}

	public event EventHandler ExpandedClick
	{
		add
		{
			EventHandler eventHandler = ℱ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ℱ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ℱ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ℱ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public PanelExTitle()
	{
		_212E = new ButtonItem("close");
		_212E.Style = eDotNetBarStyle.Office2003;
		_212E.Image = (Image)(object)BarFunctions.ই("SystemImages.CollapseTitle.png");
		_212E.Displayed = true;
		_212E.Click += Ⅎ;
		ଡ଼ = new ItemContainer();
		ଡ଼.ContainerControl = this;
		ଡ଼.Style = eDotNetBarStyle.Office2003;
		ଡ଼.SubItems.Add(_212E);
		base.MarkupUsesStyleAlignment = true;
	}

	protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		((ScrollableControl)this).ScaleControl(factor, specified);
		ଡ଼.NotifyScaleItem(factor);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		base.OnMouseEnter(e);
		ଡ଼.InternalMouseEnter();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		base.OnMouseLeave(e);
		ଡ଼.InternalMouseLeave();
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		base.OnMouseDown(e);
		ଡ଼.InternalMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		base.OnMouseUp(e);
		ଡ଼.InternalMouseUp(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);
		ଡ଼.InternalMouseMove(e);
	}

	protected override void OnClick(EventArgs e)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		Point point = ((Control)this).PointToClient(Control.MousePosition);
		ଡ଼.InternalClick(Control.MouseButtons, new Point(point.X, point.Y));
		base.OnClick(e);
	}

	protected override void OnResize(EventArgs e)
	{
		base.OnResize(e);
		if (BarFunctions.IsHandleValid((Control)(object)this))
		{
			UpdateButtonPosition();
		}
	}

	public void UpdateButtonPosition()
	{
		_212E.FixedSize = Size.Empty;
		ଡ଼.NeedRecalcSize = true;
		ଡ଼.RecalcSize();
		if (ଡ଼.HeightInternal >= ((Control)this).Height && ((Control)this).Height > 8)
		{
			_212E.FixedSize = new Size(((Control)this).Height - 2, ((Control)this).Height - 2);
			ଡ଼.RecalcSize();
		}
		if (Ḁ == eTitleButtonAlignment.Right)
		{
			ଡ଼.LeftInternal = ((Control)this).Width - ଡ଼.WidthInternal - ℯ;
			ଡ଼.TopInternal = (((Control)this).Height - ଡ଼.HeightInternal) / 2;
		}
		else
		{
			ଡ଼.LeftInternal = ℯ;
			ଡ଼.TopInternal = (((Control)this).Height - ଡ଼.HeightInternal) / 2;
		}
		ଡ଼.RecalcSize();
		ଡ଼.Displayed = true;
		RefreshTextClientRectangle();
	}

	protected override void RefreshTextClientRectangle()
	{
		Rectangle displayRectangle = ((Control)this).DisplayRectangle;
		displayRectangle.Width -= ଡ଼.WidthInternal + ℯ;
		if (Ḁ == eTitleButtonAlignment.Left)
		{
			displayRectangle.X += ଡ଼.WidthInternal + ℯ;
		}
		if (!ℰ)
		{
			displayRectangle.Width = 0;
		}
		base.ClientTextRectangle = displayRectangle;
		ResizeMarkup();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);
		ItemPaintArgs p = new ItemPaintArgs(null, (Control)(object)this, e.Graphics, base.ColorScheme);
		ଡ଼.Paint(p);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);
		UpdateButtonPosition();
	}

	private void Ⅎ(object _٠, EventArgs ؿ)
	{
		InvokeExpandedClick(ؿ);
	}

	protected virtual void InvokeExpandedClick(EventArgs e)
	{
		if (ℱ != null)
		{
			ℱ(this, e);
		}
	}
}
