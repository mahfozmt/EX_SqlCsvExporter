using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Topshelf;
using Topshelf.Hosts;

namespace servicewithtopself
{
    public class CsvExporterService : ServiceControl
    {        
        ICsvExporter _csvExporter;        
        Thread _ListenerThread;        

        public CsvExporterService(ICsvExporter csvExporter)
        {            
            _csvExporter = csvExporter;

            _ListenerThread = new Thread(_csvExporter.Start);
            _ListenerThread.IsBackground = true;            
        }            

        public bool Start(HostControl hostControl)
        {            
            Utils.LogToFile(1, "[INFO]", "Service Starting");
            _ListenerThread.Start();
            
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            Utils.LogToFile(1, "[INFO]", "Service Stopping");
            return true;
        }

        public bool Pause(HostControl hostControl)
        {
            return true;


        }
        public bool Continue(HostControl hostControl)
        {
            return true;


        }
        public bool Shutdown(HostControl hostControl)
        {
            return true;


        }
    }
}
