using Entities.Concrete;
using System.Linq.Expressions;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfReadingTimeDal
    {
        public ReadingTime Get(Expression<Func<ReadingTime, bool>> filter)
        {
            using (var context = new MeterDbContext())
            {
                return context.Set<ReadingTime>().SingleOrDefault(filter);
            }
        }

        public List<ReadingTime> GetAll(Expression<Func<ReadingTime, bool>> filter = null)
        {
            using (var context = new MeterDbContext())
            {
                return filter == null ? context.Set<ReadingTime>().ToList() : context.Set<ReadingTime>().Where(filter).ToList();
            }
        }
    }
}
