using CSAcademyProject.Drawables;
using CSAcademyProject.Loaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Reflection;

namespace CSAcademyProject
{
    class BlockLoader
    {
        private List<bool[][]> BlockPrototypes { get; set; }
        private Random Random { get; set; }
        public static BlockLoader instance;

        private BlockLoader()
        {
            BlockPrototypes = new List<bool[][]>();
            Random = new Random();
            AddStructureTemplate();
            LoadBlocks();
        }

        private void AddStructureTemplate()
        {
            bool[][] structureTemplate = new bool[1][];
            structureTemplate[0] = new bool[1];
            structureTemplate[0][0] = true;
            BlockPrototypes.Add(structureTemplate);
        }

        private void LoadBlocks()
        {
            try
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("CSAcademyProject.Resources.block_types.txt"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    while (reader.EndOfStream == false)
                    {
                        String line = reader.ReadLine();
                        if (line.Equals(""))
                            continue;

                        int sizeX, sizeY;
                        sizeX = Convert.ToInt32(line.Split(' ')[0]);
                        sizeY = Convert.ToInt32(line.Split(' ')[1]);

                        bool[][] blockStructure = new bool[sizeY][];
                        for (int i = 0; i < sizeY; i++)
                            blockStructure[i] = new bool[sizeX];


                        for (int i = 0; i < sizeY; i++)
                        {
                            line = reader.ReadLine();
                            String[] parts = line.Split();
                            for (int j = 0; j < parts.Length; j++)
                            {
                                if (parts[j].Equals("1"))
                                    blockStructure[i][j] = true;
                            }
                        }
                        BlockPrototypes.Add(blockStructure);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error occured during block loading");
            }
        }


        public static BlockLoader Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BlockLoader();
                }
                return instance;
            }
        }

        public DrawableBlock GetRandomBlock(int sizeX, int sizeY, int margin)
        {
            int index = Random.Next(BlockPrototypes.Count);
            return new DrawableBlock(BlockPrototypes[index], sizeX, sizeY,
                margin, ColorLoader.Instance.GetRandomColor());
        }

    }
}