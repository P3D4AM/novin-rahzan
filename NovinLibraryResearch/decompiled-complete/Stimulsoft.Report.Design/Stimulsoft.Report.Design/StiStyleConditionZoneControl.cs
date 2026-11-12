using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design;

public class StiStyleConditionZoneControl : StiZoneControl
{
	private bool lockCheckPlacement;

	private StiStyleComponentPlacement placement;

	private StiStyleComponentType componentType = StiStyleComponentType.Text;

	private StiStyleLocation location;

	private IContainer components;

	public ComboBox comboBoxOperationPlacement;

	public TextBox textBoxName;

	public ComboBox comboBoxOperationComponentType;

	public ComboBox comboBoxOperationLocation;

	public ComboBox comboBoxOperationName;

	public CheckBox checkBoxPlacement;

	public CheckBox checkBoxComponentType;

	public CheckBox checkBoxLocation;

	public CheckBox checkBoxName;

	private StiHitTestVisibleLabel labelPlacement;

	private StiHitTestVisibleLabel labelComponentType;

	private StiHitTestVisibleLabel labelLocation;

	public StiTreeViewBox treeBoxPlacement;

	private ImageList imageListPlacement;

	private ToolTip toolTip1;

	public StiTreeViewBox treeBoxComponentType;

	private ImageList imageListComponentType;

	private ImageList imageListLocation;

	public StiTreeViewBox treeBoxLocation;

	public Stimulsoft.Controls.StiNumericUpDown textBoxNestedLevel;

	public ComboBox comboBoxOperationPlacementNestedLevel;

	public CheckBox checkBoxPlacementNestedLevel;

	public StiStyleComponentPlacement Placement
	{
		get
		{
			return placement;
		}
		set
		{
			placement = value;
			string text = string.Empty;
			if ((placement & StiStyleComponentPlacement.ReportTitle) > StiStyleComponentPlacement.None)
			{
				text = text + ", " + StiLocalization.Get("Components", "StiReportTitleBand");
			}
			if ((placement & StiStyleComponentPlacement.ReportSummary) > StiStyleComponentPlacement.None)
			{
				text = text + ", " + StiLocalization.Get("Components", "StiReportSummaryBand");
			}
			if ((placement & StiStyleComponentPlacement.PageHeader) > StiStyleComponentPlacement.None)
			{
				text = text + ", " + StiLocalization.Get("Components", "StiPageHeaderBand");
			}
			if ((placement & StiStyleComponentPlacement.PageFooter) > StiStyleComponentPlacement.None)
			{
				text = text + ", " + StiLocalization.Get("Components", "StiPageFooterBand");
			}
			if ((placement & StiStyleComponentPlacement.GroupHeader) > StiStyleComponentPlacement.None)
			{
				text = text + ", " + StiLocalization.Get("Components", "StiGroupHeaderBand");
			}
			if ((placement & StiStyleComponentPlacement.GroupFooter) > StiStyleComponentPlacement.None)
			{
				text = text + ", " + StiLocalization.Get("Components", "StiGroupFooterBand");
			}
			if ((placement & StiStyleComponentPlacement.Header) > StiStyleComponentPlacement.None)
			{
				text = text + ", " + StiLocalization.Get("Components", "StiHeaderBand");
			}
			if ((placement & StiStyleComponentPlacement.Footer) > StiStyleComponentPlacement.None)
			{
				text = text + ", " + StiLocalization.Get("Components", "StiFooterBand");
			}
			if ((placement & StiStyleComponentPlacement.ColumnHeader) > StiStyleComponentPlacement.None)
			{
				text = text + ", " + StiLocalization.Get("Components", "StiColumnHeaderBand");
			}
			if ((placement & StiStyleComponentPlacement.ColumnFooter) > StiStyleComponentPlacement.None)
			{
				text = text + ", " + StiLocalization.Get("Components", "StiColumnFooterBand");
			}
			if ((placement & StiStyleComponentPlacement.Data) > StiStyleComponentPlacement.None)
			{
				text = text + ", " + StiLocalization.Get("Components", "StiDataBand");
			}
			if ((placement & StiStyleComponentPlacement.DataOddStyle) > StiStyleComponentPlacement.None)
			{
				text = text + ", " + StiLocalization.Get("PropertyMain", "OddStyle");
			}
			if ((placement & StiStyleComponentPlacement.DataEvenStyle) > StiStyleComponentPlacement.None)
			{
				text = text + ", " + StiLocalization.Get("PropertyMain", "EvenStyle");
			}
			if ((placement & StiStyleComponentPlacement.Table) > StiStyleComponentPlacement.None)
			{
				text = text + ", " + StiLocalization.Get("Components", "StiTable");
			}
			if ((placement & StiStyleComponentPlacement.Hierarchical) > StiStyleComponentPlacement.None)
			{
				text = text + ", " + StiLocalization.Get("Components", "StiHierarchicalBand");
			}
			if ((placement & StiStyleComponentPlacement.Child) > StiStyleComponentPlacement.None)
			{
				text = text + ", " + StiLocalization.Get("Components", "StiChildBand");
			}
			if ((placement & StiStyleComponentPlacement.Empty) > StiStyleComponentPlacement.None)
			{
				text = text + ", " + StiLocalization.Get("Components", "StiEmptyBand");
			}
			if ((placement & StiStyleComponentPlacement.Overlay) > StiStyleComponentPlacement.None)
			{
				text = text + ", " + StiLocalization.Get("Components", "StiOverlayBand");
			}
			if ((placement & StiStyleComponentPlacement.Panel) > StiStyleComponentPlacement.None)
			{
				text = text + ", " + StiLocalization.Get("Components", "StiPanel");
			}
			if ((placement & StiStyleComponentPlacement.Page) > StiStyleComponentPlacement.None)
			{
				text = text + ", " + StiLocalization.Get("Components", "StiPage");
			}
			if (text.StartsWith(", "))
			{
				text = text.Substring(2, text.Length - 2);
			}
			((Control)treeBoxPlacement).Text = text;
			toolTip1.SetToolTip((Control)(object)treeBoxPlacement, text.Replace(", ", "\n"));
		}
	}

	public int PlacementNestedLevel
	{
		get
		{
			return (int)textBoxNestedLevel.Value;
		}
		set
		{
			textBoxNestedLevel.Value = value;
		}
	}

	public StiStyleComponentType ComponentType
	{
		get
		{
			return componentType;
		}
		set
		{
			componentType = value;
			string text = string.Empty;
			if ((componentType & StiStyleComponentType.Text) > (StiStyleComponentType)0)
			{
				text = text + ", " + StiLocalization.Get("PropertyEnum", "StiStyleComponentTypeText");
			}
			if ((componentType & StiStyleComponentType.Primitive) > (StiStyleComponentType)0)
			{
				text = text + ", " + StiLocalization.Get("PropertyEnum", "StiStyleComponentTypePrimitive");
			}
			if ((componentType & StiStyleComponentType.Image) > (StiStyleComponentType)0)
			{
				text = text + ", " + StiLocalization.Get("PropertyEnum", "StiStyleComponentTypeImage");
			}
			if ((componentType & StiStyleComponentType.CrossTab) > (StiStyleComponentType)0)
			{
				text = text + ", " + StiLocalization.Get("PropertyEnum", "StiStyleComponentTypeCrossTab");
			}
			if ((componentType & StiStyleComponentType.Chart) > (StiStyleComponentType)0)
			{
				text = text + ", " + StiLocalization.Get("PropertyEnum", "StiStyleComponentTypeChart");
			}
			if ((componentType & StiStyleComponentType.CheckBox) > (StiStyleComponentType)0)
			{
				text = text + ", " + StiLocalization.Get("PropertyEnum", "StiStyleComponentTypeCheckBox");
			}
			if (text.StartsWith(", "))
			{
				text = text.Substring(2, text.Length - 2);
			}
			((Control)treeBoxComponentType).Text = text;
			toolTip1.SetToolTip((Control)(object)treeBoxComponentType, text.Replace(", ", "\n"));
		}
	}

	public StiStyleLocation Location
	{
		get
		{
			return location;
		}
		set
		{
			location = value;
			string text = string.Empty;
			if ((location & StiStyleLocation.TopLeft) > StiStyleLocation.None)
			{
				text = text + ", " + StiLocalization.Get("PropertyEnum", "ContentAlignmentTopLeft");
			}
			if ((location & StiStyleLocation.TopCenter) > StiStyleLocation.None)
			{
				text = text + ", " + StiLocalization.Get("PropertyEnum", "ContentAlignmentTopCenter");
			}
			if ((location & StiStyleLocation.TopRight) > StiStyleLocation.None)
			{
				text = text + ", " + StiLocalization.Get("PropertyEnum", "ContentAlignmentTopRight");
			}
			if ((location & StiStyleLocation.MiddleLeft) > StiStyleLocation.None)
			{
				text = text + ", " + StiLocalization.Get("PropertyEnum", "ContentAlignmentMiddleLeft");
			}
			if ((location & StiStyleLocation.MiddleCenter) > StiStyleLocation.None)
			{
				text = text + ", " + StiLocalization.Get("PropertyEnum", "ContentAlignmentMiddleCenter");
			}
			if ((location & StiStyleLocation.MiddleRight) > StiStyleLocation.None)
			{
				text = text + ", " + StiLocalization.Get("PropertyEnum", "ContentAlignmentMiddleRight");
			}
			if ((location & StiStyleLocation.BottomLeft) > StiStyleLocation.None)
			{
				text = text + ", " + StiLocalization.Get("PropertyEnum", "ContentAlignmentBottomLeft");
			}
			if ((location & StiStyleLocation.BottomCenter) > StiStyleLocation.None)
			{
				text = text + ", " + StiLocalization.Get("PropertyEnum", "ContentAlignmentBottomCenter");
			}
			if ((location & StiStyleLocation.BottomRight) > StiStyleLocation.None)
			{
				text = text + ", " + StiLocalization.Get("PropertyEnum", "ContentAlignmentBottomRight");
			}
			if ((location & StiStyleLocation.Left) > StiStyleLocation.None)
			{
				text = text + ", " + StiLocalization.Get("PropertyEnum", "HorizontalAlignmentLeft");
			}
			if ((location & StiStyleLocation.CenterHorizontal) > StiStyleLocation.None)
			{
				text = text + ", " + StiLocalization.Get("Toolbars", "CenterHorizontally");
			}
			if ((location & StiStyleLocation.Right) > StiStyleLocation.None)
			{
				text = text + ", " + StiLocalization.Get("PropertyEnum", "HorizontalAlignmentRight");
			}
			if ((location & StiStyleLocation.Top) > StiStyleLocation.None)
			{
				text = text + ", " + StiLocalization.Get("PropertyEnum", "StiVertAlignmentTop");
			}
			if ((location & StiStyleLocation.CenterVertical) > StiStyleLocation.None)
			{
				text = text + ", " + StiLocalization.Get("Toolbars", "CenterVertically");
			}
			if ((location & StiStyleLocation.Bottom) > StiStyleLocation.None)
			{
				text = text + ", " + StiLocalization.Get("PropertyEnum", "StiVertAlignmentBottom");
			}
			if (text.StartsWith(", "))
			{
				text = text.Substring(2, text.Length - 2);
			}
			((Control)treeBoxLocation).Text = text;
			toolTip1.SetToolTip((Control)(object)treeBoxLocation, text.Replace(", ", "\n"));
		}
	}

	public StiStyleConditionZoneControl(StiStyleComponentPlacement placement, int placementNestedLevel, StiStyleComponentType componentType, StiStyleLocation location)
	{
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			Placement = placement;
			PlacementNestedLevel = placementNestedLevel;
			ComponentType = componentType;
			Location = location;
			DoScaling();
			Localize();
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormConditions", "title");
		((Control)labelPlacement).Text = StiLocalization.Get("FormBand", "And");
		((Control)labelComponentType).Text = StiLocalization.Get("FormBand", "And");
		((Control)labelLocation).Text = StiLocalization.Get("FormBand", "And");
		((Control)checkBoxPlacement).Text = StiLocalization.Get("PropertyEnum", "StiStyleConditionTypePlacement");
		((Control)checkBoxComponentType).Text = StiLocalization.Get("PropertyEnum", "StiStyleConditionTypeComponentType");
		((Control)checkBoxLocation).Text = StiLocalization.Get("PropertyEnum", "StiStyleConditionTypeLocation");
		((Control)checkBoxName).Text = StiLocalization.Get("PropertyEnum", "StiStyleConditionTypeComponentName");
		((Control)checkBoxPlacementNestedLevel).Text = StiLocalization.Get("PropertyMain", "NestedLevel");
	}

	private void CheckBoxPlacement_CheckedChanged(object sender, EventArgs e)
	{
		StiTreeViewBox stiTreeViewBox = treeBoxPlacement;
		ComboBox obj = comboBoxOperationPlacement;
		StiHitTestVisibleLabel stiHitTestVisibleLabel = labelPlacement;
		CheckBox obj2 = checkBoxPlacementNestedLevel;
		ComboBox obj3 = comboBoxOperationPlacementNestedLevel;
		bool flag = (((Control)textBoxNestedLevel).Enabled = checkBoxPlacement.Checked);
		bool flag3 = (((Control)obj3).Enabled = flag);
		bool flag5 = (((Control)obj2).Enabled = flag3);
		bool flag7 = (((Control)stiHitTestVisibleLabel).Enabled = flag5);
		bool enabled = (((Control)obj).Enabled = flag7);
		((Control)stiTreeViewBox).Enabled = enabled;
		Stimulsoft.Controls.StiNumericUpDown stiNumericUpDown = textBoxNestedLevel;
		enabled = (((Control)comboBoxOperationPlacementNestedLevel).Enabled = checkBoxPlacementNestedLevel.Checked && checkBoxPlacement.Checked);
		((Control)stiNumericUpDown).Enabled = enabled;
		StiTreeViewBox stiTreeViewBox2 = treeBoxComponentType;
		ComboBox obj4 = comboBoxOperationComponentType;
		flag7 = (((Control)labelComponentType).Enabled = checkBoxComponentType.Checked);
		enabled = (((Control)obj4).Enabled = flag7);
		((Control)stiTreeViewBox2).Enabled = enabled;
		StiTreeViewBox stiTreeViewBox3 = treeBoxLocation;
		ComboBox obj5 = comboBoxOperationLocation;
		flag7 = (((Control)labelLocation).Enabled = checkBoxLocation.Checked);
		enabled = (((Control)obj5).Enabled = flag7);
		((Control)stiTreeViewBox3).Enabled = enabled;
		TextBox obj6 = textBoxName;
		enabled = (((Control)comboBoxOperationName).Enabled = checkBoxName.Checked);
		((Control)obj6).Enabled = enabled;
	}

	private void TreeBoxPlacement_TreeNodesFill(object sender, StiTreeNodesFillEventArgs e)
	{
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Expected O, but got Unknown
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Expected O, but got Unknown
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Expected O, but got Unknown
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Expected O, but got Unknown
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Expected O, but got Unknown
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Expected O, but got Unknown
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Expected O, but got Unknown
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0509: Expected O, but got Unknown
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_052c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Unknown result type (might be due to invalid IL or missing references)
		//IL_054c: Expected O, but got Unknown
		//IL_056a: Unknown result type (might be due to invalid IL or missing references)
		//IL_056f: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0595: Expected O, but got Unknown
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05de: Expected O, but got Unknown
		//IL_05fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0603: Unknown result type (might be due to invalid IL or missing references)
		//IL_0613: Unknown result type (might be due to invalid IL or missing references)
		//IL_0629: Expected O, but got Unknown
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_064e: Unknown result type (might be due to invalid IL or missing references)
		//IL_065e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0674: Expected O, but got Unknown
		//IL_0694: Unknown result type (might be due to invalid IL or missing references)
		//IL_0699: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bf: Expected O, but got Unknown
		//IL_06df: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_070b: Expected O, but got Unknown
		//IL_072c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0731: Unknown result type (might be due to invalid IL or missing references)
		//IL_0741: Unknown result type (might be due to invalid IL or missing references)
		//IL_0757: Expected O, but got Unknown
		//IL_0777: Unknown result type (might be due to invalid IL or missing references)
		//IL_077c: Unknown result type (might be due to invalid IL or missing references)
		//IL_078c: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a2: Expected O, but got Unknown
		//IL_07c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ed: Expected O, but got Unknown
		//IL_080d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0812: Unknown result type (might be due to invalid IL or missing references)
		//IL_0822: Unknown result type (might be due to invalid IL or missing references)
		//IL_0838: Expected O, but got Unknown
		//IL_0858: Unknown result type (might be due to invalid IL or missing references)
		//IL_085d: Unknown result type (might be due to invalid IL or missing references)
		//IL_086d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0883: Expected O, but got Unknown
		//IL_08a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ce: Expected O, but got Unknown
		//IL_08ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0903: Unknown result type (might be due to invalid IL or missing references)
		//IL_0919: Expected O, but got Unknown
		if (imageListPlacement.Images.Count == 0)
		{
			imageListPlacement.ImageSize = new Size(StiScale.I16, StiScale.I16);
			imageListPlacement.Images.Add(StiReportDesignImages.Components.ReportTitleBand());
			imageListPlacement.Images.Add(StiReportDesignImages.Components.ReportSummaryBand());
			imageListPlacement.Images.Add(StiReportDesignImages.Components.PageHeaderBand());
			imageListPlacement.Images.Add(StiReportDesignImages.Components.PageFooterBand());
			imageListPlacement.Images.Add(StiReportDesignImages.Components.GroupHeaderBand());
			imageListPlacement.Images.Add(StiReportDesignImages.Components.GroupFooterBand());
			imageListPlacement.Images.Add(StiReportDesignImages.Components.HeaderBand());
			imageListPlacement.Images.Add(StiReportDesignImages.Components.FooterBand());
			imageListPlacement.Images.Add(StiReportDesignImages.Components.ColumnHeaderBand());
			imageListPlacement.Images.Add(StiReportDesignImages.Components.ColumnFooterBand());
			imageListPlacement.Images.Add(StiReportDesignImages.Components.DataBand());
			imageListPlacement.Images.Add(StiReportDesignImages.Components.Table());
			imageListPlacement.Images.Add(StiReportDesignImages.Components.HierarchicalBand());
			imageListPlacement.Images.Add(StiReportDesignImages.Components.ChildBand());
			imageListPlacement.Images.Add(StiReportDesignImages.Components.EmptyBand());
			imageListPlacement.Images.Add(StiReportDesignImages.Components.OverlayBand());
			imageListPlacement.Images.Add(StiReportDesignImages.Components.Panel());
			imageListPlacement.Images.Add(StiReportDesignImages.Components.Page());
			imageListPlacement.Images.Add(StiReportDesignImages.Components.Component());
		}
		((TreeView)e.TreeView).ShowRootLines = false;
		((TreeView)e.TreeView).ImageList = imageListPlacement;
		((TreeView)e.TreeView).CheckBoxes = true;
		((Control)e.TreeView).Dock = (DockStyle)1;
		((TreeView)e.TreeView).AfterCheck += new TreeViewEventHandler(PlacementTreeView_AfterCheck);
		((Control)e.TreeView).Height = ((Control)e.TreeView).Parent.Height - StiScale.I(32);
		Button val = new Button
		{
			UseVisualStyleBackColor = true,
			Text = StiLocalization.Get("Buttons", "SetAll"),
			Size = StiScale.I(new Size(100, 24)),
			Tag = e.TreeView
		};
		((Control)val).Location = new Point(((Control)e.TreeView).Width / 2 - ((Control)val).Width - StiScale.I2, ((Control)e.TreeView).Height + StiScale.I4);
		((Control)e.TreeView).Parent.Controls.Add((Control)(object)val);
		((Control)val).Click += delegate(object sender2, EventArgs e2)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			lockCheckPlacement = true;
			Button val9 = (Button)((sender2 is Button) ? sender2 : null);
			object tag2 = ((Control)val9).Tag;
			foreach (TreeNode node in ((TreeView)((tag2 is TreeView) ? tag2 : null)).Nodes)
			{
				TreeNode val10 = node;
				val10.Checked = true;
				foreach (TreeNode node2 in val10.Nodes)
				{
					TreeNode val11 = node2;
					val11.Checked = false;
				}
			}
			Placement = StiStyleComponentPlacement.AllExeptStyles;
			lockCheckPlacement = false;
		};
		Button val2 = new Button
		{
			UseVisualStyleBackColor = true,
			Text = StiLocalization.Get("Buttons", "RemoveAll"),
			Size = StiScale.I(new Size(100, 24)),
			Location = new Point(((Control)e.TreeView).Width / 2 + StiScale.I2, ((Control)e.TreeView).Height + StiScale.I4),
			Tag = e.TreeView
		};
		((Control)e.TreeView).Parent.Controls.Add((Control)(object)val2);
		((Control)val2).Click += delegate(object sender3, EventArgs e3)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			lockCheckPlacement = true;
			Button val6 = (Button)((sender3 is Button) ? sender3 : null);
			object tag = ((Control)val6).Tag;
			foreach (TreeNode node3 in ((TreeView)((tag is TreeView) ? tag : null)).Nodes)
			{
				TreeNode val7 = node3;
				val7.Checked = false;
				foreach (TreeNode node4 in val7.Nodes)
				{
					TreeNode val8 = node4;
					val8.Checked = false;
				}
			}
			Placement = StiStyleComponentPlacement.None;
			lockCheckPlacement = false;
		};
		TreeNode val3 = new TreeNode(StiLocalization.Get("Components", "StiReportTitleBand"), 0, 0)
		{
			Tag = StiStyleComponentPlacement.ReportTitle,
			Checked = ((Placement & StiStyleComponentPlacement.ReportTitle) > StiStyleComponentPlacement.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("Components", "StiReportSummaryBand"), 1, 1)
		{
			Tag = StiStyleComponentPlacement.ReportSummary,
			Checked = ((Placement & StiStyleComponentPlacement.ReportSummary) > StiStyleComponentPlacement.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("Components", "StiPageHeaderBand"), 2, 2)
		{
			Tag = StiStyleComponentPlacement.PageHeader,
			Checked = ((Placement & StiStyleComponentPlacement.PageHeader) > StiStyleComponentPlacement.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("Components", "StiPageFooterBand"), 3, 3)
		{
			Tag = StiStyleComponentPlacement.PageFooter,
			Checked = ((Placement & StiStyleComponentPlacement.PageFooter) > StiStyleComponentPlacement.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("Components", "StiGroupHeaderBand"), 4, 4)
		{
			Tag = StiStyleComponentPlacement.GroupHeader,
			Checked = ((Placement & StiStyleComponentPlacement.GroupHeader) > StiStyleComponentPlacement.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("Components", "StiGroupFooterBand"), 5, 5)
		{
			Tag = StiStyleComponentPlacement.GroupFooter,
			Checked = ((Placement & StiStyleComponentPlacement.GroupFooter) > StiStyleComponentPlacement.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("Components", "StiHeaderBand"), 6, 6)
		{
			Tag = StiStyleComponentPlacement.Header,
			Checked = ((Placement & StiStyleComponentPlacement.Header) > StiStyleComponentPlacement.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("Components", "StiFooterBand"), 7, 7)
		{
			Tag = StiStyleComponentPlacement.Footer,
			Checked = ((Placement & StiStyleComponentPlacement.Footer) > StiStyleComponentPlacement.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("Components", "StiColumnHeaderBand"), 8, 8)
		{
			Tag = StiStyleComponentPlacement.ColumnHeader,
			Checked = ((Placement & StiStyleComponentPlacement.ColumnHeader) > StiStyleComponentPlacement.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("Components", "StiColumnFooterBand"), 9, 9)
		{
			Tag = StiStyleComponentPlacement.ColumnFooter,
			Checked = ((Placement & StiStyleComponentPlacement.ColumnFooter) > StiStyleComponentPlacement.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("Components", "StiDataBand"), 10, 10)
		{
			Tag = StiStyleComponentPlacement.Data,
			Checked = ((Placement & StiStyleComponentPlacement.Data) > StiStyleComponentPlacement.None)
		};
		e.Nodes.Add(val3);
		TreeNode val4 = new TreeNode(StiLocalization.Get("PropertyMain", "OddStyle"), 18, 18)
		{
			Tag = StiStyleComponentPlacement.DataOddStyle,
			Checked = ((Placement & StiStyleComponentPlacement.DataOddStyle) > StiStyleComponentPlacement.None)
		};
		val3.Nodes.Add(val4);
		TreeNode val5 = new TreeNode(StiLocalization.Get("PropertyMain", "EvenStyle"), 18, 18)
		{
			Tag = StiStyleComponentPlacement.DataEvenStyle,
			Checked = ((Placement & StiStyleComponentPlacement.DataEvenStyle) > StiStyleComponentPlacement.None)
		};
		val3.Nodes.Add(val5);
		val3 = new TreeNode(StiLocalization.Get("Components", "StiTable"), 11, 11)
		{
			Tag = StiStyleComponentPlacement.Table,
			Checked = ((Placement & StiStyleComponentPlacement.Table) > StiStyleComponentPlacement.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("Components", "StiHierarchicalBand"), 12, 12)
		{
			Tag = StiStyleComponentPlacement.Hierarchical,
			Checked = ((Placement & StiStyleComponentPlacement.Hierarchical) > StiStyleComponentPlacement.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("Components", "StiChildBand"), 13, 13)
		{
			Tag = StiStyleComponentPlacement.Child,
			Checked = ((Placement & StiStyleComponentPlacement.Child) > StiStyleComponentPlacement.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("Components", "StiEmptyBand"), 14, 14)
		{
			Tag = StiStyleComponentPlacement.Empty,
			Checked = ((Placement & StiStyleComponentPlacement.Empty) > StiStyleComponentPlacement.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("Components", "StiOverlayBand"), 15, 15)
		{
			Tag = StiStyleComponentPlacement.Overlay,
			Checked = ((Placement & StiStyleComponentPlacement.Overlay) > StiStyleComponentPlacement.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("Components", "StiPanel"), 16, 16)
		{
			Tag = StiStyleComponentPlacement.Panel,
			Checked = ((Placement & StiStyleComponentPlacement.Panel) > StiStyleComponentPlacement.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("Components", "StiPage"), 17, 17)
		{
			Tag = StiStyleComponentPlacement.Page,
			Checked = ((Placement & StiStyleComponentPlacement.Page) > StiStyleComponentPlacement.None)
		};
		e.Nodes.Add(val3);
		((TreeView)e.TreeView).ExpandAll();
	}

	private void PlacementTreeView_AfterCheck(object sender, TreeViewEventArgs e)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Expected O, but got Unknown
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected O, but got Unknown
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		if (lockCheckPlacement)
		{
			return;
		}
		if ((StiStyleComponentPlacement)e.Node.Tag == StiStyleComponentPlacement.DataEvenStyle)
		{
			lockCheckPlacement = true;
			foreach (TreeNode node in e.Node.TreeView.Nodes)
			{
				TreeNode val = node;
				val.Checked = false;
			}
			lockCheckPlacement = false;
			Placement = StiStyleComponentPlacement.DataEvenStyle;
			return;
		}
		if ((StiStyleComponentPlacement)e.Node.Tag == StiStyleComponentPlacement.DataOddStyle)
		{
			lockCheckPlacement = true;
			foreach (TreeNode node2 in e.Node.TreeView.Nodes)
			{
				TreeNode val2 = node2;
				val2.Checked = false;
			}
			lockCheckPlacement = false;
			Placement = StiStyleComponentPlacement.DataOddStyle;
			return;
		}
		Placement = StiStyleComponentPlacement.None;
		TreeView val3 = (TreeView)((sender is TreeView) ? sender : null);
		foreach (TreeNode node3 in val3.Nodes)
		{
			TreeNode val4 = node3;
			if (val4.Checked)
			{
				Placement |= (StiStyleComponentPlacement)val4.Tag;
			}
			foreach (TreeNode node4 in val4.Nodes)
			{
				TreeNode val5 = node4;
				if (val5.Checked)
				{
					lockCheckPlacement = true;
					val5.Checked = false;
					lockCheckPlacement = false;
				}
			}
		}
	}

	private void TreeBoxComponentType_TreeNodesFill(object sender, StiTreeNodesFillEventArgs e)
	{
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Expected O, but got Unknown
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Expected O, but got Unknown
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Expected O, but got Unknown
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Expected O, but got Unknown
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Expected O, but got Unknown
		if (imageListComponentType.Images.Count == 0)
		{
			imageListComponentType.ImageSize = new Size(StiScale.I16, StiScale.I16);
			imageListComponentType.Images.Add(StiReportDesignImages.Components.Text());
			imageListComponentType.Images.Add(StiReportDesignImages.Components.VerticalLinePrimitive());
			imageListComponentType.Images.Add(StiReportDesignImages.Components.Image());
			imageListComponentType.Images.Add(StiReportDesignImages.Components.CrossTab());
			imageListComponentType.Images.Add(StiReportDesignImages.Components.Chart());
			imageListComponentType.Images.Add(StiReportDesignImages.Components.CheckBox());
		}
		((TreeView)e.TreeView).ShowRootLines = false;
		((TreeView)e.TreeView).ImageList = imageListComponentType;
		((TreeView)e.TreeView).CheckBoxes = true;
		((TreeView)e.TreeView).AfterCheck += new TreeViewEventHandler(ComponentTypeTreeView_AfterCheck);
		((Control)e.TreeView).Dock = (DockStyle)1;
		((Control)e.TreeView).Height = ((Control)e.TreeView).Parent.Height - StiScale.I(32);
		Button val = new Button
		{
			UseVisualStyleBackColor = true,
			Text = StiLocalization.Get("Buttons", "SetAll"),
			Size = StiScale.I(new Size(100, 24)),
			Tag = e.TreeView
		};
		((Control)val).Location = new Point(((Control)e.TreeView).Width / 2 - ((Control)val).Width - StiScale.I2, ((Control)e.TreeView).Height + StiScale.I4);
		((Control)e.TreeView).Parent.Controls.Add((Control)(object)val);
		((Control)val).Click += delegate(object sender2, EventArgs e2)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected O, but got Unknown
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			Button val7 = (Button)((sender2 is Button) ? sender2 : null);
			object tag2 = ((Control)val7).Tag;
			foreach (TreeNode node in ((TreeView)((tag2 is TreeView) ? tag2 : null)).Nodes)
			{
				TreeNode val8 = node;
				val8.Checked = true;
				foreach (TreeNode node2 in val8.Nodes)
				{
					TreeNode val9 = node2;
					val9.Checked = true;
				}
			}
		};
		Button val2 = new Button
		{
			UseVisualStyleBackColor = true,
			Text = StiLocalization.Get("Buttons", "RemoveAll"),
			Size = StiScale.I(new Size(100, 24)),
			Location = new Point(((Control)e.TreeView).Width / 2 + StiScale.I2, ((Control)e.TreeView).Height + StiScale.I4),
			Tag = e.TreeView
		};
		((Control)e.TreeView).Parent.Controls.Add((Control)(object)val2);
		((Control)val2).Click += delegate(object sender3, EventArgs e3)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected O, but got Unknown
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			Button val4 = (Button)((sender3 is Button) ? sender3 : null);
			object tag = ((Control)val4).Tag;
			foreach (TreeNode node3 in ((TreeView)((tag is TreeView) ? tag : null)).Nodes)
			{
				TreeNode val5 = node3;
				val5.Checked = false;
				foreach (TreeNode node4 in val5.Nodes)
				{
					TreeNode val6 = node4;
					val6.Checked = false;
				}
			}
		};
		TreeNode val3 = new TreeNode(StiLocalization.Get("PropertyEnum", "StiStyleComponentTypeText"), 0, 0)
		{
			Tag = StiStyleComponentType.Text,
			Checked = ((ComponentType & StiStyleComponentType.Text) > (StiStyleComponentType)0)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("PropertyEnum", "StiStyleComponentTypePrimitive"), 1, 1)
		{
			Tag = StiStyleComponentType.Primitive,
			Checked = ((ComponentType & StiStyleComponentType.Primitive) > (StiStyleComponentType)0)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("PropertyEnum", "StiStyleComponentTypeImage"), 2, 2)
		{
			Tag = StiStyleComponentType.Image,
			Checked = ((ComponentType & StiStyleComponentType.Image) > (StiStyleComponentType)0)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("PropertyEnum", "StiStyleComponentTypeCrossTab"), 3, 3)
		{
			Tag = StiStyleComponentType.CrossTab,
			Checked = ((ComponentType & StiStyleComponentType.CrossTab) > (StiStyleComponentType)0)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("PropertyEnum", "StiStyleComponentTypeChart"), 4, 4)
		{
			Tag = StiStyleComponentType.Chart,
			Checked = ((ComponentType & StiStyleComponentType.Chart) > (StiStyleComponentType)0)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("PropertyEnum", "StiStyleComponentTypeCheckBox"), 5, 5)
		{
			Tag = StiStyleComponentType.CheckBox,
			Checked = ((ComponentType & StiStyleComponentType.CheckBox) > (StiStyleComponentType)0)
		};
		e.Nodes.Add(val3);
	}

	private void ComponentTypeTreeView_AfterCheck(object sender, TreeViewEventArgs e)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		ComponentType = (StiStyleComponentType)0;
		TreeView val = (TreeView)((sender is TreeView) ? sender : null);
		foreach (TreeNode node in val.Nodes)
		{
			TreeNode val2 = node;
			if (val2.Checked)
			{
				ComponentType |= (StiStyleComponentType)val2.Tag;
			}
		}
	}

	private void TreeBoxLocation_TreeNodesFill(object sender, StiTreeNodesFillEventArgs e)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Expected O, but got Unknown
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Expected O, but got Unknown
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Expected O, but got Unknown
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Expected O, but got Unknown
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Expected O, but got Unknown
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Expected O, but got Unknown
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Expected O, but got Unknown
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Expected O, but got Unknown
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Expected O, but got Unknown
		//IL_048b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b6: Expected O, but got Unknown
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04db: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Expected O, but got Unknown
		//IL_0521: Unknown result type (might be due to invalid IL or missing references)
		//IL_0526: Unknown result type (might be due to invalid IL or missing references)
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		//IL_054c: Expected O, but got Unknown
		//IL_056c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_0581: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Expected O, but got Unknown
		//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e2: Expected O, but got Unknown
		((TreeView)e.TreeView).ShowRootLines = false;
		((TreeView)e.TreeView).ImageList = imageListLocation;
		((TreeView)e.TreeView).CheckBoxes = true;
		((TreeView)e.TreeView).AfterCheck += new TreeViewEventHandler(LocationTreeView_AfterCheck);
		((Control)e.TreeView).Dock = (DockStyle)1;
		((Control)e.TreeView).Height = ((Control)e.TreeView).Parent.Height - StiScale.I(32);
		Button val = new Button
		{
			UseVisualStyleBackColor = true,
			Text = StiLocalization.Get("Buttons", "SetAll"),
			Size = StiScale.I(new Size(100, 24))
		};
		((Control)val).Location = new Point(((Control)e.TreeView).Width / 2 - ((Control)val).Width - StiScale.I2, ((Control)e.TreeView).Height + StiScale.I4);
		((Control)val).Tag = e.TreeView;
		((Control)e.TreeView).Parent.Controls.Add((Control)(object)val);
		((Control)val).Click += delegate(object sender2, EventArgs e2)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected O, but got Unknown
			Button val6 = (Button)((sender2 is Button) ? sender2 : null);
			object tag2 = ((Control)val6).Tag;
			foreach (TreeNode node in ((TreeView)((tag2 is TreeView) ? tag2 : null)).Nodes)
			{
				TreeNode val7 = node;
				val7.Checked = true;
			}
		};
		Button val2 = new Button
		{
			UseVisualStyleBackColor = true,
			Text = StiLocalization.Get("Buttons", "RemoveAll"),
			Size = StiScale.I(new Size(100, 24)),
			Location = new Point(((Control)e.TreeView).Width / 2 + StiScale.I2, ((Control)e.TreeView).Height + StiScale.I4),
			Tag = e.TreeView
		};
		((Control)e.TreeView).Parent.Controls.Add((Control)(object)val2);
		((Control)val2).Click += delegate(object sender3, EventArgs e3)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected O, but got Unknown
			Button val4 = (Button)((sender3 is Button) ? sender3 : null);
			object tag = ((Control)val4).Tag;
			foreach (TreeNode node2 in ((TreeView)((tag is TreeView) ? tag : null)).Nodes)
			{
				TreeNode val5 = node2;
				val5.Checked = false;
			}
		};
		TreeNode val3 = new TreeNode(StiLocalization.Get("PropertyEnum", "ContentAlignmentTopLeft"), 0, 0)
		{
			Tag = StiStyleLocation.TopLeft,
			Checked = ((Location & StiStyleLocation.TopLeft) > StiStyleLocation.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("PropertyEnum", "ContentAlignmentTopCenter"), 1, 1)
		{
			Tag = StiStyleLocation.TopCenter,
			Checked = ((Location & StiStyleLocation.TopCenter) > StiStyleLocation.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("PropertyEnum", "ContentAlignmentTopRight"), 2, 2)
		{
			Tag = StiStyleLocation.TopRight,
			Checked = ((Location & StiStyleLocation.TopRight) > StiStyleLocation.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("PropertyEnum", "ContentAlignmentMiddleLeft"), 3, 3)
		{
			Tag = StiStyleLocation.MiddleLeft,
			Checked = ((Location & StiStyleLocation.MiddleLeft) > StiStyleLocation.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("PropertyEnum", "ContentAlignmentMiddleCenter"), 4, 4)
		{
			Tag = StiStyleLocation.MiddleCenter,
			Checked = ((Location & StiStyleLocation.MiddleCenter) > StiStyleLocation.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("PropertyEnum", "ContentAlignmentMiddleRight"), 5, 5)
		{
			Tag = StiStyleLocation.MiddleRight,
			Checked = ((Location & StiStyleLocation.MiddleRight) > StiStyleLocation.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("PropertyEnum", "ContentAlignmentBottomLeft"), 6, 6)
		{
			Tag = StiStyleLocation.BottomLeft,
			Checked = ((Location & StiStyleLocation.BottomLeft) > StiStyleLocation.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("PropertyEnum", "ContentAlignmentBottomCenter"), 7, 7)
		{
			Tag = StiStyleLocation.BottomCenter,
			Checked = ((Location & StiStyleLocation.BottomCenter) > StiStyleLocation.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("PropertyEnum", "ContentAlignmentBottomRight"), 8, 8)
		{
			Tag = StiStyleLocation.BottomRight,
			Checked = ((Location & StiStyleLocation.BottomRight) > StiStyleLocation.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("PropertyEnum", "HorizontalAlignmentLeft"), 9, 9)
		{
			Tag = StiStyleLocation.Left,
			Checked = ((Location & StiStyleLocation.Left) > StiStyleLocation.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("Toolbars", "CenterHorizontally"), 10, 10)
		{
			Tag = StiStyleLocation.CenterHorizontal,
			Checked = ((Location & StiStyleLocation.CenterHorizontal) > StiStyleLocation.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("PropertyEnum", "HorizontalAlignmentRight"), 11, 11)
		{
			Tag = StiStyleLocation.Right,
			Checked = ((Location & StiStyleLocation.Right) > StiStyleLocation.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("PropertyEnum", "StiVertAlignmentTop"), 12, 12)
		{
			Tag = StiStyleLocation.Top,
			Checked = ((Location & StiStyleLocation.Top) > StiStyleLocation.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("Toolbars", "CenterVertically"), 13, 13)
		{
			Tag = StiStyleLocation.CenterVertical,
			Checked = ((Location & StiStyleLocation.CenterHorizontal) > StiStyleLocation.None)
		};
		e.Nodes.Add(val3);
		val3 = new TreeNode(StiLocalization.Get("PropertyEnum", "StiVertAlignmentBottom"), 14, 14)
		{
			Tag = StiStyleLocation.Bottom,
			Checked = ((Location & StiStyleLocation.Bottom) > StiStyleLocation.None)
		};
		e.Nodes.Add(val3);
	}

	private void LocationTreeView_AfterCheck(object sender, TreeViewEventArgs e)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		Location = StiStyleLocation.None;
		TreeView val = (TreeView)((sender is TreeView) ? sender : null);
		foreach (TreeNode node in val.Nodes)
		{
			TreeNode val2 = node;
			if (val2.Checked)
			{
				Location |= (StiStyleLocation)val2.Tag;
			}
		}
	}

	private void CheckBoxPlacementNestedLevel_CheckedChanged(object sender, EventArgs e)
	{
		Stimulsoft.Controls.StiNumericUpDown stiNumericUpDown = textBoxNestedLevel;
		bool enabled = (((Control)comboBoxOperationPlacementNestedLevel).Enabled = checkBoxPlacementNestedLevel.Checked);
		((Control)stiNumericUpDown).Enabled = enabled;
	}

	private void DoScaling()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		ImageList val = new ImageList();
		val.ImageSize = new Size(StiScale.I16, StiScale.I16);
		val.ColorDepth = (ColorDepth)32;
		foreach (Image image2 in imageListLocation.Images)
		{
			Image image = image2;
			val.Images.Add(StiImageResizer.GetImage(image, allowDispose: false));
		}
		imageListLocation = val;
		treeBoxPlacement.FixedHeight = false;
		CheckBox obj = checkBoxPlacement;
		StiHitTestVisibleLabel stiHitTestVisibleLabel = labelPlacement;
		int num = (((Control)treeBoxPlacement).Height = ((Control)comboBoxOperationPlacement).Height);
		int height2 = (((Control)stiHitTestVisibleLabel).Height = num);
		((Control)obj).Height = height2;
		textBoxNestedLevel.FixedHeight = false;
		Stimulsoft.Controls.StiNumericUpDown stiNumericUpDown = textBoxNestedLevel;
		height2 = (((Control)checkBoxPlacementNestedLevel).Height = ((Control)comboBoxOperationPlacement).Height);
		((Control)stiNumericUpDown).Height = height2;
		treeBoxComponentType.FixedHeight = false;
		CheckBox obj2 = checkBoxComponentType;
		StiHitTestVisibleLabel stiHitTestVisibleLabel2 = labelComponentType;
		num = (((Control)treeBoxComponentType).Height = ((Control)comboBoxOperationComponentType).Height);
		height2 = (((Control)stiHitTestVisibleLabel2).Height = num);
		((Control)obj2).Height = height2;
		treeBoxLocation.FixedHeight = false;
		CheckBox obj3 = checkBoxLocation;
		StiHitTestVisibleLabel stiHitTestVisibleLabel3 = labelLocation;
		num = (((Control)treeBoxLocation).Height = ((Control)comboBoxOperationLocation).Height);
		height2 = (((Control)stiHitTestVisibleLabel3).Height = num);
		((Control)obj3).Height = height2;
		((Control)checkBoxName).Height = ((Control)comboBoxOperationName).Height;
	}

	public override void SetBackColor()
	{
		base.SetBackColor();
		SetBackColor(base.IsFocused);
	}

	public void SetBackColor(bool isFocused)
	{
		base.SetBackColor();
		CheckBox obj = checkBoxComponentType;
		CheckBox obj2 = checkBoxLocation;
		CheckBox obj3 = checkBoxName;
		CheckBox obj4 = checkBoxPlacement;
		CheckBox obj5 = checkBoxPlacementNestedLevel;
		StiHitTestVisibleLabel stiHitTestVisibleLabel = labelComponentType;
		StiHitTestVisibleLabel stiHitTestVisibleLabel2 = labelLocation;
		Color color2 = (((Control)labelPlacement).BackColor = (isFocused ? GetLightBaseColor() : Color.White));
		Color color4 = (((Control)stiHitTestVisibleLabel2).BackColor = color2);
		Color color6 = (((Control)stiHitTestVisibleLabel).BackColor = color4);
		Color color8 = (((Control)obj5).BackColor = color6);
		Color color10 = (((Control)obj4).BackColor = color8);
		Color color12 = (((Control)obj3).BackColor = color10);
		Color backColor = (((Control)obj2).BackColor = color12);
		((Control)obj).BackColor = backColor;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_072e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0738: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiStyleConditionZoneControl));
		comboBoxOperationPlacement = new ComboBox();
		textBoxName = new TextBox();
		comboBoxOperationComponentType = new ComboBox();
		comboBoxOperationLocation = new ComboBox();
		comboBoxOperationName = new ComboBox();
		checkBoxPlacement = new CheckBox();
		checkBoxComponentType = new CheckBox();
		checkBoxLocation = new CheckBox();
		checkBoxName = new CheckBox();
		labelPlacement = new StiHitTestVisibleLabel();
		labelComponentType = new StiHitTestVisibleLabel();
		labelLocation = new StiHitTestVisibleLabel();
		imageListPlacement = new ImageList(components);
		toolTip1 = new ToolTip(components);
		imageListComponentType = new ImageList(components);
		imageListLocation = new ImageList(components);
		treeBoxLocation = new StiTreeViewBox();
		treeBoxComponentType = new StiTreeViewBox();
		treeBoxPlacement = new StiTreeViewBox();
		textBoxNestedLevel = new Stimulsoft.Controls.StiNumericUpDown();
		checkBoxPlacementNestedLevel = new CheckBox();
		comboBoxOperationPlacementNestedLevel = new ComboBox();
		((ISupportInitialize)textBoxNestedLevel).BeginInit();
		((Control)this).SuspendLayout();
		comboBoxOperationPlacement.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxOperationPlacement).FormattingEnabled = true;
		((Control)comboBoxOperationPlacement).Location = new Point(198, 11);
		((Control)comboBoxOperationPlacement).Name = "comboBoxOperationPlacement";
		((Control)comboBoxOperationPlacement).Size = new Size(121, 21);
		((Control)comboBoxOperationPlacement).TabIndex = 1;
		((Control)textBoxName).BackColor = SystemColors.Window;
		((Control)textBoxName).Location = new Point(325, 122);
		((Control)textBoxName).Name = "textBoxName";
		((Control)textBoxName).Size = new Size(121, 20);
		((Control)textBoxName).TabIndex = 17;
		comboBoxOperationComponentType.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxOperationComponentType).FormattingEnabled = true;
		((Control)comboBoxOperationComponentType).Location = new Point(198, 66);
		((Control)comboBoxOperationComponentType).Name = "comboBoxOperationComponentType";
		((Control)comboBoxOperationComponentType).Size = new Size(121, 21);
		((Control)comboBoxOperationComponentType).TabIndex = 8;
		comboBoxOperationLocation.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxOperationLocation).FormattingEnabled = true;
		((Control)comboBoxOperationLocation).Location = new Point(198, 93);
		((Control)comboBoxOperationLocation).Name = "comboBoxOperationLocation";
		((Control)comboBoxOperationLocation).Size = new Size(121, 21);
		((Control)comboBoxOperationLocation).TabIndex = 12;
		comboBoxOperationName.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxOperationName).FormattingEnabled = true;
		((Control)comboBoxOperationName).Location = new Point(198, 122);
		((Control)comboBoxOperationName).Name = "comboBoxOperationName";
		((Control)comboBoxOperationName).Size = new Size(121, 21);
		((Control)comboBoxOperationName).TabIndex = 16;
		((Control)checkBoxPlacement).BackColor = Color.White;
		((Control)checkBoxPlacement).Location = new Point(13, 11);
		((Control)checkBoxPlacement).Name = "checkBoxPlacement";
		((Control)checkBoxPlacement).Size = new Size(179, 21);
		((Control)checkBoxPlacement).TabIndex = 0;
		((Control)checkBoxPlacement).Text = "Placement";
		((ButtonBase)checkBoxPlacement).UseVisualStyleBackColor = false;
		checkBoxPlacement.CheckedChanged += CheckBoxPlacement_CheckedChanged;
		((Control)checkBoxComponentType).BackColor = Color.White;
		((Control)checkBoxComponentType).Location = new Point(13, 66);
		((Control)checkBoxComponentType).Name = "checkBoxComponentType";
		((Control)checkBoxComponentType).Size = new Size(179, 21);
		((Control)checkBoxComponentType).TabIndex = 7;
		((Control)checkBoxComponentType).Text = "Component Type";
		((ButtonBase)checkBoxComponentType).UseVisualStyleBackColor = false;
		checkBoxComponentType.CheckedChanged += CheckBoxPlacement_CheckedChanged;
		((Control)checkBoxLocation).BackColor = Color.White;
		((Control)checkBoxLocation).Location = new Point(13, 93);
		((Control)checkBoxLocation).Name = "checkBoxLocation";
		((Control)checkBoxLocation).Size = new Size(179, 21);
		((Control)checkBoxLocation).TabIndex = 11;
		((Control)checkBoxLocation).Text = "Location";
		((ButtonBase)checkBoxLocation).UseVisualStyleBackColor = false;
		checkBoxLocation.CheckedChanged += CheckBoxPlacement_CheckedChanged;
		((Control)checkBoxName).BackColor = Color.White;
		((Control)checkBoxName).Location = new Point(13, 122);
		((Control)checkBoxName).Name = "checkBoxName";
		((Control)checkBoxName).Size = new Size(179, 21);
		((Control)checkBoxName).TabIndex = 15;
		((Control)checkBoxName).Text = "Name";
		((ButtonBase)checkBoxName).UseVisualStyleBackColor = false;
		checkBoxName.CheckedChanged += CheckBoxPlacement_CheckedChanged;
		((Control)labelPlacement).Anchor = (AnchorStyles)9;
		((Control)labelPlacement).BackColor = Color.White;
		((Control)labelPlacement).Location = new Point(530, 11);
		((Control)labelPlacement).Name = "labelPlacement";
		((Control)labelPlacement).Size = new Size(34, 21);
		((Control)labelPlacement).TabIndex = 3;
		((Control)labelPlacement).Text = "and";
		((Label)labelPlacement).TextAlign = (ContentAlignment)16;
		((Control)labelComponentType).Anchor = (AnchorStyles)9;
		((Control)labelComponentType).BackColor = Color.White;
		((Control)labelComponentType).Location = new Point(530, 66);
		((Control)labelComponentType).Name = "labelComponentType";
		((Control)labelComponentType).Size = new Size(34, 21);
		((Control)labelComponentType).TabIndex = 10;
		((Control)labelComponentType).Text = "and";
		((Label)labelComponentType).TextAlign = (ContentAlignment)16;
		((Control)labelLocation).Anchor = (AnchorStyles)9;
		((Control)labelLocation).BackColor = Color.White;
		((Control)labelLocation).Location = new Point(530, 93);
		((Control)labelLocation).Name = "labelLocation";
		((Control)labelLocation).Size = new Size(34, 21);
		((Control)labelLocation).TabIndex = 14;
		((Control)labelLocation).Text = "and";
		((Label)labelLocation).TextAlign = (ContentAlignment)16;
		imageListPlacement.ColorDepth = (ColorDepth)32;
		imageListPlacement.ImageSize = new Size(16, 16);
		imageListPlacement.TransparentColor = Color.Magenta;
		imageListComponentType.ColorDepth = (ColorDepth)32;
		imageListComponentType.ImageSize = new Size(16, 16);
		imageListComponentType.TransparentColor = Color.Magenta;
		imageListLocation.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageListLocation.ImageStream");
		imageListLocation.TransparentColor = Color.Magenta;
		imageListLocation.Images.SetKeyName(0, "TopLeft.png");
		imageListLocation.Images.SetKeyName(1, "TopCenter.png");
		imageListLocation.Images.SetKeyName(2, "TopRight.png");
		imageListLocation.Images.SetKeyName(3, "MiddleLeft.png");
		imageListLocation.Images.SetKeyName(4, "MiddleCenter.png");
		imageListLocation.Images.SetKeyName(5, "MiddleRight.png");
		imageListLocation.Images.SetKeyName(6, "BottomLeft.png");
		imageListLocation.Images.SetKeyName(7, "BottomCenter.png");
		imageListLocation.Images.SetKeyName(8, "BottomRight.png");
		imageListLocation.Images.SetKeyName(9, "Left.png");
		imageListLocation.Images.SetKeyName(10, "CenterHorizontal.png");
		imageListLocation.Images.SetKeyName(11, "Right.png");
		imageListLocation.Images.SetKeyName(12, "Top.png");
		imageListLocation.Images.SetKeyName(13, "CenterVertical.png");
		imageListLocation.Images.SetKeyName(14, "Bottom.png");
		((Control)treeBoxLocation).Anchor = (AnchorStyles)13;
		((Control)treeBoxLocation).BackColor = SystemColors.Window;
		treeBoxLocation.Flat = false;
		((Control)treeBoxLocation).ForeColor = SystemColors.ControlText;
		((Control)treeBoxLocation).Location = new Point(325, 93);
		treeBoxLocation.MaxDropDownHeight = 280;
		((Control)treeBoxLocation).Name = "treeBoxLocation";
		((Control)treeBoxLocation).Size = new Size(200, 21);
		((Control)treeBoxLocation).TabIndex = 13;
		treeBoxLocation.TreeNodesFill += TreeBoxLocation_TreeNodesFill;
		((Control)treeBoxComponentType).Anchor = (AnchorStyles)13;
		((Control)treeBoxComponentType).BackColor = SystemColors.Window;
		treeBoxComponentType.Flat = false;
		((Control)treeBoxComponentType).ForeColor = SystemColors.ControlText;
		((Control)treeBoxComponentType).Location = new Point(325, 66);
		treeBoxComponentType.MaxDropDownHeight = 130;
		((Control)treeBoxComponentType).Name = "treeBoxComponentType";
		((Control)treeBoxComponentType).Size = new Size(200, 21);
		((Control)treeBoxComponentType).TabIndex = 9;
		treeBoxComponentType.TreeNodesFill += TreeBoxComponentType_TreeNodesFill;
		((Control)treeBoxPlacement).Anchor = (AnchorStyles)13;
		((Control)treeBoxPlacement).BackColor = SystemColors.Window;
		treeBoxPlacement.Flat = false;
		((Control)treeBoxPlacement).ForeColor = SystemColors.ControlText;
		((Control)treeBoxPlacement).Location = new Point(325, 11);
		treeBoxPlacement.MaxDropDownHeight = 360;
		((Control)treeBoxPlacement).Name = "treeBoxPlacement";
		((Control)treeBoxPlacement).Size = new Size(200, 21);
		((Control)treeBoxPlacement).TabIndex = 2;
		treeBoxPlacement.TreeNodesFill += TreeBoxPlacement_TreeNodesFill;
		((Control)textBoxNestedLevel).Anchor = (AnchorStyles)9;
		((Control)textBoxNestedLevel).BackColor = Color.White;
		textBoxNestedLevel.DecimalPlaces = 0;
		((Control)textBoxNestedLevel).Location = new Point(452, 40);
		((Control)textBoxNestedLevel).Name = "textBoxNestedLevel";
		((Control)textBoxNestedLevel).Size = new Size(72, 21);
		((Control)textBoxNestedLevel).TabIndex = 6;
		((Control)textBoxNestedLevel).Text = "1";
		((Control)checkBoxPlacementNestedLevel).BackColor = Color.White;
		checkBoxPlacementNestedLevel.CheckAlign = (ContentAlignment)64;
		((Control)checkBoxPlacementNestedLevel).Location = new Point(13, 40);
		((Control)checkBoxPlacementNestedLevel).Name = "checkBoxPlacementNestedLevel";
		((Control)checkBoxPlacementNestedLevel).Size = new Size(306, 21);
		((Control)checkBoxPlacementNestedLevel).TabIndex = 4;
		((Control)checkBoxPlacementNestedLevel).Text = "Nested Level";
		((ButtonBase)checkBoxPlacementNestedLevel).TextAlign = (ContentAlignment)64;
		((ButtonBase)checkBoxPlacementNestedLevel).UseVisualStyleBackColor = false;
		checkBoxPlacementNestedLevel.CheckedChanged += CheckBoxPlacementNestedLevel_CheckedChanged;
		((Control)comboBoxOperationPlacementNestedLevel).Anchor = (AnchorStyles)13;
		comboBoxOperationPlacementNestedLevel.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxOperationPlacementNestedLevel).FormattingEnabled = true;
		((Control)comboBoxOperationPlacementNestedLevel).Location = new Point(325, 40);
		((Control)comboBoxOperationPlacementNestedLevel).Name = "comboBoxOperationPlacementNestedLevel";
		((Control)comboBoxOperationPlacementNestedLevel).Size = new Size(121, 21);
		((Control)comboBoxOperationPlacementNestedLevel).TabIndex = 5;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((UserControl)this).AutoSizeMode = (AutoSizeMode)0;
		((Control)this).Controls.Add((Control)(object)comboBoxOperationPlacementNestedLevel);
		((Control)this).Controls.Add((Control)(object)checkBoxPlacementNestedLevel);
		((Control)this).Controls.Add((Control)(object)textBoxNestedLevel);
		((Control)this).Controls.Add((Control)(object)treeBoxLocation);
		((Control)this).Controls.Add((Control)(object)treeBoxComponentType);
		((Control)this).Controls.Add((Control)(object)treeBoxPlacement);
		((Control)this).Controls.Add((Control)(object)labelLocation);
		((Control)this).Controls.Add((Control)(object)labelComponentType);
		((Control)this).Controls.Add((Control)(object)labelPlacement);
		((Control)this).Controls.Add((Control)(object)checkBoxName);
		((Control)this).Controls.Add((Control)(object)checkBoxLocation);
		((Control)this).Controls.Add((Control)(object)checkBoxComponentType);
		((Control)this).Controls.Add((Control)(object)checkBoxPlacement);
		((Control)this).Controls.Add((Control)(object)textBoxName);
		((Control)this).Controls.Add((Control)(object)comboBoxOperationName);
		((Control)this).Controls.Add((Control)(object)comboBoxOperationLocation);
		((Control)this).Controls.Add((Control)(object)comboBoxOperationComponentType);
		((Control)this).Controls.Add((Control)(object)comboBoxOperationPlacement);
		((Control)this).Name = "StiStyleConditionZoneControl";
		base.ShowClose = true;
		((Control)this).Size = new Size(572, 152);
		((ISupportInitialize)textBoxNestedLevel).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
