using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreApiBase.Domain.Models
{
    public class StockPrice
    {
        public StockPrice(string symbol, double price)
        {
            Symbol = symbol;
            Price = price;
        }

        public string Symbol { get; private set; }
        public double Price { get; private set; }
    }
}
