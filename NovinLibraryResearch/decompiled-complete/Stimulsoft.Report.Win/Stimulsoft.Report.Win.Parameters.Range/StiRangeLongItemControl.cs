using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Win.Parameters.Controls;

namespace Stimulsoft.Report.Win.Parameters.Range;

[ToolboxItem(false)]
public class StiRangeLongItemControl : UserControl, IStiRangeItemControl
{
	private Type type;

	private StiReport report;

	private StiVariable variable;

	private IContainer components;

	private Label labelFrom;

	private Label labelTo;

	private StiTextBox textBoxFrom;

	private StiTextBox textBoxTo;

	private StiParameterDropDownButtonV2 dropDownButton;

	public object Value
	{
		get
		{
			Stimulsoft.Report.Range range = null;
			if (type == typeof(ByteRange))
			{
				byte result = 0;
				byte result2 = 0;
				byte.TryParse(((Control)textBoxFrom).Text, out result);
				byte.TryParse(((Control)textBoxTo).Text, out result2);
				return new ByteRange(result, result2);
			}
			if (type == typeof(ShortRange))
			{
				short result3 = 0;
				short result4 = 0;
				short.TryParse(((Control)textBoxFrom).Text, out result3);
				short.TryParse(((Control)textBoxTo).Text, out result4);
				return new ShortRange(result3, result4);
			}
			if (type == typeof(IntRange))
			{
				int result5 = 0;
				int result6 = 0;
				int.TryParse(((Control)textBoxFrom).Text, out result5);
				int.TryParse(((Control)textBoxTo).Text, out result6);
				return new IntRange(result5, result6);
			}
			long result7 = 0L;
			long result8 = 0L;
			long.TryParse(((Control)textBoxFrom).Text, out result7);
			long.TryParse(((Control)textBoxTo).Text, out result8);
			return new LongRange(result7, result8);
		}
	}

	private void DropDownButton_ItemChanged(object sender, StiParameterInfoValueEventArgs e)
	{
		if (e.NewValue is StiDialogInfoRange stiDialogInfoRange)
		{
			((Control)textBoxFrom).Text = stiDialogInfoRange.From.ToString();
			((Control)textBoxTo).Text = stiDialogInfoRange.To.ToString();
			StiVariableHelper.SetVariableLabel(report, variable, e.Label);
		}
	}

	public StiRangeLongItemControl(StiVariable variable, StiReport report)
	{
		type = variable.Type;
		this.report = report;
		this.variable = variable;
		InitializeComponent();
		((Control)labelFrom).Text = StiLocalization.Get("PropertyMain", "RangeFrom");
		((Control)labelTo).Text = StiLocalization.Get("PropertyMain", "RangeTo");
		((Control)labelFrom).Width = TextRenderer.MeasureText(((Control)labelFrom).Text, ((Control)labelFrom).Font).Width + StiScale2.I2;
		((Control)labelTo).Width = TextRenderer.MeasureText(((Control)labelTo).Text, ((Control)labelTo).Font).Width + StiScale2.I2;
		dropDownButton.ShowFullBorder = true;
		List<StiDialogInfoItem> dialogInfoItems = variable.DialogInfo.GetDialogInfoItems(typeof(LongRange));
		List<StiDialogInfoRange> list = new List<StiDialogInfoRange>();
		foreach (StiLongRangeDialogInfoItem item2 in dialogInfoItems)
		{
			StiDialogInfoRange item = new StiDialogInfoRange(item2.From, item2.To, item2.Value);
			list.Add(item);
		}
		dropDownButton.ListRanges = list;
		if (list.Count == 0)
		{
			((Control)dropDownButton).Visible = false;
		}
		if (!variable.DialogInfo.AllowUserValues)
		{
			((StiEditBase)textBoxFrom).ReadOnly = true;
			((StiEditBase)textBoxTo).ReadOnly = true;
		}
		StiNewRequestFromUserDialogCreatingEventArgs stiNewRequestFromUserDialogCreatingEventArgs = new StiNewRequestFromUserDialogCreatingEventArgs(variable, ((Control)textBoxFrom).Width);
		StiOptions.Engine.GlobalEvents.InvokeNewRequestFromUserDialogCreating(stiNewRequestFromUserDialogCreatingEventArgs);
		((Control)textBoxFrom).Width = stiNewRequestFromUserDialogCreatingEventArgs.Width;
		((Control)textBoxTo).Width = stiNewRequestFromUserDialogCreatingEventArgs.Width;
		((Control)this).Width = ((Control)labelFrom).Width + ((Control)labelTo).Width + ((Control)textBoxFrom).Width + ((Control)textBoxTo).Width;
		((Control)this).Height = ((Control)textBoxFrom).Height;
		if (((Control)dropDownButton).Visible)
		{
			((Control)this).Width = ((Control)this).Width + (((Control)dropDownButton).Width + StiScale2.I4);
		}
		if (variable.ValueObject is Stimulsoft.Report.Range range)
		{
			((Control)textBoxFrom).Text = range.FromObject.ToString();
			((Control)textBoxTo).Text = range.ToObject.ToString();
			return;
		}
		string text = variable.ValueObject as string;
		if (!string.IsNullOrEmpty(text) && text.Contains("<<|>>") && variable.InitBy == StiVariableInitBy.Expression && report[variable.Name] is Stimulsoft.Report.Range range2)
		{
			((Control)textBoxFrom).Text = range2.FromObject.ToString();
			((Control)textBoxTo).Text = range2.ToObject.ToString();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		labelFrom = new Label();
		labelTo = new Label();
		textBoxFrom = new StiTextBox();
		textBoxTo = new StiTextBox();
		dropDownButton = new StiParameterDropDownButtonV2();
		((Control)this).SuspendLayout();
		((Control)labelFrom).Dock = (DockStyle)3;
		((Control)labelFrom).Location = new Point(0, 0);
		((Control)labelFrom).Name = "labelFrom";
		((Control)labelFrom).Size = new Size(30, 22);
		((Control)labelFrom).TabIndex = 0;
		((Control)labelFrom).Text = "From";
		labelFrom.TextAlign = (ContentAlignment)16;
		((Control)labelTo).Dock = (DockStyle)3;
		((Control)labelTo).Location = new Point(170, 0);
		((Control)labelTo).Name = "labelTo";
		((Control)labelTo).Size = new Size(20, 22);
		((Control)labelTo).TabIndex = 1;
		((Control)labelTo).Text = "To";
		labelTo.TextAlign = (ContentAlignment)16;
		((Control)textBoxFrom).BackColor = Color.White;
		((Control)textBoxFrom).Dock = (DockStyle)3;
		((Control)textBoxFrom).Location = new Point(30, 0);
		((Control)textBoxFrom).Name = "textBoxFrom";
		((Control)textBoxFrom).Size = new Size(140, 21);
		((Control)textBoxFrom).TabIndex = 2;
		((Control)textBoxTo).BackColor = Color.White;
		((Control)textBoxTo).Dock = (DockStyle)3;
		((Control)textBoxTo).Location = new Point(190, 0);
		((Control)textBoxTo).Name = "textBoxTo";
		((Control)textBoxTo).Size = new Size(140, 21);
		((Control)textBoxTo).TabIndex = 3;
		((Control)dropDownButton).Dock = (DockStyle)4;
		((StiToolButton)dropDownButton).Draw3DButton = false;
		((StiToolButton)dropDownButton).DropDownMenu = null;
		((StiToolButton)dropDownButton).HotDragDrop = false;
		((Control)dropDownButton).Location = new Point(333, 0);
		((Control)dropDownButton).Name = "dropDownButton";
		((Control)dropDownButton).Size = new Size(21, 22);
		((Control)dropDownButton).TabIndex = 4;
		((ButtonBase)dropDownButton).UseVisualStyleBackColor = true;
		dropDownButton.ItemChanged += DropDownButton_ItemChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)textBoxTo);
		((Control)this).Controls.Add((Control)(object)labelTo);
		((Control)this).Controls.Add((Control)(object)textBoxFrom);
		((Control)this).Controls.Add((Control)(object)labelFrom);
		((Control)this).Controls.Add((Control)(object)dropDownButton);
		((Control)this).Name = "StiRangeLongItemControl";
		((Control)this).Size = new Size(354, 22);
		((Control)this).ResumeLayout(false);
	}
}
