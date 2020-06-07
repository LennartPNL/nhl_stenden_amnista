using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amnista.Views.Data
{
    class ControlInfoDataItem
    {
        public ControlInfoDataItem(Object pageType, string title = null)
        {
            PageType = pageType;
            Title = title;
        }

        public string Title { get; }

        public Object PageType { get; }

        public override string ToString()
        {
            return Title;
        }
    }
}
