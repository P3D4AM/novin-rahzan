using System.ComponentModel;
using System.Windows.Forms;

namespace DevComponents.Editors;

public class VisualInputGroup : VisualGroup
{
	private bool __22CA = true;

	private bool __22CB = true;

	private bool __22CC = true;

	private bool __17F7;

	private bool _ᣇ;

	private bool __22CD = true;

	private bool __22CE = true;

	private bool __22CF;

	private bool __22D0 = true;

	private bool __22D1;

	private string __22D2 = "";

	[DefaultValue(true)]
	public bool TabNavigationEnabled
	{
		get
		{
			return __22CA;
		}
		set
		{
			__22CA = value;
		}
	}

	[DefaultValue(true)]
	public bool EnterNavigationEnabled
	{
		get
		{
			return __22CB;
		}
		set
		{
			__22CB = value;
		}
	}

	[DefaultValue(true)]
	public bool ArrowNavigationEnabled
	{
		get
		{
			return __22CC;
		}
		set
		{
			__22CC = value;
		}
	}

	[DefaultValue(false)]
	public bool AutoAdvance
	{
		get
		{
			return __17F7;
		}
		set
		{
			__17F7 = value;
		}
	}

	public bool IsReadOnly
	{
		get
		{
			return _ᣇ;
		}
		set
		{
			if (_ᣇ != value)
			{
				_ᣇ = value;
				_22D7();
			}
		}
	}

	[DefaultValue(true)]
	public bool AllowEmptyState
	{
		get
		{
			return __22CD;
		}
		set
		{
			if (__22CD != value)
			{
				__22CD = value;
				OnAllowEmptyStateChanged();
				InvalidateArrange();
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(true)]
	public bool IsEmpty
	{
		get
		{
			return __22CE;
		}
		set
		{
			if (__22CE != value)
			{
				__22CE = value;
				if (__22CE)
				{
					ResetValue();
				}
				if (base.Parent is VisualInputGroup)
				{
					((VisualInputGroup)base.Parent).UpdateIsEmpty();
				}
			}
		}
	}

	public bool AutoOverwrite
	{
		get
		{
			return __22D0;
		}
		set
		{
			if (__22D0 != value)
			{
				__22D0 = value;
				_22D8();
			}
		}
	}

	public bool IsUserInput
	{
		get
		{
			return __22D1;
		}
		set
		{
			__22D1 = value;
		}
	}

	public string SelectNextInputCharacters
	{
		get
		{
			return __22D2;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			if (__22D2 != value)
			{
				__22D2 = value;
			}
		}
	}

	public void ResetInputPosition()
	{
		foreach (VisualItem item in Items)
		{
			if (item is VisualInputBase visualInputBase)
			{
				visualInputBase.ResetInputPosition();
			}
		}
	}

	protected override bool OnCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Invalid comparison between Unknown and I4
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Invalid comparison between Unknown and I4
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Invalid comparison between Unknown and I4
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Invalid comparison between Unknown and I4
		if (((int)keyData == 9 && (Control.ModifierKeys & 0x10000) != 65536 && TabNavigationEnabled) || ((int)keyData == 13 && EnterNavigationEnabled) || ((int)keyData == 39 && ArrowNavigationEnabled))
		{
			ValidateInput(base.FocusedItem);
			if (_22D3())
			{
				return true;
			}
		}
		else if (((int)keyData == 37 && ArrowNavigationEnabled) || ((keyData & 9) == 9 && (keyData & 0x10000) == 65536 && TabNavigationEnabled))
		{
			ValidateInput(base.FocusedItem);
			if (_22D4())
			{
				return true;
			}
		}
		return base.OnCmdKey(ref msg, keyData);
	}

	protected override void OnInputComplete()
	{
		if (ValidateInput(base.FocusedItem) && __17F7 && !_22D3())
		{
			OnGroupInputComplete();
		}
		base.OnInputComplete();
	}

	protected virtual bool ValidateInput(VisualItem inputItem)
	{
		return true;
	}

	protected virtual void OnGroupInputComplete()
	{
		if (base.Parent != null)
		{
			base.Parent.ᡵ();
		}
	}

	internal bool _22D3()
	{
		return _22D5(_22D6: true);
	}

	internal bool _22D4()
	{
		return _22D5(_22D6: false);
	}

	internal bool _22D5(bool _22D6)
	{
		if (base.FocusedItem is VisualInputGroup)
		{
			VisualInputGroup visualInputGroup = base.FocusedItem as VisualInputGroup;
			if (visualInputGroup._22D5(_22D6))
			{
				return true;
			}
		}
		ᢇ ᢇ2 = new ᢇ(Items, !_22D6 || base.IsRightToLeft);
		ᢇ2.ᢈ = Items.IndexOf(base.FocusedItem);
		while (ᢇ2.MoveNext())
		{
			if (ᢇ2.Current is VisualInputBase visualInputBase && CanFocus(visualInputBase))
			{
				base.FocusedItem = visualInputBase;
				return true;
			}
			if (ᢇ2.Current is VisualInputGroup)
			{
				VisualInputGroup visualInputGroup2 = ᢇ2.Current as VisualInputGroup;
				if (visualInputGroup2._22D5(_22D6))
				{
					base.FocusedItem = visualInputGroup2;
					return true;
				}
			}
		}
		return false;
	}

	protected override void OnGroupFocused()
	{
		if (base.FocusedItem == null)
		{
			_22D5(_22D6: true);
		}
	}

	private void _22D7()
	{
		for (int i = 0; i < Items.Count; i++)
		{
			if (Items[i] is VisualInputBase visualInputBase)
			{
				visualInputBase.IsReadOnly = IsReadOnly;
			}
		}
	}

	protected override void OnItemsCollectionChanged(CollectionChangedInfo collectionChangedInfo)
	{
		if ((collectionChangedInfo.ChangeType == eCollectionChangeType.Adding || collectionChangedInfo.ChangeType == eCollectionChangeType.Removing || collectionChangedInfo.ChangeType == eCollectionChangeType.Clearing) && collectionChangedInfo.Added != null)
		{
			VisualItem[] added = collectionChangedInfo.Added;
			foreach (VisualItem visualItem in added)
			{
				if (visualItem is VisualInputBase visualInputBase)
				{
					visualInputBase.IsReadOnly = IsReadOnly;
					visualInputBase.AutoOverwrite = __22D0;
				}
			}
		}
		base.OnItemsCollectionChanged(collectionChangedInfo);
	}

	protected virtual void OnAllowEmptyStateChanged()
	{
		foreach (VisualItem item in Items)
		{
			if (item is VisualNumericInput)
			{
				((VisualNumericInput)item).AllowEmptyState = AllowEmptyState;
			}
		}
	}

	protected override void OnInputChanged(VisualInputBase input)
	{
		UpdateIsEmpty();
		base.OnInputChanged(input);
	}

	public virtual void UpdateIsEmpty()
	{
		bool isEmpty = true;
		foreach (VisualItem item in Items)
		{
			if (item is VisualInputBase && !((VisualInputBase)item).IsEmpty)
			{
				isEmpty = false;
				break;
			}
			if (item is VisualInputGroup && !((VisualInputGroup)item).IsEmpty)
			{
				isEmpty = false;
				break;
			}
		}
		IsEmpty = isEmpty;
	}

	protected virtual void ResetValue()
	{
		__22CF = true;
		try
		{
			VisualItem[] array = new VisualItem[Items.Count];
			Items._0CD0(array);
			VisualItem[] array2 = array;
			foreach (VisualItem visualItem in array2)
			{
				if (visualItem is VisualInputGroup)
				{
					((VisualInputGroup)visualItem).IsEmpty = true;
				}
				else if (visualItem is VisualInputBase)
				{
					((VisualInputBase)visualItem).IsEmpty = true;
				}
			}
		}
		finally
		{
			__22CF = false;
		}
	}

	private void _22D8()
	{
		foreach (VisualItem item in Items)
		{
			if (item is VisualInputBase visualInputBase && visualInputBase.AutoOverwrite != __22D0)
			{
				visualInputBase.AutoOverwrite = __22D0;
			}
		}
	}

	protected override void OnKeyPress(KeyPressEventArgs e)
	{
		if (__22D2.Length > 0 && __22D2.Contains(e.KeyChar.ToString()) && _22D3())
		{
			e.Handled = true;
		}
		else
		{
			base.OnKeyPress(e);
		}
	}
}
