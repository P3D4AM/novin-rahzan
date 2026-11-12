using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Data.Engine;

namespace Stimulsoft.Report.Design.DataTransformation.Editors;

public class StiDataTransformRunningTotalForm : Form
{
	private List<StiDataActionRule> actionRules;

	private string columnKey;

	private string columnPath;

	private IContainer components;

	private StiGroupLine stiGroupLine1;

	private Button buttonOk;

	private Button buttonCancel;

	private TextBox textBoxInitialValue;

	private Label labelInitialValue;

	public StiDataTransformRunningTotalForm(List<StiDataActionRule> actionRules, string columnKey, string columnPath)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		this.actionRules = actionRules;
		this.columnKey = columnKey;
		this.columnPath = columnPath;
		Localize();
		ReadColumnActions();
	}

	private void Localize()
	{
		((Control)this).Text = Loc.Get("FormSystemTextEditor", "RunningTotal");
		((Control)labelInitialValue).Text = Loc.Get("Dashboard", "InitialValue");
		((Control)buttonOk).Text = Loc.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = Loc.Get("Buttons", "Cancel");
	}

	private void ReadColumnActions()
	{
		StiDataActionRule stiDataActionRule = actionRules.FirstOrDefault((StiDataActionRule a) => a.Type == StiDataActionType.RunningTotal && (a.Key == columnKey || (a.Key == null && a.Path == columnPath)));
		if (stiDataActionRule != null)
		{
			((Control)textBoxInitialValue).Text = ((stiDataActionRule.InitialValue != null) ? stiDataActionRule.InitialValue.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator) : "0");
		}
	}

	private void WriteColumnActions()
	{
		actionRules.RemoveAll((StiDataActionRule a) => a.Type == StiDataActionType.RunningTotal && (a.Key == columnKey || (a.Key == null && a.Path == columnPath)));
		if (((Control)textBoxInitialValue).Text.Length > 0)
		{
			if (((Control)textBoxInitialValue).Text.IndexOf("{") < 0 || ((Control)textBoxInitialValue).Text.IndexOf("}") < 1)
			{
				decimal result;
				bool flag = decimal.TryParse(((Control)textBoxInitialValue).Text.Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture, out result);
				((Control)textBoxInitialValue).Text = (flag ? result.ToString().Replace(",", ".") : "0");
			}
			StiDataActionRule item = new StiDataActionRule(columnKey, columnPath, ((Control)textBoxInitialValue).Text);
			actionRules.Add(item);
		}
	}

	protected override void OnShown(EventArgs e)
	{
		((Form)this).OnShown(e);
		((Control)textBoxInitialValue).Focus();
	}

	private void ButtonOk_Click(object sender, EventArgs e)
	{
		WriteColumnActions();
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		stiGroupLine1 = new StiGroupLine();
		buttonOk = new Button();
		buttonCancel = new Button();
		textBoxInitialValue = new TextBox();
		labelInitialValue = new Label();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonOk);
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonCancel);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 47);
		((Control)stiGroupLine1).Margin = new Padding(2);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(299, 46);
		((Control)stiGroupLine1).TabIndex = 1;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		buttonOk.DialogResult = (DialogResult)1;
		((Control)buttonOk).Location = new Point(133, 12);
		((Control)buttonOk).Margin = new Padding(2);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += ButtonOk_Click;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(212, 12);
		((Control)buttonCancel).Margin = new Padding(2);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)textBoxInitialValue).Location = new Point(135, 12);
		((Control)textBoxInitialValue).Name = "textBoxInitialValue";
		((Control)textBoxInitialValue).Size = new Size(152, 20);
		((Control)textBoxInitialValue).TabIndex = 0;
		((Control)textBoxInitialValue).Text = "0";
		((Control)labelInitialValue).ForeColor = Color.DimGray;
		((Control)labelInitialValue).Location = new Point(12, 12);
		((Control)labelInitialValue).Name = "labelInitialValue";
		((Control)labelInitialValue).Size = new Size(117, 20);
		((Control)labelInitialValue).TabIndex = 2;
		((Control)labelInitialValue).Text = "Initial Value";
		labelInitialValue.TextAlign = (ContentAlignment)16;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(299, 93);
		((Control)this).Controls.Add((Control)(object)textBoxInitialValue);
		((Control)this).Controls.Add((Control)(object)labelInitialValue);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiDataTransformRunningTotalForm";
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Running Total";
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
