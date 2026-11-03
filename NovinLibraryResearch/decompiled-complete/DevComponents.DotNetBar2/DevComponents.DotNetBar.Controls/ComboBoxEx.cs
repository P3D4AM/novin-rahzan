using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.AdvTree;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.TextMarkup;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Controls;

[ToolboxItem(true)]
[Designer("DevComponents.DotNetBar.Design.ComboBoxExDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxBitmap(typeof(ComboBoxEx), "Controls.ComboBoxEx.ico")]
public class ComboBoxEx : ComboBox, ICommandSource
{
	public delegate void OnDropDownChangeEventHandler(object sender, bool Expanded);

	private class ḿ
	{
		public Color ཥ = SystemColors.Window;

		public Color ᙴ = SystemColors.Window;

		public LinearGradientColorTable Ṁ;

		public Color ṁ = SystemColors.ControlText;

		public LinearGradientColorTable Ṃ;

		public LinearGradientColorTable ṃ;
	}

	private class _26A9 : NativeWindow
	{
		private struct _17BB
		{
			public int _17BC;

			public int _17BD;

			public IntPtr _17BF;

			public int _17BE;
		}

		private const int _08E7 = 675;

		private const int _08E6 = 512;

		private const int _08D3 = 2;

		private PaintEventHandler m__139A;

		private bool _26AA;

		public event EventHandler ભ;

		public event PaintEventHandler _139A
		{
			add
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				PaintEventHandler val = this.m__139A;
				PaintEventHandler val2;
				do
				{
					val2 = val;
					PaintEventHandler value2 = (PaintEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
					val = Interlocked.CompareExchange(ref this.m__139A, value2, val2);
				}
				while (val != val2);
			}
			remove
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				PaintEventHandler val = this.m__139A;
				PaintEventHandler val2;
				do
				{
					val2 = val;
					PaintEventHandler value2 = (PaintEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
					val = Interlocked.CompareExchange(ref this.m__139A, value2, val2);
				}
				while (val != val2);
			}
		}

		[DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool TrackMouseEvent(ref _17BB lpEventTrack);

		protected override void WndProc(ref Message m)
		{
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Expected O, but got Unknown
			if (((Message)(ref m)).Msg == 512 && !_26AA)
			{
				_26AA = true;
				_17BB lpEventTrack = default(_17BB);
				lpEventTrack._17BD = 2;
				lpEventTrack._17BC = Marshal.SizeOf((object)lpEventTrack);
				lpEventTrack._17BF = ((NativeWindow)this).Handle;
				lpEventTrack._17BE = 0;
				_26AA = TrackMouseEvent(ref lpEventTrack);
			}
			else if (((Message)(ref m)).Msg == 675)
			{
				if (this.ભ != null)
				{
					this.ભ(this, new EventArgs());
				}
				_26AA = false;
			}
			else if (((Message)(ref m)).Msg == 15)
			{
				((NativeWindow)this).WndProc(ref m);
				if (this._139A != null)
				{
					Graphics val = Graphics.FromHwnd(((Message)(ref m)).HWnd);
					try
					{
						this._139A.Invoke((object)this, new PaintEventArgs(val, Rectangle.Empty));
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

	private class _26AB : NativeWindow
	{
		protected override void WndProc(ref Message m)
		{
			if (((Message)(ref m)).Msg == 277)
			{
				((NativeWindow)this).WndProc(ref m);
				_0EFE.RedrawWindow(((Message)(ref m)).HWnd, IntPtr.Zero, IntPtr.Zero, (_0EFE.RedrawWindowFlags)257u);
			}
			else
			{
				((NativeWindow)this).WndProc(ref m);
			}
		}
	}

	[Flags]
	private enum _26AC
	{
		ᘶ = 0,
		_05C1 = 1,
		_26AD = 2,
		_26AE = 4,
		_26AF = 7
	}

	private const uint ᚫ = 5u;

	private const uint ᚬ = 7u;

	private const uint ᚭ = 8u;

	private OnDropDownChangeEventHandler ᚮ;

	private eDotNetBarStyle Ӷ = eDotNetBarStyle.Office2007;

	private bool ᚯ;

	private bool ݡ;

	private bool ᚰ;

	private bool _ᚱ;

	private bool ᚲ;

	private bool ᚳ;

	private ImageList գ;

	private int ᚴ;

	private IntPtr _0A03;

	private IntPtr ᚵ = IntPtr.Zero;

	private _26A9 ᚶ;

	private bool ᚕ;

	private string ᚷ = "";

	private bool હ;

	private Font ᚸ;

	private Color ᚹ = SystemColors.GrayText;

	private bool ᚺ = true;

	private Timer ᄧ;

	private Rectangle ᚻ = Rectangle.Empty;

	private bool _ᚼ;

	private static Color _ᚽ = Color.FromArgb(255, 255, 136);

	private Color _ᚾ = _ᚽ;

	private bool _ᒖ = true;

	private bool _ᚿ = true;

	private eWatermarkBehavior _ᛀ;

	private _0CB4 ݶ;

	private bool ᛁ;

	internal BaseItem ᛂ;

	private Timer ᛃ;

	private _26AB _ᛄ;

	private bool _ᛅ = true;

	private bool _ᛆ = true;

	private int ᛇ = -1;

	private bool _ᛈ;

	private bool _ᛉ = true;

	private Office2007ButtonItemColorTable _ᛊ;

	private Color _ᛋ = Color.Empty;

	private Color _ᖣ = Color.Empty;

	private bool _ᛌ = true;

	private string _ᛍ = "";

	private DevComponents.AdvTree.AdvTree _ᛎ;

	private string _ᛏ = "";

	private string[] _ᛐ = new string[0];

	private CancelEventHandler ᛑ;

	private _26AC _ᛒ;

	private DateTime _ᛓ = DateTime.MinValue;

	private DateTime _ᛔ = DateTime.MinValue;

	private _26B0 _ᛕ;

	private bool _ᛖ;

	private DataColumnEventHandler _05CC;

	private AdvTreeNodeCancelEventHandler ᛗ;

	private bool _ᛘ = true;

	private bool _ᛙ = true;

	private bool _ᛚ = true;

	private bool _ᛛ = true;

	private Font _ᛜ;

	private ICommand _૦;

	private object _૧;

	[Description("Indicates whether control displays focus cues when focused.")]
	[Category("Behavior")]
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

	[Description("Indicates the appearance of the control.")]
	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(true)]
	public bool IsStandalone
	{
		get
		{
			return ᚺ;
		}
		set
		{
			ᚺ = value;
		}
	}

	[Description("Indicates whether watermark text is displayed when control is empty.")]
	[DefaultValue(true)]
	public virtual bool WatermarkEnabled
	{
		get
		{
			return _ᚿ;
		}
		set
		{
			_ᚿ = value;
			((Control)this).Invalidate();
		}
	}

	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Localizable(true)]
	[Category("Appearance")]
	[DefaultValue("")]
	[Browsable(true)]
	[Description("Indicates watermark text displayed inside of the control when Text is not set and control does not have input focus.")]
	public string WatermarkText
	{
		get
		{
			return ᚷ;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			ᚷ = value;
			ᛝ();
			((Control)this).Invalidate();
		}
	}

	[Description("Indicates watermark hiding behaviour.")]
	[Category("Behavior")]
	[DefaultValue(eWatermarkBehavior.HideOnFocus)]
	public eWatermarkBehavior WatermarkBehavior
	{
		get
		{
			return _ᛀ;
		}
		set
		{
			_ᛀ = value;
			((Control)this).Invalidate();
		}
	}

	[Description("Indicates watermark font.")]
	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Appearance")]
	public Font WatermarkFont
	{
		get
		{
			return ᚸ;
		}
		set
		{
			ᚸ = value;
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
			return ᚹ;
		}
		set
		{
			ᚹ = value;
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

	[DefaultValue(false)]
	[Browsable(false)]
	public bool UseCustomBackColor
	{
		get
		{
			return ᛁ;
		}
		set
		{
			ᛁ = value;
		}
	}

	[Description("Makes Combo box appear the same as built-in Combo box.")]
	[Category("Behavior")]
	[Browsable(false)]
	[DefaultValue(false)]
	public bool DefaultStyle
	{
		get
		{
			return ᚯ;
		}
		set
		{
			if (ᚯ != value)
			{
				ᚯ = value;
				((Control)this).Invalidate(true);
			}
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	[Browsable(false)]
	[Description("When running on WindowsXP draws control using the Windows XP Themes if theme manager is enabled.")]
	public bool ThemeAware
	{
		get
		{
			return ᚲ;
		}
		set
		{
			if (ᚲ != value)
			{
				ᚲ = value;
				if (!ᚲ)
				{
					ᚯ = false;
				}
				else if (ᚲ && BarFunctions.ThemedOS)
				{
					ᚯ = true;
					((ComboBox)this).FlatStyle = (FlatStyle)2;
				}
			}
		}
	}

	[Browsable(true)]
	[Description("Disables internal drawing support for the List-box portion of Combo-box.")]
	[DefaultValue(false)]
	[Category("Behavior")]
	public bool DisableInternalDrawing
	{
		get
		{
			return ᚳ;
		}
		set
		{
			if (ᚳ != value)
			{
				ᚳ = value;
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Indicates whether combo box generates the audible alert when Enter key is pressed.")]
	[Category("Behavior")]
	[DevCoBrowsable(true)]
	public bool PreventEnterBeep
	{
		get
		{
			return ᚕ;
		}
		set
		{
			ᚕ = value;
		}
	}

	[Description("The ImageList control used by Combo box to draw images.")]
	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Behavior")]
	public ImageList Images
	{
		get
		{
			return գ;
		}
		set
		{
			գ = value;
		}
	}

	[Description("Determines the display of the item when shown.")]
	[DefaultValue(eDotNetBarStyle.Office2007)]
	[Browsable(true)]
	[Category("Appearance")]
	public eDotNetBarStyle Style
	{
		get
		{
			return Ӷ;
		}
		set
		{
			Ӷ = value;
			ᛸ();
			((Control)this).Invalidate(true);
		}
	}

	private bool ᚦ
	{
		get
		{
			return ᚰ;
		}
		set
		{
			if (ᚰ != value)
			{
				ᚰ = value;
				((Control)this).Invalidate();
			}
		}
	}

	private bool ᚧ
	{
		get
		{
			return _ᚱ;
		}
		set
		{
			if (ᚪ)
			{
				_ᚱ = value;
			}
			else if (_ᚱ != value)
			{
				ᛢ();
				_ᚱ = value;
				((Control)this).Invalidate();
				if (ᚮ != null)
				{
					ᚮ(this, _ᚱ);
				}
				if (!_ᚱ)
				{
					ᛦ();
				}
			}
		}
	}

	[Browsable(false)]
	public bool IsToolbarStyle => !ᚺ;

	internal bool ᚨ
	{
		get
		{
			return _ᛅ;
		}
		set
		{
			_ᛅ = value;
		}
	}

	internal bool ᚩ
	{
		get
		{
			return _ᛆ;
		}
		set
		{
			_ᛆ = value;
		}
	}

	public override string Text
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)((ComboBox)this).DropDownStyle == 2 && ((ComboBox)this).SelectedItem != null && !(((ComboBox)this).SelectedItem is string))
			{
				return ((ListControl)this).GetItemText(((ComboBox)this).SelectedItem);
			}
			return ((ComboBox)this).Text;
		}
		set
		{
			((ComboBox)this).Text = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Indicates whether ItemHeight property is set automatically based on the current font when DrawMode=OwnerDrawFixed")]
	public bool AutoItemHeight
	{
		get
		{
			return _ᛉ;
		}
		set
		{
			_ᛉ = value;
			if (((Control)this).IsHandleCreated && value)
			{
				ᛳ();
			}
		}
	}

	[DefaultValue(null)]
	[Browsable(false)]
	public Office2007ButtonItemColorTable ComboItemColors
	{
		get
		{
			return _ᛊ;
		}
		set
		{
			_ᛊ = value;
		}
	}

	[Description("Indicates text color for the text in combo-box when control Enabled property is set to false. Setting this property is effective only for DropDownList ComboBox style.")]
	[Category("Appearance")]
	public Color DisabledForeColor
	{
		get
		{
			return _ᛋ;
		}
		set
		{
			_ᛋ = value;
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
			return _ᖣ;
		}
		set
		{
			if (_ᖣ != value)
			{
				_ᖣ = value;
				if (!((Control)this).Enabled)
				{
					((Control)this).Invalidate();
				}
			}
		}
	}

	[DefaultValue(true)]
	[Browsable(false)]
	public bool AutoReleaseFocus
	{
		get
		{
			return _ᛌ;
		}
		set
		{
			_ᛌ = value;
		}
	}

	[Browsable(false)]
	public IntPtr DropDownHandle => ᚵ;

	internal bool ज़
	{
		get
		{
			return ݡ;
		}
		set
		{
			if (ݡ != value)
			{
				ݡ = value;
				((Control)this).Invalidate(true);
			}
		}
	}

	[Editor("DevComponents.DotNetBar.Design.ComboItemsEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Localizable(true)]
	public ObjectCollection Items => ((ComboBox)this).Items;

	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether FocusHighlightColor is used as background color to highlight text box when it has input focus.")]
	[Browsable(true)]
	public virtual bool FocusHighlightEnabled
	{
		get
		{
			return _ᚼ;
		}
		set
		{
			if (_ᚼ != value)
			{
				_ᚼ = value;
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
			return _ᚾ;
		}
		set
		{
			if (_ᚾ != value)
			{
				_ᚾ = value;
				if (((Control)this).Focused)
				{
					((Control)this).Invalidate();
				}
			}
		}
	}

	[Category("Multi-Column")]
	[Editor("DevComponents.DotNetBar.Design.DataMembersSelector, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue("")]
	[Description("Indicates comma separated list of field names that are displayed on drop down. When set it activates ComboBoxEx multi-column drop-down mode.")]
	public string DropDownColumns
	{
		get
		{
			return _ᛍ;
		}
		set
		{
			if (value != _ᛍ)
			{
				string ᛍ = _ᛍ;
				_ᛍ = value;
				OnDropDownColumnsChanged(ᛍ, value);
			}
		}
	}

	[DefaultValue("")]
	[TypeConverter("DevComponents.DotNetBar.Design.ColumnNamesConverter, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	[Category("Multi-Column")]
	[Editor("DevComponents.DotNetBar.Design.ColumnNamesEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Description("Indicates custom column names with each column separated by new line, i.e. \r\n escape sequence.")]
	public string DropDownColumnsHeaders
	{
		get
		{
			return _ᛏ;
		}
		set
		{
			if (value != _ᛏ)
			{
				string ᛏ = _ᛏ;
				_ᛏ = value;
				OnDropDownColumnsHeadersChanged(ᛏ, value);
			}
		}
	}

	private bool ᚪ => !string.IsNullOrEmpty(_ᛍ);

	protected bool DisconnectedPopupDataSource
	{
		get
		{
			return _ᛖ;
		}
		set
		{
			_ᛖ = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether selection change on multi-column drop-down closes the popup. Default value is true. Applies to multi-column drop-down only.")]
	[Category("Multi-Column")]
	public bool SelectionClosesPopup
	{
		get
		{
			return _ᛘ;
		}
		set
		{
			_ᛘ = value;
		}
	}

	[Category("Multi-Column")]
	[DefaultValue(true)]
	[Description("Indicates whether multi-column popup size is preserved between popup displays if popup is resized by end-user. Applies to multi-column drop-down only.")]
	public bool PreservePopupSize
	{
		get
		{
			return _ᛙ;
		}
		set
		{
			_ᛙ = value;
		}
	}

	[DefaultValue(true)]
	[Category("Multi-Column")]
	[Description("Indicates whether multi-column popup can be resized by end user. Applies to multi-column drop-down only.")]
	public bool EnablePopupResize
	{
		get
		{
			return _ᛚ;
		}
		set
		{
			_ᛚ = value;
		}
	}

	[DefaultValue(true)]
	[Category("Multi-Column")]
	[Description("Indicates whether multi-column popup close button is visible.")]
	public bool PopupCloseButtonVisible
	{
		get
		{
			return _ᛛ;
		}
		set
		{
			_ᛛ = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool IsPopupOpen
	{
		get
		{
			return _ᚱ;
		}
		set
		{
			if (value != _ᚱ && ᚪ)
			{
				if (value)
				{
					_16FD();
				}
				else
				{
					_16FF();
				}
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public DevComponents.AdvTree.AdvTree MultiColumnDisplayControl => _ᛎ;

	[Category("Appearance")]
	[DefaultValue(null)]
	[Description("Indicates the font used by the multi-column control on the popup.")]
	public Font MultiColumnControlFont
	{
		get
		{
			return _ᛜ;
		}
		set
		{
			if (value != _ᛜ)
			{
				Font ᛜ = _ᛜ;
				_ᛜ = value;
				OnMultiColumnControlFontChanged(ᛜ, value);
			}
		}
	}

	[Category("Commands")]
	[DefaultValue(null)]
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

	ICommand ICommandSource.ᒍ
	{
		get
		{
			return _૦;
		}
		set
		{
			bool flag = false;
			if (_૦ != value)
			{
				flag = true;
			}
			if (_૦ != null)
			{
				CommandManager.UnRegisterCommandSource(this, _૦);
			}
			_૦ = value;
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

	[Category("Commands")]
	[Localizable(true)]
	[TypeConverter(typeof(StringConverter))]
	[Description("Indicates user defined data value that can be passed to the command when it is executed.")]
	[Browsable(true)]
	[DefaultValue(null)]
	public object CommandParameter
	{
		get
		{
			return _૧;
		}
		set
		{
			_૧ = value;
		}
	}

	public event OnDropDownChangeEventHandler DropDownChange
	{
		add
		{
			OnDropDownChangeEventHandler onDropDownChangeEventHandler = ᚮ;
			OnDropDownChangeEventHandler onDropDownChangeEventHandler2;
			do
			{
				onDropDownChangeEventHandler2 = onDropDownChangeEventHandler;
				OnDropDownChangeEventHandler value2 = (OnDropDownChangeEventHandler)Delegate.Combine(onDropDownChangeEventHandler2, value);
				onDropDownChangeEventHandler = Interlocked.CompareExchange(ref ᚮ, value2, onDropDownChangeEventHandler2);
			}
			while ((object)onDropDownChangeEventHandler != onDropDownChangeEventHandler2);
		}
		remove
		{
			OnDropDownChangeEventHandler onDropDownChangeEventHandler = ᚮ;
			OnDropDownChangeEventHandler onDropDownChangeEventHandler2;
			do
			{
				onDropDownChangeEventHandler2 = onDropDownChangeEventHandler;
				OnDropDownChangeEventHandler value2 = (OnDropDownChangeEventHandler)Delegate.Remove(onDropDownChangeEventHandler2, value);
				onDropDownChangeEventHandler = Interlocked.CompareExchange(ref ᚮ, value2, onDropDownChangeEventHandler2);
			}
			while ((object)onDropDownChangeEventHandler != onDropDownChangeEventHandler2);
		}
	}

	[Description("Occurs before the multi-column popup is opened and it allows canceling of popup by setting CancelEventArgs.Cancel=true.")]
	public event CancelEventHandler BeforeMultiColumnPopupOpen
	{
		add
		{
			CancelEventHandler cancelEventHandler = ᛑ;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref ᛑ, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = ᛑ;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref ᛑ, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	[Description("Occurs when ColumnHeader is automatically created by control as result of data binding for multi-column drop down and provides you with opportunity to modify it.")]
	public event DataColumnEventHandler DataColumnCreated
	{
		add
		{
			DataColumnEventHandler dataColumnEventHandler = _05CC;
			DataColumnEventHandler dataColumnEventHandler2;
			do
			{
				dataColumnEventHandler2 = dataColumnEventHandler;
				DataColumnEventHandler value2 = (DataColumnEventHandler)Delegate.Combine(dataColumnEventHandler2, value);
				dataColumnEventHandler = Interlocked.CompareExchange(ref _05CC, value2, dataColumnEventHandler2);
			}
			while ((object)dataColumnEventHandler != dataColumnEventHandler2);
		}
		remove
		{
			DataColumnEventHandler dataColumnEventHandler = _05CC;
			DataColumnEventHandler dataColumnEventHandler2;
			do
			{
				dataColumnEventHandler2 = dataColumnEventHandler;
				DataColumnEventHandler value2 = (DataColumnEventHandler)Delegate.Remove(dataColumnEventHandler2, value);
				dataColumnEventHandler = Interlocked.CompareExchange(ref _05CC, value2, dataColumnEventHandler2);
			}
			while ((object)dataColumnEventHandler != dataColumnEventHandler2);
		}
	}

	[Description("Occurs before selection on multi-column drop-down has changed and allows canceling of the change.")]
	public event AdvTreeNodeCancelEventHandler SelectionChanging
	{
		add
		{
			AdvTreeNodeCancelEventHandler advTreeNodeCancelEventHandler = ᛗ;
			AdvTreeNodeCancelEventHandler advTreeNodeCancelEventHandler2;
			do
			{
				advTreeNodeCancelEventHandler2 = advTreeNodeCancelEventHandler;
				AdvTreeNodeCancelEventHandler value2 = (AdvTreeNodeCancelEventHandler)Delegate.Combine(advTreeNodeCancelEventHandler2, value);
				advTreeNodeCancelEventHandler = Interlocked.CompareExchange(ref ᛗ, value2, advTreeNodeCancelEventHandler2);
			}
			while ((object)advTreeNodeCancelEventHandler != advTreeNodeCancelEventHandler2);
		}
		remove
		{
			AdvTreeNodeCancelEventHandler advTreeNodeCancelEventHandler = ᛗ;
			AdvTreeNodeCancelEventHandler advTreeNodeCancelEventHandler2;
			do
			{
				advTreeNodeCancelEventHandler2 = advTreeNodeCancelEventHandler;
				AdvTreeNodeCancelEventHandler value2 = (AdvTreeNodeCancelEventHandler)Delegate.Remove(advTreeNodeCancelEventHandler2, value);
				advTreeNodeCancelEventHandler = Interlocked.CompareExchange(ref ᛗ, value2, advTreeNodeCancelEventHandler2);
			}
			while ((object)advTreeNodeCancelEventHandler != advTreeNodeCancelEventHandler2);
		}
	}

	[DllImport("user32")]
	private static extern bool ValidateRect(IntPtr hWnd, ref _089F.RECT pRect);

	[DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

	public ComboBoxEx()
	{
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Expected O, but got Unknown
		if (!ColorFunctions._0893)
		{
			_089F.ळ();
			_089F.ऴ();
			ColorFunctions.LoadColors();
		}
		ᄧ = new Timer();
		ᄧ.Interval = 10;
		ᄧ.Enabled = false;
		ᄧ.Tick += ᛠ;
		((ComboBox)this).FlatStyle = (FlatStyle)0;
		StyleManager.Register((Control)(object)this);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		if (StyleManager.IsMetro(newStyle) || (StyleManager.IsMetro(StyleManager.PreviousStyle) && !StyleManager.IsMetro(newStyle)))
		{
			StyleManager.UpdateAmbientColors((Control)(object)this);
		}
	}

	private void ᛝ()
	{
		ݶ = null;
		if (ሒ.መ(ref ᚷ))
		{
			ݶ = ሒ.ሕ(ᚷ);
			ᛞ();
		}
	}

	private void ᛞ()
	{
		if (ݶ != null)
		{
			Graphics val = ((Control)this).CreateGraphics();
			try
			{
				MarkupDrawContext markupDrawContext = ᛟ(val);
				ݶ.Measure(ᛱ().Size, markupDrawContext);
				Size size = ݶ.Bounds.Size;
				ݶ._0CC6(new Rectangle(ᛱ().Location, size), markupDrawContext);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	private MarkupDrawContext ᛟ(Graphics _٥)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		return new MarkupDrawContext(_٥, (ᚸ == null) ? ((Control)this).Font : ᚸ, ᚹ, (int)((Control)this).RightToLeft == 1);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeWatermarkColor()
	{
		return ᚹ != SystemColors.GrayText;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetWatermarkColor()
	{
		WatermarkColor = SystemColors.GrayText;
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		((ComboBox)this).OnHandleCreated(e);
		ᛸ();
		_16FA();
		ᛤ(((Control)this).Handle);
		if (_ᛉ)
		{
			ᛳ();
		}
	}

	protected override void OnResize(EventArgs e)
	{
		ᛞ();
		((Control)this).Invalidate();
		((ComboBox)this).OnResize(e);
	}

	private void ᛠ(object _٠, EventArgs ؿ)
	{
		bool flag = false;
		if (((Control)this).IsHandleCreated && ((Control)this).Visible)
		{
			_0EFE.RECT r = default(_0EFE.RECT);
			_0EFE.GetWindowRect(((Control)this).Handle, ref r);
			if (r.ToRectangle().Contains(Control.MousePosition))
			{
				flag = true;
			}
			Point pt = ((Control)this).PointToClient(Control.MousePosition);
			if (ᚻ.Contains(pt))
			{
				ᚦ = true;
			}
			else
			{
				ᚦ = false;
			}
		}
		if (!flag)
		{
			_16F9(ԝ: false);
			ݡ = false;
			ᛸ();
			((Control)this).Invalidate();
			if (ᛂ != null)
			{
				ᛂ.HideToolTip();
			}
		}
	}

	protected override void OnGotFocus(EventArgs e)
	{
		((ComboBox)this).OnGotFocus(e);
		if (!ݡ)
		{
			ݡ = true;
		}
		ᛸ();
		((Control)this).Invalidate(true);
	}

	protected override void OnLostFocus(EventArgs e)
	{
		_16F9(ԝ: true);
		_0A03 = IntPtr.Zero;
		((ComboBox)this).OnLostFocus(e);
	}

	private void ᛡ()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		if (ᛃ == null)
		{
			ᛃ = new Timer();
			ᛃ.Tick += ᛣ;
			ᛃ.Interval = 200;
		}
		ᛃ.Start();
	}

	private void ᛢ()
	{
		Timer val = ᛃ;
		ᛃ = null;
		if (val != null)
		{
			val.Stop();
			((Component)(object)val).Dispose();
		}
	}

	private void ᛣ(object _٠, EventArgs ؿ)
	{
		ᚧ = _0EFE.SendMessage(((Control)this).Handle, 343, IntPtr.Zero, IntPtr.Zero) != 0;
	}

	protected override void OnDropDownClosed(EventArgs e)
	{
		if (!ᚪ)
		{
			ᚧ = false;
			ᛇ = ((ListControl)this).SelectedIndex;
		}
		((ComboBox)this).OnDropDownClosed(e);
	}

	protected override void OnDropDown(EventArgs e)
	{
		((ComboBox)this).OnDropDown(e);
		if (!ᚪ)
		{
			ᚧ = true;
		}
	}

	[DllImport("user32", CharSet = CharSet.Unicode)]
	private static extern int SetWindowTheme(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string pszSubAppName, [MarshalAs(UnmanagedType.LPWStr)] string pszSubIdList);

	private void ᛤ(IntPtr ह)
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
		if (flag)
		{
			SetWindowTheme(ह, " ", " ");
		}
	}

	private ḿ ᛥ()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		ḿ ḿ = new ḿ();
		bool flag = ᚰ || ((Control)this).Focused || (ᚧ && (int)((ComboBox)this).DropDownStyle != 0);
		if (flag && !((Control)this).Enabled)
		{
			flag = false;
		}
		if (BarFunctions.IsOffice2007Style(Style) && GlobalManager.Renderer is Office2007Renderer)
		{
			Office2007ComboBoxColorTable comboBox = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.ComboBox;
			Office2007ComboBoxStateColorTable office2007ComboBoxStateColorTable = ((IsToolbarStyle && !ݡ) ? comboBox.Default : comboBox.DefaultStandalone);
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
			ḿ.ཥ = office2007ComboBoxStateColorTable.Background;
			ḿ.ᙴ = office2007ComboBoxStateColorTable.Border;
			ḿ.Ṁ = office2007ComboBoxStateColorTable.ExpandBackground;
			ḿ.ṁ = office2007ComboBoxStateColorTable.ExpandText;
			ḿ.Ṃ = office2007ComboBoxStateColorTable.ExpandBorderOuter;
			ḿ.ṃ = office2007ComboBoxStateColorTable.ExpandBorderInner;
		}
		else
		{
			ColorScheme colorScheme = new ColorScheme(Style);
			if (flag)
			{
				if (ᚧ)
				{
					ḿ.Ṁ = new LinearGradientColorTable(colorScheme.ItemPressedBackground, colorScheme.ItemPressedBackground2, colorScheme.ItemPressedBackgroundGradientAngle);
					ḿ.ᙴ = colorScheme.ItemPressedBorder;
					ḿ.ṁ = colorScheme.ItemPressedText;
				}
				else
				{
					if (ᚰ)
					{
						ḿ.Ṁ = new LinearGradientColorTable(colorScheme.ItemHotBackground, colorScheme.ItemHotBackground2, colorScheme.ItemHotBackgroundGradientAngle);
					}
					else
					{
						ḿ.Ṁ = new LinearGradientColorTable(colorScheme.BarBackground, colorScheme.BarBackground2, colorScheme.BarBackgroundGradientAngle);
					}
					ḿ.ᙴ = colorScheme.ItemHotBorder;
					ḿ.ṁ = colorScheme.ItemHotText;
				}
			}
			else
			{
				ḿ.Ṁ = new LinearGradientColorTable(colorScheme.BarBackground, colorScheme.BarBackground2, colorScheme.BarBackgroundGradientAngle);
				if (ݡ || !IsToolbarStyle)
				{
					ḿ.ᙴ = colorScheme.ItemHotBorder;
				}
			}
		}
		if (_ᚼ && ((Control)this).Enabled && ((Control)this).Focused)
		{
			ḿ.ཥ = _ᚾ;
		}
		else if (!((Control)this).Enabled)
		{
			ḿ.ཥ = (_ᖣ.IsEmpty ? SystemColors.Control : _ᖣ);
			ḿ.ṁ = (_ᛋ.IsEmpty ? SystemColors.ControlDark : _ᛋ);
		}
		return ḿ;
	}

	private void ᛦ()
	{
		_26AB ᛄ = _ᛄ;
		_ᛄ = null;
		if (ᛄ != null)
		{
			((NativeWindow)ᛄ).ReleaseHandle();
		}
	}

	private void ᛧ(IntPtr ह)
	{
		if (_ᛄ == null)
		{
			_ᛄ = new _26AB();
			((NativeWindow)_ᛄ).AssignHandle(ह);
		}
	}

	protected override bool ProcessDialogKey(Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if ((int)keyData == 115 && ᚪ)
		{
			IsPopupOpen = !IsPopupOpen;
			return true;
		}
		return ((Control)this).ProcessDialogKey(keyData);
	}

	protected override void WndProc(ref Message m)
	{
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Invalid comparison between Unknown and I4
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		MethodInvoker val = null;
		_0EFE.RECT r = default(_0EFE.RECT);
		switch (((Message)(ref m)).Msg)
		{
		case 308:
			if (ᚪ && ((ComboBox)this).DroppedDown)
			{
				if (val == null)
				{
					val = (MethodInvoker)delegate
					{
						((ComboBox)this).DroppedDown = false;
					};
				}
				BarUtilities.InvokeDelayed(val, 10);
				break;
			}
			if (BarFunctions.IsWindows7 && !ᚪ)
			{
				if (ᚵ != ((Message)(ref m)).LParam)
				{
					ᛦ();
				}
				if (_ᛄ == null)
				{
					ᛧ(((Message)(ref m)).LParam);
				}
			}
			ᚵ = ((Message)(ref m)).LParam;
			if (!ᚪ && (ᚴ > 0 || Items.Count == 0))
			{
				_0EFE.GetWindowRect(((Message)(ref m)).LParam, ref r);
				int cy = ᚴ;
				if (Items.Count == 0)
				{
					cy = Math.Max(18, ((ComboBox)this).ItemHeight);
				}
				Point point = ((Control)this).PointToScreen(Point.Empty);
				if (r.Top < point.Y)
				{
					r.Top = point.Y + ((Control)this).Height;
				}
				_089F.SetWindowPos(((Message)(ref m)).LParam, IntPtr.Zero, r.Left, r.Top, r.Width, cy, 16);
			}
			break;
		case 343:
			((ComboBox)this).WndProc(ref m);
			if (((Message)(ref m)).Result == IntPtr.Zero && ᚧ && !ᚪ)
			{
				ᚧ = false;
			}
			return;
		case 7:
			if (((Message)(ref m)).WParam != ((Control)this).Handle)
			{
				_0A03 = ((Message)(ref m)).WParam;
			}
			break;
		case 323:
			if (Items.Count > 0 && Items[Items.Count - 1] is ComboItem comboItem2)
			{
				comboItem2.ᚐ = this;
			}
			break;
		case 330:
		{
			int num2 = _0EFE._0F11(((Message)(ref m)).WParam);
			if (num2 >= 0 && num2 < Items.Count && Items[num2] is ComboItem comboItem)
			{
				comboItem.ᚐ = this;
			}
			ᛇ = -1;
			break;
		}
		case 324:
			ᛇ = -1;
			break;
		case 1031:
			if ((int)((ComboBox)this).DropDownStyle == 1 && !હ)
			{
				((ComboBox)this).SelectionLength = 0;
			}
			((Control)this).Invalidate(true);
			return;
		case 513:
			if (ᚪ)
			{
				if (IsPopupOpen)
				{
					_16FF();
				}
				else if (DateTime.Now.Subtract(_ᛔ).TotalMilliseconds > 500.0 && (((Control)this).Focused || ((Control)this).Focus()))
				{
					_16FD();
				}
				return;
			}
			break;
		case 8465:
			if (ᚪ)
			{
				int num = _0EFE._0F0D(((Message)(ref m)).WParam);
				if ((long)num == 7)
				{
					_16FD();
					return;
				}
				if ((long)num == 8 && DateTime.Now.Subtract(_ᛓ).TotalMilliseconds > 900.0)
				{
					_16FF();
					return;
				}
			}
			break;
		}
		if (ᚯ)
		{
			((ComboBox)this).WndProc(ref m);
		}
		else if ((((Message)(ref m)).Msg == 15 || ((Message)(ref m)).Msg == 792) && (int)((ComboBox)this).DrawMode != 0)
		{
			_0EFE.GetWindowRect(((Message)(ref m)).HWnd, ref r);
			Rectangle rectangle = new Rectangle(0, 0, r.Width, r.Height);
			if (((Message)(ref m)).Msg == 15)
			{
				_0EFE.PAINTSTRUCT ps = default(_0EFE.PAINTSTRUCT);
				IntPtr intPtr = _0EFE.BeginPaint(((Message)(ref m)).HWnd, ref ps);
				Graphics val2 = Graphics.FromHdc(intPtr);
				try
				{
					using BufferedBitmap bufferedBitmap = new BufferedBitmap(val2, rectangle);
					ᛨ(bufferedBitmap.Graphics, rectangle);
					bufferedBitmap.Render(val2);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
				_0EFE.EndPaint(((Message)(ref m)).HWnd, ref ps);
			}
			else
			{
				Graphics val3 = Graphics.FromHdc(((Message)(ref m)).WParam);
				try
				{
					ᛨ(val3, rectangle);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			if (((Control)this).Parent is ItemControl)
			{
				((ItemControl)(object)((Control)this).Parent).ટ();
			}
		}
		else
		{
			((ComboBox)this).WndProc(ref m);
		}
	}

	public override bool PreProcessMessage(ref Message m)
	{
		if (((Message)(ref m)).Msg == 8465 && ᚪ && (long)_0EFE._0F0D(((Message)(ref m)).WParam) == 7)
		{
			return false;
		}
		return ((Control)this).PreProcessMessage(ref m);
	}

	private void ᛨ(Graphics _٥, Rectangle ल)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Invalid comparison between Unknown and I4
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		ḿ ᛪ = ᛥ();
		_16EE(_٥, ल, ᛪ);
		_16EC(_٥, ल, ᛪ);
		Rectangle rectangle = ᛩ(_٥, ल, ᛪ);
		int num = ((((ListControl)this).SelectedIndex == -1) ? (-1) : ᛇ);
		if (ᛇ == -1 && ((ListControl)this).SelectedIndex >= 0)
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
			_٥.SetClip(rectangle);
			rectangle.Inflate(-1, -1);
			((ComboBox)this).OnDrawItem(new DrawItemEventArgs(_٥, ((Control)this).Font, rectangle, num, val, ((Control)this).ForeColor, ((Control)this).BackColor));
			_٥.ResetClip();
		}
		if (_16EF())
		{
			_16F0(_٥);
		}
	}

	private Rectangle ᛩ(Graphics _٥, Rectangle ल, ḿ ᛪ)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Invalid comparison between Unknown and I4
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Invalid comparison between Unknown and I4
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Invalid comparison between Unknown and I4
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Expected O, but got Unknown
		Rectangle result = ल;
		result.Inflate(-2, -2);
		if ((int)((ComboBox)this).DropDownStyle == 0 || !_ᛅ)
		{
			return result;
		}
		int horizontalScrollBarThumbWidth = SystemInformation.HorizontalScrollBarThumbWidth;
		Rectangle rectangle = new Rectangle(ल.Width - horizontalScrollBarThumbWidth, ल.Y, horizontalScrollBarThumbWidth, ल.Height);
		if ((int)((Control)this).RightToLeft == 1)
		{
			rectangle = new Rectangle(ल.X + 1, ल.Y + 1, horizontalScrollBarThumbWidth, ल.Height - 2);
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
		if (!IsToolbarStyle && BarFunctions.IsOffice2007Style(Style))
		{
			ᯊ.ᯓ(_٥, GetOffice2007StateColorTable(), rectangle, RoundRectangleShapeDescriptor.RectangleShape);
		}
		else
		{
			if (ᛪ.Ṁ != null)
			{
				DisplayHelp.FillRectangle(_٥, rectangle, ᛪ.Ṁ);
			}
			if (ᛪ.Ṃ != null)
			{
				DisplayHelp.DrawGradientRectangle(_٥, rectangle, ᛪ.Ṃ, 1);
			}
			Rectangle bounds = rectangle;
			bounds.Inflate(-1, -1);
			if (ᛪ.ṃ != null)
			{
				DisplayHelp.DrawGradientRectangle(_٥, bounds, ᛪ.ṃ, 1);
			}
		}
		SolidBrush val = new SolidBrush(ᛪ.ṁ);
		try
		{
			ᛲ(rectangle, _٥, (Brush)(object)val);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		ᚻ = rectangle;
		return result;
	}

	internal Rectangle _16EB(Rectangle ल)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Invalid comparison between Unknown and I4
		if ((int)((ComboBox)this).DropDownStyle == 0 || !_ᛅ)
		{
			return Rectangle.Empty;
		}
		int horizontalScrollBarThumbWidth = SystemInformation.HorizontalScrollBarThumbWidth;
		Rectangle result = new Rectangle(ल.Width - horizontalScrollBarThumbWidth, ल.Y, horizontalScrollBarThumbWidth, ल.Height);
		if ((int)((Control)this).RightToLeft == 1)
		{
			result = new Rectangle(ल.X + 1, ल.Y + 1, horizontalScrollBarThumbWidth, ल.Height - 2);
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
		bool flag = ᚰ || (ᚧ && (int)((ComboBox)this).DropDownStyle != 0);
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			Office2007ColorTable colorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
			Office2007ButtonItemColorTable office2007ButtonItemColorTable = colorTable.ButtonItemColors[Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground)];
			if (!((Control)this).Enabled)
			{
				return office2007ButtonItemColorTable.Disabled;
			}
			if (ᚧ)
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

	private void _16EC(Graphics _٥, Rectangle _16ED, ḿ ᛪ)
	{
		if (_ᛆ)
		{
			DisplayHelp.DrawRectangle(_٥, ᛪ.ᙴ, _16ED);
		}
	}

	private void _16EE(Graphics _٥, Rectangle _16ED, ḿ ᛪ)
	{
		if (UseCustomBackColor && !((Control)this).BackColor.IsEmpty)
		{
			DisplayHelp.FillRectangle(_٥, _16ED, ((Control)this).BackColor);
		}
		else
		{
			DisplayHelp.FillRectangle(_٥, _16ED, ᛪ.ཥ);
		}
	}

	private bool _16EF()
	{
		if (_ᚿ && ((Control)this).Enabled && (!((Control)this).Focused || _ᛀ == eWatermarkBehavior.HideNonEmpty) && ((Control)this).Text == "" && ((ListControl)this).SelectedIndex == -1)
		{
			return true;
		}
		return false;
	}

	private void _16F0(Graphics _٥)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		if (ݶ != null)
		{
			MarkupDrawContext d = ᛟ(_٥);
			ݶ.Render(d);
			return;
		}
		eTextFormat eTextFormat = eTextFormat.VerticalCenter;
		if ((int)((Control)this).RightToLeft == 1)
		{
			eTextFormat |= eTextFormat.RightToLeft;
		}
		eTextFormat |= eTextFormat.EndEllipsis;
		eTextFormat |= eTextFormat.WordBreak;
		TextDrawing.DrawString(_٥, ᚷ, (ᚸ == null) ? ((Control)this).Font : ᚸ, ᚹ, ᛱ(), eTextFormat);
	}

	private Rectangle ᛱ()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Invalid comparison between Unknown and I4
		if ((int)((ComboBox)this).DropDownStyle != 2 && ᚶ != null)
		{
			_0EFE.RECT r = default(_0EFE.RECT);
			_0EFE.GetWindowRect(((NativeWindow)ᚶ).Handle, ref r);
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

	private void ᛲ(Rectangle ल, Graphics _٥, Brush _0BCF)
	{
		Point[] array = new Point[3];
		array[0].X = ल.Left + (ल.Width - Dpi.Width4) / 2;
		array[0].Y = ल.Top + (ल.Height - Dpi.Height3) / 2 + 1;
		array[1].X = array[0].X + Dpi.Width5;
		array[1].Y = array[0].Y;
		array[2].X = array[0].X + Dpi.Width2;
		array[2].Y = array[0].Y + Dpi.Height3;
		_٥.FillPolygon(_0BCF, array);
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		((Control)this).OnVisibleChanged(e);
		ݡ = false;
		if ((int)((ComboBox)this).DropDownStyle == 1 && Items.Count > 0 && Items[0] is ComboItem && ((ListControl)this).DisplayMember != "")
		{
			string displayMember = ((ListControl)this).DisplayMember;
			((ListControl)this).DisplayMember = "";
			((ListControl)this).DisplayMember = displayMember;
		}
		if (((Control)this).IsHandleCreated && !((Control)this).IsDisposed)
		{
			ᛸ();
		}
		if (!((Control)this).Visible && ᚪ && IsPopupOpen)
		{
			IsPopupOpen = false;
		}
	}

	protected override void OnSelectedIndexChanged(EventArgs e)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Invalid comparison between Unknown and I4
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Invalid comparison between Unknown and I4
		ᛇ = ((ListControl)this).SelectedIndex;
		if (!ᚧ)
		{
			if (ᚧ)
			{
				ᚧ = false;
			}
			if (!ݡ)
			{
				((Control)this).Invalidate(true);
			}
		}
		if (((ListControl)this).SelectedIndex == -1 && _ᛀ == eWatermarkBehavior.HideNonEmpty && ᚷ.Length > 0 && ((Control)this).Text == "")
		{
			((Control)this).Invalidate(true);
		}
		else if ((int)((ComboBox)this).DropDownStyle == 2)
		{
			((Control)this).Invalidate();
		}
		((ComboBox)this).OnSelectedIndexChanged(e);
		if (!_ᛈ && (int)((ComboBox)this).DropDownStyle != 1 && !(((ComboBox)this).SelectedItem is string))
		{
			try
			{
				_ᛈ = true;
				if (((ComboBox)this).SelectedItem == null)
				{
					((Control)this).Text = "";
				}
				else
				{
					((Control)this).Text = ((ListControl)this).GetItemText(((ComboBox)this).SelectedItem);
				}
			}
			finally
			{
				_ᛈ = false;
			}
		}
		ExecuteCommand();
	}

	protected override void OnTextChanged(EventArgs e)
	{
		if (((ListControl)this).SelectedIndex == -1 && _ᛀ == eWatermarkBehavior.HideNonEmpty && ᚷ.Length > 0 && ((Control)this).Text == "")
		{
			((Control)this).Invalidate(true);
		}
		((ComboBox)this).OnTextChanged(e);
	}

	protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (Dpi.RecordScalePerControl)
		{
			Dpi.SetScaling(factor);
		}
		((ComboBox)this).ScaleControl(factor, specified);
		if (_ᛉ && !((Component)this).DesignMode)
		{
			ᛳ();
		}
	}

	protected override void Dispose(bool disposing)
	{
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		if (ᄧ != null)
		{
			ᄧ.Enabled = false;
			((Component)(object)ᄧ).Dispose();
			ᄧ = null;
		}
		ᚳ = true;
		if (ᚶ != null)
		{
			((NativeWindow)ᚶ).ReleaseHandle();
			ᚶ = null;
		}
		ᛦ();
		if (_ᛎ != null)
		{
			_ᛎ.AfterNodeSelect -= ᜅ;
			_ᛎ.BeforeNodeSelect -= ᜆ;
			_ᛎ.NodeMouseDown -= ᜃ;
			_ᛎ.DataColumnCreated -= ᜂ;
			((Control)_ᛎ).KeyDown -= new KeyEventHandler(ᜁ);
			((Component)(object)_ᛎ).Dispose();
			_ᛎ = null;
		}
		if (_ᛕ != null)
		{
			_ᛕ._19E5 -= new ToolStripDropDownClosedEventHandler(_16FE);
			_ᛕ.Dispose();
			_ᛕ = null;
		}
		((ComboBox)this).Dispose(disposing);
	}

	protected override void OnFontChanged(EventArgs e)
	{
		((ComboBox)this).OnFontChanged(e);
		ᛳ();
	}

	private void ᛳ()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		if (!((Control)this).Disposing && !((Control)this).IsDisposed && !ᚳ && (int)((ComboBox)this).DrawMode == 1 && ((Control)this).IsHandleCreated && ((Control)this).Parent != null && !((Control)this).Parent.IsDisposed)
		{
			((ComboBox)this).ItemHeight = ((Control)this).FontHeight + 2;
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
		if (ᚳ)
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
		if (((ComboItem)obj).Ṉ)
		{
			ᛷ(e);
			return;
		}
		Size comboItemSize = GetComboItemSize(obj as ComboItem);
		e.ItemHeight = comboItemSize.Height;
		e.ItemWidth = comboItemSize.Width;
		if (BarFunctions.IsOffice2007Style(Ӷ))
		{
			e.ItemHeight += 6;
			e.ItemWidth += 6;
		}
	}

	protected override void OnDrawItem(DrawItemEventArgs e)
	{
		((ComboBox)this).OnDrawItem(e);
		ᛴ(e);
	}

	private void ᛴ(DrawItemEventArgs ؿ)
	{
		if (!ᚳ && ؿ.Index >= 0)
		{
			object obj = Items[ؿ.Index];
			if (obj is ComboItem)
			{
				DrawComboItem(ؿ);
			}
			else
			{
				DrawObjectItem(ؿ);
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
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Invalid comparison between Unknown and I4
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		string itemText = ((ListControl)this).GetItemText(Items[e.Index]);
		Color color = Color.Empty;
		Office2007ButtonItemStateColorTable office2007ButtonItemStateColorTable = null;
		if (BarFunctions.IsOffice2007Style(Ӷ))
		{
			Office2007ButtonItemColorTable comboItemColor = GetComboItemColor();
			if ((e.State & 1) != 0 || (e.State & 0x40) != 0)
			{
				office2007ButtonItemStateColorTable = comboItemColor.MouseOver;
				if (color.IsEmpty)
				{
					color = office2007ButtonItemStateColorTable.Text;
				}
			}
			else
			{
				office2007ButtonItemStateColorTable = (((e.State & 4) == 0 && ((Control)this).Enabled) ? comboItemColor.Default : comboItemColor.Disabled);
			}
			if ((e.State & 4) != 0 || !((Control)this).Enabled)
			{
				color = (_ᛋ.IsEmpty ? SystemColors.ControlDark : _ᛋ);
			}
			else if (color.IsEmpty)
			{
				color = ((Control)this).ForeColor;
			}
			if ((e.State & 0x40) != 0 || (e.State & 1) != 0)
			{
				Rectangle bounds = e.Bounds;
				if ((e.State & 0x1000) != 0)
				{
					bounds.Inflate(1, 1);
				}
				ᯊ.ᯓ(graphics, office2007ButtonItemStateColorTable, bounds, StyleManager.IsMetro(Style) ? RoundRectangleShapeDescriptor.RectangleShape : RoundRectangleShapeDescriptor.RoundCorner2);
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
				color = SystemColors.HighlightText;
			}
			else
			{
				color = (((e.State & 4) != 0 || (e.State & 2) != 0) ? (_ᛋ.IsEmpty ? SystemColors.ControlDark : _ᛋ) : ((!((Control)this).ForeColor.IsEmpty) ? ((Control)this).ForeColor : SystemColors.ControlText));
			}
		}
		if ((e.State & 0x10) != 0)
		{
			ᛵ(e);
		}
		Rectangle bounds2 = e.Bounds;
		if ((e.State & 0x1000) != 4096)
		{
			if (BarFunctions.IsOffice2007Style(Ӷ))
			{
				bounds2.Inflate(-3, 0);
			}
			else
			{
				bounds2.Inflate(-2, 0);
			}
		}
		TextDrawing.DrawString(graphics, itemText, ((Control)this).Font, color, bounds2, eTextFormat.NoClipping | eTextFormat.NoPrefix);
	}

	protected virtual Office2007ButtonItemColorTable GetComboItemColor()
	{
		if (_ᛊ != null)
		{
			return _ᛊ;
		}
		return ((Office2007Renderer)GlobalManager.Renderer).ColorTable.ButtonItemColors[0];
	}

	private void ᛵ(DrawItemEventArgs ؿ)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Invalid comparison between Unknown and I4
		if (_ᒖ && (ؿ.State & 0x10) == 16 && (ؿ.State & 0x200) != 512 && (ؿ.State & 0x200) == 4096)
		{
			Rectangle bounds = ؿ.Bounds;
			bounds.Inflate(2, 2);
			ControlPaint.DrawFocusRectangle(ؿ.Graphics, bounds, ؿ.ForeColor, ؿ.BackColor);
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShouldSerializeDisabledForeColor()
	{
		return !_ᛋ.IsEmpty;
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public void ResetDisabledForeColor()
	{
		DisabledForeColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDisabledBackColor()
	{
		return !_ᖣ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDisabledBackColor()
	{
		DisabledBackColor = Color.Empty;
	}

	protected virtual void DrawComboItem(DrawItemEventArgs e)
	{
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Invalid comparison between Unknown and I4
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Invalid comparison between Unknown and I4
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Invalid comparison between Unknown and I4
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Invalid comparison between Unknown and I4
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Invalid comparison between Unknown and I4
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Expected O, but got Unknown
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Invalid comparison between Unknown and I4
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Invalid comparison between Unknown and I4
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Expected O, but got Unknown
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Expected O, but got Unknown
		ComboItem comboItem = Items[e.Index] as ComboItem;
		if (comboItem.Ṉ)
		{
			ᛶ(e);
			return;
		}
		Graphics graphics = e.Graphics;
		Image val = null;
		Color color = comboItem.ForeColor;
		if (comboItem.ImageIndex >= 0 && գ != null && գ.Images.Count > comboItem.ImageIndex)
		{
			val = գ.Images[comboItem.ImageIndex];
		}
		else if (comboItem.Image != null)
		{
			val = comboItem.Image;
		}
		Office2007ButtonItemStateColorTable office2007ButtonItemStateColorTable = null;
		if (BarFunctions.IsOffice2007Style(Ӷ))
		{
			Office2007ButtonItemColorTable comboItemColor = GetComboItemColor();
			if ((e.State & 1) != 0 || (e.State & 0x40) != 0)
			{
				office2007ButtonItemStateColorTable = comboItemColor.MouseOver;
				if (color.IsEmpty)
				{
					color = office2007ButtonItemStateColorTable.Text;
				}
			}
			else
			{
				office2007ButtonItemStateColorTable = (((e.State & 4) == 0 && ((Control)this).Enabled) ? comboItemColor.Default : comboItemColor.Disabled);
			}
			if (!((Control)this).Enabled)
			{
				color = (_ᛋ.IsEmpty ? SystemColors.ControlDark : _ᛋ);
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
			Rectangle bounds = e.Bounds;
			if ((e.State & 0x1000) != 0)
			{
				bounds.Inflate(1, 1);
			}
			if (BarFunctions.IsOffice2007Style(Ӷ))
			{
				ᯊ.ᯓ(graphics, office2007ButtonItemStateColorTable, bounds, StyleManager.IsMetro(Style) ? RoundRectangleShapeDescriptor.RectangleShape : RoundRectangleShapeDescriptor.RoundCorner2);
			}
			else
			{
				e.DrawBackground();
			}
			ᛵ(e);
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
		Rectangle bounds2 = e.Bounds;
		Rectangle bounds3 = e.Bounds;
		if ((e.State & 0x1000) == 4096)
		{
			if (val != null)
			{
				bounds3.X += 3;
				bounds3.Width -= 3;
			}
		}
		else if (BarFunctions.IsOffice2007Style(Ӷ))
		{
			bounds3.Inflate(-3, 0);
		}
		else
		{
			bounds3.Inflate(-2, 0);
		}
		if (val != null)
		{
			bounds2.Width = val.Width;
			bounds2.Height = val.Height;
			if ((int)comboItem.ImagePosition == 0)
			{
				if (e.Bounds.Height > val.Height)
				{
					bounds2.Y += (e.Bounds.Height - val.Height) / 2;
				}
				bounds3.Width -= bounds2.Width;
				bounds3.X += bounds2.Width;
			}
			else if ((int)comboItem.ImagePosition == 1)
			{
				if (e.Bounds.Height > val.Height)
				{
					bounds2.Y += (e.Bounds.Height - val.Height) / 2;
				}
				bounds2.X = e.Bounds.Right - val.Width;
				bounds3.Width -= bounds2.Width;
			}
			else
			{
				bounds2.X += (e.Bounds.Width - val.Width) / 2;
				bounds3.Y = bounds2.Bottom;
			}
			graphics.DrawImage(val, bounds2);
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
		TextDrawing.DrawString(graphics, comboItem.Text, val3, color, bounds3, eTextFormat);
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
			val = գ.Images[item.ImageIndex];
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
		FontFamily[] families = ((FontCollection)val).Families;
		FontFamily[] array = families;
		foreach (FontFamily val2 in array)
		{
			ComboItem comboItem = new ComboItem();
			comboItem.Ṉ = true;
			comboItem.FontName = val2.GetName(0);
			comboItem.FontSize = ((Control)this).Font.Size;
			comboItem.Text = val2.GetName(0);
			Items.Add((object)comboItem);
		}
		((ComboBox)this).DropDownWidth = ((Control)this).Width * 2;
	}

	private void ᛶ(DrawItemEventArgs ؿ)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Invalid comparison between Unknown and I4
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Invalid comparison between Unknown and I4
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Expected O, but got Unknown
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		FontStyle[] array = (FontStyle[])(object)new FontStyle[4]
		{
			default(FontStyle),
			(FontStyle)1,
			(FontStyle)2,
			(FontStyle)3
		};
		if (!BarFunctions.IsOffice2007Style(Ӷ))
		{
			ؿ.DrawBackground();
		}
		string text = Items[ؿ.Index].ToString();
		FontFamily val = new FontFamily(text);
		int num = ((ComboBox)this).DropDownWidth / 2 - 4;
		if (num <= 0)
		{
			num = ((Control)this).Width;
		}
		FontStyle[] array2 = array;
		foreach (FontStyle val2 in array2)
		{
			if (!val.IsStyleAvailable(val2))
			{
				continue;
			}
			eTextFormat eTextFormat = eTextFormat.NoPrefix;
			Color color = (((ؿ.State & 1) != 0) ? SystemColors.HighlightText : SystemColors.ControlText);
			Office2007ButtonItemStateColorTable office2007ButtonItemStateColorTable = null;
			if (BarFunctions.IsOffice2007Style(Ӷ))
			{
				Office2007ButtonItemColorTable comboItemColor = GetComboItemColor();
				office2007ButtonItemStateColorTable = (((ؿ.State & 1) == 0 && (ؿ.State & 0x40) == 0) ? (((ؿ.State & 4) == 0 && ((Control)this).Enabled) ? comboItemColor.Default : comboItemColor.Disabled) : comboItemColor.MouseOver);
				color = ((!((Control)this).Enabled) ? (_ᛋ.IsEmpty ? SystemColors.ControlDark : _ᛋ) : (StyleManager.IsMetro(StyleManager.Style) ? office2007ButtonItemStateColorTable.Text : ((!((Control)this).ForeColor.IsEmpty) ? ((Control)this).ForeColor : SystemColors.ControlText)));
				if ((ؿ.State & 1) != 0 || (ؿ.State & 0x40) != 0)
				{
					ᯊ.ᯓ(ؿ.Graphics, office2007ButtonItemStateColorTable, ؿ.Bounds, StyleManager.IsMetro(Style) ? RoundRectangleShapeDescriptor.RectangleShape : RoundRectangleShapeDescriptor.RoundCorner2);
				}
				else
				{
					ؿ.DrawBackground();
				}
			}
			if ((ؿ.State & 0x1000) == 4096)
			{
				Rectangle bounds = ؿ.Bounds;
				bounds.Height = 0;
				eTextFormat |= eTextFormat.NoClipping | eTextFormat.NoPadding;
				TextDrawing.DrawString(ؿ.Graphics, text, ((Control)this).Font, color, bounds, eTextFormat);
				break;
			}
			Size size = TextDrawing.MeasureString(ؿ.Graphics, text, ((Control)this).Font);
			int num2 = (ؿ.Bounds.Height - size.Height) / 2;
			Rectangle bounds2 = new Rectangle(ؿ.Bounds.X, ؿ.Bounds.Y + num2, ((ؿ.State & 4) == 4) ? ؿ.Bounds.Width : Math.Max(ؿ.Bounds.Width - 100, 32), ؿ.Bounds.Height - num2);
			TextDrawing.DrawString(ؿ.Graphics, text, ((Control)this).Font, color, bounds2, eTextFormat);
			Rectangle bounds3 = new Rectangle(ؿ.Bounds.X + num + 4, ؿ.Bounds.Y, ؿ.Bounds.Width + 100, ؿ.Bounds.Height);
			Font val3 = new Font(val, ((Control)this).Font.Size, val2);
			try
			{
				TextDrawing.DrawString(ؿ.Graphics, text, val3, color, bounds3, eTextFormat);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			break;
		}
		if (val != null)
		{
			val.Dispose();
		}
	}

	private void ᛷ(MeasureItemEventArgs ؿ)
	{
		ؿ.ItemHeight = Dpi.Height18;
	}

	public void ReleaseFocus()
	{
		if (!_ᛌ || !((Control)this).Focused || !(_0A03 != IntPtr.Zero))
		{
			return;
		}
		Control val = Control.FromChildHandle(_0A03);
		if (val != this)
		{
			if (val != null)
			{
				val.Focus();
			}
			else
			{
				_089F.SetFocus(_0A03);
			}
			((Control)this).OnLostFocus(new EventArgs());
		}
		_0A03 = IntPtr.Zero;
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		if ((int)keyData == 13 && ᚕ)
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
		ᛸ();
		((Control)this).OnEnabledChanged(e);
	}

	private void ᛸ()
	{
		if (!ᛁ)
		{
			Color ཥ = ᛥ().ཥ;
			if (((Control)this).BackColor != ཥ)
			{
				((Control)this).BackColor = ཥ;
			}
		}
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		if (!ݡ)
		{
			ݡ = true;
			ᛸ();
			((Control)this).Invalidate(true);
		}
		_16F9(ԝ: true);
		((ComboBox)this).OnMouseEnter(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		_16F9(ԝ: true);
		((ComboBox)this).OnMouseLeave(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		if (!ݡ)
		{
			ݡ = true;
			((Control)this).Invalidate();
			_16F9(ԝ: true);
		}
	}

	private void _16F9(bool ԝ)
	{
		if (ᄧ != null)
		{
			ᄧ.Enabled = ԝ;
		}
	}

	private void _16FA()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		if ((int)((ComboBox)this).DropDownStyle != 2 && (int)((ComboBox)this).AutoCompleteMode == 0)
		{
			if (ᚶ == null)
			{
				IntPtr window = GetWindow(((Control)this).Handle, 5u);
				if (window != IntPtr.Zero)
				{
					ᚶ = new _26A9();
					ᚶ.ભ += _16FC;
					ᚶ._139A += new PaintEventHandler(_16FB);
					((NativeWindow)ᚶ).AssignHandle(window);
				}
			}
		}
		else if (ᚶ != null)
		{
			((NativeWindow)ᚶ).ReleaseHandle();
			ᚶ = null;
		}
	}

	protected override void OnDropDownStyleChanged(EventArgs e)
	{
		_16FA();
		((ComboBox)this).OnDropDownStyleChanged(e);
	}

	private void _16FB(object _٠, PaintEventArgs ؿ)
	{
		if (_16EF())
		{
			_16F0(ؿ.Graphics);
		}
	}

	private void _16FC(object _٠, EventArgs ؿ)
	{
		if (ݡ)
		{
			_16F9(ԝ: true);
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFocusHighlightColor()
	{
		return !_ᚾ.Equals((object?)_ᚽ);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void ResetFocusHighlightColor()
	{
		FocusHighlightColor = _ᚽ;
	}

	protected override void OnLeave(EventArgs e)
	{
		if (ᚪ && IsPopupOpen)
		{
			IsPopupOpen = false;
		}
		((Control)this).OnLeave(e);
	}

	protected virtual void OnDropDownColumnsChanged(string oldValue, string newValue)
	{
		_ᛒ |= _26AC._26AD;
	}

	protected virtual void OnDropDownColumnsHeadersChanged(string oldValue, string newValue)
	{
		_ᛒ |= _26AC._26AD;
		if (!string.IsNullOrEmpty(_ᛏ))
		{
			_ᛐ = _ᛏ.Split(new char[1] { '\r' });
			for (int i = 0; i < _ᛐ.Length; i++)
			{
				_ᛐ[i] = _ᛐ[i].Replace("\n", "");
			}
		}
		else
		{
			_ᛐ = new string[0];
		}
	}

	protected virtual void OnBeforeMultiColumnPopupOpen(CancelEventArgs e)
	{
		ᛑ?.Invoke(this, e);
	}

	private void _16FD()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		if (_ᛕ == null)
		{
			_ᛕ = new _26B0();
			_ᛕ._19E5 += new ToolStripDropDownClosedEventHandler(_16FE);
		}
		if (_ᛎ == null)
		{
			_ᛎ = ᜀ();
			_ᛒ = _26AC._26AF;
		}
		if (ᜇ(_26AC._26AE))
		{
			((Control)_ᛎ).BindingContext = ((Control)this).BindingContext;
		}
		if (ᜇ(_26AC._05C1))
		{
			_ᛎ.DataSource = ((ComboBox)this).DataSource;
		}
		if (ᜇ(_26AC._26AD))
		{
			_ᛎ.DisplayMembers = _ᛍ;
		}
		if (!_ᛕ._26B3 || !_ᛙ)
		{
			((Control)_ᛎ).Height = Dpi.Height(((ComboBox)this).DropDownHeight);
			((Control)_ᛎ).Width = Dpi.Width(((ComboBox)this).DropDownWidth);
		}
		_ᛕ._26B4 = _ᛛ;
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnBeforeMultiColumnPopupOpen(cancelEventArgs);
		if (!cancelEventArgs.Cancel)
		{
			_ᛎ.SelectedIndex = ((ListControl)this).SelectedIndex;
			Point point = ((Control)this).PointToScreen(new Point(0, ((Control)this).Height));
			ePopupResizeEdge _26BB = (_ᛚ ? ePopupResizeEdge.BottomRight : ePopupResizeEdge.None);
			_ᛕ._26BA((Control)(object)_ᛎ, point.X, point.Y, 0, 0, _26BB);
			if (_ᛎ.SelectedNode != null)
			{
				_ᛎ.SelectedNode.EnsureVisible();
			}
			_ᚱ = true;
			_ᛓ = DateTime.Now;
			_ᛒ = _26AC.ᘶ;
			((ComboBox)this).OnDropDown(EventArgs.Empty);
		}
	}

	private void _16FE(object _٠, ToolStripDropDownClosedEventArgs ؿ)
	{
		_ᚱ = false;
		ᛇ = ((ListControl)this).SelectedIndex;
		_ᛔ = DateTime.Now;
		if (DisconnectedPopupDataSource && _ᛎ != null)
		{
			_ᛎ.DataSource = null;
			_ᛎ.DisplayMembers = string.Empty;
			((Control)_ᛎ).BindingContext = null;
			_ᛒ = _26AC._26AF;
		}
		((ComboBox)this).OnDropDownClosed((EventArgs)(object)ؿ);
	}

	private void _16FF()
	{
		if (_ᛕ != null && _ᚱ)
		{
			_ᛕ._26BC();
		}
	}

	private DevComponents.AdvTree.AdvTree ᜀ()
	{
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Expected O, but got Unknown
		DevComponents.AdvTree.AdvTree advTree = new DevComponents.AdvTree.AdvTree();
		advTree.EnableDataPositionChange = false;
		advTree.BackgroundStyle.Reset();
		advTree.BackgroundStyle.BackColor = SystemColors.Window;
		advTree.BackgroundStyle.Border = eStyleBorderType.None;
		advTree.ExpandButtonType = eExpandButtonType.Triangle;
		advTree.DragDropEnabled = false;
		advTree.HotTracking = true;
		advTree.Indent = 0;
		advTree.ExpandWidth = 0;
		advTree.SelectionFocusAware = false;
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Name = "nodeElementStyle";
		elementStyle.TextColor = SystemColors.ControlText;
		advTree.NodeStyle = elementStyle;
		advTree.PathSeparator = ";";
		advTree.Styles.Add(elementStyle);
		advTree.AfterNodeSelect += ᜅ;
		advTree.BeforeNodeSelect += ᜆ;
		advTree.NodeMouseDown += ᜃ;
		advTree.NodeClick += ᜄ;
		advTree.DataColumnCreated += ᜂ;
		((Control)advTree).KeyDown += new KeyEventHandler(ᜁ);
		if (_ᛜ != null)
		{
			((Control)advTree).Font = _ᛜ;
		}
		return advTree;
	}

	private void ᜁ(object _٠, KeyEventArgs ؿ)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		if (_ᚱ && (int)ؿ.KeyCode == 13)
		{
			((ListControl)this).SelectedIndex = _ᛎ.SelectedIndex;
			_16FF();
			((ComboBox)this).OnSelectionChangeCommitted((EventArgs)(object)ؿ);
		}
	}

	protected virtual void OnDataColumnCreated(DataColumnEventArgs e)
	{
		if (_05CC != null)
		{
			_05CC(this, e);
		}
	}

	private void ᜂ(object _٠, DataColumnEventArgs ؿ)
	{
		if (_ᛐ.Length > 0)
		{
			int num = ؿ.ColumnHeader.Parent.IndexOf(ؿ.ColumnHeader);
			if (num < _ᛐ.Length)
			{
				ؿ.ColumnHeader.Text = _ᛐ[num];
			}
		}
		OnDataColumnCreated(ؿ);
	}

	private void ᜃ(object _٠, TreeNodeMouseEventArgs ؿ)
	{
	}

	private void ᜄ(object _٠, TreeNodeMouseEventArgs ؿ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)ؿ.Button == 1048576 && ؿ.Node != null && ؿ.Node.IsSelected && _ᚱ)
		{
			((ListControl)this).SelectedIndex = _ᛎ.SelectedIndex;
			if (SelectionClosesPopup)
			{
				_16FF();
			}
			((ComboBox)this).OnSelectionChangeCommitted((EventArgs)ؿ);
		}
	}

	private void ᜅ(object _٠, AdvTreeNodeEventArgs ؿ)
	{
	}

	private void ᜆ(object _٠, AdvTreeNodeCancelEventArgs ؿ)
	{
		OnSelectionChanging(ؿ);
	}

	protected virtual void OnSelectionChanging(AdvTreeNodeCancelEventArgs e)
	{
		ᛗ?.Invoke(this, e);
	}

	protected override void OnDataSourceChanged(EventArgs e)
	{
		if (_ᛎ != null)
		{
			_ᛎ.DataSource = null;
		}
		_ᛒ |= _26AC._05C1;
		((ComboBox)this).OnDataSourceChanged(e);
	}

	protected override void OnBindingContextChanged(EventArgs e)
	{
		_ᛒ |= _26AC._26AE;
		((ListControl)this).OnBindingContextChanged(e);
	}

	private bool ᜇ(_26AC ᜈ)
	{
		return (_ᛒ & ᜈ) == ᜈ;
	}

	protected virtual void OnMultiColumnControlFontChanged(Font oldValue, Font newValue)
	{
		if (_ᛎ != null)
		{
			((Control)_ᛎ).Font = _ᛜ;
		}
	}

	protected virtual void ExecuteCommand()
	{
		if (_૦ != null)
		{
			CommandManager._૭(this);
		}
	}

	protected virtual void OnCommandChanged()
	{
	}

	[CompilerGenerated]
	private void ᜉ()
	{
		((ComboBox)this).DroppedDown = false;
	}
}
