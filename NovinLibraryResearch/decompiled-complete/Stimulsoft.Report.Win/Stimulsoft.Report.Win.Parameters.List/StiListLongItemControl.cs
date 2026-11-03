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
public class StiListLongItemControl : UserControl, IStiListItemControl
{
	private StiButtonListInfo btInfo;

	private IContainer components;

	private StiToolButton btRemove;

	private StiTextBox textBox;

	public object Value => btInfo.Type switch
	{
		StiListType.ByteList => (byte)StiValueHelper.TryToLong((object)((Control)textBox).Text), 
		StiListType.ShortList => (short)StiValueHelper.TryToLong((object)((Control)textBox).Text), 
		StiListType.IntList => (int)StiValueHelper.TryToLong((object)((Control)textBox).Text), 
		_ => StiValueHelper.TryToLong((object)((Control)textBox).Text), 
	};

	public string GetValueStr()
	{
		return Value.ToString();
	}

	public void SetFocus()
	{
		((Control)textBox).Focus();
	}

	private void ButtonRemove_Click(object sender, EventArgs e)
	{
		btInfo.Control.RemoveItem((Control)(object)this);
	}

	public StiListLongItemControl(StiButtonListInfo btInfo, StiDialogInfoItem infoItem)
	{
		this.btInfo = btInfo;
		InitializeComponent();
		((ButtonBase)btRemove).Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Delete" + StiScale.StepName + ".png");
		if (infoItem is StiLongDialogInfoItem)
		{
			((Control)textBox).Text = ((StiLongDialogInfoItem)infoItem).Key.ToString();
		}
		else
		{
			((Control)textBox).Text = "0";
		}
		StiNewRequestFromUserDialogCreatingEventArgs stiNewRequestFromUserDialogCreatingEventArgs = new StiNewRequestFromUserDialogCreatingEventArgs(btInfo.Variable, ((Control)textBox).Width);
		StiOptions.Engine.GlobalEvents.InvokeNewRequestFromUserDialogCreating(stiNewRequestFromUserDialogCreatingEventArgs);
		((Control)textBox).Width = stiNewRequestFromUserDialogCreatingEventArgs.Width;
		((Control)btRemove).Left = ((Control)textBox).Right + 3;
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
		textBox = new StiTextBox();
		((Control)this).SuspendLayout();
		((Control)btRemove).BackgroundImageLayout = (ImageLayout)0;
		((ButtonBase)btRemove).UseVisualStyleBackColor = true;
		((Control)btRemove).Location = new Point(204, 2);
		((Control)btRemove).Name = "btRemove";
		((Control)btRemove).Size = new Size(22, 22);
		((Control)btRemove).TabIndex = 7;
		((ButtonBase)btRemove).UseVisualStyleBackColor = true;
		((Control)btRemove).Click += ButtonRemove_Click;
		((Control)textBox).Location = new Point(2, 2);
		((Control)textBox).Name = "textBox";
		((Control)textBox).Size = new Size(200, 21);
		((Control)textBox).TabIndex = 9;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)textBox);
		((Control)this).Controls.Add((Control)(object)btRemove);
		((Control)this).Name = "StiListLongItemControl";
		((Control)this).Size = new Size(228, 25);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
