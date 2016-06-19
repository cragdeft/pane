using AplombTech.WMS.Messages.Commands;
using AplombTech.WMS.Persistence.Repositories;
using AplombTech.WMS.Persistence.UnitOfWorks;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Sensor.Data.Processor
{
    public class SummarGenerator : IHandleMessages<SummaryGenerationMessage>
    {
        public void Handle(SummaryGenerationMessage message)
        {
            using (WMSUnitOfWork uow = new WMSUnitOfWork())
            {
                ProcessRepository repo = new ProcessRepository(WMSUnitOfWork.CurrentObjectContext);
                repo.GenerateSummary(message);
                WMSUnitOfWork.CurrentObjectContext.SaveChanges();
            }
        }
    }
}
