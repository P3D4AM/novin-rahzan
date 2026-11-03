using System;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Rendering;
using DevComponents.Editors.DateTimeAdv;

namespace DevComponents.AdvTree;

public class Utilities
{
	private static string _17CE => DateTimeInput.GetActiveCulture().NumberFormat.NumberDecimalSeparator;

	private static string _17CF => DateTimeInput.GetActiveCulture().NumberFormat.NumberGroupSeparator;

	public static void InitializeTree(AdvTree tree)
	{
		InitializeTree(tree, new ComponentFactory());
	}

	public static void InitializeTree(AdvTree tree, ComponentFactory factory)
	{
		tree.NodesConnector = factory.CreateComponent(typeof(NodeConnector)) as NodeConnector;
		tree.NodesConnector.LineWidth = 1;
		tree.NodesConnector.LineColor = SystemColors.ControlText;
		((Control)tree).BackColor = SystemColors.Window;
		ElementStyle elementStyle = factory.CreateComponent(typeof(ElementStyle)) as ElementStyle;
		elementStyle.TextColor = SystemColors.ControlText;
		tree.Styles.Add(elementStyle);
		tree.NodeStyle = elementStyle;
		tree.BackgroundStyle.Class = ElementStyleClassKeys.TreeBorderKey;
		((Control)tree).AccessibleRole = (AccessibleRole)35;
	}

	internal static ElementStyle _17D0(ComponentFactory ቖ, string _17D1, Color ዾ, Color ጁ, Color ገ, int ॷ, Color ໆ)
	{
		eStyleBorderType eStyleBorderType = eStyleBorderType.Solid;
		ElementStyle elementStyle = ቖ.CreateComponent(typeof(ElementStyle)) as ElementStyle;
		elementStyle.Description = _17D1;
		elementStyle.BackColor = ጁ;
		elementStyle.BackColor2 = ገ;
		elementStyle.BackColorGradientAngle = ॷ;
		elementStyle.CornerDiameter = 4;
		elementStyle.CornerType = eCornerType.Square;
		elementStyle.BorderLeft = eStyleBorderType;
		elementStyle.BorderLeftWidth = 1;
		elementStyle.BorderTop = eStyleBorderType;
		elementStyle.BorderTopWidth = 1;
		elementStyle.BorderBottom = eStyleBorderType;
		elementStyle.BorderBottomWidth = 1;
		elementStyle.BorderRight = eStyleBorderType;
		elementStyle.BorderRightWidth = 1;
		elementStyle.BorderColor = ዾ;
		elementStyle.PaddingBottom = 1;
		elementStyle.PaddingLeft = 1;
		elementStyle.PaddingRight = 1;
		elementStyle.PaddingTop = 1;
		elementStyle.TextColor = ໆ;
		return elementStyle;
	}

	public static Node FindNodeForControl(AdvTree tree, Control c)
	{
		if (tree == null || c == null || tree.Nodes.Count == 0)
		{
			return null;
		}
		for (Node node = tree.Nodes[0]; node != null; node = node.NextVisibleNode)
		{
			foreach (Cell cell in node.Cells)
			{
				if (cell.HostedControl == c)
				{
					return node;
				}
			}
		}
		return null;
	}

	internal static int _17D2(string ዲ, string _17D3)
	{
		return string.Compare(ዲ, _17D3, StringComparison.CurrentCulture);
	}

	internal static bool _17D4(string ٮ)
	{
		if (ٮ.Length > 0 && char.IsDigit(ٮ[0]))
		{
			return true;
		}
		return false;
	}

	internal static int _17D5(string ዲ, string _17D3)
	{
		if (_17D4(ዲ) && _17D4(_17D3))
		{
			long num = _17D6(ዲ);
			long value = _17D6(_17D3);
			int num2 = num.CompareTo(value);
			if (num2 != 0)
			{
				return num2;
			}
		}
		return string.Compare(ዲ, _17D3, StringComparison.CurrentCulture);
	}

	internal static long _17D6(string ٮ)
	{
		long result = 0L;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < ٮ.Length && char.IsDigit(ٮ[i]); i++)
		{
			num2 = i;
		}
		long.TryParse(ٮ.Substring(num, num2 - num + 1), out result);
		return result;
	}

	internal static string ៗ(string _065C)
	{
		string text = "";
		string text2 = _17CE;
		string text3 = _17CF;
		for (int i = 0; i < _065C.Length; i++)
		{
			if (_065C[i].ToString() == text2 || _065C[i].ToString() == text3 || (_065C[i] >= '0' && _065C[i] <= '9') || (i == 0 && _065C[i] == '-'))
			{
				text += _065C[i];
			}
			else if (text.Length > 0)
			{
				break;
			}
		}
		return text;
	}
}
