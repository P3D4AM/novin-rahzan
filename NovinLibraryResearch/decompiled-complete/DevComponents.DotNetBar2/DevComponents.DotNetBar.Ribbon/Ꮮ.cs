using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Ribbon;

internal class Ꮮ : ItemControl
{
	private ᱭ ଡ଼;

	private ElementStyle ൿ = new ElementStyle();

	[DefaultValue(eDotNetBarStyle.Office2007)]
	[Category("Appearance")]
	[Description("Specifies the visual style of the control.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public eDotNetBarStyle ߕ
	{
		get
		{
			return ଡ଼.Style;
		}
		set
		{
			base.ColorScheme.ߕ = value;
			ଡ଼.Style = value;
			((Control)this).Invalidate();
			RecalcLayout();
		}
	}

	internal eDotNetBarStyle _൨
	{
		get
		{
			if (ߕ == eDotNetBarStyle.StyleManagerControlled)
			{
				return StyleManager.GetEffectiveStyle();
			}
			return ߕ;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public SubItemsCollection ᯥ => ଡ଼.SubItems;

	public Ꮮ()
	{
		ଡ଼ = new ᱭ();
		ଡ଼.GlobalItem = false;
		ଡ଼.ContainerControl = this;
		ଡ଼.WrapItems = false;
		ଡ଼._0DBE = false;
		ଡ଼._0DBF = false;
		ଡ଼.Stretch = true;
		ଡ଼.Displayed = true;
		ଡ଼.SystemContainer = true;
		ଡ଼.PaddingTop = 0;
		ଡ଼.PaddingBottom = 0;
		ଡ଼.ItemSpacing = 0;
		ଡ଼.SetOwner(this);
		ଡ଼.PaddingBottom = 0;
		ଡ଼.PaddingTop = 0;
		ଡ଼.ItemSpacing = 1;
		ଡ଼.ශ = false;
		SetBaseItemContainer(ଡ଼);
		ଡ଼.Style = eDotNetBarStyle.Office2007;
	}

	protected override ElementStyle GetBackgroundStyle()
	{
		if (base.BackgroundStyle.Custom)
		{
			return base.GetBackgroundStyle();
		}
		return ൿ;
	}

	protected override void RecalcSize()
	{
		ඕ();
		base.RecalcSize();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		ඕ();
		base.OnPaint(e);
	}

	private void ඕ()
	{
		RibbonPredefinedColorSchemes._2106(ൿ, this);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 2097152 && ((Control)this).Parent is RibbonControl)
		{
			((RibbonControl)(object)((Control)this).Parent).ᐆ(HitTest(e.X, e.Y), ᱩ: true);
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
