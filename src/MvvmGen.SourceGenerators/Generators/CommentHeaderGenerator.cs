﻿// ********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in project root
// ********************************************************************

namespace MvvmGen.SourceGenerators.Generators
{
    internal static class CommentHeaderGenerator
    {
        internal static void GenerateCommentHeader(this ViewModelBuilder vmBuilder, string versionString)
        {
            vmBuilder.AppendLine("// <auto-generated>");
            vmBuilder.AppendLine("//   This code was generated for you by");
            vmBuilder.AppendLine("//   ⚡ MvvmGen, a tool created by Thomas Claudius Huber (https://www.thomasclaudiushuber.com)");
            vmBuilder.AppendLine($"//   Generator version: {versionString}");
            vmBuilder.AppendLine("// </auto-generated>");
        }
    }
}
