using CsvHelper;
using CsvHelper.Configuration;
using CsvUploader.Helpers;
using Intranet.Classes;
using Intranet.Data;
using Intranet.Models;
using Intranet.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Intranet.Controllers
{
    [Authorize(Roles = SD.CIOAdmin + "," + SD.CNCAdmin + "," + SD.CNCUser)]
    public class BdoPEController : Controller
    {
        private readonly BdoPEContext _context;
        private readonly EmailOptions _emailOptions;
        private readonly BdoInfo _bdoInfo;
        private readonly BpiInfo _bpiInfo;
        private readonly SbcInfo _sbcInfo;
        private readonly MtrInfo _mtrInfo;
        private readonly IToastNotification _toastNotification;

        public BdoPEController(
            BdoPEContext context,
            IOptions<EmailOptions> emailOptions,
            IOptions<BdoInfo> bdoInfo,
            IOptions<BpiInfo> bpiInfo,
            IOptions<SbcInfo> sbcInfo,
            IOptions<MtrInfo> mtrInfo,
            IToastNotification toastNotification
            )
        {
            _context = context;
            _emailOptions = emailOptions.Value;
            _bdoInfo = bdoInfo.Value;
            _bpiInfo = bpiInfo.Value;
            _sbcInfo = sbcInfo.Value;
            _mtrInfo = mtrInfo.Value;
            _toastNotification = toastNotification;
        }

        // GET: BdoPE/Create
        public async Task<IActionResult> Lsmw()
        {
            UserDetails();
            string cncuser = ViewBag.DisplayName;
            return View(await _context.bdoPEs.Where(c => c.isDownloaded == false && c.MarketerZ2 == cncuser).OrderByDescending(s => s.Id).ToListAsync());
        }

        [Authorize(Roles = SD.CIOAdmin)]
        public async Task<IActionResult> CustomerNumChecker()
        {
            UserDetails();
            string cncuser = ViewBag.DisplayName;
            return View(await _context.bdoPEs.Where(
                                c => c.MarketerZ2 == "" || c.MarketerZ2 == null ||
                                c.AmountDocCur == "" || c.AmountDocCur == null
                                ).ToListAsync());
        }

        // GET: BdoPE/Edit
        [HttpGet]
        [Authorize(Roles = SD.CIOAdmin)]
        public IActionResult CustomerNumCheckerEdit(int id)
        {
            UserDetails();
            return View(_context.bdoPEs.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.CIOAdmin)]
        public async Task<IActionResult> CustomerNumCheckerEdit(int id, [Bind("Id,DocDateInDoc,DocType,CompanyCode,PosDateInDoc,FiscalPeriod,CurrentKey,RefDocNum,DocHeadT,PosKeyInNextLine,AccMatNextLine,AmountDocCur,ValDate,AssignNum,ItemText,PosKeyInNextLine2,AccMatNextLine2,AmountDocCur2,BaseDateDueCal,ItemText2,MarketerZ2,isDownloaded,UserName,UserIP,UserDate")] BdoPE bdoPE)
        {
            UserDetails();
            if (id != bdoPE.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bdoPE);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BdoPEExists(bdoPE.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(LsmwAdmin));
            }
            return View(bdoPE);
        }

        // GET: BdoPE/Create
        [Authorize(Roles = SD.CIOAdmin + "," + SD.CNCAdmin)]
        public async Task<IActionResult> DownloadRecord()
        {
            UserDetails();
            string cncuser = ViewBag.DisplayName;
            return View(await _context.bdoPEs.Where(
                    c => c.DocType != null &&
                    c.isDownloaded == false &&
                    c.CompanyCode != null &&
                    c.AssignNum != null &&
                    c.ItemText != null &&
                    c.ItemText2 != null &&
                    c.isDownloaded == false).ToListAsync());
        }

        // GET: BdoPE/Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            UserDetails();
            return View(_context.bdoPEs.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DocType,CompanyCode,AssignNum,ItemText,ItemText2")] BdoPE bdoPE, string doctype, string companycode, string assignnum, string itemtext, string itemtext2)
        {
            UserDetails();

            if (id != bdoPE.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                string username = ViewBag.DisplayName;
                string ipadd = HttpContext.Connection.RemoteIpAddress.ToString(); ;
                string userdate = DateTime.Now.ToString("MM/dd/yyyy");

                var bdope = _context.bdoPEs.Find(bdoPE.Id);

                bdope.DocType = doctype;
                bdope.CompanyCode = companycode;
                bdope.AssignNum = assignnum;
                bdope.ItemText = itemtext;
                bdope.ItemText2 = itemtext2;
                bdope.UserName = username;
                bdope.UserIP = ipadd;
                bdope.UserDate = userdate;
                editToast();
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Lsmw));
            }
            return View(bdoPE);
        }

        #region BDO Uploading Actions

        // GET: BdoPE
        public async Task<IActionResult> Index()
        {
            UserDetails();
            return View(await _context.bdoPEs.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Index([Bind("Id,DocDateInDoc,DocType,CompanyCode,PosDateInDoc," +
            "FiscalPeriod,CurrentKey,RefDocNum,DocHeadT,PosKeyInNextLine,AccMatNextLine,AmountDocCur,ValDate," +
            "AssignNum,ItemText,PosKeyInNextLine2,AccMatNextLine2,AmountDocCur2,BaseDateDueCal,ItemText2,MarketerZ2," +
            "isDownloaded,UserName,UserIP,UserDate")] BdoPE bdoPE, List<IFormFile> files)
        {
            UserDetails(); // retrieve user details in Active directory

            // process for uploading and reading the file.

            #region Reading uploaded text file

            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filepath = Path.GetTempFileName();
            var users = new List<BDOCsv>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = formFile.OpenReadStream())
                    {
                        try { users.AddRange(stream.CsvToList<BDOCsv>()); }
                        catch (Exception ex) { return BadRequest(ex.Message); }
                    }
                }
            }

            #endregion Reading uploaded text file

            #region Accout Number Checker

            // Checks the account numbers from text file
            // This process checks first all the account numbers from the uploaded file
            // if it is not on the database it will redirect to a page putting all the list of accounts
            // that are not on the database to eliminate the error encountered while uploading
            int h = 0;
            var NoRecCustomer = new List<string>();
            if (users.Count != 0)
            {
                while (h < users.Count)
                {
                    string customernumberindb = CustomerNumber(users[h].CustomerNumber);

                    // calls a public function CustomerIsInDB to check the  list of
                    // account numbers from the text file and add the customers
                    // account that is not in the database to a dynamic ViewBag
                    CustomerIsInDB(NoRecCustomer, customernumberindb);
                    h++;
                }

                // if the dynamic ViewBag is has count it will open the page
                // and show account that are not on the database
                if (NoRecCustomer.Count != 0)
                {
                    ViewBag.NoCustomerNum = NoRecCustomer;
                    return View("NoCustomer");
                }
            }

            #endregion Accout Number Checker

            #region ORM method

            int i = 0;
            while (i < users.Count)
            {
                string transactiondate = users[i].TransactionDate;
                string customernumber = users[i].CustomerNumber;
                string transactionorigin = users[i].TransactionOrigin;
                string typeOfPayment = users[i].TypeOfPayment;
                string amount = users[i].TransactionAmount.ToString();
                string customername = users[i].CustomerName;
                string bankname = _bdoInfo.BankName;
                string glaccount = _bdoInfo.GL;
                string currency = _bdoInfo.Currency;
                string postkey = _bdoInfo.PostKey;
                string postkey2 = _bdoInfo.PostKey2;
                string username = ViewBag.DisplayName;
                string ipadd = HttpContext.Connection.RemoteIpAddress.ToString();
                string userdate = DateTime.Now.ToString("MM/dd/yyyy");

                bdoPE = new BdoPE();

                #region Insert to DB

                bdoPE.DocDateInDoc = TransactionDateBDO(transactiondate);
                bdoPE.PosDateInDoc = TransactionDateBDO(transactiondate);
                bdoPE.CurrentKey = Currency(currency);
                bdoPE.DocHeadT = BankName(bankname);
                bdoPE.PosKeyInNextLine = PostingKey(postkey);
                bdoPE.AccMatNextLine = GLAccount(glaccount);
                bdoPE.AmountDocCur = AmountBDO(amount);
                bdoPE.ValDate = TransactionDateBDO(transactiondate);
                bdoPE.PosKeyInNextLine2 = PostingKey(postkey2);
                bdoPE.AccMatNextLine2 = CustomerNumber(customernumber);
                bdoPE.AmountDocCur2 = AmountBDO(amount);
                bdoPE.BaseDateDueCal = TransactionDateBDO(transactiondate);
                bdoPE.isDownloaded = false;
                bdoPE.FiscalPeriod = FiscalPeriodBDO(transactiondate);
                bdoPE.RefDocNum = ReferenceBDO(transactionorigin);
                bdoPE.MarketerZ2 = CNCOfClient(customernumber);
                bdoPE.UserName = username;
                bdoPE.UserIP = ipadd;
                bdoPE.UserDate = userdate;
                i++;
                _context.Add(bdoPE);

                #endregion Insert to DB
            }

            #endregion ORM method

            // Toaster Event: notification event after the transaction.
            if (i == 0)
            {
                NoUploadedFile();
            }
            else
            {
                addToast();
            }

            // save all queued process on database
            // this will add all validated data to the dabase
            await _context.SaveChangesAsync();

            // redirect the page to Lsmw page
            return RedirectToAction(nameof(Lsmw));
        }

        #endregion BDO Uploading Actions

        #region SBC Uploading Actions

        public async Task<IActionResult> IndexSBC()
        {
            UserDetails();
            return View(await _context.bdoPEs.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> IndexSBC(
            [Bind("Id,DocDateInDoc,DocType,CompanyCode,PosDateInDoc,FiscalPeriod," +
                "CurrentKey,RefDocNum,DocHeadT,PosKeyInNextLine,AccMatNextLine,AmountDocCur," +
                "ValDate,AssignNum,ItemText,PosKeyInNextLine2,AccMatNextLine2,AmountDocCur2," +
                "BaseDateDueCal,ItemText2,MarketerZ2,isDownloaded,UserName,UserIP,UserDate")] BdoPE bdoPE, List<IFormFile> files)
        {
            // GET: user details
            UserDetails();

            #region Reading uploaded text file

            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filepath = Path.GetTempFileName();
            var users = new List<BDOCsv>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = formFile.OpenReadStream())
                    {
                        try
                        {
                            users.AddRange(stream.CsvToList<BDOCsv>());
                        }
                        catch (Exception ex)
                        {
                            return BadRequest(ex.Message);
                        }
                    }
                }
            }

            #endregion Reading uploaded text file

            #region Accout Number Checker

            // Checks the account numbers from text file
            // This process checks first all the account numbers from the uploaded file
            // if it is not on the database it will redirect to a page putting all the list of accounts
            // that are not on the database to eliminate the error encountered while uploading
            int h = 0;
            var NoRecCustomer = new List<string>();
            if (users.Count != 0)
            {
                while (h < users.Count)
                {
                    string customernumberindb = CustomerNumber(users[h].PaymentDetails.Substring(33, 15));

                    // calls a public function CustomerIsInDB to check the  list of
                    // account numbers from the text file and add the customers
                    // account that is not in the database to a dynamic ViewBag
                    CustomerIsInDB(NoRecCustomer, customernumberindb);
                    h++;
                }

                // if the dynamic ViewBag is has count it will open the page
                // and show account that are not on the database
                if (NoRecCustomer.Count != 0)
                {
                    ViewBag.NoCustomerNum = NoRecCustomer;
                    return View("NoCustomer");
                }
            }

            #endregion Accout Number Checker

            #region ORM method

            int i = 0;
            while (i < users.Count)
            {
                string customernumber = users[i].PaymentDetails.Substring(33, 15);
                string transactiondate = users[i].PaymentDetails.Substring(0, 2).Trim() +
                                   "/" + users[i].PaymentDetails.Substring(2, 2).Trim() +
                                   "/" + users[i].PaymentDetails.Substring(4, 4).Trim();
                string glaccount = _sbcInfo.GL;
                string currency = _sbcInfo.Currency;
                string bankname = _sbcInfo.BankName;
                string postingkey = _sbcInfo.PostKey;
                string postingkey2 = _sbcInfo.PostKey2;
                string amount = users[i].PaymentDetails.Substring(49, 13);
                string fiscalperiod = users[i].PaymentDetails.Substring(0, 2);
                string reference = users[i].PaymentDetails.Substring(8, 1);
                string cncofclient = CustomerNumber(customernumber);
                string username = ViewBag.DisplayName;
                string ipadd = HttpContext.Connection.RemoteIpAddress.ToString();
                string userdate = DateTime.Now.ToString("MM/dd/yyyy");

                bdoPE = new BdoPE();

                #region Insert to DB

                bdoPE.DocDateInDoc = TransactionDate(transactiondate);
                bdoPE.PosDateInDoc = TransactionDate(transactiondate);
                bdoPE.CurrentKey = Currency(currency);
                bdoPE.DocHeadT = BankName(bankname);
                bdoPE.PosKeyInNextLine = PostingKey(postingkey);
                bdoPE.AccMatNextLine = GLAccount(glaccount);
                bdoPE.AmountDocCur = Amount(amount);
                bdoPE.ValDate = TransactionDate(transactiondate);
                bdoPE.PosKeyInNextLine2 = PostingKey(postingkey2);
                bdoPE.AccMatNextLine2 = CustomerNumber(customernumber);
                bdoPE.AmountDocCur2 = Amount(amount);
                bdoPE.BaseDateDueCal = TransactionDate(transactiondate);
                bdoPE.isDownloaded = false;
                bdoPE.FiscalPeriod = FiscalPeriod(fiscalperiod);
                bdoPE.RefDocNum = ReferenceSBC(reference);
                bdoPE.MarketerZ2 = CNCOfClient(cncofclient);
                bdoPE.UserName = username;
                bdoPE.UserIP = ipadd;
                bdoPE.UserDate = userdate;
                i++;
                _context.Add(bdoPE);

                #endregion Insert to DB
            }

            #endregion ORM method

            // Toaster Event: notification event after the transaction.
            if (i == 0)
            {
                NoUploadedFile();
            }
            else
            {
                addToast();
            }

            // save all queued process on database
            // this will add all validated data to the dabase
            await _context.SaveChangesAsync();

            // redirect the page to Lsmw page
            return RedirectToAction(nameof(Lsmw));
        }

        #endregion SBC Uploading Actions

        #region BPI Uploading Action

        public async Task<IActionResult> IndexBPI()
        {
            UserDetails();
            return View(await _context.bdoPEs.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> IndexBPI([Bind("Id,DocDateInDoc,DocType,CompanyCode," +
            "PosDateInDoc,FiscalPeriod,CurrentKey,RefDocNum,DocHeadT,PosKeyInNextLine,AccMatNextLine," +
            "AmountDocCur,ValDate,AssignNum,ItemText,PosKeyInNextLine2,AccMatNextLine2,AmountDocCur2,BaseDateDueCal," +
            "ItemText2,MarketerZ2,isDownloaded,UserName,UserIP,UserDate")] BdoPE bdoPE, List<IFormFile> files)
        {
            UserDetails();

            #region Reading uploaded text file

            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filepath = Path.GetTempFileName();
            var users = new List<BDOCsv>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = formFile.OpenReadStream())
                    {
                        try
                        {
                            users.AddRange(stream.CsvToList<BDOCsv>());
                        }
                        catch (Exception ex)
                        {
                            return BadRequest(ex.Message);
                        }
                    }
                }
            }

            #endregion Reading uploaded text file

            #region Accout Number Checker

            // Checks the account numbers from text file
            // This process checks first all the account numbers from the uploaded file
            // if it is not on the database it will redirect to a page putting all the list of accounts
            // that are not on the database to eliminate the error encountered while uploading
            int h = 0;
            var NoRecCustomer = new List<string>();
            if (users.Count != 0)
            {
                while (h < users.Count)
                {
                    string rectype = users[h].PaymentDetails.Substring(0, 1);

                    // check is the string beginning is 2 which means it is it is bills payment
                    if (rectype == "2")
                    {
                        string customernumberindb = CustomerNumber(users[h].PaymentDetails.Substring(37, 20));

                        // calls a public function CustomerIsInDB to check the  list of
                        // account numbers from the text file and add the customers
                        // account that is not in the database to a dynamic ViewBag
                        CustomerIsInDB(NoRecCustomer, customernumberindb);
                    }
                    h++;
                }

                if (NoRecCustomer.Count != 0)
                {
                    // if the dynamic ViewBag is has count it will open the page
                    // and show account that are not on the database
                    ViewBag.NoCustomerNum = NoRecCustomer;
                    return View("NoCustomer");
                }
            }

            #endregion Accout Number Checker

            #region ORM method

            int i = 0;
            while (i < users.Count)
            {
                string rectype = users[i].PaymentDetails.Substring(0, 1);

                if (rectype == "2")
                {
                    string transactiondate = users[i].PaymentDetails.Substring(13, 2).Trim() +
                                       "/" + users[i].PaymentDetails.Substring(15, 2).Trim() +
                                       "/" + users[i].PaymentDetails.Substring(9, 4).Trim();
                    string glaccount = _bpiInfo.GL;
                    string currency = _bpiInfo.Currency;
                    string bankname = _bpiInfo.BankName;
                    string postkey = _bpiInfo.PostKey;
                    string postkey2 = _bpiInfo.PostKey2;
                    string amount = users[i].PaymentDetails.Substring(124, 13);
                    string fiscalperiod = users[i].PaymentDetails.Substring(13, 2);
                    string customernumber = users[i].PaymentDetails.Substring(37, 20);
                    string refer = users[i].PaymentDetails.Substring(30, 2); // OK
                    string username = ViewBag.DisplayName;
                    string ipadd = HttpContext.Connection.RemoteIpAddress.ToString(); ;
                    string userdate = DateTime.Now.ToString("MM/dd/yyyy");

                    bdoPE = new BdoPE();

                    #region Insert to DB

                    bdoPE.DocDateInDoc = TransactionDate(transactiondate);
                    bdoPE.PosDateInDoc = TransactionDate(transactiondate);
                    bdoPE.CurrentKey = Currency(currency);
                    bdoPE.DocHeadT = BankName(bankname);
                    bdoPE.PosKeyInNextLine = PostingKey(postkey);
                    bdoPE.AccMatNextLine = GLAccount(glaccount);
                    bdoPE.AmountDocCur = Amount(amount);
                    bdoPE.ValDate = TransactionDate(transactiondate);
                    bdoPE.PosKeyInNextLine2 = PostingKey(postkey2);
                    bdoPE.AccMatNextLine2 = CustomerNumber(customernumber);
                    bdoPE.AmountDocCur2 = Amount(amount);
                    bdoPE.BaseDateDueCal = TransactionDate(transactiondate);
                    bdoPE.isDownloaded = false;
                    bdoPE.FiscalPeriod = FiscalPeriod(fiscalperiod);
                    bdoPE.RefDocNum = ReferenceBPI(refer);
                    bdoPE.MarketerZ2 = CNCOfClient(customernumber);
                    bdoPE.UserName = username;
                    bdoPE.UserIP = ipadd;
                    bdoPE.UserDate = userdate;
                    _context.Add(bdoPE);

                    #endregion Insert to DB
                }
                i++;
            }

            #endregion ORM method

            // Toaster Event: notification event after the transaction.
            if (i == 0)
            {
                NoUploadedFile();
            }
            else
            {
                addToast();
            }

            // save all queued process on database
            // this will add all validated data to the dabase
            await _context.SaveChangesAsync();

            // redirect the page to Lsmw page
            return RedirectToAction(nameof(Lsmw));
        }

        #endregion BPI Uploading Action

        #region MTR Uploading Action

        public async Task<IActionResult> IndexMTR()
        {
            UserDetails();
            return View(await _context.bdoPEs.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> IndexMTR([Bind("Id,DocDateInDoc,DocType," +
            "CompanyCode,PosDateInDoc,FiscalPeriod,CurrentKey,RefDocNum,DocHeadT," +
            "PosKeyInNextLine,AccMatNextLine,AmountDocCur,ValDate,AssignNum,ItemText," +
            "PosKeyInNextLine2,AccMatNextLine2,AmountDocCur2,BaseDateDueCal,ItemText2," +
            "MarketerZ2,isDownloaded,UserName,UserIP,UserDate")] BdoPE bdoPE, List<IFormFile> files)
        {
            UserDetails();

            #region reading uploaded text file

            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filepath = Path.GetTempFileName();
            var users = new List<BDOCsv>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = formFile.OpenReadStream())
                    {
                        try
                        {
                            users.AddRange(stream.CsvToList<BDOCsv>());
                        }
                        catch (Exception ex)
                        {
                            return BadRequest(ex.Message);
                        }
                    }
                }
            }

            #endregion reading uploaded text file

            #region Accout Number Checker

            // Checks the account numbers from text file
            // This process checks first all the account numbers from the uploaded file
            // if it is not on the database it will redirect to a page putting all the list of accounts
            // that are not on the database to eliminate the error encountered while uploading
            int h = 0;
            var NoRecCustomer = new List<string>();
            if (users.Count != 0)
            {
                while (h < users.Count)
                {
                    string customernumberindb = CustomerNumber(users[h].PaymentDetails.Substring(41, 10));

                    // calls a public function CustomerIsInDB to check the  list of
                    // account numbers from the text file and add the customers
                    // account that is not in the database to a dynamic ViewBag
                    CustomerIsInDB(NoRecCustomer, customernumberindb);
                    h++;
                }

                if (NoRecCustomer.Count != 0)
                {
                    // if the dynamic ViewBag is has count it will open the page
                    // and show account that are not on the database
                    ViewBag.NoCustomerNum = NoRecCustomer;
                    return View("NoCustomer");
                }
            }

            #endregion Accout Number Checker

            #region ORM method

            int i = 0;
            while (i < users.Count)
            {
                string transactiondate = users[i].PaymentDetails.Substring(135, 2).Trim() +
                            "/" + users[i].PaymentDetails.Substring(137, 2).Trim() +
                            "/20" + users[i].PaymentDetails.Substring(139, 2).Trim();
                string glaccount = _mtrInfo.GL;
                string currency = _mtrInfo.Currency;
                string bankname = _mtrInfo.BankName;
                string postkey = _mtrInfo.PostKey;
                string postkey2 = _mtrInfo.PostKey2;
                string amount = users[i].PaymentDetails.Substring(101, 11);
                string customernumber = users[i].PaymentDetails.Substring(41, 30);
                string fiscalperiod = users[i].PaymentDetails.Substring(135, 2);
                string refer = users[i].PaymentDetails.Substring(53, 1); // OK
                string username = ViewBag.DisplayName;
                string ipadd = HttpContext.Connection.RemoteIpAddress.ToString(); ;
                string userdate = DateTime.Now.ToString("MM/dd/yyyy");

                bdoPE = new BdoPE();

                #region Insert to DB

                bdoPE.DocDateInDoc = TransactionDate(transactiondate);
                bdoPE.PosDateInDoc = TransactionDate(transactiondate);
                bdoPE.CurrentKey = Currency(currency);
                bdoPE.DocHeadT = BankName(bankname);
                bdoPE.PosKeyInNextLine = PostingKey(postkey);
                bdoPE.AccMatNextLine = GLAccount(glaccount);
                bdoPE.AmountDocCur = Amount(amount);
                bdoPE.ValDate = TransactionDate(transactiondate);
                bdoPE.PosKeyInNextLine2 = PostingKey(postkey2);
                bdoPE.AccMatNextLine2 = CustomerNumber(customernumber);
                bdoPE.AmountDocCur2 = Amount(amount);
                bdoPE.BaseDateDueCal = TransactionDate(transactiondate);
                bdoPE.isDownloaded = false;
                bdoPE.FiscalPeriod = FiscalPeriod(fiscalperiod);
                bdoPE.RefDocNum = ReferenceMTR(refer);
                bdoPE.MarketerZ2 = CNCOfClient(customernumber);
                bdoPE.UserName = username;
                bdoPE.UserIP = ipadd;
                bdoPE.UserDate = userdate;
                i++;
                _context.Add(bdoPE);

                #endregion Insert to DB
            }

            #endregion ORM method

            // Toaster Event: notification event after the transaction.
            if (i == 0)
            {
                NoUploadedFile();
            }
            else
            {
                addToast();
            }

            // save all queued process on database
            // this will add all validated data to the dabase
            await _context.SaveChangesAsync();

            // redirect the page to Lsmw page
            return RedirectToAction(nameof(Lsmw));
        }

        #endregion MTR Uploading Action

        // GET: BdoPE/Edit
        [HttpGet]
        [Authorize(Roles = SD.CIOAdmin + "," + SD.CNCAdmin)]
        public IActionResult EditAdmin(int id)
        {
            UserDetails();
            return View(_context.bdoPEs.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.CIOAdmin + "," + SD.CNCAdmin)]
        public async Task<IActionResult> EditAdmin(int id, [Bind("Id,DocDateInDoc,DocType,CompanyCode,PosDateInDoc,FiscalPeriod,CurrentKey,RefDocNum,DocHeadT,PosKeyInNextLine,AccMatNextLine,AmountDocCur,ValDate,AssignNum,ItemText,PosKeyInNextLine2,AccMatNextLine2,AmountDocCur2,BaseDateDueCal,ItemText2,MarketerZ2,isDownloaded,UserName,UserIP,UserDate")] BdoPE bdoPE)
        {
            UserDetails();
            if (id != bdoPE.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bdoPE);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BdoPEExists(bdoPE.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(LsmwAdmin));
            }
            return View(bdoPE);
        }

        [Authorize(Roles = SD.CIOAdmin + "," + SD.CNCAdmin)]
        public async Task<IActionResult> LsmwAdmin()
        {
            UserDetails();
            return View(await _context.bdoPEs.OrderByDescending(s => s.Id).ToListAsync());
        }

        [Authorize(Roles = SD.CIOAdmin + "," + SD.CNCAdmin)]
        public async Task<IActionResult> DownloadList([Bind("Id,isDownload")] BdoPE bdoPE)
        {
            #region download process

            UserDetails();
            string cncuser = ViewBag.DisplayName;

            var config = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                Delimiter = "\t"
            };

            var records = new List<BdoRpt>();
            var record = _context.bdoPEs.Where(
            c => c.DocType != null &&
            c.CompanyCode != null &&
            c.AssignNum != null &&
            c.ItemText != null &&
            c.ItemText2 != null &&
            c.isDownloaded == false).ToList();
            if (record.Count > 0)
            {
                foreach (var data in record)
                {
                    records.Add(new BdoRpt()
                    {
                        DocDateInDoc = data.DocDateInDoc,
                        DocType = data.DocType,
                        CompanyCode = data.CompanyCode,
                        PosDateInDoc = data.PosDateInDoc,
                        FiscalPeriod = data.FiscalPeriod,
                        CurrentKey = data.CurrentKey,
                        RefDocNum = data.RefDocNum,
                        DocHeadT = data.DocHeadT,
                        PosKeyInNextLine = data.PosKeyInNextLine,
                        AccMatNextLine = data.AccMatNextLine,
                        AmountDocCur = data.AmountDocCur,
                        ValDate = data.ValDate,
                        AssignNum = data.AssignNum,
                        ItemText = data.ItemText,
                        PosKeyInNextLine2 = data.PosKeyInNextLine2,
                        AccMatNextLine2 = data.AccMatNextLine2,
                        AmountDocCur2 = data.AmountDocCur2,
                        BaseDateDueCal = data.BaseDateDueCal,
                        ItemText2 = data.ItemText2,
                    });
                }

                // process of writting the record to the text file
                using (var writer = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files\\", "file.txt")))
                using (var csv = new CsvWriter(writer, config))
                {
                    csv.WriteRecords(records);
                }

                // start of isDownload
                // this process update the record from false to true.
                recordDownloaded();
                var bdope = _context.bdoPEs.Where(
                    c => c.DocType != null &&
                    c.CompanyCode != null &&
                    c.AssignNum != null &&
                    c.ItemText != null &&
                    c.ItemText2 != null &&
                    c.isDownloaded == false).ToList();
                foreach (var data in bdope)
                {
                    data.isDownloaded = true;
                }
                await _context.SaveChangesAsync();
                RedirectToPage(nameof(Lsmw));

                // lets the user save the file where they want to the save the file.
                var file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files\\", "file.txt");
                return File(System.IO.File.ReadAllBytes(file), "application/octet-stream", "file.txt");
            }
            else
            {
                NoRecordFound();
                return RedirectToAction(nameof(Lsmw));
            }

            #endregion download process
        }

        #region raw SQL

        //using (var sql2 = new SqlConnection(_emailOptions.DevConnection))
        //{
        //    using (var cmd = new SqlCommand()
        //    {
        //        CommandText = "SELECT [DocDateInDoc],[DocType],[CompanyCode],[PosDateInDoc],[FiscalPeriod],[CurrentKey],[RefDocNum]," +
        //                        "       [DocHeadT],[PosKeyInNextLine],[AccMatNextLine],[AmountDocCur],[ValDate],[AssignNum]," +
        //                        "       [ItemText],[PosKeyInNextLine2],[AccMatNextLine2],[AmountDocCur2],[BaseDateDueCal],[ItemText2],[isDownloaded]" +
        //                        "FROM   [dbo].[bdoPEs]" +
        //                        "WHERE  [DocType] = 'DZ' " +
        //                        "AND    [isDownloaded] = '0'",
        //        CommandType = CommandType.Text,
        //        Connection = sql2
        //    })
        //    {
        //        sql2.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();

        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                payments.DocDateInDoc = reader[1].ToString();
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine("No rows found");
        //        }
        //    }
        //}

        #endregion raw SQL

        #region Refactor Functions

        #region Customer Number

        // Customer Number Sanitation process
        public string CustomerNumber(string fromTextFile)
        {
            string customerNum = fromTextFile;
            string f_customerNum = "";

            // Check if the string length is more than 10
            if (customerNum.Length > 10)
            {
                // if more than 10, trim character that exceed 10
                f_customerNum = customerNum.Substring(0, 10).Trim();
            }
            else
            {
                // trim white spaces
                f_customerNum = customerNum.Trim();
            }

            // returns a value
            return f_customerNum;
        }

        #endregion Customer Number

        #region Transaction Date

        // Transaction Date Sanitation process
        public string TransactionDate(string fromTextFile)
        {
            string dates = fromTextFile;
            string f_dates = "";

            // Check if the string length is more than 10
            if (dates.Length > 10)
            {
                // if more than 10, trim character that exceed 10
                f_dates = dates.Substring(0, 10).Trim();
            }
            else
            {
                // trim white spaces
                f_dates = dates.Trim();
            }

            // returns a value
            return f_dates;
        }

        // Transaction Date Sanitation process for BDO
        public string TransactionDateBDO(string fromTextFile)
        {
            string dates = fromTextFile;
            string f_dates = "";

            // Check if the string length is more than 10
            if (dates.Length > 10)
            {
                // if more than 10, trim character that exceed 10
                f_dates = dates.Substring(0, 10).Trim();
            }
            else if (dates.Length < 10)
            {
                // trim white spaces
                f_dates = dates.Trim();

                // add 0 at the beginning of the string
                f_dates = "0" + dates.Trim();
            }
            else
            {
                // trim white spaces
                f_dates = dates.Trim();
            }

            // returns a value
            return f_dates;
        }

        #endregion Transaction Date

        #region GL Account

        // GL Account Sanitation process
        public string GLAccount(string fromTextFile)
        {
            string GL = fromTextFile;
            string f_GL = "";

            // Check if the string length is more than 10
            if (GL.Length > 10)
            {
                // if more than 10, trim character that exceed 10
                f_GL = GL.Substring(0, 10).Trim();
            }
            else
            {
                // trim white spaces
                f_GL = GL.Trim();
            }

            // returns a value
            return f_GL;
        }

        #endregion GL Account

        #region Currency

        // Currency Sanitation process
        public string Currency(string fromTextFile)
        {
            string currency = fromTextFile;
            string f_currency = "";

            // Check if the string length is more than 5
            if (currency.Length > 5)
            {
                // if more than 5, trim character that exceed 5
                f_currency = currency.Substring(0, 5).Trim();
            }
            else
            {
                // trim white spaces
                f_currency = currency.Trim();
            }

            // returns a value
            return f_currency;
        }

        #endregion Currency

        #region Bank Name

        // Bank Name Sanitation process
        public string BankName(string fromTextFile)
        {
            string bankName = fromTextFile;
            string f_bankName = "";

            // Check if the string length is more than 25
            if (bankName.Length > 25)
            {
                // if more than 25, trim character that exceed 25
                f_bankName = bankName.Substring(0, 25).Trim();
            }
            else
            {
                // trim white spaces
                f_bankName = bankName.Trim();
            }

            // returns a value
            return f_bankName;
        }

        #endregion Bank Name

        #region Posting Key

        // Posting Key Sanitation process
        public string PostingKey(string fromTextFile)
        {
            string postKey = fromTextFile;
            string f_postkey = "";

            // Check if the string length is more than 2
            if (postKey.Length > 2)
            {
                // if more than 2, trim character that exceed 2
                f_postkey = postKey.Substring(0, 2).Trim();
            }
            else
            {
                // trim white spaces
                f_postkey = postKey.Trim();
            }

            // returns a value
            return f_postkey;
        }

        #endregion Posting Key

        #region Amount

        // Amount Sanitation process
        public string Amount(string fromTextFile)
        {
            string amount = fromTextFile;
            string f_amount = "";

            // Check if the string length is more than 20
            if (amount.Length > 20)
            {
                // if more than 20, trim character that exceed 20
                f_amount = amount.Substring(0, 20).Trim();
            }
            else
            {
                // trim white spaces
                f_amount = amount.Trim();
            }

            f_amount = f_amount.Insert(amount.Length - CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits,
                    CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator)
                    .TrimStart('0');

            // returns a value
            return f_amount;
        }

        public string AmountBDO(string fromTextFile)
        {
            string amount = fromTextFile;
            string f_amount = "";

            // Check if the string length is more than 20
            if (amount.Length > 20)
            {
                // if more than 20, trim character that exceed 20
                f_amount = amount.Substring(0, 20).Trim();
            }
            else
            {
                // trim white spaces
                f_amount = amount.Trim();
            }

            // returns a value
            return f_amount;
        }

        #endregion Amount

        #region Fiscal Period

        // Fiscal Period Sanitation process
        public string FiscalPeriod(string fromTextFile)
        {
            string fiscalperiod = fromTextFile;
            string f_fiscalperiod = "";

            // Check if the string length is more than 2
            if (fiscalperiod.Length > 2)
            {
                // if more than 2, trim character that exceed 2
                f_fiscalperiod = fiscalperiod.Substring(0, 2).Trim();
            }
            else
            {
                // trim white spaces
                f_fiscalperiod = fiscalperiod.Trim();
            }

            // returns a value
            return f_fiscalperiod;
        }

        // Fiscal Period Sanitation process for BDO
        public string FiscalPeriodBDO(string fromtransactionDate)
        {
            string f_fiscal = "";

            // Check if the string length is more than 10
            if (fromtransactionDate.Length < 10)
            {
                // get the first digit of the date then add 0 at the start
                f_fiscal = "0" + fromtransactionDate.Substring(0, 1);
            }
            else
            {
                // trim character that exceed 2
                f_fiscal = fromtransactionDate.Substring(0, 2).Trim();
            }

            // returns a value
            return f_fiscal;
        }

        #endregion Fiscal Period

        #region CNCOfClient

        // CNCOfClient return a value
        // GET: Credit and Collection of the Customer
        public string CNCOfClient(string fromTextFile)
        {
            string cncofclient = null;
            string f_cncofclient = null;
            string ff_cncofclient = null;
            string DevConnection = Startup.StaticConfig.GetConnectionString("DevConnection");

            // run a query that check if there is a record on the database with
            // the selected customer number from the uploaded file
            using (var sql = new SqlConnection(DevConnection))
            {
                using (var cmd = new SqlCommand()
                {
                    CommandText = "SELECT * " +
                                    "FROM CreditAndCollectionDetails " +
                                    "WHERE CreditAndCollectionDetails.KUNNR = @customernum",
                    CommandType = CommandType.Text,
                    Connection = sql
                })
                {
                    sql.Open();
                    cmd.Parameters.Add("@customernum", SqlDbType.NVarChar).Value = CustomerNumber(fromTextFile);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            // assign the value to the cnccloent string
                            cncofclient = reader[6].ToString();
                        }
                    }
                }
            }

            // retrieve thr cnc number that is assign my the customer
            using (var sql = new SqlConnection(DevConnection))
            {
                using (var cmd = new SqlCommand()
                {
                    CommandText = "SELECT * " +
                                    "FROM CustomerDetails " +
                                    "WHERE CustomerDetails.KUNNR = @cncofclient",
                    CommandType = CommandType.Text,
                    Connection = sql
                })
                {
                    sql.Open();
                    cmd.Parameters.Add("@cncofclient", SqlDbType.NVarChar).Value = cncofclient; // OK
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {   // if it has assigned cnc assign it to the f_cncclient
                            f_cncofclient = reader[2].ToString();
                        }
                        else
                        {  // if it does not have assigned cnc,  assign the value of the DefaultCNC
                            f_cncofclient = _emailOptions.DefaultCNC;
                        }
                    }
                    else
                    {   // if it does not have assigned cnc,  assign the value of the DefaultCNC
                        f_cncofclient = _emailOptions.DefaultCNC;
                    }
                }
            }

            ff_cncofclient = f_cncofclient.Trim();

            // return a value
            return ff_cncofclient;
        }

        #endregion CNCOfClient

        #region CustomerIsInDB

        // CustomerIsInDB returns a value
        // GET: list of customer number that is not in the Database yet
        // then assign it to NoRecCustomer
        public List<string> CustomerIsInDB(List<string> NoRecCustomer, string customernumberindb)
        {
            string DevConnection = Startup.StaticConfig.GetConnectionString("DevConnection");
            using (var sql = new SqlConnection(DevConnection))
            {
                using (var cmd = new SqlCommand()
                {
                    CommandText = "SELECT * " +
                                    "FROM CreditAndCollectionDetails " +
                                    "WHERE CreditAndCollectionDetails.KUNNR = @customernum",
                    CommandType = CommandType.Text,
                    Connection = sql
                })
                {
                    sql.Open();
                    cmd.Parameters.Add("@customernum", SqlDbType.NVarChar).Value = customernumberindb;
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        NoRecCustomer.Add(customernumberindb);
                    }
                }
            }

            return NoRecCustomer;
        }

        #endregion CustomerIsInDB

        #region Reference

        // Reference SBC Sanitation process
        public string ReferenceSBC(string fromTextFile)
        {
            string refer = fromTextFile; // OK
            string f_refer = "";
            string ff_refer = "";
            switch (refer)
            {
                case "1":
                    f_refer = "Cash";
                    break;

                case "2":
                    f_refer = "On-us Check";
                    break;

                case "3":
                    f_refer = "Debit Memo";
                    break;

                default:
                    f_refer = "Off-us Check";
                    break;
            }
            if (f_refer.Length > 16)
            {
                ff_refer = f_refer.Substring(0, 16);
            }
            else
            {
                ff_refer = f_refer.Trim();
            }

            return ff_refer;
        }

        // Reference BDO Sanitation process
        public string ReferenceBDO(string fromTextFile)
        {
            string refer = fromTextFile; // OK
            string f_refer = "";
            string ff_refer = "";
            switch (refer)
            {
                case "1":
                    f_refer = "OverTCnt";
                    break;

                case "2":
                    f_refer = "RIB";
                    break;

                case "3":
                    f_refer = "CIB";
                    break;

                case "4":
                    f_refer = "PhoneBnk";
                    break;

                case "5":
                    f_refer = "ATM";
                    break;

                case "6":
                    f_refer = "Mobile";
                    break;

                default:
                    f_refer = "POS";
                    break;
            }
            if (f_refer.Length > 16)
            {
                ff_refer = f_refer.Substring(0, 16);
            }
            else
            {
                ff_refer = f_refer.Trim();
            }

            return ff_refer;
        }

        public string ReferenceBPI(string fromTextFile)
        {
            string f_refer = "";
            string ff_refer = "";
            switch (fromTextFile)
            {
                case "01":
                    f_refer = "ATM";
                    break;

                case "02":
                    f_refer = "Express Phone";
                    break;

                case "03":
                    f_refer = "ExpressLink (Corp Bills Pmt & ADA)";
                    break;

                case "04":
                    f_refer = "Over-the-counter";
                    break;

                case "05":
                    f_refer = "(Reserved)";
                    break;

                case "06":
                    f_refer = "Express Online & Express Mobile";
                    break;

                default:
                    f_refer = "Express Bills Payment & Presentment";
                    break;
            }
            if (f_refer.Length > 16)
            {
                ff_refer = f_refer.Substring(0, 16);
            }
            else
            {
                ff_refer = f_refer.Trim();
            }

            return ff_refer;
        }

        public string ReferenceMTR(string fromTextFile)
        {
            string f_refer = "";
            string ff_refer = "";
            switch (fromTextFile)
            {
                case "CS":
                    f_refer = "Cash";
                    break;

                case "DR":
                    f_refer = "Credit/Debit Acc";
                    break;

                case "CK":
                    f_refer = "Check Payment";
                    break;

                case "SM":
                    f_refer = "Smart MobileBank";
                    break;

                case "IB":
                    f_refer = "Internet Banking";
                    break;

                case "MP":
                    f_refer = "Metro Phone Bank";
                    break;

                case "ET":
                    f_refer = "ATM";
                    break;

                case "RI":
                    f_refer = "Remittance Int'l";
                    break;

                default:
                    f_refer = "Remittance Local";
                    break;
            }
            if (f_refer.Length > 16)
            {
                ff_refer = f_refer.Substring(0, 16);
            }
            else
            {
                ff_refer = f_refer.Trim();
            }
            return ff_refer;
        }

        #endregion Reference

        #endregion Refactor Functions

        // POST: BdoPE/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            UserDetails();
            var bdoPE = await _context.bdoPEs.FindAsync(id);
            _context.bdoPEs.Remove(bdoPE);
            delToast();
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lsmw));
        }

        // POST: BdoPE/Delete/5
        public async Task<IActionResult> DeleteAdmin(int? id)
        {
            UserDetails();
            var bdoPE = await _context.bdoPEs.FindAsync(id);
            _context.bdoPEs.Remove(bdoPE);
            delToast();
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(LsmwAdmin));
        }

        // POST: BdoPE/Delete/5
        public async Task<IActionResult> CustomerNumCheckerDelete(int? id)
        {
            UserDetails();
            var bdoPE = await _context.bdoPEs.FindAsync(id);
            _context.bdoPEs.Remove(bdoPE);
            delToast();
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CustomerNumChecker));
        }

        private bool BdoPEExists(int id)
        {
            return _context.bdoPEs.Any(e => e.Id == id);
        }

        #region UserDetails function

        public void UserDetails()
        {
            var username = User.Identity.Name;
            var domain = _emailOptions.AuthDomain;
            using (var context = new PrincipalContext(ContextType.Domain, domain))
            {
                var user = UserPrincipal.FindByIdentity(context, username);
                ViewBag.Department = user.GetDepartment();
                ViewBag.DisplayName = user.GetDisplayname();
            }
        }

        #endregion UserDetails function

        #region Toastr function

        public void delToast()
        {
            _toastNotification.AddErrorToastMessage("The record has been deleted successfully.", new ToastrOptions()
            {
                Title = ""
            });
        }

        public void NoUploadedFile()
        {
            _toastNotification.AddErrorToastMessage("No selected file to upload.", new ToastrOptions()
            {
                Title = ""
            });
        }

        public void NoRecordFound()
        {
            _toastNotification.AddErrorToastMessage("No record to download.", new ToastrOptions()
            {
                Title = ""
            });
        }

        public void addToast()
        {
            _toastNotification.AddSuccessToastMessage("Bills payment successfully uploaded!", new ToastrOptions()
            {
                Title = ""
            });
        }

        public void recordDownloaded()
        {
            _toastNotification.AddSuccessToastMessage("Download successfull.!", new ToastrOptions()
            {
                Title = ""
            });
        }

        public void editToast()
        {
            _toastNotification.AddInfoToastMessage("The record has been updated successfully.", new ToastrOptions()
            {
                Title = ""
            });
        }

        public void warnToast()
        {
            _toastNotification.AddInfoToastMessage("The record already exists", new ToastrOptions()
            {
                Title = ""
            });
        }

        #endregion Toastr function
    }
}