using System;
using Xfsm.Core;
using Xfsm.Core.Enums;
using Xfsm.Core.Interfaces;

namespace Xfsm.SqlServer.Builders
{
    public static class XfsmProcessorBuilder
    {
        public static XfsmProcessor<T> Build<T>(string connectionString, XfsmPeekMode mode, IXfsmState<T> state)
        {
            var provider = new XfsmDatabaseProvider(connectionString);
            var bag = new XfsmBag<T>(provider, mode);
            return new XfsmProcessor<T>(bag, state);
        }
    }
}
