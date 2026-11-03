using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Controls;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Win.Parameters.List;

[ToolboxItem(false)]
public class StiListBoolItemControl : UserControl, IStiListItemControl
{
	private StiButtonListInfo btInfo;

	private IContainer components;

	private StiToolButton btRemove;

	private CheckBox checkBox;

	public object Value => checkBox.Checked;

	public string GetValueStr()
	{
		return Value.ToString();
	}

	public void SetFocus()
	{
		((Control)checkBox).Focus();
	}

	private void ButtonRemove_Click(object sender, EventArgs e)
	{
		btInfo.Control.RemoveItem((Control)(object)this);
	}

	public StiListBoolItemControl(StiButtonListInfo btInfo, StiDialogInfoItem infoItem)
	{
		this.btInfo = btInfo;
		InitializeComponent();
		((ButtonBase)btRemove).Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Delete" + StiScale.StepName + ".png");
		if (infoItem is StiBoolDialogInfoItem)
		{
			checkBox.Checked = StiValueHelper.TryToBool((object)((StiBoolDialogInfoItem)infoItem).Key);
		}
		else
		{
			checkBox.Checked = false;
		}
		((Control)checkBox).Text = ((infoItem != null && infoItem.Value != null) ? infoItem.Value : "");
		StiNewRequestFromUserDialogCreatingEventArgs stiNewRequestFromUserDialogCreatingEventArgs = new StiNewRequestFromUserDialogCreatingEventArgs(btInfo.Variable, ((Control)checkBox).Width);
		StiOptions.Engine.GlobalEvents.InvokeNewRequestFromUserDialogCreating(stiNewRequestFromUserDialogCreatingEventArgs);
		((Control)checkBox).Width = stiNewRequestFromUserDialogCreatingEventArgs.Width;
		((Control)btRemove).Left = ((Control)checkBox).Right + 3;
		((Control)this).Width = ((Control)btRemove).Right + 3;
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
		btRemove = new StiToolButton();
		checkBox = new CheckBox();
		((Control)this).SuspendLayout();
		((Control)btRemove).BackgroundImageLayout = (ImageLayout)0;
		btRemove.Draw3DButton = false;
		btRemove.HotDragDrop = false;
		((Control)btRemove).Location = new Point(204, 2);
		((Control)btRemove).Name = "btRemove";
		((Control)btRemove).Size = new Size(22, 22);
		((Control)btRemove).TabIndex = 6;
		((ButtonBase)btRemove).UseVisualStyleBackColor = true;
		((Control)btRemove).Click += ButtonRemove_Click;
		((Control)checkBox).Location = new Point(0, 0);
		((Control)checkBox).Name = "checkBox";
		((Control)checkBox).Size = new Size(202, 25);
		((Control)checkBox).TabIndex = 7;
		((Control)checkBox).Text = "checkBox1";
		((ButtonBase)checkBox).UseVisualStyleBackColor = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)checkBox);
		((Control)this).Controls.Add((Control)(object)btRemove);
		((Control)this).Name = "StiListBoolItemControl";
		((Control)this).Size = new Size(228, 25);
		((Control)this).ResumeLayout(false);
	}
}
