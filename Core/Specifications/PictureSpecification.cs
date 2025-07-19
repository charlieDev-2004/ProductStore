using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Specifications
{
    public class PictureSpecification : BaseSpecification<Picture>
    {
        public PictureSpecification(int id) : base(p => p.Id == id)
        {
        }
    }
}