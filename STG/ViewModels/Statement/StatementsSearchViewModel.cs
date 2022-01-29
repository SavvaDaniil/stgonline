using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Statement
{
    public class StatementsSearchViewModel
    {
        public List<StatementPreviewViewModel> statements { get; set; }
        public int count { get; set; }

        public StatementsSearchViewModel(List<StatementPreviewViewModel> statements, int count)
        {
            this.statements = statements;
            this.count = count;
        }
    }
}
