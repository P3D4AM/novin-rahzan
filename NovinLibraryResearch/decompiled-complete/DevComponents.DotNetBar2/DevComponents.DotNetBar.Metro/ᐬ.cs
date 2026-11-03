using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Metro;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
internal class ᐬ : ImageItem, IDesignTimeProvider
{
	private const string ᐳ = "<font size=\"7\">SETTINGS</font>";

	private const string ᐴ = "<font size=\"7\">HELP</font>";

	private ᐾ _ᐵ;

	private ᱭ _ᐶ;

	private SystemCaptionItem _ᐷ;

	private MetroTabStrip _Ꮺ;

	private SystemCaptionItem _ප;

	private Separator _ඵ;

	private ButtonItem _ᐸ;

	private ButtonItem _ᐹ;

	private string _ᐺ = "";

	private string _ᐻ;

	public SimpleItemContainer ᐭ => _ᐵ;

	public GenericItemContainer ᐮ => _ᐶ;

	public SystemCaptionItem _൧ => _ᐷ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
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

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	public bool ᐯ
	{
		get
		{
			return _ᐸ.Visible;
		}
		set
		{
			if (value != _ᐸ.Visible)
			{
				_ᐸ.Visible = value;
				RecalcSize();
			}
		}
	}

	public bool ᐰ
	{
		get
		{
			return _ᐹ.Visible;
		}
		set
		{
			if (value != _ᐹ.Visible)
			{
				_ᐹ.Visible = value;
				RecalcSize();
			}
		}
	}

	public string ᐱ
	{
		get
		{
			return _ᐺ;
		}
		set
		{
			if (value != _ᐺ)
			{
				string ᐺ = _ᐺ;
				_ᐺ = value;
				OnSettingsButtonTextChanged(ᐺ, value);
			}
		}
	}

	public string ᐲ
	{
		get
		{
			return _ᐻ;
		}
		set
		{
			if (value != _ᐻ)
			{
				string ᐻ = _ᐻ;
				_ᐻ = value;
				OnHelpButtonTextChanged(ᐻ, value);
			}
		}
	}

	public ᐬ(MetroTabStrip parent)
	{
		_Ꮺ = parent;
		m_IsContainer = true;
		AccessibleRole = (AccessibleRole)20;
		_ᐵ = new ᐾ();
		_ᐵ.ContainerControl = parent;
		_ᐵ.GlobalItem = false;
		_ᐵ.Stretch = true;
		_ᐵ.Displayed = true;
		_ᐶ = new ᱭ();
		_ᐶ.ContainerControl = parent;
		_ᐶ.GlobalItem = false;
		_ᐶ.WrapItems = false;
		_ᐶ._0DBE = false;
		_ᐶ.EqualButtonSize = false;
		_ᐶ.ව = eContainerVerticalAlignment.Top;
		_ᐶ._0DBF = false;
		_ᐶ.Stretch = true;
		_ᐶ.Displayed = true;
		_ᐶ.SystemContainer = true;
		_ᐶ.PaddingBottom = 0;
		_ᐶ.PaddingTop = 0;
		_ᐶ.PaddingLeft = 6;
		_ᐶ.ItemSpacing = 1;
		_ᐶ.ශ = false;
		ᱭ ᐶ = _ᐶ;
		ᐶ.ৎ = (EventHandler)Delegate.Combine(ᐶ.ৎ, new EventHandler(ම));
		SubItems.Add(_ᐶ);
		SubItems.Add(_ᐵ);
		_ᐸ = new ButtonItem("sysSettingsButton");
		_ᐸ.Text = "<font size=\"7\">SETTINGS</font>";
		_ᐸ.ItemAlignment = eItemAlignment.Far;
		_ᐸ.Click += Ꮸ;
		_ᐸ.SetSystemItem(b: true);
		_ᐸ.CanCustomize = false;
		_ᐶ.SubItems.Add(_ᐸ);
		_ᐹ = new ButtonItem("sysHelpButton");
		_ᐹ.Text = "<font size=\"7\">HELP</font>";
		_ᐹ.SetSystemItem(b: true);
		_ᐹ.CanCustomize = false;
		_ᐹ.ItemAlignment = eItemAlignment.Far;
		_ᐹ.Click += Ꮹ;
		_ᐶ.SubItems.Add(_ᐹ);
		SystemCaptionItem systemCaptionItem = new SystemCaptionItem
		{
			RestoreEnabled = false,
			IsSystemIcon = false,
			ItemAlignment = eItemAlignment.Far
		};
		_ᐶ.SubItems.Add(systemCaptionItem);
		_ᐷ = systemCaptionItem;
	}

	private void ම(object _٠, EventArgs ؿ)
	{
		if (!(_٠ is BaseItem))
		{
			return;
		}
		BaseItem baseItem = _٠ as BaseItem;
		if (!(baseItem is SystemCaptionItem))
		{
			if (_ᐶ.SubItems.Contains(_ᐸ))
			{
				_ᐶ.SubItems._Remove(_ᐸ);
				_ᐶ.SubItems.__0AF3(_ᐸ);
			}
			if (_ᐶ.SubItems.Contains(_ᐹ))
			{
				_ᐶ.SubItems._Remove(_ᐹ);
				_ᐶ.SubItems.__0AF3(_ᐹ);
			}
			if (_ᐶ.SubItems.Contains(_ᐷ))
			{
				_ᐶ.SubItems._Remove(_ᐷ);
				_ᐶ.SubItems.__0AF3(_ᐷ);
			}
		}
	}

	private void Ꮸ(object _٠, EventArgs ؿ)
	{
		if (ContainerControl is MetroTabStrip metroTabStrip && ((Control)metroTabStrip).Parent is MetroShell metroShell)
		{
			metroShell.ᏺ(ؿ);
		}
	}

	private void Ꮹ(object _٠, EventArgs ؿ)
	{
		if (ContainerControl is MetroTabStrip metroTabStrip && ((Control)metroTabStrip).Parent is MetroShell metroShell)
		{
			metroShell.ᏻ(ؿ);
		}
	}

	internal void ඹ()
	{
		_ᐵ.ඹ();
		if (_Ꮺ.CaptionVisible)
		{
			_ᐶ.ඹ();
		}
	}

	public override void ContainerLostFocus(bool appLostFocus)
	{
		base.ContainerLostFocus(appLostFocus);
		_ᐵ.ContainerLostFocus(appLostFocus);
		if (_Ꮺ.CaptionVisible)
		{
			_ᐶ.ContainerLostFocus(appLostFocus);
		}
	}

	internal void ௐ()
	{
		if (_Ꮺ.CaptionVisible && _ᐵ.ExpandedItem() == null)
		{
			_ᐶ.ௐ();
		}
		else
		{
			_ᐵ.ௐ();
		}
	}

	public override void Paint(ItemPaintArgs pa)
	{
		if (!SuspendLayout)
		{
			_ᐵ.Paint(pa);
			if (_Ꮺ.CaptionVisible)
			{
				_ᐶ.Paint(pa);
			}
		}
	}

	public override void RecalcSize()
	{
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Invalid comparison between Unknown and I4
		if (SuspendLayout)
		{
			return;
		}
		_ᐵ.Bounds = _0D97();
		_ᐵ.RecalcSize();
		if (_ᐵ.HeightInternal < 0)
		{
			_ᐵ.HeightInternal = 0;
		}
		bool flag = false;
		if (_Ꮺ.CaptionVisible)
		{
			_ᐶ.Bounds = _0D98();
			_ᐶ.RecalcSize();
			Size frameBorderSize = SystemInformation.FrameBorderSize;
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				Form val2 = val.FindForm();
				if (val2 != null)
				{
					if (_ප != null)
					{
						_ප.SetVisibleDirect(val2.ShowIcon && _Ꮺ.ShowIcon);
					}
					if (_ඵ != null)
					{
						_ඵ.SetVisibleDirect(val2.ShowIcon && _Ꮺ.ShowIcon);
					}
				}
				if (val2 is MetroAppForm metroAppForm)
				{
					Ἇ ἇ = metroAppForm.Ꮛ();
					frameBorderSize.Width = ἇ.ἒ;
					frameBorderSize.Height = ἇ.ἑ;
				}
			}
			if (_Ꮺ.CaptionHeight == 0 && _ᐷ.TopInternal < frameBorderSize.Height - 1)
			{
				object containerControl2 = ContainerControl;
				Control val3 = (Control)((containerControl2 is Control) ? containerControl2 : null);
				Form val4 = null;
				if (val3 != null)
				{
					val4 = val3.FindForm();
				}
				if (val4 != null && (int)val4.WindowState == 2)
				{
					flag = true;
				}
				if (flag)
				{
					_ᐷ.TopInternal = 1;
					if (_ප != null)
					{
						_ප.TopInternal = 1;
					}
				}
				else
				{
					_ᐷ.TopInternal = Math.Max(1, frameBorderSize.Height - 6);
					if (_ප != null)
					{
						_ප.TopInternal = frameBorderSize.Height - 5;
					}
				}
			}
			if (Environment.OSVersion.Version.Major >= 6)
			{
				int num = 3;
				if (flag)
				{
					num++;
				}
				int num2 = 0;
				foreach (BaseItem subItem in _ᐶ.SubItems)
				{
					if (!(subItem is ApplicationButton) && !(subItem is MetroAppButton) && subItem != _ᐷ && subItem != _ඵ)
					{
						subItem.TopInternal += num;
					}
					else if (subItem == _ඵ)
					{
						subItem.TopInternal += ((!flag) ? 1 : 2);
					}
					num2 = Math.Max(subItem.Bounds.Bottom, num2);
				}
				if (_ᐶ._0DBC != null)
				{
					_ᐶ._0DBC.TopInternal += num;
				}
				if (num2 > _ᐶ.HeightInternal)
				{
					_ᐶ.SetDisplayRectangle(new Rectangle(_ᐶ.Bounds.X, _ᐶ.Bounds.Y, _ᐶ.Bounds.Width, num2));
				}
			}
			else
			{
				int num3 = 0;
				foreach (BaseItem subItem2 in _ᐶ.SubItems)
				{
					if (subItem2.HeightInternal < _ᐷ.HeightInternal && subItem2 != _ඵ && subItem2 != _ප)
					{
						subItem2.TopInternal = _ᐷ.Bounds.Bottom - (subItem2.HeightInternal + ((subItem2 is LabelItem) ? 2 : 0));
						num3 = Math.Max(subItem2.Bounds.Bottom, num3);
					}
				}
				if (_ᐶ._0DBC != null)
				{
					_ᐶ._0DBC.TopInternal += _ᐷ.HeightInternal - _ᐶ._0DBC.HeightInternal;
				}
				if (num3 > _ᐶ.HeightInternal)
				{
					_ᐶ.SetDisplayRectangle(new Rectangle(_ᐶ.Bounds.X, _ᐶ.Bounds.Y, _ᐶ.Bounds.Width, num3));
				}
			}
			if (_ᐵ.HeightInternal == 0)
			{
				HeightInternal = _ᐶ.HeightInternal;
			}
			else
			{
				HeightInternal = _ᐵ.Bounds.Bottom;
			}
		}
		else
		{
			int heightInternal = _ᐵ.HeightInternal;
			HeightInternal = heightInternal;
		}
		base.RecalcSize();
	}

	private Rectangle _0D97()
	{
		return _Ꮺ._0D97();
	}

	private Rectangle _0D98()
	{
		return _Ꮺ._0D98();
	}

	public override BaseItem Copy()
	{
		ᐬ ᐬ2 = new ᐬ(_Ꮺ);
		CopyToItem(ᐬ2);
		return ᐬ2;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		ᐬ objCopy = copy as ᐬ;
		base.CopyToItem(objCopy);
	}

	public override void InternalClick(MouseButtons mb, Point mpos)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		_ᐵ.InternalClick(mb, mpos);
		if (_Ꮺ.CaptionVisible)
		{
			_ᐶ.InternalClick(mb, mpos);
		}
	}

	public override void InternalDoubleClick(MouseButtons mb, Point mpos)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		_ᐵ.InternalDoubleClick(mb, mpos);
		if (_Ꮺ.CaptionVisible)
		{
			_ᐶ.InternalDoubleClick(mb, mpos);
		}
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		_ᐵ.InternalMouseDown(objArg);
		if (_Ꮺ.CaptionVisible && ((DesignMode && _ᐶ.ItemAtLocation(objArg.X, objArg.Y) != null) || !DesignMode))
		{
			_ᐶ.InternalMouseDown(objArg);
		}
	}

	public override void InternalMouseHover()
	{
		_ᐵ.InternalMouseHover();
		if (_Ꮺ.CaptionVisible)
		{
			_ᐶ.InternalMouseHover();
		}
	}

	public override void InternalMouseLeave()
	{
		_ᐵ.InternalMouseLeave();
		if (_Ꮺ.CaptionVisible)
		{
			_ᐶ.InternalMouseLeave();
		}
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		_ᐵ.InternalMouseMove(objArg);
		if (_Ꮺ.CaptionVisible)
		{
			_ᐶ.InternalMouseMove(objArg);
		}
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		_ᐵ.InternalMouseUp(objArg);
		if (_Ꮺ.CaptionVisible)
		{
			_ᐶ.InternalMouseUp(objArg);
		}
	}

	public override void InternalKeyDown(KeyEventArgs objArg)
	{
		BaseItem baseItem = ExpandedItem();
		if (baseItem == null)
		{
			baseItem = _ᐶ.ExpandedItem();
		}
		if (baseItem == null)
		{
			baseItem = _ᐵ.ExpandedItem();
		}
		if (baseItem == null || !_Ꮺ.CaptionVisible)
		{
			_ᐵ.InternalKeyDown(objArg);
			if (!objArg.Handled && _Ꮺ.CaptionVisible)
			{
				_ᐶ.InternalKeyDown(objArg);
			}
		}
		else if (baseItem.Parent == _ᐵ)
		{
			_ᐵ.InternalKeyDown(objArg);
		}
		else
		{
			_ᐶ.InternalKeyDown(objArg);
		}
	}

	public override BaseItem ItemAtLocation(int x, int y)
	{
		if (_ᐵ.DisplayRectangle.Contains(x, y))
		{
			return _ᐵ.ItemAtLocation(x, y);
		}
		if (_ᐶ.DisplayRectangle.Contains(x, y))
		{
			return _ᐶ.ItemAtLocation(x, y);
		}
		return null;
	}

	protected override void OnStyleChanged()
	{
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Expected O, but got Unknown
		eDotNetBarStyle effectiveStyle = EffectiveStyle;
		switch (effectiveStyle)
		{
		case eDotNetBarStyle.Office2010:
			if (_ප == null)
			{
				_ඵ = new Separator("sys_caption_separator");
				_ඵ.SetSystemItem(b: true);
				_ඵ.DesignTimeVisible = false;
				_ඵ.CanCustomize = false;
				_ᐶ.SubItems.__0AF3(_ඵ, 0);
				_ප = new SystemCaptionItem();
				_ප.Name = "sys_caption_icon";
				_ප.Enabled = false;
				_ප.Style = Style;
				_ප.IsSystemIcon = true;
				_ප.DesignTimeVisible = false;
				_ප.CanCustomize = false;
				_ප.QueryIconOnPaint = true;
				_ප.MouseDown += new MouseEventHandler(ᐽ);
				_ප.DoubleClick += ᐼ;
				_ᐶ.SubItems.__0AF3(_ප, 0);
			}
			break;
		case eDotNetBarStyle.Windows7:
			if (_ප == null)
			{
				_ඵ = new Separator("sys_caption_separator");
				_ඵ.FixedSize = new Size(3, 12);
				_ඵ.SetSystemItem(b: true);
				_ඵ.DesignTimeVisible = false;
				_ඵ.CanCustomize = false;
				_ᐶ.SubItems.__0AF3(_ඵ, 0);
				_ප = new SystemCaptionItem();
				_ප.Name = "sys_caption_icon";
				_ප.Enabled = false;
				_ප.Style = Style;
				_ප.IsSystemIcon = true;
				_ප.QueryIconOnPaint = true;
				_ප.DesignTimeVisible = false;
				_ප.CanCustomize = false;
				_ප.MouseDown += new MouseEventHandler(ᐽ);
				_ᐶ.SubItems.__0AF3(_ප, 0);
			}
			break;
		default:
			if (StyleManager.IsMetro(effectiveStyle))
			{
				if (_ප == null)
				{
					_ඵ = new Separator("sys_caption_separator");
					_ඵ.FixedSize = new Size(3, 14);
					_ඵ.SetSystemItem(b: true);
					_ඵ.DesignTimeVisible = false;
					_ඵ.CanCustomize = false;
					_ᐶ.SubItems.__0AF3(_ඵ, 0);
					_ප = new SystemCaptionItem();
					_ප.Name = "sys_caption_icon";
					_ප.Enabled = false;
					_ප.Style = Style;
					_ප.IsSystemIcon = true;
					_ප.DesignTimeVisible = false;
					_ප.CanCustomize = false;
					_ප.QueryIconOnPaint = true;
					_ප.MouseDown += new MouseEventHandler(ᐽ);
					_ප.DoubleClick += ᐼ;
					_ᐶ.SubItems.__0AF3(_ප, 0);
				}
			}
			else if (_ප != null)
			{
				if (_ᐶ.SubItems.Contains(_ප))
				{
					_ᐶ.SubItems._Remove(_ප);
				}
				_ප.MouseDown -= new MouseEventHandler(ᐽ);
				_ප.DoubleClick -= ᐼ;
				_ප.Dispose();
				_ප = null;
				if (_ᐶ.SubItems.Contains(_ඵ))
				{
					_ᐶ.SubItems._Remove(_ඵ);
				}
				_ඵ.Dispose();
				_ඵ = null;
			}
			break;
		}
		base.OnStyleChanged();
	}

	private void ᐼ(object _٠, EventArgs ؿ)
	{
		if (_Ꮺ != null)
		{
			_Ꮺ.ᐫ();
		}
	}

	private void ᐽ(object _٠, MouseEventArgs ؿ)
	{
		if (ContainerControl is MetroTabStrip metroTabStrip)
		{
			Point point = new Point(_ප.LeftInternal, _ප.Bounds.Bottom + 1);
			point = ((Control)metroTabStrip).PointToScreen(point);
			metroTabStrip.ᐪ(point);
		}
	}

	public InsertPosition GetInsertPosition(Point pScreen, BaseItem DragItem)
	{
		InsertPosition insertPosition = _ᐵ.GetInsertPosition(pScreen, DragItem);
		if (insertPosition == null && _Ꮺ.CaptionVisible)
		{
			insertPosition = _ᐶ.GetInsertPosition(pScreen, DragItem);
		}
		return insertPosition;
	}

	public void DrawReversibleMarker(int iPos, bool Before)
	{
	}

	public void InsertItemAt(BaseItem objItem, int iPos, bool Before)
	{
	}

	protected virtual void OnSettingsButtonTextChanged(string oldValue, string newValue)
	{
		if (string.IsNullOrEmpty(newValue))
		{
			_ᐸ.Text = "<font size=\"7\">SETTINGS</font>";
		}
		else
		{
			_ᐸ.Text = "<font size=\"7\">" + newValue + "</font>";
		}
	}

	protected virtual void OnHelpButtonTextChanged(string oldValue, string newValue)
	{
		if (string.IsNullOrEmpty(newValue))
		{
			_ᐹ.Text = "<font size=\"7\">HELP</font>";
		}
		else
		{
			_ᐹ.Text = "<font size=\"7\">" + newValue + "</font>";
		}
	}
}
