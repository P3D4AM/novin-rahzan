using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiDropDownBox : StiEditBase, IStiPopupParentControl
{
	[CompilerGenerated]
	private DrawItemEventHandler m_DrawItem;

	private ComboBoxStyle dropDownStyle = (ComboBoxStyle)1;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool LockPopupInvoke { get; set; }

	private bool DroppedDown
	{
		get
		{
			if (DropDownBoxPopupForm != null)
			{
				return ((Control)DropDownBoxPopupForm).Visible;
			}
			return false;
		}
	}

	private bool IsMouseOverButton => GetDownButtonRect().Contains(((Control)this).PointToClient(Cursor.Position));

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual object DataSource
	{
		get
		{
			return ((ListControl)ListBox).DataSource;
		}
		set
		{
			((ListControl)ListBox).DataSource = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[Localizable(true)]
	[MergableProperty(false)]
	public virtual ObjectCollection Items => ((ListBox)ListBox).Items;

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public virtual DrawMode DrawMode
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((ListBox)ListBox).DrawMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((ListBox)ListBox).DrawMode = value;
		}
	}

	[DefaultValue(0)]
	public int DropDownWidth { get; set; }

	[DefaultValue(8)]
	public int MaxDropDownItems { get; set; } = 8;


	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Category("Appearance")]
	public ComboBoxStyle DropDownStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return dropDownStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Invalid comparison between Unknown and I4
			dropDownStyle = value;
			((Control)base.TextBox).Visible = (int)value != 2;
		}
	}

	[DefaultValue(false)]
	public int SelectedIndex
	{
		get
		{
			return ((ListControl)ListBox).SelectedIndex;
		}
		set
		{
			((ListControl)ListBox).SelectedIndex = value;
		}
	}

	[DefaultValue(false)]
	public object SelectedItem
	{
		get
		{
			return ((ListBox)ListBox).SelectedItem;
		}
		set
		{
			((ListBox)ListBox).SelectedItem = value;
		}
	}

	[DefaultValue(false)]
	public object SelectedValue
	{
		get
		{
			return ((ListControl)ListBox).SelectedValue;
		}
		set
		{
			((ListControl)ListBox).SelectedValue = value;
			((Control)base.TextBox).Text = value?.ToString();
		}
	}

	[DefaultValue(false)]
	public bool Sorted
	{
		get
		{
			return ((ListBox)ListBox).Sorted;
		}
		set
		{
			((ListBox)ListBox).Sorted = value;
		}
	}

	[DefaultValue(false)]
	public bool FormattingEnabled
	{
		get
		{
			return ((ListControl)ListBox).FormattingEnabled;
		}
		set
		{
			((ListControl)ListBox).FormattingEnabled = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Image Image { get; set; }

	private int DropDownButtonWidth => SystemInformation.VerticalScrollBarWidth - 1;

	internal StiDropDownBoxPopupForm DropDownBoxPopupForm { get; set; }

	private StiListBoxUX ListBox { get; set; } = new StiListBoxUX();


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int HotTrackingItemIndex
	{
		get
		{
			if (DropDownBoxPopupForm == null)
			{
				return -1;
			}
			return DropDownBoxPopupForm.HotTrackingItemIndex;
		}
		set
		{
			if (DropDownBoxPopupForm != null)
			{
				DropDownBoxPopupForm.HotTrackingItemIndex = value;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string SelectedText
	{
		get
		{
			return ((TextBoxBase)base.TextBox).SelectedText;
		}
		set
		{
			((TextBoxBase)base.TextBox).SelectedText = value;
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Category("Appearance")]
	public HorizontalAlignment TextAlign
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return base.TextBox.TextAlign;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			base.TextBox.TextAlign = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool FixedHeight { get; set; } = true;


	[DefaultValue(false)]
	[Category("Behavior")]
	public bool AcceptsTab
	{
		get
		{
			return ((TextBoxBase)base.TextBox).AcceptsTab;
		}
		set
		{
			((TextBoxBase)base.TextBox).AcceptsTab = value;
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	public bool AcceptsReturn
	{
		get
		{
			return base.TextBox.AcceptsReturn;
		}
		set
		{
			base.TextBox.AcceptsReturn = value;
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	public bool WordWrap
	{
		get
		{
			return ((TextBoxBase)base.TextBox).WordWrap;
		}
		set
		{
			((TextBoxBase)base.TextBox).WordWrap = value;
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	public bool Multiline
	{
		get
		{
			return ((TextBoxBase)base.TextBox).Multiline;
		}
		set
		{
			((TextBoxBase)base.TextBox).Multiline = value;
		}
	}

	[DefaultValue('\0')]
	[Category("CatBehavior")]
	public char PasswordChar
	{
		get
		{
			return base.TextBox.PasswordChar;
		}
		set
		{
			base.TextBox.PasswordChar = value;
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	public bool IsEditable
	{
		get
		{
			return !((TextBoxBase)base.TextBox).ReadOnly;
		}
		set
		{
			((TextBoxBase)base.TextBox).ReadOnly = !value;
			((Control)base.TextBox).Enabled = value;
		}
	}

	[Category("Behavior")]
	public event EventHandler DropDown;

	[Category("Behavior")]
	public event EventHandler DropUp;

	public event EventHandler SelectedValueChanged;

	public event EventHandler SelectedIndexChanged;

	public event DrawItemEventHandler DrawItem
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			DrawItemEventHandler val = this.m_DrawItem;
			DrawItemEventHandler val2;
			do
			{
				val2 = val;
				DrawItemEventHandler value2 = (DrawItemEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_DrawItem, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			DrawItemEventHandler val = this.m_DrawItem;
			DrawItemEventHandler val2;
			do
			{
				val2 = val;
				DrawItemEventHandler value2 = (DrawItemEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_DrawItem, value2, val2);
			}
			while (val != val2);
		}
	}

	protected virtual void OnDropDown(EventArgs e)
	{
		this.DropDown?.Invoke(this, e);
	}

	public void InvokeDropDown(EventArgs e)
	{
		OnDropDown(e);
	}

	protected virtual void OnDropUp(EventArgs e)
	{
		this.DropUp?.Invoke(this, e);
	}

	public void InvokeDropUp(EventArgs e)
	{
		OnDropUp(e);
	}

	protected override void OnTextChanged(EventArgs e)
	{
		base.OnTextChanged(e);
		((Control)this).Invalidate();
	}

	protected override void OnPaint(PaintEventArgs p)
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		base.OnPaint(p);
		Graphics graphics = p.Graphics;
		DrawDownButton(graphics);
		if (((Control)base.TextBox).Visible)
		{
			return;
		}
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		Rectangle downButtonRect = GetDownButtonRect();
		clientRectangle.Width -= downButtonRect.Width;
		clientRectangle.Inflate(-2, -2);
		if (this.DrawItem != null)
		{
			this.DrawItem.Invoke((object)this, new DrawItemEventArgs(graphics, ((Control)this).Font, clientRectangle, SelectedIndex, (DrawItemState)(((Control)this).Focused ? 1 : 0)));
			return;
		}
		StringFormat val = new StringFormat();
		try
		{
			val.LineAlignment = (StringAlignment)1;
			val.FormatFlags = (StringFormatFlags)4096;
			val.Trimming = (StringTrimming)3;
			SolidBrush val2 = new SolidBrush(StiUX.InputForeground);
			try
			{
				graphics.DrawString(((Control)this).Text, ((Control)this).Font, (Brush)(object)val2, (RectangleF)clientRectangle, val);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		base.OnMouseDown(e);
		if (!IsMouseOverButton && (int)DropDownStyle != 2)
		{
			return;
		}
		if ((DropDownBoxPopupForm == null || !((Control)DropDownBoxPopupForm).Visible) && !LockPopupInvoke)
		{
			if (DropDownBoxPopupForm == null)
			{
				CreatePopupForm();
			}
			InvokeDropDown(EventArgs.Empty);
			((ListBox)ListBox).SelectedItem = ((Control)this).Text;
			Rectangle rectangle = ((Control)this).RectangleToScreen(((Control)this).ClientRectangle);
			Size itemsSize = DropDownBoxPopupForm.GetItemsSize(MaxDropDownItems);
			int width = Math.Max(((Control)this).Width, (DropDownWidth > 0) ? DropDownWidth : itemsSize.Width);
			int num = ((itemsSize.Height > StiScaleUI.I(300)) ? StiScaleUI.I(300) : itemsSize.Height);
			StiDropDownBoxPopupForm dropDownBoxPopupForm = DropDownBoxPopupForm;
			int left = rectangle.Left;
			int y = rectangle.Top + ((Control)this).Height + StiScaleUI.I1;
			Padding padding = ((Control)DropDownBoxPopupForm).Padding;
			int num2 = num + ((Padding)(ref padding)).Left;
			padding = ((Control)DropDownBoxPopupForm).Padding;
			dropDownBoxPopupForm.SetLocation(left, y, width, num2 + ((Padding)(ref padding)).Right);
			DropDownBoxPopupForm.ShowPopupForm();
			InvokeDropDown(EventArgs.Empty);
		}
		else
		{
			LockPopupInvoke = false;
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);
		((Control)this).Invalidate();
	}

	protected override void OnClick(EventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		base.OnClick(e);
		if ((int)DropDownStyle == 2)
		{
			((Control)this).Focus();
		}
	}

	private void ListBoxItems_SelectedValueChanged(object sender, EventArgs e)
	{
		this.SelectedValueChanged?.Invoke(sender, e);
		((Control)this).Text = ((Control)ListBox).Text;
	}

	private void ListBoxItems_SelectedIndexChanged(object sender, EventArgs e)
	{
		this.SelectedIndexChanged?.Invoke(sender, e);
		((Control)this).Text = ((SelectedItem != null) ? SelectedItem.ToString() : "");
	}

	private void ListBoxItems_DrawItem(object sender, DrawItemEventArgs e)
	{
		DrawItemEventHandler drawItem = this.DrawItem;
		if (drawItem != null)
		{
			drawItem.Invoke(sender, e);
		}
	}

	private void DropDownBoxPopupForm_ClosePopup(object sender, EventArgs e)
	{
		if (DropDownBoxPopupForm != null)
		{
			InvokeDropUp(e);
			((Control)this).Invalidate();
			DestroyPopupForm();
		}
	}

	public void BeginUpdate()
	{
		((ListBox)ListBox).BeginUpdate();
	}

	public void EndUpdate()
	{
		((ListBox)ListBox).EndUpdate();
	}

	protected override void Dispose(bool disposing)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		((Control)this).Dispose(disposing);
		if (ListBox != null)
		{
			((ListBox)ListBox).DrawItem -= new DrawItemEventHandler(ListBoxItems_DrawItem);
			((ListBox)ListBox).SelectedIndexChanged -= ListBoxItems_SelectedIndexChanged;
			((ListControl)ListBox).SelectedValueChanged -= ListBoxItems_SelectedValueChanged;
		}
		DestroyPopupForm();
	}

	private void CreatePopupForm()
	{
		DropDownBoxPopupForm = new StiDropDownBoxPopupForm((Control)(object)this, ListBox);
		DropDownBoxPopupForm.ClosePopup += DropDownBoxPopupForm_ClosePopup;
	}

	private void DestroyPopupForm()
	{
		if (DropDownBoxPopupForm != null)
		{
			DropDownBoxPopupForm.ListBoxItems = null;
			DropDownBoxPopupForm.ClosePopup -= DropDownBoxPopupForm_ClosePopup;
			DropDownBoxPopupForm = null;
		}
	}

	protected override Rectangle GetContentRect()
	{
		Rectangle contentRect = base.GetContentRect();
		contentRect.Width -= DropDownButtonWidth;
		return contentRect;
	}

	protected override void PaintBorder(Graphics g, Rectangle rect, bool isMouseOver)
	{
		StiControlPaint.DrawBorder(g, rect, (isMouseOver | ((Control)this).Focused) && !DroppedDown, ((Control)this).Enabled, flat: true);
	}

	private Color GetButtonBackground()
	{
		if (DroppedDown)
		{
			return StiUX.ButtonBackgroundPressed;
		}
		if (IsMouseOverButton)
		{
			return StiUX.ButtonBackgroundHover;
		}
		return StiUX.InputBackground;
	}

	private Color GetBorderColor()
	{
		if (!((Control)this).Enabled)
		{
			return StiUX.InputBorder;
		}
		if ((IsMouseOver || ((Control)this).Focused) && !DroppedDown)
		{
			return StiUX.Accent;
		}
		return StiUX.InputBorder;
	}

	private void DrawDownButton(Graphics g)
	{
		Rectangle downButtonRect = GetDownButtonRect();
		Color buttonBackground = GetButtonBackground();
		Color borderColor = GetBorderColor();
		StiDrawing.FillRectangle(g, buttonBackground, downButtonRect);
		if (IsMouseOverButton && !DroppedDown)
		{
			StiDrawing.FillRectangle(g, StiUX.Accent.ChangeAlpha(50), downButtonRect);
		}
		if (IsMouseOverButton || DroppedDown)
		{
			downButtonRect.Height--;
			StiDrawing.DrawLine(g, borderColor, downButtonRect.X, downButtonRect.Y, downButtonRect.X, downButtonRect.Bottom);
		}
		if (Image == null)
		{
			DrawDownArrow(g);
			return;
		}
		downButtonRect = GetDownButtonRect();
		Rectangle rectangle = new Rectangle(downButtonRect.X + (downButtonRect.Width - Image.Width) / 2, downButtonRect.Y + (downButtonRect.Height - Image.Height) / 2, Image.Width, Image.Height);
		g.DrawImage(Image, rectangle);
	}

	private void DrawDownArrow(Graphics g)
	{
		Rectangle downButtonRect = GetDownButtonRect();
		StiButtonHelper.DrawDownArrow(g, downButtonRect, ((Control)this).Enabled);
	}

	private Rectangle GetDownButtonRect()
	{
		return new Rectangle(((Control)this).Width - DropDownButtonWidth - 1, 1, DropDownButtonWidth, ((Control)this).Height - 2);
	}

	public void SelectAll()
	{
		((TextBoxBase)base.TextBox).SelectAll();
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		if (FixedHeight)
		{
			height = StiScaleUI.I(21);
		}
		((Control)this).SetBoundsCore(x, y, width, height, specified);
	}

	public StiDropDownBox()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		((ListBox)ListBox).DrawItem += new DrawItemEventHandler(ListBoxItems_DrawItem);
		((ListBox)ListBox).SelectedIndexChanged += ListBoxItems_SelectedIndexChanged;
		((ListControl)ListBox).SelectedValueChanged += ListBoxItems_SelectedValueChanged;
	}
}
