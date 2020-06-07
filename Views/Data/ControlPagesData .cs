using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amnista.Views.Data
{
    class ControlPagesData : List<ControlInfoDataItem>
    {
        public ControlPagesData(){
            AddPage(new ClientProfileView(), "Profile");
            AddPage(new SettingsView(), "Settings");
            AddPage(new CoffeeMachineModeView(), "Server Mode");
        
        }

            private void AddPage(Object pageType, string displayName = null)
        {
            Add(new ControlInfoDataItem(pageType, displayName));
        }

    }
}
