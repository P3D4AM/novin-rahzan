using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

public class SuperTabCloseBox : BaseItem
{
	private SuperTabControlBox __2075;

	private Size _ᡍ = new Size(16, 16);

	private bool _ᆿ;

	private bool __171A;

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

	[Description("Indicates CloseBox Visible state.")]
	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Behavior")]
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
				ᾦ();
			}
		}
	}

	internal bool _206D => __171A;

	internal bool ᆼ => _ᆿ;

	public SuperTabCloseBox(SuperTabControlBox controlBox)
	{
		__2075 = controlBox;
		Visible = false;
	}

	public override void RecalcSize()
	{
		base.RecalcSize();
		WidthInternal = _ᡍ.Width;
		HeightInternal = _ᡍ.Height;
	}

	private void ᾦ()
	{
		if (__2075.έ != null)
		{
			SuperTabStripItem superTabStripItem = __2075.έ._1FD4;
			if (superTabStripItem != null)
			{
				__2075.NeedRecalcSize = true;
				__2075.RecalcSize();
				superTabStripItem.NeedRecalcSize = true;
				superTabStripItem.Refresh();
			}
		}
	}

	public override void Paint(ItemPaintArgs p)
	{
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		Graphics graphics = p.Graphics;
		using SuperTabColorTable superTabColorTable = __2075.έ.ጧ();
		Color image = superTabColorTable.ControlBoxDefault.Image;
		if (_ᆿ)
		{
			Rectangle bounds = Bounds;
			bounds.Width--;
			bounds.Height--;
			if (__171A && _ᆿ)
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
		graphics.DrawImageUnscaled((Image)(object)__2075.έ._2028(graphics, image), Bounds);
	}

	public override void InternalMouseEnter()
	{
		base.InternalMouseEnter();
		_ᆿ = true;
		Refresh();
	}

	public override void InternalMouseLeave()
	{
		base.InternalMouseLeave();
		_ᆿ = false;
		Refresh();
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		base.InternalMouseDown(objArg);
		if (!DesignMode && (int)objArg.Button == 1048576)
		{
			__171A = true;
		}
		Refresh();
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		base.InternalMouseUp(objArg);
		__171A = false;
		if (_ᆿ)
		{
			__2075.έ.Ὼ?.Close();
		}
		Refresh();
	}

	public override BaseItem Copy()
	{
		SuperTabCloseBox superTabCloseBox = new SuperTabCloseBox(__2075);
		CopyToItem(superTabCloseBox);
		return superTabCloseBox;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		SuperTabCloseBox objCopy = copy as SuperTabCloseBox;
		base.CopyToItem(objCopy);
	}
}
