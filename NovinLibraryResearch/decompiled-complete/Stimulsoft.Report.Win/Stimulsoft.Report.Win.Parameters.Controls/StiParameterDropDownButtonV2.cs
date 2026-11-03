using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Win.Parameters.Controls;

[ToolboxItem(false)]
internal class StiParameterDropDownButtonV2 : StiToolButton, IStiPopupParentControl
{
	private StiPopupForm popupForm;

	private object selectedItem;

	private int focusItem;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool LockPopupInvoke { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiVariable Variable { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public int DropDownButtonWidth => SystemInformation.VerticalScrollBarWidth - 1;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public int PopupWidth { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<StiDialogInfoValue> ListValues { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<StiDialogInfoRange> ListRanges { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsImageValue { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsKeyResult { get; set; } = true;


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal bool ShowSearching { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShowFullBorder { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal StiParameterListBox ListBox { get; set; }

	private bool IsAnyFocused
	{
		get
		{
			if (!((Control)this).Focused)
			{
				return ((IEnumerable)((Control)this).Parent.Controls).Cast<Control>().Any((Control c) => c.Focused);
			}
			return true;
		}
	}

	private bool IsAnyMouseOver
	{
		get
		{
			if (!base.IsMouseOver)
			{
				return ((IEnumerable)((Control)this).Parent.Controls).Cast<Control>().Any((Control c) => c.ClientRectangle.Contains(c.PointToClient(Cursor.Position)));
			}
			return true;
		}
	}

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
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Expected O, but got Unknown
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		Graphics graphics = p.Graphics;
		if (!((Control)this).Enabled)
		{
			graphics.FillRectangle(Brushes.WhiteSmoke, clientRectangle);
		}
		else
		{
			graphics.FillRectangle(Brushes.White, clientRectangle);
		}
		Pen val = ((IsAnyFocused || IsAnyMouseOver) ? StiPens.SelectedText : SystemPens.ControlDark);
		if (ShowFullBorder)
		{
			graphics.DrawRectangle(val, clientRectangle.X, clientRectangle.Y, clientRectangle.Width - 1, clientRectangle.Height - 1);
		}
		else
		{
			graphics.DrawLines(val, new Point[4]
			{
				new Point(clientRectangle.X, clientRectangle.Y),
				new Point(clientRectangle.Right - 1, clientRectangle.Y),
				new Point(clientRectangle.Right - 1, clientRectangle.Bottom - 1),
				new Point(clientRectangle.X, clientRectangle.Bottom - 1)
			});
		}
		Color color = StiColors.Selected;
		Color color2 = StiColors.SelectedText;
		if (base.IsPressed)
		{
			color = StiColorUtils.Dark(color, (byte)30);
			color2 = StiColorUtils.Dark(color2, (byte)30);
		}
		SolidBrush val2 = new SolidBrush(color);
		try
		{
			Pen val3 = new Pen(color2);
			try
			{
				if (base.IsMouseOver)
				{
					graphics.FillRectangle((Brush)(object)val2, clientRectangle);
					clientRectangle.Width--;
					clientRectangle.Height--;
					graphics.DrawRectangle(val3, clientRectangle);
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		clientRectangle = ((Control)this).ClientRectangle;
		if (((ButtonBase)this).Image == null)
		{
			((StiToolButton)this).DrawDropDownArrow(graphics, ((Control)this).ClientRectangle);
		}
		else
		{
			graphics.DrawImage(((ButtonBase)this).Image, new Rectangle(clientRectangle.X + (clientRectangle.Width - ((ButtonBase)this).Image.Width) / 2, clientRectangle.Y + (clientRectangle.Height - ((ButtonBase)this).Image.Height) / 2, ((ButtonBase)this).Image.Width, ((ButtonBase)this).Image.Height));
		}
	}

	private void PopupForm_ClosePopup(object sender, EventArgs e)
	{
		((Component)(object)popupForm).Dispose();
		popupForm = null;
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		((StiToolButton)this).OnMouseDown(e);
		if (!LockPopupInvoke && popupForm == null)
		{
			LockPopupInvoke = true;
			popupForm = new StiPopupForm((Control)(object)this)
			{
				BackColor = Color.White
			};
			((Control)popupForm).Paint += new PaintEventHandler(PopupForm_Paint);
			((StiPopupBaseForm)popupForm).ClosePopup += PopupForm_ClosePopup;
			((Control)popupForm).Controls.Add((Control)(object)CreateContent(popupForm));
			Size itemsSize = GetItemsSize();
			int num = Math.Max(((Control)this).Parent.Width, itemsSize.Width);
			int num2 = ((itemsSize.Height > StiScale.I(400)) ? StiScale.I(400) : itemsSize.Height);
			if (num2 == StiScale.I(400))
			{
				num += SystemInformation.VerticalScrollBarWidth;
			}
			num = ((num > StiScale.I(500)) ? StiScale.I(500) : num);
			if (GetPopupWidth() > 0)
			{
				num = GetPopupWidth();
			}
			Rectangle rectangle = ((Control)this).Parent.RectangleToScreen(((Control)this).Parent.ClientRectangle);
			StiPopupForm obj = popupForm;
			int left = rectangle.Left;
			int num3 = rectangle.Top + ((Control)this).Height + StiScale.I1;
			int num4 = num;
			Padding padding = ((Control)popupForm).Padding;
			int num5 = num4 + ((Padding)(ref padding)).Top;
			padding = ((Control)popupForm).Padding;
			int num6 = num5 + ((Padding)(ref padding)).Bottom;
			padding = ((Control)popupForm).Padding;
			int num7 = num2 + ((Padding)(ref padding)).Left;
			padding = ((Control)popupForm).Padding;
			((StiPopupBaseForm)obj).SetLocation(left, num3, num6, num7 + ((Padding)(ref padding)).Right);
			((StiPopupBaseForm)popupForm).ShowPopupForm();
			LockPopupInvoke = false;
		}
		else
		{
			LockPopupInvoke = false;
			Pushed = true;
		}
	}

	private void TextBox_TextChanged(object sender, EventArgs e)
	{
		StiHintTextBox val = (StiHintTextBox)((sender is StiHintTextBox) ? sender : null);
		string value = ((Control)val).Text.ToLower();
		((ListBox)ListBox).Items.Clear();
		foreach (StiDialogInfoValue listValue in ListValues)
		{
			if (listValue.GetString(Variable).ToLower().Contains(value))
			{
				((ListBox)ListBox).Items.Add((object)listValue);
			}
		}
	}

	private void ListBox_Click(object sender, EventArgs e)
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

	private void ListBox_MouseMove(object sender, MouseEventArgs e)
	{
		int num = ((ListBox)ListBox).IndexFromPoint(((Control)ListBox).PointToClient(Cursor.Position));
		if (num != focusItem)
		{
			if (focusItem != -1 && ((ListBox)ListBox).Items.Count > focusItem)
			{
				((Control)ListBox).Invalidate(((ListBox)ListBox).GetItemRectangle(focusItem));
			}
			focusItem = num;
			if (num != -1)
			{
				((Control)ListBox).Invalidate(((ListBox)ListBox).GetItemRectangle(num));
			}
		}
	}

	private void ListBox_MouseLeave(object sender, EventArgs e)
	{
		if (focusItem > -1)
		{
			focusItem = -1;
			((Control)ListBox).Invalidate();
		}
	}

	private void ListBox_DrawItem(object sender, DrawItemEventArgs e)
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Invalid comparison between Unknown and I4
		if (e.Index == -1)
		{
			return;
		}
		e.DrawBackground();
		if (e.Index == focusItem)
		{
			e.Graphics.FillRectangle(SystemBrushes.HotTrack, e.Bounds);
		}
		RectangleF rectangleF = new RectangleF(e.Bounds.Left, e.Bounds.Top, e.Bounds.Width, e.Bounds.Height);
		StringFormat val = new StringFormat();
		try
		{
			val.LineAlignment = (StringAlignment)1;
			val.FormatFlags = (StringFormatFlags)4096;
			val.Trimming = (StringTrimming)3;
			string itemString = GetItemString(e);
			if ((e.State & 1) == 1 || e.Index == focusItem)
			{
				e.Graphics.DrawString(itemString, ((Control)ListBox).Font, SystemBrushes.HighlightText, rectangleF, val);
			}
			else if (e.Index >= 0)
			{
				e.Graphics.DrawString(itemString, ((Control)ListBox).Font, SystemBrushes.ControlText, rectangleF, val);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private string GetItemString(DrawItemEventArgs e)
	{
		object obj = ((ListBox)ListBox).Items[e.Index];
		if (obj is StiDialogInfoValue)
		{
			return ((StiDialogInfoValue)obj).GetString(Variable);
		}
		if (obj is StiDialogInfoRange)
		{
			return ((StiDialogInfoRange)obj).ToString();
		}
		return "";
	}

	internal void InvokeMouseDown()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		((Control)this).OnMouseDown(new MouseEventArgs((MouseButtons)1048576, 1, 0, 0, 0));
	}

	public int GetPopupWidth()
	{
		if (StiOptions.Viewer.RequestFromUserDropDownWidth.HasValue && !StiOptions.Engine.Interaction.ComboBoxDropDownAutoWidth)
		{
			return StiOptions.Viewer.RequestFromUserDropDownWidth.GetValueOrDefault();
		}
		return PopupWidth;
	}

	private Panel CreateContent(StiPopupForm parentPopup)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Expected O, but got Unknown
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Expected O, but got Unknown
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Expected O, but got Unknown
		Panel val = new Panel
		{
			Location = new Point(1, 1),
			Size = new Size(((Control)parentPopup).Width - 2, ((Control)parentPopup).Height - 2),
			Anchor = (AnchorStyles)15
		};
		ListBox = new StiParameterListBox();
		((Control)ListBox).Tag = parentPopup;
		((ListBox)ListBox).DrawItem += new DrawItemEventHandler(ListBox_DrawItem);
		((Control)ListBox).MouseMove += new MouseEventHandler(ListBox_MouseMove);
		((Control)ListBox).MouseLeave += ListBox_MouseLeave;
		if (ListValues != null && ListValues.Count > 0)
		{
			ObjectCollection items = ((ListBox)ListBox).Items;
			object[] array = ListValues.ToArray();
			items.AddRange(array);
		}
		if (ListRanges != null && ListRanges.Count > 0)
		{
			ObjectCollection items2 = ((ListBox)ListBox).Items;
			object[] array = ListRanges.ToArray();
			items2.AddRange(array);
		}
		if (selectedItem != null)
		{
			int num = ((ListBox)ListBox).Items.IndexOf(selectedItem);
			if (num >= 0)
			{
				((ListControl)ListBox).SelectedIndex = num;
			}
		}
		((ListBox)ListBox).Click += ListBox_Click;
		((Control)ListBox).DoubleClick += ListBox_Click;
		StiHintTextBox textBox = null;
		if (ShowSearching && ListValues.Count > 10)
		{
			Panel val2 = new Panel
			{
				Dock = (DockStyle)1,
				Height = StiScale.I(33),
				Padding = new Padding(StiScale.I3)
			};
			textBox = new StiHintTextBox
			{
				Dock = (DockStyle)5,
				Hint = Loc.GetCleaned("Editor", "TypeToSearch")
			};
			((Control)textBox).TextChanged += TextBox_TextChanged;
			StiDotSeparator val3 = new StiDotSeparator
			{
				Dock = (DockStyle)2
			};
			((Control)val2).Controls.Add((Control)(object)textBox);
			((Control)val2).Controls.Add((Control)(object)val3);
			((Control)val).Controls.Add((Control)(object)val2);
			((Control)ListBox).Location = new Point(0, ((Control)val2).Bottom);
			StiParameterListBox listBox = ListBox;
			((Control)listBox).Height = ((Control)listBox).Height - ((Control)val2).Height;
		}
		((Control)val).Controls.Add((Control)(object)ListBox);
		((Control)ListBox).BringToFront();
		if (textBox != null)
		{
			Timer timer = new Timer();
			timer.Interval = 300;
			timer.Start();
			timer.Tick += delegate
			{
				timer.Stop();
				((Component)(object)timer).Dispose();
				((Control)textBox).Focus();
			};
		}
		return val;
	}

	public Size GetItemsSize()
	{
		int num = 0;
		int num2 = ((ListBox)ListBox).Items.Count * ((ListBox)ListBox).ItemHeight;
		foreach (object item in from object i in (IEnumerable)((ListBox)ListBox).Items
			where i != null
			select i)
		{
			num = Math.Max(num, TextRenderer.MeasureText(item.ToString(), ((Control)ListBox).Font).Width);
		}
		return new Size(num + StiScale.I4 + 2, num2 + StiScale.I4 + 2);
	}

	private void InvokeItemChanged(object key, string label)
	{
		this.ItemChanged?.Invoke(this, new StiParameterInfoValueEventArgs(key, label));
	}

	public StiParameterDropDownButtonV2()
	{
		((Control)this).SetStyle((ControlStyles)512, false);
		((StiToolButton)this).IsDrawDropDownArrow = true;
		((StiToolButton)this).Style = (StiToolBarButtonStyle)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Width = DropDownButtonWidth;
		((StiToolButton)this).TabStop = false;
	}
}
