using System;
using System.Linq;
using System.Collections.Generic;

namespace OrderSystem
{
    public class SecurityOrders
    {
        //Class holds all current GTC orders separated into buy/sell orders by security
        private List<OrderDetails> buyOrders = new List<OrderDetails>();
        private List<OrderDetails> sellOrders = new List<OrderDetails>();

        public void NewOrder(OrderDetails details, OrderType oType, OrderDirection oDir)
        {
            //if order is buy then check sells for match.
            //if no match and it is GTC then add to buy book.
            //
            //if order is sell then check buys for match
            //if no match and it is GTC then add to sell book.

            if (oDir==OrderDirection.BUY)
            {
                CheckForBuyMatch(details, oType);
                RemoveFilledOrders(sellOrders);
            }
            else if (oDir ==OrderDirection.SELL)
            {
                CheckForSellMatch(details, oType);
                RemoveFilledOrders(buyOrders);
            }
            else
            {
                throw new UnknownSideException();
            }
        }

        private void RemoveFilledOrders(List<OrderDetails> orders)
        {
            orders.RemoveAll(x => x.Quantity == 0);
        }

        public double OrderQuantity(OrderDirection oDir)
        {
            if (oDir == OrderDirection.BUY)
            {
                return (from order in buyOrders
                        select order.Quantity).Sum();
            }
            else if (oDir == OrderDirection.SELL)
            {
                return (from order in sellOrders
                        select order.Quantity).Sum();
            }
            else
            {
                throw new UnknownSideException();
            }
        }

        void CheckForBuyMatch(OrderDetails newOrder, OrderType oType)
        {
            bool bMatchedOrder = false;
            double dRemainingQty = newOrder.Quantity;
            foreach (var historicOrder in sellOrders)
            {
                if (historicOrder.Price <= newOrder.Price)
                {
                    bMatchedOrder = true;
                    if (newOrder.Quantity > historicOrder.Quantity)
                    {
                        dRemainingQty -= historicOrder.Quantity;
                        historicOrder.Quantity = 0.0;
                        Console.WriteLine($"Matched : {newOrder.Quantity}");
                    }
                    else if (newOrder.Quantity < historicOrder.Quantity)
                    {
                        dRemainingQty = 0;
                        historicOrder.Quantity -= newOrder.Quantity;
                        Console.WriteLine($"Matched : {newOrder.Quantity}");
                    }
                    else //exact match
                    {
                        dRemainingQty = 0;
                        historicOrder.Quantity = 0;
                        Console.WriteLine($"Matched : {newOrder.Quantity}");
                    }
                }
            }
            if (bMatchedOrder==false && oType==OrderType.GTC)
            {
                buyOrders.Add(newOrder);
                buyOrders.Sort();
            }
        }

        void CheckForSellMatch(OrderDetails newOrder, OrderType oType)
        {
            bool bMatchedOrder = false;

            if (buyOrders.Count == 0)
            {
                sellOrders.Add(newOrder);
                return;
            }

            for (int i=buyOrders.Count-1;i>=0;i--)
            {
                var historicOrder = buyOrders[i];
                double dRemainingQty = newOrder.Quantity;
                if (historicOrder.Price >= newOrder.Price)
                {
                    bMatchedOrder = true;
                    if (newOrder.Quantity > historicOrder.Quantity)
                    {
                        dRemainingQty -= historicOrder.Quantity;
                        historicOrder.Quantity = 0.0;
                        Console.WriteLine($"Matched : {newOrder.Quantity}");
                    }
                    else if (newOrder.Quantity < historicOrder.Quantity)
                    {
                        dRemainingQty = 0;
                        historicOrder.Quantity -= newOrder.Quantity;
                        Console.WriteLine($"Matched : {newOrder.Quantity}");
                    }
                    else //exact match
                    {
                        dRemainingQty = 0;
                        historicOrder.Quantity = 0;
                        Console.WriteLine($"Matched : {newOrder.Quantity}");
                    }
                }
            }
            if (bMatchedOrder == false && oType == OrderType.GTC)
            {
                sellOrders.Add(newOrder);
                sellOrders.Sort();
            }
        }
    }
}