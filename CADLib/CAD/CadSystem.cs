using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace CAD
{
    public class CadSystem
    {
        //Datasets
        //Create commandHistory object
        public CommandHistory commandHistory = new CommandHistory();
        public CadSystem.ClickCache clickCache = new CadSystem.ClickCache();
        public GridSystem gridSystem;
        public Shape SelectedShape;
        public PointF CurrentPosition = new PointF(0, 0);
        public bool RelativePositioning = false;
        public Dictionary<string, Action<List<float>>> drawingFunctions = new Dictionary<string, Action<List<float>>>();
        public Dictionary<string, Action> gridFunctions = new Dictionary<string, Action>();
        public bool InProcess = false;
        
        public CadSystem(PointF origin, SizeF containerSize, float scale, int dpi)
        {
            this.gridSystem = new GridSystem(origin, containerSize, scale, dpi);
            ShapeSystem.InitSystem();
            ShapeSystem.SetGrid(gridSystem);
            //SET FUNCTIONS
            this.drawingFunctions.Add("L", Line.AddLine);
            this.drawingFunctions.Add("RT", Rect.AddRect);
            this.drawingFunctions.Add("E", Ellipse.AddEllipse);
            this.drawingFunctions.Add("C", gridSystem.SetCursor);
            this.drawingFunctions.Add("P", cadPoint.AddPoint);
            this.drawingFunctions.Add("AJD", LinearDimension.AdjDim);
            this.drawingFunctions.Add("DIM", LinearDimension.AddNewDim);
            this.drawingFunctions.Add("FILL", ShapeSystem.SetShapeFillColor);
            Action PosToggle =
                () => gridSystem.TogglePositioning();
            Action DimActiveLine =
                () => ShapeSystem.DimensionActiveLine();
            this.gridFunctions.Add("R", PosToggle);
            this.gridFunctions.Add("D", DimActiveLine);
        }

        public bool SerializeAndSaveShapes(string fileName)
        {
                Snapshot savefile = new Snapshot();
                SerializeToFile(savefile, fileName);
                Console.WriteLine("Shape system saved successfully to \"{0}\".", fileName);
                return true;
        }

        public bool DeSerializeAndLoadShapes(string fileName)
        {
            try
            {
                Snapshot snapshot = DeserializeFromFile<Snapshot>(fileName);
                ShapeSystem.ClearData();
                snapshot.Load();
                ShapeSystem.UpdateSnapPoints();
                Console.WriteLine("Shape system \"{0}\" loaded.", fileName);
            }
            catch(Exception e)
            {
                Console.WriteLine("An error occoured: {0}", e.Message);
            }
            return false;
        }

        private static T DeserializeFromFile<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(T); }
            
            byte[] b = File.ReadAllBytes(fileName);
            using (var stream = new MemoryStream(b))
            {
                var formatter = new BinaryFormatter();
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        private static void SerializeToFile<T>(T obj, string fileName)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Flush();
                stream.Position = 0;
                File.WriteAllBytes(fileName,stream.ToArray());
            }
        }

        public void ParseInput(string text)
        {
            //Split Input
            IO.parsedObj result = IO.parsing.parseString(text);
            if (result == null)
            {
                return;
            }
            //Switch first Letter
            if (drawingFunctions.ContainsKey(result.type))
                drawingFunctions[result.type].Invoke(result.value);
            else if (gridFunctions.ContainsKey(result.type))
                gridFunctions[result.type].Invoke();
            else
                Console.WriteLine("Command not recognized");
        }

        public class IO
        {

            public static class parsing
            {
                public static parsedObj parseString(string input, char delim = ' ')
                {
                    parsedObj retObj = new parsedObj();

                    if (Regex.IsMatch(input, @"^[a-zA-Z]+(\s[0-9.-]*)*?$")) // if starts with a string
                    {
                        input = Regex.Replace(input, @"\s+", @" ");
                        string[] text_array = input.Trim().Split(delim);
                        retObj.type = text_array[0].ToUpper();
                        if (text_array.Length > 1)
                        {
                            for (int i = 1; i < text_array.Length; i++)
                            {
                                retObj.value.Add(float.Parse(text_array[i], CultureInfo.InvariantCulture.NumberFormat));
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Input was not in correct format. Example: 'L 1.5 1 2 2' to draw a line from (1.5,1) to (2,2) while in global positioning.");
                        return null;
                    }
                    return retObj;
                }
            }

            public class parsedObj
            {
                public string type = null;
                public List<float> value = new List<float>();
            }
        }

        public class ClickCache
        {
            private Queue<PointF> cache = new Queue<PointF>();
            public PointF dequeue()
            {
                return cache.Dequeue();
            }
            public int enqueue(PointF point)
            {
                cache.Enqueue(point);
                return cache.Count();
            }
            public int count()
            {
                return cache.Count();
            }
            public int clear()
            {
                int numItems = cache.Count();
                if ( numItems > 0)
                {
                    cache.Clear();
                }
                return numItems;
            }
            public PointF peek()
            {
                return cache.Peek();
            }
        }
    }
    
}
