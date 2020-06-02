using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.Utilities
{
    public static class SD
    {
        public static string ConString = "Server=(LocalDb)\\MSSQLLocalDB;Database=CorpComm;User Id=sa;Password=mis@2019;MultipleActiveResultSets=True";

        public const string ssShoppingCart = "Shopping Cart Session";

        public const string StatusRequestSent = "Request Sent";
        public const string StatusForApproval = "For Approval";
        public const string StatusForDelivery = "For Delivery";
        public const string StatusForAcknowledgement = "For Acknowledgement";
        public const string StatusForRating = "For Rating";
        public const string StatusRejected = "Rejected";
        public const string StatusCompleted = "Completed";

        public const string CIOAdmin = "Office of the Chief Information Officer";

        public static int GetPriceBaseOnQuantity(int quantity, int price) 
        {
            int total = 0;
            total = price * quantity;
            return total;
        }
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

        //public const string EmailFrom = "jose.baleros@pttphils.com";
    }
}
