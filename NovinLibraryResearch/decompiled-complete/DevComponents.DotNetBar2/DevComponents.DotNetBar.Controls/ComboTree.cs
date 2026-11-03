using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DevComponents.AdvTree;
using DevComponents.AdvTree.Display;
using DevComponents.AdvTree.Interop;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Controls;

[ToolboxBitmap(typeof(ComboTree), "Controls.ComboTree.ico")]
[DefaultProperty("Text")]
[ToolboxItem(true)]
[DefaultBindingProperty("Text")]
[DefaultEvent("TextChanged")]
[Designer("DevComponents.DotNetBar.Design.ComboTreeDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class ComboTree : PopupItemControl, ICommandSource, IInputButtonControl
{
	private DevComponents.AdvTree.AdvTree __28AB;

	private ButtonItem _០;

	private static string __22BB = "sysPopupItemContainer";

	private static string __22BA = "sysPopupControlContainer";

	private Color _ᚾ = ColorScheme.GetColor(16777096);

	private bool _ᚼ;

	private CancelEventHandler _៦;

	private CancelEventHandler _៧;

	private EventHandler _181E;

	private EventHandler _181F;

	private EventHandler _28AC;

	private AdvTreeNodeCancelEventHandler ᛗ;

	private AdvTreeNodeEventHandler _05B1;

	private EventHandler _05C1;

	private EventHandler _05C2;

	private TreeConvertEventHandler _05C3;

	private EventHandler _05C4;

	private EventHandler _05C5;

	private EventHandler _05C6;

	private DataNodeEventHandler _05C7;

	private DataNodeEventHandler _05C8;

	private EventHandler _05C9;

	private EventHandler _05CA;

	private EventHandler _05CB;

	private DataColumnEventHandler _05CC;

	private bool _ᛘ = true;

	private string __28AD = "";

	private List<BindingMemberInfo> _ئ;

	private object _ا;

	private bool _ب;

	private string _ة = "";

	private IFormatProvider _ت;

	private bool _ث;

	private Hashtable _ج = new Hashtable();

	private string _خ = "";

	private string _د = "";

	private ElementStyle _ذ;

	private CurrencyManager _ر;

	private bool _ز;

	private bool _س;

	private static TypeConverter ش;

	private BindingMemberInfo _ض = default(BindingMemberInfo);

	private Font _ᠣ;

	private Color _ᠤ = SystemColors.GrayText;

	private bool _ᚿ = true;

	private string _ᠥ = "";

	private eTextAlignment _ᠦ;

	private ElementStyle _ᠩ = new ElementStyle();

	private InputButtonSettings __17F1;

	private InputButtonSettings __17F2;

	private InputButtonSettings _ᠧ;

	private InputButtonSettings _ᠨ;

	private VisualGroup _ᶇ;

	private bool _ኗ;

	private bool __28AE = true;

	private bool __05F7 = true;

	private bool __28AF = true;

	private Timer __05F8;

	private int __05F9 = 1000;

	private string __05FB = "";

	private bool __28B0;

	private Control __22C0;

	private bool __28B1;

	private DateTime __17F4 = DateTime.MinValue;

	private Point? __28B2 = null;

	private Control __22C3;

	private int __28B3;

	private int __28B4;

	private Color _ᖣ = Color.Empty;

	private ICommand _૦;

	private object _૧;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Node SelectedNode
	{
		get
		{
			return __28AB.SelectedNode;
		}
		set
		{
			__28AB.SelectedNode = value;
		}
	}

	[Description("Indicates whether selection change on popup tree closes the popup.")]
	[DefaultValue(true)]
	[Category("Behavior")]
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

	[Category("Data")]
	[Description("Indicates field name that holds the text that will be displayed in the control for selected item. When not set all items set in DisplayMembers will be displayed in control.")]
	[DefaultValue("")]
	[Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version= 2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	public string SelectedDisplayMember
	{
		get
		{
			return __28AD;
		}
		set
		{
			__28AD = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Obsolete("This property name was misspelled. Use SelectedDisplayMember instead.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public string SelectedDisplayMemeber
	{
		get
		{
			return __28AD;
		}
		set
		{
			__28AD = value;
			((Control)this).Invalidate();
		}
	}

	[Editor("DevComponents.DotNetBar.Design.DataMembersSelector, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Description("Indicates comma separated list of property or column names to display on popup tree control")]
	[DefaultValue("")]
	[Category("Data")]
	public string DisplayMembers
	{
		get
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			if (_ئ == null || _ئ.Count == 0)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < _ئ.Count; i++)
			{
				BindingMemberInfo val = _ئ[i];
				stringBuilder.Append(((BindingMemberInfo)(ref val)).BindingMember);
				if (i + 1 < _ئ.Count)
				{
					stringBuilder.Append(',');
				}
			}
			return stringBuilder.ToString();
		}
		set
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			List<BindingMemberInfo> ئ = _ئ;
			List<BindingMemberInfo> list = null;
			if (!string.IsNullOrEmpty(value))
			{
				list = new List<BindingMemberInfo>();
				string[] array = value.Split(new char[1] { ',' });
				for (int i = 0; i < array.Length; i++)
				{
					list.Add(new BindingMemberInfo(array[i].Trim()));
				}
			}
			try
			{
				_06DA(_ا, list, _06DD: false);
			}
			catch
			{
				_ئ = ئ;
			}
		}
	}

	[Description("Indicates data source for the ComboTree.")]
	[Category("Data")]
	[DefaultValue(null)]
	[RefreshProperties(RefreshProperties.Repaint)]
	[AttributeProvider(typeof(IListSource))]
	public object DataSource
	{
		get
		{
			return _ا;
		}
		set
		{
			if (value != null && !(value is IList) && !(value is IListSource))
			{
				throw new ArgumentException("Data type is not supported for complex data binding");
			}
			if (_ا != value)
			{
				try
				{
					_06DA(value, _ئ, _06DD: true);
				}
				catch
				{
					DisplayMembers = "";
				}
				if (value == null)
				{
					DisplayMembers = "";
				}
			}
		}
	}

	[Description("Indicates whether formatting is applied to the DisplayMembers property of the control.")]
	[DefaultValue(false)]
	public bool FormattingEnabled
	{
		get
		{
			return _ب;
		}
		set
		{
			if (value != _ب)
			{
				_ب = value;
				RefreshItems();
				OnFormattingEnabledChanged(EventArgs.Empty);
			}
		}
	}

	[Editor("System.Windows.Forms.Design.FormatStringEditor, System.Design, Version= 2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[DefaultValue("")]
	[Description("Indicates format-specifier characters that indicate how a value is to be displayed.")]
	[MergableProperty(false)]
	public string FormatString
	{
		get
		{
			return _ة;
		}
		set
		{
			if (value == null)
			{
				value = string.Empty;
			}
			if (!value.Equals(_ة))
			{
				_ة = value;
				RefreshItems();
				OnFormatStringChanged(EventArgs.Empty);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[DefaultValue(null)]
	[Browsable(false)]
	public IFormatProvider FormatInfo
	{
		get
		{
			return _ت;
		}
		set
		{
			if (value != _ت)
			{
				_ت = value;
				RefreshItems();
				OnFormatInfoChanged(EventArgs.Empty);
			}
		}
	}

	[DefaultValue("")]
	[Description("Indicates comma separated field or property names that holds the value that is used to identify node and parent node. Format expected is: FieldNodeId,ParentNodeFieldId. For example if your table represents departments, you have DepartmentId field which uniquely identifies a department and ParentDepartmentId field which identifies parent of the department if any you would set this property to DepartmentId,ParentDepartmentId.")]
	[Category("Data")]
	public string ParentFieldNames
	{
		get
		{
			return _خ;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			if (!string.IsNullOrEmpty(value))
			{
				string[] array = value.Split(new char[1] { ',' });
				if (array.Length != 2)
				{
					throw new ArgumentException("ParentFieldNames excepts two and only two fields/property names separated by comma character.");
				}
				if (string.IsNullOrEmpty(array[0]))
				{
					throw new ArgumentException("ParentFieldNames excepts two and only two fields/property names separated by comma character. First field name is empty.");
				}
				if (string.IsNullOrEmpty(array[1]))
				{
					throw new ArgumentException("ParentFieldNames excepts two and only two fields/property names separated by comma character. Second field name is empty.");
				}
			}
			_خ = value;
			OnParentFieldNamesChanged();
		}
	}

	[Description("Indicates comma separated list of field or property names that are used for grouping when data-binding is used")]
	[Category("Data")]
	[DefaultValue("")]
	public string GroupingMembers
	{
		get
		{
			return _د;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			_د = value;
			OnGroupingMembersChanged();
		}
	}

	[Browsable(true)]
	[Category("Node Style")]
	[Description("Gets or sets default style for the node.")]
	[DefaultValue(null)]
	[Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	public ElementStyle GroupNodeStyle
	{
		get
		{
			return _ذ;
		}
		set
		{
			if (_ذ != value)
			{
				_ذ = value;
				if (_ر != null && __28AB.Nodes != null)
				{
					RefreshItems();
				}
			}
		}
	}

	private bool Մ => true;

	protected CurrencyManager DataManager => _ر;

	[Description("Gets or sets the index specifying the currently selected item.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public int SelectedIndex
	{
		get
		{
			if (_ر != null)
			{
				if (__28AB.SelectedNode == null || __28AB.SelectedNode.BindingIndex < 0)
				{
					return -1;
				}
				return __28AB.SelectedNode.BindingIndex;
			}
			if (__28AB.SelectedNode == null)
			{
				return -1;
			}
			return __28AB.GetNodeFlatIndex(__28AB.SelectedNode);
		}
		set
		{
			_۷(value);
		}
	}

	[Category("Data")]
	[DefaultValue("")]
	[Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version= 2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[Description("property to use as the actual value for the items in the control. Applies to data-binding scenarios. SelectedValue property will return the value of selected node as indicated by this property.")]
	public string ValueMember
	{
		get
		{
			return ((BindingMemberInfo)(ref _ض)).BindingMember;
		}
		set
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			if (value == null)
			{
				value = "";
			}
			BindingMemberInfo val = default(BindingMemberInfo);
			((BindingMemberInfo)(ref val))._002Ector(value);
			if (!((object)(BindingMemberInfo)(ref val)).Equals((object?)_ض))
			{
				if (DisplayMembers.Length == 0 && Columns.Count == 0)
				{
					List<BindingMemberInfo> list = new List<BindingMemberInfo>();
					list.Add(val);
					_06DA(DataSource, list, _06DD: false);
				}
				if (_ر != null && value != null && value.Length != 0 && !_06E0(val))
				{
					throw new ArgumentException("Invalid value for ValueMember", "value");
				}
				_ض = val;
				OnValueMemberChanged(EventArgs.Empty);
				OnSelectedValueChanged(EventArgs.Empty);
			}
		}
	}

	[Bindable(true)]
	[Category("Data")]
	[Description("Indicates value of the member property specified by the ValueMember property.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[DefaultValue(null)]
	public object SelectedValue
	{
		get
		{
			if (_ر != null && SelectedIndex != -1)
			{
				object item = _ر.List[SelectedIndex];
				return GetPropertyValue(item, ((BindingMemberInfo)(ref _ض)).BindingField);
			}
			return null;
		}
		set
		{
			if (_ر != null)
			{
				string bindingField = ((BindingMemberInfo)(ref _ض)).BindingField;
				if (string.IsNullOrEmpty(bindingField))
				{
					throw new InvalidOperationException("ValueMember property must be set to be able to set SelectedValue");
				}
				PropertyDescriptor propertyDescriptor = ((BindingManagerBase)_ر).GetItemProperties().Find(bindingField, ignoreCase: true);
				MethodInfo method = ((object)_ر).GetType().GetMethod("Find", BindingFlags.Instance | BindingFlags.NonPublic);
				int selectedIndex = -1;
				if ((object)method != null)
				{
					selectedIndex = (int)method.Invoke(_ر, new object[3] { propertyDescriptor, value, true });
				}
				SelectedIndex = selectedIndex;
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(null)]
	[Description("Indicates watermark font.")]
	[Browsable(true)]
	public virtual Font WatermarkFont
	{
		get
		{
			return _ᠣ;
		}
		set
		{
			_ᠣ = value;
			((Control)this).Invalidate();
		}
	}

	[Description("Indicates watermark text color.")]
	[Browsable(true)]
	[Category("Appearance")]
	public virtual Color WatermarkColor
	{
		get
		{
			return _ᠤ;
		}
		set
		{
			_ᠤ = value;
			((Control)this).Invalidate();
		}
	}

	[Description("Indicates whether watermark text is displayed if set for the input items.")]
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

	[Localizable(true)]
	[Description("Indicates watermark text displayed on the input control when control is empty.")]
	[Category("Watermark")]
	[DefaultValue("")]
	public string WatermarkText
	{
		get
		{
			return _ᠥ;
		}
		set
		{
			if (value != null)
			{
				_ᠥ = value;
				((Control)this).Invalidate();
			}
		}
	}

	[DefaultValue(eTextAlignment.Left)]
	[Browsable(true)]
	[Category("Watermark")]
	[Description("Indicates watermark text alignment.")]
	public eTextAlignment WatermarkAlignment
	{
		get
		{
			return _ᠦ;
		}
		set
		{
			if (_ᠦ != value)
			{
				_ᠦ = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Description("Indicates whether FocusHighlightColor is used as background color to highlight text box when it has input focus.")]
	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(false)]
	public bool FocusHighlightEnabled
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
					((Control)this).Invalidate(true);
				}
			}
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[Description("Indicates color used as background color to highlight text box when it has input focus and focus highlight is enabled.")]
	public Color FocusHighlightColor
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
					((Control)this).Invalidate(true);
				}
			}
		}
	}

	[Description("Gets or sets control background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Style")]
	public ElementStyle BackgroundStyle => _ᠩ;

	[Category("Buttons")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Describes the settings for the button that shows drop-down when clicked.")]
	public InputButtonSettings ButtonDropDown => __17F1;

	[Description("Describes the settings for the button that clears the content of the control when clicked.")]
	[Category("Buttons")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonClear => __17F2;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[Category("Buttons")]
	public InputButtonSettings ButtonCustom => _ᠧ;

	[Category("Buttons")]
	[Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public InputButtonSettings ButtonCustom2 => _ᠨ;

	protected virtual bool IsWatermarkRendered
	{
		get
		{
			if (!((Control)this).Focused)
			{
				return SelectedNode == null;
			}
			return false;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether Enter key when pressed while control has input focus toggles the popup.")]
	[Category("Behavior")]
	public bool EnterKeyTogglesPopup
	{
		get
		{
			return __28AE;
		}
		set
		{
			__28AE = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Indicates whether keyboard incremental search is enabled.")]
	public bool KeyboardSearchEnabled
	{
		get
		{
			return __05F7;
		}
		set
		{
			__05F7 = value;
		}
	}

	[Category("Behavior")]
	[Description("Indicates whether during keyboard search selected node can be set to nothing/null if there is no match found.")]
	[DefaultValue(true)]
	public bool KeyboardSearchNoSelectionAllowed
	{
		get
		{
			return __28AF;
		}
		set
		{
			__28AF = value;
		}
	}

	[DefaultValue(1000)]
	[Category("Behavior")]
	[Description("Indicates keyboard search buffer expiration timeout.")]
	public int SearchBufferExpireTimeout
	{
		get
		{
			return __05F9;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			__05F9 = value;
		}
	}

	[DefaultValue(null)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Indicates the custom popup location for the ComboTree popup")]
	[Category("Behavior")]
	[Browsable(false)]
	public Point? PopupLocation
	{
		get
		{
			return __28B2;
		}
		set
		{
			if (value != __28B2)
			{
				Point? oldValue = __28B2;
				__28B2 = value;
				OnPopupLocationChanged(oldValue, value);
			}
		}
	}

	[DefaultValue(null)]
	[Description("Indicates reference of the control that will be displayed on popup that is shown when drop-down button is clicked.")]
	internal Control _28AA
	{
		get
		{
			return __22C3;
		}
		set
		{
			__22C3 = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public SubItemsCollection DropDownItems => _០.SubItems;

	private bool ᵸ
	{
		get
		{
			if ((_ᠧ == null || !_ᠧ.Visible) && (_ᠨ == null || !_ᠨ.Visible) && (__17F1 == null || !__17F1.Visible))
			{
				if (__17F2 != null)
				{
					return __17F2.Visible;
				}
				return false;
			}
			return true;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DevComponents.AdvTree.AdvTree AdvTree => __28AB;

	[Category("Behavior")]
	[Description("Indicates height in pixels of the drop-down portion of the ComboTreeBox control.")]
	[DefaultValue(0)]
	public int DropDownHeight
	{
		get
		{
			return __28B3;
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentException("DropDownHeight Value must be equal or greater than zero.");
			}
			__28B3 = value;
			_28BF();
		}
	}

	[DefaultValue(0)]
	[Category("Behavior")]
	[Description("Indicates width in pixels of the drop-down portion of the ComboTreeBox control.")]
	public int DropDownWidth
	{
		get
		{
			return __28B4;
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentException("DropDownWidth Value must be equal or greater than zero.");
			}
			__28B4 = value;
			_28BF();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool IsPopupOpen
	{
		get
		{
			return _០.Expanded;
		}
		set
		{
			if (value != _០.Expanded)
			{
				if (value)
				{
					ShowDropDown();
				}
				else
				{
					CloseDropDown();
				}
			}
		}
	}

	[Description("Gets the collection of tree nodes that are assigned to the tree control.")]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public NodeCollection Nodes => __28AB.Nodes;

	[Description("Gets collection of column headers that appear in the popup tree.")]
	[Category("Columns")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ColumnHeaderCollection Columns => __28AB.Columns;

	[DefaultValue(true)]
	[Description("Indicates whether column headers are visible if they are defined through Columns collection.")]
	[Category("Columns")]
	public bool ColumnsVisible
	{
		get
		{
			return __28AB.ColumnsVisible;
		}
		set
		{
			__28AB.ColumnsVisible = value;
		}
	}

	[DefaultValue(true)]
	[Category("Columns")]
	[Description("Indicates whether grid lines are displayed when columns are defined.")]
	public bool GridColumnLines
	{
		get
		{
			return __28AB.GridColumnLines;
		}
		set
		{
			__28AB.GridColumnLines = value;
		}
	}

	[Category("Columns")]
	[Description("Indicates grid lines color.")]
	public Color GridLinesColor
	{
		get
		{
			return __28AB.GridLinesColor;
		}
		set
		{
			__28AB.GridLinesColor = value;
		}
	}

	[Description("")]
	[Category("Columns")]
	[DefaultValue(false)]
	public bool GridRowLines
	{
		get
		{
			return __28AB.GridRowLines;
		}
		set
		{
			__28AB.GridRowLines = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates whether node is highlighted when mouse enters the node.")]
	[DefaultValue(true)]
	public bool HotTracking
	{
		get
		{
			return __28AB.HotTracking;
		}
		set
		{
			__28AB.HotTracking = value;
		}
	}

	[DefaultValue(eSelectionStyle.HighlightCells)]
	[Category("Selection")]
	[Description("Indicates node selection box style of popup tree control.")]
	public eSelectionStyle SelectionBoxStyle
	{
		get
		{
			return __28AB.SelectionBoxStyle;
		}
		set
		{
			__28AB.SelectionBoxStyle = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Images")]
	[Description("Indicates the ImageList that contains the Image objects used by the tree nodes.")]
	public ImageList ImageList
	{
		get
		{
			return __28AB.ImageList;
		}
		set
		{
			__28AB.ImageList = value;
		}
	}

	[DefaultValue(-1)]
	[Editor("DevComponents.AdvTree.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Category("Images")]
	[Browsable(true)]
	[Description("Indicates the image-list index value of the default image that is displayed by the tree nodes.")]
	[TypeConverter(typeof(ImageIndexConverter))]
	public int ImageIndex
	{
		get
		{
			return __28AB.ImageIndex;
		}
		set
		{
			__28AB.ImageIndex = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates control background color when control is disabled")]
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

	[Category("Commands")]
	[Description("Indicates the command assigned to the item.")]
	[DefaultValue(null)]
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

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

	[Localizable(true)]
	[Description("Indicates user defined data value that can be passed to the command when it is executed.")]
	[Browsable(true)]
	[Category("Commands")]
	[TypeConverter(typeof(StringConverter))]
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

	public event CancelEventHandler ButtonClearClick
	{
		add
		{
			CancelEventHandler cancelEventHandler = _៦;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _៦, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = _៦;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _៦, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	public event CancelEventHandler ButtonDropDownClick
	{
		add
		{
			CancelEventHandler cancelEventHandler = _៧;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _៧, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = _៧;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _៧, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	public event EventHandler ButtonCustomClick
	{
		add
		{
			EventHandler eventHandler = _181E;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _181E, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _181E;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _181E, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler ButtonCustom2Click
	{
		add
		{
			EventHandler eventHandler = _181F;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _181F, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _181F;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _181F, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when the text alignment in text box has changed.")]
	public event EventHandler TextAlignChanged
	{
		add
		{
			EventHandler eventHandler = _28AC;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _28AC, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _28AC;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _28AC, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

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

	public event AdvTreeNodeEventHandler SelectionChanged
	{
		add
		{
			AdvTreeNodeEventHandler advTreeNodeEventHandler = _05B1;
			AdvTreeNodeEventHandler advTreeNodeEventHandler2;
			do
			{
				advTreeNodeEventHandler2 = advTreeNodeEventHandler;
				AdvTreeNodeEventHandler value2 = (AdvTreeNodeEventHandler)Delegate.Combine(advTreeNodeEventHandler2, value);
				advTreeNodeEventHandler = Interlocked.CompareExchange(ref _05B1, value2, advTreeNodeEventHandler2);
			}
			while ((object)advTreeNodeEventHandler != advTreeNodeEventHandler2);
		}
		remove
		{
			AdvTreeNodeEventHandler advTreeNodeEventHandler = _05B1;
			AdvTreeNodeEventHandler advTreeNodeEventHandler2;
			do
			{
				advTreeNodeEventHandler2 = advTreeNodeEventHandler;
				AdvTreeNodeEventHandler value2 = (AdvTreeNodeEventHandler)Delegate.Remove(advTreeNodeEventHandler2, value);
				advTreeNodeEventHandler = Interlocked.CompareExchange(ref _05B1, value2, advTreeNodeEventHandler2);
			}
			while ((object)advTreeNodeEventHandler != advTreeNodeEventHandler2);
		}
	}

	[Description("Occurs when the DataSource changes.")]
	public event EventHandler DataSourceChanged
	{
		add
		{
			EventHandler eventHandler = _05C1;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05C1, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _05C1;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05C1, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when the DisplayMembers property changes")]
	public event EventHandler DisplayMembersChanged
	{
		add
		{
			EventHandler eventHandler = _05C2;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05C2, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _05C2;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05C2, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when the control is bound to a data value that need to be converted.")]
	public event TreeConvertEventHandler Format
	{
		add
		{
			TreeConvertEventHandler treeConvertEventHandler = _05C3;
			TreeConvertEventHandler treeConvertEventHandler2;
			do
			{
				treeConvertEventHandler2 = treeConvertEventHandler;
				TreeConvertEventHandler value2 = (TreeConvertEventHandler)Delegate.Combine(treeConvertEventHandler2, value);
				treeConvertEventHandler = Interlocked.CompareExchange(ref _05C3, value2, treeConvertEventHandler2);
			}
			while ((object)treeConvertEventHandler != treeConvertEventHandler2);
		}
		remove
		{
			TreeConvertEventHandler treeConvertEventHandler = _05C3;
			TreeConvertEventHandler treeConvertEventHandler2;
			do
			{
				treeConvertEventHandler2 = treeConvertEventHandler;
				TreeConvertEventHandler value2 = (TreeConvertEventHandler)Delegate.Remove(treeConvertEventHandler2, value);
				treeConvertEventHandler = Interlocked.CompareExchange(ref _05C3, value2, treeConvertEventHandler2);
			}
			while ((object)treeConvertEventHandler != treeConvertEventHandler2);
		}
	}

	[Description("Occurs when FormattingEnabled property changes.")]
	public event EventHandler FormattingEnabledChanged
	{
		add
		{
			EventHandler eventHandler = _05C4;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05C4, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _05C4;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05C4, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when FormatString property changes.")]
	public event EventHandler FormatStringChanged
	{
		add
		{
			EventHandler eventHandler = _05C5;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05C5, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _05C5;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05C5, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when FormatInfo property has changed.")]
	public event EventHandler FormatInfoChanged
	{
		add
		{
			EventHandler eventHandler = _05C6;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05C6, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _05C6;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05C6, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when a Node for an data-bound object item has been created and provides you with opportunity to modify the node")]
	public event DataNodeEventHandler DataNodeCreated
	{
		add
		{
			DataNodeEventHandler dataNodeEventHandler = _05C7;
			DataNodeEventHandler dataNodeEventHandler2;
			do
			{
				dataNodeEventHandler2 = dataNodeEventHandler;
				DataNodeEventHandler value2 = (DataNodeEventHandler)Delegate.Combine(dataNodeEventHandler2, value);
				dataNodeEventHandler = Interlocked.CompareExchange(ref _05C7, value2, dataNodeEventHandler2);
			}
			while ((object)dataNodeEventHandler != dataNodeEventHandler2);
		}
		remove
		{
			DataNodeEventHandler dataNodeEventHandler = _05C7;
			DataNodeEventHandler dataNodeEventHandler2;
			do
			{
				dataNodeEventHandler2 = dataNodeEventHandler;
				DataNodeEventHandler value2 = (DataNodeEventHandler)Delegate.Remove(dataNodeEventHandler2, value);
				dataNodeEventHandler = Interlocked.CompareExchange(ref _05C7, value2, dataNodeEventHandler2);
			}
			while ((object)dataNodeEventHandler != dataNodeEventHandler2);
		}
	}

	[Description("Occurs when a group Node is created as result of GroupingMembers property setting and provides you with opportunity to modify the node")]
	public event DataNodeEventHandler GroupNodeCreated
	{
		add
		{
			DataNodeEventHandler dataNodeEventHandler = _05C8;
			DataNodeEventHandler dataNodeEventHandler2;
			do
			{
				dataNodeEventHandler2 = dataNodeEventHandler;
				DataNodeEventHandler value2 = (DataNodeEventHandler)Delegate.Combine(dataNodeEventHandler2, value);
				dataNodeEventHandler = Interlocked.CompareExchange(ref _05C8, value2, dataNodeEventHandler2);
			}
			while ((object)dataNodeEventHandler != dataNodeEventHandler2);
		}
		remove
		{
			DataNodeEventHandler dataNodeEventHandler = _05C8;
			DataNodeEventHandler dataNodeEventHandler2;
			do
			{
				dataNodeEventHandler2 = dataNodeEventHandler;
				DataNodeEventHandler value2 = (DataNodeEventHandler)Delegate.Remove(dataNodeEventHandler2, value);
				dataNodeEventHandler = Interlocked.CompareExchange(ref _05C8, value2, dataNodeEventHandler2);
			}
			while ((object)dataNodeEventHandler != dataNodeEventHandler2);
		}
	}

	[Description("Occurs when value of ValueMember property has changed.")]
	public event EventHandler ValueMemberChanged
	{
		add
		{
			EventHandler eventHandler = _05C9;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05C9, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _05C9;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05C9, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when value of SelectedValue property has changed.")]
	public event EventHandler SelectedValueChanged
	{
		add
		{
			EventHandler eventHandler = _05CA;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05CA, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _05CA;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05CA, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when value of SelectedValue property has changed.")]
	public event EventHandler SelectedIndexChanged
	{
		add
		{
			EventHandler eventHandler = _05CB;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05CB, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _05CB;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05CB, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when ColumnHeader is automatically created by control as result of data binding and provides you with opportunity to modify it.")]
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

	public ComboTree()
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).SetStyle((ControlStyles)512, true);
		კ();
		((Control)this).BackColor = SystemColors.Window;
	}

	private void კ()
	{
		_ᠩ.SetColorScheme(base.ColorScheme);
		_ᠩ.StyleChanged += ઐ;
		_ᠧ = new InputButtonSettings(this);
		_ᠨ = new InputButtonSettings(this);
		__17F2 = new InputButtonSettings(this);
		__17F1 = new InputButtonSettings(this);
		ᶑ();
		__28AB = new DevComponents.AdvTree.AdvTree();
		__28AB.BackgroundStyle.Reset();
		__28AB.BackgroundStyle.BackColor = SystemColors.Window;
		__28AB.BackgroundStyle.Border = eStyleBorderType.None;
		__28AB.ExpandButtonType = eExpandButtonType.Triangle;
		__28AB.DragDropEnabled = false;
		__28AB.HotTracking = true;
		__28AB.Indent = 6;
		__28AB.SelectionFocusAware = false;
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Name = "nodeElementStyle";
		elementStyle.TextColor = SystemColors.ControlText;
		__28AB.NodeStyle = elementStyle;
		__28AB.PathSeparator = ";";
		__28AB.Styles.Add(elementStyle);
		__28AB.AfterNodeSelect += ᜅ;
		__28AB.BeforeNodeSelect += ᜆ;
		__28AB.NodeMouseDown += ᜃ;
		_28AA = (Control)(object)__28AB;
	}

	protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		base.ScaleControl(factor, specified);
		if (_ᶇ != null)
		{
			_ᶇ.InvalidateArrange();
		}
	}

	protected override void OnBackColorChanged(EventArgs e)
	{
		__28AB.BackgroundStyle.BackColor = ((Control)this).BackColor;
		((Control)this).OnBackColorChanged(e);
	}

	protected override void InvalidateAutoSize()
	{
		if (__28AB != null)
		{
			__28AB.AntiAlias = base.AntiAlias;
		}
	}

	private void ᜃ(object _٠, TreeNodeMouseEventArgs ؿ)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		if (SelectionClosesPopup && (int)ؿ.Button == 1048576 && ؿ.Node != null && ؿ.Node.IsSelected && IsPopupOpen && !ؿ.Node.ExpandPartRectangle.Contains(ؿ.X, ؿ.Y))
		{
			IsPopupOpen = false;
		}
	}

	private void ᜆ(object _٠, AdvTreeNodeCancelEventArgs ؿ)
	{
		if (ᛗ != null)
		{
			ᛗ(this, ؿ);
		}
	}

	private void ᜅ(object _٠, AdvTreeNodeEventArgs ؿ)
	{
		if (_05B1 != null)
		{
			_05B1(this, ؿ);
		}
		if (_ر != null)
		{
			Node selectedNode = __28AB.SelectedNode;
			if (selectedNode != null && selectedNode.BindingIndex > -1 && ((BindingManagerBase)_ر).Position != selectedNode.BindingIndex)
			{
				((BindingManagerBase)_ر).Position = selectedNode.BindingIndex;
			}
		}
		if (SelectionClosesPopup && IsPopupOpen && ؿ.Action == eTreeAction.Mouse)
		{
			IsPopupOpen = false;
		}
		if (ؿ.Node != null)
		{
			((Control)this).Text = ؿ.Node.ToString();
		}
		((Control)this).Invalidate();
		OnSelectedIndexChanged(EventArgs.Empty);
		if (!string.IsNullOrEmpty(ValueMember))
		{
			OnSelectedValueChanged(EventArgs.Empty);
		}
	}

	protected virtual void OnFormattingEnabledChanged(EventArgs e)
	{
		_05C4?.Invoke(this, e);
	}

	protected virtual void OnFormatStringChanged(EventArgs e)
	{
		_05C5?.Invoke(this, e);
	}

	protected virtual void OnFormatInfoChanged(EventArgs e)
	{
		_05C6?.Invoke(this, e);
	}

	private void _06DA(object _06DB, List<BindingMemberInfo> _06DC, bool _06DD)
	{
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		bool flag = _ا != _06DB;
		bool flag2 = _ئ != _06DC;
		if (_ث)
		{
			return;
		}
		try
		{
			if (_06DD || flag || flag2)
			{
				_ث = true;
				IList list = ((DataManager != null) ? DataManager.List : null);
				bool flag3 = DataManager == null;
				_۱();
				_ا = _06DB;
				_ئ = _06DC;
				_۳();
				if (_س)
				{
					CurrencyManager val = null;
					if (_06DB != null && ((Control)this).BindingContext != null && _06DB != Convert.DBNull)
					{
						string text = "";
						if (_ئ != null && _ئ.Count > 0)
						{
							BindingMemberInfo val2 = _06DC[0];
							text = ((BindingMemberInfo)(ref val2)).BindingPath;
						}
						val = (CurrencyManager)((Control)this).BindingContext[_06DB, text];
					}
					if (_ر != val)
					{
						if (_ر != null)
						{
							_ر.ItemChanged -= new ItemChangedEventHandler(_06E4);
							((BindingManagerBase)_ر).PositionChanged -= ۥ;
						}
						_ر = val;
						if (_ر != null)
						{
							_ر.ItemChanged += new ItemChangedEventHandler(_06E4);
							((BindingManagerBase)_ر).PositionChanged += ۥ;
						}
					}
					if (_ر != null && (flag2 || flag) && !_06DE(_ئ))
					{
						throw new ArgumentException("Wrong DisplayMembers parameter", "newDisplayMember");
					}
					if (_ر != null && (flag || flag2 || _06DD) && (flag2 || (_06DD && (list != _ر.List || flag3 || flag))))
					{
						_06E4(_ر, null);
					}
				}
				_ج.Clear();
			}
			if (flag)
			{
				OnDataSourceChanged(EventArgs.Empty);
			}
			if (flag2)
			{
				OnDisplayMembersChanged(EventArgs.Empty);
			}
		}
		finally
		{
			_ث = false;
		}
	}

	private bool _06DE(List<BindingMemberInfo> _06DF)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		if (_06DF == null || _06DF.Count == 0)
		{
			return true;
		}
		foreach (BindingMemberInfo item in _06DF)
		{
			BindingMemberInfo current = item;
			if (((BindingMemberInfo)(ref current)).BindingMember != null && !_06E0(current))
			{
				return false;
			}
		}
		return true;
	}

	private bool _06E0(BindingMemberInfo _06E1)
	{
		if (_ر != null)
		{
			PropertyDescriptorCollection itemProperties = ((BindingManagerBase)_ر).GetItemProperties();
			int count = itemProperties.Count;
			for (int i = 0; i < count; i++)
			{
				if (!typeof(IList).IsAssignableFrom(itemProperties[i].PropertyType) && itemProperties[i].Name.Equals(((BindingMemberInfo)(ref _06E1)).BindingField))
				{
					return true;
				}
			}
			for (int j = 0; j < count; j++)
			{
				if (!typeof(IList).IsAssignableFrom(itemProperties[j].PropertyType) && string.Compare(itemProperties[j].Name, ((BindingMemberInfo)(ref _06E1)).BindingField, ignoreCase: true, CultureInfo.CurrentCulture) == 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	protected virtual void OnDataSourceChanged(EventArgs e)
	{
		_05C1?.Invoke(this, e);
	}

	protected virtual void OnDisplayMembersChanged(EventArgs e)
	{
		_05C2?.Invoke(this, e);
	}

	private TypeConverter _06E2(string _06E3)
	{
		if (_ج.ContainsKey(_06E3))
		{
			return (TypeConverter)_ج[_06E3];
		}
		if (DataManager != null)
		{
			PropertyDescriptorCollection itemProperties = ((BindingManagerBase)DataManager).GetItemProperties();
			if (itemProperties != null)
			{
				PropertyDescriptor propertyDescriptor = itemProperties.Find(_06E3, ignoreCase: true);
				if (propertyDescriptor != null)
				{
					_ج.Add(_06E3, propertyDescriptor.Converter);
					return propertyDescriptor.Converter;
				}
			}
		}
		return null;
	}

	private void _06E4(object _٠, ItemChangedEventArgs ؿ)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		if (((Control)this).InvokeRequired)
		{
			((Control)this).Invoke((Delegate)new ItemChangedEventHandler(_06E4), new object[2] { _٠, ؿ });
		}
		else
		{
			if (_ر == null)
			{
				return;
			}
			if (ؿ == null || ؿ.Index == -1)
			{
				SetItemsCore(_ر.List);
				if (Մ)
				{
					if (((BindingManagerBase)_ر).Position == -1)
					{
						SelectedNode = null;
					}
					else
					{
						SelectedNode = __28AB.FindNodeByBindingIndex(((BindingManagerBase)_ر).Position);
					}
				}
			}
			else
			{
				SetItemCore(ؿ.Index, _ر.List[ؿ.Index]);
			}
		}
	}

	private void ۥ(object _٠, EventArgs ؿ)
	{
		if (_ر != null && Մ)
		{
			if (((BindingManagerBase)_ر).Position == -1)
			{
				SelectedNode = null;
			}
			else
			{
				SelectedNode = __28AB.FindNodeByBindingIndex(((BindingManagerBase)_ر).Position);
			}
		}
	}

	public virtual void RefreshItems()
	{
		if (_ر == null)
		{
			return;
		}
		SetItemsCore(_ر.List);
		if (Մ)
		{
			if (((BindingManagerBase)_ر).Position == -1)
			{
				SelectedNode = null;
			}
			else
			{
				SelectedNode = __28AB.FindNodeByBindingIndex(((BindingManagerBase)_ر).Position);
			}
		}
	}

	private bool ۦ(object _06E7)
	{
		if (_06E7 == null)
		{
			return false;
		}
		if (!(_06E7 is string))
		{
			return _06E7.GetType().IsPrimitive;
		}
		return true;
	}

	protected virtual void SetItemsCore(IList items)
	{
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		if (((Component)(object)this).DesignMode)
		{
			return;
		}
		__28AB.BeginUpdate();
		__28AB.ClearAndDisposeAllNodes();
		bool flag = !string.IsNullOrEmpty(_د);
		List<string> list = new List<string>();
		if (string.IsNullOrEmpty(DisplayMembers))
		{
			if (__28AB.Columns.Count > 0)
			{
				foreach (ColumnHeader column in __28AB.Columns)
				{
					if (!string.IsNullOrEmpty(column.DataFieldName))
					{
						list.Add(column.DataFieldName);
					}
				}
			}
			if (list.Count == 0)
			{
				__28AB.Columns.Clear();
				if (_ر.List is Array)
				{
					ColumnHeader columnHeader2 = new ColumnHeader("Items");
					columnHeader2.Width.Relative = 100;
					__28AB.Columns.Add(columnHeader2);
					OnDataColumnCreated(new DataColumnEventArgs(columnHeader2));
				}
				else if (_ر != null)
				{
					if (((BindingManagerBase)_ر).Current != null && ۦ(((BindingManagerBase)_ر).Current))
					{
						ColumnHeader columnHeader3 = new ColumnHeader();
						columnHeader3.Width.Relative = 100;
						__28AB.Columns.Add(columnHeader3);
						OnDataColumnCreated(new DataColumnEventArgs(columnHeader3));
					}
					else
					{
						PropertyDescriptorCollection itemProperties = ((BindingManagerBase)_ر).GetItemProperties();
						foreach (PropertyDescriptor item in itemProperties)
						{
							list.Add(item.Name);
							ColumnHeader columnHeader4 = new ColumnHeader(_26E6._26E8(item.Name));
							columnHeader4.DataFieldName = item.Name;
							columnHeader4.Width.Relative = Math.Max(10, 100 / itemProperties.Count);
							__28AB.Columns.Add(columnHeader4);
							OnDataColumnCreated(new DataColumnEventArgs(columnHeader4));
						}
					}
				}
			}
		}
		else
		{
			__28AB.Columns.Clear();
			if (_ئ != null && _ئ.Count > 0)
			{
				foreach (BindingMemberInfo item2 in _ئ)
				{
					BindingMemberInfo current = item2;
					ColumnHeader columnHeader5 = new ColumnHeader(_26E6._26E8(((BindingMemberInfo)(ref current)).BindingMember));
					columnHeader5.Tag = current;
					columnHeader5.Width.Relative = Math.Max(10, 100 / _ئ.Count);
					__28AB.Columns.Add(columnHeader5);
					list.Add(((BindingMemberInfo)(ref current)).BindingMember);
					OnDataColumnCreated(new DataColumnEventArgs(columnHeader5));
				}
			}
		}
		if (string.IsNullOrEmpty(_د) && string.IsNullOrEmpty(_خ))
		{
			for (int i = 0; i < items.Count; i++)
			{
				object obj = items[i];
				_06EB(__28AB.Nodes, obj, i, list);
			}
		}
		else if (!string.IsNullOrEmpty(_خ))
		{
			flag = true;
			Dictionary<string, Node> dictionary = new Dictionary<string, Node>();
			Dictionary<string, List<Node>> dictionary2 = new Dictionary<string, List<Node>>();
			string[] array = _خ.Split(new char[1] { ',' });
			for (int j = 0; j < items.Count; j++)
			{
				object obj2 = items[j];
				string itemText = GetItemText(obj2, array[0]);
				string itemText2 = GetItemText(obj2, array[1]);
				Node value = null;
				if (dictionary.TryGetValue(itemText2, out value))
				{
					Node value2 = _06EB(value.Nodes, obj2, j, list);
					dictionary.Add(itemText, value2);
					continue;
				}
				Node node = _06EB(Nodes, obj2, j, list);
				List<Node> value3 = null;
				if (!dictionary2.TryGetValue(itemText2, out value3))
				{
					value3 = new List<Node>();
					dictionary2.Add(itemText2, value3);
				}
				value3.Add(node);
				dictionary.Add(itemText, node);
			}
			foreach (KeyValuePair<string, List<Node>> item3 in dictionary2)
			{
				Node value4 = null;
				if (!dictionary.TryGetValue(item3.Key, out value4))
				{
					continue;
				}
				foreach (Node item4 in item3.Value)
				{
					item4.Remove();
					value4.Nodes.Add(item4);
				}
			}
		}
		else
		{
			string[] array2 = _د.Split(new char[1] { ',' });
			for (int k = 0; k < array2.Length; k++)
			{
				array2[k] = array2[k].Trim();
			}
			Dictionary<string, Node> dictionary3 = new Dictionary<string, Node>();
			for (int l = 0; l < items.Count; l++)
			{
				object obj3 = items[l];
				NodeCollection nodes = __28AB.Nodes;
				string text = "";
				for (int m = 0; m < array2.Length; m++)
				{
					string itemText3 = GetItemText(obj3, array2[m]);
					text = text + itemText3.ToLower() + "/";
					Node value5 = null;
					if (!dictionary3.TryGetValue(text, out value5))
					{
						value5 = _06E8(nodes, itemText3);
						dictionary3.Add(text, value5);
					}
					nodes = value5.Nodes;
				}
				_06EB(nodes, obj3, l, list);
			}
		}
		if (flag)
		{
			__28AB.ExpandWidth = 14;
		}
		else
		{
			__28AB.ExpandWidth = 0;
		}
		__28AB.EndUpdate();
	}

	protected virtual void OnDataColumnCreated(DataColumnEventArgs args)
	{
		if (_05CC != null)
		{
			_05CC(this, args);
		}
	}

	private Node _06E8(NodeCollection _06E9, string _06EA)
	{
		Node node = new Node();
		node.Text = _06EA;
		node.Style = _ذ;
		node.Expanded = true;
		node.Selectable = false;
		_06E9.Add(node);
		DataNodeEventArgs dataNodeEventArgs = new DataNodeEventArgs(node, null);
		OnGroupNodeCreated(dataNodeEventArgs);
		return node;
	}

	protected virtual void OnGroupNodeCreated(DataNodeEventArgs dataNodeEventArgs)
	{
		if (_05C8 != null)
		{
			_05C8(this, dataNodeEventArgs);
		}
	}

	private Node _06EB(NodeCollection _06E9, object _06EC, int _06ED, List<string> ۮ)
	{
		Node node = new Node();
		_06E9.Add(node);
		ۯ(node, _06EC, ۮ, _06ED);
		DataNodeEventArgs dataNodeEventArgs = new DataNodeEventArgs(node, _06EC);
		OnDataNodeCreated(dataNodeEventArgs);
		return dataNodeEventArgs.Node;
	}

	private void ۯ(Node _0653, object _06EC, List<string> ۮ, int _۰)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		_0653.DataKey = _06EC;
		_0653.BindingIndex = _۰;
		_0653.CreateCells();
		if (ۮ.Count > 0)
		{
			for (int i = 0; i < ۮ.Count; i++)
			{
				object propertyValue = GetPropertyValue(_06EC, ۮ[i]);
				if (propertyValue is Image)
				{
					_0653.Cells[i].Images.Image = (Image)propertyValue;
				}
				else
				{
					_0653.Cells[i].Text = GetItemText(_06EC, ۮ[i]);
				}
			}
		}
		else if (_06EC != null)
		{
			_0653.Text = _06EC.ToString();
		}
	}

	protected virtual void OnDataNodeCreated(DataNodeEventArgs dataNodeEventArgs)
	{
		if (_05C7 != null)
		{
			_05C7(this, dataNodeEventArgs);
		}
	}

	protected virtual void SetItemCore(int index, object value)
	{
		Node node = __28AB.FindNodeByBindingIndex(index);
		if (node == null)
		{
			return;
		}
		List<string> list = new List<string>();
		foreach (ColumnHeader column in __28AB.Columns)
		{
			if (!string.IsNullOrEmpty(column.DataFieldName))
			{
				list.Add(column.DataFieldName);
			}
		}
		ۯ(node, value, list, index);
	}

	protected virtual void OnParentFieldNamesChanged()
	{
		RefreshItems();
	}

	protected virtual void OnGroupingMembersChanged()
	{
		RefreshItems();
	}

	private void _۱()
	{
		if (_ا is IComponent)
		{
			((IComponent)_ا).Disposed -= _۲;
		}
		if (_ا is ISupportInitializeNotification supportInitializeNotification && _ز)
		{
			supportInitializeNotification.Initialized -= _۴;
			_ز = false;
		}
	}

	private void _۲(object _٠, EventArgs ؿ)
	{
		_06DA(null, null, _06DD: true);
	}

	private void _۳()
	{
		if (_ا is IComponent)
		{
			((IComponent)_ا).Disposed += _۲;
		}
		if (_ا is ISupportInitializeNotification { IsInitialized: false } supportInitializeNotification)
		{
			supportInitializeNotification.Initialized += _۴;
			_ز = true;
			_س = false;
		}
		else
		{
			_س = true;
		}
	}

	private void _۴(object _٠, EventArgs ؿ)
	{
		_06DA(_ا, _ئ, _06DD: true);
	}

	protected virtual void OnFormat(TreeConvertEventArgs e)
	{
		_05C3?.Invoke(this, e);
	}

	public string GetItemText(object item, string fieldName)
	{
		object propertyValue = GetPropertyValue(item, fieldName);
		if (!_ب)
		{
			if (item == null)
			{
				return string.Empty;
			}
			if (propertyValue == null)
			{
				return "";
			}
			return Convert.ToString(propertyValue, CultureInfo.CurrentCulture);
		}
		TreeConvertEventArgs treeConvertEventArgs = new TreeConvertEventArgs(propertyValue, typeof(string), item, fieldName);
		OnFormat(treeConvertEventArgs);
		if (((ConvertEventArgs)treeConvertEventArgs).Value != item && ((ConvertEventArgs)treeConvertEventArgs).Value is string)
		{
			return (string)((ConvertEventArgs)treeConvertEventArgs).Value;
		}
		if (ش == null)
		{
			ش = TypeDescriptor.GetConverter(typeof(string));
		}
		try
		{
			return (string)ᬒ.ᬖ(propertyValue, typeof(string), _06E2(fieldName), ش, _ة, _ت, null, DBNull.Value);
		}
		catch (Exception ex)
		{
			if (ex is SecurityException || _۵(ex))
			{
				throw;
			}
			return (propertyValue != null) ? Convert.ToString(item, CultureInfo.CurrentCulture) : "";
		}
	}

	private static bool _۵(Exception _۶)
	{
		if (!(_۶ is NullReferenceException) && !(_۶ is StackOverflowException) && !(_۶ is OutOfMemoryException) && !(_۶ is ThreadAbortException) && !(_۶ is ExecutionEngineException) && !(_۶ is IndexOutOfRangeException))
		{
			return _۶ is AccessViolationException;
		}
		return true;
	}

	protected object GetPropertyValue(object item, string fieldName)
	{
		if (item != null && fieldName.Length > 0)
		{
			try
			{
				PropertyDescriptor propertyDescriptor = ((_ر == null) ? TypeDescriptor.GetProperties(item).Find(fieldName, ignoreCase: true) : ((BindingManagerBase)_ر).GetItemProperties().Find(fieldName, ignoreCase: true));
				if (propertyDescriptor != null)
				{
					item = propertyDescriptor.GetValue(item);
				}
			}
			catch
			{
			}
		}
		return item;
	}

	private void _۷(int ԝ)
	{
		if (ԝ == -1)
		{
			__28AB.SelectedNode = null;
			return;
		}
		Node node = null;
		node = ((_ر == null) ? __28AB.GetNodeByFlatIndex(ԝ) : __28AB.FindNodeByBindingIndex(ԝ));
		__28AB.SelectedNode = node;
	}

	protected virtual void OnSelectedIndexChanged(EventArgs e)
	{
		_05CB?.Invoke(this, e);
	}

	protected override void OnParentChanged(EventArgs e)
	{
		if (DataSource != null && __28AB.Nodes.Count == 0 && _ئ != null && _ئ.Count > 0)
		{
			_06DA(DataSource, _ئ, _06DD: true);
			__28AB._0654();
		}
		base.OnParentChanged(e);
	}

	protected virtual void OnValueMemberChanged(EventArgs e)
	{
		_05C9?.Invoke(this, e);
	}

	protected virtual void OnSelectedValueChanged(EventArgs e)
	{
		_05CA?.Invoke(this, e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeWatermarkColor()
	{
		return _ᠤ != SystemColors.GrayText;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetWatermarkColor()
	{
		WatermarkColor = SystemColors.GrayText;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFocusHighlightColor()
	{
		return !_ᚾ.Equals((object?)ColorScheme.GetColor(16777096));
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetFocusHighlightColor()
	{
		FocusHighlightColor = ColorScheme.GetColor(16777096);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackgroundStyle()
	{
		_ᠩ.StyleChanged -= ઐ;
		_ᠩ = new ElementStyle();
		_ᠩ.StyleChanged += ઐ;
		((Control)this).Invalidate();
	}

	private void ઐ(object _٠, EventArgs ؿ)
	{
		OnVisualPropertyChanged();
	}

	protected virtual void OnVisualPropertyChanged()
	{
		_ᶇ.InvalidateArrange();
		((Control)this).Invalidate();
	}

	protected override void Dispose(bool disposing)
	{
		if (_ᠩ != null)
		{
			_ᠩ.StyleChanged -= ઐ;
		}
		Timer val = __05F8;
		__05F8 = null;
		if (val != null)
		{
			val.Stop();
			((Component)(object)val).Dispose();
		}
		if (__28AB != null)
		{
			__28AB.AfterNodeSelect -= ᜅ;
			__28AB.BeforeNodeSelect -= ᜆ;
			__28AB.NodeMouseDown -= ᜃ;
		}
		if (__22C3 != null)
		{
			((Component)(object)__22C3).Dispose();
			__22C3 = null;
		}
		base.Dispose(disposing);
	}

	private void ᠵ(InputButtonSettings ᠶ)
	{
		OnInputButtonSettingsChanged(ᠶ);
	}

	void IInputButtonControl.InputButtonSettingsChanged(InputButtonSettings ᠶ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᠵ
		this.ᠵ(ᠶ);
	}

	protected virtual void OnInputButtonSettingsChanged(InputButtonSettings inputButtonSettings)
	{
		ᶎ();
	}

	private void ᶎ()
	{
		RecreateButtons();
		_ᶇ.InvalidateArrange();
		((Control)this).Invalidate();
	}

	protected virtual void RecreateButtons()
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		VisualItem[] items = ᠹ();
		VisualGroup ᶇ = _ᶇ;
		VisualItem[] array = new VisualItem[ᶇ.Items.Count];
		ᶇ.Items._0CD0(array);
		VisualItem[] array2 = array;
		foreach (VisualItem visualItem in array2)
		{
			if (visualItem.ᡇ == _1885._1886)
			{
				ᶇ.Items.Remove(visualItem);
				if (visualItem == _ᠧ.ItemReference)
				{
					visualItem.MouseUp -= new MouseEventHandler(ᠷ);
				}
				else if (visualItem == _ᠨ.ItemReference)
				{
					visualItem.MouseUp -= new MouseEventHandler(ᠸ);
				}
			}
		}
		ᶇ.Items.AddRange(items);
	}

	private void ᠷ(object _٠, MouseEventArgs ؿ)
	{
		if (_ᠧ.ItemReference.RenderBounds.Contains(ؿ.X, ؿ.Y))
		{
			OnButtonCustomClick((EventArgs)(object)ؿ);
		}
	}

	protected virtual void OnButtonCustomClick(EventArgs e)
	{
		if (_181E != null)
		{
			_181E(this, e);
		}
	}

	private void ᠸ(object _٠, MouseEventArgs ؿ)
	{
		if (_ᠨ.ItemReference.RenderBounds.Contains(ؿ.X, ؿ.Y))
		{
			OnButtonCustom2Click((EventArgs)(object)ؿ);
		}
	}

	protected virtual void OnButtonCustom2Click(EventArgs e)
	{
		if (_181F != null)
		{
			_181F(this, e);
		}
	}

	private VisualItem[] ᠹ()
	{
		SortedList sortedList = CreateSortedButtonList();
		VisualItem[] array = new VisualItem[sortedList.Count];
		sortedList.Values.CopyTo(array, 0);
		return array;
	}

	protected virtual SortedList CreateSortedButtonList()
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Expected O, but got Unknown
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Expected O, but got Unknown
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Expected O, but got Unknown
		SortedList sortedList = new SortedList(4);
		if (_ᠧ.Visible)
		{
			VisualItem visualItem = CreateButton(_ᠧ);
			if (_ᠧ.ItemReference != null)
			{
				_ᠧ.ItemReference.MouseUp -= new MouseEventHandler(ᠷ);
				_ᠧ.ItemReference.Click -= ᶏ;
			}
			_ᠧ.ItemReference = visualItem;
			visualItem.Click += ᶏ;
			visualItem.MouseUp += new MouseEventHandler(ᠷ);
			visualItem.Enabled = _ᠧ.Enabled;
			sortedList.Add(_ᠧ, visualItem);
		}
		if (_ᠨ.Visible)
		{
			VisualItem visualItem2 = CreateButton(_ᠨ);
			if (_ᠨ.ItemReference != null)
			{
				_ᠨ.ItemReference.MouseUp -= new MouseEventHandler(ᠸ);
				_ᠨ.ItemReference.Click -= ᶐ;
			}
			_ᠨ.ItemReference = visualItem2;
			visualItem2.Click += ᶐ;
			visualItem2.MouseUp += new MouseEventHandler(ᠸ);
			visualItem2.Enabled = _ᠨ.Enabled;
			sortedList.Add(_ᠨ, visualItem2);
		}
		if (__17F2.Visible)
		{
			VisualItem visualItem3 = CreateButton(__17F2);
			if (__17F2.ItemReference != null)
			{
				__17F2.ItemReference.MouseUp -= new MouseEventHandler(_180F);
				__17F2.ItemReference.Click -= _28B5;
			}
			__17F2.ItemReference = visualItem3;
			visualItem3.MouseUp += new MouseEventHandler(_180F);
			visualItem3.Click += _28B5;
			sortedList.Add(__17F2, visualItem3);
		}
		if (__17F1.Visible)
		{
			VisualItem visualItem4 = CreateButton(__17F1);
			if (__17F1.ItemReference != null)
			{
				__17F1.ItemReference.MouseDown -= new MouseEventHandler(_180C);
				__17F1.ItemReference.Click -= _28B6;
			}
			__17F1.ItemReference = visualItem4;
			visualItem4.MouseDown += new MouseEventHandler(_180C);
			visualItem4.Click += _28B6;
			sortedList.Add(__17F1, visualItem4);
		}
		return sortedList;
	}

	private void _28B5(object _٠, EventArgs ؿ)
	{
		if (ؿ is KeyEventArgs)
		{
			_180F(_٠, ؿ);
		}
	}

	private void ᶏ(object _٠, EventArgs ؿ)
	{
		if (ؿ is KeyEventArgs)
		{
			OnButtonCustomClick(ؿ);
		}
	}

	private void ᶐ(object _٠, EventArgs ؿ)
	{
		if (ؿ is KeyEventArgs)
		{
			OnButtonCustom2Click(ؿ);
		}
	}

	private void _28B6(object _٠, EventArgs ؿ)
	{
		if (ؿ is KeyEventArgs)
		{
			IsPopupOpen = !IsPopupOpen;
		}
	}

	protected virtual VisualItem CreateButton(InputButtonSettings buttonSettings)
	{
		VisualItem visualItem = null;
		if (buttonSettings == __17F1)
		{
			visualItem = new VisualDropDownButton();
			ApplyButtonSettings(buttonSettings, visualItem as VisualButton);
		}
		else
		{
			visualItem = new VisualCustomButton();
			ApplyButtonSettings(buttonSettings, visualItem as VisualButton);
		}
		VisualButton visualButton = visualItem as VisualButton;
		visualButton.ClickAutoRepeat = false;
		if (buttonSettings == __17F2 && buttonSettings.Image == null && string.IsNullOrEmpty(buttonSettings.Text))
		{
			visualButton.Symbol = "\uf00d";
		}
		return visualItem;
	}

	protected virtual void ApplyButtonSettings(InputButtonSettings buttonSettings, VisualButton button)
	{
		button.Text = buttonSettings.Text;
		button.Image = buttonSettings.Image;
		button.ᡇ = _1885._1886;
		button.Enabled = buttonSettings.Enabled;
		button.Shortcut = buttonSettings.Shortcut;
		button.Tooltip = buttonSettings.Tooltip;
		button.Symbol = buttonSettings.Symbol;
		button.SymbolSet = buttonSettings.SymbolSet;
		button.SymbolColor = buttonSettings.SymbolColor;
	}

	private void ᶑ()
	{
		VisualGroup visualGroup = new VisualGroup();
		visualGroup.HorizontalItemSpacing = 0;
		visualGroup.ArrangeInvalid += ᶓ;
		visualGroup.RenderInvalid += ᶒ;
		visualGroup.ResetMouseHover += _28B7;
		_ᶇ = visualGroup;
	}

	private void ᶒ(object _٠, EventArgs ؿ)
	{
		((Control)this).Invalidate();
	}

	private void ᶓ(object _٠, EventArgs ؿ)
	{
		((Control)this).Invalidate();
	}

	private void _28B7(object _٠, EventArgs ؿ)
	{
		DevComponents.AdvTree.Interop._0EFE.ఘ((Control)(object)this);
	}

	private PaintInfo ᶕ(Graphics _٥)
	{
		PaintInfo paintInfo = new PaintInfo();
		paintInfo.Graphics = _٥;
		paintInfo.DefaultFont = ((Control)this).Font;
		paintInfo.ForeColor = ((Control)this).ForeColor;
		paintInfo.RenderOffset = default(Point);
		Size size = ((Control)this).Size;
		bool ᠲ = false;
		ElementStyle elementStyle = _1400(out ᠲ);
		size.Height -= ᝈ._1753(elementStyle, eSpacePart.Border) + ᝈ._1754(elementStyle, eSpacePart.Border) + 2;
		size.Width -= ᝈ.ᝐ(elementStyle, eSpacePart.Border) + ᝈ._1752(elementStyle, eSpacePart.Border) + 2;
		paintInfo.AvailableSize = size;
		paintInfo.ParentEnabled = ((Control)this).Enabled;
		paintInfo.MouseOver = _ኗ || ((Control)this).Focused;
		if (ᠲ)
		{
			elementStyle.Dispose();
		}
		return paintInfo;
	}

	private ElementStyle _1400(out bool ᠲ)
	{
		ᠲ = false;
		_ᠩ.SetColorScheme(GetColorScheme());
		return ElementStyleDisplay.റ(_ᠩ, out ᠲ);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).OnPaint(e);
		Graphics graphics = e.Graphics;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		bool enabled = ((Control)this).Enabled;
		if (_089F.फ() || !_089F.थ)
		{
			e.Graphics.Clear(SystemColors.Control);
			return;
		}
		if (!((Control)this).Enabled)
		{
			if (!_ᖣ.IsEmpty)
			{
				SolidBrush val = new SolidBrush(_ᖣ);
				try
				{
					e.Graphics.FillRectangle((Brush)(object)val, clientRectangle);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			else
			{
				e.Graphics.FillRectangle(SystemBrushes.Control, clientRectangle);
			}
		}
		else
		{
			SolidBrush val2 = new SolidBrush(((Control)this).BackColor);
			try
			{
				e.Graphics.FillRectangle((Brush)(object)val2, clientRectangle);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		bool ᠲ = false;
		ElementStyle elementStyle = _1400(out ᠲ);
		if (elementStyle.Custom)
		{
			SmoothingMode smoothingMode = graphics.SmoothingMode;
			if (base.AntiAlias)
			{
				graphics.SmoothingMode = (SmoothingMode)4;
			}
			ElementStyleDisplayInfo e2 = new ElementStyleDisplayInfo(elementStyle, graphics, clientRectangle);
			if (!enabled)
			{
				ElementStyleDisplay.PaintBorder(e2);
			}
			else
			{
				ElementStyleDisplay.Paint(e2);
			}
			if (base.AntiAlias)
			{
				graphics.SmoothingMode = smoothingMode;
			}
		}
		Rectangle exclude = ᶖ(graphics);
		Rectangle rectangle = _28B8(elementStyle, clientRectangle);
		if (!exclude.IsEmpty)
		{
			Rectangle[] array = DisplayHelp.ExcludeRectangle(rectangle, exclude);
			if (array.Length >= 1)
			{
				rectangle = array[0];
			}
			rectangle.Width--;
		}
		if (WatermarkEnabled && WatermarkText.Length > 0 && IsWatermarkRendered)
		{
			Rectangle ल = rectangle;
			ल.Inflate(-1, -1);
			_16F0(graphics, ल);
		}
		PaintSelection(graphics, rectangle);
		if (ᠲ)
		{
			elementStyle.Dispose();
		}
	}

	private void _16F0(Graphics _٥, Rectangle ल)
	{
		if (WatermarkText.Length != 0)
		{
			Font font = ((Control)this).Font;
			if (WatermarkFont != null)
			{
				font = WatermarkFont;
			}
			eTextFormat eTextFormat = eTextFormat.Default;
			if (_ᠦ == eTextAlignment.Center)
			{
				eTextFormat |= eTextFormat.HorizontalCenter;
			}
			else if (_ᠦ == eTextAlignment.Right)
			{
				eTextFormat |= eTextFormat.Right;
			}
			TextDrawing.DrawString(_٥, WatermarkText, font, WatermarkColor, ल, eTextFormat);
		}
	}

	private Rectangle _28B8(ElementStyle _0657, Rectangle _28B9)
	{
		return new Rectangle(ᝈ.ᝐ(_0657, eSpacePart.Border) + 1, ᝈ._1753(_0657, eSpacePart.Border) + 1, _28B9.Width - (ᝈ._1752(_0657, eSpacePart.Border) + ᝈ.ᝐ(_0657, eSpacePart.Border) + 2), _28B9.Height - (ᝈ._1753(_0657, eSpacePart.Border) + ᝈ._1754(_0657, eSpacePart.Border) + 2));
	}

	protected string GetSelectedDisplayMemberText(Node node)
	{
		int num = __28AB.Columns._1737(__28AD);
		if (num >= 0)
		{
			return node.Cells[num].Text;
		}
		return GetItemText(node.DataKey, __28AD);
	}

	protected virtual void PaintSelection(Graphics g, Rectangle bounds)
	{
		if (bounds.Width < 2 || bounds.Height < 2)
		{
			return;
		}
		if (((Control)this).Focused && !IsPopupOpen)
		{
			SelectionRendererEventArgs selectionRendererEventArgs = new SelectionRendererEventArgs();
			selectionRendererEventArgs.Bounds = bounds;
			selectionRendererEventArgs.Graphics = g;
			selectionRendererEventArgs.SelectionBoxStyle = eSelectionStyle.HighlightCells;
			selectionRendererEventArgs.TreeActive = true;
			((NodeTreeDisplay)__28AB.NodeDisplay).GetTreeRenderer().DrawSelection(selectionRendererEventArgs);
		}
		Node selectedNode = __28AB.SelectedNode;
		if (selectedNode == null)
		{
			return;
		}
		if (selectedNode.BoundsRelative.IsEmpty || __28AB.IsLayoutPending || selectedNode.BoundsRelative.Width <= 0 || ((Control)__28AB).Width <= 0)
		{
			UpdateTreeSize();
			if (__28AB.IsLayoutPending)
			{
				__28AB.RecalcLayout();
			}
		}
		Region clip = g.Clip;
		g.SetClip(bounds);
		Rectangle bounds2 = bounds;
		if (!string.IsNullOrEmpty(__28AD))
		{
			string selectedDisplayMemberText = GetSelectedDisplayMemberText(selectedNode);
			if (!string.IsNullOrEmpty(selectedDisplayMemberText))
			{
				bounds2.Y--;
				bounds2.X++;
				bounds2.Width--;
				TextDrawing.DrawString(g, selectedDisplayMemberText, ((Control)this).Font, ((Control)this).Enabled ? ((Control)this).ForeColor : SystemColors.ControlDark, bounds2, eTextFormat.NoClipping | eTextFormat.NoPadding | eTextFormat.VerticalCenter);
			}
		}
		else
		{
			if (NodeDisplay._070C(selectedNode))
			{
				bounds2.X -= __28AB.ExpandWidth;
			}
			else
			{
				bounds2.X -= selectedNode.Bounds.X - selectedNode.BoundsRelative.X - 2;
			}
			if (bounds2.Height > selectedNode.BoundsRelative.Height)
			{
				bounds2.Y += (bounds2.Height - selectedNode.BoundsRelative.Height) / 2;
			}
			else
			{
				bounds2.Y++;
			}
			bool enabled = selectedNode.Enabled;
			if (selectedNode.Enabled != ((Control)this).Enabled)
			{
				selectedNode.Enabled = ((Control)this).Enabled;
			}
			bool isSelectionVisible = selectedNode.IsSelectionVisible;
			selectedNode.IsSelectionVisible = ((Control)this).Focused && !IsPopupOpen;
			((NodeTreeDisplay)__28AB.NodeDisplay).ExternalPaintNode(selectedNode, g, bounds2);
			selectedNode.Enabled = enabled;
			selectedNode.IsSelectionVisible = isSelectionVisible;
		}
		g.Clip = clip;
		if (clip != null)
		{
			clip.Dispose();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor()
	{
		return ((Control)this).BackColor != SystemColors.Window;
	}

	private Rectangle ᶖ(Graphics _٥)
	{
		PaintInfo paintInfo = ᶕ(_٥);
		if (!_ᶇ.IsLayoutValid)
		{
			ᘥ(paintInfo);
		}
		bool ᠲ = false;
		ElementStyle elementStyle = _1400(out ᠲ);
		_ᶇ.RenderBounds = _28BA(elementStyle);
		_ᶇ.ᡤ(paintInfo);
		if (ᠲ)
		{
			elementStyle.Dispose();
		}
		return _ᶇ.RenderBounds;
	}

	private Rectangle _28BA(ElementStyle _0657)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)((Control)this).RightToLeft == 1)
		{
			return new Rectangle(ᝈ.ᝐ(_0657, eSpacePart.Border) + 1, ᝈ._1753(_0657, eSpacePart.Border) + 1, _ᶇ.Size.Width, _ᶇ.Size.Height);
		}
		return new Rectangle(((Control)this).Width - (ᝈ._1752(_0657, eSpacePart.Border) + 1) - _ᶇ.Size.Width, ᝈ._1753(_0657, eSpacePart.Border) + 1, _ᶇ.Size.Width, _ᶇ.Size.Height);
	}

	protected override void OnResize(EventArgs e)
	{
		_ᶇ.InvalidateArrange();
		ᘥ();
		((Control)this).Invalidate();
		((Control)this).OnResize(e);
	}

	protected override void OnEnter(EventArgs e)
	{
		((Control)this).Invalidate();
		((Control)this).OnEnter(e);
	}

	protected override void OnLeave(EventArgs e)
	{
		if (IsPopupOpen)
		{
			IsPopupOpen = false;
		}
		((Control)this).Invalidate();
		((Control)this).OnLeave(e);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		_ᶇ.ᡟ(e);
		((Control)this).OnKeyDown(e);
	}

	protected override bool _੪(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Invalid comparison between Unknown and I4
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Invalid comparison between Unknown and I4
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Invalid comparison between Unknown and I4
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Invalid comparison between Unknown and I4
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Invalid comparison between Unknown and I4
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Invalid comparison between Unknown and I4
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Invalid comparison between Unknown and I4
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected I4, but got Unknown
		if (((Control)this).Focused)
		{
			Keys val = (Keys)_089F.MapVirtualKey((uint)(int)_੦, 2u);
			if ((int)val == 0)
			{
				val = (Keys)_0EFE._0F11(_੦);
			}
			if ((int)val == 115)
			{
				IsPopupOpen = !IsPopupOpen;
				return true;
			}
			if ((((int)val == 40 && (Control.ModifierKeys & 0x40000) != 0) || ((int)val == 13 && __28AE)) && !IsPopupOpen)
			{
				IsPopupOpen = !IsPopupOpen;
				return true;
			}
			if (IsPopupOpen && ((int)val == 9 || (int)val == 13))
			{
				IsPopupOpen = false;
				if ((int)val == 13)
				{
					return true;
				}
			}
			else if (IsPopupOpen)
			{
				if ((int)val == 38)
				{
					_28BD(eTreeAction.Keyboard);
					return true;
				}
				if ((int)val == 40)
				{
					_28BC(eTreeAction.Keyboard);
					return true;
				}
				try
				{
					byte[] array = new byte[256];
					if (_089F.GetKeyboardState(array))
					{
						byte[] array2 = new byte[2];
						if (_089F.ToAscii((uint)(int)val, 0u, array, array2, 0u) != 0)
						{
							char[] array3 = new char[2];
							Encoding.Default.GetDecoder().GetChars(array2, 0, 2, array3, 0);
							if (_28BB(array3[0]))
							{
								return true;
							}
						}
					}
				}
				catch (Exception)
				{
				}
			}
		}
		return base._੪(_0A65, _੦, _੧);
	}

	protected override void OnKeyUp(KeyEventArgs e)
	{
		_ᶇ.ᡠ(e);
		((Control)this).OnKeyUp(e);
	}

	protected override bool IsInputChar(char charCode)
	{
		if (__05F7 && char.IsLetterOrDigit(charCode))
		{
			return true;
		}
		return ((Control)this).IsInputChar(charCode);
	}

	protected override void OnKeyPress(KeyPressEventArgs e)
	{
		if (__05F7)
		{
			e.Handled = _28BB(e.KeyChar);
		}
		if (!e.Handled)
		{
			_ᶇ.ᡡ(e);
		}
		((Control)this).OnKeyPress(e);
	}

	private bool _28BB(char _065C)
	{
		if (!char.IsLetterOrDigit(_065C))
		{
			return false;
		}
		string text = _066D(_065C.ToString());
		Node node = __28AB.FindNodeByText(text, __28AB.SelectedNode, ignoreCase: true);
		if (node == null && __28AB.SelectedNode != null)
		{
			node = __28AB.FindNodeByText(text, ignoreCase: true);
		}
		if (_ا != null && node != null && node.BindingIndex == -1)
		{
			while (node != null && node.BindingIndex == -1)
			{
				node = __28AB.FindNodeByText(text, node, ignoreCase: true);
			}
		}
		if (!__28AF && node == null)
		{
			return false;
		}
		_28BE(node, eTreeAction.Keyboard);
		return false;
	}

	private string _066D(string ٮ)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		if (__05F9 <= 0)
		{
			return ٮ;
		}
		if (__05F8 == null)
		{
			__05F8 = new Timer();
			__05F8.Interval = __05F9;
			__05F8.Tick += ٯ;
			__05F8.Start();
		}
		else
		{
			__05F8.Start();
		}
		__05FB += ٮ;
		return __05FB;
	}

	private void ٯ(object _٠, EventArgs ؿ)
	{
		__05F8.Stop();
		__05FB = "";
	}

	protected override bool ProcessMnemonic(char charCode)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		if (((Control)this).Focused && (int)Control.ModifierKeys != 262144)
		{
			return true;
		}
		return ((Control)this).ProcessMnemonic(charCode);
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Invalid comparison between Unknown and I4
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Invalid comparison between Unknown and I4
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Invalid comparison between Unknown and I4
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		if ((keyData & 0x28) == 40 && (Control.ModifierKeys & 0x40000) != 0 && !IsPopupOpen)
		{
			IsPopupOpen = !IsPopupOpen;
			return true;
		}
		if ((int)keyData == 38)
		{
			_28BD(eTreeAction.Keyboard);
			return true;
		}
		if ((int)keyData == 40)
		{
			_28BC(eTreeAction.Keyboard);
			return true;
		}
		if (_ᶇ.ᘭ(ref msg, keyData))
		{
			return true;
		}
		return ((Control)this).ProcessCmdKey(ref msg, keyData);
	}

	private void _28BC(eTreeAction ڃ)
	{
		Node node = null;
		node = ((SelectedNode != null) ? _175C.ᝩ(SelectedNode) : _175C.ᝤ(__28AB));
		if (node != null && !node.CanSelect)
		{
			int num = 0;
			while (node != null && num < 100)
			{
				node = _175C.ᝩ(node);
				if (node != null && node.CanSelect)
				{
					break;
				}
			}
		}
		if (node == null)
		{
			return;
		}
		if (_ر != null)
		{
			while (node != null && node.DataKey == null)
			{
				node = _175C.ᝩ(node);
			}
			if (node != null)
			{
				_28BE(node, ڃ);
			}
		}
		else
		{
			_28BE(node, ڃ);
		}
	}

	private void _28BD(eTreeAction ڃ)
	{
		Node node = null;
		node = ((SelectedNode != null) ? _175C.ᝯ(SelectedNode) : _175C.ᝥ(__28AB));
		if (node != null && !node.CanSelect)
		{
			int num = 0;
			while (node != null && num < 100)
			{
				node = _175C.ᝯ(node);
				if (node != null && node.CanSelect)
				{
					break;
				}
			}
		}
		if (node == null)
		{
			return;
		}
		if (_ر != null)
		{
			while (node != null && node.DataKey == null)
			{
				node = _175C.ᝩ(node);
			}
			if (node != null)
			{
				_28BE(node, ڃ);
			}
		}
		else
		{
			_28BE(node, ڃ);
		}
	}

	private void _28BE(Node _0653, eTreeAction ڃ)
	{
		__28AB.SelectNode(_0653, ڃ);
	}

	protected override void OnRightToLeftChanged(EventArgs e)
	{
		_ᶇ.InvalidateArrange();
		ᘥ();
		((Control)this).Invalidate();
		((Control)this).OnRightToLeftChanged(e);
	}

	protected override void OnFontChanged(EventArgs e)
	{
		_ᶇ.InvalidateArrange();
		((Control)__28AB).Font = ((Control)this).Font;
		ᘥ();
		((Control)this).Invalidate();
		base.OnFontChanged(e);
	}

	private void ᘥ()
	{
		Graphics val = BarFunctions.CreateGraphics((Control)(object)this);
		try
		{
			ᘥ(ᶕ(val));
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void ᘥ(PaintInfo _065C)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Invalid comparison between Unknown and I4
		if (__28B0)
		{
			return;
		}
		__28B0 = true;
		try
		{
			if (!_ᶇ.IsLayoutValid)
			{
				_ᶇ.PerformLayout(_065C);
			}
			bool ᠲ = false;
			ElementStyle elementStyle = _1400(out ᠲ);
			Rectangle rectangle = ᝈ._1756(elementStyle, ((Control)this).ClientRectangle);
			if (ᵸ)
			{
				Rectangle rectangle2 = _28BA(elementStyle);
				if ((int)((Control)this).RightToLeft == 1)
				{
					rectangle.X += rectangle2.Right;
					rectangle.Width -= rectangle2.Right;
				}
				else
				{
					rectangle.Width -= rectangle.Right - rectangle2.X;
				}
			}
			if (ᠲ)
			{
				elementStyle.Dispose();
			}
		}
		finally
		{
			__28B0 = false;
		}
	}

	private void _180C(object _٠, MouseEventArgs ؿ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)ؿ.Button != 1048576 || (__17F4 != DateTime.MinValue && DateTime.Now.Subtract(__17F4).TotalMilliseconds < 250.0))
		{
			__17F4 = DateTime.MinValue;
		}
		else
		{
			ShowDropDown();
		}
	}

	public void UpdateTreeSize()
	{
		__28AB.BeginUpdate();
		bool flag = false;
		if (__28B4 == 0 && ((Control)__28AB).Width != ((Control)this).Width - 4)
		{
			((Control)__28AB).Width = ((Control)this).Width - 4;
			flag = true;
		}
		if (__28B3 == 0)
		{
			((Control)__28AB).Height = 600;
		}
		else
		{
			((Control)__28AB).Height = __28B3;
		}
		__28AB.EndUpdate(performLayoutAndRefresh: false);
		if (__28B3 == 0)
		{
			__28AB.RecalcLayout();
			ঊ ঊ = BarFunctions.ॻ((Control)(object)this);
			if (ঊ != null && __28AB.Ԫ._0711 > ঊ.ঌ.Height / 2)
			{
				((Control)__28AB).Height = ঊ.ঌ.Height / 2;
			}
			else if (__28AB.Nodes.Count > 0)
			{
				int num = __28AB.Ԫ._0711 + 4;
				if (__28AB.HScrollBar != null)
				{
					num += ((Control)__28AB.HScrollBar).Height + 1;
				}
				((Control)__28AB).Height = num;
			}
			else
			{
				((Control)__28AB).Height = ((Control)this).Height;
			}
		}
		else
		{
			((Control)__28AB).Height = __28B3;
			if (flag)
			{
				__28AB.RecalcLayout();
			}
		}
	}

	public void ShowDropDown()
	{
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		if ((__22C3 == null && _០.SubItems.Count == 0) || __28B1 || _០.Expanded)
		{
			return;
		}
		__28B1 = true;
		try
		{
			ControlContainerItem controlContainerItem = null;
			ItemContainer itemContainer = null;
			if (__22C3 != null)
			{
				itemContainer = new ItemContainer();
				itemContainer.Name = __22BB;
				controlContainerItem = new ControlContainerItem(__22BA);
				itemContainer.SubItems.Add(controlContainerItem);
				_០.SubItems.Insert(0, itemContainer);
			}
			CancelEventArgs cancelEventArgs = new CancelEventArgs();
			OnButtonDropDownClick(cancelEventArgs);
			if (cancelEventArgs.Cancel || _០.SubItems.Count == 0)
			{
				if (itemContainer != null)
				{
					_០.SubItems.Remove(itemContainer);
				}
				itemContainer.Dispose();
				return;
			}
			UpdateTreeSize();
			__22C0 = __22C3.Parent;
			controlContainerItem.Control = __22C3;
			_០.SetDisplayRectangle(((Control)this).ClientRectangle);
			if (__28B2.HasValue)
			{
				_០.PopupLocation = __28B2.Value;
			}
			else if ((int)((Control)this).RightToLeft == 0)
			{
				Point point = new Point(((Control)this).Width - _០.PopupSize.Width, ((Control)this).Height);
				ঊ ঊ = BarFunctions.ॻ((Control)(object)this);
				Point point2 = ((Control)this).PointToScreen(point);
				if (ঊ != null && ঊ.ঌ.X > point2.X)
				{
					point.X = 0;
				}
				_០.PopupLocation = point;
			}
			if (!IsPopupOpen && SelectedNode != null)
			{
				SelectedNode.EnsureVisible();
			}
			_០.Expanded = !_០.Expanded;
		}
		finally
		{
			__28B1 = false;
		}
		((Control)this).Invalidate();
	}

	public void CloseDropDown()
	{
		if (_០.Expanded)
		{
			_០.Expanded = false;
		}
	}

	private void _180E(object _٠, EventArgs ؿ)
	{
		__17F4 = DateTime.Now;
		if (_០.SubItems[__22BB] is ItemContainer itemContainer)
		{
			if (itemContainer.SubItems[__22BA] is ControlContainerItem controlContainerItem)
			{
				controlContainerItem.Control = null;
				itemContainer.SubItems.Remove(controlContainerItem);
				controlContainerItem.Dispose();
				if (__22C3 != null)
				{
					__22C3.Parent = __22C0;
					__22C0 = null;
				}
			}
			_០.SubItems.Remove(itemContainer);
			itemContainer.Dispose();
		}
		((Control)this).Invalidate();
	}

	protected virtual void OnPopupLocationChanged(Point? oldValue, Point? newValue)
	{
	}

	private void _180F(object _٠, EventArgs ؿ)
	{
		if (__17F2.Enabled)
		{
			CancelEventArgs cancelEventArgs = new CancelEventArgs();
			OnButtonClearClick(cancelEventArgs);
			if (!cancelEventArgs.Cancel)
			{
				SelectedNode = null;
			}
		}
	}

	protected virtual void OnButtonClearClick(CancelEventArgs e)
	{
		if (_៦ != null)
		{
			_៦(this, e);
		}
	}

	protected virtual void OnButtonDropDownClick(CancelEventArgs e)
	{
		if (_៧ != null)
		{
			_៧(this, e);
		}
	}

	protected override PopupItem CreatePopupItem()
	{
		ButtonItem buttonItem = new ButtonItem("sysPopupProvider");
		buttonItem.PopupFinalized += _180E;
		_០ = buttonItem;
		return buttonItem;
	}

	protected override void RecalcSize()
	{
	}

	public override void PerformClick()
	{
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		if (ᵸ)
		{
			_ᶇ.ᡘ(e);
		}
		((Control)this).OnMouseMove(e);
	}

	protected override void OnMouseHover(EventArgs e)
	{
		if (ᵸ)
		{
			_ᶇ.ᡜ(e);
		}
		((Control)this).OnMouseHover(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		if (ᵸ)
		{
			_ᶇ.ᡗ();
		}
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Invalid comparison between Unknown and I4
		((Control)this).Focus();
		if (ᵸ)
		{
			_ᶇ.ᡚ(e);
		}
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		clientRectangle.Inflate(1, 1);
		if (ᵸ && !_ᶇ.RenderBounds.Contains(e.X, e.Y) && clientRectangle.Contains(e.X, e.Y) && !IsPopupOpen)
		{
			if (__17F4 != DateTime.MinValue && DateTime.Now.Subtract(__17F4).TotalMilliseconds < 150.0)
			{
				__17F4 = DateTime.MinValue;
			}
			else if ((int)e.Button == 1048576)
			{
				IsPopupOpen = true;
			}
		}
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (ᵸ)
		{
			_ᶇ.ᡛ(e);
		}
		((Control)this).OnMouseUp(e);
	}

	private void _28BF()
	{
		__28AB.BeginUpdate();
		if (__28B4 > 0)
		{
			((Control)__28AB).Width = __28B4;
		}
		if (__28B3 > 0)
		{
			((Control)__28AB).Height = __28B3;
		}
		__28AB.EndUpdate(performLayoutAndRefresh: false);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeGridLinesColor()
	{
		return __28AB.ShouldSerializeGridLinesColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetGridLinesColor()
	{
		__28AB.ResetGridLinesColor();
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
}
