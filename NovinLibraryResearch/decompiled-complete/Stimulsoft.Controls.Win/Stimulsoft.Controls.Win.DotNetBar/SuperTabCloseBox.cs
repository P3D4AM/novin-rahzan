using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class SuperTabCloseBox : BaseItem
{
	private SuperTabControlBox _ControlBox;

	private Size _ItemSize = new Size(16, 16);

	private bool _IsMouseOver;

	private bool _IsMouseDown;

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

	[Browsable(false)]
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

	[Browsable(false)]
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

	[Browsable(false)]
	public override Cursor Cursor
	{
		get
		{
			return base.Cursor;
		}
		set
		{
			base.Cursor = value;
		}
	}

	[Browsable(false)]
	public override bool Enabled
	{
		get
		{
			return base.Enabled;
		}
		set
		{
			base.Enabled = value;
		}
	}

	[Browsable(false)]
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

	[Browsable(false)]
	public override string Text
	{
		get
		{
			return base.Text;
		}
		set
		{
			base.Text = value;
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
	public override string Tooltip
	{
		get
		{
			return base.Tooltip;
		}
		set
		{
			base.Tooltip = value;
		}
	}

	[Browsable(false)]
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

	[Browsable(false)]
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

	[Browsable(false)]
	public override ShortcutsCollection Shortcuts
	{
		get
		{
			return base.Shortcuts;
		}
		set
		{
			base.Shortcuts = value;
		}
	}

	[Browsable(false)]
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

	[Browsable(false)]
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
	public override bool ShowSubItems
	{
		get
		{
			return base.ShowSubItems;
		}
		set
		{
			base.ShowSubItems = value;
		}
	}

	[Browsable(false)]
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
	public override Command Command
	{
		get
		{
			return base.Command;
		}
		set
		{
			base.Command = value;
		}
	}

	[Browsable(false)]
	public override object CommandParameter
	{
		get
		{
			return base.CommandParameter;
		}
		set
		{
			base.CommandParameter = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates CloseBox Visible state.")]
	public override bool Visible
	{
		get
		{
			return base.Visible;
		}
		set
		{
			if (base.Visible != value)
			{
				base.Visible = value;
				MyRefresh();
			}
		}
	}

	internal bool IsMouseDown => _IsMouseDown;

	internal bool IsMouseOver => _IsMouseOver;

	public SuperTabCloseBox(SuperTabControlBox controlBox)
	{
		_ControlBox = controlBox;
		Visible = false;
	}

	public override void RecalcSize()
	{
		base.RecalcSize();
		WidthInternal = _ItemSize.Width;
		HeightInternal = _ItemSize.Height;
	}

	private void MyRefresh()
	{
		if (_ControlBox.TabDisplay != null)
		{
			SuperTabStripItem tabStripItem = _ControlBox.TabDisplay.TabStripItem;
			if (tabStripItem != null)
			{
				_ControlBox.NeedRecalcSize = true;
				_ControlBox.RecalcSize();
				tabStripItem.NeedRecalcSize = true;
				tabStripItem.Refresh();
			}
		}
	}

	public override void Paint(ItemPaintArgs p)
	{
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Expected O, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		Graphics graphics = p.Graphics;
		using SuperTabColorTable superTabColorTable = _ControlBox.TabDisplay.GetColorTable();
		Color image = superTabColorTable.ControlBoxDefault.Image;
		if (_IsMouseOver)
		{
			Rectangle bounds = Bounds;
			bounds.Width--;
			bounds.Height--;
			if (_IsMouseDown && _IsMouseOver)
			{
				image = superTabColorTable.ControlBoxPressed.Image;
				Brush val = (Brush)new SolidBrush(superTabColorTable.ControlBoxPressed.Background);
				try
				{
					graphics.FillRectangle(val, bounds);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
				Pen val2 = new Pen(superTabColorTable.ControlBoxPressed.Border);
				try
				{
					graphics.DrawRectangle(val2, bounds);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			else
			{
				image = superTabColorTable.ControlBoxMouseOver.Image;
				Brush val3 = (Brush)new SolidBrush(superTabColorTable.ControlBoxMouseOver.Background);
				try
				{
					graphics.FillRectangle(val3, bounds);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				Pen val4 = new Pen(superTabColorTable.ControlBoxMouseOver.Border);
				try
				{
					graphics.DrawRectangle(val4, bounds);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
		}
		graphics.DrawImageUnscaled((Image)(object)_ControlBox.TabDisplay.GetCloseButton(graphics, image), Bounds);
	}

	public override void InternalMouseEnter()
	{
		base.InternalMouseEnter();
		_IsMouseOver = true;
		Refresh();
	}

	public override void InternalMouseLeave()
	{
		base.InternalMouseLeave();
		_IsMouseOver = false;
		Refresh();
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		base.InternalMouseDown(objArg);
		if (!DesignMode && (int)objArg.Button == 1048576)
		{
			_IsMouseDown = true;
		}
		Refresh();
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		base.InternalMouseUp(objArg);
		_IsMouseDown = false;
		if (_IsMouseOver)
		{
			_ControlBox.TabDisplay.SelectedTab?.Close();
		}
		Refresh();
	}

	public override BaseItem Copy()
	{
		SuperTabCloseBox superTabCloseBox = new SuperTabCloseBox(_ControlBox);
		CopyToItem(superTabCloseBox);
		return superTabCloseBox;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		SuperTabCloseBox objCopy = copy as SuperTabCloseBox;
		base.CopyToItem(objCopy);
	}
}
