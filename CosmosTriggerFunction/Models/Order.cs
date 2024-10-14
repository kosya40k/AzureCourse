using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosTriggerFunction.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public double Total { get; set; }

        public List<OrderItem> Items { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"OrderId: {OrderId}");
            sb.AppendLine($"OrderDate: {OrderDate}");
            sb.AppendLine($"Total: {Total}");

            sb.AppendLine("Items: ");
            foreach (var item in Items) 
            {
                sb.AppendLine(item.ToString());
            }

            return sb.ToString();
        }
    }
}