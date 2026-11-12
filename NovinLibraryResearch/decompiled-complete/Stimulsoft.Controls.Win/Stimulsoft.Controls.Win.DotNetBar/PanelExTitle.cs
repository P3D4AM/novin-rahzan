using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public class PanelExTitle : PanelEx
{
	private ItemContainer m_ItemContainer;

	private ButtonItem m_ExpandChangeButton;

	private int m_ButtonMargin = 3;

	private eTitleButtonAlignment m_ButtonAlignment = eTitleButtonAlignment.Right;

	private bool m_DrawText = true;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ButtonItem ExpandChangeButton => m_ExpandChangeButton;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool DrawText
	{
		get
		{
			return m_DrawText;
		}
		set
		{
			m_DrawText = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Category("Expand Button")]
	[Description("Indicates whether expand button is visible or not.")]
	public bool ExpandButtonVisible
	{
		get
		{
			return m_ExpandChangeButton.Visible;
		}
		set
		{
			m_ExpandChangeButton.Visible = value;
			UpdateButtonPosition();
			((Control)this).Invalidate();
		}
	}

	[Browsable(false)]
	[DefaultValue(eTitleButtonAlignment.Right)]
	[Category("Expand Button")]
	[Description("Indicates the alignment of button.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public eTitleButtonAlignment ButtonAlignment
	{
		get
		{
			return m_ButtonAlignment;
		}
		set
		{
			m_ButtonAlignment = value;
			UpdateButtonPosition();
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Indicates whether text markup if it occupies less space than control provides uses the Style Alignment and LineAlignment properties to align the markup inside of the control.")]
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

	public event EventHandler ExpandedClick;

	public PanelExTitle()
	{
		m_ExpandChangeButton = new ButtonItem("close");
		m_ExpandChangeButton.Style = eDotNetBarStyle.Office2003;
		m_ExpandChangeButton.Image = (Image)(object)BarFunctions.LoadBitmap("SystemImages.CollapseTitle.png");
		m_ExpandChangeButton.Displayed = true;
		m_ExpandChangeButton.Click += m_ExpandChangeButton_Click;
		m_ItemContainer = new ItemContainer();
		m_ItemContainer.ContainerControl = this;
		m_ItemContainer.Style = eDotNetBarStyle.Office2003;
		m_ItemContainer.SubItems.Add(m_ExpandChangeButton);
		base.MarkupUsesStyleAlignment = true;
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		base.OnMouseEnter(e);
		m_ItemContainer.InternalMouseEnter();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		base.OnMouseLeave(e);
		m_ItemContainer.InternalMouseLeave();
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		base.OnMouseDown(e);
		m_ItemContainer.InternalMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		base.OnMouseUp(e);
		m_ItemContainer.InternalMouseUp(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);
		m_ItemContainer.InternalMouseMove(e);
	}

	protected override void OnClick(EventArgs e)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		Point point = ((Control)this).PointToClient(Control.MousePosition);
		m_ItemContainer.InternalClick(Control.MouseButtons, new Point(point.X, point.Y));
		base.OnClick(e);
	}

	protected override void OnResize(EventArgs e)
	{
		base.OnResize(e);
		UpdateButtonPosition();
	}

	public void UpdateButtonPosition()
	{
		m_ExpandChangeButton.FixedSize = Size.Empty;
		m_ItemContainer.NeedRecalcSize = true;
		m_ItemContainer.RecalcSize();
		if (m_ItemContainer.HeightInternal >= ((Control)this).Height && ((Control)this).Height > 8)
		{
			m_ExpandChangeButton.FixedSize = new Size(((Control)this).Height - 2, ((Control)this).Height - 2);
			m_ItemContainer.RecalcSize();
		}
		if (m_ButtonAlignment == eTitleButtonAlignment.Right)
		{
			m_ItemContainer.LeftInternal = ((Control)this).Width - m_ItemContainer.WidthInternal - m_ButtonMargin;
			m_ItemContainer.TopInternal = (((Control)this).Height - m_ItemContainer.HeightInternal) / 2;
		}
		else
		{
			m_ItemContainer.LeftInternal = m_ButtonMargin;
			m_ItemContainer.TopInternal = (((Control)this).Height - m_ItemContainer.HeightInternal) / 2;
		}
		m_ItemContainer.RecalcSize();
		RefreshTextClientRectangle();
	}

	protected override void RefreshTextClientRectangle()
	{
		Rectangle displayRectangle = ((Control)this).DisplayRectangle;
		displayRectangle.Width -= m_ItemContainer.WidthInternal + m_ButtonMargin;
		if (m_ButtonAlignment == eTitleButtonAlignment.Left)
		{
			displayRectangle.X += m_ItemContainer.WidthInternal + m_ButtonMargin;
		}
		if (!m_DrawText)
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
		m_ItemContainer.Paint(p);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		((Control)this).OnHandleCreated(e);
		UpdateButtonPosition();
	}

	private void m_ExpandChangeButton_Click(object sender, EventArgs e)
	{
		InvokeExpandedClick(e);
	}

	protected virtual void InvokeExpandedClick(EventArgs e)
	{
		if (this.ExpandedClick != null)
		{
			this.ExpandedClick(this, e);
		}
	}
}
