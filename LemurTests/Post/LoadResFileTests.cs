using Xunit;

namespace Lemur.Post.Tests
{
    public class FileParserTests
    {
        [Fact]
        public void FileParserTest()
        {
            var fileParser = new ResFileParser();
            fileParser.Parse("TestFile/Result/lemur.res.0.1");
            Dictionary<int, Dictionary<string, double[]>> nodeData = fileParser.GetNodeDataByLabel();
            Dictionary<int, Dictionary<string, double[]>> elemData = fileParser.GetElemDataByLabel();
        }
    }
}
