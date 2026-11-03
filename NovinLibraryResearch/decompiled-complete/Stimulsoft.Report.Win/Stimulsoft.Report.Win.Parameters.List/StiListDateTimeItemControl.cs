using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Win.Parameters.List;

[ToolboxItem(false)]
public class StiListDateTimeItemControl : UserControl, IStiListItemControl
{
	private StiButtonListInfo buttonListInfo;

	private ButtonX mainButton;

	private IContainer components;

	private StiToolButton buttonRemove;

	private DateTimePicker dateTimePicker;

	public object Value
	{
		get
		{
			if (buttonListInfo.Type == StiListType.DateTimeList)
			{
				if (buttonListInfo.Variable.DialogInfo.DateTimeType == StiDateTimeType.Date)
				{
					return new DateTime(dateTimePicker.Value.Year, dateTimePicker.Value.Month, dateTimePicker.Value.Day);
				}
				return dateTimePicker.Value;
			}
			return dateTimePicker.Value.TimeOfDay;
		}
	}

	public string GetValueStr()
	{
		if (buttonListInfo.Type == StiListType.DateTimeList)
		{
			string dateTimeFormat = StiDateTimeFormatHelper.GetDateTimeFormat(buttonListInfo.Variable.DialogInfo.DateTimeType);
			return ((DateTime)Value).ToString(dateTimeFormat);
		}
		return Value.ToString();
	}

	public void SetFocus()
	{
		((Control)dateTimePicker).Focus();
	}

	private void ButtonRemove_Click(object sender, EventArgs e)
	{
		buttonListInfo.Control.RemoveItem((Control)(object)this);
	}

	private void DateTimePicker_DropDown(object sender, EventArgs e)
	{
		((PopupItemControl)mainButton).LockClosePopup = true;
	}

	private void DateTimePicker_CloseUp(object sender, EventArgs e)
	{
		((PopupItemControl)mainButton).LockClosePopup = false;
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

	public StiListDateTimeItemControl(StiButtonListInfo buttonListInfo, StiDialogInfoItem dialogInfoItem, ButtonX mainButton)
	{
		InitializeComponent();
		this.buttonListInfo = buttonListInfo;
		this.mainButton = mainButton;
		dateTimePicker.CloseUp += DateTimePicker_CloseUp;
		dateTimePicker.DropDown += DateTimePicker_DropDown;
		((ButtonBase)buttonRemove).Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Delete" + StiScale.StepName + ".png");
		if (dialogInfoItem is StiDateTimeDialogInfoItem)
		{
			dateTimePicker.Value = ((StiDateTimeDialogInfoItem)dialogInfoItem).Key;
		}
		else if (dialogInfoItem is StiTimeSpanDialogInfoItem)
		{
			dateTimePicker.Value = ConvertTimeSpanToDateTime(((StiTimeSpanDialogInfoItem)dialogInfoItem).Key);
		}
		if (buttonListInfo.Type == StiListType.DateTimeList)
		{
			StiDateTimeFormatHelper.SetDateTimePickerFormat(dateTimePicker, buttonListInfo.Variable.DialogInfo.DateTimeType);
		}
		else
		{
			dateTimePicker.Format = (DateTimePickerFormat)4;
		}
		StiNewRequestFromUserDialogCreatingEventArgs stiNewRequestFromUserDialogCreatingEventArgs = new StiNewRequestFromUserDialogCreatingEventArgs(buttonListInfo.Variable, ((Control)dateTimePicker).Width);
		StiOptions.Engine.GlobalEvents.InvokeNewRequestFromUserDialogCreating(stiNewRequestFromUserDialogCreatingEventArgs);
		((Control)dateTimePicker).Width = stiNewRequestFromUserDialogCreatingEventArgs.Width;
		((Control)buttonRemove).Left = ((Control)dateTimePicker).Right + StiScaleUI.I3;
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
		buttonRemove = new StiToolButton();
		dateTimePicker = new DateTimePicker();
		((Control)this).SuspendLayout();
		((Control)buttonRemove).BackgroundImageLayout = (ImageLayout)0;
		((ButtonBase)buttonRemove).UseVisualStyleBackColor = true;
		((Control)buttonRemove).Location = new Point(204, 2);
		((Control)buttonRemove).Name = "btRemove";
		((Control)buttonRemove).Size = new Size(22, 22);
		((Control)buttonRemove).TabIndex = 7;
		((ButtonBase)buttonRemove).UseVisualStyleBackColor = true;
		((Control)buttonRemove).Click += ButtonRemove_Click;
		dateTimePicker.Format = (DateTimePickerFormat)8;
		((Control)dateTimePicker).Location = new Point(2, 3);
		((Control)dateTimePicker).Name = "dateTimePicker";
		((Control)dateTimePicker).Size = new Size(200, 21);
		((Control)dateTimePicker).TabIndex = 9;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)dateTimePicker);
		((Control)this).Controls.Add((Control)(object)buttonRemove);
		((Control)this).Name = "StiListDateTimeItemControl";
		((Control)this).Size = new Size(228, 25);
		((Control)this).ResumeLayout(false);
	}
}
