using Apprenda.SaaSGrid.Extensions;

namespace Apprenda.SaaSGrid.Extensions.NetApp
{
    public class SnapExtension : IDeveloperPortalExtensionService
    {
        // here's the use case
        // when a version is demoted, Apprenda can invoke NetApp to snap data from one volume to another.
        // this will be accomplished by using the application's rest API to get the applications custom properties
        public void OnDemotingVersion(DTO.ReadOnlyVersionDataDTO version, DTO.ApplicationVersionStageDTO proposedStage)
        {
            // there's actually some funky stuff going on here. since we don't
            var application = version.ApplicationAlias;
            // if we are demoting to sandbox - which means its coming from production
            if (proposedStage.Equals(2))
            {
                // we'd snap data from production into a new volume
            }
            if (proposedStage.Equals(1))
            {
                // we could delete a CIFS/NFS share and free up space
            }
        }

        public void OnPromotingVersion(DTO.ReadOnlyVersionDataDTO version, DTO.ApplicationVersionStageDTO proposedStage)
        {
            var application = version.ApplicationAlias;
            // if we are promoting to sandbox - which means its coming from definition
            if (proposedStage.Equals(2))
            {
                // load test data from snapshot into sandbox - especially if we have it from a previous version.
            }
            // if we are promoting to published - which means its coming from sandbox
            if (proposedStage.Equals(3))
            {
                // we might not want to snap data here from sandbox to production
            }
            // if we are promoting to archive - which means its coming from production
            if (proposedStage.Equals(4))
            {
                // create a snapshot that saves the production data for the new version
            }
        }

        public void OnVersionDemoted(DTO.ReadOnlyVersionDataDTO version, DTO.ApplicationVersionStageDTO previousStage, DTO.ApplicationVersionStageDTO proposedStage, DTO.PublishReportCardDataDTO reportCard)
        {
            // here we can run some validation to make sure the snapped / deleted volumes are complete
        }

        public void OnVersionPromoted(DTO.ReadOnlyVersionDataDTO version, DTO.ApplicationVersionStageDTO previousStage, DTO.ApplicationVersionStageDTO proposedStage, DTO.PublishReportCardDataDTO reportCard)
        {
            // here we can run some validation to makre sure the snapped / created volumes are complete
        }
    }
}