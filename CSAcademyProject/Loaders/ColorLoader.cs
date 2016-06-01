using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Reflection;

namespace CSAcademyProject.Loaders
{
    class ColorLoader
    {
        public List<Color> Colors { get; private set; }
        public Random Rand{ get; private set; }
        private static ColorLoader instance;

        private ColorLoader()
        {
            Colors = new List<Color>();
            Rand = new Random();
            LoadColors();
        }

        private void LoadColors()
        {

            try
            {
                using (StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("CSAcademyProject.colors.txt")))
                {
                    while (reader.EndOfStream == false)
                    {
                        String colorValue = reader.ReadLine().Split(' ')[0];
                        Colors.Add((Color)ColorConverter.ConvertFromString(colorValue));
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error during loadnig colors");
            }
            
        }

        public static ColorLoader Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ColorLoader();
                }
                return instance;
            }
        }

        public Color GetRandomColor()
        {
            if (Colors.Count > 0)
            {
                int colorIndex = Rand.Next(Colors.Count);
                return Colors[colorIndex];
            }
            else
                return Color.FromRgb(0, 0, 0);
        }
    }
}
