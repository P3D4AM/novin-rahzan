using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls;

public class StiStringCollectionEditor : CollectionEditor
{
	private class StringCollectionForm : CollectionForm
	{
		private StiStringCollectionEditor editor;

		private Label instruction;

		private StiButton btOk;

		private StiButton btCancel;

		private StiTextBox textEntry;

		public StringCollectionForm(CollectionEditor editor)
			: base(editor)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			instruction = new Label();
			textEntry = new StiTextBox();
			btOk = new StiButton();
			btCancel = new StiButton();
			this.editor = null;
			this.editor = (StiStringCollectionEditor)(object)editor;
			InitializeComponent();
			((Control)textEntry).Focus();
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
			textEntry.AcceptsTab = true;
			textEntry.AcceptsReturn = true;
			((Control)textEntry).AutoSize = false;
			textEntry.Multiline = true;
			textEntry.WordWrap = true;
			((Control)btOk).Location = new Point(260, 274);
			((Control)btOk).Size = new Size(75, 23);
			((Control)btOk).TabIndex = 1;
			((Control)btOk).Text = "Ok";
			((Button)btOk).DialogResult = (DialogResult)1;
			((Control)btOk).Click += btOk_click;
			((Control)btCancel).Location = new Point(339, 274);
			((Control)btCancel).Size = new Size(75, 23);
			((Control)btCancel).TabIndex = 2;
			((Control)btCancel).Text = "Cancel";
			((Button)btCancel).DialogResult = (DialogResult)2;
			((Form)this).Location = new Point(7, 7);
			((Form)this).AcceptButton = (IButtonControl)(object)btOk;
			((Form)this).AutoScaleBaseSize = new Size(5, 13);
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
			if (((Control)textEntry).Text.Length != 0)
			{
				string[] array = ((Control)textEntry).Text.Split(new char[1] { '\r' });
				object[] array2 = (object[])Array.CreateInstance(typeof(object), array.Length);
				int num = 0;
				string[] array3 = array;
				foreach (string text in array3)
				{
					array2[num++] = text.Trim(new char[1] { '\n' });
				}
				int num2 = array.Length;
				int num3 = array.Length - 1;
				while (num3 >= 0 && array[num3].Trim().Length == 0)
				{
					num2--;
					num3--;
				}
				if (num2 == array.Length)
				{
					((CollectionForm)this).Items = array2;
				}
				else
				{
					Array.Copy(array2, 0, ((CollectionForm)this).Items, 0, num2);
				}
			}
			else
			{
				((CollectionForm)this).Items = new object[0];
			}
		}

		protected override void OnEditValueChanged()
		{
			string text = string.Empty;
			object[] items = ((CollectionForm)this).Items;
			foreach (object obj in items)
			{
				string text2 = obj.ToString();
				text = text + text2 + "\r\n";
			}
			((Control)textEntry).Text = text;
		}
	}

	public StiStringCollectionEditor(Type type)
		: base(type)
	{
	}

	protected override CollectionForm CreateCollectionForm()
	{
		return (CollectionForm)(object)new StringCollectionForm((CollectionEditor)(object)this);
	}

	protected override object[] GetItems(object editValue)
	{
		if (editValue == null || !(editValue is ICollection))
		{
			return new object[0];
		}
		ArrayList arrayList = new ArrayList();
		ICollection collection = (ICollection)editValue;
		foreach (object item in collection)
		{
			arrayList.Add(item);
		}
		object[] array = new object[arrayList.Count];
		arrayList.CopyTo(array, 0);
		return array;
	}
}
