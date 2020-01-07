using System;
using System.Collections.Generic;
using System.Text;

namespace AwesomeBus.MessageContracts
{
    public class OrderData : IOrderData
    {
        public string OrderItem { get ; set ; }
        public int Quantity { get ; set ; }
        public double TotalPrice { get ; set ; }
        public string OrderDateTime { get; set ; }
    }
}
