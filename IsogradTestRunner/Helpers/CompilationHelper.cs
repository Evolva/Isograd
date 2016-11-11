using System;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace IsogradTestRunner.Helpers
{
    public class CompilationHelper
    {
        private readonly Compilation _compilation;

        public static CompilationHelper For(Compilation compilation)
        {
            return new CompilationHelper(compilation);
        }

        private CompilationHelper(Compilation compilation)
        {
            _compilation = compilation;
        }

        public T TryEmit<T>(Func<Assembly, T> onSuccess, Action<Diagnostic> onErrorAction = null)
        {
            using (var memoryStream = new MemoryStream())
            {
                var emitResult = _compilation.Emit(memoryStream);
                if (emitResult.Success)
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    var assembly = Assembly.Load(memoryStream.ToArray());

                    return onSuccess(assembly);
                }

                if (onErrorAction != null)
                {
                    foreach (var diagnostic in emitResult.Diagnostics)
                    {
                        onErrorAction(diagnostic);
                    }
                }
                return default(T);
            }
        }
    }
}