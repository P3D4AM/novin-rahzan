using System.ComponentModel;
using System.ComponentModel.Design;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar.Design;

internal class TabFormControlActionList : DesignerActionList
{
	private TabFormControlDesigner _Designer;

	public bool CaptionVisible
	{
		get
		{
			return ((TabFormControl)((DesignerActionList)this).Component).CaptionVisible;
		}
		set
		{
			TypeDescriptor.GetProperties(((DesignerActionList)this).Component)["CaptionVisible"].SetValue(((DesignerActionList)this).Component, value);
		}
	}

	public TabFormControlActionList(TabFormControlDesigner designer)
		: base(((ComponentDesigner)designer).Component)
	{
		_Designer = designer;
	}

	public override DesignerActionItemCollection GetSortedActionItems()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Expected O, but got Unknown
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		DesignerActionItemCollection val = new DesignerActionItemCollection();
		string text = "Tab Strip";
		val.Add((DesignerActionItem)new DesignerActionHeaderItem(text));
		string text2 = "Caption";
		val.Add((DesignerActionItem)new DesignerActionHeaderItem(text2));
		val.Add((DesignerActionItem)new DesignerActionMethodItem((DesignerActionList)(object)this, "CreateTab", "Create Tab", text, true));
		val.Add((DesignerActionItem)new DesignerActionMethodItem((DesignerActionList)(object)this, "CreateButton", "Create Button", text, true));
		val.Add((DesignerActionItem)new DesignerActionMethodItem((DesignerActionList)(object)this, "CreateLabel", "Create Label", text, true));
		val.Add((DesignerActionItem)new DesignerActionMethodItem((DesignerActionList)(object)this, "CreateComboBox", "Create ComboBox", text, true));
		val.Add((DesignerActionItem)new DesignerActionMethodItem((DesignerActionList)(object)this, "CreateTextBox", "Create TextBox", text, true));
		val.Add((DesignerActionItem)new DesignerActionMethodItem((DesignerActionList)(object)this, "CreateCaptionButton", "Create Button", text2, true));
		val.Add((DesignerActionItem)new DesignerActionMethodItem((DesignerActionList)(object)this, "CreateCaptionLabel", "Create Label", text2, true));
		val.Add((DesignerActionItem)new DesignerActionPropertyItem("CaptionVisible", "Caption visible?", text2, "Indicates whether caption is visible"));
		return val;
	}

	public void CreateCaptionButton()
	{
		_Designer.CreateCaptionButton();
	}

	public void CreateCaptionLabel()
	{
		_Designer.CreateCaptionLabel();
	}

	public void CreateTab()
	{
		_Designer.CreateTab(string.Empty);
	}

	public void CreateButton()
	{
		_Designer.CreateButton();
	}

	public void CreateLabel()
	{
		_Designer.CreateLabel();
	}

	public void CreateComboBox()
	{
		_Designer.CreateComboBox();
	}

	public void CreateTextBox()
	{
		_Designer.CreateTextBox();
	}
}
