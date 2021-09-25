using System.Text.RegularExpressions;


namespace GcodeInterpreter
{
    public interface ICommentRemover
    {
        string RemoveComment(string line);
    }

    public class AfterSemiColonCommentRemover : ICommentRemover
    {
        public string RemoveComment(string line) =>
            Regex.Replace(line, ".*(;.*)$", string.Empty);
    }

    public class InPerenthesesisCommentRemover : ICommentRemover
    {
        public string RemoveComment(string line) =>
            Regex.Replace(line, "[[].*[]]", string.Empty);
    }
}