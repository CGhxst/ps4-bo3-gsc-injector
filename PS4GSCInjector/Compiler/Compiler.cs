using TreyarchCompiler.Games;
using TreyarchCompiler.Utilities;

namespace TreyarchCompiler
{
    public class Compiler
    {
        public static CompiledCode Compile(string code, string path = "")
        {
            return new GSCCompiler(code, path)?.Compile();
        }

        public static CompiledCode CompileT8(string code)
        {
            return new T89Compiler(code)?.Compile();
        }
    }
}
