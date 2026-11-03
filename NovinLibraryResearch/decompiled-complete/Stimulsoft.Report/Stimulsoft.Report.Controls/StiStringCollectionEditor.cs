using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Report.Dialogs;

namespace Stimulsoft.Report.Controls;

public class StiStringCollectionEditor : UITypeEditor
{
	private class StringCollectionForm : Form
	{
		private Label instruction;

		private Button btOk;

		private Button btCancel;

		private TextBox textEntry;

		public StiStringCollection items;

		public StringCollectionForm(StiStringCollection items)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			instruction = new Label();
			textEntry = new TextBox();
			btOk = new Button();
			btCancel = new Button();
			InitializeComponent();
			((Control)textEntry).Focus();
			string text = string.Empty;
			foreach (string item in items)
			{
				text = text + item + "\r\n";
			}
			((Control)textEntry).Text = text;
		}

		private void InitializeComponent()
		{
			((Control)instruction).Location = new Point(4, 7);
			((Control)instruction).Size = new Size(422, 14);
			((Control)instruction).TabIndex = 0;
			instruction.TabStop = false;
			((Control)instruction).Text = "Enter the strings in the collection (one per line):";
			((Control)textEntry).Location = new Point(4, 22);
			((Control)textEntry).Size = new Size(422, 244);
			((Control)textEntry).TabIndex = 0;
			((Control)textEntry).Text = "";
			((TextBoxBase)textEntry).AcceptsTab = true;
			textEntry.AcceptsReturn = true;
			((Control)textEntry).AutoSize = false;
			((TextBoxBase)textEntry).Multiline = true;
			((TextBoxBase)textEntry).WordWrap = true;
			((Control)btOk).Location = new Point(260, 274);
			((Control)btOk).Size = new Size(75, 23);
			((Control)btOk).TabIndex = 1;
			((Control)btOk).Text = "Ok";
			btOk.DialogResult = (DialogResult)1;
			((Control)btOk).Click += btOk_click;
			((Control)btCancel).Location = new Point(339, 274);
			((Control)btCancel).Size = new Size(75, 23);
			((Control)btCancel).TabIndex = 2;
			((Control)btCancel).Text = "Cancel";
			btCancel.DialogResult = (DialogResult)2;
			((Form)this).Location = new Point(7, 7);
			((Form)this).AcceptButton = (IButtonControl)(object)btOk;
			((Form)this).CancelButton = (IButtonControl)(object)btCancel;
			((Form)this).ClientSize = new Size(429, 307);
			((Form)this).MaximizeBox = false;
			((Form)this).MinimizeBox = false;
			((Form)this).ShowInTaskbar = false;
			((Form)this).FormBorderStyle = (FormBorderStyle)3;
			((Form)this).StartPosition = (FormStartPosition)1;
			((Form)this).MinimumSize = new Size(300, 200);
			((Form)this).Text = "String Collection Editor";
			((Control)this).Controls.Clear();
			((Control)this).Controls.Add((Control)(object)textEntry);
			((Control)this).Controls.Add((Control)(object)btOk);
			((Control)this).Controls.Add((Control)(object)btCancel);
			((Control)this).Controls.Add((Control)(object)instruction);
		}

		private void btOk_click(object sender, EventArgs e)
		{
			string[] array = ((Control)textEntry).Text.Split(new char[1] { '\r' });
			List<string> list = new List<string>();
			items = new StiStringCollection();
			string[] array2 = array;
			foreach (string text in array2)
			{
				string text2 = text.Trim(new char[1] { '\n' });
				if (text2.Trim().Length != 0)
				{
					items.Add(text2);
				}
			}
		}
	}

	private IWindowsFormsEditorService edSvc;

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null)
		{
			return (UITypeEditorEditStyle)2;
		}
		return ((UITypeEditor)this).GetEditStyle(context);
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Invalid comparison between Unknown and I4
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		StiStringCollection stiStringCollection = value as StiStringCollection;
		StiArrayList stiArrayList = value as StiArrayList;
		if (stiArrayList != null)
		{
			stiStringCollection = new StiStringCollection();
			foreach (object item in stiArrayList)
			{
				stiStringCollection.Add(item.ToString());
			}
		}
		StringCollectionForm stringCollectionForm = new StringCollectionForm(stiStringCollection);
		try
		{
			if ((int)((Form)stringCollectionForm).ShowDialog() == 1)
			{
				if (stiArrayList != null)
				{
					stiArrayList.Clear();
					foreach (string item2 in stringCollectionForm.items)
					{
						stiArrayList.Add(item2);
					}
					return stiArrayList;
				}
				return stringCollectionForm.items;
			}
			return value;
		}
		finally
		{
			((IDisposable)stringCollectionForm)?.Dispose();
		}
	}
}
