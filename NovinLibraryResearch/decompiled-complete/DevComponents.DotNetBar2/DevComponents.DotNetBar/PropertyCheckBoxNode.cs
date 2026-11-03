using System;
using System.ComponentModel;
using DevComponents.AdvTree;

namespace DevComponents.DotNetBar;

public class PropertyCheckBoxNode : PropertyNode
{
	public PropertyCheckBoxNode(PropertyDescriptor property)
		: base(property)
	{
	}

	internal override void _2E74()
	{
		base._2E74();
		Cell editCell = EditCell;
		editCell.CheckBoxStyle = eCheckBoxStyle.CheckBox;
		editCell.CheckBoxAlignment = eCellPartAlignment.NearCenter;
		editCell.CheckBoxVisible = true;
	}

	protected override void UpdateDisplayedValue(object propertyValue, bool refreshSubProperties)
	{
		base.UpdateDisplayedValue(propertyValue, refreshSubProperties);
		if (propertyValue is bool)
		{
			EditCell.Checked = (bool)propertyValue;
		}
		else
		{
			EditCell.Checked = false;
		}
	}

	public override void EnterEditorMode(eTreeAction action, bool focusEditor)
	{
	}

	protected override void OnBeforeCellCheck(Cell cell, AdvTreeCellBeforeCheckEventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Invalid comparison between Unknown and I4
		if (base.IsReadOnly && e.Action != eTreeAction.Code)
		{
			e.Cancel = true;
		}
		else if (e.Action != eTreeAction.Code)
		{
			bool flag = (int)e.NewCheckState == 1;
			e.Cancel = !ApplyValue(flag, null);
		}
		base.OnBeforeCellCheck(cell, e);
	}

	protected internal override void InvokeNodeClick(object sender, EventArgs e)
	{
		base.InvokeNodeClick(sender, e);
		if (e is TreeNodeMouseEventArgs)
		{
			Cell editCell = EditCell;
			TreeNodeMouseEventArgs treeNodeMouseEventArgs = (TreeNodeMouseEventArgs)e;
			if (editCell.Bounds.Contains(treeNodeMouseEventArgs.X, treeNodeMouseEventArgs.Y) && !editCell.CheckBoxBounds.Contains(treeNodeMouseEventArgs.X, treeNodeMouseEventArgs.Y))
			{
				editCell.SetChecked(!editCell.Checked, eTreeAction.Mouse);
			}
		}
	}
}
