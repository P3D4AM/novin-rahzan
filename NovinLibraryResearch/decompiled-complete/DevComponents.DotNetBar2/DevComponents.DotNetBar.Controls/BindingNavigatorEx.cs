using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

[ToolboxItem(true)]
[DefaultProperty("BindingSource")]
[DefaultEvent("RefreshItems")]
[ClassInterface(ClassInterfaceType.AutoDispatch)]
[ToolboxBitmap(typeof(BindingNavigatorEx), "Controls.BindingNavigatorEx.ico")]
[Designer("DevComponents.DotNetBar.Design.BindingNavigatorExDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[Description("DotNetBar Binding Navigator Control")]
public class BindingNavigatorEx : Bar, ISupportInitialize
{
	private ButtonItem __2A8D;

	private bool __2A8E;

	private BindingSource __2A8F;

	private LabelItem __2A90;

	private string __2A91;

	private ButtonItem __2A92;

	private bool __2A93;

	private bool _ᠭ;

	private ButtonItem __2A94;

	private ButtonItem __2A95;

	private ButtonItem __2A96;

	private ButtonItem __2A97;

	private TextBoxItem __2A98;

	private bool __237D;

	[TypeConverter(typeof(ReferenceConverter))]
	[DefaultValue(null)]
	[Category("Items")]
	[Description("BindingNavigatorAddNewItemPropDescr")]
	public ButtonItem AddNewRecordButton
	{
		get
		{
			if (__2A8D != null && __2A8D.IsDisposed)
			{
				__2A8D = null;
			}
			return __2A8D;
		}
		set
		{
			if (__2A8D != value && value != null)
			{
				value.EnabledChanged += _2A9C;
				__2A8E = value.Enabled;
			}
			_2AAB(ref __2A8D, value, _2A9B);
		}
	}

	[Description("Gets or sets the binding source for the navigator.")]
	[Category("Data")]
	[DefaultValue(null)]
	[TypeConverter(typeof(ReferenceConverter))]
	public BindingSource BindingSource
	{
		get
		{
			return __2A8F;
		}
		set
		{
			_2AA8(ref __2A8F, value);
		}
	}

	[Description("Indicates label which represents the items count.")]
	[Category("Items")]
	[TypeConverter(typeof(ReferenceConverter))]
	public LabelItem CountLabel
	{
		get
		{
			if (__2A90 != null && __2A90.IsDisposed)
			{
				__2A90 = null;
			}
			return __2A90;
		}
		set
		{
			_2AAF(ref __2A90, value);
		}
	}

	[Category("Appearance")]
	[Description("Indicates the format string for the label which displays the number of items bound.")]
	public string CountLabelFormat
	{
		get
		{
			return __2A91;
		}
		set
		{
			if (__2A91 != value)
			{
				__2A91 = value;
				_2AA7();
			}
		}
	}

	[Description("Indicates item which deletes current record.")]
	[TypeConverter(typeof(ReferenceConverter))]
	[DefaultValue(null)]
	[Category("Items")]
	public ButtonItem DeleteButton
	{
		get
		{
			if (__2A92 != null && __2A92.IsDisposed)
			{
				__2A92 = null;
			}
			return __2A92;
		}
		set
		{
			if (__2A92 != value && value != null)
			{
				value.EnabledChanged += _2AA0;
				__2A93 = value.Enabled;
			}
			_2AAB(ref __2A92, value, _2A9F);
		}
	}

	[Category("Items")]
	[TypeConverter(typeof(ReferenceConverter))]
	[Description("Indicates item which moves to first record.")]
	[DefaultValue(null)]
	public ButtonItem MoveFirstButton
	{
		get
		{
			if (__2A94 != null && __2A94.IsDisposed)
			{
				__2A94 = null;
			}
			return __2A94;
		}
		set
		{
			_2AAB(ref __2A94, value, _2AA1);
		}
	}

	[Category("Items")]
	[TypeConverter(typeof(ReferenceConverter))]
	[DefaultValue(null)]
	[Description("Indicates item which moves to last record.")]
	public ButtonItem MoveLastButton
	{
		get
		{
			if (__2A95 != null && __2A95.IsDisposed)
			{
				__2A95 = null;
			}
			return __2A95;
		}
		set
		{
			_2AAB(ref __2A95, value, _2AA2);
		}
	}

	[Category("Items")]
	[Description("Indicates item which moves to next record")]
	[TypeConverter(typeof(ReferenceConverter))]
	[DefaultValue(null)]
	public ButtonItem MoveNextButton
	{
		get
		{
			if (__2A96 != null && __2A96.IsDisposed)
			{
				__2A96 = null;
			}
			return __2A96;
		}
		set
		{
			_2AAB(ref __2A96, value, _2AA3);
		}
	}

	[DefaultValue(null)]
	[Category("Items")]
	[TypeConverter(typeof(ReferenceConverter))]
	[Description("Indicates item which moves to previous record")]
	public ButtonItem MovePreviousButton
	{
		get
		{
			if (__2A97 != null && __2A97.IsDisposed)
			{
				__2A97 = null;
			}
			return __2A97;
		}
		set
		{
			_2AAB(ref __2A97, value, _2AA4);
		}
	}

	[Description("Indicates text-box which shows current position.")]
	[DefaultValue(null)]
	[Category("Items")]
	[TypeConverter(typeof(ReferenceConverter))]
	public TextBoxItem PositionTextBox
	{
		get
		{
			if (__2A98 != null && __2A98.IsDisposed)
			{
				__2A98 = null;
			}
			return __2A98;
		}
		set
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			_2AB2(ref __2A98, value, new KeyEventHandler(_2AA5), _2AA6);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public BindingNavigatorEx()
		: this(addStandardItems: false)
	{
	}

	public BindingNavigatorEx(bool addStandardItems)
	{
		__2A91 = "of {0}";
		__2A8E = true;
		__2A93 = true;
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

	private void _2A99()
	{
		if (__2A98 != null && __2A8F != null)
		{
			int num = __2A8F.Position;
			try
			{
				num = Convert.ToInt32(__2A98.Text, CultureInfo.CurrentCulture) - 1;
			}
			catch (FormatException)
			{
			}
			catch (OverflowException)
			{
			}
			if (num != __2A8F.Position)
			{
				__2A8F.Position = num;
			}
			_2AA7();
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
		__2A98.BeginGroup = true;
		__2A96.BeginGroup = true;
		__2A94.Image = (Image)(object)BarFunctions.ই("SystemImages.FirstRecord.png");
		__2A97.Image = (Image)(object)BarFunctions.ই("SystemImages.PreviousRecord.png");
		__2A96.Image = (Image)(object)BarFunctions.ই("SystemImages.NextRecord.png");
		__2A95.Image = (Image)(object)BarFunctions.ই("SystemImages.LastRecord.png");
		__2A8D.Image = (Image)(object)BarFunctions.ই("SystemImages.NewRecord.png");
		__2A92.Image = (Image)(object)BarFunctions.ই("SystemImages.Delete.png");
		__2A98.TextBoxWidth = 54;
		base.Items.AddRange(new BaseItem[8] { MoveFirstButton, MovePreviousButton, PositionTextBox, CountLabel, MoveNextButton, MoveLastButton, AddNewRecordButton, DeleteButton });
	}

	private void _2A9A()
	{
		_2AA7();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			__237D = true;
			try
			{
				BindingSource = null;
			}
			finally
			{
				__237D = false;
			}
		}
		base.Dispose(disposing);
	}

	private void _2A9B(object _٠, EventArgs ؿ)
	{
		if (Validate() && __2A8F != null)
		{
			__2A8F.EndEdit();
			__2A8F.AddNew();
			_2AA7();
		}
	}

	private void _2A9C(object _٠, EventArgs ؿ)
	{
		if (AddNewRecordButton != null)
		{
			__2A8E = __2A8D.Enabled;
		}
	}

	private void _2A9D(object _٠, ListChangedEventArgs ؿ)
	{
		_2AA7();
	}

	private void _2A9E(object _٠, EventArgs ؿ)
	{
		_2AA7();
	}

	private void _2A9F(object _٠, EventArgs ؿ)
	{
		if (Validate() && __2A8F != null)
		{
			__2A8F.RemoveCurrent();
			_2AA7();
		}
	}

	private void _2AA0(object _٠, EventArgs ؿ)
	{
		if (DeleteButton != null)
		{
			__2A93 = __2A92.Enabled;
		}
	}

	private void _2AA1(object _٠, EventArgs ؿ)
	{
		if (Validate() && __2A8F != null)
		{
			__2A8F.MoveFirst();
			_2AA7();
		}
	}

	private void _2AA2(object _٠, EventArgs ؿ)
	{
		if (Validate() && __2A8F != null)
		{
			__2A8F.MoveLast();
			_2AA7();
		}
	}

	private void _2AA3(object _٠, EventArgs ؿ)
	{
		if (Validate() && __2A8F != null)
		{
			__2A8F.MoveNext();
			_2AA7();
		}
	}

	private void _2AA4(object _٠, EventArgs ؿ)
	{
		if (Validate() && __2A8F != null)
		{
			__2A8F.MovePrevious();
			_2AA7();
		}
	}

	private void _2AA5(object _٠, KeyEventArgs ؿ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		Keys keyCode = ؿ.KeyCode;
		if ((int)keyCode != 13)
		{
			if ((int)keyCode == 27)
			{
				_2A9A();
			}
		}
		else
		{
			_2A99();
		}
	}

	private void _2AA6(object _٠, EventArgs ؿ)
	{
		_2A99();
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
		if (__2A8F == null)
		{
			num = 0;
			num2 = 0;
			flag = false;
			flag2 = false;
		}
		else if (__2A8F.Position == -1)
		{
			num = -1;
			num2 = -1;
			flag = true;
			flag2 = false;
		}
		else
		{
			num = __2A8F.Count;
			num2 = __2A8F.Position + 1;
			flag = __2A8F.AllowNew;
			flag2 = __2A8F.AllowRemove;
		}
		if (!((Component)(object)this).DesignMode)
		{
			if (MoveFirstButton != null)
			{
				__2A94.Enabled = num2 > 1;
			}
			if (MovePreviousButton != null)
			{
				__2A97.Enabled = num2 > 1;
			}
			if (MoveNextButton != null)
			{
				__2A96.Enabled = num2 < num;
			}
			if (MoveLastButton != null)
			{
				__2A95.Enabled = num2 < num;
			}
			if (AddNewRecordButton != null)
			{
				EventHandler value = _2A9C;
				__2A8D.EnabledChanged -= value;
				__2A8D.Enabled = __2A8E && flag;
				__2A8D.EnabledChanged += value;
			}
			if (DeleteButton != null)
			{
				EventHandler value2 = _2AA0;
				__2A92.EnabledChanged -= value2;
				__2A92.Enabled = __2A93 && flag2 && num > 0;
				__2A92.EnabledChanged += value2;
			}
			if (PositionTextBox != null)
			{
				__2A98.Enabled = num2 > 0 && num > 0;
			}
			if (CountLabel != null)
			{
				__2A90.Enabled = num > 0;
			}
		}
		if (__2A98 != null)
		{
			__2A98.Text = num2.ToString(CultureInfo.CurrentCulture);
		}
		if (__2A90 != null)
		{
			__2A90.Text = (((Component)(object)this).DesignMode ? CountLabelFormat : string.Format(CultureInfo.CurrentCulture, CountLabelFormat, new object[1] { num }));
		}
	}

	private void _2AA7()
	{
		if (!_ᠭ && !__237D)
		{
			OnRefreshState();
		}
	}

	public bool Validate()
	{
		bool flag = false;
		MethodInfo method = ((object)this).GetType().GetMethod("ValidateActiveControl", BindingFlags.Instance | BindingFlags.NonPublic);
		if ((object)method != null)
		{
			return (bool)method.Invoke(this, new object[1] { flag });
		}
		return true;
	}

	private void _2AA8(ref BindingSource _2AA9, BindingSource _2AAA)
	{
		if (_2AA9 != _2AAA)
		{
			if (_2AA9 != null)
			{
				_2AA9.PositionChanged -= _2A9E;
				_2AA9.CurrentChanged -= _2A9E;
				_2AA9.CurrentItemChanged -= _2A9E;
				_2AA9.DataSourceChanged -= _2A9E;
				_2AA9.DataMemberChanged -= _2A9E;
				_2AA9.ListChanged -= _2A9D;
			}
			if (_2AAA != null)
			{
				_2AAA.PositionChanged += _2A9E;
				_2AAA.CurrentChanged += _2A9E;
				_2AAA.CurrentItemChanged += _2A9E;
				_2AAA.DataSourceChanged += _2A9E;
				_2AAA.DataMemberChanged += _2A9E;
				_2AAA.ListChanged += _2A9D;
			}
			_2AA9 = _2AAA;
			_2AA7();
		}
	}

	private void _2AAB(ref ButtonItem _2AAC, ButtonItem _2AAD, EventHandler _2AAE)
	{
		if (_2AAC != _2AAD)
		{
			if (_2AAC != null)
			{
				_2AAC.Click -= _2AAE;
			}
			if (_2AAD != null)
			{
				_2AAD.Click += _2AAE;
			}
			_2AAC = _2AAD;
			_2AA7();
		}
	}

	private void _2AAF(ref LabelItem _2AB0, LabelItem _2AB1)
	{
		if (_2AB0 != _2AB1)
		{
			_2AB0 = _2AB1;
			_2AA7();
		}
	}

	private void _2AB2(ref TextBoxItem _2AB3, TextBoxItem _2AB4, KeyEventHandler _2AB5, EventHandler _2AB6)
	{
		if (_2AB3 != _2AB4)
		{
			if (_2AB3 != null)
			{
				_2AB3.KeyUp -= _2AB5;
				_2AB3.LostFocus -= _2AB6;
			}
			if (_2AB4 != null)
			{
				_2AB4.KeyUp += _2AB5;
				_2AB4.LostFocus += _2AB6;
			}
			_2AB3 = _2AB4;
			_2AA7();
		}
	}

	private void ህ()
	{
		_ᠭ = true;
	}

	void ISupportInitialize.BeginInit()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ህ
		this.ህ();
	}

	private void ሆ()
	{
		_ᠭ = false;
		_2AA7();
	}

	void ISupportInitialize.EndInit()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ሆ
		this.ሆ();
	}
}
