using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Ribbon;

internal class QatToolbar : ItemControl
{
	private CaptionItemContainer m_ItemContainer;

	private ElementStyle m_DefaultBackgroundStyle = new ElementStyle();

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies the visual style of the control.")]
	[DefaultValue(eDotNetBarStyle.Office2007)]
	public eDotNetBarStyle Style
	{
		get
		{
			return m_ItemContainer.Style;
		}
		set
		{
			base.ColorScheme.Style = value;
			m_ItemContainer.Style = value;
			((Control)this).Invalidate();
			RecalcLayout();
		}
	}

	internal eDotNetBarStyle EffectiveStyle
	{
		get
		{
			if (Style == eDotNetBarStyle.StyleManagerControlled)
			{
				return StyleManager.GetEffectiveStyle();
			}
			return Style;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public SubItemsCollection Items => m_ItemContainer.SubItems;

	public QatToolbar()
	{
		m_ItemContainer = new CaptionItemContainer();
		m_ItemContainer.GlobalItem = false;
		m_ItemContainer.ContainerControl = this;
		m_ItemContainer.WrapItems = false;
		m_ItemContainer.EventHeight = false;
		m_ItemContainer.UseMoreItemsButton = false;
		m_ItemContainer.Stretch = true;
		m_ItemContainer.Displayed = true;
		m_ItemContainer.SystemContainer = true;
		m_ItemContainer.PaddingTop = 0;
		m_ItemContainer.PaddingBottom = 0;
		m_ItemContainer.ItemSpacing = 0;
		m_ItemContainer.SetOwner(this);
		m_ItemContainer.PaddingBottom = 0;
		m_ItemContainer.PaddingTop = 0;
		m_ItemContainer.ItemSpacing = 1;
		m_ItemContainer.TrackSubItemsImageSize = false;
		SetBaseItemContainer(m_ItemContainer);
		m_ItemContainer.Style = eDotNetBarStyle.Office2007;
	}

	protected override ElementStyle GetBackgroundStyle()
	{
		if (base.BackgroundStyle.Custom)
		{
			return base.GetBackgroundStyle();
		}
		return m_DefaultBackgroundStyle;
	}

	protected override void RecalcSize()
	{
		InitDefaultStyles();
		base.RecalcSize();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		InitDefaultStyles();
		base.OnPaint(e);
	}

	private void InitDefaultStyles()
	{
		RibbonPredefinedColorSchemes.ApplyQatElementStyle(m_DefaultBackgroundStyle, this);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 2097152 && ((Control)this).Parent is RibbonControl)
		{
			((RibbonControl)(object)((Control)this).Parent).ShowCustomizeContextMenu(HitTest(e.X, e.Y), ribbonStrip: true);
		}
		base.OnMouseDown(e);
	}

	protected override bool ProcessMnemonic(char charCode)
	{
		if (!ShowKeyTips)
		{
			return false;
		}
		return base.ProcessMnemonic(charCode);
	}

	public override ArrayList GetItems(string ItemName)
	{
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl != null && ribbonControl.RibbonStrip != null)
		{
			return ribbonControl.RibbonStrip.GetItems(ItemName);
		}
		return base.GetItems(ItemName);
	}

	public override ArrayList GetItems(string ItemName, Type itemType)
	{
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl != null && ribbonControl.RibbonStrip != null)
		{
			return ribbonControl.RibbonStrip.GetItems(ItemName, itemType);
		}
		return base.GetItems(ItemName, itemType);
	}

	public override ArrayList GetItems(string ItemName, Type itemType, bool useGlobalName)
	{
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl != null && ribbonControl.RibbonStrip != null)
		{
			return ribbonControl.RibbonStrip.GetItems(ItemName, itemType, useGlobalName);
		}
		return base.GetItems(ItemName, itemType, useGlobalName);
	}

	public override BaseItem GetItem(string ItemName)
	{
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl != null && ribbonControl.RibbonStrip != null)
		{
			return ribbonControl.RibbonStrip.GetItem(ItemName);
		}
		return base.GetItem(ItemName);
	}
}
