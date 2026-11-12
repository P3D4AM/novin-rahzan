using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Design.Components;

[ToolboxItem(false)]
public class StiMathFormulaSelectionScrollableControl : Control
{
	private StiMathFormulaGroupType groupType;

	private Panel scrolablePanel;

	private StiMathFormulaSelectionControl formulaSelectionControl;

	private bool firstPaint = true;

	public string CurrentFormula => formulaSelectionControl.CurrentFormula;

	public event EventHandler FomulaClick;

	internal StiMathFormulaSelectionScrollableControl(StiMathFormulaGroupType groupType)
	{
		((Control)this).ResizeRedraw = true;
		((Control)this).DoubleBuffered = true;
		this.groupType = groupType;
	}

	protected void InvokeFomulaClick()
	{
		this.FomulaClick?.Invoke(this, EventArgs.Empty);
	}

	private void FormulaSelectionControl_FomulaClick(object sender, EventArgs e)
	{
		InvokeFomulaClick();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		((Control)this).OnPaint(e);
		if (firstPaint)
		{
			firstPaint = false;
			Build();
		}
	}

	private void Build()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		scrolablePanel = new Panel
		{
			AutoScroll = true,
			Dock = (DockStyle)5,
			BackColor = Color.White
		};
		((Control)this).Controls.Add((Control)(object)scrolablePanel);
		StiMathFormulaSelectionControl stiMathFormulaSelectionControl = new StiMathFormulaSelectionControl(groupType);
		((Control)stiMathFormulaSelectionControl).Width = ((Control)scrolablePanel).Width;
		((Control)stiMathFormulaSelectionControl).Height = ((Control)scrolablePanel).Height;
		((Control)stiMathFormulaSelectionControl).Anchor = (AnchorStyles)13;
		formulaSelectionControl = stiMathFormulaSelectionControl;
		formulaSelectionControl.FomulaClick += FormulaSelectionControl_FomulaClick;
		((Control)scrolablePanel).Controls.Add((Control)(object)formulaSelectionControl);
	}
}
