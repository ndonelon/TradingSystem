using System;
using System.Collections.Generic;

namespace OrderSystem
{
    public class OrderBook
    {
        //Class holds all current GTC orders separated into buy/sell orders by security

        private Dictionary<string,SecurityOrders> orderBook { get; set; } = new Dictionary<string, SecurityOrders>();

        public void NewOrder(OrderDetails details, OrderType oType, OrderDirection oDir)
        {
            if (!orderBook.ContainsKey(details.Symbol))
            {
                orderBook[details.Symbol] = new SecurityOrders();
            }
            SecurityOrders symbolOrders = orderBook[details.Symbol];
            symbolOrders.NewOrder(details, oType, oDir);
        }

        public double OrderTotals(string symbol, OrderDirection oDir)
        {
            if (orderBook.ContainsKey(symbol))
            {
                SecurityOrders symbolOrders = orderBook[symbol];
                return symbolOrders.OrderQuantity(oDir);
            }
            return 0;
        }
    }
}