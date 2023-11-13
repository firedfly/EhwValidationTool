using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EhwValidationTool
{
    public class ProgramOptions
    {
        [Option("install", Required = false, HelpText = "Internal Use Only")]
        public bool Install { get; set; }

        [Option("cleanup", Required =false, HelpText ="Internal Use Only")]
        public bool Cleanup { get; set; }
    }
}
