using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Soneta.Business;

namespace Soneta.ObecnoscDb.Config
{
    public class ConfigCompiler : Business.BusinessCompiler
    {
        public string ClassName { get; set; }
        IConfigCompiler Config;
        public ConfigCompiler(IConfigCompiler cfgCompiler, string className) :
            base(cfgCompiler)
        {
            this.Config = cfgCompiler;
            this.ClassName = className;
        }

        protected override void AddFooter(int idx, TextWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void AddHeader(int idx, TextWriter writer)
        {
            this.AddSystemUsing(idx, writer);
            this.AddSonetaUsing(idx, writer);
            writer.WriteLine("using System.Drawing;");

            writer.WriteLine("public class " + ClassName + " {");

            writer.WriteLine(Config.Code);
        }

        protected override IEnumerable GetObjects(int idx)
        {
            return new IConfigCompiler[1] { this.Config };
        }
    }

    public interface IConfigCompiler : ISessionable
    {
        MemoText Code { get; set; }
    }
}
