using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;
using Stimulsoft.Controls.Win.Editors;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxBitmap(typeof(ComboBoxEx), "Controls.ComboBoxEx.ico")]
[ToolboxItem(false)]
public class ComboBoxEx : ComboBox, ICommandSource
{
	public delegate void OnDropDownChangeEventHandler(object sender, bool Expanded);

	private class InternalComboColors
	{
		public Color Background = SystemColors.Window;

		public Color Border = SystemColors.Window;

		public LinearGradientColorTable ThumbBackground;

		public Color ThumbText = SystemColors.ControlText;

		public LinearGradientColorTable ThumbBorderOuter;

		public LinearGradientColorTable ThumbBorderInner;
	}

	private class ComboTextBoxMsgHandler : NativeWindow
	{
		private struct TRACKMOUSEEVENT
		{
			public int cbSize;

			public int dwFlags;

			public IntPtr hwndTrack;

			public int dwHoverTime;
		}

		private const int WM_MOUSELEAVE = 675;

		private const int WM_MOUSEMOVE = 512;

		private const int TME_LEAVE = 2;

		[CompilerGenerated]
		private PaintEventHandler m_Paint;

		private bool m_MouseTracking;

		public event EventHandler MouseLeave;

		public event PaintEventHandler Paint
		{
			[CompilerGenerated]
			add
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				PaintEventHandler val = this.m_Paint;
				PaintEventHandler val2;
				do
				{
					val2 = val;
					PaintEventHandler value2 = (PaintEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
					val = Interlocked.CompareExchange(ref this.m_Paint, value2, val2);
				}
				while (val != val2);
			}
			[CompilerGenerated]
			remove
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				PaintEventHandler val = this.m_Paint;
				PaintEventHandler val2;
				do
				{
					val2 = val;
					PaintEventHandler value2 = (PaintEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
					val = Interlocked.CompareExchange(ref this.m_Paint, value2, val2);
				}
				while (val != val2);
			}
		}

		[DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool TrackMouseEvent(ref TRACKMOUSEEVENT lpEventTrack);

		protected override void WndProc(ref Message m)
		{
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Expected O, but got Unknown
			if (((Message)(ref m)).Msg == 512 && !m_MouseTracking)
			{
				m_MouseTracking = true;
				TRACKMOUSEEVENT lpEventTrack = default(TRACKMOUSEEVENT);
				lpEventTrack.dwFlags = 2;
				lpEventTrack.cbSize = Marshal.SizeOf(lpEventTrack);
				lpEventTrack.hwndTrack = ((NativeWindow)this).Handle;
				lpEventTrack.dwHoverTime = 0;
				m_MouseTracking = TrackMouseEvent(ref lpEventTrack);
			}
			else if (((Message)(ref m)).Msg == 675)
			{
				this.MouseLeave?.Invoke(this, new EventArgs());
				m_MouseTracking = false;
			}
			else if (((Message)(ref m)).Msg == 15)
			{
				((NativeWindow)this).WndProc(ref m);
				if (this.Paint != null)
				{
					Graphics val = Graphics.FromHwnd(((Message)(ref m)).HWnd);
					try
					{
						this.Paint.Invoke((object)this, new PaintEventArgs(val, Rectangle.Empty));
						return;
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
				return;
			}
			((NativeWindow)this).WndProc(ref m);
		}
	}

	private class ComboBoxPopupMsgHandler : NativeWindow
	{
		protected override void WndProc(ref Message m)
		{
			if (((Message)(ref m)).Msg == 277)
			{
				((NativeWindow)this).WndProc(ref m);
				WinApi.RedrawWindow(((Message)(ref m)).HWnd, IntPtr.Zero, IntPtr.Zero, (WinApi.RedrawWindowFlags)257u);
			}
			else
			{
				((NativeWindow)this).WndProc(ref m);
			}
		}
	}

	private bool allowCustomFontDrawing = true;

	private eDotNetBarStyle m_Style = eDotNetBarStyle.Office2007;

	private bool m_DefaultStyle;

	private bool m_MouseOver;

	private bool m_MouseOverThumb;

	private bool m_DroppedDown;

	private bool m_WindowsXPAware;

	private bool m_DisableInternalDrawing;

	private ImageList m_ImageList;

	private int m_DropDownHeight;

	private IntPtr m_LastFocusWindow;

	private IntPtr m_DropDownHandle = IntPtr.Zero;

	private ComboTextBoxMsgHandler m_TextWindowMsgHandler;

	private const uint GW_CHILD = 5u;

	private bool m_PreventEnterBeep;

	private string m_WatermarkText = "";

	private bool m_Focused;

	private Font m_WatermarkFont;

	private Color m_WatermarkColor = SystemColors.GrayText;

	private bool m_IsStandalone = true;

	private Timer m_MouseOverTimer;

	private Rectangle m_ThumbRect = Rectangle.Empty;

	private bool _FocusHighlightEnabled;

	private static Color _DefaultHighlightColor = Color.FromArgb(255, 255, 136);

	private Color _FocusHighlightColor = _DefaultHighlightColor;

	private bool _FocusCuesEnabled = true;

	private bool _WatermarkEnabled = true;

	private eWatermarkBehavior _WatermarkBehavior;

	private BodyElement m_TextMarkup;

	private bool m_UseCustomBackColor;

	internal BaseItem ParentItem;

	private Timer m_DropDownTrackTimer;

	private ComboBoxPopupMsgHandler _PopupMsgHandler;

	private bool _RenderThumb = true;

	private bool _RenderBorder = true;

	private int m_SelectedIndexInternal = -1;

	private bool _InSelectedIndexChanged;

	private Color _DisabledForeColor = Color.Empty;

	private Color _DisabledBackColor = Color.Empty;

	public static List<FontFamily> FontFamilies = null;

	public static PrivateFontCollection FontCollection = null;

	private ICommand _Command;

	private object _CommandParameter;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool AllowCustomFontDrawing
	{
		get
		{
			return allowCustomFontDrawing;
		}
		set
		{
			allowCustomFontDrawing = value;
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

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates the appearance of the control.")]
	public bool IsStandalone
	{
		get
		{
			return m_IsStandalone;
		}
		set
		{
			m_IsStandalone = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether watermark text is displayed when control is empty.")]
	public virtual bool WatermarkEnabled
	{
		get
		{
			return _WatermarkEnabled;
		}
		set
		{
			_WatermarkEnabled = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Localizable(true)]
	[Category("Appearance")]
	[Description("Indicates watermark text displayed inside of the control when Text is not set and control does not have input focus.")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.TextMarkupUIEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public string WatermarkText
	{
		get
		{
			return m_WatermarkText;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			m_WatermarkText = value;
			MarkupTextChanged();
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(eWatermarkBehavior.HideOnFocus)]
	[Category("Behavior")]
	[Description("Indicates watermark hiding behaviour.")]
	public eWatermarkBehavior WatermarkBehavior
	{
		get
		{
			return _WatermarkBehavior;
		}
		set
		{
			_WatermarkBehavior = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates watermark font.")]
	[DefaultValue(null)]
	public Font WatermarkFont
	{
		get
		{
			return m_WatermarkFont;
		}
		set
		{
			m_WatermarkFont = value;
			((Control)this).Invalidate(true);
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates watermark text color.")]
	public Color WatermarkColor
	{
		get
		{
			return m_WatermarkColor;
		}
		set
		{
			m_WatermarkColor = value;
			((Control)this).Invalidate(true);
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color BackColor
	{
		get
		{
			return ((ComboBox)this).BackColor;
		}
		set
		{
			((ComboBox)this).BackColor = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	public bool UseCustomBackColor
	{
		get
		{
			return m_UseCustomBackColor;
		}
		set
		{
			m_UseCustomBackColor = value;
		}
	}

	[Browsable(false)]
	[Category("Behavior")]
	[Description("Makes Combo box appear the same as built-in Combo box.")]
	[DefaultValue(false)]
	public bool DefaultStyle
	{
		get
		{
			return m_DefaultStyle;
		}
		set
		{
			if (m_DefaultStyle != value)
			{
				m_DefaultStyle = value;
				((Control)this).Invalidate(true);
			}
		}
	}

	[Browsable(false)]
	[Category("Behavior")]
	[Description("When running on WindowsXP draws control using the Windows XP Themes if theme manager is enabled.")]
	[DefaultValue(false)]
	public bool ThemeAware
	{
		get
		{
			return m_WindowsXPAware;
		}
		set
		{
			if (m_WindowsXPAware != value)
			{
				m_WindowsXPAware = value;
				if (!m_WindowsXPAware)
				{
					m_DefaultStyle = false;
				}
				else if (m_WindowsXPAware && BarFunctions.ThemedOS)
				{
					m_DefaultStyle = true;
					((ComboBox)this).FlatStyle = (FlatStyle)2;
				}
			}
		}
	}

	[Browsable(true)]
	[Category("Behavior")]
	[Description("Disables internal drawing support for the List-box portion of Combo-box.")]
	[DefaultValue(false)]
	public bool DisableInternalDrawing
	{
		get
		{
			return m_DisableInternalDrawing;
		}
		set
		{
			if (m_DisableInternalDrawing != value)
			{
				m_DisableInternalDrawing = value;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether combo box generates the audible alert when Enter key is pressed.")]
	[DefaultValue(false)]
	public bool PreventEnterBeep
	{
		get
		{
			return m_PreventEnterBeep;
		}
		set
		{
			m_PreventEnterBeep = value;
		}
	}

	[Browsable(true)]
	[Category("Behavior")]
	[Description("The ImageList control used by Combo box to draw images.")]
	[DefaultValue(null)]
	public ImageList Images
	{
		get
		{
			return m_ImageList;
		}
		set
		{
			m_ImageList = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(eDotNetBarStyle.Office2007)]
	[Description("Determines the display of the item when shown.")]
	public eDotNetBarStyle Style
	{
		get
		{
			return m_Style;
		}
		set
		{
			m_Style = value;
			UpdateBackColor();
			((Control)this).Invalidate(true);
		}
	}

	private bool MouseOverThumb
	{
		get
		{
			return m_MouseOverThumb;
		}
		set
		{
			if (m_MouseOverThumb != value)
			{
				m_MouseOverThumb = value;
				((Control)this).Invalidate();
			}
		}
	}

	private bool DroppedDownInternal
	{
		get
		{
			return m_DroppedDown;
		}
		set
		{
			if (m_DroppedDown != value)
			{
				StopDropDownTrackTimer();
				m_DroppedDown = value;
				((Control)this).Invalidate();
				this.DropDownChange?.Invoke(this, m_DroppedDown);
				if (!m_DroppedDown)
				{
					ReleasePopupHandler();
				}
			}
		}
	}

	[Browsable(false)]
	public bool IsToolbarStyle => !m_IsStandalone;

	internal bool RenderThumb
	{
		get
		{
			return _RenderThumb;
		}
		set
		{
			_RenderThumb = value;
		}
	}

	internal bool RenderBorder
	{
		get
		{
			return _RenderBorder;
		}
		set
		{
			_RenderBorder = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates text color for the text in combo-box when control Enabled property is set to false. Setting this property is effective only for DropDownList ComboBox style.")]
	public Color DisabledForeColor
	{
		get
		{
			return _DisabledForeColor;
		}
		set
		{
			_DisabledForeColor = value;
			if (!((Control)this).Enabled)
			{
				((Control)this).Invalidate();
			}
		}
	}

	[Description("Indicates control background color when control is disabled")]
	[Category("Appearance")]
	public Color DisabledBackColor
	{
		get
		{
			return _DisabledBackColor;
		}
		set
		{
			if (_DisabledBackColor != value)
			{
				_DisabledBackColor = value;
				if (!((Control)this).Enabled)
				{
					((Control)this).Invalidate();
				}
			}
		}
	}

	[Browsable(false)]
	public IntPtr DropDownHandle => m_DropDownHandle;

	internal bool MouseOver
	{
		get
		{
			return m_MouseOver;
		}
		set
		{
			if (m_MouseOver != value)
			{
				m_MouseOver = value;
				((Control)this).Invalidate(true);
			}
		}
	}

	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ComboItemsEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Localizable(true)]
	public ObjectCollection Items => ((ComboBox)this).Items;

	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates whether FocusHighlightColor is used as background color to highlight text box when it has input focus.")]
	public virtual bool FocusHighlightEnabled
	{
		get
		{
			return _FocusHighlightEnabled;
		}
		set
		{
			if (_FocusHighlightEnabled != value)
			{
				_FocusHighlightEnabled = value;
				if (((Control)this).Focused)
				{
					((Control)this).Invalidate();
				}
			}
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates color used as background color to highlight text box when it has input focus and focus highlight is enabled.")]
	public virtual Color FocusHighlightColor
	{
		get
		{
			return _FocusHighlightColor;
		}
		set
		{
			if (_FocusHighlightColor != value)
			{
				_FocusHighlightColor = value;
				if (((Control)this).Focused)
				{
					((Control)this).Invalidate();
				}
			}
		}
	}

	[DefaultValue(null)]
	[Category("Commands")]
	[Description("Indicates the command assigned to the item.")]
	public Command Command
	{
		get
		{
			return (Command)((ICommandSource)this).Command;
		}
		set
		{
			((ICommandSource)this).Command = value;
		}
	}

	ICommand ICommandSource.Command
	{
		get
		{
			return _Command;
		}
		set
		{
			bool flag = false;
			if (_Command != value)
			{
				flag = true;
			}
			if (_Command != null)
			{
				CommandManager.UnRegisterCommandSource(this, _Command);
			}
			_Command = value;
			if (value != null)
			{
				CommandManager.RegisterCommand(this, value);
			}
			if (flag)
			{
				OnCommandChanged();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Commands")]
	[Description("Indicates user defined data value that can be passed to the command when it is executed.")]
	[TypeConverter(typeof(StringConverter))]
	[Localizable(true)]
	public object CommandParameter
	{
		get
		{
			return _CommandParameter;
		}
		set
		{
			_CommandParameter = value;
		}
	}

	public event OnDropDownChangeEventHandler DropDownChange;

	[DllImport("user32")]
	private static extern bool ValidateRect(IntPtr hWnd, ref NativeFunctions.RECT pRect);

	[DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

	public ComboBoxEx()
	{
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		if (!ColorFunctions.ColorsLoaded)
		{
			NativeFunctions.RefreshSettings();
			NativeFunctions.OnDisplayChange();
			ColorFunctions.LoadColors();
		}
		m_MouseOverTimer = new Timer();
		m_MouseOverTimer.Interval = 10;
		m_MouseOverTimer.Enabled = false;
		m_MouseOverTimer.Tick += MouseOverTimerTick;
		((ComboBox)this).FlatStyle = (FlatStyle)0;
	}

	private void MarkupTextChanged()
	{
		m_TextMarkup = null;
		if (MarkupParser.IsMarkup(ref m_WatermarkText))
		{
			m_TextMarkup = MarkupParser.Parse(m_WatermarkText);
			ResizeMarkup();
		}
	}

	private void ResizeMarkup()
	{
		if (m_TextMarkup != null)
		{
			Graphics val = ((Control)this).CreateGraphics();
			try
			{
				MarkupDrawContext markupDrawContext = GetMarkupDrawContext(val);
				m_TextMarkup.Measure(GetWatermarkBounds().Size, markupDrawContext);
				Size size = m_TextMarkup.Bounds.Size;
				m_TextMarkup.Arrange(new Rectangle(GetWatermarkBounds().Location, size), markupDrawContext);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	private MarkupDrawContext GetMarkupDrawContext(Graphics g)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		return new MarkupDrawContext(g, (m_WatermarkFont == null) ? ((Control)this).Font : m_WatermarkFont, m_WatermarkColor, (int)((Control)this).RightToLeft == 1);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeWatermarkColor()
	{
		return m_WatermarkColor != SystemColors.GrayText;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetWatermarkColor()
	{
		WatermarkColor = SystemColors.GrayText;
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		((ComboBox)this).OnHandleCreated(e);
		UpdateBackColor();
		SetupTextBoxMessageHandler();
		RemoveTheme(((Control)this).Handle);
	}

	protected override void OnResize(EventArgs e)
	{
		ResizeMarkup();
		((Control)this).Invalidate();
		((ComboBox)this).OnResize(e);
	}

	private void MouseOverTimerTick(object sender, EventArgs e)
	{
		bool flag = false;
		if (((Control)this).IsHandleCreated && ((Control)this).Visible)
		{
			WinApi.RECT r = default(WinApi.RECT);
			WinApi.GetWindowRect(((Control)this).Handle, ref r);
			if (r.ToRectangle().Contains(Control.MousePosition))
			{
				flag = true;
			}
			Point pt = ((Control)this).PointToClient(Control.MousePosition);
			if (m_ThumbRect.Contains(pt))
			{
				MouseOverThumb = true;
			}
			else
			{
				MouseOverThumb = false;
			}
		}
		if (!flag)
		{
			SetMouseOverTimerEnabled(value: false);
			m_MouseOver = false;
			UpdateBackColor();
			((Control)this).Invalidate();
			if (ParentItem != null)
			{
				ParentItem.HideToolTip();
			}
		}
	}

	protected override void OnGotFocus(EventArgs e)
	{
		((ComboBox)this).OnGotFocus(e);
		if (!m_MouseOver)
		{
			m_MouseOver = true;
		}
		UpdateBackColor();
		((Control)this).Invalidate(true);
	}

	protected override void OnLostFocus(EventArgs e)
	{
		SetMouseOverTimerEnabled(value: true);
		m_LastFocusWindow = IntPtr.Zero;
		((ComboBox)this).OnLostFocus(e);
	}

	private void StartDropDownTrackTimer()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		if (m_DropDownTrackTimer == null)
		{
			m_DropDownTrackTimer = new Timer();
			m_DropDownTrackTimer.Tick += DropDownTrackTimerTick;
			m_DropDownTrackTimer.Interval = 200;
		}
		m_DropDownTrackTimer.Start();
	}

	private void StopDropDownTrackTimer()
	{
		Timer dropDownTrackTimer = m_DropDownTrackTimer;
		m_DropDownTrackTimer = null;
		if (dropDownTrackTimer != null)
		{
			dropDownTrackTimer.Stop();
			((Component)(object)dropDownTrackTimer).Dispose();
		}
	}

	private void DropDownTrackTimerTick(object sender, EventArgs e)
	{
		DroppedDownInternal = WinApi.SendMessage(((Control)this).Handle, 343, IntPtr.Zero, IntPtr.Zero) != 0;
	}

	protected override void OnDropDownClosed(EventArgs e)
	{
		DroppedDownInternal = false;
		m_SelectedIndexInternal = ((ListControl)this).SelectedIndex;
		((ComboBox)this).OnDropDownClosed(e);
	}

	protected override void OnDropDown(EventArgs e)
	{
		((ComboBox)this).OnDropDown(e);
		DroppedDownInternal = true;
	}

	[DllImport("user32", CharSet = CharSet.Unicode)]
	private static extern int SetWindowTheme(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string pszSubAppName, [MarshalAs(UnmanagedType.LPWStr)] string pszSubIdList);

	private void RemoveTheme(IntPtr handle)
	{
		bool flag = false;
		if (Environment.Version.Major > 5)
		{
			flag = true;
		}
		else if (Environment.Version.Major == 5 && Environment.Version.Minor >= 1)
		{
			flag = true;
		}
	}

	private InternalComboColors GetComboColors()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		InternalComboColors internalComboColors = new InternalComboColors();
		bool flag = m_MouseOverThumb || ((Control)this).Focused || (DroppedDownInternal && (int)((ComboBox)this).DropDownStyle > 0);
		if (flag && !((Control)this).Enabled)
		{
			flag = false;
		}
		if (BarFunctions.IsOffice2007Style(Style) && GlobalManager.Renderer is Office2007Renderer)
		{
			Office2007ComboBoxColorTable comboBox = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.ComboBox;
			Office2007ComboBoxStateColorTable office2007ComboBoxStateColorTable = ((IsToolbarStyle && !m_MouseOver) ? comboBox.Default : comboBox.DefaultStandalone);
			if (flag)
			{
				if (((ComboBox)this).DroppedDown)
				{
					office2007ComboBoxStateColorTable = comboBox.DroppedDown;
				}
				else if (flag)
				{
					office2007ComboBoxStateColorTable = comboBox.MouseOver;
				}
			}
			internalComboColors.Background = office2007ComboBoxStateColorTable.Background;
			internalComboColors.Border = office2007ComboBoxStateColorTable.Border;
			internalComboColors.ThumbBackground = office2007ComboBoxStateColorTable.ExpandBackground;
			internalComboColors.ThumbText = office2007ComboBoxStateColorTable.ExpandText;
			internalComboColors.ThumbBorderOuter = office2007ComboBoxStateColorTable.ExpandBorderOuter;
			internalComboColors.ThumbBorderInner = office2007ComboBoxStateColorTable.ExpandBorderInner;
		}
		else
		{
			ColorScheme colorScheme = new ColorScheme(Style);
			if (flag)
			{
				if (DroppedDownInternal)
				{
					internalComboColors.ThumbBackground = new LinearGradientColorTable(colorScheme.ItemPressedBackground, colorScheme.ItemPressedBackground2, colorScheme.ItemPressedBackgroundGradientAngle);
					internalComboColors.Border = colorScheme.ItemPressedBorder;
					internalComboColors.ThumbText = colorScheme.ItemPressedText;
				}
				else
				{
					if (m_MouseOverThumb)
					{
						internalComboColors.ThumbBackground = new LinearGradientColorTable(colorScheme.ItemHotBackground, colorScheme.ItemHotBackground2, colorScheme.ItemHotBackgroundGradientAngle);
					}
					else
					{
						internalComboColors.ThumbBackground = new LinearGradientColorTable(colorScheme.BarBackground, colorScheme.BarBackground2, colorScheme.BarBackgroundGradientAngle);
					}
					internalComboColors.Border = colorScheme.ItemHotBorder;
					internalComboColors.ThumbText = colorScheme.ItemHotText;
				}
			}
			else
			{
				internalComboColors.ThumbBackground = new LinearGradientColorTable(colorScheme.BarBackground, colorScheme.BarBackground2, colorScheme.BarBackgroundGradientAngle);
				if (m_MouseOver || !IsToolbarStyle)
				{
					internalComboColors.Border = colorScheme.ItemHotBorder;
				}
			}
		}
		if (_FocusHighlightEnabled && ((Control)this).Enabled && ((Control)this).Focused)
		{
			internalComboColors.Background = _FocusHighlightColor;
		}
		else if (!((Control)this).Enabled)
		{
			internalComboColors.Background = (_DisabledBackColor.IsEmpty ? SystemColors.Control : _DisabledBackColor);
			internalComboColors.ThumbText = (_DisabledForeColor.IsEmpty ? SystemColors.ControlDark : _DisabledForeColor);
		}
		return internalComboColors;
	}

	private void ReleasePopupHandler()
	{
		ComboBoxPopupMsgHandler popupMsgHandler = _PopupMsgHandler;
		_PopupMsgHandler = null;
		if (popupMsgHandler != null)
		{
			((NativeWindow)popupMsgHandler).ReleaseHandle();
		}
	}

	private void CreatePopupHandler(IntPtr handle)
	{
		if (_PopupMsgHandler == null)
		{
			_PopupMsgHandler = new ComboBoxPopupMsgHandler();
			((NativeWindow)_PopupMsgHandler).AssignHandle(handle);
		}
	}

	protected override void WndProc(ref Message m)
	{
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Invalid comparison between Unknown and I4
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		WinApi.RECT r = default(WinApi.RECT);
		switch (((Message)(ref m)).Msg)
		{
		case 308:
			if (BarFunctions.IsWindows7)
			{
				if (m_DropDownHandle != ((Message)(ref m)).LParam)
				{
					ReleasePopupHandler();
				}
				if (_PopupMsgHandler == null)
				{
					CreatePopupHandler(((Message)(ref m)).LParam);
				}
			}
			m_DropDownHandle = ((Message)(ref m)).LParam;
			if (m_DropDownHeight > 0 || Items.Count == 0)
			{
				WinApi.GetWindowRect(((Message)(ref m)).LParam, ref r);
				int cy = m_DropDownHeight;
				if (Items.Count == 0)
				{
					cy = Math.Max(18, ((ComboBox)this).ItemHeight);
				}
				Point point = ((Control)this).PointToScreen(Point.Empty);
				if (r.Top < point.Y)
				{
					r.Top = point.Y + ((Control)this).Height;
				}
				NativeFunctions.SetWindowPos(((Message)(ref m)).LParam, IntPtr.Zero, r.Left, r.Top, r.Width, cy, 16);
			}
			break;
		case 343:
			((ComboBox)this).WndProc(ref m);
			if (((Message)(ref m)).Result == IntPtr.Zero && DroppedDownInternal)
			{
				DroppedDownInternal = false;
			}
			return;
		case 7:
			if (((Message)(ref m)).WParam != ((Control)this).Handle)
			{
				m_LastFocusWindow = ((Message)(ref m)).WParam;
			}
			break;
		case 323:
			if (Items.Count > 0 && Items[Items.Count - 1] is ComboItem comboItem)
			{
				comboItem.m_ComboBox = this;
			}
			break;
		case 330:
		{
			int num = WinApi.ToInt(((Message)(ref m)).WParam);
			if (num >= 0 && num < Items.Count && Items[num] is ComboItem comboItem2)
			{
				comboItem2.m_ComboBox = this;
			}
			m_SelectedIndexInternal = -1;
			break;
		}
		case 324:
			m_SelectedIndexInternal = -1;
			break;
		case 1031:
			if ((int)((ComboBox)this).DropDownStyle == 1 && !m_Focused)
			{
				((ComboBox)this).SelectionLength = 0;
			}
			((Control)this).Invalidate(true);
			return;
		}
		if (m_DefaultStyle)
		{
			((ComboBox)this).WndProc(ref m);
		}
		else if ((((Message)(ref m)).Msg == 15 || ((Message)(ref m)).Msg == 792) && (int)((ComboBox)this).DrawMode != 0)
		{
			WinApi.GetWindowRect(((Message)(ref m)).HWnd, ref r);
			Rectangle rectangle = new Rectangle(0, 0, r.Width, r.Height);
			if (((Message)(ref m)).Msg == 15)
			{
				WinApi.PAINTSTRUCT ps = default(WinApi.PAINTSTRUCT);
				IntPtr intPtr = WinApi.BeginPaint(((Message)(ref m)).HWnd, ref ps);
				Graphics val = Graphics.FromHdc(intPtr);
				try
				{
					using BufferedBitmap bufferedBitmap = new BufferedBitmap(val, rectangle);
					PaintComboBox(bufferedBitmap.Graphics, rectangle);
					bufferedBitmap.Render(val);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
				WinApi.EndPaint(((Message)(ref m)).HWnd, ref ps);
			}
			else
			{
				Graphics val2 = Graphics.FromHdc(((Message)(ref m)).WParam);
				try
				{
					PaintComboBox(val2, rectangle);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			if (((Control)this).Parent is ItemControl)
			{
				((ItemControl)(object)((Control)this).Parent).UpdateKeyTipsCanvas();
			}
		}
		else
		{
			((ComboBox)this).WndProc(ref m);
		}
	}

	private void PaintComboBox(Graphics g, Rectangle r)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Invalid comparison between Unknown and I4
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		InternalComboColors comboColors = GetComboColors();
		PaintComboBackground(g, r, comboColors);
		PaintComboBorder(g, r, comboColors);
		Rectangle rectangle = PaintComboThumb(g, r, comboColors);
		int num = ((((ListControl)this).SelectedIndex == -1) ? (-1) : m_SelectedIndexInternal);
		if (m_SelectedIndexInternal == -1 && ((ListControl)this).SelectedIndex >= 0)
		{
			num = ((ListControl)this).SelectedIndex;
		}
		if ((int)((ComboBox)this).DropDownStyle != 2)
		{
			return;
		}
		if (Items.Count > 0 && (uint)num < Items.Count)
		{
			DrawItemState val = (DrawItemState)4096;
			if (!((Control)this).Enabled)
			{
				val = (DrawItemState)(val | 4);
			}
			else if (((Control)this).Focused)
			{
				val = (DrawItemState)(val | 0x11);
			}
			rectangle.Inflate(-1, -1);
			((ComboBox)this).OnDrawItem(new DrawItemEventArgs(g, ((Control)this).Font, rectangle, num, val, ((Control)this).ForeColor, ((Control)this).BackColor));
		}
		if (ShouldDrawWatermark())
		{
			DrawWatermark(g);
		}
	}

	private Rectangle PaintComboThumb(Graphics g, Rectangle r, InternalComboColors colors)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Invalid comparison between Unknown and I4
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Invalid comparison between Unknown and I4
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Invalid comparison between Unknown and I4
		Rectangle result = r;
		result.Inflate(-2, -2);
		if ((int)((ComboBox)this).DropDownStyle == 0 || !_RenderThumb)
		{
			return result;
		}
		int horizontalScrollBarThumbWidth = SystemInformation.HorizontalScrollBarThumbWidth;
		Rectangle rectangle = new Rectangle(r.Width - horizontalScrollBarThumbWidth, r.Y, horizontalScrollBarThumbWidth, r.Height);
		if ((int)((Control)this).RightToLeft == 1)
		{
			rectangle = new Rectangle(r.X + 1, r.Y + 1, horizontalScrollBarThumbWidth, r.Height - 2);
		}
		if (!IsToolbarStyle)
		{
			rectangle.Y += 2;
			rectangle.Height -= 4;
			rectangle.Width -= 2;
			if ((int)((Control)this).RightToLeft == 1)
			{
				rectangle.X += 2;
			}
		}
		else if (!BarFunctions.IsOffice2007Style(Style))
		{
			rectangle.Inflate(-1, -1);
		}
		if ((int)((Control)this).RightToLeft == 1)
		{
			int num = rectangle.Right - result.X + 2;
			result.Width -= num;
			result.X += num;
		}
		else
		{
			int num2 = result.Right - rectangle.X + 2;
			result.Width -= num2;
		}
		if (IsToolbarStyle || !BarFunctions.IsOffice2007Style(Style))
		{
			Rectangle rectangle2 = rectangle;
			rectangle2.Inflate(-1, -1);
		}
		if (MouseOver)
		{
			Rectangle bounds = rectangle;
			bounds.Inflate(-2, -2);
			DisplayHelp.FillRectangle(g, bounds, Color.Gainsboro);
		}
		DrawArrow(rectangle, g, colors.ThumbText);
		m_ThumbRect = rectangle;
		return result;
	}

	internal Rectangle GetThumbRect(Rectangle r)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Invalid comparison between Unknown and I4
		if ((int)((ComboBox)this).DropDownStyle == 0 || !_RenderThumb)
		{
			return Rectangle.Empty;
		}
		int horizontalScrollBarThumbWidth = SystemInformation.HorizontalScrollBarThumbWidth;
		Rectangle result = new Rectangle(r.Width - horizontalScrollBarThumbWidth, r.Y, horizontalScrollBarThumbWidth, r.Height);
		if ((int)((Control)this).RightToLeft == 1)
		{
			result = new Rectangle(r.X + 1, r.Y + 1, horizontalScrollBarThumbWidth, r.Height - 2);
		}
		if (!IsToolbarStyle)
		{
			result.Y += 2;
			result.Height -= 4;
			result.Width -= 2;
			if ((int)((Control)this).RightToLeft == 1)
			{
				result.X += 2;
			}
		}
		else if (!BarFunctions.IsOffice2007Style(Style))
		{
			result.Inflate(-1, -1);
		}
		return result;
	}

	protected Office2007ButtonItemStateColorTable GetOffice2007StateColorTable()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		bool flag = m_MouseOverThumb || (DroppedDownInternal && (int)((ComboBox)this).DropDownStyle > 0);
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			Office2007ColorTable colorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
			Office2007ButtonItemColorTable office2007ButtonItemColorTable = colorTable.ButtonItemColors[Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground)];
			if (!((Control)this).Enabled)
			{
				return office2007ButtonItemColorTable.Disabled;
			}
			if (DroppedDownInternal)
			{
				return office2007ButtonItemColorTable.Checked;
			}
			if (flag)
			{
				return office2007ButtonItemColorTable.MouseOver;
			}
			return office2007ButtonItemColorTable.Default;
		}
		return null;
	}

	private void PaintComboBorder(Graphics g, Rectangle controlBounds, InternalComboColors colors)
	{
		if (_RenderBorder)
		{
			DisplayHelp.DrawRectangle(g, colors.Border, controlBounds);
		}
	}

	private void PaintComboBackground(Graphics g, Rectangle controlBounds, InternalComboColors colors)
	{
		DisplayHelp.FillRectangle(g, controlBounds, colors.Background);
	}

	private bool ShouldDrawWatermark()
	{
		if (_WatermarkEnabled && ((Control)this).Enabled && (!((Control)this).Focused || _WatermarkBehavior == eWatermarkBehavior.HideNonEmpty) && ((Control)this).Text == "" && ((ListControl)this).SelectedIndex == -1)
		{
			return true;
		}
		return false;
	}

	private void DrawWatermark(Graphics g)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		if (m_TextMarkup != null)
		{
			MarkupDrawContext markupDrawContext = GetMarkupDrawContext(g);
			m_TextMarkup.Render(markupDrawContext);
			return;
		}
		eTextFormat eTextFormat = eTextFormat.VerticalCenter;
		if ((int)((Control)this).RightToLeft == 1)
		{
			eTextFormat |= eTextFormat.RightToLeft;
		}
		eTextFormat |= eTextFormat.EndEllipsis;
		eTextFormat |= eTextFormat.WordBreak;
		TextDrawing.DrawString(g, m_WatermarkText, (m_WatermarkFont == null) ? ((Control)this).Font : m_WatermarkFont, m_WatermarkColor, GetWatermarkBounds(), eTextFormat);
	}

	private Rectangle GetWatermarkBounds()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Invalid comparison between Unknown and I4
		if ((int)((ComboBox)this).DropDownStyle != 2 && m_TextWindowMsgHandler != null)
		{
			WinApi.RECT r = default(WinApi.RECT);
			WinApi.GetWindowRect(((NativeWindow)m_TextWindowMsgHandler).Handle, ref r);
			return new Rectangle(0, 0, r.Width, r.Height);
		}
		Rectangle result = new Rectangle(2, 0, ((Control)this).Width - 2, ((Control)this).Height);
		result.Inflate(-2, -1);
		int horizontalScrollBarThumbWidth = SystemInformation.HorizontalScrollBarThumbWidth;
		result.Width -= horizontalScrollBarThumbWidth;
		if ((int)((Control)this).RightToLeft == 1)
		{
			result.X += horizontalScrollBarThumbWidth;
		}
		return result;
	}

	private void DrawArrow(Rectangle r, Graphics g, Color color)
	{
		StiButtonHelper.DrawDownArrow(g, r, ((Control)this).Enabled, color);
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		((Control)this).OnVisibleChanged(e);
		m_MouseOver = false;
		if ((int)((ComboBox)this).DropDownStyle == 1 && Items.Count > 0 && Items[0] is ComboItem && ((ListControl)this).DisplayMember != "")
		{
			string displayMember = ((ListControl)this).DisplayMember;
			((ListControl)this).DisplayMember = "";
			((ListControl)this).DisplayMember = displayMember;
		}
		if (((Control)this).IsHandleCreated && !((Control)this).IsDisposed)
		{
			UpdateBackColor();
		}
	}

	protected override void OnSelectedIndexChanged(EventArgs e)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Invalid comparison between Unknown and I4
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Invalid comparison between Unknown and I4
		m_SelectedIndexInternal = ((ListControl)this).SelectedIndex;
		if (!DroppedDownInternal)
		{
			if (DroppedDownInternal)
			{
				DroppedDownInternal = false;
			}
			if (!m_MouseOver)
			{
				((Control)this).Invalidate(true);
			}
		}
		if (((ListControl)this).SelectedIndex == -1 && _WatermarkBehavior == eWatermarkBehavior.HideNonEmpty && m_WatermarkText.Length > 0 && ((Control)this).Text == "")
		{
			((Control)this).Invalidate(true);
		}
		else if ((int)((ComboBox)this).DropDownStyle == 2)
		{
			((Control)this).Invalidate();
		}
		((ComboBox)this).OnSelectedIndexChanged(e);
		if (!_InSelectedIndexChanged && (int)((ComboBox)this).DropDownStyle != 1 && !(((ComboBox)this).SelectedItem is string))
		{
			try
			{
				_InSelectedIndexChanged = true;
				if (((ComboBox)this).SelectedItem == null)
				{
					((Control)this).Text = "";
				}
				else
				{
					((Control)this).Text = ((ComboBox)this).SelectedItem.ToString();
				}
			}
			finally
			{
				_InSelectedIndexChanged = false;
			}
		}
		ExecuteCommand();
	}

	protected override void OnTextChanged(EventArgs e)
	{
		if (((ListControl)this).SelectedIndex == -1 && _WatermarkBehavior == eWatermarkBehavior.HideNonEmpty && m_WatermarkText.Length > 0 && ((Control)this).Text == "")
		{
			((Control)this).Invalidate(true);
		}
		((ComboBox)this).OnTextChanged(e);
	}

	protected override void Dispose(bool disposing)
	{
		if (m_MouseOverTimer != null)
		{
			m_MouseOverTimer.Enabled = false;
			((Component)(object)m_MouseOverTimer).Dispose();
			m_MouseOverTimer = null;
		}
		m_DisableInternalDrawing = true;
		if (m_TextWindowMsgHandler != null)
		{
			((NativeWindow)m_TextWindowMsgHandler).ReleaseHandle();
			m_TextWindowMsgHandler = null;
		}
		ReleasePopupHandler();
		((ComboBox)this).Dispose(disposing);
	}

	protected override void OnFontChanged(EventArgs e)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Invalid comparison between Unknown and I4
		((ComboBox)this).OnFontChanged(e);
		if (!((Control)this).Disposing && !((Control)this).IsDisposed && !m_DisableInternalDrawing && (int)((ComboBox)this).DrawMode == 1 && ((Control)this).IsHandleCreated && ((Control)this).Parent != null && !((Control)this).Parent.IsDisposed)
		{
			((ComboBox)this).ItemHeight = ((Control)this).FontHeight + 1;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int GetFontHeight()
	{
		return ((Control)this).FontHeight;
	}

	protected override void OnMeasureItem(MeasureItemEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Invalid comparison between Unknown and I4
		((ComboBox)this).OnMeasureItem(e);
		if (m_DisableInternalDrawing)
		{
			return;
		}
		if ((int)((ComboBox)this).DrawMode == 1)
		{
			e.ItemHeight = ((ComboBox)this).ItemHeight - 2;
			return;
		}
		object obj = Items[e.Index];
		if (!(obj is ComboItem))
		{
			return;
		}
		if (((ComboItem)obj).IsFontItem || ((ComboItem)obj).IsResourceFontItem)
		{
			MeasureFontItem(e);
			return;
		}
		Size comboItemSize = GetComboItemSize(obj as ComboItem);
		e.ItemHeight = comboItemSize.Height;
		e.ItemWidth = comboItemSize.Width;
		if (BarFunctions.IsOffice2007Style(m_Style))
		{
			e.ItemHeight += 6;
			e.ItemWidth += 6;
		}
	}

	protected override void OnDrawItem(DrawItemEventArgs e)
	{
		((ComboBox)this).OnDrawItem(e);
		InternalDrawItem(e);
	}

	private void InternalDrawItem(DrawItemEventArgs e)
	{
		if (!m_DisableInternalDrawing && e.Index >= 0)
		{
			object obj = Items[e.Index];
			if (obj is ComboItem)
			{
				DrawComboItem(e);
			}
			else
			{
				DrawObjectItem(e);
			}
		}
	}

	protected virtual Size GetComboItemSize(ComboItem item)
	{
		Size result = Size.Empty;
		if (BarFunctions.IsHandleValid((Control)(object)this))
		{
			Graphics val = ((Control)this).CreateGraphics();
			try
			{
				result = GetComboItemSize(item, val);
			}
			finally
			{
				val.Dispose();
			}
		}
		return result;
	}

	protected virtual void DrawObjectItem(DrawItemEventArgs e)
	{
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Invalid comparison between Unknown and I4
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		string itemText = ((ListControl)this).GetItemText(Items[e.Index]);
		Color empty = Color.Empty;
		Office2007ButtonItemStateColorTable office2007ButtonItemStateColorTable = null;
		if (BarFunctions.IsOffice2007Style(m_Style))
		{
			Office2007ButtonItemColorTable office2007ButtonItemColorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.ButtonItemColors[0];
			office2007ButtonItemStateColorTable = (((e.State & 1) == 0 && (e.State & 0x40) == 0) ? (((e.State & 4) == 0 && ((Control)this).Enabled) ? office2007ButtonItemColorTable.Default : office2007ButtonItemColorTable.Disabled) : office2007ButtonItemColorTable.MouseOver);
			empty = (((e.State & 4) == 0 && ((Control)this).Enabled) ? ((Control)this).ForeColor : (_DisabledForeColor.IsEmpty ? SystemColors.ControlDark : _DisabledForeColor));
			if ((e.State & 0x40) != 0 || (e.State & 1) != 0)
			{
				Rectangle bounds = e.Bounds;
				bounds.Width--;
				bounds.Height--;
				Office2007ButtonItemPainter.PaintBackground(graphics, office2007ButtonItemStateColorTable, bounds, RoundRectangleShapeDescriptor.RoundCorner2);
			}
			else
			{
				e.DrawBackground();
			}
		}
		else
		{
			e.DrawBackground();
			if ((e.State & 0x40) != 0 || (e.State & 1) != 0)
			{
				graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
				empty = SystemColors.HighlightText;
			}
			else
			{
				empty = (((e.State & 4) == 0 && (e.State & 2) == 0) ? SystemColors.ControlText : (_DisabledForeColor.IsEmpty ? SystemColors.ControlDark : _DisabledForeColor));
			}
		}
		if ((e.State & 0x10) != 0)
		{
			DrawFocusRectangle(e);
		}
		Rectangle bounds2 = e.Bounds;
		if ((e.State & 0x1000) != 4096)
		{
			if (BarFunctions.IsOffice2007Style(m_Style))
			{
				bounds2.Inflate(-3, 0);
			}
			else
			{
				bounds2.Inflate(-2, 0);
			}
		}
		TextDrawing.DrawString(graphics, itemText, ((Control)this).Font, empty, bounds2, eTextFormat.NoClipping | eTextFormat.NoPrefix);
	}

	private void DrawFocusRectangle(DrawItemEventArgs e)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		if (_FocusCuesEnabled && (e.State & 0x10) == 16 && (e.State & 0x200) != 512)
		{
			Rectangle bounds = e.Bounds;
			ControlPaint.DrawFocusRectangle(e.Graphics, bounds, e.ForeColor, e.BackColor);
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShouldSerializeDisabledForeColor()
	{
		return !_DisabledForeColor.IsEmpty;
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public void ResetDisabledForeColor()
	{
		DisabledForeColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDisabledBackColor()
	{
		return !_DisabledBackColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDisabledBackColor()
	{
		DisabledBackColor = Color.Empty;
	}

	protected virtual void DrawComboItem(DrawItemEventArgs e)
	{
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Invalid comparison between Unknown and I4
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Invalid comparison between Unknown and I4
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Invalid comparison between Unknown and I4
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Invalid comparison between Unknown and I4
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Invalid comparison between Unknown and I4
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Invalid comparison between Unknown and I4
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Invalid comparison between Unknown and I4
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Expected O, but got Unknown
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Expected O, but got Unknown
		ComboItem comboItem = Items[e.Index] as ComboItem;
		if (comboItem.IsFontItem || comboItem.IsResourceFontItem)
		{
			DrawFontItem(e);
			return;
		}
		Graphics graphics = e.Graphics;
		Image val = null;
		Color color = comboItem.ForeColor;
		if (comboItem.ImageIndex >= 0 && m_ImageList != null && m_ImageList.Images.Count > comboItem.ImageIndex)
		{
			val = m_ImageList.Images[comboItem.ImageIndex];
		}
		else if (comboItem.Image != null)
		{
			val = comboItem.Image;
		}
		Office2007ButtonItemStateColorTable office2007ButtonItemStateColorTable = null;
		if (BarFunctions.IsOffice2007Style(m_Style))
		{
			Office2007ButtonItemColorTable office2007ButtonItemColorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.ButtonItemColors[0];
			if ((e.State & 1) != 0 || (e.State & 0x40) != 0)
			{
				office2007ButtonItemStateColorTable = office2007ButtonItemColorTable.MouseOver;
				if (color.IsEmpty)
				{
					color = office2007ButtonItemStateColorTable.Text;
				}
			}
			else
			{
				office2007ButtonItemStateColorTable = (((e.State & 4) == 0 && ((Control)this).Enabled) ? office2007ButtonItemColorTable.Default : office2007ButtonItemColorTable.Disabled);
			}
			if (!((Control)this).Enabled)
			{
				color = (_DisabledForeColor.IsEmpty ? SystemColors.ControlDark : _DisabledForeColor);
			}
		}
		if (color.IsEmpty)
		{
			color = ((Control)this).ForeColor;
		}
		int dropDownWidth = ((ComboBox)this).DropDownWidth;
		if ((int)comboItem.ImagePosition != 2 && val != null)
		{
			dropDownWidth -= val.Width;
			if (dropDownWidth <= 0)
			{
				dropDownWidth = ((ComboBox)this).DropDownWidth;
			}
		}
		if ((e.State & 1) != 0 || (e.State & 0x40) != 0)
		{
			if (BarFunctions.IsOffice2007Style(m_Style))
			{
				Office2007ButtonItemPainter.PaintBackground(graphics, office2007ButtonItemStateColorTable, e.Bounds, RoundRectangleShapeDescriptor.RoundCorner2);
			}
			else
			{
				e.DrawBackground();
			}
			DrawFocusRectangle(e);
		}
		else
		{
			Color backColor = comboItem.BackColor;
			if (comboItem.BackColor.IsEmpty)
			{
				backColor = e.BackColor;
			}
			SolidBrush val2 = new SolidBrush(backColor);
			try
			{
				graphics.FillRectangle((Brush)(object)val2, e.Bounds);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		Rectangle bounds = e.Bounds;
		Rectangle bounds2 = e.Bounds;
		if ((e.State & 0x1000) == 4096)
		{
			if (val != null)
			{
				bounds2.X += 3;
				bounds2.Width -= 3;
			}
		}
		else if (BarFunctions.IsOffice2007Style(m_Style))
		{
			bounds2.Inflate(-3, 0);
		}
		else
		{
			bounds2.Inflate(-2, 0);
		}
		if (val != null)
		{
			bounds.Width = val.Width;
			bounds.Height = val.Height;
			if ((int)comboItem.ImagePosition == 0)
			{
				if (e.Bounds.Height > val.Height)
				{
					bounds.Y += (e.Bounds.Height - val.Height) / 2;
				}
				bounds2.Width -= bounds.Width;
				bounds2.X += bounds.Width;
			}
			else if ((int)comboItem.ImagePosition == 1)
			{
				if (e.Bounds.Height > val.Height)
				{
					bounds.Y += (e.Bounds.Height - val.Height) / 2;
				}
				bounds.X = e.Bounds.Right - val.Width;
				bounds2.Width -= bounds.Width;
			}
			else
			{
				bounds.X += (e.Bounds.Width - val.Width) / 2;
				bounds2.Y = bounds.Bottom;
			}
			graphics.DrawImage(val, bounds);
		}
		if (!(comboItem.Text != ""))
		{
			return;
		}
		Font val3 = e.Font;
		bool flag = false;
		try
		{
			if (comboItem.FontName != "")
			{
				val3 = new Font(comboItem.FontName, comboItem.FontSize, comboItem.FontStyle);
				flag = true;
			}
			else if (comboItem.FontStyle != val3.Style)
			{
				val3 = new Font(val3, comboItem.FontStyle);
				flag = true;
			}
		}
		catch
		{
			val3 = e.Font;
			if (val3 == null)
			{
				val3 = SystemFonts.DefaultFont;
				flag = true;
			}
		}
		eTextFormat eTextFormat = eTextFormat.NoClipping | eTextFormat.NoPrefix;
		if ((int)comboItem.TextFormat.Alignment == 1)
		{
			eTextFormat = eTextFormat.HorizontalCenter;
		}
		else if ((int)comboItem.TextFormat.Alignment == 2)
		{
			eTextFormat = eTextFormat.Right;
		}
		if ((int)comboItem.TextLineAlignment == 1)
		{
			eTextFormat |= eTextFormat.VerticalCenter;
		}
		else if ((int)comboItem.TextLineAlignment == 2)
		{
			eTextFormat |= eTextFormat.Bottom;
		}
		TextDrawing.DrawString(graphics, comboItem.Text, val3, color, bounds2, eTextFormat);
		if (flag)
		{
			val3.Dispose();
		}
	}

	protected virtual Size GetComboItemSize(ComboItem item, Graphics g)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Invalid comparison between Unknown and I4
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Invalid comparison between Unknown and I4
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Invalid comparison between Unknown and I4
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Invalid comparison between Unknown and I4
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Invalid comparison between Unknown and I4
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Expected O, but got Unknown
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Invalid comparison between Unknown and I4
		if ((int)((ComboBox)this).DrawMode == 1)
		{
			return new Size(((ComboBox)this).DropDownWidth, ((ComboBox)this).ItemHeight);
		}
		Size empty = Size.Empty;
		Size empty2 = Size.Empty;
		Image val = null;
		if (item.ImageIndex >= 0)
		{
			val = m_ImageList.Images[item.ImageIndex];
		}
		else if (item.Image != null)
		{
			val = item.Image;
		}
		int dropDownWidth = ((ComboBox)this).DropDownWidth;
		if ((int)item.ImagePosition != 2 && val != null)
		{
			dropDownWidth -= val.Width;
			if (dropDownWidth <= 0)
			{
				dropDownWidth = ((ComboBox)this).DropDownWidth;
			}
		}
		Font font = ((Control)this).Font;
		if (item.FontName != "")
		{
			try
			{
				font = new Font(item.FontName, item.FontSize, item.FontStyle);
			}
			catch
			{
				font = ((Control)this).Font;
			}
		}
		eTextFormat eTextFormat = eTextFormat.Default;
		if ((int)item.TextFormat.Alignment == 1)
		{
			eTextFormat = eTextFormat.HorizontalCenter;
		}
		else if ((int)item.TextFormat.Alignment == 2)
		{
			eTextFormat = eTextFormat.Right;
		}
		if ((int)item.TextLineAlignment == 1)
		{
			eTextFormat |= eTextFormat.VerticalCenter;
		}
		else if ((int)item.TextLineAlignment == 2)
		{
			eTextFormat |= eTextFormat.Bottom;
		}
		empty2 = TextDrawing.MeasureString(g, item.Text, font, ((ComboBox)this).DropDownWidth, eTextFormat);
		empty2.Width += 2;
		empty.Width = empty2.Width;
		empty.Height = empty2.Height;
		if (empty.Width < ((ComboBox)this).DropDownWidth)
		{
			empty.Width = ((ComboBox)this).DropDownWidth;
		}
		if ((int)item.ImagePosition == 2 && val != null)
		{
			empty.Height += val.Height;
		}
		else if (val != null && val.Height > empty.Height)
		{
			empty.Height = val.Height;
		}
		return empty;
	}

	public void LoadFonts()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		Items.Clear();
		InstalledFontCollection val = new InstalledFontCollection();
		try
		{
			IEnumerable<FontFamily> enumerable;
			if (FontFamilies == null)
			{
				IEnumerable<FontFamily> families = ((FontCollection)val).Families;
				enumerable = families;
			}
			else
			{
				IEnumerable<FontFamily> families = FontFamilies;
				enumerable = families;
			}
			IEnumerable<FontFamily> source = enumerable;
			foreach (FontFamily item in source.Where((FontFamily f) => !string.IsNullOrWhiteSpace(f.Name)))
			{
				ComboItem comboItem = new ComboItem();
				comboItem.IsFontItem = true;
				comboItem.FontName = item.GetName(0);
				comboItem.FontSize = ((Control)this).Font.Size;
				comboItem.Text = item.GetName(0);
				Items.Add((object)comboItem);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void DrawFontItem(DrawItemEventArgs e)
	{
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Invalid comparison between Unknown and I4
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Invalid comparison between Unknown and I4
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Expected O, but got Unknown
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Expected O, but got Unknown
		try
		{
			FontStyle[] array = new FontStyle[4];
			RuntimeHelpers.InitializeArray(array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			FontStyle[] array2 = (FontStyle[])(object)array;
			if (!BarFunctions.IsOffice2007Style(m_Style))
			{
				e.DrawBackground();
			}
			string fontname = Items[e.Index].ToString();
			FontFamily val = ((IEnumerable<FontFamily>)((FontCollection)FontCollection).Families).FirstOrDefault((Func<FontFamily, bool>)((FontFamily f) => f.Name == fontname));
			FontFamily val2 = val;
			if (val2 == null && FontFamily.Families.Any((FontFamily x) => x.Name.Equals(fontname)))
			{
				val2 = new FontFamily(fontname);
			}
			try
			{
				int num = ((ComboBox)this).DropDownWidth / 2 - 4;
				if (num <= 0)
				{
					num = ((Control)this).Width;
				}
				FontStyle[] array3 = array2;
				foreach (FontStyle val3 in array3)
				{
					eTextFormat format = eTextFormat.NoPrefix;
					Color color = (((e.State & 1) != 0) ? SystemColors.HighlightText : SystemColors.ControlText);
					Office2007ButtonItemStateColorTable office2007ButtonItemStateColorTable = null;
					if (BarFunctions.IsOffice2007Style(m_Style))
					{
						Office2007ButtonItemColorTable office2007ButtonItemColorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.ButtonItemColors[0];
						office2007ButtonItemStateColorTable = (((e.State & 1) == 0 && (e.State & 0x40) == 0) ? (((e.State & 4) == 0 && ((Control)this).Enabled) ? office2007ButtonItemColorTable.Default : office2007ButtonItemColorTable.Disabled) : office2007ButtonItemColorTable.MouseOver);
						color = (((Control)this).Enabled ? SystemColors.ControlText : (_DisabledForeColor.IsEmpty ? SystemColors.ControlDark : _DisabledForeColor));
						if ((e.State & 1) != 0 || (e.State & 0x40) != 0)
						{
							Office2007ButtonItemPainter.PaintBackground(e.Graphics, office2007ButtonItemStateColorTable, e.Bounds, RoundRectangleShapeDescriptor.RoundCorner2);
						}
						else
						{
							e.DrawBackground();
						}
					}
					if ((e.State & 0x1000) == 4096)
					{
						Rectangle bounds = e.Bounds;
						TextDrawing.DrawString(e.Graphics, fontname, ((Control)this).Font, color, bounds, format);
						continue;
					}
					double factor = StiScale2.Factor;
					Size size = TextDrawing.MeasureString(e.Graphics, fontname, ((Control)this).Font);
					int num2 = (e.Bounds.Height - size.Height) / 2;
					Rectangle bounds2 = new Rectangle(e.Bounds.X, e.Bounds.Y + num2, ((e.State & 4) == 4) ? e.Bounds.Width : Math.Max(e.Bounds.Width - (int)Math.Ceiling(100.0 * StiScale2.X), (int)Math.Ceiling(32.0 * StiScale2.X)), e.Bounds.Height - num2);
					TextDrawing.DrawString(e.Graphics, fontname, ((Control)this).Font, color, bounds2, format);
					Rectangle bounds3 = new Rectangle(e.Bounds.X + num + (int)Math.Ceiling(4.0 * StiScale2.X), e.Bounds.Y, e.Bounds.Width + (int)Math.Ceiling(100.0 * StiScale2.X), e.Bounds.Height);
					if (!AllowCustomFontDrawing || val2 == null || !val2.IsStyleAvailable(val3))
					{
						break;
					}
					float num3 = 1f;
					float num4 = 1f;
					while (Math.Round(num4) < (double)(bounds3.Height - 1))
					{
						num3 += 1f;
						Font val4 = new Font(val2, num3, val3);
						try
						{
							num4 = e.Graphics.MeasureString(fontname, val4).Height;
						}
						finally
						{
							((IDisposable)val4)?.Dispose();
						}
					}
					num3 -= 1f;
					Font val5 = new Font(val2, num3, val3);
					try
					{
						num4 = e.Graphics.MeasureString(fontname, val5).Height;
						bounds3.Height = (int)Math.Round(num4);
						TextDrawing.DrawString(e.Graphics, fontname, val5, color, bounds3, format);
						break;
					}
					finally
					{
						((IDisposable)val5)?.Dispose();
					}
				}
			}
			finally
			{
				if (val2 != null && val == null)
				{
					val2.Dispose();
				}
			}
		}
		catch
		{
		}
	}

	private void MeasureFontItem(MeasureItemEventArgs e)
	{
		e.ItemHeight = (int)Math.Ceiling(18.0 * StiScale2.Y);
	}

	public void ReleaseFocus()
	{
		if (!((Control)this).Focused || !(m_LastFocusWindow != IntPtr.Zero))
		{
			return;
		}
		Control val = Control.FromChildHandle(m_LastFocusWindow);
		if (val != this)
		{
			if (val != null)
			{
				val.Focus();
			}
			else
			{
				NativeFunctions.SetFocus(m_LastFocusWindow);
			}
			((Control)this).OnLostFocus(new EventArgs());
		}
		m_LastFocusWindow = IntPtr.Zero;
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		if ((int)keyData == 13 && m_PreventEnterBeep)
		{
			((Control)this).OnKeyPress(new KeyPressEventArgs('\r'));
			return true;
		}
		return ((Control)this).ProcessCmdKey(ref msg, keyData);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		if (!IsStandalone)
		{
			if ((int)e.KeyCode == 13)
			{
				ReleaseFocus();
			}
			else if ((int)e.KeyCode == 27)
			{
				ReleaseFocus();
			}
		}
		((ComboBox)this).OnKeyDown(e);
	}

	protected override void OnEnabledChanged(EventArgs e)
	{
		UpdateBackColor();
		((Control)this).OnEnabledChanged(e);
	}

	private void UpdateBackColor()
	{
		if (!m_UseCustomBackColor)
		{
			Color background = GetComboColors().Background;
			if (((Control)this).BackColor != background)
			{
				((Control)this).BackColor = background;
			}
		}
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		if (!m_MouseOver)
		{
			m_MouseOver = true;
			UpdateBackColor();
			((Control)this).Invalidate(true);
		}
		SetMouseOverTimerEnabled(value: true);
		((ComboBox)this).OnMouseEnter(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		SetMouseOverTimerEnabled(value: true);
		((ComboBox)this).OnMouseLeave(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		if (!m_MouseOver)
		{
			m_MouseOver = true;
			((Control)this).Invalidate();
			SetMouseOverTimerEnabled(value: true);
		}
	}

	private void SetMouseOverTimerEnabled(bool value)
	{
		if (m_MouseOverTimer != null)
		{
			m_MouseOverTimer.Enabled = value;
		}
	}

	private void SetupTextBoxMessageHandler()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		if ((int)((ComboBox)this).DropDownStyle != 2 && (int)((ComboBox)this).AutoCompleteMode == 0)
		{
			if (m_TextWindowMsgHandler == null)
			{
				IntPtr window = GetWindow(((Control)this).Handle, 5u);
				if (window != IntPtr.Zero)
				{
					m_TextWindowMsgHandler = new ComboTextBoxMsgHandler();
					m_TextWindowMsgHandler.MouseLeave += TextBoxMouseLeave;
					m_TextWindowMsgHandler.Paint += new PaintEventHandler(TextBoxPaint);
					((NativeWindow)m_TextWindowMsgHandler).AssignHandle(window);
				}
			}
		}
		else if (m_TextWindowMsgHandler != null)
		{
			((NativeWindow)m_TextWindowMsgHandler).ReleaseHandle();
			m_TextWindowMsgHandler = null;
		}
	}

	protected override void OnDropDownStyleChanged(EventArgs e)
	{
		SetupTextBoxMessageHandler();
		((ComboBox)this).OnDropDownStyleChanged(e);
	}

	private void TextBoxPaint(object sender, PaintEventArgs e)
	{
		if (ShouldDrawWatermark())
		{
			DrawWatermark(e.Graphics);
		}
	}

	private void TextBoxMouseLeave(object sender, EventArgs e)
	{
		if (m_MouseOver)
		{
			SetMouseOverTimerEnabled(value: true);
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFocusHighlightColor()
	{
		return !_FocusHighlightColor.Equals((object?)_DefaultHighlightColor);
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetFocusHighlightColor()
	{
		FocusHighlightColor = _DefaultHighlightColor;
	}

	protected virtual void ExecuteCommand()
	{
		if (_Command != null)
		{
			CommandManager.ExecuteCommand(this);
		}
	}

	protected virtual void OnCommandChanged()
	{
	}
}
