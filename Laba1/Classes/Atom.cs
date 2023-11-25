using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba1.Classes
{
    public class Atom
    {
        public readonly int Value = 1;
        public int Position { get; set; }

        public Atom(int position)
        {
            Position = position;
        }
    }

   
}
