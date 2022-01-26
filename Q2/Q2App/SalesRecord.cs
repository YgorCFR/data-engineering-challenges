using System;
using System.Collections.Generic;
using System.Text;

namespace Q2App
{
    public class SalesRecord
    {
        public string Region { get; set; }
        public string Country { get; set; }
        public string ItemType { get; set; }
        public string SalesChannel { get; set; }
        public string OrderPriority { get; set; }
        public DateTime OrderDate { get; set; }
        public int OrderID { get; set; }
        public DateTime ShipDate { get; set; }
        public int UnitsSold { get; set; }
        public float UnitPrice { get; set; }
        public float UnitCost { get; set; }
        public float TotalRevenue { get; set; }
        public float TotalCost { get; set; }
        public float TotalProfit { get; set; }

        public SalesRecord () { }

        public SalesRecord(string region, string country, string itemType, string salesChannel, string orderPriority, DateTime orderDate, int orderID, DateTime shipDate, int unitsSold, float unitPrice, float unitCost, float totalRevenue, float totalCost, float totalProfit)
        {
            Region = region;
            Country = country;
            ItemType = itemType;
            SalesChannel = salesChannel;
            OrderPriority = orderPriority;
            OrderDate = orderDate;
            OrderID = orderID;
            ShipDate = shipDate;
            UnitsSold = unitsSold;
            UnitPrice = unitPrice;
            UnitCost = unitCost;
            TotalRevenue = totalRevenue;
            TotalCost = totalCost;
            TotalProfit = totalProfit;
        }
    }
}
