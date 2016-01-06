using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmpleoDotNet.Models
{
    /// <summary>
    /// Define all types of services implemented in the
    /// app
    /// </summary>
    public enum ServiceType
    {
        /// <summary>
        /// Entity framework
        /// </summary>
        EF,

        /// <summary>
        /// Oracle 
        /// </summary>
        Oracle
    }
}