﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBe.Domain.Entities
{
    public class Entity<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }
    }
}
