﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4600Project
{
    /// <summary>
    /// The following json properties are used to access the result ids of Twitter
    /// </summary>
    public class ResultIds
    {
        private long[] _ids;

        [JsonProperty("ids")]
        public long[] Ids
        {
            get { return _ids ?? new long[0]; }
            set { _ids = value; }
        }
    }
}