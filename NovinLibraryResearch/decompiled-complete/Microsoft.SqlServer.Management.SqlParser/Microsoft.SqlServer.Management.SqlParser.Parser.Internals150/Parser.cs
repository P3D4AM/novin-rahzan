using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Common;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals;
using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals150;

internal class Parser : VersionedParserBase
{
	public static readonly ParserConfig ParserConfig = new ParserConfig();

	public Parser()
		: base(ParserConfig)
	{
	}

	protected override Babel.ParserGenerator.StateOnlyParser CreateStateOnlyParser()
	{
		return new StateOnlyParser(scanner);
	}

	protected override void DoAction(int action)
	{
		switch (action / 50)
		{
		case 0:
			DoAction_0_49(action);
			break;
		case 1:
			DoAction_50_99(action);
			break;
		case 2:
			DoAction_100_149(action);
			break;
		case 3:
			DoAction_150_199(action);
			break;
		case 4:
			DoAction_200_249(action);
			break;
		case 5:
			DoAction_250_299(action);
			break;
		case 6:
			DoAction_300_349(action);
			break;
		case 7:
			DoAction_350_399(action);
			break;
		case 8:
			DoAction_400_449(action);
			break;
		case 9:
			DoAction_450_499(action);
			break;
		case 10:
			DoAction_500_549(action);
			break;
		case 11:
			DoAction_550_599(action);
			break;
		case 12:
			DoAction_600_649(action);
			break;
		case 13:
			DoAction_650_699(action);
			break;
		case 14:
			DoAction_700_749(action);
			break;
		case 15:
			DoAction_750_799(action);
			break;
		case 16:
			DoAction_800_849(action);
			break;
		case 17:
			DoAction_850_899(action);
			break;
		case 18:
			DoAction_900_949(action);
			break;
		case 19:
			DoAction_950_999(action);
			break;
		case 20:
			DoAction_1000_1049(action);
			break;
		case 21:
			DoAction_1050_1099(action);
			break;
		case 22:
			DoAction_1100_1149(action);
			break;
		case 23:
			DoAction_1150_1199(action);
			break;
		case 24:
			DoAction_1200_1249(action);
			break;
		case 25:
			DoAction_1250_1299(action);
			break;
		case 26:
			DoAction_1300_1349(action);
			break;
		case 27:
			DoAction_1350_1399(action);
			break;
		case 28:
			DoAction_1400_1449(action);
			break;
		case 29:
			DoAction_1450_1499(action);
			break;
		case 30:
			DoAction_1500_1549(action);
			break;
		case 31:
			DoAction_1550_1599(action);
			break;
		case 32:
			DoAction_1600_1649(action);
			break;
		case 33:
			DoAction_1650_1699(action);
			break;
		case 34:
			DoAction_1700_1749(action);
			break;
		case 35:
			DoAction_1750_1799(action);
			break;
		case 36:
			DoAction_1800_1849(action);
			break;
		case 37:
			DoAction_1850_1899(action);
			break;
		case 38:
			DoAction_1900_1949(action);
			break;
		case 39:
			DoAction_1950_1999(action);
			break;
		case 40:
			DoAction_2000_2049(action);
			break;
		case 41:
			DoAction_2050_2099(action);
			break;
		case 42:
			DoAction_2100_2149(action);
			break;
		case 43:
			DoAction_2150_2199(action);
			break;
		case 44:
			DoAction_2200_2249(action);
			break;
		case 45:
			DoAction_2250_2299(action);
			break;
		case 46:
			DoAction_2300_2349(action);
			break;
		case 47:
			DoAction_2350_2399(action);
			break;
		case 48:
			DoAction_2400_2449(action);
			break;
		case 49:
			DoAction_2450_2499(action);
			break;
		case 50:
			DoAction_2500_2549(action);
			break;
		case 51:
			DoAction_2550_2599(action);
			break;
		case 52:
			DoAction_2600_2649(action);
			break;
		case 53:
			DoAction_2650_2699(action);
			break;
		case 54:
			DoAction_2700_2749(action);
			break;
		case 55:
			DoAction_2750_2799(action);
			break;
		case 56:
			DoAction_2800_2849(action);
			break;
		case 57:
			DoAction_2850_2899(action);
			break;
		case 58:
			DoAction_2900_2949(action);
			break;
		case 59:
			DoAction_2950_2999(action);
			break;
		case 60:
			DoAction_3000_3049(action);
			break;
		case 61:
			DoAction_3050_3099(action);
			break;
		case 62:
			DoAction_3100_3149(action);
			break;
		case 63:
			DoAction_3150_3199(action);
			break;
		case 64:
			DoAction_3200_3249(action);
			break;
		case 65:
			DoAction_3250_3299(action);
			break;
		case 66:
			DoAction_3300_3349(action);
			break;
		case 67:
			DoAction_3350_3399(action);
			break;
		case 68:
			DoAction_3400_3449(action);
			break;
		case 69:
			DoAction_3450_3499(action);
			break;
		case 70:
			DoAction_3500_3549(action);
			break;
		case 71:
			DoAction_3550_3599(action);
			break;
		case 72:
			DoAction_3600_3649(action);
			break;
		case 73:
			DoAction_3650_3699(action);
			break;
		case 74:
			DoAction_3700_3749(action);
			break;
		case 75:
			DoAction_3750_3799(action);
			break;
		case 76:
			DoAction_3800_3849(action);
			break;
		case 77:
			DoAction_3850_3899(action);
			break;
		case 78:
			DoAction_3900_3907(action);
			break;
		}
	}

	private void DoAction_0_49(int action)
	{
		switch (action)
		{
		case 2:
			root = value_stack.array[value_stack.top - 1].value as SqlBatch;
			break;
		case 3:
		{
			SqlBatch sqlBatch3 = new SqlBatch(value_stack.array[value_stack.top - 1].value as List<SqlStatement>);
			sqlBatch3.Position = yyloc;
			yyval.value = sqlBatch3;
			break;
		}
		case 4:
		{
			SqlBatch sqlBatch2 = new SqlBatch(value_stack.array[value_stack.top - 1].value as SqlStatement);
			sqlBatch2.Position = yyloc;
			yyval.value = sqlBatch2;
			break;
		}
		case 5:
		{
			SqlBatch sqlBatch = new SqlBatch(value_stack.array[value_stack.top - 1].value as SqlStatement);
			sqlBatch.Position = yyloc;
			yyval.value = sqlBatch;
			break;
		}
		case 7:
		{
			List<SqlStatement> list4 = value_stack.array[value_stack.top - 1].value as List<SqlStatement>;
			if (list4 == null)
			{
				list4 = new List<SqlStatement>();
			}
			list4.Insert(0, value_stack.array[value_stack.top - 2].value as SqlStatement);
			yyval.value = list4;
			break;
		}
		case 8:
		{
			List<SqlStatement> list5 = new List<SqlStatement>();
			list5.Add(value_stack.array[value_stack.top - 2].value as SqlStatement);
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlStatement);
			yyval.value = list5;
			break;
		}
		case 9:
		{
			SqlStatement sqlStatement3 = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement3 == null)
			{
				sqlStatement3 = new SqlNullStatement();
			}
			sqlStatement3.Position = yyloc;
			yyval.value = sqlStatement3;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement3);
			break;
		}
		case 10:
		{
			SqlStatement sqlStatement6 = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement6 == null)
			{
				sqlStatement6 = new SqlNullStatement();
			}
			sqlStatement6.Position = yyloc;
			yyval.value = sqlStatement6;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement6);
			break;
		}
		case 11:
		{
			SqlStatement sqlStatement5 = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement5 == null)
			{
				sqlStatement5 = new SqlNullStatement();
			}
			sqlStatement5.Position = yyloc;
			yyval.value = sqlStatement5;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement5);
			break;
		}
		case 12:
		{
			SqlStatement sqlStatement4 = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement4 == null)
			{
				sqlStatement4 = new SqlNullStatement();
			}
			sqlStatement4.Position = yyloc;
			yyval.value = sqlStatement4;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement4);
			break;
		}
		case 16:
		{
			List<SqlStatement> list3 = value_stack.array[value_stack.top - 2].value as List<SqlStatement>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlStatement);
			yyval.value = list3;
			break;
		}
		case 19:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 20:
		{
			List<SqlStatement> list2 = new List<SqlStatement>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlStatement);
			yyval.value = list2;
			break;
		}
		case 21:
		{
			List<SqlStatement> list = value_stack.array[value_stack.top - 2].value as List<SqlStatement>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlStatement);
			yyval.value = list;
			break;
		}
		case 29:
		{
			yyval = value_stack.array[value_stack.top - 2];
			SqlStatement sqlStatement2 = value_stack.array[value_stack.top - 2].value as SqlStatement;
			if (sqlStatement2 == null)
			{
				sqlStatement2 = new SqlNullStatement();
				sqlStatement2.Position = yyloc;
				yyval.value = sqlStatement2;
			}
			break;
		}
		case 30:
		{
			yyval = value_stack.array[value_stack.top - 1];
			SqlStatement sqlStatement = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement == null)
			{
				sqlStatement = new SqlNullStatement();
				sqlStatement.Position = yyloc;
				yyval.value = sqlStatement;
			}
			break;
		}
		case 34:
		{
			SqlStatementError sqlStatementError2 = new SqlStatementError(value_stack.array[value_stack.top - 2].value as SqlErrorCodeObject);
			sqlStatementError2.Position = yyloc;
			yyval.value = sqlStatementError2;
			scanner.ResetAllParseStates();
			break;
		}
		case 35:
		{
			SqlStatementError sqlStatementError = new SqlStatementError(value_stack.array[value_stack.top - 2].value as SqlQueryWithClause, value_stack.array[value_stack.top - 1].value as SqlErrorCodeObject);
			sqlStatementError.Position = yyloc;
			yyval.value = sqlStatementError;
			break;
		}
		case 1:
		case 6:
		case 13:
		case 14:
		case 15:
		case 17:
		case 18:
		case 22:
		case 23:
		case 24:
		case 25:
		case 26:
		case 27:
		case 28:
		case 31:
		case 32:
		case 33:
		case 36:
		case 37:
		case 38:
		case 39:
		case 40:
		case 41:
		case 42:
		case 43:
		case 44:
		case 45:
		case 46:
		case 47:
		case 48:
		case 49:
			break;
		}
	}

	private void DoAction_50_99(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_100_149(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_150_199(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_200_249(int action)
	{
		switch (action)
		{
		case 245:
		{
			SqlIdentifier sqlIdentifier4 = new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlIdentifier4.Position = yyloc;
			yyval.value = sqlIdentifier4;
			break;
		}
		case 246:
		{
			SqlIdentifier sqlIdentifier3 = new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlIdentifier3.Position = yyloc;
			yyval.value = sqlIdentifier3;
			break;
		}
		case 247:
		{
			SqlIdentifier sqlIdentifier2 = new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlIdentifier2.Position = yyloc;
			yyval.value = sqlIdentifier2;
			break;
		}
		case 248:
		{
			SqlIdentifier sqlIdentifier = new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlIdentifier.Position = yyloc;
			yyval.value = sqlIdentifier;
			break;
		}
		case 200:
		case 201:
		case 202:
		case 203:
		case 204:
		case 205:
		case 206:
		case 207:
		case 208:
		case 209:
		case 210:
		case 211:
		case 212:
		case 213:
		case 214:
		case 215:
		case 216:
		case 217:
		case 218:
		case 219:
		case 220:
		case 221:
		case 222:
		case 223:
		case 224:
		case 225:
		case 226:
		case 227:
		case 228:
		case 229:
		case 230:
		case 231:
		case 232:
		case 233:
		case 234:
		case 235:
		case 236:
		case 237:
		case 238:
		case 239:
		case 240:
		case 241:
		case 242:
		case 243:
		case 244:
		case 249:
			break;
		}
	}

	private void DoAction_250_299(int action)
	{
		switch (action)
		{
		case 250:
			scanner.SetQueryStatus(QueryStatus.Q_ODBC_ESCAPE);
			break;
		case 251:
			scanner.ClearQueryStatus(QueryStatus.Q_ODBC_ESCAPE);
			break;
		case 253:
		{
			SqlUseStatement sqlUseStatement2 = new SqlUseStatement(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlUseStatement2.Position = yyloc;
			yyval.value = sqlUseStatement2;
			break;
		}
		case 254:
		{
			SqlIdentifier sqlIdentifier = SqlIdentifier.Missing();
			sqlIdentifier.Position = location_stack.array[location_stack.top - 1];
			SqlUseStatement sqlUseStatement = new SqlUseStatement(sqlIdentifier);
			sqlUseStatement.Position = yyloc;
			yyval.value = sqlUseStatement;
			break;
		}
		case 255:
			scanner.SetQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 256:
			scanner.ClearQueryStatus(QueryStatus.Q_CRDB);
			break;
		case 257:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 252:
		case 258:
		case 259:
		case 260:
		case 261:
		case 262:
		case 263:
		case 264:
		case 265:
		case 266:
		case 267:
		case 268:
		case 269:
		case 270:
		case 271:
		case 272:
		case 273:
		case 274:
		case 275:
		case 276:
		case 277:
		case 278:
		case 279:
		case 280:
		case 281:
		case 282:
		case 283:
		case 284:
		case 285:
		case 286:
		case 287:
		case 288:
		case 289:
		case 290:
		case 291:
		case 292:
		case 293:
		case 294:
		case 295:
		case 296:
		case 297:
		case 298:
		case 299:
			break;
		}
	}

	private void DoAction_300_349(int action)
	{
		switch (action)
		{
		case 327:
			scanner.SetQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 330:
			scanner.ClearQueryStatus(QueryStatus.Q_ALTDB);
			break;
		case 331:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 332:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 334:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 340:
			if (GetStringToken(location_stack.array[location_stack.top - 3]) == StringToken.MaxDop)
			{
				VersionedParserBase.ReportError(SR.InvalidUsageOfScopedConfiguration(GetText(location_stack.array[location_stack.top - 3])), location_stack.array[location_stack.top - 1]);
			}
			break;
		case 341:
			switch (GetStringToken(location_stack.array[location_stack.top - 3]))
			{
			case StringToken.MaxDop:
			case StringToken.ParameterSniffing:
			case StringToken.QueryOptimizerHotfixes:
			case StringToken.LegacyCardinalityEstimation:
				VersionedParserBase.ReportError(SR.InvalidUsageOfScopedConfiguration(GetText(location_stack.array[location_stack.top - 3])), location_stack.array[location_stack.top - 1]);
				break;
			}
			break;
		case 342:
		{
			StringToken stringToken = GetStringToken(location_stack.array[location_stack.top - 3]);
			if (stringToken == StringToken.ParameterSniffing || stringToken == StringToken.QueryOptimizerHotfixes || stringToken == StringToken.LegacyCardinalityEstimation)
			{
				VersionedParserBase.ReportError(SR.InvalidUsageOfScopedConfiguration(GetText(location_stack.array[location_stack.top - 3])), location_stack.array[location_stack.top - 1]);
			}
			break;
		}
		case 300:
		case 301:
		case 302:
		case 303:
		case 304:
		case 305:
		case 306:
		case 307:
		case 308:
		case 309:
		case 310:
		case 311:
		case 312:
		case 313:
		case 314:
		case 315:
		case 316:
		case 317:
		case 318:
		case 319:
		case 320:
		case 321:
		case 322:
		case 323:
		case 324:
		case 325:
		case 326:
		case 328:
		case 329:
		case 333:
		case 335:
		case 336:
		case 337:
		case 338:
		case 339:
		case 343:
		case 344:
		case 345:
		case 346:
		case 347:
		case 348:
		case 349:
			break;
		}
	}

	private void DoAction_350_399(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_400_449(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_450_499(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_500_549(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_550_599(int action)
	{
		switch (action)
		{
		case 590:
		{
			SqlTableDefinition sqlTableDefinition4 = new SqlTableDefinition(value_stack.array[value_stack.top - 7].value as List<ISqlTableElement>);
			sqlTableDefinition4.Position = location_stack.array[location_stack.top - 7];
			SqlCreateTableStatement sqlCreateTableStatement5 = new SqlCreateTableStatement(value_stack.array[value_stack.top - 9].value as SqlObjectIdentifier, sqlTableDefinition4, value_stack.array[value_stack.top - 3].value as SqlStorageSpecification, value_stack.array[value_stack.top - 2].value as SqlLargeDataStorageInformation);
			sqlCreateTableStatement5.Position = yyloc;
			yyval.value = sqlCreateTableStatement5;
			break;
		}
		case 591:
		{
			SqlTableDefinition sqlTableDefinition3 = new SqlTableDefinition(value_stack.array[value_stack.top - 7].value as List<ISqlTableElement>);
			sqlTableDefinition3.Position = location_stack.array[location_stack.top - 7];
			SqlCreateTableStatement sqlCreateTableStatement4 = new SqlCreateTableStatement(value_stack.array[value_stack.top - 9].value as SqlObjectIdentifier, sqlTableDefinition3, value_stack.array[value_stack.top - 3].value as SqlStorageSpecification, value_stack.array[value_stack.top - 2].value as SqlLargeDataStorageInformation);
			sqlCreateTableStatement4.Position = yyloc;
			yyval.value = sqlCreateTableStatement4;
			break;
		}
		case 592:
		{
			SqlCreateTableStatement sqlCreateTableStatement3 = new SqlCreateTableStatement(value_stack.array[value_stack.top - 6].value as SqlObjectIdentifier, new SqlTableDefinition(new List<ISqlTableElement>()), value_stack.array[value_stack.top - 3].value as SqlStorageSpecification, value_stack.array[value_stack.top - 2].value as SqlLargeDataStorageInformation);
			sqlCreateTableStatement3.Position = yyloc;
			yyval.value = sqlCreateTableStatement3;
			break;
		}
		case 594:
		{
			SqlTableDefinition sqlTableDefinition2 = new SqlTableDefinition(value_stack.array[value_stack.top - 5].value as List<ISqlTableElement>);
			sqlTableDefinition2.Position = location_stack.array[location_stack.top - 5];
			SqlCreateTableStatement sqlCreateTableStatement2 = new SqlCreateTableStatement(value_stack.array[value_stack.top - 7].value as SqlObjectIdentifier, sqlTableDefinition2, value_stack.array[value_stack.top - 3].value as SqlStorageSpecification, value_stack.array[value_stack.top - 2].value as SqlLargeDataStorageInformation);
			sqlCreateTableStatement2.Position = yyloc;
			yyval.value = sqlCreateTableStatement2;
			break;
		}
		case 595:
		{
			SqlTableDefinition sqlTableDefinition = new SqlTableDefinition(value_stack.array[value_stack.top - 10].value as List<ISqlTableElement>);
			sqlTableDefinition.Position = location_stack.array[location_stack.top - 10];
			SqlCreateTableStatement sqlCreateTableStatement = new SqlCreateTableStatement(value_stack.array[value_stack.top - 12].value as SqlObjectIdentifier, sqlTableDefinition, value_stack.array[value_stack.top - 8].value as SqlStorageSpecification, value_stack.array[value_stack.top - 7].value as SqlLargeDataStorageInformation);
			sqlCreateTableStatement.Position = yyloc;
			yyval.value = sqlCreateTableStatement;
			break;
		}
		case 550:
		case 551:
		case 552:
		case 553:
		case 554:
		case 555:
		case 556:
		case 557:
		case 558:
		case 559:
		case 560:
		case 561:
		case 562:
		case 563:
		case 564:
		case 565:
		case 566:
		case 567:
		case 568:
		case 569:
		case 570:
		case 571:
		case 572:
		case 573:
		case 574:
		case 575:
		case 576:
		case 577:
		case 578:
		case 579:
		case 580:
		case 581:
		case 582:
		case 583:
		case 584:
		case 585:
		case 586:
		case 587:
		case 588:
		case 589:
		case 593:
		case 596:
		case 597:
		case 598:
		case 599:
			break;
		}
	}

	private void DoAction_600_649(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_650_699(int action)
	{
		switch (action)
		{
		case 663:
		{
			List<ISqlTableElement> list2 = new List<ISqlTableElement>();
			list2.Add(value_stack.array[value_stack.top - 1].value as ISqlTableElement);
			yyval.value = list2;
			break;
		}
		case 664:
		{
			List<ISqlTableElement> list = value_stack.array[value_stack.top - 3].value as List<ISqlTableElement>;
			list.Add(value_stack.array[value_stack.top - 1].value as ISqlTableElement);
			yyval.value = list;
			break;
		}
		case 665:
		{
			SqlColumnDefinition sqlColumnDefinition2 = value_stack.array[value_stack.top - 2].value as SqlColumnDefinition;
			sqlColumnDefinition2.AddConstraints(value_stack.array[value_stack.top - 1].value as List<SqlConstraint>);
			sqlColumnDefinition2.Position = yyloc;
			yyval.value = sqlColumnDefinition2;
			break;
		}
		case 667:
		{
			SqlColumnDefinition sqlColumnDefinition = value_stack.array[value_stack.top - 2].value as SqlColumnDefinition;
			sqlColumnDefinition.AddConstraints(value_stack.array[value_stack.top - 1].value as List<SqlConstraint>);
			sqlColumnDefinition.Position = yyloc;
			yyval.value = sqlColumnDefinition;
			break;
		}
		case 670:
		{
			SqlTemporalPeriodDefinition sqlTemporalPeriodDefinition = new SqlTemporalPeriodDefinition(value_stack.array[value_stack.top - 4].value as SqlIdentifier, value_stack.array[value_stack.top - 2].value as SqlIdentifier);
			sqlTemporalPeriodDefinition.Position = yyloc;
			yyval.value = sqlTemporalPeriodDefinition;
			ExpectKeyword(StringToken.Period, location_stack.array[location_stack.top - 8]);
			ExpectKeyword(StringToken.SystemTime, location_stack.array[location_stack.top - 6]);
			break;
		}
		case 674:
		{
			SqlColumnDefinition sqlColumnDefinition7 = new SqlColumnDefinition(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlColumnDefinition7.Position = yyloc;
			yyval.value = sqlColumnDefinition7;
			break;
		}
		case 676:
		{
			SqlDataTypeSpecification sqlDataTypeSpecification4 = new SqlDataTypeSpecification(value_stack.array[value_stack.top - 3].value as SqlDataType);
			sqlDataTypeSpecification4.Position = location_stack.array[location_stack.top - 3];
			SqlColumnDefinition sqlColumnDefinition6 = new SqlColumnDefinition(value_stack.array[value_stack.top - 4].value as SqlIdentifier, sqlDataTypeSpecification4, value_stack.array[value_stack.top - 2].value as SqlCollation, value_stack.array[value_stack.top - 1].value as SqlColumnAdditionalInfo);
			sqlColumnDefinition6.Position = yyloc;
			yyval.value = sqlColumnDefinition6;
			break;
		}
		case 677:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 4]));
			sqlLiteralExpression.Position = location_stack.array[location_stack.top - 4];
			SqlDataTypeSpecification sqlDataTypeSpecification3 = new SqlDataTypeSpecification(value_stack.array[value_stack.top - 6].value as SqlDataType, sqlLiteralExpression);
			sqlDataTypeSpecification3.Position = MergeLocationRange(location_stack.top - 6, location_stack.top - 3);
			SqlColumnDefinition sqlColumnDefinition5 = new SqlColumnDefinition(value_stack.array[value_stack.top - 7].value as SqlIdentifier, sqlDataTypeSpecification3, value_stack.array[value_stack.top - 2].value as SqlCollation, value_stack.array[value_stack.top - 1].value as SqlColumnAdditionalInfo);
			sqlColumnDefinition5.Position = yyloc;
			yyval.value = sqlColumnDefinition5;
			break;
		}
		case 678:
		{
			SqlDataTypeSpecification sqlDataTypeSpecification2 = new SqlDataTypeSpecification(value_stack.array[value_stack.top - 7].value as SqlDataType, value_stack.array[value_stack.top - 5].value as SqlLiteralExpression, value_stack.array[value_stack.top - 3].value as SqlLiteralExpression);
			sqlDataTypeSpecification2.Position = MergeLocationRange(location_stack.top - 7, location_stack.top - 2);
			SqlColumnDefinition sqlColumnDefinition4 = new SqlColumnDefinition(value_stack.array[value_stack.top - 8].value as SqlIdentifier, sqlDataTypeSpecification2, value_stack.array[value_stack.top - 1].value as SqlColumnAdditionalInfo);
			sqlColumnDefinition4.Position = yyloc;
			yyval.value = sqlColumnDefinition4;
			break;
		}
		case 679:
		{
			SqlDataTypeSpecification sqlDataTypeSpecification = new SqlDataTypeSpecification(value_stack.array[value_stack.top - 7].value as SqlDataType, value_stack.array[value_stack.top - 5].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 4].value as SqlObjectIdentifier);
			sqlDataTypeSpecification.Position = MergeLocationRange(location_stack.top - 7, location_stack.top - 3);
			SqlColumnDefinition sqlColumnDefinition3 = new SqlColumnDefinition(value_stack.array[value_stack.top - 8].value as SqlIdentifier, sqlDataTypeSpecification, value_stack.array[value_stack.top - 2].value as SqlCollation);
			sqlColumnDefinition3.Position = yyloc;
			yyval.value = sqlColumnDefinition3;
			break;
		}
		case 680:
		{
			SqlDataType sqlDataType3 = new SqlDataType(value_stack.array[value_stack.top - 2].value as SqlObjectIdentifier, national: false, value_stack.array[value_stack.top - 1].value != null);
			sqlDataType3.Position = yyloc;
			yyval.value = sqlDataType3;
			if ((sqlDataType3.TypeErrors & DataTypeErrors.VaryingNotAllowed) == DataTypeErrors.VaryingNotAllowed)
			{
				VersionedParserBase.SyntaxError("varying", location_stack.array[location_stack.top - 1]);
			}
			break;
		}
		case 681:
		{
			SqlDataType sqlDataType2 = new SqlDataType(value_stack.array[value_stack.top - 2].value as SqlObjectIdentifier, national: true, value_stack.array[value_stack.top - 1].value != null);
			sqlDataType2.Position = yyloc;
			yyval.value = sqlDataType2;
			if ((sqlDataType2.TypeErrors & DataTypeErrors.NationalNotAllowed) == DataTypeErrors.NationalNotAllowed)
			{
				VersionedParserBase.SyntaxError("national", location_stack.array[location_stack.top - 3]);
			}
			else if ((sqlDataType2.TypeErrors & DataTypeErrors.VaryingNotAllowed) == DataTypeErrors.VaryingNotAllowed)
			{
				VersionedParserBase.SyntaxError("varying", location_stack.array[location_stack.top - 1]);
			}
			break;
		}
		case 682:
		{
			SqlObjectIdentifier sqlObjectIdentifier4 = SqlObjectIdentifier.Create(new SqlIdentifier(GetText(location_stack.array[location_stack.top - 2]))
			{
				Position = location_stack.array[location_stack.top - 2]
			});
			sqlObjectIdentifier4.Position = location_stack.array[location_stack.top - 2];
			SqlDataType sqlDataType = SqlDataType.Double(sqlObjectIdentifier4);
			sqlDataType.Position = yyloc;
			yyval.value = sqlDataType;
			ExpectKeyword(StringToken.Precision, location_stack.array[location_stack.top - 1]);
			break;
		}
		case 683:
		{
			SqlObjectIdentifier sqlObjectIdentifier3 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier3.Position = yyloc;
			yyval.value = sqlObjectIdentifier3;
			break;
		}
		case 684:
		{
			SqlObjectIdentifier sqlObjectIdentifier2 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier2.Position = yyloc;
			yyval.value = sqlObjectIdentifier2;
			break;
		}
		case 685:
		{
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 5].value as SqlIdentifier, value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier.Position = yyloc;
			yyval.value = sqlObjectIdentifier;
			break;
		}
		case 686:
			yyval.value = SqlIdentifier.Null;
			break;
		case 691:
		{
			SqlCollation sqlCollation = new SqlCollation(new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]))
			{
				Position = location_stack.array[location_stack.top - 1]
			});
			sqlCollation.Position = yyloc;
			yyval.value = sqlCollation;
			if (EscapeSequence.IdentifyEscapeSequence(GetText(location_stack.array[location_stack.top - 1])) != null)
			{
				VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 1]);
			}
			break;
		}
		case 650:
		case 651:
		case 652:
		case 653:
		case 654:
		case 655:
		case 656:
		case 657:
		case 658:
		case 659:
		case 660:
		case 661:
		case 662:
		case 666:
		case 668:
		case 669:
		case 671:
		case 672:
		case 673:
		case 675:
		case 687:
		case 688:
		case 689:
		case 690:
		case 692:
		case 693:
		case 694:
		case 695:
		case 696:
		case 697:
		case 698:
		case 699:
			break;
		}
	}

	private void DoAction_700_749(int action)
	{
		switch (action)
		{
		case 710:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.None);
			break;
		case 711:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.Sparse);
			break;
		case 712:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.Sparse);
			break;
		case 713:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.None);
			break;
		case 714:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.Sparse);
			break;
		case 715:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.None);
			break;
		case 716:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.Sparse);
			break;
		case 717:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.ColumnSet);
			break;
		case 718:
			ExpectKeyword(StringToken.Always, location_stack.array[location_stack.top - 5]);
			ExpectKeyword(StringToken.Start, location_stack.array[location_stack.top - 2]);
			switch (GetStringToken(location_stack.array[location_stack.top - 3]))
			{
			case StringToken.Row:
				yyval.value = new SqlColumnAdditionalInfo(SqlGeneratedAlwaysType.AsRowStart);
				break;
			case StringToken.TransactionId:
				yyval.value = new SqlColumnAdditionalInfo(SqlGeneratedAlwaysType.AsTransactionIdStart);
				break;
			case StringToken.SequenceNumber:
				yyval.value = new SqlColumnAdditionalInfo(SqlGeneratedAlwaysType.AsSequenceNumberStart);
				break;
			default:
				VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 3]);
				break;
			}
			break;
		case 719:
			ExpectKeyword(StringToken.Always, location_stack.array[location_stack.top - 5]);
			switch (GetStringToken(location_stack.array[location_stack.top - 3]))
			{
			case StringToken.Row:
				yyval.value = new SqlColumnAdditionalInfo(SqlGeneratedAlwaysType.AsRowEnd);
				break;
			case StringToken.TransactionId:
				yyval.value = new SqlColumnAdditionalInfo(SqlGeneratedAlwaysType.AsTransactionIdEnd);
				break;
			case StringToken.SequenceNumber:
				yyval.value = new SqlColumnAdditionalInfo(SqlGeneratedAlwaysType.AsSequenceNumberEnd);
				break;
			default:
				VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 3]);
				break;
			}
			break;
		case 721:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.None);
			break;
		case 722:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.Sparse);
			break;
		case 723:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.Sparse);
			break;
		case 724:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.None);
			break;
		case 725:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.Sparse);
			break;
		case 726:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.None);
			break;
		case 727:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.Sparse);
			break;
		case 728:
			ExpectKeyword(StringToken.Always, location_stack.array[location_stack.top - 5]);
			ExpectKeyword(StringToken.Start, location_stack.array[location_stack.top - 2]);
			switch (GetStringToken(location_stack.array[location_stack.top - 3]))
			{
			case StringToken.Row:
				yyval.value = new SqlColumnAdditionalInfo(SqlGeneratedAlwaysType.AsRowStart);
				break;
			case StringToken.TransactionId:
				yyval.value = new SqlColumnAdditionalInfo(SqlGeneratedAlwaysType.AsTransactionIdStart);
				break;
			case StringToken.SequenceNumber:
				yyval.value = new SqlColumnAdditionalInfo(SqlGeneratedAlwaysType.AsSequenceNumberStart);
				break;
			default:
				VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 3]);
				break;
			}
			break;
		case 729:
			ExpectKeyword(StringToken.Always, location_stack.array[location_stack.top - 5]);
			switch (GetStringToken(location_stack.array[location_stack.top - 3]))
			{
			case StringToken.Row:
				yyval.value = new SqlColumnAdditionalInfo(SqlGeneratedAlwaysType.AsRowEnd);
				break;
			case StringToken.TransactionId:
				yyval.value = new SqlColumnAdditionalInfo(SqlGeneratedAlwaysType.AsTransactionIdEnd);
				break;
			case StringToken.SequenceNumber:
				yyval.value = new SqlColumnAdditionalInfo(SqlGeneratedAlwaysType.AsSequenceNumberEnd);
				break;
			default:
				VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 3]);
				break;
			}
			break;
		case 730:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.None);
			break;
		case 733:
			yyval.value = null;
			break;
		case 734:
			yyval.value = "varying";
			break;
		case 737:
		{
			List<SqlConstraint> list2 = new List<SqlConstraint>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlConstraint);
			yyval.value = list2;
			break;
		}
		case 738:
		{
			List<SqlConstraint> list = value_stack.array[value_stack.top - 2].value as List<SqlConstraint>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlConstraint);
			yyval.value = list;
			break;
		}
		case 740:
		{
			SqlConstraint sqlConstraint2 = value_stack.array[value_stack.top - 1].value as SqlConstraint;
			sqlConstraint2.Name = value_stack.array[value_stack.top - 2].value as SqlIdentifier;
			sqlConstraint2.Position = yyloc;
			yyval.value = sqlConstraint2;
			break;
		}
		case 741:
			yyval.value = value_stack.array[value_stack.top - 1].value as ISqlTableElement;
			break;
		case 743:
		{
			SqlConstraint sqlConstraint = value_stack.array[value_stack.top - 1].value as SqlConstraint;
			sqlConstraint.Name = value_stack.array[value_stack.top - 2].value as SqlIdentifier;
			sqlConstraint.Position = yyloc;
			yyval.value = sqlConstraint;
			break;
		}
		case 748:
		{
			SqlForeignKeyConstraint sqlForeignKeyConstraint = new SqlForeignKeyConstraint(value_stack.array[value_stack.top - 6].value as List<SqlIdentifier>, value_stack.array[value_stack.top - 4].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 3].value as List<SqlIdentifier>, (SqlForeignKeyAction)value_stack.array[value_stack.top - 2].value, (bool)value_stack.array[value_stack.top - 1].value);
			sqlForeignKeyConstraint.Position = yyloc;
			yyval.value = sqlForeignKeyConstraint;
			break;
		}
		case 749:
		{
			SqlDefaultConstraint sqlDefaultConstraint = new SqlDefaultConstraint(value_stack.array[value_stack.top - 3].value as SqlScalarExpression);
			sqlDefaultConstraint.Position = yyloc;
			yyval.value = sqlDefaultConstraint;
			break;
		}
		case 700:
		case 701:
		case 702:
		case 703:
		case 704:
		case 705:
		case 706:
		case 707:
		case 708:
		case 709:
		case 720:
		case 731:
		case 732:
		case 735:
		case 736:
		case 739:
		case 742:
		case 744:
		case 745:
		case 746:
		case 747:
			break;
		}
	}

	private void DoAction_750_799(int action)
	{
		switch (action)
		{
		case 750:
		{
			SqlForeignKeyConstraint sqlForeignKeyConstraint = new SqlForeignKeyConstraint(null, value_stack.array[value_stack.top - 4].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 3].value as List<SqlIdentifier>, (SqlForeignKeyAction)value_stack.array[value_stack.top - 2].value, (bool)value_stack.array[value_stack.top - 1].value);
			sqlForeignKeyConstraint.Position = yyloc;
			yyval.value = sqlForeignKeyConstraint;
			break;
		}
		case 751:
		{
			SqlCheckConstraint sqlCheckConstraint = new SqlCheckConstraint((bool)value_stack.array[value_stack.top - 4].value, value_stack.array[value_stack.top - 2].value as SqlBooleanExpression);
			sqlCheckConstraint.Position = yyloc;
			yyval.value = sqlCheckConstraint;
			break;
		}
		case 753:
		{
			SqlColumnIdentity sqlColumnIdentity = value_stack.array[value_stack.top - 2].value as SqlColumnIdentity;
			if ((bool)value_stack.array[value_stack.top - 1].value)
			{
				sqlColumnIdentity.AddNotForReplicationClause();
			}
			sqlColumnIdentity.Position = yyloc;
			yyval.value = sqlColumnIdentity;
			yyval = value_stack.array[value_stack.top - 2];
			break;
		}
		case 755:
		{
			SqlConstraint sqlConstraint = SqlConstraint.RowGuidCol();
			sqlConstraint.Position = yyloc;
			yyval.value = sqlConstraint;
			break;
		}
		case 756:
			yyval.value = false;
			break;
		case 757:
			yyval.value = true;
			break;
		case 761:
			yyval.value = 512;
			break;
		case 762:
			yyval.value = 768;
			break;
		case 763:
			yyval.value = 1024;
			break;
		case 764:
		{
			bool flag2 = ExpectKeyword(StringToken.No, location_stack.array[location_stack.top - 2]) && ExpectKeyword(StringToken.Action, location_stack.array[location_stack.top - 1]);
			yyval.value = (flag2 ? 256 : 0);
			break;
		}
		case 765:
			yyval.value = SqlForeignKeyAction.Cascade;
			break;
		case 766:
			yyval.value = SqlForeignKeyAction.SetDefault;
			break;
		case 767:
			yyval.value = SqlForeignKeyAction.SetNull;
			break;
		case 768:
		{
			bool flag = ExpectKeyword(StringToken.No, location_stack.array[location_stack.top - 2]) && ExpectKeyword(StringToken.Action, location_stack.array[location_stack.top - 1]);
			yyval.value = (flag ? SqlForeignKeyAction.NoAction : SqlForeignKeyAction.None);
			break;
		}
		case 769:
			yyval.value = SqlForeignKeyAction.None;
			break;
		case 770:
			yyval.value = value_stack.array[value_stack.top - 1].value;
			break;
		case 771:
			yyval.value = value_stack.array[value_stack.top - 1].value;
			break;
		case 772:
			yyval.value = (int)value_stack.array[value_stack.top - 2].value + (int)value_stack.array[value_stack.top - 1].value;
			break;
		case 773:
			yyval.value = (int)value_stack.array[value_stack.top - 2].value + (int)value_stack.array[value_stack.top - 1].value;
			break;
		case 783:
		{
			SqlUniqueConstraintBase sqlUniqueConstraintBase = SqlUniqueConstraintBase.Create((SqlConstraintType)value_stack.array[value_stack.top - 6].value, (SqlClusterOption)value_stack.array[value_stack.top - 5].value, value_stack.array[value_stack.top - 4].value as List<SqlIndexedColumn>, value_stack.array[value_stack.top - 3].value as List<SqlIndexOption>);
			sqlUniqueConstraintBase.Position = yyloc;
			yyval.value = sqlUniqueConstraintBase;
			break;
		}
		case 784:
		{
			SqlInlineIndexConstraint sqlInlineIndexConstraint3 = new SqlInlineIndexConstraint(value_stack.array[value_stack.top - 3].value as SqlIdentifier);
			sqlInlineIndexConstraint3.Position = yyloc;
			yyval.value = sqlInlineIndexConstraint3;
			break;
		}
		case 785:
		{
			SqlInlineIndexConstraint sqlInlineIndexConstraint2 = new SqlInlineIndexConstraint(value_stack.array[value_stack.top - 4].value as SqlIdentifier);
			sqlInlineIndexConstraint2.Position = yyloc;
			yyval.value = sqlInlineIndexConstraint2;
			break;
		}
		case 786:
		{
			SqlInlineIndexConstraint sqlInlineIndexConstraint = new SqlInlineIndexConstraint(value_stack.array[value_stack.top - 10].value as SqlIdentifier, value_stack.array[value_stack.top - 3].value as List<SqlIndexOption>);
			sqlInlineIndexConstraint.Position = yyloc;
			yyval.value = sqlInlineIndexConstraint;
			break;
		}
		case 787:
			yyval.value = SqlClusterOption.Default;
			break;
		case 788:
			yyval.value = SqlClusterOption.Clustered;
			break;
		case 789:
			yyval.value = SqlClusterOption.NonClustered;
			break;
		case 790:
			yyval.value = SqlClusterOption.NonClusteredHash;
			break;
		case 791:
			yyval.value = SqlClusterOption.Hash;
			break;
		case 792:
			yyval.value = SqlClusterOption.ClusteredColumnstore;
			break;
		case 793:
			yyval.value = SqlClusterOption.NonClusteredColumnstore;
			break;
		case 795:
			yyval.value = SqlConstraintType.Unique;
			break;
		case 796:
			yyval.value = SqlConstraintType.PrimaryKey;
			break;
		case 798:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 799:
		{
			List<SqlIdentifier> list = new List<SqlIdentifier>();
			list.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list;
			break;
		}
		case 752:
		case 754:
		case 758:
		case 759:
		case 760:
		case 774:
		case 775:
		case 776:
		case 777:
		case 778:
		case 779:
		case 780:
		case 781:
		case 782:
		case 794:
		case 797:
			break;
		}
	}

	private void DoAction_800_849(int action)
	{
		switch (action)
		{
		case 800:
		{
			List<SqlIdentifier> list7 = value_stack.array[value_stack.top - 3].value as List<SqlIdentifier>;
			list7.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list7;
			break;
		}
		case 802:
		{
			SqlConstraint sqlConstraint2 = SqlConstraint.Null();
			sqlConstraint2.Position = yyloc;
			yyval.value = sqlConstraint2;
			break;
		}
		case 803:
		{
			SqlConstraint sqlConstraint = SqlConstraint.NotNull();
			sqlConstraint.Position = yyloc;
			yyval.value = sqlConstraint;
			break;
		}
		case 804:
		{
			SqlColumnIdentity sqlColumnIdentity2 = new SqlColumnIdentity();
			sqlColumnIdentity2.Position = yyloc;
			yyval.value = sqlColumnIdentity2;
			break;
		}
		case 805:
		{
			SqlColumnIdentity sqlColumnIdentity = new SqlColumnIdentity(value_stack.array[value_stack.top - 4].value as SqlLiteralExpression, value_stack.array[value_stack.top - 2].value as SqlLiteralExpression);
			sqlColumnIdentity.Position = yyloc;
			yyval.value = sqlColumnIdentity;
			break;
		}
		case 806:
		{
			SqlLiteralExpression sqlLiteralExpression2 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]), (bool)value_stack.array[value_stack.top - 2].value);
			sqlLiteralExpression2.Position = yyloc;
			yyval.value = sqlLiteralExpression2;
			break;
		}
		case 807:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Numeric(GetText(location_stack.array[location_stack.top - 1]), (bool)value_stack.array[value_stack.top - 2].value);
			sqlLiteralExpression.Position = yyloc;
			yyval.value = sqlLiteralExpression;
			break;
		}
		case 808:
			yyval.value = false;
			break;
		case 809:
			yyval.value = false;
			break;
		case 810:
			yyval.value = true;
			break;
		case 828:
			ExpectKeyword(StringToken.Partitions, location_stack.array[location_stack.top - 4]);
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 829:
		{
			List<SqlCompressionPartitionRange> list6 = new List<SqlCompressionPartitionRange>();
			list6.Add(value_stack.array[value_stack.top - 1].value as SqlCompressionPartitionRange);
			yyval.value = list6;
			break;
		}
		case 830:
		{
			List<SqlCompressionPartitionRange> list5 = value_stack.array[value_stack.top - 3].value as List<SqlCompressionPartitionRange>;
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlCompressionPartitionRange);
			yyval.value = list5;
			break;
		}
		case 831:
		{
			SqlCompressionPartitionRange sqlCompressionPartitionRange2 = new SqlCompressionPartitionRange(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlCompressionPartitionRange2.Position = yyloc;
			yyval.value = sqlCompressionPartitionRange2;
			break;
		}
		case 832:
		{
			SqlCompressionPartitionRange sqlCompressionPartitionRange = new SqlCompressionPartitionRange(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlCompressionPartitionRange.Position = yyloc;
			yyval.value = sqlCompressionPartitionRange;
			break;
		}
		case 835:
		{
			List<SqlIdentifier> list4 = new List<SqlIdentifier>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list4;
			break;
		}
		case 836:
		{
			List<SqlIdentifier> list3 = value_stack.array[value_stack.top - 3].value as List<SqlIdentifier>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list3;
			break;
		}
		case 837:
		{
			List<SqlIdentifier> list2 = new List<SqlIdentifier>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list2;
			break;
		}
		case 838:
		{
			List<SqlIdentifier> list = value_stack.array[value_stack.top - 3].value as List<SqlIdentifier>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list;
			break;
		}
		case 840:
		{
			SqlIdentifier sqlIdentifier = new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlIdentifier.Position = yyloc;
			yyval.value = sqlIdentifier;
			break;
		}
		case 848:
		{
			SqlComputedColumnDefinition sqlComputedColumnDefinition = new SqlComputedColumnDefinition(value_stack.array[value_stack.top - 4].value as SqlIdentifier, value_stack.array[value_stack.top - 2].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value != null);
			sqlComputedColumnDefinition.Position = yyloc;
			yyval.value = sqlComputedColumnDefinition;
			break;
		}
		case 801:
		case 811:
		case 812:
		case 813:
		case 814:
		case 815:
		case 816:
		case 817:
		case 818:
		case 819:
		case 820:
		case 821:
		case 822:
		case 823:
		case 824:
		case 825:
		case 826:
		case 827:
		case 833:
		case 834:
		case 839:
		case 841:
		case 842:
		case 843:
		case 844:
		case 845:
		case 846:
		case 847:
		case 849:
			break;
		}
	}

	private void DoAction_850_899(int action)
	{
		switch (action)
		{
		case 850:
		{
			bool flag = ExpectKeyword(StringToken.Persisted, location_stack.array[location_stack.top - 1]);
			yyval.value = (flag ? "not null" : null);
			break;
		}
		case 851:
		{
			SqlInlineTableVariableDeclareStatement sqlInlineTableVariableDeclareStatement = new SqlInlineTableVariableDeclareStatement(new SqlInlineTableVariableDeclaration(GetText(location_stack.array[location_stack.top - 4]), value_stack.array[value_stack.top - 1].value as SqlTableDefinition)
			{
				Position = MergeLocationRange(location_stack.top - 4, location_stack.top - 1)
			});
			sqlInlineTableVariableDeclareStatement.Position = yyloc;
			yyval.value = sqlInlineTableVariableDeclareStatement;
			break;
		}
		case 852:
		{
			SqlTableDefinition sqlTableDefinition = new SqlTableDefinition(value_stack.array[value_stack.top - 2].value as List<ISqlTableElement>);
			sqlTableDefinition.Position = yyloc;
			yyval.value = sqlTableDefinition;
			break;
		}
		case 853:
		case 854:
		case 855:
		case 856:
		case 857:
		case 858:
		case 859:
		case 860:
		case 861:
		case 862:
		case 863:
		case 864:
		case 865:
		case 866:
		case 867:
		case 868:
		case 869:
		case 870:
		case 871:
		case 872:
		case 873:
		case 874:
		case 875:
		case 876:
		case 877:
		case 878:
		case 879:
		case 880:
		case 881:
		case 882:
		case 883:
		case 884:
		case 885:
		case 886:
		case 887:
		case 888:
		case 889:
		case 890:
		case 891:
		case 892:
		case 893:
		case 894:
		case 895:
		case 896:
		case 897:
		case 898:
		case 899:
			break;
		}
	}

	private void DoAction_900_949(int action)
	{
		switch (action)
		{
		case 902:
			yyval.value = new CreateIndexStart(isUnique: false, SqlClusterOption.Default);
			break;
		case 903:
			yyval.value = new CreateIndexStart(isUnique: true, (SqlClusterOption)value_stack.array[value_stack.top - 3].value);
			break;
		case 904:
			yyval.value = new CreateIndexStart(isUnique: false, SqlClusterOption.Clustered);
			break;
		case 905:
			yyval.value = new CreateIndexStart(isUnique: false, SqlClusterOption.NonClustered);
			break;
		case 906:
			yyval.value = new CreateIndexStart(isUnique: false, SqlClusterOption.Default);
			break;
		case 907:
		{
			SqlCreateIndexStatement sqlCreateIndexStatement = new SqlCreateIndexStatement(value_stack.array[value_stack.top - 10].value as CreateIndexStart, value_stack.array[value_stack.top - 9].value as SqlIdentifier, value_stack.array[value_stack.top - 7].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 6].value as CreateIndexKeyList, value_stack.array[value_stack.top - 4].value as SqlFilterClause, value_stack.array[value_stack.top - 3].value as List<SqlIndexOption>, value_stack.array[value_stack.top - 2].value as SqlStorageSpecification, value_stack.array[value_stack.top - 1].value as SqlStorageSpecification);
			sqlCreateIndexStatement.Position = yyloc;
			yyval.value = sqlCreateIndexStatement;
			if ((value_stack.array[value_stack.top - 10].value as CreateIndexStart).ClusterOption == SqlClusterOption.Clustered && value_stack.array[value_stack.top - 4].value is SqlFilterClause)
			{
				VersionedParserBase.SyntaxError("WHERE", location_stack.array[location_stack.top - 4]);
			}
			break;
		}
		case 909:
		{
			SqlFilterClause sqlFilterClause = new SqlFilterClause(value_stack.array[value_stack.top - 1].value as SqlFilterExpression);
			sqlFilterClause.Position = yyloc;
			yyval.value = sqlFilterClause;
			break;
		}
		case 912:
			scanner.SetParseState(EParseState.e_ParseIdxInclude);
			break;
		case 913:
			yyval.value = false;
			break;
		case 914:
			yyval.value = true;
			break;
		case 915:
			yyval.value = SqlClusterOption.Default;
			break;
		case 916:
			yyval.value = SqlClusterOption.NonClustered;
			break;
		case 917:
			yyval.value = SqlClusterOption.Clustered;
			break;
		case 921:
			yyval = value_stack.array[value_stack.top - 3];
			break;
		case 925:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 926:
		{
			List<SqlIndexedColumn> list8 = new List<SqlIndexedColumn>();
			list8.Add(value_stack.array[value_stack.top - 1].value as SqlIndexedColumn);
			yyval.value = list8;
			break;
		}
		case 927:
		{
			List<SqlIndexedColumn> list7 = value_stack.array[value_stack.top - 3].value as List<SqlIndexedColumn>;
			list7.Add(value_stack.array[value_stack.top - 1].value as SqlIndexedColumn);
			yyval.value = list7;
			break;
		}
		case 928:
		{
			SqlIndexedColumn sqlIndexedColumn2 = new SqlIndexedColumn(value_stack.array[value_stack.top - 2].value as SqlIdentifier, (SqlSortOrder)value_stack.array[value_stack.top - 1].value);
			sqlIndexedColumn2.Position = yyloc;
			yyval.value = sqlIndexedColumn2;
			break;
		}
		case 929:
		{
			SqlIndexedColumn sqlIndexedColumn = new SqlIndexedColumn(new SqlIdentifier(GetText(location_stack.array[location_stack.top - 2]))
			{
				Position = location_stack.array[location_stack.top - 2]
			}, (SqlSortOrder)value_stack.array[value_stack.top - 1].value);
			sqlIndexedColumn.Position = yyloc;
			yyval.value = sqlIndexedColumn;
			break;
		}
		case 931:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 932:
		{
			List<SqlIndexOption> list6 = new List<SqlIndexOption>();
			list6.Add(value_stack.array[value_stack.top - 1].value as SqlIndexOption);
			yyval.value = list6;
			break;
		}
		case 933:
		{
			List<SqlIndexOption> list5 = new List<SqlIndexOption>();
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlIndexOption);
			yyval.value = list5;
			break;
		}
		case 934:
		{
			List<SqlIndexOption> list4 = new List<SqlIndexOption>();
			list4.Add(value_stack.array[value_stack.top - 2].value as SqlIndexOption);
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlIndexOption);
			yyval.value = list4;
			break;
		}
		case 935:
		{
			List<SqlIndexOption> list3 = new List<SqlIndexOption>();
			list3.Add(value_stack.array[value_stack.top - 2].value as SqlIndexOption);
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlIndexOption);
			yyval.value = list3;
			break;
		}
		case 937:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 939:
			yyval.value = new CreateIndexKeyList(value_stack.array[value_stack.top - 3].value as List<SqlIndexedColumn>, value_stack.array[value_stack.top - 1].value as List<SqlIdentifier>);
			break;
		case 941:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 942:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 943:
		{
			List<SqlIndexOption> list2 = new List<SqlIndexOption>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlIndexOption);
			yyval.value = list2;
			break;
		}
		case 944:
		{
			List<SqlIndexOption> list = value_stack.array[value_stack.top - 3].value as List<SqlIndexOption>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlIndexOption);
			yyval.value = list;
			break;
		}
		case 945:
		{
			SqlIndexOption sqlIndexOption;
			switch (GetStringToken(location_stack.array[location_stack.top - 1]))
			{
			case StringToken.PadIndex:
				sqlIndexOption = new SqlPadIndexOption(SqlOnOffValue.None);
				break;
			case StringToken.SortInTempDb:
				sqlIndexOption = new SqlSortInTempDbIndexOption(SqlOnOffValue.None);
				break;
			case StringToken.IgnoreDupKey:
				sqlIndexOption = new SqlIgnoreDupKeyIndexOption(SqlOnOffValue.None);
				break;
			case StringToken.StatisticsNoRecompute:
				sqlIndexOption = new SqlStatisticsNoRecomputeIndexOption(SqlOnOffValue.None);
				break;
			case StringToken.DropExisting:
				sqlIndexOption = new SqlDropExistingIndexOption(SqlOnOffValue.None);
				break;
			default:
				sqlIndexOption = new SqlIndexOptionError();
				VersionedParserBase.ReportError(SR.InvalidUsageOfIndexOption(GetText(location_stack.array[location_stack.top - 1])), location_stack.array[location_stack.top - 1]);
				break;
			}
			sqlIndexOption.Position = yyloc;
			yyval.value = sqlIndexOption;
			break;
		}
		case 948:
		{
			SqlFillFactorIndexOption sqlFillFactorIndexOption = new SqlFillFactorIndexOption(value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			sqlFillFactorIndexOption.Position = yyloc;
			yyval.value = sqlFillFactorIndexOption;
			break;
		}
		case 949:
		{
			VersionedParserBase.ReportError(SR.InvalidUsageOfIndexOption(GetText(location_stack.array[location_stack.top - 3])), location_stack.array[location_stack.top - 3]);
			SqlMaxDegreeOfParallelismIndexOption sqlMaxDegreeOfParallelismIndexOption = new SqlMaxDegreeOfParallelismIndexOption((SqlLiteralExpression)value_stack.array[value_stack.top - 1].value, isValid: true);
			sqlMaxDegreeOfParallelismIndexOption.Position = yyloc;
			yyval.value = sqlMaxDegreeOfParallelismIndexOption;
			break;
		}
		case 900:
		case 901:
		case 908:
		case 910:
		case 911:
		case 918:
		case 919:
		case 920:
		case 922:
		case 923:
		case 924:
		case 930:
		case 936:
		case 938:
		case 940:
		case 946:
		case 947:
			break;
		}
	}

	private void DoAction_950_999(int action)
	{
		switch (action)
		{
		case 950:
		{
			SqlIndexOption sqlIndexOption4 = new SqlSortedDataIndexOption(SqlOnOffValue.None);
			sqlIndexOption4.Position = yyloc;
			yyval.value = sqlIndexOption4;
			break;
		}
		case 951:
		{
			SqlIndexOption sqlIndexOption2 = new SqlSortedDataReorgIndexOption(SqlOnOffValue.None);
			sqlIndexOption2.Position = yyloc;
			yyval.value = sqlIndexOption2;
			break;
		}
		case 970:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 973:
		{
			List<SqlIndexOption> list2 = new List<SqlIndexOption>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlIndexOption);
			yyval.value = list2;
			break;
		}
		case 974:
		{
			List<SqlIndexOption> list = value_stack.array[value_stack.top - 3].value as List<SqlIndexOption>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlIndexOption);
			yyval.value = list;
			break;
		}
		case 980:
		{
			SqlOnlineIndexOption sqlOnlineIndexOption = new SqlOnlineIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 2].value);
			sqlOnlineIndexOption.Position = yyloc;
			yyval.value = sqlOnlineIndexOption;
			break;
		}
		case 982:
		{
			SqlIndexOption sqlIndexOption3;
			switch (GetStringToken(location_stack.array[location_stack.top - 3]))
			{
			case StringToken.PadIndex:
				sqlIndexOption3 = new SqlPadIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			case StringToken.SortInTempDb:
				sqlIndexOption3 = new SqlSortInTempDbIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			case StringToken.IgnoreDupKey:
				sqlIndexOption3 = new SqlIgnoreDupKeyIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			case StringToken.StatisticsIncremental:
				sqlIndexOption3 = new SqlStatisticsIncrementalIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			case StringToken.StatisticsNoRecompute:
				sqlIndexOption3 = new SqlStatisticsNoRecomputeIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			case StringToken.DropExisting:
				sqlIndexOption3 = new SqlDropExistingIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			case StringToken.AllowRowLocks:
				sqlIndexOption3 = new SqlAllowRowLocksIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			case StringToken.AllowPageLocks:
				sqlIndexOption3 = new SqlAllowPageLocksIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			case StringToken.SortedData:
				sqlIndexOption3 = new SqlSortedDataIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			case StringToken.SortedDataReorg:
				sqlIndexOption3 = new SqlSortedDataReorgIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			case StringToken.OptimizeForSequentialKey:
				sqlIndexOption3 = new SqlOptimizeForSequentialKeyIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			default:
				sqlIndexOption3 = new SqlIndexOptionError();
				VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 3]);
				break;
			}
			sqlIndexOption3.Position = yyloc;
			yyval.value = sqlIndexOption3;
			break;
		}
		case 983:
		{
			SqlIndexOptionError sqlIndexOptionError3 = new SqlIndexOptionError();
			sqlIndexOptionError3.Position = yyloc;
			yyval.value = sqlIndexOptionError3;
			VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 1]);
			break;
		}
		case 984:
		{
			SqlMaxDegreeOfParallelismIndexOption sqlMaxDegreeOfParallelismIndexOption = new SqlMaxDegreeOfParallelismIndexOption(value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			sqlMaxDegreeOfParallelismIndexOption.Position = yyloc;
			yyval.value = sqlMaxDegreeOfParallelismIndexOption;
			break;
		}
		case 985:
		{
			SqlStatisticsOnlyIndexOption sqlStatisticsOnlyIndexOption2 = new SqlStatisticsOnlyIndexOption(value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			sqlStatisticsOnlyIndexOption2.Position = yyloc;
			yyval.value = sqlStatisticsOnlyIndexOption2;
			break;
		}
		case 986:
		{
			SqlStatisticsOnlyIndexOption sqlStatisticsOnlyIndexOption = new SqlStatisticsOnlyIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
			sqlStatisticsOnlyIndexOption.Position = yyloc;
			yyval.value = sqlStatisticsOnlyIndexOption;
			break;
		}
		case 987:
		{
			List<SqlCompressionPartitionRange> partitions = value_stack.array[value_stack.top - 1].value as List<SqlCompressionPartitionRange>;
			SqlIndexOption sqlIndexOption;
			switch (GetStringToken(location_stack.array[location_stack.top - 2]))
			{
			case StringToken.NONE:
				sqlIndexOption = new SqlDataCompressionIndexOption(SqlDataCompressionType.None, partitions);
				break;
			case StringToken.Row:
				sqlIndexOption = new SqlDataCompressionIndexOption(SqlDataCompressionType.Row, partitions);
				break;
			case StringToken.Page:
				sqlIndexOption = new SqlDataCompressionIndexOption(SqlDataCompressionType.Page, partitions);
				break;
			case StringToken.ColumnStore:
				sqlIndexOption = new SqlDataCompressionIndexOption(SqlDataCompressionType.ColumnStore, partitions);
				break;
			case StringToken.ColumnStoreArchive:
				sqlIndexOption = new SqlDataCompressionIndexOption(SqlDataCompressionType.ColumnStoreArchive, partitions);
				break;
			default:
				sqlIndexOption = new SqlDataCompressionIndexOption(SqlDataCompressionType.Invalid, partitions);
				VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 2]);
				break;
			}
			sqlIndexOption.Position = yyloc;
			yyval.value = sqlIndexOption;
			break;
		}
		case 991:
		{
			SqlBucketCountIndexOption sqlBucketCountIndexOption = new SqlBucketCountIndexOption(value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			sqlBucketCountIndexOption.Position = yyloc;
			yyval.value = sqlBucketCountIndexOption;
			break;
		}
		case 992:
		{
			SqlCompressionDelayIndexOption sqlCompressionDelayIndexOption2 = new SqlCompressionDelayIndexOption(value_stack.array[value_stack.top - 2].value as SqlLiteralExpression);
			sqlCompressionDelayIndexOption2.Position = yyloc;
			yyval.value = sqlCompressionDelayIndexOption2;
			break;
		}
		case 993:
		{
			SqlIndexOptionError sqlIndexOptionError2 = new SqlIndexOptionError();
			sqlIndexOptionError2.Position = yyloc;
			yyval.value = sqlIndexOptionError2;
			break;
		}
		case 994:
		{
			SqlIndexOptionError sqlIndexOptionError = new SqlIndexOptionError();
			sqlIndexOptionError.Position = yyloc;
			yyval.value = sqlIndexOptionError;
			break;
		}
		case 995:
		{
			SqlCompressionDelayIndexOption sqlCompressionDelayIndexOption = new SqlCompressionDelayIndexOption(value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			sqlCompressionDelayIndexOption.Position = yyloc;
			yyval.value = sqlCompressionDelayIndexOption;
			break;
		}
		case 999:
			ExpectKeyword(StringToken.Minutes, location_stack.array[location_stack.top - 1]);
			break;
		case 952:
		case 953:
		case 954:
		case 955:
		case 956:
		case 957:
		case 958:
		case 959:
		case 960:
		case 961:
		case 962:
		case 963:
		case 964:
		case 965:
		case 966:
		case 967:
		case 968:
		case 969:
		case 971:
		case 972:
		case 975:
		case 976:
		case 977:
		case 978:
		case 979:
		case 981:
		case 988:
		case 989:
		case 990:
		case 996:
		case 997:
		case 998:
			break;
		}
	}

	private void DoAction_1000_1049(int action)
	{
		switch (action)
		{
		case 1002:
		{
			SqlCreateUserStatement sqlCreateUserStatement = SqlCreateUserStatement.CreateFromFactory(value_stack.array[value_stack.top - 2].value as CreateUserStatementFactory, value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlCreateUserOption>);
			sqlCreateUserStatement.Position = yyloc;
			yyval.value = sqlCreateUserStatement;
			break;
		}
		case 1004:
			yyval.value = CreateUserStatementFactory.CreateCreateUserWithImplicitAuthenticationStatementFactory();
			break;
		case 1005:
		{
			CreateUserStatementFactory value;
			switch (GetStringToken(location_stack.array[location_stack.top - 2]))
			{
			case StringToken.Login:
				value = CreateUserStatementFactory.CreateCreateUserFromLoginStatementFactory(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
				break;
			case StringToken.Certificate:
				value = CreateUserStatementFactory.CreateCreateUserFromCertificateStatementFactory(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
				break;
			default:
				value = CreateUserStatementFactory.CreateCreateUserStatementErrorFactory();
				VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 2]);
				break;
			}
			yyval.value = value;
			break;
		}
		case 1006:
			yyval.value = CreateUserStatementFactory.CreateCreateUserFromAsymKeyStatementFactory(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			ExpectKeyword(StringToken.Asymmetric, location_stack.array[location_stack.top - 3]);
			break;
		case 1007:
			yyval.value = CreateUserStatementFactory.CreateCreateUserWithoutLoginStatementFactory();
			ExpectKeyword(StringToken.Without, location_stack.array[location_stack.top - 2]);
			ExpectKeyword(StringToken.Login, location_stack.array[location_stack.top - 1]);
			break;
		case 1009:
			yyval.value = CreateUserStatementFactory.CreateCreateUserFromExternalProviderStatementFactory();
			ExpectKeyword(StringToken.Provider, location_stack.array[location_stack.top - 1]);
			break;
		case 1013:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1014:
		{
			List<SqlCreateUserOption> list2 = new List<SqlCreateUserOption>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlCreateUserOption);
			yyval.value = list2;
			break;
		}
		case 1015:
		{
			List<SqlCreateUserOption> list = value_stack.array[value_stack.top - 3].value as List<SqlCreateUserOption>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlCreateUserOption);
			yyval.value = list;
			break;
		}
		case 1016:
		{
			SqlCreateUserOption sqlCreateUserOption4 = new SqlCreateUserOption(GetStringToken(location_stack.array[location_stack.top - 3]), value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			sqlCreateUserOption4.Position = yyloc;
			yyval.value = sqlCreateUserOption4;
			break;
		}
		case 1018:
		{
			SqlCreateUserOption sqlCreateUserOption3 = new SqlCreateUserOption(GetStringToken(location_stack.array[location_stack.top - 3]), value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlCreateUserOption3.Position = yyloc;
			yyval.value = sqlCreateUserOption3;
			StringToken stringToken = GetStringToken(location_stack.array[location_stack.top - 3]);
			if (stringToken != StringToken.Password && stringToken != StringToken.DefaultSchema && stringToken != StringToken.DefaultLanguage)
			{
				VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 3]);
			}
			break;
		}
		case 1019:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression.Position = location_stack.array[location_stack.top - 1];
			SqlCreateUserOption sqlCreateUserOption2 = new SqlCreateUserOption(GetStringToken(location_stack.array[location_stack.top - 3]), sqlLiteralExpression);
			sqlCreateUserOption2.Position = yyloc;
			yyval.value = sqlCreateUserOption2;
			if (GetStringToken(location_stack.array[location_stack.top - 3]) != StringToken.DefaultLanguage)
			{
				VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 3]);
			}
			break;
		}
		case 1022:
		{
			SqlCreateUserOption sqlCreateUserOption = new SqlCreateUserOption(GetStringToken(location_stack.array[location_stack.top - 3]), value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			sqlCreateUserOption.Position = yyloc;
			yyval.value = sqlCreateUserOption;
			switch (GetStringToken(location_stack.array[location_stack.top - 3]))
			{
			case StringToken.DefaultSchema:
				VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 1]);
				break;
			default:
				VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 3]);
				break;
			case StringToken.ObjectId:
				break;
			}
			break;
		}
		case 1035:
		{
			SqlDropUserStatement sqlDropUserStatement = new SqlDropUserStatement(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlDropUserStatement.IsDropIfExists = (bool)value_stack.array[value_stack.top - 2].value;
			sqlDropUserStatement.Position = yyloc;
			yyval.value = sqlDropUserStatement;
			break;
		}
		case 1036:
		{
			SqlCreateRoleStatement sqlCreateRoleStatement = new SqlCreateRoleStatement(value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlCreateRoleStatement.Position = yyloc;
			yyval.value = sqlCreateRoleStatement;
			break;
		}
		case 1000:
		case 1001:
		case 1003:
		case 1008:
		case 1010:
		case 1011:
		case 1012:
		case 1017:
		case 1020:
		case 1021:
		case 1023:
		case 1024:
		case 1025:
		case 1026:
		case 1027:
		case 1028:
		case 1029:
		case 1030:
		case 1031:
		case 1032:
		case 1033:
		case 1034:
		case 1037:
		case 1038:
		case 1039:
		case 1040:
		case 1041:
		case 1042:
		case 1043:
		case 1044:
		case 1045:
		case 1046:
		case 1047:
		case 1048:
		case 1049:
			break;
		}
	}

	private void DoAction_1050_1099(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_1100_1149(int action)
	{
		switch (action)
		{
		case 1145:
		{
			SqlBackupMasterKeyStatement sqlBackupMasterKeyStatement = SqlBackupMasterKeyStatement.Create(isDumpLoadStatement);
			sqlBackupMasterKeyStatement.Position = yyloc;
			yyval.value = sqlBackupMasterKeyStatement;
			break;
		}
		case 1146:
		{
			SqlRestoreMasterKeyStatement sqlRestoreMasterKeyStatement = SqlRestoreMasterKeyStatement.Create(isDumpLoadStatement);
			sqlRestoreMasterKeyStatement.Position = yyloc;
			yyval.value = sqlRestoreMasterKeyStatement;
			break;
		}
		case 1100:
		case 1101:
		case 1102:
		case 1103:
		case 1104:
		case 1105:
		case 1106:
		case 1107:
		case 1108:
		case 1109:
		case 1110:
		case 1111:
		case 1112:
		case 1113:
		case 1114:
		case 1115:
		case 1116:
		case 1117:
		case 1118:
		case 1119:
		case 1120:
		case 1121:
		case 1122:
		case 1123:
		case 1124:
		case 1125:
		case 1126:
		case 1127:
		case 1128:
		case 1129:
		case 1130:
		case 1131:
		case 1132:
		case 1133:
		case 1134:
		case 1135:
		case 1136:
		case 1137:
		case 1138:
		case 1139:
		case 1140:
		case 1141:
		case 1142:
		case 1143:
		case 1144:
		case 1147:
		case 1148:
		case 1149:
			break;
		}
	}

	private void DoAction_1150_1199(int action)
	{
		switch (action)
		{
		case 1160:
		{
			SqlBackupServiceMasterKeyStatement sqlBackupServiceMasterKeyStatement = SqlBackupServiceMasterKeyStatement.Create(isDumpLoadStatement);
			sqlBackupServiceMasterKeyStatement.Position = yyloc;
			yyval.value = sqlBackupServiceMasterKeyStatement;
			break;
		}
		case 1161:
			scanner.SetParseState(EParseState.e_ParseLoad);
			isDumpLoadStatement = false;
			break;
		case 1162:
			scanner.SetParseState(EParseState.e_ParseLoad);
			isDumpLoadStatement = true;
			break;
		case 1163:
		{
			SqlRestoreServiceMasterKeyStatement sqlRestoreServiceMasterKeyStatement = SqlRestoreServiceMasterKeyStatement.Create(isDumpLoadStatement);
			sqlRestoreServiceMasterKeyStatement.Position = yyloc;
			yyval.value = sqlRestoreServiceMasterKeyStatement;
			ExpectKeyword(StringToken.Master, location_stack.array[location_stack.top - 10]);
			ExpectKeyword(StringToken.Decryption, location_stack.array[location_stack.top - 6]);
			ExpectKeyword(StringToken.Password, location_stack.array[location_stack.top - 4]);
			break;
		}
		case 1164:
			scanner.SetParseState(EParseState.e_ParseOpen);
			break;
		case 1165:
			scanner.SetParseState(EParseState.e_ParseClose);
			break;
		case 1186:
		{
			SqlLoginPassword sqlLoginPassword = new SqlLoginPassword(value_stack.array[value_stack.top - 3].value as SqlLiteralExpression, value_stack.array[value_stack.top - 2].value as List<SqlLoginOption>);
			sqlLoginPassword.Position = LexLocation.Merge(location_stack.array[location_stack.top - 3], location_stack.array[location_stack.top - 2]);
			SqlCreateLoginWithPasswordStatement sqlCreateLoginWithPasswordStatement = new SqlCreateLoginWithPasswordStatement(value_stack.array[value_stack.top - 7].value as SqlIdentifier, sqlLoginPassword, value_stack.array[value_stack.top - 1].value as List<SqlLoginOption>);
			sqlCreateLoginWithPasswordStatement.Position = yyloc;
			yyval.value = sqlCreateLoginWithPasswordStatement;
			ExpectKeyword(StringToken.Password, location_stack.array[location_stack.top - 5]);
			break;
		}
		case 1188:
		{
			SqlCreateLoginFromWindowsStatement sqlCreateLoginFromWindowsStatement = new SqlCreateLoginFromWindowsStatement(value_stack.array[value_stack.top - 4].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlLoginOption>);
			sqlCreateLoginFromWindowsStatement.Position = yyloc;
			yyval.value = sqlCreateLoginFromWindowsStatement;
			ExpectKeyword(StringToken.Windows, location_stack.array[location_stack.top - 2]);
			break;
		}
		case 1189:
		{
			SqlCreateLoginFromCertificateStatement sqlCreateLoginFromCertificateStatement = new SqlCreateLoginFromCertificateStatement(value_stack.array[value_stack.top - 5].value as SqlIdentifier, value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlLoginOption>);
			sqlCreateLoginFromCertificateStatement.Position = yyloc;
			yyval.value = sqlCreateLoginFromCertificateStatement;
			ExpectKeyword(StringToken.Certificate, location_stack.array[location_stack.top - 3]);
			break;
		}
		case 1190:
		{
			SqlCreateLoginFromAsymKeyStatement sqlCreateLoginFromAsymKeyStatement = new SqlCreateLoginFromAsymKeyStatement(value_stack.array[value_stack.top - 6].value as SqlIdentifier, value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlLoginOption>);
			sqlCreateLoginFromAsymKeyStatement.Position = yyloc;
			yyval.value = sqlCreateLoginFromAsymKeyStatement;
			ExpectKeyword(StringToken.Asymmetric, location_stack.array[location_stack.top - 4]);
			break;
		}
		case 1192:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Binary(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression.Position = yyloc;
			yyval.value = sqlLiteralExpression;
			break;
		}
		case 1195:
		{
			List<SqlLoginOption> list2 = new List<SqlLoginOption>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list2;
			break;
		}
		case 1196:
		{
			List<SqlLoginOption> list = value_stack.array[value_stack.top - 2].value as List<SqlLoginOption>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list;
			break;
		}
		case 1197:
		{
			SqlLoginOption sqlLoginOption = SqlLoginOption.Create(GetText(location_stack.array[location_stack.top - 1]), SqlLoginOptionContext.PasswordContext);
			sqlLoginOption.Position = yyloc;
			yyval.value = sqlLoginOption;
			break;
		}
		case 1150:
		case 1151:
		case 1152:
		case 1153:
		case 1154:
		case 1155:
		case 1156:
		case 1157:
		case 1158:
		case 1159:
		case 1166:
		case 1167:
		case 1168:
		case 1169:
		case 1170:
		case 1171:
		case 1172:
		case 1173:
		case 1174:
		case 1175:
		case 1176:
		case 1177:
		case 1178:
		case 1179:
		case 1180:
		case 1181:
		case 1182:
		case 1183:
		case 1184:
		case 1185:
		case 1187:
		case 1191:
		case 1193:
		case 1194:
		case 1198:
		case 1199:
			break;
		}
	}

	private void DoAction_1200_1249(int action)
	{
		switch (action)
		{
		case 1200:
		{
			List<SqlLoginOption> list11 = new List<SqlLoginOption>();
			list11.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list11;
			break;
		}
		case 1201:
		{
			List<SqlLoginOption> list10 = value_stack.array[value_stack.top - 3].value as List<SqlLoginOption>;
			list10.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list10;
			break;
		}
		case 1202:
		{
			SqlLoginOption sqlLoginOption13 = SqlLoginOption.Create(GetText(location_stack.array[location_stack.top - 3]), (SqlOnOffValue)value_stack.array[value_stack.top - 1].value, SqlLoginOptionContext.WithPasswordContext);
			sqlLoginOption13.Position = yyloc;
			yyval.value = sqlLoginOption13;
			break;
		}
		case 1203:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Binary(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression.Position = location_stack.array[location_stack.top - 1];
			SqlLoginOption sqlLoginOption12 = SqlLoginOption.Create(GetText(location_stack.array[location_stack.top - 3]), sqlLiteralExpression, SqlLoginOptionContext.WithPasswordContext);
			sqlLoginOption12.Position = yyloc;
			yyval.value = sqlLoginOption12;
			break;
		}
		case 1204:
		{
			SqlLoginOption sqlLoginOption11 = SqlLoginOption.Create(GetText(location_stack.array[location_stack.top - 3]), value_stack.array[value_stack.top - 1].value as SqlIdentifier, SqlLoginOptionContext.WithPasswordContext);
			sqlLoginOption11.Position = yyloc;
			yyval.value = sqlLoginOption11;
			break;
		}
		case 1215:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1216:
		{
			List<SqlLoginOption> list9 = new List<SqlLoginOption>();
			list9.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list9;
			break;
		}
		case 1217:
		{
			List<SqlLoginOption> list8 = value_stack.array[value_stack.top - 3].value as List<SqlLoginOption>;
			list8.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list8;
			break;
		}
		case 1218:
		{
			SqlLoginOption sqlLoginOption10 = SqlLoginOption.Create(GetText(location_stack.array[location_stack.top - 3]), value_stack.array[value_stack.top - 1].value as SqlIdentifier, SqlLoginOptionContext.FromWindowsContext);
			sqlLoginOption10.Position = yyloc;
			yyval.value = sqlLoginOption10;
			break;
		}
		case 1220:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1221:
		{
			List<SqlLoginOption> list7 = new List<SqlLoginOption>();
			list7.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list7;
			break;
		}
		case 1222:
		{
			List<SqlLoginOption> list6 = value_stack.array[value_stack.top - 3].value as List<SqlLoginOption>;
			list6.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list6;
			break;
		}
		case 1223:
		{
			SqlLoginOption sqlLoginOption9 = SqlLoginOption.Create(GetText(location_stack.array[location_stack.top - 3]), value_stack.array[value_stack.top - 1].value as SqlIdentifier, SqlLoginOptionContext.FromCertificateContext);
			sqlLoginOption9.Position = yyloc;
			yyval.value = sqlLoginOption9;
			break;
		}
		case 1224:
		{
			SqlAlterLoginStatement sqlAlterLoginStatement2 = new SqlAlterLoginStatement(value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlLoginOption>);
			sqlAlterLoginStatement2.Position = yyloc;
			yyval.value = sqlAlterLoginStatement2;
			break;
		}
		case 1225:
		{
			SqlAlterLoginStatement sqlAlterLoginStatement = new SqlAlterLoginStatement(value_stack.array[value_stack.top - 2].value as SqlIdentifier);
			sqlAlterLoginStatement.Position = yyloc;
			yyval.value = sqlAlterLoginStatement;
			break;
		}
		case 1226:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1227:
		{
			List<SqlLoginOption> list5 = new List<SqlLoginOption>();
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list5;
			break;
		}
		case 1228:
		{
			List<SqlLoginOption> list4 = new List<SqlLoginOption>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list4;
			break;
		}
		case 1229:
		{
			List<SqlLoginOption> list3 = value_stack.array[value_stack.top - 3].value as List<SqlLoginOption>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list3;
			break;
		}
		case 1230:
		{
			SqlLoginOption sqlLoginOption8 = SqlLoginOption.Create(new SqlLoginPassword(value_stack.array[value_stack.top - 4].value as SqlLiteralExpression, null)
			{
				Position = location_stack.array[location_stack.top - 4]
			}, new SqlLoginPassword(value_stack.array[value_stack.top - 1].value as SqlLiteralExpression, null)
			{
				Position = location_stack.array[location_stack.top - 1]
			});
			sqlLoginOption8.Position = yyloc;
			yyval.value = sqlLoginOption8;
			ExpectKeyword(StringToken.Password, location_stack.array[location_stack.top - 6]);
			break;
		}
		case 1231:
		{
			SqlLoginOption sqlLoginOption7 = SqlLoginOption.Create(new SqlLoginPassword(value_stack.array[value_stack.top - 2].value as SqlLiteralExpression, value_stack.array[value_stack.top - 1].value as List<SqlLoginOption>)
			{
				Position = LexLocation.Merge(location_stack.array[location_stack.top - 2], location_stack.array[location_stack.top - 1])
			}, null);
			sqlLoginOption7.Position = yyloc;
			yyval.value = sqlLoginOption7;
			ExpectKeyword(StringToken.Password, location_stack.array[location_stack.top - 4]);
			break;
		}
		case 1232:
		{
			SqlLoginOption sqlLoginOption6 = SqlLoginOption.Create(GetText(location_stack.array[location_stack.top - 3]), (SqlOnOffValue)value_stack.array[value_stack.top - 1].value, SqlLoginOptionContext.AlterOptionsContext);
			sqlLoginOption6.Position = yyloc;
			yyval.value = sqlLoginOption6;
			break;
		}
		case 1233:
		{
			SqlLoginOption sqlLoginOption5 = SqlLoginOption.Create(GetText(location_stack.array[location_stack.top - 3]), value_stack.array[value_stack.top - 1].value as SqlIdentifier, SqlLoginOptionContext.AlterOptionsContext);
			sqlLoginOption5.Position = yyloc;
			yyval.value = sqlLoginOption5;
			break;
		}
		case 1234:
		{
			SqlLoginOption sqlLoginOption4 = SqlLoginOption.Create(SqlLoginOptionType.NoCredential);
			sqlLoginOption4.Position = yyloc;
			yyval.value = sqlLoginOption4;
			ExpectKeyword(StringToken.No, location_stack.array[location_stack.top - 2]);
			ExpectKeyword(StringToken.Credential, location_stack.array[location_stack.top - 1]);
			break;
		}
		case 1235:
		{
			SqlLoginOption sqlLoginOption3 = SqlLoginOption.Create(SqlModificationType.Add, GetText(location_stack.array[location_stack.top - 2]), value_stack.array[value_stack.top - 1].value as SqlIdentifier, SqlLoginOptionContext.FromCertificateContext);
			sqlLoginOption3.Position = yyloc;
			yyval.value = sqlLoginOption3;
			ExpectKeyword(StringToken.Credential, location_stack.array[location_stack.top - 2]);
			break;
		}
		case 1236:
		{
			SqlLoginOption sqlLoginOption2 = SqlLoginOption.Create(SqlModificationType.Drop, GetText(location_stack.array[location_stack.top - 2]), value_stack.array[value_stack.top - 1].value as SqlIdentifier, SqlLoginOptionContext.FromCertificateContext);
			sqlLoginOption2.Position = yyloc;
			yyval.value = sqlLoginOption2;
			ExpectKeyword(StringToken.Credential, location_stack.array[location_stack.top - 2]);
			break;
		}
		case 1238:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1239:
		{
			List<SqlLoginOption> list2 = new List<SqlLoginOption>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list2;
			break;
		}
		case 1240:
		{
			List<SqlLoginOption> list = value_stack.array[value_stack.top - 2].value as List<SqlLoginOption>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list;
			break;
		}
		case 1241:
		{
			SqlLoginOption sqlLoginOption = SqlLoginOption.Create(GetText(location_stack.array[location_stack.top - 1]), SqlLoginOptionContext.PasswordContext);
			sqlLoginOption.Position = yyloc;
			yyval.value = sqlLoginOption;
			break;
		}
		case 1242:
		{
			SqlDropLoginStatement sqlDropLoginStatement = new SqlDropLoginStatement(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlDropLoginStatement.Position = yyloc;
			yyval.value = sqlDropLoginStatement;
			break;
		}
		case 1205:
		case 1206:
		case 1207:
		case 1208:
		case 1209:
		case 1210:
		case 1211:
		case 1212:
		case 1213:
		case 1214:
		case 1219:
		case 1237:
		case 1243:
		case 1244:
		case 1245:
		case 1246:
		case 1247:
		case 1248:
		case 1249:
			break;
		}
	}

	private void DoAction_1250_1299(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_1300_1349(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_1350_1399(int action)
	{
		switch (action)
		{
		case 1371:
			scanner.SetParseState(EParseState.e_ParseDump);
			isDumpLoadStatement = false;
			break;
		case 1372:
			scanner.SetParseState(EParseState.e_ParseDump);
			isDumpLoadStatement = true;
			break;
		case 1373:
		{
			SqlBackupCertificateStatement sqlBackupCertificateStatement = SqlBackupCertificateStatement.Create(isDumpLoadStatement);
			sqlBackupCertificateStatement.Position = yyloc;
			yyval.value = sqlBackupCertificateStatement;
			break;
		}
		case 1386:
			scanner.ClearQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 1350:
		case 1351:
		case 1352:
		case 1353:
		case 1354:
		case 1355:
		case 1356:
		case 1357:
		case 1358:
		case 1359:
		case 1360:
		case 1361:
		case 1362:
		case 1363:
		case 1364:
		case 1365:
		case 1366:
		case 1367:
		case 1368:
		case 1369:
		case 1370:
		case 1374:
		case 1375:
		case 1376:
		case 1377:
		case 1378:
		case 1379:
		case 1380:
		case 1381:
		case 1382:
		case 1383:
		case 1384:
		case 1385:
		case 1387:
		case 1388:
		case 1389:
		case 1390:
		case 1391:
		case 1392:
		case 1393:
		case 1394:
		case 1395:
		case 1396:
		case 1397:
		case 1398:
		case 1399:
			break;
		}
	}

	private void DoAction_1400_1449(int action)
	{
		switch (action)
		{
		case 1429:
			scanner.ClearQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 1400:
		case 1401:
		case 1402:
		case 1403:
		case 1404:
		case 1405:
		case 1406:
		case 1407:
		case 1408:
		case 1409:
		case 1410:
		case 1411:
		case 1412:
		case 1413:
		case 1414:
		case 1415:
		case 1416:
		case 1417:
		case 1418:
		case 1419:
		case 1420:
		case 1421:
		case 1422:
		case 1423:
		case 1424:
		case 1425:
		case 1426:
		case 1427:
		case 1428:
		case 1430:
		case 1431:
		case 1432:
		case 1433:
		case 1434:
		case 1435:
		case 1436:
		case 1437:
		case 1438:
		case 1439:
		case 1440:
		case 1441:
		case 1442:
		case 1443:
		case 1444:
		case 1445:
		case 1446:
		case 1447:
		case 1448:
		case 1449:
			break;
		}
	}

	private void DoAction_1450_1499(int action)
	{
		switch (action)
		{
		case 1475:
		{
			SqlProcedureDefinitionForCreate sqlProcedureDefinitionForCreate = new SqlProcedureDefinitionForCreate(value_stack.array[value_stack.top - 1].value as SqlProcedureDefinition, (bool)value_stack.array[value_stack.top - 3].value);
			sqlProcedureDefinitionForCreate.Position = yyloc;
			yyval.value = sqlProcedureDefinitionForCreate;
			break;
		}
		case 1476:
			yyval.value = value_stack.array[value_stack.top - 1].value;
			break;
		case 1478:
		{
			SqlCreateProcedureStatement sqlCreateProcedureStatement = new SqlCreateProcedureStatement(value_stack.array[value_stack.top - 3].value as SqlProcedureDefinitionForCreate, value_stack.array[value_stack.top - 1].value as List<SqlStatement>);
			sqlCreateProcedureStatement.Position = yyloc;
			yyval.value = sqlCreateProcedureStatement;
			break;
		}
		case 1480:
		{
			SqlAlterProcedureStatement sqlAlterProcedureStatement = new SqlAlterProcedureStatement(value_stack.array[value_stack.top - 3].value as SqlProcedureDefinition, value_stack.array[value_stack.top - 1].value as List<SqlStatement>);
			sqlAlterProcedureStatement.Position = yyloc;
			yyval.value = sqlAlterProcedureStatement;
			break;
		}
		case 1482:
		{
			SqlProcedureDefinition sqlProcedureDefinition = new SqlProcedureDefinition(value_stack.array[value_stack.top - 6].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 5].value as SqlLiteralExpression, value_stack.array[value_stack.top - 4].value as List<SqlParameterDeclaration>, value_stack.array[value_stack.top - 3].value as List<SqlModuleOption>, value_stack.array[value_stack.top - 2].value != null);
			sqlProcedureDefinition.Position = yyloc;
			yyval.value = sqlProcedureDefinition;
			scanner.SetStmtDelimiter(value: true);
			break;
		}
		case 1485:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 1489:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression.Position = yyloc;
			yyval.value = sqlLiteralExpression;
			break;
		}
		case 1491:
			yyval.value = "for replication";
			break;
		case 1493:
			yyval.value = value_stack.array[value_stack.top - 1].value;
			break;
		case 1494:
		{
			List<SqlModuleOption> list2 = new List<SqlModuleOption>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlModuleOption);
			yyval.value = list2;
			break;
		}
		case 1495:
		{
			List<SqlModuleOption> list = value_stack.array[value_stack.top - 3].value as List<SqlModuleOption>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlModuleOption);
			yyval.value = list;
			break;
		}
		case 1496:
		{
			SqlModuleOption sqlModuleOption2 = SqlModuleOption.Create(GetText(location_stack.array[location_stack.top - 2]));
			sqlModuleOption2.Position = yyloc;
			yyval.value = sqlModuleOption2;
			break;
		}
		case 1497:
		{
			SqlModuleOption sqlModuleOption = SqlModuleOption.ExecuteAs(value_stack.array[value_stack.top - 1].value as SqlExecuteAsClause);
			sqlModuleOption.Position = yyloc;
			yyval.value = sqlModuleOption;
			break;
		}
		case 1450:
		case 1451:
		case 1452:
		case 1453:
		case 1454:
		case 1455:
		case 1456:
		case 1457:
		case 1458:
		case 1459:
		case 1460:
		case 1461:
		case 1462:
		case 1463:
		case 1464:
		case 1465:
		case 1466:
		case 1467:
		case 1468:
		case 1469:
		case 1470:
		case 1471:
		case 1472:
		case 1473:
		case 1474:
		case 1477:
		case 1479:
		case 1481:
		case 1483:
		case 1484:
		case 1486:
		case 1487:
		case 1488:
		case 1490:
		case 1492:
		case 1498:
		case 1499:
			break;
		}
	}

	private void DoAction_1500_1549(int action)
	{
		switch (action)
		{
		case 1509:
		{
			SqlExecuteAsClause sqlExecuteAsClause3 = value_stack.array[value_stack.top - 1].value as SqlExecuteAsClause;
			sqlExecuteAsClause3.Position = yyloc;
			yyval.value = sqlExecuteAsClause3;
			break;
		}
		case 1510:
		{
			SqlExecuteAsClause sqlExecuteAsClause2;
			switch (GetStringToken(location_stack.array[location_stack.top - 1]))
			{
			case StringToken.Caller:
				sqlExecuteAsClause2 = SqlExecuteAsClause.Caller();
				break;
			case StringToken.Owner:
				sqlExecuteAsClause2 = SqlExecuteAsClause.Owner();
				break;
			case StringToken.Self:
				sqlExecuteAsClause2 = SqlExecuteAsClause.Self();
				break;
			default:
				sqlExecuteAsClause2 = SqlExecuteAsClause.Invalid(GetText(location_stack.array[location_stack.top - 1]));
				VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 1]);
				break;
			}
			sqlExecuteAsClause2.Position = yyloc;
			yyval.value = sqlExecuteAsClause2;
			break;
		}
		case 1511:
		{
			SqlExecuteAsClause sqlExecuteAsClause = SqlExecuteAsClause.User(value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			sqlExecuteAsClause.Position = yyloc;
			yyval.value = sqlExecuteAsClause;
			break;
		}
		case 1500:
		case 1501:
		case 1502:
		case 1503:
		case 1504:
		case 1505:
		case 1506:
		case 1507:
		case 1508:
		case 1512:
		case 1513:
		case 1514:
		case 1515:
		case 1516:
		case 1517:
		case 1518:
		case 1519:
		case 1520:
		case 1521:
		case 1522:
		case 1523:
		case 1524:
		case 1525:
		case 1526:
		case 1527:
		case 1528:
		case 1529:
		case 1530:
		case 1531:
		case 1532:
		case 1533:
		case 1534:
		case 1535:
		case 1536:
		case 1537:
		case 1538:
		case 1539:
		case 1540:
		case 1541:
		case 1542:
		case 1543:
		case 1544:
		case 1545:
		case 1546:
		case 1547:
		case 1548:
		case 1549:
			break;
		}
	}

	private void DoAction_1550_1599(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_1600_1649(int action)
	{
		switch (action)
		{
		case 1631:
			scanner.SetParseState(EParseState.e_ParseBegin);
			break;
		case 1600:
		case 1601:
		case 1602:
		case 1603:
		case 1604:
		case 1605:
		case 1606:
		case 1607:
		case 1608:
		case 1609:
		case 1610:
		case 1611:
		case 1612:
		case 1613:
		case 1614:
		case 1615:
		case 1616:
		case 1617:
		case 1618:
		case 1619:
		case 1620:
		case 1621:
		case 1622:
		case 1623:
		case 1624:
		case 1625:
		case 1626:
		case 1627:
		case 1628:
		case 1629:
		case 1630:
		case 1632:
		case 1633:
		case 1634:
		case 1635:
		case 1636:
		case 1637:
		case 1638:
		case 1639:
		case 1640:
		case 1641:
		case 1642:
		case 1643:
		case 1644:
		case 1645:
		case 1646:
		case 1647:
		case 1648:
		case 1649:
			break;
		}
	}

	private void DoAction_1650_1699(int action)
	{
		switch (action)
		{
		case 1672:
		{
			SqlCreateTypeStatement sqlCreateTypeStatement = SqlCreateTypeStatement.Create(value_stack.array[value_stack.top - 2].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 1].value as CreateTypeStatementInfo);
			sqlCreateTypeStatement.Position = yyloc;
			yyval.value = sqlCreateTypeStatement;
			break;
		}
		case 1673:
			yyval.value = new CreateTypeStatementInfo(value_stack.array[value_stack.top - 2].value as SqlDataTypeSpecification, (SqlUserDefinedDataTypeNullState)value_stack.array[value_stack.top - 1].value);
			break;
		case 1674:
			yyval.value = new CreateTypeStatementInfo(value_stack.array[value_stack.top - 1].value as SqlClrClassSpecifier);
			break;
		case 1675:
			yyval.value = new CreateTypeStatementInfo(value_stack.array[value_stack.top - 2].value as SqlTableDefinition);
			break;
		case 1676:
			yyval = value_stack.array[value_stack.top - 1];
			ExpectKeyword(StringToken.Name, location_stack.array[location_stack.top - 2]);
			break;
		case 1677:
			yyval.value = SqlUserDefinedDataTypeNullState.None;
			break;
		case 1678:
			yyval.value = SqlUserDefinedDataTypeNullState.Null;
			break;
		case 1679:
			yyval.value = SqlUserDefinedDataTypeNullState.NotNull;
			break;
		case 1681:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1682:
		{
			SqlClrClassSpecifier sqlClrClassSpecifier2 = new SqlClrClassSpecifier(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlClrClassSpecifier2.Position = yyloc;
			yyval.value = sqlClrClassSpecifier2;
			break;
		}
		case 1683:
		{
			SqlClrClassSpecifier sqlClrClassSpecifier = new SqlClrClassSpecifier(value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlClrClassSpecifier.Position = yyloc;
			yyval.value = sqlClrClassSpecifier;
			break;
		}
		case 1684:
		{
			SqlCreateFunctionStatement sqlCreateFunctionStatement = new SqlCreateFunctionStatement(value_stack.array[value_stack.top - 1].value as SqlFunctionDefinition, (bool)value_stack.array[value_stack.top - 3].value);
			sqlCreateFunctionStatement.Position = yyloc;
			yyval.value = sqlCreateFunctionStatement;
			break;
		}
		case 1685:
		{
			SqlAlterFunctionStatement sqlAlterFunctionStatement = new SqlAlterFunctionStatement(value_stack.array[value_stack.top - 1].value as SqlFunctionDefinition);
			sqlAlterFunctionStatement.Position = yyloc;
			yyval.value = sqlAlterFunctionStatement;
			break;
		}
		case 1686:
		{
			SqlFunctionDefinition sqlFunctionDefinition = SqlFunctionDefinition.CreateFromFactory(value_stack.array[value_stack.top - 1].value as FunctionDefinitionFactory, value_stack.array[value_stack.top - 6].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 4].value as List<SqlParameterDeclaration>);
			sqlFunctionDefinition.Position = yyloc;
			yyval.value = sqlFunctionDefinition;
			ExpectKeyword(StringToken.Returns, location_stack.array[location_stack.top - 2]);
			break;
		}
		case 1687:
			yyval.value = FunctionDefinitionFactory.CreateScalarFactory(value_stack.array[value_stack.top - 4].value as SqlScalarFunctionReturnType, value_stack.array[value_stack.top - 3].value as List<SqlModuleOption>, value_stack.array[value_stack.top - 1].value as SqlFunctionBodyDefinition);
			break;
		case 1688:
			yyval.value = FunctionDefinitionFactory.CreateTableRelationalFactory(value_stack.array[value_stack.top - 4].value as FunctionVariableNameAndTableReturnTypePair, value_stack.array[value_stack.top - 3].value as List<SqlModuleOption>, value_stack.array[value_stack.top - 1].value as SqlFunctionBodyDefinition);
			break;
		case 1689:
			yyval.value = FunctionDefinitionFactory.CreateTableClrFactory(value_stack.array[value_stack.top - 5].value as SqlTableFunctionReturnType, value_stack.array[value_stack.top - 4].value as List<SqlModuleOption>, value_stack.array[value_stack.top - 3].value as SqlSimpleOrderByClause, value_stack.array[value_stack.top - 1].value as SqlClrFunctionBodyDefinition);
			break;
		case 1691:
		{
			SqlSimpleOrderByClause sqlSimpleOrderByClause = new SqlSimpleOrderByClause(value_stack.array[value_stack.top - 2].value as List<SqlSimpleOrderByItem>);
			sqlSimpleOrderByClause.Position = yyloc;
			yyval.value = sqlSimpleOrderByClause;
			break;
		}
		case 1693:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1694:
		{
			List<SqlModuleOption> list2 = new List<SqlModuleOption>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlModuleOption);
			yyval.value = list2;
			break;
		}
		case 1695:
		{
			List<SqlModuleOption> list = value_stack.array[value_stack.top - 3].value as List<SqlModuleOption>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlModuleOption);
			yyval.value = list;
			break;
		}
		case 1698:
		{
			SqlModuleOption sqlModuleOption2 = SqlModuleOption.ExecuteAs(value_stack.array[value_stack.top - 1].value as SqlExecuteAsClause);
			sqlModuleOption2.Position = yyloc;
			yyval.value = sqlModuleOption2;
			break;
		}
		case 1699:
		{
			SqlModuleOption sqlModuleOption = SqlModuleOption.Create(GetText(location_stack.array[location_stack.top - 1]));
			sqlModuleOption.Position = yyloc;
			yyval.value = sqlModuleOption;
			break;
		}
		case 1650:
		case 1651:
		case 1652:
		case 1653:
		case 1654:
		case 1655:
		case 1656:
		case 1657:
		case 1658:
		case 1659:
		case 1660:
		case 1661:
		case 1662:
		case 1663:
		case 1664:
		case 1665:
		case 1666:
		case 1667:
		case 1668:
		case 1669:
		case 1670:
		case 1671:
		case 1680:
		case 1690:
		case 1692:
		case 1696:
		case 1697:
			break;
		}
	}

	private void DoAction_1700_1749(int action)
	{
		switch (action)
		{
		case 1700:
		{
			SqlModuleOption sqlModuleOption2 = SqlModuleOption.CreateReturnsNullOnNullInputOption();
			sqlModuleOption2.Position = yyloc;
			yyval.value = sqlModuleOption2;
			ExpectKeyword(StringToken.Returns, location_stack.array[location_stack.top - 5]);
			ExpectKeyword(StringToken.Input, location_stack.array[location_stack.top - 1]);
			break;
		}
		case 1701:
		{
			SqlModuleOption sqlModuleOption = SqlModuleOption.CreateCalledOnNullInputOption();
			sqlModuleOption.Position = yyloc;
			yyval.value = sqlModuleOption;
			ExpectKeyword(StringToken.Called, location_stack.array[location_stack.top - 4]);
			ExpectKeyword(StringToken.Input, location_stack.array[location_stack.top - 1]);
			break;
		}
		case 1706:
		{
			SqlMultistatementFunctionBodyDefinition sqlMultistatementFunctionBodyDefinition = new SqlMultistatementFunctionBodyDefinition(value_stack.array[value_stack.top - 1].value as SqlCompoundStatement);
			sqlMultistatementFunctionBodyDefinition.Position = yyloc;
			yyval.value = sqlMultistatementFunctionBodyDefinition;
			break;
		}
		case 1707:
		{
			SqlFunctionBodyDefinition sqlFunctionBodyDefinition = value_stack.array[value_stack.top - 1].value as SqlFunctionBodyDefinition;
			sqlFunctionBodyDefinition.Position = yyloc;
			yyval.value = sqlFunctionBodyDefinition;
			break;
		}
		case 1708:
		{
			SqlInlineFunctionBodyDefinition sqlInlineFunctionBodyDefinition = new SqlInlineFunctionBodyDefinition(null, value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlInlineFunctionBodyDefinition.Position = yyloc;
			yyval.value = sqlInlineFunctionBodyDefinition;
			break;
		}
		case 1710:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 1711:
		{
			SqlClrFunctionBodyDefinition sqlClrFunctionBodyDefinition = new SqlClrFunctionBodyDefinition(value_stack.array[value_stack.top - 1].value as SqlClrMethodSpecifier);
			sqlClrFunctionBodyDefinition.Position = yyloc;
			yyval.value = sqlClrFunctionBodyDefinition;
			ExpectKeyword(StringToken.Name, location_stack.array[location_stack.top - 2]);
			break;
		}
		case 1712:
		{
			SqlClrMethodSpecifier sqlClrMethodSpecifier = new SqlClrMethodSpecifier(value_stack.array[value_stack.top - 5].value as SqlIdentifier, value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlClrMethodSpecifier.Position = yyloc;
			yyval.value = sqlClrMethodSpecifier;
			break;
		}
		case 1713:
		{
			SqlScalarFunctionReturnType sqlScalarFunctionReturnType = new SqlScalarFunctionReturnType(value_stack.array[value_stack.top - 1].value as SqlDataTypeSpecification);
			sqlScalarFunctionReturnType.Position = yyloc;
			yyval.value = sqlScalarFunctionReturnType;
			break;
		}
		case 1714:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.String(GetText(location_stack.array[location_stack.top - 3]));
			sqlLiteralExpression.Position = location_stack.array[location_stack.top - 3];
			SqlTableFunctionReturnType sqlTableFunctionReturnType2 = new SqlTableFunctionReturnType(value_stack.array[value_stack.top - 1].value as SqlTableDefinition);
			sqlTableFunctionReturnType2.Position = location_stack.array[location_stack.top - 1];
			yyval.value = new FunctionVariableNameAndTableReturnTypePair(sqlLiteralExpression, sqlTableFunctionReturnType2);
			break;
		}
		case 1716:
		{
			SqlTableFunctionReturnType sqlTableFunctionReturnType = new SqlTableFunctionReturnType(value_stack.array[value_stack.top - 1].value as SqlTableDefinition);
			sqlTableFunctionReturnType.Position = yyloc;
			yyval.value = sqlTableFunctionReturnType;
			break;
		}
		case 1720:
		{
			SqlTriggerDefinition sqlTriggerDefinition2 = SqlTriggerDefinition.Create(value_stack.array[value_stack.top - 1].value as SqlTriggerDefinition, (bool)value_stack.array[value_stack.top - 3].value);
			sqlTriggerDefinition2.Position = yyloc;
			yyval.value = sqlTriggerDefinition2;
			break;
		}
		case 1721:
			yyval.value = value_stack.array[value_stack.top - 1].value;
			break;
		case 1722:
		{
			SqlCreateTriggerStatement sqlCreateTriggerStatement = new SqlCreateTriggerStatement(value_stack.array[value_stack.top - 3].value as SqlTriggerDefinition, value_stack.array[value_stack.top - 1].value as List<SqlStatement>);
			sqlCreateTriggerStatement.Position = yyloc;
			yyval.value = sqlCreateTriggerStatement;
			break;
		}
		case 1724:
		{
			SqlAlterTriggerStatement sqlAlterTriggerStatement = new SqlAlterTriggerStatement(value_stack.array[value_stack.top - 3].value as SqlTriggerDefinition, value_stack.array[value_stack.top - 1].value as List<SqlStatement>);
			sqlAlterTriggerStatement.Position = yyloc;
			yyval.value = sqlAlterTriggerStatement;
			break;
		}
		case 1726:
			yyval.value = TriggerTargetData.CreateDmlTarget(value_stack.array[value_stack.top - 1].value as SqlObjectIdentifier);
			break;
		case 1727:
			yyval.value = TriggerTargetData.CreateDdlTarget(SqlDdlTriggerTargetType.Database);
			break;
		case 1728:
			yyval.value = TriggerTargetData.CreateDdlTarget(SqlDdlTriggerTargetType.AllServer);
			ExpectKeyword(StringToken.Server, location_stack.array[location_stack.top - 1]);
			break;
		case 1729:
			yyval.value = TriggerActivationData.CreateDmlActivation((SqlTriggerActivationType)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as List<SqlTriggerAction>);
			break;
		case 1730:
			yyval.value = TriggerActivationData.CreateDdlActivation((SqlTriggerActivationType)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as List<SqlTriggerEvent>);
			break;
		case 1731:
			yyval.value = SqlTriggerActivationType.For;
			break;
		case 1732:
			ExpectKeyword(StringToken.After, location_stack.array[location_stack.top - 1]);
			yyval.value = SqlTriggerActivationType.After;
			break;
		case 1733:
		{
			List<SqlTriggerEvent> list4 = new List<SqlTriggerEvent>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlTriggerEvent);
			yyval.value = list4;
			break;
		}
		case 1734:
		{
			List<SqlTriggerEvent> list3 = value_stack.array[value_stack.top - 3].value as List<SqlTriggerEvent>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlTriggerEvent);
			yyval.value = list3;
			break;
		}
		case 1735:
		{
			SqlTriggerEvent sqlTriggerEvent = new SqlTriggerEvent(GetText(location_stack.array[location_stack.top - 1]));
			sqlTriggerEvent.Position = yyloc;
			yyval.value = sqlTriggerEvent;
			break;
		}
		case 1736:
		{
			SqlTriggerDefinition sqlTriggerDefinition = SqlTriggerDefinition.Create(value_stack.array[value_stack.top - 7].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 5].value as TriggerTargetData, value_stack.array[value_stack.top - 4].value as List<SqlModuleOption>, value_stack.array[value_stack.top - 3].value as TriggerActivationData, (bool)value_stack.array[value_stack.top - 2].value, (bool)value_stack.array[value_stack.top - 1].value);
			sqlTriggerDefinition.Position = yyloc;
			yyval.value = sqlTriggerDefinition;
			break;
		}
		case 1737:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 1739:
			yyval.value = false;
			break;
		case 1740:
			yyval.value = true;
			break;
		case 1741:
			yyval.value = SqlTriggerActivationType.For;
			break;
		case 1742:
			ExpectKeyword(StringToken.After, location_stack.array[location_stack.top - 1]);
			yyval.value = SqlTriggerActivationType.After;
			break;
		case 1743:
			ExpectKeyword(StringToken.Instead, location_stack.array[location_stack.top - 2]);
			yyval.value = SqlTriggerActivationType.InsteadOf;
			break;
		case 1744:
		{
			List<SqlTriggerAction> list2 = new List<SqlTriggerAction>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlTriggerAction);
			yyval.value = list2;
			break;
		}
		case 1745:
		{
			List<SqlTriggerAction> list = value_stack.array[value_stack.top - 3].value as List<SqlTriggerAction>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlTriggerAction);
			yyval.value = list;
			break;
		}
		case 1746:
		{
			SqlTriggerAction sqlTriggerAction3 = new SqlTriggerAction(SqlTriggerActionType.Insert);
			sqlTriggerAction3.Position = yyloc;
			yyval.value = sqlTriggerAction3;
			break;
		}
		case 1747:
		{
			SqlTriggerAction sqlTriggerAction2 = new SqlTriggerAction(SqlTriggerActionType.Update);
			sqlTriggerAction2.Position = yyloc;
			yyval.value = sqlTriggerAction2;
			break;
		}
		case 1748:
		{
			SqlTriggerAction sqlTriggerAction = new SqlTriggerAction(SqlTriggerActionType.Delete);
			sqlTriggerAction.Position = yyloc;
			yyval.value = sqlTriggerAction;
			break;
		}
		case 1749:
		{
			SqlCreateViewStatement sqlCreateViewStatement = new SqlCreateViewStatement(value_stack.array[value_stack.top - 1].value as SqlViewDefinition, (bool)value_stack.array[value_stack.top - 3].value);
			sqlCreateViewStatement.Position = yyloc;
			yyval.value = sqlCreateViewStatement;
			break;
		}
		case 1702:
		case 1703:
		case 1704:
		case 1705:
		case 1709:
		case 1715:
		case 1717:
		case 1718:
		case 1719:
		case 1723:
		case 1725:
		case 1738:
			break;
		}
	}

	private void DoAction_1750_1799(int action)
	{
		switch (action)
		{
		case 1750:
		{
			SqlAlterViewStatement sqlAlterViewStatement = new SqlAlterViewStatement(value_stack.array[value_stack.top - 1].value as SqlViewDefinition);
			sqlAlterViewStatement.Position = yyloc;
			yyval.value = sqlAlterViewStatement;
			break;
		}
		case 1751:
		{
			SqlViewDefinition sqlViewDefinition = new SqlViewDefinition(value_stack.array[value_stack.top - 7].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 6].value as List<SqlIdentifier>, value_stack.array[value_stack.top - 5].value as List<SqlModuleOption>, value_stack.array[value_stack.top - 3].value as SqlQueryWithClause, value_stack.array[value_stack.top - 2].value as SqlQueryExpression, (bool)value_stack.array[value_stack.top - 1].value);
			sqlViewDefinition.Position = yyloc;
			yyval.value = sqlViewDefinition;
			break;
		}
		case 1753:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 1754:
		{
			List<SqlIdentifier> list8 = new List<SqlIdentifier>();
			list8.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list8;
			break;
		}
		case 1755:
		{
			List<SqlIdentifier> list7 = value_stack.array[value_stack.top - 3].value as List<SqlIdentifier>;
			list7.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list7;
			break;
		}
		case 1757:
			yyval.value = false;
			break;
		case 1758:
			yyval.value = true;
			break;
		case 1761:
		{
			SqlCreateSchemaStatement sqlCreateSchemaStatement = new SqlCreateSchemaStatement(value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlStatement>);
			sqlCreateSchemaStatement.Position = yyloc;
			yyval.value = sqlCreateSchemaStatement;
			break;
		}
		case 1766:
		{
			List<SqlStatement> list6 = new List<SqlStatement>();
			list6.Add(value_stack.array[value_stack.top - 1].value as SqlStatement);
			yyval.value = list6;
			break;
		}
		case 1767:
		{
			List<SqlStatement> list5 = value_stack.array[value_stack.top - 2].value as List<SqlStatement>;
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlStatement);
			yyval.value = list5;
			break;
		}
		case 1776:
		{
			SqlDropSchemaStatement sqlDropSchemaStatement2 = new SqlDropSchemaStatement(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlDropSchemaStatement2.IsDropIfExists = (bool)value_stack.array[value_stack.top - 2].value;
			sqlDropSchemaStatement2.Position = yyloc;
			yyval.value = sqlDropSchemaStatement2;
			break;
		}
		case 1777:
		{
			SqlIdentifier sqlIdentifier3 = SqlIdentifier.Missing();
			sqlIdentifier3.Position = location_stack.array[location_stack.top - 2];
			SqlDropSchemaStatement sqlDropSchemaStatement = new SqlDropSchemaStatement(sqlIdentifier3);
			sqlDropSchemaStatement.Position = yyloc;
			yyval.value = sqlDropSchemaStatement;
			break;
		}
		case 1778:
			scanner.SetQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 1779:
			yyval.value = false;
			break;
		case 1780:
			yyval.value = true;
			break;
		case 1781:
		{
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			SqlDropDatabaseStatement sqlDropDatabaseStatement = new SqlDropDatabaseStatement(value_stack.array[value_stack.top - 1].value as List<SqlIdentifier>);
			sqlDropDatabaseStatement.IsDropIfExists = (bool)value_stack.array[value_stack.top - 2].value;
			sqlDropDatabaseStatement.Position = yyloc;
			yyval.value = sqlDropDatabaseStatement;
			break;
		}
		case 1782:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 1785:
		{
			List<SqlIdentifier> list4 = new List<SqlIdentifier>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list4;
			break;
		}
		case 1786:
		{
			List<SqlIdentifier> list3 = value_stack.array[value_stack.top - 3].value as List<SqlIdentifier>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list3;
			break;
		}
		case 1787:
		{
			SqlIdentifier sqlIdentifier2 = SqlIdentifier.Missing();
			sqlIdentifier2.Position = yyloc;
			List<SqlIdentifier> list2 = new List<SqlIdentifier>();
			list2.Add(sqlIdentifier2);
			yyval.value = list2;
			break;
		}
		case 1788:
		{
			SqlIdentifier sqlIdentifier = SqlIdentifier.Missing();
			sqlIdentifier.Position = location_stack.array[location_stack.top - 1];
			List<SqlIdentifier> list = value_stack.array[value_stack.top - 3].value as List<SqlIdentifier>;
			list.Add(sqlIdentifier);
			yyval.value = list;
			break;
		}
		case 1789:
		{
			SqlDropTypeStatement sqlDropTypeStatement = new SqlDropTypeStatement(value_stack.array[value_stack.top - 1].value as SqlObjectIdentifier);
			sqlDropTypeStatement.IsDropIfExists = (bool)value_stack.array[value_stack.top - 2].value;
			sqlDropTypeStatement.Position = yyloc;
			yyval.value = sqlDropTypeStatement;
			break;
		}
		case 1752:
		case 1756:
		case 1759:
		case 1760:
		case 1762:
		case 1763:
		case 1764:
		case 1765:
		case 1768:
		case 1769:
		case 1770:
		case 1771:
		case 1772:
		case 1773:
		case 1774:
		case 1775:
		case 1783:
		case 1784:
		case 1790:
		case 1791:
		case 1792:
		case 1793:
		case 1794:
		case 1795:
		case 1796:
		case 1797:
		case 1798:
		case 1799:
			break;
		}
	}

	private void DoAction_1800_1849(int action)
	{
		switch (action)
		{
		case 1817:
		{
			SqlDropStatement sqlDropStatement = SqlDropStatement.Create((SqlDropObjectType)value_stack.array[value_stack.top - 4].value, value_stack.array[value_stack.top - 2].value as List<SqlObjectIdentifier>, (SqlDropTriggerTargetScope)value_stack.array[value_stack.top - 1].value);
			sqlDropStatement.IsDropIfExists = (bool)value_stack.array[value_stack.top - 3].value;
			sqlDropStatement.Position = yyloc;
			yyval.value = sqlDropStatement;
			break;
		}
		case 1818:
			yyval.value = SqlDropTriggerTargetScope.None;
			break;
		case 1819:
			yyval.value = SqlDropTriggerTargetScope.Database;
			break;
		case 1820:
		{
			bool flag = ExpectKeyword(StringToken.Server, location_stack.array[location_stack.top - 1]);
			yyval.value = (flag ? SqlDropTriggerTargetScope.AllServer : SqlDropTriggerTargetScope.None);
			break;
		}
		case 1821:
			yyval.value = Enum.Parse(typeof(SqlDropObjectType), GetText(location_stack.array[location_stack.top - 1]), ignoreCase: true);
			break;
		case 1822:
			yyval.value = Enum.Parse(typeof(SqlDropObjectType), GetText(location_stack.array[location_stack.top - 1]), ignoreCase: true);
			break;
		case 1823:
			yyval.value = Enum.Parse(typeof(SqlDropObjectType), GetText(location_stack.array[location_stack.top - 1]), ignoreCase: true);
			break;
		case 1824:
			yyval.value = Enum.Parse(typeof(SqlDropObjectType), GetText(location_stack.array[location_stack.top - 1]), ignoreCase: true);
			break;
		case 1825:
			yyval.value = Enum.Parse(typeof(SqlDropObjectType), GetText(location_stack.array[location_stack.top - 1]), ignoreCase: true);
			break;
		case 1826:
			yyval.value = Enum.Parse(typeof(SqlDropObjectType), GetText(location_stack.array[location_stack.top - 1]), ignoreCase: true);
			break;
		case 1827:
			yyval.value = Enum.Parse(typeof(SqlDropObjectType), GetText(location_stack.array[location_stack.top - 1]), ignoreCase: true);
			break;
		case 1828:
			yyval.value = Enum.Parse(typeof(SqlDropObjectType), GetText(location_stack.array[location_stack.top - 1]), ignoreCase: true);
			break;
		case 1829:
			yyval.value = Enum.Parse(typeof(SqlDropObjectType), GetText(location_stack.array[location_stack.top - 1]), ignoreCase: true);
			break;
		case 1830:
			yyval.value = SqlDropObjectType.Sequence;
			break;
		case 1831:
			yyval.value = SqlDropObjectType.SecurityPolicy;
			break;
		case 1838:
		{
			List<SqlObjectIdentifier> list2 = new List<SqlObjectIdentifier>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlObjectIdentifier);
			yyval.value = list2;
			break;
		}
		case 1839:
		{
			List<SqlObjectIdentifier> list = value_stack.array[value_stack.top - 3].value as List<SqlObjectIdentifier>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlObjectIdentifier);
			yyval.value = list;
			break;
		}
		case 1844:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1845:
			scanner.SetParseState(EParseState.e_ParseWith);
			break;
		case 1846:
		{
			yyval = value_stack.array[value_stack.top - 1];
			SqlQueryWithClause sqlQueryWithClause4 = yyval.value as SqlQueryWithClause;
			sqlQueryWithClause4.Position = yyloc;
			yyval.value = sqlQueryWithClause4;
			break;
		}
		case 1847:
		{
			SqlQueryWithClause sqlQueryWithClause3 = new SqlQueryWithClause(value_stack.array[value_stack.top - 1].value as List<SqlCommonTableExpression>);
			sqlQueryWithClause3.Position = yyloc;
			yyval.value = sqlQueryWithClause3;
			break;
		}
		case 1848:
		{
			SqlQueryWithClause sqlQueryWithClause2 = new SqlQueryWithClause(new SqlXmlNamespacesDeclaration
			{
				Position = MergeLocationRange(location_stack.top - 5, location_stack.top - 2)
			}, value_stack.array[value_stack.top - 1].value as List<SqlCommonTableExpression>);
			sqlQueryWithClause2.Position = yyloc;
			yyval.value = sqlQueryWithClause2;
			break;
		}
		case 1849:
		{
			SqlQueryWithClause sqlQueryWithClause = new SqlQueryWithClause(value_stack.array[value_stack.top - 2].value as SqlChangeTrackingContext, null, value_stack.array[value_stack.top - 1].value as List<SqlCommonTableExpression>);
			sqlQueryWithClause.Position = yyloc;
			yyval.value = sqlQueryWithClause;
			break;
		}
		case 1800:
		case 1801:
		case 1802:
		case 1803:
		case 1804:
		case 1805:
		case 1806:
		case 1807:
		case 1808:
		case 1809:
		case 1810:
		case 1811:
		case 1812:
		case 1813:
		case 1814:
		case 1815:
		case 1816:
		case 1832:
		case 1833:
		case 1834:
		case 1835:
		case 1836:
		case 1837:
		case 1840:
		case 1841:
		case 1842:
		case 1843:
			break;
		}
	}

	private void DoAction_1850_1899(int action)
	{
		switch (action)
		{
		case 1850:
		{
			SqlXmlNamespacesDeclaration sqlXmlNamespacesDeclaration = new SqlXmlNamespacesDeclaration();
			sqlXmlNamespacesDeclaration.Position = MergeLocationRange(location_stack.top - 5, location_stack.top - 2);
			SqlQueryWithClause sqlQueryWithClause = new SqlQueryWithClause(value_stack.array[value_stack.top - 7].value as SqlChangeTrackingContext, sqlXmlNamespacesDeclaration, value_stack.array[value_stack.top - 1].value as List<SqlCommonTableExpression>);
			sqlQueryWithClause.Position = yyloc;
			yyval.value = sqlQueryWithClause;
			break;
		}
		case 1855:
		{
			List<SqlCommonTableExpression> list10 = new List<SqlCommonTableExpression>();
			list10.Add(value_stack.array[value_stack.top - 1].value as SqlCommonTableExpression);
			yyval.value = list10;
			break;
		}
		case 1856:
		{
			List<SqlCommonTableExpression> list9 = value_stack.array[value_stack.top - 3].value as List<SqlCommonTableExpression>;
			list9.Add(value_stack.array[value_stack.top - 1].value as SqlCommonTableExpression);
			yyval.value = list9;
			break;
		}
		case 1857:
		{
			SqlIdentifier sqlIdentifier4 = new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlIdentifier4.Position = yyloc;
			yyval.value = sqlIdentifier4;
			break;
		}
		case 1858:
		{
			SqlIdentifier sqlIdentifier3 = new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlIdentifier3.Position = yyloc;
			yyval.value = sqlIdentifier3;
			break;
		}
		case 1859:
		{
			SqlCommonTableExpression sqlCommonTableExpression = new SqlCommonTableExpression(value_stack.array[value_stack.top - 6].value as SqlIdentifier, value_stack.array[value_stack.top - 5].value as List<SqlIdentifier>, value_stack.array[value_stack.top - 2].value as SqlQueryExpression);
			sqlCommonTableExpression.Position = yyloc;
			yyval.value = sqlCommonTableExpression;
			scanner.SetParseState(EParseState.e_ParseAfterWith);
			break;
		}
		case 1860:
		{
			SqlChangeTrackingContext sqlChangeTrackingContext = new SqlChangeTrackingContext();
			sqlChangeTrackingContext.Position = yyloc;
			yyval.value = sqlChangeTrackingContext;
			break;
		}
		case 1861:
		{
			SqlInsertStatement sqlInsertStatement2 = new SqlInsertStatement(value_stack.array[value_stack.top - 1].value as SqlInsertSpecification);
			sqlInsertStatement2.Position = yyloc;
			yyval.value = sqlInsertStatement2;
			break;
		}
		case 1862:
		{
			SqlInsertStatement sqlInsertStatement = new SqlInsertStatement(value_stack.array[value_stack.top - 2].value as SqlQueryWithClause, value_stack.array[value_stack.top - 1].value as SqlInsertSpecification);
			sqlInsertStatement.Position = yyloc;
			yyval.value = sqlInsertStatement;
			break;
		}
		case 1863:
		{
			SqlInsertSpecification sqlInsertSpecification = new SqlInsertSpecification(value_stack.array[value_stack.top - 5].value as SqlTopSpecification, value_stack.array[value_stack.top - 3].value as DmlTarget, value_stack.array[value_stack.top - 2].value as List<SqlOutputClause>, value_stack.array[value_stack.top - 1].value as SqlInsertSource);
			sqlInsertSpecification.Position = yyloc;
			yyval.value = sqlInsertSpecification;
			break;
		}
		case 1868:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 1869:
		{
			List<SqlColumnRefExpression> list8 = new List<SqlColumnRefExpression>();
			list8.Add(value_stack.array[value_stack.top - 1].value as SqlColumnRefExpression);
			yyval.value = list8;
			break;
		}
		case 1870:
		{
			List<SqlColumnRefExpression> list7 = value_stack.array[value_stack.top - 3].value as List<SqlColumnRefExpression>;
			list7.Add(value_stack.array[value_stack.top - 1].value as SqlColumnRefExpression);
			yyval.value = list7;
			break;
		}
		case 1871:
		{
			SqlObjectIdentifier sqlObjectIdentifier5 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier5.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression4 = new SqlColumnRefExpression(sqlObjectIdentifier5);
			sqlColumnRefExpression4.Position = yyloc;
			yyval.value = sqlColumnRefExpression4;
			break;
		}
		case 1872:
		{
			List<SqlIdentifier> obj2 = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			obj2.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			SqlObjectIdentifier sqlObjectIdentifier4 = SqlObjectIdentifier.Create(obj2);
			sqlObjectIdentifier4.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression3 = new SqlColumnRefExpression(sqlObjectIdentifier4, discardColumnPrefix: true);
			sqlColumnRefExpression3.Position = yyloc;
			yyval.value = sqlColumnRefExpression3;
			break;
		}
		case 1874:
		{
			SqlIdentifier sqlIdentifier2 = SqlIdentifier.Missing();
			sqlIdentifier2.Position = yyloc;
			SqlObjectIdentifier sqlObjectIdentifier3 = SqlObjectIdentifier.Create(sqlIdentifier2);
			sqlObjectIdentifier3.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression2 = new SqlColumnRefExpression(sqlObjectIdentifier3);
			sqlColumnRefExpression2.Position = yyloc;
			yyval.value = sqlColumnRefExpression2;
			break;
		}
		case 1875:
		{
			SqlIdentifier sqlIdentifier = SqlIdentifier.Missing();
			sqlIdentifier.Position = location_stack.array[location_stack.top - 1];
			List<SqlIdentifier> obj = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			obj.Add(sqlIdentifier);
			SqlObjectIdentifier sqlObjectIdentifier2 = SqlObjectIdentifier.Create(obj);
			sqlObjectIdentifier2.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression = new SqlColumnRefExpression(sqlObjectIdentifier2, discardColumnPrefix: true);
			sqlColumnRefExpression.Position = yyloc;
			yyval.value = sqlColumnRefExpression;
			break;
		}
		case 1876:
		{
			SqlDefaultValuesInsertSource sqlDefaultValuesInsertSource = new SqlDefaultValuesInsertSource();
			sqlDefaultValuesInsertSource.Position = yyloc;
			yyval.value = sqlDefaultValuesInsertSource;
			break;
		}
		case 1877:
		{
			SqlTableConstructorInsertSource sqlTableConstructorInsertSource = new SqlTableConstructorInsertSource(value_stack.array[value_stack.top - 2].value as SqlTableConstructorExpression);
			sqlTableConstructorInsertSource.Position = yyloc;
			yyval.value = sqlTableConstructorInsertSource;
			break;
		}
		case 1878:
		{
			SqlSelectSpecificationInsertSource sqlSelectSpecificationInsertSource = new SqlSelectSpecificationInsertSource(value_stack.array[value_stack.top - 1].value as SqlSelectSpecification);
			sqlSelectSpecificationInsertSource.Position = yyloc;
			yyval.value = sqlSelectSpecificationInsertSource;
			break;
		}
		case 1879:
		{
			SqlNullInsertSource sqlNullInsertSource = new SqlNullInsertSource();
			sqlNullInsertSource.Position = yyloc;
			yyval.value = sqlNullInsertSource;
			break;
		}
		case 1880:
		{
			SqlMissingInsertSource sqlMissingInsertSource = new SqlMissingInsertSource();
			sqlMissingInsertSource.Position = yyloc;
			yyval.value = sqlMissingInsertSource;
			break;
		}
		case 1881:
		{
			List<SqlScalarExpression> list6 = new List<SqlScalarExpression>();
			list6.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list6;
			break;
		}
		case 1882:
		{
			List<SqlScalarExpression> list5 = value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>;
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list5;
			break;
		}
		case 1887:
		{
			List<SqlOutputClause> list4 = new List<SqlOutputClause>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlOutputClause);
			yyval.value = list4;
			break;
		}
		case 1888:
		{
			List<SqlOutputClause> list3 = new List<SqlOutputClause>();
			list3.Add(value_stack.array[value_stack.top - 2].value as SqlOutputClause);
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlOutputClause);
			yyval.value = list3;
			break;
		}
		case 1889:
		{
			SqlOutputIntoClause sqlOutputIntoClause = new SqlOutputIntoClause(value_stack.array[value_stack.top - 2].value as SqlOutputClause, value_stack.array[value_stack.top - 1].value as SqlIntoClause);
			sqlOutputIntoClause.Position = yyloc;
			yyval.value = sqlOutputIntoClause;
			break;
		}
		case 1892:
		{
			SqlOutputClause sqlOutputClause = new SqlOutputClause(value_stack.array[value_stack.top - 1].value as List<SqlSelectExpression>);
			sqlOutputClause.Position = yyloc;
			yyval.value = sqlOutputClause;
			break;
		}
		case 1893:
		{
			List<SqlSelectExpression> list2 = new List<SqlSelectExpression>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlSelectExpression);
			yyval.value = list2;
			break;
		}
		case 1894:
		{
			List<SqlSelectExpression> list = value_stack.array[value_stack.top - 3].value as List<SqlSelectExpression>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlSelectExpression);
			yyval.value = list;
			break;
		}
		case 1895:
		{
			SqlSelectScalarExpression sqlSelectScalarExpression = new SqlSelectScalarExpression(value_stack.array[value_stack.top - 2].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlSelectScalarExpression.Position = yyloc;
			yyval.value = sqlSelectScalarExpression;
			break;
		}
		case 1896:
		{
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>);
			sqlObjectIdentifier.Position = location_stack.array[location_stack.top - 2];
			SqlSelectStarExpression sqlSelectStarExpression = new SqlSelectStarExpression(sqlObjectIdentifier);
			sqlSelectStarExpression.Position = yyloc;
			yyval.value = sqlSelectStarExpression;
			break;
		}
		case 1851:
		case 1852:
		case 1853:
		case 1854:
		case 1864:
		case 1865:
		case 1866:
		case 1867:
		case 1873:
		case 1883:
		case 1884:
		case 1885:
		case 1886:
		case 1890:
		case 1891:
		case 1897:
		case 1898:
		case 1899:
			break;
		}
	}

	private void DoAction_1900_1949(int action)
	{
		switch (action)
		{
		case 1900:
		{
			SqlIntoClause sqlIntoClause2 = new SqlIntoClause(new SqlTableVariableRefExpression(GetText(location_stack.array[location_stack.top - 2]))
			{
				Position = location_stack.array[location_stack.top - 2]
			}, value_stack.array[value_stack.top - 1].value as List<SqlColumnRefExpression>);
			sqlIntoClause2.Position = yyloc;
			yyval.value = sqlIntoClause2;
			break;
		}
		case 1901:
		{
			SqlIntoClause sqlIntoClause = new SqlIntoClause(new SqlTableRefExpression(value_stack.array[value_stack.top - 2].value as SqlObjectIdentifier)
			{
				Position = location_stack.array[location_stack.top - 2]
			}, value_stack.array[value_stack.top - 1].value as List<SqlColumnRefExpression>);
			sqlIntoClause.Position = yyloc;
			yyval.value = sqlIntoClause;
			break;
		}
		case 1926:
		{
			List<SqlSimpleOrderByItem> list4 = new List<SqlSimpleOrderByItem>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlSimpleOrderByItem);
			yyval.value = list4;
			break;
		}
		case 1927:
		{
			List<SqlSimpleOrderByItem> list3 = value_stack.array[value_stack.top - 3].value as List<SqlSimpleOrderByItem>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlSimpleOrderByItem);
			yyval.value = list3;
			break;
		}
		case 1928:
		{
			SqlSimpleOrderByItem sqlSimpleOrderByItem = new SqlSimpleOrderByItem(value_stack.array[value_stack.top - 2].value as SqlIdentifier, (SqlSortOrder)value_stack.array[value_stack.top - 1].value);
			sqlSimpleOrderByItem.Position = yyloc;
			yyval.value = sqlSimpleOrderByItem;
			break;
		}
		case 1929:
			VersionedParserBase.ReportError(SR.MergeStatementMustTerminated, new LexLocation(location_stack.array[location_stack.top - 1].endTokenNumber, location_stack.array[location_stack.top - 1].endTokenNumber));
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1930:
			yyval = value_stack.array[value_stack.top - 3];
			break;
		case 1931:
		{
			SqlMergeStatement sqlMergeStatement2 = new SqlMergeStatement(value_stack.array[value_stack.top - 1].value as SqlMergeSpecification);
			sqlMergeStatement2.Position = yyloc;
			yyval.value = sqlMergeStatement2;
			break;
		}
		case 1932:
		{
			SqlMergeStatement sqlMergeStatement = new SqlMergeStatement(value_stack.array[value_stack.top - 2].value as SqlQueryWithClause, value_stack.array[value_stack.top - 1].value as SqlMergeSpecification);
			sqlMergeStatement.Position = yyloc;
			yyval.value = sqlMergeStatement;
			break;
		}
		case 1933:
		{
			SqlConditionClause sqlConditionClause = new SqlConditionClause(value_stack.array[value_stack.top - 4].value as SqlBooleanExpression);
			sqlConditionClause.Position = LexLocation.Merge(location_stack.array[location_stack.top - 5], location_stack.array[location_stack.top - 4]);
			SqlMergeSpecification sqlMergeSpecification = new SqlMergeSpecification(value_stack.array[value_stack.top - 11].value as SqlTopSpecification, value_stack.array[value_stack.top - 9].value as DmlTarget, value_stack.array[value_stack.top - 8].value as SqlIdentifier, value_stack.array[value_stack.top - 6].value as SqlTableExpression, sqlConditionClause, value_stack.array[value_stack.top - 3].value as List<SqlMergeActionClause>, value_stack.array[value_stack.top - 2].value as List<SqlOutputClause>);
			sqlMergeSpecification.Position = yyloc;
			yyval.value = sqlMergeSpecification;
			scanner.ClearQueryStatus(QueryStatus.Q_MERGE);
			break;
		}
		case 1938:
		{
			List<SqlMergeActionClause> list2 = new List<SqlMergeActionClause>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlMergeActionClause);
			yyval.value = list2;
			break;
		}
		case 1939:
		{
			List<SqlMergeActionClause> list = value_stack.array[value_stack.top - 2].value as List<SqlMergeActionClause>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlMergeActionClause);
			yyval.value = list;
			break;
		}
		case 1940:
		{
			SqlMergeActionClause sqlMergeActionClause2 = new SqlMergeActionClause((SqlMergeConditionType)value_stack.array[value_stack.top - 3].value, value_stack.array[value_stack.top - 1].value as SqlMergeAction);
			sqlMergeActionClause2.Position = yyloc;
			yyval.value = sqlMergeActionClause2;
			break;
		}
		case 1941:
		{
			SqlMergeActionClause sqlMergeActionClause = new SqlMergeActionClause((SqlMergeConditionType)value_stack.array[value_stack.top - 5].value, value_stack.array[value_stack.top - 3].value as SqlBooleanExpression, value_stack.array[value_stack.top - 1].value as SqlMergeAction);
			sqlMergeActionClause.Position = yyloc;
			yyval.value = sqlMergeActionClause;
			break;
		}
		case 1942:
			ExpectKeyword(StringToken.Matched, location_stack.array[location_stack.top - 1]);
			yyval.value = SqlMergeConditionType.Matched;
			break;
		case 1943:
			ExpectKeyword(StringToken.Matched, location_stack.array[location_stack.top - 1]);
			yyval.value = SqlMergeConditionType.TargetNotMatched;
			break;
		case 1944:
			ExpectKeyword(StringToken.Matched, location_stack.array[location_stack.top - 3]);
			switch (GetStringToken(location_stack.array[location_stack.top - 1]))
			{
			case StringToken.Source:
				yyval.value = SqlMergeConditionType.SourceNotMatched;
				break;
			case StringToken.Target:
				yyval.value = SqlMergeConditionType.TargetNotMatched;
				break;
			default:
				yyval.value = SqlMergeConditionType.Error;
				VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 1]);
				break;
			}
			break;
		case 1945:
		{
			SqlUpdateMergeAction sqlUpdateMergeAction = new SqlUpdateMergeAction(new SqlSetClause(value_stack.array[value_stack.top - 1].value as List<SqlAssignment>)
			{
				Position = LexLocation.Merge(location_stack.array[location_stack.top - 2], location_stack.array[location_stack.top - 1])
			});
			sqlUpdateMergeAction.Position = yyloc;
			yyval.value = sqlUpdateMergeAction;
			break;
		}
		case 1946:
		{
			SqlInsertMergeAction sqlInsertMergeAction = new SqlInsertMergeAction(value_stack.array[value_stack.top - 2].value as List<SqlColumnRefExpression>, value_stack.array[value_stack.top - 1].value as SqlInsertMergeActionSource);
			sqlInsertMergeAction.Position = yyloc;
			yyval.value = sqlInsertMergeAction;
			break;
		}
		case 1947:
		{
			SqlDeleteMergeAction sqlDeleteMergeAction = new SqlDeleteMergeAction();
			sqlDeleteMergeAction.Position = yyloc;
			yyval.value = sqlDeleteMergeAction;
			break;
		}
		case 1948:
		{
			SqlDefaultValuesInsertMergeActionSource sqlDefaultValuesInsertMergeActionSource = new SqlDefaultValuesInsertMergeActionSource();
			sqlDefaultValuesInsertMergeActionSource.Position = yyloc;
			yyval.value = sqlDefaultValuesInsertMergeActionSource;
			break;
		}
		case 1949:
		{
			SqlValuesInsertMergeActionSource sqlValuesInsertMergeActionSource = new SqlValuesInsertMergeActionSource(value_stack.array[value_stack.top - 2].value as List<SqlScalarExpression>);
			sqlValuesInsertMergeActionSource.Position = yyloc;
			yyval.value = sqlValuesInsertMergeActionSource;
			break;
		}
		case 1902:
		case 1903:
		case 1904:
		case 1905:
		case 1906:
		case 1907:
		case 1908:
		case 1909:
		case 1910:
		case 1911:
		case 1912:
		case 1913:
		case 1914:
		case 1915:
		case 1916:
		case 1917:
		case 1918:
		case 1919:
		case 1920:
		case 1921:
		case 1922:
		case 1923:
		case 1924:
		case 1925:
		case 1934:
		case 1935:
		case 1936:
		case 1937:
			break;
		}
	}

	private void DoAction_1950_1999(int action)
	{
		switch (action)
		{
		case 1950:
		{
			SqlUpdateStatement sqlUpdateStatement2 = new SqlUpdateStatement(value_stack.array[value_stack.top - 1].value as SqlUpdateSpecification);
			sqlUpdateStatement2.Position = yyloc;
			yyval.value = sqlUpdateStatement2;
			break;
		}
		case 1951:
		{
			SqlUpdateStatement sqlUpdateStatement = new SqlUpdateStatement(value_stack.array[value_stack.top - 2].value as SqlQueryWithClause, value_stack.array[value_stack.top - 1].value as SqlUpdateSpecification);
			sqlUpdateStatement.Position = yyloc;
			yyval.value = sqlUpdateStatement;
			break;
		}
		case 1952:
		{
			SqlUpdateSpecification sqlUpdateSpecification = new SqlUpdateSpecification(value_stack.array[value_stack.top - 7].value as SqlTopSpecification, value_stack.array[value_stack.top - 6].value as DmlTarget, value_stack.array[value_stack.top - 5].value as SqlSetClause, value_stack.array[value_stack.top - 4].value as List<SqlOutputClause>, value_stack.array[value_stack.top - 3].value as SqlFromClause, value_stack.array[value_stack.top - 2].value as SqlWhereClause);
			sqlUpdateSpecification.Position = yyloc;
			yyval.value = sqlUpdateSpecification;
			break;
		}
		case 1953:
		{
			List<SqlAssignment> list2 = new List<SqlAssignment>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlAssignment);
			yyval.value = list2;
			break;
		}
		case 1954:
		{
			List<SqlAssignment> list = value_stack.array[value_stack.top - 3].value as List<SqlAssignment>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlAssignment);
			yyval.value = list;
			break;
		}
		case 1955:
			yyval = value_stack.array[value_stack.top - 1];
			if (value_stack.array[value_stack.top - 1].value == null)
			{
				SqlNullAssignment sqlNullAssignment = new SqlNullAssignment(ShiftReduceParser.UpdatePos(new SqlNullScalarExpression(), yyloc));
				sqlNullAssignment.Position = yyloc;
				yyval.value = sqlNullAssignment;
			}
			break;
		case 1956:
		{
			SqlScalarVariableAssignment sqlScalarVariableAssignment = new SqlScalarVariableAssignment(value_stack.array[value_stack.top - 3].value as SqlScalarVariableRefExpression, (SqlAssignmentOperatorType)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlScalarVariableAssignment.Position = yyloc;
			yyval.value = sqlScalarVariableAssignment;
			break;
		}
		case 1957:
		{
			SqlVariableColumnAssignment sqlVariableColumnAssignment = new SqlVariableColumnAssignment(value_stack.array[value_stack.top - 3].value as SqlScalarVariableRefExpression, value_stack.array[value_stack.top - 1].value as SqlColumnAssignment);
			sqlVariableColumnAssignment.Position = yyloc;
			yyval.value = sqlVariableColumnAssignment;
			break;
		}
		case 1958:
		{
			SqlColumnAssignment sqlColumnAssignment2 = new SqlColumnAssignment(value_stack.array[value_stack.top - 3].value as SqlColumnRefExpression, (SqlAssignmentOperatorType)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlColumnAssignment2.Position = yyloc;
			yyval.value = sqlColumnAssignment2;
			break;
		}
		case 1959:
		{
			SqlObjectIdentifier sqlObjectIdentifier6 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier6.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression4 = new SqlColumnRefExpression(sqlObjectIdentifier6);
			sqlColumnRefExpression4.Position = yyloc;
			yyval.value = sqlColumnRefExpression4;
			break;
		}
		case 1960:
		{
			List<SqlIdentifier> obj3 = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			obj3.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			SqlObjectIdentifier sqlObjectIdentifier5 = SqlObjectIdentifier.Create(obj3);
			sqlObjectIdentifier5.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression3 = new SqlColumnRefExpression(sqlObjectIdentifier5);
			sqlColumnRefExpression3.Position = yyloc;
			yyval.value = sqlColumnRefExpression3;
			break;
		}
		case 1963:
			yyval.value = SqlAssignmentOperatorType.Assignment;
			break;
		case 1964:
			yyval.value = SqlAssignmentOperatorType.AdditionAssignment;
			break;
		case 1965:
			yyval.value = SqlAssignmentOperatorType.SubtractionAssignment;
			break;
		case 1966:
			yyval.value = SqlAssignmentOperatorType.MultiplicationAssignment;
			break;
		case 1967:
			yyval.value = SqlAssignmentOperatorType.DivisionAssignment;
			break;
		case 1968:
			yyval.value = SqlAssignmentOperatorType.ModulusAssignment;
			break;
		case 1969:
			yyval.value = SqlAssignmentOperatorType.BitwiseAndAssignment;
			break;
		case 1970:
			yyval.value = SqlAssignmentOperatorType.BitwiseOrAssignment;
			break;
		case 1971:
			yyval.value = SqlAssignmentOperatorType.BitwiseXorAssignment;
			break;
		case 1972:
		{
			SqlObjectIdentifier sqlObjectIdentifier4 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier4.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression2 = new SqlColumnRefExpression(sqlObjectIdentifier4);
			sqlColumnRefExpression2.Position = yyloc;
			yyval.value = sqlColumnRefExpression2;
			break;
		}
		case 1973:
		{
			List<SqlIdentifier> obj2 = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			obj2.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			SqlObjectIdentifier sqlObjectIdentifier3 = SqlObjectIdentifier.Create(obj2);
			sqlObjectIdentifier3.Position = yyloc;
			SqlScalarRefExpression sqlScalarRefExpression2 = SqlScalarRefExpression.ColumnOrPropertyRefExpression(sqlObjectIdentifier3);
			sqlScalarRefExpression2.Position = yyloc;
			yyval.value = sqlScalarRefExpression2;
			break;
		}
		case 1975:
		{
			SqlIdentifier sqlIdentifier2 = SqlIdentifier.Missing();
			sqlIdentifier2.Position = yyloc;
			SqlObjectIdentifier sqlObjectIdentifier2 = SqlObjectIdentifier.Create(sqlIdentifier2);
			sqlObjectIdentifier2.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression = new SqlColumnRefExpression(sqlObjectIdentifier2);
			sqlColumnRefExpression.Position = yyloc;
			yyval.value = sqlColumnRefExpression;
			break;
		}
		case 1976:
		{
			SqlIdentifier sqlIdentifier = SqlIdentifier.Missing();
			sqlIdentifier.Position = location_stack.array[location_stack.top - 1];
			List<SqlIdentifier> obj = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			obj.Add(sqlIdentifier);
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(obj);
			sqlObjectIdentifier.Position = yyloc;
			SqlScalarRefExpression sqlScalarRefExpression = SqlScalarRefExpression.ColumnOrPropertyRefExpression(sqlObjectIdentifier);
			sqlScalarRefExpression.Position = yyloc;
			yyval.value = sqlScalarRefExpression;
			break;
		}
		case 1981:
		{
			SqlColumnAssignment sqlColumnAssignment = new SqlColumnAssignment(value_stack.array[value_stack.top - 3].value as SqlScalarRefExpression, (SqlAssignmentOperatorType)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlColumnAssignment.Position = yyloc;
			yyval.value = sqlColumnAssignment;
			break;
		}
		case 1982:
		{
			SqlSelectStatement sqlSelectStatement2 = new SqlSelectStatement(value_stack.array[value_stack.top - 1].value as SqlSelectSpecification);
			sqlSelectStatement2.Position = yyloc;
			yyval.value = sqlSelectStatement2;
			break;
		}
		case 1983:
		{
			SqlSelectStatement sqlSelectStatement = new SqlSelectStatement(value_stack.array[value_stack.top - 2].value as SqlQueryWithClause, value_stack.array[value_stack.top - 1].value as SqlSelectSpecification);
			sqlSelectStatement.Position = yyloc;
			yyval.value = sqlSelectStatement;
			break;
		}
		case 1984:
		{
			SqlSelectSpecification sqlSelectSpecification = new SqlSelectSpecification(value_stack.array[value_stack.top - 3].value as SqlQueryExpression, value_stack.array[value_stack.top - 2].value as SqlOrderByClause, value_stack.array[value_stack.top - 1].value as SqlForClause);
			sqlSelectSpecification.Position = yyloc;
			yyval.value = sqlSelectSpecification;
			break;
		}
		case 1986:
		{
			SqlBinaryQueryExpression sqlBinaryQueryExpression2 = new SqlBinaryQueryExpression(value_stack.array[value_stack.top - 4].value as SqlQueryExpression, (SqlBinaryQueryOperatorType)value_stack.array[value_stack.top - 3].value, (bool)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlBinaryQueryExpression2.Position = yyloc;
			yyval.value = sqlBinaryQueryExpression2;
			break;
		}
		case 1987:
			yyval.value = SqlBinaryQueryOperatorType.Union;
			break;
		case 1988:
			yyval.value = SqlBinaryQueryOperatorType.Except;
			break;
		case 1990:
		{
			SqlBinaryQueryExpression sqlBinaryQueryExpression = new SqlBinaryQueryExpression(value_stack.array[value_stack.top - 4].value as SqlQueryExpression, SqlBinaryQueryOperatorType.Intersect, (bool)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlBinaryQueryExpression.Position = yyloc;
			yyval.value = sqlBinaryQueryExpression;
			break;
		}
		case 1991:
			yyval.value = false;
			break;
		case 1992:
			yyval.value = true;
			break;
		case 1994:
			if (value_stack.array[value_stack.top - 2].value is SqlCodeObject sqlCodeObject)
			{
				sqlCodeObject.Position = yyloc;
				yyval.value = sqlCodeObject;
			}
			break;
		case 1996:
		{
			SqlQuerySpecification sqlQuerySpecification = new SqlQuerySpecification(new SqlSelectClause(value_stack.array[value_stack.top - 8].value != null, value_stack.array[value_stack.top - 7].value as SqlTopSpecification, value_stack.array[value_stack.top - 6].value as List<SqlSelectExpression>)
			{
				Position = MergeLocationRange(location_stack.top - 9, location_stack.top - 6)
			}, value_stack.array[value_stack.top - 5].value as SqlSelectIntoClause, value_stack.array[value_stack.top - 4].value as SqlFromClause, value_stack.array[value_stack.top - 3].value as SqlWhereClause, value_stack.array[value_stack.top - 2].value as SqlGroupByClause, value_stack.array[value_stack.top - 1].value as SqlHavingClause);
			sqlQuerySpecification.Position = yyloc;
			yyval.value = sqlQuerySpecification;
			break;
		}
		case 1998:
		{
			SqlSelectIntoClause sqlSelectIntoClause = new SqlSelectIntoClause(value_stack.array[value_stack.top - 2].value as SqlObjectIdentifier);
			sqlSelectIntoClause.Position = yyloc;
			yyval.value = sqlSelectIntoClause;
			break;
		}
		case 1961:
		case 1962:
		case 1974:
		case 1977:
		case 1978:
		case 1979:
		case 1980:
		case 1985:
		case 1989:
		case 1993:
		case 1995:
		case 1997:
		case 1999:
			break;
		}
	}

	private void DoAction_2000_2049(int action)
	{
		switch (action)
		{
		case 2001:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 2002:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2003:
		{
			SqlForBrowseClause sqlForBrowseClause = new SqlForBrowseClause();
			sqlForBrowseClause.Position = yyloc;
			yyval.value = sqlForBrowseClause;
			break;
		}
		case 2045:
			if (ExpectKeywords(new StringToken[2]
			{
				StringToken.Xml,
				StringToken.Json
			}, location_stack.array[location_stack.top - 2]))
			{
				StringToken stringToken = GetStringToken(location_stack.array[location_stack.top - 2]);
				SqlForXmlClause sqlForXmlClause = SqlForXmlClause.Create(value_stack.array[value_stack.top - 1].value as List<SqlForXmlUnknownDirective>, stringToken);
				sqlForXmlClause.Position = yyloc;
				yyval.value = sqlForXmlClause;
			}
			break;
		case 2046:
		{
			List<SqlForXmlUnknownDirective> list2 = new List<SqlForXmlUnknownDirective>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlForXmlUnknownDirective);
			yyval.value = list2;
			break;
		}
		case 2047:
		{
			List<SqlForXmlUnknownDirective> list = value_stack.array[value_stack.top - 3].value as List<SqlForXmlUnknownDirective>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlForXmlUnknownDirective);
			yyval.value = list;
			break;
		}
		case 2048:
		{
			SqlForXmlUnknownDirective sqlForXmlUnknownDirective2 = new SqlForXmlUnknownDirective(GetText(location_stack.array[location_stack.top - 1]), null);
			sqlForXmlUnknownDirective2.Position = yyloc;
			yyval.value = sqlForXmlUnknownDirective2;
			break;
		}
		case 2049:
		{
			SqlForXmlUnknownDirective sqlForXmlUnknownDirective = new SqlForXmlUnknownDirective(GetText(location_stack.array[location_stack.top - 2]) + " " + GetText(location_stack.array[location_stack.top - 1]), null);
			sqlForXmlUnknownDirective.Position = yyloc;
			yyval.value = sqlForXmlUnknownDirective;
			break;
		}
		case 2000:
		case 2004:
		case 2005:
		case 2006:
		case 2007:
		case 2008:
		case 2009:
		case 2010:
		case 2011:
		case 2012:
		case 2013:
		case 2014:
		case 2015:
		case 2016:
		case 2017:
		case 2018:
		case 2019:
		case 2020:
		case 2021:
		case 2022:
		case 2023:
		case 2024:
		case 2025:
		case 2026:
		case 2027:
		case 2028:
		case 2029:
		case 2030:
		case 2031:
		case 2032:
		case 2033:
		case 2034:
		case 2035:
		case 2036:
		case 2037:
		case 2038:
		case 2039:
		case 2040:
		case 2041:
		case 2042:
		case 2043:
		case 2044:
			break;
		}
	}

	private void DoAction_2050_2099(int action)
	{
		switch (action)
		{
		case 2050:
		{
			SqlForXmlUnknownDirective sqlForXmlUnknownDirective = new SqlForXmlUnknownDirective(GetText(location_stack.array[location_stack.top - 4]), GetText(location_stack.array[location_stack.top - 2]));
			sqlForXmlUnknownDirective.Position = yyloc;
			yyval.value = sqlForXmlUnknownDirective;
			break;
		}
		case 2057:
			yyval.value = true;
			break;
		case 2059:
		{
			SqlOffsetFetchClause sqlOffsetFetchClause = new SqlOffsetFetchClause(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlOffsetFetchClause.Position = yyloc;
			yyval.value = sqlOffsetFetchClause;
			ExpectKeyword(StringToken.Offset, location_stack.array[location_stack.top - 4]);
			break;
		}
		case 2061:
			ExpectKeywords(new StringToken[2]
			{
				StringToken.First,
				StringToken.Next
			}, location_stack.array[location_stack.top - 4]);
			ExpectKeyword(StringToken.Only, location_stack.array[location_stack.top - 1]);
			yyval = value_stack.array[value_stack.top - 3];
			break;
		case 2062:
			ExpectKeywords(new StringToken[2]
			{
				StringToken.Row,
				StringToken.Rows
			}, location_stack.array[location_stack.top - 1]);
			break;
		case 2063:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2064:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2066:
		{
			SqlTopSpecification sqlTopSpecification2 = new SqlTopSpecification(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, (bool)value_stack.array[value_stack.top - 2].value, (bool)value_stack.array[value_stack.top - 1].value);
			sqlTopSpecification2.Position = yyloc;
			yyval.value = sqlTopSpecification2;
			break;
		}
		case 2067:
		{
			SqlLiteralExpression sqlLiteralExpression3 = SqlLiteralExpression.Real(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression3.Position = yyloc;
			yyval.value = sqlLiteralExpression3;
			break;
		}
		case 2068:
		{
			SqlLiteralExpression sqlLiteralExpression2 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression2.Position = yyloc;
			yyval.value = sqlLiteralExpression2;
			break;
		}
		case 2069:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Numeric(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression.Position = yyloc;
			yyval.value = sqlLiteralExpression;
			break;
		}
		case 2072:
		{
			SqlTopSpecification sqlTopSpecification = new SqlTopSpecification(value_stack.array[value_stack.top - 2].value as SqlScalarExpression, (bool)value_stack.array[value_stack.top - 1].value);
			sqlTopSpecification.Position = yyloc;
			yyval.value = sqlTopSpecification;
			break;
		}
		case 2073:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 2074:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2075:
			yyval.value = false;
			break;
		case 2076:
			yyval.value = true;
			break;
		case 2077:
			yyval.value = false;
			break;
		case 2078:
			yyval.value = true;
			break;
		case 2079:
		{
			SqlDeleteStatement sqlDeleteStatement2 = new SqlDeleteStatement(value_stack.array[value_stack.top - 1].value as SqlDeleteSpecification);
			sqlDeleteStatement2.Position = yyloc;
			yyval.value = sqlDeleteStatement2;
			break;
		}
		case 2080:
		{
			SqlDeleteStatement sqlDeleteStatement = new SqlDeleteStatement(value_stack.array[value_stack.top - 2].value as SqlQueryWithClause, value_stack.array[value_stack.top - 1].value as SqlDeleteSpecification);
			sqlDeleteStatement.Position = yyloc;
			yyval.value = sqlDeleteStatement;
			break;
		}
		case 2081:
		{
			SqlDeleteSpecification sqlDeleteSpecification = new SqlDeleteSpecification(value_stack.array[value_stack.top - 7].value as SqlTopSpecification, value_stack.array[value_stack.top - 5].value as DmlTarget, value_stack.array[value_stack.top - 4].value as List<SqlOutputClause>, value_stack.array[value_stack.top - 3].value as SqlFromClause, value_stack.array[value_stack.top - 2].value as SqlWhereClause);
			sqlDeleteSpecification.Position = yyloc;
			yyval.value = sqlDeleteSpecification;
			break;
		}
		case 2084:
			if (value_stack.array[value_stack.top - 1].value is SqlExecuteStatement sqlExecuteStatement)
			{
				sqlExecuteStatement.Position = yyloc;
				yyval.value = sqlExecuteStatement;
			}
			break;
		case 2085:
		{
			SqlExecuteStringStatement sqlExecuteStringStatement2 = value_stack.array[value_stack.top - 5].value as SqlExecuteStringStatement;
			sqlExecuteStringStatement2.Position = yyloc;
			yyval.value = sqlExecuteStringStatement2;
			break;
		}
		case 2087:
		{
			SqlExecuteModuleStatement sqlExecuteModuleStatement2 = value_stack.array[value_stack.top - 1].value as SqlExecuteModuleStatement;
			sqlExecuteModuleStatement2.ReturnValue = ShiftReduceParser.UpdatePos(SqlScalarVariableRefExpression.Create(value_stack.array[value_stack.top - 3].value as string), location_stack.array[location_stack.top - 3]);
			sqlExecuteModuleStatement2.Position = yyloc;
			yyval.value = sqlExecuteModuleStatement2;
			break;
		}
		case 2093:
		{
			SqlExecuteStringStatement sqlExecuteStringStatement = new SqlExecuteStringStatement(value_stack.array[value_stack.top - 2].value as List<SqlScalarExpression>, value_stack.array[value_stack.top - 1].value as List<SqlExecuteArgument>);
			sqlExecuteStringStatement.Position = yyloc;
			yyval.value = sqlExecuteStringStatement;
			break;
		}
		case 2097:
		{
			SqlExecuteModuleStatement sqlExecuteModuleStatement = new SqlExecuteModuleStatement(value_stack.array[value_stack.top - 4].value as SqlObjectReference, value_stack.array[value_stack.top - 3].value as SqlLiteralExpression, value_stack.array[value_stack.top - 2].value as List<SqlExecuteArgument>, value_stack.array[value_stack.top - 1].value as SqlModuleOption);
			sqlExecuteModuleStatement.Position = yyloc;
			yyval.value = sqlExecuteModuleStatement;
			break;
		}
		case 2051:
		case 2052:
		case 2053:
		case 2054:
		case 2055:
		case 2056:
		case 2058:
		case 2060:
		case 2065:
		case 2070:
		case 2071:
		case 2082:
		case 2083:
		case 2086:
		case 2088:
		case 2089:
		case 2090:
		case 2091:
		case 2092:
		case 2094:
		case 2095:
		case 2096:
		case 2098:
		case 2099:
			break;
		}
	}

	private void DoAction_2100_2149(int action)
	{
		switch (action)
		{
		case 2101:
		{
			List<SqlScalarExpression> list4 = new List<SqlScalarExpression>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list4;
			break;
		}
		case 2102:
		{
			List<SqlScalarExpression> list3 = value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list3;
			break;
		}
		case 2103:
		{
			SqlLiteralExpression sqlLiteralExpression2 = SqlLiteralExpression.String(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression2.Position = yyloc;
			yyval.value = sqlLiteralExpression2;
			break;
		}
		case 2104:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.String(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression.Position = yyloc;
			yyval.value = sqlLiteralExpression;
			break;
		}
		case 2108:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2111:
		{
			List<SqlExecuteArgument> list2 = new List<SqlExecuteArgument>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlExecuteArgument);
			yyval.value = list2;
			break;
		}
		case 2112:
		{
			List<SqlExecuteArgument> list = value_stack.array[value_stack.top - 3].value as List<SqlExecuteArgument>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlExecuteArgument);
			yyval.value = list;
			break;
		}
		case 2113:
		{
			SqlScalarVariableRefExpression sqlScalarVariableRefExpression = SqlScalarVariableRefExpression.Create(GetText(location_stack.array[location_stack.top - 4]));
			sqlScalarVariableRefExpression.Position = location_stack.array[location_stack.top - 4];
			SqlExecuteArgument sqlExecuteArgument2 = new SqlExecuteArgument(sqlScalarVariableRefExpression, value_stack.array[value_stack.top - 2].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlParameterOption);
			sqlExecuteArgument2.Position = yyloc;
			yyval.value = sqlExecuteArgument2;
			break;
		}
		case 2114:
		{
			SqlExecuteArgument sqlExecuteArgument = new SqlExecuteArgument(value_stack.array[value_stack.top - 2].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlParameterOption);
			sqlExecuteArgument.Position = yyloc;
			yyval.value = sqlExecuteArgument;
			break;
		}
		case 2120:
		{
			SqlModuleOption sqlModuleOption = SqlModuleOption.CreateRecompileOption();
			sqlModuleOption.Position = yyloc;
			yyval.value = sqlModuleOption;
			break;
		}
		case 2100:
		case 2105:
		case 2106:
		case 2107:
		case 2109:
		case 2110:
		case 2115:
		case 2116:
		case 2117:
		case 2118:
		case 2119:
		case 2121:
		case 2122:
		case 2123:
		case 2124:
		case 2125:
		case 2126:
		case 2127:
		case 2128:
		case 2129:
		case 2130:
		case 2131:
		case 2132:
		case 2133:
		case 2134:
		case 2135:
		case 2136:
		case 2137:
		case 2138:
		case 2139:
		case 2140:
		case 2141:
		case 2142:
		case 2143:
		case 2144:
		case 2145:
		case 2146:
		case 2147:
		case 2148:
		case 2149:
			break;
		}
	}

	private void DoAction_2150_2199(int action)
	{
		switch (action)
		{
		case 2150:
		{
			SqlVariableDeclareStatement sqlVariableDeclareStatement = new SqlVariableDeclareStatement(value_stack.array[value_stack.top - 1].value as List<SqlVariableDeclaration>);
			sqlVariableDeclareStatement.Position = yyloc;
			yyval.value = sqlVariableDeclareStatement;
			break;
		}
		case 2151:
		{
			SqlCursorDeclareStatement sqlCursorDeclareStatement = new SqlCursorDeclareStatement(value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 2].value as List<SqlCursorOption>, value_stack.array[value_stack.top - 1].value as CursorDefinitionInfo);
			sqlCursorDeclareStatement.Position = yyloc;
			yyval.value = sqlCursorDeclareStatement;
			break;
		}
		case 2153:
		{
			List<SqlVariableDeclaration> list3 = new List<SqlVariableDeclaration>();
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlVariableDeclaration);
			yyval.value = list3;
			break;
		}
		case 2154:
		{
			List<SqlVariableDeclaration> list2 = value_stack.array[value_stack.top - 3].value as List<SqlVariableDeclaration>;
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlVariableDeclaration);
			yyval.value = list2;
			break;
		}
		case 2155:
		{
			SqlVariableDeclaration sqlVariableDeclaration2 = new SqlVariableDeclaration(GetText(location_stack.array[location_stack.top - 5]), value_stack.array[value_stack.top - 3].value as SqlDataTypeSpecification, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlVariableDeclaration2.Position = yyloc;
			yyval.value = sqlVariableDeclaration2;
			break;
		}
		case 2156:
		{
			SqlVariableDeclaration sqlVariableDeclaration = SqlVariableDeclaration.Create(value_stack.array[value_stack.top - 1].value as SqlParameterDeclaration);
			sqlVariableDeclaration.Position = yyloc;
			yyval.value = sqlVariableDeclaration;
			break;
		}
		case 2158:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2159:
			yyval.value = new CursorDefinitionInfo(value_stack.array[value_stack.top - 1].value as SqlSelectStatement, value_stack.array[value_stack.top - 3].value as List<SqlCursorOption>);
			break;
		case 2160:
			yyval.value = new List<SqlCursorOption>();
			break;
		case 2161:
		{
			SqlCursorOption sqlCursorOption = new SqlCursorOption(GetStringToken(location_stack.array[location_stack.top - 1]));
			sqlCursorOption.Position = location_stack.array[location_stack.top - 1];
			List<SqlCursorOption> list = value_stack.array[value_stack.top - 2].value as List<SqlCursorOption>;
			list.Add(sqlCursorOption);
			yyval.value = list;
			break;
		}
		case 2179:
		{
			SqlReturnStatement sqlReturnStatement = new SqlReturnStatement(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlReturnStatement.Position = yyloc;
			yyval.value = sqlReturnStatement;
			break;
		}
		case 2182:
		{
			SqlIfElseStatement sqlIfElseStatement2 = new SqlIfElseStatement(value_stack.array[value_stack.top - 2].value as SqlBooleanExpression, value_stack.array[value_stack.top - 1].value as SqlStatement);
			sqlIfElseStatement2.Position = yyloc;
			yyval.value = sqlIfElseStatement2;
			break;
		}
		case 2183:
		{
			SqlIfElseStatement sqlIfElseStatement = new SqlIfElseStatement(value_stack.array[value_stack.top - 4].value as SqlBooleanExpression, value_stack.array[value_stack.top - 3].value as SqlStatement, value_stack.array[value_stack.top - 1].value as SqlStatement);
			sqlIfElseStatement.Position = yyloc;
			yyval.value = sqlIfElseStatement;
			break;
		}
		case 2184:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2186:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2187:
		{
			SqlWhileStatement sqlWhileStatement = new SqlWhileStatement(value_stack.array[value_stack.top - 2].value as SqlBooleanExpression, value_stack.array[value_stack.top - 1].value as SqlStatement);
			sqlWhileStatement.Position = yyloc;
			yyval.value = sqlWhileStatement;
			break;
		}
		case 2188:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2189:
		{
			SqlBreakStatement sqlBreakStatement = new SqlBreakStatement();
			sqlBreakStatement.Position = yyloc;
			yyval.value = sqlBreakStatement;
			break;
		}
		case 2190:
		{
			SqlContinueStatement sqlContinueStatement = new SqlContinueStatement();
			sqlContinueStatement.Position = yyloc;
			yyval.value = sqlContinueStatement;
			break;
		}
		case 2192:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 2193:
		{
			SqlBackupRestoreStatement sqlBackupRestoreStatement = value_stack.array[value_stack.top - 3].value as SqlBackupRestoreStatement;
			sqlBackupRestoreStatement.Position = yyloc;
			yyval.value = sqlBackupRestoreStatement;
			break;
		}
		case 2194:
		{
			SqlBackupDatabaseStatement sqlBackupDatabaseStatement = SqlBackupDatabaseStatement.Create(isDumpLoadStatement);
			sqlBackupDatabaseStatement.Position = yyloc;
			yyval.value = sqlBackupDatabaseStatement;
			break;
		}
		case 2195:
		{
			SqlBackupTableStatement sqlBackupTableStatement = SqlBackupTableStatement.Create(isDumpLoadStatement);
			sqlBackupTableStatement.Position = yyloc;
			yyval.value = sqlBackupTableStatement;
			break;
		}
		case 2196:
		{
			SqlBackupLogStatement sqlBackupLogStatement2 = SqlBackupLogStatement.Create(isDumpLoadStatement, isDumpTransaction: false);
			sqlBackupLogStatement2.Position = yyloc;
			yyval.value = sqlBackupLogStatement2;
			break;
		}
		case 2197:
		{
			SqlBackupLogStatement sqlBackupLogStatement = SqlBackupLogStatement.Create(isDumpLoadStatement, isDumpTransaction: true);
			sqlBackupLogStatement.Position = yyloc;
			yyval.value = sqlBackupLogStatement;
			break;
		}
		case 2152:
		case 2157:
		case 2162:
		case 2163:
		case 2164:
		case 2165:
		case 2166:
		case 2167:
		case 2168:
		case 2169:
		case 2170:
		case 2171:
		case 2172:
		case 2173:
		case 2174:
		case 2175:
		case 2176:
		case 2177:
		case 2178:
		case 2180:
		case 2181:
		case 2185:
		case 2191:
		case 2198:
		case 2199:
			break;
		}
	}

	private void DoAction_2200_2249(int action)
	{
		switch (action)
		{
		case 2237:
		{
			SqlBackupRestoreStatement sqlBackupRestoreStatement = value_stack.array[value_stack.top - 3].value as SqlBackupRestoreStatement;
			sqlBackupRestoreStatement.Position = yyloc;
			yyval.value = sqlBackupRestoreStatement;
			break;
		}
		case 2238:
		{
			SqlRestoreDatabaseStatement sqlRestoreDatabaseStatement = SqlRestoreDatabaseStatement.Create(isDumpLoadStatement);
			sqlRestoreDatabaseStatement.Position = yyloc;
			yyval.value = sqlRestoreDatabaseStatement;
			break;
		}
		case 2239:
		{
			SqlRestoreTableStatement sqlRestoreTableStatement = SqlRestoreTableStatement.Create(isDumpLoadStatement);
			sqlRestoreTableStatement.Position = yyloc;
			yyval.value = sqlRestoreTableStatement;
			break;
		}
		case 2240:
		{
			SqlRestoreLogStatement sqlRestoreLogStatement2 = SqlRestoreLogStatement.Create(isDumpLoadStatement, isLoadTransaction: false);
			sqlRestoreLogStatement2.Position = yyloc;
			yyval.value = sqlRestoreLogStatement2;
			break;
		}
		case 2241:
		{
			SqlRestoreLogStatement sqlRestoreLogStatement = SqlRestoreLogStatement.Create(isDumpLoadStatement, isLoadTransaction: true);
			sqlRestoreLogStatement.Position = yyloc;
			yyval.value = sqlRestoreLogStatement;
			break;
		}
		case 2200:
		case 2201:
		case 2202:
		case 2203:
		case 2204:
		case 2205:
		case 2206:
		case 2207:
		case 2208:
		case 2209:
		case 2210:
		case 2211:
		case 2212:
		case 2213:
		case 2214:
		case 2215:
		case 2216:
		case 2217:
		case 2218:
		case 2219:
		case 2220:
		case 2221:
		case 2222:
		case 2223:
		case 2224:
		case 2225:
		case 2226:
		case 2227:
		case 2228:
		case 2229:
		case 2230:
		case 2231:
		case 2232:
		case 2233:
		case 2234:
		case 2235:
		case 2236:
		case 2242:
		case 2243:
		case 2244:
		case 2245:
		case 2246:
		case 2247:
		case 2248:
		case 2249:
			break;
		}
	}

	private void DoAction_2250_2299(int action)
	{
		switch (action)
		{
		case 2256:
		{
			SqlRestoreInformationStatement sqlRestoreInformationStatement = SqlRestoreInformationStatement.Create(isDumpLoadStatement);
			sqlRestoreInformationStatement.Position = yyloc;
			yyval.value = sqlRestoreInformationStatement;
			break;
		}
		case 2260:
		{
			SqlGdrStatement sqlGdrStatement = SqlGdrStatement.Create((SqlGdrStatementType)value_stack.array[value_stack.top - 8].value);
			sqlGdrStatement.Position = yyloc;
			yyval.value = sqlGdrStatement;
			break;
		}
		case 2263:
			scanner.SetGDRState();
			break;
		case 2264:
			scanner.SetGDRState();
			yyval.value = SqlGdrStatementType.SqlGrant;
			break;
		case 2265:
			scanner.SetGDRState();
			yyval.value = SqlGdrStatementType.SqlDeny;
			break;
		case 2266:
			yyval.value = SqlGdrStatementType.SqlRevoke;
			break;
		case 2267:
			scanner.SetGDRState();
			yyval.value = SqlGdrStatementType.SqlRevoke;
			break;
		case 2268:
			scanner.SetGDRState();
			break;
		case 2283:
			scanner.SetGDRState();
			break;
		case 2250:
		case 2251:
		case 2252:
		case 2253:
		case 2254:
		case 2255:
		case 2257:
		case 2258:
		case 2259:
		case 2261:
		case 2262:
		case 2269:
		case 2270:
		case 2271:
		case 2272:
		case 2273:
		case 2274:
		case 2275:
		case 2276:
		case 2277:
		case 2278:
		case 2279:
		case 2280:
		case 2281:
		case 2282:
		case 2284:
		case 2285:
		case 2286:
		case 2287:
		case 2288:
		case 2289:
		case 2290:
		case 2291:
		case 2292:
		case 2293:
		case 2294:
		case 2295:
		case 2296:
		case 2297:
		case 2298:
		case 2299:
			break;
		}
	}

	private void DoAction_2300_2349(int action)
	{
		switch (action)
		{
		case 2300:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2338:
		{
			SqlTryCatchStatement sqlTryCatchStatement = new SqlTryCatchStatement(value_stack.array[value_stack.top - 3].value as List<SqlStatement>, value_stack.array[value_stack.top - 1].value as List<SqlStatement>);
			sqlTryCatchStatement.Position = yyloc;
			yyval.value = sqlTryCatchStatement;
			break;
		}
		case 2343:
			yyval = value_stack.array[value_stack.top - 3];
			break;
		case 2301:
		case 2302:
		case 2303:
		case 2304:
		case 2305:
		case 2306:
		case 2307:
		case 2308:
		case 2309:
		case 2310:
		case 2311:
		case 2312:
		case 2313:
		case 2314:
		case 2315:
		case 2316:
		case 2317:
		case 2318:
		case 2319:
		case 2320:
		case 2321:
		case 2322:
		case 2323:
		case 2324:
		case 2325:
		case 2326:
		case 2327:
		case 2328:
		case 2329:
		case 2330:
		case 2331:
		case 2332:
		case 2333:
		case 2334:
		case 2335:
		case 2336:
		case 2337:
		case 2339:
		case 2340:
		case 2341:
		case 2342:
		case 2344:
		case 2345:
		case 2346:
		case 2347:
		case 2348:
		case 2349:
			break;
		}
	}

	private void DoAction_2350_2399(int action)
	{
		switch (action)
		{
		case 2377:
			scanner.SetParseState(EParseState.e_ParseEventNotifObjectOn);
			break;
		case 2350:
		case 2351:
		case 2352:
		case 2353:
		case 2354:
		case 2355:
		case 2356:
		case 2357:
		case 2358:
		case 2359:
		case 2360:
		case 2361:
		case 2362:
		case 2363:
		case 2364:
		case 2365:
		case 2366:
		case 2367:
		case 2368:
		case 2369:
		case 2370:
		case 2371:
		case 2372:
		case 2373:
		case 2374:
		case 2375:
		case 2376:
		case 2378:
		case 2379:
		case 2380:
		case 2381:
		case 2382:
		case 2383:
		case 2384:
		case 2385:
		case 2386:
		case 2387:
		case 2388:
		case 2389:
		case 2390:
		case 2391:
		case 2392:
		case 2393:
		case 2394:
		case 2395:
		case 2396:
		case 2397:
		case 2398:
		case 2399:
			break;
		}
	}

	private void DoAction_2400_2449(int action)
	{
		switch (action)
		{
		case 2400:
		{
			SqlDBCCStatement sqlDBCCStatement = new SqlDBCCStatement(GetText(location_stack.array[location_stack.top - 3]));
			sqlDBCCStatement.Position = yyloc;
			yyval.value = sqlDBCCStatement;
			break;
		}
		case 2401:
		case 2402:
		case 2403:
		case 2404:
		case 2405:
		case 2406:
		case 2407:
		case 2408:
		case 2409:
		case 2410:
		case 2411:
		case 2412:
		case 2413:
		case 2414:
		case 2415:
		case 2416:
		case 2417:
		case 2418:
		case 2419:
		case 2420:
		case 2421:
		case 2422:
		case 2423:
		case 2424:
		case 2425:
		case 2426:
		case 2427:
		case 2428:
		case 2429:
		case 2430:
		case 2431:
		case 2432:
		case 2433:
		case 2434:
		case 2435:
		case 2436:
		case 2437:
		case 2438:
		case 2439:
		case 2440:
		case 2441:
		case 2442:
		case 2443:
		case 2444:
		case 2445:
		case 2446:
		case 2447:
		case 2448:
		case 2449:
			break;
		}
	}

	private void DoAction_2450_2499(int action)
	{
		switch (action)
		{
		case 2473:
		{
			SqlSetStatement sqlSetStatement = value_stack.array[value_stack.top - 1].value as SqlSetStatement;
			if (sqlSetStatement is SqlSetStatementError)
			{
				VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 2]);
			}
			if (sqlSetStatement != null)
			{
				sqlSetStatement.Position = yyloc;
				yyval.value = sqlSetStatement;
			}
			break;
		}
		case 2474:
		{
			SqlSetStatementError sqlSetStatementError = new SqlSetStatementError();
			sqlSetStatementError.Position = yyloc;
			yyval.value = sqlSetStatementError;
			break;
		}
		case 2486:
		{
			SqlCursorVariableRefExpression sqlCursorVariableRefExpression = SqlCursorVariableRefExpression.Create(value_stack.array[value_stack.top - 3].value as string);
			sqlCursorVariableRefExpression.Position = location_stack.array[location_stack.top - 3];
			SqlSetAssignmentStatement sqlSetAssignmentStatement2 = new SqlSetAssignmentStatement(new SqlCursorVariableAssignment(sqlCursorVariableRefExpression, value_stack.array[value_stack.top - 1].value as CursorDefinitionInfo)
			{
				Position = yyloc
			});
			sqlSetAssignmentStatement2.Position = yyloc;
			yyval.value = sqlSetAssignmentStatement2;
			break;
		}
		case 2487:
		{
			SqlSetAssignmentStatement sqlSetAssignmentStatement = new SqlSetAssignmentStatement(value_stack.array[value_stack.top - 1].value as SqlScalarVariableAssignment);
			sqlSetAssignmentStatement.Position = yyloc;
			yyval.value = sqlSetAssignmentStatement;
			break;
		}
		case 2489:
		{
			SqlScalarVariableRefExpression sqlScalarVariableRefExpression = SqlScalarVariableRefExpression.Create(GetText(location_stack.array[location_stack.top - 1]));
			sqlScalarVariableRefExpression.Position = yyloc;
			yyval.value = sqlScalarVariableRefExpression;
			break;
		}
		case 2493:
			yyval.value = SqlOnOffValue.On;
			break;
		case 2494:
			yyval.value = SqlOnOffValue.Off;
			break;
		case 2450:
		case 2451:
		case 2452:
		case 2453:
		case 2454:
		case 2455:
		case 2456:
		case 2457:
		case 2458:
		case 2459:
		case 2460:
		case 2461:
		case 2462:
		case 2463:
		case 2464:
		case 2465:
		case 2466:
		case 2467:
		case 2468:
		case 2469:
		case 2470:
		case 2471:
		case 2472:
		case 2475:
		case 2476:
		case 2477:
		case 2478:
		case 2479:
		case 2480:
		case 2481:
		case 2482:
		case 2483:
		case 2484:
		case 2485:
		case 2488:
		case 2490:
		case 2491:
		case 2492:
		case 2495:
		case 2496:
		case 2497:
		case 2498:
		case 2499:
			break;
		}
	}

	private void DoAction_2500_2549(int action)
	{
		switch (action)
		{
		case 2545:
		{
			List<SqlSelectExpression> list2 = new List<SqlSelectExpression>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlSelectExpression);
			yyval.value = list2;
			break;
		}
		case 2546:
		{
			List<SqlSelectExpression> list = value_stack.array[value_stack.top - 3].value as List<SqlSelectExpression>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlSelectExpression);
			yyval.value = list;
			break;
		}
		case 2547:
		{
			SqlSelectScalarExpression sqlSelectScalarExpression3 = new SqlSelectScalarExpression(value_stack.array[value_stack.top - 2].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlSelectScalarExpression3.Position = yyloc;
			yyval.value = sqlSelectScalarExpression3;
			break;
		}
		case 2548:
		{
			SqlSelectScalarExpression sqlSelectScalarExpression2 = SqlSelectScalarExpression.IdentityFunction(value_stack.array[value_stack.top - 1].value as SqlIdentityFunctionCallExpression, value_stack.array[value_stack.top - 3].value as SqlIdentifier);
			sqlSelectScalarExpression2.Position = yyloc;
			yyval.value = sqlSelectScalarExpression2;
			break;
		}
		case 2549:
		{
			SqlSelectScalarExpression sqlSelectScalarExpression = new SqlSelectScalarExpression(value_stack.array[value_stack.top - 1].value as SqlScalarExpression, value_stack.array[value_stack.top - 3].value as SqlIdentifier);
			sqlSelectScalarExpression.Position = yyloc;
			yyval.value = sqlSelectScalarExpression;
			break;
		}
		case 2500:
		case 2501:
		case 2502:
		case 2503:
		case 2504:
		case 2505:
		case 2506:
		case 2507:
		case 2508:
		case 2509:
		case 2510:
		case 2511:
		case 2512:
		case 2513:
		case 2514:
		case 2515:
		case 2516:
		case 2517:
		case 2518:
		case 2519:
		case 2520:
		case 2521:
		case 2522:
		case 2523:
		case 2524:
		case 2525:
		case 2526:
		case 2527:
		case 2528:
		case 2529:
		case 2530:
		case 2531:
		case 2532:
		case 2533:
		case 2534:
		case 2535:
		case 2536:
		case 2537:
		case 2538:
		case 2539:
		case 2540:
		case 2541:
		case 2542:
		case 2543:
		case 2544:
			break;
		}
	}

	private void DoAction_2550_2599(int action)
	{
		switch (action)
		{
		case 2550:
		{
			SqlSelectScalarExpression sqlSelectScalarExpression = SqlSelectScalarExpression.IdentityFunction(value_stack.array[value_stack.top - 2].value as SqlIdentityFunctionCallExpression, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlSelectScalarExpression.Position = yyloc;
			yyval.value = sqlSelectScalarExpression;
			break;
		}
		case 2551:
		{
			SqlSelectStarExpression sqlSelectStarExpression2 = new SqlSelectStarExpression();
			sqlSelectStarExpression2.Position = yyloc;
			yyval.value = sqlSelectStarExpression2;
			break;
		}
		case 2552:
		{
			SqlObjectIdentifier sqlObjectIdentifier2 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>);
			sqlObjectIdentifier2.Position = location_stack.array[location_stack.top - 2];
			SqlSelectStarExpression sqlSelectStarExpression = new SqlSelectStarExpression(sqlObjectIdentifier2);
			sqlSelectStarExpression.Position = yyloc;
			yyval.value = sqlSelectStarExpression;
			break;
		}
		case 2553:
		{
			SqlSelectVariableAssignmentExpression sqlSelectVariableAssignmentExpression = new SqlSelectVariableAssignmentExpression(value_stack.array[value_stack.top - 1].value as SqlScalarVariableAssignment);
			sqlSelectVariableAssignmentExpression.Position = yyloc;
			yyval.value = sqlSelectVariableAssignmentExpression;
			break;
		}
		case 2554:
		{
			SqlScalarVariableRefExpression sqlScalarVariableRefExpression = SqlScalarVariableRefExpression.Create(value_stack.array[value_stack.top - 3].value as string);
			sqlScalarVariableRefExpression.Position = location_stack.array[location_stack.top - 3];
			SqlScalarVariableAssignment sqlScalarVariableAssignment = new SqlScalarVariableAssignment(sqlScalarVariableRefExpression, (SqlAssignmentOperatorType)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlScalarVariableAssignment.Position = yyloc;
			yyval.value = sqlScalarVariableAssignment;
			break;
		}
		case 2555:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2559:
		{
			SqlLiteralStringIdentifier sqlLiteralStringIdentifier = new SqlLiteralStringIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralStringIdentifier.Position = yyloc;
			yyval.value = sqlLiteralStringIdentifier;
			break;
		}
		case 2560:
		{
			SqlIdentifier sqlIdentifier2 = new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlIdentifier2.Position = yyloc;
			yyval.value = sqlIdentifier2;
			break;
		}
		case 2561:
		{
			SqlIdentifier sqlIdentifier = new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlIdentifier.Position = yyloc;
			yyval.value = sqlIdentifier;
			break;
		}
		case 2562:
		{
			SqlObjectIdentifier sqlObjectIdentifier5 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier5.Position = yyloc;
			SqlDataTypeSpecification sqlDataTypeSpecification4 = new SqlDataTypeSpecification(new SqlDataType(sqlObjectIdentifier5)
			{
				Position = yyloc
			});
			sqlDataTypeSpecification4.Position = yyloc;
			yyval.value = sqlDataTypeSpecification4;
			break;
		}
		case 2563:
		{
			SqlObjectIdentifier sqlObjectIdentifier4 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 4].value as SqlIdentifier);
			sqlObjectIdentifier4.Position = location_stack.array[location_stack.top - 4];
			SqlDataType dataType2 = new SqlDataType(sqlObjectIdentifier4)
			{
				Position = location_stack.array[location_stack.top - 4]
			};
			SqlLiteralExpression sqlLiteralExpression3 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 2]));
			sqlLiteralExpression3.Position = location_stack.array[location_stack.top - 2];
			SqlDataTypeSpecification sqlDataTypeSpecification3 = new SqlDataTypeSpecification(dataType2, sqlLiteralExpression3);
			sqlDataTypeSpecification3.Position = yyloc;
			yyval.value = sqlDataTypeSpecification3;
			break;
		}
		case 2564:
		{
			SqlObjectIdentifier sqlObjectIdentifier3 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 6].value as SqlIdentifier);
			sqlObjectIdentifier3.Position = location_stack.array[location_stack.top - 6];
			SqlDataType dataType = new SqlDataType(sqlObjectIdentifier3)
			{
				Position = location_stack.array[location_stack.top - 6]
			};
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 4]));
			sqlLiteralExpression.Position = location_stack.array[location_stack.top - 4];
			SqlLiteralExpression sqlLiteralExpression2 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 2]));
			sqlLiteralExpression2.Position = location_stack.array[location_stack.top - 2];
			SqlDataTypeSpecification sqlDataTypeSpecification2 = new SqlDataTypeSpecification(dataType, sqlLiteralExpression, sqlLiteralExpression2);
			sqlDataTypeSpecification2.Position = yyloc;
			yyval.value = sqlDataTypeSpecification2;
			break;
		}
		case 2565:
		{
			SqlIdentityFunctionCallExpression sqlIdentityFunctionCallExpression2 = new SqlIdentityFunctionCallExpression(GetText(location_stack.array[location_stack.top - 4]), value_stack.array[value_stack.top - 2].value as SqlDataTypeSpecification);
			sqlIdentityFunctionCallExpression2.Position = yyloc;
			yyval.value = sqlIdentityFunctionCallExpression2;
			break;
		}
		case 2566:
		{
			SqlIdentityFunctionCallExpression sqlIdentityFunctionCallExpression = new SqlIdentityFunctionCallExpression(GetText(location_stack.array[location_stack.top - 8]), value_stack.array[value_stack.top - 6].value as SqlDataTypeSpecification, value_stack.array[value_stack.top - 4].value as SqlLiteralExpression, value_stack.array[value_stack.top - 2].value as SqlLiteralExpression);
			sqlIdentityFunctionCallExpression.Position = yyloc;
			yyval.value = sqlIdentityFunctionCallExpression;
			break;
		}
		case 2567:
		{
			List<SqlParameterDeclaration> list4 = new List<SqlParameterDeclaration>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlParameterDeclaration);
			yyval.value = list4;
			break;
		}
		case 2568:
		{
			List<SqlParameterDeclaration> list3 = value_stack.array[value_stack.top - 3].value as List<SqlParameterDeclaration>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlParameterDeclaration);
			yyval.value = list3;
			break;
		}
		case 2569:
		{
			SqlParameterDeclaration sqlParameterDeclaration2 = new SqlParameterDeclaration(GetText(location_stack.array[location_stack.top - 6]), value_stack.array[value_stack.top - 4].value as SqlDataTypeSpecification, value_stack.array[value_stack.top - 2].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlParameterOption);
			sqlParameterDeclaration2.Position = yyloc;
			yyval.value = sqlParameterDeclaration2;
			break;
		}
		case 2571:
		{
			SqlDataTypeSpecification sqlDataTypeSpecification = SqlDataTypeSpecification.Cursor();
			sqlDataTypeSpecification.Position = location_stack.array[location_stack.top - 3];
			SqlParameterDeclaration sqlParameterDeclaration = new SqlParameterDeclaration(GetText(location_stack.array[location_stack.top - 5]), sqlDataTypeSpecification, null, value_stack.array[value_stack.top - 1].value as SqlParameterOption);
			sqlParameterDeclaration.Position = yyloc;
			yyval.value = sqlParameterDeclaration;
			break;
		}
		case 2573:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2575:
		{
			SqlParameterOption sqlParameterOption = SqlParameterOption.Create(GetText(location_stack.array[location_stack.top - 1]));
			sqlParameterOption.Position = yyloc;
			yyval.value = sqlParameterOption;
			break;
		}
		case 2579:
		{
			SqlFromClause sqlFromClause = new SqlFromClause(value_stack.array[value_stack.top - 1].value as List<SqlTableExpression>);
			sqlFromClause.Position = yyloc;
			yyval.value = sqlFromClause;
			scanner.SetQueryState(QueryState.None);
			break;
		}
		case 2580:
		{
			List<SqlTableExpression> list2 = new List<SqlTableExpression>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlTableExpression);
			yyval.value = list2;
			break;
		}
		case 2581:
		{
			List<SqlTableExpression> list = value_stack.array[value_stack.top - 3].value as List<SqlTableExpression>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlTableExpression);
			yyval.value = list;
			break;
		}
		case 2582:
			if (value_stack.array[value_stack.top - 1].value == null)
			{
				SqlTableExpression sqlTableExpression6 = new SqlNullTableExpression();
				sqlTableExpression6.Position = yyloc;
				yyval.value = sqlTableExpression6;
			}
			break;
		case 2583:
			if (value_stack.array[value_stack.top - 1].value == null)
			{
				SqlTableExpression sqlTableExpression5 = new SqlNullTableExpression();
				sqlTableExpression5.Position = yyloc;
				yyval.value = sqlTableExpression5;
			}
			break;
		case 2584:
			if (value_stack.array[value_stack.top - 1].value == null)
			{
				SqlTableExpression sqlTableExpression4 = new SqlNullTableExpression();
				sqlTableExpression4.Position = yyloc;
				yyval.value = sqlTableExpression4;
			}
			break;
		case 2585:
			if (value_stack.array[value_stack.top - 1].value == null)
			{
				SqlTableExpression sqlTableExpression3 = new SqlNullTableExpression();
				sqlTableExpression3.Position = yyloc;
				yyval.value = sqlTableExpression3;
			}
			break;
		case 2586:
			if (value_stack.array[value_stack.top - 1].value == null)
			{
				SqlTableExpression sqlTableExpression2 = new SqlNullTableExpression();
				sqlTableExpression2.Position = yyloc;
				yyval.value = sqlTableExpression2;
			}
			break;
		case 2587:
			if (value_stack.array[value_stack.top - 1].value == null)
			{
				SqlTableExpression sqlTableExpression = new SqlNullTableExpression();
				sqlTableExpression.Position = yyloc;
				yyval.value = sqlTableExpression;
			}
			break;
		case 2588:
		{
			SqlTableExpressionError sqlTableExpressionError = new SqlTableExpressionError(value_stack.array[value_stack.top - 1].value as SqlErrorCodeObject);
			sqlTableExpressionError.Position = yyloc;
			yyval.value = sqlTableExpressionError;
			break;
		}
		case 2590:
		{
			SqlTableRefExpression sqlTableRefExpression3 = new SqlTableRefExpression(value_stack.array[value_stack.top - 4].value as SqlObjectIdentifier);
			sqlTableRefExpression3.Position = yyloc;
			yyval.value = sqlTableRefExpression3;
			break;
		}
		case 2591:
		{
			SqlTableUdtInstanceMethodExpression sqlTableUdtInstanceMethodExpression = new SqlTableUdtInstanceMethodExpression(value_stack.array[value_stack.top - 8].value as SqlScalarVariableRefExpression, value_stack.array[value_stack.top - 6].value as SqlIdentifier, value_stack.array[value_stack.top - 4].value as List<SqlScalarExpression>, value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlIdentifier>);
			sqlTableUdtInstanceMethodExpression.Position = yyloc;
			yyval.value = sqlTableUdtInstanceMethodExpression;
			break;
		}
		case 2592:
		{
			SqlTableValuedFunctionRefExpression sqlTableValuedFunctionRefExpression2 = new SqlTableValuedFunctionRefExpression(value_stack.array[value_stack.top - 8].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 4].value as List<SqlScalarExpression>, value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlIdentifier>);
			sqlTableValuedFunctionRefExpression2.Position = yyloc;
			yyval.value = sqlTableValuedFunctionRefExpression2;
			break;
		}
		case 2594:
		{
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(new SqlIdentifier(GetText(location_stack.array[location_stack.top - 3]))
			{
				Position = location_stack.array[location_stack.top - 3]
			});
			sqlObjectIdentifier.Position = location_stack.array[location_stack.top - 3];
			SqlTableValuedFunctionRefExpression sqlTableValuedFunctionRefExpression = new SqlTableValuedFunctionRefExpression(sqlObjectIdentifier, value_stack.array[value_stack.top - 2].value as List<SqlScalarExpression>, value_stack.array[value_stack.top - 1].value as SqlIdentifier, null, isSys: true);
			sqlTableValuedFunctionRefExpression.Position = yyloc;
			yyval.value = sqlTableValuedFunctionRefExpression;
			break;
		}
		case 2595:
		{
			SqlTableRefExpression sqlTableRefExpression2 = new SqlTableRefExpression(value_stack.array[value_stack.top - 5].value as SqlObjectIdentifier, null, value_stack.array[value_stack.top - 1].value as List<SqlHint>);
			sqlTableRefExpression2.Position = yyloc;
			yyval.value = sqlTableRefExpression2;
			break;
		}
		case 2596:
		{
			SqlTableRefExpression sqlTableRefExpression = new SqlTableRefExpression(value_stack.array[value_stack.top - 7].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlHint>);
			sqlTableRefExpression.Position = yyloc;
			yyval.value = sqlTableRefExpression;
			break;
		}
		case 2556:
		case 2557:
		case 2558:
		case 2570:
		case 2572:
		case 2574:
		case 2576:
		case 2577:
		case 2578:
		case 2589:
		case 2593:
		case 2597:
		case 2598:
		case 2599:
			break;
		}
	}

	private void DoAction_2600_2649(int action)
	{
		switch (action)
		{
		case 2607:
		{
			SqlTableVariableRefExpression sqlTableVariableRefExpression = new SqlTableVariableRefExpression(GetText(location_stack.array[location_stack.top - 3]), value_stack.array[value_stack.top - 2].value as SqlIdentifier);
			sqlTableVariableRefExpression.Position = yyloc;
			yyval.value = sqlTableVariableRefExpression;
			break;
		}
		case 2640:
		{
			SqlNullTableExpression sqlNullTableExpression5 = new SqlNullTableExpression();
			sqlNullTableExpression5.Position = yyloc;
			yyval.value = sqlNullTableExpression5;
			break;
		}
		case 2641:
		{
			SqlNullTableExpression sqlNullTableExpression4 = new SqlNullTableExpression();
			sqlNullTableExpression4.Position = yyloc;
			yyval.value = sqlNullTableExpression4;
			break;
		}
		case 2642:
		{
			SqlNullTableExpression sqlNullTableExpression3 = new SqlNullTableExpression();
			sqlNullTableExpression3.Position = yyloc;
			yyval.value = sqlNullTableExpression3;
			break;
		}
		case 2643:
		{
			SqlNullTableExpression sqlNullTableExpression2 = new SqlNullTableExpression();
			sqlNullTableExpression2.Position = yyloc;
			yyval.value = sqlNullTableExpression2;
			break;
		}
		case 2644:
		{
			SqlNullTableExpression sqlNullTableExpression = new SqlNullTableExpression();
			sqlNullTableExpression.Position = yyloc;
			yyval.value = sqlNullTableExpression;
			break;
		}
		case 2600:
		case 2601:
		case 2602:
		case 2603:
		case 2604:
		case 2605:
		case 2606:
		case 2608:
		case 2609:
		case 2610:
		case 2611:
		case 2612:
		case 2613:
		case 2614:
		case 2615:
		case 2616:
		case 2617:
		case 2618:
		case 2619:
		case 2620:
		case 2621:
		case 2622:
		case 2623:
		case 2624:
		case 2625:
		case 2626:
		case 2627:
		case 2628:
		case 2629:
		case 2630:
		case 2631:
		case 2632:
		case 2633:
		case 2634:
		case 2635:
		case 2636:
		case 2637:
		case 2638:
		case 2639:
		case 2645:
		case 2646:
		case 2647:
		case 2648:
		case 2649:
			break;
		}
	}

	private void DoAction_2650_2699(int action)
	{
		switch (action)
		{
		case 2659:
			ExpectKeyword(StringToken.Json, location_stack.array[location_stack.top - 1]);
			break;
		case 2679:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 2683:
		{
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]))
			{
				Position = yyloc
			});
			sqlObjectIdentifier.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression = new SqlColumnRefExpression(sqlObjectIdentifier);
			sqlColumnRefExpression.Position = yyloc;
			yyval.value = sqlColumnRefExpression;
			break;
		}
		case 2684:
		{
			List<SqlScalarExpression> list4 = new List<SqlScalarExpression>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list4;
			break;
		}
		case 2685:
		{
			List<SqlScalarExpression> list3 = value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list3;
			break;
		}
		case 2688:
		{
			List<SqlScalarExpression> list2 = new List<SqlScalarExpression>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list2;
			break;
		}
		case 2689:
		{
			List<SqlScalarExpression> list = value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list;
			break;
		}
		case 2691:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2696:
		{
			SqlDerivedTableExpression sqlDerivedTableExpression2 = new SqlDerivedTableExpression(value_stack.array[value_stack.top - 5].value as SqlQueryExpression, value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlIdentifier>);
			sqlDerivedTableExpression2.Position = yyloc;
			yyval.value = sqlDerivedTableExpression2;
			break;
		}
		case 2697:
		{
			SqlDerivedTableExpression sqlDerivedTableExpression = new SqlDerivedTableExpression(value_stack.array[value_stack.top - 6].value as SqlQueryExpression, value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlIdentifier>);
			sqlDerivedTableExpression.Position = yyloc;
			yyval.value = sqlDerivedTableExpression;
			break;
		}
		case 2650:
		case 2651:
		case 2652:
		case 2653:
		case 2654:
		case 2655:
		case 2656:
		case 2657:
		case 2658:
		case 2660:
		case 2661:
		case 2662:
		case 2663:
		case 2664:
		case 2665:
		case 2666:
		case 2667:
		case 2668:
		case 2669:
		case 2670:
		case 2671:
		case 2672:
		case 2673:
		case 2674:
		case 2675:
		case 2676:
		case 2677:
		case 2678:
		case 2680:
		case 2681:
		case 2682:
		case 2686:
		case 2687:
		case 2690:
		case 2692:
		case 2693:
		case 2694:
		case 2695:
		case 2698:
		case 2699:
			break;
		}
	}

	private void DoAction_2700_2749(int action)
	{
		switch (action)
		{
		case 2700:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 2701:
		{
			SqlUnqualifiedJoinTableExpression sqlUnqualifiedJoinTableExpression3 = new SqlUnqualifiedJoinTableExpression(value_stack.array[value_stack.top - 4].value as SqlTableExpression, SqlJoinOperatorType.CrossJoin, value_stack.array[value_stack.top - 1].value as SqlTableExpression);
			sqlUnqualifiedJoinTableExpression3.Position = yyloc;
			yyval.value = sqlUnqualifiedJoinTableExpression3;
			break;
		}
		case 2702:
		{
			SqlUnqualifiedJoinTableExpression sqlUnqualifiedJoinTableExpression2 = new SqlUnqualifiedJoinTableExpression(value_stack.array[value_stack.top - 4].value as SqlTableExpression, SqlJoinOperatorType.CrossApply, value_stack.array[value_stack.top - 1].value as SqlTableExpression);
			sqlUnqualifiedJoinTableExpression2.Position = yyloc;
			yyval.value = sqlUnqualifiedJoinTableExpression2;
			ExpectKeyword(StringToken.Apply, location_stack.array[location_stack.top - 2]);
			break;
		}
		case 2703:
		{
			SqlUnqualifiedJoinTableExpression sqlUnqualifiedJoinTableExpression = new SqlUnqualifiedJoinTableExpression(value_stack.array[value_stack.top - 4].value as SqlTableExpression, SqlJoinOperatorType.OuterApply, value_stack.array[value_stack.top - 1].value as SqlTableExpression);
			sqlUnqualifiedJoinTableExpression.Position = yyloc;
			yyval.value = sqlUnqualifiedJoinTableExpression;
			ExpectKeyword(StringToken.Apply, location_stack.array[location_stack.top - 2]);
			break;
		}
		case 2704:
		{
			SqlQualifiedJoinTableExpression sqlQualifiedJoinTableExpression = new SqlQualifiedJoinTableExpression(value_stack.array[value_stack.top - 4].value as SqlTableExpression, (SqlJoinOperatorType)value_stack.array[value_stack.top - 3].value, value_stack.array[value_stack.top - 2].value as SqlTableExpression, value_stack.array[value_stack.top - 1].value as SqlConditionClause);
			sqlQualifiedJoinTableExpression.Position = yyloc;
			yyval.value = sqlQualifiedJoinTableExpression;
			break;
		}
		case 2714:
			yyval.value = SqlJoinOperatorType.InnerJoin;
			break;
		case 2715:
			yyval.value = SqlJoinOperatorType.InnerJoin;
			break;
		case 2716:
			yyval.value = SqlJoinOperatorType.LeftOuterJoin;
			break;
		case 2717:
			yyval.value = SqlJoinOperatorType.RightOuterJoin;
			break;
		case 2718:
			yyval.value = SqlJoinOperatorType.FullOuterJoin;
			break;
		case 2719:
		{
			SqlConditionClause sqlConditionClause = new SqlConditionClause(value_stack.array[value_stack.top - 1].value as SqlBooleanExpression);
			sqlConditionClause.Position = yyloc;
			yyval.value = sqlConditionClause;
			break;
		}
		case 2720:
		{
			SqlOnClauseError sqlOnClauseError = new SqlOnClauseError(value_stack.array[value_stack.top - 1].value as SqlBooleanExpression);
			sqlOnClauseError.Position = yyloc;
			yyval.value = sqlOnClauseError;
			break;
		}
		case 2723:
		{
			SqlPivotClause sqlPivotClause = new SqlPivotClause(value_stack.array[value_stack.top - 13].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 11].value as List<SqlColumnRefExpression>, value_stack.array[value_stack.top - 8].value as SqlColumnRefExpression, value_stack.array[value_stack.top - 5].value as List<SqlColumnRefExpression>);
			sqlPivotClause.Position = MergeLocationRange(location_stack.top - 13, location_stack.top - 3);
			SqlPivotTableExpression sqlPivotTableExpression = new SqlPivotTableExpression(value_stack.array[value_stack.top - 16].value as SqlTableExpression, sqlPivotClause, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlPivotTableExpression.Position = yyloc;
			yyval.value = sqlPivotTableExpression;
			break;
		}
		case 2724:
		{
			SqlObjectIdentifier sqlObjectIdentifier2 = SqlObjectIdentifier.Create(new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]))
			{
				Position = yyloc
			});
			sqlObjectIdentifier2.Position = yyloc;
			yyval.value = sqlObjectIdentifier2;
			break;
		}
		case 2725:
		{
			List<SqlIdentifier> obj = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			obj.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(obj);
			sqlObjectIdentifier.Position = yyloc;
			yyval.value = sqlObjectIdentifier;
			break;
		}
		case 2726:
		{
			SqlUnpivotClause sqlUnpivotClause = new SqlUnpivotClause(value_stack.array[value_stack.top - 10].value as SqlColumnRefExpression, value_stack.array[value_stack.top - 8].value as SqlColumnRefExpression, value_stack.array[value_stack.top - 5].value as List<SqlColumnRefExpression>);
			sqlUnpivotClause.Position = MergeLocationRange(location_stack.top - 10, location_stack.top - 3);
			SqlUnpivotTableExpression sqlUnpivotTableExpression = new SqlUnpivotTableExpression(value_stack.array[value_stack.top - 13].value as SqlTableExpression, sqlUnpivotClause, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlUnpivotTableExpression.Position = yyloc;
			yyval.value = sqlUnpivotTableExpression;
			break;
		}
		case 2735:
		{
			List<SqlColumnRefExpression> list3 = new List<SqlColumnRefExpression>();
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlColumnRefExpression);
			yyval.value = list3;
			break;
		}
		case 2736:
		{
			List<SqlColumnRefExpression> list2 = value_stack.array[value_stack.top - 3].value as List<SqlColumnRefExpression>;
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlColumnRefExpression);
			yyval.value = list2;
			break;
		}
		case 2737:
		{
			SqlDmlSpecificationTableSource sqlDmlSpecificationTableSource = new SqlDmlSpecificationTableSource(value_stack.array[value_stack.top - 5].value as SqlDmlSpecification, value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlIdentifier>);
			sqlDmlSpecificationTableSource.Position = yyloc;
			yyval.value = sqlDmlSpecificationTableSource;
			break;
		}
		case 2748:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2749:
		{
			List<SqlHint> list = new List<SqlHint>();
			list.Add(value_stack.array[value_stack.top - 1].value as SqlHint);
			yyval.value = list;
			break;
		}
		case 2705:
		case 2706:
		case 2707:
		case 2708:
		case 2709:
		case 2710:
		case 2711:
		case 2712:
		case 2713:
		case 2721:
		case 2722:
		case 2727:
		case 2728:
		case 2729:
		case 2730:
		case 2731:
		case 2732:
		case 2733:
		case 2734:
		case 2738:
		case 2739:
		case 2740:
		case 2741:
		case 2742:
		case 2743:
		case 2744:
		case 2745:
		case 2746:
		case 2747:
			break;
		}
	}

	private void DoAction_2750_2799(int action)
	{
		switch (action)
		{
		case 2750:
		{
			SqlHint sqlHint = SqlHint.Table(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlHint.Position = location_stack.array[location_stack.top - 1];
			List<SqlHint> list5 = new List<SqlHint>();
			list5.Add(sqlHint);
			yyval.value = list5;
			break;
		}
		case 2751:
		{
			SqlLiteralExpression sqlLiteralExpression3 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 2]));
			sqlLiteralExpression3.Position = location_stack.array[location_stack.top - 2];
			SqlIndexHint sqlIndexHint3 = new SqlIndexHint(sqlLiteralExpression3);
			sqlIndexHint3.Position = yyloc;
			yyval.value = sqlIndexHint3;
			break;
		}
		case 2752:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 2762:
		{
			List<SqlHint> list4 = new List<SqlHint>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlHint);
			yyval.value = list4;
			break;
		}
		case 2763:
		{
			List<SqlHint> list3 = value_stack.array[value_stack.top - 3].value as List<SqlHint>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlHint);
			yyval.value = list3;
			break;
		}
		case 2768:
		{
			SqlTableHint sqlTableHint4 = new SqlTableHint(GetText(location_stack.array[location_stack.top - 3]));
			sqlTableHint4.Position = yyloc;
			yyval.value = sqlTableHint4;
			ExpectKeyword(StringToken.SpatialWindowMaxCells, location_stack.array[location_stack.top - 3]);
			break;
		}
		case 2771:
		{
			SqlTableHint sqlTableHint3 = new SqlTableHint(GetText(location_stack.array[location_stack.top - 7]));
			sqlTableHint3.Position = yyloc;
			yyval.value = sqlTableHint3;
			ExpectKeyword(StringToken.ForceSeek, location_stack.array[location_stack.top - 7]);
			break;
		}
		case 2774:
		{
			SqlTableHint sqlTableHint2 = new SqlTableHint(GetText(location_stack.array[location_stack.top - 1]));
			sqlTableHint2.Position = yyloc;
			yyval.value = sqlTableHint2;
			break;
		}
		case 2775:
		{
			SqlTableHint sqlTableHint = new SqlTableHint(GetText(location_stack.array[location_stack.top - 1]));
			sqlTableHint.Position = yyloc;
			yyval.value = sqlTableHint;
			break;
		}
		case 2776:
		{
			SqlIndexHint sqlIndexHint2 = new SqlIndexHint(value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			sqlIndexHint2.Position = yyloc;
			yyval.value = sqlIndexHint2;
			break;
		}
		case 2777:
		{
			SqlIndexHint sqlIndexHint = new SqlIndexHint(value_stack.array[value_stack.top - 2].value as List<SqlLiteralExpression>);
			sqlIndexHint.Position = yyloc;
			yyval.value = sqlIndexHint;
			break;
		}
		case 2778:
		{
			List<SqlLiteralExpression> list2 = new List<SqlLiteralExpression>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			yyval.value = list2;
			break;
		}
		case 2779:
		{
			List<SqlLiteralExpression> list = value_stack.array[value_stack.top - 3].value as List<SqlLiteralExpression>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			yyval.value = list;
			break;
		}
		case 2781:
		{
			SqlLiteralExpression sqlLiteralExpression2 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression2.Position = yyloc;
			yyval.value = sqlLiteralExpression2;
			break;
		}
		case 2782:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Identifier(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlLiteralExpression.Position = yyloc;
			yyval.value = sqlLiteralExpression;
			break;
		}
		case 2795:
		{
			SqlBinaryQueryExpression sqlBinaryQueryExpression2 = new SqlBinaryQueryExpression(value_stack.array[value_stack.top - 4].value as SqlQueryExpression, (SqlBinaryQueryOperatorType)value_stack.array[value_stack.top - 3].value, (bool)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlBinaryQueryExpression2.Position = yyloc;
			yyval.value = sqlBinaryQueryExpression2;
			break;
		}
		case 2796:
		{
			SqlInlineFunctionBodyDefinition sqlInlineFunctionBodyDefinition = new SqlInlineFunctionBodyDefinition(value_stack.array[value_stack.top - 2].value as SqlQueryWithClause, value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlInlineFunctionBodyDefinition.Position = yyloc;
			yyval.value = sqlInlineFunctionBodyDefinition;
			break;
		}
		case 2798:
		{
			SqlBinaryQueryExpression sqlBinaryQueryExpression = new SqlBinaryQueryExpression(value_stack.array[value_stack.top - 4].value as SqlQueryExpression, SqlBinaryQueryOperatorType.Intersect, (bool)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlBinaryQueryExpression.Position = yyloc;
			yyval.value = sqlBinaryQueryExpression;
			break;
		}
		case 2753:
		case 2754:
		case 2755:
		case 2756:
		case 2757:
		case 2758:
		case 2759:
		case 2760:
		case 2761:
		case 2764:
		case 2765:
		case 2766:
		case 2767:
		case 2769:
		case 2770:
		case 2772:
		case 2773:
		case 2780:
		case 2783:
		case 2784:
		case 2785:
		case 2786:
		case 2787:
		case 2788:
		case 2789:
		case 2790:
		case 2791:
		case 2792:
		case 2793:
		case 2794:
		case 2797:
		case 2799:
			break;
		}
	}

	private void DoAction_2800_2849(int action)
	{
		switch (action)
		{
		case 2800:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 2801:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 2802:
			if (value_stack.array[value_stack.top - 1].value == null)
			{
				SqlNullQueryExpression sqlNullQueryExpression = new SqlNullQueryExpression();
				sqlNullQueryExpression.Position = yyloc;
				yyval.value = sqlNullQueryExpression;
			}
			break;
		case 2803:
		{
			SqlScalarSubQueryExpression sqlScalarSubQueryExpression = new SqlScalarSubQueryExpression(value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlScalarSubQueryExpression.Position = yyloc;
			yyval.value = sqlScalarSubQueryExpression;
			break;
		}
		case 2805:
		{
			SqlQuerySpecification sqlQuerySpecification = new SqlQuerySpecification(new SqlSelectClause(value_stack.array[value_stack.top - 9].value != null, value_stack.array[value_stack.top - 8].value as SqlTopSpecification, value_stack.array[value_stack.top - 7].value as List<SqlSelectExpression>)
			{
				Position = MergeLocationRange(location_stack.top - 10, location_stack.top - 7)
			}, value_stack.array[value_stack.top - 6].value as SqlFromClause, value_stack.array[value_stack.top - 5].value as SqlWhereClause, value_stack.array[value_stack.top - 4].value as SqlGroupByClause, value_stack.array[value_stack.top - 3].value as SqlHavingClause, value_stack.array[value_stack.top - 2].value as SqlOrderByClause, value_stack.array[value_stack.top - 1].value as SqlForClause);
			sqlQuerySpecification.Position = yyloc;
			yyval.value = sqlQuerySpecification;
			break;
		}
		case 2806:
		{
			List<SqlSelectExpression> list15 = new List<SqlSelectExpression>();
			list15.Add(value_stack.array[value_stack.top - 1].value as SqlSelectExpression);
			yyval.value = list15;
			break;
		}
		case 2807:
		{
			List<SqlSelectExpression> list14 = value_stack.array[value_stack.top - 3].value as List<SqlSelectExpression>;
			list14.Add(value_stack.array[value_stack.top - 1].value as SqlSelectExpression);
			yyval.value = list14;
			break;
		}
		case 2808:
		{
			SqlSelectScalarExpression sqlSelectScalarExpression3 = new SqlSelectScalarExpression(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlSelectScalarExpression3.Position = yyloc;
			yyval.value = sqlSelectScalarExpression3;
			break;
		}
		case 2809:
		{
			SqlSelectScalarExpression sqlSelectScalarExpression2 = new SqlSelectScalarExpression(value_stack.array[value_stack.top - 1].value as SqlScalarExpression, value_stack.array[value_stack.top - 3].value as SqlIdentifier);
			sqlSelectScalarExpression2.Position = yyloc;
			yyval.value = sqlSelectScalarExpression2;
			break;
		}
		case 2810:
		{
			SqlSelectScalarExpression sqlSelectScalarExpression = new SqlSelectScalarExpression(value_stack.array[value_stack.top - 2].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlSelectScalarExpression.Position = yyloc;
			yyval.value = sqlSelectScalarExpression;
			break;
		}
		case 2811:
		{
			SqlSelectStarExpression sqlSelectStarExpression2 = new SqlSelectStarExpression();
			sqlSelectStarExpression2.Position = yyloc;
			yyval.value = sqlSelectStarExpression2;
			break;
		}
		case 2812:
		{
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>);
			sqlObjectIdentifier.Position = location_stack.array[location_stack.top - 2];
			SqlSelectStarExpression sqlSelectStarExpression = new SqlSelectStarExpression(sqlObjectIdentifier);
			sqlSelectStarExpression.Position = yyloc;
			yyval.value = sqlSelectStarExpression;
			break;
		}
		case 2813:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2815:
		{
			SqlLiteralStringIdentifier sqlLiteralStringIdentifier = new SqlLiteralStringIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralStringIdentifier.Position = yyloc;
			yyval.value = sqlLiteralStringIdentifier;
			break;
		}
		case 2817:
		{
			SqlOrderByClause sqlOrderByClause = new SqlOrderByClause(value_stack.array[value_stack.top - 2].value as List<SqlOrderByItem>, value_stack.array[value_stack.top - 1].value as SqlOffsetFetchClause);
			sqlOrderByClause.Position = yyloc;
			yyval.value = sqlOrderByClause;
			break;
		}
		case 2818:
		{
			List<SqlOrderByItem> list13 = new List<SqlOrderByItem>();
			list13.Add(value_stack.array[value_stack.top - 1].value as SqlOrderByItem);
			yyval.value = list13;
			break;
		}
		case 2819:
		{
			List<SqlOrderByItem> list12 = value_stack.array[value_stack.top - 3].value as List<SqlOrderByItem>;
			list12.Add(value_stack.array[value_stack.top - 1].value as SqlOrderByItem);
			yyval.value = list12;
			break;
		}
		case 2820:
		{
			SqlOrderByItem sqlOrderByItem = new SqlOrderByItem(value_stack.array[value_stack.top - 2].value as SqlScalarExpression, (SqlSortOrder)value_stack.array[value_stack.top - 1].value);
			sqlOrderByItem.Position = yyloc;
			yyval.value = sqlOrderByItem;
			break;
		}
		case 2822:
			if (ExpectKeywords(new StringToken[2]
			{
				StringToken.Xml,
				StringToken.Json
			}, location_stack.array[location_stack.top - 2]))
			{
				StringToken stringToken = GetStringToken(location_stack.array[location_stack.top - 2]);
				SqlForXmlClause sqlForXmlClause = SqlForXmlClause.Create(value_stack.array[value_stack.top - 1].value as List<SqlForXmlUnknownDirective>, stringToken);
				sqlForXmlClause.Position = yyloc;
				yyval.value = sqlForXmlClause;
			}
			break;
		case 2823:
		{
			SqlForBrowseClause sqlForBrowseClause = new SqlForBrowseClause();
			sqlForBrowseClause.Position = yyloc;
			yyval.value = sqlForBrowseClause;
			break;
		}
		case 2824:
		{
			SqlTableConstructorExpression sqlTableConstructorExpression2 = new SqlTableConstructorExpression(value_stack.array[value_stack.top - 1].value as List<SqlRowConstructorExpression>);
			sqlTableConstructorExpression2.Position = yyloc;
			yyval.value = sqlTableConstructorExpression2;
			break;
		}
		case 2825:
		{
			List<SqlRowConstructorExpression> list11 = new List<SqlRowConstructorExpression>();
			list11.Add(value_stack.array[value_stack.top - 1].value as SqlRowConstructorExpression);
			yyval.value = list11;
			break;
		}
		case 2826:
		{
			List<SqlRowConstructorExpression> list10 = value_stack.array[value_stack.top - 3].value as List<SqlRowConstructorExpression>;
			list10.Add(value_stack.array[value_stack.top - 1].value as SqlRowConstructorExpression);
			yyval.value = list10;
			break;
		}
		case 2827:
		{
			SqlRowConstructorExpression sqlRowConstructorExpression2 = new SqlRowConstructorExpression(value_stack.array[value_stack.top - 2].value as List<SqlScalarExpression>);
			sqlRowConstructorExpression2.Position = yyloc;
			yyval.value = sqlRowConstructorExpression2;
			break;
		}
		case 2828:
		{
			List<SqlScalarExpression> list9 = new List<SqlScalarExpression>();
			list9.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list9;
			break;
		}
		case 2829:
		{
			List<SqlScalarExpression> list8 = value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>;
			list8.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list8;
			break;
		}
		case 2830:
		{
			SqlTableConstructorExpression sqlTableConstructorExpression = new SqlTableConstructorExpression(value_stack.array[value_stack.top - 1].value as List<SqlRowConstructorExpression>);
			sqlTableConstructorExpression.Position = yyloc;
			yyval.value = sqlTableConstructorExpression;
			break;
		}
		case 2831:
		{
			List<SqlRowConstructorExpression> list7 = new List<SqlRowConstructorExpression>();
			list7.Add(value_stack.array[value_stack.top - 1].value as SqlRowConstructorExpression);
			yyval.value = list7;
			break;
		}
		case 2832:
		{
			List<SqlRowConstructorExpression> list6 = value_stack.array[value_stack.top - 3].value as List<SqlRowConstructorExpression>;
			list6.Add(value_stack.array[value_stack.top - 1].value as SqlRowConstructorExpression);
			yyval.value = list6;
			break;
		}
		case 2833:
		{
			SqlRowConstructorExpression sqlRowConstructorExpression = new SqlRowConstructorExpression(value_stack.array[value_stack.top - 2].value as List<SqlScalarExpression>);
			sqlRowConstructorExpression.Position = yyloc;
			yyval.value = sqlRowConstructorExpression;
			break;
		}
		case 2834:
		{
			List<SqlScalarExpression> list5 = new List<SqlScalarExpression>();
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list5;
			break;
		}
		case 2835:
		{
			List<SqlScalarExpression> list4 = value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>;
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list4;
			break;
		}
		case 2837:
		{
			SqlWhereClause sqlWhereClause = new SqlWhereClause(value_stack.array[value_stack.top - 1].value as SqlBooleanExpression);
			sqlWhereClause.Position = yyloc;
			yyval.value = sqlWhereClause;
			scanner.SetQueryState(QueryState.None);
			break;
		}
		case 2841:
		{
			SqlGroupByClause sqlGroupByClause = new SqlGroupByClause(value_stack.array[value_stack.top - 3].value as List<SqlGroupByItem>, value_stack.array[value_stack.top - 1].value as string, (bool)value_stack.array[value_stack.top - 4].value);
			sqlGroupByClause.Position = yyloc;
			yyval.value = sqlGroupByClause;
			scanner.SetParseState(EParseState.e_ParseNeutral);
			break;
		}
		case 2842:
			yyval.value = false;
			break;
		case 2843:
			scanner.SetQueryStatus(QueryStatus.Q_BYALL);
			yyval.value = true;
			break;
		case 2844:
			yyval.value = "";
			break;
		case 2845:
			yyval.value = GetText(location_stack.array[location_stack.top - 2]) + " " + GetText(location_stack.array[location_stack.top - 1]);
			break;
		case 2846:
		{
			List<SqlGroupByItem> list3 = new List<SqlGroupByItem>();
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlGroupByItem);
			yyval.value = list3;
			break;
		}
		case 2848:
		{
			List<SqlGroupByItem> list2 = new List<SqlGroupByItem>();
			list2.Add(value_stack.array[value_stack.top - 3].value as SqlGroupByItem);
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlGroupByItem);
			yyval.value = list2;
			break;
		}
		case 2849:
		{
			List<SqlGroupByItem> list = value_stack.array[value_stack.top - 3].value as List<SqlGroupByItem>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlGroupByItem);
			yyval.value = list;
			break;
		}
		case 2804:
		case 2814:
		case 2816:
		case 2821:
		case 2836:
		case 2838:
		case 2839:
		case 2840:
		case 2847:
			break;
		}
	}

	private void DoAction_2850_2899(int action)
	{
		switch (action)
		{
		case 2856:
		{
			SqlGroupBySets sqlGroupBySets = new SqlGroupBySets(value_stack.array[value_stack.top - 2].value as List<SqlGroupingSet>);
			sqlGroupBySets.Position = yyloc;
			yyval.value = sqlGroupBySets;
			ExpectKeyword(StringToken.Grouping, location_stack.array[location_stack.top - 5]);
			ExpectKeyword(StringToken.Sets, location_stack.array[location_stack.top - 4]);
			break;
		}
		case 2857:
		{
			List<SqlGroupingSet> list10 = new List<SqlGroupingSet>();
			list10.Add(value_stack.array[value_stack.top - 1].value as SqlGroupingSet);
			yyval.value = list10;
			break;
		}
		case 2858:
		{
			List<SqlGroupingSet> list9 = value_stack.array[value_stack.top - 3].value as List<SqlGroupingSet>;
			list9.Add(value_stack.array[value_stack.top - 1].value as SqlGroupingSet);
			yyval.value = list9;
			break;
		}
		case 2859:
		{
			SqlGrandTotalGroupingSet sqlGrandTotalGroupingSet = new SqlGrandTotalGroupingSet(value_stack.array[value_stack.top - 1].value as SqlGrandTotalGroupByItem);
			sqlGrandTotalGroupingSet.Position = yyloc;
			yyval.value = sqlGrandTotalGroupingSet;
			break;
		}
		case 2860:
		{
			SqlGroupingSetItemsCollection sqlGroupingSetItemsCollection4 = new SqlGroupingSetItemsCollection(value_stack.array[value_stack.top - 1].value as SqlGroupingSetItem);
			sqlGroupingSetItemsCollection4.Position = yyloc;
			yyval.value = sqlGroupingSetItemsCollection4;
			break;
		}
		case 2861:
		{
			SqlGroupingSetItemsCollection sqlGroupingSetItemsCollection3 = new SqlGroupingSetItemsCollection(value_stack.array[value_stack.top - 2].value as List<SqlGroupingSetItem>);
			sqlGroupingSetItemsCollection3.Position = yyloc;
			yyval.value = sqlGroupingSetItemsCollection3;
			break;
		}
		case 2862:
		{
			SqlGroupingSetItemsCollection sqlGroupingSetItemsCollection2 = new SqlGroupingSetItemsCollection(value_stack.array[value_stack.top - 2].value as SqlRollupGroupByItem);
			sqlGroupingSetItemsCollection2.Position = yyloc;
			yyval.value = sqlGroupingSetItemsCollection2;
			break;
		}
		case 2863:
		{
			SqlGroupingSetItemsCollection sqlGroupingSetItemsCollection = new SqlGroupingSetItemsCollection(value_stack.array[value_stack.top - 2].value as SqlCubeGroupByItem);
			sqlGroupingSetItemsCollection.Position = yyloc;
			yyval.value = sqlGroupingSetItemsCollection;
			break;
		}
		case 2864:
		{
			SqlGrandTotalGroupByItem sqlGrandTotalGroupByItem = new SqlGrandTotalGroupByItem();
			sqlGrandTotalGroupByItem.Position = yyloc;
			yyval.value = sqlGrandTotalGroupByItem;
			break;
		}
		case 2865:
		{
			List<SqlGroupingSetItem> list8 = new List<SqlGroupingSetItem>();
			list8.Add(value_stack.array[value_stack.top - 3].value as SqlGroupingSetItem);
			list8.Add(value_stack.array[value_stack.top - 1].value as SqlGroupingSetItem);
			yyval.value = list8;
			break;
		}
		case 2866:
		{
			List<SqlGroupingSetItem> list7 = value_stack.array[value_stack.top - 3].value as List<SqlGroupingSetItem>;
			list7.Add(value_stack.array[value_stack.top - 1].value as SqlGroupingSetItem);
			yyval.value = list7;
			break;
		}
		case 2870:
		{
			SqlRollupGroupByItem sqlRollupGroupByItem = new SqlRollupGroupByItem(value_stack.array[value_stack.top - 2].value as List<SqlSimpleGroupByItem>);
			sqlRollupGroupByItem.Position = yyloc;
			yyval.value = sqlRollupGroupByItem;
			break;
		}
		case 2871:
		{
			SqlCubeGroupByItem sqlCubeGroupByItem = new SqlCubeGroupByItem(value_stack.array[value_stack.top - 2].value as List<SqlSimpleGroupByItem>);
			sqlCubeGroupByItem.Position = yyloc;
			yyval.value = sqlCubeGroupByItem;
			break;
		}
		case 2873:
		{
			List<SqlSimpleGroupByItem> list6 = value_stack.array[value_stack.top - 3].value as List<SqlSimpleGroupByItem>;
			list6.AddRange(value_stack.array[value_stack.top - 1].value as List<SqlSimpleGroupByItem>);
			yyval.value = list6;
			break;
		}
		case 2874:
		{
			List<SqlSimpleGroupByItem> list5 = new List<SqlSimpleGroupByItem>();
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlSimpleGroupByItem);
			yyval.value = list5;
			break;
		}
		case 2875:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 2876:
		{
			List<SqlSimpleGroupByItem> list4 = new List<SqlSimpleGroupByItem>();
			list4.Add(value_stack.array[value_stack.top - 3].value as SqlSimpleGroupByItem);
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlSimpleGroupByItem);
			yyval.value = list4;
			break;
		}
		case 2877:
		{
			List<SqlSimpleGroupByItem> list3 = value_stack.array[value_stack.top - 3].value as List<SqlSimpleGroupByItem>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlSimpleGroupByItem);
			yyval.value = list3;
			break;
		}
		case 2878:
		{
			SqlSimpleGroupByItem sqlSimpleGroupByItem3 = new SqlSimpleGroupByItem(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlSimpleGroupByItem3.Position = yyloc;
			yyval.value = sqlSimpleGroupByItem3;
			break;
		}
		case 2879:
		{
			SqlSimpleGroupByItem sqlSimpleGroupByItem2 = new SqlSimpleGroupByItem(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlSimpleGroupByItem2.Position = yyloc;
			yyval.value = sqlSimpleGroupByItem2;
			break;
		}
		case 2880:
		{
			SqlSimpleGroupByItem sqlSimpleGroupByItem = new SqlSimpleGroupByItem(value_stack.array[value_stack.top - 5].value as SqlScalarExpression);
			sqlSimpleGroupByItem.Position = yyloc;
			yyval.value = sqlSimpleGroupByItem;
			break;
		}
		case 2884:
		{
			SqlHavingClause sqlHavingClause = new SqlHavingClause(value_stack.array[value_stack.top - 1].value as SqlBooleanExpression);
			sqlHavingClause.Position = yyloc;
			yyval.value = sqlHavingClause;
			break;
		}
		case 2889:
		{
			SqlOrderByClause sqlOrderByClause2 = new SqlOrderByClause(value_stack.array[value_stack.top - 1].value as List<SqlOrderByItem>);
			sqlOrderByClause2.Position = yyloc;
			yyval.value = sqlOrderByClause2;
			break;
		}
		case 2890:
		{
			SqlOrderByClause sqlOrderByClause = value_stack.array[value_stack.top - 2].value as SqlOrderByClause;
			sqlOrderByClause.OffsetFetchClause = value_stack.array[value_stack.top - 1].value as SqlOffsetFetchClause;
			sqlOrderByClause.Position = yyloc;
			yyval.value = sqlOrderByClause;
			break;
		}
		case 2891:
		{
			List<SqlOrderByItem> list2 = new List<SqlOrderByItem>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlOrderByItem);
			yyval.value = list2;
			break;
		}
		case 2892:
		{
			List<SqlOrderByItem> list = value_stack.array[value_stack.top - 3].value as List<SqlOrderByItem>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlOrderByItem);
			yyval.value = list;
			break;
		}
		case 2893:
		{
			SqlOrderByItem sqlOrderByItem = new SqlOrderByItem(value_stack.array[value_stack.top - 2].value as SqlScalarExpression, (SqlSortOrder)value_stack.array[value_stack.top - 1].value);
			sqlOrderByItem.Position = yyloc;
			yyval.value = sqlOrderByItem;
			break;
		}
		case 2894:
			yyval.value = SqlSortOrder.None;
			break;
		case 2895:
			yyval.value = SqlSortOrder.Ascending;
			break;
		case 2896:
			yyval.value = SqlSortOrder.Descending;
			break;
		case 2898:
		{
			SqlBooleanExpressionError sqlBooleanExpressionError = new SqlBooleanExpressionError(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBooleanExpressionError.Position = yyloc;
			yyval.value = sqlBooleanExpressionError;
			break;
		}
		case 2899:
		{
			SqlBooleanFilterExpression sqlBooleanFilterExpression = SqlBooleanFilterExpression.Create(value_stack.array[value_stack.top - 1].value as SqlComparisonBooleanExpression);
			sqlBooleanFilterExpression.Position = yyloc;
			yyval.value = sqlBooleanFilterExpression;
			break;
		}
		case 2850:
		case 2851:
		case 2852:
		case 2853:
		case 2854:
		case 2855:
		case 2867:
		case 2868:
		case 2869:
		case 2872:
		case 2881:
		case 2882:
		case 2883:
		case 2885:
		case 2886:
		case 2887:
		case 2888:
		case 2897:
			break;
		}
	}

	private void DoAction_2900_2949(int action)
	{
		switch (action)
		{
		case 2900:
		{
			SqlBooleanFilterExpression sqlBooleanFilterExpression2 = SqlBooleanFilterExpression.Create(value_stack.array[value_stack.top - 1].value as SqlIsNullBooleanExpression);
			sqlBooleanFilterExpression2.Position = yyloc;
			yyval.value = sqlBooleanFilterExpression2;
			break;
		}
		case 2901:
		{
			SqlBooleanFilterExpression sqlBooleanFilterExpression = SqlBooleanFilterExpression.Create(value_stack.array[value_stack.top - 1].value as SqlInBooleanExpression);
			sqlBooleanFilterExpression.Position = yyloc;
			yyval.value = sqlBooleanFilterExpression;
			break;
		}
		case 2902:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 2903:
		{
			SqlBinaryFilterExpression sqlBinaryFilterExpression = new SqlBinaryFilterExpression(value_stack.array[value_stack.top - 3].value as SqlFilterExpression, value_stack.array[value_stack.top - 1].value as SqlFilterExpression);
			sqlBinaryFilterExpression.Position = yyloc;
			yyval.value = sqlBinaryFilterExpression;
			break;
		}
		case 2913:
		{
			SqlBooleanExpression sqlBooleanExpression = value_stack.array[value_stack.top - 2].value as SqlBooleanExpression;
			sqlBooleanExpression.Position = yyloc;
			yyval.value = sqlBooleanExpression;
			break;
		}
		case 2915:
		{
			SqlNotBooleanExpression sqlNotBooleanExpression = new SqlNotBooleanExpression(value_stack.array[value_stack.top - 1].value as SqlBooleanExpression);
			sqlNotBooleanExpression.Position = yyloc;
			yyval.value = sqlNotBooleanExpression;
			break;
		}
		case 2916:
		{
			SqlBinaryBooleanExpression sqlBinaryBooleanExpression2 = new SqlBinaryBooleanExpression(value_stack.array[value_stack.top - 3].value as SqlBooleanExpression, SqlBooleanOperatorType.And, value_stack.array[value_stack.top - 1].value as SqlBooleanExpression);
			sqlBinaryBooleanExpression2.Position = yyloc;
			yyval.value = sqlBinaryBooleanExpression2;
			break;
		}
		case 2917:
		{
			SqlBinaryBooleanExpression sqlBinaryBooleanExpression = new SqlBinaryBooleanExpression(value_stack.array[value_stack.top - 3].value as SqlBooleanExpression, SqlBooleanOperatorType.Or, value_stack.array[value_stack.top - 1].value as SqlBooleanExpression);
			sqlBinaryBooleanExpression.Position = yyloc;
			yyval.value = sqlBinaryBooleanExpression;
			break;
		}
		case 2918:
		{
			SqlComparisonBooleanExpression sqlComparisonBooleanExpression = new SqlComparisonBooleanExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, (SqlComparisonBooleanExpressionType)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlComparisonBooleanExpression.Position = yyloc;
			yyval.value = sqlComparisonBooleanExpression;
			break;
		}
		case 2919:
			yyval.value = SqlComparisonBooleanExpressionType.Equals;
			break;
		case 2920:
			yyval.value = SqlComparisonBooleanExpressionType.NotEqual;
			break;
		case 2921:
			yyval.value = SqlComparisonBooleanExpressionType.GreaterThan;
			break;
		case 2922:
			yyval.value = SqlComparisonBooleanExpressionType.GreaterThanOrEqual;
			break;
		case 2923:
			yyval.value = SqlComparisonBooleanExpressionType.LessThan;
			break;
		case 2924:
			yyval.value = SqlComparisonBooleanExpressionType.LessOrGreaterThan;
			break;
		case 2925:
			yyval.value = SqlComparisonBooleanExpressionType.LessThanOrEqual;
			break;
		case 2926:
			yyval.value = SqlComparisonBooleanExpressionType.NotLessThan;
			break;
		case 2927:
			yyval.value = SqlComparisonBooleanExpressionType.NotGreaterThan;
			break;
		case 2928:
		{
			SqlBetweenBooleanExpression sqlBetweenBooleanExpression2 = new SqlBetweenBooleanExpression(value_stack.array[value_stack.top - 5].value as SqlScalarExpression, value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBetweenBooleanExpression2.Position = yyloc;
			yyval.value = sqlBetweenBooleanExpression2;
			break;
		}
		case 2929:
		{
			SqlBetweenBooleanExpression sqlBetweenBooleanExpression = new SqlBetweenBooleanExpression(value_stack.array[value_stack.top - 6].value as SqlScalarExpression, hasNot: true, value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBetweenBooleanExpression.Position = yyloc;
			yyval.value = sqlBetweenBooleanExpression;
			break;
		}
		case 2930:
		{
			SqlIsNullBooleanExpression sqlIsNullBooleanExpression2 = new SqlIsNullBooleanExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression);
			sqlIsNullBooleanExpression2.Position = yyloc;
			yyval.value = sqlIsNullBooleanExpression2;
			break;
		}
		case 2931:
		{
			SqlIsNullBooleanExpression sqlIsNullBooleanExpression = new SqlIsNullBooleanExpression(value_stack.array[value_stack.top - 4].value as SqlScalarExpression, hasNot: true);
			sqlIsNullBooleanExpression.Position = yyloc;
			yyval.value = sqlIsNullBooleanExpression;
			break;
		}
		case 2932:
		{
			SqlInBooleanExpression sqlInBooleanExpression2 = new SqlInBooleanExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlInBooleanExpressionValue);
			sqlInBooleanExpression2.Position = yyloc;
			yyval.value = sqlInBooleanExpression2;
			break;
		}
		case 2933:
		{
			SqlInBooleanExpression sqlInBooleanExpression = new SqlInBooleanExpression(value_stack.array[value_stack.top - 4].value as SqlScalarExpression, hasNot: true, value_stack.array[value_stack.top - 1].value as SqlInBooleanExpressionValue);
			sqlInBooleanExpression.Position = yyloc;
			yyval.value = sqlInBooleanExpression;
			break;
		}
		case 2934:
		{
			SqlInBooleanExpressionQueryValue sqlInBooleanExpressionQueryValue = new SqlInBooleanExpressionQueryValue(value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlInBooleanExpressionQueryValue.Position = yyloc;
			yyval.value = sqlInBooleanExpressionQueryValue;
			break;
		}
		case 2935:
		{
			SqlInBooleanExpressionCollectionValue sqlInBooleanExpressionCollectionValue = new SqlInBooleanExpressionCollectionValue(value_stack.array[value_stack.top - 2].value as List<SqlScalarExpression>);
			sqlInBooleanExpressionCollectionValue.Position = yyloc;
			yyval.value = sqlInBooleanExpressionCollectionValue;
			break;
		}
		case 2936:
		{
			List<SqlScalarExpression> list6 = new List<SqlScalarExpression>();
			list6.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list6;
			break;
		}
		case 2937:
			yyval.value = value_stack.array[value_stack.top - 1].value as List<SqlScalarExpression>;
			break;
		case 2938:
		{
			List<SqlScalarExpression> list5 = new List<SqlScalarExpression>();
			list5.Add(value_stack.array[value_stack.top - 3].value as SqlScalarExpression);
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list5;
			break;
		}
		case 2939:
		{
			List<SqlScalarExpression> list4 = value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>;
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list4;
			break;
		}
		case 2940:
		{
			SqlFullTextBooleanExpression sqlFullTextBooleanExpression = new SqlFullTextBooleanExpression((SqlFullTextFunctionType)value_stack.array[value_stack.top - 7].value, value_stack.array[value_stack.top - 5].value as List<SqlFullTextColumn>, value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 2].value as SqlScalarExpression);
			sqlFullTextBooleanExpression.Position = yyloc;
			yyval.value = sqlFullTextBooleanExpression;
			break;
		}
		case 2941:
			yyval.value = SqlFullTextFunctionType.Contains;
			break;
		case 2942:
			yyval.value = SqlFullTextFunctionType.FreeText;
			break;
		case 2943:
		{
			List<SqlFullTextColumn> list3 = new List<SqlFullTextColumn>();
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlFullTextColumn);
			yyval.value = list3;
			break;
		}
		case 2944:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 2946:
		{
			SqlFullTextColumn sqlFullTextColumn4 = new SqlFullTextColumn(value_stack.array[value_stack.top - 1].value as SqlScalarRefExpression);
			sqlFullTextColumn4.Position = yyloc;
			List<SqlFullTextColumn> list2 = new List<SqlFullTextColumn>();
			list2.Add(sqlFullTextColumn4);
			yyval.value = list2;
			break;
		}
		case 2947:
		{
			SqlFullTextColumn sqlFullTextColumn3 = new SqlFullTextColumn(value_stack.array[value_stack.top - 1].value as SqlScalarRefExpression);
			sqlFullTextColumn3.Position = location_stack.array[location_stack.top - 1];
			List<SqlFullTextColumn> list = value_stack.array[value_stack.top - 3].value as List<SqlFullTextColumn>;
			list.Add(sqlFullTextColumn3);
			yyval.value = list;
			break;
		}
		case 2948:
		{
			SqlFullTextColumn sqlFullTextColumn2 = new SqlFullTextColumn(value_stack.array[value_stack.top - 1].value as SqlScalarRefExpression);
			sqlFullTextColumn2.Position = yyloc;
			yyval.value = sqlFullTextColumn2;
			break;
		}
		case 2949:
		{
			SqlFullTextColumn sqlFullTextColumn = new SqlFullTextColumn(value_stack.array[value_stack.top - 1].value as SqlSelectStarExpression);
			sqlFullTextColumn.Position = yyloc;
			yyval.value = sqlFullTextColumn;
			break;
		}
		case 2904:
		case 2905:
		case 2906:
		case 2907:
		case 2908:
		case 2909:
		case 2910:
		case 2911:
		case 2912:
		case 2914:
		case 2945:
			break;
		}
	}

	private void DoAction_2950_2999(int action)
	{
		switch (action)
		{
		case 2950:
		{
			SqlFullTextColumn sqlFullTextColumn = new SqlFullTextColumn(value_stack.array[value_stack.top - 2].value as SqlSelectStarExpression);
			sqlFullTextColumn.Position = yyloc;
			yyval.value = sqlFullTextColumn;
			break;
		}
		case 2951:
		{
			SqlSelectStarExpression sqlSelectStarExpression2 = new SqlSelectStarExpression();
			sqlSelectStarExpression2.Position = yyloc;
			yyval.value = sqlSelectStarExpression2;
			break;
		}
		case 2952:
		{
			SqlObjectIdentifier sqlObjectIdentifier3 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>);
			sqlObjectIdentifier3.Position = location_stack.array[location_stack.top - 2];
			SqlSelectStarExpression sqlSelectStarExpression = new SqlSelectStarExpression(sqlObjectIdentifier3);
			sqlSelectStarExpression.Position = yyloc;
			yyval.value = sqlSelectStarExpression;
			break;
		}
		case 2960:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2961:
			if (ExpectKeyword(StringToken.Language, location_stack.array[location_stack.top - 2]))
			{
				yyval = value_stack.array[value_stack.top - 1];
			}
			break;
		case 2962:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression.Position = yyloc;
			yyval.value = sqlLiteralExpression;
			break;
		}
		case 2963:
		{
			SqlLiteralExpression sqlLiteralExpression2 = SqlLiteralExpression.Binary(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression2.Position = yyloc;
			yyval.value = sqlLiteralExpression2;
			break;
		}
		case 2965:
		{
			SqlAllAnyComparisonBooleanExpression sqlAllAnyComparisonBooleanExpression2 = new SqlAllAnyComparisonBooleanExpression(value_stack.array[value_stack.top - 4].value as SqlScalarExpression, (SqlComparisonBooleanExpressionType)value_stack.array[value_stack.top - 3].value, "ANY", value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlAllAnyComparisonBooleanExpression2.Position = yyloc;
			yyval.value = sqlAllAnyComparisonBooleanExpression2;
			break;
		}
		case 2966:
		{
			SqlAllAnyComparisonBooleanExpression sqlAllAnyComparisonBooleanExpression = new SqlAllAnyComparisonBooleanExpression(value_stack.array[value_stack.top - 4].value as SqlScalarExpression, (SqlComparisonBooleanExpressionType)value_stack.array[value_stack.top - 3].value, "ALL", value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlAllAnyComparisonBooleanExpression.Position = yyloc;
			yyval.value = sqlAllAnyComparisonBooleanExpression;
			break;
		}
		case 2967:
		{
			SqlExistsBooleanExpression sqlExistsBooleanExpression = new SqlExistsBooleanExpression(value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlExistsBooleanExpression.Position = yyloc;
			yyval.value = sqlExistsBooleanExpression;
			break;
		}
		case 2968:
		{
			SqlLikeBooleanExpression sqlLikeBooleanExpression = new SqlLikeBooleanExpression(value_stack.array[value_stack.top - 4].value as SqlScalarExpression, (bool)value_stack.array[value_stack.top - 3].value, value_stack.array[value_stack.top - 2].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlLikeBooleanExpression.Position = yyloc;
			yyval.value = sqlLikeBooleanExpression;
			break;
		}
		case 2969:
			yyval.value = false;
			break;
		case 2970:
			yyval.value = true;
			break;
		case 2974:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2978:
		{
			SqlCollation sqlCollation = new SqlCollation(new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]))
			{
				Position = location_stack.array[location_stack.top - 1]
			});
			sqlCollation.Position = yyloc;
			yyval.value = sqlCollation;
			if (EscapeSequence.IdentifyEscapeSequence(GetText(location_stack.array[location_stack.top - 1])) != null)
			{
				VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 1]);
			}
			break;
		}
		case 2980:
		{
			SqlScalarExpression sqlScalarExpression5 = value_stack.array[value_stack.top - 2].value as SqlScalarSubQueryExpression;
			if (sqlScalarExpression5 == null)
			{
				sqlScalarExpression5 = ShiftReduceParser.UpdatePos(new SqlNullScalarExpression(), location_stack.array[location_stack.top - 2]);
			}
			SqlScalarExpression sqlScalarExpression6 = SqlScalarExpression.Create(sqlScalarExpression5, value_stack.array[value_stack.top - 1].value as SqlCollation);
			sqlScalarExpression6.Position = yyloc;
			yyval.value = sqlScalarExpression6;
			break;
		}
		case 2981:
		{
			SqlScalarExpression sqlScalarExpression3 = value_stack.array[value_stack.top - 2].value as SqlScalarExpression;
			if (sqlScalarExpression3 == null)
			{
				sqlScalarExpression3 = ShiftReduceParser.UpdatePos(new SqlNullScalarExpression(), location_stack.array[location_stack.top - 2]);
			}
			SqlScalarExpression sqlScalarExpression4 = SqlScalarExpression.Create(sqlScalarExpression3, value_stack.array[value_stack.top - 1].value as SqlCollation);
			sqlScalarExpression4.Position = yyloc;
			yyval.value = sqlScalarExpression4;
			break;
		}
		case 2982:
		{
			SqlScalarExpression sqlScalarExpression = value_stack.array[value_stack.top - 2].value as SqlScalarRefExpression;
			if (sqlScalarExpression == null)
			{
				sqlScalarExpression = ShiftReduceParser.UpdatePos(new SqlNullScalarExpression(), location_stack.array[location_stack.top - 2]);
			}
			SqlScalarExpression sqlScalarExpression2 = SqlScalarExpression.Create(sqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlCollation);
			sqlScalarExpression2.Position = yyloc;
			yyval.value = sqlScalarExpression2;
			break;
		}
		case 2983:
		{
			SqlUnaryScalarExpression sqlUnaryScalarExpression3 = new SqlUnaryScalarExpression(SqlUnaryScalarOperatorType.Positive, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlUnaryScalarExpression3.Position = yyloc;
			yyval.value = sqlUnaryScalarExpression3;
			break;
		}
		case 2984:
		{
			SqlUnaryScalarExpression sqlUnaryScalarExpression2 = new SqlUnaryScalarExpression(SqlUnaryScalarOperatorType.Negative, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlUnaryScalarExpression2.Position = yyloc;
			yyval.value = sqlUnaryScalarExpression2;
			break;
		}
		case 2985:
		{
			SqlUnaryScalarExpression sqlUnaryScalarExpression = new SqlUnaryScalarExpression(SqlUnaryScalarOperatorType.BitwiseNot, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlUnaryScalarExpression.Position = yyloc;
			yyval.value = sqlUnaryScalarExpression;
			break;
		}
		case 2986:
		{
			SqlBinaryScalarExpression sqlBinaryScalarExpression8 = new SqlBinaryScalarExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, SqlBinaryScalarOperatorType.Multiply, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBinaryScalarExpression8.Position = yyloc;
			yyval.value = sqlBinaryScalarExpression8;
			break;
		}
		case 2987:
		{
			SqlBinaryScalarExpression sqlBinaryScalarExpression7 = new SqlBinaryScalarExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, SqlBinaryScalarOperatorType.Divide, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBinaryScalarExpression7.Position = yyloc;
			yyval.value = sqlBinaryScalarExpression7;
			break;
		}
		case 2988:
		{
			SqlBinaryScalarExpression sqlBinaryScalarExpression6 = new SqlBinaryScalarExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, SqlBinaryScalarOperatorType.Modulus, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBinaryScalarExpression6.Position = yyloc;
			yyval.value = sqlBinaryScalarExpression6;
			break;
		}
		case 2989:
		{
			SqlBinaryScalarExpression sqlBinaryScalarExpression5 = new SqlBinaryScalarExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, SqlBinaryScalarOperatorType.Add, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBinaryScalarExpression5.Position = yyloc;
			yyval.value = sqlBinaryScalarExpression5;
			break;
		}
		case 2990:
		{
			SqlBinaryScalarExpression sqlBinaryScalarExpression4 = new SqlBinaryScalarExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, SqlBinaryScalarOperatorType.Subtract, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBinaryScalarExpression4.Position = yyloc;
			yyval.value = sqlBinaryScalarExpression4;
			break;
		}
		case 2991:
		{
			SqlBinaryScalarExpression sqlBinaryScalarExpression3 = new SqlBinaryScalarExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, SqlBinaryScalarOperatorType.BitwiseAnd, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBinaryScalarExpression3.Position = yyloc;
			yyval.value = sqlBinaryScalarExpression3;
			break;
		}
		case 2992:
		{
			SqlBinaryScalarExpression sqlBinaryScalarExpression2 = new SqlBinaryScalarExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, SqlBinaryScalarOperatorType.BitwiseOr, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBinaryScalarExpression2.Position = yyloc;
			yyval.value = sqlBinaryScalarExpression2;
			break;
		}
		case 2993:
		{
			SqlBinaryScalarExpression sqlBinaryScalarExpression = new SqlBinaryScalarExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, SqlBinaryScalarOperatorType.BitwiseXor, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBinaryScalarExpression.Position = yyloc;
			yyval.value = sqlBinaryScalarExpression;
			break;
		}
		case 2994:
		{
			SqlAtTimeZoneExpression sqlAtTimeZoneExpression = new SqlAtTimeZoneExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlAtTimeZoneExpression.Position = yyloc;
			yyval.value = sqlAtTimeZoneExpression;
			break;
		}
		case 2995:
		{
			SqlNullScalarExpression sqlNullScalarExpression = new SqlNullScalarExpression();
			sqlNullScalarExpression.Position = yyloc;
			yyval.value = sqlNullScalarExpression;
			break;
		}
		case 2996:
		{
			SqlScalarExpressionError sqlScalarExpressionError = new SqlScalarExpressionError(value_stack.array[value_stack.top - 1].value as SqlErrorCodeObject);
			sqlScalarExpressionError.Position = yyloc;
			yyval.value = sqlScalarExpressionError;
			break;
		}
		case 2997:
		{
			SqlObjectIdentifier sqlObjectIdentifier2 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier2.Position = yyloc;
			SqlDataTypeSpecification sqlDataTypeSpecification2 = new SqlDataTypeSpecification(new SqlDataType(sqlObjectIdentifier2)
			{
				Position = yyloc
			});
			sqlDataTypeSpecification2.Position = yyloc;
			yyval.value = sqlDataTypeSpecification2;
			break;
		}
		case 2998:
		{
			List<SqlIdentifier> obj = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			obj.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(obj);
			sqlObjectIdentifier.Position = yyloc;
			SqlDataTypeSpecification sqlDataTypeSpecification = new SqlDataTypeSpecification(new SqlDataType(sqlObjectIdentifier)
			{
				Position = yyloc
			});
			sqlDataTypeSpecification.Position = yyloc;
			yyval.value = sqlDataTypeSpecification;
			break;
		}
		case 2953:
		case 2954:
		case 2955:
		case 2956:
		case 2957:
		case 2958:
		case 2959:
		case 2964:
		case 2971:
		case 2972:
		case 2973:
		case 2975:
		case 2976:
		case 2977:
		case 2979:
		case 2999:
			break;
		}
	}

	private void DoAction_3000_3049(int action)
	{
		switch (action)
		{
		case 3003:
			if (value_stack.array[value_stack.top - 2].value is SqlCodeObject sqlCodeObject)
			{
				sqlCodeObject.Position = yyloc;
				yyval.value = sqlCodeObject;
			}
			break;
		case 3006:
		{
			SqlUdtInstanceMethodExpression sqlUdtInstanceMethodExpression = new SqlUdtInstanceMethodExpression(value_stack.array[value_stack.top - 6].value as SqlScalarExpression, value_stack.array[value_stack.top - 4].value as SqlIdentifier, value_stack.array[value_stack.top - 2].value as List<SqlScalarExpression>);
			sqlUdtInstanceMethodExpression.Position = yyloc;
			yyval.value = sqlUdtInstanceMethodExpression;
			break;
		}
		case 3007:
		{
			SqlUdtInstanceDataMemberExpression sqlUdtInstanceDataMemberExpression = new SqlUdtInstanceDataMemberExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlUdtInstanceDataMemberExpression.Position = yyloc;
			yyval.value = sqlUdtInstanceDataMemberExpression;
			break;
		}
		case 3008:
		{
			SqlUdtStaticMethodExpression sqlUdtStaticMethodExpression = new SqlUdtStaticMethodExpression(value_stack.array[value_stack.top - 6].value as SqlDataTypeSpecification, value_stack.array[value_stack.top - 4].value as SqlIdentifier, value_stack.array[value_stack.top - 2].value as List<SqlScalarExpression>);
			sqlUdtStaticMethodExpression.Position = yyloc;
			yyval.value = sqlUdtStaticMethodExpression;
			break;
		}
		case 3009:
		{
			SqlUdtStaticDataMemberExpression sqlUdtStaticDataMemberExpression = new SqlUdtStaticDataMemberExpression(value_stack.array[value_stack.top - 3].value as SqlDataTypeSpecification, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlUdtStaticDataMemberExpression.Position = yyloc;
			yyval.value = sqlUdtStaticDataMemberExpression;
			break;
		}
		case 3016:
		{
			SqlSimpleCaseExpression sqlSimpleCaseExpression = new SqlSimpleCaseExpression(value_stack.array[value_stack.top - 4].value as SqlScalarExpression, value_stack.array[value_stack.top - 3].value as List<SqlSimpleWhenClause>, value_stack.array[value_stack.top - 2].value as SqlScalarExpression);
			sqlSimpleCaseExpression.Position = yyloc;
			yyval.value = sqlSimpleCaseExpression;
			break;
		}
		case 3017:
		{
			SqlSearchedCaseExpression sqlSearchedCaseExpression = new SqlSearchedCaseExpression(value_stack.array[value_stack.top - 3].value as List<SqlSearchedWhenClause>, value_stack.array[value_stack.top - 2].value as SqlScalarExpression);
			sqlSearchedCaseExpression.Position = yyloc;
			yyval.value = sqlSearchedCaseExpression;
			break;
		}
		case 3020:
		{
			List<SqlSimpleWhenClause> list6 = new List<SqlSimpleWhenClause>();
			list6.Add(value_stack.array[value_stack.top - 1].value as SqlSimpleWhenClause);
			yyval.value = list6;
			break;
		}
		case 3021:
		{
			List<SqlSimpleWhenClause> list5 = value_stack.array[value_stack.top - 2].value as List<SqlSimpleWhenClause>;
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlSimpleWhenClause);
			yyval.value = list5;
			break;
		}
		case 3022:
		{
			SqlSimpleWhenClause sqlSimpleWhenClause = new SqlSimpleWhenClause(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlSimpleWhenClause.Position = yyloc;
			yyval.value = sqlSimpleWhenClause;
			break;
		}
		case 3023:
		{
			List<SqlSearchedWhenClause> list4 = new List<SqlSearchedWhenClause>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlSearchedWhenClause);
			yyval.value = list4;
			break;
		}
		case 3024:
		{
			List<SqlSearchedWhenClause> list3 = value_stack.array[value_stack.top - 2].value as List<SqlSearchedWhenClause>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlSearchedWhenClause);
			yyval.value = list3;
			break;
		}
		case 3025:
		{
			SqlSearchedWhenClause sqlSearchedWhenClause = new SqlSearchedWhenClause(value_stack.array[value_stack.top - 3].value as SqlBooleanExpression, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlSearchedWhenClause.Position = yyloc;
			yyval.value = sqlSearchedWhenClause;
			break;
		}
		case 3027:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 3028:
		{
			SqlObjectIdentifier sqlObjectIdentifier6 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier6.Position = yyloc;
			yyval.value = sqlObjectIdentifier6;
			break;
		}
		case 3029:
		{
			List<SqlIdentifier> obj3 = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			obj3.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			SqlObjectIdentifier sqlObjectIdentifier5 = SqlObjectIdentifier.Create(obj3);
			sqlObjectIdentifier5.Position = yyloc;
			yyval.value = sqlObjectIdentifier5;
			break;
		}
		case 3031:
		{
			SqlIdentifier sqlIdentifier3 = SqlIdentifier.Missing();
			sqlIdentifier3.Position = yyloc;
			yyval.value = sqlIdentifier3;
			break;
		}
		case 3037:
		{
			SqlObjectIdentifier sqlObjectIdentifier4 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier4.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression3 = new SqlColumnRefExpression(sqlObjectIdentifier4);
			sqlColumnRefExpression3.Position = yyloc;
			yyval.value = sqlColumnRefExpression3;
			break;
		}
		case 3038:
		{
			List<SqlIdentifier> obj2 = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			obj2.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			SqlObjectIdentifier sqlObjectIdentifier3 = SqlObjectIdentifier.Create(obj2);
			sqlObjectIdentifier3.Position = yyloc;
			SqlScalarRefExpression sqlScalarRefExpression = SqlScalarRefExpression.ColumnOrPropertyRefExpression(sqlObjectIdentifier3);
			sqlScalarRefExpression.Position = yyloc;
			yyval.value = sqlScalarRefExpression;
			break;
		}
		case 3042:
		{
			SqlObjectIdentifier sqlObjectIdentifier2 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier2.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression2 = new SqlColumnRefExpression(sqlObjectIdentifier2);
			sqlColumnRefExpression2.Position = yyloc;
			yyval.value = sqlColumnRefExpression2;
			break;
		}
		case 3043:
		{
			List<SqlIdentifier> obj = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			obj.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(obj);
			sqlObjectIdentifier.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression = new SqlColumnRefExpression(sqlObjectIdentifier, discardColumnPrefix: true);
			sqlColumnRefExpression.Position = yyloc;
			yyval.value = sqlColumnRefExpression;
			break;
		}
		case 3044:
		{
			List<SqlIdentifier> list2 = new List<SqlIdentifier>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list2;
			break;
		}
		case 3045:
		{
			List<SqlIdentifier> list = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list;
			break;
		}
		case 3046:
		{
			SqlIdentifier sqlIdentifier2 = new SqlIdentifier(string.Empty);
			sqlIdentifier2.Position = yyloc;
			yyval.value = sqlIdentifier2;
			break;
		}
		case 3047:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 3048:
		{
			SqlIdentifier sqlIdentifier = SqlIdentifier.Missing();
			sqlIdentifier.Position = location_stack.array[location_stack.top - 2];
			yyval.value = sqlIdentifier;
			break;
		}
		case 3000:
		case 3001:
		case 3002:
		case 3004:
		case 3005:
		case 3010:
		case 3011:
		case 3012:
		case 3013:
		case 3014:
		case 3015:
		case 3018:
		case 3019:
		case 3026:
		case 3030:
		case 3032:
		case 3033:
		case 3034:
		case 3035:
		case 3036:
		case 3039:
		case 3040:
		case 3041:
		case 3049:
			break;
		}
	}

	private void DoAction_3050_3099(int action)
	{
		switch (action)
		{
		case 3050:
		{
			SqlTableRefExpression sqlTableRefExpression2 = new SqlTableRefExpression(value_stack.array[value_stack.top - 2].value as SqlObjectIdentifier);
			sqlTableRefExpression2.Position = location_stack.array[location_stack.top - 2];
			yyval.value = new DmlTarget(sqlTableRefExpression2, value_stack.array[value_stack.top - 1].value as List<SqlColumnRefExpression>);
			break;
		}
		case 3051:
		{
			SqlTableRefExpression sqlTableRefExpression = new SqlTableRefExpression(value_stack.array[value_stack.top - 4].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 2].value as List<SqlHint>);
			sqlTableRefExpression.Position = MergeLocationRange(location_stack.top - 4, location_stack.top - 2);
			yyval.value = new DmlTarget(sqlTableRefExpression, value_stack.array[value_stack.top - 1].value as List<SqlColumnRefExpression>);
			break;
		}
		case 3052:
		{
			SqlTableValuedFunctionRefExpression sqlTableValuedFunctionRefExpression = new SqlTableValuedFunctionRefExpression(value_stack.array[value_stack.top - 5].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>);
			sqlTableValuedFunctionRefExpression.Position = MergeLocationRange(location_stack.top - 5, location_stack.top - 2);
			yyval.value = new DmlTarget(sqlTableValuedFunctionRefExpression, value_stack.array[value_stack.top - 1].value as List<SqlColumnRefExpression>);
			break;
		}
		case 3053:
			yyval.value = new DmlTarget(value_stack.array[value_stack.top - 2].value as SqlTableExpression, value_stack.array[value_stack.top - 1].value as List<SqlColumnRefExpression>);
			break;
		case 3054:
		{
			SqlTableVariableRefExpression sqlTableVariableRefExpression = new SqlTableVariableRefExpression(GetText(location_stack.array[location_stack.top - 2]));
			sqlTableVariableRefExpression.Position = location_stack.array[location_stack.top - 2];
			yyval.value = new DmlTarget(sqlTableVariableRefExpression, value_stack.array[value_stack.top - 1].value as List<SqlColumnRefExpression>);
			break;
		}
		case 3055:
		{
			SqlTableExpressionError sqlTableExpressionError = new SqlTableExpressionError(value_stack.array[value_stack.top - 2].value as SqlErrorCodeObject);
			sqlTableExpressionError.Position = location_stack.array[location_stack.top - 2];
			yyval.value = new DmlTarget(sqlTableExpressionError, value_stack.array[value_stack.top - 1].value as List<SqlColumnRefExpression>);
			break;
		}
		case 3064:
		{
			SqlObjectReference sqlObjectReference2 = new SqlObjectReference(value_stack.array[value_stack.top - 1].value as SqlObjectIdentifier);
			sqlObjectReference2.Position = yyloc;
			yyval.value = sqlObjectReference2;
			break;
		}
		case 3065:
		{
			SqlObjectReference sqlObjectReference = new SqlObjectReference(value_stack.array[value_stack.top - 1].value as SqlScalarVariableRefExpression);
			sqlObjectReference.Position = yyloc;
			yyval.value = sqlObjectReference;
			break;
		}
		case 3068:
		{
			SqlLiteralExpression sqlLiteralExpression11 = SqlLiteralExpression.String(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression11.Position = yyloc;
			yyval.value = sqlLiteralExpression11;
			break;
		}
		case 3070:
		{
			SqlLiteralExpression sqlLiteralExpression10 = SqlLiteralExpression.String(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression10.Position = yyloc;
			yyval.value = sqlLiteralExpression10;
			break;
		}
		case 3071:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 3072:
		{
			SqlScalarExpressionError sqlScalarExpressionError = new SqlScalarExpressionError();
			sqlScalarExpressionError.Position = yyloc;
			yyval.value = sqlScalarExpressionError;
			break;
		}
		case 3077:
		{
			SqlLiteralExpression sqlLiteralExpression9 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression9.Position = yyloc;
			yyval.value = sqlLiteralExpression9;
			break;
		}
		case 3078:
			VersionedParserBase.ReportError(SR.IntegerValueOutOfRange(GetText(location_stack.array[location_stack.top - 1])), location_stack.array[location_stack.top - 1]);
			break;
		case 3079:
		{
			SqlLiteralExpression sqlLiteralExpression8 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression8.Position = yyloc;
			yyval.value = sqlLiteralExpression8;
			break;
		}
		case 3080:
			VersionedParserBase.ReportError(SR.IntegerValueOutOfRange(GetText(location_stack.array[location_stack.top - 1])), location_stack.array[location_stack.top - 1]);
			break;
		case 3081:
		{
			SqlLiteralExpression sqlLiteralExpression7 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]), isNegative: true);
			sqlLiteralExpression7.Position = yyloc;
			yyval.value = sqlLiteralExpression7;
			break;
		}
		case 3082:
			if (IsMaxAbsoluteIntegerValue(GetText(location_stack.array[location_stack.top - 1])))
			{
				SqlLiteralExpression sqlLiteralExpression6 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]), isNegative: true);
				sqlLiteralExpression6.Position = yyloc;
				yyval.value = sqlLiteralExpression6;
			}
			else
			{
				VersionedParserBase.ReportError(SR.IntegerValueOutOfRange(GetText(location_stack.array[location_stack.top - 1])), location_stack.array[location_stack.top - 1]);
			}
			break;
		case 3087:
		{
			SqlLiteralExpression sqlLiteralExpression5 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression5.Position = yyloc;
			yyval.value = sqlLiteralExpression5;
			break;
		}
		case 3088:
		{
			SqlLiteralExpression sqlLiteralExpression4 = SqlLiteralExpression.Real(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression4.Position = yyloc;
			yyval.value = sqlLiteralExpression4;
			break;
		}
		case 3089:
		{
			SqlLiteralExpression sqlLiteralExpression3 = SqlLiteralExpression.Money(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression3.Position = yyloc;
			yyval.value = sqlLiteralExpression3;
			break;
		}
		case 3090:
		{
			SqlLiteralExpression sqlLiteralExpression2 = SqlLiteralExpression.Numeric(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression2.Position = yyloc;
			yyval.value = sqlLiteralExpression2;
			break;
		}
		case 3091:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Binary(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression.Position = yyloc;
			yyval.value = sqlLiteralExpression;
			break;
		}
		case 3056:
		case 3057:
		case 3058:
		case 3059:
		case 3060:
		case 3061:
		case 3062:
		case 3063:
		case 3066:
		case 3067:
		case 3069:
		case 3073:
		case 3074:
		case 3075:
		case 3076:
		case 3083:
		case 3084:
		case 3085:
		case 3086:
		case 3092:
		case 3093:
		case 3094:
		case 3095:
		case 3096:
		case 3097:
		case 3098:
		case 3099:
			break;
		}
	}

	private void DoAction_3100_3149(int action)
	{
		switch (action)
		{
		case 3101:
		{
			SqlLiteralExpression sqlLiteralExpression7 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]), isNegative: true);
			sqlLiteralExpression7.Position = yyloc;
			yyval.value = sqlLiteralExpression7;
			break;
		}
		case 3102:
		{
			SqlLiteralExpression sqlLiteralExpression6 = SqlLiteralExpression.Real(GetText(location_stack.array[location_stack.top - 1]), isNegative: true);
			sqlLiteralExpression6.Position = yyloc;
			yyval.value = sqlLiteralExpression6;
			break;
		}
		case 3103:
		{
			SqlLiteralExpression sqlLiteralExpression5 = SqlLiteralExpression.Money(GetText(location_stack.array[location_stack.top - 1]), isNegative: true);
			sqlLiteralExpression5.Position = yyloc;
			yyval.value = sqlLiteralExpression5;
			break;
		}
		case 3104:
		{
			SqlLiteralExpression sqlLiteralExpression4 = SqlLiteralExpression.Numeric(GetText(location_stack.array[location_stack.top - 1]), isNegative: true);
			sqlLiteralExpression4.Position = yyloc;
			yyval.value = sqlLiteralExpression4;
			break;
		}
		case 3107:
		{
			SqlLiteralExpression sqlLiteralExpression3 = SqlLiteralExpression.Identifier(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlLiteralExpression3.Position = yyloc;
			yyval.value = sqlLiteralExpression3;
			break;
		}
		case 3108:
		{
			SqlScalarExpressionError sqlScalarExpressionError = new SqlScalarExpressionError();
			sqlScalarExpressionError.Position = yyloc;
			yyval.value = sqlScalarExpressionError;
			break;
		}
		case 3111:
		{
			SqlLiteralExpression sqlLiteralExpression2 = SqlLiteralExpression.Default(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression2.Position = yyloc;
			yyval.value = sqlLiteralExpression2;
			break;
		}
		case 3112:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Null(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression.Position = yyloc;
			yyval.value = sqlLiteralExpression;
			break;
		}
		case 3113:
		{
			SqlScalarVariableRefExpression sqlScalarVariableRefExpression = SqlScalarVariableRefExpression.Create(GetText(location_stack.array[location_stack.top - 1]));
			sqlScalarVariableRefExpression.Position = yyloc;
			yyval.value = sqlScalarVariableRefExpression;
			break;
		}
		case 3114:
		{
			SqlGlobalScalarVariableRefExpression sqlGlobalScalarVariableRefExpression = new SqlGlobalScalarVariableRefExpression(GetText(location_stack.array[location_stack.top - 1]));
			sqlGlobalScalarVariableRefExpression.Position = yyloc;
			yyval.value = sqlGlobalScalarVariableRefExpression;
			break;
		}
		case 3142:
		{
			SqlBuiltinScalarFunctionCallExpression sqlBuiltinScalarFunctionCallExpression3 = new SqlBuiltinScalarFunctionCallExpression(GetText(location_stack.array[location_stack.top - 4]), value_stack.array[value_stack.top - 2].value as List<SqlScalarExpression>);
			sqlBuiltinScalarFunctionCallExpression3.Position = yyloc;
			yyval.value = sqlBuiltinScalarFunctionCallExpression3;
			break;
		}
		case 3143:
		{
			SqlBuiltinScalarFunctionCallExpression sqlBuiltinScalarFunctionCallExpression2 = new SqlBuiltinScalarFunctionCallExpression(GetText(location_stack.array[location_stack.top - 4]), value_stack.array[value_stack.top - 2].value as List<SqlScalarExpression>);
			sqlBuiltinScalarFunctionCallExpression2.Position = yyloc;
			yyval.value = sqlBuiltinScalarFunctionCallExpression2;
			break;
		}
		case 3144:
		{
			SqlBuiltinScalarFunctionCallExpression sqlBuiltinScalarFunctionCallExpression = SqlBuiltinScalarFunctionCallExpression.Create(value_stack.array[value_stack.top - 5].value as string, value_stack.array[value_stack.top - 4].value as List<SqlScalarExpression>);
			sqlBuiltinScalarFunctionCallExpression.Position = yyloc;
			yyval.value = sqlBuiltinScalarFunctionCallExpression;
			break;
		}
		case 3147:
		{
			SqlAggregateFunctionCallExpression sqlAggregateFunctionCallExpression = SqlAggregateFunctionCallExpression.Create(GetText(location_stack.array[location_stack.top - 6]), (SqlSetQuantifier)value_stack.array[value_stack.top - 4].value, value_stack.array[value_stack.top - 3].value as SqlScalarExpression);
			sqlAggregateFunctionCallExpression.Position = yyloc;
			yyval.value = sqlAggregateFunctionCallExpression;
			break;
		}
		case 3149:
		{
			SqlCastExpression sqlCastExpression = new SqlCastExpression(value_stack.array[value_stack.top - 6].value as string, value_stack.array[value_stack.top - 3].value as SqlDataTypeSpecification, value_stack.array[value_stack.top - 5].value as SqlScalarExpression);
			sqlCastExpression.Position = yyloc;
			yyval.value = sqlCastExpression;
			scanner.ClearQueryStatus(QueryStatus.Q_FN_PARSE);
			break;
		}
		case 3100:
		case 3105:
		case 3106:
		case 3109:
		case 3110:
		case 3115:
		case 3116:
		case 3117:
		case 3118:
		case 3119:
		case 3120:
		case 3121:
		case 3122:
		case 3123:
		case 3124:
		case 3125:
		case 3126:
		case 3127:
		case 3128:
		case 3129:
		case 3130:
		case 3131:
		case 3132:
		case 3133:
		case 3134:
		case 3135:
		case 3136:
		case 3137:
		case 3138:
		case 3139:
		case 3140:
		case 3141:
		case 3145:
		case 3146:
		case 3148:
			break;
		}
	}

	private void DoAction_3150_3199(int action)
	{
		switch (action)
		{
		case 3150:
		{
			SqlCodeObject sqlCodeObject = SqlBuiltinScalarFunctionCallExpression.CreateStar(GetText(location_stack.array[location_stack.top - 5]));
			if (sqlCodeObject == null)
			{
				sqlCodeObject = new SqlScalarExpressionError(new SqlErrorCodeObject(null));
				VersionedParserBase.SyntaxError("*", location_stack.array[location_stack.top - 3]);
			}
			sqlCodeObject.Position = yyloc;
			yyval.value = sqlCodeObject;
			break;
		}
		case 3151:
		{
			SqlBuiltinScalarFunctionCallExpression sqlBuiltinScalarFunctionCallExpression5 = new SqlBuiltinScalarFunctionCallExpression(GetText(location_stack.array[location_stack.top - 1]));
			sqlBuiltinScalarFunctionCallExpression5.Position = yyloc;
			yyval.value = sqlBuiltinScalarFunctionCallExpression5;
			break;
		}
		case 3152:
		{
			SqlBuiltinScalarFunctionCallExpression sqlBuiltinScalarFunctionCallExpression4 = new SqlBuiltinScalarFunctionCallExpression(GetText(location_stack.array[location_stack.top - 1]));
			sqlBuiltinScalarFunctionCallExpression4.Position = yyloc;
			yyval.value = sqlBuiltinScalarFunctionCallExpression4;
			break;
		}
		case 3153:
		{
			SqlBuiltinScalarFunctionCallExpression sqlBuiltinScalarFunctionCallExpression3 = new SqlBuiltinScalarFunctionCallExpression(GetText(location_stack.array[location_stack.top - 1]));
			sqlBuiltinScalarFunctionCallExpression3.Position = yyloc;
			yyval.value = sqlBuiltinScalarFunctionCallExpression3;
			break;
		}
		case 3154:
		{
			SqlBuiltinScalarFunctionCallExpression sqlBuiltinScalarFunctionCallExpression2 = new SqlBuiltinScalarFunctionCallExpression(GetText(location_stack.array[location_stack.top - 1]));
			sqlBuiltinScalarFunctionCallExpression2.Position = yyloc;
			yyval.value = sqlBuiltinScalarFunctionCallExpression2;
			break;
		}
		case 3155:
		{
			SqlBuiltinScalarFunctionCallExpression sqlBuiltinScalarFunctionCallExpression = new SqlBuiltinScalarFunctionCallExpression(GetText(location_stack.array[location_stack.top - 1]));
			sqlBuiltinScalarFunctionCallExpression.Position = yyloc;
			yyval.value = sqlBuiltinScalarFunctionCallExpression;
			break;
		}
		case 3157:
		{
			SqlConvertExpression sqlConvertExpression2 = new SqlConvertExpression(GetText(location_stack.array[location_stack.top - 7]), value_stack.array[value_stack.top - 5].value as SqlDataTypeSpecification, value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 2].value as SqlScalarExpression);
			sqlConvertExpression2.Position = yyloc;
			yyval.value = sqlConvertExpression2;
			break;
		}
		case 3158:
		{
			SqlConvertExpression sqlConvertExpression = new SqlConvertExpression(GetText(location_stack.array[location_stack.top - 7]), value_stack.array[value_stack.top - 5].value as SqlDataTypeSpecification, value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 2].value as SqlScalarExpression);
			sqlConvertExpression.Position = yyloc;
			yyval.value = sqlConvertExpression;
			break;
		}
		case 3159:
			yyval.value = GetText(location_stack.array[location_stack.top - 2]);
			break;
		case 3181:
		{
			List<SqlIdentifier> obj = value_stack.array[value_stack.top - 6].value as List<SqlIdentifier>;
			obj.Add(value_stack.array[value_stack.top - 5].value as SqlIdentifier);
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(obj);
			sqlObjectIdentifier.Position = LexLocation.Merge(location_stack.array[location_stack.top - 6], location_stack.array[location_stack.top - 5]);
			SqlUserDefinedScalarFunctionCallExpression sqlUserDefinedScalarFunctionCallExpression = new SqlUserDefinedScalarFunctionCallExpression(sqlObjectIdentifier, value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>);
			sqlUserDefinedScalarFunctionCallExpression.Position = yyloc;
			yyval.value = sqlUserDefinedScalarFunctionCallExpression;
			break;
		}
		case 3184:
		{
			List<SqlScalarExpression> list4 = new List<SqlScalarExpression>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list4;
			break;
		}
		case 3185:
		{
			List<SqlScalarExpression> list3 = value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list3;
			break;
		}
		case 3188:
		{
			List<SqlScalarExpression> list2 = new List<SqlScalarExpression>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list2;
			break;
		}
		case 3189:
		{
			List<SqlScalarExpression> list = value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list;
			break;
		}
		case 3191:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 3194:
		{
			SqlDataTypeSpecification sqlDataTypeSpecification4 = new SqlDataTypeSpecification(value_stack.array[value_stack.top - 1].value as SqlDataType);
			sqlDataTypeSpecification4.Position = yyloc;
			yyval.value = sqlDataTypeSpecification4;
			break;
		}
		case 3195:
		{
			SqlLiteralExpression sqlLiteralExpression3 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 2]));
			sqlLiteralExpression3.Position = location_stack.array[location_stack.top - 2];
			SqlDataTypeSpecification sqlDataTypeSpecification3 = new SqlDataTypeSpecification(value_stack.array[value_stack.top - 4].value as SqlDataType, sqlLiteralExpression3);
			sqlDataTypeSpecification3.Position = yyloc;
			yyval.value = sqlDataTypeSpecification3;
			break;
		}
		case 3196:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 4]));
			sqlLiteralExpression.Position = location_stack.array[location_stack.top - 4];
			SqlLiteralExpression sqlLiteralExpression2 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 2]));
			sqlLiteralExpression2.Position = location_stack.array[location_stack.top - 2];
			SqlDataTypeSpecification sqlDataTypeSpecification2 = new SqlDataTypeSpecification(value_stack.array[value_stack.top - 6].value as SqlDataType, sqlLiteralExpression, sqlLiteralExpression2);
			sqlDataTypeSpecification2.Position = yyloc;
			yyval.value = sqlDataTypeSpecification2;
			break;
		}
		case 3197:
		{
			SqlDataTypeSpecification sqlDataTypeSpecification = new SqlDataTypeSpecification(value_stack.array[value_stack.top - 5].value as SqlDataType, value_stack.array[value_stack.top - 3].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 2].value as SqlObjectIdentifier);
			sqlDataTypeSpecification.Position = yyloc;
			yyval.value = sqlDataTypeSpecification;
			break;
		}
		case 3198:
			yyval.value = SqlSetQuantifier.All;
			break;
		case 3199:
			yyval.value = SqlSetQuantifier.Distinct;
			break;
		case 3156:
		case 3160:
		case 3161:
		case 3162:
		case 3163:
		case 3164:
		case 3165:
		case 3166:
		case 3167:
		case 3168:
		case 3169:
		case 3170:
		case 3171:
		case 3172:
		case 3173:
		case 3174:
		case 3175:
		case 3176:
		case 3177:
		case 3178:
		case 3179:
		case 3180:
		case 3182:
		case 3183:
		case 3186:
		case 3187:
		case 3190:
		case 3192:
		case 3193:
			break;
		}
	}

	private void DoAction_3200_3249(int action)
	{
		switch (action)
		{
		case 3200:
		{
			SqlUpdateBooleanExpression sqlUpdateBooleanExpression = new SqlUpdateBooleanExpression(value_stack.array[value_stack.top - 2].value as SqlIdentifier);
			sqlUpdateBooleanExpression.Position = yyloc;
			yyval.value = sqlUpdateBooleanExpression;
			break;
		}
		case 3201:
		case 3202:
		case 3203:
		case 3204:
		case 3205:
		case 3206:
		case 3207:
		case 3208:
		case 3209:
		case 3210:
		case 3211:
		case 3212:
		case 3213:
		case 3214:
		case 3215:
		case 3216:
		case 3217:
		case 3218:
		case 3219:
		case 3220:
		case 3221:
		case 3222:
		case 3223:
		case 3224:
		case 3225:
		case 3226:
		case 3227:
		case 3228:
		case 3229:
		case 3230:
		case 3231:
		case 3232:
		case 3233:
		case 3234:
		case 3235:
		case 3236:
		case 3237:
		case 3238:
		case 3239:
		case 3240:
		case 3241:
		case 3242:
		case 3243:
		case 3244:
		case 3245:
		case 3246:
		case 3247:
		case 3248:
		case 3249:
			break;
		}
	}

	private void DoAction_3250_3299(int action)
	{
		switch (action)
		{
		case 3279:
		{
			SqlObjectIdentifier sqlObjectIdentifier4 = SqlObjectIdentifier.Create(new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]))
			{
				Position = yyloc
			});
			sqlObjectIdentifier4.Position = yyloc;
			yyval.value = sqlObjectIdentifier4;
			break;
		}
		case 3280:
		{
			SqlObjectIdentifier sqlObjectIdentifier3 = SqlObjectIdentifier.Create(new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]))
			{
				Position = yyloc
			});
			sqlObjectIdentifier3.Position = yyloc;
			yyval.value = sqlObjectIdentifier3;
			break;
		}
		case 3281:
		{
			SqlObjectIdentifier sqlObjectIdentifier2 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier2.Position = yyloc;
			yyval.value = sqlObjectIdentifier2;
			break;
		}
		case 3282:
		{
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 5].value as SqlIdentifier, value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier.Position = yyloc;
			yyval.value = sqlObjectIdentifier;
			break;
		}
		case 3287:
		{
			SqlLiteralExpression sqlLiteralExpression2 = SqlLiteralExpression.String(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression2.Position = yyloc;
			yyval.value = sqlLiteralExpression2;
			break;
		}
		case 3288:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.String(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression.Position = yyloc;
			yyval.value = sqlLiteralExpression;
			break;
		}
		case 3250:
		case 3251:
		case 3252:
		case 3253:
		case 3254:
		case 3255:
		case 3256:
		case 3257:
		case 3258:
		case 3259:
		case 3260:
		case 3261:
		case 3262:
		case 3263:
		case 3264:
		case 3265:
		case 3266:
		case 3267:
		case 3268:
		case 3269:
		case 3270:
		case 3271:
		case 3272:
		case 3273:
		case 3274:
		case 3275:
		case 3276:
		case 3277:
		case 3278:
		case 3283:
		case 3284:
		case 3285:
		case 3286:
		case 3289:
		case 3290:
		case 3291:
		case 3292:
		case 3293:
		case 3294:
		case 3295:
		case 3296:
		case 3297:
		case 3298:
		case 3299:
			break;
		}
	}

	private void DoAction_3300_3349(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_3350_3399(int action)
	{
		switch (action)
		{
		case 3362:
			scanner.SetQueryStatus(QueryStatus.P_BREAKDECPOINT);
			break;
		case 3363:
			scanner.ClearQueryStatus(QueryStatus.P_BREAKDECPOINT);
			break;
		case 3389:
		{
			SqlCreateSynonymStatement sqlCreateSynonymStatement = new SqlCreateSynonymStatement(value_stack.array[value_stack.top - 3].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 1].value as SqlObjectIdentifier);
			sqlCreateSynonymStatement.Position = yyloc;
			yyval.value = sqlCreateSynonymStatement;
			break;
		}
		case 3350:
		case 3351:
		case 3352:
		case 3353:
		case 3354:
		case 3355:
		case 3356:
		case 3357:
		case 3358:
		case 3359:
		case 3360:
		case 3361:
		case 3364:
		case 3365:
		case 3366:
		case 3367:
		case 3368:
		case 3369:
		case 3370:
		case 3371:
		case 3372:
		case 3373:
		case 3374:
		case 3375:
		case 3376:
		case 3377:
		case 3378:
		case 3379:
		case 3380:
		case 3381:
		case 3382:
		case 3383:
		case 3384:
		case 3385:
		case 3386:
		case 3387:
		case 3388:
		case 3390:
		case 3391:
		case 3392:
		case 3393:
		case 3394:
		case 3395:
		case 3396:
		case 3397:
		case 3398:
		case 3399:
			break;
		}
	}

	private void DoAction_3400_3449(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_3450_3499(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_3500_3549(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_3550_3599(int action)
	{
		switch (action)
		{
		case 3572:
			scanner.SetQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3573:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3582:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3583:
			scanner.SetQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3584:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3585:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3550:
		case 3551:
		case 3552:
		case 3553:
		case 3554:
		case 3555:
		case 3556:
		case 3557:
		case 3558:
		case 3559:
		case 3560:
		case 3561:
		case 3562:
		case 3563:
		case 3564:
		case 3565:
		case 3566:
		case 3567:
		case 3568:
		case 3569:
		case 3570:
		case 3571:
		case 3574:
		case 3575:
		case 3576:
		case 3577:
		case 3578:
		case 3579:
		case 3580:
		case 3581:
		case 3586:
		case 3587:
		case 3588:
		case 3589:
		case 3590:
		case 3591:
		case 3592:
		case 3593:
		case 3594:
		case 3595:
		case 3596:
		case 3597:
		case 3598:
		case 3599:
			break;
		}
	}

	private void DoAction_3600_3649(int action)
	{
		switch (action)
		{
		case 3611:
			scanner.SetGDRState();
			break;
		case 3612:
			scanner.SetGDRState();
			break;
		case 3613:
			scanner.SetGDRState();
			break;
		case 3600:
		case 3601:
		case 3602:
		case 3603:
		case 3604:
		case 3605:
		case 3606:
		case 3607:
		case 3608:
		case 3609:
		case 3610:
		case 3614:
		case 3615:
		case 3616:
		case 3617:
		case 3618:
		case 3619:
		case 3620:
		case 3621:
		case 3622:
		case 3623:
		case 3624:
		case 3625:
		case 3626:
		case 3627:
		case 3628:
		case 3629:
		case 3630:
		case 3631:
		case 3632:
		case 3633:
		case 3634:
		case 3635:
		case 3636:
		case 3637:
		case 3638:
		case 3639:
		case 3640:
		case 3641:
		case 3642:
		case 3643:
		case 3644:
		case 3645:
		case 3646:
		case 3647:
		case 3648:
		case 3649:
			break;
		}
	}

	private void DoAction_3650_3699(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_3700_3749(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_3750_3799(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_3800_3849(int action)
	{
		switch (action)
		{
		case 3807:
			yyval.value = false;
			break;
		case 3808:
			yyval.value = true;
			break;
		case 3838:
		{
			SqlExternalStreamDefinition sqlExternalStreamDefinition2 = new SqlExternalStreamDefinition(value_stack.array[value_stack.top - 2].value as List<SqlExternalStreamParam>);
			if (!sqlExternalStreamDefinition2.CheckRequiredParamsExist(out var requiredParam2))
			{
				VersionedParserBase.ReportError(SR.RequiredParam(SqlExternalStreamParam.ToString(requiredParam2)), location_stack.array[location_stack.top - 2]);
			}
			if (sqlExternalStreamDefinition2.CheckDuplicateParamExist(out requiredParam2))
			{
				VersionedParserBase.ReportError(SR.DuplicateParam(SqlExternalStreamParam.ToString(requiredParam2)), location_stack.array[location_stack.top - 2]);
			}
			break;
		}
		case 3839:
		{
			List<SqlExternalStreamParam> streamParam = value_stack.array[value_stack.top - 2].value as List<SqlExternalStreamParam>;
			SqlExternalStreamDefinition sqlExternalStreamDefinition = new SqlExternalStreamDefinition(value_stack.array[value_stack.top - 5].value as SqlTableDefinition, streamParam);
			if (!sqlExternalStreamDefinition.CheckRequiredParamsExist(out var requiredParam))
			{
				VersionedParserBase.ReportError(SR.RequiredParam(SqlExternalStreamParam.ToString(requiredParam)), location_stack.array[location_stack.top - 2]);
			}
			if (sqlExternalStreamDefinition.CheckDuplicateParamExist(out requiredParam))
			{
				VersionedParserBase.ReportError(SR.DuplicateParam(SqlExternalStreamParam.ToString(requiredParam)), location_stack.array[location_stack.top - 2]);
			}
			break;
		}
		case 3840:
		{
			List<SqlExternalStreamParam> list2 = new List<SqlExternalStreamParam>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlExternalStreamParam);
			yyval.value = list2;
			break;
		}
		case 3841:
		{
			List<SqlExternalStreamParam> list = value_stack.array[value_stack.top - 3].value as List<SqlExternalStreamParam>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlExternalStreamParam);
			yyval.value = list;
			break;
		}
		case 3842:
		{
			SqlIdentifier value6 = value_stack.array[value_stack.top - 1].value as SqlIdentifier;
			SqlExternalStreamParam value7 = new SqlExternalStreamParam(SqlExternalStreamParamType.DataSource, value6);
			yyval.value = value7;
			break;
		}
		case 3843:
		{
			SqlLiteralExpression sqlLiteralExpression3 = value_stack.array[value_stack.top - 1].value as SqlLiteralExpression;
			SqlExternalStreamParam value5 = new SqlExternalStreamParam(SqlExternalStreamParamType.Location, new SqlIdentifier(sqlLiteralExpression3.Value));
			yyval.value = value5;
			break;
		}
		case 3844:
		{
			SqlIdentifier value3 = value_stack.array[value_stack.top - 1].value as SqlIdentifier;
			SqlExternalStreamParam value4 = new SqlExternalStreamParam(SqlExternalStreamParamType.FileFormat, value3);
			yyval.value = value4;
			break;
		}
		case 3845:
		{
			SqlLiteralExpression sqlLiteralExpression2 = value_stack.array[value_stack.top - 1].value as SqlLiteralExpression;
			SqlExternalStreamParam value2 = new SqlExternalStreamParam(SqlExternalStreamParamType.InputOptions, new SqlIdentifier(sqlLiteralExpression2.Value));
			yyval.value = value2;
			break;
		}
		case 3846:
		{
			SqlLiteralExpression sqlLiteralExpression = value_stack.array[value_stack.top - 1].value as SqlLiteralExpression;
			SqlExternalStreamParam value = new SqlExternalStreamParam(SqlExternalStreamParamType.OutputOptions, new SqlIdentifier(sqlLiteralExpression.Value));
			yyval.value = value;
			break;
		}
		case 3800:
		case 3801:
		case 3802:
		case 3803:
		case 3804:
		case 3805:
		case 3806:
		case 3809:
		case 3810:
		case 3811:
		case 3812:
		case 3813:
		case 3814:
		case 3815:
		case 3816:
		case 3817:
		case 3818:
		case 3819:
		case 3820:
		case 3821:
		case 3822:
		case 3823:
		case 3824:
		case 3825:
		case 3826:
		case 3827:
		case 3828:
		case 3829:
		case 3830:
		case 3831:
		case 3832:
		case 3833:
		case 3834:
		case 3835:
		case 3836:
		case 3837:
		case 3847:
		case 3848:
		case 3849:
			break;
		}
	}

	private void DoAction_3850_3899(int action)
	{
		switch (action)
		{
		case 3852:
			yyval.value = null;
			break;
		case 3853:
			yyval.value = GetText(location_stack.array[location_stack.top - 1]);
			break;
		case 3861:
			yyval.value = SqlAssignmentOperatorType.None;
			break;
		case 3868:
		{
			SqlSetClause sqlSetClause = new SqlSetClause(value_stack.array[value_stack.top - 1].value as List<SqlAssignment>);
			sqlSetClause.Position = yyloc;
			yyval.value = sqlSetClause;
			break;
		}
		case 3869:
		{
			SqlSetClauseError sqlSetClauseError = new SqlSetClauseError(value_stack.array[value_stack.top - 1].value as List<SqlAssignment>);
			sqlSetClauseError.Position = yyloc;
			yyval.value = sqlSetClauseError;
			break;
		}
		case 3873:
		{
			SqlDataTypeSpecification sqlDataTypeSpecification = SqlDataTypeSpecification.Error();
			sqlDataTypeSpecification.Position = yyloc;
			yyval.value = sqlDataTypeSpecification;
			break;
		}
		case 3874:
			scanner.SetParseState(EParseState.e_ParseBegin);
			break;
		case 3875:
		{
			SqlCompoundStatement sqlCompoundStatement = new SqlCompoundStatement(value_stack.array[value_stack.top - 2].value as List<SqlStatement>);
			sqlCompoundStatement.Position = yyloc;
			yyval.value = sqlCompoundStatement;
			break;
		}
		case 3876:
		{
			SqlStatement sqlStatement12 = new SqlStatementError();
			sqlStatement12.Position = location_stack.array[location_stack.top - 1];
			SqlCompoundStatement sqlCompoundStatement2 = new SqlCompoundStatement(new List<SqlStatement> { sqlStatement12 });
			sqlCompoundStatement2.Position = yyloc;
			yyval.value = sqlCompoundStatement2;
			VersionedParserBase.SyntaxError("end", location_stack.array[location_stack.top - 1]);
			break;
		}
		case 3877:
		{
			SqlCompoundStatement sqlCompoundStatement3 = new SqlCompoundStatement(value_stack.array[value_stack.top - 2].value as List<SqlStatement>);
			sqlCompoundStatement3.Position = yyloc;
			yyval.value = sqlCompoundStatement3;
			break;
		}
		case 3878:
		{
			SqlStatement sqlStatement9 = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement9 == null)
			{
				sqlStatement9 = new SqlNullStatement();
			}
			sqlStatement9.Position = yyloc;
			yyval.value = sqlStatement9;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement9);
			break;
		}
		case 3879:
		{
			SqlStatement sqlStatement7 = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement7 == null)
			{
				sqlStatement7 = new SqlNullStatement();
			}
			sqlStatement7.Position = yyloc;
			yyval.value = sqlStatement7;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement7);
			break;
		}
		case 3880:
		{
			SqlStatement sqlStatement4 = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement4 == null)
			{
				sqlStatement4 = new SqlNullStatement();
			}
			sqlStatement4.Position = yyloc;
			yyval.value = sqlStatement4;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement4);
			break;
		}
		case 3881:
		{
			SqlStatement sqlStatement2 = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement2 == null)
			{
				sqlStatement2 = new SqlNullStatement();
			}
			sqlStatement2.Position = yyloc;
			yyval.value = sqlStatement2;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement2);
			break;
		}
		case 3882:
		{
			SqlStatement sqlStatement11 = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement11 == null)
			{
				sqlStatement11 = new SqlNullStatement();
			}
			sqlStatement11.Position = yyloc;
			yyval.value = sqlStatement11;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement11);
			break;
		}
		case 3883:
		{
			SqlStatement sqlStatement10 = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement10 == null)
			{
				sqlStatement10 = new SqlNullStatement();
			}
			sqlStatement10.Position = yyloc;
			yyval.value = sqlStatement10;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement10);
			break;
		}
		case 3884:
		{
			SqlStatement sqlStatement8 = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement8 == null)
			{
				sqlStatement8 = new SqlNullStatement();
			}
			sqlStatement8.Position = yyloc;
			yyval.value = sqlStatement8;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement8);
			break;
		}
		case 3885:
		{
			SqlStatement sqlStatement6 = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement6 == null)
			{
				sqlStatement6 = new SqlNullStatement();
			}
			sqlStatement6.Position = yyloc;
			yyval.value = sqlStatement6;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement6);
			break;
		}
		case 3886:
		{
			SqlStatement sqlStatement5 = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement5 == null)
			{
				sqlStatement5 = new SqlNullStatement();
			}
			sqlStatement5.Position = yyloc;
			yyval.value = sqlStatement5;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement5);
			break;
		}
		case 3887:
		{
			SqlStatement sqlStatement3 = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement3 == null)
			{
				sqlStatement3 = new SqlNullStatement();
			}
			sqlStatement3.Position = yyloc;
			yyval.value = sqlStatement3;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement3);
			break;
		}
		case 3888:
		{
			SqlStatement sqlStatement = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement == null)
			{
				sqlStatement = new SqlNullStatement();
			}
			sqlStatement.Position = yyloc;
			yyval.value = sqlStatement;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement);
			break;
		}
		case 3891:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			scanner.SetQueryStatus(QueryStatus.Q_CRDB);
			break;
		case 3892:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			scanner.SetQueryStatus(QueryStatus.Q_ALTDB);
			break;
		case 3893:
			scanner.SetQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 3894:
			scanner.SetQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 3895:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3896:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3897:
			scanner.SetGDRState();
			break;
		case 3898:
			scanner.SetParseState(EParseState.e_ParseAfterWith);
			break;
		case 3899:
			scanner.SetQueryStatus(QueryStatus.Q_MERGE);
			break;
		case 3850:
		case 3851:
		case 3854:
		case 3855:
		case 3856:
		case 3857:
		case 3858:
		case 3859:
		case 3860:
		case 3862:
		case 3863:
		case 3864:
		case 3865:
		case 3866:
		case 3867:
		case 3870:
		case 3871:
		case 3872:
		case 3889:
		case 3890:
			break;
		}
	}

	private void DoAction_3900_3907(int action)
	{
		switch (action)
		{
		case 3900:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3901:
		{
			int tokenId = 173;
			if (OptLookup.OptStmtStarters.TryGetTokenId(scanner.GetStringToken(), ref tokenId))
			{
				scanner.RepeatToken(tokenId);
			}
			else
			{
				scanner.RepeatToken();
			}
			break;
		}
		case 3902:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3903:
			scanner.SetQueryState(QueryState.IN_FROM_CLAUSE);
			break;
		case 3904:
			scanner.SetQueryState(QueryState.IN_WHERE_CLAUSE);
			break;
		case 3905:
			scanner.SetParseState(EParseState.e_ParseGroupBy);
			break;
		case 3906:
			scanner.SetParseState(EParseState.e_ParseNeutral);
			break;
		case 3907:
			scanner.SetQueryStatus(QueryStatus.Q_FN_PARSE);
			break;
		}
	}
}
