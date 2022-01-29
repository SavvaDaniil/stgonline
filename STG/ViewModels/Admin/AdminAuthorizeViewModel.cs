using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels.Admin
{
    public class AdminAuthorizeViewModel
    {
        public string menuActive { get; set; }
        public int level { get; set; }
        public List<string> listOfAvailablePanels { get; set; }
        public int countUnreadPackeChatsAndHomeworks { get; set; }
        public int countInactivatedStatements { get; set; }

        public AdminAuthorizeViewModel(string menuActive, int level, List<string> listOfAvailablePanels, int countUnreadPackeChatsAndHomeworks, int countInactivatedStatements)
        {
            this.menuActive = menuActive;
            this.level = level;
            this.listOfAvailablePanels = listOfAvailablePanels;
            this.countUnreadPackeChatsAndHomeworks = countUnreadPackeChatsAndHomeworks;
            this.countInactivatedStatements = countInactivatedStatements;
        }
    }
}
