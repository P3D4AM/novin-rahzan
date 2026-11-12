using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Data.Functions;
using Stimulsoft.Report;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Design.Editors.Menu;

public class StiIdentsMenu
{
	protected IStiElement Element => EditorForm.Element;

	protected IStiDashboard Dashboard => Element.Page as IStiDashboard;

	protected StiReport Report => Element.Report?.Dictionary?.Report;

	protected StiElementEditorForm EditorForm { get; }

	protected ButtonItem ExpressionMenu { get; }

	protected TextBox ExpressionTextBox { get; }

	protected List<string> Idents { get; }

	public virtual void CreateItems()
	{
		ExpressionMenu.SubItems.Clear();
		if (Idents == null || Idents.Count == 0)
		{
			return;
		}
		int num = 0;
		string text = ((Control)ExpressionTextBox).Text;
		foreach (string ident in Idents)
		{
			bool flag = text != null && "{" + Funcs.ToLowerCase(ident) + "}" == Funcs.ToLowerCase(text);
			ButtonItem buttonItem = new ButtonItem
			{
				AutoCheckOnClick = true,
				Checked = flag,
				FontBold = flag,
				HotFontBold = flag,
				Text = ident,
				Tag = ident
			};
			buttonItem.Click += delegate(object sender, EventArgs args)
			{
				ButtonItem buttonItem2 = sender as ButtonItem;
				buttonItem2.Checked = true;
				string text2 = (string)buttonItem2.Tag;
				((Control)ExpressionTextBox).Text = "{" + text2 + "}";
				((TextBoxBase)ExpressionTextBox).Select(0, 0);
				EditorForm.RefreshElementView();
			};
			ExpressionMenu.SubItems.Add(buttonItem);
			num++;
		}
	}

	public StiIdentsMenu(ButtonItem expressionMenu, List<string> idents, TextBox expressionTextBox, StiElementEditorForm editorForm)
	{
		Idents = idents;
		ExpressionMenu = expressionMenu;
		ExpressionTextBox = expressionTextBox;
		EditorForm = editorForm;
	}
}
