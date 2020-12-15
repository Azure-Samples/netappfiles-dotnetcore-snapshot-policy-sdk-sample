// Copyright (c) Microsoft and contributors.  All rights reserved.
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.

namespace Microsoft.Azure.Management.ANF.Samples
{
    using Microsoft.Azure.Management.NetApp;
    using Microsoft.Azure.Management.NetApp.Models;
    using System.Threading.Tasks;

    /// <summary>
    /// Update functionalities for ANF resources
    /// </summary>
    public class Update
    {
        /// <summary>
        /// Update current Snapshot policy hourly schedule
        /// </summary>
        /// <param name="anfClient">ANF client</param>
        /// <param name="resourceGroupName">Resource Group name</param>
        /// <param name="location">Azure location</param>
        /// <param name="accountName">Azure NetApp Files Account name</param>
        /// <param name="snapshotPolicyName">Azure NetApp Files Snapshot Policy name</param>
        /// <param name="currentHourlySchedule">Current Snapshot hourly schedule</param>
        /// <returns>Returns current Snapshot Policy</returns>
        public static async Task<SnapshotPolicy> UpdateANFSnapshotPolicy(AzureNetAppFilesManagementClient anfClient,
            string resourceGroupName,
            string location,
            string accountName,
            string snapshotPolicyName,
            HourlySchedule currentHourlySchedule)
        {
            HourlySchedule newHourlySchedule = currentHourlySchedule;
            newHourlySchedule.SnapshotsToKeep = 10;

            SnapshotPolicyPatch snapshotPolicyPatch = new SnapshotPolicyPatch()
            {
                HourlySchedule = newHourlySchedule,
                Enabled = true,
                Location = location
            };
            return await anfClient.SnapshotPolicies.UpdateAsync(snapshotPolicyPatch, resourceGroupName, accountName, snapshotPolicyName);
        }
    }
}
