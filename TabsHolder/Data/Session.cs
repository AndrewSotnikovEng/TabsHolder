﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabsHolder.Data
{
    public class Session : Config
    {
        public ObservableCollection<TabItem> TabItems;
    }
}