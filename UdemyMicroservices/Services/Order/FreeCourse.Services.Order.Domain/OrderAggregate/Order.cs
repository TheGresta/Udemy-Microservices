using FreeCourse.Services.Order.Core;

namespace FreeCourse.Services.Order.Domain.OrderAggregate
{
    public class Order : Entity, IAggregateRoot
    {
        public DateTime CreatedDate { get; private set; }
        public Address Address { get; private set; }
        public string BuyerId { get; private set; }
        private readonly List<OrderItem> _orderItem;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItem;

        public Order()
        {
        }
        public Order(Address address, string buyerId)
        {
            _orderItem = new List<OrderItem>();
            CreatedDate = DateTime.Now;
            Address = address;
            BuyerId = buyerId;
        }

        public void AddOrderItem(string productId, string productName, decimal price, string pictureUrl)
        {
            var existProduct = _orderItem.Any(x => x.ProductId == productId);

            if (!existProduct)
            {
                var newOrderItem = new OrderItem(productId, productName, pictureUrl, price);
                _orderItem.Add(newOrderItem);
            }
        }

        public decimal GetTotalPrice => _orderItem.Sum(x => x.Price);
    }
}
