using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.Editors;

public class VisualInputGroup : VisualGroup
{
	private bool _TabNavigationEnabled = true;

	private bool _EnterNavigationEnabled = true;

	private bool _ArrowNavigationEnabled = true;

	private bool _AutoAdvance;

	private bool _IsReadOnly;

	private bool _AllowEmptyState = true;

	private bool _IsEmpty = true;

	private bool _AutoOverwrite = true;

	private bool _IsUserInput;

	private string _SelectNextInputCharacters = "";

	[DefaultValue(true)]
	public bool TabNavigationEnabled
	{
		get
		{
			return _TabNavigationEnabled;
		}
		set
		{
			_TabNavigationEnabled = value;
		}
	}

	[DefaultValue(true)]
	public bool EnterNavigationEnabled
	{
		get
		{
			return _EnterNavigationEnabled;
		}
		set
		{
			_EnterNavigationEnabled = value;
		}
	}

	[DefaultValue(true)]
	public bool ArrowNavigationEnabled
	{
		get
		{
			return _ArrowNavigationEnabled;
		}
		set
		{
			_ArrowNavigationEnabled = value;
		}
	}

	[DefaultValue(false)]
	public bool AutoAdvance
	{
		get
		{
			return _AutoAdvance;
		}
		set
		{
			_AutoAdvance = value;
		}
	}

	public bool IsReadOnly
	{
		get
		{
			return _IsReadOnly;
		}
		set
		{
			if (_IsReadOnly != value)
			{
				_IsReadOnly = value;
				OnIsReadOnlyChanged();
			}
		}
	}

	[DefaultValue(true)]
	public bool AllowEmptyState
	{
		get
		{
			return _AllowEmptyState;
		}
		set
		{
			if (_AllowEmptyState != value)
			{
				_AllowEmptyState = value;
				OnAllowEmptyStateChanged();
				InvalidateArrange();
			}
		}
	}

	[DefaultValue(true)]
	[Browsable(false)]
	public bool IsEmpty
	{
		get
		{
			return _IsEmpty;
		}
		set
		{
			if (_IsEmpty != value)
			{
				_IsEmpty = value;
				if (_IsEmpty)
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
			return _AutoOverwrite;
		}
		set
		{
			if (_AutoOverwrite != value)
			{
				_AutoOverwrite = value;
				OnAutoOverwriteChanged();
			}
		}
	}

	public bool IsUserInput
	{
		get
		{
			return _IsUserInput;
		}
		set
		{
			_IsUserInput = value;
		}
	}

	public string SelectNextInputCharacters
	{
		get
		{
			return _SelectNextInputCharacters;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			if (_SelectNextInputCharacters != value)
			{
				_SelectNextInputCharacters = value;
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
			if (SelectNextInput())
			{
				return true;
			}
		}
		else if (((int)keyData == 37 && ArrowNavigationEnabled) || ((keyData & 9) == 9 && (keyData & 0x10000) == 65536 && TabNavigationEnabled))
		{
			ValidateInput(base.FocusedItem);
			if (SelectPreviousInput())
			{
				return true;
			}
		}
		return base.OnCmdKey(ref msg, keyData);
	}

	protected override void OnInputComplete()
	{
		if (ValidateInput(base.FocusedItem) && _AutoAdvance && !SelectNextInput())
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
			base.Parent.ProcessInputComplete();
		}
	}

	internal bool SelectNextInput()
	{
		return SelectInputItem(moveForward: true);
	}

	internal bool SelectPreviousInput()
	{
		return SelectInputItem(moveForward: false);
	}

	internal bool SelectInputItem(bool moveForward)
	{
		if (base.FocusedItem is VisualInputGroup)
		{
			VisualInputGroup visualInputGroup = base.FocusedItem as VisualInputGroup;
			if (visualInputGroup.SelectInputItem(moveForward))
			{
				return true;
			}
		}
		VisualCollectionEnumerator visualCollectionEnumerator = new VisualCollectionEnumerator(Items, !moveForward || base.IsRightToLeft);
		visualCollectionEnumerator.CurrentIndex = Items.IndexOf(base.FocusedItem);
		while (visualCollectionEnumerator.MoveNext())
		{
			if (visualCollectionEnumerator.Current is VisualInputBase visualInputBase && CanFocus(visualInputBase))
			{
				base.FocusedItem = visualInputBase;
				return true;
			}
			if (visualCollectionEnumerator.Current is VisualInputGroup)
			{
				VisualInputGroup visualInputGroup2 = visualCollectionEnumerator.Current as VisualInputGroup;
				if (visualInputGroup2.SelectInputItem(moveForward))
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
			SelectInputItem(moveForward: true);
		}
	}

	private void OnIsReadOnlyChanged()
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
					visualInputBase.AutoOverwrite = _AutoOverwrite;
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
		foreach (VisualItem item in Items)
		{
			if (item is VisualInputGroup)
			{
				((VisualInputGroup)item).IsEmpty = true;
			}
			else if (item is VisualInputBase)
			{
				((VisualInputBase)item).IsEmpty = true;
			}
		}
	}

	private void OnAutoOverwriteChanged()
	{
		foreach (VisualItem item in Items)
		{
			if (item is VisualInputBase visualInputBase && visualInputBase.AutoOverwrite != _AutoOverwrite)
			{
				visualInputBase.AutoOverwrite = _AutoOverwrite;
			}
		}
	}

	protected override void OnKeyPress(KeyPressEventArgs e)
	{
		if (_SelectNextInputCharacters.Length > 0 && _SelectNextInputCharacters.Contains(e.KeyChar.ToString()) && SelectNextInput())
		{
			e.Handled = true;
		}
		else
		{
			base.OnKeyPress(e);
		}
	}
}
