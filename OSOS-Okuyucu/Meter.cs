
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Meter 
    {
        public Guid Id { get; private set; }
        public Guid ModelId { get;  set; }
        public Guid ReadingTimeId { get; set; }

        public int SerialNo { get; set; }

    }
}
