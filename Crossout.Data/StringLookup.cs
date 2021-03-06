﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crossout.Data
{
    public class StringLookup
    {
        public StringLookup()
        {
            stringsRegex = new Regex(stringsPattern, RegexOptions.IgnoreCase);
        }

        public Dictionary<string, string> Items { get; } = new Dictionary<string, string>();

        string stringsPattern = "\"(?<name>.+)\"\t\"(?<value>.+)\"";
        private Regex stringsRegex;
        
        public void ReadStats(string file)
        {
            Items.Clear();
            using (StreamReader sr = new StreamReader(file))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    AddLine(line);
                }
            }
        }

        private void AddLine(string line)
        {
            var match = stringsRegex.Match(line);
            if (match.Success)
            {
                if (match.Groups["name"].Success && match.Groups["value"].Success)
                {
                    var name = match.Groups["name"].Value;
                    var value = match.Groups["value"].Value;

                    if (!Items.ContainsKey(name))
                    {
                        Items[name.ToLowerInvariant()] = value;
                    }
                }
            }
        }

        public string ReadDescription(string key)
        {
            const string keyExtension = "_desc";
            var fullKey = key.ToLowerInvariant() + keyExtension;
            if (Items.ContainsKey(fullKey))
            {
                return Items[fullKey];
            }
            return null;
        }
    }
}
