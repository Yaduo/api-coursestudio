using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace CourseStudio.Lib.Utilities
{
    public static class TransactionScopeHelper
    {
        public static TransactionScope CreateNewTransactionScope()
        {
            return new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled
            );
        }

    }
}
