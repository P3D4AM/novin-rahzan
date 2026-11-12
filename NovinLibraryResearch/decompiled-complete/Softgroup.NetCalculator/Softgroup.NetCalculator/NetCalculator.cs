using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using VV3esfRQYvkTDyq8r8;
using mKbaIlVHVypdZRXAVh;
using og5v0aGw6yPMOuFqdk;

namespace Softgroup.NetCalculator;

[DefaultEvent("ResultClick")]
[DefaultProperty("Value")]
[DesignerGenerated]
[ToolboxBitmap(typeof(NetCalculator), "Calculator.bmp")]
public class NetCalculator : UserControl
{
	public delegate void CloseClickEventHandler();

	public delegate void ResultClickEventHandler(bool EnterPressed);

	public delegate void DragDropEventHandler(object sender, DragEventArgs e);

	public delegate void DragEnterEventHandler(object sender, DragEventArgs e);

	public delegate void DragLeaveEventHandler(object sender, EventArgs e);

	public delegate void DragOverEventHandler(object sender, DragEventArgs e);

	[EditorBrowsable(EditorBrowsableState.Never)]
	public delegate void AutoSizeChangedEventHandler(object sender, EventArgs e);

	[EditorBrowsable(EditorBrowsableState.Never)]
	public delegate void BindingContextChangedEventHandler(object sender, EventArgs e);

	[EditorBrowsable(EditorBrowsableState.Never)]
	public delegate void PaddingChangedEventHandler(object sender, EventArgs e);

	[EditorBrowsable(EditorBrowsableState.Never)]
	public delegate void ScrollEventHandler(object sender, ScrollEventArgs e);

	[EditorBrowsable(EditorBrowsableState.Never)]
	public delegate void AutoValidateChangedEventHandler(object sender, EventArgs e);

	[EditorBrowsable(EditorBrowsableState.Never)]
	public delegate void CausesValidationChangedEventHandler(object sender, EventArgs e);

	[EditorBrowsable(EditorBrowsableState.Never)]
	public delegate void ValidatedEventHandler(object sender, EventArgs e);

	[EditorBrowsable(EditorBrowsableState.Never)]
	public delegate void ValidatingEventHandler(object sender, CancelEventArgs e);

	private IContainer WPs4aofmG3;

	[AccessedThroughProperty("OperatorMinus")]
	private Button _8K446f9Vd2;

	[AccessedThroughProperty("ToolTip1")]
	private ToolTip nGc4GrDJpy;

	[AccessedThroughProperty("Number1")]
	private Button _2T74TpnXls;

	[AccessedThroughProperty("Number2")]
	private Button UM14RKFKD7;

	[AccessedThroughProperty("Number3")]
	private Button VxU411ElZn;

	[AccessedThroughProperty("OperatorMultiple")]
	private Button _6w14FwFAAt;

	[AccessedThroughProperty("OperatorDivide")]
	private Button u7U4QJd1Ld;

	[AccessedThroughProperty("Number0")]
	private Button K3r4S1Af9c;

	[AccessedThroughProperty("NDecimal")]
	private Button cqe48loY52;

	[AccessedThroughProperty("OperatorResult")]
	private Button aJk4JtMxgM;

	[AccessedThroughProperty("Percent")]
	private Button _1bl4L2jUCy;

	[AccessedThroughProperty("ReadOut")]
	private Label _48C4l5YRq3;

	[AccessedThroughProperty("OperatorPlus")]
	private Button _3lJ4hjp6xT;

	[AccessedThroughProperty("CmdPlusMinus")]
	private Button nZk4AWP3pZ;

	[AccessedThroughProperty("Amount")]
	private Button _2b04dm1kLK;

	[AccessedThroughProperty("Number7")]
	private Button SRi4r90wWj;

	[AccessedThroughProperty("Number8")]
	private Button Cia4o29U4i;

	[AccessedThroughProperty("Number9")]
	private Button Gsf4OcW1LL;

	[AccessedThroughProperty("Cancel")]
	private Button YbP4fTV2md;

	[AccessedThroughProperty("CancelEntry")]
	private Button _7pe4upnTMq;

	[AccessedThroughProperty("Number4")]
	private Button TWJ4veRaZk;

	[AccessedThroughProperty("Number5")]
	private Button Arj4yooeNq;

	[AccessedThroughProperty("Number6")]
	private Button uqC49v719M;

	[AccessedThroughProperty("CmdClose")]
	private Button Ptl4ExZ5wr;

	private CloseClickEventHandler m274MJBGOP;

	private ResultClickEventHandler u7a4pvj0Lp;

	private bool PhJ4VRNbhR;

	private Color RO94IuDCdA;

	private Color ho44PLDTKJ;

	private bool Z3S4iJayrk;

	private FlatStyle AOE4NQVRE7;

	private string gXo4nvo3Tj;

	private string XIj4C4M8kd;

	private int DSO4s5YOCE;

	private string osC4zMku12;

	private string A1MKWv116b;

	private string pGwK4KLZyY;

	private Color wDZKKAjgpH;

	private int pFFKe5Mvvk;

	private int iNZKBRaCXe;

	private bool VTrKmooNOc;

	private Color _0D3K7AL3le;

	private Color _6f2KYUQwyi;

	private bool WicKZupjXP;

	private bool YeZKkPkZCm;

	private bool _03rKcAIyOY;

	private bool k5ZKblE9tU;

	private DragDropEventHandler TTnKgCoay8;

	private DragEnterEventHandler rATK2lQeG7;

	private DragLeaveEventHandler _4JtKthrV0q;

	private DragOverEventHandler UhUKx62ppC;

	private AutoSizeChangedEventHandler U1YK3wmOrZ;

	private BindingContextChangedEventHandler ireKDF6JJc;

	private PaddingChangedEventHandler CnJKXJf1BV;

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected const int ScrollStateAutoScrolling = 1;

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected const int ScrollStateFullDrag = 16;

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected const int ScrollStateHScrollVisible = 2;

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected const int ScrollStateUserHasScrolled = 8;

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected const int ScrollStateVScrollVisible = 4;

	private ScrollEventHandler QXDKq2DQpB;

	private AutoValidateChangedEventHandler VRMKHGNnl6;

	private CausesValidationChangedEventHandler WcWKjRUnuT;

	private ValidatedEventHandler RuKKUOIMu7;

	private ValidatingEventHandler _5F2K5WLlE6;

	internal virtual Button OperatorMinus
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return _8K446f9Vd2;
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		set
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			PreviewKeyDownEventHandler val = new PreviewKeyDownEventHandler(BmF4KyC7c5);
			KeyPressEventHandler val2 = new KeyPressEventHandler(QJs440uVjH);
			EventHandler eventHandler = _7P04Wh9dha;
			if (_8K446f9Vd2 != null)
			{
				((Control)_8K446f9Vd2).PreviewKeyDown -= val;
				((Control)_8K446f9Vd2).KeyPress -= val2;
				((Control)_8K446f9Vd2).Click -= eventHandler;
			}
			_8K446f9Vd2 = value;
			if (_8K446f9Vd2 != null)
			{
				((Control)_8K446f9Vd2).PreviewKeyDown += val;
				((Control)_8K446f9Vd2).KeyPress += val2;
				((Control)_8K446f9Vd2).Click += eventHandler;
			}
		}
	}

	internal virtual ToolTip ToolTip1
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return nGc4GrDJpy;
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		set
		{
			nGc4GrDJpy = value;
		}
	}

	internal virtual Button Number1
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return _2T74TpnXls;
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		set
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			PreviewKeyDownEventHandler val = new PreviewKeyDownEventHandler(wRVS1Npal);
			KeyPressEventHandler val2 = new KeyPressEventHandler(JVOQkZRhS);
			EventHandler eventHandler = FyHF56v2A;
			if (_2T74TpnXls != null)
			{
				((Control)_2T74TpnXls).PreviewKeyDown -= val;
				((Control)_2T74TpnXls).KeyPress -= val2;
				((Control)_2T74TpnXls).Click -= eventHandler;
			}
			_2T74TpnXls = value;
			if (_2T74TpnXls != null)
			{
				((Control)_2T74TpnXls).PreviewKeyDown += val;
				((Control)_2T74TpnXls).KeyPress += val2;
				((Control)_2T74TpnXls).Click += eventHandler;
			}
		}
	}

	internal virtual Button Number2
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return UM14RKFKD7;
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		set
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			PreviewKeyDownEventHandler val = new PreviewKeyDownEventHandler(DhJLWK2rl);
			KeyPressEventHandler val2 = new KeyPressEventHandler(D4DJKPQDk);
			EventHandler eventHandler = iCK8MpEBU;
			if (UM14RKFKD7 != null)
			{
				((Control)UM14RKFKD7).PreviewKeyDown -= val;
				((Control)UM14RKFKD7).KeyPress -= val2;
				((Control)UM14RKFKD7).Click -= eventHandler;
			}
			UM14RKFKD7 = value;
			if (UM14RKFKD7 != null)
			{
				((Control)UM14RKFKD7).PreviewKeyDown += val;
				((Control)UM14RKFKD7).KeyPress += val2;
				((Control)UM14RKFKD7).Click += eventHandler;
			}
		}
	}

	internal virtual Button Number3
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return VxU411ElZn;
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		set
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			PreviewKeyDownEventHandler val = new PreviewKeyDownEventHandler(A1HAo1FNt);
			KeyPressEventHandler val2 = new KeyPressEventHandler(qpIhQNrDQ);
			EventHandler eventHandler = TkElfgVSd;
			if (VxU411ElZn != null)
			{
				((Control)VxU411ElZn).PreviewKeyDown -= val;
				((Control)VxU411ElZn).KeyPress -= val2;
				((Control)VxU411ElZn).Click -= eventHandler;
			}
			VxU411ElZn = value;
			if (VxU411ElZn != null)
			{
				((Control)VxU411ElZn).PreviewKeyDown += val;
				((Control)VxU411ElZn).KeyPress += val2;
				((Control)VxU411ElZn).Click += eventHandler;
			}
		}
	}

	internal virtual Button OperatorMultiple
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return _6w14FwFAAt;
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		set
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			PreviewKeyDownEventHandler val = new PreviewKeyDownEventHandler(AVu4miPnpe);
			KeyPressEventHandler val2 = new KeyPressEventHandler(Qul4Bw1iGN);
			EventHandler eventHandler = yQA4eRTpdi;
			if (_6w14FwFAAt != null)
			{
				((Control)_6w14FwFAAt).PreviewKeyDown -= val;
				((Control)_6w14FwFAAt).KeyPress -= val2;
				((Control)_6w14FwFAAt).Click -= eventHandler;
			}
			_6w14FwFAAt = value;
			if (_6w14FwFAAt != null)
			{
				((Control)_6w14FwFAAt).PreviewKeyDown += val;
				((Control)_6w14FwFAAt).KeyPress += val2;
				((Control)_6w14FwFAAt).Click += eventHandler;
			}
		}
	}

	internal virtual Button OperatorDivide
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return u7U4QJd1Ld;
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		set
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			PreviewKeyDownEventHandler val = new PreviewKeyDownEventHandler(f0RzDy0nZ);
			KeyPressEventHandler val2 = new KeyPressEventHandler(krgsnKx0H);
			EventHandler eventHandler = ZecCT9sRd;
			if (u7U4QJd1Ld != null)
			{
				((Control)u7U4QJd1Ld).PreviewKeyDown -= val;
				((Control)u7U4QJd1Ld).KeyPress -= val2;
				((Control)u7U4QJd1Ld).Click -= eventHandler;
			}
			u7U4QJd1Ld = value;
			if (u7U4QJd1Ld != null)
			{
				((Control)u7U4QJd1Ld).PreviewKeyDown += val;
				((Control)u7U4QJd1Ld).KeyPress += val2;
				((Control)u7U4QJd1Ld).Click += eventHandler;
			}
		}
	}

	internal virtual Button Number0
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return K3r4S1Af9c;
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		set
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected O, but got Unknown
			EventHandler eventHandler = _2U8TXggJK;
			PreviewKeyDownEventHandler val = new PreviewKeyDownEventHandler(_9t61ZEg0G);
			KeyPressEventHandler val2 = new KeyPressEventHandler(vUIRBbNLB);
			if (K3r4S1Af9c != null)
			{
				((Control)K3r4S1Af9c).Click -= eventHandler;
				((Control)K3r4S1Af9c).PreviewKeyDown -= val;
				((Control)K3r4S1Af9c).KeyPress -= val2;
			}
			K3r4S1Af9c = value;
			if (K3r4S1Af9c != null)
			{
				((Control)K3r4S1Af9c).Click += eventHandler;
				((Control)K3r4S1Af9c).PreviewKeyDown += val;
				((Control)K3r4S1Af9c).KeyPress += val2;
			}
		}
	}

	internal virtual Button NDecimal
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return cqe48loY52;
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		set
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			PreviewKeyDownEventHandler val = new PreviewKeyDownEventHandler(_1WJG9boA1);
			KeyPressEventHandler val2 = new KeyPressEventHandler(Pdr6kKiH3);
			EventHandler eventHandler = GgEaADF22;
			if (cqe48loY52 != null)
			{
				((Control)cqe48loY52).PreviewKeyDown -= val;
				((Control)cqe48loY52).KeyPress -= val2;
				((Control)cqe48loY52).Click -= eventHandler;
			}
			cqe48loY52 = value;
			if (cqe48loY52 != null)
			{
				((Control)cqe48loY52).PreviewKeyDown += val;
				((Control)cqe48loY52).KeyPress += val2;
				((Control)cqe48loY52).Click += eventHandler;
			}
		}
	}

	internal virtual Button OperatorResult
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return aJk4JtMxgM;
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		set
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			KeyPressEventHandler val = new KeyPressEventHandler(BVd4bAHOVg);
			PreviewKeyDownEventHandler val2 = new PreviewKeyDownEventHandler(wo94cl7a4C);
			EventHandler eventHandler = UsL4kn13SH;
			if (aJk4JtMxgM != null)
			{
				((Control)aJk4JtMxgM).KeyPress -= val;
				((Control)aJk4JtMxgM).PreviewKeyDown -= val2;
				((Control)aJk4JtMxgM).Click -= eventHandler;
			}
			aJk4JtMxgM = value;
			if (aJk4JtMxgM != null)
			{
				((Control)aJk4JtMxgM).KeyPress += val;
				((Control)aJk4JtMxgM).PreviewKeyDown += val2;
				((Control)aJk4JtMxgM).Click += eventHandler;
			}
		}
	}

	internal virtual Button Percent
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return _1bl4L2jUCy;
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		set
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			PreviewKeyDownEventHandler val = new PreviewKeyDownEventHandler(_4CU4tn4exT);
			KeyPressEventHandler val2 = new KeyPressEventHandler(OpP420VIGE);
			EventHandler eventHandler = ARD4gJLDPY;
			if (_1bl4L2jUCy != null)
			{
				((Control)_1bl4L2jUCy).PreviewKeyDown -= val;
				((Control)_1bl4L2jUCy).KeyPress -= val2;
				((Control)_1bl4L2jUCy).Click -= eventHandler;
			}
			_1bl4L2jUCy = value;
			if (_1bl4L2jUCy != null)
			{
				((Control)_1bl4L2jUCy).PreviewKeyDown += val;
				((Control)_1bl4L2jUCy).KeyPress += val2;
				((Control)_1bl4L2jUCy).Click += eventHandler;
			}
		}
	}

	internal virtual Label ReadOut
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return _48C4l5YRq3;
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		set
		{
			_48C4l5YRq3 = value;
		}
	}

	internal virtual Button OperatorPlus
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return _3lJ4hjp6xT;
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		set
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			PreviewKeyDownEventHandler val = new PreviewKeyDownEventHandler(HM84Zt2Mr0);
			KeyPressEventHandler val2 = new KeyPressEventHandler(TS34YGVxpa);
			EventHandler eventHandler = e6N47snQf9;
			if (_3lJ4hjp6xT != null)
			{
				((Control)_3lJ4hjp6xT).PreviewKeyDown -= val;
				((Control)_3lJ4hjp6xT).KeyPress -= val2;
				((Control)_3lJ4hjp6xT).Click -= eventHandler;
			}
			_3lJ4hjp6xT = value;
			if (_3lJ4hjp6xT != null)
			{
				((Control)_3lJ4hjp6xT).PreviewKeyDown += val;
				((Control)_3lJ4hjp6xT).KeyPress += val2;
				((Control)_3lJ4hjp6xT).Click += eventHandler;
			}
		}
	}

	internal virtual Button CmdPlusMinus
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return nZk4AWP3pZ;
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		set
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			PreviewKeyDownEventHandler val = new PreviewKeyDownEventHandler(qGK02XhsS);
			KeyPressEventHandler val2 = new KeyPressEventHandler(BOswfaFEI);
			EventHandler eventHandler = _5Y254Y4RT;
			if (nZk4AWP3pZ != null)
			{
				((Control)nZk4AWP3pZ).PreviewKeyDown -= val;
				((Control)nZk4AWP3pZ).KeyPress -= val2;
				((Control)nZk4AWP3pZ).Click -= eventHandler;
			}
			nZk4AWP3pZ = value;
			if (nZk4AWP3pZ != null)
			{
				((Control)nZk4AWP3pZ).PreviewKeyDown += val;
				((Control)nZk4AWP3pZ).KeyPress += val2;
				((Control)nZk4AWP3pZ).Click += eventHandler;
			}
		}
	}

	internal virtual Button Amount
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return _2b04dm1kLK;
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		set
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			PreviewKeyDownEventHandler val = new PreviewKeyDownEventHandler(I3373P6Xi);
			KeyPressEventHandler val2 = new KeyPressEventHandler(_0IqmZ8KXt);
			EventHandler eventHandler = yKOBkU78q;
			if (_2b04dm1kLK != null)
			{
				((Control)_2b04dm1kLK).PreviewKeyDown -= val;
				((Control)_2b04dm1kLK).KeyPress -= val2;
				((Control)_2b04dm1kLK).Click -= eventHandler;
			}
			_2b04dm1kLK = value;
			if (_2b04dm1kLK != null)
			{
				((Control)_2b04dm1kLK).PreviewKeyDown += val;
				((Control)_2b04dm1kLK).KeyPress += val2;
				((Control)_2b04dm1kLK).Click += eventHandler;
			}
		}
	}

	internal virtual Button Number7
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return SRi4r90wWj;
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		set
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			PreviewKeyDownEventHandler val = new PreviewKeyDownEventHandler(X8ZprRADC);
			KeyPressEventHandler val2 = new KeyPressEventHandler(ZrQM4uNoD);
			EventHandler eventHandler = sJFEC1fMv;
			if (SRi4r90wWj != null)
			{
				((Control)SRi4r90wWj).PreviewKeyDown -= val;
				((Control)SRi4r90wWj).KeyPress -= val2;
				((Control)SRi4r90wWj).Click -= eventHandler;
			}
			SRi4r90wWj = value;
			if (SRi4r90wWj != null)
			{
				((Control)SRi4r90wWj).PreviewKeyDown += val;
				((Control)SRi4r90wWj).KeyPress += val2;
				((Control)SRi4r90wWj).Click += eventHandler;
			}
		}
	}

	internal virtual Button Number8
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return Cia4o29U4i;
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		set
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			PreviewKeyDownEventHandler val = new PreviewKeyDownEventHandler(_2PDPQsUoF);
			KeyPressEventHandler val2 = new KeyPressEventHandler(XstIvrbS4);
			EventHandler eventHandler = _9uBVmAIN3;
			if (Cia4o29U4i != null)
			{
				((Control)Cia4o29U4i).PreviewKeyDown -= val;
				((Control)Cia4o29U4i).KeyPress -= val2;
				((Control)Cia4o29U4i).Click -= eventHandler;
			}
			Cia4o29U4i = value;
			if (Cia4o29U4i != null)
			{
				((Control)Cia4o29U4i).PreviewKeyDown += val;
				((Control)Cia4o29U4i).KeyPress += val2;
				((Control)Cia4o29U4i).Click += eventHandler;
			}
		}
	}

	internal virtual Button Number9
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return Gsf4OcW1LL;
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		set
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			PreviewKeyDownEventHandler val = new PreviewKeyDownEventHandler(uIKnHAs20);
			KeyPressEventHandler val2 = new KeyPressEventHandler(sHLNSPfMX);
			EventHandler eventHandler = TNsibWK0C;
			if (Gsf4OcW1LL != null)
			{
				((Control)Gsf4OcW1LL).PreviewKeyDown -= val;
				((Control)Gsf4OcW1LL).KeyPress -= val2;
				((Control)Gsf4OcW1LL).Click -= eventHandler;
			}
			Gsf4OcW1LL = value;
			if (Gsf4OcW1LL != null)
			{
				((Control)Gsf4OcW1LL).PreviewKeyDown += val;
				((Control)Gsf4OcW1LL).KeyPress += val2;
				((Control)Gsf4OcW1LL).Click += eventHandler;
			}
		}
	}

	internal virtual Button Cancel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return YbP4fTV2md;
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		set
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			PreviewKeyDownEventHandler val = new PreviewKeyDownEventHandler(cp83BS5Rr);
			KeyPressEventHandler val2 = new KeyPressEventHandler(_9J8x1iZ8M);
			EventHandler eventHandler = yM5tEmlMa;
			if (YbP4fTV2md != null)
			{
				((Control)YbP4fTV2md).PreviewKeyDown -= val;
				((Control)YbP4fTV2md).KeyPress -= val2;
				((Control)YbP4fTV2md).Click -= eventHandler;
			}
			YbP4fTV2md = value;
			if (YbP4fTV2md != null)
			{
				((Control)YbP4fTV2md).PreviewKeyDown += val;
				((Control)YbP4fTV2md).KeyPress += val2;
				((Control)YbP4fTV2md).Click += eventHandler;
			}
		}
	}

	internal virtual Button CancelEntry
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return _7pe4upnTMq;
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		set
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			PreviewKeyDownEventHandler val = new PreviewKeyDownEventHandler(DwlqDig1j);
			KeyPressEventHandler val2 = new KeyPressEventHandler(KrVXOAgNK);
			EventHandler eventHandler = _0YjDkwMdj;
			if (_7pe4upnTMq != null)
			{
				((Control)_7pe4upnTMq).PreviewKeyDown -= val;
				((Control)_7pe4upnTMq).KeyPress -= val2;
				((Control)_7pe4upnTMq).Click -= eventHandler;
			}
			_7pe4upnTMq = value;
			if (_7pe4upnTMq != null)
			{
				((Control)_7pe4upnTMq).PreviewKeyDown += val;
				((Control)_7pe4upnTMq).KeyPress += val2;
				((Control)_7pe4upnTMq).Click += eventHandler;
			}
		}
	}

	internal virtual Button Number4
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return TWJ4veRaZk;
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		set
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			PreviewKeyDownEventHandler val = new PreviewKeyDownEventHandler(_7BEoy4PCF);
			KeyPressEventHandler val2 = new KeyPressEventHandler(pLVrWhCdj);
			EventHandler eventHandler = tlldbks8Y;
			if (TWJ4veRaZk != null)
			{
				((Control)TWJ4veRaZk).PreviewKeyDown -= val;
				((Control)TWJ4veRaZk).KeyPress -= val2;
				((Control)TWJ4veRaZk).Click -= eventHandler;
			}
			TWJ4veRaZk = value;
			if (TWJ4veRaZk != null)
			{
				((Control)TWJ4veRaZk).PreviewKeyDown += val;
				((Control)TWJ4veRaZk).KeyPress += val2;
				((Control)TWJ4veRaZk).Click += eventHandler;
			}
		}
	}

	internal virtual Button Number5
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return Arj4yooeNq;
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		set
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			PreviewKeyDownEventHandler val = new PreviewKeyDownEventHandler(u7ZuKri4Q);
			KeyPressEventHandler val2 = new KeyPressEventHandler(_0btf1q5jY);
			EventHandler eventHandler = BQIOyY4Dm;
			if (Arj4yooeNq != null)
			{
				((Control)Arj4yooeNq).PreviewKeyDown -= val;
				((Control)Arj4yooeNq).KeyPress -= val2;
				((Control)Arj4yooeNq).Click -= eventHandler;
			}
			Arj4yooeNq = value;
			if (Arj4yooeNq != null)
			{
				((Control)Arj4yooeNq).PreviewKeyDown += val;
				((Control)Arj4yooeNq).KeyPress += val2;
				((Control)Arj4yooeNq).Click += eventHandler;
			}
		}
	}

	internal virtual Button Number6
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return uqC49v719M;
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		set
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			PreviewKeyDownEventHandler val = new PreviewKeyDownEventHandler(GN39ubeoD);
			KeyPressEventHandler val2 = new KeyPressEventHandler(JoQyJ6ATL);
			EventHandler eventHandler = eEKvXYvB7;
			if (uqC49v719M != null)
			{
				((Control)uqC49v719M).PreviewKeyDown -= val;
				((Control)uqC49v719M).KeyPress -= val2;
				((Control)uqC49v719M).Click -= eventHandler;
			}
			uqC49v719M = value;
			if (uqC49v719M != null)
			{
				((Control)uqC49v719M).PreviewKeyDown += val;
				((Control)uqC49v719M).KeyPress += val2;
				((Control)uqC49v719M).Click += eventHandler;
			}
		}
	}

	internal virtual Button CmdClose
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return Ptl4ExZ5wr;
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		set
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			PreviewKeyDownEventHandler val = new PreviewKeyDownEventHandler(G1OUdiOv2);
			KeyPressEventHandler val2 = new KeyPressEventHandler(kAPjvfi5k);
			EventHandler eventHandler = RatHV1tWa;
			if (Ptl4ExZ5wr != null)
			{
				((Control)Ptl4ExZ5wr).PreviewKeyDown -= val;
				((Control)Ptl4ExZ5wr).KeyPress -= val2;
				((Control)Ptl4ExZ5wr).Click -= eventHandler;
			}
			Ptl4ExZ5wr = value;
			if (Ptl4ExZ5wr != null)
			{
				((Control)Ptl4ExZ5wr).PreviewKeyDown += val;
				((Control)Ptl4ExZ5wr).KeyPress += val2;
				((Control)Ptl4ExZ5wr).Click += eventHandler;
			}
		}
	}

	[Description("Gets/Sets if Return key will be handled like result command")]
	[DefaultValue(true)]
	[Category("Calculator")]
	public bool AcceptReturn
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return PhJ4VRNbhR;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			PhJ4VRNbhR = value;
		}
	}

	[Category("Calculator")]
	[DefaultValue(typeof(Color), "WhiteSmoke")]
	[Description("Gets/Sets buttons background color")]
	public Color ButtonsBackColor
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return RO94IuDCdA;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected O, but got Unknown
			RO94IuDCdA = value;
			foreach (Control control in ((Control)this).Controls)
			{
				Control val = control;
				if (val is Button)
				{
					val.BackColor = value;
				}
			}
		}
	}

	[DefaultValue(typeof(Color), "Red")]
	[Description("Gets/Sets cancels buttons foreground color")]
	[Category("Calculator")]
	public Color CancelsForeColor
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return ho44PLDTKJ;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			ho44PLDTKJ = value;
			((Control)Cancel).ForeColor = value;
			((Control)CancelEntry).ForeColor = value;
		}
	}

	[DefaultValue(typeof(FlatStyle), "Popup")]
	[Description("Gets/Sets FlatStyle for controls")]
	[Category("Calculator")]
	public FlatStyle FlatStyle
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			return AOE4NQVRE7;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected O, but got Unknown
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Invalid comparison between Unknown and I4
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			AOE4NQVRE7 = value;
			foreach (Control control in ((Control)this).Controls)
			{
				Control val = control;
				if (val is Button)
				{
					((ButtonBase)(Button)val).FlatStyle = AOE4NQVRE7;
				}
				else if (val is Label)
				{
					((Label)val).FlatStyle = AOE4NQVRE7;
				}
			}
			if ((int)value == 3)
			{
				((ButtonBase)CmdPlusMinus).Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(5150);
				((ButtonBase)CmdClose).Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(5158);
			}
			else
			{
				((ButtonBase)CmdPlusMinus).Text = "";
				((ButtonBase)CmdClose).Text = "";
			}
		}
	}

	[Description("Gets a string that rappresent last command requested by the user.")]
	[Category("Calculator")]
	public string LastCommand
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return gXo4nvo3Tj;
		}
	}

	[Category("Calculator")]
	[DefaultValue(typeof(Color), "Fuchsia")]
	[Description("Gets/Sets operators buttons foreground color")]
	public Color OperatorsForeColor
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return wDZKKAjgpH;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			wDZKKAjgpH = value;
			((Control)OperatorDivide).ForeColor = value;
			((Control)OperatorMinus).ForeColor = value;
			((Control)OperatorPlus).ForeColor = value;
			((Control)OperatorMultiple).ForeColor = value;
			((Control)OperatorDivide).ForeColor = value;
			((Control)OperatorResult).ForeColor = value;
			((Control)Percent).ForeColor = value;
			((Control)Amount).ForeColor = value;
			((Control)CmdPlusMinus).ForeColor = value;
			((ButtonBase)CmdPlusMinus).Image = (Image)(object)IDq40yCXeJ();
		}
	}

	[Category("Calculator")]
	[Description("Gets/Sets result box background color")]
	[DefaultValue(typeof(Color), "LightBlue")]
	public Color ResultBackColor
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return _0D3K7AL3le;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			_0D3K7AL3le = value;
			((Control)ReadOut).BackColor = value;
		}
	}

	[Category("Calculator")]
	[DefaultValue(typeof(Color), "Black")]
	[Description("Gets/Sets result box foreground color")]
	public Color ResultForeColor
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return _6f2KYUQwyi;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			_6f2KYUQwyi = value;
			((Control)ReadOut).ForeColor = value;
		}
	}

	[Category("Calculator")]
	[DefaultValue(true)]
	[Description("Gets/Sets if Close button is visible")]
	public bool ShowCloseButton
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return YeZKkPkZCm;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			YeZKkPkZCm = value;
			((Control)CmdClose).Visible = value;
		}
	}

	[Category("Calculator")]
	[DefaultValue(true)]
	[Description("Gets/Sets if Result button is visible")]
	public bool ShowResultButton
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return _03rKcAIyOY;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			_03rKcAIyOY = value;
			((Control)CmdPlusMinus).Visible = value;
		}
	}

	[DefaultValue(false)]
	[Description("Gets/Sets if Tooltips for controls are enabled")]
	[Category("Calculator")]
	public bool ShowTips
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return k5ZKblE9tU;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			k5ZKblE9tU = value;
			ToolTip1.Active = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool AllowDrop
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return ((Control)this).AllowDrop;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool AutoSize
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return ((UserControl)this).AutoSize;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public AutoSizeMode AutoSizeMode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			return ((UserControl)this).AutoSizeMode;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public BindingContext BindingContext
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return ((ContainerControl)this).BindingContext;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public ControlBindingsCollection DataBindings
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return ((Control)this).DataBindings;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	protected Padding DefaultPadding
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).DefaultPadding;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Padding Padding
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).Padding;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool AutoScroll
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return ((ScrollableControl)this).AutoScroll;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Size AutoScrollMargin
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return ((ScrollableControl)this).AutoScrollMargin;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Size AutoScrollMinSize
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return ((ScrollableControl)this).AutoScrollMinSize;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Point AutoScrollOffset
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return ((Control)this).AutoScrollOffset;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Point AutoScrollPosition
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return ((ScrollableControl)this).AutoScrollPosition;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public HScrollProperties HorizontalScroll
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return ((ScrollableControl)this).HorizontalScroll;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	protected bool HScroll
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return ((ScrollableControl)this).HScroll;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public VScrollProperties VerticalScroll
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return ((ScrollableControl)this).VerticalScroll;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	protected bool VScroll
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return ((ScrollableControl)this).VScroll;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public AutoValidate AutoValidate
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			return ((UserControl)this).AutoValidate;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool CausesValidation
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return ((Control)this).CausesValidation;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
		}
	}

	public event CloseClickEventHandler CloseClick
	{
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		add
		{
			m274MJBGOP = (CloseClickEventHandler)Delegate.Combine(m274MJBGOP, value);
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		remove
		{
			m274MJBGOP = (CloseClickEventHandler)Delegate.Remove(m274MJBGOP, value);
		}
	}

	public event ResultClickEventHandler ResultClick
	{
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		add
		{
			u7a4pvj0Lp = (ResultClickEventHandler)Delegate.Combine(u7a4pvj0Lp, value);
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		remove
		{
			u7a4pvj0Lp = (ResultClickEventHandler)Delegate.Remove(u7a4pvj0Lp, value);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public event DragDropEventHandler DragDrop
	{
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		add
		{
			TTnKgCoay8 = (DragDropEventHandler)Delegate.Combine(TTnKgCoay8, value);
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		remove
		{
			TTnKgCoay8 = (DragDropEventHandler)Delegate.Remove(TTnKgCoay8, value);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public event DragEnterEventHandler DragEnter
	{
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		add
		{
			rATK2lQeG7 = (DragEnterEventHandler)Delegate.Combine(rATK2lQeG7, value);
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		remove
		{
			rATK2lQeG7 = (DragEnterEventHandler)Delegate.Remove(rATK2lQeG7, value);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public event DragLeaveEventHandler DragLeave
	{
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		add
		{
			_4JtKthrV0q = (DragLeaveEventHandler)Delegate.Combine(_4JtKthrV0q, value);
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		remove
		{
			_4JtKthrV0q = (DragLeaveEventHandler)Delegate.Remove(_4JtKthrV0q, value);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public event DragOverEventHandler DragOver
	{
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		add
		{
			UhUKx62ppC = (DragOverEventHandler)Delegate.Combine(UhUKx62ppC, value);
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		remove
		{
			UhUKx62ppC = (DragOverEventHandler)Delegate.Remove(UhUKx62ppC, value);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public event AutoSizeChangedEventHandler AutoSizeChanged
	{
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		add
		{
			U1YK3wmOrZ = (AutoSizeChangedEventHandler)Delegate.Combine(U1YK3wmOrZ, value);
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		remove
		{
			U1YK3wmOrZ = (AutoSizeChangedEventHandler)Delegate.Remove(U1YK3wmOrZ, value);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public event BindingContextChangedEventHandler BindingContextChanged
	{
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		add
		{
			ireKDF6JJc = (BindingContextChangedEventHandler)Delegate.Combine(ireKDF6JJc, value);
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		remove
		{
			ireKDF6JJc = (BindingContextChangedEventHandler)Delegate.Remove(ireKDF6JJc, value);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public event PaddingChangedEventHandler PaddingChanged
	{
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		add
		{
			CnJKXJf1BV = (PaddingChangedEventHandler)Delegate.Combine(CnJKXJf1BV, value);
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		remove
		{
			CnJKXJf1BV = (PaddingChangedEventHandler)Delegate.Remove(CnJKXJf1BV, value);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public event ScrollEventHandler Scroll
	{
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		add
		{
			QXDKq2DQpB = (ScrollEventHandler)Delegate.Combine(QXDKq2DQpB, value);
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		remove
		{
			QXDKq2DQpB = (ScrollEventHandler)Delegate.Remove(QXDKq2DQpB, value);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public event AutoValidateChangedEventHandler AutoValidateChanged
	{
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		add
		{
			VRMKHGNnl6 = (AutoValidateChangedEventHandler)Delegate.Combine(VRMKHGNnl6, value);
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		remove
		{
			VRMKHGNnl6 = (AutoValidateChangedEventHandler)Delegate.Remove(VRMKHGNnl6, value);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public event CausesValidationChangedEventHandler CausesValidationChanged
	{
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		add
		{
			WcWKjRUnuT = (CausesValidationChangedEventHandler)Delegate.Combine(WcWKjRUnuT, value);
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		remove
		{
			WcWKjRUnuT = (CausesValidationChangedEventHandler)Delegate.Remove(WcWKjRUnuT, value);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public event ValidatedEventHandler Validated
	{
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		add
		{
			RuKKUOIMu7 = (ValidatedEventHandler)Delegate.Combine(RuKKUOIMu7, value);
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		remove
		{
			RuKKUOIMu7 = (ValidatedEventHandler)Delegate.Remove(RuKKUOIMu7, value);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public event ValidatingEventHandler Validating
	{
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		add
		{
			_5F2K5WLlE6 = (ValidatingEventHandler)Delegate.Combine(_5F2K5WLlE6, value);
		}
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.Synchronized)]
		remove
		{
			_5F2K5WLlE6 = (ValidatingEventHandler)Delegate.Remove(_5F2K5WLlE6, value);
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[DebuggerNonUserCode]
	protected override void Dispose(bool disposing)
	{
		try
		{
			if (disposing && WPs4aofmG3 != null)
			{
				WPs4aofmG3.Dispose();
			}
		}
		finally
		{
			((ContainerControl)this).Dispose(disposing);
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[DebuggerStepThrough]
	private void W3c4LH5ao()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_132a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1334: Expected O, but got Unknown
		WPs4aofmG3 = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(NetCalculator));
		OperatorMinus = new Button();
		Number1 = new Button();
		Number2 = new Button();
		Number3 = new Button();
		OperatorMultiple = new Button();
		OperatorDivide = new Button();
		Number0 = new Button();
		NDecimal = new Button();
		OperatorResult = new Button();
		Percent = new Button();
		ReadOut = new Label();
		OperatorPlus = new Button();
		CmdPlusMinus = new Button();
		Amount = new Button();
		Number7 = new Button();
		Number8 = new Button();
		Number9 = new Button();
		ToolTip1 = new ToolTip(WPs4aofmG3);
		Cancel = new Button();
		CancelEntry = new Button();
		Number4 = new Button();
		Number5 = new Button();
		Number6 = new Button();
		CmdClose = new Button();
		((Control)this).SuspendLayout();
		((ButtonBase)OperatorMinus).BackColor = Color.WhiteSmoke;
		((Control)OperatorMinus).Cursor = Cursors.Arrow;
		((ButtonBase)OperatorMinus).FlatStyle = (FlatStyle)1;
		((Control)OperatorMinus).ForeColor = Color.Fuchsia;
		Button operatorMinus = OperatorMinus;
		Point location = new Point(147, 72);
		((Control)operatorMinus).Location = location;
		((Control)OperatorMinus).Name = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3646);
		((Control)OperatorMinus).RightToLeft = (RightToLeft)0;
		Button operatorMinus2 = OperatorMinus;
		Size size = new Size(27, 27);
		((Control)operatorMinus2).Size = size;
		((Control)OperatorMinus).TabIndex = 15;
		((ButtonBase)OperatorMinus).Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3676);
		ToolTip1.SetToolTip((Control)(object)OperatorMinus, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3682));
		((ButtonBase)OperatorMinus).UseVisualStyleBackColor = false;
		((ButtonBase)Number1).BackColor = Color.WhiteSmoke;
		((Control)Number1).Cursor = Cursors.Arrow;
		((ButtonBase)Number1).FlatStyle = (FlatStyle)1;
		Button number = Number1;
		location = new Point(10, 104);
		((Control)number).Location = location;
		((Control)Number1).Name = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3688);
		((Control)Number1).RightToLeft = (RightToLeft)0;
		Button number2 = Number1;
		size = new Size(27, 27);
		((Control)number2).Size = size;
		((Control)Number1).TabIndex = 3;
		((ButtonBase)Number1).Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3706);
		ToolTip1.SetToolTip((Control)(object)Number1, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3712));
		((ButtonBase)Number1).UseVisualStyleBackColor = false;
		((ButtonBase)Number2).BackColor = Color.WhiteSmoke;
		((Control)Number2).Cursor = Cursors.Arrow;
		((ButtonBase)Number2).FlatStyle = (FlatStyle)1;
		Button number3 = Number2;
		location = new Point(42, 104);
		((Control)number3).Location = location;
		((Control)Number2).Name = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3718);
		((Control)Number2).RightToLeft = (RightToLeft)0;
		Button number4 = Number2;
		size = new Size(27, 27);
		((Control)number4).Size = size;
		((Control)Number2).TabIndex = 4;
		((ButtonBase)Number2).Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3736);
		ToolTip1.SetToolTip((Control)(object)Number2, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3742));
		((ButtonBase)Number2).UseVisualStyleBackColor = false;
		((ButtonBase)Number3).BackColor = Color.WhiteSmoke;
		((Control)Number3).Cursor = Cursors.Arrow;
		((ButtonBase)Number3).FlatStyle = (FlatStyle)1;
		Button number5 = Number3;
		location = new Point(74, 104);
		((Control)number5).Location = location;
		((Control)Number3).Name = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3748);
		((Control)Number3).RightToLeft = (RightToLeft)0;
		Button number6 = Number3;
		size = new Size(27, 27);
		((Control)number6).Size = size;
		((Control)Number3).TabIndex = 5;
		((ButtonBase)Number3).Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3766);
		ToolTip1.SetToolTip((Control)(object)Number3, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3772));
		((ButtonBase)Number3).UseVisualStyleBackColor = false;
		((ButtonBase)OperatorMultiple).BackColor = Color.WhiteSmoke;
		((Control)OperatorMultiple).Cursor = Cursors.Arrow;
		((ButtonBase)OperatorMultiple).FlatStyle = (FlatStyle)1;
		((Control)OperatorMultiple).ForeColor = Color.Fuchsia;
		Button operatorMultiple = OperatorMultiple;
		location = new Point(115, 104);
		((Control)operatorMultiple).Location = location;
		((Control)OperatorMultiple).Name = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3778);
		((Control)OperatorMultiple).RightToLeft = (RightToLeft)0;
		Button operatorMultiple2 = OperatorMultiple;
		size = new Size(27, 27);
		((Control)operatorMultiple2).Size = size;
		((Control)OperatorMultiple).TabIndex = 16;
		((ButtonBase)OperatorMultiple).Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3814);
		ToolTip1.SetToolTip((Control)(object)OperatorMultiple, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3820));
		((ButtonBase)OperatorMultiple).UseVisualStyleBackColor = false;
		((ButtonBase)OperatorDivide).BackColor = Color.WhiteSmoke;
		((Control)OperatorDivide).Cursor = Cursors.Arrow;
		((ButtonBase)OperatorDivide).FlatStyle = (FlatStyle)1;
		((Control)OperatorDivide).ForeColor = Color.Fuchsia;
		Button operatorDivide = OperatorDivide;
		location = new Point(147, 104);
		((Control)operatorDivide).Location = location;
		((Control)OperatorDivide).Name = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3826);
		((Control)OperatorDivide).RightToLeft = (RightToLeft)0;
		Button operatorDivide2 = OperatorDivide;
		size = new Size(27, 27);
		((Control)operatorDivide2).Size = size;
		((Control)OperatorDivide).TabIndex = 17;
		((ButtonBase)OperatorDivide).Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3858);
		ToolTip1.SetToolTip((Control)(object)OperatorDivide, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3864));
		((ButtonBase)OperatorDivide).UseVisualStyleBackColor = false;
		((ButtonBase)Number0).BackColor = Color.WhiteSmoke;
		((Control)Number0).Cursor = Cursors.Arrow;
		((ButtonBase)Number0).FlatStyle = (FlatStyle)1;
		Button number7 = Number0;
		location = new Point(10, 136);
		((Control)number7).Location = location;
		((Control)Number0).Name = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3870);
		((Control)Number0).RightToLeft = (RightToLeft)0;
		Button number8 = Number0;
		size = new Size(59, 27);
		((Control)number8).Size = size;
		((Control)Number0).TabIndex = 2;
		((ButtonBase)Number0).Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3888);
		ToolTip1.SetToolTip((Control)(object)Number0, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3894));
		((ButtonBase)Number0).UseVisualStyleBackColor = false;
		((ButtonBase)NDecimal).BackColor = Color.WhiteSmoke;
		((Control)NDecimal).Cursor = Cursors.Arrow;
		((ButtonBase)NDecimal).FlatStyle = (FlatStyle)1;
		Button nDecimal = NDecimal;
		location = new Point(74, 136);
		((Control)nDecimal).Location = location;
		((Control)NDecimal).Name = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3900);
		((Control)NDecimal).RightToLeft = (RightToLeft)0;
		Button nDecimal2 = NDecimal;
		size = new Size(27, 27);
		((Control)nDecimal2).Size = size;
		((Control)NDecimal).TabIndex = 20;
		((ButtonBase)NDecimal).Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3920);
		ToolTip1.SetToolTip((Control)(object)NDecimal, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3926));
		((ButtonBase)NDecimal).UseVisualStyleBackColor = false;
		((ButtonBase)OperatorResult).BackColor = Color.WhiteSmoke;
		((Control)OperatorResult).Cursor = Cursors.Arrow;
		((ButtonBase)OperatorResult).FlatStyle = (FlatStyle)1;
		((Control)OperatorResult).ForeColor = Color.Fuchsia;
		Button operatorResult = OperatorResult;
		location = new Point(10, 168);
		((Control)operatorResult).Location = location;
		((Control)OperatorResult).Name = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3932);
		((Control)OperatorResult).RightToLeft = (RightToLeft)0;
		Button operatorResult2 = OperatorResult;
		size = new Size(91, 27);
		((Control)operatorResult2).Size = size;
		((Control)OperatorResult).TabIndex = 1;
		((ButtonBase)OperatorResult).Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3964);
		ToolTip1.SetToolTip((Control)(object)OperatorResult, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3970));
		((ButtonBase)OperatorResult).UseVisualStyleBackColor = false;
		((ButtonBase)Percent).BackColor = Color.WhiteSmoke;
		((Control)Percent).Cursor = Cursors.Arrow;
		((ButtonBase)Percent).FlatStyle = (FlatStyle)1;
		((Control)Percent).ForeColor = Color.Fuchsia;
		Button percent = Percent;
		location = new Point(115, 136);
		((Control)percent).Location = location;
		((Control)Percent).Name = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3976);
		((Control)Percent).RightToLeft = (RightToLeft)0;
		Button percent2 = Percent;
		size = new Size(27, 27);
		((Control)percent2).Size = size;
		((Control)Percent).TabIndex = 18;
		((ButtonBase)Percent).Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(3994);
		ToolTip1.SetToolTip((Control)(object)Percent, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4000));
		((ButtonBase)Percent).UseVisualStyleBackColor = false;
		((Control)ReadOut).BackColor = Color.LightBlue;
		ReadOut.BorderStyle = (BorderStyle)2;
		((Control)ReadOut).Cursor = Cursors.Arrow;
		ReadOut.FlatStyle = (FlatStyle)1;
		((Control)ReadOut).ForeColor = Color.Black;
		Label readOut = ReadOut;
		location = new Point(10, 10);
		((Control)readOut).Location = location;
		((Control)ReadOut).Name = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4006);
		((Control)ReadOut).RightToLeft = (RightToLeft)0;
		Label readOut2 = ReadOut;
		size = new Size(163, 22);
		((Control)readOut2).Size = size;
		((Control)ReadOut).TabIndex = 21;
		ReadOut.Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4024);
		ReadOut.TextAlign = (ContentAlignment)64;
		((ButtonBase)OperatorPlus).BackColor = Color.WhiteSmoke;
		((Control)OperatorPlus).Cursor = Cursors.Arrow;
		((ButtonBase)OperatorPlus).FlatStyle = (FlatStyle)1;
		((Control)OperatorPlus).ForeColor = Color.Fuchsia;
		Button operatorPlus = OperatorPlus;
		location = new Point(115, 72);
		((Control)operatorPlus).Location = location;
		((Control)OperatorPlus).Name = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4032);
		((Control)OperatorPlus).RightToLeft = (RightToLeft)0;
		Button operatorPlus2 = OperatorPlus;
		size = new Size(27, 27);
		((Control)operatorPlus2).Size = size;
		((Control)OperatorPlus).TabIndex = 14;
		((ButtonBase)OperatorPlus).Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4060);
		ToolTip1.SetToolTip((Control)(object)OperatorPlus, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4066));
		((ButtonBase)OperatorPlus).UseVisualStyleBackColor = false;
		((ButtonBase)CmdPlusMinus).BackColor = Color.WhiteSmoke;
		((Control)CmdPlusMinus).Cursor = Cursors.Arrow;
		((ButtonBase)CmdPlusMinus).FlatStyle = (FlatStyle)1;
		((Control)CmdPlusMinus).ForeColor = Color.Fuchsia;
		Button cmdPlusMinus = CmdPlusMinus;
		location = new Point(115, 168);
		((Control)cmdPlusMinus).Location = location;
		((Control)CmdPlusMinus).Name = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4072);
		((Control)CmdPlusMinus).RightToLeft = (RightToLeft)0;
		Button cmdPlusMinus2 = CmdPlusMinus;
		size = new Size(27, 27);
		((Control)cmdPlusMinus2).Size = size;
		((Control)CmdPlusMinus).TabIndex = 22;
		ToolTip1.SetToolTip((Control)(object)CmdPlusMinus, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4100));
		((ButtonBase)CmdPlusMinus).UseVisualStyleBackColor = false;
		((ButtonBase)Amount).BackColor = Color.WhiteSmoke;
		((Control)Amount).Cursor = Cursors.Arrow;
		((ButtonBase)Amount).FlatStyle = (FlatStyle)1;
		((Control)Amount).ForeColor = Color.Fuchsia;
		Button amount = Amount;
		location = new Point(147, 136);
		((Control)amount).Location = location;
		((Control)Amount).Name = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4110);
		((Control)Amount).RightToLeft = (RightToLeft)0;
		Button amount2 = Amount;
		size = new Size(27, 27);
		((Control)amount2).Size = size;
		((Control)Amount).TabIndex = 19;
		((ButtonBase)Amount).Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4126);
		ToolTip1.SetToolTip((Control)(object)Amount, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4132));
		((ButtonBase)Amount).UseVisualStyleBackColor = false;
		((ButtonBase)Number7).BackColor = Color.WhiteSmoke;
		((Control)Number7).Cursor = Cursors.Arrow;
		((ButtonBase)Number7).FlatStyle = (FlatStyle)1;
		Button number9 = Number7;
		location = new Point(10, 40);
		((Control)number9).Location = location;
		((Control)Number7).Name = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4138);
		((Control)Number7).RightToLeft = (RightToLeft)0;
		Button number10 = Number7;
		size = new Size(27, 27);
		((Control)number10).Size = size;
		((Control)Number7).TabIndex = 9;
		((ButtonBase)Number7).Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4156);
		ToolTip1.SetToolTip((Control)(object)Number7, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4162));
		((ButtonBase)Number7).UseVisualStyleBackColor = false;
		((ButtonBase)Number8).BackColor = Color.WhiteSmoke;
		((Control)Number8).Cursor = Cursors.Arrow;
		((ButtonBase)Number8).FlatStyle = (FlatStyle)1;
		Button number11 = Number8;
		location = new Point(42, 40);
		((Control)number11).Location = location;
		((Control)Number8).Name = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4168);
		((Control)Number8).RightToLeft = (RightToLeft)0;
		Button number12 = Number8;
		size = new Size(27, 27);
		((Control)number12).Size = size;
		((Control)Number8).TabIndex = 10;
		((ButtonBase)Number8).Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4186);
		ToolTip1.SetToolTip((Control)(object)Number8, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4192));
		((ButtonBase)Number8).UseVisualStyleBackColor = false;
		((ButtonBase)Number9).BackColor = Color.WhiteSmoke;
		((Control)Number9).Cursor = Cursors.Arrow;
		((ButtonBase)Number9).FlatStyle = (FlatStyle)1;
		Button number13 = Number9;
		location = new Point(74, 40);
		((Control)number13).Location = location;
		((Control)Number9).Name = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4198);
		((Control)Number9).RightToLeft = (RightToLeft)0;
		Button number14 = Number9;
		size = new Size(27, 27);
		((Control)number14).Size = size;
		((Control)Number9).TabIndex = 11;
		((ButtonBase)Number9).Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4216);
		ToolTip1.SetToolTip((Control)(object)Number9, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4222));
		((ButtonBase)Number9).UseVisualStyleBackColor = false;
		ToolTip1.Active = false;
		ToolTip1.IsBalloon = true;
		ToolTip1.UseAnimation = false;
		((ButtonBase)Cancel).BackColor = Color.WhiteSmoke;
		((Control)Cancel).Cursor = Cursors.Arrow;
		((ButtonBase)Cancel).FlatStyle = (FlatStyle)1;
		((Control)Cancel).ForeColor = Color.Red;
		Button cancel = Cancel;
		location = new Point(115, 40);
		((Control)cancel).Location = location;
		((Control)Cancel).Name = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4228);
		((Control)Cancel).RightToLeft = (RightToLeft)0;
		Button cancel2 = Cancel;
		size = new Size(27, 27);
		((Control)cancel2).Size = size;
		((Control)Cancel).TabIndex = 12;
		((ButtonBase)Cancel).Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4244);
		ToolTip1.SetToolTip((Control)(object)Cancel, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4250));
		((ButtonBase)Cancel).UseVisualStyleBackColor = false;
		((ButtonBase)CancelEntry).BackColor = Color.WhiteSmoke;
		((Control)CancelEntry).Cursor = Cursors.Arrow;
		((ButtonBase)CancelEntry).FlatStyle = (FlatStyle)1;
		((Control)CancelEntry).ForeColor = Color.Red;
		Button cancelEntry = CancelEntry;
		location = new Point(147, 40);
		((Control)cancelEntry).Location = location;
		((Control)CancelEntry).Name = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4256);
		((Control)CancelEntry).RightToLeft = (RightToLeft)0;
		Button cancelEntry2 = CancelEntry;
		size = new Size(27, 27);
		((Control)cancelEntry2).Size = size;
		((Control)CancelEntry).TabIndex = 13;
		((ButtonBase)CancelEntry).Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4282);
		ToolTip1.SetToolTip((Control)(object)CancelEntry, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4288));
		((ButtonBase)CancelEntry).UseVisualStyleBackColor = false;
		((ButtonBase)Number4).BackColor = Color.WhiteSmoke;
		((Control)Number4).Cursor = Cursors.Arrow;
		((ButtonBase)Number4).FlatStyle = (FlatStyle)1;
		Button number15 = Number4;
		location = new Point(10, 72);
		((Control)number15).Location = location;
		((Control)Number4).Name = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4294);
		((Control)Number4).RightToLeft = (RightToLeft)0;
		Button number16 = Number4;
		size = new Size(27, 27);
		((Control)number16).Size = size;
		((Control)Number4).TabIndex = 6;
		((ButtonBase)Number4).Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4312);
		ToolTip1.SetToolTip((Control)(object)Number4, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4318));
		((ButtonBase)Number4).UseVisualStyleBackColor = false;
		((ButtonBase)Number5).BackColor = Color.WhiteSmoke;
		((Control)Number5).Cursor = Cursors.Arrow;
		((ButtonBase)Number5).FlatStyle = (FlatStyle)1;
		Button number17 = Number5;
		location = new Point(42, 72);
		((Control)number17).Location = location;
		((Control)Number5).Name = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4324);
		((Control)Number5).RightToLeft = (RightToLeft)0;
		Button number18 = Number5;
		size = new Size(27, 27);
		((Control)number18).Size = size;
		((Control)Number5).TabIndex = 7;
		((ButtonBase)Number5).Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4342);
		ToolTip1.SetToolTip((Control)(object)Number5, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4348));
		((ButtonBase)Number5).UseVisualStyleBackColor = false;
		((ButtonBase)Number6).BackColor = Color.WhiteSmoke;
		((Control)Number6).Cursor = Cursors.Arrow;
		((ButtonBase)Number6).FlatStyle = (FlatStyle)1;
		Button number19 = Number6;
		location = new Point(74, 72);
		((Control)number19).Location = location;
		((Control)Number6).Name = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4354);
		((Control)Number6).RightToLeft = (RightToLeft)0;
		Button number20 = Number6;
		size = new Size(27, 27);
		((Control)number20).Size = size;
		((Control)Number6).TabIndex = 8;
		((ButtonBase)Number6).Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4372);
		ToolTip1.SetToolTip((Control)(object)Number6, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4378));
		((ButtonBase)Number6).UseVisualStyleBackColor = false;
		((ButtonBase)CmdClose).BackColor = Color.WhiteSmoke;
		((Control)CmdClose).Cursor = Cursors.Arrow;
		((ButtonBase)CmdClose).FlatStyle = (FlatStyle)1;
		((Control)CmdClose).ForeColor = Color.Fuchsia;
		((ButtonBase)CmdClose).Image = (Image)componentResourceManager.GetObject(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4384));
		Button cmdClose = CmdClose;
		location = new Point(147, 168);
		((Control)cmdClose).Location = location;
		((Control)CmdClose).Name = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4416);
		((Control)CmdClose).RightToLeft = (RightToLeft)0;
		Button cmdClose2 = CmdClose;
		size = new Size(27, 27);
		((Control)cmdClose2).Size = size;
		((Control)CmdClose).TabIndex = 0;
		((ButtonBase)CmdClose).UseVisualStyleBackColor = false;
		SizeF autoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleDimensions = autoScaleDimensions;
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)CmdClose);
		((Control)this).Controls.Add((Control)(object)OperatorMinus);
		((Control)this).Controls.Add((Control)(object)Number1);
		((Control)this).Controls.Add((Control)(object)Number2);
		((Control)this).Controls.Add((Control)(object)Number3);
		((Control)this).Controls.Add((Control)(object)OperatorMultiple);
		((Control)this).Controls.Add((Control)(object)OperatorDivide);
		((Control)this).Controls.Add((Control)(object)Number0);
		((Control)this).Controls.Add((Control)(object)NDecimal);
		((Control)this).Controls.Add((Control)(object)OperatorResult);
		((Control)this).Controls.Add((Control)(object)Percent);
		((Control)this).Controls.Add((Control)(object)ReadOut);
		((Control)this).Controls.Add((Control)(object)OperatorPlus);
		((Control)this).Controls.Add((Control)(object)CmdPlusMinus);
		((Control)this).Controls.Add((Control)(object)Amount);
		((Control)this).Controls.Add((Control)(object)Number7);
		((Control)this).Controls.Add((Control)(object)Number8);
		((Control)this).Controls.Add((Control)(object)Number9);
		((Control)this).Controls.Add((Control)(object)Cancel);
		((Control)this).Controls.Add((Control)(object)CancelEntry);
		((Control)this).Controls.Add((Control)(object)Number4);
		((Control)this).Controls.Add((Control)(object)Number5);
		((Control)this).Controls.Add((Control)(object)Number6);
		((Control)this).DoubleBuffered = true;
		((Control)this).Name = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4436);
		size = new Size(185, 207);
		((Control)this).Size = size;
		((Control)this).ResumeLayout(false);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public NetCalculator()
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		eOWyXJHApIkypx1lmV.hB8jVZbTXcmOk();
		((UserControl)this)._002Ector();
		((Control)this).Enter += vbpYyCX6p;
		((Control)this).FontChanged += nhFZKSr32;
		((Control)this).ForeColorChanged += GwwkKaI8P;
		((Control)this).KeyPress += new KeyPressEventHandler(t4rcIWuoR);
		((Control)this).Layout += new LayoutEventHandler(QS4boPVS2);
		((UserControl)this).Load += EO3gs4ml9;
		((Control)this).Resize += DgN2yn0Zf;
		PhJ4VRNbhR = true;
		RO94IuDCdA = Color.FromKnownColor(KnownColor.WhiteSmoke);
		ho44PLDTKJ = Color.Red;
		Z3S4iJayrk = false;
		AOE4NQVRE7 = (FlatStyle)1;
		gXo4nvo3Tj = "";
		XIj4C4M8kd = "";
		osC4zMku12 = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4466);
		A1MKWv116b = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4472);
		pGwK4KLZyY = "";
		wDZKKAjgpH = Color.Fuchsia;
		pFFKe5Mvvk = 0;
		iNZKBRaCXe = 0;
		VTrKmooNOc = false;
		_0D3K7AL3le = Color.FromKnownColor(KnownColor.LightBlue);
		_6f2KYUQwyi = Color.FromKnownColor(KnownColor.Black);
		WicKZupjXP = false;
		YeZKkPkZCm = true;
		_03rKcAIyOY = true;
		k5ZKblE9tU = false;
		W3c4LH5ao();
		((ButtonBase)CmdPlusMinus).Image = (Image)(object)IDq40yCXeJ();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Finalize()
	{
		((Component)this).Finalize();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string G5IKTreRG(Point P_0, Size P_1, Font P_2)
	{
		return P_0.X + ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4478) + P_0.Y + ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4484) + P_1.Width + ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4490) + P_1.Height + ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4496) + P_2.Size;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double vdReqsWtc(string P_0)
	{
		double result;
		try
		{
			result = Conversions.ToDouble(P_0);
		}
		catch (Exception projectError)
		{
			ProjectData.SetProjectError(projectError);
			result = 0.0;
			ProjectData.ClearProjectError();
		}
		return result;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Description("Gets/Sets current value result of calculation as decimal")]
	public decimal ToDecimal()
	{
		if (ReadOut.Text.Length > 0)
		{
			return Conversions.ToDecimal(ReadOut.Text);
		}
		return 0m;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Description("Gets/Sets current value result of calculation as double")]
	public double ToDouble()
	{
		if (ReadOut.Text.Length > 0)
		{
			return Conversions.ToDouble(ReadOut.Text);
		}
		return 0.0;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Description("Gets/Sets current value result of calculation as single")]
	public float ToSingle()
	{
		if (ReadOut.Text.Length > 0)
		{
			return Conversions.ToSingle(ReadOut.Text);
		}
		return 0f;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Description("Gets/Sets current value result of calculation")]
	public string Value()
	{
		double num = ToDouble();
		if (num == 0.0)
		{
			return "";
		}
		return num.ToString();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void yKOBkU78q(object P_0, EventArgs P_1)
	{
		gXo4nvo3Tj = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4502);
		ReadOut.Text = Conversions.ToString(Conversion.Val(ReadOut.Text) / 100.0);
		if (!ReadOut.Text.Contains(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
		{
			ReadOut.Text += NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
		}
		XIj4C4M8kd = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4508);
		pGwK4KLZyY = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4518);
		checked
		{
			DSO4s5YOCE++;
			Z3S4iJayrk = true;
			((Control)NDecimal).Enabled = !Z3S4iJayrk;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void _0IqmZ8KXt(object P_0, KeyPressEventArgs P_1)
	{
		XZd4qt8Pdt(P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void I3373P6Xi(object P_0, PreviewKeyDownEventArgs P_1)
	{
		c6b45KE8aA(RuntimeHelpers.GetObjectValue(P_0), P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void vbpYyCX6p(object P_0, EventArgs P_1)
	{
		((Control)CmdPlusMinus).Focus();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void nhFZKSr32(object P_0, EventArgs P_1)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		foreach (Control control in ((Control)this).Controls)
		{
			Control val = control;
			val.Font = ((Control)this).Font;
		}
		jrN4w0lThx();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GwwkKaI8P(object P_0, EventArgs P_1)
	{
		((Control)Number0).ForeColor = ((Control)this).ForeColor;
		((Control)Number1).ForeColor = ((Control)this).ForeColor;
		((Control)Number2).ForeColor = ((Control)this).ForeColor;
		((Control)Number3).ForeColor = ((Control)this).ForeColor;
		((Control)Number4).ForeColor = ((Control)this).ForeColor;
		((Control)Number5).ForeColor = ((Control)this).ForeColor;
		((Control)Number6).ForeColor = ((Control)this).ForeColor;
		((Control)Number7).ForeColor = ((Control)this).ForeColor;
		((Control)Number8).ForeColor = ((Control)this).ForeColor;
		((Control)Number9).ForeColor = ((Control)this).ForeColor;
		((Control)NDecimal).ForeColor = ((Control)this).ForeColor;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void t4rcIWuoR(object P_0, KeyPressEventArgs P_1)
	{
		XZd4qt8Pdt(P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void QS4boPVS2(object P_0, LayoutEventArgs P_1)
	{
		jrN4w0lThx();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EO3gs4ml9(object P_0, EventArgs P_1)
	{
		((ButtonBase)NDecimal).Text = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
		ToolTip1.SetToolTip((Control)(object)NDecimal, NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
		((Control)this).ResizeRedraw = true;
		jrN4w0lThx();
		p1v4X4bRyH();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DgN2yn0Zf(object P_0, EventArgs P_1)
	{
		jrN4w0lThx();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void yM5tEmlMa(object P_0, EventArgs P_1)
	{
		_60q4xTgkix();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void _9J8x1iZ8M(object P_0, KeyPressEventArgs P_1)
	{
		XZd4qt8Pdt(P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void cp83BS5Rr(object P_0, PreviewKeyDownEventArgs P_1)
	{
		c6b45KE8aA(RuntimeHelpers.GetObjectValue(P_0), P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void _0YjDkwMdj(object P_0, EventArgs P_1)
	{
		_0Eo43LeuWv();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void KrVXOAgNK(object P_0, KeyPressEventArgs P_1)
	{
		XZd4qt8Pdt(P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DwlqDig1j(object P_0, PreviewKeyDownEventArgs P_1)
	{
		c6b45KE8aA(RuntimeHelpers.GetObjectValue(P_0), P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RatHV1tWa(object P_0, EventArgs P_1)
	{
		m274MJBGOP?.Invoke();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void kAPjvfi5k(object P_0, KeyPressEventArgs P_1)
	{
		XZd4qt8Pdt(P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void G1OUdiOv2(object P_0, PreviewKeyDownEventArgs P_1)
	{
		c6b45KE8aA(RuntimeHelpers.GetObjectValue(P_0), P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void _5Y254Y4RT(object P_0, EventArgs P_1)
	{
		gXo4nvo3Tj = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4526);
		ReadOut.Text = Conversions.ToString(Conversion.Val(ReadOut.Text) * -1.0);
		if (!ReadOut.Text.Contains(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
		{
			ReadOut.Text += NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BOswfaFEI(object P_0, KeyPressEventArgs P_1)
	{
		XZd4qt8Pdt(P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void qGK02XhsS(object P_0, PreviewKeyDownEventArgs P_1)
	{
		c6b45KE8aA(RuntimeHelpers.GetObjectValue(P_0), P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Category("Calculator")]
	[Description("Send a Result (=) command to the calculator")]
	public void DoResult()
	{
		_4B74jVPhJE(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4536));
		u7a4pvj0Lp?.Invoke(EnterPressed: false);
		License.q2lZj53xp();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GgEaADF22(object P_0, EventArgs P_1)
	{
		t1q4D5Jgcb();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Pdr6kKiH3(object P_0, KeyPressEventArgs P_1)
	{
		XZd4qt8Pdt(P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void _1WJG9boA1(object P_0, PreviewKeyDownEventArgs P_1)
	{
		c6b45KE8aA(RuntimeHelpers.GetObjectValue(P_0), P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void _2U8TXggJK(object P_0, EventArgs P_1)
	{
		pAh4Hemq2I(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4542));
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void vUIRBbNLB(object P_0, KeyPressEventArgs P_1)
	{
		XZd4qt8Pdt(P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void _9t61ZEg0G(object P_0, PreviewKeyDownEventArgs P_1)
	{
		c6b45KE8aA(RuntimeHelpers.GetObjectValue(P_0), P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FyHF56v2A(object P_0, EventArgs P_1)
	{
		pAh4Hemq2I(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4548));
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void JVOQkZRhS(object P_0, KeyPressEventArgs P_1)
	{
		XZd4qt8Pdt(P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void wRVS1Npal(object P_0, PreviewKeyDownEventArgs P_1)
	{
		c6b45KE8aA(RuntimeHelpers.GetObjectValue(P_0), P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void iCK8MpEBU(object P_0, EventArgs P_1)
	{
		pAh4Hemq2I(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4554));
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void D4DJKPQDk(object P_0, KeyPressEventArgs P_1)
	{
		XZd4qt8Pdt(P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DhJLWK2rl(object P_0, PreviewKeyDownEventArgs P_1)
	{
		c6b45KE8aA(RuntimeHelpers.GetObjectValue(P_0), P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void TkElfgVSd(object P_0, EventArgs P_1)
	{
		pAh4Hemq2I(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4560));
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void qpIhQNrDQ(object P_0, KeyPressEventArgs P_1)
	{
		XZd4qt8Pdt(P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void A1HAo1FNt(object P_0, PreviewKeyDownEventArgs P_1)
	{
		c6b45KE8aA(RuntimeHelpers.GetObjectValue(P_0), P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void tlldbks8Y(object P_0, EventArgs P_1)
	{
		pAh4Hemq2I(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4566));
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void pLVrWhCdj(object P_0, KeyPressEventArgs P_1)
	{
		XZd4qt8Pdt(P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void _7BEoy4PCF(object P_0, PreviewKeyDownEventArgs P_1)
	{
		c6b45KE8aA(RuntimeHelpers.GetObjectValue(P_0), P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BQIOyY4Dm(object P_0, EventArgs P_1)
	{
		pAh4Hemq2I(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4572));
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void _0btf1q5jY(object P_0, KeyPressEventArgs P_1)
	{
		XZd4qt8Pdt(P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void u7ZuKri4Q(object P_0, PreviewKeyDownEventArgs P_1)
	{
		c6b45KE8aA(RuntimeHelpers.GetObjectValue(P_0), P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void eEKvXYvB7(object P_0, EventArgs P_1)
	{
		pAh4Hemq2I(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4578));
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void JoQyJ6ATL(object P_0, KeyPressEventArgs P_1)
	{
		XZd4qt8Pdt(P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GN39ubeoD(object P_0, PreviewKeyDownEventArgs P_1)
	{
		c6b45KE8aA(RuntimeHelpers.GetObjectValue(P_0), P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void sJFEC1fMv(object P_0, EventArgs P_1)
	{
		pAh4Hemq2I(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4584));
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ZrQM4uNoD(object P_0, KeyPressEventArgs P_1)
	{
		XZd4qt8Pdt(P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void X8ZprRADC(object P_0, PreviewKeyDownEventArgs P_1)
	{
		c6b45KE8aA(RuntimeHelpers.GetObjectValue(P_0), P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void _9uBVmAIN3(object P_0, EventArgs P_1)
	{
		pAh4Hemq2I(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4590));
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void XstIvrbS4(object P_0, KeyPressEventArgs P_1)
	{
		XZd4qt8Pdt(P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void _2PDPQsUoF(object P_0, PreviewKeyDownEventArgs P_1)
	{
		c6b45KE8aA(RuntimeHelpers.GetObjectValue(P_0), P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void TNsibWK0C(object P_0, EventArgs P_1)
	{
		pAh4Hemq2I(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4596));
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void sHLNSPfMX(object P_0, KeyPressEventArgs P_1)
	{
		XZd4qt8Pdt(P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void uIKnHAs20(object P_0, PreviewKeyDownEventArgs P_1)
	{
		c6b45KE8aA(RuntimeHelpers.GetObjectValue(P_0), P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ZecCT9sRd(object P_0, EventArgs P_1)
	{
		_4B74jVPhJE(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4602));
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void krgsnKx0H(object P_0, KeyPressEventArgs P_1)
	{
		XZd4qt8Pdt(P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void f0RzDy0nZ(object P_0, PreviewKeyDownEventArgs P_1)
	{
		c6b45KE8aA(RuntimeHelpers.GetObjectValue(P_0), P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void _7P04Wh9dha(object P_0, EventArgs P_1)
	{
		_4B74jVPhJE(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4608));
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void QJs440uVjH(object P_0, KeyPressEventArgs P_1)
	{
		XZd4qt8Pdt(P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BmF4KyC7c5(object P_0, PreviewKeyDownEventArgs P_1)
	{
		c6b45KE8aA(RuntimeHelpers.GetObjectValue(P_0), P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void yQA4eRTpdi(object P_0, EventArgs P_1)
	{
		_4B74jVPhJE(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4614));
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Qul4Bw1iGN(object P_0, KeyPressEventArgs P_1)
	{
		XZd4qt8Pdt(P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AVu4miPnpe(object P_0, PreviewKeyDownEventArgs P_1)
	{
		c6b45KE8aA(RuntimeHelpers.GetObjectValue(P_0), P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void e6N47snQf9(object P_0, EventArgs P_1)
	{
		_4B74jVPhJE(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4620));
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void TS34YGVxpa(object P_0, KeyPressEventArgs P_1)
	{
		XZd4qt8Pdt(P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void HM84Zt2Mr0(object P_0, PreviewKeyDownEventArgs P_1)
	{
		c6b45KE8aA(RuntimeHelpers.GetObjectValue(P_0), P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UsL4kn13SH(object P_0, EventArgs P_1)
	{
		_4B74jVPhJE(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4626));
		u7a4pvj0Lp?.Invoke(WicKZupjXP);
		WicKZupjXP = false;
		License.q2lZj53xp();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void wo94cl7a4C(object P_0, PreviewKeyDownEventArgs P_1)
	{
		c6b45KE8aA(RuntimeHelpers.GetObjectValue(P_0), P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BVd4bAHOVg(object P_0, KeyPressEventArgs P_1)
	{
		XZd4qt8Pdt(P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ARD4gJLDPY(object P_0, EventArgs P_1)
	{
		Z3p4UyVSLc();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OpP420VIGE(object P_0, KeyPressEventArgs P_1)
	{
		XZd4qt8Pdt(P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void _4CU4tn4exT(object P_0, PreviewKeyDownEventArgs P_1)
	{
		c6b45KE8aA(RuntimeHelpers.GetObjectValue(P_0), P_1);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void _60q4xTgkix()
	{
		gXo4nvo3Tj = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4632);
		p1v4X4bRyH();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void _0Eo43LeuWv()
	{
		gXo4nvo3Tj = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4638);
		checked
		{
			if (((Operators.CompareString(XIj4C4M8kd, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4644), false) == 0) | (Operators.CompareString(XIj4C4M8kd, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4656), false) == 0)) && ReadOut.Text.Length > 0)
			{
				if (Z3S4iJayrk)
				{
					if (!ReadOut.Text.EndsWith(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
					{
						ReadOut.Text = ReadOut.Text.Substring(0, ReadOut.Text.Length - 1);
					}
					else
					{
						Z3S4iJayrk = false;
						((Control)NDecimal).Enabled = !Z3S4iJayrk;
					}
				}
				else
				{
					int num = ReadOut.Text.IndexOf(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
					if (num == -1)
					{
						ReadOut.Text = ReadOut.Text.Substring(0, ReadOut.Text.Length - 1) + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
					}
					else if (unchecked(num == 0 || num == 1))
					{
						ReadOut.Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4664) + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
					}
					else if (num > 1)
					{
						ReadOut.Text = ReadOut.Text.Substring(0, num - 1);
						if (Operators.CompareString(ReadOut.Text, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4670), false) == 0)
						{
							ReadOut.Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4676);
						}
						ReadOut.Text += NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
					}
				}
			}
			XIj4C4M8kd = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4682);
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void t1q4D5Jgcb()
	{
		gXo4nvo3Tj = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4690);
		if (Operators.CompareString(XIj4C4M8kd, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4696), false) == 0)
		{
			ReadOut.Text = Strings.Format((object)0, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4706));
			if (!ReadOut.Text.EndsWith(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
			{
				ReadOut.Text += NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
			}
		}
		else if (!((Operators.CompareString(XIj4C4M8kd, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4716), false) == 0) | (Operators.CompareString(XIj4C4M8kd, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4728), false) == 0)))
		{
			ReadOut.Text = Strings.Format((object)0, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4736));
			if (!ReadOut.Text.EndsWith(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
			{
				ReadOut.Text += NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
			}
		}
		Z3S4iJayrk = true;
		((Control)NDecimal).Enabled = !Z3S4iJayrk;
		XIj4C4M8kd = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4744);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void p1v4X4bRyH()
	{
		Z3S4iJayrk = false;
		((Control)NDecimal).Enabled = !Z3S4iJayrk;
		DSO4s5YOCE = 0;
		XIj4C4M8kd = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4756);
		pGwK4KLZyY = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4768);
		ReadOut.Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4774) + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void XZd4qt8Pdt(KeyPressEventArgs P_0)
	{
		switch (P_0.KeyChar)
		{
		case '0':
			((Control)Number0).Focus();
			pAh4Hemq2I(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4780));
			P_0.Handled = false;
			break;
		case '1':
			((Control)Number1).Focus();
			pAh4Hemq2I(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4786));
			P_0.Handled = false;
			break;
		case '2':
			((Control)Number2).Focus();
			pAh4Hemq2I(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4792));
			P_0.Handled = false;
			break;
		case '3':
			((Control)Number3).Focus();
			pAh4Hemq2I(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4798));
			P_0.Handled = false;
			break;
		case '4':
			((Control)Number4).Focus();
			pAh4Hemq2I(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4804));
			P_0.Handled = false;
			break;
		case '5':
			((Control)Number5).Focus();
			pAh4Hemq2I(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4810));
			P_0.Handled = false;
			break;
		case '6':
			((Control)Number6).Focus();
			pAh4Hemq2I(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4816));
			P_0.Handled = false;
			break;
		case '7':
			((Control)Number7).Focus();
			pAh4Hemq2I(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4822));
			P_0.Handled = false;
			break;
		case '8':
			((Control)Number8).Focus();
			pAh4Hemq2I(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4828));
			P_0.Handled = false;
			break;
		case '9':
			((Control)Number9).Focus();
			pAh4Hemq2I(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4834));
			P_0.Handled = false;
			break;
		case '.':
			((Control)NDecimal).Focus();
			t1q4D5Jgcb();
			P_0.Handled = false;
			break;
		case 'C':
		case 'c':
			((Control)Cancel).Focus();
			_60q4xTgkix();
			P_0.Handled = false;
			break;
		case '\b':
			((Control)CancelEntry).Focus();
			_0Eo43LeuWv();
			P_0.Handled = true;
			break;
		case '+':
			((Control)OperatorPlus).Focus();
			_4B74jVPhJE(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4840));
			P_0.Handled = true;
			break;
		case '-':
			((Control)OperatorMinus).Focus();
			_4B74jVPhJE(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4846));
			P_0.Handled = true;
			break;
		case '*':
		case 'X':
		case 'x':
			((Control)OperatorMultiple).Focus();
			_4B74jVPhJE(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4852));
			P_0.Handled = true;
			break;
		case '/':
		case ':':
		case '\\':
			((Control)OperatorDivide).Focus();
			_4B74jVPhJE(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4858));
			P_0.Handled = true;
			break;
		case '=':
			((Control)OperatorResult).Focus();
			_4B74jVPhJE(ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4864));
			P_0.Handled = true;
			break;
		case '%':
			((Control)Percent).Focus();
			Z3p4UyVSLc();
			P_0.Handled = true;
			break;
		case '\t':
		case '\n':
		case '\v':
		case '\f':
		case '\r':
		case '\u000e':
		case '\u000f':
		case '\u0010':
		case '\u0011':
		case '\u0012':
		case '\u0013':
		case '\u0014':
		case '\u0015':
		case '\u0016':
		case '\u0017':
		case '\u0018':
		case '\u0019':
		case '\u001a':
		case '\u001b':
		case '\u001c':
		case '\u001d':
		case '\u001e':
		case '\u001f':
		case ' ':
		case '!':
		case '"':
		case '#':
		case '$':
		case '&':
		case '\'':
		case '(':
		case ')':
		case ',':
		case ';':
		case '<':
		case '>':
		case '?':
		case '@':
		case 'A':
		case 'B':
		case 'D':
		case 'E':
		case 'F':
		case 'G':
		case 'H':
		case 'I':
		case 'J':
		case 'K':
		case 'L':
		case 'M':
		case 'N':
		case 'O':
		case 'P':
		case 'Q':
		case 'R':
		case 'S':
		case 'T':
		case 'U':
		case 'V':
		case 'W':
		case 'Y':
		case 'Z':
		case '[':
		case ']':
		case '^':
		case '_':
		case '`':
		case 'a':
		case 'b':
		case 'd':
		case 'e':
		case 'f':
		case 'g':
		case 'h':
		case 'i':
		case 'j':
		case 'k':
		case 'l':
		case 'm':
		case 'n':
		case 'o':
		case 'p':
		case 'q':
		case 'r':
		case 's':
		case 't':
		case 'u':
		case 'v':
		case 'w':
			break;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void pAh4Hemq2I(string P_0)
	{
		gXo4nvo3Tj = P_0;
		if (!((Operators.CompareString(XIj4C4M8kd, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4870), false) == 0) | (Operators.CompareString(XIj4C4M8kd, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4882), false) == 0)))
		{
			ReadOut.Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4890) + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
			Z3S4iJayrk = false;
			((Control)NDecimal).Enabled = !Z3S4iJayrk;
		}
		if (Z3S4iJayrk)
		{
			ReadOut.Text += P_0;
			if (!ReadOut.Text.Contains(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
			{
				ReadOut.Text += NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
			}
		}
		else
		{
			int num = ReadOut.Text.IndexOf(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
			if (num == -1)
			{
				if (Operators.CompareString(ReadOut.Text, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4896), false) == 0)
				{
					ReadOut.Text = "";
				}
				ReadOut.Text = ReadOut.Text + P_0 + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
			}
			else
			{
				ReadOut.Text = ReadOut.Text.Substring(0, num);
				if (Operators.CompareString(ReadOut.Text, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4902), false) == 0)
				{
					ReadOut.Text = "";
				}
				ReadOut.Text = ReadOut.Text + P_0 + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
			}
		}
		if (Operators.CompareString(XIj4C4M8kd, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4908), false) == 0)
		{
			ReadOut.Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4918) + ReadOut.Text;
		}
		XIj4C4M8kd = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4924);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void _4B74jVPhJE(string P_0)
	{
		string text = ReadOut.Text;
		gXo4nvo3Tj = P_0;
		checked
		{
			if (Operators.CompareString(XIj4C4M8kd, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4936), false) == 0)
			{
				DSO4s5YOCE++;
			}
			switch (DSO4s5YOCE)
			{
			case 0:
				if ((Operators.CompareString(P_0, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4948), false) == 0) & (Operators.CompareString(XIj4C4M8kd, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4954), false) != 0))
				{
					ReadOut.Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4964) + ReadOut.Text;
					XIj4C4M8kd = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4970);
				}
				if (!ReadOut.Text.Contains(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
				{
					ReadOut.Text += NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
				}
				break;
			case 1:
				osC4zMku12 = ReadOut.Text;
				if ((Operators.CompareString(P_0, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4980), false) == 0) & (Operators.CompareString(XIj4C4M8kd, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4986), false) != 0) & (Operators.CompareString(pGwK4KLZyY, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(4998), false) != 0))
				{
					ReadOut.Text = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(5004);
					XIj4C4M8kd = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(5012);
				}
				if (!ReadOut.Text.Contains(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
				{
					ReadOut.Text += NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
				}
				break;
			case 2:
			{
				A1MKWv116b = text;
				string text2 = pGwK4KLZyY;
				if (Operators.CompareString(text2, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(5022), false) == 0)
				{
					osC4zMku12 = Conversions.ToString(vdReqsWtc(osC4zMku12) + vdReqsWtc(A1MKWv116b));
				}
				else if (Operators.CompareString(text2, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(5028), false) == 0)
				{
					osC4zMku12 = Conversions.ToString(vdReqsWtc(osC4zMku12) - vdReqsWtc(A1MKWv116b));
				}
				else if (Operators.CompareString(text2, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(5034), false) == 0 || Operators.CompareString(text2, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(5040), false) == 0 || Operators.CompareString(text2, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(5046), false) == 0)
				{
					osC4zMku12 = Conversions.ToString(vdReqsWtc(osC4zMku12) * vdReqsWtc(A1MKWv116b));
				}
				else if (Operators.CompareString(text2, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(5052), false) == 0 || Operators.CompareString(text2, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(5058), false) == 0 || Operators.CompareString(text2, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(5064), false) == 0)
				{
					if (Conversions.ToDouble(A1MKWv116b) == 0.0)
					{
						osC4zMku12 = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(5070);
					}
					else
					{
						osC4zMku12 = Conversions.ToString(vdReqsWtc(osC4zMku12) / vdReqsWtc(A1MKWv116b));
					}
				}
				else if (Operators.CompareString(text2, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(5076), false) == 0)
				{
					osC4zMku12 = vdReqsWtc(A1MKWv116b).ToString();
				}
				else if (Operators.CompareString(text2, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(5082), false) == 0)
				{
					osC4zMku12 = Conversions.ToString(vdReqsWtc(osC4zMku12) / vdReqsWtc(A1MKWv116b)) + ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(5088);
				}
				else if (Operators.CompareString(text2, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(5094), false) == 0)
				{
					osC4zMku12 = (vdReqsWtc(osC4zMku12) * vdReqsWtc(A1MKWv116b)).ToString();
				}
				ReadOut.Text = osC4zMku12;
				DSO4s5YOCE = 1;
				if (!ReadOut.Text.Contains(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
				{
					ReadOut.Text += NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
				}
				break;
			}
			}
			if (Operators.CompareString(XIj4C4M8kd, ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(5102), false) != 0)
			{
				XIj4C4M8kd = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(5112);
				pGwK4KLZyY = P_0;
			}
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Z3p4UyVSLc()
	{
		gXo4nvo3Tj = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(5122);
		ReadOut.Text = Conversions.ToString(Conversion.Val(ReadOut.Text) / 100.0);
		if (!ReadOut.Text.Contains(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
		{
			ReadOut.Text += NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
		}
		XIj4C4M8kd = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(5128);
		pGwK4KLZyY = ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(5138);
		checked
		{
			DSO4s5YOCE++;
			Z3S4iJayrk = true;
			((Control)NDecimal).Enabled = !Z3S4iJayrk;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void c6b45KE8aA(object P_0, PreviewKeyDownEventArgs P_1)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Invalid comparison between Unknown and I4
		if (PhJ4VRNbhR && !P_1.Alt && !P_1.Control && !P_1.Shift && (int)P_1.KeyCode == 13)
		{
			WicKZupjXP = true;
			((Control)OperatorResult).Focus();
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void jrN4w0lThx()
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Expected O, but got Unknown
		string[] array = new string[0];
		Control val = null;
		double num = 1.0;
		double num2 = 1.0;
		double num3 = 1.0;
		checked
		{
			try
			{
				if (!VTrKmooNOc)
				{
					iNZKBRaCXe = ((Control)this).Size.Width;
					pFFKe5Mvvk = ((Control)this).Size.Height;
					foreach (Control control in ((Control)this).Controls)
					{
						val = control;
						val.Tag = G5IKTreRG(val.Location, val.Size, val.Font);
					}
					VTrKmooNOc = true;
					return;
				}
				num2 = (double)((Control)this).Size.Width / (double)iNZKBRaCXe;
				num3 = (double)((Control)this).Size.Height / (double)pFFKe5Mvvk;
				num = ((!(num3 < num2)) ? num2 : num3);
				((Control)this).SuspendLayout();
				foreach (Control control2 in ((Control)this).Controls)
				{
					val = control2;
					if (val.Tag != null)
					{
						array = Strings.Split(val.Tag.ToString(), ahKb9xUYBvE7Wxgg2P.W3c4LH5ao(5144), -1, (CompareMethod)0);
						Control val2 = val;
						Control obj = val2;
						Point location = new Point((int)Math.Round(Conversion.Val(array[0]) * num2), (int)Math.Round(Conversion.Val(array[1]) * num3));
						obj.Location = location;
						Control obj2 = val2;
						Size size = new Size((int)Math.Round(Conversion.Val(array[2]) * num2), (int)Math.Round(Conversion.Val(array[3]) * num3));
						obj2.Size = size;
						val2.Font = new Font(val2.Font.FontFamily, (float)((double)Conversions.ToSingle(array[4]) * num), val2.Font.Style, val2.Font.Unit);
						val2 = null;
					}
				}
				((Control)this).ResumeLayout();
			}
			finally
			{
				if (val != null)
				{
					val = null;
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Bitmap IDq40yCXeJ()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		Bitmap val = new Bitmap((Image)(object)ruYkb7o0RQE5EUPUqF.gMb6BRaRL().ToBitmap());
		ImageAttributes val2 = new ImageAttributes();
		try
		{
			ColorMap val3 = new ColorMap();
			val3.OldColor = Color.Fuchsia;
			val3.NewColor = wDZKKAjgpH;
			val2.SetRemapTable((ColorMap[])(object)new ColorMap[1] { val3 });
			Graphics val4 = Graphics.FromImage((Image)(object)val);
			try
			{
				Rectangle rectangle = new Rectangle(0, 0, ((Image)val).Width, ((Image)val).Height);
				val4.DrawImage((Image)(object)val, rectangle, 0, 0, ((Image)val).Width, ((Image)val).Height, (GraphicsUnit)2, val2);
				return val;
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		DragDropEffects result = default(DragDropEffects);
		return result;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected void OnAutoSizeChanged(EventArgs e)
	{
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected AutoSizeMode GetAutoSizeMode()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		AutoSizeMode result = default(AutoSizeMode);
		return result;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected void SetAutoSizeMode(AutoSizeMode mode)
	{
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected void OnBindingContextChanged(EventArgs e)
	{
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBindings()
	{
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected void OnPaddingChanged(EventArgs e)
	{
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected void OnScroll(ScrollEventArgs se)
	{
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected void AdjustFormScrollbars(bool displayScrollbars)
	{
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected bool GetScrollState(int bit)
	{
		bool result = default(bool);
		return result;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ScrollControlIntoView(Control activeControl)
	{
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected Point ScrollToControl(Control activeControl)
	{
		Point result = default(Point);
		return result;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SetAutoScrollMargin(int x, int y)
	{
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected void SetScrollState(int bit, bool value)
	{
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected void OnAutoValidateChanged(EventArgs e)
	{
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected void OnCausesValidationChanged(EventArgs e)
	{
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected void OnValidated(EventArgs e)
	{
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected void OnValidating(CancelEventArgs e)
	{
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool Validate()
	{
		bool result = default(bool);
		return result;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool Validate(bool checkAutoValidate)
	{
		bool result = default(bool);
		return result;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ValidateChildren()
	{
		bool result = default(bool);
		return result;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ValidateChildren(ValidationConstraints validationConstraints)
	{
		bool result = default(bool);
		return result;
	}
}
