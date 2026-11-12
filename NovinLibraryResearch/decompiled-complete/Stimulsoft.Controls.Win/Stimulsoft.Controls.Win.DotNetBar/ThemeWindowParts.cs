namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeWindowParts : ThemePart
{
	public static ThemeWindowParts Root = new ThemeWindowParts(0);

	public static ThemeWindowParts Caption = new ThemeWindowParts(1);

	public static ThemeWindowParts SmallCaption = new ThemeWindowParts(2);

	public static ThemeWindowParts MinCaption = new ThemeWindowParts(3);

	public static ThemeWindowParts SmallMinCaption = new ThemeWindowParts(4);

	public static ThemeWindowParts MaxCaption = new ThemeWindowParts(5);

	public static ThemeWindowParts SmallMaxCaption = new ThemeWindowParts(6);

	public static ThemeWindowParts FrameLeft = new ThemeWindowParts(7);

	public static ThemeWindowParts FrameRight = new ThemeWindowParts(8);

	public static ThemeWindowParts FrameBottom = new ThemeWindowParts(9);

	public static ThemeWindowParts SmallFrameLeft = new ThemeWindowParts(10);

	public static ThemeWindowParts SmallFrameRight = new ThemeWindowParts(11);

	public static ThemeWindowParts SmallFrameBottom = new ThemeWindowParts(12);

	public static ThemeWindowParts SysButton = new ThemeWindowParts(13);

	public static ThemeWindowParts MdiSysButton = new ThemeWindowParts(14);

	public static ThemeWindowParts MdiMinButton = new ThemeWindowParts(16);

	public static ThemeWindowParts MaxButton = new ThemeWindowParts(17);

	public static ThemeWindowParts CloseButton = new ThemeWindowParts(18);

	public static ThemeWindowParts SmallCloseButton = new ThemeWindowParts(19);

	public static ThemeWindowParts MdiCloseButton = new ThemeWindowParts(20);

	public static ThemeWindowParts RestoreButton = new ThemeWindowParts(21);

	public static ThemeWindowParts MdiRestoreButton = new ThemeWindowParts(22);

	public static ThemeWindowParts HelpButton = new ThemeWindowParts(23);

	public static ThemeWindowParts MdiHelpButton = new ThemeWindowParts(24);

	public static ThemeWindowParts HorzScroll = new ThemeWindowParts(25);

	public static ThemeWindowParts HorzThumb = new ThemeWindowParts(26);

	public static ThemeWindowParts VertScroll = new ThemeWindowParts(27);

	public static ThemeWindowParts VertThumb = new ThemeWindowParts(28);

	public static ThemeWindowParts Dialog = new ThemeWindowParts(29);

	public static ThemeWindowParts HtCaptionSizingTemplate = new ThemeWindowParts(30);

	public static ThemeWindowParts HtSmallCaptionSizingTemplate = new ThemeWindowParts(31);

	public static ThemeWindowParts HtFrameLeftSizingTemplate = new ThemeWindowParts(32);

	public static ThemeWindowParts HtSmallFrameLeftSizingTemplate = new ThemeWindowParts(33);

	public static ThemeWindowParts HtFrameRightSizingTemplate = new ThemeWindowParts(34);

	public static ThemeWindowParts HtSmallFrameRightSizingTemplate = new ThemeWindowParts(35);

	public static ThemeWindowParts HtFrameBottomSizingTemplate = new ThemeWindowParts(36);

	public static ThemeWindowParts HtSmallFrameBottomSizingTemplate = new ThemeWindowParts(37);

	protected ThemeWindowParts(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
