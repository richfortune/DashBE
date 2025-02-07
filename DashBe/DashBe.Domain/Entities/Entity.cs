using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBe.Domain.Entities
{
    public abstract class Entity<TPrimaryKey>
    {
        public TPrimaryKey Id { get; private set; }

        protected Entity(TPrimaryKey id) 
        {
            Id = id;
        }
    }
}
