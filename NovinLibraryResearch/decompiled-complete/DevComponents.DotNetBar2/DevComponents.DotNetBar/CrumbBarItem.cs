using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace DevComponents.DotNetBar;

[Designer("DevComponents.DotNetBar.Design.CrumbBarItemDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class CrumbBarItem : ButtonItem
{
	private bool _ԅ;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool ShowSubItems
	{
		get
		{
			return false;
		}
		set
		{
			base.ShowSubItems = value;
		}
	}

	[Browsable(false)]
	public bool IsSelected
	{
		get
		{
			return _ԅ;
		}
		internal set
		{
			_ԅ = value;
			if (_ԅ)
			{
				FontBold = true;
			}
			else
			{
				FontBold = false;
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	[Editor("DevComponents.DotNetBar.Design.CrumbBarItemCollectionEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	public override SubItemsCollection SubItems => base.SubItems;

	[Browsable(false)]
	public string FullPath
	{
		get
		{
			if (!(GetOwner() is CrumbBar crumbBar))
			{
				return "";
			}
			return CrumbBar._175E(this, crumbBar.PathSeparator);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override string AlternateShortCutText
	{
		get
		{
			return base.AlternateShortCutText;
		}
		set
		{
			base.AlternateShortCutText = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool AutoCheckOnClick
	{
		get
		{
			return base.AutoCheckOnClick;
		}
		set
		{
			base.AutoCheckOnClick = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override bool AutoCollapseOnClick
	{
		get
		{
			return base.AutoCollapseOnClick;
		}
		set
		{
			base.AutoCollapseOnClick = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool AutoExpandOnClick
	{
		get
		{
			return base.AutoExpandOnClick;
		}
		set
		{
			base.AutoExpandOnClick = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override bool BeginGroup
	{
		get
		{
			return base.BeginGroup;
		}
		set
		{
			base.BeginGroup = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override eButtonStyle ButtonStyle
	{
		get
		{
			return base.ButtonStyle;
		}
		set
		{
			base.ButtonStyle = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override bool CanCustomize
	{
		get
		{
			return base.CanCustomize;
		}
		set
		{
			base.CanCustomize = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override string Category
	{
		get
		{
			return base.Category;
		}
		set
		{
			base.Category = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override bool Checked
	{
		get
		{
			return base.Checked;
		}
		set
		{
			base.Checked = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool ClickAutoRepeat
	{
		get
		{
			return base.ClickAutoRepeat;
		}
		set
		{
			base.ClickAutoRepeat = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override int ClickRepeatInterval
	{
		get
		{
			return base.ClickRepeatInterval;
		}
		set
		{
			base.ClickRepeatInterval = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override eButtonColor ColorTable
	{
		get
		{
			return base.ColorTable;
		}
		set
		{
			base.ColorTable = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override string CustomColorName
	{
		get
		{
			return base.CustomColorName;
		}
		set
		{
			base.CustomColorName = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override string Description
	{
		get
		{
			return base.Description;
		}
		set
		{
			base.Description = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override bool FontBold
	{
		get
		{
			return base.FontBold;
		}
		set
		{
			base.FontBold = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool FontItalic
	{
		get
		{
			return base.FontItalic;
		}
		set
		{
			base.FontItalic = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool FontUnderline
	{
		get
		{
			return base.FontUnderline;
		}
		set
		{
			base.FontUnderline = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool GlobalItem
	{
		get
		{
			return base.GlobalItem;
		}
		set
		{
			base.GlobalItem = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override string GlobalName
	{
		get
		{
			return base.GlobalName;
		}
		set
		{
			base.GlobalName = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool HotFontBold
	{
		get
		{
			return base.HotFontBold;
		}
		set
		{
			base.HotFontBold = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool HotFontUnderline
	{
		get
		{
			return base.HotFontUnderline;
		}
		set
		{
			base.HotFontUnderline = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override eHotTrackingStyle HotTrackingStyle
	{
		get
		{
			return base.HotTrackingStyle;
		}
		set
		{
			base.HotTrackingStyle = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Image HoverImage
	{
		get
		{
			return base.HoverImage;
		}
		set
		{
			base.HoverImage = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override int HoverImageIndex
	{
		get
		{
			return base.HoverImageIndex;
		}
		set
		{
			base.HoverImageIndex = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Icon Icon
	{
		get
		{
			return base.Icon;
		}
		set
		{
			base.Icon = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override eButtonImageListSelection ImageListSizeSelection
	{
		get
		{
			return base.ImageListSizeSelection;
		}
		set
		{
			base.ImageListSizeSelection = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override int ImagePaddingHorizontal
	{
		get
		{
			return base.ImagePaddingHorizontal;
		}
		set
		{
			base.ImagePaddingHorizontal = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override int ImagePaddingVertical
	{
		get
		{
			return base.ImagePaddingVertical;
		}
		set
		{
			base.ImagePaddingVertical = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override eImagePosition ImagePosition
	{
		get
		{
			return base.ImagePosition;
		}
		set
		{
			base.ImagePosition = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Image ImageSmall
	{
		get
		{
			return base.ImageSmall;
		}
		set
		{
			base.ImageSmall = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override eItemAlignment ItemAlignment
	{
		get
		{
			return base.ItemAlignment;
		}
		set
		{
			base.ItemAlignment = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override string KeyTips
	{
		get
		{
			return base.KeyTips;
		}
		set
		{
			base.KeyTips = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override eMenuVisibility MenuVisibility
	{
		get
		{
			return base.MenuVisibility;
		}
		set
		{
			base.MenuVisibility = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override string OptionGroup
	{
		get
		{
			return base.OptionGroup;
		}
		set
		{
			base.OptionGroup = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override ePersonalizedMenus PersonalizedMenus
	{
		get
		{
			return base.PersonalizedMenus;
		}
		set
		{
			base.PersonalizedMenus = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override ePopupSide PopupSide
	{
		get
		{
			return base.PopupSide;
		}
		set
		{
			base.PopupSide = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override ePopupType PopupType
	{
		get
		{
			return base.PopupType;
		}
		set
		{
			base.PopupType = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override int PopupWidth
	{
		get
		{
			return base.PopupWidth;
		}
		set
		{
			base.PopupWidth = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Image PressedImage
	{
		get
		{
			return base.PressedImage;
		}
		set
		{
			base.PressedImage = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override int PressedImageIndex
	{
		get
		{
			return base.PressedImageIndex;
		}
		set
		{
			base.PressedImageIndex = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool RibbonWordWrap
	{
		get
		{
			return base.RibbonWordWrap;
		}
		set
		{
			base.RibbonWordWrap = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override ShapeDescriptor Shape
	{
		get
		{
			return base.Shape;
		}
		set
		{
			base.Shape = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override bool SplitButton
	{
		get
		{
			return base.SplitButton;
		}
		set
		{
			base.SplitButton = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool Stretch
	{
		get
		{
			return base.Stretch;
		}
		set
		{
			base.Stretch = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override int SubItemsExpandWidth
	{
		get
		{
			return base.SubItemsExpandWidth;
		}
		set
		{
			base.SubItemsExpandWidth = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override Size FixedSize
	{
		get
		{
			return base.FixedSize;
		}
		set
		{
			base.FixedSize = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override int PulseSpeed
	{
		get
		{
			return base.PulseSpeed;
		}
		set
		{
			base.PulseSpeed = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override bool StopPulseOnMouseOver
	{
		get
		{
			return base.StopPulseOnMouseOver;
		}
		set
		{
			base.StopPulseOnMouseOver = value;
		}
	}

	protected override void OnClick()
	{
		_21E0(eEventSource.Mouse);
		base.OnClick();
	}

	private void _21E0(eEventSource ᎂ)
	{
		if (GetOwner() is CrumbBar crumbBar)
		{
			crumbBar.SetSelectedItem(this, ᎂ);
		}
	}

	public override void OnImageChanged()
	{
		base.OnImageChanged();
		if (GetOwner() is CrumbBar crumbBar && crumbBar.SelectedItem == this)
		{
			crumbBar._21D7();
		}
	}

	protected override void OnParentChanged()
	{
		if (GetOwner() is CrumbBar crumbBar && (crumbBar.SelectedItem == this || crumbBar.GetIsInSelectedPath(this)))
		{
			crumbBar.SetSelectedItem(crumbBar.SelectedItem, eEventSource.Code);
		}
		base.OnParentChanged();
	}

	protected internal override void OnAfterItemRemoved(BaseItem objItem, int itemIndex)
	{
		if (objItem is CrumbBarItem && ((CrumbBarItem)objItem).IsSelected && GetOwner() is CrumbBar crumbBar)
		{
			crumbBar.SetSelectedItem(this, eEventSource.Code);
		}
		base.OnAfterItemRemoved(objItem, itemIndex);
	}
}
