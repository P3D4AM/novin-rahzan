using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree;

[ToolboxItem(false)]
[Designer("DevComponents.AdvTree.Design.ColumnHeaderDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class ColumnHeader : Component
{
	private string ө = "";

	private ColumnWidth ܘ;

	private bool ᜊ = true;

	private eStyleTextAlignment ᜋ;

	private string ݒ = "";

	private string ݔ = "";

	private string Ӱ = "";

	private string ᜌ = "";

	private bool Ӻ = true;

	private Rectangle ݗ = Rectangle.Empty;

	private bool ӱ = true;

	private string Ӿ = "";

	private EventHandler _170D;

	private SortCellsEventHandler ᜎ;

	private MouseEventHandler ग़;

	private MouseEventHandler ફ;

	private EventHandler ધ;

	private EventHandler દ;

	private ColumnHeaderCollection _ᗚ;

	private bool _ݬ = true;

	private int _ᜏ;

	private int _ᜐ;

	private bool _ᜑ;

	private Image _ነ;

	private eColumnImageAlignment __1712;

	private string __1713 = "";

	private object __1714;

	private Color __1715 = Color.Empty;

	private int __1716 = -1;

	private bool __1717 = true;

	private eSortDirection __1718;

	private string _ݩ = "";

	private ToolTip _ݪ;

	private EventHandler ݫ;

	private bool _ו = true;

	private bool __1719 = true;

	private bool __171A;

	private bool _ᆿ;

	internal bool _171B;

	internal bool _171C;

	private MouseEventHandler મ;

	private EventHandler બ;

	private EventHandler ભ;

	private EventHandler ય;

	private eCellEditorType _ݳ;

	private bool __171D;

	private IComparer __171E;

	private IComparer __171F;

	internal ColumnHeaderCollection Parent
	{
		get
		{
			return _ᗚ;
		}
		set
		{
			_ᗚ = value;
		}
	}

	[Description("Indicates whether cells content in this column is editable when cell editing is enabled on tree control.")]
	[Category("Behavior")]
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

	[DefaultValue(0)]
	[Category("Behavior")]
	[Description("Indicates maximum number of characters the user can type or paste when editing cells in this column.")]
	public int MaxInputLength
	{
		get
		{
			return _ᜏ;
		}
		set
		{
			_ᜏ = value;
		}
	}

	[Category("Design")]
	[Description("Indicates the name used to identify column header.")]
	[Browsable(false)]
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
	public Rectangle Bounds => ݗ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Layout")]
	[Description("Gets or sets the width of the column.")]
	[Browsable(true)]
	public ColumnWidth Width => ܘ;

	[Description("Indicates minimum column width in pixels that is enforced when user is resizing the columns using mouse")]
	[DefaultValue(0)]
	[Category("Layout")]
	public int MinimumWidth
	{
		get
		{
			return _ᜐ;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			_ᜐ = value;
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether column is stretched to fill any empty space horizontally in tree when all columns consume less width than available. Only one column in tree may have this property set to true and only last column with this property set will be stretched. You should always set the Width for the column since Width will be used when columns consume more space in tree horizontally than available. Applies to top-level columns only.")]
	[Category("Layout")]
	public bool StretchToFill
	{
		get
		{
			return _ᜑ;
		}
		set
		{
			if (_ᜑ != value)
			{
				_ᜑ = value;
				OnSizeChanged();
			}
		}
	}

	[Description("Indicates the style class assigned to the column.")]
	[Browsable(true)]
	[Category("Style")]
	[DefaultValue("")]
	public string StyleNormal
	{
		get
		{
			return ݒ;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			ݒ = value;
			OnSizeChanged();
		}
	}

	[Category("Style")]
	[Browsable(true)]
	[Description("Indicates the style class assigned to the column when mouse is down.")]
	[DefaultValue("")]
	public string StyleMouseDown
	{
		get
		{
			return ݔ;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			ݔ = value;
			OnSizeChanged();
		}
	}

	[Browsable(true)]
	[Category("Style")]
	[Description("Indicates the style class assigned to the cell when mouse is over the column.")]
	[DefaultValue("")]
	public string StyleMouseOver
	{
		get
		{
			return Ӱ;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			Ӱ = value;
			OnSizeChanged();
		}
	}

	[Browsable(true)]
	[Description("Indicates the name of the column in the ColumnHeaderCollection.")]
	[DefaultValue("")]
	[Category("Data")]
	public string ColumnName
	{
		get
		{
			return ᜌ;
		}
		set
		{
			ᜌ = value;
		}
	}

	[DefaultValue("")]
	[Localizable(true)]
	[Description("Indicates column caption.")]
	[Browsable(true)]
	[Category("Appearance")]
	public string Text
	{
		get
		{
			return ө;
		}
		set
		{
			ө = value;
		}
	}

	[Category("Behavior")]
	[Browsable(true)]
	[Description("Indicates whether column is visible.")]
	[DefaultValue(true)]
	public bool Visible
	{
		get
		{
			return Ӻ;
		}
		set
		{
			if (Ӻ != value)
			{
				Ӻ = value;
				OnSizeChanged();
			}
		}
	}

	[Browsable(true)]
	[Category("Images")]
	[Description("Indicates column image")]
	[DevCoSerialize]
	[DefaultValue(null)]
	public Image Image
	{
		get
		{
			return _ነ;
		}
		set
		{
			_ነ = value;
			OnImageChanged();
		}
	}

	[Description("Indicates image alignment.")]
	[DevCoSerialize]
	[DefaultValue(eColumnImageAlignment.Left)]
	[Category("Images")]
	public eColumnImageAlignment ImageAlignment
	{
		get
		{
			return __1712;
		}
		set
		{
			__1712 = value;
			OnSizeChanged();
		}
	}

	[Description("Indicates data-field or property name that is used as source of data for this column when data-binding is used.")]
	[DefaultValue("")]
	[Category("Data")]
	public string DataFieldName
	{
		get
		{
			return __1713;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			if (__1713 != value)
			{
				__1713 = value;
				OnDataFieldNameChanged();
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public object Tag
	{
		get
		{
			return __1714;
		}
		set
		{
			__1714 = value;
		}
	}

	[Description("Indicates color of cells background for this column.")]
	[Category("Columns")]
	public Color CellsBackColor
	{
		get
		{
			return __1715;
		}
		set
		{
			if (__1715 != value)
			{
				__1715 = value;
				if (_ᗚ != null && _ᗚ.ކ != null)
				{
					((Control)_ᗚ.ކ).Invalidate();
				}
			}
		}
	}

	[Description("Indicates display index of the column. -1 indicates default value and is modified to actual display index when the column is added to a ColumnHeaderCollection.")]
	[DefaultValue(-1)]
	[Category("Appearance")]
	public int DisplayIndex
	{
		get
		{
			return __1716;
		}
		set
		{
			if (value != __1716)
			{
				int oldValue = __1716;
				__1716 = value;
				OnDisplayIndexChanged(oldValue, value);
			}
		}
	}

	[Description("Indicates whether user can sort by this column by clicking it.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool SortingEnabled
	{
		get
		{
			return __1717;
		}
		set
		{
			if (value != __1717)
			{
				bool oldValue = __1717;
				__1717 = value;
				OnSortingEnabledChanged(oldValue, value);
			}
		}
	}

	[Description("Indicates sort direction. Sort direction can be changed by clicking the column header if SortingEnabled=true.")]
	[Category("Behavior")]
	[DefaultValue(eSortDirection.None)]
	public eSortDirection SortDirection
	{
		get
		{
			return __1718;
		}
		set
		{
			if (value != __1718)
			{
				eSortDirection oldValue = __1718;
				__1718 = value;
				OnSortDirectionChanged(oldValue, value);
			}
		}
	}

	[DefaultValue("")]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates the text that is displayed when mouse hovers over the cell.")]
	[Localizable(true)]
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
			AdvTree advTree = AdvTree;
			if (advTree == null)
			{
				return Rectangle.Empty;
			}
			return new Rectangle(((Control)advTree).PointToScreen(Bounds.Location), Bounds.Size);
		}
	}

	[Category("Behavior")]
	[Description("Indicates whether tooltips are shown when mouse is over the cell when Tooltip property is set.")]
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

	[Browsable(false)]
	public AdvTree AdvTree
	{
		get
		{
			if (_ᗚ == null)
			{
				return null;
			}
			if (_ᗚ.ParentNode != null)
			{
				return _ᗚ.ParentNode.TreeControl;
			}
			return _ᗚ.ކ;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether column is automatically sized to content when user double-clicks the column resize marker.")]
	[Category("Behavior")]
	public bool DoubleClickAutoSize
	{
		get
		{
			return __1719;
		}
		set
		{
			__1719 = value;
		}
	}

	internal bool SizeChanged
	{
		get
		{
			return ӱ;
		}
		set
		{
			ӱ = value;
		}
	}

	[Browsable(false)]
	public bool IsMouseDown
	{
		get
		{
			return __171A;
		}
		internal set
		{
			if (__171A != value)
			{
				__171A = value;
				OnIsMouseDownChanged();
			}
		}
	}

	[Browsable(false)]
	public bool IsMouseOver
	{
		get
		{
			return _ᆿ;
		}
		internal set
		{
			_ᆿ = value;
			Invalidate();
		}
	}

	[Category("Behavior")]
	[DefaultValue(eCellEditorType.Default)]
	[Description("Indicates editor type used to edit the cell.")]
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

	[DefaultValue(null)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public IComparer SortComparer
	{
		get
		{
			return __171E;
		}
		set
		{
			if (value != __171E)
			{
				IComparer oldValue = __171E;
				__171E = value;
				OnSortComparerChanged(oldValue, value);
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IComparer SortComparerReverse
	{
		get
		{
			return __171F;
		}
		set
		{
			if (value != __171F)
			{
				IComparer oldValue = __171F;
				__171F = value;
				OnSortComparerReverseChanged(oldValue, value);
			}
		}
	}

	public event EventHandler HeaderSizeChanged
	{
		add
		{
			EventHandler eventHandler = _170D;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _170D, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _170D;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _170D, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	internal event SortCellsEventHandler SortCells
	{
		add
		{
			SortCellsEventHandler sortCellsEventHandler = ᜎ;
			SortCellsEventHandler sortCellsEventHandler2;
			do
			{
				sortCellsEventHandler2 = sortCellsEventHandler;
				SortCellsEventHandler value2 = (SortCellsEventHandler)Delegate.Combine(sortCellsEventHandler2, value);
				sortCellsEventHandler = Interlocked.CompareExchange(ref ᜎ, value2, sortCellsEventHandler2);
			}
			while ((object)sortCellsEventHandler != sortCellsEventHandler2);
		}
		remove
		{
			SortCellsEventHandler sortCellsEventHandler = ᜎ;
			SortCellsEventHandler sortCellsEventHandler2;
			do
			{
				sortCellsEventHandler2 = sortCellsEventHandler;
				SortCellsEventHandler value2 = (SortCellsEventHandler)Delegate.Remove(sortCellsEventHandler2, value);
				sortCellsEventHandler = Interlocked.CompareExchange(ref ᜎ, value2, sortCellsEventHandler2);
			}
			while ((object)sortCellsEventHandler != sortCellsEventHandler2);
		}
	}

	public event MouseEventHandler MouseDown
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ग़;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ग़, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ग़;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ग़, value2, val2);
			}
			while (val != val2);
		}
	}

	public event MouseEventHandler MouseUp
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ફ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ફ, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = ફ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ફ, value2, val2);
			}
			while (val != val2);
		}
	}

	public event EventHandler DoubleClick
	{
		add
		{
			EventHandler eventHandler = ધ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ધ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ધ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ધ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler Click
	{
		add
		{
			EventHandler eventHandler = દ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref દ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = દ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref દ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

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

	[Description("Occurs when mouse is moving over the column header")]
	public event MouseEventHandler MouseMove
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = મ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref મ, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = મ;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref મ, value2, val2);
			}
			while (val != val2);
		}
	}

	[Description("Occurs when mouse enters column header.")]
	public event EventHandler MouseEnter
	{
		add
		{
			EventHandler eventHandler = બ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref બ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = બ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref બ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when mouse leaves the column header.")]
	public event EventHandler MouseLeave
	{
		add
		{
			EventHandler eventHandler = ભ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ભ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ભ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ભ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when mouse hovers over the column.")]
	public event EventHandler MouseHover
	{
		add
		{
			EventHandler eventHandler = ય;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ય, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ય;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ય, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public ColumnHeader()
		: this("")
	{
	}

	public ColumnHeader(string text)
	{
		ө = text;
		ܘ = new ColumnWidth();
		ܘ.ᜠ += WidthChanged;
	}

	protected override void Dispose(bool disposing)
	{
		if (BarUtilities.DisposeItemImages)
		{
			BarUtilities.ᆟ(ref _ነ);
		}
		base.Dispose(disposing);
	}

	public virtual ColumnHeader Copy()
	{
		ColumnHeader columnHeader = new ColumnHeader();
		columnHeader.ColumnName = ColumnName;
		columnHeader.StyleMouseDown = StyleMouseDown;
		columnHeader.StyleMouseOver = StyleMouseOver;
		columnHeader.StyleNormal = StyleNormal;
		columnHeader.Text = Text;
		columnHeader.Visible = Visible;
		columnHeader.Width.Absolute = Width.Absolute;
		columnHeader.Width.Relative = Width.Relative;
		columnHeader.Width.AutoSize = Width.AutoSize;
		columnHeader.Width.AutoSizeMinHeader = Width.AutoSizeMinHeader;
		columnHeader.DisplayIndex = DisplayIndex;
		columnHeader.SortDirection = SortDirection;
		columnHeader.SortingEnabled = SortingEnabled;
		columnHeader.StretchToFill = StretchToFill;
		columnHeader.Tag = Tag;
		columnHeader.DataFieldName = DataFieldName;
		columnHeader.DoubleClickAutoSize = DoubleClickAutoSize;
		columnHeader.Editable = Editable;
		columnHeader.EditorType = EditorType;
		columnHeader.Image = Image;
		columnHeader.ImageAlignment = ImageAlignment;
		columnHeader.MaxInputLength = MaxInputLength;
		columnHeader.MinimumWidth = MinimumWidth;
		columnHeader.SortComparer = SortComparer;
		columnHeader.SortComparerReverse = SortComparerReverse;
		return columnHeader;
	}

	internal void SetBounds(Rectangle bounds)
	{
		ݗ = bounds;
	}

	private void OnImageChanged()
	{
		OnSizeChanged();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetImage()
	{
		TypeDescriptor.GetProperties(this)["Image"].SetValue(this, null);
	}

	protected virtual void OnDataFieldNameChanged()
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeCellsBackColor()
	{
		return !__1715.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetCellsBackColor()
	{
		CellsBackColor = Color.Empty;
	}

	private void OnDisplayIndexChanged(int oldValue, int newValue)
	{
		if (_ᗚ != null)
		{
			_ᗚ.ᝁ(this, newValue, oldValue);
		}
	}

	protected virtual void OnSortingEnabledChanged(bool oldValue, bool newValue)
	{
	}

	protected virtual void OnSortDirectionChanged(eSortDirection oldValue, eSortDirection newValue)
	{
		Invalidate();
		if (__1717)
		{
			if (_ᗚ != null)
			{
				_ᗚ._173C(this);
			}
			switch (newValue)
			{
			case eSortDirection.Ascending:
				Sort(reverse: false);
				break;
			case eSortDirection.Descending:
				Sort(reverse: true);
				break;
			}
		}
	}

	private void Invalidate()
	{
		if (_ᗚ == null)
		{
			return;
		}
		if (_ᗚ.ކ != null)
		{
			if (_ᗚ.ކ.Ԥ != null)
			{
				Rectangle bounds = Bounds;
				if (!_171C)
				{
					int cellHorizontalSpacing = _ᗚ.ކ.CellHorizontalSpacing;
					bounds.Width += cellHorizontalSpacing;
					bounds.X -= cellHorizontalSpacing;
				}
				((Control)_ᗚ.ކ.Ԥ).Invalidate(bounds);
			}
		}
		else if (_ᗚ.ParentNode != null)
		{
			_ᗚ.ParentNode.Invalidate();
		}
	}

	protected virtual void OnTooltipChanged()
	{
	}

	protected virtual void OnTooltip(bool isShown)
	{
	}

	public virtual void ShowToolTip()
	{
		if (base.DesignMode || !Visible)
		{
			return;
		}
		AdvTree advTree = AdvTree;
		if ((advTree != null && !advTree.ShowToolTips) || !ShowToolTips)
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
		AdvTree advTree = AdvTree;
		if (advTree != null)
		{
			((Control)advTree).Invalidate(((Control)advTree).RectangleToClient(bounds), false);
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

	public void AutoSize()
	{
		if (_ᗚ == null)
		{
			throw new InvalidOperationException("ColumnHeader is not parented.");
		}
		AdvTree advTree = AdvTree;
		if (advTree == null)
		{
			throw new InvalidOperationException("Cannot obtain reference to parent tree control, column might not be parented.");
		}
		Width.AutoSize = true;
		advTree.RecalcLayout();
		((Control)advTree).Invalidate();
		Width.ᜪ(ᜫ: false);
		Width.ᜦ(Width.AutoSizeWidth);
		OnHeaderSizeChanged(EventArgs.Empty);
	}

	private void OnHeaderSizeChanged(EventArgs e)
	{
		_170D?.Invoke(this, e);
	}

	private void OnSizeChanged()
	{
		ӱ = true;
		OnHeaderSizeChanged(EventArgs.Empty);
	}

	private void WidthChanged(object sender, EventArgs e)
	{
		OnSizeChanged();
	}

	private void OnIsMouseDownChanged()
	{
		if (_ᗚ != null && _ᗚ.ParentNode != null)
		{
			AdvTree treeControl = _ᗚ.ParentNode.TreeControl;
			if (treeControl != null)
			{
				((Control)treeControl).Invalidate(Bounds);
			}
		}
	}

	protected virtual void OnMouseMove(MouseEventArgs e)
	{
		MouseEventHandler val = મ;
		if (val != null)
		{
			val.Invoke((object)this, e);
		}
	}

	internal void InternalMouseMove(MouseEventArgs e)
	{
		OnMouseMove(e);
	}

	protected virtual void OnMouseEnter(EventArgs e)
	{
		બ?.Invoke(this, e);
	}

	internal void InternalMouseEnter(EventArgs e)
	{
		IsMouseOver = true;
		OnMouseEnter(e);
	}

	protected virtual void OnMouseLeave(EventArgs e)
	{
		ભ?.Invoke(this, e);
	}

	internal void InternalMouseLeave(EventArgs e)
	{
		HideToolTip();
		IsMouseOver = false;
		OnMouseLeave(e);
	}

	protected virtual void OnMouseHover(EventArgs e)
	{
		ય?.Invoke(this, e);
	}

	internal void InternalMouseHover(EventArgs e)
	{
		ShowToolTip();
		OnMouseHover(e);
	}

	internal virtual void OnMouseDown(MouseEventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		HideToolTip();
		if ((int)e.Button == 1048576)
		{
			IsMouseDown = true;
		}
		MouseEventHandler val = ग़;
		if (val != null)
		{
			val.Invoke((object)this, e);
		}
	}

	internal virtual void OnMouseUp(MouseEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Invalid comparison between Unknown and I4
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Invalid comparison between Unknown and I4
		bool isMouseDown = IsMouseDown;
		if ((int)e.Button == 1048576)
		{
			IsMouseDown = false;
		}
		if (__1717 && isMouseDown)
		{
			if ((int)e.Button == 4194304 && __1718 != 0)
			{
				SortDirection = eSortDirection.None;
			}
			else if ((int)e.Button == 1048576)
			{
				if (__1718 == eSortDirection.None || __1718 == eSortDirection.Descending)
				{
					SortDirection = eSortDirection.Ascending;
				}
				else
				{
					SortDirection = eSortDirection.Descending;
				}
			}
		}
		MouseEventHandler val = ફ;
		if (val != null)
		{
			val.Invoke((object)this, e);
		}
	}

	internal virtual void OnDoubleClick(EventArgs e)
	{
		ધ?.Invoke(this, e);
	}

	internal virtual void OnClick(EventArgs e)
	{
		દ?.Invoke(this, e);
	}

	public void Sort()
	{
		Sort(__171D);
		__171D = !__171D;
	}

	public void Sort(bool reverse)
	{
		if (ᜎ != null)
		{
			ᜎ(this, new SortEventArgs(reverse, this));
		}
	}

	protected virtual void OnSortComparerChanged(IComparer oldValue, IComparer newValue)
	{
	}

	protected virtual void OnSortComparerReverseChanged(IComparer oldValue, IComparer newValue)
	{
	}
}
