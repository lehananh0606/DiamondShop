﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Exeptions
{
    public class OptimisticException : Exception
    {
        public OptimisticException(string message) : base(message)
        {

        }
    }

}
