// Copyright (c) Microsoft and contributors.  All rights reserved.
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.

namespace Microsoft.Azure.Management.ANF.Samples
{
    using Microsoft.Azure.Management.NetApp;
    using Microsoft.Azure.Management.NetApp.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Includes Creation functionality for ANF resources
    /// </summary>
    public class Creation
    {
        /// <summary>
        /// Creates or Updates Azure NetApp Files Account
        /// </summary>
        /// <param name="anfClient">ANF client object</param>
        /// <param name="resourceGroupName">Resource group name</param>
        /// <param name="location">Azure location</param>
        /// <param name="accountName">Azure NetApp Files Account name</param>
        /// <returns>NetApp Account object</returns>
        public static async Task<NetAppAccount> CreateOrUpdateANFAccountAsync(AzureNetAppFilesManagementClient anfClient, 
            string resourceGroupName, 
            string location, 
            string accountName)
        {
           NetAppAccount anfAccountBody = new NetAppAccount(location, null, accountName);
           return await anfClient.Accounts.CreateOrUpdateAsync(anfAccountBody, resourceGroupName, accountName);           
        }

        /// <summary>
        /// Create Azure NetApp Files Snapshot Policy 
        /// </summary>
        /// <param name="anfClient">ANF client</param>
        /// <param name="resourceGroupName">Resource Group name</param>
        /// <param name="accountName">Azure NetApp Files</param>
        /// <param name="snapshotPolicyName"></param>
        /// <returns></returns>
        public static async Task<SnapshotPolicy> CreateANFSnapshotPolicy(AzureNetAppFilesManagementClient anfClient,
            string resourceGroupName,
            string location,
            string accountName,
            string snapshotPolicyName)
        {
            SnapshotPolicy snapshotPolicyBody = new SnapshotPolicy()
            {
                Location = location,
                Enabled = true,
                HourlySchedule = new HourlySchedule() 
                {
                    SnapshotsToKeep = 5,
                    Minute = 50                     
                },

                DailySchedule = new DailySchedule() 
                { 
                    SnapshotsToKeep = 5, 
                    Hour = 15, 
                    Minute = 30 
                },

                WeeklySchedule = new WeeklySchedule() 
                { 
                    SnapshotsToKeep = 5, 
                    Day = "Monday", 
                    Hour = 12, 
                    Minute = 30 
                },

                MonthlySchedule = new MonthlySchedule() 
                { 
                    SnapshotsToKeep = 5, 
                    DaysOfMonth = "10,11,12", 
                    Hour = 14, 
                    Minute = 50 
                }
            };
            return await anfClient.SnapshotPolicies.CreateAsync(snapshotPolicyBody, resourceGroupName,accountName,snapshotPolicyName);
        }        

        /// <summary>
        /// Creates or Updates Azure NetApp Files Capacity Pool
        /// </summary>
        /// <param name="anfClient">ANF client object</param>
        /// <param name="resourceGroupName">Resource group name</param>
        /// <param name="location">Azure location</param>
        /// <param name="accountName">Azure NetApp Files Account name</param>
        /// <param name="poolName">Azure NetApp Files Capacity Pool name</param>
        /// <param name="poolSize">Azure NetApp Files Capacity Pool size</param>
        /// <param name="serviceLevel">Service Level</param>
        /// <returns>Azure NetApp Files Capacity Pool</returns>
        public static async Task<CapacityPool> CreateOrUpdateANFCapacityPoolAsync(AzureNetAppFilesManagementClient anfClient, 
            string resourceGroupName, 
            string location, 
            string accountName, 
            string poolName, 
            long poolSize, 
            string serviceLevel)
        {
            CapacityPool primaryCapacityPoolBody = new CapacityPool()
            {
                Location = location.ToLower(), // Important: location needs to be lower case
                ServiceLevel = serviceLevel, //Service level can be one of three levels -> { Standard, Premium, Ultra }
                Size = poolSize
            };
            return await anfClient.Pools.CreateOrUpdateAsync(primaryCapacityPoolBody, resourceGroupName, accountName, poolName);
        }

        /// <summary>
        /// Creates Or Updates Azure NetApp Files Volume
        /// </summary>
        /// <param name="anfClient">ANF client object</param>
        /// <param name="resourceGroupName">Resource group name</param>
        /// <param name="location">Azure location</param>
        /// <param name="accountName">Azure NetApp Files Account name</param>
        /// <param name="poolName">Azure NetApp Files Capacity Pool name</param>
        /// <param name="serviceLevel">Service Level</param>
        /// <param name="volumeName">Azure NetApp Files Volume name</param>
        /// <param name="subnetId">Subnet Id</param>
        /// <param name="volumeSize">Azure NetApp Files Volume size</param>
        /// <param name="snapshotPolicyId">Existing Snapshot Policy Id</param>
        /// <returns>Azure NetApp Files Volume</returns>
        public static async Task<Volume> CreateOrUpdateANFVolumeAsync(AzureNetAppFilesManagementClient anfClient, 
            string resourceGroupName, 
            string location, 
            string accountName, 
            string poolName, 
            string serviceLevel, 
            string volumeName, 
            string subnetId, 
            long volumeSize,
            string snapshotPolicyId)
        {

            VolumeSnapshotProperties snapshotProperties = new VolumeSnapshotProperties()
            {
                SnapshotPolicyId = snapshotPolicyId
            };

            VolumePropertiesDataProtection dataProtectionProperties = new VolumePropertiesDataProtection()
            {
                Snapshot = snapshotProperties
            };

            Volume volumeBody = new Volume()
            {
                Location = location.ToLower(),
                ServiceLevel = serviceLevel, //Service level can be one of three levels -> { Standard, Premium, Ultra }
                CreationToken = volumeName,
                SubnetId = subnetId,
                UsageThreshold = volumeSize,
                DataProtection = dataProtectionProperties,
                ProtocolTypes = new List<string>() { "NFSv3" }
            };

            return await anfClient.Volumes.CreateOrUpdateAsync(volumeBody, resourceGroupName, accountName, poolName, volumeName);
        }
    }
}
