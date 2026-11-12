using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxItem(false)]
[Docking(/*Could not decode attribute arguments.*/)]
[Description("Provides rich-text, RTF text editing.")]
[ToolboxBitmap(typeof(RichTextBoxEx), "Controls.RichTextBoxEx.ico")]
[DefaultEvent("TextChanged")]
[DefaultProperty("Text")]
[DefaultBindingProperty("Text")]
public class RichTextBoxEx : ScrollbarControl
{
	[CompilerGenerated]
	private LinkClickedEventHandler m_LinkClicked;

	private Color _BackColorRichTextBox = SystemColors.Window;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public RichTextBox RichTextBox { get; internal set; }

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	[Browsable(false)]
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
			return _BackColorRichTextBox;
		}
		set
		{
			_BackColorRichTextBox = value;
			((Control)RichTextBox).BackColor = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public BorderStyle BorderStyle { get; set; }

	[DefaultValue(false)]
	[Description("Indicates whether tab characters are accepted as input.")]
	[Category("Behavior")]
	public bool AcceptsTab
	{
		get
		{
			return ((TextBoxBase)RichTextBox).AcceptsTab;
		}
		set
		{
			((TextBoxBase)RichTextBox).AcceptsTab = value;
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

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Indicates whether automatic word selection is enabled.")]
	public bool AutoWordSelection
	{
		get
		{
			return RichTextBox.AutoWordSelection;
		}
		set
		{
			RichTextBox.AutoWordSelection = value;
		}
	}

	[Bindable(false)]
	[Browsable(false)]
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
	[Category("Behavior")]
	[Description("Indicates the indent for bullets in the control.")]
	[Localizable(true)]
	public int BulletIndent
	{
		get
		{
			return RichTextBox.BulletIndent;
		}
		set
		{
			RichTextBox.BulletIndent = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool CanRedo => RichTextBox.CanRedo;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool CanUndo => ((TextBoxBase)RichTextBox).CanUndo;

	public override ContextMenuStrip ContextMenuStrip
	{
		get
		{
			return ((Control)this).ContextMenuStrip;
		}
		set
		{
			((Control)this).ContextMenuStrip = value;
			((Control)RichTextBox).ContextMenuStrip = value;
		}
	}

	protected override Size DefaultSize => new Size(100, 100);

	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Indicates whether URLs are automatically formatted as links.")]
	public bool DetectUrls
	{
		get
		{
			return RichTextBox.DetectUrls;
		}
		set
		{
			RichTextBox.DetectUrls = value;
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether drag/drop of text, pictures and other data is enabled.")]
	public bool EnableAutoDragDrop
	{
		get
		{
			return RichTextBox.EnableAutoDragDrop;
		}
		set
		{
			RichTextBox.EnableAutoDragDrop = value;
		}
	}

	[Browsable(false)]
	public override bool Focused => ((Control)RichTextBox).Focused;

	[Description("Indicates whether selection should be hidden when the edit control loses focus.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool HideSelection
	{
		get
		{
			return ((TextBoxBase)RichTextBox).HideSelection;
		}
		set
		{
			((TextBoxBase)RichTextBox).HideSelection = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public RichTextBoxLanguageOptions LanguageOption
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return RichTextBox.LanguageOption;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			RichTextBox.LanguageOption = value;
		}
	}

	[Editor("System.Windows.Forms.Design.StringArrayEditor, System.Design, Version=10.9.0.8, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[MergableProperty(false)]
	[Localizable(true)]
	[Category("Appearance")]
	[Description("Indicates lines of text in a multi-line edit, as an array of String values.")]
	public string[] Lines
	{
		get
		{
			return ((TextBoxBase)RichTextBox).Lines;
		}
		set
		{
			((TextBoxBase)RichTextBox).Lines = value;
		}
	}

	[Localizable(true)]
	[Category("Behavior")]
	[Description("Indicates maximum number of characters that can be entered into the edit control.")]
	[DefaultValue(int.MaxValue)]
	public int MaxLength
	{
		get
		{
			return ((TextBoxBase)RichTextBox).MaxLength;
		}
		set
		{
			((TextBoxBase)RichTextBox).MaxLength = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool Modified => ((TextBoxBase)RichTextBox).Modified;

	[RefreshProperties(RefreshProperties.All)]
	[Description("Indicates whether the text in the control can span more than one line.")]
	[DefaultValue(true)]
	[Localizable(true)]
	[Category("Behavior")]
	public bool Multiline
	{
		get
		{
			return ((TextBoxBase)RichTextBox).Multiline;
		}
		set
		{
			((TextBoxBase)RichTextBox).Multiline = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Localizable(false)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether the text in the edit control can be changed or not.")]
	[RefreshProperties(RefreshProperties.Repaint)]
	public bool ReadOnly
	{
		get
		{
			return ((TextBoxBase)RichTextBox).ReadOnly;
		}
		set
		{
			((TextBoxBase)RichTextBox).ReadOnly = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public string RedoActionName => RichTextBox.RedoActionName;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[DefaultValue(true)]
	public bool RichTextShortcutsEnabled
	{
		get
		{
			return RichTextBox.RichTextShortcutsEnabled;
		}
		set
		{
			RichTextBox.RichTextShortcutsEnabled = value;
		}
	}

	[Description("Indicates the right margin dimensions.")]
	[Localizable(true)]
	[Category("Behavior")]
	[DefaultValue(0)]
	public int RightMargin
	{
		get
		{
			return RichTextBox.RightMargin;
		}
		set
		{
			RichTextBox.RightMargin = value;
		}
	}

	[RefreshProperties(RefreshProperties.All)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public string Rtf
	{
		get
		{
			return RichTextBox.Rtf;
		}
		set
		{
			RichTextBox.Rtf = value;
			UpdateScrollBarsDelayed();
		}
	}

	[Localizable(true)]
	[Description("Indicates, for multi-line edit control, which scroll bars will be shown.")]
	[DefaultValue(typeof(RichTextBoxScrollBars), "Both")]
	[Category("Appearance")]
	public RichTextBoxScrollBars ScrollBars
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return RichTextBox.ScrollBars;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			RichTextBox.ScrollBars = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue("")]
	public string SelectedRtf
	{
		get
		{
			return RichTextBox.SelectedRtf;
		}
		set
		{
			RichTextBox.SelectedRtf = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public string SelectedText
	{
		get
		{
			return ((TextBoxBase)RichTextBox).SelectedText;
		}
		set
		{
			((TextBoxBase)RichTextBox).SelectedText = value;
		}
	}

	[DefaultValue(typeof(HorizontalAlignment), "Left")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public HorizontalAlignment SelectionAlignment
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return RichTextBox.SelectionAlignment;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			RichTextBox.SelectionAlignment = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Color SelectionBackColor
	{
		get
		{
			return RichTextBox.SelectionBackColor;
		}
		set
		{
			RichTextBox.SelectionBackColor = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool SelectionBullet
	{
		get
		{
			return RichTextBox.SelectionBullet;
		}
		set
		{
			RichTextBox.SelectionBullet = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int SelectionCharOffset
	{
		get
		{
			return RichTextBox.SelectionCharOffset;
		}
		set
		{
			RichTextBox.SelectionCharOffset = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color SelectionColor
	{
		get
		{
			return RichTextBox.SelectionColor;
		}
		set
		{
			RichTextBox.SelectionColor = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Font SelectionFont
	{
		get
		{
			return RichTextBox.SelectionFont;
		}
		set
		{
			RichTextBox.SelectionFont = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public int SelectionHangingIndent
	{
		get
		{
			return RichTextBox.SelectionHangingIndent;
		}
		set
		{
			RichTextBox.SelectionHangingIndent = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public int SelectionIndent
	{
		get
		{
			return RichTextBox.SelectionIndent;
		}
		set
		{
			RichTextBox.SelectionIndent = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public int SelectionLength
	{
		get
		{
			return ((TextBoxBase)RichTextBox).SelectionLength;
		}
		set
		{
			((TextBoxBase)RichTextBox).SelectionLength = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public int SelectionProtected
	{
		get
		{
			return ((TextBoxBase)RichTextBox).SelectionLength;
		}
		set
		{
			((TextBoxBase)RichTextBox).SelectionLength = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int SelectionRightIndent
	{
		get
		{
			return RichTextBox.SelectionRightIndent;
		}
		set
		{
			RichTextBox.SelectionRightIndent = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public int SelectionStart
	{
		get
		{
			return ((TextBoxBase)RichTextBox).SelectionStart;
		}
		set
		{
			((TextBoxBase)RichTextBox).SelectionStart = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int[] SelectionTabs
	{
		get
		{
			return RichTextBox.SelectionTabs;
		}
		set
		{
			RichTextBox.SelectionTabs = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public RichTextBoxSelectionTypes SelectionType => RichTextBox.SelectionType;

	[DefaultValue(true)]
	[Description("Indicates whether shortcuts defined for the control are enabled.")]
	[Category("Behavior")]
	public bool ShortcutsEnabled
	{
		get
		{
			return ((TextBoxBase)RichTextBox).ShortcutsEnabled;
		}
		set
		{
			((TextBoxBase)RichTextBox).ShortcutsEnabled = value;
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether selection margin is visible.")]
	public bool ShowSelectionMargin
	{
		get
		{
			return RichTextBox.ShowSelectionMargin;
		}
		set
		{
			RichTextBox.ShowSelectionMargin = value;
		}
	}

	public bool TabStop
	{
		get
		{
			return ((Control)RichTextBox).TabStop;
		}
		set
		{
			((Control)RichTextBox).TabStop = value;
		}
	}

	[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=10.9.0.8, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	public override string Text
	{
		get
		{
			return ((Control)RichTextBox).Text;
		}
		set
		{
			((Control)RichTextBox).Text = value;
			UpdateScrollBarsDelayed();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public int TextLength => ((TextBoxBase)RichTextBox).TextLength;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public string UndoActionName => RichTextBox.UndoActionName;

	[DefaultValue(true)]
	[Description("Indicates whether lines are automatically word-wrapped.")]
	[Localizable(true)]
	[Category("Behavior")]
	public bool WordWrap
	{
		get
		{
			return ((TextBoxBase)RichTextBox).WordWrap;
		}
		set
		{
			((TextBoxBase)RichTextBox).WordWrap = value;
		}
	}

	[Localizable(true)]
	[DefaultValue(1f)]
	[Category("Behavior")]
	[Description("Indicates current zoom factor for the control content.")]
	public float ZoomFactor
	{
		get
		{
			return RichTextBox.ZoomFactor;
		}
		set
		{
			RichTextBox.ZoomFactor = value;
		}
	}

	[Description("Occurs when the value of the AcceptsTab property changes.")]
	public event EventHandler AcceptsTabChanged;

	[Description("Occurs when the value of the HideSelection property changes.")]
	public event EventHandler HideSelectionChanged;

	[Description("Occurs when the value of the Modified property changes.")]
	public event EventHandler ModifiedChanged;

	[Description("Occurs when the value of the Multiline property changes.")]
	public event EventHandler MultilineChanged;

	[Description("Occurs when the value of the ReadOnly property changes.")]
	public event EventHandler ReadOnlyChanged;

	[Description("Occurs when a hyperlink in the text is clicked.")]
	[Category("Behavior")]
	public event LinkClickedEventHandler LinkClicked
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			LinkClickedEventHandler val = this.m_LinkClicked;
			LinkClickedEventHandler val2;
			do
			{
				val2 = val;
				LinkClickedEventHandler value2 = (LinkClickedEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_LinkClicked, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			LinkClickedEventHandler val = this.m_LinkClicked;
			LinkClickedEventHandler val2;
			do
			{
				val2 = val;
				LinkClickedEventHandler value2 = (LinkClickedEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_LinkClicked, value2, val2);
			}
			while (val != val2);
		}
	}

	[Category("Behavior")]
	[Description("Occurs when the user takes an action that would change a protected range of text.")]
	public event EventHandler Protected;

	[Category("Behavior")]
	[Description("Occurs when the current selection has changed.")]
	public event EventHandler SelectionChanged;

	[Description("Occurs when the horizontal scroll bar is clicked.")]
	[Category("Behavior")]
	public event EventHandler HScroll;

	[Description("Occurs when the vertical scroll bar is clicked.")]
	[Category("Behavior")]
	public event EventHandler VScroll;

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
		RichTextBoxScrollEx richTextBoxScrollEx = new RichTextBoxScrollEx();
		((Control)this).SetStyle((ControlStyles)512, false);
		((TextBoxBase)richTextBoxScrollEx).AcceptsTabChanged += OnRichTextBoxAcceptsTabChanged;
		((Control)richTextBoxScrollEx).TextChanged += OnRichTextBoxTextChanged;
		((TextBoxBase)richTextBoxScrollEx).HideSelectionChanged += OnRichTextBoxHideSelectionChanged;
		((TextBoxBase)richTextBoxScrollEx).ModifiedChanged += OnRichTextBoxModifiedChanged;
		((TextBoxBase)richTextBoxScrollEx).MultilineChanged += OnRichTextBoxMultilineChanged;
		((TextBoxBase)richTextBoxScrollEx).ReadOnlyChanged += OnRichTextBoxReadOnlyChanged;
		((Control)richTextBoxScrollEx).GotFocus += OnRichTextBoxGotFocus;
		((Control)richTextBoxScrollEx).LostFocus += OnRichTextBoxLostFocus;
		((Control)richTextBoxScrollEx).KeyDown += new KeyEventHandler(OnRichTextBoxKeyDown);
		((Control)richTextBoxScrollEx).KeyUp += new KeyEventHandler(OnRichTextBoxKeyUp);
		((Control)richTextBoxScrollEx).KeyPress += new KeyPressEventHandler(OnRichTextBoxKeyPress);
		((Control)richTextBoxScrollEx).PreviewKeyDown += new PreviewKeyDownEventHandler(OnRichTextBoxPreviewKeyDown);
		((RichTextBox)richTextBoxScrollEx).LinkClicked += new LinkClickedEventHandler(OnRichTextBoxLinkClicked);
		((RichTextBox)richTextBoxScrollEx).Protected += OnRichTextBoxProtected;
		((RichTextBox)richTextBoxScrollEx).SelectionChanged += OnRichTextBoxSelectionChanged;
		((RichTextBox)richTextBoxScrollEx).HScroll += OnRichTextBoxHScroll;
		((RichTextBox)richTextBoxScrollEx).VScroll += OnRichTextBoxVScroll;
		((Control)richTextBoxScrollEx).Validating += OnRichTextBoxValidating;
		((Control)richTextBoxScrollEx).Validated += OnRichTextBoxValidated;
		RichTextBox = (RichTextBox)(object)richTextBoxScrollEx;
		((TextBoxBase)richTextBoxScrollEx).BorderStyle = (BorderStyle)0;
		base.ScrollOverrideControl = (Control)(object)richTextBoxScrollEx;
	}

	private void OnRichTextBoxAcceptsTabChanged(object sender, EventArgs e)
	{
		OnAcceptsTabChanged(e);
	}

	protected virtual void OnAcceptsTabChanged(EventArgs e)
	{
		this.AcceptsTabChanged?.Invoke(this, e);
	}

	private void OnRichTextBoxTextChanged(object sender, EventArgs e)
	{
		((Control)this).OnTextChanged(e);
	}

	private void OnRichTextBoxHideSelectionChanged(object sender, EventArgs e)
	{
		OnHideSelectionChanged(e);
	}

	protected virtual void OnHideSelectionChanged(EventArgs e)
	{
		this.HideSelectionChanged?.Invoke(this, e);
	}

	private void OnRichTextBoxModifiedChanged(object sender, EventArgs e)
	{
		OnModifiedChanged(e);
	}

	protected virtual void OnModifiedChanged(EventArgs e)
	{
		this.ModifiedChanged?.Invoke(this, e);
	}

	private void OnRichTextBoxMultilineChanged(object sender, EventArgs e)
	{
		OnMultilineChanged(e);
	}

	protected virtual void OnMultilineChanged(EventArgs e)
	{
		this.MultilineChanged?.Invoke(this, e);
	}

	private void OnRichTextBoxReadOnlyChanged(object sender, EventArgs e)
	{
		OnReadOnlyChanged(e);
	}

	protected virtual void OnReadOnlyChanged(EventArgs e)
	{
		this.ReadOnlyChanged?.Invoke(this, e);
	}

	private void OnRichTextBoxGotFocus(object sender, EventArgs e)
	{
		((Control)this).OnGotFocus(e);
	}

	private void OnRichTextBoxLostFocus(object sender, EventArgs e)
	{
		((Control)this).OnLostFocus(e);
	}

	private void OnRichTextBoxKeyDown(object sender, KeyEventArgs e)
	{
		((Control)this).OnKeyDown(e);
	}

	private void OnRichTextBoxKeyUp(object sender, KeyEventArgs e)
	{
		((Control)this).OnKeyUp(e);
	}

	private void OnRichTextBoxKeyPress(object sender, KeyPressEventArgs e)
	{
		((Control)this).OnKeyPress(e);
	}

	private void OnRichTextBoxPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
	{
		((Control)this).OnPreviewKeyDown(e);
	}

	private void OnRichTextBoxLinkClicked(object sender, LinkClickedEventArgs e)
	{
		OnLinkClicked(e);
	}

	protected virtual void OnLinkClicked(LinkClickedEventArgs e)
	{
		LinkClickedEventHandler linkClicked = this.LinkClicked;
		if (linkClicked != null)
		{
			linkClicked.Invoke((object)this, e);
		}
	}

	private void OnRichTextBoxProtected(object sender, EventArgs e)
	{
		OnProtected(e);
	}

	protected virtual void OnProtected(EventArgs e)
	{
		this.Protected?.Invoke(this, e);
	}

	private void OnRichTextBoxSelectionChanged(object sender, EventArgs e)
	{
		OnSelectionChanged(e);
	}

	protected virtual void OnSelectionChanged(EventArgs e)
	{
		this.SelectionChanged?.Invoke(this, e);
	}

	private void OnRichTextBoxHScroll(object sender, EventArgs e)
	{
		OnHScroll(e);
	}

	protected virtual void OnHScroll(EventArgs e)
	{
		this.HScroll?.Invoke(this, e);
	}

	private void OnRichTextBoxVScroll(object sender, EventArgs e)
	{
		OnVScroll(e);
	}

	protected virtual void OnVScroll(EventArgs e)
	{
		this.VScroll?.Invoke(this, e);
	}

	private void OnRichTextBoxValidating(object sender, CancelEventArgs e)
	{
		((Control)this).OnValidating(e);
	}

	private void OnRichTextBoxValidated(object sender, EventArgs e)
	{
		((Control)this).OnValidated(e);
	}

	protected override void OnForeColorChanged(EventArgs e)
	{
		((Control)RichTextBox).ForeColor = ((Control)this).ForeColor;
		((Control)this).OnForeColorChanged(e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColorRichTextBox()
	{
		return !_BackColorRichTextBox.Equals((object?)SystemColors.Window);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackColorRichTextBox()
	{
		BackColorRichTextBox = SystemColors.Window;
	}

	public void AppendText(string text)
	{
		((TextBoxBase)RichTextBox).AppendText(text);
		UpdateScrollBarsDelayed();
	}

	public bool CanPaste(Format clipFormat)
	{
		return RichTextBox.CanPaste(clipFormat);
	}

	public void Clear()
	{
		((TextBoxBase)RichTextBox).Clear();
		UpdateScrollBarsDelayed();
	}

	public void ClearUndo()
	{
		((TextBoxBase)RichTextBox).ClearUndo();
		UpdateScrollBarsDelayed();
	}

	public void Copy()
	{
		((TextBoxBase)RichTextBox).Copy();
	}

	public void Cut()
	{
		((TextBoxBase)RichTextBox).Cut();
		UpdateScrollBarsDelayed();
	}

	public void DeselectAll()
	{
		((TextBoxBase)RichTextBox).DeselectAll();
	}

	public int Find(string str)
	{
		return RichTextBox.Find(str);
	}

	public int Find(char[] characterSet)
	{
		return RichTextBox.Find(characterSet);
	}

	public int Find(string str, RichTextBoxFinds options)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return RichTextBox.Find(str, options);
	}

	public int Find(char[] characterSet, int start)
	{
		return RichTextBox.Find(characterSet, start);
	}

	public int Find(string str, int start, RichTextBoxFinds options)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return RichTextBox.Find(str, start, options);
	}

	public int Find(char[] characterSet, int start, int end)
	{
		return RichTextBox.Find(characterSet, start, end);
	}

	public int Find(string str, int start, int end, RichTextBoxFinds options)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		return RichTextBox.Find(str, start, end, options);
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
		return ((TextBoxBase)RichTextBox).GetCharFromPosition(pt);
	}

	public int GetCharIndexFromPosition(Point pt)
	{
		return ((TextBoxBase)RichTextBox).GetCharIndexFromPosition(pt);
	}

	public int GetFirstCharIndexFromLine(int lineNumber)
	{
		return ((TextBoxBase)RichTextBox).GetFirstCharIndexFromLine(lineNumber);
	}

	public int GetFirstCharIndexOfCurrentLine()
	{
		return ((TextBoxBase)RichTextBox).GetFirstCharIndexOfCurrentLine();
	}

	public int GetLineFromCharIndex(int index)
	{
		return ((TextBoxBase)RichTextBox).GetLineFromCharIndex(index);
	}

	public Point GetPositionFromCharIndex(int index)
	{
		return ((TextBoxBase)RichTextBox).GetPositionFromCharIndex(index);
	}

	public void LoadFile(string path)
	{
		RichTextBox.LoadFile(path);
		UpdateScrollBarsDelayed();
	}

	public void LoadFile(Stream data, RichTextBoxStreamType fileType)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		RichTextBox.LoadFile(data, fileType);
		UpdateScrollBarsDelayed();
	}

	public void LoadFile(string path, RichTextBoxStreamType fileType)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		RichTextBox.LoadFile(path, fileType);
		UpdateScrollBarsDelayed();
	}

	public void Paste()
	{
		((TextBoxBase)RichTextBox).Paste();
		UpdateScrollBarsDelayed();
	}

	public void Paste(Format clipFormat)
	{
		RichTextBox.Paste(clipFormat);
		UpdateScrollBarsDelayed();
	}

	public void Redo()
	{
		RichTextBox.Redo();
		UpdateScrollBarsDelayed();
	}

	public void SaveFile(string path)
	{
		RichTextBox.SaveFile(path);
	}

	public void SaveFile(Stream data, RichTextBoxStreamType fileType)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		RichTextBox.SaveFile(data, fileType);
	}

	public void SaveFile(string path, RichTextBoxStreamType fileType)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		RichTextBox.SaveFile(path, fileType);
	}

	public void ScrollToCaret()
	{
		((TextBoxBase)RichTextBox).ScrollToCaret();
	}

	public void Select()
	{
		((Control)RichTextBox).Select();
	}

	public void Select(int start, int length)
	{
		((TextBoxBase)RichTextBox).Select(start, length);
	}

	public void SelectAll()
	{
		((TextBoxBase)RichTextBox).SelectAll();
	}

	public void Undo()
	{
		((TextBoxBase)RichTextBox).Undo();
		UpdateScrollBarsDelayed();
	}
}
