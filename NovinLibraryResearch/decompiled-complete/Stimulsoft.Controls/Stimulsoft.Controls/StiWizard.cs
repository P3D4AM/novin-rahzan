using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using Stimulsoft.Base;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[Designer(typeof(StiWizardDesigner))]
public class StiWizard : ContainerControl
{
	internal StringFormat sfHeaderTitle;

	internal StringFormat sfHeaderDescription;

	private int step = StiScaleUI.XXI(14.0);

	private StiWizardPage selectedPage;

	private bool drawHeaderBorder;

	private bool fillAcceptCancelProps = true;

	private bool showProgressTree = true;

	private bool progressTreeOneLevel;

	private bool markLastPage = true;

	private int treeProgressWidth = 200;

	private int headerHeight = 66;

	private int dockPaddingFromLeft;

	private int dockPaddingFromRight;

	private int dockPaddingFromBottom;

	private bool showProgressLine = true;

	private bool showHeader = true;

	private bool showHeaderText = true;

	private bool showHeaderImageAtRight = true;

	private bool headerOnAll = true;

	private bool drawPageBorders = true;

	private Font headerTitleFont;

	private Font headerDescriptionFont;

	private Font treeProgressFont;

	private Color headerColor = Color.White;

	private Color headerTextColor = Color.Empty;

	private Color contentColor = Color.White;

	private Image headerBackgroundImage;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DockPaddingEdges DockPadding => ((ScrollableControl)this).DockPadding;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiWizardPageCollection Pages { get; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Button BackButton { get; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Button NextButton { get; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Button CancelButton { get; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Button HelpButton { get; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Button FinishButton { get; }

	[Browsable(false)]
	internal bool IsDesignMode => ((Component)this).DesignMode;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color BackColor
	{
		get
		{
			return ((Control)this).BackColor;
		}
		set
		{
			((Control)this).BackColor = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiWizardPage SelectedPage
	{
		get
		{
			return selectedPage;
		}
		set
		{
			foreach (StiWizardPage page in Pages)
			{
				if (page == value)
				{
					page.Dock = (DockStyle)5;
					((Control)page).Show();
				}
			}
			foreach (StiWizardPage page2 in Pages)
			{
				if (page2 != value)
				{
					((Control)page2).Hide();
				}
			}
			selectedPage = value;
			SetLayout();
			((Control)this).Invalidate();
			InvokeSelectedIndexChanged(this, EventArgs.Empty);
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int SelectedIndex
	{
		get
		{
			return Pages.IndexOf(SelectedPage);
		}
		set
		{
			if (value >= 0 && value < Pages.Count)
			{
				SelectedPage = Pages[value];
			}
		}
	}

	[DefaultValue("< &Back")]
	public string BackText
	{
		get
		{
			return ((Control)BackButton).Text;
		}
		set
		{
			((Control)BackButton).Text = value;
		}
	}

	[DefaultValue("&Next >")]
	public string NextText
	{
		get
		{
			return ((Control)NextButton).Text;
		}
		set
		{
			((Control)NextButton).Text = value;
		}
	}

	[DefaultValue("&Help")]
	public string HelpText
	{
		get
		{
			return ((Control)HelpButton).Text;
		}
		set
		{
			((Control)HelpButton).Text = value;
		}
	}

	[DefaultValue("Cancel")]
	public string CancelText
	{
		get
		{
			return ((Control)CancelButton).Text;
		}
		set
		{
			((Control)CancelButton).Text = value;
		}
	}

	[DefaultValue("&Finish")]
	public string FinishText
	{
		get
		{
			return ((Control)FinishButton).Text;
		}
		set
		{
			((Control)FinishButton).Text = value;
		}
	}

	public bool HelpButtonVisible
	{
		get
		{
			return ((Control)HelpButton).Visible;
		}
		set
		{
			((Control)HelpButton).Visible = value;
		}
	}

	[DefaultValue(false)]
	[Category("Appearance")]
	public bool AllowPageSwitch { get; set; }

	[DefaultValue(false)]
	[Category("Appearance")]
	public bool DrawHeaderBorder
	{
		get
		{
			return drawHeaderBorder;
		}
		set
		{
			drawHeaderBorder = value;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(true)]
	public bool FillAcceptCancelProps
	{
		get
		{
			return fillAcceptCancelProps;
		}
		set
		{
			if (fillAcceptCancelProps != value)
			{
				fillAcceptCancelProps = value;
				SetLayout();
			}
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	public bool ShowProgressTree
	{
		get
		{
			return showProgressTree;
		}
		set
		{
			showProgressTree = value;
			SetLayout();
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(false)]
	[Category("Appearance")]
	public bool ProgressTreeOneLevel
	{
		get
		{
			return progressTreeOneLevel;
		}
		set
		{
			progressTreeOneLevel = value;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	public bool MarkLastPage
	{
		get
		{
			return markLastPage;
		}
		set
		{
			markLastPage = value;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(200)]
	[Category("Appearance")]
	public int TreeProgressWidth
	{
		get
		{
			return treeProgressWidth;
		}
		set
		{
			treeProgressWidth = value;
			SetLayout();
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(66)]
	[Category("Appearance")]
	public int HeaderHeight
	{
		get
		{
			return headerHeight;
		}
		set
		{
			headerHeight = value;
			SetLayout();
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(0)]
	[Category("Appearance")]
	public int DockPaddingFromLeft
	{
		get
		{
			return dockPaddingFromLeft;
		}
		set
		{
			dockPaddingFromLeft = value;
			SetLayout();
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(0)]
	[Category("Appearance")]
	public int DockPaddingFromRight
	{
		get
		{
			return dockPaddingFromRight;
		}
		set
		{
			dockPaddingFromRight = value;
			SetLayout();
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(0)]
	[Category("Appearance")]
	public int DockPaddingFromBottom
	{
		get
		{
			return dockPaddingFromBottom;
		}
		set
		{
			dockPaddingFromBottom = value;
			SetLayout();
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	public bool ShowProgressLine
	{
		get
		{
			return showProgressLine;
		}
		set
		{
			showProgressLine = value;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	public bool ShowHeader
	{
		get
		{
			return showHeader;
		}
		set
		{
			showHeader = value;
			SetLayout();
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	public bool ShowHeaderText
	{
		get
		{
			return showHeaderText;
		}
		set
		{
			showHeaderText = value;
			SetLayout();
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	public bool ShowHeaderImageAtRight
	{
		get
		{
			return showHeaderImageAtRight;
		}
		set
		{
			showHeaderImageAtRight = value;
			SetLayout();
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	public bool HeaderOnAll
	{
		get
		{
			return headerOnAll;
		}
		set
		{
			headerOnAll = value;
			SetLayout();
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	public bool DrawPageBorders
	{
		get
		{
			return drawPageBorders;
		}
		set
		{
			drawPageBorders = value;
			SetLayout();
			((Control)this).Invalidate();
		}
	}

	public Font HeaderTitleFont
	{
		get
		{
			return headerTitleFont;
		}
		set
		{
			headerTitleFont = value;
			((Control)this).Invalidate();
		}
	}

	public Font HeaderDescriptionFont
	{
		get
		{
			return headerDescriptionFont;
		}
		set
		{
			headerDescriptionFont = value;
			((Control)this).Invalidate();
		}
	}

	public Font TreeProgressFont
	{
		get
		{
			return treeProgressFont;
		}
		set
		{
			treeProgressFont = value;
			((Control)this).Invalidate();
		}
	}

	[Category("Appearance")]
	public Color HeaderColor
	{
		get
		{
			return headerColor;
		}
		set
		{
			headerColor = value;
			((Control)this).Invalidate();
		}
	}

	[Category("Appearance")]
	public Color HeaderTextColor
	{
		get
		{
			return headerTextColor;
		}
		set
		{
			headerTextColor = value;
			((Control)this).Invalidate();
		}
	}

	[Category("Appearance")]
	public Color ContentColor
	{
		get
		{
			return contentColor;
		}
		set
		{
			contentColor = value;
			((Control)this).Invalidate();
		}
	}

	[Category("Appearance")]
	public Image HeaderBackgroundImage
	{
		get
		{
			return headerBackgroundImage;
		}
		set
		{
			headerBackgroundImage = value;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(StiControlStyle.Flat)]
	public StiControlStyle ControlStyle { get; set; }

	[Category("Behavior")]
	public event EventHandler TitleClick;

	[Category("Behavior")]
	public event EventHandler SelectedIndexChanged;

	public event EventHandler CancelClick;

	public event EventHandler BackClick;

	public event EventHandler NextClick;

	public event StiBeforePageChangedHandler BeforeBackClick;

	public event StiBeforePageChangedHandler BeforeNextClick;

	public event StiBeforeFinishHandler BeforeFinishClick;

	public event EventHandler FinishClick;

	public event EventHandler HelpClick;

	public event StiCanNextHandler CanNext;

	public event StiCanFinishHandler CanFinish;

	protected override void OnControlAdded(ControlEventArgs e)
	{
		((Control)this).OnControlAdded(e);
		if (e.Control is StiWizardPage)
		{
			e.Control.Dock = (DockStyle)5;
			e.Control.CreateGraphics();
			if (Pages.Count == 1)
			{
				SelectedPage = Pages[0];
			}
			Pages.LockActions = true;
			Pages.Add(e.Control as StiWizardPage);
			Pages.LockActions = false;
			((Control)this).Invalidate();
		}
	}

	protected override void OnControlRemoved(ControlEventArgs e)
	{
		((Control)this).OnControlRemoved(e);
		if (!(e.Control is StiWizardPage))
		{
			return;
		}
		Pages.LockActions = true;
		Pages.Remove(e.Control as StiWizardPage);
		Pages.LockActions = false;
		try
		{
			foreach (StiWizardPage page in Pages)
			{
				if (page == SelectedPage)
				{
					return;
				}
			}
			if (Pages.Count > 0)
			{
				SelectedPage = Pages[0];
			}
			else
			{
				SelectedPage = null;
			}
		}
		finally
		{
			((Control)this).Invalidate();
		}
	}

	internal Rectangle GetBackButtonRect()
	{
		Rectangle controlRect = GetControlRect();
		controlRect.Y += StiScaleUI.YYI(12.0);
		controlRect.Height = StiScaleUI.YYI(24.0);
		controlRect.Location = new Point(GetNextButtonRect().X - StiScaleUI.XXI(77.0), controlRect.Y);
		controlRect.Size = new Size(StiScaleUI.XXI(73.0), StiScaleUI.YYI(24.0));
		return controlRect;
	}

	internal Rectangle GetNextButtonRect()
	{
		Rectangle controlRect = GetControlRect();
		controlRect.Y += StiScaleUI.YYI(12.0);
		controlRect.Height = StiScaleUI.YYI(24.0);
		controlRect.Location = new Point(GetFinishButtonRect().X - StiScaleUI.XXI(77.0), controlRect.Y);
		controlRect.Size = new Size(StiScaleUI.XXI(73.0), StiScaleUI.YYI(24.0));
		return controlRect;
	}

	internal Rectangle GetFinishButtonRect()
	{
		Rectangle controlRect = GetControlRect();
		controlRect.Y += StiScaleUI.YYI(12.0);
		controlRect.Height = StiScaleUI.YYI(24.0);
		controlRect.Location = new Point(GetCancelButtonRect().X - StiScaleUI.XXI(85.0), controlRect.Y);
		controlRect.Size = new Size(StiScaleUI.XXI(73.0), StiScaleUI.YYI(24.0));
		return controlRect;
	}

	internal Rectangle GetCancelButtonRect()
	{
		Rectangle controlRect = GetControlRect();
		controlRect.Y += StiScaleUI.YYI(12.0);
		controlRect.Height = StiScaleUI.YYI(24.0);
		controlRect.Location = new Point(controlRect.Right - StiScaleUI.XXI(85.0), controlRect.Y);
		controlRect.Size = new Size(StiScaleUI.XXI(73.0), StiScaleUI.YYI(24.0));
		return controlRect;
	}

	internal Rectangle GetHelpButtonRect()
	{
		Rectangle controlRect = GetControlRect();
		controlRect.Y += StiScaleUI.YYI(12.0);
		controlRect.Height = StiScaleUI.YYI(24.0);
		controlRect.Location = new Point(controlRect.X + StiScaleUI.XXI(14.0), controlRect.Y);
		controlRect.Size = new Size(StiScaleUI.XXI(73.0), StiScaleUI.YYI(24.0));
		return controlRect;
	}

	internal StiWizardPage GetWizardPageAtPoint(Point pos)
	{
		for (int i = 0; i < Pages.Count; i++)
		{
			if (GetBoxRect(i).Contains(pos))
			{
				return Pages[i];
			}
			if (GetBoxTextRect(i).Contains(pos))
			{
				return Pages[i];
			}
		}
		return null;
	}

	private Rectangle GetTreeProgressRect()
	{
		if (ShowProgressTree)
		{
			return new Rectangle(0, (HeaderOnAll && ShowHeader) ? StiScaleUI.YYI(HeaderHeight) : 0, StiScaleUI.XXI(200.0), ((Control)this).Height - StiScaleUI.YYI(50.0) - ((HeaderOnAll && ShowHeader) ? StiScaleUI.YYI(HeaderHeight) : 0));
		}
		return Rectangle.Empty;
	}

	private Rectangle GetHeaderRect()
	{
		if (!ShowHeader)
		{
			return Rectangle.Empty;
		}
		int num = ((!HeaderOnAll && ShowProgressTree) ? StiScaleUI.XXI(TreeProgressWidth) : 0);
		Rectangle result = new Rectangle(num, 0, ((Control)this).Width - num, HeaderHeight);
		if (DrawHeaderBorder)
		{
			result.Inflate(-2, 0);
			result.Y += 2;
			result.Height -= 2;
		}
		return result;
	}

	private Rectangle GetHeaderTitleRect()
	{
		if (!ShowHeaderText)
		{
			return Rectangle.Empty;
		}
		Rectangle headerRect = GetHeaderRect();
		Rectangle headerImageRect = GetHeaderImageRect();
		if (ShowHeaderImageAtRight)
		{
			return new Rectangle(headerRect.X + StiScaleUI.XXI(10.0), headerRect.Y + StiScaleUI.YYI(10.0), ((Control)this).Width - StiScaleUI.XXI(20.0) - headerImageRect.Width, StiScaleUI.YYI(20.0));
		}
		return new Rectangle(headerImageRect.Right + StiScaleUI.XXI(10.0), headerRect.Y + StiScaleUI.YYI(10.0), ((Control)this).Width - StiScaleUI.XXI(10.0), StiScaleUI.YYI(20.0));
	}

	private Rectangle GetHeaderDescriptionRect()
	{
		Rectangle rectangle = GetHeaderTitleRect();
		if (rectangle.IsEmpty)
		{
			rectangle = GetHeaderRect();
		}
		Rectangle headerImageRect = GetHeaderImageRect();
		if (ShowHeaderText)
		{
			if (ShowHeaderImageAtRight)
			{
				return new Rectangle(rectangle.X + StiScaleUI.XXI(10.0), rectangle.Y + StiScaleUI.YYI(20.0), rectangle.Width - StiScaleUI.XXI(20.0) - headerImageRect.Width, rectangle.Height - rectangle.Y + StiScaleUI.YYI(20.0));
			}
			return new Rectangle(headerImageRect.Right + StiScaleUI.XXI(10.0), rectangle.Y + StiScaleUI.YYI(20.0), rectangle.Width - StiScaleUI.XXI(10.0), rectangle.Height - rectangle.Y + StiScaleUI.YYI(20.0));
		}
		if (ShowHeaderImageAtRight)
		{
			return new Rectangle(rectangle.X + StiScaleUI.XXI(10.0), rectangle.Y, rectangle.Width - StiScaleUI.XXI(20.0) - headerImageRect.Width, rectangle.Height);
		}
		return new Rectangle(headerImageRect.Right + StiScaleUI.XXI(10.0), rectangle.Y, rectangle.Width - StiScaleUI.XXI(10.0), rectangle.Height);
	}

	private Rectangle GetHeaderImageRect()
	{
		if (SelectedPage.Image == null)
		{
			return Rectangle.Empty;
		}
		Rectangle headerRect = GetHeaderRect();
		if (ShowHeaderImageAtRight)
		{
			headerRect.X = ((Control)this).Width - SelectedPage.Image.Width - StiScaleUI.XXI(10.0);
		}
		else
		{
			headerRect.X = StiScaleUI.XXI(10.0);
		}
		headerRect.Y = (headerRect.Height - SelectedPage.Image.Height) / 2;
		headerRect.Width = SelectedPage.Image.Width;
		headerRect.Height = SelectedPage.Image.Height;
		return headerRect;
	}

	private Rectangle GetControlRect()
	{
		return new Rectangle(0, ((Control)this).Height - StiScaleUI.YYI(50.0), ((Control)this).Width, StiScaleUI.YYI(50.0));
	}

	private Rectangle GetBoxTextRect(int index)
	{
		Rectangle boxRect = GetBoxRect(index);
		boxRect.X += step + 2;
		boxRect.Y -= step >> 1;
		boxRect.Width = StiScaleUI.XXI(200.0) - boxRect.X - StiScaleUI.XXI(5.0);
		boxRect.Height += step;
		if ((index == 0 || Pages.Count - 1 == index) && !ProgressTreeOneLevel)
		{
			boxRect.X += StiScaleUI.XXI(6.0);
		}
		return boxRect;
	}

	internal Rectangle GetBoxRect(int index)
	{
		Rectangle treeProgressRect = GetTreeProgressRect();
		Rectangle result = new Rectangle(StiScaleUI.XXI(8.0), treeProgressRect.Top + StiScaleUI.YYI(16.0) + index * step * 2, step, step);
		if (!ProgressTreeOneLevel && index > 0 && index < Pages.Count - 1)
		{
			result.X += step;
		}
		return result;
	}

	private void DrawBox(Rectangle fullRect, Graphics g, int index)
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		Rectangle boxTextRect = GetBoxTextRect(index);
		boxTextRect.X -= StiScaleUI.XXI(16.0);
		boxTextRect.Y += StiScaleUI.I1;
		Brush val;
		if (index == SelectedIndex)
		{
			val = Brushes.White;
			Rectangle rectangle = new Rectangle(fullRect.X, boxTextRect.Top, fullRect.Width, boxTextRect.Height);
			SolidBrush val2 = new SolidBrush(Color.FromArgb(23, 97, 197));
			try
			{
				g.FillRectangle((Brush)(object)val2, rectangle);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		else
		{
			val = Brushes.Black;
		}
		Font val3 = new Font(TreeProgressFont.FontFamily.Name, TreeProgressFont.Size);
		try
		{
			StringFormat val4 = new StringFormat();
			try
			{
				val4.LineAlignment = (StringAlignment)1;
				val4.Alignment = (StringAlignment)0;
				string text = ((Control)Pages[index]).Text;
				g.DrawString(text, val3, val, (RectangleF)boxTextRect, val4);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
	}

	private void SetLayout()
	{
		DockPadding.Top = (ShowHeader ? StiScaleUI.YYI(HeaderHeight) : 0);
		DockPadding.Left = (ShowProgressTree ? StiScaleUI.XXI(TreeProgressWidth) : StiScaleUI.XXI(DockPaddingFromLeft));
		DockPadding.Right = StiScaleUI.XXI(DockPaddingFromRight);
		DockPadding.Bottom = StiScaleUI.YYI(46 + DockPaddingFromBottom);
		((Control)BackButton).Bounds = GetBackButtonRect();
		((Control)NextButton).Bounds = GetNextButtonRect();
		((Control)CancelButton).Bounds = GetCancelButtonRect();
		((Control)FinishButton).Bounds = GetFinishButtonRect();
		((Control)HelpButton).Bounds = GetHelpButtonRect();
		UpdateWizardState();
		if (FillAcceptCancelProps)
		{
			Form val = ((Control)this).FindForm();
			if (val != null)
			{
				val.AcceptButton = (IButtonControl)(object)(((Control)NextButton).Visible ? NextButton : FinishButton);
				val.CancelButton = (IButtonControl)(object)CancelButton;
			}
		}
	}

	public void UpdateWizardState()
	{
		((Control)BackButton).Enabled = SelectedIndex != 0;
		if (SelectedIndex == Pages.Count - 1)
		{
			((Control)NextButton).Enabled = false;
			((Control)FinishButton).Enabled = true;
			((Control)FinishButton).Focus();
			return;
		}
		StiCanNextEventArgs stiCanNextEventArgs = new StiCanNextEventArgs();
		InvokeOnCanNext(stiCanNextEventArgs);
		((Control)NextButton).Enabled = stiCanNextEventArgs.CanNext;
		StiCanFinishEventArgs stiCanFinishEventArgs = new StiCanFinishEventArgs();
		InvokeOnCanFinish(stiCanFinishEventArgs);
		((Control)FinishButton).Enabled = stiCanFinishEventArgs.CanFinish;
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
		if (AllowPageSwitch && ShowProgressTree)
		{
			Point pos = ((Control)this).PointToClient(Cursor.Position);
			StiWizardPage wizardPageAtPoint = GetWizardPageAtPoint(pos);
			if (wizardPageAtPoint != null)
			{
				SelectedPage = wizardPageAtPoint;
			}
		}
	}

	protected override void OnPaint(PaintEventArgs p)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Expected O, but got Unknown
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Expected O, but got Unknown
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Expected O, but got Unknown
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_046f: Expected O, but got Unknown
		//IL_0526: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Expected O, but got Unknown
		Graphics graphics = p.Graphics;
		Rectangle controlRect = GetControlRect();
		if (!ContentColor.IsEmpty)
		{
			SolidBrush val = new SolidBrush(ContentColor);
			try
			{
				graphics.FillRectangle((Brush)(object)val, new Rectangle(0, 0, ((Control)this).Width, controlRect.Y));
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (ShowProgressTree)
		{
			Rectangle treeProgressRect = GetTreeProgressRect();
			graphics.SetClip(treeProgressRect);
			SolidBrush val2 = new SolidBrush(Color.White);
			try
			{
				graphics.FillRectangle((Brush)(object)val2, treeProgressRect);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			if (Pages.Count > 1)
			{
				Rectangle boxRect = GetBoxRect(0);
				Rectangle boxRect2 = GetBoxRect(Pages.Count - 1);
				if (Pages.Count > 2)
				{
					Rectangle rectangle = new Rectangle(boxRect.X + (step >> 1), boxRect.Y + (step >> 1), step, boxRect2.Bottom - boxRect.Bottom);
					if (ProgressTreeOneLevel)
					{
						if (ShowProgressLine)
						{
							graphics.DrawLine(Pens.White, rectangle.X, rectangle.Y, rectangle.X, rectangle.Bottom);
						}
					}
					else if (ShowProgressLine)
					{
						graphics.DrawRectangle(Pens.White, rectangle);
					}
				}
				else if (ShowProgressLine)
				{
					graphics.DrawLine(Pens.White, boxRect.X + (step >> 1), boxRect.Y + (step >> 1), boxRect2.X + (step >> 1), boxRect2.Y + (step >> 1));
				}
			}
			if (ControlStyle == StiControlStyle.Office2013Blue)
			{
				Pen val3 = new Pen(Color.FromArgb(198, 198, 198));
				val3.DashPattern = new float[2] { 1f, 2f };
				Pen val4 = val3;
				try
				{
					graphics.DrawLine(val4, treeProgressRect.Right - 1, treeProgressRect.Y, treeProgressRect.Right - 1, treeProgressRect.Bottom);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			else
			{
				graphics.DrawLine(SystemPens.ControlDark, treeProgressRect.Right - 1, treeProgressRect.Y, treeProgressRect.Right - 1, treeProgressRect.Bottom);
			}
			treeProgressRect.X += StiScaleUI.I1;
			treeProgressRect.Width -= StiScaleUI.I3;
			for (int i = 0; i < Pages.Count; i++)
			{
				DrawBox(treeProgressRect, p.Graphics, i);
			}
			graphics.ResetClip();
		}
		if (ControlStyle == StiControlStyle.Office2013Blue)
		{
			Pen val3 = new Pen(Color.FromArgb(198, 198, 198));
			val3.DashPattern = new float[2] { 1f, 2f };
			Pen val5 = val3;
			try
			{
				graphics.DrawLine(val5, controlRect.X, controlRect.Y, controlRect.Right, controlRect.Y);
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
		}
		else
		{
			graphics.DrawLine(SystemPens.ControlDark, controlRect.X, controlRect.Y, controlRect.Right, controlRect.Y);
			graphics.DrawLine(SystemPens.ControlLightLight, controlRect.X, controlRect.Y + 1, controlRect.Right, controlRect.Y + 1);
		}
		if (!ShowHeader || SelectedPage == null)
		{
			return;
		}
		Rectangle headerRect = GetHeaderRect();
		if (HeaderBackgroundImage != null)
		{
			graphics.DrawImage(HeaderBackgroundImage, headerRect);
		}
		else
		{
			SolidBrush val6 = new SolidBrush(HeaderColor);
			try
			{
				graphics.FillRectangle((Brush)(object)val6, headerRect);
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
		}
		if (DrawHeaderBorder)
		{
			Rectangle rectangle2 = headerRect;
			rectangle2.Width--;
			rectangle2.Height--;
			graphics.DrawRectangle(SystemPens.ControlDark, rectangle2);
		}
		if (ShowHeaderText)
		{
			graphics.DrawString(((Control)SelectedPage).Text, HeaderTitleFont, SystemBrushes.ControlText, (RectangleF)GetHeaderTitleRect(), sfHeaderTitle);
		}
		graphics.SetClip(GetHeaderDescriptionRect());
		if (!HeaderTextColor.IsEmpty)
		{
			SolidBrush val7 = new SolidBrush(HeaderTextColor);
			try
			{
				graphics.DrawString(SelectedPage.Description, HeaderDescriptionFont, (Brush)(object)val7, (RectangleF)GetHeaderDescriptionRect(), sfHeaderDescription);
			}
			finally
			{
				((IDisposable)val7)?.Dispose();
			}
		}
		else
		{
			graphics.DrawString(SelectedPage.Description, HeaderDescriptionFont, SystemBrushes.ControlText, (RectangleF)GetHeaderDescriptionRect(), sfHeaderDescription);
		}
		graphics.ResetClip();
		if (SelectedPage.Image != null)
		{
			graphics.DrawImage(SelectedPage.Image, GetHeaderImageRect());
		}
		if (!DrawPageBorders)
		{
			return;
		}
		if (ControlStyle == StiControlStyle.Office2013Blue)
		{
			Pen val3 = new Pen(Color.FromArgb(198, 198, 198));
			val3.DashPattern = new float[2] { 1f, 2f };
			Pen val8 = val3;
			try
			{
				graphics.DrawLine(val8, headerRect.X, headerRect.Bottom - 1, headerRect.Right, headerRect.Bottom - 1);
				return;
			}
			finally
			{
				((IDisposable)val8)?.Dispose();
			}
		}
		graphics.DrawLine(SystemPens.ControlLightLight, headerRect.X, headerRect.Bottom - 1, headerRect.Right, headerRect.Bottom - 1);
		graphics.DrawLine(SystemPens.ControlDark, headerRect.X, headerRect.Bottom - 2, headerRect.Right, headerRect.Bottom - 2);
	}

	protected override void OnLayout(LayoutEventArgs e)
	{
		((ContainerControl)this).OnLayout(e);
		SetLayout();
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		((ScrollableControl)this).OnVisibleChanged(e);
		SetLayout();
	}

	protected override void OnResize(EventArgs e)
	{
		((Control)this).OnResize(e);
		SetLayout();
	}

	protected virtual void OnTitleClick(EventArgs e)
	{
	}

	public virtual void InvokeTitleClick(object sender, EventArgs e)
	{
		try
		{
			OnTitleClick(EventArgs.Empty);
			this.TitleClick?.Invoke(null, EventArgs.Empty);
		}
		catch
		{
		}
	}

	protected virtual void OnSelectedIndexChanged(EventArgs e)
	{
	}

	public virtual void InvokeSelectedIndexChanged(object sender, EventArgs e)
	{
		OnSelectedIndexChanged(EventArgs.Empty);
		this.SelectedIndexChanged?.Invoke(sender, e);
	}

	public void InvokeOnCancelClick(EventArgs e)
	{
		OnCancelClick(e);
	}

	protected void OnCancelClick(EventArgs e)
	{
		OnCancelClick(this, e);
	}

	private void OnCancelClick(object sender, EventArgs e)
	{
		this.CancelClick?.Invoke(sender, e);
	}

	public void InvokeOnBackClick(EventArgs e)
	{
		OnBackClick(e);
	}

	protected void OnBackClick(EventArgs e)
	{
		OnBackClick(this, e);
	}

	private void OnBackClick(object sender, EventArgs e)
	{
		if (SelectedIndex > 0)
		{
			StiBeforePageChangedEventArgs stiBeforePageChangedEventArgs = new StiBeforePageChangedEventArgs(Pages[SelectedIndex], Pages[SelectedIndex - 1]);
			InvokeOnBeforeBackClick(stiBeforePageChangedEventArgs);
			if (!stiBeforePageChangedEventArgs.Cancel)
			{
				SelectedIndex--;
				UpdateWizardState();
				this.BackClick?.Invoke(sender, e);
			}
		}
	}

	public void InvokeOnNextClick(EventArgs e)
	{
		OnNextClick(e);
	}

	protected void OnNextClick(EventArgs e)
	{
		OnNextClick(this, e);
	}

	private void OnNextClick(object sender, EventArgs e)
	{
		if (SelectedIndex < Pages.Count - 1)
		{
			StiBeforePageChangedEventArgs stiBeforePageChangedEventArgs = new StiBeforePageChangedEventArgs(Pages[SelectedIndex], Pages[SelectedIndex + 1]);
			InvokeOnBeforeNextClick(stiBeforePageChangedEventArgs);
			if (!stiBeforePageChangedEventArgs.Cancel)
			{
				SelectedIndex++;
				UpdateWizardState();
				this.NextClick?.Invoke(sender, e);
			}
		}
	}

	public void InvokeOnBeforeBackClick(StiBeforePageChangedEventArgs e)
	{
		OnBeforeBackClick(e);
	}

	protected void OnBeforeBackClick(StiBeforePageChangedEventArgs e)
	{
		OnBeforeBackClick(this, e);
	}

	private void OnBeforeBackClick(object sender, StiBeforePageChangedEventArgs e)
	{
		this.BeforeBackClick?.Invoke(sender, e);
	}

	public void InvokeOnBeforeNextClick(StiBeforePageChangedEventArgs e)
	{
		OnBeforeNextClick(e);
	}

	protected void OnBeforeNextClick(StiBeforePageChangedEventArgs e)
	{
		OnBeforeNextClick(this, e);
	}

	private void OnBeforeNextClick(object sender, StiBeforePageChangedEventArgs e)
	{
		this.BeforeNextClick?.Invoke(sender, e);
	}

	public void InvokeOnBeforeFinishClick(StiBeforeFinishEventArgs e)
	{
		OnBeforeFinishClick(e);
	}

	protected void OnBeforeFinishClick(StiBeforeFinishEventArgs e)
	{
		OnBeforeFinishClick(this, e);
	}

	private void OnBeforeFinishClick(object sender, StiBeforeFinishEventArgs e)
	{
		this.BeforeFinishClick?.Invoke(sender, e);
	}

	public void InvokeOnFinishClick(EventArgs e)
	{
		OnFinishClick(e);
	}

	protected void OnFinishClick(EventArgs e)
	{
		StiBeforeFinishEventArgs stiBeforeFinishEventArgs = new StiBeforeFinishEventArgs();
		InvokeOnBeforeFinishClick(stiBeforeFinishEventArgs);
		if (!stiBeforeFinishEventArgs.Cancel)
		{
			OnFinishClick(this, e);
		}
	}

	private void OnFinishClick(object sender, EventArgs e)
	{
		this.FinishClick?.Invoke(sender, e);
	}

	public void InvokeOnHelpClick(EventArgs e)
	{
		OnHelpClick(e);
	}

	private void OnHelpClick(object sender, EventArgs e)
	{
		this.HelpClick?.Invoke(sender, e);
	}

	protected void OnHelpClick(EventArgs e)
	{
		OnHelpClick(this, e);
	}

	public void InvokeOnCanNext(StiCanNextEventArgs e)
	{
		OnCanNext(e);
	}

	private void OnCanNext(object sender, StiCanNextEventArgs e)
	{
		this.CanNext?.Invoke(sender, e);
	}

	protected void OnCanNext(StiCanNextEventArgs e)
	{
		OnCanNext(this, e);
	}

	public void InvokeOnCanFinish(StiCanFinishEventArgs e)
	{
		OnCanFinish(e);
	}

	private void OnCanFinish(object sender, StiCanFinishEventArgs e)
	{
		this.CanFinish?.Invoke(sender, e);
	}

	protected void OnCanFinish(StiCanFinishEventArgs e)
	{
		OnCanFinish(this, e);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			sfHeaderTitle.Dispose();
			sfHeaderDescription.Dispose();
			headerTitleFont.Dispose();
			headerDescriptionFont.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	public StiWizard()
	{
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Expected O, but got Unknown
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Invalid comparison between Unknown and I4
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Expected O, but got Unknown
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Invalid comparison between Unknown and I4
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Expected O, but got Unknown
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Expected O, but got Unknown
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Expected O, but got Unknown
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		Pages = new StiWizardPageCollection(this);
		NextButton = new Button();
		((ButtonBase)NextButton).UseVisualStyleBackColor = true;
		((Control)NextButton).TabIndex = 1000;
		((Control)NextButton).Click += OnNextClick;
		FinishButton = new Button();
		((ButtonBase)FinishButton).UseVisualStyleBackColor = true;
		FinishButton.DialogResult = (DialogResult)1;
		((Control)FinishButton).TabIndex = 1001;
		((Control)FinishButton).Click += OnFinishClick;
		CancelButton = new Button();
		((ButtonBase)CancelButton).UseVisualStyleBackColor = true;
		CancelButton.DialogResult = (DialogResult)2;
		((Control)CancelButton).TabIndex = 1002;
		((Control)CancelButton).Click += OnCancelClick;
		HelpButton = new Button();
		((ButtonBase)HelpButton).UseVisualStyleBackColor = true;
		((Control)HelpButton).TabIndex = 1003;
		((Control)HelpButton).Click += OnHelpClick;
		BackButton = new Button();
		((ButtonBase)BackButton).UseVisualStyleBackColor = true;
		((Control)BackButton).TabIndex = 1004;
		((Control)BackButton).Enabled = false;
		((Control)BackButton).Click += OnBackClick;
		((Control)this).Controls.Add((Control)(object)BackButton);
		((Control)this).Controls.Add((Control)(object)HelpButton);
		((Control)this).Controls.Add((Control)(object)CancelButton);
		((Control)this).Controls.Add((Control)(object)FinishButton);
		((Control)this).Controls.Add((Control)(object)NextButton);
		BackText = "< &Back";
		NextText = "&Next >";
		CancelText = "Cancel";
		FinishText = "&Finish";
		HelpText = "&Help";
		sfHeaderTitle = new StringFormat
		{
			HotkeyPrefix = (HotkeyPrefix)1,
			LineAlignment = (StringAlignment)0,
			Alignment = (StringAlignment)0,
			FormatFlags = (StringFormatFlags)4096,
			Trimming = (StringTrimming)3
		};
		if ((int)((Control)this).RightToLeft == 1)
		{
			StringFormat obj = sfHeaderTitle;
			obj.FormatFlags = (StringFormatFlags)(obj.FormatFlags | 1);
		}
		sfHeaderDescription = new StringFormat
		{
			HotkeyPrefix = (HotkeyPrefix)1,
			LineAlignment = (StringAlignment)1,
			Alignment = (StringAlignment)0,
			Trimming = (StringTrimming)3
		};
		if ((int)((Control)this).RightToLeft == 1)
		{
			StringFormat obj2 = sfHeaderDescription;
			obj2.FormatFlags = (StringFormatFlags)(obj2.FormatFlags | 1);
		}
		headerTitleFont = new Font(((Control)this).Font.FontFamily.Name, 10f, (FontStyle)1);
		headerDescriptionFont = new Font(((Control)this).Font.FontFamily.Name, 8f);
		treeProgressFont = new Font(((Control)this).Font.FontFamily.Name, 8f);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Control)this).BackColor = Color.White;
		SetLayout();
	}
}
