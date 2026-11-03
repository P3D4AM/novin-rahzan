using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class RibbonStripContainerItem : ImageItem, IDesignTimeProvider
{
	private ය ଡ଼;

	private ᱭ න;

	private SystemCaptionItem _0DB2;

	private RibbonStrip ඳ;

	private SystemCaptionItem _ප;

	private Separator _ඵ;

	private List<BaseItem> _බ = new List<BaseItem>();

	private bool _භ;

	public bool MetroBackstageOpen
	{
		get
		{
			return _භ;
		}
		internal set
		{
			if (_භ == value)
			{
				return;
			}
			_භ = value;
			if (value)
			{
				foreach (BaseItem subItem in න.SubItems)
				{
					if (subItem.Visible && subItem != _0DB2)
					{
						subItem.Displayed = false;
						subItem._0AD9 = true;
						_බ.Add(subItem);
					}
				}
				return;
			}
			foreach (BaseItem item in _බ)
			{
				item.Displayed = true;
				item._0AD9 = false;
			}
			_බ.Clear();
			NeedRecalcSize = true;
		}
	}

	private bool ষ
	{
		get
		{
			if (DesignMode || !ඳ.CanSupportGlass)
			{
				return false;
			}
			if (((Control)ඳ).FindForm() is RibbonForm ribbonForm)
			{
				return ribbonForm.IsGlassEnabled;
			}
			return _0EFE.ষ;
		}
	}

	public GenericItemContainer RibbonStripContainer => ଡ଼;

	public GenericItemContainer CaptionContainer => න;

	public SystemCaptionItem SystemCaptionItem => _0DB2;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue(false)]
	public override bool Expanded
	{
		get
		{
			return base.Expanded;
		}
		set
		{
			base.Expanded = value;
			if (value)
			{
				return;
			}
			foreach (BaseItem subItem in SubItems)
			{
				subItem.Expanded = false;
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override Size SubItemsImageSize
	{
		get
		{
			return base.SubItemsImageSize;
		}
		set
		{
		}
	}

	public RibbonStripContainerItem(RibbonStrip parent)
	{
		ඳ = parent;
		m_IsContainer = true;
		AccessibleRole = (AccessibleRole)20;
		ଡ଼ = new ය();
		ଡ଼.ContainerControl = parent;
		ଡ଼.GlobalItem = false;
		ଡ଼.WrapItems = false;
		ଡ଼._0DBE = false;
		ଡ଼._0DBF = false;
		ଡ଼.Stretch = true;
		ଡ଼.Displayed = true;
		ଡ଼.SystemContainer = true;
		ଡ଼.PaddingTop = 0;
		ଡ଼.PaddingBottom = 0;
		ଡ଼.ItemSpacing = 1;
		න = new ᱭ();
		න.ContainerControl = parent;
		න.GlobalItem = false;
		න.WrapItems = false;
		න._0DBE = false;
		න.EqualButtonSize = false;
		න.ව = eContainerVerticalAlignment.Top;
		න._0DBF = false;
		න.Stretch = true;
		න.Displayed = true;
		න.SystemContainer = true;
		න.PaddingBottom = 0;
		න.PaddingTop = 0;
		න.ItemSpacing = 1;
		න.ශ = false;
		ᱭ obj = න;
		obj.ৎ = (EventHandler)Delegate.Combine(obj.ৎ, new EventHandler(ම));
		SubItems.Add(න);
		SubItems.Add(ଡ଼);
		SystemCaptionItem systemCaptionItem = new SystemCaptionItem
		{
			RestoreEnabled = false,
			IsSystemIcon = false,
			ItemAlignment = eItemAlignment.Far
		};
		න.SubItems.Add(systemCaptionItem);
		_0DB2 = systemCaptionItem;
	}

	protected override void Dispose(bool disposing)
	{
		ᱭ obj = න;
		obj.ৎ = (EventHandler)Delegate.Remove(obj.ৎ, new EventHandler(ම));
		base.Dispose(disposing);
	}

	private void ම(object _٠, EventArgs ؿ)
	{
		if (_٠ is BaseItem)
		{
			BaseItem baseItem = _٠ as BaseItem;
			if (!(baseItem is SystemCaptionItem) && න.SubItems.Contains(_0DB2))
			{
				න.SubItems._Remove(_0DB2);
				න.SubItems.__0AF3(_0DB2);
			}
		}
	}

	internal void ඹ()
	{
		ଡ଼.ඹ();
		if (ඳ.CaptionVisible)
		{
			න.ඹ();
		}
	}

	public override void ContainerLostFocus(bool appLostFocus)
	{
		base.ContainerLostFocus(appLostFocus);
		ଡ଼.ContainerLostFocus(appLostFocus);
		if (ඳ.CaptionVisible)
		{
			න.ContainerLostFocus(appLostFocus);
		}
	}

	internal void ௐ()
	{
		if (ඳ.CaptionVisible && ଡ଼.ExpandedItem() == null)
		{
			න.ௐ();
		}
		else
		{
			ଡ଼.ௐ();
		}
	}

	public override void Paint(ItemPaintArgs pa)
	{
		if (!SuspendLayout)
		{
			ଡ଼.Paint(pa);
			if (ඳ.CaptionVisible)
			{
				න.Paint(pa);
			}
		}
	}

	public override void RecalcSize()
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Invalid comparison between Unknown and I4
		if (SuspendLayout)
		{
			return;
		}
		ଡ଼.Bounds = _0D97();
		ଡ଼.RecalcSize();
		if (ଡ଼.HeightInternal < 0)
		{
			ଡ଼.HeightInternal = 0;
		}
		bool flag = false;
		bool flag2 = ষ;
		if (ඳ.CaptionVisible)
		{
			Size frameBorderSize = SystemInformation.FrameBorderSize;
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			Form val2 = null;
			if (val != null)
			{
				val2 = val.FindForm();
			}
			if (val2 != null)
			{
				flag = (int)val2.WindowState == 2;
				if (_ප != null)
				{
					_ප.SetVisibleDirect(val2.ShowIcon);
				}
			}
			if (val2 is RibbonForm)
			{
				Ἇ ἇ = ((RibbonForm)(object)val2).Ꮛ();
				frameBorderSize.Width = ἇ.ἒ;
				frameBorderSize.Height = ἇ.ἑ;
				if (_ප != null)
				{
					_ප.SetVisibleDirect(val2.ShowIcon && ((RibbonForm)(object)val2).RenderFormIcon);
				}
			}
			if (_ඵ != null)
			{
				_ඵ.SetVisibleDirect(_ප.Visible);
			}
			න.Bounds = _0D98();
			න.RecalcSize();
			if (ඳ.CaptionHeight == 0 && _0DB2.TopInternal < frameBorderSize.Height - 1)
			{
				if (Environment.OSVersion.Version.Major >= 6 && !flag2)
				{
					if (flag)
					{
						_0DB2.TopInternal = 1;
						if (_ප != null)
						{
							_ප.TopInternal = ((Dpi.Factor.Height > 1f) ? Dpi.Height5 : 6);
						}
					}
					else
					{
						_0DB2.TopInternal = frameBorderSize.Height - Dpi.Height4;
						if (_ප != null)
						{
							_ප.TopInternal = Math.Max(0, frameBorderSize.Height - ((Dpi.Factor.Height > 1f) ? Dpi.Height3 : 2));
						}
					}
				}
				else
				{
					if (EffectiveStyle == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(EffectiveStyle))
					{
						_0DB2.TopInternal = 1;
					}
					else
					{
						_0DB2.TopInternal = 0;
					}
					if (flag2)
					{
						if (_ප != null)
						{
							_ප.TopInternal = frameBorderSize.Height - Dpi.Width2;
						}
						if (_ඵ != null)
						{
							_ඵ.TopInternal = frameBorderSize.Height - 4;
						}
					}
					else
					{
						if (_ප != null)
						{
							_ප.TopInternal = frameBorderSize.Height + Dpi.Width2;
						}
						if (_ඵ != null)
						{
							_ඵ.TopInternal = frameBorderSize.Height;
						}
					}
				}
			}
			if (Environment.OSVersion.Version.Major >= 6)
			{
				int num = 2;
				if (!flag2)
				{
					num++;
				}
				else if (flag)
				{
					num++;
				}
				int num2 = 0;
				foreach (BaseItem subItem in න.SubItems)
				{
					if (!(subItem is ApplicationButton) && subItem != _0DB2 && subItem != _ප)
					{
						subItem.TopInternal += num;
					}
					num2 = Math.Max(subItem.Bounds.Bottom, num2);
				}
				if (න._0DBC != null)
				{
					න._0DBC.TopInternal += num;
				}
				if (num2 > න.HeightInternal)
				{
					න.SetDisplayRectangle(new Rectangle(න.Bounds.X, න.Bounds.Y, න.Bounds.Width, num2));
				}
			}
			else
			{
				int num3 = 0;
				foreach (BaseItem subItem2 in න.SubItems)
				{
					if (subItem2.HeightInternal < _0DB2.HeightInternal && subItem2 != _ඵ && subItem2 != _ප)
					{
						subItem2.TopInternal = _0DB2.Bounds.Bottom - (subItem2.HeightInternal + ((subItem2 is LabelItem) ? 2 : 0));
						num3 = Math.Max(subItem2.Bounds.Bottom, num3);
					}
				}
				if (න._0DBC != null)
				{
					න._0DBC.TopInternal += _0DB2.HeightInternal - න._0DBC.HeightInternal;
				}
				if (num3 > න.HeightInternal)
				{
					න.SetDisplayRectangle(new Rectangle(න.Bounds.X, න.Bounds.Y, න.Bounds.Width, num3));
				}
			}
			if (ଡ଼.HeightInternal == 0)
			{
				HeightInternal = න.HeightInternal;
			}
			else
			{
				HeightInternal = ଡ଼.Bounds.Bottom;
			}
		}
		else
		{
			int num4 = ଡ଼.HeightInternal;
			if (ඳ.TabGroupsVisible)
			{
				num4 += ඳ.TabGroupHeight + 1;
			}
			HeightInternal = num4;
		}
		base.RecalcSize();
	}

	private Rectangle _0D97()
	{
		return ඳ._0D97();
	}

	private Rectangle _0D98()
	{
		return ඳ._0D98();
	}

	public override BaseItem Copy()
	{
		RibbonStripContainerItem ribbonStripContainerItem = new RibbonStripContainerItem(ඳ);
		CopyToItem(ribbonStripContainerItem);
		return ribbonStripContainerItem;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		RibbonStripContainerItem objCopy = copy as RibbonStripContainerItem;
		base.CopyToItem(objCopy);
	}

	public override void InternalClick(MouseButtons mb, Point mpos)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		ଡ଼.InternalClick(mb, mpos);
		if (ඳ.CaptionVisible)
		{
			න.InternalClick(mb, mpos);
		}
	}

	public override void InternalDoubleClick(MouseButtons mb, Point mpos)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		ଡ଼.InternalDoubleClick(mb, mpos);
		if (ඳ.CaptionVisible)
		{
			න.InternalDoubleClick(mb, mpos);
		}
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		ଡ଼.InternalMouseDown(objArg);
		if (ඳ.CaptionVisible && ((DesignMode && න.ItemAtLocation(objArg.X, objArg.Y) != null) || !DesignMode))
		{
			න.InternalMouseDown(objArg);
		}
	}

	public override void InternalMouseHover()
	{
		ଡ଼.InternalMouseHover();
		if (ඳ.CaptionVisible)
		{
			න.InternalMouseHover();
		}
	}

	public override void InternalMouseLeave()
	{
		ଡ଼.InternalMouseLeave();
		if (ඳ.CaptionVisible)
		{
			න.InternalMouseLeave();
		}
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		ଡ଼.InternalMouseMove(objArg);
		if (ඳ.CaptionVisible)
		{
			න.InternalMouseMove(objArg);
		}
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		ଡ଼.InternalMouseUp(objArg);
		if (ඳ.CaptionVisible)
		{
			න.InternalMouseUp(objArg);
		}
	}

	public override void InternalKeyDown(KeyEventArgs objArg)
	{
		BaseItem baseItem = ExpandedItem();
		if (baseItem == null)
		{
			baseItem = න.ExpandedItem();
		}
		if (baseItem == null)
		{
			baseItem = ଡ଼.ExpandedItem();
		}
		if (baseItem == null || !ඳ.CaptionVisible)
		{
			ଡ଼.InternalKeyDown(objArg);
			if (!objArg.Handled && ඳ.CaptionVisible)
			{
				න.InternalKeyDown(objArg);
			}
		}
		else if (baseItem.Parent == ଡ଼)
		{
			ଡ଼.InternalKeyDown(objArg);
		}
		else
		{
			න.InternalKeyDown(objArg);
		}
	}

	public override BaseItem ItemAtLocation(int x, int y)
	{
		if (ଡ଼.DisplayRectangle.Contains(x, y))
		{
			return ଡ଼.ItemAtLocation(x, y);
		}
		if (න.DisplayRectangle.Contains(x, y))
		{
			return න.ItemAtLocation(x, y);
		}
		return null;
	}

	protected override void OnStyleChanged()
	{
		if (EffectiveStyle == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(EffectiveStyle))
		{
			if (_ප == null)
			{
				_ඵ = new Separator("sys_caption_separator");
				_ඵ.SetSystemItem(b: true);
				න.SubItems.__0AF3(_ඵ, 0);
				_ප = new SystemCaptionItem();
				_ප.Name = "sys_caption_icon";
				_ප.Enabled = false;
				_ප.Style = Style;
				_ප.IsSystemIcon = true;
				_ප.QueryIconOnPaint = true;
				න.SubItems.__0AF3(_ප, 0);
			}
		}
		else if (EffectiveStyle == eDotNetBarStyle.Windows7)
		{
			if (_ප == null)
			{
				_ඵ = new Separator("sys_caption_separator");
				_ඵ.FixedSize = new Size(3, 12);
				_ඵ.SetSystemItem(b: true);
				න.SubItems.__0AF3(_ඵ, 0);
				_ප = new SystemCaptionItem();
				_ප.Name = "sys_caption_icon";
				_ප.Enabled = false;
				_ප.Style = Style;
				_ප.IsSystemIcon = true;
				_ප.QueryIconOnPaint = true;
				න.SubItems.__0AF3(_ප, 0);
			}
		}
		else if (_ප != null)
		{
			if (න.SubItems.Contains(_ප))
			{
				න.SubItems._Remove(_ප);
			}
			_ප.Dispose();
			_ප = null;
			if (න.SubItems.Contains(_ඵ))
			{
				න.SubItems._Remove(_ඵ);
			}
			_ඵ.Dispose();
			_ඵ = null;
		}
		base.OnStyleChanged();
	}

	public InsertPosition GetInsertPosition(Point pScreen, BaseItem DragItem)
	{
		InsertPosition insertPosition = ଡ଼.GetInsertPosition(pScreen, DragItem);
		if (insertPosition == null && ඳ.CaptionVisible)
		{
			insertPosition = න.GetInsertPosition(pScreen, DragItem);
		}
		return insertPosition;
	}

	public void DrawReversibleMarker(int iPos, bool Before)
	{
	}

	public void InsertItemAt(BaseItem objItem, int iPos, bool Before)
	{
	}
}
