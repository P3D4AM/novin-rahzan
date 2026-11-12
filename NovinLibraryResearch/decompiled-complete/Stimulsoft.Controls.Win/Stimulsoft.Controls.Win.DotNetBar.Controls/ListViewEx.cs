using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxBitmap(typeof(ListViewEx), "Controls.ListViewEx.ico")]
[ToolboxItem(false)]
public class ListViewEx : ListView, INonClientControl
{
	private struct NMHDR
	{
		public IntPtr hwndFrom;

		public IntPtr idFrom;

		public int code;
	}

	private struct NMCUSTOMDRAW
	{
		public NMHDR nmcd;

		public int dwDrawStage;

		public IntPtr hdc;

		public WinApi.RECT rc;

		public IntPtr dwItemSpec;

		public int uItemState;

		public IntPtr lItemlParam;
	}

	private class HeaderHandler : NativeWindow
	{
		private ListViewEx m_Parent;

		private bool m_MouseDown;

		private Point m_MouseDownPoint = Point.Empty;

		public HeaderHandler(ListViewEx parent)
		{
			m_Parent = parent;
			IntPtr intPtr = new IntPtr(WinApi.SendMessage(((Control)parent).Handle, 4127, IntPtr.Zero, IntPtr.Zero));
			if (intPtr != IntPtr.Zero)
			{
				((NativeWindow)this).AssignHandle(intPtr);
			}
		}

		protected override void WndProc(ref Message m)
		{
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Invalid comparison between Unknown and I4
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
				WinApi.PAINTSTRUCT ps = default(WinApi.PAINTSTRUCT);
				IntPtr intPtr = WinApi.BeginPaint(((Message)(ref m)).HWnd, ref ps);
				try
				{
					Graphics val = Graphics.FromHdc(intPtr);
					try
					{
						WinApi.RECT r = default(WinApi.RECT);
						WinApi.GetWindowRect(((Message)(ref m)).HWnd, ref r);
						Rectangle rectangle = new Rectangle(0, 0, r.Width, r.Height);
						using BufferedBitmap bufferedBitmap = new BufferedBitmap(intPtr, rectangle);
						m_Parent.PaintHeaderBackground(bufferedBitmap.Graphics, rectangle);
						IList orderedColumnList = m_Parent.GetOrderedColumnList();
						int num = 0;
						foreach (ColumnHeader item in orderedColumnList)
						{
							ColumnHeader val2 = item;
							Rectangle rectangle2 = new Rectangle(num, 0, val2.Width, r.Height);
							ListViewItemStates val3 = (ListViewItemStates)512;
							if (m_MouseDown && rectangle2.Contains(m_MouseDownPoint))
							{
								Rectangle rectangle3 = rectangle2;
								rectangle3.Inflate(-6, 0);
								if (rectangle3.Contains(m_MouseDownPoint))
								{
									val3 = (ListViewItemStates)(val3 | 1);
								}
							}
							m_Parent.DrawColumnHeaderInternal(new DrawListViewColumnHeaderEventArgs(bufferedBitmap.Graphics, rectangle2, val2.DisplayIndex, val2, val3, SystemColors.ControlText, Color.Empty, m_Parent.ColumnHeaderFont ?? ((Control)m_Parent).Font));
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
					WinApi.EndPaint(((Message)(ref m)).HWnd, ref ps);
				}
			}
			if (((Message)(ref m)).Msg == 513)
			{
				if ((int)((ListView)m_Parent).HeaderStyle == 2)
				{
					m_MouseDown = true;
					m_MouseDownPoint = new Point(WinApi.LOWORD(((Message)(ref m)).LParam), WinApi.HIWORD(((Message)(ref m)).LParam));
					WinApi.RedrawWindow(((Message)(ref m)).HWnd, IntPtr.Zero, IntPtr.Zero, WinApi.RedrawWindowFlags.RDW_INVALIDATE);
				}
			}
			else if (((Message)(ref m)).Msg == 514)
			{
				m_MouseDown = false;
				m_MouseDownPoint = Point.Empty;
			}
			else if (((Message)(ref m)).Msg == 512 && m_MouseDown)
			{
				m_MouseDownPoint = new Point(WinApi.LOWORD(((Message)(ref m)).LParam), WinApi.HIWORD(((Message)(ref m)).LParam));
			}
			((NativeWindow)this).WndProc(ref m);
		}
	}

	private NonClientPaintHandler m_NCPainter;

	private ElementStyle m_BorderStyle;

	private Font _ColumnHeaderFont;

	private HeaderHandler m_HeaderHandler;

	private Office2007ListViewColorTable m_ListViewColorTable;

	private Office2007CheckBoxColorTable m_CheckBoxColorTable;

	private Size m_CheckSignSize = new Size(13, 13);

	private Color _DisabledForeColor = Color.Empty;

	private bool _FocusCuesEnabled = true;

	private Point m_MouseDown = Point.Empty;

	private bool m_FirstMouseEnter = true;

	private BaseRenderer m_DefaultRenderer;

	private BaseRenderer m_Renderer;

	private eRenderMode m_RenderMode = eRenderMode.Global;

	private ColorScheme m_ColorScheme;

	private bool _IsItemInitialized;

	private bool _IsSubItemInitialized;

	private FieldInfo _ItemStateField;

	private FieldInfo _SubItemStateField;

	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Indicates column header font.")]
	public Font ColumnHeaderFont
	{
		get
		{
			return _ColumnHeaderFont;
		}
		set
		{
			if (value != _ColumnHeaderFont)
			{
				Font columnHeaderFont = _ColumnHeaderFont;
				_ColumnHeaderFont = value;
				OnColumnHeaderFontChanged(columnHeaderFont, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates item foreground color when control or item is disabled.")]
	public Color DisabledForeColor
	{
		get
		{
			return _DisabledForeColor;
		}
		set
		{
			_DisabledForeColor = value;
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether control displays focus cues when focused.")]
	public virtual bool FocusCuesEnabled
	{
		get
		{
			return _FocusCuesEnabled;
		}
		set
		{
			_FocusCuesEnabled = value;
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
			return m_RenderMode;
		}
		set
		{
			if (m_RenderMode != value)
			{
				m_RenderMode = value;
				m_ListViewColorTable = null;
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
			return m_Renderer;
		}
		set
		{
			m_Renderer = value;
			m_ListViewColorTable = null;
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

	[Browsable(true)]
	[Category("Style")]
	[Description("Specifies the control border style. Default value has Class property set so the system style for the control is used.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle Border => m_BorderStyle;

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((ListView)this).CreateParams;
			createParams.ExStyle &= ~(createParams.ExStyle & 0x200);
			return createParams;
		}
	}

	ElementStyle INonClientControl.BorderStyle => GetBorderStyle();

	IntPtr INonClientControl.Handle => ((Control)this).Handle;

	int INonClientControl.Width => ((Control)this).Width;

	int INonClientControl.Height => ((Control)this).Height;

	bool INonClientControl.IsHandleCreated => ((Control)this).IsHandleCreated;

	Color INonClientControl.BackColor => ((Control)this).BackColor;

	public ListViewEx()
	{
		m_BorderStyle = new ElementStyle();
		m_BorderStyle.Class = ElementStyleClassKeys.ListViewBorderKey;
		m_BorderStyle.StyleChanged += BorderStyle_StyleChanged;
		m_NCPainter = new NonClientPaintHandler(this, eScrollBarSkin.Optimized);
		OwnerDraw = true;
		((Control)this).DoubleBuffered = true;
		BorderStyle = (BorderStyle)0;
		StyleManager.Register((Control)(object)this);
	}

	protected override void Dispose(bool disposing)
	{
		StyleManager.Unregister((Control)(object)this);
		if (m_NCPainter != null)
		{
			m_NCPainter.Dispose();
		}
		if (m_BorderStyle != null)
		{
			m_BorderStyle.StyleChanged -= BorderStyle_StyleChanged;
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

	private void PaintHeaderBackground(Graphics g, Rectangle r)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		Office2007ListViewColorTable colorTable = GetColorTable();
		DisplayHelp.FillRectangle(g, r, colorTable.ColumnBackground);
		Pen val = new Pen(colorTable.Border);
		try
		{
			g.DrawLine(val, r.X, r.Bottom - 1, r.Right, r.Bottom - 1);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		m_HeaderHandler = new HeaderHandler(this);
		((ListView)this).OnHandleCreated(e);
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		if (m_HeaderHandler != null)
		{
			((NativeWindow)m_HeaderHandler).ReleaseHandle();
			m_HeaderHandler = null;
		}
		((ListView)this).OnHandleDestroyed(e);
	}

	public void ResetHeaderHandler()
	{
		if (((Control)this).IsHandleCreated)
		{
			if (m_HeaderHandler != null)
			{
				((NativeWindow)m_HeaderHandler).ReleaseHandle();
				m_HeaderHandler = null;
			}
			m_HeaderHandler = new HeaderHandler(this);
		}
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		if (Environment.OSVersion.Version.Major < 6 && ((Control)this).Visible)
		{
			WinApi.PostMessage(((Control)this).Handle, 288, IntPtr.Zero, IntPtr.Zero);
		}
		((Control)this).OnVisibleChanged(e);
	}

	public void ResetCachedColorTableReference()
	{
		m_ListViewColorTable = null;
		m_CheckBoxColorTable = null;
	}

	private Office2007ListViewColorTable GetColorTable()
	{
		if (m_ListViewColorTable == null && GetRenderer() is Office2007Renderer office2007Renderer)
		{
			m_ListViewColorTable = office2007Renderer.ColorTable.ListViewEx;
		}
		return m_ListViewColorTable;
	}

	private Office2007CheckBoxColorTable GetCheckBoxColorTable()
	{
		if (m_CheckBoxColorTable == null && GetRenderer() is Office2007Renderer office2007Renderer)
		{
			m_CheckBoxColorTable = office2007Renderer.ColorTable.CheckBoxItem;
		}
		return m_CheckBoxColorTable;
	}

	private void DrawColumnHeaderInternal(DrawListViewColumnHeaderEventArgs e)
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
		//IL_0102: Invalid comparison between Unknown and I4
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Invalid comparison between Unknown and I4
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Invalid comparison between Unknown and I4
		Graphics graphics = e.Graphics;
		Rectangle bounds = e.Bounds;
		Office2007ListViewColorTable colorTable = GetColorTable();
		Color color = colorTable.ColumnBackground.Start;
		Color color2 = colorTable.ColumnBackground.End;
		if ((e.State & 1) == 1 && !color2.IsEmpty)
		{
			Color color3 = color;
			color = color2;
			color2 = color3;
		}
		DisplayHelp.FillRectangle(graphics, bounds, color, color2, colorTable.ColumnBackground.GradientAngle);
		Pen val = new Pen(colorTable.Border);
		try
		{
			graphics.DrawLine(val, bounds.X, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		Pen val2 = new Pen(colorTable.ColumnSeparator);
		try
		{
			graphics.DrawLine(val2, bounds.Right - 1, bounds.Y + 3, bounds.Right - 1, bounds.Bottom - 4);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		TextFormatFlags val3 = (TextFormatFlags)264228;
		HorizontalAlignment textAlign = e.Header.TextAlign;
		if ((int)textAlign != 1)
		{
			if ((int)textAlign == 2)
			{
				val3 = (TextFormatFlags)(val3 | 1);
			}
		}
		else
		{
			val3 = (TextFormatFlags)(val3 | 2);
		}
		if (e.Header.ImageList != null && (e.Header.ImageIndex >= 0 || (e.Header.ImageKey != null && e.Header.ImageKey.Length > 0)))
		{
			Image val4 = null;
			val4 = ((e.Header.ImageIndex < 0) ? e.Header.ImageList.Images[e.Header.ImageKey] : e.Header.ImageList.Images[e.Header.ImageIndex]);
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
		DrawHeaderText(graphics, e.Font, e.ForeColor, val3, e.Header.Text, bounds);
	}

	protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
	{
		((ListView)this).OnDrawColumnHeader(e);
	}

	private void DrawHeaderText(Graphics g, Font font, Color c, TextFormatFlags flags, string text, Rectangle r)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		int width = TextRenderer.MeasureText(" ", font).Width;
		r.Inflate(-(width / 2), 0);
		TextRenderer.DrawText((IDeviceContext)(object)g, text, font, r, c, flags);
	}

	private void DrawItemBackground(Graphics g, Rectangle r, ListViewItem item, ListViewItemStates state)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Invalid comparison between Unknown and I4
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Invalid comparison between Unknown and I4
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Invalid comparison between Unknown and I4
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Invalid comparison between Unknown and I4
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Invalid comparison between Unknown and I4
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Invalid comparison between Unknown and I4
		if (!((Control)this).Enabled)
		{
			return;
		}
		if ((int)state == 0 && item.Selected && (int)((ListView)this).View == 1 && ((ListView)this).FullRowSelect && ((Control)this).Focused)
		{
			r.X++;
		}
		if (!item.BackColor.IsEmpty)
		{
			SolidBrush val = new SolidBrush(item.BackColor);
			try
			{
				g.FillRectangle((Brush)(object)val, r);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		bool flag = IsInState(state, (ListViewItemStates)1) || ((int)state == 0 && item.Selected);
		if (flag)
		{
			Office2007ListViewColorTable colorTable = GetColorTable();
			r.Height--;
			r.Width--;
			Pen val2 = new Pen(colorTable.SelectionBorder);
			try
			{
				g.DrawRectangle(val2, r);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			r.Height++;
			r.Width++;
			Rectangle r2 = new Rectangle(r.X, r.Y + 1, r.Width, r.Height - 2);
			DisplayHelp.FillRectangle(g, r2, colorTable.SelectionBackground);
		}
		else if (IsInState(state, (ListViewItemStates)64) && ((ListView)this).HotTracking)
		{
			Office2007Renderer office2007Renderer = GetRenderer() as Office2007Renderer;
			Office2007ButtonItemPainter.PaintBackground(g, office2007Renderer.ColorTable.ButtonItemColors[0].MouseOver, r, RoundRectangleShapeDescriptor.RectangleShape);
		}
		if (IsInState(state, (ListViewItemStates)16) && ((!IsInState(state, (ListViewItemStates)64) && (int)((ListView)this).View > 0) || flag))
		{
			Rectangle r3 = item.Bounds;
			if (((int)((ListView)this).View == 1 && !((ListView)this).FullRowSelect) || (int)((ListView)this).View == 3)
			{
				r3 = item.GetBounds((ItemBoundsPortion)2);
			}
			else if ((int)((ListView)this).View == 1 && ((ListView)this).FullRowSelect)
			{
				r3 = r;
			}
			else if ((int)((ListView)this).View == 2)
			{
				r3 = r;
			}
			if (flag)
			{
				r3.Y++;
				r3.Height -= 2;
			}
			DrawFocusRectangle(g, r3, item);
		}
		else if (flag && (int)((ListView)this).View == 1 && ((ListView)this).FullRowSelect && ((Control)this).Focused)
		{
			Rectangle rectangle = r;
			rectangle.Y++;
			rectangle.Height -= 2;
			Region clip = g.Clip;
			Rectangle clip2 = rectangle;
			clip2.Width--;
			g.SetClip(clip2);
			DrawFocusRectangle(g, rectangle, item);
			g.Clip = clip;
		}
	}

	private bool IsInState(ListViewItemStates currentState, ListViewItemStates testState)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return (ListViewItemStates)(currentState & testState) == testState;
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
		FixStateForHideSelection(e);
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
		DrawItemBackground(graphics, bounds, e.Item, e.State);
		Color color = e.Item.ForeColor;
		if (IsInState(e.State, (ListViewItemStates)1) && ((Control)this).Enabled)
		{
			Office2007ListViewColorTable colorTable = GetColorTable();
			if (!colorTable.SelectionForeColor.IsEmpty)
			{
				color = colorTable.SelectionForeColor;
			}
		}
		DrawListViewItem(graphics, e.Item, null, e.Item.Font, color, e.State);
		((ListView)this).OnDrawItem(e);
	}

	private bool HasImage(ListViewItem item)
	{
		if (item.ImageList != null && (item.ImageIndex >= 0 || item.ImageKey.Length > 0))
		{
			return true;
		}
		return false;
	}

	private void DrawListViewItem(Graphics g, ListViewItem item, ColumnHeader header, Font font, Color color, ListViewItemStates state)
	{
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Invalid comparison between Unknown and I4
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Invalid comparison between Unknown and I4
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Invalid comparison between Unknown and I4
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Invalid comparison between Unknown and I4
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Invalid comparison between Unknown and I4
		//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Invalid comparison between Unknown and I4
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Invalid comparison between Unknown and I4
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_050d: Invalid comparison between Unknown and I4
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Invalid comparison between Unknown and I4
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Invalid comparison between Unknown and I4
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_0510: Unknown result type (might be due to invalid IL or missing references)
		//IL_0516: Invalid comparison between Unknown and I4
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Invalid comparison between Unknown and I4
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Invalid comparison between Unknown and I4
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Invalid comparison between Unknown and I4
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Invalid comparison between Unknown and I4
		//IL_0567: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Invalid comparison between Unknown and I4
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Invalid comparison between Unknown and I4
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_0588: Invalid comparison between Unknown and I4
		//IL_05f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Invalid comparison between Unknown and I4
		//IL_05af: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Invalid comparison between Unknown and I4
		bool flag = HasImage(item);
		if (flag && (header == null || header.Width > 4))
		{
			Rectangle bounds = item.GetBounds((ItemBoundsPortion)1);
			int num = item.ImageIndex;
			if (num < 0)
			{
				num = item.ImageList.Images.IndexOfKey(item.ImageKey);
			}
			if ((int)((ListView)this).View != 1 && (int)((ListView)this).View != 3 && ((ListView)this).StateImageList != null)
			{
				bounds.X += ((ListView)this).StateImageList.ImageSize.Width + 3;
			}
			else if ((int)((ListView)this).View == 2 && ((ListView)this).CheckBoxes && ((ListView)this).Groups.Count == 0)
			{
				bounds.X += ((ListView)this).SmallImageList.ImageSize.Width;
			}
			else if ((int)((ListView)this).View == 0 && (item.ImageList.ImageSize.Width < bounds.Width || item.ImageList.ImageSize.Height < bounds.Height))
			{
				if (item.ImageList.ImageSize.Width < bounds.Width)
				{
					bounds.X += (bounds.Width - item.ImageList.ImageSize.Width) / 2;
				}
				if (item.ImageList.ImageSize.Height < bounds.Height)
				{
					bounds.Y += (bounds.Height - item.ImageList.ImageSize.Height) / 2;
				}
			}
			Region val = null;
			if (header != null && header.Width < bounds.Width)
			{
				Rectangle clip = bounds;
				clip.Width = header.Width;
				val = g.Clip;
				g.SetClip(clip);
			}
			if (bounds.Width > 2)
			{
				g.DrawImage(item.ImageList.Images[num], bounds.Location);
			}
			if (val != null)
			{
				g.Clip = val;
			}
		}
		Rectangle bounds2 = item.GetBounds((ItemBoundsPortion)2);
		if (bounds2.Width > 2)
		{
			eTextFormat eTextFormat = eTextFormat.NoPrefix | eTextFormat.SingleLine;
			eTextFormat = (((int)((ListView)this).View != 4 || item.SubItems.Count <= 1) ? (eTextFormat | eTextFormat.VerticalCenter) : (eTextFormat | eTextFormat.Default));
			if ((int)((ListView)this).View == 0)
			{
				eTextFormat = eTextFormat.EndEllipsis | eTextFormat.HorizontalCenter | eTextFormat.NoClipping | eTextFormat.WordBreak;
			}
			else if ((int)((ListView)this).View == 1 && header != null)
			{
				eTextFormat |= eTextFormat.EndEllipsis;
				if ((int)header.TextAlign == 2)
				{
					eTextFormat |= eTextFormat.HorizontalCenter;
				}
				else if ((int)header.TextAlign == 1)
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
			TextDrawing.DrawString(g, item.Text, font, color, bounds2, eTextFormat);
			if ((int)((ListView)this).View == 4 && item.SubItems.Count > 1)
			{
				Size size = TextDrawing.MeasureString(g, item.Text, font);
				bounds2.Y += size.Height;
				bounds2.Height -= size.Height;
				Color foreColor = item.SubItems[1].ForeColor;
				color = ((foreColor.IsEmpty || !(foreColor != color)) ? SystemColors.ControlDarkDark : foreColor);
				TextDrawing.DrawString(g, item.SubItems[1].Text, font, color, bounds2, eTextFormat);
			}
		}
		if (((int)((ListView)this).View == 1 || ((ListView)this).StateImageList != null) && ((ListView)this).StateImageList != null && item.StateImageIndex >= 0 && item.StateImageIndex < ((ListView)this).StateImageList.Images.Count)
		{
			Rectangle bounds3 = item.GetBounds((ItemBoundsPortion)1);
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
			((ListView)this).StateImageList.Draw(g, bounds3.Location, item.StateImageIndex);
		}
		if (((ListView)this).CheckBoxes && ((int)((ListView)this).View == 1 || (int)((ListView)this).View == 3 || (int)((ListView)this).View == 2 || (int)((ListView)this).View == 0) && ((ListView)this).StateImageList == null)
		{
			Rectangle bounds4 = item.GetBounds((ItemBoundsPortion)1);
			if ((int)((ListView)this).View == 0)
			{
				bounds4.X += (bounds4.Width - m_CheckSignSize.Width) / 2 - 4;
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
			Office2007CheckBoxItemPainter office2007CheckBoxItemPainter = PainterFactory.CreateCheckBoxItemPainter(null);
			Office2007CheckBoxColorTable checkBoxColorTable = GetCheckBoxColorTable();
			Office2007CheckBoxStateColorTable ct = checkBoxColorTable.Default;
			if ((state & 2) != 0 || !((Control)this).Enabled)
			{
				ct = checkBoxColorTable.Disabled;
			}
			office2007CheckBoxItemPainter.PaintCheckBox(g, new Rectangle(bounds4.X + 4, bounds4.Y + (bounds4.Height - m_CheckSignSize.Height) / 2, m_CheckSignSize.Width, m_CheckSignSize.Height), ct, (CheckState)(item.Checked ? 1 : 0));
		}
	}

	protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		FixStateForHideSelection(e);
		Graphics graphics = e.Graphics;
		if (((ListView)this).FullRowSelect && IsInState(e.ItemState, (ListViewItemStates)1) && e.Header != null && (e.Header.DisplayIndex == 0 || (e.Bounds.X <= 0 && e.Header.DisplayIndex > 0 && ((ListView)this).Scrollable)))
		{
			Rectangle bounds = e.Item.Bounds;
			if ((HasImage(e.Item) || e.Header.DisplayIndex != e.Header.Index) && e.Header.DisplayIndex == 0)
			{
				Rectangle bounds2 = e.Item.GetBounds((ItemBoundsPortion)1);
				int num = Math.Min(e.Header.Width, bounds2.Right - bounds.X + 1);
				bounds.Width -= num;
				bounds.X += num;
			}
			DrawItemBackground(graphics, bounds, e.Item, e.ItemState);
		}
		if (e.ColumnIndex == 0)
		{
			if (!((ListView)this).FullRowSelect || !IsInState(e.ItemState, (ListViewItemStates)1))
			{
				Rectangle bounds3 = e.Item.GetBounds((ItemBoundsPortion)2);
				DrawItemBackground(graphics, bounds3, e.Item, e.ItemState);
			}
			Color color = e.Item.ForeColor;
			Office2007ListViewColorTable colorTable = GetColorTable();
			if (IsInState(e.ItemState, (ListViewItemStates)1) && !colorTable.SelectionForeColor.IsEmpty)
			{
				color = colorTable.SelectionForeColor;
			}
			if (!((Control)this).Enabled)
			{
				color = (_DisabledForeColor.IsEmpty ? SystemColors.ControlDark : _DisabledForeColor);
			}
			DrawListViewItem(graphics, e.Item, e.Header, e.Item.Font, color, e.ItemState);
		}
		else
		{
			if (((ListView)this).FullRowSelect && IsInState(e.ItemState, (ListViewItemStates)1) && e.Header.DisplayIndex != e.Header.Index)
			{
				Rectangle bounds4 = e.Bounds;
				DrawItemBackground(graphics, bounds4, e.Item, e.ItemState);
			}
			if (e.Header.Width > 0)
			{
				PaintSubItem(graphics, e.SubItem, e.Bounds, e.ItemState, ((ListView)this).Columns[e.ColumnIndex], e.Item.BackColor, e.ColumnIndex);
			}
		}
		((ListView)this).OnDrawSubItem(e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDisabledForeColor()
	{
		return !_DisabledForeColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDisabledForeColor()
	{
		DisabledForeColor = Color.Empty;
	}

	private IList GetOrderedColumnList()
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

	private void PaintSubItem(Graphics g, ListViewSubItem subitem, Rectangle r, ListViewItemStates state, ColumnHeader header, Color itemBackColor, int columnIndex)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Invalid comparison between Unknown and I4
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Invalid comparison between Unknown and I4
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		if (!IsInState(state, (ListViewItemStates)1) && ((Control)this).Enabled)
		{
			if (!subitem.BackColor.IsEmpty)
			{
				SolidBrush val = new SolidBrush(subitem.BackColor);
				try
				{
					g.FillRectangle((Brush)(object)val, r);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			else if (!itemBackColor.IsEmpty)
			{
				SolidBrush val2 = new SolidBrush(itemBackColor);
				try
				{
					g.FillRectangle((Brush)(object)val2, r);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
		}
		eTextFormat eTextFormat = eTextFormat.EndEllipsis | eTextFormat.NoClipping | eTextFormat.NoPrefix | eTextFormat.SingleLine | eTextFormat.VerticalCenter;
		if (header != null)
		{
			if ((int)header.TextAlign == 2)
			{
				eTextFormat |= eTextFormat.HorizontalCenter;
			}
			else if ((int)header.TextAlign == 1)
			{
				eTextFormat |= eTextFormat.Right;
			}
		}
		Color color = subitem.ForeColor;
		if (IsInState(state, (ListViewItemStates)1) && ((Control)this).Enabled && (columnIndex == 0 || ((ListView)this).FullRowSelect))
		{
			Office2007ListViewColorTable colorTable = GetColorTable();
			if (!colorTable.SelectionForeColor.IsEmpty)
			{
				color = colorTable.SelectionForeColor;
			}
		}
		if (!((Control)this).Enabled)
		{
			color = (_DisabledForeColor.IsEmpty ? SystemColors.ControlDark : _DisabledForeColor);
		}
		Rectangle bounds = Rectangle.Inflate(r, -2, (!BarFunctions.IsVista) ? 1 : 0);
		TextDrawing.DrawString(g, subitem.Text, subitem.Font, color, bounds, eTextFormat);
	}

	private void DrawFocusRectangle(Graphics g, Rectangle r, ListViewItem item)
	{
		if (_FocusCuesEnabled)
		{
			ControlPaint.DrawFocusRectangle(g, r, item.ForeColor, item.BackColor);
		}
	}

	private Rectangle UpdateBounds(Rectangle originalBounds, bool drawText, ListViewItem item)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		Rectangle result = originalBounds;
		if ((int)((ListView)this).View == 1)
		{
			if (!((ListView)this).FullRowSelect && item.SubItems.Count > 0)
			{
				ListViewSubItem val = item.SubItems[0];
				Size size = TextRenderer.MeasureText(val.Text, val.Font);
				result = new Rectangle(originalBounds.X, originalBounds.Y, size.Width, size.Height);
				result.X += 4;
				result.Width++;
			}
			else
			{
				result.X += 4;
				result.Width -= 4;
			}
			if (drawText)
			{
				result.X--;
			}
		}
		return result;
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		m_MouseDown = new Point(e.X, e.Y);
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
			ListViewItem itemAt2 = ((ListView)this).GetItemAt(5, m_MouseDown.Y);
			if (itemAt != null && itemAt == itemAt2 && itemAt.Selected)
			{
				itemAt.Focused = true;
			}
		}
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		if (m_FirstMouseEnter)
		{
			((Control)this).Refresh();
			m_FirstMouseEnter = false;
		}
		((Control)this).OnMouseEnter(e);
	}

	protected override void OnLostFocus(EventArgs e)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Invalid comparison between Unknown and I4
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
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
		if (m_NCPainter != null)
		{
			if (m_NCPainter.WndProc(ref m))
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
		if (m_RenderMode == eRenderMode.Global && GlobalManager.Renderer != null)
		{
			return GlobalManager.Renderer;
		}
		if (m_RenderMode == eRenderMode.Custom && m_Renderer != null)
		{
			return m_Renderer;
		}
		if (m_DefaultRenderer == null)
		{
			m_DefaultRenderer = new Office2007Renderer();
		}
		return m_DefaultRenderer;
	}

	private ElementStyle GetBorderStyle()
	{
		m_BorderStyle.SetColorScheme(GetColorScheme());
		bool dispose = false;
		return ElementStyleDisplay.GetElementStyle(m_BorderStyle, out dispose);
	}

	private void BorderStyle_StyleChanged(object sender, EventArgs e)
	{
		if (BarFunctions.IsHandleValid((Control)(object)this))
		{
			NativeFunctions.RECT lprcUpdate = new NativeFunctions.RECT(0, 0, ((Control)this).Width, ((Control)this).Height);
			NativeFunctions.RedrawWindow(((Control)this).Handle, ref lprcUpdate, IntPtr.Zero, 1025u);
		}
	}

	void INonClientControl.BaseWndProc(ref Message m)
	{
		((ListView)this).WndProc(ref m);
	}

	ItemPaintArgs INonClientControl.GetItemPaintArgs(Graphics g)
	{
		ItemPaintArgs itemPaintArgs = new ItemPaintArgs(this as IOwner, (Control)(object)this, g, GetColorScheme());
		itemPaintArgs.Renderer = GetRenderer();
		itemPaintArgs.DesignerSelection = false;
		itemPaintArgs.GlassEnabled = !((Component)this).DesignMode && WinApi.IsGlassEnabled;
		return itemPaintArgs;
	}

	void INonClientControl.PaintBackground(PaintEventArgs e)
	{
	}

	protected virtual ColorScheme GetColorScheme()
	{
		BaseRenderer renderer = GetRenderer();
		if (renderer is Office2007Renderer)
		{
			return ((Office2007Renderer)renderer).ColorTable.LegacyColors;
		}
		if (m_ColorScheme == null)
		{
			m_ColorScheme = new ColorScheme(eDotNetBarStyle.Office2007);
		}
		return m_ColorScheme;
	}

	Point INonClientControl.PointToScreen(Point client)
	{
		return ((Control)this).PointToScreen(client);
	}

	void INonClientControl.AdjustClientRectangle(ref Rectangle r)
	{
	}

	void INonClientControl.AdjustBorderRectangle(ref Rectangle r)
	{
	}

	void INonClientControl.RenderNonClient(Graphics g)
	{
	}

	private void FixStateForHideSelection(DrawListViewItemEventArgs e)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		if (!((ListView)this).HideSelection)
		{
			if (!_IsItemInitialized)
			{
				_ItemStateField = ((object)e).GetType().GetField("state", BindingFlags.Instance | BindingFlags.NonPublic);
				_IsItemInitialized = true;
			}
			if (_ItemStateField != null)
			{
				UpdateStateBit(e, _ItemStateField, e.Item.Selected, e.State);
			}
		}
	}

	private void FixStateForHideSelection(DrawListViewSubItemEventArgs e)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		if (!((ListView)this).HideSelection)
		{
			if (!_IsSubItemInitialized)
			{
				_SubItemStateField = ((object)e).GetType().GetField("itemState", BindingFlags.Instance | BindingFlags.NonPublic);
				_IsSubItemInitialized = true;
			}
			if (_SubItemStateField != null)
			{
				UpdateStateBit(e, _SubItemStateField, e.Item.Selected, e.ItemState);
			}
		}
	}

	private void UpdateStateBit(object o, FieldInfo f, bool itemSelected, ListViewItemStates currentState)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		bool flag = (currentState & 1) > 0;
		if (itemSelected != flag)
		{
			ListViewItemStates val = currentState;
			val = (ListViewItemStates)((!itemSelected) ? (val & -2) : (val | 1));
			try
			{
				f.SetValue(o, val);
			}
			catch
			{
			}
		}
	}

	bool INonClientControl.get_Enabled()
	{
		return ((Control)this).Enabled;
	}

	void INonClientControl.set_Enabled(bool value)
	{
		((Control)this).Enabled = value;
	}
}
