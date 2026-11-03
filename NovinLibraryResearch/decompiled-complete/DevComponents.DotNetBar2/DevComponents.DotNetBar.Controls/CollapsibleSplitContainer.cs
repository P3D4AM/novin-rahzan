using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Controls;

[ToolboxBitmap(typeof(CollapsibleSplitContainer), "Controls.CollapsibleSplitContainer.ico")]
public class CollapsibleSplitContainer : SplitContainer
{
	private const int _2AB7 = 2;

	private Rectangle __2AB8 = Rectangle.Empty;

	private Rectangle __2AB9 = Rectangle.Empty;

	private eButtonState __2ABA;

	private eButtonState __2ABB;

	private Orientation __2ABC = (Orientation)1;

	private Cursor __2ABD;

	private PropertyInfo __2ABE;

	private CancelEventHandler _2ABF;

	private EventHandler _2AC0;

	private int __2AC1;

	private bool __2AC2;

	private bool __2AC3;

	private bool __2AC4;

	private CancelEventHandler _2AC5;

	private EventHandler _2AC6;

	private eSplitterButtonPosition __2AC7;

	private eCollapseMode __2AC8;

	[Description("Indicates position of buttons inside container.")]
	[DefaultValue(eSplitterButtonPosition.Near)]
	[Category("Appearance")]
	public eSplitterButtonPosition ButtonPosition
	{
		get
		{
			return __2AC7;
		}
		set
		{
			if (value != __2AC7)
			{
				eSplitterButtonPosition oldValue = __2AC7;
				__2AC7 = value;
				OnButtonPositionChanged(oldValue, value);
			}
		}
	}

	[Description("Specifies how panels are collapsed when collapse buttons are pressed.")]
	[Category("Behavior")]
	[DefaultValue(eCollapseMode.PanelMinSize)]
	public eCollapseMode CollapseMode
	{
		get
		{
			return __2AC8;
		}
		set
		{
			if (value != __2AC8)
			{
				eCollapseMode oldValue = __2AC8;
				__2AC8 = value;
				OnCollapseModeChanged(oldValue, value);
			}
		}
	}

	[Description("Occurs before near collapse button is clicked and allows you to cancel its action.")]
	public event CancelEventHandler BeforeNearCollapseButtonClick
	{
		add
		{
			CancelEventHandler cancelEventHandler = _2ABF;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _2ABF, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = _2ABF;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _2ABF, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	[Description("Occurs after near collapse button is clicked.")]
	public event EventHandler NearCollapseButtonClicked
	{
		add
		{
			EventHandler eventHandler = _2AC0;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2AC0, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2AC0;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2AC0, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs before far collapse button is clicked and allows you to cancel its action.")]
	public event CancelEventHandler BeforeFarCollapseButtonClick
	{
		add
		{
			CancelEventHandler cancelEventHandler = _2AC5;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _2AC5, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = _2AC5;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _2AC5, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	[Description("Occurs after far collapse button is clicked.")]
	public event EventHandler FarCollapseButtonClicked
	{
		add
		{
			EventHandler eventHandler = _2AC6;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2AC6, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2AC6;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2AC6, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public CollapsibleSplitContainer()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		ControlStyles val = (ControlStyles)139282;
		((Control)this).SetStyle(val, true);
		object[] parameters = new object[2] { val, true };
		MethodInfo method = typeof(Control).GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
		if ((object)method != null)
		{
			method.Invoke(((SplitContainer)this).Panel1, parameters);
			method.Invoke(((SplitContainer)this).Panel2, parameters);
		}
		((SplitContainer)this).SplitterWidth = 20;
		__2ABE = typeof(SplitContainer).GetProperty("OverrideCursor", BindingFlags.Instance | BindingFlags.NonPublic);
		((SplitContainer)this).SplitterMoved += new SplitterEventHandler(_2ACE);
	}

	protected override void Dispose(bool disposing)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		((SplitContainer)this).SplitterMoved -= new SplitterEventHandler(_2ACE);
		((ContainerControl)this).Dispose(disposing);
	}

	private void _2AC9()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		Rectangle splitterRectangle = ((SplitContainer)this).SplitterRectangle;
		Size size = new Size(((SplitContainer)this).SplitterWidth, ((SplitContainer)this).SplitterWidth);
		if ((int)((SplitContainer)this).Orientation == 0)
		{
			if (__2AC7 == eSplitterButtonPosition.Near)
			{
				__2AB8 = new Rectangle(splitterRectangle.X, splitterRectangle.Y, size.Width, size.Height);
				__2AB9 = new Rectangle(__2AB8.Right + 2, splitterRectangle.Y, size.Width, size.Height);
			}
			else if (__2AC7 == eSplitterButtonPosition.Center)
			{
				__2AB8 = new Rectangle(splitterRectangle.X + (splitterRectangle.Width - (size.Width * 2 + 2)) / 2, splitterRectangle.Y, size.Width, size.Height);
				__2AB9 = new Rectangle(__2AB8.Right + 2, splitterRectangle.Y, size.Width, size.Height);
			}
			else if (__2AC7 == eSplitterButtonPosition.Far)
			{
				__2AB8 = new Rectangle(splitterRectangle.Right - size.Width * 2 - 2, splitterRectangle.Y, size.Width, size.Height);
				__2AB9 = new Rectangle(__2AB8.Right + 2, splitterRectangle.Y, size.Width, size.Height);
			}
		}
		else if (__2AC7 == eSplitterButtonPosition.Near)
		{
			__2AB8 = new Rectangle(splitterRectangle.X, splitterRectangle.Y, size.Width, size.Height);
			__2AB9 = new Rectangle(splitterRectangle.X, __2AB8.Bottom + 2, size.Width, size.Height);
		}
		else if (__2AC7 == eSplitterButtonPosition.Center)
		{
			__2AB8 = new Rectangle(splitterRectangle.X, splitterRectangle.Y + (splitterRectangle.Height - (size.Height * 2 + 2)) / 2, size.Width, size.Height);
			__2AB9 = new Rectangle(splitterRectangle.X, __2AB8.Bottom + 2, size.Width, size.Height);
		}
		else if (__2AC7 == eSplitterButtonPosition.Far)
		{
			__2AB8 = new Rectangle(splitterRectangle.X, splitterRectangle.Bottom - size.Height * 2 - 2, size.Width, size.Height);
			__2AB9 = new Rectangle(splitterRectangle.X, __2AB8.Bottom + 2, size.Width, size.Height);
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Invalid comparison between Unknown and I4
		_2AC9();
		((SplitContainer)this).OnPaint(e);
		Graphics graphics = e.Graphics;
		bool flag = (int)((SplitContainer)this).Orientation == 0;
		if (__2ABA != eButtonState.Hidden)
		{
			Office2007ButtonItemStateColorTable office2007StateColorTable = GetOffice2007StateColorTable(__2ABA);
			ᯊ.ᯓ(graphics, office2007StateColorTable, __2AB8, RoundRectangleShapeDescriptor.RectangleShape);
			Rectangle bounds = __2AB8;
			bounds.Inflate(-2, -2);
			TextDrawing.DrawStringLegacy(graphics, flag ? "\uf077" : "\uf053", Symbols.GetFont(9f, eSymbolSet.Awesome), office2007StateColorTable.Text, bounds, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter);
		}
		if (__2ABB != eButtonState.Hidden)
		{
			Office2007ButtonItemStateColorTable office2007StateColorTable2 = GetOffice2007StateColorTable(__2ABB);
			ᯊ.ᯓ(graphics, office2007StateColorTable2, __2AB9, RoundRectangleShapeDescriptor.RectangleShape);
			Rectangle bounds2 = __2AB9;
			bounds2.Inflate(-2, -2);
			TextDrawing.DrawStringLegacy(graphics, flag ? "\uf078" : "\uf054", Symbols.GetFont(9f, eSymbolSet.Awesome), office2007StateColorTable2.Text, bounds2, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter);
		}
	}

	protected Office2007ButtonItemStateColorTable GetOffice2007StateColorTable(eButtonState state)
	{
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			Office2007ColorTable colorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
			Office2007ButtonItemColorTable office2007ButtonItemColorTable = colorTable.ButtonItemColors[Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground)];
			if (((Control)this).Enabled)
			{
				switch (state)
				{
				case eButtonState.Disabled:
					break;
				case eButtonState.MouseDownLeft:
					return office2007ButtonItemColorTable.Pressed;
				case eButtonState.MouseOver:
					return office2007ButtonItemColorTable.MouseOver;
				default:
					return office2007ButtonItemColorTable.Default;
				}
			}
			return office2007ButtonItemColorTable.Disabled;
		}
		return null;
	}

	protected override void OnLayout(LayoutEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		((SplitContainer)this).OnLayout(e);
		if (__2ABC != ((SplitContainer)this).Orientation)
		{
			__2ABC = ((SplitContainer)this).Orientation;
			_2AC9();
		}
		((Control)this).Invalidate();
	}

	private bool _2ACA(eButtonState య)
	{
		if (య != eButtonState.Hidden)
		{
			return య != eButtonState.Disabled;
		}
		return false;
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		if (_2ACA(__2ABA))
		{
			if (__2AB8.Contains(e.Location))
			{
				if (__2ABA == eButtonState.Normal)
				{
					__2ABA = eButtonState.MouseOver;
					((Control)this).Invalidate();
				}
			}
			else
			{
				__2ABA = eButtonState.Normal;
				((Control)this).Invalidate();
			}
		}
		if (_2ACA(__2ABB))
		{
			if (__2AB9.Contains(e.Location))
			{
				if (__2ABB == eButtonState.Normal)
				{
					__2ABB = eButtonState.MouseOver;
					((Control)this).Invalidate();
				}
			}
			else
			{
				__2ABB = eButtonState.Normal;
				((Control)this).Invalidate();
			}
		}
		if (__2AB8.Contains(e.Location) || __2AB9.Contains(e.Location))
		{
			if (__2ABD == (Cursor)null && (object)__2ABE != null)
			{
				__2ABD = (Cursor)__2ABE.GetValue(this, null);
				__2ABE.SetValue(this, Cursors.Default, null);
			}
		}
		else
		{
			_2ACB();
			((SplitContainer)this).OnMouseMove(e);
		}
	}

	private void _2ACB()
	{
		if (__2ABD != (Cursor)null)
		{
			__2ABE.SetValue(this, __2ABD, null);
			__2ABD = null;
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Invalid comparison between Unknown and I4
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Invalid comparison between Unknown and I4
		if (_2ACA(__2ABA) && __2AB8.Contains(e.Location) && (int)e.Button == 1048576)
		{
			__2ABA = eButtonState.MouseDownLeft;
			((Control)this).Invalidate();
		}
		if (_2ACA(__2ABB) && __2AB9.Contains(e.Location) && (int)e.Button == 1048576)
		{
			__2ABB = eButtonState.MouseDownLeft;
			((Control)this).Invalidate();
		}
		if (!__2AB8.Contains(e.Location) && !__2AB9.Contains(e.Location))
		{
			((SplitContainer)this).OnMouseDown(e);
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (_2ACA(__2ABA) && __2AB8.Contains(e.Location))
		{
			if (__2ABA == eButtonState.MouseDownLeft)
			{
				__2ABA = eButtonState.MouseOver;
				_2ACC();
			}
			((Control)this).Invalidate();
		}
		if (_2ACA(__2ABB) && __2AB9.Contains(e.Location))
		{
			if (__2ABB == eButtonState.MouseDownLeft)
			{
				__2ABB = eButtonState.MouseOver;
				_2ACD();
			}
			((Control)this).Invalidate();
		}
		if (!__2AB8.Contains(e.Location) && !__2AB9.Contains(e.Location))
		{
			((SplitContainer)this).OnMouseUp(e);
		}
	}

	protected override void OnKeyUp(KeyEventArgs e)
	{
		((SplitContainer)this).OnKeyUp(e);
		((Control)this).Invalidate();
	}

	protected virtual void OnBeforeNearCollapseButtonClick(CancelEventArgs e)
	{
		_2ABF?.Invoke(this, e);
	}

	protected virtual void OnNearCollapseButtonClicked(EventArgs e)
	{
		_2AC0?.Invoke(this, e);
	}

	private void _2ACC()
	{
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnBeforeNearCollapseButtonClick(cancelEventArgs);
		if (cancelEventArgs.Cancel)
		{
			return;
		}
		if (__2AC8 == eCollapseMode.PanelMinSize)
		{
			__2AC4 = true;
			try
			{
				if (__2AC3)
				{
					((SplitContainer)this).SplitterDistance = __2AC1;
					__2AC3 = false;
					__2ABB = eButtonState.Normal;
				}
				else if (!__2AC2)
				{
					__2AC1 = ((SplitContainer)this).SplitterDistance;
					((SplitContainer)this).SplitterDistance = ((SplitContainer)this).Panel1MinSize;
					__2AC2 = true;
					__2ABA = eButtonState.Disabled;
				}
			}
			finally
			{
				__2AC4 = false;
			}
		}
		else
		{
			((SplitContainer)this).Panel1Collapsed = true;
		}
		OnNearCollapseButtonClicked(EventArgs.Empty);
	}

	protected virtual void OnBeforeFarCollapseButtonClick(CancelEventArgs e)
	{
		_2AC5?.Invoke(this, e);
	}

	protected virtual void OnFarCollapseButtonClicked(EventArgs e)
	{
		_2AC6?.Invoke(this, e);
	}

	private void _2ACD()
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Invalid comparison between Unknown and I4
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnBeforeFarCollapseButtonClick(cancelEventArgs);
		if (cancelEventArgs.Cancel)
		{
			return;
		}
		if (__2AC8 == eCollapseMode.PanelMinSize)
		{
			__2AC4 = true;
			try
			{
				if (__2AC2)
				{
					((SplitContainer)this).SplitterDistance = __2AC1;
					__2AC2 = false;
					__2ABA = eButtonState.Normal;
				}
				else if (!__2AC3)
				{
					__2AC1 = ((SplitContainer)this).SplitterDistance;
					if ((int)((SplitContainer)this).Orientation == 1)
					{
						((SplitContainer)this).SplitterDistance = ((Control)this).Width - ((SplitContainer)this).Panel2MinSize;
					}
					else
					{
						((SplitContainer)this).SplitterDistance = ((Control)this).Height - ((SplitContainer)this).Panel2MinSize;
					}
					__2AC3 = true;
					__2ABB = eButtonState.Disabled;
				}
			}
			finally
			{
				__2AC4 = false;
			}
		}
		else
		{
			((SplitContainer)this).Panel2Collapsed = true;
		}
		OnFarCollapseButtonClicked(EventArgs.Empty);
	}

	private void _2ACE(object _٠, SplitterEventArgs ؿ)
	{
		if (!__2AC4)
		{
			if (__2AC2)
			{
				__2AC2 = false;
				__2ABA = eButtonState.Normal;
			}
			else if (__2AC3)
			{
				__2AC3 = false;
				__2ABB = eButtonState.Normal;
			}
		}
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		if (_2ACA(__2ABA))
		{
			__2ABA = eButtonState.Normal;
		}
		if (_2ACA(__2ABB))
		{
			__2ABB = eButtonState.Normal;
		}
		_2ACB();
		((Control)this).Invalidate();
		((SplitContainer)this).OnMouseLeave(e);
	}

	protected virtual void OnButtonPositionChanged(eSplitterButtonPosition oldValue, eSplitterButtonPosition newValue)
	{
		_2AC9();
		((Control)this).Invalidate();
	}

	protected virtual void OnCollapseModeChanged(eCollapseMode oldValue, eCollapseMode newValue)
	{
	}
}
