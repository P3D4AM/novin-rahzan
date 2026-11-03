using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.BarCodes;

public class StiGS1_128BarCodeType : StiCode128BarCodeType
{
	public class StiGS1ApplicationIdentifierItem
	{
		public string AICode;

		public int AILength;

		public int NumericMin;

		public int NumericMax;

		public int CharacterMin;

		public int CharacterMax;

		public bool NeedFNC1;

		public string DataTitle;

		public string DataContent;

		public StiGS1ApplicationIdentifierItem(string AICode, int AILength, int NumericMin, int NumericMax, int CharacterMin, int CharacterMax, bool NeedFNC1, string DataTitle, string DataContent)
		{
			this.AICode = AICode;
			this.AILength = AILength;
			this.NumericMin = NumericMin;
			this.NumericMax = NumericMax;
			this.CharacterMin = CharacterMin;
			this.CharacterMax = CharacterMax;
			this.NeedFNC1 = NeedFNC1;
			this.DataTitle = DataTitle;
			this.DataContent = DataContent;
		}
	}

	public static StiGS1ApplicationIdentifierItem[] GS1ApplicationIdentifiers = new StiGS1ApplicationIdentifierItem[142]
	{
		new StiGS1ApplicationIdentifierItem("00", 2, 18, 18, 0, 0, NeedFNC1: false, "SSCC", "Serial Shipping Container Code (SSCC)"),
		new StiGS1ApplicationIdentifierItem("01", 2, 14, 14, 0, 0, NeedFNC1: false, "GTIN", "Global Trade Item Number (GTIN)"),
		new StiGS1ApplicationIdentifierItem("02", 2, 14, 14, 0, 0, NeedFNC1: false, "CONTENT", "GTIN of contained trade items"),
		new StiGS1ApplicationIdentifierItem("10", 2, 0, 0, 0, 20, NeedFNC1: true, "BATCH/LOT", "Batch or lot number"),
		new StiGS1ApplicationIdentifierItem("11", 2, 6, 6, 0, 0, NeedFNC1: false, "PROD DATE", "Production date (YYMMDD)"),
		new StiGS1ApplicationIdentifierItem("12", 2, 6, 6, 0, 0, NeedFNC1: false, "DUE DATE", "Due date (YYMMDD)"),
		new StiGS1ApplicationIdentifierItem("13", 2, 6, 6, 0, 0, NeedFNC1: false, "PACK DATE", "Packaging date (YYMMDD)"),
		new StiGS1ApplicationIdentifierItem("15", 2, 6, 6, 0, 0, NeedFNC1: false, "BEST BEFORE or BEST BY", "Best before date (YYMMDD)"),
		new StiGS1ApplicationIdentifierItem("16", 2, 6, 6, 0, 0, NeedFNC1: false, "SELL BY", "Sell by date (YYMMDD)"),
		new StiGS1ApplicationIdentifierItem("17", 2, 6, 6, 0, 0, NeedFNC1: false, "USE BY OR EXPIRY", "Expiration date (YYMMDD)"),
		new StiGS1ApplicationIdentifierItem("20", 2, 2, 2, 0, 0, NeedFNC1: false, "VARIANT", "Variant number"),
		new StiGS1ApplicationIdentifierItem("21", 2, 0, 0, 0, 20, NeedFNC1: true, "SERIAL", "Serial number"),
		new StiGS1ApplicationIdentifierItem("240", 3, 0, 0, 0, 30, NeedFNC1: true, "ADDITIONAL ID", "Additional item identification"),
		new StiGS1ApplicationIdentifierItem("241", 3, 0, 0, 0, 30, NeedFNC1: true, "CUST. PART NO.", "Customer part number"),
		new StiGS1ApplicationIdentifierItem("242", 3, 0, 6, 0, 0, NeedFNC1: true, "MTO VARIANT", "Made-to-Order variation number"),
		new StiGS1ApplicationIdentifierItem("243", 3, 0, 0, 0, 20, NeedFNC1: true, "PCN", "Packaging component number"),
		new StiGS1ApplicationIdentifierItem("250", 3, 0, 0, 0, 30, NeedFNC1: true, "SECONDARY SERIAL", "Secondary serial number"),
		new StiGS1ApplicationIdentifierItem("251", 3, 0, 0, 0, 30, NeedFNC1: true, "REF. TO SOURCE", "Reference to source entity"),
		new StiGS1ApplicationIdentifierItem("253", 3, 13, 13, 0, 17, NeedFNC1: true, "GDTI", "Global Document Type Identifier (GDTI)"),
		new StiGS1ApplicationIdentifierItem("254", 3, 0, 0, 0, 20, NeedFNC1: true, "GLN EXTENSION COMPONENT", "GLN extension component"),
		new StiGS1ApplicationIdentifierItem("255", 3, 13, 25, 0, 0, NeedFNC1: true, "GCN", "Global Coupon Number (GCN)"),
		new StiGS1ApplicationIdentifierItem("30", 2, 0, 8, 0, 0, NeedFNC1: true, "VAR. COUNT", "Count of items (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("310", 4, 6, 6, 0, 0, NeedFNC1: false, "NET WEIGHT (kg)", "Net weight, kilograms (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("311", 4, 6, 6, 0, 0, NeedFNC1: false, "LENGTH (m)", "Length or first dimension, metres (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("312", 4, 6, 6, 0, 0, NeedFNC1: false, "WIDTH (m)", "Width, diameter, or second dimension, metres (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("313", 4, 6, 6, 0, 0, NeedFNC1: false, "HEIGHT (m)", "Depth, thickness, height, or third dimension, metres (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("314", 4, 6, 6, 0, 0, NeedFNC1: false, "AREA (m2)", "Area, square metres (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("315", 4, 6, 6, 0, 0, NeedFNC1: false, "NET VOLUME (l)", "Net volume, litres (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("316", 4, 6, 6, 0, 0, NeedFNC1: false, "NET VOLUME (m3)", "Net volume, cubic metres (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("320", 4, 6, 6, 0, 0, NeedFNC1: false, "NET WEIGHT (lb)", "Net weight, pounds (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("321", 4, 6, 6, 0, 0, NeedFNC1: false, "LENGTH (i)", "Length or first dimension, inches (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("322", 4, 6, 6, 0, 0, NeedFNC1: false, "LENGTH (f)", "Length or first dimension, feet (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("323", 4, 6, 6, 0, 0, NeedFNC1: false, "LENGTH (y)", "Length or first dimension, yards (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("324", 4, 6, 6, 0, 0, NeedFNC1: false, "WIDTH (i)", "Width, diameter, or second dimension, inches (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("325", 4, 6, 6, 0, 0, NeedFNC1: false, "WIDTH (f)", "Width, diameter, or second dimension, feet (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("326", 4, 6, 6, 0, 0, NeedFNC1: false, "WIDTH (y)", "Width, diameter, or second dimension, yards (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("327", 4, 6, 6, 0, 0, NeedFNC1: false, "HEIGHT (i)", "Depth, thickness, height, or third dimension, inches (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("328", 4, 6, 6, 0, 0, NeedFNC1: false, "HEIGHT (f)", "Depth, thickness, height, or third dimension, feet (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("329", 4, 6, 6, 0, 0, NeedFNC1: false, "HEIGHT (y)", "Depth, thickness, height, or third dimension, yards (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("330", 4, 6, 6, 0, 0, NeedFNC1: false, "GROSS WEIGHT (kg)", "Logistic weight, kilograms"),
		new StiGS1ApplicationIdentifierItem("331", 4, 6, 6, 0, 0, NeedFNC1: false, "LENGTH (m), log", "Length or first dimension, metres"),
		new StiGS1ApplicationIdentifierItem("332", 4, 6, 6, 0, 0, NeedFNC1: false, "WIDTH (m), log", "Width, diameter, or second dimension, metres"),
		new StiGS1ApplicationIdentifierItem("333", 4, 6, 6, 0, 0, NeedFNC1: false, "HEIGHT (m), log", "Depth, thickness, height, or third dimension, metres"),
		new StiGS1ApplicationIdentifierItem("334", 4, 6, 6, 0, 0, NeedFNC1: false, "AREA (m2), log", "Area, square metres"),
		new StiGS1ApplicationIdentifierItem("335", 4, 6, 6, 0, 0, NeedFNC1: false, "VOLUME (l), log", "Logistic volume, litres"),
		new StiGS1ApplicationIdentifierItem("336", 4, 6, 6, 0, 0, NeedFNC1: false, "VOLUME (m3), log", "Logistic volume, cubic metres"),
		new StiGS1ApplicationIdentifierItem("337", 4, 6, 6, 0, 0, NeedFNC1: false, "KG PER m²", "Kilograms per square metre"),
		new StiGS1ApplicationIdentifierItem("340", 4, 6, 6, 0, 0, NeedFNC1: false, "GROSS WEIGHT (lb)", "Logistic weight, pounds"),
		new StiGS1ApplicationIdentifierItem("341", 4, 6, 6, 0, 0, NeedFNC1: false, "LENGTH (i), log", "Length or first dimension, inches"),
		new StiGS1ApplicationIdentifierItem("342", 4, 6, 6, 0, 0, NeedFNC1: false, "LENGTH (f), log", "Length or first dimension, feet"),
		new StiGS1ApplicationIdentifierItem("343", 4, 6, 6, 0, 0, NeedFNC1: false, "LENGTH (y), log", "Length or first dimension, yards"),
		new StiGS1ApplicationIdentifierItem("344", 4, 6, 6, 0, 0, NeedFNC1: false, "WIDTH (i), log", "Width, diameter, or second dimension, inches"),
		new StiGS1ApplicationIdentifierItem("345", 4, 6, 6, 0, 0, NeedFNC1: false, "WIDTH (f), log", "Width, diameter, or second dimension, feet"),
		new StiGS1ApplicationIdentifierItem("346", 4, 6, 6, 0, 0, NeedFNC1: false, "WIDTH (y), log", "Width, diameter, or second dimension, yard"),
		new StiGS1ApplicationIdentifierItem("347", 4, 6, 6, 0, 0, NeedFNC1: false, "HEIGHT (i), log", "Depth, thickness, height, or third dimension, inches"),
		new StiGS1ApplicationIdentifierItem("348", 4, 6, 6, 0, 0, NeedFNC1: false, "HEIGHT (f), log", "Depth, thickness, height, or third dimension, feet"),
		new StiGS1ApplicationIdentifierItem("349", 4, 6, 6, 0, 0, NeedFNC1: false, "HEIGHT (y), log", "Depth, thickness, height, or third dimension, yards"),
		new StiGS1ApplicationIdentifierItem("350", 4, 6, 6, 0, 0, NeedFNC1: false, "AREA (i2)", "Area, square inches (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("351", 4, 6, 6, 0, 0, NeedFNC1: false, "AREA (f2)", "Area, square feet (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("352", 4, 6, 6, 0, 0, NeedFNC1: false, "AREA (y2)", "Area, square yards (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("353", 4, 6, 6, 0, 0, NeedFNC1: false, "AREA (i2), log", "Area, square inches"),
		new StiGS1ApplicationIdentifierItem("354", 4, 6, 6, 0, 0, NeedFNC1: false, "AREA (f2), log", "Area, square feet"),
		new StiGS1ApplicationIdentifierItem("355", 4, 6, 6, 0, 0, NeedFNC1: false, "AREA (y2), log", "Area, square yards"),
		new StiGS1ApplicationIdentifierItem("356", 4, 6, 6, 0, 0, NeedFNC1: false, "NET WEIGHT (t)", "Net weight, troy ounces (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("357", 4, 6, 6, 0, 0, NeedFNC1: false, "NET VOLUME (oz)", "Net weight (or volume), ounces (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("360", 4, 6, 6, 0, 0, NeedFNC1: false, "NET VOLUME (q)", "Net volume, quarts (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("361", 4, 6, 6, 0, 0, NeedFNC1: false, "NET VOLUME (g)", "Net volume, gallons U.S. (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("362", 4, 6, 6, 0, 0, NeedFNC1: false, "VOLUME (q), log", "Logistic volume, quarts"),
		new StiGS1ApplicationIdentifierItem("363", 4, 6, 6, 0, 0, NeedFNC1: false, "VOLUME (g), log", "Logistic volume, gallons U.S."),
		new StiGS1ApplicationIdentifierItem("364", 4, 6, 6, 0, 0, NeedFNC1: false, "VOLUME (i3)", "Net volume, cubic inches (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("365", 4, 6, 6, 0, 0, NeedFNC1: false, "VOLUME (f3)", "Net volume, cubic feet (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("366", 4, 6, 6, 0, 0, NeedFNC1: false, "VOLUME (y3)", "Net volume, cubic yards (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("367", 4, 6, 6, 0, 0, NeedFNC1: false, "VOLUME (i3), log", "Logistic volume, cubic inches"),
		new StiGS1ApplicationIdentifierItem("368", 4, 6, 6, 0, 0, NeedFNC1: false, "VOLUME (f3), log", "Logistic volume, cubic feet"),
		new StiGS1ApplicationIdentifierItem("369", 4, 6, 6, 0, 0, NeedFNC1: false, "VOLUME (y3), log", "Logistic volume, cubic yards"),
		new StiGS1ApplicationIdentifierItem("37", 2, 0, 8, 0, 0, NeedFNC1: true, "COUNT", "Count of trade items"),
		new StiGS1ApplicationIdentifierItem("390", 4, 0, 15, 0, 0, NeedFNC1: true, "AMOUNT", "Applicable amount payable or Coupon value, local currency"),
		new StiGS1ApplicationIdentifierItem("391", 4, 3, 18, 0, 0, NeedFNC1: true, "AMOUNT", "Applicable amount payable with ISO currency code"),
		new StiGS1ApplicationIdentifierItem("392", 4, 0, 15, 0, 0, NeedFNC1: true, "PRICE", "Applicable amount payable, single monetary area (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("393", 4, 3, 18, 0, 0, NeedFNC1: true, "PRICE", "Applicable amount payable with ISO currency code (variable measure trade item)"),
		new StiGS1ApplicationIdentifierItem("394", 4, 4, 4, 0, 0, NeedFNC1: true, "PRCNT OFF", "Percentage discount of a coupon"),
		new StiGS1ApplicationIdentifierItem("400", 3, 0, 0, 0, 30, NeedFNC1: true, "ORDER NUMBER", "Customer's purchase order number"),
		new StiGS1ApplicationIdentifierItem("401", 3, 0, 0, 0, 30, NeedFNC1: true, "GINC", "Global Identification Number for Consignment (GINC)"),
		new StiGS1ApplicationIdentifierItem("402", 3, 17, 17, 0, 0, NeedFNC1: true, "GSIN", "Global Shipment Identification Number (GSIN)"),
		new StiGS1ApplicationIdentifierItem("403", 3, 0, 0, 0, 30, NeedFNC1: true, "ROUTE", "Routing code"),
		new StiGS1ApplicationIdentifierItem("410", 3, 13, 13, 0, 0, NeedFNC1: false, "SHIP TO LOC", "Ship to - Deliver to Global Location Number"),
		new StiGS1ApplicationIdentifierItem("411", 3, 13, 13, 0, 0, NeedFNC1: false, "BILL TO", "Bill to - Invoice to Global Location Number"),
		new StiGS1ApplicationIdentifierItem("412", 3, 13, 13, 0, 0, NeedFNC1: false, "PURCHASE FROM", "Purchased from Global Location Number"),
		new StiGS1ApplicationIdentifierItem("413", 3, 13, 13, 0, 0, NeedFNC1: false, "SHIP FOR LOC", "Ship for - Deliver for - Forward to Global Location Number"),
		new StiGS1ApplicationIdentifierItem("414", 3, 13, 13, 0, 0, NeedFNC1: false, "LOC No", "Identification of a physical location - Global Location Number"),
		new StiGS1ApplicationIdentifierItem("415", 3, 13, 13, 0, 0, NeedFNC1: false, "PAY TO", "Global Location Number of the invoicing party"),
		new StiGS1ApplicationIdentifierItem("420", 3, 0, 0, 0, 20, NeedFNC1: true, "SHIP TO POST", "Ship to - Deliver to postal code within a single postal authority"),
		new StiGS1ApplicationIdentifierItem("421", 3, 3, 3, 0, 9, NeedFNC1: true, "SHIP TO POST", "Ship to - Deliver to postal code with ISO country code"),
		new StiGS1ApplicationIdentifierItem("422", 3, 3, 3, 0, 0, NeedFNC1: true, "ORIGIN", "Country of origin of a trade item"),
		new StiGS1ApplicationIdentifierItem("423", 3, 3, 15, 0, 0, NeedFNC1: true, "COUNTRY - INITIAL PROCESS.", "Country of initial processing"),
		new StiGS1ApplicationIdentifierItem("424", 3, 3, 3, 0, 0, NeedFNC1: true, "COUNTRY - PROCESS.", "Country of processing"),
		new StiGS1ApplicationIdentifierItem("425", 3, 3, 3, 0, 0, NeedFNC1: true, "COUNTRY - DISASSEMBLY", "Country of disassembly"),
		new StiGS1ApplicationIdentifierItem("426", 3, 3, 3, 0, 0, NeedFNC1: true, "COUNTRY – FULL PROCESS", "Country covering full process chain"),
		new StiGS1ApplicationIdentifierItem("427", 3, 0, 0, 0, 3, NeedFNC1: true, "ORIGIN SUBDIVISION", "Country subdivision Of origin"),
		new StiGS1ApplicationIdentifierItem("7001", 4, 13, 13, 0, 0, NeedFNC1: true, "NSN", "NATO Stock Number (NSN)"),
		new StiGS1ApplicationIdentifierItem("7002", 4, 0, 0, 0, 30, NeedFNC1: true, "MEAT CUT", "UN/ECE meat carcasses and cuts classification"),
		new StiGS1ApplicationIdentifierItem("7003", 4, 10, 10, 0, 0, NeedFNC1: true, "EXPIRY TIME", "Expiration date and time"),
		new StiGS1ApplicationIdentifierItem("7004", 4, 0, 4, 0, 0, NeedFNC1: true, "ACTIVE POTENCY", "Active potency"),
		new StiGS1ApplicationIdentifierItem("7005", 4, 0, 0, 0, 12, NeedFNC1: true, "CATCH AREA", "Catch area"),
		new StiGS1ApplicationIdentifierItem("7006", 4, 6, 6, 0, 0, NeedFNC1: true, "FIRST FREEZE DATE", "First freeze date"),
		new StiGS1ApplicationIdentifierItem("7007", 4, 6, 12, 0, 0, NeedFNC1: true, "HARVEST DATE", "Harvest date"),
		new StiGS1ApplicationIdentifierItem("7008", 4, 0, 0, 0, 3, NeedFNC1: true, "AQUATIC SPECIES", "Species for fishery purposes"),
		new StiGS1ApplicationIdentifierItem("7009", 4, 0, 0, 0, 10, NeedFNC1: true, "FISHING GEAR TYPE", "Fishing gear type"),
		new StiGS1ApplicationIdentifierItem("7010", 4, 0, 0, 0, 2, NeedFNC1: true, "PROD METHOD", "Production method"),
		new StiGS1ApplicationIdentifierItem("703", 4, 3, 3, 0, 27, NeedFNC1: true, "PROCESSOR # s", "Number of processor with ISO Country Code"),
		new StiGS1ApplicationIdentifierItem("710", 3, 0, 0, 0, 20, NeedFNC1: true, "NHRN PZN", "National Healthcare Reimbursement Number (NHRN) – Germany PZN"),
		new StiGS1ApplicationIdentifierItem("711", 3, 0, 0, 0, 20, NeedFNC1: true, "NHRN CIP", "National Healthcare Reimbursement Number (NHRN) – France CIP"),
		new StiGS1ApplicationIdentifierItem("712", 3, 0, 0, 0, 20, NeedFNC1: true, "NHRN CN", "National Healthcare Reimbursement Number (NHRN) – Spain CN"),
		new StiGS1ApplicationIdentifierItem("713", 3, 0, 0, 0, 20, NeedFNC1: true, "NHRN DRN", "National Healthcare Reimbursement Number (NHRN) – Brasil DRN"),
		new StiGS1ApplicationIdentifierItem("8001", 4, 14, 14, 0, 0, NeedFNC1: true, "DIMENSIONS", "Roll products (width, length, core diameter, direction, splices)"),
		new StiGS1ApplicationIdentifierItem("8002", 4, 0, 0, 0, 20, NeedFNC1: true, "CMT No", "Cellular mobile telephone identifier"),
		new StiGS1ApplicationIdentifierItem("8003", 4, 14, 14, 0, 16, NeedFNC1: true, "GRAI", "Global Returnable Asset Identifier (GRAI)"),
		new StiGS1ApplicationIdentifierItem("8004", 4, 0, 0, 0, 30, NeedFNC1: true, "GIAI", "Global Individual Asset Identifier (GIAI)"),
		new StiGS1ApplicationIdentifierItem("8005", 4, 6, 6, 0, 0, NeedFNC1: true, "PRICE PER UNIT", "Price per unit of measure"),
		new StiGS1ApplicationIdentifierItem("8006", 4, 18, 18, 0, 0, NeedFNC1: true, "GCTIN", "Identification of the components of a trade item"),
		new StiGS1ApplicationIdentifierItem("8007", 4, 0, 0, 0, 34, NeedFNC1: true, "IBAN", "International Bank Account Number (IBAN)"),
		new StiGS1ApplicationIdentifierItem("8008", 4, 8, 12, 0, 0, NeedFNC1: true, "PROD TIME", "Date and time of production"),
		new StiGS1ApplicationIdentifierItem("8010", 4, 0, 0, 0, 30, NeedFNC1: true, "CPID", "Component / Part Identifier (CPID)"),
		new StiGS1ApplicationIdentifierItem("8011", 4, 0, 12, 0, 0, NeedFNC1: true, "CPID SERIAL", "Component / Part Identifier serial number (CPID SERIAL)"),
		new StiGS1ApplicationIdentifierItem("8012", 4, 0, 0, 0, 20, NeedFNC1: true, "VERSION", "Software version"),
		new StiGS1ApplicationIdentifierItem("8017", 4, 18, 18, 0, 0, NeedFNC1: true, "GSRN - PROVIDER", "Global Service Relation Number to identify the relationship between an organisation offering services and the provider of services"),
		new StiGS1ApplicationIdentifierItem("8018", 4, 18, 18, 0, 0, NeedFNC1: true, "GSRN - RECIPIENT", "Global Service Relation Number to identify the relationship between an organisation offering services and the recipient of services"),
		new StiGS1ApplicationIdentifierItem("8019", 4, 0, 10, 0, 0, NeedFNC1: true, "SRIN", "Service Relation Instance Number (SRIN)"),
		new StiGS1ApplicationIdentifierItem("8020", 4, 0, 0, 0, 25, NeedFNC1: true, "REF No", "Payment slip reference number"),
		new StiGS1ApplicationIdentifierItem("8110", 4, 0, 0, 0, 70, NeedFNC1: true, "-", "Coupon code identification for use in North America"),
		new StiGS1ApplicationIdentifierItem("8111", 4, 4, 4, 0, 0, NeedFNC1: true, "POINTS", "Loyalty points of a coupon"),
		new StiGS1ApplicationIdentifierItem("8200", 4, 0, 0, 0, 70, NeedFNC1: true, "PRODUCT URL", "Extended Packaging URL"),
		new StiGS1ApplicationIdentifierItem("90", 2, 0, 0, 0, 30, NeedFNC1: true, "INTERNAL", "Information mutually agreed between trading partners"),
		new StiGS1ApplicationIdentifierItem("91", 2, 0, 0, 0, 30, NeedFNC1: true, "INTERNAL", "Company internal information"),
		new StiGS1ApplicationIdentifierItem("92", 2, 0, 0, 0, 30, NeedFNC1: true, "INTERNAL", "Company internal information"),
		new StiGS1ApplicationIdentifierItem("93", 2, 0, 0, 0, 30, NeedFNC1: true, "INTERNAL", "Company internal information"),
		new StiGS1ApplicationIdentifierItem("94", 2, 0, 0, 0, 30, NeedFNC1: true, "INTERNAL", "Company internal information"),
		new StiGS1ApplicationIdentifierItem("95", 2, 0, 0, 0, 30, NeedFNC1: true, "INTERNAL", "Company internal information"),
		new StiGS1ApplicationIdentifierItem("96", 2, 0, 0, 0, 30, NeedFNC1: true, "INTERNAL", "Company internal information"),
		new StiGS1ApplicationIdentifierItem("97", 2, 0, 0, 0, 30, NeedFNC1: true, "INTERNAL", "Company internal information"),
		new StiGS1ApplicationIdentifierItem("98", 2, 0, 0, 0, 30, NeedFNC1: true, "INTERNAL", "Company internal information"),
		new StiGS1ApplicationIdentifierItem("99", 2, 0, 0, 0, 30, NeedFNC1: true, "INTERNAL", "Company internal information")
	};

	private static Hashtable AICodeToItem = new Hashtable();

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiGS1_128BarCodeType;

	public override string ServiceName => "GS1-128";

	public override string DefaultCodeValue => "(21)012345(3103)000123";

	protected override bool TextSpacing => false;

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[2]
		{
			propertiesHelper.fHeight(),
			propertiesHelper.Module()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public override void Draw(object context, StiBarCode barCode, RectangleF rect, float zoom)
	{
		string code = GetCode(barCode);
		string[] array = code.Split(new char[2] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
		int num = array.Length / 2;
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		bool flag = false;
		string message = null;
		if (!code.StartsWith("("))
		{
			flag = true;
			message = $"Incorrect data: {code} \r\nThe first bracket is missing.";
		}
		if (num == 0)
		{
			flag = true;
			message = $"Incorrect data: {code}";
		}
		if (!flag)
		{
			for (int i = 0; i < num; i++)
			{
				string text = array[i * 2].Trim();
				string text2 = array[i * 2 + 1].Trim();
				StiGS1ApplicationIdentifierItem applicationIdentifierItemByCode = GetApplicationIdentifierItemByCode(text);
				if ((text == "01" || text == "02") && (text2.Length == 8 || text2.Length == 12 || text2.Length == 13))
				{
					text2 = new string('0', 14 - text2.Length) + text2;
					int[] array2 = new int[14];
					for (int j = 0; j < 14; j++)
					{
						array2[j] = int.Parse(text2[j].ToString(CultureInfo.InvariantCulture));
					}
					int num2 = array2[1] + array2[3] + array2[5] + array2[7] + array2[9] + array2[11] + (array2[0] + array2[2] + array2[4] + array2[6] + array2[8] + array2[10] + array2[12]) * 3;
					int num3 = 10 - num2 % 10;
					if (num3 == 10)
					{
						num3 = 0;
					}
					text2 = text2.Substring(0, 13) + (char)(num3 + 48);
				}
				string text3 = text;
				foreach (char c in text3)
				{
					if (!char.IsDigit(c))
					{
						flag = true;
						break;
					}
				}
				if (applicationIdentifierItemByCode != null && text.Length > applicationIdentifierItemByCode.AILength)
				{
					flag = true;
				}
				if (flag)
				{
					message = $"Incorrect AI: {text}";
				}
				else if (applicationIdentifierItemByCode != null)
				{
					int num4 = applicationIdentifierItemByCode.NumericMin + applicationIdentifierItemByCode.CharacterMin;
					int num5 = applicationIdentifierItemByCode.NumericMax + applicationIdentifierItemByCode.CharacterMax;
					if (text2.Length < num4 || text2.Length > num5)
					{
						flag = true;
						message = $"Incorrect data length: ({text}){text2}";
					}
					else if (applicationIdentifierItemByCode.NumericMax > 0)
					{
						int num6 = applicationIdentifierItemByCode.NumericMax;
						if (applicationIdentifierItemByCode.NumericMin != applicationIdentifierItemByCode.NumericMax)
						{
							num6 = text2.Length;
						}
						for (int l = 0; l < num6; l++)
						{
							if (!char.IsDigit(text2, l))
							{
								flag = true;
								message = $"Incorrect data: ({text}){text2}";
								break;
							}
						}
					}
				}
				stringBuilder.Append(text + text2);
				if (i < num - 1 && (applicationIdentifierItemByCode == null || applicationIdentifierItemByCode.NeedFNC1))
				{
					stringBuilder.Append('\u0081');
				}
				stringBuilder2.Append("(" + text + ")" + text2);
				if (i < num - 1)
				{
					stringBuilder2.Append(" ");
				}
				if (flag)
				{
					break;
				}
			}
		}
		if (flag && barCode.CodeValue == null && barCode.Code.Value.Contains("{"))
		{
			flag = false;
			stringBuilder = new StringBuilder(barCode.Code.Value);
			stringBuilder2 = new StringBuilder(barCode.Code.Value);
		}
		if (!flag)
		{
			string text4 = StiCode128BarCodeType.EncodeAuto(stringBuilder.ToString(), encodeAsEan: true);
			int[] array3 = new int[text4.Length];
			for (int m = 0; m < text4.Length; m++)
			{
				array3[m] = text4[m];
			}
			int num7 = array3[0] + array3[1];
			for (int n = 0; n < text4.Length - 4; n++)
			{
				num7 += array3[n + 2] * (n + 2);
			}
			array3[^2] = num7 % 103;
			StringBuilder stringBuilder3 = new StringBuilder();
			for (int num8 = 0; num8 < array3.Length; num8++)
			{
				stringBuilder3.Append(CodeToBar(Code128Table[array3[num8]]));
			}
			CalculateSizeFull(10f, 10f, 0f, 1f, 45f, 45f, 46f, 8.33f, 55f, 45f, 2f, zoom, stringBuilder.ToString(), stringBuilder2.ToString(), stringBuilder3.ToString(), rect, barCode);
			DrawBarCode(context, rect, barCode);
		}
		else
		{
			DrawBarCodeError(context, rect, barCode, message);
		}
	}

	public override StiBarCodeTypeService CreateNew()
	{
		return new StiGS1_128BarCodeType();
	}

	public static StiGS1ApplicationIdentifierItem GetApplicationIdentifierItemByCode(string code)
	{
		if (AICodeToItem.ContainsKey(code))
		{
			return AICodeToItem[code] as StiGS1ApplicationIdentifierItem;
		}
		StiGS1ApplicationIdentifierItem[] gS1ApplicationIdentifiers = GS1ApplicationIdentifiers;
		foreach (StiGS1ApplicationIdentifierItem stiGS1ApplicationIdentifierItem in gS1ApplicationIdentifiers)
		{
			if (code.Length >= stiGS1ApplicationIdentifierItem.AILength && code.StartsWith(stiGS1ApplicationIdentifierItem.AICode))
			{
				AICodeToItem[code] = stiGS1ApplicationIdentifierItem;
				return stiGS1ApplicationIdentifierItem;
			}
		}
		return null;
	}

	public StiGS1_128BarCodeType()
		: this(13f, 1f)
	{
	}

	public StiGS1_128BarCodeType(float module, float height)
		: base(module, height)
	{
	}
}
