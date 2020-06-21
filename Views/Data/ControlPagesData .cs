using System;
using System.Collections.Generic;
using Amnista.Models;

namespace Amnista.Views.Data
{
    class ControlPagesData : List<ControlInfoDataItem>
    {
        public ClientSocket ClientSocket { get; set; }

        public ControlPagesData()
        {
            AddPage(new CoffeeVoteView(), "Home");
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