﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Intranet.DataAccess.Data.Migrations.SOA
{
    public partial class AddDbsToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_T001s",
                table: "T001s");

            migrationBuilder.RenameTable(
                name: "T001s",
                newName: "T001");

            migrationBuilder.AddPrimaryKey(
                name: "PK_T001",
                table: "T001",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BKPFs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BUKRS = table.Column<string>(nullable: true),
                    BELNR = table.Column<string>(nullable: true),
                    GJAHR = table.Column<string>(nullable: true),
                    BLART = table.Column<string>(nullable: true),
                    BLDAT = table.Column<string>(nullable: true),
                    BUDAT = table.Column<string>(nullable: true),
                    MONAT = table.Column<string>(nullable: true),
                    CPUDT = table.Column<string>(nullable: true),
                    CPUTM = table.Column<string>(nullable: true),
                    AEDAT = table.Column<string>(nullable: true),
                    UPDDT = table.Column<string>(nullable: true),
                    WWERT = table.Column<string>(nullable: true),
                    USNAM = table.Column<string>(nullable: true),
                    TCODE = table.Column<string>(nullable: true),
                    BVORG = table.Column<string>(nullable: true),
                    XBLNR = table.Column<string>(nullable: true),
                    DBBLG = table.Column<string>(nullable: true),
                    STBLG = table.Column<string>(nullable: true),
                    STJAH = table.Column<string>(nullable: true),
                    BKTXT = table.Column<string>(nullable: true),
                    WAERS = table.Column<string>(nullable: true),
                    KURSF = table.Column<string>(nullable: true),
                    KZWRS = table.Column<string>(nullable: true),
                    KZKRS = table.Column<string>(nullable: true),
                    BSTAT = table.Column<string>(nullable: true),
                    XNETB = table.Column<string>(nullable: true),
                    FRATH = table.Column<string>(nullable: true),
                    XRUEB = table.Column<string>(nullable: true),
                    GLVOR = table.Column<string>(nullable: true),
                    GRPID = table.Column<string>(nullable: true),
                    DOKID = table.Column<string>(nullable: true),
                    ARCID = table.Column<string>(nullable: true),
                    IBLAR = table.Column<string>(nullable: true),
                    AWTYP = table.Column<string>(nullable: true),
                    AWKEY = table.Column<string>(nullable: true),
                    FIKRS = table.Column<string>(nullable: true),
                    HWAER = table.Column<string>(nullable: true),
                    HWAE2 = table.Column<string>(nullable: true),
                    HWAE3 = table.Column<string>(nullable: true),
                    KURS2 = table.Column<string>(nullable: true),
                    KURS3 = table.Column<string>(nullable: true),
                    BASW2 = table.Column<string>(nullable: true),
                    BASW3 = table.Column<string>(nullable: true),
                    UMRD2 = table.Column<string>(nullable: true),
                    UMRD3 = table.Column<string>(nullable: true),
                    XSTOV = table.Column<string>(nullable: true),
                    STODT = table.Column<string>(nullable: true),
                    XMWST = table.Column<string>(nullable: true),
                    CURT2 = table.Column<string>(nullable: true),
                    CURT3 = table.Column<string>(nullable: true),
                    KUTY2 = table.Column<string>(nullable: true),
                    KUTY3 = table.Column<string>(nullable: true),
                    XSNET = table.Column<string>(nullable: true),
                    AUSBK = table.Column<string>(nullable: true),
                    XUSVR = table.Column<string>(nullable: true),
                    DUEFL = table.Column<string>(nullable: true),
                    AWSYS = table.Column<string>(nullable: true),
                    TXKRS = table.Column<string>(nullable: true),
                    CTXKRS = table.Column<string>(nullable: true),
                    LOTKZ = table.Column<string>(nullable: true),
                    XWVOF = table.Column<string>(nullable: true),
                    STGRD = table.Column<string>(nullable: true),
                    PPNAM = table.Column<string>(nullable: true),
                    PPDAT = table.Column<string>(nullable: true),
                    PPTME = table.Column<string>(nullable: true),
                    PPTCOD = table.Column<string>(nullable: true),
                    BRNCH = table.Column<string>(nullable: true),
                    NUMPG = table.Column<string>(nullable: true),
                    ADISC = table.Column<string>(nullable: true),
                    XREF1_HD = table.Column<string>(nullable: true),
                    XREF2_HD = table.Column<string>(nullable: true),
                    XREVERSAL = table.Column<string>(nullable: true),
                    REINDAT = table.Column<string>(nullable: true),
                    RLDNR = table.Column<string>(nullable: true),
                    LDGRP = table.Column<string>(nullable: true),
                    PROPMANO = table.Column<string>(nullable: true),
                    XBLNR_ALT = table.Column<string>(nullable: true),
                    VATDATE = table.Column<string>(nullable: true),
                    DOCCAT = table.Column<string>(nullable: true),
                    XSPLIT = table.Column<string>(nullable: true),
                    CASH_ALLOC = table.Column<string>(nullable: true),
                    FOLLOW_ON = table.Column<string>(nullable: true),
                    XREORG = table.Column<string>(nullable: true),
                    SUBSET = table.Column<string>(nullable: true),
                    KURST = table.Column<string>(nullable: true),
                    KURSX = table.Column<string>(nullable: true),
                    KUR2X = table.Column<string>(nullable: true),
                    KUR3X = table.Column<string>(nullable: true),
                    XMCA = table.Column<string>(nullable: true),
                    RESUBMISSION = table.Column<string>(nullable: true),
                    SAPF15_STATUS = table.Column<string>(nullable: true),
                    PSOTY = table.Column<string>(nullable: true),
                    PSOAK = table.Column<string>(nullable: true),
                    PSOKS = table.Column<string>(nullable: true),
                    PSOSG = table.Column<string>(nullable: true),
                    PSOFN = table.Column<string>(nullable: true),
                    INTFORM = table.Column<string>(nullable: true),
                    INTDATE = table.Column<string>(nullable: true),
                    PSOBT = table.Column<string>(nullable: true),
                    PSOZL = table.Column<string>(nullable: true),
                    PSODT = table.Column<string>(nullable: true),
                    PSOTM = table.Column<string>(nullable: true),
                    FM_UMART = table.Column<string>(nullable: true),
                    CCINS = table.Column<string>(nullable: true),
                    CCNUM = table.Column<string>(nullable: true),
                    SSBLK = table.Column<string>(nullable: true),
                    BATCH = table.Column<string>(nullable: true),
                    SNAME = table.Column<string>(nullable: true),
                    SAMPLED = table.Column<string>(nullable: true),
                    EXCLUDE_FLAG = table.Column<string>(nullable: true),
                    BLIND = table.Column<string>(nullable: true),
                    OFFSET_STATUS = table.Column<string>(nullable: true),
                    OFFSET_REFER_DAT = table.Column<string>(nullable: true),
                    PENRC = table.Column<string>(nullable: true),
                    KNUMV = table.Column<string>(nullable: true),
                    OINETNUM = table.Column<string>(nullable: true),
                    OINJAHR = table.Column<string>(nullable: true),
                    OININD = table.Column<string>(nullable: true),
                    RECHN = table.Column<string>(nullable: true),
                    PYBASTYP = table.Column<string>(nullable: true),
                    PYBASNO = table.Column<string>(nullable: true),
                    PYBASDAT = table.Column<string>(nullable: true),
                    PYIBAN = table.Column<string>(nullable: true),
                    INWARDNO_HD = table.Column<string>(nullable: true),
                    INWARDDT_HD = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BKPFs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BSADs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BUKRS = table.Column<string>(nullable: true),
                    KUNNR = table.Column<string>(nullable: true),
                    UMSKS = table.Column<string>(nullable: true),
                    UMSKZ = table.Column<string>(nullable: true),
                    AUGDT = table.Column<string>(nullable: true),
                    AUGBL = table.Column<string>(nullable: true),
                    ZUONR = table.Column<string>(nullable: true),
                    GJAHR = table.Column<string>(nullable: true),
                    BELNR = table.Column<string>(nullable: true),
                    BUZEI = table.Column<string>(nullable: true),
                    BUDAT = table.Column<string>(nullable: true),
                    BLDAT = table.Column<string>(nullable: true),
                    CPUDT = table.Column<string>(nullable: true),
                    WAERS = table.Column<string>(nullable: true),
                    XBLNR = table.Column<string>(nullable: true),
                    BLART = table.Column<string>(nullable: true),
                    MONAT = table.Column<string>(nullable: true),
                    BSCHL = table.Column<string>(nullable: true),
                    ZUMSK = table.Column<string>(nullable: true),
                    SHKZG = table.Column<string>(nullable: true),
                    GSBER = table.Column<string>(nullable: true),
                    MWSKZ = table.Column<string>(nullable: true),
                    DMBTR = table.Column<string>(nullable: true),
                    WRBTR = table.Column<string>(nullable: true),
                    MWSTS = table.Column<string>(nullable: true),
                    WMWST = table.Column<string>(nullable: true),
                    BDIFF = table.Column<string>(nullable: true),
                    BDIF2 = table.Column<string>(nullable: true),
                    SGTXT = table.Column<string>(nullable: true),
                    PROJN = table.Column<string>(nullable: true),
                    AUFNR = table.Column<string>(nullable: true),
                    ANLN1 = table.Column<string>(nullable: true),
                    ANLN2 = table.Column<string>(nullable: true),
                    SAKNR = table.Column<string>(nullable: true),
                    HKONT = table.Column<string>(nullable: true),
                    FKONT = table.Column<string>(nullable: true),
                    FILKD = table.Column<string>(nullable: true),
                    ZFBDT = table.Column<string>(nullable: true),
                    ZTERM = table.Column<string>(nullable: true),
                    ZBD1T = table.Column<string>(nullable: true),
                    ZBD2T = table.Column<string>(nullable: true),
                    ZBD3T = table.Column<string>(nullable: true),
                    ZBD1P = table.Column<string>(nullable: true),
                    ZBD2P = table.Column<string>(nullable: true),
                    SKFBT = table.Column<string>(nullable: true),
                    SKNTO = table.Column<string>(nullable: true),
                    WSKTO = table.Column<string>(nullable: true),
                    ZLSCH = table.Column<string>(nullable: true),
                    ZLSPR = table.Column<string>(nullable: true),
                    ZBFIX = table.Column<string>(nullable: true),
                    HBKID = table.Column<string>(nullable: true),
                    BVTYP = table.Column<string>(nullable: true),
                    REBZG = table.Column<string>(nullable: true),
                    REBZJ = table.Column<string>(nullable: true),
                    REBZZ = table.Column<string>(nullable: true),
                    SAMNR = table.Column<string>(nullable: true),
                    ANFBN = table.Column<string>(nullable: true),
                    ANFBJ = table.Column<string>(nullable: true),
                    ANFBU = table.Column<string>(nullable: true),
                    ANFAE = table.Column<string>(nullable: true),
                    MANSP = table.Column<string>(nullable: true),
                    MSCHL = table.Column<string>(nullable: true),
                    MADAT = table.Column<string>(nullable: true),
                    MANST = table.Column<string>(nullable: true),
                    MABER = table.Column<string>(nullable: true),
                    XNETB = table.Column<string>(nullable: true),
                    XANET = table.Column<string>(nullable: true),
                    XCPDD = table.Column<string>(nullable: true),
                    XINVE = table.Column<string>(nullable: true),
                    XZAHL = table.Column<string>(nullable: true),
                    MWSK1 = table.Column<string>(nullable: true),
                    DMBT1 = table.Column<string>(nullable: true),
                    WRBT1 = table.Column<string>(nullable: true),
                    MWSK2 = table.Column<string>(nullable: true),
                    DMBT2 = table.Column<string>(nullable: true),
                    WRBT2 = table.Column<string>(nullable: true),
                    MWSK3 = table.Column<string>(nullable: true),
                    DMBT3 = table.Column<string>(nullable: true),
                    WRBT3 = table.Column<string>(nullable: true),
                    BSTAT = table.Column<string>(nullable: true),
                    VBUND = table.Column<string>(nullable: true),
                    VBELN = table.Column<string>(nullable: true),
                    REBZT = table.Column<string>(nullable: true),
                    INFAE = table.Column<string>(nullable: true),
                    STCEG = table.Column<string>(nullable: true),
                    EGBLD = table.Column<string>(nullable: true),
                    EGLLD = table.Column<string>(nullable: true),
                    RSTGR = table.Column<string>(nullable: true),
                    XNOZA = table.Column<string>(nullable: true),
                    VERTT = table.Column<string>(nullable: true),
                    VERTN = table.Column<string>(nullable: true),
                    VBEWA = table.Column<string>(nullable: true),
                    WVERW = table.Column<string>(nullable: true),
                    PROJK = table.Column<string>(nullable: true),
                    FIPOS = table.Column<string>(nullable: true),
                    NPLNR = table.Column<string>(nullable: true),
                    AUFPL = table.Column<string>(nullable: true),
                    APLZL = table.Column<string>(nullable: true),
                    XEGDR = table.Column<string>(nullable: true),
                    DMBE2 = table.Column<string>(nullable: true),
                    DMBE3 = table.Column<string>(nullable: true),
                    DMB21 = table.Column<string>(nullable: true),
                    DMB22 = table.Column<string>(nullable: true),
                    DMB23 = table.Column<string>(nullable: true),
                    DMB31 = table.Column<string>(nullable: true),
                    DMB32 = table.Column<string>(nullable: true),
                    DMB33 = table.Column<string>(nullable: true),
                    BDIF3 = table.Column<string>(nullable: true),
                    XRAGL = table.Column<string>(nullable: true),
                    UZAWE = table.Column<string>(nullable: true),
                    XSTOV = table.Column<string>(nullable: true),
                    MWST2 = table.Column<string>(nullable: true),
                    MWST3 = table.Column<string>(nullable: true),
                    SKNT2 = table.Column<string>(nullable: true),
                    SKNT3 = table.Column<string>(nullable: true),
                    XREF1 = table.Column<string>(nullable: true),
                    XREF2 = table.Column<string>(nullable: true),
                    XARCH = table.Column<string>(nullable: true),
                    PSWSL = table.Column<string>(nullable: true),
                    PSWBT = table.Column<string>(nullable: true),
                    LZBKZ = table.Column<string>(nullable: true),
                    LANDL = table.Column<string>(nullable: true),
                    IMKEY = table.Column<string>(nullable: true),
                    VBEL2 = table.Column<string>(nullable: true),
                    VPOS2 = table.Column<string>(nullable: true),
                    POSN2 = table.Column<string>(nullable: true),
                    ETEN2 = table.Column<string>(nullable: true),
                    FISTL = table.Column<string>(nullable: true),
                    GEBER = table.Column<string>(nullable: true),
                    DABRZ = table.Column<string>(nullable: true),
                    XNEGP = table.Column<string>(nullable: true),
                    KOSTL = table.Column<string>(nullable: true),
                    RFZEI = table.Column<string>(nullable: true),
                    KKBER = table.Column<string>(nullable: true),
                    EMPFB = table.Column<string>(nullable: true),
                    PRCTR = table.Column<string>(nullable: true),
                    XREF3 = table.Column<string>(nullable: true),
                    QSSKZ = table.Column<string>(nullable: true),
                    ZINKZ = table.Column<string>(nullable: true),
                    DTWS1 = table.Column<string>(nullable: true),
                    DTWS2 = table.Column<string>(nullable: true),
                    DTWS3 = table.Column<string>(nullable: true),
                    DTWS4 = table.Column<string>(nullable: true),
                    XPYPR = table.Column<string>(nullable: true),
                    KIDNO = table.Column<string>(nullable: true),
                    ABSBT = table.Column<string>(nullable: true),
                    CCBTC = table.Column<string>(nullable: true),
                    PYCUR = table.Column<string>(nullable: true),
                    PYAMT = table.Column<string>(nullable: true),
                    BUPLA = table.Column<string>(nullable: true),
                    SECCO = table.Column<string>(nullable: true),
                    CESSION_KZ = table.Column<string>(nullable: true),
                    PPDIFF = table.Column<string>(nullable: true),
                    PPDIF2 = table.Column<string>(nullable: true),
                    PPDIF3 = table.Column<string>(nullable: true),
                    KBLNR = table.Column<string>(nullable: true),
                    KBLPOS = table.Column<string>(nullable: true),
                    GRANT_NBR = table.Column<string>(nullable: true),
                    GMVKZ = table.Column<string>(nullable: true),
                    SRTYPE = table.Column<string>(nullable: true),
                    LOTKZ = table.Column<string>(nullable: true),
                    FKBER = table.Column<string>(nullable: true),
                    INTRENO = table.Column<string>(nullable: true),
                    PPRCT = table.Column<string>(nullable: true),
                    BUZID = table.Column<string>(nullable: true),
                    AUGGJ = table.Column<string>(nullable: true),
                    HKTID = table.Column<string>(nullable: true),
                    BUDGET_PD = table.Column<string>(nullable: true),
                    PAYS_PROV = table.Column<string>(nullable: true),
                    PAYS_TRAN = table.Column<string>(nullable: true),
                    MNDID = table.Column<string>(nullable: true),
                    KONTT = table.Column<string>(nullable: true),
                    KONTL = table.Column<string>(nullable: true),
                    UEBGDAT = table.Column<string>(nullable: true),
                    VNAME = table.Column<string>(nullable: true),
                    EGRUP = table.Column<string>(nullable: true),
                    BTYPE = table.Column<string>(nullable: true),
                    OIEXGNUM = table.Column<string>(nullable: true),
                    OINETCYC = table.Column<string>(nullable: true),
                    OIEXGTYP = table.Column<string>(nullable: true),
                    PROPMANO = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BSADs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BSEGs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BUKRS = table.Column<string>(nullable: true),
                    BELNR = table.Column<string>(nullable: true),
                    GJAHR = table.Column<string>(nullable: true),
                    KOART = table.Column<string>(nullable: true),
                    BUZEI = table.Column<string>(nullable: true),
                    HKONT = table.Column<string>(nullable: true),
                    SGTXT = table.Column<string>(nullable: true),
                    SHKZG = table.Column<string>(nullable: true),
                    DMBTR = table.Column<string>(nullable: true),
                    WRBTR = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BSEGs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BSIDs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BUKRS = table.Column<string>(nullable: true),
                    KUNNR = table.Column<string>(nullable: true),
                    UMSKS = table.Column<string>(nullable: true),
                    UMSKZ = table.Column<string>(nullable: true),
                    AUGDT = table.Column<string>(nullable: true),
                    AUGBL = table.Column<string>(nullable: true),
                    ZUONR = table.Column<string>(nullable: true),
                    GJAHR = table.Column<string>(nullable: true),
                    BELNR = table.Column<string>(nullable: true),
                    BUZEI = table.Column<string>(nullable: true),
                    BUDAT = table.Column<string>(nullable: true),
                    BLDAT = table.Column<string>(nullable: true),
                    CPUDT = table.Column<string>(nullable: true),
                    WAERS = table.Column<string>(nullable: true),
                    XBLNR = table.Column<string>(nullable: true),
                    BLART = table.Column<string>(nullable: true),
                    MONAT = table.Column<string>(nullable: true),
                    BSCHL = table.Column<string>(nullable: true),
                    ZUMSK = table.Column<string>(nullable: true),
                    SHKZG = table.Column<string>(nullable: true),
                    GSBER = table.Column<string>(nullable: true),
                    MWSKZ = table.Column<string>(nullable: true),
                    DMBTR = table.Column<string>(nullable: true),
                    WRBTR = table.Column<string>(nullable: true),
                    MWSTS = table.Column<string>(nullable: true),
                    WMWST = table.Column<string>(nullable: true),
                    BDIFF = table.Column<string>(nullable: true),
                    BDIF2 = table.Column<string>(nullable: true),
                    SGTXT = table.Column<string>(nullable: true),
                    PROJN = table.Column<string>(nullable: true),
                    AUFNR = table.Column<string>(nullable: true),
                    ANLN1 = table.Column<string>(nullable: true),
                    ANLN2 = table.Column<string>(nullable: true),
                    SAKNR = table.Column<string>(nullable: true),
                    HKONT = table.Column<string>(nullable: true),
                    FKONT = table.Column<string>(nullable: true),
                    FILKD = table.Column<string>(nullable: true),
                    ZFBDT = table.Column<string>(nullable: true),
                    ZTERM = table.Column<string>(nullable: true),
                    ZBD1T = table.Column<string>(nullable: true),
                    ZBD2T = table.Column<string>(nullable: true),
                    ZBD3T = table.Column<string>(nullable: true),
                    ZBD1P = table.Column<string>(nullable: true),
                    ZBD2P = table.Column<string>(nullable: true),
                    SKFBT = table.Column<string>(nullable: true),
                    SKNTO = table.Column<string>(nullable: true),
                    WSKTO = table.Column<string>(nullable: true),
                    ZLSCH = table.Column<string>(nullable: true),
                    ZLSPR = table.Column<string>(nullable: true),
                    ZBFIX = table.Column<string>(nullable: true),
                    HBKID = table.Column<string>(nullable: true),
                    BVTYP = table.Column<string>(nullable: true),
                    REBZG = table.Column<string>(nullable: true),
                    REBZJ = table.Column<string>(nullable: true),
                    REBZZ = table.Column<string>(nullable: true),
                    SAMNR = table.Column<string>(nullable: true),
                    ANFBN = table.Column<string>(nullable: true),
                    ANFBJ = table.Column<string>(nullable: true),
                    ANFBU = table.Column<string>(nullable: true),
                    ANFAE = table.Column<string>(nullable: true),
                    MANSP = table.Column<string>(nullable: true),
                    MSCHL = table.Column<string>(nullable: true),
                    MADAT = table.Column<string>(nullable: true),
                    MANST = table.Column<string>(nullable: true),
                    MABER = table.Column<string>(nullable: true),
                    XNETB = table.Column<string>(nullable: true),
                    XANET = table.Column<string>(nullable: true),
                    XCPDD = table.Column<string>(nullable: true),
                    XINVE = table.Column<string>(nullable: true),
                    XZAHL = table.Column<string>(nullable: true),
                    MWSK1 = table.Column<string>(nullable: true),
                    DMBT1 = table.Column<string>(nullable: true),
                    WRBT1 = table.Column<string>(nullable: true),
                    MWSK2 = table.Column<string>(nullable: true),
                    DMBT2 = table.Column<string>(nullable: true),
                    WRBT2 = table.Column<string>(nullable: true),
                    MWSK3 = table.Column<string>(nullable: true),
                    DMBT3 = table.Column<string>(nullable: true),
                    WRBT3 = table.Column<string>(nullable: true),
                    BSTAT = table.Column<string>(nullable: true),
                    VBUND = table.Column<string>(nullable: true),
                    VBELN = table.Column<string>(nullable: true),
                    REBZT = table.Column<string>(nullable: true),
                    INFAE = table.Column<string>(nullable: true),
                    STCEG = table.Column<string>(nullable: true),
                    EGBLD = table.Column<string>(nullable: true),
                    EGLLD = table.Column<string>(nullable: true),
                    RSTGR = table.Column<string>(nullable: true),
                    XNOZA = table.Column<string>(nullable: true),
                    VERTT = table.Column<string>(nullable: true),
                    VERTN = table.Column<string>(nullable: true),
                    VBEWA = table.Column<string>(nullable: true),
                    WVERW = table.Column<string>(nullable: true),
                    PROJK = table.Column<string>(nullable: true),
                    FIPOS = table.Column<string>(nullable: true),
                    NPLNR = table.Column<string>(nullable: true),
                    AUFPL = table.Column<string>(nullable: true),
                    APLZL = table.Column<string>(nullable: true),
                    XEGDR = table.Column<string>(nullable: true),
                    DMBE2 = table.Column<string>(nullable: true),
                    DMBE3 = table.Column<string>(nullable: true),
                    DMB21 = table.Column<string>(nullable: true),
                    DMB22 = table.Column<string>(nullable: true),
                    DMB23 = table.Column<string>(nullable: true),
                    DMB31 = table.Column<string>(nullable: true),
                    DMB32 = table.Column<string>(nullable: true),
                    DMB33 = table.Column<string>(nullable: true),
                    BDIF3 = table.Column<string>(nullable: true),
                    XRAGL = table.Column<string>(nullable: true),
                    UZAWE = table.Column<string>(nullable: true),
                    XSTOV = table.Column<string>(nullable: true),
                    MWST2 = table.Column<string>(nullable: true),
                    MWST3 = table.Column<string>(nullable: true),
                    SKNT2 = table.Column<string>(nullable: true),
                    SKNT3 = table.Column<string>(nullable: true),
                    XREF1 = table.Column<string>(nullable: true),
                    XREF2 = table.Column<string>(nullable: true),
                    XARCH = table.Column<string>(nullable: true),
                    PSWSL = table.Column<string>(nullable: true),
                    PSWBT = table.Column<string>(nullable: true),
                    LZBKZ = table.Column<string>(nullable: true),
                    LANDL = table.Column<string>(nullable: true),
                    IMKEY = table.Column<string>(nullable: true),
                    VBEL2 = table.Column<string>(nullable: true),
                    VPOS2 = table.Column<string>(nullable: true),
                    POSN2 = table.Column<string>(nullable: true),
                    ETEN2 = table.Column<string>(nullable: true),
                    FISTL = table.Column<string>(nullable: true),
                    GEBER = table.Column<string>(nullable: true),
                    DABRZ = table.Column<string>(nullable: true),
                    XNEGP = table.Column<string>(nullable: true),
                    KOSTL = table.Column<string>(nullable: true),
                    RFZEI = table.Column<string>(nullable: true),
                    KKBER = table.Column<string>(nullable: true),
                    EMPFB = table.Column<string>(nullable: true),
                    PRCTR = table.Column<string>(nullable: true),
                    XREF3 = table.Column<string>(nullable: true),
                    QSSKZ = table.Column<string>(nullable: true),
                    ZINKZ = table.Column<string>(nullable: true),
                    DTWS1 = table.Column<string>(nullable: true),
                    DTWS2 = table.Column<string>(nullable: true),
                    DTWS3 = table.Column<string>(nullable: true),
                    DTWS4 = table.Column<string>(nullable: true),
                    XPYPR = table.Column<string>(nullable: true),
                    KIDNO = table.Column<string>(nullable: true),
                    ABSBT = table.Column<string>(nullable: true),
                    CCBTC = table.Column<string>(nullable: true),
                    PYCUR = table.Column<string>(nullable: true),
                    PYAMT = table.Column<string>(nullable: true),
                    BUPLA = table.Column<string>(nullable: true),
                    SECCO = table.Column<string>(nullable: true),
                    CESSION_KZ = table.Column<string>(nullable: true),
                    PPDIFF = table.Column<string>(nullable: true),
                    PPDIF2 = table.Column<string>(nullable: true),
                    PPDIF3 = table.Column<string>(nullable: true),
                    KBLNR = table.Column<string>(nullable: true),
                    KBLPOS = table.Column<string>(nullable: true),
                    GRANT_NBR = table.Column<string>(nullable: true),
                    GMVKZ = table.Column<string>(nullable: true),
                    SRTYPE = table.Column<string>(nullable: true),
                    LOTKZ = table.Column<string>(nullable: true),
                    FKBER = table.Column<string>(nullable: true),
                    INTRENO = table.Column<string>(nullable: true),
                    PPRCT = table.Column<string>(nullable: true),
                    BUZID = table.Column<string>(nullable: true),
                    AUGGJ = table.Column<string>(nullable: true),
                    HKTID = table.Column<string>(nullable: true),
                    BUDGET_PD = table.Column<string>(nullable: true),
                    PAYS_PROV = table.Column<string>(nullable: true),
                    PAYS_TRAN = table.Column<string>(nullable: true),
                    MNDID = table.Column<string>(nullable: true),
                    KONTT = table.Column<string>(nullable: true),
                    KONTL = table.Column<string>(nullable: true),
                    UEBGDAT = table.Column<string>(nullable: true),
                    VNAME = table.Column<string>(nullable: true),
                    EGRUP = table.Column<string>(nullable: true),
                    BTYPE = table.Column<string>(nullable: true),
                    OIEXGNUM = table.Column<string>(nullable: true),
                    OINETCYC = table.Column<string>(nullable: true),
                    OIEXGTYP = table.Column<string>(nullable: true),
                    PROPMANO = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BSIDs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KNA1s",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KUNNR = table.Column<string>(nullable: true),
                    NAME1 = table.Column<string>(nullable: true),
                    STRAS = table.Column<string>(nullable: true),
                    ORT01 = table.Column<string>(nullable: true),
                    PSTLZ = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KNA1s", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KNB1s",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KUNNR = table.Column<string>(nullable: true),
                    BUKRS = table.Column<string>(nullable: true),
                    KVERM = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KNB1s", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KNKKs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KUNNR = table.Column<string>(nullable: true),
                    KKBER = table.Column<string>(nullable: true),
                    KLIMK = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KNKKs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KNVKs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KUNNR = table.Column<string>(nullable: true),
                    NAME1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KNVKs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "R_BKPFs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BUKRS = table.Column<string>(nullable: true),
                    BELNR = table.Column<string>(nullable: true),
                    GJAHR = table.Column<string>(nullable: true),
                    BLART = table.Column<string>(nullable: true),
                    BLDAT = table.Column<string>(nullable: true),
                    BUDAT = table.Column<string>(nullable: true),
                    MONAT = table.Column<string>(nullable: true),
                    CPUDT = table.Column<string>(nullable: true),
                    CPUTM = table.Column<string>(nullable: true),
                    AEDAT = table.Column<string>(nullable: true),
                    UPDDT = table.Column<string>(nullable: true),
                    WWERT = table.Column<string>(nullable: true),
                    USNAM = table.Column<string>(nullable: true),
                    TCODE = table.Column<string>(nullable: true),
                    BVORG = table.Column<string>(nullable: true),
                    XBLNR = table.Column<string>(nullable: true),
                    DBBLG = table.Column<string>(nullable: true),
                    STBLG = table.Column<string>(nullable: true),
                    STJAH = table.Column<string>(nullable: true),
                    BKTXT = table.Column<string>(nullable: true),
                    WAERS = table.Column<string>(nullable: true),
                    KURSF = table.Column<string>(nullable: true),
                    KZWRS = table.Column<string>(nullable: true),
                    KZKRS = table.Column<string>(nullable: true),
                    BSTAT = table.Column<string>(nullable: true),
                    XNETB = table.Column<string>(nullable: true),
                    FRATH = table.Column<string>(nullable: true),
                    XRUEB = table.Column<string>(nullable: true),
                    GLVOR = table.Column<string>(nullable: true),
                    GRPID = table.Column<string>(nullable: true),
                    DOKID = table.Column<string>(nullable: true),
                    ARCID = table.Column<string>(nullable: true),
                    IBLAR = table.Column<string>(nullable: true),
                    AWTYP = table.Column<string>(nullable: true),
                    AWKEY = table.Column<string>(nullable: true),
                    FIKRS = table.Column<string>(nullable: true),
                    HWAER = table.Column<string>(nullable: true),
                    HWAE2 = table.Column<string>(nullable: true),
                    HWAE3 = table.Column<string>(nullable: true),
                    KURS2 = table.Column<string>(nullable: true),
                    KURS3 = table.Column<string>(nullable: true),
                    BASW2 = table.Column<string>(nullable: true),
                    BASW3 = table.Column<string>(nullable: true),
                    UMRD2 = table.Column<string>(nullable: true),
                    UMRD3 = table.Column<string>(nullable: true),
                    XSTOV = table.Column<string>(nullable: true),
                    STODT = table.Column<string>(nullable: true),
                    XMWST = table.Column<string>(nullable: true),
                    CURT2 = table.Column<string>(nullable: true),
                    CURT3 = table.Column<string>(nullable: true),
                    KUTY2 = table.Column<string>(nullable: true),
                    KUTY3 = table.Column<string>(nullable: true),
                    XSNET = table.Column<string>(nullable: true),
                    AUSBK = table.Column<string>(nullable: true),
                    XUSVR = table.Column<string>(nullable: true),
                    DUEFL = table.Column<string>(nullable: true),
                    AWSYS = table.Column<string>(nullable: true),
                    TXKRS = table.Column<string>(nullable: true),
                    CTXKRS = table.Column<string>(nullable: true),
                    LOTKZ = table.Column<string>(nullable: true),
                    XWVOF = table.Column<string>(nullable: true),
                    STGRD = table.Column<string>(nullable: true),
                    PPNAM = table.Column<string>(nullable: true),
                    PPDAT = table.Column<string>(nullable: true),
                    PPTME = table.Column<string>(nullable: true),
                    PPTCOD = table.Column<string>(nullable: true),
                    BRNCH = table.Column<string>(nullable: true),
                    NUMPG = table.Column<string>(nullable: true),
                    ADISC = table.Column<string>(nullable: true),
                    XREF1_HD = table.Column<string>(nullable: true),
                    XREF2_HD = table.Column<string>(nullable: true),
                    XREVERSAL = table.Column<string>(nullable: true),
                    REINDAT = table.Column<string>(nullable: true),
                    RLDNR = table.Column<string>(nullable: true),
                    LDGRP = table.Column<string>(nullable: true),
                    PROPMANO = table.Column<string>(nullable: true),
                    XBLNR_ALT = table.Column<string>(nullable: true),
                    VATDATE = table.Column<string>(nullable: true),
                    DOCCAT = table.Column<string>(nullable: true),
                    XSPLIT = table.Column<string>(nullable: true),
                    CASH_ALLOC = table.Column<string>(nullable: true),
                    FOLLOW_ON = table.Column<string>(nullable: true),
                    XREORG = table.Column<string>(nullable: true),
                    SUBSET = table.Column<string>(nullable: true),
                    KURST = table.Column<string>(nullable: true),
                    KURSX = table.Column<string>(nullable: true),
                    KUR2X = table.Column<string>(nullable: true),
                    KUR3X = table.Column<string>(nullable: true),
                    XMCA = table.Column<string>(nullable: true),
                    RESUBMISSION = table.Column<string>(nullable: true),
                    SAPF15_STATUS = table.Column<string>(nullable: true),
                    PSOTY = table.Column<string>(nullable: true),
                    PSOAK = table.Column<string>(nullable: true),
                    PSOKS = table.Column<string>(nullable: true),
                    PSOSG = table.Column<string>(nullable: true),
                    PSOFN = table.Column<string>(nullable: true),
                    INTFORM = table.Column<string>(nullable: true),
                    INTDATE = table.Column<string>(nullable: true),
                    PSOBT = table.Column<string>(nullable: true),
                    PSOZL = table.Column<string>(nullable: true),
                    PSODT = table.Column<string>(nullable: true),
                    PSOTM = table.Column<string>(nullable: true),
                    FM_UMART = table.Column<string>(nullable: true),
                    CCINS = table.Column<string>(nullable: true),
                    CCNUM = table.Column<string>(nullable: true),
                    SSBLK = table.Column<string>(nullable: true),
                    BATCH = table.Column<string>(nullable: true),
                    SNAME = table.Column<string>(nullable: true),
                    SAMPLED = table.Column<string>(nullable: true),
                    EXCLUDE_FLAG = table.Column<string>(nullable: true),
                    BLIND = table.Column<string>(nullable: true),
                    OFFSET_STATUS = table.Column<string>(nullable: true),
                    OFFSET_REFER_DAT = table.Column<string>(nullable: true),
                    PENRC = table.Column<string>(nullable: true),
                    KNUMV = table.Column<string>(nullable: true),
                    OINETNUM = table.Column<string>(nullable: true),
                    OINJAHR = table.Column<string>(nullable: true),
                    OININD = table.Column<string>(nullable: true),
                    RECHN = table.Column<string>(nullable: true),
                    PYBASTYP = table.Column<string>(nullable: true),
                    PYBASNO = table.Column<string>(nullable: true),
                    PYBASDAT = table.Column<string>(nullable: true),
                    PYIBAN = table.Column<string>(nullable: true),
                    INWARDNO_HD = table.Column<string>(nullable: true),
                    INWARDDT_HD = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_R_BKPFs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T014s",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KKBER = table.Column<string>(nullable: true),
                    WAERS = table.Column<string>(nullable: true),
                    STAFO = table.Column<string>(nullable: true),
                    PERIV = table.Column<string>(nullable: true),
                    CTLPC = table.Column<string>(nullable: true),
                    KLIMK = table.Column<string>(nullable: true),
                    SBGRP = table.Column<string>(nullable: true),
                    ALLCC = table.Column<string>(nullable: true),
                    KKBTX = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T014s", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T052s",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZTERM = table.Column<string>(nullable: true),
                    ZTAG1 = table.Column<string>(nullable: true),
                    ZTAG2 = table.Column<string>(nullable: true),
                    ZTAG3 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T052s", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BKPFs");

            migrationBuilder.DropTable(
                name: "BSADs");

            migrationBuilder.DropTable(
                name: "BSEGs");

            migrationBuilder.DropTable(
                name: "BSIDs");

            migrationBuilder.DropTable(
                name: "KNA1s");

            migrationBuilder.DropTable(
                name: "KNB1s");

            migrationBuilder.DropTable(
                name: "KNKKs");

            migrationBuilder.DropTable(
                name: "KNVKs");

            migrationBuilder.DropTable(
                name: "R_BKPFs");

            migrationBuilder.DropTable(
                name: "T014s");

            migrationBuilder.DropTable(
                name: "T052s");

            migrationBuilder.DropPrimaryKey(
                name: "PK_T001",
                table: "T001");

            migrationBuilder.RenameTable(
                name: "T001",
                newName: "T001s");

            migrationBuilder.AddPrimaryKey(
                name: "PK_T001s",
                table: "T001s",
                column: "Id");
        }
    }
}
