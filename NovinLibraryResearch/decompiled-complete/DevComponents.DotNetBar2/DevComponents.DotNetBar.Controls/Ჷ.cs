using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Animation;
using DevComponents.DotNetBar.Metro;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.DotNetBar.Controls;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
internal class Ჷ : ImageItem, IDesignTimeProvider
{
	private TabFormItemsSimpleContainer _ᐵ;

	private ᱭ _ᐶ;

	private SystemCaptionItem _ᐷ;

	private TabFormStripControl _Ꮺ;

	private SystemCaptionItem _ප;

	private Separator _ඵ;

	private ᱭ _Ჾ;

	private int _Ჿ = 22;

	private bool __1CC0;

	private ButtonX __1CC1;

	private ButtonX __1CC2;

	private Rectangle __1CC3 = Rectangle.Empty;

	private int __1CC4;

	private int __1CC5 = 96;

	private _1A6A __1CC6;

	private int __1CC7;

	private bool __1CC8;

	private NewTabFormItem __1CC9;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[MergableProperty(false)]
	public SubItemsCollection Ჸ => _Ჾ.SubItems;

	public int Ჹ
	{
		get
		{
			return _Ჿ;
		}
		set
		{
			_Ჿ = value;
		}
	}

	private bool Ჺ
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Invalid comparison between Unknown and I4
			Control val = (Control)ContainerControl;
			if (val != null)
			{
				Form val2 = val.FindForm();
				if (val2 != null)
				{
					return (int)val2.WindowState == 2;
				}
			}
			return false;
		}
	}

	public int _1CBB
	{
		get
		{
			return __1CC5;
		}
		set
		{
			__1CC5 = value;
		}
	}

	public TabFormItemsSimpleContainer _1CBC => _ᐵ;

	public GenericItemContainer ᐮ => _ᐶ;

	public SystemCaptionItem _൧ => _ᐷ;

	[DefaultValue(false)]
	[Description("Indicates whether new tab item which allows creation of new tab when clicked is visible. When visible you need to handle CreateNewTab event and create your new tab in event handler.")]
	[Category("Appearance")]
	public bool Ჽ
	{
		get
		{
			return __1CC8;
		}
		set
		{
			__1CC8 = value;
			_1CCF();
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

	[Description("Occurs when new tab item is clicked by end user and allows you to create and add new tab to the control.")]
	public event EventHandler Ს;

	public Ჷ(TabFormStripControl parent)
	{
		_Ꮺ = parent;
		m_IsContainer = true;
		AccessibleRole = (AccessibleRole)20;
		_ᐵ = new TabFormItemsSimpleContainer();
		_ᐵ.ContainerControl = parent;
		_ᐵ.GlobalItem = false;
		_ᐵ.OverlapSpacing = TabFormItem._136F;
		_ᐵ.OverlapType = typeof(TabFormItemBase);
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
		_Ჾ = new ᱭ();
		_Ჾ.ContainerControl = parent;
		_Ჾ.GlobalItem = false;
		_Ჾ.WrapItems = false;
		_Ჾ._0DBE = false;
		_Ჾ.EqualButtonSize = false;
		_Ჾ.ව = eContainerVerticalAlignment.Top;
		_Ჾ._0DBF = false;
		_Ჾ.Stretch = true;
		_Ჾ.Displayed = true;
		_Ჾ.SystemContainer = true;
		_Ჾ.PaddingBottom = 0;
		_Ჾ.PaddingTop = 0;
		_Ჾ.PaddingLeft = 6;
		_Ჾ.ItemSpacing = 1;
		_Ჾ.ශ = false;
		SubItems.Add(_ᐶ);
		SubItems.Add(_Ჾ);
		SubItems.Add(_ᐵ);
		SystemCaptionItem systemCaptionItem = new SystemCaptionItem();
		systemCaptionItem.RestoreEnabled = false;
		systemCaptionItem.IsSystemIcon = false;
		systemCaptionItem.ItemAlignment = eItemAlignment.Far;
		systemCaptionItem.SetSystemItem(b: true);
		_ᐷ = systemCaptionItem;
		SubItems.Add(_ᐷ);
	}

	internal void ඍ(bool _1CCA)
	{
		if (_ᐷ == null)
		{
			return;
		}
		if (_1CCA)
		{
			if (_ᐷ.Parent != _ᐶ)
			{
				if (_ᐷ.Parent != null)
				{
					_ᐷ.Parent.SubItems.Remove(_ᐷ);
				}
				_ᐶ.SubItems.Add(_ᐷ);
			}
		}
		else if (_ᐷ.Parent != this)
		{
			if (_ᐷ.Parent != null)
			{
				_ᐷ.Parent.SubItems.Remove(_ᐷ);
			}
			SubItems.Add(_ᐷ);
		}
	}

	protected internal override void OnItemAdded(BaseItem item)
	{
		if (_ᐷ != null && _ᐷ.Parent == this && item != _ᐷ)
		{
			SubItems._Remove(_ᐷ);
			SubItems.__0AF3(_ᐷ);
		}
		base.OnItemAdded(item);
	}

	private void ම(object _٠, EventArgs ؿ)
	{
		if (_٠ is BaseItem)
		{
			BaseItem baseItem = _٠ as BaseItem;
			if (!(baseItem is SystemCaptionItem) && _ᐶ.SubItems.Contains(_ᐷ))
			{
				_ᐶ.SubItems._Remove(_ᐷ);
				_ᐶ.SubItems.__0AF3(_ᐷ);
			}
		}
	}

	internal void ඹ()
	{
		_ᐵ.ඹ();
		if (_Ꮺ.CaptionVisible)
		{
			_ᐶ.ඹ();
			_Ჾ.ඹ();
		}
	}

	public override void ContainerLostFocus(bool appLostFocus)
	{
		base.ContainerLostFocus(appLostFocus);
		_ᐵ.ContainerLostFocus(appLostFocus);
		if (_Ꮺ.CaptionVisible)
		{
			_ᐶ.ContainerLostFocus(appLostFocus);
			_Ჾ.ContainerLostFocus(appLostFocus);
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
			if (__1CC0)
			{
				pa.Graphics.SetClip(__1CC3);
				_ᐵ.ClippingRectangle = __1CC3;
			}
			else
			{
				_ᐵ.ClippingRectangle = Rectangle.Empty;
			}
			_ᐵ.Paint(pa);
			if (_Ꮺ.CaptionVisible)
			{
				_ᐶ.Paint(pa);
				_Ჾ.Paint(pa);
			}
			if (_ᐷ.Parent == this)
			{
				_ᐷ.Paint(pa);
			}
		}
	}

	private void _1CCB()
	{
		if (__1CC1 != null && __1CC2 != null)
		{
			Rectangle rectangle = _0D97();
			Rectangle bounds = _ᐵ.Bounds;
			int num = Dpi.Width(_Ჿ);
			((Control)__1CC1).Bounds = new Rectangle(rectangle.X, bounds.Y + Dpi.Height2, num, bounds.Height - Dpi.Height3);
			((Control)__1CC2).Bounds = new Rectangle(rectangle.Right - num, bounds.Y + Dpi.Height2, num, bounds.Height - Dpi.Height3);
		}
	}

	public override void RecalcSize()
	{
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Invalid comparison between Unknown and I4
		//IL_0854: Unknown result type (might be due to invalid IL or missing references)
		//IL_085a: Invalid comparison between Unknown and I4
		if (SuspendLayout)
		{
			return;
		}
		TabFormStripControl tabFormStripControl = ContainerControl as TabFormStripControl;
		Form val = null;
		if (tabFormStripControl != null)
		{
			val = ((Control)tabFormStripControl).FindForm();
		}
		if (_ᐷ.Parent == this)
		{
			_ᐷ.Displayed = true;
			_ᐷ.RecalcSize();
			_ᐷ.LeftInternal = Bounds.Right - _ᐷ.WidthInternal;
			_ᐷ.TopInternal = Bounds.Y + (Ჺ ? Dpi.Height4 : Dpi.Height1);
		}
		Rectangle bounds = _0D97();
		_ᐵ.Bounds = bounds;
		_ᐵ.RecalcSize();
		if (_ᐵ.WidthInternal > bounds.Width)
		{
			__1CC4 = bounds.Width - Dpi.Width(_Ჿ * 2) - _ᐵ.WidthInternal;
			if (__1CC0)
			{
				_1CCB();
			}
			else if (tabFormStripControl != null)
			{
				__1CC1 = new ButtonX();
				((Control)__1CC1).Name = "sysLeftScrollButton";
				__1CC1.Symbol = "\ue314";
				__1CC1.SymbolSet = eSymbolSet.Material;
				__1CC1.Shape = new RoundRectangleShapeDescriptor(0);
				((Control)__1CC1).Click += _1CCD;
				__1CC2 = new ButtonX();
				((Control)__1CC2).Name = "sysRightScrollButton";
				__1CC2.Symbol = "\ue315";
				__1CC2.SymbolSet = eSymbolSet.Material;
				__1CC2.Shape = new RoundRectangleShapeDescriptor(0);
				((Control)__1CC2).Click += _1CCC;
				_1CCB();
				((Control)tabFormStripControl).Controls.Add((Control)(object)__1CC1);
				((Control)tabFormStripControl).Controls.Add((Control)(object)__1CC2);
				__1CC0 = true;
			}
			bounds.X += Dpi.Width(_Ჿ);
			bounds.Width -= Dpi.Width(_Ჿ) * 2;
			_ᐵ.Bounds = bounds;
			_ᐵ.RecalcSize();
			if (__1CC7 < __1CC4)
			{
				__1CC7 = __1CC4;
			}
			_ᐵ.ScrollPosition = __1CC7;
			__1CC3 = bounds;
		}
		else if (__1CC0)
		{
			if (__1CC1 != null)
			{
				if (((Control)__1CC1).Parent != null)
				{
					((Control)__1CC1).Parent.Controls.Remove((Control)(object)__1CC1);
				}
				((Control)__1CC1).Click -= _1CCD;
				((Component)(object)__1CC1).Dispose();
				__1CC1 = null;
			}
			if (__1CC2 != null)
			{
				if (((Control)__1CC2).Parent != null)
				{
					((Control)__1CC2).Parent.Controls.Remove((Control)(object)__1CC2);
				}
				((Control)__1CC2).Click -= _1CCC;
				((Component)(object)__1CC2).Dispose();
				__1CC2 = null;
			}
			__1CC0 = false;
		}
		if (_ᐵ.HeightInternal < 0)
		{
			_ᐵ.HeightInternal = 0;
		}
		bool flag = false;
		if (_Ꮺ.CaptionVisible)
		{
			Rectangle rectangle = _0D98();
			_ᐶ.Bounds = rectangle;
			_ᐶ.RecalcSize();
			Size frameBorderSize = SystemInformation.FrameBorderSize;
			if (tabFormStripControl != null)
			{
				if (val != null)
				{
					if (_ප != null)
					{
						_ප.SetVisibleDirect(val.ShowIcon && _Ꮺ.ShowIcon);
					}
					if (_ඵ != null)
					{
						_ඵ.SetVisibleDirect(val.ShowIcon && _Ꮺ.ShowIcon);
					}
				}
				if (val is TabParentForm tabParentForm)
				{
					Ἇ ἇ = tabParentForm.Ꮛ();
					frameBorderSize.Width = ἇ.ἒ;
					frameBorderSize.Height = ἇ.ἑ;
				}
			}
			if (_Ꮺ.CaptionHeight == 0 && _ᐷ.TopInternal < frameBorderSize.Height - 1)
			{
				if (val != null && (int)val.WindowState == 2)
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
						subItem.TopInternal += (flag ? 2 : Dpi.Height4);
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
			Rectangle rectangle2 = rectangle;
			if (_ඵ != null)
			{
				rectangle2.X = _ඵ.Bounds.Right + Dpi.Width6;
				rectangle2.Width -= _ඵ.Bounds.Right + Dpi.Width6;
			}
			if (_ᐷ != null)
			{
				rectangle2.Width -= rectangle2.Right - _ᐷ.Bounds.Left + Dpi.Width6;
			}
			if (tabFormStripControl != null)
			{
				Size size = Size.Empty;
				Graphics val2 = tabFormStripControl.CreateGraphics();
				try
				{
					string text = tabFormStripControl.TitleText;
					if (string.IsNullOrEmpty(text) && val != null)
					{
						text = ((Control)val).Text;
					}
					if (tabFormStripControl._0D62 != null)
					{
						MarkupDrawContext markupDrawContext = new MarkupDrawContext(val2, ((Control)tabFormStripControl).Font, Color.Black, (int)((Control)tabFormStripControl).RightToLeft == 1, rectangle2, hotKeyPrefixVisible: false);
						markupDrawContext.AllowMultiLine = false;
						_0CB4 obj = tabFormStripControl._0D62;
						obj.Measure(rectangle2.Size, markupDrawContext);
						size = obj.Bounds.Size;
						tabFormStripControl.අ = Rectangle.Empty;
					}
					else
					{
						size = TextDrawing.MeasureString(val2, text, ((Control)tabFormStripControl).Font);
					}
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
				rectangle2.X += size.Width + Dpi.Width4;
				rectangle2.Width -= size.Width + Dpi.Width4;
			}
			if (rectangle2.Width > 0)
			{
				_Ჾ.Bounds = rectangle2;
				_Ჾ.RecalcSize();
				if (_Ჾ.SubItems.Count > 0)
				{
					int num4 = 3;
					if (flag)
					{
						num4++;
					}
					int num5 = 0;
					foreach (BaseItem subItem3 in _Ჾ.SubItems)
					{
						subItem3.TopInternal += num4;
						num5 = Math.Max(subItem3.Bounds.Bottom, num5);
					}
					if (_Ჾ._0DBC != null)
					{
						_Ჾ._0DBC.TopInternal += num4;
					}
					if (num5 > _Ჾ.HeightInternal)
					{
						_Ჾ.SetDisplayRectangle(new Rectangle(_Ჾ.Bounds.X, _Ჾ.Bounds.Y, _Ჾ.Bounds.Width, num5));
					}
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

	private void _1CCC(object _٠, EventArgs ؿ)
	{
		int num = Math.Max(__1CC4, __1CC7 - Dpi.Width(__1CC5));
		if (num != __1CC7)
		{
			ᨊ();
			_1CCE();
			__1CC7 = num;
			__1CC6.Animations.Add(new ခ(_ᐵ, "ScrollPosition", _ᐵ.ScrollPosition, num));
			__1CC6.Start();
		}
	}

	private void _1CCD(object _٠, EventArgs ؿ)
	{
		int num = Math.Min(0, __1CC7 + Dpi.Width(__1CC5));
		if (num != __1CC7)
		{
			ᨊ();
			_1CCE();
			__1CC7 = num;
			__1CC6.Animations.Add(new ခ(_ᐵ, "ScrollPosition", _ᐵ.ScrollPosition, num));
			__1CC6.Start();
		}
	}

	private void _1CCE()
	{
		if (__1CC6 == null)
		{
			__1CC6 = new _1A6A(AnimationEasing.EaseOutQuint, 300);
			__1CC6.AnimationCompleted += ચ;
		}
	}

	private void ચ(object _٠, EventArgs ؿ)
	{
		_1A18 obj = (_1A18)_٠;
		obj.AnimationCompleted -= ચ;
		obj.Dispose();
	}

	private void ᨊ()
	{
		if (__1CC6 == null)
		{
			return;
		}
		DateTime now = DateTime.Now;
		while (!__1CC6.IsDisposed)
		{
			Application.DoEvents();
			if (DateTime.Now.Subtract(now).TotalMilliseconds > 1000.0)
			{
				ᨋ();
				break;
			}
		}
		__1CC6 = null;
	}

	private void ᨋ()
	{
		_1A18 obj = __1CC6;
		__1CC6 = null;
		if (obj != null)
		{
			obj.Stop();
			obj.Dispose();
		}
	}

	private Rectangle _0D97()
	{
		Rectangle result = _Ꮺ._0D97();
		if (_ᐷ.Parent == this)
		{
			result.Width -= _ᐷ.WidthInternal + Dpi.Width2;
		}
		return result;
	}

	private Rectangle _0D98()
	{
		return _Ꮺ._0D98();
	}

	public override BaseItem Copy()
	{
		Ჷ ჷ = new Ჷ(_Ꮺ);
		CopyToItem(ჷ);
		return ჷ;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		Ჷ objCopy = copy as Ჷ;
		base.CopyToItem(objCopy);
	}

	public override void InternalClick(MouseButtons mb, Point mpos)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		_ᐵ.InternalClick(mb, mpos);
		if (_ᐷ.Parent == this)
		{
			_ᐷ.InternalClick(mb, mpos);
		}
		if (_Ꮺ.CaptionVisible)
		{
			_ᐶ.InternalClick(mb, mpos);
			_Ჾ.InternalClick(mb, mpos);
		}
	}

	public override void InternalDoubleClick(MouseButtons mb, Point mpos)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		_ᐵ.InternalDoubleClick(mb, mpos);
		if (_Ꮺ.CaptionVisible)
		{
			_ᐶ.InternalDoubleClick(mb, mpos);
			_Ჾ.InternalDoubleClick(mb, mpos);
		}
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		_ᐵ.InternalMouseDown(objArg);
		if (_ᐷ.Parent == this)
		{
			_ᐷ.InternalMouseDown(objArg);
		}
		if (_Ꮺ.CaptionVisible)
		{
			if ((DesignMode && _ᐶ.ItemAtLocation(objArg.X, objArg.Y) != null) || !DesignMode)
			{
				_ᐶ.InternalMouseDown(objArg);
			}
			if ((DesignMode && _Ჾ.ItemAtLocation(objArg.X, objArg.Y) != null) || !DesignMode)
			{
				_Ჾ.InternalMouseDown(objArg);
			}
		}
	}

	public override void InternalMouseHover()
	{
		_ᐵ.InternalMouseHover();
		if (_ᐷ.Parent == this)
		{
			_ᐷ.InternalMouseHover();
		}
		if (_Ꮺ.CaptionVisible)
		{
			_ᐶ.InternalMouseHover();
			_Ჾ.InternalMouseHover();
		}
	}

	public override void InternalMouseLeave()
	{
		_ᐵ.InternalMouseLeave();
		if (_ᐷ.Parent == this)
		{
			_ᐷ.InternalMouseLeave();
		}
		if (_Ꮺ.CaptionVisible)
		{
			_ᐶ.InternalMouseLeave();
			_Ჾ.InternalMouseLeave();
		}
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		_ᐵ.InternalMouseMove(objArg);
		if (_ᐷ.Parent == this)
		{
			_ᐷ.InternalMouseMove(objArg);
		}
		if (_Ꮺ.CaptionVisible)
		{
			_ᐶ.InternalMouseMove(objArg);
			_Ჾ.InternalMouseMove(objArg);
		}
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		_ᐵ.InternalMouseUp(objArg);
		if (_ᐷ.Parent == this)
		{
			_ᐷ.InternalMouseUp(objArg);
		}
		if (_Ꮺ.CaptionVisible)
		{
			_ᐶ.InternalMouseUp(objArg);
			_Ჾ.InternalMouseUp(objArg);
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
				_Ჾ.InternalKeyDown(objArg);
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

	private void _1CCF()
	{
		if (__1CC8)
		{
			if (__1CC9 == null)
			{
				__1CC9 = new NewTabFormItem();
				_ᐵ.SubItems.Add(__1CC9);
				__1CC9.Click += _1CD0;
			}
		}
		else if (__1CC9 != null)
		{
			__1CC9.Parent.SubItems.Remove(__1CC9);
			__1CC9.Dispose();
			__1CC9 = null;
		}
		NeedRecalcSize = true;
		RecalcSize();
	}

	private void _1CD0(object _٠, EventArgs ؿ)
	{
		OnCreateNewTab(EventArgs.Empty);
	}

	protected virtual void OnCreateNewTab(EventArgs e)
	{
		this.Ს?.Invoke(this, e);
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
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		_ = EffectiveStyle;
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
			_ප.SetSystemItem(b: true);
			_ᐶ.SubItems.__0AF3(_ප, 0);
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
		if (ContainerControl is TabFormStripControl tabFormStripControl)
		{
			Point point = new Point(_ප.LeftInternal, _ප.Bounds.Bottom + 1);
			point = ((Control)tabFormStripControl).PointToScreen(point);
			tabFormStripControl.ᐪ(point);
		}
	}

	public InsertPosition GetInsertPosition(Point pScreen, BaseItem DragItem)
	{
		return _ᐵ.GetInsertPosition(pScreen, DragItem);
	}

	public void DrawReversibleMarker(int iPos, bool Before)
	{
	}

	public void InsertItemAt(BaseItem objItem, int iPos, bool Before)
	{
	}
}
