using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

public class ListBoxItem : BaseItem
{
	private const int ᬧ = 2;

	internal const int ܨ = 4;

	internal const int ᬨ = 6;

	private CheckState _ݱ;

	private bool __0605;

	private Size _ܫ = new Size(13, 13);

	private Padding _ᬩ = new Padding(2);

	private Rectangle _ᬪ = Rectangle.Empty;

	private Size _ᬫ = Size.Empty;

	private bool _ኗ;

	private eMouseState _ᬬ;

	private Image _ነ;

	private bool _ԅ;

	private Color __0EA4 = Color.Empty;

	private string _ລ = "";

	private string __0EA6 = "";

	private eSymbolSet _ວ;

	private float _ຨ = 13f;

	private Color[] __137D;

	private Color __0CD1 = Color.Empty;

	private eButtonTextAlignment _ᬭ;

	private bool __0EB9 = true;

	internal Rectangle _1758
	{
		get
		{
			return _ᬪ;
		}
		set
		{
			_ᬪ = value;
		}
	}

	internal Size ᬦ => _ᬫ;

	[Browsable(false)]
	public bool IsMouseOver
	{
		get
		{
			return _ኗ;
		}
		internal set
		{
			_ኗ = value;
		}
	}

	internal Size ܦ => _ܫ;

	[Browsable(false)]
	public eMouseState CheckBoxMouseState
	{
		get
		{
			return _ᬬ;
		}
		internal set
		{
			if (_ᬬ != value)
			{
				_ᬬ = value;
				Refresh();
			}
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Category("Appearance")]
	[Description("Indicates check-box state if visible.")]
	public CheckState CheckState
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _ݱ;
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if (value != _ݱ)
			{
				CheckState ݱ = _ݱ;
				_ݱ = value;
				OnCheckStateChanged(ݱ, value);
			}
		}
	}

	[DefaultValue(false)]
	[Browsable(false)]
	public bool IsSelected
	{
		get
		{
			return _ԅ;
		}
		set
		{
			if (value != _ԅ)
			{
				SetIsSelected(value, eEventSource.Code);
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Indicates whether item changes its background colors when mouse is over the item")]
	public bool HotTracking
	{
		get
		{
			return __0605;
		}
		set
		{
			if (value != __0605)
			{
				bool oldValue = __0605;
				__0605 = value;
				OnHotTrackingChanged(oldValue, value);
			}
		}
	}

	[Description("Specifies image displayed on the item.")]
	[DefaultValue(null)]
	[Category("Appearance")]
	public Image Image
	{
		get
		{
			return _ነ;
		}
		set
		{
			if (value != _ነ)
			{
				Image ነ = _ነ;
				_ነ = value;
				OnImageChanged(ነ, value);
			}
		}
	}

	[Description("Indicates color of the Symbol.")]
	[Category("Appearance")]
	public Color SymbolColor
	{
		get
		{
			return __0EA4;
		}
		set
		{
			__0EA4 = value;
			Refresh();
		}
	}

	[Browsable(false)]
	public string SymbolRealized => __0EA6;

	[Editor("DevComponents.DotNetBar.Design.SymbolTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Category("Appearance")]
	[DefaultValue("")]
	[Description("Indicates the symbol displayed on face of the button instead of the image. Setting the symbol overrides the image setting.")]
	public string Symbol
	{
		get
		{
			return _ລ;
		}
		set
		{
			if (value != _ລ)
			{
				string ລ = _ລ;
				_ລ = value;
				OnSymbolChanged(ລ, value);
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(eSymbolSet.Awesome)]
	public eSymbolSet SymbolSet
	{
		get
		{
			return _ວ;
		}
		set
		{
			if (_ວ != value)
			{
				eSymbolSet ວ = _ວ;
				_ວ = value;
				OnSymbolSetChanged(ວ, value);
			}
		}
	}

	[DefaultValue(13f)]
	[Category("Appearance")]
	[Description("Indicates the size of the symbol in points.")]
	public float SymbolSize
	{
		get
		{
			return _ຨ;
		}
		set
		{
			if (value != _ຨ)
			{
				float ຨ = _ຨ;
				_ຨ = value;
				OnSymbolSizeChanged(ຨ, value);
			}
		}
	}

	[TypeConverter(typeof(ArrayConverter))]
	[Category("Appearance")]
	[Description("Indicates the array of colors that when set are used to draw the background of the item.")]
	[DefaultValue(null)]
	public Color[] BackColors
	{
		get
		{
			return __137D;
		}
		set
		{
			if (__137D != value)
			{
				__137D = value;
				Refresh();
			}
		}
	}

	[Category("Columns")]
	[Description("Indicates color of text.")]
	public Color TextColor
	{
		get
		{
			return __0CD1;
		}
		set
		{
			__0CD1 = value;
			Refresh();
		}
	}

	[Category("Appearance")]
	[Description("The text contained in the item.")]
	[Localizable(true)]
	[DevCoBrowsable(true)]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue("")]
	[Browsable(true)]
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

	[Browsable(true)]
	[DefaultValue(eButtonTextAlignment.Left)]
	[Description("Indicates text alignment.")]
	[Category("Appearance")]
	public eButtonTextAlignment TextAlignment
	{
		get
		{
			return _ᬭ;
		}
		set
		{
			_ᬭ = value;
			Refresh();
		}
	}

	protected override bool IsMarkupSupported => __0EB9;

	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Indicates whether text-markup support is enabled for items Text property.")]
	public bool EnableMarkup
	{
		get
		{
			return __0EB9;
		}
		set
		{
			if (__0EB9 != value)
			{
				__0EB9 = value;
				NeedRecalcSize = true;
				OnTextChanged();
			}
		}
	}

	public override BaseItem Copy()
	{
		ListBoxItem listBoxItem = new ListBoxItem();
		CopyToItem(listBoxItem);
		return listBoxItem;
	}

	internal void _0EBA(ListBoxItem _0EBB)
	{
		CopyToItem(_0EBB);
	}

	protected override void CopyToItem(BaseItem copy)
	{
		base.CopyToItem(copy);
		ListBoxItem listBoxItem = copy as ListBoxItem;
		listBoxItem.IsSelected = _ԅ;
		listBoxItem.Symbol = _ລ;
		listBoxItem.SymbolSet = _ວ;
		listBoxItem.SymbolColor = __0EA4;
		listBoxItem.SymbolSize = _ຨ;
	}

	public override void Paint(ItemPaintArgs p)
	{
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		BaseRenderer ໝ = p.ໝ;
		if (ໝ != null)
		{
			ListBoxItemRendererEventArgs listBoxItemRendererEventArgs = new ListBoxItemRendererEventArgs(this, p.Graphics);
			listBoxItemRendererEventArgs.က = p;
			ໝ.DrawListBoxItem(listBoxItemRendererEventArgs);
		}
		else
		{
			_245E obj = ᣓ._192A(this);
			if (obj != null)
			{
				ListBoxItemRendererEventArgs listBoxItemRendererEventArgs2 = new ListBoxItemRendererEventArgs(this, p.Graphics);
				listBoxItemRendererEventArgs2.က = p;
				obj.Paint(listBoxItemRendererEventArgs2);
			}
		}
		if (DesignMode && Focused)
		{
			Rectangle rect = m_Rect;
			rect.Inflate(-1, -1);
			_1A6B._1A6C(p.Graphics, rect, p.Colors.ItemDesignTimeBorder);
		}
		else if (Focused)
		{
			Rectangle rect2 = m_Rect;
			rect2.Inflate(-1, -1);
			Color color = SystemColors.Control;
			if (ໝ is Office2007Renderer)
			{
				color = ((Office2007Renderer)ໝ).ColorTable.ListBoxItem.Default.TextColor;
			}
			Pen val = new Pen(color, 1f);
			try
			{
				val.DashStyle = (DashStyle)2;
				SmoothingMode smoothingMode = p.Graphics.SmoothingMode;
				p.Graphics.SmoothingMode = (SmoothingMode)3;
				p.Graphics.DrawRectangle(val, rect2);
				p.Graphics.SmoothingMode = smoothingMode;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		DrawInsertMarker(p.Graphics);
	}

	public override void RecalcSize()
	{
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Invalid comparison between Unknown and I4
		if (!(ContainerControl is ListBoxAdv listBoxAdv) || ((Control)listBoxAdv).Disposing || ((Control)listBoxAdv).IsDisposed)
		{
			return;
		}
		bool checkBoxesVisible = listBoxAdv.CheckBoxesVisible;
		int num = Dpi.Height(listBoxAdv.ItemHeight);
		Size size = Size.Empty;
		Graphics val = BarFunctions.CreateGraphics((Control)(object)listBoxAdv);
		if (val == null)
		{
			return;
		}
		try
		{
			if (!string.IsNullOrEmpty(_ລ))
			{
				_ᬫ = _᧗(val);
				size = _ᬫ;
			}
			else if (_ነ != null)
			{
				size = Dpi.ImageSize(_ነ.Size);
			}
			if (!string.IsNullOrEmpty(Text))
			{
				Size size2 = _20B1._20B2(this, val, 0, ((Control)listBoxAdv).Font, eTextFormat.Default, (int)((Control)listBoxAdv).RightToLeft == 1);
				size.Width += size2.Width;
				size.Height = Math.Max(size.Height, size2.Height);
				if (_ነ != null || !string.IsNullOrEmpty(_ລ))
				{
					size.Width += Dpi.Width(4);
				}
			}
			else if (string.IsNullOrEmpty(_ລ) && _ነ == null)
			{
				size = new Size(Dpi.Width16, Dpi.Height16);
			}
			size.Width += _ᬩ.Horizontal;
			size.Height += _ᬩ.Vertical;
			base.RecalcSize();
		}
		finally
		{
			val.Dispose();
		}
		if (checkBoxesVisible)
		{
			size.Width += Dpi.Width(_ܫ.Width + 6);
			size.Height = Math.Max(Dpi.Height(_ܫ.Height), size.Height);
		}
		if (num > 0)
		{
			size.Height = num;
		}
		_ᬪ = Rectangle.Empty;
		m_Rect.Size = size;
		base.RecalcSize();
	}

	public override void InternalMouseEnter()
	{
		base.InternalMouseEnter();
		if (!DesignMode)
		{
			_ኗ = true;
			if (GetEnabled() && __0605)
			{
				Refresh();
			}
		}
	}

	public override void InternalMouseLeave()
	{
		base.InternalMouseLeave();
		if (!DesignMode)
		{
			_ኗ = false;
			CheckBoxMouseState = eMouseState.None;
			if (GetEnabled() && __0605)
			{
				Refresh();
			}
		}
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		if (_ᬪ.Contains(objArg.Location))
		{
			CheckBoxMouseState = eMouseState.Hot;
		}
		else
		{
			CheckBoxMouseState = eMouseState.None;
		}
		base.InternalMouseMove(objArg);
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)objArg.Button == 1048576 && !_ᬪ.IsEmpty && _ᬪ.Contains(objArg.Location))
		{
			CheckBoxMouseState = eMouseState.Down;
		}
		base.InternalMouseDown(objArg);
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Invalid comparison between Unknown and I4
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Invalid comparison between Unknown and I4
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Invalid comparison between Unknown and I4
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Invalid comparison between Unknown and I4
		if ((int)objArg.Button == 1048576 && !_ᬪ.IsEmpty && _ᬪ.Contains(objArg.Location))
		{
			CheckState = (CheckState)((int)CheckState != 1);
			CheckBoxMouseState = eMouseState.Hot;
		}
		else if ((int)objArg.Button == 1048576 && Bounds.Contains(objArg.Location))
		{
			if (ContainerControl is ListBoxAdv listBoxAdv)
			{
				if (listBoxAdv.SelectionMode != 0)
				{
					if (listBoxAdv.SelectionMode == eSelectionMode.MultiSimple)
					{
						SetIsSelected(!IsSelected, eEventSource.Mouse);
					}
					else if (listBoxAdv.SelectionMode == eSelectionMode.MultiExtended)
					{
						if ((Control.ModifierKeys & 0x20000) == 131072)
						{
							SetIsSelected(!IsSelected, eEventSource.Mouse);
						}
						else if ((Control.ModifierKeys & 0x10000) == 65536)
						{
							BaseItem baseItemContainer = listBoxAdv.GetBaseItemContainer();
							int num = baseItemContainer.SubItems.IndexOf(this);
							int num2 = -1;
							int num3 = -1;
							for (int i = 0; i < listBoxAdv.SelectedItems.Count; i++)
							{
								int num4 = baseItemContainer.SubItems.IndexOf(listBoxAdv.SelectedItems[i]);
								if (num4 > num2 || num2 < 0)
								{
									num2 = num4;
								}
								if (num4 < num3 || num3 < 0)
								{
									num3 = num4;
								}
							}
							if (num2 < 0 || num3 < 0)
							{
								SetIsSelected(isSelected: true, eEventSource.Mouse);
							}
							else if (num < num3)
							{
								for (int num5 = num3; num5 >= num; num5--)
								{
									if (baseItemContainer.SubItems[num5] is ListBoxItem listBoxItem)
									{
										listBoxItem.SetIsSelected(isSelected: true, eEventSource.Mouse);
									}
								}
							}
							else if (num > num2)
							{
								for (int j = num2; j <= num; j++)
								{
									if (baseItemContainer.SubItems[j] is ListBoxItem listBoxItem2)
									{
										listBoxItem2.SetIsSelected(isSelected: true, eEventSource.Mouse);
									}
								}
							}
						}
						else
						{
							listBoxAdv._1B3B(eEventSource.Mouse);
							SetIsSelected(isSelected: true, eEventSource.Mouse);
						}
					}
					else if (!IsSelected)
					{
						SetIsSelected(isSelected: true, eEventSource.Mouse);
					}
				}
			}
			else
			{
				SetIsSelected(isSelected: true, eEventSource.Mouse);
			}
		}
		base.InternalMouseUp(objArg);
	}

	private Size _᧗(Graphics _٥)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		Size empty = Size.Empty;
		if (_٥ == null || string.IsNullOrEmpty(_ລ))
		{
			return empty;
		}
		Font font = Symbols.GetFont(SymbolSize, SymbolSet);
		empty = TextDrawing.MeasureString(_٥, "\uf00a", font);
		int num = (int)Math.Ceiling((float)font.FontFamily.GetCellDescent(font.Style) * font.Size / (float)font.FontFamily.GetEmHeight(font.Style));
		empty.Height -= num;
		return empty;
	}

	protected virtual void OnCheckStateChanged(CheckState oldValue, CheckState newValue)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		if (ContainerControl is ListBoxAdv listBoxAdv)
		{
			ListBoxAdvItemCheckEventArgs listBoxAdvItemCheckEventArgs = new ListBoxAdvItemCheckEventArgs(this, newValue);
			listBoxAdv._1B43(listBoxAdvItemCheckEventArgs);
			if (listBoxAdvItemCheckEventArgs.Cancel)
			{
				_ݱ = oldValue;
			}
		}
		Refresh();
	}

	protected virtual void OnIsSelectedChanged(bool oldValue, bool newValue)
	{
		Refresh();
	}

	public void SetIsSelected(bool isSelected, eEventSource source)
	{
		bool ԅ = _ԅ;
		ListBoxAdv listBoxAdv = ContainerControl as ListBoxAdv;
		_ԅ = isSelected;
		listBoxAdv?._1B3A(this, source);
		OnIsSelectedChanged(ԅ, isSelected);
	}

	protected virtual void OnHotTrackingChanged(bool oldValue, bool newValue)
	{
	}

	protected virtual void OnImageChanged(Image oldValue, Image newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSymbolColor()
	{
		return !__0EA4.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSymbolColor()
	{
		SymbolColor = Color.Empty;
	}

	protected virtual void OnSymbolChanged(string oldValue, string newValue)
	{
		if (string.IsNullOrEmpty(newValue))
		{
			__0EA6 = "";
		}
		else
		{
			__0EA6 = Symbols.GetSymbol(newValue);
		}
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	protected virtual void OnSymbolSetChanged(eSymbolSet oldValue, eSymbolSet newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	protected virtual void OnSymbolSizeChanged(float oldValue, float newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		return !__0CD1.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		TextColor = Color.Empty;
	}
}
