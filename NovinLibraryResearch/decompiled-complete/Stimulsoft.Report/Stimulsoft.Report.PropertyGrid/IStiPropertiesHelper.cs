namespace Stimulsoft.Report.PropertyGrid;

public interface IStiPropertiesHelper
{
	StiPropertyObject ChartOptions3D();

	StiPropertyObject SummarySortDirection();

	StiPropertyObject SummaryExpression();

	StiPropertyObject SummaryType();

	StiPropertyObject PaperSize();

	StiPropertyObject PageWidth();

	StiPropertyObject PageHeight();

	StiPropertyObject PageOrientation();

	StiPropertyObject Watermark();

	StiPropertyObject Margins();

	StiPropertyObject MirrorMargins();

	StiPropertyObject NumberOfCopies();

	StiPropertyObject StopBeforePrint();

	StiPropertyObject TitleBeforeHeader();

	StiPropertyObject StretchToPrintArea();

	StiPropertyObject UnlimitedHeight();

	StiPropertyObject UnlimitedBreakable();

	StiPropertyObject SegmentPerWidth();

	StiPropertyObject SegmentPerHeight();

	StiPropertyObject Columns();

	StiPropertyObject ColumnWidth();

	StiPropertyObject ColumnGaps();

	StiPropertyObject RightToLeft();

	StiPropertyObject Trimming();

	StiPropertyObject PageBrush();

	StiPropertyObject Border();

	StiPropertyObject Conditions();

	StiPropertyObject ComponentStyle();

	StiPropertyObject OddStyle();

	StiPropertyObject EvenStyle();

	StiPropertyObject Enabled();

	StiPropertyObject UpperMarks();

	StiPropertyObject InteractionEditor();

	StiPropertyObject PrintOnPreviousPage();

	StiPropertyObject PrintHeadersFootersFromPreviousPage();

	StiPropertyObject ResetPageNumber();

	StiPropertyObject Name();

	StiPropertyObject Alias();

	StiPropertyObject ConnectOnStart();

	StiPropertyObject Path();

	StiPropertyObject CodePage();

	StiPropertyObject Separator();

	StiPropertyObject ConvertEmptyStringToNull();

	StiPropertyObject FilterOn();

	StiPropertyObject AllowExpressions();

	StiPropertyObject DataSourceType();

	StiPropertyObject CommandTimeout();

	StiPropertyObject ReconnectOnEachRow();

	StiPropertyObject SqlCommand();

	StiPropertyObject Query();

	StiPropertyObject PathData();

	StiPropertyObject PathSchema();

	StiPropertyObject XmlType();

	StiPropertyObject ConnectionString();

	StiPropertyObject PageName();

	StiPropertyObject PageAlias();

	StiPropertyObject PageIcon();

	StiPropertyObject Category();

	StiPropertyObject LargeHeight();

	StiPropertyObject LargeHeightFactor();

	StiPropertyObject ExcelSheet();

	StiPropertyObject Text();

	StiPropertyObject TextEditor();

	StiPropertyObject TextBrush();

	StiPropertyObject Font();

	StiPropertyObject HorAlignment();

	StiPropertyObject BackgroundColor();

	StiPropertyObject TextColor();

	StiPropertyObject VertAlignment();

	StiPropertyObject ShowLabelText();

	StiPropertyObject ShowQuickButtons();

	StiPropertyObject TextFormat();

	StiPropertyObject TextAngle();

	StiPropertyObject HideZeros();

	StiPropertyObject WordWrap();

	StiPropertyObject Wysiwyg();

	StiPropertyObject FullConvertExpression();

	StiPropertyObject AllowHtmlTags();

	StiPropertyObject Editable();

	StiPropertyObject LinesOfUnderline();

	StiPropertyObject LineSpacing();

	StiPropertyObject MaxNumberOfLines();

	StiPropertyObject OnlyText();

	StiPropertyObject DetectUrls();

	StiPropertyObject ProcessingDuplicates();

	StiPropertyObject TextQuality();

	StiPropertyObject ProcessAtEnd();

	StiPropertyObject ShrinkFontToFit();

	StiPropertyObject ShrinkFontToFitMinimumSize();

	StiPropertyObject TextOptions();

	StiPropertyObject Left();

	StiPropertyObject Top();

	StiPropertyObject Width();

	StiPropertyObject Height();

	StiPropertyObject MinSize();

	StiPropertyObject MaxSize();

	StiPropertyObject Brush();

	StiPropertyObject NegativeTextBrush();

	StiPropertyObject CapBrush();

	StiPropertyObject CapBorderBrush();

	StiPropertyObject AutoWidth();

	StiPropertyObject UseParentStyles();

	StiPropertyObject CanGrow();

	StiPropertyObject CalcInvisible();

	StiPropertyObject CanShrink();

	StiPropertyObject GrowToHeight();

	StiPropertyObject CanBreak();

	StiPropertyObject DockStyle();

	StiPropertyObject PrintOn();

	StiPropertyObject Printable();

	StiPropertyObject AnchorMode();

	StiPropertyObject ShiftMode();

	StiPropertyObject GlobalizedName();

	StiPropertyObject Restrictions();

	StiPropertyObject Locked();

	StiPropertyObject Linked();

	StiPropertyObject ExcelValue();

	StiPropertyObject ExportAsImage();

	StiPropertyObject ReportName();

	StiPropertyObject ReportAlias();

	StiPropertyObject ReportAuthor();

	StiPropertyObject ReportDescription();

	StiPropertyObject ReportIcon();

	StiPropertyObject ReportImage();

	StiPropertyObject ConvertNulls();

	StiPropertyObject ReportCulture();

	StiPropertyObject ReportEngineVersion();

	StiPropertyObject NumberOfPass();

	StiPropertyObject CacheTotals();

	StiPropertyObject PreviewSettings();

	StiPropertyObject ReportPrinterSettings();

	StiPropertyObject ReferencedAssemblies();

	StiPropertyObject ReportUnit();

	StiPropertyObject RetrieveOnlyUsedData();

	StiPropertyObject ScriptLanguage();

	StiPropertyObject ParametersOrientation();

	StiPropertyObject AutoLocalizeReportOnRun();

	StiPropertyObject CacheAllData();

	StiPropertyObject CalculationMode();

	StiPropertyObject Collate();

	StiPropertyObject GlobalizationStrings();

	StiPropertyObject ReportCacheMode();

	StiPropertyObject StopBeforePage();

	StiPropertyObject ReportStyles();

	StiPropertyObject RefreshTime();

	StiPropertyObject Legend();

	StiPropertyObject ClusteredColumnArea();

	StiPropertyObject PieArea();

	StiPropertyObject WaterfallArea();

	StiPropertyObject BoxAndWhiskerArea();

	StiPropertyObject PictorialArea();

	StiPropertyObject TreemapArea();

	StiPropertyObject FunnelArea();

	StiPropertyObject RadarAreaArea();

	StiPropertyObject RadarLineArea();

	StiPropertyObject RadarPointArea();

	StiPropertyObject StackedColumnArea();

	StiPropertyObject GridLines();

	StiPropertyObject Interlacing();

	StiPropertyObject XAxis();

	StiPropertyObject XTopAxis();

	StiPropertyObject StiYAxis();

	StiPropertyObject YRightAxis();

	StiPropertyObject RadarGridLines();

	StiPropertyObject XRadarAxis();

	StiPropertyObject YRadarAxis();

	StiPropertyObject SeriesNoneLabels();

	StiPropertyObject SeriesOutsideAxisLabels();

	StiPropertyObject SeriesCenterPieLabels();

	StiPropertyObject SeriesOutsidePieLabels();

	StiPropertyObject SeriesTwoColumnsPieLabels();

	StiPropertyObject SeriesCenterFunnelLabels();

	StiPropertyObject SeriesOutsideLeftFunnelLabels();

	StiPropertyObject SeriesOutsideRightFunnelLabels();

	StiPropertyObject SeriesInsideEndAxisLabels();

	StiPropertyObject SeriesInsideBaseAxisLabels();

	StiPropertyObject SeriesCenterAxisLabels();

	StiPropertyObject SeriesOutsideEndAxisLabels();

	StiPropertyObject SeriesOutsideBaseAxisLabels();

	StiPropertyObject SeriesLeftAxisLabels();

	StiPropertyObject SeriesValueAxisLabels();

	StiPropertyObject SeriesRightAxisLabels();

	StiPropertyObject SeriesCenterTreemapLabels();

	StiPropertyObject ChartStyle();

	StiPropertyObject AllowApplyStyle();

	StiPropertyObject Rotation();

	StiPropertyObject iHorSpacing();

	StiPropertyObject iVertSpacing();

	StiPropertyObject ChartTitle();

	StiPropertyObject ChartTable();

	StiPropertyObject ChartTopN();

	StiPropertyObject ChartTrendLine();

	StiPropertyObject DataSource();

	StiPropertyObject BusinessObject();

	StiPropertyObject DataRelation();

	StiPropertyObject MasterComponent();

	StiPropertyObject CountData();

	StiPropertyObject Filters();

	StiPropertyObject FilterEngine();

	StiPropertyObject FilterMode();

	StiPropertyObject Sort();

	StiPropertyObject BarCodeEditor();

	StiPropertyObject ForeColor();

	StiPropertyObject IconColor();

	StiPropertyObject BackColor();

	StiPropertyObject GlyphColor();

	StiPropertyObject HotBackColor();

	StiPropertyObject NegativeColor();

	StiPropertyObject PositiveColor();

	StiPropertyObject HotForeColor();

	StiPropertyObject DefaultColor();

	StiPropertyObject Checked();

	StiPropertyObject CheckStyleForTrue();

	StiPropertyObject Size();

	StiPropertyObject SpaceRatio();

	StiPropertyObject CheckStyleForFalse();

	StiPropertyObject ContourColor();

	StiPropertyObject CheckBoxValues();

	StiPropertyObject NewPageBefore();

	StiPropertyObject NewPageAfter();

	StiPropertyObject NewColumnBefore();

	StiPropertyObject NewColumnAfter();

	StiPropertyObject SkipFirst();

	StiPropertyObject LimitRows();

	StiPropertyObject BreakIfLessThan();

	StiPropertyObject MaxHeight();

	StiPropertyObject MinHeight();

	StiPropertyObject KeepChildTogether();

	StiPropertyObject PrintAtBottom();

	StiPropertyObject PrintIfParentDisabled();

	StiPropertyObject CloneEditor();

	StiPropertyObject KeepFooterTogether();

	StiPropertyObject PrintIfEmpty();

	StiPropertyObject PrintOnAllPages();

	StiPropertyObject PrintOnEvenOddPages();

	StiPropertyObject KeepHeaderTogether();

	StiPropertyObject KeepGroupFooterTogether();

	StiPropertyObject MinWidth();

	StiPropertyObject MaxWidth();

	StiPropertyObject StartNewPageIfLessThan();

	StiPropertyObject GroupHeaderEditor();

	StiPropertyObject GroupHeaderExpression();

	StiPropertyObject RenderTo();

	StiPropertyObject SortDirection();

	StiPropertyObject KeepDetailsTogether();

	StiPropertyObject KeepDetails();

	StiPropertyObject MinRowsInColumn();

	StiPropertyObject ColumnDirection();

	StiPropertyObject PrintIfDetailEmpty();

	StiPropertyObject SizeMode();

	StiPropertyObject KeepGroupHeaderTogether();

	StiPropertyObject KeepGroupTogether();

	StiPropertyObject Indent();

	StiPropertyObject Color();

	StiPropertyObject HeaderColor();

	StiPropertyObject HeaderForeground();

	StiPropertyObject FooterForeground();

	StiPropertyObject DataColor();

	StiPropertyObject DataForeground();

	StiPropertyObject GridColor();

	StiPropertyObject SizeFloat();

	StiPropertyObject Style();

	StiPropertyObject StartCap();

	StiPropertyObject EndCap();

	StiPropertyObject ImageEditor();

	StiPropertyObject RichTextDesign();

	StiPropertyObject DataColumn();

	StiPropertyObject AspectRatio();

	StiPropertyObject Stretch();

	StiPropertyObject Smoothing();

	StiPropertyObject ImageRotation();

	StiPropertyObject MultipleFactor();

	StiPropertyObject LeftSide();

	StiPropertyObject TopSide();

	StiPropertyObject RightSide();

	StiPropertyObject BottomSide();

	StiPropertyObject KeepReportSummaryTogether();

	StiPropertyObject Round();

	StiPropertyObject ShapeRound();

	StiPropertyObject ShapeEditor();

	StiPropertyObject BorderColor();

	StiPropertyObject BorderSize();

	StiPropertyObject ChartAreaBorderColor();

	StiPropertyObject ChartAreaBrush();

	StiPropertyObject SeriesLighting();

	StiPropertyObject SeriesShowShadow();

	StiPropertyObject KeepSubReportTogether();

	StiPropertyObject SubReportDesigner();

	StiPropertyObject TableAutoWidth();

	StiPropertyObject AutoWidthType();

	StiPropertyObject ColumnCount();

	StiPropertyObject RowCount();

	StiPropertyObject HeaderRowsCount();

	StiPropertyObject FooterRowsCount();

	StiPropertyObject DockableTable();

	StiPropertyObject HeaderPrintOn();

	StiPropertyObject HeaderCanGrow();

	StiPropertyObject HeaderCanShrink();

	StiPropertyObject HeaderCanBreak();

	StiPropertyObject HeaderPrintAtBottom();

	StiPropertyObject HeaderPrintIfEmpty();

	StiPropertyObject HeaderPrintOnAllPages();

	StiPropertyObject HeaderPrintOnEvenOddPages();

	StiPropertyObject FooterPrintOn();

	StiPropertyObject FooterCanGrow();

	StiPropertyObject FooterCanShrink();

	StiPropertyObject FooterCanBreak();

	StiPropertyObject FooterPrintAtBottom();

	StiPropertyObject FooterPrintIfEmpty();

	StiPropertyObject FooterPrintOnAllPages();

	StiPropertyObject FooterPrintOnEvenOddPages();

	StiPropertyObject CellDockStyle();

	StiPropertyObject CellType();

	StiPropertyObject ProcessAt();

	StiPropertyObject FixedWidth();

	StiPropertyObject CellWidth();

	StiPropertyObject CellHeight();

	StiPropertyObject HorSpacing();

	StiPropertyObject VertSpacing();

	StiPropertyObject ContinuousText();

	StiPropertyObject Code();

	StiPropertyObject Ratio();

	StiPropertyObject NameInSource();

	StiPropertyObject Type();

	StiPropertyObject StringDialogInfoKey();

	StiPropertyObject DialogInfoValue();

	StiPropertyObject GuidDialogInfoKey();

	StiPropertyObject CharDialogInfoKey();

	StiPropertyObject BoolDialogInfoKey();

	StiPropertyObject ImageDialogInfoKey();

	StiPropertyObject DateDialogInfoKey();

	StiPropertyObject TimeDialogInfoKey();

	StiPropertyObject DoubleDialogInfoKey();

	StiPropertyObject DecimalDialogInfoKey();

	StiPropertyObject LongDialogInfoKey();

	StiPropertyObject RangeStringFromDialogInfo();

	StiPropertyObject RangeStringToDialogInfo();

	StiPropertyObject RangeGuidFromDialogInfo();

	StiPropertyObject RangeGuidToDialogInfo();

	StiPropertyObject RangeCharFromDialogInfo();

	StiPropertyObject RangeCharToDialogInfo();

	StiPropertyObject RangeDateFromDialogInfo();

	StiPropertyObject RangeDateToDialogInfo();

	StiPropertyObject RangeTimeFromDialogInfo();

	StiPropertyObject RangeTimeToDialogInfo();

	StiPropertyObject RangeDoubleFromDialogInfo();

	StiPropertyObject RangeDoubleToDialogInfo();

	StiPropertyObject RangeDecimalFromDialogInfo();

	StiPropertyObject RangeDecimalToDialogInfo();

	StiPropertyObject RangeLongFromDialogInfo();

	StiPropertyObject RangeLongToDialogInfo();

	StiPropertyObject RangeExpressionFromDialogInfo();

	StiPropertyObject RangeExpressionToDialogInfo();

	StiPropertyObject StyleName();

	StiPropertyObject StyleCollectionName();

	StiPropertyObject Description();

	StiPropertyObject SimpleImage();

	StiPropertyObject AllowUseBorderFormatting();

	StiPropertyObject AllowUseBorderSidesFromLocation();

	StiPropertyObject AllowUseBorderSides();

	StiPropertyObject AllowUseBrush();

	StiPropertyObject AllowUseFont();

	StiPropertyObject AllowUseImage();

	StiPropertyObject AllowUseTextBrush();

	StiPropertyObject AllowUseNegativeTextBrush();

	StiPropertyObject AllowUseTextOptions();

	StiPropertyObject AllowUseTextFormat();

	StiPropertyObject AllowUseHorAlignment();

	StiPropertyObject AllowUseVertAlignment();

	StiPropertyObject BasicStyleColor();

	StiPropertyObject BrushType();

	StiPropertyObject AllowUseBackColor();

	StiPropertyObject AllowUseForeColor();

	StiPropertyObject OracleConnectionString();

	StiPropertyObject OracleOmitOracleConnectionName();

	StiPropertyObject OracleUnicode();

	StiPropertyObject OracleEnlist();

	StiPropertyObject OracleLoadBalanceTimeout();

	StiPropertyObject OracleMaxPoolSize();

	StiPropertyObject OracleMinPoolSize();

	StiPropertyObject OraclePooling();

	StiPropertyObject OracleIntegratedSecurity();

	StiPropertyObject OraclePassword();

	StiPropertyObject OraclePersistSecurityInfo();

	StiPropertyObject OracleUserID();

	StiPropertyObject OracleDataSource();

	StiPropertyObject Expression();

	StiPropertyObject StringExpression();

	StiPropertyObject DataParameterSize();

	StiPropertyObject DataParameterType();

	StiPropertyObject Format();

	StiPropertyObject SortBy();

	StiPropertyObject AutoSeriesKeyDataColumn();

	StiPropertyObject AutoSeriesColorDataColumn();

	StiPropertyObject AutoSeriesTitleDataColumn();

	StiPropertyObject LabelsOffset();

	StiPropertyObject ShowShadow();

	StiPropertyObject ChartAreaShowShadow();

	StiPropertyObject SeriesInteraction();

	StiPropertyObject Icon();

	StiPropertyObject ShowInLegend();

	StiPropertyObject ShowNulls();

	StiPropertyObject ShowSeriesLabels();

	StiPropertyObject Title();

	StiPropertyObject YAxis();

	StiPropertyObject ArgumentDataColumn();

	StiPropertyObject Argument();

	StiPropertyObject ListOfArguments();

	StiPropertyObject ValueDataColumn();

	StiPropertyObject ValueDataColumnEnd();

	StiPropertyObject Value();

	StiPropertyObject ValueOpen();

	StiPropertyObject ValueClose();

	StiPropertyObject ValueHigh();

	StiPropertyObject ValueLow();

	StiPropertyObject ValueEnd();

	StiPropertyObject ListOfValues();

	StiPropertyObject ListOfValuesOpen();

	StiPropertyObject ListOfValuesClose();

	StiPropertyObject ListOfValuesHigh();

	StiPropertyObject ListOfValuesLow();

	StiPropertyObject ListOfValuesEnd();

	StiPropertyObject WeightDataColumn();

	StiPropertyObject Weight();

	StiPropertyObject ListOfWeights();

	StiPropertyObject ShowZeros();

	StiPropertyObject fWidth();

	StiPropertyObject Lighting();

	StiPropertyObject LineColor();

	StiPropertyObject LineMarker();

	StiPropertyObject Marker();

	StiPropertyObject LineStyle();

	StiPropertyObject LineWidth();

	StiPropertyObject Tension();

	StiPropertyObject PointAtCenter();

	StiPropertyObject AllowApplyBorderColor();

	StiPropertyObject AllowApplyBrush();

	StiPropertyObject Diameter();

	StiPropertyObject CutPieList();

	StiPropertyObject Distance();

	StiPropertyObject StartAngle();

	StiPropertyObject MinValue();

	StiPropertyObject MaxValue();

	StiPropertyObject Orientation();

	StiPropertyObject ShowBehind();

	StiPropertyObject StripBrush();

	StiPropertyObject Visible();

	StiPropertyObject Antialiasing();

	StiPropertyObject TitleColor();

	StiPropertyObject TitleVisible();

	StiPropertyObject AxisValue();

	StiPropertyObject Position();

	StiPropertyObject TextNotEdit();

	StiPropertyObject StylesCollectionName();

	StiPropertyObject StyleConditions();

	StiPropertyObject Colors();

	StiPropertyObject StyleColors();

	StiPropertyObject Heatmap();

	StiPropertyObject HeatmapWithGroup();

	StiPropertyObject ValueDataColumnOpen();

	StiPropertyObject ValueDataColumnClose();

	StiPropertyObject ValueDataColumnHigh();

	StiPropertyObject ValueDataColumnLow();

	StiPropertyObject TopmostLine();

	StiPropertyObject MarkerBrush();

	StiPropertyObject KeyDataColumn();

	StiPropertyObject MasterKeyDataColumn();

	StiPropertyObject ParentValue();

	StiPropertyObject Headers();

	StiPropertyObject Footers();

	StiPropertyObject DisplayValue();

	StiPropertyObject SortType();

	StiPropertyObject fAngle();

	StiPropertyObject BarCodeAngle();

	StiPropertyObject AutoScale();

	StiPropertyObject EnumeratorSeparator();

	StiPropertyObject KeepMergedCellsTogether();

	StiPropertyObject EnumeratorType();

	StiPropertyObject MergeHeaders();

	StiPropertyObject ShowTotal();

	StiPropertyObject ShowPercents();

	StiPropertyObject Summary();

	StiPropertyObject SummaryValues();

	StiPropertyObject ImageHorAlignment();

	StiPropertyObject ImageVertAlignment();

	StiPropertyObject UseStyleOfSummaryInRowTotal();

	StiPropertyObject UseStyleOfSummaryInColumnTotal();

	StiPropertyObject EmptyValue();

	StiPropertyObject Wrap();

	StiPropertyObject WrapGap();

	StiPropertyObject PrintTitleOnAllPages();

	StiPropertyObject KeepCrossTabTogether();

	StiPropertyObject Zoom();

	StiPropertyObject RequestParameters();

	StiPropertyObject ChartEditor();

	StiPropertyObject DataBandEditor();

	StiPropertyObject CrossTabEditor();

	StiPropertyObject MapEditor();

	StiPropertyObject DataSourceEditor();

	StiPropertyObject ChartFilters();

	StiPropertyObject BrushNegative();

	StiPropertyObject AllowApplyBrushNegative();

	StiPropertyObject LineColorNegative();

	StiPropertyObject AllowApplyColorNegative();

	StiPropertyObject ShortValue();

	StiPropertyObject Minimum();

	StiPropertyObject Maximum();

	StiPropertyObject MajorInterval();

	StiPropertyObject MinorInterval();

	StiPropertyObject StartWidth();

	StiPropertyObject EndWidth();

	StiPropertyObject IsReversed();

	StiPropertyObject SweepAngle();

	StiPropertyObject Radius();

	StiPropertyObject RadiusMode();

	StiPropertyObject Skin();

	StiPropertyObject Center();

	StiPropertyObject LeftF();

	StiPropertyObject TopF();

	StiPropertyObject BorderBrush();

	StiPropertyObject RelativeHeight();

	StiPropertyObject NullableMaximumValue();

	StiPropertyObject NullableMinimumValue();

	StiPropertyObject Offset();

	StiPropertyObject OffsetNeedle();

	StiPropertyObject SkipIndices();

	StiPropertyObject SkipValues();

	StiPropertyObject OffsetAngle();

	StiPropertyObject NullablePlacement();

	StiPropertyObject Placement();

	StiPropertyObject RelativeWidth();

	StiPropertyObject BorderWidth();

	StiPropertyObject CapBorderWidth();

	StiPropertyObject SkipMajorValues();

	StiPropertyObject ValueExpr();

	StiPropertyObject LabelRotationMode();

	StiPropertyObject TextFormatStr();

	StiPropertyObject NullableOffset();

	StiPropertyObject NullableOffsetAngle();

	StiPropertyObject ValueF();

	StiPropertyObject NullableRelativeHeight();

	StiPropertyObject NullableRelativeWidth();

	StiPropertyObject NullableBorderWidth();

	StiPropertyObject NullableLabelRotationMode();

	StiPropertyObject TextStr();

	StiPropertyObject EndValue();

	StiPropertyObject StartValue();

	StiPropertyObject UseValuesFromTheSpecifiedRange();

	StiPropertyObject UseRangeColor();

	StiPropertyObject EmptyBrush();

	StiPropertyObject EmptyBorderBrush();

	StiPropertyObject EmptyBorderWidth();

	StiPropertyObject ShowValue();

	StiPropertyObject MapType();

	StiPropertyObject MapID();

	StiPropertyObject ShowLegend();

	StiPropertyObject AutoCalculateCenterPoint();

	StiPropertyObject CenterPoint();

	StiPropertyObject RangeColorMode();

	StiPropertyObject TickMarkMajorBorder();

	StiPropertyObject TickMarkMajorBrush();

	StiPropertyObject TickMarkMinorBorder();

	StiPropertyObject TickMarkMinorBrush();

	StiPropertyObject TickLabelMajorTextBrush();

	StiPropertyObject TickLabelMajorFont();

	StiPropertyObject TickLabelMinorTextBrush();

	StiPropertyObject TickLabelMinorFont();

	StiPropertyObject LinearBarBorderBrush();

	StiPropertyObject LinearBarBrush();

	StiPropertyObject LinearBarEmptyBorderBrush();

	StiPropertyObject LinearBarEmptyBrush();

	StiPropertyObject RadialBarBorderBrush();

	StiPropertyObject RadialBarBrush();

	StiPropertyObject RadialBarEmptyBorderBrush();

	StiPropertyObject RadialBarEmptyBrush();

	StiPropertyObject NeedleBorderBrush();

	StiPropertyObject NeedleBrush();

	StiPropertyObject NeedleCapBorderBrush();

	StiPropertyObject NeedleCapBrush();

	StiPropertyObject Direction();

	StiPropertyObject ArrowWidth();

	StiPropertyObject ArrowHeight();

	StiPropertyObject AutoSize();

	StiPropertyObject Bevel();

	StiPropertyObject Mode();

	StiPropertyObject ProcessTilde();

	StiPropertyObject TrimExcessData();

	StiPropertyObject StructuredAppendPosition();

	StiPropertyObject StructuredAppendTotal();

	StiPropertyObject Image();

	StiPropertyObject ImageMultipleFactor();

	StiPropertyObject Module();

	StiPropertyObject BarCodeChecksum();

	StiPropertyObject fHeight();

	StiPropertyObject fRatio();

	StiPropertyObject EncodingType();

	StiPropertyObject MatrixSize();

	StiPropertyObject UseRectangularSymbols();

	StiPropertyObject SupplementCode();

	StiPropertyObject ShowQuietZoneIndicator();

	StiPropertyObject SupplementType();

	StiPropertyObject AddClearZone();

	StiPropertyObject PrintVerticalBars();

	StiPropertyObject CheckSum1();

	StiPropertyObject CheckSum2();

	StiPropertyObject fAspectRatio();

	StiPropertyObject AutoDataColumns();

	StiPropertyObject AutoDataRows();

	StiPropertyObject DataColumns();

	StiPropertyObject DataRows();

	StiPropertyObject EncodingMode();

	StiPropertyObject ErrorsCorrectionLevel();

	StiPropertyObject RatioY();

	StiPropertyObject Space();

	StiPropertyObject CheckSum();

	StiPropertyObject CompanyPrefix();

	StiPropertyObject ExtensionDigit();

	StiPropertyObject SerialNumber();

	StiPropertyObject ErrorCorrectionLevel();

	StiPropertyObject Column();

	StiPropertyObject Label();

	StiPropertyObject ShowNullValues();

	StiPropertyObject TextAllignment();

	StiPropertyObject DimensionFunction();

	StiPropertyObject MeasureFunction();

	StiPropertyObject ShowTitles();

	StiPropertyObject TitleStr();

	StiPropertyObject SubTitle();

	StiPropertyObject Footnote();

	StiPropertyObject SeriesLabelsBorderColor();

	StiPropertyObject SeriesLabelsColor();

	StiPropertyObject SeriesLabelsBrush();

	StiPropertyObject TrendLineShowShadow();

	StiPropertyObject TrendLineColor();

	StiPropertyObject LegendBorderColor();

	StiPropertyObject LegendLabelsColor();

	StiPropertyObject LegendTitleColor();

	StiPropertyObject LegendBrush();

	StiPropertyObject AxisLabelsColor();

	StiPropertyObject AxisLineColor();

	StiPropertyObject AxisTitleColor();

	StiPropertyObject InterlacingHorBrush();

	StiPropertyObject InterlacingVertBrush();

	StiPropertyObject GridLinesHorColor();

	StiPropertyObject GridLinesVertColor();

	StiPropertyObject MapColorDataColumn();

	StiPropertyObject MapGroupDataColumn();

	StiPropertyObject MapKeyDataColumn();

	StiPropertyObject MapGssDataColumn();

	StiPropertyObject MapNameDataColumn();

	StiPropertyObject MapValueDataColumn();

	StiPropertyObject AvailableinTheViewer();

	StiPropertyObject LaTexExpression();
}
