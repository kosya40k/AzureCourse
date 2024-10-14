
using System.Text;

namespace CosmosTriggerFunction.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Id: {Id}, ");
            sb.Append($"Name: {Name}");
            sb.Append($"Price: {Price}");

            return sb.ToString();
        }
    }
}
