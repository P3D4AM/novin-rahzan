using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.InternalsAzure;

internal class StateOnlyParser : Babel.ParserGenerator.StateOnlyParser
{
	internal StateOnlyParser(SimpleParserScanner baseScanner)
		: base(baseScanner.CreateTemporaryParserScanner())
	{
	}

	public override Babel.ParserGenerator.StateOnlyParser Create()
	{
		return new StateOnlyParser(scanner);
	}

	public override void DoAction(int action)
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
			DoAction_3900_3917(action);
			break;
		}
	}

	private void DoAction_0_49(int action)
	{
		switch (action)
		{
		case 34:
			scanner.ResetAllParseStates();
			break;
		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
		case 8:
		case 9:
		case 10:
		case 11:
		case 12:
		case 13:
		case 14:
		case 15:
		case 16:
		case 17:
		case 18:
		case 19:
		case 20:
		case 21:
		case 22:
		case 23:
		case 24:
		case 25:
		case 26:
		case 27:
		case 28:
		case 29:
		case 30:
		case 31:
		case 32:
		case 33:
		case 35:
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
		case 253:
		case 254:
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
		case 328:
			scanner.SetQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 331:
			scanner.ClearQueryStatus(QueryStatus.Q_ALTDB);
			break;
		case 332:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 333:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 335:
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
		case 319:
		case 320:
		case 321:
		case 322:
		case 323:
		case 324:
		case 325:
		case 326:
		case 327:
		case 329:
		case 330:
		case 334:
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
		}
	}

	private void DoAction_550_599(int action)
	{
		switch (action)
		{
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
		}
	}

	private void DoAction_700_749(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_750_799(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_800_849(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_850_899(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_900_949(int action)
	{
		switch (action)
		{
		case 922:
			scanner.SetParseState(EParseState.e_ParseIdxInclude);
			break;
		case 900:
		case 901:
		case 902:
		case 903:
		case 904:
		case 905:
		case 906:
		case 907:
		case 908:
		case 909:
		case 910:
		case 911:
		case 912:
		case 913:
		case 914:
		case 915:
		case 916:
		case 917:
		case 918:
		case 919:
		case 920:
		case 921:
		case 923:
		case 924:
		case 925:
		case 926:
		case 927:
		case 928:
		case 929:
		case 930:
		case 931:
		case 932:
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
		case 947:
		case 948:
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
		}
	}

	private void DoAction_1150_1199(int action)
	{
		switch (action)
		{
		case 1171:
			scanner.SetParseState(EParseState.e_ParseLoad);
			break;
		case 1172:
			scanner.SetParseState(EParseState.e_ParseLoad);
			break;
		case 1174:
			scanner.SetParseState(EParseState.e_ParseOpen);
			break;
		case 1175:
			scanner.SetParseState(EParseState.e_ParseClose);
			break;
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
		case 1160:
		case 1161:
		case 1162:
		case 1163:
		case 1164:
		case 1165:
		case 1166:
		case 1167:
		case 1168:
		case 1169:
		case 1170:
		case 1173:
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
		case 1186:
		case 1187:
		case 1188:
		case 1189:
		case 1190:
		case 1191:
		case 1192:
		case 1193:
		case 1194:
		case 1195:
		case 1196:
		case 1197:
		case 1198:
		case 1199:
			break;
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
		case 1381:
			scanner.SetParseState(EParseState.e_ParseDump);
			break;
		case 1382:
			scanner.SetParseState(EParseState.e_ParseDump);
			break;
		case 1396:
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
		case 1371:
		case 1372:
		case 1373:
		case 1374:
		case 1375:
		case 1376:
		case 1377:
		case 1378:
		case 1379:
		case 1380:
		case 1383:
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
		case 1439:
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
		case 1429:
		case 1430:
		case 1431:
		case 1432:
		case 1433:
		case 1434:
		case 1435:
		case 1436:
		case 1437:
		case 1438:
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
		case 1492:
			scanner.SetStmtDelimiter(value: true);
			break;
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
		case 1475:
		case 1476:
		case 1477:
		case 1478:
		case 1479:
		case 1480:
		case 1481:
		case 1482:
		case 1483:
		case 1484:
		case 1485:
		case 1486:
		case 1487:
		case 1488:
		case 1489:
		case 1490:
		case 1491:
		case 1493:
		case 1494:
		case 1495:
		case 1496:
		case 1497:
		case 1498:
		case 1499:
			break;
		}
	}

	private void DoAction_1500_1549(int action)
	{
		switch (action)
		{
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
		case 1641:
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
		case 1631:
		case 1632:
		case 1633:
		case 1634:
		case 1635:
		case 1636:
		case 1637:
		case 1638:
		case 1639:
		case 1640:
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
		}
	}

	private void DoAction_1700_1749(int action)
	{
		switch (action)
		{
		case 1747:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 1700:
		case 1701:
		case 1702:
		case 1703:
		case 1704:
		case 1705:
		case 1706:
		case 1707:
		case 1708:
		case 1709:
		case 1710:
		case 1711:
		case 1712:
		case 1713:
		case 1714:
		case 1715:
		case 1716:
		case 1717:
		case 1718:
		case 1719:
		case 1720:
		case 1721:
		case 1722:
		case 1723:
		case 1724:
		case 1725:
		case 1726:
		case 1727:
		case 1728:
		case 1729:
		case 1730:
		case 1731:
		case 1732:
		case 1733:
		case 1734:
		case 1735:
		case 1736:
		case 1737:
		case 1738:
		case 1739:
		case 1740:
		case 1741:
		case 1742:
		case 1743:
		case 1744:
		case 1745:
		case 1746:
		case 1748:
		case 1749:
			break;
		}
	}

	private void DoAction_1750_1799(int action)
	{
		switch (action)
		{
		case 1788:
			scanner.SetQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 1791:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 1792:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 1750:
		case 1751:
		case 1752:
		case 1753:
		case 1754:
		case 1755:
		case 1756:
		case 1757:
		case 1758:
		case 1759:
		case 1760:
		case 1761:
		case 1762:
		case 1763:
		case 1764:
		case 1765:
		case 1766:
		case 1767:
		case 1768:
		case 1769:
		case 1770:
		case 1771:
		case 1772:
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
		case 1789:
		case 1790:
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
		}
	}

	private void DoAction_1850_1899(int action)
	{
		switch (action)
		{
		case 1855:
			scanner.SetParseState(EParseState.e_ParseWith);
			break;
		case 1869:
			scanner.SetParseState(EParseState.e_ParseAfterWith);
			break;
		case 1850:
		case 1851:
		case 1852:
		case 1853:
		case 1854:
		case 1856:
		case 1857:
		case 1858:
		case 1859:
		case 1860:
		case 1861:
		case 1862:
		case 1863:
		case 1864:
		case 1865:
		case 1866:
		case 1867:
		case 1868:
		case 1870:
		case 1871:
		case 1872:
		case 1873:
		case 1874:
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
		case 1943:
			scanner.ClearQueryStatus(QueryStatus.Q_MERGE);
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
		case 1926:
		case 1927:
		case 1928:
		case 1929:
		case 1930:
		case 1931:
		case 1932:
		case 1933:
		case 1934:
		case 1935:
		case 1936:
		case 1937:
		case 1938:
		case 1939:
		case 1940:
		case 1941:
		case 1942:
		case 1944:
		case 1945:
		case 1946:
		case 1947:
		case 1948:
		case 1949:
			break;
		}
	}

	private void DoAction_1950_1999(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2000_2049(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2050_2099(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2100_2149(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2150_2199(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2200_2249(int action)
	{
		switch (action)
		{
		case 2202:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 2200:
		case 2201:
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
		case 2237:
		case 2238:
		case 2239:
		case 2240:
		case 2241:
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
		case 2273:
			scanner.SetGDRState();
			break;
		case 2274:
			scanner.SetGDRState();
			break;
		case 2275:
			scanner.SetGDRState();
			break;
		case 2277:
			scanner.SetGDRState();
			break;
		case 2278:
			scanner.SetGDRState();
			break;
		case 2293:
			scanner.SetGDRState();
			break;
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
		case 2269:
		case 2270:
		case 2271:
		case 2272:
		case 2276:
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
		}
	}

	private void DoAction_2350_2399(int action)
	{
		switch (action)
		{
		case 2387:
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
		}
	}

	private void DoAction_2450_2499(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2500_2549(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2550_2599(int action)
	{
		switch (action)
		{
		case 2589:
			scanner.SetQueryState(QueryState.None);
			break;
		case 2550:
		case 2551:
		case 2552:
		case 2553:
		case 2554:
		case 2555:
		case 2556:
		case 2557:
		case 2558:
		case 2559:
		case 2560:
		case 2561:
		case 2562:
		case 2563:
		case 2564:
		case 2565:
		case 2566:
		case 2567:
		case 2568:
		case 2569:
		case 2570:
		case 2571:
		case 2572:
		case 2573:
		case 2574:
		case 2575:
		case 2576:
		case 2577:
		case 2578:
		case 2579:
		case 2580:
		case 2581:
		case 2582:
		case 2583:
		case 2584:
		case 2585:
		case 2586:
		case 2587:
		case 2588:
		case 2590:
		case 2591:
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
		}
	}

	private void DoAction_2650_2699(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2700_2749(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2750_2799(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2800_2849(int action)
	{
		switch (action)
		{
		case 2847:
			scanner.SetQueryState(QueryState.None);
			break;
		case 2800:
		case 2801:
		case 2802:
		case 2803:
		case 2804:
		case 2805:
		case 2806:
		case 2807:
		case 2808:
		case 2809:
		case 2810:
		case 2811:
		case 2812:
		case 2813:
		case 2814:
		case 2815:
		case 2816:
		case 2817:
		case 2818:
		case 2819:
		case 2820:
		case 2821:
		case 2822:
		case 2823:
		case 2824:
		case 2825:
		case 2826:
		case 2827:
		case 2828:
		case 2829:
		case 2830:
		case 2831:
		case 2832:
		case 2833:
		case 2834:
		case 2835:
		case 2836:
		case 2837:
		case 2838:
		case 2839:
		case 2840:
		case 2841:
		case 2842:
		case 2843:
		case 2844:
		case 2845:
		case 2846:
		case 2848:
		case 2849:
			break;
		}
	}

	private void DoAction_2850_2899(int action)
	{
		switch (action)
		{
		case 2851:
			scanner.SetParseState(EParseState.e_ParseNeutral);
			break;
		case 2853:
			scanner.SetQueryStatus(QueryStatus.Q_BYALL);
			break;
		case 2850:
		case 2852:
		case 2854:
		case 2855:
		case 2856:
		case 2857:
		case 2858:
		case 2859:
		case 2860:
		case 2861:
		case 2862:
		case 2863:
		case 2864:
		case 2865:
		case 2866:
		case 2867:
		case 2868:
		case 2869:
		case 2870:
		case 2871:
		case 2872:
		case 2873:
		case 2874:
		case 2875:
		case 2876:
		case 2877:
		case 2878:
		case 2879:
		case 2880:
		case 2881:
		case 2882:
		case 2883:
		case 2884:
		case 2885:
		case 2886:
		case 2887:
		case 2888:
		case 2889:
		case 2890:
		case 2891:
		case 2892:
		case 2893:
		case 2894:
		case 2895:
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
		}
	}

	private void DoAction_2950_2999(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_3000_3049(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_3050_3099(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_3100_3149(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_3150_3199(int action)
	{
		switch (action)
		{
		case 3159:
			scanner.ClearQueryStatus(QueryStatus.Q_FN_PARSE);
			break;
		case 3150:
		case 3151:
		case 3152:
		case 3153:
		case 3154:
		case 3155:
		case 3156:
		case 3157:
		case 3158:
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
		case 3181:
		case 3182:
		case 3183:
		case 3184:
		case 3185:
		case 3186:
		case 3187:
		case 3188:
		case 3189:
		case 3190:
		case 3191:
		case 3192:
		case 3193:
		case 3194:
		case 3195:
		case 3196:
		case 3197:
		case 3198:
		case 3199:
			break;
		}
	}

	private void DoAction_3200_3249(int action)
	{
		switch (action)
		{
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
		case 3372:
			scanner.SetQueryStatus(QueryStatus.P_BREAKDECPOINT);
			break;
		case 3373:
			scanner.ClearQueryStatus(QueryStatus.P_BREAKDECPOINT);
			break;
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
		case 3362:
		case 3363:
		case 3364:
		case 3365:
		case 3366:
		case 3367:
		case 3368:
		case 3369:
		case 3370:
		case 3371:
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
		case 3389:
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
		case 3582:
			scanner.SetQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3583:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3592:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3593:
			scanner.SetQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3594:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3595:
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
		case 3572:
		case 3573:
		case 3574:
		case 3575:
		case 3576:
		case 3577:
		case 3578:
		case 3579:
		case 3580:
		case 3581:
		case 3584:
		case 3585:
		case 3586:
		case 3587:
		case 3588:
		case 3589:
		case 3590:
		case 3591:
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
		case 3621:
			scanner.SetGDRState();
			break;
		case 3622:
			scanner.SetGDRState();
			break;
		case 3623:
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
		}
	}

	private void DoAction_3850_3899(int action)
	{
		switch (action)
		{
		case 3884:
			scanner.SetParseState(EParseState.e_ParseBegin);
			break;
		case 3850:
		case 3851:
		case 3852:
		case 3853:
		case 3854:
		case 3855:
		case 3856:
		case 3857:
		case 3858:
		case 3859:
		case 3860:
		case 3861:
		case 3862:
		case 3863:
		case 3864:
		case 3865:
		case 3866:
		case 3867:
		case 3868:
		case 3869:
		case 3870:
		case 3871:
		case 3872:
		case 3873:
		case 3874:
		case 3875:
		case 3876:
		case 3877:
		case 3878:
		case 3879:
		case 3880:
		case 3881:
		case 3882:
		case 3883:
		case 3885:
		case 3886:
		case 3887:
		case 3888:
		case 3889:
		case 3890:
		case 3891:
		case 3892:
		case 3893:
		case 3894:
		case 3895:
		case 3896:
		case 3897:
		case 3898:
		case 3899:
			break;
		}
	}

	private void DoAction_3900_3917(int action)
	{
		switch (action)
		{
		case 3901:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			scanner.SetQueryStatus(QueryStatus.Q_CRDB);
			break;
		case 3902:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			scanner.SetQueryStatus(QueryStatus.Q_ALTDB);
			break;
		case 3903:
			scanner.SetQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 3904:
			scanner.SetQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 3905:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3906:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3907:
			scanner.SetGDRState();
			break;
		case 3908:
			scanner.SetParseState(EParseState.e_ParseAfterWith);
			break;
		case 3909:
			scanner.SetQueryStatus(QueryStatus.Q_MERGE);
			break;
		case 3910:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3911:
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
		case 3912:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3913:
			scanner.SetQueryState(QueryState.IN_FROM_CLAUSE);
			break;
		case 3914:
			scanner.SetQueryState(QueryState.IN_WHERE_CLAUSE);
			break;
		case 3915:
			scanner.SetParseState(EParseState.e_ParseGroupBy);
			break;
		case 3916:
			scanner.SetParseState(EParseState.e_ParseNeutral);
			break;
		case 3917:
			scanner.SetQueryStatus(QueryStatus.Q_FN_PARSE);
			break;
		case 3900:
			break;
		}
	}
}
