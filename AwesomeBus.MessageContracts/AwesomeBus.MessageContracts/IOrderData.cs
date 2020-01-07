using System;
using System.Collections.Generic;
using System.Text;

namespace AwesomeBus.MessageContracts
{
    public interface IOrderData
    {
        string OrderItem { get; set; }
        int Quantity { get; set; }
        double TotalPrice { get; set;}
        string OrderDateTime { get; set; }
       
    }
}
