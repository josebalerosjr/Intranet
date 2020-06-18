using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.Utilities
{
    public static class SD
    {
        #region CorpComm
        // Connection String
        public static string ConString = "Server=(LocalDb)\\MSSQLLocalDB;Database=CorpComm;User Id=sa;Password=mis@2019;MultipleActiveResultSets=True";
        //public static string ConString = "Server=192.168.10.43;Database=CorpComm;User Id=sa;Password=mis@2019;MultipleActiveResultSets=True";

        // Cart Session
        public const string ssShoppingCart = "Shopping Cart Session";

        // Request Status
        public const string StatusRequestSent = "Request Sent";
        public const string StatusForApproval = "For Approval";
        public const string StatusForDelivery = "For Delivery";
        public const string StatusForAcknowledgement = "For Acknowledgement";
        public const string StatusForRating = "For Rating";
        public const string StatusRejected = "Rejected";
        public const string StatusCompleted = "Completed";

        // Drop-Off Location
        public const string DO_Edsa = "EDSA Vet";
        public const string DO_LKG = "LKG Makati";

        // item value
        public static string collateralLoc;
        public static string collateralName;
        public static int collateralLocId;

        // Request type
        public const string ReqUrgent = "Urgent";
        public const string ReqRegular = "Regular";

        #endregion

        #region MIS/ICT
        public const string CIOAdmin = "Office of the Chief Information Officer";
        #endregion

        //public static int GetPriceBaseOnQuantity(int quantity, int price) 
        //{
        //    int total = 0;
        //    total = price * quantity;
        //    return total;
        //}

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
        #endregion
    }
}
