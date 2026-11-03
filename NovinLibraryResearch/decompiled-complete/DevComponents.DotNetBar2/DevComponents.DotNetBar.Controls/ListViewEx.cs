using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Controls;

[ToolboxBitmap(typeof(ListViewEx), "Controls.ListViewEx.ico")]
[ToolboxItem(true)]
public class ListViewEx : ListView, INonClientControl
{
	private class _21AC : NativeWindow
	{
		private ListViewEx ӭ;

		private bool ݠ;

		private Point ၽ = Point.Empty;

		public _21AC(ListViewEx parent)
		{
			ӭ = parent;
			IntPtr intPtr = new IntPtr(_0EFE.SendMessage(((Control)parent).Handle, 4127, IntPtr.Zero, IntPtr.Zero));
			if (intPtr != IntPtr.Zero)
			{
				((NativeWindow)this).AssignHandle(intPtr);
			}
		}

		protected override void WndProc(ref Message m)
		{
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Invalid comparison between Unknown and I4
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Expected O, but got Unknown
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Expected O, but got Unknown
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			if (((Message)(ref m)).Msg == 15)
			{
				_0EFE.PAINTSTRUCT ps = default(_0EFE.PAINTSTRUCT);
				IntPtr intPtr = _0EFE.BeginPaint(((Message)(ref m)).HWnd, ref ps);
				try
				{
					Graphics val = Graphics.FromHdc(intPtr);
					try
					{
						_0EFE.RECT r = default(_0EFE.RECT);
						_0EFE.GetWindowRect(((Message)(ref m)).HWnd, ref r);
						Rectangle rectangle = new Rectangle(0, 0, r.Width, r.Height);
						using BufferedBitmap bufferedBitmap = new BufferedBitmap(intPtr, rectangle);
						ӭ._2197(bufferedBitmap.Graphics, rectangle);
						IList list = ӭ._21A1();
						int num = 0;
						foreach (ColumnHeader item in list)
						{
							ColumnHeader val2 = item;
							Rectangle rectangle2 = new Rectangle(num, 0, val2.Width, r.Height);
							ListViewItemStates val3 = (ListViewItemStates)512;
							if (ݠ && rectangle2.Contains(ၽ))
							{
								Rectangle rectangle3 = rectangle2;
								rectangle3.Inflate(-6, 0);
								if (rectangle3.Contains(ၽ))
								{
									val3 = (ListViewItemStates)(val3 | 1);
								}
							}
							ӭ._2198(new DrawListViewColumnHeaderEventArgs(bufferedBitmap.Graphics, rectangle2, val2.DisplayIndex, val2, val3, SystemColors.ControlText, Color.Empty, ӭ.ColumnHeaderFont ?? ((Control)ӭ).Font));
							num += val2.Width;
						}
						bufferedBitmap.Render(val);
						return;
					}
					finally
					{
						val.Dispose();
					}
				}
				finally
				{
					_0EFE.EndPaint(((Message)(ref m)).HWnd, ref ps);
				}
			}
			if (((Message)(ref m)).Msg == 513)
			{
				if ((int)((ListView)ӭ).HeaderStyle == 2)
				{
					ݠ = true;
					ၽ = new Point(_0EFE._0F0B(((Message)(ref m)).LParam), _0EFE._0F0D(((Message)(ref m)).LParam));
					_0EFE.RedrawWindow(((Message)(ref m)).HWnd, IntPtr.Zero, IntPtr.Zero, _0EFE.RedrawWindowFlags.RDW_INVALIDATE);
				}
			}
			else if (((Message)(ref m)).Msg == 514)
			{
				ݠ = false;
				ၽ = Point.Empty;
			}
			else if (((Message)(ref m)).Msg == 512 && ݠ)
			{
				ၽ = new Point(_0EFE._0F0B(((Message)(ref m)).LParam), _0EFE._0F0D(((Message)(ref m)).LParam));
			}
			((NativeWindow)this).WndProc(ref m);
		}
	}

	private struct Ⰷ
	{
		public IntPtr Ⰸ;

		public IntPtr Ⰹ;

		public int Ⰺ;
	}

	private struct Ⰻ
	{
		public Ⰷ Ⰼ;

		public int Ⰽ;

		public IntPtr श;

		public _0EFE.RECT ᑠ;

		public IntPtr Ⰾ;

		public int Ⰿ;

		public IntPtr Ⱀ;
	}

	private ᶨ ᵹ;

	private ElementStyle ᇧ;

	private Font __218E;

	private _21AC _218F;

	private Office2007ListViewColorTable _2190;

	private Office2007CheckBoxColorTable _2191;

	private Size ᢜ = new Size(13, 13);

	private Color _ᛋ = Color.Empty;

	private bool _ᒖ = true;

	private Point ݠ = Point.Empty;

	private bool _2192 = true;

	private BaseRenderer ਛ;

	private BaseRenderer ਜ;

	private eRenderMode ԁ = eRenderMode.Global;

	private ColorScheme լ;

	private Color _ᖣ = Color.Empty;

	private bool __2193;

	private bool __2194;

	private FieldInfo __2195;

	private FieldInfo __2196;

	[Category("Appearance")]
	[Description("Indicates column header font.")]
	[DefaultValue(null)]
	public Font ColumnHeaderFont
	{
		get
		{
			return __218E;
		}
		set
		{
			if (value != __218E)
			{
				Font oldValue = __218E;
				__218E = value;
				OnColumnHeaderFontChanged(oldValue, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates item foreground color when control or item is disabled.")]
	public Color DisabledForeColor
	{
		get
		{
			return _ᛋ;
		}
		set
		{
			_ᛋ = value;
		}
	}

	[Category("Behavior")]
	[Description("Indicates whether control displays focus cues when focused.")]
	[DefaultValue(true)]
	public virtual bool FocusCuesEnabled
	{
		get
		{
			return _ᒖ;
		}
		set
		{
			_ᒖ = value;
			if (((Control)this).Focused)
			{
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool OwnerDraw
	{
		get
		{
			return ((ListView)this).OwnerDraw;
		}
		set
		{
			((ListView)this).OwnerDraw = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(eRenderMode.Global)]
	public eRenderMode RenderMode
	{
		get
		{
			return ԁ;
		}
		set
		{
			if (ԁ != value)
			{
				ԁ = value;
				_2190 = null;
				((Control)this).Invalidate(true);
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public BaseRenderer Renderer
	{
		get
		{
			return ਜ;
		}
		set
		{
			ਜ = value;
			_2190 = null;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public BorderStyle BorderStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((ListView)this).BorderStyle;
		}
		set
		{
		}
	}

	[Description("Specifies the control border style. Default value has Class property set so the system style for the control is used.")]
	[Category("Style")]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle Border => ᇧ;

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((ListView)this).CreateParams;
			createParams.ExStyle &= ~(createParams.ExStyle & 0x200);
			return createParams;
		}
	}

	ElementStyle INonClientControl.ᖕ => ᶍ();

	IntPtr INonClientControl.ᖖ => ((Control)this).Handle;

	int INonClientControl.ᖗ => ((Control)this).Width;

	int INonClientControl.ᖘ => ((Control)this).Height;

	bool INonClientControl.ᖙ => ((Control)this).IsHandleCreated;

	Color INonClientControl.ᖚ => ((Control)this).BackColor;

	[Browsable(false)]
	[Description("Specifies back color when Enabled=false")]
	[Category("Appearance")]
	public Color DisabledBackColor
	{
		get
		{
			return _ᖣ;
		}
		set
		{
			_ᖣ = value;
			if (!((Control)this).Enabled)
			{
				((Control)this).Invalidate();
			}
		}
	}

	public ListViewEx()
	{
		ᇧ = new ElementStyle();
		ᇧ.Class = ElementStyleClassKeys.ListViewBorderKey;
		ᇧ.StyleChanged += ᶋ;
		ᵹ = new ᶨ(this, eScrollBarSkin.Optimized);
		OwnerDraw = true;
		((Control)this).DoubleBuffered = true;
		BorderStyle = (BorderStyle)0;
		StyleManager.Register((Control)(object)this);
	}

	protected override void Dispose(bool disposing)
	{
		StyleManager.Unregister((Control)(object)this);
		if (ᵹ != null)
		{
			ᵹ._1A17();
		}
		if (ᇧ != null)
		{
			ᇧ.StyleChanged -= ᶋ;
		}
		((ListView)this).Dispose(disposing);
	}

	protected virtual void OnColumnHeaderFontChanged(Font oldValue, Font newValue)
	{
		if (((Control)this).IsHandleCreated)
		{
			((Control)this).Invalidate(true);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		((Control)this).Invalidate(true);
		ResetCachedColorTableReference();
	}

	private void _2197(Graphics _٥, Rectangle ल)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		Office2007ListViewColorTable office2007ListViewColorTable = ጧ();
		DisplayHelp.FillRectangle(_٥, ल, office2007ListViewColorTable.ColumnBackground);
		Pen val = new Pen(office2007ListViewColorTable.Border);
		try
		{
			_٥.DrawLine(val, ल.X, ल.Bottom - 1, ल.Right, ल.Bottom - 1);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		_218F = new _21AC(this);
		((ListView)this).OnHandleCreated(e);
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		if (_218F != null)
		{
			((NativeWindow)_218F).ReleaseHandle();
			_218F = null;
		}
		((ListView)this).OnHandleDestroyed(e);
	}

	public void ResetHeaderHandler()
	{
		if (((Control)this).IsHandleCreated)
		{
			if (_218F != null)
			{
				((NativeWindow)_218F).ReleaseHandle();
				_218F = null;
			}
			_218F = new _21AC(this);
		}
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		if (Environment.OSVersion.Version.Major < 6 && ((Control)this).Visible)
		{
			_0EFE.PostMessage(((Control)this).Handle, 288, IntPtr.Zero, IntPtr.Zero);
		}
		((Control)this).OnVisibleChanged(e);
	}

	public void ResetCachedColorTableReference()
	{
		_2190 = null;
		_2191 = null;
	}

	private Office2007ListViewColorTable ጧ()
	{
		if (_2190 == null && GetRenderer() is Office2007Renderer office2007Renderer)
		{
			_2190 = office2007Renderer.ColorTable.ListViewEx;
		}
		return _2190;
	}

	private Office2007CheckBoxColorTable ᢍ()
	{
		if (_2191 == null && GetRenderer() is Office2007Renderer office2007Renderer)
		{
			_2191 = office2007Renderer.ColorTable.CheckBoxItem;
		}
		return _2191;
	}

	private void _2198(DrawListViewColumnHeaderEventArgs ؿ)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Invalid comparison between Unknown and I4
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected I4, but got Unknown
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Invalid comparison between Unknown and I4
		Graphics graphics = ؿ.Graphics;
		Rectangle bounds = ؿ.Bounds;
		Office2007ListViewColorTable office2007ListViewColorTable = ጧ();
		Color color = office2007ListViewColorTable.ColumnBackground.Start;
		Color color2 = office2007ListViewColorTable.ColumnBackground.End;
		if ((ؿ.State & 1) == 1 && !color2.IsEmpty)
		{
			Color color3 = color;
			color = color2;
			color2 = color3;
		}
		DisplayHelp.FillRectangle(graphics, bounds, color, color2, office2007ListViewColorTable.ColumnBackground.GradientAngle);
		Pen val = new Pen(office2007ListViewColorTable.Border);
		try
		{
			graphics.DrawLine(val, bounds.X, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		Pen val2 = new Pen(office2007ListViewColorTable.ColumnSeparator);
		try
		{
			graphics.DrawLine(val2, bounds.Right - 1, bounds.Y + 3, bounds.Right - 1, bounds.Bottom - 4);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		TextFormatFlags val3 = (TextFormatFlags)264228;
		HorizontalAlignment textAlign = ؿ.Header.TextAlign;
		switch (textAlign - 1)
		{
		case 1:
			val3 = (TextFormatFlags)(val3 | 1);
			break;
		case 0:
			val3 = (TextFormatFlags)(val3 | 2);
			break;
		}
		if (ؿ.Header.ImageList != null && (ؿ.Header.ImageIndex >= 0 || (ؿ.Header.ImageKey != null && ؿ.Header.ImageKey.Length > 0)))
		{
			Image val4 = null;
			val4 = ((ؿ.Header.ImageIndex < 0) ? ؿ.Header.ImageList.Images[ؿ.Header.ImageKey] : ؿ.Header.ImageList.Images[ؿ.Header.ImageIndex]);
			if (val4 != null)
			{
				Rectangle rectangle = new Rectangle(bounds.X + 2, bounds.Y + (bounds.Height - val4.Height) / 2, val4.Width, val4.Height);
				if ((int)((Control)this).RightToLeft == 1)
				{
					rectangle.X = bounds.Right - rectangle.Width - 2;
					bounds.Width -= rectangle.Width + 2;
				}
				else
				{
					bounds.Width -= rectangle.Width;
					bounds.X += rectangle.Width;
				}
				graphics.DrawImage(val4, rectangle);
			}
		}
		Color color4 = ؿ.ForeColor;
		if (!office2007ListViewColorTable.ColumnForeColor.IsEmpty)
		{
			color4 = office2007ListViewColorTable.ColumnForeColor;
		}
		_2199(graphics, ؿ.Font, color4, val3, ؿ.Header.Text, bounds);
	}

	protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
	{
		((ListView)this).OnDrawColumnHeader(e);
	}

	private void _2199(Graphics _٥, Font _0EE7, Color _0BDE, TextFormatFlags _219A, string _06EA, Rectangle ल)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		int width = TextRenderer.MeasureText(" ", _0EE7).Width;
		ल.Inflate(-(width / 2), 0);
		TextRenderer.DrawText((IDeviceContext)(object)_٥, _06EA, _0EE7, ल, _0BDE, _219A);
	}

	private void _219B(Graphics _٥, Rectangle ल, ListViewItem _06EC, ListViewItemStates య)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Invalid comparison between Unknown and I4
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Invalid comparison between Unknown and I4
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Invalid comparison between Unknown and I4
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Invalid comparison between Unknown and I4
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Invalid comparison between Unknown and I4
		if (!((Control)this).Enabled)
		{
			return;
		}
		if ((int)య == 0 && _06EC.Selected && (int)((ListView)this).View == 1 && ((ListView)this).FullRowSelect && ((Control)this).Focused)
		{
			ल.X++;
		}
		if (!_06EC.BackColor.IsEmpty)
		{
			SolidBrush val = new SolidBrush(_06EC.BackColor);
			try
			{
				_٥.FillRectangle((Brush)(object)val, ल);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		bool flag = _219C(య, (ListViewItemStates)1) || ((int)య == 0 && _06EC.Selected);
		if (flag)
		{
			Office2007ListViewColorTable office2007ListViewColorTable = ጧ();
			ल.Height--;
			ल.Width--;
			Pen val2 = new Pen(office2007ListViewColorTable.SelectionBorder);
			try
			{
				_٥.DrawRectangle(val2, ल);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			ल.Height++;
			ल.Width++;
			Rectangle r = new Rectangle(ल.X, ल.Y + 1, ल.Width, ल.Height - 2);
			DisplayHelp.FillRectangle(_٥, r, office2007ListViewColorTable.SelectionBackground);
		}
		else if (_219C(య, (ListViewItemStates)64) && ((ListView)this).HotTracking)
		{
			Office2007Renderer office2007Renderer = GetRenderer() as Office2007Renderer;
			ᯊ.ᯓ(_٥, office2007Renderer.ColorTable.ButtonItemColors[0].MouseOver, ल, RoundRectangleShapeDescriptor.RectangleShape);
		}
		if (_219C(య, (ListViewItemStates)16) && ((!_219C(య, (ListViewItemStates)64) && (int)((ListView)this).View != 0) || flag))
		{
			Rectangle ल2 = _06EC.Bounds;
			if (((int)((ListView)this).View == 1 && !((ListView)this).FullRowSelect) || (int)((ListView)this).View == 3)
			{
				ल2 = _06EC.GetBounds((ItemBoundsPortion)2);
			}
			else if ((int)((ListView)this).View == 1 && ((ListView)this).FullRowSelect)
			{
				ल2 = ल;
			}
			else if ((int)((ListView)this).View == 2)
			{
				ल2 = ल;
			}
			if (flag)
			{
				ल2.Y++;
				ल2.Height -= 2;
			}
			ᛵ(_٥, ल2, _06EC);
		}
		else if (flag && (int)((ListView)this).View == 1 && ((ListView)this).FullRowSelect && ((Control)this).Focused)
		{
			Rectangle rectangle = ल;
			rectangle.Y++;
			rectangle.Height -= 2;
			Region clip = _٥.Clip;
			Rectangle clip2 = rectangle;
			clip2.Width--;
			_٥.SetClip(clip2);
			ᛵ(_٥, rectangle, _06EC);
			_٥.Clip = clip;
		}
	}

	private bool _219C(ListViewItemStates _219D, ListViewItemStates _219E)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return (ListViewItemStates)(_219D & _219E) == _219E;
	}

	protected override void OnDrawItem(DrawListViewItemEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Invalid comparison between Unknown and I4
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Invalid comparison between Unknown and I4
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Invalid comparison between Unknown and I4
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Invalid comparison between Unknown and I4
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Invalid comparison between Unknown and I4
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		if (((int)e.State == 0 && (int)((ListView)this).View != 3 && (int)((ListView)this).View != 4 && (int)((ListView)this).View != 2 && (int)((ListView)this).View != 0) || (int)((ListView)this).View == 1)
		{
			((ListView)this).OnDrawItem(e);
			return;
		}
		_21A9(e);
		Graphics graphics = e.Graphics;
		Rectangle bounds = e.Bounds;
		if ((int)((ListView)this).View == 3)
		{
			bounds = e.Item.GetBounds((ItemBoundsPortion)2);
		}
		else if ((int)((ListView)this).View == 2)
		{
			bounds = e.Item.GetBounds((ItemBoundsPortion)2);
		}
		_219B(graphics, bounds, e.Item, e.State);
		Color color = e.Item.ForeColor;
		if (_219C(e.State, (ListViewItemStates)1) && ((Control)this).Enabled)
		{
			Office2007ListViewColorTable office2007ListViewColorTable = ጧ();
			if (!office2007ListViewColorTable.SelectionForeColor.IsEmpty)
			{
				color = office2007ListViewColorTable.SelectionForeColor;
			}
		}
		_21A0(graphics, e.Item, null, e.Item.Font, color, e.State);
		((ListView)this).OnDrawItem(e);
	}

	private bool _219F(ListViewItem _06EC)
	{
		if (_06EC.ImageList != null && (_06EC.ImageIndex >= 0 || _06EC.ImageKey.Length > 0))
		{
			return true;
		}
		return false;
	}

	private void _21A0(Graphics _٥, ListViewItem _06EC, ColumnHeader _173D, Font _0EE7, Color _065B, ListViewItemStates య)
	{
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Invalid comparison between Unknown and I4
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Invalid comparison between Unknown and I4
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Invalid comparison between Unknown and I4
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Invalid comparison between Unknown and I4
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Invalid comparison between Unknown and I4
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fc: Invalid comparison between Unknown and I4
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Invalid comparison between Unknown and I4
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0505: Invalid comparison between Unknown and I4
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Invalid comparison between Unknown and I4
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Invalid comparison between Unknown and I4
		//IL_0530: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Invalid comparison between Unknown and I4
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Invalid comparison between Unknown and I4
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Invalid comparison between Unknown and I4
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Invalid comparison between Unknown and I4
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Invalid comparison between Unknown and I4
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0553: Invalid comparison between Unknown and I4
		//IL_0511: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Invalid comparison between Unknown and I4
		//IL_0568: Unknown result type (might be due to invalid IL or missing references)
		//IL_056e: Invalid comparison between Unknown and I4
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Invalid comparison between Unknown and I4
		//IL_0594: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Invalid comparison between Unknown and I4
		bool flag = _219F(_06EC);
		if (flag && (_173D == null || _173D.Width > 4))
		{
			Rectangle bounds = _06EC.GetBounds((ItemBoundsPortion)1);
			int num = _06EC.ImageIndex;
			if (num < 0)
			{
				num = _06EC.ImageList.Images.IndexOfKey(_06EC.ImageKey);
			}
			if ((int)((ListView)this).View != 1 && (int)((ListView)this).View != 3 && ((ListView)this).StateImageList != null)
			{
				bounds.X += ((ListView)this).StateImageList.ImageSize.Width + 3;
			}
			else if ((int)((ListView)this).View == 2 && ((ListView)this).CheckBoxes && ((ListView)this).Groups.Count == 0)
			{
				bounds.X += ((ListView)this).SmallImageList.ImageSize.Width;
			}
			else if ((int)((ListView)this).View == 0 && (_06EC.ImageList.ImageSize.Width < bounds.Width || _06EC.ImageList.ImageSize.Height < bounds.Height))
			{
				if (_06EC.ImageList.ImageSize.Width < bounds.Width)
				{
					bounds.X += (bounds.Width - _06EC.ImageList.ImageSize.Width) / 2;
				}
				if (_06EC.ImageList.ImageSize.Height < bounds.Height)
				{
					bounds.Y += (bounds.Height - _06EC.ImageList.ImageSize.Height) / 2;
				}
			}
			Region val = null;
			if (_173D != null && _173D.Width < bounds.Width)
			{
				Rectangle clip = bounds;
				clip.Width = _173D.Width;
				val = _٥.Clip;
				_٥.SetClip(clip);
			}
			if (bounds.Width > 2)
			{
				_٥.DrawImage(_06EC.ImageList.Images[num], bounds.Location);
			}
			if (val != null)
			{
				_٥.Clip = val;
			}
		}
		Rectangle bounds2 = _06EC.GetBounds((ItemBoundsPortion)2);
		if (bounds2.Width > 2)
		{
			eTextFormat eTextFormat = eTextFormat.NoPrefix | eTextFormat.SingleLine;
			eTextFormat = (((int)((ListView)this).View != 4 || _06EC.SubItems.Count <= 1) ? (eTextFormat | eTextFormat.VerticalCenter) : eTextFormat);
			if ((int)((ListView)this).View == 0)
			{
				eTextFormat = eTextFormat.EndEllipsis | eTextFormat.HorizontalCenter | eTextFormat.NoClipping | eTextFormat.WordBreak;
			}
			else if ((int)((ListView)this).View == 1 && _173D != null)
			{
				eTextFormat |= eTextFormat.EndEllipsis;
				if ((int)_173D.TextAlign == 2)
				{
					eTextFormat |= eTextFormat.HorizontalCenter;
				}
				else if ((int)_173D.TextAlign == 1)
				{
					eTextFormat |= eTextFormat.Right;
				}
				bounds2.X += 2;
			}
			else if ((int)((ListView)this).View == 3 || (int)((ListView)this).View == 2)
			{
				bounds2.X += 2;
			}
			if (!BarFunctions.IsVista)
			{
				bounds2.Inflate(0, 1);
			}
			TextDrawing.DrawString(_٥, _06EC.Text, _0EE7, _065B, bounds2, eTextFormat);
			if ((int)((ListView)this).View == 4 && _06EC.SubItems.Count > 1)
			{
				Size size = TextDrawing.MeasureString(_٥, _06EC.Text, _0EE7);
				bounds2.Y += size.Height;
				bounds2.Height -= size.Height;
				Color foreColor = _06EC.SubItems[1].ForeColor;
				_065B = ((foreColor.IsEmpty || !(foreColor != _065B)) ? SystemColors.ControlDarkDark : foreColor);
				TextDrawing.DrawString(_٥, _06EC.SubItems[1].Text, _0EE7, _065B, bounds2, eTextFormat);
			}
		}
		if (((int)((ListView)this).View == 1 || ((ListView)this).StateImageList != null) && ((ListView)this).StateImageList != null && _06EC.StateImageIndex >= 0 && _06EC.StateImageIndex < ((ListView)this).StateImageList.Images.Count)
		{
			Rectangle bounds3 = _06EC.GetBounds((ItemBoundsPortion)1);
			if ((int)((ListView)this).View == 1 || (int)((ListView)this).View == 3)
			{
				bounds3.X -= 19;
			}
			else if ((int)((ListView)this).View == 0 && bounds3.Width > ((ListView)this).StateImageList.ImageSize.Width)
			{
				bounds3.X += (bounds3.Width - ((ListView)this).StateImageList.ImageSize.Width) / 2;
				bounds3.Y++;
			}
			else if ((int)((ListView)this).View == 4 && bounds3.Height > ((ListView)this).StateImageList.ImageSize.Height)
			{
				bounds3.Y += (bounds3.Height - ((ListView)this).StateImageList.ImageSize.Height) / 2;
				bounds3.X++;
			}
			((ListView)this).StateImageList.Draw(_٥, bounds3.Location, _06EC.StateImageIndex);
		}
		if (((ListView)this).CheckBoxes && ((int)((ListView)this).View == 1 || (int)((ListView)this).View == 3 || (int)((ListView)this).View == 2 || (int)((ListView)this).View == 0) && ((ListView)this).StateImageList == null)
		{
			Rectangle bounds4 = _06EC.GetBounds((ItemBoundsPortion)1);
			if ((int)((ListView)this).View == 0)
			{
				bounds4.X += 9;
			}
			else if ((int)((ListView)this).View == 3)
			{
				bounds4.X -= 18;
			}
			else if ((int)((ListView)this).View == 2 && flag && ((ListView)this).Groups.Count > 0)
			{
				bounds4.X -= 20;
			}
			else if ((int)((ListView)this).View == 2)
			{
				bounds4.X -= 3;
			}
			else
			{
				bounds4.X -= 21;
			}
			ᢏ ᢏ = ᣓ._191F(null);
			Office2007CheckBoxColorTable office2007CheckBoxColorTable = ᢍ();
			Office2007CheckBoxStateColorTable ዅ = office2007CheckBoxColorTable.Default;
			if ((య & 2) != 0 || !((Control)this).Enabled)
			{
				ዅ = office2007CheckBoxColorTable.Disabled;
			}
			ᢏ.ᢓ(_٥, new Rectangle(bounds4.X + 4, bounds4.Y + (bounds4.Height - ᢜ.Height) / 2, ᢜ.Width, ᢜ.Height), ዅ, (CheckState)(_06EC.Checked ? 1 : 0));
		}
	}

	protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Invalid comparison between Unknown and I4
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		_21A9(e);
		Graphics graphics = e.Graphics;
		bool flag = false;
		if (((ListView)this).FullRowSelect && _219C(e.ItemState, (ListViewItemStates)1) && e.Header != null && (e.Header.DisplayIndex == 0 || (e.Bounds.X <= 0 && e.Header.DisplayIndex > 0 && ((ListView)this).Scrollable)))
		{
			Rectangle bounds = e.Item.Bounds;
			if ((_219F(e.Item) || e.Header.DisplayIndex != e.Header.Index) && e.Header.DisplayIndex == 0)
			{
				Rectangle bounds2 = e.Item.GetBounds((ItemBoundsPortion)1);
				int num = Math.Min(e.Header.Width, bounds2.Right - bounds.X + 1);
				bounds.Width -= num;
				bounds.X += num;
			}
			_219B(graphics, bounds, e.Item, e.ItemState);
			flag = true;
		}
		if (e.ColumnIndex == 0)
		{
			if (!((ListView)this).FullRowSelect || !_219C(e.ItemState, (ListViewItemStates)1))
			{
				Rectangle bounds3 = e.Item.GetBounds((ItemBoundsPortion)2);
				_219B(graphics, bounds3, e.Item, e.ItemState);
				flag = true;
			}
			Color color = e.Item.ForeColor;
			Office2007ListViewColorTable office2007ListViewColorTable = ጧ();
			if (_219C(e.ItemState, (ListViewItemStates)1) && !office2007ListViewColorTable.SelectionForeColor.IsEmpty)
			{
				color = office2007ListViewColorTable.SelectionForeColor;
			}
			if (!((Control)this).Enabled)
			{
				color = (_ᛋ.IsEmpty ? SystemColors.ControlDark : _ᛋ);
			}
			_21A0(graphics, e.Item, e.Header, e.Item.Font, color, e.ItemState);
			if (flag && ((ListView)this).FullRowSelect && _219C(e.ItemState, (ListViewItemStates)1) && (int)((ListView)this).View == 1)
			{
				for (int i = 1; i < e.Item.SubItems.Count; i++)
				{
					ListViewSubItem val = e.Item.SubItems[i];
					Rectangle bounds4 = val.Bounds;
					if (bounds4.IntersectsWith(((Control)this).DisplayRectangle))
					{
						_21A2(graphics, val, bounds4, e.ItemState, (i < ((ListView)this).Columns.Count) ? ((ListView)this).Columns[i] : null, e.Item.BackColor, i);
					}
				}
			}
		}
		else
		{
			if (((ListView)this).FullRowSelect && _219C(e.ItemState, (ListViewItemStates)1) && e.Header.DisplayIndex != e.Header.Index)
			{
				Rectangle bounds5 = e.Bounds;
				_219B(graphics, bounds5, e.Item, e.ItemState);
				flag = true;
			}
			if (e.Header.Width > 0)
			{
				_21A2(graphics, e.SubItem, e.Bounds, e.ItemState, ((ListView)this).Columns[e.ColumnIndex], e.Item.BackColor, e.ColumnIndex);
			}
		}
		((ListView)this).OnDrawSubItem(e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDisabledForeColor()
	{
		return !_ᛋ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDisabledForeColor()
	{
		DisabledForeColor = Color.Empty;
	}

	private IList _21A1()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		IList list = (IList)((ListView)this).Columns;
		if (((ListView)this).AllowColumnReorder)
		{
			ArrayList arrayList = new ArrayList((ICollection)((ListView)this).Columns);
			foreach (ColumnHeader item in list)
			{
				ColumnHeader val = item;
				if (val.DisplayIndex >= 0)
				{
					arrayList[val.DisplayIndex] = val;
				}
			}
			list = arrayList;
		}
		return list;
	}

	private void _21A2(Graphics _٥, ListViewSubItem _21A3, Rectangle ल, ListViewItemStates య, ColumnHeader _173D, Color _21A4, int _21A5)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Invalid comparison between Unknown and I4
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Invalid comparison between Unknown and I4
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		if (!_219C(య, (ListViewItemStates)1) && ((Control)this).Enabled)
		{
			if (!_21A3.BackColor.IsEmpty)
			{
				SolidBrush val = new SolidBrush(_21A3.BackColor);
				try
				{
					_٥.FillRectangle((Brush)(object)val, ल);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			else if (!_21A4.IsEmpty)
			{
				SolidBrush val2 = new SolidBrush(_21A4);
				try
				{
					_٥.FillRectangle((Brush)(object)val2, ल);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
		}
		eTextFormat eTextFormat = eTextFormat.EndEllipsis | eTextFormat.NoClipping | eTextFormat.NoPrefix | eTextFormat.SingleLine | eTextFormat.VerticalCenter;
		if (_173D != null)
		{
			if ((int)_173D.TextAlign == 2)
			{
				eTextFormat |= eTextFormat.HorizontalCenter;
			}
			else if ((int)_173D.TextAlign == 1)
			{
				eTextFormat |= eTextFormat.Right;
			}
		}
		Color color = _21A3.ForeColor;
		if (_219C(య, (ListViewItemStates)1) && ((Control)this).Enabled && (_21A5 == 0 || ((ListView)this).FullRowSelect))
		{
			Office2007ListViewColorTable office2007ListViewColorTable = ጧ();
			if (!office2007ListViewColorTable.SelectionForeColor.IsEmpty)
			{
				color = office2007ListViewColorTable.SelectionForeColor;
			}
		}
		if (!((Control)this).Enabled)
		{
			color = (_ᛋ.IsEmpty ? SystemColors.ControlDark : _ᛋ);
		}
		Rectangle bounds = Rectangle.Inflate(ल, -2, (!BarFunctions.IsVista) ? 1 : 0);
		TextDrawing.DrawString(_٥, _21A3.Text, _21A3.Font, color, bounds, eTextFormat);
	}

	private void ᛵ(Graphics _٥, Rectangle ल, ListViewItem _06EC)
	{
		if (_ᒖ)
		{
			ControlPaint.DrawFocusRectangle(_٥, ल, _06EC.ForeColor, _06EC.BackColor);
		}
	}

	private Rectangle _21A6(Rectangle _21A7, bool _21A8, ListViewItem _06EC)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		Rectangle result = _21A7;
		if ((int)((ListView)this).View == 1)
		{
			if (!((ListView)this).FullRowSelect && _06EC.SubItems.Count > 0)
			{
				ListViewSubItem val = _06EC.SubItems[0];
				Size size = TextRenderer.MeasureText(val.Text, val.Font);
				result = new Rectangle(_21A7.X, _21A7.Y, size.Width, size.Height);
				result.X += 4;
				result.Width++;
			}
			else
			{
				result.X += 4;
				result.Width -= 4;
			}
			if (_21A8)
			{
				result.X--;
			}
		}
		return result;
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		ݠ = new Point(e.X, e.Y);
		if ((int)((ListView)this).View == 0 && ((ListView)this).CheckBoxes)
		{
			ListViewHitTestInfo val = ((ListView)this).HitTest(ݠ);
			if (val.Item == null)
			{
				val = ((ListView)this).HitTest(new Point(e.X + 9, e.Y));
			}
			if (val.Item != null)
			{
				Rectangle bounds = val.Item.GetBounds((ItemBoundsPortion)1);
				bounds.X += 9;
				if (new Rectangle(bounds.X + 4, bounds.Y + (bounds.Height - ᢜ.Height) / 2, ᢜ.Width - 4, ᢜ.Height).Contains(ݠ))
				{
					val.Item.Checked = !val.Item.Checked;
				}
			}
		}
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		((Control)this).OnMouseUp(e);
		if ((int)((ListView)this).View == 1 && ((ListView)this).FullRowSelect && !((Control)this).IsDisposed && !((ListView)this).CheckBoxes)
		{
			ListViewItem itemAt = ((ListView)this).GetItemAt(5, e.Y);
			ListViewItem itemAt2 = ((ListView)this).GetItemAt(5, ݠ.Y);
			if (itemAt != null && itemAt == itemAt2 && itemAt.Selected)
			{
				itemAt.Focused = true;
			}
		}
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		if (_2192)
		{
			((Control)this).Refresh();
			_2192 = false;
		}
		((Control)this).OnMouseEnter(e);
	}

	protected override void OnLostFocus(EventArgs e)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Invalid comparison between Unknown and I4
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		if (((ListView)this).HideSelection && !((ListView)this).VirtualMode && ((ListView)this).SelectedItems.Count > 0)
		{
			foreach (ListViewItem selectedItem in ((ListView)this).SelectedItems)
			{
				ListViewItem val = selectedItem;
				Rectangle bounds = val.Bounds;
				if ((int)((ListView)this).View == 1)
				{
					bounds.X = 0;
					bounds.Width = ((Control)this).Width;
				}
				else if ((int)((ListView)this).View == 0)
				{
					bounds.Inflate(32, 32);
				}
				((Control)this).Invalidate(bounds);
			}
		}
		((Control)this).OnLostFocus(e);
	}

	protected override void WndProc(ref Message m)
	{
		if (ᵹ != null)
		{
			if (ᵹ.WndProc(ref m))
			{
				((ListView)this).WndProc(ref m);
			}
		}
		else
		{
			((ListView)this).WndProc(ref m);
		}
	}

	public virtual BaseRenderer GetRenderer()
	{
		if (ԁ == eRenderMode.Global && GlobalManager.Renderer != null)
		{
			return GlobalManager.Renderer;
		}
		if (ԁ == eRenderMode.Custom && ਜ != null)
		{
			return ਜ;
		}
		if (ਛ == null)
		{
			ਛ = new Office2007Renderer();
		}
		return ਛ;
	}

	private ElementStyle ᶍ()
	{
		ᇧ.SetColorScheme(GetColorScheme());
		bool ല = false;
		return ElementStyleDisplay.റ(ᇧ, out ല);
	}

	private void ᶋ(object _٠, EventArgs ؿ)
	{
		if (BarFunctions.IsHandleValid((Control)(object)this))
		{
			_089F.RECT lprcUpdate = new _089F.RECT(0, 0, ((Control)this).Width, ((Control)this).Height);
			_089F.RedrawWindow(((Control)this).Handle, ref lprcUpdate, IntPtr.Zero, 1025u);
		}
	}

	private void ᖫ(ref Message Ⴘ)
	{
		((ListView)this).WndProc(ref Ⴘ);
	}

	void INonClientControl.BaseWndProc(ref Message Ⴘ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᖫ
		this.ᖫ(ref Ⴘ);
	}

	private ItemPaintArgs ᖬ(Graphics _٥)
	{
		ItemPaintArgs itemPaintArgs = new ItemPaintArgs(this as IOwner, (Control)(object)this, _٥, GetColorScheme());
		itemPaintArgs.ໝ = GetRenderer();
		itemPaintArgs.DesignerSelection = false;
		itemPaintArgs.GlassEnabled = !((Component)this).DesignMode && _0EFE.ষ;
		return itemPaintArgs;
	}

	ItemPaintArgs INonClientControl.GetItemPaintArgs(Graphics _٥)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᖬ
		return this.ᖬ(_٥);
	}

	private void ᖮ(PaintEventArgs ؿ)
	{
	}

	void INonClientControl.PaintBackground(PaintEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᖮ
		this.ᖮ(ؿ);
	}

	protected virtual ColorScheme GetColorScheme()
	{
		BaseRenderer renderer = GetRenderer();
		if (renderer is Office2007Renderer)
		{
			return ((Office2007Renderer)renderer).ColorTable.LegacyColors;
		}
		if (լ == null)
		{
			լ = new ColorScheme(eDotNetBarStyle.Office2007);
		}
		return լ;
	}

	private Point ᖱ(Point ᖲ)
	{
		return ((Control)this).PointToScreen(ᖲ);
	}

	Point INonClientControl.PointToScreen(Point ᖲ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᖱ
		return this.ᖱ(ᖲ);
	}

	private void ᖴ(ref Rectangle ल)
	{
	}

	void INonClientControl.AdjustClientRectangle(ref Rectangle ल)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᖴ
		this.ᖴ(ref ल);
	}

	private void ᖵ(ref Rectangle ल)
	{
	}

	void INonClientControl.AdjustBorderRectangle(ref Rectangle ल)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᖵ
		this.ᖵ(ref ल);
	}

	private void ᖶ(Graphics _٥)
	{
	}

	void INonClientControl.RenderNonClient(Graphics _٥)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᖶ
		this.ᖶ(_٥);
	}

	private void _21A9(DrawListViewItemEventArgs ؿ)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		if (!((ListView)this).HideSelection)
		{
			if (!__2193)
			{
				__2195 = ((object)ؿ).GetType().GetField("state", BindingFlags.Instance | BindingFlags.NonPublic);
				__2193 = true;
			}
			if ((object)__2195 != null)
			{
				_21AA(ؿ, __2195, ؿ.Item.Selected, ؿ.State);
			}
		}
	}

	private void _21A9(DrawListViewSubItemEventArgs ؿ)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		if (!((ListView)this).HideSelection)
		{
			if (!__2194)
			{
				__2196 = ((object)ؿ).GetType().GetField("itemState", BindingFlags.Instance | BindingFlags.NonPublic);
				__2194 = true;
			}
			if ((object)__2196 != null)
			{
				_21AA(ؿ, __2196, ؿ.Item.Selected, ؿ.ItemState);
			}
		}
	}

	private void _21AA(object ໄ, FieldInfo ግ, bool _21AB, ListViewItemStates _219D)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Invalid comparison between Unknown and I4
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		bool flag = (_219D & 1) != 0;
		if (_21AB != flag)
		{
			ListViewItemStates val = _219D;
			val = (ListViewItemStates)((!_21AB) ? (val & -2) : (val | 1));
			try
			{
				ግ.SetValue(ໄ, val);
			}
			catch
			{
			}
		}
	}

	[SpecialName]
	bool INonClientControl.get_Enabled()
	{
		return ((Control)this).Enabled;
	}

	[SpecialName]
	void INonClientControl.set_Enabled(bool P_0)
	{
		((Control)this).Enabled = P_0;
	}
}
