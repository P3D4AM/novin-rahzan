using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Images;

namespace Stimulsoft.Report.QuickButtons.Design;

public static class StiQuickButtonHelper
{
	private static StiDataBuilder dataBuilder = new StiDataBuilder();

	private static Image DataRelationImage => StiReportImages.Dictionary.DataRelation();

	public static Image TextFormatImage => StiReportDesignImages.Actions.TextFormat();

	public static Image ClearContentImage => StiReportDesignImages.Editor.ClearContent();

	public static void BuildDataSource(ButtonItem contextMenu, StiDataBand dataBand, string componentContent, StiSetFieldDelegate onSetField)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		TreeNodeEx dataNode = new TreeNodeEx();
		dataBuilder.BuildDataSource(dataBand.DataSource, ref dataNode, includeCalcColumn: true);
		dataBuilder.ExpandedNode(dataNode, includeCalcColumn: true);
		int num = 0;
		foreach (TreeNode node in ((TreeNode)dataNode).Nodes)
		{
			TreeNode val = node;
			ButtonItem buttonItem = new ButtonItem
			{
				BeginGroup = (num == 0),
				Text = val.Text,
				Image = StiReportImages.Dictionary.DataColumn()
			};
			buttonItem.Click += onSetField.Invoke;
			buttonItem.Tag = val;
			contextMenu.SubItems.Add(buttonItem);
			if (val.Nodes.Count > 0)
			{
				buttonItem.Image = DataRelationImage;
			}
			else
			{
				StiDataColumn column = val.Tag as StiDataColumn;
				buttonItem.Image = StiDataBuilder.Images.Images[StiDataBuilder.GetImageKeyFromColumn(column)];
			}
			if (val.Nodes.Count > 0)
			{
				FillMenuItems(val, buttonItem, componentContent, onSetField);
			}
			else if (GetColumnPathFromNode(val) == componentContent)
			{
				buttonItem.Checked = true;
			}
			num++;
		}
	}

	public static void BuildBusinessObject(ButtonItem contextMenu, StiDataBand dataBand, string componentContent, StiSetBusinessObjectFieldDelegate OnSetBusinessObjectField)
	{
		StiBusinessObject topParentBusinessObject = dataBand.BusinessObject.GetTopParentBusinessObject();
		string name = topParentBusinessObject.Name;
		int num = 0;
		foreach (StiBusinessObject businessObject in topParentBusinessObject.BusinessObjects)
		{
			if (businessObject.Columns.Count > 0 || businessObject.BusinessObjects.Count > 0)
			{
				ButtonItem buttonItem = new ButtonItem
				{
					BeginGroup = (num == 0),
					Text = businessObject.ToString(),
					Image = StiReportImages.Dictionary.BusinessObject(),
					Tag = businessObject
				};
				contextMenu.SubItems.Add(buttonItem);
				if (topParentBusinessObject.BusinessObjects.Count > 0 || topParentBusinessObject.Columns.Count > 0)
				{
					ButtonItem buttonItem2 = new ButtonItem();
					buttonItem2.Tag = new object[3]
					{
						name + "." + businessObject.Name,
						businessObject,
						OnSetBusinessObjectField
					};
					ButtonItem item = buttonItem2;
					buttonItem.SubItems.Add(item);
					buttonItem.ExpandChange += BusinessItem_ExpandChange;
				}
				num++;
			}
		}
		foreach (StiDataColumn column in topParentBusinessObject.Columns)
		{
			ButtonItem buttonItem3 = new ButtonItem
			{
				BeginGroup = (num == 0),
				Text = column.ToString(),
				Image = StiReportImages.Dictionary.DataColumn(),
				Tag = name + "." + column.Name
			};
			buttonItem3.Click += OnSetBusinessObjectField.Invoke;
			contextMenu.SubItems.Add(buttonItem3);
			num++;
		}
	}

	private static void FillMenuItems(StiBusinessObject businessObject, BaseItem item, string textToSet, StiSetBusinessObjectFieldDelegate onSetBusinessObjectField)
	{
		int num = 0;
		foreach (StiBusinessObject businessObject2 in businessObject.BusinessObjects)
		{
			if (businessObject2.Columns.Count > 0 || businessObject2.BusinessObjects.Count > 0)
			{
				ButtonItem buttonItem = new ButtonItem
				{
					BeginGroup = (num == 0),
					Text = businessObject2.ToString(),
					Image = StiReportImages.Dictionary.BusinessObject(),
					Tag = businessObject2
				};
				item.SubItems.Add(buttonItem);
				if (businessObject2.BusinessObjects.Count > 0 || businessObject2.Columns.Count > 0)
				{
					ButtonItem buttonItem2 = new ButtonItem();
					buttonItem2.Tag = new object[3]
					{
						textToSet + "." + businessObject2.Name,
						businessObject2,
						onSetBusinessObjectField
					};
					ButtonItem item2 = buttonItem2;
					buttonItem.SubItems.Add(item2);
					buttonItem.ExpandChange += BusinessItem_ExpandChange;
				}
				num++;
			}
		}
		foreach (StiDataColumn column in businessObject.Columns)
		{
			ButtonItem buttonItem3 = new ButtonItem
			{
				BeginGroup = (num == 0),
				Text = column.ToString(),
				Image = StiReportImages.Dictionary.DataColumn(),
				Tag = textToSet + "." + column.Name
			};
			buttonItem3.Click += onSetBusinessObjectField.Invoke;
			item.SubItems.Add(buttonItem3);
			num++;
		}
	}

	private static void FillMenuItems(TreeNode node, BaseItem item, string componentContent, StiSetFieldDelegate onSetField)
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		if (node.Nodes.Count == 1 && node.Nodes[0].Text == "Loading")
		{
			ButtonItem buttonItem = new ButtonItem();
			buttonItem.Tag = new object[2] { componentContent, onSetField };
			buttonItem.Text = "Loading";
			ButtonItem item2 = buttonItem;
			item.SubItems.Add(item2);
			item.ExpandChange += Item_ExpandChange;
			return;
		}
		int num = 0;
		foreach (TreeNode node2 in node.Nodes)
		{
			TreeNode val = node2;
			ButtonItem buttonItem2 = new ButtonItem
			{
				Text = val.Text,
				Tag = val
			};
			buttonItem2.Click += onSetField.Invoke;
			item.SubItems.Add(buttonItem2);
			if (val.Nodes.Count > 0)
			{
				buttonItem2.Image = DataRelationImage;
			}
			else if (val.Tag is StiDataColumn column)
			{
				buttonItem2.Image = StiDataBuilder.Images.Images[StiDataBuilder.GetImageKeyFromColumn(column)];
			}
			if (val.Nodes.Count > 0)
			{
				FillMenuItems(val, item.SubItems[num], componentContent, onSetField);
			}
			else if (GetColumnPathFromNode(val) == componentContent)
			{
				SetChecked(item.SubItems[num]);
			}
			num++;
		}
	}

	private static string GetColumnPathFromNode(TreeNode treeNode)
	{
		return "{" + StiDataBuilder.GetColumnPathFromNode(treeNode) + "}";
	}

	private static void SetChecked(BaseItem item)
	{
		((ButtonItem)item).Checked = true;
		while (item.Parent is ButtonItem)
		{
			item = item.Parent;
			((ButtonItem)item).FontBold = true;
		}
	}

	private static void BusinessItem_ExpandChange(object sender, EventArgs e)
	{
		BaseItem baseItem = sender as BaseItem;
		baseItem.ExpandChange -= Item_ExpandChange;
		if (baseItem.SubItems.Count == 1 && baseItem.SubItems[0].Tag != null)
		{
			object[] array = baseItem.SubItems[0].Tag as object[];
			string textToSet = (string)array[0];
			StiBusinessObject businessObject = array[1] as StiBusinessObject;
			StiSetBusinessObjectFieldDelegate onSetBusinessObjectField = array[2] as StiSetBusinessObjectFieldDelegate;
			baseItem.SubItems.Clear();
			FillMenuItems(businessObject, baseItem, textToSet, onSetBusinessObjectField);
		}
	}

	private static void Item_ExpandChange(object sender, EventArgs e)
	{
		BaseItem baseItem = sender as BaseItem;
		baseItem.ExpandChange -= Item_ExpandChange;
		if (baseItem.Tag is TreeNodeEx treeNodeEx && ((TreeNode)treeNodeEx).Tag != null)
		{
			object[] array = baseItem.SubItems[0].Tag as object[];
			string componentContent = (string)array[0];
			StiSetFieldDelegate onSetField = array[1] as StiSetFieldDelegate;
			dataBuilder.ExpandedNode(treeNodeEx, includeCalcColumn: true);
			baseItem.SubItems.Clear();
			FillMenuItems((TreeNode)(object)treeNodeEx, baseItem, componentContent, onSetField);
		}
	}
}
