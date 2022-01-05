using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laapi.Models
{
    public class Logs
    {
        public LogResultTable[] Tables { get; set; }

        public void Dump()
        {
            foreach(var table in Tables)
            {
                table.Dump();
            }
        }
    }

    public class LogResultTable
    {
        public string Name { get; set; }
        public LogResultColumn[] Columns { get; set; }
        public object[][] Rows { get; set; }

        internal void Dump()
        {
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($" Table: {this.Name} ");

            var MAX_COL = 8;
            var columnCount = 0;
            var MAX_ROW = 20;
            var rowCount = 0;

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            columnCount = 0;
            foreach (var col in Columns)
            {
                if (++columnCount > MAX_COL)
                {
                    Console.WriteLine();
                    break;
                }
                Console.Write($"   {col.Name.PadRight(30).Substring(0, 20)}");
            }

            if (columnCount <= MAX_COL) { Console.WriteLine(); }

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            foreach (var row in Rows)
            {
                if(++rowCount > MAX_ROW)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{(Rows.Length - MAX_ROW)} more rows are not shown...");
                    break;
                }
                columnCount = 0;
                foreach (var value in row)
                {
                    if (++columnCount > MAX_COL)
                    {
                        Console.WriteLine();
                        break;
                    }
                    Console.Write($"   {(value + "").PadRight(30).Substring(0, 20)}");
                }
                if (columnCount <= MAX_COL) { Console.WriteLine(); }
            }
            if (rowCount <= MAX_ROW) { Console.WriteLine(); }
            Console.ResetColor();
        }
    }

    public class LogResultColumn
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
