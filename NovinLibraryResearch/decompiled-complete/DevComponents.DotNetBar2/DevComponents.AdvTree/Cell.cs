using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Threading;
using System.Windows.Forms;
using DevComponents.AdvTree.Display;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.AdvTree;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class Cell : Component
{
	private ElementStyle ݒ;

	private ElementStyle ݓ;

	private ElementStyle ݔ;

	private ElementStyle Ӱ;

	private ElementStyle ӯ;

	private bool ݕ = true;

	private CellImages ݖ;

	private Rectangle ݗ = Rectangle.Empty;

	private Rectangle ݘ = Rectangle.Empty;

	private Rectangle ݙ = Rectangle.Empty;

	private Rectangle ݚ = Rectangle.Empty;

	private Rectangle ݛ = Rectangle.Empty;

	private object ӧ;

	private string ө = "";

	private eCellPartAlignment ݜ = eCellPartAlignment.Default;

	private eCellPartAlignment ݝ = eCellPartAlignment.Default;

	private bool ݞ;

	private bool ݟ;

	private bool ݠ;

	private bool ݡ;

	private bool ݢ;

	private Node ӭ;

	private bool Ӻ = true;

	private eTreeAction ݣ = eTreeAction.Code;

	private eCellPartLayout _0594;

	private Cursor ݤ;

	private bool ݥ;

	private string Ӿ = "";

	private Control ݦ;

	private Size ݧ = Size.Empty;

	private bool ݨ;

	private string _ݩ = "";

	private ToolTip _ݪ;

	private EventHandler ݫ;

	private bool _ו = true;

	private bool _ݬ = true;

	private BaseItem _ݭ;

	private string _ݮ = "";

	private string _ݯ = string.Empty;

	private bool _ݰ;

	private CheckState _ݱ;

	private eCheckBoxStyle _ݲ;

	private eCellEditorType _ݳ;

	private bool _ݴ;

	private bool _ݵ;

	private bool _Ԇ = true;

	private _0CB4 ݶ;

	private bool _ݷ = true;

	private bool _ݸ;

	private bool _ݹ;

	[Description("Indicates the text that is displayed when mouse hovers over the cell.")]
	[DefaultValue("")]
	[Localizable(true)]
	[Category("Appearance")]
	[Browsable(true)]
	public string Tooltip
	{
		get
		{
			return _ݩ;
		}
		set
		{
			if (_ݩ == value)
			{
				return;
			}
			if (value == null)
			{
				value = "";
			}
			_ݩ = value;
			if (ToolTipVisible)
			{
				if (string.IsNullOrEmpty(_ݩ))
				{
					HideToolTip();
				}
				else
				{
					ToolTip ݪ = _ݪ;
					ݪ.Text = _ݩ;
					ݪ.ShowToolTip();
					((Control)ݪ).Invalidate();
				}
			}
			OnTooltipChanged();
		}
	}

	protected internal bool ToolTipVisible => _ݪ != null;

	private Rectangle ScreenRectangle
	{
		get
		{
			AdvTree treeControl = TreeControl;
			if (treeControl == null)
			{
				return Rectangle.Empty;
			}
			return new Rectangle(((Control)treeControl).PointToScreen(Bounds.Location), Bounds.Size);
		}
	}

	[Description("Indicates whether tooltips are shown when mouse is over the cell when Tooltip property is set.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool ShowToolTips
	{
		get
		{
			return _ו;
		}
		set
		{
			_ו = value;
		}
	}

	[Category("Behavior")]
	[Description("Indicates whether cell content is editable when cell editing is enabled on tree control.")]
	[DefaultValue(true)]
	public bool Editable
	{
		get
		{
			return _ݬ;
		}
		set
		{
			_ݬ = value;
		}
	}

	[Browsable(false)]
	public bool IsEditable
	{
		get
		{
			ColumnHeader columnHeader = _175C.ផ(TreeControl, this);
			if (columnHeader == null)
			{
				return _ݬ;
			}
			if (_ݬ)
			{
				return columnHeader.Editable;
			}
			return false;
		}
	}

	[Description("Indicates item hosted inside of the cell. Only items that do not generate popups are supported. Note that cell can only host either HostedItem or HostedControl but not both.")]
	[Category("Appearance")]
	[Editor("DevComponents.AdvTree.Design.HostedItemTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue(null)]
	public BaseItem HostedItem
	{
		get
		{
			return _ݭ;
		}
		set
		{
			SetHostedItem(value);
		}
	}

	[Category("Behavior")]
	[Description("Indicates control hosted inside of the cell.")]
	[Browsable(true)]
	[DefaultValue(null)]
	public Control HostedControl
	{
		get
		{
			return ݦ;
		}
		set
		{
			SetHostedControl(value);
		}
	}

	internal bool IgnoreHostedControlSizeChange
	{
		get
		{
			return ݨ;
		}
		set
		{
			ݨ = value;
		}
	}

	internal Size HostedControlSize
	{
		get
		{
			return ݧ;
		}
		set
		{
			ݧ = value;
		}
	}

	[Browsable(false)]
	[Description("Indicates the name used to identify cell.")]
	[Category("Design")]
	public string Name
	{
		get
		{
			if (Site != null)
			{
				Ӿ = Site.Name;
			}
			return Ӿ;
		}
		set
		{
			if (Site != null)
			{
				Site.Name = value;
			}
			if (value == null)
			{
				Ӿ = "";
			}
			else
			{
				Ӿ = value;
			}
		}
	}

	[Browsable(false)]
	internal Rectangle BoundsRelative => ݗ;

	[Browsable(false)]
	public Rectangle Bounds
	{
		get
		{
			AdvTree treeControl = TreeControl;
			if (treeControl != null)
			{
				return NodeDisplay._070A(_1757._175B, this, treeControl.NodeDisplay.Offset);
			}
			return Rectangle.Empty;
		}
	}

	internal Rectangle TextContentBounds
	{
		get
		{
			return ݙ;
		}
		set
		{
			ݙ = value;
		}
	}

	[Browsable(false)]
	public Rectangle TextBounds
	{
		get
		{
			AdvTree treeControl = TreeControl;
			if (treeControl != null)
			{
				return NodeDisplay._070A(_1757._175A, this, treeControl.NodeDisplay.Offset);
			}
			return Rectangle.Empty;
		}
	}

	[Browsable(false)]
	internal Rectangle ImageBoundsRelative => ݚ;

	[Browsable(false)]
	public Rectangle ImageBounds
	{
		get
		{
			AdvTree treeControl = TreeControl;
			if (treeControl != null)
			{
				return NodeDisplay._070A(_1757._1759, this, treeControl.NodeDisplay.Offset);
			}
			return Rectangle.Empty;
		}
	}

	[Browsable(false)]
	internal Rectangle CheckBoxBoundsRelative => ݛ;

	[Browsable(false)]
	public Rectangle CheckBoxBounds
	{
		get
		{
			AdvTree treeControl = TreeControl;
			if (treeControl != null)
			{
				return NodeDisplay._070A(_1757._1758, this, treeControl.NodeDisplay.Offset);
			}
			return Rectangle.Empty;
		}
	}

	[Browsable(false)]
	public bool IsEditing => false;

	[Browsable(false)]
	public bool IsSelected => ݢ;

	[Browsable(false)]
	public bool IsVisible => Ӻ;

	[Browsable(false)]
	public Node Parent => ӭ;

	[Description("Indicates text that contains data about the cell.")]
	[DefaultValue(null)]
	[Category("Data")]
	[Browsable(false)]
	public object Tag
	{
		get
		{
			return ӧ;
		}
		set
		{
			ӧ = value;
		}
	}

	[Category("Data")]
	[Description("Indicates text that contains data about the cell.")]
	[Browsable(true)]
	[DevCoSerialize]
	[DefaultValue("")]
	public string TagString
	{
		get
		{
			if (ӧ == null)
			{
				return "";
			}
			return ӧ.ToString();
		}
		set
		{
			ӧ = value;
		}
	}

	[DefaultValue("")]
	[Browsable(true)]
	[Description("Indicates format that is applied to the value of Text property for display purposes.")]
	[Category("Appearance")]
	public string TextDisplayFormat
	{
		get
		{
			return _ݮ;
		}
		set
		{
			_ݮ = value;
			UpdateDisplayText();
			OnSizeChanged();
		}
	}

	internal string DisplayText => _ݯ;

	[DevCoSerialize]
	[Browsable(true)]
	[Localizable(true)]
	[Category("Appearance")]
	[DefaultValue("")]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Description("Indicates text displayed in the cell.")]
	public string Text
	{
		get
		{
			return ө;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			if (ө != value)
			{
				ө = value;
				UpdateDisplayText();
				OnTextChanged();
				OnSizeChanged();
			}
		}
	}

	[Browsable(false)]
	public AdvTree TreeControl
	{
		get
		{
			if (Parent != null)
			{
				return Parent.TreeControl;
			}
			return null;
		}
	}

	[Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Category("Style")]
	[Description("Indicates the style class assigned to the cell.")]
	[Browsable(true)]
	[DefaultValue(null)]
	public ElementStyle StyleNormal
	{
		get
		{
			return ݒ;
		}
		set
		{
			ݒ = value;
		}
	}

	[Browsable(false)]
	[DevCoSerialize]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue("")]
	public string StyleNormalName
	{
		get
		{
			if (ݒ != null)
			{
				return ݒ.Name;
			}
			return "";
		}
		set
		{
			if (value.Length == 0)
			{
				TypeDescriptor.GetProperties(this)["StyleNormal"].SetValue(this, null);
				return;
			}
			AdvTree treeControl = TreeControl;
			if (treeControl != null)
			{
				TypeDescriptor.GetProperties(this)["StyleNormal"].SetValue(this, treeControl.Styles[value]);
			}
		}
	}

	[Category("Style")]
	[DefaultValue(null)]
	[Browsable(true)]
	[Description("Indicates the style class assigned to the cell.")]
	[Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	public ElementStyle StyleSelected
	{
		get
		{
			return ӯ;
		}
		set
		{
			ӯ = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DefaultValue("")]
	[Browsable(false)]
	[DevCoSerialize]
	public string StyleSelectedName
	{
		get
		{
			if (ӯ != null)
			{
				return ӯ.Name;
			}
			return "";
		}
		set
		{
			if (value.Length == 0)
			{
				TypeDescriptor.GetProperties(this)["StyleSelected"].SetValue(this, null);
				return;
			}
			AdvTree treeControl = TreeControl;
			if (treeControl != null)
			{
				TypeDescriptor.GetProperties(this)["StyleSelected"].SetValue(this, treeControl.Styles[value]);
			}
		}
	}

	[Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Description("Indicates the disabled style class assigned to the cell.")]
	[Category("Style")]
	[DefaultValue(null)]
	[Browsable(true)]
	public ElementStyle StyleDisabled
	{
		get
		{
			return ݓ;
		}
		set
		{
			ݓ = value;
		}
	}

	[DefaultValue("")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DevCoSerialize]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string StyleDisabledName
	{
		get
		{
			if (ݓ != null)
			{
				return ݓ.Name;
			}
			return "";
		}
		set
		{
			if (value.Length == 0)
			{
				TypeDescriptor.GetProperties(this)["StyleDisabled"].SetValue(this, null);
				return;
			}
			AdvTree treeControl = TreeControl;
			if (treeControl != null)
			{
				TypeDescriptor.GetProperties(this)["StyleDisabled"].SetValue(this, treeControl.Styles[value]);
			}
		}
	}

	[DefaultValue(null)]
	[Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Category("Style")]
	[Browsable(true)]
	[Description("Indicates the style class assigned to the cell when mouse is down.")]
	public ElementStyle StyleMouseDown
	{
		get
		{
			return ݔ;
		}
		set
		{
			ݔ = value;
		}
	}

	[DefaultValue("")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DevCoSerialize]
	public string StyleMouseDownName
	{
		get
		{
			if (ݔ != null)
			{
				return ݔ.Name;
			}
			return "";
		}
		set
		{
			if (value.Length == 0)
			{
				TypeDescriptor.GetProperties(this)["StyleMouseDown"].SetValue(this, null);
				return;
			}
			AdvTree treeControl = TreeControl;
			if (treeControl != null)
			{
				TypeDescriptor.GetProperties(this)["StyleMouseDown"].SetValue(this, treeControl.Styles[value]);
			}
		}
	}

	[Category("Style")]
	[Description("Indicates the style class assigned to the cell when mouse is over the cell.")]
	[Browsable(true)]
	[DefaultValue("")]
	[Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	public ElementStyle StyleMouseOver
	{
		get
		{
			return Ӱ;
		}
		set
		{
			Ӱ = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[DevCoSerialize]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DefaultValue("")]
	public string StyleMouseOverName
	{
		get
		{
			if (Ӱ != null)
			{
				return Ӱ.Name;
			}
			return "";
		}
		set
		{
			if (value.Length == 0)
			{
				TypeDescriptor.GetProperties(this)["StyleMouseOver"].SetValue(this, null);
				return;
			}
			AdvTree treeControl = TreeControl;
			if (treeControl != null)
			{
				TypeDescriptor.GetProperties(this)["StyleMouseOver"].SetValue(this, treeControl.Styles[value]);
			}
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Description("Gets or sets whether cell is enabled or not.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool Enabled
	{
		get
		{
			return ݕ;
		}
		set
		{
			ݕ = value;
			if (Parent != null)
			{
				Parent.OnDisplayChanged();
			}
		}
	}

	[Category("Images")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	[Description("Gets the reference to images associated with this cell.")]
	public CellImages Images => ݖ;

	[Category("Image Properties")]
	[Description("Gets or sets the image alignment in relation to the text displayed by cell.")]
	[DefaultValue(eCellPartAlignment.Default)]
	[Browsable(true)]
	[DevCoSerialize]
	public eCellPartAlignment ImageAlignment
	{
		get
		{
			return ݜ;
		}
		set
		{
			if (ݜ != value)
			{
				ݜ = value;
				OnAppearanceChanged();
			}
		}
	}

	[Description("Indicates checkbox alignment in relation to the text displayed by cell.")]
	[Browsable(true)]
	[DefaultValue(eCellPartAlignment.Default)]
	[Category("Check-box Properties")]
	public eCellPartAlignment CheckBoxAlignment
	{
		get
		{
			return ݝ;
		}
		set
		{
			if (ݝ != value)
			{
				ݝ = value;
				OnAppearanceChanged();
			}
		}
	}

	[DevCoSerialize]
	[DefaultValue(false)]
	[Browsable(true)]
	[Description("Indicates whether check box is visible inside the cell.")]
	[Category("Check-box Properties")]
	public bool CheckBoxVisible
	{
		get
		{
			return ݞ;
		}
		set
		{
			if (ݞ != value)
			{
				ݞ = value;
				OnAppearanceChanged();
			}
		}
	}

	[Description("Indicates whether the check box is in the checked state.")]
	[Category("Check-box Properties")]
	[DefaultValue(false)]
	[Browsable(true)]
	[DevCoSerialize]
	public bool Checked
	{
		get
		{
			return ݟ;
		}
		set
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			if (ݟ != value && (!_ݰ || !value || (int)_ݱ == 0))
			{
				SetChecked((CheckState)(value ? 1 : 0), eTreeAction.Code);
			}
		}
	}

	[Category("Check-box Properties")]
	[DefaultValue(false)]
	[Description("Indicates whether the CheckBox will allow three check states rather than two.")]
	[Browsable(true)]
	public bool CheckBoxThreeState
	{
		get
		{
			return _ݰ;
		}
		set
		{
			_ݰ = value;
		}
	}

	[Browsable(true)]
	[RefreshProperties(RefreshProperties.All)]
	[Category("Check-box Properties")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Specifies the state of a control, such as a check box, that can be checked, unchecked, or set to an indeterminate state")]
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
			if (value != _ݱ)
			{
				SetChecked(value, eTreeAction.Code);
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(eCheckBoxStyle.CheckBox)]
	[Category("Check-box Properties")]
	[Description("Indicates appearance style of the item. Default value is CheckBox. Item can also assume the style of radio-button.")]
	public eCheckBoxStyle CheckBoxStyle
	{
		get
		{
			return _ݲ;
		}
		set
		{
			_ݲ = value;
			OnAppearanceChanged();
		}
	}

	[Browsable(false)]
	public bool IsMouseOver => ݡ;

	[Browsable(false)]
	public bool IsMouseDown => ݠ;

	[Category("Cells")]
	[DevCoSerialize]
	[Description("Indicates the layout of the cell parts like check box, image and text.")]
	[Browsable(true)]
	[DefaultValue(eCellPartLayout.Default)]
	public eCellPartLayout Layout
	{
		get
		{
			return _0594;
		}
		set
		{
			if (_0594 != value)
			{
				_0594 = value;
				OnAppearanceChanged();
			}
		}
	}

	[Description("Specifies the mouse cursor displayed when mouse is over the cell.")]
	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(null)]
	public Cursor Cursor
	{
		get
		{
			return ݤ;
		}
		set
		{
			if (ݤ != value)
			{
				ݤ = value;
			}
		}
	}

	internal bool WordWrap
	{
		get
		{
			return ݥ;
		}
		set
		{
			ݥ = value;
		}
	}

	[Category("Behavior")]
	[Description("Indicates editor type used to edit the cell.")]
	[DefaultValue(eCellEditorType.Default)]
	public eCellEditorType EditorType
	{
		get
		{
			return _ݳ;
		}
		set
		{
			_ݳ = value;
		}
	}

	[Description("Indicates whether cell can be selected by user by clicking it with the mouse or using keyboard.")]
	[DefaultValue(true)]
	[Category("Behavior")]
	public bool Selectable
	{
		get
		{
			return _Ԇ;
		}
		set
		{
			_Ԇ = value;
		}
	}

	[Browsable(false)]
	public bool CanSelect => Enabled & IsVisible & Selectable;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ColumnHeader ColumnHeader
	{
		get
		{
			AdvTree treeControl = TreeControl;
			if (Parent == null || treeControl == null)
			{
				return null;
			}
			int num = Parent.Cells.IndexOf(this);
			if (Parent.HasColumns && Parent.NodesColumns.Count > num)
			{
				return Parent.NodesColumns[num];
			}
			if (treeControl.Columns.Count > num)
			{
				return treeControl.Columns[num];
			}
			return null;
		}
	}

	internal _0CB4 TextMarkupBody => ݶ;

	[DefaultValue(true)]
	[Description("Gets or sets whether cell supports and renders text markup.")]
	[Category("Behavior")]
	public bool TextMarkupEnabled
	{
		get
		{
			return _ݷ;
		}
		set
		{
			_ݷ = value;
		}
	}

	protected virtual bool IsMarkupSupported => _ݷ;

	[Description("Occurs when item's tooltip visibility has changed.")]
	public event EventHandler ToolTipVisibleChanged
	{
		add
		{
			EventHandler eventHandler = ݫ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ݫ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ݫ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ݫ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public Cell()
		: this("")
	{
	}

	public Cell(string text)
	{
		Text = text;
		ݖ = new CellImages(this);
	}

	public Cell(string text, ElementStyle styleNormal)
		: this(text)
	{
		StyleNormal = styleNormal;
	}

	public Cell(string text, string styleNormalName)
		: this(text)
	{
		StyleNormalName = styleNormalName;
	}

	protected override void Dispose(bool disposing)
	{
		if (ݦ != null)
		{
			ݦ.SizeChanged -= HostedControlSizedChanged;
		}
		ݦ = null;
		if (ݖ != null)
		{
			ݖ.Dispose();
		}
		base.Dispose(disposing);
	}

	protected virtual void OnTooltipChanged()
	{
	}

	protected virtual void OnTooltip(bool isShown)
	{
	}

	public virtual void ShowToolTip()
	{
		if (base.DesignMode || !IsVisible)
		{
			return;
		}
		AdvTree treeControl = TreeControl;
		if ((treeControl != null && !treeControl.ShowToolTips) || !ShowToolTips)
		{
			return;
		}
		OnTooltip(isShown: true);
		if (_ݩ != "")
		{
			if (_ݪ == null)
			{
				_ݪ = new ToolTip();
			}
			_ݪ.Style = StyleManager.GetEffectiveStyle();
			_ݪ.Text = _ݩ;
			_ݪ.ReferenceRectangle = ScreenRectangle;
			OnToolTipVisibleChanged(new EventArgs());
			_ݪ.ShowToolTip();
		}
	}

	protected internal void HideToolTip()
	{
		if (_ݪ == null)
		{
			return;
		}
		Rectangle bounds = ((Control)_ݪ).Bounds;
		bounds.Width += 5;
		bounds.Height += 6;
		OnTooltip(isShown: false);
		OnToolTipVisibleChanged(new EventArgs());
		try
		{
			if (_ݪ != null)
			{
				((Control)_ݪ).Hide();
				((Component)(object)_ݪ).Dispose();
				_ݪ = null;
			}
		}
		catch
		{
		}
		AdvTree treeControl = TreeControl;
		if (treeControl != null)
		{
			((Control)treeControl).Invalidate(((Control)treeControl).RectangleToClient(bounds), false);
		}
	}

	private void OnToolTipVisibleChanged(EventArgs eventArgs)
	{
		EventHandler eventHandler = ݫ;
		if (eventHandler != null)
		{
			ݫ(this, eventArgs);
		}
	}

	internal void SetBounds(Rectangle bounds)
	{
		ݗ = bounds;
	}

	internal void SetImageBounds(Rectangle bounds)
	{
		ݚ = bounds;
	}

	internal void SetCheckBoxBounds(Rectangle bounds)
	{
		ݛ = bounds;
	}

	internal void SetSelected(bool selected, eTreeAction actionSource)
	{
		ݢ = selected;
	}

	internal void SetVisible(bool visible)
	{
		if (Ӻ != visible)
		{
			Ӻ = visible;
			OnVisibleChanged();
		}
	}

	internal void SetParent(Node parent)
	{
		ӭ = parent;
	}

	private void UpdateDisplayText()
	{
		if (string.IsNullOrEmpty(_ݮ))
		{
			_ݯ = ө;
			return;
		}
		long result = 0L;
		double result2 = 0.0;
		DateTime result3 = DateTime.MinValue;
		if (long.TryParse(ө, out result))
		{
			_ݯ = result.ToString(_ݮ);
		}
		else if (double.TryParse(ө, out result2))
		{
			_ݯ = result2.ToString(_ݮ);
		}
		else if (DateTime.TryParse(ө, out result3))
		{
			_ݯ = result3.ToString(_ݮ);
		}
		else
		{
			_ݯ = ө;
		}
	}

	protected virtual void OnTextChanged()
	{
		MarkupTextChanged();
	}

	internal void SetCellImages(CellImages ci)
	{
		ݖ = ci;
		ݖ.Parent = this;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal bool ShouldSerializeImages()
	{
		return ݖ._26ED;
	}

	public void SetChecked(bool value, eTreeAction actionSource)
	{
		SetChecked((CheckState)(value ? 1 : 0), actionSource);
	}

	public void SetChecked(CheckState value, eTreeAction actionSource)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Invalid comparison between Unknown and I4
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Invalid comparison between Unknown and I4
		AdvTreeCellBeforeCheckEventArgs advTreeCellBeforeCheckEventArgs = new AdvTreeCellBeforeCheckEventArgs(actionSource, this, value);
		InvokeBeforeCheck(advTreeCellBeforeCheckEventArgs);
		if (advTreeCellBeforeCheckEventArgs.Cancel)
		{
			return;
		}
		_ݱ = value;
		ݟ = (int)value == 1;
		OnAppearanceChanged();
		if (CheckBoxStyle == eCheckBoxStyle.RadioButton && (int)value == 1 && Parent != null)
		{
			Node parent = Parent;
			AdvTree treeControl = parent.TreeControl;
			treeControl?.BeginUpdate();
			bool flag = true;
			foreach (Cell cell in parent.Cells)
			{
				if (cell != this && cell.CheckBoxStyle == eCheckBoxStyle.RadioButton)
				{
					cell.Checked = false;
					flag = false;
				}
			}
			if (flag)
			{
				if (parent.Parent != null)
				{
					foreach (Node node3 in parent.Parent.Nodes)
					{
						if (node3 != parent && node3.CheckBoxStyle == eCheckBoxStyle.RadioButton)
						{
							node3.Checked = false;
						}
					}
				}
				else if (treeControl != null && _175C.ܢ(treeControl, parent))
				{
					foreach (Node node4 in treeControl.Nodes)
					{
						if (node4 != parent && node4.CheckBoxStyle == eCheckBoxStyle.RadioButton)
						{
							node4.Checked = false;
						}
					}
				}
			}
			treeControl?.EndUpdate();
		}
		InvokeAfterCheck(actionSource);
	}

	internal void SetMouseOver(bool over)
	{
		ݡ = over;
	}

	internal void SetMouseDown(bool over)
	{
		ݠ = over;
	}

	internal bool GetEnabled()
	{
		return ݕ;
	}

	internal eCellEditorType GetEffectiveEditorType()
	{
		if (_ݳ != 0)
		{
			return _ݳ;
		}
		int num = -1;
		if (Parent != null)
		{
			num = Parent.Cells.IndexOf(this);
		}
		if (num == -1)
		{
			return _ݳ;
		}
		if (Parent != null && Parent.Parent != null && Parent.Parent.NodesColumns.Count > 0 && num < Parent.Parent.NodesColumns.Count)
		{
			return Parent.Parent.NodesColumns[num].EditorType;
		}
		AdvTree treeControl = TreeControl;
		if (treeControl != null && treeControl.Columns.Count > 0 && num < treeControl.Columns.Count)
		{
			return treeControl.Columns[num].EditorType;
		}
		return _ݳ;
	}

	public virtual Cell Copy()
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		Cell cell = new Cell();
		cell.CheckBoxAlignment = CheckBoxAlignment;
		cell.CheckBoxVisible = CheckBoxVisible;
		cell.CheckBoxStyle = CheckBoxStyle;
		if (CheckBoxThreeState)
		{
			cell.CheckState = CheckState;
		}
		else
		{
			cell.Checked = Checked;
		}
		cell.Cursor = Cursor;
		cell.Enabled = Enabled;
		cell.ImageAlignment = ImageAlignment;
		cell.SetCellImages(Images.Copy());
		cell.Layout = Layout;
		cell.StyleDisabled = StyleDisabled;
		cell.StyleMouseDown = StyleMouseDown;
		cell.StyleMouseOver = StyleMouseOver;
		cell.StyleNormal = StyleNormal;
		cell.StyleSelected = StyleSelected;
		cell.Tag = Tag;
		cell.Text = Text;
		cell.Editable = Editable;
		cell.EditorType = EditorType;
		return cell;
	}

	public void EnsureVisible()
	{
		_175C._1771(this);
	}

	private void OnAppearanceChanged()
	{
		if (Parent != null)
		{
			Parent.SizeChanged = true;
			Parent.OnDisplayChanged();
		}
	}

	private void OnSizeChanged()
	{
		if (Parent != null)
		{
			Parent.SizeChanged = true;
			Parent.OnDisplayChanged();
		}
	}

	internal void OnImageChanged()
	{
		if (ӭ != null)
		{
			ӭ.OnImageChanged();
		}
	}

	protected virtual void InvokeAfterCheck(eTreeAction actionSource)
	{
		TreeControl?.ڷ(new AdvTreeCellEventArgs(actionSource, this));
		ݣ = eTreeAction.Code;
	}

	protected virtual void InvokeBeforeCheck(AdvTreeCellBeforeCheckEventArgs e)
	{
		TreeControl?.ڶ(e);
		if (Parent != null)
		{
			Parent.BeforeCellCheck(this, e);
		}
	}

	internal void OnLayoutCell()
	{
		if (ݦ != null && !_ݵ)
		{
			AddHostedControlToTree();
		}
		else if (_ݭ != null && !_ݴ)
		{
			AddHostedItemToTree();
		}
	}

	private void SetHostedItem(BaseItem value)
	{
		if (ݦ != null)
		{
			throw new InvalidOperationException("HostedItem cannot be set if HostedControl property is set to non-null value.");
		}
		if (_ݭ != null)
		{
			RemoveHostedItemFromTree();
		}
		_ݭ = value;
		if (_ݭ != null)
		{
			if (_ݭ.Parent != null)
			{
				_ݭ.Parent.SubItems.Remove(_ݭ);
			}
			_ݭ.Visible = true;
			_ݭ.Style = eDotNetBarStyle.StyleManagerControlled;
			AddHostedItemToTree();
		}
		OnSizeChanged();
	}

	private void RemoveHostedItemFromTree()
	{
		if (_ݭ != null)
		{
			_ݭ.ContainerControl = null;
		}
		_ݴ = false;
	}

	private void AddHostedItemToTree()
	{
		if (_ݴ)
		{
			return;
		}
		AdvTree treeControl = TreeControl;
		if (treeControl != null)
		{
			if (_ݭ.ContainerControl != treeControl)
			{
				_ݭ.ContainerControl = treeControl;
			}
			_ݴ = true;
		}
	}

	internal void AddHostedControlToTree()
	{
		if (_ݵ)
		{
			return;
		}
		AdvTree treeControl = TreeControl;
		if (treeControl == null)
		{
			return;
		}
		if (ݦ.Parent != treeControl)
		{
			if (ݦ.Parent != null)
			{
				ݦ.Parent.Controls.Remove(ݦ);
			}
			((Control)treeControl).Controls.Add(ݦ);
			treeControl.Հ = true;
			ݦ.SendToBack();
		}
		treeControl.Ծ.Add(this);
		_ݵ = true;
	}

	internal void RemoveHostedControlFromTree()
	{
		AdvTree advTree = ݦ.Parent as AdvTree;
		if (advTree == null)
		{
			advTree = TreeControl;
		}
		if (advTree != null)
		{
			((Control)advTree).Controls.Remove(ݦ);
			advTree.Ծ.Remove(this);
			_ݵ = false;
		}
	}

	private void SetHostedControl(Control value)
	{
		if (value == TreeControl)
		{
			return;
		}
		if (_ݭ != null)
		{
			throw new InvalidOperationException("HostedControl cannot be set when HostedItem is set to non-null value.");
		}
		if (ݦ != null)
		{
			ݦ.SizeChanged -= HostedControlSizedChanged;
			ݦ.Enter -= HostedControlEnter;
			RemoveHostedControlFromTree();
		}
		ݦ = value;
		if (ݦ != null)
		{
			ݦ.SizeChanged += HostedControlSizedChanged;
			ݦ.Enter += HostedControlEnter;
			TypeDescriptor.GetProperties(ݦ)["Dock"].SetValue(ݦ, (object)(DockStyle)0);
			if (ݦ.Parent != null)
			{
				ݦ.Parent.Controls.Remove(ݦ);
			}
			if (!base.DesignMode)
			{
				ݦ.Visible = false;
			}
			if (base.DesignMode || (Parent != null && Parent.Site != null && Parent.Site.DesignMode))
			{
				bool visible = GetVisible();
				TypeDescriptor.GetProperties(ݦ)["Visible"].SetValue(ݦ, visible);
			}
			AddHostedControlToTree();
		}
		OnSizeChanged();
	}

	private void HostedControlEnter(object sender, EventArgs e)
	{
		AdvTree treeControl = TreeControl;
		if (treeControl != null)
		{
			treeControl.SelectedNode = Parent;
		}
	}

	private void HostedControlSizedChanged(object sender, EventArgs e)
	{
		if (!ݧ.IsEmpty && ݦ != null)
		{
			ݧ = ݦ.Size;
		}
		if (!ݨ)
		{
			OnSizeChanged();
		}
	}

	internal void OnVisibleChanged()
	{
		if (ݦ != null)
		{
			TypeDescriptor.GetProperties(ݦ)["Visible"].SetValue(ݦ, GetVisible());
		}
	}

	private bool GetVisible()
	{
		if (ӭ != null && !_175C.ᝫ(ӭ))
		{
			return false;
		}
		return Ӻ;
	}

	internal void OnParentExpandedChanged(bool expanded)
	{
		if (ݦ != null && ݦ.Visible != expanded)
		{
			bool visible = GetVisible();
			ݦ.Visible = visible;
			if (base.DesignMode || (Parent != null && Parent.Site != null && Parent.Site.DesignMode))
			{
				TypeDescriptor.GetProperties(ݦ)["Visible"].SetValue(ݦ, visible);
			}
		}
	}

	private void MarkupTextChanged()
	{
		if (!IsMarkupSupported)
		{
			return;
		}
		if (ݶ != null)
		{
			ݶ.ವ -= TextMarkupLinkClick;
		}
		ݶ = null;
		if (ሒ.መ(ref ө))
		{
			ݶ = ሒ.ሕ(ө);
			if (ݶ != null)
			{
				ݶ.ವ += TextMarkupLinkClick;
			}
		}
	}

	protected virtual void TextMarkupLinkClick(object sender, EventArgs e)
	{
		if (sender is _0CFE obj && Parent != null)
		{
			Parent.InvokeMarkupLinkClick(this, new MarkupLinkClickEventArgs(obj._0D01, obj._0D00));
		}
	}

	protected internal virtual void InvokeNodeMouseMove(object sender, MouseEventArgs e)
	{
		if (ݶ != null)
		{
			ݶ.મ((Control)((sender is Control) ? sender : null), e);
		}
		if (_ݭ != null)
		{
			if (_ݭ.DisplayRectangle.Contains(e.X, e.Y))
			{
				if (!_ݸ)
				{
					_ݭ.InternalMouseEnter();
					_ݸ = true;
				}
				_ݭ.InternalMouseMove(e);
			}
			else if (_ݸ)
			{
				_ݭ.InternalMouseLeave();
				_ݸ = false;
			}
		}
		if (!IsMouseOver)
		{
			HideToolTip();
		}
	}

	protected internal virtual void InvokeNodeMouseDown(object sender, MouseEventArgs e)
	{
		HideToolTip();
		if (ݶ != null)
		{
			ݶ.ग़((Control)((sender is Control) ? sender : null), e);
		}
		if (_ݭ != null && _ݭ.DisplayRectangle.Contains(e.X, e.Y))
		{
			_ݭ.InternalMouseDown(e);
			_ݹ = true;
		}
	}

	protected internal virtual void InvokeNodeMouseUp(object sender, MouseEventArgs e)
	{
		if (ݶ != null)
		{
			ݶ.ફ((Control)((sender is Control) ? sender : null), e);
		}
		if (_ݭ != null && _ݹ)
		{
			_ݭ.InternalMouseUp(e);
			_ݹ = false;
		}
	}

	protected internal virtual void InvokeNodeMouseLeave(object sender, EventArgs e)
	{
		HideToolTip();
		if (ݶ != null)
		{
			ݶ.ભ((Control)((sender is Control) ? sender : null));
		}
		if (_ݭ != null && _ݸ)
		{
			_ݭ.InternalMouseLeave();
			_ݸ = false;
		}
	}

	protected internal virtual void InvokeNodeClick(object sender, EventArgs e)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		if (ݶ != null)
		{
			ݶ.દ((Control)((sender is Control) ? sender : null));
		}
		if (_ݭ != null && e is TreeNodeMouseEventArgs)
		{
			TreeNodeMouseEventArgs treeNodeMouseEventArgs = (TreeNodeMouseEventArgs)e;
			if (Bounds.Contains(treeNodeMouseEventArgs.X, treeNodeMouseEventArgs.Y))
			{
				_ݭ.InternalClick(treeNodeMouseEventArgs.Button, new Point(treeNodeMouseEventArgs.X, treeNodeMouseEventArgs.Y));
			}
		}
	}

	protected internal virtual void InvokeNodeMouseHover(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (_ݭ != null)
		{
			_ݭ.InternalMouseHover();
		}
		if ((int)Control.MouseButtons == 0)
		{
			ShowToolTip();
		}
	}
}
