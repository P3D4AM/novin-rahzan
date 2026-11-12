using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.Components;

[ToolboxItem(false)]
public class StiMathFormulaSelectionControl : Control
{
	private StiMathFormulaGroupType groupType;

	private Hashtable positionHashtable = new Hashtable();

	private Hashtable metaInfoHashtable;

	private int mainHeight;

	private bool isDown;

	private string[] basicArray = new string[18]
	{
		"\\:", "\\quad", "\\plus", "\\minus", "\\div", "\\times", "=", "\\neq", "x_a^b", "\\sqrt[n]{ab}",
		"\\overline{ab}", "(", ")", "\\lbrace", "\\rbrace", "\\sum_a^b", "\\lim_{a \\rightarrow b}", "\\sti{}"
	};

	private string[] arrowArray = new string[31]
	{
		"\\to", "\\longrightarrow", "\\Rightarrow", "\\Longrightarrow", "\\hookrightarrow", "\\mapsto", "\\longmapsto", "\\gets", "\\longleftarrow", "\\Leftarrow",
		"\\Longleftarrow", "\\hookleftarrow", "\\leftrightarrow", "\\longleftrightarrow", "\\Leftrightarrow", "\\Longleftrightarrow", "\\uparrow", "\\Uparrow", "\\downarrow", "\\Downarrow",
		"\\updownarrow", "\\Updownarrow", "\\nearrow", "\\searrow", "\\swarrow", "\\nwarrow", "\\leftharpoondown", "\\leftharpoonup", "\\rightharpoonup", "\\rightharpoondown",
		"\\rightleftharpoons"
	};

	private string[] functionArray = new string[19]
	{
		"\\log", "\\lg", "\\ln", "\\arg", "\\ker", "\\dim", "\\hom", "\\deg", "\\exp", "\\sin",
		"\\cos", "\\tan", "\\cot", "\\sec", "\\csc", "\\sinh", "\\cosh", "\\tanh", "\\coth"
	};

	private string[] mathArray = new string[25]
	{
		"\\int", "\\int_{a}^{b}", "\\oint", "\\oint_a^b", "\\sum", "\\sum_a^b", "\\coprod", "\\coprod_a^b", "\\prod", "\\prod_a^b",
		"\\bigcap", "\\bigcap_a^b", "\\bigcup", "\\bigcup_a^b", "\\sqrt{ab}", "\\sqrt[n]{ab}", "\\log_{a}{b}", "\\lg{ab}", "a^{b}", "a_{b}",
		"x_a^b", "\\overline{ab}", "\\underline{ab}", "\\frac{ab}{cd}", "\\lim_{a \\rightarrow b}"
	};

	private string[] formulaArray = new string[9] { "\\displaystyle\\sum\\limits_{i=0}^n i^3", "\\left(\\begin{array}{c}a\\\\ b\\end{array}\\right)", "\\left(\\frac{a^2}{b^3}\\right)", "\\left.\\frac{a^3}{3}\\right|_0^1", "\\sqrt{\\frac{n}{n-1} S}", "\\frac{1}{k}\\log_2 c(f)\\;", "x^n + y^n = z^n", "E=mc^2", "\\frac{1+\\frac{a}{b}}{1+\\frac{1}{1+\\frac{1}{a}}}" };

	private string[] alphabetsArray = new string[28]
	{
		"\\alpha", "\\beta", "\\gamma", "\\delta", "\\varepsilon", "\\zeta", "\\eta", "\\theta", "\\vartheta", "\\iota",
		"\\kappa", "\\lambda", "\\mu", "\\nu", "\\xi", "\\pi", "\\varpi", "\\rho", "\\varrho", "\\sigma",
		"\\varsigma", "\\tau", "\\upsilon", "\\phi", "\\varphi", "\\chi", "\\psi", "\\omega"
	};

	private string[] operatorsArray = new string[210]
	{
		"\\mp", "\\doteqdot", "\\gtrless", "\\doublebarwedge", "\\gtrdot", "\\eqsim", "\\gvertneqq", "\\ngeqq", "\\leqq", "\\ngtr",
		"\\sim", "\\circ", "\\intercal", "\\amalg", "\\circledast", "\\leftthreetimes", "\\star", "\\ast", "\\circledcirc", "\\lessdot",
		"\\barwedge", "\\circleddash", "\\ltimes", "\\bigcirc", "\\odot", "\\uplus", "\\curlyvee", "\\ominus", "\\vee", "\\boxdot",
		"\\curlywedge", "\\oplus", "\\veebar", "\\boxminus", "\\dagger", "\\oslash", "\\wedge", "\\boxplus", "\\ddagger", "\\otimes",
		"\\wr", "\\boxtimes", "\\diamond", "\\pm", "\\bullet", "\\rightthreetimes", "\\divideontimes", "\\rtimes", "\\dotplus", "\\setminus",
		"\\eqslantless", "\\equiv", "\\leqslant", "\\nleq", "\\simeq", "\\fallingdotseq", "\\lessapprox", "\\lesseqgtr", "\\nleqslant", "\\nleqq",
		"\\succ", "\\approx", "\\geq", "\\succapprox", "\\approxeq", "\\geqq", "\\lesseqqgtr", "\\nless", "\\succcurlyeq", "\\asymp",
		"\\geqslant", "\\lessgtr", "\\nprec", "\\succeq", "\\backsim", "\\gg", "\\lesssim", "\\npreceq", "\\succnapprox", "\\backsimeq",
		"\\ggg", "\\ll", "\\nsim", "\\succneqq", "\\bumpeq", "\\gnapprox", "\\lll", "\\nsucc", "\\succnsim", "\\Bumpeq",
		"\\gneq", "\\lnapprox", "\\nsucceq", "\\succsim", "\\circeq", "\\gneqq", "\\lneq", "\\prec", "\\thickapprox", "\\cong",
		"\\gnsim", "\\lneqq", "\\precapprox", "\\thicksim", "\\curlyeqprec", "\\gtrapprox", "\\lnsim", "\\preccurlyeq", "\\curlyeqsucc", "\\gtreqless",
		"\\lvertneqq", "\\preceq", "\\doteq", "\\gtreqqless", "\\ncong", "\\precnapprox", "\\precneqq", "\\eqcirc", "\\gtrsim", "\\ngeq",
		"\\precnsim", "\\precsim", "\\eqslantgtr", "\\leq", "\\ngeqslant", "\\risingdotseq", "\\backepsilon", "\\smallsmile", "\\therefore", "\\because",
		"\\smile", "\\between", "\\varpropto", "\\bowtie", "\\dashv", "\\frown", "\\smallfrown", "\\nvdash", "\\in", "\\nVdash",
		"\\mid", "\\nvDash", "\\models", "\\nVDash", "\\ni", "\\parallel", "\\vdash", "\\nmid", "\\perp", "\\Vdash",
		"\\notin", "\\pitchfork", "\\vDash", "\\propto", "\\Vvdash", "\\nshortmid", "\\shortmid", "\\nparallel", "\\nshortparallel", "\\shortparallel",
		"\\triangleleft", "\\vartriangle", "\\triangleright", "\\ntrianglerighteq", "\\vartriangleright", "\\vartriangleleft", "\\ntriangleright", "\\blacktriangleright", "\\ntrianglelefteq", "\\trianglerighteq",
		"\\blacktriangleleft", "\\ntriangleleft", "\\trianglelefteq", "\\bigtriangledown", "\\bigtriangleup", "\\triangleq", "\\sqcap", "\\sqcup", "\\sqsupset", "\\sqsubset",
		"\\sqsupseteq", "\\sqsubseteq", "\\Cap", "\\Cup", "\\Subset", "\\Supset", "\\cup", "\\cap", "\\subset", "\\supset",
		"\\subseteq", "\\subseteqq", "\\varsubsetneq", "\\subsetneqq", "\\varsubsetneqq", "\\nsubseteqq", "\\subsetneq", "\\nsubseteq", "\\supseteq", "\\supseteqq",
		"\\supsetneq", "\\varsupsetneq", "\\nsupseteq", "\\supsetneqq", "\\varsupsetneqq", "\\nsupseteqq", "\\bigodot", "\\bigotimes", "\\bigoplus", "\\biguplus"
	};

	public string CurrentFormula { get; set; }

	public event EventHandler FomulaClick;

	public StiMathFormulaSelectionControl()
		: this(StiMathFormulaGroupType.Arrows)
	{
	}

	internal StiMathFormulaSelectionControl(StiMathFormulaGroupType groupType)
	{
		((Control)this).ResizeRedraw = true;
		((Control)this).DoubleBuffered = true;
		this.groupType = groupType;
	}

	protected void InvokeFomulaClick()
	{
		this.FomulaClick?.Invoke(this, EventArgs.Empty);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		SetFocus(e.Location.X, e.Location.Y);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		CurrentFormula = null;
		isDown = false;
		((Control)this).Invalidate();
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		((Control)this).OnMouseClick(e);
		InvokeFomulaClick();
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
		isDown = true;
		((Control)this).Invalidate();
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		isDown = false;
		((Control)this).Invalidate();
	}

	protected override void Dispose(bool disposing)
	{
		((Control)this).Dispose(disposing);
		if (metaInfoHashtable == null)
		{
			return;
		}
		foreach (StiMathFormulaMetaInfo value in metaInfoHashtable.Values)
		{
			value.Image.Dispose();
		}
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		BuldMetaInfo(width - SystemInformation.VerticalScrollBarWidth);
		mainHeight = Math.Max(mainHeight, height);
		((Control)this).SetBoundsCore(x, y, width, mainHeight, specified);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		((Control)this).OnPaint(e);
		Graphics graphics = e.Graphics;
		graphics.Clear(Color.White);
		PaintIcons(graphics);
	}

	private void SetFocus(int x, int y)
	{
		foreach (object key in positionHashtable.Keys)
		{
			if (((Rectangle)positionHashtable[key]).Contains(x, y))
			{
				string text = key.ToString();
				if (CurrentFormula == null || !CurrentFormula.Equals(text))
				{
					CurrentFormula = text;
					((Control)this).Invalidate();
				}
				return;
			}
		}
		CurrentFormula = null;
		((Control)this).Invalidate();
	}

	private string[] GetArrayFormula()
	{
		return groupType switch
		{
			StiMathFormulaGroupType.Basic => basicArray, 
			StiMathFormulaGroupType.Arrows => arrowArray, 
			StiMathFormulaGroupType.Alphabets => alphabetsArray, 
			StiMathFormulaGroupType.Functions => functionArray, 
			StiMathFormulaGroupType.Formula => formulaArray, 
			StiMathFormulaGroupType.Operators => operatorsArray, 
			StiMathFormulaGroupType.Maths => mathArray, 
			_ => arrowArray, 
		};
	}

	private void PaintIcons(Graphics g)
	{
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Expected O, but got Unknown
		Hashtable hashtable = BuldMetaInfo(((Control)this).Width);
		foreach (StiMathFormulaMetaInfo value in hashtable.Values)
		{
			Image image = value.Image;
			if (image == null)
			{
				continue;
			}
			Rectangle rectButton = value.RectButton;
			int num = image.Width / (int)StiScale.Step;
			int num2 = image.Height / (int)StiScale.Step;
			int num3 = (rectButton.Width - num) / 2;
			int num4 = (rectButton.Height - num2) / 2;
			g.DrawImage(image, rectButton.X + num3, rectButton.Y + num4, num, num2);
			Pen val = new Pen(Color.LightGray, 1f);
			try
			{
				g.DrawRectangle(val, rectButton);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			if (CurrentFormula != null && CurrentFormula.Equals(value.Formula))
			{
				Color color = (isDown ? Color.Black : Color.DarkGray);
				Pen val2 = new Pen(color, 1f);
				try
				{
					g.DrawRectangle(val2, rectButton);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
		}
	}

	private Hashtable BuldMetaInfo(int mainWidth)
	{
		if (metaInfoHashtable != null)
		{
			return metaInfoHashtable;
		}
		metaInfoHashtable = new Hashtable();
		int num = 0;
		int num2 = 0;
		string folderImages = GetFolderImages();
		int num3 = 0;
		string[] arrayFormula = GetArrayFormula();
		foreach (string text in arrayFormula)
		{
			string text2 = folderImages + GetNameFile(text);
			string text3 = ((StiScale.Factor > 1.0) ? "_x2" : "");
			Image image = StiReportDesignImages.GetImage(text2 + text3);
			if (image != null)
			{
				int num4 = image.Width / (int)StiScale.Step;
				int num5 = image.Height / (int)StiScale.Step;
				int num6 = num4 + 2;
				int num7 = num5 + 2;
				if ((double)num6 < 32.0 * StiScale.Step)
				{
					num6 = 32 * (int)StiScale.Step;
				}
				if ((double)num7 < 29.0 * StiScale.Step)
				{
					num7 = 29 * (int)StiScale.Step;
				}
				num3 = Math.Max(num3, num7);
				if (num + num6 > mainWidth)
				{
					num = 0;
					num2 += num3 + StiScale.I2;
					num3 = num7 + StiScale.I2;
				}
				Rectangle rectangle = new Rectangle(num, num2, num6, num7);
				StiMathFormulaMetaInfo value = new StiMathFormulaMetaInfo
				{
					Image = image,
					RectButton = rectangle,
					Formula = text
				};
				metaInfoHashtable.Add(text2, value);
				positionHashtable.Add(text, rectangle);
				num += rectangle.Width + StiScale.I2;
			}
		}
		mainHeight = num2 + num3;
		return metaInfoHashtable;
	}

	private string GetFolderImages()
	{
		string result = "";
		switch (groupType)
		{
		case StiMathFormulaGroupType.Basic:
			result = "Math.Basic.";
			break;
		case StiMathFormulaGroupType.Alphabets:
			result = "Math.Alphabets.";
			break;
		case StiMathFormulaGroupType.Arrows:
			result = "Math.Arrows.";
			break;
		case StiMathFormulaGroupType.Functions:
			result = "Math.Functions.";
			break;
		case StiMathFormulaGroupType.Formula:
			result = "Math.Formula.";
			break;
		case StiMathFormulaGroupType.Maths:
			result = "Math.Maths.";
			break;
		case StiMathFormulaGroupType.Operators:
			result = "Math.Operators.";
			break;
		}
		return result;
	}

	private string GetNameFile(string formula)
	{
		formula = formula.Replace("\\", "").Replace("|", "").Replace(" ", "-");
		if (groupType == StiMathFormulaGroupType.Basic)
		{
			formula = formula.Replace(":", "space").Replace("=", "eq").Replace("(", "lgroup")
				.Replace(")", "rgroup");
			if (formula.Equals("sti{}"))
			{
				return "sti";
			}
		}
		string[] value = Regex.Split(formula, "(?<!^)(?=[A-Z])");
		formula = string.Join("_", value);
		if (formula.Length > 0 && char.IsUpper(formula[0]))
		{
			formula = "_" + formula;
		}
		return formula.ToLower();
	}
}
