﻿using HomeTask.Domain.Aggregates.Base;
using HomeTask.Domain.Aggregates.ClientAgg;

namespace HomeTask.Domain.Aggregates.OrderAgg
{
    public class Order : IEntity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int ClientId { get; set; }

        // -> navigation properties
        public virtual Client Client { get; set; }
    }
}