using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Design;

public class StiCultureEditor : UITypeEditor
{
	private string selectedCulture;

	private IWindowsFormsEditorService edSvc;

	private static Hashtable hashtable;

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null)
		{
			return (UITypeEditorEditStyle)3;
		}
		return ((UITypeEditor)this).GetEditStyle(context);
	}

	protected void ListBox_Click(object sender, EventArgs e)
	{
		ListBox val = (ListBox)((sender is ListBox) ? sender : null);
		if (val.SelectedItem != null)
		{
			selectedCulture = val.SelectedItem.ToString();
			edSvc.CloseDropDown();
		}
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Expected O, but got Unknown
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		StiHintTextBox stiHintTextBox = new StiHintTextBox();
		((Control)stiHintTextBox).Dock = (DockStyle)5;
		stiHintTextBox.Hint = StiLocalization.Get("Editor", "TypeToSearch");
		StiHintTextBox stiHintTextBox2 = stiHintTextBox;
		((Control)stiHintTextBox2).TextChanged += MetricsSearch_TextChanged;
		((Control)stiHintTextBox2).Focus();
		Panel val = new Panel
		{
			Dock = (DockStyle)1,
			Padding = new Padding(StiScale.XXI(4.0))
		};
		((Control)val).Height = StiScale.YYI(30.0);
		((Control)val).Controls.Add((Control)(object)stiHintTextBox2);
		Panel val2 = new Panel
		{
			Dock = (DockStyle)1,
			Size = StiScale.I(new Size(250, 400))
		};
		StiDotSeparator stiDotSeparator = new StiDotSeparator();
		((Control)stiDotSeparator).Dock = (DockStyle)1;
		StiDotSeparator stiDotSeparator2 = stiDotSeparator;
		ListBox val3 = new ListBox
		{
			Width = StiScale.XXI(250.0),
			ItemHeight = StiScale.YYI(20.0),
			BorderStyle = (BorderStyle)0,
			Dock = (DockStyle)5
		};
		((Control)val3).DoubleClick += ListBox_Click;
		val3.Click += ListBox_Click;
		BuildListBox(val3, ((Control)stiHintTextBox2).Text, value);
		((Control)val2).Controls.Add((Control)(object)val3);
		((Control)val2).Controls.Add((Control)(object)stiDotSeparator2);
		((Control)val2).Controls.Add((Control)(object)val);
		edSvc.DropDownControl((Control)(object)val2);
		if (selectedCulture != null && hashtable.ContainsKey(selectedCulture))
		{
			return hashtable[selectedCulture];
		}
		return null;
	}

	private void MetricsSearch_TextChanged(object sender, EventArgs e)
	{
		StiHintTextBox stiHintTextBox = sender as StiHintTextBox;
		Control parent = ((Control)stiHintTextBox).Parent.Parent;
		Panel val = (Panel)(object)((parent is Panel) ? parent : null);
		Control obj = ((Control)val).Controls[0];
		ListBox val2 = (ListBox)(object)((obj is ListBox) ? obj : null);
		BuildListBox(val2, ((Control)stiHintTextBox).Text, val2.SelectedItem);
	}

	private void BuildListBox(ListBox listBox, string search, object value)
	{
		CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
		BuildCash(cultures);
		listBox.BeginUpdate();
		listBox.Items.Clear();
		List<string> list = new List<string>();
		CultureInfo[] array = cultures;
		foreach (CultureInfo cultureInfo in array)
		{
			string text = cultureInfo.DisplayName + "  (" + cultureInfo.Name + ")";
			if (string.IsNullOrEmpty(search) || text.ToLower().Contains(search.ToLower()))
			{
				list.Add(text);
				if (cultureInfo.Name.Equals(value))
				{
					selectedCulture = text;
				}
			}
		}
		list.Sort();
		listBox.Items.Add((object)string.Empty);
		foreach (string item in list)
		{
			listBox.Items.Add((object)item);
		}
		if (selectedCulture != null)
		{
			listBox.SelectedItem = selectedCulture;
		}
		listBox.EndUpdate();
	}

	private static void BuildCash(CultureInfo[] cultures)
	{
		if (hashtable == null)
		{
			hashtable = new Hashtable();
			foreach (CultureInfo cultureInfo in cultures)
			{
				string key = cultureInfo.DisplayName + "  (" + cultureInfo.Name + ")";
				hashtable.Add(key, cultureInfo.Name);
			}
		}
	}
}
