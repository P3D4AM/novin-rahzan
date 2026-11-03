using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom.ExtendedControls.MultiSelectComboBox;
using PacketParser.Entities;

public class UcMultiSelectComboBox : ComboBox
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<ComboItemWrapper, Guid> _003C_003E9__18_0;

		public static Func<ComboItemWrapper, string> _003C_003E9__20_0;

		public static KeyPressEventHandler _003C_003E9__23_1;

		internal Guid _003Cget_CheckedItems_003Eb__18_0(ComboItemWrapper x)
		{
			return x.Value;
		}

		internal string _003CUpdateText_003Eb__20_0(ComboItemWrapper x)
		{
			return x.Display;
		}

		internal void _003C_002Ector_003Eb__23_1(object s, KeyPressEventArgs e)
		{
			if (e.KeyChar != '\t' && e.KeyChar != '\r')
			{
				e.Handled = true;
			}
		}
	}

	private CheckedListBox _checkedListBox;

	private List<Guid> _pendingCheckedValues;

	public override Font Font
	{
		get
		{
			return ((Control)this).Font;
		}
		set
		{
			((Control)this).Font = value;
			if (_checkedListBox != null)
			{
				((Control)_checkedListBox).Font = ((Control)this).Font;
			}
		}
	}

	public List<Guid> CheckedItems
	{
		get
		{
			try
			{
				List<Guid> pendingCheckedValues = _pendingCheckedValues;
				if (pendingCheckedValues != null && pendingCheckedValues.Any())
				{
					return _pendingCheckedValues;
				}
				List<Guid> list = (from ComboItemWrapper x in (IEnumerable)_checkedListBox.CheckedItems
					select x.Value).ToList();
				if (list != null && list.Any())
				{
					return list;
				}
				if (((ListControl)this).SelectedValue != null)
				{
					return new List<Guid> { (Guid)((ListControl)this).SelectedValue };
				}
				return null;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckedItems", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ExtendedControls\\MultiSelectComboBox\\UcMultiSelectComboBox.cs", 44);
				return null;
			}
		}
	}

	public event Func<Task> OnInsertRequest;

	public event Func<Task> OnShowListRequest;

	public event Func<Task> OnClosed;

	public event Func<Guid, Task> OnEditRequest;

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if ((int)keyData == 13 || (int)keyData == 9)
			{
				((Control)this).Parent.SelectNextControl((Control)(object)this, true, true, true, true);
				return true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ProcessCmdKey", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ExtendedControls\\MultiSelectComboBox\\UcMultiSelectComboBox.cs", 60);
		}
		return ((Control)this).ProcessCmdKey(ref msg, keyData);
	}

	private void UpdateText()
	{
		try
		{
			List<string> list = (from ComboItemWrapper x in (IEnumerable)_checkedListBox.CheckedItems
				select x.Display).ToList();
			((Control)this).RightToLeft = (RightToLeft)1;
			string text = "";
			if ((list?.Count ?? 0) <= 2)
			{
				foreach (string item in list)
				{
					text = text + item + ", ";
				}
			}
			else
			{
				string text2 = list[0];
				string arg = list[1];
				text = text + text2 + ", ";
				text += $"{arg} و {list.Count - 2} گروه دیگر";
			}
			if (text.EndsWith(", "))
			{
				text = text.Substring(0, text.Length - 2);
			}
			((Control)this).Text = text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateText", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ExtendedControls\\MultiSelectComboBox\\UcMultiSelectComboBox.cs", 87);
		}
	}

	private void ApplyCheckedItems(IEnumerable<Guid> values)
	{
		try
		{
			if (values == null || !values.Any())
			{
				return;
			}
			foreach (ComboItemWrapper item in ((IEnumerable)_checkedListBox.Items).Cast<ComboItemWrapper>().ToList())
			{
				bool flag = values.Contains(item.Value);
				int num = ((ObjectCollection)_checkedListBox.Items).IndexOf((object)item);
				_checkedListBox.SetItemChecked(num, flag);
			}
			UpdateText();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplyCheckedItems", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ExtendedControls\\MultiSelectComboBox\\UcMultiSelectComboBox.cs", 104);
		}
	}

	public void SetCheckedItems(IEnumerable<Guid> values)
	{
		try
		{
			if (values != null && values.Any())
			{
				List<Guid> list = values.ToList();
				if (_checkedListBox == null || ((ObjectCollection)_checkedListBox.Items).Count == 0)
				{
					_pendingCheckedValues = list;
				}
				else
				{
					ApplyCheckedItems(list);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetCheckedItems", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ExtendedControls\\MultiSelectComboBox\\UcMultiSelectComboBox.cs", 121);
		}
	}

	public UcMultiSelectComboBox()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		try
		{
			((ComboBox)this).DropDownHeight = 1;
			((ComboBox)this).DropDownStyle = (ComboBoxStyle)1;
			_checkedListBox = new CheckedListBox
			{
				CheckOnClick = true,
				RightToLeft = (RightToLeft)1,
				DrawMode = (DrawMode)1,
				Width = ((Control)this).Width,
				ScrollAlwaysVisible = true,
				HorizontalScrollbar = true
			};
			_checkedListBox.ItemCheck += (ItemCheckEventHandler)delegate
			{
				((Control)this).BeginInvoke((Delegate)new Action(UpdateText));
			};
			((Control)_checkedListBox).KeyDown += new KeyEventHandler(_checkedListBox_KeyDown);
			object obj = _003C_003Ec._003C_003E9__23_1;
			if (obj == null)
			{
				KeyPressEventHandler val = delegate(object s, KeyPressEventArgs e)
				{
					if (e.KeyChar != '\t' && e.KeyChar != '\r')
					{
						e.Handled = true;
					}
				};
				_003C_003Ec._003C_003E9__23_1 = val;
				obj = (object)val;
			}
			((Control)this).KeyPress += (KeyPressEventHandler)obj;
			((Control)this).KeyDown += (KeyEventHandler)delegate(object s, KeyEventArgs e)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0008: Invalid comparison between Unknown and I4
				if ((int)e.KeyCode == 119)
				{
					this.OnShowListRequest?.Invoke();
				}
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ExtendedControls\\MultiSelectComboBox\\UcMultiSelectComboBox.cs", 153);
		}
	}

	private void _checkedListBox_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode <= 13)
			{
				if ((int)keyCode == 9 || (int)keyCode == 13)
				{
					Control parent = ((Control)_checkedListBox).Parent;
					while (parent != null && !(parent is ToolStripDropDown))
					{
						parent = parent.Parent;
					}
					ToolStripDropDown val = (ToolStripDropDown)(object)((parent is ToolStripDropDown) ? parent : null);
					if (val != null)
					{
						val.Close();
					}
					((Control)this).Parent.SelectNextControl((Control)(object)this, true, true, true, true);
					e.Handled = true;
				}
			}
			else if ((int)keyCode != 45)
			{
				if ((int)keyCode != 118)
				{
					if ((int)keyCode == 119)
					{
						this.OnShowListRequest?.Invoke();
					}
				}
				else if (((ListBox)_checkedListBox).SelectedItem != null && ((ListBox)_checkedListBox).SelectedItem is ComboItemWrapper comboItemWrapper)
				{
					this.OnEditRequest?.Invoke(comboItemWrapper.Value);
				}
			}
			else
			{
				this.OnInsertRequest?.Invoke();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "_checkedListBox_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ExtendedControls\\MultiSelectComboBox\\UcMultiSelectComboBox.cs", 183);
		}
	}

	protected override void OnDropDown(EventArgs e)
	{
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Expected O, but got Unknown
		try
		{
			Rectangle rectangle = ((Control)this).RectangleToScreen(((Control)this).ClientRectangle);
			if (((ObjectCollection)_checkedListBox.Items).Count != ((ComboBox)this).Items.Count)
			{
				((ObjectCollection)_checkedListBox.Items).Clear();
				foreach (object item in ((ComboBox)this).Items)
				{
					string display = ((!string.IsNullOrEmpty(((ListControl)this).DisplayMember)) ? item.GetType().GetProperty(((ListControl)this).DisplayMember)?.GetValue(item)?.ToString() : item.ToString());
					object obj = ((!string.IsNullOrEmpty(((ListControl)this).ValueMember)) ? item.GetType().GetProperty(((ListControl)this).ValueMember)?.GetValue(item) : item);
					_checkedListBox.Items.Add((object)new ComboItemWrapper((Guid)obj, display), false);
				}
			}
			ToolStripDropDown val = new ToolStripDropDown();
			ToolStripControlHost val2 = new ToolStripControlHost((Control)(object)_checkedListBox)
			{
				Margin = Padding.Empty,
				Padding = Padding.Empty
			};
			((ListBox)_checkedListBox).BorderStyle = (BorderStyle)0;
			((ToolStrip)val).Items.Add((ToolStripItem)(object)val2);
			((Control)_checkedListBox).Height = Math.Min(200, ((ListBox)_checkedListBox).PreferredHeight);
			((Control)_checkedListBox).Width = ((Control)this).Width;
			val.Closed += (ToolStripDropDownClosedEventHandler)delegate
			{
				this.OnClosed?.Invoke();
			};
			List<Guid> pendingCheckedValues = _pendingCheckedValues;
			if (pendingCheckedValues != null && pendingCheckedValues.Any())
			{
				ApplyCheckedItems(_pendingCheckedValues);
				_pendingCheckedValues.Clear();
			}
			val.Show(rectangle.Left, rectangle.Bottom);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnDropDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ExtendedControls\\MultiSelectComboBox\\UcMultiSelectComboBox.cs", 223);
		}
	}
}
