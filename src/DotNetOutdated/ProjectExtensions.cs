﻿using System.Collections.Generic;
using System.Linq;
using DotNetOutdated.Services;

namespace DotNetOutdated
{
    public static class ProjectExtensions
    {
        public static List<ConsolidatedPackage> ConsolidatePackages(this List<Project> projects)
        {
            // Get a flattened view of all the outdated packages
            var outdated = from p in projects
                from f in p.TargetFrameworks
                from d in f.Dependencies
                where d.LatestVersion > d.ResolvedVersion
                select new
                {
                    Project = p.Name,
                    ProjectFilePath = p.FilePath,
                    TargetFramework = f.Name,
                    Dependency = d.Name,
                    ResolvedVersion = d.ResolvedVersion,
                    LatestVersion = d.LatestVersion,
                    IsAutoReferenced = d.IsAutoReferenced,
                    IsTransitive = d.IsTransitive,
                    UpgradeSeverity = d.UpgradeSeverity
                };

            // Now group them by package
            var consolidatedPackages = outdated.GroupBy(p => new
                {
                    p.Dependency,
                    p.ResolvedVersion,
                    p.LatestVersion,
                    p.IsTransitive,
                    p.IsAutoReferenced,
                    p.UpgradeSeverity
                })
                .Select(gp => new ConsolidatedPackage
                {
                    Name = gp.Key.Dependency,
                    ResolvedVersion = gp.Key.ResolvedVersion,
                    LatestVersion = gp.Key.LatestVersion,
                    IsTransitive = gp.Key.IsTransitive,
                    IsAutoReferenced = gp.Key.IsAutoReferenced,
                    UpgradeSeverity = gp.Key.UpgradeSeverity,
                    Projects = gp.Select(v => new ConsolidatedPackage.PackageProjectReference
                    {
                        Project = v.Project,
                        ProjectFilePath = v.ProjectFilePath,
                        Framework = v.TargetFramework
                    }).ToList()
                })
                .ToList();

            return consolidatedPackages;
        }
    }
}