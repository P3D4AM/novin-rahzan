using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class RadialMenuDesigner : ControlDesigner
{
	private DesignerActionListCollection _ActionLists;

	public override ICollection AssociatedComponents
	{
		get
		{
			ArrayList arrayList = new ArrayList(((ControlDesigner)this).AssociatedComponents);
			if (((ControlDesigner)this).Control is RadialMenu radialMenu)
			{
				foreach (BaseItem item in radialMenu.Items)
				{
					if (item.DesignMode)
					{
						arrayList.Add(item);
					}
				}
			}
			return arrayList;
		}
	}

	public override DesignerActionListCollection ActionLists
	{
		get
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			if (_ActionLists == null)
			{
				_ActionLists = new DesignerActionListCollection();
				_ActionLists.Add((DesignerActionList)(object)new RadialMenuActionList(this));
			}
			return _ActionLists;
		}
	}

	public override SelectionRules SelectionRules => (SelectionRules)1342177280;

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		RadialMenu radialMenu = ((ControlDesigner)this).Control as RadialMenu;
		radialMenu.Symbol = "\uf043";
		radialMenu.SymbolSize = 13f;
		if (defaultValues != null && defaultValues.Contains("Size"))
		{
			defaultValues["Size"] = new Size(28, 28);
		}
		((ControlDesigner)this).InitializeNewComponent(defaultValues);
	}

	public void EditItems()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		RadialMenu radialMenu = ((ComponentDesigner)this).Component as RadialMenu;
		Form val = new Form();
		((Control)val).Text = "Radial Menu Editor";
		val.FormBorderStyle = (FormBorderStyle)4;
		val.MinimizeBox = false;
		val.StartPosition = (FormStartPosition)1;
		RadialMenuItemEditor radialMenuItemEditor = new RadialMenuItemEditor();
		((Control)radialMenuItemEditor).Dock = (DockStyle)5;
		val.ClientSize = new Size(722, 660);
		((Control)val).Controls.Add((Control)(object)radialMenuItemEditor);
		((Control)val).BackColor = Color.White;
		radialMenuItemEditor.RadialMenu = radialMenu;
		radialMenuItemEditor.Designer = this;
		radialMenuItemEditor.UpdateDisplay();
		val.ShowDialog();
		((Component)(object)val).Dispose();
	}

	public object GetDesignService(Type serviceType)
	{
		return ((ComponentDesigner)this).GetService(serviceType);
	}
}
