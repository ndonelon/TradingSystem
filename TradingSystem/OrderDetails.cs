using System;
using System.Collections.Generic;

namespace OrderSystem
{
    public class OrderDetails : IComparable<OrderDetails>
    {
        public OrderDetails(string symbol, double qty, double price)
        {
            Symbol = symbol;
            Quantity = qty;
            Price = price;
        }

        public string Symbol { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }

        public int CompareTo(OrderDetails other)
        {
            if (this.Price < other.Price)
                return 1;
            else if (this.Price < other.Price)
                return -1;
            else
                return 0;
        }
        
    }
}