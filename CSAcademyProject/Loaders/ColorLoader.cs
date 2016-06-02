using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Reflection;

namespace CSAcademyProject.Loaders
{
    class ColorLoader
    {
        private static ColorLoader instance;

        private List<Color> Colors { get;  set; }
        private Random Rand { get;  set; }
        public Color White { get; private set; }
        public Color Black { get; private set; }

        private ColorLoader()
        {
            Colors = new List<Color>();
            Rand = new Random();
            LoadColors();
        }

        private void LoadColors()
        {
            White = Color.FromRgb(255, 255, 255);
            Black = Color.FromRgb(0, 0, 0);

            try
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(WindowParameters.COLORS_STREAM))
                using (StreamReader reader = new StreamReader(stream))
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
                return Colors[Rand.Next(Colors.Count)];
            else
                return Black;
        }
    }
}
