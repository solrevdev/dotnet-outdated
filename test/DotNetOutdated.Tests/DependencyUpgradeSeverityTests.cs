using McMaster.Extensions.CommandLineUtils;
using Moq;
using NuGet.Versioning;
using Xunit;
using Xunit.Abstractions;
using DotNetOutdated.Services;

namespace DotNetOutdated.Tests
{
    public class DependencyUpgradeSeverityTests
    {
        private readonly ITestOutputHelper _output;

        public DependencyUpgradeSeverityTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData("1.2.3    ", "2.0.0    ")]
        [InlineData("1.0.13   ", "2.0.1    ")]
        [InlineData("12.15.16 ", "13.0.0   ")]
        public void DependencyUpgradeSeverityForMajorUpgrades(string resolved, string latest)
        {
            var resolvedVersion = new NuGetVersion(resolved);
            var latestVersion = new NuGetVersion(latest);

            var dependency = new Project.Dependency
            {
                ResolvedVersion = resolvedVersion,
                LatestVersion = latestVersion
            };

            Assert.Equal(DependencyUpgradeSeverity.Major, dependency.UpgradeSeverity);
        }

        [Theory]
        [InlineData("1.0.1-al ", "1.0.1-be ")]
        [InlineData("1.0.2-al ", "1.0.2    ")]
        public void DependencyUpgradeSeverityForPrereleaseUpgrades(string resolved, string latest)
        {
            var resolvedVersion = new NuGetVersion(resolved);
            var latestVersion = new NuGetVersion(latest);

            var dependency = new Project.Dependency
            {
                ResolvedVersion = resolvedVersion,
                LatestVersion = latestVersion
            };

            Assert.Equal(DependencyUpgradeSeverity.Major, dependency.UpgradeSeverity);
        }

        [Theory]
        [InlineData("1.2.3    ", "1.3.0    ")]
        [InlineData("1.0.13   ", "1.4.13   ")]
        [InlineData("12.0.16  ", "12.18.0  ")]
        public void DependencyUpgradeSeverityForMinorUpgrades(string resolved, string latest)
        {
            var resolvedVersion = new NuGetVersion(resolved);
            var latestVersion = new NuGetVersion(latest);

            var dependency = new Project.Dependency
            {
                ResolvedVersion = resolvedVersion,
                LatestVersion = latestVersion
            };

            Assert.Equal(DependencyUpgradeSeverity.Minor, dependency.UpgradeSeverity);
        }

        [Theory]
        [InlineData("1.2.3    ", "1.2.4    ")]
        [InlineData("1.0.13   ", "1.0.20   ")]
        [InlineData("12.0.16  ", "12.0.1542")]
        public void DependencyUpgradeSeverityForPatchUpgrades(string resolved, string latest)
        {
            var resolvedVersion = new NuGetVersion(resolved);
            var latestVersion = new NuGetVersion(latest);

            var dependency = new Project.Dependency
            {
                ResolvedVersion = resolvedVersion,
                LatestVersion = latestVersion
            };

            Assert.Equal(DependencyUpgradeSeverity.Patch, dependency.UpgradeSeverity);
        }

        [Theory]
        [InlineData("1.2.3    ", "1.2.3    ")]
        [InlineData("1.0.13   ", "1.0.13   ")]
        [InlineData("12.0.16  ", "12.0.16")]
        public void DependencyUpgradeSeverityForNoUpgrades(string resolved, string latest)
        {
            var resolvedVersion = new NuGetVersion(resolved);
            var latestVersion = new NuGetVersion(latest);

            var dependency = new Project.Dependency
            {
                ResolvedVersion = resolvedVersion,
                LatestVersion = latestVersion
            };

            Assert.Equal(DependencyUpgradeSeverity.None, dependency.UpgradeSeverity);
        }
    }
}
