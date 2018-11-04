using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DayZLootEdit
{
    class LootTable
    {
        public string FilePath { get; private set; }
        public XElement XML { get; private set; }

        public ObservableCollection<LootType> Loot { get; } = new ObservableCollection<LootType>();


        public LootTable(string file)
        {
            FilePath = file;
        }

        public void LoadFile()
        {
            XML = XElement.Load(FilePath);
            process();
        }

        public void SaveFile()
        {
            FileInfo file = new FileInfo(FilePath);

            if (file.Exists)
            {
                FileInfo backup = new FileInfo(file.FullName + ".original.xml");

                if (!backup.Exists)
                {
                    file.CopyTo(backup.FullName);
                }
            }

            XML.Save(FilePath);
        }

        private void process()
        {
            Loot.Clear();

            foreach(XElement node in XML.Elements())
            {
                Loot.Add(new LootType(node));
            }
        }

    }
}
