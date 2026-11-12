using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxBitmap(typeof(LabelX), "Controls.LabelX.ico")]
[ToolboxItem(false)]
[ComVisible(false)]
public class LabelX : BaseItemControl, ICommandSource
{
	private LabelItem m_Label;

	private bool m_UseMnemonic = true;

	private Size m_PreferredSize = Size.Empty;

	private ICommand _Command;

	private object _CommandParameter;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	protected string PlainText => m_Label.PlainText;

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text-markup support is enabled for controls Text property.")]
	public bool EnableMarkup
	{
		get
		{
			return m_Label.EnableMarkup;
		}
		set
		{
			m_Label.EnableMarkup = value;
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether control displays focus cues when focused.")]
	public override bool FocusCuesEnabled
	{
		get
		{
			return base.FocusCuesEnabled;
		}
		set
		{
			base.FocusCuesEnabled = value;
		}
	}

	[Browsable(false)]
	[Category("Appearance")]
	[DefaultValue(eBorderSide.All)]
	[Description("Specifies border sides that are displayed.")]
	public eBorderSide BorderSide
	{
		get
		{
			return m_Label.BorderSide;
		}
		set
		{
			m_Label.BorderSide = value;
			InvalidateAutoSize();
		}
	}

	[Browsable(false)]
	[Category("Appearance")]
	[DefaultValue(eBorderType.None)]
	[Description("Indicates the type of the border drawn around the label.")]
	public eBorderType BorderType
	{
		get
		{
			return m_Label.BorderType;
		}
		set
		{
			m_Label.BorderType = value;
			InvalidateAutoSize();
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("The image that will be displayed on the face of the item.")]
	[DefaultValue(null)]
	public Image Image
	{
		get
		{
			return m_Label.Image;
		}
		set
		{
			m_Label.Image = value;
			InvalidateAutoSize();
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("The alignment of the image in relation to text displayed by this item.")]
	[DefaultValue(eImagePosition.Left)]
	public eImagePosition ImagePosition
	{
		get
		{
			return m_Label.ImagePosition;
		}
		set
		{
			m_Label.ImagePosition = value;
			InvalidateAutoSize();
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates border line color when border is single line.")]
	public Color SingleLineColor
	{
		get
		{
			return m_Label.SingleLineColor;
		}
		set
		{
			m_Label.SingleLineColor = value;
		}
	}

	[Browsable(true)]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.TextMarkupUIEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Category("Appearance")]
	[Description("The text contained in the item.")]
	public override string Text
	{
		get
		{
			return ((Control)this).Text;
		}
		set
		{
			((Control)this).Text = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates text alignment.")]
	public StringAlignment TextAlignment
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return m_Label.TextAlignment;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			m_Label.TextAlignment = value;
			RecalcLayout();
		}
	}

	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates text line alignment.")]
	public StringAlignment TextLineAlignment
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return m_Label.TextLineAlignment;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			m_Label.TextLineAlignment = value;
			RecalcLayout();
		}
	}

	[Browsable(true)]
	[Category("Style")]
	[DefaultValue(false)]
	[Description("Gets or sets a value that determines whether text is displayed in multiple lines or one long line.")]
	public bool WordWrap
	{
		get
		{
			return m_Label.WordWrap;
		}
		set
		{
			m_Label.WordWrap = value;
			RecalcLayout();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool TabStop
	{
		get
		{
			return ((Control)this).TabStop;
		}
		set
		{
			((Control)this).TabStop = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Layout")]
	[Description("Indicates left padding in pixels.")]
	public int PaddingLeft
	{
		get
		{
			return m_Label.PaddingLeft;
		}
		set
		{
			m_Label.PaddingLeft = value;
			InvalidateAutoSize();
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Layout")]
	[Description("Indicates right padding in pixels.")]
	public int PaddingRight
	{
		get
		{
			return m_Label.PaddingRight;
		}
		set
		{
			m_Label.PaddingRight = value;
			InvalidateAutoSize();
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Layout")]
	[Description("Indicates top padding in pixels.")]
	public int PaddingTop
	{
		get
		{
			return m_Label.PaddingTop;
		}
		set
		{
			m_Label.PaddingTop = value;
			InvalidateAutoSize();
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Layout")]
	[Description("Indicates bottom padding in pixels.")]
	public int PaddingBottom
	{
		get
		{
			return m_Label.PaddingBottom;
		}
		set
		{
			m_Label.PaddingBottom = value;
			InvalidateAutoSize();
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether the control interprets an ampersand character (&) in the control's Text property to be an access key prefix character.")]
	public bool UseMnemonic
	{
		get
		{
			return m_UseMnemonic;
		}
		set
		{
			m_UseMnemonic = value;
			InvalidateAutoSize();
			((Control)this).Invalidate();
		}
	}

	[Localizable(true)]
	[Browsable(false)]
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

	internal bool IsUsingTextMarkup
	{
		get
		{
			if (m_Label != null)
			{
				return m_Label.TextMarkupBody != null;
			}
			return false;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public override bool AutoSize
	{
		get
		{
			return ((Control)this).AutoSize;
		}
		set
		{
			if (((Control)this).AutoSize != value)
			{
				((Control)this).AutoSize = value;
				AdjustSize();
			}
		}
	}

	[DefaultValue(eOrientation.Horizontal)]
	[Category("Appearance")]
	[Description("Indicates text-orientation")]
	public eOrientation TextOrientation
	{
		get
		{
			return m_Label.TextOrientation;
		}
		set
		{
			m_Label.TextOrientation = value;
			InvalidateAutoSize();
			AdjustSize();
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates how vertical text is rotated when TextOrientation = Vertical.")]
	public bool VerticalTextTopUp
	{
		get
		{
			return m_Label.VerticalTextTopUp;
		}
		set
		{
			m_Label.VerticalTextTopUp = value;
			if (TextOrientation == eOrientation.Vertical)
			{
				((Control)this).Invalidate();
			}
		}
	}

	internal LabelItem LabelItem => m_Label;

	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates the symbol displayed on face of the tile instead of the image. Setting the symbol overrides the image setting.")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.SymbolTypeEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public string Symbol
	{
		get
		{
			return m_Label.Symbol;
		}
		set
		{
			m_Label.Symbol = value;
		}
	}

	[DefaultValue(0f)]
	[Category("Appearance")]
	[Description("Indicates the size of the symbol in points.")]
	public float SymbolSize
	{
		get
		{
			return m_Label.SymbolSize;
		}
		set
		{
			m_Label.SymbolSize = value;
		}
	}

	[Category("Columns")]
	[Description("Indicates color of the symbol.")]
	public Color SymbolColor
	{
		get
		{
			return m_Label.SymbolColor;
		}
		set
		{
			m_Label.SymbolColor = value;
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

	public event MarkupLinkClickEventHandler MarkupLinkClick;

	public LabelX()
	{
		m_Label = new LabelItem();
		m_Label.Style = eDotNetBarStyle.Office2007;
		m_Label.MarkupLinkClick += LabelMarkupLinkClick;
		FocusCuesEnabled = false;
		HostItem = m_Label;
		TabStop = false;
		((Control)this).SetStyle((ControlStyles)512, false);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		if (((Control)this).AutoSize)
		{
			AdjustSize();
		}
		RecalcLayout();
		((Control)this).OnHandleCreated(e);
	}

	protected override void RecalcSize()
	{
		m_Label.SuspendPaint = true;
		m_Label.Width = m_Label.Bounds.Width;
		m_Label.Height = m_Label.Bounds.Height;
		m_Label.SuspendPaint = false;
		base.RecalcSize();
	}

	protected override void OnBackColorChanged(EventArgs e)
	{
		m_Label.BackColor = ((Control)this).BackColor;
		((Control)this).OnBackColorChanged(e);
	}

	protected override void OnForeColorChanged(EventArgs e)
	{
		if (((Control)this).ForeColor == SystemColors.ControlText)
		{
			m_Label.ForeColor = Color.Empty;
		}
		else
		{
			m_Label.ForeColor = ((Control)this).ForeColor;
		}
		((Control)this).OnForeColorChanged(e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSingleLineColor()
	{
		return m_Label.ShouldSerializeSingleLineColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSingleLineColor()
	{
		m_Label.ResetSingleLineColor();
	}

	private void LabelMarkupLinkClick(object sender, MarkupLinkClickEventArgs e)
	{
		OnMarkupLinkClick(e);
	}

	protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
	{
		if (this.MarkupLinkClick != null)
		{
			this.MarkupLinkClick(this, e);
		}
	}

	protected void InvalidateAutoSize()
	{
		m_PreferredSize = Size.Empty;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (m_UseMnemonic)
		{
			m_Label.ShowPrefix = true;
		}
		else
		{
			m_Label.ShowPrefix = false;
		}
		if (m_Label.NeedRecalcSize)
		{
			m_Label.RecalcSize();
		}
		base.OnPaint(e);
	}

	private bool CanProcessMnemonic()
	{
		if (!((Control)this).Enabled || !((Control)this).Visible)
		{
			return false;
		}
		return true;
	}

	protected override bool ProcessMnemonic(char charCode)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		if (!UseMnemonic || !Control.IsMnemonic(charCode, ((Control)this).Text) || !CanProcessMnemonic() || (int)Control.ModifierKeys != 262144 || m_Label.IsUsingTextMarkup)
		{
			return false;
		}
		Control parent = ((Control)this).Parent;
		if (parent != null && parent.SelectNextControl((Control)(object)this, true, false, true, false) && !parent.ContainsFocus)
		{
			parent.Focus();
		}
		return true;
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		if (!m_PreferredSize.IsEmpty && (proposedSize.Width == m_PreferredSize.Width || proposedSize.Width == 0))
		{
			return m_PreferredSize;
		}
		if (!BarFunctions.IsHandleValid((Control)(object)this) || (((Component)(object)this).DesignMode && !((Control)this).AutoSize))
		{
			return ((Control)this).GetPreferredSize(proposedSize);
		}
		if (((Control)this).Text.Length == 0)
		{
			return ((Control)this).GetPreferredSize(proposedSize);
		}
		int width = m_Label.Width;
		int height = m_Label.Height;
		m_Label.SuspendPaint = true;
		m_Label.Width = 0;
		m_Label.Height = 0;
		if (((proposedSize.Width > 0 && proposedSize.Width < 500000) || ((Control)this).MaximumSize.Width > 0) && m_Label.TextMarkupBody != null)
		{
			if (TextOrientation == eOrientation.Horizontal)
			{
				m_Label.RecalcSizeMarkup((((Control)this).MaximumSize.Width > 0) ? ((Control)this).MaximumSize.Width : proposedSize.Width);
			}
			else
			{
				m_Label.RecalcSizeMarkup((((Control)this).MaximumSize.Height > 0) ? ((Control)this).MaximumSize.Height : proposedSize.Height);
			}
		}
		else
		{
			m_Label.RecalcSize();
			if (WordWrap && m_Label.WidthInternal > ((Control)this).MaximumSize.Width && ((Control)this).MaximumSize.Width > 0)
			{
				m_Label.Height = 0;
				m_Label.Width = ((Control)this).MaximumSize.Width;
				m_Label.RecalcSize();
			}
			else if (WordWrap && m_Label.WidthInternal > proposedSize.Width && proposedSize.Width > 0)
			{
				m_Label.Height = 0;
				m_Label.Width = proposedSize.Width;
				m_Label.RecalcSize();
			}
		}
		Size size = m_Label.Size;
		size.Height += 2;
		if (TextOrientation == eOrientation.Vertical)
		{
			size.Width += 2;
		}
		if (((Control)this).Font.Size > 13f)
		{
			size.Height += 2;
		}
		m_Label.Width = width;
		m_Label.Height = height;
		m_Label.SuspendPaint = false;
		m_PreferredSize = size;
		return m_PreferredSize;
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		if (((Control)this).AutoSize)
		{
			Size empty = Size.Empty;
			if (width != m_PreferredSize.Width && width > 0)
			{
				InvalidateAutoSize();
				empty = ((Control)this).GetPreferredSize(new Size(width, 0));
			}
			else
			{
				empty = ((Control)this).PreferredSize;
			}
			width = empty.Width;
			height = empty.Height;
		}
		((Control)this).SetBoundsCore(x, y, width, height, specified);
	}

	private void AdjustSize()
	{
		if (((Control)this).AutoSize)
		{
			((Control)this).Size = ((Control)this).PreferredSize;
		}
	}

	protected override void OnFontChanged(EventArgs e)
	{
		InvalidateAutoSize();
		base.OnFontChanged(e);
	}

	protected override void OnTextChanged(EventArgs e)
	{
		InvalidateAutoSize();
		base.OnTextChanged(e);
		AdjustSize();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSymbolColor()
	{
		return m_Label.ShouldSerializeSymbolColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSymbolColor()
	{
		m_Label.ResetSymbolColor();
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
