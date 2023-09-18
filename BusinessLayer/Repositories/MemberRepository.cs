using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class MemberRepository : GenericRepository<Member>
    {
        public MemberRepository(DbContext context) : base(context)
        {
        }
    }
}
