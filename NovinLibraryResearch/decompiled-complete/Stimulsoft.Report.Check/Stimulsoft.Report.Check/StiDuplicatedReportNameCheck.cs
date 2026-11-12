using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiDuplicatedReportNameCheck : StiReportCheck
{
	private StiDataBuilder dataBuilder = new StiDataBuilder();

	public override string ElementName
	{
		get
		{
			if (base.Element is StiDataSource)
			{
				return ((StiDataSource)base.Element).Name;
			}
			if (base.Element is StiComponent)
			{
				return ((StiComponent)base.Element).Name;
			}
			return null;
		}
	}

	public override bool PreviewVisible
	{
		get
		{
			if (!(base.Element is StiComponent))
			{
				return base.Element is StiDataSource;
			}
			return true;
		}
	}

	public override string ShortMessage => string.Format(StiLocalizationExt.Get("CheckReport", "StiDuplicatedNameCheckShort"));

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckReport", IsDataSource ? "StiDuplicatedReportName2CheckLong" : "StiDuplicatedReportNameCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Error;

	public bool IsDataSource { get; set; }

	private TreeNode GetTreeNode(TreeNodeCollection nodes)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		foreach (TreeNode node in nodes)
		{
			TreeNode val = node;
			if (val.Tag == base.Element)
			{
				return val;
			}
			if (!val.IsExpanded)
			{
				val.Expand();
			}
			TreeNode treeNode = GetTreeNode(val.Nodes);
			if (treeNode != null)
			{
				return treeNode;
			}
			val.Collapse();
		}
		return null;
	}

	public override void CreatePreviewImage(out Image elementImage, out Image highlightedElementImage, bool useScale = false)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Expected O, but got Unknown
		if (!(base.Element is StiDataSource))
		{
			base.CreatePreviewImage(out elementImage, out highlightedElementImage, useScale);
			return;
		}
		int num = 300;
		int num2 = 300;
		if (useScale)
		{
			num *= 2;
			num2 *= 2;
		}
		StiDataSource stiDataSource = base.Element as StiDataSource;
		TreeView val = new TreeView();
		val.BeforeExpand += new TreeViewCancelEventHandler(TreeNode_Expand);
		val.BeforeCollapse += new TreeViewCancelEventHandler(TreeNode_Collapsed);
		val.Scrollable = false;
		val.ImageList = StiDataBuilder.Images;
		((Control)val).Width = num - 8;
		((Control)val).Height = num2 - 8;
		val.BorderStyle = (BorderStyle)0;
		dataBuilder.Build(val.Nodes, stiDataSource.Dictionary, dictionaryDesign: true, includeCalcColumn: true);
		Rectangle rectangle = Rectangle.Empty;
		TreeNode treeNode = GetTreeNode(val.Nodes);
		if (treeNode != null)
		{
			treeNode.EnsureVisible();
			rectangle = treeNode.Bounds;
			rectangle.X += 4;
			rectangle.Y += 4;
		}
		Bitmap val2 = new Bitmap(num, num2);
		Graphics val3 = Graphics.FromImage((Image)(object)val2);
		try
		{
			val3.Clear(Color.White);
			((Control)val).DrawToBitmap(val2, new Rectangle(4, 4, ((Control)val).Width, ((Control)val).Height));
			val3.DrawRectangle(Pens.Gray, new Rectangle(0, 0, ((Image)val2).Width - 1, ((Image)val2).Height - 1));
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		elementImage = (Image)(object)val2;
		highlightedElementImage = null;
		if (rectangle.IsEmpty)
		{
			return;
		}
		object obj = ((Image)val2).Clone();
		Image val4 = (Image)((obj is Image) ? obj : null);
		Graphics val5 = Graphics.FromImage(val4);
		try
		{
			Pen val6 = new Pen(Color.FromArgb(2013200384));
			try
			{
				val6.Width = 3f;
				val5.DrawRectangle(val6, rectangle);
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val5)?.Dispose();
		}
		highlightedElementImage = val4;
	}

	private void TreeNode_Expand(object sender, TreeViewCancelEventArgs e)
	{
		if (e.Node is TreeNodeEx node && e.Node.Tag != null)
		{
			dataBuilder.ExpandedNode(node, includeCalcColumn: true);
		}
	}

	private void TreeNode_Collapsed(object sender, TreeViewCancelEventArgs e)
	{
		if (e.Node is TreeNodeEx && e.Node.Tag != null)
		{
			dataBuilder.CollapsedNode(e.Node);
		}
	}

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		if (report.CalculationMode == StiCalculationMode.Interpretation || string.IsNullOrWhiteSpace(report.ReportName))
		{
			return null;
		}
		List<StiCheck> list = null;
		StringComparison comparisonType = ((report.ScriptLanguage == StiReportLanguageType.VB) ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture);
		foreach (StiDataSource dataSource in report.Dictionary.DataSources)
		{
			if (report.ReportName.Equals(dataSource.Name, comparisonType))
			{
				StiDuplicatedReportNameCheck stiDuplicatedReportNameCheck = new StiDuplicatedReportNameCheck();
				stiDuplicatedReportNameCheck.Element = dataSource;
				stiDuplicatedReportNameCheck.IsDataSource = true;
				if (list == null)
				{
					list = new List<StiCheck>();
				}
				list.Add(stiDuplicatedReportNameCheck);
			}
		}
		StiComponentsCollection components = report.GetComponents();
		foreach (StiComponent item in components)
		{
			if (report.ReportName.Equals(item.Name, comparisonType))
			{
				StiDuplicatedReportNameCheck stiDuplicatedReportNameCheck2 = new StiDuplicatedReportNameCheck();
				stiDuplicatedReportNameCheck2.Element = item;
				stiDuplicatedReportNameCheck2.Actions.Add(new StiEditNameAction());
				stiDuplicatedReportNameCheck2.Actions.Add(new StiGenerateNewNameComponentAction());
				if (list == null)
				{
					list = new List<StiCheck>();
				}
				list.Add(stiDuplicatedReportNameCheck2);
			}
		}
		return list;
	}
}
