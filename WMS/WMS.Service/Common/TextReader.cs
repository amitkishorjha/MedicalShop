using System;
using System.Collections.Generic;

namespace WMS.Service.Common
{
    public class TextReader
    {
        private string path;

        public TextReader(string filePath)
        {
            this.path = filePath;
        }

        public List<string> GetLines()
        {
            System.IO.StreamReader file = null;

            string line;
            List<string> lines = new List<string>();

            try
            {
                file = new System.IO.StreamReader(this.path);

                while ((line = file.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                file.Close();
            }

            return lines;
        }
    }
}
