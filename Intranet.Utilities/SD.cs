using System;

namespace Intranet.Utilities
{
    public static class SD
    {

        #region Admin Details

        public const string OfficeDomain = "pttphils.com";
        public const string SMTPClient = "smtp.office365.com";
        public const int SMTPPort = 587;
        public const bool SMTPBool = false;

        // Email Details
        public const string AdminEmail = "jose.baleros@pttphils.com";
        public const string AdminPass = "hfhnmbrphkcjnpjc";

        #endregion Admin Details

        #region CorpComm

        // Connection String
        //public static string ConString = "Server=(LocalDb)\\MSSQLLocalDB;Database=CorpComm;User Id=sa;Password=mis@2019;MultipleActiveResultSets=True";
        public static string ConString = "Server=192.168.10.43;Database=CorpComm;User Id=sa;Password=mis@2019;MultipleActiveResultSets=True";

        //public static string ConString = "Server=192.168.10.42;Database=CorpComm;User Id=sa;Password=mis@2019;MultipleActiveResultSets=True";

        // Cart Session
        public const string ssShoppingCart = "Shopping Cart Session";

        // Email of Admin
        public const string RenzEmail = "renz.sunico@pttphils.com";
        public const string CormCommEmail = "corpcomm@pttphils.com";
        public const string CormCommPass = "Subicbay5*";

        // Request Status
        public const string StatusForApproval = "For Approval";

        public const string StatusApproved = "Approved";
        public const string StatusForDelivery = "For Delivery";
        public const string StatusForAcknowledgement = "For Acknowledgement";
        public const string StatusForRating = "For Rating";
        public const string StatusRejected = "Rejected";
        public const string StatusCompleted = "Completed";
        public const string StatusCancelled = "Canceled";

        // Drop-Off Location
        public const string DO_Edsa = "EDSA Vet";

        public const string DO_LKG = "LKG Makati";

        // history
        public static int historyid = 0;

        public static string histLoginName;
        public static string histColId;
        public static string histColName;
        public static string histQty;
        public static string histReqDate;
        public static string histReconRemarks;

        // App URL
        // public const string IntranetLink = "http://qas.intranet.pttphils.com/";
        public const string IntranetLink = "http://localhost:44322/";

        //public const string IntranetLink = "http://intranet.pttphils.com/";

        // item value
        public static string collateralLoc;

        public static string collateralName;
        public static int collateralLocId;

        // Request type
        public const string ReqUrgent = "Urgent";

        public const string ReqRegular = "Regular";

        // Autorization
        public const string CorpCommAdmin = "CorpComm Admin";

        #endregion CorpComm

        #region MIS/ICT

        public const string CIOAdmin = "Office of the Chief Information Officer";
        public const string CIOAdminFake = "1Office of the Chief Information Officer";
        public const string ICTIntranet = "ICT Intranet";
        public const string MISIntranet = "MIS Intranet";
        public const string SAPConsult = "SAP Consultant Intranet";

        #endregion MIS/ICT

        #region QSHE

        public const string QSHEIntranet = "QSHE Intranet";
        public const string QSHEQtyAdmin = "QSHE QtyAdmin";
        public const string QSHEQtyUser = "QSHE QtyUser";
        public const string QSHESHEAdmin = "SSHE Admin";
        public const string QSHESHEUser = "SSHE User";

        #endregion QSHE

        #region CNC

        public const string CNCAdmin = "CNC Admin";
        public const string CNCUser = "CNC User";
        public const string DEfaultCNC = "odea Valle";

        // Start of Bank Info
        public const string BDO_GL = "11231041";
        public const string BPI_GL = "11231021";
        public const string SBC_GL = "11231151";
        public const string MBTC_GL = "11231061";

        public const string CNC_BDO = "BDO";
        public const string CNC_BPI = "BPI";
        public const string CNC_SBC = "SBC";
        public const string CNC_MBTC = "MBTC";

        public const string CNC_Currency = "PHP";
        public const string CNC_PostKey = "40";
        public const string CNC_PostKey2 = "15";
        // End of Bank Info


        #endregion CNC

        #region SPEM

        public const string SPEMIntranet = "SPEM Intranet";
        public const string SPEMLink = "http://192.168.10.42:9000/?id=";

        #endregion SPEM

        #region Logistics

        public const string LogisticsIntranet = "Logistics Management Intranet";

        #endregion Logistics

        #region Procurement

        public const string ProcurementIntranet = "Procurement Intranet";

        #endregion Procurement

        #region RawHtmlConverter

        public static string ConvertToRawHtml(String source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }

        #endregion RawHtmlConverter

        #region URL Links

        public const string Fiori = "https://fiori.pttphils.com:44300/fiori#Shell-home";
        public const string ESS = "http://sub-ess-prd.pttphils.com:50000/irj/portal";
        public const string Ticketing = "https://apps.powerapps.com/play/ffa735ea-8496-4cfa-84c2-ab57dbb240a0?tenantId=78db0b1d-e5d5-4f05-af3c-cc72575b1ba3";
        public const string ManualDeliveryRequest = "https://pttadmin.sharepoint.com/sites/LogisticsManagement/Lists/Manual%20DRLoading%20Request/AllItems.aspx";
        public const string VendorEvaluation = "https://pttadmin.sharepoint.com/sites/Vendor-Evaluation/VES/SitePages/Home.aspx?CT=1575440268879&OR=OWA-NT&CID=fdf82904-aec6-c146-1ba4-a9d5c3a03499";
        public const string GoPro = "https://pttadmin.sharepoint.com/sites/Procurement/Lists/Unable%20to%20Process/AllItems.aspx";
        public const string ChangeRequestLog = "https://pttadmin.sharepoint.com/:x:/r/MIS-ICT/_layouts/15/Doc.aspx?sourcedoc=%7BCF17F927-7CC5-4F98-8196-6F534A8D56BC%7D&file=Change%20Request%20Log.Request%20Number.2014.xlsx&action=default&mobileredirect=true&cid=a7069a15-c2b7-4f0a-8352-2fde6e0765cf";
        public const string DocumentMonitoring = "https://pttadmin.sharepoint.com/:x:/r/MIS-ICT/_layouts/15/Doc.aspx?sourcedoc=%7BB22F3651-FDA7-402E-9441-DD583A428A6A%7D&file=MIS%20ICT%20Document%20Monitoring%20Schedule%20Updated.xlsx&action=default&mobileredirect=true&cid=6df4a096-9dd6-45df-98f0-c02b37ed0a22";
        public const string PTTMobilePhoneRefreshMonitoring = "https://pttadmin.sharepoint.com/sites/mis-ict-ticket/ict/Lists/PTT%20Mobile%20Phone%20Refresh%20Monitoring/AllItems.aspx?viewpath=%2Fsites%2Fmis-ict-ticket%2Fict%2FLists%2FPTT%20Mobile%20Phone%20Refresh%20Monitoring%2FAllItems.aspx";
        public const string ICTTicketing = "https://pttadmin.sharepoint.com/sites/mis-ict-ticket/ict/SitePages/Home.aspx";
        public const string MISTicketing = "https://pttadmin.sharepoint.com/sites/mis-ict-ticket/Lists/MIS%20Ticket%202019/AllItems.aspx";
        public const string DxCare = "https://dxcare.pttdigital.com/";
        public const string Office365 = "https://www.office.com/";

        public const string LegalTicketing = "https://apps.powerapps.com/play/a77b0e16-86e4-435c-96b6-6526ac1f05c4?tenantId=78db0b1d-e5d5-4f05-af3c-cc72575b1ba3";

        //public const string VendorEvaluation = ;

        #endregion URL Links

    }
}