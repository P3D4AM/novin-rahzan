using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Report.Controls;

[ToolboxItem(false)]
public class StiLookUpBox : StiComboBox
{
	private ArrayList keys = new ArrayList();

	public ArrayList Keys
	{
		get
		{
			return keys;
		}
		set
		{
			keys = value;
		}
	}

	public object SelectedKey
	{
		get
		{
			if (((ComboBox)this).Items == null || ((ComboBox)this).Items.Count == 0)
			{
				return string.Empty;
			}
			int num = 0;
			foreach (object item in ((ComboBox)this).Items)
			{
				if (((ComboBox)this).SelectedItem == item)
				{
					return Keys[num];
				}
				num++;
				if (num == keys.Count)
				{
					return string.Empty;
				}
			}
			return null;
		}
		set
		{
			if (((ComboBox)this).Items == null || ((ComboBox)this).Items.Count == 0)
			{
				return;
			}
			int num = 0;
			foreach (object key in keys)
			{
				if (value == key)
				{
					((ListControl)this).SelectedIndex = num;
					break;
				}
				num++;
				if (num == ((ComboBox)this).Items.Count)
				{
					break;
				}
			}
		}
	}
}
