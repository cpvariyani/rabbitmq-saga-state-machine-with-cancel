using order_ms.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace order_ms.Infra
{
    public class OrderDataAccess : IOrderDataAccess
    {
        public bool DeleteOrder(Guid orderId)
        {
            using (var context = new OrderDbContext())
            {
                OrderModel order = context.OrderData.Where(x => x.OrderId == orderId).FirstOrDefault();

                if (order != null)
                {
                    context.Remove(order);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<OrderModel> GetAllOrder()
        {
            using (var context = new OrderDbContext())
            {
                return context.OrderData.ToList();
            }
        }

        public OrderModel GetOrder(Guid orderId)
        {
            using (var context = new OrderDbContext())
            {
                return context.OrderData.Where(x => x.OrderId == orderId).FirstOrDefault();
            }
        }

        public void SaveOrder(OrderModel order)
        {
            using (var context = new OrderDbContext())
            {
                context.Add<OrderModel>(order);
                context.SaveChanges();
            }
        }
    }
}
