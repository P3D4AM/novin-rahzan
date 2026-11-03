using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Controls;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Win.Parameters.List;

[ToolboxItem(false)]
public class StiListTimeSpanItemControl : UserControl, IStiListItemControl
{
	private StiButtonListInfo buttonListInfo;

	private IContainer components;

	private StiToolButton buttonRemove;

	private StiTimeSpanPicker timeSpanPicker;

	public object Value => timeSpanPicker.Value;

	public string GetValueStr()
	{
		return Value.ToString();
	}

	public void SetFocus()
	{
		((Control)timeSpanPicker).Focus();
	}

	private void ButtonRemove_Click(object sender, EventArgs e)
	{
		buttonListInfo.Control.RemoveItem((Control)(object)this);
	}

	private DateTime ConvertTimeSpanToDateTime(object obj)
	{
		if (obj is TimeSpan timeSpan)
		{
			try
			{
				return new DateTime(2010, 1, Math.Max(1, timeSpan.Days), Math.Min(23, Math.Max(0, timeSpan.Hours)), Math.Min(59, Math.Max(0, timeSpan.Minutes)), Math.Min(59, Math.Max(0, timeSpan.Seconds)), Math.Min(999, Math.Max(0, timeSpan.Milliseconds)));
			}
			catch
			{
				return DateTime.Now;
			}
		}
		return DateTime.Now;
	}

	public StiListTimeSpanItemControl(StiButtonListInfo buttonListInfo, StiDialogInfoItem dialogInfoItem)
	{
		InitializeComponent();
		this.buttonListInfo = buttonListInfo;
		((ButtonBase)buttonRemove).Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Delete" + StiScale.StepName + ".png");
		if (dialogInfoItem is StiTimeSpanDialogInfoItem stiTimeSpanDialogInfoItem)
		{
			timeSpanPicker.Value = stiTimeSpanDialogInfoItem.Key;
		}
		StiNewRequestFromUserDialogCreatingEventArgs stiNewRequestFromUserDialogCreatingEventArgs = new StiNewRequestFromUserDialogCreatingEventArgs(buttonListInfo.Variable, ((Control)timeSpanPicker).Width);
		StiOptions.Engine.GlobalEvents.InvokeNewRequestFromUserDialogCreating(stiNewRequestFromUserDialogCreatingEventArgs);
		((Control)timeSpanPicker).Width = stiNewRequestFromUserDialogCreatingEventArgs.Width;
		((Control)buttonRemove).Left = ((Control)timeSpanPicker).Right + StiScaleUI.I3;
		((Control)this).Width = ((Control)buttonRemove).Right + StiScaleUI.I3;
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
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		buttonRemove = new StiToolButton();
		timeSpanPicker = new StiTimeSpanPicker();
		((Control)this).SuspendLayout();
		((Control)buttonRemove).BackgroundImageLayout = (ImageLayout)0;
		buttonRemove.Draw3DButton = false;
		buttonRemove.HotDragDrop = false;
		((Control)buttonRemove).Location = new Point(204, 2);
		((Control)buttonRemove).Name = "buttonRemove";
		((Control)buttonRemove).Size = new Size(22, 22);
		((Control)buttonRemove).TabIndex = 7;
		((ButtonBase)buttonRemove).UseVisualStyleBackColor = true;
		((Control)buttonRemove).Click += ButtonRemove_Click;
		((Control)timeSpanPicker).Location = new Point(2, 3);
		((Control)timeSpanPicker).Name = "timeSpanPicker";
		((Control)timeSpanPicker).Padding = new Padding(2);
		timeSpanPicker.ReadOnly = false;
		((Control)timeSpanPicker).Size = new Size(200, 20);
		((Control)timeSpanPicker).TabIndex = 11;
		timeSpanPicker.Value = TimeSpan.Parse("00:00:00");
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)timeSpanPicker);
		((Control)this).Controls.Add((Control)(object)buttonRemove);
		((Control)this).Name = "StiListTimeSpanItemControl";
		((Control)this).Size = new Size(228, 25);
		((Control)this).ResumeLayout(false);
	}
}
