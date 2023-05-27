using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfMeterDal 
    {
        public Meter Get(Expression<Func<Meter, bool>> filter)
        {
            using (var context = new MeterDbContext())
            {
                return context.Set<Meter>().SingleOrDefault(filter);
            }
        }

        public List<Meter> GetAll(Expression<Func<Meter, bool>> filter = null)
        {
            using (var context = new MeterDbContext())
            {
                return filter == null ? context.Set<Meter>().ToList() : context.Set<Meter>().Where(filter).ToList();
            }
        }
    }
}
