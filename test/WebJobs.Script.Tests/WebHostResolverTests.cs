﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs.Script.Config;
using Microsoft.Azure.WebJobs.Script.WebHost;
using Moq;
using Xunit;

namespace Microsoft.Azure.WebJobs.Script.Tests
{
    public class WebHostResolverTests
    {
        [Fact]
        public void GetScriptHostConfiguration_SetsHostId()
        {
            var environmentSettings = new Dictionary<string, string>
            {
                { EnvironmentSettingNames.AzureWebsiteName, "testsite" },
                { EnvironmentSettingNames.AzureWebsiteSlotName, "production" },
            };

            using (var environment = new TestScopedEnvironmentVariable(environmentSettings))
            {
                var settingsManager = new ScriptSettingsManager();
                var secretManagerFactoryMock = new Mock<ISecretManagerFactory>();
                var resolver = new WebHostResolver(settingsManager, secretManagerFactoryMock.Object);

                var settings = new WebHostSettings
                {
                    ScriptPath = @"c:\some\path",
                    LogPath = @"c:\log\path",
                    SecretsPath = @"c:\secrets\path"
                };

                ScriptHostConfiguration configuration = resolver.GetScriptHostConfiguration(settings);

                Assert.Equal("testsite", configuration.HostConfig.HostId);
            }
        }
    }
}
