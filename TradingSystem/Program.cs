using System;

namespace OrderSystem
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Test1_load_buy_orders();
            Test2_load_mixed_orders();
            Test3_load_mixed_orders_different_symbols();
            Test4_check_IOC_ignored_on_add();
            Test5_check_sell_order_match();
            Test6_check_sell_order_not_match();
            Test7_check_buy_order_not_match();
            Test8_check_buy_order_match();

            Console.WriteLine("Done... (Press a key to close)");
            Console.ReadKey();
        }

        private static void Test1_load_buy_orders()
        {
            Console.WriteLine("Test1 Load Buy Orders");
            var orders = new OrderBook();
            orders.NewOrder(new OrderDetails("AA01", 1000, 45), OrderType.GTC, OrderDirection.BUY);
            orders.NewOrder(new OrderDetails("AA01", 3000, 50), OrderType.GTC, OrderDirection.BUY);
            Assert(AreEqual(orders.OrderTotals("AA01",OrderDirection.BUY), 4000),
                " Test1 Failed, Expected Value:" + "\t" + 4000 + ",\t" + "Actual Value: \t" + orders.OrderTotals("AA01", OrderDirection.BUY) + "\n");
        }

        private static void Test2_load_mixed_orders()
        {
            Console.WriteLine("Test2 Load Mixed Orders");
            var orders = new OrderBook();
            orders.NewOrder(new OrderDetails("AA01", 2000, 50), OrderType.GTC, OrderDirection.BUY);
            orders.NewOrder(new OrderDetails("AA01", 3000, 45), OrderType.GTC, OrderDirection.BUY);
            orders.NewOrder(new OrderDetails("AA01", 500, 60), OrderType.GTC, OrderDirection.SELL);
            orders.NewOrder(new OrderDetails("AA01", 200, 65), OrderType.GTC, OrderDirection.SELL);
            Assert(AreEqual(orders.OrderTotals("AA01", OrderDirection.BUY), 5000),
                " Test2 Failed, Expected Value:" + "\t" + 5000 + ",\t" + "Actual Value: \t" + orders.OrderTotals("AA01", OrderDirection.BUY) + "\n");
            Assert(AreEqual(orders.OrderTotals("AA01", OrderDirection.SELL), 700),
                " Test2 Failed, Expected Value:" + "\t" + 700 + ",\t" + "Actual Value: \t" + orders.OrderTotals("AA01", OrderDirection.SELL) + "\n");
        }

        private static void Test3_load_mixed_orders_different_symbols()
        {
            Console.WriteLine("Test3 Load Mixed Orders Different Symbols");
            var orders = new OrderBook();
            orders.NewOrder(new OrderDetails("AA01", 150, 50), OrderType.GTC, OrderDirection.BUY);
            orders.NewOrder(new OrderDetails("AA01", 210, 45), OrderType.GTC, OrderDirection.BUY);
            orders.NewOrder(new OrderDetails("AA02", 330, 50), OrderType.GTC, OrderDirection.SELL);
            orders.NewOrder(new OrderDetails("AA02", 120, 45), OrderType.GTC, OrderDirection.SELL);
            Assert(AreEqual(orders.OrderTotals("AA01", OrderDirection.BUY), 360),
                " Test3 Failed, Expected Value:" + "\t" + 360 + ",\t" + "Actual Value: \t" + orders.OrderTotals("AA01", OrderDirection.BUY) + "\n");
            Assert(AreEqual(orders.OrderTotals("AA02", OrderDirection.SELL), 450),
                " Test3 Failed, Expected Value:" + "\t" + 450 + ",\t" + "Actual Value: \t" + orders.OrderTotals("AA02", OrderDirection.SELL) + "\n");
        }

        private static void Test4_check_IOC_ignored_on_add()
        {
            Console.WriteLine("Test4 Check IOC Ignored On Add");
            var orders = new OrderBook();
            orders.NewOrder(new OrderDetails("AA01", 2000, 50), OrderType.IOC, OrderDirection.BUY);
            Assert(AreEqual(orders.OrderTotals("AA01", OrderDirection.BUY), 0),
                " Test4 Failed, Expected Value:" + "\t" + 0 + ",\t" + "Actual Value: \t" + orders.OrderTotals("AA01", OrderDirection.BUY) + "\n");
        }

        private static void Test5_check_sell_order_match()
        {
            Console.WriteLine("Test5 Check Sell Orders Match");
            var orders = new OrderBook();
            orders.NewOrder(new OrderDetails("AA01", 2000, 50), OrderType.GTC, OrderDirection.BUY);
            orders.NewOrder(new OrderDetails("AA01", 400, 49), OrderType.GTC, OrderDirection.SELL);
            Assert(AreEqual(orders.OrderTotals("AA01", OrderDirection.BUY), 1600),
                " Test5 Failed, Expected Value:" + "\t" + 1600 + ",\t" + "Actual Value: \t" + orders.OrderTotals("AA01", OrderDirection.BUY) + "\n");
        }

        private static void Test6_check_sell_order_not_match()
        {
            Console.WriteLine("Test6 Check Sell Orders Not Match");
            var orders = new OrderBook();
            orders.NewOrder(new OrderDetails("AA01", 2000, 50), OrderType.GTC, OrderDirection.BUY);
            orders.NewOrder(new OrderDetails("AA01", 400, 51), OrderType.GTC, OrderDirection.SELL);
            Assert(AreEqual(orders.OrderTotals("AA01", OrderDirection.BUY), 2000),
                " Test6 Failed, Expected Value:" + "\t" + 2000 + ",\t" + "Actual Value: \t" + orders.OrderTotals("AA01", OrderDirection.BUY) + "\n");
        }

        private static void Test7_check_buy_order_not_match()
        {
            Console.WriteLine("Test7 Check Buy Order Not match");
            var orders = new OrderBook();
            orders.NewOrder(new OrderDetails("AA01", 2000, 50), OrderType.GTC, OrderDirection.SELL);
            orders.NewOrder(new OrderDetails("AA01", 400, 49), OrderType.GTC, OrderDirection.BUY);
            Assert(AreEqual(orders.OrderTotals("AA01", OrderDirection.SELL), 2000),
                " Test7 Failed, Expected Value:" + "\t" + 2000 + ",\t" + "Actual Value: \t" + orders.OrderTotals("AA01", OrderDirection.SELL) + "\n");
        }

        private static void Test8_check_buy_order_match()
        {
            Console.WriteLine("Test8 Check Buy Order Match");
            var orders = new OrderBook();
            orders.NewOrder(new OrderDetails("AA01", 2000, 50), OrderType.GTC, OrderDirection.SELL);
            orders.NewOrder(new OrderDetails("AA01", 400, 51), OrderType.GTC, OrderDirection.BUY);
            Assert(AreEqual(orders.OrderTotals("AA01", OrderDirection.SELL), 1600),
                " Test8 Failed, Expected Value:" + "\t" + 1600 + ",\t" + "Actual Value: \t" + orders.OrderTotals("AA01", OrderDirection.SELL) + "\n");
        }

        private static void Assert(bool condition, string failure)
        {
            if (!condition)
                Console.WriteLine(failure);
        }

        private static bool AreEqual(double d1, double d2)
        {
            return Math.Abs(d1 - d2) < .0001;
        }
    }
}