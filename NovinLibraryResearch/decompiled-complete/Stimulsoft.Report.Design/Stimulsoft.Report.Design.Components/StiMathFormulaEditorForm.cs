using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Inplace;

namespace Stimulsoft.Report.Design.Components;

public class StiMathFormulaEditorForm : StiInplaceToolForm
{
	private StiMathFormula mathFormula;

	private IContainer components;

	private StiGroupLine panelFooter;

	private Button buttonOk;

	private Button buttonCancel;

	private StiGroupLine stiGroupLine1;

	private TabControl tabControlArrow;

	private TabPage tabPageBasic;

	private TabPage tabPageArrows;

	private TabPage tabPageMaths;

	private TabPage tabPageFormula;

	private TabPage tabPageAlphabets;

	private TabPage tabPageOperators;

	private TabPage tabPageFunctions;

	private Label labelDescription;

	private StiCodeEditor textBoxLaText;

	internal virtual StiDesignerControl Designer { get; set; }

	public override bool AllowResize => false;

	public StiMathFormulaEditorForm()
		: this(null)
	{
	}

	public StiMathFormulaEditorForm(StiMathFormula mathFormula)
	{
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		this.mathFormula = mathFormula;
		Designer = this.mathFormula?.Report?.Designer?.DesignerControl as StiDesignerControl;
		if (!((Control)this).MinimumSize.IsEmpty)
		{
			((Form)this).Size = ((Control)this).MinimumSize;
		}
		Build();
		LocalizeInternal();
		textBoxLaText.Init(mathFormula.Report, StiCodeEditorMode.EditorText);
		textBoxLaText.ShowInsertSymbol = true;
		textBoxLaText.Editor.WordWrap = true;
		((Form)this).Load += StiMathFormulaEditorForm_Load;
	}

	private void StiMathFormulaEditorForm_Load(object sender, EventArgs e)
	{
		UpdateFormPosition();
	}

	private void FormulaSelectionControl_FomulaClick(object sender, EventArgs e)
	{
		StiMathFormulaSelectionScrollableControl stiMathFormulaSelectionScrollableControl = sender as StiMathFormulaSelectionScrollableControl;
		if (!string.IsNullOrEmpty(stiMathFormulaSelectionScrollableControl.CurrentFormula))
		{
			((Control)textBoxLaText).Text = ((Control)textBoxLaText).Text.Insert(textBoxLaText.Editor.Selection.SelectionStart, stiMathFormulaSelectionScrollableControl.CurrentFormula);
			textBoxLaText.Editor.Selection.SelectionStart = ((Control)textBoxLaText).Text.Length;
			textBoxLaText.Editor.Selection.SelectionLength = 0;
		}
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		WritePropertiesToElement();
		((Form)this).Close();
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void This_Activated(object sender, EventArgs e)
	{
		textBoxLaText.Editor.Selection.SelectionStart = 0;
		textBoxLaText.Editor.Selection.SelectionLength = 0;
		((Control)textBoxLaText.Editor).Focus();
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		((Control)this).OnSizeChanged(e);
		UpdateFormPosition();
	}

	private void LocalizeInternal()
	{
		((Control)this).Text = Loc.Get("Components", "StiMathFormula");
		((Control)tabPageBasic).Text = Loc.Get("Report", "Basic");
		((Control)tabPageAlphabets).Text = Loc.Get("MathFormula", "Alphabets");
		((Control)tabPageArrows).Text = Loc.Get("MathFormula", "Arrows");
		((Control)tabPageFormula).Text = Loc.Get("MathFormula", "Formulas");
		((Control)tabPageFunctions).Text = Loc.Get("PropertyMain", "Functions");
		((Control)tabPageMaths).Text = Loc.Get("MathFormula", "Maths");
		((Control)tabPageOperators).Text = Loc.Get("MathFormula", "Operators");
		((Control)labelDescription).Text = Loc.Get("Messages", "LatexFormat");
	}

	private void UpdateFormPosition()
	{
		if (Designer != null && mathFormula != null)
		{
			Size size = ((Form)this).Size;
			Rectangle rectangle = StiConvert.RectangleToScreen(Designer, mathFormula.GetDisplayRectangle());
			rectangle = ((Control)Designer.PageView.GetCurrentPageView().View).RectangleToScreen(rectangle);
			Point location = ((rectangle.Right + StiInplaceEditor.ButtonMargin + StiInplaceEditor.ButtonSize > Screen.PrimaryScreen.Bounds.Right) ? new Point(rectangle.Left - size.Width - StiScale.I2, rectangle.Top) : new Point(rectangle.Right + StiScale.I2, rectangle.Top));
			if (location.Y + size.Height > Screen.PrimaryScreen.Bounds.Bottom)
			{
				location.Y = Screen.PrimaryScreen.Bounds.Bottom - StiScale.I20 - size.Height;
			}
			((Form)this).Location = location;
		}
	}

	private void Build()
	{
		((Control)textBoxLaText).Text = mathFormula.LaTexExpression;
		StiMathFormulaSelectionScrollableControl stiMathFormulaSelectionScrollableControl = new StiMathFormulaSelectionScrollableControl(StiMathFormulaGroupType.Basic);
		((Control)stiMathFormulaSelectionScrollableControl).Dock = (DockStyle)5;
		stiMathFormulaSelectionScrollableControl.FomulaClick += FormulaSelectionControl_FomulaClick;
		((Control)tabPageBasic).Controls.Add((Control)(object)stiMathFormulaSelectionScrollableControl);
		StiMathFormulaSelectionScrollableControl stiMathFormulaSelectionScrollableControl2 = new StiMathFormulaSelectionScrollableControl(StiMathFormulaGroupType.Functions);
		((Control)stiMathFormulaSelectionScrollableControl2).Dock = (DockStyle)5;
		stiMathFormulaSelectionScrollableControl2.FomulaClick += FormulaSelectionControl_FomulaClick;
		((Control)tabPageFunctions).Controls.Add((Control)(object)stiMathFormulaSelectionScrollableControl2);
		StiMathFormulaSelectionScrollableControl stiMathFormulaSelectionScrollableControl3 = new StiMathFormulaSelectionScrollableControl(StiMathFormulaGroupType.Maths);
		((Control)stiMathFormulaSelectionScrollableControl3).Dock = (DockStyle)5;
		stiMathFormulaSelectionScrollableControl3.FomulaClick += FormulaSelectionControl_FomulaClick;
		((Control)tabPageMaths).Controls.Add((Control)(object)stiMathFormulaSelectionScrollableControl3);
		StiMathFormulaSelectionScrollableControl stiMathFormulaSelectionScrollableControl4 = new StiMathFormulaSelectionScrollableControl(StiMathFormulaGroupType.Arrows);
		((Control)stiMathFormulaSelectionScrollableControl4).Dock = (DockStyle)5;
		stiMathFormulaSelectionScrollableControl4.FomulaClick += FormulaSelectionControl_FomulaClick;
		((Control)tabPageArrows).Controls.Add((Control)(object)stiMathFormulaSelectionScrollableControl4);
		StiMathFormulaSelectionScrollableControl stiMathFormulaSelectionScrollableControl5 = new StiMathFormulaSelectionScrollableControl(StiMathFormulaGroupType.Alphabets);
		((Control)stiMathFormulaSelectionScrollableControl5).Dock = (DockStyle)5;
		stiMathFormulaSelectionScrollableControl5.FomulaClick += FormulaSelectionControl_FomulaClick;
		((Control)tabPageAlphabets).Controls.Add((Control)(object)stiMathFormulaSelectionScrollableControl5);
		StiMathFormulaSelectionScrollableControl stiMathFormulaSelectionScrollableControl6 = new StiMathFormulaSelectionScrollableControl(StiMathFormulaGroupType.Formula);
		((Control)stiMathFormulaSelectionScrollableControl6).Dock = (DockStyle)5;
		stiMathFormulaSelectionScrollableControl6.FomulaClick += FormulaSelectionControl_FomulaClick;
		((Control)tabPageFormula).Controls.Add((Control)(object)stiMathFormulaSelectionScrollableControl6);
		StiMathFormulaSelectionScrollableControl stiMathFormulaSelectionScrollableControl7 = new StiMathFormulaSelectionScrollableControl(StiMathFormulaGroupType.Operators);
		((Control)stiMathFormulaSelectionScrollableControl7).Dock = (DockStyle)5;
		stiMathFormulaSelectionScrollableControl7.FomulaClick += FormulaSelectionControl_FomulaClick;
		((Control)tabPageOperators).Controls.Add((Control)(object)stiMathFormulaSelectionScrollableControl7);
		textBoxLaText.Editor.Selection.SelectionStart = ((Control)textBoxLaText).Text.Length;
		textBoxLaText.Editor.Selection.SelectionLength = 0;
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

	public void WritePropertiesToElement()
	{
		mathFormula.LaTexExpression = ((Control)textBoxLaText).Text;
		RefreshElementView();
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
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0636: Unknown result type (might be due to invalid IL or missing references)
		//IL_069c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0722: Unknown result type (might be due to invalid IL or missing references)
		//IL_0798: Unknown result type (might be due to invalid IL or missing references)
		//IL_0850: Unknown result type (might be due to invalid IL or missing references)
		//IL_093a: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		panelFooter = new StiGroupLine();
		buttonOk = new Button();
		buttonCancel = new Button();
		stiGroupLine1 = new StiGroupLine();
		labelDescription = new Label();
		tabControlArrow = new TabControl();
		tabPageBasic = new TabPage();
		tabPageOperators = new TabPage();
		tabPageAlphabets = new TabPage();
		tabPageMaths = new TabPage();
		tabPageFunctions = new TabPage();
		tabPageArrows = new TabPage();
		tabPageFormula = new TabPage();
		textBoxLaText = new StiCodeEditor();
		((Control)panelFooter).SuspendLayout();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)tabControlArrow).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelFooter).BackColor = Color.White;
		((Control)panelFooter).Controls.Add((Control)(object)buttonOk);
		((Control)panelFooter).Controls.Add((Control)(object)buttonCancel);
		((Control)panelFooter).Dock = (DockStyle)2;
		panelFooter.LinePosition = StiLinePosition.Top;
		panelFooter.LineStyle = StiLineStyle.Dot;
		((Control)panelFooter).Location = new Point(0, 395);
		((Control)panelFooter).Name = "panelFooter";
		((Control)panelFooter).Size = new Size(464, 46);
		((Control)panelFooter).TabIndex = 2;
		((Control)buttonOk).Anchor = (AnchorStyles)9;
		buttonOk.DialogResult = (DialogResult)1;
		((Control)buttonOk).Location = new Point(291, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonCancel).Anchor = (AnchorStyles)9;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(371, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Click += This_CancelClick;
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)labelDescription);
		((Control)stiGroupLine1).Controls.Add((Control)(object)tabControlArrow);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 200);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(464, 195);
		((Control)stiGroupLine1).TabIndex = 5;
		((Control)labelDescription).AutoSize = true;
		((Control)labelDescription).Location = new Point(10, 172);
		((Control)labelDescription).Name = "labelDescription";
		((Control)labelDescription).Size = new Size(280, 13);
		((Control)labelDescription).TabIndex = 6;
		((Control)labelDescription).Text = "The Latex format is used to display mathematical formulas.";
		((Control)tabControlArrow).Anchor = (AnchorStyles)13;
		((Control)tabControlArrow).Controls.Add((Control)(object)tabPageBasic);
		((Control)tabControlArrow).Controls.Add((Control)(object)tabPageOperators);
		((Control)tabControlArrow).Controls.Add((Control)(object)tabPageAlphabets);
		((Control)tabControlArrow).Controls.Add((Control)(object)tabPageMaths);
		((Control)tabControlArrow).Controls.Add((Control)(object)tabPageFunctions);
		((Control)tabControlArrow).Controls.Add((Control)(object)tabPageArrows);
		((Control)tabControlArrow).Controls.Add((Control)(object)tabPageFormula);
		((Control)tabControlArrow).Location = new Point(12, 11);
		((Control)tabControlArrow).Name = "tabControlArrow";
		tabControlArrow.SelectedIndex = 0;
		((Control)tabControlArrow).Size = new Size(441, 154);
		((Control)tabControlArrow).TabIndex = 5;
		tabPageBasic.Location = new Point(4, 22);
		((Control)tabPageBasic).Name = "tabPageBasic";
		((Control)tabPageBasic).Padding = new Padding(3);
		((Control)tabPageBasic).Size = new Size(433, 128);
		tabPageBasic.TabIndex = 0;
		((Control)tabPageBasic).Text = "Basic";
		tabPageBasic.UseVisualStyleBackColor = true;
		tabPageOperators.Location = new Point(4, 22);
		((Control)tabPageOperators).Name = "tabPageOperators";
		((Control)tabPageOperators).Padding = new Padding(3);
		((Control)tabPageOperators).Size = new Size(433, 128);
		tabPageOperators.TabIndex = 4;
		((Control)tabPageOperators).Text = "Operators";
		tabPageOperators.UseVisualStyleBackColor = true;
		tabPageAlphabets.Location = new Point(4, 22);
		((Control)tabPageAlphabets).Name = "tabPageAlphabets";
		((Control)tabPageAlphabets).Padding = new Padding(3);
		((Control)tabPageAlphabets).Size = new Size(433, 128);
		tabPageAlphabets.TabIndex = 3;
		((Control)tabPageAlphabets).Text = "Alphabets";
		tabPageAlphabets.UseVisualStyleBackColor = true;
		tabPageMaths.Location = new Point(4, 22);
		((Control)tabPageMaths).Name = "tabPageMaths";
		((Control)tabPageMaths).Padding = new Padding(3);
		((Control)tabPageMaths).Size = new Size(433, 128);
		tabPageMaths.TabIndex = 2;
		((Control)tabPageMaths).Text = "Math";
		tabPageMaths.UseVisualStyleBackColor = true;
		tabPageFunctions.Location = new Point(4, 22);
		((Control)tabPageFunctions).Margin = new Padding(2);
		((Control)tabPageFunctions).Name = "tabPageFunctions";
		((Control)tabPageFunctions).Size = new Size(433, 128);
		tabPageFunctions.TabIndex = 5;
		((Control)tabPageFunctions).Text = "Functions";
		tabPageFunctions.UseVisualStyleBackColor = true;
		tabPageArrows.Location = new Point(4, 22);
		((Control)tabPageArrows).Name = "tabPageArrows";
		((Control)tabPageArrows).Padding = new Padding(3);
		((Control)tabPageArrows).Size = new Size(433, 128);
		tabPageArrows.TabIndex = 1;
		((Control)tabPageArrows).Text = "Arrow";
		tabPageArrows.UseVisualStyleBackColor = true;
		tabPageFormula.Location = new Point(4, 22);
		((Control)tabPageFormula).Name = "tabPageFormula";
		((Control)tabPageFormula).Padding = new Padding(3);
		((Control)tabPageFormula).Size = new Size(433, 128);
		tabPageFormula.TabIndex = 2;
		((Control)tabPageFormula).Text = "Formula";
		tabPageFormula.UseVisualStyleBackColor = true;
		textBoxLaText.AllowOutlining = false;
		textBoxLaText.AllowSQLFormatting = false;
		((Control)textBoxLaText).Anchor = (AnchorStyles)15;
		((Control)textBoxLaText).BackColor = Color.White;
		textBoxLaText.ForceShowAutoGeneratedCode = false;
		((Control)textBoxLaText).Location = new Point(12, 12);
		((Control)textBoxLaText).Name = "textBoxLaText";
		((Control)textBoxLaText).Padding = new Padding(1);
		textBoxLaText.ReplaceMode = false;
		textBoxLaText.ShowAutoGeneratedCode = false;
		textBoxLaText.ShowInsertSymbol = false;
		textBoxLaText.ShowLineNumbersInEditor = false;
		((Control)textBoxLaText).Size = new Size(442, 177);
		((Control)textBoxLaText).TabIndex = 7;
		textBoxLaText.TextFont = null;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(464, 441);
		((Control)this).Controls.Add((Control)(object)textBoxLaText);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Control)this).Controls.Add((Control)(object)panelFooter);
		((Form)this).Margin = new Padding(3);
		((Control)this).MinimumSize = new Size(464, 441);
		((Control)this).Name = "StiMathFormulaEditorForm";
		((Control)this).Text = "StiMathFormulaEditorFormForm";
		((Form)this).Activated += This_Activated;
		((Control)panelFooter).ResumeLayout(false);
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)stiGroupLine1).PerformLayout();
		((Control)tabControlArrow).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
