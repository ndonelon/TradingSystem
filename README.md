# TradingSystem

First cut of TradingSystem solution. 
Orders are held in OrderBook class. New orders are added by calling NewOrder method.
The order book is inspected for match following each new order and match is reported and ladder updated if appropriate.

OrderBook holds a map of string to SecurityOrder class. 
SecurityOrder class holds lists of buy & sell orders currently held. Adding a new order causes the existing lists to be inspected for matches. A buy order causes sell list to be inspected for matches +vv. Lists are updated after check for match to remove any filled orders - their quantity is now zero.

Unit tests are provided in Program.cs to :

Test1-3 - load different instruments in both buys & sells.
Test4 - ensure IOC not added to ladder.
Test5 - sell order match existing buys.
Test6 - sell order not match existing buys.
Test7 - buy order not match existing sells.
Test8 - buy order match existing sells.
