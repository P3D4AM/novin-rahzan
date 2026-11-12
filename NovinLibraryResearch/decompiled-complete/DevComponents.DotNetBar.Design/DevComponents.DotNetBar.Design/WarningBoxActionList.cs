using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar.Design;

public class WarningBoxActionList : DesignerActionList
{
	private WarningBoxDesigner _Designer;

	public bool OptionsButtonVisible
	{
		get
		{
			return ((WarningBox)((DesignerActionList)this).Component).OptionsButtonVisible;
		}
		set
		{
			TypeDescriptor.GetProperties(((DesignerActionList)this).Component)["OptionsButtonVisible"].SetValue(((DesignerActionList)this).Component, value);
		}
	}

	public bool CloseButtonVisible
	{
		get
		{
			return ((WarningBox)((DesignerActionList)this).Component).CloseButtonVisible;
		}
		set
		{
			TypeDescriptor.GetProperties(((DesignerActionList)this).Component)["CloseButtonVisible"].SetValue(((DesignerActionList)this).Component, value);
		}
	}

	public Image Image
	{
		get
		{
			return ((WarningBox)((DesignerActionList)this).Component).Image;
		}
		set
		{
			TypeDescriptor.GetProperties(((DesignerActionList)this).Component)["Image"].SetValue(((DesignerActionList)this).Component, value);
		}
	}

	public WarningBoxActionList(WarningBoxDesigner designer)
		: base(((ComponentDesigner)designer).Component)
	{
		_Designer = designer;
	}

	public override DesignerActionItemCollection GetSortedActionItems()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		DesignerActionItemCollection val = new DesignerActionItemCollection();
		val.Add((DesignerActionItem)new DesignerActionHeaderItem("Appearance"));
		val.Add((DesignerActionItem)new DesignerActionHeaderItem("Information"));
		val.Add((DesignerActionItem)new DesignerActionPropertyItem("OptionsButtonVisible", "Show Options button?", "Appearance", "Controls visibility of Options button in WarningBox"));
		val.Add((DesignerActionItem)new DesignerActionPropertyItem("CloseButtonVisible", "Show Close button?", "Appearance", "Controls visibility of Close button in WarningBox"));
		val.Add((DesignerActionItem)new DesignerActionPropertyItem("Image", "Image", "Appearance", "Image that is displayed on Warning Box"));
		val.Add((DesignerActionItem)new DesignerActionTextItem("See Office2007ColorTable.WarningBox members to customize colors.", "Information"));
		val.Add((DesignerActionItem)new DesignerActionMethodItem((DesignerActionList)(object)this, "OpenKB", "How to customize color table?", "Information", "Opens online Knowledge Base article which shows how to customize Office2007ColorTable", true));
		val.Add((DesignerActionItem)new DesignerActionMethodItem((DesignerActionList)(object)this, "OpenTextMarkupRef", "Text Markup Reference...", "Information", "Opens online Text Markup reference. Markup can be used in Text property.", true));
		return val;
	}

	public void OpenKB()
	{
		Process.Start("http://www.devcomponents.com/kb2/");
	}

	public void OpenTextMarkupRef()
	{
		Process.Start("http://www.devcomponents.com/kb2/?p=515");
	}
}
