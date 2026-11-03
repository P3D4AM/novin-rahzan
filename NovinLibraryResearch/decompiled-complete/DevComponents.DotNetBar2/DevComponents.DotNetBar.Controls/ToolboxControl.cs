using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Primitives;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Controls;

[ToolboxBitmap(typeof(ToolboxControl), "Controls.ToolboxControl.bmp")]
[ComVisible(false)]
[Designer("DevComponents.DotNetBar.Design.ToolboxControlDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(true)]
public class ToolboxControl : ContainerControl
{
	private ItemPanel _Ɑ;

	private Bar __20C8;

	private TextBoxX _Ɱ;

	private Bar _Ɐ;

	private ButtonItem _Ɒ;

	private string _ⱱ = "Enter text to search...";

	private ButtonItem __2418;

	private LabelItem __20C9;

	private string _ᅶ = "Toolbox";

	private int _Ⱳ;

	private CustomCollection<ToolboxGroup> _ⱳ = new CustomCollection<ToolboxGroup>();

	private bool _ⱴ = true;

	private EventHandler _1C2B;

	private bool __223A = true;

	private Color _Ⱶ = Color.Gray;

	private List<_23D7> _ⱶ = new List<_23D7>();

	private int _ⱷ;

	private EventHandler ⱸ;

	private eToolboxItemSelectionMode _ᬱ = eToolboxItemSelectionMode.Single;

	private EventHandler _20DD;

	private CustomCollection<ToolboxItem> _ᬳ;

	private ToolboxItem __21D1;

	private bool _ⱹ;

	private bool _ᅵ = true;

	private bool _ⱺ = true;

	private bool _ⱻ = true;

	private bool _ⱼ = true;

	private bool _ⱽ;

	private List<BaseItem> _Ȿ = new List<BaseItem>();

	private List<ToolboxGroup> _Ɀ = new List<ToolboxGroup>();

	private string _Ⲁ = string.Empty;

	private ElementStyle _ᠩ = new ElementStyle();

	private CancelEventHandler _0A0D;

	[Browsable(false)]
	public Bar MenuBar => _Ɐ;

	[Description("Indicates the search-box watermark text.")]
	[DefaultValue("Enter text to search...")]
	[Localizable(true)]
	[Category("Appearance")]
	public string SearchBoxWatermark
	{
		get
		{
			return _ⱱ;
		}
		set
		{
			if (_ⱱ != value)
			{
				_ⱱ = value;
				OnSearchBoxWatermarkChanged(value, value);
			}
		}
	}

	[Browsable(false)]
	public TextBoxX SearchBoxTextBox => _Ɱ;

	[DefaultValue("Toolbox")]
	[Description("Indicates the title label text.")]
	[Localizable(true)]
	[Category("Appearance")]
	public string TitleText
	{
		get
		{
			return _ᅶ;
		}
		set
		{
			if (_ᅶ != value)
			{
				_ᅶ = value;
				OnTitleTextChanged(value, value);
			}
		}
	}

	[Browsable(false)]
	public Bar TitleBar => __20C8;

	[Browsable(false)]
	public bool IsUpdateSuspended => _Ⱳ > 0;

	[Browsable(false)]
	public ItemPanel ItemsPanel => _Ɑ;

	[MergableProperty(false)]
	[Description("List of items displayed in list box.")]
	[Category("Appearance")]
	[Localizable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public CustomCollection<ToolboxGroup> Groups => _ⱳ;

	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Indicates whether single toolbox group only is expanded at a time")]
	public bool ExpandSingleGroupOnly
	{
		get
		{
			return _ⱴ;
		}
		set
		{
			_ⱴ = value;
		}
	}

	[Description("Indicates whether control is expanded and shows items in full size with images and text")]
	[DefaultValue(true)]
	[Category("Appearance")]
	public bool Expanded
	{
		get
		{
			return __223A;
		}
		set
		{
			if (__223A != value)
			{
				__223A = value;
				OnExpandedChanged();
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates the collapsed toolbox separator color, the line drawn between the toolbox and menu/expand buttons above.")]
	public Color CollapsedSeparatorColor
	{
		get
		{
			return _Ⱶ;
		}
		set
		{
			_Ⱶ = value;
			if (!__223A)
			{
				((Control)_Ɐ).Invalidate();
				_Ɐ._0AFA = new Color[4]
				{
					Color.Empty,
					Color.Empty,
					Color.Empty,
					_Ⱶ
				};
			}
		}
	}

	[Browsable(false)]
	public int ExpandedWidth
	{
		get
		{
			return _ⱷ;
		}
		set
		{
			_ⱷ = value;
		}
	}

	[Category("Behavior")]
	[Description("Indicates toolbox item selection mode.")]
	[DefaultValue(eToolboxItemSelectionMode.Single)]
	public eToolboxItemSelectionMode SelectionMode
	{
		get
		{
			return _ᬱ;
		}
		set
		{
			if (_ᬱ != value)
			{
				_ᬱ = value;
				OnSelectionModeChanged();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public CustomCollection<ToolboxItem> SelectedItems => _ᬳ;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ToolboxItem SelectedItem
	{
		get
		{
			return __21D1;
		}
		set
		{
			if (__21D1 != value)
			{
				ToolboxItem oldValue = __21D1;
				__21D1 = value;
				OnSelectedItemChanged(oldValue, value);
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Indicates whether title bar of the control is visible.")]
	public bool TitleVisible
	{
		get
		{
			return _ᅵ;
		}
		set
		{
			if (_ᅵ != value)
			{
				bool oldValue = value;
				_ᅵ = value;
				OnTitleVisibleChanged(oldValue, value);
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Indicates whether menu bar of the control is visible")]
	public bool MenuVisible
	{
		get
		{
			return _ⱺ;
		}
		set
		{
			if (_ⱺ != value)
			{
				bool oldValue = value;
				_ⱺ = value;
				OnMenuVisibleChanged(oldValue, value);
			}
		}
	}

	[Description("Indicates whether search text-box which allows searching for the toolbox items is visible.")]
	[Category("Appearance")]
	[DefaultValue(true)]
	public bool SearchBoxVisible
	{
		get
		{
			return _ⱻ;
		}
		set
		{
			if (_ⱻ != value)
			{
				_ⱻ = value;
				ⲕ();
			}
		}
	}

	[Category("Behavior")]
	[Description("Indicates whether search text when entered is split into separate words and items returned that match any of the words entered.")]
	[DefaultValue(true)]
	public bool SearchForEachWord
	{
		get
		{
			return _ⱼ;
		}
		set
		{
			if (_ⱼ != value)
			{
				bool oldValue = value;
				_ⱼ = value;
				OnSearchForEachWordChanged(oldValue, value);
			}
		}
	}

	[Browsable(false)]
	public bool IsSearching => _ⱽ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Gets or sets bar background style.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Style")]
	public ElementStyle BackgroundStyle => _ᠩ;

	[DefaultValue(true)]
	[Description("Indicates whether item drag and drop is enabled. Default value is true.")]
	[Category("Behavior")]
	public bool ItemDragDropEnabled
	{
		get
		{
			return _Ɑ.EnableDragDrop;
		}
		set
		{
			_Ɑ.EnableDragDrop = value;
		}
	}

	[Category("Behavior")]
	[Description("Indicates whether toolbox items can be rearranged using drag & drop.")]
	[DefaultValue(true)]
	public bool ItemRearrangeEnabled
	{
		get
		{
			return ((Control)_Ɑ).AllowDrop;
		}
		set
		{
			((Control)_Ɑ).AllowDrop = value;
		}
	}

	[Description("Occurs after Expanded property value has changed")]
	public event EventHandler ExpandedChanged
	{
		add
		{
			EventHandler eventHandler = _1C2B;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1C2B, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _1C2B;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1C2B, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs after toolbox group is expanded")]
	public event EventHandler ToolboxGroupExpanded
	{
		add
		{
			EventHandler eventHandler = ⱸ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ⱸ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ⱸ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ⱸ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs after selected item has changed. ")]
	public event EventHandler SelectedItemChanged
	{
		add
		{
			EventHandler eventHandler = _20DD;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _20DD, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _20DD;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _20DD, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs before an item drag & drop operation is started and allows cancellation")]
	public event CancelEventHandler BeforeItemDrag
	{
		add
		{
			CancelEventHandler cancelEventHandler = _0A0D;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _0A0D, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = _0A0D;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _0A0D, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	public ToolboxControl()
	{
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		_ᠩ.StyleChanged += ઐ;
		_ᠩ.Class = ElementStyleClassKeys.ToolboxControlKey;
		((Control)this).Padding = new Padding(2);
		((Control)this).SetStyle((ControlStyles)74262, true);
		_Ɑ = ⲋ();
		((Control)this).Controls.Add((Control)(object)_Ɑ);
		_Ɐ = Ⲃ();
		((Control)this).Controls.Add((Control)(object)_Ɐ);
		_Ɱ = ⲇ();
		((Control)this).Controls.Add((Control)(object)_Ɱ);
		__20C8 = Ⲋ();
		((Control)this).Controls.Add((Control)(object)__20C8);
		_ᬳ = new CustomCollection<ToolboxItem>();
		_ⱳ.CollectionChanged += ⲁ;
	}

	private void ⲁ(object _٠, NotifyCollectionChangedEventArgs ؿ)
	{
		if (ؿ.Action == NotifyCollectionChangedAction.Add)
		{
			int num = ؿ.NewStartingIndex;
			{
				foreach (ToolboxGroup newItem in ؿ.NewItems)
				{
					_Ɑ.Items.Insert(num, newItem);
					num++;
				}
				return;
			}
		}
		if (ؿ.Action == NotifyCollectionChangedAction.Remove)
		{
			foreach (ToolboxGroup oldItem in ؿ.OldItems)
			{
				_Ɑ.Items.Remove(oldItem);
			}
			return;
		}
		if (ؿ.Action == NotifyCollectionChangedAction.Reset)
		{
			_Ɑ.Items.Clear();
			if (ؿ.NewItems == null)
			{
				return;
			}
			{
				foreach (ToolboxGroup newItem2 in ؿ.NewItems)
				{
					_Ɑ.Items.Add(newItem2);
				}
				return;
			}
		}
		if (ؿ.Action == NotifyCollectionChangedAction.Replace)
		{
			_Ɑ.Items.Remove(ؿ.NewStartingIndex);
			_Ɑ.Items.Insert(ؿ.NewStartingIndex, (BaseItem)ؿ.NewItems[0]);
		}
	}

	protected override void Dispose(bool disposing)
	{
		((ContainerControl)this).Dispose(disposing);
	}

	private Bar Ⲃ()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		Bar bar = new Bar();
		bar.Name = "menuBar";
		bar.AntiAlias = true;
		((Control)bar).BackColor = Color.Transparent;
		((Control)bar).Dock = (DockStyle)1;
		((Control)bar).Font = new Font("Segoe UI", 9f);
		bar.IsMaximized = false;
		bar.Location = new Point(6, 93);
		bar.PaddingBottom = 4;
		bar.PaddingTop = 4;
		bar.Size = new Size(365, 61);
		bar.Stretch = true;
		bar.Style = eDotNetBarStyle.StyleManagerControlled;
		bar.TabIndex = 2;
		bar.TabStop = false;
		ButtonItem buttonItem = new ButtonItem("menuButton");
		buttonItem.AutoExpandOnClick = true;
		buttonItem.ButtonStyle = eButtonStyle.ImageAndText;
		buttonItem.ImagePaddingHorizontal = 30;
		buttonItem.ImagePaddingVertical = 12;
		buttonItem.ImagePosition = eImagePosition.Right;
		buttonItem.PopupSide = ePopupSide.Right;
		buttonItem.Text = "Menu";
		buttonItem.SubItems.Add(new ButtonItem());
		buttonItem.PopupOpen += ⲅ;
		buttonItem.PopupClose += ⲃ;
		_Ɒ = buttonItem;
		bar.Items.AddRange(new BaseItem[1] { buttonItem });
		return bar;
	}

	private void ⲃ(object _٠, EventArgs ؿ)
	{
		Ⲅ();
		_Ɒ.SubItems.Add(new ButtonItem());
	}

	private void Ⲅ()
	{
		foreach (BaseItem subItem in _Ɒ.SubItems)
		{
			subItem.Dispose();
		}
		_Ɒ.SubItems.Clear();
	}

	private void ⲅ(object _٠, PopupOpenEventArgs ؿ)
	{
		Ⲅ();
		foreach (ToolboxGroup group in Groups)
		{
			if (group is ToolboxGroup toolboxGroup)
			{
				ButtonItem buttonItem = new ButtonItem();
				buttonItem.Text = toolboxGroup.TitleText;
				buttonItem.Click += Ⲇ;
				buttonItem.Tag = toolboxGroup;
				_Ɒ.SubItems.Add(buttonItem);
			}
		}
	}

	private void Ⲇ(object _٠, EventArgs ؿ)
	{
		if (((BaseItem)_٠).Tag is ToolboxGroup toolboxGroup)
		{
			toolboxGroup.Expanded = true;
		}
	}

	protected virtual void OnSearchBoxWatermarkChanged(string oldValue, string newValue)
	{
		_Ɱ.WatermarkText = newValue;
	}

	private TextBoxX ⲇ()
	{
		TextBoxX textBoxX = new TextBoxX();
		((Control)textBoxX).Name = "searchBox";
		textBoxX.Border.Class = "TextBoxBorder";
		textBoxX.Border.CornerType = eCornerType.Square;
		textBoxX.ButtonCustom.Symbol = "\uf002";
		textBoxX.ButtonCustom.Visible = true;
		textBoxX.ButtonCustomClick += Ⲉ;
		((Control)textBoxX).Dock = (DockStyle)1;
		((Control)textBoxX).Location = new Point(6, 55);
		textBoxX.PreventEnterBeep = true;
		((Control)textBoxX).Size = new Size(365, 38);
		((Control)textBoxX).TabIndex = 0;
		textBoxX.WatermarkText = "Enter text to search...";
		((Control)textBoxX).TextChanged += ⲉ;
		((TextBoxBase)textBoxX).WordWrap = false;
		return textBoxX;
	}

	private void Ⲉ(object _٠, EventArgs ؿ)
	{
		((Control)_Ɱ).Text = string.Empty;
	}

	private void ⲉ(object _٠, EventArgs ؿ)
	{
		if (string.IsNullOrEmpty(((Control)_Ɱ).Text))
		{
			_Ɱ.ButtonCustom.Symbol = "\uf002";
		}
		else
		{
			_Ɱ.ButtonCustom.Symbol = "\uf00d";
		}
		Search(((Control)_Ɱ).Text);
	}

	private Bar Ⲋ()
	{
		Bar bar = new Bar();
		bar.Name = "titleBar";
		bar.AntiAlias = true;
		((Control)bar).BackColor = Color.Transparent;
		((Control)bar).Dock = (DockStyle)1;
		bar.IsMaximized = false;
		bar.Location = new Point(6, 6);
		bar.PaddingBottom = 4;
		bar.PaddingTop = 4;
		bar.Size = new Size(365, 49);
		bar.Stretch = true;
		bar.Style = eDotNetBarStyle.StyleManagerControlled;
		bar.TabIndex = 0;
		bar.TabStop = false;
		LabelItem labelItem = new LabelItem("toolboxLabel");
		labelItem.Text = "Toolbox";
		__20C9 = labelItem;
		ButtonItem buttonItem = new ButtonItem("expandButton");
		buttonItem.ItemAlignment = eItemAlignment.Far;
		buttonItem.Symbol = "\uf053";
		buttonItem.SymbolSize = 10f;
		buttonItem.Click += _1CE4;
		__2418 = buttonItem;
		bar.Items.AddRange(new BaseItem[2] { labelItem, buttonItem });
		return bar;
	}

	protected virtual void OnTitleTextChanged(string oldValue, string newValue)
	{
		__20C9.Text = newValue;
	}

	private void _1CE4(object _٠, EventArgs ؿ)
	{
		Expanded = !Expanded;
	}

	private ItemPanel ⲋ()
	{
		ItemPanel itemPanel = new ItemPanel();
		((Control)itemPanel).Dock = (DockStyle)5;
		itemPanel.LayoutOrientation = eOrientation.Vertical;
		itemPanel.AutoScroll = true;
		itemPanel.ItemClick += Ⲓ;
		itemPanel.DragDropSupport = true;
		itemPanel.EnableDragDrop = true;
		itemPanel.UseNativeDragDrop = true;
		((Control)itemPanel).AllowDrop = true;
		itemPanel.BeforeItemDrag += ⲍ;
		itemPanel.GetBaseItemContainer().SubItemsChanged += Ⲍ;
		itemPanel.DragDropAllowedContainerTypes.Add(typeof(ToolboxGroup));
		return itemPanel;
	}

	private void Ⲍ(object _٠, CollectionChangeEventArgs ؿ)
	{
		if (!IsUpdateSuspended)
		{
			RecalcLayout();
		}
	}

	public void BeginUpdate()
	{
		_Ⱳ++;
	}

	public void EndUpdate()
	{
		EndUpdate(performLayoutAndRefresh: true);
	}

	public void EndUpdate(bool performLayoutAndRefresh)
	{
		if (_Ⱳ > 0)
		{
			_Ⱳ--;
		}
		if (_Ⱳ == 0 && performLayoutAndRefresh)
		{
			RecalcLayout();
			((Control)this).Invalidate(true);
		}
	}

	public void RecalcLayout()
	{
		_Ɑ.RecalcLayout();
	}

	private void ⲍ(object _٠, CancelEventArgs ؿ)
	{
		OnBeforeItemDrag(_٠, ؿ);
	}

	protected virtual void OnExpandedChanged(EventArgs e)
	{
		_1C2B?.Invoke(this, e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeCollapsedSeparatorColor()
	{
		return _Ⱶ != Color.Gray;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetCollapsedSeparatorColor()
	{
		CollapsedSeparatorColor = Color.Gray;
	}

	protected virtual void OnExpandedChanged()
	{
		OnExpandedChanged(EventArgs.Empty);
		if (!__223A)
		{
			_ⱷ = ((Control)this).Width;
			__2418.Symbol = "\uf054";
			_Ɒ.ButtonStyle = eButtonStyle.Default;
			__20C9.Visible = false;
			_Ɒ.Symbol = "\ue037";
			_Ɒ.SymbolSet = eSymbolSet.Material;
			_Ɒ.SymbolSize = 10f;
			_Ɒ.ButtonStyle = eButtonStyle.Default;
			_Ɒ.ImagePaddingHorizontal = 8;
			_Ɒ.ImagePaddingVertical = 6;
			_Ɒ.ImagePosition = eImagePosition.Left;
			_Ɒ.ShowSubItems = false;
			_Ɐ._0AFA = new Color[4]
			{
				Color.Empty,
				Color.Empty,
				Color.Empty,
				_Ⱶ
			};
			((Control)_Ɱ).Visible = false;
			foreach (BaseItem item in _Ɑ.Items)
			{
				if (!(item is ToolboxGroup))
				{
					continue;
				}
				ToolboxGroup toolboxGroup = (ToolboxGroup)item;
				_23D7 _23D7 = _2901._2902(toolboxGroup);
				_23D7.RecordSetting(item);
				_ⱶ.Add(_23D7);
				toolboxGroup.TitleVisible = false;
				toolboxGroup.LayoutOrientation = eOrientation.Vertical;
				toolboxGroup.MultiLine = false;
				foreach (BaseItem subItem in toolboxGroup.SubItems)
				{
					if (subItem is ButtonItem buttonItem)
					{
						_23D7 _23D72 = _2901._2902(buttonItem);
						_23D72.RecordSetting(buttonItem);
						_ⱶ.Add(_23D72);
						buttonItem.ButtonStyle = eButtonStyle.Default;
					}
				}
			}
		}
		else
		{
			__2418.Symbol = "\uf053";
			_Ɐ._0AFA = new Color[0];
			_Ɒ.ButtonStyle = eButtonStyle.ImageAndText;
			__20C9.Visible = true;
			_Ɒ.Symbol = string.Empty;
			_Ɒ.ButtonStyle = eButtonStyle.Default;
			_Ɒ.ImagePaddingHorizontal = 30;
			_Ɒ.ImagePaddingVertical = 12;
			_Ɒ.ImagePosition = eImagePosition.Right;
			_Ɒ.ShowSubItems = true;
			_Ɐ._0AFA = new Color[0];
			((Control)_Ɱ).Visible = true;
			_23D7[] array = new _23D7[_ⱶ.Count];
			_ⱶ.CopyTo(array);
			_23D7[] array2 = array;
			foreach (_23D7 _23D73 in array2)
			{
				_23D73.RestoreSettings();
			}
			_ⱶ.Clear();
			((Control)this).Width = _ⱷ;
		}
		_Ɑ.RecalcLayout();
		if (__223A)
		{
			_Ɒ.FixedSize = Size.Empty;
			__2418.FixedSize = Size.Empty;
		}
		else
		{
			int num = Ⲏ();
			_Ɒ.FixedSize = new Size(num - Dpi.Width2, _Ɒ.HeightInternal);
			__2418.FixedSize = new Size(num - Dpi.Width2, __2418.HeightInternal);
		}
		_Ɐ.RecalcLayout();
		__20C8.RecalcLayout();
	}

	private int Ⲏ()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		int num = ⲏ();
		int num2 = num + ᝈ.ᝐ(_Ɑ.BackgroundStyle, eSpacePart.Padding | eSpacePart.Margin) + ᝈ._1752(_Ɑ.BackgroundStyle, eSpacePart.Padding | eSpacePart.Margin);
		Padding padding = ((Control)this).Padding;
		int width = num2 + ((Padding)(ref padding)).Horizontal;
		_Ɒ.FixedSize = new Size(num - Dpi.Width2, _Ɒ.HeightInternal);
		__2418.FixedSize = new Size(num - Dpi.Width2, __2418.HeightInternal);
		((Control)this).Width = width;
		return num;
	}

	private int ⲏ()
	{
		int num = 0;
		bool flag = false;
		foreach (BaseItem item in _Ɑ.Items)
		{
			if (item is ToolboxGroup)
			{
				foreach (BaseItem subItem in item.SubItems)
				{
					if (subItem is ButtonItem && subItem.WidthInternal > num)
					{
						num = subItem.WidthInternal;
					}
				}
			}
			if (item.Bounds.Bottom > ((Control)this).ClientRectangle.Height)
			{
				flag = true;
			}
		}
		if (flag)
		{
			num += SystemInformation.VerticalScrollBarWidth;
		}
		if (num == 0)
		{
			num = 32;
		}
		return num;
	}

	protected virtual void OnToolboxGroupExpanded(object sender, EventArgs e)
	{
		ⱸ?.Invoke(sender, e);
	}

	internal void Ⲑ(ToolboxGroup ⲑ)
	{
		if (!__223A)
		{
			Ⲏ();
		}
		OnToolboxGroupExpanded(ⲑ, EventArgs.Empty);
	}

	protected virtual void OnSelectionModeChanged()
	{
	}

	private void Ⲓ(object _٠, EventArgs ؿ)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		if (_ᬱ != 0 && _٠ is ToolboxItem { AutoCheckOnClick: false } toolboxItem && (!toolboxItem.Checked || (Control.ModifierKeys & 0x20000) != 0))
		{
			if (toolboxItem.Checked)
			{
				SetSelectedItem(toolboxItem, isSelected: false);
			}
			else
			{
				SetSelectedItem(toolboxItem, isSelected: true);
			}
		}
	}

	public void SetSelectedItem(ToolboxItem item, bool isSelected)
	{
		if (_ᬱ == eToolboxItemSelectionMode.Single)
		{
			if (__21D1 == item && !isSelected)
			{
				SelectedItem = null;
			}
			else if (isSelected)
			{
				SelectedItem = item;
			}
		}
		else if (item.Checked != isSelected)
		{
			item.Checked = isSelected;
		}
	}

	protected virtual void OnSelectedItemChanged(EventArgs e)
	{
		_20DD?.Invoke(this, e);
	}

	protected virtual void OnSelectedItemChanged(ToolboxItem oldValue, ToolboxItem newValue)
	{
		if (oldValue != null)
		{
			if (newValue != null)
			{
				_ⱹ = true;
			}
			oldValue.Checked = false;
			_ⱹ = false;
		}
		if (newValue != null)
		{
			newValue.Checked = true;
		}
	}

	internal void ⲓ(ToolboxItem Ⲕ)
	{
		if (_ᬱ == eToolboxItemSelectionMode.Single)
		{
			_ᬳ.Clear();
			if (Ⲕ.Checked)
			{
				__21D1 = Ⲕ;
			}
		}
		else if (!Ⲕ.Checked && _ᬳ.Contains(Ⲕ))
		{
			_ᬳ.Remove(Ⲕ);
		}
		else if (Ⲕ.Checked)
		{
			_ᬳ.Add(Ⲕ);
		}
		if (!_ⱹ)
		{
			OnSelectedItemChanged(EventArgs.Empty);
		}
	}

	protected virtual void OnTitleVisibleChanged(bool oldValue, bool newValue)
	{
		__20C8.Visible = newValue;
	}

	protected virtual void OnMenuVisibleChanged(bool oldValue, bool newValue)
	{
		_Ɐ.Visible = newValue;
	}

	protected virtual void OnSearchForEachWordChanged(bool oldValue, bool newValue)
	{
	}

	private void ⲕ()
	{
		((Control)_Ɱ).Visible = _ⱻ;
	}

	public void Search(string text)
	{
		_ⱽ = true;
		Ⲗ();
		if (!string.IsNullOrEmpty(text))
		{
			text = text.ToLower();
			string[] array = text.Split(new char[1] { ' ' });
			foreach (BaseItem item in _Ɑ.Items)
			{
				if (!(item is ToolboxGroup toolboxGroup))
				{
					continue;
				}
				if (toolboxGroup.Expanded)
				{
					_Ɀ.Add(toolboxGroup);
				}
				toolboxGroup.Expanded = true;
				foreach (BaseItem subItem in toolboxGroup.SubItems)
				{
					ToolboxItem toolboxItem = subItem as ToolboxItem;
					if (toolboxItem == null)
					{
						toolboxItem.Visible = false;
						_Ȿ.Add(toolboxItem);
					}
					else if (((_ⱼ && array.Length >= 2) || !toolboxItem.Text.ToLower().Contains(text)) && (!_ⱻ || !ⲗ(toolboxItem.Text.ToLower(), array)))
					{
						toolboxItem.Visible = false;
						_Ȿ.Add(toolboxItem);
					}
				}
				if (toolboxGroup.VisibleSubItems == 0)
				{
					toolboxGroup.Visible = false;
					_Ȿ.Add(toolboxGroup);
				}
			}
		}
		_Ɑ.RecalcLayout();
		_ⱽ = false;
		_Ⲁ = text;
	}

	private void Ⲗ()
	{
		bool flag = _Ȿ.Count > 0 || !string.IsNullOrEmpty(_Ⲁ);
		if (_Ȿ.Count > 0)
		{
			foreach (BaseItem item in _Ȿ)
			{
				item.Visible = true;
			}
			_Ȿ.Clear();
		}
		if (!flag)
		{
			return;
		}
		foreach (BaseItem item2 in _Ɑ.Items)
		{
			if (item2 is ToolboxGroup toolboxGroup)
			{
				if (_Ɀ.Contains(toolboxGroup))
				{
					toolboxGroup.Expanded = true;
				}
				else
				{
					toolboxGroup.Expanded = false;
				}
			}
		}
		_Ɀ.Clear();
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
		((Control)this).Invalidate();
	}

	private void _٤(Graphics _٥)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		if (!((Control)this).BackColor.IsEmpty)
		{
			SolidBrush val = new SolidBrush(((Control)this).BackColor);
			try
			{
				_٥.FillRectangle((Brush)(object)val, ((Control)this).ClientRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		ElementStyleDisplayInfo elementStyleDisplayInfo = new ElementStyleDisplayInfo();
		elementStyleDisplayInfo.Bounds = ((Control)this).ClientRectangle;
		elementStyleDisplayInfo.Graphics = _٥;
		bool ല = false;
		ElementStyle elementStyle = (elementStyleDisplayInfo.Style = ElementStyleDisplay.റ(_ᠩ, out ല));
		ElementStyleDisplay.Paint(elementStyleDisplayInfo);
		if (ല)
		{
			elementStyle.Dispose();
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		Graphics graphics = e.Graphics;
		_٤(graphics);
		((Control)this).OnPaint(e);
	}

	protected virtual void OnBeforeItemDrag(object itemSource, CancelEventArgs e)
	{
		_0A0D?.Invoke(itemSource, e);
	}

	private static bool ⲗ(string Ⲙ, params string[] ⲙ)
	{
		foreach (string value in ⲙ)
		{
			if (Ⲙ.Contains(value))
			{
				return true;
			}
		}
		return false;
	}
}
