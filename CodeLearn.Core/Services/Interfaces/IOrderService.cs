using CodeLearn.DataLayer.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLearn.Core.Services.Interfaces
{
    public interface IOrderService
    {
        int? AddOrder(string userName, int courseId);
        void UpdatePriceOrder(int orderId);
        Order GetOrderForUserPanel(string userName, int orderId);
    }
}
