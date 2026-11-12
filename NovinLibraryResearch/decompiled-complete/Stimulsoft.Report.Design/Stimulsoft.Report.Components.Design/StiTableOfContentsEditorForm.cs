using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Design.Panels;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Components.Design;

public class StiTableOfContentsEditorForm : StiInplaceToolForm
{
	private IContainer components;

	private ToolTip toolTip;

	private StiButtonPanelUX panelFooter;

	private StiButtonUX buttonOk;

	private StiButtonUX buttonCancel;

	private StiTreeView treeViewElements;

	private StiLabelUX labelExpression;

	private StiPanelBox panelBoxExpression;

	private StiOperationButton buttonExpressionEdit;

	private HintTextBox textBoxExpression;

	private StiOperationButton buttonExpressionMenu;

	private StiLabelUX labelInfo;

	private ContextMenuBar contextMenuBar;

	private ButtonItem menuExpression;

	internal virtual StiDesignerControl Designer { get; set; }

	internal StiReport Report => TableOfContents.Report;

	internal StiTableOfContents TableOfContents { get; }

	public override bool AllowResize => true;

	public StiTableOfContentsEditorForm()
		: this(null)
	{
	}

	public StiTableOfContentsEditorForm(StiTableOfContents tableOfContents)
	{
		InitializeComponent();
		TableOfContents = tableOfContents;
		Designer = Report?.Designer?.DesignerControl as StiDesignerControl;
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		((TreeView)treeViewElements).CheckBoxes = true;
		((TreeView)treeViewElements).HideSelection = false;
		LocalizeInternal();
		BuildReportTree();
		DoScaling();
	}

	public override void Localize()
	{
		LocalizeInternal();
		((Control)this).Invalidate();
	}

	private void LocalizeInternal()
	{
		((Control)this).Text = Loc.Get("Components", "StiTableOfContents");
		((Control)labelInfo).Text = Loc.Get("Report", "LabelTableOfContentsHint");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void DoScaling()
	{
		((ButtonBase)buttonExpressionEdit).Image = StiReportDesignImages.Actions.Edit();
		StiButtonsLocator.ArrangeButtons((Control)(object)panelBoxExpression);
	}

	public void RefreshElementView()
	{
		StiDesignerControl designer = Designer;
		if (designer != null)
		{
			((Control)designer).Refresh();
		}
		Designer?.Builder.Redraw();
	}

	private void BuildReportTree()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		((TreeView)treeViewElements).BeginUpdate();
		((TreeView)treeViewElements).Nodes.Clear();
		ImageList imageList = new ImageList
		{
			ColorDepth = (ColorDepth)32,
			ImageSize = StiScale.I(new Size(16, 16))
		};
		((TreeView)treeViewElements).ImageList = imageList;
		((TreeView)treeViewElements).Nodes.Clear();
		Hashtable imagesHash = new Hashtable();
		TreeNode reportNode = BuildReportNode(imageList);
		foreach (StiPage page in Report.Pages)
		{
			BuildPageNode(imageList, imagesHash, reportNode, page);
		}
		((TreeView)treeViewElements).ExpandAll();
		((Control)treeViewElements).Enabled = true;
		((TreeView)treeViewElements).EndUpdate();
	}

	private TreeNode BuildReportNode(ImageList imageList)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		TreeNode val = new TreeNode(Report.ToString());
		val.Checked = IsChecked(Report);
		Image imageType = Designer.GetImageType(typeof(StiReport), typeof(StiReport));
		if (imageType != null)
		{
			imageList.Images.Add(imageType);
			int imageIndex = (val.SelectedImageIndex = imageList.Images.Count - 1);
			val.ImageIndex = imageIndex;
		}
		val.Tag = Designer.Report;
		((TreeView)treeViewElements).Nodes.Add(val);
		return val;
	}

	private bool IsChecked(object element)
	{
		if (element is StiReport)
		{
			return !string.IsNullOrEmpty(TableOfContents.ReportPointer);
		}
		if (element is StiComponent)
		{
			return !string.IsNullOrEmpty((element as StiComponent).Pointer.Value);
		}
		return false;
	}

	private void BuildPageNode(ImageList imageList, Hashtable imagesHash, TreeNode reportNode, StiPage page)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		Image imageType = Designer.GetImageType(page.GetType(), typeof(StiPage));
		if (imageType != null)
		{
			imageList.Images.Add(imageType);
		}
		string text = page.ToString(StiOptions.Dictionary.ShowOnlyAliasForPages && Designer.UseAliases);
		TreeNode val = new TreeNode(text);
		val.Tag = page;
		val.Checked = IsChecked(page);
		reportNode.Nodes.Add(val);
		if (imageType != null)
		{
			int imageIndex = (val.SelectedImageIndex = imageList.Images.Count - 1);
			val.ImageIndex = imageIndex;
		}
		BuildComponentNode(imageList, imagesHash, val, page);
	}

	private void BuildComponentNode(ImageList imageList, Hashtable imagesHash, TreeNode parentNode, StiContainer container)
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		if (container == null)
		{
			return;
		}
		foreach (StiComponent item in from c in container.Components.ToList()
			orderby c.Left
			select c)
		{
			if (!IsAllowedComponent(item))
			{
				continue;
			}
			string text = item.ToString(StiOptions.Dictionary.ShowOnlyAliasForComponents && Designer.UseAliases);
			TreeNode val = new TreeNode(text)
			{
				Tag = item,
				Checked = IsChecked(item)
			};
			if (imagesHash[item.GetType()] == null)
			{
				Image imageType = Designer.GetImageType(item.GetType(), typeof(StiComponent));
				if (imageType != null)
				{
					imageList.Images.Add(imageType);
					Type type = item.GetType();
					int num2 = (val.ImageIndex = imageList.Images.Count - 1);
					int num4 = (val.SelectedImageIndex = num2);
					imagesHash[type] = num4;
				}
			}
			else
			{
				int num4 = (val.ImageIndex = (int)imagesHash[item.GetType()]);
				val.SelectedImageIndex = num4;
			}
			parentNode.Nodes.Add(val);
			BuildComponentNode(imageList, imagesHash, val, item as StiContainer);
		}
	}

	private bool IsAllowedComponent(StiComponent comp)
	{
		if (comp is StiPageHeaderBand || comp is StiPageFooterBand || comp is StiTableOfContents)
		{
			return false;
		}
		if (!(comp is StiBand) && !(comp is StiContainer) && !(comp is StiText) && !(comp is StiImage))
		{
			return false;
		}
		if (comp is StiContainer && comp.Parent is StiBand)
		{
			return false;
		}
		return true;
	}

	private void RefreshUI(TreeViewEventArgs e)
	{
		TreeNode node = e.Node;
		object obj = ((node != null) ? node.Tag : null);
		StiLabelUX stiLabelUX = labelExpression;
		bool enabled = (((Control)panelBoxExpression).Enabled = obj != null && e.Node.Checked);
		((Control)stiLabelUX).Enabled = enabled;
		StiPanelBox stiPanelBox = panelBoxExpression;
		Color backColor = (((Control)textBoxExpression).BackColor = (((Control)labelExpression).Enabled ? Color.White : Color.WhiteSmoke));
		((Control)stiPanelBox).BackColor = backColor;
		if (obj is StiComponent)
		{
			((Control)textBoxExpression).Text = (obj as StiComponent).Pointer.Value;
		}
		else if (obj is StiReport)
		{
			((Control)textBoxExpression).Text = TableOfContents.ReportPointer;
		}
		else
		{
			((Control)textBoxExpression).Text = "";
		}
	}

	private void CleanUncheckedPointers(TreeNodeCollection nodes)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		foreach (TreeNode node in nodes)
		{
			TreeNode val = node;
			if (!val.Checked)
			{
				if (val.Tag is StiComponent stiComponent)
				{
					stiComponent.Pointer.Value = string.Empty;
				}
				if (val.Tag is StiReport)
				{
					TableOfContents.ReportPointer = string.Empty;
				}
			}
			CleanUncheckedPointers(val.Nodes);
		}
	}

	private List<string> GetIdents(object comp)
	{
		List<string> list = new List<string>();
		if (comp is StiReport stiReport)
		{
			if (!string.IsNullOrWhiteSpace(TableOfContents.ReportPointer))
			{
				list.Add(TableOfContents.ReportPointer);
			}
			if (!string.IsNullOrWhiteSpace(stiReport.ReportAlias))
			{
				list.Add(stiReport.ReportAlias);
			}
			if (!string.IsNullOrWhiteSpace(stiReport.ReportName))
			{
				list.Add(stiReport.ReportName);
			}
		}
		if (comp is StiPage stiPage)
		{
			if (!string.IsNullOrWhiteSpace(stiPage.Alias))
			{
				list.Add(stiPage.Alias);
			}
			if (!string.IsNullOrWhiteSpace(stiPage.Name))
			{
				list.Add(stiPage.Name);
			}
		}
		if (comp is StiGroupHeaderBand stiGroupHeaderBand && !string.IsNullOrWhiteSpace(stiGroupHeaderBand.Condition.Value) && stiGroupHeaderBand.Condition.Value.Contains("{"))
		{
			list.Add(stiGroupHeaderBand.Condition.Value);
		}
		if (comp is StiBand stiBand)
		{
			if (!string.IsNullOrWhiteSpace(stiBand.Bookmark.Value) && stiBand.Bookmark.Value.Contains("{"))
			{
				list.Add(stiBand.Bookmark.Value);
			}
			if (!string.IsNullOrWhiteSpace(stiBand.Hyperlink.Value) && stiBand.Hyperlink.Value.Contains("{"))
			{
				list.Add(stiBand.Hyperlink.Value);
			}
			foreach (StiText item in (from c in stiBand.Components.ToList()
				where c is StiText
				select c).Cast<StiText>())
			{
				if (!string.IsNullOrWhiteSpace(item.Text.Value) && item.Text.Value.Contains("{"))
				{
					list.Add(item.Text.Value);
				}
			}
		}
		if (comp is StiText stiText)
		{
			if (!string.IsNullOrWhiteSpace(stiText.Bookmark.Value) && stiText.Bookmark.Value.Contains("{"))
			{
				list.Add(stiText.Bookmark.Value);
			}
			if (!string.IsNullOrWhiteSpace(stiText.Hyperlink.Value) && stiText.Hyperlink.Value.Contains("{"))
			{
				list.Add(stiText.Hyperlink.Value);
			}
			if (!string.IsNullOrWhiteSpace(stiText.Text.Value) && (stiText.Text.Value.Contains("{") || list.Count == 0))
			{
				list.Add(stiText.Text.Value);
			}
		}
		list = list.Distinct().ToList();
		if (list.Count == 0)
		{
			list.Add("");
		}
		return list;
	}

	private void TreeViewElements_AfterCheck(object sender, TreeViewEventArgs e)
	{
		if (e.Node.Checked)
		{
			object tag = e.Node.Tag;
			if (tag is StiComponent && string.IsNullOrWhiteSpace((tag as StiComponent).Pointer.Value))
			{
				(tag as StiComponent).Pointer.Value = GetIdents(tag)?.FirstOrDefault();
			}
			else if (tag is StiReport && string.IsNullOrWhiteSpace(TableOfContents.ReportPointer))
			{
				TableOfContents.ReportPointer = GetIdents(tag)?.FirstOrDefault();
			}
		}
		RefreshUI(e);
	}

	private void TreeViewElements_AfterSelect(object sender, TreeViewEventArgs e)
	{
		RefreshUI(e);
	}

	private void TextBoxExpression_TextChanged(object sender, EventArgs e)
	{
		if (((TreeView)treeViewElements).SelectedNode != null)
		{
			if (((TreeView)treeViewElements).SelectedNode.Tag is StiComponent stiComponent)
			{
				stiComponent.Pointer.Value = ((Control)textBoxExpression).Text;
			}
			if (((TreeView)treeViewElements).SelectedNode.Tag is StiReport)
			{
				TableOfContents.ReportPointer = ((Control)textBoxExpression).Text;
			}
		}
	}

	private void ButtonExpressionEdit_Click(object sender, EventArgs e)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Invalid comparison between Unknown and I4
		string text = ((Control)textBoxExpression).Text;
		DialogResult val = StiTextEditor.EditText(TableOfContents.Report, ref text, StiCodeEditorMode.EditorExpression, Loc.Get("FormTitles", "ExpressionEditorForm"));
		if ((int)val == 1)
		{
			((Control)textBoxExpression).Text = text;
		}
		((Control)textBoxExpression).Focus();
		((TextBoxBase)textBoxExpression).Select(0, 0);
	}

	private void ButtonExpressionMenu_Click(object sender, EventArgs e)
	{
		Point point = new Point(((Control)panelBoxExpression).Width, ((Control)panelBoxExpression).Height + StiScale.I1);
		if (StiLeftRightHandedHelper.IsLeftHanded)
		{
			point.X -= menuExpression.PopupSize.Width;
		}
		point = ((Control)panelBoxExpression).PointToScreen(point);
		menuExpression.Popup(point);
	}

	private void MenuExpression_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		menuExpression.SubItems.Clear();
		object tag = ((TreeView)treeViewElements).SelectedNode.Tag;
		List<string> idents = GetIdents(tag);
		foreach (string item in idents)
		{
			ButtonItem buttonItem = new ButtonItem();
			buttonItem.Text = item;
			buttonItem.Click += MenuItem_Click;
			menuExpression.SubItems.Add(buttonItem);
		}
	}

	private void MenuItem_Click(object sender, EventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		((Control)textBoxExpression).Text = buttonItem.Text;
	}

	protected override void OnFormClosed(FormClosedEventArgs e)
	{
		base.OnFormClosed(e);
		((PropertyGrid)StiPropertiesPanelHelper.GetPropertyGrid(Designer)).SelectedObject = TableOfContents;
	}

	protected override void OnClosed(EventArgs e)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Invalid comparison between Unknown and I4
		((Form)this).OnClosed(e);
		lock (StiInplaceEditorForm.FormClosingObject)
		{
			if (!base.IsFormClosed)
			{
				base.IsFormClosed = true;
				if ((int)((Form)this).DialogResult != 1)
				{
					StiActions.Clipboard.InvokeUndo(Designer, closeAllInplaceDesigners: false);
					Designer.UndoRedo.RedoStack.Clear();
					((Control)Designer).Refresh();
				}
			}
		}
	}

	protected override void OnHelpButtonClicked(CancelEventArgs e)
	{
		((Form)this).OnHelpButtonClicked(e);
		StiHelpProvider.ShowHelpViewer("user-manual/index.html?report_internals_tableofcontents.htm#editor");
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		base.OnLoad(e);
		Timer timer = new Timer
		{
			Interval = 1000
		};
		timer.Tick += delegate
		{
			timer.Stop();
			((Component)(object)timer).Dispose();
			((TreeView)treeViewElements).SelectedNode = ((IEnumerable)((TreeView)treeViewElements).Nodes).Cast<TreeNode>().FirstOrDefault();
			((Control)treeViewElements).Focus();
		};
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		CleanUncheckedPointers(((TreeView)treeViewElements).Nodes);
		((Form)this).Close();
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Expected O, but got Unknown
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Expected O, but got Unknown
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d5: Expected O, but got Unknown
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_064c: Expected O, but got Unknown
		//IL_0669: Unknown result type (might be due to invalid IL or missing references)
		//IL_0714: Unknown result type (might be due to invalid IL or missing references)
		//IL_0792: Unknown result type (might be due to invalid IL or missing references)
		//IL_0818: Unknown result type (might be due to invalid IL or missing references)
		//IL_0822: Expected O, but got Unknown
		//IL_0934: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiTableOfContentsEditorForm));
		toolTip = new ToolTip(components);
		panelFooter = new StiButtonPanelUX();
		contextMenuBar = new ContextMenuBar();
		menuExpression = new ButtonItem();
		buttonOk = new StiButtonUX();
		buttonCancel = new StiButtonUX();
		treeViewElements = new StiTreeView();
		labelExpression = new StiLabelUX();
		panelBoxExpression = new StiPanelBox();
		buttonExpressionEdit = new StiOperationButton();
		textBoxExpression = new HintTextBox();
		buttonExpressionMenu = new StiOperationButton();
		labelInfo = new StiLabelUX();
		((Control)panelFooter).SuspendLayout();
		((ISupportInitialize)contextMenuBar).BeginInit();
		((Control)panelBoxExpression).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelFooter).BackColor = Color.White;
		((Control)panelFooter).Controls.Add((Control)(object)contextMenuBar);
		((Control)panelFooter).Controls.Add((Control)(object)buttonOk);
		((Control)panelFooter).Controls.Add((Control)(object)buttonCancel);
		((Control)panelFooter).Dock = (DockStyle)2;
		panelFooter.LinePosition = StiLinePosition.Top;
		panelFooter.LineStyle = StiLineStyle.Dot;
		((Control)panelFooter).Location = new Point(0, 466);
		((Control)panelFooter).Name = "panelFooter";
		((Control)panelFooter).Size = new Size(344, 46);
		((Control)panelFooter).TabIndex = 2;
		contextMenuBar.AntiAlias = true;
		contextMenuBar.DockSide = eDockSide.Bottom;
		contextMenuBar.DockTabStripHeight = 50;
		((Control)contextMenuBar).Font = new Font("Segoe UI", 9f);
		contextMenuBar.Items.AddRange(new BaseItem[1] { menuExpression });
		contextMenuBar.Location = new Point(11, 5);
		((Control)contextMenuBar).Margin = new Padding(2);
		contextMenuBar.Name = "contextMenuBar";
		contextMenuBar.Size = new Size(75, 31);
		contextMenuBar.Stretch = true;
		contextMenuBar.Style = eDotNetBarStyle.StyleManagerControlled;
		contextMenuBar.TabIndex = 7;
		contextMenuBar.TabStop = false;
		((Control)contextMenuBar).Text = "contextMenuBar";
		contextMenuBar.WrapItemsDock = true;
		menuExpression.AutoExpandOnClick = true;
		menuExpression.ImagePaddingHorizontal = 16;
		menuExpression.ImagePaddingVertical = 12;
		menuExpression.LockClosePopup = false;
		menuExpression.Name = "menuExpression";
		menuExpression.Text = "menuExpression";
		menuExpression.PopupOpen += MenuExpression_PopupOpen;
		((Control)buttonOk).Anchor = (AnchorStyles)9;
		((Button)buttonOk).DialogResult = (DialogResult)1;
		((Control)buttonOk).Location = new Point(176, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonCancel).Anchor = (AnchorStyles)9;
		((Button)buttonCancel).DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(256, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(76, 24);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Click += This_CancelClick;
		((Control)treeViewElements).Anchor = (AnchorStyles)15;
		((Control)treeViewElements).BackColor = Color.White;
		((TreeView)treeViewElements).BorderStyle = (BorderStyle)0;
		((TreeView)treeViewElements).HideSelection = false;
		((Control)treeViewElements).Location = new Point(13, 11);
		((Control)treeViewElements).Margin = new Padding(2);
		((Control)treeViewElements).Name = "treeViewElements";
		((Control)treeViewElements).Size = new Size(318, 366);
		((Control)treeViewElements).TabIndex = 0;
		((TreeView)treeViewElements).AfterCheck += new TreeViewEventHandler(TreeViewElements_AfterCheck);
		((TreeView)treeViewElements).AfterSelect += new TreeViewEventHandler(TreeViewElements_AfterSelect);
		((Control)labelExpression).Anchor = (AnchorStyles)6;
		((Control)labelExpression).ForeColor = Color.DimGray;
		((Control)labelExpression).Location = new Point(12, 426);
		((Control)labelExpression).Margin = new Padding(2, 0, 2, 0);
		((Control)labelExpression).Name = "labelExpression";
		((Control)labelExpression).Size = new Size(86, 22);
		((Control)labelExpression).TabIndex = 10;
		((Control)labelExpression).Text = "Expression";
		((Label)labelExpression).TextAlign = (ContentAlignment)16;
		((Control)panelBoxExpression).Anchor = (AnchorStyles)14;
		((Control)panelBoxExpression).BackColor = Color.White;
		((Control)panelBoxExpression).Controls.Add((Control)(object)buttonExpressionEdit);
		((Control)panelBoxExpression).Controls.Add((Control)(object)textBoxExpression);
		((Control)panelBoxExpression).Controls.Add((Control)(object)buttonExpressionMenu);
		((Control)panelBoxExpression).Location = new Point(101, 426);
		((Control)panelBoxExpression).Margin = new Padding(2);
		((Control)panelBoxExpression).Name = "panelBoxExpression";
		((Control)panelBoxExpression).Size = new Size(230, 25);
		((Control)panelBoxExpression).TabIndex = 1;
		((Control)buttonExpressionEdit).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonExpressionEdit).Image = (Image)componentResourceManager.GetObject("buttonExpressionEdit.Image");
		((Control)buttonExpressionEdit).Location = new Point(184, 1);
		((Control)buttonExpressionEdit).Margin = new Padding(2);
		((Control)buttonExpressionEdit).Name = "buttonExpressionEdit";
		((Control)buttonExpressionEdit).Size = new Size(23, 23);
		((Control)buttonExpressionEdit).TabIndex = 1;
		((Control)buttonExpressionEdit).TabStop = false;
		((ButtonBase)buttonExpressionEdit).UseVisualStyleBackColor = true;
		((Control)buttonExpressionEdit).Click += ButtonExpressionEdit_Click;
		((Control)textBoxExpression).Anchor = (AnchorStyles)13;
		((Control)textBoxExpression).BackColor = SystemColors.Window;
		((TextBoxBase)textBoxExpression).BorderStyle = (BorderStyle)0;
		((Control)textBoxExpression).Location = new Point(4, 6);
		((Control)textBoxExpression).Margin = new Padding(2);
		((Control)textBoxExpression).Name = "textBoxExpression";
		((Control)textBoxExpression).Size = new Size(176, 13);
		((Control)textBoxExpression).TabIndex = 0;
		((Control)textBoxExpression).TextChanged += TextBoxExpression_TextChanged;
		((Control)buttonExpressionMenu).Anchor = (AnchorStyles)9;
		((Control)buttonExpressionMenu).Location = new Point(206, 1);
		((Control)buttonExpressionMenu).Margin = new Padding(2);
		((Control)buttonExpressionMenu).Name = "buttonExpressionMenu";
		((Control)buttonExpressionMenu).Size = new Size(23, 23);
		((Control)buttonExpressionMenu).TabIndex = 2;
		((Control)buttonExpressionMenu).TabStop = false;
		((ButtonBase)buttonExpressionMenu).UseVisualStyleBackColor = true;
		((Control)buttonExpressionMenu).Click += ButtonExpressionMenu_Click;
		((Control)labelInfo).Anchor = (AnchorStyles)14;
		((Control)labelInfo).Font = new Font("Microsoft Sans Serif", 6.75f);
		((Control)labelInfo).ForeColor = Color.DimGray;
		((Control)labelInfo).Location = new Point(14, 381);
		((Control)labelInfo).Name = "labelInfo";
		((Control)labelInfo).Size = new Size(317, 36);
		((Control)labelInfo).TabIndex = 11;
		((Control)labelInfo).Text = "Please, check the components which should form the table of contents. One component is equal to one heading level.";
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(344, 512);
		((Control)this).Controls.Add((Control)(object)labelInfo);
		((Control)this).Controls.Add((Control)(object)treeViewElements);
		((Control)this).Controls.Add((Control)(object)panelBoxExpression);
		((Control)this).Controls.Add((Control)(object)labelExpression);
		((Control)this).Controls.Add((Control)(object)panelFooter);
		((Form)this).FormBorderStyle = (FormBorderStyle)4;
		((Form)this).Margin = new Padding(0);
		((Control)this).MinimumSize = new Size(320, 520);
		((Control)this).Name = "StiTableOfContentsEditorForm";
		((Control)this).Text = "TableOfContents";
		((Control)panelFooter).ResumeLayout(false);
		((ISupportInitialize)contextMenuBar).EndInit();
		((Control)panelBoxExpression).ResumeLayout(false);
		((Control)panelBoxExpression).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
