using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
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

        public void addTable()
        {
            addNode();
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

        private void addNode()
        {
            var typeTreeElement = new XElement("type", new XAttribute("name", "classname"),
                    new XElement("nominal", 10),
                    new XElement("lifetime", 14400),
                    new XElement("restock", 0),
                    new XElement("min", 20),
                    new XElement("quantmin", -1),
                    new XElement("quantmax", -1),
                    new XElement("cost", 100),
                    new XElement("flags", new XAttribute("count_in_cargo", 0), new XAttribute("count_in_hoarder", 0), new XAttribute("count_in_map", 1), new XAttribute("count_in_player", 0), new XAttribute("crafted", 0), new XAttribute("deloot", 0)),
                    new XElement("category", new XAttribute("name", "category type")),
                    new XElement("tag", new XAttribute("name", "shelves")),
                    new XElement("usage", new XAttribute("name", "Town"))
                    );
            XML.Add(typeTreeElement);
        }

    }
}
