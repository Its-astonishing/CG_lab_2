using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CG_lab_2
{
    class Bin
    {
        FileStream file_stream;

        public int x, y, z;
        public int[] array;

        public Bin()
        {

        }

        public Bin(string path)
        {
            readBIN(path);
        }

        public void readBIN(string path)
        {
            if (File.Exists(path))
            {
                FileStream file_stream = new FileStream(path, FileMode.Open);
                BinaryReader reader = new BinaryReader(file_stream);

                x = reader.ReadInt32();
                y = reader.ReadInt32();
                z = reader.ReadInt32();

                array = new int[x * y * z];

                for (uint i = 0u; i < array.Length; ++i)
                {
                    array[i] = (int)reader.ReadInt16();
                }
            }
        }
    }
}
