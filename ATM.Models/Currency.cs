﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Models
{
    [Serializable]
    public class Currency
    {
        public string Name { get; set; }
        public double ExchangeRate { get; set; }
    }
}