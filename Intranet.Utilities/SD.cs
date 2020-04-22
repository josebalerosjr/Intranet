using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.Utilities
{
    public static class SD
    {
        public static string ConString = "Server=(LocalDb)\\MSSQLLocalDB;Database=CorpComm;User Id=sa;Password=mis@2020;MultipleActiveResultSets=True";

        public const string ssShoppingCart = "Shopping Cart Session";
        
        public static double GetPriceBaseOnQuantity(double quantity, double price, double total) 
        {
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
    }
}
