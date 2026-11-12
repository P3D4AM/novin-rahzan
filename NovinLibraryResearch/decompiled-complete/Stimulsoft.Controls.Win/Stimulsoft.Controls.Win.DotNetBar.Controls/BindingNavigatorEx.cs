using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[DefaultEvent("RefreshItems")]
[Description("DotNetBar Binding Navigator Control")]
[ClassInterface(ClassInterfaceType.AutoDispatch)]
[DefaultProperty("BindingSource")]
[ToolboxBitmap(typeof(BindingNavigatorEx), "Controls.BindingNavigatorEx.ico")]
[ToolboxItem(false)]
public class BindingNavigatorEx : Bar, ISupportInitialize
{
	private ButtonItem _AddNewButton;

	private bool _AddNewItemUserEnabled;

	private BindingSource _BindingSource;

	private LabelItem _CountLabel;

	private string _CountLabelFormat;

	private ButtonItem _DeleteButton;

	private bool _DeleteButtonUserEnabled;

	private bool _Initializing;

	private ButtonItem _MoveFirstButton;

	private ButtonItem _MoveLastButton;

	private ButtonItem _MoveNextButton;

	private ButtonItem _MovePreviousButton;

	private TextBoxItem _PositionTextBox;

	private bool _IsDisposing;

	[DefaultValue(null)]
	[Category("Items")]
	[TypeConverter(typeof(ReferenceConverter))]
	[Description("BindingNavigatorAddNewItemPropDescr")]
	public ButtonItem AddNewRecordButton
	{
		get
		{
			if (_AddNewButton != null && _AddNewButton.IsDisposed)
			{
				_AddNewButton = null;
			}
			return _AddNewButton;
		}
		set
		{
			if (_AddNewButton != value && value != null)
			{
				value.EnabledChanged += OnAddNewItemEnabledChanged;
				_AddNewItemUserEnabled = value.Enabled;
			}
			WireUpButton(ref _AddNewButton, value, OnAddNew);
		}
	}

	[Category("Data")]
	[Description("Gets or sets the binding source for the navigator.")]
	[DefaultValue(null)]
	[TypeConverter(typeof(ReferenceConverter))]
	public BindingSource BindingSource
	{
		get
		{
			return _BindingSource;
		}
		set
		{
			WireUpBindingSource(ref _BindingSource, value);
		}
	}

	[TypeConverter(typeof(ReferenceConverter))]
	[Category("Items")]
	[Description("Indicates label which represents the items count.")]
	public LabelItem CountLabel
	{
		get
		{
			if (_CountLabel != null && _CountLabel.IsDisposed)
			{
				_CountLabel = null;
			}
			return _CountLabel;
		}
		set
		{
			WireUpLabel(ref _CountLabel, value);
		}
	}

	[Description("Indicates the format string for the label which displays the number of items bound.")]
	[Category("Appearance")]
	public string CountLabelFormat
	{
		get
		{
			return _CountLabelFormat;
		}
		set
		{
			if (_CountLabelFormat != value)
			{
				_CountLabelFormat = value;
				RefreshStateInternal();
			}
		}
	}

	[DefaultValue(null)]
	[Description("Indicates item which deletes current record.")]
	[TypeConverter(typeof(ReferenceConverter))]
	[Category("Items")]
	public ButtonItem DeleteButton
	{
		get
		{
			if (_DeleteButton != null && _DeleteButton.IsDisposed)
			{
				_DeleteButton = null;
			}
			return _DeleteButton;
		}
		set
		{
			if (_DeleteButton != value && value != null)
			{
				value.EnabledChanged += OnDeleteItemEnabledChanged;
				_DeleteButtonUserEnabled = value.Enabled;
			}
			WireUpButton(ref _DeleteButton, value, OnDelete);
		}
	}

	[DefaultValue(null)]
	[TypeConverter(typeof(ReferenceConverter))]
	[Category("Items")]
	[Description("Indicates item which moves to first record.")]
	public ButtonItem MoveFirstButton
	{
		get
		{
			if (_MoveFirstButton != null && _MoveFirstButton.IsDisposed)
			{
				_MoveFirstButton = null;
			}
			return _MoveFirstButton;
		}
		set
		{
			WireUpButton(ref _MoveFirstButton, value, OnMoveFirst);
		}
	}

	[DefaultValue(null)]
	[Category("Items")]
	[Description("Indicates item which moves to last record.")]
	[TypeConverter(typeof(ReferenceConverter))]
	public ButtonItem MoveLastButton
	{
		get
		{
			if (_MoveLastButton != null && _MoveLastButton.IsDisposed)
			{
				_MoveLastButton = null;
			}
			return _MoveLastButton;
		}
		set
		{
			WireUpButton(ref _MoveLastButton, value, OnMoveLast);
		}
	}

	[DefaultValue(null)]
	[TypeConverter(typeof(ReferenceConverter))]
	[Category("Items")]
	[Description("Indicates item which moves to next record")]
	public ButtonItem MoveNextButton
	{
		get
		{
			if (_MoveNextButton != null && _MoveNextButton.IsDisposed)
			{
				_MoveNextButton = null;
			}
			return _MoveNextButton;
		}
		set
		{
			WireUpButton(ref _MoveNextButton, value, OnMoveNext);
		}
	}

	[DefaultValue(null)]
	[Description("Indicates item which moves to previous record")]
	[TypeConverter(typeof(ReferenceConverter))]
	[Category("Items")]
	public ButtonItem MovePreviousButton
	{
		get
		{
			if (_MovePreviousButton != null && _MovePreviousButton.IsDisposed)
			{
				_MovePreviousButton = null;
			}
			return _MovePreviousButton;
		}
		set
		{
			WireUpButton(ref _MovePreviousButton, value, OnMovePrevious);
		}
	}

	[DefaultValue(null)]
	[Category("Items")]
	[Description("Indicates text-box which shows current position.")]
	[TypeConverter(typeof(ReferenceConverter))]
	public TextBoxItem PositionTextBox
	{
		get
		{
			if (_PositionTextBox != null && _PositionTextBox.IsDisposed)
			{
				_PositionTextBox = null;
			}
			return _PositionTextBox;
		}
		set
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			WireUpTextBox(ref _PositionTextBox, value, new KeyEventHandler(OnPositionKey), OnPositionLostFocus);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public BindingNavigatorEx()
		: this(addStandardItems: false)
	{
	}

	public BindingNavigatorEx(bool addStandardItems)
	{
		_CountLabelFormat = "of {0}";
		_AddNewItemUserEnabled = true;
		_DeleteButtonUserEnabled = true;
		if (addStandardItems)
		{
			AddDefaultItems();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public BindingNavigatorEx(IContainer container)
		: this(addStandardItems: false)
	{
		if (container == null)
		{
			throw new ArgumentNullException("container");
		}
		container.Add((IComponent?)this);
	}

	public BindingNavigatorEx(BindingSource bindingSource)
		: this(addStandardItems: true)
	{
		BindingSource = bindingSource;
	}

	private void AcceptNewPosition()
	{
		if (_PositionTextBox != null && _BindingSource != null)
		{
			int num = _BindingSource.Position;
			try
			{
				num = Convert.ToInt32(_PositionTextBox.Text, CultureInfo.CurrentCulture) - 1;
			}
			catch (FormatException)
			{
			}
			catch (OverflowException)
			{
			}
			if (num != _BindingSource.Position)
			{
				_BindingSource.Position = num;
			}
			RefreshStateInternal();
		}
	}

	public virtual void AddDefaultItems()
	{
		MoveFirstButton = new ButtonItem();
		MovePreviousButton = new ButtonItem();
		MoveNextButton = new ButtonItem();
		MoveLastButton = new ButtonItem();
		PositionTextBox = new TextBoxItem();
		CountLabel = new LabelItem();
		AddNewRecordButton = new ButtonItem();
		DeleteButton = new ButtonItem();
		char c = ((string.IsNullOrEmpty(base.Name) || char.IsLower(base.Name[0])) ? 'b' : 'B');
		MoveFirstButton.Name = c + "indingNavigatorMoveFirstItem";
		MovePreviousButton.Name = c + "indingNavigatorMovePreviousItem";
		MoveNextButton.Name = c + "indingNavigatorMoveNextItem";
		MoveLastButton.Name = c + "indingNavigatorMoveLastItem";
		PositionTextBox.Name = c + "indingNavigatorPositionItem";
		CountLabel.Name = c + "indingNavigatorCountItem";
		AddNewRecordButton.Name = c + "indingNavigatorAddNewItem";
		DeleteButton.Name = c + "indingNavigatorDeleteItem";
		MoveFirstButton.Text = "Move first";
		MovePreviousButton.Text = "Move previous";
		MoveNextButton.Text = "Move next";
		MoveLastButton.Text = "Move last";
		AddNewRecordButton.Text = "Add new";
		DeleteButton.Text = "Delete";
		PositionTextBox.AccessibleName = "Position";
		_PositionTextBox.BeginGroup = true;
		_MoveNextButton.BeginGroup = true;
		_MoveFirstButton.Image = (Image)(object)BarFunctions.LoadBitmap("SystemImages.FirstRecord.png");
		_MovePreviousButton.Image = (Image)(object)BarFunctions.LoadBitmap("SystemImages.PreviousRecord.png");
		_MoveNextButton.Image = (Image)(object)BarFunctions.LoadBitmap("SystemImages.NextRecord.png");
		_MoveLastButton.Image = (Image)(object)BarFunctions.LoadBitmap("SystemImages.LastRecord.png");
		_AddNewButton.Image = (Image)(object)BarFunctions.LoadBitmap("SystemImages.NewRecord.png");
		_DeleteButton.Image = (Image)(object)BarFunctions.LoadBitmap("SystemImages.Delete.png");
		_PositionTextBox.TextBoxWidth = 54;
		base.Items.AddRange(new BaseItem[8] { MoveFirstButton, MovePreviousButton, PositionTextBox, CountLabel, MoveNextButton, MoveLastButton, AddNewRecordButton, DeleteButton });
	}

	private void CancelNewPosition()
	{
		RefreshStateInternal();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_IsDisposing = true;
			try
			{
				BindingSource = null;
			}
			finally
			{
				_IsDisposing = false;
			}
		}
		base.Dispose(disposing);
	}

	private void OnAddNew(object sender, EventArgs e)
	{
		if (Validate() && _BindingSource != null)
		{
			_BindingSource.EndEdit();
			_BindingSource.AddNew();
			RefreshStateInternal();
		}
	}

	private void OnAddNewItemEnabledChanged(object sender, EventArgs e)
	{
		if (AddNewRecordButton != null)
		{
			_AddNewItemUserEnabled = _AddNewButton.Enabled;
		}
	}

	private void OnBindingSourceListChanged(object sender, ListChangedEventArgs e)
	{
		RefreshStateInternal();
	}

	private void OnBindingSourceStateChanged(object sender, EventArgs e)
	{
		RefreshStateInternal();
	}

	private void OnDelete(object sender, EventArgs e)
	{
		if (Validate() && _BindingSource != null)
		{
			_BindingSource.RemoveCurrent();
			RefreshStateInternal();
		}
	}

	private void OnDeleteItemEnabledChanged(object sender, EventArgs e)
	{
		if (DeleteButton != null)
		{
			_DeleteButtonUserEnabled = _DeleteButton.Enabled;
		}
	}

	private void OnMoveFirst(object sender, EventArgs e)
	{
		if (Validate() && _BindingSource != null)
		{
			_BindingSource.MoveFirst();
			RefreshStateInternal();
		}
	}

	private void OnMoveLast(object sender, EventArgs e)
	{
		if (Validate() && _BindingSource != null)
		{
			_BindingSource.MoveLast();
			RefreshStateInternal();
		}
	}

	private void OnMoveNext(object sender, EventArgs e)
	{
		if (Validate() && _BindingSource != null)
		{
			_BindingSource.MoveNext();
			RefreshStateInternal();
		}
	}

	private void OnMovePrevious(object sender, EventArgs e)
	{
		if (Validate() && _BindingSource != null)
		{
			_BindingSource.MovePrevious();
			RefreshStateInternal();
		}
	}

	private void OnPositionKey(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		Keys keyCode = e.KeyCode;
		if ((int)keyCode != 13)
		{
			if ((int)keyCode == 27)
			{
				CancelNewPosition();
			}
		}
		else
		{
			AcceptNewPosition();
		}
	}

	private void OnPositionLostFocus(object sender, EventArgs e)
	{
		AcceptNewPosition();
	}

	protected virtual void OnRefreshState()
	{
		RefreshState();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	protected virtual void RefreshState()
	{
		int num;
		int num2;
		bool flag;
		bool flag2;
		if (_BindingSource == null)
		{
			num = 0;
			num2 = 0;
			flag = false;
			flag2 = false;
		}
		else if (_BindingSource.Position == -1)
		{
			num = -1;
			num2 = -1;
			flag = true;
			flag2 = false;
		}
		else
		{
			num = _BindingSource.Count;
			num2 = _BindingSource.Position + 1;
			flag = _BindingSource.AllowNew;
			flag2 = _BindingSource.AllowRemove;
		}
		if (!((Component)(object)this).DesignMode)
		{
			if (MoveFirstButton != null)
			{
				_MoveFirstButton.Enabled = num2 > 1;
			}
			if (MovePreviousButton != null)
			{
				_MovePreviousButton.Enabled = num2 > 1;
			}
			if (MoveNextButton != null)
			{
				_MoveNextButton.Enabled = num2 < num;
			}
			if (MoveLastButton != null)
			{
				_MoveLastButton.Enabled = num2 < num;
			}
			if (AddNewRecordButton != null)
			{
				EventHandler value = OnAddNewItemEnabledChanged;
				_AddNewButton.EnabledChanged -= value;
				_AddNewButton.Enabled = _AddNewItemUserEnabled && flag;
				_AddNewButton.EnabledChanged += value;
			}
			if (DeleteButton != null)
			{
				EventHandler value2 = OnDeleteItemEnabledChanged;
				_DeleteButton.EnabledChanged -= value2;
				_DeleteButton.Enabled = _DeleteButtonUserEnabled && flag2 && num > 0;
				_DeleteButton.EnabledChanged += value2;
			}
			if (PositionTextBox != null)
			{
				_PositionTextBox.Enabled = num2 > 0 && num > 0;
			}
			if (CountLabel != null)
			{
				_CountLabel.Enabled = num > 0;
			}
		}
		if (_PositionTextBox != null)
		{
			_PositionTextBox.Text = num2.ToString(CultureInfo.CurrentCulture);
		}
		if (_CountLabel != null)
		{
			_CountLabel.Text = (((Component)(object)this).DesignMode ? CountLabelFormat : string.Format(CultureInfo.CurrentCulture, CountLabelFormat, new object[1] { num }));
		}
	}

	private void RefreshStateInternal()
	{
		if (!_Initializing && !_IsDisposing)
		{
			OnRefreshState();
		}
	}

	public bool Validate()
	{
		bool flag = false;
		MethodInfo method = ((object)this).GetType().GetMethod("ValidateActiveControl", BindingFlags.Instance | BindingFlags.NonPublic);
		if (method != null)
		{
			return (bool)method.Invoke(this, new object[1] { flag });
		}
		return true;
	}

	private void WireUpBindingSource(ref BindingSource oldBindingSource, BindingSource newBindingSource)
	{
		if (oldBindingSource != newBindingSource)
		{
			if (oldBindingSource != null)
			{
				oldBindingSource.PositionChanged -= OnBindingSourceStateChanged;
				oldBindingSource.CurrentChanged -= OnBindingSourceStateChanged;
				oldBindingSource.CurrentItemChanged -= OnBindingSourceStateChanged;
				oldBindingSource.DataSourceChanged -= OnBindingSourceStateChanged;
				oldBindingSource.DataMemberChanged -= OnBindingSourceStateChanged;
				oldBindingSource.ListChanged -= OnBindingSourceListChanged;
			}
			if (newBindingSource != null)
			{
				newBindingSource.PositionChanged += OnBindingSourceStateChanged;
				newBindingSource.CurrentChanged += OnBindingSourceStateChanged;
				newBindingSource.CurrentItemChanged += OnBindingSourceStateChanged;
				newBindingSource.DataSourceChanged += OnBindingSourceStateChanged;
				newBindingSource.DataMemberChanged += OnBindingSourceStateChanged;
				newBindingSource.ListChanged += OnBindingSourceListChanged;
			}
			oldBindingSource = newBindingSource;
			RefreshStateInternal();
		}
	}

	private void WireUpButton(ref ButtonItem oldButton, ButtonItem newButton, EventHandler clickHandler)
	{
		if (oldButton != newButton)
		{
			if (oldButton != null)
			{
				oldButton.Click -= clickHandler;
			}
			if (newButton != null)
			{
				newButton.Click += clickHandler;
			}
			oldButton = newButton;
			RefreshStateInternal();
		}
	}

	private void WireUpLabel(ref LabelItem oldLabel, LabelItem newLabel)
	{
		if (oldLabel != newLabel)
		{
			oldLabel = newLabel;
			RefreshStateInternal();
		}
	}

	private void WireUpTextBox(ref TextBoxItem oldTextBox, TextBoxItem newTextBox, KeyEventHandler keyUpHandler, EventHandler lostFocusHandler)
	{
		if (oldTextBox != newTextBox)
		{
			if (oldTextBox != null)
			{
				oldTextBox.KeyUp -= keyUpHandler;
				oldTextBox.LostFocus -= lostFocusHandler;
			}
			if (newTextBox != null)
			{
				newTextBox.KeyUp += keyUpHandler;
				newTextBox.LostFocus += lostFocusHandler;
			}
			oldTextBox = newTextBox;
			RefreshStateInternal();
		}
	}

	void ISupportInitialize.BeginInit()
	{
		_Initializing = true;
	}

	void ISupportInitialize.EndInit()
	{
		_Initializing = false;
		RefreshStateInternal();
	}
}
