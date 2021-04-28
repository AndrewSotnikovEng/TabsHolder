using System;

namespace Installer
{
    public static class MessengerStatic
    {
        public static event Action<object> OutputFolderNotEmpty;

        public static void NotifyAboutOutputFolderFilled(object outputFolder)
            => OutputFolderNotEmpty?.Invoke(outputFolder);

        
    }
}

