﻿using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ProductRepository : GenericRepository<Product>
    {
        public ProductRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
