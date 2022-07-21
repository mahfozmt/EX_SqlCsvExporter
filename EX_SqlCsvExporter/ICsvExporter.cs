using System;
using System.Collections.Generic;
using System.Text;

namespace servicewithtopself
{
    public interface ICsvExporter
    {
        public void Start();
        public void Stop();
    }
}
