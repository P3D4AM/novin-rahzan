using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Editor;
using Stimulsoft.Report;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Dashboard.Design.Editors;

public class StiExpressionEditorForm : Form
{
	private List<string> idents;

	private IContainer components;

	private ImageList ilTool;

	private Button buttonOk;

	private Button buttonCancel;

	private StiCodeEditor codeEditor;

	private StiGroupLine glButtons;

	private Bar barElements;

	internal ButtonItem buttonInsertConstant;

	private ButtonItem buttonItemTemp;

	private StiGroupLine groupLineTop;

	private Panel panel1;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<string> Idents
	{
		get
		{
			return idents;
		}
		set
		{
			idents = value;
			((Control)groupLineTop).Visible = value != null;
			barElements.Visible = value != null;
		}
	}

	public string CodeEditorText
	{
		get
		{
			return ((Control)codeEditor).Text;
		}
		set
		{
			((Control)codeEditor).Text = value;
		}
	}

	public StiExpressionEditorForm(string text, StiReport report)
	{
		((Form)(object)this).InitializeNetCoreFont();
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			CodeEditorText = text;
			Localize();
			StiFormSettings.Load((Form)(object)this);
			((Control)codeEditor.DictionaryControl).Visible = false;
			((Control)codeEditor.splitter).Visible = false;
			codeEditor.sbEditorInfo.Visible = false;
			codeEditor.Editor.AutoCorrection = false;
			codeEditor.Editor.Gutter.Options = GutterOptions.None;
			((Form)this).ShowIcon = false;
			codeEditor.Init(report, StiCodeEditorMode.EditorExpression);
			((Control)codeEditor.Editor).Focus();
			Idents = null;
			DoScaling();
		}
	}

	private void Localize()
	{
		((Control)this).Text = Loc.GetMain("Hyperlink");
		buttonInsertConstant.Tooltip = StiHint.CreateHint(StiLocalization.Get("FormRichTextEditor", "Insert"));
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void DoScaling()
	{
		buttonInsertConstant.Image = StiReportDesignImages.Editor.Function();
	}

	private void ButtonInsertConstant_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		buttonInsertConstant.SubItems.Clear();
		foreach (string ident in Idents)
		{
			ButtonItem buttonItem = new ButtonItem
			{
				Text = ident,
				Tag = ident
			};
			buttonItem.Click += delegate(object sender2, EventArgs e2)
			{
				ButtonItem buttonItem2 = sender2 as ButtonItem;
				string text = buttonItem2.Tag as string;
				bool flag = true;
				int selectionStart = codeEditor.Editor.Selection.SelectionStart;
				if (selectionStart >= 1 && selectionStart < ((Control)codeEditor.Editor).Text.Length)
				{
					int num = selectionStart;
					while (num >= 0 && ((Control)codeEditor.Editor).Text[num - 1] != '}')
					{
						if (((Control)codeEditor.Editor).Text[num - 1] == '{')
						{
							flag = false;
							break;
						}
						num--;
					}
				}
				if (flag)
				{
					text = "{" + text + "}";
				}
				Clipboard.SetText(text);
				codeEditor.Editor.Selection.Paste();
			};
			buttonInsertConstant.SubItems.Add(buttonItem);
		}
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/report_internals_expressions.htm");
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
	}

	private void This_Activated(object sender, EventArgs e)
	{
		((Control)codeEditor).Focus();
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).DialogResult = (DialogResult)2;
		}
	}

	private void This_Closed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
	}

	private void This_Closing(object sender, CancelEventArgs e)
	{
		if (codeEditor.Editor.SearchDialog.Visible)
		{
			codeEditor.Editor.SearchDialog.Close();
		}
	}

	private void This_Load(object sender, EventArgs e)
	{
		Bar bar = barElements;
		bool visible = (((Control)groupLineTop).Visible = Idents != null && Idents.Count > 0);
		bar.Visible = visible;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Expected O, but got Unknown
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Expected O, but got Unknown
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0620: Unknown result type (might be due to invalid IL or missing references)
		//IL_078c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0796: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiExpressionEditorForm));
		ilTool = new ImageList(components);
		buttonOk = new Button();
		buttonCancel = new Button();
		glButtons = new StiGroupLine();
		barElements = new Bar();
		buttonInsertConstant = new ButtonItem();
		buttonItemTemp = new ButtonItem();
		groupLineTop = new StiGroupLine();
		panel1 = new Panel();
		codeEditor = new StiCodeEditor();
		((Control)glButtons).SuspendLayout();
		((ISupportInitialize)barElements).BeginInit();
		((Control)panel1).SuspendLayout();
		((Control)this).SuspendLayout();
		ilTool.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilTool.ImageStream");
		ilTool.TransparentColor = Color.Magenta;
		ilTool.Images.SetKeyName(0, "");
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		((Control)buttonOk).Location = new Point(449, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(530, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Click += This_CancelClick;
		((Control)glButtons).BackColor = Color.White;
		((Control)glButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)glButtons).Controls.Add((Control)(object)buttonOk);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 321);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(617, 46);
		((Control)glButtons).TabIndex = 1;
		((Control)barElements).AccessibleDescription = "stiToolBar4 (barElements)";
		((Control)barElements).AccessibleName = "stiToolBar4";
		((Control)barElements).AccessibleRole = (AccessibleRole)22;
		barElements.BarType = eBarType.MenuBar;
		((Control)barElements).Dock = (DockStyle)1;
		barElements.DockTabStripHeight = 50;
		((Control)barElements).Font = new Font("Segoe UI", 9f);
		barElements.Items.AddRange(new BaseItem[1] { buttonInsertConstant });
		barElements.Location = new Point(0, 0);
		barElements.Name = "barElements";
		barElements.RoundCorners = false;
		barElements.SingleLineColor = Color.Empty;
		barElements.Size = new Size(593, 31);
		barElements.Stretch = true;
		barElements.Style = eDotNetBarStyle.StyleManagerControlled;
		barElements.TabIndex = 11;
		barElements.TabStop = false;
		((Control)barElements).Text = "stiToolBar4";
		buttonInsertConstant.AutoExpandOnClick = true;
		buttonInsertConstant.Image = (Image)componentResourceManager.GetObject("buttonInsertConstant.Image");
		buttonInsertConstant.ImagePaddingHorizontal = 16;
		buttonInsertConstant.ImagePaddingVertical = 12;
		buttonInsertConstant.Name = "buttonInsertConstant";
		buttonInsertConstant.SubItems.AddRange(new BaseItem[1] { buttonItemTemp });
		buttonInsertConstant.PopupOpen += ButtonInsertConstant_PopupOpen;
		buttonItemTemp.ImagePaddingHorizontal = 16;
		buttonItemTemp.ImagePaddingVertical = 12;
		buttonItemTemp.Name = "buttonItemTemp";
		buttonItemTemp.Text = "buttonItem1";
		((Control)groupLineTop).BackColor = Color.White;
		((Control)groupLineTop).Dock = (DockStyle)1;
		groupLineTop.LinePosition = StiLinePosition.Center;
		groupLineTop.LineStyle = StiLineStyle.Dot;
		((Control)groupLineTop).Location = new Point(0, 31);
		((Control)groupLineTop).Margin = new Padding(2);
		((Control)groupLineTop).Name = "groupLineTop";
		((Control)groupLineTop).Size = new Size(593, 12);
		((Control)groupLineTop).TabIndex = 71;
		((Control)panel1).Anchor = (AnchorStyles)15;
		((Control)panel1).Controls.Add((Control)(object)codeEditor);
		((Control)panel1).Controls.Add((Control)(object)groupLineTop);
		((Control)panel1).Controls.Add((Control)(object)barElements);
		((Control)panel1).Location = new Point(12, 12);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(593, 294);
		((Control)panel1).TabIndex = 72;
		codeEditor.AllowOutlining = false;
		((Control)codeEditor).BackColor = Color.White;
		((Control)codeEditor).Dock = (DockStyle)5;
		codeEditor.ForceShowAutoGeneratedCode = false;
		((Control)codeEditor).Location = new Point(0, 43);
		((Control)codeEditor).Name = "codeEditor";
		((Control)codeEditor).Padding = new Padding(1);
		codeEditor.ReplaceMode = false;
		codeEditor.ShowAutoGeneratedCode = false;
		codeEditor.ShowLineNumbersInEditor = false;
		((Control)codeEditor).Size = new Size(593, 251);
		((Control)codeEditor).TabIndex = 0;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(617, 367);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiExpressionEditorForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).SizeGripStyle = (SizeGripStyle)2;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Hyperlink";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Activated += This_Activated;
		((Form)this).Closing += This_Closing;
		((Form)this).Closed += This_Closed;
		((Form)this).Load += This_Load;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)glButtons).ResumeLayout(false);
		((ISupportInitialize)barElements).EndInit();
		((Control)panel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
