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
        public List<bool[][]> BlockPrototypes { get; private set; }
        public Random Random { get; private set; }
        public static BlockLoader instance;

        public static bool[][] StructureTemplate;

        private BlockLoader()
        {
            BlockPrototypes = new List<bool[][]>();

            StructureTemplate = new bool[1][];
            StructureTemplate[0] = new bool[1];
            StructureTemplate[0][0] = true;

            Random = new Random();
            LoadBlocks();
        }

        private void LoadBlocks()
        {
            try
            {
                using (StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("CSAcademyProject.block_types.txt")))
                {
                    while (reader.EndOfStream == false)
                    {
                        String line = reader.ReadLine();
                        if (line.Equals(""))
                            continue;

                        int sizeX, sizeY;
                        sizeX = Convert.ToInt32(line.Split(' ')[0]);
                        sizeY = Convert.ToInt32(line.Split(' ')[1]);

                        //DrawableBlock newBlock = new DrawableBlock(sizeX, sizeY, Color.FromRgb(0, 0, 0));
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

        public DrawableBlock GetRandomBlock(int sizeX, int sizeY,int margin)
        {
            if (BlockPrototypes.Count > 0)
            {
                int index = Random.Next(BlockPrototypes.Count);
                int transformation = Random.Next(4);
                return new DrawableBlock(TransformBlockStructure(BlockPrototypes[index], transformation), sizeX, sizeY,
                    margin, ColorLoader.Instance.GetRandomColor());
            }
            else
                return new DrawableBlock(StructureTemplate, sizeX, sizeY, margin, ColorLoader.Instance.GetRandomColor());
        }


        private bool[][] TransformBlockStructure(bool[][] structure,int numberOfRotations)
        {
            //TO BE CHANGED
            //NAIVE APPROACH

                
            return structure;
        }

    }
}