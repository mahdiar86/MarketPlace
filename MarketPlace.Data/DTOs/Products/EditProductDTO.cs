﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Data.DTOs.Products
{
    public class EditProductDTO : CreateProductDTO
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string ImageName { get; set; }
    }
}
