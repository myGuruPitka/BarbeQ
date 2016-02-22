using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarbeQ.Examples
{
    class CommandLineOptions
    {
        [Option('p', "produce", DefaultValue = false, HelpText = "Start producing messages")]
        public bool Produce { get; set; }

        [Option('c', "consume", DefaultValue = false, HelpText = "Start consuming messages")]
        public bool Consume { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage() {
            return HelpText.AutoBuild(this,
                (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
