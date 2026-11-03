using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Win.Parameters.Controls;

[ToolboxItem(false)]
internal class StiParameterDropDownButton : StiToolButton, IStiPopupParentControl
{
	private object selectedItem;

	private int popupWidth;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool LockPopupInvoke { get; set; }

	public int HorizontalOffset { get; set; }

	public int PopupWidth
	{
		get
		{
			if (StiOptions.Viewer.RequestFromUserDropDownWidth.HasValue && !StiOptions.Engine.Interaction.ComboBoxDropDownAutoWidth)
			{
				return StiOptions.Viewer.RequestFromUserDropDownWidth.GetValueOrDefault();
			}
			return popupWidth;
		}
		set
		{
			popupWidth = value;
		}
	}

	public int PopupHeight { get; set; }

	[Browsable(false)]
	public List<StiDialogInfoValue> ListValues { get; set; }

	[Browsable(false)]
	public List<StiDialogInfoRange> ListRanges { get; set; }

	public bool IsImageValue { get; set; }

	public bool IsKeyResult { get; set; } = true;


	internal bool ShowSearching { get; set; }

	internal ListBox ListBox { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiToolBarButtonStyle Style
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((StiToolButton)this).Style;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((StiToolButton)this).Style = value;
		}
	}

	[Browsable(false)]
	public bool Pushed
	{
		get
		{
			return ((StiToolButton)this).Pushed;
		}
		set
		{
			((StiToolButton)this).Pushed = value;
		}
	}

	public event StiParameterInfoValueEventHandlers ItemChanged;

	private void PopupForm_Paint(object sender, PaintEventArgs e)
	{
		StiPopupForm val = (StiPopupForm)((sender is StiPopupForm) ? sender : null);
		e.Graphics.DrawRectangle(SystemPens.ControlDark, 0, 0, ((Control)val).Width - 1, ((Control)val).Height - 1);
	}

	protected override void OnPaint(PaintEventArgs p)
	{
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		Graphics graphics = p.Graphics;
		if (((Control)this).Enabled)
		{
			if (Pushed)
			{
				ButtonRenderer.DrawButton(graphics, clientRectangle, (PushButtonState)3);
			}
			else
			{
				ButtonRenderer.DrawButton(graphics, clientRectangle, (PushButtonState)((!base.IsMouseOver) ? 1 : 2));
			}
		}
		else
		{
			ButtonRenderer.DrawButton(graphics, clientRectangle, (PushButtonState)4);
		}
		if (((ButtonBase)this).Image == null)
		{
			((StiToolButton)this).DrawDropDownArrow(graphics, new Rectangle(clientRectangle.X + (clientRectangle.Width - StiScale.I7) / 2 + StiScale.I1, clientRectangle.Y + (clientRectangle.Height - StiScale.I5) / 2 + StiScale.I1, StiScale.I7, StiScale.I5));
		}
		else
		{
			graphics.DrawImage(((ButtonBase)this).Image, new Rectangle(clientRectangle.X + (clientRectangle.Width - ((ButtonBase)this).Image.Width) / 2, clientRectangle.Y + (clientRectangle.Height - ((ButtonBase)this).Image.Height) / 2, ((ButtonBase)this).Image.Width, ((ButtonBase)this).Image.Height));
		}
	}

	private void PopupForm_ClosePopup(object sender, EventArgs e)
	{
		Pushed = false;
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		((StiToolButton)this).OnMouseDown(e);
		if (!LockPopupInvoke && !Pushed)
		{
			LockPopupInvoke = true;
			StiPopupForm val = new StiPopupForm((Control)(object)this)
			{
				BackColor = SystemColors.Window
			};
			((ScrollableControl)val).DockPadding.All = 1;
			Rectangle rectangle = ((Control)this).Parent.RectangleToScreen(((Control)this).Parent.ClientRectangle);
			((StiPopupBaseForm)val).SetLocation(rectangle.Left, rectangle.Top + ((Control)this).Height + StiScale.I1, StiScale.I(PopupWidth), StiScale.I(PopupHeight));
			((Control)val).Paint += new PaintEventHandler(PopupForm_Paint);
			((StiPopupBaseForm)val).ClosePopup += PopupForm_ClosePopup;
			((Control)val).Controls.Add((Control)(object)CreateContent(val));
			((StiPopupBaseForm)val).ShowPopupForm();
			LockPopupInvoke = false;
		}
		else
		{
			LockPopupInvoke = false;
			Pushed = true;
		}
	}

	private void ListBox_SelectedIndexChanged(object sender, EventArgs e)
	{
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		ListBox val = (ListBox)((sender is ListBox) ? sender : null);
		if (val.SelectedItem != null)
		{
			selectedItem = val.SelectedItem;
			if (val.SelectedItem is StiDialogInfoRange)
			{
				InvokeItemChanged(val.SelectedItem, ((StiDialogInfoRange)val.SelectedItem).Value);
			}
			else if (IsKeyResult)
			{
				InvokeItemChanged(((StiDialogInfoValue)val.SelectedItem).Key, ((StiDialogInfoValue)val.SelectedItem).Value);
			}
			else
			{
				InvokeItemChanged((StiDialogInfoValue)val.SelectedItem, ((StiDialogInfoValue)val.SelectedItem).Value);
			}
		}
		Pushed = false;
		object tag = ((Control)val).Tag;
		StiPopupForm val2 = (StiPopupForm)((tag is StiPopupForm) ? tag : null);
		((Control)val2).Paint -= new PaintEventHandler(PopupForm_Paint);
		((Control)val2).Controls.Clear();
		((StiPopupBaseForm)val2).ClosePopupForm();
	}

	private void TextBox_TextChanged(object sender, EventArgs e)
	{
		TextBox val = (TextBox)((sender is TextBox) ? sender : null);
		string value = ((Control)val).Text.ToLower();
		ListBox.Items.Clear();
		foreach (StiDialogInfoValue listValue in ListValues)
		{
			if (listValue.GetString().ToLower().Contains(value))
			{
				ListBox.Items.Add((object)listValue);
			}
		}
	}

	private Panel CreateContent(StiPopupForm parentPopup)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Expected O, but got Unknown
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Expected O, but got Unknown
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Expected O, but got Unknown
		ListBox = new ListBox
		{
			BackColor = SystemColors.Window,
			BorderStyle = (BorderStyle)0,
			Dock = (DockStyle)0,
			Tag = parentPopup,
			Location = new Point(0, 0),
			Size = StiScale.I(new Size(PopupWidth, PopupHeight)),
			DrawMode = (DrawMode)1
		};
		ListBox.ItemHeight = StiScale.I(ListBox.ItemHeight);
		ListBox.DrawItem += new DrawItemEventHandler(ListBox_DrawItem);
		if (ListValues != null && ListValues.Count > 0)
		{
			ObjectCollection items = ListBox.Items;
			object[] array = ListValues.ToArray();
			items.AddRange(array);
		}
		if (ListRanges != null && ListRanges.Count > 0)
		{
			ObjectCollection items2 = ListBox.Items;
			object[] array = ListRanges.ToArray();
			items2.AddRange(array);
		}
		if (selectedItem != null)
		{
			int num = ListBox.Items.IndexOf(selectedItem);
			if (num >= 0)
			{
				((ListControl)ListBox).SelectedIndex = num;
			}
		}
		PopupWidth = (int)((double)StiParameterHelper.CheckDropDownWidth(ListBox, ListBox.Items) / StiScale.Factor);
		((Control)parentPopup).Width = StiScale.I(PopupWidth);
		Panel val = new Panel
		{
			Location = new Point(1, 1),
			Size = StiScale.I(new Size(PopupWidth - 2, PopupHeight - 2))
		};
		ListBox.SelectedIndexChanged += ListBox_SelectedIndexChanged;
		if (ShowSearching && ListValues.Count > 10)
		{
			Panel val2 = new Panel
			{
				Dock = (DockStyle)1,
				Size = StiScale.I(new Size(PopupWidth - 2, StiScale.I(33))),
				Padding = new Padding(3)
			};
			StiHintTextBox val3 = new StiHintTextBox
			{
				Dock = (DockStyle)5,
				Hint = StiLocalization.Get("Editor", "TypeToSearch")
			};
			((Control)val3).TextChanged += TextBox_TextChanged;
			StiDotSeparator val4 = new StiDotSeparator
			{
				Dock = (DockStyle)2
			};
			((Control)val2).Controls.Add((Control)(object)val3);
			((Control)val2).Controls.Add((Control)(object)val4);
			((Control)val).Controls.Add((Control)(object)val2);
			((Control)ListBox).Location = new Point(0, ((Control)val2).Bottom);
			ListBox listBox = ListBox;
			((Control)listBox).Height = ((Control)listBox).Height - ((Control)val2).Height;
		}
		((Control)val).Controls.Add((Control)(object)ListBox);
		return val;
	}

	private void ListBox_DrawItem(object sender, DrawItemEventArgs e)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Invalid comparison between Unknown and I4
		if (e.Index != -1)
		{
			e.DrawBackground();
			RectangleF rectangleF = new RectangleF(e.Bounds.Left, e.Bounds.Top, e.Bounds.Width, e.Bounds.Height);
			if ((e.State & 1) == 1)
			{
				e.Graphics.DrawString(((StiDialogInfoValue)ListBox.Items[e.Index]).GetString(), ((Control)ListBox).Font, SystemBrushes.HighlightText, rectangleF);
			}
			else if (e.Index >= 0 && ListBox.Items[e.Index] is StiDialogInfoValue stiDialogInfoValue)
			{
				e.Graphics.DrawString(stiDialogInfoValue.GetString(), ((Control)ListBox).Font, SystemBrushes.ControlText, rectangleF);
			}
		}
	}

	private void InvokeItemChanged(object key, string label)
	{
		this.ItemChanged?.Invoke(this, new StiParameterInfoValueEventArgs(key, label));
	}

	public StiParameterDropDownButton()
	{
		((StiToolButton)this).IsDrawDropDownArrow = true;
		((StiToolButton)this).Style = (StiToolBarButtonStyle)1;
	}
}
