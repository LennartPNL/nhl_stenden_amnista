using System;

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