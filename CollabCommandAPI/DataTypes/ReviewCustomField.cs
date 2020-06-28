using System;
using System.Collections.Generic;
using System.Text;

namespace CollabCommandAPI
{
    public struct ReviewCustomField
    {
        // Project, (TI) Units (Loc/Pages), (TI) Outside Time (minutes), Overview
        public string Title { get; set; }
        public string Value { get; set; }
    }
}
