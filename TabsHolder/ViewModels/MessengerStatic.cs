using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabsHolder
{
    public static class MessengerStatic
    {
        public static event Action<object> TabItemAdded;

        public static void NotifyAboutTabItemAdding(object newTabItemData)
            => TabItemAdded?.Invoke(newTabItemData);



        public static event Action<object> TabItemAddedByEnter;

        public static void NotifyAboutTabItemAddingByEnter(object data=null)
            => TabItemAddedByEnter?.Invoke(data);


        public static event Action<object> TabItemEditedByEnter;

        public static void NotifyAboutTabItemEditedByEnter(object data = null)
            => TabItemEditedByEnter?.Invoke(data);



        public static event Action<object> AddTabWindowClosed;

        public static void NotifyAddTabWinClosing(object data)
        {
            AddTabWindowClosed?.Invoke(data);
        }

        public static event Action<object> AddTabWindowOpened;

        public static void NotifyAddTabWindowOpenning(object data=null)
        {
            AddTabWindowOpened?.Invoke(data);
        }


        public static event Action<object> RenameTabWindowOpened;
        internal static void NotifyRenameTabWindowOpenning(object selectedItem)
        {
            RenameTabWindowOpened?.Invoke(selectedItem);
        }

        public static event Action<object> RenameTabWindowClosed;
        internal static void NotifyRenameTabWindowClosing(object data=null)
        {
            RenameTabWindowClosed?.Invoke(data);
        }


        public static event Action<object> TabItemNameChanged;
        internal static void NotifyTabItemNameChanging(object selectedItem)
        {
            TabItemNameChanged?.Invoke(selectedItem);
        }


        public static event Action<object> SessionOverwrited;
        internal static void NotifySessionOverwriting(object data)
        {
            SessionOverwrited?.Invoke(null);
        }


        public static event Action<object> SessionCreated;
        internal static void NotifySessionCreating(object fileName, EventArgs e)
        {
            SessionCreated?.Invoke(fileName);
        }

        public static event Action<object> LastSessionSelected;

        public static void NotifyLastSessionSelecting(object session)
            => LastSessionSelected?.Invoke(session);


        public static event Action<object> BrowseFolderPathClicked;

        public static void NotifyBrowseFolderPathOpen()
            => BrowseFolderPathClicked?.Invoke(null);


        public static event Action<object> SettingsWindowClosed;

        public static void NotifySettingsWindowClosing(object viewBag)
            => SettingsWindowClosed?.Invoke(viewBag);

    }
}

