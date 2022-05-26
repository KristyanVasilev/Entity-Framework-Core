﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst.Models
{
    public class Category
    {
        public Category()
        {
            this.News = new HashSet<News>();
        }
        public int Id { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        public virtual ICollection<News> News { get; set; }
    }
}
