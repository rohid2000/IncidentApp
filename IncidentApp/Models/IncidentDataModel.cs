﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentApp.Models
{
    public class IncidentDataModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } = "Gemeld";
        public string Priority { get; set; }
        public int UserId { get; set; }
    }
}
