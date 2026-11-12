using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class ShortcutsDesigner : UITypeEditor
{
	private class ShortcutsListBox : CheckedListBox
	{
		private struct NameValue
		{
			public string Name;

			public eShortcut Key;

			public NameValue(string name, eShortcut k)
			{
				Name = name;
				Key = k;
			}

			public override string ToString()
			{
				return Name;
			}
		}

		public ShortcutsListBox(ShortcutsCollection editingInstance)
		{
			Array values = Enum.GetValues(typeof(eShortcut));
			for (int i = 1; i < values.Length; i++)
			{
				NameValue nameValue = new NameValue(Enum.GetName(typeof(eShortcut), values.GetValue(i)), (eShortcut)values.GetValue(i));
				if (editingInstance.Contains((eShortcut)values.GetValue(i)))
				{
					((CheckedListBox)this).Items.Add((object)nameValue, (CheckState)1);
				}
				else
				{
					((CheckedListBox)this).Items.Add((object)nameValue, (CheckState)0);
				}
			}
		}

		public ShortcutsCollection Shortcuts()
		{
			ShortcutsCollection shortcutsCollection = new ShortcutsCollection(null);
			foreach (NameValue checkedItem in ((CheckedListBox)this).CheckedItems)
			{
				shortcutsCollection.Add(checkedItem.Key);
			}
			return shortcutsCollection;
		}
	}

	private IWindowsFormsEditorService m_EdSvc;

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null)
		{
			return (UITypeEditorEditStyle)3;
		}
		return ((UITypeEditor)this).GetEditStyle(context);
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null && provider != null)
		{
			m_EdSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
			if (m_EdSvc != null)
			{
				ShortcutsListBox shortcutsListBox = null;
				if (context.Instance is BaseItem)
				{
					shortcutsListBox = new ShortcutsListBox(((BaseItem)context.Instance).Shortcuts);
				}
				else if (context.Instance is ButtonX)
				{
					shortcutsListBox = new ShortcutsListBox(((ButtonX)context.Instance).Shortcuts);
				}
				else if (context.Instance is DotNetBarManager)
				{
					shortcutsListBox = new ShortcutsListBox(((DotNetBarManager)context.Instance).AutoDispatchShortcuts);
				}
				else if (context.Instance != null)
				{
					MessageBox.Show("Unknow control using shortcuts. Cannot edit shortcuts. [" + context.Instance.ToString() + "]");
				}
				else
				{
					MessageBox.Show("Unknow control using shortcuts. Cannot edit shortcuts. [context instance null]");
				}
				if (shortcutsListBox != null)
				{
					m_EdSvc.DropDownControl((Control)(object)shortcutsListBox);
					value = shortcutsListBox.Shortcuts();
					((ShortcutsCollection)value).Parent = context.Instance as BaseItem;
				}
			}
		}
		return value;
	}
}
