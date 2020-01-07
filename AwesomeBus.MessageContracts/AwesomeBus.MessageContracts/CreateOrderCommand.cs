﻿using NServiceBus;
using System;

namespace AwesomeBus.MessageContracts
{
    public class CreateOrderCommand : ICommand
    {
        public Guid OrderID { get; set; }
        public OrderData OrderData {get;set;}
    }
}
