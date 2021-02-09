using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spice.Models;

namespace Spice.Utility
{
    public class SD
    {
        public const string DefaultFoodImage = "default_food.png";

        public const string ManagerUser = "Manager";
        public const string KitchenUser = "Kitchen";
        public const string FrontDeskUser = "FrontDesk";
        public const string CustomerEndUser = "Customer";

        public const string ssShoppingCartCount = "ssCartCount";
        public const string ssCouponCode = "ssCouponCode";


        public const string StatusSubmitted = "Submitted";
        public const string StatusInProcess = "Being Prepared";
        public const string StatusReady = "Ready for Pickup";
        public const string StatusCompleted = "Completed";
        public const string StatusCancelled = "Cancelled";

        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusRejected = "Rejected";


        public const string OrderPlaced = "OrderPlaced.png";
        public const string InKitchen = "InKitchen.png";
        public const string ReadyForPickup = "ReadyForPickup.png";
        public const string Completed = "completed.png";



        public static string ConvertToRawHtml(string source)
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

        public static double DiscountedPrice(CouponModel couponFromDb, double originalOrderTotal)
        {
            if (couponFromDb == null)
            {
                return originalOrderTotal;
            }
            else
            {
                if (couponFromDb.MinimumAmount > originalOrderTotal)
                {
                    return originalOrderTotal;
                }
                else
                {
                    //everything is valid
                    if (Convert.ToInt32(couponFromDb.CouponType) == (int)CouponModel.ECoupounType.Dollar)
                    {
                        //$10 off $100
                        return Math.Round(originalOrderTotal - couponFromDb.Discount, 2);
                    }
                    if (Convert.ToInt32(couponFromDb.CouponType) == (int)CouponModel.ECoupounType.Percent)
                    {
                        //10% off $100
                        return Math.Round(originalOrderTotal - (originalOrderTotal * couponFromDb.Discount / 100), 2);
                    }
                }
            }
            return originalOrderTotal;
        }
    }
}