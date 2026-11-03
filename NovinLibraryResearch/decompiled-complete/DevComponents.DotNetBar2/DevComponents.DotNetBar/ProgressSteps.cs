using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

[ToolboxBitmap(typeof(ProgressSteps), "ProgressSteps.ico")]
[ToolboxItem(true)]
[ComVisible(false)]
[DefaultEvent("ItemClick")]
[Designer("DevComponents.DotNetBar.Design.ProgressStepsDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class ProgressSteps : ItemControl
{
	private StepItemContainer __21CE;

	private Size __21D2 = Size.Empty;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public SubItemsCollection Items => __21CE.SubItems;

	[DevCoBrowsable(true)]
	[DefaultValue(eDotNetBarStyle.StyleManagerControlled)]
	[Description("Specifies the visual style of the control.")]
	[Browsable(true)]
	[Category("Appearance")]
	public eDotNetBarStyle Style
	{
		get
		{
			return __21CE.Style;
		}
		set
		{
			base.ColorScheme._0875(value);
			__21CE.Style = value;
			((Control)this).Invalidate();
			RecalcLayout();
		}
	}

	[Browsable(false)]
	[Localizable(true)]
	public Padding Padding
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).Padding;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Padding = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Always)]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[DefaultValue(false)]
	public override bool AutoSize
	{
		get
		{
			return base.AutoSize;
		}
		set
		{
			if (((Control)this).AutoSize != value)
			{
				base.AutoSize = value;
				_21DE();
				_21DF();
			}
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Image BackgroundImage
	{
		get
		{
			return ((Control)this).BackgroundImage;
		}
		set
		{
			((Control)this).BackgroundImage = value;
		}
	}

	[Category("Appearance")]
	[DefaultValue(10)]
	[Description("Gets or sets the arrow pointer width for the StepItem objects hosted within this container.")]
	public int PointerSize
	{
		get
		{
			return __21CE.PointerSize;
		}
		set
		{
			__21CE.PointerSize = value;
		}
	}

	[Browsable(false)]
	public override eBarImageSize ImageSize
	{
		get
		{
			return base.ImageSize;
		}
		set
		{
			base.ImageSize = value;
		}
	}

	[Browsable(false)]
	public override ImageList ImagesLarge
	{
		get
		{
			return base.ImagesLarge;
		}
		set
		{
			base.ImagesLarge = value;
		}
	}

	[Browsable(false)]
	public override ImageList ImagesMedium
	{
		get
		{
			return base.ImagesMedium;
		}
		set
		{
			base.ImagesMedium = value;
		}
	}

	[Browsable(false)]
	public override Font KeyTipsFont
	{
		get
		{
			return base.KeyTipsFont;
		}
		set
		{
			base.KeyTipsFont = value;
		}
	}

	[Browsable(false)]
	public override bool ShowShortcutKeysInToolTips
	{
		get
		{
			return base.ShowShortcutKeysInToolTips;
		}
		set
		{
			base.ShowShortcutKeysInToolTips = value;
		}
	}

	[Browsable(false)]
	public override bool ThemeAware
	{
		get
		{
			return base.ThemeAware;
		}
		set
		{
			base.ThemeAware = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override string Text
	{
		get
		{
			return ((Control)this).Text;
		}
		set
		{
			((Control)this).Text = value;
		}
	}

	public ProgressSteps()
	{
		__21CE = new StepItemContainer();
		__21CE.GlobalItem = false;
		__21CE.ContainerControl = this;
		__21CE.Displayed = true;
		__21CE.Style = eDotNetBarStyle.StyleManagerControlled;
		__21CE.SetOwner(this);
		SetBaseItemContainer(__21CE);
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		if (!__21D2.IsEmpty)
		{
			return __21D2;
		}
		if (!BarFunctions.IsHandleValid((Control)(object)this))
		{
			return ((Control)this).GetPreferredSize(proposedSize);
		}
		if (Items.Count == 0 || !BarFunctions.IsHandleValid((Control)(object)this) || __21CE.SubItems.Count == 0)
		{
			return new Size(((Control)this).GetPreferredSize(proposedSize).Width, Dpi.Height22);
		}
		int height = ⰵ();
		__21D2 = new Size(proposedSize.Width, height);
		return __21D2;
	}

	private int ⰵ()
	{
		int num = ᝈ.ᝏ(GetBackgroundStyle());
		return num + ((__21CE._21E4 > 0) ? __21CE._21E4 : 20);
	}

	protected override void RecalcSize()
	{
		base.RecalcSize();
		if (((Control)this).AutoSize && ((Control)this).IsHandleCreated && ⰵ() != ((Control)this).Height)
		{
			_21DE();
			_21DF();
		}
	}

	private void _21DE()
	{
		__21D2 = Size.Empty;
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		if (((Control)this).AutoSize)
		{
			Size preferredSize = ((Control)this).PreferredSize;
			if (preferredSize.Width > 0)
			{
				width = preferredSize.Width;
			}
			if (preferredSize.Height > 0)
			{
				height = preferredSize.Height;
			}
		}
		((Control)this).SetBoundsCore(x, y, width, height, specified);
	}

	private void _21DF()
	{
		if (((Control)this).AutoSize)
		{
			Size preferredSize = ((Control)this).PreferredSize;
			if (preferredSize.Width > 0 && preferredSize.Height > 0)
			{
				((Control)this).Size = ((Control)this).PreferredSize;
			}
			else if (preferredSize.Height > 0)
			{
				((Control)this).Size = new Size(((Control)this).Width, ((Control)this).PreferredSize.Height);
			}
		}
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);
		if (((Control)this).AutoSize)
		{
			_21DF();
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);
		if (_089F.फ() || !_089F.थ)
		{
			e.Graphics.Clear(SystemColors.Control);
		}
	}
}
