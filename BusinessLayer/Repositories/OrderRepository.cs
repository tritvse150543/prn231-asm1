using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class OrderRepository : GenericRepository<Order>
    {
        public OrderRepository(DatabaseContext context) : base(context)
        {
        }
    }
    public class OrderDetailRepository : GenericRepository<OrderDetail>
    {
        public OrderDetailRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
