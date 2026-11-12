using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using DevComponents.AdvTree;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar.Design;

internal class ComboTreeActionList : DesignerActionList
{
	private ComboTreeDesigner _Designer;

	public bool HotTracking
	{
		get
		{
			return ((ComboTree)((DesignerActionList)this).Component).HotTracking;
		}
		set
		{
			TypeDescriptor.GetProperties(((DesignerActionList)this).Component)["HotTracking"].SetValue(((DesignerActionList)this).Component, value);
		}
	}

	public eSelectionStyle SelectionBoxStyle
	{
		get
		{
			return ((ComboTree)((DesignerActionList)this).Component).SelectionBoxStyle;
		}
		set
		{
			TypeDescriptor.GetProperties(((DesignerActionList)this).Component)["SelectionBoxStyle"].SetValue(((DesignerActionList)this).Component, value);
		}
	}

	public bool ColumnsVisible
	{
		get
		{
			return ((ComboTree)((DesignerActionList)this).Component).ColumnsVisible;
		}
		set
		{
			TypeDescriptor.GetProperties(((DesignerActionList)this).Component)["ColumnsVisible"].SetValue(((DesignerActionList)this).Component, value);
		}
	}

	public bool GridColumnLines
	{
		get
		{
			return ((ComboTree)((DesignerActionList)this).Component).GridColumnLines;
		}
		set
		{
			TypeDescriptor.GetProperties(((DesignerActionList)this).Component)["GridColumnLines"].SetValue(((DesignerActionList)this).Component, value);
		}
	}

	public bool GridRowLines
	{
		get
		{
			return ((ComboTree)((DesignerActionList)this).Component).GridRowLines;
		}
		set
		{
			TypeDescriptor.GetProperties(((DesignerActionList)this).Component)["GridRowLines"].SetValue(((DesignerActionList)this).Component, value);
		}
	}

	public Color GridLinesColor
	{
		get
		{
			return ((ComboTree)((DesignerActionList)this).Component).GridLinesColor;
		}
		set
		{
			TypeDescriptor.GetProperties(((DesignerActionList)this).Component)["GridLinesColor"].SetValue(((DesignerActionList)this).Component, value);
		}
	}

	public ComboTreeActionList(ComboTreeDesigner designer)
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
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		DesignerActionItemCollection val = new DesignerActionItemCollection();
		val.Add((DesignerActionItem)new DesignerActionHeaderItem("Nodes"));
		val.Add((DesignerActionItem)new DesignerActionHeaderItem("Columns"));
		val.Add((DesignerActionItem)new DesignerActionMethodItem((DesignerActionList)(object)this, "EditColumns", "Edit Columns...", "Columns", "Edit Tree Control Columns", true));
		val.Add((DesignerActionItem)new DesignerActionPropertyItem("ColumnsVisible", "Column header visible?", "Columns", "Indicates whether tree column header is visible"));
		val.Add((DesignerActionItem)new DesignerActionPropertyItem("GridColumnLines", "Show grid column lines?", "Columns", "Indicates whether grid lines are visible"));
		val.Add((DesignerActionItem)new DesignerActionPropertyItem("GridRowLines", "Show grid row lines?", "Nodes", "Indicates whether grid lines between nodes are visible"));
		val.Add((DesignerActionItem)new DesignerActionPropertyItem("GridLinesColor", "Grid lines color:", "Columns", "Indicates custom color for grid lines"));
		val.Add((DesignerActionItem)new DesignerActionPropertyItem("SelectionBoxStyle", "Selection style:", "Selection", "Indicates selection style"));
		val.Add((DesignerActionItem)new DesignerActionPropertyItem("HotTracking", "Highlight mouse over node?", "Selection", "Indicates whether node that mouse is over is highlighted"));
		return val;
	}

	public void CreateNode()
	{
		_Designer.CreateNode();
	}

	public void EditColumns()
	{
		_Designer.EditColumns();
	}
}
