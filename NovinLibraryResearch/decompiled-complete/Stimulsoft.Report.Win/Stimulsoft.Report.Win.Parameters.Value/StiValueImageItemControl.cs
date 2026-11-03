using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Dictionary.Helpers;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Win.Parameters.Controls;

namespace Stimulsoft.Report.Win.Parameters.Value;

[ToolboxItem(false)]
public class StiValueImageItemControl : UserControl, IStiValueItemControl
{
	private IContainer components;

	private Button btOpen;

	private Button btRemove;

	private PictureBox pictureBox;

	private StiParameterDropDownButton dropDownButton;

	internal CheckBox checkBoxNotAssigned;

	public object Value
	{
		get
		{
			if (checkBoxNotAssigned.Checked)
			{
				return null;
			}
			return pictureBox.Image;
		}
	}

	public StiVariable Variable { get; }

	public event EventHandler SelectedValue;

	public void InvokeSelectedValue()
	{
	}

	private void InvokeSelectedValue(object sender)
	{
		this.SelectedValue?.Invoke(sender, EventArgs.Empty);
	}

	public void FilterValues(object selectedObject)
	{
		List<StiDialogInfoItem> dialogInfoItems = Variable.DialogInfo.GetDialogInfoItems(Variable.Type);
		HashSet<Image> hashSet = new HashSet<Image>();
		List<StiDialogInfoValue> list = new List<StiDialogInfoValue>();
		object[] array = StiVariableBindingHelper.GetArray(selectedObject);
		foreach (StiImageDialogInfoItem item in dialogInfoItems)
		{
			foreach (object item2 in item.ValueBinding)
			{
				if (StiVariableBindingHelper.IsEqualBinding(item2, array) && !hashSet.Contains(item.Key))
				{
					hashSet.Add(item.Key);
					list.Add(new StiDialogInfoValue(item.Key, item.Value, item2));
				}
			}
		}
		dropDownButton.ListValues = list;
	}

	private void CheckBoxNotAssigned_CheckedChanged(object sender, EventArgs e)
	{
		((Control)btOpen).Enabled = !checkBoxNotAssigned.Checked;
		((Control)btRemove).Enabled = !checkBoxNotAssigned.Checked;
	}

	private void ButtonRemove_Click(object sender, EventArgs e)
	{
		pictureBox.Image = null;
	}

	private void ButtonOpen_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		try
		{
			OpenFileDialog val = new OpenFileDialog();
			try
			{
				((FileDialog)val).Filter = StiFileDialogHelper.GetImageFilter();
				((FileDialog)val).RestoreDirectory = true;
				if ((int)((CommonDialog)val).ShowDialog() == 1)
				{
					pictureBox.Image = StiImageHelper.FromFile(((FileDialog)val).FileName);
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			StiExceptionProvider.Show(ex, false, (Form)null);
		}
	}

	private void DropDownButton_ItemChanged(object sender, StiParameterInfoValueEventArgs e)
	{
		if (e.NewValue == null)
		{
			if (((Control)checkBoxNotAssigned).Visible)
			{
				checkBoxNotAssigned.Checked = true;
			}
		}
		else
		{
			_003F val = pictureBox;
			object newValue = e.NewValue;
			((PictureBox)val).Image = (Image)((newValue is Image) ? newValue : null);
			InvokeSelectedValue(e.NewValue);
		}
	}

	public StiValueImageItemControl(StiVariable variable, StiReport report, Hashtable variablesCurrentValues)
	{
		InitializeComponent();
		Variable = variable;
		bool allowUserValues = variable.DialogInfo.AllowUserValues;
		((Control)btOpen).BackgroundImage = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.ReportOpen" + StiScale.StepName + ".png");
		((Control)btRemove).BackgroundImage = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Delete" + StiScale.StepName + ".png");
		List<StiDialogInfoItem> andParseDialogInfoItems = variable.DialogInfo.GetAndParseDialogInfoItems(typeof(Image), null, null, report);
		andParseDialogInfoItems = variable.DialogInfo.OrderBy(andParseDialogInfoItems);
		List<StiDialogInfoValue> list = new List<StiDialogInfoValue>();
		foreach (StiImageDialogInfoItem item2 in andParseDialogInfoItems)
		{
			StiDialogInfoValue item = new StiDialogInfoValue(item2.Key, item2.Value);
			list.Add(item);
		}
		dropDownButton.ListValues = list;
		if (list.Count == 0)
		{
			((Control)dropDownButton).Visible = false;
		}
		if (!allowUserValues)
		{
			((Control)btOpen).Visible = false;
			((Control)btRemove).Visible = false;
			((Control)checkBoxNotAssigned).Visible = false;
			((Control)dropDownButton).Left = ((Control)pictureBox).Right + 3;
			((Control)this).Width = ((Control)dropDownButton).Right + 2;
		}
		else
		{
			((Control)this).Width = ((Control)checkBoxNotAssigned).Right + 2;
		}
		dropDownButton.HorizontalOffset = -((Control)dropDownButton).Left;
		switch (variable.Selection)
		{
		case StiSelectionMode.First:
			if (list.Count > 0)
			{
				_003F val3 = pictureBox;
				object key = list[0].Key;
				((PictureBox)val3).Image = (Image)((key is Image) ? key : null);
			}
			else if (((Control)checkBoxNotAssigned).Visible)
			{
				checkBoxNotAssigned.Checked = true;
			}
			break;
		case StiSelectionMode.FromVariable:
		{
			object obj = report[variable.Name];
			Image val = (Image)((obj is Image) ? obj : null);
			if (val != null)
			{
				pictureBox.Image = val;
			}
			if (variable.InitBy == StiVariableInitBy.Expression)
			{
				try
				{
					StiText component = new StiText
					{
						Name = "**VariableRequestFromUser**",
						Page = report.Pages[0]
					};
					object obj2 = StiParser.ParseTextValue("{" + variable.Value + "}", component);
					if (obj2 is Image)
					{
						pictureBox.Image = (Image)((obj2 is Image) ? obj2 : null);
					}
				}
				catch
				{
				}
			}
			if (variablesCurrentValues.ContainsKey(variable.Name))
			{
				_003F val2 = pictureBox;
				object? obj4 = variablesCurrentValues[variable.Name];
				((PictureBox)val2).Image = (Image)((obj4 is Image) ? obj4 : null);
			}
			if (((Control)checkBoxNotAssigned).Visible)
			{
				checkBoxNotAssigned.Checked = true;
			}
			break;
		}
		case StiSelectionMode.Nothing:
			break;
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
		checkBoxNotAssigned = new CheckBox();
		btOpen = new Button();
		btRemove = new Button();
		pictureBox = new PictureBox();
		dropDownButton = new StiParameterDropDownButton();
		((ISupportInitialize)pictureBox).BeginInit();
		((Control)this).SuspendLayout();
		((Control)checkBoxNotAssigned).Location = new Point(173, 3);
		((Control)checkBoxNotAssigned).Name = "checkBoxNotAssigned";
		((Control)checkBoxNotAssigned).Size = new Size(14, 17);
		((Control)checkBoxNotAssigned).TabIndex = 5;
		((ButtonBase)checkBoxNotAssigned).UseVisualStyleBackColor = true;
		checkBoxNotAssigned.CheckedChanged += CheckBoxNotAssigned_CheckedChanged;
		((Control)btOpen).BackgroundImageLayout = (ImageLayout)0;
		((Control)btOpen).Location = new Point(101, -1);
		((Control)btOpen).Name = "btOpen";
		((Control)btOpen).Size = new Size(23, 23);
		((Control)btOpen).TabIndex = 2;
		((ButtonBase)btOpen).UseVisualStyleBackColor = true;
		((Control)btOpen).Click += ButtonOpen_Click;
		((Control)btRemove).BackgroundImageLayout = (ImageLayout)0;
		((Control)btRemove).Location = new Point(123, -1);
		((Control)btRemove).Name = "btRemove";
		((Control)btRemove).Size = new Size(23, 23);
		((Control)btRemove).TabIndex = 3;
		((ButtonBase)btRemove).UseVisualStyleBackColor = true;
		((Control)btRemove).Click += ButtonRemove_Click;
		pictureBox.BorderStyle = (BorderStyle)1;
		((Control)pictureBox).Location = new Point(0, 0);
		((Control)pictureBox).Name = "pictureBox";
		((Control)pictureBox).Size = new Size(100, 21);
		pictureBox.TabIndex = 1;
		pictureBox.TabStop = false;
		((StiToolButton)dropDownButton).Draw3DButton = false;
		((StiToolButton)dropDownButton).DropDownMenu = null;
		dropDownButton.HorizontalOffset = 0;
		((StiToolButton)dropDownButton).HotDragDrop = false;
		dropDownButton.IsImageValue = true;
		dropDownButton.IsKeyResult = true;
		dropDownButton.ListRanges = null;
		dropDownButton.ListValues = null;
		((Control)dropDownButton).Location = new Point(147, 0);
		((Control)dropDownButton).Name = "dropDownButton";
		dropDownButton.PopupHeight = 200;
		dropDownButton.PopupWidth = 250;
		((Control)dropDownButton).Size = new Size(21, 21);
		((Control)dropDownButton).TabIndex = 4;
		((ButtonBase)dropDownButton).UseVisualStyleBackColor = true;
		dropDownButton.ItemChanged += DropDownButton_ItemChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)pictureBox);
		((Control)this).Controls.Add((Control)(object)btRemove);
		((Control)this).Controls.Add((Control)(object)btOpen);
		((Control)this).Controls.Add((Control)(object)dropDownButton);
		((Control)this).Controls.Add((Control)(object)checkBoxNotAssigned);
		((Control)this).Name = "StiValueImageItemControl";
		((Control)this).Size = new Size(192, 21);
		((ISupportInitialize)pictureBox).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
