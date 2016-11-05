﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Common
{
    public class Page<T>
    {
      
            /// <summary>
            ///     The current page number contained in this page of result set
            /// </summary>
            public long CurrentPage { get; set; }

            /// <summary>
            ///     The total number of pages in the full result set
            /// </summary>
            public long TotalPages { get; set; }

            /// <summary>
            ///     The total number of records in the full result set
            /// </summary>
            public long TotalItems { get; set; }

            /// <summary>
            ///     The number of items per page
            /// </summary>
            public long ItemsPerPage { get; set; }

            /// <summary>
            ///     The actual records on this page
            /// </summary>
            public List<T> Items { get; set; }

            /// <summary>
            ///     User property to hold anything.
            /// </summary>
            public object Context { get; set; }
    }
}
