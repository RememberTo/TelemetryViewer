﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelemetryViewer.Models
{
    public class FrameModel
    {
        public ushort[]? Headers { get; set; }
        public ushort[]? Body { get; set; }
    }
}
