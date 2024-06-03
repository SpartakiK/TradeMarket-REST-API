using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        protected BaseEntity()
        {
            Id = 0;
        }
        protected BaseEntity(int id)
        {
            Id = id;
        }
    }
}
