// --------------------------------------------------------------------------------------------------
// <copyright file="HangfireLogJobFilter.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using Hangfire.Client;
using Hangfire.Logging;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;

namespace InmoIT.Shared.Infrastructure.HangfireJobs
{
    public class HangfireLogJobFilter : IClientFilter, IServerFilter, IElectStateFilter, IApplyStateFilter
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        public void OnCreating(CreatingContext context) =>
            Logger.InfoFormat("Creating a job based on method {0}...", context.Job.Method.Name);

        public void OnCreated(CreatedContext context) =>
            Logger.InfoFormat("Job that is based on method {0} has been created with id {1}", context.Job.Method.Name, context.BackgroundJob?.Id);

        public void OnPerforming(PerformingContext context) =>
            Logger.InfoFormat("Starting to perform job {0}", context.BackgroundJob.Id);

        public void OnPerformed(PerformedContext context) =>
            Logger.InfoFormat("Job {0} has been performed", context.BackgroundJob.Id);

        public void OnStateElection(ElectStateContext context)
        {
            if (context.CandidateState is FailedState failedState)
            {
                Logger.WarnFormat("Job '{0}' has been failed due to an exception {1}", context.BackgroundJob.Id, failedState.Exception);
            }
        }

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction) =>
            Logger.InfoFormat("Job {0} state was changed from {1} to {2}", context.BackgroundJob.Id, context.OldStateName, context.NewState.Name);

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction) =>
            Logger.InfoFormat("Job {0} state {1} was unapplied.", context.BackgroundJob.Id, context.OldStateName);
    }
}