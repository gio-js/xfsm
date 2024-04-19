using System;
using System.Collections.Generic;
using System.Text;

namespace Xfsm.Core.Interfaces
{
    /// <summary>
    /// The provider in charge of creating the required database connection
    /// Derived class will be specialized for the required database engines
    /// </summary>
    public interface IXfsmDatabaseProvider
    {
        /// <summary>
        /// Creates the database connection
        /// </summary>
        /// <returns></returns>
        IXfsmDatabaseConnection OpenConnection();
    }
}
