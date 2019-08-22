﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using LibGit2Sharp;
using Xunit;

namespace Intellias.CQRS.Tests
{
    /// <summary>
    /// CrossRepoTest.
    /// </summary>
    public class CrossRepoTest
    {
        private const string BasePath = "https://IntelliasTS@dev.azure.com/IntelliasTS/IntelliGrowth/_git/";

        private readonly List<string> sourceFiles;

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossRepoTest"/> class.
        /// </summary>
        public CrossRepoTest()
        {
            var currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var basePath = Path.GetFullPath(Path.Combine(currentPath, @"..\..\..\..\"));
            sourceFiles = GetProjectFiles(basePath);
        }

        /// <summary>
        /// IntelliGrowthCompetenciesTest.
        /// </summary>
        /// <param name="repoName">name of IntelliGrowth repo.</param>
        [Theory]
        [InlineData("IntelliGrowth_JobProfiles")]
        [InlineData("IntelliGrowth_Identity")]
        public void RepoConsistencyTest(string repoName)
        {
            var repoPath = $"repos\\{repoName}";

            CloneRepo(repoName, repoPath);
            var projectFiles = GetProjectFiles(repoPath);

            var solutionFile = Directory.GetFiles(repoPath, "*.sln").Single();

            var projectsToAdd = sourceFiles.Aggregate((i, j) => i + " " + j);
            DotNet($"sln {solutionFile} add {projectsToAdd}");

            foreach (var projectFile in projectFiles)
            {
                var packages = GetPackages(projectFile, "Intellias.CQRS.");
                if (packages.Any())
                {
                    var packagesToRemove = packages.Aggregate((i, j) => i + " " + j);
                    DotNet($"remove {projectFile} reference {packagesToRemove}");

                    var projectsRefsToAdd = packages.Select(p => sourceFiles.Single(f => f.Contains($"{p}.csproj", StringComparison.InvariantCultureIgnoreCase))).Aggregate((i, j) => i + " " + j);
                    DotNet($"add {projectFile} reference {projectsRefsToAdd}");
                }
            }

            DotNet($"build {solutionFile}");

            DeleteDirectory(repoPath);
        }

        private static void DotNet(string args)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = "dotnet";
                process.StartInfo.Arguments = args;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
                {
                    Trace.WriteLine(e.Data);
                });
                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
                Assert.Equal(0, process.ExitCode);
            }
        }

        private static List<string> GetPackages(string projectFile, string startWith)
        {
            var packages = new List<string>();
            var doc = new XmlDocument();
            doc.Load(projectFile);

            var packageReferences = doc.GetElementsByTagName("PackageReference").Cast<XmlNode>().ToList();
            foreach (var packageReference in packageReferences)
            {
                var packageName = packageReference.Attributes["Include"].Value;
                if (packageName.StartsWith(startWith, StringComparison.InvariantCultureIgnoreCase))
                {
                    packages.Add(packageName);
                }
            }

            return packages;
        }

        private static List<string> GetProjectFiles(string path)
        {
            var dirs = Directory.GetDirectories(path).Select(x => Path.GetFullPath(x));
            return dirs.SelectMany(dir => Directory.GetFiles(dir, "*.csproj")).ToList();
        }

        private static void DeleteDirectory(string directory)
        {
            var files = Directory.GetFiles(directory);
            var dirs = Directory.GetDirectories(directory);

            foreach (var file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (var dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(directory, false);
        }

        private static void CloneRepo(string name, string repoPath)
        {
            var accessToken = Environment.GetEnvironmentVariable("SYSTEM_ACCESSTOKEN") ?? "hanfpowgg33ge5sxq4txa3bhwhb5dxc4z2qcf22jndhx3nz2kpkq";

            var co = new CloneOptions
            {
                CredentialsProvider = (url, user, cred) => new UsernamePasswordCredentials
                {
                    Username = accessToken,
                    Password = accessToken
                }
            };

            var ro = new RepositoryOptions
            {
                Identity = new Identity(accessToken, "sseletskyi@intellias.com"),
                WorkingDirectoryPath = repoPath
            };

            Directory.CreateDirectory(repoPath);
            var repoLink = Repository.Clone($"{BasePath}{name}", repoPath, co);

            using (var repo = new Repository(repoLink, ro))
            {
                var master = repo.Branches["master"];
                Commands.Checkout(repo, master);
            }
        }
    }
}