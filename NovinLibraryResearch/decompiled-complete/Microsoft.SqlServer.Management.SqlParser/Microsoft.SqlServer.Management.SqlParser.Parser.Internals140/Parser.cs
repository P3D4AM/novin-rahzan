using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Common;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals;
using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals140;

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
			DoAction_3700_3716(action);
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
		case 239:
		{
			SqlIdentifier sqlIdentifier5 = new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlIdentifier5.Position = yyloc;
			yyval.value = sqlIdentifier5;
			break;
		}
		case 240:
		{
			SqlIdentifier sqlIdentifier4 = new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlIdentifier4.Position = yyloc;
			yyval.value = sqlIdentifier4;
			break;
		}
		case 241:
		{
			SqlIdentifier sqlIdentifier3 = new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlIdentifier3.Position = yyloc;
			yyval.value = sqlIdentifier3;
			break;
		}
		case 242:
		{
			SqlIdentifier sqlIdentifier2 = new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlIdentifier2.Position = yyloc;
			yyval.value = sqlIdentifier2;
			break;
		}
		case 244:
			scanner.SetQueryStatus(QueryStatus.Q_ODBC_ESCAPE);
			break;
		case 245:
			scanner.ClearQueryStatus(QueryStatus.Q_ODBC_ESCAPE);
			break;
		case 247:
		{
			SqlUseStatement sqlUseStatement2 = new SqlUseStatement(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlUseStatement2.Position = yyloc;
			yyval.value = sqlUseStatement2;
			break;
		}
		case 248:
		{
			SqlIdentifier sqlIdentifier = SqlIdentifier.Missing();
			sqlIdentifier.Position = location_stack.array[location_stack.top - 1];
			SqlUseStatement sqlUseStatement = new SqlUseStatement(sqlIdentifier);
			sqlUseStatement.Position = yyloc;
			yyval.value = sqlUseStatement;
			break;
		}
		case 249:
			scanner.SetQueryStatus(QueryStatus.Q_DATABASE);
			break;
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
		case 243:
		case 246:
			break;
		}
	}

	private void DoAction_250_299(int action)
	{
		switch (action)
		{
		case 250:
			scanner.ClearQueryStatus(QueryStatus.Q_CRDB);
			break;
		case 251:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 252:
		case 253:
		case 254:
		case 255:
		case 256:
		case 257:
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
		case 319:
			scanner.SetQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 322:
			scanner.ClearQueryStatus(QueryStatus.Q_ALTDB);
			break;
		case 323:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 324:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 326:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
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
		case 320:
		case 321:
		case 325:
		case 327:
		case 328:
		case 329:
		case 330:
		case 331:
		case 332:
		case 333:
		case 334:
		case 335:
		case 336:
		case 337:
		case 338:
		case 339:
		case 340:
		case 341:
		case 342:
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
		case 546:
		{
			SqlTableDefinition sqlTableDefinition2 = new SqlTableDefinition(value_stack.array[value_stack.top - 7].value as List<ISqlTableElement>);
			sqlTableDefinition2.Position = location_stack.array[location_stack.top - 7];
			SqlCreateTableStatement sqlCreateTableStatement3 = new SqlCreateTableStatement(value_stack.array[value_stack.top - 9].value as SqlObjectIdentifier, sqlTableDefinition2, value_stack.array[value_stack.top - 3].value as SqlStorageSpecification, value_stack.array[value_stack.top - 2].value as SqlLargeDataStorageInformation);
			sqlCreateTableStatement3.Position = yyloc;
			yyval.value = sqlCreateTableStatement3;
			break;
		}
		case 547:
		{
			SqlTableDefinition sqlTableDefinition = new SqlTableDefinition(value_stack.array[value_stack.top - 7].value as List<ISqlTableElement>);
			sqlTableDefinition.Position = location_stack.array[location_stack.top - 7];
			SqlCreateTableStatement sqlCreateTableStatement2 = new SqlCreateTableStatement(value_stack.array[value_stack.top - 9].value as SqlObjectIdentifier, sqlTableDefinition, value_stack.array[value_stack.top - 3].value as SqlStorageSpecification, value_stack.array[value_stack.top - 2].value as SqlLargeDataStorageInformation);
			sqlCreateTableStatement2.Position = yyloc;
			yyval.value = sqlCreateTableStatement2;
			break;
		}
		case 548:
		{
			SqlCreateTableStatement sqlCreateTableStatement = new SqlCreateTableStatement(value_stack.array[value_stack.top - 6].value as SqlObjectIdentifier, new SqlTableDefinition(new List<ISqlTableElement>()), value_stack.array[value_stack.top - 3].value as SqlStorageSpecification, value_stack.array[value_stack.top - 2].value as SqlLargeDataStorageInformation);
			sqlCreateTableStatement.Position = yyloc;
			yyval.value = sqlCreateTableStatement;
			break;
		}
		case 500:
		case 501:
		case 502:
		case 503:
		case 504:
		case 505:
		case 506:
		case 507:
		case 508:
		case 509:
		case 510:
		case 511:
		case 512:
		case 513:
		case 514:
		case 515:
		case 516:
		case 517:
		case 518:
		case 519:
		case 520:
		case 521:
		case 522:
		case 523:
		case 524:
		case 525:
		case 526:
		case 527:
		case 528:
		case 529:
		case 530:
		case 531:
		case 532:
		case 533:
		case 534:
		case 535:
		case 536:
		case 537:
		case 538:
		case 539:
		case 540:
		case 541:
		case 542:
		case 543:
		case 544:
		case 545:
		case 549:
			break;
		}
	}

	private void DoAction_550_599(int action)
	{
		switch (action)
		{
		case 550:
		{
			SqlTableDefinition sqlTableDefinition2 = new SqlTableDefinition(value_stack.array[value_stack.top - 5].value as List<ISqlTableElement>);
			sqlTableDefinition2.Position = location_stack.array[location_stack.top - 5];
			SqlCreateTableStatement sqlCreateTableStatement2 = new SqlCreateTableStatement(value_stack.array[value_stack.top - 7].value as SqlObjectIdentifier, sqlTableDefinition2, value_stack.array[value_stack.top - 3].value as SqlStorageSpecification, value_stack.array[value_stack.top - 2].value as SqlLargeDataStorageInformation);
			sqlCreateTableStatement2.Position = yyloc;
			yyval.value = sqlCreateTableStatement2;
			break;
		}
		case 551:
		{
			SqlTableDefinition sqlTableDefinition = new SqlTableDefinition(value_stack.array[value_stack.top - 10].value as List<ISqlTableElement>);
			sqlTableDefinition.Position = location_stack.array[location_stack.top - 10];
			SqlCreateTableStatement sqlCreateTableStatement = new SqlCreateTableStatement(value_stack.array[value_stack.top - 12].value as SqlObjectIdentifier, sqlTableDefinition, value_stack.array[value_stack.top - 8].value as SqlStorageSpecification, value_stack.array[value_stack.top - 7].value as SqlLargeDataStorageInformation);
			sqlCreateTableStatement.Position = yyloc;
			yyval.value = sqlCreateTableStatement;
			break;
		}
		case 586:
		{
			List<ISqlTableElement> list2 = new List<ISqlTableElement>();
			list2.Add(value_stack.array[value_stack.top - 1].value as ISqlTableElement);
			yyval.value = list2;
			break;
		}
		case 587:
		{
			List<ISqlTableElement> list = value_stack.array[value_stack.top - 3].value as List<ISqlTableElement>;
			list.Add(value_stack.array[value_stack.top - 1].value as ISqlTableElement);
			yyval.value = list;
			break;
		}
		case 588:
		{
			SqlColumnDefinition sqlColumnDefinition4 = value_stack.array[value_stack.top - 2].value as SqlColumnDefinition;
			sqlColumnDefinition4.AddConstraints(value_stack.array[value_stack.top - 1].value as List<SqlConstraint>);
			sqlColumnDefinition4.Position = yyloc;
			yyval.value = sqlColumnDefinition4;
			break;
		}
		case 590:
		{
			SqlColumnDefinition sqlColumnDefinition3 = value_stack.array[value_stack.top - 2].value as SqlColumnDefinition;
			sqlColumnDefinition3.AddConstraints(value_stack.array[value_stack.top - 1].value as List<SqlConstraint>);
			sqlColumnDefinition3.Position = yyloc;
			yyval.value = sqlColumnDefinition3;
			break;
		}
		case 593:
		{
			SqlTemporalPeriodDefinition sqlTemporalPeriodDefinition = new SqlTemporalPeriodDefinition(value_stack.array[value_stack.top - 4].value as SqlIdentifier, value_stack.array[value_stack.top - 2].value as SqlIdentifier);
			sqlTemporalPeriodDefinition.Position = yyloc;
			yyval.value = sqlTemporalPeriodDefinition;
			ExpectKeyword(StringToken.Period, location_stack.array[location_stack.top - 8]);
			ExpectKeyword(StringToken.SystemTime, location_stack.array[location_stack.top - 6]);
			break;
		}
		case 597:
		{
			SqlColumnDefinition sqlColumnDefinition2 = new SqlColumnDefinition(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlColumnDefinition2.Position = yyloc;
			yyval.value = sqlColumnDefinition2;
			break;
		}
		case 599:
		{
			SqlDataTypeSpecification sqlDataTypeSpecification = new SqlDataTypeSpecification(value_stack.array[value_stack.top - 3].value as SqlDataType);
			sqlDataTypeSpecification.Position = location_stack.array[location_stack.top - 3];
			SqlColumnDefinition sqlColumnDefinition = new SqlColumnDefinition(value_stack.array[value_stack.top - 4].value as SqlIdentifier, sqlDataTypeSpecification, value_stack.array[value_stack.top - 2].value as SqlCollation, value_stack.array[value_stack.top - 1].value as SqlColumnAdditionalInfo);
			sqlColumnDefinition.Position = yyloc;
			yyval.value = sqlColumnDefinition;
			break;
		}
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
		case 589:
		case 591:
		case 592:
		case 594:
		case 595:
		case 596:
		case 598:
			break;
		}
	}

	private void DoAction_600_649(int action)
	{
		switch (action)
		{
		case 600:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 4]));
			sqlLiteralExpression.Position = location_stack.array[location_stack.top - 4];
			SqlDataTypeSpecification sqlDataTypeSpecification = new SqlDataTypeSpecification(value_stack.array[value_stack.top - 6].value as SqlDataType, sqlLiteralExpression);
			sqlDataTypeSpecification.Position = MergeLocationRange(location_stack.top - 6, location_stack.top - 3);
			SqlColumnDefinition sqlColumnDefinition = new SqlColumnDefinition(value_stack.array[value_stack.top - 7].value as SqlIdentifier, sqlDataTypeSpecification, value_stack.array[value_stack.top - 2].value as SqlCollation, value_stack.array[value_stack.top - 1].value as SqlColumnAdditionalInfo);
			sqlColumnDefinition.Position = yyloc;
			yyval.value = sqlColumnDefinition;
			break;
		}
		case 601:
		{
			SqlDataTypeSpecification sqlDataTypeSpecification3 = new SqlDataTypeSpecification(value_stack.array[value_stack.top - 7].value as SqlDataType, value_stack.array[value_stack.top - 5].value as SqlLiteralExpression, value_stack.array[value_stack.top - 3].value as SqlLiteralExpression);
			sqlDataTypeSpecification3.Position = MergeLocationRange(location_stack.top - 7, location_stack.top - 2);
			SqlColumnDefinition sqlColumnDefinition3 = new SqlColumnDefinition(value_stack.array[value_stack.top - 8].value as SqlIdentifier, sqlDataTypeSpecification3, value_stack.array[value_stack.top - 1].value as SqlColumnAdditionalInfo);
			sqlColumnDefinition3.Position = yyloc;
			yyval.value = sqlColumnDefinition3;
			break;
		}
		case 602:
		{
			SqlDataTypeSpecification sqlDataTypeSpecification2 = new SqlDataTypeSpecification(value_stack.array[value_stack.top - 7].value as SqlDataType, value_stack.array[value_stack.top - 5].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 4].value as SqlObjectIdentifier);
			sqlDataTypeSpecification2.Position = MergeLocationRange(location_stack.top - 7, location_stack.top - 3);
			SqlColumnDefinition sqlColumnDefinition2 = new SqlColumnDefinition(value_stack.array[value_stack.top - 8].value as SqlIdentifier, sqlDataTypeSpecification2, value_stack.array[value_stack.top - 2].value as SqlCollation);
			sqlColumnDefinition2.Position = yyloc;
			yyval.value = sqlColumnDefinition2;
			break;
		}
		case 603:
		{
			SqlDataType sqlDataType = new SqlDataType(value_stack.array[value_stack.top - 2].value as SqlObjectIdentifier, national: false, value_stack.array[value_stack.top - 1].value != null);
			sqlDataType.Position = yyloc;
			yyval.value = sqlDataType;
			if ((sqlDataType.TypeErrors & DataTypeErrors.VaryingNotAllowed) == DataTypeErrors.VaryingNotAllowed)
			{
				VersionedParserBase.SyntaxError("varying", location_stack.array[location_stack.top - 1]);
			}
			break;
		}
		case 604:
		{
			SqlDataType sqlDataType3 = new SqlDataType(value_stack.array[value_stack.top - 2].value as SqlObjectIdentifier, national: true, value_stack.array[value_stack.top - 1].value != null);
			sqlDataType3.Position = yyloc;
			yyval.value = sqlDataType3;
			if ((sqlDataType3.TypeErrors & DataTypeErrors.NationalNotAllowed) == DataTypeErrors.NationalNotAllowed)
			{
				VersionedParserBase.SyntaxError("national", location_stack.array[location_stack.top - 3]);
			}
			else if ((sqlDataType3.TypeErrors & DataTypeErrors.VaryingNotAllowed) == DataTypeErrors.VaryingNotAllowed)
			{
				VersionedParserBase.SyntaxError("varying", location_stack.array[location_stack.top - 1]);
			}
			break;
		}
		case 605:
		{
			SqlObjectIdentifier sqlObjectIdentifier4 = SqlObjectIdentifier.Create(new SqlIdentifier(GetText(location_stack.array[location_stack.top - 2]))
			{
				Position = location_stack.array[location_stack.top - 2]
			});
			sqlObjectIdentifier4.Position = location_stack.array[location_stack.top - 2];
			SqlDataType sqlDataType2 = SqlDataType.Double(sqlObjectIdentifier4);
			sqlDataType2.Position = yyloc;
			yyval.value = sqlDataType2;
			ExpectKeyword(StringToken.Precision, location_stack.array[location_stack.top - 1]);
			break;
		}
		case 606:
		{
			SqlObjectIdentifier sqlObjectIdentifier3 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier3.Position = yyloc;
			yyval.value = sqlObjectIdentifier3;
			break;
		}
		case 607:
		{
			SqlObjectIdentifier sqlObjectIdentifier2 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier2.Position = yyloc;
			yyval.value = sqlObjectIdentifier2;
			break;
		}
		case 608:
		{
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 5].value as SqlIdentifier, value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier.Position = yyloc;
			yyval.value = sqlObjectIdentifier;
			break;
		}
		case 609:
			yyval.value = SqlIdentifier.Null;
			break;
		case 614:
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
		case 633:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.None);
			break;
		case 634:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.Sparse);
			break;
		case 635:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.Sparse);
			break;
		case 636:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.None);
			break;
		case 637:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.Sparse);
			break;
		case 638:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.None);
			break;
		case 639:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.Sparse);
			break;
		case 640:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.ColumnSet);
			break;
		case 641:
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
		case 642:
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
		case 644:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.None);
			break;
		case 645:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.Sparse);
			break;
		case 646:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.Sparse);
			break;
		case 647:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.None);
			break;
		case 648:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.Sparse);
			break;
		case 649:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.None);
			break;
		case 610:
		case 611:
		case 612:
		case 613:
		case 615:
		case 616:
		case 617:
		case 618:
		case 619:
		case 620:
		case 621:
		case 622:
		case 623:
		case 624:
		case 625:
		case 626:
		case 627:
		case 628:
		case 629:
		case 630:
		case 631:
		case 632:
		case 643:
			break;
		}
	}

	private void DoAction_650_699(int action)
	{
		switch (action)
		{
		case 650:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.Sparse);
			break;
		case 651:
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
		case 652:
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
		case 653:
			yyval.value = new SqlColumnAdditionalInfo(SqlSparseOption.None);
			break;
		case 656:
			yyval.value = null;
			break;
		case 657:
			yyval.value = "varying";
			break;
		case 660:
		{
			List<SqlConstraint> list = new List<SqlConstraint>();
			list.Add(value_stack.array[value_stack.top - 1].value as SqlConstraint);
			yyval.value = list;
			break;
		}
		case 661:
		{
			List<SqlConstraint> list2 = value_stack.array[value_stack.top - 2].value as List<SqlConstraint>;
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlConstraint);
			yyval.value = list2;
			break;
		}
		case 663:
		{
			SqlConstraint sqlConstraint = value_stack.array[value_stack.top - 1].value as SqlConstraint;
			sqlConstraint.Name = value_stack.array[value_stack.top - 2].value as SqlIdentifier;
			sqlConstraint.Position = yyloc;
			yyval.value = sqlConstraint;
			break;
		}
		case 664:
			yyval.value = value_stack.array[value_stack.top - 1].value as ISqlTableElement;
			break;
		case 666:
		{
			SqlConstraint sqlConstraint2 = value_stack.array[value_stack.top - 1].value as SqlConstraint;
			sqlConstraint2.Name = value_stack.array[value_stack.top - 2].value as SqlIdentifier;
			sqlConstraint2.Position = yyloc;
			yyval.value = sqlConstraint2;
			break;
		}
		case 671:
		{
			SqlForeignKeyConstraint sqlForeignKeyConstraint = new SqlForeignKeyConstraint(value_stack.array[value_stack.top - 6].value as List<SqlIdentifier>, value_stack.array[value_stack.top - 4].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 3].value as List<SqlIdentifier>, (SqlForeignKeyAction)value_stack.array[value_stack.top - 2].value, (bool)value_stack.array[value_stack.top - 1].value);
			sqlForeignKeyConstraint.Position = yyloc;
			yyval.value = sqlForeignKeyConstraint;
			break;
		}
		case 672:
		{
			SqlDefaultConstraint sqlDefaultConstraint = new SqlDefaultConstraint(value_stack.array[value_stack.top - 3].value as SqlScalarExpression);
			sqlDefaultConstraint.Position = yyloc;
			yyval.value = sqlDefaultConstraint;
			break;
		}
		case 673:
		{
			SqlForeignKeyConstraint sqlForeignKeyConstraint2 = new SqlForeignKeyConstraint(null, value_stack.array[value_stack.top - 4].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 3].value as List<SqlIdentifier>, (SqlForeignKeyAction)value_stack.array[value_stack.top - 2].value, (bool)value_stack.array[value_stack.top - 1].value);
			sqlForeignKeyConstraint2.Position = yyloc;
			yyval.value = sqlForeignKeyConstraint2;
			break;
		}
		case 674:
		{
			SqlCheckConstraint sqlCheckConstraint = new SqlCheckConstraint((bool)value_stack.array[value_stack.top - 4].value, value_stack.array[value_stack.top - 2].value as SqlBooleanExpression);
			sqlCheckConstraint.Position = yyloc;
			yyval.value = sqlCheckConstraint;
			break;
		}
		case 675:
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
		case 677:
		{
			SqlConstraint sqlConstraint3 = SqlConstraint.RowGuidCol();
			sqlConstraint3.Position = yyloc;
			yyval.value = sqlConstraint3;
			break;
		}
		case 678:
			yyval.value = false;
			break;
		case 679:
			yyval.value = true;
			break;
		case 680:
			yyval.value = 512;
			break;
		case 681:
			yyval.value = 768;
			break;
		case 682:
			yyval.value = 1024;
			break;
		case 683:
		{
			bool flag2 = ExpectKeyword(StringToken.No, location_stack.array[location_stack.top - 2]) && ExpectKeyword(StringToken.Action, location_stack.array[location_stack.top - 1]);
			yyval.value = (flag2 ? 256 : 0);
			break;
		}
		case 684:
			yyval.value = SqlForeignKeyAction.Cascade;
			break;
		case 685:
			yyval.value = SqlForeignKeyAction.SetDefault;
			break;
		case 686:
			yyval.value = SqlForeignKeyAction.SetNull;
			break;
		case 687:
		{
			bool flag = ExpectKeyword(StringToken.No, location_stack.array[location_stack.top - 2]) && ExpectKeyword(StringToken.Action, location_stack.array[location_stack.top - 1]);
			yyval.value = (flag ? SqlForeignKeyAction.NoAction : SqlForeignKeyAction.None);
			break;
		}
		case 688:
			yyval.value = SqlForeignKeyAction.None;
			break;
		case 689:
			yyval.value = value_stack.array[value_stack.top - 1].value;
			break;
		case 690:
			yyval.value = value_stack.array[value_stack.top - 1].value;
			break;
		case 691:
			yyval.value = (int)value_stack.array[value_stack.top - 2].value + (int)value_stack.array[value_stack.top - 1].value;
			break;
		case 692:
			yyval.value = (int)value_stack.array[value_stack.top - 2].value + (int)value_stack.array[value_stack.top - 1].value;
			break;
		case 699:
		{
			SqlUniqueConstraintBase sqlUniqueConstraintBase = SqlUniqueConstraintBase.Create((SqlConstraintType)value_stack.array[value_stack.top - 6].value, (SqlClusterOption)value_stack.array[value_stack.top - 5].value, value_stack.array[value_stack.top - 4].value as List<SqlIndexedColumn>, value_stack.array[value_stack.top - 3].value as List<SqlIndexOption>);
			sqlUniqueConstraintBase.Position = yyloc;
			yyval.value = sqlUniqueConstraintBase;
			break;
		}
		case 654:
		case 655:
		case 658:
		case 659:
		case 662:
		case 665:
		case 667:
		case 668:
		case 669:
		case 670:
		case 676:
		case 693:
		case 694:
		case 695:
		case 696:
		case 697:
		case 698:
			break;
		}
	}

	private void DoAction_700_749(int action)
	{
		switch (action)
		{
		case 700:
		{
			SqlInlineIndexConstraint sqlInlineIndexConstraint3 = new SqlInlineIndexConstraint(value_stack.array[value_stack.top - 3].value as SqlIdentifier);
			sqlInlineIndexConstraint3.Position = yyloc;
			yyval.value = sqlInlineIndexConstraint3;
			break;
		}
		case 701:
		{
			SqlInlineIndexConstraint sqlInlineIndexConstraint2 = new SqlInlineIndexConstraint(value_stack.array[value_stack.top - 4].value as SqlIdentifier);
			sqlInlineIndexConstraint2.Position = yyloc;
			yyval.value = sqlInlineIndexConstraint2;
			break;
		}
		case 702:
		{
			SqlInlineIndexConstraint sqlInlineIndexConstraint = new SqlInlineIndexConstraint(value_stack.array[value_stack.top - 9].value as SqlIdentifier, value_stack.array[value_stack.top - 3].value as List<SqlIndexOption>);
			sqlInlineIndexConstraint.Position = yyloc;
			yyval.value = sqlInlineIndexConstraint;
			break;
		}
		case 703:
			yyval.value = SqlClusterOption.Default;
			break;
		case 704:
			yyval.value = SqlClusterOption.Clustered;
			break;
		case 705:
			yyval.value = SqlClusterOption.NonClustered;
			break;
		case 706:
			yyval.value = SqlClusterOption.NonClusteredHash;
			break;
		case 707:
			yyval.value = SqlClusterOption.Hash;
			break;
		case 708:
			yyval.value = SqlClusterOption.ClusteredColumnstore;
			break;
		case 709:
			yyval.value = SqlClusterOption.NonClusteredColumnstore;
			break;
		case 711:
			yyval.value = SqlConstraintType.Unique;
			break;
		case 712:
			yyval.value = SqlConstraintType.PrimaryKey;
			break;
		case 714:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 715:
		{
			List<SqlIdentifier> list4 = new List<SqlIdentifier>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list4;
			break;
		}
		case 716:
		{
			List<SqlIdentifier> list3 = value_stack.array[value_stack.top - 3].value as List<SqlIdentifier>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list3;
			break;
		}
		case 718:
		{
			SqlConstraint sqlConstraint2 = SqlConstraint.Null();
			sqlConstraint2.Position = yyloc;
			yyval.value = sqlConstraint2;
			break;
		}
		case 719:
		{
			SqlConstraint sqlConstraint = SqlConstraint.NotNull();
			sqlConstraint.Position = yyloc;
			yyval.value = sqlConstraint;
			break;
		}
		case 720:
		{
			SqlColumnIdentity sqlColumnIdentity2 = new SqlColumnIdentity();
			sqlColumnIdentity2.Position = yyloc;
			yyval.value = sqlColumnIdentity2;
			break;
		}
		case 721:
		{
			SqlColumnIdentity sqlColumnIdentity = new SqlColumnIdentity(value_stack.array[value_stack.top - 4].value as SqlLiteralExpression, value_stack.array[value_stack.top - 2].value as SqlLiteralExpression);
			sqlColumnIdentity.Position = yyloc;
			yyval.value = sqlColumnIdentity;
			break;
		}
		case 722:
		{
			SqlLiteralExpression sqlLiteralExpression2 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]), (bool)value_stack.array[value_stack.top - 2].value);
			sqlLiteralExpression2.Position = yyloc;
			yyval.value = sqlLiteralExpression2;
			break;
		}
		case 723:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Numeric(GetText(location_stack.array[location_stack.top - 1]), (bool)value_stack.array[value_stack.top - 2].value);
			sqlLiteralExpression.Position = yyloc;
			yyval.value = sqlLiteralExpression;
			break;
		}
		case 724:
			yyval.value = false;
			break;
		case 725:
			yyval.value = false;
			break;
		case 726:
			yyval.value = true;
			break;
		case 744:
			ExpectKeyword(StringToken.Partitions, location_stack.array[location_stack.top - 4]);
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 745:
		{
			List<SqlCompressionPartitionRange> list2 = new List<SqlCompressionPartitionRange>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlCompressionPartitionRange);
			yyval.value = list2;
			break;
		}
		case 746:
		{
			List<SqlCompressionPartitionRange> list = value_stack.array[value_stack.top - 3].value as List<SqlCompressionPartitionRange>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlCompressionPartitionRange);
			yyval.value = list;
			break;
		}
		case 747:
		{
			SqlCompressionPartitionRange sqlCompressionPartitionRange2 = new SqlCompressionPartitionRange(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlCompressionPartitionRange2.Position = yyloc;
			yyval.value = sqlCompressionPartitionRange2;
			break;
		}
		case 748:
		{
			SqlCompressionPartitionRange sqlCompressionPartitionRange = new SqlCompressionPartitionRange(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlCompressionPartitionRange.Position = yyloc;
			yyval.value = sqlCompressionPartitionRange;
			break;
		}
		case 710:
		case 713:
		case 717:
		case 727:
		case 728:
		case 729:
		case 730:
		case 731:
		case 732:
		case 733:
		case 734:
		case 735:
		case 736:
		case 737:
		case 738:
		case 739:
		case 740:
		case 741:
		case 742:
		case 743:
		case 749:
			break;
		}
	}

	private void DoAction_750_799(int action)
	{
		switch (action)
		{
		case 751:
		{
			List<SqlIdentifier> list2 = new List<SqlIdentifier>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list2;
			break;
		}
		case 752:
		{
			List<SqlIdentifier> list = value_stack.array[value_stack.top - 3].value as List<SqlIdentifier>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list;
			break;
		}
		case 760:
		{
			SqlComputedColumnDefinition sqlComputedColumnDefinition = new SqlComputedColumnDefinition(value_stack.array[value_stack.top - 4].value as SqlIdentifier, value_stack.array[value_stack.top - 2].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value != null);
			sqlComputedColumnDefinition.Position = yyloc;
			yyval.value = sqlComputedColumnDefinition;
			break;
		}
		case 762:
		{
			bool flag = ExpectKeyword(StringToken.Persisted, location_stack.array[location_stack.top - 1]);
			yyval.value = (flag ? "not null" : null);
			break;
		}
		case 763:
		{
			SqlInlineTableVariableDeclareStatement sqlInlineTableVariableDeclareStatement = new SqlInlineTableVariableDeclareStatement(new SqlInlineTableVariableDeclaration(GetText(location_stack.array[location_stack.top - 4]), value_stack.array[value_stack.top - 1].value as SqlTableDefinition)
			{
				Position = MergeLocationRange(location_stack.top - 4, location_stack.top - 1)
			});
			sqlInlineTableVariableDeclareStatement.Position = yyloc;
			yyval.value = sqlInlineTableVariableDeclareStatement;
			break;
		}
		case 764:
		{
			SqlTableDefinition sqlTableDefinition = new SqlTableDefinition(value_stack.array[value_stack.top - 2].value as List<ISqlTableElement>);
			sqlTableDefinition.Position = yyloc;
			yyval.value = sqlTableDefinition;
			break;
		}
		case 750:
		case 753:
		case 754:
		case 755:
		case 756:
		case 757:
		case 758:
		case 759:
		case 761:
		case 765:
		case 766:
		case 767:
		case 768:
		case 769:
		case 770:
		case 771:
		case 772:
		case 773:
		case 774:
		case 775:
		case 776:
		case 777:
		case 778:
		case 779:
		case 780:
		case 781:
		case 782:
		case 783:
		case 784:
		case 785:
		case 786:
		case 787:
		case 788:
		case 789:
		case 790:
		case 791:
		case 792:
		case 793:
		case 794:
		case 795:
		case 796:
		case 797:
		case 798:
		case 799:
			break;
		}
	}

	private void DoAction_800_849(int action)
	{
		switch (action)
		{
		case 814:
			yyval.value = new CreateIndexStart(isUnique: false, SqlClusterOption.Default);
			break;
		case 815:
			yyval.value = new CreateIndexStart(isUnique: true, (SqlClusterOption)value_stack.array[value_stack.top - 3].value);
			break;
		case 816:
			yyval.value = new CreateIndexStart(isUnique: false, SqlClusterOption.Clustered);
			break;
		case 817:
			yyval.value = new CreateIndexStart(isUnique: false, SqlClusterOption.NonClustered);
			break;
		case 818:
			yyval.value = new CreateIndexStart(isUnique: false, SqlClusterOption.Default);
			break;
		case 819:
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
		case 821:
		{
			SqlFilterClause sqlFilterClause = new SqlFilterClause(value_stack.array[value_stack.top - 1].value as SqlFilterExpression);
			sqlFilterClause.Position = yyloc;
			yyval.value = sqlFilterClause;
			break;
		}
		case 824:
			scanner.SetParseState(EParseState.e_ParseIdxInclude);
			break;
		case 825:
			yyval.value = false;
			break;
		case 826:
			yyval.value = true;
			break;
		case 827:
			yyval.value = SqlClusterOption.Default;
			break;
		case 828:
			yyval.value = SqlClusterOption.NonClustered;
			break;
		case 829:
			yyval.value = SqlClusterOption.Clustered;
			break;
		case 833:
			yyval = value_stack.array[value_stack.top - 3];
			break;
		case 837:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 838:
		{
			List<SqlIndexedColumn> list6 = new List<SqlIndexedColumn>();
			list6.Add(value_stack.array[value_stack.top - 1].value as SqlIndexedColumn);
			yyval.value = list6;
			break;
		}
		case 839:
		{
			List<SqlIndexedColumn> list5 = value_stack.array[value_stack.top - 3].value as List<SqlIndexedColumn>;
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlIndexedColumn);
			yyval.value = list5;
			break;
		}
		case 840:
		{
			SqlIndexedColumn sqlIndexedColumn2 = new SqlIndexedColumn(value_stack.array[value_stack.top - 2].value as SqlIdentifier, (SqlSortOrder)value_stack.array[value_stack.top - 1].value);
			sqlIndexedColumn2.Position = yyloc;
			yyval.value = sqlIndexedColumn2;
			break;
		}
		case 841:
		{
			SqlIndexedColumn sqlIndexedColumn = new SqlIndexedColumn(new SqlIdentifier(GetText(location_stack.array[location_stack.top - 2]))
			{
				Position = location_stack.array[location_stack.top - 2]
			}, (SqlSortOrder)value_stack.array[value_stack.top - 1].value);
			sqlIndexedColumn.Position = yyloc;
			yyval.value = sqlIndexedColumn;
			break;
		}
		case 843:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 844:
		{
			List<SqlIndexOption> list4 = new List<SqlIndexOption>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlIndexOption);
			yyval.value = list4;
			break;
		}
		case 845:
		{
			List<SqlIndexOption> list3 = new List<SqlIndexOption>();
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlIndexOption);
			yyval.value = list3;
			break;
		}
		case 846:
		{
			List<SqlIndexOption> list2 = new List<SqlIndexOption>();
			list2.Add(value_stack.array[value_stack.top - 2].value as SqlIndexOption);
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlIndexOption);
			yyval.value = list2;
			break;
		}
		case 847:
		{
			List<SqlIndexOption> list = new List<SqlIndexOption>();
			list.Add(value_stack.array[value_stack.top - 2].value as SqlIndexOption);
			list.Add(value_stack.array[value_stack.top - 1].value as SqlIndexOption);
			yyval.value = list;
			break;
		}
		case 849:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 800:
		case 801:
		case 802:
		case 803:
		case 804:
		case 805:
		case 806:
		case 807:
		case 808:
		case 809:
		case 810:
		case 811:
		case 812:
		case 813:
		case 820:
		case 822:
		case 823:
		case 830:
		case 831:
		case 832:
		case 834:
		case 835:
		case 836:
		case 842:
		case 848:
			break;
		}
	}

	private void DoAction_850_899(int action)
	{
		switch (action)
		{
		case 851:
			yyval.value = new CreateIndexKeyList(value_stack.array[value_stack.top - 3].value as List<SqlIndexedColumn>, value_stack.array[value_stack.top - 1].value as List<SqlIdentifier>);
			break;
		case 853:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 854:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 855:
		{
			List<SqlIndexOption> list4 = new List<SqlIndexOption>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlIndexOption);
			yyval.value = list4;
			break;
		}
		case 856:
		{
			List<SqlIndexOption> list3 = value_stack.array[value_stack.top - 3].value as List<SqlIndexOption>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlIndexOption);
			yyval.value = list3;
			break;
		}
		case 857:
		{
			SqlIndexOption sqlIndexOption5;
			switch (GetStringToken(location_stack.array[location_stack.top - 1]))
			{
			case StringToken.PadIndex:
				sqlIndexOption5 = new SqlPadIndexOption(SqlOnOffValue.None);
				break;
			case StringToken.SortInTempDb:
				sqlIndexOption5 = new SqlSortInTempDbIndexOption(SqlOnOffValue.None);
				break;
			case StringToken.IgnoreDupKey:
				sqlIndexOption5 = new SqlIgnoreDupKeyIndexOption(SqlOnOffValue.None);
				break;
			case StringToken.StatisticsNoRecompute:
				sqlIndexOption5 = new SqlStatisticsNoRecomputeIndexOption(SqlOnOffValue.None);
				break;
			case StringToken.DropExisting:
				sqlIndexOption5 = new SqlDropExistingIndexOption(SqlOnOffValue.None);
				break;
			default:
				sqlIndexOption5 = new SqlIndexOptionError();
				VersionedParserBase.ReportError(SR.InvalidUsageOfIndexOption(GetText(location_stack.array[location_stack.top - 1])), location_stack.array[location_stack.top - 1]);
				break;
			}
			sqlIndexOption5.Position = yyloc;
			yyval.value = sqlIndexOption5;
			break;
		}
		case 860:
		{
			SqlFillFactorIndexOption sqlFillFactorIndexOption = new SqlFillFactorIndexOption(value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			sqlFillFactorIndexOption.Position = yyloc;
			yyval.value = sqlFillFactorIndexOption;
			break;
		}
		case 861:
		{
			VersionedParserBase.ReportError(SR.InvalidUsageOfIndexOption(GetText(location_stack.array[location_stack.top - 3])), location_stack.array[location_stack.top - 3]);
			SqlMaxDegreeOfParallelismIndexOption sqlMaxDegreeOfParallelismIndexOption2 = new SqlMaxDegreeOfParallelismIndexOption((SqlLiteralExpression)value_stack.array[value_stack.top - 1].value, isValid: true);
			sqlMaxDegreeOfParallelismIndexOption2.Position = yyloc;
			yyval.value = sqlMaxDegreeOfParallelismIndexOption2;
			break;
		}
		case 862:
		{
			SqlIndexOption sqlIndexOption4 = new SqlSortedDataIndexOption(SqlOnOffValue.None);
			sqlIndexOption4.Position = yyloc;
			yyval.value = sqlIndexOption4;
			break;
		}
		case 863:
		{
			SqlIndexOption sqlIndexOption3 = new SqlSortedDataReorgIndexOption(SqlOnOffValue.None);
			sqlIndexOption3.Position = yyloc;
			yyval.value = sqlIndexOption3;
			break;
		}
		case 882:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 885:
		{
			List<SqlIndexOption> list2 = new List<SqlIndexOption>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlIndexOption);
			yyval.value = list2;
			break;
		}
		case 886:
		{
			List<SqlIndexOption> list = value_stack.array[value_stack.top - 3].value as List<SqlIndexOption>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlIndexOption);
			yyval.value = list;
			break;
		}
		case 892:
		{
			SqlOnlineIndexOption sqlOnlineIndexOption = new SqlOnlineIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 2].value);
			sqlOnlineIndexOption.Position = yyloc;
			yyval.value = sqlOnlineIndexOption;
			break;
		}
		case 894:
		{
			SqlIndexOption sqlIndexOption2;
			switch (GetStringToken(location_stack.array[location_stack.top - 3]))
			{
			case StringToken.PadIndex:
				sqlIndexOption2 = new SqlPadIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			case StringToken.SortInTempDb:
				sqlIndexOption2 = new SqlSortInTempDbIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			case StringToken.IgnoreDupKey:
				sqlIndexOption2 = new SqlIgnoreDupKeyIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			case StringToken.StatisticsIncremental:
				sqlIndexOption2 = new SqlStatisticsIncrementalIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			case StringToken.StatisticsNoRecompute:
				sqlIndexOption2 = new SqlStatisticsNoRecomputeIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			case StringToken.DropExisting:
				sqlIndexOption2 = new SqlDropExistingIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			case StringToken.AllowRowLocks:
				sqlIndexOption2 = new SqlAllowRowLocksIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			case StringToken.AllowPageLocks:
				sqlIndexOption2 = new SqlAllowPageLocksIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			case StringToken.SortedData:
				sqlIndexOption2 = new SqlSortedDataIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			case StringToken.SortedDataReorg:
				sqlIndexOption2 = new SqlSortedDataReorgIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			default:
				sqlIndexOption2 = new SqlIndexOptionError();
				VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 3]);
				break;
			}
			sqlIndexOption2.Position = yyloc;
			yyval.value = sqlIndexOption2;
			break;
		}
		case 895:
		{
			SqlIndexOptionError sqlIndexOptionError = new SqlIndexOptionError();
			sqlIndexOptionError.Position = yyloc;
			yyval.value = sqlIndexOptionError;
			VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 1]);
			break;
		}
		case 896:
		{
			SqlMaxDegreeOfParallelismIndexOption sqlMaxDegreeOfParallelismIndexOption = new SqlMaxDegreeOfParallelismIndexOption(value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			sqlMaxDegreeOfParallelismIndexOption.Position = yyloc;
			yyval.value = sqlMaxDegreeOfParallelismIndexOption;
			break;
		}
		case 897:
		{
			SqlStatisticsOnlyIndexOption sqlStatisticsOnlyIndexOption2 = new SqlStatisticsOnlyIndexOption(value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			sqlStatisticsOnlyIndexOption2.Position = yyloc;
			yyval.value = sqlStatisticsOnlyIndexOption2;
			break;
		}
		case 898:
		{
			SqlStatisticsOnlyIndexOption sqlStatisticsOnlyIndexOption = new SqlStatisticsOnlyIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
			sqlStatisticsOnlyIndexOption.Position = yyloc;
			yyval.value = sqlStatisticsOnlyIndexOption;
			break;
		}
		case 899:
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
		case 850:
		case 852:
		case 858:
		case 859:
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
		case 883:
		case 884:
		case 887:
		case 888:
		case 889:
		case 890:
		case 891:
		case 893:
			break;
		}
	}

	private void DoAction_900_949(int action)
	{
		switch (action)
		{
		case 904:
		{
			SqlBucketCountIndexOption sqlBucketCountIndexOption = new SqlBucketCountIndexOption(value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			sqlBucketCountIndexOption.Position = yyloc;
			yyval.value = sqlBucketCountIndexOption;
			break;
		}
		case 905:
		{
			SqlCompressionDelayIndexOption sqlCompressionDelayIndexOption2 = new SqlCompressionDelayIndexOption(value_stack.array[value_stack.top - 2].value as SqlLiteralExpression);
			sqlCompressionDelayIndexOption2.Position = yyloc;
			yyval.value = sqlCompressionDelayIndexOption2;
			break;
		}
		case 906:
		{
			SqlIndexOptionError sqlIndexOptionError2 = new SqlIndexOptionError();
			sqlIndexOptionError2.Position = yyloc;
			yyval.value = sqlIndexOptionError2;
			break;
		}
		case 907:
		{
			SqlIndexOptionError sqlIndexOptionError = new SqlIndexOptionError();
			sqlIndexOptionError.Position = yyloc;
			yyval.value = sqlIndexOptionError;
			break;
		}
		case 908:
		{
			SqlCompressionDelayIndexOption sqlCompressionDelayIndexOption = new SqlCompressionDelayIndexOption(value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			sqlCompressionDelayIndexOption.Position = yyloc;
			yyval.value = sqlCompressionDelayIndexOption;
			break;
		}
		case 912:
			ExpectKeyword(StringToken.Minutes, location_stack.array[location_stack.top - 1]);
			break;
		case 915:
		{
			SqlCreateUserStatement sqlCreateUserStatement = SqlCreateUserStatement.CreateFromFactory(value_stack.array[value_stack.top - 2].value as CreateUserStatementFactory, value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlCreateUserOption>);
			sqlCreateUserStatement.Position = yyloc;
			yyval.value = sqlCreateUserStatement;
			break;
		}
		case 917:
			yyval.value = CreateUserStatementFactory.CreateCreateUserWithImplicitAuthenticationStatementFactory();
			break;
		case 918:
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
		case 919:
			yyval.value = CreateUserStatementFactory.CreateCreateUserFromAsymKeyStatementFactory(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			ExpectKeyword(StringToken.Asymmetric, location_stack.array[location_stack.top - 3]);
			break;
		case 920:
			yyval.value = CreateUserStatementFactory.CreateCreateUserWithoutLoginStatementFactory();
			ExpectKeyword(StringToken.Without, location_stack.array[location_stack.top - 2]);
			ExpectKeyword(StringToken.Login, location_stack.array[location_stack.top - 1]);
			break;
		case 922:
			yyval.value = CreateUserStatementFactory.CreateCreateUserFromExternalProviderStatementFactory();
			ExpectKeyword(StringToken.Provider, location_stack.array[location_stack.top - 1]);
			break;
		case 926:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 927:
		{
			List<SqlCreateUserOption> list2 = new List<SqlCreateUserOption>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlCreateUserOption);
			yyval.value = list2;
			break;
		}
		case 928:
		{
			List<SqlCreateUserOption> list = value_stack.array[value_stack.top - 3].value as List<SqlCreateUserOption>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlCreateUserOption);
			yyval.value = list;
			break;
		}
		case 929:
		{
			SqlCreateUserOption sqlCreateUserOption3 = new SqlCreateUserOption(GetStringToken(location_stack.array[location_stack.top - 3]), value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			sqlCreateUserOption3.Position = yyloc;
			yyval.value = sqlCreateUserOption3;
			break;
		}
		case 931:
		{
			SqlCreateUserOption sqlCreateUserOption2 = new SqlCreateUserOption(GetStringToken(location_stack.array[location_stack.top - 3]), value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlCreateUserOption2.Position = yyloc;
			yyval.value = sqlCreateUserOption2;
			StringToken stringToken = GetStringToken(location_stack.array[location_stack.top - 3]);
			if (stringToken != StringToken.Password && stringToken != StringToken.DefaultSchema && stringToken != StringToken.DefaultLanguage)
			{
				VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 3]);
			}
			break;
		}
		case 932:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression.Position = location_stack.array[location_stack.top - 1];
			SqlCreateUserOption sqlCreateUserOption = new SqlCreateUserOption(GetStringToken(location_stack.array[location_stack.top - 3]), sqlLiteralExpression);
			sqlCreateUserOption.Position = yyloc;
			yyval.value = sqlCreateUserOption;
			if (GetStringToken(location_stack.array[location_stack.top - 3]) != StringToken.DefaultLanguage)
			{
				VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 3]);
			}
			break;
		}
		case 947:
		{
			SqlDropUserStatement sqlDropUserStatement = new SqlDropUserStatement(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlDropUserStatement.IsDropIfExists = (bool)value_stack.array[value_stack.top - 2].value;
			sqlDropUserStatement.Position = yyloc;
			yyval.value = sqlDropUserStatement;
			break;
		}
		case 948:
		{
			SqlCreateRoleStatement sqlCreateRoleStatement = new SqlCreateRoleStatement(value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlCreateRoleStatement.Position = yyloc;
			yyval.value = sqlCreateRoleStatement;
			break;
		}
		case 900:
		case 901:
		case 902:
		case 903:
		case 909:
		case 910:
		case 911:
		case 913:
		case 914:
		case 916:
		case 921:
		case 923:
		case 924:
		case 925:
		case 930:
		case 933:
		case 934:
		case 935:
		case 936:
		case 937:
		case 938:
		case 939:
		case 940:
		case 941:
		case 942:
		case 943:
		case 944:
		case 945:
		case 946:
		case 949:
			break;
		}
	}

	private void DoAction_950_999(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_1000_1049(int action)
	{
		switch (action)
		{
		case 1049:
		{
			SqlBackupMasterKeyStatement sqlBackupMasterKeyStatement = SqlBackupMasterKeyStatement.Create(isDumpLoadStatement);
			sqlBackupMasterKeyStatement.Position = yyloc;
			yyval.value = sqlBackupMasterKeyStatement;
			break;
		}
		case 1000:
		case 1001:
		case 1002:
		case 1003:
		case 1004:
		case 1005:
		case 1006:
		case 1007:
		case 1008:
		case 1009:
		case 1010:
		case 1011:
		case 1012:
		case 1013:
		case 1014:
		case 1015:
		case 1016:
		case 1017:
		case 1018:
		case 1019:
		case 1020:
		case 1021:
		case 1022:
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
		case 1035:
		case 1036:
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
			break;
		}
	}

	private void DoAction_1050_1099(int action)
	{
		switch (action)
		{
		case 1050:
		{
			SqlRestoreMasterKeyStatement sqlRestoreMasterKeyStatement = SqlRestoreMasterKeyStatement.Create(isDumpLoadStatement);
			sqlRestoreMasterKeyStatement.Position = yyloc;
			yyval.value = sqlRestoreMasterKeyStatement;
			break;
		}
		case 1064:
		{
			SqlBackupServiceMasterKeyStatement sqlBackupServiceMasterKeyStatement = SqlBackupServiceMasterKeyStatement.Create(isDumpLoadStatement);
			sqlBackupServiceMasterKeyStatement.Position = yyloc;
			yyval.value = sqlBackupServiceMasterKeyStatement;
			break;
		}
		case 1065:
			scanner.SetParseState(EParseState.e_ParseLoad);
			isDumpLoadStatement = false;
			break;
		case 1066:
			scanner.SetParseState(EParseState.e_ParseLoad);
			isDumpLoadStatement = true;
			break;
		case 1067:
		{
			SqlRestoreServiceMasterKeyStatement sqlRestoreServiceMasterKeyStatement = SqlRestoreServiceMasterKeyStatement.Create(isDumpLoadStatement);
			sqlRestoreServiceMasterKeyStatement.Position = yyloc;
			yyval.value = sqlRestoreServiceMasterKeyStatement;
			ExpectKeyword(StringToken.Master, location_stack.array[location_stack.top - 12]);
			ExpectKeyword(StringToken.Decryption, location_stack.array[location_stack.top - 6]);
			ExpectKeyword(StringToken.Password, location_stack.array[location_stack.top - 4]);
			break;
		}
		case 1068:
			scanner.SetParseState(EParseState.e_ParseOpen);
			break;
		case 1069:
			scanner.SetParseState(EParseState.e_ParseClose);
			break;
		case 1090:
		{
			SqlLoginPassword sqlLoginPassword = new SqlLoginPassword(value_stack.array[value_stack.top - 3].value as SqlLiteralExpression, value_stack.array[value_stack.top - 2].value as List<SqlLoginOption>);
			sqlLoginPassword.Position = LexLocation.Merge(location_stack.array[location_stack.top - 3], location_stack.array[location_stack.top - 2]);
			SqlCreateLoginWithPasswordStatement sqlCreateLoginWithPasswordStatement = new SqlCreateLoginWithPasswordStatement(value_stack.array[value_stack.top - 7].value as SqlIdentifier, sqlLoginPassword, value_stack.array[value_stack.top - 1].value as List<SqlLoginOption>);
			sqlCreateLoginWithPasswordStatement.Position = yyloc;
			yyval.value = sqlCreateLoginWithPasswordStatement;
			ExpectKeyword(StringToken.Password, location_stack.array[location_stack.top - 5]);
			break;
		}
		case 1091:
		{
			SqlCreateLoginFromWindowsStatement sqlCreateLoginFromWindowsStatement = new SqlCreateLoginFromWindowsStatement(value_stack.array[value_stack.top - 4].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlLoginOption>);
			sqlCreateLoginFromWindowsStatement.Position = yyloc;
			yyval.value = sqlCreateLoginFromWindowsStatement;
			ExpectKeyword(StringToken.Windows, location_stack.array[location_stack.top - 2]);
			break;
		}
		case 1092:
		{
			SqlCreateLoginFromCertificateStatement sqlCreateLoginFromCertificateStatement = new SqlCreateLoginFromCertificateStatement(value_stack.array[value_stack.top - 5].value as SqlIdentifier, value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlLoginOption>);
			sqlCreateLoginFromCertificateStatement.Position = yyloc;
			yyval.value = sqlCreateLoginFromCertificateStatement;
			ExpectKeyword(StringToken.Certificate, location_stack.array[location_stack.top - 3]);
			break;
		}
		case 1093:
		{
			SqlCreateLoginFromAsymKeyStatement sqlCreateLoginFromAsymKeyStatement = new SqlCreateLoginFromAsymKeyStatement(value_stack.array[value_stack.top - 6].value as SqlIdentifier, value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlLoginOption>);
			sqlCreateLoginFromAsymKeyStatement.Position = yyloc;
			yyval.value = sqlCreateLoginFromAsymKeyStatement;
			ExpectKeyword(StringToken.Asymmetric, location_stack.array[location_stack.top - 4]);
			break;
		}
		case 1095:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Binary(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression.Position = yyloc;
			yyval.value = sqlLiteralExpression;
			break;
		}
		case 1098:
		{
			List<SqlLoginOption> list2 = new List<SqlLoginOption>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list2;
			break;
		}
		case 1099:
		{
			List<SqlLoginOption> list = value_stack.array[value_stack.top - 2].value as List<SqlLoginOption>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list;
			break;
		}
		case 1051:
		case 1052:
		case 1053:
		case 1054:
		case 1055:
		case 1056:
		case 1057:
		case 1058:
		case 1059:
		case 1060:
		case 1061:
		case 1062:
		case 1063:
		case 1070:
		case 1071:
		case 1072:
		case 1073:
		case 1074:
		case 1075:
		case 1076:
		case 1077:
		case 1078:
		case 1079:
		case 1080:
		case 1081:
		case 1082:
		case 1083:
		case 1084:
		case 1085:
		case 1086:
		case 1087:
		case 1088:
		case 1089:
		case 1094:
		case 1096:
		case 1097:
			break;
		}
	}

	private void DoAction_1100_1149(int action)
	{
		switch (action)
		{
		case 1100:
		{
			SqlLoginOption sqlLoginOption14 = SqlLoginOption.Create(GetText(location_stack.array[location_stack.top - 1]), SqlLoginOptionContext.PasswordContext);
			sqlLoginOption14.Position = yyloc;
			yyval.value = sqlLoginOption14;
			break;
		}
		case 1103:
		{
			List<SqlLoginOption> list11 = new List<SqlLoginOption>();
			list11.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list11;
			break;
		}
		case 1104:
		{
			List<SqlLoginOption> list10 = value_stack.array[value_stack.top - 3].value as List<SqlLoginOption>;
			list10.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list10;
			break;
		}
		case 1105:
		{
			SqlLoginOption sqlLoginOption13 = SqlLoginOption.Create(GetText(location_stack.array[location_stack.top - 3]), (SqlOnOffValue)value_stack.array[value_stack.top - 1].value, SqlLoginOptionContext.WithPasswordContext);
			sqlLoginOption13.Position = yyloc;
			yyval.value = sqlLoginOption13;
			break;
		}
		case 1106:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Binary(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression.Position = location_stack.array[location_stack.top - 1];
			SqlLoginOption sqlLoginOption12 = SqlLoginOption.Create(GetText(location_stack.array[location_stack.top - 3]), sqlLiteralExpression, SqlLoginOptionContext.WithPasswordContext);
			sqlLoginOption12.Position = yyloc;
			yyval.value = sqlLoginOption12;
			break;
		}
		case 1107:
		{
			SqlLoginOption sqlLoginOption11 = SqlLoginOption.Create(GetText(location_stack.array[location_stack.top - 3]), value_stack.array[value_stack.top - 1].value as SqlIdentifier, SqlLoginOptionContext.WithPasswordContext);
			sqlLoginOption11.Position = yyloc;
			yyval.value = sqlLoginOption11;
			break;
		}
		case 1109:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1110:
		{
			List<SqlLoginOption> list9 = new List<SqlLoginOption>();
			list9.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list9;
			break;
		}
		case 1111:
		{
			List<SqlLoginOption> list8 = value_stack.array[value_stack.top - 3].value as List<SqlLoginOption>;
			list8.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list8;
			break;
		}
		case 1112:
		{
			SqlLoginOption sqlLoginOption10 = SqlLoginOption.Create(GetText(location_stack.array[location_stack.top - 3]), value_stack.array[value_stack.top - 1].value as SqlIdentifier, SqlLoginOptionContext.FromWindowsContext);
			sqlLoginOption10.Position = yyloc;
			yyval.value = sqlLoginOption10;
			break;
		}
		case 1114:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1115:
		{
			List<SqlLoginOption> list7 = new List<SqlLoginOption>();
			list7.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list7;
			break;
		}
		case 1116:
		{
			List<SqlLoginOption> list6 = value_stack.array[value_stack.top - 3].value as List<SqlLoginOption>;
			list6.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list6;
			break;
		}
		case 1117:
		{
			SqlLoginOption sqlLoginOption9 = SqlLoginOption.Create(GetText(location_stack.array[location_stack.top - 3]), value_stack.array[value_stack.top - 1].value as SqlIdentifier, SqlLoginOptionContext.FromCertificateContext);
			sqlLoginOption9.Position = yyloc;
			yyval.value = sqlLoginOption9;
			break;
		}
		case 1118:
		{
			SqlAlterLoginStatement sqlAlterLoginStatement2 = new SqlAlterLoginStatement(value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlLoginOption>);
			sqlAlterLoginStatement2.Position = yyloc;
			yyval.value = sqlAlterLoginStatement2;
			break;
		}
		case 1119:
		{
			SqlAlterLoginStatement sqlAlterLoginStatement = new SqlAlterLoginStatement(value_stack.array[value_stack.top - 2].value as SqlIdentifier);
			sqlAlterLoginStatement.Position = yyloc;
			yyval.value = sqlAlterLoginStatement;
			break;
		}
		case 1120:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1121:
		{
			List<SqlLoginOption> list5 = new List<SqlLoginOption>();
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list5;
			break;
		}
		case 1122:
		{
			List<SqlLoginOption> list4 = new List<SqlLoginOption>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list4;
			break;
		}
		case 1123:
		{
			List<SqlLoginOption> list3 = value_stack.array[value_stack.top - 3].value as List<SqlLoginOption>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list3;
			break;
		}
		case 1124:
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
		case 1125:
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
		case 1126:
		{
			SqlLoginOption sqlLoginOption6 = SqlLoginOption.Create(GetText(location_stack.array[location_stack.top - 3]), (SqlOnOffValue)value_stack.array[value_stack.top - 1].value, SqlLoginOptionContext.AlterOptionsContext);
			sqlLoginOption6.Position = yyloc;
			yyval.value = sqlLoginOption6;
			break;
		}
		case 1127:
		{
			SqlLoginOption sqlLoginOption5 = SqlLoginOption.Create(GetText(location_stack.array[location_stack.top - 3]), value_stack.array[value_stack.top - 1].value as SqlIdentifier, SqlLoginOptionContext.AlterOptionsContext);
			sqlLoginOption5.Position = yyloc;
			yyval.value = sqlLoginOption5;
			break;
		}
		case 1128:
		{
			SqlLoginOption sqlLoginOption4 = SqlLoginOption.Create(SqlLoginOptionType.NoCredential);
			sqlLoginOption4.Position = yyloc;
			yyval.value = sqlLoginOption4;
			ExpectKeyword(StringToken.No, location_stack.array[location_stack.top - 2]);
			ExpectKeyword(StringToken.Credential, location_stack.array[location_stack.top - 1]);
			break;
		}
		case 1129:
		{
			SqlLoginOption sqlLoginOption3 = SqlLoginOption.Create(SqlModificationType.Add, GetText(location_stack.array[location_stack.top - 2]), value_stack.array[value_stack.top - 1].value as SqlIdentifier, SqlLoginOptionContext.FromCertificateContext);
			sqlLoginOption3.Position = yyloc;
			yyval.value = sqlLoginOption3;
			ExpectKeyword(StringToken.Credential, location_stack.array[location_stack.top - 2]);
			break;
		}
		case 1130:
		{
			SqlLoginOption sqlLoginOption2 = SqlLoginOption.Create(SqlModificationType.Drop, GetText(location_stack.array[location_stack.top - 2]), value_stack.array[value_stack.top - 1].value as SqlIdentifier, SqlLoginOptionContext.FromCertificateContext);
			sqlLoginOption2.Position = yyloc;
			yyval.value = sqlLoginOption2;
			ExpectKeyword(StringToken.Credential, location_stack.array[location_stack.top - 2]);
			break;
		}
		case 1132:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1133:
		{
			List<SqlLoginOption> list2 = new List<SqlLoginOption>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list2;
			break;
		}
		case 1134:
		{
			List<SqlLoginOption> list = value_stack.array[value_stack.top - 2].value as List<SqlLoginOption>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list;
			break;
		}
		case 1135:
		{
			SqlLoginOption sqlLoginOption = SqlLoginOption.Create(GetText(location_stack.array[location_stack.top - 1]), SqlLoginOptionContext.PasswordContext);
			sqlLoginOption.Position = yyloc;
			yyval.value = sqlLoginOption;
			break;
		}
		case 1136:
		{
			SqlDropLoginStatement sqlDropLoginStatement = new SqlDropLoginStatement(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlDropLoginStatement.Position = yyloc;
			yyval.value = sqlDropLoginStatement;
			break;
		}
		case 1101:
		case 1102:
		case 1108:
		case 1113:
		case 1131:
		case 1137:
		case 1138:
		case 1139:
		case 1140:
		case 1141:
		case 1142:
		case 1143:
		case 1144:
		case 1145:
		case 1146:
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
		}
	}

	private void DoAction_1200_1249(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_1250_1299(int action)
	{
		switch (action)
		{
		case 1254:
			scanner.SetParseState(EParseState.e_ParseDump);
			isDumpLoadStatement = false;
			break;
		case 1255:
			scanner.SetParseState(EParseState.e_ParseDump);
			isDumpLoadStatement = true;
			break;
		case 1256:
		{
			SqlBackupCertificateStatement sqlBackupCertificateStatement = SqlBackupCertificateStatement.Create(isDumpLoadStatement);
			sqlBackupCertificateStatement.Position = yyloc;
			yyval.value = sqlBackupCertificateStatement;
			break;
		}
		case 1269:
			scanner.ClearQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 1250:
		case 1251:
		case 1252:
		case 1253:
		case 1257:
		case 1258:
		case 1259:
		case 1260:
		case 1261:
		case 1262:
		case 1263:
		case 1264:
		case 1265:
		case 1266:
		case 1267:
		case 1268:
		case 1270:
		case 1271:
		case 1272:
		case 1273:
		case 1274:
		case 1275:
		case 1276:
		case 1277:
		case 1278:
		case 1279:
		case 1280:
		case 1281:
		case 1282:
		case 1283:
		case 1284:
		case 1285:
		case 1286:
		case 1287:
		case 1288:
		case 1289:
		case 1290:
		case 1291:
		case 1292:
		case 1293:
		case 1294:
		case 1295:
		case 1296:
		case 1297:
		case 1298:
		case 1299:
			break;
		}
	}

	private void DoAction_1300_1349(int action)
	{
		switch (action)
		{
		case 1312:
			scanner.ClearQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 1300:
		case 1301:
		case 1302:
		case 1303:
		case 1304:
		case 1305:
		case 1306:
		case 1307:
		case 1308:
		case 1309:
		case 1310:
		case 1311:
		case 1313:
		case 1314:
		case 1315:
		case 1316:
		case 1317:
		case 1318:
		case 1319:
		case 1320:
		case 1321:
		case 1322:
		case 1323:
		case 1324:
		case 1325:
		case 1326:
		case 1327:
		case 1328:
		case 1329:
		case 1330:
		case 1331:
		case 1332:
		case 1333:
		case 1334:
		case 1335:
		case 1336:
		case 1337:
		case 1338:
		case 1339:
		case 1340:
		case 1341:
		case 1342:
		case 1343:
		case 1344:
		case 1345:
		case 1346:
		case 1347:
		case 1348:
		case 1349:
			break;
		}
	}

	private void DoAction_1350_1399(int action)
	{
		switch (action)
		{
		case 1358:
		{
			SqlProcedureDefinitionForCreate sqlProcedureDefinitionForCreate = new SqlProcedureDefinitionForCreate(value_stack.array[value_stack.top - 1].value as SqlProcedureDefinition, (bool)value_stack.array[value_stack.top - 3].value);
			sqlProcedureDefinitionForCreate.Position = yyloc;
			yyval.value = sqlProcedureDefinitionForCreate;
			break;
		}
		case 1359:
			yyval.value = value_stack.array[value_stack.top - 1].value;
			break;
		case 1361:
		{
			SqlCreateProcedureStatement sqlCreateProcedureStatement = new SqlCreateProcedureStatement(value_stack.array[value_stack.top - 3].value as SqlProcedureDefinitionForCreate, value_stack.array[value_stack.top - 1].value as List<SqlStatement>);
			sqlCreateProcedureStatement.Position = yyloc;
			yyval.value = sqlCreateProcedureStatement;
			break;
		}
		case 1363:
		{
			SqlAlterProcedureStatement sqlAlterProcedureStatement = new SqlAlterProcedureStatement(value_stack.array[value_stack.top - 3].value as SqlProcedureDefinition, value_stack.array[value_stack.top - 1].value as List<SqlStatement>);
			sqlAlterProcedureStatement.Position = yyloc;
			yyval.value = sqlAlterProcedureStatement;
			break;
		}
		case 1365:
		{
			SqlProcedureDefinition sqlProcedureDefinition = new SqlProcedureDefinition(value_stack.array[value_stack.top - 6].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 5].value as SqlLiteralExpression, value_stack.array[value_stack.top - 4].value as List<SqlParameterDeclaration>, value_stack.array[value_stack.top - 3].value as List<SqlModuleOption>, value_stack.array[value_stack.top - 2].value != null);
			sqlProcedureDefinition.Position = yyloc;
			yyval.value = sqlProcedureDefinition;
			scanner.SetStmtDelimiter(value: true);
			break;
		}
		case 1368:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 1372:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression.Position = yyloc;
			yyval.value = sqlLiteralExpression;
			break;
		}
		case 1374:
			yyval.value = "for replication";
			break;
		case 1376:
			yyval.value = value_stack.array[value_stack.top - 1].value;
			break;
		case 1377:
		{
			List<SqlModuleOption> list2 = new List<SqlModuleOption>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlModuleOption);
			yyval.value = list2;
			break;
		}
		case 1378:
		{
			List<SqlModuleOption> list = value_stack.array[value_stack.top - 3].value as List<SqlModuleOption>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlModuleOption);
			yyval.value = list;
			break;
		}
		case 1379:
		{
			SqlModuleOption sqlModuleOption2 = SqlModuleOption.Create(GetText(location_stack.array[location_stack.top - 1]));
			sqlModuleOption2.Position = yyloc;
			yyval.value = sqlModuleOption2;
			break;
		}
		case 1380:
		{
			SqlModuleOption sqlModuleOption = SqlModuleOption.ExecuteAs(value_stack.array[value_stack.top - 1].value as SqlExecuteAsClause);
			sqlModuleOption.Position = yyloc;
			yyval.value = sqlModuleOption;
			break;
		}
		case 1381:
		{
			SqlExecuteAsClause sqlExecuteAsClause3 = value_stack.array[value_stack.top - 1].value as SqlExecuteAsClause;
			sqlExecuteAsClause3.Position = yyloc;
			yyval.value = sqlExecuteAsClause3;
			break;
		}
		case 1382:
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
		case 1383:
		{
			SqlExecuteAsClause sqlExecuteAsClause = SqlExecuteAsClause.User(value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			sqlExecuteAsClause.Position = yyloc;
			yyval.value = sqlExecuteAsClause;
			break;
		}
		case 1350:
		case 1351:
		case 1352:
		case 1353:
		case 1354:
		case 1355:
		case 1356:
		case 1357:
		case 1360:
		case 1362:
		case 1364:
		case 1366:
		case 1367:
		case 1369:
		case 1370:
		case 1371:
		case 1373:
		case 1375:
		case 1384:
		case 1385:
		case 1386:
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
		}
	}

	private void DoAction_1450_1499(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_1500_1549(int action)
	{
		switch (action)
		{
		case 1503:
			scanner.SetParseState(EParseState.e_ParseBegin);
			break;
		case 1544:
		{
			SqlCreateTypeStatement sqlCreateTypeStatement = SqlCreateTypeStatement.Create(value_stack.array[value_stack.top - 2].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 1].value as CreateTypeStatementInfo);
			sqlCreateTypeStatement.Position = yyloc;
			yyval.value = sqlCreateTypeStatement;
			break;
		}
		case 1545:
			yyval.value = new CreateTypeStatementInfo(value_stack.array[value_stack.top - 2].value as SqlDataTypeSpecification, (SqlUserDefinedDataTypeNullState)value_stack.array[value_stack.top - 1].value);
			break;
		case 1546:
			yyval.value = new CreateTypeStatementInfo(value_stack.array[value_stack.top - 1].value as SqlClrClassSpecifier);
			break;
		case 1547:
			yyval.value = new CreateTypeStatementInfo(value_stack.array[value_stack.top - 2].value as SqlTableDefinition);
			break;
		case 1548:
			yyval = value_stack.array[value_stack.top - 1];
			ExpectKeyword(StringToken.Name, location_stack.array[location_stack.top - 2]);
			break;
		case 1549:
			yyval.value = SqlUserDefinedDataTypeNullState.None;
			break;
		case 1500:
		case 1501:
		case 1502:
		case 1504:
		case 1505:
		case 1506:
		case 1507:
		case 1508:
		case 1509:
		case 1510:
		case 1511:
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
			break;
		}
	}

	private void DoAction_1550_1599(int action)
	{
		switch (action)
		{
		case 1550:
			yyval.value = SqlUserDefinedDataTypeNullState.Null;
			break;
		case 1551:
			yyval.value = SqlUserDefinedDataTypeNullState.NotNull;
			break;
		case 1553:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1554:
		{
			SqlClrClassSpecifier sqlClrClassSpecifier2 = new SqlClrClassSpecifier(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlClrClassSpecifier2.Position = yyloc;
			yyval.value = sqlClrClassSpecifier2;
			break;
		}
		case 1555:
		{
			SqlClrClassSpecifier sqlClrClassSpecifier = new SqlClrClassSpecifier(value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlClrClassSpecifier.Position = yyloc;
			yyval.value = sqlClrClassSpecifier;
			break;
		}
		case 1556:
		{
			SqlCreateFunctionStatement sqlCreateFunctionStatement = new SqlCreateFunctionStatement(value_stack.array[value_stack.top - 1].value as SqlFunctionDefinition, (bool)value_stack.array[value_stack.top - 3].value);
			sqlCreateFunctionStatement.Position = yyloc;
			yyval.value = sqlCreateFunctionStatement;
			break;
		}
		case 1557:
		{
			SqlAlterFunctionStatement sqlAlterFunctionStatement = new SqlAlterFunctionStatement(value_stack.array[value_stack.top - 1].value as SqlFunctionDefinition);
			sqlAlterFunctionStatement.Position = yyloc;
			yyval.value = sqlAlterFunctionStatement;
			break;
		}
		case 1558:
		{
			SqlFunctionDefinition sqlFunctionDefinition = SqlFunctionDefinition.CreateFromFactory(value_stack.array[value_stack.top - 1].value as FunctionDefinitionFactory, value_stack.array[value_stack.top - 6].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 4].value as List<SqlParameterDeclaration>);
			sqlFunctionDefinition.Position = yyloc;
			yyval.value = sqlFunctionDefinition;
			ExpectKeyword(StringToken.Returns, location_stack.array[location_stack.top - 2]);
			break;
		}
		case 1559:
			yyval.value = FunctionDefinitionFactory.CreateScalarFactory(value_stack.array[value_stack.top - 4].value as SqlScalarFunctionReturnType, value_stack.array[value_stack.top - 3].value as List<SqlModuleOption>, value_stack.array[value_stack.top - 1].value as SqlFunctionBodyDefinition);
			break;
		case 1560:
			yyval.value = FunctionDefinitionFactory.CreateTableRelationalFactory(value_stack.array[value_stack.top - 4].value as FunctionVariableNameAndTableReturnTypePair, value_stack.array[value_stack.top - 3].value as List<SqlModuleOption>, value_stack.array[value_stack.top - 1].value as SqlFunctionBodyDefinition);
			break;
		case 1561:
			yyval.value = FunctionDefinitionFactory.CreateTableClrFactory(value_stack.array[value_stack.top - 5].value as SqlTableFunctionReturnType, value_stack.array[value_stack.top - 4].value as List<SqlModuleOption>, value_stack.array[value_stack.top - 3].value as SqlSimpleOrderByClause, value_stack.array[value_stack.top - 1].value as SqlClrFunctionBodyDefinition);
			break;
		case 1563:
		{
			SqlSimpleOrderByClause sqlSimpleOrderByClause = new SqlSimpleOrderByClause(value_stack.array[value_stack.top - 2].value as List<SqlSimpleOrderByItem>);
			sqlSimpleOrderByClause.Position = yyloc;
			yyval.value = sqlSimpleOrderByClause;
			break;
		}
		case 1565:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1566:
		{
			List<SqlModuleOption> list2 = new List<SqlModuleOption>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlModuleOption);
			yyval.value = list2;
			break;
		}
		case 1567:
		{
			List<SqlModuleOption> list = value_stack.array[value_stack.top - 3].value as List<SqlModuleOption>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlModuleOption);
			yyval.value = list;
			break;
		}
		case 1570:
		{
			SqlModuleOption sqlModuleOption4 = SqlModuleOption.ExecuteAs(value_stack.array[value_stack.top - 1].value as SqlExecuteAsClause);
			sqlModuleOption4.Position = yyloc;
			yyval.value = sqlModuleOption4;
			break;
		}
		case 1571:
		{
			SqlModuleOption sqlModuleOption3 = SqlModuleOption.Create(GetText(location_stack.array[location_stack.top - 1]));
			sqlModuleOption3.Position = yyloc;
			yyval.value = sqlModuleOption3;
			break;
		}
		case 1572:
		{
			SqlModuleOption sqlModuleOption2 = SqlModuleOption.CreateReturnsNullOnNullInputOption();
			sqlModuleOption2.Position = yyloc;
			yyval.value = sqlModuleOption2;
			ExpectKeyword(StringToken.Returns, location_stack.array[location_stack.top - 5]);
			ExpectKeyword(StringToken.Input, location_stack.array[location_stack.top - 1]);
			break;
		}
		case 1573:
		{
			SqlModuleOption sqlModuleOption = SqlModuleOption.CreateCalledOnNullInputOption();
			sqlModuleOption.Position = yyloc;
			yyval.value = sqlModuleOption;
			ExpectKeyword(StringToken.Called, location_stack.array[location_stack.top - 4]);
			ExpectKeyword(StringToken.Input, location_stack.array[location_stack.top - 1]);
			break;
		}
		case 1577:
		{
			SqlMultistatementFunctionBodyDefinition sqlMultistatementFunctionBodyDefinition = new SqlMultistatementFunctionBodyDefinition(value_stack.array[value_stack.top - 1].value as SqlCompoundStatement);
			sqlMultistatementFunctionBodyDefinition.Position = yyloc;
			yyval.value = sqlMultistatementFunctionBodyDefinition;
			break;
		}
		case 1578:
		{
			SqlFunctionBodyDefinition sqlFunctionBodyDefinition = value_stack.array[value_stack.top - 1].value as SqlFunctionBodyDefinition;
			sqlFunctionBodyDefinition.Position = yyloc;
			yyval.value = sqlFunctionBodyDefinition;
			break;
		}
		case 1579:
		{
			SqlInlineFunctionBodyDefinition sqlInlineFunctionBodyDefinition = new SqlInlineFunctionBodyDefinition(null, value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlInlineFunctionBodyDefinition.Position = yyloc;
			yyval.value = sqlInlineFunctionBodyDefinition;
			break;
		}
		case 1581:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 1582:
		{
			SqlClrFunctionBodyDefinition sqlClrFunctionBodyDefinition = new SqlClrFunctionBodyDefinition(value_stack.array[value_stack.top - 1].value as SqlClrMethodSpecifier);
			sqlClrFunctionBodyDefinition.Position = yyloc;
			yyval.value = sqlClrFunctionBodyDefinition;
			ExpectKeyword(StringToken.Name, location_stack.array[location_stack.top - 2]);
			break;
		}
		case 1583:
		{
			SqlClrMethodSpecifier sqlClrMethodSpecifier = new SqlClrMethodSpecifier(value_stack.array[value_stack.top - 5].value as SqlIdentifier, value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlClrMethodSpecifier.Position = yyloc;
			yyval.value = sqlClrMethodSpecifier;
			break;
		}
		case 1584:
		{
			SqlScalarFunctionReturnType sqlScalarFunctionReturnType = new SqlScalarFunctionReturnType(value_stack.array[value_stack.top - 1].value as SqlDataTypeSpecification);
			sqlScalarFunctionReturnType.Position = yyloc;
			yyval.value = sqlScalarFunctionReturnType;
			break;
		}
		case 1585:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.String(GetText(location_stack.array[location_stack.top - 3]));
			sqlLiteralExpression.Position = location_stack.array[location_stack.top - 3];
			SqlTableFunctionReturnType sqlTableFunctionReturnType2 = new SqlTableFunctionReturnType(value_stack.array[value_stack.top - 1].value as SqlTableDefinition);
			sqlTableFunctionReturnType2.Position = location_stack.array[location_stack.top - 1];
			yyval.value = new FunctionVariableNameAndTableReturnTypePair(sqlLiteralExpression, sqlTableFunctionReturnType2);
			break;
		}
		case 1587:
		{
			SqlTableFunctionReturnType sqlTableFunctionReturnType = new SqlTableFunctionReturnType(value_stack.array[value_stack.top - 1].value as SqlTableDefinition);
			sqlTableFunctionReturnType.Position = yyloc;
			yyval.value = sqlTableFunctionReturnType;
			break;
		}
		case 1591:
		{
			SqlTriggerDefinition sqlTriggerDefinition = SqlTriggerDefinition.Create(value_stack.array[value_stack.top - 1].value as SqlTriggerDefinition, (bool)value_stack.array[value_stack.top - 3].value);
			sqlTriggerDefinition.Position = yyloc;
			yyval.value = sqlTriggerDefinition;
			break;
		}
		case 1592:
			yyval.value = value_stack.array[value_stack.top - 1].value;
			break;
		case 1593:
		{
			SqlCreateTriggerStatement sqlCreateTriggerStatement = new SqlCreateTriggerStatement(value_stack.array[value_stack.top - 3].value as SqlTriggerDefinition, value_stack.array[value_stack.top - 1].value as List<SqlStatement>);
			sqlCreateTriggerStatement.Position = yyloc;
			yyval.value = sqlCreateTriggerStatement;
			break;
		}
		case 1595:
		{
			SqlAlterTriggerStatement sqlAlterTriggerStatement = new SqlAlterTriggerStatement(value_stack.array[value_stack.top - 3].value as SqlTriggerDefinition, value_stack.array[value_stack.top - 1].value as List<SqlStatement>);
			sqlAlterTriggerStatement.Position = yyloc;
			yyval.value = sqlAlterTriggerStatement;
			break;
		}
		case 1597:
			yyval.value = TriggerTargetData.CreateDmlTarget(value_stack.array[value_stack.top - 1].value as SqlObjectIdentifier);
			break;
		case 1598:
			yyval.value = TriggerTargetData.CreateDdlTarget(SqlDdlTriggerTargetType.Database);
			break;
		case 1599:
			yyval.value = TriggerTargetData.CreateDdlTarget(SqlDdlTriggerTargetType.AllServer);
			ExpectKeyword(StringToken.Server, location_stack.array[location_stack.top - 1]);
			break;
		case 1552:
		case 1562:
		case 1564:
		case 1568:
		case 1569:
		case 1574:
		case 1575:
		case 1576:
		case 1580:
		case 1586:
		case 1588:
		case 1589:
		case 1590:
		case 1594:
		case 1596:
			break;
		}
	}

	private void DoAction_1600_1649(int action)
	{
		switch (action)
		{
		case 1600:
			yyval.value = TriggerActivationData.CreateDmlActivation((SqlTriggerActivationType)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as List<SqlTriggerAction>);
			break;
		case 1601:
			yyval.value = TriggerActivationData.CreateDdlActivation((SqlTriggerActivationType)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as List<SqlTriggerEvent>);
			break;
		case 1602:
			yyval.value = SqlTriggerActivationType.For;
			break;
		case 1603:
			ExpectKeyword(StringToken.After, location_stack.array[location_stack.top - 1]);
			yyval.value = SqlTriggerActivationType.After;
			break;
		case 1604:
		{
			List<SqlTriggerEvent> list8 = new List<SqlTriggerEvent>();
			list8.Add(value_stack.array[value_stack.top - 1].value as SqlTriggerEvent);
			yyval.value = list8;
			break;
		}
		case 1605:
		{
			List<SqlTriggerEvent> list7 = value_stack.array[value_stack.top - 3].value as List<SqlTriggerEvent>;
			list7.Add(value_stack.array[value_stack.top - 1].value as SqlTriggerEvent);
			yyval.value = list7;
			break;
		}
		case 1606:
		{
			SqlTriggerEvent sqlTriggerEvent = new SqlTriggerEvent(GetText(location_stack.array[location_stack.top - 1]));
			sqlTriggerEvent.Position = yyloc;
			yyval.value = sqlTriggerEvent;
			break;
		}
		case 1607:
		{
			SqlTriggerDefinition sqlTriggerDefinition = SqlTriggerDefinition.Create(value_stack.array[value_stack.top - 7].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 5].value as TriggerTargetData, value_stack.array[value_stack.top - 4].value as List<SqlModuleOption>, value_stack.array[value_stack.top - 3].value as TriggerActivationData, (bool)value_stack.array[value_stack.top - 2].value, (bool)value_stack.array[value_stack.top - 1].value);
			sqlTriggerDefinition.Position = yyloc;
			yyval.value = sqlTriggerDefinition;
			break;
		}
		case 1608:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 1610:
			yyval.value = false;
			break;
		case 1611:
			yyval.value = true;
			break;
		case 1612:
			yyval.value = SqlTriggerActivationType.For;
			break;
		case 1613:
			ExpectKeyword(StringToken.After, location_stack.array[location_stack.top - 1]);
			yyval.value = SqlTriggerActivationType.After;
			break;
		case 1614:
			ExpectKeyword(StringToken.Instead, location_stack.array[location_stack.top - 2]);
			yyval.value = SqlTriggerActivationType.InsteadOf;
			break;
		case 1615:
		{
			List<SqlTriggerAction> list6 = new List<SqlTriggerAction>();
			list6.Add(value_stack.array[value_stack.top - 1].value as SqlTriggerAction);
			yyval.value = list6;
			break;
		}
		case 1616:
		{
			List<SqlTriggerAction> list5 = value_stack.array[value_stack.top - 3].value as List<SqlTriggerAction>;
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlTriggerAction);
			yyval.value = list5;
			break;
		}
		case 1617:
		{
			SqlTriggerAction sqlTriggerAction3 = new SqlTriggerAction(SqlTriggerActionType.Insert);
			sqlTriggerAction3.Position = yyloc;
			yyval.value = sqlTriggerAction3;
			break;
		}
		case 1618:
		{
			SqlTriggerAction sqlTriggerAction2 = new SqlTriggerAction(SqlTriggerActionType.Update);
			sqlTriggerAction2.Position = yyloc;
			yyval.value = sqlTriggerAction2;
			break;
		}
		case 1619:
		{
			SqlTriggerAction sqlTriggerAction = new SqlTriggerAction(SqlTriggerActionType.Delete);
			sqlTriggerAction.Position = yyloc;
			yyval.value = sqlTriggerAction;
			break;
		}
		case 1620:
		{
			SqlCreateViewStatement sqlCreateViewStatement = new SqlCreateViewStatement(value_stack.array[value_stack.top - 1].value as SqlViewDefinition, (bool)value_stack.array[value_stack.top - 3].value);
			sqlCreateViewStatement.Position = yyloc;
			yyval.value = sqlCreateViewStatement;
			break;
		}
		case 1621:
		{
			SqlAlterViewStatement sqlAlterViewStatement = new SqlAlterViewStatement(value_stack.array[value_stack.top - 1].value as SqlViewDefinition);
			sqlAlterViewStatement.Position = yyloc;
			yyval.value = sqlAlterViewStatement;
			break;
		}
		case 1622:
		{
			SqlViewDefinition sqlViewDefinition = new SqlViewDefinition(value_stack.array[value_stack.top - 7].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 6].value as List<SqlIdentifier>, value_stack.array[value_stack.top - 5].value as List<SqlModuleOption>, value_stack.array[value_stack.top - 3].value as SqlQueryWithClause, value_stack.array[value_stack.top - 2].value as SqlQueryExpression, (bool)value_stack.array[value_stack.top - 1].value);
			sqlViewDefinition.Position = yyloc;
			yyval.value = sqlViewDefinition;
			break;
		}
		case 1624:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 1625:
		{
			List<SqlIdentifier> list4 = new List<SqlIdentifier>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list4;
			break;
		}
		case 1626:
		{
			List<SqlIdentifier> list3 = value_stack.array[value_stack.top - 3].value as List<SqlIdentifier>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list3;
			break;
		}
		case 1628:
			yyval.value = false;
			break;
		case 1629:
			yyval.value = true;
			break;
		case 1632:
		{
			SqlCreateSchemaStatement sqlCreateSchemaStatement = new SqlCreateSchemaStatement(value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlStatement>);
			sqlCreateSchemaStatement.Position = yyloc;
			yyval.value = sqlCreateSchemaStatement;
			break;
		}
		case 1637:
		{
			List<SqlStatement> list2 = new List<SqlStatement>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlStatement);
			yyval.value = list2;
			break;
		}
		case 1638:
		{
			List<SqlStatement> list = value_stack.array[value_stack.top - 2].value as List<SqlStatement>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlStatement);
			yyval.value = list;
			break;
		}
		case 1647:
		{
			SqlDropSchemaStatement sqlDropSchemaStatement2 = new SqlDropSchemaStatement(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlDropSchemaStatement2.IsDropIfExists = (bool)value_stack.array[value_stack.top - 2].value;
			sqlDropSchemaStatement2.Position = yyloc;
			yyval.value = sqlDropSchemaStatement2;
			break;
		}
		case 1648:
		{
			SqlIdentifier sqlIdentifier = SqlIdentifier.Missing();
			sqlIdentifier.Position = location_stack.array[location_stack.top - 2];
			SqlDropSchemaStatement sqlDropSchemaStatement = new SqlDropSchemaStatement(sqlIdentifier);
			sqlDropSchemaStatement.Position = yyloc;
			yyval.value = sqlDropSchemaStatement;
			break;
		}
		case 1649:
			scanner.SetQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 1609:
		case 1623:
		case 1627:
		case 1630:
		case 1631:
		case 1633:
		case 1634:
		case 1635:
		case 1636:
		case 1639:
		case 1640:
		case 1641:
		case 1642:
		case 1643:
		case 1644:
		case 1645:
		case 1646:
			break;
		}
	}

	private void DoAction_1650_1699(int action)
	{
		switch (action)
		{
		case 1650:
			yyval.value = false;
			break;
		case 1651:
			yyval.value = true;
			break;
		case 1652:
		{
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			SqlDropDatabaseStatement sqlDropDatabaseStatement = new SqlDropDatabaseStatement(value_stack.array[value_stack.top - 1].value as List<SqlIdentifier>);
			sqlDropDatabaseStatement.IsDropIfExists = (bool)value_stack.array[value_stack.top - 2].value;
			sqlDropDatabaseStatement.Position = yyloc;
			yyval.value = sqlDropDatabaseStatement;
			break;
		}
		case 1653:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 1656:
		{
			List<SqlIdentifier> list4 = new List<SqlIdentifier>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list4;
			break;
		}
		case 1657:
		{
			List<SqlIdentifier> list3 = value_stack.array[value_stack.top - 3].value as List<SqlIdentifier>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list3;
			break;
		}
		case 1658:
		{
			SqlIdentifier sqlIdentifier2 = SqlIdentifier.Missing();
			sqlIdentifier2.Position = yyloc;
			List<SqlIdentifier> list2 = new List<SqlIdentifier>();
			list2.Add(sqlIdentifier2);
			yyval.value = list2;
			break;
		}
		case 1659:
		{
			SqlIdentifier sqlIdentifier = SqlIdentifier.Missing();
			sqlIdentifier.Position = location_stack.array[location_stack.top - 1];
			List<SqlIdentifier> list = value_stack.array[value_stack.top - 3].value as List<SqlIdentifier>;
			list.Add(sqlIdentifier);
			yyval.value = list;
			break;
		}
		case 1660:
		{
			SqlDropTypeStatement sqlDropTypeStatement = new SqlDropTypeStatement(value_stack.array[value_stack.top - 1].value as SqlObjectIdentifier);
			sqlDropTypeStatement.IsDropIfExists = (bool)value_stack.array[value_stack.top - 2].value;
			sqlDropTypeStatement.Position = yyloc;
			yyval.value = sqlDropTypeStatement;
			break;
		}
		case 1688:
		{
			SqlDropStatement sqlDropStatement = SqlDropStatement.Create((SqlDropObjectType)value_stack.array[value_stack.top - 4].value, value_stack.array[value_stack.top - 2].value as List<SqlObjectIdentifier>, (SqlDropTriggerTargetScope)value_stack.array[value_stack.top - 1].value);
			sqlDropStatement.IsDropIfExists = (bool)value_stack.array[value_stack.top - 3].value;
			sqlDropStatement.Position = yyloc;
			yyval.value = sqlDropStatement;
			break;
		}
		case 1689:
			yyval.value = SqlDropTriggerTargetScope.None;
			break;
		case 1690:
			yyval.value = SqlDropTriggerTargetScope.Database;
			break;
		case 1691:
		{
			bool flag = ExpectKeyword(StringToken.Server, location_stack.array[location_stack.top - 1]);
			yyval.value = (flag ? SqlDropTriggerTargetScope.AllServer : SqlDropTriggerTargetScope.None);
			break;
		}
		case 1692:
			yyval.value = Enum.Parse(typeof(SqlDropObjectType), GetText(location_stack.array[location_stack.top - 1]), ignoreCase: true);
			break;
		case 1693:
			yyval.value = Enum.Parse(typeof(SqlDropObjectType), GetText(location_stack.array[location_stack.top - 1]), ignoreCase: true);
			break;
		case 1694:
			yyval.value = Enum.Parse(typeof(SqlDropObjectType), GetText(location_stack.array[location_stack.top - 1]), ignoreCase: true);
			break;
		case 1695:
			yyval.value = Enum.Parse(typeof(SqlDropObjectType), GetText(location_stack.array[location_stack.top - 1]), ignoreCase: true);
			break;
		case 1696:
			yyval.value = Enum.Parse(typeof(SqlDropObjectType), GetText(location_stack.array[location_stack.top - 1]), ignoreCase: true);
			break;
		case 1697:
			yyval.value = Enum.Parse(typeof(SqlDropObjectType), GetText(location_stack.array[location_stack.top - 1]), ignoreCase: true);
			break;
		case 1698:
			yyval.value = Enum.Parse(typeof(SqlDropObjectType), GetText(location_stack.array[location_stack.top - 1]), ignoreCase: true);
			break;
		case 1699:
			yyval.value = Enum.Parse(typeof(SqlDropObjectType), GetText(location_stack.array[location_stack.top - 1]), ignoreCase: true);
			break;
		case 1654:
		case 1655:
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
		case 1672:
		case 1673:
		case 1674:
		case 1675:
		case 1676:
		case 1677:
		case 1678:
		case 1679:
		case 1680:
		case 1681:
		case 1682:
		case 1683:
		case 1684:
		case 1685:
		case 1686:
		case 1687:
			break;
		}
	}

	private void DoAction_1700_1749(int action)
	{
		switch (action)
		{
		case 1700:
			yyval.value = Enum.Parse(typeof(SqlDropObjectType), GetText(location_stack.array[location_stack.top - 1]), ignoreCase: true);
			break;
		case 1701:
			yyval.value = SqlDropObjectType.Sequence;
			break;
		case 1702:
			yyval.value = SqlDropObjectType.SecurityPolicy;
			break;
		case 1709:
		{
			List<SqlObjectIdentifier> list6 = new List<SqlObjectIdentifier>();
			list6.Add(value_stack.array[value_stack.top - 1].value as SqlObjectIdentifier);
			yyval.value = list6;
			break;
		}
		case 1710:
		{
			List<SqlObjectIdentifier> list5 = value_stack.array[value_stack.top - 3].value as List<SqlObjectIdentifier>;
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlObjectIdentifier);
			yyval.value = list5;
			break;
		}
		case 1715:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1716:
			scanner.SetParseState(EParseState.e_ParseWith);
			break;
		case 1717:
		{
			yyval = value_stack.array[value_stack.top - 1];
			SqlQueryWithClause sqlQueryWithClause5 = yyval.value as SqlQueryWithClause;
			sqlQueryWithClause5.Position = yyloc;
			yyval.value = sqlQueryWithClause5;
			break;
		}
		case 1718:
		{
			SqlQueryWithClause sqlQueryWithClause4 = new SqlQueryWithClause(value_stack.array[value_stack.top - 1].value as List<SqlCommonTableExpression>);
			sqlQueryWithClause4.Position = yyloc;
			yyval.value = sqlQueryWithClause4;
			break;
		}
		case 1719:
		{
			SqlQueryWithClause sqlQueryWithClause3 = new SqlQueryWithClause(new SqlXmlNamespacesDeclaration
			{
				Position = MergeLocationRange(location_stack.top - 5, location_stack.top - 2)
			}, value_stack.array[value_stack.top - 1].value as List<SqlCommonTableExpression>);
			sqlQueryWithClause3.Position = yyloc;
			yyval.value = sqlQueryWithClause3;
			break;
		}
		case 1720:
		{
			SqlQueryWithClause sqlQueryWithClause2 = new SqlQueryWithClause(value_stack.array[value_stack.top - 2].value as SqlChangeTrackingContext, null, value_stack.array[value_stack.top - 1].value as List<SqlCommonTableExpression>);
			sqlQueryWithClause2.Position = yyloc;
			yyval.value = sqlQueryWithClause2;
			break;
		}
		case 1721:
		{
			SqlXmlNamespacesDeclaration sqlXmlNamespacesDeclaration = new SqlXmlNamespacesDeclaration();
			sqlXmlNamespacesDeclaration.Position = MergeLocationRange(location_stack.top - 5, location_stack.top - 2);
			SqlQueryWithClause sqlQueryWithClause = new SqlQueryWithClause(value_stack.array[value_stack.top - 7].value as SqlChangeTrackingContext, sqlXmlNamespacesDeclaration, value_stack.array[value_stack.top - 1].value as List<SqlCommonTableExpression>);
			sqlQueryWithClause.Position = yyloc;
			yyval.value = sqlQueryWithClause;
			break;
		}
		case 1726:
		{
			List<SqlCommonTableExpression> list4 = new List<SqlCommonTableExpression>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlCommonTableExpression);
			yyval.value = list4;
			break;
		}
		case 1727:
		{
			List<SqlCommonTableExpression> list3 = value_stack.array[value_stack.top - 3].value as List<SqlCommonTableExpression>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlCommonTableExpression);
			yyval.value = list3;
			break;
		}
		case 1728:
		{
			SqlIdentifier sqlIdentifier4 = new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlIdentifier4.Position = yyloc;
			yyval.value = sqlIdentifier4;
			break;
		}
		case 1729:
		{
			SqlIdentifier sqlIdentifier3 = new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlIdentifier3.Position = yyloc;
			yyval.value = sqlIdentifier3;
			break;
		}
		case 1730:
		{
			SqlCommonTableExpression sqlCommonTableExpression = new SqlCommonTableExpression(value_stack.array[value_stack.top - 6].value as SqlIdentifier, value_stack.array[value_stack.top - 5].value as List<SqlIdentifier>, value_stack.array[value_stack.top - 2].value as SqlQueryExpression);
			sqlCommonTableExpression.Position = yyloc;
			yyval.value = sqlCommonTableExpression;
			scanner.SetParseState(EParseState.e_ParseAfterWith);
			break;
		}
		case 1731:
		{
			SqlChangeTrackingContext sqlChangeTrackingContext = new SqlChangeTrackingContext();
			sqlChangeTrackingContext.Position = yyloc;
			yyval.value = sqlChangeTrackingContext;
			break;
		}
		case 1732:
		{
			SqlInsertStatement sqlInsertStatement2 = new SqlInsertStatement(value_stack.array[value_stack.top - 1].value as SqlInsertSpecification);
			sqlInsertStatement2.Position = yyloc;
			yyval.value = sqlInsertStatement2;
			break;
		}
		case 1733:
		{
			SqlInsertStatement sqlInsertStatement = new SqlInsertStatement(value_stack.array[value_stack.top - 2].value as SqlQueryWithClause, value_stack.array[value_stack.top - 1].value as SqlInsertSpecification);
			sqlInsertStatement.Position = yyloc;
			yyval.value = sqlInsertStatement;
			break;
		}
		case 1734:
		{
			SqlInsertSpecification sqlInsertSpecification = new SqlInsertSpecification(value_stack.array[value_stack.top - 5].value as SqlTopSpecification, value_stack.array[value_stack.top - 3].value as DmlTarget, value_stack.array[value_stack.top - 2].value as List<SqlOutputClause>, value_stack.array[value_stack.top - 1].value as SqlInsertSource);
			sqlInsertSpecification.Position = yyloc;
			yyval.value = sqlInsertSpecification;
			break;
		}
		case 1739:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 1740:
		{
			List<SqlColumnRefExpression> list2 = new List<SqlColumnRefExpression>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlColumnRefExpression);
			yyval.value = list2;
			break;
		}
		case 1741:
		{
			List<SqlColumnRefExpression> list = value_stack.array[value_stack.top - 3].value as List<SqlColumnRefExpression>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlColumnRefExpression);
			yyval.value = list;
			break;
		}
		case 1742:
		{
			SqlObjectIdentifier sqlObjectIdentifier4 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier4.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression4 = new SqlColumnRefExpression(sqlObjectIdentifier4);
			sqlColumnRefExpression4.Position = yyloc;
			yyval.value = sqlColumnRefExpression4;
			break;
		}
		case 1743:
		{
			List<SqlIdentifier> obj2 = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			obj2.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			SqlObjectIdentifier sqlObjectIdentifier3 = SqlObjectIdentifier.Create(obj2);
			sqlObjectIdentifier3.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression3 = new SqlColumnRefExpression(sqlObjectIdentifier3, discardColumnPrefix: true);
			sqlColumnRefExpression3.Position = yyloc;
			yyval.value = sqlColumnRefExpression3;
			break;
		}
		case 1745:
		{
			SqlIdentifier sqlIdentifier2 = SqlIdentifier.Missing();
			sqlIdentifier2.Position = yyloc;
			SqlObjectIdentifier sqlObjectIdentifier2 = SqlObjectIdentifier.Create(sqlIdentifier2);
			sqlObjectIdentifier2.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression2 = new SqlColumnRefExpression(sqlObjectIdentifier2);
			sqlColumnRefExpression2.Position = yyloc;
			yyval.value = sqlColumnRefExpression2;
			break;
		}
		case 1746:
		{
			SqlIdentifier sqlIdentifier = SqlIdentifier.Missing();
			sqlIdentifier.Position = location_stack.array[location_stack.top - 1];
			List<SqlIdentifier> obj = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			obj.Add(sqlIdentifier);
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(obj);
			sqlObjectIdentifier.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression = new SqlColumnRefExpression(sqlObjectIdentifier, discardColumnPrefix: true);
			sqlColumnRefExpression.Position = yyloc;
			yyval.value = sqlColumnRefExpression;
			break;
		}
		case 1747:
		{
			SqlDefaultValuesInsertSource sqlDefaultValuesInsertSource = new SqlDefaultValuesInsertSource();
			sqlDefaultValuesInsertSource.Position = yyloc;
			yyval.value = sqlDefaultValuesInsertSource;
			break;
		}
		case 1748:
		{
			SqlTableConstructorInsertSource sqlTableConstructorInsertSource = new SqlTableConstructorInsertSource(value_stack.array[value_stack.top - 2].value as SqlTableConstructorExpression);
			sqlTableConstructorInsertSource.Position = yyloc;
			yyval.value = sqlTableConstructorInsertSource;
			break;
		}
		case 1749:
		{
			SqlSelectSpecificationInsertSource sqlSelectSpecificationInsertSource = new SqlSelectSpecificationInsertSource(value_stack.array[value_stack.top - 1].value as SqlSelectSpecification);
			sqlSelectSpecificationInsertSource.Position = yyloc;
			yyval.value = sqlSelectSpecificationInsertSource;
			break;
		}
		case 1703:
		case 1704:
		case 1705:
		case 1706:
		case 1707:
		case 1708:
		case 1711:
		case 1712:
		case 1713:
		case 1714:
		case 1722:
		case 1723:
		case 1724:
		case 1725:
		case 1735:
		case 1736:
		case 1737:
		case 1738:
		case 1744:
			break;
		}
	}

	private void DoAction_1750_1799(int action)
	{
		switch (action)
		{
		case 1750:
		{
			SqlNullInsertSource sqlNullInsertSource = new SqlNullInsertSource();
			sqlNullInsertSource.Position = yyloc;
			yyval.value = sqlNullInsertSource;
			break;
		}
		case 1751:
		{
			SqlMissingInsertSource sqlMissingInsertSource = new SqlMissingInsertSource();
			sqlMissingInsertSource.Position = yyloc;
			yyval.value = sqlMissingInsertSource;
			break;
		}
		case 1752:
		{
			List<SqlScalarExpression> list8 = new List<SqlScalarExpression>();
			list8.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list8;
			break;
		}
		case 1753:
		{
			List<SqlScalarExpression> list7 = value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>;
			list7.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list7;
			break;
		}
		case 1758:
		{
			List<SqlOutputClause> list6 = new List<SqlOutputClause>();
			list6.Add(value_stack.array[value_stack.top - 1].value as SqlOutputClause);
			yyval.value = list6;
			break;
		}
		case 1759:
		{
			List<SqlOutputClause> list5 = new List<SqlOutputClause>();
			list5.Add(value_stack.array[value_stack.top - 2].value as SqlOutputClause);
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlOutputClause);
			yyval.value = list5;
			break;
		}
		case 1760:
		{
			SqlOutputIntoClause sqlOutputIntoClause = new SqlOutputIntoClause(value_stack.array[value_stack.top - 2].value as SqlOutputClause, value_stack.array[value_stack.top - 1].value as SqlIntoClause);
			sqlOutputIntoClause.Position = yyloc;
			yyval.value = sqlOutputIntoClause;
			break;
		}
		case 1763:
		{
			SqlOutputClause sqlOutputClause = new SqlOutputClause(value_stack.array[value_stack.top - 1].value as List<SqlSelectExpression>);
			sqlOutputClause.Position = yyloc;
			yyval.value = sqlOutputClause;
			break;
		}
		case 1764:
		{
			List<SqlSelectExpression> list4 = new List<SqlSelectExpression>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlSelectExpression);
			yyval.value = list4;
			break;
		}
		case 1765:
		{
			List<SqlSelectExpression> list3 = value_stack.array[value_stack.top - 3].value as List<SqlSelectExpression>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlSelectExpression);
			yyval.value = list3;
			break;
		}
		case 1766:
		{
			SqlSelectScalarExpression sqlSelectScalarExpression = new SqlSelectScalarExpression(value_stack.array[value_stack.top - 2].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlSelectScalarExpression.Position = yyloc;
			yyval.value = sqlSelectScalarExpression;
			break;
		}
		case 1767:
		{
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>);
			sqlObjectIdentifier.Position = location_stack.array[location_stack.top - 2];
			SqlSelectStarExpression sqlSelectStarExpression = new SqlSelectStarExpression(sqlObjectIdentifier);
			sqlSelectStarExpression.Position = yyloc;
			yyval.value = sqlSelectStarExpression;
			break;
		}
		case 1771:
		{
			SqlIntoClause sqlIntoClause2 = new SqlIntoClause(new SqlTableVariableRefExpression(GetText(location_stack.array[location_stack.top - 2]))
			{
				Position = location_stack.array[location_stack.top - 2]
			}, value_stack.array[value_stack.top - 1].value as List<SqlColumnRefExpression>);
			sqlIntoClause2.Position = yyloc;
			yyval.value = sqlIntoClause2;
			break;
		}
		case 1772:
		{
			SqlIntoClause sqlIntoClause = new SqlIntoClause(new SqlTableRefExpression(value_stack.array[value_stack.top - 2].value as SqlObjectIdentifier)
			{
				Position = location_stack.array[location_stack.top - 2]
			}, value_stack.array[value_stack.top - 1].value as List<SqlColumnRefExpression>);
			sqlIntoClause.Position = yyloc;
			yyval.value = sqlIntoClause;
			break;
		}
		case 1797:
		{
			List<SqlSimpleOrderByItem> list2 = new List<SqlSimpleOrderByItem>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlSimpleOrderByItem);
			yyval.value = list2;
			break;
		}
		case 1798:
		{
			List<SqlSimpleOrderByItem> list = value_stack.array[value_stack.top - 3].value as List<SqlSimpleOrderByItem>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlSimpleOrderByItem);
			yyval.value = list;
			break;
		}
		case 1799:
		{
			SqlSimpleOrderByItem sqlSimpleOrderByItem = new SqlSimpleOrderByItem(value_stack.array[value_stack.top - 2].value as SqlIdentifier, (SqlSortOrder)value_stack.array[value_stack.top - 1].value);
			sqlSimpleOrderByItem.Position = yyloc;
			yyval.value = sqlSimpleOrderByItem;
			break;
		}
		case 1754:
		case 1755:
		case 1756:
		case 1757:
		case 1761:
		case 1762:
		case 1768:
		case 1769:
		case 1770:
		case 1773:
		case 1774:
		case 1775:
		case 1776:
		case 1777:
		case 1778:
		case 1779:
		case 1780:
		case 1781:
		case 1782:
		case 1783:
		case 1784:
		case 1785:
		case 1786:
		case 1787:
		case 1788:
		case 1789:
		case 1790:
		case 1791:
		case 1792:
		case 1793:
		case 1794:
		case 1795:
		case 1796:
			break;
		}
	}

	private void DoAction_1800_1849(int action)
	{
		switch (action)
		{
		case 1800:
			VersionedParserBase.ReportError(SR.MergeStatementMustTerminated, new LexLocation(location_stack.array[location_stack.top - 1].endTokenNumber, location_stack.array[location_stack.top - 1].endTokenNumber));
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1801:
			yyval = value_stack.array[value_stack.top - 3];
			break;
		case 1802:
		{
			SqlMergeStatement sqlMergeStatement2 = new SqlMergeStatement(value_stack.array[value_stack.top - 1].value as SqlMergeSpecification);
			sqlMergeStatement2.Position = yyloc;
			yyval.value = sqlMergeStatement2;
			break;
		}
		case 1803:
		{
			SqlMergeStatement sqlMergeStatement = new SqlMergeStatement(value_stack.array[value_stack.top - 2].value as SqlQueryWithClause, value_stack.array[value_stack.top - 1].value as SqlMergeSpecification);
			sqlMergeStatement.Position = yyloc;
			yyval.value = sqlMergeStatement;
			break;
		}
		case 1804:
		{
			SqlConditionClause sqlConditionClause = new SqlConditionClause(value_stack.array[value_stack.top - 4].value as SqlBooleanExpression);
			sqlConditionClause.Position = LexLocation.Merge(location_stack.array[location_stack.top - 5], location_stack.array[location_stack.top - 4]);
			SqlMergeSpecification sqlMergeSpecification = new SqlMergeSpecification(value_stack.array[value_stack.top - 11].value as SqlTopSpecification, value_stack.array[value_stack.top - 9].value as DmlTarget, value_stack.array[value_stack.top - 8].value as SqlIdentifier, value_stack.array[value_stack.top - 6].value as SqlTableExpression, sqlConditionClause, value_stack.array[value_stack.top - 3].value as List<SqlMergeActionClause>, value_stack.array[value_stack.top - 2].value as List<SqlOutputClause>);
			sqlMergeSpecification.Position = yyloc;
			yyval.value = sqlMergeSpecification;
			scanner.ClearQueryStatus(QueryStatus.Q_MERGE);
			break;
		}
		case 1809:
		{
			List<SqlMergeActionClause> list4 = new List<SqlMergeActionClause>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlMergeActionClause);
			yyval.value = list4;
			break;
		}
		case 1810:
		{
			List<SqlMergeActionClause> list3 = value_stack.array[value_stack.top - 2].value as List<SqlMergeActionClause>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlMergeActionClause);
			yyval.value = list3;
			break;
		}
		case 1811:
		{
			SqlMergeActionClause sqlMergeActionClause2 = new SqlMergeActionClause((SqlMergeConditionType)value_stack.array[value_stack.top - 3].value, value_stack.array[value_stack.top - 1].value as SqlMergeAction);
			sqlMergeActionClause2.Position = yyloc;
			yyval.value = sqlMergeActionClause2;
			break;
		}
		case 1812:
		{
			SqlMergeActionClause sqlMergeActionClause = new SqlMergeActionClause((SqlMergeConditionType)value_stack.array[value_stack.top - 5].value, value_stack.array[value_stack.top - 3].value as SqlBooleanExpression, value_stack.array[value_stack.top - 1].value as SqlMergeAction);
			sqlMergeActionClause.Position = yyloc;
			yyval.value = sqlMergeActionClause;
			break;
		}
		case 1813:
			ExpectKeyword(StringToken.Matched, location_stack.array[location_stack.top - 1]);
			yyval.value = SqlMergeConditionType.Matched;
			break;
		case 1814:
			ExpectKeyword(StringToken.Matched, location_stack.array[location_stack.top - 1]);
			yyval.value = SqlMergeConditionType.TargetNotMatched;
			break;
		case 1815:
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
		case 1816:
		{
			SqlUpdateMergeAction sqlUpdateMergeAction = new SqlUpdateMergeAction(new SqlSetClause(value_stack.array[value_stack.top - 1].value as List<SqlAssignment>)
			{
				Position = LexLocation.Merge(location_stack.array[location_stack.top - 2], location_stack.array[location_stack.top - 1])
			});
			sqlUpdateMergeAction.Position = yyloc;
			yyval.value = sqlUpdateMergeAction;
			break;
		}
		case 1817:
		{
			SqlInsertMergeAction sqlInsertMergeAction = new SqlInsertMergeAction(value_stack.array[value_stack.top - 2].value as List<SqlColumnRefExpression>, value_stack.array[value_stack.top - 1].value as SqlInsertMergeActionSource);
			sqlInsertMergeAction.Position = yyloc;
			yyval.value = sqlInsertMergeAction;
			break;
		}
		case 1818:
		{
			SqlDeleteMergeAction sqlDeleteMergeAction = new SqlDeleteMergeAction();
			sqlDeleteMergeAction.Position = yyloc;
			yyval.value = sqlDeleteMergeAction;
			break;
		}
		case 1819:
		{
			SqlDefaultValuesInsertMergeActionSource sqlDefaultValuesInsertMergeActionSource = new SqlDefaultValuesInsertMergeActionSource();
			sqlDefaultValuesInsertMergeActionSource.Position = yyloc;
			yyval.value = sqlDefaultValuesInsertMergeActionSource;
			break;
		}
		case 1820:
		{
			SqlValuesInsertMergeActionSource sqlValuesInsertMergeActionSource = new SqlValuesInsertMergeActionSource(value_stack.array[value_stack.top - 2].value as List<SqlScalarExpression>);
			sqlValuesInsertMergeActionSource.Position = yyloc;
			yyval.value = sqlValuesInsertMergeActionSource;
			break;
		}
		case 1821:
		{
			SqlUpdateStatement sqlUpdateStatement2 = new SqlUpdateStatement(value_stack.array[value_stack.top - 1].value as SqlUpdateSpecification);
			sqlUpdateStatement2.Position = yyloc;
			yyval.value = sqlUpdateStatement2;
			break;
		}
		case 1822:
		{
			SqlUpdateStatement sqlUpdateStatement = new SqlUpdateStatement(value_stack.array[value_stack.top - 2].value as SqlQueryWithClause, value_stack.array[value_stack.top - 1].value as SqlUpdateSpecification);
			sqlUpdateStatement.Position = yyloc;
			yyval.value = sqlUpdateStatement;
			break;
		}
		case 1823:
		{
			SqlUpdateSpecification sqlUpdateSpecification = new SqlUpdateSpecification(value_stack.array[value_stack.top - 7].value as SqlTopSpecification, value_stack.array[value_stack.top - 6].value as DmlTarget, value_stack.array[value_stack.top - 5].value as SqlSetClause, value_stack.array[value_stack.top - 4].value as List<SqlOutputClause>, value_stack.array[value_stack.top - 3].value as SqlFromClause, value_stack.array[value_stack.top - 2].value as SqlWhereClause);
			sqlUpdateSpecification.Position = yyloc;
			yyval.value = sqlUpdateSpecification;
			break;
		}
		case 1824:
		{
			List<SqlAssignment> list2 = new List<SqlAssignment>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlAssignment);
			yyval.value = list2;
			break;
		}
		case 1825:
		{
			List<SqlAssignment> list = value_stack.array[value_stack.top - 3].value as List<SqlAssignment>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlAssignment);
			yyval.value = list;
			break;
		}
		case 1826:
			yyval = value_stack.array[value_stack.top - 1];
			if (value_stack.array[value_stack.top - 1].value == null)
			{
				SqlNullAssignment sqlNullAssignment = new SqlNullAssignment(ShiftReduceParser.UpdatePos(new SqlNullScalarExpression(), yyloc));
				sqlNullAssignment.Position = yyloc;
				yyval.value = sqlNullAssignment;
			}
			break;
		case 1827:
		{
			SqlScalarVariableAssignment sqlScalarVariableAssignment = new SqlScalarVariableAssignment(value_stack.array[value_stack.top - 3].value as SqlScalarVariableRefExpression, (SqlAssignmentOperatorType)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlScalarVariableAssignment.Position = yyloc;
			yyval.value = sqlScalarVariableAssignment;
			break;
		}
		case 1828:
		{
			SqlVariableColumnAssignment sqlVariableColumnAssignment = new SqlVariableColumnAssignment(value_stack.array[value_stack.top - 3].value as SqlScalarVariableRefExpression, value_stack.array[value_stack.top - 1].value as SqlColumnAssignment);
			sqlVariableColumnAssignment.Position = yyloc;
			yyval.value = sqlVariableColumnAssignment;
			break;
		}
		case 1829:
		{
			SqlColumnAssignment sqlColumnAssignment = new SqlColumnAssignment(value_stack.array[value_stack.top - 3].value as SqlColumnRefExpression, (SqlAssignmentOperatorType)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlColumnAssignment.Position = yyloc;
			yyval.value = sqlColumnAssignment;
			break;
		}
		case 1830:
		{
			SqlObjectIdentifier sqlObjectIdentifier6 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier6.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression4 = new SqlColumnRefExpression(sqlObjectIdentifier6);
			sqlColumnRefExpression4.Position = yyloc;
			yyval.value = sqlColumnRefExpression4;
			break;
		}
		case 1831:
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
		case 1834:
			yyval.value = SqlAssignmentOperatorType.Assignment;
			break;
		case 1835:
			yyval.value = SqlAssignmentOperatorType.AdditionAssignment;
			break;
		case 1836:
			yyval.value = SqlAssignmentOperatorType.SubtractionAssignment;
			break;
		case 1837:
			yyval.value = SqlAssignmentOperatorType.MultiplicationAssignment;
			break;
		case 1838:
			yyval.value = SqlAssignmentOperatorType.DivisionAssignment;
			break;
		case 1839:
			yyval.value = SqlAssignmentOperatorType.ModulusAssignment;
			break;
		case 1840:
			yyval.value = SqlAssignmentOperatorType.BitwiseAndAssignment;
			break;
		case 1841:
			yyval.value = SqlAssignmentOperatorType.BitwiseOrAssignment;
			break;
		case 1842:
			yyval.value = SqlAssignmentOperatorType.BitwiseXorAssignment;
			break;
		case 1843:
		{
			SqlObjectIdentifier sqlObjectIdentifier4 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier4.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression2 = new SqlColumnRefExpression(sqlObjectIdentifier4);
			sqlColumnRefExpression2.Position = yyloc;
			yyval.value = sqlColumnRefExpression2;
			break;
		}
		case 1844:
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
		case 1846:
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
		case 1847:
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
		case 1805:
		case 1806:
		case 1807:
		case 1808:
		case 1832:
		case 1833:
		case 1845:
		case 1848:
		case 1849:
			break;
		}
	}

	private void DoAction_1850_1899(int action)
	{
		switch (action)
		{
		case 1852:
		{
			SqlColumnAssignment sqlColumnAssignment = new SqlColumnAssignment(value_stack.array[value_stack.top - 3].value as SqlScalarRefExpression, (SqlAssignmentOperatorType)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlColumnAssignment.Position = yyloc;
			yyval.value = sqlColumnAssignment;
			break;
		}
		case 1853:
		{
			SqlSelectStatement sqlSelectStatement2 = new SqlSelectStatement(value_stack.array[value_stack.top - 1].value as SqlSelectSpecification);
			sqlSelectStatement2.Position = yyloc;
			yyval.value = sqlSelectStatement2;
			break;
		}
		case 1854:
		{
			SqlSelectStatement sqlSelectStatement = new SqlSelectStatement(value_stack.array[value_stack.top - 2].value as SqlQueryWithClause, value_stack.array[value_stack.top - 1].value as SqlSelectSpecification);
			sqlSelectStatement.Position = yyloc;
			yyval.value = sqlSelectStatement;
			break;
		}
		case 1855:
		{
			SqlSelectSpecification sqlSelectSpecification = new SqlSelectSpecification(value_stack.array[value_stack.top - 3].value as SqlQueryExpression, value_stack.array[value_stack.top - 2].value as SqlOrderByClause, value_stack.array[value_stack.top - 1].value as SqlForClause);
			sqlSelectSpecification.Position = yyloc;
			yyval.value = sqlSelectSpecification;
			break;
		}
		case 1857:
		{
			SqlBinaryQueryExpression sqlBinaryQueryExpression2 = new SqlBinaryQueryExpression(value_stack.array[value_stack.top - 4].value as SqlQueryExpression, (SqlBinaryQueryOperatorType)value_stack.array[value_stack.top - 3].value, (bool)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlBinaryQueryExpression2.Position = yyloc;
			yyval.value = sqlBinaryQueryExpression2;
			break;
		}
		case 1858:
			yyval.value = SqlBinaryQueryOperatorType.Union;
			break;
		case 1859:
			yyval.value = SqlBinaryQueryOperatorType.Except;
			break;
		case 1861:
		{
			SqlBinaryQueryExpression sqlBinaryQueryExpression = new SqlBinaryQueryExpression(value_stack.array[value_stack.top - 4].value as SqlQueryExpression, SqlBinaryQueryOperatorType.Intersect, (bool)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlBinaryQueryExpression.Position = yyloc;
			yyval.value = sqlBinaryQueryExpression;
			break;
		}
		case 1862:
			yyval.value = false;
			break;
		case 1863:
			yyval.value = true;
			break;
		case 1865:
			if (value_stack.array[value_stack.top - 2].value is SqlCodeObject sqlCodeObject)
			{
				sqlCodeObject.Position = yyloc;
				yyval.value = sqlCodeObject;
			}
			break;
		case 1867:
		{
			SqlQuerySpecification sqlQuerySpecification = new SqlQuerySpecification(new SqlSelectClause(value_stack.array[value_stack.top - 8].value != null, value_stack.array[value_stack.top - 7].value as SqlTopSpecification, value_stack.array[value_stack.top - 6].value as List<SqlSelectExpression>)
			{
				Position = MergeLocationRange(location_stack.top - 9, location_stack.top - 6)
			}, value_stack.array[value_stack.top - 5].value as SqlSelectIntoClause, value_stack.array[value_stack.top - 4].value as SqlFromClause, value_stack.array[value_stack.top - 3].value as SqlWhereClause, value_stack.array[value_stack.top - 2].value as SqlGroupByClause, value_stack.array[value_stack.top - 1].value as SqlHavingClause);
			sqlQuerySpecification.Position = yyloc;
			yyval.value = sqlQuerySpecification;
			break;
		}
		case 1869:
		{
			SqlSelectIntoClause sqlSelectIntoClause = new SqlSelectIntoClause(value_stack.array[value_stack.top - 2].value as SqlObjectIdentifier);
			sqlSelectIntoClause.Position = yyloc;
			yyval.value = sqlSelectIntoClause;
			break;
		}
		case 1872:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 1873:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1874:
		{
			SqlForBrowseClause sqlForBrowseClause = new SqlForBrowseClause();
			sqlForBrowseClause.Position = yyloc;
			yyval.value = sqlForBrowseClause;
			break;
		}
		case 1850:
		case 1851:
		case 1856:
		case 1860:
		case 1864:
		case 1866:
		case 1868:
		case 1870:
		case 1871:
		case 1875:
		case 1876:
		case 1877:
		case 1878:
		case 1879:
		case 1880:
		case 1881:
		case 1882:
		case 1883:
		case 1884:
		case 1885:
		case 1886:
		case 1887:
		case 1888:
		case 1889:
		case 1890:
		case 1891:
		case 1892:
		case 1893:
		case 1894:
		case 1895:
		case 1896:
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
		case 1916:
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
		case 1917:
		{
			List<SqlForXmlUnknownDirective> list2 = new List<SqlForXmlUnknownDirective>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlForXmlUnknownDirective);
			yyval.value = list2;
			break;
		}
		case 1918:
		{
			List<SqlForXmlUnknownDirective> list = value_stack.array[value_stack.top - 3].value as List<SqlForXmlUnknownDirective>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlForXmlUnknownDirective);
			yyval.value = list;
			break;
		}
		case 1919:
		{
			SqlForXmlUnknownDirective sqlForXmlUnknownDirective3 = new SqlForXmlUnknownDirective(GetText(location_stack.array[location_stack.top - 1]), null);
			sqlForXmlUnknownDirective3.Position = yyloc;
			yyval.value = sqlForXmlUnknownDirective3;
			break;
		}
		case 1920:
		{
			SqlForXmlUnknownDirective sqlForXmlUnknownDirective2 = new SqlForXmlUnknownDirective(GetText(location_stack.array[location_stack.top - 2]) + " " + GetText(location_stack.array[location_stack.top - 1]), null);
			sqlForXmlUnknownDirective2.Position = yyloc;
			yyval.value = sqlForXmlUnknownDirective2;
			break;
		}
		case 1921:
		{
			SqlForXmlUnknownDirective sqlForXmlUnknownDirective = new SqlForXmlUnknownDirective(GetText(location_stack.array[location_stack.top - 4]), GetText(location_stack.array[location_stack.top - 2]));
			sqlForXmlUnknownDirective.Position = yyloc;
			yyval.value = sqlForXmlUnknownDirective;
			break;
		}
		case 1928:
			yyval.value = true;
			break;
		case 1930:
		{
			SqlOffsetFetchClause sqlOffsetFetchClause = new SqlOffsetFetchClause(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlOffsetFetchClause.Position = yyloc;
			yyval.value = sqlOffsetFetchClause;
			ExpectKeyword(StringToken.Offset, location_stack.array[location_stack.top - 4]);
			break;
		}
		case 1932:
			ExpectKeywords(new StringToken[2]
			{
				StringToken.First,
				StringToken.Next
			}, location_stack.array[location_stack.top - 4]);
			ExpectKeyword(StringToken.Only, location_stack.array[location_stack.top - 1]);
			yyval = value_stack.array[value_stack.top - 3];
			break;
		case 1933:
			ExpectKeywords(new StringToken[2]
			{
				StringToken.Row,
				StringToken.Rows
			}, location_stack.array[location_stack.top - 1]);
			break;
		case 1934:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1935:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1937:
		{
			SqlTopSpecification sqlTopSpecification2 = new SqlTopSpecification(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, (bool)value_stack.array[value_stack.top - 2].value, (bool)value_stack.array[value_stack.top - 1].value);
			sqlTopSpecification2.Position = yyloc;
			yyval.value = sqlTopSpecification2;
			break;
		}
		case 1938:
		{
			SqlLiteralExpression sqlLiteralExpression3 = SqlLiteralExpression.Real(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression3.Position = yyloc;
			yyval.value = sqlLiteralExpression3;
			break;
		}
		case 1939:
		{
			SqlLiteralExpression sqlLiteralExpression2 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression2.Position = yyloc;
			yyval.value = sqlLiteralExpression2;
			break;
		}
		case 1940:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Numeric(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression.Position = yyloc;
			yyval.value = sqlLiteralExpression;
			break;
		}
		case 1943:
		{
			SqlTopSpecification sqlTopSpecification = new SqlTopSpecification(value_stack.array[value_stack.top - 2].value as SqlScalarExpression, (bool)value_stack.array[value_stack.top - 1].value);
			sqlTopSpecification.Position = yyloc;
			yyval.value = sqlTopSpecification;
			break;
		}
		case 1944:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 1945:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1946:
			yyval.value = false;
			break;
		case 1947:
			yyval.value = true;
			break;
		case 1948:
			yyval.value = false;
			break;
		case 1949:
			yyval.value = true;
			break;
		case 1900:
		case 1901:
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
		case 1922:
		case 1923:
		case 1924:
		case 1925:
		case 1926:
		case 1927:
		case 1929:
		case 1931:
		case 1936:
		case 1941:
		case 1942:
			break;
		}
	}

	private void DoAction_1950_1999(int action)
	{
		switch (action)
		{
		case 1950:
		{
			SqlDeleteStatement sqlDeleteStatement = new SqlDeleteStatement(value_stack.array[value_stack.top - 1].value as SqlDeleteSpecification);
			sqlDeleteStatement.Position = yyloc;
			yyval.value = sqlDeleteStatement;
			break;
		}
		case 1951:
		{
			SqlDeleteStatement sqlDeleteStatement2 = new SqlDeleteStatement(value_stack.array[value_stack.top - 2].value as SqlQueryWithClause, value_stack.array[value_stack.top - 1].value as SqlDeleteSpecification);
			sqlDeleteStatement2.Position = yyloc;
			yyval.value = sqlDeleteStatement2;
			break;
		}
		case 1952:
		{
			SqlDeleteSpecification sqlDeleteSpecification = new SqlDeleteSpecification(value_stack.array[value_stack.top - 7].value as SqlTopSpecification, value_stack.array[value_stack.top - 5].value as DmlTarget, value_stack.array[value_stack.top - 4].value as List<SqlOutputClause>, value_stack.array[value_stack.top - 3].value as SqlFromClause, value_stack.array[value_stack.top - 2].value as SqlWhereClause);
			sqlDeleteSpecification.Position = yyloc;
			yyval.value = sqlDeleteSpecification;
			break;
		}
		case 1955:
			if (value_stack.array[value_stack.top - 1].value is SqlExecuteStatement sqlExecuteStatement)
			{
				sqlExecuteStatement.Position = yyloc;
				yyval.value = sqlExecuteStatement;
			}
			break;
		case 1956:
		{
			SqlExecuteStringStatement sqlExecuteStringStatement2 = value_stack.array[value_stack.top - 5].value as SqlExecuteStringStatement;
			sqlExecuteStringStatement2.Position = yyloc;
			yyval.value = sqlExecuteStringStatement2;
			break;
		}
		case 1958:
		{
			SqlExecuteModuleStatement sqlExecuteModuleStatement2 = value_stack.array[value_stack.top - 1].value as SqlExecuteModuleStatement;
			sqlExecuteModuleStatement2.ReturnValue = ShiftReduceParser.UpdatePos(SqlScalarVariableRefExpression.Create(value_stack.array[value_stack.top - 3].value as string), location_stack.array[location_stack.top - 3]);
			sqlExecuteModuleStatement2.Position = yyloc;
			yyval.value = sqlExecuteModuleStatement2;
			break;
		}
		case 1964:
		{
			SqlExecuteStringStatement sqlExecuteStringStatement = new SqlExecuteStringStatement(value_stack.array[value_stack.top - 2].value as List<SqlScalarExpression>, value_stack.array[value_stack.top - 1].value as List<SqlExecuteArgument>);
			sqlExecuteStringStatement.Position = yyloc;
			yyval.value = sqlExecuteStringStatement;
			break;
		}
		case 1968:
		{
			SqlExecuteModuleStatement sqlExecuteModuleStatement = new SqlExecuteModuleStatement(value_stack.array[value_stack.top - 4].value as SqlObjectReference, value_stack.array[value_stack.top - 3].value as SqlLiteralExpression, value_stack.array[value_stack.top - 2].value as List<SqlExecuteArgument>, value_stack.array[value_stack.top - 1].value as SqlModuleOption);
			sqlExecuteModuleStatement.Position = yyloc;
			yyval.value = sqlExecuteModuleStatement;
			break;
		}
		case 1971:
		{
			List<SqlScalarExpression> list4 = new List<SqlScalarExpression>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list4;
			break;
		}
		case 1972:
		{
			List<SqlScalarExpression> list3 = value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list3;
			break;
		}
		case 1973:
		{
			SqlLiteralExpression sqlLiteralExpression2 = SqlLiteralExpression.String(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression2.Position = yyloc;
			yyval.value = sqlLiteralExpression2;
			break;
		}
		case 1974:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.String(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression.Position = yyloc;
			yyval.value = sqlLiteralExpression;
			break;
		}
		case 1978:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1981:
		{
			List<SqlExecuteArgument> list2 = new List<SqlExecuteArgument>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlExecuteArgument);
			yyval.value = list2;
			break;
		}
		case 1982:
		{
			List<SqlExecuteArgument> list = value_stack.array[value_stack.top - 3].value as List<SqlExecuteArgument>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlExecuteArgument);
			yyval.value = list;
			break;
		}
		case 1983:
		{
			SqlScalarVariableRefExpression sqlScalarVariableRefExpression = SqlScalarVariableRefExpression.Create(GetText(location_stack.array[location_stack.top - 4]));
			sqlScalarVariableRefExpression.Position = location_stack.array[location_stack.top - 4];
			SqlExecuteArgument sqlExecuteArgument2 = new SqlExecuteArgument(sqlScalarVariableRefExpression, value_stack.array[value_stack.top - 2].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlParameterOption);
			sqlExecuteArgument2.Position = yyloc;
			yyval.value = sqlExecuteArgument2;
			break;
		}
		case 1984:
		{
			SqlExecuteArgument sqlExecuteArgument = new SqlExecuteArgument(value_stack.array[value_stack.top - 2].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlParameterOption);
			sqlExecuteArgument.Position = yyloc;
			yyval.value = sqlExecuteArgument;
			break;
		}
		case 1990:
		{
			SqlModuleOption sqlModuleOption = SqlModuleOption.CreateRecompileOption();
			sqlModuleOption.Position = yyloc;
			yyval.value = sqlModuleOption;
			break;
		}
		case 1953:
		case 1954:
		case 1957:
		case 1959:
		case 1960:
		case 1961:
		case 1962:
		case 1963:
		case 1965:
		case 1966:
		case 1967:
		case 1969:
		case 1970:
		case 1975:
		case 1976:
		case 1977:
		case 1979:
		case 1980:
		case 1985:
		case 1986:
		case 1987:
		case 1988:
		case 1989:
		case 1991:
		case 1992:
		case 1993:
		case 1994:
		case 1995:
		case 1996:
		case 1997:
		case 1998:
		case 1999:
			break;
		}
	}

	private void DoAction_2000_2049(int action)
	{
		switch (action)
		{
		case 2020:
		{
			SqlVariableDeclareStatement sqlVariableDeclareStatement = new SqlVariableDeclareStatement(value_stack.array[value_stack.top - 1].value as List<SqlVariableDeclaration>);
			sqlVariableDeclareStatement.Position = yyloc;
			yyval.value = sqlVariableDeclareStatement;
			break;
		}
		case 2021:
		{
			SqlCursorDeclareStatement sqlCursorDeclareStatement = new SqlCursorDeclareStatement(value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 2].value as List<SqlCursorOption>, value_stack.array[value_stack.top - 1].value as CursorDefinitionInfo);
			sqlCursorDeclareStatement.Position = yyloc;
			yyval.value = sqlCursorDeclareStatement;
			break;
		}
		case 2023:
		{
			List<SqlVariableDeclaration> list3 = new List<SqlVariableDeclaration>();
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlVariableDeclaration);
			yyval.value = list3;
			break;
		}
		case 2024:
		{
			List<SqlVariableDeclaration> list2 = value_stack.array[value_stack.top - 3].value as List<SqlVariableDeclaration>;
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlVariableDeclaration);
			yyval.value = list2;
			break;
		}
		case 2025:
		{
			SqlVariableDeclaration sqlVariableDeclaration2 = new SqlVariableDeclaration(GetText(location_stack.array[location_stack.top - 5]), value_stack.array[value_stack.top - 3].value as SqlDataTypeSpecification, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlVariableDeclaration2.Position = yyloc;
			yyval.value = sqlVariableDeclaration2;
			break;
		}
		case 2026:
		{
			SqlVariableDeclaration sqlVariableDeclaration = SqlVariableDeclaration.Create(value_stack.array[value_stack.top - 1].value as SqlParameterDeclaration);
			sqlVariableDeclaration.Position = yyloc;
			yyval.value = sqlVariableDeclaration;
			break;
		}
		case 2028:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2029:
			yyval.value = new CursorDefinitionInfo(value_stack.array[value_stack.top - 1].value as SqlSelectStatement, value_stack.array[value_stack.top - 3].value as List<SqlCursorOption>);
			break;
		case 2030:
			yyval.value = new List<SqlCursorOption>();
			break;
		case 2031:
		{
			SqlCursorOption sqlCursorOption = new SqlCursorOption(GetStringToken(location_stack.array[location_stack.top - 1]));
			sqlCursorOption.Position = location_stack.array[location_stack.top - 1];
			List<SqlCursorOption> list = value_stack.array[value_stack.top - 2].value as List<SqlCursorOption>;
			list.Add(sqlCursorOption);
			yyval.value = list;
			break;
		}
		case 2049:
		{
			SqlReturnStatement sqlReturnStatement = new SqlReturnStatement(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlReturnStatement.Position = yyloc;
			yyval.value = sqlReturnStatement;
			break;
		}
		case 2000:
		case 2001:
		case 2002:
		case 2003:
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
		case 2022:
		case 2027:
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
		case 2045:
		case 2046:
		case 2047:
		case 2048:
			break;
		}
	}

	private void DoAction_2050_2099(int action)
	{
		switch (action)
		{
		case 2052:
		{
			SqlIfElseStatement sqlIfElseStatement2 = new SqlIfElseStatement(value_stack.array[value_stack.top - 2].value as SqlBooleanExpression, value_stack.array[value_stack.top - 1].value as SqlStatement);
			sqlIfElseStatement2.Position = yyloc;
			yyval.value = sqlIfElseStatement2;
			break;
		}
		case 2053:
		{
			SqlIfElseStatement sqlIfElseStatement = new SqlIfElseStatement(value_stack.array[value_stack.top - 4].value as SqlBooleanExpression, value_stack.array[value_stack.top - 3].value as SqlStatement, value_stack.array[value_stack.top - 1].value as SqlStatement);
			sqlIfElseStatement.Position = yyloc;
			yyval.value = sqlIfElseStatement;
			break;
		}
		case 2054:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2056:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2057:
		{
			SqlWhileStatement sqlWhileStatement = new SqlWhileStatement(value_stack.array[value_stack.top - 2].value as SqlBooleanExpression, value_stack.array[value_stack.top - 1].value as SqlStatement);
			sqlWhileStatement.Position = yyloc;
			yyval.value = sqlWhileStatement;
			break;
		}
		case 2058:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2059:
		{
			SqlBreakStatement sqlBreakStatement = new SqlBreakStatement();
			sqlBreakStatement.Position = yyloc;
			yyval.value = sqlBreakStatement;
			break;
		}
		case 2060:
		{
			SqlContinueStatement sqlContinueStatement = new SqlContinueStatement();
			sqlContinueStatement.Position = yyloc;
			yyval.value = sqlContinueStatement;
			break;
		}
		case 2062:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 2063:
		{
			SqlBackupRestoreStatement sqlBackupRestoreStatement = value_stack.array[value_stack.top - 3].value as SqlBackupRestoreStatement;
			sqlBackupRestoreStatement.Position = yyloc;
			yyval.value = sqlBackupRestoreStatement;
			break;
		}
		case 2064:
		{
			SqlBackupDatabaseStatement sqlBackupDatabaseStatement = SqlBackupDatabaseStatement.Create(isDumpLoadStatement);
			sqlBackupDatabaseStatement.Position = yyloc;
			yyval.value = sqlBackupDatabaseStatement;
			break;
		}
		case 2065:
		{
			SqlBackupTableStatement sqlBackupTableStatement = SqlBackupTableStatement.Create(isDumpLoadStatement);
			sqlBackupTableStatement.Position = yyloc;
			yyval.value = sqlBackupTableStatement;
			break;
		}
		case 2066:
		{
			SqlBackupLogStatement sqlBackupLogStatement2 = SqlBackupLogStatement.Create(isDumpLoadStatement, isDumpTransaction: false);
			sqlBackupLogStatement2.Position = yyloc;
			yyval.value = sqlBackupLogStatement2;
			break;
		}
		case 2067:
		{
			SqlBackupLogStatement sqlBackupLogStatement = SqlBackupLogStatement.Create(isDumpLoadStatement, isDumpTransaction: true);
			sqlBackupLogStatement.Position = yyloc;
			yyval.value = sqlBackupLogStatement;
			break;
		}
		case 2050:
		case 2051:
		case 2055:
		case 2061:
		case 2068:
		case 2069:
		case 2070:
		case 2071:
		case 2072:
		case 2073:
		case 2074:
		case 2075:
		case 2076:
		case 2077:
		case 2078:
		case 2079:
		case 2080:
		case 2081:
		case 2082:
		case 2083:
		case 2084:
		case 2085:
		case 2086:
		case 2087:
		case 2088:
		case 2089:
		case 2090:
		case 2091:
		case 2092:
		case 2093:
		case 2094:
		case 2095:
		case 2096:
		case 2097:
		case 2098:
		case 2099:
			break;
		}
	}

	private void DoAction_2100_2149(int action)
	{
		switch (action)
		{
		case 2107:
		{
			SqlBackupRestoreStatement sqlBackupRestoreStatement = value_stack.array[value_stack.top - 3].value as SqlBackupRestoreStatement;
			sqlBackupRestoreStatement.Position = yyloc;
			yyval.value = sqlBackupRestoreStatement;
			break;
		}
		case 2108:
		{
			SqlRestoreDatabaseStatement sqlRestoreDatabaseStatement = SqlRestoreDatabaseStatement.Create(isDumpLoadStatement);
			sqlRestoreDatabaseStatement.Position = yyloc;
			yyval.value = sqlRestoreDatabaseStatement;
			break;
		}
		case 2109:
		{
			SqlRestoreTableStatement sqlRestoreTableStatement = SqlRestoreTableStatement.Create(isDumpLoadStatement);
			sqlRestoreTableStatement.Position = yyloc;
			yyval.value = sqlRestoreTableStatement;
			break;
		}
		case 2110:
		{
			SqlRestoreLogStatement sqlRestoreLogStatement2 = SqlRestoreLogStatement.Create(isDumpLoadStatement, isLoadTransaction: false);
			sqlRestoreLogStatement2.Position = yyloc;
			yyval.value = sqlRestoreLogStatement2;
			break;
		}
		case 2111:
		{
			SqlRestoreLogStatement sqlRestoreLogStatement = SqlRestoreLogStatement.Create(isDumpLoadStatement, isLoadTransaction: true);
			sqlRestoreLogStatement.Position = yyloc;
			yyval.value = sqlRestoreLogStatement;
			break;
		}
		case 2126:
		{
			SqlRestoreInformationStatement sqlRestoreInformationStatement = SqlRestoreInformationStatement.Create(isDumpLoadStatement);
			sqlRestoreInformationStatement.Position = yyloc;
			yyval.value = sqlRestoreInformationStatement;
			break;
		}
		case 2130:
		{
			SqlGdrStatement sqlGdrStatement = SqlGdrStatement.Create((SqlGdrStatementType)value_stack.array[value_stack.top - 8].value);
			sqlGdrStatement.Position = yyloc;
			yyval.value = sqlGdrStatement;
			break;
		}
		case 2133:
			scanner.SetGDRState();
			break;
		case 2134:
			scanner.SetGDRState();
			yyval.value = SqlGdrStatementType.SqlGrant;
			break;
		case 2135:
			scanner.SetGDRState();
			yyval.value = SqlGdrStatementType.SqlDeny;
			break;
		case 2136:
			yyval.value = SqlGdrStatementType.SqlRevoke;
			break;
		case 2137:
			scanner.SetGDRState();
			yyval.value = SqlGdrStatementType.SqlRevoke;
			break;
		case 2138:
			scanner.SetGDRState();
			break;
		case 2100:
		case 2101:
		case 2102:
		case 2103:
		case 2104:
		case 2105:
		case 2106:
		case 2112:
		case 2113:
		case 2114:
		case 2115:
		case 2116:
		case 2117:
		case 2118:
		case 2119:
		case 2120:
		case 2121:
		case 2122:
		case 2123:
		case 2124:
		case 2125:
		case 2127:
		case 2128:
		case 2129:
		case 2131:
		case 2132:
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
		case 2153:
			scanner.SetGDRState();
			break;
		case 2170:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2150:
		case 2151:
		case 2152:
		case 2154:
		case 2155:
		case 2156:
		case 2157:
		case 2158:
		case 2159:
		case 2160:
		case 2161:
		case 2162:
		case 2163:
		case 2164:
		case 2165:
		case 2166:
		case 2167:
		case 2168:
		case 2169:
		case 2171:
		case 2172:
		case 2173:
		case 2174:
		case 2175:
		case 2176:
		case 2177:
		case 2178:
		case 2179:
		case 2180:
		case 2181:
		case 2182:
		case 2183:
		case 2184:
		case 2185:
		case 2186:
		case 2187:
		case 2188:
		case 2189:
		case 2190:
		case 2191:
		case 2192:
		case 2193:
		case 2194:
		case 2195:
		case 2196:
		case 2197:
		case 2198:
		case 2199:
			break;
		}
	}

	private void DoAction_2200_2249(int action)
	{
		switch (action)
		{
		case 2208:
		{
			SqlTryCatchStatement sqlTryCatchStatement = new SqlTryCatchStatement(value_stack.array[value_stack.top - 3].value as List<SqlStatement>, value_stack.array[value_stack.top - 1].value as List<SqlStatement>);
			sqlTryCatchStatement.Position = yyloc;
			yyval.value = sqlTryCatchStatement;
			break;
		}
		case 2213:
			yyval = value_stack.array[value_stack.top - 3];
			break;
		case 2246:
			scanner.SetParseState(EParseState.e_ParseEventNotifObjectOn);
			break;
		case 2200:
		case 2201:
		case 2202:
		case 2203:
		case 2204:
		case 2205:
		case 2206:
		case 2207:
		case 2209:
		case 2210:
		case 2211:
		case 2212:
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
		case 2237:
		case 2238:
		case 2239:
		case 2240:
		case 2241:
		case 2242:
		case 2243:
		case 2244:
		case 2245:
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
		case 2269:
		{
			SqlDBCCStatement sqlDBCCStatement = new SqlDBCCStatement(GetText(location_stack.array[location_stack.top - 3]));
			sqlDBCCStatement.Position = yyloc;
			yyval.value = sqlDBCCStatement;
			break;
		}
		case 2250:
		case 2251:
		case 2252:
		case 2253:
		case 2254:
		case 2255:
		case 2256:
		case 2257:
		case 2258:
		case 2259:
		case 2260:
		case 2261:
		case 2262:
		case 2263:
		case 2264:
		case 2265:
		case 2266:
		case 2267:
		case 2268:
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
		case 2283:
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
		case 2342:
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
		case 2343:
		{
			SqlSetStatementError sqlSetStatementError = new SqlSetStatementError();
			sqlSetStatementError.Position = yyloc;
			yyval.value = sqlSetStatementError;
			break;
		}
		case 2300:
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
		case 2338:
		case 2339:
		case 2340:
		case 2341:
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
		case 2355:
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
		case 2356:
		{
			SqlSetAssignmentStatement sqlSetAssignmentStatement = new SqlSetAssignmentStatement(value_stack.array[value_stack.top - 1].value as SqlScalarVariableAssignment);
			sqlSetAssignmentStatement.Position = yyloc;
			yyval.value = sqlSetAssignmentStatement;
			break;
		}
		case 2358:
		{
			SqlScalarVariableRefExpression sqlScalarVariableRefExpression = SqlScalarVariableRefExpression.Create(GetText(location_stack.array[location_stack.top - 1]));
			sqlScalarVariableRefExpression.Position = yyloc;
			yyval.value = sqlScalarVariableRefExpression;
			break;
		}
		case 2362:
			yyval.value = SqlOnOffValue.On;
			break;
		case 2363:
			yyval.value = SqlOnOffValue.Off;
			break;
		case 2350:
		case 2351:
		case 2352:
		case 2353:
		case 2354:
		case 2357:
		case 2359:
		case 2360:
		case 2361:
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
		case 2377:
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
		case 2414:
		{
			List<SqlSelectExpression> list5 = new List<SqlSelectExpression>();
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlSelectExpression);
			yyval.value = list5;
			break;
		}
		case 2415:
		{
			List<SqlSelectExpression> list4 = value_stack.array[value_stack.top - 3].value as List<SqlSelectExpression>;
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlSelectExpression);
			yyval.value = list4;
			break;
		}
		case 2416:
		{
			SqlSelectScalarExpression sqlSelectScalarExpression4 = new SqlSelectScalarExpression(value_stack.array[value_stack.top - 2].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlSelectScalarExpression4.Position = yyloc;
			yyval.value = sqlSelectScalarExpression4;
			break;
		}
		case 2417:
		{
			SqlSelectScalarExpression sqlSelectScalarExpression3 = SqlSelectScalarExpression.IdentityFunction(value_stack.array[value_stack.top - 1].value as SqlIdentityFunctionCallExpression, value_stack.array[value_stack.top - 3].value as SqlIdentifier);
			sqlSelectScalarExpression3.Position = yyloc;
			yyval.value = sqlSelectScalarExpression3;
			break;
		}
		case 2418:
		{
			SqlSelectScalarExpression sqlSelectScalarExpression2 = new SqlSelectScalarExpression(value_stack.array[value_stack.top - 1].value as SqlScalarExpression, value_stack.array[value_stack.top - 3].value as SqlIdentifier);
			sqlSelectScalarExpression2.Position = yyloc;
			yyval.value = sqlSelectScalarExpression2;
			break;
		}
		case 2419:
		{
			SqlSelectScalarExpression sqlSelectScalarExpression = SqlSelectScalarExpression.IdentityFunction(value_stack.array[value_stack.top - 2].value as SqlIdentityFunctionCallExpression, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlSelectScalarExpression.Position = yyloc;
			yyval.value = sqlSelectScalarExpression;
			break;
		}
		case 2420:
		{
			SqlSelectStarExpression sqlSelectStarExpression2 = new SqlSelectStarExpression();
			sqlSelectStarExpression2.Position = yyloc;
			yyval.value = sqlSelectStarExpression2;
			break;
		}
		case 2421:
		{
			SqlObjectIdentifier sqlObjectIdentifier4 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>);
			sqlObjectIdentifier4.Position = location_stack.array[location_stack.top - 2];
			SqlSelectStarExpression sqlSelectStarExpression = new SqlSelectStarExpression(sqlObjectIdentifier4);
			sqlSelectStarExpression.Position = yyloc;
			yyval.value = sqlSelectStarExpression;
			break;
		}
		case 2422:
		{
			SqlSelectVariableAssignmentExpression sqlSelectVariableAssignmentExpression = new SqlSelectVariableAssignmentExpression(value_stack.array[value_stack.top - 1].value as SqlScalarVariableAssignment);
			sqlSelectVariableAssignmentExpression.Position = yyloc;
			yyval.value = sqlSelectVariableAssignmentExpression;
			break;
		}
		case 2423:
		{
			SqlScalarVariableRefExpression sqlScalarVariableRefExpression = SqlScalarVariableRefExpression.Create(value_stack.array[value_stack.top - 3].value as string);
			sqlScalarVariableRefExpression.Position = location_stack.array[location_stack.top - 3];
			SqlScalarVariableAssignment sqlScalarVariableAssignment = new SqlScalarVariableAssignment(sqlScalarVariableRefExpression, (SqlAssignmentOperatorType)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlScalarVariableAssignment.Position = yyloc;
			yyval.value = sqlScalarVariableAssignment;
			break;
		}
		case 2424:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2428:
		{
			SqlLiteralStringIdentifier sqlLiteralStringIdentifier = new SqlLiteralStringIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralStringIdentifier.Position = yyloc;
			yyval.value = sqlLiteralStringIdentifier;
			break;
		}
		case 2429:
		{
			SqlIdentifier sqlIdentifier2 = new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlIdentifier2.Position = yyloc;
			yyval.value = sqlIdentifier2;
			break;
		}
		case 2430:
		{
			SqlIdentifier sqlIdentifier = new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlIdentifier.Position = yyloc;
			yyval.value = sqlIdentifier;
			break;
		}
		case 2431:
		{
			SqlObjectIdentifier sqlObjectIdentifier3 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier3.Position = yyloc;
			SqlDataTypeSpecification sqlDataTypeSpecification4 = new SqlDataTypeSpecification(new SqlDataType(sqlObjectIdentifier3)
			{
				Position = yyloc
			});
			sqlDataTypeSpecification4.Position = yyloc;
			yyval.value = sqlDataTypeSpecification4;
			break;
		}
		case 2432:
		{
			SqlObjectIdentifier sqlObjectIdentifier2 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 4].value as SqlIdentifier);
			sqlObjectIdentifier2.Position = location_stack.array[location_stack.top - 4];
			SqlDataType dataType2 = new SqlDataType(sqlObjectIdentifier2)
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
		case 2433:
		{
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 6].value as SqlIdentifier);
			sqlObjectIdentifier.Position = location_stack.array[location_stack.top - 6];
			SqlDataType dataType = new SqlDataType(sqlObjectIdentifier)
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
		case 2434:
		{
			SqlIdentityFunctionCallExpression sqlIdentityFunctionCallExpression2 = new SqlIdentityFunctionCallExpression(GetText(location_stack.array[location_stack.top - 4]), value_stack.array[value_stack.top - 2].value as SqlDataTypeSpecification);
			sqlIdentityFunctionCallExpression2.Position = yyloc;
			yyval.value = sqlIdentityFunctionCallExpression2;
			break;
		}
		case 2435:
		{
			SqlIdentityFunctionCallExpression sqlIdentityFunctionCallExpression = new SqlIdentityFunctionCallExpression(GetText(location_stack.array[location_stack.top - 8]), value_stack.array[value_stack.top - 6].value as SqlDataTypeSpecification, value_stack.array[value_stack.top - 4].value as SqlLiteralExpression, value_stack.array[value_stack.top - 2].value as SqlLiteralExpression);
			sqlIdentityFunctionCallExpression.Position = yyloc;
			yyval.value = sqlIdentityFunctionCallExpression;
			break;
		}
		case 2436:
		{
			List<SqlParameterDeclaration> list3 = new List<SqlParameterDeclaration>();
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlParameterDeclaration);
			yyval.value = list3;
			break;
		}
		case 2437:
		{
			List<SqlParameterDeclaration> list2 = value_stack.array[value_stack.top - 3].value as List<SqlParameterDeclaration>;
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlParameterDeclaration);
			yyval.value = list2;
			break;
		}
		case 2438:
		{
			SqlParameterDeclaration sqlParameterDeclaration2 = new SqlParameterDeclaration(GetText(location_stack.array[location_stack.top - 6]), value_stack.array[value_stack.top - 4].value as SqlDataTypeSpecification, value_stack.array[value_stack.top - 2].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlParameterOption);
			sqlParameterDeclaration2.Position = yyloc;
			yyval.value = sqlParameterDeclaration2;
			break;
		}
		case 2440:
		{
			SqlDataTypeSpecification sqlDataTypeSpecification = SqlDataTypeSpecification.Cursor();
			sqlDataTypeSpecification.Position = location_stack.array[location_stack.top - 3];
			SqlParameterDeclaration sqlParameterDeclaration = new SqlParameterDeclaration(GetText(location_stack.array[location_stack.top - 5]), sqlDataTypeSpecification, null, value_stack.array[value_stack.top - 1].value as SqlParameterOption);
			sqlParameterDeclaration.Position = yyloc;
			yyval.value = sqlParameterDeclaration;
			break;
		}
		case 2442:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2444:
		{
			SqlParameterOption sqlParameterOption = SqlParameterOption.Create(GetText(location_stack.array[location_stack.top - 1]));
			sqlParameterOption.Position = yyloc;
			yyval.value = sqlParameterOption;
			break;
		}
		case 2448:
		{
			SqlFromClause sqlFromClause = new SqlFromClause(value_stack.array[value_stack.top - 1].value as List<SqlTableExpression>);
			sqlFromClause.Position = yyloc;
			yyval.value = sqlFromClause;
			scanner.SetQueryState(QueryState.None);
			break;
		}
		case 2449:
		{
			List<SqlTableExpression> list = new List<SqlTableExpression>();
			list.Add(value_stack.array[value_stack.top - 1].value as SqlTableExpression);
			yyval.value = list;
			break;
		}
		case 2400:
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
		case 2425:
		case 2426:
		case 2427:
		case 2439:
		case 2441:
		case 2443:
		case 2445:
		case 2446:
		case 2447:
			break;
		}
	}

	private void DoAction_2450_2499(int action)
	{
		switch (action)
		{
		case 2450:
		{
			List<SqlTableExpression> list = value_stack.array[value_stack.top - 3].value as List<SqlTableExpression>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlTableExpression);
			yyval.value = list;
			break;
		}
		case 2451:
			if (value_stack.array[value_stack.top - 1].value == null)
			{
				SqlTableExpression sqlTableExpression2 = new SqlNullTableExpression();
				sqlTableExpression2.Position = yyloc;
				yyval.value = sqlTableExpression2;
			}
			break;
		case 2452:
			if (value_stack.array[value_stack.top - 1].value == null)
			{
				SqlTableExpression sqlTableExpression4 = new SqlNullTableExpression();
				sqlTableExpression4.Position = yyloc;
				yyval.value = sqlTableExpression4;
			}
			break;
		case 2453:
			if (value_stack.array[value_stack.top - 1].value == null)
			{
				SqlTableExpression sqlTableExpression6 = new SqlNullTableExpression();
				sqlTableExpression6.Position = yyloc;
				yyval.value = sqlTableExpression6;
			}
			break;
		case 2454:
			if (value_stack.array[value_stack.top - 1].value == null)
			{
				SqlTableExpression sqlTableExpression5 = new SqlNullTableExpression();
				sqlTableExpression5.Position = yyloc;
				yyval.value = sqlTableExpression5;
			}
			break;
		case 2455:
			if (value_stack.array[value_stack.top - 1].value == null)
			{
				SqlTableExpression sqlTableExpression3 = new SqlNullTableExpression();
				sqlTableExpression3.Position = yyloc;
				yyval.value = sqlTableExpression3;
			}
			break;
		case 2456:
			if (value_stack.array[value_stack.top - 1].value == null)
			{
				SqlTableExpression sqlTableExpression = new SqlNullTableExpression();
				sqlTableExpression.Position = yyloc;
				yyval.value = sqlTableExpression;
			}
			break;
		case 2457:
		{
			SqlTableExpressionError sqlTableExpressionError = new SqlTableExpressionError(value_stack.array[value_stack.top - 1].value as SqlErrorCodeObject);
			sqlTableExpressionError.Position = yyloc;
			yyval.value = sqlTableExpressionError;
			break;
		}
		case 2459:
		{
			SqlTableRefExpression sqlTableRefExpression3 = new SqlTableRefExpression(value_stack.array[value_stack.top - 3].value as SqlObjectIdentifier);
			sqlTableRefExpression3.Position = yyloc;
			yyval.value = sqlTableRefExpression3;
			break;
		}
		case 2460:
		{
			SqlTableUdtInstanceMethodExpression sqlTableUdtInstanceMethodExpression = new SqlTableUdtInstanceMethodExpression(value_stack.array[value_stack.top - 8].value as SqlScalarVariableRefExpression, value_stack.array[value_stack.top - 6].value as SqlIdentifier, value_stack.array[value_stack.top - 4].value as List<SqlScalarExpression>, value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlIdentifier>);
			sqlTableUdtInstanceMethodExpression.Position = yyloc;
			yyval.value = sqlTableUdtInstanceMethodExpression;
			break;
		}
		case 2461:
		{
			SqlTableValuedFunctionRefExpression sqlTableValuedFunctionRefExpression2 = new SqlTableValuedFunctionRefExpression(value_stack.array[value_stack.top - 7].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 4].value as List<SqlScalarExpression>, value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlIdentifier>);
			sqlTableValuedFunctionRefExpression2.Position = yyloc;
			yyval.value = sqlTableValuedFunctionRefExpression2;
			break;
		}
		case 2463:
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
		case 2464:
		{
			SqlTableRefExpression sqlTableRefExpression2 = new SqlTableRefExpression(value_stack.array[value_stack.top - 4].value as SqlObjectIdentifier, null, value_stack.array[value_stack.top - 1].value as List<SqlHint>);
			sqlTableRefExpression2.Position = yyloc;
			yyval.value = sqlTableRefExpression2;
			break;
		}
		case 2465:
		{
			SqlTableRefExpression sqlTableRefExpression = new SqlTableRefExpression(value_stack.array[value_stack.top - 6].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlHint>);
			sqlTableRefExpression.Position = yyloc;
			yyval.value = sqlTableRefExpression;
			break;
		}
		case 2477:
		{
			SqlTableVariableRefExpression sqlTableVariableRefExpression = new SqlTableVariableRefExpression(GetText(location_stack.array[location_stack.top - 2]), value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlTableVariableRefExpression.Position = yyloc;
			yyval.value = sqlTableVariableRefExpression;
			break;
		}
		case 2458:
		case 2462:
		case 2466:
		case 2467:
		case 2468:
		case 2469:
		case 2470:
		case 2471:
		case 2472:
		case 2473:
		case 2474:
		case 2475:
		case 2476:
		case 2478:
		case 2479:
		case 2480:
		case 2481:
		case 2482:
		case 2483:
		case 2484:
		case 2485:
		case 2486:
		case 2487:
		case 2488:
		case 2489:
		case 2490:
		case 2491:
		case 2492:
		case 2493:
		case 2494:
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
		case 2510:
		{
			SqlNullTableExpression sqlNullTableExpression6 = new SqlNullTableExpression();
			sqlNullTableExpression6.Position = yyloc;
			yyval.value = sqlNullTableExpression6;
			break;
		}
		case 2511:
		{
			SqlNullTableExpression sqlNullTableExpression5 = new SqlNullTableExpression();
			sqlNullTableExpression5.Position = yyloc;
			yyval.value = sqlNullTableExpression5;
			break;
		}
		case 2512:
		{
			SqlNullTableExpression sqlNullTableExpression4 = new SqlNullTableExpression();
			sqlNullTableExpression4.Position = yyloc;
			yyval.value = sqlNullTableExpression4;
			break;
		}
		case 2513:
		{
			SqlNullTableExpression sqlNullTableExpression3 = new SqlNullTableExpression();
			sqlNullTableExpression3.Position = yyloc;
			yyval.value = sqlNullTableExpression3;
			break;
		}
		case 2514:
		{
			SqlNullTableExpression sqlNullTableExpression2 = new SqlNullTableExpression();
			sqlNullTableExpression2.Position = yyloc;
			yyval.value = sqlNullTableExpression2;
			break;
		}
		case 2515:
		{
			SqlNullTableExpression sqlNullTableExpression = new SqlNullTableExpression();
			sqlNullTableExpression.Position = yyloc;
			yyval.value = sqlNullTableExpression;
			break;
		}
		case 2535:
			ExpectKeyword(StringToken.Json, location_stack.array[location_stack.top - 1]);
			break;
		case 2546:
			yyval = value_stack.array[value_stack.top - 2];
			break;
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
		case 2536:
		case 2537:
		case 2538:
		case 2539:
		case 2540:
		case 2541:
		case 2542:
		case 2543:
		case 2544:
		case 2545:
		case 2547:
		case 2548:
		case 2549:
			break;
		}
	}

	private void DoAction_2550_2599(int action)
	{
		switch (action)
		{
		case 2550:
		{
			SqlObjectIdentifier sqlObjectIdentifier3 = SqlObjectIdentifier.Create(new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]))
			{
				Position = yyloc
			});
			sqlObjectIdentifier3.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression = new SqlColumnRefExpression(sqlObjectIdentifier3);
			sqlColumnRefExpression.Position = yyloc;
			yyval.value = sqlColumnRefExpression;
			break;
		}
		case 2551:
		{
			List<SqlScalarExpression> list4 = new List<SqlScalarExpression>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list4;
			break;
		}
		case 2552:
		{
			List<SqlScalarExpression> list3 = value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list3;
			break;
		}
		case 2555:
		{
			List<SqlScalarExpression> list2 = new List<SqlScalarExpression>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list2;
			break;
		}
		case 2556:
		{
			List<SqlScalarExpression> list = value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list;
			break;
		}
		case 2558:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2561:
		{
			SqlDerivedTableExpression sqlDerivedTableExpression2 = new SqlDerivedTableExpression(value_stack.array[value_stack.top - 4].value as SqlQueryExpression, value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlIdentifier>);
			sqlDerivedTableExpression2.Position = yyloc;
			yyval.value = sqlDerivedTableExpression2;
			break;
		}
		case 2562:
		{
			SqlDerivedTableExpression sqlDerivedTableExpression = new SqlDerivedTableExpression(value_stack.array[value_stack.top - 5].value as SqlQueryExpression, value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlIdentifier>);
			sqlDerivedTableExpression.Position = yyloc;
			yyval.value = sqlDerivedTableExpression;
			break;
		}
		case 2565:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 2566:
		{
			SqlUnqualifiedJoinTableExpression sqlUnqualifiedJoinTableExpression3 = new SqlUnqualifiedJoinTableExpression(value_stack.array[value_stack.top - 4].value as SqlTableExpression, SqlJoinOperatorType.CrossJoin, value_stack.array[value_stack.top - 1].value as SqlTableExpression);
			sqlUnqualifiedJoinTableExpression3.Position = yyloc;
			yyval.value = sqlUnqualifiedJoinTableExpression3;
			break;
		}
		case 2567:
		{
			SqlUnqualifiedJoinTableExpression sqlUnqualifiedJoinTableExpression2 = new SqlUnqualifiedJoinTableExpression(value_stack.array[value_stack.top - 4].value as SqlTableExpression, SqlJoinOperatorType.CrossApply, value_stack.array[value_stack.top - 1].value as SqlTableExpression);
			sqlUnqualifiedJoinTableExpression2.Position = yyloc;
			yyval.value = sqlUnqualifiedJoinTableExpression2;
			ExpectKeyword(StringToken.Apply, location_stack.array[location_stack.top - 2]);
			break;
		}
		case 2568:
		{
			SqlUnqualifiedJoinTableExpression sqlUnqualifiedJoinTableExpression = new SqlUnqualifiedJoinTableExpression(value_stack.array[value_stack.top - 4].value as SqlTableExpression, SqlJoinOperatorType.OuterApply, value_stack.array[value_stack.top - 1].value as SqlTableExpression);
			sqlUnqualifiedJoinTableExpression.Position = yyloc;
			yyval.value = sqlUnqualifiedJoinTableExpression;
			ExpectKeyword(StringToken.Apply, location_stack.array[location_stack.top - 2]);
			break;
		}
		case 2569:
		{
			SqlQualifiedJoinTableExpression sqlQualifiedJoinTableExpression = new SqlQualifiedJoinTableExpression(value_stack.array[value_stack.top - 4].value as SqlTableExpression, (SqlJoinOperatorType)value_stack.array[value_stack.top - 3].value, value_stack.array[value_stack.top - 2].value as SqlTableExpression, value_stack.array[value_stack.top - 1].value as SqlConditionClause);
			sqlQualifiedJoinTableExpression.Position = yyloc;
			yyval.value = sqlQualifiedJoinTableExpression;
			break;
		}
		case 2579:
			yyval.value = SqlJoinOperatorType.InnerJoin;
			break;
		case 2580:
			yyval.value = SqlJoinOperatorType.InnerJoin;
			break;
		case 2581:
			yyval.value = SqlJoinOperatorType.LeftOuterJoin;
			break;
		case 2582:
			yyval.value = SqlJoinOperatorType.RightOuterJoin;
			break;
		case 2583:
			yyval.value = SqlJoinOperatorType.FullOuterJoin;
			break;
		case 2584:
		{
			SqlConditionClause sqlConditionClause = new SqlConditionClause(value_stack.array[value_stack.top - 1].value as SqlBooleanExpression);
			sqlConditionClause.Position = yyloc;
			yyval.value = sqlConditionClause;
			break;
		}
		case 2585:
		{
			SqlOnClauseError sqlOnClauseError = new SqlOnClauseError(value_stack.array[value_stack.top - 1].value as SqlBooleanExpression);
			sqlOnClauseError.Position = yyloc;
			yyval.value = sqlOnClauseError;
			break;
		}
		case 2588:
		{
			SqlPivotClause sqlPivotClause = new SqlPivotClause(value_stack.array[value_stack.top - 13].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 11].value as List<SqlColumnRefExpression>, value_stack.array[value_stack.top - 8].value as SqlColumnRefExpression, value_stack.array[value_stack.top - 5].value as List<SqlColumnRefExpression>);
			sqlPivotClause.Position = MergeLocationRange(location_stack.top - 13, location_stack.top - 3);
			SqlPivotTableExpression sqlPivotTableExpression = new SqlPivotTableExpression(value_stack.array[value_stack.top - 16].value as SqlTableExpression, sqlPivotClause, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlPivotTableExpression.Position = yyloc;
			yyval.value = sqlPivotTableExpression;
			break;
		}
		case 2589:
		{
			SqlObjectIdentifier sqlObjectIdentifier2 = SqlObjectIdentifier.Create(new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]))
			{
				Position = yyloc
			});
			sqlObjectIdentifier2.Position = yyloc;
			yyval.value = sqlObjectIdentifier2;
			break;
		}
		case 2590:
		{
			List<SqlIdentifier> obj = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			obj.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(obj);
			sqlObjectIdentifier.Position = yyloc;
			yyval.value = sqlObjectIdentifier;
			break;
		}
		case 2591:
		{
			SqlUnpivotClause sqlUnpivotClause = new SqlUnpivotClause(value_stack.array[value_stack.top - 10].value as SqlColumnRefExpression, value_stack.array[value_stack.top - 8].value as SqlColumnRefExpression, value_stack.array[value_stack.top - 5].value as List<SqlColumnRefExpression>);
			sqlUnpivotClause.Position = MergeLocationRange(location_stack.top - 10, location_stack.top - 3);
			SqlUnpivotTableExpression sqlUnpivotTableExpression = new SqlUnpivotTableExpression(value_stack.array[value_stack.top - 13].value as SqlTableExpression, sqlUnpivotClause, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlUnpivotTableExpression.Position = yyloc;
			yyval.value = sqlUnpivotTableExpression;
			break;
		}
		case 2553:
		case 2554:
		case 2557:
		case 2559:
		case 2560:
		case 2563:
		case 2564:
		case 2570:
		case 2571:
		case 2572:
		case 2573:
		case 2574:
		case 2575:
		case 2576:
		case 2577:
		case 2578:
		case 2586:
		case 2587:
		case 2592:
		case 2593:
		case 2594:
		case 2595:
		case 2596:
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
		case 2600:
		{
			List<SqlColumnRefExpression> list8 = new List<SqlColumnRefExpression>();
			list8.Add(value_stack.array[value_stack.top - 1].value as SqlColumnRefExpression);
			yyval.value = list8;
			break;
		}
		case 2601:
		{
			List<SqlColumnRefExpression> list7 = value_stack.array[value_stack.top - 3].value as List<SqlColumnRefExpression>;
			list7.Add(value_stack.array[value_stack.top - 1].value as SqlColumnRefExpression);
			yyval.value = list7;
			break;
		}
		case 2602:
		{
			SqlDmlSpecificationTableSource sqlDmlSpecificationTableSource = new SqlDmlSpecificationTableSource(value_stack.array[value_stack.top - 5].value as SqlDmlSpecification, value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlIdentifier>);
			sqlDmlSpecificationTableSource.Position = yyloc;
			yyval.value = sqlDmlSpecificationTableSource;
			break;
		}
		case 2612:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2613:
		{
			List<SqlHint> list6 = new List<SqlHint>();
			list6.Add(value_stack.array[value_stack.top - 1].value as SqlHint);
			yyval.value = list6;
			break;
		}
		case 2614:
		{
			SqlHint sqlHint = SqlHint.Table(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlHint.Position = location_stack.array[location_stack.top - 1];
			List<SqlHint> list5 = new List<SqlHint>();
			list5.Add(sqlHint);
			yyval.value = list5;
			break;
		}
		case 2615:
		{
			SqlLiteralExpression sqlLiteralExpression3 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 2]));
			sqlLiteralExpression3.Position = location_stack.array[location_stack.top - 2];
			SqlIndexHint sqlIndexHint3 = new SqlIndexHint(sqlLiteralExpression3);
			sqlIndexHint3.Position = yyloc;
			yyval.value = sqlIndexHint3;
			break;
		}
		case 2616:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 2626:
		{
			List<SqlHint> list4 = new List<SqlHint>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlHint);
			yyval.value = list4;
			break;
		}
		case 2627:
		{
			List<SqlHint> list3 = value_stack.array[value_stack.top - 3].value as List<SqlHint>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlHint);
			yyval.value = list3;
			break;
		}
		case 2632:
		{
			SqlTableHint sqlTableHint4 = new SqlTableHint(GetText(location_stack.array[location_stack.top - 3]));
			sqlTableHint4.Position = yyloc;
			yyval.value = sqlTableHint4;
			ExpectKeyword(StringToken.SpatialWindowMaxCells, location_stack.array[location_stack.top - 3]);
			break;
		}
		case 2635:
		{
			SqlTableHint sqlTableHint3 = new SqlTableHint(GetText(location_stack.array[location_stack.top - 7]));
			sqlTableHint3.Position = yyloc;
			yyval.value = sqlTableHint3;
			ExpectKeyword(StringToken.ForceSeek, location_stack.array[location_stack.top - 7]);
			break;
		}
		case 2638:
		{
			SqlTableHint sqlTableHint2 = new SqlTableHint(GetText(location_stack.array[location_stack.top - 1]));
			sqlTableHint2.Position = yyloc;
			yyval.value = sqlTableHint2;
			break;
		}
		case 2639:
		{
			SqlTableHint sqlTableHint = new SqlTableHint(GetText(location_stack.array[location_stack.top - 1]));
			sqlTableHint.Position = yyloc;
			yyval.value = sqlTableHint;
			break;
		}
		case 2640:
		{
			SqlIndexHint sqlIndexHint2 = new SqlIndexHint(value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			sqlIndexHint2.Position = yyloc;
			yyval.value = sqlIndexHint2;
			break;
		}
		case 2641:
		{
			SqlIndexHint sqlIndexHint = new SqlIndexHint(value_stack.array[value_stack.top - 2].value as List<SqlLiteralExpression>);
			sqlIndexHint.Position = yyloc;
			yyval.value = sqlIndexHint;
			break;
		}
		case 2642:
		{
			List<SqlLiteralExpression> list2 = new List<SqlLiteralExpression>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			yyval.value = list2;
			break;
		}
		case 2643:
		{
			List<SqlLiteralExpression> list = value_stack.array[value_stack.top - 3].value as List<SqlLiteralExpression>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			yyval.value = list;
			break;
		}
		case 2645:
		{
			SqlLiteralExpression sqlLiteralExpression2 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression2.Position = yyloc;
			yyval.value = sqlLiteralExpression2;
			break;
		}
		case 2646:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Identifier(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlLiteralExpression.Position = yyloc;
			yyval.value = sqlLiteralExpression;
			break;
		}
		case 2603:
		case 2604:
		case 2605:
		case 2606:
		case 2607:
		case 2608:
		case 2609:
		case 2610:
		case 2611:
		case 2617:
		case 2618:
		case 2619:
		case 2620:
		case 2621:
		case 2622:
		case 2623:
		case 2624:
		case 2625:
		case 2628:
		case 2629:
		case 2630:
		case 2631:
		case 2633:
		case 2634:
		case 2636:
		case 2637:
		case 2644:
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
		{
			SqlBinaryQueryExpression sqlBinaryQueryExpression = new SqlBinaryQueryExpression(value_stack.array[value_stack.top - 4].value as SqlQueryExpression, (SqlBinaryQueryOperatorType)value_stack.array[value_stack.top - 3].value, (bool)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlBinaryQueryExpression.Position = yyloc;
			yyval.value = sqlBinaryQueryExpression;
			break;
		}
		case 2660:
		{
			SqlInlineFunctionBodyDefinition sqlInlineFunctionBodyDefinition = new SqlInlineFunctionBodyDefinition(value_stack.array[value_stack.top - 2].value as SqlQueryWithClause, value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlInlineFunctionBodyDefinition.Position = yyloc;
			yyval.value = sqlInlineFunctionBodyDefinition;
			break;
		}
		case 2662:
		{
			SqlBinaryQueryExpression sqlBinaryQueryExpression2 = new SqlBinaryQueryExpression(value_stack.array[value_stack.top - 4].value as SqlQueryExpression, SqlBinaryQueryOperatorType.Intersect, (bool)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlBinaryQueryExpression2.Position = yyloc;
			yyval.value = sqlBinaryQueryExpression2;
			break;
		}
		case 2664:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 2665:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 2666:
			if (value_stack.array[value_stack.top - 1].value == null)
			{
				SqlNullQueryExpression sqlNullQueryExpression = new SqlNullQueryExpression();
				sqlNullQueryExpression.Position = yyloc;
				yyval.value = sqlNullQueryExpression;
			}
			break;
		case 2667:
		{
			SqlScalarSubQueryExpression sqlScalarSubQueryExpression = new SqlScalarSubQueryExpression(value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlScalarSubQueryExpression.Position = yyloc;
			yyval.value = sqlScalarSubQueryExpression;
			break;
		}
		case 2669:
		{
			SqlQuerySpecification sqlQuerySpecification = new SqlQuerySpecification(new SqlSelectClause(value_stack.array[value_stack.top - 9].value != null, value_stack.array[value_stack.top - 8].value as SqlTopSpecification, value_stack.array[value_stack.top - 7].value as List<SqlSelectExpression>)
			{
				Position = MergeLocationRange(location_stack.top - 10, location_stack.top - 7)
			}, value_stack.array[value_stack.top - 6].value as SqlFromClause, value_stack.array[value_stack.top - 5].value as SqlWhereClause, value_stack.array[value_stack.top - 4].value as SqlGroupByClause, value_stack.array[value_stack.top - 3].value as SqlHavingClause, value_stack.array[value_stack.top - 2].value as SqlOrderByClause, value_stack.array[value_stack.top - 1].value as SqlForClause);
			sqlQuerySpecification.Position = yyloc;
			yyval.value = sqlQuerySpecification;
			break;
		}
		case 2670:
		{
			List<SqlSelectExpression> list12 = new List<SqlSelectExpression>();
			list12.Add(value_stack.array[value_stack.top - 1].value as SqlSelectExpression);
			yyval.value = list12;
			break;
		}
		case 2671:
		{
			List<SqlSelectExpression> list11 = value_stack.array[value_stack.top - 3].value as List<SqlSelectExpression>;
			list11.Add(value_stack.array[value_stack.top - 1].value as SqlSelectExpression);
			yyval.value = list11;
			break;
		}
		case 2672:
		{
			SqlSelectScalarExpression sqlSelectScalarExpression3 = new SqlSelectScalarExpression(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlSelectScalarExpression3.Position = yyloc;
			yyval.value = sqlSelectScalarExpression3;
			break;
		}
		case 2673:
		{
			SqlSelectScalarExpression sqlSelectScalarExpression2 = new SqlSelectScalarExpression(value_stack.array[value_stack.top - 1].value as SqlScalarExpression, value_stack.array[value_stack.top - 3].value as SqlIdentifier);
			sqlSelectScalarExpression2.Position = yyloc;
			yyval.value = sqlSelectScalarExpression2;
			break;
		}
		case 2674:
		{
			SqlSelectScalarExpression sqlSelectScalarExpression = new SqlSelectScalarExpression(value_stack.array[value_stack.top - 2].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlSelectScalarExpression.Position = yyloc;
			yyval.value = sqlSelectScalarExpression;
			break;
		}
		case 2675:
		{
			SqlSelectStarExpression sqlSelectStarExpression2 = new SqlSelectStarExpression();
			sqlSelectStarExpression2.Position = yyloc;
			yyval.value = sqlSelectStarExpression2;
			break;
		}
		case 2676:
		{
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>);
			sqlObjectIdentifier.Position = location_stack.array[location_stack.top - 2];
			SqlSelectStarExpression sqlSelectStarExpression = new SqlSelectStarExpression(sqlObjectIdentifier);
			sqlSelectStarExpression.Position = yyloc;
			yyval.value = sqlSelectStarExpression;
			break;
		}
		case 2677:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2679:
		{
			SqlLiteralStringIdentifier sqlLiteralStringIdentifier = new SqlLiteralStringIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralStringIdentifier.Position = yyloc;
			yyval.value = sqlLiteralStringIdentifier;
			break;
		}
		case 2681:
		{
			SqlOrderByClause sqlOrderByClause = new SqlOrderByClause(value_stack.array[value_stack.top - 2].value as List<SqlOrderByItem>, value_stack.array[value_stack.top - 1].value as SqlOffsetFetchClause);
			sqlOrderByClause.Position = yyloc;
			yyval.value = sqlOrderByClause;
			break;
		}
		case 2682:
		{
			List<SqlOrderByItem> list10 = new List<SqlOrderByItem>();
			list10.Add(value_stack.array[value_stack.top - 1].value as SqlOrderByItem);
			yyval.value = list10;
			break;
		}
		case 2683:
		{
			List<SqlOrderByItem> list9 = value_stack.array[value_stack.top - 3].value as List<SqlOrderByItem>;
			list9.Add(value_stack.array[value_stack.top - 1].value as SqlOrderByItem);
			yyval.value = list9;
			break;
		}
		case 2684:
		{
			SqlOrderByItem sqlOrderByItem = new SqlOrderByItem(value_stack.array[value_stack.top - 2].value as SqlScalarExpression, (SqlSortOrder)value_stack.array[value_stack.top - 1].value);
			sqlOrderByItem.Position = yyloc;
			yyval.value = sqlOrderByItem;
			break;
		}
		case 2686:
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
		case 2687:
		{
			SqlForBrowseClause sqlForBrowseClause = new SqlForBrowseClause();
			sqlForBrowseClause.Position = yyloc;
			yyval.value = sqlForBrowseClause;
			break;
		}
		case 2688:
		{
			SqlTableConstructorExpression sqlTableConstructorExpression2 = new SqlTableConstructorExpression(value_stack.array[value_stack.top - 1].value as List<SqlRowConstructorExpression>);
			sqlTableConstructorExpression2.Position = yyloc;
			yyval.value = sqlTableConstructorExpression2;
			break;
		}
		case 2689:
		{
			List<SqlRowConstructorExpression> list8 = new List<SqlRowConstructorExpression>();
			list8.Add(value_stack.array[value_stack.top - 1].value as SqlRowConstructorExpression);
			yyval.value = list8;
			break;
		}
		case 2690:
		{
			List<SqlRowConstructorExpression> list7 = value_stack.array[value_stack.top - 3].value as List<SqlRowConstructorExpression>;
			list7.Add(value_stack.array[value_stack.top - 1].value as SqlRowConstructorExpression);
			yyval.value = list7;
			break;
		}
		case 2691:
		{
			SqlRowConstructorExpression sqlRowConstructorExpression2 = new SqlRowConstructorExpression(value_stack.array[value_stack.top - 2].value as List<SqlScalarExpression>);
			sqlRowConstructorExpression2.Position = yyloc;
			yyval.value = sqlRowConstructorExpression2;
			break;
		}
		case 2692:
		{
			List<SqlScalarExpression> list6 = new List<SqlScalarExpression>();
			list6.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list6;
			break;
		}
		case 2693:
		{
			List<SqlScalarExpression> list5 = value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>;
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list5;
			break;
		}
		case 2694:
		{
			SqlTableConstructorExpression sqlTableConstructorExpression = new SqlTableConstructorExpression(value_stack.array[value_stack.top - 1].value as List<SqlRowConstructorExpression>);
			sqlTableConstructorExpression.Position = yyloc;
			yyval.value = sqlTableConstructorExpression;
			break;
		}
		case 2695:
		{
			List<SqlRowConstructorExpression> list4 = new List<SqlRowConstructorExpression>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlRowConstructorExpression);
			yyval.value = list4;
			break;
		}
		case 2696:
		{
			List<SqlRowConstructorExpression> list3 = value_stack.array[value_stack.top - 3].value as List<SqlRowConstructorExpression>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlRowConstructorExpression);
			yyval.value = list3;
			break;
		}
		case 2697:
		{
			SqlRowConstructorExpression sqlRowConstructorExpression = new SqlRowConstructorExpression(value_stack.array[value_stack.top - 2].value as List<SqlScalarExpression>);
			sqlRowConstructorExpression.Position = yyloc;
			yyval.value = sqlRowConstructorExpression;
			break;
		}
		case 2698:
		{
			List<SqlScalarExpression> list2 = new List<SqlScalarExpression>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list2;
			break;
		}
		case 2699:
		{
			List<SqlScalarExpression> list = value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list;
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
		case 2661:
		case 2663:
		case 2668:
		case 2678:
		case 2680:
		case 2685:
			break;
		}
	}

	private void DoAction_2700_2749(int action)
	{
		switch (action)
		{
		case 2701:
		{
			SqlWhereClause sqlWhereClause = new SqlWhereClause(value_stack.array[value_stack.top - 1].value as SqlBooleanExpression);
			sqlWhereClause.Position = yyloc;
			yyval.value = sqlWhereClause;
			scanner.SetQueryState(QueryState.None);
			break;
		}
		case 2705:
		{
			SqlGroupByClause sqlGroupByClause = new SqlGroupByClause(value_stack.array[value_stack.top - 3].value as List<SqlGroupByItem>, value_stack.array[value_stack.top - 1].value as string, (bool)value_stack.array[value_stack.top - 4].value);
			sqlGroupByClause.Position = yyloc;
			yyval.value = sqlGroupByClause;
			scanner.SetParseState(EParseState.e_ParseNeutral);
			break;
		}
		case 2706:
			yyval.value = false;
			break;
		case 2707:
			scanner.SetQueryStatus(QueryStatus.Q_BYALL);
			yyval.value = true;
			break;
		case 2708:
			yyval.value = "";
			break;
		case 2709:
			yyval.value = GetText(location_stack.array[location_stack.top - 2]) + " " + GetText(location_stack.array[location_stack.top - 1]);
			break;
		case 2710:
		{
			List<SqlGroupByItem> list11 = new List<SqlGroupByItem>();
			list11.Add(value_stack.array[value_stack.top - 1].value as SqlGroupByItem);
			yyval.value = list11;
			break;
		}
		case 2712:
		{
			List<SqlGroupByItem> list10 = new List<SqlGroupByItem>();
			list10.Add(value_stack.array[value_stack.top - 3].value as SqlGroupByItem);
			list10.Add(value_stack.array[value_stack.top - 1].value as SqlGroupByItem);
			yyval.value = list10;
			break;
		}
		case 2713:
		{
			List<SqlGroupByItem> list9 = value_stack.array[value_stack.top - 3].value as List<SqlGroupByItem>;
			list9.Add(value_stack.array[value_stack.top - 1].value as SqlGroupByItem);
			yyval.value = list9;
			break;
		}
		case 2720:
		{
			SqlGroupBySets sqlGroupBySets = new SqlGroupBySets(value_stack.array[value_stack.top - 2].value as List<SqlGroupingSet>);
			sqlGroupBySets.Position = yyloc;
			yyval.value = sqlGroupBySets;
			ExpectKeyword(StringToken.Grouping, location_stack.array[location_stack.top - 5]);
			ExpectKeyword(StringToken.Sets, location_stack.array[location_stack.top - 4]);
			break;
		}
		case 2721:
		{
			List<SqlGroupingSet> list8 = new List<SqlGroupingSet>();
			list8.Add(value_stack.array[value_stack.top - 1].value as SqlGroupingSet);
			yyval.value = list8;
			break;
		}
		case 2722:
		{
			List<SqlGroupingSet> list7 = value_stack.array[value_stack.top - 3].value as List<SqlGroupingSet>;
			list7.Add(value_stack.array[value_stack.top - 1].value as SqlGroupingSet);
			yyval.value = list7;
			break;
		}
		case 2723:
		{
			SqlGrandTotalGroupingSet sqlGrandTotalGroupingSet = new SqlGrandTotalGroupingSet(value_stack.array[value_stack.top - 1].value as SqlGrandTotalGroupByItem);
			sqlGrandTotalGroupingSet.Position = yyloc;
			yyval.value = sqlGrandTotalGroupingSet;
			break;
		}
		case 2724:
		{
			SqlGroupingSetItemsCollection sqlGroupingSetItemsCollection4 = new SqlGroupingSetItemsCollection(value_stack.array[value_stack.top - 1].value as SqlGroupingSetItem);
			sqlGroupingSetItemsCollection4.Position = yyloc;
			yyval.value = sqlGroupingSetItemsCollection4;
			break;
		}
		case 2725:
		{
			SqlGroupingSetItemsCollection sqlGroupingSetItemsCollection3 = new SqlGroupingSetItemsCollection(value_stack.array[value_stack.top - 2].value as List<SqlGroupingSetItem>);
			sqlGroupingSetItemsCollection3.Position = yyloc;
			yyval.value = sqlGroupingSetItemsCollection3;
			break;
		}
		case 2726:
		{
			SqlGroupingSetItemsCollection sqlGroupingSetItemsCollection2 = new SqlGroupingSetItemsCollection(value_stack.array[value_stack.top - 2].value as SqlRollupGroupByItem);
			sqlGroupingSetItemsCollection2.Position = yyloc;
			yyval.value = sqlGroupingSetItemsCollection2;
			break;
		}
		case 2727:
		{
			SqlGroupingSetItemsCollection sqlGroupingSetItemsCollection = new SqlGroupingSetItemsCollection(value_stack.array[value_stack.top - 2].value as SqlCubeGroupByItem);
			sqlGroupingSetItemsCollection.Position = yyloc;
			yyval.value = sqlGroupingSetItemsCollection;
			break;
		}
		case 2728:
		{
			SqlGrandTotalGroupByItem sqlGrandTotalGroupByItem = new SqlGrandTotalGroupByItem();
			sqlGrandTotalGroupByItem.Position = yyloc;
			yyval.value = sqlGrandTotalGroupByItem;
			break;
		}
		case 2729:
		{
			List<SqlGroupingSetItem> list6 = new List<SqlGroupingSetItem>();
			list6.Add(value_stack.array[value_stack.top - 3].value as SqlGroupingSetItem);
			list6.Add(value_stack.array[value_stack.top - 1].value as SqlGroupingSetItem);
			yyval.value = list6;
			break;
		}
		case 2730:
		{
			List<SqlGroupingSetItem> list5 = value_stack.array[value_stack.top - 3].value as List<SqlGroupingSetItem>;
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlGroupingSetItem);
			yyval.value = list5;
			break;
		}
		case 2734:
		{
			SqlRollupGroupByItem sqlRollupGroupByItem = new SqlRollupGroupByItem(value_stack.array[value_stack.top - 2].value as List<SqlSimpleGroupByItem>);
			sqlRollupGroupByItem.Position = yyloc;
			yyval.value = sqlRollupGroupByItem;
			break;
		}
		case 2735:
		{
			SqlCubeGroupByItem sqlCubeGroupByItem = new SqlCubeGroupByItem(value_stack.array[value_stack.top - 2].value as List<SqlSimpleGroupByItem>);
			sqlCubeGroupByItem.Position = yyloc;
			yyval.value = sqlCubeGroupByItem;
			break;
		}
		case 2737:
		{
			List<SqlSimpleGroupByItem> list4 = value_stack.array[value_stack.top - 3].value as List<SqlSimpleGroupByItem>;
			list4.AddRange(value_stack.array[value_stack.top - 1].value as List<SqlSimpleGroupByItem>);
			yyval.value = list4;
			break;
		}
		case 2738:
		{
			List<SqlSimpleGroupByItem> list3 = new List<SqlSimpleGroupByItem>();
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlSimpleGroupByItem);
			yyval.value = list3;
			break;
		}
		case 2739:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 2740:
		{
			List<SqlSimpleGroupByItem> list2 = new List<SqlSimpleGroupByItem>();
			list2.Add(value_stack.array[value_stack.top - 3].value as SqlSimpleGroupByItem);
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlSimpleGroupByItem);
			yyval.value = list2;
			break;
		}
		case 2741:
		{
			List<SqlSimpleGroupByItem> list = value_stack.array[value_stack.top - 3].value as List<SqlSimpleGroupByItem>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlSimpleGroupByItem);
			yyval.value = list;
			break;
		}
		case 2742:
		{
			SqlSimpleGroupByItem sqlSimpleGroupByItem3 = new SqlSimpleGroupByItem(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlSimpleGroupByItem3.Position = yyloc;
			yyval.value = sqlSimpleGroupByItem3;
			break;
		}
		case 2743:
		{
			SqlSimpleGroupByItem sqlSimpleGroupByItem2 = new SqlSimpleGroupByItem(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlSimpleGroupByItem2.Position = yyloc;
			yyval.value = sqlSimpleGroupByItem2;
			break;
		}
		case 2744:
		{
			SqlSimpleGroupByItem sqlSimpleGroupByItem = new SqlSimpleGroupByItem(value_stack.array[value_stack.top - 5].value as SqlScalarExpression);
			sqlSimpleGroupByItem.Position = yyloc;
			yyval.value = sqlSimpleGroupByItem;
			break;
		}
		case 2748:
		{
			SqlHavingClause sqlHavingClause = new SqlHavingClause(value_stack.array[value_stack.top - 1].value as SqlBooleanExpression);
			sqlHavingClause.Position = yyloc;
			yyval.value = sqlHavingClause;
			break;
		}
		case 2700:
		case 2702:
		case 2703:
		case 2704:
		case 2711:
		case 2714:
		case 2715:
		case 2716:
		case 2717:
		case 2718:
		case 2719:
		case 2731:
		case 2732:
		case 2733:
		case 2736:
		case 2745:
		case 2746:
		case 2747:
		case 2749:
			break;
		}
	}

	private void DoAction_2750_2799(int action)
	{
		switch (action)
		{
		case 2753:
		{
			SqlOrderByClause sqlOrderByClause2 = new SqlOrderByClause(value_stack.array[value_stack.top - 1].value as List<SqlOrderByItem>);
			sqlOrderByClause2.Position = yyloc;
			yyval.value = sqlOrderByClause2;
			break;
		}
		case 2754:
		{
			SqlOrderByClause sqlOrderByClause = value_stack.array[value_stack.top - 2].value as SqlOrderByClause;
			sqlOrderByClause.OffsetFetchClause = value_stack.array[value_stack.top - 1].value as SqlOffsetFetchClause;
			sqlOrderByClause.Position = yyloc;
			yyval.value = sqlOrderByClause;
			break;
		}
		case 2755:
		{
			List<SqlOrderByItem> list2 = new List<SqlOrderByItem>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlOrderByItem);
			yyval.value = list2;
			break;
		}
		case 2756:
		{
			List<SqlOrderByItem> list = value_stack.array[value_stack.top - 3].value as List<SqlOrderByItem>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlOrderByItem);
			yyval.value = list;
			break;
		}
		case 2757:
		{
			SqlOrderByItem sqlOrderByItem = new SqlOrderByItem(value_stack.array[value_stack.top - 2].value as SqlScalarExpression, (SqlSortOrder)value_stack.array[value_stack.top - 1].value);
			sqlOrderByItem.Position = yyloc;
			yyval.value = sqlOrderByItem;
			break;
		}
		case 2758:
			yyval.value = SqlSortOrder.None;
			break;
		case 2759:
			yyval.value = SqlSortOrder.Ascending;
			break;
		case 2760:
			yyval.value = SqlSortOrder.Descending;
			break;
		case 2762:
		{
			SqlBooleanExpressionError sqlBooleanExpressionError = new SqlBooleanExpressionError(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBooleanExpressionError.Position = yyloc;
			yyval.value = sqlBooleanExpressionError;
			break;
		}
		case 2763:
		{
			SqlBooleanFilterExpression sqlBooleanFilterExpression3 = SqlBooleanFilterExpression.Create(value_stack.array[value_stack.top - 1].value as SqlComparisonBooleanExpression);
			sqlBooleanFilterExpression3.Position = yyloc;
			yyval.value = sqlBooleanFilterExpression3;
			break;
		}
		case 2764:
		{
			SqlBooleanFilterExpression sqlBooleanFilterExpression2 = SqlBooleanFilterExpression.Create(value_stack.array[value_stack.top - 1].value as SqlIsNullBooleanExpression);
			sqlBooleanFilterExpression2.Position = yyloc;
			yyval.value = sqlBooleanFilterExpression2;
			break;
		}
		case 2765:
		{
			SqlBooleanFilterExpression sqlBooleanFilterExpression = SqlBooleanFilterExpression.Create(value_stack.array[value_stack.top - 1].value as SqlInBooleanExpression);
			sqlBooleanFilterExpression.Position = yyloc;
			yyval.value = sqlBooleanFilterExpression;
			break;
		}
		case 2766:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 2767:
		{
			SqlBinaryFilterExpression sqlBinaryFilterExpression = new SqlBinaryFilterExpression(value_stack.array[value_stack.top - 3].value as SqlFilterExpression, value_stack.array[value_stack.top - 1].value as SqlFilterExpression);
			sqlBinaryFilterExpression.Position = yyloc;
			yyval.value = sqlBinaryFilterExpression;
			break;
		}
		case 2777:
		{
			SqlBooleanExpression sqlBooleanExpression = value_stack.array[value_stack.top - 2].value as SqlBooleanExpression;
			sqlBooleanExpression.Position = yyloc;
			yyval.value = sqlBooleanExpression;
			break;
		}
		case 2779:
		{
			SqlNotBooleanExpression sqlNotBooleanExpression = new SqlNotBooleanExpression(value_stack.array[value_stack.top - 1].value as SqlBooleanExpression);
			sqlNotBooleanExpression.Position = yyloc;
			yyval.value = sqlNotBooleanExpression;
			break;
		}
		case 2780:
		{
			SqlBinaryBooleanExpression sqlBinaryBooleanExpression2 = new SqlBinaryBooleanExpression(value_stack.array[value_stack.top - 3].value as SqlBooleanExpression, SqlBooleanOperatorType.And, value_stack.array[value_stack.top - 1].value as SqlBooleanExpression);
			sqlBinaryBooleanExpression2.Position = yyloc;
			yyval.value = sqlBinaryBooleanExpression2;
			break;
		}
		case 2781:
		{
			SqlBinaryBooleanExpression sqlBinaryBooleanExpression = new SqlBinaryBooleanExpression(value_stack.array[value_stack.top - 3].value as SqlBooleanExpression, SqlBooleanOperatorType.Or, value_stack.array[value_stack.top - 1].value as SqlBooleanExpression);
			sqlBinaryBooleanExpression.Position = yyloc;
			yyval.value = sqlBinaryBooleanExpression;
			break;
		}
		case 2782:
		{
			SqlComparisonBooleanExpression sqlComparisonBooleanExpression = new SqlComparisonBooleanExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, (SqlComparisonBooleanExpressionType)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlComparisonBooleanExpression.Position = yyloc;
			yyval.value = sqlComparisonBooleanExpression;
			break;
		}
		case 2783:
			yyval.value = SqlComparisonBooleanExpressionType.Equals;
			break;
		case 2784:
			yyval.value = SqlComparisonBooleanExpressionType.NotEqual;
			break;
		case 2785:
			yyval.value = SqlComparisonBooleanExpressionType.GreaterThan;
			break;
		case 2786:
			yyval.value = SqlComparisonBooleanExpressionType.GreaterThanOrEqual;
			break;
		case 2787:
			yyval.value = SqlComparisonBooleanExpressionType.LessThan;
			break;
		case 2788:
			yyval.value = SqlComparisonBooleanExpressionType.LessOrGreaterThan;
			break;
		case 2789:
			yyval.value = SqlComparisonBooleanExpressionType.LessThanOrEqual;
			break;
		case 2790:
			yyval.value = SqlComparisonBooleanExpressionType.NotLessThan;
			break;
		case 2791:
			yyval.value = SqlComparisonBooleanExpressionType.NotGreaterThan;
			break;
		case 2792:
		{
			SqlBetweenBooleanExpression sqlBetweenBooleanExpression2 = new SqlBetweenBooleanExpression(value_stack.array[value_stack.top - 5].value as SqlScalarExpression, value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBetweenBooleanExpression2.Position = yyloc;
			yyval.value = sqlBetweenBooleanExpression2;
			break;
		}
		case 2793:
		{
			SqlBetweenBooleanExpression sqlBetweenBooleanExpression = new SqlBetweenBooleanExpression(value_stack.array[value_stack.top - 6].value as SqlScalarExpression, hasNot: true, value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBetweenBooleanExpression.Position = yyloc;
			yyval.value = sqlBetweenBooleanExpression;
			break;
		}
		case 2794:
		{
			SqlIsNullBooleanExpression sqlIsNullBooleanExpression2 = new SqlIsNullBooleanExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression);
			sqlIsNullBooleanExpression2.Position = yyloc;
			yyval.value = sqlIsNullBooleanExpression2;
			break;
		}
		case 2795:
		{
			SqlIsNullBooleanExpression sqlIsNullBooleanExpression = new SqlIsNullBooleanExpression(value_stack.array[value_stack.top - 4].value as SqlScalarExpression, hasNot: true);
			sqlIsNullBooleanExpression.Position = yyloc;
			yyval.value = sqlIsNullBooleanExpression;
			break;
		}
		case 2796:
		{
			SqlInBooleanExpression sqlInBooleanExpression2 = new SqlInBooleanExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlInBooleanExpressionValue);
			sqlInBooleanExpression2.Position = yyloc;
			yyval.value = sqlInBooleanExpression2;
			break;
		}
		case 2797:
		{
			SqlInBooleanExpression sqlInBooleanExpression = new SqlInBooleanExpression(value_stack.array[value_stack.top - 4].value as SqlScalarExpression, hasNot: true, value_stack.array[value_stack.top - 1].value as SqlInBooleanExpressionValue);
			sqlInBooleanExpression.Position = yyloc;
			yyval.value = sqlInBooleanExpression;
			break;
		}
		case 2798:
		{
			SqlInBooleanExpressionQueryValue sqlInBooleanExpressionQueryValue = new SqlInBooleanExpressionQueryValue(value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlInBooleanExpressionQueryValue.Position = yyloc;
			yyval.value = sqlInBooleanExpressionQueryValue;
			break;
		}
		case 2799:
		{
			SqlInBooleanExpressionCollectionValue sqlInBooleanExpressionCollectionValue = new SqlInBooleanExpressionCollectionValue(value_stack.array[value_stack.top - 2].value as List<SqlScalarExpression>);
			sqlInBooleanExpressionCollectionValue.Position = yyloc;
			yyval.value = sqlInBooleanExpressionCollectionValue;
			break;
		}
		case 2750:
		case 2751:
		case 2752:
		case 2761:
		case 2768:
		case 2769:
		case 2770:
		case 2771:
		case 2772:
		case 2773:
		case 2774:
		case 2775:
		case 2776:
		case 2778:
			break;
		}
	}

	private void DoAction_2800_2849(int action)
	{
		switch (action)
		{
		case 2800:
		{
			List<SqlScalarExpression> list4 = new List<SqlScalarExpression>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list4;
			break;
		}
		case 2801:
			yyval.value = value_stack.array[value_stack.top - 1].value as List<SqlScalarExpression>;
			break;
		case 2802:
		{
			List<SqlScalarExpression> list2 = new List<SqlScalarExpression>();
			list2.Add(value_stack.array[value_stack.top - 3].value as SqlScalarExpression);
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list2;
			break;
		}
		case 2803:
		{
			List<SqlScalarExpression> list3 = value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list3;
			break;
		}
		case 2804:
		{
			SqlFullTextBooleanExpression sqlFullTextBooleanExpression = new SqlFullTextBooleanExpression((SqlFullTextFunctionType)value_stack.array[value_stack.top - 7].value, value_stack.array[value_stack.top - 5].value as List<SqlFullTextColumn>, value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 2].value as SqlScalarExpression);
			sqlFullTextBooleanExpression.Position = yyloc;
			yyval.value = sqlFullTextBooleanExpression;
			break;
		}
		case 2805:
			yyval.value = SqlFullTextFunctionType.Contains;
			break;
		case 2806:
			yyval.value = SqlFullTextFunctionType.FreeText;
			break;
		case 2807:
		{
			List<SqlFullTextColumn> list = new List<SqlFullTextColumn>();
			list.Add(value_stack.array[value_stack.top - 1].value as SqlFullTextColumn);
			yyval.value = list;
			break;
		}
		case 2808:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 2810:
		{
			SqlFullTextColumn sqlFullTextColumn5 = new SqlFullTextColumn(value_stack.array[value_stack.top - 1].value as SqlScalarRefExpression);
			sqlFullTextColumn5.Position = yyloc;
			List<SqlFullTextColumn> list6 = new List<SqlFullTextColumn>();
			list6.Add(sqlFullTextColumn5);
			yyval.value = list6;
			break;
		}
		case 2811:
		{
			SqlFullTextColumn sqlFullTextColumn4 = new SqlFullTextColumn(value_stack.array[value_stack.top - 1].value as SqlScalarRefExpression);
			sqlFullTextColumn4.Position = location_stack.array[location_stack.top - 1];
			List<SqlFullTextColumn> list5 = value_stack.array[value_stack.top - 3].value as List<SqlFullTextColumn>;
			list5.Add(sqlFullTextColumn4);
			yyval.value = list5;
			break;
		}
		case 2812:
		{
			SqlFullTextColumn sqlFullTextColumn3 = new SqlFullTextColumn(value_stack.array[value_stack.top - 1].value as SqlScalarRefExpression);
			sqlFullTextColumn3.Position = yyloc;
			yyval.value = sqlFullTextColumn3;
			break;
		}
		case 2813:
		{
			SqlFullTextColumn sqlFullTextColumn2 = new SqlFullTextColumn(value_stack.array[value_stack.top - 1].value as SqlSelectStarExpression);
			sqlFullTextColumn2.Position = yyloc;
			yyval.value = sqlFullTextColumn2;
			break;
		}
		case 2814:
		{
			SqlFullTextColumn sqlFullTextColumn = new SqlFullTextColumn(value_stack.array[value_stack.top - 2].value as SqlSelectStarExpression);
			sqlFullTextColumn.Position = yyloc;
			yyval.value = sqlFullTextColumn;
			break;
		}
		case 2815:
		{
			SqlSelectStarExpression sqlSelectStarExpression2 = new SqlSelectStarExpression();
			sqlSelectStarExpression2.Position = yyloc;
			yyval.value = sqlSelectStarExpression2;
			break;
		}
		case 2816:
		{
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>);
			sqlObjectIdentifier.Position = location_stack.array[location_stack.top - 2];
			SqlSelectStarExpression sqlSelectStarExpression = new SqlSelectStarExpression(sqlObjectIdentifier);
			sqlSelectStarExpression.Position = yyloc;
			yyval.value = sqlSelectStarExpression;
			break;
		}
		case 2824:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2825:
			if (ExpectKeyword(StringToken.Language, location_stack.array[location_stack.top - 2]))
			{
				yyval = value_stack.array[value_stack.top - 1];
			}
			break;
		case 2826:
		{
			SqlLiteralExpression sqlLiteralExpression2 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression2.Position = yyloc;
			yyval.value = sqlLiteralExpression2;
			break;
		}
		case 2827:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Binary(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression.Position = yyloc;
			yyval.value = sqlLiteralExpression;
			break;
		}
		case 2829:
		{
			SqlAllAnyComparisonBooleanExpression sqlAllAnyComparisonBooleanExpression2 = new SqlAllAnyComparisonBooleanExpression(value_stack.array[value_stack.top - 4].value as SqlScalarExpression, (SqlComparisonBooleanExpressionType)value_stack.array[value_stack.top - 3].value, "ANY", value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlAllAnyComparisonBooleanExpression2.Position = yyloc;
			yyval.value = sqlAllAnyComparisonBooleanExpression2;
			break;
		}
		case 2830:
		{
			SqlAllAnyComparisonBooleanExpression sqlAllAnyComparisonBooleanExpression = new SqlAllAnyComparisonBooleanExpression(value_stack.array[value_stack.top - 4].value as SqlScalarExpression, (SqlComparisonBooleanExpressionType)value_stack.array[value_stack.top - 3].value, "ALL", value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlAllAnyComparisonBooleanExpression.Position = yyloc;
			yyval.value = sqlAllAnyComparisonBooleanExpression;
			break;
		}
		case 2831:
		{
			SqlExistsBooleanExpression sqlExistsBooleanExpression = new SqlExistsBooleanExpression(value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlExistsBooleanExpression.Position = yyloc;
			yyval.value = sqlExistsBooleanExpression;
			break;
		}
		case 2832:
		{
			SqlLikeBooleanExpression sqlLikeBooleanExpression = new SqlLikeBooleanExpression(value_stack.array[value_stack.top - 4].value as SqlScalarExpression, (bool)value_stack.array[value_stack.top - 3].value, value_stack.array[value_stack.top - 2].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlLikeBooleanExpression.Position = yyloc;
			yyval.value = sqlLikeBooleanExpression;
			break;
		}
		case 2833:
			yyval.value = false;
			break;
		case 2834:
			yyval.value = true;
			break;
		case 2838:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2842:
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
		case 2844:
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
		case 2845:
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
		case 2846:
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
		case 2847:
		{
			SqlUnaryScalarExpression sqlUnaryScalarExpression3 = new SqlUnaryScalarExpression(SqlUnaryScalarOperatorType.Positive, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlUnaryScalarExpression3.Position = yyloc;
			yyval.value = sqlUnaryScalarExpression3;
			break;
		}
		case 2848:
		{
			SqlUnaryScalarExpression sqlUnaryScalarExpression2 = new SqlUnaryScalarExpression(SqlUnaryScalarOperatorType.Negative, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlUnaryScalarExpression2.Position = yyloc;
			yyval.value = sqlUnaryScalarExpression2;
			break;
		}
		case 2849:
		{
			SqlUnaryScalarExpression sqlUnaryScalarExpression = new SqlUnaryScalarExpression(SqlUnaryScalarOperatorType.BitwiseNot, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlUnaryScalarExpression.Position = yyloc;
			yyval.value = sqlUnaryScalarExpression;
			break;
		}
		case 2809:
		case 2817:
		case 2818:
		case 2819:
		case 2820:
		case 2821:
		case 2822:
		case 2823:
		case 2828:
		case 2835:
		case 2836:
		case 2837:
		case 2839:
		case 2840:
		case 2841:
		case 2843:
			break;
		}
	}

	private void DoAction_2850_2899(int action)
	{
		switch (action)
		{
		case 2850:
		{
			SqlBinaryScalarExpression sqlBinaryScalarExpression = new SqlBinaryScalarExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, SqlBinaryScalarOperatorType.Multiply, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBinaryScalarExpression.Position = yyloc;
			yyval.value = sqlBinaryScalarExpression;
			break;
		}
		case 2851:
		{
			SqlBinaryScalarExpression sqlBinaryScalarExpression8 = new SqlBinaryScalarExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, SqlBinaryScalarOperatorType.Divide, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBinaryScalarExpression8.Position = yyloc;
			yyval.value = sqlBinaryScalarExpression8;
			break;
		}
		case 2852:
		{
			SqlBinaryScalarExpression sqlBinaryScalarExpression7 = new SqlBinaryScalarExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, SqlBinaryScalarOperatorType.Modulus, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBinaryScalarExpression7.Position = yyloc;
			yyval.value = sqlBinaryScalarExpression7;
			break;
		}
		case 2853:
		{
			SqlBinaryScalarExpression sqlBinaryScalarExpression6 = new SqlBinaryScalarExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, SqlBinaryScalarOperatorType.Add, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBinaryScalarExpression6.Position = yyloc;
			yyval.value = sqlBinaryScalarExpression6;
			break;
		}
		case 2854:
		{
			SqlBinaryScalarExpression sqlBinaryScalarExpression5 = new SqlBinaryScalarExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, SqlBinaryScalarOperatorType.Subtract, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBinaryScalarExpression5.Position = yyloc;
			yyval.value = sqlBinaryScalarExpression5;
			break;
		}
		case 2855:
		{
			SqlBinaryScalarExpression sqlBinaryScalarExpression4 = new SqlBinaryScalarExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, SqlBinaryScalarOperatorType.BitwiseAnd, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBinaryScalarExpression4.Position = yyloc;
			yyval.value = sqlBinaryScalarExpression4;
			break;
		}
		case 2856:
		{
			SqlBinaryScalarExpression sqlBinaryScalarExpression3 = new SqlBinaryScalarExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, SqlBinaryScalarOperatorType.BitwiseOr, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBinaryScalarExpression3.Position = yyloc;
			yyval.value = sqlBinaryScalarExpression3;
			break;
		}
		case 2857:
		{
			SqlBinaryScalarExpression sqlBinaryScalarExpression2 = new SqlBinaryScalarExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, SqlBinaryScalarOperatorType.BitwiseXor, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBinaryScalarExpression2.Position = yyloc;
			yyval.value = sqlBinaryScalarExpression2;
			break;
		}
		case 2858:
		{
			SqlAtTimeZoneExpression sqlAtTimeZoneExpression = new SqlAtTimeZoneExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlAtTimeZoneExpression.Position = yyloc;
			yyval.value = sqlAtTimeZoneExpression;
			break;
		}
		case 2859:
		{
			SqlNullScalarExpression sqlNullScalarExpression = new SqlNullScalarExpression();
			sqlNullScalarExpression.Position = yyloc;
			yyval.value = sqlNullScalarExpression;
			break;
		}
		case 2860:
		{
			SqlScalarExpressionError sqlScalarExpressionError = new SqlScalarExpressionError(value_stack.array[value_stack.top - 1].value as SqlErrorCodeObject);
			sqlScalarExpressionError.Position = yyloc;
			yyval.value = sqlScalarExpressionError;
			break;
		}
		case 2861:
		{
			SqlObjectIdentifier sqlObjectIdentifier4 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier4.Position = yyloc;
			SqlDataTypeSpecification sqlDataTypeSpecification2 = new SqlDataTypeSpecification(new SqlDataType(sqlObjectIdentifier4)
			{
				Position = yyloc
			});
			sqlDataTypeSpecification2.Position = yyloc;
			yyval.value = sqlDataTypeSpecification2;
			break;
		}
		case 2862:
		{
			List<SqlIdentifier> obj2 = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			obj2.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			SqlObjectIdentifier sqlObjectIdentifier3 = SqlObjectIdentifier.Create(obj2);
			sqlObjectIdentifier3.Position = yyloc;
			SqlDataTypeSpecification sqlDataTypeSpecification = new SqlDataTypeSpecification(new SqlDataType(sqlObjectIdentifier3)
			{
				Position = yyloc
			});
			sqlDataTypeSpecification.Position = yyloc;
			yyval.value = sqlDataTypeSpecification;
			break;
		}
		case 2867:
			if (value_stack.array[value_stack.top - 2].value is SqlCodeObject sqlCodeObject)
			{
				sqlCodeObject.Position = yyloc;
				yyval.value = sqlCodeObject;
			}
			break;
		case 2870:
		{
			SqlUdtInstanceMethodExpression sqlUdtInstanceMethodExpression = new SqlUdtInstanceMethodExpression(value_stack.array[value_stack.top - 6].value as SqlScalarExpression, value_stack.array[value_stack.top - 4].value as SqlIdentifier, value_stack.array[value_stack.top - 2].value as List<SqlScalarExpression>);
			sqlUdtInstanceMethodExpression.Position = yyloc;
			yyval.value = sqlUdtInstanceMethodExpression;
			break;
		}
		case 2871:
		{
			SqlUdtInstanceDataMemberExpression sqlUdtInstanceDataMemberExpression = new SqlUdtInstanceDataMemberExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlUdtInstanceDataMemberExpression.Position = yyloc;
			yyval.value = sqlUdtInstanceDataMemberExpression;
			break;
		}
		case 2872:
		{
			SqlUdtStaticMethodExpression sqlUdtStaticMethodExpression = new SqlUdtStaticMethodExpression(value_stack.array[value_stack.top - 6].value as SqlDataTypeSpecification, value_stack.array[value_stack.top - 4].value as SqlIdentifier, value_stack.array[value_stack.top - 2].value as List<SqlScalarExpression>);
			sqlUdtStaticMethodExpression.Position = yyloc;
			yyval.value = sqlUdtStaticMethodExpression;
			break;
		}
		case 2873:
		{
			SqlUdtStaticDataMemberExpression sqlUdtStaticDataMemberExpression = new SqlUdtStaticDataMemberExpression(value_stack.array[value_stack.top - 3].value as SqlDataTypeSpecification, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlUdtStaticDataMemberExpression.Position = yyloc;
			yyval.value = sqlUdtStaticDataMemberExpression;
			break;
		}
		case 2880:
		{
			SqlSimpleCaseExpression sqlSimpleCaseExpression = new SqlSimpleCaseExpression(value_stack.array[value_stack.top - 4].value as SqlScalarExpression, value_stack.array[value_stack.top - 3].value as List<SqlSimpleWhenClause>, value_stack.array[value_stack.top - 2].value as SqlScalarExpression);
			sqlSimpleCaseExpression.Position = yyloc;
			yyval.value = sqlSimpleCaseExpression;
			break;
		}
		case 2881:
		{
			SqlSearchedCaseExpression sqlSearchedCaseExpression = new SqlSearchedCaseExpression(value_stack.array[value_stack.top - 3].value as List<SqlSearchedWhenClause>, value_stack.array[value_stack.top - 2].value as SqlScalarExpression);
			sqlSearchedCaseExpression.Position = yyloc;
			yyval.value = sqlSearchedCaseExpression;
			break;
		}
		case 2884:
		{
			List<SqlSimpleWhenClause> list4 = new List<SqlSimpleWhenClause>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlSimpleWhenClause);
			yyval.value = list4;
			break;
		}
		case 2885:
		{
			List<SqlSimpleWhenClause> list3 = value_stack.array[value_stack.top - 2].value as List<SqlSimpleWhenClause>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlSimpleWhenClause);
			yyval.value = list3;
			break;
		}
		case 2886:
		{
			SqlSimpleWhenClause sqlSimpleWhenClause = new SqlSimpleWhenClause(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlSimpleWhenClause.Position = yyloc;
			yyval.value = sqlSimpleWhenClause;
			break;
		}
		case 2887:
		{
			List<SqlSearchedWhenClause> list2 = new List<SqlSearchedWhenClause>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlSearchedWhenClause);
			yyval.value = list2;
			break;
		}
		case 2888:
		{
			List<SqlSearchedWhenClause> list = value_stack.array[value_stack.top - 2].value as List<SqlSearchedWhenClause>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlSearchedWhenClause);
			yyval.value = list;
			break;
		}
		case 2889:
		{
			SqlSearchedWhenClause sqlSearchedWhenClause = new SqlSearchedWhenClause(value_stack.array[value_stack.top - 3].value as SqlBooleanExpression, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlSearchedWhenClause.Position = yyloc;
			yyval.value = sqlSearchedWhenClause;
			break;
		}
		case 2891:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2892:
		{
			SqlObjectIdentifier sqlObjectIdentifier2 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier2.Position = yyloc;
			yyval.value = sqlObjectIdentifier2;
			break;
		}
		case 2893:
		{
			List<SqlIdentifier> obj = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			obj.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(obj);
			sqlObjectIdentifier.Position = yyloc;
			yyval.value = sqlObjectIdentifier;
			break;
		}
		case 2895:
		{
			SqlIdentifier sqlIdentifier = SqlIdentifier.Missing();
			sqlIdentifier.Position = yyloc;
			yyval.value = sqlIdentifier;
			break;
		}
		case 2863:
		case 2864:
		case 2865:
		case 2866:
		case 2868:
		case 2869:
		case 2874:
		case 2875:
		case 2876:
		case 2877:
		case 2878:
		case 2879:
		case 2882:
		case 2883:
		case 2890:
		case 2894:
		case 2896:
		case 2897:
		case 2898:
		case 2899:
			break;
		}
	}

	private void DoAction_2900_2949(int action)
	{
		switch (action)
		{
		case 2901:
		{
			SqlObjectIdentifier sqlObjectIdentifier2 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier2.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression = new SqlColumnRefExpression(sqlObjectIdentifier2);
			sqlColumnRefExpression.Position = yyloc;
			yyval.value = sqlColumnRefExpression;
			break;
		}
		case 2902:
		{
			List<SqlIdentifier> obj = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			obj.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(obj);
			sqlObjectIdentifier.Position = yyloc;
			SqlScalarRefExpression sqlScalarRefExpression = SqlScalarRefExpression.ColumnOrPropertyRefExpression(sqlObjectIdentifier);
			sqlScalarRefExpression.Position = yyloc;
			yyval.value = sqlScalarRefExpression;
			break;
		}
		case 2906:
		{
			SqlObjectIdentifier sqlObjectIdentifier4 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier4.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression3 = new SqlColumnRefExpression(sqlObjectIdentifier4);
			sqlColumnRefExpression3.Position = yyloc;
			yyval.value = sqlColumnRefExpression3;
			break;
		}
		case 2907:
		{
			List<SqlIdentifier> obj2 = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			obj2.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			SqlObjectIdentifier sqlObjectIdentifier3 = SqlObjectIdentifier.Create(obj2);
			sqlObjectIdentifier3.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression2 = new SqlColumnRefExpression(sqlObjectIdentifier3, discardColumnPrefix: true);
			sqlColumnRefExpression2.Position = yyloc;
			yyval.value = sqlColumnRefExpression2;
			break;
		}
		case 2908:
		{
			List<SqlIdentifier> list2 = new List<SqlIdentifier>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list2;
			break;
		}
		case 2909:
		{
			List<SqlIdentifier> list = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list;
			break;
		}
		case 2910:
		{
			SqlIdentifier sqlIdentifier2 = new SqlIdentifier(string.Empty);
			sqlIdentifier2.Position = yyloc;
			yyval.value = sqlIdentifier2;
			break;
		}
		case 2911:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 2912:
		{
			SqlIdentifier sqlIdentifier = SqlIdentifier.Missing();
			sqlIdentifier.Position = location_stack.array[location_stack.top - 2];
			yyval.value = sqlIdentifier;
			break;
		}
		case 2914:
		{
			SqlTableRefExpression sqlTableRefExpression2 = new SqlTableRefExpression(value_stack.array[value_stack.top - 2].value as SqlObjectIdentifier);
			sqlTableRefExpression2.Position = location_stack.array[location_stack.top - 2];
			yyval.value = new DmlTarget(sqlTableRefExpression2, value_stack.array[value_stack.top - 1].value as List<SqlColumnRefExpression>);
			break;
		}
		case 2915:
		{
			SqlTableRefExpression sqlTableRefExpression = new SqlTableRefExpression(value_stack.array[value_stack.top - 4].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 2].value as List<SqlHint>);
			sqlTableRefExpression.Position = MergeLocationRange(location_stack.top - 4, location_stack.top - 2);
			yyval.value = new DmlTarget(sqlTableRefExpression, value_stack.array[value_stack.top - 1].value as List<SqlColumnRefExpression>);
			break;
		}
		case 2916:
		{
			SqlTableValuedFunctionRefExpression sqlTableValuedFunctionRefExpression = new SqlTableValuedFunctionRefExpression(value_stack.array[value_stack.top - 5].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>);
			sqlTableValuedFunctionRefExpression.Position = MergeLocationRange(location_stack.top - 5, location_stack.top - 2);
			yyval.value = new DmlTarget(sqlTableValuedFunctionRefExpression, value_stack.array[value_stack.top - 1].value as List<SqlColumnRefExpression>);
			break;
		}
		case 2917:
			yyval.value = new DmlTarget(value_stack.array[value_stack.top - 2].value as SqlTableExpression, value_stack.array[value_stack.top - 1].value as List<SqlColumnRefExpression>);
			break;
		case 2918:
			yyval.value = new DmlTarget(value_stack.array[value_stack.top - 2].value as SqlTableExpression, value_stack.array[value_stack.top - 1].value as List<SqlColumnRefExpression>);
			break;
		case 2919:
		{
			SqlTableVariableRefExpression sqlTableVariableRefExpression = new SqlTableVariableRefExpression(GetText(location_stack.array[location_stack.top - 2]));
			sqlTableVariableRefExpression.Position = location_stack.array[location_stack.top - 2];
			yyval.value = new DmlTarget(sqlTableVariableRefExpression, value_stack.array[value_stack.top - 1].value as List<SqlColumnRefExpression>);
			break;
		}
		case 2920:
		{
			SqlTableExpressionError sqlTableExpressionError = new SqlTableExpressionError(value_stack.array[value_stack.top - 2].value as SqlErrorCodeObject);
			sqlTableExpressionError.Position = location_stack.array[location_stack.top - 2];
			yyval.value = new DmlTarget(sqlTableExpressionError, value_stack.array[value_stack.top - 1].value as List<SqlColumnRefExpression>);
			break;
		}
		case 2929:
		{
			SqlObjectReference sqlObjectReference2 = new SqlObjectReference(value_stack.array[value_stack.top - 1].value as SqlObjectIdentifier);
			sqlObjectReference2.Position = yyloc;
			yyval.value = sqlObjectReference2;
			break;
		}
		case 2930:
		{
			SqlObjectReference sqlObjectReference = new SqlObjectReference(value_stack.array[value_stack.top - 1].value as SqlScalarVariableRefExpression);
			sqlObjectReference.Position = yyloc;
			yyval.value = sqlObjectReference;
			break;
		}
		case 2933:
		{
			SqlLiteralExpression sqlLiteralExpression6 = SqlLiteralExpression.String(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression6.Position = yyloc;
			yyval.value = sqlLiteralExpression6;
			break;
		}
		case 2935:
		{
			SqlLiteralExpression sqlLiteralExpression5 = SqlLiteralExpression.String(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression5.Position = yyloc;
			yyval.value = sqlLiteralExpression5;
			break;
		}
		case 2936:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2937:
		{
			SqlScalarExpressionError sqlScalarExpressionError = new SqlScalarExpressionError();
			sqlScalarExpressionError.Position = yyloc;
			yyval.value = sqlScalarExpressionError;
			break;
		}
		case 2942:
		{
			SqlLiteralExpression sqlLiteralExpression4 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression4.Position = yyloc;
			yyval.value = sqlLiteralExpression4;
			break;
		}
		case 2943:
			VersionedParserBase.ReportError(SR.IntegerValueOutOfRange(GetText(location_stack.array[location_stack.top - 1])), location_stack.array[location_stack.top - 1]);
			break;
		case 2944:
		{
			SqlLiteralExpression sqlLiteralExpression3 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression3.Position = yyloc;
			yyval.value = sqlLiteralExpression3;
			break;
		}
		case 2945:
			VersionedParserBase.ReportError(SR.IntegerValueOutOfRange(GetText(location_stack.array[location_stack.top - 1])), location_stack.array[location_stack.top - 1]);
			break;
		case 2946:
		{
			SqlLiteralExpression sqlLiteralExpression2 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]), isNegative: true);
			sqlLiteralExpression2.Position = yyloc;
			yyval.value = sqlLiteralExpression2;
			break;
		}
		case 2947:
			if (IsMaxAbsoluteIntegerValue(GetText(location_stack.array[location_stack.top - 1])))
			{
				SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]), isNegative: true);
				sqlLiteralExpression.Position = yyloc;
				yyval.value = sqlLiteralExpression;
			}
			else
			{
				VersionedParserBase.ReportError(SR.IntegerValueOutOfRange(GetText(location_stack.array[location_stack.top - 1])), location_stack.array[location_stack.top - 1]);
			}
			break;
		case 2900:
		case 2903:
		case 2904:
		case 2905:
		case 2913:
		case 2921:
		case 2922:
		case 2923:
		case 2924:
		case 2925:
		case 2926:
		case 2927:
		case 2928:
		case 2931:
		case 2932:
		case 2934:
		case 2938:
		case 2939:
		case 2940:
		case 2941:
		case 2948:
		case 2949:
			break;
		}
	}

	private void DoAction_2950_2999(int action)
	{
		switch (action)
		{
		case 2952:
		{
			SqlLiteralExpression sqlLiteralExpression12 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression12.Position = yyloc;
			yyval.value = sqlLiteralExpression12;
			break;
		}
		case 2953:
		{
			SqlLiteralExpression sqlLiteralExpression11 = SqlLiteralExpression.Real(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression11.Position = yyloc;
			yyval.value = sqlLiteralExpression11;
			break;
		}
		case 2954:
		{
			SqlLiteralExpression sqlLiteralExpression10 = SqlLiteralExpression.Money(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression10.Position = yyloc;
			yyval.value = sqlLiteralExpression10;
			break;
		}
		case 2955:
		{
			SqlLiteralExpression sqlLiteralExpression9 = SqlLiteralExpression.Numeric(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression9.Position = yyloc;
			yyval.value = sqlLiteralExpression9;
			break;
		}
		case 2956:
		{
			SqlLiteralExpression sqlLiteralExpression8 = SqlLiteralExpression.Binary(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression8.Position = yyloc;
			yyval.value = sqlLiteralExpression8;
			break;
		}
		case 2966:
		{
			SqlLiteralExpression sqlLiteralExpression7 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]), isNegative: true);
			sqlLiteralExpression7.Position = yyloc;
			yyval.value = sqlLiteralExpression7;
			break;
		}
		case 2967:
		{
			SqlLiteralExpression sqlLiteralExpression6 = SqlLiteralExpression.Real(GetText(location_stack.array[location_stack.top - 1]), isNegative: true);
			sqlLiteralExpression6.Position = yyloc;
			yyval.value = sqlLiteralExpression6;
			break;
		}
		case 2968:
		{
			SqlLiteralExpression sqlLiteralExpression5 = SqlLiteralExpression.Money(GetText(location_stack.array[location_stack.top - 1]), isNegative: true);
			sqlLiteralExpression5.Position = yyloc;
			yyval.value = sqlLiteralExpression5;
			break;
		}
		case 2969:
		{
			SqlLiteralExpression sqlLiteralExpression4 = SqlLiteralExpression.Numeric(GetText(location_stack.array[location_stack.top - 1]), isNegative: true);
			sqlLiteralExpression4.Position = yyloc;
			yyval.value = sqlLiteralExpression4;
			break;
		}
		case 2972:
		{
			SqlLiteralExpression sqlLiteralExpression3 = SqlLiteralExpression.Identifier(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlLiteralExpression3.Position = yyloc;
			yyval.value = sqlLiteralExpression3;
			break;
		}
		case 2973:
		{
			SqlScalarExpressionError sqlScalarExpressionError = new SqlScalarExpressionError();
			sqlScalarExpressionError.Position = yyloc;
			yyval.value = sqlScalarExpressionError;
			break;
		}
		case 2976:
		{
			SqlLiteralExpression sqlLiteralExpression2 = SqlLiteralExpression.Default(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression2.Position = yyloc;
			yyval.value = sqlLiteralExpression2;
			break;
		}
		case 2977:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Null(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression.Position = yyloc;
			yyval.value = sqlLiteralExpression;
			break;
		}
		case 2978:
		{
			SqlScalarVariableRefExpression sqlScalarVariableRefExpression = SqlScalarVariableRefExpression.Create(GetText(location_stack.array[location_stack.top - 1]));
			sqlScalarVariableRefExpression.Position = yyloc;
			yyval.value = sqlScalarVariableRefExpression;
			break;
		}
		case 2979:
		{
			SqlGlobalScalarVariableRefExpression sqlGlobalScalarVariableRefExpression = new SqlGlobalScalarVariableRefExpression(GetText(location_stack.array[location_stack.top - 1]));
			sqlGlobalScalarVariableRefExpression.Position = yyloc;
			yyval.value = sqlGlobalScalarVariableRefExpression;
			break;
		}
		case 2950:
		case 2951:
		case 2957:
		case 2958:
		case 2959:
		case 2960:
		case 2961:
		case 2962:
		case 2963:
		case 2964:
		case 2965:
		case 2970:
		case 2971:
		case 2974:
		case 2975:
		case 2980:
		case 2981:
		case 2982:
		case 2983:
		case 2984:
		case 2985:
		case 2986:
		case 2987:
		case 2988:
		case 2989:
		case 2990:
		case 2991:
		case 2992:
		case 2993:
		case 2994:
		case 2995:
		case 2996:
		case 2997:
		case 2998:
		case 2999:
			break;
		}
	}

	private void DoAction_3000_3049(int action)
	{
		switch (action)
		{
		case 3007:
		{
			SqlBuiltinScalarFunctionCallExpression sqlBuiltinScalarFunctionCallExpression6 = new SqlBuiltinScalarFunctionCallExpression(GetText(location_stack.array[location_stack.top - 4]), value_stack.array[value_stack.top - 2].value as List<SqlScalarExpression>);
			sqlBuiltinScalarFunctionCallExpression6.Position = yyloc;
			yyval.value = sqlBuiltinScalarFunctionCallExpression6;
			break;
		}
		case 3008:
		{
			SqlBuiltinScalarFunctionCallExpression sqlBuiltinScalarFunctionCallExpression7 = new SqlBuiltinScalarFunctionCallExpression(GetText(location_stack.array[location_stack.top - 4]), value_stack.array[value_stack.top - 2].value as List<SqlScalarExpression>);
			sqlBuiltinScalarFunctionCallExpression7.Position = yyloc;
			yyval.value = sqlBuiltinScalarFunctionCallExpression7;
			break;
		}
		case 3009:
		{
			SqlBuiltinScalarFunctionCallExpression sqlBuiltinScalarFunctionCallExpression8 = SqlBuiltinScalarFunctionCallExpression.Create(value_stack.array[value_stack.top - 5].value as string, value_stack.array[value_stack.top - 4].value as List<SqlScalarExpression>);
			sqlBuiltinScalarFunctionCallExpression8.Position = yyloc;
			yyval.value = sqlBuiltinScalarFunctionCallExpression8;
			break;
		}
		case 3011:
		{
			SqlAggregateFunctionCallExpression sqlAggregateFunctionCallExpression = SqlAggregateFunctionCallExpression.Create(GetText(location_stack.array[location_stack.top - 6]), (SqlSetQuantifier)value_stack.array[value_stack.top - 4].value, value_stack.array[value_stack.top - 3].value as SqlScalarExpression);
			sqlAggregateFunctionCallExpression.Position = yyloc;
			yyval.value = sqlAggregateFunctionCallExpression;
			break;
		}
		case 3013:
		{
			SqlCastExpression sqlCastExpression = new SqlCastExpression(value_stack.array[value_stack.top - 6].value as string, value_stack.array[value_stack.top - 3].value as SqlDataTypeSpecification, value_stack.array[value_stack.top - 5].value as SqlScalarExpression);
			sqlCastExpression.Position = yyloc;
			yyval.value = sqlCastExpression;
			scanner.ClearQueryStatus(QueryStatus.Q_FN_PARSE);
			break;
		}
		case 3014:
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
		case 3015:
		{
			SqlBuiltinScalarFunctionCallExpression sqlBuiltinScalarFunctionCallExpression5 = new SqlBuiltinScalarFunctionCallExpression(GetText(location_stack.array[location_stack.top - 1]));
			sqlBuiltinScalarFunctionCallExpression5.Position = yyloc;
			yyval.value = sqlBuiltinScalarFunctionCallExpression5;
			break;
		}
		case 3016:
		{
			SqlBuiltinScalarFunctionCallExpression sqlBuiltinScalarFunctionCallExpression4 = new SqlBuiltinScalarFunctionCallExpression(GetText(location_stack.array[location_stack.top - 1]));
			sqlBuiltinScalarFunctionCallExpression4.Position = yyloc;
			yyval.value = sqlBuiltinScalarFunctionCallExpression4;
			break;
		}
		case 3017:
		{
			SqlBuiltinScalarFunctionCallExpression sqlBuiltinScalarFunctionCallExpression3 = new SqlBuiltinScalarFunctionCallExpression(GetText(location_stack.array[location_stack.top - 1]));
			sqlBuiltinScalarFunctionCallExpression3.Position = yyloc;
			yyval.value = sqlBuiltinScalarFunctionCallExpression3;
			break;
		}
		case 3018:
		{
			SqlBuiltinScalarFunctionCallExpression sqlBuiltinScalarFunctionCallExpression2 = new SqlBuiltinScalarFunctionCallExpression(GetText(location_stack.array[location_stack.top - 1]));
			sqlBuiltinScalarFunctionCallExpression2.Position = yyloc;
			yyval.value = sqlBuiltinScalarFunctionCallExpression2;
			break;
		}
		case 3019:
		{
			SqlBuiltinScalarFunctionCallExpression sqlBuiltinScalarFunctionCallExpression = new SqlBuiltinScalarFunctionCallExpression(GetText(location_stack.array[location_stack.top - 1]));
			sqlBuiltinScalarFunctionCallExpression.Position = yyloc;
			yyval.value = sqlBuiltinScalarFunctionCallExpression;
			break;
		}
		case 3021:
		{
			SqlConvertExpression sqlConvertExpression2 = new SqlConvertExpression(GetText(location_stack.array[location_stack.top - 7]), value_stack.array[value_stack.top - 5].value as SqlDataTypeSpecification, value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 2].value as SqlScalarExpression);
			sqlConvertExpression2.Position = yyloc;
			yyval.value = sqlConvertExpression2;
			break;
		}
		case 3022:
		{
			SqlConvertExpression sqlConvertExpression = new SqlConvertExpression(GetText(location_stack.array[location_stack.top - 7]), value_stack.array[value_stack.top - 5].value as SqlDataTypeSpecification, value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 2].value as SqlScalarExpression);
			sqlConvertExpression.Position = yyloc;
			yyval.value = sqlConvertExpression;
			break;
		}
		case 3023:
			yyval.value = GetText(location_stack.array[location_stack.top - 2]);
			break;
		case 3045:
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
		case 3048:
		{
			List<SqlScalarExpression> list2 = new List<SqlScalarExpression>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list2;
			break;
		}
		case 3049:
		{
			List<SqlScalarExpression> list = value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list;
			break;
		}
		case 3000:
		case 3001:
		case 3002:
		case 3003:
		case 3004:
		case 3005:
		case 3006:
		case 3010:
		case 3012:
		case 3020:
		case 3024:
		case 3025:
		case 3026:
		case 3027:
		case 3028:
		case 3029:
		case 3030:
		case 3031:
		case 3032:
		case 3033:
		case 3034:
		case 3035:
		case 3036:
		case 3037:
		case 3038:
		case 3039:
		case 3040:
		case 3041:
		case 3042:
		case 3043:
		case 3044:
		case 3046:
		case 3047:
			break;
		}
	}

	private void DoAction_3050_3099(int action)
	{
		switch (action)
		{
		case 3052:
		{
			List<SqlScalarExpression> list2 = new List<SqlScalarExpression>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list2;
			break;
		}
		case 3053:
		{
			List<SqlScalarExpression> list = value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list;
			break;
		}
		case 3055:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 3058:
		{
			SqlDataTypeSpecification sqlDataTypeSpecification4 = new SqlDataTypeSpecification(value_stack.array[value_stack.top - 1].value as SqlDataType);
			sqlDataTypeSpecification4.Position = yyloc;
			yyval.value = sqlDataTypeSpecification4;
			break;
		}
		case 3059:
		{
			SqlLiteralExpression sqlLiteralExpression3 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 2]));
			sqlLiteralExpression3.Position = location_stack.array[location_stack.top - 2];
			SqlDataTypeSpecification sqlDataTypeSpecification3 = new SqlDataTypeSpecification(value_stack.array[value_stack.top - 4].value as SqlDataType, sqlLiteralExpression3);
			sqlDataTypeSpecification3.Position = yyloc;
			yyval.value = sqlDataTypeSpecification3;
			break;
		}
		case 3060:
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
		case 3061:
		{
			SqlDataTypeSpecification sqlDataTypeSpecification = new SqlDataTypeSpecification(value_stack.array[value_stack.top - 5].value as SqlDataType, value_stack.array[value_stack.top - 3].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 2].value as SqlObjectIdentifier);
			sqlDataTypeSpecification.Position = yyloc;
			yyval.value = sqlDataTypeSpecification;
			break;
		}
		case 3062:
			yyval.value = SqlSetQuantifier.All;
			break;
		case 3063:
			yyval.value = SqlSetQuantifier.Distinct;
			break;
		case 3064:
		{
			SqlUpdateBooleanExpression sqlUpdateBooleanExpression = new SqlUpdateBooleanExpression(value_stack.array[value_stack.top - 2].value as SqlIdentifier);
			sqlUpdateBooleanExpression.Position = yyloc;
			yyval.value = sqlUpdateBooleanExpression;
			break;
		}
		case 3050:
		case 3051:
		case 3054:
		case 3056:
		case 3057:
		case 3065:
		case 3066:
		case 3067:
		case 3068:
		case 3069:
		case 3070:
		case 3071:
		case 3072:
		case 3073:
		case 3074:
		case 3075:
		case 3076:
		case 3077:
		case 3078:
		case 3079:
		case 3080:
		case 3081:
		case 3082:
		case 3083:
		case 3084:
		case 3085:
		case 3086:
		case 3087:
		case 3088:
		case 3089:
		case 3090:
		case 3091:
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
		case 3128:
		{
			SqlObjectIdentifier sqlObjectIdentifier4 = SqlObjectIdentifier.Create(new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]))
			{
				Position = yyloc
			});
			sqlObjectIdentifier4.Position = yyloc;
			yyval.value = sqlObjectIdentifier4;
			break;
		}
		case 3129:
		{
			SqlObjectIdentifier sqlObjectIdentifier3 = SqlObjectIdentifier.Create(new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]))
			{
				Position = yyloc
			});
			sqlObjectIdentifier3.Position = yyloc;
			yyval.value = sqlObjectIdentifier3;
			break;
		}
		case 3130:
		{
			SqlObjectIdentifier sqlObjectIdentifier2 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier2.Position = yyloc;
			yyval.value = sqlObjectIdentifier2;
			break;
		}
		case 3131:
		{
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 5].value as SqlIdentifier, value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier.Position = yyloc;
			yyval.value = sqlObjectIdentifier;
			break;
		}
		case 3136:
		{
			SqlLiteralExpression sqlLiteralExpression2 = SqlLiteralExpression.String(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression2.Position = yyloc;
			yyval.value = sqlLiteralExpression2;
			break;
		}
		case 3137:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.String(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression.Position = yyloc;
			yyval.value = sqlLiteralExpression;
			break;
		}
		case 3100:
		case 3101:
		case 3102:
		case 3103:
		case 3104:
		case 3105:
		case 3106:
		case 3107:
		case 3108:
		case 3109:
		case 3110:
		case 3111:
		case 3112:
		case 3113:
		case 3114:
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
		case 3132:
		case 3133:
		case 3134:
		case 3135:
		case 3138:
		case 3139:
		case 3140:
		case 3141:
		case 3142:
		case 3143:
		case 3144:
		case 3145:
		case 3146:
		case 3147:
		case 3148:
		case 3149:
			break;
		}
	}

	private void DoAction_3150_3199(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_3200_3249(int action)
	{
		switch (action)
		{
		case 3211:
			scanner.SetQueryStatus(QueryStatus.P_BREAKDECPOINT);
			break;
		case 3212:
			scanner.ClearQueryStatus(QueryStatus.P_BREAKDECPOINT);
			break;
		case 3238:
		{
			SqlCreateSynonymStatement sqlCreateSynonymStatement = new SqlCreateSynonymStatement(value_stack.array[value_stack.top - 3].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 1].value as SqlObjectIdentifier);
			sqlCreateSynonymStatement.Position = yyloc;
			yyval.value = sqlCreateSynonymStatement;
			break;
		}
		case 3200:
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
		}
	}

	private void DoAction_3400_3449(int action)
	{
		switch (action)
		{
		case 3416:
			scanner.SetQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3417:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3426:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3427:
			scanner.SetQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3428:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3429:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3400:
		case 3401:
		case 3402:
		case 3403:
		case 3404:
		case 3405:
		case 3406:
		case 3407:
		case 3408:
		case 3409:
		case 3410:
		case 3411:
		case 3412:
		case 3413:
		case 3414:
		case 3415:
		case 3418:
		case 3419:
		case 3420:
		case 3421:
		case 3422:
		case 3423:
		case 3424:
		case 3425:
		case 3430:
		case 3431:
		case 3432:
		case 3433:
		case 3434:
		case 3435:
		case 3436:
		case 3437:
		case 3438:
		case 3439:
		case 3440:
		case 3441:
		case 3442:
		case 3443:
		case 3444:
		case 3445:
		case 3446:
		case 3447:
		case 3448:
		case 3449:
			break;
		}
	}

	private void DoAction_3450_3499(int action)
	{
		switch (action)
		{
		case 3455:
			scanner.SetGDRState();
			break;
		case 3456:
			scanner.SetGDRState();
			break;
		case 3457:
			scanner.SetGDRState();
			break;
		case 3450:
		case 3451:
		case 3452:
		case 3453:
		case 3454:
		case 3458:
		case 3459:
		case 3460:
		case 3461:
		case 3462:
		case 3463:
		case 3464:
		case 3465:
		case 3466:
		case 3467:
		case 3468:
		case 3469:
		case 3470:
		case 3471:
		case 3472:
		case 3473:
		case 3474:
		case 3475:
		case 3476:
		case 3477:
		case 3478:
		case 3479:
		case 3480:
		case 3481:
		case 3482:
		case 3483:
		case 3484:
		case 3485:
		case 3486:
		case 3487:
		case 3488:
		case 3489:
		case 3490:
		case 3491:
		case 3492:
		case 3493:
		case 3494:
		case 3495:
		case 3496:
		case 3497:
		case 3498:
		case 3499:
			break;
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
		}
	}

	private void DoAction_3600_3649(int action)
	{
		switch (action)
		{
		case 3646:
			yyval.value = false;
			break;
		case 3647:
			yyval.value = true;
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
		case 3611:
		case 3612:
		case 3613:
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
		case 3648:
		case 3649:
			break;
		}
	}

	private void DoAction_3650_3699(int action)
	{
		switch (action)
		{
		case 3657:
			yyval.value = null;
			break;
		case 3658:
			yyval.value = GetText(location_stack.array[location_stack.top - 1]);
			break;
		case 3666:
			yyval.value = SqlAssignmentOperatorType.None;
			break;
		case 3673:
		{
			SqlSetClause sqlSetClause = new SqlSetClause(value_stack.array[value_stack.top - 1].value as List<SqlAssignment>);
			sqlSetClause.Position = yyloc;
			yyval.value = sqlSetClause;
			break;
		}
		case 3674:
		{
			SqlSetClauseError sqlSetClauseError = new SqlSetClauseError(value_stack.array[value_stack.top - 1].value as List<SqlAssignment>);
			sqlSetClauseError.Position = yyloc;
			yyval.value = sqlSetClauseError;
			break;
		}
		case 3678:
		{
			SqlDataTypeSpecification sqlDataTypeSpecification = SqlDataTypeSpecification.Error();
			sqlDataTypeSpecification.Position = yyloc;
			yyval.value = sqlDataTypeSpecification;
			break;
		}
		case 3679:
			scanner.SetParseState(EParseState.e_ParseBegin);
			break;
		case 3680:
		{
			SqlCompoundStatement sqlCompoundStatement3 = new SqlCompoundStatement(value_stack.array[value_stack.top - 2].value as List<SqlStatement>);
			sqlCompoundStatement3.Position = yyloc;
			yyval.value = sqlCompoundStatement3;
			break;
		}
		case 3681:
		{
			SqlStatement sqlStatement2 = new SqlStatementError();
			sqlStatement2.Position = location_stack.array[location_stack.top - 1];
			SqlCompoundStatement sqlCompoundStatement = new SqlCompoundStatement(new List<SqlStatement> { sqlStatement2 });
			sqlCompoundStatement.Position = yyloc;
			yyval.value = sqlCompoundStatement;
			VersionedParserBase.SyntaxError("end", location_stack.array[location_stack.top - 1]);
			break;
		}
		case 3682:
		{
			SqlCompoundStatement sqlCompoundStatement2 = new SqlCompoundStatement(value_stack.array[value_stack.top - 2].value as List<SqlStatement>);
			sqlCompoundStatement2.Position = yyloc;
			yyval.value = sqlCompoundStatement2;
			break;
		}
		case 3683:
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
		case 3684:
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
		case 3685:
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
		case 3686:
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
		case 3687:
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
		case 3688:
		{
			SqlStatement sqlStatement12 = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement12 == null)
			{
				sqlStatement12 = new SqlNullStatement();
			}
			sqlStatement12.Position = yyloc;
			yyval.value = sqlStatement12;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement12);
			break;
		}
		case 3689:
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
		case 3690:
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
		case 3691:
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
		case 3692:
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
		case 3693:
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
		case 3695:
			if (GetStringToken(location_stack.array[location_stack.top - 3]) == StringToken.MaxDop)
			{
				VersionedParserBase.ReportError(SR.InvalidUsageOfScopedConfiguration(GetText(location_stack.array[location_stack.top - 3])), location_stack.array[location_stack.top - 1]);
			}
			break;
		case 3696:
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
		case 3697:
		{
			StringToken stringToken = GetStringToken(location_stack.array[location_stack.top - 3]);
			if (stringToken == StringToken.ParameterSniffing || stringToken == StringToken.QueryOptimizerHotfixes || stringToken == StringToken.LegacyCardinalityEstimation)
			{
				VersionedParserBase.ReportError(SR.InvalidUsageOfScopedConfiguration(GetText(location_stack.array[location_stack.top - 3])), location_stack.array[location_stack.top - 1]);
			}
			break;
		}
		case 3650:
		case 3651:
		case 3652:
		case 3653:
		case 3654:
		case 3655:
		case 3656:
		case 3659:
		case 3660:
		case 3661:
		case 3662:
		case 3663:
		case 3664:
		case 3665:
		case 3667:
		case 3668:
		case 3669:
		case 3670:
		case 3671:
		case 3672:
		case 3675:
		case 3676:
		case 3677:
		case 3694:
		case 3698:
		case 3699:
			break;
		}
	}

	private void DoAction_3700_3716(int action)
	{
		switch (action)
		{
		case 3700:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			scanner.SetQueryStatus(QueryStatus.Q_CRDB);
			break;
		case 3701:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			scanner.SetQueryStatus(QueryStatus.Q_ALTDB);
			break;
		case 3702:
			scanner.SetQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 3703:
			scanner.SetQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 3704:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3705:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3706:
			scanner.SetGDRState();
			break;
		case 3707:
			scanner.SetParseState(EParseState.e_ParseAfterWith);
			break;
		case 3708:
			scanner.SetQueryStatus(QueryStatus.Q_MERGE);
			break;
		case 3709:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3710:
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
		case 3711:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3712:
			scanner.SetQueryState(QueryState.IN_FROM_CLAUSE);
			break;
		case 3713:
			scanner.SetQueryState(QueryState.IN_WHERE_CLAUSE);
			break;
		case 3714:
			scanner.SetParseState(EParseState.e_ParseGroupBy);
			break;
		case 3715:
			scanner.SetParseState(EParseState.e_ParseNeutral);
			break;
		case 3716:
			scanner.SetQueryStatus(QueryStatus.Q_FN_PARSE);
			break;
		}
	}
}
