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
            var nodeData = fileParser.GetNodeDataByLabel();
            var elemData = fileParser.GetElemDataByLabel();
        }
    }
}
