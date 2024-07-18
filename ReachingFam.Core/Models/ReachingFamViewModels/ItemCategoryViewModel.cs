﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models.ReachingFamViewModels
{
    public class ItemCategoryViewModel
    {
        public int ItemCategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
