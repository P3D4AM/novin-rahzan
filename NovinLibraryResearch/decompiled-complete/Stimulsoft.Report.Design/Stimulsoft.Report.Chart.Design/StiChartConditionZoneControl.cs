using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Design.Components;

namespace Stimulsoft.Report.Chart.Design;

[ToolboxItem(false)]
public class StiChartConditionZoneControl : StiChartFilterZoneControl
{
	public StiColorBoxXF colorBoxColor;

	public Label labelMarkerType;

	public Label labelMarkerAngel;

	public ComboBox comboBoxMarkerType;

	public NumericUpDown numericUpDownMarkerAngel;

	private Label lbColor;

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
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		colorBoxColor = new StiColorBoxXF();
		lbColor = new Label();
		labelMarkerType = new Label();
		labelMarkerAngel = new Label();
		comboBoxMarkerType = new ComboBox();
		numericUpDownMarkerAngel = new NumericUpDown();
		((ISupportInitialize)numericUpDownMarkerAngel).BeginInit();
		((Control)this).SuspendLayout();
		cbDataType.ItemHeight = 13;
		cbItem.ItemHeight = 13;
		cbCondition.ItemHeight = 13;
		dtpValue.Value = new DateTime(2006, 8, 25, 5, 9, 17, 281);
		((Control)colorBoxColor).BackColor = SystemColors.Window;
		colorBoxColor.Flat = false;
		((Control)colorBoxColor).Location = new Point(14, 74);
		((Control)colorBoxColor).Name = "colorBoxColor";
		colorBoxColor.SelectedColor = Color.Transparent;
		((Control)colorBoxColor).Size = new Size(160, 21);
		((Control)colorBoxColor).TabIndex = 27;
		((Control)lbColor).BackColor = Color.Transparent;
		((Control)lbColor).Location = new Point(14, 57);
		((Control)lbColor).Name = "lbColor";
		((Control)lbColor).Size = new Size(160, 16);
		((Control)lbColor).TabIndex = 28;
		((Control)lbColor).Text = "Color";
		lbColor.TextAlign = (ContentAlignment)16;
		((Control)labelMarkerType).BackColor = Color.Transparent;
		((Control)labelMarkerType).Location = new Point(186, 57);
		((Control)labelMarkerType).Name = "labelMarkerType";
		((Control)labelMarkerType).Size = new Size(104, 16);
		((Control)labelMarkerType).TabIndex = 29;
		((Control)labelMarkerType).Text = "Marker Type";
		labelMarkerType.TextAlign = (ContentAlignment)16;
		((Control)labelMarkerType).Visible = false;
		((Control)labelMarkerAngel).BackColor = Color.Transparent;
		((Control)labelMarkerAngel).Location = new Point(301, 56);
		((Control)labelMarkerAngel).Name = "labelMarkerAngel";
		((Control)labelMarkerAngel).Size = new Size(112, 16);
		((Control)labelMarkerAngel).TabIndex = 29;
		((Control)labelMarkerAngel).Text = "Marker Angle";
		labelMarkerAngel.TextAlign = (ContentAlignment)16;
		((Control)labelMarkerAngel).Visible = false;
		((ListControl)comboBoxMarkerType).FormattingEnabled = true;
		((Control)comboBoxMarkerType).Location = new Point(186, 74);
		((Control)comboBoxMarkerType).Name = "comboBoxMarkerType";
		((Control)comboBoxMarkerType).Size = new Size(104, 21);
		((Control)comboBoxMarkerType).TabIndex = 30;
		((Control)comboBoxMarkerType).Visible = false;
		((Control)numericUpDownMarkerAngel).Location = new Point(301, 74);
		numericUpDownMarkerAngel.Maximum = new decimal(new int[4] { 360, 0, 0, 0 });
		numericUpDownMarkerAngel.Minimum = new decimal(new int[4] { 360, 0, 0, -2147483648 });
		((Control)numericUpDownMarkerAngel).Name = "numericUpDownMarkerAngel";
		((Control)numericUpDownMarkerAngel).Size = new Size(120, 20);
		((Control)numericUpDownMarkerAngel).TabIndex = 33;
		((Control)numericUpDownMarkerAngel).Visible = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((Control)this).Controls.Add((Control)(object)numericUpDownMarkerAngel);
		((Control)this).Controls.Add((Control)(object)comboBoxMarkerType);
		((Control)this).Controls.Add((Control)(object)labelMarkerAngel);
		((Control)this).Controls.Add((Control)(object)labelMarkerType);
		((Control)this).Controls.Add((Control)(object)colorBoxColor);
		((Control)this).Controls.Add((Control)(object)lbColor);
		((Control)this).Name = "StiChartConditionZoneControl";
		base.ShowClose = true;
		((Control)this).Size = new Size(488, 107);
		((Control)this).Controls.SetChildIndex((Control)(object)cbCondition, 0);
		((Control)this).Controls.SetChildIndex((Control)(object)cbItem, 0);
		((Control)this).Controls.SetChildIndex((Control)(object)cbDataType, 0);
		((Control)this).Controls.SetChildIndex((Control)(object)tbValue, 0);
		((Control)this).Controls.SetChildIndex((Control)(object)dtpValue, 0);
		((Control)this).Controls.SetChildIndex((Control)(object)lbColor, 0);
		((Control)this).Controls.SetChildIndex((Control)(object)colorBoxColor, 0);
		((Control)this).Controls.SetChildIndex((Control)(object)labelMarkerType, 0);
		((Control)this).Controls.SetChildIndex((Control)(object)labelMarkerAngel, 0);
		((Control)this).Controls.SetChildIndex((Control)(object)comboBoxMarkerType, 0);
		((Control)this).Controls.SetChildIndex((Control)(object)numericUpDownMarkerAngel, 0);
		((ISupportInitialize)numericUpDownMarkerAngel).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}

	public StiChartConditionZoneControl()
		: base(null)
	{
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			Localize();
		}
		dtpValue.Value = DateTime.Now;
	}

	public void Localize()
	{
		((Control)lbColor).Text = StiLocalization.Get("PropertyMain", "Color");
		((Control)labelMarkerAngel).Text = StiLocalization.Get("PropertyMain", "MarkerAngle");
		((Control)labelMarkerType).Text = StiLocalization.Get("PropertyMain", "MarkerType");
	}
}
