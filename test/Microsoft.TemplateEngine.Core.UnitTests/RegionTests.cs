﻿using System.IO;
using System.Text;
using Microsoft.TemplateEngine.Abstractions.Engine;
using Microsoft.TemplateEngine.Utils;
using Xunit;

namespace Microsoft.TemplateEngine.Core.UnitTests
{
    public class RegionTests : TestBase
    {
        [Fact]
        public void VerifyRegionExclude()
        {
            string value = @"test value value x test foo bar";
            string expected = @"test  bar";

            byte[] valueBytes = Encoding.UTF8.GetBytes(value);
            MemoryStream input = new MemoryStream(valueBytes);
            MemoryStream output = new MemoryStream();

            IOperationProvider[] operations = { new Region("value", "foo", false, false, false, null) };
            EngineConfig cfg = new EngineConfig(VariableCollection.Environment(), "${0}$");
            IProcessor processor = Processor.Create(cfg, operations);

            //Changes should be made
            bool changed = processor.Run(input, output);
            Verify(Encoding.UTF8, output, changed, value, expected);
        }

        [Fact]
        public void VerifyRegionInclude()
        {
            string value = @"test value value x test foo bar";
            string expected = @"test   x test  bar";

            byte[] valueBytes = Encoding.UTF8.GetBytes(value);
            MemoryStream input = new MemoryStream(valueBytes);
            MemoryStream output = new MemoryStream();

            IOperationProvider[] operations = { new Region("value", "foo", true, false, false, null) };
            EngineConfig cfg = new EngineConfig(VariableCollection.Environment(), "${0}$");
            IProcessor processor = Processor.Create(cfg, operations);

            //Changes should be made
            bool changed = processor.Run(input, output);
            Verify(Encoding.UTF8, output, changed, value, expected);
        }

        [Fact]
        public void VerifyRegionStrayEnd()
        {
            string value = @"test foo value bar foo";
            string expected = @"test   bar ";

            byte[] valueBytes = Encoding.UTF8.GetBytes(value);
            MemoryStream input = new MemoryStream(valueBytes);
            MemoryStream output = new MemoryStream();

            IOperationProvider[] operations = { new Region("value", "foo", true, false, false, null) };
            EngineConfig cfg = new EngineConfig(VariableCollection.Environment(), "${0}$");
            IProcessor processor = Processor.Create(cfg, operations);

            //Changes should be made
            bool changed = processor.Run(input, output);
            Verify(Encoding.UTF8, output, changed, value, expected);
        }

        [Fact]
        public void VerifyRegionIncludeToggle()
        {
            string value = @"test region value x test region bar";
            string expected = @"test  value x test  bar";

            byte[] valueBytes = Encoding.UTF8.GetBytes(value);
            MemoryStream input = new MemoryStream(valueBytes);
            MemoryStream output = new MemoryStream();

            IOperationProvider[] operations = { new Region("region", "region", true, false, false, null) };
            EngineConfig cfg = new EngineConfig(VariableCollection.Environment(), "${0}$");
            IProcessor processor = Processor.Create(cfg, operations);

            //Changes should be made
            bool changed = processor.Run(input, output);
            Verify(Encoding.UTF8, output, changed, value, expected);
        }

        [Fact]
        public void VerifyRegionExcludeToggle()
        {
            string value = @"test region value x test region bar";
            string expected = @"test  bar";

            byte[] valueBytes = Encoding.UTF8.GetBytes(value);
            MemoryStream input = new MemoryStream(valueBytes);
            MemoryStream output = new MemoryStream();

            IOperationProvider[] operations = { new Region("region", "region", false, false, false, null) };
            EngineConfig cfg = new EngineConfig(VariableCollection.Environment(), "${0}$");
            IProcessor processor = Processor.Create(cfg, operations);

            //Changes should be made
            bool changed = processor.Run(input, output);
            Verify(Encoding.UTF8, output, changed, value, expected);
        }

        [Fact]
        public void VerifyRegionIncludeWhitespaceFixup()
        {
            string value = @"test value value x test foo bar";
            string expected = @"testx testbar";

            byte[] valueBytes = Encoding.UTF8.GetBytes(value);
            MemoryStream input = new MemoryStream(valueBytes);
            MemoryStream output = new MemoryStream();

            IOperationProvider[] operations = { new Region("value", "foo", true, false, true, null) };
            EngineConfig cfg = new EngineConfig(VariableCollection.Environment(), "${0}$");
            IProcessor processor = Processor.Create(cfg, operations);

            //Changes should be made
            bool changed = processor.Run(input, output);
            Verify(Encoding.UTF8, output, changed, value, expected);
        }

        [Fact]
        public void VerifyRegionIncludeWhitespaceFixup2()
        {
            string value = @"Hello
    #begin foo
value
    #end
There";
            string expected = @"Hello
foo
value
There";

            byte[] valueBytes = Encoding.UTF8.GetBytes(value);
            MemoryStream input = new MemoryStream(valueBytes);
            MemoryStream output = new MemoryStream();

            IOperationProvider[] operations = { new Region("#begin", "#end", true, false, true, null) };
            EngineConfig cfg = new EngineConfig(VariableCollection.Environment(), "${0}$");
            IProcessor processor = Processor.Create(cfg, operations);

            //Changes should be made
            bool changed = processor.Run(input, output);
            Verify(Encoding.UTF8, output, changed, value, expected);
        }

        [Fact]
        public void VerifyRegionIncludeWholeLine()
        {
            string value = @"Hello
    #begin foo
value
    #end
There";
            string expected = @"Hello
value
There";

            byte[] valueBytes = Encoding.UTF8.GetBytes(value);
            MemoryStream input = new MemoryStream(valueBytes);
            MemoryStream output = new MemoryStream();

            IOperationProvider[] operations = { new Region("#begin", "#end", true, true, true, null) };
            EngineConfig cfg = new EngineConfig(VariableCollection.Environment(), "${0}$");
            IProcessor processor = Processor.Create(cfg, operations);

            //Changes should be made
            bool changed = processor.Run(input, output);
            Verify(Encoding.UTF8, output, changed, value, expected);
        }

        [Fact]
        public void VerifyNoRegion()
        {
            string value = @"Hello
    #begin foo
value
    #end
There";
            string expected = @"Hello
    #begin foo
value
    #end
There";

            byte[] valueBytes = Encoding.UTF8.GetBytes(value);
            MemoryStream input = new MemoryStream(valueBytes);
            MemoryStream output = new MemoryStream();

            IOperationProvider[] operations = { new Region("#begin2", "#end2", true, true, true, null) };
            EngineConfig cfg = new EngineConfig(VariableCollection.Environment(), "${0}$");
            IProcessor processor = Processor.Create(cfg, operations);

            //Changes should be made
            bool changed = processor.Run(input, output);
            Verify(Encoding.UTF8, output, changed, value, expected);
        }

        [Fact]
        public void VerifyTornRegion1()
        {
            string value = @"Hello
    #begin foo
value
    #end
There";
            string expected = @"Hello
value
There";

            byte[] valueBytes = Encoding.UTF8.GetBytes(value);
            MemoryStream input = new MemoryStream(valueBytes);
            MemoryStream output = new MemoryStream();

            IOperationProvider[] operations = { new Region("#begin", "#end", true, true, true, null) };
            EngineConfig cfg = new EngineConfig(VariableCollection.Environment(), "${0}$");
            IProcessor processor = Processor.Create(cfg, operations);

            //Changes should be made
            bool changed = processor.Run(input, output, 14);
            Verify(Encoding.UTF8, output, changed, value, expected);
        }

        [Fact]
        public void VerifyTornRegion2()
        {
            string value = @"Hello
    #begin foo
value
    #end
There";
            string expected = @"Hello
value
There";

            byte[] valueBytes = Encoding.UTF8.GetBytes(value);
            MemoryStream input = new MemoryStream(valueBytes);
            MemoryStream output = new MemoryStream();

            IOperationProvider[] operations = { new Region("#begin", "#end", true, true, true, null) };
            EngineConfig cfg = new EngineConfig(VariableCollection.Environment(), "${0}$");
            IProcessor processor = Processor.Create(cfg, operations);

            //Changes should be made
            bool changed = processor.Run(input, output, 36);
            Verify(Encoding.UTF8, output, changed, value, expected);
        }

        [Fact]
        public void VerifyTornRegion3()
        {
            string value = @"Hello
    #begin foo
value
    #end
There";
            string expected = @"Hello
value
There";

            byte[] valueBytes = Encoding.UTF8.GetBytes(value);
            MemoryStream input = new MemoryStream(valueBytes);
            MemoryStream output = new MemoryStream();

            IOperationProvider[] operations = { new Region("#begin", "#end", true, true, true, null) };
            EngineConfig cfg = new EngineConfig(VariableCollection.Environment(), "${0}$");
            IProcessor processor = Processor.Create(cfg, operations);

            //Changes should be made
            bool changed = processor.Run(input, output, 28);
            Verify(Encoding.UTF8, output, changed, value, expected);
        }

        [Fact]
        public void VerifyTinyPageRegion()
        {
            string value = @"Hello
    #begin foo
value
    #end
There";
            string expected = @"Hello
value
There";

            byte[] valueBytes = Encoding.UTF8.GetBytes(value);
            MemoryStream input = new MemoryStream(valueBytes);
            MemoryStream output = new MemoryStream();

            IOperationProvider[] operations = { new Region("#begin", "#end", true, true, true, null) };
            EngineConfig cfg = new EngineConfig(VariableCollection.Environment(), "${0}$");
            IProcessor processor = Processor.Create(cfg, operations);

            //Changes should be made
            bool changed = processor.Run(input, output, 1);
            Verify(Encoding.UTF8, output, changed, value, expected);
        }

        [Fact]
        public void VerifyTornPageInCloseSeekRegion()
        {
            string value = @"Hello
    #begin foo
value
value
value
value
value
value
value
value
    #end
There";
            string expected = @"Hello
There";

            byte[] valueBytes = Encoding.UTF8.GetBytes(value);
            MemoryStream input = new MemoryStream(valueBytes);
            MemoryStream output = new MemoryStream();

            IOperationProvider[] operations = { new Region("#begin", "#end", false, true, true, null) };
            EngineConfig cfg = new EngineConfig(VariableCollection.Environment(), "${0}$");
            IProcessor processor = Processor.Create(cfg, operations);

            //Changes should be made
            bool changed = processor.Run(input, output, 28);
            Verify(Encoding.UTF8, output, changed, value, expected);
        }
    }
}
