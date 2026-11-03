using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

[DefaultEvent("TextChanged")]
[ToolboxItem(true)]
[Designer("DevComponents.DotNetBar.Design.RichTextBoxExDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[DefaultBindingProperty("Text")]
[ToolboxBitmap(typeof(RichTextBoxEx), "Controls.RichTextBoxEx.ico")]
[Description("Provides rich-text, RTF text editing.")]
[DefaultProperty("Text")]
[Docking(/*Could not decode attribute arguments.*/)]
public class RichTextBoxEx : ScrollbarControl
{
	private RichTextBox _ᮇ;

	private EventHandler ᮈ;

	private EventHandler ᮉ;

	private EventHandler ᮊ;

	private EventHandler ᮋ;

	private EventHandler ᮌ;

	private LinkClickedEventHandler ᮍ;

	private EventHandler ᮎ;

	private EventHandler _05B1;

	private EventHandler ᮏ;

	private EventHandler ᮐ;

	private Color _ᮑ = SystemColors.Window;

	private BorderStyle _ᮒ;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public RichTextBox RichTextBox
	{
		get
		{
			return _ᮇ;
		}
		internal set
		{
			_ᮇ = value;
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Browsable(false)]
	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	public override bool AntiAlias
	{
		get
		{
			return base.AntiAlias;
		}
		set
		{
			base.AntiAlias = value;
		}
	}

	[Category("Columns")]
	[Description("Indicates back color of RichTextBox.")]
	public Color BackColorRichTextBox
	{
		get
		{
			return _ᮑ;
		}
		set
		{
			_ᮑ = value;
			((Control)_ᮇ).BackColor = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public BorderStyle BorderStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _ᮒ;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_ᮒ = value;
		}
	}

	[Description("Indicates whether tab characters are accepted as input.")]
	[DefaultValue(false)]
	[Category("Behavior")]
	public bool AcceptsTab
	{
		get
		{
			return ((TextBoxBase)_ᮇ).AcceptsTab;
		}
		set
		{
			((TextBoxBase)_ᮇ).AcceptsTab = value;
		}
	}

	[Browsable(false)]
	public override bool AllowDrop
	{
		get
		{
			return ((Control)this).AllowDrop;
		}
		set
		{
			((Control)this).AllowDrop = value;
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether automatic word selection is enabled.")]
	[Category("Behavior")]
	public bool AutoWordSelection
	{
		get
		{
			return _ᮇ.AutoWordSelection;
		}
		set
		{
			_ᮇ.AutoWordSelection = value;
		}
	}

	[Browsable(false)]
	[Bindable(false)]
	public override Color BackColor
	{
		get
		{
			return ((Control)this).BackColor;
		}
		set
		{
			((Control)this).BackColor = value;
		}
	}

	[DefaultValue(0)]
	[Description("Indicates the indent for bullets in the control.")]
	[Localizable(true)]
	[Category("Behavior")]
	public int BulletIndent
	{
		get
		{
			return _ᮇ.BulletIndent;
		}
		set
		{
			_ᮇ.BulletIndent = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool CanRedo => _ᮇ.CanRedo;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool CanUndo => ((TextBoxBase)_ᮇ).CanUndo;

	public override ContextMenuStrip ContextMenuStrip
	{
		get
		{
			return ((Control)this).ContextMenuStrip;
		}
		set
		{
			((Control)this).ContextMenuStrip = value;
			((Control)_ᮇ).ContextMenuStrip = value;
		}
	}

	protected override Size DefaultSize => new Size(100, 100);

	[Description("Indicates whether URLs are automatically formatted as links.")]
	[DefaultValue(true)]
	[Category("Behavior")]
	public bool DetectUrls
	{
		get
		{
			return _ᮇ.DetectUrls;
		}
		set
		{
			_ᮇ.DetectUrls = value;
		}
	}

	[Description("Indicates whether drag/drop of text, pictures and other data is enabled.")]
	[Category("Behavior")]
	[DefaultValue(false)]
	public bool EnableAutoDragDrop
	{
		get
		{
			return _ᮇ.EnableAutoDragDrop;
		}
		set
		{
			_ᮇ.EnableAutoDragDrop = value;
		}
	}

	[Browsable(false)]
	public override bool Focused => ((Control)_ᮇ).Focused;

	[Category("Behavior")]
	[Description("Indicates whether selection should be hidden when the edit control loses focus.")]
	[DefaultValue(true)]
	public bool HideSelection
	{
		get
		{
			return ((TextBoxBase)_ᮇ).HideSelection;
		}
		set
		{
			((TextBoxBase)_ᮇ).HideSelection = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public RichTextBoxLanguageOptions LanguageOption
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _ᮇ.LanguageOption;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_ᮇ.LanguageOption = value;
		}
	}

	[MergableProperty(false)]
	[Description("Indicates lines of text in a multi-line edit, as an array of String values.")]
	[Localizable(true)]
	[Category("Appearance")]
	[Editor("System.Windows.Forms.Design.StringArrayEditor, System.Design, Version= 2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string[] Lines
	{
		get
		{
			return ((TextBoxBase)_ᮇ).Lines;
		}
		set
		{
			((TextBoxBase)_ᮇ).Lines = value;
		}
	}

	[Description("Indicates maximum number of characters that can be entered into the edit control.")]
	[Category("Behavior")]
	[Localizable(true)]
	[DefaultValue(int.MaxValue)]
	public int MaxLength
	{
		get
		{
			return ((TextBoxBase)_ᮇ).MaxLength;
		}
		set
		{
			((TextBoxBase)_ᮇ).MaxLength = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool Modified => ((TextBoxBase)_ᮇ).Modified;

	[Category("Behavior")]
	[Localizable(true)]
	[DefaultValue(true)]
	[RefreshProperties(RefreshProperties.All)]
	[Description("Indicates whether the text in the control can span more than one line.")]
	public bool Multiline
	{
		get
		{
			return ((TextBoxBase)_ᮇ).Multiline;
		}
		set
		{
			((TextBoxBase)_ᮇ).Multiline = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[Localizable(false)]
	public Padding Padding
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).Padding;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Padding = value;
		}
	}

	[Description("Indicates whether the text in the edit control can be changed or not.")]
	[DefaultValue(false)]
	[RefreshProperties(RefreshProperties.Repaint)]
	[Category("Behavior")]
	public bool ReadOnly
	{
		get
		{
			return ((TextBoxBase)_ᮇ).ReadOnly;
		}
		set
		{
			((TextBoxBase)_ᮇ).ReadOnly = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public string RedoActionName => _ᮇ.RedoActionName;

	[DefaultValue(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool RichTextShortcutsEnabled
	{
		get
		{
			return _ᮇ.RichTextShortcutsEnabled;
		}
		set
		{
			_ᮇ.RichTextShortcutsEnabled = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(0)]
	[Description("Indicates the right margin dimensions.")]
	[Localizable(true)]
	public int RightMargin
	{
		get
		{
			return _ᮇ.RightMargin;
		}
		set
		{
			_ᮇ.RightMargin = value;
		}
	}

	[Category("Appearance")]
	[DefaultValue("")]
	[Bindable(true)]
	[SettingsBindable(true)]
	[RefreshProperties(RefreshProperties.All)]
	[Description("Gets or sets the text of the RichTextBox control, including all rich text format (RTF) codes.")]
	public string Rtf
	{
		get
		{
			return _ᮇ.Rtf;
		}
		set
		{
			_ᮇ.Rtf = value;
			UpdateScrollBarsDelayed();
		}
	}

	[Description("Indicates, for multi-line edit control, which scroll bars will be shown.")]
	[Localizable(true)]
	[DefaultValue(typeof(RichTextBoxScrollBars), "Both")]
	[Category("Appearance")]
	public RichTextBoxScrollBars ScrollBars
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _ᮇ.ScrollBars;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_ᮇ.ScrollBars = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue("")]
	public string SelectedRtf
	{
		get
		{
			return _ᮇ.SelectedRtf;
		}
		set
		{
			_ᮇ.SelectedRtf = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string SelectedText
	{
		get
		{
			return ((TextBoxBase)_ᮇ).SelectedText;
		}
		set
		{
			((TextBoxBase)_ᮇ).SelectedText = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue(typeof(HorizontalAlignment), "Left")]
	[Browsable(false)]
	public HorizontalAlignment SelectionAlignment
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _ᮇ.SelectionAlignment;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_ᮇ.SelectionAlignment = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color SelectionBackColor
	{
		get
		{
			return _ᮇ.SelectionBackColor;
		}
		set
		{
			_ᮇ.SelectionBackColor = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool SelectionBullet
	{
		get
		{
			return _ᮇ.SelectionBullet;
		}
		set
		{
			_ᮇ.SelectionBullet = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int SelectionCharOffset
	{
		get
		{
			return _ᮇ.SelectionCharOffset;
		}
		set
		{
			_ᮇ.SelectionCharOffset = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Color SelectionColor
	{
		get
		{
			return _ᮇ.SelectionColor;
		}
		set
		{
			_ᮇ.SelectionColor = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Font SelectionFont
	{
		get
		{
			return _ᮇ.SelectionFont;
		}
		set
		{
			_ᮇ.SelectionFont = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int SelectionHangingIndent
	{
		get
		{
			return _ᮇ.SelectionHangingIndent;
		}
		set
		{
			_ᮇ.SelectionHangingIndent = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int SelectionIndent
	{
		get
		{
			return _ᮇ.SelectionIndent;
		}
		set
		{
			_ᮇ.SelectionIndent = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public int SelectionLength
	{
		get
		{
			return ((TextBoxBase)_ᮇ).SelectionLength;
		}
		set
		{
			((TextBoxBase)_ᮇ).SelectionLength = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int SelectionProtected
	{
		get
		{
			return ((TextBoxBase)_ᮇ).SelectionLength;
		}
		set
		{
			((TextBoxBase)_ᮇ).SelectionLength = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public int SelectionRightIndent
	{
		get
		{
			return _ᮇ.SelectionRightIndent;
		}
		set
		{
			_ᮇ.SelectionRightIndent = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int SelectionStart
	{
		get
		{
			return ((TextBoxBase)_ᮇ).SelectionStart;
		}
		set
		{
			((TextBoxBase)_ᮇ).SelectionStart = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int[] SelectionTabs
	{
		get
		{
			return _ᮇ.SelectionTabs;
		}
		set
		{
			_ᮇ.SelectionTabs = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public RichTextBoxSelectionTypes SelectionType => _ᮇ.SelectionType;

	[Description("Indicates whether shortcuts defined for the control are enabled.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool ShortcutsEnabled
	{
		get
		{
			return ((TextBoxBase)_ᮇ).ShortcutsEnabled;
		}
		set
		{
			((TextBoxBase)_ᮇ).ShortcutsEnabled = value;
		}
	}

	[Category("Behavior")]
	[Description("Indicates whether selection margin is visible.")]
	[DefaultValue(false)]
	public bool ShowSelectionMargin
	{
		get
		{
			return _ᮇ.ShowSelectionMargin;
		}
		set
		{
			_ᮇ.ShowSelectionMargin = value;
		}
	}

	public bool TabStop
	{
		get
		{
			return ((Control)_ᮇ).TabStop;
		}
		set
		{
			((Control)_ᮇ).TabStop = value;
		}
	}

	[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version= 2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	public override string Text
	{
		get
		{
			return ((Control)_ᮇ).Text;
		}
		set
		{
			((Control)_ᮇ).Text = value;
			UpdateScrollBarsDelayed();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int TextLength => ((TextBoxBase)_ᮇ).TextLength;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string UndoActionName => _ᮇ.UndoActionName;

	[Localizable(true)]
	[DefaultValue(true)]
	[Description("Indicates whether lines are automatically word-wrapped.")]
	[Category("Behavior")]
	public bool WordWrap
	{
		get
		{
			return ((TextBoxBase)_ᮇ).WordWrap;
		}
		set
		{
			((TextBoxBase)_ᮇ).WordWrap = value;
		}
	}

	[Category("Behavior")]
	[Description("Indicates current zoom factor for the control content.")]
	[Localizable(true)]
	[DefaultValue(1f)]
	public float ZoomFactor
	{
		get
		{
			return _ᮇ.ZoomFactor;
		}
		set
		{
			_ᮇ.ZoomFactor = value;
		}
	}

	public override ContextMenu ContextMenu
	{
		get
		{
			return ((Control)_ᮇ).ContextMenu;
		}
		set
		{
			((Control)_ᮇ).ContextMenu = value;
		}
	}

	[Description("Occurs when the value of the AcceptsTab property changes.")]
	public event EventHandler AcceptsTabChanged
	{
		add
		{
			EventHandler eventHandler = ᮈ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᮈ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᮈ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᮈ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when the value of the HideSelection property changes.")]
	public event EventHandler HideSelectionChanged
	{
		add
		{
			EventHandler eventHandler = ᮉ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᮉ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᮉ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᮉ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when the value of the Modified property changes.")]
	public event EventHandler ModifiedChanged
	{
		add
		{
			EventHandler eventHandler = ᮊ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᮊ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᮊ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᮊ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when the value of the Multiline property changes.")]
	public event EventHandler MultilineChanged
	{
		add
		{
			EventHandler eventHandler = ᮋ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᮋ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᮋ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᮋ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when the value of the ReadOnly property changes.")]
	public event EventHandler ReadOnlyChanged
	{
		add
		{
			EventHandler eventHandler = ᮌ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᮌ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᮌ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᮌ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when a hyperlink in the text is clicked.")]
	[Category("Behavior")]
	public event LinkClickedEventHandler LinkClicked
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			LinkClickedEventHandler val = ᮍ;
			LinkClickedEventHandler val2;
			do
			{
				val2 = val;
				LinkClickedEventHandler value2 = (LinkClickedEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ᮍ, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			LinkClickedEventHandler val = ᮍ;
			LinkClickedEventHandler val2;
			do
			{
				val2 = val;
				LinkClickedEventHandler value2 = (LinkClickedEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ᮍ, value2, val2);
			}
			while (val != val2);
		}
	}

	[Category("Behavior")]
	[Description("Occurs when the user takes an action that would change a protected range of text.")]
	public event EventHandler Protected
	{
		add
		{
			EventHandler eventHandler = ᮎ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᮎ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᮎ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᮎ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when the current selection has changed.")]
	[Category("Behavior")]
	public event EventHandler SelectionChanged
	{
		add
		{
			EventHandler eventHandler = _05B1;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05B1, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _05B1;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05B1, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Category("Behavior")]
	[Description("Occurs when the horizontal scroll bar is clicked.")]
	public event EventHandler HScroll
	{
		add
		{
			EventHandler eventHandler = ᮏ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᮏ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᮏ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᮏ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Category("Behavior")]
	[Description("Occurs when the vertical scroll bar is clicked.")]
	public event EventHandler VScroll
	{
		add
		{
			EventHandler eventHandler = ᮐ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᮐ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᮐ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᮐ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public RichTextBoxEx()
	{
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		_᮱ _᮱2 = new _᮱();
		((Control)this).SetStyle((ControlStyles)512, false);
		((TextBoxBase)_᮱2).AcceptsTabChanged += ᮓ;
		((Control)_᮱2).TextChanged += ᮔ;
		((TextBoxBase)_᮱2).HideSelectionChanged += ᮕ;
		((TextBoxBase)_᮱2).ModifiedChanged += ᮖ;
		((TextBoxBase)_᮱2).MultilineChanged += ᮗ;
		((TextBoxBase)_᮱2).ReadOnlyChanged += ᮘ;
		((Control)_᮱2).GotFocus += ᮙ;
		((Control)_᮱2).LostFocus += ᮚ;
		((Control)_᮱2).KeyDown += new KeyEventHandler(ᮛ);
		((Control)_᮱2).KeyUp += new KeyEventHandler(ᮜ);
		((Control)_᮱2).KeyPress += new KeyPressEventHandler(ᮝ);
		((Control)_᮱2).PreviewKeyDown += new PreviewKeyDownEventHandler(ᮞ);
		((RichTextBox)_᮱2).LinkClicked += new LinkClickedEventHandler(ᮟ);
		((RichTextBox)_᮱2).Protected += ᮠ;
		((RichTextBox)_᮱2).SelectionChanged += _1BA1;
		((RichTextBox)_᮱2).HScroll += _1BA2;
		((RichTextBox)_᮱2).VScroll += _1BA3;
		((Control)_᮱2).Validating += _1BA4;
		((Control)_᮱2).Validated += _1BA5;
		RichTextBox = (RichTextBox)(object)_᮱2;
		((TextBoxBase)_᮱2).BorderStyle = (BorderStyle)0;
		base._1BA6 = (Control)(object)_᮱2;
	}

	private void ᮓ(object _٠, EventArgs ؿ)
	{
		OnAcceptsTabChanged(ؿ);
	}

	protected virtual void OnAcceptsTabChanged(EventArgs e)
	{
		ᮈ?.Invoke(this, e);
	}

	private void ᮔ(object _٠, EventArgs ؿ)
	{
		((Control)this).OnTextChanged(ؿ);
	}

	private void ᮕ(object _٠, EventArgs ؿ)
	{
		OnHideSelectionChanged(ؿ);
	}

	protected virtual void OnHideSelectionChanged(EventArgs e)
	{
		ᮉ?.Invoke(this, e);
	}

	private void ᮖ(object _٠, EventArgs ؿ)
	{
		OnModifiedChanged(ؿ);
	}

	protected virtual void OnModifiedChanged(EventArgs e)
	{
		ᮊ?.Invoke(this, e);
	}

	private void ᮗ(object _٠, EventArgs ؿ)
	{
		OnMultilineChanged(ؿ);
	}

	protected virtual void OnMultilineChanged(EventArgs e)
	{
		ᮋ?.Invoke(this, e);
	}

	private void ᮘ(object _٠, EventArgs ؿ)
	{
		OnReadOnlyChanged(ؿ);
	}

	protected virtual void OnReadOnlyChanged(EventArgs e)
	{
		ᮌ?.Invoke(this, e);
	}

	private void ᮙ(object _٠, EventArgs ؿ)
	{
		((Control)this).OnGotFocus(ؿ);
	}

	private void ᮚ(object _٠, EventArgs ؿ)
	{
		((Control)this).OnLostFocus(ؿ);
	}

	private void ᮛ(object _٠, KeyEventArgs ؿ)
	{
		((Control)this).OnKeyDown(ؿ);
	}

	private void ᮜ(object _٠, KeyEventArgs ؿ)
	{
		((Control)this).OnKeyUp(ؿ);
	}

	private void ᮝ(object _٠, KeyPressEventArgs ؿ)
	{
		((Control)this).OnKeyPress(ؿ);
	}

	private void ᮞ(object _٠, PreviewKeyDownEventArgs ؿ)
	{
		((Control)this).OnPreviewKeyDown(ؿ);
	}

	private void ᮟ(object _٠, LinkClickedEventArgs ؿ)
	{
		OnLinkClicked(ؿ);
	}

	protected virtual void OnLinkClicked(LinkClickedEventArgs e)
	{
		LinkClickedEventHandler val = ᮍ;
		if (val != null)
		{
			val.Invoke((object)this, e);
		}
	}

	private void ᮠ(object _٠, EventArgs ؿ)
	{
		OnProtected(ؿ);
	}

	protected virtual void OnProtected(EventArgs e)
	{
		ᮎ?.Invoke(this, e);
	}

	private void _1BA1(object _٠, EventArgs ؿ)
	{
		OnSelectionChanged(ؿ);
	}

	protected virtual void OnSelectionChanged(EventArgs e)
	{
		_05B1?.Invoke(this, e);
	}

	private void _1BA2(object _٠, EventArgs ؿ)
	{
		OnHScroll(ؿ);
	}

	protected virtual void OnHScroll(EventArgs e)
	{
		ᮏ?.Invoke(this, e);
	}

	private void _1BA3(object _٠, EventArgs ؿ)
	{
		OnVScroll(ؿ);
	}

	protected virtual void OnVScroll(EventArgs e)
	{
		ᮐ?.Invoke(this, e);
	}

	private void _1BA4(object _٠, CancelEventArgs ؿ)
	{
		((Control)this).OnValidating(ؿ);
	}

	private void _1BA5(object _٠, EventArgs ؿ)
	{
		((Control)this).OnValidated(ؿ);
	}

	protected override void OnForeColorChanged(EventArgs e)
	{
		((Control)_ᮇ).ForeColor = ((Control)this).ForeColor;
		((Control)this).OnForeColorChanged(e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColorRichTextBox()
	{
		return !_ᮑ.Equals((object?)SystemColors.Window);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackColorRichTextBox()
	{
		BackColorRichTextBox = SystemColors.Window;
	}

	public void AppendText(string text)
	{
		((TextBoxBase)_ᮇ).AppendText(text);
		UpdateScrollBarsDelayed();
	}

	public bool CanPaste(Format clipFormat)
	{
		return _ᮇ.CanPaste(clipFormat);
	}

	public void Clear()
	{
		((TextBoxBase)_ᮇ).Clear();
		UpdateScrollBarsDelayed();
	}

	public void ClearUndo()
	{
		((TextBoxBase)_ᮇ).ClearUndo();
		UpdateScrollBarsDelayed();
	}

	public void Copy()
	{
		((TextBoxBase)_ᮇ).Copy();
	}

	public void Cut()
	{
		((TextBoxBase)_ᮇ).Cut();
		UpdateScrollBarsDelayed();
	}

	public void DeselectAll()
	{
		((TextBoxBase)_ᮇ).DeselectAll();
	}

	public int Find(string str)
	{
		return _ᮇ.Find(str);
	}

	public int Find(char[] characterSet)
	{
		return _ᮇ.Find(characterSet);
	}

	public int Find(string str, RichTextBoxFinds options)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return _ᮇ.Find(str, options);
	}

	public int Find(char[] characterSet, int start)
	{
		return _ᮇ.Find(characterSet, start);
	}

	public int Find(string str, int start, RichTextBoxFinds options)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return _ᮇ.Find(str, start, options);
	}

	public int Find(char[] characterSet, int start, int end)
	{
		return _ᮇ.Find(characterSet, start, end);
	}

	public int Find(string str, int start, int end, RichTextBoxFinds options)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		return _ᮇ.Find(str, start, end, options);
	}

	public bool Focus()
	{
		if (RichTextBox != null)
		{
			return ((Control)RichTextBox).Focus();
		}
		return false;
	}

	public int GetCharFromPosition(Point pt)
	{
		return ((TextBoxBase)_ᮇ).GetCharFromPosition(pt);
	}

	public int GetCharIndexFromPosition(Point pt)
	{
		return ((TextBoxBase)_ᮇ).GetCharIndexFromPosition(pt);
	}

	public int GetFirstCharIndexFromLine(int lineNumber)
	{
		return ((TextBoxBase)_ᮇ).GetFirstCharIndexFromLine(lineNumber);
	}

	public int GetFirstCharIndexOfCurrentLine()
	{
		return ((TextBoxBase)_ᮇ).GetFirstCharIndexOfCurrentLine();
	}

	public int GetLineFromCharIndex(int index)
	{
		return ((TextBoxBase)_ᮇ).GetLineFromCharIndex(index);
	}

	public Point GetPositionFromCharIndex(int index)
	{
		return ((TextBoxBase)_ᮇ).GetPositionFromCharIndex(index);
	}

	public void LoadFile(string path)
	{
		_ᮇ.LoadFile(path);
		UpdateScrollBarsDelayed();
	}

	public void LoadFile(Stream data, RichTextBoxStreamType fileType)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		_ᮇ.LoadFile(data, fileType);
		UpdateScrollBarsDelayed();
	}

	public void LoadFile(string path, RichTextBoxStreamType fileType)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		_ᮇ.LoadFile(path, fileType);
		UpdateScrollBarsDelayed();
	}

	public void Paste()
	{
		((TextBoxBase)_ᮇ).Paste();
		UpdateScrollBarsDelayed();
	}

	public void Paste(Format clipFormat)
	{
		_ᮇ.Paste(clipFormat);
		UpdateScrollBarsDelayed();
	}

	public void Redo()
	{
		_ᮇ.Redo();
		UpdateScrollBarsDelayed();
	}

	public void SaveFile(string path)
	{
		_ᮇ.SaveFile(path);
	}

	public void SaveFile(Stream data, RichTextBoxStreamType fileType)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		_ᮇ.SaveFile(data, fileType);
	}

	public void SaveFile(string path, RichTextBoxStreamType fileType)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		_ᮇ.SaveFile(path, fileType);
	}

	public void ScrollToCaret()
	{
		((TextBoxBase)_ᮇ).ScrollToCaret();
	}

	public void Select()
	{
		((Control)_ᮇ).Select();
	}

	public void Select(int start, int length)
	{
		((TextBoxBase)_ᮇ).Select(start, length);
	}

	public void SelectAll()
	{
		((TextBoxBase)_ᮇ).SelectAll();
	}

	public void Undo()
	{
		((TextBoxBase)_ᮇ).Undo();
		UpdateScrollBarsDelayed();
	}
}
